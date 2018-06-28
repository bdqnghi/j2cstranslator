/**
 * 
 */
package com.ilog.translator.java2cs.configuration.info;

import java.util.HashMap;

import org.w3c.dom.Element;

import sun.reflect.generics.reflectiveObjects.NotImplementedException;

import com.ilog.translator.java2cs.configuration.target.TargetProperty;
import com.ilog.translator.java2cs.translation.noderewriter.PropertyRewriter.ProperyKind;

public class PropertyInfo implements IElementInfo {

	// 
	private MethodInfo getMethod;
	private MethodInfo setMethod;
	private HashMap<String, TargetProperty> targetProperties = new HashMap<String, TargetProperty>();

	//
	// Options
	//
	private String name;
	private ProperyKind kind;

	//
	// Constructor
	//

	public PropertyInfo(String name, ProperyKind kind, MethodInfo getMethod,
			MethodInfo setMethod) {
		super();
		this.name = name;
		this.kind = kind;
		this.getMethod = getMethod;
		this.setMethod = setMethod;
	}

	public PropertyInfo(String name, ProperyKind kind, MethodInfo method) {
		super();
		this.name = name;
		this.kind = kind;
		if (kind == ProperyKind.READ)
			getMethod = method;
		else if (kind == ProperyKind.WRITE)
			setMethod = method;
	}

	//
	// addGetOrSetMethod
	//

	public void addGetOrSetMethod(ProperyKind kind, MethodInfo method) {
		if (kind == ProperyKind.READ) {
			getMethod = method;
		} else if (kind == ProperyKind.WRITE) {
			setMethod = method;
		}
		updateKind(kind);
	}

	//
	//
	//

	private void updateKind(ProperyKind kind2) {
		if (kind != kind2) {
			kind = ProperyKind.READ_WRITE;
		}
	}

	//
	// Method
	//

	public MethodInfo getGetMethod() {
		return getMethod;
	}

	public void setGetMethod(MethodInfo getMethod) {
		this.getMethod = getMethod;
	}

	//
	// Kind
	//

	public ProperyKind getKind() {
		return kind;
	}

	public void setKind(ProperyKind kind) {
		this.kind = kind;
	}

	//
	// Name
	//

	public String getName() {
		return name;
	}

	public void setName(String name) {
		this.name = name;
	}

	//
	// SetMethod
	//

	public MethodInfo getSetMethod() {
		return setMethod;
	}

	public void setSetMethod(MethodInfo setMethod) {
		this.setMethod = setMethod;
	}

	//
	// target
	//

	public TargetProperty getTarget(String targetFramework) {
		TargetProperty targetProperty = null;
		if (targetProperties.size() == 0) {
			targetProperty = new TargetProperty(name, kind);
			targetProperties.put(targetFramework, targetProperty);
		} else {
			targetProperty = targetProperties.get(targetFramework);
			if (targetProperty == null)
				return null;
		}
		targetProperty.setName(name);
		targetProperty.setKind(kind);
		
		if (kind == ProperyKind.READ_WRITE) {
			targetProperty.setGetter(getMethod);
			targetProperty.setSetter(setMethod);
		} else if (kind == ProperyKind.READ) {
			targetProperty.setGetter(getMethod);
		} else if (kind == ProperyKind.WRITE) {
			targetProperty.setSetter(setMethod);
		}

		// check jdk and framework compatibility ?
		return targetProperty;
	}

	public void addTarget(String targetFramework, TargetProperty prop) {
		targetProperties.put(targetFramework, prop);
	}

	//
	// toFile
	//

	public String toFile() {
		// TODO Auto-generated method stub
		return null;
	}

	//
	// toXML
	//

	public void toXML(StringBuilder res, String tabValue) {
		throw new NotImplementedException();
	}

	//
	// fromXML
	//

	public void fromXML(Element elem) {
		throw new NotImplementedException();
	}

	//
	// PackageName
	//

	public String getPackageName() {
		// TODO Auto-generated method stub
		return null;
	}

	//
	// excluded
	//

	public boolean isExcluded() {
		// TODO Auto-generated method stub
		return false;
	}

	public void setExcluded(boolean isExcluded) {
		// TODO Auto-generated method stub
	}

	public MappingsInfo getMappingInfo() {
		// TODO Auto-generated method stub
		return null;
	}
}