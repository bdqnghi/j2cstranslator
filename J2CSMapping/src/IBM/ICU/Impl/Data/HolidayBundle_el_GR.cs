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
	using ILOG.J2CsMapping.Util;
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.IO;
	using System.Runtime.CompilerServices;
	
	public class HolidayBundle_el_GR : ListResourceBundle {
	    static private readonly Holiday[] fHolidays = { IBM.ICU.Util.SimpleHoliday.NEW_YEARS_DAY,
	            IBM.ICU.Util.SimpleHoliday.EPIPHANY,
	
	              new SimpleHoliday(IBM.ICU.Util.Calendar.MARCH,     25,  0,    "Independence Day"),

        SimpleHoliday.MAY_DAY,
        SimpleHoliday.ASSUMPTION,

        new SimpleHoliday(IBM.ICU.Util.Calendar.OCTOBER,   28,  0,    "Ochi Day"),
	
	            IBM.ICU.Util.SimpleHoliday.CHRISTMAS,
	            IBM.ICU.Util.SimpleHoliday.BOXING_DAY,
	
	            // Easter and related holidays in the Orthodox calendar
	            new EasterHoliday(-2, true, "Good Friday"),
	            new EasterHoliday(0, true, "Easter Sunday"),
	            new EasterHoliday(1, true, "Easter Monday"),
	            new EasterHoliday(50, true, "Whit Monday"), };
	
	    static private readonly Object[][] fContents = { new Object[] { "holidays", fHolidays } };
	
	    [MethodImpl(MethodImplOptions.Synchronized)]
	    public override Object[][] GetContents() {
	        return fContents;
	    }
	}
}
