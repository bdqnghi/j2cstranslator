/*
 **********************************************************************
 * Copyright (c) 2004-2005, International Business Machines
 * Corporation and others.  All Rights Reserved.
 **********************************************************************
 * Author: Alan Liu
 * Created: April 20, 2004
 * Since: ICU 3.0
 **********************************************************************
 */
// --------------------------------------------------------------------------------------------------
// This file was automatically generated by J2CS Translator (http://j2cstranslator.sourceforge.net/). 
// Version 1.3.6.20101125_01     
// 12/2/10 11:30 AM    
// ${CustomMessageForDisclaimer}                                                                             
// --------------------------------------------------------------------------------------------------
 namespace IBM.ICU.Text {
	
	using IBM.ICU.Util;
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.IO;
	using System.Runtime.CompilerServices;
	using System.Text;
     using ILOG.J2CsMapping.Util;
     using ILOG.J2CsMapping.Text;
	
	/// <exclude/>
	/// <summary>
	/// Temporary internal concrete subclass of MeasureFormat implementing parsing
	/// and formatting of CurrencyAmount objects. This class is likely to be
	/// redesigned and rewritten in the near future.
	/// <p>
	/// This class currently delegates to DecimalFormat for parsing and formatting.
	/// </summary>
	///
	/// <seealso cref="T:IBM.ICU.Text.UFormat"/>
	/// <seealso cref="T:IBM.ICU.Text.DecimalFormat"/>
	internal class CurrencyFormat : MeasureFormat {
	    // Generated by serialver from JDK 1.4.1_01
	    internal const long serialVersionUID = -931679363692504634L;
	
	    private NumberFormat fmt;
	
	    public CurrencyFormat(ULocale locale) {
	        fmt = IBM.ICU.Text.NumberFormat.GetCurrencyInstance(locale.ToLocale());
	    }
	
	    /// <summary>
	    /// Override Format.format().
	    /// </summary>
	    ///
	    /// <seealso cref="M:ILOG.J2CsMapping.Text.IlFormat.Format(System.Object, System.Text.StringBuilder, IBM.ICU.Text.FieldPosition)"/>
	    public override StringBuilder FormatObject(Object obj, StringBuilder toAppendTo,
	            FieldPosition pos) {
	        try {
	            CurrencyAmount currency = (CurrencyAmount) obj;
	            fmt.SetCurrency(currency.GetCurrency());
	            return fmt.FormatObject(currency.GetNumber(), toAppendTo, pos);
	        } catch (InvalidCastException e) {
	            throw new ArgumentException("Invalid type: "
	                    + obj.GetType().FullName);
	        }
	    }
	
	    /// <summary>
	    /// Override Format.parseObject().
	    /// </summary>
	    ///
	    /// <seealso cref="M:ILOG.J2CsMapping.Text.IlFormat.ParseObject(System.String, IBM.ICU.Text.ParsePosition)"/>
	    public override Object ParseObject(String source, ParsePosition pos) {
	        return fmt.ParseCurrency(source, pos);
	    }
	}
}