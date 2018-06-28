package com.ilog.translator.java2cs.translation.util;

import java.util.HashMap;
import java.util.Map;

import com.ilog.translator.java2cs.configuration.target.TargetClass;

/**
 * @author nrrg
 *
 */
public class InterfaceRenamingUtil {

	private static Map<String,String> renamedInterfaces;
	
	public static void resetRenaming(){
		renamedInterfaces = new HashMap<String,String>();
	}
	
	public static void saveRenaming(String oldName, String newName){
		if(renamedInterfaces == null) resetRenaming();
		renamedInterfaces.put(TargetClass.shortClassName(newName), TargetClass.shortClassName(oldName));
	}
	
	public static String replaceInterfaceRenamed(String toReplace){
		if(renamedInterfaces == null)
			return toReplace;
		String newValue = toReplace;
		for(String key: renamedInterfaces.keySet()){
			newValue = newValue.replace(key, renamedInterfaces.get(key));
		}
		return newValue;
	}
}
