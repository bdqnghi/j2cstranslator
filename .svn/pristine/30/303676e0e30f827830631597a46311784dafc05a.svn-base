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
    /// 
    /// </summary>
    public class TimeZone
    {
        private String tz_name;
        private String tz_full_name;
        private int gmt_shift;

        /// <summary>
        /// 
        /// </summary>
        public TimeZone()
        {
            tz_name = "";
            tz_full_name = "";
            gmt_shift = 0;
        }

        /// <summary>
        /// 
        /// </summary>
        public String Abbrev
        {
            get
            {
                return tz_name;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public String FullName
        {
            get
            {
                return tz_full_name;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int ISO
        {
            get
            {
                return gmt_shift;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "" + ISO + " " + Abbrev + " (" + FullName + ")";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="timezone"></param>
        /// <returns></returns>
        static public TimeZone GetTimeZone(String timezone)
        {
            TimeZone tz = new TimeZone();
            for (int i = 0; i < IDs.Length; i++)
            {
                if (IDs[i].CompareTo(timezone) == 0 || FullNames[i].CompareTo(timezone) == 0)
                {
                    tz.tz_name = IDs[i];
                    tz.tz_full_name = FullNames[i];
                    tz.gmt_shift = GMTShift[i];
                }
            }

            return tz;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="timezone"></param>
        /// <returns></returns>
        static public TimeZone GetTimeZoneInDate(String date)
        {
            TimeZone tz = new TimeZone();
            for (int i = 0; i < IDs.Length; i++)
            {
                if (date.IndexOf(IDs[i]) != -1)
                {
                    tz.tz_name = IDs[i];
                    tz.tz_full_name = FullNames[i];
                    tz.gmt_shift = GMTShift[i];
                }
            }

            return tz;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        static public String FormatDOTNET(String dt)
        {
            for (int i = 0; i < IDs.Length; i++)
            {
                if (GMTShift[i] >= 0)
                    dt = dt.Replace(IDs[i], "+" + GMTShift[i]);
                else
                    dt = dt.Replace(IDs[i], "" + GMTShift[i]);
            }
            return dt;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        static public String FormatJava(String dt)
        {
            for (int i = 0; i < GMTShift.Length; i++)
            {
                if (GMTShift[i] >= 0)
                    dt = dt.Replace("+" + GMTShift[i], IDs[i]);
                else
                    dt = dt.Replace("" + GMTShift[i], IDs[i]);
            }
            return dt;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        static public String GetTZShift(String ID)
        {
            TimeZone tz = GetTimeZone(ID);

            if (tz != null)
            {
                if (tz.gmt_shift > 0)
                    return "+" + tz.gmt_shift;
                else
                    return "" + tz.gmt_shift;
            }

            return "";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Shift"></param>
        /// <returns></returns>
        static public String GetTZID(String Shift)
        {
            int shift = int.Parse(Shift);

            for (int i = 0; i < GMTShift.Length; i++)
                if (GMTShift[i] == shift)
                    return IDs[i];

            return "";
        }

        /// <summary>
        /// Currently only supports GMT/UTC time-zone...
        /// </summary>
        static private String[] IDs =
        {
            "GMT",
            "UTC",
            "EST",
            "CST",
            "MST",
            "PST",
            "AST",
            "HST",
            "CET",
            "EET",
        };

        /// <summary>
        /// Currently only supports GMT/UTC time-zone...
        /// </summary>
        static private String[] FullNames =
        {
            "Greenwitch Mean Time",
            "Coordinated Universal Time",
            "Eastern Standard Time",
            "Central Standard Time",
            "Mountain Standard Time",
            "Pacific Standard Time",
            "Alaska Standard Time",
            "Hawaii Standard Time",
            "Central European Time",
            "Eastern European Time",
        };

        static private int[] GMTShift = 
        {
            0,
            0,
            -5,
            -6,
            -7,
            -8,
            -9,
            -10,
            1,
            2,
        };

    }
}
