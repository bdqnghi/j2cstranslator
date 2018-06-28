//##header J2SE15
// --------------------------------------------------------------------------------------------------
// This file was automatically generated by J2CS Translator (http://j2cstranslator.sourceforge.net/). 
// Version 1.3.6.20101125_01     
// 12/13/10 4:01 PM    
// ${CustomMessageForDisclaimer}                                                                             
// --------------------------------------------------------------------------------------------------
 //#if defined(FOUNDATION10) || defined(J2SE13)
//#else
/*
 *******************************************************************************
 * Copyright (C) 2002-2006, International Business Machines Corporation and    *
 * others. All Rights Reserved.                                                *
 *******************************************************************************
 */
namespace IBM.ICU.Charset {
	
	using IBM.ICU.Text;
	using ILOG.J2CsMapping.Collections;
	using ILOG.J2CsMapping.Collections.Generics;
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.IO;
	using System.Runtime.CompilerServices;
	using System.Text;
	
	public class BNF {
	    private IDictionary map;
	
	    private ILOG.J2CsMapping.Collections.ISet variables;
	
	    private Pick pick;
	
	    private Pick.Target target;
	
	    private Tokenizer t;
	
	    private Quoter quoter;
	
	    private Random random;
	
	    public String Next() {
	        return target.Next();
	    }
	
	    public String GetInternal() {
	        return pick.GetInternal(0, new HashedSet());
	    }
	
	    /*
	     * + "weight = integer '%';" +
	     * "range = '{' integer (',' integer?)? '}' weight*;" + "quote = '@';" +
	     * "star = '*' weight*;" + "plus = '+' weight*;" + "maybe = '?' weight?;" +
	     * "quantifier = range | star | maybe | plus;" +
	     * "core = string | unicodeSet | '(' alternation ')';" +
	     * "sequence = (core quantifier*)+;" +
	     * "alternation = sequence (weight? ('|' sequence weight?)+)?;" +
	     * "rule = string '=' alternation;";
	     * 
	     * 
	     * Match 0 or more times + Match 1 or more times ? Match 1 or 0 times {n}
	     * Match exactly n times {n,} Match at least n times {n,m} Match at least n
	     * but not more than m times
	     */
	
	    public BNF(Random random_0, Quoter quoter_1) {
	        this.map = new Hashtable();
	        this.variables = new HashedSet();
	        this.pick = null;
	        this.target = null;
	        this.maxRepeat = 99;
	        this.random = random_0;
	        this.quoter = quoter_1;
	        t = new Tokenizer();
	    }
	
	    public BNF AddRules(String rules) {
	        t.SetSource(rules);
	        while (AddRule()) {
	        }
	        return this; // for chaining
	    }
	
	    public BNF Complete() {
	        // check that the rules match the variables, except for $root in rules
	        ILOG.J2CsMapping.Collections.ISet ruleSet = new ILOG.J2CsMapping.Collections.ListSet(map.Keys);
	        // add also
	        ILOG.J2CsMapping.Collections.Generics.Collections.Add(variables,"$root");
	        ILOG.J2CsMapping.Collections.Generics.Collections.AddAll(t.GetLookedUpItems(),variables);
	        if (!ruleSet.Equals(variables)) {
	            String msg = ShowDiff(variables, ruleSet);
	            if (msg.Length != 0)
	                msg = "Error: Missing definitions for: " + msg;
	            String temp = ShowDiff(ruleSet, variables);
	            if (temp.Length != 0)
	                temp = "Warning: Defined but not used: " + temp;
	            if (msg.Length == 0)
	                msg = temp;
	            else if (temp.Length != 0) {
	                msg = msg + "; " + temp;
	            }
	            Error(msg);
	        }
	
	        if (!ruleSet.Equals(variables)) {
	            String msg_0 = ShowDiff(variables, ruleSet);
	            if (msg_0.Length != 0)
	                msg_0 = "Missing definitions for: " + msg_0;
	            String temp_1 = ShowDiff(ruleSet, variables);
	            if (temp_1.Length != 0)
	                temp_1 = "Defined but not used: " + temp_1;
	            if (msg_0.Length == 0)
	                msg_0 = temp_1;
	            else if (temp_1.Length != 0) {
	                msg_0 = msg_0 + "; " + temp_1;
	            }
	            Error(msg_0);
	        }
	
	        // replace variables by definitions
	        IIterator it = new ILOG.J2CsMapping.Collections.IteratorAdapter(ruleSet.GetEnumerator());
	        while (it.HasNext()) {
	            String key = (String) it.Next();
	            Pick expression = (Pick) ILOG.J2CsMapping.Collections.Collections.Get(map,key);
	            IIterator it2 = new ILOG.J2CsMapping.Collections.IteratorAdapter(ruleSet.GetEnumerator());
	            if (false && key.Equals("$crlf")) {
	                System.Console.Out.WriteLine("debug");
	            }
	            while (it2.HasNext()) {
	                Object key2 = it2.Next();
	                if (key.Equals(key2))
	                    continue;
	                Pick expression2 = (Pick) ILOG.J2CsMapping.Collections.Collections.Get(map,key2);
	                expression2.Replace(key, expression);
	            }
	        }
	        pick = (Pick) ILOG.J2CsMapping.Collections.Collections.Get(map,"$root");
	        target = IBM.ICU.Charset.Pick.Target.Make(pick, random, quoter);
	        // TODO remove temp collections
	        return this;
	    }
	
	    internal String ShowDiff(ILOG.J2CsMapping.Collections.ISet a, ILOG.J2CsMapping.Collections.ISet b) {
	        ILOG.J2CsMapping.Collections.ISet temp = new HashedSet();
	        ILOG.J2CsMapping.Collections.Generics.Collections.AddAll(a,temp);
	        temp.RemoveAll(b);
	        if (temp.Count == 0)
	            return "";
	        StringBuilder buffer = new StringBuilder();
	        IIterator it = new ILOG.J2CsMapping.Collections.IteratorAdapter(temp.GetEnumerator());
	        while (it.HasNext()) {
	            if (buffer.Length != 0)
	                buffer.Append(", ");
	            buffer.Append(it.Next().ToString());
	        }
	        return buffer.ToString();
	    }
	
	    internal void Error(String msg) {
	        throw new ArgumentException(msg + "\r\n" + t.ToString());
	    }
	
	    private bool AddRule() {
	        int type = t.Next();
	        if (type == IBM.ICU.Charset.Tokenizer.DONE)
	            return false;
	        if (type != IBM.ICU.Charset.Tokenizer.STRING)
	            Error("missing weight");
	        String s = t.GetString();
	        if (s.Length == 0 || s[0] != '$')
	            Error("missing $ in variable");
	        if (t.Next() != '=')
	            Error("missing =");
	        int startBody = t.index;
	        Pick rule = GetAlternation();
	        if (rule == null)
	            Error("missing expression");
	        t.AddSymbol(s, t.GetSource(), startBody, t.index);
	        if (t.Next() != ';')
	            Error("missing ;");
	        return AddPick(s, rule);
	    }
	
	    protected internal bool AddPick(String s, Pick rule) {
	        Object temp = ILOG.J2CsMapping.Collections.Collections.Get(map,s);
	        if (temp != null)
	            Error("duplicate variable");
	        if (rule.name == null)
	            rule.Name(s);
	        ILOG.J2CsMapping.Collections.Collections.Put(map,s,rule);
	        return true;
	    }
	
	    public BNF AddSet(String variable, UnicodeSet set) {
	        if (set != null) {
	            String body = set.ToString();
	            t.AddSymbol(variable, body, 0, body.Length);
	            AddPick(variable, IBM.ICU.Charset.Pick.CodePointMthd(set));
	        }
	        return this;
	    }
	
	    internal int maxRepeat;
	
	    internal Pick Qualify(Pick item) {
	        int[] weights;
	        int type = t.Next();
	        switch (type) {
	        case '@':
	            return new Pick.Quote(item);
	        case '~':
	            return new Pick.Morph(item);
	        case '?':
	            int weight = GetWeight();
	            if (weight == NO_WEIGHT)
	                weight = 50;
	            weights = new int[] { 100 - weight, weight };
	            return IBM.ICU.Charset.Pick.RepeatMthd(0, 1, weights, item);
	        case '*':
	            weights = GetWeights();
	            return IBM.ICU.Charset.Pick.RepeatMthd(1, maxRepeat, weights, item);
	        case '+':
	            weights = GetWeights();
	            return IBM.ICU.Charset.Pick.RepeatMthd(1, maxRepeat, weights, item);
	        case '{':
	            if (t.Next() != IBM.ICU.Charset.Tokenizer.NUMBER)
	                Error("missing number");
	            int start = (int) t.GetNumber();
	            int end = start;
	            type = t.Next();
	            if (type == ',') {
	                end = maxRepeat;
	                type = t.Next();
	                if (type == IBM.ICU.Charset.Tokenizer.NUMBER) {
	                    end = (int) t.GetNumber();
	                    type = t.Next();
	                }
	            }
	            if (type != '}')
	                Error("missing }");
	            weights = GetWeights();
	            return IBM.ICU.Charset.Pick.RepeatMthd(start, end, weights, item);
	        }
	        t.Backup();
	        return item;
	    }
	
	    internal Pick GetCore() {
	        int token = t.Next();
	        if (token == IBM.ICU.Charset.Tokenizer.STRING) {
	            String s = t.GetString();
	            if (s[0] == '$')
	                ILOG.J2CsMapping.Collections.Generics.Collections.Add(variables,s);
	            return IBM.ICU.Charset.Pick.String(s);
	        }
	        if (token == IBM.ICU.Charset.Tokenizer.UNICODESET) {
	            return IBM.ICU.Charset.Pick.CodePointMthd(t.GetUnicodeSet());
	        }
	        if (token != '(') {
	            t.Backup();
	            return null;
	        }
	        Pick temp = GetAlternation();
	        token = t.Next();
	        if (token != ')')
	            Error("missing )");
	        return temp;
	    }
	
	    internal Pick GetSequence() {
	        Pick.Sequence result = null;
	        Pick last = null;
	        while (true) {
	            Pick item = GetCore();
	            if (item == null) {
	                if (result != null)
	                    return result;
	                if (last != null)
	                    return last;
	                Error("missing item");
	            }
	            // qualify it as many times as possible
	            Pick oldItem;
	            do {
	                oldItem = item;
	                item = Qualify(item);
	            } while (item != oldItem);
	            // add it in
	            if (last == null) {
	                last = item;
	            } else {
	                if (result == null)
	                    result = IBM.ICU.Charset.Pick.MakeSequence().And2(last);
	                result = result.And2(item);
	            }
	        }
	    }
	
	    // for simplicity, we just use recursive descent
	    internal Pick GetAlternation() {
	        Pick.Alternation result = null;
	        Pick last = null;
	        int lastWeight = NO_WEIGHT;
	        while (true) {
	            Pick temp = GetSequence();
	            if (temp == null)
	                Error("empty alternation");
	            int weight = GetWeight();
	            if (weight == NO_WEIGHT)
	                weight = 1;
	            if (last == null) {
	                last = temp;
	                lastWeight = weight;
	            } else {
	                if (result == null)
	                    result = IBM.ICU.Charset.Pick.MakeAlternation().Or2(lastWeight, last);
	                result = result.Or2(weight, temp);
	            }
	            int token = t.Next();
	            if (token != '|') {
	                t.Backup();
	                if (result != null)
	                    return result;
	                if (last != null)
	                    return last;
	            }
	        }
	    }
	
	    private const int NO_WEIGHT = Int32.MinValue;
	
	    internal int GetWeight() {
	        int weight;
	        int token = t.Next();
	        if (token != IBM.ICU.Charset.Tokenizer.NUMBER) {
	            t.Backup();
	            return NO_WEIGHT;
	        }
	        weight = (int) t.GetNumber();
	        token = t.Next();
	        if (token != '%')
	            Error("missing %");
	        return weight;
	    }
	
	    internal int[] GetWeights() {
	        ArrayList list = new ArrayList();
	        while (true) {
	            int weight = GetWeight();
	            if (weight == NO_WEIGHT)
	                break;
	            ILOG.J2CsMapping.Collections.Generics.Collections.Add(list,((int)(weight)));
	        }
	        if (list.Count == 0)
	            return null;
	        int[] result = new int[list.Count];
	        for (int i = 0; i < list.Count; ++i) {
	            result[i] = ((Int32) list[i]);
	        }
	        return result;
	    }
	
	    public int GetMaxRepeat() {
	        return maxRepeat;
	    }
	
	    public BNF SetMaxRepeat(int maxRepeat_0) {
	        this.maxRepeat = maxRepeat_0;
	        return this;
	    }
	}
	// #endif
}