package com.ilog.translator.java2cs.popup.actions;

import java.io.IOException;

import javax.xml.parsers.ParserConfigurationException;

import org.eclipse.core.resources.IContainer;
import org.eclipse.core.resources.IFile;
import org.eclipse.core.resources.IProject;
import org.eclipse.core.runtime.CoreException;
import org.eclipse.jdt.core.ICompilationUnit;
import org.eclipse.jdt.core.IJavaElement;
import org.eclipse.jdt.core.IJavaProject;
import org.eclipse.jdt.core.JavaCore;
import org.eclipse.jdt.core.JavaModelException;
import org.eclipse.jdt.internal.core.SourceMethod;
import org.eclipse.jdt.internal.core.SourceType;
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
import org.xml.sax.SAXException;

import com.ilog.translator.java2cs.configuration.GlobalOptions;
import com.ilog.translator.java2cs.configuration.Java2CsModel;
import com.ilog.translator.java2cs.configuration.TranslationConfiguration;
import com.ilog.translator.java2cs.configuration.TranslatorProjectOptions;
import com.ilog.translator.java2cs.plugin.Messages;

@SuppressWarnings("restriction")
public class MigrateThisMappingFileAction extends ActionDelegate implements
		IObjectActionDelegate, IEditorActionDelegate {

	IEditorPart editor = null;

	/**
	 * Constructor for Action1.
	 */
	public MigrateThisMappingFileAction() {
		super();
	}

	@Override
	public void run(IAction action) {
		final PluginAction opa = (PluginAction) action;
		if (opa.getSelection() instanceof StructuredSelection) {
			final StructuredSelection selection = (StructuredSelection) opa
					.getSelection();
			final Object firstElem = selection.getFirstElement();
			if ((firstElem instanceof IFile)) {
				final IFile proj = (IFile) firstElem;
				migrate(proj);
			}		
		}
	}

	public void migrate(IFile file) {
		try {
			IProject iproject = file.getProject();
			final IJavaProject javaProject = JavaCore.create(iproject);
			final GlobalOptions globalOptions = new GlobalOptions();
			migrateMappingFiles(javaProject, file,  globalOptions);
		} catch (final Exception e) {
			System.err
					.println(Messages
							.getString("ProjectTransferHandler.error_copying_projects") + e); //$NON-NLS-1$
		}
	}

	private static void migrateMappingFiles(IJavaProject javaProject, IFile file, 
			GlobalOptions globalOptions) throws ParserConfigurationException,
			SAXException, JavaModelException, IOException, CoreException {
		final String translatorDirectory = TranslatorProjectOptions
				.searchTranslatorDir(javaProject, globalOptions, true, true,
						null /* TODO */);
		final String confFileName = TranslatorProjectOptions
				.getOrCreateTranslatorConfigurationFile(javaProject,
						globalOptions, true, translatorDirectory, true);
		final TranslatorProjectOptions options = new TranslatorProjectOptions(
				confFileName, globalOptions);
		options.read(javaProject, false);
		final TranslationConfiguration configuration = new TranslationConfiguration(
				javaProject, javaProject, options);
		final Java2CsModel model = new Java2CsModel(javaProject, configuration
				.getLogger(), configuration);
		model.migrateThisMappingFile(file);
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
