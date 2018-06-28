/**
 * 
 */
package com.ilog.translator.java2cs.configuration.options;

import java.util.ArrayList;
import java.util.List;

public class ListOfStringOptionBuilder implements
		Builder<OptionImpl<List<String>>> {
	public void build(String value, OptionImpl<List<String>> option) {
		List<String> list = new ArrayList<String>();
		for(String s : value.split(","))
			list.add(s);
		option.setValue(list);
	}

	public String createStringValue(OptionImpl<List<String>> option) {
		final List<String> values = option.getValue();
		final StringBuffer strBufValues = new StringBuffer();
		for (final String strValue : values) {
			strBufValues.append(strValue);
			strBufValues.append(',');
		}
		if (values.size() > 0) {
			// delete the last appended ','
			strBufValues.deleteCharAt(strBufValues.length() - 1);
		}
		return strBufValues.toString();
	}
}