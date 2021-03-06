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
	/// Represents canonical decomposition of
	/// Unicode character. Is used when
	/// CANON_EQ flag of Pattern class
	/// is specified.
	/// </summary>
	///
	internal class DecomposedCharSet : JointSet {
	
		/// <summary>
		/// Contains information about number of chars
		/// that were read for a codepoint last time
		/// </summary>
		///
		private int readCharsForCodePoint;
	
		/// <summary>
		/// UTF-16 encoding of decomposedChar
		/// </summary>
		///
		private String decomposedCharUTF16;
	
		/// <summary>
		/// Decomposition of the Unicode codepoint
		/// </summary>
		///
		private int[] decomposedChar;
	
		/// <summary>
		/// Length of useful part of decomposedChar
		/// decomposedCharLength <= decomposedChar.length
		/// </summary>
		///
		private int decomposedCharLength;
	
		public DecomposedCharSet(int[] decomposedChar_0, int decomposedCharLength_1) {
			this.readCharsForCodePoint = 1;
			this.decomposedCharUTF16 = null;
			this.decomposedChar = decomposedChar_0;
			this.decomposedCharLength = decomposedCharLength_1;
		}
	
		/// <summary>
		/// Returns the next.
		/// </summary>
		///
		public override AbstractSet GetNext() {
			return this.next;
		}
	
		/// <summary>
		/// Sets next abstract set.
		/// </summary>
		///
		/// <param name="next">The next to set.</param>
		public override void SetNext(AbstractSet next) {
			this.next = next;
		}
	
		public override int Matches(int strIndex, String testString,
				MatchResultImpl matchResult) {
	
			/*
			 * All decompositions have length that 
			 * is less or equal Lexer.MAX_DECOMPOSITION_LENGTH
			 */
			int[] decCurCodePoint;
			int[] decCodePoint = new int[ILOG.J2CsMapping.RegEx.Lexer.MAX_DECOMPOSITION_LENGTH];
			int readCodePoints = 0;
			int rightBound = matchResult.GetRightBound();
			int curChar;
			int i = 0;
	
			if (strIndex >= rightBound) {
				return -1;
			}
	
			/*
			 * We read testString and decompose it gradually to compare with
			 * this decomposedChar at position strIndex 
			 */
			curChar = CodePointAt(strIndex, testString, rightBound);
			strIndex += readCharsForCodePoint;
			decCurCodePoint = ILOG.J2CsMapping.RegEx.Lexer.GetDecomposition(curChar);
			if (decCurCodePoint == null) {
				decCodePoint[readCodePoints++] = curChar;
			} else {
				i = decCurCodePoint.Length;
				System.Array.Copy((Array)(decCurCodePoint),0,(Array)(decCodePoint),0,i);
				readCodePoints += i;
			}
	
			if (strIndex < rightBound) {
				curChar = CodePointAt(strIndex, testString, rightBound);
	
				/*
				 * Read testString until we met a decomposed char boundary
				 * and decompose obtained portion of testString
				 */
				while ((readCodePoints < ILOG.J2CsMapping.RegEx.Lexer.MAX_DECOMPOSITION_LENGTH)
						&& !ILOG.J2CsMapping.RegEx.Lexer.IsDecomposedCharBoundary(curChar)) {
	
					if (ILOG.J2CsMapping.RegEx.Lexer.HasDecompositionNonNullCanClass(curChar)) {
	
						/*
						 * A few codepoints have decompositions and non null
						 * canonical classes, we have to take them into
						 * consideration, but general rule is: 
						 * if canonical class != 0 then no decomposition
						 */
						decCurCodePoint = ILOG.J2CsMapping.RegEx.Lexer.GetDecomposition(curChar);
	
						/*
						 * Length of such decomposition is 1 or 2. See 
						 * UnicodeData file 
						 * http://www.unicode.org/Public/4.0-Update
						 *        /UnicodeData-4.0.0.txt
						 */
						if (decCurCodePoint.Length == 2) {
							decCodePoint[readCodePoints++] = decCurCodePoint[0];
							decCodePoint[readCodePoints++] = decCurCodePoint[1];
						} else {
							decCodePoint[readCodePoints++] = decCurCodePoint[0];
						}
					} else {
						decCodePoint[readCodePoints++] = curChar;
					}
	
					strIndex += readCharsForCodePoint;
	
					if (strIndex < rightBound) {
						curChar = CodePointAt(strIndex, testString, rightBound);
					} else {
						break;
					}
				}
			}
	
			/*
			 * Some optimization since length of decomposed char is <= 3 usually 
			 */
			switch (readCodePoints) {
			case 0:
			case 1:
			case 2:
				break;
	
			case 3:
				int i1 = ILOG.J2CsMapping.RegEx.Lexer.GetCanonicalClass(decCodePoint[1]);
				int i2 = ILOG.J2CsMapping.RegEx.Lexer.GetCanonicalClass(decCodePoint[2]);
	
				if ((i2 != 0) && (i1 > i2)) {
					i1 = decCodePoint[1];
					decCodePoint[1] = decCodePoint[2];
					decCodePoint[2] = i1;
				}
				break;
	
			default:
				decCodePoint = ILOG.J2CsMapping.RegEx.Lexer
						.GetCanonicalOrder(decCodePoint, readCodePoints);
				break;
			}
	
			/*
			 * Compare decomposedChar with decomposed char
			 * that was just read from testString
			 */
			if (readCodePoints != decomposedCharLength) {
				return -1;
			}
	
			for (i = 0; i < readCodePoints; i++) {
				if (decCodePoint[i] != decomposedChar[i]) {
					return -1;
				}
			}
	
			return next.Matches(strIndex, testString, matchResult);
		}
	
		/// <summary>
		/// Return UTF-16 encoding of given Unicode codepoint.
		/// </summary>
		///
		/// <returns>UTF-16 encoding</returns>
		private String GetDecomposedChar() {
			if (decomposedCharUTF16 == null) {
				StringBuilder strBuff = new StringBuilder();
	
				for (int i = 0; i < decomposedCharLength; i++) {
                    strBuff.Append(Character.ToChars(decomposedChar[i]));
				}
				decomposedCharUTF16 = strBuff.ToString();
			}
			return decomposedCharUTF16;
		}

        public override String GetName()
        {
			return "decomposed char:" + GetDecomposedChar(); //$NON-NLS-1$
		}
	
		/// <summary>
		/// Reads Unicode codepoint from input.
		/// </summary>
		///
		/// <param name="strIndex">- index to read codepoint at</param>
		/// <param name="testString">- input</param>
		/// <param name="matchResult">- auxiliary object</param>
		/// <returns>codepoint at given strIndex at testString and</returns>
		public int CodePointAt(int strIndex, String testString, int rightBound) {
	
			/*
			 * We store information about number of codepoints
			 * we read at variable readCharsForCodePoint.
			 */
			int curChar;
	
			readCharsForCodePoint = 1;
			if (strIndex < rightBound - 1) {
				char high = testString[strIndex++];
				char low = testString[strIndex];
	
				if (System.Char.IsSurrogatePair(high, low)) {
					char[] curCodePointUTF16 = new char[] { high, low };
                    curChar = Character.CodePointAt(curCodePointUTF16, 0);
					readCharsForCodePoint = 2;
				} else {
					curChar = high;
				}
			} else {
				curChar = testString[strIndex];
			}
	
			return curChar;
		}
	
		public override bool First(AbstractSet set) {
			return (set  is  DecomposedCharSet) ? ((DecomposedCharSet) set)
					.GetDecomposedChar().Equals(GetDecomposedChar()) : true;
		}
	
		public override bool HasConsumed(MatchResultImpl matchResult) {
			return true;
		}
	}
}
