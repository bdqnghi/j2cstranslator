package com.ilog.translator.java2cs.configuration;

import org.w3c.dom.Element;


public interface XMLElement {
	public void toXML(StringBuilder descr, String tabValue);
	
	public void fromXML(Element pckElement);
}
