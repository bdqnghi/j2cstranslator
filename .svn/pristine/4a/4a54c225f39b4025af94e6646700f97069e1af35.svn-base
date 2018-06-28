package com.ilog.translator.java2cs.plugin.util;

import java.io.File;

import org.eclipse.core.resources.IProject;
import org.eclipse.core.resources.IProjectDescription;
import org.eclipse.core.resources.IWorkspace;
import org.eclipse.core.resources.ResourcesPlugin;
import org.eclipse.core.runtime.CoreException;
import org.eclipse.core.runtime.IPath;
import org.eclipse.core.runtime.Path;
import org.eclipse.core.runtime.Platform;

/**
 * Import an external project into the workspace
 */
public class ProjectImporter {

	private IProjectDescription description;

	private IPath locationPath;

	private String projectName;

	private static final String PROJECT_DESCRIPTION_FILENAME = ".project";

	public ProjectImporter() {
	}

	public String createExistingProject(IPath locationPath) throws Exception {
		this.locationPath = locationPath;
		setProjectDescription(new File(locationPath.toOSString(),
				PROJECT_DESCRIPTION_FILENAME));
		return createExistingProject().getName();
	}

	/**
	 * Return whether or not the specifed location is a prefix of the root.
	 */
	private boolean isPrefixOfRoot(IPath locationPath) {
		return Platform.getLocation().isPrefixOf(locationPath);
	}

	/**
	 * Set the project name using either the name of the parent of the file or
	 * the name entry in the xml for the file
	 */
	private void setProjectDescription(File projectFile) throws CoreException {

		// If there is no file or the user has already specified forget it
		if (projectFile == null)
			return;

		final IPath path = new Path(projectFile.getPath());

		IProjectDescription newDescription = null;

		newDescription = ResourcesPlugin.getWorkspace().loadProjectDescription(
				path);

		if (newDescription == null) {
			description = null;
			projectName = "";
		} else {
			description = newDescription;
			projectName = description.getName();
		}
	}

	private IProject createExistingProject() throws Exception {
		final IWorkspace workspace = ResourcesPlugin.getWorkspace();
		final IProject project = workspace.getRoot().getProject(projectName);
		if (description == null) {
			description = workspace.newProjectDescription(projectName);
			// If it is under the root use the default location
			if (isPrefixOfRoot(locationPath))
				description.setLocation(null);
			else
				description.setLocation(locationPath);
		} else
			description.setName(projectName);

		// create the new project operation
		// workspace.run(new IWorkspaceRunnable() {
		// public void run(IProgressMonitor monitor) throws CoreException {
		project.create(description, null);
		project.open(null);
		// }
		// },null);

		return project;
	}

}
