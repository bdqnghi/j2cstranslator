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
	/// Possessive ? quantifier node.
	/// </summary>
	///
	internal class PossessiveAltQuantifierSet : AltQuantifierSet {
	
		public PossessiveAltQuantifierSet(LeafSet innerSet, AbstractSet next,
				int type) : base(innerSet, next, type) {
		}
	
		public override int Matches(int stringIndex, String testString,
				MatchResultImpl matchResult) {
			int shift = 0;
	
			if (stringIndex + leaf.CharCount() <= matchResult.GetRightBound()
					&& (shift = leaf.Accepts(stringIndex, testString)) >= 1) {
				stringIndex += shift;
			}
	
			return next.Matches(stringIndex, testString, matchResult);
		}
	}
}
