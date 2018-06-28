/*
 *******************************************************************************
 * Copyright (C) 1996-2007, International Business Machines Corporation and    *
 * others. All Rights Reserved.                                                *
 *******************************************************************************
 */
// --------------------------------------------------------------------------------------------------
// This file was automatically generated by J2CS Translator (http://j2cstranslator.sourceforge.net/). 
// Version 1.3.6.20101125_01     
// 12/2/10 11:30 AM    
// ${CustomMessageForDisclaimer}                                                                             
// --------------------------------------------------------------------------------------------------
 namespace IBM.ICU.Text {
	
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.IO;
	using System.Runtime.CompilerServices;
	using System.Text;
	
	
	/// <summary>
	/// <c>ReplaceableString</c> is an adapter class that implements the
	/// <c>Replaceable</c> API around an ordinary <c>StringBuffer</c>.
	/// <p>
	/// <em>Note:</em> This class does not support attributes and is not intended for
	/// general use. Most clients will need to implement <see cref="T:IBM.ICU.Text.Replaceable"/> in their
	/// text representation class.
	/// <p>
	/// Copyright &copy; IBM Corporation 1999. All rights reserved.
	/// </summary>
	///
	/// <seealso cref="T:IBM.ICU.Text.Replaceable"/>
	/// @stable ICU 2.0
	public class ReplaceableString : Replaceable {
	    private StringBuilder buf;
	
	    /// <summary>
	    /// Construct a new object with the given initial contents.
	    /// </summary>
	    ///
	    /// <param name="str">initial contents</param>
	    /// @stable ICU 2.0
	    public ReplaceableString(String str) {
	        buf = new StringBuilder(str);
	    }
	
	    /// <summary>
	    /// Construct a new object using <c>buf</c> for internal storage. The
	    /// contents of <c>buf</c> at the time of construction are used as the
	    /// initial contents. <em>Note!
	    /// Modifications to <c>buf</c> will modify this object, and
	    /// vice versa.</em>
	    /// </summary>
	    ///
	    /// <param name="buf_0">object to be used as internal storage</param>
	    /// @stable ICU 2.0
	    public ReplaceableString(StringBuilder buf_0) {
	        this.buf = buf_0;
	    }
	
	    /// <summary>
	    /// Construct a new empty object.
	    /// </summary>
	    ///
	    /// @stable ICU 2.0
	    public ReplaceableString() {
	        buf = new StringBuilder();
	    }
	
	    /// <summary>
	    /// Return the contents of this object as a <c>String</c>.
	    /// </summary>
	    ///
	    /// <returns>string contents of this object</returns>
	    /// @stable ICU 2.0
	    public override String ToString() {
	        return buf.ToString();
	    }
	
	    /// <summary>
	    /// Return a substring of the given string.
	    /// </summary>
	    ///
	    /// @stable ICU 2.0
	    public String Substring(int start, int limit) {
	        return buf.ToString(start,limit-start);
	    }
	
	    /// <summary>
	    /// Return the number of characters contained in this object.
	    /// <c>Replaceable</c> API.
	    /// </summary>
	    ///
	    /// @stable ICU 2.0
	    public virtual int Length() {
	        return buf.Length;
	    }
	
	    /// <summary>
	    /// Return the character at the given position in this object.
	    /// <c>Replaceable</c> API.
	    /// </summary>
	    ///
	    /// <param name="offset">offset into the contents, from 0 to <c>length()</c> - 1</param>
	    /// @stable ICU 2.0
	    public virtual char CharAt(int offset) {
	        return buf[offset];
	    }
	
	    /// <summary>
	    /// Return the 32-bit code point at the given 16-bit offset into the text.
	    /// This assumes the text is stored as 16-bit code units with surrogate pairs
	    /// intermixed. If the offset of a leading or trailing code unit of a
	    /// surrogate pair is given, return the code point of the surrogate pair.
	    /// </summary>
	    ///
	    /// <param name="offset">an integer between 0 and <c>length()</c>-1 inclusive</param>
	    /// <returns>32-bit code point of text at given offset</returns>
	    /// @stable ICU 2.0
	    public virtual int Char32At(int offset) {
	        return IBM.ICU.Text.UTF16.CharAt(buf, offset);
	    }
	
	    /// <summary>
	    /// Copies characters from this object into the destination character array.
	    /// The first character to be copied is at index <c>srcStart</c>; the
	    /// last character to be copied is at index <c>srcLimit-1</c> (thus the
	    /// total number of characters to be copied is <c>srcLimit-srcStart</c>
	    /// ). The characters are copied into the subarray of <c>dst</c>
	    /// starting at index <c>dstStart</c> and ending at index
	    /// <c>dstStart + (srcLimit-srcStart) - 1</c>.
	    /// </summary>
	    ///
	    /// <param name="srcStart">the beginning index to copy, inclusive; <code>0<= start <= limit</code>.</param>
	    /// <param name="srcLimit">the ending index to copy, exclusive;<code>start <= limit <= length()</code>.</param>
	    /// <param name="dst">the destination array.</param>
	    /// <param name="dstStart">the start offset in the destination array.</param>
	    /// @stable ICU 2.0
	    public virtual void GetChars(int srcStart, int srcLimit, char[] dst, int dstStart) {
	        IBM.ICU.Impl.Utility.GetChars(buf, srcStart, srcLimit, dst, dstStart);
	    }
	
	    /// <summary>
	    /// Replace zero or more characters with new characters.
	    /// <c>Replaceable</c> API.
	    /// </summary>
	    ///
	    /// <param name="start">the beginning index, inclusive; <code>0 <= start<= limit</code>.</param>
	    /// <param name="limit">the ending index, exclusive; <code>start <= limit<= length()</code>.</param>
	    /// <param name="text">new text to replace characters <c>start</c> to<c>limit - 1</c></param>
	    /// @stable ICU 2.0
	    public virtual void Replace(int start, int limit, String text) {
	        buf.Insert(start, text, limit - start);
	    }
	
	    /// <summary>
	    /// Replace a substring of this object with the given text.
	    /// </summary>
	    ///
	    /// <param name="start">the beginning index, inclusive; <code>0 <= start<= limit</code>.</param>
	    /// <param name="limit">the ending index, exclusive; <code>start <= limit<= length()</code>.</param>
	    /// <param name="chars">the text to replace characters <c>start</c> to<c>limit - 1</c></param>
	    /// <param name="charsStart">the beginning index into <c>chars</c>, inclusive;<code>0 <= start <= limit</code>.</param>
	    /// <param name="charsLen">the number of characters of <c>chars</c>.</param>
	    /// @stable ICU 2.0
	    public virtual void Replace(int start, int limit, char[] chars, int charsStart,
	            int charsLen) {
	        buf.Remove(start,limit-(start));
	        buf.Insert(start, chars, charsStart, charsLen);
	    }
	
	    /// <summary>
	    /// Copy a substring of this object, retaining attribute (out-of-band)
	    /// information. This method is used to duplicate or reorder substrings. The
	    /// destination index must not overlap the source range.
	    /// </summary>
	    ///
	    /// <param name="start">the beginning index, inclusive; <code>0 <= start <=limit</code>.</param>
	    /// <param name="limit">the ending index, exclusive; <code>start <= limit <=length()</code>.</param>
	    /// <param name="dest">the destination index. The characters from<c>start..limit-1</c> will be copied to<c>dest</c>. Implementations of this method may assumethat <code>dest <= start ||dest >= limit</code>.</param>
	    /// @stable ICU 2.0
	    public virtual void Copy(int start, int limit, int dest) {
	        if (start == limit && start >= 0 && start <= buf.Length) {
	            return;
	        }
	        char[] text = new char[limit - start];
	        GetChars(start, limit, text, 0);
	        Replace(dest, dest, text, 0, limit - start);
	    }
	
	    /// <summary>
	    /// Implements Replaceable
	    /// </summary>
	    ///
	    /// @stable ICU 2.0
	    public virtual bool HasMetaData() {
	        return false;
	    }
	}
}
