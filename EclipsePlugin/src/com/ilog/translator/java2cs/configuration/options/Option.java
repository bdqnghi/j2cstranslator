/**
 * 
 */
package com.ilog.translator.java2cs.configuration.options;

import org.eclipse.swt.widgets.Composite;
import org.eclipse.swt.widgets.Control;

import com.ilog.translator.java2cs.configuration.XMLElement;

/**
 * 
 * @author afau
 *
 */
public interface Option extends XMLElement {

	//
	// UI
	//
	public Control createUIControl(Composite parent);

	//
	//
	//
	public void initializeControlValue();

	//
	// Description
	//
	public String[] getDescription() ;
	
}