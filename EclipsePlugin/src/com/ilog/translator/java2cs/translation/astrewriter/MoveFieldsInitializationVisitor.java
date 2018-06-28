package com.ilog.translator.java2cs.translation.astrewriter;

import java.util.ArrayList;
import java.util.Iterator;
import java.util.List;

import org.eclipse.core.runtime.Assert;
import org.eclipse.core.runtime.CoreException;
import org.eclipse.core.runtime.NullProgressMonitor;
import org.eclipse.jdt.core.Flags;
import org.eclipse.jdt.core.IMethod;
import org.eclipse.jdt.core.IType;
import org.eclipse.jdt.core.ITypeHierarchy;
import org.eclipse.jdt.core.JavaModelException;
import org.eclipse.jdt.core.dom.AST;
import org.eclipse.jdt.core.dom.ASTNode;
import org.eclipse.jdt.core.dom.ASTVisitor;
import org.eclipse.jdt.core.dom.ArrayCreation;
import org.eclipse.jdt.core.dom.ArrayInitializer;
import org.eclipse.jdt.core.dom.ArrayType;
import org.eclipse.jdt.core.dom.Assignment;
import org.eclipse.jdt.core.dom.Block;
import org.eclipse.jdt.core.dom.BodyDeclaration;
import org.eclipse.jdt.core.dom.CastExpression;
import org.eclipse.jdt.core.dom.ClassInstanceCreation;
import org.eclipse.jdt.core.dom.Expression;
import org.eclipse.jdt.core.dom.ExpressionStatement;
import org.eclipse.jdt.core.dom.FieldAccess;
import org.eclipse.jdt.core.dom.FieldDeclaration;
import org.eclipse.jdt.core.dom.IBinding;
import org.eclipse.jdt.core.dom.IMethodBinding;
import org.eclipse.jdt.core.dom.ITypeBinding;
import org.eclipse.jdt.core.dom.IVariableBinding;
import org.eclipse.jdt.core.dom.Initializer;
import org.eclipse.jdt.core.dom.MethodDeclaration;
import org.eclipse.jdt.core.dom.MethodInvocation;
import org.eclipse.jdt.core.dom.Modifier;
import org.eclipse.jdt.core.dom.ParenthesizedExpression;
import org.eclipse.jdt.core.dom.PrimitiveType;
import org.eclipse.jdt.core.dom.SimpleName;
import org.eclipse.jdt.core.dom.SingleVariableDeclaration;
import org.eclipse.jdt.core.dom.SuperConstructorInvocation;
import org.eclipse.jdt.core.dom.Type;
import org.eclipse.jdt.core.dom.TypeDeclaration;
import org.eclipse.jdt.core.dom.VariableDeclarationFragment;
import org.eclipse.jdt.core.dom.rewrite.ASTRewrite;
import org.eclipse.jdt.internal.corext.dom.ASTNodes;
import org.eclipse.jdt.internal.corext.refactoring.util.JavaElementUtil;
import org.eclipse.text.edits.TextEditGroup;

import com.ilog.translator.java2cs.translation.ITranslationContext;

public class MoveFieldsInitializationVisitor extends ASTRewriterVisitor {

	//
	//
	//

	public MoveFieldsInitializationVisitor(ITranslationContext context) {
		super(context);
		transformerName = "Move Fields Initialization to constructors";
		description = new TextEditGroup(transformerName);
	}

	//
	//
	//

	@SuppressWarnings("unchecked")
	@Override
	public void endVisit(MethodInvocation node) {
		final IMethodBinding mBinding = node.resolveMethodBinding();
		if (mBinding != null) {
			if (mBinding.isGenericMethod()
					|| mBinding.getDeclaringClass().isGenericType()
					|| (mBinding.getTypeArguments() != null && mBinding
							.getTypeArguments().length > 0)
					|| (mBinding.getDeclaringClass().getTypeArguments() != null && mBinding
							.getDeclaringClass().getTypeArguments().length > 0)
					|| (mBinding.getDeclaringClass().getTypeParameters() != null && mBinding
							.getDeclaringClass().getTypeParameters().length > 0))
				return;
		}
		if (node.getExpression() != null) {
			final ITypeBinding type = node.getExpression().resolveTypeBinding();
			final List<IType> interfaces = getInterfaceThatDeclare(type,
					mBinding);
			if (interfaces != null && interfaces.size() > 1) {
				final AST ast = currentRewriter.getAST();
				final CastExpression cast = ast.newCastExpression();
				cast.setExpression((Expression) currentRewriter
						.createMoveTarget(node.getExpression()));
				final IType inter = interfaces.get(0);
				// IType model = (IType)
				// mBinding.getDeclaringClass().getJavaElement();
				try {
					final Type ctype = (Type) currentRewriter
							.createStringPlaceholder(inter
									.getFullyQualifiedParameterizedName()
									.replace('$', '.'), ASTNode.SIMPLE_TYPE);
					cast.setType(ctype);
					final ParenthesizedExpression replacement = ast
							.newParenthesizedExpression();
					replacement.setExpression(cast);
					currentRewriter.replace(node.getExpression(), replacement,
							description);
				} catch (final CoreException e) {
					context.getLogger().logException("", e);
				}
			}
		}
	}

	@SuppressWarnings("unchecked")
	@Override
	public void endVisit(TypeDeclaration node) {
		if (!node.isInterface()) {
			final List<VariableDeclarationFragment> fields = getFieldsToInitializeInConstructor(node);
			final List<VariableDeclarationFragment> sfields = getStaticFinalFieldsToInitializeInStaticConstructor(node);
			if ((fields != null) && (fields.size() > 0)) {
				try {
					final IType fType = (IType) node.resolveBinding()
							.getJavaElement();
					if (JavaElementUtil.getAllConstructors(fType).length == 0) {
						// No existing constructor, need to create one
						if (fields != null && fields.size() > 0) {
							final MethodDeclaration constructor = createConstructor(
									node, fields);
							currentRewriter.getListRewrite(node,
									node.getBodyDeclarationsProperty())
									.insertFirst(constructor, description);
						}
						if (sfields != null && sfields.size() > 0) {
							final Initializer sconstructor = createStaticInitializer(
									node, sfields);
							currentRewriter.getListRewrite(node,
									node.getBodyDeclarationsProperty())
									.insertFirst(sconstructor, description);
						}
					} else {
						for (final Iterator iter = node.bodyDeclarations()
								.iterator(); iter.hasNext();) {
							final ASTNode astNode = (ASTNode) iter.next();
							if (astNode.getNodeType() == ASTNode.METHOD_DECLARATION) {
								final MethodDeclaration mDecl = (MethodDeclaration) astNode;
								if (mDecl.isConstructor()) {
									if (fields != null && fields.size() > 0) {
										addFieldInitialization(node,
												currentRewriter, mDecl
														.getBody(), fields);
									}
								}
							} else if (astNode.getNodeType() == ASTNode.INITIALIZER) {
								final Initializer mDecl = (Initializer) astNode;
								if (Modifier.isStatic(mDecl.getModifiers())) {
									if (sfields != null && sfields.size() > 0) {
										addFieldInitialization(node,
												currentRewriter, mDecl
														.getBody(), sfields);
									}
								}
							}
						}
					}
					removeInitializationFromDeclaredFields(node,
							currentRewriter);
				} catch (final JavaModelException e) {
					e.printStackTrace();
					context.getLogger().logException("", e);
				}
			}
		}
	}

	//
	//
	//

	@SuppressWarnings("unchecked")
	private Initializer createStaticInitializer(TypeDeclaration node,
			List<VariableDeclarationFragment> fields) {
		final AST ast = node.getAST();
		final Initializer constructor = ast.newInitializer();
		final Block body = ast.newBlock();
		addFieldInitialization(node, currentRewriter, body, fields);
		constructor.setBody(body);
		constructor.modifiers().add(
				ast.newModifier(Modifier.ModifierKeyword.STATIC_KEYWORD));
		return constructor;
	}

	@SuppressWarnings("unchecked")
	private MethodDeclaration createConstructor(TypeDeclaration node,
			List<VariableDeclarationFragment> fields) {
		final AST ast = node.getAST();
		final MethodDeclaration emptyConstructor = ast.newMethodDeclaration();
		emptyConstructor.setConstructor(true);
		emptyConstructor.setName(ast.newSimpleName(node.getName()
				.getIdentifier()));
		Block body = ast.newBlock();
		addFieldInitialization(node, currentRewriter, body, fields);
		emptyConstructor.setBody(body);
		emptyConstructor.modifiers().add(
				ast.newModifier(Modifier.ModifierKeyword.PUBLIC_KEYWORD));
		//
		final MethodDeclaration constructor = ast.newMethodDeclaration();
		constructor.setConstructor(true);
		constructor.setName(ast.newSimpleName(node.getName().getIdentifier()));
		body = ast.newBlock();
		//					
		final int paramCount = constructor.parameters().size();
		if (paramCount > 0) {
			// hum hum
			final SuperConstructorInvocation superConstructorInvocation = ast
					.newSuperConstructorInvocation();
			for (int i = 0; i < paramCount; i++) {
				final SingleVariableDeclaration param = (SingleVariableDeclaration) constructor
						.parameters().get(i);
				superConstructorInvocation.arguments().add(
						ast.newSimpleName(param.getName().getIdentifier()));
			}
			body.statements().add(superConstructorInvocation);
		}
		//
		addFieldInitialization(node, currentRewriter, body, fields);
		constructor.setBody(body);
		constructor.modifiers().add(
				ast.newModifier(Modifier.ModifierKeyword.PUBLIC_KEYWORD));
		return constructor;
	}

	@SuppressWarnings("unchecked")
	private List getStaticFinalFieldsToInitializeInStaticConstructor(
			TypeDeclaration typeDecl) {
		final List result = new ArrayList();
		for (final Iterator iter = typeDecl.bodyDeclarations().iterator(); iter
				.hasNext();) {
			final BodyDeclaration element = (BodyDeclaration) iter.next();
			if (!(element instanceof FieldDeclaration)) {
				continue;
			}
			final FieldDeclaration field = (FieldDeclaration) element;
			for (final Iterator fragmentIter = field.fragments().iterator(); fragmentIter
					.hasNext();) {
				final VariableDeclarationFragment fragment = (VariableDeclarationFragment) fragmentIter
						.next();
				if (isStaticFinal(field)
				/* && (this.isToBeInitializerInConstructor(fragment)) */) {
					// result.add(fragment);
				}
			}
		}
		return reverse(result);
	}

	@SuppressWarnings("unchecked")
	private List<VariableDeclarationFragment> getFieldsToInitializeInConstructor(
			TypeDeclaration typeDecl) {
		final List<VariableDeclarationFragment> result = new ArrayList<VariableDeclarationFragment>(
				0);
		for (final FieldDeclaration field : typeDecl.getFields()) {
			for (final Iterator fragmentIter = field.fragments().iterator(); fragmentIter
					.hasNext();) {
				final VariableDeclarationFragment fragment = (VariableDeclarationFragment) fragmentIter
						.next();
				if (!isStaticFinal(field)
						&& !Modifier.isStatic(field.getModifiers())
						&& isToBeInitializerInConstructor(fragment)) {
					result.add(fragment);
				} else if (isClassInstanceCreationWithRef(fragment)) {
					if (!isStaticFinal(field))
						result.add(fragment);
				}
			}
		}
		/*
		 * for (final Iterator iter = typeDecl.bodyDeclarations().iterator();
		 * iter .hasNext();) { final BodyDeclaration element = (BodyDeclaration)
		 * iter.next(); if (!(element instanceof FieldDeclaration)) { continue;
		 * } final FieldDeclaration field = (FieldDeclaration) element; for
		 * (final Iterator fragmentIter = field.fragments().iterator();
		 * fragmentIter .hasNext();) { final VariableDeclarationFragment
		 * fragment = (VariableDeclarationFragment) fragmentIter .next(); if
		 * (!isStaticFinal(field) && !Modifier.isStatic(field.getModifiers()) &&
		 * isToBeInitializerInConstructor(fragment)) { result.add(fragment); }
		 * else if (isClassInstanceCreationWithRef(fragment)) { if
		 * (!isStaticFinal(field)) result.add(fragment); } } }
		 */
		return reverse(result);
	}

	/**
	 * The funny part ... if there is a dependance between final fields that
	 * order list does not take care of it ...
	 */
	private List<VariableDeclarationFragment> reverse(
			List<VariableDeclarationFragment> fields) {
		List<VariableDeclarationFragment> result = new ArrayList<VariableDeclarationFragment>();
		for(int i = 0; i < fields.size(); i++) {
			result.add(fields.get(fields.size() - 1 - i));
		}
		return result;
	}

	
	

	private boolean isStaticFinal(FieldDeclaration field) {
		return Modifier.isFinal(field.getModifiers())
				&& Modifier.isStatic(field.getModifiers());
	}

	class SearchAnonymous extends ASTVisitor {
		public boolean isClassInstanceCreationWithReference = false;

		public boolean isAnonymousClassInstanceCreation = false;

		@Override
		public boolean visit(FieldAccess node) {
			if (!Modifier.isStatic(node.resolveFieldBinding().getModifiers())) {
				isClassInstanceCreationWithReference = true;
			}
			return true;
		}

		@Override
		public boolean visit(MethodInvocation node) {
			if (!Modifier.isStatic(node.resolveMethodBinding().getModifiers())) {
				isClassInstanceCreationWithReference = true;
			}
			return true;
		}

		@SuppressWarnings("unchecked")
		@Override
		public boolean visit(ClassInstanceCreation node) {

			if (((node.arguments() != null) && (node.arguments().size() != 0))
					|| (node.getType().resolveBinding().isNested())) {
				final List args = node.arguments();
				for (int i = 0; i < args.size(); i++) {
					final ASTNode arg = (ASTNode) args.get(i);
					switch (arg.getNodeType()) {
					case ASTNode.SIMPLE_NAME:
					case ASTNode.QUALIFIED_NAME:
						break;
					case ASTNode.THIS_EXPRESSION:
						isClassInstanceCreationWithReference = true;
						break;
					case ASTNode.FIELD_ACCESS:
						final FieldAccess fa = (FieldAccess) arg;
						if (!Modifier.isStatic(fa.resolveFieldBinding()
								.getModifiers())) {
							isClassInstanceCreationWithReference = true;
						}
						break;
					case ASTNode.METHOD_INVOCATION:
						final MethodInvocation mi = (MethodInvocation) arg;
						if (!Modifier.isStatic(mi.resolveMethodBinding()
								.getModifiers())) {
							isClassInstanceCreationWithReference = true;
						}
						break;
					}
				}

				final ITypeBinding typeB = node.resolveTypeBinding();
				if (typeB.isMember()) {
					final ITypeBinding enclosingT = typeB.getDeclaringClass();
					if (context.hasEnclosingAccess(typeB.getQualifiedName(),
							enclosingT.getQualifiedName()))
						isClassInstanceCreationWithReference = true;
				}
			}
			return true;
		}
	};

	private boolean isClassInstanceCreationWithRef(
			VariableDeclarationFragment fragment) {
		final Expression init = fragment.getInitializer();
		if (init == null) {
			return false;
		}
		final SearchAnonymous visitor = new SearchAnonymous();
		init.accept(visitor);
		return visitor.isClassInstanceCreationWithReference;
		//

	}

	private boolean isToBeInitializerInConstructor(
			VariableDeclarationFragment fragment) {
		if (fragment.getInitializer() == null) {
			return false;
		}
		return true; // areLocalsUsedIn(fragment.getInitializer());
	}

	@SuppressWarnings("unchecked")
	private void addFieldInitialization(TypeDeclaration typeDecl,
			ASTRewrite rewrite, Block constructorBody,
			List<VariableDeclarationFragment> fields) {
		final AST ast = rewrite.getAST(); // fNestedOrInnerClassNode.getAST();

		//
		final boolean isAnonymous = typeDecl.resolveBinding().getName()
				.startsWith("Anonymous_C");
		final ASTNode firstStatement = constructorBody.statements().size() > 0 ? (ASTNode) constructorBody
				.statements().get(0)
				: null;
		ASTNode currentInsertPosition = firstStatement;
		int pos = 0;
		while (isAFieldAssignement(currentInsertPosition)
				&& pos < constructorBody.statements().size()) {
			currentInsertPosition = (ASTNode) constructorBody.statements().get(
					pos++);
		}
		//
		for (final Iterator iter = fields.iterator(); iter.hasNext();) {
			final VariableDeclarationFragment fragment = (VariableDeclarationFragment) iter
					.next();
			final Assignment assignmentExpression = ast.newAssignment();
			assignmentExpression.setOperator(Assignment.Operator.ASSIGN);
			final String name = fragment.getName().getIdentifier();

			final FieldAccess access = ast.newFieldAccess();
			if (!Modifier.isStatic(fragment.resolveBinding().getModifiers())) {
				access.setExpression(ast.newThisExpression());
				access.setName(ast.newSimpleName(name));
				assignmentExpression.setLeftHandSide(access);
			} else {
				assignmentExpression.setLeftHandSide(ast.newSimpleName(name));
			}

			final Expression initializer = fragment.getInitializer();

			int extraArrayDimentions = fragment.getExtraDimensions();
			
			final Expression rhs = addArrayCreation(initializer, ASTNodes
					.getType(fragment), extraArrayDimentions, rewrite);

			assignmentExpression.setRightHandSide(rhs);
			final ExpressionStatement assignmentStatement = ast
					.newExpressionStatement(assignmentExpression);
			//
			if ((firstStatement != null)
					&& (firstStatement.getNodeType() == ASTNode.SUPER_CONSTRUCTOR_INVOCATION)) {
				currentRewriter.getListRewrite(constructorBody,
						Block.STATEMENTS_PROPERTY).insertAfter(
						assignmentStatement, firstStatement, null);
			} else if ((firstStatement != null)
					&& (firstStatement.getNodeType() == ASTNode.CONSTRUCTOR_INVOCATION)) {
				// Do nothing ! initialization are done in the invoked
				// constructor
			} else if (isAnonymous && currentInsertPosition != null) {
				currentRewriter.getListRewrite(constructorBody,
						Block.STATEMENTS_PROPERTY).insertAfter(
						assignmentStatement, currentInsertPosition, null);
				currentInsertPosition = assignmentStatement;
			} else {
				currentRewriter.getListRewrite(constructorBody,
						Block.STATEMENTS_PROPERTY).insertFirst(
						assignmentStatement, description);
			}
		}
	}

	private boolean isAFieldAssignement(ASTNode currentInsertPosition) {
		if ((currentInsertPosition != null)
				&& (currentInsertPosition.getNodeType() == ASTNode.EXPRESSION_STATEMENT)) {
			final ExpressionStatement eStat = (ExpressionStatement) currentInsertPosition;
			if (eStat.getExpression().getNodeType() == ASTNode.ASSIGNMENT) {
				final Assignment assign = (Assignment) eStat.getExpression();
				if (assign.getLeftHandSide().getNodeType() == ASTNode.FIELD_ACCESS) {
					return true;
				}
			}
		}
		return false;
	}

	private Expression addArrayCreation(Expression initializer, Type type, int extraArrayDimentions, 
			ASTRewrite rewrite) {
		if (initializer.getNodeType() == ASTNode.ARRAY_INITIALIZER) {
			final AST ast = initializer.getAST();
			final ArrayCreation creation = ast.newArrayCreation();
			creation.setInitializer((ArrayInitializer) rewrite
					.createCopyTarget(initializer));			
			if (!type.isPrimitiveType()) {
			   // ArrayType aType = (ArrayType) cloneArrayType(ast, type);
			   creation.setType((ArrayType) ASTNode.copySubtree(ast, type));
			} else {
				PrimitiveType pType = (PrimitiveType) type;							
				ArrayType aType = null;
				if (extraArrayDimentions > 0)
					aType = ast.newArrayType(ast.newPrimitiveType(pType.getPrimitiveTypeCode()), extraArrayDimentions);
				else
					aType = ast.newArrayType(ast.newPrimitiveType(pType.getPrimitiveTypeCode()));
				
				creation.setType(aType);
			}
			return creation;
		}
		return (Expression) rewrite.createCopyTarget(initializer);
	}

	@SuppressWarnings("unchecked")
	private void removeInitializationFromDeclaredFields(
			TypeDeclaration typeDecl, ASTRewrite rewrite) {
		for (final Iterator iter = getFieldsToInitializeInConstructor(typeDecl)
				.iterator(); iter.hasNext();) {
			final VariableDeclarationFragment fragment = (VariableDeclarationFragment) iter
					.next();
			Assert.isNotNull(fragment.getInitializer());
			rewrite.remove(fragment.getInitializer(), description);
		}
	}

	private List<IType> getInterfaceThatDeclare(ITypeBinding typeB,
			IMethodBinding binding) {
		final List<IType> result = new ArrayList<IType>();
		if (typeB != null && typeB.getJavaElement() instanceof IType) {
			final IType type = (IType) typeB.getJavaElement();
			if (type == null) { // array ??
				return result;
			}
			final IMethod method = (IMethod) binding.getJavaElement();
			if (method == null) { // enum ??
				return result;
			}
			final NullProgressMonitor monitor = new NullProgressMonitor();
			try {
				final ITypeHierarchy hierarchy = type
						.newSupertypeHierarchy(monitor);
				final IType[] interfaces = hierarchy.getAllInterfaces();
				for (final IType in : interfaces) {
					final IMethod[] methods = in.getMethods();
					if (methods != null && methods.length > 0) {
						final IMethod declmethod = findMethod(method, methods);
						if (declmethod != null
								&& !containsInHierarchy(hierarchy, result, in)) {
							result.add(in);
						}
					}
				}
				return result;
			} catch (final JavaModelException e) {

			}
		}
		return result;
	}

	private boolean containsInHierarchy(ITypeHierarchy hierarchy,
			List<IType> interfaces, IType current) throws JavaModelException {
		for (final IType candidate : interfaces) {
			final IType[] superInterfaceOfCandidate = hierarchy
					.getAllSuperInterfaces(candidate);
			if (contains(current, superInterfaceOfCandidate))
				return true;
			final IType[] superInterfaceOfCurrent = hierarchy
					.getAllSuperInterfaces(current);
			if (contains(candidate, superInterfaceOfCurrent))
				return true;
		}
		return false;
	}

	private boolean contains(IType current, IType[] superInterfaceOfCandidate) {
		for (final IType candidate : superInterfaceOfCandidate) {
			if (candidate.equals(current))
				return true;
		}
		return false;
	}

	private IMethod findMethod(IMethod method, IMethod[] methods) {
		for (final IMethod candidate : methods) {
			if (candidate.isSimilar(method))
				return candidate;
		}
		return null;
	}
}
