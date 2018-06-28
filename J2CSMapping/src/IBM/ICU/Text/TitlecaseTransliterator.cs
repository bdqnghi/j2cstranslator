/*
 * Copyright (C) 1996-2007, International Business Machines Corporation and
 * others. All Rights Reserved.
 *
 */
// --------------------------------------------------------------------------------------------------
// This file was automatically generated by J2CS Translator (http://j2cstranslator.sourceforge.net/). 
// Version 1.3.6.20101125_01     
// 12/2/10 11:30 AM    
// ${CustomMessageForDisclaimer}                                                                             
// --------------------------------------------------------------------------------------------------
 namespace IBM.ICU.Text {
	
	using IBM.ICU.Impl;
	using IBM.ICU.Util;
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.IO;
	using System.Runtime.CompilerServices;
	using System.Text;
	
	/// <summary>
	/// A transliterator that converts all letters (as defined by
	/// <c>UCharacter.isLetter()</c>) to lower case, except for those letters
	/// preceded by non-letters. The latter are converted to title case using
	/// <c>UCharacter.toTitleCase()</c>.
	/// </summary>
	///
	internal class TitlecaseTransliterator : Transliterator {
	
	    public sealed class Anonymous_C0 : Transliterator.Factory {
	        public Transliterator GetInstance(String ID) {
	            return new TitlecaseTransliterator(IBM.ICU.Util.ULocale.US);
	        }
	    }
	
	    internal const String _ID = "Any-Title";
	
	    /// <summary>
	    /// System registration hook.
	    /// </summary>
	    ///
	    static internal void Register() {
	        IBM.ICU.Text.Transliterator.RegisterFactory(_ID, new TitlecaseTransliterator.Anonymous_C0 ());
	
	        IBM.ICU.Text.Transliterator.RegisterSpecialInverse("Title", "Lower", false);
	    }
	
	    private ULocale locale;
	
	    private UCaseProps csp;
	
	    private ReplaceableContextIterator iter;
	
	    private StringBuilder result;
	
	    private int[] locCache;
	
	    /// <summary>
	    /// Constructs a transliterator.
	    /// </summary>
	    ///
	    public TitlecaseTransliterator(ULocale loc) : base(_ID, null) {
	        locale = loc;
	        // Need to look back 2 characters in the case of "can't"
	        SetMaximumContextLength(2);
	        try {
	            csp = IBM.ICU.Impl.UCaseProps.GetSingleton();
	        } catch (IOException e) {
	            csp = null;
	        }
	        iter = new ReplaceableContextIterator();
	        result = new StringBuilder();
	        locCache = new int[1];
	        locCache[0] = 0;
	    }
	
	    /// <summary>
	    /// Implements <see cref="M:IBM.ICU.Text.Transliterator.HandleTransliterate(IBM.ICU.Text.Replaceable, null, System.Boolean)"/>.
	    /// </summary>
	    ///
	    protected internal override void HandleTransliterate(Replaceable text, Transliterator.Position  offsets,
	            bool isIncremental) {
	        // TODO reimplement, see ustrcase.c
	        // using a real word break iterator
	        // instead of just looking for a transition between cased and uncased
	        // characters
	        // call CaseMapTransliterator::handleTransliterate() for lowercasing?
	        // (set fMap)
	        // needs to take isIncremental into account because case mappings are
	        // context-sensitive
	        // also detect when lowercasing function did not finish because of
	        // context
	
	        if (offsets.start >= offsets.limit) {
	            return;
	        }
	
	        // case type: >0 cased (UCaseProps.LOWER etc.) ==0 uncased <0
	        // case-ignorable
	        int type;
	
	        // Our mode; we are either converting letter toTitle or
	        // toLower.
	        bool doTitle = true;
	
	        // Determine if there is a preceding context of cased case-ignorable*,
	        // in which case we want to start in toLower mode. If the
	        // prior context is anything else (including empty) then start
	        // in toTitle mode.
	        int c, start;
	        for (start = offsets.start - 1; start >= offsets.contextStart; start -= IBM.ICU.Text.UTF16
	                .GetCharCount(c)) {
	            c = text.Char32At(start);
	            type = csp.GetTypeOrIgnorable(c);
	            if (type > 0) { // cased
	                doTitle = false;
	                break;
	            } else if (type == 0) { // uncased but not ignorable
	                break;
	            }
	            // else (type<0) case-ignorable: continue
	        }
	
	        // Convert things after a cased character toLower; things
	        // after a uncased, non-case-ignorable character toTitle. Case-ignorable
	        // characters are copied directly and do not change the mode.
	
	        iter.SetText(text);
	        iter.SetIndex(offsets.start);
	        iter.SetLimit(offsets.limit);
	        iter.SetContextLimits(offsets.contextStart, offsets.contextLimit);
	
	        result.Length=0;
	
	        // Walk through original string
	        // If there is a case change, modify corresponding position in
	        // replaceable
	        int delta;
	
	        while ((c = iter.NextCaseMapCP()) >= 0) {
	            type = csp.GetTypeOrIgnorable(c);
	            if (type >= 0) { // not case-ignorable
	                if (doTitle) {
	                    c = csp.ToFullTitle(c, iter, result, locale, locCache);
	                } else {
	                    c = csp.ToFullLower(c, iter, result, locale, locCache);
	                }
	                doTitle = type == 0; // doTitle=isUncased
	
	                if (iter.DidReachLimit() && isIncremental) {
	                    // the case mapping function tried to look beyond the
	                    // context limit
	                    // wait for more input
	                    offsets.start = iter.GetCaseMapCPStart();
	                    return;
	                }
	
	                /* decode the result */
	                if (c < 0) {
	                    /* c mapped to itself, no change */
	                    continue;
	                } else if (c <= IBM.ICU.Impl.UCaseProps.MAX_STRING_LENGTH) {
	                    /* replace by the mapping string */
	                    delta = iter.Replace(result.ToString());
	                    result.Length=0;
	                } else {
	                    /* replace by single-code point mapping */
	                    delta = iter.Replace(IBM.ICU.Text.UTF16.ValueOf(c));
	                }
	
	                if (delta != 0) {
	                    offsets.limit += delta;
	                    offsets.contextLimit += delta;
	                }
	            }
	        }
	        offsets.start = offsets.limit;
	    }
	}
}