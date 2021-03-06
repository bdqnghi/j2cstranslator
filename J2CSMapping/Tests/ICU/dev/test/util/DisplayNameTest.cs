/*
 *******************************************************************************
 * Copyright (C) 1996-2007, International Business Machines Corporation and    *
 * others. All Rights Reserved.                                                *
 *******************************************************************************
 */
// --------------------------------------------------------------------------------------------------
// This file was automatically generated by J2CS Translator (http://j2cstranslator.sourceforge.net/). 
// Version 1.3.6.20101125_01     
// 12/13/10 4:01 PM    
// ${CustomMessageForDisclaimer}                                                                             
// --------------------------------------------------------------------------------------------------
 namespace IBM.ICU.Charset {
	
	using IBM.ICU.Impl;
	using IBM.ICU.Util;
	using ILOG.J2CsMapping.Collections;
	using ILOG.J2CsMapping.Collections.Generics;
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.IO;
	using System.Runtime.CompilerServices;
	
	public class DisplayNameTest : TestFmwk {
	    public DisplayNameTest() {
	        for (int k = 0; k < codeToName.Length; ++k)
	            codeToName[k] = new Hashtable();
	        this.codeToName = new IDictionary[10];
	        this.countries = AddUnknown(IBM.ICU.Util.ULocale.GetISOCountries(), 2);
	        this.languages = AddUnknown(IBM.ICU.Util.ULocale.GetISOLanguages(), 2);
	        this.zones = AddUnknown(GetRealZoneIDs(), 5);
	        this.scripts = AddUnknown(
	                GetCodes(new ULocale("en", "US", ""), "Scripts"), 4);
	        this.currencies = AddUnknown(
	                GetCodes(new ULocale("en", "", ""), "Currencies"), 3);
	        this.zoneData = new Hashtable();
	        this.bogusZones = null;
	    }
	
	    internal const bool SHOW_ALL = false;
	
	    public static void Main(String[] args) {
	        new DisplayNameTest().Run(args);
	    }
	
	    public sealed class Anonymous_C4 : DisplayNameTest.DisplayNameGetter  {
	        public String Get(ULocale loc, String code, Object context) {
	            return IBM.ICU.Util.ULocale.GetDisplayLanguage(code, loc);
	        }
	    }
	
	    public sealed class Anonymous_C3 : DisplayNameTest.DisplayNameGetter  {
	        public String Get(ULocale loc, String code, Object context) {
	            // TODO This is kinda a hack; ought to be direct way.
	            return IBM.ICU.Util.ULocale.GetDisplayScript("en_" + code, loc);
	        }
	    }
	
	    public sealed class Anonymous_C2 : DisplayNameTest.DisplayNameGetter  {
	        public String Get(ULocale loc, String code, Object context) {
	            // TODO This is kinda a hack; ought to be direct way.
	            return IBM.ICU.Util.ULocale.GetDisplayCountry("en_" + code, loc);
	        }
	    }
	
	    public sealed class Anonymous_C1 : DisplayNameTest.DisplayNameGetter  {
	        public String Get(ULocale loc, String code, Object context) {
	            Currency s = IBM.ICU.Util.Currency.GetInstance(code);
	            return s.GetName(loc, ((Int32) context),
	                    new bool[1]);
	        }
	    }
	
	    public sealed class Anonymous_C0 : DisplayNameTest.DisplayNameGetter {
	            private readonly DisplayNameTest outer_DisplayNameTest;
	    
	            
	            /// <param name="paramouter_DisplayNameTest"></param>
	            public Anonymous_C0(DisplayNameTest paramouter_DisplayNameTest) {
	                this.outer_DisplayNameTest = paramouter_DisplayNameTest;
	            }
	    
	            // TODO replace once we have real API
	            public String Get(ULocale loc, String code, Object context) {
	                return outer_DisplayNameTest.GetZoneString(loc, code, ((Int32) context));
	            }
	        }
	
	    interface DisplayNameGetter {
	        String Get(ULocale locale, String code, Object context);
	    }
	
	    internal IDictionary[] codeToName;
	    static internal readonly Object[] zoneFormats = { ((int)(0)), ((int)(1)),
	            ((int)(2)), ((int)(3)), ((int)(4)), ((int)(5)),
	            ((int)(6)), ((int)(7)) };
	
	    static internal readonly Object[] currencyFormats = {
	            ((int)(IBM.ICU.Util.Currency.SYMBOL_NAME)), ((int)(IBM.ICU.Util.Currency.LONG_NAME)) };
	
	    static internal readonly Object[] NO_CONTEXT = { null };
	
	    static internal readonly DateTime JAN1 = new DateTime(2004 - 1900, 0, 1);
	
	    static internal readonly DateTime JULY1 = new DateTime(2004 - 1900, 6, 1);
	
	    internal String[] countries;
	
	    internal String[] languages;
	
	    internal String[] zones;
	
	    internal String[] scripts;
	
	    // TODO fix once there is a way to get a list of all script codes
	    internal String[] currencies;
	
	    // TODO fix once there is a way to get a list of all currency codes
	
	    public void TestLocales() {
	        ULocale[] locales = IBM.ICU.Util.ULocale.GetAvailableLocales();
	        for (int i = 0; i < locales.Length; ++i) {
	            CheckLocale(locales[i]);
	        }
	    }
	
	    
	    /// <returns></returns>
	    private String[] GetRealZoneIDs() {
	        ILOG.J2CsMapping.Collections.ISet temp = new SortedSet(ILOG.J2CsMapping.Collections.Arrays.AsList(IBM.ICU.Util.TimeZone.GetAvailableIDs()));
	        temp.RemoveAll(new ILOG.J2CsMapping.Collections.ListSet(GetAliasMap().Keys));
	        return (String[]) ILOG.J2CsMapping.Collections.Generics.Collections.ToArray(temp,new String[temp.Count]);
	    }
	
	    public void TestEnglish() {
	        CheckLocale(IBM.ICU.Util.ULocale.ENGLISH);
	    }
	
	    public void TestFrench() {
	        CheckLocale(IBM.ICU.Util.ULocale.FRENCH);
	    }
	
	    private void CheckLocale(ULocale locale) {
	        Logln("Checking " + locale);
	        Check("Language", locale, languages, null, new DisplayNameTest.Anonymous_C4 ());
	        Check("Script", locale, scripts, null, new DisplayNameTest.Anonymous_C3 ());
	        Check("Country", locale, countries, null, new DisplayNameTest.Anonymous_C2 ());
	        Check("Currencies", locale, currencies, currencyFormats,
	                new DisplayNameTest.Anonymous_C1 ());
	        // comment this out, because the zone string information is lost
	        // we'd have to access the resources directly to test them
	
	        Check("Zones", locale, zones, zoneFormats, new DisplayNameTest.Anonymous_C0 (this));
	
	    }
	
	    internal IDictionary zoneData;
	
	    internal String GetZoneString(ULocale locale, String olsonID, int item) {
	        IDictionary data = (IDictionary) ILOG.J2CsMapping.Collections.Collections.Get(zoneData,locale);
	        if (data == null) {
	            data = new Hashtable();
	            if (SHOW_ALL)
	                System.Console.Out.WriteLine();
	            if (SHOW_ALL)
	                System.Console.Out.WriteLine("zones for " + locale);
	            ICUResourceBundle bundle = (ICUResourceBundle) IBM.ICU.Util.UResourceBundle
	                    .GetBundleInstance(locale);
	            ICUResourceBundle table = bundle.GetWithFallback("zoneStrings");
	            for (int i = 0; i < table.GetSize(); ++i) {
	                UResourceBundle stringSet = table.Get(i);
	                // ICUResourceBundle stringSet =
	                // table.getWithFallback(String.valueOf(i));
	                String key = stringSet.GetString(0);
	                if (SHOW_ALL)
	                    System.Console.Out.WriteLine("key: " + key);
	                ArrayList list = new ArrayList();
	                for (int j = 1; j < stringSet.GetSize(); ++j) {
	                    String entry = stringSet.GetString(j);
	                    if (SHOW_ALL)
	                        System.Console.Out.WriteLine("  entry: " + entry);
	                    ILOG.J2CsMapping.Collections.Generics.Collections.Add(list,entry);
	                }
	                ILOG.J2CsMapping.Collections.Collections.Put(data,key,ILOG.J2CsMapping.Collections.Generics.Collections.ToArray(list,new String[list.Count]));
	            }
	            ILOG.J2CsMapping.Collections.Collections.Put(zoneData,locale,data);
	        }
	        String[] strings = (String[]) ILOG.J2CsMapping.Collections.Collections.Get(data,olsonID);
	        if (strings == null || item >= strings.Length)
	            return olsonID;
	        return strings[item];
	    }
	
	    static internal String[][] zonesAliases = {
	            new String[] { "America/Atka", "America/Atka" },
	            new String[] { "America/Ensenada", "America/Ensenada" },
	            new String[] { "America/Fort_Wayne", "America/Fort_Wayne" },
	            new String[] { "America/Indiana/Indianapolis",
	                    "America/Indiana/Indianapolis" },
	            new String[] { "America/Kentucky/Louisville",
	                    "America/Kentucky/Louisville" },
	            new String[] { "America/Knox_IN", "America/Knox_IN" },
	            new String[] { "America/Porto_Acre", "America/Porto_Acre" },
	            new String[] { "America/Rosario", "America/Rosario" },
	            new String[] { "America/Shiprock", "America/Shiprock" },
	            new String[] { "America/Virgin", "America/Virgin" },
	            new String[] { "Antarctica/South_Pole", "Antarctica/South_Pole" },
	            new String[] { "Arctic/Longyearbyen", "Arctic/Longyearbyen" },
	            new String[] { "Asia/Ashkhabad", "Asia/Ashkhabad" },
	            new String[] { "Asia/Chungking", "Asia/Chungking" },
	            new String[] { "Asia/Dacca", "Asia/Dacca" },
	            new String[] { "Asia/Istanbul", "Asia/Istanbul" },
	            new String[] { "Asia/Macao", "Asia/Macao" },
	            new String[] { "Asia/Tel_Aviv", "Asia/Tel_Aviv" },
	            new String[] { "Asia/Thimbu", "Asia/Thimbu" },
	            new String[] { "Asia/Ujung_Pandang", "Asia/Ujung_Pandang" },
	            new String[] { "Asia/Ulan_Bator", "Asia/Ulan_Bator" },
	            new String[] { "Australia/ACT", "Australia/ACT" },
	            new String[] { "Australia/Canberra", "Australia/Canberra" },
	            new String[] { "Australia/LHI", "Australia/LHI" },
	            new String[] { "Australia/NSW", "Australia/NSW" },
	            new String[] { "Australia/North", "Australia/North" },
	            new String[] { "Australia/Queensland", "Australia/Queensland" },
	            new String[] { "Australia/South", "Australia/South" },
	            new String[] { "Australia/Tasmania", "Australia/Tasmania" },
	            new String[] { "Australia/Victoria", "Australia/Victoria" },
	            new String[] { "Australia/West", "Australia/West" },
	            new String[] { "Australia/Yancowinna", "Australia/Yancowinna" },
	            new String[] { "Brazil/Acre", "Brazil/Acre" },
	            new String[] { "Brazil/DeNoronha", "Brazil/DeNoronha" },
	            new String[] { "Brazil/East", "Brazil/East" },
	            new String[] { "Brazil/West", "Brazil/West" },
	            new String[] { "CST6CDT", "CST6CDT" },
	            new String[] { "Canada/Atlantic", "Canada/Atlantic" },
	            new String[] { "Canada/Central", "Canada/Central" },
	            new String[] { "Canada/East-Saskatchewan",
	                    "Canada/East-Saskatchewan" },
	            new String[] { "Canada/Eastern", "Canada/Eastern" },
	            new String[] { "Canada/Mountain", "Canada/Mountain" },
	            new String[] { "Canada/Newfoundland", "Canada/Newfoundland" },
	            new String[] { "Canada/Pacific", "Canada/Pacific" },
	            new String[] { "Canada/Saskatchewan", "Canada/Saskatchewan" },
	            new String[] { "Canada/Yukon", "Canada/Yukon" },
	            new String[] { "Chile/Continental", "Chile/Continental" },
	            new String[] { "Chile/EasterIsland", "Chile/EasterIsland" },
	            new String[] { "Cuba", "Cuba" }, new String[] { "EST", "EST" },
	            new String[] { "EST5EDT", "EST5EDT" },
	            new String[] { "Egypt", "Egypt" }, new String[] { "Eire", "Eire" },
	            new String[] { "Etc/GMT+0", "Etc/GMT+0" },
	            new String[] { "Etc/GMT-0", "Etc/GMT-0" },
	            new String[] { "Etc/GMT0", "Etc/GMT0" },
	            new String[] { "Etc/Greenwich", "Etc/Greenwich" },
	            new String[] { "Etc/Universal", "Etc/Universal" },
	            new String[] { "Etc/Zulu", "Etc/Zulu" },
	            new String[] { "Europe/Nicosia", "Europe/Nicosia" },
	            new String[] { "Europe/Tiraspol", "Europe/Tiraspol" },
	            new String[] { "GB", "GB" }, new String[] { "GB-Eire", "GB-Eire" },
	            new String[] { "GMT", "GMT" }, new String[] { "GMT+0", "GMT+0" },
	            new String[] { "GMT-0", "GMT-0" }, new String[] { "GMT0", "GMT0" },
	            new String[] { "Greenwich", "Greenwich" },
	            new String[] { "HST", "HST" },
	            new String[] { "Hongkong", "Hongkong" },
	            new String[] { "Iceland", "Iceland" },
	            new String[] { "Iran", "Iran" },
	            new String[] { "Israel", "Israel" },
	            new String[] { "Jamaica", "Jamaica" },
	            new String[] { "Japan", "Japan" },
	            new String[] { "Kwajalein", "Kwajalein" },
	            new String[] { "Libya", "Libya" }, new String[] { "MST", "MST" },
	            new String[] { "MST7MDT", "MST7MDT" },
	            new String[] { "Mexico/BajaNorte", "Mexico/BajaNorte" },
	            new String[] { "Mexico/BajaSur", "Mexico/BajaSur" },
	            new String[] { "Mexico/General", "Mexico/General" },
	            new String[] { "Mideast/Riyadh87", "Mideast/Riyadh87" },
	            new String[] { "Mideast/Riyadh88", "Mideast/Riyadh88" },
	            new String[] { "Mideast/Riyadh89", "Mideast/Riyadh89" },
	            new String[] { "NZ", "NZ" }, new String[] { "NZ-CHAT", "NZ-CHAT" },
	            new String[] { "Navajo", "Navajo" }, new String[] { "PRC", "PRC" },
	            new String[] { "PST8PDT", "PST8PDT" },
	            new String[] { "Pacific/Samoa", "Pacific/Samoa" },
	            new String[] { "Poland", "Poland" },
	            new String[] { "Portugal", "Portugal" },
	            new String[] { "ROC", "ROC" }, new String[] { "ROK", "ROK" },
	            new String[] { "Singapore", "Singapore" },
	            new String[] { "SystemV/AST4", "SystemV/AST4" },
	            new String[] { "SystemV/AST4ADT", "SystemV/AST4ADT" },
	            new String[] { "SystemV/CST6", "SystemV/CST6" },
	            new String[] { "SystemV/CST6CDT", "SystemV/CST6CDT" },
	            new String[] { "SystemV/EST5", "SystemV/EST5" },
	            new String[] { "SystemV/EST5EDT", "SystemV/EST5EDT" },
	            new String[] { "SystemV/HST10", "SystemV/HST10" },
	            new String[] { "SystemV/MST7", "SystemV/MST7" },
	            new String[] { "SystemV/MST7MDT", "SystemV/MST7MDT" },
	            new String[] { "SystemV/PST8", "SystemV/PST8" },
	            new String[] { "SystemV/PST8PDT", "SystemV/PST8PDT" },
	            new String[] { "SystemV/YST9", "SystemV/YST9" },
	            new String[] { "SystemV/YST9YDT", "SystemV/YST9YDT" },
	            new String[] { "Turkey", "Turkey" }, new String[] { "UCT", "UCT" },
	            new String[] { "US/Alaska", "US/Alaska" },
	            new String[] { "US/Aleutian", "US/Aleutian" },
	            new String[] { "US/Arizona", "US/Arizona" },
	            new String[] { "US/Central", "US/Central" },
	            new String[] { "US/East-Indiana", "US/East-Indiana" },
	            new String[] { "US/Eastern", "US/Eastern" },
	            new String[] { "US/Hawaii", "US/Hawaii" },
	            new String[] { "US/Indiana-Starke", "US/Indiana-Starke" },
	            new String[] { "US/Michigan", "US/Michigan" },
	            new String[] { "US/Mountain", "US/Mountain" },
	            new String[] { "US/Pacific", "US/Pacific" },
	            new String[] { "US/Pacific-New", "US/Pacific-New" },
	            new String[] { "US/Samoa", "US/Samoa" },
	            new String[] { "UTC", "UTC" },
	            new String[] { "Universal", "Universal" },
	            new String[] { "W-SU", "W-SU" }, new String[] { "Zulu", "Zulu" },
	            new String[] { "ACT", "ACT" }, new String[] { "AET", "AET" },
	            new String[] { "AGT", "AGT" }, new String[] { "ART", "ART" },
	            new String[] { "AST", "AST" }, new String[] { "BET", "BET" },
	            new String[] { "BST", "BST" }, new String[] { "CAT", "CAT" },
	            new String[] { "CNT", "CNT" }, new String[] { "CST", "CST" },
	            new String[] { "CTT", "CTT" }, new String[] { "EAT", "EAT" },
	            new String[] { "ECT", "ECT" }, new String[] { "IET", "IET" },
	            new String[] { "IST", "IST" }, new String[] { "JST", "JST" },
	            new String[] { "MIT", "MIT" }, new String[] { "NET", "NET" },
	            new String[] { "NST", "NST" }, new String[] { "PLT", "PLT" },
	            new String[] { "PNT", "PNT" }, new String[] { "PRT", "PRT" },
	            new String[] { "PST", "PST" }, new String[] { "SST", "SST" },
	            new String[] { "VST", "VST" } };
	
	    /// <summary>
	    /// Hack to get code list
	    /// </summary>
	    ///
	    /// <returns></returns>
	    private static String[] GetCodes(ULocale locale, String tableName) {
	        // TODO remove Ugly Hack
	        // get stuff
	        ICUResourceBundle bundle = (ICUResourceBundle) IBM.ICU.Util.UResourceBundle
	                .GetBundleInstance(locale);
	        ICUResourceBundle table = bundle.GetWithFallback(tableName);
	        // copy into array
	        ArrayList stuff = new ArrayList();
	        for (IIterator keys = table.GetKeys(); keys.HasNext();) {
	            ILOG.J2CsMapping.Collections.Generics.Collections.Add(stuff,keys.Next());
	        }
	        String[] result = new String[stuff.Count];
	        return (String[]) ILOG.J2CsMapping.Collections.Generics.Collections.ToArray(stuff,result);
	        // return new String[] {"Latn", "Cyrl"};
	    }
	
	    /// <summary>
	    /// Add two unknown strings, just to make sure they get passed through
	    /// without colliding
	    /// </summary>
	    ///
	    /// <param name="strings"></param>
	    /// <returns></returns>
	    private String[] AddUnknown(String[] strings, int len) {
	        String[] result = new String[strings.Length + 2];
	        result[0] = "x1unknown".Substring(0,(len)-(0));
	        result[1] = "y1nknown".Substring(0,(len)-(0));
	        System.Array.Copy((Array)(strings),0,(Array)(result),2,strings.Length);
	        return result;
	    }
	
	    internal IDictionary bogusZones;
	
	    private IDictionary GetAliasMap() {
	        if (bogusZones == null) {
	            bogusZones = new SortedList();
	            for (int i = 0; i < zonesAliases.Length; ++i) {
	                ILOG.J2CsMapping.Collections.Collections.Put(bogusZones,zonesAliases[i][0],zonesAliases[i][1]);
	            }
	        }
	        return bogusZones;
	    }
	
	    private void Check(String type, ULocale locale, String[] codes,
	            Object[] contextList, DisplayNameTest.DisplayNameGetter  getter) {
	        if (contextList == null)
	            contextList = NO_CONTEXT;
	        for (int k = 0; k < contextList.Length; ++k)
	            codeToName[k].Clear();
	        for (int j = 0; j < codes.Length; ++j) {
	            String code = codes[j];
	            for (int k_0 = 0; k_0 < contextList.Length; ++k_0) {
	                Object context = contextList[k_0];
	                String name = getter.Get(locale, code, context);
	                if (name == null || name.Length == 0) {
	                    Errln("Null or Zero-Length Display Name\t" + type + "\t("
	                            + ((context != null) ? (Object) (context) : (Object) ("")) + ")" + ":\t"
	                            + locale + " ["
	                            + locale.GetDisplayName(IBM.ICU.Util.ULocale.ENGLISH) + "]"
	                            + "\t" + code + " ["
	                            + getter.Get(IBM.ICU.Util.ULocale.ENGLISH, code, context) + "]");
	                    continue;
	                }
	                String otherCode = (String) ILOG.J2CsMapping.Collections.Collections.Get(codeToName[k_0],name);
	                if (otherCode != null) {
	                    Errln("Display Names collide for\t" + type + "\t("
	                            + ((context != null) ? (Object) (context) : (Object) ("")) + ")" + ":\t"
	                            + locale + " ["
	                            + locale.GetDisplayName(IBM.ICU.Util.ULocale.ENGLISH) + "]"
	                            + "\t" + code + " ["
	                            + getter.Get(IBM.ICU.Util.ULocale.ENGLISH, code, context) + "]"
	                            + "\t& " + otherCode + " ["
	                            + getter.Get(IBM.ICU.Util.ULocale.ENGLISH, otherCode, context)
	                            + "]" + "\t=> " + name);
	                } else {
	                    ILOG.J2CsMapping.Collections.Collections.Put(codeToName[k_0],name,code);
	                    if (SHOW_ALL)
	                        Logln(type + " (" + ((context != null) ? (Object) (context) : (Object) (""))
	                                + ")" + "\t" + locale + " ["
	                                + locale.GetDisplayName(IBM.ICU.Util.ULocale.ENGLISH) + "]"
	                                + "\t" + code + "["
	                                + getter.Get(IBM.ICU.Util.ULocale.ENGLISH, code, context)
	                                + "]" + "\t=> " + name);
	                }
	            }
	        }
	    }
	}
}
