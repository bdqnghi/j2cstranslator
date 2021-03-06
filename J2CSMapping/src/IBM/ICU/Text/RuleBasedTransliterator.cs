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
	
	/// <exclude/>
	/// <summary>
	/// <c>RuleBasedTransliterator</c> is a transliterator that reads a set of
	/// rules in order to determine how to perform translations. Rule sets are stored
	/// in resource bundles indexed by name. Rules within a rule set are separated by
	/// semicolons (';'). To include a literal semicolon, prefix it with a backslash
	/// ('\'). Whitespace, as defined by
	/// <c>UCharacterProperty.isRuleWhiteSpace()</c>, is ignored. If the first
	/// non-blank character on a line is '#', the entire line is ignored as a
	/// comment. </p>
	/// <p>
	/// Each set of rules consists of two groups, one forward, and one reverse. This
	/// is a convention that is not enforced; rules for one direction may be omitted,
	/// with the result that translations in that direction will not modify the
	/// source text. In addition, bidirectional forward-reverse rules may be
	/// specified for symmetrical transformations.
	/// </p>
	/// <p>
	/// <b>Rule syntax</b>
	/// </p>
	/// <p>
	/// Rule statements take one of the following forms:
	/// </p>
	/// <dl>
	/// <dt><c>$alefmadda=\u0622;</c></dt>
	/// <dd><strong>Variable definition.</strong> The name on the left is assigned
	/// the text on the right. In this example, after this statement, instances of
	/// the left hand name, &quot;<c>$alefmadda</c>&quot;, will be replaced by
	/// the Unicode character U+0622. Variable names must begin with a letter and
	/// consist only of letters, digits, and underscores. Case is significant.
	/// Duplicate names cause an exception to be thrown, that is, variables cannot be
	/// redefined. The right hand side may contain well-formed text of any length,
	/// including no text at all (&quot;<c>$empty=;</c>&quot;). The right hand
	/// side may contain embedded <c>UnicodeSet</c> patterns, for example,
	/// &quot;<c>$softvowel=[eiyEIY]</c>&quot;.</dd>
	/// <dd>&nbsp;</dd>
	/// <dt><c>ai&gt;$alefmadda;</c></dt>
	/// <dd><strong>Forward translation rule.</strong> This rule states that the
	/// string on the left will be changed to the string on the right when performing
	/// forward transliteration.</dd>
	/// <dt>&nbsp;</dt>
	/// <dt><c>ai&lt;$alefmadda;</c></dt>
	/// <dd><strong>Reverse translation rule.</strong> This rule states that the
	/// string on the right will be changed to the string on the left when performing
	/// reverse transliteration.</dd>
	/// </dl>
	/// <dl>
	/// <dt><c>ai&lt;&gt;$alefmadda;</c></dt>
	/// <dd><strong>Bidirectional translation rule.</strong> This rule states that
	/// the string on the right will be changed to the string on the left when
	/// performing forward transliteration, and vice versa when performing reverse
	/// transliteration.</dd>
	/// </dl>
	/// <p>
	/// Translation rules consist of a <em>match pattern</em> and an <em>output
	/// string</em>. The match pattern consists of literal characters, optionally
	/// preceded by context, and optionally followed by context. Context characters,
	/// like literal pattern characters, must be matched in the text being
	/// transliterated. However, unlike literal pattern characters, they are not
	/// replaced by the output text. For example, the pattern &quot;
	/// <c>abc{def}</c>&quot; indicates the characters &quot;<c>def</c>
	/// &quot; must be preceded by &quot;<c>abc</c>&quot; for a successful
	/// match. If there is a successful match, &quot;<c>def</c>&quot; will be
	/// replaced, but not &quot;<c>abc</c>&quot;. The final '<c>}</c>' is
	/// optional, so &quot;<c>abc{def</c>&quot; is equivalent to &quot;
	/// <c>abc{def}</c>&quot;. Another example is &quot;<c>{123}456</c>
	/// &quot; (or &quot;<c>123}456</c>&quot;) in which the literal pattern
	/// &quot;<c>123</c>&quot; must be followed by &quot;<c>456</c>
	/// &quot;.
	/// </p>
	/// <p>
	/// The output string of a forward or reverse rule consists of characters to
	/// replace the literal pattern characters. If the output string contains the
	/// character '<c>|</c>', this is taken to indicate the location of the
	/// <em>cursor</em> after replacement. The cursor is the point in the text at
	/// which the next replacement, if any, will be applied. The cursor is usually
	/// placed within the replacement text; however, it can actually be placed into
	/// the precending or following context by using the special character '
	/// <c>@</c>'. Examples:
	/// </p>
	/// <blockquote>
	/// <p>
	/// <code>a {foo} z &gt; | @ bar; # foo -&gt; bar, move cursor
	/// before a<br>
	/// {foo} xyz &gt; bar @@|; #&nbsp;foo -&gt; bar, cursor between
	/// y and z</code>
	/// </p>
	/// </blockquote>
	/// <p>
	/// <b>UnicodeSet</b>
	/// </p>
	/// <p>
	/// <c>UnicodeSet</c> patterns may appear anywhere that makes sense. They
	/// may appear in variable definitions. Contrariwise, <c>UnicodeSet</c>
	/// patterns may themselves contain variable references, such as &quot;
	/// <c>$a=[a-z];$not_a=[^$a]</c>&quot;, or &quot;
	/// <c>$range=a-z;$ll=[$range]</c>&quot;.
	/// </p>
	/// <p>
	/// <c>UnicodeSet</c> patterns may also be embedded directly into rule
	/// strings. Thus, the following two rules are equivalent:
	/// </p>
	/// <blockquote>
	/// <p>
	/// <code>$vowel=[aeiou]; $vowel&gt;'///'; # One way to do this<br>
	/// [aeiou]&gt;'///';
	/// &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;#
	/// Another way</code>
	/// </p>
	/// </blockquote>
	/// <p>
	/// See <see cref="T:IBM.ICU.Text.UnicodeSet"/> for more documentation and examples.
	/// </p>
	/// <p>
	/// <b>Segments</b>
	/// </p>
	/// <p>
	/// Segments of the input string can be matched and copied to the output string.
	/// This makes certain sets of rules simpler and more general, and makes
	/// reordering possible. For example:
	/// </p>
	/// <blockquote>
	/// <p>
	/// <code>([a-z]) &gt; $1 $1;
	/// &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;#
	/// double lowercase letters<br>
	/// ([:Lu:]) ([:Ll:]) &gt; $2 $1; # reverse order of Lu-Ll pairs</code>
	/// </p>
	/// </blockquote>
	/// <p>
	/// The segment of the input string to be copied is delimited by &quot;
	/// <c>(</c>&quot; and &quot;<c>)</c>&quot;. Up to nine segments may
	/// be defined. Segments may not overlap. In the output string, &quot;
	/// <c>$1</c>&quot; through &quot;<c>$9</c>&quot; represent the input
	/// string segments, in left-to-right order of definition.
	/// </p>
	/// <p>
	/// <b>Anchors</b>
	/// </p>
	/// <p>
	/// Patterns can be anchored to the beginning or the end of the text. This is
	/// done with the special characters '<c>^</c>' and '<c>$</c>'. For
	/// example:
	/// </p>
	/// <blockquote>
	/// <p>
	/// <code>^ a&nbsp;&nbsp; &gt; 'BEG_A'; &nbsp;&nbsp;# match 'a' at start of text<br>
	/// &nbsp; a&nbsp;&nbsp; &gt; 'A';&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; # match other instances
	/// of 'a'<br>
	/// &nbsp; z $ &gt; 'END_Z'; &nbsp;&nbsp;# match 'z' at end of text<br>
	/// &nbsp; z&nbsp;&nbsp; &gt; 'Z';&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; # match other instances
	/// of 'z'</code>
	/// </p>
	/// </blockquote>
	/// <p>
	/// It is also possible to match the beginning or the end of the text using a
	/// <c>UnicodeSet</c>. This is done by including a virtual anchor character
	/// '<c>$</c>' at the end of the set pattern. Although this is usually the
	/// match chafacter for the end anchor, the set will match either the beginning
	/// or the end of the text, depending on its placement. For example:
	/// </p>
	/// <blockquote>
	/// <p>
	/// <code>$x = [a-z$]; &nbsp;&nbsp;# match 'a' through 'z' OR anchor<br>
	/// $x 1&nbsp;&nbsp;&nbsp; &gt; 2;&nbsp;&nbsp; # match '1' after a-z or at the start<br>
	/// &nbsp;&nbsp; 3 $x &gt; 4; &nbsp;&nbsp;# match '3' before a-z or at the end</code>
	/// </p>
	/// </blockquote>
	/// <p>
	/// <b>Example</b>
	/// </p>
	/// <p>
	/// The following example rules illustrate many of the features of the rule
	/// language.
	/// </p>
	/// <table border="0" cellpadding="4">
	/// <tr>
	/// <td valign="top">Rule 1.</td>
	/// <td valign="top" nowrap><c>abc{def}&gt;x|y</c></td>
	/// </tr>
	/// <tr>
	/// <td valign="top">Rule 2.</td>
	/// <td valign="top" nowrap><c>xyz&gt;r</c></td>
	/// </tr>
	/// <tr>
	/// <td valign="top">Rule 3.</td>
	/// <td valign="top" nowrap><c>yz&gt;q</c></td>
	/// </tr>
	/// </table>
	/// <p>
	/// Applying these rules to the string &quot;<c>adefabcdefz</c>&quot;
	/// yields the following results:
	/// </p>
	/// <table border="0" cellpadding="4">
	/// <tr>
	/// <td valign="top" nowrap><c>|adefabcdefz</c></td>
	/// <td valign="top">Initial state, no rules match. Advance cursor.</td>
	/// </tr>
	/// <tr>
	/// <td valign="top" nowrap><c>a|defabcdefz</c></td>
	/// <td valign="top">Still no match. Rule 1 does not match because the preceding
	/// context is not present.</td>
	/// </tr>
	/// <tr>
	/// <td valign="top" nowrap><c>ad|efabcdefz</c></td>
	/// <td valign="top">Still no match. Keep advancing until there is a match...</td>
	/// </tr>
	/// <tr>
	/// <td valign="top" nowrap><c>ade|fabcdefz</c></td>
	/// <td valign="top">...</td>
	/// </tr>
	/// <tr>
	/// <td valign="top" nowrap><c>adef|abcdefz</c></td>
	/// <td valign="top">...</td>
	/// </tr>
	/// <tr>
	/// <td valign="top" nowrap><c>adefa|bcdefz</c></td>
	/// <td valign="top">...</td>
	/// </tr>
	/// <tr>
	/// <td valign="top" nowrap><c>adefab|cdefz</c></td>
	/// <td valign="top">...</td>
	/// </tr>
	/// <tr>
	/// <td valign="top" nowrap><c>adefabc|defz</c></td>
	/// <td valign="top">Rule 1 matches; replace &quot;<c>def</c>&quot; with
	/// &quot;<c>xy</c>&quot; and back up the cursor to before the '
	/// <c>y</c>'.</td>
	/// </tr>
	/// <tr>
	/// <td valign="top" nowrap><c>adefabcx|yz</c></td>
	/// <td valign="top">Although &quot;<c>xyz</c>&quot; is present, rule 2
	/// does not match because the cursor is before the '<c>y</c>', not before
	/// the '<c>x</c>'. Rule 3 does match. Replace &quot;<c>yz</c>&quot;
	/// with &quot;<c>q</c>&quot;.</td>
	/// </tr>
	/// <tr>
	/// <td valign="top" nowrap><c>adefabcxq|</c></td>
	/// <td valign="top">The cursor is at the end; transliteration is complete.</td>
	/// </tr>
	/// </table>
	/// <p>
	/// The order of rules is significant. If multiple rules may match at some point,
	/// the first matching rule is applied.
	/// </p>
	/// <p>
	/// Forward and reverse rules may have an empty output string. Otherwise, an
	/// empty left or right hand side of any statement is a syntax error.
	/// </p>
	/// <p>
	/// Single quotes are used to quote any character other than a digit or letter.
	/// To specify a single quote itself, inside or outside of quotes, use two single
	/// quotes in a row. For example, the rule &quot;<c>'&gt;'&gt;o''clock</c>
	/// &quot; changes the string &quot;<c>&gt;</c>&quot; to the string &quot;
	/// <c>o'clock</c>&quot;.
	/// </p>
	/// <p>
	/// <b>Notes</b>
	/// </p>
	/// <p>
	/// While a RuleBasedTransliterator is being built, it checks that the rules are
	/// added in proper order. For example, if the rule &quot;a&gt;x&quot; is
	/// followed by the rule &quot;ab&gt;y&quot;, then the second rule will throw an
	/// exception. The reason is that the second rule can never be triggered, since
	/// the first rule always matches anything it matches. In other words, the first
	/// rule <em>masks</em> the second rule.
	/// </p>
	/// <p>
	/// Copyright (c) IBM Corporation 1999-2000. All rights reserved.
	/// </p>
	/// </summary>
	///
	public class RuleBasedTransliterator : Transliterator {
	
	    private RuleBasedTransliterator.Data  data;
	
	    /**
	     * Constructs a new transliterator from the given rules.
	     * 
	     * @param rules
	     *            rules, separated by ';'
	     * @param direction
	     *            either FORWARD or REVERSE.
	     * @exception IllegalArgumentException
	     *                if rules are malformed or direction is invalid.
	     * @internal
	     * @deprecated This API is ICU internal only.
	     */
	    /*
	     * public RuleBasedTransliterator(String ID, String rules, int direction,
	     * UnicodeFilter filter) { super(ID, filter); if (direction != FORWARD &&
	     * direction != REVERSE) { throw new
	     * IllegalArgumentException("Invalid direction"); }
	     * 
	     * TransliteratorParser parser = new TransliteratorParser();
	     * parser.parse(rules, direction); if (parser.idBlockVector.size() != 0 ||
	     * parser.compoundFilter != null) { throw new IllegalArgumentException(
	     * "::ID blocks illegal in RuleBasedTransliterator constructor"); }
	     * 
	     * data = (Data)parser.dataVector.get(0);
	     * setMaximumContextLength(data.ruleSet.getMaximumContextLength()); }
	     */
	
	    /// <exclude/>
	    /// <summary>
	    /// Constructs a new transliterator from the given rules in the
	    /// <c>FORWARD</c> direction.
	    /// </summary>
	    ///
	    /// <param name="rules">rules, separated by ';'</param>
	    /// <exception cref="IllegalArgumentException">if rules are malformed or direction is invalid.</exception>
	    internal /*
	     * public RuleBasedTransliterator(String ID, String rules) { this(ID, rules,
	     * FORWARD, null); }
	     */
	
	    RuleBasedTransliterator(String ID, RuleBasedTransliterator.Data  data_0, UnicodeFilter filter) : base(ID, filter) {
	        this.data = data_0;
	        SetMaximumContextLength(data_0.ruleSet.GetMaximumContextLength());
	    }
	
	    /// <exclude/>
	    /// <summary>
	    /// Implements <see cref="M:IBM.ICU.Text.Transliterator.HandleTransliterate(IBM.ICU.Text.Replaceable, null, System.Boolean)"/>.
	    /// </summary>
	    ///
	    [MethodImpl(MethodImplOptions.Synchronized)]
	    protected internal override void HandleTransliterate(Replaceable text,
	            Transliterator.Position  index, bool incremental) {
	        /*
	         * We keep start and limit fixed the entire time, relative to the text
	         * -- limit may move numerically if text is inserted or removed. The
	         * cursor moves from start to limit, with replacements happening under
	         * it.
	         * 
	         * Example: rules 1. ab>x|y 2. yc>z
	         * 
	         * |eabcd start - no match, advance cursor e|abcd match rule 1 - change
	         * text & adjust cursor ex|ycd match rule 2 - change text & adjust
	         * cursor exz|d no match, advance cursor exzd| done
	         */
	
	        /*
	         * A rule like a>b|a creates an infinite loop. To prevent that, we put
	         * an arbitrary limit on the number of iterations that we take, one that
	         * is high enough that any reasonable rules are ok, but low enough to
	         * prevent a server from hanging. The limit is 16 times the number of
	         * characters n, unless n is so large that 16n exceeds a uint32_t.
	         */
	        int loopCount = 0;
	        int loopLimit = (index.limit - index.start) << 4;
	        if (loopLimit < 0) {
	            loopLimit = 0x7FFFFFFF;
	        }
	
	        while (index.start < index.limit && loopCount <= loopLimit
	                && data.ruleSet.Transliterate(text, index, incremental)) {
	            ++loopCount;
	        }
	    }
	
	    internal class Data {
	        public Data() {
	            variableNames = new Hashtable();
	            ruleSet = new TransliterationRuleSet();
	        }
	
	        /// <summary>
	        /// Rule table. May be empty.
	        /// </summary>
	        ///
	        public TransliterationRuleSet ruleSet;
	
	        /// <summary>
	        /// Map variable name (String) to variable (char[]). A variable name
	        /// corresponds to zero or more characters, stored in a char[] array in
	        /// this hash. One or more of these chars may also correspond to a
	        /// UnicodeSet, in which case the character in the char[] in this hash is
	        /// a stand-in: it is an index for a secondary lookup in data.variables.
	        /// The stand-in also represents the UnicodeSet in the stored rules.
	        /// </summary>
	        ///
	        internal Hashtable variableNames;
	
	        /// <summary>
	        /// Map category variable (Character) to UnicodeMatcher or
	        /// UnicodeReplacer. Variables that correspond to a set of characters are
	        /// mapped from variable name to a stand-in character in
	        /// data.variableNames. The stand-in then serves as a key in this hash to
	        /// lookup the actual UnicodeSet object. In addition, the stand-in is
	        /// stored in the rule text to represent the set of characters.
	        /// variables[i] represents character (variablesBase + i).
	        /// </summary>
	        ///
	        internal Object[] variables;
	
	        /// <summary>
	        /// The character that represents variables[0]. Characters variablesBase
	        /// through variablesBase + variables.length - 1 represent UnicodeSet
	        /// objects.
	        /// </summary>
	        ///
	        internal char variablesBase;
	
	        /// <summary>
	        /// Return the UnicodeMatcher represented by the given character, or null
	        /// if none.
	        /// </summary>
	        ///
	        public UnicodeMatcher LookupMatcher(int standIn) {
	            int i = standIn - variablesBase;
	            return (i >= 0 && i < variables.Length) ? (UnicodeMatcher) variables[i]
	                    : null;
	        }
	
	        /// <summary>
	        /// Return the UnicodeReplacer represented by the given character, or
	        /// null if none.
	        /// </summary>
	        ///
	        public UnicodeReplacer LookupReplacer(int standIn) {
	            int i = standIn - variablesBase;
	            return (i >= 0 && i < variables.Length) ? (UnicodeReplacer) variables[i]
	                    : null;
	        }
	    }
	
	    /// <exclude/>
	    /// <summary>
	    /// Return a representation of this transliterator as source rules. These
	    /// rules will produce an equivalent transliterator if used to construct a
	    /// new transliterator.
	    /// </summary>
	    ///
	    /// <param name="escapeUnprintable">if TRUE then convert unprintable character to their hex escaperepresentations, \\uxxxx or \\Uxxxxxxxx. Unprintablecharacters are those other than U+000A, U+0020..U+007E.</param>
	    /// <returns>rules string</returns>
	    public override String ToRules(bool escapeUnprintable) {
	        return data.ruleSet.ToRules(escapeUnprintable);
	    }
	
	    /// <exclude/>
	    /// <summary>
	    /// Return the set of all characters that may be modified by this
	    /// Transliterator, ignoring the effect of our filter.
	    /// </summary>
	    ///
	    protected internal override UnicodeSet HandleGetSourceSet() {
	        return data.ruleSet.GetSourceTargetSet(false);
	    }
	
	    /// <exclude/>
	    /// <summary>
	    /// Returns the set of all characters that may be generated as replacement
	    /// text by this transliterator.
	    /// </summary>
	    ///
	    public override UnicodeSet GetTargetSet() {
	        return data.ruleSet.GetSourceTargetSet(true);
	    }
	}
}
