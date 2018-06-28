package com.ilog.translator.java2cs.plugin;

import org.eclipse.core.resources.IProject;
import org.eclipse.core.runtime.IProgressMonitor;
import org.eclipse.jdt.core.IJavaProject;
import org.eclipse.jdt.core.JavaCore;
import org.eclipse.jface.dialogs.ProgressMonitorDialog;
import org.eclipse.jface.operation.IRunnableWithProgress;
import org.eclipse.ui.internal.Workbench;

import com.ilog.translator.java2cs.configuration.TranslatorProjectOptions;
import com.ilog.translator.java2cs.plugin.util.ProjectBuilder;

/**
 * 
 */
@SuppressWarnings(value = { "restriction" })
public class TranslationHandler implements ITranslationHandler {

	//
	//
	//

	public void translateProjects(IProject project,
			final TranslatorProjectOptions options,
			final boolean needToReadOptions) {
		try {
			final IJavaProject javaProject = JavaCore.create(project);
			options.getGlobalOptions().getLogger().logInfo(
					"--- Translating project '" + project.getName()
							+ "' to directory '" + options.getSourcesDestDir()
							+ "'");
			final IRunnableWithProgress runnable = new IRunnableWithProgress() {

				public void run(IProgressMonitor monitor) {
					try {
						boolean res = ProjectBuilder.copyAndTranslate(
								javaProject, options, needToReadOptions,
								monitor, false);
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
}