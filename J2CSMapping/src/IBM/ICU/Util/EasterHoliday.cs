/*
 *******************************************************************************
 * Copyright (C) 1996-2006, International Business Machines Corporation and    *
 * others. All Rights Reserved.                                                *
 *******************************************************************************
 */

// --------------------------------------------------------------------------------------------------
// This file was automatically generated by J2CS Translator (http://j2cstranslator.sourceforge.net/). 
// Version 1.3.6.20101125_01     
// 12/2/10 11:47 AM    
// ${CustomMessageForDisclaimer}                                                                             
// --------------------------------------------------------------------------------------------------
 namespace IBM.ICU.Util {
	
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.IO;
	using System.Runtime.CompilerServices;
	
	/// <summary>
	/// A Holiday subclass which represents holidays that occur a fixed number of
	/// days before or after Easter. Supports both the Western and Orthodox methods
	/// for calculating Easter.
	/// </summary>
	///
	/// @draft ICU 2.8 (retainAll)
	/// @provisional This API might change or be removed in a future release.
	public class EasterHoliday : Holiday {
	    /// <summary>
	    /// Construct a holiday that falls on Easter Sunday every year
	    /// </summary>
	    ///
	    /// <param name="name">The name of the holiday</param>
	    /// @draft ICU 2.8
	    /// @provisional This API might change or be removed in a future release.
	    public EasterHoliday(String name) : base(name, new EasterRule(0,false)) {
	    }
	
	    /// <summary>
	    /// Construct a holiday that falls a specified number of days before or after
	    /// Easter Sunday each year.
	    /// </summary>
	    ///
	    /// <param name="daysAfter">The number of days before (-) or after (+) Easter</param>
	    /// <param name="name">The name of the holiday</param>
	    /// @draft ICU 2.8
	    /// @provisional This API might change or be removed in a future release.
	    public EasterHoliday(int daysAfter, String name) : base(name, new EasterRule(daysAfter,false)) {
	    }
	
	    /// <summary>
	    /// Construct a holiday that falls a specified number of days before or after
	    /// Easter Sunday each year, using either the Western or Orthodox calendar.
	    /// </summary>
	    ///
	    /// <param name="daysAfter">The number of days before (-) or after (+) Easter</param>
	    /// <param name="orthodox">Use the Orthodox calendar?</param>
	    /// <param name="name">The name of the holiday</param>
	    /// @draft ICU 2.8
	    /// @provisional This API might change or be removed in a future release.
	    public EasterHoliday(int daysAfter, bool orthodox, String name) : base(name, new EasterRule(daysAfter,orthodox)) {
	    }
	
	    /// <summary>
	    /// Shrove Tuesday, aka Mardi Gras, 48 days before Easter
	    /// </summary>
	    ///
	    /// @draft ICU 2.8
	    /// @provisional This API might change or be removed in a future release.
	    static public readonly EasterHoliday SHROVE_TUESDAY = new EasterHoliday(-48,
	            "Shrove Tuesday");
	
	    /// <summary>
	    /// Ash Wednesday, start of Lent, 47 days before Easter
	    /// </summary>
	    ///
	    /// @draft ICU 2.8
	    /// @provisional This API might change or be removed in a future release.
	    static public readonly EasterHoliday ASH_WEDNESDAY = new EasterHoliday(-47,
	            "Ash Wednesday");
	
	    /// <summary>
	    /// Palm Sunday, 7 days before Easter
	    /// </summary>
	    ///
	    /// @draft ICU 2.8
	    /// @provisional This API might change or be removed in a future release.
	    static public readonly EasterHoliday PALM_SUNDAY = new EasterHoliday(-7,
	            "Palm Sunday");
	
	    /// <summary>
	    /// Maundy Thursday, 3 days before Easter
	    /// </summary>
	    ///
	    /// @draft ICU 2.8
	    /// @provisional This API might change or be removed in a future release.
	    static public readonly EasterHoliday MAUNDY_THURSDAY = new EasterHoliday(-3,
	            "Maundy Thursday");
	
	    /// <summary>
	    /// Good Friday, 2 days before Easter
	    /// </summary>
	    ///
	    /// @draft ICU 2.8
	    /// @provisional This API might change or be removed in a future release.
	    static public readonly EasterHoliday GOOD_FRIDAY = new EasterHoliday(-2,
	            "Good Friday");
	
	    /// <summary>
	    /// Easter Sunday
	    /// </summary>
	    ///
	    /// @draft ICU 2.8
	    /// @provisional This API might change or be removed in a future release.
	    static public readonly EasterHoliday EASTER_SUNDAY = new EasterHoliday(0,
	            "Easter Sunday");
	
	    /// <summary>
	    /// Easter Monday, 1 day after Easter
	    /// </summary>
	    ///
	    /// @draft ICU 2.8
	    /// @provisional This API might change or be removed in a future release.
	    static public readonly EasterHoliday EASTER_MONDAY = new EasterHoliday(1,
	            "Easter Monday");
	
	    /// <summary>
	    /// Ascension, 39 days after Easter
	    /// </summary>
	    ///
	    /// @draft ICU 2.8
	    /// @provisional This API might change or be removed in a future release.
	    static public readonly EasterHoliday ASCENSION = new EasterHoliday(39,
	            "Ascension");
	
	    /// <summary>
	    /// Pentecost (aka Whit Sunday), 49 days after Easter
	    /// </summary>
	    ///
	    /// @draft ICU 2.8
	    /// @provisional This API might change or be removed in a future release.
	    static public readonly EasterHoliday PENTECOST = new EasterHoliday(49,
	            "Pentecost");
	
	    /// <summary>
	    /// Whit Sunday (aka Pentecost), 49 days after Easter
	    /// </summary>
	    ///
	    /// @draft ICU 2.8
	    /// @provisional This API might change or be removed in a future release.
	    static public readonly EasterHoliday WHIT_SUNDAY = new EasterHoliday(49,
	            "Whit Sunday");
	
	    /// <summary>
	    /// Whit Monday, 50 days after Easter
	    /// </summary>
	    ///
	    /// @draft ICU 2.8
	    /// @provisional This API might change or be removed in a future release.
	    static public readonly EasterHoliday WHIT_MONDAY = new EasterHoliday(50,
	            "Whit Monday");
	
	    /// <summary>
	    /// Corpus Christi, 60 days after Easter
	    /// </summary>
	    ///
	    /// @draft ICU 2.8
	    /// @provisional This API might change or be removed in a future release.
	    static public readonly EasterHoliday CORPUS_CHRISTI = new EasterHoliday(60,
	            "Corpus Christi");
	}
	
	internal class EasterRule : DateRule {
	    public EasterRule(int daysAfterEaster, bool isOrthodox) {
	        this.calendar = gregorian;
	        this.daysAfterEaster = daysAfterEaster;
	        if (isOrthodox) {
	           // TODO:  orthodox.SetGregorianChange(new DateTime((Int64.MaxValue)*10000));
	            calendar = orthodox;
	        }
	    }
	
	    /// <summary>
	    /// Return the first occurrance of this rule on or after the given date
	    /// </summary>
	    ///
	    public virtual DateTime FirstAfter(DateTime start) {
	        return DoFirstBetween(start, default(DateTime));
	    }
	
	    /// <summary>
	    /// Return the first occurrance of this rule on or after the given start date
	    /// and before the given end date.
	    /// </summary>
	    ///
	    public virtual DateTime FirstBetween(DateTime start, DateTime end) {
	        return DoFirstBetween(start, end);
	    }
	
	    /// <summary>
	    /// Return true if the given Date is on the same day as Easter
	    /// </summary>
	    ///
	    public virtual bool IsOn(DateTime date) {
	         lock (calendar) {
	                    calendar.SetTime(date);
	                    int dayOfYear = calendar.Get(IBM.ICU.Util.Calendar.DAY_OF_YEAR);
	        
	                    calendar.SetTime(ComputeInYear(calendar.GetTime(), calendar));
	        
	                    return calendar.Get(IBM.ICU.Util.Calendar.DAY_OF_YEAR) == dayOfYear;
	                }
	    }
	
	    /// <summary>
	    /// Return true if Easter occurs between the two dates given
	    /// </summary>
	    ///
	    public virtual bool IsBetween(DateTime start, DateTime end) {
	        return FirstBetween(start, end) != null; // TODO: optimize?
	    }
	
	    private DateTime DoFirstBetween(DateTime start, DateTime end) {
	        // System.out.println("doFirstBetween: start   = " + start.toString());
	        // System.out.println("doFirstBetween: end     = " + end.toString());
	
	         lock (calendar) {
	                    // Figure out when this holiday lands in the given year
	                    DateTime result = ComputeInYear(start, calendar);
	        
	                    // System.out.println("                result  = " +
	                    // result.toString());
	        
	                    // We might have gotten a date that's in the same year as "start",
	                    // but
	                    // earlier in the year. If so, go to next year
	                    if (ILOG.J2CsMapping.Util.DateUtil.Before(result,start)) {
	                        calendar.SetTime(start);
	                        calendar.Get(IBM.ICU.Util.Calendar.YEAR); // JDK 1.1.2 bug workaround
	                        calendar.Add(IBM.ICU.Util.Calendar.YEAR, 1);
	        
	                        // System.out.println("                Result before start, going to next year: "
	                        // + calendar.getTime().toString());
	        
	                        result = ComputeInYear(calendar.GetTime(), calendar);
	                        // System.out.println("                result  = " +
	                        // result.toString());
	                    }
	        
	                    if (end != null && ILOG.J2CsMapping.Util.DateUtil.After(result,end)) {
	                        // System.out.println("Result after end, returning null");
	                        return default(DateTime);
	                    }
	                    return result;
	                }
	    }
	
	    /// <summary>
	    /// Compute the month and date on which this holiday falls in the year
	    /// containing the date "date". First figure out which date Easter lands on
	    /// in this year, and then add the offset for this holiday to get the right
	    /// date.
	    /// <p>
	    /// The algorithm here is taken from the <a
	    /// href="http://www.faqs.org/faqs/calendars/faq/">Calendar FAQ</a>.
	    /// </summary>
	    ///
	    private DateTime ComputeInYear(DateTime date, GregorianCalendar cal) {
	        if (cal == null)
	            cal = calendar;
	
	         lock (cal) {
	                    cal.SetTime(date);
	        
	                    int year = cal.Get(IBM.ICU.Util.Calendar.YEAR);
	                    int g = year % 19; // "Golden Number" of year - 1
	                    int i = 0; // # of days from 3/21 to the Paschal full moon
	                    int j = 0; // Weekday (0-based) of Paschal full moon
	        
	                    if (ILOG.J2CsMapping.Util.DateUtil.After(cal.GetTime(),cal.GetGregorianChange())) {
	                        // We're past the Gregorian switchover, so use the Gregorian
	                        // rules.
	                        int c = year / 100;
	                        int h = (c - c / 4 - (8 * c + 13) / 25 + 19 * g + 15) % 30;
	                        i = h - (h / 28)
	                                * (1 - (h / 28) * (29 / (h + 1)) * ((21 - g) / 11));
	                        j = (year + year / 4 + i + 2 - c + c / 4) % 7;
	                    } else {
	                        // Use the old Julian rules.
	                        i = (19 * g + 15) % 30;
	                        j = (year + year / 4 + i) % 7;
	                    }
	                    int l = i - j;
	                    int m = 3 + (l + 40) / 44; // 1-based month in which Easter falls
	                    int d = l + 28 - 31 * (m / 4); // Date of Easter within that month
	        
	                    cal.Clear();
	                    cal.Set(IBM.ICU.Util.Calendar.ERA, IBM.ICU.Util.GregorianCalendar.AD);
	                    cal.Set(IBM.ICU.Util.Calendar.YEAR, year);
	                    cal.Set(IBM.ICU.Util.Calendar.MONTH, m - 1); // 0-based
	                    cal.Set(IBM.ICU.Util.Calendar.DATE, d);
	                    cal.GetTime(); // JDK 1.1.2 bug workaround
	                    cal.Add(IBM.ICU.Util.Calendar.DATE, daysAfterEaster);
	        
	                    return cal.GetTime();
	                }
	    }
	
	    private static GregorianCalendar gregorian = new GregorianCalendar(/*
	                                                                        * new
	                                                                        * SimpleTimeZone
	                                                                        * (0,
	                                                                        * "UTC")
	                                                                        */);
	
	    private static GregorianCalendar orthodox = new GregorianCalendar(/*
	                                                                       * new
	                                                                       * SimpleTimeZone
	                                                                       * (0,
	                                                                       * "UTC")
	                                                                       */);
	
	    private int daysAfterEaster;
	
	    private GregorianCalendar calendar;
	}
}