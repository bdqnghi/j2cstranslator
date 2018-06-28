package com.ilog.translator.java2cs.translation.astrewriter;

/*******************************************************************************
 * Copyright (c) 2000, 2006 IBM Corporation and others.
 * All rights reserved. This program and the accompanying materials
 * are made available under the terms of the Eclipse Public License v1.0
 * which accompanies this distribution, and is available at
 * http://www.eclipse.org/legal/epl-v10.html
 *
 * Contributors:
 *     IBM Corporation - initial API and implementation
 *******************************************************************************/

import java.util.ArrayList;
import java.util.List;
import java.util.StringTokenizer;

import org.eclipse.core.resources.IFile;
import org.eclipse.core.runtime.Assert;
import org.eclipse.core.runtime.CoreException;
import org.eclipse.core.runtime.IProgressMonitor;
import org.eclipse.core.runtime.OperationCanceledException;
import org.eclipse.jdt.core.ICompilationUnit;
import org.eclipse.jdt.core.IJavaElement;
import org.eclipse.jdt.core.ILocalVariable;
import org.eclipse.jdt.core.IMethod;
import org.eclipse.jdt.core.ISourceRange;
import org.eclipse.jdt.core.JavaModelException;
import org.eclipse.jdt.core.dom.ASTNode;
import org.eclipse.jdt.core.dom.CompilationUnit;
import org.eclipse.jdt.core.dom.Initializer;
import org.eclipse.jdt.core.dom.MethodDeclaration;
import org.eclipse.jdt.core.dom.VariableDeclaration;
import org.eclipse.jdt.core.refactoring.descriptors.RenameLocalVariableDescriptor;
import org.eclipse.jdt.internal.corext.dom.NodeFinder;
import org.eclipse.jdt.internal.corext.refactoring.Checks;
import org.eclipse.jdt.internal.corext.refactoring.JavaRefactoringArguments;
import org.eclipse.jdt.internal.corext.refactoring.JavaRefactoringDescriptorUtil;
//import org.eclipse.jdt.internal.corext.refactoring.JavaRefactoringDescriptorComment;
import org.eclipse.jdt.internal.corext.refactoring.RefactoringAvailabilityTester;
import org.eclipse.jdt.internal.corext.refactoring.RefactoringCoreMessages;
import org.eclipse.jdt.internal.corext.refactoring.changes.CompilationUnitChange;
import org.eclipse.jdt.internal.corext.refactoring.changes.TextChangeCompatibility;
import org.eclipse.jdt.internal.corext.refactoring.participants.JavaProcessors;
import org.eclipse.jdt.internal.corext.refactoring.rename.JavaRenameProcessor;
import org.eclipse.jdt.internal.corext.refactoring.rename.RenameModifications;
import org.eclipse.jdt.internal.corext.refactoring.rename.TempOccurrenceAnalyzer;
import org.eclipse.jdt.internal.corext.refactoring.tagging.INameUpdating;
import org.eclipse.jdt.internal.corext.refactoring.tagging.IReferenceUpdating;
import org.eclipse.jdt.internal.corext.refactoring.util.RefactoringASTParser;
import org.eclipse.jdt.internal.corext.refactoring.util.ResourceUtil;
import org.eclipse.jdt.internal.corext.refactoring.util.TextChangeManager;
import org.eclipse.jdt.internal.corext.util.Messages;
import org.eclipse.jdt.internal.ui.JavaPlugin;
import org.eclipse.ltk.core.refactoring.Change;
import org.eclipse.ltk.core.refactoring.GroupCategorySet;
import org.eclipse.ltk.core.refactoring.RefactoringStatus;
import org.eclipse.ltk.core.refactoring.TextChange;
import org.eclipse.ltk.core.refactoring.participants.CheckConditionsContext;
import org.eclipse.ltk.core.refactoring.participants.RenameArguments;
import org.eclipse.ltk.internal.core.refactoring.history.RefactoringDescriptorChange;
import org.eclipse.text.edits.MultiTextEdit;
import org.eclipse.text.edits.ReplaceEdit;
import org.eclipse.text.edits.TextEdit;
import org.eclipse.text.edits.TextEditGroup;

import com.ilog.translator.java2cs.translation.astrewriter.RenameAnalyzeUtil2.LocalAnalyzePackage;

@SuppressWarnings({ "deprecation", "restriction" })
public class RenameLocalVariablesProcessor extends JavaRenameProcessor
		implements INameUpdating, IReferenceUpdating {

	private static final String ID_RENAME_LOCAL_VARIABLE = "org.eclipse.jdt.ui.rename.local.variable"; //$NON-NLS-1$

	private static final String ATTRIBUTE_REFERENCES = "references"; //$NON-NLS-1$

	private final List<ILocalVariable> fLocalVariables;

	private ICompilationUnit fCu;

	// the following fields are set or modified after the construction
	private boolean fUpdateReferences;

	private final List<String> fCurrentNames = new ArrayList<String>();

	private List<String> fNewNames;

	private CompilationUnit fCompilationUnitNode;

	private List<VariableDeclaration> fTempDeclarationNodes = new ArrayList<VariableDeclaration>();

	private TextChange fChange;

	private boolean fIsComposite;

	private GroupCategorySet fCategorySet;

	private TextChangeManager fChangeManager;

	private final List<RenameAnalyzeUtil2.LocalAnalyzePackage> fLocalAnalyzePackage = new ArrayList<LocalAnalyzePackage>();

	public static final String IDENTIFIER = "org.eclipse.jdt.ui.renameLocalVariableProcessor"; //$NON-NLS-1$

	/**
	 * Creates a new rename local variable processor.
	 * 
	 * @param localVariable
	 *            the local variable, or <code>null</code> if invoked by
	 *            scripting
	 */
	public RenameLocalVariablesProcessor(List<ILocalVariable> localVariables) {
		fLocalVariables = localVariables;
		fUpdateReferences = true;
		if (localVariables != null)
			fCu = (ICompilationUnit) localVariables.get(0).getAncestor(
					IJavaElement.COMPILATION_UNIT);
		fNewNames = new ArrayList<String>();
		fIsComposite = false;
	}

	/**
	 * Creates a new rename local variable processor.
	 * <p>
	 * This constructor is only used by <code>RenameTypeProcessor</code>.
	 * </p>
	 * 
	 * @param localVariable
	 *            the local variable
	 * @param manager
	 *            the change manager
	 * @param node
	 *            the compilation unit node
	 * @param categorySet
	 *            the group category set
	 */
	RenameLocalVariablesProcessor(List<ILocalVariable> localVariables,
			TextChangeManager manager, CompilationUnit node,
			GroupCategorySet categorySet) {
		this(localVariables);
		fChangeManager = manager;
		fCategorySet = categorySet;
		fCompilationUnitNode = node;
		fIsComposite = true;
	}

	/*
	 * @see org.eclipse.jdt.internal.corext.refactoring.rename.JavaRenameProcessor#needsSavedEditors()
	 */
	public boolean needsSavedEditors() {
		return false;
	}

	/*
	 * @see org.eclipse.jdt.internal.corext.refactoring.rename.JavaRenameProcessor#getAffectedProjectNatures()
	 */
	@Override
	protected final String[] getAffectedProjectNatures() throws CoreException {
		return JavaProcessors.computeAffectedNaturs(fLocalVariables
				.toArray(new IJavaElement[0]));
	}

	/*
	 * @see org.eclipse.ltk.core.refactoring.participants.RefactoringProcessor#getElements()
	 */
	@Override
	public Object[] getElements() {
		return new Object[] { fLocalVariables };
	}

	/*
	 * @see org.eclipse.ltk.core.refactoring.participants.RefactoringProcessor#getIdentifier()
	 */
	@Override
	public String getIdentifier() {
		return IDENTIFIER;
	}

	/*
	 * @see org.eclipse.ltk.core.refactoring.participants.RefactoringProcessor#getProcessorName()
	 */
	@Override
	public String getProcessorName() {
		return RefactoringCoreMessages.RenameTempRefactoring_rename;
	}

	/*
	 * @see org.eclipse.ltk.core.refactoring.participants.RefactoringProcessor#isApplicable()
	 */
	@Override
	public boolean isApplicable() throws CoreException {
		for (final ILocalVariable local : fLocalVariables) {
			if (!RefactoringAvailabilityTester.isRenameAvailable(local))
				return false;
		}
		return true;
	}

	/*
	 * @see org.eclipse.jdt.internal.corext.refactoring.tagging.IReferenceUpdating#canEnableUpdateReferences()
	 */
	public boolean canEnableUpdateReferences() {
		return true;
	}

	/*
	 * @see org.eclipse.jdt.internal.corext.refactoring.rename.JavaRenameProcessor#getUpdateReferences()
	 */
	public boolean getUpdateReferences() {
		return fUpdateReferences;
	}

	/*
	 * @see org.eclipse.jdt.internal.corext.refactoring.tagging.IReferenceUpdating#setUpdateReferences(boolean)
	 */
	public void setUpdateReferences(boolean updateReferences) {
		fUpdateReferences = updateReferences;
	}

	/*
	 * @see org.eclipse.jdt.internal.corext.refactoring.tagging.INameUpdating#getCurrentElementName()
	 */
	public List<String> getCurrentElementNames() {
		return fCurrentNames;
	}

	/*
	 * @see org.eclipse.jdt.internal.corext.refactoring.tagging.INameUpdating#getNewElementName()
	 */
	public List<String> getNewElementNames() {
		return fNewNames;
	}

	/*
	 * @see org.eclipse.jdt.internal.corext.refactoring.tagging.INameUpdating#setNewElementName(java.lang.String)
	 */
	public void setNewElementNames(List<String> newNames) {
		Assert.isNotNull(newNames);
		fNewNames = newNames;
	}

	/*
	 * @see org.eclipse.jdt.internal.corext.refactoring.tagging.INameUpdating#getNewElement()
	 */
	public Object getNewElement() {
		return null; // cannot create an ILocalVariable
	}

	@Override
	public RefactoringStatus checkInitialConditions(IProgressMonitor pm)
			throws CoreException {
		initAST();
		for (final VariableDeclaration local : fTempDeclarationNodes) {
			if (local == null || local.resolveBinding() == null)
				return RefactoringStatus
						.createFatalErrorStatus(RefactoringCoreMessages.RenameTempRefactoring_must_select_local);
			if (!Checks.isDeclaredIn(local, MethodDeclaration.class)
					&& !Checks.isDeclaredIn(local, Initializer.class))
				return RefactoringStatus
						.createFatalErrorStatus(RefactoringCoreMessages.RenameTempRefactoring_only_in_methods_and_initializers);
		}
		initNames();
		return new RefactoringStatus();
	}

	private void initAST() throws JavaModelException {
		if (!fIsComposite)
			fCompilationUnitNode = RefactoringASTParser.parseWithASTProvider(
					fCu, true, null);
		for (final ILocalVariable local : fLocalVariables) {
			final ISourceRange sourceRange = local.getNameRange();
			final ASTNode name = NodeFinder.perform(fCompilationUnitNode,
					sourceRange);
			if (name == null)
				return;
			if (name instanceof VariableDeclaration) {
				fTempDeclarationNodes.add((VariableDeclaration) name);
			} else if (name.getParent() instanceof VariableDeclaration) {
				fTempDeclarationNodes.add((VariableDeclaration) name
						.getParent());
			}
		}
	}

	private void initNames() {
		for (final VariableDeclaration local : fTempDeclarationNodes) {
			fCurrentNames.add(local.getName().getIdentifier());
		}
	}

	@Override
	protected RenameModifications computeRenameModifications()
			throws CoreException {
		final RenameModifications result = new RenameModifications();
		for (int i = 0; i < fLocalVariables.size(); i++) {
			final ILocalVariable local = fLocalVariables.get(i);
			result.rename(local, new RenameArguments(getNewElementNames()
					.get(i), getUpdateReferences()));
		}
		return result;
	}

	@Override
	protected IFile[] getChangedFiles() throws CoreException {
		return new IFile[] { ResourceUtil.getFile(fCu) };
	}

	@Override
	protected RefactoringStatus doCheckFinalConditions(IProgressMonitor pm,
			CheckConditionsContext context) throws CoreException,
			OperationCanceledException {
		try {
			pm.beginTask("", 1); //$NON-NLS-1$

			final RefactoringStatus result = new RefactoringStatus();
			for (int i = 0; i < fNewNames.size(); i++) {
				final String newName = fNewNames.get(i);
				final ILocalVariable localV = fLocalVariables.get(i);
				checkNewElementName(result, newName, localV);
			}

			final MultiTextEdit rootEdit = new MultiTextEdit();

			fChange = new CompilationUnitChange(
					RefactoringCoreMessages.RenameTempRefactoring_rename, fCu);
			fChange.setEdit(rootEdit);
			fChange.setKeepPreviewEdits(true);

			if (result.hasFatalError())
				return result;
			for (int i = 0; i < fTempDeclarationNodes.size(); i++) {
				final VariableDeclaration vDecl = fTempDeclarationNodes.get(i);
				final String currentName = fCurrentNames.get(i);
				final String newName = fNewNames.get(i);
				createEdits(vDecl, currentName, newName);
			}
			if (!fIsComposite) {
				final LocalAnalyzePackage[] localAnalyzePackages = fLocalAnalyzePackage
						.toArray(new RenameAnalyzeUtil2.LocalAnalyzePackage[0]);
				result.merge(RenameAnalyzeUtil2.analyzeLocalRenames(
						localAnalyzePackages, fChange, fCompilationUnitNode,
						true));
			}
			return result;
		} finally {
			pm.done();
			if (fIsComposite) {
				// end of life cycle for this processor
				fChange = null;
				fCompilationUnitNode = null;
				fTempDeclarationNodes = null;
			}
		}
	}

	/*
	 * @see org.eclipse.jdt.internal.corext.refactoring.tagging.INameUpdating#checkNewElementName(java.lang.String)
	 */
	public RefactoringStatus checkNewElementName(RefactoringStatus result,
			String newName, ILocalVariable localV) throws JavaModelException {
		result.merge(Checks.checkFieldName(newName, localV));
		if (!Checks.startsWithLowerCase(newName))
			if (fIsComposite) {
				final String nameOfParent = (localV.getParent() instanceof IMethod) ? localV
						.getParent().getElementName()
						: RefactoringCoreMessages.JavaElementUtil_initializer;
				final String nameOfType = localV.getAncestor(IJavaElement.TYPE)
						.getElementName();
				result
						.addWarning(Messages
								.format(
										RefactoringCoreMessages.RenameTempRefactoring_lowercase2,
										new String[] { newName, nameOfParent,
												nameOfType }));
			} else {
				result
						.addWarning(RefactoringCoreMessages.RenameTempRefactoring_lowercase);
			}
		return result;
	}

	private void createEdits(VariableDeclaration vDecl, String currentName,
			String newName) {
		final TextEdit declarationEdit = createRenameEdit(vDecl.getName()
				.getStartPosition(), currentName, newName);
		final TextEdit[] allRenameEdits = getAllRenameEdits(declarationEdit,
				vDecl, currentName, newName);

		final TextEdit[] allUnparentedRenameEdits = new TextEdit[allRenameEdits.length];
		TextEdit unparentedDeclarationEdit = null;

		for (int i = 0; i < allRenameEdits.length; i++) {
			if (fIsComposite) {
				// Add a copy of the text edit (text edit may only have one
				// parent) to keep problem reporting code clean
				TextChangeCompatibility
						.addTextEdit(
								fChangeManager.get(fCu),
								RefactoringCoreMessages.RenameTempRefactoring_changeName,
								allRenameEdits[i].copy(), fCategorySet);

				// Add a separate copy for problem reporting
				allUnparentedRenameEdits[i] = allRenameEdits[i].copy();
				if (allRenameEdits[i].equals(declarationEdit))
					unparentedDeclarationEdit = allUnparentedRenameEdits[i];
			}
			fChange.getEdit().addChild(allRenameEdits[i]);
			fChange.addTextEditGroup(new TextEditGroup(
					RefactoringCoreMessages.RenameTempRefactoring_changeName,
					allRenameEdits[i]));
		}

		// store information for analysis
		if (fIsComposite) {
			fLocalAnalyzePackage
					.add(new RenameAnalyzeUtil2.LocalAnalyzePackage(
							unparentedDeclarationEdit, allUnparentedRenameEdits));
		} else
			fLocalAnalyzePackage
					.add(new RenameAnalyzeUtil2.LocalAnalyzePackage(
							declarationEdit, allRenameEdits));
	}

	private TextEdit[] getAllRenameEdits(TextEdit declarationEdit,
			VariableDeclaration vDecl, String currentName, String newName) {
		if (!fUpdateReferences)
			return new TextEdit[] { declarationEdit };

		final TempOccurrenceAnalyzer fTempAnalyzer = new TempOccurrenceAnalyzer(
				vDecl, true);
		fTempAnalyzer.perform();
		final int[] referenceOffsets = fTempAnalyzer
				.getReferenceAndJavadocOffsets();

		final TextEdit[] allRenameEdits = new TextEdit[referenceOffsets.length + 1];
		for (int i = 0; i < referenceOffsets.length; i++)
			allRenameEdits[i] = createRenameEdit(referenceOffsets[i],
					currentName, newName);
		allRenameEdits[referenceOffsets.length] = declarationEdit;
		return allRenameEdits;
	}

	private TextEdit createRenameEdit(int offset, String currentName,
			String newName) {
		return new ReplaceEdit(offset, currentName.length(), newName);
	}

	@Override
	public Change createChange(IProgressMonitor monitor) throws CoreException {
		try {
			Change change = fChange;
			if (change != null) {
				final RenameLocalVariableDescriptor descriptor = new RenameLocalVariableDescriptor();
				final RefactoringDescriptorChange result = new RefactoringDescriptorChange(
						"");
				result.add(change);
				result.markAsSynthetic();
				change = result;
			}
			return change;
		} finally {
			monitor.done();
		}
	}

	public RefactoringStatus initialize(JavaRefactoringArguments arguments) {
		if (arguments instanceof JavaRefactoringArguments) {
			final JavaRefactoringArguments extended = arguments;
			final String handle = extended
					.getAttribute(JavaRefactoringDescriptorUtil.ATTRIBUTE_INPUT);
			if (handle != null) {
				final IJavaElement element = JavaRefactoringDescriptorUtil
						.handleToElement(extended.getProject(), handle, false);
				if (element == null
						|| !element.exists()
						|| element.getElementType() != IJavaElement.COMPILATION_UNIT)
					return JavaRefactoringDescriptorUtil.createInputFatalStatus(
							element, getRefactoring().getName(),
							ID_RENAME_LOCAL_VARIABLE);
				else
					fCu = (ICompilationUnit) element;
			} else
				return RefactoringStatus
						.createFatalErrorStatus(Messages
								.format(
										RefactoringCoreMessages.InitializableRefactoring_argument_not_exist,
										JavaRefactoringDescriptorUtil.ATTRIBUTE_INPUT));
			final String name = extended
					.getAttribute(JavaRefactoringDescriptorUtil.ATTRIBUTE_NAME);
			if (name != null && !"".equals(name)) //$NON-NLS-1$
				setNewElementName(name);
			else
				return RefactoringStatus
						.createFatalErrorStatus(Messages
								.format(
										RefactoringCoreMessages.InitializableRefactoring_argument_not_exist,
										JavaRefactoringDescriptorUtil.ATTRIBUTE_NAME));
			if (fCu != null) {
				final String selection = extended
						.getAttribute(JavaRefactoringDescriptorUtil.ATTRIBUTE_SELECTION);
				if (selection != null) {
					int offset = -1;
					int length = -1;
					final StringTokenizer tokenizer = new StringTokenizer(
							selection);
					if (tokenizer.hasMoreTokens())
						offset = Integer.valueOf(tokenizer.nextToken())
								.intValue();
					if (tokenizer.hasMoreTokens())
						length = Integer.valueOf(tokenizer.nextToken())
								.intValue();
					if (offset >= 0 && length >= 0) {
						try {
							final IJavaElement[] elements = fCu.codeSelect(
									offset, length);
							if (elements != null) {
								for (int index = 0; index < elements.length; index++) {
									final IJavaElement element = elements[index];
									if (element instanceof ILocalVariable)
										fLocalVariables
												.add((ILocalVariable) element); // todo
								}
							}
							if (fLocalVariables == null)
								return JavaRefactoringDescriptorUtil
										.createInputFatalStatus(null,
												getRefactoring().getName(),
												ID_RENAME_LOCAL_VARIABLE);
						} catch (final JavaModelException exception) {
							JavaPlugin.log(exception);
						}
					} else
						return RefactoringStatus
								.createFatalErrorStatus(Messages
										.format(
												RefactoringCoreMessages.InitializableRefactoring_illegal_argument,
												new Object[] {
														selection,
														JavaRefactoringDescriptorUtil.ATTRIBUTE_SELECTION }));
				} else
					return RefactoringStatus
							.createFatalErrorStatus(Messages
									.format(
											RefactoringCoreMessages.InitializableRefactoring_argument_not_exist,
											JavaRefactoringDescriptorUtil.ATTRIBUTE_SELECTION));
			}
			final String references = extended
					.getAttribute(ATTRIBUTE_REFERENCES);
			if (references != null) {
				fUpdateReferences = Boolean.valueOf(references).booleanValue();
			} else
				return RefactoringStatus
						.createFatalErrorStatus(Messages
								.format(
										RefactoringCoreMessages.InitializableRefactoring_argument_not_exist,
										ATTRIBUTE_REFERENCES));
		} else
			return RefactoringStatus
					.createFatalErrorStatus(RefactoringCoreMessages.InitializableRefactoring_inacceptable_arguments);
		return new RefactoringStatus();
	}

	public List<RenameAnalyzeUtil2.LocalAnalyzePackage> getLocalAnalyzePackage() {
		return fLocalAnalyzePackage;
	}

	public RefactoringStatus checkNewElementName(String newName)
			throws CoreException {
		// TODO Auto-generated method stub
		return null;
	}

	public String getCurrentElementName() {
		// TODO Auto-generated method stub
		return fCurrentNames.get(0);
	}

	@Override
	public int getSaveMode() {
		// TODO Auto-generated method stub
		return 0;
	}
}
