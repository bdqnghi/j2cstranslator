/*
 *******************************************************************************
 * Copyright (C) 1996-2007, International Business Machines Corporation and    *
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
	
	using IBM.ICU.Text;
	using IBM.ICU.Util;
	using ILOG.J2CsMapping.Collections;
	using ILOG.J2CsMapping.Collections.Generics;
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.IO;
	using System.Runtime.CompilerServices;
	using System.Text;
     using NUnit.Framework;
     using ILOG.J2CsMapping.Text;
	
	/// <summary>
	/// A base class for classes that test individual Calendar subclasses. Defines
	/// various useful utility methods and constants
	/// </summary>
	///
	public class CalendarTest : TestFmwk {
	
	    // Constants for use by subclasses, solely to save typing
	    public const int SUN = IBM.ICU.Util.Calendar.SUNDAY;
	
	    public const int MON = IBM.ICU.Util.Calendar.MONDAY;
	
	    public const int TUE = IBM.ICU.Util.Calendar.TUESDAY;
	
	    public const int WED = IBM.ICU.Util.Calendar.WEDNESDAY;
	
	    public const int THU = IBM.ICU.Util.Calendar.THURSDAY;
	
	    public const int FRI = IBM.ICU.Util.Calendar.FRIDAY;
	
	    public const int SAT = IBM.ICU.Util.Calendar.SATURDAY;
	
	    public const int ERA = IBM.ICU.Util.Calendar.ERA;
	
	    public const int YEAR = IBM.ICU.Util.Calendar.YEAR;
	
	    public const int MONTH = IBM.ICU.Util.Calendar.MONTH;
	
	    public const int DATE = IBM.ICU.Util.Calendar.DATE;
	
	    public const int HOUR = IBM.ICU.Util.Calendar.HOUR;
	
	    public const int MINUTE = IBM.ICU.Util.Calendar.MINUTE;
	
	    public const int SECOND = IBM.ICU.Util.Calendar.SECOND;
	
	    public const int DOY = IBM.ICU.Util.Calendar.DAY_OF_YEAR;
	
	    public const int WOY = IBM.ICU.Util.Calendar.WEEK_OF_YEAR;
	
	    public const int WOM = IBM.ICU.Util.Calendar.WEEK_OF_MONTH;
	
	    public const int DOW = IBM.ICU.Util.Calendar.DAY_OF_WEEK;
	
	    public const int DOWM = IBM.ICU.Util.Calendar.DAY_OF_WEEK_IN_MONTH;
	
	    public static readonly SimpleTimeZone UTC = new SimpleTimeZone(0, "GMT");
	
	    private static readonly String[] FIELD_NAME = { "ERA", "YEAR", "MONTH",
	            "WEEK_OF_YEAR", "WEEK_OF_MONTH", "DAY_OF_MONTH", "DAY_OF_YEAR",
	            "DAY_OF_WEEK", "DAY_OF_WEEK_IN_MONTH", "AM_PM", "HOUR",
	            "HOUR_OF_DAY", "MINUTE", "SECOND", "MILLISECOND", "ZONE_OFFSET",
	            "DST_OFFSET", "YEAR_WOY", "DOW_LOCAL", "EXTENDED_YEAR",
	            "JULIAN_DAY", "MILLISECONDS_IN_DAY", "IS_LEAP_MONTH" // (ChineseCalendar
	                                                                 // only)
	    };
	
	    public static String FieldName(int f) {
	        return (f >= 0 && f < FIELD_NAME.Length) ? FIELD_NAME[f] : ("<Field "
	                + f + ">");
	    }
	
	    /// <summary>
	    /// Iterates through a list of calendar <c>TestCase</c> objects and
	    /// makes sure that the time-to-fields and fields-to-time calculations work
	    /// correnctly for the values in each test case.
	    /// </summary>
	    ///
	    public void DoTestCases(TestCase[] cases, Calendar cal) {
	        cal.SetTimeZone(UTC);
	
	        // Get a format to use for printing dates in the calendar system we're
	        // testing
            IBM.ICU.Text.DateFormat format = IBM.ICU.Text.DateFormat.GetDateTimeInstance(cal,
	                IBM.ICU.Text.DateFormat.SHORT, -1, ILOG.J2CsMapping.Util.Locale.GetDefault());
	
	        String pattern = (cal  is  ChineseCalendar) ? "E MMl/dd/y G HH:mm:ss.S z"
	                : "E, MM/dd/yyyy G HH:mm:ss.S z";

            ((IBM.ICU.Text.SimpleDateFormat)format).ApplyPattern(pattern);
	
	        // This format is used for printing Gregorian dates.
            IBM.ICU.Text.DateFormat gregFormat = new IBM.ICU.Text.SimpleDateFormat(pattern);
	        gregFormat.SetTimeZone(UTC);
	
	        GregorianCalendar pureGreg = new GregorianCalendar(UTC);
	        pureGreg.SetGregorianChange(ILOG.J2CsMapping.Util.DateUtil.DateFromJavaMillis(Int64.MinValue));
            IBM.ICU.Text.DateFormat pureGregFmt = new IBM.ICU.Text.SimpleDateFormat("E M/d/yyyy G");
	        pureGregFmt.SetCalendar(pureGreg);
	
	        // Now iterate through the test cases and see what happens
	        for (int i = 0; i < cases.Length; i++) {
	            Logln("\ntest case: " + i);
                //throw new NotImplementedException();
                
	            TestCase test = cases[i];
	
	            //
	            // First we want to make sure that the millis -> fields calculation
	            // works
	            // test.applyTime will call setTime() on the calendar object, and
	            // test.fieldsEqual will retrieve all of the field values and make
	            // sure
	            // that they're the same as the ones in the testcase
	            //
	            test.ApplyTime(cal);
	            if (!test.FieldsEqual(cal, this)) {
	                Errln("Fail: (millis=>fields) "
	                        + gregFormat.Format(test.GetTime()) + " => "
	                        + format.Format(cal.GetTime()) + ", expected " + test);
	            }
	
	            //
	            // If that was OK, check the fields -> millis calculation
	            // test.applyFields will set all of the calendar's fields to
	            // match those in the test case.
	            //
	            cal.Clear();
	            test.ApplyFields(cal);
	            if (!test.Equals(cal)) {
	                Errln("Fail: (fields=>millis) " + test + " => "
	                        + pureGregFmt.Format(cal.GetTime()) + ", expected "
	                        + pureGregFmt.Format(test.GetTime()));
	            }
	        }
	    }
	
	    public const bool ROLL = true;
	
	    public const bool ADD = false;
	
	    /// <summary>
	    /// Process test cases for <c>add</c> and <c>roll</c> methods.
	    /// Each test case is an array of integers, as follows:
	    /// <ul>
	    /// <li>0: input year
	    /// <li>1: month (zero-based)
	    /// <li>2: day
	    /// <li>3: field to roll or add to
	    /// <li>4: amount to roll or add
	    /// <li>5: result year
	    /// <li>6: month (zero-based)
	    /// <li>7: day
	    /// </ul>
	    /// For example:
	    /// <pre>
	    /// //       input                add by          output
	    /// //  year  month     day     field amount    year  month     day
	    /// {   5759, HESHVAN,   2,     MONTH,   1,     5759, KISLEV,    2 },
	    /// </pre>
	    /// </summary>
	    ///
	    /// <param name="roll"><c>true</c> or <c>ROLL</c> to test the<c>roll</c> method; <c>false</c> or<c>ADD</c> to test the <code>add</code method</param>
	    public void DoRollAdd(bool roll, Calendar cal, int[][] tests) {
	        String name = (roll) ? "rolling" : "adding";
	
	        for (int i = 0; i < tests.Length; i++) {
	            int[] test = tests[i];
	
	            cal.Clear();
	            if (cal  is  ChineseCalendar) {
	                cal.Set(IBM.ICU.Util.Calendar.EXTENDED_YEAR, test[0]);
	                cal.Set(IBM.ICU.Util.Calendar.MONTH, test[1]);
	                cal.Set(IBM.ICU.Util.Calendar.DAY_OF_MONTH, test[2]);
	            } else {
	                cal.Set(test[0], test[1], test[2]);
	            }
	            double day0 = GetJulianDay(cal);
	            if (roll) {
	                cal.Roll(test[3], test[4]);
	            } else {
	                cal.Add(test[3], test[4]);
	            }
	            int y = cal
	                    .Get((cal  is  ChineseCalendar) ? IBM.ICU.Util.Calendar.EXTENDED_YEAR
	                            : YEAR);
	            if (y != test[5] || cal.Get(MONTH) != test[6]
	                    || cal.Get(DATE) != test[7]) {
	                Errln("Fail: " + name + " "
	                        + YmdToString(test[0], test[1], test[2]) + " (" + day0
	                        + ")" + " " + FIELD_NAME[test[3]] + " by " + test[4]
	                        + ": expected "
	                        + YmdToString(test[5], test[6], test[7]) + ", got "
	                        + YmdToString(cal));
	            } else if (IsVerbose()) {
	                Logln("OK: " + name + " "
	                        + YmdToString(test[0], test[1], test[2]) + " (" + day0
	                        + ")" + " " + FIELD_NAME[test[3]] + " by " + test[4]
	                        + ": got " + YmdToString(cal));
	            }
	        }
	    }
	
	    /// <summary>
	    /// Test the functions getXxxMinimum() and getXxxMaximum() by marching a test
	    /// calendar 'cal' through 'numberOfDays' sequential days starting with
	    /// 'startDate'. For each date, read a field value along with its reported
	    /// actual minimum and actual maximum. These values are checked against one
	    /// another as well as against getMinimum(), getGreatestMinimum(),
	    /// getLeastMaximum(), and getMaximum(). We expect to see:
	    /// 1. minimum <= actualMinimum <= greatestMinimum <= leastMaximum <=
	    /// actualMaximum <= maximum
	    /// 2. actualMinimum <= value <= actualMaximum
	    /// Note: In addition to outright failures, this test reports some results as
	    /// warnings. These are not generally of concern, but they should be
	    /// evaluated by a human. To see these, run this test in verbose mode.
	    /// </summary>
	    ///
	    /// <param name="cal">the calendar to be tested</param>
	    /// <param name="fieldsToTest">an array of field values to be tested, e.g., new int[] {Calendar.MONTH, Calendar.DAY_OF_MONTH }. It only makes senseto test the day fields; the time fields are not tested by thismethod. If null, then test all standard fields.</param>
	    /// <param name="startDate">the first date to test</param>
	    /// <param name="testDuration">if positive, the number of days to be tested. If negative, thenumber of seconds to run the test.</param>
	    public void DoLimitsTest(Calendar cal, int[] fieldsToTest, DateTime startDate,
	            int testDuration) {
	        GregorianCalendar greg = new GregorianCalendar();
	        greg.SetTime(startDate);
	        Logln("Start: " + startDate);
	
	        if (fieldsToTest == null) {
	            fieldsToTest = new int[] { IBM.ICU.Util.Calendar.ERA, IBM.ICU.Util.Calendar.YEAR,
	                    IBM.ICU.Util.Calendar.MONTH, IBM.ICU.Util.Calendar.WEEK_OF_YEAR,
	                    IBM.ICU.Util.Calendar.WEEK_OF_MONTH, IBM.ICU.Util.Calendar.DAY_OF_MONTH,
	                    IBM.ICU.Util.Calendar.DAY_OF_YEAR, IBM.ICU.Util.Calendar.DAY_OF_WEEK_IN_MONTH,
	                    IBM.ICU.Util.Calendar.YEAR_WOY, IBM.ICU.Util.Calendar.EXTENDED_YEAR };
	        }
	
	        // Keep a record of minima and maxima that we actually see.
	        // These are kept in an array of arrays of hashes.
	        Hashtable[][] limits = (Hashtable[][])ILOG.J2CsMapping.Collections.Arrays.CreateJaggedArray(typeof(Hashtable), fieldsToTest.Length, 2);
	        Object nub = new Object(); // Meaningless placeholder
	
	        // This test can run for a long time; show progress.
	        long millis = DateTime.Now.Millisecond;
	        long mark = millis + 5000; // 5 sec
	        millis -= testDuration * 1000; // stop time if testDuration<0
	
	        for (int i = 0; (testDuration > 0) ? i < testDuration : DateTime.Now.Millisecond < millis; ++i) {
	            if (DateTime.Now.Millisecond >= mark) {
	                Logln("(" + i + " days)");
	                mark += 5000; // 5 sec
	            }
	            cal.SetTimeInMillis(greg.GetTimeInMillis());
	            for (int j = 0; j < fieldsToTest.Length; ++j) {
	                int f = fieldsToTest[j];
	                int v = cal.Get(f);
	                int minActual = cal.GetActualMinimum(f);
	                int maxActual = cal.GetActualMaximum(f);
	                int minLow = cal.GetMinimum(f);
	                int minHigh = cal.GetGreatestMinimum(f);
	                int maxLow = cal.GetLeastMaximum(f);
	                int maxHigh = cal.GetMaximum(f);
	
	                // Fetch the hash for this field and keep track of the
	                // minima and maxima.
	                Hashtable[] h = limits[j];
	                if (h[0] == null) {
	                    h[0] = new Hashtable();
	                    h[1] = new Hashtable();
	                }
	                ILOG.J2CsMapping.Collections.Collections.Put(h[0],((int)(minActual)),nub);
	                ILOG.J2CsMapping.Collections.Collections.Put(h[1],((int)(maxActual)),nub);
	
	                if (minActual < minLow || minActual > minHigh) {
	                    Errln("Fail: " + YmdToString(cal) + " Range for min of "
	                            + FIELD_NAME[f] + "=" + minLow + ".." + minHigh
	                            + ", actual_min=" + minActual);
	                }
	                if (maxActual < maxLow || maxActual > maxHigh) {
	                    Errln("Fail: " + YmdToString(cal) + " Range for max of "
	                            + FIELD_NAME[f] + "=" + maxLow + ".." + maxHigh
	                            + ", actual_max=" + maxActual);
	                }
	                if (v < minActual || v > maxActual) {
	                    Errln("Fail: " + YmdToString(cal) + " " + FIELD_NAME[f]
	                            + "=" + v + ", actual range=" + minActual + ".."
	                            + maxActual + ", allowed=(" + minLow + ".."
	                            + minHigh + ")..(" + maxLow + ".." + maxHigh + ")");
	                }
	            }
	            greg.Add(IBM.ICU.Util.Calendar.DAY_OF_YEAR, 1);
	        }
	
	        // Check actual maxima and minima seen against ranges returned
	        // by API.
	        StringBuilder buf = new StringBuilder();
	        for (int j_0 = 0; j_0 < fieldsToTest.Length; ++j_0) {
	            int f_1 = fieldsToTest[j_0];
	            buf.Length=0;
	            buf.Append(FIELD_NAME[f_1]);
	            Hashtable[] h_2 = limits[j_0];
	            bool fullRangeSeen = true;
	            for (int k = 0; k < 2; ++k) {
	                int rangeLow = (k == 0) ? cal.GetMinimum(f_1) : cal
	                        .GetLeastMaximum(f_1);
	                int rangeHigh = (k == 0) ? cal.GetGreatestMinimum(f_1) : cal
	                        .GetMaximum(f_1);
	                // If either the top of the range or the bottom was never
	                // seen, then there may be a problem.
	                if (h_2[k][((int)(rangeLow))] == null
	                        || h_2[k][((int)(rangeHigh))] == null) {
	                    fullRangeSeen = false;
	                }
	                buf.Append((k == 0) ? " minima seen=(" : "; maxima seen=(");
	                for (IIterator e = new ILOG.J2CsMapping.Collections.IteratorAdapter(h_2[k].Keys.GetEnumerator()); e.HasNext();) {
	                    int v_3 = ((Int32) e.Next());
	                    buf.Append(" " + v_3);
	                }
	                buf.Append(") range=" + rangeLow + ".." + rangeHigh);
	            }
	            if (fullRangeSeen) {
	                Logln("OK: " + buf.ToString());
	            } else {
	                // This may or may not be an error -- if the range of dates
	                // we scan over doesn't happen to contain a minimum or
	                // maximum, it doesn't mean some other range won't.
	                Logln("Warning: " + buf.ToString());
	            }
	        }
	
	        Logln("End: " + greg.GetTime());
	    }
	
	    /// <summary>
	    /// doLimitsTest with default test duration
	    /// </summary>
	    ///
	    public void DoLimitsTest(Calendar cal, int[] fieldsToTest, DateTime startDate) {
	        int testTime = (GetInclusion() <= 5) ? -3 : -60; // in seconds
	        DoLimitsTest(cal, fieldsToTest, startDate, testTime);
	    }
	
	    /// <summary>
	    /// Test the functions getMaximum/getGeratestMinimum logically correct. This
	    /// method assumes day of week cycle is consistent.
	    /// </summary>
	    ///
	    /// <param name="cal">The calendar instance to be tested.</param>
	    /// <param name="leapMonth">true if the calendar system has leap months</param>
	    public void DoTheoreticalLimitsTest(Calendar cal, bool leapMonth) {
	        int nDOW = cal.GetMaximum(IBM.ICU.Util.Calendar.DAY_OF_WEEK);
	        int maxDOY = cal.GetMaximum(IBM.ICU.Util.Calendar.DAY_OF_YEAR);
	        int lmaxDOW = cal.GetLeastMaximum(IBM.ICU.Util.Calendar.DAY_OF_YEAR);
	        int maxWOY = cal.GetMaximum(IBM.ICU.Util.Calendar.WEEK_OF_YEAR);
	        int lmaxWOY = cal.GetLeastMaximum(IBM.ICU.Util.Calendar.WEEK_OF_YEAR);
	        int maxM = cal.GetMaximum(IBM.ICU.Util.Calendar.MONTH) + 1;
	        int lmaxM = cal.GetLeastMaximum(IBM.ICU.Util.Calendar.MONTH) + 1;
	        int maxDOM = cal.GetMaximum(IBM.ICU.Util.Calendar.DAY_OF_MONTH);
	        int lmaxDOM = cal.GetLeastMaximum(IBM.ICU.Util.Calendar.DAY_OF_MONTH);
	        int maxDOWIM = cal.GetMaximum(IBM.ICU.Util.Calendar.DAY_OF_WEEK_IN_MONTH);
	        int lmaxDOWIM = cal.GetLeastMaximum(IBM.ICU.Util.Calendar.DAY_OF_WEEK_IN_MONTH);
	        int maxWOM = cal.GetMaximum(IBM.ICU.Util.Calendar.WEEK_OF_MONTH);
	        int lmaxWOM = cal.GetLeastMaximum(IBM.ICU.Util.Calendar.WEEK_OF_MONTH);
	
	        // Day of year
	        int expected;
	        if (!leapMonth) {
	            expected = maxM * maxDOM;
	            if (maxDOY > expected) {
	                Errln("FAIL: Maximum value of DAY_OF_YEAR is too big: "
	                        + maxDOY + "/expected: <=" + expected);
	            }
	            expected = lmaxM * lmaxDOM;
	            if (lmaxDOW < expected) {
	                Errln("FAIL: Least maximum value of DAY_OF_YEAR is too small: "
	                        + lmaxDOW + "/expected: >=" + expected);
	            }
	        }
	
	        // Week of year
	        expected = maxDOY / nDOW + 1;
	        if (maxWOY > expected) {
	            Errln("FAIL: Maximum value of WEEK_OF_YEAR is too big: " + maxWOY
	                    + "/expected: <=" + expected);
	        }
	        expected = lmaxDOW / nDOW;
	        if (lmaxWOY < expected) {
	            Errln("FAIL: Least maximum value of WEEK_OF_YEAR is too small: "
	                    + lmaxWOY + "/expected >=" + expected);
	        }
	
	        // Day of week in month
	        expected = (maxDOM + nDOW - 1) / nDOW;
	        if (maxDOWIM != expected) {
	            Errln("FAIL: Maximum value of DAY_OF_WEEK_IN_MONTH is incorrect: "
	                    + maxDOWIM + "/expected: " + expected);
	        }
	        expected = (lmaxDOM + nDOW - 1) / nDOW;
	        if (lmaxDOWIM != expected) {
	            Errln("FAIL: Least maximum value of DAY_OF_WEEK_IN_MONTH is incorrect: "
	                    + lmaxDOWIM + "/expected: " + expected);
	        }
	
	        // Week of month
	        expected = (maxDOM + nDOW - 2) / nDOW + 1;
	        if (maxWOM != expected) {
	            Errln("FAIL: Maximum value of WEEK_OF_MONTH is incorrect: "
	                    + maxWOM + "/expected: " + expected);
	        }
	        expected = lmaxDOM / nDOW;
	        if (lmaxWOM != expected) {
	            Errln("FAIL: Least maximum value of WEEK_OF_MONTH is incorrect: "
	                    + lmaxWOM + "/expected: " + expected);
	        }
	    }
	
	    /// <summary>
	    /// Convert year,month,day values to the form "year/month/day". On input the
	    /// month value is zero-based, but in the result string it is one-based.
	    /// </summary>
	    ///
	    static public String YmdToString(int year, int month, int day) {
	        return "" + year + "/" + (month + 1) + "/" + day;
	    }
	
	    /// <summary>
	    /// Convert year,month,day values to the form "year/month/day".
	    /// </summary>
	    ///
	    static public String YmdToString(Calendar cal) {
	        double day = GetJulianDay(cal);
	        if (cal  is  ChineseCalendar) {
	            return ""
	                    + cal.Get(IBM.ICU.Util.Calendar.EXTENDED_YEAR)
	                    + "/"
	                    + (cal.Get(IBM.ICU.Util.Calendar.MONTH) + 1)
	                    + ((cal.Get(IBM.ICU.Util.ChineseCalendar.IS_LEAP_MONTH) == 1) ? "(leap)"
	                            : "") + "/" + cal.Get(IBM.ICU.Util.Calendar.DATE) + " (" + day
	                    + ")";
	        }
	        return YmdToString(cal.Get(IBM.ICU.Util.Calendar.EXTENDED_YEAR), cal.Get(MONTH),
	                cal.Get(DATE))
	                + " (" + day + ")";
	    }
	
	    static internal double GetJulianDay(Calendar cal) {
	        return ((cal.GetTime().Ticks/10000) - JULIAN_EPOCH) / DAY_MS;
	    }
	
	    internal const double DAY_MS = 24 * 60 * 60 * 1000.0d;
	
	    internal const long JULIAN_EPOCH = -210866760000000L; // 1/1/4713 BC 12:00
	}
}
