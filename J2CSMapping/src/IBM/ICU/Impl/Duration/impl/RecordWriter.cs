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
	using System.IO;
	using System.Runtime.CompilerServices;
	
	public interface RecordWriter {
	    bool Open(String title);
	
	    bool Close();
	
	    void Bool(String name, bool value_ren);
	
	    void BoolArray(String name, bool[] values);
	
	    void Character(String name, char value_ren);
	
	    void CharacterArray(String name, char[] values);
	
	    void NamedIndex(String name, String[] names, int value_ren);
	
	    void NamedIndexArray(String name, String[] names, sbyte[] values);
	
	    void String(String name, String value_ren);
	
	    void StringArray(String name, String[] values);
	
	    void StringTable(String name, String[][] values);
	}
}