package com.ilog.translator.java2cs.plugin;

import org.eclipse.core.resources.IProject;
import org.eclipse.core.resources.IProjectDescription;
import org.eclipse.core.resources.IProjectNature;
import org.eclipse.core.runtime.CoreException;

public class TranslationNature implements IProjectNature {

	private IProject project;

	public void configure() throws CoreException {
		// TODO Auto-generated method stub
		final IProjectDescription description = project.getDescription();

	}

	public void deconfigure() throws CoreException {
		// TODO Auto-generated method stub
		final IProjectDescription description = project.getDescription();
	}

	public IProject getProject() {
		// TODO Auto-generated method stub
		return project;
	}

	public void setProject(IProject project) {
		this.project = project;
	}

}
