/*
 *******************************************************************************
 * Copyright (C) 2002-2007, International Business Machines Corporation and    *
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
	
	abstract public class Pick {
	    private static bool DEBUG = false;
	
	    // for using to get strings
	
	    public class Target {
	        private Pick pick;
	
	        public Random random;
	
	        public Quoter quoter;
	
	        public static Pick.Target  Make(Pick pick_0, Random random_1, Quoter quoter_2) {
	            Pick.Target  result = new Pick.Target ();
	            result.pick = pick_0;
	            result.random = random_1;
	            result.quoter = quoter_2;
	            return result;
	        }
	
	        public String Next() {
	            quoter.Clear();
	            pick.AddTo(this);
	            return Get();
	        }
	
	        public String Get() {
	            return quoter.ToString();
	        }
	
	        public void CopyState(Pick.Target  other) {
	            random = other.random;
	        }
	
	        public void Clear() {
	            quoter.Clear();
	        }
	
	        /*
	         * private int length() { return quoter.length(); }
	         */
	        public Pick.Target  Append(int codepoint) {
	            quoter.Append(codepoint);
	            return this;
	        }
	
	        public Pick.Target  Append(String s) {
	            quoter.Append(s);
	            return this;
	        }
	
	        // must return value between 0 (inc) and 1 (exc)
	        public double NextDouble() {
	            return random.NextDouble();
	        }
	    }
	
	    // for Building
	
	    public Pick Replace(String toReplace, Pick replacement) {
	        Pick.Replacer  visitor = new Pick.Replacer (toReplace, replacement);
	        return Visit(visitor);
	    }
	
	    public Pick Name(String name) {
	        this.name = name;
	        return this;
	    }
	
	    static public Pick.Sequence MakeSequence() {
	        return new Pick.Sequence ();
	    }
	
	    static public Pick.Alternation MakeAlternation() {
	        return new Pick.Alternation ();
	    }
	
	    /*
	     * static public Pick.Sequence and(Object item) { return new
	     * Sequence().and2(item); } static public Pick.Sequence and(Object[] items)
	     * { return new Sequence().and2(items); } static public Pick.Alternation
	     * or(int itemWeight, Object item) { return new
	     * Alternation().or2(itemWeight, item); } static public Pick.Alternation
	     * or(Object[] items) { return new Alternation().or2(1, items); } static
	     * public Pick.Alternation or(int itemWeight, Object[] items) { return new
	     * Alternation().or2(itemWeight, items); } static public Pick.Alternation
	     * or(int[] itemWeights, Object[] items) { return new
	     * Alternation().or2(itemWeights, items); }
	     * 
	     * static public Pick maybe(int percent, Object item) { return new Repeat(0,
	     * 1, new int[]{100-percent, percent}, item); //return Pick.or(1.0-percent,
	     * NOTHING).or2(percent, item); } static public Pick repeat(int minCount,
	     * int maxCount, int itemWeights, Object item) { return new Repeat(minCount,
	     * maxCount, itemWeights, item); }
	     * 
	     * static public Pick codePoint(String source) { return new CodePoint(new
	     * UnicodeSet(source)); }
	     */
	
	    static public Pick RepeatMthd(int minCount, int maxCount, int[] itemWeights,
	            Pick item) {
	        return new Pick.Repeat (minCount, maxCount, itemWeights, item);
	    }
	
	    static public Pick CodePointMthd(UnicodeSet source) {
	        return new Pick.CodePoint (source);
	    }
	
	    static public Pick String(String source) {
	        return new Pick.Literal (source);
	    }
	
	    /*
	     * static public Pick unquoted(String source) { return new Literal(source);
	     * } static public Pick string(int minLength, int maxLength, Pick item) {
	     * return new Morph(item, minLength, maxLength); }
	     */
	
	    public abstract String GetInternal(int depth, ILOG.J2CsMapping.Collections.ISet alreadySeen);
	
	    // Internals
	
	    protected internal String name;
	
	    protected abstract internal void AddTo(Pick.Target  target);
	
	    protected abstract internal bool Match(String input, Pick.Position  p);
	
	    public class Sequence : Pick.ListPick  {
	        public Pick.Sequence  And2(Pick item) {
	            AddInternal(new Pick[] { item }); // we don't care about perf
	            return this; // for chaining
	        }
	
	        public Pick.Sequence  And2(Pick[] items) {
	            AddInternal(items);
	            return this; // for chaining
	        }
	
	        protected internal override void AddTo(Pick.Target  target) {
	            for (int i = 0; i < items.Length; ++i) {
	                items[i].AddTo(target);
	            }
	        }
	
	        public override String GetInternal(int depth, ILOG.J2CsMapping.Collections.ISet alreadySeen) {
	            String result = CheckName(name, alreadySeen);
	            if (result.StartsWith("$"))
	                return result;
	            result = IBM.ICU.Charset.Pick.Indent(depth) + result + "SEQ(";
	            for (int i = 0; i < items.Length; ++i) {
	                if (i != 0)
	                    result += ", ";
	                result += items[i].GetInternal(depth + 1, alreadySeen);
	            }
	            result += ")";
	            return result;
	        }
	
	        // keep private
	        public Sequence() {
	        }
	
	        protected internal override bool Match(String input, Pick.Position  p) {
	            int originalIndex = p.index;
	            for (int i = 0; i < items.Length; ++i) {
	                if (!items[i].Match(input, p)) {
	                    p.index = originalIndex;
	                    return false;
	                }
	            }
	            return true;
	        }
	    }
	
	    internal String CheckName(String name_0, ILOG.J2CsMapping.Collections.ISet alreadySeen) {
	        if (name_0 == null)
	            return "";
	        if (ILOG.J2CsMapping.Collections.Collections.Contains(name_0,alreadySeen))
	            return name_0;
	        ILOG.J2CsMapping.Collections.Generics.Collections.Add(alreadySeen,name_0);
	        return "{" + name_0 + "=}";
	    }
	
	    public class Alternation : Pick.ListPick  {
	        private Pick.WeightedIndex  weightedIndex;
	
	        public Pick.Alternation  Or2(Pick[] newItems) {
	            return Or2(1, newItems);
	        }
	
	        public Pick.Alternation  Or2(int itemWeight, Pick item) {
	            return Or2(itemWeight, new Pick[] { item }); // we don't care about
	                                                         // perf
	        }
	
	        public Pick.Alternation  Or2(int itemWeight, Pick[] newItems) {
	            int[] itemWeights = new int[newItems.Length];
	            ILOG.J2CsMapping.Collections.Arrays.Fill(itemWeights,itemWeight);
	            return Or2(itemWeights, newItems); // we don't care about perf
	        }
	
	        public Pick.Alternation  Or2(int[] itemWeights, Pick[] newItems) {
	            if (newItems.Length != itemWeights.Length) {
	                throw new IndexOutOfRangeException("or lengths must be equal: " + newItems.Length + " != "
	                                                + itemWeights.Length.ToString());
	            }
	            // int lastLen = this.items.length;
	            AddInternal(newItems);
	            weightedIndex.Add(itemWeights);
	            return this; // for chaining
	        }
	
	        protected internal override void AddTo(Pick.Target  target) {
	            items[weightedIndex.ToIndex(target.NextDouble())].AddTo(target);
	        }
	
	        public override String GetInternal(int depth, ILOG.J2CsMapping.Collections.ISet alreadySeen) {
	            String result = CheckName(name, alreadySeen);
	            if (result.StartsWith("$"))
	                return result;
	            result = IBM.ICU.Charset.Pick.Indent(depth) + result + "OR(";
	            for (int i = 0; i < items.Length; ++i) {
	                if (i != 0)
	                    result += ", ";
	                result += items[i].GetInternal(depth + 1, alreadySeen) + "/"
	                        + weightedIndex.weights[i];
	            }
	            return result + ")";
	        }
	
	        // keep private
	        public Alternation() {
	            this.weightedIndex = new Pick.WeightedIndex (0);
	        }
	
	        // take first matching option
	        protected internal override bool Match(String input, Pick.Position  p) {
	            for (int i = 0; i < weightedIndex.weights.Length; ++i) {
	                if (p.IsFailure(this, i))
	                    continue;
	                if (items[i].Match(input, p))
	                    return true;
	                p.SetFailure(this, i);
	            }
	            return false;
	        }
	    }
	
	    private static String Indent(int depth) {
	        String result = "\r\n";
	        for (int i = 0; i < depth; ++i) {
	            result += " ";
	        }
	        return result;
	    }
	
	    private class Repeat : Pick.ItemPick  {
	        internal Pick.WeightedIndex  weightedIndex;
	
	        internal int minCount;
	
	        public Repeat(int minCount_0, int maxCount, int[] itemWeights, Pick item) : base(item) {
	            this.minCount = 0;
	            weightedIndex = new Pick.WeightedIndex (minCount_0).Add(maxCount - minCount_0
	                    + 1, itemWeights);
	        }
	
	        /*
	         * private Repeat(int minCount, int maxCount, int itemWeight, Pick item)
	         * { super(item); weightedIndex = new
	         * WeightedIndex(minCount).add(maxCount-minCount+1, itemWeight); }
	         */
	        /*
	         * private Repeat(int minCount, int maxCount, Object item) { this.item =
	         * convert(item); weightedIndex = new
	         * WeightedIndex(minCount).add(maxCount-minCount+1, 1); }
	         */
	        protected internal override void AddTo(Pick.Target  target) {
	            // int count ;
	            for (int i = weightedIndex.ToIndex(target.NextDouble()); i > 0; --i) {
	                item.AddTo(target);
	            }
	        }
	
	        public override String GetInternal(int depth, ILOG.J2CsMapping.Collections.ISet alreadySeen) {
	            String result = CheckName(name, alreadySeen);
	            if (result.StartsWith("$"))
	                return result;
	            result = IBM.ICU.Charset.Pick.Indent(depth) + result + "REPEAT(" + weightedIndex + "; "
	                    + item.GetInternal(depth + 1, alreadySeen) + ")";
	            return result;
	        }
	
	        // match longest, e.g. up to just before a failure
	        protected internal override bool Match(String input, Pick.Position  p) {
	            // int bestMatch = p.index;
	            int count = 0;
	            for (int i = 0; i < weightedIndex.weights.Length; ++i) {
	                if (p.IsFailure(this, i))
	                    break;
	                if (!item.Match(input, p)) {
	                    p.SetFailure(this, i);
	                    break;
	                }
	                // bestMatch = p.index;
	                count++;
	            }
	            if (count >= minCount) {
	                return true;
	            }
	            // TODO fix failure
	            return false;
	        }
	    }
	
	    public class CodePoint : Pick.FinalPick  {
	        private UnicodeSet source;
	
	        public CodePoint(UnicodeSet source_0) {
	            this.source = source_0;
	        }
	
	        protected internal override void AddTo(Pick.Target  target) {
	            target.Append(source.CharAt(IBM.ICU.Charset.Pick.PickMthd(target.random, 0,
	                    source.Size() - 1)));
	        }
	
	        protected internal override bool Match(String s, Pick.Position  p) {
	            int cp = IBM.ICU.Text.UTF16.CharAt(s, p.index);
	            if (source.Contains(cp)) {
	                p.index += IBM.ICU.Text.UTF16.GetCharCount(cp);
	                return true;
	            }
	            p.SetMax("codePoint");
	            return false;
	        }
	
	        public override String GetInternal(int depth, ILOG.J2CsMapping.Collections.ISet alreadySeen) {
	            String result = CheckName(name, alreadySeen);
	            if (result.StartsWith("$"))
	                return result;
	            return source.ToString();
	        }
	    }
	
	    internal class Morph : Pick.ItemPick  {
	        internal Morph(Pick item) : base(item) {
	            this.lastValue = null;
	            this.addBuffer = IBM.ICU.Charset.Pick.Target.Make(this, null,
	                    new Quoter.RuleQuoter());
	            this.mergeBuffer = new StringBuilder();
	        }
	
	        private String lastValue;
	
	        private Pick.Target  addBuffer;
	
	        private StringBuilder mergeBuffer;
	
	        private const int COPY_NEW = 0, COPY_BOTH = 1, COPY_LAST = 3,
	                SKIP = 4, LEAST_SKIP = 4;
	
	        // give weights to the above. make sure we delete about the same as we
	        // insert
	        private static readonly Pick.WeightedIndex  choice = new Pick.WeightedIndex (0)
	                .Add(new int[] { 10, 10, 100, 10 });
	
	        protected internal override void AddTo(Pick.Target  target) {
	            // get contents into separate buffer
	            addBuffer.CopyState(target);
	            addBuffer.Clear();
	            item.AddTo(addBuffer);
	            String newValue = addBuffer.Get();
	            if (IBM.ICU.Charset.Pick.DEBUG)
	                System.Console.Out.WriteLine("Old: " + lastValue + ", New:" + newValue);
	
	            // if not first one, merge with old
	            if (lastValue != null) {
	                mergeBuffer.Length=0;
	                int lastIndex = 0;
	                int newIndex = 0;
	                // the new length is a random value between old and new.
	                int newLenLimit = (int) IBM.ICU.Charset.Pick.PickMthd(target.random, lastValue.Length,
	                        newValue.Length);
	
	                while (mergeBuffer.Length < newLenLimit
	                        && newIndex < newValue.Length
	                        && lastIndex < lastValue.Length) {
	                    int c = choice.ToIndex(target.NextDouble());
	                    if (c == COPY_NEW || c == COPY_BOTH || c == SKIP) {
	                        newIndex = IBM.ICU.Charset.Pick.GetChar(newValue, newIndex, mergeBuffer,
	                                c < LEAST_SKIP);
	                        if (mergeBuffer.Length >= newLenLimit)
	                            break;
	                    }
	                    if (c == COPY_LAST || c == COPY_BOTH || c == SKIP) {
	                        lastIndex = IBM.ICU.Charset.Pick.GetChar(lastValue, lastIndex, mergeBuffer,
	                                c < LEAST_SKIP);
	                    }
	                }
	                newValue = mergeBuffer.ToString();
	            }
	            lastValue = newValue;
	            target.Append(newValue);
	            if (IBM.ICU.Charset.Pick.DEBUG)
	                System.Console.Out.WriteLine("Result: " + newValue);
	        }
	
	        public override String GetInternal(int depth, ILOG.J2CsMapping.Collections.ISet alreadySeen) {
	            String result = CheckName(name, alreadySeen);
	            if (result.StartsWith("$"))
	                return result;
	            return IBM.ICU.Charset.Pick.Indent(depth) + result + "MORPH("
	                    + item.GetInternal(depth + 1, alreadySeen) + ")";
	        }
	
	        /*
	         * (non-Javadoc)
	         * 
	         * @see Pick#match(java.lang.String, Pick.Position)
	         */
	        protected internal override bool Match(String input, Pick.Position  p) {
	            // TODO Auto-generated method stub
	            return false;
	        }
	    }
	
	    /*
	     * Add character if we can
	     */
	    static internal int GetChar(String newValue, int newIndex, StringBuilder mergeBuffer_0,
	            bool copy) {
	        if (newIndex >= newValue.Length)
	            return newIndex;
	        int cp = IBM.ICU.Text.UTF16.CharAt(newValue, newIndex);
	        if (copy)
	            IBM.ICU.Text.UTF16.Append(mergeBuffer_0, cp);
	        return newIndex + IBM.ICU.Text.UTF16.GetCharCount(cp);
	    }
	
	    /*
	     * // quoted add appendQuoted(target, addBuffer.toString(), quoteBuffer); //
	     * fix buffers StringBuffer swapTemp = addBuffer; addBuffer = source; source
	     * = swapTemp; } }
	     */
	
	    internal class Quote : Pick.ItemPick  {
	        internal Quote(Pick item) : base(item) {
	        }
	
	        protected internal override void AddTo(Pick.Target  target) {
	            target.quoter.SetQuoting(true);
	            item.AddTo(target);
	            target.quoter.SetQuoting(false);
	        }
	
	        protected internal override bool Match(String s, Pick.Position  p) {
	            return false;
	        }
	
	        public override String GetInternal(int depth, ILOG.J2CsMapping.Collections.ISet alreadySeen) {
	            String result = CheckName(name, alreadySeen);
	            if (result.StartsWith("$"))
	                return result;
	            return IBM.ICU.Charset.Pick.Indent(depth) + result + "QUOTE("
	                    + item.GetInternal(depth + 1, alreadySeen) + ")";
	        }
	    }
	
	    private class Literal : Pick.FinalPick  {
	        public override String ToString() {
	            return name;
	        }
	
	        public Literal(String source_0) {
	            this.name = source_0;
	        }
	
	        protected internal override void AddTo(Pick.Target  target) {
	            target.Append(name);
	        }
	
	        protected internal override bool Match(String input, Pick.Position  p) {
	            int len = name.Length;
	            if (ILOG.J2CsMapping.Util.StringUtil.RegionMatches(input, p.index, name, 0, len)) {
	                p.index += len;
	                return true;
	            }
	            p.SetMax("literal");
	            return false;
	        }
	
	        public override String GetInternal(int depth, ILOG.J2CsMapping.Collections.ISet alreadySeen) {
	            return "'" + name + "'";
	        }
	    }
	
	    public class Position {
	        public Position() {
	            this.failures = new ArrayList();
	        }
	
	        public ArrayList failures;
	
	        public int index;
	
	        public int maxInt;
	
	        public String maxType;
	
	        public void SetMax(String type) {
	            if (index >= maxInt) {
	                maxType = type;
	            }
	        }
	
	        public override String ToString() {
	            return "index; " + index + ", maxInt:" + maxInt + ", maxType: "
	                    + maxType;
	        }
	
	        /*
	         * private static final Object BAD = new Object(); private static final
	         * Object GOOD = new Object();
	         */
	
	        public bool IsFailure(Pick pick_0, int item) {
	            ArrayList val = (ArrayList) failures[index];
	            if (val == null)
	                return false;
	            ILOG.J2CsMapping.Collections.ISet set = (ISet) val[item];
	            if (set == null)
	                return false;
	            return !ILOG.J2CsMapping.Collections.Collections.Contains(pick_0,set);
	        }
	
	        public void SetFailure(Pick pick_0, int item) {
	            ArrayList val = (ArrayList) failures[index];
	            if (val == null) {
	                val = new ArrayList();
	                failures[index]=val;
	            }
	            ILOG.J2CsMapping.Collections.ISet set = (ISet) val[item];
	            if (set == null) {
	                set = new HashedSet();
	                val[item]=set;
	            }
	            ILOG.J2CsMapping.Collections.Generics.Collections.Add(set,pick_0);
	        }
	    }
	
	    /*
	     * public static final Pick NOTHING = new Nothing();
	     * 
	     * 
	     * private static class Nothing : FinalPick { protected void
	     * addTo(Target target) {} protected boolean match(String input, Position p)
	     * { return true; } public String getInternal(int depth, Set alreadySeen) {
	     * return indent(depth) + "\u00F8"; } }
	     */
	
	    // intermediates
	
	    abstract public class Visitor {
	        public Visitor() {
	            this.already = new HashedSet();
	        }
	
	        internal ILOG.J2CsMapping.Collections.ISet already;
	
	        // Note: each visitor should return the Pick that will replace a (or a
	        // itself)
	        abstract internal Pick Handle(Pick a);
	
	        internal bool AlreadyEntered(Pick item) {
	            bool result = ILOG.J2CsMapping.Collections.Collections.Contains(item,already);
	            ILOG.J2CsMapping.Collections.Generics.Collections.Add(already,item);
	            return result;
	        }
	
	        internal void Reset() {
	            ILOG.J2CsMapping.Collections.Collections.Clear(already);
	        }
	    }
	
	    protected abstract internal Pick Visit(Pick.Visitor  visitor);
	
	    internal class Replacer : Pick.Visitor  {
	        internal String toReplace;
	
	        internal Pick replacement;
	
	        internal Replacer(String toReplace_0, Pick replacement_1) {
	            this.toReplace = toReplace_0;
	            this.replacement = replacement_1;
	        }

             internal override Pick Handle(Pick a)
            {
	            if (toReplace.Equals(a.name)) {
	                a = replacement;
	            }
	            return a;
	        }
	    }
	
	    abstract public class FinalPick : Pick {
            protected internal override Pick Visit(Pick.Visitor visitor)
            {
	            return visitor.Handle(this);
	        }
	    }
	
	    public abstract class ItemPick : Pick {
	        protected internal Pick item;
	
	        internal ItemPick(Pick item_0) {
	            this.item = item_0;
	        }

            protected internal override Pick Visit(Pick.Visitor visitor)
            {
	            Pick result = visitor.Handle(this);
	            if (visitor.AlreadyEntered(this))
	                return result;
	            if (item != null)
	                item = item.Visit(visitor);
	            return result;
	        }
	    }
	
	    public abstract class ListPick : Pick {
	        public ListPick() {
	            this.items = new Pick[0];
	        }
	
	        protected internal Pick[] items;
	
	        internal Pick Simplify() {
	            if (items.Length > 1)
	                return this;
	            if (items.Length == 1)
	                return items[0];
	            return null;
	        }
	
	        internal int Size() {
	            return items.Length;
	        }
	
	        internal Pick GetLast() {
	            return items[items.Length - 1];
	        }
	
	        internal void SetLast(Pick newOne) {
	            items[items.Length - 1] = newOne;
	        }
	
	        protected internal void AddInternal(Pick[] objs) {
	            int lastLen = items.Length;
	            items = IBM.ICU.Charset.Pick.Realloc(items, items.Length + objs.Length);
	            for (int i = 0; i < objs.Length; ++i) {
	                items[lastLen + i] = objs[i];
	            }
	        }
	
	        protected internal override Pick Visit(Pick.Visitor  visitor) {
	            Pick result = visitor.Handle(this);
	            if (visitor.AlreadyEntered(this))
	                return result;
	            for (int i = 0; i < items.Length; ++i) {
	                items[i] = items[i].Visit(visitor);
	            }
	            return result;
	        }
	    }
	
	    /// <summary>
	    /// Simple class to distribute a number between 0 (inclusive) and 1
	    /// (exclusive) among a number of indices, where each index is weighted. Item
	    /// weights may be zero, but cannot be negative.
	    /// </summary>
	    ///
	    // As in other case, we use an array for runtime speed; don't care about
	    // buildspeed.
	    public class WeightedIndex {
	        public int[] weights;
	
	        private int minCount;
	
	        private double total;
	
	        public WeightedIndex(int minCount_0) {
	            this.weights = new int[0];
	            this.minCount = 0;
	            this.minCount = minCount_0;
	        }
	
	        public Pick.WeightedIndex  Add(int count, int itemWeights) {
	            if (count > 0) {
	                int[] newWeights = new int[count];
	                if (itemWeights < 1)
	                    itemWeights = 1;
	                ILOG.J2CsMapping.Collections.Arrays.Fill(newWeights,0,count,itemWeights);
	                Add(1, newWeights);
	            }
	            return this; // for chaining
	        }
	
	        public Pick.WeightedIndex  Add(int[] newWeights) {
	            return Add(newWeights.Length, newWeights);
	        }
	
	        public Pick.WeightedIndex  Add(int maxCount, int[] newWeights) {
	            if (newWeights == null)
	                newWeights = new int[] { 1 };
	            int oldLen = weights.Length;
	            if (maxCount < newWeights.Length)
	                maxCount = newWeights.Length;
	            weights = (int[]) IBM.ICU.Charset.Pick.Realloc(weights, weights.Length + maxCount);
	            System.Array.Copy((Array)(newWeights),0,(Array)(weights),oldLen,newWeights.Length);
	            int lastWeight = weights[oldLen + newWeights.Length - 1];
	            for (int i = oldLen + newWeights.Length; i < maxCount; ++i) {
	                weights[i] = lastWeight;
	            }
	            total = 0;
	            for (int i_0 = 0; i_0 < weights.Length; ++i_0) {
	                if (weights[i_0] < 0) {
	                    throw new Exception("only positive weights: " + i_0);
	                }
	                total += weights[i_0];
	            }
	            return this; // for chaining
	        }
	
	        // TODO, make this more efficient
	        public int ToIndex(double zeroToOne) {
	            double weight = zeroToOne * total;
	            int i;
	            for (i = 0; i < weights.Length; ++i) {
	                weight -= weights[i];
	                if (weight <= 0)
	                    break;
	            }
	            return i + minCount;
	        }
	
	        public override String ToString() {
	            String result = "";
	            for (int i = 0; i < minCount; ++i) {
	                if (result.Length != 0)
	                    result += ",";
	                result += "0";
	            }
	            for (int i_0 = 0; i_0 < weights.Length; ++i_0) {
	                if (result.Length != 0)
	                    result += ",";
	                result += weights[i_0];
	            }
	            return result;
	        }
	    }
	
	    /*
	     * private static Pick convert(Object obj) { if (obj instanceof Pick) return
	     * (Pick)obj; return new Literal(obj.toString(), false); }
	     */
	    // Useful statics
	
	    static public int PickMthd(Random random_0, int start, int end) {
	        return start + (int) (random_0.NextDouble() * (end + 1 - start));
	    }

        static public double PickMthd(Random random_0, double start, double end)
        {
	        return start + (random_0.NextDouble() * (end + 1 - start));
	    }

        static public bool PickMthd(Random random_0, double percent)
        {
	        return random_0.NextDouble() <= percent;
	    }

        static public int PickMthd(Random random_0, UnicodeSet s)
        {
	        return s.CharAt(PickMthd(random_0, 0, s.Size() - 1));
	    }

        static public String PickMthd(Random random_0, String[] source_1)
        {
	        return source_1[PickMthd(random_0, 0, source_1.Length - 1)];
	    }
	
	    // these utilities really ought to be in Java
	
	    public static double[] Realloc(double[] source_0, int newSize) {
	        double[] temp = new double[newSize];
	        if (newSize > source_0.Length)
	            newSize = source_0.Length;
	        if (newSize != 0)
	            System.Array.Copy((Array)(source_0),0,(Array)(temp),0,newSize);
	        return temp;
	    }
	
	    public static int[] Realloc(int[] source_0, int newSize) {
	        int[] temp = new int[newSize];
	        if (newSize > source_0.Length)
	            newSize = source_0.Length;
	        if (newSize != 0)
	            System.Array.Copy((Array)(source_0),0,(Array)(temp),0,newSize);
	        return temp;
	    }
	
	    public static Pick[] Realloc(Pick[] source_0, int newSize) {
	        Pick[] temp = new Pick[newSize];
	        if (newSize > source_0.Length)
	            newSize = source_0.Length;
	        if (newSize != 0)
	            System.Array.Copy((Array)(source_0),0,(Array)(temp),0,newSize);
	        return temp;
	    }
	
	    // test utilities
	    /*
	     * private static void append(StringBuffer target, String toAdd,
	     * StringBuffer quoteBuffer) { Utility.appendToRule(target, (int)-1, true,
	     * false, quoteBuffer); // close previous quote if (DEBUG)
	     * System.out.println("\"" + toAdd + "\""); target.append(toAdd); }
	     * 
	     * private static void appendQuoted(StringBuffer target, String toAdd,
	     * StringBuffer quoteBuffer) { if (DEBUG) System.out.println("\"" + toAdd +
	     * "\""); Utility.appendToRule(target, toAdd, false, false, quoteBuffer); }
	     */
	
	    /*
	     * public static abstract class MatchHandler { public abstract void
	     * handleString(String source, int start, int limit); public abstract void
	     * handleSequence(String source, int start, int limit); public abstract void
	     * handleAlternation(String source, int start, int limit);
	     * 
	     * }
	     */
	    /*
	     * // redistributes random value // values are still between 0 and 1, but
	     * with a different distribution public interface Spread { public double
	     * spread(double value); }
	     * 
	     * // give the weight for the high end. // values are linearly scaled
	     * according to the weight. static public class SimpleSpread : 
	     * Spread { static final Spread FLAT = new SimpleSpread(1.0); boolean flat =
	     * false; double aa, bb, cc; public SimpleSpread(double maxWeight) { if
	     * (maxWeight > 0.999 && maxWeight < 1.001) { flat = true; } else { double q
	     * = (maxWeight - 1.0); aa = -1/q; bb = 1/(q*q); cc = (2.0+q)/q; } } public
	     * double spread(double value) { if (flat) return value; value = aa +
	     * Math.sqrt(bb + cc*value); if (value < 0.0) return 0.0; // catch math gorp
	     * if (value >= 1.0) return 1.0; return value; } } static public int
	     * pick(Spread spread, Random random, int start, int end) { return start +
	     * (int)(spread.spread(random.nextDouble()) * (end + 1 - start)); }
	     */
	
	}}
