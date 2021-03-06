/*
 ******************************************************************************
 * Copyright (C) 2007, International Business Machines Corporation and   *
 * others. All Rights Reserved.                                               *
 ******************************************************************************
 */

// --------------------------------------------------------------------------------------------------
// This file was automatically generated by J2CS Translator (http://j2cstranslator.sourceforge.net/). 
// Version 1.3.6.20101125_01     
// 12/8/10 10:24 AM    
// ${CustomMessageForDisclaimer}                                                                             
// --------------------------------------------------------------------------------------------------
 namespace IBM.ICU.Impl.Duration.Impl {
	
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Globalization;
	using System.IO;
	using System.Runtime.CompilerServices;
     using ILOG.J2CsMapping.Util;
	
	public class Utils {
        public static Locale LocaleFromString(String s)
        {
	        String language = s;
	        String region = "";
	        String variant = "";
	
	        int x = language.IndexOf("_");
	        if (x != -1) {
	            region = language.Substring(x + 1);
	            language = language.Substring(0,(x)-(0));
	        }
	        x = region.IndexOf("_");
	        if (x != -1) {
	            variant = region.Substring(x + 1);
	            region = region.Substring(0,(x)-(0));
	        }
	        return new Locale(language, region, variant);
	    }
	
	    /*
	     * public static <T> T[] arraycopy(T[] src) { T[] result =
	     * (T[])Array.newInstance(src.getClass().getComponentType(), src.length); //
	     * can we do this without casting? for (int i = 0; i < src.length; ++i) {
	     * result[i] = src[i]; } return result; }
	     */
	
	    /// <summary>
	    /// Interesting features of chinese numbers: - Each digit is followed by a
	    /// unit symbol (10's, 100's, 1000's). - Units repeat in levels of 10,000,
	    /// there are symbols for each level too (except 1's). - The digit 2 has a
	    /// special form before the 10 symbol and at the end of the number. - If the
	    /// first digit in the number is 1 and its unit is 10, the 1 is omitted. -
	    /// Sequences of 0 digits and their units are replaced by a single 0 and no
	    /// unit. - If there are two such sequences of 0 digits in a level (1000's
	    /// and 10's), the 1000's 0 is also omitted. - The 1000's 0 is also omitted
	    /// in alternating levels, such that it is omitted in the rightmost level
	    /// with a 10's 0, or if none, in the rightmost level. - Level symbols are
	    /// omitted if all of their units are omitted
	    /// </summary>
	    ///
	    public static String ChineseNumber(long n, Utils.ChineseDigits  zh) {
	        if (n < 0) {
	            n = -n;
	        }
	        if (n <= 10) {
	            if (n == 2) {
	                return zh.liang.ToString();
	            }
	            return zh.digits[(int) n].ToString();
	        }
	
	        // 9223372036854775807
	        char[] buf = new char[40]; // as long as we get, and actually we can't
	                                   // get this high, no units past zhao
	        char[] digits = n.ToString().ToCharArray();
	
	        // first, generate all the digits in place
	        // convert runs of zeros into a single zero, but keep places
	        //
	        bool inZero = true; // true if we should zap zeros in this block,
	                               // resets at start of block
	        bool forcedZero = false; // true if we have a 0 in tens's place
	        int x = buf.Length;
	        for (int i = digits.Length, u = -1, l = -1; --i >= 0;) {
	            if (u == -1) {
	                if (l != -1) {
	                    buf[--x] = zh.levels[l];
	                    inZero = true;
	                    forcedZero = false;
	                }
	                ++u;
	            } else {
	                buf[--x] = zh.units[u++];
	                if (u == 3) {
	                    u = -1;
	                    ++l;
	                }
	            }
	            int d = digits[i] - '0';
	            if (d == 0) {
	                if (x < buf.Length - 1 && u != 0) {
	                    buf[x] = '*';
	                }
	                if (inZero || forcedZero) {
	                    buf[--x] = '*';
	                } else {
	                    buf[--x] = zh.digits[0];
	                    inZero = true;
	                    forcedZero = u == 1;
	                }
	            } else {
	                inZero = false;
	                buf[--x] = zh.digits[d];
	            }
	        }
	
	        // scanning from right, find first required 'ling'
	        // we only care if n > 101,0000 as this is the first case where
	        // it might shift. remove optional lings in alternating blocks.
	        if (n > 1000000) {
	            bool last = true;
	            int i_0 = buf.Length - 3;
	            do {
	                if (buf[i_0] == '0') {
	                    break;
	                }
	                i_0 -= 8;
	                last = !last;
	            } while (i_0 > x);
	
	            i_0 = buf.Length - 7;
	            do {
	                if (buf[i_0] == zh.digits[0] && !last) {
	                    buf[i_0] = '*';
	                }
	                i_0 -= 8;
	                last = !last;
	            } while (i_0 > x);
	
	            // remove levels for empty blocks
	            if (n >= 100000000) {
	                i_0 = buf.Length - 8;
	                do {
	                    bool empty = true;
	                    for (int j = i_0 - 1, e = Math.Max(x - 1,i_0 - 8); j > e; --j) {
	                        if (buf[j] != '*') {
	                            empty = false;
	                            break;
	                        }
	                    }
	                    if (empty) {
	                        if (buf[i_0 + 1] != '*' && buf[i_0 + 1] != zh.digits[0]) {
	                            buf[i_0] = zh.digits[0];
	                        } else {
	                            buf[i_0] = '*';
	                        }
	                    }
	                    i_0 -= 8;
	                } while (i_0 > x);
	            }
	        }
	
	        // replace er by liang except before or after shi or after ling
	        for (int i_1 = x; i_1 < buf.Length; ++i_1) {
	            if (buf[i_1] != zh.digits[2])
	                continue;
	            if (i_1 < buf.Length - 1 && buf[i_1 + 1] == zh.units[0])
	                continue;
	            if (i_1 > x
	                    && (buf[i_1 - 1] == zh.units[0] || buf[i_1 - 1] == zh.digits[0] || buf[i_1 - 1] == '*'))
	                continue;
	
	            buf[i_1] = zh.liang;
	        }
	
	        // eliminate leading 1 if following unit is shi
	        if (buf[x] == zh.digits[1] && (zh.ko || buf[x + 1] == zh.units[0])) {
	            ++x;
	        }
	
	        // now, compress out the '*'
	        int w = x;
	        for (int r = x; r < buf.Length; ++r) {
	            if (buf[r] != '*') {
	                buf[w++] = buf[r];
	            }
	        }
	        return new String(buf, x, w - x);
	    }
	
	    public static void Main(String[] args) {
	        for (int i = 0; i < args.Length; ++i) {
	            String arg = args[i];
	            System.Console.Out.Write(arg);
	            System.Console.Out.Write(" > ");
	            long n = ((Int64 )Int64.Parse(arg,System.Globalization.NumberStyles.Integer));
	            System.Console.Out.WriteLine(ChineseNumber(n, IBM.ICU.Impl.Duration.Impl.Utils.ChineseDigits.DEBUG));
	        }
	    }
	
	    public class ChineseDigits {
	        internal readonly char[] digits;
	
	        internal readonly char[] units;
	
	        internal readonly char[] levels;
	
	        internal readonly char liang;
	
	        internal readonly bool ko;
	
	        internal ChineseDigits(String digits_0, String units_1, String levels_2, char liang_3,
	                bool ko_4) {
	            this.digits = digits_0.ToCharArray();
	            this.units = units_1.ToCharArray();
	            this.levels = levels_2.ToCharArray();
	            this.liang = liang_3;
	            this.ko = ko_4;
	        }
	
	        public static readonly Utils.ChineseDigits  DEBUG = new Utils.ChineseDigits (
	                "0123456789s", "sbq", "WYZ", 'L', false);
	
	        public static readonly Utils.ChineseDigits  TRADITIONAL = new Utils.ChineseDigits (
	                "\u96f6\u4e00\u4e8c\u4e09\u56db\u4e94\u516d\u4e03\u516b\u4e5d\u5341", // to
	                                                                                      // shi
	                "\u5341\u767e\u5343", // shi, bai, qian
	                "\u842c\u5104\u5146", // wan, yi, zhao
	                '\u5169', false); // liang
	
	        public static readonly Utils.ChineseDigits  SIMPLIFIED = new Utils.ChineseDigits (
	                "\u96f6\u4e00\u4e8c\u4e09\u56db\u4e94\u516d\u4e03\u516b\u4e5d\u5341", // to
	                                                                                      // shi
	                "\u5341\u767e\u5343", // shi, bai, qian
	                "\u4e07\u4ebf\u5146", // wan, yi, zhao
	                '\u4e24', false); // liang
	
	        // no 1 before first unit no matter what it is
	        // not sure if there are 'ling' units
	        public static readonly Utils.ChineseDigits  KOREAN = new Utils.ChineseDigits (
	                "\uc601\uc77c\uc774\uc0bc\uc0ac\uc624\uc721\uce60\ud314\uad6c\uc2ed", // to
	                                                                                      // ten
	                "\uc2ed\ubc31\ucc9c", // 10, 100, 1000
	                "\ub9cc\uc5b5?", // 10^4, 10^8, 10^12
	                '\uc774', true);
	    }
	}
}
