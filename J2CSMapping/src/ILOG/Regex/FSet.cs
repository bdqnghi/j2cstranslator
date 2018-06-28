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
	/// The node which marks end of the particular group.
	/// </summary>
	///
	internal class FSet : AbstractSet {
	
		static internal FSet.PossessiveFSet  posFSet = new FSet.PossessiveFSet ();
	
		internal bool isBackReferenced;
	
		private int groupIndex;
	
		public FSet(int groupIndex_0) {
			this.isBackReferenced = false;
			this.groupIndex = groupIndex_0;
		}
	
		public override int Matches(int stringIndex, String testString,
				MatchResultImpl matchResult) {
			int end = matchResult.GetEnd(groupIndex);
			matchResult.SetEnd(groupIndex, stringIndex);
			int shift = next.Matches(stringIndex, testString, matchResult);
			/*
			 * if(shift >=0 && matchResult.getEnd(groupIndex) == -1) {
			 * matchResult.setEnd(groupIndex, stringIndex); }
			 */
			if (shift < 0)
				matchResult.SetEnd(groupIndex, end);
			return shift;
		}
	
		public int GetGroupIndex() {
			return groupIndex;
		}

        public override String GetName()
        {
			return "fSet"; //$NON-NLS-1$
		}
	
		public override bool HasConsumed(MatchResultImpl mr) {
			return false;
		}
	
		/// <summary>
		/// Marks the end of the particular group and not take into account possible
		/// kickbacks(required for atomic groups, for instance)
		/// </summary>
		///
		internal class PossessiveFSet : AbstractSet {
	
			public override int Matches(int stringIndex, String testString,
					MatchResultImpl matchResult) {
				return stringIndex;
			}

            public override String GetName()
            {
				return "posFSet"; //$NON-NLS-1$
			}
	
			public override bool HasConsumed(MatchResultImpl mr) {
				return false;
			}
		}
	}}