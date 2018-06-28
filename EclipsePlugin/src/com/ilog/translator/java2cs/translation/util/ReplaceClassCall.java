package com.ilog.translator.java2cs.translation.util;

import java.util.HashMap;
import java.util.Map;
import java.util.Set;

/**
 * @author nrrg
 * Used for double checking the replacement in case of class renaming.
 */
public class ReplaceClassCall {

	private static Map<String,String> replaceClassCall;
	
	public static void reset(){
		replaceClassCall = new HashMap<String,String>();
	}
	
	public static void replaceClassCall(String oldName, String newName){
		if(replaceClassCall == null) 
			reset();
		replaceClassCall.put(oldName, newName);
	}
	
	public static String getReplacingCall(String toReplace){
		if(replaceClassCall == null)
			return toReplace;
		return replaceClassCall.get(toReplace);
	}
	
	public static Set<String> getReplacedCallKeys(){
		if(replaceClassCall == null) 
			reset();
		return replaceClassCall.keySet(); 
	}
}
