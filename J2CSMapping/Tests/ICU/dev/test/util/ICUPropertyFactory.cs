//##header J2SE15
// --------------------------------------------------------------------------------------------------
// This file was automatically generated by J2CS Translator (http://j2cstranslator.sourceforge.net/). 
// Version 1.3.6.20101125_01     
// 12/13/10 4:01 PM    
// ${CustomMessageForDisclaimer}                                                                             
// --------------------------------------------------------------------------------------------------
 //#if defined(FOUNDATION10) || defined(J2SE13)
//#else
/*
 *******************************************************************************
 * Copyright (C) 2002-2007, International Business Machines Corporation and    *
 * others. All Rights Reserved.                                                *
 *******************************************************************************
 */
namespace IBM.ICU.Charset
{

    using ILOG.J2CsMapping.Collections;
    using ILOG.J2CsMapping.Collections.Generics;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using System.Runtime.CompilerServices;
    using ILOG.J2CsMapping.Text;
    using System.Globalization;
    using ILOG.J2CsMapping.Util;

    /// <summary>
    /// Provides a general interface for Unicode Properties, and extracting sets
    /// based on those values.
    /// </summary>
    ///

    public class ICUPropertyFactory : UnicodeProperty.Factory
    {

        internal class ICUProperty : UnicodeProperty
        {
            protected internal int propEnum;

            protected internal ICUProperty(String propName, int propEnum_0)
            {
                this.propEnum = Int32.MinValue;
                this.shownException = false;
                this.cccHack = new Hashtable();
                this.needCccHack = true;
                SetName(propName);
                this.propEnum = propEnum_0;
                SetType(InternalGetPropertyType(propEnum_0));
            }

            internal bool shownException;

            protected internal override String _getValue(int codePoint)
            {
                switch (propEnum)
                {
                    case IBM.ICU.Lang.UProperty_Constants.AGE:
                        String temp = IBM.ICU.Lang.UCharacter.GetAge(codePoint).ToString();
                        if (temp.Equals("0.0.0.0"))
                            return "unassigned";
                        if (temp.EndsWith(".0.0"))
                            return temp.Substring(0, (temp.Length - 4) - (0));
                        return temp;
                    case IBM.ICU.Lang.UProperty_Constants.BIDI_MIRRORING_GLYPH:
                        return IBM.ICU.Text.UTF16.ValueOf(IBM.ICU.Lang.UCharacter.GetMirror(codePoint));
                    case IBM.ICU.Lang.UProperty_Constants.CASE_FOLDING:
                        return IBM.ICU.Lang.UCharacter.FoldCase(IBM.ICU.Text.UTF16.ValueOf(codePoint), true);
                    case IBM.ICU.Lang.UProperty_Constants.ISO_COMMENT:
                        return IBM.ICU.Lang.UCharacter.GetISOComment(codePoint);
                    case IBM.ICU.Lang.UProperty_Constants.LOWERCASE_MAPPING:
                        return IBM.ICU.Lang.UCharacter.ToLowerCase(Locale.GetDefault(),
                                IBM.ICU.Text.UTF16.ValueOf(codePoint));
                    case IBM.ICU.Lang.UProperty_Constants.NAME:
                        return IBM.ICU.Lang.UCharacter.GetName(codePoint);
                    case IBM.ICU.Lang.UProperty_Constants.SIMPLE_CASE_FOLDING:
                        return IBM.ICU.Text.UTF16.ValueOf(IBM.ICU.Lang.UCharacter.FoldCase(codePoint, true));
                    case IBM.ICU.Lang.UProperty_Constants.SIMPLE_LOWERCASE_MAPPING:
                        return IBM.ICU.Text.UTF16.ValueOf(IBM.ICU.Lang.UCharacter.ToLowerCase(codePoint));
                    case IBM.ICU.Lang.UProperty_Constants.SIMPLE_TITLECASE_MAPPING:
                        return IBM.ICU.Text.UTF16.ValueOf(IBM.ICU.Lang.UCharacter.ToTitleCase(codePoint));
                    case IBM.ICU.Lang.UProperty_Constants.SIMPLE_UPPERCASE_MAPPING:
                        return IBM.ICU.Text.UTF16.ValueOf(IBM.ICU.Lang.UCharacter.ToUpperCase(codePoint));
                    case IBM.ICU.Lang.UProperty_Constants.TITLECASE_MAPPING:
                        return IBM.ICU.Lang.UCharacter.ToTitleCase(Locale.GetDefault(),
                                IBM.ICU.Text.UTF16.ValueOf(codePoint), null);
                    case IBM.ICU.Lang.UProperty_Constants.UNICODE_1_NAME:
                        return IBM.ICU.Lang.UCharacter.GetName1_0(codePoint);
                    case IBM.ICU.Lang.UProperty_Constants.UPPERCASE_MAPPING:
                        return IBM.ICU.Lang.UCharacter.ToUpperCase(Locale.GetDefault(),
                                IBM.ICU.Text.UTF16.ValueOf(codePoint));
                    case IBM.ICU.Charset.ICUPropertyFactory.NFC:
                        return IBM.ICU.Text.Normalizer.Normalize(codePoint, IBM.ICU.Text.Normalizer.NFC);
                    case IBM.ICU.Charset.ICUPropertyFactory.NFD:
                        return IBM.ICU.Text.Normalizer.Normalize(codePoint, IBM.ICU.Text.Normalizer.NFD);
                    case IBM.ICU.Charset.ICUPropertyFactory.NFKC:
                        return IBM.ICU.Text.Normalizer.Normalize(codePoint, IBM.ICU.Text.Normalizer.NFKC);
                    case IBM.ICU.Charset.ICUPropertyFactory.NFKD:
                        return IBM.ICU.Text.Normalizer.Normalize(codePoint, IBM.ICU.Text.Normalizer.NFKD);
                    case IBM.ICU.Charset.ICUPropertyFactory.isNFC:
                        return IBM.ICU.Text.Normalizer.Normalize(codePoint,
                                                IBM.ICU.Text.Normalizer.NFC).Equals(IBM.ICU.Text.UTF16.ValueOf(codePoint)).ToString();
                    case IBM.ICU.Charset.ICUPropertyFactory.isNFD:
                        return IBM.ICU.Text.Normalizer.Normalize(codePoint,
                                                IBM.ICU.Text.Normalizer.NFD).Equals(IBM.ICU.Text.UTF16.ValueOf(codePoint)).ToString();
                    case IBM.ICU.Charset.ICUPropertyFactory.isNFKC:
                        return IBM.ICU.Text.Normalizer.Normalize(codePoint,
                                                IBM.ICU.Text.Normalizer.NFKC).Equals(IBM.ICU.Text.UTF16.ValueOf(codePoint)).ToString();
                    case IBM.ICU.Charset.ICUPropertyFactory.isNFKD:
                        return IBM.ICU.Text.Normalizer.Normalize(codePoint,
                                                IBM.ICU.Text.Normalizer.NFKD).Equals(IBM.ICU.Text.UTF16.ValueOf(codePoint)).ToString();
                    case IBM.ICU.Charset.ICUPropertyFactory.isLowercase:
                        return IBM.ICU.Lang.UCharacter.ToLowerCase(Locale.GetDefault(),
                                                IBM.ICU.Text.UTF16.ValueOf(codePoint)).Equals(
                                                IBM.ICU.Text.UTF16.ValueOf(codePoint)).ToString();
                    case IBM.ICU.Charset.ICUPropertyFactory.isUppercase:
                        return IBM.ICU.Lang.UCharacter.ToUpperCase(Locale.GetDefault(),
                                                IBM.ICU.Text.UTF16.ValueOf(codePoint)).Equals(
                                                IBM.ICU.Text.UTF16.ValueOf(codePoint)).ToString();
                    case IBM.ICU.Charset.ICUPropertyFactory.isTitlecase:
                        return IBM.ICU.Lang.UCharacter.ToTitleCase(Locale.GetDefault(),
                                                IBM.ICU.Text.UTF16.ValueOf(codePoint), null).Equals(
                                                IBM.ICU.Text.UTF16.ValueOf(codePoint)).ToString();
                    case IBM.ICU.Charset.ICUPropertyFactory.isCasefolded:
                        return IBM.ICU.Lang.UCharacter.FoldCase(
                                                IBM.ICU.Text.UTF16.ValueOf(codePoint), true).Equals(
                                                IBM.ICU.Text.UTF16.ValueOf(codePoint)).ToString();
                    case IBM.ICU.Charset.ICUPropertyFactory.isCased:
                        return IBM.ICU.Lang.UCharacter.ToLowerCase(Locale.GetDefault(),
                                                IBM.ICU.Text.UTF16.ValueOf(codePoint)).Equals(
                                                IBM.ICU.Text.UTF16.ValueOf(codePoint)).ToString();
                }
                if (propEnum < IBM.ICU.Lang.UProperty_Constants.INT_LIMIT)
                {
                    int enumValue = -1;
                    String value_ren = null;
                    try
                    {
                        enumValue = IBM.ICU.Lang.UCharacter.GetIntPropertyValue(codePoint,
                                propEnum);
                        if (enumValue >= 0)
                            value_ren = FixedGetPropertyValueName(propEnum, enumValue,
                                    IBM.ICU.Lang.UProperty_Constants.NameChoice.LONG);
                    }
                    catch (ArgumentException e)
                    {
                        if (!shownException)
                        {
                            System.Console.Out.WriteLine("Fail: " + GetName() + ", "
                                    + ILOG.J2CsMapping.Util.IlNumber.ToString(codePoint, 16));
                            shownException = true;
                        }
                    }
                    return (value_ren != null) ? value_ren : enumValue.ToString();
                }
                else if (propEnum < IBM.ICU.Lang.UProperty_Constants.DOUBLE_LIMIT)
                {
                    double num = IBM.ICU.Lang.UCharacter.GetUnicodeNumericValue(codePoint);
                    if (num == IBM.ICU.Lang.UCharacter.NO_NUMERIC_VALUE)
                        return null;
                    return String.Concat(num);
                    // TODO: Fix HACK -- API deficient
                }
                return null;
            }


            /// <param name="valueAlias">null if unused.</param>
            /// <param name="valueEnum">-1 if unused</param>
            /// <param name="nameChoice"></param>
            /// <returns></returns>
            public String GetFixedValueAlias(String valueAlias, int valueEnum,
                    int nameChoice)
            {
                if (propEnum >= IBM.ICU.Lang.UProperty_Constants.STRING_START)
                {
                    if (nameChoice != IBM.ICU.Lang.UProperty_Constants.NameChoice.LONG)
                        return null;
                    return "<string>";
                }
                else if (propEnum >= IBM.ICU.Lang.UProperty_Constants.DOUBLE_START)
                {
                    if (nameChoice != IBM.ICU.Lang.UProperty_Constants.NameChoice.LONG)
                        return null;
                    return "<number>";
                }
                if (valueAlias != null && !valueAlias.Equals("<integer>"))
                {
                    valueEnum = FixedGetPropertyValueEnum(propEnum, valueAlias);
                }
                // because these are defined badly, there may be no normal (long)
                // name.
                // if there is
                String result = FixedGetPropertyValueName(propEnum, valueEnum,
                        nameChoice);
                if (result != null)
                    return result;
                // HACK try other namechoice
                if (nameChoice == IBM.ICU.Lang.UProperty_Constants.NameChoice.LONG)
                {
                    result = FixedGetPropertyValueName(propEnum, valueEnum,
                            IBM.ICU.Lang.UProperty_Constants.NameChoice.SHORT);
                    if (result != null)
                        return result;
                    if (propEnum == IBM.ICU.Lang.UProperty_Constants.CANONICAL_COMBINING_CLASS)
                        return null;
                    return "<integer>";
                }
                return null;
            }

            public static int FixedGetPropertyValueEnum(int propEnum_0,
                    String valueAlias)
            {
                try
                {
                    return IBM.ICU.Lang.UCharacter.GetPropertyValueEnum(propEnum_0, valueAlias);
                }
                catch (Exception e)
                {
                    return Int32.Parse(valueAlias);
                }
            }

            static internal IDictionary fixSkeleton = new Hashtable();

            public static String FixedGetPropertyValueName(int propEnum_0,
                    int valueEnum, int nameChoice)
            {

                try
                {
                    String value_ren = IBM.ICU.Lang.UCharacter.GetPropertyValueName(propEnum_0,
                            valueEnum, nameChoice);
                    String newValue = (String)ILOG.J2CsMapping.Collections.Collections.Get(fixSkeleton, value_ren);
                    if (newValue == null)
                    {
                        newValue = value_ren;
                        if (propEnum_0 == IBM.ICU.Lang.UProperty_Constants.JOINING_GROUP)
                        {
                            throw new NotImplementedException();
                            // newValue = newValue.ToLower(Locale.ENGLISH);
                        }
                        newValue = IBM.ICU.Charset.UnicodeProperty.Regularize(newValue, true);
                        ILOG.J2CsMapping.Collections.Collections.Put(fixSkeleton, value_ren, newValue);
                    }
                    return newValue;
                }
                catch (Exception e)
                {
                    return null;
                }
            }

            protected internal override IList _getNameAliases(IList result)
            {
                if (result == null)
                    result = new ArrayList();
                String alias = IBM.ICU.Charset.ICUPropertyFactory.String_Extras.Get(propEnum);
                if (alias == null)
                    alias = IBM.ICU.Charset.ICUPropertyFactory.Binary_Extras.Get(propEnum);
                if (alias != null)
                {
                    IBM.ICU.Charset.UnicodeProperty.AddUnique(alias, result);
                }
                else
                {
                    IBM.ICU.Charset.UnicodeProperty.AddUnique(
                            GetFixedPropertyName(propEnum,
                                    IBM.ICU.Lang.UProperty_Constants.NameChoice.SHORT), result);
                    IBM.ICU.Charset.UnicodeProperty.AddUnique(
                            GetFixedPropertyName(propEnum,
                                    IBM.ICU.Lang.UProperty_Constants.NameChoice.LONG), result);
                }
                return result;
            }

            public String GetFixedPropertyName(int propName, int nameChoice)
            {
                try
                {
                    return IBM.ICU.Lang.UCharacter.GetPropertyName(propEnum, nameChoice);
                }
                catch (ArgumentException e)
                {
                    return null;
                }
            }

            private IDictionary cccHack;

            internal bool needCccHack;

            protected internal override IList _getAvailableValues(IList result)
            {
                if (result == null)
                    result = new ArrayList();
                if (propEnum == IBM.ICU.Lang.UProperty_Constants.AGE)
                {
                    IBM.ICU.Charset.UnicodeProperty.AddAllUnique(new String[] { "unassigned", "1.1", "2.0", "2.1",
	                        "3.0", "3.1", "3.2", "4.0" }, result);
                    return result;
                }
                if (propEnum < IBM.ICU.Lang.UProperty_Constants.INT_LIMIT)
                {
                    if (IBM.ICU.Charset.ICUPropertyFactory.Binary_Extras.IsInRange(propEnum))
                    {
                        propEnum = IBM.ICU.Lang.UProperty_Constants.BINARY_START; // HACK
                    }
                    int start = IBM.ICU.Lang.UCharacter.GetIntPropertyMinValue(propEnum);
                    int end = IBM.ICU.Lang.UCharacter.GetIntPropertyMaxValue(propEnum);
                    for (int i = start; i <= end; ++i)
                    {
                        String alias = GetFixedValueAlias(null, i,
                                IBM.ICU.Lang.UProperty_Constants.NameChoice.LONG);
                        String alias2 = GetFixedValueAlias(null, i,
                                IBM.ICU.Lang.UProperty_Constants.NameChoice.SHORT);
                        if (alias == null)
                        {
                            alias = alias2;
                            if (alias == null
                                    && propEnum == IBM.ICU.Lang.UProperty_Constants.CANONICAL_COMBINING_CLASS)
                            {
                                alias = i.ToString();
                            }
                        }
                        if (needCccHack
                                && propEnum == IBM.ICU.Lang.UProperty_Constants.CANONICAL_COMBINING_CLASS)
                        { // HACK
                            ILOG.J2CsMapping.Collections.Collections.Put(cccHack, alias, i.ToString());
                        }
                        // System.out.println(propertyAlias + "\t" + i + ":\t" +
                        // alias);
                        IBM.ICU.Charset.UnicodeProperty.AddUnique(alias, result);
                    }
                    needCccHack = false;
                }
                else
                {
                    String alias_0 = GetFixedValueAlias(null, -1,
                            IBM.ICU.Lang.UProperty_Constants.NameChoice.LONG);
                    IBM.ICU.Charset.UnicodeProperty.AddUnique(alias_0, result);
                }
                return result;
            }

            protected internal override IList _getValueAliases(String valueAlias, IList result)
            {
                if (result == null)
                    result = new ArrayList();
                if (propEnum == IBM.ICU.Lang.UProperty_Constants.AGE)
                {
                    IBM.ICU.Charset.UnicodeProperty.AddUnique(valueAlias, result);
                    return result;
                }
                if (propEnum == IBM.ICU.Lang.UProperty_Constants.CANONICAL_COMBINING_CLASS)
                {
                    IBM.ICU.Charset.UnicodeProperty.AddUnique(ILOG.J2CsMapping.Collections.Collections.Get(cccHack, valueAlias), result); // add number
                }
                IBM.ICU.Charset.UnicodeProperty.AddUnique(
                        GetFixedValueAlias(valueAlias, -1,
                                IBM.ICU.Lang.UProperty_Constants.NameChoice.SHORT), result);
                IBM.ICU.Charset.UnicodeProperty.AddUnique(
                        GetFixedValueAlias(valueAlias, -1,
                                IBM.ICU.Lang.UProperty_Constants.NameChoice.LONG), result);
                return result;
            }

            /*
             * (non-Javadoc)
             * 
             * @see
             * com.ibm.icu.dev.test.util.UnicodePropertySource#getPropertyType()
             */
            public int InternalGetPropertyType(int propEnum_0)
            {
                switch (propEnum_0)
                {
                    case IBM.ICU.Lang.UProperty_Constants.AGE:
                    case IBM.ICU.Lang.UProperty_Constants.BLOCK:
                    case IBM.ICU.Lang.UProperty_Constants.SCRIPT:
                        return IBM.ICU.Charset.UnicodeProperty.CATALOG;
                    case IBM.ICU.Lang.UProperty_Constants.ISO_COMMENT:
                    case IBM.ICU.Lang.UProperty_Constants.NAME:
                    case IBM.ICU.Lang.UProperty_Constants.UNICODE_1_NAME:
                        return IBM.ICU.Charset.UnicodeProperty.MISC;
                    case IBM.ICU.Lang.UProperty_Constants.BIDI_MIRRORING_GLYPH:
                    case IBM.ICU.Lang.UProperty_Constants.CASE_FOLDING:
                    case IBM.ICU.Lang.UProperty_Constants.LOWERCASE_MAPPING:
                    case IBM.ICU.Lang.UProperty_Constants.SIMPLE_CASE_FOLDING:
                    case IBM.ICU.Lang.UProperty_Constants.SIMPLE_LOWERCASE_MAPPING:
                    case IBM.ICU.Lang.UProperty_Constants.SIMPLE_TITLECASE_MAPPING:
                    case IBM.ICU.Lang.UProperty_Constants.SIMPLE_UPPERCASE_MAPPING:
                    case IBM.ICU.Lang.UProperty_Constants.TITLECASE_MAPPING:
                    case IBM.ICU.Lang.UProperty_Constants.UPPERCASE_MAPPING:
                        return IBM.ICU.Charset.UnicodeProperty.EXTENDED_STRING;
                }
                if (propEnum_0 < IBM.ICU.Lang.UProperty_Constants.BINARY_START)
                    return IBM.ICU.Charset.UnicodeProperty.UNKNOWN;
                if (propEnum_0 < IBM.ICU.Lang.UProperty_Constants.BINARY_LIMIT)
                    return IBM.ICU.Charset.UnicodeProperty.BINARY;
                if (propEnum_0 < IBM.ICU.Lang.UProperty_Constants.INT_START)
                    return IBM.ICU.Charset.UnicodeProperty.EXTENDED_BINARY;
                if (propEnum_0 < IBM.ICU.Lang.UProperty_Constants.INT_LIMIT)
                    return IBM.ICU.Charset.UnicodeProperty.ENUMERATED;
                if (propEnum_0 < IBM.ICU.Lang.UProperty_Constants.DOUBLE_START)
                    return IBM.ICU.Charset.UnicodeProperty.EXTENDED_ENUMERATED;
                if (propEnum_0 < IBM.ICU.Lang.UProperty_Constants.DOUBLE_LIMIT)
                    return IBM.ICU.Charset.UnicodeProperty.NUMERIC;
                if (propEnum_0 < IBM.ICU.Lang.UProperty_Constants.STRING_START)
                    return IBM.ICU.Charset.UnicodeProperty.EXTENDED_NUMERIC;
                if (propEnum_0 < IBM.ICU.Lang.UProperty_Constants.STRING_LIMIT)
                    return IBM.ICU.Charset.UnicodeProperty.STRING;
                return IBM.ICU.Charset.UnicodeProperty.EXTENDED_STRING;
            }

            /*
             * (non-Javadoc)
             * 
             * @see com.ibm.icu.dev.test.util.UnicodeProperty#getVersion()
             */
            protected internal override String _getVersion()
            {
                return IBM.ICU.Util.VersionInfo.ICU_VERSION.ToString();
            }
        }

        /*
         * { matchIterator = new UnicodeSetIterator( new
         * UnicodeSet("[^[:Cn:]-[:Default_Ignorable_Code_Point:]]")); }
         */

        /*
         * Other Missing Functions: Expands_On_NFC Expands_On_NFD Expands_On_NFKC
         * Expands_On_NFKD Composition_Exclusion Decomposition_Mapping
         * FC_NFKC_Closure ISO_Comment NFC_Quick_Check NFD_Quick_Check
         * NFKC_Quick_Check NFKD_Quick_Check Special_Case_Condition
         * Unicode_Radical_Stroke
         */

        static internal readonly ICUPropertyFactory.Names Binary_Extras = new ICUPropertyFactory.Names(IBM.ICU.Lang.UProperty_Constants.BINARY_LIMIT,
                new String[] { "isNFC", "isNFD", "isNFKC", "isNFKD", "isLowercase",
	                    "isUppercase", "isTitlecase", "isCasefolded", "isCased", });

        static internal readonly ICUPropertyFactory.Names String_Extras = new ICUPropertyFactory.Names(IBM.ICU.Lang.UProperty_Constants.STRING_LIMIT,
                new String[] { "toNFC", "toNFD", "toNFKC", "toNKFD", });

        internal const int isNFC = IBM.ICU.Lang.UProperty_Constants.BINARY_LIMIT,
                isNFD = IBM.ICU.Lang.UProperty_Constants.BINARY_LIMIT + 1,
                isNFKC = IBM.ICU.Lang.UProperty_Constants.BINARY_LIMIT + 2,
                isNFKD = IBM.ICU.Lang.UProperty_Constants.BINARY_LIMIT + 3,
                isLowercase = IBM.ICU.Lang.UProperty_Constants.BINARY_LIMIT + 4,
                isUppercase = IBM.ICU.Lang.UProperty_Constants.BINARY_LIMIT + 5,
                isTitlecase = IBM.ICU.Lang.UProperty_Constants.BINARY_LIMIT + 6,
                isCasefolded = IBM.ICU.Lang.UProperty_Constants.BINARY_LIMIT + 7,
                isCased = IBM.ICU.Lang.UProperty_Constants.BINARY_LIMIT + 8,

                NFC = IBM.ICU.Lang.UProperty_Constants.STRING_LIMIT, NFD = IBM.ICU.Lang.UProperty_Constants.STRING_LIMIT + 1,
                NFKC = IBM.ICU.Lang.UProperty_Constants.STRING_LIMIT + 2,
                NFKD = IBM.ICU.Lang.UProperty_Constants.STRING_LIMIT + 3;

        private ICUPropertyFactory()
        {
            ICollection c = GetInternalAvailablePropertyAliases(new ArrayList());
            IIterator it = new ILOG.J2CsMapping.Collections.IteratorAdapter(c.GetEnumerator());
            while (it.HasNext())
            {
                Add(GetInternalProperty((String)it.Next()));
            }
        }

        private static ICUPropertyFactory singleton = null;

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static ICUPropertyFactory Make()
        {
            if (singleton != null)
                return singleton;
            singleton = new ICUPropertyFactory();
            return singleton;
        }

        public IList GetInternalAvailablePropertyAliases(IList result)
        {
            int[][] ranges = {
	                new int[] { IBM.ICU.Lang.UProperty_Constants.BINARY_START,
	                        IBM.ICU.Lang.UProperty_Constants.BINARY_LIMIT },
	                new int[] { IBM.ICU.Lang.UProperty_Constants.INT_START,
	                        IBM.ICU.Lang.UProperty_Constants.INT_LIMIT },
	                new int[] { IBM.ICU.Lang.UProperty_Constants.DOUBLE_START,
	                        IBM.ICU.Lang.UProperty_Constants.DOUBLE_LIMIT },
	                new int[] { IBM.ICU.Lang.UProperty_Constants.STRING_START,
	                        IBM.ICU.Lang.UProperty_Constants.STRING_LIMIT } };
            for (int i = 0; i < ranges.Length; ++i)
            {
                for (int j = ranges[i][0]; j < ranges[i][1]; ++j)
                {
                    String alias = IBM.ICU.Lang.UCharacter.GetPropertyName(j,
                            IBM.ICU.Lang.UProperty_Constants.NameChoice.LONG);
                    IBM.ICU.Charset.UnicodeProperty.AddUnique(alias, result);
                    if (!result.Contains(alias))
                        ILOG.J2CsMapping.Collections.Generics.Collections.Add(result, alias);
                }
            }
            ILOG.J2CsMapping.Collections.Generics.Collections.AddAll(String_Extras.GetNames(), result);
            ILOG.J2CsMapping.Collections.Generics.Collections.AddAll(Binary_Extras.GetNames(), result);
            return result;
        }

        public UnicodeProperty GetInternalProperty(String propertyAlias)
        {
            int propEnum_0;
        main:
            {
                {
                    int possibleItem = Binary_Extras.Get(propertyAlias);
                    if (possibleItem >= 0)
                    {
                        propEnum_0 = possibleItem;
                        goto gotomain;
                    }
                    possibleItem = String_Extras.Get(propertyAlias);
                    if (possibleItem >= 0)
                    {
                        propEnum_0 = possibleItem;
                        goto gotomain;
                    }
                    propEnum_0 = IBM.ICU.Lang.UCharacter.GetPropertyEnum(propertyAlias);
                }
            }
        gotomain:
            ;
            return new ICUPropertyFactory.ICUProperty(propertyAlias, propEnum_0);
        }

        /*
         * (non-Javadoc)
         * 
         * @see
         * com.ibm.icu.dev.test.util.UnicodePropertySource#getProperty(java.lang
         * .String)
         */
        // TODO file bug on getPropertyValueName for Canonical_Combining_Class

        public class Names
        {
            private String[] names;

            private int bs;

            public Names(int bs, String[] names_0)
            {
                this.bs = bs;
                this.names = names_0;
            }

            public int Get(String name)
            {
                for (int i = 0; i < names.Length; ++i)
                {
                    if (name.Equals(names[i], StringComparison.InvariantCultureIgnoreCase))
                        return bs + i;
                }
                return -1;
            }

            public String Get(int number)
            {
                number -= bs;
                if (number < 0 || names.Length <= number)
                    return null;
                return names[number];
            }

            public bool IsInRange(int number)
            {
                number -= bs;
                return (0 <= number && number < names.Length);
            }

            public IList GetNames()
            {
                return ILOG.J2CsMapping.Collections.Arrays.AsList(names);
            }
        }
    }
    // #endif
}