/**
 * 
 */
package com.ilog.translator.java2cs.configuration.options;


public class DotNetFrameworkOptionEditor extends
		ComboBoxEditor<DotNetFramework> {

	String[] getListItems() {

		final DotNetFramework[] values = DotNetFramework.values();
		final String[] stringValues = new String[values.length];
		for (int i = 0; i < values.length; i++) {
			stringValues[i] = values[i].toString();
		}
		return stringValues;
	}

	@Override
	void setOptionValue(OptionImpl<DotNetFramework> option, int index) {
		option.setValue(DotNetFramework.values()[index]);
	}
}