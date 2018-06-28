/*
 *******************************************************************************
 * Copyright (C) 2002-2004, International Business Machines Corporation and    *
 * others. All Rights Reserved.                                                *
 *******************************************************************************
 */
// --------------------------------------------------------------------------------------------------
// This file was automatically generated by J2CS Translator (http://j2cstranslator.sourceforge.net/). 
// Version 1.3.6.20101125_01     
// 12/13/10 4:02 PM    
// ${CustomMessageForDisclaimer}                                                                             
// --------------------------------------------------------------------------------------------------
 namespace IBM.ICU.Charset {
	
	using IBM.ICU.Util;
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.IO;
	using System.Runtime.CompilerServices;
	
	public sealed class CompactArrayTest : TestFmwk {
	    public static void Main(String[] args) {
	        new CompactArrayTest().Run(args);
	    }
	
        /*
	    public void TestByteArrayCoverage() {
	        CompactByteArray cba = new CompactByteArray();
	        cba.SetElementAt((char) 0x5, 0xdf);
	        cba.SetElementAt((char) 0x105, 0xdf);
	        cba.SetElementAt((char) 0x205, 0xdf);
	        cba.SetElementAt((char) 0x305, 0xdf);
	        CompactByteArray cba2 = new CompactByteArray(0xdf);
	        if (cba.Equals(cba2)) {
	            Errln("unequal byte arrays compare equal");
	        }
	        CompactByteArray cba3 = (CompactByteArray) cba.Clone();
	
	        Logln("equals null: " + cba.Equals(null));
	        Logln("equals self: " + cba.Equals(cba));
	        Logln("equals clone: " + cba.Equals(cba3));
	        Logln("equals bogus: " + cba.Equals(new Object()));
	        Logln("hash: " + cba.GetHashCode());
	
	        cba.Compact(true);
	        cba.Compact(true);
	
	        char[] xa = cba.GetIndexArray();
	        sbyte[] va = cba.GetValueArray();
	        CompactByteArray cba4 = new CompactByteArray(xa, va);
	
	        String xs = IBM.ICU.Impl.Utility.ArrayToRLEString(xa);
	        String vs = IBM.ICU.Impl.Utility.ArrayToRLEString(va);
	        CompactByteArray cba5 = new CompactByteArray(xs, vs);
	
	        Logln("equals: " + cba4.Equals(cba5));
	        Logln("equals: " + cba.Equals(cba4));
	
	        cba4.Compact(false);
	        Logln("equals: " + cba4.Equals(cba5));
	
	        cba5.Compact(true);
	        Logln("equals: " + cba4.Equals(cba5));
	
	        cba.SetElementAt((char) 0x405, (byte) 0xdf); // force expand
	        Logln("modified equals clone: " + cba.Equals(cba3));
	
	        cba3.SetElementAt((char) 0x405, (byte) 0xdf); // equivalent modification
	        Logln("modified equals modified clone: " + cba.Equals(cba3));
	
	        cba3.SetElementAt((char) 0x405, (byte) 0xee); // different modification
	        Logln("different mod equals: " + cba.Equals(cba3));
	
	        cba.Compact();
	        CompactByteArray cba6 = (CompactByteArray) cba.Clone();
	        Logln("cloned compact equals: " + cba.Equals(cba6));
	
	        cba6.SetElementAt((char) 0x405, (byte) 0xee);
	        Logln("modified clone: " + cba3.Equals(cba6));
	
	        cba6.SetElementAt((char) 0x100, (char) 0x104, (byte) 0xfe);
	        for (int i = 0x100; i < 0x105; ++i) {
	            cba3.SetElementAt((char) i, (byte) 0xfe);
	        }
	        Logln("double modified: " + cba3.Equals(cba6));
	    }*/
	
	    public void TestCharArrayCoverage() {
	        // v1.8 fails with extensive compaction, so set to false
	        bool EXTENSIVE = false;
	
	        CompactCharArray cca = new CompactCharArray();
	        cca.SetElementAt((char) 0x5, (char) 0xdf);
	        cca.SetElementAt((char) 0x105, (char) 0xdf);
	        cca.SetElementAt((char) 0x205, (char) 0xdf);
	        cca.SetElementAt((char) 0x305, (char) 0xdf);
	        CompactCharArray cca2 = new CompactCharArray((char) 0xdf);
	        if (cca.Equals(cca2)) {
	            Errln("unequal char arrays compare equal");
	        }
	        CompactCharArray cca3 = (CompactCharArray) cca.Clone();
	
	        Logln("equals null: " + cca.Equals(null));
	        Logln("equals self: " + cca.Equals(cca));
	        Logln("equals clone: " + cca.Equals(cca3));
	        Logln("equals bogus: " + cca.Equals(new Object()));
	        Logln("hash: " + cca.GetHashCode());
	
	        cca.Compact(EXTENSIVE);
	        cca.Compact(EXTENSIVE);
	
	        char[] xa = cca.GetIndexArray();
	        char[] va = cca.GetValueArray();
	        CompactCharArray cca4 = new CompactCharArray(xa, va);
	
	        String xs = IBM.ICU.Impl.Utility.ArrayToRLEString(xa);
	        String vs = IBM.ICU.Impl.Utility.ArrayToRLEString(va);
	        CompactCharArray cca5 = new CompactCharArray(xs, vs);
	
	        Logln("equals: " + cca4.Equals(cca5));
	        Logln("equals: " + cca.Equals(cca4));
	
	        cca4.Compact(false);
	        Logln("equals: " + cca4.Equals(cca5));
	
	        cca5.Compact(EXTENSIVE);
	        Logln("equals: " + cca4.Equals(cca5));
	
	        cca.SetElementAt((char) 0x405, (char) 0xdf); // force expand
	        Logln("modified equals clone: " + cca.Equals(cca3));
	
	        cca3.SetElementAt((char) 0x405, (char) 0xdf); // equivalent modification
	        Logln("modified equals modified clone: " + cca.Equals(cca3));
	
	        cca3.SetElementAt((char) 0x405, (char) 0xee); // different modification
	        Logln("different mod equals: " + cca.Equals(cca3));
	
	        // after setElementAt isCompact is set to false
	        cca3.Compact(true);
	        Logln("different mod equals: " + cca.Equals(cca3));
	
	        cca3.SetElementAt((char) 0x405, (char) 0xee); // different modification
	        Logln("different mod equals: " + cca.Equals(cca3));
	        // after setElementAt isCompact is set to false
	        cca3.Compact();
	        Logln("different mod equals: " + cca.Equals(cca3));
	
	        // v1.8 fails with extensive compaction, and defaults extensive, so
	        // don't compact
	        // cca.compact();
	        CompactCharArray cca6 = (CompactCharArray) cca.Clone();
	        Logln("cloned compact equals: " + cca.Equals(cca6));
	
	        cca6.SetElementAt((char) 0x405, (char) 0xee);
	        Logln("modified clone: " + cca3.Equals(cca6));
	
	        cca6.SetElementAt((char) 0x100, (char) 0x104, (char) 0xfe);
	        for (int i = 0x100; i < 0x105; ++i) {
	            cca3.SetElementAt((char) i, (char) 0xfe);
	        }
	        Logln("double modified: " + cca3.Equals(cca6));
	    }
	}
}