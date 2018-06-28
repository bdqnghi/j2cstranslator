package com.ilog.translator.java2cs.translation.astrewriter;

import org.eclipse.jdt.core.dom.AST;
import org.eclipse.jdt.core.dom.ASTNode;
import org.eclipse.jdt.core.dom.CastExpression;
import org.eclipse.jdt.core.dom.ConditionalExpression;
import org.eclipse.jdt.core.dom.Expression;
import org.eclipse.jdt.core.dom.IMethodBinding;
import org.eclipse.jdt.core.dom.ITypeBinding;
import org.eclipse.jdt.core.dom.MethodInvocation;
import org.eclipse.jdt.core.dom.ParenthesizedExpression;
import org.eclipse.jdt.core.dom.PrimitiveType;
import org.eclipse.jdt.core.dom.SwitchStatement;
import org.eclipse.jdt.core.dom.Type;
import org.eclipse.jdt.core.dom.VariableDeclarationFragment;
import org.eclipse.jdt.internal.corext.dom.ASTNodes;
import org.eclipse.text.edits.TextEditGroup;

import com.ilog.translator.java2cs.translation.ITranslationContext;
import com.ilog.translator.java2cs.translation.util.TranslationUtils;


// - change char to int : 
//     * in switch statement : In C# we can't use 'char' as type for a switch
//     * in variable declaration
// - cast then and else part of the conditionalexpression "?" in order to have same type.
//
public class ChangeSwitchCharToIntVisitor extends ASTRewriterVisitor {

	//
	//
	//

	public ChangeSwitchCharToIntVisitor(ITranslationContext context) {
		super(context);
		transformerName = "Change Char into Int in switch";
		description = new TextEditGroup(transformerName);
	}

	//
	//
	//

	@Override
	public void endVisit(VariableDeclarationFragment node) {
		final Expression initializer = node.getInitializer();
		if (initializer != null) {
			final Type type = ASTNodes.getType(node);
			if (type.isPrimitiveType()) {
				final PrimitiveType pType = (PrimitiveType) type;
				if (pType.getPrimitiveTypeCode() == PrimitiveType.CHAR) {
					if (initializer.resolveTypeBinding().isPrimitive()) {
						if (TranslationUtils.isIntType(initializer
								.resolveTypeBinding())) {
							currentRewriter.replace(initializer, cast(
									currentRewriter.getAST(), initializer,
									pType.resolveBinding()), description);
						}
					}
				}
			}
		}
	}

	@Override
	public void endVisit(SwitchStatement node) {
		final Expression expr = node.getExpression();
		final ITypeBinding itype = expr.resolveTypeBinding();
		if ((itype != null) && itype.isPrimitive()) {
			if (TranslationUtils.isCharType(itype)) {
				final AST ast = node.getAST();
				final Expression newexpr = (Expression) ASTNode.copySubtree(
						ast, expr);
				final CastExpression cExpr = ast.newCastExpression();
				final Type ptype = ast.newPrimitiveType(PrimitiveType.INT);
				cExpr.setExpression(newexpr);
				cExpr.setType(ptype);
				currentRewriter.replace(expr, cExpr, description);
			}
		}
	}

	@Override
	public void endVisit(ConditionalExpression node) {
		final ITypeBinding tbinding = getTypeOfExpectedNode(node); // node.resolveTypeBinding();
		final Expression thenE = node.getThenExpression();
		final Expression elseE = node.getElseExpression();
		final Expression test = node.getExpression();

		// VS2008 new check ....
		if (test.getNodeType() != ASTNode.PARENTHESIZED_EXPRESSION) {
			final ParenthesizedExpression replacement = currentRewriter
					.getAST().newParenthesizedExpression();
			replacement.setExpression((Expression) currentRewriter
					.createMoveTarget(test));
			currentRewriter.replace(test, replacement, description);
		}

		// Limitation of C# : In conditionnalExpression (the short if with ? and
		// :)
		// then and else part MUST have the "same" type.
		if (tbinding != null
				&& !thenE.resolveTypeBinding().isEqualTo(
						elseE.resolveTypeBinding())) {
			if (thenE.getNodeType() == ASTNode.NULL_LITERAL
					|| elseE.getNodeType() == ASTNode.NULL_LITERAL)
				return;
			final AST ast = node.getAST();
			// ImportRewriteUtil.addImports(rewrite, node, typeImports,
			// staticImports, declarations);
			currentRewriter.replace(thenE, cast(ast, thenE, tbinding),
					description);
			currentRewriter.replace(elseE, cast(ast, elseE, tbinding),
					description);
		}
	}

	private ITypeBinding getTypeOfExpectedNode(ConditionalExpression node) {
		if (node.getParent().getNodeType() == ASTNode.METHOD_INVOCATION) {
			final MethodInvocation method = (MethodInvocation) node.getParent();
			final int index = findIndex(method, node);
			final IMethodBinding mBind = method.resolveMethodBinding();
			if (mBind.isVarargs()) {
				final int nbParam = mBind.getParameterTypes().length;
				final ITypeBinding arrayType = mBind.getParameterTypes()[nbParam - 1];
				return arrayType.getElementType();
			} else {
				return mBind.getParameterTypes()[index];
			}
		}
		return node.resolveTypeBinding();
	}

	private int findIndex(MethodInvocation method, ASTNode node) {
		int i = 0;
		for (final Object arg : method.arguments()) {
			if (((ASTNode) arg) == node)
				return i;
			i++;
		}
		return -1;
	}

	private ASTNode cast(AST ast, Expression thenE, ITypeBinding tbinding) {
		final CastExpression cast = ast.newCastExpression();
		final ParenthesizedExpression parentExpr = ast
				.newParenthesizedExpression();
		parentExpr.setExpression((Expression) ASTNode.copySubtree(ast, thenE));
		cast.setExpression(parentExpr);
		String name = null;
		if (tbinding.isWildcardType()) {
			final ITypeBinding bound = tbinding.getBound();
			name = TranslationUtils.getFQNNameWithGenerics(bound);
		} else {
			name = TranslationUtils.getFQNNameWithGenerics(tbinding);
		}
		if (tbinding.isPrimitive()) {
			PrimitiveType.Code typeCode = null;
			if (name.equals(TranslationUtils.INT)) {
				typeCode = PrimitiveType.INT;
			} else if (name.equals(TranslationUtils.LONG)) {
				typeCode = PrimitiveType.LONG;
			} else if (name.equals(TranslationUtils.SHORT)) {
				typeCode = PrimitiveType.SHORT;
			} else if (name.equals(TranslationUtils.FLOAT)) {
				typeCode = PrimitiveType.FLOAT;
			} else if (name.equals(TranslationUtils.DOUBLE)) {
				typeCode = PrimitiveType.DOUBLE;
			} else if (name.equals(TranslationUtils.BOOLEAN)) {
				typeCode = PrimitiveType.BOOLEAN;
			} else if (name.equals(TranslationUtils.BYTE)) {
				typeCode = PrimitiveType.BYTE;
			} else if (name.equals(TranslationUtils.CHAR)) {
				typeCode = PrimitiveType.CHAR;
			}
			final Type type = ast.newPrimitiveType(typeCode);
			cast.setType(type);
			return cast;
		} else {
			final Type type = (Type) currentRewriter.createStringPlaceholder(
					name, ASTNode.SIMPLE_TYPE);
			cast.setType(type);
			return cast;
		}
	}

}
