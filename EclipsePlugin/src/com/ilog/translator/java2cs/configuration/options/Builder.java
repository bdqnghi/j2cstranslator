/**
 * 
 */
package com.ilog.translator.java2cs.configuration.options;

public interface Builder<T> {
	void build(String value, T option);

	String createStringValue(T option);
}