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
	
	using IBM.ICU.Util;
	using ILOG.J2CsMapping.Text;
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.IO;
	using System.Runtime.CompilerServices;
     using ILOG.J2CsMapping.Util;
	
	/// <summary>
	/// Inserts the specified characters at word breaks. To restrict it to particular
	/// characters, use a filter. TODO: this is an internal class, and only
	/// temporary. Remove it once we have \b notation in Transliterator.
	/// </summary>
	///
	internal sealed class BreakTransliterator : Transliterator {
	    private BreakIterator bi;
	
	    private String insertion;
	
	    private int[] boundaries;
	
	    private int boundaryCount;
	
	    public BreakTransliterator(String ID, UnicodeFilter filter,
	            BreakIterator bi_0, String insertion_1) : base(ID, filter) {
	        this.boundaries = new int[50];
	        this.boundaryCount = 0;
	        this.bi = bi_0;
	        this.insertion = insertion_1;
	    }
	
	    public BreakTransliterator(String ID, UnicodeFilter filter) : this(ID, filter, null, " ") {
	    }
	
	    public String GetInsertion() {
	        return insertion;
	    }
	
	    public void SetInsertion(String insertion_0) {
	        this.insertion = insertion_0;
	    }
	
	    public BreakIterator GetBreakIterator() {
	        // Defer initialization of BreakIterator because it is slow,
	        // typically over 2000 ms.
	        if (bi == null)
	            bi = IBM.ICU.Text.BreakIterator.GetWordInstance(new ULocale("th_TH"));
	        return bi;
	    }
	
	    public void SetBreakIterator(BreakIterator bi_0) {
	        this.bi = bi_0;
	    }
	
	    internal readonly int LETTER_OR_MARK_MASK = (1 << Character.UPPERCASE_LETTER)
                | (1 << Character.LOWERCASE_LETTER)
                | (1 << Character.TITLECASE_LETTER)
                | (1 << Character.MODIFIER_LETTER) | (1 << Character.OTHER_LETTER)
                | (1 << Character.COMBINING_SPACING_MARK)
                | (1 << Character.NON_SPACING_MARK)
                | (1 << Character.ENCLOSING_MARK);
	
	    protected internal override void HandleTransliterate(Replaceable text, Transliterator.Position  pos,
	            bool incremental) {
	        boundaryCount = 0;
	        int boundary = 0;
	        GetBreakIterator(); // Lazy-create it if necessary
	        bi.SetText(new BreakTransliterator.ReplaceableCharacterIterator (text, pos.start, pos.limit,
	                pos.start));
	        // TODO: fix clumsy workaround used below.
	        /*
	         * char[] tempBuffer = new char[text.length()]; text.getChars(0,
	         * text.length(), tempBuffer, 0); bi.setText(new
	         * StringCharacterIterator(new String(tempBuffer), pos.start, pos.limit,
	         * pos.start));
	         */
	        // end debugging
	
	        // To make things much easier, we will stack the boundaries, and then
	        // insert at the end.
	        // generally, we won't need too many, since we will be filtered.
	
	        for (boundary = bi.First(); boundary != IBM.ICU.Text.BreakIterator.DONE
	                && boundary < pos.limit; boundary = bi.Next()) {
	            if (boundary == 0)
	                continue;
	            // HACK: Check to see that preceeding item was a letter
	
	            int cp = IBM.ICU.Text.UTF16.CharAt(text, boundary - 1);
	            int type = IBM.ICU.Lang.UCharacter.GetType(cp);
	            // System.out.println(Integer.toString(cp,16) + " (before): " +
	            // type);
	            if (((1 << type) & LETTER_OR_MARK_MASK) == 0)
	                continue;
	
	            cp = IBM.ICU.Text.UTF16.CharAt(text, boundary);
	            type = IBM.ICU.Lang.UCharacter.GetType(cp);
	            // System.out.println(Integer.toString(cp,16) + " (after): " +
	            // type);
	            if (((1 << type) & LETTER_OR_MARK_MASK) == 0)
	                continue;
	
	            if (boundaryCount >= boundaries.Length) { // realloc if necessary
	                int[] temp = new int[boundaries.Length * 2];
	                System.Array.Copy((Array)(boundaries),0,(Array)(temp),0,boundaries.Length);
	                boundaries = temp;
	            }
	
	            boundaries[boundaryCount++] = boundary;
	            // System.out.println(boundary);
	        }
	
	        int delta = 0;
	        int lastBoundary = 0;
	
	        if (boundaryCount != 0) { // if we found something, adjust
	            delta = boundaryCount * insertion.Length;
	            lastBoundary = boundaries[boundaryCount - 1];
	
	            // we do this from the end backwards, so that we don't have to keep
	            // updating.
	
	            while (boundaryCount > 0) {
	                boundary = boundaries[--boundaryCount];
	                text.Replace(boundary, boundary, insertion);
	            }
	        }
	
	        // Now fix up the return values
	        pos.contextLimit += delta;
	        pos.limit += delta;
	        pos.start = (incremental) ? lastBoundary + delta : pos.limit;
	    }
	
	    /// <summary>
	    /// Registers standard variants with the system. Called by Transliterator
	    /// during initialization.
	    /// </summary>
	    ///
	    static internal void Register() {
	        // false means that it is invisible
	        Transliterator trans = new BreakTransliterator("Any-BreakInternal",
	                null);
	        IBM.ICU.Text.Transliterator.RegisterInstance(trans, false);
	        /*
	         * Transliterator.registerFactory("Any-Break", new
	         * Transliterator.Factory() { public Transliterator getInstance(String
	         * ID) { return new BreakTransliterator("Any-Break", null); } });
	         */
	    }
	
	    // Hack, just to get a real character iterator.
	    internal sealed class ReplaceableCharacterIterator : 
	            ICharacterIterator {
	        private Replaceable text;
	
	        private int begin;
	
	        private int end;
	
	        // invariant: begin <= pos <= end
	        private int pos;
	
	        /**
	         * Constructs an iterator with an initial index of 0.
	         */
	        /*
	         * public ReplaceableCharacterIterator(Replaceable text) { this(text,
	         * 0); }
	         */
	
	        /**
	         * Constructs an iterator with the specified initial index.
	         * 
	         * @param text
	         *            The String to be iterated over
	         * @param pos
	         *            Initial iterator position
	         */
	        /*
	         * public ReplaceableCharacterIterator(Replaceable text, int pos) {
	         * this(text, 0, text.length(), pos); }
	         */
	
	        /// <summary>
	        /// Constructs an iterator over the given range of the given string, with
	        /// the index set at the specified position.
	        /// </summary>
	        ///
	        /// <param name="text_0">The String to be iterated over</param>
	        /// <param name="begin_1">Index of the first character</param>
	        /// <param name="end_2">Index of the character following the last character</param>
	        /// <param name="pos_3">Initial iterator position</param>
	        public ReplaceableCharacterIterator(Replaceable text_0, int begin_1,
	                int end_2, int pos_3) {
	            if (text_0 == null) {
	                throw new NullReferenceException();
	            }
	            this.text = text_0;
	
	            if (begin_1 < 0 || begin_1 > end_2 || end_2 > text_0.Length()) {
	                throw new ArgumentException("Invalid substring range");
	            }
	
	            if (pos_3 < begin_1 || pos_3 > end_2) {
	                throw new ArgumentException("Invalid position");
	            }
	
	            this.begin = begin_1;
	            this.end = end_2;
	            this.pos = pos_3;
	        }
	
	        /// <summary>
	        /// Reset this iterator to point to a new string. This package-visible
	        /// method is used by other java.text classes that want to avoid
	        /// allocating new ReplaceableCharacterIterator objects every time their
	        /// setText method is called.
	        /// </summary>
	        ///
	        /// <param name="text_0">The String to be iterated over</param>
	        public void SetText(Replaceable text_0) {
	            if (text_0 == null) {
	                throw new NullReferenceException();
	            }
	            this.text = text_0;
	            this.begin = 0;
	            this.end = text_0.Length();
	            this.pos = 0;
	        }
	
	        /// <summary>
	        /// Implements CharacterIterator.first() for String.
	        /// </summary>
	        ///
	        /// <seealso cref="null"/>
	        public char First() {
	            pos = begin;
	            return Current();
	        }
	
	        /// <summary>
	        /// Implements CharacterIterator.last() for String.
	        /// </summary>
	        ///
	        /// <seealso cref="null"/>
	        public char Last() {
	            if (end != begin) {
	                pos = end - 1;
	            } else {
	                pos = end;
	            }
	            return Current();
	        }
	
	        /// <summary>
	        /// Implements CharacterIterator.setIndex() for String.
	        /// </summary>
	        ///
	        /// <seealso cref="null"/>
	        public char SetIndex(int p) {
	            if (p < begin || p > end) {
	                throw new ArgumentException("Invalid index");
	            }
	            pos = p;
	            return Current();
	        }
	
	        /// <summary>
	        /// Implements CharacterIterator.current() for String.
	        /// </summary>
	        ///
	        /// <seealso cref="null"/>
	        public char Current() {
	            if (pos >= begin && pos < end) {
	                return text.CharAt(pos);
	            } else {
	                return ILOG.J2CsMapping.Text.CharacterIterator.Done;
	            }
	        }
	
	        /// <summary>
	        /// Implements CharacterIterator.next() for String.
	        /// </summary>
	        ///
	        /// <seealso cref="null"/>
	        public char Next() {
	            if (pos < end - 1) {
	                pos++;
	                return text.CharAt(pos);
	            } else {
	                pos = end;
	                return ILOG.J2CsMapping.Text.CharacterIterator.Done;
	            }
	        }
	
	        /// <summary>
	        /// Implements CharacterIterator.previous() for String.
	        /// </summary>
	        ///
	        /// <seealso cref="null"/>
	        public char Previous() {
	            if (pos > begin) {
	                pos--;
	                return text.CharAt(pos);
	            } else {
	                return ILOG.J2CsMapping.Text.CharacterIterator.Done;
	            }
	        }
	
	        /// <summary>
	        /// Implements CharacterIterator.getBeginIndex() for String.
	        /// </summary>
	        ///
	        /// <seealso cref="null"/>
	        public int GetBeginIndex() {
	            return begin;
	        }
	
	        /// <summary>
	        /// Implements CharacterIterator.getEndIndex() for String.
	        /// </summary>
	        ///
	        /// <seealso cref="null"/>
	        public int GetEndIndex() {
	            return end;
	        }
	
	        /// <summary>
	        /// Implements CharacterIterator.getIndex() for String.
	        /// </summary>
	        ///
	        /// <seealso cref="null"/>
	        public int GetIndex() {
	            return pos;
	        }
	
	        /// <summary>
	        /// Compares the equality of two ReplaceableCharacterIterator objects.
	        /// </summary>
	        ///
	        /// <param name="obj">the ReplaceableCharacterIterator object to be comparedwith.</param>
	        /// <returns>true if the given obj is the same as this
	        /// ReplaceableCharacterIterator object; false otherwise.</returns>
	        public override bool Equals(Object obj) {
	            if ((Object) this == obj) {
	                return true;
	            }
	            if (!(obj  is  BreakTransliterator.ReplaceableCharacterIterator )) {
	                return false;
	            }
	
	            BreakTransliterator.ReplaceableCharacterIterator  that = (BreakTransliterator.ReplaceableCharacterIterator ) obj;
	
	            if (GetHashCode() != that.GetHashCode()) {
	                return false;
	            }
	            if (!text.Equals(that.text)) {
	                return false;
	            }
	            if (pos != that.pos || begin != that.begin || end != that.end) {
	                return false;
	            }
	            return true;
	        }
	
	        /// <summary>
	        /// Computes a hashcode for this iterator.
	        /// </summary>
	        ///
	        /// <returns>A hash code</returns>
	        public override int GetHashCode() {
	            return text.GetHashCode() ^ pos ^ begin ^ end;
	        }
	
	        /// <summary>
	        /// Creates a copy of this iterator.
	        /// </summary>
	        ///
	        /// <returns>A copy of this</returns>
	        public Object Clone() {
	            try {
	                BreakTransliterator.ReplaceableCharacterIterator  other = (BreakTransliterator.ReplaceableCharacterIterator ) base.MemberwiseClone();
	                return other;
	            } catch (Exception e) {
	                throw new InvalidOperationException();
	            }
	        }
	
	    }
	
	}
}
