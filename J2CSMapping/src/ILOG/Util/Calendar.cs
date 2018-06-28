// 
// J2CsMapping : runtime library for J2CsTranslator
// 
// Copyright (c) 2008-2010 Alexandre FAU.
// All rights reserved. This program and the accompanying materials
// are made available under the terms of the Eclipse Public License v1.0
// which accompanies this distribution, and is available at
// http://www.eclipse.org/legal/epl-v10.html
// Contributors:
//   Alexandre FAU (IBM)
//

using System;
using System.Collections.Generic;
using System.Text;

namespace ILOG.J2CsMapping.Util
{
    /// <summary>
    /// Calendar .NET equivalent to java.util.calendar class
    /// </summary>
    public class Calendar
    {
        /// <summary>
        /// 
        /// </summary>
        protected DateTime internal_dt;

        /// <summary>
        /// 
        /// </summary>
        public Calendar()
        {
            internal_dt = DateTime.Today;
        }

        /// <summary>
        /// Set this objects time in ticks...
        /// </summary>
        /// <param name="millis">Ticks in Windows format!</param>
        public void SetTimeInMillis(long millis)
        {
            internal_dt = new DateTime(millis);
        }

        /// <summary>
        /// Retrieves current DateTime Object
        /// </summary>
        /// <returns>DateTime object</returns>
        public DateTime GetTime()
        {
            return internal_dt;
        }

        static public Calendar GetInstance()
        {
            return new Calendar();
        }

        /// <summary>
        /// Field descriptor #101 I
        /// </summary>
        public const int ERA = 0;

        /// <summary>
        /// Field descriptor #101 I
        /// </summary>
        public const int YEAR = 1;

        /// <summary>
        /// Field descriptor #101 I
        /// </summary>
        public const int MONTH = 2;

        /// <summary>
        /// Field descriptor #101 I
        /// </summary>
        public const int WEEK_OF_YEAR = 3;

        /// <summary>
        /// Field descriptor #101 I
        /// </summary>
        public const int WEEK_OF_MONTH = 4;

        /// <summary>
        /// Field descriptor #101 I
        /// </summary>
        public const int DATE = 5;

        /// <summary>
        /// Field descriptor #101 I
        /// </summary>
        public const int DAY_OF_MONTH = 5;

        /// <summary>
        /// Field descriptor #101 I
        /// </summary>
        public const int DAY_OF_YEAR = 6;

        /// <summary>
        /// Field descriptor #101 I
        /// </summary>
        public const int DAY_OF_WEEK = 7;

        /// <summary>
        /// Field descriptor #101 I
        /// </summary>
        public const int DAY_OF_WEEK_IN_MONTH = 8;

        /// <summary>
        /// Field descriptor #101 I
        /// </summary>
        public const int AM_PM = 9;

        /// <summary>
        /// Field descriptor #101 I
        /// </summary>
        public const int HOUR = 10;

        /// <summary>
        /// Field descriptor #101 I
        /// </summary>
        public const int HOUR_OF_DAY = 11;

        /// <summary>
        /// Field descriptor #101 I
        /// </summary>
        public const int MINUTE = 12;

        /// <summary>
        /// Field descriptor #101 I
        /// </summary>
        public const int SECOND = 13;

        /// <summary>
        /// Field descriptor #101 I
        /// </summary>
        public const int MILLISECOND = 14;

        /// <summary>
        /// Field descriptor #101 I
        /// </summary>
        public const int ZONE_OFFSET = 15;

        /// <summary>
        /// Field descriptor #101 I
        /// </summary>
        public const int DST_OFFSET = 16;

        // 
        /// <summary>
        /// Field descriptor #101 I
        /// </summary>
        public const int FIELD_COUNT = 17;


        public TimeZone GetTimeZone()
        {
            throw new NotImplementedException();
        }

        public int GetFirstDayOfWeek()
        {
            throw new NotImplementedException();
        }

        public int GetMinimalDaysInFirstWeek()
        {
            throw new NotImplementedException();
        }

        public bool IsLenient()
        {
            throw new NotImplementedException();
        }

        internal void SetLenient(bool value_ren)
        {
            throw new NotImplementedException();
        }

        internal void SetTimeZone(TimeZone timezone)
        {
            throw new NotImplementedException();
        }
    }
}
