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
namespace IBM.ICU.Charset
{

    using IBM.ICU.Text;
    using NUnit;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// Regression test for Bidi failure recovery
    /// </summary>
    ///

    [NUnit.Framework.TestFixture]
    public class TestFailureRecovery : BidiTest
    {

        [NUnit.Framework.Test]
        public void TestFailureRecovery2()
        {
            Logln("\nEntering TestFailureRecovery\n");
            Bidi bidi = new Bidi();
            try
            {
                bidi.SetPara("abc", (sbyte)(IBM.ICU.Text.Bidi.LEVEL_DEFAULT_LTR - 1), null);
                Errln("Bidi.setPara did not fail when passed too big para level");
            }
            catch (ArgumentException e)
            {
                Logln("OK: Got exception for bidi.setPara(..., Bidi.LEVEL_DEFAULT_LTR - 1, ...)"
                        + " as expected: " + e.Message);
            }
            try
            {
                bidi.SetPara("abc", (sbyte)(-1), null);
                Errln("Bidi.setPara did not fail when passed negative para level");
            }
            catch (ArgumentException e_0)
            {
                Logln("OK: Got exception for bidi.setPara(..., -1, ...)"
                        + " as expected: " + e_0.Message);
            }
            try
            {
                IBM.ICU.Text.Bidi.WriteReverse(null, 0);
                Errln("Bidi.writeReverse did not fail when passed a null string");
            }
            catch (ArgumentException e_1)
            {
                Logln("OK: Got exception for Bidi.writeReverse(null) as expected: "
                        + e_1.Message);
            }
            bidi = new Bidi();
            try
            {
                bidi.SetLine(0, 1);
                Errln("bidi.setLine did not fail when called before valid setPara()");
            }
            catch (InvalidOperationException e_2)
            {
                Logln("OK: Got exception for Bidi.setLine(0, 1) as expected: "
                        + e_2.Message);
            }
            try
            {
                bidi.GetDirection();
                Errln("bidi.getDirection did not fail when called before valid setPara()");
            }
            catch (InvalidOperationException e_3)
            {
                Logln("OK: Got exception for Bidi.getDirection() as expected: "
                        + e_3.Message);
            }
            bidi.SetPara("abc", IBM.ICU.Text.Bidi.LTR, null);
            try
            {
                bidi.GetLevelAt(3);
                Errln("bidi.getLevelAt did not fail when called with bad argument");
            }
            catch (ArgumentException e_4)
            {
                Logln("OK: Got exception for Bidi.getLevelAt(3) as expected: "
                        + e_4.Message);
            }
            try
            {
                bidi = new Bidi(-1, 0);
                Errln("Bidi constructor did not fail when called with bad argument");
            }
            catch (ArgumentException e_5)
            {
                Logln("OK: Got exception for Bidi(-1,0) as expected: "
                        + e_5.Message);
            }
            bidi = new Bidi(2, 1);
            try
            {
                bidi.SetPara("abc", IBM.ICU.Text.Bidi.LTR, null);
                Errln("setPara did not fail when called with text too long");
            }
            catch (Exception e_6)
            {
                Logln("OK: Got exception for setPara(\"abc\") as expected: "
                        + e_6.Message);
            }
            try
            {
                bidi.SetPara("=2", IBM.ICU.Text.Bidi.RTL, null);
                bidi.CountRuns();
                Errln("countRuns did not fail when called for too many runs");
            }
            catch (Exception e_7)
            {
                Logln("OK: Got exception for countRuns as expected: "
                        + e_7.Message);
            }
            int rm = bidi.GetReorderingMode();
            bidi.SetReorderingMode(IBM.ICU.Text.Bidi.REORDER_DEFAULT - 1);
            if (rm != bidi.GetReorderingMode())
            {
                Errln("setReorderingMode with bad argument #1 should have no effect");
            }
            bidi.SetReorderingMode(9999);
            if (rm != bidi.GetReorderingMode())
            {
                Errln("setReorderingMode with bad argument #2 should have no effect");
            }
            /* Try a surrogate char */
            bidi = new Bidi();
            bidi.SetPara("\uD800\uDC00", IBM.ICU.Text.Bidi.RTL, null);
            if (bidi.GetDirection() != IBM.ICU.Text.Bidi.MIXED)
            {
                Errln("getDirection for 1st surrogate char should be MIXED");
            }
            sbyte[] levels = new sbyte[] { 6, 5, 4 };
            try
            {
                bidi.SetPara("abc", (sbyte)5, levels);
                Errln("setPara did not fail when called with bad levels");
            }
            catch (ArgumentException e_8)
            {
                Logln("OK: Got exception for setPara(..., levels) as expected: "
                        + e_8.Message);
            }

            Logln("\nExiting TestFailureRecovery\n");
        }

        public static void Main(String[] args)
        {
            try
            {
                new TestFailureRecovery().Run(args);
            }
            catch (Exception e)
            {
                System.Console.Out.WriteLine(e);
            }
        }
    }
}