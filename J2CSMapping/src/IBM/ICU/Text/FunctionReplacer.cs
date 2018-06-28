/*
 **********************************************************************
 *   Copyright (c) 2002-2003, International Business Machines Corporation
 *   and others.  All Rights Reserved.
 **********************************************************************
 *   Date        Name        Description
 *   01/14/2002  aliu        Creation.
 **********************************************************************
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
	
	/// <summary>
	/// A replacer that calls a transliterator to generate its output text. The input
	/// text to the transliterator is the output of another UnicodeReplacer object.
	/// That is, this replacer wraps another replacer with a transliterator.
	/// </summary>
	///
	internal class FunctionReplacer : UnicodeReplacer {
	
	    /// <summary>
	    /// The transliterator. Must not be null.
	    /// </summary>
	    ///
	    private Transliterator translit;
	
	    /// <summary>
	    /// The replacer object. This generates text that is then processed by
	    /// 'translit'. Must not be null.
	    /// </summary>
	    ///
	    private UnicodeReplacer replacer;
	
	    /// <summary>
	    /// Construct a replacer that takes the output of the given replacer, passes
	    /// it through the given transliterator, and emits the result as output.
	    /// </summary>
	    ///
	    public FunctionReplacer(Transliterator theTranslit,
	            UnicodeReplacer theReplacer) {
	        translit = theTranslit;
	        replacer = theReplacer;
	    }
	
	    /// <summary>
	    /// UnicodeReplacer API
	    /// </summary>
	    ///
	    public virtual int Replace(Replaceable text, int start, int limit, int[] cursor) {
	
	        // First delegate to subordinate replacer
	        int len = replacer.Replace(text, start, limit, cursor);
	        limit = start + len;
	
	        // Now transliterate
	        limit = translit.Transliterate(text, start, limit);
	
	        return limit - start;
	    }
	
	    /// <summary>
	    /// UnicodeReplacer API
	    /// </summary>
	    ///
	    public virtual String ToReplacerPattern(bool escapeUnprintable) {
	        StringBuilder rule = new StringBuilder("&");
	        rule.Append(translit.GetID());
	        rule.Append("( ");
	        rule.Append(replacer.ToReplacerPattern(escapeUnprintable));
	        rule.Append(" )");
	        return rule.ToString();
	    }
	
	    /// <summary>
	    /// Union the set of all characters that may output by this object into the
	    /// given set.
	    /// </summary>
	    ///
	    /// <param name="toUnionTo">the set into which to union the output characters</param>
	    public virtual void AddReplacementSetTo(UnicodeSet toUnionTo) {
	        toUnionTo.AddAll(translit.GetTargetSet());
	    }
	}
	
	// eof
}