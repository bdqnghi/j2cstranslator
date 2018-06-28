/**
 * 
 */
package com.ilog.translator.java2cs.configuration.options;

public class ArrayOfStringOptionBuilder implements
		Builder<OptionImpl<String[]>> {
	public void build(String value, OptionImpl<String[]> option) {
		option.setValue(value.split(","));
	}

	public String createStringValue(OptionImpl<String[]> option) {
		final String[] values = option.getValue();
		final StringBuffer strBufValues = new StringBuffer();
		for (final String strValue : values) {
			strBufValues.append(strValue);
			strBufValues.append(',');
		}
		if (values.length > 0) {
			// delete the last appended ','
			strBufValues.deleteCharAt(strBufValues.length() - 1);
		}
		return strBufValues.toString();
	}
}