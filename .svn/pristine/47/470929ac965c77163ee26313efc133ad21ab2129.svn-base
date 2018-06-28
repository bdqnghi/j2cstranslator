package com.ilog.translator.java2cs.configuration.target;

import com.ilog.translator.java2cs.configuration.options.BooleanOptionBuilder;
import com.ilog.translator.java2cs.configuration.options.BooleanOptionEditor;
import com.ilog.translator.java2cs.configuration.options.MethodMappingPolicy;
import com.ilog.translator.java2cs.configuration.options.MethodMappingPolicyOptionBuilder;
import com.ilog.translator.java2cs.configuration.options.MethodMappingPolicyOptionEditor;
import com.ilog.translator.java2cs.configuration.options.OptionImpl;
import com.ilog.translator.java2cs.configuration.options.OptionImpl.XMLKind;

/**
 * 
 * @author afau
 * 
 *         Represents a target element (i.e. a C# artifact).
 * 
 *         options : - memberMappingBehavior - removeFromImport
 *         
 * 
 */
public abstract class TargetImportableElement extends TargetElement {

	//
	// Options
	//
	protected OptionImpl<MethodMappingPolicy> memberMappingBehavior = new OptionImpl<MethodMappingPolicy>(
			"memberMappingBehavior", null, null, OptionImpl.Status.PRODUCTION,
			new MethodMappingPolicyOptionBuilder(),
			new MethodMappingPolicyOptionEditor(), XMLKind.ATTRIBUT, "");
	protected OptionImpl<Boolean> removeFromImport = new OptionImpl<Boolean>("removeFromImport", null,
			false, OptionImpl.Status.BETA, new BooleanOptionBuilder(),
			new BooleanOptionEditor(), XMLKind.ATTRIBUT, 
			"Remove all occurence of that package from imports clauses");
	
	//
	// Constructor
	//
	
	public TargetImportableElement() {
	}
	
	/**
	 * Create a new target element with given name.
	 * 
	 * @param packageName
	 *            The package name of this
	 * @param name
	 *            The name of this target.
	 */
	public TargetImportableElement(String packageName, String keyword) {
		super(null, packageName);		
	}

	//
	// MemberMappingBehavior
	//

	public void setMemberMappingBehavior(
			MethodMappingPolicy memberMappingBehavior) {
		this.memberMappingBehavior.setValue(memberMappingBehavior);
	}

	public MethodMappingPolicy getMemberMappingBehavior() {
		return memberMappingBehavior.getValue();
	}
	
	//
	// removeFromImport
	//

	public boolean isRemoveFromImport() {
		return removeFromImport.getValue();
	}

	public void setRemoveFromImport(boolean rem) {
		removeFromImport.setValue(rem);
	}
}