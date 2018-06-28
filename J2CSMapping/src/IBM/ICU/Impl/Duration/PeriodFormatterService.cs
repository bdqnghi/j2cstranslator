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
 namespace IBM.ICU.Impl.Duration {
	
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.IO;
	using System.Runtime.CompilerServices;
	
	/// <summary>
	/// Provider of Factory instances for building PeriodBuilders, PeriodFormatters,
	/// and DurationFormatters.
	/// </summary>
	///
	public interface PeriodFormatterService {
	
	    /// <summary>
	    /// Creates a new factory for creating DurationFormatters.
	    /// </summary>
	    ///
	    /// <returns>a new DurationFormatterFactory.</returns>
	    DurationFormatterFactory NewDurationFormatterFactory();
	
	    /// <summary>
	    /// Creates a new factory for creating PeriodFormatters.
	    /// </summary>
	    ///
	    /// <returns>a new PeriodFormatterFactory</returns>
	    PeriodFormatterFactory NewPeriodFormatterFactory();
	
	    /// <summary>
	    /// Creates a new factory for creating PeriodBuilders.
	    /// </summary>
	    ///
	    /// <returns>a new PeriodBuilderFactory</returns>
	    PeriodBuilderFactory NewPeriodBuilderFactory();
	
	    /// <summary>
	    /// Return the names of locales supported by factories produced by this
	    /// service.
	    /// </summary>
	    ///
	    /// <returns>a collection of String (locale names)</returns>
	    ICollection GetAvailableLocaleNames();
	}
}