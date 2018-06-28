package com.ilog.translator.java2cs.configuration.info;

import java.util.HashMap;

import org.eclipse.jdt.core.IField;
import org.w3c.dom.Element;
import org.w3c.dom.Node;
import org.w3c.dom.NodeList;

import com.ilog.translator.java2cs.configuration.options.DotNetFramework;
import com.ilog.translator.java2cs.configuration.target.TargetField;

/**
 * 
 * @author afau
 * 
 * Encapsulate a java field and its translation (target field)
 * 
 * options :
 * - modifiers (from parser)
 * - name (from parser)
 * - pattern (from parser)
 * - generation (from parser)
 * 
 */
public class FieldInfo extends MemberInfo implements Cloneable {
	
	private HashMap<String, TargetField> targetFields = new HashMap<String, TargetField>();
	
	//
	// constructor
	//

	public FieldInfo(MappingsInfo mappingInfo, String name) {
		super(mappingInfo, name);
	}

	/**
	 * Create a new FieldInfo that encapsulate the given field.
	 * 
	 * @param field
	 *            the field of this FieldInfo.
	 */
	protected FieldInfo(MappingsInfo mappingInfo, IField f) {
		super(mappingInfo, f.getElementName());
	}

	//
	// target field
	//

	/**
	 * 
	 * @return true if that FieldInfo has a target field.
	 */
	public boolean hasTargetField() {
		return (targetFields != null && targetFields.size() > 0);
	}

	public TargetField getTarget(String targetFramework) {
		if (targetFields == null && targetFields.size() > 0) {
			TargetField targetField = new TargetField();
			targetFields.put(targetFramework, targetField);
			return targetField;
		}
		// check jdk and framework compatibility ?
		return targetFields.get(targetFramework);
	}

	public void addTargetField(String targetFramework, TargetField field) {
		targetFields.put(targetFramework, field);
	}

	//
	// toString
	//

	@Override
	public String toString() {
		String descr = "";
		for(String key : targetFields.keySet()) {
			TargetField tf = targetFields.get(key);
			descr += name + " :: " + tf;
		}
		return descr;
	}

	//
	// clone
	//

	@Override
	public Object clone() {
		final FieldInfo copy = new FieldInfo(mappingInfo, name.getValue());		
		for(String key : targetFields.keySet()) {
			TargetField tf = targetFields.get(key);
			copy.addTargetField(key, tf);
		}
		return copy;
	}

	//
	// toFile
	//
	
	public String toFile() {
		String descr = name + " { ";
		for(TargetField tf : targetFields.values()) {
			if (tf.getName() != null) {
				// targetField = new TargetField();
				descr += " name = " + tf.getName() + ";";
			} else if (tf.getPattern() != null) {
				descr += " pattern = " + tf.getPattern() + ";";
			}
			if (tf.getChangeModifierDescriptor() != null) {
				descr += " modifiers = " + tf.getChangeModifierDescriptor()
						+ ";";
			}
		}
		descr += "}";
		return descr;
	}

	//
	// toXML
	//
	// <field name="..." isExcluded="true|false">
	//    <target ... />
	// </field>
	public void toXML(StringBuilder descr, String tabValue) {
		descr.append(Constants.THREETAB + "<!--                    -->\n");
		descr.append(Constants.THREETAB + "<!-- Field " + name.getValue() + " -->\n");
		descr.append(Constants.THREETAB + "<!--                    -->\n");		
		descr.append(Constants.THREETAB + "<field name=\"" + name.getValue() + "\"");	
		toXML(descr, sinceJDK, "\n" + Constants.FOURTAB + "       ", "", "");
		
		if (targetFields != null && targetFields.size() > 0) {
			for(TargetField tf : targetFields.values())
				tf.toXML(descr, tabValue);
		} else {
			descr.append(">\n");    
		}
		descr.append(Constants.THREETAB + "</field>"); 
	}

	//
	// fromXML
	//
	// <field name="..." isExcluded="true|false">
	//    <target ... />
	// </field>
	public void fromXML(Element pack) {
		NodeList child = pack.getChildNodes();
		
		sinceJDK.fromXML(pack);
		
		for(int i = 0; i < child.getLength(); i++) {
			Node pckNode = child.item(i);
			
			if (pckNode.getNodeType() == Node.ELEMENT_NODE) {
				Element pckElement = (Element) pckNode;
				String elemName = pckElement.getNodeName();		
				String targetFramework = pckElement.getAttribute("dotnetFramework");
				if (targetFramework == null || targetFramework.isEmpty())
					targetFramework = DotNetFramework.NET3_5.name();
				if (elemName.equals("target")) {
					TargetField targetField = new TargetField();
					targetField.fromXML(pckElement);
					targetFields.put(targetFramework, targetField);
				} 
			}
		}
	}
}