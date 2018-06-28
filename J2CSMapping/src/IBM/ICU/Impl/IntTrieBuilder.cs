/*
 ******************************************************************************
 * Copyright (C) 1996-2006, International Business Machines Corporation and   *
 * others. All Rights Reserved.                                               *
 ******************************************************************************
 */

// --------------------------------------------------------------------------------------------------
// This file was automatically generated by J2CS Translator (http://j2cstranslator.sourceforge.net/). 
// Version 1.3.6.20101125_01     
// 12/2/10 11:47 AM    
// ${CustomMessageForDisclaimer}                                                                             
// --------------------------------------------------------------------------------------------------
 namespace IBM.ICU.Impl {
	
	using ILOG.J2CsMapping.IO;
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.IO;
	using System.Runtime.CompilerServices;
	
	/// <summary>
	/// Builder class to manipulate and generate a trie. This is useful for ICU data
	/// in primitive types. Provides a compact way to store information that is
	/// indexed by Unicode values, such as character properties, types, keyboard
	/// values, etc. This is very useful when you have a block of Unicode data that
	/// contains significant values while the rest of the Unicode data is unused in
	/// the application or when you have a lot of redundance, such as where all
	/// 21,000 Han ideographs have the same value. However, lookup is much faster
	/// than a hash table. A trie of any primitive data type serves two purposes:
	/// <UL type = round>
	/// <LI>Fast access of the indexed values.
	/// <LI>Smaller memory footprint.
	/// </UL>
	/// This is a direct port from the ICU4C version
	/// </summary>
	///
	public class IntTrieBuilder : TrieBuilder {
	    // public constructor ----------------------------------------------
	
	    /// <summary>
	    /// Copy constructor
	    /// </summary>
	    ///
	    public IntTrieBuilder(IntTrieBuilder table) : base(table) {
	        m_data_ = new int[m_dataCapacity_];
	        System.Array.Copy((Array)(table.m_data_),0,(Array)(m_data_),0,m_dataLength_);
	        m_initialValue_ = table.m_initialValue_;
	        m_leadUnitValue_ = table.m_leadUnitValue_;
	    }
	
	    /// <summary>
	    /// Constructs a build table
	    /// </summary>
	    ///
	    /// <param name="aliasdata">data to be filled into table</param>
	    /// <param name="maxdatalength">maximum data length allowed in table</param>
	    /// <param name="initialvalue">inital data value</param>
	    /// <param name="latin1linear">is latin 1 to be linear</param>
	    public IntTrieBuilder(int[] aliasdata, int maxdatalength, int initialvalue,
	            int leadunitvalue, bool latin1linear) : base() {
	        if (maxdatalength < IBM.ICU.Impl.TrieBuilder.DATA_BLOCK_LENGTH
	                || (latin1linear && maxdatalength < 1024)) {
	            throw new ArgumentException(
	                    "Argument maxdatalength is too small");
	        }
	
	        if (aliasdata != null) {
	            m_data_ = aliasdata;
	        } else {
	            m_data_ = new int[maxdatalength];
	        }
	
	        // preallocate and reset the first data block (block index 0)
	        int j = IBM.ICU.Impl.TrieBuilder.DATA_BLOCK_LENGTH;
	
	        if (latin1linear) {
	            // preallocate and reset the first block (number 0) and Latin-1
	            // (U+0000..U+00ff) after that made sure above that
	            // maxDataLength >= 1024
	            // set indexes to point to consecutive data blocks
	            int i = 0;
	            do {
	                // do this at least for trie->index[0] even if that block is
	                // only partly used for Latin-1
	                m_index_[i++] = j;
	                j += IBM.ICU.Impl.TrieBuilder.DATA_BLOCK_LENGTH;
	            } while (i < (256 >> IBM.ICU.Impl.TrieBuilder.SHIFT_));
	        }
	
	        m_dataLength_ = j;
	        // reset the initially allocated blocks to the initial value
	        ILOG.J2CsMapping.Collections.Arrays.Fill(m_data_,0,m_dataLength_,initialvalue);
	        m_initialValue_ = initialvalue;
	        m_leadUnitValue_ = leadunitvalue;
	        m_dataCapacity_ = maxdatalength;
	        m_isLatin1Linear_ = latin1linear;
	        m_isCompacted_ = false;
	    }
	
	    // public methods -------------------------------------------------------
	
	    /*
	     * public final void print() { int i = 0; int oldvalue = m_index_[i]; int
	     * count = 0; System.out.println("index length " + m_indexLength_ +
	     * " --------------------------"); while (i < m_indexLength_) { if
	     * (m_index_[i] != oldvalue) { System.out.println("index has " + count +
	     * " counts of " + Integer.toHexString(oldvalue)); count = 0; oldvalue =
	     * m_index_[i]; } count ++; i ++; } System.out.println("index has " + count
	     * + " counts of " + Integer.toHexString(oldvalue)); i = 0; oldvalue =
	     * m_data_[i]; count = 0; System.out.println("data length " + m_dataLength_
	     * + " --------------------------"); while (i < m_dataLength_) { if
	     * (m_data_[i] != oldvalue) { if ((oldvalue & 0xf1000000) == 0xf1000000) {
	     * int temp = oldvalue & 0xffffff; temp += 0x320; oldvalue = 0xf1000000 |
	     * temp; } if ((oldvalue & 0xf2000000) == 0xf2000000) { int temp = oldvalue
	     * & 0xffffff; temp += 0x14a; oldvalue = 0xf2000000 | temp; }
	     * System.out.println("data has " + count + " counts of " +
	     * Integer.toHexString(oldvalue)); count = 0; oldvalue = m_data_[i]; } count
	     * ++; i ++; } if ((oldvalue & 0xf1000000) == 0xf1000000) { int temp =
	     * oldvalue & 0xffffff; temp += 0x320; oldvalue = 0xf1000000 | temp; } if
	     * ((oldvalue & 0xf2000000) == 0xf2000000) { int temp = oldvalue & 0xffffff;
	     * temp += 0x14a; oldvalue = 0xf2000000 | temp; }
	     * System.out.println("data has " + count + " counts of " +
	     * Integer.toHexString(oldvalue)); }
	     */
	    /// <summary>
	    /// Gets a 32 bit data from the table data
	    /// </summary>
	    ///
	    /// <param name="ch">codepoint which data is to be retrieved</param>
	    /// <returns>the 32 bit data</returns>
	    public int GetValue(int ch) {
	        // valid, uncompacted trie and valid c?
	        if (m_isCompacted_ || ch > IBM.ICU.Lang.UCharacter.MAX_VALUE || ch < 0) {
	            return 0;
	        }
	
	        int block = m_index_[ch >> IBM.ICU.Impl.TrieBuilder.SHIFT_];
	        return m_data_[Math.Abs(block) + (ch & IBM.ICU.Impl.TrieBuilder.MASK_)];
	    }
	
	    /// <summary>
	    /// Get a 32 bit data from the table data
	    /// </summary>
	    ///
	    /// <param name="ch">code point for which data is to be retrieved.</param>
	    /// <param name="inBlockZero">Output parameter, inBlockZero[0] returns true if the char mapsinto block zero, otherwise false.</param>
	    /// <returns>the 32 bit data value.</returns>
	    public int GetValue(int ch, bool[] inBlockZero) {
	        // valid, uncompacted trie and valid c?
	        if (m_isCompacted_ || ch > IBM.ICU.Lang.UCharacter.MAX_VALUE || ch < 0) {
	            if (inBlockZero != null) {
	                inBlockZero[0] = true;
	            }
	            return 0;
	        }
	
	        int block = m_index_[ch >> IBM.ICU.Impl.TrieBuilder.SHIFT_];
	        if (inBlockZero != null) {
	            inBlockZero[0] = (block == 0);
	        }
	        return m_data_[Math.Abs(block) + (ch & IBM.ICU.Impl.TrieBuilder.MASK_)];
	    }
	
	    /// <summary>
	    /// Sets a 32 bit data in the table data
	    /// </summary>
	    ///
	    /// <param name="ch">codepoint which data is to be set</param>
	    /// <param name="value">to set</param>
	    /// <returns>true if the set is successful, otherwise if the table has been
	    /// compacted return false</returns>
	    public bool SetValue(int ch, int value_ren) {
	        // valid, uncompacted trie and valid c?
	        if (m_isCompacted_ || ch > IBM.ICU.Lang.UCharacter.MAX_VALUE || ch < 0) {
	            return false;
	        }
	
	        int block = GetDataBlock(ch);
	        if (block < 0) {
	            return false;
	        }
	
	        m_data_[block + (ch & IBM.ICU.Impl.TrieBuilder.MASK_)] = value_ren;
	        return true;
	    }
	
	    /// <summary>
	    /// Serializes the build table with 32 bit data
	    /// </summary>
	    ///
	    /// <param name="datamanipulate">builder raw fold method implementation</param>
	    /// <param name="triedatamanipulate">result trie fold method</param>
	    /// <returns>a new trie</returns>
	    public IntTrie Serialize(TrieBuilder.DataManipulate datamanipulate,
	            Trie.DataManipulate triedatamanipulate) {
	        if (datamanipulate == null) {
	            throw new ArgumentException("Parameters can not be null");
	        }
	        // fold and compact if necessary, also checks that indexLength is
	        // within limits
	        if (!m_isCompacted_) {
	            // compact once without overlap to improve folding
	            Compact(false);
	            // fold the supplementary part of the index array
	            Fold(datamanipulate);
	            // compact again with overlap for minimum data array length
	            Compact(true);
	            m_isCompacted_ = true;
	        }
	        // is dataLength within limits?
	        if (m_dataLength_ >= IBM.ICU.Impl.TrieBuilder.MAX_DATA_LENGTH_) {
	            throw new IndexOutOfRangeException("Data length too small".ToString());
	        }
	
	        char[] index = new char[m_indexLength_];
	        int[] data = new int[m_dataLength_];
	        // write the index (stage 1) array and the 32-bit data (stage 2) array
	        // write 16-bit index values shifted right by INDEX_SHIFT_
	        for (int i = 0; i < m_indexLength_; i++) {
	            index[i] = (char) ((int) (((uint) m_index_[i]) >> IBM.ICU.Impl.TrieBuilder.INDEX_SHIFT_));
	        }
	        // write 32-bit data values
	        System.Array.Copy((Array)(m_data_),0,(Array)(data),0,m_dataLength_);
	
	        int options = IBM.ICU.Impl.TrieBuilder.SHIFT_ | (IBM.ICU.Impl.TrieBuilder.INDEX_SHIFT_ << IBM.ICU.Impl.TrieBuilder.OPTIONS_INDEX_SHIFT_);
	        options |= IBM.ICU.Impl.TrieBuilder.OPTIONS_DATA_IS_32_BIT_;
	        if (m_isLatin1Linear_) {
	            options |= IBM.ICU.Impl.TrieBuilder.OPTIONS_LATIN1_IS_LINEAR_;
	        }
	        return new IntTrie(index, data, m_initialValue_, options,
	                triedatamanipulate);
	    }
	
	    /// <summary>
	    /// Serializes the build table to an output stream.
	    /// Compacts the build-time trie after all values are set, and then writes
	    /// the serialized form onto an output stream.
	    /// After this, this build-time Trie can only be serialized again and/or
	    /// closed; no further values can be added.
	    /// This function is the rough equivalent of utrie_seriaize() in ICU4C.
	    /// </summary>
	    ///
	    /// <param name="os">the output stream to which the seriaized trie will be written.If nul, the function still returns the size of the serializedTrie.</param>
	    /// <param name="reduceTo16Bits">If true, reduce the data size to 16 bits. The resultingserialized form can then be used to create a CharTrie.</param>
	    /// <param name="datamanipulate">builder raw fold method implementation</param>
	    /// <returns>the number of bytes written to the output stream.</returns>
	    public int Serialize(Stream os, bool reduceTo16Bits,
	            TrieBuilder.DataManipulate datamanipulate) {
	        if (datamanipulate == null) {
	            throw new ArgumentException("Parameters can not be null");
	        }
	
	        // fold and compact if necessary, also checks that indexLength is
	        // within limits
	        if (!m_isCompacted_) {
	            // compact once without overlap to improve folding
	            Compact(false);
	            // fold the supplementary part of the index array
	            Fold(datamanipulate);
	            // compact again with overlap for minimum data array length
	            Compact(true);
	            m_isCompacted_ = true;
	        }
	
	        // is dataLength within limits?
	        int length;
	        if (reduceTo16Bits) {
	            length = m_dataLength_ + m_indexLength_;
	        } else {
	            length = m_dataLength_;
	        }
	        if (length >= IBM.ICU.Impl.TrieBuilder.MAX_DATA_LENGTH_) {
	            throw new IndexOutOfRangeException("Data length too small".ToString());
	        }
	
	        // struct UTrieHeader {
	        // int32_t signature;
	        // int32_t options (a bit field)
	        // int32_t indexLength
	        // int32_t dataLength
	        length = IBM.ICU.Impl.Trie.HEADER_LENGTH_ + 2 * m_indexLength_;
	        if (reduceTo16Bits) {
	            length += 2 * m_dataLength_;
	        } else {
	            length += 4 * m_dataLength_;
	        }
	
	        if (os == null) {
	            // No output stream. Just return the length of the serialized Trie,
	            // in bytes.
	            return length;
	        }
	
	        DataOutputStream dos = new DataOutputStream(os);
	        dos.WriteInt(IBM.ICU.Impl.Trie.HEADER_SIGNATURE_);
	
	        int options = IBM.ICU.Impl.Trie.INDEX_STAGE_1_SHIFT_
	                | (IBM.ICU.Impl.Trie.INDEX_STAGE_2_SHIFT_ << IBM.ICU.Impl.Trie.HEADER_OPTIONS_INDEX_SHIFT_);
	        if (!reduceTo16Bits) {
	            options |= IBM.ICU.Impl.Trie.HEADER_OPTIONS_DATA_IS_32_BIT_;
	        }
	        if (m_isLatin1Linear_) {
	            options |= IBM.ICU.Impl.Trie.HEADER_OPTIONS_LATIN1_IS_LINEAR_MASK_;
	        }
	        dos.WriteInt(options);
	
	        dos.WriteInt(m_indexLength_);
	        dos.WriteInt(m_dataLength_);
	
	        /*
	         * write the index (stage 1) array and the 16/32-bit data (stage 2)
	         * array
	         */
	        if (reduceTo16Bits) {
	            /*
	             * write 16-bit index values shifted right by UTRIE_INDEX_SHIFT,
	             * after adding indexLength
	             */
	            for (int i = 0; i < m_indexLength_; i++) {
	                int v = (int) (((uint) (m_index_[i] + m_indexLength_)) >> IBM.ICU.Impl.Trie.INDEX_STAGE_2_SHIFT_);
	                dos.WriteChar((char) v);
	            }
	
	            /* write 16-bit data values */
	            for (int i_0 = 0; i_0 < m_dataLength_; i_0++) {
	                int v_1 = m_data_[i_0] & 0x0000ffff;
                    dos.WriteChar((char)v_1);
	            }
	        } else {
	            /* write 16-bit index values shifted right by UTRIE_INDEX_SHIFT */
	            for (int i_2 = 0; i_2 < m_indexLength_; i_2++) {
	                int v_3 = (int) (((uint) (m_index_[i_2])) >> IBM.ICU.Impl.Trie.INDEX_STAGE_2_SHIFT_);
                    dos.WriteChar((char)v_3);
	            }
	
	            /* write 32-bit data values */
	            for (int i_4 = 0; i_4 < m_dataLength_; i_4++) {
	                dos.WriteInt(m_data_[i_4]);
	            }
	        }
	
	        return length;
	
	    }
	
	    /// <summary>
	    /// Set a value in a range of code points [start..limit]. All code points c
	    /// with start &lt;= c &lt; limit will get the value if overwrite is true or
	    /// if the old value is 0.
	    /// </summary>
	    ///
	    /// <param name="start">the first code point to get the value</param>
	    /// <param name="limit">one past the last code point to get the value</param>
	    /// <param name="value">the value</param>
	    /// <param name="overwrite">flag for whether old non-initial values are to be overwritten</param>
	    /// <returns>false if a failure occurred (illegal argument or data array
	    /// overrun)</returns>
	    public bool SetRange(int start, int limit, int value_ren, bool overwrite) {
	        // repeat value in [start..limit[
	        // mark index values for repeat-data blocks by setting bit 31 of the
	        // index values fill around existing values if any, if(overwrite)
	
	        // valid, uncompacted trie and valid indexes?
	        if (m_isCompacted_ || start < IBM.ICU.Lang.UCharacter.MIN_VALUE
	                || start > IBM.ICU.Lang.UCharacter.MAX_VALUE || limit < IBM.ICU.Lang.UCharacter.MIN_VALUE
	                || limit > (IBM.ICU.Lang.UCharacter.MAX_VALUE + 1) || start > limit) {
	            return false;
	        }
	
	        if (start == limit) {
	            return true; // nothing to do
	        }
	
	        if ((start & IBM.ICU.Impl.TrieBuilder.MASK_) != 0) {
	            // set partial block at [start..following block boundary[
	            int block = GetDataBlock(start);
	            if (block < 0) {
	                return false;
	            }
	
	            int nextStart = (start + IBM.ICU.Impl.TrieBuilder.DATA_BLOCK_LENGTH) & ~IBM.ICU.Impl.TrieBuilder.MASK_;
	            if (nextStart <= limit) {
	                FillBlock(block, start & IBM.ICU.Impl.TrieBuilder.MASK_, IBM.ICU.Impl.TrieBuilder.DATA_BLOCK_LENGTH, value_ren,
	                        overwrite);
	                start = nextStart;
	            } else {
	                FillBlock(block, start & IBM.ICU.Impl.TrieBuilder.MASK_, limit & IBM.ICU.Impl.TrieBuilder.MASK_, value_ren, overwrite);
	                return true;
	            }
	        }
	
	        // number of positions in the last, partial block
	        int rest = limit & IBM.ICU.Impl.TrieBuilder.MASK_;
	
	        // round down limit to a block boundary
	        limit &= ~IBM.ICU.Impl.TrieBuilder.MASK_;
	
	        // iterate over all-value blocks
	        int repeatBlock = 0;
	        if (value_ren == m_initialValue_) {
	            // repeatBlock = 0; assigned above
	        } else {
	            repeatBlock = -1;
	        }
	        while (start < limit) {
	            // get index value
	            int block_0 = m_index_[start >> IBM.ICU.Impl.TrieBuilder.SHIFT_];
	            if (block_0 > 0) {
	                // already allocated, fill in value
	                FillBlock(block_0, 0, IBM.ICU.Impl.TrieBuilder.DATA_BLOCK_LENGTH, value_ren, overwrite);
	            } else if (m_data_[-block_0] != value_ren && (block_0 == 0 || overwrite)) {
	                // set the repeatBlock instead of the current block 0 or range
	                // block
	                if (repeatBlock >= 0) {
	                    m_index_[start >> IBM.ICU.Impl.TrieBuilder.SHIFT_] = -repeatBlock;
	                } else {
	                    // create and set and fill the repeatBlock
	                    repeatBlock = GetDataBlock(start);
	                    if (repeatBlock < 0) {
	                        return false;
	                    }
	
	                    // set the negative block number to indicate that it is a
	                    // repeat block
	                    m_index_[start >> IBM.ICU.Impl.TrieBuilder.SHIFT_] = -repeatBlock;
	                    FillBlock(repeatBlock, 0, IBM.ICU.Impl.TrieBuilder.DATA_BLOCK_LENGTH, value_ren, true);
	                }
	            }
	
	            start += IBM.ICU.Impl.TrieBuilder.DATA_BLOCK_LENGTH;
	        }
	
	        if (rest > 0) {
	            // set partial block at [last block boundary..limit[
	            int block_1 = GetDataBlock(start);
	            if (block_1 < 0) {
	                return false;
	            }
	            FillBlock(block_1, 0, rest, value_ren, overwrite);
	        }
	
	        return true;
	    }
	
	    // protected data member ------------------------------------------------
	
	    protected internal int[] m_data_;
	
	    protected internal int m_initialValue_;
	
	    // private data member ------------------------------------------------
	
	    private int m_leadUnitValue_;
	
	    // private methods ------------------------------------------------------
	
	    private int AllocDataBlock() {
	        int newBlock = m_dataLength_;
	        int newTop = newBlock + IBM.ICU.Impl.TrieBuilder.DATA_BLOCK_LENGTH;
	        if (newTop > m_dataCapacity_) {
	            // out of memory in the data array
	            return -1;
	        }
	        m_dataLength_ = newTop;
	        return newBlock;
	    }
	
	    /// <summary>
	    /// No error checking for illegal arguments.
	    /// </summary>
	    ///
	    /// <param name="ch">codepoint to look for</param>
	    /// <returns>-1 if no new data block available (out of memory in data array)</returns>
	    private int GetDataBlock(int ch) {
	        ch >>= IBM.ICU.Impl.TrieBuilder.SHIFT_;
	        int indexValue = m_index_[ch];
	        if (indexValue > 0) {
	            return indexValue;
	        }
	
	        // allocate a new data block
	        int newBlock = AllocDataBlock();
	        if (newBlock < 0) {
	            // out of memory in the data array
	            return -1;
	        }
	        m_index_[ch] = newBlock;
	
	        // copy-on-write for a block from a setRange()
	        System.Array.Copy((Array)(m_data_),Math.Abs(indexValue),(Array)(m_data_),newBlock,IBM.ICU.Impl.TrieBuilder.DATA_BLOCK_LENGTH << 2);
	        return newBlock;
	    }
	
	    /// <summary>
	    /// Compact a folded build-time trie. The compaction - removes blocks that
	    /// are identical with earlier ones - overlaps adjacent blocks as much as
	    /// possible (if overlap == true) - moves blocks in steps of the data
	    /// granularity - moves and overlaps blocks that overlap with multiple values
	    /// in the overlap region
	    /// It does not - try to move and overlap blocks that are not already
	    /// adjacent
	    /// </summary>
	    ///
	    /// <param name="overlap">flag</param>
	    private void Compact(bool overlap) {
	        if (m_isCompacted_) {
	            return; // nothing left to do
	        }
	
	        // compaction
	        // initialize the index map with "block is used/unused" flags
	        FindUnusedBlocks();
	
	        // if Latin-1 is preallocated and linear, then do not compact Latin-1
	        // data
	        int overlapStart = IBM.ICU.Impl.TrieBuilder.DATA_BLOCK_LENGTH;
	        if (m_isLatin1Linear_ && IBM.ICU.Impl.TrieBuilder.SHIFT_ <= 8) {
	            overlapStart += 256;
	        }
	
	        int newStart = IBM.ICU.Impl.TrieBuilder.DATA_BLOCK_LENGTH;
	        int i;
	        for (int start = newStart; start < m_dataLength_;) {
	            // start: index of first entry of current block
	            // newStart: index where the current block is to be moved
	            // (right after current end of already-compacted data)
	            // skip blocks that are not used
	            if (m_map_[(int) (((uint) start) >> IBM.ICU.Impl.TrieBuilder.SHIFT_)] < 0) {
	                // advance start to the next block
	                start += IBM.ICU.Impl.TrieBuilder.DATA_BLOCK_LENGTH;
	                // leave newStart with the previous block!
	                continue;
	            }
	            // search for an identical block
	            if (start >= overlapStart) {
	                i = FindSameDataBlock(m_data_, newStart, start,
	                        (overlap) ? IBM.ICU.Impl.TrieBuilder.DATA_GRANULARITY_ : IBM.ICU.Impl.TrieBuilder.DATA_BLOCK_LENGTH);
	                if (i >= 0) {
	                    // found an identical block, set the other block's index
	                    // value for the current block
	                    m_map_[(int) (((uint) start) >> IBM.ICU.Impl.TrieBuilder.SHIFT_)] = i;
	                    // advance start to the next block
	                    start += IBM.ICU.Impl.TrieBuilder.DATA_BLOCK_LENGTH;
	                    // leave newStart with the previous block!
	                    continue;
	                }
	            }
	            // see if the beginning of this block can be overlapped with the
	            // end of the previous block
	            if (overlap && start >= overlapStart) {
	                /*
	                 * look for maximum overlap (modulo granularity) with the
	                 * previous, adjacent block
	                 */
	                for (i = IBM.ICU.Impl.TrieBuilder.DATA_BLOCK_LENGTH - IBM.ICU.Impl.TrieBuilder.DATA_GRANULARITY_; i > 0
	                        && !IBM.ICU.Impl.TrieBuilder.Equal_int(m_data_, newStart - i, start, i); i -= IBM.ICU.Impl.TrieBuilder.DATA_GRANULARITY_) {
	                }
	            } else {
	                i = 0;
	            }
	            if (i > 0) {
	                // some overlap
	                m_map_[(int) (((uint) start) >> IBM.ICU.Impl.TrieBuilder.SHIFT_)] = newStart - i;
	                // move the non-overlapping indexes to their new positions
	                start += i;
	                for (i = IBM.ICU.Impl.TrieBuilder.DATA_BLOCK_LENGTH - i; i > 0; --i) {
	                    m_data_[newStart++] = m_data_[start++];
	                }
	            } else if (newStart < start) {
	                // no overlap, just move the indexes to their new positions
	                m_map_[(int) (((uint) start) >> IBM.ICU.Impl.TrieBuilder.SHIFT_)] = newStart;
	                for (i = IBM.ICU.Impl.TrieBuilder.DATA_BLOCK_LENGTH; i > 0; --i) {
	                    m_data_[newStart++] = m_data_[start++];
	                }
	            } else { // no overlap && newStart==start
	                m_map_[(int) (((uint) start) >> IBM.ICU.Impl.TrieBuilder.SHIFT_)] = start;
	                newStart += IBM.ICU.Impl.TrieBuilder.DATA_BLOCK_LENGTH;
	                start = newStart;
	            }
	        }
	        // now adjust the index (stage 1) table
	        for (i = 0; i < m_indexLength_; ++i) {
	            m_index_[i] = m_map_[(int) (((uint) System.Math.Abs(m_index_[i])) >> IBM.ICU.Impl.TrieBuilder.SHIFT_)];
	        }
	        m_dataLength_ = newStart;
	    }
	
	    /// <summary>
	    /// Find the same data block
	    /// </summary>
	    ///
	    /// <param name="data">array</param>
	    /// <param name="dataLength"></param>
	    /// <param name="otherBlock"></param>
	    /// <param name="step"></param>
	    private static int FindSameDataBlock(int[] data, int dataLength,
	            int otherBlock, int step) {
	        // ensure that we do not even partially get past dataLength
	        dataLength -= IBM.ICU.Impl.TrieBuilder.DATA_BLOCK_LENGTH;
	
	        for (int block = 0; block <= dataLength; block += step) {
	            if (IBM.ICU.Impl.TrieBuilder.Equal_int(data, block, otherBlock, IBM.ICU.Impl.TrieBuilder.DATA_BLOCK_LENGTH)) {
	                return block;
	            }
	        }
	        return -1;
	    }
	
	    /// <summary>
	    /// Fold the normalization data for supplementary code points into a compact
	    /// area on top of the BMP-part of the trie index, with the lead surrogates
	    /// indexing this compact area.
	    /// Duplicate the index values for lead surrogates: From inside the BMP area,
	    /// where some may be overridden with folded values, to just after the BMP
	    /// area, where they can be retrieved for code point lookups.
	    /// </summary>
	    ///
	    /// <param name="manipulate">fold implementation</param>
	    private void Fold(TrieBuilder.DataManipulate  manipulate) {
	        int[] leadIndexes = new int[IBM.ICU.Impl.TrieBuilder.SURROGATE_BLOCK_COUNT_];
	        int[] index = m_index_;
	        // copy the lead surrogate indexes into a temporary array
	        System.Array.Copy((Array)(index),0xd800 >> IBM.ICU.Impl.TrieBuilder.SHIFT_,(Array)(leadIndexes),0,IBM.ICU.Impl.TrieBuilder.SURROGATE_BLOCK_COUNT_);
	
	        // set all values for lead surrogate code *units* to leadUnitValue
	        // so that by default runtime lookups will find no data for associated
	        // supplementary code points, unless there is data for such code points
	        // which will result in a non-zero folding value below that is set for
	        // the respective lead units
	        // the above saved the indexes for surrogate code *points*
	        // fill the indexes with simplified code from utrie_setRange32()
	        int block = 0;
	        if (m_leadUnitValue_ == m_initialValue_) {
	            // leadUnitValue == initialValue, use all-initial-value block
	            // block = 0; if block here left empty
	        } else {
	            // create and fill the repeatBlock
	            block = AllocDataBlock();
	            if (block < 0) {
	                // data table overflow
	                throw new InvalidOperationException(
	                        "Internal error: Out of memory space");
	            }
	            FillBlock(block, 0, IBM.ICU.Impl.TrieBuilder.DATA_BLOCK_LENGTH, m_leadUnitValue_, true);
	            // negative block number to indicate that it is a repeat block
	            block = -block;
	        }
	        for (int c = (0xd800 >> IBM.ICU.Impl.TrieBuilder.SHIFT_); c < (0xdc00 >> IBM.ICU.Impl.TrieBuilder.SHIFT_); ++c) {
	            m_index_[c] = block;
	        }
	
	        // Fold significant index values into the area just after the BMP
	        // indexes.
	        // In case the first lead surrogate has significant data,
	        // its index block must be used first (in which case the folding is a
	        // no-op).
	        // Later all folded index blocks are moved up one to insert the copied
	        // lead surrogate indexes.
	        int indexLength = IBM.ICU.Impl.TrieBuilder.BMP_INDEX_LENGTH_;
	        // search for any index (stage 1) entries for supplementary code points
	        for (int c_0 = 0x10000; c_0 < 0x110000;) {
	            if (index[c_0 >> IBM.ICU.Impl.TrieBuilder.SHIFT_] != 0) {
	                // there is data, treat the full block for a lead surrogate
	                c_0 &= ~0x3ff;
	                // is there an identical index block?
	                block = IBM.ICU.Impl.TrieBuilder.FindSameIndexBlock(index, indexLength, c_0 >> IBM.ICU.Impl.TrieBuilder.SHIFT_);
	
	                // get a folded value for [c..c+0x400[ and,
	                // if different from the value for the lead surrogate code
	                // point, set it for the lead surrogate code unit
	
	                int value_ren = manipulate.GetFoldedValue(c_0, block
	                        + IBM.ICU.Impl.TrieBuilder.SURROGATE_BLOCK_COUNT_);
	                if (value_ren != GetValue(IBM.ICU.Text.UTF16.GetLeadSurrogate(c_0))) {
	                    if (!SetValue(IBM.ICU.Text.UTF16.GetLeadSurrogate(c_0), value_ren)) {
	                        // data table overflow
	                        throw new IndexOutOfRangeException("Data table overflow".ToString());
	                    }
	                    // if we did not find an identical index block...
	                    if (block == indexLength) {
	                        // move the actual index (stage 1) entries from the
	                        // supplementary position to the new one
	                        System.Array.Copy((Array)(index),c_0 >> IBM.ICU.Impl.TrieBuilder.SHIFT_,(Array)(index),indexLength,IBM.ICU.Impl.TrieBuilder.SURROGATE_BLOCK_COUNT_);
	                        indexLength += IBM.ICU.Impl.TrieBuilder.SURROGATE_BLOCK_COUNT_;
	                    }
	                }
	                c_0 += 0x400;
	            } else {
	                c_0 += IBM.ICU.Impl.TrieBuilder.DATA_BLOCK_LENGTH;
	            }
	        }
	
	        // index array overflow?
	        // This is to guarantee that a folding offset is of the form
	        // UTRIE_BMP_INDEX_LENGTH+n*UTRIE_SURROGATE_BLOCK_COUNT with n=0..1023.
	        // If the index is too large, then n>=1024 and more than 10 bits are
	        // necessary.
	        // In fact, it can only ever become n==1024 with completely unfoldable
	        // data and the additional block of duplicated values for lead
	        // surrogates.
	        if (indexLength >= IBM.ICU.Impl.TrieBuilder.MAX_INDEX_LENGTH_) {
	            throw new IndexOutOfRangeException("Index table overflow".ToString());
	        }
	        // make space for the lead surrogate index block and insert it between
	        // the BMP indexes and the folded ones
	        System.Array.Copy((Array)(index),IBM.ICU.Impl.TrieBuilder.BMP_INDEX_LENGTH_,(Array)(index),IBM.ICU.Impl.TrieBuilder.BMP_INDEX_LENGTH_
	                        + IBM.ICU.Impl.TrieBuilder.SURROGATE_BLOCK_COUNT_,indexLength - IBM.ICU.Impl.TrieBuilder.BMP_INDEX_LENGTH_);
	        System.Array.Copy((Array)(leadIndexes),0,(Array)(index),IBM.ICU.Impl.TrieBuilder.BMP_INDEX_LENGTH_,IBM.ICU.Impl.TrieBuilder.SURROGATE_BLOCK_COUNT_);
	        indexLength += IBM.ICU.Impl.TrieBuilder.SURROGATE_BLOCK_COUNT_;
	        m_indexLength_ = indexLength;
	    }
	
	    /// <exclude/>
	    private void FillBlock(int block, int start, int limit, int value_ren,
	            bool overwrite) {
	        limit += block;
	        block += start;
	        if (overwrite) {
	            while (block < limit) {
	                m_data_[block++] = value_ren;
	            }
	        } else {
	            while (block < limit) {
	                if (m_data_[block] == m_initialValue_) {
	                    m_data_[block] = value_ren;
	                }
	                ++block;
	            }
	        }
	    }
	}
}
