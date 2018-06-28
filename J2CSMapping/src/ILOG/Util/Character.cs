// 
// J2CsMapping : runtime library for J2CsTranslator
// 
// Copyright (c) 2008-2010 Alexandre FAU.
// All rights reserved. This program and the accompanying materials
// are made available under the terms of the Eclipse Public License v1.0
// which accompanies this distribution, and is available at
// http://www.eclipse.org/legal/epl-v10.html
// Contributors:
//   Alexandre FAU (IBM)
//

using System;

namespace ILOG.J2CsMapping.Util
{
    /// <summary>
    /// Utility class for the mapping of System.Character with java.lang.Char
    /// </summary>
    public class Character
    {

        public class Subset
        {

            private String name;

            /**
             * Constructs a new <code>Subset</code> instance.
             *
             * @exception NullPointerException if name is <code>null</code>
             * @param  name  The name of this subset
             */
            protected Subset(String name)
            {
                if (name == null)
                {
                    throw new NullReferenceException("name");
                }
                this.name = name;
            }

            /**
             * Compares two <code>Subset</code> objects for equality.
             * This method returns <code>true</code> if and only if
             * <code>this</code> and the argument refer to the same
             * object; since this method is <code>final</code>, this
             * guarantee holds for all subclasses.
             */
            public override bool Equals(Object obj)
            {
                return (this == obj);
            }

            /**
             * Returns the standard hash code as defined by the
             * <code>{@link Object#hashCode}</code> method.  This method
             * is <code>final</code> in order to ensure that the
             * <code>equals</code> and <code>hashCode</code> methods will
             * be consistent in all subclasses.
             */
            public int HashCode()
            {
                return base.GetHashCode();
            }

            /**
             * Returns the name of this subset.
             */
            public override String ToString()
            {
                return name;
            }
        }

        /**
    * The minimum possible Character value.
    */
        public static char MIN_VALUE = '\u0000';

        /**
         * The maximum possible Character value.
         */
        public static char MAX_VALUE = '\uffff';

        /**
         * The minimum possible radix used for conversions between Characters and
         * integers.
         */
        public static int MIN_RADIX = 2;

        /**
         * The maximum possible radix used for conversions between Characters and
         * integers.
         */
        public static int MAX_RADIX = 36;

        /**
         * The <code>char</code> {@link Class} object.
         */
        public static Type TYPE = (Type)new char[0]
                .GetType().GetElementType();

        // Note: This can't be set to "char.class", since *that* is
        // defined to be "java.lang.Character.TYPE";

        /**
         * Unicode category constant Cn.
         */
        public static byte UNASSIGNED = 0;

        /**
         * Unicode category constant Lu.
         */
        public static byte UPPERCASE_LETTER = 1;

        /**
         * Unicode category constant Ll.
         */
        public static byte LOWERCASE_LETTER = 2;

        /**
         * Unicode category constant Lt.
         */
        public static byte TITLECASE_LETTER = 3;

        /**
         * Unicode category constant Lm.
         */
        public static byte MODIFIER_LETTER = 4;

        /**
         * Unicode category constant Lo.
         */
        public static byte OTHER_LETTER = 5;

        /**
         * Unicode category constant Mn.
         */
        public static byte NON_SPACING_MARK = 6;

        /**
         * Unicode category constant Me.
         */
        public static byte ENCLOSING_MARK = 7;

        /**
         * Unicode category constant Mc.
         */
        public static byte COMBINING_SPACING_MARK = 8;

        /**
         * Unicode category constant Nd.
         */
        public static byte DECIMAL_DIGIT_NUMBER = 9;

        /**
         * Unicode category constant Nl.
         */
        public static byte LETTER_NUMBER = 10;

        /**
         * Unicode category constant No.
         */
        public static byte OTHER_NUMBER = 11;

        /**
         * Unicode category constant Zs.
         */
        public static byte SPACE_SEPARATOR = 12;

        /**
         * Unicode category constant Zl.
         */
        public static byte LINE_SEPARATOR = 13;

        /**
         * Unicode category constant Zp.
         */
        public static byte PARAGRAPH_SEPARATOR = 14;

        /**
         * Unicode category constant Cc.
         */
        public static byte CONTROL = 15;

        /**
         * Unicode category constant Cf.
         */
        public static byte FORMAT = 16;

        /**
         * Unicode category constant Co.
         */
        public static byte PRIVATE_USE = 18;

        /**
         * Unicode category constant Cs.
         */
        public static byte SURROGATE = 19;

        /**
         * Unicode category constant Pd.
         */
        public static byte DASH_PUNCTUATION = 20;

        /**
         * Unicode category constant Ps.
         */
        public static byte START_PUNCTUATION = 21;

        /**
         * Unicode category constant Pe.
         */
        public static byte END_PUNCTUATION = 22;

        /**
         * Unicode category constant Pc.
         */
        public static byte CONNECTOR_PUNCTUATION = 23;

        /**
         * Unicode category constant Po.
         */
        public static byte OTHER_PUNCTUATION = 24;

        /**
         * Unicode category constant Sm.
         */
        public static byte MATH_SYMBOL = 25;

        /**
         * Unicode category constant Sc.
         */
        public static byte CURRENCY_SYMBOL = 26;

        /**
         * Unicode category constant Sk.
         */
        public static byte MODIFIER_SYMBOL = 27;

        /**
         * Unicode category constant So.
         */
        public static byte OTHER_SYMBOL = 28;

        /**
         * Unicode category constant Pi.
         * @since 1.4
         */
        public static byte INITIAL_QUOTE_PUNCTUATION = 29;

        /**
         * Unicode category constant Pf.
         * @since 1.4
         */
        public static byte FINAL_QUOTE_PUNCTUATION = 30;

        /**
         * Unicode bidirectional constant.
         * @since 1.4
         */
        public static sbyte DIRECTIONALITY_UNDEFINED = -1;

        /**
         * Unicode bidirectional constant L.
         * @since 1.4
         */
        public static byte DIRECTIONALITY_LEFT_TO_RIGHT = 0;

        /**
         * Unicode bidirectional constant R.
         * @since 1.4
         */
        public static byte DIRECTIONALITY_RIGHT_TO_LEFT = 1;

        /**
         * Unicode bidirectional constant AL.
         * @since 1.4
         */
        public static byte DIRECTIONALITY_RIGHT_TO_LEFT_ARABIC = 2;

        /**
         * Unicode bidirectional constant EN.
         * @since 1.4
         */
        public static byte DIRECTIONALITY_EUROPEAN_NUMBER = 3;

        /**
         * Unicode bidirectional constant ES.
         * @since 1.4
         */
        public static byte DIRECTIONALITY_EUROPEAN_NUMBER_SEPARATOR = 4;

        /**
         * Unicode bidirectional constant ET.
         * @since 1.4
         */
        public static byte DIRECTIONALITY_EUROPEAN_NUMBER_TERMINATOR = 5;

        /**
         * Unicode bidirectional constant AN.
         * @since 1.4
         */
        public static byte DIRECTIONALITY_ARABIC_NUMBER = 6;

        /**
         * Unicode bidirectional constant CS.
         * @since 1.4
         */
        public static byte DIRECTIONALITY_COMMON_NUMBER_SEPARATOR = 7;

        /**
         * Unicode bidirectional constant NSM.
         * @since 1.4
         */
        public static byte DIRECTIONALITY_NONSPACING_MARK = 8;

        /**
         * Unicode bidirectional constant BN.
         * @since 1.4
         */
        public static byte DIRECTIONALITY_BOUNDARY_NEUTRAL = 9;

        /**
         * Unicode bidirectional constant B.
         * @since 1.4
         */
        public static byte DIRECTIONALITY_PARAGRAPH_SEPARATOR = 10;

        /**
         * Unicode bidirectional constant S.
         * @since 1.4
         */
        public static byte DIRECTIONALITY_SEGMENT_SEPARATOR = 11;

        /**
         * Unicode bidirectional constant WS.
         * @since 1.4
         */
        public static byte DIRECTIONALITY_WHITESPACE = 12;

        /**
         * Unicode bidirectional constant ON.
         * @since 1.4
         */
        public static byte DIRECTIONALITY_OTHER_NEUTRALS = 13;

        /**
         * Unicode bidirectional constant LRE.
         * @since 1.4
         */
        public static byte DIRECTIONALITY_LEFT_TO_RIGHT_EMBEDDING = 14;

        /**
         * Unicode bidirectional constant LRO.
         * @since 1.4
         */
        public static byte DIRECTIONALITY_LEFT_TO_RIGHT_OVERRIDE = 15;

        /**
         * Unicode bidirectional constant RLE.
         * @since 1.4
         */
        public static byte DIRECTIONALITY_RIGHT_TO_LEFT_EMBEDDING = 16;

        /**
         * Unicode bidirectional constant RLO.
         * @since 1.4
         */
        public static byte DIRECTIONALITY_RIGHT_TO_LEFT_OVERRIDE = 17;

        /**
         * Unicode bidirectional constant PDF.
         * @since 1.4
         */
        public static byte DIRECTIONALITY_POP_DIRECTIONAL_FORMAT = 18;

        /**
         * <p>
         * Minimum value of a high surrogate or leading surrogate unit in UTF-16
         * encoding - <code>'&#92;uD800'</code>.
         * </p>
         * 
         * @since 1.5
         */
        public static char MIN_HIGH_SURROGATE = '\uD800';

        /**
         * <p>
         * Maximum value of a high surrogate or leading surrogate unit in UTF-16
         * encoding - <code>'&#92;uDBFF'</code>.
         * </p>
         * 
         * @since 1.5
         */
        public static char MAX_HIGH_SURROGATE = '\uDBFF';

        /**
         * <p>
         * Minimum value of a low surrogate or trailing surrogate unit in UTF-16
         * encoding - <code>'&#92;uDC00'</code>.
         * </p>
         * 
         * @since 1.5
         */
        public static char MIN_LOW_SURROGATE = '\uDC00';

        /**
         * Maximum value of a low surrogate or trailing surrogate unit in UTF-16
         * encoding - <code>'&#92;uDFFF'</code>.
         * </p>
         * 
         * @since 1.5
         */
        public static char MAX_LOW_SURROGATE = '\uDFFF';

        /**
         * <p>
         * Minimum value of a surrogate unit in UTF-16 encoding - <code>'&#92;uD800'</code>.
         * </p>
         * 
         * @since 1.5
         */
        public static char MIN_SURROGATE = '\uD800';

        /**
         * <p>
         * Maximum value of a surrogate unit in UTF-16 encoding - <code>'&#92;uDFFF'</code>.
         * </p>
         * 
         * @since 1.5
         */
        public static char MAX_SURROGATE = '\uDFFF';

        /**
         * <p>
         * Minimum value of a supplementary code point - <code>U+010000</code>.
         * </p>
         * 
         * @since 1.5
         */
        public static int MIN_SUPPLEMENTARY_CODE_POINT = 0x10000;

        /**
         * <p>
         * Minimum code point value - <code>U+0000</code>.
         * </p>
         * 
         * @since 1.5
         */
        public static int MIN_CODE_POINT = 0x000000;

        /**
         * <p>
         * Maximum code point value - <code>U+10FFFF</code>.
         * </p>
         * 
         * @since 1.5
         */
        public static int MAX_CODE_POINT = 0x10FFFF;

        /**
         * <p>
         * Constant for the number of bits to represent a <code>char</code> in
         * two's compliment form.
         * </p>
         * 
         * @since 1.5
         */
        public static int SIZE = 16;

        private static char[] mirrored = "\u0000\u0000\u0300\u5000\u0000\u2800\u0000\u2800\u0000\u0000\u0800\u0800\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0600`\u0000\u0000\u6000\u6000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u3f1e\ubc62\uf857\ufa0f\u1fff\u803c\ucff5\uffff\u9fff\u0107\uffcc\uc1ff\u3e00\uffc3\u3fff\u0003\u0f00\u0000\u0603\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\uff00\u0ff3"
          .ToCharArray();

        private static String digitKeys = "0Aa\u0660\u06f0\u0966\u09e6\u0a66\u0ae6\u0b66\u0be7\u0c66\u0ce6\u0d66\u0e50\u0ed0\u0f20\u1040\u1369\u17e0\u1810\uff10\uff21\uff41";

        private static char[] digitValues = "90Z7zW\u0669\u0660\u06f9\u06f0\u096f\u0966\u09ef\u09e6\u0a6f\u0a66\u0aef\u0ae6\u0b6f\u0b66\u0bef\u0be6\u0c6f\u0c66\u0cef\u0ce6\u0d6f\u0d66\u0e59\u0e50\u0ed9\u0ed0\u0f29\u0f20\u1049\u1040\u1371\u1368\u17e9\u17e0\u1819\u1810\uff19\uff10\uff3a\uff17\uff5a\uff37"
                .ToCharArray();

        private static char[] typeTags = "\u0002\u0002\u0002\u0002\u0002\u0002\u0002\u0002\u0002\u0000\u0000\u0000\u0000\u0000\u0002\u0002\u0002\u0002\u0002\u0002\u0002\u0002\u0002\u0002\u0002\u0002\u0002\u0002\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0003\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0002\u0002\u0002\u0002\u0002\u0002\u0002\u0002\u0002\u0002\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0003\u0003\u0003\u0003\u0003\u0003\u0003\u0003\u0003\u0003\u0003\u0003\u0003\u0003\u0003\u0003\u0003\u0003\u0003\u0003\u0003\u0003\u0003\u0003\u0003\u0003\u0000\u0000\u0000\u0000\u0003\u0000\u0003\u0003\u0003\u0003\u0003\u0003\u0003\u0003\u0003\u0003\u0003\u0003\u0003\u0003\u0003\u0003\u0003\u0003\u0003\u0003\u0003\u0003\u0003\u0003\u0003\u0003\u0000\u0000\u0000\u0000\u0002"
                .ToCharArray();

        // Unicode 3.0.1 (same as Unicode 3.0.0)
        private static String uppercaseKeys = "a\u00b5\u00e0\u00f8\u00ff\u0101\u0131\u0133\u013a\u014b\u017a\u017f\u0183\u0188\u018c\u0192\u0195\u0199\u01a1\u01a8\u01ad\u01b0\u01b4\u01b9\u01bd\u01bf\u01c5\u01c6\u01c8\u01c9\u01cb\u01cc\u01ce\u01dd\u01df\u01f2\u01f3\u01f5\u01f9\u0223\u0253\u0254\u0256\u0259\u025b\u0260\u0263\u0268\u0269\u026f\u0272\u0275\u0280\u0283\u0288\u028a\u0292\u0345\u03ac\u03ad\u03b1\u03c2\u03c3\u03cc\u03cd\u03d0\u03d1\u03d5\u03d6\u03db\u03f0\u03f1\u03f2\u0430\u0450\u0461\u048d\u04c2\u04c8\u04cc\u04d1\u04f9\u0561\u1e01\u1e9b\u1ea1\u1f00\u1f10\u1f20\u1f30\u1f40\u1f51\u1f60\u1f70\u1f72\u1f76\u1f78\u1f7a\u1f7c\u1f80\u1f90\u1fa0\u1fb0\u1fb3\u1fbe\u1fc3\u1fd0\u1fe0\u1fe5\u1ff3\u2170\u24d0\uff41";

        private static char[] uppercaseValues = "z\uffe0\u00b5\u02e7\u00f6\uffe0\u00fe\uffe0\u00ffy\u812f\uffff\u0131\uff18\u8137\uffff\u8148\uffff\u8177\uffff\u817e\uffff\u017f\ufed4\u8185\uffff\u0188\uffff\u018c\uffff\u0192\uffff\u0195a\u0199\uffff\u81a5\uffff\u01a8\uffff\u01ad\uffff\u01b0\uffff\u81b6\uffff\u01b9\uffff\u01bd\uffff\u01bf8\u01c5\uffff\u01c6\ufffe\u01c8\uffff\u01c9\ufffe\u01cb\uffff\u01cc\ufffe\u81dc\uffff\u01dd\uffb1\u81ef\uffff\u01f2\uffff\u01f3\ufffe\u01f5\uffff\u821f\uffff\u8233\uffff\u0253\uff2e\u0254\uff32\u0257\uff33\u0259\uff36\u025b\uff35\u0260\uff33\u0263\uff31\u0268\uff2f\u0269\uff2d\u026f\uff2d\u0272\uff2b\u0275\uff2a\u0280\uff26\u0283\uff26\u0288\uff26\u028b\uff27\u0292\uff25\u0345T\u03ac\uffda\u03af\uffdb\u03c1\uffe0\u03c2\uffe1\u03cb\uffe0\u03cc\uffc0\u03ce\uffc1\u03d0\uffc2\u03d1\uffc7\u03d5\uffd1\u03d6\uffca\u83ef\uffff\u03f0\uffaa\u03f1\uffb0\u03f2\uffb1\u044f\uffe0\u045f\uffb0\u8481\uffff\u84bf\uffff\u84c4\uffff\u04c8\uffff\u04cc\uffff\u84f5\uffff\u04f9\uffff\u0586\uffd0\u9e95\uffff\u1e9b\uffc5\u9ef9\uffff\u1f07\b\u1f15\b\u1f27\b\u1f37\b\u1f45\b\u9f57\b\u1f67\b\u1f71J\u1f75V\u1f77d\u1f79\u0080\u1f7bp\u1f7d~\u1f87\b\u1f97\b\u1fa7\b\u1fb1\b\u1fb3\t\u1fbe\ue3db\u1fc3\t\u1fd1\b\u1fe1\b\u1fe5\u0007\u1ff3\t\u217f\ufff0\u24e9\uffe6\uff5a\uffe0"
                .ToCharArray();

        private static int[] uppercaseValuesCache = {
    	924, 182, 183, 184, 185, 186, 187, 188, 189, 190, 191, 192, 193, 194, 195, 196, 
    	197, 198, 199, 200, 201, 202, 203, 204, 205, 206, 207, 208, 209, 210, 211, 212, 
    	213, 214, 215, 216, 217, 218, 219, 220, 221, 222, 223, 192, 193, 194, 195, 196, 
    	197, 198, 199, 200, 201, 202, 203, 204, 205, 206, 207, 208, 209, 210, 211, 212, 
    	213, 214, 247, 216, 217, 218, 219, 220, 221, 222, 376, 256, 256, 258, 258, 260, 
    	260, 262, 262, 264, 264, 266, 266, 268, 268, 270, 270, 272, 272, 274, 274, 276, 
    	276, 278, 278, 280, 280, 282, 282, 284, 284, 286, 286, 288, 288, 290, 290, 292, 
    	292, 294, 294, 296, 296, 298, 298, 300, 300, 302, 302, 304, 73, 306, 306, 308, 
    	308, 310, 310, 312, 313, 313, 315, 315, 317, 317, 319, 319, 321, 321, 323, 323, 
    	325, 325, 327, 327, 329, 330, 330, 332, 332, 334, 334, 336, 336, 338, 338, 340, 
    	340, 342, 342, 344, 344, 346, 346, 348, 348, 350, 350, 352, 352, 354, 354, 356, 
    	356, 358, 358, 360, 360, 362, 362, 364, 364, 366, 366, 368, 368, 370, 370, 372, 
    	372, 374, 374, 376, 377, 377, 379, 379, 381, 381, 83, 384, 385, 386, 386, 388, 
    	388, 390, 391, 391, 393, 394, 395, 395, 397, 398, 399, 400, 401, 401, 403, 404, 
    	502, 406, 407, 408, 408, 410, 411, 412, 413, 544, 415, 416, 416, 418, 418, 420, 
    	420, 422, 423, 423, 425, 426, 427, 428, 428, 430, 431, 431, 433, 434, 435, 435, 
    	437, 437, 439, 440, 440, 442, 443, 444, 444, 446, 503, 448, 449, 450, 451, 452, 
    	452, 452, 455, 455, 455, 458, 458, 458, 461, 461, 463, 463, 465, 465, 467, 467, 
    	469, 469, 471, 471, 473, 473, 475, 475, 398, 478, 478, 480, 480, 482, 482, 484, 
    	484, 486, 486, 488, 488, 490, 490, 492, 492, 494, 494, 496, 497, 497, 497, 500, 
    	500, 502, 503, 504, 504, 506, 506, 508, 508, 510, 510, 512, 512, 514, 514, 516, 
    	516, 518, 518, 520, 520, 522, 522, 524, 524, 526, 526, 528, 528, 530, 530, 532, 
    	532, 534, 534, 536, 536, 538, 538, 540, 540, 542, 542, 544, 545, 546, 546, 548, 
    	548, 550, 550, 552, 552, 554, 554, 556, 556, 558, 558, 560, 560, 562, 562, 564, 
    	565, 566, 567, 568, 569, 570, 571, 572, 573, 574, 575, 576, 577, 578, 579, 580, 
    	581, 582, 583, 584, 585, 586, 587, 588, 589, 590, 591, 592, 593, 594, 385, 390, 
    	597, 393, 394, 600, 399, 602, 400, 604, 605, 606, 607, 403, 609, 610, 404, 612, 
    	613, 614, 615, 407, 406, 618, 619, 620, 621, 622, 412, 624, 625, 413, 627, 628, 
    	415, 630, 631, 632, 633, 634, 635, 636, 637, 638, 639, 422, 641, 642, 425, 644, 
    	645, 646, 647, 430, 649, 433, 434, 652, 653, 654, 655, 656, 657, 439, 659, 660, 
    	661, 662, 663, 664, 665, 666, 667, 668, 669, 670, 671, 672, 673, 674, 675, 676, 
    	677, 678, 679, 680, 681, 682, 683, 684, 685, 686, 687, 688, 689, 690, 691, 692, 
    	693, 694, 695, 696, 697, 698, 699, 700, 701, 702, 703, 704, 705, 706, 707, 708, 
    	709, 710, 711, 712, 713, 714, 715, 716, 717, 718, 719, 720, 721, 722, 723, 724, 
    	725, 726, 727, 728, 729, 730, 731, 732, 733, 734, 735, 736, 737, 738, 739, 740, 
    	741, 742, 743, 744, 745, 746, 747, 748, 749, 750, 751, 752, 753, 754, 755, 756, 
    	757, 758, 759, 760, 761, 762, 763, 764, 765, 766, 767, 768, 769, 770, 771, 772, 
    	773, 774, 775, 776, 777, 778, 779, 780, 781, 782, 783, 784, 785, 786, 787, 788, 
    	789, 790, 791, 792, 793, 794, 795, 796, 797, 798, 799, 800, 801, 802, 803, 804, 
    	805, 806, 807, 808, 809, 810, 811, 812, 813, 814, 815, 816, 817, 818, 819, 820, 
    	821, 822, 823, 824, 825, 826, 827, 828, 829, 830, 831, 832, 833, 834, 835, 836, 
    	921, 838, 839, 840, 841, 842, 843, 844, 845, 846, 847, 848, 849, 850, 851, 852, 
    	853, 854, 855, 856, 857, 858, 859, 860, 861, 862, 863, 864, 865, 866, 867, 868, 
    	869, 870, 871, 872, 873, 874, 875, 876, 877, 878, 879, 880, 881, 882, 883, 884, 
    	885, 886, 887, 888, 889, 890, 891, 892, 893, 894, 895, 896, 897, 898, 899, 900, 
    	901, 902, 903, 904, 905, 906, 907, 908, 909, 910, 911, 912, 913, 914, 915, 916, 
    	917, 918, 919, 920, 921, 922, 923, 924, 925, 926, 927, 928, 929, 930, 931, 932, 
    	933, 934, 935, 936, 937, 938, 939, 902, 904, 905, 906, 944, 913, 914, 915, 916, 
    	917, 918, 919, 920, 921, 922, 923, 924, 925, 926, 927, 928, 929, 931, 931, 932, 
    	933, 934, 935, 936, 937, 938, 939, 908, 910, 911, 975, 914, 920, 978, 979, 980, 
    	934, 928, 983, 984, 984, 986, 986, 988, 988, 990, 990, 992, 992, 994, 994, 996, 
    	996, 998, 998};

        private static int[] typeValuesCache = {
    	15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 
    	15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 
    	12, 24, 24, 24, 26, 24, 24, 24, 21, 22, 24, 25, 24, 20, 24, 24, 
    	9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 24, 24, 25, 25, 25, 24, 
    	24, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 
    	1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 21, 24, 22, 27, 23, 
    	27, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 
    	2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 21, 25, 22, 25, 15, 
    	15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 
    	15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 
    	12, 24, 26, 26, 26, 26, 28, 28, 27, 28, 2, 29, 25, 16, 28, 27, 
    	28, 25, 11, 11, 27, 2, 28, 24, 27, 11, 2, 30, 11, 11, 11, 24, 
    	1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 
    	1, 1, 1, 1, 1, 1, 1, 25, 1, 1, 1, 1, 1, 1, 1, 2, 
    	2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 
    	2, 2, 2, 2, 2, 2, 2, 25, 2, 2, 2, 2, 2, 2, 2, 2, 
    	1, 2, 1, 2, 1, 2, 1, 2, 1, 2, 1, 2, 1, 2, 1, 2, 
    	1, 2, 1, 2, 1, 2, 1, 2, 1, 2, 1, 2, 1, 2, 1, 2, 
    	1, 2, 1, 2, 1, 2, 1, 2, 1, 2, 1, 2, 1, 2, 1, 2, 
    	1, 2, 1, 2, 1, 2, 1, 2, 2, 1, 2, 1, 2, 1, 2, 1, 
    	2, 1, 2, 1, 2, 1, 2, 1, 2, 2, 1, 2, 1, 2, 1, 2, 
    	1, 2, 1, 2, 1, 2, 1, 2, 1, 2, 1, 2, 1, 2, 1, 2, 
    	1, 2, 1, 2, 1, 2, 1, 2, 1, 2, 1, 2, 1, 2, 1, 2, 
    	1, 2, 1, 2, 1, 2, 1, 2, 1, 1, 2, 1, 2, 1, 2, 2, 
    	2, 1, 1, 2, 1, 2, 1, 1, 2, 1, 1, 1, 2, 2, 1, 1, 
    	1, 1, 2, 1, 1, 2, 1, 1, 1, 2, 2, 2, 1, 1, 2, 1, 
    	1, 2, 1, 2, 1, 2, 1, 1, 2, 1, 2, 2, 1, 2, 1, 1, 
    	2, 1, 1, 1, 2, 1, 2, 1, 1, 2, 2, 5, 1, 2, 2, 2, 
    	5, 5, 5, 5, 1, 3, 2, 1, 3, 2, 1, 3, 2, 1, 2, 1, 
    	2, 1, 2, 1, 2, 1, 2, 1, 2, 1, 2, 1, 2, 2, 1, 2, 
    	1, 2, 1, 2, 1, 2, 1, 2, 1, 2, 1, 2, 1, 2, 1, 2, 
    	2, 1, 3, 2, 1, 2, 1, 1, 1, 2, 1, 2, 1, 2, 1, 2, 
    	1, 2, 1, 2, 1, 2, 1, 2, 1, 2, 1, 2, 1, 2, 1, 2, 
    	1, 2, 1, 2, 1, 2, 1, 2, 1, 2, 1, 2, 1, 2, 1, 2, 
    	1, 2, 1, 2, 1, 2, 1, 2, 1, 2, 1, 2, 1, 2, 1, 2, 
    	1, 2, 1, 2, 2, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 
    	0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 
    	2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 
    	2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 
    	2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 
    	2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 
    	2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 
    	2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 
    	4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 
    	4, 4, 27, 27, 27, 27, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 
    	4, 4, 27, 27, 27, 27, 27, 27, 27, 27, 27, 27, 27, 27, 27, 27, 
    	4, 4, 4, 4, 4, 27, 27, 27, 27, 27, 27, 27, 27, 27, 4, 27, 
    	27, 27, 27, 27, 27, 27, 27, 27, 27, 27, 27, 27, 27, 27, 27, 27, 
    	6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 
    	6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 
    	6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 
    	6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 
    	6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 
    	6, 6, 6, 6, 6, 6, 6, 6, 0, 0, 0, 0, 0, 6, 6, 6, 
    	6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 
    	0, 0, 0, 0, 27, 27, 0, 0, 0, 0, 4, 0, 0, 0, 24, 0, 
    	0, 0, 0, 0, 27, 27, 1, 24, 1, 1, 1, 0, 1, 0, 1, 1, 
    	2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 
    	1, 1, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 
    	2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 
    	2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 0, 
    	2, 2, 1, 1, 1, 2, 2, 2, 1, 2, 1, 2, 1, 2, 1, 2, 
            	1, 2, 1, 2, 1, 2, 1, 2 };

        // Unicode 3.0.1 (same as Unicode 3.0.0)
        private static String typeKeys = "\u0000 \"$&(*-/1:<?A[]_a{}\u007f\u00a0\u00a2\u00a6\u00a8\u00aa\u00ac\u00ae\u00b1\u00b3\u00b5\u00b7\u00b9\u00bb\u00bd\u00bf\u00c1\u00d7\u00d9\u00df\u00f7\u00f9\u0100\u0138\u0149\u0179\u017f\u0181\u0183\u0187\u018a\u018c\u018e\u0192\u0194\u0197\u0199\u019c\u019e\u01a0\u01a7\u01ab\u01af\u01b2\u01b4\u01b8\u01ba\u01bc\u01be\u01c0\u01c4\u01c6\u01c8\u01ca\u01cc\u01dd\u01f0\u01f2\u01f4\u01f7\u01f9\u0222\u0250\u02b0\u02b9\u02bb\u02c2\u02d0\u02d2\u02e0\u02e5\u02ee\u0300\u0360\u0374\u037a\u037e\u0384\u0386\u0389\u038c\u038e\u0390\u0392\u03a3\u03ac\u03d0\u03d2\u03d5\u03da\u03f0\u0400\u0430\u0460\u0482\u0484\u0488\u048c\u04c1\u04c7\u04cb\u04d0\u04f8\u0531\u0559\u055b\u0561\u0589\u0591\u05a3\u05bb\u05be\u05c2\u05d0\u05f0\u05f3\u060c\u061b\u061f\u0621\u0640\u0642\u064b\u0660\u066a\u0670\u0672\u06d4\u06d6\u06dd\u06df\u06e5\u06e7\u06e9\u06eb\u06f0\u06fa\u06fd\u0700\u070f\u0711\u0713\u0730\u0780\u07a6\u0901\u0903\u0905\u093c\u093e\u0941\u0949\u094d\u0950\u0952\u0958\u0962\u0964\u0966\u0970\u0981\u0983\u0985\u098f\u0993\u09aa\u09b2\u09b6\u09bc\u09be\u09c1\u09c7\u09cb\u09cd\u09d7\u09dc\u09df\u09e2\u09e6\u09f0\u09f2\u09f4\u09fa\u0a02\u0a05\u0a0f\u0a13\u0a2a\u0a32\u0a35\u0a38\u0a3c\u0a3e\u0a41\u0a47\u0a4b\u0a59\u0a5e\u0a66\u0a70\u0a72\u0a81\u0a83\u0a85\u0a8d\u0a8f\u0a93\u0aaa\u0ab2\u0ab5\u0abc\u0abe\u0ac1\u0ac7\u0ac9\u0acb\u0acd\u0ad0\u0ae0\u0ae6\u0b01\u0b03\u0b05\u0b0f\u0b13\u0b2a\u0b32\u0b36\u0b3c\u0b3e\u0b42\u0b47\u0b4b\u0b4d\u0b56\u0b5c\u0b5f\u0b66\u0b70\u0b82\u0b85\u0b8e\u0b92\u0b99\u0b9c\u0b9e\u0ba3\u0ba8\u0bae\u0bb7\u0bbe\u0bc0\u0bc2\u0bc6\u0bca\u0bcd\u0bd7\u0be7\u0bf0\u0c01\u0c05\u0c0e\u0c12\u0c2a\u0c35\u0c3e\u0c41\u0c46\u0c4a\u0c55\u0c60\u0c66\u0c82\u0c85\u0c8e\u0c92\u0caa\u0cb5\u0cbe\u0cc1\u0cc6\u0cc8\u0cca\u0ccc\u0cd5\u0cde\u0ce0\u0ce6\u0d02\u0d05\u0d0e\u0d12\u0d2a\u0d3e\u0d41\u0d46\u0d4a\u0d4d\u0d57\u0d60\u0d66\u0d82\u0d85\u0d9a\u0db3\u0dbd\u0dc0\u0dca\u0dcf\u0dd2\u0dd6\u0dd8\u0df2\u0df4\u0e01\u0e31\u0e33\u0e35\u0e3f\u0e41\u0e46\u0e48\u0e4f\u0e51\u0e5a\u0e81\u0e84\u0e87\u0e8a\u0e8d\u0e94\u0e99\u0ea1\u0ea5\u0ea7\u0eaa\u0ead\u0eb1\u0eb3\u0eb5\u0ebb\u0ebd\u0ec0\u0ec6\u0ec8\u0ed0\u0edc\u0f00\u0f02\u0f04\u0f13\u0f18\u0f1a\u0f20\u0f2a\u0f34\u0f3a\u0f3e\u0f40\u0f49\u0f71\u0f7f\u0f81\u0f85\u0f87\u0f89\u0f90\u0f99\u0fbe\u0fc6\u0fc8\u0fcf\u1000\u1023\u1029\u102c\u102e\u1031\u1036\u1038\u1040\u104a\u1050\u1056\u1058\u10a0\u10d0\u10fb\u1100\u115f\u11a8\u1200\u1208\u1248\u124a\u1250\u1258\u125a\u1260\u1288\u128a\u1290\u12b0\u12b2\u12b8\u12c0\u12c2\u12c8\u12d0\u12d8\u12f0\u1310\u1312\u1318\u1320\u1348\u1361\u1369\u1372\u13a0\u1401\u166d\u166f\u1680\u1682\u169b\u16a0\u16eb\u16ee\u1780\u17b4\u17b7\u17be\u17c6\u17c8\u17ca\u17d4\u17db\u17e0\u1800\u1806\u1808\u180b\u1810\u1820\u1843\u1845\u1880\u18a9\u1e00\u1e96\u1ea0\u1f00\u1f08\u1f10\u1f18\u1f20\u1f28\u1f30\u1f38\u1f40\u1f48\u1f50\u1f59\u1f5b\u1f5d\u1f5f\u1f61\u1f68\u1f70\u1f80\u1f88\u1f90\u1f98\u1fa0\u1fa8\u1fb0\u1fb6\u1fb8\u1fbc\u1fbe\u1fc0\u1fc2\u1fc6\u1fc8\u1fcc\u1fce\u1fd0\u1fd6\u1fd8\u1fdd\u1fe0\u1fe8\u1fed\u1ff2\u1ff6\u1ff8\u1ffc\u1ffe\u2000\u200c\u2010\u2016\u2018\u201a\u201c\u201e\u2020\u2028\u202a\u202f\u2031\u2039\u203b\u203f\u2041\u2044\u2046\u2048\u206a\u2070\u2074\u207a\u207d\u207f\u2081\u208a\u208d\u20a0\u20d0\u20dd\u20e1\u20e3\u2100\u2102\u2104\u2107\u2109\u210b\u210e\u2110\u2113\u2115\u2117\u2119\u211e\u2124\u212b\u212e\u2130\u2132\u2134\u2136\u2139\u2153\u2160\u2190\u2195\u219a\u219c\u21a0\u21a2\u21a5\u21a8\u21ae\u21b0\u21ce\u21d0\u21d2\u21d6\u2200\u2300\u2308\u230c\u2320\u2322\u2329\u232b\u237d\u2400\u2440\u2460\u249c\u24ea\u2500\u25a0\u25b7\u25b9\u25c1\u25c3\u2600\u2619\u266f\u2671\u2701\u2706\u270c\u2729\u274d\u274f\u2756\u2758\u2761\u2776\u2794\u2798\u27b1\u2800\u2e80\u2e9b\u2f00\u2ff0\u3000\u3002\u3004\u3006\u3008\u3012\u3014\u301c\u301e\u3020\u3022\u302a\u3030\u3032\u3036\u3038\u303e\u3041\u3099\u309b\u309d\u30a1\u30fb\u30fd\u3105\u3131\u3190\u3192\u3196\u31a0\u3200\u3220\u322a\u3260\u327f\u3281\u328a\u32c0\u32d0\u3300\u337b\u33e0\u3400\u4e00\ua000\ua490\ua4a4\ua4b5\ua4c2\ua4c6\uac00\ud800\ue000\uf900\ufb00\ufb13\ufb1d\ufb20\ufb29\ufb2b\ufb38\ufb3e\ufb40\ufb43\ufb46\ufbd3\ufd3e\ufd50\ufd92\ufdf0\ufe20\ufe30\ufe32\ufe34\ufe36\ufe49\ufe4d\ufe50\ufe54\ufe58\ufe5a\ufe5f\ufe62\ufe65\ufe68\ufe6b\ufe70\ufe74\ufe76\ufeff\uff01\uff04\uff06\uff08\uff0a\uff0d\uff0f\uff11\uff1a\uff1c\uff1f\uff21\uff3b\uff3d\uff3f\uff41\uff5b\uff5d\uff61\uff63\uff65\uff67\uff70\uff72\uff9e\uffa0\uffc2\uffca\uffd2\uffda\uffe0\uffe2\uffe4\uffe6\uffe8\uffea\uffed\ufff9\ufffc";


        private static char[] typeValues = "\u001f\u000f!\u180c#\u0018%\u181a'\u0018)\u1615,\u1918.\u14180\u18099\t;\u0018>\u0019@\u0018Z\u0001\\\u1518^\u161b`\u171bz\u0002|\u1519~\u1619\u009f\u000f\u00a1\u180c\u00a5\u001a\u00a7\u001c\u00a9\u1c1b\u00ab\u1d02\u00ad\u1419\u00b0\u1b1c\u00b2\u190b\u00b4\u0b1b\u00b6\u021c\u00b8\u181b\u00ba\u0b02\u00bc\u1e0b\u00be\u000b\u00c0\u1801\u00d6\u0001\u00d8\u1901\u00de\u0001\u00f6\u0002\u00f8\u1902\u00ff\u0002\u0137\u0201\u0148\u0102\u0178\u0201\u017e\u0102\u0180\u0002\u0182\u0001\u0186\u0201\u0189\u0102\u018b\u0001\u018d\u0002\u0191\u0001\u0193\u0102\u0196\u0201\u0198\u0001\u019b\u0002\u019d\u0001\u019f\u0102\u01a6\u0201\u01aa\u0102\u01ae\u0201\u01b1\u0102\u01b3\u0001\u01b7\u0102\u01b9\u0201\u01bb\u0502\u01bd\u0201\u01bf\u0002\u01c3\u0005\u01c5\u0301\u01c7\u0102\u01c9\u0203\u01cb\u0301\u01dc\u0102\u01ef\u0201\u01f1\u0102\u01f3\u0203\u01f6\u0201\u01f8\u0001\u021f\u0201\u0233\u0201\u02ad\u0002\u02b8\u0004\u02ba\u001b\u02c1\u0004\u02cf\u001b\u02d1\u0004\u02df\u001b\u02e4\u0004\u02ed\u001b\u02ee\u0004\u034e\u0006\u0362\u0006\u0375\u001b\u037a\u0004\u037e\u0018\u0385\u001b\u0388\u1801\u038a\u0001\u038c\u0001\u038f\u0001\u0391\u0102\u03a1\u0001\u03ab\u0001\u03ce\u0002\u03d1\u0002\u03d4\u0001\u03d7\u0002\u03ef\u0201\u03f3\u0002\u042f\u0001\u045f\u0002\u0481\u0201\u0483\u061c\u0486\u0006\u0489\u0007\u04c0\u0201\u04c4\u0102\u04c8\u0102\u04cc\u0102\u04f5\u0201\u04f9\u0201\u0556\u0001\u055a\u0418\u055f\u0018\u0587\u0002\u058a\u1814\u05a1\u0006\u05b9\u0006\u05bd\u0006\u05c1\u0618\u05c4\u1806\u05ea\u0005\u05f2\u0005\u05f4\u0018\u060c\u0018\u061b\u1800\u061f\u1800\u063a\u0005\u0641\u0504\u064a\u0005\u0655\u0006\u0669\t\u066d\u0018\u0671\u0506\u06d3\u0005\u06d5\u0518\u06dc\u0006\u06de\u0007\u06e4\u0006\u06e6\u0004\u06e8\u0006\u06ea\u1c06\u06ed\u0006\u06f9\t\u06fc\u0005\u06fe\u001c\u070d\u0018\u0710\u1005\u0712\u0605\u072c\u0005\u074a\u0006\u07a5\u0005\u07b0\u0006\u0902\u0006\u0903\u0800\u0939\u0005\u093d\u0506\u0940\b\u0948\u0006\u094c\b\u094d\u0600\u0951\u0605\u0954\u0006\u0961\u0005\u0963\u0006\u0965\u0018\u096f\t\u0970\u0018\u0982\u0608\u0983\u0800\u098c\u0005\u0990\u0005\u09a8\u0005\u09b0\u0005\u09b2\u0005\u09b9\u0005\u09bc\u0006\u09c0\b\u09c4\u0006\u09c8\b\u09cc\b\u09cd\u0600\u09d7\u0800\u09dd\u0005\u09e1\u0005\u09e3\u0006\u09ef\t\u09f1\u0005\u09f3\u001a\u09f9\u000b\u09fa\u001c\u0a02\u0006\u0a0a\u0005\u0a10\u0005\u0a28\u0005\u0a30\u0005\u0a33\u0005\u0a36\u0005\u0a39\u0005\u0a3c\u0006\u0a40\b\u0a42\u0006\u0a48\u0006\u0a4d\u0006\u0a5c\u0005\u0a5e\u0005\u0a6f\t\u0a71\u0006\u0a74\u0005\u0a82\u0006\u0a83\u0800\u0a8b\u0005\u0a8d\u0500\u0a91\u0005\u0aa8\u0005\u0ab0\u0005\u0ab3\u0005\u0ab9\u0005\u0abd\u0506\u0ac0\b\u0ac5\u0006\u0ac8\u0006\u0ac9\u0800\u0acc\b\u0acd\u0600\u0ad0\u0005\u0ae0\u0005\u0aef\t\u0b02\u0608\u0b03\u0800\u0b0c\u0005\u0b10\u0005\u0b28\u0005\u0b30\u0005\u0b33\u0005\u0b39\u0005\u0b3d\u0506\u0b41\u0608\u0b43\u0006\u0b48\b\u0b4c\b\u0b4d\u0600\u0b57\u0806\u0b5d\u0005\u0b61\u0005\u0b6f\t\u0b70\u001c\u0b83\u0806\u0b8a\u0005\u0b90\u0005\u0b95\u0005\u0b9a\u0005\u0b9c\u0005\u0b9f\u0005\u0ba4\u0005\u0baa\u0005\u0bb5\u0005\u0bb9\u0005\u0bbf\b\u0bc1\u0806\u0bc2\b\u0bc8\b\u0bcc\b\u0bcd\u0600\u0bd7\u0800\u0bef\t\u0bf2\u000b\u0c03\b\u0c0c\u0005\u0c10\u0005\u0c28\u0005\u0c33\u0005\u0c39\u0005\u0c40\u0006\u0c44\b\u0c48\u0006\u0c4d\u0006\u0c56\u0006\u0c61\u0005\u0c6f\t\u0c83\b\u0c8c\u0005\u0c90\u0005\u0ca8\u0005\u0cb3\u0005\u0cb9\u0005\u0cc0\u0608\u0cc4\b\u0cc7\u0806\u0cc8\b\u0ccb\b\u0ccd\u0006\u0cd6\b\u0cde\u0005\u0ce1\u0005\u0cef\t\u0d03\b\u0d0c\u0005\u0d10\u0005\u0d28\u0005\u0d39\u0005\u0d40\b\u0d43\u0006\u0d48\b\u0d4c\b\u0d4d\u0600\u0d57\u0800\u0d61\u0005\u0d6f\t\u0d83\b\u0d96\u0005\u0db1\u0005\u0dbb\u0005\u0dbd\u0500\u0dc6\u0005\u0dca\u0006\u0dd1\b\u0dd4\u0006\u0dd6\u0006\u0ddf\b\u0df3\b\u0df4\u0018\u0e30\u0005\u0e32\u0605\u0e34\u0506\u0e3a\u0006\u0e40\u1a05\u0e45\u0005\u0e47\u0604\u0e4e\u0006\u0e50\u1809\u0e59\t\u0e5b\u0018\u0e82\u0005\u0e84\u0005\u0e88\u0005\u0e8a\u0005\u0e8d\u0500\u0e97\u0005\u0e9f\u0005\u0ea3\u0005\u0ea5\u0500\u0ea7\u0500\u0eab\u0005\u0eb0\u0005\u0eb2\u0605\u0eb4\u0506\u0eb9\u0006\u0ebc\u0006\u0ebd\u0500\u0ec4\u0005\u0ec6\u0004\u0ecd\u0006\u0ed9\t\u0edd\u0005\u0f01\u1c05\u0f03\u001c\u0f12\u0018\u0f17\u001c\u0f19\u0006\u0f1f\u001c\u0f29\t\u0f33\u000b\u0f39\u061c\u0f3d\u1615\u0f3f\b\u0f47\u0005\u0f6a\u0005\u0f7e\u0006\u0f80\u0806\u0f84\u0006\u0f86\u1806\u0f88\u0605\u0f8b\u0005\u0f97\u0006\u0fbc\u0006\u0fc5\u001c\u0fc7\u1c06\u0fcc\u001c\u0fcf\u1c00\u1021\u0005\u1027\u0005\u102a\u0005\u102d\u0608\u1030\u0006\u1032\u0806\u1037\u0006\u1039\u0608\u1049\t\u104f\u0018\u1055\u0005\u1057\b\u1059\u0006\u10c5\u0001\u10f6\u0005\u10fb\u1800\u1159\u0005\u11a2\u0005\u11f9\u0005\u1206\u0005\u1246\u0005\u1248\u0005\u124d\u0005\u1256\u0005\u1258\u0005\u125d\u0005\u1286\u0005\u1288\u0005\u128d\u0005\u12ae\u0005\u12b0\u0005\u12b5\u0005\u12be\u0005\u12c0\u0005\u12c5\u0005\u12ce\u0005\u12d6\u0005\u12ee\u0005\u130e\u0005\u1310\u0005\u1315\u0005\u131e\u0005\u1346\u0005\u135a\u0005\u1368\u0018\u1371\t\u137c\u000b\u13f4\u0005\u166c\u0005\u166e\u0018\u1676\u0005\u1681\u050c\u169a\u0005\u169c\u1516\u16ea\u0005\u16ed\u0018\u16f0\u000b\u17b3\u0005\u17b6\b\u17bd\u0006\u17c5\b\u17c7\u0806\u17c9\u0608\u17d3\u0006\u17da\u0018\u17dc\u1a18\u17e9\t\u1805\u0018\u1807\u1814\u180a\u0018\u180e\u0010\u1819\t\u1842\u0005\u1844\u0405\u1877\u0005\u18a8\u0005\u18a9\u0600\u1e95\u0201\u1e9b\u0002\u1ef9\u0201\u1f07\u0002\u1f0f\u0001\u1f15\u0002\u1f1d\u0001\u1f27\u0002\u1f2f\u0001\u1f37\u0002\u1f3f\u0001\u1f45\u0002\u1f4d\u0001\u1f57\u0002\u1f59\u0100\u1f5b\u0100\u1f5d\u0100\u1f60\u0102\u1f67\u0002\u1f6f\u0001\u1f7d\u0002\u1f87\u0002\u1f8f\u0003\u1f97\u0002\u1f9f\u0003\u1fa7\u0002\u1faf\u0003\u1fb4\u0002\u1fb7\u0002\u1fbb\u0001\u1fbd\u1b03\u1fbf\u1b02\u1fc1\u001b\u1fc4\u0002\u1fc7\u0002\u1fcb\u0001\u1fcd\u1b03\u1fcf\u001b\u1fd3\u0002\u1fd7\u0002\u1fdb\u0001\u1fdf\u001b\u1fe7\u0002\u1fec\u0001\u1fef\u001b\u1ff4\u0002\u1ff7\u0002\u1ffb\u0001\u1ffd\u1b03\u1ffe\u001b\u200b\f\u200f\u0010\u2015\u0014\u2017\u0018\u2019\u1e1d\u201b\u1d15\u201d\u1e1d\u201f\u1d15\u2027\u0018\u2029\u0e0d\u202e\u0010\u2030\u0c18\u2038\u0018\u203a\u1d1e\u203e\u0018\u2040\u0017\u2043\u0018\u2045\u1519\u2046\u0016\u204d\u0018\u206f\u0010\u2070\u000b\u2079\u000b\u207c\u0019\u207e\u1516\u2080\u020b\u2089\u000b\u208c\u0019\u208e\u1516\u20af\u001a\u20dc\u0006\u20e0\u0007\u20e2\u0607\u20e3\u0700\u2101\u001c\u2103\u1c01\u2106\u001c\u2108\u011c\u210a\u1c02\u210d\u0001\u210f\u0002\u2112\u0001\u2114\u021c\u2116\u011c\u2118\u001c\u211d\u0001\u2123\u001c\u212a\u1c01\u212d\u0001\u212f\u021c\u2131\u0001\u2133\u011c\u2135\u0502\u2138\u0005\u213a\u021c\u215f\u000b\u2183\n\u2194\u0019\u2199\u001c\u219b\u0019\u219f\u001c\u21a1\u1c19\u21a4\u191c\u21a7\u1c19\u21ad\u001c\u21af\u1c19\u21cd\u001c\u21cf\u0019\u21d1\u001c\u21d5\u1c19\u21f3\u001c\u22f1\u0019\u2307\u001c\u230b\u0019\u231f\u001c\u2321\u0019\u2328\u001c\u232a\u1516\u237b\u001c\u239a\u001c\u2426\u001c\u244a\u001c\u249b\u000b\u24e9\u001c\u24ea\u000b\u2595\u001c\u25b6\u001c\u25b8\u191c\u25c0\u001c\u25c2\u191c\u25f7\u001c\u2613\u001c\u266e\u001c\u2670\u191c\u2671\u1c00\u2704\u001c\u2709\u001c\u2727\u001c\u274b\u001c\u274d\u1c00\u2752\u001c\u2756\u001c\u275e\u001c\u2767\u001c\u2793\u000b\u2794\u001c\u27af\u001c\u27be\u001c\u28ff\u001c\u2e99\u001c\u2ef3\u001c\u2fd5\u001c\u2ffb\u001c\u3001\u180c\u3003\u0018\u3005\u041c\u3007\u0a05\u3011\u1615\u3013\u001c\u301b\u1615\u301d\u1514\u301f\u0016\u3021\u0a1c\u3029\n\u302f\u0006\u3031\u0414\u3035\u0004\u3037\u001c\u303a\n\u303f\u001c\u3094\u0005\u309a\u0006\u309c\u001b\u309e\u0004\u30fa\u0005\u30fc\u1704\u30fe\u0004\u312c\u0005\u318e\u0005\u3191\u001c\u3195\u000b\u319f\u001c\u31b7\u0005\u321c\u001c\u3229\u000b\u3243\u001c\u327b\u001c\u3280\u1c0b\u3289\u000b\u32b0\u001c\u32cb\u001c\u32fe\u001c\u3376\u001c\u33dd\u001c\u33fe\u001c\u4db5\u0005\u9fa5\u0005\ua48c\u0005\ua4a1\u001c\ua4b3\u001c\ua4c0\u001c\ua4c4\u001c\ua4c6\u001c\ud7a3\u0005\udfff\u0013\uf8ff\u0012\ufa2d\u0005\ufb06\u0002\ufb17\u0002\ufb1f\u0506\ufb28\u0005\ufb2a\u1905\ufb36\u0005\ufb3c\u0005\ufb3e\u0005\ufb41\u0005\ufb44\u0005\ufbb1\u0005\ufd3d\u0005\ufd3f\u1615\ufd8f\u0005\ufdc7\u0005\ufdfb\u0005\ufe23\u0006\ufe31\u1418\ufe33\u1714\ufe35\u1517\ufe44\u1516\ufe4c\u0018\ufe4f\u0017\ufe52\u0018\ufe57\u0018\ufe59\u1514\ufe5e\u1516\ufe61\u0018\ufe64\u1419\ufe66\u0019\ufe6a\u1a18\ufe6b\u1800\ufe72\u0005\ufe74\u0005\ufefc\u0005\ufeff\u1000\uff03\u0018\uff05\u181a\uff07\u0018\uff09\u1615\uff0c\u1918\uff0e\u1418\uff10\u1809\uff19\t\uff1b\u0018\uff1e\u0019\uff20\u0018\uff3a\u0001\uff3c\u1518\uff3e\u161b\uff40\u171b\uff5a\u0002\uff5c\u1519\uff5e\u1619\uff62\u1815\uff64\u1618\uff66\u1705\uff6f\u0005\uff71\u0504\uff9d\u0005\uff9f\u0004\uffbe\u0005\uffc7\u0005\uffcf\u0005\uffd7\u0005\uffdc\u0005\uffe1\u001a\uffe3\u1b19\uffe5\u1a1c\uffe6\u001a\uffe9\u191c\uffec\u0019\uffee\u001c\ufffb\u0010\ufffd\u001c"
                .ToCharArray();

        private static String lowercaseKeys = "A\u00c0\u00d8\u0100\u0130\u0132\u0139\u014a\u0178\u0179\u0181\u0182\u0186\u0187\u0189\u018b\u018e\u018f\u0190\u0191\u0193\u0194\u0196\u0197\u0198\u019c\u019d\u019f\u01a0\u01a6\u01a7\u01a9\u01ac\u01ae\u01af\u01b1\u01b3\u01b7\u01b8\u01bc\u01c4\u01c5\u01c7\u01c8\u01ca\u01cb\u01de\u01f1\u01f2\u01f6\u01f7\u01f8\u0222\u0386\u0388\u038c\u038e\u0391\u03a3\u03da\u0400\u0410\u0460\u048c\u04c1\u04c7\u04cb\u04d0\u04f8\u0531\u1e00\u1ea0\u1f08\u1f18\u1f28\u1f38\u1f48\u1f59\u1f68\u1f88\u1f98\u1fa8\u1fb8\u1fba\u1fbc\u1fc8\u1fcc\u1fd8\u1fda\u1fe8\u1fea\u1fec\u1ff8\u1ffa\u1ffc\u2126\u212a\u212b\u2160\u24b6\uff21";

        private static char[] lowercaseValues = "Z \u00d6 \u00de \u812e\u0001\u0130\uff39\u8136\u0001\u8147\u0001\u8176\u0001\u0178\uff87\u817d\u0001\u0181\u00d2\u8184\u0001\u0186\u00ce\u0187\u0001\u018a\u00cd\u018b\u0001\u018eO\u018f\u00ca\u0190\u00cb\u0191\u0001\u0193\u00cd\u0194\u00cf\u0196\u00d3\u0197\u00d1\u0198\u0001\u019c\u00d3\u019d\u00d5\u019f\u00d6\u81a4\u0001\u01a6\u00da\u01a7\u0001\u01a9\u00da\u01ac\u0001\u01ae\u00da\u01af\u0001\u01b2\u00d9\u81b5\u0001\u01b7\u00db\u01b8\u0001\u01bc\u0001\u01c4\u0002\u01c5\u0001\u01c7\u0002\u01c8\u0001\u01ca\u0002\u81db\u0001\u81ee\u0001\u01f1\u0002\u81f4\u0001\u01f6\uff9f\u01f7\uffc8\u821e\u0001\u8232\u0001\u0386&\u038a%\u038c@\u038f?\u03a1 \u03ab \u83ee\u0001\u040fP\u042f \u8480\u0001\u84be\u0001\u84c3\u0001\u04c7\u0001\u04cb\u0001\u84f4\u0001\u04f8\u0001\u05560\u9e94\u0001\u9ef8\u0001\u1f0f\ufff8\u1f1d\ufff8\u1f2f\ufff8\u1f3f\ufff8\u1f4d\ufff8\u9f5f\ufff8\u1f6f\ufff8\u1f8f\ufff8\u1f9f\ufff8\u1faf\ufff8\u1fb9\ufff8\u1fbb\uffb6\u1fbc\ufff7\u1fcb\uffaa\u1fcc\ufff7\u1fd9\ufff8\u1fdb\uff9c\u1fe9\ufff8\u1feb\uff90\u1fec\ufff9\u1ff9\uff80\u1ffb\uff82\u1ffc\ufff7\u2126\ue2a3\u212a\udf41\u212b\udfba\u216f\u0010\u24cf\u001a\uff3a "
                .ToCharArray();

        private static int[] lowercaseValuesCache = {
    	224, 225, 226, 227, 228, 229, 230, 231, 232, 233, 234, 235, 236, 237, 238, 239, 
    	240, 241, 242, 243, 244, 245, 246, 215, 248, 249, 250, 251, 252, 253, 254, 223, 
    	224, 225, 226, 227, 228, 229, 230, 231, 232, 233, 234, 235, 236, 237, 238, 239, 
    	240, 241, 242, 243, 244, 245, 246, 247, 248, 249, 250, 251, 252, 253, 254, 255, 
    	257, 257, 259, 259, 261, 261, 263, 263, 265, 265, 267, 267, 269, 269, 271, 271, 
    	273, 273, 275, 275, 277, 277, 279, 279, 281, 281, 283, 283, 285, 285, 287, 287, 
    	289, 289, 291, 291, 293, 293, 295, 295, 297, 297, 299, 299, 301, 301, 303, 303, 
    	105, 305, 307, 307, 309, 309, 311, 311, 312, 314, 314, 316, 316, 318, 318, 320, 
    	320, 322, 322, 324, 324, 326, 326, 328, 328, 329, 331, 331, 333, 333, 335, 335, 
    	337, 337, 339, 339, 341, 341, 343, 343, 345, 345, 347, 347, 349, 349, 351, 351, 
    	353, 353, 355, 355, 357, 357, 359, 359, 361, 361, 363, 363, 365, 365, 367, 367, 
    	369, 369, 371, 371, 373, 373, 375, 375, 255, 378, 378, 380, 380, 382, 382, 383, 
    	384, 595, 387, 387, 389, 389, 596, 392, 392, 598, 599, 396, 396, 397, 477, 601, 
    	603, 402, 402, 608, 611, 405, 617, 616, 409, 409, 410, 411, 623, 626, 414, 629, 
    	417, 417, 419, 419, 421, 421, 640, 424, 424, 643, 426, 427, 429, 429, 648, 432, 
    	432, 650, 651, 436, 436, 438, 438, 658, 441, 441, 442, 443, 445, 445, 446, 447, 
    	448, 449, 450, 451, 454, 454, 454, 457, 457, 457, 460, 460, 460, 462, 462, 464, 
    	464, 466, 466, 468, 468, 470, 470, 472, 472, 474, 474, 476, 476, 477, 479, 479, 
    	481, 481, 483, 483, 485, 485, 487, 487, 489, 489, 491, 491, 493, 493, 495, 495, 
    	496, 499, 499, 499, 501, 501, 405, 447, 505, 505, 507, 507, 509, 509, 511, 511, 
    	513, 513, 515, 515, 517, 517, 519, 519, 521, 521, 523, 523, 525, 525, 527, 527, 
    	529, 529, 531, 531, 533, 533, 535, 535, 537, 537, 539, 539, 541, 541, 543, 543, 
    	414, 545, 547, 547, 549, 549, 551, 551, 553, 553, 555, 555, 557, 557, 559, 559, 
    	561, 561, 563, 563, 564, 565, 566, 567, 568, 569, 570, 571, 572, 573, 574, 575, 
    	576, 577, 578, 579, 580, 581, 582, 583, 584, 585, 586, 587, 588, 589, 590, 591, 
    	592, 593, 594, 595, 596, 597, 598, 599, 600, 601, 602, 603, 604, 605, 606, 607, 
    	608, 609, 610, 611, 612, 613, 614, 615, 616, 617, 618, 619, 620, 621, 622, 623, 
    	624, 625, 626, 627, 628, 629, 630, 631, 632, 633, 634, 635, 636, 637, 638, 639, 
    	640, 641, 642, 643, 644, 645, 646, 647, 648, 649, 650, 651, 652, 653, 654, 655, 
    	656, 657, 658, 659, 660, 661, 662, 663, 664, 665, 666, 667, 668, 669, 670, 671, 
    	672, 673, 674, 675, 676, 677, 678, 679, 680, 681, 682, 683, 684, 685, 686, 687, 
    	688, 689, 690, 691, 692, 693, 694, 695, 696, 697, 698, 699, 700, 701, 702, 703, 
    	704, 705, 706, 707, 708, 709, 710, 711, 712, 713, 714, 715, 716, 717, 718, 719, 
    	720, 721, 722, 723, 724, 725, 726, 727, 728, 729, 730, 731, 732, 733, 734, 735, 
    	736, 737, 738, 739, 740, 741, 742, 743, 744, 745, 746, 747, 748, 749, 750, 751, 
    	752, 753, 754, 755, 756, 757, 758, 759, 760, 761, 762, 763, 764, 765, 766, 767, 
    	768, 769, 770, 771, 772, 773, 774, 775, 776, 777, 778, 779, 780, 781, 782, 783, 
    	784, 785, 786, 787, 788, 789, 790, 791, 792, 793, 794, 795, 796, 797, 798, 799, 
    	800, 801, 802, 803, 804, 805, 806, 807, 808, 809, 810, 811, 812, 813, 814, 815, 
    	816, 817, 818, 819, 820, 821, 822, 823, 824, 825, 826, 827, 828, 829, 830, 831, 
    	832, 833, 834, 835, 836, 837, 838, 839, 840, 841, 842, 843, 844, 845, 846, 847, 
    	848, 849, 850, 851, 852, 853, 854, 855, 856, 857, 858, 859, 860, 861, 862, 863, 
    	864, 865, 866, 867, 868, 869, 870, 871, 872, 873, 874, 875, 876, 877, 878, 879, 
    	880, 881, 882, 883, 884, 885, 886, 887, 888, 889, 890, 891, 892, 893, 894, 895, 
    	896, 897, 898, 899, 900, 901, 940, 903, 941, 942, 943, 907, 972, 909, 973, 974, 
    	912, 945, 946, 947, 948, 949, 950, 951, 952, 953, 954, 955, 956, 957, 958, 959, 
    	960, 961, 930, 963, 964, 965, 966, 967, 968, 969, 970, 971, 940, 941, 942, 943, 
    	944, 945, 946, 947, 948, 949, 950, 951, 952, 953, 954, 955, 956, 957, 958, 959, 
    	960, 961, 962, 963, 964, 965, 966, 967, 968, 969, 970, 971, 972, 973, 974, 975, 
    	976, 977, 978, 979, 980, 981, 982, 983, 985, 985, 987, 987, 989, 989, 991, 991, 
    	993, 993, 995, 995, 997, 997, 999, 999};

        /*  public static char LINE_SEPARATOR = '\n';
          public const int MAX_RADIX = 36;
          public static int MIN_SURROGATE;
          public static Type NON_SPACING_MARK;*/

        private static int ISJAVASTART = 1;

        private static int ISJAVAPART = 2;

        /// <summary>
        ///         
        /// </summary>
        /// <param name="ch"></param>
        /// <returns></returns>
        public static bool IsCSharpIdentifierStart(char ch)
        {
            return char.IsLetter(ch) || ch.Equals('_');
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ch"></param>
        /// <returns></returns>
        public static bool IsCSharpIdentifierPart(char ch)
        {
            return char.IsLetterOrDigit(ch) || ch.Equals('_');
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ch"></param>
        /// <param name="radix"></param>
        /// <returns></returns>
        public static int Digit(char ch, int radix)
        {
            try
            {
                return (char)Convert.ToInt32(new string(ch, 1), 16);
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public static int CharCount(int codePoint)
        {
            return (codePoint >= 0x10000 ? 2 : 1);
        }

        public static int CodePointAt(char[] seq, int index)
        {
            if (seq == null)
            {
                throw new NullReferenceException();
            }
            int len = seq.Length;
            if (index < 0 || index >= len)
            {
                throw new IndexOutOfRangeException();
            }

            char high = seq[index++];
            if (index >= len)
            {
                return high;
            }
            char low = seq[index];
            if (IsSurrogatePair(high, low))
            {
                return ToCodePoint(high, low);
            }
            return high;
        }

        public static bool IsHighSurrogate(char ch)
        {
            return (MIN_HIGH_SURROGATE <= ch && MAX_HIGH_SURROGATE >= ch);
        }

        public static bool IsLowSurrogate(char ch)
        {
            return (MIN_LOW_SURROGATE <= ch && MAX_LOW_SURROGATE >= ch);
        }

        public static bool IsSurrogatePair(char high, char low)
        {
            return (IsHighSurrogate(high) && IsLowSurrogate(low));
        }

        public static bool IsDefined(char c)
        {
            return GetType(c) != UNASSIGNED;
        }

        public static bool IsDefined(int codePoint)
        {
            throw new NotImplementedException();
        }

        public static bool IsMirrored(char c)
        {
            int value = c / 16;
            if (value >= mirrored.Length)
            {
                return false;
            }
            int bit = 1 << (c % 16);
            return (mirrored[value] & bit) != 0;
        }

        public static bool IsIdentifierIgnorable(char c)
        {
            return (c >= 0 && c <= 8) || (c >= 0xe && c <= 0x1b)
                 || (c >= 0x7f && c <= 0x9f) || GetType(c) == FORMAT;
        }

        public static bool IsISOControl(char c)
        {
            return IsISOControl((int)c);
        }

        public static bool IsISOControl(int c)
        {
            return (c >= 0 && c <= 0x1f) || (c >= 0x7f && c <= 0x9f);
        }

        public static bool IsJavaIdentifierPart(char c)
        {
            // Optimized case for ASCII
            if (c < 128)
            {
                return (typeTags[c] & ISJAVAPART) != 0;
            }

            int type = GetType(c);
            return (type >= UPPERCASE_LETTER && type <= OTHER_LETTER)
                    || type == CURRENCY_SYMBOL || type == CONNECTOR_PUNCTUATION
                    || (type >= DECIMAL_DIGIT_NUMBER && type <= LETTER_NUMBER)
                    || type == NON_SPACING_MARK || type == COMBINING_SPACING_MARK
                    || (c >= 0x80 && c <= 0x9f) || type == FORMAT;
        }

        public static bool IsJavaIdentifierStart(char c)
        {
            // Optimized case for ASCII
            if (c < 128)
            {
                return (typeTags[c] & ISJAVASTART) != 0;
            }

            int type = GetType(c);
            return (type >= UPPERCASE_LETTER && type <= OTHER_LETTER)
                    || type == CURRENCY_SYMBOL || type == CONNECTOR_PUNCTUATION
                    || type == LETTER_NUMBER;
        }

        public static bool IsLetter(char c)
        {
            if (('A' <= c && c <= 'Z') || ('a' <= c && c <= 'z'))
            {
                return true;
            }
            if (c < 128)
            {
                return false;
            }
            int type = GetType(c);
            return type >= UPPERCASE_LETTER && type <= OTHER_LETTER;
        }

        public static bool IsLetterOrDigit(char c)
        {
            int type = GetType(c);
            return (type >= UPPERCASE_LETTER && type <= OTHER_LETTER)
                    || type == DECIMAL_DIGIT_NUMBER;
        }

        public static bool IsSupplementaryCodePoint(int codePoint)
        {
            return (MIN_SUPPLEMENTARY_CODE_POINT <= codePoint && MAX_CODE_POINT >= codePoint);
        }

        public static bool IsValidCodePoint(int codePoint)
        {
            return (MIN_CODE_POINT <= codePoint && MAX_CODE_POINT >= codePoint);
        }

        public static char[] ToChars(int codePoint)
        {
            if (!IsValidCodePoint(codePoint))
            {
                throw new Exception();
            }

            if (IsSupplementaryCodePoint(codePoint))
            {
                int cpPrime = codePoint - 0x10000;
                int high = 0xD800 | ((cpPrime >> 10) & 0x3FF);
                int low = 0xDC00 | (cpPrime & 0x3FF);
                return new char[] { (char)high, (char)low };
            }
            return new char[] { (char)codePoint };
        }

        public static bool IsLowerCase(char c)
        {
            // Optimized case for ASCII
            if ('a' <= c && c <= 'z')
            {
                return true;
            }
            if (c < 128)
            {
                return false;
            }

            return GetType(c) == LOWERCASE_LETTER;
        }

        public static char ToUpperCase(char c)
        {
            // Optimized case for ASCII
            if ('a' <= c && c <= 'z')
            {
                return (char)(c - ('a' - 'A'));
            }
            if (c < 181)
            {
                return c;
            }
            if (c < 1000)
            {
                return (char)uppercaseValuesCache[(int)c - 181];
            }
            int result = BinarySearch.SearchRange(uppercaseKeys, c);
            if (result >= 0)
            {
                bool by2 = false;
                char start = uppercaseKeys[result];
                int end = uppercaseValues[result * 2];
                if ((start & 0x8000) != (end & 0x8000))
                {
                    end ^= 0x8000;
                    by2 = true;
                }
                if (c <= end)
                {
                    if (by2 && (c & 1) != (start & 1))
                    {
                        return c;
                    }
                    char mapping = uppercaseValues[result * 2 + 1];
                    return (char)(c + mapping);
                }
            }
            return c;
        }

        public static int ToUpperCase(int codePoint)
        {
            throw new NotImplementedException();
        }

        public static bool IsSpaceChar(char c)
        {
            if (c == 0x20 || c == 0xa0 || c == 0x1680)
            {
                return true;
            }
            if (c < 0x2000)
            {
                return false;
            }
            return c <= 0x200b || c == 0x2028 || c == 0x2029 || c == 0x202f
                    || c == 0x3000;
        }

        public static bool IsTitleCase(char c)
        {
            if (c == '\u01c5' || c == '\u01c8' || c == '\u01cb' || c == '\u01f2')
            {
                return true;
            }
            if (c >= '\u1f88' && c <= '\u1ffc')
            {
                // 0x1f88 - 0x1f8f, 0x1f98 - 0x1f9f, 0x1fa8 - 0x1faf
                if (c > '\u1faf')
                {
                    return c == '\u1fbc' || c == '\u1fcc' || c == '\u1ffc';
                }
                int last = c & 0xf;
                return last >= 8 && last <= 0xf;
            }
            return false;
        }

        public static bool IsUnicodeIdentifierPart(char c)
        {
            int type = GetType(c);
            return (type >= UPPERCASE_LETTER && type <= OTHER_LETTER)
                    || type == CONNECTOR_PUNCTUATION
                    || (type >= DECIMAL_DIGIT_NUMBER && type <= LETTER_NUMBER)
                    || type == NON_SPACING_MARK || type == COMBINING_SPACING_MARK
                    || IsIdentifierIgnorable(c);

        }

        public static int GetType(char c)
        {
            if (c < 1000)
            {
                return typeValuesCache[(int)c];
            }
            int result = BinarySearch.SearchRange(typeKeys, c);
            int high = typeValues[result * 2];
            if (c <= high)
            {
                int code = typeValues[result * 2 + 1];
                if (code < 0x100)
                {
                    return code;
                }
                return (c & 1) == 1 ? code >> 8 : code & 0xff;
            }
            return UNASSIGNED;
        }

        public static bool IsUnicodeIdentifierStart(char c)
        {
            int type = GetType(c);
            return (type >= UPPERCASE_LETTER && type <= OTHER_LETTER)
                    || type == LETTER_NUMBER;
        }

        public static bool IsUpperCase(char c)
        {
            // Optimized case for ASCII
            if ('A' <= c && c <= 'Z')
            {
                return true;
            }
            if (c < 128)
            {
                return false;
            }

            return GetType(c) == UPPERCASE_LETTER;
        }

        public static bool IsWhitespace(char c)
        {
            // Optimized case for ASCII
            if ((c >= 0x1c && c <= 0x20) || (c >= 0x9 && c <= 0xd))
            {
                return true;
            }
            if (c == 0x1680)
            {
                return true;
            }
            if (c < 0x2000 || c == 0x2007)
            {
                return false;
            }
            return c <= 0x200b || c == 0x2028 || c == 0x2029 || c == 0x3000;
        }

        public static int ToCodePoint(char high, char low)
        {
            // See RFC 2781, Section 2.2
            // http://www.faqs.org/rfcs/rfc2781.html
            int h = (high & 0x3FF) << 10;
            int l = low & 0x3FF;
            return (h | l) + 0x10000;
        }

        public static int ToLowerCase(int c)
        {
            throw new NotImplementedException();
        }

        public static char ToLowerCase(char c)
        {
            // Optimized case for ASCII
            if ('A' <= c && c <= 'Z')
            {
                return (char)(c + ('a' - 'A'));
            }
            if (c < 192)
            {// || c == 215 || (c > 222 && c < 256)) {
                return c;
            }
            if (c < 1000)
            {
                return (char)lowercaseValuesCache[c - 192];
            }

            int result = BinarySearch.SearchRange(lowercaseKeys, c);
            if (result >= 0)
            {
                bool by2 = false;
                char start = lowercaseKeys[result];
                int end = lowercaseValues[result * 2];
                if ((start & 0x8000) != (end & 0x8000))
                {
                    end ^= 0x8000;
                    by2 = true;
                }
                if (c <= end)
                {
                    if (by2 && (c & 1) != (start & 1))
                    {
                        return c;
                    }
                    char mapping = lowercaseValues[result * 2 + 1];
                    return (char)(c + mapping);
                }
            }
            return c;
        }

        public static bool IsIdentifierIgnorable(int c)
        {
            throw new NotImplementedException();
        }

        public static bool IsJavaIdentifierPart(int c)
        {
            throw new NotImplementedException();
        }

        public static bool IsJavaIdentifierStart(int c)
        {
            throw new NotImplementedException();
        }

        public static bool IsLetter(int c)
        {
            throw new NotImplementedException();
        }

        public static bool IsLetterOrDigit(int c)
        {
            throw new NotImplementedException();
        }

        public static bool IsLowerCase(int c)
        {
            throw new NotImplementedException();
        }

        public static bool IsMirrored(int c)
        {
            throw new NotImplementedException();
        }
        public static bool IsSpaceChar(int c)
        {
            throw new NotImplementedException();
        }

        public static bool IsTitleCase(int c)
        {
            throw new NotImplementedException();
        }
        public static bool IsUnicodeIdentifierPart(int c)
        {
            throw new NotImplementedException();
        }
        public static bool IsUnicodeIdentifierStart(int c)
        {
            throw new NotImplementedException();
        }
        public static bool IsUpperCase(int c)
        {
            throw new NotImplementedException();
        }
        public static bool IsWhitespace(int c)
        {
            throw new NotImplementedException();
        }

        public static char ForDigit(int digit, int radix)
        {
            if (MIN_RADIX <= radix && radix <= MAX_RADIX)
            {
                if (0 <= digit && digit < radix)
                {
                    return (char)(digit < 10 ? digit + '0' : digit + 'a' - 10);
                }
            }
            return (char) 0;
        }

    }

    public class BinarySearch
    {

        /**
         * Search the sorted characters in the string and return an exact index or
         * -1.
         * 
         * @param data
         *            the String to search
         * @param value
         *            the character to search for
         * @return the matching index, or -1
         */
        public static int Search(String data, char value)
        {
            int low = 0, high = data.Length - 1;
            while (low <= high)
            {
                int mid = (low + high) >> 1;
                char target = data[mid];
                if (value == target)
                    return mid;
                else if (value < target)
                    high = mid - 1;
                else
                    low = mid + 1;
            }
            return -1;
        }

        /**
         * Search the sorted characters in the string and return the nearest index.
         * 
         * @param data
         *            the String to search
         * @param c
         *            the character to search for
         * @return the nearest index
         */
        public static int SearchRange(String data, char c)
        {
            int value = 0;
            int low = 0, mid = -1, high = data.Length - 1;
            while (low <= high)
            {
                mid = (low + high) >> 1;
                value = data[mid];
                if (c > value)
                    low = mid + 1;
                else if (c == value)
                    return mid;
                else
                    high = mid - 1;
            }
            return mid - (c < value ? 1 : 0);
        }
    }
}
