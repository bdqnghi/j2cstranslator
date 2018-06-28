// --------------------------------------------------------------------------------------------------
// This file was automatically generated by J2CS Translator (http://j2cstranslator.sourceforge.net/). 
// Version 1.3.6.20101125_01     
// 12/2/10 2:05 PM    
// ${CustomMessageForDisclaimer}                                                                             
// --------------------------------------------------------------------------------------------------
 /// <summary>
/// Copyright (C) 1996-2007, international Business Machines Corporation and    
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
	/// <p>
	/// Selection constants for Unicode properties.
	/// </p>
	/// <p>
	/// These constants are used in functions like UCharacter.hasBinaryProperty(int)
	/// to select one of the Unicode properties.
	/// </p>
	/// <p>
	/// The properties APIs are intended to reflect Unicode properties as defined in
	/// the Unicode Character Database (UCD) and Unicode Technical Reports (UTR).
	/// </p>
	/// <p>
	/// For details about the properties see <a href=http://www.unicode.org>
	/// http://www.unicode.org</a>.
	/// </p>
	/// <p>
	/// For names of Unicode properties see the UCD file PropertyAliases.txt.
	/// </p>
	/// <p>
	/// Important: If ICU is built with UCD files from Unicode versions below 3.2,
	/// then properties marked with "new" are not or not fully available. Check
	/// UCharacter.getUnicodeVersion() to be sure.
	/// </p>
	/// </summary>
	///
	/// @stable ICU 2.6
	/// <seealso cref="T:IBM.ICU.Lang.UCharacter"/>
	public interface UProperty {
	}
}