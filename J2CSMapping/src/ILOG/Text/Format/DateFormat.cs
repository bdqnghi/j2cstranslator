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
using ILOG.J2CsMapping.Text;

namespace ILOG.J2CsMapping.Text
{
    /// <summary>
    /// .NET Replacement for java DateFormat
    /// </summary>
    public interface DateFormat : IlFormat
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        DateTime Parse(string text);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        string Format(DateTime date);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lenient"></param>
        void SetLenient(bool lenient);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        void ApplyPattern(string s);
    }

    /// <summary>
    /// .NET Replacement for java SimpleDateFormat
    /// </summary>
    public class SimpleDateFormat : DateFormat
    {
        private string pattern;
        private bool lenient;
        private CultureInfo locale;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pattern"></param>
        /// <param name="locale"></param>
        public SimpleDateFormat(string pattern, CultureInfo locale)
        {
            this.pattern = pattern;
            this.locale = locale;
        }

        //

        /// <summary>
        /// 
        /// </summary>
        public int TwoDigitYearStart
        {
          get { return locale.DateTimeFormat.Calendar.TwoDigitYearMax - 100; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public DateTime Parse(string text)
        {
            return DateTime.ParseExact(text, pattern, locale.DateTimeFormat);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <param name="twoDigitYearStart"></param>
        /// <returns></returns>
        public DateTime Parse(string text, int twoDigitYearStart)
        {
          DateTimeFormatInfo dateTimeFormat = (DateTimeFormatInfo)locale.DateTimeFormat.Clone();
          dateTimeFormat.Calendar.TwoDigitYearMax = twoDigitYearStart + 100;
          return DateTime.ParseExact(text, pattern, dateTimeFormat);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public string Format(DateTime value)
        {
            return value.ToString(pattern, locale.DateTimeFormat);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lenient"></param>
        public void SetLenient(bool lenient)
        {
            this.lenient = lenient;
        }

        #region IlFormat Members

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string Format(object obj)
        {
            return "" + obj;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public object ParseObject(string source)
        {
            return source;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        public void ApplyPattern(string s)
        {
            throw new SystemException("IlFormat.ApplyPattern(string) NOT YET IMPLEMENTED !");
        }

        #endregion
    }
}
