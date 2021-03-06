/*
 *******************************************************************************
 * Copyright (C) 2007, International Business Machines Corporation and         *
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
	
	using IBM.ICU.Util;
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.IO;
	using System.Runtime.CompilerServices;
	using System.Text;
     using ILOG.J2CsMapping.Util;
	
	/// <summary>
	/// This class implements a formatter over a duration in time such as
	/// "2 days from now" or "3 hours ago".
	/// </summary>
	///
	/// @draft ICU 3.8
	/// @provisional This API might change or be removed in a future release.
	public abstract class DurationFormat : UFormat {
	
	    /// <summary>
	    /// Construct a duration format for the specified locale
	    /// </summary>
	    ///
	    /// @draft ICU 3.8
	    /// @provisional This API might change or be removed in a future release.
	    public static DurationFormat GetInstance(ULocale locale) {
	        return (DurationFormat) IBM.ICU.Impl.Duration.BasicDurationFormat.GetInstance(locale);
	    }
	
	    /// <exclude/>
	    /// <summary>
	    /// Subclass interface
	    /// </summary>
	    ///
	    protected internal DurationFormat() {
	    }
	
	    /// <exclude/>
	    /// <summary>
	    /// Subclass interface
	    /// </summary>
	    ///
	    protected internal DurationFormat(ULocale locale) {
	        SetLocale(locale, locale);
	    }
	
	    /// <summary>
	    /// Format an arbitrary object. Defaults to a call to formatDurationFromNow()
	    /// for either Long or Date objects.
	    /// </summary>
	    ///
	    /// <param name="object">the object to format. Should be either a Long, Date, orjavax.xml.datatype.Duration object.</param>
	    /// <param name="toAppend">the buffer to append to</param>
	    /// <param name="pos">the field position, may contain additional error messages.</param>
	    /// <returns>the toAppend buffer</returns>
	    /// @draft ICU 3.8
	    /// @provisional This API might change or be removed in a future release.
	    public abstract override StringBuilder FormatObject(Object obj0, StringBuilder toAppend,
	            ILOG.J2CsMapping.Text.FieldPosition pos);
	
	    /// <summary>
	    /// DurationFormat cannot parse, by default. This method will throw an
	    /// UnsupportedOperationException.
	    /// </summary>
	    ///
	    /// @draft ICU 3.8
	    /// @provisional This API might change or be removed in a future release.
	    public override Object ParseObject(String source, ILOG.J2CsMapping.Text.ParsePosition pos) {
	        throw new NotSupportedException();
	    }
	
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
	    /// @draft ICU 3.8
	    /// @provisional This API might change or be removed in a future release.
	    public abstract String FormatDurationFromNowTo(DateTime targetDate);
	
	    /// <summary>
	    /// Formats a duration expressed in milliseconds.
	    /// <p>
	    /// This is a convenience method that calls formatDurationFrom using the
	    /// current system time as the reference date.
	    /// </summary>
	    ///
	    /// <param name="duration">the duration in milliseconds</param>
	    /// <returns>the formatted time</returns>
	    /// @draft ICU 3.8
	    /// @provisional This API might change or be removed in a future release.
	    public abstract String FormatDurationFromNow(long duration);
	
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
	    /// @draft ICU 3.8
	    /// @provisional This API might change or be removed in a future release.
	    public abstract String FormatDurationFrom(long duration, long referenceDate);
	
	}
}
