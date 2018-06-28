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
	/// Summary description for NumberFormatProvider.
	/// </summary>
	public class NumberFormatProvider
	{
		protected static NumberFormatInfo formatProvider = null;

        /// <summary>
        /// 
        /// </summary>
		public NumberFormatProvider()
		{
		}

        /// <summary>
        /// 
        /// </summary>
		public static NumberFormatInfo NumberFormat
		{
			get 
			{
				if (formatProvider == null) 
				{
					formatProvider = new NumberFormatInfo();
					formatProvider.NumberDecimalSeparator = ".";
				}
				return formatProvider;
			}
		}
	}
}
