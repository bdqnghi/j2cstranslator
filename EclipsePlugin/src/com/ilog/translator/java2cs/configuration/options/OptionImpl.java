/**
 * 
 */
package com.ilog.translator.java2cs.configuration.options;

import org.eclipse.swt.widgets.Composite;
import org.eclipse.swt.widgets.Control;
import org.w3c.dom.CDATASection;
import org.w3c.dom.Element;
import org.w3c.dom.Node;
import org.w3c.dom.NodeList;

import com.ilog.translator.java2cs.util.Utils;



public class OptionImpl<U> implements Option {

	public enum XMLKind {
		ELEMENT,
		ATTRIBUT,
		CDATA
	}
	
	public enum Status {
		PRODUCTION {
			@Override
			public String toString() {
				return "PRODUCTION";
			}
		},
		BETA {
			@Override
			public String toString() {
				return "BETA";
			}
		},
		EXPERIMENTAL {
			@Override
			public String toString() {
				return "EXPERIMENTAL";
			}
		},
		DEPCRECATED {
			@Override
			public String toString() {
				return "DEPCRECATED";
			}
		}
	}

	private final String name;
	private final String[] synonymes;
	private final U defaultValue;
	private U value;
	private final OptionImpl.Status status;
	private final OptionImpl.XMLKind kind;
	private final Builder<OptionImpl<U>> builder;
	private final Editor<OptionImpl<U>> editor;
	private String[] description;

	//
	//
	//
	
	public OptionImpl(String name, String[] synonymes, U defaultValue,
			OptionImpl.Status status, Builder<OptionImpl<U>> builder,
			Editor<OptionImpl<U>> editor, String... description) {
		this.name = name;
		this.defaultValue = defaultValue;
		this.synonymes = synonymes;
		this.status = status;
		this.value = defaultValue;
		this.builder = builder;
		this.editor = editor;
		this.description = description;
		this.kind = XMLKind.ELEMENT;
	}
	
	public OptionImpl(String name, String[] synonymes, U defaultValue,
			OptionImpl.Status status, Builder<OptionImpl<U>> builder,
			Editor<OptionImpl<U>> editor, XMLKind kind , String... description) {
		this.name = name;
		this.defaultValue = defaultValue;
		this.synonymes = synonymes;
		this.status = status;
		this.value = defaultValue;
		this.builder = builder;
		this.editor = editor;
		this.description = description;
		this.kind = kind;
	}

	//
	// Value
	//
	
	public void setValue(U value) {
		this.value = value;
	}

	public U getValue() {
		return this.value;
	}

	//
	// Name
	//		
	public String getName() {
		return name;
	}

	public U getDefaultValue() {
		return defaultValue;
	}

	//
	// Status
	//
	public OptionImpl.Status getStatus() {
		return status;
	}

	public boolean isDefaultValue() {
		if (value == defaultValue)
			return true;
		return (value.equals(defaultValue));
	}

	//
	// Synonymes
	//
	public String[] getSynonymes() {
		return synonymes;
	}

	//
	// parse
	//
	public void parse(String value) {
		builder.build(value, this);
	}

	//
	// StringValue
	//
	public String getStringValue() {
		return builder.createStringValue(this);
	}

	//
	// UI
	//
	public Control createUIControl(Composite parent) {
		return editor.createUIEditor(parent, this);
	}

	//
	//
	//
	public void initializeControlValue() {
		editor.initializeValue(this);
	}

	//
	// Decription
	//
	public String[] getDescription() {
		return description;
	}

	public void setDescription(String[] description) {
		this.description = description;
	}
	
	//
	// toXML
	//
	
	public void toXML(StringBuilder res, String tabValue) {	
		if (!isDefaultValue()) {
			switch(kind) {
			case ELEMENT:
				break;
			case ATTRIBUT:				
				res.append("" + name + "=\"" + Utils.xmlify(value.toString()) + "\""); 
				break;
			case CDATA:
				res.append(tabValue + "<" + name + ">\n");
				res.append("<![CDATA[");
				res.append(value.toString());
				res.append("]]>\n");
				res.append(tabValue + "</" + name + ">\n");
				break;
			}
		}
	}
	
	//
	// fromXML
	//
	
	public void fromXML(Element node) {
		switch(node.getNodeType()) {
		case Node.ELEMENT_NODE:
			if (kind == XMLKind.ATTRIBUT) {
				Element elem = (Element) node;
				String stringValue = elem.getAttribute(name);
				if (!stringValue.isEmpty())
					parse(Utils.dexmlify(stringValue));
				else {
					if (synonymes != null) {
					for(String alternateName : synonymes) {
						stringValue = elem.getAttribute(alternateName);
						if (!stringValue.isEmpty()) {
							parse(Utils.dexmlify(stringValue));
							return;
						}
					}
					}
				}
			} if (kind == XMLKind.CDATA) {
				NodeList child = node.getChildNodes();
				for (int i = 0; i < child.getLength(); i++) {
					Node pckNode = child.item(i);
					if (pckNode.getNodeName().equals(name)) {
						Element elem = (Element) pckNode;
						NodeList child2 = elem.getChildNodes();
						for (int j = 0; j < child2.getLength(); j++) {
							Node pckNode2 = child2.item(j);
							if (pckNode2.getNodeType() == Node.CDATA_SECTION_NODE) {
								CDATASection cdata = (CDATASection) pckNode2;
								parse(Utils.dexmlify(cdata.getData()));
							}
						}
					}
				}
			} else {
			}
			break;
		case Node.CDATA_SECTION_NODE:
			if (kind == XMLKind.CDATA) {
				CDATASection cdata = (CDATASection) node;
				parse(cdata.getData());
			} else {
				// Error
			}
			break;
		}
	}
}