/*
 * *****************************************************************************
 * Copyright (C) 2007, International Business Machines Corporation and others.
 * All Rights Reserved.
 * *****************************************************************************
 */
// --------------------------------------------------------------------------------------------------
// This file was automatically generated by J2CS Translator (http://j2cstranslator.sourceforge.net/). 
// Version 1.3.6.20101125_01     
// 12/2/10 11:47 AM    
// ${CustomMessageForDisclaimer}                                                                             
// --------------------------------------------------------------------------------------------------
 namespace IBM.ICU.Impl {
	
	using ILOG.J2CsMapping.Collections;
	using ILOG.J2CsMapping.Collections.Generics;
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.IO;
	using System.Runtime.CompilerServices;
	
	/// <summary>
	/// TextTrieMap is a trie implementation for supporting fast prefix match for the
	/// key.
	/// </summary>
	///
	public class TextTrieMap {
	    /// <summary>
	    /// Constructs a TextTrieMap object.
	    /// </summary>
	    ///
	    /// <param name="ignoreCase">true to use case insensitive match</param>
	    public TextTrieMap(bool ignoreCase) {
	        this.root = new TextTrieMap.CharacterNode (this, 0);
	        this.ignoreCase = ignoreCase;
	    }
	
	    /// <summary>
	    /// Adds the text key and its associated object in this object.
	    /// </summary>
	    ///
	    /// <param name="text">The text.</param>
	    /// <param name="o">The object associated with the text.</param>
	    [MethodImpl(MethodImplOptions.Synchronized)]
	    public void Put(String text, Object o) {
	        TextTrieMap.CharacterNode  node = root;
	        for (int i = 0; i < text.Length; i++) {
	            int ch = IBM.ICU.Text.UTF16.CharAt(text, i);
	            node = node.AddChildNode(ch);
	            if (IBM.ICU.Text.UTF16.GetCharCount(ch) == 2) {
	                i++;
	            }
	        }
	        node.AddObject(o);
	    }
	
	    /// <summary>
	    /// Gets an iterator of the objects associated with the longest prefix
	    /// matching string key.
	    /// </summary>
	    ///
	    /// <param name="text">The text to be matched with prefixes.</param>
	    /// <returns>An iterator of the objects associated with the longest prefix
	    /// matching matching key, or null if no matching entry is found.</returns>
	    public IIterator Get(String text) {
	        return Get(text, 0);
	    }
	
	    /// <summary>
	    /// Gets an iterator of the objects associated with the longest prefix
	    /// matching string key starting at the specified position.
	    /// </summary>
	    ///
	    /// <param name="text">The text to be matched with prefixes.</param>
	    /// <param name="start">The start index of of the text</param>
	    /// <returns>An iterator of the objects associated with the longest prefix
	    /// matching matching key, or null if no matching entry is found.</returns>
	    public IIterator Get(String text, int start) {
	        TextTrieMap.LongestMatchHandler  handler = new TextTrieMap.LongestMatchHandler ();
	        Find(text, start, handler);
	        return handler.GetMatches();
	    }
	
	    public void Find(String text, TextTrieMap.ResultHandler  handler) {
	        Find(text, 0, handler);
	    }
	
	    public void Find(String text, int start, TextTrieMap.ResultHandler  handler) {
	        Find(root, text, start, start, handler);
	    }
	
	    /*
	     * Find an iterator of the objects associated with the longest prefix
	     * matching string key under the specified node.
	     * 
	     * @param node The character node in this trie.
	     * 
	     * @param text The text to be matched with prefixes.
	     * 
	     * @param start The start index within the text.
	     * 
	     * @param index The current index within the text.
	     * 
	     * @param handler The result handler, ResultHandler#handlePrefixMatch is
	     * called when any prefix match is found.
	     */
	    [MethodImpl(MethodImplOptions.Synchronized)]
	    private void Find(TextTrieMap.CharacterNode  node, String text, int start,
	            int index, TextTrieMap.ResultHandler  handler) {
	        IIterator itr = node.Iterator();
	        if (itr != null) {
	            if (!handler.HandlePrefixMatch(index - start, itr)) {
	                return;
	            }
	        }
	        if (index < text.Length) {
	            IList childNodes = node.GetChildNodes();
	            if (childNodes == null) {
	                return;
	            }
	            int ch = IBM.ICU.Text.UTF16.CharAt(text, index);
	            int chLen = IBM.ICU.Text.UTF16.GetCharCount(ch);
	            for (int i = 0; i < childNodes.Count; i++) {
	                TextTrieMap.CharacterNode  child = (TextTrieMap.CharacterNode ) childNodes[i];
	                if (Compare(ch, child.GetCharacter())) {
	                    Find(child, text, start, index + chLen, handler);
	                    break;
	                }
	            }
	        }
	    }
	
	    /// <summary>
	    /// A private method used for comparing two characters.
	    /// </summary>
	    ///
	    /// <param name="ch1">The first character.</param>
	    /// <param name="ch2">The second character.</param>
	    /// <returns>true if the first character matches the second.</returns>
	    internal bool Compare(int ch1, int ch2) {
	        if (ch1 == ch2) {
	            return true;
	        } else if (ignoreCase) {
	            if (IBM.ICU.Lang.UCharacter.ToLowerCase(ch1) == IBM.ICU.Lang.UCharacter.ToLowerCase(ch2)) {
	                return true;
	            } else if (IBM.ICU.Lang.UCharacter.ToUpperCase(ch1) == IBM.ICU.Lang.UCharacter
	                    .ToUpperCase(ch2)) {
	                return true;
	            }
	        }
	        return false;
	    }
	
	    // The root node of this trie
	    private TextTrieMap.CharacterNode  root;
	
	    // Character matching option
	    internal bool ignoreCase;
	
	    /// <summary>
	    /// Inner class representing a character node in the trie.
	    /// </summary>
	    ///
	        internal class CharacterNode {
	            private TextTrieMap outer_TextTrieMap;
	    
	            internal int character;
	    
	            internal IList children;
	    
	            internal IList objlist;
	    
	            /// <summary>
	            /// Constructs a node for the character.
	            /// </summary>
	            ///
	            /// <param name="ch">The character associated with this node.</param>
	            /// <param name="map">TODO</param>
	            public CharacterNode(TextTrieMap map, int ch) {
	                outer_TextTrieMap = map;
	                character = ch;
	            }
	    
	            /// <summary>
	            /// Gets the character associated with this node.
	            /// </summary>
	            ///
	            /// <returns>The character</returns>
	            public int GetCharacter() {
	                return character;
	            }
	    
	            /// <summary>
	            /// Adds the object to the node.
	            /// </summary>
	            ///
	            /// <param name="obj">The object set in the leaf node.</param>
	            public void AddObject(Object obj) {
	                if (objlist == null) {
	                    objlist = new LinkedList();
	                }
	                ILOG.J2CsMapping.Collections.Generics.Collections.Add(objlist,obj);
	            }
	    
	            /// <summary>
	            /// Gets an iterator of the objects associated with the leaf node.
	            /// </summary>
	            ///
	            /// <returns>The iterator or null if no objects are associated with this
	            /// node.</returns>
	            public IIterator Iterator() {
	                if (objlist == null) {
	                    return null;
	                }
	                return new ILOG.J2CsMapping.Collections.IteratorAdapter(objlist.GetEnumerator());
	            }
	    
	            /// <summary>
	            /// Adds a child node for the character under this character node in the
	            /// trie. When the matching child node already exists, the reference of
	            /// the existing child node is returned.
	            /// </summary>
	            ///
	            /// <param name="ch">The character associated with a child node.</param>
	            /// <returns>The child node.</returns>
	            public TextTrieMap.CharacterNode  AddChildNode(int ch) {
	                if (children == null) {
	                    children = new ArrayList();
	                    TextTrieMap.CharacterNode  newNode = new TextTrieMap.CharacterNode (outer_TextTrieMap, ch);
	                    ILOG.J2CsMapping.Collections.Generics.Collections.Add(children,newNode);
	                    return newNode;
	                }
	                TextTrieMap.CharacterNode  node = null;
	                for (int i = 0; i < children.Count; i++) {
	                    TextTrieMap.CharacterNode  cur = (TextTrieMap.CharacterNode ) children[i];
	                    if (outer_TextTrieMap.Compare(ch, cur.GetCharacter())) {
	                        node = cur;
	                        break;
	                    }
	                }
	                if (node == null) {
	                    node = new TextTrieMap.CharacterNode (outer_TextTrieMap, ch);
	                    ILOG.J2CsMapping.Collections.Generics.Collections.Add(children,node);
	                }
	                return node;
	            }
	    
	            /// <summary>
	            /// Gets the list of child nodes under this node.
	            /// </summary>
	            ///
	            /// <returns>The list of child nodes.</returns>
	            public IList GetChildNodes() {
	                return children;
	            }
	        }
	
	    /// <summary>
	    /// Callback handler for processing prefix matches used by find method.
	    /// </summary>
	    ///
	    public interface ResultHandler {
	        /// <summary>
	        /// Handles a prefix key match
	        /// </summary>
	        ///
	        /// <param name="matchLength">Matched key's length</param>
	        /// <param name="values">An iterator of the objects associated with the matched key</param>
	        /// <returns>Return true to continue the search in the trie, false to
	        /// quit.</returns>
	        bool HandlePrefixMatch(int matchLength, IIterator values);
	    }
	
	    private class LongestMatchHandler : TextTrieMap.ResultHandler  {
	        public LongestMatchHandler() {
	            this.matches = null;
	            this.length = 0;
	        }
	
	        private IIterator matches;
	
	        private int length;
	
	        public virtual bool HandlePrefixMatch(int matchLength, IIterator values) {
	            if (matchLength > length) {
	                length = matchLength;
	                matches = values;
	            }
	            return true;
	        }
	
	        public IIterator GetMatches() {
	            return matches;
	        }
	    }
	}
}
