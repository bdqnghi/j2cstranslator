// --------------------------------------------------------------------------------------------------
// This file was automatically generated by J2CS Translator (http://j2cstranslator.sourceforge.net/). 
// Version 1.3.6.20101125_01     
// 12/2/10 11:47 AM    
// ${CustomMessageForDisclaimer}                                                                             
// --------------------------------------------------------------------------------------------------
 /// <summary>
/// Copyright (C) 2001-2007, International Business Machines Corporation and    
/// others. All Rights Reserved.                                                
/// </summary>
///
namespace IBM.ICU.Impl {
	
	using IBM.ICU.Util;
	using ILOG.J2CsMapping.Collections;
	using ILOG.J2CsMapping.Collections.Generics;
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Globalization;
	using System.IO;
	using System.Runtime.CompilerServices;
	using System.Text;
	
	public class ICULocaleService : ICUService {
	    private ULocale fallbackLocale;
	
	    private String fallbackLocaleName;
	
	    /// <summary>
	    /// Construct an ICULocaleService.
	    /// </summary>
	    ///
	    public ICULocaleService() {
	    }
	
	    /// <summary>
	    /// Construct an ICULocaleService with a name (useful for debugging).
	    /// </summary>
	    ///
	    public ICULocaleService(String name) : base(name) {
	    }
	
	    /// <summary>
	    /// Convenience override for callers using locales. This calls get(ULocale,
	    /// int, ULocale[]) with KIND_ANY for kind and null for actualReturn.
	    /// </summary>
	    ///
	    public Object Get(ULocale locale) {
	        return Get(locale, IBM.ICU.Impl.ICULocaleService.LocaleKey.KIND_ANY, null);
	    }
	
	    /// <summary>
	    /// Convenience override for callers using locales. This calls get(ULocale,
	    /// int, ULocale[]) with a null actualReturn.
	    /// </summary>
	    ///
	    public Object Get(ULocale locale, int kind) {
	        return Get(locale, kind, null);
	    }
	
	    /// <summary>
	    /// Convenience override for callers using locales. This calls get(ULocale,
	    /// int, ULocale[]) with KIND_ANY for kind.
	    /// </summary>
	    ///
	    public Object Get(ULocale locale, ULocale[] actualReturn) {
	        return Get(locale, IBM.ICU.Impl.ICULocaleService.LocaleKey.KIND_ANY, actualReturn);
	    }
	
	    /// <summary>
	    /// Convenience override for callers using locales. This uses
	    /// createKey(ULocale.toString(), kind) to create a key, calls getKey, and
	    /// then if actualReturn is not null, returns the actualResult from getKey
	    /// (stripping any prefix) into a ULocale.
	    /// </summary>
	    ///
	    public Object Get(ULocale locale, int kind, ULocale[] actualReturn) {
	        ICUService.Key  key = CreateKey(locale, kind);
	        if (actualReturn == null) {
	            return GetKey(key);
	        }
	
	        String[] temp = new String[1];
	        Object result = GetKey(key, temp);
	        if (result != null) {
	            int n = temp[0].IndexOf("/");
	            if (n >= 0) {
	                temp[0] = temp[0].Substring(n + 1);
	            }
	            actualReturn[0] = new ULocale(temp[0]);
	        }
	        return result;
	    }
	
	    /// <summary>
	    /// Convenience override for callers using locales. This calls
	    /// registerObject(Object, ULocale, int kind, boolean visible) passing
	    /// KIND_ANY for the kind, and true for the visibility.
	    /// </summary>
	    ///
	    public ICUService.Factory  RegisterObject(Object obj, ULocale locale) {
	        return RegisterObject(obj, locale, IBM.ICU.Impl.ICULocaleService.LocaleKey.KIND_ANY, true);
	    }
	
	    /// <summary>
	    /// Convenience override for callers using locales. This calls
	    /// registerObject(Object, ULocale, int kind, boolean visible) passing
	    /// KIND_ANY for the kind.
	    /// </summary>
	    ///
	    public ICUService.Factory  RegisterObject(Object obj, ULocale locale, bool visible) {
	        return RegisterObject(obj, locale, IBM.ICU.Impl.ICULocaleService.LocaleKey.KIND_ANY, visible);
	    }
	
	    /// <summary>
	    /// Convenience function for callers using locales. This calls
	    /// registerObject(Object, ULocale, int kind, boolean visible) passing true
	    /// for the visibility.
	    /// </summary>
	    ///
	    public ICUService.Factory  RegisterObject(Object obj, ULocale locale, int kind) {
	        return RegisterObject(obj, locale, kind, true);
	    }
	
	    /// <summary>
	    /// Convenience function for callers using locales. This instantiates a
	    /// SimpleLocaleKeyFactory, and registers the factory.
	    /// </summary>
	    ///
	    public ICUService.Factory  RegisterObject(Object obj, ULocale locale, int kind,
	            bool visible) {
	        ICUService.Factory  factory = new ICULocaleService.SimpleLocaleKeyFactory (obj, locale, kind, visible);
	        return RegisterFactory(factory);
	    }
	
	    /// <summary>
	    /// Convenience method for callers using locales. This returns the standard
	    /// Locale list, built from the Set of visible ids.
	    /// </summary>
	    ///
	    public ILOG.J2CsMapping.Util.Locale[] GetAvailableLocales() {
	        // TODO make this wrap getAvailableULocales later
	        ILOG.J2CsMapping.Collections.ISet visIDs = GetVisibleIDs();
	        IIterator iter = new ILOG.J2CsMapping.Collections.IteratorAdapter(visIDs.GetEnumerator());
            ILOG.J2CsMapping.Util.Locale[] locales = new ILOG.J2CsMapping.Util.Locale[visIDs.Count];
	        int n = 0;
	        while (iter.HasNext()) {
                ILOG.J2CsMapping.Util.Locale loc = IBM.ICU.Impl.LocaleUtility.GetLocaleFromName((String)iter.Next());
	            locales[n++] = loc;
	        }
	        return locales;
	    }
	
	    /// <summary>
	    /// Convenience method for callers using locales. This returns the standard
	    /// ULocale list, built from the Set of visible ids.
	    /// </summary>
	    ///
	    public ULocale[] GetAvailableULocales() {
	        ILOG.J2CsMapping.Collections.ISet visIDs = GetVisibleIDs();
	        IIterator iter = new ILOG.J2CsMapping.Collections.IteratorAdapter(visIDs.GetEnumerator());
	        ULocale[] locales = new ULocale[visIDs.Count];
	        int n = 0;
	        while (iter.HasNext()) {
	            locales[n++] = new ULocale((String) iter.Next());
	        }
	        return locales;
	    }
	
	    /// <summary>
	    /// A subclass of Key that implements a locale fallback mechanism. The first
	    /// locale to search for is the locale provided by the client, and the
	    /// fallback locale to search for is the current default locale. If a prefix
	    /// is present, the currentDescriptor includes it before the locale proper,
	    /// separated by "/". This is the default key instantiated by
	    /// ICULocaleService.</p>
	    /// <p>
	    /// Canonicalization adjusts the locale string so that the section before the
	    /// first understore is in lower case, and the rest is in upper case, with no
	    /// trailing underscores.
	    /// </p>
	    /// </summary>
	    ///
	    public class LocaleKey : ICUService.Key {
	        private int kind;
	
	        private int varstart;
	
	        private String primaryID;
	
	        private String fallbackID;
	
	        private String currentID;
	
	        public const int KIND_ANY = -1;
	
	        /// <summary>
	        /// Create a LocaleKey with canonical primary and fallback IDs.
	        /// </summary>
	        ///
	        public static ICULocaleService.LocaleKey  CreateWithCanonicalFallback(String primaryID_0,
	                String canonicalFallbackID) {
	            return CreateWithCanonicalFallback(primaryID_0, canonicalFallbackID,
	                    KIND_ANY);
	        }
	
	        /// <summary>
	        /// Create a LocaleKey with canonical primary and fallback IDs.
	        /// </summary>
	        ///
	        public static ICULocaleService.LocaleKey  CreateWithCanonicalFallback(String primaryID_0,
	                String canonicalFallbackID, int kind_1) {
	            if (primaryID_0 == null) {
	                return null;
	            }
	            if (primaryID_0.Length == 0) {
	                primaryID_0 = "root";
	            }
	            String canonicalPrimaryID = IBM.ICU.Util.ULocale.GetName(primaryID_0);
	            return new ICULocaleService.LocaleKey (primaryID_0, canonicalPrimaryID,
	                    canonicalFallbackID, kind_1);
	        }
	
	        /// <summary>
	        /// Create a LocaleKey with canonical primary and fallback IDs.
	        /// </summary>
	        ///
	        public static ICULocaleService.LocaleKey  CreateWithCanonical(ULocale locale,
	                String canonicalFallbackID, int kind_0) {
	            if (locale == null) {
	                return null;
	            }
	            String canonicalPrimaryID = locale.GetName();
	            return new ICULocaleService.LocaleKey (canonicalPrimaryID, canonicalPrimaryID,
	                    canonicalFallbackID, kind_0);
	        }
	
	        /// <summary>
	        /// PrimaryID is the user's requested locale string, canonicalPrimaryID
	        /// is this string in canonical form, fallbackID is the current default
	        /// locale's string in canonical form.
	        /// </summary>
	        ///
	        protected internal LocaleKey(String primaryID_0, String canonicalPrimaryID,
	                String canonicalFallbackID, int kind_1) : base(primaryID_0) {
	            this.kind = kind_1;
	            if (canonicalPrimaryID == null) {
	                this.primaryID = "";
	            } else {
	                this.primaryID = canonicalPrimaryID;
	                this.varstart = this.primaryID.IndexOf('@');
	            }
	            if (this.primaryID == "") {
	                this.fallbackID = null;
	            } else {
	                if (canonicalFallbackID == null
	                        || this.primaryID.Equals(canonicalFallbackID)) {
	                    this.fallbackID = "";
	                } else {
	                    this.fallbackID = canonicalFallbackID;
	                }
	            }
	
	            this.currentID = (varstart == -1) ? this.primaryID : this.primaryID.Substring(0,(varstart)-(0));
	        }
	
	        /// <summary>
	        /// Return the prefix associated with the kind, or null if the kind is
	        /// KIND_ANY.
	        /// </summary>
	        ///
	        public String Prefix() {
	            return (kind == KIND_ANY) ? null : ILOG.J2CsMapping.Util.IlNumber.ToString(Kind());
	        }
	
	        /// <summary>
	        /// Return the kind code associated with this key.
	        /// </summary>
	        ///
	        public int Kind() {
	            return kind;
	        }
	
	        /// <summary>
	        /// Return the (canonical) original ID.
	        /// </summary>
	        ///
	        public override String CanonicalID() {
	            return primaryID;
	        }
	
	        /// <summary>
	        /// Return the (canonical) current ID, or null if no current id.
	        /// </summary>
	        ///
	        public override String CurrentID() {
	            return currentID;
	        }
	
	        /// <summary>
	        /// Return the (canonical) current descriptor, or null if no current id.
	        /// Includes the keywords, whereas the ID does not include keywords.
	        /// </summary>
	        ///
	        public override String CurrentDescriptor() {
	            String result = CurrentID();
	            if (result != null) {
	                StringBuilder buf = new StringBuilder(); // default capacity 16 is
	                                                       // usually good enough
	                if (kind != KIND_ANY) {
	                    buf.Append(Prefix());
	                }
	                buf.Append('/');
	                buf.Append(result);
	                if (varstart != -1) {
	                    buf.Append(primaryID.Substring(varstart,(primaryID.Length)-(varstart)));
	                }
	                result = buf.ToString();
	            }
	            return result;
	        }
	
	        /// <summary>
	        /// Convenience method to return the locale corresponding to the
	        /// (canonical) original ID.
	        /// </summary>
	        ///
	        public ULocale CanonicalLocale() {
	            return new ULocale(primaryID);
	        }
	
	        /// <summary>
	        /// Convenience method to return the ulocale corresponding to the
	        /// (canonical) currentID.
	        /// </summary>
	        ///
	        public ULocale CurrentLocale() {
	            if (varstart == -1) {
	                return new ULocale(currentID);
	            } else {
	                return new ULocale(currentID + primaryID.Substring(varstart));
	            }
	        }
	
	        /// <summary>
	        /// If the key has a fallback, modify the key and return true, otherwise
	        /// return false.</p>
	        /// <p>
	        /// First falls back through the primary ID, then through the fallbackID.
	        /// The final fallback is "root" unless the primary id was "root", in
	        /// which case there is no fallback.
	        /// </summary>
	        ///
	        public override bool Fallback() {
	            int x = currentID.LastIndexOf('_');
	            if (x != -1) {
	                while (--x >= 0 && currentID[x] == '_') { // handle
	                                                                 // zh__PINYIN
	                }
	                currentID = currentID.Substring(0,(x + 1)-(0));
	                return true;
	            }
	            if (fallbackID != null) {
	                if (fallbackID.Length == 0) {
	                    currentID = "root";
	                    fallbackID = null;
	                } else {
	                    currentID = fallbackID;
	                    fallbackID = "";
	                }
	                return true;
	            }
	            currentID = null;
	            return false;
	        }
	
	        /// <summary>
	        /// If a key created from id would eventually fallback to match the
	        /// canonical ID of this key, return true.
	        /// </summary>
	        ///
	        public override bool IsFallbackOf(String id) {
	            return IBM.ICU.Impl.LocaleUtility.IsFallbackOf(CanonicalID(), id);
	        }
	    }
	
	    /// <summary>
	    /// A subclass of Factory that uses LocaleKeys. If 'visible' the factory
	    /// reports its IDs.
	    /// </summary>
	    ///
	    public abstract class LocaleKeyFactory : ICUService.Factory  {
	        protected internal readonly String name;
	
	        protected internal readonly bool visible;
	
	        public const bool VISIBLE = true;
	
	        public const bool INVISIBLE = false;
	
	        /// <summary>
	        /// Constructor used by subclasses.
	        /// </summary>
	        ///
	        protected internal LocaleKeyFactory(bool visible_0) {
	            this.visible = visible_0;
	            this.name = null;
	        }
	
	        /// <summary>
	        /// Constructor used by subclasses.
	        /// </summary>
	        ///
	        protected internal LocaleKeyFactory(bool visible_0, String name_1) {
	            this.visible = visible_0;
	            this.name = name_1;
	        }
	
	        /// <summary>
	        /// Implement superclass abstract method. This checks the currentID of
	        /// the key against the supported IDs, and passes the canonicalLocale and
	        /// kind off to handleCreate (which subclasses must implement).
	        /// </summary>
	        ///
	        public virtual Object Create(ICUService.Key  key, ICUService service) {
	            if (HandlesKey(key)) {
	                ICULocaleService.LocaleKey  lkey = (ICULocaleService.LocaleKey ) key;
	                int kind_0 = lkey.Kind();
	
	                ULocale uloc = lkey.CurrentLocale();
	                return HandleCreate(uloc, kind_0, service);
	            } else {
	                // System.out.println("factory: " + this +
	                // " did not support id: " + key.currentID());
	                // System.out.println("supported ids: " + getSupportedIDs());
	            }
	            return null;
	        }
	
	        protected internal bool HandlesKey(ICUService.Key  key) {
	            if (key != null) {
	                String id = key.CurrentID();
	                ILOG.J2CsMapping.Collections.ISet supported = GetSupportedIDs();
	                return ILOG.J2CsMapping.Collections.Collections.Contains(id,supported);
	            }
	            return false;
	        }
	
	        /// <summary>
	        /// Override of superclass method.
	        /// </summary>
	        ///
	        public virtual void UpdateVisibleIDs(IDictionary result) {
	            ILOG.J2CsMapping.Collections.ISet cache = GetSupportedIDs();
	            IIterator iter = new ILOG.J2CsMapping.Collections.IteratorAdapter(cache.GetEnumerator());
	            while (iter.HasNext()) {
	                String id = (String) iter.Next();
	                if (visible) {
	                    ILOG.J2CsMapping.Collections.Collections.Put(result,id,this);
	                } else {
	                    ILOG.J2CsMapping.Collections.Collections.Remove(result,id);
	                }
	            }
	        }
	
	        /// <summary>
	        /// Return a localized name for the locale represented by id.
	        /// </summary>
	        ///
	        public virtual String GetDisplayName(String id, ULocale locale) {
	            // assume if the user called this on us, we must have handled some
	            // fallback of this id
	            // if (isSupportedID(id)) {
	            if (locale == null) {
	                return id;
	            }
	            ULocale loc = new ULocale(id);
	            return loc.GetDisplayName(locale);
	            // }
	            // return null;
	        }
	
	        // /CLOVER:OFF
	        /// <summary>
	        /// Utility method used by create(Key, ICUService). Subclasses can
	        /// implement this instead of create.
	        /// </summary>
	        ///
	        protected internal virtual Object HandleCreate(ULocale loc, int kind_0, ICUService service) {
	            return null;
	        }
	
	        // /CLOVER:ON
	
	        /// <summary>
	        /// Return true if this id is one the factory supports (visible or
	        /// otherwise).
	        /// </summary>
	        ///
	        protected internal virtual bool IsSupportedID(String id) {
	            return ILOG.J2CsMapping.Collections.Collections.Contains(id,GetSupportedIDs());
	        }
	
	        /// <summary>
	        /// Return the set of ids that this factory supports (visible or
	        /// otherwise). This can be called often and might need to be cached if
	        /// it is expensive to create.
	        /// </summary>
	        ///
            protected internal virtual ILOG.J2CsMapping.Collections.ISet GetSupportedIDs()
            {
	            return ILOG.J2CsMapping.Collections.Generics.Collections.EMPTY_SET;
	        }
	
	        /// <summary>
	        /// For debugging.
	        /// </summary>
	        ///
	        public override String ToString() {
	            StringBuilder buf = new StringBuilder(base.ToString());
	            if (name != null) {
	                buf.Append(", name: ");
	                buf.Append(name);
	            }
	            buf.Append(", visible: ");
	            buf.Append(visible);
	            return buf.ToString();
	        }
	    }
	
	    /// <summary>
	    /// A LocaleKeyFactory that just returns a single object for a kind/locale.
	    /// </summary>
	    ///
	    public class SimpleLocaleKeyFactory : ICULocaleService.LocaleKeyFactory  {
	        private readonly Object obj;
	
	        private readonly String id;
	
	        private readonly int kind;
	
	        // TODO: remove when we no longer need this
	        public SimpleLocaleKeyFactory(Object obj_0, ULocale locale, int kind_1,
	                bool visible_2) : this(obj_0, locale, kind_1, visible_2, null) {
	        }
	
	        public SimpleLocaleKeyFactory(Object obj_0, ULocale locale, int kind_1,
	                bool visible_2, String name_3) : base(visible_2, name_3) {
	            this.obj = obj_0;
	            this.id = locale.GetBaseName();
	            this.kind = kind_1;
	        }
	
	        /// <summary>
	        /// Returns the service object if kind/locale match. Service is not used.
	        /// </summary>
	        ///
	        public override Object Create(ICUService.Key  key, ICUService service) {
	            ICULocaleService.LocaleKey  lkey = (ICULocaleService.LocaleKey ) key;
	            if (kind == IBM.ICU.Impl.ICULocaleService.LocaleKey.KIND_ANY || kind == lkey.Kind()) {
	                String keyID = lkey.CurrentID();
	                if (id.Equals(keyID)) {
	                    return obj;
	                }
	            }
	            return null;
	        }
	
	        protected internal override bool IsSupportedID(String idToCheck) {
	            return this.id.Equals(idToCheck);
	        }
	
	        public override void UpdateVisibleIDs(IDictionary result) {
	            if (visible) {
	                ILOG.J2CsMapping.Collections.Collections.Put(result,id,this);
	            } else {
	                ILOG.J2CsMapping.Collections.Collections.Remove(result,id);
	            }
	        }
	
	        public override String ToString() {
	            StringBuilder buf = new StringBuilder(base.ToString());
	            buf.Append(", id: ");
	            buf.Append(id);
	            buf.Append(", kind: ");
	            buf.Append(kind);
	            return buf.ToString();
	        }
	    }
	
	    /// <summary>
	    /// A LocaleKeyFactory that creates a service based on the ICU locale data.
	    /// This is a base class for most ICU factories. Subclasses instantiate it
	    /// with a constructor that takes a bundle name, which determines the
	    /// supported IDs. Subclasses then override handleCreate to create the actual
	    /// service object. The default implementation returns a resource bundle.
	    /// </summary>
	    ///
	    public class ICUResourceBundleFactory : ICULocaleService.LocaleKeyFactory  {
	        protected internal readonly String bundleName;
	
	        /// <summary>
	        /// Convenience constructor that uses the main ICU bundle name.
	        /// </summary>
	        ///
	        public ICUResourceBundleFactory() : this(IBM.ICU.Impl.ICUResourceBundle.ICU_BASE_NAME) {
	        }
	
	        /// <summary>
	        /// A service factory based on ICU resource data in resources with the
	        /// given name.
	        /// </summary>
	        ///
	        public ICUResourceBundleFactory(String bundleName_0) : base(true) {
	            this.bundleName = bundleName_0;
	        }
	
	        /// <summary>
	        /// Return the supported IDs. This is the set of all locale names for the
	        /// bundleName.
	        /// </summary>
	        ///
            protected internal override ILOG.J2CsMapping.Collections.ISet GetSupportedIDs()
            {
	            // note: "root" is one of the ids, but "" is not. Must convert
	            // ULocale.ROOT.
	            return IBM.ICU.Impl.ICUResourceBundle.GetFullLocaleNameSet(bundleName);
	        }
	
	        /// <summary>
	        /// Override of superclass method.
	        /// </summary>
	        ///
	        public override void UpdateVisibleIDs(IDictionary result) {
	            ILOG.J2CsMapping.Collections.ISet visibleIDs = IBM.ICU.Impl.ICUResourceBundle
	                    .GetAvailableLocaleNameSet(bundleName); // only visible ids
	            IIterator iter = new ILOG.J2CsMapping.Collections.IteratorAdapter(visibleIDs.GetEnumerator());
	            while (iter.HasNext()) {
	                String id_0 = (String) iter.Next();
	                ILOG.J2CsMapping.Collections.Collections.Put(result,id_0,this);
	            }
	        }
	
	        /// <summary>
	        /// Create the service. The default implementation returns the resource
	        /// bundle for the locale, ignoring kind, and service.
	        /// </summary>
	        ///
	        protected internal override Object HandleCreate(ULocale loc, int kind_0, ICUService service) {
	            return IBM.ICU.Util.UResourceBundle.GetBundleInstance(bundleName, loc);
	        }
	
	        public override String ToString() {
	            return base.ToString() + ", bundle: " + bundleName;
	        }
	    }
	
	    /// <summary>
	    /// Return the name of the current fallback locale. If it has changed since
	    /// this was last accessed, the service cache is cleared.
	    /// </summary>
	    ///
	    public String ValidateFallbackLocale() {
	        ULocale loc = IBM.ICU.Util.ULocale.GetDefault();
	        if (loc != fallbackLocale) {
	             lock (this) {
	                            if (loc != fallbackLocale) {
	                                fallbackLocale = loc;
	                                fallbackLocaleName = loc.GetBaseName();
	                                ClearServiceCache();
	                            }
	                        }
	        }
	        return fallbackLocaleName;
	    }
	
	    public override ICUService.Key  CreateKey(String id_0) {
	        return IBM.ICU.Impl.ICULocaleService.LocaleKey.CreateWithCanonicalFallback(id_0,
	                ValidateFallbackLocale());
	    }
	
	    public ICUService.Key  CreateKey(String id_0, int kind_1) {
	        return IBM.ICU.Impl.ICULocaleService.LocaleKey.CreateWithCanonicalFallback(id_0,
	                ValidateFallbackLocale(), kind_1);
	    }
	
	    public ICUService.Key  CreateKey(ULocale l, int kind_0) {
	        return IBM.ICU.Impl.ICULocaleService.LocaleKey.CreateWithCanonical(l, ValidateFallbackLocale(), kind_0);
	    }
	}
}