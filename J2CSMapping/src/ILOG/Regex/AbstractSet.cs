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
	/// Basic class for nodes, representing given regular expression.
	/// Note: All the classes representing nodes has set prefix;
	/// </summary>
	///
	abstract internal class AbstractSet {
	
		public const int TYPE_LEAF = 1 << 0;
	
		public const int TYPE_FSET = 1 << 1;
	
		public const int TYPE_QUANT = 1 << 3;
	
		public const int TYPE_DOTSET = -2147483648 | '.';
	
		/// <summary>
		/// Next node to visit
		/// </summary>
		///
		protected internal AbstractSet next;
	
		/// <summary>
		/// Counter for debugging purposes, represent unique node index;
		/// </summary>
		///
		static internal int counter = 1;
	
		protected internal bool isSecondPassVisited;
	
		protected internal String index;
	
		private int type;
	
		public AbstractSet() {
			this.isSecondPassVisited = false;
			this.index = ((int)(AbstractSet.counter++)).ToString();
			this.type = 0;
		}
	
		public AbstractSet(AbstractSet n) {
			this.isSecondPassVisited = false;
			this.index = ((int)(AbstractSet.counter++)).ToString();
			this.type = 0;
			next = n;
		}
	
		/// <summary>
		/// Checks if this node matches in given position and recursively call
		/// next node matches on positive self match. Returns positive integer if 
		/// entire match succeed, negative otherwise
		/// </summary>
		///
		/// <param name="stringIndex">- string index to start from;</param>
		/// <param name="testString">- input string</param>
		/// <param name="matchResult">- MatchResult to sore result into</param>
		/// <returns>-1 if match fails or n > 0;</returns>
		public abstract int Matches(int stringIndex, String testString,
				MatchResultImpl matchResult);
	
		/// <summary>
		/// Attempts to apply pattern starting from this set/stringIndex; returns
		/// index this search was started from, if value is negative, this means that
		/// this search didn't succeed, additional information could be obtained via
		/// matchResult;
		/// Note: this is default implementation for find method, it's based on 
		/// matches, subclasses do not have to override find method unless 
		/// more effective find method exists for a particular node type 
		/// (sequence, i.e. substring, for example). Same applies for find back 
		/// method.
		/// </summary>
		///
		/// <param name="stringIndex">starting index</param>
		/// <param name="testString">string to search in</param>
		/// <param name="matchResult">result of the match</param>
		/// <returns>last searched index</returns>
		public virtual int Find(int stringIndex, String testString,
				MatchResultImpl matchResult) {
			int length = matchResult.GetRightBound();
			while (stringIndex <= length) {
				if (Matches(stringIndex, testString, matchResult) >= 0) {
					return stringIndex;
				} else {
					stringIndex++;
				}
			}
			return -1;
		}
	
		
		/// <param name="stringIndex">-an index, to finish search back (left limit)</param>
		/// <param name="startSearch">-an index to start search from (right limit)</param>
		/// <param name="testString">-test string;</param>
		/// <param name="matchResult">match result</param>
		/// <returns>an index to start back search next time if this search fails(new
		/// left bound); if this search fails the value is negative;</returns>
		public virtual int FindBack(int stringIndex, int startSearch,
				String testString, MatchResultImpl matchResult) {
			int shift;
			while (startSearch >= stringIndex) {
				if (Matches(startSearch, testString, matchResult) >= 0) {
					return startSearch;
				} else {
					startSearch--;
				}
			}
			return -1;
		}
	
		/// <summary>
		/// Returns true, if this node has consumed any characters during 
		/// positive match attempt, for example node representing character always 
		/// consumes one character if it matches. If particular node matches 
		/// empty sting this method will return false;
		/// </summary>
		///
		/// <param name="matchResult"></param>
		/// <returns></returns>
		public abstract bool HasConsumed(MatchResultImpl matchResult);
	
		/// <summary>
		/// Returns name for the particular node type.
		/// Used for debugging purposes.
		/// </summary>
		///
		public abstract String GetName();
	
		protected internal void SetType(int type_0) {
			this.type = type_0;
		}
	
		public virtual int GetType() {
			return this.type;
		}
	
		protected internal String GetQualifiedName() {
			return "<" + index + ":" + GetName() + ">"; //$NON-NLS-1$ //$NON-NLS-2$ //$NON-NLS-3$
		}
	
		public override String ToString() {
			return GetQualifiedName();
		}
	
		/// <summary>
		/// Returns the next.
		/// </summary>
		///
		public virtual AbstractSet GetNext() {
			return next;
		}
	
		/// <summary>
		/// Sets next abstract set
		/// </summary>
		///
		/// <param name="next_0">The next to set.</param>
		public virtual void SetNext(AbstractSet next_0) {
			this.next = next_0;
		}
	
		/// <summary>
		/// Returns true if the given node intersects with this one,
		/// false otherwise.
		/// This method is being used for quantifiers construction, 
		/// lets consider the following regular expression (a|b)///ccc.
		/// (a|b) does not intersects with "ccc" and thus can be quantified 
		/// greedily (w/o kickbacks), like///+ instead of///.
		/// </summary>
		///
		/// <param name="set">- usually previous node</param>
		/// <returns>true if the given node intersects with this one</returns>
		public virtual bool First(AbstractSet set) {
			return true;
		}
	
		/// <summary>
		/// This method is used for replacement backreferenced
		/// sets.
		/// </summary>
		///
		/// <param name="prev">- node who references to this node</param>
		/// <returns>null if current node need not to be replaced
		/// JointSet which is replacement of 
		/// current node otherwise</returns>
		public virtual JointSet ProcessBackRefReplacement() {
			return null;
		}
	
		/// <summary>
		/// This method is used for traversing nodes after the 
		/// first stage of compilation.
		/// </summary>
		///
		public virtual void ProcessSecondPass() {
			this.isSecondPassVisited = true;
	
			if (next != null) {
	
				if (!next.isSecondPassVisited) {
	
					/*
					 * Add here code to do during the pass
					 */
					JointSet set = next.ProcessBackRefReplacement();
	
					if (set != null) {
						next.isSecondPassVisited = true;
						next = (AbstractSet) set;
					}
	
					/*
					 * End code to do during the pass
					 */
					next.ProcessSecondPass();
				} else {
	
					/*
					 * We reach node through next but it is already traversed.
					 * You can see this situation for AltGroupQuantifierSet.next
					 * when we reach this node through 
					 * AltGroupQuantifierSet.innerset. ... .next 
					 */
	
					/*
					 * Add here code to do during the pass
					 */
					if (next  is  SingleSet
							&& ((FSet) ((JointSet) next).fSet).isBackReferenced) {
						next = next.next;
					}
	
					/*
					 * End code to do during the pass
					 */
				}
			}
		}
	}}
