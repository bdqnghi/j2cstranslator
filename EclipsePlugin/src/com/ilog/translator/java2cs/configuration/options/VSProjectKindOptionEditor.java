/**
 * 
 */
package com.ilog.translator.java2cs.configuration.options;

import com.ilog.translator.java2cs.configuration.TranslatorProjectOptions.VSProjectKind;

public class VSProjectKindOptionEditor extends ComboBoxEditor<VSProjectKind> {

	String[] getListItems() {

		final VSProjectKind[] values = VSProjectKind.values();
		final String[] stringValues = new String[values.length];
		for (int i = 0; i < values.length; i++) {
			stringValues[i] = values[i].toString();
		}
		return stringValues;
	}

	@Override
	void setOptionValue(OptionImpl<VSProjectKind> option, int index) {
		option.setValue(VSProjectKind.values()[index]);
	}
}