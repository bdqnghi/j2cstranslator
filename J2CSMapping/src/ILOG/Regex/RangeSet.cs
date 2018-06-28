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
	
	/// <summary>
	/// Represents node accepting single character from the given char class.
	/// </summary>
	///
	
	internal class RangeSet : LeafSet {
	
		private AbstractCharClass chars;
	
		private bool alt;
	
		public RangeSet(AbstractCharClass cs, AbstractSet next) : base(next) {
			this.alt = false;
			this.chars = cs.GetInstance();
			this.alt = cs.alt;
		}
	
		public RangeSet(AbstractCharClass cc) {
			this.alt = false;
			this.chars = cc.GetInstance();
			this.alt = cc.alt;
		}
	
		public override int Accepts(int strIndex, String testString) {
			return (chars.Contains(testString[strIndex])) ? 1 : -1;
		}

        public override String GetName()
        {
			return "range:" + ((alt) ? "^ " : " ") + chars.ToString(); //$NON-NLS-1$ //$NON-NLS-2$ //$NON-NLS-3$
		}
	
		public override bool First(AbstractSet set) {
			if (set  is  CharSet) {
				return ILOG.J2CsMapping.RegEx.AbstractCharClass.Intersects(chars,
						((CharSet) set).GetChar());
			} else if (set  is  RangeSet) {
				return ILOG.J2CsMapping.RegEx.AbstractCharClass.Intersects(chars, ((RangeSet) set).chars);
			} else if (set  is  SupplRangeSet) {
				return ILOG.J2CsMapping.RegEx.AbstractCharClass.Intersects(chars,
						((SupplRangeSet) set).GetChars());
			} else if (set  is  SupplCharSet) {
				return false;
			}
			return true;
		}
	
		protected internal AbstractCharClass GetChars() {
			return chars;
		}
	}
}
