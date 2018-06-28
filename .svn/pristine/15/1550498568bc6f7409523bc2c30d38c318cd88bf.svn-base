// 
// J2CsMapping : runtime library for J2CsTranslator
// 
// Copyright (c) 2008-2010 Alexandre FAU.
// All rights reserved. This program and the accompanying materials
// are made available under the terms of the Eclipse Public License v1.0
// which accompanies this distribution, and is available at
// http://www.eclipse.org/legal/epl-v10.html
// Contributors:
//   Alexandre FAU (IBM)
//

using System;
using System.IO;

namespace ILOG.J2CsMapping.IO
{
    /// <summary>
    /// .NET Replacement for Java CharArrayReader
    /// 
    /// CharArrayReader is used as a buffered character input stream on a character
    /// array.
    /// </summary>
    /// ToBeChecked
    public class CharArrayReader : TextReader
    {
        protected Object sync;

        /// <summary>
        /// Current buffer position.
        /// </summary>
        protected int position;

        /// <summary>
        /// Current mark position.
        /// </summary>
        protected int curMarkedPos = -1;

        /// <summary>
        /// Buffer for characters
        /// </summary>
        protected char[] buffer;

        /// <summary>
        /// The ending index of the buffer.
        /// </summary>
        protected int length;

        /// <summary>
        /// Construct a CharArrayReader on the char array <code>buffer</code>. The
        /// size of the reader is set to the <code>Length</code> of the buffer
        /// and the Object to synchro
        /// </summary>
        /// <param name="buffer">the char array to filter reads on.</param>
        public CharArrayReader(char[] buffer)
        {
            this.buffer = buffer;
            this.length = buffer.Length;
            sync = buffer;
        }

        /// <summary>
        /// Construct a CharArrayReader on the char array <code>buffer</code>. The
        /// size of the reader is set to the parameter <code>Length</code> and
        /// the original offset is set to <code>offset</code>.
        /// </summary>
        /// <param name="buffer">the char array to filter reads on.</param>
        /// <param name="offset">the offset in <code>buf</code> to start streaming at.</param>
        /// <param name="length">the number of characters available to stream over.</param>
        public CharArrayReader(char[] buffer, int offset, int length)
            : this(buffer)
        {
            if (offset < 0 || offset > buffer.Length || length < 0)
            {
                throw new ArgumentException();
            }
            this.buffer = buffer;
            this.position = offset;
            this.curMarkedPos = offset;

            // This is according to spec
            this.length = this.position + length < buffer.Length ? length : buffer.Length;
        }

        /// <summary>
        /// This method closes this CharArrayReader. Once it is closed, you can no
        /// longer read from it. Only the first invocation of this method has any
        /// effect.
        /// </summary>
        public override void Close()
        {
            lock (sync)
            {
                if (IsOpen)
                {
                    buffer = null;
                }
            }
        }

        /// <summary>
        /// Answer a boolean indicating whether or not this CharArrayReader is open.
        /// </summary>
        /// <returns><code>true</code> if the reader is open, <code>false</code> otherwise.</returns>
        private bool IsOpen
        {
            get { return buffer != null; }
        }

        /// <summary>
        /// Answer a boolean indicating whether or not this CharArrayReader is
        /// closed.
        /// </summary>
        /// <returns><code>true</code> if the reader is closed, <code>false</code> otherwise.</returns>
        private bool IsClosed
        {
            get { return buffer == null; }
        }

        //
        // Mark
        //

        /// <summary>
        /// Set a Mark position in this Reader. The parameter <code>limit</code>
        /// is ignored for CharArrayReaders. Sending Reset() will reposition the
        /// reader back to the marked position provided the mark has not been
        /// invalidated.
        /// </summary>
        /// <param name="limit">ignored for CharArrayReaders.</param>
        public void Mark(int limit)
        {
            lock (sync)
            {
                if (IsClosed)
                {
                    throw new IOException();
                }
                curMarkedPos = position;
            }
        }

        /// <summary>
        /// Answers a boolean indicating whether or not this CharArrayReader supports
        /// Mark() and Reset(). This method always returns true.
        /// </summary>
        /// <returns>indicates whether or not Mark() and Reset() are supported.</returns>
        public bool MarkSupported()
        {
            return true;
        }

        //
        // Read
        //

        /// <summary>
        /// Reads a single character from this CharArrayReader and returns the result
        /// as an int. The 2 higher-order bytes are set to 0. If the end of reader
        /// was encountered then return -1.
        /// </summary>
        /// <returns>int the character read or -1 if end of reader.</returns>
        public override int Read()
        {
            lock (sync)
            {
                if (IsClosed)
                {
                    throw new IOException("Reader is closed.");
                }
                if (position == length)
                {
                    return -1;
                }
                return buffer[position++];
            }
        }

        /// <summary>
        /// Reads at most <code>count</code> characters from this CharArrayReader
        /// and stores them at <code>offset</code> in the character array
        /// <code>buffer</code>. Returns the number of characters actually read or -1
        /// if the end of reader was encountered.
        /// </summary>
        /// <param name="buffer">character array to store the read characters</param>
        /// <param name="offset">offset in buf to store the read characters</param>
        /// <param name="count">maximum number of characters to read</param>
        /// <returns>number of characters read or -1 if end of reader.</returns>
        public override int Read(char[] buffer, int offset, int count)
        {
            // avoid int overflow
            if (offset < 0 || offset > buffer.Length || count < 0
                    || count > buffer.Length - offset)
            {
                throw new ArgumentOutOfRangeException();
            }
            lock (sync)
            {
                if (IsClosed)
                {
                    throw new IOException();
                }
                if (position < this.length)
                {
                    int bytesRead = position + count > this.length ? this.length - position : count;
                    System.Array.Copy(this.buffer, position, buffer, offset, bytesRead);
                    position += bytesRead;
                    return bytesRead;
                }
                return -1;
            }
        }

        /// <summary>
        /// Answers a <code>bool</code> indicating whether or not this
        /// CharArrayReader is ready to be read without blocking. If the result is
        /// <code>true</code>, the next <code>Read()</code> will not block. If
        /// the result is <code>false</code> this Reader may or may not block when
        /// <code>Read()</code> is sent. The implementation in CharArrayReader
        /// always returns <code>true</code> even when it has been closed.
        /// </summary>
        /// <returns><code>true</code> if the receiver will not block when
        ///         <code>Read()</code> is called, <code>false</code> if unknown
        ///         or blocking will occur.</returns>
        public bool Ready()
        {
            lock (sync)
            {
                if (IsClosed)
                {
                    throw new IOException("Reader is closed.");
                }
                return position != length;
            }
        }

        /// <summary>
        /// Reset this CharArrayReader's position to the last <code>Mark()</code>
        /// location. Invocations of <code>Read()/Skip()</code> will occur from
        /// this new location. If this Reader was not marked, the CharArrayReader is
        /// reset to the beginning of the String.
        /// </summary>
        public void Reset()
        {
            lock (sync)
            {
                if (IsClosed)
                {
                    throw new IOException("Reader is closed.");
                }
                position = curMarkedPos != -1 ? curMarkedPos : 0;
            }
        }

        /// <summary>
        /// Skips <code>count</code> number of characters in this CharArrayReader.
        /// Subsequent <code>Read()</code>'s will not return these characters
        /// unless <code>Reset()</code> is used.
        /// </summary>
        /// <param name="numberToSkip">The number of characters to skip.</param>
        /// <returns>The number of characters actually skipped.</returns>
        public long Skip(long numberToSkip)
        {
            lock (sync)
            {
                if (IsClosed)
                {
                    throw new IOException("Reader is closed.");
                }
                if (numberToSkip <= 0)
                {
                    return 0;
                }
                long skipped = 0;
                if (numberToSkip < this.length - position)
                {
                    position = position + (int)numberToSkip;
                    skipped = numberToSkip;
                }
                else
                {
                    skipped = this.length - position;
                    position = this.length;
                }
                return skipped;
            }
        }
    }
}
