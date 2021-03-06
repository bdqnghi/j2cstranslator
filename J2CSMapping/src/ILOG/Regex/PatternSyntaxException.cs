/*
 *  Licensed to the Apache Software Foundation (ASF) under one or more
 *  contributor license agreements.  See the NOTICE file distributed with
 *  this work for additional information regarding copyright ownership.
 *  The ASF licenses this file to You under the Apache License, Version 2.0
 *  (the "License"); you may not use this file except in compliance with
 *  the License.  You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 *  Unless required by applicable law or agreed to in writing, software
 *  distributed under the License is distributed on an "AS IS" BASIS,
 *  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 *  See the License for the specific language governing permissions and
 *  limitations under the License.
 */

// --------------------------------------------------------------------------------------------------
// This file was automatically generated by J2CS Translator (http://j2cstranslator.sourceforge.net/). 
// Version 1.3.6.20101125_01     
// 11/30/10 3:38 PM    
// ${CustomMessageForDisclaimer}                                                                             
// --------------------------------------------------------------------------------------------------
 namespace ILOG.J2CsMapping.RegEx {
	
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.IO;
	using System.Runtime.CompilerServices;
	
	
	
	/// @com.intel.drl.spec_ref
	[Serializable]
	public class PatternSyntaxException : ArgumentException {
	
		private const long serialVersionUID = -3864639126226059218L;
	
		private String desc;
	
		private String pattern;
	
		private int index;
	
		
		/// @com.intel.drl.spec_ref
		public PatternSyntaxException(String desc_0, String pattern_1, int index_2) {
			this.index = -1;
			this.desc = desc_0;
			this.pattern = pattern_1;
			this.index = index_2;
		}
	
		
		/// @com.intel.drl.spec_ref
		public String GetPattern() {
			return pattern;
		}
	
		
		/// @com.intel.drl.spec_ref
		public override String Message {
		
		/// @com.intel.drl.spec_ref
		  get {
				String filler = ""; //$NON-NLS-1$
				if (index >= 1) {
					char[] temp = new char[index];
					ILOG.J2CsMapping.Collections.Arrays.Fill(temp,' ');
					filler = ILOG.J2CsMapping.Util.StringUtil.NewString(temp);
				}
				return desc + ((pattern != null && pattern.Length != 0) ? "regex.07" + //$NON-NLS-1$
						new Object[] { index, pattern, filler } : ""); //$NON-NLS-1$
			}
		}
		
	
		
		/// @com.intel.drl.spec_ref
		public String GetDescription() {
			return desc;
		}
	
		
		/// @com.intel.drl.spec_ref
		public int GetIndex() {
			return index;
		}
	}
}
