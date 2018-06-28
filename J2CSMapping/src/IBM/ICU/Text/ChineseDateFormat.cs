//##header J2SE15
// --------------------------------------------------------------------------------------------------
// This file was automatically generated by J2CS Translator (http://j2cstranslator.sourceforge.net/). 
// Version 1.3.6.20101125_01     
// 12/2/10 11:30 AM    
// ${CustomMessageForDisclaimer}                                                                             
// --------------------------------------------------------------------------------------------------
 /// <summary>
/// Copyright (C) 2000-2007, International Business Machines Corporation and
/// others. All Rights Reserved.
/// </summary>
///
namespace IBM.ICU.Text {
	
	using IBM.ICU.Util;
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Globalization;
	using System.IO;
	using System.Runtime.CompilerServices;
	using System.Text;
    using ILOG.J2CsMapping.Util;
    using ILOG.J2CsMapping.Text;
	
	/// <summary>
	/// A concrete <see cref="T:IBM.ICU.Text.DateFormat"/> for <see cref="T:IBM.ICU.Text.ChineseCalendar"/>.
	/// This class handles a <c>ChineseCalendar</c>-specific field,
	/// <c>ChineseCalendar.IS_LEAP_MONTH</c>. It also redefines the handling of
	/// two fields, <c>ERA</c> and <c>YEAR</c>. The former is displayed
	/// numerically, instead of symbolically, since it is the numeric cycle number in
	/// <c>ChineseCalendar</c>. The latter is numeric, as before, but has no
	/// special 2-digit Y2K behavior.
	/// <p>
	/// With regard to <c>ChineseCalendar.IS_LEAP_MONTH</c>, this class handles
	/// parsing specially. If no string symbol is found at all, this is taken as
	/// equivalent to an <c>IS_LEAP_MONTH</c> value of zero. This allows
	/// formats to display a special string (e.g., "///") for leap months, but no
	/// string for normal months.
	/// <p>
	/// Summary of field changes vs. <see cref="T:IBM.ICU.Text.SimpleDateFormat"/>:
	/// <pre>
	/// Symbol   Meaning                 Presentation        Example
	/// ------   -------                 ------------        -------
	/// G        cycle                   (Number)            78
	/// y        year of cycle (1..60)   (Number)            17
	/// l        is leap month           (Text)              4637
	/// </pre>
	/// </summary>
	///
	/// <seealso cref="T:IBM.ICU.Text.ChineseCalendar"/>
	/// <seealso cref="T:IBM.ICU.Text.ChineseDateFormatSymbols"/>
	/// @stable ICU 2.0
	public class ChineseDateFormat : SimpleDateFormat {
	    // Generated by serialver from JDK 1.4.1_01
	    internal const long serialVersionUID = -4610300753104099899L;
	
	    // TODO Finish the constructors
	
	    /// <summary>
	    /// Construct a ChineseDateFormat from a date format pattern and locale
	    /// </summary>
	    ///
	    /// <param name="pattern">the pattern</param>
	    /// <param name="locale">the locale</param>
	    /// @stable ICU 2.0
        public ChineseDateFormat(String pattern, Locale locale)
            : this(pattern, IBM.ICU.Util.ULocale.ForLocale(locale))
        {
	    }
	
	    /// <summary>
	    /// Construct a ChineseDateFormat from a date format pattern and locale
	    /// </summary>
	    ///
	    /// <param name="pattern">the pattern</param>
	    /// <param name="locale">the locale</param>
	    /// @stable ICU 3.2
	    public ChineseDateFormat(String pattern, ULocale locale) : base(pattern, new ChineseDateFormatSymbols(locale), new ChineseCalendar(IBM.ICU.Util.TimeZone.GetDefault(),locale), locale, true) {
	    }
	
	    // NOTE: This API still exists; we just inherit it from SimpleDateFormat
	    // as of ICU 3.0
	    // /**
	    // * @stable ICU 2.0
	    // */
	    // protected String subFormat(char ch, int count, int beginOffset,
	    // FieldPosition pos, DateFormatSymbols formatData,
	    // Calendar cal) {
	    // switch (ch) {
	    // case 'G': // 'G' - ERA
	    // return zeroPaddingNumber(cal.get(Calendar.ERA), 1, 9);
	    // case 'l': // 'l' - IS_LEAP_MONTH
	    // {
	    // ChineseDateFormatSymbols symbols =
	    // (ChineseDateFormatSymbols) formatData;
	    // return symbols.getLeapMonth(cal.get(
	    // ChineseCalendar.IS_LEAP_MONTH));
	    // }
	    // default:
	    // return super.subFormat(ch, count, beginOffset, pos, formatData, cal);
	    // }
	    // }
	
	    /// <exclude/>
	    /// <summary>
	    /// 
	    /// </summary>
	    ///
	    protected internal override void SubFormat(StringBuilder buf, char ch, int count,
	            int beginOffset, FieldPosition pos, IBM.ICU.Util.Calendar cal) {
	        switch ((int) ch) {
	        case 'G': // 'G' - ERA
	            ZeroPaddingNumber(buf, cal.Get(IBM.ICU.Util.Calendar.ERA), 1, 9);
	            break;
	        case 'l': // 'l' - IS_LEAP_MONTH
	            buf.Append(((ChineseDateFormatSymbols) GetSymbols())
	                    .GetLeapMonth(cal.Get(IBM.ICU.Util.ChineseCalendar.IS_LEAP_MONTH)));
	            break;
	        default:
	            base.SubFormat(buf,ch,count,beginOffset,pos,cal);
	            break;
	        }
	
	        // TODO: add code to set FieldPosition for 'G' and 'l' fields. This
	        // is a DESIGN FLAW -- subclasses shouldn't have to duplicate the
	        // code that handles this at the end of SimpleDateFormat.subFormat.
	        // The logic should be moved up into SimpleDateFormat.format.
	    }
	
	    /// <summary>
	    /// 
	    /// </summary>
	    ///
	    /// @stable ICU 2.0
	    protected internal override int SubParse(String text, int start, char ch, int count,
	            bool obeyCount, bool allowNegative, bool[] ambiguousYear,
	            IBM.ICU.Util.Calendar cal) {
	        if (ch != 'G' && ch != 'l' && ch != 'y') {
	            return base.SubParse(text,start,ch,count,obeyCount,allowNegative,ambiguousYear,cal);
	        }
	
	        // Skip whitespace
	        start = IBM.ICU.Impl.Utility.SkipWhitespace(text, start);
	
	        ParsePosition pos = new ParsePosition(start);
	
	        switch ((int) ch) {
	        case 'G': // 'G' - ERA
	        case 'y': // 'y' - YEAR, but without the 2-digit Y2K adjustment
	        {
	            object number = null;
	            if (obeyCount) {
	                if ((start + count) > text.Length) {
	                    return -start;
	                }
	                number = numberFormat.Parse(text.Substring(0,(start + count)-(0)),
	                        pos);
	            } else {
	                number = numberFormat.Parse(text, pos);
	            }
	            if (number == null) {
	                return -start;
	            }
	            int value_ren = System.Convert.ToInt32(number);
	            cal.Set((ch == 'G') ? IBM.ICU.Util.Calendar.ERA : IBM.ICU.Util.Calendar.YEAR, value_ren);
	            return pos.GetIndex();
	        }
	        case 'l': // 'l' - IS_LEAP_MONTH
	        {
	            ChineseDateFormatSymbols symbols = (ChineseDateFormatSymbols) GetSymbols();
	            int result = MatchString(text, start,
	                    IBM.ICU.Util.ChineseCalendar.IS_LEAP_MONTH, symbols.isLeapMonth, cal);
	            // Treat the absence of any matching string as setting
	            // IS_LEAP_MONTH to false.
	            if (result < 0) {
	                cal.Set(IBM.ICU.Util.ChineseCalendar.IS_LEAP_MONTH, 0);
	                result = start;
	            }
	            return result;
	        }
	        default:
	            // /CLOVER:OFF
	            return 0; // This can never happen
	            // /CLOVER:ON
	        }
	    }
	
	    // #if defined(FOUNDATION10) || defined(J2SE13)
	    // #else
	    /// <summary>
	    /// 
	    /// </summary>
	    ///
	    /// @draft ICU 3.8
	    /// @provisional This API might change or be removed in a future release.
	    protected internal override DateFormat.Field PatternCharToDateFormatField(char ch) {
	        if (ch == 'l') {
	            return IBM.ICU.Text.ChineseDateFormat.Field.IS_LEAP_MONTH;
	        }
	        return base.PatternCharToDateFormatField(ch);
	    }
	
	    /// <summary>
	    /// The instances of this inner class are used as attribute keys and values
	    /// in AttributedCharacterIterator that
	    /// ChineseDateFormat.formatToCharacterIterator() method returns.
	    /// <p>
	    /// There is no public constructor to this class, the only instances are the
	    /// constants defined here.
	    /// <p>
	    /// </summary>
	    ///
	    /// @stable ICU 3.8
	    public class Field : DateFormat.Field {
	
	        private const long serialVersionUID = -5102130532751400330L;
	
	        /// <summary>
	        /// Constant identifying the leap month marker.
	        /// </summary>
	        ///
	        /// @stable ICU 3.8
	        public static readonly ChineseDateFormat.Field  IS_LEAP_MONTH = new ChineseDateFormat.Field ("is leap month",
	                IBM.ICU.Util.ChineseCalendar.IS_LEAP_MONTH);
	
	        /// <summary>
	        /// Constructs a <c>ChineseDateFormat.Field</c> with the given name
	        /// and the <c>ChineseCalendar</c> field which this attribute
	        /// represents. Use -1 for <c>calendarField</c> if this field does
	        /// not have a corresponding <c>ChineseCalendar</c> field.
	        /// </summary>
	        ///
	        /// <param name="name">Name of the attribute</param>
	        /// <param name="calendarField"><c>Calendar</c> field constant</param>
	        /// @stable ICU 3.8
	        protected internal Field(String name, int calendarField) : base(name, calendarField) {
	        }
	
	        /// <summary>
	        /// Returns the <c>Field</c> constant that corresponds to the
	        /// <code>
	        /// ChineseCalendar</code> field <c>calendarField</c>. If there is
	        /// no corresponding <c>Field</c> is available, null is returned.
	        /// </summary>
	        ///
	        /// <param name="calendarField"><c>ChineseCalendar</c> field constant</param>
	        /// <returns><c>Field</c> associated with the
	        /// <c>calendarField</c>, or null if no associated
	        /// <c>Field</c> is available.</returns>
	        /// <exception cref="IllegalArgumentException">if <c>calendarField</c> is not a valid<c>Calendar</c> field constant.</exception>
	        /// @stable ICU 3.8
	        public static DateFormat.Field OfCalendarField(int calendarField) {
	            if (calendarField == IBM.ICU.Util.ChineseCalendar.IS_LEAP_MONTH) {
	                return IS_LEAP_MONTH;
	            }
	            return IBM.ICU.Text.DateFormat.Field.OfCalendarField(calendarField);
	        }
	
	        /// <summary>
	        /// 
	        /// </summary>
	        ///
	        /// @stable ICU 3.8
	        protected internal override Object ReadResolve() {
	            if ((Object) this.GetType() != (Object) typeof(ChineseDateFormat.Field)) {
	                throw new IOException(
	                        "A subclass of ChineseDateFormat.Field must implement readResolve.");
	            }
	            if (this.GetName().Equals(IS_LEAP_MONTH.GetName())) {
	                return IS_LEAP_MONTH;
	            } else {
	                throw new IOException("Unknown attribute name.");
	            }
	        }
	    }
	    // #endif
	}
}
