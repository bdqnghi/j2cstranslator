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
	/// Hashtable implementation for int values.
	/// </summary>
	///
	internal class IntHash {
		internal int[] table;
	
		internal int[] values;
	
		internal int mask;
	
		internal int size; // maximum shift
	
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
						|| table[hashCode] == key) { // rewrite
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
	}}
