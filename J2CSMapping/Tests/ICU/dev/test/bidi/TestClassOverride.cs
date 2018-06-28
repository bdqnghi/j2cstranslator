/*
 *******************************************************************************
 *   Copyright (C) 2007, International Business Machines
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
	
	using IBM.ICU.Text;
	using NUnit;
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.IO;
	using System.Runtime.CompilerServices;
	
	/// <summary>
	/// Regression test for Bidi class override.
	/// </summary>
	///
	
	[NUnit.Framework.TestFixture]
	public class TestClassOverride : BidiTest {
	
	    public TestClassOverride() {
	        this.classifier = null;
	    }
	
	    private const int DEF = IBM.ICU.Charset.TestData.DEF;
	
	    private const int L = IBM.ICU.Charset.TestData.L;
	
	    private const int R = IBM.ICU.Charset.TestData.R;
	
	    private const int AL = IBM.ICU.Charset.TestData.AL;
	
	    private const int AN = IBM.ICU.Charset.TestData.AN;
	
	    private const int EN = IBM.ICU.Charset.TestData.EN;
	
	    private const int LRE = IBM.ICU.Charset.TestData.LRE;
	
	    private const int RLE = IBM.ICU.Charset.TestData.RLE;
	
	    private const int LRO = IBM.ICU.Charset.TestData.LRO;
	
	    private const int RLO = IBM.ICU.Charset.TestData.RLO;
	
	    private const int PDF = IBM.ICU.Charset.TestData.PDF;
	
	    private const int NSM = IBM.ICU.Charset.TestData.NSM;
	
	    private const int B = IBM.ICU.Charset.TestData.B;
	
	    private const int S = IBM.ICU.Charset.TestData.S;
	
	    private const int BN = IBM.ICU.Charset.TestData.BN;
	
	    private static readonly int[] customClasses = {
	    /* 0/8 1/9 2/A 3/B 4/C 5/D 6/E 7/F */
	    DEF, DEF, DEF, DEF, DEF, DEF, DEF, DEF, // 00-07
	            DEF, DEF, DEF, DEF, DEF, DEF, DEF, DEF, // 08-0F
	            DEF, DEF, DEF, DEF, DEF, DEF, DEF, DEF, // 10-17
	            DEF, DEF, DEF, DEF, DEF, DEF, DEF, DEF, // 18-1F
	            DEF, DEF, DEF, DEF, DEF, DEF, R, DEF, // 20-27
	            DEF, DEF, DEF, DEF, DEF, DEF, DEF, DEF, // 28-2F
	            EN, EN, EN, EN, EN, EN, AN, AN, // 30-37
	            AN, AN, DEF, DEF, DEF, DEF, DEF, DEF, // 38-3F
	            L, AL, AL, AL, AL, AL, AL, R, // 40-47
	            R, R, R, R, R, R, R, R, // 48-4F
	            R, R, R, R, R, R, R, R, // 50-57
	            R, R, R, LRE, DEF, RLE, PDF, S, // 58-5F
	            NSM, DEF, DEF, DEF, DEF, DEF, DEF, DEF, // 60-67
	            DEF, DEF, DEF, DEF, DEF, DEF, DEF, DEF, // 68-6F
	            DEF, DEF, DEF, DEF, DEF, DEF, DEF, DEF, // 70-77
	            DEF, DEF, DEF, LRO, B, RLO, BN, DEF // 78-7F
	    };
	
	    static internal readonly int nEntries = customClasses.Length;
	
	    internal const String textIn = "JIH.>12->a \u05d0\u05d1 6 ABC78";
	
	    internal const String textOut = "12<.HIJ->a 78CBA 6 \u05d1\u05d0";
	
	    protected internal class CustomClassifier : BidiClassifier {
	
	        public CustomClassifier(Object context) : base(context) {
	        }
	
	        public override int Classify(int c) {
	            // some (meaningless) action - just for testing purposes
	            return ((this.context != null) ? ((Int32) context)
	                    : (c >= IBM.ICU.Charset.TestClassOverride.nEntries) ? base.Classify(c) : IBM.ICU.Charset.TestClassOverride.customClasses[c]);
	        }
	    }
	
	    private void VerifyClassifier(Bidi bidi) {
	        BidiClassifier actualClassifier = bidi.GetCustomClassifier();
	
	        if (this.classifier == null) {
	            if (actualClassifier != null) {
	                Errln("Bidi classifier is not yet set, but reported as not null");
	            }
	        } else {
	            Type expectedClass = this.classifier.GetType();
	            AssertTrue("null Bidi classifier", actualClassifier != null);
	            if (actualClassifier == null) {
	                return;
	            }
	            if (expectedClass.IsInstanceOfType(actualClassifier)) {
	                Object context = classifier.GetContext();
	                if (context == null) {
	                    if (actualClassifier.GetContext() != null) {
	                        Errln("Unexpected context, should be null");
	                    }
	                } else {
	                    AssertEquals("Unexpected classifier context", context,
	                            actualClassifier.GetContext());
	                    AssertEquals("Unexpected context's content",
	                            ((Int32) context),
	                            bidi.GetCustomizedClass('a'));
	                }
	            } else {
	                Errln("Bidi object reports classifier is an instance of "
	                        + actualClassifier.GetType().FullName
	                        + ",\nwhile the expected classifier should be an "
	                        + "instance of " + expectedClass);
	            }
	        }
	    }
	
	    internal TestClassOverride.CustomClassifier  classifier;
	
	    [NUnit.Framework.Test]
	    public void TestClassOverride2() {
	        Bidi bidi;
	
	        Logln("\nEntering TestClassOverride\n");
	
	        bidi = new Bidi();
	        VerifyClassifier(bidi);
	
	        classifier = new TestClassOverride.CustomClassifier (((int)(IBM.ICU.Charset.TestData.R)));
	        bidi.SetCustomClassifier(classifier);
	        VerifyClassifier(bidi);
	
	        classifier.SetContext(null);
	        VerifyClassifier(bidi);
	
	        bidi.SetPara(textIn, IBM.ICU.Text.Bidi.LTR, null);
	
	        String xout = bidi.WriteReordered(IBM.ICU.Text.Bidi.DO_MIRRORING);
	        AssertEquals("Actual and expected output mismatch", textOut, xout);
	
	        Logln("\nExiting TestClassOverride\n");
	    }
	
	    public static void Main(String[] args) {
	        try {
	            new TestClassOverride().Run(args);
	        } catch (Exception e) {
	            System.Console.Out.WriteLine(e);
	        }
	    }
	}
}
