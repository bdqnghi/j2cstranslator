package com.ilog.translator.java2cs.configuration.info;

import java.util.ArrayList;
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
import org.w3c.dom.Element;

import sun.reflect.generics.reflectiveObjects.NotImplementedException;

import com.ilog.translator.java2cs.configuration.options.MappingOverridingPolicy;
import com.ilog.translator.java2cs.configuration.target.TargetClass;
import com.ilog.translator.java2cs.configuration.target.TargetProperty;
import com.ilog.translator.java2cs.translation.noderewriter.IndexerRewriter.IndexerKind;
import com.ilog.translator.java2cs.translation.noderewriter.PropertyRewriter.ProperyKind;

public class CompositeClassInfo implements ClassInfo {

	private List<ClassInfo> classes;
	private ClassInfo last = null;
	private int collectionSize = 0;
	private boolean mergeTargetClass = false;
	private boolean isMember = false;
	
	//
	// Represent a ClassInfo coming from more than on mapping
	// for each mapping we build a ClassInfo
	//
	
	public CompositeClassInfo(List<ClassInfo> classes) {
		this.classes = classes;
		this.collectionSize = classes.size();
		this.last = classes.get(collectionSize - 1);
	}
	
	//
	// isMember
	//
	
	public boolean isMember() {
		return isMember;
	}
	
	//
	//
	//
	
	private boolean hasChanged() {
		return collectionSize != classes.size();
	}
	
	//
	//
	//

	public MethodInfo getConstructor(IMethod method) throws JavaModelException {
		List<MethodInfo> res = new ArrayList<MethodInfo>();
		for(ClassInfo ci : classes) {
			MethodInfo mi = ci.getConstructor(method);
			if (mi != null)
				res.add(mi);
		}
		if (res.size() > 0) {
			// what return ? first or last ?
			// check jdk and .NET Framework compatibility ?
			return res.get(res.size() - 1); // last as start
		}
		return null;
	}

	public FieldInfo getField(IField field) throws JavaModelException {
		List<FieldInfo> res = new ArrayList<FieldInfo>();
		for(ClassInfo ci : classes) {
			FieldInfo fi = ci.getField(field);
			if (fi != null)
				res.add(fi);
		}
		if (res.size() > 0) {
			// what return ? first or last ?
			// check jdk and .NET Framework compatibility ?
			return res.get(res.size() - 1); // last as start
		}
		return null;
	}


	public MethodInfo getMethod(String signature, IMethod method)
			throws JavaModelException {
		List<MethodInfo> res = new ArrayList<MethodInfo>();
		for(ClassInfo ci : classes) {
			MethodInfo mi = ci.getMethod(signature, method);
			if (mi != null)
				res.add(mi);
		}
		if (res.size() > 0) {
			// what return ? first or last ?
			// check jdk and .NET Framework compatibility ?
			return res.get(res.size() - 1); // last as start
		}
		return null;
	}

	//
	// Don't know if I have to answer ....
	//

	public String getName() {
		return last.getName();
	}


	public String getPackageName() {
		return last.getName();
	}
	

	public Map<String, IndexerInfo> getIndexers() {
		return last.getIndexers();
	}
/*
	@Override
	public MethodMappingPolicy getMemberMappingBehavior() {
		return last.getMemberMappingBehavior();
	}
	*/

	public PackageInfo getPackageInfo() {
		return last.getPackageInfo();
	}


	public TargetClass getTarget(String targetFramework) {		
		TargetClass res = last.getTarget(targetFramework);
		if (hasChanged() || !mergeTargetClass) {
			if (res != null && res.getName() == null) {
				for(int i = classes.size() - 1; i >= 0; i--) {
					ClassInfo candidate = classes.get(i);
					if (candidate.getTarget(targetFramework) != null && candidate.getTarget(targetFramework).getName() != null) {
						// copy if exist mapping name/pck to the "last" class info the 
						res.setName(candidate.getTarget(targetFramework).getShortName());
						res.setPackageName(candidate.getTarget(targetFramework).getPackageName());
						return res;
					}
				}
			}
			mergeTargetClass = true;
		}
		// check jdk and .NET Framework compatibility ?
		return res;
	}


	public IType getType() {
		// TODO Auto-generated method stub
		return last.getType();
	}
	

	public boolean hasCovariantMethod() {
		// TODO Auto-generated method stub
		return last.hasCovariantMethod();
	}


	public boolean hasTargetClass() {
		// TODO Auto-generated method stub
		return last.hasTargetClass();
	}


	public void implicitFieldRename(IField ifield, String newName)
			throws JavaModelException {
		last.implicitFieldRename(ifield, newName);
	}


	public boolean isConstructor(IMethod m) {
		// TODO Auto-generated method stub
		return last.isConstructor(m);
	}
/*
	@Override
	public boolean isNullable() {
		// TODO Auto-generated method stub
		return last.isNullable();
	}

	@Override
	public boolean isProcessDoc() {
		// TODO Auto-generated method stub
		return last.isProcessDoc();
	}

	@Override
	public boolean isRemoveGenerics() {
		// TODO Auto-generated method stub
		return last.isRemoveGenerics();
	}

	@Override
	public boolean isRemoveStaticInitializers() {
		// TODO Auto-generated method stub
		return last.isRemoveStaticInitializers();
	}

	@Override
	public boolean isRemoved() {
		// TODO Auto-generated method stub
		return last.isRemoved();
	}
*/
	//
	

	public FieldInfo resolveField(String description) throws JavaModelException {
		// TODO Auto-generated method stub
		return last.resolveField(description);
	}


	public MethodInfo resolveMethod(String name, String[] classes)
			throws JavaModelException {
		return last.resolveMethod(name, classes);
	}


	public MethodInfo resolveMethod(String name, String[] classes, boolean fqn)
			throws JavaModelException {
		return last.resolveMethod(name, classes, fqn);
	}
	
	//
	

	public HashMap<String, MethodInfo> getMethodsMap() {
		throw new NotImplementedException();
	}
	

	public Map<String, PropertyInfo> getProperties() {
		return last.getProperties();
	}
	
	//
	// For sure can't do that on a composite classinfo
	//
	


	public void setExcluded(boolean isExcluded) {
		throw new NotImplementedException();
	}
	/*
	@Override
	public void setChangeHierachy(
			ChangeHierarchyDescriptor changeHierarchyDescriptor) {
		throw new NotImplementedException();
	}
*/

	public void setCovariantMethod(boolean b) {
		throw new NotImplementedException();
	}
/*
	@Override
	public void setMemberMappingBehavior(
			MethodMappingPolicy memberMappingBehavior) {
		throw new NotImplementedException();
	}

	@Override
	public void setNullable(boolean nullable) {
		throw new NotImplementedException();
	}
*/

	public void setPackageInfo(PackageInfo info) {
		throw new NotImplementedException();
	}
/*
	@Override
	public void setPartial(boolean partial) {
		throw new NotImplementedException();
	}

	@Override
	public void setProcessDoc(boolean processDoc) {
		throw new NotImplementedException();
	}

	@Override
	public void setRemoveGenerics(boolean b) {
		throw new NotImplementedException();
	}

	@Override
	public void setRemoveStaticInitializers(boolean b) {
		throw new NotImplementedException();
	}

	@Override
	public void setRemoved(boolean removed) {
		throw new NotImplementedException();
	}*/

	
	public void addTargetClass(String targetFramework, TargetClass tclazz) {
		throw new NotImplementedException();
	}


	public String toFile() {
		throw new NotImplementedException();
	}

	//
	//
	//
	

	public void addExplicitInterfaceMethods(String interf) {
		throw new NotImplementedException();
	}


	public void addIndexer(IndexerKind kind, int[] paramsIndexs,
			int valueIndex, MethodInfo info) {
		throw new NotImplementedException();
	}

	
	public void addIndexers(List<IndexerInfo> properties2) {
		throw new NotImplementedException();
	}

	
	public void addProperties(List<PropertyInfo> properties2) {
		throw new NotImplementedException();
	}

	
	public void addProperty(String targetFramework, String name, ProperyKind kind, MethodInfo info, TargetProperty prop) {
		throw new NotImplementedException();
	}

	
	public ClassInfo cloneContentFor(IType otherType, List<String> fieldsName)
			throws JavaModelException {
		return last.cloneContentFor(otherType, fieldsName);
		// throw new NotImplementedException();
	}


	public ClassInfo cloneContentFor(IType otherType, IMember[] methodsToClone)
			throws JavaModelException {
		return last.cloneContentFor(otherType, methodsToClone);
	}


	public void computeParents(IJavaProject project,
			Hashtable<String, List<ClassInfo>> allClasses) throws JavaModelException {
		throw new NotImplementedException();
	}
/*
	@Override
	public ChangeHierarchyDescriptor getChangeHierarchy() {
		throw new NotImplementedException();
	}
	*/
	//
	
	public void toXML(StringBuilder descr, String tabValue){
		throw new NotImplementedException();
	}
	
	public void fromXML(Element pckElement) {
		throw new NotImplementedException();
	}

	public MappingOverridingPolicy getMappingOverringPolicy() {
		// TODO Auto-generated method stub
		return null;
	}

	public MappingsInfo getMappingInfo() {
		// TODO Auto-generated method stub
		return null;
	}

}
