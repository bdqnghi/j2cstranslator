// --------------------------------------------------------------------------------------------------
// This file was automatically generated by J2CS Translator (http://j2cstranslator.sourceforge.net/). 
// Version 1.3.6.20101125_01     
// 12/2/10 11:48 AM    
// ${CustomMessageForDisclaimer}                                                                             
// --------------------------------------------------------------------------------------------------
 /// <summary>
/// Copyright (C) 1996-2004, International Business Machines Corporation and    
/// others. All Rights Reserved.                                                
/// </summary>
///
namespace IBM.ICU.Impl {
	
	using IBM.ICU.Text;
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.IO;
	using System.Runtime.CompilerServices;
	
	public sealed class UCharArrayIterator : UCharacterIterator {
	    private readonly char[] text;
	
	    private readonly int start;
	
	    private readonly int limit;
	
	    private int pos;
	
	    public UCharArrayIterator(char[] text_0, int start_1, int limit_2) {
	        if (start_1 < 0 || limit_2 > text_0.Length || start_1 > limit_2) {
	            throw new ArgumentException("start: " + start_1
	                    + " or limit: " + limit_2 + " out of range [0, "
	                    + text_0.Length + ")");
	        }
	        this.text = text_0;
	        this.start = start_1;
	        this.limit = limit_2;
	
	        this.pos = start_1;
	    }
	
	    public override int Current() {
	        return (pos < limit) ? (int) (text[pos]) : (int) (IBM.ICU.Text.UForwardCharacterIterator_Constants.DONE);
	    }
	
	    public override int GetLength() {
	        return limit - start;
	    }
	
	    public override int GetIndex() {
	        return pos - start;
	    }
	
	    public override int Next() {
	        return (pos < limit) ? (int) (text[pos++]) : (int) (IBM.ICU.Text.UForwardCharacterIterator_Constants.DONE);
	    }
	
	    public override int Previous() {
	        return (pos > start) ? (int) (text[--pos]) : (int) (IBM.ICU.Text.UForwardCharacterIterator_Constants.DONE);
	    }
	
	    public override void SetIndex(int index) {
	        if (index < 0 || index > limit - start) {
	            throw new IndexOutOfRangeException("index: " + index
	                                + " out of range [0, " + (limit - start) + ")".ToString());
	        }
	        pos = start + index;
	    }
	
	    public override int GetText(char[] fillIn, int offset) {
	        int len = limit - start;
	        System.Array.Copy((Array)(text),start,(Array)(fillIn),offset,len);
	        return len;
	    }
	
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
	}}
