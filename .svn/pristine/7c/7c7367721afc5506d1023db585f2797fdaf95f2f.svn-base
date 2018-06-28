package com.ilog.translator.java2cs.configuration.info;

import com.ilog.translator.java2cs.configuration.ChangeHierarchyDescriptor;
import com.ilog.translator.java2cs.configuration.ChangeModifierDescriptor;
import com.ilog.translator.java2cs.configuration.ChangeUsingDescriptor;
import com.ilog.translator.java2cs.configuration.options.JDK;
import com.ilog.translator.java2cs.configuration.options.JDKOptionBuilder;
import com.ilog.translator.java2cs.configuration.options.JDKOptionEditor;
import com.ilog.translator.java2cs.configuration.options.OptionImpl;
import com.ilog.translator.java2cs.configuration.options.StringOptionBuilder;
import com.ilog.translator.java2cs.configuration.options.StringOptionEditor;
import com.ilog.translator.java2cs.configuration.options.OptionImpl.XMLKind;
import com.ilog.translator.java2cs.util.CSharpModelUtil;

/**
 * 
 * @author afau
 * 
 *         options : - isExcluded (ElementInfo) - name (ElementInfo) -
 *         packagename (ElementInfo)
 * 
 */
public abstract class ElementInfo implements IElementInfo {

	//
	// Options
	//
	protected OptionImpl<String> name = new OptionImpl<String>("name", null,
			null, OptionImpl.Status.PRODUCTION, new StringOptionBuilder(),
			new StringOptionEditor(), XMLKind.ATTRIBUT,
			"name of this (java) element");
	protected OptionImpl<String> packageName = new OptionImpl<String>(
			"packageName", null, null, OptionImpl.Status.PRODUCTION,
			new StringOptionBuilder(), new StringOptionEditor(),
			XMLKind.ATTRIBUT, "package name of this (java) element");
	protected OptionImpl<JDK> sinceJDK = new OptionImpl<JDK>(
			"sinceJDK", null, JDK.JDK1_5, OptionImpl.Status.EXPERIMENTAL,
			new JDKOptionBuilder(),
			new JDKOptionEditor(), XMLKind.ATTRIBUT, "jdk where this element has been introduce");

	//
	//
	//
	protected MappingsInfo mappingInfo;

	//
	// constructor
	//

	public MappingsInfo getMappingInfo() {
		return mappingInfo;
	}

	protected ElementInfo(MappingsInfo mappingInfo, String fqn) {
		name.setValue(fqn);
		this.mappingInfo = mappingInfo;
		if (name != null) {
			final int id = name.getValue().lastIndexOf(
					CSharpModelUtil.CLASS_SEPARATOR);
			if (id > 0) {
				packageName.setValue(name.getValue().substring(0, id));
			}
		}
	}

	//
	// name
	//

	/**
	 * @return Returns the name
	 */
	public String getName() {
		return name.getValue();
	}

	//
	// package name
	//

	/**
	 * @return Returns the name
	 */
	public String getPackageName() {
		return packageName.getValue();
	}

	//
	// toXML
	//

	public static <T> void toXML(StringBuilder res, OptionImpl<T> option,
			String before, String after, String tabValue) {
		if (!option.isDefaultValue()) {
			res.append(before);
			option.toXML(res, tabValue);
			res.append(after);
		}
	}

	public static void toXML(StringBuilder res,
			ChangeModifierDescriptor changeModifier, String tabValue) {
		if (changeModifier != null) {
			changeModifier.toXML(res, tabValue);
		}
	}

	public static void toXML(StringBuilder res,
			ChangeHierarchyDescriptor changeModifier, String tabValue) {
		if (changeModifier != null) {
			changeModifier.toXML(res, tabValue);
		}
	}

	public static void toXML(StringBuilder res,
			ChangeUsingDescriptor changeModifier, String tabValue) {
		if (changeModifier != null) {
			changeModifier.toXML(res, tabValue);
		}
	}

	public static String printParamIndex(int[] paramsIndexs) {
		String res = "[";
		for (int i = 0; i < paramsIndexs.length; i++) {
			res += "@" + paramsIndexs[i];
			if (i < paramsIndexs.length - 1)
				res += ",";
		}
		res += "]";
		return res;
	}
}