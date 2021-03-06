/*
 *******************************************************************************
 * Copyright (C) 2001-2005, International Business Machines Corporation and    *
 * others. All Rights Reserved.                                                *
 *******************************************************************************
 */
// --------------------------------------------------------------------------------------------------
// This file was automatically generated by J2CS Translator (http://j2cstranslator.sourceforge.net/). 
// Version 1.3.6.20101125_01     
// 12/2/10 11:30 AM    
// ${CustomMessageForDisclaimer}                                                                             
// --------------------------------------------------------------------------------------------------
 namespace IBM.ICU.Text {
	
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.IO;
	using System.Runtime.CompilerServices;
	using System.Text;
	
	
	internal class Quantifier : UnicodeMatcher {
	
	    private UnicodeMatcher matcher;
	
	    private int minCount;
	
	    private int maxCount;
	
	    /// <summary>
	    /// Maximum count a quantifier can have.
	    /// </summary>
	    ///
	    public const int MAX = Int32.MaxValue;
	
	    public Quantifier(UnicodeMatcher theMatcher, int theMinCount,
	            int theMaxCount) {
	        if (theMatcher == null || minCount < 0 || maxCount < 0
	                || minCount > maxCount) {
	            throw new ArgumentException();
	        }
	        matcher = theMatcher;
	        minCount = theMinCount;
	        maxCount = theMaxCount;
	    }
	
	    /// <summary>
	    /// Implement UnicodeMatcher API.
	    /// </summary>
	    ///
	    public virtual int Matches(Replaceable text, int[] offset, int limit,
	            bool incremental) {
	        int start = offset[0];
	        int count = 0;
	        while (count < maxCount) {
	            int pos = offset[0];
	            int m = matcher.Matches(text, offset, limit, incremental);
	            if (m == IBM.ICU.Text.UnicodeMatcher_Constants.U_MATCH) {
	                ++count;
	                if (pos == offset[0]) {
	                    // If offset has not moved we have a zero-width match.
	                    // Don't keep matching it infinitely.
	                    break;
	                }
	            } else if (incremental && m == IBM.ICU.Text.UnicodeMatcher_Constants.U_PARTIAL_MATCH) {
	                return IBM.ICU.Text.UnicodeMatcher_Constants.U_PARTIAL_MATCH;
	            } else {
	                break;
	            }
	        }
	        if (incremental && offset[0] == limit) {
	            return IBM.ICU.Text.UnicodeMatcher_Constants.U_PARTIAL_MATCH;
	        }
	        if (count >= minCount) {
	            return IBM.ICU.Text.UnicodeMatcher_Constants.U_MATCH;
	        }
	        offset[0] = start;
	        return IBM.ICU.Text.UnicodeMatcher_Constants.U_MISMATCH;
	    }
	
	    /// <summary>
	    /// Implement UnicodeMatcher API
	    /// </summary>
	    ///
	    public virtual String ToPattern(bool escapeUnprintable) {
	        StringBuilder result = new StringBuilder();
	        result.Append(matcher.ToPattern(escapeUnprintable));
	        if (minCount == 0) {
	            if (maxCount == 1) {
	                return result.Append('?').ToString();
	            } else if (maxCount == MAX) {
	                return result.Append('*').ToString();
	            }
	            // else fall through
	        } else if (minCount == 1 && maxCount == MAX) {
	            return result.Append('+').ToString();
	        }
	        result.Append('{');
	        IBM.ICU.Impl.Utility.AppendNumber(result, minCount);
	        result.Append(',');
	        if (maxCount != MAX) {
	            IBM.ICU.Impl.Utility.AppendNumber(result, maxCount);
	        }
	        result.Append('}');
	        return result.ToString();
	    }
	
	    /// <summary>
	    /// Implement UnicodeMatcher API
	    /// </summary>
	    ///
	    public virtual bool MatchesIndexValue(int v) {
	        return (minCount == 0) || matcher.MatchesIndexValue(v);
	    }
	
	    /// <summary>
	    /// Implementation of UnicodeMatcher API. Union the set of all characters
	    /// that may be matched by this object into the given set.
	    /// </summary>
	    ///
	    /// <param name="toUnionTo">the set into which to union the source characters</param>
	    /// @returns a reference to toUnionTo
	    public virtual void AddMatchSetTo(UnicodeSet toUnionTo) {
	        if (maxCount > 0) {
	            matcher.AddMatchSetTo(toUnionTo);
	        }
	    }
	}
	
	// eof
}
