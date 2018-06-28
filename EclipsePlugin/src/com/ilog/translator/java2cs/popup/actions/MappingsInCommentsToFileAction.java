package com.ilog.translator.java2cs.popup.actions;

import org.eclipse.jdt.core.ICompilationUnit;
import org.eclipse.jdt.core.IJavaElement;
import org.eclipse.jdt.core.IJavaProject;
import org.eclipse.jdt.core.IType;
import org.eclipse.jdt.core.JavaModelException;
import org.eclipse.jdt.core.dom.CompilationUnit;
import org.eclipse.jdt.core.dom.TypeDeclaration;
import org.eclipse.jdt.internal.core.SourceMethod;
import org.eclipse.jdt.internal.core.SourceType;
import org.eclipse.jdt.internal.corext.refactoring.structure.CompilationUnitRewrite;
import org.eclipse.jdt.internal.ui.javaeditor.CompilationUnitEditor;
import org.eclipse.jdt.ui.JavaUI;
import org.eclipse.jface.action.IAction;
import org.eclipse.jface.text.ITextSelection;
import org.eclipse.jface.viewers.ISelection;
import org.eclipse.jface.viewers.StructuredSelection;
import org.eclipse.ui.IEditorActionDelegate;
import org.eclipse.ui.IEditorPart;
import org.eclipse.ui.IObjectActionDelegate;
import org.eclipse.ui.IWorkbenchPart;
import org.eclipse.ui.actions.ActionDelegate;
import org.eclipse.ui.internal.PluginAction;

import com.ilog.translator.java2cs.configuration.GlobalOptions;
import com.ilog.translator.java2cs.configuration.TranslationConfiguration;
import com.ilog.translator.java2cs.configuration.TranslatorProjectOptions;
import com.ilog.translator.java2cs.plugin.Messages;
import com.ilog.translator.java2cs.translation.TranslationContext;

@SuppressWarnings("restriction")
public class MappingsInCommentsToFileAction extends ActionDelegate implements
		IObjectActionDelegate, IEditorActionDelegate {

	IEditorPart editor = null;

	/**
	 * Constructor for Action1.
	 */
	public MappingsInCommentsToFileAction() {
		super();
	}

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
		try {
			final IJavaProject javaProject = compilationUnit.getJavaProject();
			final GlobalOptions globalOptions = new GlobalOptions();
			final String translatorDirectory = TranslatorProjectOptions
					.searchTranslatorDir(javaProject, globalOptions, true,
							false, /* TODO */null); // TODO
			final String confFileName = TranslatorProjectOptions
					.getOrCreateTranslatorConfigurationFile(javaProject,
							globalOptions, true, translatorDirectory, false); // TODO:
			final TranslatorProjectOptions options = new TranslatorProjectOptions(
					confFileName, globalOptions);
			options.read(javaProject, false);
			globalOptions.getLogger().logInfo(
					"--- Translating project '"
							+ javaProject.getProject().getName()
							+ "' to directory '" + options.getSourcesDestDir()
							+ "'");
			//
			final IType primaryType = compilationUnit.findPrimaryType();
			if (primaryType == null)
				return;
			//
			final CompilationUnitRewrite cur = new CompilationUnitRewrite(
					compilationUnit);
			final CompilationUnit cu = cur.getRoot();

			final TypeDeclaration typeDecl = searchForType(cu, primaryType);
			final TranslationConfiguration configuration = new TranslationConfiguration(
					javaProject, javaProject, options);
			final TranslationContext context = new TranslationContext(
					javaProject, configuration);
			context.extractMappingFromCommentsToFile(typeDecl);
		} catch (final Exception e) {
			System.err
					.println(Messages
							.getString("ProjectTransferHandler.error_copying_projects") + e); //$NON-NLS-1$
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
