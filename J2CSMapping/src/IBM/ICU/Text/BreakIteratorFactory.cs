/*
 *******************************************************************************
 * Copyright (C) 2002-2007, International Business Machines Corporation and    *
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
	
	using IBM.ICU.Impl;
     using IBM.ICU.Util;
     using System;
     using ILOG.J2CsMapping.Util;
     using System.IO;
     using System.Resources;
	
	internal sealed class BreakIteratorFactory : BreakIterator.BreakIteratorServiceShim {
	
	    public override Object RegisterInstance(BreakIterator iter, ULocale locale, int kind) {
	        iter.SetText(new ILOG.J2CsMapping.Text.StringCharacterIterator(""));
	        return service.RegisterObject(iter, locale, kind);
	    }
	
	    public override bool Unregister(Object key) {
	        if (service.IsDefault()) {
	            return false;
	        }
	        return service.UnregisterFactory((IBM.ICU.Impl.ICUService.Factory ) key);
	    }
	
	    public override Locale[] GetAvailableLocales() {
	        if (service == null) {
	            return IBM.ICU.Impl.ICUResourceBundle
	                    .GetAvailableLocales(IBM.ICU.Impl.ICUResourceBundle.ICU_BASE_NAME);
	        } else {
	            return service.GetAvailableLocales();
	        }
	    }
	
	    public override ULocale[] GetAvailableULocales() {
	        if (service == null) {
	            return IBM.ICU.Impl.ICUResourceBundle
	                    .GetAvailableULocales(IBM.ICU.Impl.ICUResourceBundle.ICU_BASE_NAME);
	        } else {
	            return service.GetAvailableULocales();
	        }
	    }
	
	    public override BreakIterator CreateBreakIterator(ULocale locale, int kind) {
	        // TODO: convert to ULocale when service switches over
	        if (service.IsDefault()) {
	            return CreateBreakInstance(locale, kind);
	        }
	        ULocale[] actualLoc = new ULocale[1];
	        BreakIterator iter = (BreakIterator) service.Get(locale, kind,
	                actualLoc);
	        iter.SetLocale(actualLoc[0], actualLoc[0]); // services make no
	                                                    // distinction between
	                                                    // actual & valid
	        return iter;
	    }
	
	    private class BFService : ICULocaleService {
	        internal BFService() : base("BreakIterator") {
	            RegisterFactory(new IBM.ICU.Text.BreakIteratorFactory.BFService.RBBreakIteratorFactory ());
	
	            MarkDefault();
	        }
	
	        internal class RBBreakIteratorFactory : IBM.ICU.Impl.ICULocaleService.ICUResourceBundleFactory  {
	            protected internal override Object HandleCreate(ULocale loc, int kind,
	                    ICUService service) {
	                return IBM.ICU.Text.BreakIteratorFactory.CreateBreakInstance(loc, kind);
	            }
	        }
	    }
	
	    static internal readonly ICULocaleService service = new BreakIteratorFactory.BFService ();
	
	    /// <exclude/>
	    /// <summary>
	    /// KIND_NAMES are the resource key to be used to fetch the name of the
	    /// pre-compiled break rules. The resource bundle name is "boundaries". The
	    /// value for each key will be the rules to be used for the specified locale
	    /// - "word" -> "word_th" for Thai, for example. DICTIONARY_POSSIBLE indexes
	    /// in the same way, and indicates whether a dictionary is a possibility for
	    /// that type of break. This is just an optimization to avoid a resource
	    /// lookup where no dictionary is ever possible.
	    /// </summary>
	    ///
	    private static readonly String[] KIND_NAMES = { "grapheme", "word", "line",
	            "sentence", "title" };
	
	    private static readonly bool[] DICTIONARY_POSSIBLE = { false, true, true,
	            false, false };
	
	    private static BreakIterator CreateBreakInstance(ULocale locale, int kind) {
	
	        BreakIterator iter = null;
	        ICUResourceBundle rb = (ICUResourceBundle) IBM.ICU.Util.UResourceBundle
	                .GetBundleInstance(IBM.ICU.Impl.ICUResourceBundle.ICU_BRKITR_BASE_NAME,
	                        locale);
	
	        //
	        // Get the binary rules. These are needed for both normal
	        // RulesBasedBreakIterators
	        // and for Dictionary iterators.
	        //
	        Stream ruleStream = null;
	        try {
	            String typeKey = KIND_NAMES[kind];
	            String brkfname = rb.GetStringWithFallback("boundaries/" + typeKey);
                String rulesFileName = IBM.ICU.Impl.ICUResourceBundle.ICU_BASE_NAME /*+
                    IBM.ICU.Impl.ICUResourceBundle.ICU_BUNDLE*/
	                    + IBM.ICU.Impl.ICUResourceBundle.ICU_BRKITR_NAME + "/" + brkfname;
	            ruleStream = IBM.ICU.Impl.ICUData.GetStream(rulesFileName);
	        } catch (Exception e) {
	            throw new MissingManifestResourceException(e.ToString());
	        }
	
	        //
	        // Check whether a dictionary exists, and create a DBBI iterator is
	        // one does.
	        //
	        if (DICTIONARY_POSSIBLE[kind]) {
	            // This type of break iterator could potentially use a dictionary.
	            //
	            try {
	                // ICUResourceBundle dictRes =
	                // (ICUResourceBundle)rb.getObject("BreakDictionaryData");
	                // byte[] dictBytes = null;
	                // dictBytes = dictRes.getBinary(dictBytes);
	                // TODO: Hard code this for now! fix it once
	                // CompactTrieDictionary is ported
	                if (locale.Equals("th")) {
	                    String fileName = "data/th.brk";
	                    Stream mask0 = IBM.ICU.Impl.ICUData.GetStream(fileName);
	                    iter = new DictionaryBasedBreakIterator(ruleStream, mask0);
	                }
	            } catch (MissingManifestResourceException e_0) {
	                // Couldn't find a dictionary.
	                // This is normal, and will occur whenever creating a word or
	                // line
	                // break iterator for a locale that does not have a
	                // BreakDictionaryData
	                // resource - meaning for all but Thai.
	                // Fall through to creating a normal RulebasedBreakIterator.
	            } catch (IOException e_1) {
	                IBM.ICU.Impl.Assert.Fail(e_1);
	            }
	        }
	
	        if (iter == null) {
	            //
	            // Create a normal RuleBasedBreakIterator.
	            // We have determined that this is not supposed to be a dictionary
	            // iterator.
	            //
	            try {
	                iter = IBM.ICU.Text.RuleBasedBreakIterator
	                        .GetInstanceFromCompiledRules(ruleStream);
	            } catch (IOException e_2) {
	                // Shouldn't be possible to get here.
	                // If it happens, the compiled rules are probably corrupted in
	                // some way.
	                IBM.ICU.Impl.Assert.Fail(e_2);
	            }
	        }
	        // TODO: Determine valid and actual locale correctly.
	        ULocale uloc = IBM.ICU.Util.ULocale.ForLocale(rb.GetLocale());
	        iter.SetLocale(uloc, uloc);
	
	        return iter;
	
	    }
	
	}
}
