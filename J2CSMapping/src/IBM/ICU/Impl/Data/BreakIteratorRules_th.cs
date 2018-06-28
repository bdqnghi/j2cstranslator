/*
 *******************************************************************************
 * Copyright (C) 1996-2005, International Business Machines Corporation and    *
 * others. All Rights Reserved.                                                *
 *******************************************************************************
 */
// --------------------------------------------------------------------------------------------------
// This file was automatically generated by J2CS Translator (http://j2cstranslator.sourceforge.net/). 
// Version 1.3.6.20101125_01     
// 12/8/10 10:24 AM    
// ${CustomMessageForDisclaimer}                                                                             
// --------------------------------------------------------------------------------------------------
 namespace IBM.ICU.Impl.Data {
	
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.IO;
	using System.Runtime.CompilerServices;
     using ILOG.J2CsMapping.Util;
	
	public class BreakIteratorRules_th : ListResourceBundle {
	    private const String DATA_NAME = "data/th.brk";
	
	    public override Object[][] GetContents() {
	        bool exists = IBM.ICU.Impl.ICUData.Exists(DATA_NAME);
	
	        // if dictionary wasn't found, then this resource bundle doesn't have
	        // much to contribute...
	        if (!exists) {
	            return (Object[][])ILOG.J2CsMapping.Collections.Arrays.CreateJaggedArray(typeof(Object), 0, 0);
	        }
	
	        return new Object[][] {
	                        new Object[] {
	                                "BreakIteratorClasses",
	                                new String[] { "RuleBasedBreakIterator",
	                                        "DictionaryBasedBreakIterator",
	                                        "DictionaryBasedBreakIterator",
	                                        "RuleBasedBreakIterator" } },
	                        new Object[] { "WordBreakDictionary", DATA_NAME },
	                        new Object[] { "LineBreakDictionary", DATA_NAME } };
	    }
	}
}
