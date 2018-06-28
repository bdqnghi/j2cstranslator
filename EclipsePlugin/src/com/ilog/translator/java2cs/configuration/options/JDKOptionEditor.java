/**
 * 
 */
package com.ilog.translator.java2cs.configuration.options;


public class JDKOptionEditor extends
		ComboBoxEditor<JDK> {

	String[] getListItems() {

		final JDK[] values = JDK.values();
		final String[] stringValues = new String[values.length];
		for (int i = 0; i < values.length; i++) {
			stringValues[i] = values[i].toString();
		}
		return stringValues;
	}

	@Override
	void setOptionValue(OptionImpl<JDK> option, int index) {
		option.setValue(JDK.values()[index]);
	}
}