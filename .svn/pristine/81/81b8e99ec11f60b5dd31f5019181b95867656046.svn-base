/**
 * 
 */
package com.ilog.translator.java2cs.configuration.options;


public class DotNetFrameworkOptionBuilder implements
		Builder<OptionImpl<DotNetFramework>> {
	public void build(String value, OptionImpl<DotNetFramework> option) {
		if (value.equals("net2"))
			option.setValue(DotNetFramework.NET2);
		else if (value.equals("net3"))
			option.setValue(DotNetFramework.NET3);
		else if (value.equals("net3_5"))
			option.setValue(DotNetFramework.NET3_5);
		else if (value.equals("net4"))
			option.setValue(DotNetFramework.NET4);
	}

	public String createStringValue(OptionImpl<DotNetFramework> option) {
		return option.getValue().toString();
	}
}