package com.ilog.translator.java2cs.configuration.target;

import com.ilog.translator.java2cs.configuration.options.OptionImpl;
import com.ilog.translator.java2cs.configuration.options.StringOptionBuilder;
import com.ilog.translator.java2cs.configuration.options.StringOptionEditor;
import com.ilog.translator.java2cs.configuration.options.OptionImpl.XMLKind;

/**
 * 
 * @author afau
 * 
 * Represents a 'target field' i.e. the description (package & name) of a field
 * in a C# classes.
 * 
 * options :
 *    - format
 *    
 */
public abstract class TargetMemberElement extends TargetElement {

	//
	// Options
	//
	protected OptionImpl<String> pattern = new OptionImpl<String>("format", null, null, 
			OptionImpl.Status.PRODUCTION, new StringOptionBuilder(), 
			new StringOptionEditor(), XMLKind.CDATA, "");
	protected OptionImpl<String> type = new OptionImpl<String>("type", null, null,
			OptionImpl.Status.BETA, new StringOptionBuilder(),
			new StringOptionEditor(), XMLKind.ATTRIBUT, "name of this new type for this field");

	//
	// constructors
	//
	
	/**
	 * 
	 */
	public TargetMemberElement() {
		super(null, null);
	}
	
	/**
	 * 
	 * @param packageName
	 * @param name
	 */
	protected TargetMemberElement(String packageName, String name) {
		super(null, null);
	}
	
	/**
	 * Create a new target field with given name.
	 * 
	 * @param name
	 *            The name of this target field.
	 */
	public TargetMemberElement(String fname) {
		super(null, fname);
	}

	public TargetMemberElement(int dummy, String pattern) {
		super(null, null /* was: pattern*/ );
	}

	//
	// Pattern
	//

	public String getPattern() {
		return pattern.getValue();
	}
	
	public OptionImpl<String> getPatternOption() {
		return pattern;
	}
	
	//
	// Return Type
	//
	public String getReturnType() {
		return type.getValue();
	}
	
	public void setReturnType(String rType) {
		type.setValue(rType);
	}
	
};
