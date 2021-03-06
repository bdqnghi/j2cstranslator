/*
 **********************************************************************
 *   Copyright (c) 2001-2004, International Business Machines
 *   Corporation and others.  All Rights Reserved.
 **********************************************************************
 *   Date        Name        Description
 *   11/19/2001  aliu        Creation.
 **********************************************************************
 */
// --------------------------------------------------------------------------------------------------
// This file was automatically generated by J2CS Translator (http://j2cstranslator.sourceforge.net/). 
// Version 1.3.6.20101125_01     
// 12/2/10 11:30 AM    
// ${CustomMessageForDisclaimer}                                                                             
// --------------------------------------------------------------------------------------------------
 namespace IBM.ICU.Text {
	
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.IO;
	using System.Runtime.CompilerServices;
	using System.Text;
	
	
	/// <summary>
	/// A transliterator that converts Unicode characters to an escape form. Examples
	/// of escape forms are "U+4E01" and "&#x10FFFF;". Escape forms have a prefix and
	/// suffix, either of which may be empty, a radix, typically 16 or 10, a minimum
	/// digit count, typically 1, 4, or 8, and a boolean that specifies whether
	/// supplemental characters are handled as 32-bit code points or as two 16-bit
	/// code units. Most escape forms handle 32-bit code points, but some, such as
	/// the Java form, intentionally break them into two surrogate pairs, for
	/// backward compatibility.
	/// <p>
	/// Some escape forms actually have two different patterns, one for BMP
	/// characters (0..FFFF) and one for supplements (>FFFF). To handle this, a
	/// second EscapeTransliterator may be defined that specifies the pattern to be
	/// produced for supplementals. An example of a form that requires this is the C
	/// form, which uses "\\uFFFF" for BMP characters and "\\U0010FFFF" for
	/// supplementals.
	/// <p>
	/// This class is package private. It registers several standard variants with
	/// the system which are then accessed via their IDs.
	/// </summary>
	///
	internal class EscapeTransliterator : Transliterator {
	
	    public sealed class Anonymous_C6 : Transliterator.Factory {
	        public Transliterator GetInstance(String ID) {
	            return new EscapeTransliterator("Any-Hex/Unicode",
	                    "U+", "", 16, 4, true, null);
	        }
	    }
	
	    public sealed class Anonymous_C5 : Transliterator.Factory {
	        public Transliterator GetInstance(String ID) {
	            return new EscapeTransliterator("Any-Hex/Java", "\\u",
	                    "", 16, 4, false, null);
	        }
	    }
	
	    public sealed class Anonymous_C4 : Transliterator.Factory {
	        public Transliterator GetInstance(String ID) {
	            return new EscapeTransliterator("Any-Hex/C", "\\u", "",
	                    16, 4, true, new EscapeTransliterator("",
	                            "\\U", "", 16, 8, true, null));
	        }
	    }
	
	    public sealed class Anonymous_C3 : Transliterator.Factory {
	        public Transliterator GetInstance(String ID) {
	            return new EscapeTransliterator("Any-Hex/XML", "&#x",
	                    ";", 16, 1, true, null);
	        }
	    }
	
	    public sealed class Anonymous_C2 : Transliterator.Factory {
	        public Transliterator GetInstance(String ID) {
	            return new EscapeTransliterator("Any-Hex/XML10", "&#",
	                    ";", 10, 1, true, null);
	        }
	    }
	
	    public sealed class Anonymous_C1 : Transliterator.Factory {
	        public Transliterator GetInstance(String ID) {
	            return new EscapeTransliterator("Any-Hex/Perl", "\\x{",
	                    "}", 16, 1, true, null);
	        }
	    }
	
	    public sealed class Anonymous_C0 : Transliterator.Factory {
	        public Transliterator GetInstance(String ID) {
	            return new EscapeTransliterator("Any-Hex", "\\u", "", 16, 4,
	                    false, null);
	        }
	    }
	
	    /// <summary>
	    /// The prefix of the escape form; may be empty, but usually isn't. May not
	    /// be null.
	    /// </summary>
	    ///
	    private String prefix;
	
	    /// <summary>
	    /// The prefix of the escape form; often empty. May not be null.
	    /// </summary>
	    ///
	    private String suffix;
	
	    /// <summary>
	    /// The radix to display the number in. Typically 16 or 10. Must be in the
	    /// range 2 to 36.
	    /// </summary>
	    ///
	    private int radix;
	
	    /// <summary>
	    /// The minimum number of digits. Typically 1, 4, or 8. Values less than 1
	    /// are equivalent to 1.
	    /// </summary>
	    ///
	    private int minDigits;
	
	    /// <summary>
	    /// If true, supplementals are handled as 32-bit code points. If false, they
	    /// are handled as two 16-bit code units.
	    /// </summary>
	    ///
	    private bool grokSupplementals;
	
	    /// <summary>
	    /// The form to be used for supplementals. If this is null then the same form
	    /// is used for BMP characters and supplementals. If this is not null and if
	    /// grokSupplementals is true then the prefix, suffix, radix, and minDigits
	    /// of this object are used for supplementals.
	    /// </summary>
	    ///
	    private EscapeTransliterator supplementalHandler;
	
	    /// <summary>
	    /// Registers standard variants with the system. Called by Transliterator
	    /// during initialization.
	    /// </summary>
	    ///
	    static internal void Register() {
	        // Unicode: "U+10FFFF" hex, min=4, max=6
	        IBM.ICU.Text.Transliterator.RegisterFactory("Any-Hex/Unicode",
	                new EscapeTransliterator.Anonymous_C6 ());
	
	        // Java: "\\uFFFF" hex, min=4, max=4
	        IBM.ICU.Text.Transliterator.RegisterFactory("Any-Hex/Java",
	                new EscapeTransliterator.Anonymous_C5 ());
	
	        // C: "\\uFFFF" hex, min=4, max=4; \\U0010FFFF hex, min=8, max=8
	        IBM.ICU.Text.Transliterator.RegisterFactory("Any-Hex/C",
	                new EscapeTransliterator.Anonymous_C4 ());
	
	        // XML: "&#x10FFFF;" hex, min=1, max=6
	        IBM.ICU.Text.Transliterator.RegisterFactory("Any-Hex/XML",
	                new EscapeTransliterator.Anonymous_C3 ());
	
	        // XML10: "&1114111;" dec, min=1, max=7 (not really "Any-Hex")
	        IBM.ICU.Text.Transliterator.RegisterFactory("Any-Hex/XML10",
	                new EscapeTransliterator.Anonymous_C2 ());
	
	        // Perl: "\\x{263A}" hex, min=1, max=6
	        IBM.ICU.Text.Transliterator.RegisterFactory("Any-Hex/Perl",
	                new EscapeTransliterator.Anonymous_C1 ());
	
	        // Generic
	        IBM.ICU.Text.Transliterator.RegisterFactory("Any-Hex", new EscapeTransliterator.Anonymous_C0 ());
	    }
	
	    /// <summary>
	    /// Constructs an escape transliterator with the given ID and parameters. See
	    /// the class member documentation for details.
	    /// </summary>
	    ///
	    internal EscapeTransliterator(String ID, String prefix_0, String suffix_1, int radix_2,
	            int minDigits_3, bool grokSupplementals_4,
	            EscapeTransliterator supplementalHandler_5) : base(ID, null) {
	        this.prefix = prefix_0;
	        this.suffix = suffix_1;
	        this.radix = radix_2;
	        this.minDigits = minDigits_3;
	        this.grokSupplementals = grokSupplementals_4;
	        this.supplementalHandler = supplementalHandler_5;
	    }
	
	    /// <summary>
	    /// Implements <see cref="M:IBM.ICU.Text.Transliterator.HandleTransliterate(IBM.ICU.Text.Replaceable, null, System.Boolean)"/>.
	    /// </summary>
	    ///
	    protected internal override void HandleTransliterate(Replaceable text, Transliterator.Position  pos,
	            bool incremental) {
	        int start = pos.start;
	        int limit = pos.limit;
	
	        StringBuilder buf = new StringBuilder(prefix);
	        int prefixLen = prefix.Length;
	        bool redoPrefix = false;
	
	        while (start < limit) {
	            int c = (grokSupplementals) ? (int) (text.Char32At(start)) : (int) (text.CharAt(start));
	            int charLen = (grokSupplementals) ? IBM.ICU.Text.UTF16.GetCharCount(c) : 1;
	
	            if ((c & -65536) != 0 && supplementalHandler != null) {
	                buf.Length=0;
	                buf.Append(supplementalHandler.prefix);
	                IBM.ICU.Impl.Utility.AppendNumber(buf, c, supplementalHandler.radix,
	                        supplementalHandler.minDigits);
	                buf.Append(supplementalHandler.suffix);
	                redoPrefix = true;
	            } else {
	                if (redoPrefix) {
	                    buf.Length=0;
	                    buf.Append(prefix);
	                    redoPrefix = false;
	                } else {
	                    buf.Length=prefixLen;
	                }
	                IBM.ICU.Impl.Utility.AppendNumber(buf, c, radix, minDigits);
	                buf.Append(suffix);
	            }
	
	            text.Replace(start, start + charLen, buf.ToString());
	            start += buf.Length;
	            limit += buf.Length - charLen;
	        }
	
	        pos.contextLimit += limit - pos.limit;
	        pos.limit = limit;
	        pos.start = start;
	    }
	}
}
