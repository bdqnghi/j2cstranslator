/**
 * 
 */
package com.ilog.translator.java2cs.configuration.options;

public enum DotNetFramework {
	NET2 {
		@Override
		public String toString() {
			return "net2";
		}
	},
	NET3 {
		@Override
		public String toString() {
			return "net3";
		}
	},
	NET3_5 {
		@Override
		public String toString() {
			return "net3_5";
		}
	},
	NET4 {
		@Override
		public String toString() {
			return "net4";
		}
	}
}