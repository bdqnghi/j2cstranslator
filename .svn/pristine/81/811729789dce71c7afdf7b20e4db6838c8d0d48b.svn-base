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

public class ChangeUsingDescriptor implements Cloneable, Option {

	private List<String> usingToAdd = new ArrayList<String>();
	private List<String> usingToRemove = new ArrayList<String>();

	//
	// Link to a class
	//

	public ChangeUsingDescriptor() {

	}

	public ChangeUsingDescriptor(ChangeUsingDescriptor other) {
		if (other != null) {
			usingToRemove = new ArrayList<String>(other.usingToRemove);
			usingToAdd = new ArrayList<String>(other.usingToAdd);
		}
	}

	//
	//
	//

	public void addUsing(String modifier) {
		if (!usingToRemove.contains(modifier)) {
			usingToRemove.add(modifier);
		}
	}

	//
	//
	//

	// TODO: use it for 'forced' modifier
	public void removeUsing(String modifier) {
		if (!usingToAdd.contains(modifier)) {
			usingToAdd.add(modifier);
		}
	}

	//
	//
	//

	public List<String> getUsingToAdd() {
		return Collections.unmodifiableList(usingToAdd);
	}

	public void setUsingToAdd(String[] split) {
		usingToAdd = new ArrayList<String>();
		for (String modToAdd : split) {
			usingToAdd.add(modToAdd);
		}
	}

	//
	//
	//

	public List<String> getUsingToRemove() {
		return Collections.unmodifiableList(usingToRemove);
	}

	public void setUsingToRemove(String[] split) {
		usingToRemove = new ArrayList<String>();
		for (String modToAdd : split) {
			usingToRemove.add(modToAdd);
		}
	}

	//
	//
	//

	@Override
	public Object clone() {
		final ChangeUsingDescriptor obj = new ChangeUsingDescriptor();
		for (final String mod : usingToRemove) {
			obj.addUsing(mod);
		}
		for (final String mod : usingToAdd) {
			obj.removeUsing(mod);
		}
		return obj;
	}

	//
	// toXML
	//

	public void toXML(StringBuilder res, String tabValue) {
		if (usingToAdd.size() > 0 || usingToRemove.size() > 0) {
			res.append(tabValue + Constants.TAB + "<imports>");
			for (String toAdd : usingToAdd) {
				res.append(tabValue + Constants.TWOTAB + "<add name=\""
						+ toAdd + "\"");
			}
			for (String toRemove : usingToRemove) {
				res.append(tabValue + Constants.TWOTAB + "<remove name=\""
						+ toRemove + "\"");
			}
			res.append(tabValue + Constants.TAB + "</imports>");
		}
	}
	
	//
	// fromXML
	//
	public void fromXML(Element node) {
		if (node.getNodeType() == Node.ELEMENT_NODE) {
			Element elem = (Element) node;
			String nodeName = elem.getNodeName(); // must be "imports"
			if (nodeName.equals("imports")) {
				NodeList list = elem.getChildNodes();
				for (int i = 0; i < list.getLength(); i++) {
					Node child = list.item(i);
					if (child.getNodeType() == Node.ELEMENT_NODE) {
						Element childElem = (Element) node;
						String childName = child.getNodeName(); // must be "add"
																// or "remove"
						if (childName.equals("add")) {
							String keyword = childElem.getAttribute("name");						
							addUsing(keyword);
						} else if (childName.equals("remove")) {
							String keyword = childElem.getAttribute("name");							
							removeUsing(keyword);
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
