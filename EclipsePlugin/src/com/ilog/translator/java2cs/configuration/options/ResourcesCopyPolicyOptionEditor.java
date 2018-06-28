/**
 * 
 */
package com.ilog.translator.java2cs.configuration.options;

import com.ilog.translator.java2cs.configuration.TranslatorProjectOptions.ResourcesCopyPolicy;

public class ResourcesCopyPolicyOptionEditor extends
		ComboBoxEditor<ResourcesCopyPolicy> {

	String[] getListItems() {

		final ResourcesCopyPolicy[] values = ResourcesCopyPolicy.values();
		final String[] stringValues = new String[values.length];
		for (int i = 0; i < values.length; i++) {
			stringValues[i] = values[i].toString();
		}
		return stringValues;
	}

	@Override
	void setOptionValue(OptionImpl<ResourcesCopyPolicy> option, int index) {
		option.setValue(ResourcesCopyPolicy.values()[index]);
	}
}