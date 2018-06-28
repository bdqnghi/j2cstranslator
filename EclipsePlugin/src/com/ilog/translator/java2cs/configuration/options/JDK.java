/**
 * 
 */
package com.ilog.translator.java2cs.configuration.options;

public enum JDK {
	JDK1_4 {
		@Override
		public String toString() {
			return "jdk1_4";
		}
	},
	JDK1_5 {
		@Override
		public String toString() {
			return "jdk1_5";
		}
	},
	JDK6 {
		@Override
		public String toString() {
			return "jdk6";
		}
	},
	JDK7 {
		@Override
		public String toString() {
			return "jdk7";
		}
	}
}