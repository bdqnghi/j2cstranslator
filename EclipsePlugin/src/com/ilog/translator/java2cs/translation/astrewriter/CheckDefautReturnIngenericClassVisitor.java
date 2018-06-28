package com.ilog.translator.java2cs.translation.astrewriter;

import org.eclipse.jdt.core.dom.ASTNode;
import org.eclipse.jdt.core.dom.Assignment;
import org.eclipse.jdt.core.dom.ConditionalExpression;
import org.eclipse.jdt.core.dom.ConstructorInvocation;
import org.eclipse.jdt.core.dom.Expression;
import org.eclipse.jdt.core.dom.IMethodBinding;
import org.eclipse.jdt.core.dom.ITypeBinding;
import org.eclipse.jdt.core.dom.IVariableBinding;
import org.eclipse.jdt.core.dom.MethodDeclaration;
import org.eclipse.jdt.core.dom.ParenthesizedExpression;
import org.eclipse.jdt.core.dom.ReturnStatement;
import org.eclipse.jdt.core.dom.TypeDeclaration;
import org.eclipse.jdt.core.dom.VariableDeclarationFragment;
import org.eclipse.jdt.internal.corext.dom.ASTNodes;
import org.eclipse.text.edits.TextEditGroup;

import com.ilog.translator.java2cs.translation.ITranslationContext;

/**
 * Replace 'return null' by 'return default(XX)' for generic type.
 * 
 * 
 * @author afau
 *
 */
public class CheckDefautReturnIngenericClassVisitor extends ASTRewriterVisitor {

	//
	//
	//

	private ITypeBinding currentReturnType;

	public CheckDefautReturnIngenericClassVisitor(ITranslationContext context) {
		super(context);
		transformerName = "Check Default value for type variable and enum";
		description = new TextEditGroup(transformerName);
	}

	//
	//
	//	

	@Override
	public boolean visit(Assignment node) {
		final ITypeBinding returnType = node.resolveTypeBinding();
		if (returnType.isTypeVariable() || returnType.isEnum()) {
			currentReturnType = returnType;
			replaceNullLiteral(node.getRightHandSide());
		}
		return true;
	}
	
	@Override
	public boolean visit(VariableDeclarationFragment node) {
		if (node.getInitializer() != null) {
			if (node.getInitializer().getNodeType() == ASTNode.NULL_LITERAL) {
				final IVariableBinding variableBinding = node.resolveBinding();
				final ITypeBinding returnType = variableBinding.getType();
				if (returnType.isTypeVariable() || returnType.isEnum() ) {
					currentReturnType = returnType;
					replaceNullLiteral(node.getInitializer());
				}
			}
		}
		return true;
	}

	@Override
	public boolean visit(MethodDeclaration node) {
		final IMethodBinding mb = node.resolveBinding();
		if (mb == null) {
			context.getLogger().logError(
					node + " has no binding ! " + fCu.getElementName());
		}
		final ITypeBinding returnType = mb.getReturnType();
		//
		if (returnType.isTypeVariable() || returnType.isEnum()) {
			currentReturnType = returnType;
			return true;
		}
		if (node.isConstructor()) {
			return true;
		}
		return true;
	}

	@Override
	public void endVisit(MethodDeclaration node) {
		currentReturnType = null;
	}
	
	@Override
	public boolean visit(TypeDeclaration node) {
		currentReturnType = null;
		return true;
	}
	
	@Override
	public void endVisit(ReturnStatement node) {
		if (node.getExpression() != null) {			
			if (currentReturnType != null) {
				MethodDeclaration parent = (MethodDeclaration) ASTNodes.getParent(node, ASTNode.METHOD_DECLARATION);
				if (parent != null) {
					final IMethodBinding mb = parent.resolveBinding();
					if (mb == null) {
						context.getLogger().logError(
								parent + " has no binding ! " + fCu.getElementName());
					}
					final ITypeBinding returnType = mb.getReturnType();
					//
					if (returnType.isTypeVariable() || returnType.isEnum()) {
						replaceNullLiteral(node.getExpression());
					}
				}			
			}
		}
	}

	@Override
	public boolean visit(ConstructorInvocation node) {
		final IMethodBinding mb = node.resolveConstructorBinding();
		if (mb == null) {
			context.getLogger().logError(
					node + " has no binding ! " + fCu.getElementName());
		}
		final ITypeBinding[] paramTypes = mb.getParameterTypes();
		for (int i = 0; i < paramTypes.length; i++) {
			final ITypeBinding type = paramTypes[i];
			if (type.isTypeVariable() || type.isEnum() ) {
				currentReturnType = type;
				replaceNullLiteral((Expression) node.arguments().get(i));
			}
		}
		return false;
	}

	//
	//
	//

	private void replaceNullLiteral(Expression node) {
		switch (node.getNodeType()) {
		case ASTNode.NULL_LITERAL:
			addDefautComment(node);
			break;
		case ASTNode.PARENTHESIZED_EXPRESSION:
			final ParenthesizedExpression pExpr = (ParenthesizedExpression) node;
			replaceNullLiteral(pExpr.getExpression());
			break;
		case ASTNode.CONDITIONAL_EXPRESSION:
			final ConditionalExpression cExpr = (ConditionalExpression) node;
			replaceNullLiteral(cExpr.getThenExpression());
			replaceNullLiteral(cExpr.getElseExpression());
		default:
		}
	}

	private void addDefautComment(Expression node) {
		final ASTNode replacement = currentRewriter.createStringPlaceholder(
				" /* default(" + currentReturnType.getName() + ") */ "
						+ ASTNodes.asString(node), node.getNodeType());
		currentRewriter.replace(node, replacement, description);
	}
}
