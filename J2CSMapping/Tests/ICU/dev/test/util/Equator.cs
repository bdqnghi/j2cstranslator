/*
 *******************************************************************************
 * Copyright (C) 2002-2004, International Business Machines Corporation and    *
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
	
	public interface Equator {
	    /// <summary>
	    /// Comparator function. If overridden, must handle case of null, and compare
	    /// any two objects that could be compared. Must obey normal rules of
	    /// symmetry: a=b => b=a and transitivity: a=b & b=c => a=b)
	    /// </summary>
	    ///
	    /// <param name="a"></param>
	    /// <param name="b"></param>
	    /// <returns>true if a and b are equal</returns>
	    bool IsEqual(Object a, Object b);
	
	    /// <summary>
	    /// Must obey normal rules: a=b => getHashCode(a)=getHashCode(b)
	    /// </summary>
	    ///
	    /// <param name="object"></param>
	    /// <returns>a hash code for the object</returns>
	    int GetHashCode(Object obj0);
	}}