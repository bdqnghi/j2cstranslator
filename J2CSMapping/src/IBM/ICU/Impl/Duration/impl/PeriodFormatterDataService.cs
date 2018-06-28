/*
 ******************************************************************************
 * Copyright (C) 2007, International Business Machines Corporation and   *
 * others. All Rights Reserved.                                               *
 ******************************************************************************
 */

// --------------------------------------------------------------------------------------------------
// This file was automatically generated by J2CS Translator (http://j2cstranslator.sourceforge.net/). 
// Version 1.3.6.20101125_01     
// 12/8/10 10:24 AM    
// ${CustomMessageForDisclaimer}                                                                             
// --------------------------------------------------------------------------------------------------
 namespace IBM.ICU.Impl.Duration.Impl {
	
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.IO;
	using System.Runtime.CompilerServices;
	
	/// <summary>
	/// Abstract service for PeriodFormatterData, which defines the localization data
	/// used by period formatters.
	/// </summary>
	///
	public abstract class PeriodFormatterDataService {
	    /// <summary>
	    /// Returns a PeriodFormatterData for the given locale name.
	    /// </summary>
	    ///
	    /// <param name="localeName">the name of the locale</param>
	    /// <returns>a PeriodFormatterData object</returns>
	    public abstract PeriodFormatterData Get(String localeName);
	
	    /// <summary>
	    /// Returns a collection of all the locale names supported by this service.
	    /// </summary>
	    ///
	    /// <returns>a collection of locale names, as String</returns>
	    public abstract ICollection GetAvailableLocales();
	}
}
