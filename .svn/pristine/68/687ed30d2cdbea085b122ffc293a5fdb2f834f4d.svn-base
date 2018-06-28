package com.ilog.translator.java2cs.translation.astrewriter;

import java.util.List;

import org.eclipse.jdt.core.dom.ASTNode;
import org.eclipse.jdt.core.dom.Assignment;
import org.eclipse.jdt.core.dom.EnumDeclaration;
import org.eclipse.jdt.core.dom.Expression;
import org.eclipse.jdt.core.dom.FieldAccess;
import org.eclipse.jdt.core.dom.IBinding;
import org.eclipse.jdt.core.dom.IMethodBinding;
import org.eclipse.jdt.core.dom.ITypeBinding;
import org.eclipse.jdt.core.dom.MethodInvocation;
import org.eclipse.jdt.core.dom.Name;
import org.eclipse.jdt.core.dom.SimpleName;
import org.eclipse.jdt.core.dom.ThisExpression;
import org.eclipse.jdt.core.dom.TypeDeclaration;
import org.eclipse.jdt.internal.corext.dom.ASTNodes;
import org.eclipse.text.edits.TextEditGroup;

import com.ilog.translator.java2cs.translation.ITranslationContext;

/**
 * 
 * @author afau
 */
public class ThisRemoverVisitor extends ASTRewriterVisitor {

	//
	//
	//

	public ThisRemoverVisitor(ITranslationContext context) {
		super(context);
		transformerName = "This Remover";
		description = new TextEditGroup(transformerName);
	}

	//
	//
	//

	@Override
	public void endVisit(FieldAccess node) {
		final Expression expr = node.getExpression();
		if (expr != null && expr.getNodeType() == ASTNode.THIS_EXPRESSION) {
			TypeDeclaration enclosingType = (TypeDeclaration) ASTNodes.getParent(
					node, ASTNode.TYPE_DECLARATION);
			// if parent is assignement and rightside is simplename with same
			// name do nothing
			if (enclosingType != null && !isAmbigousChange(node)) {
				boolean done = removeThis(node, expr, enclosingType);
				if (!done) {
					enclosingType = (TypeDeclaration) ASTNodes.getParent(enclosingType,
							ASTNode.TYPE_DECLARATION);
					if (enclosingType != null)
						removeThis(node, expr, enclosingType);
				}
			}
			//
			EnumDeclaration enclosingEnum = (EnumDeclaration) ASTNodes.getParent(
					node, ASTNode.ENUM_DECLARATION);
			// if parent is assignement and rightside is simplename with same
			// name do nothing
			if (enclosingEnum != null && !isAmbigousChange(node)) {
				boolean done = removeThis(node, expr, enclosingEnum);
				// Enum in enum possible ?
				/*if (!done) {
					enclosingEnum = (TypeDeclaration) ASTNodes.getParent(enclosingEnum,
							ASTNode.TYPE_DECLARATION);
					if (enclosingType != null)
						removeThis(node, expr, enclosingType);
				}*/
			}
		}
	}

	private boolean isAmbigousChange(FieldAccess node) {
		final ASTNode parent = node.getParent();
		if (parent.getNodeType() == ASTNode.ASSIGNMENT) {
			final Assignment assign = (Assignment) parent;
			final Expression rightHandSide = assign.getRightHandSide();
			if (rightHandSide != null
					&& rightHandSide.getNodeType() == ASTNode.SIMPLE_NAME) {
				final SimpleName sName = (SimpleName) rightHandSide;
				if (sName.getIdentifier()
						.equals(node.getName().getIdentifier()))
					return true;
			}
		}
		return false;
	}

	@Override
	public void endVisit(MethodInvocation node) {
		final Expression expr = node.getExpression();
		if (expr != null && expr.getNodeType() == ASTNode.THIS_EXPRESSION) {
			final TypeDeclaration enclosingtype = (TypeDeclaration) ASTNodes
					.getParent(node, ASTNode.TYPE_DECLARATION);
			if (enclosingtype != null) {
					final boolean done = removeThis(node, expr, enclosingtype);
			}
			// only want to remove at one level
			// Other cases must be handled by move inner to top ?
			/*
			 * if (!done) { enclosing = (TypeDeclaration)
			 * ASTNodes.getParent(enclosing, ASTNode.TYPE_DECLARATION); if
			 * (enclosing != null) removeThis(node, expr, enclosing); }
			 */
			EnumDeclaration enclosingEnum = (EnumDeclaration) ASTNodes.getParent(
					node, ASTNode.ENUM_DECLARATION);
			if (enclosingEnum != null) {
				boolean done = removeThis(node, expr, enclosingEnum);
			}
		}
	}

	//
	//
	//
	
	private boolean removeThis(FieldAccess node, Expression expr,
			TypeDeclaration enclosing) {
		final ThisExpression thisExpr = (ThisExpression) expr;
		final Name thisQualifier = thisExpr.getQualifier();
		if (thisQualifier != null) {
			final IBinding thisQBinding = thisQualifier.resolveBinding();
			if (enclosing != null) {
				final ITypeBinding eTb = enclosing.resolveBinding();
				final ITypeBinding tb = (ITypeBinding) thisQBinding;
				if (eTb.getErasure().isEqualTo(tb.getErasure())) {
					currentRewriter.replace(node, node.getName(), description);
					return true;
				}
			}
		}
		return false;
	}
	
	private boolean removeThis(FieldAccess node, Expression expr,
			EnumDeclaration enclosing) {
		final ThisExpression thisExpr = (ThisExpression) expr;
		final Name thisQualifier = thisExpr.getQualifier();
		if (thisQualifier != null) {
			final IBinding thisQBinding = thisQualifier.resolveBinding();
			if (enclosing != null) {
				final ITypeBinding eTb = enclosing.resolveBinding();
				final ITypeBinding tb = (ITypeBinding) thisQBinding;
				if (eTb.getErasure().isEqualTo(tb.getErasure())) {
					currentRewriter.replace(node, node.getName(), description);
					return true;
				}
			}
		}
		return false;
	}

	@SuppressWarnings("unchecked")
	private boolean removeThis(MethodInvocation node, Expression expr,
			TypeDeclaration enclosing) {
		final ThisExpression thisExpr = (ThisExpression) expr;
		final Name thisQualifier = thisExpr.getQualifier();
		if (thisQualifier != null) {
			final IBinding thisQBinding = thisQualifier.resolveBinding();
			if (enclosing != null) {
				final ITypeBinding eTb = enclosing.resolveBinding();
				final ITypeBinding tb = (ITypeBinding) thisQBinding;
				if (eTb.getErasure().isEqualTo(tb.getErasure())) {
					// Ob but what to do of the method exist both in enclosing
					// and in the type that contains
					// the XXX.this call ?
					if (!methodExists(node, tb)) {
						final MethodInvocation newInvok = currentRewriter
								.getAST().newMethodInvocation();
						final SimpleName name = (SimpleName) ASTNode
								.copySubtree(currentRewriter.getAST(), node
										.getName());
						final List arguments = ASTNode.copySubtrees(
								currentRewriter.getAST(), node.arguments());
						final List typeArguments = ASTNode.copySubtrees(
								currentRewriter.getAST(), node.typeArguments());
						newInvok.setName(name);
						newInvok.arguments().addAll(arguments);
						newInvok.typeArguments().addAll(typeArguments);
						currentRewriter.replace(node, newInvok, description);
						return true;
					} else
						return false;
				}
			}
		}
		return false;
	}
	
	@SuppressWarnings("unchecked")
	private boolean removeThis(MethodInvocation node, Expression expr,
			EnumDeclaration enclosing) {
		final ThisExpression thisExpr = (ThisExpression) expr;
		final Name thisQualifier = thisExpr.getQualifier();
		if (thisQualifier != null) {
			final IBinding thisQBinding = thisQualifier.resolveBinding();
			if (enclosing != null) {
				final ITypeBinding eTb = enclosing.resolveBinding();
				final ITypeBinding tb = (ITypeBinding) thisQBinding;
				if (eTb.getErasure().isEqualTo(tb.getErasure())) {
					// Ob but what to do of the method exist both in enclosing
					// and in the type that contains
					// the XXX.this call ?
					if (!methodExists(node, tb)) {
						final MethodInvocation newInvok = currentRewriter
								.getAST().newMethodInvocation();
						final SimpleName name = (SimpleName) ASTNode
								.copySubtree(currentRewriter.getAST(), node
										.getName());
						final List arguments = ASTNode.copySubtrees(
								currentRewriter.getAST(), node.arguments());
						final List typeArguments = ASTNode.copySubtrees(
								currentRewriter.getAST(), node.typeArguments());
						newInvok.setName(name);
						newInvok.arguments().addAll(arguments);
						newInvok.typeArguments().addAll(typeArguments);
						currentRewriter.replace(node, newInvok, description);
						return true;
					} else
						return false;
				}
			}
		}
		return false;
	}

	private boolean methodExists(MethodInvocation node,
			ITypeBinding enclosingType) {
		final ITypeBinding typeThatContainsMethodCall = ASTNodes
				.getEnclosingType(node);
		return (typeHierarchyContainsMethod(typeThatContainsMethodCall, node) && typeHierarchyContainsMethod(
				enclosingType, node));
	}

	private boolean typeHierarchyContainsMethod(
			ITypeBinding typeThatContainsMethodCall, MethodInvocation node) {
		if (typeThatContainsMethodCall == null)
			return false;
		final IMethodBinding model = node.resolveMethodBinding();
		final IMethodBinding[] methods = typeThatContainsMethodCall
				.getDeclaredMethods();
		for (final IMethodBinding currentMethod : methods) {
			if (model.getName().equals(currentMethod.getName())) {
				return true;
			}
		}
		return typeHierarchyContainsMethod(typeThatContainsMethodCall
				.getSuperclass(), node);
	}
}
