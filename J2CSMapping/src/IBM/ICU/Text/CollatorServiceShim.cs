// --------------------------------------------------------------------------------------------------
// This file was automatically generated by J2CS Translator (http://j2cstranslator.sourceforge.net/). 
// Version 1.3.6.20101125_01     
// 12/2/10 11:30 AM    
// ${CustomMessageForDisclaimer}                                                                             
// --------------------------------------------------------------------------------------------------
 /// <summary>
/// Copyright (C) 2003-2007, International Business Machines Corporation and         
/// others. All Rights Reserved.                                                
/// </summary>
///
namespace IBM.ICU.Text {
	
	using IBM.ICU.Impl;
    using IBM.ICU.Util;
    using System;
    using ILOG.J2CsMapping.Util;
    using System.Resources;
	
	internal sealed class CollatorServiceShim : Collator.ServiceShim {
	
	    internal override Collator GetInstance(ULocale locale) {
	        // use service cache, it's faster than instantiation
	        // if (service.isDefault()) {
	        // return new RuleBasedCollator(locale);
	        // }
	
	        try {
	            ULocale[] actualLoc = new ULocale[1];
	            Collator coll = (Collator) service.Get(locale, actualLoc);
	            if (coll == null) {
	                throw new MissingManifestResourceException("Could not locate Collator data");
	            }
	            coll = (Collator) coll.Clone();
	            coll.SetLocale(actualLoc[0], actualLoc[0]); // services make no
	                                                        // distinction between
	                                                        // actual & valid
	            return coll;
	        } catch (Exception e) {
	            // /CLOVER:OFF
	            throw new InvalidOperationException(e.Message);
	            // /CLOVER:ON
	        }
	    }
	
	    internal override Object RegisterInstance(Collator collator, ULocale locale) {
	        return service.RegisterObject(collator, locale);
	    }
	
	    internal override Object RegisterFactory(IBM.ICU.Text.Collator.CollatorFactory  f) {
	        
	
	        return service.RegisterFactory(new CollatorServiceShim.CFactory (f));
	    }
	
	    internal override bool Unregister(Object registryKey) {
	        return service.UnregisterFactory((IBM.ICU.Impl.ICUService.Factory ) registryKey);
	    }
	
	    internal override Locale[] GetAvailableLocales() {
	        // TODO rewrite this to just wrap getAvailableULocales later
	        if (service.IsDefault()) {
	            return IBM.ICU.Impl.ICUResourceBundle
	                    .GetAvailableLocales(IBM.ICU.Impl.ICUResourceBundle.ICU_COLLATION_BASE_NAME);
	        }
	        return service.GetAvailableLocales();
	    }
	
	    internal override ULocale[] GetAvailableULocales() {
	        if (service.IsDefault()) {
	            return IBM.ICU.Impl.ICUResourceBundle
	                    .GetAvailableULocales(IBM.ICU.Impl.ICUResourceBundle.ICU_COLLATION_BASE_NAME);
	        }
	        return service.GetAvailableULocales();
	    }
	
	    internal override String GetDisplayName(ULocale objectLocale, ULocale displayLocale) {
	        String id = objectLocale.GetName();
	        return service.GetDisplayName(id, displayLocale);
	    }
	
	    private class CService : ICULocaleService {
	        internal CService() : base("Collator") {
	            this.RegisterFactory(new IBM.ICU.Text.CollatorServiceShim.CService.CollatorFactory ());
	            MarkDefault();
	        }
	
	        protected internal override Object HandleDefault(IBM.ICU.Impl.ICUService.Key  key, String[] actualIDReturn) {
	            if (actualIDReturn != null) {
	                actualIDReturn[0] = "root";
	            }
	            try {
	                return new RuleBasedCollator(IBM.ICU.Util.ULocale.ROOT);
	            } catch (MissingManifestResourceException e) {
	                return null;
	            }
	        }
	
	        internal class CollatorFactory : IBM.ICU.Impl.ICULocaleService.ICUResourceBundleFactory  {
	            internal CollatorFactory() : base(IBM.ICU.Impl.ICUResourceBundle.ICU_COLLATION_BASE_NAME) {
	            }
	
	            protected internal override Object HandleCreate(ULocale uloc, int kind,
	                    ICUService service) {
	                return new RuleBasedCollator(uloc);
	            }
	        }
	    }
	
	    private static ICULocaleService service = new CollatorServiceShim.CService ();
	
	    internal class CFactory : IBM.ICU.Impl.ICULocaleService.LocaleKeyFactory  {
	        internal IBM.ICU.Text.Collator.CollatorFactory  delegat0;
	
	        internal CFactory(IBM.ICU.Text.Collator.CollatorFactory  f) : base(f.Visible()) {
	            this.delegat0 = f;
	        }
	
	        protected internal override Object HandleCreate(ULocale loc, int kind, ICUService service_0) {
	            Object coll = delegat0.CreateCollator(loc);
	            return coll;
	        }
	
	        public override String GetDisplayName(String id, ULocale displayLocale) {
	            ULocale objectLocale = new ULocale(id);
	            return delegat0.GetDisplayName(objectLocale, displayLocale);
	        }

            protected internal override ILOG.J2CsMapping.Collections.ISet GetSupportedIDs()
            {
	            return delegat0.GetSupportedLocaleIDs();
	        }
	    }
	}
}
