/*
 *******************************************************************************
 * Copyright (C) 2002-2006, International Business Machines Corporation and    *
 * others. All Rights Reserved.                                                *
 *******************************************************************************
 */
// --------------------------------------------------------------------------------------------------
// This file was automatically generated by J2CS Translator (http://j2cstranslator.sourceforge.net/). 
// Version 1.3.6.20101125_01     
// 12/13/10 4:01 PM    
// ${CustomMessageForDisclaimer}                                                                             
// --------------------------------------------------------------------------------------------------
 namespace IBM.ICU.Charset {
	
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.IO;
	using System.Runtime.CompilerServices;
	using System.Text;
	
	public abstract class Tabber {
	    public Tabber() {
	        this.prefix = "";
	        this.postfix = "";
	    }
	
	    public const byte LEFT = 0, CENTER = 1, RIGHT = 2;
	
	    private static readonly String[] ALIGNMENT_NAMES = { "Left", "Center", "Right" };
	
	    /// <summary>
	    /// Repeats a string n times
	    /// </summary>
	    ///
	    /// <param name="source"></param>
	    /// <param name="times"></param>
	    // TODO - optimize repeats using doubling?
	    public static String Repeat(String source, int times) {
	        if (times <= 0)
	            return "";
	        if (times == 1)
	            return source;
	        StringBuilder result = new StringBuilder();
	        for (; times > 0; --times) {
	            result.Append(source);
	        }
	        return result.ToString();
	    }
	
	    public String Process(String source) {
	        StringBuilder result = new StringBuilder();
	        int lastPos = 0;
	        for (int count = 0; lastPos < source.Length; ++count) {
	            int pos = source.IndexOf('\t', lastPos);
	            if (pos < 0)
	                pos = source.Length;
	            Process_field(count, source, lastPos, pos, result);
	            lastPos = pos + 1;
	        }
	        return prefix + result.ToString() + postfix;
	    }
	
	    private String prefix;
	
	    private String postfix;
	
	    public abstract void Process_field(int count, String source, int start,
	            int limit, StringBuilder output);
	
	    public virtual Tabber Clear() {
	        return this;
	    }
	
	    public sealed class Anonymous_C0 : Tabber {
	        public override void Process_field(int count, String source, int start,
	                int limit, StringBuilder output) {
	            if (count > 0)
	                output.Append("\t");
	            output.Append(source.Substring(start,(limit)-(start)));
	        }
	    }
	
	    public class MonoTabber : Tabber {
	        public MonoTabber() {
	            this.minGap = 0;
	            this.stops = new ArrayList();
	            this.types = new ArrayList();
	        }
	
	        internal int minGap;
	
	        private IList stops;
	
	        private IList types;
	
	        public override Tabber Clear() {
	            ILOG.J2CsMapping.Collections.Collections.Clear(stops);
	            ILOG.J2CsMapping.Collections.Collections.Clear(types);
	            minGap = 0;
	            return this;
	        }
	
	        public override String ToString() {
	            StringBuilder buffer = new StringBuilder();
	            for (int i = 0; i < stops.Count; ++i) {
	                if (i != 0)
	                    buffer.Append("; ");
	                buffer.Append(
	                        IBM.ICU.Charset.Tabber.ALIGNMENT_NAMES[((Int32) types[i])])
	                        .Append(",").Append(stops[i]);
	            }
	            return buffer.ToString();
	        }
	
	        /// <summary>
	        /// Adds tab stop and how to align the text UP TO that stop
	        /// </summary>
	        ///
	        /// <param name="tabPos"></param>
	        /// <param name="type"></param>
	        public Tabber.MonoTabber  AddAbsolute(int tabPos, int type) {
	            ILOG.J2CsMapping.Collections.Generics.Collections.Add(stops,((int)(tabPos)));
	            ILOG.J2CsMapping.Collections.Generics.Collections.Add(types,((int)(type)));
	            return this;
	        }
	
	        /// <summary>
	        /// Adds relative tab stop and how to align the text UP TO that stop
	        /// </summary>
	        ///
	        public override Tabber Add(int fieldWidth, byte type) {
	            int last = GetStop(stops.Count - 1);
	            ILOG.J2CsMapping.Collections.Generics.Collections.Add(stops,((int)(last + fieldWidth)));
	            ILOG.J2CsMapping.Collections.Generics.Collections.Add(types,((int)(type)));
	            return this;
	        }
	
	        public int GetStop(int fieldNumber) {
	            if (fieldNumber < 0)
	                return 0;
	            if (fieldNumber >= stops.Count)
	                fieldNumber = stops.Count - 1;
	            return ((Int32) stops[fieldNumber]);
	        }
	
	        public int GetType(int fieldNumber) {
	            if (fieldNumber < 0)
	                return IBM.ICU.Charset.Tabber.LEFT;
	            if (fieldNumber >= stops.Count)
	                return IBM.ICU.Charset.Tabber.LEFT;
	            return ((Int32) types[fieldNumber]);
	        }
	
	        /*
	         * public String process(String source) { StringBuffer result = new
	         * StringBuffer(); int lastPos = 0; int count = 0; for (count = 0;
	         * lastPos < source.length() && count < stops.size(); count++) { int pos
	         * = source.indexOf('\t', lastPos); if (pos < 0) pos = source.length();
	         * String piece = source.substring(lastPos, pos); int stopPos =
	         * getStop(count); if (result.length() < stopPos) {
	         * result.append(repeat(" ", stopPos - result.length())); // TODO fix
	         * type } result.append(piece); lastPos = pos+1; } if (lastPos <
	         * source.length()) { result.append(source.substring(lastPos)); } return
	         * result.toString(); }
	         */
	
	        public override void Process_field(int count, String source, int start,
	                int limit, StringBuilder output) {
	            String piece = source.Substring(start,(limit)-(start));
	            int startPos = GetStop(count - 1);
	            int endPos = GetStop(count) - minGap;
	            int type = GetType(count);
	            switch (type) {
	            case IBM.ICU.Charset.Tabber.LEFT:
	                break;
	            case IBM.ICU.Charset.Tabber.RIGHT:
	                startPos = endPos - piece.Length;
	                break;
	            case IBM.ICU.Charset.Tabber.CENTER:
	                startPos = (startPos + endPos - piece.Length + 1) / 2;
	                break;
	            }
	
	            int gap = startPos - output.Length;
	            if (count != 0 && gap < minGap)
	                gap = minGap;
	            if (gap > 0)
	                output.Append(IBM.ICU.Charset.Tabber.Repeat(" ", gap));
	            output.Append(piece);
	        }
	
	    }
	
	    public static Tabber NULL_TABBER = new Tabber.Anonymous_C0 ();
	
	    public class HTMLTabber : Tabber {
	        public HTMLTabber() {
	            SetPostfix("</tr>");
	            SetPrefix("<tr>");
	            this.parameters = new ArrayList();
	        }
	
	        private IList parameters;
	        public void SetParameters(int count, String paras) {
	            while (count >= parameters.Count)
	                ILOG.J2CsMapping.Collections.Generics.Collections.Add(parameters,null);
	            parameters[count]=paras;
	        }
	
	        public override void Process_field(int count, String source, int start,
	                int limit, StringBuilder output) {
	            output.Append("<td");
	            String paras = null;
	            if (count < parameters.Count)
	                paras = (String) parameters[count];
	            if (paras != null) {
	                output.Append(' ');
	                output.Append(paras);
	            }
	            output.Append(">");
	            output.Append(source.Substring(start,(limit)-(start)));
	            // TODO Quote string
	            output.Append("</td>");
	        }
	    }
	
	    public String GetPostfix() {
	        return postfix;
	    }
	
	    public String GetPrefix() {
	        return prefix;
	    }
	
	    
	    /// <param name="string"></param>
	    public Tabber SetPostfix(String str0) {
	        postfix = str0;
	        return this;
	    }
	
	    
	    /// <param name="string"></param>
	    public Tabber SetPrefix(String str0) {
	        prefix = str0;
	        return this;
	    }
	
	    public virtual Tabber Add(int i, byte left2) {
	        // does nothing unless overridden
	        return this;
	    }
	
	}
}
