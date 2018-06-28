/**
 * 
 */
package com.ilog.translator.java2cs.configuration.options;

import org.eclipse.swt.SWT;
import org.eclipse.swt.events.SelectionEvent;
import org.eclipse.swt.layout.GridData;
import org.eclipse.swt.layout.GridLayout;
import org.eclipse.swt.widgets.Combo;
import org.eclipse.swt.widgets.Composite;
import org.eclipse.swt.widgets.Control;
import org.eclipse.swt.widgets.Label;

public abstract class ComboBoxEditor<T> extends AbstractEditor<OptionImpl<T>> {

	public class OptionComboSelectionListener extends
			org.eclipse.swt.events.SelectionAdapter {

		private final OptionImpl<T> option;

		public OptionComboSelectionListener(OptionImpl<T> option) {
			this.option = option;
		}

		@Override
		public void widgetSelected(SelectionEvent e) {
			setOptionValue(option, combo.getSelectionIndex());
		}

	}

	private Composite subComposite = null;
	private Combo combo = null;

	public Control createUIEditor(Composite parent, OptionImpl<T> option) {

		if (this.subComposite == null) {
			final GridLayout gridLayout = new GridLayout();
			gridLayout.numColumns = 2;
			gridLayout.makeColumnsEqualWidth = false;
			gridLayout.marginLeft = 0;
			gridLayout.marginWidth = 0;
			gridLayout.marginHeight = 0;

			final GridData gdSubComposite = new GridData();
			gdSubComposite.horizontalAlignment = SWT.LEFT;

			this.subComposite = new Composite(parent, SWT.NULL);
			this.subComposite.setLayout(gridLayout);
			this.subComposite.setLayoutData(gdSubComposite);

			final Label label = new Label(this.subComposite, SWT.NONE);
			label.setText(getTextLabel(option.getName()));
			label.setToolTipText(getTooltip(option.getName()));

			final GridData gridData = new GridData();
			gridData.verticalAlignment = SWT.CENTER;

			this.combo = new Combo(this.subComposite, SWT.READ_ONLY);

			this.combo.setItems(getListItems());
			initializeValue(option);
			this.combo.setToolTipText(getTooltip(option.getName()));
			this.combo
					.addSelectionListener(new OptionComboSelectionListener(
							option));
			this.combo.setLayoutData(gridData);
		}
		return this.subComposite;
	}

	@SuppressWarnings("unchecked")
	public void initializeValue(OptionImpl<T> option) {
		if (this.combo != null) {
			final int ordinal = ((Enum) option.getValue()).ordinal();
			this.combo.select(ordinal);
		}
	}

	abstract String[] getListItems();

	abstract void setOptionValue(OptionImpl<T> option, int index);
}