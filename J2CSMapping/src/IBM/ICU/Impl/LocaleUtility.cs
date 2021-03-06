/*
 ******************************************************************************
 * Copyright (C) 1996-2007, International Business Machines Corporation and   *
 * others. All Rights Reserved.                                               *
 ******************************************************************************
 *
 ******************************************************************************
 */

// --------------------------------------------------------------------------------------------------
// This file was automatically generated by J2CS Translator (http://j2cstranslator.sourceforge.net/). 
// Version 1.3.6.20101125_01     
// 12/2/10 11:47 AM    
// ${CustomMessageForDisclaimer}                                                                             
// --------------------------------------------------------------------------------------------------
 namespace IBM.ICU.Impl {
	
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Globalization;
	using System.IO;
	using System.Runtime.CompilerServices;
     using ILOG.J2CsMapping.Util;
	
	/// <summary>
	/// A class to hold utility functions missing from java.util.Locale.
	/// </summary>
	///
	public class LocaleUtility {
	
	    /// <summary>
	    /// A helper function to convert a string of the form aa_BB_CC to a locale
	    /// object. Why isn't this in Locale?
	    /// </summary>
	    ///
        public static Locale GetLocaleFromName(String name)
        {
	        String language = "";
	        String country = "";
	        String variant = "";
	
	        int i1 = name.IndexOf('_');
	        if (i1 < 0) {
	            language = name;
	        } else {
	            language = name.Substring(0,(i1)-(0));
	            ++i1;
	            int i2 = name.IndexOf('_', i1);
	            if (i2 < 0) {
	                country = name.Substring(i1);
	            } else {
	                country = name.Substring(i1,(i2)-(i1));
	                variant = name.Substring(i2 + 1);
	            }
	        }

            return new Locale(language, country, variant);
	    }
	
	    /// <summary>
	    /// Compare two locale strings of the form aa_BB_CC, and return true if
	    /// parent is a 'strict' fallback of child, that is, if child =~
	    /// "^parent(_.+)///" (roughly).
	    /// </summary>
	    ///
	    public static bool IsFallbackOf(String parent, String child) {
	        if (!child.StartsWith(parent)) {
	            return false;
	        }
	        int i = parent.Length;
	        return (i == child.Length || child[i] == '_');
	    }
	
	    /// <summary>
	    /// Compare two locales, and return true if the parent is a 'strict' fallback
	    /// of the child (parent string is a fallback of child string).
	    /// </summary>
	    ///
	    public static bool IsFallbackOf(CultureInfo parent, CultureInfo child) {
	        return IsFallbackOf(parent.ToString(), child.ToString());
	    }
	
	    /*
	     * Convenience method that calls canonicalLocaleString(String) with
	     * locale.toString();
	     */
	    /*
	     * public static String canonicalLocaleString(Locale locale) { return
	     * canonicalLocaleString(locale.toString()); }
	     */
	
	    /*
	     * You'd think that Locale canonicalizes, since it munges the renamed
	     * languages, but it doesn't quite. It forces the region to be upper case
	     * but doesn't do anything about the language or variant. Our canonical form
	     * is 'lower_UPPER_UPPER'.
	     */
	    /*
	     * public static String canonicalLocaleString(String id) { if (id != null) {
	     * int x = id.indexOf("_"); if (x == -1) { id =
	     * id.toLowerCase(Locale.ENGLISH); } else { StringBuffer buf = new
	     * StringBuffer(); buf.append(id.substring(0,
	     * x).toLowerCase(Locale.ENGLISH));
	     * buf.append(id.substring(x).toUpperCase(Locale.ENGLISH));
	     * 
	     * int len = buf.length(); int n = len; while (--n >= 0 && buf.charAt(n) ==
	     * '_') { } if (++n != len) { buf.delete(n, len); } id = buf.toString(); } }
	     * return id; }
	     */
	
	    /// <summary>
	    /// Fallback from the given locale name by removing the rightmost _-delimited
	    /// element. If there is none, return the root locale ("", "", ""). If this
	    /// is the root locale, return null. NOTE: The string "root" is not
	    /// recognized; do not use it.
	    /// </summary>
	    ///
	    /// <returns>a new Locale that is a fallback from the given locale, or null.</returns>
        public static Locale Fallback(Locale loc)
        {
	
	        // Split the locale into parts and remove the rightmost part
            String[] parts = new String[] { loc.GetLanguage(), loc.GetCountry(),
	                loc.GetVariant() };
	        int i;
	        for (i = 2; i >= 0; --i) {
	            if (parts[i].Length != 0) {
	                parts[i] = "";
	                break;
	            }
	        }
	        if (i < 0) {
	            return null; // All parts were empty
	        }
            return new Locale(parts[0], parts[1], parts[2]);
	    }
	}
}
