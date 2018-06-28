/*
 *******************************************************************************
 * Copyright (C) 2004-2006, International Business Machines Corporation and    *
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
	
	/// <exclude/>
	/// <summary>
	/// Post processor for RBNF output.
	/// </summary>
	///
	interface RBNFPostProcessor {
	    /// <summary>
	    /// Initialization routine for this instance, called once immediately after
	    /// first construction and never again.
	    /// </summary>
	    ///
	    /// <param name="formatter">the formatter that will be using this post-processor</param>
	    /// <param name="rules">the special rules for this post-procesor</param>
	    void Init(RuleBasedNumberFormat formatter, String rules);
	
	    /// <summary>
	    /// Work routine. Post process the output, which was generated by the ruleset
	    /// with the given name.
	    /// </summary>
	    ///
	    /// <param name="output">the output of the main RBNF processing</param>
	    /// <param name="ruleSet">the rule set originally invoked to generate the output</param>
	    void Process(StringBuilder output, NFRuleSet ruleSet);
	}
}
