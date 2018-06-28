/* 
 * Licensed to the Apache Software Foundation (ASF) under one or more
 * contributor license agreements.  See the NOTICE file distributed with
 * this work for additional information regarding copyright ownership.
 * The ASF licenses this file to You under the Apache License, Version 2.0
 * (the "License"); you may not use this file except in compliance with
 * the License.  You may obtain a copy of the License at
 * 
 *     http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

// --------------------------------------------------------------------------------------------------
// This file was automatically generated by J2CS Translator (http://j2cstranslator.sourceforge.net/). 
// Version 1.3.6.20110104_01     
// 1/4/11 3:58 PM    
// ${CustomMessageForDisclaimer}                                                                             
// --------------------------------------------------------------------------------------------------
namespace ILOG.J2CsMapping.Text
{
	
	using IBM.ICU.Text;
	using ILOG.J2CsMapping.Text;
	using ILOG.J2CsMapping.Util;
	using System.Collections;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.IO;
	using System.Runtime.CompilerServices;
    using System;
	
	/// <summary>
	/// <c>RuleBasedCollator</c> is a concrete subclass of
	/// <c>Collator</c>. It allows customization of the <c>Collator</c>
	/// via user-specified rule sets. <c>RuleBasedCollator</c> is designed to
	/// be fully compliant to the <a
	/// href="http://www.unicode.org/unicode/reports/tr10/"> Unicode Collation
	/// Algorithm (UCA) </a> and conforms to ISO 14651. </p>
	/// <p>
	/// Create a <c>RuleBasedCollator</c> from a locale by calling the
	/// <c>getInstance(Locale)</c> factory method in the base class
	/// <c>Collator</c>.<c>Collator.getInstance(Locale)</c> creates a
	/// <c>RuleBasedCollator</c> object based on the collation rules defined by
	/// the argument locale. If a customized collation is required, use the
	/// <c>RuleBasedCollator(String)</c> constructor with the appropriate
	/// rules. The customized <c>RuleBasedCollator</c> will base its ordering
	/// on UCA, while re-adjusting the attributes and orders of the characters in the
	/// specified rule accordingly.
	/// </p>
	/// </summary>
	///
	public class RuleBasedCollator : Collator {
	
	    internal RuleBasedCollator(IBM.ICU.Text.Collator wrapper) : base(wrapper) {
	    }
	
	    /// <summary>
	    /// Constructs a new instance of <c>RuleBasedCollator</c> using the
	    /// specified <c>rules</c>.
	    /// </summary>
	    ///
	    /// <param name="rules">the collation rules.</param>
	    /// <exception cref="ParseException">when the rules contains an invalid collation rule syntax.</exception>
	    public RuleBasedCollator(String rules) {
	        if (rules == null) {
	            throw new NullReferenceException();
	        }
	        if (rules.Length == 0) {
	            // text.06=Build rules empty
	            throw new ParseException("text.06", 0); //$NON-NLS-1$
	        }
	
	        try {
	            this.icuColl = new IBM.ICU.Text.RuleBasedCollator(rules);
	        } catch (Exception e) {
	            if (e  is  ParseException) {
	                throw (ParseException) e;
	            }
	            /*
	             * -1 means it's not a ParseException. Maybe IOException thrown when
	             * an error occured while reading internal data.
	             */
	            throw new ParseException(e.Message, -1);
	        }
	    }
	
	    /// <summary>
	    /// Obtains a <c>CollationElementIterator</c> for the given
	    /// <c>CharacterIterator</c>. The source iterator's integrity will be
	    /// preserved since a new copy will be created for use.
	    /// </summary>
	    ///
	    /// <param name="source">the specified source</param>
	    /// <returns>a <c>CollationElementIterator</c> for the source.</returns>
	    public CollationElementIterator GetCollationElementIterator(
	            CharacterIterator source) {
	        if (source == null) {
	            throw new NullReferenceException();
	        }
	        return new CollationElementIterator(
	                ((IBM.ICU.Text.RuleBasedCollator) this.icuColl)
	                        .GetCollationElementIterator(source));
	    }
	
	    /// <summary>
	    /// Obtains a <c>CollationElementIterator</c> for the given String.
	    /// </summary>
	    ///
	    /// <param name="source">the specified source</param>
	    /// <returns>a <c>CollationElementIterator</c> for the given String</returns>
	    public CollationElementIterator GetCollationElementIterator(String source) {
	        if (source == null) {
	            throw new NullReferenceException();
	        }
	        return new CollationElementIterator(
	                ((IBM.ICU.Text.RuleBasedCollator) this.icuColl)
	                        .GetCollationElementIterator(source));
	    }
	
	    /// <summary>
	    /// Obtains the collation rules of the <c>RuleBasedCollator</c>.
	    /// </summary>
	    ///
	    /// <returns>the collation rules.</returns>
	    public String GetRules() {
	        return ((IBM.ICU.Text.RuleBasedCollator) this.icuColl).GetRules();
	    }
	
	    /// <summary>
	    /// Obtains the cloned object of the <c>RuleBasedCollator</c>
	    /// </summary>
	    ///
	    /// <returns>the cloned object of the <c>RuleBasedCollator</c></returns>
	    public override Object Clone() {
	        RuleBasedCollator clone = (RuleBasedCollator) base.Clone();
	        return clone;
	    }
	
	    /// <summary>
	    /// Compares the <c>source</c> text <c>String</c> to the
	    /// <c>target</c> text <c>String</c> according to the collation
	    /// rules, strength and decomposition mode for this
	    /// <c>RuleBasedCollator</c>. See the <c>Collator</c> class
	    /// description for an example of use.
	    /// <p>
	    /// General recommendation: If comparisons are to be done to the same String
	    /// multiple times, it would be more efficient to generate
	    /// <c>CollationKeys</c> for the <c>String</c> s and use
	    /// <c>CollationKey.compareTo(CollationKey)</c> for the comparisons. If
	    /// the each Strings are compared to only once, using the method
	    /// RuleBasedCollator.compare(String, String) will have a better performance.
	    /// </p>
	    /// </summary>
	    ///
	    /// <param name="source">the source text</param>
	    /// <param name="target">the target text</param>
	    /// <returns>an integer which may be a negative value, zero, or else a
	    /// positive value depending on whether <c>source</c> is less
	    /// than, equivalent to, or greater than <c>target</c>.</returns>
	    public override int Compare(String source, String target) {
	        if (source == null || target == null) {
	            // text.08=one of arguments is null
	            throw new NullReferenceException("text.08"); //$NON-NLS-1$
	        }
	        return this.icuColl.Compare(source, target);
	    }
	
	    /// <summary>
	    /// Obtains the <c>CollationKey</c> for the given source text.
	    /// </summary>
	    ///
	    /// <param name="source">the specified source text</param>
	    /// <returns>the <c>CollationKey</c> for the given source text.</returns>
	    public override CollationKey GetCollationKey(String source) {
	        IBM.ICU.Text.CollationKey icuKey = this.icuColl
	                .GetCollationKey(source);
	        if (icuKey == null) {
	            return null;
	        }
	        return new CollationKey(source, icuKey);
	    }
	
	    /// <summary>
	    /// Obtains a unique hash code for the <c>RuleBasedCollator</c>
	    /// </summary>
	    ///
	    /// <returns>the hash code for the <c>RuleBasedCollator</c></returns>
	    public override int GetHashCode() {
	        return ((IBM.ICU.Text.RuleBasedCollator) this.icuColl).GetRules()
	                .GetHashCode();
	    }
	
	    /// <summary>
	    /// Compares the equality of two <c>RuleBasedCollator</c> objects.
	    /// <c>RuleBasedCollator</c> objects are equal if they have the same
	    /// collation rules and the same attributes.
	    /// </summary>
	    ///
	    /// <param name="obj">the other object.</param>
	    /// <returns><c>true</c> if this <c>RuleBasedCollator</c> has
	    /// exactly the same collation behaviour as obj, <c>false</c>
	    /// otherwise.</returns>
	    public override bool Equals(Object obj) {
	        if (!(obj  is  Collator)) {
	            return false;
	        }
	        return base.Equals(obj);
	    }
	}
}