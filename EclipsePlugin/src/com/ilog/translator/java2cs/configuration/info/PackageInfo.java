package com.ilog.translator.java2cs.configuration.info;

import java.util.Collection;

import org.eclipse.core.resources.IProject;
import org.eclipse.jdt.core.IType;
import org.eclipse.jdt.core.JavaModelException;

import com.ilog.translator.java2cs.configuration.target.TargetPackage;

/**
 * Encapsulate a java package and its c# counterpart (target)
 * 
 * @author afau
 *
 */
public interface PackageInfo extends IElementInfo {

	//
	// class
	//
	
	public ClassInfo getClass(IType type);
	
	public ClassInfo getClass(String fqname) throws JavaModelException;
	
	public ClassInfo createClass(String fqname, boolean isGeneric)
	throws JavaModelException;
	
	public abstract void addClass(IType type, ClassInfo ci, boolean generics);

	public abstract Collection<ClassInfo> getAllClasses();

	public abstract ClassInfo getGenericClass(IType type)
			throws TranslationModelException;
	
	//
	// wildcard
	//
	
	public abstract boolean isWildcard();

	public abstract void setWildcard(boolean isWildcard);

	//
	// target
	//
	
	public abstract TargetPackage getTarget(String targetFramework);
	public abstract void addTarget(String targetFramework, TargetPackage pck);
	
	// toFile

	public abstract String toFile();

	// reference
	public abstract IProject getReference();

}