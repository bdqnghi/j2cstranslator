package com.ilog.translator.java2cs.translation.astrewriter.astchange;

import java.util.ArrayList;
import java.util.Arrays;
import java.util.Collections;
import java.util.HashMap;
import java.util.HashSet;
import java.util.Iterator;
import java.util.List;
import java.util.Map;
import java.util.Set;
import java.util.StringTokenizer;

import org.eclipse.core.filebuffers.ITextFileBuffer;
import org.eclipse.core.runtime.Assert;
import org.eclipse.core.runtime.CoreException;
import org.eclipse.core.runtime.IProgressMonitor;
import org.eclipse.core.runtime.NullProgressMonitor;
import org.eclipse.jdt.core.Flags;
import org.eclipse.jdt.core.ICompilationUnit;
import org.eclipse.jdt.core.IJavaElement;
import org.eclipse.jdt.core.IJavaProject;
import org.eclipse.jdt.core.IMethod;
import org.eclipse.jdt.core.JavaModelException;
import org.eclipse.jdt.core.NamingConventions;
import org.eclipse.jdt.core.dom.AST;
import org.eclipse.jdt.core.dom.ASTNode;
import org.eclipse.jdt.core.dom.ASTVisitor;
import org.eclipse.jdt.core.dom.AbstractTypeDeclaration;
import org.eclipse.jdt.core.dom.AnonymousClassDeclaration;
import org.eclipse.jdt.core.dom.Assignment;
import org.eclipse.jdt.core.dom.Block;
import org.eclipse.jdt.core.dom.BodyDeclaration;
import org.eclipse.jdt.core.dom.ClassInstanceCreation;
import org.eclipse.jdt.core.dom.CompilationUnit;
import org.eclipse.jdt.core.dom.Expression;
import org.eclipse.jdt.core.dom.ExpressionStatement;
import org.eclipse.jdt.core.dom.FieldAccess;
import org.eclipse.jdt.core.dom.FieldDeclaration;
import org.eclipse.jdt.core.dom.IBinding;
import org.eclipse.jdt.core.dom.IMethodBinding;
import org.eclipse.jdt.core.dom.ITypeBinding;
import org.eclipse.jdt.core.dom.IVariableBinding;
import org.eclipse.jdt.core.dom.ImportDeclaration;
import org.eclipse.jdt.core.dom.Initializer;
import org.eclipse.jdt.core.dom.Javadoc;
import org.eclipse.jdt.core.dom.MethodDeclaration;
import org.eclipse.jdt.core.dom.MethodInvocation;
import org.eclipse.jdt.core.dom.Modifier;
import org.eclipse.jdt.core.dom.Name;
import org.eclipse.jdt.core.dom.ParameterizedType;
import org.eclipse.jdt.core.dom.QualifiedName;
import org.eclipse.jdt.core.dom.SimpleName;
import org.eclipse.jdt.core.dom.SingleVariableDeclaration;
import org.eclipse.jdt.core.dom.SuperConstructorInvocation;
import org.eclipse.jdt.core.dom.SuperFieldAccess;
import org.eclipse.jdt.core.dom.TagElement;
import org.eclipse.jdt.core.dom.TextElement;
import org.eclipse.jdt.core.dom.ThisExpression;
import org.eclipse.jdt.core.dom.Type;
import org.eclipse.jdt.core.dom.TypeDeclaration;
import org.eclipse.jdt.core.dom.TypeParameter;
import org.eclipse.jdt.core.dom.VariableDeclarationFragment;
import org.eclipse.jdt.core.dom.rewrite.ASTRewrite;
import org.eclipse.jdt.core.dom.rewrite.ITrackedNodePosition;
import org.eclipse.jdt.core.refactoring.descriptors.ConvertAnonymousDescriptor;
import org.eclipse.jdt.core.refactoring.descriptors.JavaRefactoringDescriptor;
import org.eclipse.jdt.internal.corext.codemanipulation.CodeGenerationSettings;
import org.eclipse.jdt.internal.corext.codemanipulation.StubUtility;
import org.eclipse.jdt.internal.corext.dom.ASTNodeFactory;
import org.eclipse.jdt.internal.corext.dom.ASTNodes;
import org.eclipse.jdt.internal.corext.dom.Bindings;
import org.eclipse.jdt.internal.corext.dom.NodeFinder;
import org.eclipse.jdt.internal.corext.refactoring.Checks;
import org.eclipse.jdt.internal.corext.refactoring.JDTRefactoringDescriptorComment;
import org.eclipse.jdt.internal.corext.refactoring.JavaRefactoringArguments;
import org.eclipse.jdt.internal.corext.refactoring.JavaRefactoringDescriptorUtil;
import org.eclipse.jdt.internal.corext.refactoring.RefactoringCoreMessages;
import org.eclipse.jdt.internal.corext.refactoring.changes.CompilationUnitChange;
import org.eclipse.jdt.internal.corext.refactoring.structure.CompilationUnitRewrite;
import org.eclipse.jdt.internal.corext.refactoring.util.RefactoringASTParser;
import org.eclipse.jdt.internal.corext.refactoring.util.RefactoringFileBuffers;
import org.eclipse.jdt.internal.corext.refactoring.util.ResourceUtil;
import org.eclipse.jdt.internal.corext.util.JdtFlags;
import org.eclipse.jdt.internal.corext.util.Messages;
import org.eclipse.jdt.internal.corext.util.Strings;
import org.eclipse.jdt.internal.ui.JavaPlugin;
import org.eclipse.jdt.internal.ui.preferences.JavaPreferencesSettings;
import org.eclipse.jdt.internal.ui.viewsupport.BindingLabelProvider;
import org.eclipse.jdt.ui.CodeGeneration;
import org.eclipse.jdt.ui.JavaElementLabels;
import org.eclipse.jface.text.BadLocationException;
import org.eclipse.jface.text.Document;
import org.eclipse.jface.text.IDocument;
import org.eclipse.ltk.core.refactoring.Change;
import org.eclipse.ltk.core.refactoring.Refactoring;
import org.eclipse.ltk.core.refactoring.RefactoringChangeDescriptor;
import org.eclipse.ltk.core.refactoring.RefactoringDescriptor;
import org.eclipse.ltk.core.refactoring.RefactoringStatus;
import org.eclipse.text.edits.MalformedTreeException;
import org.eclipse.text.edits.TextEdit;

import com.ilog.translator.java2cs.translation.ITranslationContext;
import com.ilog.translator.java2cs.translation.util.TranslationUtils;

public class ConvertAnonymousToInnerRefactoring extends Refactoring {

	private static final String ID_CONVERT_ANONYMOUS = "org.eclipse.jdt.ui.convert.anonymoustoInner"; //$NON-NLS-1$

	private static final String ATTRIBUTE_VISIBILITY = "visibility"; //$NON-NLS-1$

	private static final String ATTRIBUTE_FINAL = "final"; //$NON-NLS-1$

	private static final String ATTRIBUTE_STATIC = "static"; //$NON-NLS-1$

	public static class TypeVariableFinder extends ASTVisitor {

		@SuppressWarnings("unchecked")
		private final Map fBindings = new HashMap();

		@SuppressWarnings("unchecked")
		private final List fFound = new ArrayList();

		@Override
		@SuppressWarnings("unchecked")
		public final boolean visit(final SimpleName node) {
			Assert.isNotNull(node);
			final ITypeBinding binding = node.resolveTypeBinding();
			if (binding != null && binding.isTypeVariable()
					&& !fBindings.containsKey(binding.getKey())) {
				fBindings.put(binding.getKey(), binding);
				fFound.add(binding);
			}
			return true;
		}

		@SuppressWarnings("unchecked")
		public final ITypeBinding[] getResult() {
			final ITypeBinding[] result = new ITypeBinding[fFound.size()];
			fFound.toArray(result);
			return result;
		}
	}

	private int fSelectionStart;

	private int fSelectionLength;

	private ICompilationUnit fCu;

	private int fVisibility; /* see Modifier */

	private boolean fDeclareFinal = true;

	private boolean fDeclareStatic;

	private String fClassName = ""; //$NON-NLS-1$

	private CodeGenerationSettings fSettings;

	private CompilationUnit fCompilationUnitNode;

	private AnonymousClassDeclaration fAnonymousInnerClassNode;

	@SuppressWarnings("unchecked")
	private Set fClassNamesUsed;

	private boolean fSelfInitializing = false;

	//
	//
	//

	private boolean faccessEnclosing = false;

	// private ITypeBinding fEnclosingType;
	private VariableDeclarationFragment outerThis;

	// private List<ASTNode> fNodeToBeReplaced;

	private final ITranslationContext fContext;

	private String fCreatedTypeName;

	private boolean ignored = true;

	/**
	 * Creates a new convert anonymous to nested refactoring
	 * 
	 * @param unit
	 *            the compilation unit, or <code>null</code> if invoked by
	 *            scripting
	 * @param settings
	 *            the code generation settings
	 * @param selectionStart
	 * @param selectionLength
	 */
	public ConvertAnonymousToInnerRefactoring(ICompilationUnit unit,
			CodeGenerationSettings settings, int selectionStart,
			int selectionLength, ITranslationContext context, boolean ignored) {
		Assert.isTrue(selectionStart >= 0);
		Assert.isTrue(selectionLength >= 0);
		Assert.isTrue(unit == null || unit.exists());
		fSelectionStart = selectionStart;
		fSelectionLength = selectionLength;
		fContext = context;
		fCu = unit;
		this.ignored = ignored;
		if (unit != null) {
			fSettings = settings;
		}
	}

	public int[] getAvailableVisibilities() {
		if (isLocalInnerType()) {
			return new int[] { Modifier.NONE };
		} else {
			return new int[] { Modifier.PUBLIC, Modifier.PROTECTED,
					Modifier.NONE, Modifier.PRIVATE };
		}
	}

	public boolean isLocalInnerType() {
		return ASTNodes.getParent(ASTNodes.getParent(fAnonymousInnerClassNode,
				AbstractTypeDeclaration.class),
				ASTNode.ANONYMOUS_CLASS_DECLARATION) != null;
	}

	public int getVisibility() {
		return fVisibility;
	}

	public void setVisibility(int visibility) {
		/*Assert.isTrue(visibility == Modifier.PRIVATE
				|| visibility == Modifier.NONE
				|| visibility == Modifier.PROTECTED
				|| visibility == Modifier.PUBLIC);*/
		fVisibility = visibility;
	}

	public void setClassName(String className) {
		Assert.isNotNull(className);
		fClassName = className;
	}

	public boolean canEnableSettingFinal() {
		return true;
	}

	public boolean getDeclareFinal() {
		return fDeclareFinal;
	}

	public boolean getDeclareStatic() {
		return fDeclareStatic;
	}

	public void setDeclareFinal(boolean declareFinal) {
		fDeclareFinal = declareFinal;
	}

	public void setDeclareStatic(boolean declareStatic) {
		fDeclareStatic = declareStatic;
	}

	@Override
	public String getName() {
		return RefactoringCoreMessages.ConvertAnonymousToNestedRefactoring_name;
	}

	@Override
	public RefactoringStatus checkInitialConditions(IProgressMonitor pm)
			throws CoreException {
		final RefactoringStatus result = Checks.validateModifiesFiles(
				ResourceUtil.getFiles(new ICompilationUnit[] { fCu }),
				getValidationContext());
		if (result.hasFatalError()) {
			return result;
		}

		initAST(pm);

		//
		//
		//

		final AbstractTypeDeclaration declarations = (AbstractTypeDeclaration) ASTNodes
				.getParent(fAnonymousInnerClassNode,
						AbstractTypeDeclaration.class);

		final SearchForEnclosingAccess v = new SearchForEnclosingAccess(
				declarations, fContext, true);
		v.initTypeHierarchy(fAnonymousInnerClassNode.resolveBinding());
		fAnonymousInnerClassNode.accept(v);
		faccessEnclosing = v.hasEnclosingAccess();

		// List<ASTNode> fNodeToBeReplaced = v.getNodeToRewriteList();

		//
		//
		//

		if (fAnonymousInnerClassNode == null) {
			return RefactoringStatus
					.createFatalErrorStatus(RefactoringCoreMessages.ConvertAnonymousToNestedRefactoring_place_caret);
		}
		if (!fSelfInitializing) {
			initializeDefaults();
		}
		if (getSuperConstructorBinding() == null) {
			return RefactoringStatus
					.createFatalErrorStatus(RefactoringCoreMessages.ConvertAnonymousToNestedRefactoring_compile_errors);
		}
		if (getSuperTypeBinding().isLocal()) {
			return RefactoringStatus
					.createFatalErrorStatus(RefactoringCoreMessages.ConvertAnonymousToNestedRefactoring_extends_local_class);
		}
		return new RefactoringStatus();
	}

	private void initializeDefaults() {
		fVisibility = isLocalInnerType() ? Modifier.NONE : Modifier.PRIVATE;
		fDeclareStatic = mustInnerClassBeStatic();
	}

	@SuppressWarnings("unchecked")
	private void initAST(IProgressMonitor pm) {
		// TODO:
		fCompilationUnitNode = /*
								 * new
								 * RefactoringASTParser(ASTProvider.SHARED_AST_LEVEL).parse(fCu,
								 * null, true,
								 * ASTProvider.SHARED_AST_STATEMENT_RECOVERY,
								 * pm);
								 */
		RefactoringASTParser.parseWithASTProvider(fCu, true, pm);
		fAnonymousInnerClassNode = ConvertAnonymousToInnerRefactoring
				.getAnonymousInnerClass(NodeFinder
						.perform(fCompilationUnitNode, fSelectionStart,
								fSelectionLength));
		if (fAnonymousInnerClassNode != null) {
			final AbstractTypeDeclaration declaration = (AbstractTypeDeclaration) ASTNodes
					.getParent(fAnonymousInnerClassNode,
							AbstractTypeDeclaration.class);
			if (declaration instanceof TypeDeclaration) {
				final AbstractTypeDeclaration[] nested = ((TypeDeclaration) declaration)
						.getTypes();
				fClassNamesUsed = new HashSet(nested.length);
				for (final AbstractTypeDeclaration element : nested) {
					fClassNamesUsed.add(element.getName().getIdentifier());
				}
			} else {
				fClassNamesUsed = Collections.EMPTY_SET;
			}
		}
	}

	private static AnonymousClassDeclaration getAnonymousInnerClass(ASTNode node) {
		if (node == null) {
			return null;
		}
		if (node instanceof AnonymousClassDeclaration) {
			return (AnonymousClassDeclaration) node;
		}
		if (node instanceof ClassInstanceCreation) {
			final AnonymousClassDeclaration anon = ((ClassInstanceCreation) node)
					.getAnonymousClassDeclaration();
			if (anon != null) {
				return anon;
			}
		}
		node = ASTNodes.getNormalizedNode(node);
		if (node.getLocationInParent() == ClassInstanceCreation.TYPE_PROPERTY) {
			final AnonymousClassDeclaration anon = ((ClassInstanceCreation) node
					.getParent()).getAnonymousClassDeclaration();
			if (anon != null) {
				return anon;
			}
		}
		return (AnonymousClassDeclaration) ASTNodes.getParent(node,
				AnonymousClassDeclaration.class);
	}

	public RefactoringStatus validateInput() {
		RefactoringStatus result = Checks.checkTypeName(this.fClassName, fCu);
		if (result.hasFatalError()) {
			return result;
		}

		if (fClassNamesUsed.contains(fClassName)) {
			return RefactoringStatus
					.createFatalErrorStatus(RefactoringCoreMessages.ConvertAnonymousToNestedRefactoring_type_exists);
		}
		final IMethodBinding superConstructorBinding = getSuperConstructorBinding();
		if (superConstructorBinding == null) {
			return RefactoringStatus
					.createFatalErrorStatus(RefactoringCoreMessages.ConvertAnonymousToNestedRefactoring_compile_errors);
		}
		if (fClassName.equals(superConstructorBinding.getDeclaringClass()
				.getName())) {
			return RefactoringStatus
					.createFatalErrorStatus(RefactoringCoreMessages.ConvertAnonymousToNestedRefactoring_another_name);
		}
		if (classNameHidesEnclosingType()) {
			return RefactoringStatus
					.createFatalErrorStatus(RefactoringCoreMessages.ConvertAnonymousToNestedRefactoring_name_hides);
		}
		return result;
	}

	@SuppressWarnings("unchecked")
	private boolean accessesAnonymousFields() {
		final List anonymousInnerFieldTypes = getAllEnclosingAnonymousTypesField();
		final List accessedField = getAllAccessedFields();
		final Iterator it = anonymousInnerFieldTypes.iterator();
		while (it.hasNext()) {
			final IVariableBinding variableBinding = (IVariableBinding) it
					.next();
			final Iterator it2 = accessedField.iterator();
			while (it2.hasNext()) {
				final IVariableBinding variableBinding2 = (IVariableBinding) it2
						.next();
				if (Bindings.equals(variableBinding, variableBinding2)) {
					return true;
				}
			}
		}
		return false;
	}

	@SuppressWarnings("unchecked")
	private List getAllAccessedFields() {
		final List accessedFields = new ArrayList();

		final ASTVisitor visitor = new ASTVisitor() {

			@Override
			public boolean visit(FieldAccess node) {
				final IVariableBinding binding = node.resolveFieldBinding();
				if (binding != null && !binding.isEnumConstant()) {
					accessedFields.add(binding);
				}
				return super.visit(node);
			}

			@Override
			public boolean visit(QualifiedName node) {
				final IBinding binding = node.resolveBinding();
				if (binding != null && binding instanceof IVariableBinding) {
					IVariableBinding variable = (IVariableBinding) binding;
					if (!variable.isEnumConstant() && variable.isField()) {
						accessedFields.add(binding);
					}
				}
				return super.visit(node);
			}

			@Override
			public boolean visit(SimpleName node) {
				final IBinding binding = node.resolveBinding();
				if (binding != null && binding instanceof IVariableBinding) {
					IVariableBinding variable = (IVariableBinding) binding;
					if (!variable.isEnumConstant() && variable.isField()) {
						accessedFields.add(binding);
					}
				}
				return super.visit(node);
			}

			@Override
			public boolean visit(SuperFieldAccess node) {
				final IVariableBinding binding = node.resolveFieldBinding();
				if (binding != null && !binding.isEnumConstant()) {
					accessedFields.add(binding);
				}
				return super.visit(node);
			}
		};
		fAnonymousInnerClassNode.accept(visitor);

		return accessedFields;
	}

	@SuppressWarnings("unchecked")
	private List getAllEnclosingAnonymousTypesField() {
		final List ans = new ArrayList();
		final AbstractTypeDeclaration declaration = (AbstractTypeDeclaration) ASTNodes
				.getParent(fAnonymousInnerClassNode,
						AbstractTypeDeclaration.class);
		AnonymousClassDeclaration anonymous = (AnonymousClassDeclaration) ASTNodes
				.getParent(fAnonymousInnerClassNode,
						ASTNode.ANONYMOUS_CLASS_DECLARATION);
		while (anonymous != null) {
			if (ASTNodes.isParent(anonymous, declaration)) {
				final ITypeBinding binding = anonymous.resolveBinding();
				if (binding != null) {
					ans.addAll(Arrays.asList(binding.getDeclaredFields()));
				}
			} else {
				break;
			}
			anonymous = (AnonymousClassDeclaration) ASTNodes.getParent(
					anonymous, ASTNode.ANONYMOUS_CLASS_DECLARATION);
		}
		return ans;
	}

	private boolean classNameHidesEnclosingType() {
		ITypeBinding type = ((AbstractTypeDeclaration) ASTNodes.getParent(
				fAnonymousInnerClassNode, AbstractTypeDeclaration.class))
				.resolveBinding();
		while (type != null) {
			if (fClassName.equals(type.getName())) {
				return true;
			}
			type = type.getDeclaringClass();
		}
		return false;
	}

	/*
	 * @see org.eclipse.jdt.internal.corext.refactoring.base.Refactoring#checkInput(org.eclipse.core.runtime.IProgressMonitor)
	 */
	@Override
	public RefactoringStatus checkFinalConditions(IProgressMonitor pm)
			throws CoreException {
		try {
			final RefactoringStatus status = validateInput();
			if (accessesAnonymousFields()) {
				status
						.merge(RefactoringStatus
								.createErrorStatus(RefactoringCoreMessages.ConvertAnonymousToNestedRefactoring_anonymous_field_access));
			}
			return status;
		} finally {
			pm.done();
		}
	}

	/*
	 * @see org.eclipse.jdt.internal.corext.refactoring.base.IRefactoring#createChange(org.eclipse.core.runtime.IProgressMonitor)
	 */
	@Override
	public Change createChange(IProgressMonitor pm) throws CoreException {
		pm.beginTask("", 1); //$NON-NLS-1$
		try {
			final CompilationUnitRewrite rewrite = new CompilationUnitRewrite(
					fCu, fCompilationUnitNode);
			final ITypeBinding[] parameters = getTypeParameters();
			addNestedClass(rewrite, parameters);
			modifyConstructorCall(rewrite, parameters);
			return this.createChange(rewrite);
		} finally {
			pm.done();
		}
	}

	@SuppressWarnings("unchecked")
	private ITypeBinding[] getTypeParameters() {
		final List parameters = new ArrayList(4);
		// TODO:
		if (fAnonymousInnerClassNode.getParent() instanceof ClassInstanceCreation) {
			final ClassInstanceCreation creation = (ClassInstanceCreation) fAnonymousInnerClassNode
					.getParent();
			if (fDeclareStatic) {
				final TypeVariableFinder finder = new TypeVariableFinder();
				creation.accept(finder);
				return finder.getResult();
			} else {
				final MethodDeclaration declaration = getEnclosingMethodDeclaration(creation);
				if (declaration != null) {
					ITypeBinding binding = null;
					TypeParameter parameter = null;
					for (final Iterator iterator = declaration.typeParameters()
							.iterator(); iterator.hasNext();) {
						parameter = (TypeParameter) iterator.next();
						binding = parameter.resolveBinding();
						if (binding != null) {
							parameters.add(binding);
						}
					}
				}
			}
			final TypeVariableFinder finder = new TypeVariableFinder();
			creation.accept(finder);
			final ITypeBinding[] variables = finder.getResult();
			final List remove = new ArrayList(4);
			boolean match = false;
			ITypeBinding binding = null;
			ITypeBinding variable = null;
			for (final Iterator iterator = parameters.iterator(); iterator
					.hasNext();) {
				match = false;
				binding = (ITypeBinding) iterator.next();
				for (final ITypeBinding element : variables) {
					variable = element;
					if (variable.equals(binding)) {
						match = true;
					}
				}
				if (!match) {
					remove.add(binding);
				}
			}
			parameters.removeAll(remove);
			final ITypeBinding[] result = new ITypeBinding[parameters.size()];
			parameters.toArray(result);
			return result;
		} else {
			System.err.println("COnvertAnonymous.getTypeParameters "
					+ fAnonymousInnerClassNode.getParent());
			return null;
		}
	}

	private MethodDeclaration getEnclosingMethodDeclaration(ASTNode node) {
		final ASTNode parent = node.getParent();
		if (parent != null) {
			if (parent instanceof AbstractTypeDeclaration) {
				return null;
			} else if (parent instanceof MethodDeclaration) {
				return (MethodDeclaration) parent;
			}
			return getEnclosingMethodDeclaration(parent);
		}
		return null;
	}

	@SuppressWarnings("unchecked")
	private Change createChange(CompilationUnitRewrite rewrite)
			throws CoreException {
		final ITypeBinding binding = fAnonymousInnerClassNode.resolveBinding();
		final String[] labels = new String[] {
				BindingLabelProvider.getBindingLabel(binding,
						JavaElementLabels.ALL_FULLY_QUALIFIED),
				BindingLabelProvider.getBindingLabel(binding
						.getDeclaringMethod(),
						JavaElementLabels.ALL_FULLY_QUALIFIED) };
		final Map arguments = new HashMap();
		String project = null;
		final IJavaProject javaProject = fCu.getJavaProject();
		if (javaProject != null) {
			project = javaProject.getElementName();
		}
		final int flags = RefactoringDescriptor.STRUCTURAL_CHANGE
				| JavaRefactoringDescriptor.JAR_REFACTORING
				| JavaRefactoringDescriptor.JAR_SOURCE_ATTACHMENT;
		final String description = RefactoringCoreMessages.ConvertAnonymousToNestedRefactoring_descriptor_description_short;
		final String header = Messages
				.format(
						RefactoringCoreMessages.ConvertAnonymousToNestedRefactoring_descriptor_description,
						labels);
		final JDTRefactoringDescriptorComment comment = new JDTRefactoringDescriptorComment(
				"id", this, header);
		comment
				.addSetting(Messages
						.format(
								RefactoringCoreMessages.ConvertAnonymousToNestedRefactoring_original_pattern,
								BindingLabelProvider.getBindingLabel(binding,
										JavaElementLabels.ALL_FULLY_QUALIFIED)));
		comment
				.addSetting(Messages
						.format(
								RefactoringCoreMessages.ConvertAnonymousToNestedRefactoring_class_name_pattern,
								fClassName));
		String visibility = JdtFlags.getVisibilityString(fVisibility);
		if ("".equals(visibility)) {
			visibility = RefactoringCoreMessages.ConvertAnonymousToNestedRefactoring_default_visibility;
		}
		comment
				.addSetting(Messages
						.format(
								RefactoringCoreMessages.ConvertAnonymousToNestedRefactoring_visibility_pattern,
								visibility));
		if (fDeclareFinal && fDeclareStatic) {
			comment
					.addSetting(RefactoringCoreMessages.ConvertAnonymousToNestedRefactoring_declare_final_static);
		} else if (fDeclareFinal) {
			comment
					.addSetting(RefactoringCoreMessages.ConvertAnonymousToNestedRefactoring_declare_final);
		} else if (fDeclareStatic) {
			comment
					.addSetting(RefactoringCoreMessages.ConvertAnonymousToNestedRefactoring_declare_static);
		}
		final ConvertAnonymousDescriptor descriptor = new ConvertAnonymousDescriptor(project, description, comment.asString(),
				arguments, flags);
		arguments.put(JavaRefactoringDescriptorUtil.ATTRIBUTE_INPUT, JavaRefactoringDescriptorUtil
				.elementToHandle(project, this.fCu));
		arguments.put(JavaRefactoringDescriptorUtil.ATTRIBUTE_NAME, this.fClassName);
		arguments.put(JavaRefactoringDescriptorUtil.ATTRIBUTE_SELECTION,
				new Integer(fSelectionStart).toString()
						+ " " + new Integer(fSelectionLength).toString()); //$NON-NLS-1$
		arguments.put(ConvertAnonymousToInnerRefactoring.ATTRIBUTE_FINAL,
				Boolean.valueOf(fDeclareFinal).toString());
		arguments.put(ConvertAnonymousToInnerRefactoring.ATTRIBUTE_STATIC,
				Boolean.valueOf(fDeclareStatic).toString());
		arguments.put(ConvertAnonymousToInnerRefactoring.ATTRIBUTE_VISIBILITY,
				new Integer(fVisibility).toString());

		final RefactoringChangeDescriptor ch = new RefactoringChangeDescriptor(
				descriptor);
		/*
		 * final CompilationUnitChange cuChange = new CompilationUnitChange("",
		 * this.fCu); MultiTextEdit multiEdit = new MultiTextEdit();
		 * cuChange.setEdit(multiEdit); TextEdit rewriteEdit =
		 * rewrite.getASTRewrite().rewriteAST();
		 * multiEdit.addChild(rewriteEdit); cuChange.setDescriptor(ch); return
		 * cuChange;
		 */
		final NullProgressMonitor pm = new NullProgressMonitor();
		final CompilationUnitChange cuCh = rewrite
				.createChange(
						RefactoringCoreMessages.ConvertAnonymousToNestedRefactoring_name,
						false, pm);
		cuCh.setDescriptor(ch);
		return cuCh;

		/*
		 * try { ITextFileBuffer buffer =
		 * RefactoringFileBuffers.acquire(this.fCu); TextEdit resultingEdits =
		 * rewrite.getASTRewrite() .rewriteAST(buffer.getDocument(),
		 * this.fCu.getJavaProject().getOptions(true)); TextChangeCompatibility
		 * .addTextEdit( result,
		 * RefactoringCoreMessages.ConvertAnonymousToNestedRefactoring_edit_name,
		 * resultingEdits); } finally {
		 * RefactoringFileBuffers.release(this.fCu); } return result;
		 */
	}

	private void modifyConstructorCall(CompilationUnitRewrite rewrite,
			ITypeBinding[] parameters) {
		rewrite.getASTRewrite().replace(fAnonymousInnerClassNode.getParent(),
				createNewClassInstanceCreation(rewrite, parameters), null);
	}

	@SuppressWarnings("unchecked")
	private ASTNode createNewClassInstanceCreation(
			CompilationUnitRewrite rewrite, ITypeBinding[] parameters) {
		final AST ast = fAnonymousInnerClassNode.getAST();
		final ClassInstanceCreation newClassCreation = ast
				.newClassInstanceCreation();
		newClassCreation.setAnonymousClassDeclaration(null);
		Type type = null;
		if (parameters.length > 0) {
			final ParameterizedType parameterized = ast
					.newParameterizedType(ast.newSimpleType(ast
							.newSimpleName(fClassName)));
			for (final ITypeBinding element : parameters) {
				parameterized.typeArguments()
						.add(
								ast.newSimpleType(ast.newSimpleName(element
										.getName())));
			}
			type = parameterized;
		} else {
			type = ast.newSimpleType(ast.newSimpleName(fClassName));
		}
		newClassCreation.setType(type);
		copyArguments(rewrite, newClassCreation);
		addArgumentsForLocalsUsedInInnerClass(rewrite, newClassCreation);
		if (faccessEnclosing) {
			newClassCreation.arguments().add(
					rewrite.getASTRewrite().getAST().newThisExpression());
		}
		return newClassCreation;
	}

	@SuppressWarnings("unchecked")
	private void addArgumentsForLocalsUsedInInnerClass(
			CompilationUnitRewrite rewrite,
			ClassInstanceCreation newClassCreation) {
		final IVariableBinding[] usedLocals = getUsedLocalVariables();
		for (final IVariableBinding binding : usedLocals) {
			final AST ast = fAnonymousInnerClassNode.getAST();
			Name name = null;
			if (binding.isEnumConstant()) {
				name = ast.newQualifiedName(ast.newSimpleName(binding
						.getDeclaringClass().getName()), ast
						.newSimpleName(binding.getName()));
			} else {
				name = ast.newSimpleName(binding.getName());
			}
			newClassCreation.arguments().add(name);
		}
	}

	@SuppressWarnings("unchecked")
	private void copyArguments(CompilationUnitRewrite rewrite,
			ClassInstanceCreation newClassCreation) {
		for (final Iterator iter = ((ClassInstanceCreation) fAnonymousInnerClassNode
				.getParent()).arguments().iterator(); iter.hasNext();) {
			newClassCreation.arguments().add(
					rewrite.getASTRewrite().createCopyTarget(
							(Expression) iter.next()));
		}

	}

	private void addNestedClass(CompilationUnitRewrite rewrite,
			ITypeBinding[] parameters) throws CoreException {
		final AbstractTypeDeclaration declarations = (AbstractTypeDeclaration) ASTNodes
				.getParent(fAnonymousInnerClassNode,
						AbstractTypeDeclaration.class);
		int index = ConvertAnonymousToInnerRefactoring
				.findIndexOfFistNestedClass(declarations.bodyDeclarations());
		if (index == -1) {
			index = 0;
		}
		rewrite.getASTRewrite().getListRewrite(declarations,
				declarations.getBodyDeclarationsProperty()).insertAt(
				createNewNestedClass(rewrite, parameters), index, null);
		//
		//
		setCreatedTypeName(declarations.resolveBinding().getQualifiedName()
				+ "." + fClassName);
		//
		//
	}

	@SuppressWarnings("unchecked")
	private static int findIndexOfFistNestedClass(List bodyDeclarations) {
		for (int i = 0, n = bodyDeclarations.size(); i < n; i++) {
			final BodyDeclaration each = (BodyDeclaration) bodyDeclarations
					.get(i);
			if (ConvertAnonymousToInnerRefactoring.isNestedType(each)) {
				return i;
			}
		}
		return -1;
	}

	private static boolean isNestedType(BodyDeclaration each) {
		if (!(each instanceof AbstractTypeDeclaration)) {
			return false;
		}
		return (each.getParent() instanceof AbstractTypeDeclaration);
	}

	@SuppressWarnings("unchecked")
	private AbstractTypeDeclaration createNewNestedClass(
			CompilationUnitRewrite rewrite, ITypeBinding[] parameters)
			throws CoreException {
		final AST ast = fAnonymousInnerClassNode.getAST();
		final TypeDeclaration declaration = ast.newTypeDeclaration();
		declaration.setInterface(false);
		if (ignored) {
			final Javadoc jDoc = ast.newJavadoc();
			final TagElement tag = ast.newTagElement();
			tag.setTagName("@translator_mapping");
			final TextElement te = ast.newTextElement();
			te.setText("{ generation = false; }");
			tag.fragments().add(te);
			jDoc.tags().add(tag);
			declaration.setJavadoc(jDoc);
		} else
			declaration.setJavadoc(null);
		declaration.modifiers().addAll(
				ASTNodeFactory.newModifiers(ast,
						createModifiersForNestedClass()));
		declaration.setName(ast.newSimpleName(fClassName));
		TypeParameter parameter = null;
		for (final ITypeBinding element : parameters) {
			parameter = ast.newTypeParameter();
			parameter.setName(ast.newSimpleName(element.getName()));
			declaration.typeParameters().add(parameter);
		}
		setSuperType(declaration);
		removeInitializationFromDeclaredFields(rewrite);
		//
		if (faccessEnclosing) {
			createNewOuterThisField(rewrite, declaration);
		}
		//
		final IVariableBinding[] bindings = getUsedLocalVariables();
		final String[] fieldNames = computeFieldName(rewrite, declaration,
				bindings);
		//
		final String delimiter = StubUtility.getLineDelimiterUsed(fCu);
		ITextFileBuffer buffer = null;
		//
		try {
			buffer = RefactoringFileBuffers.acquire(fCu);
			final IDocument document = buffer.getDocument();
			for (final Iterator iterator = fAnonymousInnerClassNode
					.bodyDeclarations().iterator(); iterator.hasNext();) {
				final BodyDeclaration body = (BodyDeclaration) iterator.next();
				declaration.bodyDeclarations().add(
						createBodyDeclaration(rewrite, document, bindings,
								body, delimiter, fieldNames));
			}
		} finally {
			if (buffer != null) {
				RefactoringFileBuffers.release(fCu);
			}
		}

		createNewConstructorIfNeeded(rewrite, declaration, bindings, fieldNames);
		createFieldsForAccessedLocals(rewrite, declaration, bindings,
				fieldNames);

		if (fSettings.createComments) {
			final String[] parameterNames = new String[parameters.length];
			for (int index = 0; index < parameterNames.length; index++) {
				parameterNames[index] = parameters[index].getName();
			}
			final String string = CodeGeneration.getTypeComment(
					rewrite.getCu(), fClassName, parameterNames, StubUtility
							.getLineDelimiterUsed(rewrite.getCu())); /* TODO: */
			if (string != null) {
				final Javadoc javadoc = (Javadoc) rewrite.getASTRewrite()
						.createStringPlaceholder(string, ASTNode.JAVADOC);
				declaration.setJavadoc(javadoc);
			}
		}

		return declaration;
	}

	@SuppressWarnings("unchecked")
	private void replaceDirectAccessToEnclosing(ASTRewrite rewrite,
			ASTNode declaration, IVariableBinding[] bindings,
			String[] fieldsName) {
		final AST ast = fAnonymousInnerClassNode.getAST();
		final AbstractTypeDeclaration declarations = (AbstractTypeDeclaration) ASTNodes
				.getParent(fAnonymousInnerClassNode,
						AbstractTypeDeclaration.class);
		final IJavaProject javaProject = fCu.getJavaProject();
		final IJavaProject project = javaProject;

		final SearchForEnclosingAccess v = new SearchForEnclosingAccess(
				declarations, fContext, false);
		v.initTypeHierarchy(fAnonymousInnerClassNode.resolveBinding());
		declaration.accept(v);
		final List<ASTNode> nodes = v.getNodeToRewriteList();
		// List<ASTNode> nodes = l2; // fNodeToBeReplaced;
		for (final ASTNode node : nodes) {
			if (node.getNodeType() == ASTNode.SIMPLE_NAME) {
				final SimpleName name = (SimpleName) node;
				final IVariableBinding binding = (IVariableBinding) name
						.resolveBinding();

				Expression expr = null;
				if (Modifier.isStatic(binding.getModifiers())) {
					final int index = searchForBinding(binding, bindings);
					String fName = null;
					if (index > 0) {
						fName = fieldsName[index];
					} else {
						fName = declarations.getName().getFullyQualifiedName();
					}
					expr = ast.newSimpleName(fName);
				} else {
					expr = ast.newSimpleName(outerThis.getName()
							.getFullyQualifiedName());
				}
				final FieldAccess access = ast.newFieldAccess();
				access.setExpression(expr);
				final String fieldName = name.getFullyQualifiedName();
				access.setName(ast.newSimpleName(fieldName));
				rewrite.replace(node, access, null);
			} else if (node.getNodeType() == ASTNode.METHOD_INVOCATION) {
				final MethodInvocation method = (MethodInvocation) node;
				final MethodInvocation access = ast.newMethodInvocation();
				for (final Iterator iter = method.arguments().iterator(); iter
						.hasNext();) {
					final Expression org = (Expression) iter.next();
					Expression copy = null;
					if (org.getNodeType() == ASTNode.SIMPLE_NAME) {
						copy = replaceSimpleName((SimpleName) org, project,
								bindings, fieldsName);
					}
					if (copy == null) {
						copy = (Expression) rewrite.createCopyTarget(org);
					}
					access.arguments().add(copy);
				}
				access.setName((SimpleName) ASTNode.copySubtree(ast, method
						.getName()));
				if (Modifier.isStatic(method.resolveMethodBinding()
						.getModifiers())) {
					access.setExpression(ast.newName(method
							.resolveMethodBinding().getDeclaringClass()
							.getQualifiedName()));
				} else {
					access.setExpression(ast.newSimpleName(outerThis.getName()
							.getFullyQualifiedName()));
				}
				rewrite.replace(node, access, null);
			} else if (node.getNodeType() == ASTNode.THIS_EXPRESSION) {
				final ThisExpression field = (ThisExpression) node;
				// Name expr = field.getQualifier();
				// TODO : typically IlrRuleset.this
				// that we want to replace by outer
				rewrite.replace(field, ast.newSimpleName(outerThis.getName()
						.getFullyQualifiedName()), null);
			}
		}

	}

	private int searchForBinding(IVariableBinding binding,
			IVariableBinding[] bindings) {
		for (int i = 0; i < bindings.length; i++) {
			if (binding.isEqualTo(bindings[i])) {
				return i;
			}
		}
		return -1;
	}

	public Expression replaceSimpleName(SimpleName node, IJavaProject project,
			IVariableBinding[] bindings, String[] fieldsName) {
		final IBinding snBind = node.resolveBinding();
		final AST ast = node.getAST();
		if (snBind instanceof IVariableBinding) {
			final int index = searchForBinding((IVariableBinding) snBind,
					bindings);
			if (index != -1) {
				Expression newNode;
				/*
				 * String name = ifield.getName(); String fieldName = name;
				 * String oldName = name; if (!ifield.isEnumConstant()) { name =
				 * NamingConventions
				 * .removePrefixAndSuffixForLocalVariableName(project, name); if
				 * (name.equals(oldName)) name = NamingConventions
				 * .removePrefixAndSuffixForArgumentName(project, name);
				 * fieldName = NamingConventions.suggestFieldNames(project, "",
				 * name, 0, Flags.AccPrivate, new String[] {})[0]; //$NON-NLS-1$
				 * if (fSettings.useKeywordThis) { FieldAccess access =
				 * ast.newFieldAccess();
				 * access.setExpression(ast.newThisExpression());
				 * access.setName(ast.newSimpleName(fieldName)); newNode =
				 * access; } else newNode = ast.newSimpleName(fieldName); } else
				 */
				if (fSettings.useKeywordThis) {
					final FieldAccess access = ast.newFieldAccess();
					access.setExpression(ast.newThisExpression());
					access.setName(ast
							.newSimpleName(fieldsName[index] /* fieldName */));
					newNode = access;
				} else {
					newNode = ast
							.newSimpleName(fieldsName[index] /* fieldName */);
				}
				return newNode;
			}
		}
		return null;
	}

	//
	@SuppressWarnings("unchecked")
	class ReplaceEnclosingAccess extends ASTVisitor {

		private ITypeBinding anonType = null;

		private final ASTRewrite rewrite;

		public ReplaceEnclosingAccess(ITypeBinding declaration,
				IBinding[] bindings, ASTRewrite rewrite,
				AbstractTypeDeclaration declarations, IJavaProject project) {
			anonType = declaration;
			this.rewrite = rewrite;
		}

		@Override
		public void endVisit(MethodInvocation node) {
			final IMethodBinding imethod = node.resolveMethodBinding();
			final ITypeBinding binding = imethod.getDeclaringClass();
			if (!binding.getKey().equals(anonType.getKey())) {
				final AST ast = node.getAST();
				final MethodInvocation method = node;
				final MethodInvocation access = ast.newMethodInvocation();
				for (final Iterator iter = method.arguments().iterator(); iter
						.hasNext();) {
					final Expression copy = (Expression) rewrite
							.createCopyTarget((Expression) iter.next());
					access.arguments().add(copy);
				}
				access.setName((SimpleName) ASTNode.copySubtree(ast, method
						.getName()));
				access.setExpression(ast.newSimpleName(outerThis.getName()
						.getFullyQualifiedName()));
				rewrite.replace(node, access, null);
			}

		}

		@Override
		public void endVisit(FieldAccess node) {
			final IVariableBinding ifield = node.resolveFieldBinding();
			if (ifield != null) {
				final ITypeBinding binding = ifield.getDeclaringClass();
				if (binding != null && (binding.getKey() != anonType.getKey())) {

				}
			}
		}

	}

	//
	@SuppressWarnings("unchecked")
	private void createNewOuterThisField(CompilationUnitRewrite rewrite,
			AbstractTypeDeclaration declaration) {
		final AST ast = fAnonymousInnerClassNode.getAST();
		final AbstractTypeDeclaration declarations = (AbstractTypeDeclaration) ASTNodes
				.getParent(fAnonymousInnerClassNode,
						AbstractTypeDeclaration.class);
		final VariableDeclarationFragment fragment = ast
				.newVariableDeclarationFragment();
		final IJavaProject project = rewrite.getCu().getJavaProject();
		String name = getOuterThisFieldName();
		// "outerThis"; // + declarations.getName().getFullyQualifiedName();
		/*
		 * String name = NamingConventions
		 * .removePrefixAndSuffixForLocalVariableName(project, paramName); if
		 * (name.equals(paramName)) name =
		 * NamingConventions.removePrefixAndSuffixForArgumentName( project,
		 * paramName); String[] argSuggestion =
		 * NamingConventions.suggestArgumentNames(project, "", name, 0,
		 * excluded); //$NON-NLS-1$ name = argSuggestion[argSuggestion.length -
		 * 1];
		 */
		final String oldName = name;
		name = NamingConventions.removePrefixAndSuffixForLocalVariableName(
				project, name);
		if (name.equals(oldName)) {
			name = NamingConventions.removePrefixAndSuffixForArgumentName(
					project, name);
		}
		// String[] fieldsSuggestion = NamingConventions.suggestArgumentNames(
		// project, "", name, 0, /* Flags.AccPrivate, */new String[] {});
		// //$NON-NLS-1$
		//
		// name = "outer_" + fieldsSuggestion[fieldsSuggestion.length - 1];
		fragment.setName(ast.newSimpleName(name));
		fragment.setExtraDimensions(0);
		fragment.setInitializer(null);
		final FieldDeclaration field = ast.newFieldDeclaration(fragment);
		field.setType(ast.newSimpleType(ast.newSimpleName(declarations
				.getName().getFullyQualifiedName())));
		field.modifiers().addAll(
				ASTNodeFactory.newModifiers(ast, Modifier.PRIVATE
						| Modifier.FINAL));
		outerThis = fragment;
		declaration.bodyDeclarations()
				.add(
						ConvertAnonymousToInnerRefactoring
								.findIndexOfLastField(declaration
										.bodyDeclarations()) + 1, field);
	}

	@SuppressWarnings("unchecked")
	private ASTNode createBodyDeclaration(CompilationUnitRewrite rewriter,
			IDocument document, IVariableBinding[] bindings,
			BodyDeclaration body, String delimiter, String[] filedsName) {
		final ASTRewrite rewrite = ASTRewrite.create(rewriter.getAST());
		final ITrackedNodePosition position = rewrite.track(body);
		final IBinding[] binding = { null };
		final ASTNode[] newNode = { null };
		// List excludedFields = new ArrayList();
		final AST ast = fAnonymousInnerClassNode.getAST();
		final IJavaProject javaProject = fCu.getJavaProject();
		if (faccessEnclosing) {
			// if declaration if field declaration just remove
			if (body.getNodeType() == ASTNode.FIELD_DECLARATION) {
				final VariableDeclarationFragment fragment = TranslationUtils
						.getFrament(body);
				if (fragment.getInitializer() != null
						&& isToBeInitializerInConstructor(fragment)) {
					/*
					 * FieldDeclaration newFieldDecl = (FieldDeclaration)
					 * ASTNode.copySubtree(ast, fieldDecl);
					 * newFieldDecl.fragments().clear();
					 * VariableDeclarationFragment newfragment =
					 * (VariableDeclarationFragment) ASTNode.copySubtree(ast,
					 * fragment); newfragment.setInitializer(null);
					 * newFieldDecl.fragments().add(newfragment);
					 * rewrite.replace(fieldDecl, newFieldDecl, null);
					 */
					rewrite.remove(fragment.getInitializer(), null);
				}

			} else {
				replaceDirectAccessToEnclosing(rewrite, body, bindings,
						filedsName);
			}
		}
		if (body.getNodeType() == ASTNode.FIELD_DECLARATION) {
			final VariableDeclarationFragment fragment = TranslationUtils
					.getFrament(body);
			if (fragment.getInitializer() != null
					&& isToBeInitializerInConstructor(fragment)) {
				rewrite.remove(fragment.getInitializer(), null);
			}
		}
		for (int index = 0; index < bindings.length; index++) {
			binding[0] = bindings[index];
			// String name = binding[0].getName();
			// String fieldName = name;
			// String oldName = name;
			if (binding[0] instanceof IVariableBinding) {
				//final IVariableBinding variable = (IVariableBinding) binding[0];

				newNode[0] = ast
						.newSimpleName(filedsName[index] /* fieldName */);

			}
			body.accept(new ASTVisitor() {

				@Override
				public boolean visit(SimpleName node) {
					final IBinding resolved = node.resolveBinding();
					if (binding[0].equals(resolved)) {
						if (node.getParent().getNodeType() == ASTNode.FIELD_ACCESS) {
							final FieldAccess field = (FieldAccess) node
									.getParent();
							final Expression expr = field.getExpression();
							if (expr.getNodeType() == ASTNode.THIS_EXPRESSION) {
								// TODO : typically
								// IlrRuleset.this.ruletaskImplFactory
								// that we want to replace by
								// outer.ruletaskImplFactory
								rewrite.replace(field, ASTNode.copySubtree(ast,
										newNode[0]), null);
							} else {
								rewrite.replace(node, ASTNode.copySubtree(ast,
										newNode[0]), null);
							}
						} else {
							rewrite.replace(node, ASTNode.copySubtree(ast,
									newNode[0]), null);
						}
					}
					return false;
				}
			});

		}

		//
		final IDocument buffer = new Document(document.get());
		final Map options = javaProject.getOptions(true);
		final CodeGenerationSettings settings = /* new CodeGenerationSettings(); */JavaPreferencesSettings
				.getCodeGenerationSettings(javaProject);
		final TextEdit edit = rewrite.rewriteAST(buffer, options);
		try {
			edit.apply(buffer, TextEdit.UPDATE_REGIONS);
			final String trimmed = /* buffer.get(); */Strings.trimIndentation(
					buffer.get(position.getStartPosition(), position
							.getLength()), settings.tabWidth,
					settings.indentWidth, false);
			return rewriter.getASTRewrite().createStringPlaceholder(trimmed,
					ASTNode.METHOD_DECLARATION);
		} catch (final MalformedTreeException exception) {
			JavaPlugin.log(exception);
		} catch (final BadLocationException exception) {
			JavaPlugin.log(exception);
		}
		return null;
	}

	@SuppressWarnings("unchecked")
	private void removeInitializationFromDeclaredFields(
			CompilationUnitRewrite rewrite) {
		for (final Iterator iter = getFieldsToInitializeInConstructor()
				.iterator(); iter.hasNext();) {
			final VariableDeclarationFragment fragment = (VariableDeclarationFragment) iter
					.next();
			Assert.isNotNull(fragment.getInitializer());
			rewrite.getASTRewrite().remove(fragment.getInitializer(), null);
		}
	}

	@SuppressWarnings("unchecked")
	private void createFieldsForAccessedLocals(CompilationUnitRewrite rewrite,
			AbstractTypeDeclaration declaration, IVariableBinding[] bindings,
			String[] fieldsName) {
		// final IVariableBinding[] bindings = getUsedLocalVariables();
		// final IJavaProject project = rewrite.getCu().getJavaProject();
		// List excluded = new ArrayList();
		final AST ast = fAnonymousInnerClassNode.getAST();
		for (int index = 0; index < bindings.length; index++) {
			final VariableDeclarationFragment fragment = ast
					.newVariableDeclarationFragment();
			fragment.setExtraDimensions(0);
			fragment.setInitializer(null);
			/*
			 * String name = bindings[index].getName(); String oldName = name;
			 * name =
			 * NamingConventions.removePrefixAndSuffixForLocalVariableName(
			 * project, name); if (name.equals(oldName)) name =
			 * NamingConventions.removePrefixAndSuffixForArgumentName( project,
			 * name); name = NamingConventions .suggestFieldNames( project, "",
			 * name, 0, Flags.AccPrivate, (String[]) excluded.toArray(new
			 * String[excluded.size()]))[0]; //$NON-NLS-1$
			 */
			fragment.setName(ast.newSimpleName(fieldsName[index] /* name */));
			// excluded.add(name);
			final FieldDeclaration field = ast.newFieldDeclaration(fragment);
			field.setType(rewrite.getImportRewrite().addImport(
					bindings[index].getType(), ast));
			field.modifiers().addAll(
					ASTNodeFactory.newModifiers(ast, Modifier.PRIVATE
							| Modifier.FINAL));
			declaration.bodyDeclarations().add(
					ConvertAnonymousToInnerRefactoring
							.findIndexOfLastField(declaration
									.bodyDeclarations()) + 1, field);
			if (fSettings.createComments) {
				try {
					final String string = CodeGeneration.getFieldComment(
							rewrite.getCu(), bindings[index].getType()
									.getName(), fieldsName[index] /* name */,
							StubUtility.getLineDelimiterUsed(rewrite.getCu())); /* TODO: */
					if (string != null) {
						final Javadoc javadoc = (Javadoc) rewrite
								.getASTRewrite().createStringPlaceholder(
										string, ASTNode.JAVADOC);
						field.setJavadoc(javadoc);
					}
				} catch (final CoreException exception) {
					JavaPlugin.log(exception);
				}
			}
		}
	}

	@SuppressWarnings("unchecked")
	private IVariableBinding[] getUsedLocalVariables() {
		final List result = new ArrayList(); // was hashset
		/*
		 * if (faccessEnclosing) return new IVariableBinding[0];
		 */
		fAnonymousInnerClassNode.accept(createTempUsageFinder(result));
		return (IVariableBinding[]) result.toArray(new IVariableBinding[result
				.size()]);
	}

	@SuppressWarnings("unchecked")
	private ASTVisitor createTempUsageFinder(final List result) {
		return new ASTVisitor() {
			@Override
			public boolean visit(SimpleName node) {
				final IBinding binding = node.resolveBinding();
				if (ConvertAnonymousToInnerRefactoring.this
						.isBindingToTemp(binding)) {
					if (!result.contains(binding))
						result.add(binding);
				} else if (!faccessEnclosing
						&& ConvertAnonymousToInnerRefactoring.this
								.isBindingToEnclosing(binding)) {
					if (!result.contains(binding))
						result.add(binding);
				}
				return true;
			}
		};
	}

	@SuppressWarnings("unchecked")
	private ASTVisitor createEnclosingAccessFinder(final List result) {
		return new ASTVisitor() {
			@Override
			public boolean visit(SimpleName node) {
				final IBinding binding = node.resolveBinding();
				if (ConvertAnonymousToInnerRefactoring.this
						.isBindingToEnclosing(binding)) {
					if (!result.contains(binding))
						result.add(binding);
				}
				return true;
			}
		};
	}

	private boolean isBindingToEnclosing(IBinding binding) {
		if (!(binding instanceof IVariableBinding)) {
			return false;
		}
		final IVariableBinding variable = (IVariableBinding) binding;
		final ASTNode declaringNode = fCompilationUnitNode
				.findDeclaringNode(binding);
		if (declaringNode == null) {
			return false;
		}
		if (ASTNodes.isParent(declaringNode, fAnonymousInnerClassNode)) {
			return false;
		}
		// If the class that declare that field is the same as the one that
		// declare the anonymous
		if (variable.getDeclaringClass().isEqualTo(
				fAnonymousInnerClassNode.resolveBinding().getDeclaringClass())) {
			return !Modifier.isStatic(variable.getModifiers());
		}
		return false;
	}

	private boolean isBindingToTemp(IBinding binding) {
		if (!(binding instanceof IVariableBinding)) {
			return false;
		}
		final IVariableBinding variable = (IVariableBinding) binding;
		if (variable.isField()) {
			return false;
		}
		if (!Modifier.isFinal(binding.getModifiers())) {
			return false;
		}
		final ASTNode declaringNode = fCompilationUnitNode
				.findDeclaringNode(binding);
		if (declaringNode == null) {
			return false;
		}
		if (ASTNodes.isParent(declaringNode, fAnonymousInnerClassNode)) {
			return false;
		}
		return true;
	}

	//
	@SuppressWarnings("unchecked")
	private String[] computeFieldName(CompilationUnitRewrite rewrite,
			AbstractTypeDeclaration declaration, IVariableBinding[] bindings)
			throws JavaModelException {

		final IJavaProject project = fCu.getJavaProject();
		final String[] fieldsname = new String[bindings.length];

		final List excluded = new ArrayList();
		final List paramNames = new ArrayList();

		final List excludedFields = new ArrayList();
		final List excludedParams = new ArrayList();

		final AST ast = fAnonymousInnerClassNode.getAST();
		final MethodDeclaration newConstructor = ast.newMethodDeclaration();

		addParametersToNewConstructor(newConstructor, rewrite, paramNames,
				excludedParams);
		// int paramCount = newConstructor.parameters().size();

		addParametersForLocalsUsedInInnerClass(rewrite, bindings,
				newConstructor, paramNames, excluded);

		for (int index = 0; index < bindings.length; index++) {
			String name = bindings[index].getName();
			String fieldName = name;
			String paramName = name;
			final String oldName = name;
			name = NamingConventions.removePrefixAndSuffixForLocalVariableName(
					project, name);
			if (name.equals(oldName)) {
				name = NamingConventions.removePrefixAndSuffixForArgumentName(
						project, name);
			}
			final String fieldsSuggestion[] = NamingConventions
					.suggestFieldNames(
							project,
							"", name, 0, Flags.AccPrivate, (String[]) excludedFields.toArray(new String[excludedFields.size()])); //$NON-NLS-1$
			fieldName = fieldsSuggestion[fieldsSuggestion.length - 1];
			excludedFields.add(fieldName);
			final String paramsSuggestion[] = NamingConventions
					.suggestArgumentNames(
							project,
							"", name, 0, (String[]) excludedParams.toArray(new String[excludedParams.size()])); //$NON-NLS-1$
			paramName = paramsSuggestion[paramsSuggestion.length - 1];
			excludedParams.add(paramName);
			fieldsname[index] = "f_" + fieldName;

		}

		return fieldsname;
	}

	//

	@SuppressWarnings("unchecked")
	private void createNewConstructorIfNeeded(CompilationUnitRewrite rewrite,
			AbstractTypeDeclaration declaration, IVariableBinding[] bindings,
			String[] fieldsname) throws JavaModelException {

		// String[] fieldsname = new String[bindings.length];
		// IVariableBinding[] bindings = getUsedLocalVariables();

		/*
		 * if (((ClassInstanceCreation) fAnonymousInnerClassNode.getParent())
		 * .arguments().isEmpty() && bindings.length == 0) return;
		 */

		final AST ast = fAnonymousInnerClassNode.getAST();
		final MethodDeclaration newConstructor = ast.newMethodDeclaration();
		newConstructor.setConstructor(true);
		newConstructor.setExtraDimensions(0);
		newConstructor.setJavadoc(null);
		newConstructor.modifiers().addAll(
				ASTNodeFactory
						.newModifiers(ast, Modifier.PUBLIC /* fVisibility */));
		newConstructor.setName(ast.newSimpleName(fClassName));

		final List excluded = new ArrayList();
		final List paramNames = new ArrayList();

		// List excludedFields = new ArrayList();
		final List excludedParams = new ArrayList();

		final int nbParamInCtrst = addParametersToNewConstructor(
				newConstructor, rewrite, paramNames, excludedParams);
		final int paramCount = newConstructor.parameters().size();

		addParametersForLocalsUsedInInnerClass(rewrite, bindings,
				newConstructor, paramNames, excluded);

		if (faccessEnclosing) {
			final AbstractTypeDeclaration declarations = (AbstractTypeDeclaration) ASTNodes
					.getParent(fAnonymousInnerClassNode,
							AbstractTypeDeclaration.class);
			addParameterForOuterAccess(rewrite, newConstructor, declarations
					.resolveBinding(), paramNames);
		}

		final Block body = ast.newBlock();
		if (paramCount > 0) {
			final SuperConstructorInvocation superConstructorInvocation = ast
					.newSuperConstructorInvocation();
			for (int i = 0; i < paramCount; i++) {
				final SingleVariableDeclaration param = (SingleVariableDeclaration) newConstructor
						.parameters().get(i);
				superConstructorInvocation.arguments().add(
						ast.newSimpleName(param.getName().getIdentifier()));
			}
			body.statements().add(superConstructorInvocation);
		}
		// final IJavaProject project = fCu.getJavaProject();

		if (faccessEnclosing) {
			addOuterThisFieldInitialization(rewrite, body);
		}

		for (int index = 0; index < bindings.length; index++) {
			final String paramName = (String) paramNames.get(index
					+ nbParamInCtrst);
			final Assignment assignmentExpression = ast.newAssignment();
			assignmentExpression.setOperator(Assignment.Operator.ASSIGN);
			if (fSettings.useKeywordThis || fieldsname[index].equals(paramName)) {
				final FieldAccess access = ast.newFieldAccess();
				access.setExpression(ast.newThisExpression());
				access.setName(ast
						.newSimpleName(fieldsname[index] /* fieldName */));
				assignmentExpression.setLeftHandSide(access);
			} else {
				assignmentExpression.setLeftHandSide(ast
						.newSimpleName(fieldsname[index]));
			}
			assignmentExpression.setRightHandSide(ast.newSimpleName(paramName));
			final ExpressionStatement assignmentStatement = ast
					.newExpressionStatement(assignmentExpression);
			body.statements().add(assignmentStatement);
		}

		addFieldInitialization(rewrite, body, excluded, bindings, fieldsname);

		newConstructor.setBody(body);

		addExceptionsToNewConstructor(newConstructor);
		/*
		 * declaration.bodyDeclarations().add( 1 + bindings.length +
		 * findIndexOfLastField(fAnonymousInnerClassNode .bodyDeclarations()),
		 * newConstructor);
		 */
		declaration.bodyDeclarations().add(newConstructor);
		if (fSettings.createComments) {
			try {
				final String string = /* null; */CodeGeneration
						.getMethodComment(
								rewrite.getCu(),
								fClassName,
								fClassName,
								(String[]) paramNames
										.toArray(new String[paramNames.size()]),
								new String[0], null, new String[0], null,
								StubUtility.getLineDelimiterUsed(rewrite
										.getCu()));
				if (string != null) {
					final Javadoc javadoc = (Javadoc) rewrite.getASTRewrite()
							.createStringPlaceholder(string, ASTNode.JAVADOC);
					newConstructor.setJavadoc(javadoc);
				}
			} catch (final CoreException exception) {
				JavaPlugin.log(exception);
			}
		}

		// return fieldsname;
	}

	@SuppressWarnings("unchecked")
	private void addFieldInitialization(CompilationUnitRewrite rewrite,
			Block constructorBody, List excluded, IVariableBinding[] bindings,
			String[] fieldsname) {
		// final IJavaProject project = rewrite.getCu().getJavaProject();
		// List excluded = new ArrayList();
		final AST ast = fAnonymousInnerClassNode.getAST();
		for (final Iterator iter = getFieldsToInitializeInConstructor()
				.iterator(); iter.hasNext();) {
			final VariableDeclarationFragment fragment = (VariableDeclarationFragment) iter
					.next();
			final Assignment assignmentExpression = ast.newAssignment();
			assignmentExpression.setOperator(Assignment.Operator.ASSIGN);
			final String name = fragment.getName().getIdentifier();
			/*
			 * name =
			 * NamingConventions.removePrefixAndSuffixForLocalVariableName(
			 * project, name); if (name.equals(oldName)) name =
			 * NamingConventions.removePrefixAndSuffixForArgumentName( project,
			 * name); name = NamingConventions .suggestFieldNames( project, "",
			 * name, 0, Flags.AccPrivate, (String[]) excluded.toArray(new
			 * String[excluded.size()]))[0]; //$NON-NLS-1$
			 */
			if (fSettings.useKeywordThis) {
				final FieldAccess access = ast.newFieldAccess();
				access.setExpression(ast.newThisExpression());
				access.setName(ast.newSimpleName(name));
				assignmentExpression.setLeftHandSide(access);
			} else {
				assignmentExpression.setLeftHandSide(ast.newSimpleName(name));
			}
			excluded.add(name);
			// We need to check the content of that initializer !
			replaceDirectAccessToEnclosing(rewrite.getASTRewrite(), fragment
					.getInitializer(), null, null);
			final Expression rhs = (Expression) rewrite.getASTRewrite()
					.createCopyTarget(fragment.getInitializer());
			//
			assignmentExpression.setRightHandSide(rhs);
			final ExpressionStatement assignmentStatement = ast
					.newExpressionStatement(assignmentExpression);
			constructorBody.statements().add(assignmentStatement);
		}
	}

	@SuppressWarnings("unchecked")
	private void addOuterThisFieldInitialization(
			CompilationUnitRewrite rewrite, Block constructorBody) {
		final IJavaProject project = rewrite.getCu().getJavaProject();
		final AST ast = fAnonymousInnerClassNode.getAST();

		final Assignment assignmentExpression = ast.newAssignment();
		assignmentExpression.setOperator(Assignment.Operator.ASSIGN);
		String name = getOuterThisFieldName(); // fragment.getName().getIdentifier();
		final String oldName = name;
		name = NamingConventions.removePrefixAndSuffixForLocalVariableName(
				project, name);
		if (name.equals(oldName)) {
			name = NamingConventions.removePrefixAndSuffixForArgumentName(
					project, name);
		}
		final String[] fieldsSuggestion = NamingConventions.suggestFieldNames(
				project, "", name, 0, Flags.AccPrivate, new String[] {}); //$NON-NLS-1$
		// name = "outer_" + fieldsSuggestion[fieldsSuggestion.length - 1];
		final FieldAccess thisAccess = ast.newFieldAccess();
		thisAccess.setExpression(ast.newThisExpression());
		thisAccess.setName(ast.newSimpleName(name));
		assignmentExpression.setLeftHandSide(thisAccess);

		assignmentExpression.setRightHandSide(ast
				.newSimpleName(fieldsSuggestion[fieldsSuggestion.length - 1]));

		final ExpressionStatement assignmentStatement = ast
				.newExpressionStatement(assignmentExpression);
		constructorBody.statements().add(assignmentStatement);

	}

	// live List of VariableDeclarationFragments
	@SuppressWarnings("unchecked")
	private List getFieldsToInitializeInConstructor() {
		final List result = new ArrayList(0);
		for (final Iterator iter = fAnonymousInnerClassNode.bodyDeclarations()
				.iterator(); iter.hasNext();) {
			final BodyDeclaration element = (BodyDeclaration) iter.next();
			if (!(element instanceof FieldDeclaration)) {
				continue;
			}
			final FieldDeclaration field = (FieldDeclaration) element;
			for (final Iterator fragmentIter = field.fragments().iterator(); fragmentIter
					.hasNext();) {
				final VariableDeclarationFragment fragment = (VariableDeclarationFragment) fragmentIter
						.next();
				if (isToBeInitializerInConstructor(fragment)) {
					result.add(fragment);
				}
			}
		}
		return result;
	}

	private boolean isToBeInitializerInConstructor(
			VariableDeclarationFragment fragment) {
		if (fragment.getInitializer() == null) {
			return false;
		}
		return areLocalsUsedIn(fragment.getInitializer())
				|| areBindingToEnclosing(fragment.getInitializer());
	}

	@SuppressWarnings("unchecked")
	private boolean areLocalsUsedIn(Expression fieldInitializer) {
		final List localsUsed = new ArrayList();
		fieldInitializer.accept(createTempUsageFinder(localsUsed));
		return !localsUsed.isEmpty();
	}

	@SuppressWarnings("unchecked")
	private boolean areBindingToEnclosing(Expression fieldInitializer) {
		final List localsUsed = new ArrayList();
		fieldInitializer.accept(createEnclosingAccessFinder(localsUsed));
		return !localsUsed.isEmpty();
	}

	@SuppressWarnings("unchecked")
	private void addParametersForLocalsUsedInInnerClass(
			CompilationUnitRewrite rewrite, IVariableBinding[] usedLocals,
			MethodDeclaration newConstructor, List params, List excluded) {
		for (final IVariableBinding element : usedLocals) {
			/*
			 * SingleVariableDeclaration declaration =
			 * this.createNewParamDeclarationNode( element.getName(),
			 * element.getType(), rewrite, (String[]) params.toArray(new
			 * String[params.size()]));
			 */
			final SingleVariableDeclaration declaration = newConstructor
					.getAST().newSingleVariableDeclaration();
			declaration.setName(newConstructor.getAST().newSimpleName(
					element.getName()));
			declaration.setType(rewrite.getImportRewrite().addImport(
					element.getType(), newConstructor.getAST()));
			newConstructor.parameters().add(declaration);
			final String identifier = declaration.getName().getIdentifier();
			excluded.add(identifier);
			params.add(identifier);
		}
	}

	@SuppressWarnings("unchecked")
	private void addParameterForOuterAccess(CompilationUnitRewrite rewrite,
			MethodDeclaration newConstructor, ITypeBinding enclosing,
			List params) {
		final List excluded = new ArrayList();
		final SingleVariableDeclaration declaration = createNewParamDeclarationNode(
				getOuterThisFieldName() /* "outerThis" */, enclosing, rewrite,
				(String[]) excluded.toArray(new String[excluded.size()]));
		newConstructor.parameters().add(declaration);
		final String identifier = declaration.getName().getIdentifier();
		excluded.add(identifier);
		params.add(identifier);
	}

	private String fOuterThisFieldName = null;

	private String getOuterThisFieldName() {
		if (fOuterThisFieldName != null) {
			return fOuterThisFieldName;
		}
		final AbstractTypeDeclaration declarations = (AbstractTypeDeclaration) ASTNodes
				.getParent(fAnonymousInnerClassNode,
						AbstractTypeDeclaration.class);

		fOuterThisFieldName = "outer_"
				+ declarations.getName().getFullyQualifiedName();
		/*
		 * String oldName = name; name =
		 * NamingConventions.removePrefixAndSuffixForLocalVariableName(
		 * fCu.getJavaProject(), name); if (name.equals(oldName)) name =
		 * NamingConventions.removePrefixAndSuffixForArgumentName(
		 * fCu.getJavaProject(), name); String[] fieldsSuggestion =
		 * NamingConventions.suggestFieldNames(fCu.getJavaProject(), "", name,
		 * 0, Flags.AccPrivate, new String[] {}); //$NON-NLS-1$
		 * fOuterThisFieldName = fieldsSuggestion[fieldsSuggestion.length - 1];
		 */
		return fOuterThisFieldName;
	}

	private IMethodBinding getSuperConstructorBinding() {
		// workaround for missing java core functionality - finding a
		// super constructor for an anonymous class creation
		if (fAnonymousInnerClassNode.getParent() instanceof ClassInstanceCreation) {
			final IMethodBinding anonConstr = ((ClassInstanceCreation) fAnonymousInnerClassNode
					.getParent()).resolveConstructorBinding();
			if (anonConstr == null) {
				return null;
			}
			final ITypeBinding superClass = anonConstr.getDeclaringClass()
					.getSuperclass();
			final IMethodBinding[] superMethods = superClass
					.getDeclaredMethods();
			for (final IMethodBinding superMethod : superMethods) {
				if (superMethod.isConstructor()
						&& ConvertAnonymousToInnerRefactoring
								.parameterTypesMatch(superMethod, anonConstr)) {
					return superMethod;
				}
			}
			Assert.isTrue(false);// there's no way - it must be there
		} else {
			System.err.println("ConvertAnonymous .getSuperConstructorBinding "
					+ fAnonymousInnerClassNode.getParent());
		}
		return null;
	}

	private static boolean parameterTypesMatch(IMethodBinding m1,
			IMethodBinding m2) {
		final ITypeBinding[] m1Params = m1.getParameterTypes();
		final ITypeBinding[] m2Params = m2.getParameterTypes();
		if (m1Params.length != m2Params.length) {
			return false;
		}
		for (int i = 0; i < m2Params.length; i++) {
			if (!m1Params[i].equals(m2Params[i])) {
				return false;
			}
		}
		return true;
	}

	@SuppressWarnings("unchecked")
	private void addExceptionsToNewConstructor(MethodDeclaration newConstructor) {
		final IMethodBinding constructorBinding = getSuperConstructorBinding();
		if (constructorBinding == null) {
			return;
		}
		final ITypeBinding[] exceptions = constructorBinding
				.getExceptionTypes();
		for (final ITypeBinding element : exceptions) {
			final Name exceptionTypeName = fAnonymousInnerClassNode.getAST()
					.newName(Bindings.getNameComponents(element));
			newConstructor.thrownExceptions().add(exceptionTypeName);
		}
	}

	@SuppressWarnings("unchecked")
	private int addParametersToNewConstructor(MethodDeclaration newConstructor,
			CompilationUnitRewrite rewrite, List params, List excluded)
			throws JavaModelException {
		final IMethodBinding constructorBinding = getSuperConstructorBinding();
		if (constructorBinding == null) {
			return 0;
		}
		final ITypeBinding[] paramTypes = constructorBinding
				.getParameterTypes();
		final IMethod method = (IMethod) constructorBinding.getJavaElement();

		if (method == null) {
			return 0;
		}
		if (method.getDeclaringType().isMember()) {
			// In case of inner class as parent
			for (int index = 0; index < paramTypes.length; index++) {
				final SingleVariableDeclaration declaration = createNewParamDeclarationNode(
						"arg" + index, paramTypes[index], rewrite,
						(String[]) excluded
								.toArray(new String[excluded.size()]));
				newConstructor.parameters().add(declaration);
				final String identifier = declaration.getName().getIdentifier();
				excluded.add(identifier);
				params.add(identifier);
			}
		} else {
			final String[] parameterNames = method.getParameterNames();
			for (int index = 0; index < paramTypes.length; index++) {
				final SingleVariableDeclaration declaration = createNewParamDeclarationNode(
						parameterNames[index], paramTypes[index], rewrite,
						(String[]) excluded
								.toArray(new String[excluded.size()]));
				newConstructor.parameters().add(declaration);
				final String identifier = declaration.getName().getIdentifier();
				excluded.add(identifier);
				params.add(identifier);
			}
		}
		return (paramTypes == null) ? 0 : paramTypes.length;
	}

	private SingleVariableDeclaration createNewParamDeclarationNode(
			String paramName, ITypeBinding paramType,
			CompilationUnitRewrite rewrite, String[] excluded) {
		final SingleVariableDeclaration param = fAnonymousInnerClassNode
				.getAST().newSingleVariableDeclaration();
		param.setExtraDimensions(0);
		param.setInitializer(null);
		final IJavaProject project = rewrite.getCu().getJavaProject();
		String name = NamingConventions
				.removePrefixAndSuffixForLocalVariableName(project, paramName);
		if (name.equals(paramName)) {
			name = NamingConventions.removePrefixAndSuffixForArgumentName(
					project, paramName);
		}
		final String[] argSuggestion = NamingConventions.suggestArgumentNames(
				project, "", name, 0, excluded); //$NON-NLS-1$
		name = argSuggestion[argSuggestion.length - 1];
		param.setName(fAnonymousInnerClassNode.getAST().newSimpleName(name));
		final ITypeBinding paramtypeDecl = paramType.getDeclaringClass();
		if (paramType.isTopLevel() || paramtypeDecl == null) {
			param.setType(rewrite.getImportRewrite().addImport(paramType,
					fAnonymousInnerClassNode.getAST()));
		} else {
			final String typename = paramtypeDecl.getQualifiedName() + "."
					+ paramType.getName();
			// rewrite.getImportRewrite().addImport(paramType,
			// fAnonymousInnerClassNode.getAST());
			final Type t = rewrite.getImportRewrite().addImport(paramType,
					fAnonymousInnerClassNode.getAST());
			//
			final ImportDeclaration id = fAnonymousInnerClassNode.getAST()
					.newImportDeclaration();
			id.setName(fAnonymousInnerClassNode.getAST().newName(typename));
			rewrite.getASTRewrite().getListRewrite(fCompilationUnitNode,
					CompilationUnit.IMPORTS_PROPERTY).insertLast(id, null);
			//
			param.setType(t);
		}
		return param;
	}

	@SuppressWarnings("unchecked")
	private Type setSuperType(TypeDeclaration declaration)
			throws JavaModelException {
		final ClassInstanceCreation classInstanceCreation = (ClassInstanceCreation) fAnonymousInnerClassNode
				.getParent();
		final ITypeBinding binding = classInstanceCreation.resolveTypeBinding();
		if (binding == null) {
			return null;
		}
		final Type newType = (Type) ASTNode.copySubtree(
				fAnonymousInnerClassNode.getAST(), classInstanceCreation
						.getType());
		if (binding.getSuperclass().getQualifiedName().equals(
				"java.lang.Object")) { //$NON-NLS-1$
			Assert.isTrue(binding.getInterfaces().length <= 1);
			if (binding.getInterfaces().length == 0) {
				return null;
			}
			declaration.superInterfaceTypes().add(0, newType);
		} else {
			declaration.setSuperclassType(newType);
		}
		return newType;
	}

	private ITypeBinding getSuperTypeBinding() {
		final ITypeBinding types = fAnonymousInnerClassNode.resolveBinding();
		final ITypeBinding[] interfaces = types.getInterfaces();
		if (interfaces.length > 0) {
			return interfaces[0];
		} else {
			return types.getSuperclass();
		}
	}

	private int createModifiersForNestedClass() {
		int flags = fVisibility;
		if (fDeclareFinal) {
			flags |= Modifier.FINAL;
		}
		if (mustInnerClassBeStatic() || fDeclareStatic) {
			flags |= Modifier.STATIC;
		}
		return flags;
	}

	public boolean mustInnerClassBeStatic() {
		final ITypeBinding typeBinding = ((AbstractTypeDeclaration) ASTNodes
				.getParent(fAnonymousInnerClassNode,
						AbstractTypeDeclaration.class)).resolveBinding();
		ASTNode current = fAnonymousInnerClassNode.getParent();
		boolean ans = false;
		while (current != null) {
			switch (current.getNodeType()) {
			case ASTNode.ANONYMOUS_CLASS_DECLARATION: {
				final AnonymousClassDeclaration enclosingAnonymousClassDeclaration = (AnonymousClassDeclaration) current;
				final ITypeBinding binding = enclosingAnonymousClassDeclaration
						.resolveBinding();
				if (binding != null
						&& Bindings.isSuperType(typeBinding, binding
								.getSuperclass())) {
					return false;
				}
				break;
			}
			case ASTNode.FIELD_DECLARATION: {
				final FieldDeclaration enclosingFieldDeclaration = (FieldDeclaration) current;
				if (Modifier.isStatic(enclosingFieldDeclaration.getModifiers())) {
					ans = true;
				}
				break;
			}
			case ASTNode.METHOD_DECLARATION: {
				final MethodDeclaration enclosingMethodDeclaration = (MethodDeclaration) current;
				if (Modifier
						.isStatic(enclosingMethodDeclaration.getModifiers())) {
					ans = true;
				}
				break;
			}
			case ASTNode.INITIALIZER: {
				final Initializer enclosingMethodDeclaration = (Initializer) current;
				if (Modifier
						.isStatic(enclosingMethodDeclaration.getModifiers())) {
					ans = true;
				}
				break;
			}
			case ASTNode.TYPE_DECLARATION: {
				return ans;
			}
			}
			current = current.getParent();
		}
		return ans;
	}

	@SuppressWarnings("unchecked")
	private static int findIndexOfLastField(List bodyDeclarations) {
		for (int i = bodyDeclarations.size() - 1; i >= 0; i--) {
			final BodyDeclaration each = (BodyDeclaration) bodyDeclarations
					.get(i);
			if (each instanceof FieldDeclaration) {
				return i;
			}
		}
		return -1;
	}

	public RefactoringStatus initialize(final JavaRefactoringArguments arguments) {
		fSelfInitializing = true;
		if (arguments instanceof JavaRefactoringArguments) {
			final JavaRefactoringArguments extended = arguments;
			final String handle = extended
					.getAttribute(JavaRefactoringDescriptorUtil.ATTRIBUTE_INPUT);
			if (handle != null) {
				final IJavaElement element = JavaRefactoringDescriptorUtil
						.handleToElement(extended.getProject(), handle, false);
				if (element == null
						|| !element.exists()
						|| element.getElementType() != IJavaElement.COMPILATION_UNIT) {
					return JavaRefactoringDescriptorUtil.createInputFatalStatus(element, getName(), ConvertAnonymousToInnerRefactoring.ID_CONVERT_ANONYMOUS);
				} else {
					fCu = (ICompilationUnit) element;
					fSettings = /* new CodeGenerationSettings(); */JavaPreferencesSettings
							.getCodeGenerationSettings(fCu.getJavaProject());
				}
			} else {
				return RefactoringStatus
						.createFatalErrorStatus(Messages
								.format(
										RefactoringCoreMessages.InitializableRefactoring_argument_not_exist,
										JavaRefactoringDescriptorUtil.ATTRIBUTE_INPUT));
			}
			final String name = extended
					.getAttribute(JavaRefactoringDescriptorUtil.ATTRIBUTE_NAME);
			if (name != null && !"".equals(name)) {
				fClassName = name;
			} else {
				return RefactoringStatus
						.createFatalErrorStatus(Messages
								.format(
										RefactoringCoreMessages.InitializableRefactoring_argument_not_exist,
										JavaRefactoringDescriptorUtil.ATTRIBUTE_NAME));
			}
			final String visibility = extended
					.getAttribute(ConvertAnonymousToInnerRefactoring.ATTRIBUTE_VISIBILITY);
			if (visibility != null && !"".equals(visibility)) {//$NON-NLS-1$
				int flag = 0;
				try {
					flag = Integer.parseInt(visibility);
				} catch (final NumberFormatException exception) {
					return RefactoringStatus
							.createFatalErrorStatus(Messages
									.format(
											RefactoringCoreMessages.InitializableRefactoring_argument_not_exist,
											ConvertAnonymousToInnerRefactoring.ATTRIBUTE_VISIBILITY));
				}
				fVisibility = flag;
			}
			final String selection = extended
					.getAttribute(JavaRefactoringDescriptorUtil.ATTRIBUTE_SELECTION);
			if (selection != null) {
				int offset = -1;
				int length = -1;
				final StringTokenizer tokenizer = new StringTokenizer(selection);
				if (tokenizer.hasMoreTokens()) {
					offset = Integer.valueOf(tokenizer.nextToken()).intValue();
				}
				if (tokenizer.hasMoreTokens()) {
					length = Integer.valueOf(tokenizer.nextToken()).intValue();
				}
				if (offset >= 0 && length >= 0) {
					fSelectionStart = offset;
					fSelectionLength = length;
				} else {
					return RefactoringStatus
							.createFatalErrorStatus(Messages
									.format(
											RefactoringCoreMessages.InitializableRefactoring_illegal_argument,
											new Object[] {
													selection,
													JavaRefactoringDescriptorUtil.ATTRIBUTE_SELECTION }));
				}
			} else {
				return RefactoringStatus
						.createFatalErrorStatus(Messages
								.format(
										RefactoringCoreMessages.InitializableRefactoring_argument_not_exist,
										JavaRefactoringDescriptorUtil.ATTRIBUTE_SELECTION));
			}
			final String declareStatic = extended
					.getAttribute(ConvertAnonymousToInnerRefactoring.ATTRIBUTE_STATIC);
			if (declareStatic != null) {
				fDeclareStatic = Boolean.valueOf(declareStatic).booleanValue();
			} else {
				return RefactoringStatus
						.createFatalErrorStatus(Messages
								.format(
										RefactoringCoreMessages.InitializableRefactoring_argument_not_exist,
										ConvertAnonymousToInnerRefactoring.ATTRIBUTE_STATIC));
			}
			final String declareFinal = extended
					.getAttribute(ConvertAnonymousToInnerRefactoring.ATTRIBUTE_FINAL);
			if (declareFinal != null) {
				fDeclareFinal = Boolean.valueOf(declareStatic).booleanValue();
			} else {
				return RefactoringStatus
						.createFatalErrorStatus(Messages
								.format(
										RefactoringCoreMessages.InitializableRefactoring_argument_not_exist,
										ConvertAnonymousToInnerRefactoring.ATTRIBUTE_FINAL));
			}
		} else {
			return RefactoringStatus
					.createFatalErrorStatus(RefactoringCoreMessages.InitializableRefactoring_inacceptable_arguments);
		}
		return new RefactoringStatus();
	}

	public void setCreatedTypeName(String fCreatedTypeName) {
		this.fCreatedTypeName = fCreatedTypeName;
	}

	public String getCreatedTypeName() {
		return fCreatedTypeName;
	}
}
