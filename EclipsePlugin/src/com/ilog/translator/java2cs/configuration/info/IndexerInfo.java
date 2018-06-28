/**
 * 
 */
package com.ilog.translator.java2cs.configuration.info;

import java.util.HashMap;

import org.w3c.dom.Element;

import sun.reflect.generics.reflectiveObjects.NotImplementedException;

import com.ilog.translator.java2cs.configuration.options.OptionImpl;
import com.ilog.translator.java2cs.configuration.options.OptionImpl.XMLKind;
import com.ilog.translator.java2cs.configuration.target.TargetIndexer;
import com.ilog.translator.java2cs.translation.noderewriter.IndexerRewriter.IndexerKind;

public class IndexerInfo implements IElementInfo {
		
	private MethodInfo getMethod;
	private MethodInfo setMethod;
	private HashMap<String, TargetIndexer> targetIndexers = new HashMap<String, TargetIndexer>();
	
	//
	// Options
	//
	private OptionImpl<int[]> paramsIndexs = new OptionImpl<int[]>("paramsIndexs", null, null, 
			OptionImpl.Status.PRODUCTION, null, null, XMLKind.ATTRIBUT, "indexer parameters");
	private OptionImpl<Integer> valueIndex = new OptionImpl<Integer>("valueIndex", null, 0, 
			OptionImpl.Status.PRODUCTION, null, null, XMLKind.ATTRIBUT, "indexer value");
	private IndexerKind kind;

	//
	// constructors
	//
	
	public IndexerInfo(IndexerKind kind, int[] paramsIndexs, int valueIndex,
			MethodInfo method) {
		super();
		this.kind = kind;
		if (kind == IndexerKind.READ)
			getMethod = method;
		else if (kind == IndexerKind.WRITE)
			setMethod = method;
		this.paramsIndexs.setValue(paramsIndexs);
		this.valueIndex.setValue(valueIndex);
	}

	//
	// addGetOrSetMethod
	//

	public void addGetOrSetMethod(IndexerKind kind, int[] paramsIndexs,
			int valueIndex, MethodInfo method) {
		if (kind == IndexerKind.READ) {
			getMethod = method;
		} else if (kind == IndexerKind.WRITE) {
			setMethod = method;
		}
		this.paramsIndexs.setValue(paramsIndexs);
		this.valueIndex.setValue(valueIndex);
		updateKind(kind);
	}

	//
	// Kind
	//

	private void updateKind(IndexerKind kind2) {
		if (kind != kind2) {
			kind = IndexerKind.READ_WRITE;
		}
	}

	public IndexerKind getKind() {
		return kind;
	}
	
	//
	// method
	//
	
	public MethodInfo getGetMethod() {
		return getMethod;
	}

	public void setGetMethod(MethodInfo getMethod) {
		this.getMethod = getMethod;
	}

	public MethodInfo getSetMethod() {
		return setMethod;
	}

	public void setSetMethod(MethodInfo setMethod) {
		this.setMethod = setMethod;
	}

	//
	// target
	//

	public TargetIndexer getTarget(String targetFramework) {
		if (targetIndexers.size() == 0) {
			TargetIndexer targetIndexer = new TargetIndexer(kind, paramsIndexs.getValue(), valueIndex.getValue());
			if (kind == IndexerKind.READ_WRITE) {
				targetIndexer.setGetter(getMethod);
				targetIndexer.setSetter(setMethod);
			} else if (kind == IndexerKind.READ) {
				targetIndexer.setGetter(getMethod);
			} else if (kind == IndexerKind.WRITE) {
				targetIndexer.setSetter(setMethod);
			}			
			targetIndexers.put(targetFramework, targetIndexer);
		}
		// check jdk and framework compatibility ?
		return targetIndexers.get(targetFramework);
	}

	public void addTarget(String targetFramework, TargetIndexer prop) {
		targetIndexers.put(targetFramework, prop);
	}

	//
	// toFile
	//
	
	public String toFile() {		
		throw new NotImplementedException();
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
	// name
	//

	public String getName() {		
		return null;
	}

	//
	// package name
	//
	
	public String getPackageName() {		
		return null;
	}

	//
	// excluded
	//
	
	public boolean isExcluded() {
		// TODO: ??
		return false;
	}

	public void setExcluded(boolean isExcluded) {
		// TODO: ??
	}

	public MappingsInfo getMappingInfo() {
		// TODO Auto-generated method stub
		return null;
	}
}