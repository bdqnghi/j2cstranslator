/**
 * 
 */
package com.ilog.translator.java2cs.configuration.options;


public class PackageMappingPolicyOptionBuilder implements
		Builder<OptionImpl<PackageMappingPolicy>> {
	public void build(String value, OptionImpl<PackageMappingPolicy> option) {
		if (value.equals("capitalized"))
			option.setValue(PackageMappingPolicy.CAPITALIZED);
		if (value.equals("none"))
			option.setValue(PackageMappingPolicy.NONE);
	}

	public String createStringValue(OptionImpl<PackageMappingPolicy> option) {
		return option.getValue().toString();
	}
}