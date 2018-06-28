package com.ilog.translator.java2cs.popup.actions;

import org.eclipse.core.runtime.IProgressMonitor;
import org.eclipse.jdt.core.ICompilationUnit;
import org.eclipse.jdt.core.IJavaElement;
import org.eclipse.jdt.core.IJavaProject;
import org.eclipse.jdt.core.IPackageFragment;
import org.eclipse.jdt.core.IType;
import org.eclipse.jdt.core.JavaModelException;
import org.eclipse.jdt.internal.core.SourceMethod;
import org.eclipse.jdt.internal.core.SourceType;
import org.eclipse.jdt.internal.ui.javaeditor.CompilationUnitEditor;
import org.eclipse.jdt.ui.JavaUI;
import org.eclipse.jface.action.IAction;
import org.eclipse.jface.dialogs.ProgressMonitorDialog;
import org.eclipse.jface.operation.IRunnableWithProgress;
import org.eclipse.jface.text.ITextSelection;
import org.eclipse.jface.viewers.ISelection;
import org.eclipse.jface.viewers.StructuredSelection;
import org.eclipse.ui.IEditorActionDelegate;
import org.eclipse.ui.IEditorPart;
import org.eclipse.ui.IObjectActionDelegate;
import org.eclipse.ui.IWorkbenchPart;
import org.eclipse.ui.actions.ActionDelegate;
import org.eclipse.ui.internal.PluginAction;
import org.eclipse.ui.internal.Workbench;

import com.ilog.translator.java2cs.configuration.GlobalOptions;
import com.ilog.translator.java2cs.configuration.TranslatorProjectOptions;
import com.ilog.translator.java2cs.plugin.Messages;
import com.ilog.translator.java2cs.plugin.util.ProjectBuilder;

public class TranslateCompilationUnitAction extends ActionDelegate implements
		IObjectActionDelegate, IEditorActionDelegate {

	IEditorPart editor = null;

	/**
	 * Constructor for Action1.
	 */
	public TranslateCompilationUnitAction() {
		super();
	}

	@SuppressWarnings("restriction")
	@Override
	public void run(IAction action) {
		final PluginAction opa = (PluginAction) action;
		if (opa.getSelection() instanceof StructuredSelection) {
			final StructuredSelection selection = (StructuredSelection) opa
					.getSelection();
			final Object firstElem = selection.getFirstElement();
			if ((firstElem instanceof SourceMethod)) {
				final SourceMethod method = (SourceMethod) firstElem;
				final ICompilationUnit compilationUnit = method
						.getCompilationUnit();
				translate(compilationUnit);
			} else if ((firstElem instanceof SourceType)) {
				final SourceType type = (SourceType) firstElem;
				final ICompilationUnit compilationUnit = type
						.getCompilationUnit();
				translate(compilationUnit);
			} else if ((firstElem instanceof IJavaProject)) {
				final IJavaProject proj = (IJavaProject) firstElem;
				translate(proj, null);
			}
		} else {
			if (editor instanceof CompilationUnitEditor) {
				try {
					final CompilationUnitEditor cuEditor = (CompilationUnitEditor) editor;
					final ITextSelection select = (ITextSelection) opa
							.getSelection();
					final ICompilationUnit element = (ICompilationUnit) JavaUI
							.getEditorInputJavaElement(cuEditor
									.getEditorInput());
					final IJavaElement selectElement = element
							.getElementAt(select.getOffset()
									+ select.getLength() / 2);
					System.out.println();
				} catch (final JavaModelException e) {

				}
			}
		}
	}

	public void translate(ICompilationUnit compilationUnit) {
		final IJavaProject javaProject = compilationUnit.getJavaProject();
		translate(javaProject, compilationUnit);
	}

	public void translate(final IJavaProject javaProject,
			ICompilationUnit compilationUnit) {
		try {
			final GlobalOptions globalOptions = new GlobalOptions();
			final String translatorDirectory = TranslatorProjectOptions
					.searchTranslatorDir(javaProject, globalOptions, true,
							false, null /* TODO */);
			final String confFileName = TranslatorProjectOptions
					.getOrCreateTranslatorConfigurationFile(javaProject,
							globalOptions, true, translatorDirectory, false);
			final TranslatorProjectOptions options = new TranslatorProjectOptions(
					confFileName, globalOptions);
			options.read(javaProject, false);
			globalOptions.getLogger().logInfo(
					"--- Translating project '"
							+ javaProject.getProject().getName()
							+ "' to directory '" + options.getSourcesDestDir()
							+ "'");
			//
			if (compilationUnit != null) {
				final IType primaryType = compilationUnit.findPrimaryType();
				if (primaryType == null)
					return;
				final IPackageFragment frag = primaryType.getPackageFragment();
				String packageName = "";
				if (frag != null) {
					packageName = frag.getElementName() + ".";
				}
				//
				final String className = packageName
						+ primaryType.getElementName();
				options.setClassFilter(new String[] { className });
			}
			//
			final IRunnableWithProgress runnable = new IRunnableWithProgress() {

				public void run(IProgressMonitor monitor) {
					try {
						boolean res = ProjectBuilder.copyAndTranslate(
								javaProject, options, true, monitor, false);
					} catch (Exception e) {
						options.getGlobalOptions().getLogger().logException(
								"Error ", e);
					}
				}
			};
			final ProgressMonitorDialog pmd = new ProgressMonitorDialog(
					Workbench.getInstance().getActiveWorkbenchWindow()
							.getShell());
			pmd.run(true, true, runnable);
		} catch (final InterruptedException e2) {
			System.err.println("Translation interrupt by user");
		} catch (final Exception e) {
			System.err
					.println(Messages
							.getString("ProjectTransferHandler.error_copying_projects") + e); //$NON-NLS-1$
		}
	}

	public void setActivePart(IAction action, IWorkbenchPart targetPart) {
		// TODO Auto-generated method stub
	}

	@Override
	public void selectionChanged(IAction action, ISelection selection) {
	}

	public void setActiveEditor(IAction action, IEditorPart targetEditor) {
		editor = targetEditor;
	}

}
