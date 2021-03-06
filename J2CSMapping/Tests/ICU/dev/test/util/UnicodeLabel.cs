/*
 *******************************************************************************
 * Copyright (C) 1996-2004, International Business Machines Corporation and    *
 * others. All Rights Reserved.                                                *
 *******************************************************************************
 */
// --------------------------------------------------------------------------------------------------
// This file was automatically generated by J2CS Translator (http://j2cstranslator.sourceforge.net/). 
// Version 1.3.6.20101125_01     
// 12/13/10 4:01 PM    
// ${CustomMessageForDisclaimer}                                                                             
// --------------------------------------------------------------------------------------------------
 namespace IBM.ICU.Charset {
	
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.IO;
	using System.Runtime.CompilerServices;
	using System.Text;
	
	
	public abstract class UnicodeLabel {
	
	    public abstract String GetValue(int codepoint, bool isShort);
	
	    public String GetValue(String s, String separator, bool withCodePoint) {
	        if (s.Length == 1) { // optimize simple case
	            return GetValue(s[0], withCodePoint);
	        }
	        StringBuilder sb = new StringBuilder();
	        int cp;
	        for (int i = 0; i < s.Length; i += IBM.ICU.Text.UTF16.GetCharCount(cp)) {
	            cp = IBM.ICU.Text.UTF16.CharAt(s, i);
	            if (i != 0)
	                sb.Append(separator);
	            sb.Append(GetValue(cp, withCodePoint));
	        }
	        return sb.ToString();
	    }
	
	    public virtual int GetMaxWidth(bool isShort) {
	        return 0;
	    }
	
	    private class Hex : UnicodeLabel {
	        public override String GetValue(int codepoint, bool isShort) {
	            if (isShort)
	                return IBM.ICU.Impl.Utility.Hex(codepoint, 4);
	            return "U+" + IBM.ICU.Impl.Utility.Hex(codepoint, 4);
	        }
	    }
	
	    public class Constant : UnicodeLabel {
	        private String value_ren;
	
	        public Constant(String value_ren) {
	            if (value_ren == null)
	                value_ren = "";
	            this.value_ren = value_ren;
	        }
	
	        public override String GetValue(int codepoint, bool isShort) {
	            return value_ren;
	        }
	
	        public override int GetMaxWidth(bool isShort) {
	            return value_ren.Length;
	        }
	    }
	
	    public static readonly UnicodeLabel NULL = new UnicodeLabel.Constant ("");
	
	    public static readonly UnicodeLabel HEX = new UnicodeLabel.Hex ();
	}}
