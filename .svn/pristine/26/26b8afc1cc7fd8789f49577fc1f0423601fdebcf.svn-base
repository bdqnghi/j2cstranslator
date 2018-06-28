package com.ilog.translator.java2cs.configuration.info;

import com.ilog.translator.java2cs.configuration.target.TargetField;


/**
 * Common option/behavior for member (method, field, inner class)
 * 
 */
public abstract class MemberInfo extends ElementInfo {

	protected ClassInfo classInfo;

	//
	// Constructor
	//

	protected MemberInfo(MappingsInfo mappingInfo, String name) {
		super(mappingInfo, name);
	}

	//
	// class info
	//

	
	
	/**
	 * @param cinfo
	 *            The class info to set.
	 */
	public void setClassInfo(ClassInfo cinfo) {
		this.classInfo = cinfo;
	}

	/**
	 * @return Returns the class info.
	 */
	public ClassInfo getClassInfo() {
		return classInfo;
	}
}