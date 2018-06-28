/**
 * 
 */
package com.ilog.translator.java2cs.configuration.options;


public class MethodMappingPolicyOptionEditor extends
		ComboBoxEditor<MethodMappingPolicy> {

	String[] getListItems() {

		final MethodMappingPolicy[] values = MethodMappingPolicy.values();
		final String[] stringValues = new String[values.length];
		for (int i = 0; i < values.length; i++) {
			stringValues[i] = values[i].toString();
		}
		return stringValues;
	}

	@Override
	void setOptionValue(OptionImpl<MethodMappingPolicy> option, int index) {
		option.setValue(MethodMappingPolicy.values()[index]);
	}
}