package com.ilog.translator.java2cs.popup.actions;

import org.eclipse.core.runtime.CoreException;
import org.eclipse.core.runtime.IProgressMonitor;
import org.eclipse.core.runtime.NullProgressMonitor;
import org.eclipse.core.runtime.SubProgressMonitor;
import org.eclipse.jdt.core.ICompilationUnit;
import org.eclipse.jdt.core.IField;
import org.eclipse.jdt.core.IJavaElement;
import org.eclipse.jdt.core.IMethod;
import org.eclipse.jdt.core.IType;
import org.eclipse.jdt.core.JavaModelException;
import org.eclipse.jdt.core.dom.BodyDeclaration;
import org.eclipse.jdt.core.dom.CompilationUnit;
import org.eclipse.jdt.core.dom.FieldDeclaration;
import org.eclipse.jdt.core.dom.MethodDeclaration;
import org.eclipse.jdt.core.dom.TypeDeclaration;
import org.eclipse.jdt.core.dom.VariableDeclarationFragment;
import org.eclipse.jdt.internal.corext.refactoring.structure.CompilationUnitRewrite;
import org.eclipse.jdt.internal.ui.javaeditor.CompilationUnitEditor;
import org.eclipse.jdt.ui.JavaUI;
import org.eclipse.jface.action.IAction;
import org.eclipse.jface.text.ITextSelection;
import org.eclipse.jface.viewers.ISelection;
import org.eclipse.jface.viewers.StructuredSelection;
import org.eclipse.ltk.core.refactoring.Change;
import org.eclipse.ltk.core.refactoring.RefactoringStatus;
import org.eclipse.ui.IEditorActionDelegate;
import org.eclipse.ui.IEditorPart;
import org.eclipse.ui.IObjectActionDelegate;
import org.eclipse.ui.IWorkbenchPart;
import org.eclipse.ui.actions.ActionDelegate;
import org.eclipse.ui.internal.PluginAction;

import com.ilog.translator.java2cs.translation.util.TranslationUtils;

@SuppressWarnings("restriction")
public abstract class AbstractMarkAsAction extends ActionDelegate implements
		IObjectActionDelegate, IEditorActionDelegate {
	IEditorPart editor = null;

	/**
	 * Constructor for Action1.
	 */
	public AbstractMarkAsAction() {
		super();
	}

	public abstract String getTagName();

	public abstract String getTagValue();

	protected boolean applyOnType = false;
	protected boolean applyOnMethod = false;
	protected boolean applyOnField = false;

	@Override
	public void run(IAction action) {
		final PluginAction opa = (PluginAction) action;
		try {
			if (opa.getSelection() instanceof StructuredSelection) {
				final StructuredSelection selection = (StructuredSelection) opa
						.getSelection();
				final Object firstElem = selection.getFirstElement();
				apply(firstElem);
			} else {
				if (editor instanceof CompilationUnitEditor) {

					final CompilationUnitEditor cuEditor = (CompilationUnitEditor) editor;
					final ITextSelection select = (ITextSelection) opa
							.getSelection();
					final ICompilationUnit element = (ICompilationUnit) JavaUI
							.getEditorInputJavaElement(cuEditor
									.getEditorInput());
					final IJavaElement firstElem = element.getElementAt(select
							.getOffset()
							+ select.getLength() / 2);
					apply(firstElem);
				}
			}
		} catch (final Exception e) {

		}
	}

	protected void createOrUpdateTag(CompilationUnitRewrite cur,
			BodyDeclaration body) {
		TranslationUtils.createOrUpdateTag(cur, body, getTagName(),
				getTagValue());
	}

	private void apply(Object firstElem) throws Exception {
		if ((firstElem instanceof IMethod) && applyOnMethod) {
			final IMethod method = (IMethod) firstElem;
			final ICompilationUnit compilationUnit = method
					.getCompilationUnit();
			final IType declarintType = method.getDeclaringType();

			final CompilationUnitRewrite cur = new CompilationUnitRewrite(
					compilationUnit);
			final CompilationUnit cu = cur.getRoot();
			final TypeDeclaration typeDecl = searchForType(cu, declarintType);
			final MethodDeclaration methodDecl = searchForMethod(cu, typeDecl,
					method);
			createOrUpdateTag(cur, methodDecl);
			applyChange(new NullProgressMonitor(), cur.createChange());
		} else if ((firstElem instanceof IType) && applyOnType) {
			final IType type = (IType) firstElem;
			final ICompilationUnit compilationUnit = type.getCompilationUnit();
			final IType[] types = compilationUnit.getTypes();
			final IType declarintType = types[0];

			final CompilationUnitRewrite cur = new CompilationUnitRewrite(
					compilationUnit);
			final CompilationUnit cu = cur.getRoot();

			final TypeDeclaration typeDecl = searchForType(cu, declarintType);
			createOrUpdateTag(cur, typeDecl);
			applyChange(new NullProgressMonitor(), cur.createChange());
		} else if ((firstElem instanceof IField) && applyOnField) {
			final IField field = (IField) firstElem;
			final ICompilationUnit compilationUnit = field.getCompilationUnit();
			final IType[] types = compilationUnit.getTypes();
			final IType declarintType = types[0];

			final CompilationUnitRewrite cur = new CompilationUnitRewrite(
					compilationUnit);
			final CompilationUnit cu = cur.getRoot();

			final TypeDeclaration typeDecl = searchForType(cu, declarintType);
			final FieldDeclaration methodDecl = searchForField(cu, typeDecl,
					field);
			createOrUpdateTag(cur, methodDecl);
			applyChange(new NullProgressMonitor(), cur.createChange());
		}
	}

	private TypeDeclaration searchForType(CompilationUnit cu, IType type) {
		for (final Object obj : cu.types()) {
			final TypeDeclaration typeDecl = (TypeDeclaration) obj;
			if (typeDecl.resolveBinding().getJavaElement()
					.getHandleIdentifier().equals(type.getHandleIdentifier())) {
				return typeDecl;
			}
		}
		return null;
	}

	private MethodDeclaration searchForMethod(CompilationUnit cu,
			TypeDeclaration typeDecl, IMethod method) {
		for (final MethodDeclaration methodDecl : typeDecl.getMethods()) {
			if (methodDecl.resolveBinding().getJavaElement()
					.getHandleIdentifier().equals(method.getHandleIdentifier())) {
				return methodDecl;
			}
		}
		return null;
	}

	private FieldDeclaration searchForField(CompilationUnit cu,
			TypeDeclaration typeDecl, IField field) {
		for (final FieldDeclaration fieldDecl : typeDecl.getFields()) {
			final VariableDeclarationFragment frag = (VariableDeclarationFragment) fieldDecl
					.fragments().get(0);
			if (frag.getName().getIdentifier().equals(field.getElementName())) {
				return fieldDecl;
			}
		}
		return null;
	}

	public boolean applyChange(IProgressMonitor pm, Change change)
			throws CoreException {

		if (change != null) {
			final IProgressMonitor subMonitor = new SubProgressMonitor(pm, 1);

			try {
				change.initializeValidationData(subMonitor);
				if (!change.isEnabled()) {
					return false;
				}
				final RefactoringStatus valid = change.isValid(subMonitor);
				if (valid.hasFatalError()) {
					return false;
				}
				change.perform(subMonitor);
				return true;
			} finally {
				change.dispose();
				change = null;
			}
		} else {
			return false;
		}
	}

	public void setActivePart(IAction action, IWorkbenchPart targetPart) {
		// TODO Auto-generated method stub
	}

	@Override
	public void selectionChanged(IAction action, ISelection selection) {
	}

	protected IType[] getTypes(ICompilationUnit compilationUnit) {
		try {
			return compilationUnit.getTypes();
		} catch (final JavaModelException jme) {
			// TestNGPlugin.log(jme);
		}
		return null;
	}

	public void setActiveEditor(IAction action, IEditorPart targetEditor) {
		editor = targetEditor;
	}
}
