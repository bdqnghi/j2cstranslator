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
	/// Group node over subexpression w/o alternations.
	/// </summary>
	///
	internal class SingleSet : JointSet {
	
		protected internal AbstractSet kid;
	
		public SingleSet(AbstractSet child, FSet fSet) {
			this.kid = child;
			this.fSet = fSet;
			this.groupIndex = fSet.GetGroupIndex();
		}
	
		public override int Matches(int stringIndex, String testString,
				MatchResultImpl matchResult) {
			int start = matchResult.GetStart(groupIndex);
			matchResult.SetStart(groupIndex, stringIndex);
			int shift = kid.Matches(stringIndex, testString, matchResult);
			if (shift >= 0) {
				return shift;
			}
			matchResult.SetStart(groupIndex, start);
			return -1;
		}
	
		public override int Find(int stringIndex, String testString,
				MatchResultImpl matchResult) {
			int res = kid.Find(stringIndex, testString, matchResult);
			if (res >= 0)
				matchResult.SetStart(groupIndex, res);
			return res;
		}
	
		public override int FindBack(int stringIndex, int lastIndex,
				String testString, MatchResultImpl matchResult) {
			int res = kid.FindBack(stringIndex, lastIndex, testString, matchResult);
			if (res >= 0)
				matchResult.SetStart(groupIndex, res);
			return res;
		}
	
		public override bool First(AbstractSet set) {
			return kid.First(set);
		}
	
		/// <summary>
		/// This method is used for replacement backreferenced
		/// sets.
		/// </summary>
		///
		public override JointSet ProcessBackRefReplacement() {
			BackReferencedSingleSet set = new BackReferencedSingleSet(this);
	
			/*
			 * We will store a reference to created BackReferencedSingleSet
			 * in next field. This is needed toprocess replacement
			 * of sets correctly since sometimes we cannot renew all references to
			 * detachable set in the current point of traverse. See
			 * QuantifierSet and AbstractSet processSecondPass() methods for
			 * more details.
			 */
			next = set;
			return set;
		}
	
		/// <summary>
		/// This method is used for traversing nodes after the 
		/// first stage of compilation.
		/// </summary>
		///
		public override void ProcessSecondPass() {
			this.isSecondPassVisited = true;
	
			if (fSet != null && !fSet.isSecondPassVisited) {
	
				/*
				 * Add here code to do during the pass
				 */
	
				/*
				 * End code to do during the pass
				 */
				fSet.ProcessSecondPass();
			}
	
			if (kid != null && !kid.isSecondPassVisited) {
	
				/*
				 * Add here code to do during the pass
				 */
				JointSet set = kid.ProcessBackRefReplacement();
	
				if (set != null) {
					kid.isSecondPassVisited = true;
					kid = (AbstractSet) set;
				}
	
				/*
				 * End code to do during the pass
				 */
	
				kid.ProcessSecondPass();
			}
		}
	}
}