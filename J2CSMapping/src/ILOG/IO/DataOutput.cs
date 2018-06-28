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
    public interface DataOutput
    {

        void Flush();

        /**
         * Writes the entire contents of the byte array <code>buffer</code> to the
         * OutputStream.
         * 
         * @param buffer
         *            the buffer to be written
         * 
         * @throws IOException
         *             If an error occurs attempting to write to this stream.
         * 
         * @see DataInput#readFully(byte[])
         * @see DataInput#readFully(byte[], int, int)
         */
        void Write(sbyte[] buffer);

        /**
         * Writes <code>count</code> <code>bytes</code> from the byte array
         * <code>buffer</code> starting at offset <code>index</code> to the
         * OutputStream.
         * 
         * @param buffer
         *            the buffer to be written
         * @param offset
         *            offset in buffer to get bytes
         * @param count
         *            number of bytes in buffer to write
         * 
         * @throws IOException
         *             If an error occurs attempting to write to this stream.
         * 
         * @see DataInput#readFully(byte[])
         * @see DataInput#readFully(byte[], int, int)
         */
        void Write(sbyte[] buffer, int offset, int count);
        void Write(byte[] buffer, int offset, int count);

        /**
         * Writes the specified <code>byte</code> to the OutputStream.
         * 
         * @param oneByte
         *            the byte to be written
         * 
         * @throws IOException
         *             If an error occurs attempting to write to this stream.
         * 
         * @see DataInput#readByte()
         */
        void Write(int oneByte);

        /**
         * Writes a boolean to this output stream.
         * 
         * @param val
         *            the boolean value to write to the OutputStream
         * 
         * @throws IOException
         *             If an error occurs attempting to write to this stream.
         * 
         * @see DataInput#readBoolean()
         */
        void WriteBoolean(bool val);

        /**
         * Writes a 8-bit byte to this output stream.
         * 
         * @param val
         *            the byte value to write to the OutputStream
         * 
         * @throws IOException
         *             If an error occurs attempting to write to this stream.
         * 
         * @see DataInput#readByte()
         * @see DataInput#readUnsignedByte()
         */
        void WriteByte(int val);

        /**
         * Writes the low order 8-bit bytes from a String to this output stream.
         * 
         * @param str
         *            the String containing the bytes to write to the OutputStream
         * 
         * @throws IOException
         *             If an error occurs attempting to write to this stream.
         * 
         * @see DataInput#readFully(byte[])
         * @see DataInput#readFully(byte[],int,int)
         */
        void WriteBytes(String str);

        /**
         * Writes the specified 16-bit character to the OutputStream. Only the lower
         * 2 bytes are written with the higher of the 2 bytes written first. This
         * represents the Unicode value of val.
         * 
         * @param oneByte
         *            the character to be written
         * 
         * @throws IOException
         *             If an error occurs attempting to write to this stream.
         * 
         * @see DataInput#readChar()
         */
        void WriteChar(char oneByte);

        /**
         * Writes the specified 16-bit characters contained in str to the
         * OutputStream. Only the lower 2 bytes of each character are written with
         * the higher of the 2 bytes written first. This represents the Unicode
         * value of each character in str.
         * 
         * @param str
         *            the String whose characters are to be written.
         * 
         * @throws IOException
         *             If an error occurs attempting to write to this stream.
         * 
         * @see DataInput#readChar()
         */
        void WriteChars(String str);

        /**
         * Writes a 64-bit double to this output stream. The resulting output is the
         * 8 bytes resulting from calling Double.doubleToLongBits().
         * 
         * @param val
         *            the double to be written.
         * 
         * @throws IOException
         *             If an error occurs attempting to write to this stream.
         * 
         * @see DataInput#readDouble()
         */
        void WriteDouble(double val);

        /**
         * Writes a 32-bit float to this output stream. The resulting output is the
         * 4 bytes resulting from calling Float.floatToIntBits().
         * 
         * @param val
         *            the float to be written.
         * 
         * @throws IOException
         *             If an error occurs attempting to write to this stream.
         * 
         * @see DataInput#readFloat()
         */
        void WriteFloat(float val);

        /**
         * Writes a 32-bit int to this output stream. The resulting output is the 4
         * bytes, highest order first, of val.
         * 
         * @param val
         *            the int to be written.
         * 
         * @throws IOException
         *             If an error occurs attempting to write to this stream.
         * 
         * @see DataInput#readInt()
         */
        void WriteInt(int val);

        /**
         * Writes a 64-bit long to this output stream. The resulting output is the 8
         * bytes, highest order first, of val.
         * 
         * @param val
         *            the long to be written.
         * 
         * @throws IOException
         *             If an error occurs attempting to write to this stream.
         * 
         * @see DataInput#readLong()
         */
        void WriteLong(long val);

        /**
         * Writes the specified 16-bit short to the OutputStream. Only the lower 2
         * bytes are written with the higher of the 2 bytes written first.
         * 
         * @param val
         *            the short to be written
         * 
         * @throws IOException
         *             If an error occurs attempting to write to this stream.
         * 
         * @see DataInput#readShort()
         * @see DataInput#readUnsignedShort()
         */
        void WriteShort(int val);

        /**
         * Writes the specified String out in UTF format.
         * 
         * @param str
         *            the String to be written in UTF format.
         * 
         * @throws IOException
         *             If an error occurs attempting to write to this stream.
         * 
         * @see DataInput#readUTF()
         */
        void WriteUTF(String str);
    }
}
