/*
 *******************************************************************************
 * Copyright (C) 1996-2007, International Business Machines Corporation and    *
 * others. All Rights Reserved.                                                *
 *******************************************************************************
 */
// --------------------------------------------------------------------------------------------------
// This file was automatically generated by J2CS Translator (http://j2cstranslator.sourceforge.net/). 
// Version 1.3.6.20101125_01     
// 12/2/10 11:30 AM    
// ${CustomMessageForDisclaimer}                                                                             
// --------------------------------------------------------------------------------------------------
 namespace IBM.ICU.Text {
	
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.IO;
	using System.Runtime.CompilerServices;
	using System.Text;
	
	/// <summary>
	/// A set of rules for a <c>RuleBasedTransliterator</c>. This set encodes
	/// the transliteration in one direction from one set of characters or short
	/// strings to another. A <c>RuleBasedTransliterator</c> consists of up to
	/// two such sets, one for the forward direction, and one for the reverse.
	/// <p>
	/// A <c>TransliterationRuleSet</c> has one important operation, that of
	/// finding a matching rule at a given point in the text. This is accomplished by
	/// the <c>findMatch()</c> method.
	/// <p>
	/// Copyright &copy; IBM Corporation 1999. All rights reserved.
	/// </summary>
	///
	internal class TransliterationRuleSet {
	    /// <summary>
	    /// Vector of rules, in the order added.
	    /// </summary>
	    ///
	    private ArrayList ruleVector;
	
	    /// <summary>
	    /// Length of the longest preceding context
	    /// </summary>
	    ///
	    private int maxContextLength;
	
	    /// <summary>
	    /// Sorted and indexed table of rules. This is created by freeze() from the
	    /// rules in ruleVector. rules.length >= ruleVector.size(), and the
	    /// references in rules[] are aliases of the references in ruleVector. A
	    /// single rule in ruleVector is listed one or more times in rules[].
	    /// </summary>
	    ///
	    private TransliterationRule[] rules;
	
	    /// <summary>
	    /// Index table. For text having a first character c, compute x = c&0xFF. Now
	    /// use rules[index[x]..index[x+1]-1]. This index table is created by
	    /// freeze().
	    /// </summary>
	    ///
	    private int[] index;
	
	    /// <summary>
	    /// Construct a new empty rule set.
	    /// </summary>
	    ///
	    public TransliterationRuleSet() {
	        ruleVector = new ArrayList();
	        maxContextLength = 0;
	    }
	
	    /// <summary>
	    /// Return the maximum context length.
	    /// </summary>
	    ///
	    /// <returns>the length of the longest preceding context.</returns>
	    public int GetMaximumContextLength() {
	        return maxContextLength;
	    }
	
	    /// <summary>
	    /// Add a rule to this set. Rules are added in order, and order is
	    /// significant.
	    /// </summary>
	    ///
	    /// <param name="rule">the rule to add</param>
	    public void AddRule(TransliterationRule rule) {
	        ruleVector.Add(rule);
	        int len;
	        if ((len = rule.GetAnteContextLength()) > maxContextLength) {
	            maxContextLength = len;
	        }
	
	        rules = null;
	    }
	
	    /// <summary>
	    /// Close this rule set to further additions, check it for masked rules, and
	    /// index it to optimize performance.
	    /// </summary>
	    ///
	    /// <exception cref="IllegalArgumentException">if some rules are masked</exception>
	    public void Freeze() {
	        /*
	         * Construct the rule array and index table. We reorder the rules by
	         * sorting them into 256 bins. Each bin contains all rules matching the
	         * index value for that bin. A rule matches an index value if string
	         * whose first key character has a low byte equal to the index value can
	         * match the rule.
	         * 
	         * Each bin contains zero or more rules, in the same order they were
	         * found originally. However, the total rules in the bins may exceed the
	         * number in the original vector, since rules that have a variable as
	         * their first key character will generally fall into more than one bin.
	         * 
	         * That is, each bin contains all rules that either have that first
	         * index value as their first key character, or have a set containing
	         * the index value as their first character.
	         */
	        int n = ruleVector.Count;
	        index = new int[257]; // [sic]
	        ArrayList v = new ArrayList(2 * n); // heuristic; adjust as needed
	
	        /*
	         * Precompute the index values. This saves a LOT of time.
	         */
	        int[] indexValue = new int[n];
	        for (int j = 0; j < n; ++j) {
	            TransliterationRule r = (TransliterationRule) ruleVector[j];
	            indexValue[j] = r.GetIndexValue();
	        }
	        for (int x = 0; x < 256; ++x) {
	            index[x] = v.Count;
	            for (int j_0 = 0; j_0 < n; ++j_0) {
	                if (indexValue[j_0] >= 0) {
	                    if (indexValue[j_0] == x) {
	                        v.Add(ruleVector[j_0]);
	                    }
	                } else {
	                    // If the indexValue is < 0, then the first key character is
	                    // a set, and we must use the more time-consuming
	                    // matchesIndexValue check. In practice this happens
	                    // rarely, so we seldom tread this code path.
	                    TransliterationRule r_1 = (TransliterationRule) ruleVector[j_0];
	                    if (r_1.MatchesIndexValue(x)) {
	                        v.Add(r_1);
	                    }
	                }
	            }
	        }
	        index[256] = v.Count;
	
	        /*
	         * Freeze things into an array.
	         */
	        rules = new TransliterationRule[v.Count];
	        v.CopyTo(rules);
	
	        StringBuilder errors = null;
	
	        /*
	         * Check for masking. This is MUCH faster than our old check, which was
	         * each rule against each following rule, since we only have to check
	         * for masking within each bin now. It's 256*O(n2^2) instead of O(n1^2),
	         * where n1 is the total rule count, and n2 is the per-bin rule count.
	         * But n2<<n1, so it's a big win.
	         */
	        for (int x_2 = 0; x_2 < 256; ++x_2) {
	            for (int j_3 = index[x_2]; j_3 < index[x_2 + 1] - 1; ++j_3) {
	                TransliterationRule r1 = rules[j_3];
	                for (int k = j_3 + 1; k < index[x_2 + 1]; ++k) {
	                    TransliterationRule r2 = rules[k];
	                    if (r1.Masks(r2)) {
	                        if (errors == null) {
	                            errors = new StringBuilder();
	                        } else {
	                            errors.Append("\n");
	                        }
	                        errors.Append("Rule " + r1 + " masks " + r2);
	                    }
	                }
	            }
	        }
	
	        if (errors != null) {
	            throw new ArgumentException(errors.ToString());
	        }
	    }
	
	    /// <summary>
	    /// Transliterate the given text with the given UTransPosition indices.
	    /// Return TRUE if the transliteration should continue or FALSE if it should
	    /// halt (because of a U_PARTIAL_MATCH match). Note that FALSE is only ever
	    /// returned if isIncremental is TRUE.
	    /// </summary>
	    ///
	    /// <param name="text">the text to be transliterated</param>
	    /// <param name="pos">the position indices, which will be updated</param>
	    /// <param name="incremental">if TRUE, assume new text may be inserted at index.limit, andreturn FALSE if thre is a partial match.</param>
	    /// <returns>TRUE unless a U_PARTIAL_MATCH has been obtained, indicating that
	    /// transliteration should stop until more text arrives.</returns>
	    public bool Transliterate(Replaceable text, Transliterator.Position pos,
	            bool incremental) {
	        int indexByte = text.Char32At(pos.start) & 0xFF;
	        for (int i = index[indexByte]; i < index[indexByte + 1]; ++i) {
	            int m = rules[i].MatchAndReplace(text, pos, incremental);
	            switch (m) {
	            case IBM.ICU.Text.UnicodeMatcher_Constants.U_MATCH:
	                if (IBM.ICU.Text.Transliterator.DEBUG) {
	                    System.Console.Out.WriteLine(((incremental) ? "Rule.i: match "
	                            : "Rule: match ")
	                            + rules[i].ToRule(true)
	                            + " => "
	                            + IBM.ICU.Impl.UtilityExtensions.FormatInput(text, pos));
	                }
	                return true;
	            case IBM.ICU.Text.UnicodeMatcher_Constants.U_PARTIAL_MATCH:
	                if (IBM.ICU.Text.Transliterator.DEBUG) {
	                    System.Console.Out
	                            .WriteLine(((incremental) ? "Rule.i: partial match "
	                                    : "Rule: partial match ")
	                                    + rules[i].ToRule(true)
	                                    + " => "
	                                    + IBM.ICU.Impl.UtilityExtensions.FormatInput(text, pos));
	                }
	                return false;
	            default:
	                if (IBM.ICU.Text.Transliterator.DEBUG) {
	                    System.Console.Out.WriteLine("Rule: no match " + rules[i]);
	                }
	                break;
	            }
	        }
	        // No match or partial match from any rule
	        pos.start += IBM.ICU.Text.UTF16.GetCharCount(text.Char32At(pos.start));
	        if (IBM.ICU.Text.Transliterator.DEBUG) {
	            System.Console.Out.WriteLine(((incremental) ? "Rule.i: no match => "
	                    : "Rule: no match => ")
	                    + IBM.ICU.Impl.UtilityExtensions.FormatInput(text, pos));
	        }
	        return true;
	    }
	
	    /// <summary>
	    /// Create rule strings that represents this rule set.
	    /// </summary>
	    ///
	    internal String ToRules(bool escapeUnprintable) {
	        int i;
	        int count = ruleVector.Count;
	        StringBuilder ruleSource = new StringBuilder();
	        for (i = 0; i < count; ++i) {
	            if (i != 0) {
	                ruleSource.Append('\n');
	            }
	            TransliterationRule r = (TransliterationRule) ruleVector[i];
	            ruleSource.Append(r.ToRule(escapeUnprintable));
	        }
	        return ruleSource.ToString();
	    }
	
	    /// <summary>
	    /// Return the set of all characters that may be modified (getTarget=false)
	    /// or emitted (getTarget=true) by this set.
	    /// </summary>
	    ///
	    internal UnicodeSet GetSourceTargetSet(bool getTarget) {
	        UnicodeSet set = new UnicodeSet();
	        int count = ruleVector.Count;
	        for (int i = 0; i < count; ++i) {
	            TransliterationRule r = (TransliterationRule) ruleVector[i];
	            if (getTarget) {
	                r.AddTargetSetTo(set);
	            } else {
	                r.AddSourceSetTo(set);
	            }
	        }
	        return set;
	    }
	}
}