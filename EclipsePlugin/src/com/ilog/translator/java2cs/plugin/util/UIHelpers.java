package com.ilog.translator.java2cs.plugin.util;

import org.eclipse.swt.SWT;
import org.eclipse.swt.layout.GridData;
import org.eclipse.swt.widgets.Composite;
import org.eclipse.swt.widgets.Label;

public class UIHelpers {
	/**
	 * Creates a new label with a standard font.
	 * 
	 * @param parent
	 *            the parent control
	 * @param text
	 *            the label text
	 * 
	 * @return the new label control
	 */
	public final static Label createPlainLabel(Composite parent, String text) {
		final Label label = new Label(parent, SWT.NONE);
		label.setFont(parent.getFont());
		label.setText(text);
		final GridData data = new GridData(GridData.FILL, GridData.FILL, false,
				false, 1, 1);
		label.setLayoutData(data);
		return label;
	}
}
