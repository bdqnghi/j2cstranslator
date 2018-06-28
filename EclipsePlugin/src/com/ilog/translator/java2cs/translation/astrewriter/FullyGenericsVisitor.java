package com.ilog.translator.java2cs.translation.astrewriter;

import java.util.List;

import org.eclipse.jdt.core.dom.AST;
import org.eclipse.jdt.core.dom.ASTNode;
import org.eclipse.jdt.core.dom.Assignment;
import org.eclipse.jdt.core.dom.ClassInstanceCreation;
import org.eclipse.jdt.core.dom.Expression;
import org.eclipse.jdt.core.dom.FieldAccess;
import org.eclipse.jdt.core.dom.FieldDeclaration;
import org.eclipse.jdt.core.dom.IBinding;
import org.eclipse.jdt.core.dom.IMethodBinding;
import org.eclipse.jdt.core.dom.ITypeBinding;
import org.eclipse.jdt.core.dom.IVariableBinding;
import org.eclipse.jdt.core.dom.MethodDeclaration;
import org.eclipse.jdt.core.dom.MethodInvocation;
import org.eclipse.jdt.core.dom.Modifier;
import org.eclipse.jdt.core.dom.ParameterizedType;
import org.eclipse.jdt.core.dom.QualifiedName;
import org.eclipse.jdt.core.dom.SimpleName;
import org.eclipse.jdt.core.dom.Type;
import org.eclipse.jdt.core.dom.VariableDeclarationFragment;
import org.eclipse.jdt.core.dom.VariableDeclarationStatement;
import org.eclipse.jdt.core.dom.rewrite.ListRewrite;
import org.eclipse.jdt.internal.corext.dom.ASTNodes;
import org.eclipse.jdt.internal.corext.dom.Bindings;
import org.eclipse.text.edits.TextEditGroup;

import com.ilog.translator.java2cs.translation.ITranslationContext;

/**
 * 1/ Map<String, String> map = new Hashtable(); Replace new Hashtable() by
 * Hashtable<String, String> ?
 * 
 * 2/ Hashtable map = new Hashtable<String, String>(); Replace Hashtable by
 * Hashtable<String, String> ?
 * 
 * 3/ Generic Method without type parameter (implicitly infer in Java) For
 * example EnumSet.noneOf(E.class) -> EnumSet.<E>noneof(E.class)
 * 
 * @author afau
 * 
 */
public class FullyGenericsVisitor extends ASTRewriterVisitor {

	//
	//
	//

	public FullyGenericsVisitor(ITranslationContext context) {
		super(context);
		transformerName = "Fully Generics";
		description = new TextEditGroup(transformerName);
	}

	//
	//
	//

	@SuppressWarnings("unchecked")
	@Override
	public void endVisit(Assignment node) {
		if (node.getOperator() == Assignment.Operator.ASSIGN) {
			final Expression left = node.getLeftHandSide();
			final Expression right = node.getRightHandSide();
			final ITypeBinding leftType = left.resolveTypeBinding();
			final ITypeBinding rightType = right.resolveTypeBinding();
			if (right.getNodeType() == ASTNode.CLASS_INSTANCE_CREATION) {
				if (isRawTypeOf(leftType, rightType)) {
					final Type type = getTypeForField(left);
					if (type == null)
						return;
					final List<Type> typeArgs = ((ParameterizedType) type)
							.typeArguments();
					if (typeArgs != null)
						addTypeParameters(((ClassInstanceCreation) right)
								.getType(), typeArgs);
				} else if (isRawTypeOf(rightType, leftType)) {
					final Type type = getTypeForField(left);
					if (type == null)
						return;
					final ParameterizedType ptype = (ParameterizedType) ((ClassInstanceCreation) right)
							.getType();
					addTypeParameters(type, ptype.typeArguments());
				}
			}
		}
	}

	@SuppressWarnings("unchecked")
	@Override
	public void endVisit(VariableDeclarationFragment node) {
		final IVariableBinding binding = node.resolveBinding();
		if (binding != null && !binding.isField() && !binding.isEnumConstant()) {
			// check for local variable
			final ITypeBinding declaringType = binding.getType();
			final Expression initializer = node.getInitializer();
			if (initializer != null
					&& initializer.getNodeType() == ASTNode.CLASS_INSTANCE_CREATION) {
				final ITypeBinding initialierType = initializer
						.resolveTypeBinding();
				if (isRawTypeOf(declaringType, initialierType)) {
					// First case
					// ok we now need to add type parameter to the class
					// instance creation
					final VariableDeclarationStatement stat = (VariableDeclarationStatement) ASTNodes
							.getParent(node,
									ASTNode.VARIABLE_DECLARATION_STATEMENT);
					final ParameterizedType type = (ParameterizedType) stat
							.getType();
					addTypeParameters(((ClassInstanceCreation) initializer)
							.getType(), type.typeArguments());
				} else if (isRawTypeOf(initialierType, declaringType)) {
					// Second case
					final VariableDeclarationStatement stat = (VariableDeclarationStatement) ASTNodes
							.getParent(node,
									ASTNode.VARIABLE_DECLARATION_STATEMENT);
					final ParameterizedType type = (ParameterizedType) ((ClassInstanceCreation) initializer)
							.getType();
					addTypeParameters(stat.getType(), type.typeArguments());
				}
			}
		}
	}

	@Override
	public void endVisit(MethodInvocation node) {
		final IMethodBinding mBinding = node.resolveMethodBinding();

		if (mBinding != null && mBinding.isParameterizedMethod()) {
			final IMethodBinding genericMethod = mBinding
					.getMethodDeclaration();
			final ITypeBinding[] typeArgs = mBinding.getTypeArguments();
			if (genericMethod != null) {
				// ITypeBinding type = getTypeOfCallee(node);
				if (node.typeArguments().size() == 0 && typeArgs != null
						&& node.getExpression() != null
						&& isAImplicitInferOnReturnTypeMethod(genericMethod)) {
					final ListRewrite lr = currentRewriter.getListRewrite(node,
							MethodInvocation.TYPE_ARGUMENTS_PROPERTY);
					for (final ITypeBinding tParam : typeArgs) {
						if (!tParam.isCapture()) {
							final Type pType = (Type) currentRewriter
									.createStringPlaceholder(tParam
											.getQualifiedName(),
											ASTNode.SIMPLE_TYPE);
							lr.insertLast(pType, null);
						}
					}
					return;
				}
			}
			final ITypeBinding[] typeParams = mBinding.getParameterTypes();
			final AST ast = node.getAST();
			if (hasGenericDependance(typeParams, typeArgs)) {
				if (node.typeArguments().size() == 0 && typeArgs != null
						&& typeArgs.length > 0) {
					if (node.getExpression() == null && !Modifier.isStatic(mBinding.getModifiers())) {
						ASTNode value = currentRewriter
								.createStringPlaceholder("this",
										ASTNode.THIS_EXPRESSION);
						currentRewriter.set(node,
								MethodInvocation.EXPRESSION_PROPERTY, value,
								description);
					}
					final ListRewrite lr = currentRewriter.getListRewrite(node,
							MethodInvocation.TYPE_ARGUMENTS_PROPERTY);
					for (final ITypeBinding tParam : typeArgs) {
						if (!tParam.isCapture()) {
							final Type pType = ast.newSimpleType(ast
									.newName(tParam.getQualifiedName()));
							lr.insertLast(pType, null);
						}
					}
				}
			} else {
				// only the return type "use" generic arg
				if (node.getExpression() == null && !Modifier.isStatic(mBinding.getModifiers())) {
					ASTNode value = currentRewriter.createStringPlaceholder(
							"this", ASTNode.THIS_EXPRESSION);
					currentRewriter.set(node,
							MethodInvocation.EXPRESSION_PROPERTY, value,
							description);
				}
				final ListRewrite lr = currentRewriter.getListRewrite(node,
						MethodInvocation.TYPE_ARGUMENTS_PROPERTY);

				// how to determine if it's really needed ?
				/*
				if (node.typeArguments().size() == 0) {
					for (final ITypeBinding tParam : typeArgs) {
						if (!tParam.isCapture()) {
							String replacementCode = tParam.getQualifiedName();
							Type pType = null;
							if (replacementCode.contains("<"))
								pType = (Type) currentRewriter
										.createStringPlaceholder(
												replacementCode,
												ASTNode.PARAMETERIZED_TYPE);
							else
								pType = ast.newSimpleType(ast.newName(tParam
										.getQualifiedName()));
							lr.insertLast(pType, null);
						}
					}
				}*/
			}
		}
	}

	// case where
	// public <T> int myMethod(Class<T> p) {
	// return (*)otherMethod(p);
	// }
	// public <T> int otherMethod(Class<T> p) {
	// //
	// }
	// we don't want to add <T> at (*)
	//
	private boolean enclosingMethodIsGeneric(MethodInvocation node,
			ITypeBinding[] typeParams, ITypeBinding[] typeArgs) {
		MethodDeclaration md = (MethodDeclaration) ASTNodes.getParent(node,
				ASTNode.METHOD_DECLARATION);

		if (md != null) {
			IMethodBinding imb = md.resolveBinding();
			if (imb != null) {
				ITypeBinding[] thisTypeArgs = imb.getTypeArguments();
				if (thisTypeArgs != null) {
					for (int i = 0; i < thisTypeArgs.length; i++) {
						ITypeBinding current = thisTypeArgs[i];
					}
				}
				int cpt = 0;
				ITypeBinding[] thisTypeParams = imb.getTypeParameters();
				if (thisTypeParams != null) {
					// search if each typeArgs[i] is present in
					// thisTypeParams[j];
					for (int i = 0; i < thisTypeParams.length; i++) {
						for (int j = 0; j < thisTypeParams.length; j++) {
							ITypeBinding current = thisTypeParams[i];
							ITypeBinding other = typeArgs[j];
							if (other.equals(current)) {
								cpt++;
							}
						}
					}
					return (cpt == thisTypeParams.length);
				}
			}
		}
		return false;
	}

	//
	//
	//

	private Type getTypeForField(Expression left) {
		IVariableBinding vb = null;
		switch (left.getNodeType()) {
		case ASTNode.SIMPLE_NAME:
			final SimpleName sn = (SimpleName) left;
			final IBinding binding = sn.resolveBinding();
			if (binding instanceof IVariableBinding) {
				vb = (IVariableBinding) binding;

			}
			break;
		case ASTNode.QUALIFIED_NAME:
			final QualifiedName qn = (QualifiedName) left;
			final IBinding qbinding = qn.resolveBinding();
			if (qbinding instanceof IVariableBinding) {
				vb = (IVariableBinding) qbinding;
			}
			break;
		case ASTNode.FIELD_ACCESS:
			final FieldAccess fa = (FieldAccess) left;
			vb = fa.resolveFieldBinding();
			break;
		}
		if (vb != null) {
			if (vb.isField()) {
				final VariableDeclarationFragment field = (VariableDeclarationFragment) ASTNodes
						.findDeclaration(vb, left.getRoot());
				if (field != null) {
					final FieldDeclaration stat = (FieldDeclaration) ASTNodes
							.getParent(field, ASTNode.FIELD_DECLARATION);
					return stat.getType();
				}
			}
		}
		return null;
	}

	private boolean isAImplicitInferOnReturnTypeMethod(
			IMethodBinding genericMethod) {
		final ITypeBinding[] typeParams = genericMethod.getParameterTypes();
		final ITypeBinding returnType = genericMethod.getReturnType();
		if (typeParams.length > 0) {
			if (typeParams[0].isArray()) {
				if (typeParams[0].getComponentType().isEqualTo(returnType))
					return true;
				final ITypeBinding[] typeArgs = returnType.getTypeArguments();
				if (typeArgs.length > 0) {
					if (typeParams[0].getComponentType().isEqualTo(typeArgs[0]))
						return true;
				}
			} else {
				final ITypeBinding[] typeArgs = returnType.getTypeArguments();
				if (typeArgs.length > 0) {
					if (typeParams[0].isEqualTo(typeArgs[0]))
						return true;
				}
			}
		}
		return false;
	}

	/**
	 * Wants to know if one of the type arguments are parametrized by one the
	 * the type parameters
	 */
	private boolean hasGenericDependance(ITypeBinding[] typeParams,
			ITypeBinding[] typeArgs) {
		for (final ITypeBinding typeArg : typeArgs) {
			for (final ITypeBinding typeParam : typeParams) {
				if (isParametrizedBy(typeParam, typeArg))
					return true;
			}
		}
		return false;
	}

	private boolean isParametrizedBy(ITypeBinding typeArg,
			ITypeBinding typeParam) {
		final ITypeBinding etype = typeArg.getErasure();
		if (etype.getQualifiedName().equals("java.lang.Class")) {
			final ITypeBinding[] typeParameters = typeArg.getTypeArguments();
			for (final ITypeBinding currentTypeParameter : typeParameters) {
				return currentTypeParameter.isEqualTo(typeParam);
			}
		}
		return false;
	}

	@SuppressWarnings("unchecked")
	private void addTypeParameters(Type type, List<Type> list) {
		final AST ast = type.getAST();
		final ParameterizedType pType = ast.newParameterizedType((Type) ASTNode
				.copySubtree(ast, type));

		final List<Type> copiedTypeArgument = ASTNode.copySubtrees(ast, list);
		for (final Type typeArg : copiedTypeArgument) {
			pType.typeArguments().add(typeArg);
		}
		if (context.getConfiguration().getOptions().isFillRawTypesUse())
			currentRewriter.replace(type, pType, description);
	}

	private boolean isRawTypeOf(ITypeBinding genericVersion,
			ITypeBinding rawVersion) {
		final boolean generics = !genericVersion.isRawType();
		final boolean raw = rawVersion.isRawType();
		if (raw && generics) {
			final String rawName1 = Bindings.getRawQualifiedName(rawVersion);
			final String rawName2 = Bindings
					.getRawQualifiedName(genericVersion);
			return rawName1.equals(rawName2);
		}
		return false;
	}
}
