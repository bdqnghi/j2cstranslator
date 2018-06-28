package com.ilog.translator.java2cs.translation.astrewriter.astchange;

import java.util.ArrayList;
import java.util.Arrays;
import java.util.Collection;
import java.util.HashMap;
import java.util.HashSet;
import java.util.Iterator;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;
import java.util.Set;

import org.eclipse.core.runtime.Assert;
import org.eclipse.core.runtime.CoreException;
import org.eclipse.core.runtime.IProgressMonitor;
import org.eclipse.core.runtime.NullProgressMonitor;
import org.eclipse.core.runtime.SubProgressMonitor;
import org.eclipse.jdt.core.ICompilationUnit;
import org.eclipse.jdt.core.IField;
import org.eclipse.jdt.core.IJavaElement;
import org.eclipse.jdt.core.IJavaProject;
import org.eclipse.jdt.core.IMethod;
import org.eclipse.jdt.core.IType;
import org.eclipse.jdt.core.ITypeHierarchy;
import org.eclipse.jdt.core.ITypeParameter;
import org.eclipse.jdt.core.JavaModelException;
import org.eclipse.jdt.core.NamingConventions;
import org.eclipse.jdt.core.Signature;
import org.eclipse.jdt.core.dom.AST;
import org.eclipse.jdt.core.dom.ASTNode;
import org.eclipse.jdt.core.dom.ASTParser;
import org.eclipse.jdt.core.dom.ASTVisitor;
import org.eclipse.jdt.core.dom.AbstractTypeDeclaration;
import org.eclipse.jdt.core.dom.AnonymousClassDeclaration;
import org.eclipse.jdt.core.dom.Assignment;
import org.eclipse.jdt.core.dom.Block;
import org.eclipse.jdt.core.dom.ClassInstanceCreation;
import org.eclipse.jdt.core.dom.CompilationUnit;
import org.eclipse.jdt.core.dom.ConstructorInvocation;
import org.eclipse.jdt.core.dom.EnumDeclaration;
import org.eclipse.jdt.core.dom.Expression;
import org.eclipse.jdt.core.dom.FieldAccess;
import org.eclipse.jdt.core.dom.FieldDeclaration;
import org.eclipse.jdt.core.dom.IBinding;
import org.eclipse.jdt.core.dom.IMethodBinding;
import org.eclipse.jdt.core.dom.ITypeBinding;
import org.eclipse.jdt.core.dom.IVariableBinding;
import org.eclipse.jdt.core.dom.ImportDeclaration;
import org.eclipse.jdt.core.dom.Javadoc;
import org.eclipse.jdt.core.dom.MethodDeclaration;
import org.eclipse.jdt.core.dom.MethodInvocation;
import org.eclipse.jdt.core.dom.Modifier;
import org.eclipse.jdt.core.dom.Name;
import org.eclipse.jdt.core.dom.ParameterizedType;
import org.eclipse.jdt.core.dom.QualifiedName;
import org.eclipse.jdt.core.dom.QualifiedType;
import org.eclipse.jdt.core.dom.SimpleName;
import org.eclipse.jdt.core.dom.SimpleType;
import org.eclipse.jdt.core.dom.SingleVariableDeclaration;
import org.eclipse.jdt.core.dom.Statement;
import org.eclipse.jdt.core.dom.SuperConstructorInvocation;
import org.eclipse.jdt.core.dom.ThisExpression;
import org.eclipse.jdt.core.dom.Type;
import org.eclipse.jdt.core.dom.TypeDeclaration;
import org.eclipse.jdt.core.dom.TypeDeclarationStatement;
import org.eclipse.jdt.core.dom.TypeParameter;
import org.eclipse.jdt.core.dom.VariableDeclarationFragment;
import org.eclipse.jdt.core.dom.Modifier.ModifierKeyword;
import org.eclipse.jdt.core.dom.rewrite.ASTRewrite;
import org.eclipse.jdt.core.dom.rewrite.ListRewrite;
import org.eclipse.jdt.core.refactoring.descriptors.JavaRefactoringDescriptor;
import org.eclipse.jdt.core.search.IJavaSearchConstants;
import org.eclipse.jdt.core.search.SearchMatch;
import org.eclipse.jdt.core.search.SearchPattern;
import org.eclipse.jdt.internal.corext.codemanipulation.CodeGenerationSettings;
import org.eclipse.jdt.internal.corext.codemanipulation.StubUtility;
import org.eclipse.jdt.internal.corext.dom.ASTNodeFactory;
import org.eclipse.jdt.internal.corext.dom.ASTNodes;
import org.eclipse.jdt.internal.corext.dom.Bindings;
import org.eclipse.jdt.internal.corext.dom.ModifierRewrite;
import org.eclipse.jdt.internal.corext.refactoring.Checks;
import org.eclipse.jdt.internal.corext.refactoring.JavaRefactoringArguments;
import org.eclipse.jdt.core.refactoring.descriptors.ConvertAnonymousDescriptor;
//import org.eclipse.jdt.internal.corext.refactoring.JavaRefactoringDescriptorComment;
import org.eclipse.jdt.internal.corext.refactoring.JDTRefactoringDescriptorComment;
import org.eclipse.jdt.internal.corext.refactoring.JavaRefactoringDescriptorUtil;
import org.eclipse.jdt.internal.corext.refactoring.RefactoringCoreMessages;
import org.eclipse.jdt.internal.corext.refactoring.RefactoringScopeFactory;
import org.eclipse.jdt.internal.corext.refactoring.RefactoringSearchEngine2;
import org.eclipse.jdt.internal.corext.refactoring.SearchResultGroup;
import org.eclipse.jdt.internal.corext.refactoring.base.JavaStatusContext;
import org.eclipse.jdt.internal.corext.refactoring.changes.CompilationUnitChange;
import org.eclipse.jdt.internal.corext.refactoring.changes.DynamicValidationRefactoringChange;
import org.eclipse.jdt.internal.corext.refactoring.structure.ASTNodeSearchUtil;
import org.eclipse.jdt.internal.corext.refactoring.structure.CompilationUnitRewrite;
import org.eclipse.jdt.internal.corext.refactoring.structure.ImportRewriteUtil;
import org.eclipse.jdt.internal.corext.refactoring.structure.MemberVisibilityAdjustor;
import org.eclipse.jdt.internal.corext.refactoring.structure.MemberVisibilityAdjustor.OutgoingMemberVisibilityAdjustment;
import org.eclipse.jdt.internal.corext.refactoring.util.JavaElementUtil;
import org.eclipse.jdt.internal.corext.refactoring.util.JavadocUtil;
import org.eclipse.jdt.internal.corext.refactoring.util.RefactoringASTParser;
import org.eclipse.jdt.internal.corext.refactoring.util.ResourceUtil;
import org.eclipse.jdt.internal.corext.refactoring.util.TextChangeManager;
import org.eclipse.jdt.internal.corext.util.JavaModelUtil;
import org.eclipse.jdt.internal.corext.util.JdtFlags;
import org.eclipse.jdt.internal.corext.util.Messages;
import org.eclipse.jdt.internal.corext.util.SearchUtils;
import org.eclipse.jdt.internal.ui.JavaPlugin;
import org.eclipse.jdt.internal.ui.viewsupport.BindingLabelProvider;
import org.eclipse.jdt.ui.CodeGeneration;
import org.eclipse.jdt.ui.JavaElementLabels;
import org.eclipse.ltk.core.refactoring.Change;
import org.eclipse.ltk.core.refactoring.IRefactoringStatusEntryComparator;
import org.eclipse.ltk.core.refactoring.Refactoring;
import org.eclipse.ltk.core.refactoring.RefactoringDescriptor;
import org.eclipse.ltk.core.refactoring.RefactoringStatus;
import org.eclipse.ltk.core.refactoring.RefactoringStatusEntry;
import org.eclipse.ltk.core.refactoring.TextChange;
import org.eclipse.text.edits.TextEditGroup;

import com.ilog.translator.java2cs.translation.ITranslationContext;
import com.ilog.translator.java2cs.translation.util.TranslationUtils;

public final class ConvertNestedToInnerRefactoring extends
		Refactoring {

	private static final String ID_MOVE_INNER = "org.eclipse.jdt.ui.move.inner"; //$NON-NLS-1$

	private static final String ATTRIBUTE_FIELD = "field"; //$NON-NLS-1$

	private static final String ATTRIBUTE_MANDATORY = "mandatory"; //$NON-NLS-1$

	private static final String ATTRIBUTE_POSSIBLE = "possible"; //$NON-NLS-1$

	private static final String ATTRIBUTE_FINAL = "final"; //$NON-NLS-1$

	private static final String ATTRIBUTE_FIELD_NAME = "fieldName"; //$NON-NLS-1$

	private static final String ATTRIBUTE_PARAMETER_NAME = "parameterName"; //$NON-NLS-1$

	@SuppressWarnings("unchecked")
	private static class MemberAccessNodeCollector extends ASTVisitor {

		private final List fFieldAccesses = new ArrayList(0);

		private final ITypeHierarchy fHierarchy;

		private final List fMethodAccesses = new ArrayList(0);

		private final List fSimpleNames = new ArrayList(0);

		MemberAccessNodeCollector(ITypeHierarchy hierarchy) {
			Assert.isNotNull(hierarchy);
			fHierarchy = hierarchy;
		}

		FieldAccess[] getFieldAccesses() {
			return (FieldAccess[]) fFieldAccesses
					.toArray(new FieldAccess[fFieldAccesses.size()]);
		}

		MethodInvocation[] getMethodInvocations() {
			return (MethodInvocation[]) fMethodAccesses
					.toArray(new MethodInvocation[fMethodAccesses.size()]);
		}

		SimpleName[] getSimpleFieldNames() {
			return (SimpleName[]) fSimpleNames
					.toArray(new SimpleName[fSimpleNames.size()]);
		}

		@Override
		public boolean visit(FieldAccess node) {
			final ITypeBinding declaring = ConvertNestedToInnerRefactoring
					.getDeclaringTypeBinding(node);
			if (declaring != null) {
				final IType type = (IType) declaring.getJavaElement();
				if (type != null && fHierarchy.contains(type)) {
					fFieldAccesses.add(node);
				}
			}
			return super.visit(node);
		}

		@Override
		public boolean visit(MethodInvocation node) {
			final ITypeBinding declaring = ConvertNestedToInnerRefactoring
					.getDeclaringTypeBinding(node);
			if (declaring != null) {
				final IType type = (IType) declaring.getJavaElement();
				if (type != null && fHierarchy.contains(type)) {
					fMethodAccesses.add(node);
				}
			}
			return super.visit(node);
		}

		@Override
		public boolean visit(SimpleName node) {
			if (node.getParent() instanceof QualifiedName) {
				final QualifiedName qName = (QualifiedName) node.getParent();
				if (qName.getName() == node) {
					return super.visit(node);
				}
			}
			final IBinding binding = node.resolveBinding();
			if (binding instanceof IVariableBinding) {
				final IVariableBinding variable = (IVariableBinding) binding;
				final ITypeBinding declaring = variable.getDeclaringClass();
				if (variable.isField() && declaring != null) {
					final IType type = (IType) declaring.getJavaElement();
					if (type != null && fHierarchy.contains(type)) {
						fSimpleNames.add(node);
						return false;
					}
				}
			}
			return super.visit(node);
		}

		@Override
		public boolean visit(ThisExpression node) {
			final Name qualifier = node.getQualifier();
			if (qualifier != null) {
				final ITypeBinding binding = qualifier.resolveTypeBinding();
				if (binding != null) {
					final IType type = (IType) binding.getJavaElement();
					if (type != null && fHierarchy.contains(type)) {
						fSimpleNames.add(qualifier);
						return false;
					}
				}
			}
			return super.visit(node);
		}
	}

	private class TypeReferenceQualifier extends ASTVisitor {

		private final TextEditGroup fGroup;

		private final ITypeBinding fTypeBinding;

		public TypeReferenceQualifier(final ITypeBinding type,
				final TextEditGroup group) {
			Assert.isNotNull(type);
			Assert.isNotNull(type.getDeclaringClass());
			fTypeBinding = type;
			fGroup = group;
		}

		@Override
		public boolean visit(final ClassInstanceCreation node) {
			Assert.isNotNull(node);
			if (fCreateInstanceField) {
				final AST ast = node.getAST();
				final Type type = node.getType();
				final ITypeBinding binding = type.resolveBinding();
				if (binding != null
						&& binding.getDeclaringClass() != null
						&& !Bindings.equals(binding, fTypeBinding)
						&& fSourceRewrite.getRoot().findDeclaringNode(binding) != null) {
					//
					final ITypeBinding declaringClass = binding
							.getDeclaringClass();
					if (fTypeBinding == declaringClass) {
						// In that case we do not want to modify the call
						return true;
					}
					//
					if (binding.getName().contains("Anonymous_C")) { // TODO
						return true;
					}
					if (!Modifier.isStatic(binding.getModifiers())) {
						Expression expression = null;
						if (fCodeGenerationSettings.useKeywordThis
								|| fEnclosingInstanceFieldName
										.equals(fNameForEnclosingInstanceConstructorParameter)) {
							final FieldAccess access = ast.newFieldAccess();
							access.setExpression(ast.newThisExpression());
							access
									.setName(ast
											.newSimpleName(fEnclosingInstanceFieldName));
							expression = access;
						} else {
							expression = ast
									.newSimpleName(fEnclosingInstanceFieldName);
						}
						if (node.getExpression() != null) {
							fSourceRewrite.getImportRemover()
									.registerRemovedNode(node.getExpression());
						}
						fSourceRewrite.getASTRewrite().set(node,
								ClassInstanceCreation.EXPRESSION_PROPERTY,
								expression, fGroup);
					} else {
						addTypeQualification(type, fSourceRewrite, fGroup);
					}
				}
			}
			return true;
		}

		@Override
		public boolean visit(final QualifiedType node) {
			Assert.isNotNull(node);
			return false;
		}

		@Override
		public boolean visit(final SimpleType node) {
			Assert.isNotNull(node);
			if (!(node.getParent() instanceof ClassInstanceCreation)) {
				final ITypeBinding binding = node.resolveBinding();
				if (binding != null) {
					final ITypeBinding declaring = binding.getDeclaringClass();
					if (declaring != null
							&& !Bindings.equals(declaring, fTypeBinding
									.getDeclaringClass())
							&& !Bindings.equals(binding, fTypeBinding)
							&& fSourceRewrite.getRoot().findDeclaringNode(
									binding) != null
							&& Modifier.isStatic(binding.getModifiers())) {
						addTypeQualification(node, fSourceRewrite, fGroup);
					}
				}
			}
			return super.visit(node);
		}

		@Override
		public boolean visit(final ThisExpression node) {
			Assert.isNotNull(node);
			final Name name = node.getQualifier();
			if (name != null && name.isSimpleName()) {
				final AST ast = node.getAST();
				Expression expression = null;
				if ((fCodeGenerationSettings.useKeywordThis || fEnclosingInstanceFieldName
						.equals(fNameForEnclosingInstanceConstructorParameter))
						&& node.getParent().getNodeType() != ASTNode.SUPER_CONSTRUCTOR_INVOCATION) {
					final FieldAccess access = ast.newFieldAccess();
					access.setExpression(ast.newThisExpression());
					access.setName(ast
							.newSimpleName(fEnclosingInstanceFieldName));
					expression = access;
				} else if (ConvertNestedToInnerRefactoring.this
						.isInSuperConstructorInvocation(node)/*
																 * node.getParent().getNodeType() ==
																 * ASTNode.SUPER_CONSTRUCTOR_INVOCATION
																 */) {
					expression = ast
							.newSimpleName(fNameForEnclosingInstanceConstructorParameter);
				} else {
					expression = ast.newSimpleName(fEnclosingInstanceFieldName);
				}
				fSourceRewrite.getASTRewrite().replace(node, expression, null);
			}
			return super.visit(node);
		}
	}

	@SuppressWarnings("unchecked")
	private static void addTypeParameters(final CompilationUnit unit,
			final IType type, final Map map) throws JavaModelException {
		Assert.isNotNull(unit);
		Assert.isNotNull(type);
		Assert.isNotNull(map);
		final AbstractTypeDeclaration declaration = ASTNodeSearchUtil
				.getAbstractTypeDeclarationNode(type, unit);
		if (declaration instanceof TypeDeclaration) {
			ITypeBinding binding = null;
			TypeParameter parameter = null;
			for (final Iterator iterator = ((TypeDeclaration) declaration)
					.typeParameters().iterator(); iterator.hasNext();) {
				parameter = (TypeParameter) iterator.next();
				binding = parameter.resolveBinding();
				if (binding != null && !map.containsKey(binding.getKey())) {
					map.put(binding.getKey(), binding);
				}
			}
			/*
			 * not necessary, we remains in the same compilation unit (I hope
			 * ...) final IType declaring = type.getDeclaringType(); if
			 * (declaring != null && !Flags.isStatic(type.getFlags())) {
			 * ConvertNestedToInnerRefactoring.addTypeParameters(unit,
			 * declaring, map); }
			 */
		}
	}

	private static boolean containsStatusEntry(final RefactoringStatus status,
			final RefactoringStatusEntry other) {
		return status.getEntries(new IRefactoringStatusEntryComparator() {

			public final int compare(final RefactoringStatusEntry entry1,
					final RefactoringStatusEntry entry2) {
				return entry1.getMessage().compareTo(entry2.getMessage());
			}
		}, other).length > 0;
	}

	private static AbstractTypeDeclaration findTypeDeclaration(IType enclosing,
			AbstractTypeDeclaration[] declarations) {
		final String typeName = enclosing.getElementName();
		for (final AbstractTypeDeclaration declaration : declarations) {
			if (declaration.getName().getIdentifier().equals(typeName)) {
				return declaration;
			}
		}
		return null;
	}

	@SuppressWarnings("unchecked")
	private static AbstractTypeDeclaration findTypeDeclaration(
			final IType type, CompilationUnit unit) {
		/*
		 * try { if (type.isLocal()) { final AbstractTypeDeclaration[] result =
		 * new AbstractTypeDeclaration[1]; unit.accept(new ASTVisitor() { public
		 * void endVisit(TypeDeclaration node) { if (ASTNodes.getParent(node,
		 * ASTNode.METHOD_DECLARATION) != null) { ITypeBinding enclosing =
		 * ASTNodes.getEnclosingType(node.getParent()); ITypeBinding tB =
		 * node.resolveBinding(); String n2 = type.getFullyQualifiedName();
		 * String n1 = node.getName().getIdentifier(); if (n2.endsWith(n1)) {
		 * result[0] = node; } } } }); if (result[0] != null) { return
		 * result[0]; } } } catch(Exception e) { e.printStackTrace(); }
		 */
		final List types = ConvertNestedToInnerRefactoring
				.getDeclaringTypes(type);
		types.add(type);
		AbstractTypeDeclaration[] declarations = (AbstractTypeDeclaration[]) unit
				.types().toArray(
						new AbstractTypeDeclaration[unit.types().size()]);
		AbstractTypeDeclaration declaration = null;
		for (final Iterator iterator = types.iterator(); iterator.hasNext();) {
			final IType enclosing = (IType) iterator.next();
			declaration = ConvertNestedToInnerRefactoring.findTypeDeclaration(
					enclosing, declarations);
			Assert.isNotNull(declaration);
			declarations = ConvertNestedToInnerRefactoring
					.getAbstractTypeDeclarations(declaration);
		}
		Assert.isNotNull(declaration);
		return declaration;
	}

	@SuppressWarnings("unchecked")
	public static AbstractTypeDeclaration[] getAbstractTypeDeclarations(
			final AbstractTypeDeclaration declaration) {
		int typeCount = 0;
		for (final Iterator iterator = declaration.bodyDeclarations()
				.listIterator(); iterator.hasNext();) {
			if (iterator.next() instanceof AbstractTypeDeclaration) {
				typeCount++;
			}
		}
		final AbstractTypeDeclaration[] declarations = new AbstractTypeDeclaration[typeCount];
		int next = 0;
		for (final Iterator iterator = declaration.bodyDeclarations()
				.listIterator(); iterator.hasNext();) {
			final Object object = iterator.next();
			if (object instanceof AbstractTypeDeclaration) {
				declarations[next++] = (AbstractTypeDeclaration) object;
			}
		}
		return declarations;
	}

	private static ITypeBinding getDeclaringTypeBinding(FieldAccess fieldAccess) {
		final IVariableBinding varBinding = fieldAccess.resolveFieldBinding();
		if (varBinding == null) {
			return null;
		}
		return varBinding.getDeclaringClass();
	}

	private static ITypeBinding getDeclaringTypeBinding(
			MethodInvocation methodInvocation) {
		final IMethodBinding binding = methodInvocation.resolveMethodBinding();
		if (binding == null) {
			return null;
		}
		return binding.getDeclaringClass();
	}

	// List of ITypes
	@SuppressWarnings("unchecked")
	private static List getDeclaringTypes(IType type) {
		final IType declaringType = type.getDeclaringType();
		if (declaringType == null) {
			return new ArrayList(0);
		}
		final List result = ConvertNestedToInnerRefactoring
				.getDeclaringTypes(declaringType);
		result.add(declaringType);
		return result;
	}

	@SuppressWarnings("unchecked")
	private static String[] getFieldNames(IType type) {
		try {
			final IField[] fields = type.getFields();
			final List result = new ArrayList(fields.length);
			for (final IField element : fields) {
				result.add(element.getElementName());
			}
			return (String[]) result.toArray(new String[result.size()]);
		} catch (final JavaModelException e) {
			return null;
		}
	}

	@SuppressWarnings("unchecked")
	private static Set getMergedSet(Set s1, Set s2) {
		final Set result = new HashSet();
		result.addAll(s1);
		result.addAll(s2);
		return result;
	}

	@SuppressWarnings("unchecked")
	private static String[] getParameterNamesOfAllConstructors(IType type)
			throws JavaModelException {
		final IMethod[] constructors = JavaElementUtil.getAllConstructors(type);
		final Set result = new HashSet();
		for (final IMethod element : constructors) {
			result.addAll(Arrays.asList(element.getParameterNames()));
		}
		return (String[]) result.toArray(new String[result.size()]);
	}

	@SuppressWarnings("unchecked")
	private static ASTNode[] getReferenceNodesIn(CompilationUnit cuNode,
			Map references, ICompilationUnit cu) {
		final SearchMatch[] results = (SearchMatch[]) references.get(cu);
		if (results == null) {
			return new ASTNode[0];
		}
		return ASTNodeSearchUtil.getAstNodes(results, cuNode);
	}

	private static boolean isCorrespondingTypeBinding(ITypeBinding binding,
			IType type) {
		if (binding == null) {
			return false;
		}
		return Bindings.getFullyQualifiedName(binding).equals(
				JavaElementUtil.createSignature(type));
	}

	/*
	 * private static boolean isStatic(FieldAccess access) { IVariableBinding
	 * fieldBinding = access.resolveFieldBinding(); if (fieldBinding == null)
	 * return false; return JdtFlags.isStatic(fieldBinding); }
	 * 
	 * private static boolean isStatic(MethodInvocation invocation) {
	 * IMethodBinding methodBinding = invocation.resolveMethodBinding(); if
	 * (methodBinding == null) return false; return
	 * JdtFlags.isStatic(methodBinding); }
	 * 
	 * private static boolean isStaticFieldName(SimpleName name) { IBinding
	 * binding = name.resolveBinding(); if (!(binding instanceof
	 * IVariableBinding)) return false; IVariableBinding variableBinding =
	 * (IVariableBinding) binding; if (!variableBinding.isField()) return false;
	 * return JdtFlags.isStatic(variableBinding); }
	 */

	private TextChangeManager fChangeManager;

	private CodeGenerationSettings fCodeGenerationSettings;

	private boolean fCreateInstanceField;

	private String fEnclosingInstanceFieldName;

	private boolean fIsInstanceFieldCreationMandatory;

	private boolean fIsInstanceFieldCreationPossible;

	private boolean fMarkInstanceFieldAsFinal;

	private String fNameForEnclosingInstanceConstructorParameter;

	private String fNewSourceOfInputType;

	private CompilationUnitRewrite fSourceRewrite;

	@SuppressWarnings("unchecked")
	private Collection fStaticImports;

	private IType fType;

	// private String fQualifiedTypeName;

	@SuppressWarnings("unchecked")
	private Collection fTypeImports;

	private boolean fEnclosingIsSuperOfInner;

	// Ok the funny case : If the considering inner is a renamed anonymous AND
	// the super class is a nested transformed into an inner via that
	// refactoring
	// we need to modify constructor ... ouf !
	// I know that is not necessary a good idea to use that class to do that,
	// but the fact
	// is that it do the job perfectly !
	private boolean fRefEnclosing;

	private final boolean fNeedOuterField;

	private final ITranslationContext fContext;

	/**
	 * Creates a new move inner to top refactoring.
	 * 
	 * @param type
	 *            the type, or <code>null</code> if invoked by scripting
	 * @param settings
	 *            the code generation settings, or <code>null</code> if
	 *            invoked by scripting
	 * @throws JavaModelException
	 */
	public ConvertNestedToInnerRefactoring(IType type,
			CodeGenerationSettings settings, boolean enclosingIsSuperOfInner,
			boolean refEnclosing, boolean needOuterField,
			ITranslationContext context) throws JavaModelException {
		fType = type;
		fCodeGenerationSettings = settings;
		fMarkInstanceFieldAsFinal = true; // default
		fEnclosingIsSuperOfInner = enclosingIsSuperOfInner;
		fRefEnclosing = refEnclosing;
		fNeedOuterField = needOuterField;
		fContext = context;
		if (fType != null) {
			this.initialize();
		}
	}

	private void initialize() throws JavaModelException {
		/*
		 * fQualifiedTypeName = JavaModelUtil.concatenateName(fType
		 * .getPackageFragment().getElementName(), /* TODO
		 * fType.getDeclaringType().getElementName() + "_" +*
		 * fType.getElementName());
		 */
		fEnclosingInstanceFieldName = getInitialNameForEnclosingInstanceField();
		fSourceRewrite = new CompilationUnitRewrite(fType.getCompilationUnit());
		fIsInstanceFieldCreationPossible = !(/*
												 * ZORGLUB:
												 * JdtFlags.isStatic(this.fType) ||
												 */fType.isAnnotation() || fType.isEnum());
		fIsInstanceFieldCreationMandatory = true; /*
													 * fIsInstanceFieldCreationPossible &&
													 * isInstanceFieldCreationMandatory();
													 */
		fCreateInstanceField = fIsInstanceFieldCreationMandatory;
	}

	@SuppressWarnings("unchecked")
	private void addEnclosingInstanceDeclaration(
			final AbstractTypeDeclaration declaration, final ASTRewrite rewrite)
			throws CoreException {
		Assert.isNotNull(declaration);
		Assert.isNotNull(rewrite);
		final AST ast = declaration.getAST();
		final VariableDeclarationFragment fragment = ast
				.newVariableDeclarationFragment();
		fragment.setName(ast.newSimpleName(fEnclosingInstanceFieldName));
		final FieldDeclaration newField = ast.newFieldDeclaration(fragment);
		newField.modifiers().addAll(
				ASTNodeFactory.newModifiers(ast,
						getEnclosingInstanceAccessModifiers()));
		newField.setType(createEnclosingType(ast));
		final String comment = null; /*
										 * CodeGeneration.getFieldComment(fType
										 * .getCompilationUnit(),
										 * declaration.getName().getIdentifier(),
										 * fEnclosingInstanceFieldName,
										 * StubUtility
										 * .getLineDelimiterUsed(fType.getJavaProject()));
										 */
		if (comment != null && comment.length() > 0) {
			final Javadoc doc = (Javadoc) rewrite.createStringPlaceholder(
					comment, ASTNode.JAVADOC);
			newField.setJavadoc(doc);
		}
		rewrite.getListRewrite(declaration,
				declaration.getBodyDeclarationsProperty()).insertFirst(
				newField, null);
	}

	@SuppressWarnings("unchecked")
	private void addEnclosingInstanceTypeParameters(
			final ITypeBinding[] parameters,
			final AbstractTypeDeclaration declaration, final ASTRewrite rewrite) {
		Assert.isNotNull(parameters);
		Assert.isNotNull(declaration);
		Assert.isNotNull(rewrite);
		if (declaration instanceof TypeDeclaration) {
			final TypeDeclaration type = (TypeDeclaration) declaration;
			final List existing = type.typeParameters();
			final Set names = new HashSet();
			TypeParameter parameter = null;
			for (final Iterator iterator = existing.iterator(); iterator
					.hasNext();) {
				parameter = (TypeParameter) iterator.next();
				names.add(parameter.getName().getIdentifier());
			}
			final ListRewrite rewriter = rewrite.getListRewrite(type,
					TypeDeclaration.TYPE_PARAMETERS_PROPERTY);
			String name = null;
			for (final ITypeBinding element : parameters) {
				name = element.getName();
				if (!names.contains(name)) {
					parameter = type.getAST().newTypeParameter();
					parameter.setName(type.getAST().newSimpleName(name));
					rewriter.insertLast(parameter, null);
				}
			}
		}
	}

	/*
	 * private void addImportsToTargetUnit(final ICompilationUnit targetUnit,
	 * final IProgressMonitor monitor) throws CoreException, JavaModelException {
	 * monitor.beginTask("", 2); //$NON-NLS-1$ try { ImportRewrite rewrite =
	 * StubUtility ./* CompilationUnitRewrite. /createImportRewrite( targetUnit,
	 * true); if (fTypeImports != null) { ITypeBinding type = null; for (final
	 * Iterator iterator = fTypeImports.iterator(); iterator .hasNext();) { type =
	 * (ITypeBinding) iterator.next(); if (type.isNested()) { while
	 * (type.isNested()) { type = type.getDeclaringClass(); } String fqn =
	 * type.getQualifiedName(); rewrite.addImport(fqn); rewrite.addImport(fqn +
	 * ".*"); int index = fqn.lastIndexOf("."); // TODO just to be // sure ? if
	 * (index >= 0) { String name = fqn.substring(0, index);
	 * rewrite.addImport(name + ".*"); } rewrite.addImport(fqn + ".*"); } else
	 * rewrite.addImport(type); } } if (fStaticImports != null) { IBinding
	 * binding = null; for (final Iterator iterator = fStaticImports.iterator();
	 * iterator .hasNext();) { binding = (IBinding) iterator.next();
	 * rewrite.addStaticImport(binding); } } fTypeImports = null; fStaticImports =
	 * null; TextEdit edits = rewrite.rewriteImports(new SubProgressMonitor(
	 * monitor, 1)); JavaModelUtil.applyEdit(targetUnit, edits, false, new
	 * SubProgressMonitor(monitor, 1)); } finally { monitor.done(); } }
	 */

	@SuppressWarnings("unchecked")
	private void addInheritedTypeQualifications(
			final AbstractTypeDeclaration declaration,
			final CompilationUnitRewrite targetRewrite,
			final TextEditGroup group) {
		Assert.isNotNull(declaration);
		Assert.isNotNull(targetRewrite);
		final CompilationUnit unit = (CompilationUnit) declaration.getRoot();
		final ITypeBinding binding = declaration.resolveBinding();
		if (binding != null) {
			Type type = null;
			if (declaration instanceof TypeDeclaration) {
				type = ((TypeDeclaration) declaration).getSuperclassType();
				if (type != null && unit.findDeclaringNode(binding) != null) {
					addTypeQualification(type, targetRewrite, group);
				}
			}
			List types = null;
			if (declaration instanceof TypeDeclaration) {
				types = ((TypeDeclaration) declaration).superInterfaceTypes();
			} else if (declaration instanceof EnumDeclaration) {
				types = ((EnumDeclaration) declaration).superInterfaceTypes();
			}
			if (types != null) {
				for (final Iterator iterator = types.iterator(); iterator
						.hasNext();) {
					type = (Type) iterator.next();
					if (unit.findDeclaringNode(type.resolveBinding()) != null) {
						addTypeQualification(type, targetRewrite, group);
					}
				}
			}
		}
	}

	private void addParameterToConstructor(final ASTRewrite rewrite,
			final MethodDeclaration declaration) throws JavaModelException {
		Assert.isNotNull(rewrite);
		Assert.isNotNull(declaration);
		final AST ast = declaration.getAST();
		final String name = getNameForEnclosingInstanceConstructorParameter();
		final SingleVariableDeclaration variable = ast
				.newSingleVariableDeclaration();
		variable.setType(createEnclosingType(ast));
		variable.setName(ast.newSimpleName(name));
		rewrite.getListRewrite(declaration,
				MethodDeclaration.PARAMETERS_PROPERTY).insertFirst(variable,
				null);
		JavadocUtil.addParamJavadoc(name, declaration, rewrite, fType
				.getJavaProject(), null);
	}

	private void addSimpleTypeQualification(
			final CompilationUnitRewrite targetRewrite,
			final ITypeBinding declaring, final SimpleType simpleType,
			final TextEditGroup group) {
		Assert.isNotNull(targetRewrite);
		Assert.isNotNull(declaring);
		Assert.isNotNull(simpleType);
		final AST ast = targetRewrite.getRoot().getAST();
		if (!(simpleType.getName() instanceof QualifiedName)) {
			targetRewrite.getASTRewrite().replace(
					simpleType,
					ast.newQualifiedType(targetRewrite.getImportRewrite()
							.addImport(declaring, ast), ast
							.newSimpleName(simpleType.getName()
									.getFullyQualifiedName())), group);
			targetRewrite.getImportRemover().registerRemovedNode(simpleType);
		}
	}

	private void addTypeQualification(final Type type,
			final CompilationUnitRewrite targetRewrite,
			final TextEditGroup group) {
		Assert.isNotNull(type);
		Assert.isNotNull(targetRewrite);
		final ITypeBinding binding = type.resolveBinding();
		if (binding != null) {
			final ITypeBinding declaring = binding.getDeclaringClass();
			if (declaring != null) {
				if (type instanceof SimpleType) {
					final SimpleType simpleType = (SimpleType) type;
					addSimpleTypeQualification(targetRewrite, declaring,
							simpleType, group);
				} else if (type instanceof ParameterizedType) {
					final ParameterizedType parameterizedType = (ParameterizedType) type;
					final Type rawType = parameterizedType.getType();
					if (rawType instanceof SimpleType) {
						addSimpleTypeQualification(targetRewrite, declaring,
								(SimpleType) rawType, group);
					}
				}
			}
		}
	}

	@SuppressWarnings("unchecked")
	private RefactoringStatus checkConstructorParameterNames() {
		final RefactoringStatus result = new RefactoringStatus();
		final CompilationUnit cuNode = new RefactoringASTParser(AST.JLS3)
				.parse(fType.getCompilationUnit(), false);
		final MethodDeclaration[] nodes = getConstructorDeclarationNodes(ConvertNestedToInnerRefactoring
				.findTypeDeclaration(fType, cuNode));
		for (final MethodDeclaration constructor : nodes) {
			for (final Iterator iter = constructor.parameters().iterator(); iter
					.hasNext();) {
				final SingleVariableDeclaration param = (SingleVariableDeclaration) iter
						.next();
				if (fEnclosingInstanceFieldName.equals(param.getName()
						.getIdentifier())) {
					final String msg = Messages
							.format(
									RefactoringCoreMessages.MoveInnerToTopRefactoring_name_used,
									new String[] {
											param.getName().getIdentifier(),
											fType.getElementName() });
					result.addError(msg, JavaStatusContext.create(fType
							.getCompilationUnit(), param));
				}
			}
		}
		return result;
	}

	public RefactoringStatus checkEnclosingInstanceName(String name) {
		if (!fCreateInstanceField) {
			return new RefactoringStatus();
		}
		RefactoringStatus result = Checks.checkFieldName(name, this.fType); // TODO: not sure ...
		if (!Checks.startsWithLowerCase(name)) {
			result
					.addWarning(RefactoringCoreMessages.MoveInnerToTopRefactoring_names_start_lowercase);
		}

		if (fType.getField(name).exists()) {
			final Object[] keys = new String[] { name, fType.getElementName() };
			final String msg = Messages
					.format(
							RefactoringCoreMessages.MoveInnerToTopRefactoring_already_declared,
							keys);
			result
					.addError(msg, JavaStatusContext.create(fType
							.getField(name)));
		}
		return result;
	}

	@Override
	public RefactoringStatus checkFinalConditions(IProgressMonitor pm)
			throws CoreException {
		pm.beginTask("", 2);//$NON-NLS-1$
		try {
			final RefactoringStatus result = new RefactoringStatus();

			/*
			 * ZORGLUB: if (JdtFlags.isStatic(this.fType)) { result
			 * .merge(this.checkEnclosingInstanceName(this.fEnclosingInstanceFieldName)); }
			 */

			if (fType.getPackageFragment().getCompilationUnit(
					(JavaModelUtil.getRenamedCUName(fType.getCompilationUnit(),
							fType.getElementName()))).exists()) {
				final String message = Messages
						.format(
								RefactoringCoreMessages.MoveInnerToTopRefactoring_compilation_Unit_exists,
								new String[] {
										(JavaModelUtil.getRenamedCUName(fType
												.getCompilationUnit(), fType
												.getElementName())),
										fType.getPackageFragment()
												.getElementName() });
				result.addFatalError(message);
			}
			result
					.merge(checkEnclosingInstanceName(fEnclosingInstanceFieldName));
			result.merge(Checks.checkCompilationUnitName((JavaModelUtil
					.getRenamedCUName(fType.getCompilationUnit(), fType
							.getElementName())), this.fType)); // TODO: Not sure
			result.merge(checkConstructorParameterNames());
			result.merge(checkTypeNameInPackage());
			fChangeManager = createChangeManager(new SubProgressMonitor(pm, 1),
					result);
			result.merge(Checks.validateModifiesFiles(ResourceUtil
					.getFiles(fChangeManager.getAllCompilationUnits()),
					getValidationContext()));
			return result;
		} finally {
			pm.done();
		}
	}

	@Override
	public RefactoringStatus checkInitialConditions(IProgressMonitor monitor)
			throws CoreException {
		return Checks.checkIfCuBroken(fType);
	}

	private RefactoringStatus checkTypeNameInPackage()
			throws JavaModelException {
		final IType type = Checks.findTypeInPackage(fType.getPackageFragment(),
				fType.getElementName());
		if (type == null || !type.exists()) {
			return null;
		}
		final String message = Messages.format(
				RefactoringCoreMessages.MoveInnerToTopRefactoring_type_exists,
				new String[] { fType.getElementName(),
						fType.getPackageFragment().getElementName() });
		return RefactoringStatus.createErrorStatus(message);
	}

	private Expression createAccessExpressionToEnclosingInstanceFieldText(
			ASTNode node, IBinding binding, AbstractTypeDeclaration declaration) {
		if (Modifier.isStatic(binding.getModifiers())) {
			return node.getAST().newName(
					TranslationUtils
							.getTypeQualifiedName(fType.getDeclaringType()));
		} else if (this.isInSuperConstructorInvocation(node, declaration)) {
			return this.createReadAccessExpressionForEnclosingInstance(node
					.getAST(), true);
		} else if (isInThisConstructorInvocation(node, declaration)) {
			return this.createReadAccessExpressionForEnclosingInstance(node
					.getAST(), true);
		} else if ((isInAnonymousTypeInsideInputType(node, declaration)
				|| isInLocalTypeInsideInputType(node, declaration) || isInNonStaticMemberTypeInsideInputType(
				node, declaration))) {
			return createQualifiedReadAccessExpressionForEnclosingInstance(node
					.getAST());
		} else {
			return this.createReadAccessExpressionForEnclosingInstance(node
					.getAST());
		}
	}

	@Override
	@SuppressWarnings("unchecked")
	public Change createChange(final IProgressMonitor monitor)
			throws CoreException {
		monitor
				.beginTask(
						RefactoringCoreMessages.MoveInnerToTopRefactoring_creating_change,
						1);
		final Map arguments = new HashMap();
		String project = null;
		final IJavaProject javaProject = fType.getJavaProject();
		if (javaProject != null) {
			project = javaProject.getElementName();
		}
		final String description = Messages
				.format(
						RefactoringCoreMessages.MoveInnerToTopRefactoring_descriptor_description_short,
						fType.getElementName());
		final String header = Messages
				.format(
						RefactoringCoreMessages.MoveInnerToTopRefactoring_descriptor_description,
						new String[] {
								JavaElementLabels.getElementLabel(fType,
										JavaElementLabels.ALL_FULLY_QUALIFIED),
								JavaElementLabels.getElementLabel(fType
										.getParent(),
										JavaElementLabels.ALL_FULLY_QUALIFIED) });
		final JDTRefactoringDescriptorComment comment = new JDTRefactoringDescriptorComment(
				"id", this, header);
		comment
				.addSetting(Messages
						.format(
								RefactoringCoreMessages.MoveInnerToTopRefactoring_original_pattern,
								JavaElementLabels.getElementLabel(fType,
										JavaElementLabels.ALL_FULLY_QUALIFIED)));
		final boolean enclosing = fEnclosingInstanceFieldName != null
				&& !"".equals(fEnclosingInstanceFieldName); //$NON-NLS-1$
		if (enclosing) {
			comment
					.addSetting(Messages
							.format(
									RefactoringCoreMessages.MoveInnerToTopRefactoring_field_pattern,
									fEnclosingInstanceFieldName));
		}
		if (fNameForEnclosingInstanceConstructorParameter != null
				&& !"".equals(fNameForEnclosingInstanceConstructorParameter)) {
			comment
					.addSetting(Messages
							.format(
									RefactoringCoreMessages.MoveInnerToTopRefactoring_parameter_pattern,
									fNameForEnclosingInstanceConstructorParameter));
		}
		if (enclosing && fMarkInstanceFieldAsFinal) {
			comment
					.addSetting(RefactoringCoreMessages.MoveInnerToTopRefactoring_declare_final);
		}
		final ConvertAnonymousDescriptor descriptor = new ConvertAnonymousDescriptor(project, description, comment.asString(),
				arguments, RefactoringDescriptor.MULTI_CHANGE
						| RefactoringDescriptor.STRUCTURAL_CHANGE
						| JavaRefactoringDescriptor.JAR_REFACTORING
						| JavaRefactoringDescriptor.JAR_SOURCE_ATTACHMENT);
		arguments.put(JavaRefactoringDescriptorUtil.ATTRIBUTE_INPUT, JavaRefactoringDescriptorUtil
				.elementToHandle(project, this.fType));
		if (enclosing) {
			arguments.put(ConvertNestedToInnerRefactoring.ATTRIBUTE_FIELD_NAME,
					fEnclosingInstanceFieldName);
		}
		if (fNameForEnclosingInstanceConstructorParameter != null
				&& !"".equals(fNameForEnclosingInstanceConstructorParameter)) {
			arguments.put(
					ConvertNestedToInnerRefactoring.ATTRIBUTE_PARAMETER_NAME,
					fNameForEnclosingInstanceConstructorParameter);
		}
		arguments.put(ConvertNestedToInnerRefactoring.ATTRIBUTE_FIELD, Boolean
				.valueOf(fCreateInstanceField).toString());
		arguments.put(ConvertNestedToInnerRefactoring.ATTRIBUTE_FINAL, Boolean
				.valueOf(fMarkInstanceFieldAsFinal).toString());
		arguments.put(ConvertNestedToInnerRefactoring.ATTRIBUTE_POSSIBLE,
				Boolean.valueOf(fIsInstanceFieldCreationPossible).toString());
		arguments.put(ConvertNestedToInnerRefactoring.ATTRIBUTE_MANDATORY,
				Boolean.valueOf(fIsInstanceFieldCreationMandatory).toString());
		final DynamicValidationRefactoringChange result = new DynamicValidationRefactoringChange(
				descriptor,
				RefactoringCoreMessages.MoveInnerToTopRefactoring_move_to_Top);
		result.addAll(fChangeManager.getAllChanges());
		/*
		 * result.add(createCompilationUnitForMovedType(new SubProgressMonitor(
		 * monitor, 1)));
		 */
		return result;
	}

	@SuppressWarnings("unchecked")
	private TextChangeManager createChangeManager(
			final IProgressMonitor monitor, final RefactoringStatus status)
			throws CoreException {
		Assert.isNotNull(monitor);
		Assert.isNotNull(status);
		final TextChangeManager manager = new TextChangeManager();
		try {
			monitor
					.beginTask(
							RefactoringCoreMessages.MoveInnerToTopRefactoring_creating_preview,
							4);
			final Map rewrites = new HashMap(2);
			fSourceRewrite.clearASTAndImportRewrites();
			rewrites.put(fSourceRewrite.getCu(), fSourceRewrite);
			final MemberVisibilityAdjustor2 adjustor = new MemberVisibilityAdjustor2(
					fType.getPackageFragment(), fType);
			adjustor.setRewrites(rewrites);
			adjustor.setVisibilitySeverity(RefactoringStatus.WARNING);
			adjustor.setFailureSeverity(RefactoringStatus.WARNING);
			adjustor.setStatus(status);

			//
			// TODO:
			// adjustor.setOutgoing(false); // bugs, see IlrMergeWizard !
			adjustor.setFailureSeverity(RefactoringStatus.WARNING);
			// sometimes class members are inaccessible :-(
			// TODO: see impact on generation !
			// Unfortunately IlrModelSAXHandler can't be translated ....
			//
			try {
				adjustor.adjustVisibility(new SubProgressMonitor(monitor, 1));
			} catch (final JavaModelException e) {
				e.printStackTrace();
				adjustor.setOutgoing(false);
				adjustor.adjustVisibility(new SubProgressMonitor(monitor, 1));
			}

			final Map parameters = new LinkedHashMap();
			ConvertNestedToInnerRefactoring.addTypeParameters(fSourceRewrite
					.getRoot(), fType, parameters);
			final ITypeBinding[] bindings = new ITypeBinding[parameters
					.values().size()];
			parameters.values().toArray(bindings);
			final Map typeReferences = createTypeReferencesMapping(
					new SubProgressMonitor(monitor, 1), status);
			Map constructorReferences = null;
			/*
			 * ZORGLUB: if (JdtFlags.isStatic(this.fType)) {
			 * constructorReferences = new HashMap(0); } else {
			 */
			constructorReferences = createConstructorReferencesMapping(
					new SubProgressMonitor(monitor, 1), status);
			// }
			if (fCreateInstanceField) {
				// must increase visibility of all member types up
				// to the top level type to allow this
				IType type = fType;
				final ModifierKeyword keyword = null;
				while ((type = type.getDeclaringType()) != null) {
					if ((!adjustor.getAdjustments().containsKey(type))
							&& (Modifier.isPrivate(type.getFlags()))) {
						adjustor
								.getAdjustments()
								.put(
										type,
										new OutgoingMemberVisibilityAdjustment(
												type,
												keyword,
												RefactoringStatus
														.createWarningStatus(
																Messages
																		.format(
																				RefactoringCoreMessages.MemberVisibilityAdjustor_change_visibility_type_warning,
																				new String[] {
																						MemberVisibilityAdjustor
																								.getLabel(type),
																						MemberVisibilityAdjustor
																								.getLabel(keyword) }),
																JavaStatusContext
																		.create(
																				type
																						.getCompilationUnit(),
																				type
																						.getSourceRange()))));
					}
				}
			}
			monitor.worked(1);
			for (final Iterator iterator = ConvertNestedToInnerRefactoring
					.getMergedSet(typeReferences.keySet(),
							constructorReferences.keySet()).iterator(); iterator
					.hasNext();) {
				final ICompilationUnit unit = (ICompilationUnit) iterator
						.next();
				final CompilationUnitRewrite targetRewrite = getCompilationUnitRewrite(unit);
				createCompilationUnitRewrite(bindings, targetRewrite,
						typeReferences, constructorReferences, adjustor
								.getAdjustments().containsKey(fType), fType
								.getCompilationUnit(), unit, false, status,
						monitor);
				if (unit.equals(fType.getCompilationUnit())) {
					try {
						adjustor.setStatus(new RefactoringStatus());
						adjustor.rewriteVisibility(targetRewrite.getCu(),
								new SubProgressMonitor(monitor, 1));
					} finally {
						adjustor.setStatus(status);
					}
					fNewSourceOfInputType = createNewSource(targetRewrite, unit); // source
					// of
					// rewrite
					// inner
					// class
					targetRewrite.clearASTAndImportRewrites();
					// remove the "old inner class"
					createCompilationUnitRewrite(bindings, targetRewrite,
							typeReferences, constructorReferences, adjustor
									.getAdjustments().containsKey(fType), fType
									.getCompilationUnit(), unit, true, status,
							monitor);
				}
				adjustor.rewriteVisibility(targetRewrite.getCu(),
						new SubProgressMonitor(monitor, 1));
				manager.manage(unit, targetRewrite.createChange());
			}
		} catch (final Exception e) {
			System.err.println("Something goes wrong ! " + e);
			e.printStackTrace();
		} finally {
			monitor.done();
		}
		return manager;
	}

	@SuppressWarnings("unchecked")
	private void createCompilationUnitRewrite(final ITypeBinding[] parameters,
			final CompilationUnitRewrite targetRewrite,
			final Map typeReferences, final Map constructorReferences,
			boolean visibilityWasAdjusted, final ICompilationUnit sourceUnit,
			final ICompilationUnit targetUnit, final boolean remove,
			final RefactoringStatus status, final IProgressMonitor monitor)
			throws CoreException {
		Assert.isNotNull(parameters);
		Assert.isNotNull(targetRewrite);
		Assert.isNotNull(typeReferences);
		Assert.isNotNull(constructorReferences);
		Assert.isNotNull(sourceUnit);
		Assert.isNotNull(targetUnit);
		final CompilationUnit root = targetRewrite.getRoot();
		final ASTRewrite rewrite = targetRewrite.getASTRewrite();
		if (targetUnit.equals(sourceUnit)) {
			final AbstractTypeDeclaration declaration = ConvertNestedToInnerRefactoring
					.findTypeDeclaration(fType, root);
			final TextEditGroup qualifierGroup = fSourceRewrite
					.createGroupDescription(RefactoringCoreMessages.MoveInnerToTopRefactoring_change_qualifier);
			final ITypeBinding binding = declaration.resolveBinding();
			/*
			 * TODO rewrite.replace(declaration.getName(),
			 * declaration.getAST().newSimpleName(fType.getDeclaringType().getElementName() +
			 * "_" + fType.getElementName()) , null);
			 */
			if (!remove) {
				// ZORGLUB: if (!JdtFlags.isStatic(this.fType) /* &&
				// fCreateInstanceField */) {
				if (JavaElementUtil.getAllConstructors(fType).length == 0) {
					createConstructor(declaration, rewrite);
				} else {
					modifyConstructors(declaration, rewrite);
				}
				addInheritedTypeQualifications(declaration, targetRewrite,
						qualifierGroup);
				if (fNeedOuterField) {
					addEnclosingInstanceDeclaration(declaration, rewrite);
				}
				/*
				 * ZORGLUB: } else if (!JdtFlags.isStatic(this.fType)) {
				 * this.modifyConstructors(declaration, rewrite); // If super
				 * constructor has enclosing as parameter // add it at first. //
				 * see later ... addSuperConstructorInvok(declaration, //
				 * rewrite); }
				 */
				fTypeImports = new HashSet();
				fStaticImports = new HashSet();
				ImportRewriteUtil.collectImports(fType.getJavaProject(),
						declaration, fTypeImports, fStaticImports, false);
				if (binding != null) {
					fTypeImports.remove(binding);
				}
			}
			if (fRefEnclosing) {
				boolean hasGenericInParent = false;
				//
				//
				if (binding.getSuperclass() != null) {
					final ITypeBinding[] typeParamsOfSuper = binding
							.getSuperclass().getTypeParameters();
					if (typeParamsOfSuper.length > 0) {
						hasGenericInParent = true;
					}
				}
				if (binding.getInterfaces().length > 0) {
					for (final ITypeBinding interf : binding.getInterfaces()) {
						// ITypeBinding[] typeParamsOfSuper =
						// interf.getTypeParameters();
						final ITypeBinding[] typesArgsOfSuper = interf
								.getTypeArguments();
						if (typesArgsOfSuper.length > 0) {
							for (final ITypeBinding enclosingParameter : parameters) {
								for (final ITypeBinding typeArgsOfSuper : typesArgsOfSuper) {
									if (enclosingParameter
											.isEqualTo(typeArgsOfSuper)) {
										hasGenericInParent = true;
									}
								}
							}
						}
					}
				}
				if (!hasGenericInParent)
					addEnclosingInstanceTypeParameters(parameters, declaration,
							rewrite);
				//
				//
				modifyAccessToEnclosingInstance(targetRewrite, declaration,
						status, monitor);
			}
			if (binding != null) {
				modifyInterfaceMemberModifiers(binding);
				final ITypeBinding declaring = binding.getDeclaringClass();
				if (declaring != null) {
					declaration
							.accept(new TypeReferenceQualifier(binding, null));
				} else {
					System.err.println("trouble with declaring ");
				}
			} else {
				System.err.println("trouble with binding");
			}
			final TextEditGroup groupMove = targetRewrite
					.createGroupDescription(RefactoringCoreMessages.MoveInnerToTopRefactoring_change_label);
			if (remove) {
				rewrite.replace(declaration, rewrite.createStringPlaceholder(
						fNewSourceOfInputType, declaration.getNodeType()),
						groupMove);
				/*
				 * rewrite.remove(declaration, groupMove);
				 * targetRewrite.getImportRemover().registerRemovedNode(
				 * declaration);
				 */
			} else {
				// Bug 101017/96308: Rewrite the visibility of the element to be
				// moved and add a warning.

				// Note that this cannot be done in the
				// MemberVisibilityAdjustor, as the private and
				// static flags must always be cleared when moving to new type.

				int newFlags = declaration.getModifiers(); /*
															 * JdtFlags.clearFlag(Modifier.STATIC,
															 * declaration
															 * .getModifiers());
															 */

				if (!visibilityWasAdjusted) {
					if (Modifier.isPrivate(declaration.getModifiers())
							|| Modifier.isProtected(declaration.getModifiers())) {
						newFlags = JdtFlags.clearFlag(Modifier.PROTECTED
								| Modifier.PRIVATE, newFlags);
						newFlags |= Modifier.PUBLIC;
						final RefactoringStatusEntry entry = new RefactoringStatusEntry(
								RefactoringStatus.WARNING,
								Messages
										.format(
												RefactoringCoreMessages.MoveInnerToTopRefactoring_change_visibility_type_warning,
												new String[] { BindingLabelProvider
														.getBindingLabel(
																binding,
																JavaElementLabels.ALL_FULLY_QUALIFIED) }),
								JavaStatusContext
										.create(fSourceRewrite.getCu()));
						if (!ConvertNestedToInnerRefactoring
								.containsStatusEntry(status, entry)) {
							status.addEntry(entry);
						}
					} else if (!Modifier.isPublic(declaration.getModifiers())) {
						// So it's package
						final List listmod = ModifierRewrite.create(rewrite,
								declaration).getModifierRewrite()
								.getRewrittenList();
						boolean hasPublic = false;
						for (int i = 0; i < listmod.size(); i++) {
							final ASTNode node = (ASTNode) listmod.get(i);
							if (node.getNodeType() == ASTNode.MODIFIER) {
								final Modifier m = (Modifier) node;
								if (Modifier.isPublic(m.getKeyword()
										.toFlagValue())) {
									hasPublic = true;
								}
							}
						}
						if (!hasPublic) {
							newFlags |= Modifier.PUBLIC;
							final RefactoringStatusEntry entry = new RefactoringStatusEntry(
									RefactoringStatus.WARNING,
									Messages
											.format(
													RefactoringCoreMessages.MoveInnerToTopRefactoring_change_visibility_type_warning,
													new String[] { BindingLabelProvider
															.getBindingLabel(
																	binding,
																	JavaElementLabels.ALL_FULLY_QUALIFIED) }),
									JavaStatusContext.create(fSourceRewrite
											.getCu()));
							if (!ConvertNestedToInnerRefactoring
									.containsStatusEntry(status, entry)) {
								status.addEntry(entry);
							}
						}
					}
				}

				ModifierRewrite.create(rewrite, declaration).setModifiers(
						newFlags, groupMove);
			}
		}

		ASTNode[] references = ConvertNestedToInnerRefactoring
				.getReferenceNodesIn(root, typeReferences, targetUnit);

		for (int index = 0; index < references.length; index++)
			updateTypeReference(parameters, references[index], targetRewrite,
					targetUnit);

		references = ConvertNestedToInnerRefactoring.getReferenceNodesIn(root,
				constructorReferences, targetUnit);
		if (fNeedOuterField) {
			for (final ASTNode element : references) {
				this.updateConstructorReference(parameters, element,
						targetRewrite, targetUnit, remove);
			}
		}
	}

	//
	//
	//

	private void updateTypeReference(ITypeBinding[] parameters, ASTNode node,
			CompilationUnitRewrite rewrite, ICompilationUnit cu)
			throws CoreException {
		final ImportDeclaration enclosingImport = (ImportDeclaration) ASTNodes
				.getParent(node, ImportDeclaration.class);
		if (enclosingImport != null)
			updateReferenceInImport(enclosingImport, node, rewrite);
		else {
			final TextEditGroup group = rewrite
					.createGroupDescription(RefactoringCoreMessages.MoveInnerToTopRefactoring_update_type_reference);
			updateReference(parameters, node, rewrite, group);
			/*
			 * if (!fType.getPackageFragment().equals(cu.getParent())) { final
			 * String name= fType.getPackageFragment().getElementName() + '.' +
			 * fType.getElementName();
			 * rewrite.getImportRemover().registerAddedImport(name);
			 * rewrite.getImportRewrite().addImport(name); }
			 */
		}
	}

	private void updateReferenceInImport(ImportDeclaration enclosingImport,
			ASTNode node, CompilationUnitRewrite rewrite) throws CoreException {
		/*
		 * final IBinding binding= enclosingImport.resolveBinding(); if (binding
		 * instanceof ITypeBinding) { final ITypeBinding type= (ITypeBinding)
		 * binding; final ImportRewrite rewriter= rewrite.getImportRewrite(); if
		 * (enclosingImport.isStatic()) { final String oldImport=
		 * ASTNodes.asString(node); final StringBuffer buffer= new
		 * StringBuffer(oldImport); final String typeName=
		 * fType.getDeclaringType().getElementName(); final int index=
		 * buffer.indexOf(typeName); if (index >= 0) { buffer.delete(index,
		 * index + typeName.length() + 1); final String newImport=
		 * buffer.toString(); if (enclosingImport.isOnDemand()) {
		 * rewriter.removeStaticImport(oldImport + ".*"); //$NON-NLS-1$
		 * rewriter.addStaticImport(newImport, "*", false); //$NON-NLS-1$ } else {
		 * rewriter.removeStaticImport(oldImport); final int offset=
		 * newImport.lastIndexOf('.'); if (offset >= 0 && offset <
		 * newImport.length() - 1) {
		 * rewriter.addStaticImport(newImport.substring(0, offset),
		 * newImport.substring(offset + 1), false); } } } } else
		 * rewriter.removeImport(type.getQualifiedName()); }
		 */
	}

	private boolean updateReference(ITypeBinding[] parameters, ASTNode node,
			CompilationUnitRewrite rewrite, TextEditGroup group) {
		if (node.getLocationInParent() == ParameterizedType.TYPE_PROPERTY) {
			updateParameterizedTypeReference(parameters,
					(ParameterizedType) node.getParent(), rewrite, group);
			return updateNameReference(new ITypeBinding[] {},
					((SimpleType) node).getName(), rewrite, group);
			// return false;
		} else if (node instanceof QualifiedName) {
			// return updateNameReference(parameters, (QualifiedName) node,
			// rewrite, group);
			return false;
		} else if (node instanceof SimpleType) {
			// return updateNameReference(parameters, ((SimpleType)
			// node).getName(), rewrite, group);
			return false;
		} else
			return false;
	}

	private boolean updateParameterizedTypeReference(ITypeBinding[] parameters,
			ParameterizedType type, CompilationUnitRewrite targetRewrite,
			TextEditGroup group) {
		// if (!(type.getParent() instanceof ClassInstanceCreation)) {
		final ListRewrite rewrite = targetRewrite
				.getASTRewrite()
				.getListRewrite(type, ParameterizedType.TYPE_ARGUMENTS_PROPERTY);
		final AST ast = targetRewrite.getRoot().getAST();
		Type simpleType = null;
		for (int index = type.typeArguments().size(); index < parameters.length; index++) {
			//final Type typeArg = (Type) type.typeArguments().get(0);
			//final ITypeBinding yb = typeArg.resolveBinding();
			simpleType = ast.newSimpleType(ast.newSimpleName(parameters[index]
					.getName()));
			rewrite.insertLast(simpleType, group);
		}
		// }
		return true;
	}

	private boolean updateNameReference(ITypeBinding[] parameters, Name name,
			CompilationUnitRewrite targetRewrite, TextEditGroup group) {
		if (ASTNodes.asString(name).equals(fType.getFullyQualifiedName('.'))) {
			targetRewrite.getASTRewrite().replace(name,
					getNewQualifiedNameNode(parameters, name), group);
			targetRewrite.getImportRemover().registerRemovedNode(name);
			return true;
		}
		targetRewrite.getASTRewrite().replace(name,
				getNewUnqualifiedTypeNode(parameters, name), group);
		targetRewrite.getImportRemover().registerRemovedNode(name);
		return true;
	}

	@SuppressWarnings("unchecked")
	private ASTNode getNewQualifiedNameNode(ITypeBinding[] parameters, Name name) {
		final AST ast = name.getAST();
		boolean raw = false;
		final ITypeBinding binding = name.resolveTypeBinding();
		if (binding != null && binding.isRawType())
			raw = true;
		final String fQualifiedTypeName = JavaModelUtil.concatenateName(fType
				.getPackageFragment().getElementName(), fType.getElementName());
		if (parameters != null && parameters.length > 0 && !raw) {
			final ParameterizedType type = ast.newParameterizedType(ast
					.newSimpleType(ast.newName(fQualifiedTypeName)));
			for (int index = 0; index < parameters.length; index++)
				type.typeArguments().add(
						ast.newSimpleType(ast.newSimpleName(parameters[index]
								.getName())));
			return type;
		}
		return ast.newName(fQualifiedTypeName);
	}

	@SuppressWarnings("unchecked")
	private ASTNode getNewUnqualifiedTypeNode(ITypeBinding[] parameters,
			Name name) {
		final AST ast = name.getAST();
		boolean raw = false;
		final ITypeBinding binding = name.resolveTypeBinding();
		if (binding != null && binding.isRawType())
			raw = true;
		if (parameters != null && parameters.length > 0 && !raw) {
			final ParameterizedType type = ast.newParameterizedType(ast
					.newSimpleType(ast.newSimpleName(fType.getElementName())));
			for (int index = 0; index < parameters.length; index++)
				type.typeArguments().add(
						ast.newSimpleType(ast.newSimpleName(parameters[index]
								.getName())));
			return type;
		}
		return ast.newSimpleType(ast.newSimpleName(fType.getElementName()));
	}

	//
	//
	//

	@SuppressWarnings("unchecked")
	private void addSuperConstructorInvok(ITypeBinding tBinding,
			MethodDeclaration declaration, ASTRewrite rewrite)
			throws CoreException {
		final AST ast = declaration.getAST();

		Assert.isTrue(declaration.isConstructor());
		// int paramCount = declaration.parameters().size();

		final ITypeBinding superClass = tBinding.getSuperclass();
		if (superClass != null) {
			final IMethod[] constructors = JavaElementUtil
					.getAllConstructors((IType) superClass.getJavaElement());
			final boolean force = !superClass.isTopLevel()
					&& tBinding.getDeclaringClass().isEqualTo(
							superClass.getDeclaringClass());
			if (force) {
				final List stats = declaration.getBody().statements();
				if (stats != null
						&& stats.size() > 0
						&& ((ASTNode) stats.get(0)).getNodeType() == ASTNode.SUPER_CONSTRUCTOR_INVOCATION) {
					// int index = stats.size() - 1;
					final SuperConstructorInvocation superConstructorInvocation = (SuperConstructorInvocation) stats
							.get(0);
					/*
					 * IMethodBinding mb =
					 * superConstructorInvocation.resolveConstructorBinding();
					 * if (mb != null) { ITypeBinding[] paramsType =
					 * mb.getParameterTypes(); if (paramsType[0].isEqualTo()) }
					 */
					if (!Modifier.isStatic(superClass.getModifiers())) {
						final CompilationUnit root = fSourceRewrite.getRoot();
						final AbstractTypeDeclaration typeDeclaration = ConvertNestedToInnerRefactoring
								.findTypeDeclaration(fType, root);

						String name = getNameForEnclosingInstanceConstructorParameter();
						if (!fRefEnclosing) {
							name = ((SingleVariableDeclaration) declaration
									.parameters().get(0)).getName()
									.getIdentifier();
						}
						if (!fContext.hasEnclosingAccess(superClass
								.getQualifiedName(), typeDeclaration
								.resolveBinding().getDeclaringClass()
								.getQualifiedName())) {
							return;
						}
						// TODO : here we had parameter at the begining
						rewrite.getListRewrite(superConstructorInvocation,
								SuperConstructorInvocation.ARGUMENTS_PROPERTY)
								.insertFirst(ast.newSimpleName(name), null);
					}
				} else {
					final SuperConstructorInvocation superConstructorInvocation = ast
							.newSuperConstructorInvocation();
					if (!Modifier.isStatic(superClass.getModifiers())) {
						String name = getNameForEnclosingInstanceConstructorParameter();
						final int index = declaration.parameters().size() - 1;
						if (!fRefEnclosing && index > -1 /*
															 * TODO: modified to
															 * avoid NPE
															 */) {
							name = ((SingleVariableDeclaration) declaration
									.parameters().get(index)).getName()
									.getIdentifier();
						}

						final SingleVariableDeclaration param = ast
								.newSingleVariableDeclaration();
						param.setName(ast.newSimpleName(name));
						// TODO : here we had parameter at the end !
						superConstructorInvocation.arguments().add(
								ast.newSimpleName(param.getName()
										.getIdentifier()));
					}
					rewrite.getListRewrite(declaration.getBody(),
							Block.STATEMENTS_PROPERTY).insertFirst(
							superConstructorInvocation, null);
				}
			} else if (constructors != null && constructors.length > 0) {
				switch (levelofEnclosingAcces(constructors)) {
				case 1:
					SuperConstructorInvocation superConstructorInvocation = ast
							.newSuperConstructorInvocation();
					boolean exists = false;
					ASTNode firstStatement = null;
					if (declaration.getBody().statements().size() > 0) {
						firstStatement = (ASTNode) declaration.getBody()
								.statements().get(0);
						if (firstStatement.getNodeType() == ASTNode.SUPER_CONSTRUCTOR_INVOCATION) {
							exists = true;
						}
						SingleVariableDeclaration param = null;
						if (!Modifier.isStatic(superClass.getModifiers())) {
							String name = getNameForEnclosingInstanceConstructorParameter();
							final int index = declaration.parameters().size() - 1;
							if (!fRefEnclosing) {
								name = ((SingleVariableDeclaration) declaration
										.parameters().get(index)).getName()
										.getIdentifier();
							}

							param = ast.newSingleVariableDeclaration();
							param.setName(ast.newSimpleName(name));
							// TODO : here we had parameter at the end !
							superConstructorInvocation.arguments().add(
									ast.newSimpleName(param.getName()
											.getIdentifier()));
						}
						if (!exists) {
							rewrite.getListRewrite(declaration.getBody(),
									Block.STATEMENTS_PROPERTY).insertFirst(
									superConstructorInvocation, null);
						} else {
							superConstructorInvocation = (SuperConstructorInvocation) firstStatement;
							rewrite
									.getListRewrite(
											superConstructorInvocation,
											SuperConstructorInvocation.ARGUMENTS_PROPERTY)
									.insertFirst(param.getName(), null);
						}
					} else {
						superConstructorInvocation = ast
								.newSuperConstructorInvocation();
						if (!Modifier.isStatic(superClass.getModifiers())) {
							//final String enclosing = tBinding
								//	.getDeclaringClass().getName();
							String name = getNameForEnclosingInstanceConstructorParameter();
							final int index = declaration.parameters().size() - 1;
							if (!fRefEnclosing) {
								name = ((SingleVariableDeclaration) declaration
										.parameters().get(index)).getName()
										.getIdentifier();
							}
							// TODO : here we had parameter at the end !
							superConstructorInvocation.arguments().add(
									ast.newName(name));
						}
						rewrite.getListRewrite(declaration.getBody(),
								Block.STATEMENTS_PROPERTY).insertFirst(
								superConstructorInvocation, null);
					}
					break;
				case 2: // The "ulrich" case ....
					superConstructorInvocation = ast
							.newSuperConstructorInvocation();
					if (!Modifier.isStatic(superClass.getModifiers())) {
						final String enclosing = tBinding.getDeclaringClass()
								.getDeclaringClass().getName();
						String name = getNameForEnclosingInstanceConstructorParameter()
								+ ".outer_" + enclosing;
						final int index = declaration.parameters().size() - 1;
						if (!fRefEnclosing) {
							name = ((SingleVariableDeclaration) declaration
									.parameters().get(index)).getName()
									.getIdentifier();
						}
						// TODO : here we had parameter at the end !
						superConstructorInvocation.arguments().add(
								ast.newName(name));
					}
					rewrite.getListRewrite(declaration.getBody(),
							Block.STATEMENTS_PROPERTY).insertFirst(
							superConstructorInvocation, null);
					break;
				}
				// declarations[index].getBody().statements().add(superConstructorInvocation);
			}
		}
	}

	@SuppressWarnings("unchecked")
	private void createConstructor(final AbstractTypeDeclaration declaration,
			final ASTRewrite rewrite) throws CoreException {
		Assert.isNotNull(declaration);
		Assert.isNotNull(rewrite);
		final AST ast = declaration.getAST();
		final MethodDeclaration constructor = ast.newMethodDeclaration();
		constructor.setConstructor(true);
		constructor.setName(ast.newSimpleName(declaration.getName()
				.getIdentifier()));
		final String comment = CodeGeneration.getMethodComment(fType
				.getCompilationUnit(), fType.getElementName(), fType
				.getElementName(), getNewConstructorParameterNames(),
				new String[0], null, null, StubUtility
						.getLineDelimiterUsed(fType.getJavaProject()));
		if (comment != null && comment.length() > 0) {
			final Javadoc doc = (Javadoc) rewrite.createStringPlaceholder(
					comment, ASTNode.JAVADOC);
			constructor.setJavadoc(doc);
		}

		final Block body = ast.newBlock();

		if (fCreateInstanceField) {
			final SingleVariableDeclaration variable = ast
					.newSingleVariableDeclaration();
			final String name = getNameForEnclosingInstanceConstructorParameter();
			final Type enclosingType = createEnclosingType(ast);
			variable.setName(ast.newSimpleName(name));
			variable.setType(enclosingType);
			constructor.parameters().add(variable);
			final Assignment assignment = ast.newAssignment();
			if (fCodeGenerationSettings.useKeywordThis
					|| fEnclosingInstanceFieldName
							.equals(fNameForEnclosingInstanceConstructorParameter)) {
				final FieldAccess access = ast.newFieldAccess();
				access.setExpression(ast.newThisExpression());
				access.setName(ast.newSimpleName(fEnclosingInstanceFieldName));
				assignment.setLeftHandSide(access);
			} else {
				assignment.setLeftHandSide(ast
						.newSimpleName(fEnclosingInstanceFieldName));
			}
			assignment.setRightHandSide(ast.newSimpleName(name));
			// }
			// TODO :
			final int paramCount = constructor.parameters().size();
			if (paramCount > 0) {
				// hum hum
				final ITypeBinding superClass = declaration.resolveBinding()
						.getSuperclass();
				if (superClass != null) {
					final IMethod[] constructors = JavaElementUtil
							.getAllConstructors((IType) superClass
									.getJavaElement());
					if (constructors != null && constructors.length > 0) {
						switch (levelofEnclosingAcces(constructors)) {
						case 1:
							SuperConstructorInvocation superConstructorInvocation = ast
									.newSuperConstructorInvocation();
							for (int i = 0; i < paramCount; i++) {
								final SingleVariableDeclaration param = (SingleVariableDeclaration) constructor
										.parameters().get(i);
								superConstructorInvocation.arguments().add(
										ast.newSimpleName(param.getName()
												.getIdentifier()));
							}
							body.statements().add(superConstructorInvocation);
							break;
						case 2:
							superConstructorInvocation = ast
									.newSuperConstructorInvocation();
							//
							final String enclosing = superClass
									.getDeclaringClass().getName();
							final String pname = getNameForEnclosingInstanceConstructorParameter()
									+ ".outer_" + enclosing;
							//
							/*
							 * for (int i = 0; i < paramCount; i++) {
							 * SingleVariableDeclaration param =
							 * (SingleVariableDeclaration) constructor
							 * .parameters().get(i);
							 * superConstructorInvocation.arguments().add(
							 * ast.newSimpleName(param.getName()
							 * .getIdentifier())); }
							 */
							superConstructorInvocation.arguments().add(
									ast.newName(pname));
							body.statements().add(superConstructorInvocation);
							break;
						}
					}
				}
				//
				//
				/*
				 * MethodDeclaration emptyConstructor =
				 * ast.newMethodDeclaration();
				 * emptyConstructor.setConstructor(true);
				 * emptyConstructor.setName(ast.newSimpleName(declaration.getName()
				 * .getIdentifier())); Block emptyBody = ast.newBlock();
				 * emptyConstructor.setBody(emptyBody);
				 * emptyConstructor.modifiers().add(
				 * ast.newModifier(Modifier.ModifierKeyword.PUBLIC_KEYWORD));
				 * rewrite.getListRewrite(declaration,
				 * declaration.getBodyDeclarationsProperty()).insertFirst(
				 * emptyConstructor, null);
				 */
				//
				//
			}
			final Statement statement = ast.newExpressionStatement(assignment);
			body.statements().add(statement);
			constructor.setBody(body);
			//
			//
		}
		constructor.modifiers().add(
				ast.newModifier(Modifier.ModifierKeyword.PUBLIC_KEYWORD));
		rewrite.getListRewrite(declaration,
				declaration.getBodyDeclarationsProperty()).insertFirst(
				constructor, null);

	}

	private int levelofEnclosingAcces(IMethod[] constructors)
			throws JavaModelException {
		for (final IMethod constructor : constructors) {
			if (constructor.getNumberOfParameters() == 1) {
				final String typeName = constructor.getParameterTypes()[0];
				if (Signature.toString(typeName).equals(
						TranslationUtils.getTypeQualifiedName(this.fType
								.getDeclaringType()))) {
					return 1;
				}
				// We want to know if typeName is the enclosing type
				// that type.
				final String typeName2 = Signature.toString(typeName);
				if (constructor.getDeclaringType().getDeclaringType() != null) {
					final String declaringType = constructor.getDeclaringType()
							.getDeclaringType().getElementName();
					if (declaringType.equals(typeName2)) {
						return 2;
					}
				}
				//
				if (constructor.getDeclaringType().isMember()) {
					final String innerName = constructor.getDeclaringType()
							.getFullyQualifiedName().replace("$", ".");
					final String enclosingName = constructor.getDeclaringType()
							.getDeclaringType().getFullyQualifiedName();
					if (fContext.hasEnclosingAccess(innerName, enclosingName)) {
						return 1;
					}
				}
			} else if (constructor.getNumberOfParameters() == 0) {
				if (constructor.getDeclaringType().isMember()) {
					final String innerName = constructor.getDeclaringType()
							.getFullyQualifiedName().replace("$", ".");
					final String enclosingName = constructor.getDeclaringType()
							.getDeclaringType().getFullyQualifiedName();
					if (fContext.hasEnclosingAccess(innerName, enclosingName)) {
						return 1;
					}
				}
			}
		}
		return 0;
	}

	// Map<ICompilationUnit, SearchMatch[]>
	@SuppressWarnings("unchecked")
	private Map createConstructorReferencesMapping(IProgressMonitor pm,
			RefactoringStatus status) throws JavaModelException {
		final SearchResultGroup[] groups = ConstructorReferenceFinder
				.getConstructorReferences(fType, pm, status);
		final Map result = new HashMap();
		for (final SearchResultGroup group : groups) {
			final ICompilationUnit cu = group.getCompilationUnit();
			if (cu == null) {
				continue;
			}
			result.put(cu, group.getSearchResults());
		}
		return result;
	}

	private Expression createEnclosingInstanceCreationString(
			final ASTNode node, final ICompilationUnit cu)
			throws JavaModelException {
		Assert.isTrue((node instanceof ClassInstanceCreation)
				|| (node instanceof SuperConstructorInvocation));
		Assert.isNotNull(cu);
		Expression expression = null;
		if (node instanceof ClassInstanceCreation) {
			expression = ((ClassInstanceCreation) node).getExpression();
		} else {
			expression = ((SuperConstructorInvocation) node).getExpression();
		}
		final AST ast = node.getAST();
		if (expression != null) {
			return expression;
			/*
			 * } ZORGLUB: else if (JdtFlags.isStatic(this.fType)) { return null;
			 */
		} else if (isInsideSubclassOfDeclaringType(node)) {
			return ast.newThisExpression();
		} else if ((node.getStartPosition() >= fType.getSourceRange()
				.getOffset() && ASTNodes.getExclusiveEnd(node) <= fType
				.getSourceRange().getOffset()
				+ fType.getSourceRange().getLength())) {
			if (fCodeGenerationSettings.useKeywordThis
					|| fEnclosingInstanceFieldName
							.equals(fNameForEnclosingInstanceConstructorParameter)) {
				final FieldAccess access = ast.newFieldAccess();
				access.setExpression(ast.newThisExpression());
				access.setName(ast.newSimpleName(fEnclosingInstanceFieldName));
				return access;
			} else {
				return ast.newSimpleName(fEnclosingInstanceFieldName);
			}
		} else if (isInsideTypeNestedInDeclaringType(node)) {
			final ThisExpression qualified = ast.newThisExpression();
			qualified.setQualifier(ast.newSimpleName(fType.getDeclaringType()
					.getElementName()));
			return qualified;
		}
		return null;
	}

	@SuppressWarnings("unchecked")
	private Type createEnclosingType(final AST ast) throws JavaModelException {
		Assert.isNotNull(ast);
		final ITypeParameter[] parameters = fType.getDeclaringType()
				.getTypeParameters();
		final Type type = ASTNodeFactory.newType(ast, TranslationUtils
				.getTypeQualifiedName(fType.getDeclaringType()));
		if (parameters.length > 0) {
			final ParameterizedType parameterized = ast
					.newParameterizedType(type);
			for (final ITypeParameter element : parameters) {
				parameterized.typeArguments().add(
						ast.newSimpleType(ast.newSimpleName(element
								.getElementName())));
			}
			return parameterized;
		}
		return type;
	}

	private String createNewSource(final CompilationUnitRewrite targetRewrite,
			final ICompilationUnit unit) throws CoreException,
			JavaModelException {
		Assert.isNotNull(targetRewrite);
		Assert.isNotNull(unit);
		TextChange change = targetRewrite.createChange();
		if (change == null) {
			change = new CompilationUnitChange("", unit); //$NON-NLS-1$
		}
		final String source = change
				.getPreviewContent(new NullProgressMonitor());
		final ASTParser parser = ASTParser.newParser(AST.JLS3);
		parser.setProject(fType.getJavaProject());
		parser.setResolveBindings(false);
		parser.setSource(source.toCharArray());
		final AbstractTypeDeclaration declaration = ConvertNestedToInnerRefactoring
				.findTypeDeclaration(fType, (CompilationUnit) parser
						.createAST(null));
		return source.substring(declaration.getStartPosition(), ASTNodes
				.getExclusiveEnd(declaration));
	}

	private Expression createQualifiedReadAccessExpressionForEnclosingInstance(
			AST ast) {
		/*
		 * ThisExpression expression = ast.newThisExpression();
		 * expression.setQualifier(ast.newName(new String[] { this.fType
		 * .getElementName() }));
		 */
		final String outerName = "outer_" + fType.getElementName();
		final Expression expression = ast.newSimpleName(outerName);
		final FieldAccess access = ast.newFieldAccess();
		access.setExpression(expression);
		access.setName(ast.newSimpleName(fEnclosingInstanceFieldName));
		return access;
	}

	private Expression createReadAccessExpressionForEnclosingInstance(AST ast,
			boolean isInConstructor) {
		return ast.newSimpleName(fNameForEnclosingInstanceConstructorParameter);
	}

	private Expression createReadAccessExpressionForEnclosingInstance(AST ast) {
		if (fCodeGenerationSettings.useKeywordThis
				|| fEnclosingInstanceFieldName
						.equals(fNameForEnclosingInstanceConstructorParameter)) {
			final FieldAccess access = ast.newFieldAccess();
			access.setExpression(ast.newThisExpression());
			access.setName(ast.newSimpleName(fEnclosingInstanceFieldName));
			return access;
		}
		return ast.newSimpleName(fEnclosingInstanceFieldName);
	}

	/*
	 * private String createSourceForNewCu(final ICompilationUnit unit, final
	 * IProgressMonitor monitor) throws CoreException { Assert.isNotNull(unit);
	 * Assert.isNotNull(monitor); try { monitor.beginTask("", 2); //$NON-NLS-1$
	 * final String separator = StubUtility.getLineDelimiterUsed(fType
	 * .getJavaProject()); final String block = getAlignedSourceBlock(unit,
	 * fNewSourceOfInputType); String content = CodeGeneration
	 * .getCompilationUnitContent(unit, null, block, separator); if (content ==
	 * null || block.startsWith("/*") || block.startsWith("//")) {
	 * //$NON-NLS-1$//$NON-NLS-2$ final StringBuffer buffer = new
	 * StringBuffer(); if (!fType.getPackageFragment().isDefaultPackage()) {
	 * buffer .append("package
	 * ").append(fType.getPackageFragment().getElementName()).append(';');
	 * //$NON-NLS-1$ } buffer.append(separator).append(separator);
	 * buffer.append(block); content = buffer.toString(); }
	 * unit.getBuffer().setContents(content); addImportsToTargetUnit(unit, new
	 * SubProgressMonitor(monitor, 1)); } finally { monitor.done(); } return
	 * unit.getSource(); }
	 */

	// Map<ICompilationUnit, SearchMatch[]>
	@SuppressWarnings("unchecked")
	private Map createTypeReferencesMapping(IProgressMonitor pm,
			RefactoringStatus status) throws JavaModelException {
		final RefactoringSearchEngine2 engine = new RefactoringSearchEngine2(
				SearchPattern.createPattern(fType,
						IJavaSearchConstants.ALL_OCCURRENCES,
						SearchUtils.GENERICS_AGNOSTIC_MATCH_RULE));
		engine.setFiltering(true, true);
		engine.setScope(RefactoringScopeFactory.create(fType));
		engine.setStatus(status);
		engine.searchPattern(new SubProgressMonitor(pm, 1));
		final SearchResultGroup[] groups = (SearchResultGroup[]) engine
				.getResults();
		final Map result = new HashMap();
		for (final SearchResultGroup group : groups) {
			final ICompilationUnit cu = group.getCompilationUnit();
			if (cu == null) {
				continue;
			}
			result.put(cu, group.getSearchResults());
		}
		return result;
	}

	/*
	 * private String getAlignedSourceBlock(final ICompilationUnit unit, final
	 * String block) { Assert.isNotNull(block); final String[] lines =
	 * Strings.convertIntoLines(block); Strings.trimIndentation(lines,
	 * unit.getJavaProject(), false); return Strings.concatenate(lines,
	 * StubUtility .getLineDelimiterUsed(fType.getJavaProject())); }
	 */

	private CompilationUnitRewrite getCompilationUnitRewrite(
			final ICompilationUnit unit) {
		Assert.isNotNull(unit);
		if (unit.equals(fType.getCompilationUnit())) {
			return fSourceRewrite;
		}
		return new CompilationUnitRewrite(unit);
	}

	@SuppressWarnings("unchecked")
	private MethodDeclaration[] getConstructorDeclarationNodes(
			final AbstractTypeDeclaration declaration) {
		if (declaration instanceof TypeDeclaration) {
			final MethodDeclaration[] declarations = ((TypeDeclaration) declaration)
					.getMethods();
			final List result = new ArrayList(2);
			for (final MethodDeclaration element : declarations) {
				if (element.isConstructor()) {
					result.add(element);
				}
			}
			return (MethodDeclaration[]) result
					.toArray(new MethodDeclaration[result.size()]);
		}
		return new MethodDeclaration[] {};
	}

	public boolean getCreateInstanceField() {
		return fCreateInstanceField;
	}

	private int getEnclosingInstanceAccessModifiers() {
		if (fMarkInstanceFieldAsFinal) {
			return Modifier.PRIVATE | Modifier.FINAL;
		} else {
			return Modifier.PRIVATE;
		}
	}

	public String getEnclosingInstanceName() {
		return fEnclosingInstanceFieldName;
	}

	private String getInitialNameForEnclosingInstanceField() {
		final IType enclosingType = fType.getDeclaringType();
		if (enclosingType == null) {
			return ""; //$NON-NLS-1$
		}
		final String[] suggestedNames = NamingConventions.suggestFieldNames(
				enclosingType.getJavaProject(), enclosingType
						.getPackageFragment().getElementName(), TranslationUtils
						.getTypeQualifiedName(fType.getDeclaringType()), 0,
				getEnclosingInstanceAccessModifiers(),
				ConvertNestedToInnerRefactoring.getFieldNames(fType));
		if (suggestedNames.length > 0) {
			return suggestedNames[suggestedNames.length - 1];
		}
		final String name = enclosingType.getElementName();
		if (name.equals("")) {
			return ""; //$NON-NLS-1$
		}
		return Character.toLowerCase(name.charAt(0)) + name.substring(1);
	}

	public IType getInputType() {
		return fType;
	}

	/*
	 * @see org.eclipse.jdt.internal.corext.refactoring.base.IRefactoring#getName()
	 */
	@Override
	public String getName() {
		return RefactoringCoreMessages.MoveInnerToTopRefactoring_name;
	}

	private String getNameForEnclosingInstanceConstructorParameter()
			throws JavaModelException {
		if (fNameForEnclosingInstanceConstructorParameter != null) {
			return fNameForEnclosingInstanceConstructorParameter;
		}

		final IType enclosingType = fType.getDeclaringType();
		final String[] suggestedNames = NamingConventions.suggestArgumentNames(
				enclosingType.getJavaProject(), enclosingType
						.getPackageFragment().getElementName(), TranslationUtils
						.getTypeQualifiedName(fType.getDeclaringType()), 0,
				ConvertNestedToInnerRefactoring
						.getParameterNamesOfAllConstructors(fType));
		if (suggestedNames.length > 0) {
			// Choose the lase one is better ...
			fNameForEnclosingInstanceConstructorParameter = suggestedNames[suggestedNames.length - 1];
		} else {
			fNameForEnclosingInstanceConstructorParameter = fEnclosingInstanceFieldName;
		}
		return fNameForEnclosingInstanceConstructorParameter;
	}

	private String[] getNewConstructorParameterNames()
			throws JavaModelException {
		if (!fCreateInstanceField) {
			return new String[0];
		}
		return new String[] { getNameForEnclosingInstanceConstructorParameter() };
	}

	// @SuppressWarnings("unchecked")
	/*
	 * private ASTNode getNewQualifiedNameNode(ITypeBinding[] parameters, Name
	 * name) { final AST ast = name.getAST(); boolean raw = false; final
	 * ITypeBinding binding = name.resolveTypeBinding(); if (binding != null &&
	 * binding.isRawType()) raw = true; if (parameters != null &&
	 * parameters.length > 0 && !raw) { final ParameterizedType type =
	 * ast.newParameterizedType(ast
	 * .newSimpleType(ast.newName(fQualifiedTypeName))); for (int index = 0;
	 * index < parameters.length; index++) type.typeArguments().add(
	 * ast.newSimpleType(ast.newSimpleName(parameters[index] .getName())));
	 * return type; } return ast.newName(fQualifiedTypeName); }
	 */

	// @SuppressWarnings("unchecked")
	/*
	 * private ASTNode getNewUnqualifiedTypeNode(ITypeBinding[] parameters, Name
	 * name) { final AST ast = name.getAST(); boolean raw = false; final
	 * ITypeBinding binding = name.resolveTypeBinding(); if (binding != null &&
	 * binding.isRawType()) raw = true; if (parameters != null &&
	 * parameters.length > 0 && !raw) { final ParameterizedType type =
	 * ast.newParameterizedType(ast
	 * .newSimpleType(ast.newSimpleName(fType.getElementName()))); for (int
	 * index = 0; index < parameters.length; index++) type.typeArguments().add(
	 * ast.newSimpleType(ast.newSimpleName(parameters[index] .getName())));
	 * return type; } return
	 * ast.newSimpleType(ast.newSimpleName(fType.getElementName())); }
	 */

	private boolean insertExpressionAsParameter(ClassInstanceCreation cic,
			ASTRewrite rewrite, ICompilationUnit cu, TextEditGroup group)
			throws JavaModelException {
		final Expression expression = createEnclosingInstanceCreationString(
				cic, cu);
		if (expression == null) {
			return false;
		}
		rewrite.getListRewrite(cic, ClassInstanceCreation.ARGUMENTS_PROPERTY)
				.insertFirst(expression, group);
		return true;
	}

	private boolean insertExpressionAsParameter(SuperConstructorInvocation sci,
			ASTRewrite rewrite, ICompilationUnit cu, TextEditGroup group)
			throws JavaModelException {
		final Expression expression = createEnclosingInstanceCreationString(
				sci, cu);
		if (expression == null) {
			return false;
		}
		rewrite.getListRewrite(sci,
				SuperConstructorInvocation.ARGUMENTS_PROPERTY).insertFirst(
				expression, group);
		return true;
	}

	public boolean isCreatingInstanceFieldMandatory() {
		return fIsInstanceFieldCreationMandatory;
	}

	public boolean isCreatingInstanceFieldPossible() {
		return fIsInstanceFieldCreationPossible;
	}

	private boolean isInAnonymousTypeInsideInputType(ASTNode node,
			AbstractTypeDeclaration declaration) {
		final AnonymousClassDeclaration anonymous = (AnonymousClassDeclaration) ASTNodes
				.getParent(node, AnonymousClassDeclaration.class);
		return anonymous != null && ASTNodes.isParent(anonymous, declaration);
	}

	private boolean isInLocalTypeInsideInputType(ASTNode node,
			AbstractTypeDeclaration declaration) {
		final TypeDeclarationStatement statement = (TypeDeclarationStatement) ASTNodes
				.getParent(node, TypeDeclarationStatement.class);
		return statement != null && ASTNodes.isParent(statement, declaration);
	}

	private boolean isInNonStaticMemberTypeInsideInputType(ASTNode node,
			AbstractTypeDeclaration declaration) {
		final AbstractTypeDeclaration nested = (AbstractTypeDeclaration) ASTNodes
				.getParent(node, AbstractTypeDeclaration.class);
		return nested != null && !declaration.equals(nested)
				&& !Modifier.isStatic(nested.getFlags())
				&& ASTNodes.isParent(nested, declaration);
	}

	//
	private boolean isInSuperConstructorInvocation(ASTNode node,
			AbstractTypeDeclaration declaration) {
		final SuperConstructorInvocation superConstructorInvoc = (SuperConstructorInvocation) ASTNodes
				.getParent(node, SuperConstructorInvocation.class);
		return superConstructorInvoc != null
				&& ASTNodes.isParent(superConstructorInvoc, declaration);
	}

	private boolean isInThisConstructorInvocation(ASTNode node,
			AbstractTypeDeclaration declaration) {
		final ConstructorInvocation thisConstructorInvoc = (ConstructorInvocation) ASTNodes
				.getParent(node, ConstructorInvocation.class);
		return thisConstructorInvoc != null
				&& ASTNodes.isParent(thisConstructorInvoc, declaration);
	}

	private boolean isInSuperConstructorInvocation(ASTNode node) {
		final SuperConstructorInvocation superConstructorInvoc = (SuperConstructorInvocation) ASTNodes
				.getParent(node, SuperConstructorInvocation.class);
		return superConstructorInvoc != null;
	}

	//

	private boolean isInsideSubclassOfDeclaringType(ASTNode node) {
		Assert.isTrue((node instanceof ClassInstanceCreation)
				|| (node instanceof SuperConstructorInvocation));
		final AbstractTypeDeclaration declaration = (AbstractTypeDeclaration) ASTNodes
				.getParent(node, AbstractTypeDeclaration.class);
		Assert.isNotNull(declaration);

		final AnonymousClassDeclaration anonymous = (AnonymousClassDeclaration) ASTNodes
				.getParent(node, AnonymousClassDeclaration.class);
		final boolean isAnonymous = anonymous != null
				&& ASTNodes.isParent(anonymous, declaration);
		if (isAnonymous) {
			return anonymous != null
					&& isSubclassBindingOfEnclosingType(anonymous
							.resolveBinding());
		}
		return isSubclassBindingOfEnclosingType(declaration.resolveBinding());
	}

	private boolean isInsideTypeNestedInDeclaringType(ASTNode node) {
		Assert.isTrue((node instanceof ClassInstanceCreation)
				|| (node instanceof SuperConstructorInvocation));
		final AbstractTypeDeclaration declaration = (AbstractTypeDeclaration) ASTNodes
				.getParent(node, AbstractTypeDeclaration.class);
		Assert.isNotNull(declaration);
		ITypeBinding enclosing = declaration.resolveBinding();
		while (enclosing != null) {
			if (ConvertNestedToInnerRefactoring.isCorrespondingTypeBinding(
					enclosing, fType.getDeclaringType())) {
				return true;
			}
			enclosing = enclosing.getDeclaringClass();
		}
		return false;
	}

	public boolean isInstanceFieldMarkedFinal() {
		return fMarkInstanceFieldAsFinal;
	}

	private boolean isSubclassBindingOfEnclosingType(ITypeBinding binding) {
		while (binding != null) {
			if (ConvertNestedToInnerRefactoring.isCorrespondingTypeBinding(
					binding, fType.getDeclaringType())) {
				return true;
			}
			binding = binding.getSuperclass();
		}
		return false;
	}

	/*
	 * This method qualifies accesses from within the moved type to the (now
	 * former) enclosed type of the moved type. Note that all visibility changes
	 * have already been scheduled in the visibility adjustor.
	 */
	@SuppressWarnings("unchecked")
	private void modifyAccessToEnclosingInstance(
			final CompilationUnitRewrite targetRewrite,
			final AbstractTypeDeclaration declaration,
			final RefactoringStatus status, final IProgressMonitor monitor)
			throws JavaModelException {
		Assert.isNotNull(targetRewrite);
		Assert.isNotNull(declaration);
		Assert.isNotNull(monitor);
		final Set handledMethods = new HashSet();
		final Set handledFields = new HashSet();
		// An inner static can acces to non static method of the enclosing
		if (/* !Flags.isStatic(this.fType.getFlags()) && */!fEnclosingIsSuperOfInner) {
			final MemberAccessNodeCollector collector = new MemberAccessNodeCollector(
					fType.getDeclaringType().newSupertypeHierarchy(
							new SubProgressMonitor(monitor, 1)));
			declaration.accept(collector);
			modifyAccessToMethodsFromEnclosingInstance(targetRewrite,
					handledMethods, collector.getMethodInvocations(),
					declaration, status);
			this.modifyAccessToFieldsFromEnclosingInstance(targetRewrite,
					handledFields, collector.getFieldAccesses(), declaration,
					status);
			this.modifyAccessToFieldsFromEnclosingInstance(targetRewrite,
					handledFields, collector.getSimpleFieldNames(),
					declaration, status);
		}
	}

	@SuppressWarnings("unchecked")
	private void modifyAccessToFieldsFromEnclosingInstance(
			CompilationUnitRewrite targetRewrite, Set handledFields,
			FieldAccess[] fieldAccesses, AbstractTypeDeclaration declaration,
			RefactoringStatus status) {
		FieldAccess access = null;
		for (final FieldAccess element : fieldAccesses) {
			access = element;
			Assert.isNotNull(access.getExpression());
			if (!(access.getExpression() instanceof ThisExpression)
					|| (!(((ThisExpression) access.getExpression())
							.getQualifier() != null))) {
				continue;
			}

			final IVariableBinding binding = access.resolveFieldBinding();
			if (binding != null) {
				targetRewrite.getASTRewrite().replace(
						access.getExpression(),
						createAccessExpressionToEnclosingInstanceFieldText(
								access, binding, declaration), null);
				targetRewrite.getImportRemover().registerRemovedNode(
						access.getExpression());
			}
		}
	}

	@SuppressWarnings("unchecked")
	private void modifyAccessToFieldsFromEnclosingInstance(
			CompilationUnitRewrite targetRewrite, Set handledFields,
			SimpleName[] simpleNames, AbstractTypeDeclaration declaration,
			RefactoringStatus status) {
		IBinding binding = null;
		SimpleName simpleName = null;
		IVariableBinding variable = null;
		for (final SimpleName element : simpleNames) {
			simpleName = element;
			binding = simpleName.resolveBinding();
			if (binding != null && binding instanceof IVariableBinding
					&& !(simpleName.getParent() instanceof FieldAccess)) {
				variable = (IVariableBinding) binding;
				final FieldAccess access = simpleName.getAST().newFieldAccess();
				access
						.setExpression(createAccessExpressionToEnclosingInstanceFieldText(
								simpleName, variable, declaration));
				access.setName(simpleName.getAST().newSimpleName(
						simpleName.getIdentifier()));
				targetRewrite.getASTRewrite().replace(simpleName, access, null);
				targetRewrite.getImportRemover()
						.registerRemovedNode(simpleName);
			}
		}
	}

	@SuppressWarnings("unchecked")
	private void modifyAccessToMethodsFromEnclosingInstance(
			CompilationUnitRewrite targetRewrite, Set handledMethods,
			MethodInvocation[] methodInvocations,
			AbstractTypeDeclaration declaration, RefactoringStatus status) {
		IMethodBinding binding = null;
		MethodInvocation invocation = null;
		for (final MethodInvocation element : methodInvocations) {
			invocation = element;
			binding = invocation.resolveMethodBinding();
			if (binding != null) {
				final Expression target = invocation.getExpression();
				if (target == null) {
					final Expression expression = createAccessExpressionToEnclosingInstanceFieldText(
							invocation, binding, declaration);
					targetRewrite.getASTRewrite().set(invocation,
							MethodInvocation.EXPRESSION_PROPERTY, expression,
							null);
				} else {
					if (!(invocation.getExpression() instanceof ThisExpression)
							|| !(((ThisExpression) invocation.getExpression())
									.getQualifier() != null)) {
						continue;
					}
					targetRewrite.getASTRewrite().replace(
							target,
							createAccessExpressionToEnclosingInstanceFieldText(
									invocation, binding, declaration), null);
					targetRewrite.getImportRemover()
							.registerRemovedNode(target);
				}
			}
		}
	}

	private void modifyConstructors(AbstractTypeDeclaration declaration,
			ASTRewrite rewrite) throws CoreException {
		final MethodDeclaration[] declarations = getConstructorDeclarationNodes(declaration);
		for (final MethodDeclaration element : declarations) {
			Assert.isTrue(element.isConstructor());
			if (fNeedOuterField) {
				addParameterToConstructor(rewrite, element);
				setEnclosingInstanceFieldInConstructor(rewrite, element);
			}
			addSuperConstructorInvocation(rewrite, element);
		}
	}

	private void addSuperConstructorInvocation(ASTRewrite rewrite,
			MethodDeclaration declaration) throws CoreException {
		// 1. determine if super class exist and if it's an inner
		// String superClassName = fType.getSuperclassName();
		/*
		 * TODO if (superClassName != null) { if
		 * (!isRenamedByTranslator(superClassName)) return; }
		 */

		final CompilationUnit root = (CompilationUnit) declaration.getRoot();
		final AbstractTypeDeclaration superDeclaration = ConvertNestedToInnerRefactoring
				.findTypeDeclaration(fType, root);
		// 2. Search in super class for avalable constructor
		addSuperConstructorInvok(superDeclaration.resolveBinding(),
				declaration, rewrite);
	}

	private void modifyInterfaceMemberModifiers(final ITypeBinding binding) {
		Assert.isNotNull(binding);
		ITypeBinding declaring = binding.getDeclaringClass();
		while (declaring != null && !declaring.isInterface()) {
			declaring = declaring.getDeclaringClass();
		}
		if (declaring != null) {
			final ASTNode node = ASTNodes.findDeclaration(binding,
					fSourceRewrite.getRoot());
			if (node instanceof AbstractTypeDeclaration) {
				ModifierRewrite.create(fSourceRewrite.getASTRewrite(), node)
						.setVisibility(Modifier.PUBLIC, null);
			}
		}
	}

	public void setCreateInstanceField(boolean create) {
		Assert.isTrue(fIsInstanceFieldCreationPossible);
		Assert.isTrue(!fIsInstanceFieldCreationMandatory);
		fCreateInstanceField = create;
	}

	@SuppressWarnings("unchecked")
	private void setEnclosingInstanceFieldInConstructor(ASTRewrite rewrite,
			MethodDeclaration decl) throws JavaModelException {
		final AST ast = decl.getAST();
		final Block body = decl.getBody();
		final List statements = body.statements();
		if (statements.isEmpty()) {
			final Assignment assignment = ast.newAssignment();
			assignment.setLeftHandSide(this
					.createReadAccessExpressionForEnclosingInstance(ast));
			assignment
					.setRightHandSide(ast
							.newSimpleName(getNameForEnclosingInstanceConstructorParameter()));
			rewrite.getListRewrite(body, Block.STATEMENTS_PROPERTY)
					.insertFirst(ast.newExpressionStatement(assignment), null);
		} else {
			final Statement first = (Statement) statements.get(0);
			if (first instanceof ConstructorInvocation) {
				rewrite
						.getListRewrite(first,
								ConstructorInvocation.ARGUMENTS_PROPERTY)
						.insertFirst(
								ast
										.newSimpleName(getNameForEnclosingInstanceConstructorParameter()),
								null);
			} else {
				int index = 0;
				if (first instanceof SuperConstructorInvocation) {
					index++;
				}
				final Assignment assignment = ast.newAssignment();
				assignment.setLeftHandSide(this
						.createReadAccessExpressionForEnclosingInstance(ast));
				assignment
						.setRightHandSide(ast
								.newSimpleName(getNameForEnclosingInstanceConstructorParameter()));
				rewrite.getListRewrite(body, Block.STATEMENTS_PROPERTY)
						.insertAt(ast.newExpressionStatement(assignment),
								index, null);
			}
		}
	}

	public void setEnclosingInstanceName(String name) {
		Assert.isNotNull(name);
		fEnclosingInstanceFieldName = name;
	}

	public void setMarkInstanceFieldAsFinal(boolean mark) {
		fMarkInstanceFieldAsFinal = mark;
	}

	private void updateConstructorReference(
			final ClassInstanceCreation creation,
			final CompilationUnitRewrite targetRewrite,
			final ICompilationUnit unit, TextEditGroup group)
			throws JavaModelException {
		Assert.isNotNull(creation);
		Assert.isNotNull(targetRewrite);
		Assert.isNotNull(unit);
		final ASTRewrite rewrite = targetRewrite.getASTRewrite();
		if (fCreateInstanceField) {
			this.insertExpressionAsParameter(creation, rewrite, unit, group);
		}
		final Expression expression = creation.getExpression();
		if (expression != null && expression.resolveTypeBinding() != null) {
			final String eName = expression.resolveTypeBinding().getName();
			final String sName = creation.getType().resolveBinding().getName();
			rewrite.replace(creation.getType(),
					rewrite
							.createStringPlaceholder(" /* insert_here:" + eName
									+ ". */ " + sName, creation.getType()
									.getNodeType()), null);
		}
	}

	private void updateConstructorReference(ITypeBinding[] parameters,
			ASTNode reference, CompilationUnitRewrite targetRewrite,
			ICompilationUnit cu, boolean remove) throws CoreException {
		final TextEditGroup group = targetRewrite
				.createGroupDescription(RefactoringCoreMessages.MoveInnerToTopRefactoring_update_constructor_reference);
		if (reference instanceof SuperConstructorInvocation) {
			this.updateConstructorReference(
					(SuperConstructorInvocation) reference, targetRewrite, cu,
					group, remove);
		} else if (reference instanceof ClassInstanceCreation) {
			this.updateConstructorReference((ClassInstanceCreation) reference,
					targetRewrite, cu, group);
		} else if (reference.getParent() instanceof ClassInstanceCreation) {
			this.updateConstructorReference((ClassInstanceCreation) reference
					.getParent(), targetRewrite, cu, group);
		} else if (reference.getParent() instanceof ParameterizedType
				&& reference.getParent().getParent() instanceof ClassInstanceCreation) {
			this.updateConstructorReference(parameters,
					(ParameterizedType) reference.getParent(), targetRewrite,
					cu, group);
		}
	}

	private void updateConstructorReference(ITypeBinding[] parameters,
			ParameterizedType type, CompilationUnitRewrite targetRewrite,
			ICompilationUnit cu, TextEditGroup group) throws CoreException {
		final ListRewrite rewrite = targetRewrite
				.getASTRewrite()
				.getListRewrite(type, ParameterizedType.TYPE_ARGUMENTS_PROPERTY);
		TypeParameter parameter = null;
		for (int index = type.typeArguments().size(); index < parameters.length; index++) {
			parameter = targetRewrite.getRoot().getAST().newTypeParameter();
			parameter.setName(targetRewrite.getRoot().getAST().newSimpleName(
					parameters[index].getName()));
			rewrite.insertLast(parameter, group);
		}
		if (type.getParent() instanceof ClassInstanceCreation) {
			this.updateConstructorReference((ClassInstanceCreation) type
					.getParent(), targetRewrite, cu, group);
		}
	}

	private void updateConstructorReference(
			final SuperConstructorInvocation invocation,
			final CompilationUnitRewrite targetRewrite,
			final ICompilationUnit unit, TextEditGroup group, boolean remove)
			throws CoreException {
		Assert.isNotNull(invocation);
		Assert.isNotNull(targetRewrite);
		Assert.isNotNull(unit);
		final ASTRewrite rewrite = targetRewrite.getASTRewrite();
		if (fCreateInstanceField && !remove) {
			if (unit.getAllTypes()[0].getElementName().equals(
					fType.getElementName())) {
				// Only allow "normal" superconstructor invocation modification
				// if we are
				// in the same compilation unit
				this.insertExpressionAsParameter(invocation, rewrite, unit,
						group);
			} else {
				// Added for extreme case but not always work ...
				// because we can't guarantee that compilation unit are
				// processed in the same order ....
				final String fname = unit.getAllTypes()[0].getElementName();
				if (invocation.getExpression() != null) {
					rewrite.getListRewrite(invocation,
							SuperConstructorInvocation.ARGUMENTS_PROPERTY)
							.insertFirst(
									ASTNode.copySubtree(rewrite.getAST(),
											invocation.getExpression()), group);
				} else {
					final Expression expression = (Expression) rewrite
							.createStringPlaceholder(fname + ".this",
									ASTNode.FIELD_ACCESS);
					rewrite.getListRewrite(invocation,
							SuperConstructorInvocation.ARGUMENTS_PROPERTY)
							.insertFirst(expression, group);
				}
			}
		}
		final Expression expression = invocation.getExpression();
		if (expression != null) {
			rewrite.remove(expression, null);
			targetRewrite.getImportRemover().registerRemovedNode(expression);
		}
	}

	/*
	 * private boolean updateNameReference(ITypeBinding[] parameters, Name name,
	 * CompilationUnitRewrite targetRewrite, TextEditGroup group) { if
	 * (ASTNodes.asString(name).equals(fType.getFullyQualifiedName('.'))) {
	 * targetRewrite.getASTRewrite().replace(name,
	 * getNewQualifiedNameNode(parameters, name), group);
	 * targetRewrite.getImportRemover().registerRemovedNode(name); return true; }
	 * targetRewrite.getASTRewrite().replace(name,
	 * getNewUnqualifiedTypeNode(parameters, name), group);
	 * targetRewrite.getImportRemover().registerRemovedNode(name); return true; }
	 * 
	 * private boolean updateParameterizedTypeReference(ITypeBinding[]
	 * parameters, ParameterizedType type, CompilationUnitRewrite targetRewrite,
	 * TextEditGroup group) { if (!(type.getParent() instanceof
	 * ClassInstanceCreation)) { final ListRewrite rewrite =
	 * targetRewrite.getASTRewrite() .getListRewrite(type,
	 * ParameterizedType.TYPE_ARGUMENTS_PROPERTY); final AST ast =
	 * targetRewrite.getRoot().getAST(); Type simpleType = null; for (int index =
	 * type.typeArguments().size(); index < parameters.length; index++) {
	 * simpleType = ast.newSimpleType(ast
	 * .newSimpleName(parameters[index].getName()));
	 * rewrite.insertLast(simpleType, group); } } return true; }
	 */
	/*
	 * private boolean updateReference(ITypeBinding[] parameters, ASTNode node,
	 * CompilationUnitRewrite rewrite, TextEditGroup group) { if
	 * (node.getLocationInParent() == ParameterizedType.TYPE_PROPERTY) {
	 * updateParameterizedTypeReference(parameters, (ParameterizedType)
	 * node.getParent(), rewrite, group); return updateNameReference(new
	 * ITypeBinding[] {}, ((SimpleType) node).getName(), rewrite, group); } else
	 * if (node instanceof QualifiedName) return updateNameReference(parameters,
	 * (QualifiedName) node, rewrite, group); else if (node instanceof
	 * SimpleType) return updateNameReference(parameters, ((SimpleType) node)
	 * .getName(), rewrite, group); else return false; }
	 * 
	 * private void updateReferenceInImport(ImportDeclaration enclosingImport,
	 * ASTNode node, CompilationUnitRewrite rewrite) throws CoreException {
	 * final IBinding binding = enclosingImport.resolveBinding(); if (binding
	 * instanceof ITypeBinding) { final ITypeBinding type = (ITypeBinding)
	 * binding; final ImportRewrite rewriter = rewrite.getImportRewrite(); if
	 * (enclosingImport.isStatic()) { final String oldImport =
	 * ASTNodes.asString(node); final StringBuffer buffer = new
	 * StringBuffer(oldImport); final String typeName = fType.getDeclaringType()
	 * .getElementName(); final int index = buffer.indexOf(typeName); if (index >=
	 * 0) { buffer.delete(index, index + typeName.length() + 1); final String
	 * newImport = buffer.toString(); if (enclosingImport.isOnDemand()) {
	 * rewriter.removeStaticImport(oldImport + ".*"); //$NON-NLS-1$
	 * rewriter.addStaticImport(newImport, "*", false); //$NON-NLS-1$ } else {
	 * rewriter.removeStaticImport(oldImport); final int offset =
	 * newImport.lastIndexOf('.'); if (offset >= 0 && offset <
	 * newImport.length() - 1) { rewriter.addStaticImport(newImport.substring(0,
	 * offset), newImport.substring(offset + 1), false); } } } } else
	 * rewriter.removeImport(type.getQualifiedName()); } }
	 */

	public RefactoringStatus initialize(final JavaRefactoringArguments arguments) {
		if (arguments instanceof JavaRefactoringArguments) {
			final JavaRefactoringArguments extended = arguments;
			final String handle = extended
					.getAttribute(JavaRefactoringDescriptorUtil.ATTRIBUTE_INPUT);
			if (handle != null) {
				final IJavaElement element = JavaRefactoringDescriptorUtil
						.handleToElement(extended.getProject(), handle, false);
				if (element == null || !element.exists()
						|| element.getElementType() != IJavaElement.TYPE) {
					return JavaRefactoringDescriptorUtil.createInputFatalStatus(element, getName(), ConvertNestedToInnerRefactoring.ID_MOVE_INNER);
				} else {
					fType = (IType) element;
					fCodeGenerationSettings = new CodeGenerationSettings(); /*
																			 * JavaPreferencesSettings
																			 * .getCodeGenerationSettings(javaProject);
																			 */
					try {
						this.initialize();
					} catch (final JavaModelException exception) {
						JavaPlugin.log(exception);
					}
				}
			} else {
				return RefactoringStatus
						.createFatalErrorStatus(Messages
								.format(
										RefactoringCoreMessages.InitializableRefactoring_argument_not_exist,
										JavaRefactoringDescriptorUtil.ATTRIBUTE_INPUT));
			}
			final String fieldName = extended
					.getAttribute(ConvertNestedToInnerRefactoring.ATTRIBUTE_FIELD_NAME);
			if (fieldName != null && !"".equals(fieldName)) {
				fEnclosingInstanceFieldName = fieldName;
			}
			final String parameterName = extended
					.getAttribute(ConvertNestedToInnerRefactoring.ATTRIBUTE_PARAMETER_NAME);
			if (parameterName != null && !"".equals(parameterName)) {
				fNameForEnclosingInstanceConstructorParameter = parameterName;
			}
			final String createField = extended
					.getAttribute(ConvertNestedToInnerRefactoring.ATTRIBUTE_FIELD);
			if (createField != null) {
				fCreateInstanceField = Boolean.valueOf(createField)
						.booleanValue();
			} else {
				return RefactoringStatus
						.createFatalErrorStatus(Messages
								.format(
										RefactoringCoreMessages.InitializableRefactoring_argument_not_exist,
										ConvertNestedToInnerRefactoring.ATTRIBUTE_FIELD));
			}
			final String markFinal = extended
					.getAttribute(ConvertNestedToInnerRefactoring.ATTRIBUTE_FINAL);
			if (markFinal != null) {
				fMarkInstanceFieldAsFinal = Boolean.valueOf(markFinal)
						.booleanValue();
			} else {
				return RefactoringStatus
						.createFatalErrorStatus(Messages
								.format(
										RefactoringCoreMessages.InitializableRefactoring_argument_not_exist,
										ConvertNestedToInnerRefactoring.ATTRIBUTE_FINAL));
			}
			final String possible = extended
					.getAttribute(ConvertNestedToInnerRefactoring.ATTRIBUTE_POSSIBLE);
			if (possible != null) {
				fIsInstanceFieldCreationPossible = Boolean.valueOf(possible)
						.booleanValue();
			} else {
				return RefactoringStatus
						.createFatalErrorStatus(Messages
								.format(
										RefactoringCoreMessages.InitializableRefactoring_argument_not_exist,
										ConvertNestedToInnerRefactoring.ATTRIBUTE_POSSIBLE));
			}
			final String mandatory = extended
					.getAttribute(ConvertNestedToInnerRefactoring.ATTRIBUTE_MANDATORY);
			if (mandatory != null) {
				fIsInstanceFieldCreationMandatory = Boolean.valueOf(mandatory)
						.booleanValue();
			} else {
				return RefactoringStatus
						.createFatalErrorStatus(Messages
								.format(
										RefactoringCoreMessages.InitializableRefactoring_argument_not_exist,
										ConvertNestedToInnerRefactoring.ATTRIBUTE_MANDATORY));
			}
		} else {
			return RefactoringStatus
					.createFatalErrorStatus(RefactoringCoreMessages.InitializableRefactoring_inacceptable_arguments);
		}
		return new RefactoringStatus();
	}
}
