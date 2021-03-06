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
	/// Formatter for durations in milliseconds.
	/// </summary>
	///
	public interface DurationFormatter {
	
	    /// <summary>
	    /// Formats the duration between now and a target date.
	    /// <p>
	    /// This is a convenience method that calls formatDurationFrom(long, long)
	    /// using now as the reference date, and the difference between now and
	    /// <c>targetDate.getTime()</c> as the duration.
	    /// </summary>
	    ///
	    /// <param name="targetDate">the ending date</param>
	    /// <returns>the formatted time</returns>
	    String FormatDurationFromNowTo(DateTime targetDate);
	
	    /// <summary>
	    /// Formats a duration expressed in milliseconds.
	    /// <p>
	    /// This is a convenience method that calls formatDurationFrom using the
	    /// current system time as the reference date.
	    /// </summary>
	    ///
	    /// <param name="duration">the duration in milliseconds</param>
	    /// <param name="tz">the time zone</param>
	    /// <returns>the formatted time</returns>
	    String FormatDurationFromNow(long duration);
	
	    /// <summary>
	    /// Formats a duration expressed in milliseconds from a reference date.
	    /// <p>
	    /// The reference date allows formatters to use actual durations of
	    /// variable-length periods (like months) if they wish.
	    /// <p>
	    /// The duration is expressed as the number of milliseconds in the past
	    /// (negative values) or future (positive values) with respect to a reference
	    /// date (expressed as milliseconds in epoch).
	    /// </summary>
	    ///
	    /// <param name="duration">the duration in milliseconds</param>
	    /// <param name="referenceDate">the date from which to compute the duration</param>
	    /// <returns>the formatted time</returns>
	    String FormatDurationFrom(long duration, long referenceDate);
	
	    /// <summary>
	    /// Returns a new DurationFormatter that's the same as this one but formats
	    /// for a new locale.
	    /// </summary>
	    ///
	    /// <param name="localeName">the name of the new locale</param>
	    /// <returns>a new formatter for the given locale</returns>
	    DurationFormatter WithLocale(String localeName);
	
	    /// <summary>
	    /// Returns a new DurationFormatter that's the same as this one but uses a
	    /// different time zone.
	    /// </summary>
	    ///
	    /// <param name="tz">the time zone in which to compute durations.</param>
	    /// <returns>a new formatter for the given locale</returns>
        DurationFormatter WithTimeZone(IBM.ICU.Util.TimeZone tz);
	}
}
