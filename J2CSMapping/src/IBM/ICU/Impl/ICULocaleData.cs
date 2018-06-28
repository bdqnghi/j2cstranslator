/*
 * (C) Copyright IBM Corp. 2002-2007 - All Rights Reserved
 */

// --------------------------------------------------------------------------------------------------
// This file was automatically generated by J2CS Translator (http://j2cstranslator.sourceforge.net/). 
// Version 1.3.6.20101125_01     
// 12/2/10 11:47 AM    
// ${CustomMessageForDisclaimer}                                                                             
// --------------------------------------------------------------------------------------------------
 namespace IBM.ICU.Impl {
	
	using IBM.ICU.Util;
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Globalization;
	using System.IO;
	using System.Reflection;
	using System.Resources;
	using System.Runtime.CompilerServices;
     using ILOG.J2CsMapping.Util;
     using ILOG.J2CsMapping.Util;
	
	/// <summary>
	/// Provides information about and access to resource bundles in the
	/// com.ibm.text.resources package. Unlike the java version, this does not
	/// include resources from any other location. In particular, it does not look in
	/// the boot or system class path.
	/// </summary>
	///
	public class ICULocaleData {
	
	    /// <summary>
	    /// The package for ICU locale data.
	    /// </summary>
	    ///
	    private const String ICU_PACKAGE = "com.ibm.icu.impl.data";
	
	    /// <summary>
	    /// The base name (bundle name) for ICU locale data.
	    /// </summary>
	    ///
	    internal const String LOCALE_ELEMENTS = "LocaleElements";
	
	    /// <summary>
	    /// Creates a LocaleElements resource bundle for the given locale in the
	    /// default ICU package.
	    /// </summary>
	    ///
	    /// <param name="locale">the locale of the bundle to retrieve, or null to use thedefault locale</param>
	    /// <returns>a ResourceBundle for the LocaleElements of the given locale, or
	    /// null on failure</returns>
        public static ResourceBundle GetLocaleElements(ULocale locale)
        {
	        if (locale == null) {
	            locale = IBM.ICU.Util.ULocale.GetDefault();
	        }
	        return GetResourceBundle(ICU_PACKAGE, LOCALE_ELEMENTS,
	                locale.GetBaseName());
	    }
	
	    /// <summary>
	    /// Creates a LocaleElements resource bundle for the given locale in the
	    /// default ICU package.
	    /// </summary>
	    ///
	    /// <param name="localeName">the string name of the locale of the bundle to retrieve, e.g.,"en_US".</param>
	    /// <returns>a ResourceBundle for the LocaleElements of the given locale, or
	    /// null on failure</returns>
        public static ResourceBundle GetLocaleElements(String localeName)
        {
	        return GetResourceBundle(ICU_PACKAGE, LOCALE_ELEMENTS, localeName);
	    }
	
	    /// <summary>
	    /// Creates a resource bundle for the given base name and locale in the
	    /// default ICU package.
	    /// </summary>
	    ///
	    /// <param name="bundleName">the base name of the bundle to retrieve, e.g."LocaleElements".</param>
	    /// <param name="localeName">the string name of the locale of the bundle to retrieve, e.g."en_US".</param>
	    /// <returns>a ResourceBundle with the given base name for the given locale,
	    /// or null on failure</returns>
        public static ResourceBundle GetResourceBundle(String bundleName,
	            String localeName) {
	        return GetResourceBundle(ICU_PACKAGE, bundleName, localeName);
	    }
	
	    /// <summary>
	    /// Creates a resource bundle for the given base name and locale in the
	    /// default ICU package.
	    /// </summary>
	    ///
	    /// <param name="bundleName">the base name of the bundle to retrieve, e.g."LocaleElements".</param>
	    /// <param name="locale">the locale of the bundle to retrieve, or null to use thedefault locale</param>
	    /// <returns>a ResourceBundle with the given base name for the given locale,
	    /// or null on failure</returns>
        public static ResourceBundle GetResourceBundle(String bundleName,
	            ULocale locale) {
	        if (locale == null) {
	            locale = IBM.ICU.Util.ULocale.GetDefault();
	        }
	        return GetResourceBundle(ICU_PACKAGE, bundleName, locale.GetBaseName());
	    }
	
	    /// <summary>
	    /// Creates a resource bundle for the given package, base name, and locale.
	    /// </summary>
	    ///
	    /// <param name="packageName">a package name, e.g., "com.ibm.icu.impl.data".</param>
	    /// <param name="bundleName">the base name of the bundle to retrieve, e.g."LocaleElements".</param>
	    /// <param name="localeName">the string name of the locale of the bundle to retrieve, e.g."en_US".</param>
	    /// <returns>the first ResourceBundle with the given base name for the given
	    /// locale found in each of the packages, or null on failure</returns>
        public static ResourceBundle GetResourceBundle(String packageName,
	            String bundleName, String localeName) {
	        try {
	            String path = packageName + "." + bundleName;
	            if (DEBUG)
	                System.Console.Out.WriteLine("calling instantiate: " + path + "_"
	                        + localeName);
	            return Instantiate(path, localeName);
	        } catch (MissingManifestResourceException e) {
	            if (DEBUG)
	                System.Console.Out.WriteLine(bundleName + "_" + localeName
	                        + " not found in " + packageName);
	            throw e;
	        }
	    }
	
	    /// <summary>
	    /// Creates a resource bundle for the given base name and locale in one of
	    /// the given packages, trying each package in order.
	    /// </summary>
	    ///
	    /// <param name="packages">a list of one or more package names</param>
	    /// <param name="bundleName">the base name of the bundle to retrieve, e.g."LocaleElements".</param>
	    /// <param name="localeName">the string name of the locale of the bundle to retrieve, e.g."en_US".</param>
	    /// <returns>the first ResourceBundle with the given base name for the given
	    /// locale found in each of the packages, or null on failure</returns>
        public static ResourceBundle GetResourceBundle(String[] packages,
	            String bundleName, String localeName) {
                    ResourceBundle r = null;
	        for (int i = 0; r == null && i < packages.Length; ++i) {
	            r = GetResourceBundle(packages[i], bundleName, localeName);
	        }
	        return r;
	    }
	
	    /// <summary>
	    /// Get a resource bundle from the resource bundle path. Unlike
	    /// getResourceBundle, this returns an 'unparented' bundle that exactly
	    /// matches the bundle name and locale name.
	    /// </summary>
	    ///
	    public static ResourceManager LoadResourceBundle(String bundleName,
	            ULocale locale) {
	        if (locale == null) {
	            locale = IBM.ICU.Util.ULocale.GetDefault();
	        }
	        return LoadResourceBundle(bundleName, locale.GetBaseName());
	    }
	
	    /// <summary>
	    /// Get a resource bundle from the resource bundle path. Unlike
	    /// getResourceBundle, this returns an 'unparented' bundle that exactly
	    /// matches the bundle name and locale name.
	    /// </summary>
	    ///
	    public static ResourceManager LoadResourceBundle(String bundleName,
	            String localeName) {
	        if (localeName != null && localeName.Length > 0) {
	            bundleName = bundleName + "_" + localeName;
	        }
	        String name = ICU_PACKAGE + "." + bundleName;
	        try {
	            if (name.IndexOf("_zh_") == -1) { // DLF temporary hack
	                Type rbclass = ILOG.J2CsMapping.Reflect.Helper.GetNativeType(name);
	                ResourceManager rb = (ResourceManager) Activator.CreateInstance(rbclass);
	                return rb;
	            }
	        } catch (TypeLoadException e) {
	            if (DEBUG) {
	                System.Console.Out.WriteLine(name + " not found");
	            }
	            // ignore, keep looking
	        } catch (Exception e_0) {
	            if (DEBUG) {
	                Console.Error.WriteLine(e_0.StackTrace);
	                System.Console.Out.WriteLine(e_0.Message);
	            }
	        }
	        if (DEBUG) {
	            System.Console.Out.WriteLine(bundleName + " not found.");
	        }
	
	        return null;
	    }
	
	    // ========== privates ==========
	
	    // Flag for enabling/disabling debugging code
	    private static readonly bool DEBUG = IBM.ICU.Impl.ICUDebug.Enabled("localedata");
	
	    // Cache for getAvailableLocales
	    // private static SoftReference GET_AVAILABLE_CACHE;
	
	    // Cache for ResourceBundle instantiation
	    private static WeakReference BUNDLE_CACHE;

        private static ResourceBundle LoadFromCache(String key)
        {
	        if (BUNDLE_CACHE != null) {
	            IDictionary m = (IDictionary) BUNDLE_CACHE.Target;
	            if (m != null) {
                    return (ResourceBundle)ILOG.J2CsMapping.Collections.Collections.Get(m, key);
	            }
	        }
	        return null;
	    }

        private static void AddToCache(String key, ResourceBundle b)
        {
	        IDictionary m = null;
	        if (BUNDLE_CACHE != null) {
	            m = (IDictionary) BUNDLE_CACHE.Target;
	        }
	        if (m == null) {
	            m = new Hashtable();
	            BUNDLE_CACHE = new WeakReference(m);
	        }
	        ILOG.J2CsMapping.Collections.Collections.Put(m,key,b);
	    }
	
	    // recursively build bundle
        private static ResourceBundle Instantiate(String name)
        {
	        ResourceBundle b = LoadFromCache(name);
	        if (b == null) {
                ResourceBundle parent = null;
	            int i = name.LastIndexOf('_');
	
	            // TODO: convert this to use ULocale
                Locale rootLocale = new Locale("", "", "");
	
	            if (i != -1) {
	                parent = Instantiate(name.Substring(0,(i)-(0)));
	            }
	            try {
                    Locale locale = rootLocale;
	                i = name.IndexOf('_');
	                if (i != -1) {
	                    locale = IBM.ICU.Impl.LocaleUtility.GetLocaleFromName(name.Substring(i + 1));
	                } else {
	                    i = name.Length;
	                }
	
	                Assembly cl = typeof(ICULocaleData).Assembly;
	                if (cl == null) {
	                    // we're on the bootstrap
	                    cl = System.Reflection.Assembly.GetEntryAssembly();
	                }
	                Type cls = cl.GetType(name);
	                if (typeof(ICUListResourceBundle).IsAssignableFrom(cls)) {
	                    ICUListResourceBundle bx = (ICUListResourceBundle) Activator.CreateInstance(cls);
	
	                    if (parent != null) {
	                        bx.SetParentX(parent);
	                    }
	                    bx.icuLocale = locale;
	                    b = bx;
	                    // System.out.println("iculistresourcebundle: " + name +
	                    // " is " + b);
	                } else {
	                    b = ResourceBundle.GetBundle(name.Substring(0,(i)-(0)), locale);
	                    // System.out.println("resourcebundle: " + name + " is " +
	                    // b);
	                }
	                AddToCache(name, b);
	            } catch (TypeLoadException e) {
	
	                int j = name.IndexOf('_');
	                int k = name.LastIndexOf('_');
	                // if a bogus locale is passed then the parent should be
	                // the default locale not the root locale!
	                if (k == j && j != -1) {
	
	                    String locName = name.Substring(j + 1,(name.Length)-(j + 1));
	                    String defaultName = IBM.ICU.Util.ULocale.GetDefault().ToString();
	
	                    if (!locName.Equals(rootLocale.ToString())
	                            && defaultName.IndexOf(locName) == -1) {
	                        String bundle = name.Substring(0,(j)-(0));
	                        parent = Instantiate(bundle + "_" + defaultName);
	                    }
	                }
	                b = parent;
	            } catch (Exception e_0) {
	                System.Console.Out.WriteLine("failure");
	                System.Console.Out.WriteLine(e_0);
	            }
	        }
	        // if(b==null){
	        // throw new
	        // MissingResourceException("Could not load data "+name,"","");
	        // }
	        return b;
	    }
	
	    /// <summary>
	    /// Still need permissions to use our own class loader, is there no way to
	    /// load class resources from new locations that aren't already on the class
	    /// path?
	    /// </summary>
	    ///
	    [MethodImpl(MethodImplOptions.Synchronized)]
        private static ResourceBundle Instantiate(String name,
	            String localeName) {
	        if (localeName.Length != 0) {
	            name = name + "_" + localeName;
	        }
            ResourceBundle b = Instantiate(name);
	        if (b == null) {
	            throw new MissingManifestResourceException("Could not load data " + name);
	        }
	        return b;
	    }
	
	    /*
	     * private static Set createLocaleNameSet(String bundleName) { try {
	     * ResourceBundle index = getResourceBundle(bundleName, "index"); Object[][]
	     * localeStrings = (Object[][]) index.getObject("InstalledLocales");
	     * String[] localeNames = new String[localeStrings.length];
	     * 
	     * // barf gag choke spit hack... // since java's Locale 'fixes' the locale
	     * string for some locales, // we have to fix our names to match, otherwise
	     * the Locale[] list // won't match the locale name set. What were they
	     * thinking?!? for (int i = 0; i < localeNames.length; ++i) { localeNames[i]
	     * =
	     * LocaleUtility.getLocaleFromName((String)localeStrings[i][0]).toString();
	     * }
	     * 
	     * HashSet set = new HashSet(); set.addAll(Arrays.asList(localeNames));
	     * return Collections.unmodifiableSet(set); } catch
	     * (MissingResourceException e) { if (DEBUG)
	     * System.out.println("couldn't find index for bundleName: " + bundleName);
	     * Thread.dumpStack(); } return Collections.EMPTY_SET; }
	     */
	
	    /*
	     * private static Locale[] createLocaleList(String bundleName) { try {
	     * ResourceBundle index = getResourceBundle(bundleName, "index"); Object[][]
	     * localeStrings = (Object[][]) index.getObject("InstalledLocales");
	     * Locale[] locales = new Locale[localeStrings.length]; for (int i = 0; i <
	     * localeStrings.length; ++i) { locales[i] =
	     * LocaleUtility.getLocaleFromName((String)localeStrings[i][0]); } return
	     * locales; } catch (MissingResourceException e) { if (DEBUG)
	     * System.out.println("couldn't find index for bundleName: " + bundleName);
	     * Thread.dumpStack(); } return new Locale[0]; }
	     */
	}
}