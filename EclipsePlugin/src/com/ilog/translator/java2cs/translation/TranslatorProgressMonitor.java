package com.ilog.translator.java2cs.translation;

import org.eclipse.core.runtime.IProgressMonitor;

public class TranslatorProgressMonitor implements IProgressMonitor {
	private com.ilog.translator.java2cs.configuration.Logger logger = null;
	@SuppressWarnings("unused")
	private int level = 0;

	public TranslatorProgressMonitor(
			com.ilog.translator.java2cs.configuration.Logger logger, int level) {
		this.level = level;
		this.logger = logger;
	}

	//
	//
	//

	public void beginTask(String name, int totalWork) {
		logger.logDebug(">> " + name);
	}

	public void done() {
	}

	public void internalWorked(double work) {
	}

	public boolean isCanceled() {
		return false;
	}

	public void setCanceled(boolean value) {
	}

	public void setTaskName(String name) {
	}

	public void subTask(String name) {
		logger.logDebug("   " + name);
	}

	public void worked(int work) {
	}

}
