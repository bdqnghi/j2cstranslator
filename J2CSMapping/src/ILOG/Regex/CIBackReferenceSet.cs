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
	
	using ILOG.J2CsMapping.Text;
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.IO;
	using System.Runtime.CompilerServices;
	
	/// <summary>
	/// Case Insensitive back reference node;
	/// </summary>
	///
	internal class CIBackReferenceSet : JointSet {
	
		protected internal int referencedGroup;
	
		protected internal int consCounter;
	
		
		/// <param name="substring"></param>
		public CIBackReferenceSet(int groupIndex, int consCounter_0) {
			this.referencedGroup = groupIndex;
			this.consCounter = consCounter_0;
		}
	
		public int Accepts(int strIndex, String testString) {
			throw new PatternSyntaxException("regex.04", "", //$NON-NLS-1$ //$NON-NLS-2$
					0);
		}
	
		public override int Matches(int stringIndex, String testString,
				MatchResultImpl matchResult) {
			String group = GetString(matchResult);
	
			if (group == null
					|| (stringIndex + group.Length) > matchResult.GetRightBound())
				return -1;
	
			for (int i = 0; i < group.Length; i++) {
				if (group[i] != testString[stringIndex + i]
						&& Pattern.GetSupplement(group[i]) != testString[stringIndex + i]) {
					return -1;
				}
			}
			matchResult.SetConsumed(consCounter, group.Length);
			return next.Matches(stringIndex + group.Length, testString,
					matchResult);
		}
	
		public override AbstractSet GetNext() {
			return this.next;
		}
	
		public override void SetNext(AbstractSet next) {
			this.next = next;
		}
	
		protected internal String GetString(MatchResultImpl matchResult) {
			String res = matchResult.GetGroupNoCheck(referencedGroup);
			return res;
			// return (res != null) ? res : "";
		}
	
		public override String GetName() {
			return "CI back reference: " + this.groupIndex; //$NON-NLS-1$
		}
	
		public override bool HasConsumed(MatchResultImpl matchResult) {
			int cons;
			bool res = ((cons = matchResult.GetConsumed(consCounter)) < 0 || cons > 0);
			matchResult.SetConsumed(consCounter, -1);
			return res;
		}
	
	}
}
