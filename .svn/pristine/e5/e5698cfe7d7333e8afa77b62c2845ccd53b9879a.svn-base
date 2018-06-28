/**
 * 
 */
package com.ilog.translator.java2cs.configuration.options;

public class BooleanOptionBuilder implements Builder<OptionImpl<Boolean>> {
	
	public void build(String value, OptionImpl<Boolean> defaultValue) {
		defaultValue.setValue(Boolean.parseBoolean(value));
	}

	public String createStringValue(OptionImpl<Boolean> option) {
		return option.getValue().toString();
	}
}