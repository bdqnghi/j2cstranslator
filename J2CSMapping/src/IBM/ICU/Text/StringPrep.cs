/*
 *******************************************************************************
 * Copyright (C) 2003-2007, International Business Machines Corporation and    *
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
	using System.Collections;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.IO;
	using System.Runtime.CompilerServices;
	using System.Text;
	
	/// <summary>
	/// StringPrep API implements the StingPrep framework as described by <a
	/// href="http://www.ietf.org/rfc/rfc3454.txt">RFC 3454</a>. StringPrep prepares
	/// Unicode strings for use in network protocols. Profiles of StingPrep are set
	/// of rules and data according to which the Unicode Strings are prepared. Each
	/// profiles contains tables which describe how a code point should be treated.
	/// The tables are broadly classied into
	/// <ul>
	/// <li>Unassigned Table: Contains code points that are unassigned in the Unicode
	/// Version supported by StringPrep. Currently RFC 3454 supports Unicode 3.2.</li>
	/// <li>Prohibited Table: Contains code points that are prohibted from the output
	/// of the StringPrep processing function.</li>
	/// <li>Mapping Table: Contains code ponts that are deleted from the output or
	/// case mapped.</li>
	/// </ul>
	/// The procedure for preparing Unicode strings:
	/// <ol>
	/// <li>Map: For each character in the input, check if it has a mapping and, if
	/// so, replace it with its mapping.</li>
	/// <li>Normalize: Possibly normalize the result of step 1 using Unicode
	/// normalization.</li>
	/// <li>Prohibit: Check for any characters that are not allowed in the output. If
	/// any are found, return an error.</li>
	/// <li>Check bidi: Possibly check for right-to-left characters, and if any are
	/// found, make sure that the whole string satisfies the requirements for
	/// bidirectional strings. If the string does not satisfy the requirements for
	/// bidirectional strings, return an error.</li>
	/// </ol>
	/// </summary>
	///
	/// @stable ICU 2.8
	public sealed class StringPrep {
	    /// <summary>
	    /// Option to prohibit processing of unassigned code points in the input
	    /// </summary>
	    ///
	    /// <seealso cref="M:IBM.ICU.Text.StringPrep.Prepare(IBM.ICU.Text.UCharacterIterator, System.Int32)"/>
	    /// @stable ICU 2.8
	    public const int DEFAULT = 0x0000;
	
	    /// <summary>
	    /// Option to allow processing of unassigned code points in the input
	    /// </summary>
	    ///
	    /// <seealso cref="M:IBM.ICU.Text.StringPrep.Prepare(IBM.ICU.Text.UCharacterIterator, System.Int32)"/>
	    /// @stable ICU 2.8
	    public const int ALLOW_UNASSIGNED = 0x0001;
	
	    private const int UNASSIGNED = 0x0000;
	
	    private const int MAP = 0x0001;
	
	    private const int PROHIBITED = 0x0002;
	
	    private const int DELETE = 0x0003;
	
	    private const int TYPE_LIMIT = 0x0004;
	
	    private const int NORMALIZATION_ON = 0x0001;
	
	    private const int CHECK_BIDI_ON = 0x0002;
	
	    private const int TYPE_THRESHOLD = 0xFFF0;
	
	    private const int MAX_INDEX_VALUE = 0x3FBF; /* 16139 */
	
	    // private static final int MAX_INDEX_TOP_LENGTH = 0x0003;
	
	    /* indexes[] value names */
	    private const int INDEX_TRIE_SIZE = 0; /*
	                                                   * number of bytes in
	                                                   * normalization trie
	                                                   */
	
	    private const int INDEX_MAPPING_DATA_SIZE = 1; /*
	                                                           * The array that
	                                                           * contains the
	                                                           * mapping
	                                                           */
	
	    private const int NORM_CORRECTNS_LAST_UNI_VERSION = 2; /*
	                                                                   * The index
	                                                                   * of Unicode
	                                                                   * version of
	                                                                   * last entry
	                                                                   * in
	                                                                   * NormalizationCorrections
	                                                                   * .txt
	                                                                   */
	
	    private const int ONE_UCHAR_MAPPING_INDEX_START = 3; /*
	                                                                 * The starting
	                                                                 * index of 1
	                                                                 * UChar mapping
	                                                                 * index in the
	                                                                 * mapping data
	                                                                 * array
	                                                                 */
	
	    private const int TWO_UCHARS_MAPPING_INDEX_START = 4; /*
	                                                                  * The starting
	                                                                  * index of 2
	                                                                  * UChars
	                                                                  * mapping
	                                                                  * index in the
	                                                                  * mapping data
	                                                                  * array
	                                                                  */
	
	    private const int THREE_UCHARS_MAPPING_INDEX_START = 5;
	
	    private const int FOUR_UCHARS_MAPPING_INDEX_START = 6;
	
	    private const int OPTIONS = 7; /*
	                                           * Bit set of options to turn on in
	                                           * the profile
	                                           */
	
	    private const int INDEX_TOP = 16; /*
	                                              * changing this requires a new
	                                              * formatVersion
	                                              */
	
	    /// <summary>
	    /// Default buffer size of datafile
	    /// </summary>
	    ///
	    private const int DATA_BUFFER_SIZE = 25000;
	
	    // CharTrie implmentation for reading the trie data
	    private CharTrie sprepTrie;
	
	    // Indexes read from the data file
	    private int[] indexes;
	
	    // mapping data read from the data file
	    private char[] mappingData;
	
	    // format version of the data file
	    // private byte[] formatVersion;
	    // the version of Unicode supported by the data file
	    private VersionInfo sprepUniVer;
	
	    // the Unicode version of last entry in the
	    // NormalizationCorrections.txt file if normalization
	    // is turned on
	    private VersionInfo normCorrVer;
	
	    // Option to turn on Normalization
	    private bool doNFKC;
	
	    // Option to turn on checking for BiDi rules
	    private bool checkBiDi;
	
	    // bidi properties
	    private UBiDiProps bdp;
	
	    private char GetCodePointValue(int ch) {
	        return sprepTrie.GetCodePointValue(ch);
	    }
	
	    private static VersionInfo GetVersionInfo(int comp) {
	        int micro = comp & 0xFF;
	        int milli = (comp >> 8) & 0xFF;
	        int minor = (comp >> 16) & 0xFF;
	        int major = (comp >> 24) & 0xFF;
	        return IBM.ICU.Util.VersionInfo.GetInstance(major, minor, milli, micro);
	    }
	
	    private static VersionInfo GetVersionInfo(byte[] version) {
	        if (version.Length != 4) {
	            return null;
	        }
	        return IBM.ICU.Util.VersionInfo.GetInstance((int) version[0], (int) version[1],
	                (int) version[2], (int) version[3]);
	    }
	
	    /// <summary>
	    /// Creates an StringPrep object after reading the input stream. The object
	    /// does not hold a reference to the input steam, so the stream can be closed
	    /// after the method returns.
	    /// </summary>
	    ///
	    /// <param name="inputStream">The stream for reading the StringPrep profile binarySun</param>
	    /// <exception cref="IOException"></exception>
	    /// @stable ICU 2.8
	    public StringPrep(Stream inputStream) {
	
	        BufferedStream b = new BufferedStream(inputStream,DATA_BUFFER_SIZE);
	
	        StringPrepDataReader reader = new StringPrepDataReader(b);
	
	        // read the indexes
	        indexes = reader.ReadIndexes(INDEX_TOP);
	
	        byte[] sprepBytes = new byte[indexes[INDEX_TRIE_SIZE]];
	
	        // indexes[INDEX_MAPPING_DATA_SIZE] store the size of mappingData in
	        // bytes
	        mappingData = new char[indexes[INDEX_MAPPING_DATA_SIZE] / 2];
	        // load the rest of the data data and initialize the data members
	        reader.Read(sprepBytes, mappingData);

            throw new Exception("Missing code");
	       // sprepTrie = new CharTrie(sprepBytes, null);
	
	        // get the data format version
	        /* formatVersion = */reader.GetDataFormatVersion();
	
	        // get the options
	        doNFKC = ((indexes[OPTIONS] & NORMALIZATION_ON) > 0);
	        checkBiDi = ((indexes[OPTIONS] & CHECK_BIDI_ON) > 0);
	        sprepUniVer = GetVersionInfo(reader.GetUnicodeVersion());
	        normCorrVer = GetVersionInfo(indexes[NORM_CORRECTNS_LAST_UNI_VERSION]);
	        VersionInfo normUniVer = IBM.ICU.Text.Normalizer.GetUnicodeVersion();
	        if (normUniVer.CompareTo(sprepUniVer) < 0 && /*
	                                                      * the Unicode version of
	                                                      * SPREP file must be less
	                                                      * than the Unicode Vesion
	                                                      * of the normalization
	                                                      * data
	                                                      */
	        normUniVer.CompareTo(normCorrVer) < 0 && /*
	                                                  * the Unicode version of the
	                                                  * NormalizationCorrections.txt
	                                                  * file should be less than the
	                                                  * Unicode Vesion of the
	                                                  * normalization data
	                                                  */
	        ((indexes[OPTIONS] & NORMALIZATION_ON) > 0) /* normalization turned on */
	        ) {
	            throw new IOException(
	                    "Normalization Correction version not supported");
	        }
	        b.Close();
	
	        if (checkBiDi) {
	            bdp = IBM.ICU.Impl.UBiDiProps.GetSingleton();
	        }
	    }
	
	    private sealed class Values {
	        internal bool isIndex;
	
	        internal int value_ren;
	
	        internal int type;
	
	        public void Reset() {
	            isIndex = false;
	            value_ren = 0;
	            type = -1;
	        }
	    }
	
	    private static void GetValues(char trieWord, StringPrep.Values  values) {
	        values.Reset();
	        if (trieWord == 0) {
	            /*
	             * Initial value stored in the mapping table just return TYPE_LIMIT
	             * .. so that the source codepoint is copied to the destination
	             */
	            values.type = TYPE_LIMIT;
	        } else if (trieWord >= TYPE_THRESHOLD) {
	            values.type = (trieWord - TYPE_THRESHOLD);
	        } else {
	            /* get the type */
	            values.type = MAP;
	            /* ascertain if the value is index or delta */
	            if ((trieWord & 0x02) > 0) {
	                values.isIndex = true;
	                values.value_ren = trieWord >> 2; // mask off the lower 2 bits and
	                                              // shift
	
	            } else {
	                values.isIndex = false;
	                values.value_ren = ((int) (trieWord << 16)) >> 16;
	                values.value_ren = (values.value_ren >> 2);
	
	            }
	
	            if ((trieWord >> 2) == MAX_INDEX_VALUE) {
	                values.type = DELETE;
	                values.isIndex = false;
	                values.value_ren = 0;
	            }
	        }
	    }
	
	    private StringBuilder Map(UCharacterIterator iter, int options) {
	
	        StringPrep.Values  val = new StringPrep.Values ();
	        char result = (char) (0);
	        int ch = IBM.ICU.Text.UForwardCharacterIterator_Constants.DONE;
	        StringBuilder dest = new StringBuilder();
	        bool allowUnassigned = ((options & ALLOW_UNASSIGNED) > 0);
	
	        while ((ch = iter.NextCodePoint()) != IBM.ICU.Text.UForwardCharacterIterator_Constants.DONE) {
	
	            result = GetCodePointValue(ch);
	            GetValues(result, val);
	
	            // check if the source codepoint is unassigned
	            if (val.type == UNASSIGNED && allowUnassigned == false) {
	                throw new StringPrepParseException(
	                        "An unassigned code point was found in the input",
	                        IBM.ICU.Text.StringPrepParseException.UNASSIGNED_ERROR,
	                        iter.GetText(), iter.GetIndex());
	            } else if ((val.type == MAP)) {
	                int index, length;
	
	                if (val.isIndex) {
	                    index = val.value_ren;
	                    if (index >= indexes[ONE_UCHAR_MAPPING_INDEX_START]
	                            && index < indexes[TWO_UCHARS_MAPPING_INDEX_START]) {
	                        length = 1;
	                    } else if (index >= indexes[TWO_UCHARS_MAPPING_INDEX_START]
	                            && index < indexes[THREE_UCHARS_MAPPING_INDEX_START]) {
	                        length = 2;
	                    } else if (index >= indexes[THREE_UCHARS_MAPPING_INDEX_START]
	                            && index < indexes[FOUR_UCHARS_MAPPING_INDEX_START]) {
	                        length = 3;
	                    } else {
	                        length = mappingData[index++];
	                    }
	                    /* copy mapping to destination */
	                    dest.Append(mappingData, index, length);
	                    continue;
	
	                } else {
	                    ch -= val.value_ren;
	                }
	            } else if (val.type == DELETE) {
	                // just consume the codepoint and contine
	                continue;
	            }
	            // copy the source into destination
	            IBM.ICU.Text.UTF16.Append(dest, ch);
	        }
	
	        return dest;
	    }
	
	    private StringBuilder Normalize(StringBuilder src) {
	        /*
	         * Option UNORM_BEFORE_PRI_29:
	         * 
	         * IDNA as interpreted by IETF members (see unicode mailing list 2004H1)
	         * requires strict adherence to Unicode 3.2 normalization, including
	         * buggy composition from before fixing Public Review Issue #29. Note
	         * that this results in some valid but nonsensical text to be either
	         * corrupted or rejected, depending on the text. See
	         * http://www.unicode.org/review/resolved-pri.html#pri29 See unorm.cpp
	         * and cnormtst.c
	         */
	        return new StringBuilder(IBM.ICU.Text.Normalizer.Normalize(src.ToString(),
	                IBM.ICU.Text.Normalizer.NFKC, IBM.ICU.Text.Normalizer.UNICODE_3_2
	                        | IBM.ICU.Impl.NormalizerImpl.BEFORE_PRI_29));
	    }
	
	    /*
	     * boolean isLabelSeparator(int ch){ int result = getCodePointValue(ch); if(
	     * (result & 0x07) == LABEL_SEPARATOR){ return true; } return false; }
	     */
	    /*
	     * 1) Map -- For each character in the input, check if it has a mapping and,
	     * if so, replace it with its mapping.
	     * 
	     * 2) Normalize -- Possibly normalize the result of step 1 using Unicode
	     * normalization.
	     * 
	     * 3) Prohibit -- Check for any characters that are not allowed in the
	     * output. If any are found, return an error.
	     * 
	     * 4) Check bidi -- Possibly check for right-to-left characters, and if any
	     * are found, make sure that the whole string satisfies the requirements for
	     * bidirectional strings. If the string does not satisfy the requirements
	     * for bidirectional strings, return an error. [Unicode3.2] defines several
	     * bidirectional categories; each character has one bidirectional category
	     * assigned to it. For the purposes of the requirements below, an
	     * "RandALCat character" is a character that has Unicode bidirectional
	     * categories "R" or "AL"; an "LCat character" is a character that has
	     * Unicode bidirectional category "L". Note
	     * 
	     * 
	     * that there are many characters which fall in neither of the above
	     * definitions; Latin digits (<U+0030> through <U+0039>) are examples of
	     * this because they have bidirectional category "EN".
	     * 
	     * In any profile that specifies bidirectional character handling, all three
	     * of the following requirements MUST be met:
	     * 
	     * 1) The characters in section 5.8 MUST be prohibited.
	     * 
	     * 2) If a string contains any RandALCat character, the string MUST NOT
	     * contain any LCat character.
	     * 
	     * 3) If a string contains any RandALCat character, a RandALCat character
	     * MUST be the first character of the string, and a RandALCat character MUST
	     * be the last character of the string.
	     */
	    /// <summary>
	    /// Prepare the input buffer for use in applications with the given profile.
	    /// This operation maps, normalizes(NFKC), checks for prohited and BiDi
	    /// characters in the order defined by RFC 3454 depending on the options
	    /// specified in the profile.
	    /// </summary>
	    ///
	    /// <param name="src">A UCharacterIterator object containing the source string</param>
	    /// <param name="options">A bit set of options:- StringPrep.NONE Prohibit processing of unassigned codepoints in the input- StringPrep.ALLOW_UNASSIGNED Treat the unassigned code pointsare in the input as normal Unicode code points.</param>
	    /// <returns>StringBuffer A StringBuffer containing the output</returns>
	    /// <exception cref="ParseException"></exception>
	    /// @stable ICU 2.8
	    public StringBuilder Prepare(UCharacterIterator src, int options) {
	
	        // map
	        StringBuilder mapOut = Map(src, options);
	        StringBuilder normOut = mapOut;// initialize
	
	        if (doNFKC) {
	            // normalize
	            normOut = Normalize(mapOut);
	        }
	
	        int ch;
	        char result;
	        UCharacterIterator iter = IBM.ICU.Text.UCharacterIterator.GetInstance(normOut);
	        StringPrep.Values  val = new StringPrep.Values ();
	        int direction = IBM.ICU.Lang.UCharacterEnums.ECharacterDirection.CHAR_DIRECTION_COUNT, firstCharDir = IBM.ICU.Lang.UCharacterEnums.ECharacterDirection.CHAR_DIRECTION_COUNT;
	        int rtlPos = -1, ltrPos = -1;
	        bool rightToLeft = false, leftToRight = false;
	
	        while ((ch = iter.NextCodePoint()) != IBM.ICU.Text.UForwardCharacterIterator_Constants.DONE) {
	            result = GetCodePointValue(ch);
	            GetValues(result, val);
	
	            if (val.type == PROHIBITED) {
	                throw new StringPrepParseException(
	                        "A prohibited code point was found in the input",
	                        IBM.ICU.Text.StringPrepParseException.PROHIBITED_ERROR,
	                        iter.GetText(), val.value_ren);
	            }
	
	            if (checkBiDi) {
	                direction = bdp.GetClass(ch);
	                if (firstCharDir == IBM.ICU.Lang.UCharacterEnums.ECharacterDirection.CHAR_DIRECTION_COUNT) {
	                    firstCharDir = direction;
	                }
	                if (direction == IBM.ICU.Lang.UCharacterEnums.ECharacterDirection.LEFT_TO_RIGHT) {
	                    leftToRight = true;
	                    ltrPos = iter.GetIndex() - 1;
	                }
	                if (direction == IBM.ICU.Lang.UCharacterEnums.ECharacterDirection.RIGHT_TO_LEFT
	                        || direction == IBM.ICU.Lang.UCharacterEnums.ECharacterDirection.RIGHT_TO_LEFT_ARABIC) {
	                    rightToLeft = true;
	                    rtlPos = iter.GetIndex() - 1;
	                }
	            }
	        }
	        if (checkBiDi == true) {
	            // satisfy 2
	            if (leftToRight == true && rightToLeft == true) {
	                throw new StringPrepParseException(
	                        "The input does not conform to the rules for BiDi code points.",
	                        IBM.ICU.Text.StringPrepParseException.CHECK_BIDI_ERROR, iter
	                                .GetText(), (rtlPos > ltrPos) ? rtlPos : ltrPos);
	            }
	
	            // satisfy 3
	            if (rightToLeft == true
	                    && !((firstCharDir == IBM.ICU.Lang.UCharacterEnums.ECharacterDirection.RIGHT_TO_LEFT || firstCharDir == IBM.ICU.Lang.UCharacterEnums.ECharacterDirection.RIGHT_TO_LEFT_ARABIC) && (direction == IBM.ICU.Lang.UCharacterEnums.ECharacterDirection.RIGHT_TO_LEFT || direction == IBM.ICU.Lang.UCharacterEnums.ECharacterDirection.RIGHT_TO_LEFT_ARABIC))) {
	                throw new StringPrepParseException(
	                        "The input does not conform to the rules for BiDi code points.",
	                        IBM.ICU.Text.StringPrepParseException.CHECK_BIDI_ERROR, iter
	                                .GetText(), (rtlPos > ltrPos) ? rtlPos : ltrPos);
	            }
	        }
	        return normOut;
	
	    }
	}
}