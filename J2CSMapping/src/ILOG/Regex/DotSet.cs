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
     using ILOG.J2CsMapping.Util;
	
	/// <summary>
	/// Node accepting any character except line terminators;
	/// </summary>
	///
	internal sealed class DotSet : JointSet {
	
		internal AbstractLineTerminator lt;
	
		public DotSet(AbstractLineTerminator lt_0) : base() {
			this.lt = lt_0;
		}
	
		public override int Matches(int stringIndex, String testString,
				MatchResultImpl matchResult) {
			int strLength = matchResult.GetRightBound();
	
			if (stringIndex + 1 > strLength) {
				matchResult.hitEnd = true;
				return -1;
			}
			char high = testString[stringIndex];
	
			if (System.Char.IsHighSurrogate(high) && (stringIndex + 2 <= strLength)) {
				char low = testString[stringIndex + 1];
	
				if (System.Char.IsSurrogatePair(high, low)) {
                    return (lt.IsLineTerminator(Character.ToCodePoint(high, low))) ? -1
							: next.Matches(stringIndex + 2, testString, matchResult);
				}
			}
	
			return (lt.IsLineTerminator(high)) ? -1 : next.Matches(stringIndex + 1,
					testString, matchResult);
		}

        public override String GetName()
        {
			return "."; //$NON-NLS-1$
		}
	
		public override AbstractSet GetNext() {
			return this.next;
		}
	
		public override void SetNext(AbstractSet next) {
			this.next = next;
		}
	
		public override int GetType() {
			return ILOG.J2CsMapping.RegEx.AbstractSet.TYPE_DOTSET;
		}
	
		public override bool HasConsumed(MatchResultImpl matchResult) {
			return true;
		}
	}
}
