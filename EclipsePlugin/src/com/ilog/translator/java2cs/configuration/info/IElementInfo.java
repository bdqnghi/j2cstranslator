package com.ilog.translator.java2cs.configuration.info;

import com.ilog.translator.java2cs.configuration.XMLElement;

public interface IElementInfo extends XMLElement {
		
	public String toFile();
	
	/**
	 * @return Returns the name
	 */
	public String getName() ;

	/**
	 * @return Returns the name
	 */
	public String getPackageName();
	
	public MappingsInfo getMappingInfo();
	
}
