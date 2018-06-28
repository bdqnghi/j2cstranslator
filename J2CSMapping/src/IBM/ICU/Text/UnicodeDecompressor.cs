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
	
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.IO;
	using System.Runtime.CompilerServices;
	
	/// <summary>
	/// A decompression engine implementing the Standard Compression Scheme for
	/// Unicode (SCSU) as outlined in <A
	/// HREF="http://www.unicode.org/unicode/reports/tr6">Unicode Technical Report
	/// #6</A>.
	/// <P>
	/// <STRONG>USAGE</STRONG>
	/// </P>
	/// <P>
	/// The static methods on <TT>UnicodeDecompressor</TT> may be used in a
	/// straightforward manner to decompress simple strings:
	/// </P>
	/// <PRE>
	/// byte [] compressed = ... ; // get compressed bytes from somewhere
	/// String result = UnicodeDecompressor.decompress(compressed);
	/// </PRE>
	/// <P>
	/// The static methods have a fairly large memory footprint. For finer-grained
	/// control over memory usage, <TT>UnicodeDecompressor</TT> offers more powerful
	/// APIs allowing iterative decompression:
	/// </P>
	/// <PRE>
	/// // Decompress an array &quot;bytes&quot; of length &quot;len&quot; using a buffer of 512 chars
	/// // to the Writer &quot;out&quot;
	/// UnicodeDecompressor myDecompressor = new UnicodeDecompressor();
	/// final static int BUFSIZE = 512;
	/// char[] charBuffer = new char[BUFSIZE];
	/// int charsWritten = 0;
	/// int[] bytesRead = new int[1];
	/// int totalBytesDecompressed = 0;
	/// int totalCharsWritten = 0;
	/// do {
	/// // do the decompression
	/// charsWritten = myDecompressor.decompress(bytes, totalBytesDecompressed,
	/// len, bytesRead, charBuffer, 0, BUFSIZE);
	/// // do something with the current set of chars
	/// out.write(charBuffer, 0, charsWritten);
	/// // update the no. of bytes decompressed
	/// totalBytesDecompressed += bytesRead[0];
	/// // update the no. of chars written
	/// totalCharsWritten += charsWritten;
	/// } while (totalBytesDecompressed &lt; len);
	/// myDecompressor.reset(); // reuse decompressor
	/// </PRE>
	/// <P>
	/// Decompression is performed according to the standard set forth in <A
	/// HREF="http://www.unicode.org/unicode/reports/tr6">Unicode Technical Report
	/// #6</A>
	/// </P>
	/// </summary>
	///
	/// <seealso cref="T:IBM.ICU.Text.UnicodeCompressor"/>
	/// @stable ICU 2.4
	public sealed class UnicodeDecompressor : SCSU {
	    // ==========================
	    // Instance variables
	    // ==========================
	
	    /// <summary>
	    /// Alias to current dynamic window 
	    /// </summary>
	    ///
	    private int fCurrentWindow;
	
	    /// <summary>
	    /// Dynamic compression window offsets 
	    /// </summary>
	    ///
	    private int[] fOffsets;
	
	    /// <summary>
	    /// Current compression mode 
	    /// </summary>
	    ///
	    private int fMode;
	
	    /// <summary>
	    /// Size of our internal buffer 
	    /// </summary>
	    ///
	    private const int BUFSIZE = 3;
	
	    /// <summary>
	    /// Internal buffer for saving state 
	    /// </summary>
	    ///
	    private byte[] fBuffer;
	
	    /// <summary>
	    /// Number of characters in our internal buffer 
	    /// </summary>
	    ///
	    private int fBufferLength;
	
	    /// <summary>
	    /// Create a UnicodeDecompressor. Sets all windows to their default values.
	    /// </summary>
	    ///
	    /// <seealso cref="M:IBM.ICU.Text.UnicodeDecompressor.Reset"/>
	    /// @stable ICU 2.4
	    public UnicodeDecompressor() {
	        this.fCurrentWindow = 0;
	        this.fOffsets = new int[IBM.ICU.Text.SCSU_Constants.NUMWINDOWS];
	        this.fMode = IBM.ICU.Text.SCSU_Constants.SINGLEBYTEMODE;
	        this.fBuffer = new byte[BUFSIZE];
	        this.fBufferLength = 0;
	        Reset(); // initialize to defaults
	    }
	
	    /// <summary>
	    /// Decompress a byte array into a String.
	    /// </summary>
	    ///
	    /// <param name="buffer">The byte array to decompress.</param>
	    /// <returns>A String containing the decompressed characters.</returns>
	    /// <seealso cref="M:IBM.ICU.Text.UnicodeDecompressor.Decompress(null, System.Int32, System.Int32)"/>
	    /// @stable ICU 2.4
	    public static String Decompress(byte[] buffer) {
	        char[] buf = Decompress(buffer, 0, buffer.Length);
	        return ILOG.J2CsMapping.Util.StringUtil.NewString(buf);
	    }
	
	    /// <summary>
	    /// Decompress a byte array into a Unicode character array.
	    /// </summary>
	    ///
	    /// <param name="buffer">The byte array to decompress.</param>
	    /// <param name="start">The start of the byte run to decompress.</param>
	    /// <param name="limit">The limit of the byte run to decompress.</param>
	    /// <returns>A character array containing the decompressed bytes.</returns>
	    /// <seealso cref="M:IBM.ICU.Text.UnicodeDecompressor.Decompress(null)"/>
	    /// @stable ICU 2.4
	    public static char[] Decompress(byte[] buffer, int start, int limit) {
	        UnicodeDecompressor comp = new UnicodeDecompressor();
	
	        // use a buffer we know will never overflow
	        // in the worst case, each byte will decompress
	        // to a surrogate pair (buffer must be at least 2 chars)
	        int len = Math.Max(2,2 * (limit - start));
	        char[] temp = new char[len];
	
	        int charCount = comp.Decompress(buffer, start, limit, null, temp, 0,
	                len);
	
	        char[] result = new char[charCount];
	        System.Array.Copy((Array)(temp),0,(Array)(result),0,charCount);
	        return result;
	    }
	
	    /// <summary>
	    /// Decompress a byte array into a Unicode character array.
	    /// This function will either completely fill the output buffer, or consume
	    /// the entire input.
	    /// </summary>
	    ///
	    /// <param name="byteBuffer">The byte buffer to decompress.</param>
	    /// <param name="byteBufferStart">The start of the byte run to decompress.</param>
	    /// <param name="byteBufferLimit">The limit of the byte run to decompress.</param>
	    /// <param name="bytesRead">A one-element array. If not null, on return the number ofbytes read from byteBuffer.</param>
	    /// <param name="charBuffer">A buffer to receive the decompressed data. This buffer must beat minimum two characters in size.</param>
	    /// <param name="charBufferStart">The starting offset to which to write decompressed data.</param>
	    /// <param name="charBufferLimit">The limiting offset for writing decompressed data.</param>
	    /// <returns>The number of Unicode characters written to charBuffer.</returns>
	    /// @stable ICU 2.4
	    public int Decompress(byte[] byteBuffer, int byteBufferStart,
	            int byteBufferLimit, int[] bytesRead, char[] charBuffer,
	            int charBufferStart, int charBufferLimit) {
	        // the current position in the source byte buffer
	        int bytePos = byteBufferStart;
	
	        // the current position in the target char buffer
	        int ucPos = charBufferStart;
	
	        // the current byte from the source buffer
	        int aByte = 0x00;
	
	        // charBuffer must be at least 2 chars in size
	        if (charBuffer.Length < 2 || (charBufferLimit - charBufferStart) < 2)
	            throw new ArgumentException("charBuffer.length < 2");
	
	        // if our internal buffer isn't empty, flush its contents
	        // to the output buffer before doing any more decompression
	        if (fBufferLength > 0) {
	
	            int newBytes = 0;
	
	            // fill the buffer completely, to guarantee one full character
	            if (fBufferLength != BUFSIZE) {
	                newBytes = fBuffer.Length - fBufferLength;
	
	                // verify there are newBytes bytes in byteBuffer
	                if (byteBufferLimit - byteBufferStart < newBytes)
	                    newBytes = byteBufferLimit - byteBufferStart;
	
	                System.Array.Copy((Array)(byteBuffer),byteBufferStart,(Array)(fBuffer),fBufferLength,newBytes);
	            }
	
	            // reset buffer length to 0 before recursive call
	            fBufferLength = 0;
	
	            // call self recursively to decompress the buffer
	            int count = Decompress(fBuffer, 0, fBuffer.Length, null,
	                    charBuffer, charBufferStart, charBufferLimit);
	
	            // update the positions into the arrays
	            ucPos += count;
	            bytePos += newBytes;
	        }
	
	        // the main decompression loop
	        mainLoop: {
	            while (bytePos < byteBufferLimit && ucPos < charBufferLimit) {
	                switch (fMode) {
	                case IBM.ICU.Text.SCSU_Constants.SINGLEBYTEMODE:
	                    // single-byte mode decompression loop
	                    singleByteModeLoop: {
	                        while (bytePos < byteBufferLimit
	                                && ucPos < charBufferLimit) {
	                            aByte = byteBuffer[bytePos++] & 0xFF;
	                            switch (aByte) {
	                            // All bytes from 0x80 through 0xFF are remapped
	                            // to chars or surrogate pairs according to the
	                            // currently active window
	                            case 0x80:
	                            case 0x81:
	                            case 0x82:
	                            case 0x83:
	                            case 0x84:
	                            case 0x85:
	                            case 0x86:
	                            case 0x87:
	                            case 0x88:
	                            case 0x89:
	                            case 0x8A:
	                            case 0x8B:
	                            case 0x8C:
	                            case 0x8D:
	                            case 0x8E:
	                            case 0x8F:
	                            case 0x90:
	                            case 0x91:
	                            case 0x92:
	                            case 0x93:
	                            case 0x94:
	                            case 0x95:
	                            case 0x96:
	                            case 0x97:
	                            case 0x98:
	                            case 0x99:
	                            case 0x9A:
	                            case 0x9B:
	                            case 0x9C:
	                            case 0x9D:
	                            case 0x9E:
	                            case 0x9F:
	                            case 0xA0:
	                            case 0xA1:
	                            case 0xA2:
	                            case 0xA3:
	                            case 0xA4:
	                            case 0xA5:
	                            case 0xA6:
	                            case 0xA7:
	                            case 0xA8:
	                            case 0xA9:
	                            case 0xAA:
	                            case 0xAB:
	                            case 0xAC:
	                            case 0xAD:
	                            case 0xAE:
	                            case 0xAF:
	                            case 0xB0:
	                            case 0xB1:
	                            case 0xB2:
	                            case 0xB3:
	                            case 0xB4:
	                            case 0xB5:
	                            case 0xB6:
	                            case 0xB7:
	                            case 0xB8:
	                            case 0xB9:
	                            case 0xBA:
	                            case 0xBB:
	                            case 0xBC:
	                            case 0xBD:
	                            case 0xBE:
	                            case 0xBF:
	                            case 0xC0:
	                            case 0xC1:
	                            case 0xC2:
	                            case 0xC3:
	                            case 0xC4:
	                            case 0xC5:
	                            case 0xC6:
	                            case 0xC7:
	                            case 0xC8:
	                            case 0xC9:
	                            case 0xCA:
	                            case 0xCB:
	                            case 0xCC:
	                            case 0xCD:
	                            case 0xCE:
	                            case 0xCF:
	                            case 0xD0:
	                            case 0xD1:
	                            case 0xD2:
	                            case 0xD3:
	                            case 0xD4:
	                            case 0xD5:
	                            case 0xD6:
	                            case 0xD7:
	                            case 0xD8:
	                            case 0xD9:
	                            case 0xDA:
	                            case 0xDB:
	                            case 0xDC:
	                            case 0xDD:
	                            case 0xDE:
	                            case 0xDF:
	                            case 0xE0:
	                            case 0xE1:
	                            case 0xE2:
	                            case 0xE3:
	                            case 0xE4:
	                            case 0xE5:
	                            case 0xE6:
	                            case 0xE7:
	                            case 0xE8:
	                            case 0xE9:
	                            case 0xEA:
	                            case 0xEB:
	                            case 0xEC:
	                            case 0xED:
	                            case 0xEE:
	                            case 0xEF:
	                            case 0xF0:
	                            case 0xF1:
	                            case 0xF2:
	                            case 0xF3:
	                            case 0xF4:
	                            case 0xF5:
	                            case 0xF6:
	                            case 0xF7:
	                            case 0xF8:
	                            case 0xF9:
	                            case 0xFA:
	                            case 0xFB:
	                            case 0xFC:
	                            case 0xFD:
	                            case 0xFE:
	                            case 0xFF:
	                                // For offsets <= 0xFFFF, convert to a single char
	                                // by adding the window's offset and subtracting
	                                // the generic compression offset
	                                if (fOffsets[fCurrentWindow] <= 0xFFFF) {
	                                    charBuffer[ucPos++] = (char) (aByte
	                                            + fOffsets[fCurrentWindow] - IBM.ICU.Text.SCSU_Constants.COMPRESSIONOFFSET);
	                                }
	                                // For offsets > 0x10000, convert to a surrogate pair by
	                                // normBase = window's offset - 0x10000
	                                // high surr. = 0xD800 + (normBase >> 10)
	                                // low surr. = 0xDC00 + (normBase & 0x3FF) + (byte &
	                                // 0x7F)
	                                else {
	                                    // make sure there is enough room to write
	                                    // both characters
	                                    // if not, save state and break out
	                                    if ((ucPos + 1) >= charBufferLimit) {
	                                        --bytePos;
	                                        System.Array.Copy((Array)(byteBuffer),bytePos,(Array)(fBuffer),0,byteBufferLimit - bytePos);
	                                        fBufferLength = byteBufferLimit - bytePos;
	                                        bytePos += fBufferLength;
	                                        goto gotomainLoop;
	                                    }
	
	                                    int normalizedBase = fOffsets[fCurrentWindow] - 0x10000;
	                                    charBuffer[ucPos++] = (char) (0xD800 + (normalizedBase >> 10));
	                                    charBuffer[ucPos++] = (char) (0xDC00 + (normalizedBase & 0x3FF) + (aByte & 0x7F));
	                                }
	                                break;
	
	                            // bytes from 0x20 through 0x7F are treated as ASCII and
	                            // are remapped to chars by padding the high byte
	                            // (this is the same as quoting from static window 0)
	                            // NUL (0x00), HT (0x09), CR (0x0A), LF (0x0D)
	                            // are treated as ASCII as well
	                            case 0x00:
	                            case 0x09:
	                            case 0x0A:
	                            case 0x0D:
	                            case 0x20:
	                            case 0x21:
	                            case 0x22:
	                            case 0x23:
	                            case 0x24:
	                            case 0x25:
	                            case 0x26:
	                            case 0x27:
	                            case 0x28:
	                            case 0x29:
	                            case 0x2A:
	                            case 0x2B:
	                            case 0x2C:
	                            case 0x2D:
	                            case 0x2E:
	                            case 0x2F:
	                            case 0x30:
	                            case 0x31:
	                            case 0x32:
	                            case 0x33:
	                            case 0x34:
	                            case 0x35:
	                            case 0x36:
	                            case 0x37:
	                            case 0x38:
	                            case 0x39:
	                            case 0x3A:
	                            case 0x3B:
	                            case 0x3C:
	                            case 0x3D:
	                            case 0x3E:
	                            case 0x3F:
	                            case 0x40:
	                            case 0x41:
	                            case 0x42:
	                            case 0x43:
	                            case 0x44:
	                            case 0x45:
	                            case 0x46:
	                            case 0x47:
	                            case 0x48:
	                            case 0x49:
	                            case 0x4A:
	                            case 0x4B:
	                            case 0x4C:
	                            case 0x4D:
	                            case 0x4E:
	                            case 0x4F:
	                            case 0x50:
	                            case 0x51:
	                            case 0x52:
	                            case 0x53:
	                            case 0x54:
	                            case 0x55:
	                            case 0x56:
	                            case 0x57:
	                            case 0x58:
	                            case 0x59:
	                            case 0x5A:
	                            case 0x5B:
	                            case 0x5C:
	                            case 0x5D:
	                            case 0x5E:
	                            case 0x5F:
	                            case 0x60:
	                            case 0x61:
	                            case 0x62:
	                            case 0x63:
	                            case 0x64:
	                            case 0x65:
	                            case 0x66:
	                            case 0x67:
	                            case 0x68:
	                            case 0x69:
	                            case 0x6A:
	                            case 0x6B:
	                            case 0x6C:
	                            case 0x6D:
	                            case 0x6E:
	                            case 0x6F:
	                            case 0x70:
	                            case 0x71:
	                            case 0x72:
	                            case 0x73:
	                            case 0x74:
	                            case 0x75:
	                            case 0x76:
	                            case 0x77:
	                            case 0x78:
	                            case 0x79:
	                            case 0x7A:
	                            case 0x7B:
	                            case 0x7C:
	                            case 0x7D:
	                            case 0x7E:
	                            case 0x7F:
	                                charBuffer[ucPos++] = (char) aByte;
	                                break;
	
	                            // quote unicode
	                            case IBM.ICU.Text.SCSU_Constants.SQUOTEU:
	                                // verify we have two bytes following tag
	                                // if not, save state and break out
	                                if ((bytePos + 1) >= byteBufferLimit) {
	                                    --bytePos;
	                                    System.Array.Copy((Array)(byteBuffer),bytePos,(Array)(fBuffer),0,byteBufferLimit - bytePos);
	                                    fBufferLength = byteBufferLimit - bytePos;
	                                    bytePos += fBufferLength;
	                                    goto gotomainLoop;
	                                }
	
	                                aByte = byteBuffer[bytePos++];
	                                charBuffer[ucPos++] = (char) (aByte << 8 | (byteBuffer[bytePos++] & 0xFF));
	                                break;
	
	                            // switch to Unicode mode
	                            case IBM.ICU.Text.SCSU_Constants.SCHANGEU:
	                                fMode = IBM.ICU.Text.SCSU_Constants.UNICODEMODE;
	                                goto gotomainLoop;
	
	                            // handle all quote tags
	                            case IBM.ICU.Text.SCSU_Constants.SQUOTE0:
	                            case IBM.ICU.Text.SCSU_Constants.SQUOTE1:
	                            case IBM.ICU.Text.SCSU_Constants.SQUOTE2:
	                            case IBM.ICU.Text.SCSU_Constants.SQUOTE3:
	                            case IBM.ICU.Text.SCSU_Constants.SQUOTE4:
	                            case IBM.ICU.Text.SCSU_Constants.SQUOTE5:
	                            case IBM.ICU.Text.SCSU_Constants.SQUOTE6:
	                            case IBM.ICU.Text.SCSU_Constants.SQUOTE7:
	                                // verify there is a byte following the tag
	                                // if not, save state and break out
	                                if (bytePos >= byteBufferLimit) {
	                                    --bytePos;
	                                    System.Array.Copy((Array)(byteBuffer),bytePos,(Array)(fBuffer),0,byteBufferLimit - bytePos);
	                                    fBufferLength = byteBufferLimit - bytePos;
	                                    bytePos += fBufferLength;
	                                    goto gotomainLoop;
	                                }
	
	                                // if the byte is in the range 0x00 - 0x7F, use
	                                // static window n otherwise, use dynamic window n
	                                int dByte = byteBuffer[bytePos++] & 0xFF;
	                                charBuffer[ucPos++] = (char) (dByte + ((dByte >= 0x00
	                                        && dByte < 0x80) ? IBM.ICU.Text.SCSU_Constants.sOffsets[aByte - IBM.ICU.Text.SCSU_Constants.SQUOTE0]
	                                        : (fOffsets[aByte - IBM.ICU.Text.SCSU_Constants.SQUOTE0] - IBM.ICU.Text.SCSU_Constants.COMPRESSIONOFFSET)));
	                                break;
	
	                            // handle all change tags
	                            case IBM.ICU.Text.SCSU_Constants.SCHANGE0:
	                            case IBM.ICU.Text.SCSU_Constants.SCHANGE1:
	                            case IBM.ICU.Text.SCSU_Constants.SCHANGE2:
	                            case IBM.ICU.Text.SCSU_Constants.SCHANGE3:
	                            case IBM.ICU.Text.SCSU_Constants.SCHANGE4:
	                            case IBM.ICU.Text.SCSU_Constants.SCHANGE5:
	                            case IBM.ICU.Text.SCSU_Constants.SCHANGE6:
	                            case IBM.ICU.Text.SCSU_Constants.SCHANGE7:
	                                fCurrentWindow = aByte - IBM.ICU.Text.SCSU_Constants.SCHANGE0;
	                                break;
	
	                            // handle all define tags
	                            case IBM.ICU.Text.SCSU_Constants.SDEFINE0:
	                            case IBM.ICU.Text.SCSU_Constants.SDEFINE1:
	                            case IBM.ICU.Text.SCSU_Constants.SDEFINE2:
	                            case IBM.ICU.Text.SCSU_Constants.SDEFINE3:
	                            case IBM.ICU.Text.SCSU_Constants.SDEFINE4:
	                            case IBM.ICU.Text.SCSU_Constants.SDEFINE5:
	                            case IBM.ICU.Text.SCSU_Constants.SDEFINE6:
	                            case IBM.ICU.Text.SCSU_Constants.SDEFINE7:
	                                // verify there is a byte following the tag
	                                // if not, save state and break out
	                                if (bytePos >= byteBufferLimit) {
	                                    --bytePos;
	                                    System.Array.Copy((Array)(byteBuffer),bytePos,(Array)(fBuffer),0,byteBufferLimit - bytePos);
	                                    fBufferLength = byteBufferLimit - bytePos;
	                                    bytePos += fBufferLength;
	                                    goto gotomainLoop;
	                                }
	
	                                fCurrentWindow = aByte - IBM.ICU.Text.SCSU_Constants.SDEFINE0;
	                                fOffsets[fCurrentWindow] = IBM.ICU.Text.SCSU_Constants.sOffsetTable[byteBuffer[bytePos++] & 0xFF];
	                                break;
	
	                            // handle define extended tag
	                            case IBM.ICU.Text.SCSU_Constants.SDEFINEX:
	                                // verify we have two bytes following tag
	                                // if not, save state and break out
	                                if ((bytePos + 1) >= byteBufferLimit) {
	                                    --bytePos;
	                                    System.Array.Copy((Array)(byteBuffer),bytePos,(Array)(fBuffer),0,byteBufferLimit - bytePos);
	                                    fBufferLength = byteBufferLimit - bytePos;
	                                    bytePos += fBufferLength;
	                                    goto gotomainLoop;
	                                }
	
	                                aByte = byteBuffer[bytePos++] & 0xFF;
	                                fCurrentWindow = (aByte & 0xE0) >> 5;
	                                fOffsets[fCurrentWindow] = 0x10000 + (0x80 * (((aByte & 0x1F) << 8) | (byteBuffer[bytePos++] & 0xFF)));
	                                break;
	
	                            // reserved, shouldn't happen
	                            case IBM.ICU.Text.SCSU_Constants.SRESERVED:
	                                break;
	
	                            } // end switch
	                        } // end while
	                    }
	                    gotosingleByteModeLoop:
	                    ;
	                    break;
	
	                case IBM.ICU.Text.SCSU_Constants.UNICODEMODE:
	                    // unicode mode decompression loop
	                    unicodeModeLoop: {
	                        while (bytePos < byteBufferLimit
	                                && ucPos < charBufferLimit) {
	                            aByte = byteBuffer[bytePos++] & 0xFF;
	                            switch (aByte) {
	                            // handle all define tags
	                            case IBM.ICU.Text.SCSU_Constants.UDEFINE0:
	                            case IBM.ICU.Text.SCSU_Constants.UDEFINE1:
	                            case IBM.ICU.Text.SCSU_Constants.UDEFINE2:
	                            case IBM.ICU.Text.SCSU_Constants.UDEFINE3:
	                            case IBM.ICU.Text.SCSU_Constants.UDEFINE4:
	                            case IBM.ICU.Text.SCSU_Constants.UDEFINE5:
	                            case IBM.ICU.Text.SCSU_Constants.UDEFINE6:
	                            case IBM.ICU.Text.SCSU_Constants.UDEFINE7:
	                                // verify there is a byte following tag
	                                // if not, save state and break out
	                                if (bytePos >= byteBufferLimit) {
	                                    --bytePos;
	                                    System.Array.Copy((Array)(byteBuffer),bytePos,(Array)(fBuffer),0,byteBufferLimit - bytePos);
	                                    fBufferLength = byteBufferLimit - bytePos;
	                                    bytePos += fBufferLength;
	                                    goto gotomainLoop;
	                                }
	
	                                fCurrentWindow = aByte - IBM.ICU.Text.SCSU_Constants.UDEFINE0;
	                                fOffsets[fCurrentWindow] = IBM.ICU.Text.SCSU_Constants.sOffsetTable[byteBuffer[bytePos++] & 0xFF];
	                                fMode = IBM.ICU.Text.SCSU_Constants.SINGLEBYTEMODE;
	                                goto gotomainLoop;
	
	                            // handle define extended tag
	                            case IBM.ICU.Text.SCSU_Constants.UDEFINEX:
	                                // verify we have two bytes following tag
	                                // if not, save state and break out
	                                if ((bytePos + 1) >= byteBufferLimit) {
	                                    --bytePos;
	                                    System.Array.Copy((Array)(byteBuffer),bytePos,(Array)(fBuffer),0,byteBufferLimit - bytePos);
	                                    fBufferLength = byteBufferLimit - bytePos;
	                                    bytePos += fBufferLength;
	                                    goto gotomainLoop;
	                                }
	
	                                aByte = byteBuffer[bytePos++] & 0xFF;
	                                fCurrentWindow = (aByte & 0xE0) >> 5;
	                                fOffsets[fCurrentWindow] = 0x10000 + (0x80 * (((aByte & 0x1F) << 8) | (byteBuffer[bytePos++] & 0xFF)));
	                                fMode = IBM.ICU.Text.SCSU_Constants.SINGLEBYTEMODE;
	                                goto gotomainLoop;
	
	                            // handle all change tags
	                            case IBM.ICU.Text.SCSU_Constants.UCHANGE0:
	                            case IBM.ICU.Text.SCSU_Constants.UCHANGE1:
	                            case IBM.ICU.Text.SCSU_Constants.UCHANGE2:
	                            case IBM.ICU.Text.SCSU_Constants.UCHANGE3:
	                            case IBM.ICU.Text.SCSU_Constants.UCHANGE4:
	                            case IBM.ICU.Text.SCSU_Constants.UCHANGE5:
	                            case IBM.ICU.Text.SCSU_Constants.UCHANGE6:
	                            case IBM.ICU.Text.SCSU_Constants.UCHANGE7:
	                                fCurrentWindow = aByte - IBM.ICU.Text.SCSU_Constants.UCHANGE0;
	                                fMode = IBM.ICU.Text.SCSU_Constants.SINGLEBYTEMODE;
	                                goto gotomainLoop;
	
	                            // quote unicode
	                            case IBM.ICU.Text.SCSU_Constants.UQUOTEU:
	                                // verify we have two bytes following tag
	                                // if not, save state and break out
	                                if (bytePos >= byteBufferLimit - 1) {
	                                    --bytePos;
	                                    System.Array.Copy((Array)(byteBuffer),bytePos,(Array)(fBuffer),0,byteBufferLimit - bytePos);
	                                    fBufferLength = byteBufferLimit - bytePos;
	                                    bytePos += fBufferLength;
	                                    goto gotomainLoop;
	                                }
	
	                                aByte = byteBuffer[bytePos++];
	                                charBuffer[ucPos++] = (char) (aByte << 8 | (byteBuffer[bytePos++] & 0xFF));
	                                break;
	
	                            default:
	                                // verify there is a byte following tag
	                                // if not, save state and break out
	                                if (bytePos >= byteBufferLimit) {
	                                    --bytePos;
	                                    System.Array.Copy((Array)(byteBuffer),bytePos,(Array)(fBuffer),0,byteBufferLimit - bytePos);
	                                    fBufferLength = byteBufferLimit - bytePos;
	                                    bytePos += fBufferLength;
	                                    goto gotomainLoop;
	                                }
	
	                                charBuffer[ucPos++] = (char) (aByte << 8 | (byteBuffer[bytePos++] & 0xFF));
	                                break;
	
	                            } // end switch
	                        } // end while
	                    }
	                    gotounicodeModeLoop:
	                    ;
	                    break;
	
	                } // end switch( fMode )
	            } // end while
	        }
	        gotomainLoop:
	        ;
	
	        // fill in output parameter
	        if (bytesRead != null)
	            bytesRead[0] = (bytePos - byteBufferStart);
	
	        // return # of chars written
	        return (ucPos - charBufferStart);
	    }
	
	    /// <summary>
	    /// Reset the decompressor to its initial state.
	    /// </summary>
	    ///
	    /// @stable ICU 2.4
	    public void Reset() {
	        // reset dynamic windows
	        fOffsets[0] = 0x0080; // Latin-1
	        fOffsets[1] = 0x00C0; // Latin-1 Supplement + Latin Extended-A
	        fOffsets[2] = 0x0400; // Cyrillic
	        fOffsets[3] = 0x0600; // Arabic
	        fOffsets[4] = 0x0900; // Devanagari
	        fOffsets[5] = 0x3040; // Hiragana
	        fOffsets[6] = 0x30A0; // Katakana
	        fOffsets[7] = 0xFF00; // Fullwidth ASCII
	
	        fCurrentWindow = 0; // Make current window Latin-1
	        fMode = IBM.ICU.Text.SCSU_Constants.SINGLEBYTEMODE; // Always start in single-byte mode
	        fBufferLength = 0; // Empty buffer
	    }
	}
}
