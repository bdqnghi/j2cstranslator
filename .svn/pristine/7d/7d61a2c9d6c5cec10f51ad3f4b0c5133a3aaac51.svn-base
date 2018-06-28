package com.ilog.translator.java2cs.configuration.info;

import java.util.HashMap;
import java.util.Hashtable;
import java.util.List;
import java.util.Map;

import org.eclipse.jdt.core.IField;
import org.eclipse.jdt.core.IJavaProject;
import org.eclipse.jdt.core.IMember;
import org.eclipse.jdt.core.IMethod;
import org.eclipse.jdt.core.IType;
import org.eclipse.jdt.core.JavaModelException;

import com.ilog.translator.java2cs.configuration.options.MappingOverridingPolicy;
import com.ilog.translator.java2cs.configuration.target.TargetClass;
import com.ilog.translator.java2cs.configuration.target.TargetProperty;
import com.ilog.translator.java2cs.translation.noderewriter.IndexerRewriter.IndexerKind;
import com.ilog.translator.java2cs.translation.noderewriter.PropertyRewriter.ProperyKind;

/**
 * 
 * @author afau
 * 
 *         Encapsulate a java class and its translation ("target" class)
 */
public interface ClassInfo extends IElementInfo {

	//
	// Type
	//

	public IType getType();

	//
	// isMember
	//
	
	public boolean isMember();
	
	//
	// Methods
	//
	
	public HashMap<String, MethodInfo> getMethodsMap();

	/**
	 * 
	 * @param signature
	 *            The signature of a method
	 * @return Returns the corresponding MethodInfo
	 */
	public MethodInfo getMethod(String signature, IMethod method)
			throws JavaModelException;

	/**
	 * Return the method info that correspond to the given signature.
	 * 
	 * @param sign
	 *            The signature of the method
	 * @param name
	 *            The name of the method
	 * @param classes
	 *            The parameters types
	 * @return Returns the MethodInfo
	 * @throws JavaModelException
	 */
	public MethodInfo resolveMethod(String name, String[] classes)
			throws JavaModelException;

	/**
	 * Return the method info that correspond to the given signature.
	 * 
	 * @param sign
	 *            The signature of the method
	 * @param name
	 *            The name of the method
	 * @param classes
	 *            The parameters types
	 * @return Returns the MethodInfo
	 * @throws JavaModelException
	 */
	public MethodInfo resolveMethod(String name, String[] classes, boolean fqn) throws JavaModelException;

	//
	// Constructors
	//
	
	public boolean isConstructor(IMethod m);

	public MethodInfo getConstructor(IMethod method) throws JavaModelException;

	//
	// Fields
	//
	
	/**
	 * Return the field info that correspond to the given name.
	 * 
	 * @param description
	 * @return Returns the field info
	 * @throws JavaModelException
	 */
	public FieldInfo resolveField(String description) throws JavaModelException;

	public FieldInfo getField(IField field) throws JavaModelException;

	//
	// target class
	//
	
	public TargetClass getTarget(String targetFramework);

	public void addTargetClass(String targetFramework, TargetClass tclazz);

	public boolean hasTargetClass();

	//
	// explicitInterfaceMethods
	//

	public void addExplicitInterfaceMethods(String interf);

	//
	// toString
	//

	@Override
	public String toString();

	//
	// compute parents
	//
	
	public void computeParents(IJavaProject project,
			Hashtable<String, List<ClassInfo>> allClasses) throws JavaModelException;

	//
	// Property
	//

	public void addProperty(String targetFramework, String name, ProperyKind kind, MethodInfo info, TargetProperty target);

	public Map<String, PropertyInfo> getProperties();

	/**
	 * Add a list of computed PropertyInfo
	 * 
	 * @param properties2
	 */
	public void addProperties(List<PropertyInfo> properties2);

	//
	// Indexers
	//

	public void addIndexer(IndexerKind kind, int[] paramsIndexs,
			int valueIndex, MethodInfo info);

	public Map<String, IndexerInfo> getIndexers();

	public void addIndexers(List<IndexerInfo> indexers);

	//
	// CovariantMethod
	//

	public void setCovariantMethod(boolean b);

	public boolean hasCovariantMethod();

	//
	// cloneContentFor
	//

	public ClassInfo cloneContentFor(IType otherType, List<String> fieldsName)
			throws JavaModelException;

	public ClassInfo cloneContentFor(IType otherType, IMember[] methodsToClone)
			throws JavaModelException;

	//
	// implicitFieldRename
	//

	public void implicitFieldRename(IField ifield, String newName)
			throws JavaModelException;

	//
	// Package Info
	//	

	public PackageInfo getPackageInfo();

	public void setPackageInfo(PackageInfo info);

	//
	// toFile
	//

	public String toFile();
	
	//
	// mapping overriding policy
	//

	public MappingOverridingPolicy getMappingOverringPolicy();
}
