package com.ilog.translator.java2cs.configuration.info;

import java.util.ArrayList;
import java.util.Collection;
import java.util.List;

import org.eclipse.core.resources.IProject;
import org.eclipse.jdt.core.IType;
import org.eclipse.jdt.core.JavaModelException;
import org.w3c.dom.Element;

import sun.reflect.generics.reflectiveObjects.NotImplementedException;

import com.ilog.translator.java2cs.configuration.options.MappingOverridingPolicy;
import com.ilog.translator.java2cs.configuration.options.MethodMappingPolicy;
import com.ilog.translator.java2cs.configuration.options.PackageMappingPolicy;
import com.ilog.translator.java2cs.configuration.target.TargetPackage;

public class CompositePackageInfo implements PackageInfo {
	
	private List<PackageInfo> packages;
	private PackageInfo last = null;
	private PackageInfo targetSelected = null;
	private int collectionSize = 0;
	private boolean mergeTargetPackage = false;
	
	//
	//
	//
	
	public CompositePackageInfo(List<PackageInfo> packages) {
		this.packages = packages;
		this.collectionSize = packages.size();
		this.last = packages.get(collectionSize - 1);
	}
	
	//
	//
	//


	private boolean hasChanged() {
		return collectionSize != packages.size();
	}
	
	//
	//
	//
	
	public ClassInfo getClass(IType type) {
		List<ClassInfo> res = new ArrayList<ClassInfo>();
		for(PackageInfo pi : packages) {
			ClassInfo ci = pi.getClass(type);
			if (ci != null) {
				if (ci.getMappingOverringPolicy() == MappingOverridingPolicy.REPLACE)
					return res.get(0); // All old mappings are lost !
				else
					res.add(ci);
			}
		}
		if (res.size() == 1)
			return res.get(0);
		if (res.size() > 0) {
			// what return ? first or last ?
			return new CompositeClassInfo(res);
		}
		return null;
	}

	public ClassInfo getClass(String fqname) throws JavaModelException {
		List<ClassInfo> res = new ArrayList<ClassInfo>();
		for(PackageInfo pi : packages) {
			ClassInfo ci = pi.getClass(fqname);
			if (ci != null) {
				if (ci.getMappingOverringPolicy() == MappingOverridingPolicy.REPLACE)
					return res.get(0); // All old mappings are lost !
				else 
					res.add(ci);
			}
		}
		if (res.size() == 1)
			return res.get(0);
		if (res.size() > 0) {
			// what return ? first or last ?
			return new CompositeClassInfo(res);
		}
		return null;
	}

	
	public ClassInfo getGenericClass(IType type)
			throws TranslationModelException {
		List<ClassInfo> res = new ArrayList<ClassInfo>();
		for(PackageInfo pi : packages) {
			ClassInfo ci = pi.getGenericClass(type);
			if (ci != null) {
				if (ci.getMappingOverringPolicy() == MappingOverridingPolicy.REPLACE)
					return res.get(0); // All old mappings are lost !
				else
					res.add(ci);
			}
		}
		if (res.size() == 1)
			return res.get(0);
		if (res.size() > 0) {
			// what return ? first or last ?
			return new CompositeClassInfo(res);
		}
		return null;
	}
	

	//
	//
	//	


	public Collection<ClassInfo> getAllClasses() {
		return last.getAllClasses();
	}
/*
	@Override
	public MethodMappingPolicy getMemberMappingBehavior() {
		return last.getMemberMappingBehavior();
	}

	@Override
	public PackageMappingPolicy getPackageMappingBehavior() {
		return last.getPackageMappingBehavior();
	}
*/

	public IProject getReference() {
		return last.getReference();
	}


	public TargetPackage getTarget(String targetFramework) {
		// return last.getTarget();
		//		
		if (hasChanged() || targetSelected == null) {
			TargetPackage res = last.getTarget(targetFramework);		
			if (res != null && res.getName() == null) {
				for(int i = packages.size() - 1; i >= 0; i--) {
					PackageInfo candidate = packages.get(i);
					if (candidate.getTarget(targetFramework) != null && candidate.getTarget(targetFramework).getName() != null) {
						res.setName(candidate.getTarget(targetFramework).getName());
						res.setPackageName(candidate.getTarget(targetFramework).getPackageName());
						targetSelected = candidate;
						return res;
					}
				}
			} else if (res == null) {
				for(int i = packages.size() - 1; i >= 0; i--) {
					PackageInfo candidate = packages.get(i);
					if (candidate.getTarget(targetFramework) != null && candidate.getTarget(targetFramework).getName() != null) {
						targetSelected = candidate;
						return candidate.getTarget(targetFramework);
					}
				}
			} else {			
				targetSelected = last;
				return res;
			}
			mergeTargetPackage = true;			
		} 
		if (targetSelected != null)
			return targetSelected.getTarget(targetFramework);
		else
			return null;
	}


	public boolean isWildcard() {
		return last.isWildcard();
	}


	public String getName() {
		return last.getName();
	}


	public String getPackageName() {
		return last.getPackageName();
	}


	/*public boolean isExcluded() {
		return last.isExcluded();
	}*/
	
	//
	// Mutable
	//
	

	public void addClass(IType type, ClassInfo ci, boolean generics) {
		last.addClass(type, ci,generics);
	}

	public ClassInfo createClass(String fqname, boolean isGeneric)
			throws JavaModelException {
		return last.createClass(fqname, isGeneric);
	}
	
	//
	//
	//
	
	public void setMemberMappingBehavior(
			MethodMappingPolicy memberMappingBehavior) {
		throw new NotImplementedException();
	}

	public void setPackageMappingBehavior(
			PackageMappingPolicy packageMappingBehavior) {
		throw new NotImplementedException();
	}


	public void addTarget(String targetFramework, TargetPackage pck) {
		throw new NotImplementedException();
	}


	public void setWildcard(boolean isWildcard) {
		throw new NotImplementedException();
	}


	public void setExcluded(boolean isExcluded) {
		throw new NotImplementedException();
	}

	
	//
	//
	//
	

	public String toFile() {
		throw new NotImplementedException();
	}

	public void toXML(StringBuilder descr, String tabValue){
		throw new NotImplementedException();
	}
	
	public void fromXML(Element pckElement){
		throw new NotImplementedException();
	}

	public MappingsInfo getMappingInfo() {
		// TODO Auto-generated method stub
		return null;
	}
}
