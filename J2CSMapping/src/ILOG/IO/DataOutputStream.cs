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
using System.IO;

namespace ILOG.J2CsMapping.IO
{

    /**
     * DataOutputStream is a filter class which can write typed data to a Stream.
     * Typically, this stream can be read in by a DataInputStream. Types that can be
     * written include byte, 16-bit short, 32-bit int, 32-bit float, 64-bit long,
     * 64-bit double, byte strings, and UTF Strings.
     * 
     * @see DataInputStream
     */
    public class DataOutputStream : DataOutput
    {

        /** The number of bytes written out so far */
        protected int written;
        sbyte[] buff;
        Stream stream;

        /**
         * Constructs a new DataOutputStream on the OutputStream <code>out</code>.
         * All writes can now be filtered through this stream. Note that data
         * written by this Stream is not in a human readable format but can be
         * reconstructed by using a DataInputStream on the resulting output.
         * 
         * @param out
         *            the target OutputStream to filter writes on.
         */
        public DataOutputStream(Stream stream)
        {
            this.stream = stream;
            buff = new sbyte[8];
        }

        /**
         * Flush this DataOutputStream to ensure all pending data is sent out to the
         * target OutputStream. This implementation flushes the target OutputStream.
         * 
         * @throws IOException
         *             If an error occurs attempting to flush this DataOutputStream.
         */

        public void Flush()
        {
            stream.Flush();
        }

        /**
         * Answers the total number of bytes written to this stream thus far.
         * 
         * @return the number of bytes written to this DataOutputStream.
         */
        public int Size()
        {
            if (written < 0)
            {
                written = Int32.MaxValue;
            }
            return written;
        }

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
         *             If an error occurs attempting to write to this
         *             DataOutputStream.
         * 
         * @see DataInput#readFully(byte[])
         * @see DataInput#readFully(byte[], int, int)
         */
        public void Write(sbyte[] buffer)
        {
            if (buffer == null)
            {
                throw new NullReferenceException("K0047"); //$NON-NLS-1$
            }
            foreach(byte b in buffer)
                stream.WriteByte(b);            
            // written += buffer.Length;
        }

        public void Write(sbyte[] buffer, int offset, int count)
        {
            if (buffer == null)
            {
                throw new NullReferenceException("K0047"); //$NON-NLS-1$
            }
            byte[] res = new byte[buffer.Length];
            for (int i = 0; i < buffer.Length; i++)
                res[i] = (byte) buffer[i];
            stream.Write(res, offset, count);
            written += count;
        }

        public void Write(byte[] buffer, int offset, int count)
        {
            if (buffer == null)
            {
                throw new NullReferenceException("K0047"); //$NON-NLS-1$
            }
            stream.Write(buffer, offset, count);
            written += count;
        }

        /**
         * Writes the specified <code>byte</code> to the OutputStream.
         * 
         * @param oneByte
         *            the byte to be written
         * 
         * @throws IOException
         *             If an error occurs attempting to write to this
         *             DataOutputStream.
         * 
         * @see DataInput#readByte()
         */

        public void Write(int oneByte)
        {
            byte val = (byte)oneByte;
            stream.WriteByte(val);
            written++;
        }

        /**
         * Writes a boolean to this output stream.
         * 
         * @param val
         *            the boolean value to write to the OutputStream
         * 
         * @throws IOException
         *             If an error occurs attempting to write to this
         *             DataOutputStream.
         * 
         * @see DataInput#readBoolean()
         */
        public void WriteBoolean(bool val)
        {
            stream.WriteByte((byte) (val ? 1 : 0));
            written++;
        }

        /**
         * Writes a 8-bit byte to this output stream.
         * 
         * @param val
         *            the byte value to write to the OutputStream
         * 
         * @throws IOException
         *             If an error occurs attempting to write to this
         *             DataOutputStream.
         * 
         * @see DataInput#readByte()
         * @see DataInput#readUnsignedByte()
         */
        public void WriteByte(int val)
        {
            stream.WriteByte( (byte) val);
            written++;
        }

        /**
         * Writes the low order 8-bit bytes from a String to this output stream.
         * 
         * @param str
         *            the String containing the bytes to write to the OutputStream
         * 
         * @throws IOException
         *             If an error occurs attempting to write to this
         *             DataOutputStream.
         * 
         * @see DataInput#readFully(byte[])
         * @see DataInput#readFully(byte[],int,int)
         */
        public void WriteBytes(String str)
        {
            if (str.Length == 0)
            {
                return;
            }
            sbyte[] bytes = new sbyte[str.Length];
            for (int index = 0; index < str.Length; index++)
            {
                bytes[index] = (sbyte)str[index];
            }
            Write(bytes); // TODO
            written += bytes.Length;
        }

        /**
         * Writes the specified 16-bit character to the OutputStream. Only the lower
         * 2 bytes are written with the higher of the 2 bytes written first. This
         * represents the Unicode value of val.
         * 
         * @param val
         *            the character to be written
         * 
         * @throws IOException
         *             If an error occurs attempting to write to this
         *             DataOutputStream.
         * 
         * @see DataInput#readChar()
         */
        public void WriteChar(char val)
        {
            buff[0] = (sbyte)(val >> 8);
            buff[1] = (sbyte)val;
            /*stream.*/Write(buff, 0, 2);
            written += 2;
        }

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
         *             If an error occurs attempting to write to this
         *             DataOutputStream.
         * 
         * @see DataInput#readChar()
         */
        public void WriteChars(String str)
        {
            sbyte[] newBytes = new sbyte[str.Length * 2];
            for (int index = 0; index < str.Length; index++)
            {
                int newIndex = index == 0 ? index : index * 2;
                newBytes[newIndex] = (sbyte)(str[index] >> 8);
                newBytes[newIndex + 1] = (sbyte)str[index];
            }
            Write(newBytes);
            written += newBytes.Length;
        }

        /**
         * Writes a 64-bit double to this output stream. The resulting output is the
         * 8 bytes resulting from calling Double.doubleToLongBits().
         * 
         * @param val
         *            the double to be written.
         * 
         * @throws IOException
         *             If an error occurs attempting to write to this
         *             DataOutputStream.
         * 
         * @see DataInput#readDouble()
         */
        public void WriteDouble(double val)
        {
            WriteLong(BitConverter.DoubleToInt64Bits(val));
        }

        /**
         * Writes a 32-bit float to this output stream. The resulting output is the
         * 4 bytes resulting from calling Float.floatToIntBits().
         * 
         * @param val
         *            the float to be written.
         * 
         * @throws IOException
         *             If an error occurs attempting to write to this
         *             DataOutputStream.
         * 
         * @see DataInput#readFloat()
         */
        public void WriteFloat(float val)
        {
            //WriteInt((int)BitConverter.DoubleToInt64Bits((int) val));
            WriteInt(BitConverter.ToInt32(BitConverter.GetBytes(val), 0));
        }

        /**
         * Writes a 32-bit int to this output stream. The resulting output is the 4
         * bytes, highest order first, of val.
         * 
         * @param val
         *            the int to be written.
         * 
         * @throws IOException
         *             If an error occurs attempting to write to this
         *             DataOutputStream.
         * 
         * @see DataInput#readInt()
         */
        public void WriteInt(int val)
        {
            buff[0] = (sbyte)(val >> 24);
            buff[1] = (sbyte)(val >> 16);
            buff[2] = (sbyte)(val >> 8);
            buff[3] = (sbyte)val;
            /*stream.*/Write(buff, 0, 4);
            written += 4;
        }

        /**
         * Writes a 64-bit long to this output stream. The resulting output is the 8
         * bytes, highest order first, of val.
         * 
         * @param val
         *            the long to be written.
         * 
         * @throws IOException
         *             If an error occurs attempting to write to this
         *             DataOutputStream.
         * 
         * @see DataInput#readLong()
         */
        public void WriteLong(long val)
        {
            buff[0] = (sbyte)(val >> 56);
            buff[1] = (sbyte)(val >> 48);
            buff[2] = (sbyte)(val >> 40);
            buff[3] = (sbyte)(val >> 32);
            buff[4] = (sbyte)(val >> 24);
            buff[5] = (sbyte)(val >> 16);
            buff[6] = (sbyte)(val >> 8);
            buff[7] = (sbyte)val;
            /*stream.*/Write(buff, 0, 8);
            written += 8;
        }

        /**
         * Writes the specified 16-bit short to the OutputStream. Only the lower 2
         * bytes are written with the higher of the 2 bytes written first.
         * 
         * @param val
         *            the short to be written
         * 
         * @throws IOException
         *             If an error occurs attempting to write to this
         *             DataOutputStream.
         * 
         * @see DataInput#readShort()
         * @see DataInput#readUnsignedShort()
         */
        public void WriteShort(int val)
        {
            buff[0] = (sbyte)(val >> 8);
            buff[1] = (sbyte)val;
            /*stream.*/Write(buff, 0, 2);
            written += 2;
        }

        /**
         * Writes the specified String out in UTF format.
         * 
         * @param str
         *            the String to be written in UTF format.
         * 
         * @throws IOException
         *             If an error occurs attempting to write to this
         *             DataOutputStream.
         * 
         * @see DataInput#readUTF()
         */
        public void WriteUTF(String str)
        {
            long utfCount = CountUTFBytes(str);
            if (utfCount > 65535)
            {
                throw new Exception("K0068"); //$NON-NLS-1$
            }
            WriteShort((int)utfCount);
            WriteUTFBytes(str, utfCount);
        }

        long CountUTFBytes(String str)
        {
            int utfCount = 0, length = str.Length;
            for (int i = 0; i < length; i++)
            {
                int charValue = str[i];
                if (charValue > 0 && charValue <= 127)
                {
                    utfCount++;
                }
                else if (charValue <= 2047)
                {
                    utfCount += 2;
                }
                else
                {
                    utfCount += 3;
                }
            }
            return utfCount;
        }

        void WriteUTFBytes(String str, long count)
        {
            int size = (int)count;
            int length = str.Length;
            sbyte[] utfBytes = new sbyte[size];
            int utfIndex = 0;
            for (int i = 0; i < length; i++)
            {
                int charValue = str[i];
                if (charValue > 0 && charValue <= 127)
                {
                    utfBytes[utfIndex++] = (sbyte)charValue;
                }
                else if (charValue <= 2047)
                {
                    utfBytes[utfIndex++] = (sbyte)(0xc0 | (0x1f & (charValue >> 6)));
                    utfBytes[utfIndex++] = (sbyte)(0x80 | (0x3f & charValue));
                }
                else
                {
                    utfBytes[utfIndex++] = (sbyte)(0xe0 | (0x0f & (charValue >> 12)));
                    utfBytes[utfIndex++] = (sbyte)(0x80 | (0x3f & (charValue >> 6)));
                    utfBytes[utfIndex++] = (sbyte)(0x80 | (0x3f & charValue));
                }
            }
            Write(utfBytes, 0, utfIndex);
        }
    }
}
