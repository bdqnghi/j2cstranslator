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
	using System.Text;
     using ILOG.J2CsMapping.Util;
	
	/// <summary>
	/// This class represents nodes constructed with character sequences. For
	/// example, lets consider regular expression: ".///word.///". During regular
	/// expression compilation phase character sequence w-o-r-d, will be represented
	/// with single node for the entire word.
	/// During the match phase, Moyer-Moore algorithm will be used for fast
	/// searching.
	/// Please follow the next link for more details about mentioned algorithm:
	/// http://portal.acm.org/citation.cfm?id=359859
	/// </summary>
	///
	internal class SequenceSet : LeafSet {
	
		private String str0;
	
		private SequenceSet.IntHash  leftToRight;
	
		private SequenceSet.IntHash  rightToLeft;
	
		internal SequenceSet(StringBuilder substring) {
			this.str0 = null;
			this.str0 = substring.ToString();
			charCount = substring.Length;
	
			leftToRight = new SequenceSet.IntHash (charCount);
			rightToLeft = new SequenceSet.IntHash (charCount);
			for (int j = 0; j < charCount - 1; j++) {
				leftToRight.Put(str0[j], charCount - j - 1);
				rightToLeft
						.Put(str0[charCount - j - 1], charCount - j - 1);
			}
		}
	
		public override int Accepts(int strIndex, String testString) {
			return (StartsWith(testString, strIndex)) ? charCount : -1;
		}
	
		public override int Find(int strIndex, String testString,
				MatchResultImpl matchResult) {
	
			int strLength = matchResult.GetRightBound();
	
			while (strIndex <= strLength) {
				strIndex = IndexOf(testString, strIndex, strLength);
	
				if (strIndex < 0)
					return -1;
				if (next.Matches(strIndex + charCount, testString, matchResult) >= 0)
					return strIndex;
	
				strIndex++;
			}
	
			return -1;
		}
	
		public override int FindBack(int strIndex, int lastIndex, String testString,
				MatchResultImpl matchResult) {
			String testStr = testString.ToString();
	
			while (lastIndex >= strIndex) {
				lastIndex = LastIndexOf(testString, strIndex, lastIndex);
	
				if (lastIndex < 0)
					return -1;
				if (next.Matches(lastIndex + charCount, testString, matchResult) >= 0)
					return lastIndex;
	
				lastIndex--;
			}
	
			return -1;
		}
	
		public override String GetName() {
			return "sequence: " + str0; //$NON-NLS-1$
		}
	
		public override bool First(AbstractSet set) {
			if (set  is  CharSet) {
				return ((CharSet) set).GetChar() == str0[0];
			} else if (set  is  RangeSet) {
				return ((RangeSet) set).Accepts(0, str0.Substring(0,(1)-(0))) > 0;
			} else if (set  is  SupplRangeSet) {
				return ((SupplRangeSet) set).Contains(str0[0])
						|| ((str0.Length > 1) && ((SupplRangeSet) set)
                                .Contains(Character.ToCodePoint(str0[0],
										str0[1])));
			} else if ((set  is  SupplCharSet)) {
                return (str0.Length > 1) ? ((SupplCharSet)set).GetCodePoint() == Character
						.ToCodePoint(str0[0], str0[1]) : false;
			}
	
			return true;
		}
	
		protected internal int IndexOf(String str, int from, int to) {
			int last = str0[charCount - 1];
			int i = from;
	
			while (i <= to - charCount) {
				char ch = str[i + charCount - 1];
				if (ch == last && StartsWith(str, i)) {
					return i;
				}
	
				i += leftToRight.Get(ch);
			}
			return -1;
		}
	
		protected internal int LastIndexOf(String str, int to, int from) {
			int first = str0[0];
			int size = str.Length;
			int delta;
			int i = ((delta = size - from - charCount) > 0) ? from : from + delta;
	
			while (i >= to) {
				char ch = str[i];
				if (ch == first && StartsWith(str, i)) {
					return i;
				}
	
				i -= rightToLeft.Get(ch);
			}
			return -1;
		}
	
		protected internal bool StartsWith(String str, int from) {
			for (int i = 0; i < charCount; i++) {
				if (str[i + from] != str0[i])
					return false;
			}
			return true;
		}
	
		internal class IntHash {
			internal int[] table, values;
	
			internal int mask;
	
			internal int size; // <-maximum shift
	
			public IntHash(int size_0) {
				while (size_0 >= mask) {
					mask = (mask << 1) | 1;
				}
				mask = (mask << 1) | 1;
				table = new int[mask + 1];
				values = new int[mask + 1];
				this.size = size_0;
			}
	
			public void Put(int key, int value_ren) {
				int i = 0;
				int hashCode = key & mask;
	
				for (;;) {
					if (table[hashCode] == 0 // empty
							|| table[hashCode] == key) {// rewrite
						table[hashCode] = key;
						values[hashCode] = value_ren;
						return;
					}
					i++;
					i &= mask;
	
					hashCode += i;
					hashCode &= mask;
				}
			}
	
			public int Get(int key) {
	
				int hashCode = key & mask;
				int i = 0;
				int storedKey;
	
				for (;;) {
					storedKey = table[hashCode];
	
					if (storedKey == 0) { // empty
						return size;
					}
	
					if (storedKey == key) {
						return values[hashCode];
					}
	
					i++;
					i &= mask;
	
					hashCode += i;
					hashCode &= mask;
				}
			}
		}
	}
}
