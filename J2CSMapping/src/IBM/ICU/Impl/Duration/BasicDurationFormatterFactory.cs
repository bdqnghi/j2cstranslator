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
	/// Abstract factory object used to create DurationFormatters. DurationFormatters
	/// are immutable once created.
	/// <p>
	/// Setters on the factory mutate the factory and return it, for chaining.
	/// <p>
	/// Subclasses override getFormatter to return a custom DurationFormatter.
	/// </summary>
	///
	internal class BasicDurationFormatterFactory : DurationFormatterFactory {
	    private BasicPeriodFormatterService ps;
	
	    private PeriodFormatter formatter;
	
	    private PeriodBuilder builder;
	
	    private DateFormatter fallback;
	
	    private long fallbackLimit;
	
	    private String localeName;

        private IBM.ICU.Util.TimeZone timeZone;
	
	    private BasicDurationFormatter f; // cache
	
	    /// <summary>
	    /// Create a default formatter for the current locale and time zone.
	    /// </summary>
	    ///
	    internal BasicDurationFormatterFactory(BasicPeriodFormatterService ps_0) {
	        this.ps = ps_0;
	        this.localeName = System.Globalization.CultureInfo.InvariantCulture.ToString();
	        this.timeZone = IBM.ICU.Util.TimeZone.GetDefault();
	    }
	
	    /// <summary>
	    /// Set the period formatter used by the factory. New formatters created with
	    /// this factory will use the given period formatter.
	    /// </summary>
	    ///
	    /// <param name="builder">the builder to use</param>
	    /// <returns>this BasicDurationFormatterFactory</returns>
	    public virtual DurationFormatterFactory SetPeriodFormatter(PeriodFormatter formatter_0) {
	        if (formatter_0 != this.formatter) {
	            this.formatter = formatter_0;
	            Reset();
	        }
	        return this;
	    }
	
	    /// <summary>
	    /// Set the builder used by the factory. New formatters created with this
	    /// factory will use the given locale.
	    /// </summary>
	    ///
	    /// <param name="builder_0">the builder to use</param>
	    /// <returns>this BasicDurationFormatterFactory</returns>
	    public virtual DurationFormatterFactory SetPeriodBuilder(PeriodBuilder builder_0) {
	        if (builder_0 != this.builder) {
	            this.builder = builder_0;
	            Reset();
	        }
	        return this;
	    }
	
	    /// <summary>
	    /// Set a fallback formatter for durations over a given limit.
	    /// </summary>
	    ///
	    /// <param name="fallback_0">the fallback formatter to use, or null</param>
	    /// <returns>this BasicDurationFormatterFactory</returns>
	    public virtual DurationFormatterFactory SetFallback(DateFormatter fallback_0) {
	        bool doReset = (fallback_0 == null) ? this.fallback != null : !fallback_0
	                .Equals(this.fallback);
	        if (doReset) {
	            this.fallback = fallback_0;
	            Reset();
	        }
	        return this;
	    }
	
	    /// <summary>
	    /// Set a fallback limit for durations over a given limit.
	    /// </summary>
	    ///
	    /// <param name="fallbackLimit_0">the fallback limit to use, or 0 if none is desired.</param>
	    /// <returns>this BasicDurationFormatterFactory</returns>
	    public virtual DurationFormatterFactory SetFallbackLimit(long fallbackLimit_0) {
	        if (fallbackLimit_0 < 0) {
	            fallbackLimit_0 = 0;
	        }
	        if (fallbackLimit_0 != this.fallbackLimit) {
	            this.fallbackLimit = fallbackLimit_0;
	            Reset();
	        }
	        return this;
	    }
	
	    /// <summary>
	    /// Set the name of the locale that will be used when creating new
	    /// formatters.
	    /// </summary>
	    ///
	    /// <param name="localeName_0">the name of the Locale</param>
	    /// <returns>this BasicDurationFormatterFactory</returns>
	    public virtual DurationFormatterFactory SetLocale(String localeName_0) {
	        if (!localeName_0.Equals(this.localeName)) {
	            this.localeName = localeName_0;
	            Reset();
	        }
	        return this;
	    }
	
	    /// <summary>
	    /// Set the name of the locale that will be used when creating new
	    /// formatters.
	    /// </summary>
	    ///
	    /// <param name="localeName">the name of the Locale</param>
	    /// <returns>this BasicDurationFormatterFactory</returns>
        public virtual DurationFormatterFactory SetTimeZone(IBM.ICU.Util.TimeZone timeZone_0)
        {
	        if (!timeZone_0.Equals(this.timeZone)) {
	            this.timeZone = timeZone_0;
	            Reset();
	        }
	        return this;
	    }
	
	    /// <summary>
	    /// Return a formatter based on this factory's current settings.
	    /// </summary>
	    ///
	    /// <returns>a BasicDurationFormatter</returns>
	    public virtual DurationFormatter GetFormatter() {
	        if (f == null) {
	            if (fallback != null) {
	                fallback = fallback.WithLocale(localeName).WithTimeZone(
	                        timeZone);
	            }
	            formatter = GetPeriodFormatter().WithLocale(localeName);
	            builder = GetPeriodBuilder().WithLocale(localeName).WithTimeZone(
	                    timeZone);
	
	            f = CreateFormatter();
	        }
	        return f;
	    }
	
	    /// <summary>
	    /// Return the current period formatter.
	    /// </summary>
	    ///
	    /// <returns>the current period formatter</returns>
	    public PeriodFormatter GetPeriodFormatter() {
	        if (formatter == null) {
	            formatter = ps.NewPeriodFormatterFactory().GetFormatter();
	        }
	        return formatter;
	    }
	
	    /// <summary>
	    /// Return the current builder.
	    /// </summary>
	    ///
	    /// <returns>the current builder</returns>
	    public PeriodBuilder GetPeriodBuilder() {
	        if (builder == null) {
	            builder = ps.NewPeriodBuilderFactory().GetSingleUnitBuilder();
	        }
	        return builder;
	    }
	
	    /// <summary>
	    /// Return the current fallback formatter.
	    /// </summary>
	    ///
	    /// <returns>the fallback formatter, or null if there is no fallback formatter</returns>
	    public DateFormatter GetFallback() {
	        return fallback;
	    }
	
	    /// <summary>
	    /// Return the current fallback formatter limit
	    /// </summary>
	    ///
	    /// <returns>the limit, or 0 if there is no fallback.</returns>
	    public long GetFallbackLimit() {
	        return (fallback == null) ? (long) (0) : (long) (fallbackLimit);
	    }
	
	    /// <summary>
	    /// Return the current locale name.
	    /// </summary>
	    ///
	    /// <returns>the current locale name</returns>
	    public String GetLocaleName() {
	        return localeName;
	    }
	
	    /// <summary>
	    /// Return the current locale name.
	    /// </summary>
	    ///
	    /// <returns>the current locale name</returns>
        public IBM.ICU.Util.TimeZone GetTimeZone()
        {
	        return timeZone;
	    }
	
	    /// <summary>
	    /// Create the formatter. All local fields are already initialized.
	    /// </summary>
	    ///
	    protected internal BasicDurationFormatter CreateFormatter() {
	        return new BasicDurationFormatter(formatter, builder, fallback,
	                fallbackLimit, localeName, timeZone);
	    }
	
	    /// <summary>
	    /// Clear the cached formatter. Subclasses must call this if their state has
	    /// changed. This is automatically invoked by setBuilder, setFormatter,
	    /// setFallback, setLocaleName, and setTimeZone
	    /// </summary>
	    ///
	    protected internal void Reset() {
	        f = null;
	    }
	}
}
