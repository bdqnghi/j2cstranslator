/*
 **********************************************************************
 * Copyright (c) 2003-2007, International Business Machines
 * Corporation and others.  All Rights Reserved.
 **********************************************************************
 * Author: Alan Liu
 * Created: October 2 2003
 * Since: ICU 2.8
 **********************************************************************
 */

// --------------------------------------------------------------------------------------------------
// This file was automatically generated by J2CS Translator (http://j2cstranslator.sourceforge.net/). 
// Version 1.3.6.20101125_01     
// 12/2/10 11:48 AM    
// ${CustomMessageForDisclaimer}                                                                             
// --------------------------------------------------------------------------------------------------
 namespace IBM.ICU.Impl {
	
	using IBM.ICU.Util;
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.IO;
	using System.Runtime.CompilerServices;
	
	/// <summary>
	/// <c>TimeZoneAdapter</c> wraps a com.ibm.icu.util.TimeZone subclass that
	/// is NOT a JDKTimeZone, that is, that does not itself wrap a
	/// java.util.TimeZone. It inherits from java.util.TimeZone. Without this class,
	/// we would need to 'port' java.util.Date to com.ibm.icu.util as well, so that
	/// Date could interoperate properly with the com.ibm.icu.util TimeZone and
	/// Calendar classes. With this class, we can use java.util.Date together with
	/// com.ibm.icu.util classes.
	/// The complement of this is JDKTimeZone, which makes a java.util.TimeZone look
	/// like a com.ibm.icu.util.TimeZone.
	/// </summary>
	///
	/// <seealso cref="null"/>
	/// <seealso cref="M:IBM.ICU.Impl.TimeZone.SetDefault(null)"/>
	[Serializable]
	public class TimeZoneAdapter : IBM.ICU.Util.TimeZone {
	
	    // Generated by serialver from JDK 1.4.1_01
	    internal const long serialVersionUID = -2040072218820018557L;
	
	    /// <summary>
	    /// The contained com.ibm.icu.util.TimeZone object. Must not be null. We
	    /// delegate all methods to this object.
	    /// </summary>
	    ///
	    private IBM.ICU.Util.TimeZone zone;
	
	    /// <summary>
	    /// Given a java.util.TimeZone, wrap it in the appropriate adapter subclass
	    /// of com.ibm.icu.util.TimeZone and return the adapter.
	    /// </summary>
	    ///
	    public static IBM.ICU.Util.TimeZone Wrap(IBM.ICU.Util.TimeZone tz) {
	        return new TimeZoneAdapter(tz);
	    }
	
	    /// <summary>
	    /// Return the java.util.TimeZone wrapped by this object.
	    /// </summary>
	    ///
	    public IBM.ICU.Util.TimeZone Unwrap() {
	        return zone;
	    }
	
	    /// <summary>
	    /// Constructs an adapter for a com.ibm.icu.util.TimeZone object.
	    /// </summary>
	    ///
	    public TimeZoneAdapter(IBM.ICU.Util.TimeZone zone_0) {
	        this.zone = zone_0;
	        base.SetID(zone_0.GetID());
	    }
	
	    /// <summary>
	    /// TimeZone API; calls through to wrapped time zone.
	    /// </summary>
	    ///
	    public override void SetID(String ID) {
	        base.SetID(ID);
	        zone.SetID(ID);
	    }
	
	    /// <summary>
	    /// TimeZone API; calls through to wrapped time zone.
	    /// </summary>
	    ///
	    public override bool HasSameRules(IBM.ICU.Util.TimeZone other) {
	        return other  is  TimeZoneAdapter
	                && zone.HasSameRules(((TimeZoneAdapter) other).zone);
	    }
	
	    /// <summary>
	    /// TimeZone API; calls through to wrapped time zone.
	    /// </summary>
	    ///
	    public override int GetOffset(int era, int year, int month, int day, int dayOfWeek,
	            int millis) {
	        return zone.GetOffset(era, year, month, day, dayOfWeek, millis);
	    }
	
	    /// <summary>
	    /// TimeZone API; calls through to wrapped time zone.
	    /// </summary>
	    ///
	    public override int GetRawOffset() {
	        return zone.GetRawOffset();
	    }
	
	    /// <summary>
	    /// TimeZone API; calls through to wrapped time zone.
	    /// </summary>
	    ///
	    public override void SetRawOffset(int offsetMillis) {
	        zone.SetRawOffset(offsetMillis);
	    }
	
	    /// <summary>
	    /// TimeZone API; calls through to wrapped time zone.
	    /// </summary>
	    ///
	    public override bool UseDaylightTime() {
	        return zone.UseDaylightTime();
	    }
	
	    /// <summary>
	    /// TimeZone API; calls through to wrapped time zone.
	    /// </summary>
	    ///
	    public override bool InDaylightTime(DateTime date) {
	        return zone.InDaylightTime(date);
	    }
	
	    /// <summary>
	    /// Boilerplate API; calls through to wrapped object.
	    /// </summary>
	    ///
	    public override Object Clone() {
	        return new TimeZoneAdapter((IBM.ICU.Util.TimeZone) zone.Clone());
	    }
	
	    /// <summary>
	    /// Boilerplate API; calls through to wrapped object.
	    /// </summary>
	    ///
	    [MethodImpl(MethodImplOptions.Synchronized)]
	    public override int GetHashCode() {
	        return zone.GetHashCode();
	    }
	
	    /// <summary>
	    /// Boilerplate API; calls through to wrapped object.
	    /// </summary>
	    ///
	    public override bool Equals(Object obj) {
	        if (obj  is  TimeZoneAdapter) {
	            obj = ((TimeZoneAdapter) obj).zone;
	        }
	        return zone.Equals(obj);
	    }
	
	    /// <summary>
	    /// Returns a string representation of this object.
	    /// </summary>
	    ///
	    /// <returns>a string representation of this object.</returns>
	    public override String ToString() {
	        return "TimeZoneAdapter: " + zone.ToString();
	    }
	}
}
