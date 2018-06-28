/**
 * 
 */
package com.ilog.translator.java2cs.configuration.options;

import org.eclipse.swt.SWT;
import org.eclipse.swt.events.SelectionEvent;
import org.eclipse.swt.widgets.Button;
import org.eclipse.swt.widgets.Composite;
import org.eclipse.swt.widgets.Control;

public class BooleanOptionEditor extends AbstractEditor<OptionImpl<Boolean>> {

	private Button checkBoxBtn = null;

	private class OptionBooleanSelectionListener extends
			org.eclipse.swt.events.SelectionAdapter {

		private final OptionImpl<Boolean> option;

		public OptionBooleanSelectionListener(OptionImpl<Boolean> option) {
			this.option = option;
		}

		@Override
		public void widgetSelected(SelectionEvent e) {
			option.setValue(!option.getValue());
		}

	}

	public Control createUIEditor(Composite parent, OptionImpl<Boolean> option) {

		if (checkBoxBtn == null) {
			checkBoxBtn = new Button(parent, SWT.CHECK);
			checkBoxBtn.setText(getTextLabel(option.getName()));
			checkBoxBtn.setToolTipText(getTooltip(option.getName()));
			initializeValue(option);
			checkBoxBtn
					.addSelectionListener(new OptionBooleanSelectionListener(
							option));
		}
		return checkBoxBtn;
	}

	public void initializeValue(OptionImpl<Boolean> option) {
		if (checkBoxBtn != null) {
			if (option.getValue() != null) {
				checkBoxBtn.setSelection(option.getValue());
			} else {
				checkBoxBtn.setSelection(option.getDefaultValue());
			}
		}
	}
}