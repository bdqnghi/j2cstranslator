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
 * Copyright (C) 2002-2007, International Business Machines Corporation and    *
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
	
	public class Tokenizer {
	    public Tokenizer() {
	        this.buffer = new StringBuilder();
	        this.unicodeSet = null;
	        this.backedup = false;
	        this.lastIndex = -1;
	        this.lastValue = BACKEDUP_TOO_FAR;
	        this.symbolTable = new Tokenizer.TokenSymbolTable ();
	        this.whiteSpace = WHITESPACE;
	        this.syntax = SYNTAX;
	        this.non_string = NON_STRING;
	    }
	
	    protected internal String source;
	
	    protected internal StringBuilder buffer;
	
	    protected internal long number;
	
	    protected internal UnicodeSet unicodeSet;
	
	    protected internal int index;
	
	    internal bool backedup;
	
	    protected internal int lastIndex;
	
	    protected internal int nextIndex;
	
	    internal int lastValue;
	
	    internal Tokenizer.TokenSymbolTable  symbolTable;
	
	    private const char QUOTE = '\'', BSLASH = '\\';
	
	    private static readonly UnicodeSet QUOTERS = new UnicodeSet().Add(QUOTE).Add(
	            BSLASH);
	
	    private static readonly UnicodeSet WHITESPACE = new UnicodeSet("["
	            + "\\u0009-\\u000D\\u0020\\u0085\\u200E\\u200F\\u2028\\u2029" + "]");
	
	    private static readonly UnicodeSet SYNTAX = new UnicodeSet("["
	            + "\\u0021-\\u002F\\u003A-\\u0040\\u005B-\\u0060\\u007B-\\u007E"
	            + "\\u00A1-\\u00A7\\u00A9\\u00AB-\\u00AC\\u00AE"
	            + "\\u00B0-\\u00B1\\u00B6\\u00B7\\u00BB\\u00BF\\u00D7\\u00F7"
	            + "\\u2010-\\u2027\\u2030-\\u205E\\u2190-\\u2BFF"
	            + "\\u3001\\u3003\\u3008-\\u3020\\u3030"
	            + "\\uFD3E\\uFD3F\\uFE45\\uFE46" + "]").RemoveAll(QUOTERS).Remove(
	            '$');
	
	    private static readonly UnicodeSet NEWLINE = new UnicodeSet(
	            "[\\u000A\\u000D\\u0085\\u2028\\u2029]");
	
	    // private static final UnicodeSet DECIMAL = new UnicodeSet("[:Nd:]");
	    private static readonly UnicodeSet NON_STRING = new UnicodeSet().AddAll(
	            WHITESPACE).AddAll(SYNTAX);
	
	    protected internal UnicodeSet whiteSpace;
	
	    protected internal UnicodeSet syntax;
	
	    private UnicodeSet non_string;
	
	    private void FixSets() {
	        if (syntax.ContainsSome(QUOTERS) || syntax.ContainsSome(whiteSpace)) {
	            syntax = ((UnicodeSet) syntax.Clone()).RemoveAll(QUOTERS)
	                    .RemoveAll(whiteSpace);
	        }
	        if (whiteSpace.ContainsSome(QUOTERS)) {
	            whiteSpace = ((UnicodeSet) whiteSpace.Clone()).RemoveAll(QUOTERS);
	        }
	        non_string = new UnicodeSet(syntax).AddAll(whiteSpace);
	    }
	
	    public Tokenizer SetSource(String source_0) {
	        this.source = source_0;
	        this.index = 0;
	        return this; // for chaining
	    }
	
	    public Tokenizer SetIndex(int index_0) {
	        this.index = index_0;
	        return this; // for chaining
	    }
	
	    public const int DONE = -1, NUMBER = -2, STRING = -3,
	            UNICODESET = -4, UNTERMINATED_QUOTE = -5, BACKEDUP_TOO_FAR = -6;
	
	    private const int
	    // FIRST = 0,
	    // IN_NUMBER = 1,
	    // IN_SPACE = 2,
	            AFTER_QUOTE = 3, // warning: order is important for switch statement
	            IN_STRING = 4, AFTER_BSLASH = 5, IN_QUOTE = 6;
	
	    public String ToString(int type, bool backedupBefore) {
	        String s = (backedup) ? "@" : "*";
	        switch (type) {
	        case DONE:
	            return s + "Done" + s;
	        case BACKEDUP_TOO_FAR:
	            return s + "Illegal Backup" + s;
	        case UNTERMINATED_QUOTE:
	            return s + "Unterminated Quote=" + GetString() + s;
	        case STRING:
	            return s + "s=" + GetString() + s;
	        case NUMBER:
	            return s + "n=" + GetNumber() + s;
	        case UNICODESET:
	            return s + "n=" + GetUnicodeSet() + s;
	        default:
	            return s + "c=" + usf.GetName(type, true) + s;
	        }
	    }
	
	    private static readonly BagFormatter usf = new BagFormatter();
	
	    public void Backup() {
	        if (backedup)
	            throw new ArgumentException("backup too far");
	        backedup = true;
	        nextIndex = index;
	        index = lastIndex;
	    }
	
	    /*
	     * public int next2() { boolean backedupBefore = backedup; int result =
	     * next(); System.out.println(toString(result, backedupBefore)); return
	     * result; }
	     */
	
	    public int Next() {
	        if (backedup) {
	            backedup = false;
	            index = nextIndex;
	            return lastValue;
	        }
	        int cp = 0;
	        bool inComment = false;
	        // clean off any leading whitespace or comments
	        while (true) {
	            if (index >= source.Length)
	                return lastValue = DONE;
	            cp = NextChar();
	            if (inComment) {
	                if (NEWLINE.Contains(cp))
	                    inComment = false;
	            } else {
	                if (cp == '#')
	                    inComment = true;
	                else if (!whiteSpace.Contains(cp))
	                    break;
	            }
	        }
	        // record the last index in case we have to backup
	        lastIndex = index;
	
	        if (cp == '[') {
	            ILOG.J2CsMapping.Text.ParsePosition pos = new ILOG.J2CsMapping.Text.ParsePosition(index - 1);
	            unicodeSet = new UnicodeSet(source, pos, symbolTable);
	            index = pos.GetIndex();
	            return lastValue = UNICODESET;
	        }
	        // get syntax character
	        if (syntax.Contains(cp))
	            return lastValue = cp;
	
	        // get number, if there is one
	        if (IBM.ICU.Lang.UCharacter.GetType(cp) == ILOG.J2CsMapping.Util.Character.DECIMAL_DIGIT_NUMBER) {
	            number = IBM.ICU.Lang.UCharacter.GetNumericValue(cp);
	            while (index < source.Length) {
	                cp = NextChar();
                    if (IBM.ICU.Lang.UCharacter.GetType(cp) != ILOG.J2CsMapping.Util.Character.DECIMAL_DIGIT_NUMBER)
                    {
	                    index -= IBM.ICU.Text.UTF16.GetCharCount(cp); // BACKUP!
	                    break;
	                }
	                number *= 10;
	                number += IBM.ICU.Lang.UCharacter.GetNumericValue(cp);
	            }
	            return lastValue = NUMBER;
	        }
	        buffer.Length=0;
	        int status = IN_STRING;
	        main: {
	            while (true) {
	                switch (status) {
	                case AFTER_QUOTE: // check for double ''?
	                    if (cp == QUOTE) {
	                        IBM.ICU.Text.UTF16.Append(buffer, QUOTE);
	                        status = IN_QUOTE;
	                        break;
	                    }
	                    {
	                        if (cp == QUOTE)
	                            status = IN_QUOTE;
	                        else if (cp == BSLASH)
	                            status = AFTER_BSLASH;
	                        else if (non_string.Contains(cp)) {
	                            index -= IBM.ICU.Text.UTF16.GetCharCount(cp);
	                            goto gotomain;
	                        } else
	                            IBM.ICU.Text.UTF16.Append(buffer, cp);
	                        break;
	                    }
	                    break;
	                // OTHERWISE FALL THROUGH!!!
	                case IN_STRING:
	                    if (cp == QUOTE)
	                        status = IN_QUOTE;
	                    else if (cp == BSLASH)
	                        status = AFTER_BSLASH;
	                    else if (non_string.Contains(cp)) {
	                        index -= IBM.ICU.Text.UTF16.GetCharCount(cp); // BACKUP!
	                        goto gotomain;
	                    } else
	                        IBM.ICU.Text.UTF16.Append(buffer, cp);
	                    break;
	                case IN_QUOTE:
	                    if (cp == QUOTE)
	                        status = AFTER_QUOTE;
	                    else
	                        IBM.ICU.Text.UTF16.Append(buffer, cp);
	                    break;
	                case AFTER_BSLASH:
	                    switch (cp) {
	                    case 'n':
	                        cp = '\n';
	                        break;
	                    case 'r':
	                        cp = '\r';
	                        break;
	                    case 't':
	                        cp = '\t';
	                        break;
	                    }
	                    IBM.ICU.Text.UTF16.Append(buffer, cp);
	                    status = IN_STRING;
	                    break;
	                default:
	                    throw new ArgumentException("Internal Error");
	                }
	                if (index >= source.Length)
	                    break;
	                cp = NextChar();
	            }
	        }
	        gotomain:
	        ;
	        if (status > IN_STRING)
	            return lastValue = UNTERMINATED_QUOTE;
	        return lastValue = STRING;
	    }
	
	    public String GetString() {
	        return buffer.ToString();
	    }
	
	    public override String ToString() {
	        return source.Substring(0,(index)-(0)) + "$$$" + source.Substring(index);
	    }
	
	    public long GetNumber() {
	        return number;
	    }
	
	    public UnicodeSet GetUnicodeSet() {
	        return unicodeSet;
	    }
	
	    private int NextChar() {
	        int cp = IBM.ICU.Text.UTF16.CharAt(source, index);
	        index += IBM.ICU.Text.UTF16.GetCharCount(cp);
	        return cp;
	    }
	
	    public int GetIndex() {
	        return index;
	    }
	
	    public String GetSource() {
	        return source;
	    }
	
	    public UnicodeSet GetSyntax() {
	        return syntax;
	    }
	
	    public UnicodeSet GetWhiteSpace() {
	        return whiteSpace;
	    }
	
	    public void SetSyntax(UnicodeSet set) {
	        syntax = set;
	        FixSets();
	    }
	
	    public void SetWhiteSpace(UnicodeSet set) {
	        whiteSpace = set;
	        FixSets();
	    }
	
	    public ILOG.J2CsMapping.Collections.ISet GetLookedUpItems() {
	        return symbolTable.itemsLookedUp;
	    }
	
	    public void AddSymbol(String var, String value_ren, int start, int limit) {
	        // the limit is after the ';', so remove it
	        --limit;
	        char[] body = new char[limit - start];
	        value_ren.CopyTo(start,body,0,limit-start);
	        symbolTable.Add(var, body);
	    }
	
	    public class TokenSymbolTable : SymbolTable {
	        public TokenSymbolTable() {
	            this.contents = new Hashtable();
	            this.itemsLookedUp = new HashedSet();
	        }
	
	        internal IDictionary contents;
	
	        internal ILOG.J2CsMapping.Collections.ISet itemsLookedUp;
	
	        public void Add(String var, char[] body) {
	            // start from 1 to avoid the $
	            ILOG.J2CsMapping.Collections.Collections.Put(contents,var.Substring(1),body);
	        }
	
	        /*
	         * (non-Javadoc)
	         * 
	         * @see com.ibm.icu.text.SymbolTable#lookup(java.lang.String)
	         */
	        public virtual char[] Lookup(String s) {
	            ILOG.J2CsMapping.Collections.Generics.Collections.Add(itemsLookedUp,'$' + s);
	            return (char[]) ILOG.J2CsMapping.Collections.Collections.Get(contents,s);
	        }
	
	        /*
	         * (non-Javadoc)
	         * 
	         * @see com.ibm.icu.text.SymbolTable#lookupMatcher(int)
	         */
	        public virtual UnicodeMatcher LookupMatcher(int ch) {
	            // TODO Auto-generated method stub
	            return null;
	        }
	
	        /*
	         * (non-Javadoc)
	         * 
	         * @see com.ibm.icu.text.SymbolTable#parseReference(java.lang.String,
	         * java.text.ParsePosition, int)
	         */
	        public virtual String ParseReference(String text, ILOG.J2CsMapping.Text.ParsePosition pos, int limit) {
	            int cp;
	            int start = pos.GetIndex();
	            int i;
	            for (i = start; i < limit; i += IBM.ICU.Text.UTF16.GetCharCount(cp)) {
	                cp = IBM.ICU.Text.UTF16.CharAt(text, i);
	                if (!IBM.ICU.Lang.UCharacter.IsUnicodeIdentifierPart(cp)) {
	                    break;
	                }
	            }
	            pos.SetIndex(i);
	            return text.Substring(start,(i)-(start));
	        }
	
	    }
	}
	
	// #endif
}