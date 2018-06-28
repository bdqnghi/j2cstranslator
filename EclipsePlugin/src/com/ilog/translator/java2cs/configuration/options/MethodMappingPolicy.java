/**
 * 
 */
package com.ilog.translator.java2cs.configuration.options;

public enum MethodMappingPolicy {
	NONE {
		@Override
		public String toString() {
			return "none";
		}
	},
	CAPITALIZED {
		@Override
		public String toString() {
			return "capitalized";
		}
	}
}