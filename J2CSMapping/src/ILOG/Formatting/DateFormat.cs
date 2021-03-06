/* 
 * Licensed to the Apache Software Foundation (ASF) under one or more
 * contributor license agreements.  See the NOTICE file distributed with
 * this work for additional information regarding copyright ownership.
 * The ASF licenses this file to You under the Apache License, Version 2.0
 * (the "License"); you may not use this file except in compliance with
 * the License.  You may obtain a copy of the License at
 * 
 *     http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

// --------------------------------------------------------------------------------------------------
// This file was automatically generated by J2CS Translator (http://j2cstranslator.sourceforge.net/). 
// Version 1.3.6.20101125_01     
// 12/1/10 3:36 PM    
// ${CustomMessageForDisclaimer}                                                                             
// --------------------------------------------------------------------------------------------------
namespace ILOG.J2CsMapping.Formatting
{

    using ILOG.J2CsMapping.Util;
    using ILOG.J2CsMapping.Text;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Globalization;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Text;

    /// <summary>
    /// DateFormat is the abstract superclass of formats which format and parse
    /// Dates.
    /// </summary>
    ///
    public abstract class DateFormat : Format
    {

        private const long serialVersionUID = 7218322306649953788L;

        protected internal IBM.ICU.Util.Calendar calendar;

        protected internal NumberFormat numberFormat;

        /// <summary>
        /// Format style constant.
        /// </summary>
        ///
        public const int DEFAULT = 2;

        /// <summary>
        /// Format style constant.
        /// </summary>
        ///
        public const int FULL = 0;

        /// <summary>
        /// Format style constant.
        /// </summary>
        ///
        public const int LONG = 1;

        /// <summary>
        /// Format style constant.
        /// </summary>
        ///
        public const int MEDIUM = 2;

        /// <summary>
        /// Format style constant.
        /// </summary>
        ///
        public const int SHORT = 3;

        /// <summary>
        /// Field constant.
        /// </summary>
        ///
        public const int ERA_FIELD = 0;

        /// <summary>
        /// Field constant.
        /// </summary>
        ///
        public const int YEAR_FIELD = 1;

        /// <summary>
        /// Field constant.
        /// </summary>
        ///
        public const int MONTH_FIELD = 2;

        /// <summary>
        /// Field constant.
        /// </summary>
        ///
        public const int DATE_FIELD = 3;

        /// <summary>
        /// Field constant.
        /// </summary>
        ///
        public const int HOUR_OF_DAY1_FIELD = 4;

        /// <summary>
        /// Field constant.
        /// </summary>
        ///
        public const int HOUR_OF_DAY0_FIELD = 5;

        /// <summary>
        /// Field constant.
        /// </summary>
        ///
        public const int MINUTE_FIELD = 6;

        /// <summary>
        /// Field constant.
        /// </summary>
        ///
        public const int SECOND_FIELD = 7;

        /// <summary>
        /// Field constant.
        /// </summary>
        ///
        public const int MILLISECOND_FIELD = 8;

        /// <summary>
        /// Field constant.
        /// </summary>
        ///
        public const int DAY_OF_WEEK_FIELD = 9;

        /// <summary>
        /// Field constant.
        /// </summary>
        ///
        public const int DAY_OF_YEAR_FIELD = 10;

        /// <summary>
        /// Field constant.
        /// </summary>
        ///
        public const int DAY_OF_WEEK_IN_MONTH_FIELD = 11;

        /// <summary>
        /// Field constant.
        /// </summary>
        ///
        public const int WEEK_OF_YEAR_FIELD = 12;

        /// <summary>
        /// Field constant.
        /// </summary>
        ///
        public const int WEEK_OF_MONTH_FIELD = 13;

        /// <summary>
        /// Field constant.
        /// </summary>
        ///
        public const int AM_PM_FIELD = 14;

        /// <summary>
        /// Field constant.
        /// </summary>
        ///
        public const int HOUR1_FIELD = 15;

        /// <summary>
        /// Field constant.
        /// </summary>
        ///
        public const int HOUR0_FIELD = 16;

        /// <summary>
        /// Field constant.
        /// </summary>
        ///
        public const int TIMEZONE_FIELD = 17;

        /// <summary>
        /// Constructs a new instance of DateFormat.
        /// </summary>
        ///
        protected internal DateFormat()
        {
        }

        /// <summary>
        /// Answers a new instance of DateFormat with the same properties.
        /// </summary>
        ///
        /// <returns>a shallow copy of this DateFormat</returns>
        /// <seealso cref="T:System.ICloneable"/>
        public override Object Clone()
        {
            DateFormat clone = (DateFormat)base.Clone();
            clone.calendar = (IBM.ICU.Util.Calendar)calendar.Clone();
            clone.numberFormat = (NumberFormat)numberFormat.Clone();
            return clone;
            //throw new NotImplementedException();
        }

        /// <summary>
        /// Compares the specified object to this DateFormat and answer if they are
        /// equal. The object must be an instance of DateFormat with the same
        /// properties.
        /// </summary>
        ///
        /// <param name="object">the object to compare with this object</param>
        /// <returns>true if the specified object is equal to this DateFormat, false
        /// otherwise</returns>
        /// <seealso cref="M:ILOG.J2CsMapping.Text.DateFormat.HashCode"/>
        public override bool Equals(Object obj0)
        {
            if ((Object)this == obj0)
            {
                return true;
            }
            if (!(obj0 is DateFormat))
            {
                return false;
            }
            DateFormat dateFormat = (DateFormat)obj0;
            return numberFormat.Equals(dateFormat.numberFormat)
                    && calendar.GetTimeZone().Equals(
                            dateFormat.calendar.GetTimeZone())
                    && calendar.GetFirstDayOfWeek() == dateFormat.calendar
                            .GetFirstDayOfWeek()
                    && calendar.GetMinimalDaysInFirstWeek() == dateFormat.calendar
                            .GetMinimalDaysInFirstWeek()
                    && calendar.IsLenient() == dateFormat.calendar.IsLenient();
        }

        /// <summary>
        /// Formats the specified object into the specified StringBuffer using the
        /// rules of this DateFormat. If the field specified by the FieldPosition is
        /// formatted, set the begin and end index of the formatted field in the
        /// FieldPosition.
        /// </summary>
        ///
        /// <param name="object">the object to format, must be a Date or a Number. If theobject is a Number, a Date is constructed using the<c>longValue()</c> of the Number.</param>
        /// <param name="buffer">the StringBuffer</param>
        /// <param name="field">the FieldPosition</param>
        /// <returns>the StringBuffer parameter <c>buffer</c></returns>
        /// <exception cref="IllegalArgumentException">when the object is not a Date or a Number</exception>
        public sealed override StringBuilder FormatObject(Object obj0, StringBuilder buffer,
                FieldPosition field)
        {
            if (obj0 is DateTime)
            {
                return Format((DateTime)obj0, buffer, field);
            }
            if (obj0 is object)
            {
                return Format(new DateTime((Convert.ToInt64(((object)obj0))) * 10000), buffer,
                        field);
            }
            throw new ArgumentException();
        }

        /// <summary>
        /// Formats the specified Date using the rules of this DateFormat.
        /// </summary>
        ///
        /// <param name="date">the Date to format</param>
        /// <returns>the formatted String</returns>
        public String Format(DateTime date)
        {
            return Format(date, new StringBuilder(), new FieldPosition(0))
                    .ToString();
        }

        /// <summary>
        /// Formats the specified Date into the specified StringBuffer using the
        /// rules of this DateFormat. If the field specified by the FieldPosition is
        /// formatted, set the begin and end index of the formatted field in the
        /// FieldPosition.
        /// </summary>
        ///
        /// <param name="date">the Date to format</param>
        /// <param name="buffer">the StringBuffer</param>
        /// <param name="field">the FieldPosition</param>
        /// <returns>the StringBuffer parameter <c>buffer</c></returns>
        public abstract StringBuilder Format(DateTime date, StringBuilder buffer,
                FieldPosition field);

        /// <summary>
        /// Gets the list of installed Locales which support DateFormat.
        /// </summary>
        ///
        /// <returns>an array of Locale</returns>
        public static Locale[] GetAvailableLocales()
        {
            return Locale.GetAvailableLocales();
        }

        /// <summary>
        /// Answers the Calendar used by this DateFormat.
        /// </summary>
        ///
        /// <returns>a Calendar</returns>
        public IBM.ICU.Util.Calendar GetCalendar()
        {
            return calendar;
        }

        /// <summary>
        /// Answers a DateFormat instance for formatting and parsing dates in the
        /// DEFAULT style for the default Locale.
        /// </summary>
        ///
        /// <returns>a DateFormat</returns>
        public static DateFormat GetDateInstance()
        {
            return GetDateInstance(DEFAULT);
        }

        /// <summary>
        /// Answers a DateFormat instance for formatting and parsing dates in the
        /// specified style for the default Locale.
        /// </summary>
        ///
        /// <param name="style">one of SHORT, MEDIUM, LONG, FULL, or DEFAULT</param>
        /// <returns>a DateFormat</returns>
        public static DateFormat GetDateInstance(int style)
        {
            CheckDateStyle(style);
            return GetDateInstance(style, Locale.GetDefault());
        }

        /// <summary>
        /// Answers a DateFormat instance for formatting and parsing dates in the
        /// specified style for the specified Locale.
        /// </summary>
        ///
        /// <param name="style">one of SHORT, MEDIUM, LONG, FULL, or DEFAULT</param>
        /// <param name="locale">the Locale</param>
        /// <returns>a DateFormat</returns>
        public static DateFormat GetDateInstance(int style, Locale locale)
        {
            CheckDateStyle(style);
            IBM.ICU.Text.DateFormat icuFormat = IBM.ICU.Text.DateFormat
                    .GetDateInstance(style, locale);
            return new SimpleDateFormat(locale,
                    (IBM.ICU.Text.SimpleDateFormat)icuFormat);
        }

        /// <summary>
        /// Answers a DateFormat instance for formatting and parsing dates and times
        /// in the DEFAULT style for the default Locale.
        /// </summary>
        ///
        /// <returns>a DateFormat</returns>
        public static DateFormat GetDateTimeInstance()
        {
            return GetDateTimeInstance(DEFAULT, DEFAULT);
        }

        /// <summary>
        /// Answers a <c>DateFormat</c> instance for the formatting and
        /// parsing of both dates and times in the manner appropriate to the default
        /// Locale.
        /// </summary>
        ///
        /// <param name="dateStyle">one of SHORT, MEDIUM, LONG, FULL, or DEFAULT</param>
        /// <param name="timeStyle">one of SHORT, MEDIUM, LONG, FULL, or DEFAULT</param>
        /// <returns>a DateFormat</returns>
        public static DateFormat GetDateTimeInstance(int dateStyle,
                int timeStyle)
        {
            CheckTimeStyle(timeStyle);
            CheckDateStyle(dateStyle);
            return GetDateTimeInstance(dateStyle, timeStyle, Locale.GetDefault());
        }

        /// <summary>
        /// Answers a DateFormat instance for formatting and parsing dates and times
        /// in the specified styles for the specified Locale.
        /// </summary>
        ///
        /// <param name="dateStyle">one of SHORT, MEDIUM, LONG, FULL, or DEFAULT</param>
        /// <param name="timeStyle">one of SHORT, MEDIUM, LONG, FULL, or DEFAULT</param>
        /// <param name="locale">the Locale</param>
        /// <returns>a DateFormat</returns>
        public static DateFormat GetDateTimeInstance(int dateStyle,
                int timeStyle, Locale locale)
        {
            CheckTimeStyle(timeStyle);
            CheckDateStyle(dateStyle);
            IBM.ICU.Text.DateFormat icuFormat = IBM.ICU.Text.DateFormat
                    .GetDateTimeInstance(dateStyle, timeStyle, locale);
            return new SimpleDateFormat(locale,
                    (IBM.ICU.Text.SimpleDateFormat)icuFormat);
        }

        /// <summary>
        /// Answers a DateFormat instance for formatting and parsing dates and times
        /// in the SHORT style for the default Locale.
        /// </summary>
        ///
        /// <returns>a DateFormat</returns>
        public static DateFormat GetInstance()
        {
            return GetDateTimeInstance(SHORT, SHORT);
        }

        /// <summary>
        /// Answers the NumberFormat used by this DateFormat.
        /// </summary>
        ///
        /// <returns>a NumberFormat</returns>
        public NumberFormat GetNumberFormat()
        {
            return numberFormat;
        }

        static internal String GetStyleName(int style)
        {
            String styleName;
            switch (style)
            {
                case SHORT:
                    styleName = "SHORT"; //$NON-NLS-1$
                    break;
                case MEDIUM:
                    styleName = "MEDIUM"; //$NON-NLS-1$
                    break;
                case LONG:
                    styleName = "LONG"; //$NON-NLS-1$
                    break;
                case FULL:
                    styleName = "FULL"; //$NON-NLS-1$
                    break;
                default:
                    styleName = ""; //$NON-NLS-1$
                    break;
            }
            return styleName;
        }

        /// <summary>
        /// Answers a DateFormat instance for formatting and parsing times in the
        /// DEFAULT style for the default Locale.
        /// </summary>
        ///
        /// <returns>a DateFormat</returns>
        public static DateFormat GetTimeInstance()
        {
            return GetTimeInstance(DEFAULT);
        }

        /// <summary>
        /// Answers a DateFormat instance for formatting and parsing times in the
        /// specified style for the default Locale.
        /// </summary>
        ///
        /// <param name="style">one of SHORT, MEDIUM, LONG, FULL, or DEFAULT</param>
        /// <returns>a DateFormat</returns>
        public static DateFormat GetTimeInstance(int style)
        {
            CheckTimeStyle(style);
            return GetTimeInstance(style, Locale.GetDefault());
        }

        /// <summary>
        /// Answers a DateFormat instance for formatting and parsing times in the
        /// specified style for the specified Locale.
        /// </summary>
        ///
        /// <param name="style">one of SHORT, MEDIUM, LONG, FULL, or DEFAULT</param>
        /// <param name="locale">the Locale</param>
        /// <returns>a DateFormat</returns>
        public static DateFormat GetTimeInstance(int style, Locale locale)
        {
            CheckTimeStyle(style);
            IBM.ICU.Text.DateFormat icuFormat = IBM.ICU.Text.DateFormat
                    .GetTimeInstance(style, locale);
            return new SimpleDateFormat(locale,
                    (IBM.ICU.Text.SimpleDateFormat)icuFormat);
        }

        /// <summary>
        /// Answers the TimeZone of the Calendar used by this DateFormat.
        /// </summary>
        ///
        /// <returns>a TimeZone</returns>
        public IBM.ICU.Util.TimeZone GetTimeZone()
        {
            return calendar.GetTimeZone();
        }

        /// <summary>
        /// Answers an integer hash code for the receiver. Objects which are equal
        /// answer the same value for this method.
        /// </summary>
        ///
        /// <returns>the receiver's hash</returns>
        /// <seealso cref="M:ILOG.J2CsMapping.Text.DateFormat.Equals(System.Object)"/>
        public override int GetHashCode()
        {
            return calendar.GetFirstDayOfWeek()
                    + calendar.GetMinimalDaysInFirstWeek()
                    + calendar.GetTimeZone().GetHashCode()
                    + ((calendar.IsLenient()) ? 1231 : 1237)
                    + numberFormat.GetHashCode();
        }

        /// <summary>
        /// Answers if the Calendar used by this DateFormat is lenient.
        /// </summary>
        ///
        /// <returns>true when the Calendar is lenient, false otherwise</returns>
        public bool IsLenient()
        {
            return calendar.IsLenient();
        }

        /// <summary>
        /// Parse a Date from the specified String using the rules of this
        /// DateFormat.
        /// </summary>
        ///
        /// <param name="string">the String to parse</param>
        /// <returns>the Date resulting from the parse</returns>
        /// <exception cref="ParseException">when an error occurs during parsing</exception>
        public DateTime Parse(String str0)
        {
            ParsePosition position = new ParsePosition(0);
            DateTime date = Parse(str0, position);
            if (position.GetErrorIndex() != -1 || position.GetIndex() == 0)
            {
                // text.19=Unparseable date: {0}
                throw new ILOG.J2CsMapping.Util.ParseException("text.19" + str0 + position.GetErrorIndex()); //$NON-NLS-1$
            }
            return date;
        }

        /// <summary>
        /// Parse a Date from the specified String starting at the index specified by
        /// the ParsePosition. If the string is successfully parsed, the index of the
        /// ParsePosition is updated to the index following the parsed text.
        /// </summary>
        ///
        /// <param name="string">the String to parse</param>
        /// <param name="position">the ParsePosition, updated on return with the index followingthe parsed text, or on error the index is unchanged and theerror index is set to the index where the error occurred</param>
        /// <returns>the Date resulting from the parse, or null if there is an error</returns>
        public abstract DateTime Parse(String str0, ParsePosition position);

        /// <summary>
        /// Parse a Date from the specified String starting at the index specified by
        /// the ParsePosition. If the string is successfully parsed, the index of the
        /// ParsePosition is updated to the index following the parsed text.
        /// </summary>
        ///
        /// <param name="string">the String to parse</param>
        /// <param name="position">the ParsePosition, updated on return with the index followingthe parsed text, or on error the index is unchanged and theerror index is set to the index where the error occurred</param>
        /// <returns>the Date resulting from the parse, or null if there is an error</returns>
        public override Object ParseObject(String str0, ParsePosition position)
        {
            return Parse(str0, position);
        }

        /// <summary>
        /// Sets the Calendar used by this DateFormat.
        /// </summary>
        ///
        /// <param name="cal">the Calendar</param>
        public void SetCalendar(IBM.ICU.Util.Calendar cal)
        {
            calendar = cal;
        }

        /// <summary>
        /// Sets if the Calendar used by this DateFormat is lenient.
        /// </summary>
        ///
        /// <param name="value">true to set the Calendar to be lenient, false otherwise</param>
        public void SetLenient(bool value_ren)
        {
            calendar.SetLenient(value_ren);
        }

        /// <summary>
        /// Sets the NumberFormat used by this DateFormat.
        /// </summary>
        ///
        /// <param name="format">the NumberFormat</param>
        public void SetNumberFormat(NumberFormat format)
        {
            numberFormat = format;
        }

        /// <summary>
        /// Sets the TimeZone of the Calendar used by this DateFormat.
        /// </summary>
        ///
        /// <param name="timezone">the TimeZone</param>
        public void SetTimeZone(IBM.ICU.Util.TimeZone timezone)
        {
            calendar.SetTimeZone(timezone);
        }

        /// <summary>
        /// The instances of this inner class are used as attribute keys and values
        /// in AttributedCharacterIterator that
        /// SimpleDateFormat.formatToCharacterIterator() method returns.
        /// <p>
        /// There is no public constructor to this class, the only instances are the
        /// constants defined here.
        /// <p>
        /// </summary>
        ///
        public class Field : Format.Field
        {

            private const long serialVersionUID = 7441350119349544720L;

            private static Dictionary<Int32, Field> table = new Dictionary<Int32, Field>();

            public static readonly DateFormat.Field ERA = new DateFormat.Field("era", ILOG.J2CsMapping.Util.Calendar.ERA); //$NON-NLS-1$

            public static readonly DateFormat.Field YEAR = new DateFormat.Field("year", ILOG.J2CsMapping.Util.Calendar.YEAR); //$NON-NLS-1$

            public static readonly DateFormat.Field MONTH = new DateFormat.Field("month", ILOG.J2CsMapping.Util.Calendar.MONTH); //$NON-NLS-1$

            public static readonly DateFormat.Field HOUR_OF_DAY0 = new DateFormat.Field("hour of day", //$NON-NLS-1$
                    ILOG.J2CsMapping.Util.Calendar.HOUR_OF_DAY);

            public static readonly DateFormat.Field HOUR_OF_DAY1 = new DateFormat.Field("hour of day 1", -1); //$NON-NLS-1$

            public static readonly DateFormat.Field MINUTE = new DateFormat.Field("minute", ILOG.J2CsMapping.Util.Calendar.MINUTE); //$NON-NLS-1$

            public static readonly DateFormat.Field SECOND = new DateFormat.Field("second", ILOG.J2CsMapping.Util.Calendar.SECOND); //$NON-NLS-1$

            public static readonly DateFormat.Field MILLISECOND = new DateFormat.Field("millisecond", //$NON-NLS-1$
                    ILOG.J2CsMapping.Util.Calendar.MILLISECOND);

            public static readonly DateFormat.Field DAY_OF_WEEK = new DateFormat.Field("day of week", //$NON-NLS-1$
                    ILOG.J2CsMapping.Util.Calendar.DAY_OF_WEEK);

            public static readonly DateFormat.Field DAY_OF_MONTH = new DateFormat.Field("day of month", //$NON-NLS-1$
                    ILOG.J2CsMapping.Util.Calendar.DAY_OF_MONTH);

            public static readonly DateFormat.Field DAY_OF_YEAR = new DateFormat.Field("day of year", //$NON-NLS-1$
                    ILOG.J2CsMapping.Util.Calendar.DAY_OF_YEAR);

            public static readonly DateFormat.Field DAY_OF_WEEK_IN_MONTH = new DateFormat.Field(
                    "day of week in month", ILOG.J2CsMapping.Util.Calendar.DAY_OF_WEEK_IN_MONTH); //$NON-NLS-1$

            public static readonly DateFormat.Field WEEK_OF_YEAR = new DateFormat.Field("week of year", //$NON-NLS-1$
                    ILOG.J2CsMapping.Util.Calendar.WEEK_OF_YEAR);

            public static readonly DateFormat.Field WEEK_OF_MONTH = new DateFormat.Field("week of month", //$NON-NLS-1$
                    ILOG.J2CsMapping.Util.Calendar.WEEK_OF_MONTH);

            public static readonly DateFormat.Field AM_PM = new DateFormat.Field("am pm", ILOG.J2CsMapping.Util.Calendar.AM_PM); //$NON-NLS-1$

            public static readonly DateFormat.Field HOUR0 = new DateFormat.Field("hour", ILOG.J2CsMapping.Util.Calendar.HOUR); //$NON-NLS-1$

            public static readonly DateFormat.Field HOUR1 = new DateFormat.Field("hour 1", -1); //$NON-NLS-1$

            public static readonly DateFormat.Field TIME_ZONE = new DateFormat.Field("time zone", -1); //$NON-NLS-1$

            /// <summary>
            /// The Calendar field that this Field represents.
            /// </summary>
            ///
            private int calendarField;

            /// <summary>
            /// Constructs a new instance of DateFormat.Field with the given
            /// fieldName and calendar field.
            /// </summary>
            ///
            protected internal Field(String fieldName, int calendarField_0)
                : base(fieldName)
            {
                this.calendarField = -1;
                this.calendarField = calendarField_0;
                if (calendarField_0 != -1
                        && (DateFormat.Field)ILOG.J2CsMapping.Collections.Generics.Collections.Get(table, ((int)(calendarField_0))) == null)
                {
                    ILOG.J2CsMapping.Collections.Generics.Collections.Put(table, (System.Int32)(((int)(calendarField_0))), (DateFormat.Field)(this));
                }
            }

            /// <summary>
            /// Answers the Calendar field this Field represents
            /// </summary>
            ///
            /// <returns>int calendar field</returns>
            public int GetCalendarField()
            {
                return calendarField;
            }

            /// <summary>
            /// Answers the DateFormat.Field instance for the given calendar field
            /// </summary>
            ///
            /// <param name="calendarField_0">a calendar field constant</param>
            /// <returns>null if there is no Field for this calendar field</returns>
            public static DateFormat.Field OfCalendarField(int calendarField_0)
            {
                if (calendarField_0 < 0 || calendarField_0 >= ILOG.J2CsMapping.Util.Calendar.FIELD_COUNT)
                {
                    throw new ArgumentException();
                }

                return (DateFormat.Field)ILOG.J2CsMapping.Collections.Generics.Collections.Get(table, ((int)(calendarField_0)));
            }

            /// <summary>
            /// Serialization method resolve instances to the constant
            /// DateFormat.Field values
            /// </summary>
            ///
            protected internal override Object ReadResolve()
            {
                if (calendarField != -1)
                {
                    try
                    {
                        DateFormat.Field result = OfCalendarField(calendarField);
                        if (result != null && this.Equals(result))
                        {
                            return result;
                        }
                    }
                    catch (ArgumentException e)
                    {
                        // text.02=Unknown attribute
                        throw new IOException("text.02"); //$NON-NLS-1$
                    }
                }
                else
                {
                    if (this.Equals(TIME_ZONE))
                    {
                        return TIME_ZONE;
                    }
                    if (this.Equals(HOUR1))
                    {
                        return HOUR1;
                    }
                    if (this.Equals(HOUR_OF_DAY1))
                    {
                        return HOUR_OF_DAY1;
                    }
                }
                // text.02=Unknown attribute
                throw new IOException("text.02"); //$NON-NLS-1$
            }
        }

        private static void CheckDateStyle(int style)
        {
            if (!(style == SHORT || style == MEDIUM || style == LONG
                    || style == FULL || style == DEFAULT))
            {
                // text.0E=Illegal date style: {0}
                throw new ArgumentException("text.0E" + style); //$NON-NLS-1$
            }
        }

        private static void CheckTimeStyle(int style)
        {
            if (!(style == SHORT || style == MEDIUM || style == LONG
                    || style == FULL || style == DEFAULT))
            {
                // text.0F=Illegal time style: {0}
                throw new ArgumentException("text.0F" + style); //$NON-NLS-1$
            }
        }
    }
}
