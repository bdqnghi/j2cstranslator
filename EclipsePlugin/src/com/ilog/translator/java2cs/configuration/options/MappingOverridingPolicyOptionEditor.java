/**
 * 
 */
package com.ilog.translator.java2cs.configuration.options;


public class MappingOverridingPolicyOptionEditor extends
		ComboBoxEditor<MappingOverridingPolicy> {

	String[] getListItems() {

		final MappingOverridingPolicy[] values = MappingOverridingPolicy.values();
		final String[] stringValues = new String[values.length];
		for (int i = 0; i < values.length; i++) {
			stringValues[i] = values[i].toString();
		}
		return stringValues;
	}

	@Override
	void setOptionValue(OptionImpl<MappingOverridingPolicy> option, int index) {
		option.setValue(MappingOverridingPolicy.values()[index]);
	}
}