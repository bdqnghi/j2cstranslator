package com.ilog.translator.java2cs.translation;

import java.io.IOException;

import org.eclipse.core.runtime.CoreException;
import org.eclipse.core.runtime.IProgressMonitor;

public interface ITranslator {

	public abstract boolean translate(IProgressMonitor monitor)
			throws CoreException, IOException;

	public void copyResources(IProgressMonitor pm);
}