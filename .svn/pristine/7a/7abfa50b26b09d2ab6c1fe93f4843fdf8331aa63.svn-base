/**
 * 
 */
package com.ilog.translator.java2cs.configuration.options;


public class PackageMappingPolicyOptionEditor extends
		ComboBoxEditor<PackageMappingPolicy> {

	String[] getListItems() {

		final PackageMappingPolicy[] values = PackageMappingPolicy.values();
		final String[] stringValues = new String[values.length];
		for (int i = 0; i < values.length; i++) {
			stringValues[i] = values[i].toString();
		}
		return stringValues;
	}

	@Override
	void setOptionValue(OptionImpl<PackageMappingPolicy> option, int index) {
		option.setValue(PackageMappingPolicy.values()[index]);
	}
}