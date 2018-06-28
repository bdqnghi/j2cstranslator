/**
 * 
 */
package com.ilog.translator.java2cs.configuration.options;


public class MethodMappingPolicyOptionBuilder implements
		Builder<OptionImpl<MethodMappingPolicy>> {
	public void build(String value, OptionImpl<MethodMappingPolicy> option) {
		if (value.equals("capitalized"))
			option.setValue(MethodMappingPolicy.CAPITALIZED);
		if (value.equals("none"))
			option.setValue(MethodMappingPolicy.NONE);
	}

	public String createStringValue(OptionImpl<MethodMappingPolicy> option) {
		return option.getValue().toString();
	}
}