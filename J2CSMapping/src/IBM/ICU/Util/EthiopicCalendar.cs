/*
 *******************************************************************************
 * Copyright (C) 2005-2007, International Business Machines Corporation and    *
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
	/// Implement the Ethiopic calendar system.
	/// <p>
	/// EthiopicCalendar usually should be instantiated using<see cref="M:IBM.ICU.Util.Calendar.GetInstance(IBM.ICU.Util.ULocale)"/> passing in a
	/// <c>ULocale</c> with the tag <c>"@calendar=ethiopic"</c>.
	/// </p>
	/// </summary>
	///
	/// <seealso cref="T:IBM.ICU.Util.Calendar"/>
	/// @stable ICU 3.4
	public sealed class EthiopicCalendar : CECalendar {
	    // jdk1.4.2 serialver
	    private const long serialVersionUID = -2438495771339315608L;
	
	    /// <summary>
	    /// Constant for &#x1218;&#x1235;&#x12a8;&#x1228;&#x121d;, the 1st month of
	    /// the Ethiopic year.
	    /// </summary>
	    ///
	    /// @stable ICU 3.4
	    public const int MESKEREM = 0;
	
	    /// <summary>
	    /// Constant for &#x1325;&#x1245;&#x121d;&#x1275;, the 2nd month of the
	    /// Ethiopic year.
	    /// </summary>
	    ///
	    /// @stable ICU 3.4
	    public const int TEKEMT = 1;
	
	    /// <summary>
	    /// Constant for &#x1285;&#x12f3;&#x122d;, the 3rd month of the Ethiopic
	    /// year.
	    /// </summary>
	    ///
	    /// @stable ICU 3.4
	    public const int HEDAR = 2;
	
	    /// <summary>
	    /// Constant for &#x1273;&#x1285;&#x1223;&#x1225;, the 4th month of the
	    /// Ethiopic year.
	    /// </summary>
	    ///
	    /// @stable ICU 3.4
	    public const int TAHSAS = 3;
	
	    /// <summary>
	    /// Constant for &#x1325;&#x122d;, the 5th month of the Ethiopic year.
	    /// </summary>
	    ///
	    /// @stable ICU 3.4
	    public const int TER = 4;
	
	    /// <summary>
	    /// Constant for &#x12e8;&#x12ab;&#x1272;&#x1275;, the 6th month of the
	    /// Ethiopic year.
	    /// </summary>
	    ///
	    /// @stable ICU 3.4
	    public const int YEKATIT = 5;
	
	    /// <summary>
	    /// Constant for &#x1218;&#x130b;&#x1262;&#x1275;, the 7th month of the
	    /// Ethiopic year.
	    /// </summary>
	    ///
	    /// @stable ICU 3.4
	    public const int MEGABIT = 6;
	
	    /// <summary>
	    /// Constant for &#x121a;&#x12eb;&#x12dd;&#x12eb;, the 8th month of the
	    /// Ethiopic year.
	    /// </summary>
	    ///
	    /// @stable ICU 3.4
	    public const int MIAZIA = 7;
	
	    /// <summary>
	    /// Constant for &#x130d;&#x1295;&#x1266;&#x1275;, the 9th month of the
	    /// Ethiopic year.
	    /// </summary>
	    ///
	    /// @stable ICU 3.4
	    public const int GENBOT = 8;
	
	    /// <summary>
	    /// Constant for &#x1230;&#x1294;, the 10th month of the Ethiopic year.
	    /// </summary>
	    ///
	    /// @stable ICU 3.4
	    public const int SENE = 9;
	
	    /// <summary>
	    /// Constant for &#x1210;&#x121d;&#x120c;, the 11th month of the Ethiopic
	    /// year.
	    /// </summary>
	    ///
	    /// @stable ICU 3.4
	    public const int HAMLE = 10;
	
	    /// <summary>
	    /// Constant for &#x1290;&#x1210;&#x1234;, the 12th month of the Ethiopic
	    /// year.
	    /// </summary>
	    ///
	    /// @stable ICU 3.4
	    public const int NEHASSE = 11;
	
	    /// <summary>
	    /// Constant for &#x1333;&#x1309;&#x121c;&#x1295;, the 13th month of the
	    /// Ethiopic year.
	    /// </summary>
	    ///
	    /// @stable ICU 3.4
	    public const int PAGUMEN = 12;
	
	    // Up until the end of the 19th century the prevailant convention was to
	    // reference the Ethiopic Calendar from the creation of the world,
	    // \u12d3\u1218\u1270\u1361\u12d3\u1208\u121d
	    // (Amete Alem 5500 BC). As Ethiopia modernized the reference epoch from
	    // the birth of Christ (\u12d3\u1218\u1270\u1361\u121d\u1215\u1228\u1275)
	    // began to displace the creation of the
	    // world reference point. However, years before the birth of Christ are
	    // still referenced in the creation of the world system.
	    // Thus -100 \u12d3/\u121d
	    // would be rendered as 5400 \u12d3/\u12d3.
	    //
	    // The creation of the world in Ethiopic cannon was
	    // Meskerem 1, -5500 \u12d3/\u121d 00:00:00
	    // applying the birth of Christ reference and Ethiopian time conventions.
	    // This is
	    // 6 hours less than the Julian epoch reference point (noon). In Gregorian
	    // the date and time was July 18th -5493 BC 06:00 AM.
	
	    // Julian Days relative to the
	    // \u12d3\u1218\u1270\u1361\u121d\u1215\u1228\u1275 epoch
	    private const int JD_EPOCH_OFFSET_AMETE_ALEM = -285019;
	
	    // Julian Days relative to the
	    // \u12d3\u1218\u1270\u1361\u12d3\u1208\u121d epoch
	    private const int JD_EPOCH_OFFSET_AMETE_MIHRET = 1723856;
	
	    /// <summary>
	    /// Constructs a default <c>EthiopicCalendar</c> using the current time
	    /// in the default time zone with the default locale.
	    /// </summary>
	    ///
	    /// @stable ICU 3.4
	    public EthiopicCalendar() : base() {
	        jdEpochOffset = JD_EPOCH_OFFSET_AMETE_MIHRET;
	    }
	
	    /// <summary>
	    /// Constructs a <c>EthiopicCalendar</c> based on the current time in
	    /// the given time zone with the default locale.
	    /// </summary>
	    ///
	    /// <param name="zone">The time zone for the new calendar.</param>
	    /// @stable ICU 3.4
	    public EthiopicCalendar(TimeZone zone) : base(zone) {
	        jdEpochOffset = JD_EPOCH_OFFSET_AMETE_MIHRET;
	    }
	
	    /// <summary>
	    /// Constructs a <c>EthiopicCalendar</c> based on the current time in
	    /// the default time zone with the given locale.
	    /// </summary>
	    ///
	    /// <param name="aLocale">The locale for the new calendar.</param>
	    /// @stable ICU 3.4
        public EthiopicCalendar(ILOG.J2CsMapping.Util.Locale aLocale)
            : base(aLocale)
        {
	        jdEpochOffset = JD_EPOCH_OFFSET_AMETE_MIHRET;
	    }
	
	    /// <summary>
	    /// Constructs a <c>EthiopicCalendar</c> based on the current time in
	    /// the default time zone with the given locale.
	    /// </summary>
	    ///
	    /// <param name="locale">The icu locale for the new calendar.</param>
	    /// @stable ICU 3.4
	    public EthiopicCalendar(ULocale locale) : base(locale) {
	        jdEpochOffset = JD_EPOCH_OFFSET_AMETE_MIHRET;
	    }
	
	    /// <summary>
	    /// Constructs a <c>EthiopicCalendar</c> based on the current time in
	    /// the given time zone with the given locale.
	    /// </summary>
	    ///
	    /// <param name="zone">The time zone for the new calendar.</param>
	    /// <param name="aLocale">The locale for the new calendar.</param>
	    /// @stable ICU 3.4
        public EthiopicCalendar(TimeZone zone, ILOG.J2CsMapping.Util.Locale aLocale)
            : base(zone, aLocale)
        {
	        jdEpochOffset = JD_EPOCH_OFFSET_AMETE_MIHRET;
	    }
	
	    /// <summary>
	    /// Constructs a <c>EthiopicCalendar</c> based on the current time in
	    /// the given time zone with the given locale.
	    /// </summary>
	    ///
	    /// <param name="zone">The time zone for the new calendar.</param>
	    /// <param name="locale">The icu locale for the new calendar.</param>
	    /// @stable ICU 3.4
	    public EthiopicCalendar(TimeZone zone, ULocale locale) : base(zone, locale) {
	        jdEpochOffset = JD_EPOCH_OFFSET_AMETE_MIHRET;
	    }
	
	    /// <summary>
	    /// Constructs a <c>EthiopicCalendar</c> with the given date set in the
	    /// default time zone with the default locale.
	    /// </summary>
	    ///
	    /// <param name="year">The value used to set the calendar's <see cref="M:IBM.ICU.Util.EthiopicCalendar.YEAR YEAR"/> timefield.</param>
	    /// <param name="month">The value used to set the calendar's <see cref="M:IBM.ICU.Util.EthiopicCalendar.MONTH MONTH"/> timefield. The value is 0-based. e.g., 0 for Meskerem.</param>
	    /// <param name="date">The value used to set the calendar's <see cref="M:IBM.ICU.Util.EthiopicCalendar.DATE DATE"/> timefield.</param>
	    /// @stable ICU 3.4
	    public EthiopicCalendar(int year, int month, int date) : base(year, month, date) {
	        jdEpochOffset = JD_EPOCH_OFFSET_AMETE_MIHRET;
	    }
	
	    /// <summary>
	    /// Constructs a <c>EthiopicCalendar</c> with the given date set in the
	    /// default time zone with the default locale.
	    /// </summary>
	    ///
	    /// <param name="date">The date to which the new calendar is set.</param>
	    /// @stable ICU 3.4
	    public EthiopicCalendar(DateTime date) : base(date) {
	        jdEpochOffset = JD_EPOCH_OFFSET_AMETE_MIHRET;
	    }
	
	    /// <summary>
	    /// Constructs a <c>EthiopicCalendar</c> with the given date and time
	    /// set for the default time zone with the default locale.
	    /// </summary>
	    ///
	    /// <param name="year">The value used to set the calendar's <see cref="M:IBM.ICU.Util.EthiopicCalendar.YEAR YEAR"/> timefield.</param>
	    /// <param name="month">The value used to set the calendar's <see cref="M:IBM.ICU.Util.EthiopicCalendar.MONTH MONTH"/> timefield. The value is 0-based. e.g., 0 for Meskerem.</param>
	    /// <param name="date">The value used to set the calendar's <see cref="M:IBM.ICU.Util.EthiopicCalendar.DATE DATE"/> timefield.</param>
	    /// <param name="hour">The value used to set the calendar's <see cref="M:IBM.ICU.Util.EthiopicCalendar.HOUR_OF_DAYHOUR_OF_DAY"/> time field.</param>
	    /// <param name="minute">The value used to set the calendar's <see cref="M:IBM.ICU.Util.EthiopicCalendar.MINUTE MINUTE"/>time field.</param>
	    /// <param name="second">The value used to set the calendar's <see cref="M:IBM.ICU.Util.EthiopicCalendar.SECOND SECOND"/>time field.</param>
	    /// @stable ICU 3.4
	    public EthiopicCalendar(int year, int month, int date, int hour,
	            int minute, int second) : base(year, month, date, hour, minute, second) {
	        jdEpochOffset = JD_EPOCH_OFFSET_AMETE_MIHRET;
	    }
	
	    /// <summary>
	    /// Convert an Ethiopic year, month, and day to a Julian day.
	    /// </summary>
	    ///
	    /// <param name="year">the year</param>
	    /// <param name="month">the month</param>
	    /// <param name="date">the day</param>
	    /// @draft ICU 3.4
	    /// @provisional This API might change or be removed in a future release.
	    public static int EthiopicToJD(long year, int month, int date) {
	        return IBM.ICU.Util.CECalendar.CeToJD(year, month, date, JD_EPOCH_OFFSET_AMETE_MIHRET);
	    }
	
	    /// <exclude/>
	    public static Int32[] GetDateFromJD(int julianDay) {
	        return IBM.ICU.Util.CECalendar.GetDateFromJD(julianDay, JD_EPOCH_OFFSET_AMETE_MIHRET);
	    }
	
	    /// <summary>
	    /// Set Alem or Mihret era.
	    /// </summary>
	    ///
	    /// <param name="onOff">Set Amete Alem era if true, otherwise set Amete Mihret era.</param>
	    /// @stable ICU 3.4
	    public void SetAmeteAlemEra(bool onOff) {
	        this.jdEpochOffset = (onOff) ? JD_EPOCH_OFFSET_AMETE_ALEM
	                : JD_EPOCH_OFFSET_AMETE_MIHRET;
	    }
	
	    /// <summary>
	    /// Return true if this calendar is set to the Amete Alem era.
	    /// </summary>
	    ///
	    /// <returns>true if set to the Amete Alem era.</returns>
	    /// @stable ICU 3.4
	    public bool IsAmeteAlemEra() {
	        return this.jdEpochOffset == JD_EPOCH_OFFSET_AMETE_ALEM;
	    }
	
	    /// <summary>
	    /// 
	    /// </summary>
	    ///
	    /// <returns>type of calendar</returns>
	    /// @draft ICU 3.8
	    public override String GetType() {
	        return "ethiopic";
	    }
	}
}
