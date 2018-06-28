package com.ilog.translator.java2cs.configuration;

import java.util.ArrayList;
import java.util.Collections;
import java.util.HashMap;
import java.util.List;

import org.eclipse.jdt.core.dom.Modifier;
import org.eclipse.swt.widgets.Composite;
import org.eclipse.swt.widgets.Control;
import org.w3c.dom.Element;
import org.w3c.dom.Node;
import org.w3c.dom.NodeList;

import com.ilog.translator.java2cs.configuration.info.Constants;
import com.ilog.translator.java2cs.configuration.options.Option;

public class ChangeModifierDescriptor implements Cloneable, Option {

	private static final HashMap<String, ChangeModifierDescriptor> PRESETS = new HashMap<String, ChangeModifierDescriptor>();
	
	public static final ChangeModifierDescriptor CHANGE_PRIVATE_TO_PUBLIC = new ChangeModifierDescriptor("CHANGE_PRIVATE_TO_PUBLIC") {
		{
			remove(DotNetModifier.PRIVATE);
			add(DotNetModifier.PUBLIC);
		}
	};
	
	public static final ChangeModifierDescriptor CHANGE_PRIVATE_TO_INTERNAL = new ChangeModifierDescriptor("CHANGE_PRIVATE_TO_INTERNAL") {
		{
			remove(DotNetModifier.PRIVATE);
			add(DotNetModifier.INTERNAL);
		}
	};
	
	public static final ChangeModifierDescriptor CHANGE_INTERNAL_TO_PUBLIC = new ChangeModifierDescriptor("CHANGE_INTERNAL_TO_PUBLIC") {
		{
			remove(DotNetModifier.INTERNAL);
			add(DotNetModifier.PUBLIC);
		}
	};
	
	public static final ChangeModifierDescriptor CHANGE_PROTECTED_TO_PUBLIC = new ChangeModifierDescriptor("CHANGE_PROTECED_TO_PUBLIC") {
		{
			remove(DotNetModifier.PROTECTED);
			add(DotNetModifier.PUBLIC);
		}
	};

	public static final ChangeModifierDescriptor REMOVE_PUBLIC = new ChangeModifierDescriptor("REMOVE_PUBLIC") {
		{
			remove(DotNetModifier.PUBLIC);
		}
	};

	public static final ChangeModifierDescriptor REMOVE_PUBLIC_AND_ABSTRACT = new ChangeModifierDescriptor("REMOVE_PUBLIC_AND_ABSTRACT") {
		{
			remove(DotNetModifier.PUBLIC);
			remove(DotNetModifier.ABSTRACT);
		}
	};

	public static final ChangeModifierDescriptor REMOVE_PRIVATE = new ChangeModifierDescriptor("REMOVE_PRIVATE") {
		{
			remove(DotNetModifier.PRIVATE);
		}
	};

	public static final ChangeModifierDescriptor REMOVE_STATIC = new ChangeModifierDescriptor("REMOVE_STATIC") {
		{
			remove(DotNetModifier.STATIC);
		}
	};

	public static final ChangeModifierDescriptor ADD_STATIC = new ChangeModifierDescriptor("ADD_STATIC") {
		{
			add(DotNetModifier.STATIC);
		}
	};

	//
	//
	//

	private List<DotNetModifier> toRemove = new ArrayList<DotNetModifier>();
	private List<DotNetModifier> toAdd = new ArrayList<DotNetModifier>();
	private String presetName;
	
	//
	//
	//

	public ChangeModifierDescriptor() {
		/*PRESETS.put(CHANGE_PRIVATE_TO_PUBLIC.getPresetName(),CHANGE_PRIVATE_TO_PUBLIC);
		PRESETS.put(REMOVE_PUBLIC.getPresetName(),REMOVE_PUBLIC);
		PRESETS.put(REMOVE_PUBLIC_AND_ABSTRACT.getPresetName(),REMOVE_PUBLIC_AND_ABSTRACT);
		PRESETS.put(REMOVE_PRIVATE.getPresetName(),REMOVE_PRIVATE);
		PRESETS.put(REMOVE_STATIC.getPresetName(),REMOVE_STATIC);
		PRESETS.put(ADD_STATIC.getPresetName(),ADD_STATIC);*/
	}
	
	public ChangeModifierDescriptor(String presetName) {		
		this.presetName = presetName;
		PRESETS.put(presetName, this);
	}

	public ChangeModifierDescriptor(ChangeModifierDescriptor other) {
		this();
		if (other != null) {
			toAdd = new ArrayList<DotNetModifier>(other.toAdd);
			toRemove = new ArrayList<DotNetModifier>(other.toRemove);
		}
	}

	//
	//
	//

	public String getPresetName() {
		return presetName;
	}
	
	// 
	// add
	//
	public void add(DotNetModifier modifier) {
		if (!toAdd.contains(modifier)) {
			toAdd.add(modifier);
		}
	}
	
	public void add(ChangeModifierDescriptor descriptor) {
		toAdd.addAll(descriptor.getModifiersToAdd());
		toRemove.addAll(descriptor.getModifiersToRemove());
	}

	// 
	// remove
	//
	public void remove(DotNetModifier modifier) {
		if (!toRemove.contains(modifier)) {
			toRemove.add(modifier);
		}
	}

	//
	// remove from add
	//
	public void removeFromAddList(DotNetModifier modifier) {
		if (toAdd.contains(modifier)) {
			toAdd.remove(modifier);
		}
	}

	//
	// replace
	//
	public void replace(DotNetModifier oldModifier, DotNetModifier newModifier) {
		remove(oldModifier);
		add(newModifier);
	}

	//
	// ignore
	// 

	public boolean ignore(Modifier m) {
		if (m.isTransient() || m.isSynchronized() || m.isStrictfp()) {
			return true;
		}
		return false;
	}

	//
	// toRemove
	//

	public boolean toRemove(Modifier m) {
		if (ignore(m)) {
			return true;
		} else {
			final int flag = m.getKeyword().toFlagValue();
			for (final DotNetModifier modifier : toRemove) {
				if (flag == modifier.getKind()) {
					return true;
				}
			}
			return false;
		}
	}

	//
	// modifiers to add
	//

	public List<DotNetModifier> getModifiersToAdd() {
		// TODO : re-order it to put partial at the end !
		final List<DotNetModifier> res = new ArrayList<DotNetModifier>();
		if (toAdd.contains(DotNetModifier.PARTIAL)
				&& toAdd.contains(DotNetModifier.INTERNAL)) {
			res.add(DotNetModifier.INTERNAL);
			res.add(DotNetModifier.PARTIAL);
		} else if (toAdd.contains(DotNetModifier.PARTIAL))
			res.add(DotNetModifier.PARTIAL);
		else if (toAdd.contains(DotNetModifier.INTERNAL))
			res.add(DotNetModifier.INTERNAL);
		for (final DotNetModifier mod : toAdd) {
			if (mod != DotNetModifier.PARTIAL && mod != DotNetModifier.INTERNAL
					&& mod != DotNetModifier.CONST)
				res.add(mod);
		}
		// Just to be sure that is the last ...
		if (toAdd.contains(DotNetModifier.CONST))
			res.add(DotNetModifier.CONST);
		return Collections.unmodifiableList(res);
	}

	//
	// modifiers to remove
	//

	public List<DotNetModifier> getModifiersToRemove() {
		return Collections.unmodifiableList(toRemove);
	}

	//
	// Clone
	//

	@Override
	public Object clone() {
		final ChangeModifierDescriptor obj = new ChangeModifierDescriptor();
		for (final DotNetModifier mod : toAdd) {
			if ((mod != DotNetModifier.ABSTRACT)
					&& (mod != DotNetModifier.OVERRIDE)
					&& (mod != DotNetModifier.VIRTUAL)) {
				obj.add(mod);
			}
		}
		for (final DotNetModifier mod : toRemove) {
			obj.remove(mod);
		}
		return obj;
	}

	//
	//
	//

	public void toXML(StringBuilder res, String tabValue) {
		if (getModifiersToAdd().size() > 0 || getModifiersToRemove().size() > 0) {
			res.append(tabValue + "<modifiers>\n");
			List<DotNetModifier> toAdd = getModifiersToAdd();
			for (DotNetModifier mod : toAdd) {
				res.append(tabValue + Constants.TAB + "<add value=\""
						+ mod.getKeyword() + "\"/>\n");
			}
			List<DotNetModifier> toRemove = getModifiersToRemove();
			for (DotNetModifier mod : toRemove) {
				res.append(tabValue + Constants.TAB + "<remove value=\""
						+ mod.getKeyword() + "\"/>\n");
			}
			res.append(tabValue + "</modifiers>\n");
		}		
	}

	public void fromXML(Element node) {
		if (node.getNodeType() == Node.ELEMENT_NODE) {
			Element elem = (Element) node;
			String nodeName = elem.getNodeName(); // must be "modifiers"
			if (nodeName.equals("modifiers")) {
				String presetName = elem.getAttribute("preset");
				if (!presetName.isEmpty()) {
					ChangeModifierDescriptor desc = PRESETS.get(presetName);
					if (desc != null)
						add(desc);			
					// else
					// ERROR !	
				} else {
				NodeList list = elem.getChildNodes();
				for (int i = 0; i < list.getLength(); i++) {
					Node child = list.item(i);
					if (child.getNodeType() == Node.ELEMENT_NODE) {
						Element childElem = (Element) child;
						String childName = child.getNodeName(); // must be "add"
																// or "remove"
						if (childName.equals("add")) {
							String keyword = childElem.getAttribute("value");
							DotNetModifier mod = DotNetModifier
									.fromKeyword(keyword);
							add(mod);
						} else if (childName.equals("remove")) {
							String keyword = childElem.getAttribute("value");
							DotNetModifier mod = DotNetModifier
									.fromKeyword(keyword);
							remove(mod);
						}
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
