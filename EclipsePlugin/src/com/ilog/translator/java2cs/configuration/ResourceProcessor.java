/**
 * 
 */
package com.ilog.translator.java2cs.configuration;

import java.io.IOException;
import java.io.Reader;
import java.io.Writer;

public interface ResourceProcessor {	
	
	public boolean process(Reader reader, Writer writer) throws IOException;
	
	public String processFilename(String filename);
	
}