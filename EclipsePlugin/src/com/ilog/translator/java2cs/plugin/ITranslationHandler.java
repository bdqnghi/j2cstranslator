package com.ilog.translator.java2cs.plugin;

import org.eclipse.core.resources.IProject;

import com.ilog.translator.java2cs.configuration.TranslatorProjectOptions;

/**
 * @author afau
 */
public interface ITranslationHandler {

	/**
	 * @param project
	 * @param options
	 * @param needToReadOptions
	 */
	void translateProjects(IProject project, TranslatorProjectOptions options,
			boolean needToReadOptions);
}
