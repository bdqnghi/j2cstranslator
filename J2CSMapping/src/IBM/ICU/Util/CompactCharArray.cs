/*
 *******************************************************************************
 * Copyright (C) 1996-2007, International Business Machines Corporation and    *
 * others. All Rights Reserved.                                                *
 *******************************************************************************
 */

// --------------------------------------------------------------------------------------------------
// This file was automatically generated by J2CS Translator (http://j2cstranslator.sourceforge.net/). 
// Version 1.3.6.20101125_01     
// 12/2/10 11:47 AM    
// ${CustomMessageForDisclaimer}                                                                             
// --------------------------------------------------------------------------------------------------
 namespace IBM.ICU.Util {
	
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.IO;
	using System.Runtime.CompilerServices;
	
	
	/// <exclude/>
	/// <summary>
	/// class CompactATypeArray : use only on primitive data types Provides a compact
	/// way to store information that is indexed by Unicode values, such as character
	/// properties, types, keyboard values, etc.This is very useful when you have a
	/// block of Unicode data that contains significant values while the rest of the
	/// Unicode data is unused in the application or when you have a lot of
	/// redundance, such as where all 21,000 Han ideographs have the same value.
	/// However, lookup is much faster than a hash table. A compact array of any
	/// primitive data type serves two purposes:
	/// <UL type = round>
	/// <LI>Fast access of the indexed values.
	/// <LI>Smaller memory footprint.
	/// </UL>
	/// A compact array is composed of a index array and value array. The index array
	/// contains the indicies of Unicode characters to the value array.
	/// </summary>
	///
	/// <seealso cref="T:IBM.ICU.Util.CompactByteArray"/>
	public sealed class CompactCharArray : ICloneable {
	
	    /// <exclude/>
	    /// <summary>
	    /// The total number of Unicode characters.
	    /// </summary>
	    ///
	    public const int UNICODECOUNT = 65536;
	
	    /// <exclude/>
	    /// <summary>
	    /// Default constructor for CompactCharArray, the default value of the
	    /// compact array is 0.
	    /// </summary>
	    ///
	    public CompactCharArray() : this((char)0) {
	    }
	
	    /// <exclude/>
	    /// <summary>
	    /// Constructor for CompactCharArray.
	    /// </summary>
	    ///
	    /// <param name="defaultValue">the default value of the compact array.</param>
	    public CompactCharArray(char defaultValue) {
	        int i;
	        values = new char[UNICODECOUNT];
	        indices = new char[INDEXCOUNT];
	        hashes = new int[INDEXCOUNT];
	        for (i = 0; i < UNICODECOUNT; ++i) {
	            values[i] = defaultValue;
	        }
	        for (i = 0; i < INDEXCOUNT; ++i) {
	            indices[i] = (char) (i << BLOCKSHIFT);
	            hashes[i] = 0;
	        }
	        isCompact = false;
	
	        this.defaultValue = defaultValue;
	    }
	
	    /// <exclude/>
	    /// <summary>
	    /// Constructor for CompactCharArray.
	    /// </summary>
	    ///
	    /// <param name="indexArray">the indicies of the compact array.</param>
	    /// <param name="newValues">the values of the compact array.</param>
	    /// <exception cref="IllegalArgumentException">If the index is out of range.</exception>
	    public CompactCharArray(char[] indexArray, char[] newValues) {
	        int i;
	        if (indexArray.Length != INDEXCOUNT)
	            throw new ArgumentException("Index out of bounds.");
	        for (i = 0; i < INDEXCOUNT; ++i) {
	            char index = indexArray[i];
	            if ((index < 0) || (index >= newValues.Length + BLOCKCOUNT))
	                throw new ArgumentException("Index out of bounds.");
	        }
	        indices = indexArray;
	        values = newValues;
	        isCompact = true;
	    }
	
	    /// <exclude/>
	    /// <summary>
	    /// Constructor for CompactCharArray.
	    /// </summary>
	    ///
	    /// <param name="indexArray">the RLE-encoded indicies of the compact array.</param>
	    /// <param name="valueArray">the RLE-encoded values of the compact array.</param>
	    /// <exception cref="IllegalArgumentException">if the index or value array is the wrong size.</exception>
	    public CompactCharArray(String indexArray, String valueArray) : this(IBM.ICU.Impl.Utility.RLEStringToCharArray(indexArray), IBM.ICU.Impl.Utility.RLEStringToCharArray(valueArray)) {
	    }
	
	    /// <exclude/>
	    /// <summary>
	    /// Get the mapped value of a Unicode character.
	    /// </summary>
	    ///
	    /// <param name="index">the character to get the mapped value with</param>
	    /// <returns>the mapped value of the given character</returns>
	    public char ElementAt(char index) {
	        int ix = (indices[index >> BLOCKSHIFT] & 0xFFFF) + (index & BLOCKMASK);
	        return (ix >= values.Length) ? defaultValue : values[ix];
	    }
	
	    /// <exclude/>
	    /// <summary>
	    /// Set a new value for a Unicode character. Set automatically expands the
	    /// array if it is compacted.
	    /// </summary>
	    ///
	    /// <param name="index">the character to set the mapped value with</param>
	    /// <param name="value">the new mapped value</param>
	    public void SetElementAt(char index, char value_ren) {
	        if (isCompact)
	            Expand();
	        values[(int) index] = value_ren;
	        TouchBlock(index >> BLOCKSHIFT, value_ren);
	    }
	
	    /// <exclude/>
	    /// <summary>
	    /// Set new values for a range of Unicode character.
	    /// </summary>
	    ///
	    /// <param name="start">the starting offset of the range</param>
	    /// <param name="end">the ending offset of the range</param>
	    /// <param name="value">the new mapped value</param>
	    public void SetElementAt(char start, char end, char value_ren) {
	        int i;
	        if (isCompact) {
	            Expand();
	        }
	        for (i = start; i <= end; ++i) {
	            values[i] = value_ren;
	            TouchBlock(i >> BLOCKSHIFT, value_ren);
	        }
	    }
	
	    /// <exclude/>
	    /// <summary>
	    /// Compact the array
	    /// </summary>
	    ///
	    public void Compact() {
	        Compact(true);
	    }
	
	    /// <exclude/>
	    /// <summary>
	    /// Compact the array.
	    /// </summary>
	    ///
	    public void Compact(bool exhaustive) {
	        if (!isCompact) {
	            int iBlockStart = 0;
	            char iUntouched = (char) (0xFFFF);
	            int newSize = 0;
	
	            char[] target = (exhaustive) ? new char[UNICODECOUNT] : values;
	
	            for (int i = 0; i < indices.Length; ++i, iBlockStart += BLOCKCOUNT) {
	                indices[i] = ((Char)0xFFFF);
	                bool touched = BlockTouched(i);
	                if (!touched && iUntouched != 0xFFFF) {
	                    // If no values in this block were set, we can just set its
	                    // index to be the same as some other block with no values
	                    // set, assuming we've seen one yet.
	                    indices[i] = iUntouched;
	                } else {
	                    int jBlockStart = 0;
	                    // See if we can find a previously compacted block that's
	                    // identical
	                    for (int j = 0; j < i; ++j, jBlockStart += BLOCKCOUNT) {
	                        if (hashes[i] == hashes[j]
	                                && ArrayRegionMatches(values, iBlockStart,
	                                        values, jBlockStart, BLOCKCOUNT)) {
	                            indices[i] = indices[j];
	                        }
	                    }
	                    if (indices[i] == 0xFFFF) {
	                        int dest; // Where to copy
	                        if (exhaustive) {
	                            // See if we can find some overlap with another
	                            // block
	                            dest = FindOverlappingPosition(iBlockStart, target,
	                                    newSize);
	                        } else {
	                            // Just copy to the end; it's quicker
	                            dest = newSize;
	                        }
	                        int limit = dest + BLOCKCOUNT;
	                        if (limit > newSize) {
	                            for (int j_0 = newSize; j_0 < limit; ++j_0) {
	                                target[j_0] = values[iBlockStart + j_0 - dest];
	                            }
	                            newSize = limit;
	                        }
	                        indices[i] = (char) dest;
	                        if (!touched) {
	                            // If this is the first untouched block we've seen,
	                            // remember its index.
	                            iUntouched = (char) jBlockStart;
	                        }
	                    }
	                }
	            }
	            // we are done compacting, so now make the array shorter
	            char[] result = new char[newSize];
	            System.Array.Copy((Array)(target),0,(Array)(result),0,newSize);
	            values = result;
	            isCompact = true;
	            hashes = null;
	        }
	    }
	
	    private int FindOverlappingPosition(int start, char[] tempValues,
	            int tempCount) {
	        for (int i = 0; i < tempCount; i += 1) {
	            int currentCount = BLOCKCOUNT;
	            if (i + BLOCKCOUNT > tempCount) {
	                currentCount = tempCount - i;
	            }
	            if (ArrayRegionMatches(values, start, tempValues, i, currentCount))
	                return i;
	        }
	        return tempCount;
	    }
	
	    /// <summary>
	    /// Convenience utility to compare two arrays of doubles.
	    /// </summary>
	    ///
	    /// <param name="len">the length to compare. The start indices and start+len must bevalid.</param>
	    static internal bool ArrayRegionMatches(char[] source, int sourceStart,
	            char[] target, int targetStart, int len) {
	        int sourceEnd = sourceStart + len;
	        int delta = targetStart - sourceStart;
	        for (int i = sourceStart; i < sourceEnd; i++) {
	            if (source[i] != target[i + delta])
	                return false;
	        }
	        return true;
	    }
	
	    /// <summary>
	    /// Remember that a specified block was "touched", i.e. had a value set.
	    /// Untouched blocks can be skipped when compacting the array
	    /// </summary>
	    ///
	    private void TouchBlock(int i, int value_ren) {
	        hashes[i] = (hashes[i] + (value_ren << 1)) | 1;
	    }
	
	    /// <summary>
	    /// Query whether a specified block was "touched", i.e. had a value set.
	    /// Untouched blocks can be skipped when compacting the array
	    /// </summary>
	    ///
	    private bool BlockTouched(int i) {
	        return hashes[i] != 0;
	    }
	
	    /// <exclude/>
	    /// <summary>
	    /// For internal use only. Do not modify the result, the behavior of modified
	    /// results are undefined.
	    /// </summary>
	    ///
	    public char[] GetIndexArray() {
	        return indices;
	    }
	
	    /// <exclude/>
	    /// <summary>
	    /// For internal use only. Do not modify the result, the behavior of modified
	    /// results are undefined.
	    /// </summary>
	    ///
	    public char[] GetValueArray() {
	        return values;
	    }
	
	    /// <exclude/>
	    /// <summary>
	    /// Overrides Cloneable
	    /// </summary>
	    ///
	    public Object Clone() {
	        try {
	            CompactCharArray other = (CompactCharArray) base.MemberwiseClone();
	            other.values = (char[]) values.Clone();
	            other.indices = (char[]) indices.Clone();
	            if (hashes != null)
	                other.hashes = (int[]) hashes.Clone();
	            return other;
	        } catch (Exception e) {
	            throw new InvalidOperationException();
	        }
	    }
	
	    /// <exclude/>
	    /// <summary>
	    /// Compares the equality of two compact array objects.
	    /// </summary>
	    ///
	    /// <param name="obj">the compact array object to be compared with this.</param>
	    /// <returns>true if the current compact array object is the same as the
	    /// compact array object obj; false otherwise.</returns>
	    public override bool Equals(Object obj) {
	        if (obj == null)
	            return false;
	        if ((Object) this == obj) // quick check
	            return true;
	        if ((Object) GetType() != (Object) obj.GetType()) // same class?
	            return false;
	        CompactCharArray other = (CompactCharArray) obj;
	        for (int i = 0; i < UNICODECOUNT; i++) {
	            // could be sped up later
	            if (ElementAt((char) i) != other.ElementAt((char) i))
	                return false;
	        }
	        return true; // we made it through the guantlet.
	    }
	
	    /// <exclude/>
	    /// <summary>
	    /// Generates the hash code for the compact array object
	    /// </summary>
	    ///
	    public override int GetHashCode() {
	        int result = 0;
	        int increment = Math.Min(3,values.Length / 16);
	        for (int i = 0; i < values.Length; i += increment) {
	            result = result * 37 + values[i];
	        }
	        return result;
	    }
	
	    // --------------------------------------------------------------
	    // private
	    // --------------------------------------------------------------
	
	    /// <summary>
	    /// Expanding takes the array back to a 65536 element array.
	    /// </summary>
	    ///
	    private void Expand() {
	        int i;
	        if (isCompact) {
	            char[] tempArray;
	            hashes = new int[INDEXCOUNT];
	            tempArray = new char[UNICODECOUNT];
	            for (i = 0; i < UNICODECOUNT; ++i) {
	                tempArray[i] = ElementAt((char) i);
	            }
	            for (i = 0; i < INDEXCOUNT; ++i) {
	                indices[i] = (char) (i << BLOCKSHIFT);
	            }
	            values = null;
	            values = tempArray;
	            isCompact = false;
	        }
	    }
	
	    /// <exclude/>
	    public const int BLOCKSHIFT = 5; // NormalizerBuilder needs - liu
	
	    internal const int BLOCKCOUNT = (1 << BLOCKSHIFT);
	
	    internal const int INDEXSHIFT = (16 - BLOCKSHIFT);
	
	    internal const int INDEXCOUNT = (1 << INDEXSHIFT);
	
	    internal const int BLOCKMASK = BLOCKCOUNT - 1;
	
	    private char[] values;
	
	    private char[] indices;
	
	    private int[] hashes;
	
	    private bool isCompact;
	
	    internal char defaultValue;
	}
}
