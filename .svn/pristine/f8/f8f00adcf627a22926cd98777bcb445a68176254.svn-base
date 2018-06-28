/**
 * 
 */
package com.ilog.translator.java2cs.configuration.options;

import org.eclipse.swt.SWT;
import org.eclipse.swt.events.ModifyEvent;
import org.eclipse.swt.events.ModifyListener;
import org.eclipse.swt.layout.GridData;
import org.eclipse.swt.layout.GridLayout;
import org.eclipse.swt.widgets.Composite;
import org.eclipse.swt.widgets.Control;
import org.eclipse.swt.widgets.Label;
import org.eclipse.swt.widgets.Text;

import java.util.*;

public class ListOfStringOptionEditor extends AbstractEditor<OptionImpl<List<String>>> {

	private class OptionListOfStringModifyListener implements
			ModifyListener {

		private final OptionImpl<List<String>> option;

		public OptionListOfStringModifyListener(OptionImpl<List<String>> option) {
			this.option = option;
		}

		public void modifyText(ModifyEvent e) {
			// THIS COPIED FROM the ArrayOfStringOptionBuilder
			List<String> list = new ArrayList<String>();
			for(String s : text.getText().split(","))
				list.add(s);
			option.setValue(list);
		}
	}

	private Composite subComposite = null;
	private Text text = null;

	public Control createUIEditor(Composite parent, OptionImpl<List<String>> option) {

		if (subComposite == null) {
			final GridLayout gridLayout = new GridLayout();
			gridLayout.numColumns = 2;
			gridLayout.makeColumnsEqualWidth = false;
			gridLayout.marginLeft = 0;
			gridLayout.marginWidth = 0;
			gridLayout.marginHeight = 0;

			final GridData gdSubComposite = new GridData();
			gdSubComposite.horizontalAlignment = SWT.LEFT;

			subComposite = new Composite(parent, SWT.NULL);
			subComposite.setLayout(gridLayout);
			subComposite.setLayoutData(gdSubComposite);

			final Label label = new Label(subComposite, SWT.NONE);
			label.setText(getTextLabel(option.getName()));
			label.setToolTipText(getTooltip(option.getName()));

			final GridData gridData = new GridData();
			gridData.horizontalAlignment = SWT.FILL;
			gridData.verticalAlignment = SWT.CENTER;

			text = new Text(subComposite, SWT.BORDER);
			initializeValue(option);

			text.setToolTipText(getTooltip(option.getName()));
			text.addModifyListener(new OptionListOfStringModifyListener(
					option));
			text.setLayoutData(gridData);
		}
		return subComposite;
	}

	public void initializeValue(OptionImpl<List<String>> option) {
		if (text != null) {
			List<String> values = option.getValue();
			if (values == null) {
				values = option.getDefaultValue();
				if (values == null)
					values = new ArrayList<String>();
			}

			final StringBuffer strValuesBuffer = new StringBuffer();
			for (final String stringValue : values) {
				strValuesBuffer.append(stringValue);
				strValuesBuffer.append(',');
			}
			if (strValuesBuffer.length() > 0)
				strValuesBuffer.deleteCharAt(strValuesBuffer.length() - 1);

			text.setText(strValuesBuffer.toString());
		}
	}

}