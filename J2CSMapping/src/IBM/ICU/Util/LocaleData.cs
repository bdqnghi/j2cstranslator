/*
 *******************************************************************************
 * Copyright (C) 2004-2007, International Business Machines Corporation and    *
 * others. All Rights Reserved.                                                *
 *******************************************************************************
 */
// --------------------------------------------------------------------------------------------------
// This file was automatically generated by J2CS Translator (http://j2cstranslator.sourceforge.net/). 
// Version 1.3.6.20101125_01     
// 12/2/10 11:47 AM    
// ${CustomMessageForDisclaimer}                                                                             
// --------------------------------------------------------------------------------------------------
 namespace IBM.ICU.Util {
	
	using IBM.ICU.Impl;
	using IBM.ICU.Text;
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.IO;
	using System.Resources;
	using System.Runtime.CompilerServices;
	
	/// <summary>
	/// A class for accessing miscelleneous data in the locale bundles
	/// </summary>
	///
	/// @stable ICU 2.8
	public sealed class LocaleData {
	
	    private const String EXEMPLAR_CHARS = "ExemplarCharacters";
	
	    private const String MEASUREMENT_SYSTEM = "MeasurementSystem";
	
	    private const String PAPER_SIZE = "PaperSize";
	
	    private bool noSubstitute;
	
	    private ICUResourceBundle bundle;
	
	    /// <summary>
	    /// EXType for <see cref="M:IBM.ICU.Util.LocaleData.GetExemplarSet(System.Int32, System.Int32)"/>.
	    /// </summary>
	    ///
	    /// @stable ICU 3.4
	    public const int ES_STANDARD = 0;
	
	    /// <summary>
	    /// EXType for <see cref="M:IBM.ICU.Util.LocaleData.GetExemplarSet(System.Int32, System.Int32)"/>.
	    /// </summary>
	    ///
	    /// @stable ICU 3.4
	    public const int ES_AUXILIARY = 1;
	
	    /// <summary>
	    /// Count of EXTypes for <see cref="M:IBM.ICU.Util.LocaleData.GetExemplarSet(System.Int32, System.Int32)"/>.
	    /// </summary>
	    ///
	    /// @stable ICU 3.4
	    public const int ES_COUNT = 2;
	
	    /// <summary>
	    /// Delimiter type for <see cref="M:IBM.ICU.Util.LocaleData.GetDelimiter(System.Int32)"/>.
	    /// </summary>
	    ///
	    /// @stable ICU 3.4
	    public const int QUOTATION_START = 0;
	
	    /// <summary>
	    /// Delimiter type for <see cref="M:IBM.ICU.Util.LocaleData.GetDelimiter(System.Int32)"/>.
	    /// </summary>
	    ///
	    /// @stable ICU 3.4
	    public const int QUOTATION_END = 1;
	
	    /// <summary>
	    /// Delimiter type for <see cref="M:IBM.ICU.Util.LocaleData.GetDelimiter(System.Int32)"/>.
	    /// </summary>
	    ///
	    /// @stable ICU 3.4
	    public const int ALT_QUOTATION_START = 2;
	
	    /// <summary>
	    /// Delimiter type for <see cref="M:IBM.ICU.Util.LocaleData.GetDelimiter(System.Int32)"/>.
	    /// </summary>
	    ///
	    /// @stable ICU 3.4
	    public const int ALT_QUOTATION_END = 3;
	
	    /// <summary>
	    /// Count of delimiter types for <see cref="M:IBM.ICU.Util.LocaleData.GetDelimiter(System.Int32)"/>.
	    /// </summary>
	    ///
	    /// @stable ICU 3.4
	    public const int DELIMITER_COUNT = 4;
	
	    // private constructor to prevent default construction
	    // /CLOVER:OFF
	    private LocaleData() {
	    }
	
	    // /CLOVER:ON
	
	    /// <summary>
	    /// Returns the set of exemplar characters for a locale.
	    /// </summary>
	    ///
	    /// <param name="locale">Locale for which the exemplar character set is to beretrieved.</param>
	    /// <param name="options">Bitmask for options to apply to the exemplar pattern. Specifyzero to retrieve the exemplar set as it is defined in thelocale data. Specify UnicodeSet.CASE to retrieve a case-foldedexemplar set. See <see cref="null"/>for a complete list of valid options. The IGNORE_SPACE bit isalways set, regardless of the value of 'options'.</param>
	    /// <returns>The set of exemplar characters for the given locale.</returns>
	    /// @stable ICU 3.0
	    public static UnicodeSet GetExemplarSet(ULocale locale, int options) {
	        ICUResourceBundle bundle_0 = (ICUResourceBundle) IBM.ICU.Util.UResourceBundle
	                .GetBundleInstance(IBM.ICU.Impl.ICUResourceBundle.ICU_BASE_NAME, locale);
	        String pattern = bundle_0.GetString(EXEMPLAR_CHARS);
	        return new UnicodeSet(pattern, IBM.ICU.Text.UnicodeSet.IGNORE_SPACE | options);
	    }
	
	    /// <summary>
	    /// Returns the set of exemplar characters for a locale.
	    /// </summary>
	    ///
	    /// <param name="options">Bitmask for options to apply to the exemplar pattern. Specifyzero to retrieve the exemplar set as it is defined in thelocale data. Specify UnicodeSet.CASE to retrieve a case-foldedexemplar set. See <see cref="null"/>for a complete list of valid options. The IGNORE_SPACE bit isalways set, regardless of the value of 'options'.</param>
	    /// <param name="extype">The type of exemplar set to be retrieved, ES_STANDARD orES_AUXILIARY</param>
	    /// <returns>The set of exemplar characters for the given locale.</returns>
	    /// @stable ICU 3.4
	    public UnicodeSet GetExemplarSet(int options, int extype) {
	        String[] exemplarSetTypes = { "ExemplarCharacters",
	                "AuxExemplarCharacters" };
	        try {
	            ICUResourceBundle stringBundle = (ICUResourceBundle) bundle
	                    .Get(exemplarSetTypes[extype]);
	
	            if (noSubstitute
	                    && (stringBundle.GetLoadingStatus() == IBM.ICU.Impl.ICUResourceBundle.FROM_ROOT))
	                return null;
	
	            return new UnicodeSet(stringBundle.GetString(),
	                    IBM.ICU.Text.UnicodeSet.IGNORE_SPACE | options);
	        } catch (MissingManifestResourceException ex) {
	            if (extype == LocaleData.ES_AUXILIARY) {
	                return new UnicodeSet();
	            }
	            throw ex;
	        }
	    }
	
	    /// <summary>
	    /// Gets the LocaleData object associated with the ULocale specified in
	    /// locale
	    /// </summary>
	    ///
	    /// <param name="locale">Locale with thich the locale data object is associated.</param>
	    /// <returns>A locale data object.</returns>
	    /// @stable ICU 3.4
	    public static LocaleData GetInstance(ULocale locale) {
	        LocaleData ld = new LocaleData();
	        ld.bundle = (ICUResourceBundle) IBM.ICU.Util.UResourceBundle.GetBundleInstance(
	                IBM.ICU.Impl.ICUResourceBundle.ICU_BASE_NAME, locale);
	        ld.noSubstitute = false;
	        return ld;
	    }
	
	    /// <summary>
	    /// Gets the LocaleData object associated with the default locale
	    /// </summary>
	    ///
	    /// <returns>A locale data object.</returns>
	    /// @stable ICU 3.4
	    public static LocaleData GetInstance() {
	        return LocaleData.GetInstance(IBM.ICU.Util.ULocale.GetDefault());
	    }
	
	    /// <summary>
	    /// Sets the "no substitute" behavior of this locale data object.
	    /// </summary>
	    ///
	    /// <param name="setting">Value for the no substitute behavior. If TRUE, methods of thislocale data object will return an error when no data isavailable for that method, given the locale ID supplied to theconstructor.</param>
	    /// @stable ICU 3.4
	    public void SetNoSubstitute(bool setting) {
	        noSubstitute = setting;
	    }
	
	    /// <summary>
	    /// Gets the "no substitute" behavior of this locale data object.
	    /// </summary>
	    ///
	    /// <returns>Value for the no substitute behavior. If TRUE, methods of this
	    /// locale data object will return an error when no data is available
	    /// for that method, given the locale ID supplied to the constructor.</returns>
	    /// @stable ICU 3.4
	    public bool GetNoSubstitute() {
	        return noSubstitute;
	    }
	
	    /// <summary>
	    /// Retrieves a delimiter string from the locale data.
	    /// </summary>
	    ///
	    /// <param name="type">The type of delimiter string desired. Currently, the validchoices are QUOTATION_START, QUOTATION_END,ALT_QUOTATION_START, or ALT_QUOTATION_END.</param>
	    /// <returns>The desired delimiter string.</returns>
	    /// @stable ICU 3.4
	    public String GetDelimiter(int type) {
	        String[] delimiterTypes = { "quotationStart", "quotationEnd",
	                "alternateQuotationStart", "alternateQuotationEnd" };
	
	        ICUResourceBundle stringBundle = (ICUResourceBundle) bundle.Get(
	                "delimiters").Get(delimiterTypes[type]);
	
	        if (noSubstitute
	                && (stringBundle.GetLoadingStatus() == IBM.ICU.Impl.ICUResourceBundle.FROM_ROOT))
	            return null;
	
	        return stringBundle.GetString();
	    }
	
	    /// <summary>
	    /// Enumeration for representing the measurement systems.
	    /// </summary>
	    ///
	    /// @stable ICU 2.8
	    public sealed class MeasurementSystem {
	        /// <summary>
	        /// Measurement system specified by Le Syst&#x00E8;me International
	        /// d'Unit&#x00E9;s (SI) otherwise known as Metric system.
	        /// </summary>
	        ///
	        /// @stable ICU 2.8
	        public static readonly LocaleData.MeasurementSystem  SI = new LocaleData.MeasurementSystem (0);
	
	        /// <summary>
	        /// Measurement system followed in the United States of America.
	        /// </summary>
	        ///
	        /// @stable ICU 2.8
	        public static readonly LocaleData.MeasurementSystem  US = new LocaleData.MeasurementSystem (1);
	
	        private int systemID;
	
	        public MeasurementSystem(int id) {
	            systemID = id;
	        }
	
	        public bool Equals(int id) {
	            return systemID == id;
	        }
	    }
	
	    /// <summary>
	    /// Returns the measurement system used in the locale specified by the
	    /// locale.
	    /// </summary>
	    ///
	    /// <param name="locale">The locale for which the measurement system to be retrieved.</param>
	    /// <returns>MeasurementSystem the measurement system used in the locale.</returns>
	    /// @stable ICU 3.0
	    public static LocaleData.MeasurementSystem  GetMeasurementSystem(ULocale locale) {
	        UResourceBundle bundle_0 = (ICUResourceBundle) IBM.ICU.Util.UResourceBundle
	                .GetBundleInstance(IBM.ICU.Impl.ICUResourceBundle.ICU_BASE_NAME, locale);
	        UResourceBundle sysBundle = bundle_0.Get(MEASUREMENT_SYSTEM);
	
	        int system = sysBundle.GetInt();
	        if (IBM.ICU.Util.LocaleData.MeasurementSystem.US.Equals(system)) {
	            return IBM.ICU.Util.LocaleData.MeasurementSystem.US;
	        }
	        if (IBM.ICU.Util.LocaleData.MeasurementSystem.SI.Equals(system)) {
	            return IBM.ICU.Util.LocaleData.MeasurementSystem.SI;
	        }
	        // return null if the object is null or is not an instance
	        // of integer indicating an error
	        return null;
	    }
	
	    /// <summary>
	    /// A class that represents the size of letter head used in the country
	    /// </summary>
	    ///
	    /// @stable ICU 2.8
	    public sealed class PaperSize {
	        private int height;
	
	        private int width;
	
	        public PaperSize(int h, int w) {
	            height = h;
	            width = w;
	        }
	
	        /// <summary>
	        /// Retruns the height of the paper
	        /// </summary>
	        ///
	        /// <returns>the height</returns>
	        /// @stable ICU 2.8
	        public int GetHeight() {
	            return height;
	        }
	
	        /// <summary>
	        /// Returns the width of hte paper
	        /// </summary>
	        ///
	        /// <returns>the width</returns>
	        /// @stable ICU 2.8
	        public int GetWidth() {
	            return width;
	        }
	    }
	
	    /// <summary>
	    /// Returns the size of paper used in the locale. The paper sizes returned
	    /// are always in <em> milli-meters<em>.
	    /// </summary>
	    ///
	    /// <param name="locale">The locale for which the measurement system to be retrieved.</param>
	    /// <returns>The paper size used in the locale</returns>
	    /// @stable ICU 3.0
	    public static LocaleData.PaperSize  GetPaperSize(ULocale locale) {
	        UResourceBundle bundle_0 = (ICUResourceBundle) IBM.ICU.Util.UResourceBundle
	                .GetBundleInstance(IBM.ICU.Impl.ICUResourceBundle.ICU_BASE_NAME, locale);
	        UResourceBundle obj = bundle_0.Get(PAPER_SIZE);
	        int[] size = obj.GetIntVector();
	        return new LocaleData.PaperSize (size[0], size[1]);
	    }
	}
}
