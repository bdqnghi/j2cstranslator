package com.ilog.translator.java2cs.configuration.options;

import org.eclipse.swt.widgets.Composite;
import org.eclipse.swt.widgets.Control;

/**
 * Editor for option in the UI.
 * 
 * @param <T>
 *            the option that has the UI.
 */
public interface Editor<T> {
	/**
	 * Returns the control if it has been created otherwise creates and
	 * returns it.
	 * 
	 * @param option
	 * @return
	 */
	Control createUIEditor(Composite parent, T option);

	/**
	 * Initializes the control with the value of the option.
	 * 
	 * @param option
	 */
	void initializeValue(T option);
}