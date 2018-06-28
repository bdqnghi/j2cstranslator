/**
 * 
 */
package com.ilog.translator.java2cs.configuration.options;

import com.ilog.translator.java2cs.configuration.TranslatorProjectOptions.VSGenerationPolicy;

public class VSGenerationPolicyOptionEditor extends
		ComboBoxEditor<VSGenerationPolicy> {

	String[] getListItems() {

		final VSGenerationPolicy[] values = VSGenerationPolicy.values();
		final String[] stringValues = new String[values.length];
		for (int i = 0; i < values.length; i++) {
			stringValues[i] = values[i].toString();
		}
		return stringValues;
	}

	@Override
	void setOptionValue(OptionImpl<VSGenerationPolicy> option, int index) {
		option.setValue(VSGenerationPolicy.values()[index]);
	}
}