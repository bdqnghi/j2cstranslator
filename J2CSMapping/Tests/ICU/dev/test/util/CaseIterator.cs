// --------------------------------------------------------------------------------------------------
// This file was automatically generated by J2CS Translator (http://j2cstranslator.sourceforge.net/). 
// Version 1.3.6.20101125_01     
// 12/13/10 4:01 PM    
// ${CustomMessageForDisclaimer}                                                                             
// --------------------------------------------------------------------------------------------------
 /// <summary>
/// Copyright (C) 1996-2007, International Business Machines Corporation and    
/// others. All Rights Reserved.                                                
/// </summary>
///
namespace IBM.ICU.Charset {
	
	using IBM.ICU.Text;
	using ILOG.J2CsMapping.Collections;
	using ILOG.J2CsMapping.Collections.Generics;
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.IO;
	using System.Runtime.CompilerServices;
	using System.Text;
	
	/// <summary>
	/// Incrementally returns the set of all strings that case-fold to the same
	/// value.
	/// </summary>
	///
	public class CaseIterator {
	
	    public CaseIterator() {
	        this.count = 0;
	        this.done = false;
	        this.nextBuffer = new StringBuilder();
	    }
	
	    // testing stuff
	    private static Transliterator toName = IBM.ICU.Text.Transliterator
	            .GetInstance("[:^ascii:] Any-Name");
	
	    private static Transliterator toHex = IBM.ICU.Text.Transliterator
	            .GetInstance("[:^ascii:] Any-Hex");
	
	    private static Transliterator toHex2 = IBM.ICU.Text.Transliterator
	            .GetInstance("[[^\u0021-\u007F]-[,]] Any-Hex");
	
	    // global tables (could be precompiled)
	    private static IDictionary fromCaseFold = new Hashtable();
	
	    private static IDictionary toCaseFold = new Hashtable();
	
	    private static int maxLength = 0;
	
	    // This exception list is generated on the console by turning on the
	    // GENERATED flag,
	    // which MUST be false for normal operation.
	    // Once the list is generated, it is pasted in here.
	    // A bit of a cludge, but this bootstrapping is the easiest way
	    // to get around certain complications in the data.
	
	    private const bool GENERATE = false;
	
	    private const bool DUMP = false;
	
	    private static String[][] exceptionList = {
	            new String[] { "a\u02BE", "A\u02BE", "a\u02BE" },
	            new String[] { "ff", "FF", "Ff", "fF", "ff" },
	            new String[] { "ffi", "FFI", "FFi", "FfI", "Ffi", "F\uFB01", "fFI",
	                    "fFi", "ffI", "ffi", "f\uFB01", "\uFB00I", "\uFB00i" },
	            new String[] { "ffl", "FFL", "FFl", "FfL", "Ffl", "F\uFB02", "fFL",
	                    "fFl", "ffL", "ffl", "f\uFB02", "\uFB00L", "\uFB00l" },
	            new String[] { "fi", "FI", "Fi", "fI", "fi" },
	            new String[] { "fl", "FL", "Fl", "fL", "fl" },
	            new String[] { "h\u0331", "H\u0331", "h\u0331" },
	            new String[] { "i\u0307", "I\u0307", "i\u0307" },
	            new String[] { "j\u030C", "J\u030C", "j\u030C" },
	            new String[] { "ss", "SS", "Ss", "S\u017F", "sS", "ss", "s\u017F",
	                    "\u017FS", "\u017Fs", "\u017F\u017F" },
	            new String[] { "st", "ST", "St", "sT", "st", "\u017FT", "\u017Ft" },
	            new String[] { "t\u0308", "T\u0308", "t\u0308" },
	            new String[] { "w\u030A", "W\u030A", "w\u030A" },
	            new String[] { "y\u030A", "Y\u030A", "y\u030A" },
	            new String[] { "\u02BCn", "\u02BCN", "\u02BCn" },
	            new String[] { "\u03AC\u03B9", "\u0386\u0345", "\u0386\u0399",
	                    "\u0386\u03B9", "\u0386\u1FBE", "\u03AC\u0345",
	                    "\u03AC\u0399", "\u03AC\u03B9", "\u03AC\u1FBE" },
	            new String[] { "\u03AE\u03B9", "\u0389\u0345", "\u0389\u0399",
	                    "\u0389\u03B9", "\u0389\u1FBE", "\u03AE\u0345",
	                    "\u03AE\u0399", "\u03AE\u03B9", "\u03AE\u1FBE" },
	            new String[] { "\u03B1\u0342", "\u0391\u0342", "\u03B1\u0342" },
	            new String[] { "\u03B1\u0342\u03B9", "\u0391\u0342\u0345",
	                    "\u0391\u0342\u0399", "\u0391\u0342\u03B9",
	                    "\u0391\u0342\u1FBE", "\u03B1\u0342\u0345",
	                    "\u03B1\u0342\u0399", "\u03B1\u0342\u03B9",
	                    "\u03B1\u0342\u1FBE", "\u1FB6\u0345", "\u1FB6\u0399",
	                    "\u1FB6\u03B9", "\u1FB6\u1FBE" },
	            new String[] { "\u03B1\u03B9", "\u0391\u0345", "\u0391\u0399",
	                    "\u0391\u03B9", "\u0391\u1FBE", "\u03B1\u0345",
	                    "\u03B1\u0399", "\u03B1\u03B9", "\u03B1\u1FBE" },
	            new String[] { "\u03B7\u0342", "\u0397\u0342", "\u03B7\u0342" },
	            new String[] { "\u03B7\u0342\u03B9", "\u0397\u0342\u0345",
	                    "\u0397\u0342\u0399", "\u0397\u0342\u03B9",
	                    "\u0397\u0342\u1FBE", "\u03B7\u0342\u0345",
	                    "\u03B7\u0342\u0399", "\u03B7\u0342\u03B9",
	                    "\u03B7\u0342\u1FBE", "\u1FC6\u0345", "\u1FC6\u0399",
	                    "\u1FC6\u03B9", "\u1FC6\u1FBE" },
	            new String[] { "\u03B7\u03B9", "\u0397\u0345", "\u0397\u0399",
	                    "\u0397\u03B9", "\u0397\u1FBE", "\u03B7\u0345",
	                    "\u03B7\u0399", "\u03B7\u03B9", "\u03B7\u1FBE" },
	            new String[] { "\u03B9\u0308\u0300", "\u0345\u0308\u0300",
	                    "\u0399\u0308\u0300", "\u03B9\u0308\u0300",
	                    "\u1FBE\u0308\u0300" },
	            new String[] { "\u03B9\u0308\u0301", "\u0345\u0308\u0301",
	                    "\u0399\u0308\u0301", "\u03B9\u0308\u0301",
	                    "\u1FBE\u0308\u0301" },
	            new String[] { "\u03B9\u0308\u0342", "\u0345\u0308\u0342",
	                    "\u0399\u0308\u0342", "\u03B9\u0308\u0342",
	                    "\u1FBE\u0308\u0342" },
	            new String[] { "\u03B9\u0342", "\u0345\u0342", "\u0399\u0342",
	                    "\u03B9\u0342", "\u1FBE\u0342" },
	            new String[] { "\u03C1\u0313", "\u03A1\u0313", "\u03C1\u0313",
	                    "\u03F1\u0313" },
	            new String[] { "\u03C5\u0308\u0300", "\u03A5\u0308\u0300",
	                    "\u03C5\u0308\u0300" },
	            new String[] { "\u03C5\u0308\u0301", "\u03A5\u0308\u0301",
	                    "\u03C5\u0308\u0301" },
	            new String[] { "\u03C5\u0308\u0342", "\u03A5\u0308\u0342",
	                    "\u03C5\u0308\u0342" },
	            new String[] { "\u03C5\u0313", "\u03A5\u0313", "\u03C5\u0313" },
	            new String[] { "\u03C5\u0313\u0300", "\u03A5\u0313\u0300",
	                    "\u03C5\u0313\u0300", "\u1F50\u0300" },
	            new String[] { "\u03C5\u0313\u0301", "\u03A5\u0313\u0301",
	                    "\u03C5\u0313\u0301", "\u1F50\u0301" },
	            new String[] { "\u03C5\u0313\u0342", "\u03A5\u0313\u0342",
	                    "\u03C5\u0313\u0342", "\u1F50\u0342" },
	            new String[] { "\u03C5\u0342", "\u03A5\u0342", "\u03C5\u0342" },
	            new String[] { "\u03C9\u0342", "\u03A9\u0342", "\u03C9\u0342",
	                    "\u2126\u0342" },
	            new String[] { "\u03C9\u0342\u03B9", "\u03A9\u0342\u0345",
	                    "\u03A9\u0342\u0399", "\u03A9\u0342\u03B9",
	                    "\u03A9\u0342\u1FBE", "\u03C9\u0342\u0345",
	                    "\u03C9\u0342\u0399", "\u03C9\u0342\u03B9",
	                    "\u03C9\u0342\u1FBE", "\u1FF6\u0345", "\u1FF6\u0399",
	                    "\u1FF6\u03B9", "\u1FF6\u1FBE", "\u2126\u0342\u0345",
	                    "\u2126\u0342\u0399", "\u2126\u0342\u03B9",
	                    "\u2126\u0342\u1FBE" },
	            new String[] { "\u03C9\u03B9", "\u03A9\u0345", "\u03A9\u0399",
	                    "\u03A9\u03B9", "\u03A9\u1FBE", "\u03C9\u0345",
	                    "\u03C9\u0399", "\u03C9\u03B9", "\u03C9\u1FBE",
	                    "\u2126\u0345", "\u2126\u0399", "\u2126\u03B9",
	                    "\u2126\u1FBE" },
	            new String[] { "\u03CE\u03B9", "\u038F\u0345", "\u038F\u0399",
	                    "\u038F\u03B9", "\u038F\u1FBE", "\u03CE\u0345",
	                    "\u03CE\u0399", "\u03CE\u03B9", "\u03CE\u1FBE" },
	            new String[] { "\u0565\u0582", "\u0535\u0552", "\u0535\u0582",
	                    "\u0565\u0552", "\u0565\u0582" },
	            new String[] { "\u0574\u0565", "\u0544\u0535", "\u0544\u0565",
	                    "\u0574\u0535", "\u0574\u0565" },
	            new String[] { "\u0574\u056B", "\u0544\u053B", "\u0544\u056B",
	                    "\u0574\u053B", "\u0574\u056B" },
	            new String[] { "\u0574\u056D", "\u0544\u053D", "\u0544\u056D",
	                    "\u0574\u053D", "\u0574\u056D" },
	            new String[] { "\u0574\u0576", "\u0544\u0546", "\u0544\u0576",
	                    "\u0574\u0546", "\u0574\u0576" },
	            new String[] { "\u057E\u0576", "\u054E\u0546", "\u054E\u0576",
	                    "\u057E\u0546", "\u057E\u0576" },
	            new String[] { "\u1F00\u03B9", "\u1F00\u0345", "\u1F00\u0399",
	                    "\u1F00\u03B9", "\u1F00\u1FBE", "\u1F08\u0345",
	                    "\u1F08\u0399", "\u1F08\u03B9", "\u1F08\u1FBE" },
	            new String[] { "\u1F01\u03B9", "\u1F01\u0345", "\u1F01\u0399",
	                    "\u1F01\u03B9", "\u1F01\u1FBE", "\u1F09\u0345",
	                    "\u1F09\u0399", "\u1F09\u03B9", "\u1F09\u1FBE" },
	            new String[] { "\u1F02\u03B9", "\u1F02\u0345", "\u1F02\u0399",
	                    "\u1F02\u03B9", "\u1F02\u1FBE", "\u1F0A\u0345",
	                    "\u1F0A\u0399", "\u1F0A\u03B9", "\u1F0A\u1FBE" },
	            new String[] { "\u1F03\u03B9", "\u1F03\u0345", "\u1F03\u0399",
	                    "\u1F03\u03B9", "\u1F03\u1FBE", "\u1F0B\u0345",
	                    "\u1F0B\u0399", "\u1F0B\u03B9", "\u1F0B\u1FBE" },
	            new String[] { "\u1F04\u03B9", "\u1F04\u0345", "\u1F04\u0399",
	                    "\u1F04\u03B9", "\u1F04\u1FBE", "\u1F0C\u0345",
	                    "\u1F0C\u0399", "\u1F0C\u03B9", "\u1F0C\u1FBE" },
	            new String[] { "\u1F05\u03B9", "\u1F05\u0345", "\u1F05\u0399",
	                    "\u1F05\u03B9", "\u1F05\u1FBE", "\u1F0D\u0345",
	                    "\u1F0D\u0399", "\u1F0D\u03B9", "\u1F0D\u1FBE" },
	            new String[] { "\u1F06\u03B9", "\u1F06\u0345", "\u1F06\u0399",
	                    "\u1F06\u03B9", "\u1F06\u1FBE", "\u1F0E\u0345",
	                    "\u1F0E\u0399", "\u1F0E\u03B9", "\u1F0E\u1FBE" },
	            new String[] { "\u1F07\u03B9", "\u1F07\u0345", "\u1F07\u0399",
	                    "\u1F07\u03B9", "\u1F07\u1FBE", "\u1F0F\u0345",
	                    "\u1F0F\u0399", "\u1F0F\u03B9", "\u1F0F\u1FBE" },
	            new String[] { "\u1F20\u03B9", "\u1F20\u0345", "\u1F20\u0399",
	                    "\u1F20\u03B9", "\u1F20\u1FBE", "\u1F28\u0345",
	                    "\u1F28\u0399", "\u1F28\u03B9", "\u1F28\u1FBE" },
	            new String[] { "\u1F21\u03B9", "\u1F21\u0345", "\u1F21\u0399",
	                    "\u1F21\u03B9", "\u1F21\u1FBE", "\u1F29\u0345",
	                    "\u1F29\u0399", "\u1F29\u03B9", "\u1F29\u1FBE" },
	            new String[] { "\u1F22\u03B9", "\u1F22\u0345", "\u1F22\u0399",
	                    "\u1F22\u03B9", "\u1F22\u1FBE", "\u1F2A\u0345",
	                    "\u1F2A\u0399", "\u1F2A\u03B9", "\u1F2A\u1FBE" },
	            new String[] { "\u1F23\u03B9", "\u1F23\u0345", "\u1F23\u0399",
	                    "\u1F23\u03B9", "\u1F23\u1FBE", "\u1F2B\u0345",
	                    "\u1F2B\u0399", "\u1F2B\u03B9", "\u1F2B\u1FBE" },
	            new String[] { "\u1F24\u03B9", "\u1F24\u0345", "\u1F24\u0399",
	                    "\u1F24\u03B9", "\u1F24\u1FBE", "\u1F2C\u0345",
	                    "\u1F2C\u0399", "\u1F2C\u03B9", "\u1F2C\u1FBE" },
	            new String[] { "\u1F25\u03B9", "\u1F25\u0345", "\u1F25\u0399",
	                    "\u1F25\u03B9", "\u1F25\u1FBE", "\u1F2D\u0345",
	                    "\u1F2D\u0399", "\u1F2D\u03B9", "\u1F2D\u1FBE" },
	            new String[] { "\u1F26\u03B9", "\u1F26\u0345", "\u1F26\u0399",
	                    "\u1F26\u03B9", "\u1F26\u1FBE", "\u1F2E\u0345",
	                    "\u1F2E\u0399", "\u1F2E\u03B9", "\u1F2E\u1FBE" },
	            new String[] { "\u1F27\u03B9", "\u1F27\u0345", "\u1F27\u0399",
	                    "\u1F27\u03B9", "\u1F27\u1FBE", "\u1F2F\u0345",
	                    "\u1F2F\u0399", "\u1F2F\u03B9", "\u1F2F\u1FBE" },
	            new String[] { "\u1F60\u03B9", "\u1F60\u0345", "\u1F60\u0399",
	                    "\u1F60\u03B9", "\u1F60\u1FBE", "\u1F68\u0345",
	                    "\u1F68\u0399", "\u1F68\u03B9", "\u1F68\u1FBE" },
	            new String[] { "\u1F61\u03B9", "\u1F61\u0345", "\u1F61\u0399",
	                    "\u1F61\u03B9", "\u1F61\u1FBE", "\u1F69\u0345",
	                    "\u1F69\u0399", "\u1F69\u03B9", "\u1F69\u1FBE" },
	            new String[] { "\u1F62\u03B9", "\u1F62\u0345", "\u1F62\u0399",
	                    "\u1F62\u03B9", "\u1F62\u1FBE", "\u1F6A\u0345",
	                    "\u1F6A\u0399", "\u1F6A\u03B9", "\u1F6A\u1FBE" },
	            new String[] { "\u1F63\u03B9", "\u1F63\u0345", "\u1F63\u0399",
	                    "\u1F63\u03B9", "\u1F63\u1FBE", "\u1F6B\u0345",
	                    "\u1F6B\u0399", "\u1F6B\u03B9", "\u1F6B\u1FBE" },
	            new String[] { "\u1F64\u03B9", "\u1F64\u0345", "\u1F64\u0399",
	                    "\u1F64\u03B9", "\u1F64\u1FBE", "\u1F6C\u0345",
	                    "\u1F6C\u0399", "\u1F6C\u03B9", "\u1F6C\u1FBE" },
	            new String[] { "\u1F65\u03B9", "\u1F65\u0345", "\u1F65\u0399",
	                    "\u1F65\u03B9", "\u1F65\u1FBE", "\u1F6D\u0345",
	                    "\u1F6D\u0399", "\u1F6D\u03B9", "\u1F6D\u1FBE" },
	            new String[] { "\u1F66\u03B9", "\u1F66\u0345", "\u1F66\u0399",
	                    "\u1F66\u03B9", "\u1F66\u1FBE", "\u1F6E\u0345",
	                    "\u1F6E\u0399", "\u1F6E\u03B9", "\u1F6E\u1FBE" },
	            new String[] { "\u1F67\u03B9", "\u1F67\u0345", "\u1F67\u0399",
	                    "\u1F67\u03B9", "\u1F67\u1FBE", "\u1F6F\u0345",
	                    "\u1F6F\u0399", "\u1F6F\u03B9", "\u1F6F\u1FBE" },
	            new String[] { "\u1F70\u03B9", "\u1F70\u0345", "\u1F70\u0399",
	                    "\u1F70\u03B9", "\u1F70\u1FBE", "\u1FBA\u0345",
	                    "\u1FBA\u0399", "\u1FBA\u03B9", "\u1FBA\u1FBE" },
	            new String[] { "\u1F74\u03B9", "\u1F74\u0345", "\u1F74\u0399",
	                    "\u1F74\u03B9", "\u1F74\u1FBE", "\u1FCA\u0345",
	                    "\u1FCA\u0399", "\u1FCA\u03B9", "\u1FCA\u1FBE" },
	            new String[] { "\u1F7C\u03B9", "\u1F7C\u0345", "\u1F7C\u0399",
	                    "\u1F7C\u03B9", "\u1F7C\u1FBE", "\u1FFA\u0345",
	                    "\u1FFA\u0399", "\u1FFA\u03B9", "\u1FFA\u1FBE" } };
	
	    // this initializes the data used to generated the case-equivalents
	
	    // pieces that we will put together
	    // is not changed during iteration
	    private int count;
	
	    private String[][] variants;
	
	    // state information, changes during iteration
	    private bool done;
	
	    private int[] counts;
	
	    // internal buffer for efficiency
	    private StringBuilder nextBuffer;
	
	    // ========================
	
	    /// <summary>
	    /// Reset to different source. Once reset, the iteration starts from the
	    /// beginning.
	    /// </summary>
	    ///
	    /// <param name="source">The string to get case variants for</param>
	    public void Reset(String source) {
	
	        // allocate arrays to store pieces
	        // using length might be slightly too long, but we don't care much
	
	        counts = new int[source.Length];
	        variants = new String[source.Length][];
	
	        // walk through the source, and break up into pieces
	        // each piece becomes an array of equivalent values
	        // TODO: could optimized this later to coalesce all single string pieces
	
	        String piece = null;
	        count = 0;
	        for (int i = 0; i < source.Length; i += piece.Length) {
	
	            // find *longest* matching piece
	            String caseFold = null;
	
	            if (GENERATE) {
	                // do exactly one CP
	                piece = IBM.ICU.Text.UTF16.ValueOf(source, i);
	                caseFold = (String) ILOG.J2CsMapping.Collections.Collections.Get(toCaseFold,piece);
	            } else {
	                int max = i + maxLength;
	                if (max > source.Length)
	                    max = source.Length;
	                for (int j = max; j > i; --j) {
	                    piece = source.Substring(i,(j)-(i));
	                    caseFold = (String) ILOG.J2CsMapping.Collections.Collections.Get(toCaseFold,piece);
	                    if (caseFold != null)
	                        break;
	                }
	            }
	
	            // if we fail, pick one code point
	            if (caseFold == null) {
	                piece = IBM.ICU.Text.UTF16.ValueOf(source, i);
	                variants[count++] = new String[] { piece }; // single item
	                                                            // string
	            } else {
	                variants[count++] = (String[]) ILOG.J2CsMapping.Collections.Collections.Get(fromCaseFold,caseFold);
	            }
	        }
	        Reset();
	    }
	
	    /// <summary>
	    /// Restart the iteration from the beginning, but with same source
	    /// </summary>
	    ///
	    public void Reset() {
	        done = false;
	        for (int i = 0; i < count; ++i) {
	            counts[i] = 0;
	        }
	    }
	
	    /// <summary>
	    /// Iterates through the case variants.
	    /// </summary>
	    ///
	    /// <returns>next case variant. Each variant will case-fold to the same value
	    /// as the source will. When the iteration is done, null is returned.</returns>
	    public String Next() {
	
	        if (done)
	            return null;
	        int i;
	
	        // TODO Optimize so we keep the piece before and after the current
	        // position
	        // so we don't have so much concatenation
	
	        // get the result, a concatenation
	
	        nextBuffer.Length=0;
	        for (i = 0; i < count; ++i) {
	            nextBuffer.Append(variants[i][counts[i]]);
	        }
	
	        // find the next right set of pieces to concatenate
	
	        for (i = count - 1; i >= 0; --i) {
	            counts[i]++;
	            if (counts[i] < variants[i].Length)
	                break;
	            counts[i] = 0;
	        }
	
	        // if we go too far, bail
	
	        if (i < 0) {
	            done = true;
	        }
	
	        return nextBuffer.ToString();
	    }
	
	    /// <summary>
	    /// Temporary test, just to see how the stuff works.
	    /// </summary>
	    ///
	    static public void Main(String[] args) {
	        String[] testCases = { "fiss", "h\u03a3" };
	        CaseIterator ci = new CaseIterator();
	
	        for (int i = 0; i < testCases.Length; ++i) {
	            String item = testCases[i];
	            System.Console.Out.WriteLine();
	            System.Console.Out.WriteLine("Testing: " + toName.Transliterate(item));
	            System.Console.Out.WriteLine();
	            ci.Reset(item);
	            int count_0 = 0;
	            for (String temp = ci.Next(); temp != null; temp = ci.Next()) {
	                System.Console.Out.WriteLine(toName.Transliterate(temp));
	                count_0++;
	            }
	            System.Console.Out.WriteLine("Total: " + count_0);
	        }
	
	        // generate a list of all caseless characters -- characters whose
	        // case closure is themselves.
	
	        UnicodeSet caseless = new UnicodeSet();
	
	        for (int i_1 = 0; i_1 <= 0x10FFFF; ++i_1) {
	            String cp = IBM.ICU.Text.UTF16.ValueOf(i_1);
	            ci.Reset(cp);
	            int count_2 = 0;
	            String fold = null;
	            for (String temp_3 = ci.Next(); temp_3 != null; temp_3 = ci.Next()) {
	                fold = temp_3;
	                if (++count_2 > 1)
	                    break;
	            }
	            if (count_2 == 1 && fold.Equals(cp)) {
	                caseless.Add(i_1);
	            }
	        }
	
	        System.Console.Out.WriteLine("caseless = " + caseless.ToPattern(true));
	
	        UnicodeSet not_lc = new UnicodeSet("[:^lc:]");
	
	        UnicodeSet a = new UnicodeSet();
	        a.Set(not_lc);
	        a.RemoveAll(caseless);
	        System.Console.Out.WriteLine("[:^lc:] - caseless = " + a.ToPattern(true));
	
	        a.Set(caseless);
	        a.RemoveAll(not_lc);
	        System.Console.Out.WriteLine("caseless - [:^lc:] = " + a.ToPattern(true));
	    }
	
	    static CaseIterator() {
	            if (!GENERATE) {
	                for (int i = 0; i < exceptionList.Length; ++i) {
	                    String[] exception = exceptionList[i];
	                    ILOG.J2CsMapping.Collections.ISet s = new HashedSet();
	                    for (int j = 0; j < exception.Length; ++j) {
	                        ILOG.J2CsMapping.Collections.Generics.Collections.Add(s,exception[j]);
	                    }
	                    ILOG.J2CsMapping.Collections.Collections.Put(fromCaseFold,exception[0],s);
	                }
	            }
	            bool defaultmapping = true;
	            for (int i = 0; i <= 0x10FFFF; ++i) {
	                int cat = IBM.ICU.Lang.UCharacter.GetType(i);
                    if (cat == ILOG.J2CsMapping.Util.Character.UNASSIGNED
                            || cat == ILOG.J2CsMapping.Util.Character.PRIVATE_USE)
	                    continue;
	                String cp = IBM.ICU.Text.UTF16.ValueOf(i);
	                String mapped = IBM.ICU.Lang.UCharacter.FoldCase(cp,
	                        defaultmapping);
	                if (mapped.Equals(cp))
	                    continue;
	                if (maxLength < mapped.Length)
	                    maxLength = mapped.Length;
	                ILOG.J2CsMapping.Collections.ISet s = (ISet) ILOG.J2CsMapping.Collections.Collections.Get(fromCaseFold,mapped);
	                if (s == null) {
	                    s = new HashedSet();
	                    ILOG.J2CsMapping.Collections.Generics.Collections.Add(s,mapped);
	                    ILOG.J2CsMapping.Collections.Collections.Put(fromCaseFold,mapped,s);
	                }
	                ILOG.J2CsMapping.Collections.Generics.Collections.Add(s,cp);
	                ILOG.J2CsMapping.Collections.Collections.Put(toCaseFold,cp,mapped);
	                ILOG.J2CsMapping.Collections.Collections.Put(toCaseFold,mapped,mapped);
	            }
	            if (DUMP) {
	                System.Console.Out.WriteLine("maxLength = " + maxLength);
	                System.Console.Out.WriteLine("\nfromCaseFold:");
	                IIterator it = new ILOG.J2CsMapping.Collections.IteratorAdapter(new ILOG.J2CsMapping.Collections.ListSet(fromCaseFold.Keys).GetEnumerator());
	                while (it.HasNext()) {
	                    Object key = it.Next();
	                    System.Console.Out.Write(" "
	                            + toHex2.Transliterate((String) key) + ": ");
	                    ILOG.J2CsMapping.Collections.ISet s = (ISet) ILOG.J2CsMapping.Collections.Collections.Get(fromCaseFold,key);
	                    IIterator it2 = new ILOG.J2CsMapping.Collections.IteratorAdapter(s.GetEnumerator());
	                    bool first = true;
	                    while (it2.HasNext()) {
	                        if (first) {
	                            first = false;
	                        } else {
	                            System.Console.Out.Write(", ");
	                        }
	                        System.Console.Out.Write(toHex2
	                                .Transliterate((String) it2.Next()));
	                    }
	                    System.Console.Out.WriteLine("");
	                }
	                System.Console.Out.WriteLine("\ntoCaseFold:");
	                it = new ILOG.J2CsMapping.Collections.IteratorAdapter(new ILOG.J2CsMapping.Collections.ListSet(toCaseFold.Keys).GetEnumerator());
	                while (it.HasNext()) {
	                    String key = (String) it.Next();
	                    String value_ren = (String) ILOG.J2CsMapping.Collections.Collections.Get(toCaseFold,key);
	                    System.Console.Out.WriteLine(" " + toHex2.Transliterate(key)
	                            + ": " + toHex2.Transliterate(value_ren));
	                }
	            }
	            IDictionary fromCaseFold2 = new Hashtable();
	            IIterator it3 = new ILOG.J2CsMapping.Collections.IteratorAdapter(new ILOG.J2CsMapping.Collections.ListSet(fromCaseFold.Keys).GetEnumerator());
	            while (it3.HasNext()) {
	                Object key = it3.Next();
	                ILOG.J2CsMapping.Collections.ISet s = (ISet) ILOG.J2CsMapping.Collections.Collections.Get(fromCaseFold,key);
	                String[] temp = new String[s.Count];
	                ILOG.J2CsMapping.Collections.Generics.Collections.ToArray(s,temp);
	                ILOG.J2CsMapping.Collections.Collections.Put(fromCaseFold2,key,temp);
	            }
	            fromCaseFold = fromCaseFold2;
	            if (GENERATE) {
	                ILOG.J2CsMapping.Collections.ISet multichars = new SortedSet();
	                it3 = new ILOG.J2CsMapping.Collections.IteratorAdapter(new ILOG.J2CsMapping.Collections.ListSet(fromCaseFold.Keys).GetEnumerator());
	                while (it3.HasNext()) {
	                    String key = (String) it3.Next();
	                    if (IBM.ICU.Text.UTF16.CountCodePoint(key) < 2)
	                        continue;
	                    ILOG.J2CsMapping.Collections.Generics.Collections.Add(multichars,key);
	                }
	                CaseIterator ci = new CaseIterator();
	                it3 = new ILOG.J2CsMapping.Collections.IteratorAdapter(multichars.GetEnumerator());
	                while (it3.HasNext()) {
	                    String key = (String) it3.Next();
	                    ILOG.J2CsMapping.Collections.ISet partialClosure = new SortedSet();
	                    ILOG.J2CsMapping.Collections.Generics.Collections.Add(partialClosure,key);
	                    if (IBM.ICU.Text.UTF16.CountCodePoint(key) > 2) {
	                        IIterator multiIt2 = new ILOG.J2CsMapping.Collections.IteratorAdapter(multichars.GetEnumerator());
	                        while (multiIt2.HasNext()) {
	                            String otherKey = (String) multiIt2.Next();
	                            if (otherKey.Length >= key.Length)
	                                continue;
	                            int pos = -1;
	                            while (true) {
	                                pos = ILOG.J2CsMapping.Util.StringUtil.IndexOf(key,otherKey,pos + 1);
	                                if (pos < 0)
	                                    break;
	                                int endPos = pos + otherKey.Length;
	                                String[] choices = (String[]) ILOG.J2CsMapping.Collections.Collections.Get(fromCaseFold,otherKey);
	                                for (int ii = 0; ii < choices.Length; ++ii) {
	                                    String patchwork = key.Substring(0,(pos)-(0))
	                                            + choices[ii] + key.Substring(endPos);
	                                    ILOG.J2CsMapping.Collections.Generics.Collections.Add(partialClosure,patchwork);
	                                }
	                            }
	                        }
	                    }
	                    ILOG.J2CsMapping.Collections.ISet closure = new SortedSet();
	                    IIterator partialIt = new ILOG.J2CsMapping.Collections.IteratorAdapter(partialClosure.GetEnumerator());
	                    while (partialIt.HasNext()) {
	                        String key2 = (String) partialIt.Next();
	                        ci.Reset(key2);
	                        for (String temp = ci.Next(); temp != null; temp = ci
	                                .Next()) {
	                            ILOG.J2CsMapping.Collections.Generics.Collections.Add(closure,temp);
	                        }
	                    }
	                    IIterator it2 = new ILOG.J2CsMapping.Collections.IteratorAdapter(closure.GetEnumerator());
	                    System.Console.Out.WriteLine("\t// "
	                            + toName.Transliterate(key));
	                    System.Console.Out.Write("\t{\"" + toHex.Transliterate(key)
	                            + "\",");
	                    while (it2.HasNext()) {
	                        String item = (String) it2.Next();
	                        System.Console.Out.Write("\"" + toHex.Transliterate(item)
	                                + "\",");
	                    }
	                    System.Console.Out.WriteLine("},");
	                }
	            }
	        }
	}
}
