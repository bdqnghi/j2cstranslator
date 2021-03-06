/*
 *******************************************************************************
 * Copyright (C) 1996-2005, International Business Machines Corporation and    *
 * others. All Rights Reserved.                                                *
 *******************************************************************************
 */

// --------------------------------------------------------------------------------------------------
// This file was automatically generated by J2CS Translator (http://j2cstranslator.sourceforge.net/). 
// Version 1.3.6.20101125_01     
// 12/8/10 10:24 AM    
// ${CustomMessageForDisclaimer}                                                                             
// --------------------------------------------------------------------------------------------------
 namespace IBM.ICU.Impl.Data {
	
	using IBM.ICU.Util;
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.IO;
	using System.Runtime.CompilerServices;
	
	public class HolidayBundle_en_US : ILOG.J2CsMapping.Util.ListResourceBundle {
	    static private readonly Holiday[] fHolidays = {
	            IBM.ICU.Util.SimpleHoliday.NEW_YEARS_DAY,
	            new SimpleHoliday(Calendar.JANUARY,   15, Calendar.MONDAY,      "Martin Luther King Day",   1986),

        new SimpleHoliday(Calendar.FEBRUARY,  15, Calendar.MONDAY,      "Presidents' Day",          1976),
        new SimpleHoliday(Calendar.FEBRUARY,  22,                       "Washington's Birthday",    1776, 1975),

        EasterHoliday.GOOD_FRIDAY,
        EasterHoliday.EASTER_SUNDAY,

        new SimpleHoliday(Calendar.MAY,        8, Calendar.SUNDAY,      "Mother's Day",             1914),

        new SimpleHoliday(Calendar.MAY,       31, -Calendar.MONDAY,     "Memorial Day",             1971),
        new SimpleHoliday(Calendar.MAY,       30,                       "Memorial Day",             1868, 1970),

        new SimpleHoliday(Calendar.JUNE,      15, Calendar.SUNDAY,      "Father's Day",             1956),
        new SimpleHoliday(Calendar.JULY,       4,                       "Independence Day",         1776),
        new SimpleHoliday(Calendar.SEPTEMBER,  1, Calendar.MONDAY,      "Labor Day",                1894),
        new SimpleHoliday(Calendar.NOVEMBER,   2, Calendar.TUESDAY,     "Election Day"),
        new SimpleHoliday(Calendar.OCTOBER,    8, Calendar.MONDAY,      "Columbus Day",             1971),
        new SimpleHoliday(Calendar.OCTOBER ,  31,                       "Halloween"),
        new SimpleHoliday(Calendar.NOVEMBER,  11,                       "Veterans' Day",            1918),
        new SimpleHoliday(Calendar.NOVEMBER,  22, Calendar.THURSDAY,    "Thanksgiving",             1863),

	
	            IBM.ICU.Util.SimpleHoliday.CHRISTMAS, };
	
	    static private readonly Object[][] fContents = { new Object[] { "holidays", fHolidays } };
	
	    [MethodImpl(MethodImplOptions.Synchronized)]
	    public override Object[][] GetContents() {
	        return fContents;
	    }
	}
}
