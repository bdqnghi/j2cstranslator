package com.ilog.translator.java2cs.configuration.info;

import java.util.List;

import org.w3c.dom.Element;

import sun.reflect.generics.reflectiveObjects.NotImplementedException;

import com.ilog.translator.java2cs.configuration.options.ListOfStringOptionBuilder;
import com.ilog.translator.java2cs.configuration.options.ListOfStringOptionEditor;
import com.ilog.translator.java2cs.configuration.options.OptionImpl;
import com.ilog.translator.java2cs.configuration.options.OptionImpl.XMLKind;

/**
 * @author afau
 * 
 * Information regarding variable renaming
 * 
 */
public class VariableInfo extends ElementInfo {

	//
	// Options
	//
	private OptionImpl<List<String>> names = new OptionImpl<List<String>>("names", null, null, 
			OptionImpl.Status.PRODUCTION, new ListOfStringOptionBuilder(), 
			new ListOfStringOptionEditor(), XMLKind.ELEMENT, "Alternates name for that variable");

	//
	// constructor
	//

	protected VariableInfo(MappingsInfo mappingInfo, String name, List<String> names) {
		super(mappingInfo, name);
		this.names.setValue(names);
	}

	//
	// names
	//

	/*public void setNames(List<String> v) {
		names.setValue(v);
	}*/

	public List<String> getNames() {
		return names.getValue();
	}

	//
	// toFile
	//
	
	public String toFile() {
		return "variable " + name + " { " + printAliases() + "};\n";
	}

	private String printAliases() {
		String res = null;
		for (int i = 0; i < names.getValue().size(); i++) {
			res += names.getValue().get(i);
			if (i < names.getValue().size() - 1)
				res += ",";
		}
		return null;
	}

	//
	// toXML
	//	
	// <variable name="...">
	//    <alias name="..."/>
	// </variable>
	public void toXML(StringBuilder builder, String tabValue) {		
		builder.append(Constants.TAB + "<!--                       -->\n");
		builder.append(Constants.TAB + "<!-- variable " + name.getValue() + " -->\n");
		builder.append(Constants.TAB + "<!--                       -->\n");
		builder.append(Constants.TAB + "<variable name=\"" + name.getValue() + "\">\n");
		for (int i = 0; i < names.getValue().size(); i++) {
			String aname = names.getValue().get(i);
			builder.append(Constants.TWOTAB + "   <alias name=\""+ aname +"\"/>\n");
		}
		builder.append(Constants.TAB + "</variable>\n");
	}

	//
	// fromXML
	//
	// <variable name="...">
	//    <alias name="..."/>
	// </variable>
	public void fromXML(Element pckElement) {
		throw new NotImplementedException();
	}
}
