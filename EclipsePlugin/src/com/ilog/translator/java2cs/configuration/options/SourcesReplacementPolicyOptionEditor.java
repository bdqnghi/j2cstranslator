/**
 * 
 */
package com.ilog.translator.java2cs.configuration.options;

import com.ilog.translator.java2cs.configuration.TranslatorProjectOptions.SourcesReplacementPolicy;

public class SourcesReplacementPolicyOptionEditor extends
		ComboBoxEditor<SourcesReplacementPolicy> {

	String[] getListItems() {

		final SourcesReplacementPolicy[] values = SourcesReplacementPolicy
				.values();
		final String[] stringValues = new String[values.length];
		for (int i = 0; i < values.length; i++) {
			stringValues[i] = values[i].toString();
		}
		return stringValues;
	}

	@Override
	void setOptionValue(OptionImpl<SourcesReplacementPolicy> option, int index) {
		option.setValue(SourcesReplacementPolicy.values()[index]);
	}
}