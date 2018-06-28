// --------------------------------------------------------------------------------------------------
// This file was automatically generated by J2CS Translator (http://j2cstranslator.sourceforge.net/). 
// Version 1.3.6.20101125_01     
// 12/2/10 2:05 PM    
// ${CustomMessageForDisclaimer}                                                                             
// --------------------------------------------------------------------------------------------------
 /// <summary>
/// Copyright (C) 2004-2007, International Business Machines Corporation and    
/// others. All Rights Reserved.                                                
/// </summary>
///
namespace IBM.ICU.Lang {
	
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.IO;
	using System.Runtime.CompilerServices;
	
	/// <summary>
	/// A container for the different 'enumerated types' used by UCharacter.
	/// </summary>
	///
	/// @stable ICU 3.0
	public class UCharacterEnums {
	
	    /// <summary>
	    /// This is just a namespace, it is not instantiatable. 
	    /// </summary>
	    ///
	    // /CLOVER:OFF
	    private UCharacterEnums() {
	    }
	
	    /// <summary>
	    /// 'Enum' for the CharacterCategory constants. These constants are
	    /// compatible in name <b>but not in value</b> with those defined in
	    /// <c>java.lang.Character</c>.
	    /// </summary>
	    ///
	    /// <seealso cref="T:IBM.ICU.Lang.UCharacterCategory"/>
	    /// @stable ICU 3.0
	    public class ECharacterCategory {
	        /// <summary>
	        /// Unassigned character type
	        /// </summary>
	        ///
	        /// @stable ICU 2.1
            public const byte UNASSIGNED = 0;
	
	        /// <summary>
	        /// Character type Cn Not Assigned (no characters in [UnicodeData.txt]
	        /// have this property)
	        /// </summary>
	        ///
	        /// @stable ICU 2.6
            public const byte GENERAL_OTHER_TYPES = 0;
	
	        /// <summary>
	        /// Character type Lu
	        /// </summary>
	        ///
	        /// @stable ICU 2.1
            public const byte UPPERCASE_LETTER = 1;
	
	        /// <summary>
	        /// Character type Ll
	        /// </summary>
	        ///
	        /// @stable ICU 2.1
            public const byte LOWERCASE_LETTER = 2;
	
	        /// <summary>
	        /// Character type Lt
	        /// </summary>
	        ///
	        /// @stable ICU 2.1

            public const byte TITLECASE_LETTER = 3;
	
	        /// <summary>
	        /// Character type Lm
	        /// </summary>
	        ///
	        /// @stable ICU 2.1
            public const byte MODIFIER_LETTER = 4;
	
	        /// <summary>
	        /// Character type Lo
	        /// </summary>
	        ///
	        /// @stable ICU 2.1
            public const byte OTHER_LETTER = 5;
	
	        /// <summary>
	        /// Character type Mn
	        /// </summary>
	        ///
	        /// @stable ICU 2.1
            public const byte NON_SPACING_MARK = 6;
	
	        /// <summary>
	        /// Character type Me
	        /// </summary>
	        ///
	        /// @stable ICU 2.1
            public const byte ENCLOSING_MARK = 7;
	
	        /// <summary>
	        /// Character type Mc
	        /// </summary>
	        ///
	        /// @stable ICU 2.1
            public const byte COMBINING_SPACING_MARK = 8;
	
	        /// <summary>
	        /// Character type Nd
	        /// </summary>
	        ///
	        /// @stable ICU 2.1
            public const byte DECIMAL_DIGIT_NUMBER = 9;
	
	        /// <summary>
	        /// Character type Nl
	        /// </summary>
	        ///
	        /// @stable ICU 2.1
            public const byte LETTER_NUMBER = 10;
	
	        /// <summary>
	        /// Character type No
	        /// </summary>
	        ///
	        /// @stable ICU 2.1
            public const byte OTHER_NUMBER = 11;
	
	        /// <summary>
	        /// Character type Zs
	        /// </summary>
	        ///
	        /// @stable ICU 2.1
            public const byte SPACE_SEPARATOR = 12;
	
	        /// <summary>
	        /// Character type Zl
	        /// </summary>
	        ///
	        /// @stable ICU 2.1
            public const byte LINE_SEPARATOR = 13;
	
	        /// <summary>
	        /// Character type Zp
	        /// </summary>
	        ///
	        /// @stable ICU 2.1
            public const byte PARAGRAPH_SEPARATOR = 14;
	
	        /// <summary>
	        /// Character type Cc
	        /// </summary>
	        ///
	        /// @stable ICU 2.1
            public const byte CONTROL = 15;
	
	        /// <summary>
	        /// Character type Cf
	        /// </summary>
	        ///
	        /// @stable ICU 2.1
            public const byte FORMAT = 16;
	
	        /// <summary>
	        /// Character type Co
	        /// </summary>
	        ///
	        /// @stable ICU 2.1
            public const byte PRIVATE_USE = 17;
	
	        /// <summary>
	        /// Character type Cs
	        /// </summary>
	        ///
	        /// @stable ICU 2.1
            public const byte SURROGATE = 18;
	
	        /// <summary>
	        /// Character type Pd
	        /// </summary>
	        ///
	        /// @stable ICU 2.1
            public const byte DASH_PUNCTUATION = 19;
	
	        /// <summary>
	        /// Character type Ps
	        /// </summary>
	        ///
	        /// @stable ICU 2.1
            public const byte START_PUNCTUATION = 20;
	
	        /// <summary>
	        /// Character type Pe
	        /// </summary>
	        ///
	        /// @stable ICU 2.1
            public const byte END_PUNCTUATION = 21;
	
	        /// <summary>
	        /// Character type Pc
	        /// </summary>
	        ///
	        /// @stable ICU 2.1
            public const byte CONNECTOR_PUNCTUATION = 22;
	
	        /// <summary>
	        /// Character type Po
	        /// </summary>
	        ///
	        /// @stable ICU 2.1
            public const byte OTHER_PUNCTUATION = 23;
	
	        /// <summary>
	        /// Character type Sm
	        /// </summary>
	        ///
	        /// @stable ICU 2.1
            public const byte MATH_SYMBOL = 24;
	
	        /// <summary>
	        /// Character type Sc
	        /// </summary>
	        ///
	        /// @stable ICU 2.1
            public const byte CURRENCY_SYMBOL = 25;
	
	        /// <summary>
	        /// Character type Sk
	        /// </summary>
	        ///
	        /// @stable ICU 2.1
            public const byte MODIFIER_SYMBOL = 26;
	
	        /// <summary>
	        /// Character type So
	        /// </summary>
	        ///
	        /// @stable ICU 2.1
            public const byte OTHER_SYMBOL = 27;
	
	        /// <summary>
	        /// Character type Pi
	        /// </summary>
	        ///
	        /// <seealso cref="null"/>
	        /// @stable ICU 2.1
            public const byte INITIAL_PUNCTUATION = 28;
	
	        /// <summary>
	        /// Character type Pi This name is compatible with java.lang.Character's
	        /// name for this type.
	        /// </summary>
	        ///
	        /// <seealso cref="null"/>
	        /// @stable ICU 2.8
	        const byte INITIAL_QUOTE_PUNCTUATION = 28;
	
	        /// <summary>
	        /// Character type Pf
	        /// </summary>
	        ///
	        /// <seealso cref="null"/>
	        /// @stable ICU 2.1
            public const byte FINAL_PUNCTUATION = 29;
	
	        /// <summary>
	        /// Character type Pf This name is compatible with java.lang.Character's
	        /// name for this type.
	        /// </summary>
	        ///
	        /// <seealso cref="null"/>
	        /// @stable ICU 2.8
            public const byte FINAL_QUOTE_PUNCTUATION = 29;
	
	        /// <summary>
	        /// Character type count
	        /// </summary>
	        ///
	        /// @stable ICU 2.1
	        public const byte CHAR_CATEGORY_COUNT = 30;
	    }
	
	    /// <summary>
	    /// 'Enum' for the CharacterDirection constants. There are two sets of names,
	    /// those used in ICU, and those used in the JDK. The JDK constants are
	    /// compatible in name <b>but not in value</b> with those defined in
	    /// <c>java.lang.Character</c>.
	    /// </summary>
	    ///
	    /// <seealso cref="T:IBM.ICU.Lang.UCharacterDirection"/>
	    /// @stable ICU 3.0
        public class ECharacterDirection
        {
	        /// <summary>
	        /// Directional type L
	        /// </summary>
	        ///
	        /// @stable ICU 2.1
	        public const int LEFT_TO_RIGHT = 0;
	
	        /// <summary>
	        /// JDK-compatible synonym for LEFT_TO_RIGHT.
	        /// </summary>
	        ///
	        /// @stable ICU 3.0
            public const byte DIRECTIONALITY_LEFT_TO_RIGHT = (byte)LEFT_TO_RIGHT;
	
	        /// <summary>
	        /// Directional type R
	        /// </summary>
	        ///
	        /// @stable ICU 2.1
            public const int RIGHT_TO_LEFT = 1;
	
	        /// <summary>
	        /// JDK-compatible synonym for RIGHT_TO_LEFT.
	        /// </summary>
	        ///
	        /// @stable ICU 3.0
            public const byte DIRECTIONALITY_RIGHT_TO_LEFT = (byte)RIGHT_TO_LEFT;
	
	        /// <summary>
	        /// Directional type EN
	        /// </summary>
	        ///
	        /// @stable ICU 2.1
            public const int EUROPEAN_NUMBER = 2;
	
	        /// <summary>
	        /// JDK-compatible synonym for EUROPEAN_NUMBER.
	        /// </summary>
	        ///
	        /// @stable ICU 3.0
            public const byte DIRECTIONALITY_EUROPEAN_NUMBER = (byte)EUROPEAN_NUMBER;
	
	        /// <summary>
	        /// Directional type ES
	        /// </summary>
	        ///
	        /// @stable ICU 2.1
            public const int EUROPEAN_NUMBER_SEPARATOR = 3;
	
	        /// <summary>
	        /// JDK-compatible synonym for EUROPEAN_NUMBER_SEPARATOR.
	        /// </summary>
	        ///
	        /// @stable ICU 3.0
            public const byte DIRECTIONALITY_EUROPEAN_NUMBER_SEPARATOR = (byte)EUROPEAN_NUMBER_SEPARATOR;
	
	        /// <summary>
	        /// Directional type ET
	        /// </summary>
	        ///
	        /// @stable ICU 2.1
            public const int EUROPEAN_NUMBER_TERMINATOR = 4;
	
	        /// <summary>
	        /// JDK-compatible synonym for EUROPEAN_NUMBER_TERMINATOR.
	        /// </summary>
	        ///
	        /// @stable ICU 3.0
            public const byte DIRECTIONALITY_EUROPEAN_NUMBER_TERMINATOR = (byte)EUROPEAN_NUMBER_TERMINATOR;
	
	        /// <summary>
	        /// Directional type AN
	        /// </summary>
	        ///
	        /// @stable ICU 2.1
            public const int ARABIC_NUMBER = 5;
	
	        /// <summary>
	        /// JDK-compatible synonym for ARABIC_NUMBER.
	        /// </summary>
	        ///
	        /// @stable ICU 3.0
            public const byte DIRECTIONALITY_ARABIC_NUMBER = (byte)ARABIC_NUMBER;
	
	        /// <summary>
	        /// Directional type CS
	        /// </summary>
	        ///
	        /// @stable ICU 2.1
            public const int COMMON_NUMBER_SEPARATOR = 6;
	
	        /// <summary>
	        /// JDK-compatible synonym for COMMON_NUMBER_SEPARATOR.
	        /// </summary>
	        ///
	        /// @stable ICU 3.0
            public const byte DIRECTIONALITY_COMMON_NUMBER_SEPARATOR = (byte)COMMON_NUMBER_SEPARATOR;
	
	        /// <summary>
	        /// Directional type B
	        /// </summary>
	        ///
	        /// @stable ICU 2.1
            public const int BLOCK_SEPARATOR = 7;
	
	        /// <summary>
	        /// JDK-compatible synonym for BLOCK_SEPARATOR.
	        /// </summary>
	        ///
	        /// @stable ICU 3.0
            public const byte DIRECTIONALITY_PARAGRAPH_SEPARATOR = (byte)BLOCK_SEPARATOR;
	
	        /// <summary>
	        /// Directional type S
	        /// </summary>
	        ///
	        /// @stable ICU 2.1
            public const int SEGMENT_SEPARATOR = 8;
	
	        /// <summary>
	        /// JDK-compatible synonym for SEGMENT_SEPARATOR.
	        /// </summary>
	        ///
	        /// @stable ICU 3.0
            public const byte DIRECTIONALITY_SEGMENT_SEPARATOR = (byte)SEGMENT_SEPARATOR;
	
	        /// <summary>
	        /// Directional type WS
	        /// </summary>
	        ///
	        /// @stable ICU 2.1
            public const int WHITE_SPACE_NEUTRAL = 9;
	
	        /// <summary>
	        /// JDK-compatible synonym for WHITE_SPACE_NEUTRAL.
	        /// </summary>
	        ///
	        /// @stable ICU 3.0
            public const byte DIRECTIONALITY_WHITESPACE = (byte)WHITE_SPACE_NEUTRAL;
	
	        /// <summary>
	        /// Directional type ON
	        /// </summary>
	        ///
	        /// @stable ICU 2.1
            public const int OTHER_NEUTRAL = 10;
	
	        /// <summary>
	        /// JDK-compatible synonym for OTHER_NEUTRAL.
	        /// </summary>
	        ///
	        /// @stable ICU 3.0
            public const byte DIRECTIONALITY_OTHER_NEUTRALS = (byte)OTHER_NEUTRAL;
	
	        /// <summary>
	        /// Directional type LRE
	        /// </summary>
	        ///
	        /// @stable ICU 2.1
            public const int LEFT_TO_RIGHT_EMBEDDING = 11;
	
	        /// <summary>
	        /// JDK-compatible synonym for LEFT_TO_RIGHT_EMBEDDING.
	        /// </summary>
	        ///
	        /// @stable ICU 3.0
            public const byte DIRECTIONALITY_LEFT_TO_RIGHT_EMBEDDING = (byte)LEFT_TO_RIGHT_EMBEDDING;
	
	        /// <summary>
	        /// Directional type LRO
	        /// </summary>
	        ///
	        /// @stable ICU 2.1
            public const int LEFT_TO_RIGHT_OVERRIDE = 12;
	
	        /// <summary>
	        /// JDK-compatible synonym for LEFT_TO_RIGHT_OVERRIDE.
	        /// </summary>
	        ///
	        /// @stable ICU 3.0
            public const byte DIRECTIONALITY_LEFT_TO_RIGHT_OVERRIDE = (byte)LEFT_TO_RIGHT_OVERRIDE;
	
	        /// <summary>
	        /// Directional type AL
	        /// </summary>
	        ///
	        /// @stable ICU 2.1
            public const int RIGHT_TO_LEFT_ARABIC = 13;
	
	        /// <summary>
	        /// JDK-compatible synonym for RIGHT_TO_LEFT_ARABIC.
	        /// </summary>
	        ///
	        /// @stable ICU 3.0
            public const byte DIRECTIONALITY_RIGHT_TO_LEFT_ARABIC = (byte)RIGHT_TO_LEFT_ARABIC;
	
	        /// <summary>
	        /// Directional type RLE
	        /// </summary>
	        ///
	        /// @stable ICU 2.1
            public const int RIGHT_TO_LEFT_EMBEDDING = 14;
	
	        /// <summary>
	        /// JDK-compatible synonym for RIGHT_TO_LEFT_EMBEDDING.
	        /// </summary>
	        ///
	        /// @stable ICU 3.0
            public const byte DIRECTIONALITY_RIGHT_TO_LEFT_EMBEDDING = (byte)RIGHT_TO_LEFT_EMBEDDING;
	
	        /// <summary>
	        /// Directional type RLO
	        /// </summary>
	        ///
	        /// @stable ICU 2.1
            public const int RIGHT_TO_LEFT_OVERRIDE = 15;
	
	        /// <summary>
	        /// JDK-compatible synonym for RIGHT_TO_LEFT_OVERRIDE.
	        /// </summary>
	        ///
	        /// @stable ICU 3.0
            public const byte DIRECTIONALITY_RIGHT_TO_LEFT_OVERRIDE = (byte)RIGHT_TO_LEFT_OVERRIDE;
	
	        /// <summary>
	        /// Directional type PDF
	        /// </summary>
	        ///
	        /// @stable ICU 2.1
            public const int POP_DIRECTIONAL_FORMAT = 16;
	
	        /// <summary>
	        /// JDK-compatible synonym for POP_DIRECTIONAL_FORMAT.
	        /// </summary>
	        ///
	        /// @stable ICU 3.0
            public const byte DIRECTIONALITY_POP_DIRECTIONAL_FORMAT = (byte)POP_DIRECTIONAL_FORMAT;
	
	        /// <summary>
	        /// Directional type NSM
	        /// </summary>
	        ///
	        /// @stable ICU 2.1
            public const int DIR_NON_SPACING_MARK = 17;
	
	        /// <summary>
	        /// JDK-compatible synonym for DIR_NON_SPACING_MARK.
	        /// </summary>
	        ///
	        /// @stable ICU 3.0
            public const byte DIRECTIONALITY_NONSPACING_MARK = (byte)DIR_NON_SPACING_MARK;
	
	        /// <summary>
	        /// Directional type BN
	        /// </summary>
	        ///
	        /// @stable ICU 2.1
            public const int BOUNDARY_NEUTRAL = 18;
	
	        /// <summary>
	        /// JDK-compatible synonym for BOUNDARY_NEUTRAL.
	        /// </summary>
	        ///
	        /// @stable ICU 3.0
            public const byte DIRECTIONALITY_BOUNDARY_NEUTRAL = (byte)BOUNDARY_NEUTRAL;
	
	        /// <summary>
	        /// Number of directional types
	        /// </summary>
	        ///
	        /// @stable ICU 2.1
	        public const int CHAR_DIRECTION_COUNT = 19;
	
	        /// <summary>
	        /// Undefined bidirectional character type. Undefined <c>char</c>
	        /// values have undefined directionality in the Unicode specification.
	        /// </summary>
	        ///
	        /// @stable ICU 3.0
            public const sbyte DIRECTIONALITY_UNDEFINED = -1;
	    }
	}
}