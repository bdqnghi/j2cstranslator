/**
 * 
 */
package com.ilog.translator.java2cs.configuration;

import java.io.IOException;
import java.io.Reader;
import java.io.Writer;
import java.util.Enumeration;
import java.util.PropertyResourceBundle;

public class PropertiesFileResourceProcessor implements ResourceProcessor {		
	
	public boolean process(Reader reader, Writer writer) throws IOException {
		PropertyResourceBundle bundle = new PropertyResourceBundle(reader);
		Enumeration<String> keys = bundle.getKeys();
		writer.write("<?xml version=\"1.0\" encoding=\"utf-8\"?>\n");
		writer.write("<root>\n");
		while(keys.hasMoreElements()) {
			String key = keys.nextElement();
			Object value = bundle.handleGetObject(key);
			writer.write("   <data name=\"" + key + "\" xml:space=\"preserve\">\n");
			writer.write("      <value>" + value + "</value>\n");
			writer.write("   </data>\n");
		}
		writer.write("</root>\n");
		return true;
	}
	
	public String processFilename(String filename) {
		return filename.replace(".properties", ".resx");
	}
}