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
using System.Collections.Generic;
using System.Text;

namespace ILOG.J2CsMapping.IO
{
    /**
     * DataInput is an interface which declares methods for reading in typed data
     * from a Stream. Typically, this stream has been written by a class which
     * implements DataOutput. Types that can be read include byte, 16-bit short,
     * 32-bit int, 32-bit float, 64-bit long, 64-bit double, byte strings, and UTF
     * Strings.
     */
    public interface DataInput
    {

        /**
         * Reads a boolean from this stream.
         * 
         * @return the next boolean value from the source stream.
         * 
         * @throws IOException
         *             If a problem occurs reading from this stream.
         * 
         * @see DataOutput#writeBoolean(boolean)
         */
        bool ReadBoolean();

        /**
         * Reads an 8-bit byte value from this stream.
         * 
         * @return the next byte value from the source stream.
         * 
         * @throws IOException
         *             If a problem occurs reading from this stream.
         * 
         * @see DataOutput#writeByte(int)
         */
        sbyte ReadByte();

        /**
         * Reads a 16-bit character value from this stream.
         * 
         * @return the next <code>char</code> value from the source stream.
         * 
         * @throws IOException
         *             If a problem occurs reading from this stream.
         * 
         * @see DataOutput#writeChar(int)
         */
        char ReadChar();

        /**
         * Reads a 64-bit <code>double</code> value from this stream.
         * 
         * @return the next <code>double</code> value from the source stream.
         * 
         * @throws IOException
         *             If a problem occurs reading from this stream.
         * 
         * @see DataOutput#writeDouble(double)
         */
        double ReadDouble();

        /**
         * Reads a 32-bit <code>float</code> value from this stream.
         * 
         * @return the next <code>float</code> value from the source stream.
         * 
         * @throws IOException
         *             If a problem occurs reading from this stream.
         * 
         * @see DataOutput#writeFloat(float)
         */
        float ReadFloat();

        /**
         * Reads bytes from this stream into the byte array <code>buffer</code>.
         * This method will block until <code>buffer.length</code> number of bytes
         * have been read.
         * 
         * @param buffer
         *            the buffer to read bytes into
         * 
         * @throws IOException
         *             If a problem occurs reading from this stream.
         * 
         * @see DataOutput#write(byte[])
         * @see DataOutput#write(byte[], int, int)
         */
        void ReadFully(byte[] buffer);

        /**
         * Read bytes from this stream and stores them in byte array
         * <code>buffer</code> starting at offset <code>offset</code>. This
         * method blocks until <code>count</code> number of bytes have been read.
         * 
         * @param buffer
         *            the byte array in which to store the read bytes.
         * @param offset
         *            the offset in <code>buffer</code> to store the read bytes.
         * @param count
         *            the maximum number of bytes to store in <code>buffer</code>.
         * 
         * @throws IOException
         *             If a problem occurs reading from this stream.
         * 
         * @see DataOutput#write(byte[])
         * @see DataOutput#write(byte[], int, int)
         */
        void ReadFully(byte[] buffer, int offset, int count);

        /**
         * Reads a 32-bit integer value from this stream.
         * 
         * @return the next <code>int</code> value from the source stream.
         * 
         * @throws IOException
         *             If a problem occurs reading from this stream.
         * 
         * @see DataOutput#writeInt(int)
         */
        int ReadInt();

        /**
         * Answers a <code>String</code> representing the next line of text
         * available in this BufferedReader. A line is represented by 0 or more
         * characters followed by <code>'\n'</code>, <code>'\r'</code>,
         * <code>"\n\r"</code> or end of stream. The <code>String</code> does
         * not include the newline sequence.
         * 
         * @return the contents of the line or null if no characters were read
         *         before end of stream.
         * 
         * @throws IOException
         *             If a problem occurs reading from this stream.
         */
        String ReadLine();

        /**
         * Reads a 64-bit <code>long</code> value from this stream.
         * 
         * @return the next <code>long</code> value from the source stream.
         * 
         * @throws IOException
         *             If a problem occurs reading from this stream.
         * 
         * @see DataOutput#writeLong(long)
         */
        long ReadLong();

        /**
         * Reads a 16-bit <code>short</code> value from this stream.
         * 
         * @return the next <code>short</code> value from the source stream.
         * 
         * @throws IOException
         *             If a problem occurs reading from this stream.
         * 
         * @see DataOutput#writeShort(int)
         */
        short ReadShort();

        /**
         * Reads an unsigned 8-bit <code>byte</code> value from this stream and
         * returns it as an int.
         * 
         * @return the next unsigned byte value from the source stream.
         * 
         * @throws IOException
         *             If a problem occurs reading from this stream.
         * 
         * @see DataOutput#writeByte(int)
         */
        int ReadUnsignedByte();

        /**
         * Reads a 16-bit unsigned <code>short</code> value from this stream and
         * returns it as an int.
         * 
         * @return the next unsigned <code>short</code> value from the source
         *         stream.
         * 
         * @throws IOException
         *             If a problem occurs reading from this stream.
         * 
         * @see DataOutput#writeShort(int)
         */
        int ReadUnsignedShort();

        /**
         * Reads a UTF format String from this Stream.
         * 
         * @return the next UTF String from the source stream.
         * 
         * @throws IOException
         *             If a problem occurs reading from this stream.
         * 
         * @see DataOutput#writeUTF(java.lang.String)
         */
        String ReadUTF();

        /**
         * Skips <code>count</code> number of bytes in this stream. Subsequent
         * <code>read()</code>'s will not return these bytes unless
         * <code>reset()</code> is used.
         * 
         * @param count
         *            the number of bytes to skip.
         * @return the number of bytes actually skipped.
         * 
         * @throws IOException
         *             If a problem occurs reading from this stream.
         */
        int SkipBytes(int count);
    }
}
