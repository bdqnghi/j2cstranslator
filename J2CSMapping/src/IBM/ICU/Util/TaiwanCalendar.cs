/*
 *******************************************************************************
 * Copyright (C) 1996-2007, International Business Machines Corporation and    *
 * others. All Rights Reserved.                                                *
 *******************************************************************************
 */

// --------------------------------------------------------------------------------------------------
// This file was automatically generated by J2CS Translator (http://j2cstranslator.sourceforge.net/). 
// Version 1.3.6.20101125_01     
// 12/2/10 11:48 AM    
// ${CustomMessageForDisclaimer}                                                                             
// --------------------------------------------------------------------------------------------------
 namespace IBM.ICU.Util {
	
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Globalization;
	using System.IO;
	using System.Runtime.CompilerServices;
	
	/// <summary>
	/// <c>TaiwanCalendar</c> is a subclass of <c>GregorianCalendar</c>
	/// that numbers years since 1912.
	/// <p>
	/// The Taiwan calendar is identical to the Gregorian calendar in all respects
	/// except for the year and era. Years are numbered since 1912 AD (Gregorian).
	/// <p>
	/// The Taiwan Calendar has one era: <c>MINGUO</c>.
	/// <p>
	/// This class should not be subclassed.
	/// </p>
	/// <p>
	/// TaiwanCalendar usually should be instantiated using<see cref="M:IBM.ICU.Util.Calendar.GetInstance(IBM.ICU.Util.ULocale)"/> passing in a
	/// <c>ULocale</c> with the tag <c>"@calendar=Taiwan"</c>.
	/// </p>
	/// </summary>
	///
	/// <seealso cref="T:IBM.ICU.Util.Calendar"/>
	/// <seealso cref="T:IBM.ICU.Util.GregorianCalendar"/>
	/// @draft ICU 3.8
	/// @provisional This API might change or be removed in a future release.
	public class TaiwanCalendar : GregorianCalendar {
	    // jdk1.4.2 serialver
	    private const long serialVersionUID = 2583005278132380631L;
	
	    // -------------------------------------------------------------------------
	    // Constructors...
	    // -------------------------------------------------------------------------
	
	    /// <summary>
	    /// Constant for the Taiwan Era for years before Minguo 1. Brefore Minuo 1 is
	    /// Gregorian 1911, Before Minguo 2 is Gregorian 1910 and so on.
	    /// </summary>
	    ///
	    /// <seealso cref="M:IBM.ICU.Util.Calendar.ERA"/>
	    /// @draft ICU 3.8
	    /// @provisional This API might change or be removed in a future release.
	    public const int BEFORE_MINGUO = 0;
	
	    /// <summary>
	    /// Constant for the Taiwan Era for Minguo. Minguo 1 is 1912 in Gregorian
	    /// calendar.
	    /// </summary>
	    ///
	    /// <seealso cref="M:IBM.ICU.Util.Calendar.ERA"/>
	    /// @draft ICU 3.8
	    /// @provisional This API might change or be removed in a future release.
	    public const int MINGUO = 1;
	
	    /// <summary>
	    /// Constructs a <c>TaiwanCalendar</c> using the current time in the
	    /// default time zone with the default locale.
	    /// </summary>
	    ///
	    /// @draft ICU 3.8
	    /// @provisional This API might change or be removed in a future release.
	    public TaiwanCalendar() : base() {
	    }
	
	    /// <summary>
	    /// Constructs a <c>TaiwanCalendar</c> based on the current time in the
	    /// given time zone with the default locale.
	    /// </summary>
	    ///
	    /// <param name="zone">the given time zone.</param>
	    /// @draft ICU 3.8
	    /// @provisional This API might change or be removed in a future release.
	    public TaiwanCalendar(TimeZone zone) : base(zone) {
	    }
	
	    /// <summary>
	    /// Constructs a <c>TaiwanCalendar</c> based on the current time in the
	    /// default time zone with the given locale.
	    /// </summary>
	    ///
	    /// <param name="aLocale">the given locale.</param>
	    /// @draft ICU 3.8
	    /// @provisional This API might change or be removed in a future release.
        public TaiwanCalendar(ILOG.J2CsMapping.Util.Locale aLocale)
            : base(aLocale)
        {
	    }
	
	    /// <summary>
	    /// Constructs a <c>TaiwanCalendar</c> based on the current time in the
	    /// default time zone with the given locale.
	    /// </summary>
	    ///
	    /// <param name="locale">the given ulocale.</param>
	    /// @draft ICU 3.8
	    /// @provisional This API might change or be removed in a future release.
	    public TaiwanCalendar(ULocale locale) : base(locale) {
	    }
	
	    /// <summary>
	    /// Constructs a <c>TaiwanCalendar</c> based on the current time in the
	    /// given time zone with the given locale.
	    /// </summary>
	    ///
	    /// <param name="zone">the given time zone.</param>
	    /// @draft ICU 3.8
	    /// @provisional This API might change or be removed in a future release.
        public TaiwanCalendar(TimeZone zone, ILOG.J2CsMapping.Util.Locale aLocale)
            : base(zone, aLocale)
        {
	    }
	
	    /// <summary>
	    /// Constructs a <c>TaiwanCalendar</c> based on the current time in the
	    /// given time zone with the given locale.
	    /// </summary>
	    ///
	    /// <param name="zone">the given time zone.</param>
	    /// <param name="locale">the given ulocale.</param>
	    /// @draft ICU 3.8
	    /// @provisional This API might change or be removed in a future release.
	    public TaiwanCalendar(TimeZone zone, ULocale locale) : base(zone, locale) {
	    }
	
	    /// <summary>
	    /// Constructs a <c>TaiwanCalendar</c> with the given date set in the
	    /// default time zone with the default locale.
	    /// </summary>
	    ///
	    /// <param name="date">The date to which the new calendar is set.</param>
	    /// @draft ICU 3.8
	    /// @provisional This API might change or be removed in a future release.
	    public TaiwanCalendar(DateTime date) : this() {
	        SetTime(date);
	    }
	
	    /// <summary>
	    /// Constructs a <c>TaiwanCalendar</c> with the given date set in the
	    /// default time zone with the default locale.
	    /// </summary>
	    ///
	    /// <param name="year">The value used to set the calendar's <see cref="M:IBM.ICU.Util.TaiwanCalendar.YEAR YEAR"/> timefield.</param>
	    /// <param name="month">The value used to set the calendar's <see cref="M:IBM.ICU.Util.TaiwanCalendar.MONTH MONTH"/> timefield. The value is 0-based. e.g., 0 for January.</param>
	    /// <param name="date">The value used to set the calendar's <see cref="M:IBM.ICU.Util.TaiwanCalendar.DATE DATE"/> timefield.</param>
	    /// @draft ICU 3.8
	    /// @provisional This API might change or be removed in a future release.
	    public TaiwanCalendar(int year, int month, int date) : base(year, month, date) {
	    }
	
	    /// <summary>
	    /// Constructs a TaiwanCalendar with the given date and time set for the
	    /// default time zone with the default locale.
	    /// </summary>
	    ///
	    /// <param name="year">The value used to set the calendar's <see cref="M:IBM.ICU.Util.TaiwanCalendar.YEAR YEAR"/> timefield.</param>
	    /// <param name="month">The value used to set the calendar's <see cref="M:IBM.ICU.Util.TaiwanCalendar.MONTH MONTH"/> timefield. The value is 0-based. e.g., 0 for January.</param>
	    /// <param name="date">The value used to set the calendar's <see cref="M:IBM.ICU.Util.TaiwanCalendar.DATE DATE"/> timefield.</param>
	    /// <param name="hour">The value used to set the calendar's <see cref="M:IBM.ICU.Util.TaiwanCalendar.HOUR_OF_DAYHOUR_OF_DAY"/> time field.</param>
	    /// <param name="minute">The value used to set the calendar's <see cref="M:IBM.ICU.Util.TaiwanCalendar.MINUTE MINUTE"/>time field.</param>
	    /// <param name="second">The value used to set the calendar's <see cref="M:IBM.ICU.Util.TaiwanCalendar.SECOND SECOND"/>time field.</param>
	    /// @draft ICU 3.8
	    /// @provisional This API might change or be removed in a future release.
	    public TaiwanCalendar(int year, int month, int date, int hour, int minute,
	            int second) : base(year, month, date, hour, minute, second) {
	    }
	
	    // -------------------------------------------------------------------------
	    // The only practical difference from a Gregorian calendar is that years
	    // are numbered since 1912, inclusive. A couple of overrides will
	    // take care of that....
	    // -------------------------------------------------------------------------
	
	    private const int Taiwan_ERA_START = 1911; // 0=1911, 1=1912
	
	    // Use 1970 as the default value of EXTENDED_YEAR
	    private const int GREGORIAN_EPOCH = 1970;
	
	    /// <summary>
	    /// 
	    /// </summary>
	    ///
	    /// @draft ICU 3.8
	    /// @provisional This API might change or be removed in a future release.
	    protected internal override int HandleGetExtendedYear() {
	        // EXTENDED_YEAR in TaiwanCalendar is a Gregorian year
	        // The default value of EXTENDED_YEAR is 1970 (Minguo 59)
	        int year = GREGORIAN_EPOCH;
	        if (NewerField(IBM.ICU.Util.Calendar.EXTENDED_YEAR, IBM.ICU.Util.Calendar.YEAR) == IBM.ICU.Util.Calendar.EXTENDED_YEAR
	                && NewerField(IBM.ICU.Util.Calendar.EXTENDED_YEAR, IBM.ICU.Util.Calendar.ERA) == IBM.ICU.Util.Calendar.EXTENDED_YEAR) {
	            year = InternalGet(IBM.ICU.Util.Calendar.EXTENDED_YEAR, GREGORIAN_EPOCH);
	        } else {
	            int era = InternalGet(IBM.ICU.Util.Calendar.ERA, MINGUO);
	            if (era == MINGUO) {
	                year = InternalGet(IBM.ICU.Util.Calendar.YEAR, 1) + Taiwan_ERA_START;
	            } else {
	                year = 1 - InternalGet(IBM.ICU.Util.Calendar.YEAR, 1) + Taiwan_ERA_START;
	            }
	        }
	        return year;
	    }
	
	    /// <summary>
	    /// 
	    /// </summary>
	    ///
	    /// @draft ICU 3.8
	    /// @provisional This API might change or be removed in a future release.
	    protected internal override void HandleComputeFields(int julianDay) {
	        base.HandleComputeFields(julianDay);
	        int y = InternalGet(IBM.ICU.Util.Calendar.EXTENDED_YEAR) - Taiwan_ERA_START;
	        if (y > 0) {
	            InternalSet(IBM.ICU.Util.Calendar.ERA, MINGUO);
	            InternalSet(IBM.ICU.Util.Calendar.YEAR, y);
	        } else {
	            InternalSet(IBM.ICU.Util.Calendar.ERA, BEFORE_MINGUO);
	            InternalSet(IBM.ICU.Util.Calendar.YEAR, 1 - y);
	        }
	    }
	
	    /// <summary>
	    /// Override GregorianCalendar. There is only one Taiwan ERA. We should
	    /// really handle YEAR, YEAR_WOY, and EXTENDED_YEAR here too to implement the
	    /// 1..5000000 range, but it's not critical.
	    /// </summary>
	    ///
	    /// @draft ICU 3.8
	    /// @provisional This API might change or be removed in a future release.
	    protected internal override int HandleGetLimit(int field, int limitType) {
	        if (field == IBM.ICU.Util.Calendar.ERA) {
	            if (limitType == IBM.ICU.Util.Calendar.MINIMUM || limitType == IBM.ICU.Util.Calendar.GREATEST_MINIMUM) {
	                return BEFORE_MINGUO;
	            } else {
	                return MINGUO;
	            }
	        }
	        return base.HandleGetLimit(field,limitType);
	    }
	
	    /// <summary>
	    /// 
	    /// </summary>
	    ///
	    /// @draft ICU 3.8
	    /// @provisional This API might change or be removed in a future release.
	    public override String GetType() {
	        return "taiwan";
	    }
	}
}
