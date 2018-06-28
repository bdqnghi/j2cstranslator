/**
 * 
 */
package com.ilog.translator.java2cs.translation.astrewriter.astchange;

import java.util.ArrayList;
import java.util.List;

import org.eclipse.core.runtime.NullProgressMonitor;
import org.eclipse.jdt.core.Flags;
import org.eclipse.jdt.core.IType;
import org.eclipse.jdt.core.ITypeHierarchy;
import org.eclipse.jdt.core.JavaModelException;
import org.eclipse.jdt.core.dom.ASTNode;
import org.eclipse.jdt.core.dom.ASTVisitor;
import org.eclipse.jdt.core.dom.AbstractTypeDeclaration;
import org.eclipse.jdt.core.dom.CastExpression;
import org.eclipse.jdt.core.dom.ClassInstanceCreation;
import org.eclipse.jdt.core.dom.Expression;
import org.eclipse.jdt.core.dom.FieldAccess;
import org.eclipse.jdt.core.dom.IBinding;
import org.eclipse.jdt.core.dom.IMethodBinding;
import org.eclipse.jdt.core.dom.ITypeBinding;
import org.eclipse.jdt.core.dom.IVariableBinding;
import org.eclipse.jdt.core.dom.MethodInvocation;
import org.eclipse.jdt.core.dom.Modifier;
import org.eclipse.jdt.core.dom.Name;
import org.eclipse.jdt.core.dom.ParenthesizedExpression;
import org.eclipse.jdt.core.dom.QualifiedName;
import org.eclipse.jdt.core.dom.SimpleName;
import org.eclipse.jdt.core.dom.ThisExpression;
import org.eclipse.jdt.core.dom.Type;
import org.eclipse.jdt.internal.corext.dom.ASTNodes;

import com.ilog.translator.java2cs.translation.ITranslationContext;

public class SearchForEnclosingAccess extends ASTVisitor {

	private boolean result = false;

	private ITypeBinding enclosing = null;

	private ITypeHierarchy hierarchyOfEnclosingType = null;
	private ITypeHierarchy hierarchyOfInnerType = null;

	private final List<ASTNode> toRewrite = new ArrayList<ASTNode>();

	private final AbstractTypeDeclaration declarations;

	private final ITranslationContext context;

	private boolean isAnonymous = false;

	public SearchForEnclosingAccess(AbstractTypeDeclaration declarations,
			ITranslationContext context, boolean isAnonymous) {
		this.declarations = declarations;
		this.context = context;
		this.isAnonymous = isAnonymous;
		try {
			final IType itype = (IType) declarations.resolveBinding()
					.getJavaElement();
			hierarchyOfEnclosingType = itype
					.newSupertypeHierarchy(new NullProgressMonitor());
		} catch (final JavaModelException e) {
			e.printStackTrace();
			context.getLogger().logException("", e);
		}

	}

	public void initTypeHierarchy(ITypeBinding superOfInnerType) {
		try {
			final IType itype = (IType) superOfInnerType.getJavaElement();
			hierarchyOfInnerType = itype
					.newSupertypeHierarchy(new NullProgressMonitor());
		} catch (final Exception e) {
			e.printStackTrace();
			context.getLogger().logException(
					"On class " + declarations.getName()
							+ " error when computing super hierarchy for "
							+ superOfInnerType, e);
		}
	}

	public boolean hasEnclosingAccess() {
		return result;
	}

	public ITypeBinding getEncosingType() {
		return enclosing;
	}

	public List<ASTNode> getNodeToRewriteList() {
		return toRewrite;
	}

	@Override
	public void endVisit(MethodInvocation node) {
		final IMethodBinding imethod = node.resolveMethodBinding();
		if (imethod != null) {
			if ((node.getExpression() != null)
					&& (node.getExpression().resolveTypeBinding() != null)
					&& Flags.isStatic(imethod.getModifiers())) {
				// So it's a qualified static access
			} else {
				if (Flags.isStatic(imethod.getModifiers())) {
					return;
				}
				final ITypeBinding binding = imethod.getDeclaringClass();
				if (node.getExpression() != null) {
					if (node.getExpression().getNodeType() == ASTNode.SIMPLE_NAME) {
						final SimpleName sn = (SimpleName) node.getExpression();
						final IVariableBinding vb = (IVariableBinding) sn
								.resolveBinding();
						// TODO: must verify that this parameter is from the
						// enclosing method ?
						if (vb.isParameter()) {
							return;
						}
						if (!vb.isField()) {
							return;
						}
					}
				}
				if ((binding != null) && (node.getExpression() == null)) {
					// It's a method invocation without qualifier.
					final ITypeBinding declaringType = ASTNodes
							.getEnclosingType(node);
					final IType typeThatDeclareThatMethod = (IType) binding
							.getJavaElement();
					// first check if declaringType and
					// typeThatDeclareThatmethod are linked no ?
					final IType declaringIType = (IType) declaringType
							.getJavaElement();
					try {
						if (declaringIType.newSupertypeHierarchy(
								new NullProgressMonitor()).contains(
								typeThatDeclareThatMethod)) {
							return;
						}
						if (!typeThatDeclareThatMethod.getFullyQualifiedName()
								.equals("java.lang.Object")) {
							if (hierarchyOfEnclosingType
									.contains(typeThatDeclareThatMethod)) {
								// The call is potentially in the enclosing
								// class

								// IType declaringIType = (IType) declaringType
								// .getJavaElement();
								// But only if declaring type is not a subclass
								// of
								// enclosing type.
								if (!declaringIType.newSupertypeHierarchy(
										new NullProgressMonitor()).contains(
										(IType) declarations.resolveBinding()
												.getJavaElement())) {
									result = true;
									enclosing = binding;
									toRewrite.add(node);
								}

							} else if (declaringType.isEqualTo(binding)) {
								// That call is inside class
							} else {
								// The call is not local or in the enclosing
								// class
								// System.out.println("");
								// if (declarations.resolveBinding().getKey() ==
								// binding.getKey()) {
								if (isAnonymous
										&& !hierarchyOfInnerType
												.contains(typeThatDeclareThatMethod)) {
									result = true;
									enclosing = binding;
									toRewrite.add(node);
								}
							}
						}
					} catch (final JavaModelException e) {
						e.printStackTrace();
						context.getLogger().logException("", e);
					}
				}
			}
		} else {
			context
					.getLogger()
					.logError(
							"SearchForEnclosingAccess.MethodInvocation:: ERROR binding is null for method invocation "
									+ node + " " + declarations);
		}
	}

	@Override
	public void endVisit(FieldAccess node) {
		final IVariableBinding ifield = node.resolveFieldBinding();
		if (ifield != null) {
			final ITypeBinding binding = ifield.getDeclaringClass();
			if ((binding != null) && !Modifier.isStatic(ifield.getModifiers())) {
				final IType type = (IType) binding.getJavaElement();
				if (binding.isEqualTo(declarations.resolveBinding())) {
					return;
				}
				if (node.getExpression() != null) {
					switch (node.getExpression().getNodeType()) {
					case ASTNode.SIMPLE_NAME:
						final SimpleName sn = (SimpleName) node.getExpression();
						final IVariableBinding vb = (IVariableBinding) sn
								.resolveBinding();
						// TODO: must verify that this parameter is from the
						// enclosing method ?
						if (vb.isParameter()) {
							return;
						}
						break;
					case ASTNode.THIS_EXPRESSION:
						return;
					}
				}

				if (hierarchyOfEnclosingType.contains(type)) {
					result = true;
					enclosing = binding;
					toRewrite.add(node);
				}
			}
		}
	}

	@Override
	public void endVisit(SimpleName node) {
		final IBinding snBind = node.resolveBinding();
		if (snBind instanceof IVariableBinding) {
			final FieldAccess parentAsField = (FieldAccess) ASTNodes.getParent(
					node, ASTNode.FIELD_ACCESS);
			if (parentAsField != null) {
				if (parentAsField.getExpression() != null) {
					Expression expr = parentAsField.getExpression();
					//
					if (expr.getNodeType() == ASTNode.PARENTHESIZED_EXPRESSION) {
						final ParenthesizedExpression pExpr = (ParenthesizedExpression) expr;
						expr = pExpr.getExpression();
					}
					//
					switch (expr.getNodeType()) {
					case ASTNode.THIS_EXPRESSION:
						// if
						// (toRewrite.contains(parentAsField.getExpression()))
						return;
						// break;
					case ASTNode.CAST_EXPRESSION:
						expr = ((CastExpression) expr).getExpression();
						if (expr.getNodeType() == ASTNode.SIMPLE_NAME) {
							final SimpleName sn = (SimpleName) expr;
							final IVariableBinding vb = (IVariableBinding) sn
									.resolveBinding();
							// TODO: must verify that this parameter is from the
							// enclosing method ?
							if (vb.isParameter()) {
								return;
							}
						}
						break;
					case ASTNode.SIMPLE_NAME:
						final SimpleName sn = (SimpleName) expr;
						final IVariableBinding vb = (IVariableBinding) sn
								.resolveBinding();
						// TODO: must verify that this parameter is from the
						// enclosing method ?
						if (vb.isParameter()) {
							return;
						}
						break;
					}
				}
			}
			/*
			 * TODO: simplier to avoid visit if field is selected no ??? if
			 * (parentAsField != null) return;
			 */
			final IVariableBinding ifield = (IVariableBinding) snBind;
			final ITypeBinding binding = ifield.getDeclaringClass();
			if ((binding != null) && !Modifier.isStatic(ifield.getModifiers())) {
				final IType type = (IType) binding.getJavaElement();
				if (hierarchyOfEnclosingType.contains(type)
						&& !hierarchyOfInnerType.contains(type) // NEW :
				/*
				 * && declarations.resolveBinding().getKey() == binding
				 * .getKey()
				 */) {
					if (node.getParent().getNodeType() == ASTNode.QUALIFIED_NAME) {
						final QualifiedName qual = (QualifiedName) node
								.getParent();
						if (qual.getQualifier().resolveBinding() instanceof IVariableBinding) {
							final IVariableBinding qualifier = (IVariableBinding) qual
									.getQualifier().resolveBinding();
							final ITypeBinding classthatDeclarQualifier = qualifier
									.getDeclaringClass();
							if (classthatDeclarQualifier != null
									&& hierarchyOfInnerType
											.contains((IType) classthatDeclarQualifier
													.getJavaElement()))
								return;
							if (qualifier.isParameter()) {
								return;
							}
							if (!qualifier.isField()) {
								return;
							}
							// Ok it's not a parameter nor a field
							// return;
						}
					}
					result = true;
					enclosing = binding;
					toRewrite.add(node);
				}
			}
		}
	}

	@Override
	public void endVisit(ThisExpression node) {
		if (node.getQualifier() != null) {
			final Name expr = node.getQualifier();
			final ITypeBinding tb = expr.resolveTypeBinding();
			if (declarations.resolveBinding().isEqualTo(tb)) {
				result = true;
				enclosing = tb;
				toRewrite.add(node);
			}
		}
	}

	@Override
	public void endVisit(ClassInstanceCreation node) {
		final Type type = node.getType();
		if (type != null
				&& type.resolveBinding() != null
				&& context.hasEnclosingAccess(type.resolveBinding()
						.getQualifiedName(), declarations.resolveBinding()
						.getQualifiedName())) {
			result = true;
		}
	}
	/*
	 * public void endVisit( node) { IBinding snBind = node.resolveBinding(); if
	 * (snBind instanceof IVariableBinding) { IVariableBinding ifield =
	 * (IVariableBinding) snBind; ITypeBinding binding =
	 * ifield.getDeclaringClass(); if (binding != null &&
	 * declarations.resolveBinding().getKey() == binding .getKey()) { result =
	 * true; enclosing = binding; toRewrite.add(node); } } }
	 */
}