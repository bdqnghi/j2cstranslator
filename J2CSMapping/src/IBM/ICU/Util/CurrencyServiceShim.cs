// --------------------------------------------------------------------------------------------------
// This file was automatically generated by J2CS Translator (http://j2cstranslator.sourceforge.net/). 
// Version 1.3.6.20101125_01     
// 12/2/10 11:47 AM    
// ${CustomMessageForDisclaimer}                                                                             
// --------------------------------------------------------------------------------------------------
 /// <summary>
/// Copyright (C) 2001-2004, International Business Machines Corporation and    
/// others. All Rights Reserved.                                                
/// </summary>
///
namespace IBM.ICU.Util {
	
	using IBM.ICU.Impl;
    using ILOG.J2CsMapping.Util;
    using System;
	
	/// <summary>
	/// This is a package-access implementation of registration for currency. The
	/// shim is instantiated by reflection in Currency, all dependencies on
	/// ICUService are located in this file. This structure is to allow ICU4J to be
	/// built without service registration support.
	/// </summary>
	///
	internal sealed class CurrencyServiceShim : Currency.ServiceShim {
	
	    internal override Locale[] GetAvailableLocales() {
	        if (service.IsDefault()) {
	            return IBM.ICU.Impl.ICUResourceBundle
	                    .GetAvailableLocales(IBM.ICU.Impl.ICUResourceBundle.ICU_BASE_NAME);
	        }
	        return service.GetAvailableLocales();
	    }
	
	    internal override ULocale[] GetAvailableULocales() {
	        if (service.IsDefault()) {
	            return IBM.ICU.Impl.ICUResourceBundle
	                    .GetAvailableULocales(IBM.ICU.Impl.ICUResourceBundle.ICU_BASE_NAME);
	        }
	        return service.GetAvailableULocales();
	    }
	
	    internal override Currency CreateInstance(ULocale loc) {
	        // TODO: convert to ULocale when service switches over
	
	        if (service.IsDefault()) {
	            return IBM.ICU.Util.Currency.CreateCurrency(loc);
	        }
	        ULocale[] actualLoc = new ULocale[1];
	        Currency curr = (Currency) service.Get(loc, actualLoc);
	        ULocale uloc = actualLoc[0];
	        curr.SetLocale(uloc, uloc); // services make no distinction between
	                                    // actual & valid
	        return curr;
	    }
	
	    internal override Object RegisterInstance(Currency currency, ULocale locale) {
	        return service.RegisterObject(currency, locale);
	    }
	
	    internal override bool Unregister(Object registryKey) {
	        return service.UnregisterFactory((IBM.ICU.Impl.ICUService.Factory ) registryKey);
	    }
	
	    private class CFService : ICULocaleService {
	        internal CFService() : base("Currency") {
	            RegisterFactory(new IBM.ICU.Util.CurrencyServiceShim.CFService.CurrencyFactory ());
	            MarkDefault();
	        }
	
	        internal class CurrencyFactory : IBM.ICU.Impl.ICULocaleService.ICUResourceBundleFactory  {
	            protected internal override Object HandleCreate(ULocale loc, int kind,
	                    ICUService service) {
	                return IBM.ICU.Util.Currency.CreateCurrency(loc);
	            }
	        }
	    }
	
	    static internal readonly ICULocaleService service = new CurrencyServiceShim.CFService ();
	}
}