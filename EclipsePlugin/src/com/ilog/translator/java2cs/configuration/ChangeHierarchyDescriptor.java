package com.ilog.translator.java2cs.configuration;

import java.util.ArrayList;
import java.util.Collections;
import java.util.List;

import org.eclipse.swt.widgets.Composite;
import org.eclipse.swt.widgets.Control;
import org.w3c.dom.Element;
import org.w3c.dom.Node;
import org.w3c.dom.NodeList;

import com.ilog.translator.java2cs.configuration.info.Constants;
import com.ilog.translator.java2cs.configuration.options.Option;

public class ChangeHierarchyDescriptor implements Cloneable, Option {

	private List<String> interfacesoRemove = new ArrayList<String>();
	private List<String> interfacesToAdd = new ArrayList<String>();
	private String newSuperClass = null;

	//
	// Link to a class
	//

	public ChangeHierarchyDescriptor() {

	}

	public ChangeHierarchyDescriptor(ChangeHierarchyDescriptor other) {
		if (other != null) {
			interfacesToAdd = new ArrayList<String>(other.interfacesToAdd);
			interfacesoRemove = new ArrayList<String>(other.interfacesoRemove);
		}
	}

	//
	//
	//

	// TODO: use it for 'forced' modifier
	public void addInterface(String modifier) {
		if (!interfacesToAdd.contains(modifier)) {
			interfacesToAdd.add(modifier);
		}
	}

	//
	//
	//
	
	// TODO: use it for 'forced' modifier
	public void removeInterface(String modifier) {
		if (!interfacesoRemove.contains(modifier)) {
			interfacesoRemove.add(modifier);
		}
	}

	//
	//
	//
	
	public void setSuperClass(String name) {
		newSuperClass = name;
	}

	public String getSuperClass() {
		return newSuperClass;
	}

	//
	//
	//
	
	public List<String> getInterfaceToAdd() {
		// TODO : re-order it to put partial at the end !
		final List<String> res = new ArrayList<String>();
		for (final String mod : interfacesToAdd) {
			res.add(mod);
		}
		return Collections.unmodifiableList(res);
	}

	//
	//
	//
	
	public List<String> getInterfaceToRemove() {
		return Collections.unmodifiableList(interfacesoRemove);
	}

	//
	//
	//
	
	@Override
	public Object clone() {
		final ChangeHierarchyDescriptor obj = new ChangeHierarchyDescriptor();
		for (final String mod : interfacesToAdd) {
			obj.addInterface(mod);
		}
		for (final String mod : interfacesoRemove) {
			obj.removeInterface(mod);
		}
		obj.setSuperClass(newSuperClass);
		return obj;
	}
	
	//
	// toXML
	//
	
	public void toXML(StringBuilder res, String tabValue) {
		if (newSuperClass != null || interfacesoRemove.size() > 0 || interfacesToAdd.size() > 0) {
			res.append(tabValue + "<hierarchy");
			if (newSuperClass != null) {
				res.append(" newSupeClassName=\""+ newSuperClass + "\"");
			}
			res.append(">\n");
			if (interfacesoRemove.size() > 0 || interfacesToAdd.size() > 0) {
				res.append(tabValue + Constants.TAB + "<interfaces>");
				for(String toAdd : interfacesToAdd) {
					res.append(tabValue + Constants.TWOTAB +"<add name=\""+toAdd+"\"");
				}
				for(String toRemove : interfacesoRemove) {
					res.append(tabValue + Constants.TWOTAB +"<remove name=\""+toRemove+"\"");
				}
				res.append(tabValue + Constants.TAB +"</interfaces>");
			}
			res.append(tabValue + "</hierarchy>");
		}
	}

	//
	// fromXML
	// 
	public void fromXML(Element node) {
		if (node.getNodeType() == Node.ELEMENT_NODE) {
			Element elem = (Element) node;
			String nodeName = elem.getNodeName(); // must be "modifiers"
			if (nodeName.equals("hierarchy")) {
				NodeList list = elem.getChildNodes();
				newSuperClass = elem.getAttribute("newSupeClassName");
				for (int i = 0; i < list.getLength(); i++) {
					Node child = list.item(i);
					if (child.getNodeType() == Node.ELEMENT_NODE) {
						Element childElem = (Element) node;
						String childName = child.getNodeName(); // must be "add"
																// or "remove"
						if (childName.equals("add")) {
							String keyword = childElem.getAttribute("name");
							addInterface(keyword);
						} else if (childName.equals("remove")) {
							String keyword = childElem.getAttribute("name");
							removeInterface(keyword);
						}
					}
				}
			}
		} else {
			// ERROR
		}
	}

	public Control createUIControl(Composite parent) {
		// TODO Auto-generated method stub
		return null;
	}

	public String[] getDescription() {
		// TODO Auto-generated method stub
		return null;
	}

	public void initializeControlValue() {
		// TODO Auto-generated method stub
		
	}
}
