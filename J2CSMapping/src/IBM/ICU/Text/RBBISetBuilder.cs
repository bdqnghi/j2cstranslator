/*
 *******************************************************************************
 * Copyright (C) 2003-2006,
 * International Business Machines Corporation and others. All Rights Reserved.
 *******************************************************************************
 */

// --------------------------------------------------------------------------------------------------
// This file was automatically generated by J2CS Translator (http://j2cstranslator.sourceforge.net/). 
// Version 1.3.6.20101125_01     
// 12/2/10 11:30 AM    
// ${CustomMessageForDisclaimer}                                                                             
// --------------------------------------------------------------------------------------------------
 namespace IBM.ICU.Text {
	
	using IBM.ICU.Impl;
	using ILOG.J2CsMapping.Collections;
	using ILOG.J2CsMapping.Collections.Generics;
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.IO;
	using System.Runtime.CompilerServices;
	
	//
	//  RBBISetBuilder   Handles processing of Unicode Sets from RBBI rules
	//                   (part of the rule building process.)
	//
	//      Starting with the rules parse tree from the scanner,
	//
	//                   -  Enumerate the set of UnicodeSets that are referenced
	//                      by the RBBI rules.
	//                   -  compute a set of non-overlapping character ranges
	//                      with all characters within a range belonging to the same
	//                      set of input uniocde sets.
	//                   -  Derive a set of non-overlapping UnicodeSet (like things)
	//                      that will correspond to columns in the state table for
	//                      the RBBI execution engine.  All characters within one
	//                      of these sets belong to the same set of the original
	//                      UnicodeSets from the user's rules.
	//                   -  construct the trie table that maps input characters
	//                      to the index of the matching non-overlapping set of set from
	//                      the previous step.
	//
	internal class RBBISetBuilder {
	    internal class RangeDescriptor {
	        internal int fStartChar; // Start of range, unicode 32 bit value.
	
	        internal int fEndChar; // End of range, unicode 32 bit value.
	
	        internal int fNum; // runtime-mapped input value for this range.
	
	        internal IList fIncludesSets; // vector of the the original
	                            // Unicode sets that include this range.
	                            // (Contains ptrs to uset nodes)
	
	        internal RBBISetBuilder.RangeDescriptor  fNext; // Next RangeDescriptor in the linked list.
	
	        internal RangeDescriptor() {
	            fIncludesSets = new ArrayList();
	        }
	
	        internal RangeDescriptor(RBBISetBuilder.RangeDescriptor  other) {
	            fStartChar = other.fStartChar;
	            fEndChar = other.fEndChar;
	            fNum = other.fNum;
	            fIncludesSets = new ArrayList(other.fIncludesSets);
	        }
	
	        // -------------------------------------------------------------------------------------
	        //
	        // RangeDesriptor::split()
	        //
	        // -------------------------------------------------------------------------------------
	        internal void Split(int where) {
	            IBM.ICU.Impl.Assert.Assrt(where > fStartChar && where <= fEndChar);
	            RBBISetBuilder.RangeDescriptor  nr = new RBBISetBuilder.RangeDescriptor (this);
	
	            // RangeDescriptor copy constructor copies all fields.
	            // Only need to update those that are different after the split.
	            nr.fStartChar = where;
	            this.fEndChar = where - 1;
	            nr.fNext = this.fNext;
	            this.fNext = nr;
	
	            // TODO: fIncludesSets is not updated. Check it out.
	            // Probably because they haven't been populated yet,
	            // but still sloppy.
	        }
	
	        // -------------------------------------------------------------------------------------
	        //
	        // RangeDescriptor::setDictionaryFlag
	        //
	        // Character Category Numbers that include characters from
	        // the original Unicode Set named "dictionary" have bit 14
	        // set to 1. The RBBI runtime engine uses this to trigger
	        // use of the word dictionary.
	        //
	        // This function looks through the Unicode Sets that it
	        // (the range) includes, and sets the bit in fNum when
	        // "dictionary" is among them.
	        //
	        // TODO: a faster way would be to find the set node for
	        // "dictionary" just once, rather than looking it
	        // up by name every time.
	        //
	        // -------------------------------------------------------------------------------------
	        internal void SetDictionaryFlag() {
	            int i;
	
	            for (i = 0; i < this.fIncludesSets.Count; i++) {
	                RBBINode usetNode = (RBBINode) fIncludesSets[i];
	                String setName = "";
	                RBBINode setRef = usetNode.fParent;
	                if (setRef != null) {
	                    RBBINode varRef = setRef.fParent;
	                    if (varRef != null && varRef.fType == IBM.ICU.Text.RBBINode.varRef) {
	                        setName = varRef.fText;
	                    }
	                }
	                if (setName.Equals("dictionary")) {
	                    this.fNum |= 0x4000;
	                    break;
	                }
	            }
	
	        }
	    }
	
	    internal RBBIRuleBuilder fRB; // The RBBI Rule Compiler that owns us.
	
	    internal RBBISetBuilder.RangeDescriptor  fRangeList; // Head of the linked list of RangeDescriptors
	
	    internal IntTrieBuilder fTrie; // The mapping TRIE that is the end result of
	                          // processing
	
	    internal int fTrieSize; // the Unicode Sets.
	
	    // Groups correspond to character categories -
	    // groups of ranges that are in the same original UnicodeSets.
	    // fGroupCount is the index of the last used group.
	    // fGroupCount+1 is also the number of columns in the RBBI state table being
	    // compiled.
	    // State table column 0 is not used. Column 1 is for end-of-input.
	    // column 2 is for group 0. Funny counting.
	    internal int fGroupCount;
	
	    internal bool fSawBOF;
	
	    // ------------------------------------------------------------------------
	    //
	    // RBBISetBuilder Constructor
	    //
	    // ------------------------------------------------------------------------
	    internal RBBISetBuilder(RBBIRuleBuilder rb) {
	        this.dm = new RBBISetBuilder.RBBIDataManipulate (this);
	        fRB = rb;
	    }
	
	    // ------------------------------------------------------------------------
	    //
	    // build Build the list of non-overlapping character ranges
	    // from the Unicode Sets.
	    //
	    // ------------------------------------------------------------------------
	    internal void Build() {
	        RBBINode usetNode;
	        RBBISetBuilder.RangeDescriptor  rlRange;
	
	        if (fRB.fDebugEnv != null && fRB.fDebugEnv.IndexOf("usets") >= 0) {
	            PrintSets();
	        }
	
	        // Initialize the process by creating a single range encompassing all
	        // characters
	        // that is in no sets.
	        //
	        fRangeList = new RBBISetBuilder.RangeDescriptor ();
	        fRangeList.fStartChar = 0;
	        fRangeList.fEndChar = 0x10ffff;
	
	        //
	        // Find the set of non-overlapping ranges of characters
	        //
	        IIterator ni = new ILOG.J2CsMapping.Collections.IteratorAdapter(fRB.fUSetNodes.GetEnumerator());
	        while (ni.HasNext()) {
	            usetNode = (RBBINode) ni.Next();
	
	            UnicodeSet inputSet = usetNode.fInputSet;
	            int inputSetRangeCount = inputSet.GetRangeCount();
	            int inputSetRangeIndex = 0;
	            rlRange = fRangeList;
	
	            for (;;) {
	                if (inputSetRangeIndex >= inputSetRangeCount) {
	                    break;
	                }
	                int inputSetRangeBegin = inputSet
	                        .GetRangeStart(inputSetRangeIndex);
	                int inputSetRangeEnd = inputSet.GetRangeEnd(inputSetRangeIndex);
	
	                // skip over ranges from the range list that are completely
	                // below the current range from the input unicode set.
	                while (rlRange.fEndChar < inputSetRangeBegin) {
	                    rlRange = rlRange.fNext;
	                }
	
	                // If the start of the range from the range list is before with
	                // the start of the range from the unicode set, split the range
	                // list range
	                // in two, with one part being before (wholly outside of) the
	                // unicode set
	                // and the other containing the rest.
	                // Then continue the loop; the post-split current range will
	                // then be skipped
	                // over
	                if (rlRange.fStartChar < inputSetRangeBegin) {
	                    rlRange.Split(inputSetRangeBegin);
	                    continue;
	                }
	
	                // Same thing at the end of the ranges...
	                // If the end of the range from the range list doesn't coincide
	                // with
	                // the end of the range from the unicode set, split the range
	                // list
	                // range in two. The first part of the split range will be
	                // wholly inside the Unicode set.
	                if (rlRange.fEndChar > inputSetRangeEnd) {
	                    rlRange.Split(inputSetRangeEnd + 1);
	                }
	
	                // The current rlRange is now entirely within the UnicodeSet
	                // range.
	                // Add this unicode set to the list of sets for this rlRange
	                if (rlRange.fIncludesSets.IndexOf(usetNode) == -1) {
	                    ILOG.J2CsMapping.Collections.Generics.Collections.Add(rlRange.fIncludesSets,usetNode);
	                }
	
	                // Advance over ranges that we are finished with.
	                if (inputSetRangeEnd == rlRange.fEndChar) {
	                    inputSetRangeIndex++;
	                }
	                rlRange = rlRange.fNext;
	            }
	        }
	
	        if (fRB.fDebugEnv != null && fRB.fDebugEnv.IndexOf("range") >= 0) {
	            PrintRanges();
	        }
	
	        //
	        // Group the above ranges, with each group consisting of one or more
	        // ranges that are in exactly the same set of original UnicodeSets.
	        // The groups are numbered, and these group numbers are the set of
	        // input symbols recognized by the run-time state machine.
	        //
	        // Numbering: # 0 (state table column 0) is unused.
	        // # 1 is reserved - table column 1 is for end-of-input
	        // # 2 is reserved - table column 2 is for beginning-in-input
	        // # 3 is the first range list.
	        //
	        RBBISetBuilder.RangeDescriptor  rlSearchRange;
	        for (rlRange = fRangeList; rlRange != null; rlRange = rlRange.fNext) {
	            for (rlSearchRange = fRangeList; rlSearchRange != rlRange; rlSearchRange = rlSearchRange.fNext) {
	                if (rlRange.fIncludesSets.Equals(rlSearchRange.fIncludesSets)) {
	                    rlRange.fNum = rlSearchRange.fNum;
	                    break;
	                }
	            }
	            if (rlRange.fNum == 0) {
	                fGroupCount++;
	                rlRange.fNum = fGroupCount + 2;
	                rlRange.SetDictionaryFlag();
	                AddValToSets(rlRange.fIncludesSets, fGroupCount + 2);
	            }
	        }
	
	        // Handle input sets that contain the special string {eof}.
	        // Column 1 of the state table is reserved for EOF on input.
	        // Column 2 is reserved for before-the-start-input.
	        // (This column can be optimized away later if there are no rule
	        // references to {bof}.)
	        // Add this column value (1 or 2) to the equivalent expression
	        // subtree for each UnicodeSet that contains the string {eof}
	        // Because {bof} and {eof} are not a characters in the normal sense,
	        // they doesn't affect the computation of ranges or TRIE.
	
	        String eofString = "eof";
	        String bofString = "bof";
	
	        ni = new ILOG.J2CsMapping.Collections.IteratorAdapter(fRB.fUSetNodes.GetEnumerator());
	        while (ni.HasNext()) {
	            usetNode = (RBBINode) ni.Next();
	            UnicodeSet inputSet_0 = usetNode.fInputSet;
	            if (inputSet_0.Contains(eofString)) {
	                AddValToSet(usetNode, 1);
	            }
	            if (inputSet_0.Contains(bofString)) {
	                AddValToSet(usetNode, 2);
	                fSawBOF = true;
	            }
	        }
	
	        if (fRB.fDebugEnv != null && fRB.fDebugEnv.IndexOf("rgroup") >= 0) {
	            PrintRangeGroups();
	        }
	        if (fRB.fDebugEnv != null && fRB.fDebugEnv.IndexOf("esets") >= 0) {
	            PrintSets();
	        }
	
	        // IntTrieBuilder(int aliasdata[], int maxdatalength,
	        // int initialvalue, int leadunitvalue,
	        // boolean latin1linear)
	
	        fTrie = new IntTrieBuilder(null, // Data array (utrie will allocate one)
	                100000, // Max Data Length
	                0, // Initial value for all code points
	                0, // Lead Surrogate unit value,
	                true); // Keep Latin 1 in separately.
	
	        for (rlRange = fRangeList; rlRange != null; rlRange = rlRange.fNext) {
	            fTrie.SetRange(rlRange.fStartChar, rlRange.fEndChar + 1,
	                    rlRange.fNum, true);
	        }
	    }
	
	    public class RBBIDataManipulate : IntTrieBuilder.DataManipulate {
	            private RBBISetBuilder outer_RBBISetBuilder;
	    
	            
	            /// <param name="builder"></param>
	            public RBBIDataManipulate(RBBISetBuilder builder) {
	                outer_RBBISetBuilder = builder;
	            }
	    
	            public virtual int GetFoldedValue(int start, int offset) {
	                int value_ren;
	                int limit;
	                bool[] inBlockZero = new bool[1];
	    
	                limit = start + 0x400;
	                while (start < limit) {
	                    value_ren = outer_RBBISetBuilder.fTrie.GetValue(start, inBlockZero);
	                    if (inBlockZero[0]) {
	                        start += IBM.ICU.Impl.TrieBuilder.DATA_BLOCK_LENGTH;
	                    } else if (value_ren != 0) {
	                        return offset | 0x08000;
	                    } else {
	                        ++start;
	                    }
	                }
	                return 0;
	            }
	        }
	
	    internal RBBISetBuilder.RBBIDataManipulate  dm;
	
	    // -----------------------------------------------------------------------------------
	    //
	    // getTrieSize() Return the size that will be required to serialize the
	    // Trie.
	    //
	    // -----------------------------------------------------------------------------------
	    internal int GetTrieSize() {
	        int size = 0;
	        try {
	            // The trie serialize function returns the size of the data written.
	            // null output stream says give size only, don't actually write
	            // anything.
	            size = fTrie.Serialize(null, true, dm);
	        } catch (IOException e) {
	            IBM.ICU.Impl.Assert.Assrt(false);
	        }
	        return size;
	    }
	
	    // -----------------------------------------------------------------------------------
	    //
	    // serializeTrie() Write the serialized trie to an output stream
	    //
	    // -----------------------------------------------------------------------------------
	    internal void SerializeTrie(Stream os) {
	        fTrie.Serialize(os, true, dm);
	    }
	
	    // ------------------------------------------------------------------------
	    //
	    // addValToSets Add a runtime-mapped input value to each uset from a
	    // list of uset nodes. (val corresponds to a state table column.)
	    // For each of the original Unicode sets - which correspond
	    // directly to uset nodes - a logically equivalent expression
	    // is constructed in terms of the remapped runtime input
	    // symbol set. This function adds one runtime input symbol to
	    // a list of sets.
	    //
	    // The "logically equivalent expression" is the tree for an
	    // or-ing together of all of the symbols that go into the set.
	    //
	    // ------------------------------------------------------------------------
	    internal void AddValToSets(IList sets, int val) {
	        int ix;
	
	        for (ix = 0; ix < sets.Count; ix++) {
	            RBBINode usetNode = (RBBINode) sets[ix];
	            AddValToSet(usetNode, val);
	        }
	    }
	
	    internal void AddValToSet(RBBINode usetNode, int val) {
	        RBBINode leafNode = new RBBINode(IBM.ICU.Text.RBBINode.leafChar);
	        leafNode.fVal = val;
	        if (usetNode.fLeftChild == null) {
	            usetNode.fLeftChild = leafNode;
	            leafNode.fParent = usetNode;
	        } else {
	            // There are already input symbols present for this set.
	            // Set up an OR node, with the previous stuff as the left child
	            // and the new value as the right child.
	            RBBINode orNode = new RBBINode(IBM.ICU.Text.RBBINode.opOr);
	            orNode.fLeftChild = usetNode.fLeftChild;
	            orNode.fRightChild = leafNode;
	            orNode.fLeftChild.fParent = orNode;
	            orNode.fRightChild.fParent = orNode;
	            usetNode.fLeftChild = orNode;
	            orNode.fParent = usetNode;
	        }
	    }
	
	    // ------------------------------------------------------------------------
	    //
	    // getNumCharCategories
	    //
	    // ------------------------------------------------------------------------
	    internal int GetNumCharCategories() {
	        return fGroupCount + 3;
	    }
	
	    // ------------------------------------------------------------------------
	    //
	    // sawBOF
	    //
	    // ------------------------------------------------------------------------
	    internal bool SawBOF() {
	        return fSawBOF;
	    }
	
	    // ------------------------------------------------------------------------
	    //
	    // getFirstChar Given a runtime RBBI character category, find
	    // the first UChar32 that is in the set of chars
	    // in the category.
	    // ------------------------------------------------------------------------
	    internal int GetFirstChar(int category) {
	        RBBISetBuilder.RangeDescriptor  rlRange;
	        int retVal = -1;
	        for (rlRange = fRangeList; rlRange != null; rlRange = rlRange.fNext) {
	            if (rlRange.fNum == category) {
	                retVal = rlRange.fStartChar;
	                break;
	            }
	        }
	        return retVal;
	    }
	
	    // ------------------------------------------------------------------------
	    //
	    // printRanges A debugging function.
	    // dump out all of the range definitions.
	    //
	    // ------------------------------------------------------------------------
	    // /CLOVER:OFF
	    internal void PrintRanges() {
	        RBBISetBuilder.RangeDescriptor  rlRange;
	        int i;
	
	        System.Console.Out.Write("\n\n Nonoverlapping Ranges ...\n");
	        for (rlRange = fRangeList; rlRange != null; rlRange = rlRange.fNext) {
	            System.Console.Out.Write(" " + rlRange.fNum + "   "
	                    + (int) rlRange.fStartChar + "-" + (int) rlRange.fEndChar);
	
	            for (i = 0; i < rlRange.fIncludesSets.Count; i++) {
	                RBBINode usetNode = (RBBINode) rlRange.fIncludesSets[i];
	                String setName = "anon";
	                RBBINode setRef = usetNode.fParent;
	                if (setRef != null) {
	                    RBBINode varRef = setRef.fParent;
	                    if (varRef != null && varRef.fType == IBM.ICU.Text.RBBINode.varRef) {
	                        setName = varRef.fText;
	                    }
	                }
	                System.Console.Out.Write(setName);
	                System.Console.Out.Write("  ");
	            }
	            System.Console.Out.WriteLine("");
	        }
	    }
	
	    // /CLOVER:ON
	
	    // ------------------------------------------------------------------------
	    //
	    // printRangeGroups A debugging function.
	    // dump out all of the range groups.
	    //
	    // ------------------------------------------------------------------------
	    // /CLOVER:OFF
	    internal void PrintRangeGroups() {
	        RBBISetBuilder.RangeDescriptor  rlRange;
	        RBBISetBuilder.RangeDescriptor  tRange;
	        int i;
	        int lastPrintedGroupNum = 0;
	
	        System.Console.Out.Write("\nRanges grouped by Unicode Set Membership...\n");
	        for (rlRange = fRangeList; rlRange != null; rlRange = rlRange.fNext) {
	            int groupNum = rlRange.fNum & 0xbfff;
	            if (groupNum > lastPrintedGroupNum) {
	                lastPrintedGroupNum = groupNum;
	                if (groupNum < 10) {
	                    System.Console.Out.Write(" ");
	                }
	                System.Console.Out.Write(groupNum + " ");
	
	                if ((rlRange.fNum & 0x4000) != 0) {
	                    System.Console.Out.Write(" <DICT> ");
	                }
	
	                for (i = 0; i < rlRange.fIncludesSets.Count; i++) {
	                    RBBINode usetNode = (RBBINode) rlRange.fIncludesSets[i];
	                    String setName = "anon";
	                    RBBINode setRef = usetNode.fParent;
	                    if (setRef != null) {
	                        RBBINode varRef = setRef.fParent;
	                        if (varRef != null && varRef.fType == IBM.ICU.Text.RBBINode.varRef) {
	                            setName = varRef.fText;
	                        }
	                    }
	                    System.Console.Out.Write(setName);
	                    System.Console.Out.Write(" ");
	                }
	
	                i = 0;
	                for (tRange = rlRange; tRange != null; tRange = tRange.fNext) {
	                    if (tRange.fNum == rlRange.fNum) {
	                        if (i++ % 5 == 0) {
	                            System.Console.Out.Write("\n    ");
	                        }
	                        IBM.ICU.Text.RBBINode.PrintHex((int) tRange.fStartChar, -1);
	                        System.Console.Out.Write("-");
	                        IBM.ICU.Text.RBBINode.PrintHex((int) tRange.fEndChar, 0);
	                    }
	                }
	                System.Console.Out.Write("\n");
	            }
	        }
	        System.Console.Out.Write("\n");
	    }
	
	    // /CLOVER:ON
	
	    // ------------------------------------------------------------------------
	    //
	    // printSets A debugging function.
	    // dump out all of the set definitions.
	    //
	    // ------------------------------------------------------------------------
	    // /CLOVER:OFF
	    internal void PrintSets() {
	        int i;
	        System.Console.Out.Write("\n\nUnicode Sets List\n------------------\n");
	        for (i = 0; i < fRB.fUSetNodes.Count; i++) {
	            RBBINode usetNode;
	            RBBINode setRef;
	            RBBINode varRef;
	            String setName;
	
	            usetNode = (RBBINode) fRB.fUSetNodes[i];
	
	            // System.out.print(" " + i + "   ");
	            IBM.ICU.Text.RBBINode.PrintInt(2, i);
	            setName = "anonymous";
	            setRef = usetNode.fParent;
	            if (setRef != null) {
	                varRef = setRef.fParent;
	                if (varRef != null && varRef.fType == IBM.ICU.Text.RBBINode.varRef) {
	                    setName = varRef.fText;
	                }
	            }
	            System.Console.Out.Write("  " + setName);
	            System.Console.Out.Write("   ");
	            System.Console.Out.Write(usetNode.fText);
	            System.Console.Out.Write("\n");
	            if (usetNode.fLeftChild != null) {
	                usetNode.fLeftChild.PrintTree(true);
	            }
	        }
	        System.Console.Out.Write("\n");
	    }
	    // /CLOVER:ON
	}
}