// --------------------------------------------------------------------------------------------------
// This file was automatically generated by J2CS Translator (http://j2cstranslator.sourceforge.net/). 
// Version 1.3.6.20101125_01     
// 12/13/10 10:46 AM    
// ${CustomMessageForDisclaimer}                                                                             
// --------------------------------------------------------------------------------------------------
 /// <summary>
/// Copyright (C) 2003-2007, International Business Machines Corporation and         
/// others. All Rights Reserved.                                                
/// </summary>
///
namespace IBM.ICU.Charset {
	
	using IBM.ICU.Util;
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.IO;
	using System.Runtime.CompilerServices;
	
	public abstract class AbstractTestLog : TestLog {
	
	    public static bool dontSkipForVersion = false;
	
	    public bool SkipIfBeforeICU(int major, int minor, int micro) {
	        if (dontSkipForVersion
	                || IBM.ICU.Util.VersionInfo.ICU_VERSION.CompareTo(IBM.ICU.Util.VersionInfo.GetInstance(
	                        major, minor, micro)) > 0) {
	            return false;
	        }
	        Logln("Test skipped before ICU release " + major + "." + minor);
	        return true;
	    }
	
	    /// <summary>
	    /// Add a message.
	    /// </summary>
	    ///
	    public void Log(String message) {
	        Msg(message, IBM.ICU.Charset.TestLog_Constants.LOG, true, false);
	    }
	
	    /// <summary>
	    /// Add a message and newline.
	    /// </summary>
	    ///
	    public void Logln(String message) {
	        Msg(message, IBM.ICU.Charset.TestLog_Constants.LOG, true, true);
	    }
	
	    /// <summary>
	    /// Report an error.
	    /// </summary>
	    ///
	    public void Err(String message) {
	        Msg(message, IBM.ICU.Charset.TestLog_Constants.ERR, true, false);
	    }
	
	    /// <summary>
	    /// Report an error and newline.
	    /// </summary>
	    ///
	    public void Errln(String message) {
	        Msg(message, IBM.ICU.Charset.TestLog_Constants.ERR, true, true);
	    }
	
	    /// <summary>
	    /// Report a warning (generally missing tests or data).
	    /// </summary>
	    ///
	    public void Warn(String message) {
	        Msg(message, IBM.ICU.Charset.TestLog_Constants.WARN, true, false);
	    }
	
	    /// <summary>
	    /// Report a warning (generally missing tests or data) and newline.
	    /// </summary>
	    ///
	    public void Warnln(String message) {
	        Msg(message, IBM.ICU.Charset.TestLog_Constants.WARN, true, true);
	    }
	
	    /// <summary>
	    /// Vector for logging. Callers can force the logging system to not increment
	    /// the error or warning level by passing false for incCount.
	    /// </summary>
	    ///
	    /// <param name="message">the message to output.</param>
	    /// <param name="level">the message level, either LOG, WARN, or ERR.</param>
	    /// <param name="incCount">if true, increments the warning or error count</param>
	    /// <param name="newln">if true, forces a newline after the message</param>
	    public abstract void Msg(String message, int level, bool incCount,
	            bool newln);
	
	    /// <summary>
	    /// Not sure if this class is useful. This lets you log without first testing
	    /// if logging is enabled. The Delegating log will either silently ignore the
	    /// message, if the delegate is null, or forward it to the delegate.
	    /// </summary>
	    ///
	    public sealed class DelegatingLog : AbstractTestLog {
	        private TestLog delegat0;
	
	        public DelegatingLog(TestLog delegat0) {
	            this.delegat0 = delegat0;
	        }
	
	        public override void Msg(String message, int level, bool incCount,
	                bool newln) {
	            if (delegat0 != null) {
	                delegat0.Msg(message, level, incCount, newln);
	            }
	        }
	    }
	
	    public bool IsDateAtLeast(int year, int month, int day) {
	        Calendar c = IBM.ICU.Util.Calendar.GetInstance();
	        DateTime dt = new DateTime(year, month, day);
	        if (c.GetTime().CompareTo(dt) >= 0) {
	            return true;
	        }
	        return false;
	    }
	}
}
