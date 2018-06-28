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
using System.Globalization;

namespace ILOG.J2CsMapping.Util
{
	/// <summary>
	/// Utility class for the mapping of System.DateTime with java.lang.Date.
	/// </summary>
    public class DateUtil {

        
        public static long millisICU = new IBM.ICU.Util.GregorianCalendar(1970, IBM.ICU.Util.Calendar.JANUARY, 1).GetTimeInMillis();
        public static long millisNET = new DateTime(1970, 1, 1, new System.Globalization.GregorianCalendar()).Ticks / TimeSpan.TicksPerMillisecond;
        public static long deltaICU_NET = millisICU - millisNET;
        public static long deltaNET_ICU = millisNET - millisICU;

        public static DateTime DateFromJavaMillis(long time)
        {
            return new DateTime((time + DateUtil.deltaNET_ICU) * TimeSpan.TicksPerMillisecond);
        }

        public static long DotNetDateToJavaMillis(DateTime date)
        {
            return (date.Ticks / TimeSpan.TicksPerMillisecond) + deltaICU_NET;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="d1"></param>
        /// <param name="d2"></param>
        /// <returns></returns>
        public static bool After(DateTime d1, DateTime d2) {
            return (d1 > d2);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="d1"></param>
        /// <param name="d2"></param>
        /// <returns></returns>
        public static bool Before(DateTime d1, DateTime d2)
        {
            return (d1 < d2);
        }
       
    }
}
