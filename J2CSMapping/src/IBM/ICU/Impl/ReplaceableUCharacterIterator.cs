/*
 *******************************************************************************
 * Copyright (C) 1996-2005, International Business Machines Corporation and    *
 * others. All Rights Reserved.                                                *
 *******************************************************************************
 */
// --------------------------------------------------------------------------------------------------
// This file was automatically generated by J2CS Translator (http://j2cstranslator.sourceforge.net/). 
// Version 1.3.6.20101125_01     
// 12/2/10 11:48 AM    
// ${CustomMessageForDisclaimer}                                                                             
// --------------------------------------------------------------------------------------------------
 namespace IBM.ICU.Impl {
	
	using IBM.ICU.Text;
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.IO;
	using System.Runtime.CompilerServices;
	using System.Text;
	
	/// <summary>
	/// DLF docs must define behavior when Replaceable is mutated underneath the
	/// iterator.
	/// This and ICUCharacterIterator share some code, maybe they should share an
	/// implementation, or the common state and implementation should be moved up
	/// into UCharacterIterator.
	/// What are first, last, and getBeginIndex doing here?!?!?!
	/// </summary>
	///
	public class ReplaceableUCharacterIterator : UCharacterIterator {
	
	    // public constructor ------------------------------------------------------
	
	    /// <summary>
	    /// Public constructor
	    /// </summary>
	    ///
	    /// <param name="replaceable">text which the iterator will be based on</param>
	    public ReplaceableUCharacterIterator(Replaceable replaceable) {
	        if (replaceable == null) {
	            throw new ArgumentException();
	        }
	        this.replaceable = replaceable;
	        this.currentIndex = 0;
	    }
	
	    /// <summary>
	    /// Public constructor
	    /// </summary>
	    ///
	    /// <param name="str">text which the iterator will be based on</param>
	    public ReplaceableUCharacterIterator(String str) {
	        if (str == null) {
	            throw new ArgumentException();
	        }
	        this.replaceable = new ReplaceableString(str);
	        this.currentIndex = 0;
	    }
	
	    /// <summary>
	    /// Public constructor
	    /// </summary>
	    ///
	    /// <param name="buf">buffer of text on which the iterator will be based</param>
	    public ReplaceableUCharacterIterator(StringBuilder buf) {
	        if (buf == null) {
	            throw new ArgumentException();
	        }
	        this.replaceable = new ReplaceableString(buf);
	        this.currentIndex = 0;
	    }
	
	    // public methods ----------------------------------------------------------
	
	    /// <summary>
	    /// Creates a copy of this iterator, does not clone the underlying
	    /// <c>Replaceable</c>object
	    /// </summary>
	    ///
	    /// <returns>copy of this iterator</returns>
	    public override Object Clone() {
	        try {
	            return base.Clone();
	        } catch (Exception e) {
	            return null; // never invoked
	        }
	    }
	
	    /// <summary>
	    /// Returns the current UTF16 character.
	    /// </summary>
	    ///
	    /// <returns>current UTF16 character</returns>
	    public override int Current() {
	        if (currentIndex < replaceable.Length()) {
	            return replaceable.CharAt(currentIndex);
	        }
	        return IBM.ICU.Text.UForwardCharacterIterator_Constants.DONE;
	    }
	
	    /// <summary>
	    /// Returns the current codepoint
	    /// </summary>
	    ///
	    /// <returns>current codepoint</returns>
	    public override int CurrentCodePoint() {
	        // cannot use charAt due to it different
	        // behaviour when index is pointing at a
	        // trail surrogate, check for surrogates
	
	        int ch = Current();
	        if (IBM.ICU.Text.UTF16.IsLeadSurrogate((char) ch)) {
	            // advance the index to get the next code point
	            Next();
	            // due to post increment semantics current() after next()
	            // actually returns the next char which is what we want
	            int ch2 = Current();
	            // current should never change the current index so back off
	            Previous();
	
	            if (IBM.ICU.Text.UTF16.IsTrailSurrogate((char) ch2)) {
	                // we found a surrogate pair
	                return IBM.ICU.Impl.UCharacterProperty.GetRawSupplementary((char) ch,
	                        (char) ch2);
	            }
	        }
	        return ch;
	    }
	
	    /// <summary>
	    /// Returns the length of the text
	    /// </summary>
	    ///
	    /// <returns>length of the text</returns>
	    public override int GetLength() {
	        return replaceable.Length();
	    }
	
	    /// <summary>
	    /// Gets the current currentIndex in text.
	    /// </summary>
	    ///
	    /// <returns>current currentIndex in text.</returns>
	    public override int GetIndex() {
	        return currentIndex;
	    }
	
	    /// <summary>
	    /// Returns next UTF16 character and increments the iterator's currentIndex
	    /// by 1. If the resulting currentIndex is greater or equal to the text
	    /// length, the currentIndex is reset to the text length and a value of
	    /// DONECODEPOINT is returned.
	    /// </summary>
	    ///
	    /// <returns>next UTF16 character in text or DONE if the new currentIndex is
	    /// off the end of the text range.</returns>
	    public override int Next() {
	        if (currentIndex < replaceable.Length()) {
	            return replaceable.CharAt(currentIndex++);
	        }
	        return IBM.ICU.Text.UForwardCharacterIterator_Constants.DONE;
	    }
	
	    /// <summary>
	    /// Returns previous UTF16 character and decrements the iterator's
	    /// currentIndex by 1. If the resulting currentIndex is less than 0, the
	    /// currentIndex is reset to 0 and a value of DONECODEPOINT is returned.
	    /// </summary>
	    ///
	    /// <returns>next UTF16 character in text or DONE if the new currentIndex is
	    /// off the start of the text range.</returns>
	    public override int Previous() {
	        if (currentIndex > 0) {
	            return replaceable.CharAt(--currentIndex);
	        }
	        return IBM.ICU.Text.UForwardCharacterIterator_Constants.DONE;
	    }
	
	    /// <summary>
	    /// <p>
	    /// Sets the currentIndex to the specified currentIndex in the text and
	    /// returns that single UTF16 character at currentIndex. This assumes the
	    /// text is stored as 16-bit code units.
	    /// </p>
	    /// </summary>
	    ///
	    /// <param name="currentIndex">the currentIndex within the text.</param>
	    /// <exception cref="IllegalArgumentException">is thrown if an invalid currentIndex is supplied. i.e.currentIndex is out of bounds.</exception>
	    /// @returns the character at the specified currentIndex or DONE if the
	    /// specified currentIndex is equal to the end of the text.
	    public override void SetIndex(int currentIndex) {
	        if (currentIndex < 0 || currentIndex > replaceable.Length()) {
	            throw new IndexOutOfRangeException();
	        }
	        this.currentIndex = currentIndex;
	    }
	
	    public override int GetText(char[] fillIn, int offset) {
	        int length = replaceable.Length();
	        if (offset < 0 || offset + length > fillIn.Length) {
	            throw new IndexOutOfRangeException(ILOG.J2CsMapping.Util.IlNumber.ToString(length).ToString());
	        }
	        replaceable.GetChars(0, length, fillIn, offset);
	        return length;
	    }
	
	    // private data members ----------------------------------------------------
	
	    /// <summary>
	    /// Replacable object
	    /// </summary>
	    ///
	    private Replaceable replaceable;
	
	    /// <summary>
	    /// Current currentIndex
	    /// </summary>
	    ///
	    private int currentIndex;
	
	}
}