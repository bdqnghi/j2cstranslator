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
	
	/// <summary>
	/// Provide a base class for Transforms that focuses just on the transformation
	/// of the text. APIs that take Transliterator, but only depend on the text
	/// transformation should use this interface in the API instead.
	/// </summary>
	///
	/// @draft ICU 3.8
	/// @provisional This API might change or be removed in a future release.
	public interface StringTransform {
	    /// <summary>
	    /// Transform the text in some way, to be determined by the subclass.
	    /// </summary>
	    ///
	    /// <param name="source">text to be transformed (eg lowercased)</param>
	    /// <returns>result</returns>
	    /// @draft ICU 3.8
	    /// @provisional This API might change or be removed in a future release.
	    String Transform(String source);
	}}
