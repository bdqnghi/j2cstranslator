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

namespace ILOG.J2CsMapping.Text
{
    /// <summary>
    /// DatTime Format utility class.
    /// </summary>
    public class DummyFormat : IlFormat
    {
        /// <summary>
        /// Constant for full style pattern.
        /// </summary>
        public static int FULL = 0;
        /// <summary>
        /// Constant for long style pattern.
        /// </summary>
        public static int LONG = 1;
        /// <summary>
        /// Constant for medium style pattern.
        /// </summary>
        public static int MEDIUM = 2;
        /// <summary>
        /// Constant for short style pattern.
        /// </summary>
        public static int SHORT = 3;
        /// <summary>
        /// Constant for default style pattern.  Its value is MEDIUM.
        /// </summary>
        public static int DEFAULT = MEDIUM;

        CultureInfo format =
            new CultureInfo("fr-FR", true);

        /// <summary>
        /// 
        /// </summary>
        public DummyFormat()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        public DummyFormat(String a, CultureInfo b)
        {
            DateTimeFormatInfo nf = new DateTimeFormatInfo();
            nf.FullDateTimePattern = a;
            format.DateTimeFormat = nf;
        }

        /// <summary>
        /// Formats an object to produce a string. 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string Format(object obj)
        {
            if (obj is DateTime)
                return Format((DateTime)obj);
            return String.Format(format, "", obj);
        }

        public string Format(DateTime obj)
        {
            return String.Format(format, "{0:d}", obj);
        }

        /// <summary>
        /// Parses a string to produce an object. 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public object ParseObject(string source)
        {
            object o = DateTime.Parse(source, format);
            return o;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        public void ApplyPattern(string s)
        {
            throw new SystemException("IlFormat.ApplyPattern(string) NOT YET IMPLEMENTED !");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="ci"></param>
        /// <returns></returns>
        public object GetDateTimeInstance(int a, int b, CultureInfo ci)
        {
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public DateTime Parse(string text)
        {
            return DateTime.Parse(text, format);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="b"></param>
        public virtual void SetGroupingused(bool b)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="i"></param>
        public virtual void SetMinimumFractionDigits(int i)
        {
        }

        public virtual void SetMinimumIntegerDigits(int value_ren)
        {
        }

        public virtual int GetMinimumIntegerDigits()
        {
            return 0;
        }

        public virtual int GetMaximumIntegerDigits()
        {
            return 0;
        }

        public virtual void SetMaximumIntegerDigits(int a)
        {           
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual int GetMinimumFractionDigits()
        {
            return 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual int GetMaximumFractionDigits()
        {
            return 0;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int GetNumberInstance()
        {
            return 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual  bool IsGroupingUsed()
        {
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="b"></param>
        public virtual void SetGroupingUsed(bool b)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="style"></param>
        /// <returns></returns>
        public static IlFormat GetDateInstance(int style)
        {
            return new DummyFormat();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lenient"></param>
        public virtual void SetLenient(bool lenient)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        public virtual void SetMaximumFractionDigits(int p)
        {
            throw new NotImplementedException();
        }
    }
}
