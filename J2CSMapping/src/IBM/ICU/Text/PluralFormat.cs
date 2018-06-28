/*
 *******************************************************************************
 * Copyright (C) 2007, International Business Machines Corporation and         *
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
	
	using IBM.ICU.Util;
	using ILOG.J2CsMapping.Collections;
	using ILOG.J2CsMapping.Collections.Generics;
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.IO;
	using System.Runtime.CompilerServices;
	using System.Text;
     using ILOG.J2CsMapping.Util;
     using ILOG.J2CsMapping.Text;
	
	/// <summary>
	/// <p>
	/// <c>PluralFormat</c> supports the creation of internationalized messages
	/// with plural inflection. It is based on <i>plural selection</i>, i.e. the
	/// caller specifies messages for each plural case that can appear in the users
	/// language and the <c>PluralFormat</c> selects the appropriate message
	/// based on the number.
	/// </p>
	/// <h4>The Problem of Plural Forms in Internationalized Messages</h4>
	/// <p>
	/// Different languages have different ways to inflect plurals. Creating
	/// internationalized messages that include plural forms is only feasible when
	/// the framework is able to handle plural forms of <i>all</i> languages
	/// correctly. <c>ChoiceFormat</c> doesn't handle this well, because it
	/// attaches a number interval to each message and selects the message whose
	/// interval contains a given number. This can only handle a finite number of
	/// intervals. But in some languages, like Polish, one plural case applies to
	/// infinitely many intervals (e.g., paucal applies to numbers ending with 2, 3,
	/// or 4 except those ending with 12, 13, or 14). Thus <c>ChoiceFormat</c>
	/// is not adequate.
	/// </p>
	/// <p>
	/// <c>PluralFormat</c> deals with this by breaking the problem into two
	/// parts:
	/// <ul>
	/// <li>It uses <c>PluralRules</c> that can define more complex conditions
	/// for a plural case than just a single interval. These plural rules define both
	/// what plural cases exist in a language, and to which numbers these cases
	/// apply.
	/// <li>It provides predefined plural rules for many locales. Thus, the
	/// programmer need not worry about the plural cases of a language. On the flip
	/// side, the localizer does not have to specify the plural cases; he can simply
	/// use the predefined keywords. The whole plural formatting of messages can be
	/// done using localized patterns from resource bundles.
	/// </ul>
	/// </p>
	/// <h4>Usage of <c>PluralFormat</c></h4>
	/// <p>
	/// This discussion assumes that you use <c>PluralFormat</c> with a
	/// predefined set of plural rules. You can create one using one of the
	/// constructors that takes a <c>ULocale</c> object. To specify the message
	/// pattern, you can either pass it to the constructor or set it explicitly using
	/// the <c>applyPattern()</c> method. The <c>format()</c> method
	/// takes a number object and selects the message of the matching plural case.
	/// This message will be returned.
	/// </p>
	/// <h5>Patterns and Their Interpretation</h5>
	/// <p>
	/// The pattern text defines the message output for each plural case of the used
	/// locale. The pattern is a sequence of
	/// <code><i>caseKeyword</i>{<i>message</i>}</code> clauses, separated by white
	/// space characters. Each clause assigns the message <code><i>message</i></code>
	/// to the plural case identified by <code><i>caseKeyword</i></code>.
	/// </p>
	/// <p>
	/// You always have to define a message text for the default plural case "
	/// <c>other</c>" which is contained in every rule set. If the plural rules
	/// of the <c>PluralFormat</c> object do not contain a plural case
	/// identified by <code><i>caseKeyword</i></code>, an
	/// <c>IllegalArgumentException</c> is thrown. If you do not specify a
	/// message text for a particular plural case, the message text of the plural
	/// case "<c>other</c>" gets assigned to this plural case. If you specify
	/// more than one message for the same plural case, an
	/// <c>IllegalArgumentException</c> is thrown. <br/>
	/// Spaces between <code><i>caseKeyword</i></code> and
	/// <code><i>message</i></code> will be ignored; spaces within
	/// <code><i>message</i></code> will be preserved.
	/// </p>
	/// <p>
	/// The message text for a particular plural case may contain other message
	/// format patterns. <c>PluralFormat</c> preserves these so that you can
	/// use the strings produced by <c>PluralFormat</c> with other formatters.
	/// If you are using <c>PluralFormat</c> inside a
	/// <c>MessageFormat</c> pattern, <c>MessageFormat</c> will
	/// automatically evaluate the resulting format pattern.<br/>
	/// Thus, curly braces (<c>{</c>, <c>}</c>) are <i>only</i> allowed
	/// in message texts to define a nested format pattern.<br/>
	/// The pound sign (<c>#</c>) will be interpreted as the number placeholder
	/// in the message text, if it is not contained in curly braces (to preserve
	/// <c>NumberFormat</c> patterns). <c>PluralFormat</c> will replace
	/// each of those pound signs by the number passed to the <c>format()</c>
	/// method. It will be formatted using a <c>NumberFormat</c> for the
	/// <c>PluralFormat</c>'s locale. If you need special number formatting,
	/// you have to explicitly specify a <c>NumberFormat</c> for the
	/// <c>PluralFormat</c> to use.
	/// </p>
	/// Example
	/// <pre>
	/// MessageFormat msgFmt = new MessageFormat("{0, plural, " +
	/// "singular{{0, number, C''''est #,##0.0#  fichier}} " +
	/// "other {Ce sont # fichiers}} dans la liste.",
	/// new ULocale("fr"));
	/// Object args[] = {new Long(0)};
	/// System.out.println(msgFmt.format(args));
	/// args = {new Long(3)};
	/// System.out.println(msgFmt.format(args));
	/// </pre>
	/// Produces the output:<br />
	/// <c>C'est 0,0 fichier dans la liste.</c><br />
	/// <c>Ce sont 3 fichiers dans la liste."</c>
	/// <p>
	/// <strong>Note:</strong><br />
	/// Currently <c>PluralFormat</c> does not make use of quotes like
	/// <c>MessageFormat</c>. If you use plural format strings with
	/// <c>MessageFormat</c> and want to use a quote sign "<c>'</c>
	/// ", you have to write "<c>''</c>". <c>MessageFormat</c> unquotes
	/// this pattern and passes the unquoted pattern to <c>PluralFormat</c>.
	/// It's a bit trickier if you use nested formats that do quoting. In the example
	/// above, we wanted to insert "<c>'</c>" in the number format pattern.
	/// Since <c>NumberFormat</c> supports quotes, we had to insert "
	/// <c>''</c>". But since <c>MessageFormat</c> unquotes the pattern
	/// before it gets passed to <c>PluralFormat</c>, we have to double these
	/// quotes, i.e. write "<c>''''</c>".
	/// </p>
	/// <h4>Defining Custom Plural Rules</h4>
	/// <p>
	/// If you need to use <c>PluralFormat</c> with custom rules, you can
	/// create a <c>PluralRules</c> object and pass it to
	/// <c>PluralFormat</c>'s constructor. If you also specify a locale in this
	/// constructor, this locale will be used to format the number in the message
	/// texts.
	/// </p>
	/// <p>
	/// For more information about <c>PluralRules</c>, see <see cref="T:IBM.ICU.Text.PluralRules"/>.
	/// </p>
	/// </summary>
	///
	/// @draft ICU 3.8
	/// @provisional This API might change or be removed in a future release.
	public class PluralFormat : UFormat {
	    private const long serialVersionUID = 1L;
	
	    /// <summary>
	    /// The locale used for standard number formatting and getting the predefined
	    /// plural rules (if they were not defined explicitely).
	    /// </summary>
	    ///
	    private ULocale ulocale;
	
	    /// <summary>
	    /// The plural rules used for plural selection.
	    /// </summary>
	    ///
	    private PluralRules pluralRules;
	
	    /// <summary>
	    /// The applied pattern string.
	    /// </summary>
	    ///
	    private String pattern;
	
	    /// <summary>
	    /// The format messages for each plural case. It is a mapping:
	    /// <c>String</c>(plural case keyword) --&gt; <c>String</c>
	    /// (message for this plural case).
	    /// </summary>
	    ///
	    private IDictionary parsedValues;
	
	    /// <summary>
	    /// This <c>NumberFormat</c> is used for the standard formatting of the
	    /// number inserted into the message.
	    /// </summary>
	    ///
	    private NumberFormat numberFormat;
	
	    /// <summary>
	    /// Creates a new <c>PluralFormat</c> for the default locale. This
	    /// locale will be used to get the set of plural rules and for standard
	    /// number formatting.
	    /// </summary>
	    ///
	    /// @draft ICU 3.8
	    /// @provisional This API might change or be removed in a future release.
	    public PluralFormat() {
	        this.ulocale = null;
	        this.pluralRules = null;
	        this.pattern = null;
	        this.parsedValues = null;
	        this.numberFormat = null;
	        Init(null, IBM.ICU.Util.ULocale.GetDefault());
	    }
	
	    /// <summary>
	    /// Creates a new <c>PluralFormat</c> for a given locale.
	    /// </summary>
	    ///
	    /// <param name="ulocale_0">the <c>PluralFormat</c> will be configured with rulesfor this locale. This locale will also be used for standardnumber formatting.</param>
	    /// @draft ICU 3.8
	    /// @provisional This API might change or be removed in a future release.
	    public PluralFormat(ULocale ulocale_0) {
	        this.ulocale = null;
	        this.pluralRules = null;
	        this.pattern = null;
	        this.parsedValues = null;
	        this.numberFormat = null;
	        Init(null, ulocale_0);
	    }
	
	    /// <summary>
	    /// Creates a new <c>PluralFormat</c> for a given set of rules. The
	    /// standard number formatting will be done using the default locale.
	    /// </summary>
	    ///
	    /// <param name="rules">defines the behavior of the <c>PluralFormat</c> object.</param>
	    /// @draft ICU 3.8
	    /// @provisional This API might change or be removed in a future release.
	    public PluralFormat(PluralRules rules) {
	        this.ulocale = null;
	        this.pluralRules = null;
	        this.pattern = null;
	        this.parsedValues = null;
	        this.numberFormat = null;
	        Init(rules, IBM.ICU.Util.ULocale.GetDefault());
	    }
	
	    /// <summary>
	    /// Creates a new <c>PluralFormat</c> for a given set of rules. The
	    /// standard number formatting will be done using the given locale.
	    /// </summary>
	    ///
	    /// <param name="ulocale_0">the default number formatting will be done using this locale.</param>
	    /// <param name="rules">defines the behavior of the <c>PluralFormat</c> object.</param>
	    /// @draft ICU 3.8
	    /// @provisional This API might change or be removed in a future release.
	    public PluralFormat(ULocale ulocale_0, PluralRules rules) {
	        this.ulocale = null;
	        this.pluralRules = null;
	        this.pattern = null;
	        this.parsedValues = null;
	        this.numberFormat = null;
	        Init(rules, ulocale_0);
	    }
	
	    /// <summary>
	    /// Creates a new <c>PluralFormat</c> for a given pattern string. The
	    /// default locale will be used to get the set of plural rules and for
	    /// standard number formatting.
	    /// </summary>
	    ///
	    /// <param name="pattern_0">the pattern for this <c>PluralFormat</c>.</param>
	    /// <exception cref="IllegalArgumentException">if the pattern is invalid.</exception>
	    /// @draft ICU 3.8
	    /// @provisional This API might change or be removed in a future release.
	    public PluralFormat(String pattern_0) {
	        this.ulocale = null;
	        this.pluralRules = null;
	        this.pattern = null;
	        this.parsedValues = null;
	        this.numberFormat = null;
	        Init(null, IBM.ICU.Util.ULocale.GetDefault());
	        ApplyPattern(pattern_0);
	    }
	
	    /// <summary>
	    /// Creates a new <c>PluralFormat</c> for a given pattern string and
	    /// locale. The locale will be used to get the set of plural rules and for
	    /// standard number formatting.
	    /// </summary>
	    ///
	    /// <param name="ulocale_0">the <c>PluralFormat</c> will be configured with rulesfor this locale. This locale will also be used for standardnumber formatting.</param>
	    /// <param name="pattern_1">the pattern for this <c>PluralFormat</c>.</param>
	    /// <exception cref="IllegalArgumentException">if the pattern is invalid.</exception>
	    /// @draft ICU 3.8
	    /// @provisional This API might change or be removed in a future release.
	    public PluralFormat(ULocale ulocale_0, String pattern_1) {
	        this.ulocale = null;
	        this.pluralRules = null;
	        this.pattern = null;
	        this.parsedValues = null;
	        this.numberFormat = null;
	        Init(null, ulocale_0);
	        ApplyPattern(pattern_1);
	    }
	
	    /// <summary>
	    /// Creates a new <c>PluralFormat</c> for a given set of rules and a
	    /// pattern. The standard number formatting will be done using the default
	    /// locale.
	    /// </summary>
	    ///
	    /// <param name="rules">defines the behavior of the <c>PluralFormat</c> object.</param>
	    /// <param name="pattern_0">the pattern for this <c>PluralFormat</c>.</param>
	    /// <exception cref="IllegalArgumentException">if the pattern is invalid.</exception>
	    /// @draft ICU 3.8
	    /// @provisional This API might change or be removed in a future release.
	    public PluralFormat(PluralRules rules, String pattern_0) {
	        this.ulocale = null;
	        this.pluralRules = null;
	        this.pattern = null;
	        this.parsedValues = null;
	        this.numberFormat = null;
	        Init(rules, IBM.ICU.Util.ULocale.GetDefault());
	        ApplyPattern(pattern_0);
	    }
	
	    /// <summary>
	    /// Creates a new <c>PluralFormat</c> for a given set of rules, a
	    /// pattern and a locale.
	    /// </summary>
	    ///
	    /// <param name="ulocale_0">the <c>PluralFormat</c> will be configured with rulesfor this locale. This locale will also be used for standardnumber formatting.</param>
	    /// <param name="rules">defines the behavior of the <c>PluralFormat</c> object.</param>
	    /// <param name="pattern_1">the pattern for this <c>PluralFormat</c>.</param>
	    /// <exception cref="IllegalArgumentException">if the pattern is invalid.</exception>
	    /// @draft ICU 3.8
	    /// @provisional This API might change or be removed in a future release.
	    public PluralFormat(ULocale ulocale_0, PluralRules rules, String pattern_1) {
	        this.ulocale = null;
	        this.pluralRules = null;
	        this.pattern = null;
	        this.parsedValues = null;
	        this.numberFormat = null;
	        Init(rules, ulocale_0);
	        ApplyPattern(pattern_1);
	    }
	
	    /// <summary>
	    /// Initializes the <c>PluralRules</c> object. Postcondition:<br/>
	    /// <c>ulocale</c> : is <c>locale</c><br/>
	    /// <c>pluralRules</c>: if <c>rules</c> != <c>null</c> it's
	    /// set to rules, otherwise it is the predefined plural rule set for the
	    /// locale <c>ulocale</c>.<br/>
	    /// <c>parsedValues</c>: is <c>null</c><br/>
	    /// <c>pattern</c>: is <c>null</c><br/>
	    /// <c>numberFormat</c>: a <c>NumberFormat</c> for the locale
	    /// <c>ulocale</c>.
	    /// </summary>
	    ///
	    private void Init(PluralRules rules, ULocale locale) {
	        ulocale = locale;
	        pluralRules = (rules == null) ? IBM.ICU.Text.PluralRules.ForLocale(ulocale) : rules;
	        parsedValues = null;
	        pattern = null;
	        numberFormat = IBM.ICU.Text.NumberFormat.GetInstance(ulocale);
	    }
	
	    /// <summary>
	    /// Sets the pattern used by this plural format. The method parses the
	    /// pattern and creates a map of format strings for the plural rules.
	    /// Patterns and their interpretation are specified in the class description.
	    /// </summary>
	    ///
	    /// <param name="pattern_0">the pattern for this plural format.</param>
	    /// <exception cref="IllegalArgumentException">if the pattern is invalid.</exception>
	    /// @draft ICU 3.8
	    /// @provisional This API might change or be removed in a future release.
	    public void ApplyPattern(String pattern_0) {
	        this.pattern = pattern_0;
	        int braceStack = 0;
	        ILOG.J2CsMapping.Collections.ISet ruleNames = pluralRules.GetKeywords();
	        parsedValues = new Hashtable();
	
	        // Format string has to include keywords.
	        // states:
	        // 0: Reading keyword.
	        // 1: Reading value for preceding keyword.
	        int state = 0;
	        StringBuilder token = new StringBuilder();
	        String currentKeyword = null;
	        bool readSpaceAfterKeyword = false;
	        for (int i = 0; i < pattern_0.Length; ++i) {
	            char ch = pattern_0[i];
	            switch (state) {
	            case 0: // Reading value.
	                if (token.Length == 0) {
	                    readSpaceAfterKeyword = false;
	                }
	                if (IBM.ICU.Impl.UCharacterProperty.IsRuleWhiteSpace(ch)) {
	                    if (token.Length > 0) {
	                        readSpaceAfterKeyword = true;
	                    }
	                    // Skip leading and trailing whitespaces.
	                    break;
	                }
	                if (ch == '{') { // End of keyword definition reached.
	                    currentKeyword = token.ToString().ToLower(System.Globalization.CultureInfo.CreateSpecificCulture("en"));
	                    if (!ILOG.J2CsMapping.Collections.Collections.Contains(currentKeyword,ruleNames)) {
	                        ParsingFailure("Malformed formatting expression. "
	                                + "Unknown keyword \"" + currentKeyword
	                                + "\" at position " + i + ".");
	                    }
	                    if (ILOG.J2CsMapping.Collections.Collections.Get(parsedValues,currentKeyword) != null) {
	                        ParsingFailure("Malformed formatting expression. "
	                                + "Text for case \"" + currentKeyword
	                                + "\" at position " + i + " already defined!");
	                    }
	                    token.Remove(0,token.Length-(0));
	                    braceStack++;
	                    state = 1;
	                    break;
	                }
	                if (readSpaceAfterKeyword) {
	                    ParsingFailure("Malformed formatting expression. "
	                            + "Invalid keyword definition. Character \"" + ch
	                            + "\" at position " + i + " not expected!");
	                }
	                token.Append(ch);
	                break;
	            case 1: // Reading value.
	                switch ((int) ch) {
	                case '{':
	                    braceStack++;
	                    token.Append(ch);
	                    break;
	                case '}':
	                    braceStack--;
	                    if (braceStack == 0) { // End of value reached.
	                        ILOG.J2CsMapping.Collections.Collections.Put(parsedValues,currentKeyword,token.ToString());
	                        token.Remove(0,token.Length-(0));
	                        state = 0;
	                    } else if (braceStack < 0) {
	                        ParsingFailure("Malformed formatting expression. "
	                                + "Braces do not match.");
	                    } else { // braceStack > 0
	                        token.Append(ch);
	                    }
	                    break;
	                default:
	                    token.Append(ch);
	                    break;
	                }
	                break;
	            } // switch state
	        } // for loop.
	        if (braceStack != 0) {
	            ParsingFailure("Malformed formatting expression. Braces do not match.");
	        }
	        CheckSufficientDefinition();
	    }
	
	    /// <summary>
	    /// Formats a plural message for a given number.
	    /// </summary>
	    ///
	    /// <param name="number">a number for which the plural message should be formatted. Ifno pattern has been applied to this <c>PluralFormat</c>object yet, the formatted number will be returned.</param>
	    /// <returns>the string containing the formatted plural message.</returns>
	    /// @draft ICU 3.8
	    /// @provisional This API might change or be removed in a future release.
	    public String Format(long number) {
	        // If no pattern was applied, return the formatted number.
	        if (parsedValues == null) {
	            return numberFormat.Format(number);
	        }
	
	        // Get appropriate format pattern.
	        String selectedRule = pluralRules.Select(number);
	        String selectedPattern = (String) ILOG.J2CsMapping.Collections.Collections.Get(parsedValues,selectedRule);
	        if (selectedPattern == null) { // Fallback to others.
	            selectedPattern = (String) ILOG.J2CsMapping.Collections.Collections.Get(parsedValues,IBM.ICU.Text.PluralRules.KEYWORD_OTHER);
	        }
	        // Get formatted number and insert it into String.
	        // Will replace all '#' which are not inside curly braces by the
	        // formatted number.
	        return InsertFormattedNumber(number, selectedPattern);
	
	    }
	
	    /// <summary>
	    /// Formats a plural message for a given number and appends the formatted
	    /// message to the given <c>StringBuffer</c>.
	    /// </summary>
	    ///
	    /// <param name="number">a number object (instance of <c>Number</c> for which theplural message should be formatted. If no pattern has beenapplied to this <c>PluralFormat</c> object yet, theformatted number will be returned. Note: If this object is notan instance of <c>Number</c>, the<c>toAppendTo</c> will not be modified.</param>
	    /// <param name="toAppendTo">the formatted message will be appended to this<c>StringBuffer</c>.</param>
	    /// <param name="pos">will be ignored by this method.</param>
	    /// <returns>the string buffer passed in as toAppendTo, with formatted text
	    /// appended.</returns>
	    /// @draft ICU 3.8
	    /// @provisional This API might change or be removed in a future release.
	    public override StringBuilder FormatObject(Object number, StringBuilder toAppendTo,
	            FieldPosition pos) {
	        if (number  is  object) {
	            toAppendTo.Append(Format(Convert.ToInt64(((object) number))));
	            return toAppendTo;
	        }
	        throw new ArgumentException("'" + number + "' is not a Number");
	    }
	
	    /// <summary>
	    /// This method is not yet supported by <c>PluralFormat</c>.
	    /// </summary>
	    ///
	    /// <param name="text">the string to be parsed.</param>
	    /// <param name="parsePosition">defines the position where parsing is to begin, and uponreturn, the position where parsing left off. If the positionhas not changed upon return, then parsing failed.</param>
	    /// <returns>nothing because this method is not yet implemented.</returns>
	    /// <exception cref="UnsupportedOperationException">will always be thrown by this method.</exception>
	    /// @draft ICU 3.8
	    /// @provisional This API might change or be removed in a future release.
	    public object Parse(String text, ParsePosition parsePosition) {
	        throw new NotSupportedException();
	    }
	
	    /// <summary>
	    /// This method is not yet supported by <c>PluralFormat</c>.
	    /// </summary>
	    ///
	    /// <param name="source">the string to be parsed.</param>
	    /// <param name="pos">defines the position where parsing is to begin, and uponreturn, the position where parsing left off. If the positionhas not changed upon return, then parsing failed.</param>
	    /// <returns>nothing because this method is not yet implemented.</returns>
	    /// <exception cref="UnsupportedOperationException">will always be thrown by this method.</exception>
	    /// @draft ICU 3.8
	    /// @provisional This API might change or be removed in a future release.
	    public override Object ParseObject(String source, ParsePosition pos) {
	        throw new NotSupportedException();
	    }
	
	    /// <summary>
	    /// Sets the locale used by this <c>PluraFormat</c> object. Note:
	    /// Calling this method resets this <c>PluraFormat</c> object, i.e., a
	    /// pattern that was applied previously will be removed, and the NumberFormat
	    /// is set to the default number format for the locale. The resulting format
	    /// behaves the same as one constructed from <see cref="M:IBM.ICU.Text.PluralFormat.PluralFormat(null)"/>.
	    /// </summary>
	    ///
	    /// <param name="ulocale_0">the <c>ULocale</c> used to configure the formatter. If<c>ulocale</c> is <c>null</c>, the default localewill be used.</param>
	    /// @draft ICU 3.8
	    /// @provisional This API might change or be removed in a future release.
	    public void SetLocale(ULocale ulocale_0) {
	        if (ulocale_0 == null) {
	            ulocale_0 = IBM.ICU.Util.ULocale.GetDefault();
	        }
	        Init(null, ulocale_0);
	    }
	
	    /// <summary>
	    /// Sets the number format used by this formatter. You only need to call this
	    /// if you want a different number format than the default formatter for the
	    /// locale.
	    /// </summary>
	    ///
	    /// <param name="format">the number format to use.</param>
	    /// @draft ICU 3.8
	    /// @provisional This API might change or be removed in a future release.
	    public void SetNumberFormat(NumberFormat format) {
	        numberFormat = format;
	    }
	
	    /// <summary>
	    /// Checks if the applied pattern provided enough information, i.e., if the
	    /// attribute <c>parsedValues</c> stores enough information for plural
	    /// formatting. Will be called at the end of pattern parsing.
	    /// </summary>
	    ///
	    /// <exception cref="IllegalArgumentException">if there's not sufficient information provided.</exception>
	    private void CheckSufficientDefinition() {
	        // Check that at least the default rule is defined.
	        if (ILOG.J2CsMapping.Collections.Collections.Get(parsedValues,IBM.ICU.Text.PluralRules.KEYWORD_OTHER) == null) {
	            ParsingFailure("Malformed formatting expression.\n"
	                    + "Value for case \"" + IBM.ICU.Text.PluralRules.KEYWORD_OTHER
	                    + "\" was not defined.");
	        }
	    }
	
	    /// <summary>
	    /// Helper method that resets the <c>PluralFormat</c> object and throws
	    /// an <c>IllegalArgumentException</c> with a given error text.
	    /// </summary>
	    ///
	    /// <param name="errorText">the error text of the exception message.</param>
	    /// <exception cref="IllegalArgumentException">will always be thrown by this method.</exception>
	    private void ParsingFailure(String errorText) {
	        // Set PluralFormat to a valid state.
	        Init(null, IBM.ICU.Util.ULocale.GetDefault());
	        throw new ArgumentException(errorText);
	    }
	
	    /// <summary>
	    /// Helper method that is called during formatting. It replaces the character
	    /// '#' by the number used for plural selection in a message text. Only '#'
	    /// are replaced, that are not written inside curly braces. This allows the
	    /// use of nested number formats. The number will be formatted using the
	    /// attribute <c>numberformat</c>.
	    /// </summary>
	    ///
	    /// <param name="number">the number used for plural selection.</param>
	    /// <param name="message">is the text in which '#' will be replaced.</param>
	    /// <returns>the text with inserted numbers.</returns>
	    private String InsertFormattedNumber(long number, String message) {
	        if (message == null) {
	            return "";
	        }
	        String formattedNumber = numberFormat.Format(number);
	        StringBuilder result = new StringBuilder();
	        int braceStack = 0;
	        int startIndex = 0;
	        for (int i = 0; i < message.Length; ++i) {
	            switch ((int) message[i]) {
	            case '{':
	                ++braceStack;
	                break;
	            case '}':
	                --braceStack;
	                break;
	            case '#':
	                if (braceStack == 0) {
	                    result.Append(message.Substring(startIndex,(i)-(startIndex)));
	                    startIndex = i + 1;
	                    result.Append(formattedNumber);
	                }
	                break;
	            }
	        }
	        if (startIndex < message.Length) {
	            result.Append(message.Substring(startIndex,(message.Length)-(startIndex)));
	        }
	        return result.ToString();
	    }
	
	    /// <summary>
	    /// 
	    /// </summary>
	    ///
	    /// @draft ICU 3.8
	    /// @provisional This API might change or be removed in a future release.
	    public override bool Equals(Object rhs) {
	        return rhs  is  PluralFormat && Equals((PluralFormat) rhs);
	    }
	
	    /// <summary>
	    /// Returns true if this equals the provided PluralFormat.
	    /// </summary>
	    ///
	    /// <param name="rhs">the PluralFormat to compare against</param>
	    /// <returns>true if this equals rhs</returns>
	    /// @draft ICU 3.8
	    /// @provisional This API might change or be removed in a future release.
	    public bool Equals(PluralFormat rhs) {
	        return pluralRules.Equals(rhs.pluralRules)
	                && parsedValues.Equals(rhs.parsedValues)
	                && numberFormat.Equals(rhs.numberFormat);
	    }
	
	    /// <summary>
	    /// 
	    /// </summary>
	    ///
	    /// @draft ICU 3.8
	    /// @provisional This API might change or be removed in a future release.
	    public override int GetHashCode() {
	        return pluralRules.GetHashCode() ^ parsedValues.GetHashCode();
	    }
	
	    /// <summary>
	    /// For debugging purposes only
	    /// </summary>
	    ///
	    /// <returns>a text representation of the format data.</returns>
	    /// @draft ICU 3.8
	    /// @provisional This API might change or be removed in a future release.
	    public override String ToString() {
	        StringBuilder buf = new StringBuilder();
	        buf.Append("locale=" + ulocale);
	        buf.Append(", rules='" + pluralRules + "'");
	        buf.Append(", pattern='" + pattern + "'");
	        buf.Append(", parsedValues='" + parsedValues + "'");
	        buf.Append(", format='" + numberFormat + "'");
	        return buf.ToString();
	    }
	}
}