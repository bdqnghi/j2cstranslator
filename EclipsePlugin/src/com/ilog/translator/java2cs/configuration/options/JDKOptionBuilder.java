/**
 * 
 */
package com.ilog.translator.java2cs.configuration.options;


public class JDKOptionBuilder implements
		Builder<OptionImpl<JDK>> {
	public void build(String value, OptionImpl<JDK> option) {
		if (value.equals("jdk1_4"))
			option.setValue(JDK.JDK1_4);
		else if (value.equals("jdk1_5") || value.equals("jdk5"))
			option.setValue(JDK.JDK1_5);
		else if (value.equals("jdk6")|| value.equals("jdk1_6"))
			option.setValue(JDK.JDK6);
		else if (value.equals("jdk7")|| value.equals("jdk1_7"))
			option.setValue(JDK.JDK7);
	}

	public String createStringValue(OptionImpl<JDK> option) {
		return option.getValue().toString();
	}
}