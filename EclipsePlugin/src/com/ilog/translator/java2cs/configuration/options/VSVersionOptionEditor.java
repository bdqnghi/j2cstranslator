/**
 * 
 */
package com.ilog.translator.java2cs.configuration.options;

import com.ilog.translator.java2cs.configuration.TranslatorProjectOptions.VSVersion;

public class VSVersionOptionEditor extends ComboBoxEditor<VSVersion> {

	String[] getListItems() {

		final VSVersion[] values = VSVersion.values();
		final String[] stringValues = new String[values.length];
		for (int i = 0; i < values.length; i++) {
			stringValues[i] = values[i].toString();
		}
		return stringValues;
	}

	@Override
	void setOptionValue(OptionImpl<VSVersion> option, int index) {
		option.setValue(VSVersion.values()[index]);
	}
}