/*
 *  Licensed to the Apache Software Foundation (ASF) under one or more
 *  contributor license agreements.  See the NOTICE file distributed with
 *  this work for additional information regarding copyright ownership.
 *  The ASF licenses this file to You under the Apache License, Version 2.0
 *  (the "License"); you may not use this file except in compliance with
 *  the License.  You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 *  Unless required by applicable law or agreed to in writing, software
 *  distributed under the License is distributed on an "AS IS" BASIS,
 *  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 *  See the License for the specific language governing permissions and
 *  limitations under the License.
 */

// --------------------------------------------------------------------------------------------------
// This file was automatically generated by J2CS Translator (http://j2cstranslator.sourceforge.net/). 
// Version 1.3.6.20101221_01     
// 1/4/11 10:24 AM    
// ${CustomMessageForDisclaimer}                                                                             
// --------------------------------------------------------------------------------------------------
namespace ILOG.J2CsMapping.IO
{
	
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.IO;
	using System.Runtime.CompilerServices;
	
	/// <summary>
	/// PushbackReader is a filter class which allows chars read to be pushed back
	/// into the stream so that they can be reread. Parsers may find this useful.
	/// There is a progammable limit to the number of chars which may be pushed back.
	/// If the buffer of pushed back chars is empty, chars are read from the source
	/// reader.
	/// </summary>
	///
	public class PushbackReader : FilterReader {
		/// <summary>
		/// The <c>char</c> array containing the chars to read.
		/// </summary>
		///
		internal char[] buf;
	
		/// <summary>
		/// The current position within the char array <c>buf</c>. A value
		/// equal to buf.length indicates no chars available. A value of 0 indicates
		/// the buffer is full.
		/// </summary>
		///
		internal int pos;
	
		/// <summary>
		/// Constructs a new PushbackReader on the Reader <c>in</c>. The
		/// size of the pushback buffer is set to the default, or 1 character.
		/// </summary>
		///
		/// <param name="in">the Reader to allow pushback operations on.</param>
		public PushbackReader(TextReader ins0) : base(ins0) {
			buf = new char[1];
			pos = 1;
		}
	
		/// <summary>
		/// Constructs a new PushbackReader on the Reader <c>in</c>. The
		/// size of the pushback buffer is set to <c>size</c> characters.
		/// </summary>
		///
		/// <param name="in">the Reader to allow pushback operations on.</param>
		/// <param name="size">the size of the pushback buffer (<c>size>=0</c>) incharacters.</param>
		/// <exception cref="IllegalArgumentException">if size <= 0</exception>
		public PushbackReader(TextReader ins0, int size) : base(ins0) {
			if (size <= 0) {
				throw new ArgumentException("K0058"); //$NON-NLS-1$
			}
			buf = new char[size];
			pos = size;
		}
	
		/// <summary>
		/// Close this PushbackReader. This implementation closes this reader,
		/// releases the buffer used to pushback characters, and closes the target
		/// reader.
		/// </summary>
		///
		/// <exception cref="IOException">If an error occurs attempting to close this Reader.</exception>
		public override void Close() {
			 lock (this) {
						buf = null;
						ins0.Close();
					}
		}
	
		/// <summary>
		/// Mark this PushbackReader. Since mark is not supported, this method will
		/// always throw IOException.
		/// </summary>
		///
		/// <param name="readAheadLimit">ignored, this method always throws IOException.</param>
		/// <exception cref="IOException">Since mark is not supported byt PushbackReader.</exception>
		public override void Mark(int readAheadLimit) {
			throw new IOException("K007f"); //$NON-NLS-1$
		}
	
		/// <summary>
		/// Answers a boolean indicating whether or not this PushbackReader supports
		/// mark() and reset(). This implementation always answers false since
		/// PushbackReaders do not support mark/reset.
		/// </summary>
		///
		/// <returns>boolean indicates whether or not mark() and reset() are
		/// supported.</returns>
		public override bool MarkSupported() {
			return false;
		}
	
		/// <summary>
		/// Reads a single character from this PushbackReader and returns the result
		/// as an int. The 2 lowest-order bytes are returned or -1 of the end of
		/// stream was encountered. If the pushback buffer does not contain any
		/// available chars then a char from the target input reader is returned.
		/// </summary>
		///
		/// <returns>int The char read or -1 if end of stream.</returns>
		/// <exception cref="IOException">If an IOException occurs.</exception>
		public override int Read() {
			 lock (this) {
						if (buf == null) {
							throw new IOException("K0059"); //$NON-NLS-1$
						}
						/* Is there a pushback character available? */
						if (pos < buf.Length) {
							return buf[pos++];
						}
						/**
						 * Assume read() in the InputStream will return 2 lowest-order bytes
						 * or -1 if end of stream.
						 */
						return ins0.Read();
					}
		}
	
		/// <summary>
		/// Reads at most <c>count</c> chars from this PushbackReader and
		/// stores them in char array <c>buffer</c> starting at
		/// <c>offset</c>. Answer the number of chars actually read or -1 if
		/// no chars were read and end of stream was encountered. This implementation
		/// reads chars from the pushback buffer first, then the target stream if
		/// more chars are required to satisfy <c>count</c>.
		/// </summary>
		///
		/// <param name="buffer">the char array in which to store the read chars.</param>
		/// <param name="offset">the offset in <c>buffer</c> to store the read chars.</param>
		/// <param name="count">the maximum number of chars to store in <c>buffer</c>.</param>
		/// <returns>the number of chars actually read or -1 if end of stream.</returns>
		/// <exception cref="IOException">If an IOException occurs.</exception>
		public override int Read(char[] buffer, int offset, int count) {
			 lock (this) {
						if (null == buf) {
							throw new IOException("K0059"); //$NON-NLS-1$
						}
						// avoid int overflow
						if (offset < 0 || count < 0 || offset > buffer.Length - count) {
							throw new IndexOutOfRangeException();
						}
			
						int copiedChars = 0;
						int copyLength = 0;
						int newOffset = offset;
						/* Are there pushback chars available? */
						if (pos < buf.Length) {
							copyLength = (buf.Length - pos >= count) ? count : buf.Length
									- pos;
							System.Array.Copy((Array)(buf),pos,(Array)(buffer),newOffset,copyLength);
							newOffset += copyLength;
							copiedChars += copyLength;
							/* Use up the chars in the local buffer */
							pos += copyLength;
						}
						/* Have we copied enough? */
						if (copyLength == count) {
							return count;
						}
						int inCopied = ins0.Read(buffer, newOffset, count - copiedChars);
						if (inCopied > 0) {
							return inCopied + copiedChars;
						}
						if (copiedChars == 0) {
							return inCopied;
						}
						return copiedChars;
					}
		}
	
		/// <summary>
		/// Answers a <c>boolean</c> indicating whether or not this
		/// PushbackReader is ready to be read without blocking. If the result is
		/// <c>true</c>, the next <c>read()</c> will not block. If
		/// the result is <c>false</c> this Reader may or may not block when
		/// <c>read()</c> is sent.
		/// </summary>
		///
		/// <returns>boolean <c>true</c> if the receiver will not block when
		/// <c>read()</c> is called, <c>false</c> if unknown
		/// or blocking will occur.</returns>
		/// <exception cref="IOException">If the Reader is already closed or some other IO erroroccurs.</exception>
		public override bool Ready() {
            throw new NotImplementedException();
            /*
			 lock (this) {
						if (buf == null) {
							throw new IOException("K0080"); //$NON-NLS-1$
						}
						return (buf.Length - pos > 0 || ins0.Ready());
					}*/
		}
	
		/// <summary>
		/// Resets this PushbackReader. Since mark is not supported, always throw
		/// IOException.
		/// </summary>
		///
		/// <exception cref="IOException">Since mark is not supported.</exception>
		public override void Reset() {
			throw new IOException("K007f"); //$NON-NLS-1$
		}
	
		/// <summary>
		/// Push back all the chars in <c>buffer</c>. The chars are pushed
		/// so that they would be read back buffer[0], buffer[1], etc. If the push
		/// back buffer cannot handle the entire contents of <c>buffer</c>,
		/// an IOException will be thrown. Some of the buffer may already be in the
		/// buffer after the exception is thrown.
		/// </summary>
		///
		/// <param name="buffer">the char array containing chars to push back into the reader.</param>
		/// <exception cref="IOException">If the pushback buffer becomes, or is, full.</exception>
		public void Unread(char[] buffer) {
			Unread(buffer, 0, buffer.Length);
		}
	
		/// <summary>
		/// Push back <c>count</c> number of chars in <c>buffer</c>
		/// starting at <c>offset</c>. The chars are pushed so that they
		/// would be read back buffer[offset], buffer[offset+1], etc. If the push
		/// back buffer cannot handle the chars copied from <c>buffer</c>,
		/// an IOException will be thrown. Some of the chars may already be in the
		/// buffer after the exception is thrown.
		/// </summary>
		///
		/// <param name="buffer">the char array containing chars to push back into the reader.</param>
		/// <param name="offset">the location to start taking chars to push back.</param>
		/// <param name="count">the number of chars to push back.</param>
		/// <exception cref="IOException">If the pushback buffer becomes, or is, full.</exception>
		public void Unread(char[] buffer, int offset, int count) {
			 lock (this) {
						if (buf == null) {
							throw new IOException("K0059"); //$NON-NLS-1$
						}
						if (count > pos) {
							// Pushback buffer full
							throw new IOException("K007e"); //$NON-NLS-1$
						}
						if (buffer == null) {
							throw new NullReferenceException();
						}
						// avoid int overflow
						if (offset < 0 || count < 0 || offset > buffer.Length - count) {
							throw new IndexOutOfRangeException();
						}
						for (int i = offset + count - 1; i >= offset; i--) {
							Unread(buffer[i]);
						}
					}
		}
	
		/// <summary>
		/// Push back one <c>char</c>. Takes the char <c>oneChar</c>
		/// and puts in in the local buffer of chars to read back before accessing
		/// the target input stream.
		/// </summary>
		///
		/// <param name="oneChar">the char to push back into the stream.</param>
		/// <exception cref="IOException">If the pushback buffer is already full.</exception>
		public void Unread(int oneChar) {
			 lock (this) {
						if (buf == null) {
							throw new IOException("K0059"); //$NON-NLS-1$
						}
						if (pos == 0) {
							throw new IOException("K007e"); //$NON-NLS-1$
						}
						buf[--pos] = (char) oneChar;
					}
		}
	
		/// <summary>
		/// Skips <c>count</c> number of characters in this Reader.
		/// Subsequent <c>read()</c>'s will not return these characters
		/// unless <c>reset()</c> is used.
		/// </summary>
		///
		/// <param name="count">the maximum number of characters to skip.</param>
		/// <returns>the number of characters actually skipped.</returns>
		/// <exception cref="IOException">If the Reader is already closed or some other IO erroroccurs.</exception>
		/// <exception cref="IllegalArgumentException">If count is negative.</exception>
		public override long Skip(long count) {
			if (count < 0) {
				throw new ArgumentException();
			}
			 lock (this) {
						if (buf == null) {
							throw new IOException("K0059"); //$NON-NLS-1$
						}
						if (count == 0) {
							return 0;
						}
						long inSkipped;
						int availableFromBuffer = buf.Length - pos;
						if (availableFromBuffer > 0) {
							long requiredFromIn = count - availableFromBuffer;
							if (requiredFromIn <= 0) {
								pos += (int) count;
								return count;
							}
							pos += availableFromBuffer;
                            throw new NotImplementedException();
							// inSkipped = ins0.Skip(requiredFromIn);
						} else {
                            throw new NotImplementedException();
							//inSkipped = ins0.Skip(count);
						}
						return inSkipped + availableFromBuffer;
					}
		}
	}
}