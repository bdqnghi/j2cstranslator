/**
 * 
 */
package com.ilog.translator.java2cs.configuration.options;


public class MappingOverridingPolicyOptionBuilder implements
		Builder<OptionImpl<MappingOverridingPolicy>> {
	public void build(String value, OptionImpl<MappingOverridingPolicy> option) {
		if (value.equals("override"))
			option.setValue(MappingOverridingPolicy.OVERRIDE);
		if (value.equals("replace"))
			option.setValue(MappingOverridingPolicy.REPLACE);
	}

	public String createStringValue(OptionImpl<MappingOverridingPolicy> option) {
		return option.getValue().toString();
	}
}