// --------------------------------------------------------------------------------------------------
// This file was automatically generated by J2CS Translator (http://j2cstranslator.sourceforge.net/). 
// Version 1.3.6.20101125_01     
// 12/2/10 2:05 PM    
// ${CustomMessageForDisclaimer}                                                                             
// --------------------------------------------------------------------------------------------------
 /// <summary>
/// Copyright (C) 1996-2004, International Business Machines Corporation and    
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
	/// Enumerated Unicode category types from the UnicodeData.txt file. Used as
	/// return results from <a href=UCharacter.html>UCharacter</a> Equivalent to
	/// icu's UCharCategory. Refer to <a
	/// href="http://www.unicode.org/Public/UNIDATA/UCD.html"> Unicode Consortium</a>
	/// for more information about UnicodeData.txt.
	/// <p>
	/// <em>NOTE:</em> the UCharacterCategory values are <em>not</em> compatible with
	/// those returned by java.lang.Character.getType. UCharacterCategory values
	/// match the ones used in ICU4C, while java.lang.Character type values, though
	/// similar, skip the value 17.
	/// </p>
	/// <p>
	/// This class is not subclassable
	/// </p>
	/// </summary>
	///
	/// @stable ICU 2.1
	
	public sealed class UCharacterCategory : IBM.ICU.Lang.UCharacterEnums.ECharacterCategory  {
	    /// <summary>
	    /// Gets the name of the argument category
	    /// </summary>
	    ///
	    /// <param name="category">to retrieve name</param>
	    /// <returns>category name</returns>
	    /// @stable ICU 2.1
	    public static String ToString(int category) {
	        switch (category) {
	        case IBM.ICU.Lang.UCharacterEnums.ECharacterCategory.UPPERCASE_LETTER:
	            return "Letter, Uppercase";
	        case IBM.ICU.Lang.UCharacterEnums.ECharacterCategory.LOWERCASE_LETTER:
	            return "Letter, Lowercase";
	        case IBM.ICU.Lang.UCharacterEnums.ECharacterCategory.TITLECASE_LETTER:
	            return "Letter, Titlecase";
	        case IBM.ICU.Lang.UCharacterEnums.ECharacterCategory.MODIFIER_LETTER:
	            return "Letter, Modifier";
	        case IBM.ICU.Lang.UCharacterEnums.ECharacterCategory.OTHER_LETTER:
	            return "Letter, Other";
	        case IBM.ICU.Lang.UCharacterEnums.ECharacterCategory.NON_SPACING_MARK:
	            return "Mark, Non-Spacing";
	        case IBM.ICU.Lang.UCharacterEnums.ECharacterCategory.ENCLOSING_MARK:
	            return "Mark, Enclosing";
	        case IBM.ICU.Lang.UCharacterEnums.ECharacterCategory.COMBINING_SPACING_MARK:
	            return "Mark, Spacing Combining";
	        case IBM.ICU.Lang.UCharacterEnums.ECharacterCategory.DECIMAL_DIGIT_NUMBER:
	            return "Number, Decimal Digit";
	        case IBM.ICU.Lang.UCharacterEnums.ECharacterCategory.LETTER_NUMBER:
	            return "Number, Letter";
	        case IBM.ICU.Lang.UCharacterEnums.ECharacterCategory.OTHER_NUMBER:
	            return "Number, Other";
	        case IBM.ICU.Lang.UCharacterEnums.ECharacterCategory.SPACE_SEPARATOR:
	            return "Separator, Space";
	        case IBM.ICU.Lang.UCharacterEnums.ECharacterCategory.LINE_SEPARATOR:
	            return "Separator, Line";
	        case IBM.ICU.Lang.UCharacterEnums.ECharacterCategory.PARAGRAPH_SEPARATOR:
	            return "Separator, Paragraph";
	        case IBM.ICU.Lang.UCharacterEnums.ECharacterCategory.CONTROL:
	            return "Other, Control";
	        case IBM.ICU.Lang.UCharacterEnums.ECharacterCategory.FORMAT:
	            return "Other, Formatting";
	        case IBM.ICU.Lang.UCharacterEnums.ECharacterCategory.PRIVATE_USE:
	            return "Other, Private Use";
	        case IBM.ICU.Lang.UCharacterEnums.ECharacterCategory.SURROGATE:
	            return "Other, Surrogate";
	        case IBM.ICU.Lang.UCharacterEnums.ECharacterCategory.DASH_PUNCTUATION:
	            return "Punctuation, Dash";
	        case IBM.ICU.Lang.UCharacterEnums.ECharacterCategory.START_PUNCTUATION:
	            return "Punctuation, Open";
	        case IBM.ICU.Lang.UCharacterEnums.ECharacterCategory.END_PUNCTUATION:
	            return "Punctuation, Close";
	        case IBM.ICU.Lang.UCharacterEnums.ECharacterCategory.CONNECTOR_PUNCTUATION:
	            return "Punctuation, Connector";
	        case IBM.ICU.Lang.UCharacterEnums.ECharacterCategory.OTHER_PUNCTUATION:
	            return "Punctuation, Other";
	        case IBM.ICU.Lang.UCharacterEnums.ECharacterCategory.MATH_SYMBOL:
	            return "Symbol, Math";
	        case IBM.ICU.Lang.UCharacterEnums.ECharacterCategory.CURRENCY_SYMBOL:
	            return "Symbol, Currency";
	        case IBM.ICU.Lang.UCharacterEnums.ECharacterCategory.MODIFIER_SYMBOL:
	            return "Symbol, Modifier";
	        case IBM.ICU.Lang.UCharacterEnums.ECharacterCategory.OTHER_SYMBOL:
	            return "Symbol, Other";
	        case IBM.ICU.Lang.UCharacterEnums.ECharacterCategory.INITIAL_PUNCTUATION:
	            return "Punctuation, Initial quote";
	        case IBM.ICU.Lang.UCharacterEnums.ECharacterCategory.FINAL_PUNCTUATION:
	            return "Punctuation, Final quote";
	        }
	        return "Unassigned";
	    }
	
	    // private constructor -----------------------------------------------
	    // /CLOVER:OFF
	    /// <summary>
	    /// Private constructor to prevent initialisation
	    /// </summary>
	    ///
	    private UCharacterCategory() {
	    }
	    // /CLOVER:ON
	}
}
