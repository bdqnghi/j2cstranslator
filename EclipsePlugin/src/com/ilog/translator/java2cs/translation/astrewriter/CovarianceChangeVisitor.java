package com.ilog.translator.java2cs.translation.astrewriter;

import java.util.List;

import org.eclipse.jdt.core.dom.AST;
import org.eclipse.jdt.core.dom.ASTNode;
import org.eclipse.jdt.core.dom.CastExpression;
import org.eclipse.jdt.core.dom.CompilationUnit;
import org.eclipse.jdt.core.dom.Expression;
import org.eclipse.jdt.core.dom.MethodDeclaration;
import org.eclipse.jdt.core.dom.MethodInvocation;
import org.eclipse.jdt.core.dom.ParenthesizedExpression;
import org.eclipse.jdt.core.dom.SuperMethodInvocation;
import org.eclipse.jdt.core.dom.TypeDeclaration;
import org.eclipse.jdt.core.search.SearchMatch;
import org.eclipse.jdt.internal.corext.refactoring.structure.ASTNodeSearchUtil;
import org.eclipse.text.edits.TextEditGroup;

import com.ilog.translator.java2cs.translation.CovarianceData;
import com.ilog.translator.java2cs.translation.ITranslationContext;
import com.ilog.translator.java2cs.translation.util.TranslationUtils;
import com.ilog.translator.java2cs.util.TranslationModelUtil;

/**
 * Covariance change
 * 
 * @author afau
 *
 */
public class CovarianceChangeVisitor extends ASTRewriterVisitor {

	//
	//
	//

	public CovarianceChangeVisitor(ITranslationContext context) {
		super(context);
		transformerName = "Change Covariance";
		description = new TextEditGroup(transformerName);
	}

	//
	//
	//	

	@Override
	public boolean visit(MethodDeclaration node) {
		final String newReturnTypeName = TranslationUtils.getTagValueFromDoc(
				node, context.getMapper().getTag(
						TranslationModelUtil.COVARIANCE_TAG));
		if (newReturnTypeName != null) {

			final ASTNode replacement = currentRewriter
					.createStringPlaceholder(newReturnTypeName, node
							.getReturnType2().getNodeType());
			currentRewriter.replace(node.getReturnType2(), replacement,
					description);
		}
		//
		return true;
	}

	@Override
	public boolean visit(TypeDeclaration typeNode) {
		final List<CovarianceData> ref = context.getCovariance(fCu);

		if (ref != null) {
			for (final CovarianceData data : ref) {
				final SearchMatch match = data.searchMatchs;
				final ASTNode astNode = ASTNodeSearchUtil.findNode(match,
						(CompilationUnit) typeNode.getRoot());
				if (astNode != null) {
				switch (astNode.getNodeType()) {
				case ASTNode.METHOD_INVOCATION:
					final MethodInvocation node = (MethodInvocation) astNode;
					//
					final AST ast = node.getAST();
					final ParenthesizedExpression replacement = ast
							.newParenthesizedExpression();
					final CastExpression cast = ast.newCastExpression();
					cast.setType(ast
							.newSimpleType(ast.newName(data.returnType)));
					cast.setExpression((Expression) currentRewriter
							.createCopyTarget(node));
					replacement.setExpression(cast);
					currentRewriter.replace(node, replacement, description);
					break;
				case ASTNode.SUPER_METHOD_INVOCATION:
					final SuperMethodInvocation node2 = (SuperMethodInvocation) astNode;
					//
					final AST ast2 = node2.getAST();
					final ParenthesizedExpression replacement2 = ast2
							.newParenthesizedExpression();
					final CastExpression cast2 = ast2.newCastExpression();
					cast2.setType(ast2.newSimpleType(ast2
							.newName(data.returnType)));
					cast2.setExpression((Expression) ASTNode.copySubtree(ast2,
							node2));
					replacement2.setExpression(cast2);
					currentRewriter.replace(node2, replacement2, description);
					break;
				default:
					context.getLogger().logInfo(
							"Change Unexpected node type " + astNode);
				}
			}
			}
		}
		return true;
	}
}
