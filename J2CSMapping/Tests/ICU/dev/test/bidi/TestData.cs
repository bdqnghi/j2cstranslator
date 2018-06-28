/*
 *******************************************************************************
 *   Copyright (C) 2001-2007, International Business Machines
 *   Corporation and others.  All Rights Reserved.
 *******************************************************************************
 */

// --------------------------------------------------------------------------------------------------
// This file was automatically generated by J2CS Translator (http://j2cstranslator.sourceforge.net/). 
// Version 1.3.6.20101125_01     
// 12/13/10 10:46 AM    
// ${CustomMessageForDisclaimer}                                                                             
// --------------------------------------------------------------------------------------------------
 namespace IBM.ICU.Charset {
	
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.IO;
	using System.Runtime.CompilerServices;
	
	
	/// <summary>
	/// Data and helper methods for Bidi regression tests
	/// Ported from C by Lina Kemmel, Matitiahu Allouche
	/// </summary>
	///
	public class TestData {
	    protected internal const int L = IBM.ICU.Lang.UCharacterEnums.ECharacterDirection.LEFT_TO_RIGHT;
	
	    protected internal const int R = IBM.ICU.Lang.UCharacterEnums.ECharacterDirection.RIGHT_TO_LEFT;
	
	    protected internal const int EN = IBM.ICU.Lang.UCharacterEnums.ECharacterDirection.EUROPEAN_NUMBER;
	
	    protected internal const int ES = IBM.ICU.Lang.UCharacterEnums.ECharacterDirection.EUROPEAN_NUMBER_SEPARATOR;
	
	    protected internal const int ET = IBM.ICU.Lang.UCharacterEnums.ECharacterDirection.EUROPEAN_NUMBER_TERMINATOR;
	
	    protected internal const int AN = IBM.ICU.Lang.UCharacterEnums.ECharacterDirection.ARABIC_NUMBER;
	
	    protected internal const int CS = IBM.ICU.Lang.UCharacterEnums.ECharacterDirection.COMMON_NUMBER_SEPARATOR;
	
	    protected internal const int B = IBM.ICU.Lang.UCharacterEnums.ECharacterDirection.BLOCK_SEPARATOR;
	
	    protected internal const int S = IBM.ICU.Lang.UCharacterEnums.ECharacterDirection.SEGMENT_SEPARATOR;
	
	    protected internal const int WS = IBM.ICU.Lang.UCharacterEnums.ECharacterDirection.WHITE_SPACE_NEUTRAL;
	
	    protected internal const int ON = IBM.ICU.Lang.UCharacterEnums.ECharacterDirection.OTHER_NEUTRAL;
	
	    protected internal const int LRE = IBM.ICU.Lang.UCharacterEnums.ECharacterDirection.LEFT_TO_RIGHT_EMBEDDING;
	
	    protected internal const int LRO = IBM.ICU.Lang.UCharacterEnums.ECharacterDirection.LEFT_TO_RIGHT_OVERRIDE;
	
	    protected internal const int AL = IBM.ICU.Lang.UCharacterEnums.ECharacterDirection.RIGHT_TO_LEFT_ARABIC;
	
	    protected internal const int RLE = IBM.ICU.Lang.UCharacterEnums.ECharacterDirection.RIGHT_TO_LEFT_EMBEDDING;
	
	    protected internal const int RLO = IBM.ICU.Lang.UCharacterEnums.ECharacterDirection.RIGHT_TO_LEFT_OVERRIDE;
	
	    protected internal const int PDF = IBM.ICU.Lang.UCharacterEnums.ECharacterDirection.POP_DIRECTIONAL_FORMAT;
	
	    protected internal const int NSM = IBM.ICU.Lang.UCharacterEnums.ECharacterDirection.DIR_NON_SPACING_MARK;
	
	    protected internal const int BN = IBM.ICU.Lang.UCharacterEnums.ECharacterDirection.BOUNDARY_NEUTRAL;
	
	    protected internal const int DEF = IBM.ICU.Text.Bidi.CLASS_DEFAULT;
	
	    protected static internal readonly String[] dirPropNames = { "L", "R", "EN", "ES",
	            "ET", "AN", "CS", "B", "S", "WS", "ON", "LRE", "LRO", "AL", "RLE",
	            "RLO", "PDF", "NSM", "BN" };
	
	    protected static internal readonly short[][] testDirProps = {
	            new short[] { L, L, WS, L, WS, EN, L, B },
	            new short[] { R, AL, WS, R, AL, WS, R },
	            new short[] { L, L, WS, EN, CS, WS, EN, CS, EN, WS, L, L },
	            new short[] { L, AL, AL, AL, L, AL, AL, L, WS, EN, CS, WS, EN, CS,
	                    EN, WS, L, L },
	            new short[] { AL, R, AL, WS, EN, CS, WS, EN, CS, EN, WS, R, R, WS,
	                    L, L },
	            new short[] { R, EN, NSM, ET },
	            new short[] { RLE, WS, R, R, R, WS, PDF, WS, B },
	            new short[] { LRE, LRE, LRE, LRE, LRE, LRE, LRE, LRE, LRE, LRE,
	                    LRE, LRE, LRE, LRE, LRE, LRE, LRE, LRE, LRE, LRE, LRE, LRE,
	                    LRE, LRE, LRE, LRE, LRE, LRE, LRE, LRE, AN, RLO, NSM, LRE,
	                    PDF, RLE, ES, EN, ON },
	            new short[] { LRE, LRE, LRE, LRE, LRE, LRE, LRE, LRE, LRE, LRE,
	                    LRE, LRE, LRE, LRE, LRE, LRE, LRE, LRE, LRE, LRE, LRE, LRE,
	                    LRE, LRE, LRE, LRE, LRE, LRE, LRE, LRE, LRE, BN, CS, RLO,
	                    S, PDF, EN, LRO, AN, ES },
	            new short[] { S, WS, NSM, RLE, WS, L, L, L, WS, LRO, WS, R, R, R,
	                    WS, RLO, WS, L, L, L, WS, LRE, WS, R, R, R, WS, PDF, WS, L,
	                    L, L, WS, PDF, WS, AL, AL, AL, WS, PDF, WS, L, L, L, WS,
	                    PDF, WS, L, L, L, WS, PDF, ON, PDF, BN, BN, ON, PDF },
	            new short[] { NSM, WS, L, L, L, L, L, L, L, WS, L, L, L, L, WS, R,
	                    R, R, R, R, WS, L, L, L, L, L, L, L, WS, WS, AL, AL, AL,
	                    AL, WS, EN, EN, ES, EN, EN, CS, S, EN, EN, CS, WS, EN, EN,
	                    WS, AL, AL, AL, AL, AL, B, L, L, L, L, L, L, L, L, WS, AN,
	                    AN, CS, AN, AN, WS },
	            new short[] { NSM, WS, L, L, L, L, L, L, L, WS, L, L, L, L, WS, R,
	                    R, R, R, R, WS, L, L, L, L, L, L, L, WS, WS, AL, AL, AL,
	                    AL, WS, EN, EN, ES, EN, EN, CS, S, EN, EN, CS, WS, EN, EN,
	                    WS, AL, AL, AL, AL, AL, B, L, L, L, L, L, L, L, L, WS, AN,
	                    AN, CS, AN, AN, WS },
	            new short[] { NSM, WS, L, L, L, L, L, L, L, WS, L, L, L, L, WS, R,
	                    R, R, R, R, WS, L, L, L, L, L, L, L, WS, WS, AL, AL, AL,
	                    AL, WS, EN, EN, ES, EN, EN, CS, S, EN, EN, CS, WS, EN, EN,
	                    WS, AL, AL, AL, AL, AL, B, L, L, L, L, L, L, L, L, WS, AN,
	                    AN, CS, AN, AN, WS },
	            new short[] { NSM, WS, L, L, L, L, L, L, L, WS, L, L, L, L, WS, R,
	                    R, R, R, R, WS, L, L, L, L, L, L, L, WS, WS, AL, AL, AL,
	                    AL, WS, EN, EN, ES, EN, EN, CS, S, EN, EN, CS, WS, EN, EN,
	                    WS, AL, AL, AL, AL, AL, B, L, L, L, L, L, L, L, L, WS, AN,
	                    AN, CS, AN, AN, WS },
	            new short[] { NSM, WS, L, L, L, L, L, L, L, WS, L, L, L, L, WS, R,
	                    R, R, R, R, WS, L, L, L, L, L, L, L, WS, WS, AL, AL, AL,
	                    AL, WS, EN, EN, ES, EN, EN, CS, S, EN, EN, CS, WS, EN, EN,
	                    WS, AL, AL, AL, AL, AL, B, L, L, L, L, L, L, L, L, WS, AN,
	                    AN, CS, AN, AN, WS },
	            new short[] { ON, L, RLO, CS, R, WS, AN, AN, PDF, LRE, R, L, LRO,
	                    WS, BN, ON, S, LRE, LRO, B },
	            new short[] { ON, L, RLO, CS, R, WS, AN, AN, PDF, LRE, R, L, LRO,
	                    WS, BN, ON, S, LRE, LRO, B },
	            new short[] { RLO, RLO, AL, AL, WS, EN, ES, ON, WS, S, S, PDF, LRO,
	                    WS, AL, ET, RLE, ON, EN, B }, new short[] { R, L, CS, L },
	            new short[] { L, L, L, WS, L, L, L, WS, L, L, L },
	            new short[] { R, R, R, WS, R, R, R, WS, R, R, R },
	            new short[] { L }, null };
	
	    protected static internal readonly byte[][] testLevels = {
	            new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 },
	            new byte[] { 1, 1, 1, 1, 1, 1, 1 },
	            new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
	            new byte[] { 0, 1, 1, 1, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
	            new byte[] { 1, 1, 1, 1, 2, 1, 1, 2, 2, 2, 1, 1, 1, 1, 2, 2 },
	            new byte[] { 1, 2, 2, 2 },
	            new byte[] { 1, 1, 1, 1, 1, 1, 1, 1, 1 },
	            new byte[] { 62, 62, 62, 62, 62, 62, 62, 62, 62, 62, 62, 62, 62,
	                    62, 62, 62, 62, 62, 62, 62, 62, 62, 62, 62, 62, 62, 62, 62,
	                    62, 62, 62, 61, 61, 61, 61, 61, 61, 61, 61 },
	            new byte[] { 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60,
	                    60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60,
	                    60, 60, 60, 60, 60, 0, 0, 62, 62, 62, 62, 60 },
	            new byte[] { 0, 0, 0, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 3, 3, 3,
	                    3, 3, 3, 4, 4, 5, 5, 5, 4, 3, 3, 3, 3, 3, 3, 2, 2, 2, 2, 2,
	                    2, 2, 2, 2, 2, 2, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
	            new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1,
	                    1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 2, 2, 1,
	                    2, 2, 1, 0, 2, 2, 1, 1, 2, 2, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0,
	                    0, 0, 0, 0, 0, 0, 2, 2, 2, 2, 2, 0 },
	            new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1,
	                    1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 2, 2, 1,
	                    2, 2, 1, 0, 2, 2, 1, 1, 2, 2, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0,
	                    0, 0, 0, 0, 0, 0, 2, 2, 2, 2, 2, 0 },
	            new byte[] { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 3, 3, 3,
	                    3, 3, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 3, 3, 3, 3, 3, 4, 4, 3,
	                    4, 4, 3, 2, 4, 4, 3, 3, 4, 4, 3, 3, 3, 3, 3, 3, 2, 2, 2, 2,
	                    2, 2, 2, 2, 2, 2, 4, 4, 4, 4, 4, 2 },
	            new byte[] { 5, 5, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 5, 5, 5, 5,
	                    5, 5, 5, 6, 6, 6, 6, 6, 6, 6, 5, 5, 5, 5, 5, 5, 5, 6, 6, 5,
	                    6, 6, 5, 5, 6, 6, 5, 5, 6, 6, 5, 5, 5, 5, 5, 5, 5, 6, 6, 6,
	                    6, 6, 6, 6, 6, 5, 6, 6, 6, 6, 6, 5 },
	            new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1,
	                    1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 2, 2, 1,
	                    2, 2, 1, 0, 2, 2, 1, 1, 2, 2, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0,
	                    0, 0, 0, 0, 0, 0, 2, 2, 2, 2, 2, 0 },
	            new byte[] { 0, 0, 1, 1, 1, 1, 1, 1, 3, 3, 3, 2, 4, 4, 4, 4, 0, 0,
	                    0, 0 }, new byte[] { 0, 0, 1, 1, 1, 0 }, new byte[] { 1 },
	            new byte[] { 2 }, new byte[] { 2, 2, 2, 2, 2, 2, 2, 1 },
	            new byte[] { 1, 1, 1, 1, 1, 1, 1, 0 }, new byte[] { 2 }, null };
	
	    protected static internal readonly int[][] testVisualMaps = {
	            new int[] { 0, 1, 2, 3, 4, 5, 6, 7 },
	            new int[] { 6, 5, 4, 3, 2, 1, 0 },
	            new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 },
	            new int[] { 0, 3, 2, 1, 4, 6, 5, 7, 8, 9, 10, 11, 12, 13, 14, 15,
	                    16, 17 },
	            new int[] { 15, 14, 13, 12, 11, 10, 9, 6, 7, 8, 5, 4, 3, 2, 0, 1 },
	            new int[] { 3, 0, 1, 2 },
	            new int[] { 8, 7, 6, 5, 4, 3, 2, 1, 0 },
	            new int[] { 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21,
	                    22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36,
	                    37, 38, 7, 6, 5, 4, 3, 2, 1, 0 },
	            new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15,
	                    16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30,
	                    31, 32, 33, 34, 35, 36, 37, 38, 39 },
	            new int[] { 0, 1, 2, 44, 43, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 31,
	                    30, 29, 28, 27, 26, 20, 21, 24, 23, 22, 25, 19, 18, 17, 16,
	                    15, 14, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 3, 45,
	                    46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57 },
	            new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 19,
	                    18, 17, 16, 15, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 40,
	                    39, 38, 37, 36, 34, 35, 33, 31, 32, 30, 41, 52, 53, 51, 50,
	                    48, 49, 47, 46, 45, 44, 43, 42, 54, 55, 56, 57, 58, 59, 60,
	                    61, 62, 63, 64, 65, 66, 67, 68, 69 },
	            new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 19,
	                    18, 17, 16, 15, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 40,
	                    39, 38, 37, 36, 34, 35, 33, 31, 32, 30, 41, 52, 53, 51, 50,
	                    48, 49, 47, 46, 45, 44, 43, 42, 54, 55, 56, 57, 58, 59, 60,
	                    61, 62, 63, 64, 65, 66, 67, 68, 69 },
	            new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 19,
	                    18, 17, 16, 15, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 40,
	                    39, 38, 37, 36, 34, 35, 33, 31, 32, 30, 41, 52, 53, 51, 50,
	                    48, 49, 47, 46, 45, 44, 43, 42, 54, 55, 56, 57, 58, 59, 60,
	                    61, 62, 63, 64, 65, 66, 67, 68, 69 },
	            new int[] { 69, 68, 56, 57, 58, 59, 60, 61, 62, 63, 64, 65, 66, 67,
	                    55, 54, 53, 52, 51, 50, 49, 42, 43, 44, 45, 46, 47, 48, 41,
	                    40, 39, 38, 37, 36, 35, 33, 34, 32, 30, 31, 29, 28, 26, 27,
	                    25, 24, 22, 23, 21, 20, 19, 18, 17, 16, 15, 7, 8, 9, 10,
	                    11, 12, 13, 14, 6, 1, 2, 3, 4, 5, 0 },
	            new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 19,
	                    18, 17, 16, 15, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 40,
	                    39, 38, 37, 36, 34, 35, 33, 31, 32, 30, 41, 52, 53, 51, 50,
	                    48, 49, 47, 46, 45, 44, 43, 42, 54, 55, 56, 57, 58, 59, 60,
	                    61, 62, 63, 64, 65, 66, 67, 68, 69 },
	            new int[] { 0, 1, 15, 14, 13, 12, 11, 10, 4, 3, 2, 5, 6, 7, 8, 9,
	                    16, 17, 18, 19 }, new int[] { 0, 1, 4, 3, 2, 5 },
	            new int[] { 0 }, new int[] { 0 },
	            new int[] { 1, 2, 3, 4, 5, 6, 7, 0 },
	            new int[] { 6, 5, 4, 3, 2, 1, 0, 7 }, new int[] { 0 }, null };
	
	    protected static internal readonly sbyte[] testParaLevels = { IBM.ICU.Text.Bidi.LEVEL_DEFAULT_LTR,
	            IBM.ICU.Text.Bidi.LEVEL_DEFAULT_LTR, IBM.ICU.Text.Bidi.LEVEL_DEFAULT_LTR,
	            IBM.ICU.Text.Bidi.LEVEL_DEFAULT_LTR, IBM.ICU.Text.Bidi.LEVEL_DEFAULT_LTR,
	            IBM.ICU.Text.Bidi.LEVEL_DEFAULT_LTR, IBM.ICU.Text.Bidi.LEVEL_DEFAULT_LTR,
	            IBM.ICU.Text.Bidi.LEVEL_DEFAULT_LTR, IBM.ICU.Text.Bidi.LEVEL_DEFAULT_LTR,
	            IBM.ICU.Text.Bidi.LEVEL_DEFAULT_LTR, IBM.ICU.Text.Bidi.LEVEL_DEFAULT_LTR,
	            IBM.ICU.Text.Bidi.LEVEL_DEFAULT_RTL, 2, 5, IBM.ICU.Text.Bidi.LEVEL_DEFAULT_LTR,
	            IBM.ICU.Text.Bidi.LEVEL_DEFAULT_LTR, IBM.ICU.Text.Bidi.LEVEL_DEFAULT_LTR,
	            IBM.ICU.Text.Bidi.LEVEL_DEFAULT_LTR, IBM.ICU.Text.Bidi.LEVEL_DEFAULT_LTR, IBM.ICU.Text.Bidi.RTL, IBM.ICU.Text.Bidi.LTR,
	            IBM.ICU.Text.Bidi.RTL, IBM.ICU.Text.Bidi.LEVEL_DEFAULT_LTR };
	
	    protected static internal readonly sbyte[] testDirections = { IBM.ICU.Text.Bidi.LTR, IBM.ICU.Text.Bidi.RTL,
	            IBM.ICU.Text.Bidi.LTR, IBM.ICU.Text.Bidi.MIXED, IBM.ICU.Text.Bidi.MIXED, IBM.ICU.Text.Bidi.MIXED, IBM.ICU.Text.Bidi.RTL, IBM.ICU.Text.Bidi.MIXED,
	            IBM.ICU.Text.Bidi.MIXED, IBM.ICU.Text.Bidi.MIXED, IBM.ICU.Text.Bidi.MIXED, IBM.ICU.Text.Bidi.MIXED, IBM.ICU.Text.Bidi.MIXED,
	            IBM.ICU.Text.Bidi.MIXED, IBM.ICU.Text.Bidi.MIXED, IBM.ICU.Text.Bidi.MIXED, IBM.ICU.Text.Bidi.MIXED, IBM.ICU.Text.Bidi.RTL, IBM.ICU.Text.Bidi.LTR,
	            IBM.ICU.Text.Bidi.MIXED, IBM.ICU.Text.Bidi.MIXED, IBM.ICU.Text.Bidi.MIXED, IBM.ICU.Text.Bidi.LTR };
	
	    protected static internal readonly sbyte[] testResultLevels = new sbyte[] { IBM.ICU.Text.Bidi.LTR,
	            IBM.ICU.Text.Bidi.RTL, IBM.ICU.Text.Bidi.LTR, IBM.ICU.Text.Bidi.LTR, IBM.ICU.Text.Bidi.RTL, IBM.ICU.Text.Bidi.RTL, IBM.ICU.Text.Bidi.RTL,
	            IBM.ICU.Text.Bidi.LTR, IBM.ICU.Text.Bidi.LTR, IBM.ICU.Text.Bidi.LTR, IBM.ICU.Text.Bidi.LTR, IBM.ICU.Text.Bidi.LTR, 2, 5, IBM.ICU.Text.Bidi.LTR,
	            IBM.ICU.Text.Bidi.LTR, IBM.ICU.Text.Bidi.LTR, IBM.ICU.Text.Bidi.RTL, 2, IBM.ICU.Text.Bidi.RTL, IBM.ICU.Text.Bidi.LTR, IBM.ICU.Text.Bidi.RTL,
	            IBM.ICU.Text.Bidi.LTR };
	
	    protected static internal readonly sbyte[] testLineStarts = { -1, -1, -1, -1, -1, -1,
	            -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, 0, 13, 2, 0, 0, -1, -1 };
	
	    protected static internal readonly sbyte[] testLineLimits = { -1, -1, -1, -1, -1, -1,
	            -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, 6, 14, 3, 8, 8, -1, -1 };
	
	    protected internal short[] dirProps;
	
	    protected internal int lineStart;
	
	    protected internal int lineLimit;
	
	    protected internal sbyte direction;
	
	    protected internal sbyte paraLevel;
	
	    protected internal sbyte resultLevel;
	
	    protected internal byte[] levels;
	
	    protected internal int[] visualMap;
	
	    private TestData(short[] dirProps_0, int lineStart_1, int lineLimit_2,
	            sbyte direction_3, sbyte paraLevel_4, sbyte resultLevel_5, byte[] levels_6,
	            int[] visualMap_7) {
	        this.dirProps = dirProps_0;
	        this.lineStart = lineStart_1;
	        this.lineLimit = lineLimit_2;
	        this.direction = direction_3;
	        this.paraLevel = paraLevel_4;
	        this.resultLevel = resultLevel_5;
	        this.levels = levels_6;
	        this.visualMap = visualMap_7;
	    }
	
	    protected static internal TestData GetTestData(int testNumber) {
	        return new TestData(testDirProps[testNumber],
	                testLineStarts[testNumber], testLineLimits[testNumber],
	                testDirections[testNumber], testParaLevels[testNumber],
	                testResultLevels[testNumber], testLevels[testNumber],
	                testVisualMaps[testNumber]);
	    }
	
	    protected static internal int TestCount() {
	        return testDirProps.Length;
	    }
	}
}