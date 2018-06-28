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
     * DataInputStream is a filter class which can read typed data from a Stream.
     * Typically, this stream has been written by a DataOutputStream. Types that can
     * be read include byte, 16-bit short, 32-bit int, 32-bit float, 64-bit long,
     * 64-bit double, byte strings, and UTF Strings.
     * 
     * @see DataOutputStream
     */
    public class DataInputStream : DataInput
    {
        Stream stream;
        byte[] buff;
        long marked = -1;
        int limit = -1;

        public void Reset()
        {
            if (marked > -1)
                stream.Position = marked;
            else
                stream.Position = 0;
        }

        public void Mark(int limit)
        {
            marked = stream.Position;
            this.limit = limit;
        }

        public void Close()
        {
            stream.Close();
        }

        /**
         * Constructs a new DataInputStream on the InputStream <code>in</code>.
         * All reads can now be filtered through this stream. Note that data read by
         * this Stream is not in a human readable format and was most likely created
         * by a DataOutputStream.
         * 
         * @param in
         *            the target InputStream to filter reads on.
         * 
         * @see DataOutputStream
         * @see RandomAccessFile
         */
        public DataInputStream(Stream stream)
        {
            // super(in);
            this.stream = stream;
            buff = new byte[8];
        }

        /**
         * Reads bytes from the source stream into the byte array
         * <code>buffer</code>. The number of bytes actually read is returned.
         * 
         * @param buffer
         *            the buffer to read bytes into
         * @return the number of bytes actually read or -1 if end of stream.
         * 
         * @throws IOException
         *             If a problem occurs reading from this DataInputStream.
         * 
         * @see DataOutput#write(byte[])
         * @see DataOutput#write(byte[], int, int)
         */
        //@Override
        public int Read(byte[] buffer)
        {
            return stream.Read(buffer, 0, buffer.Length);
        }

        /**
         * Read at most <code>length</code> bytes from this DataInputStream and
         * stores them in byte array <code>buffer</code> starting at
         * <code>offset</code>. Answer the number of bytes actually read or -1 if
         * no bytes were read and end of stream was encountered.
         * 
         * @param buffer
         *            the byte array in which to store the read bytes.
         * @param offset
         *            the offset in <code>buffer</code> to store the read bytes.
         * @param length
         *            the maximum number of bytes to store in <code>buffer</code>.
         * @return the number of bytes actually read or -1 if end of stream.
         * 
         * @throws IOException
         *             If a problem occurs reading from this DataInputStream.
         * 
         * @see DataOutput#write(byte[])
         * @see DataOutput#write(byte[], int, int)
         */
        // @Override
        public int Read(byte[] buffer, int offset, int length)
        {
            return stream.Read(buffer, offset, length);
        }

        /**
         * Reads a boolean from this stream.
         * 
         * @return the next boolean value from the source stream.
         * 
         * @throws IOException
         *             If a problem occurs reading from this DataInputStream.
         * 
         * @see DataOutput#writeBoolean(boolean)
         */
        public bool ReadBoolean()
        {
            int temp = stream.ReadByte();
            if (temp < 0)
            {
                throw new EndOfStreamException();
            }
            return temp != 0;
        }

        /**
         * Reads an 8-bit byte value from this stream.
         * 
         * @return the next byte value from the source stream.
         * 
         * @throws IOException
         *             If a problem occurs reading from this DataInputStream.
         * 
         * @see DataOutput#writeByte(int)
         */
        public sbyte ReadByte()
        {
            int temp = stream.ReadByte();
            if (temp < 0)
            {
                throw new EndOfStreamException();
            }
            return (sbyte)temp;
        }

        /**
         * Reads a 16-bit character value from this stream.
         * 
         * @return the next <code>char</code> value from the source stream.
         * 
         * @throws IOException
         *             If a problem occurs reading from this DataInputStream.
         * 
         * @see DataOutput#writeChar(int)
         */
        private int ReadToBuff(int count)
        {
            int offset = 0;

            while (offset < count)
            {
                int bytesRead = stream.Read(buff, offset, count/* - offset*/);
                if (bytesRead == 0)
                    return bytesRead;
                offset += bytesRead;
            }
            return offset;
        }

        public char ReadChar()
        {
            if (ReadToBuff(2) < 0)
            {
                throw new EndOfStreamException();
            }
            return (char)(((buff[0] & 0xff) << 8) | (buff[1] & 0xff));

        }

        /**
         * Reads a 64-bit <code>double</code> value from this stream.
         * 
         * @return the next <code>double</code> value from the source stream.
         * 
         * @throws IOException
         *             If a problem occurs reading from this DataInputStream.
         * 
         * @see DataOutput#writeDouble(double)
         */
        public double ReadDouble()
        {
            return BitConverter.Int64BitsToDouble(ReadLong());
        }

        /**
         * Reads a 32-bit <code>float</code> value from this stream.
         * 
         * @return the next <code>float</code> value from the source stream.
         * 
         * @throws IOException
         *             If a problem occurs reading from this DataInputStream.
         * 
         * @see DataOutput#writeFloat(float)
         */
        public float ReadFloat()
        {
            //return BitConverter.DoubleToInt64Bits(ReadInt());
            return BitConverter.ToSingle(BitConverter.GetBytes(ReadInt()), 0);
        }

        /**
         * Reads bytes from this stream into the byte array <code>buffer</code>.
         * This method will block until <code>buffer.length</code> number of bytes
         * have been read.
         * 
         * @param buffer
         *            to read bytes into
         * 
         * @throws IOException
         *             If a problem occurs reading from this DataInputStream.
         * 
         * @see DataOutput#write(byte[])
         * @see DataOutput#write(byte[], int, int)
         */
        public void ReadFully(byte[] buffer)
        {
            ReadFully(buffer, 0, buffer.Length);
        }

        public void ReadFully(sbyte[] buffer)
        {
            ReadFully(buffer, 0, buffer.Length);
        }

        /**
         * Reads bytes from this stream and stores them in the byte array
         * <code>buffer</code> starting at the position <code>offset</code>.
         * This method blocks until <code>count</code> bytes have been read.
         * 
         * @param buffer
         *            the byte array into which the data is read
         * @param offset
         *            the offset the operation start at
         * @param length
         *            the maximum number of bytes to read
         * 
         * @throws IOException
         *             if a problem occurs while reading from this stream
         * @throws EOFException
         *             if reaches the end of the stream before enough bytes have
         *             been read
         * @see java.io.DataInput#readFully(byte[], int, int)
         */
        public void ReadFully(byte[] buffer, int offset, int length)
        {
            if (length < 0)
            {
                throw new IndexOutOfRangeException();
            }
            if (length == 0)
            {
                return;
            }
            if (stream == null)
            {
                throw new NullReferenceException("Stream is null"); //$NON-NLS-1$
            }
            if (buffer == null)
            {
                throw new NullReferenceException("buffer is null"); //$NON-NLS-1$
            }
            if (offset < 0 || offset > buffer.Length - length)
            {
                throw new IndexOutOfRangeException();
            }
            while (length > 0)
            {
                int result = stream.Read(buffer, offset, length);
                if (result == 0)
                {
                    throw new EndOfStreamException();
                }
                offset += result;
                length -= result;
            }
        }

        public void ReadFully(sbyte[] buffer, int offset, int length)
        {
            int start = offset;
            if (length < 0)
            {
                throw new IndexOutOfRangeException();
            }
            if (length == 0)
            {
                return;
            }
            if (stream == null)
            {
                throw new NullReferenceException("Stream is null"); //$NON-NLS-1$
            }
            if (buffer == null)
            {
                throw new NullReferenceException("buffer is null"); //$NON-NLS-1$
            }
            if (offset < 0 || offset > buffer.Length - length)
            {
                throw new IndexOutOfRangeException();
            }
            byte[] byteBuffer = new byte[buffer.Length];
            while (length > 0)
            {
                int result = stream.Read(byteBuffer, offset, length);
                if (result == 0)
                {
                    throw new EndOfStreamException();
                }
                offset += result;
                length -= result;
            }
            for (int i = start; i < buffer.Length; i++)
            {
                buffer[i] = (sbyte) byteBuffer[i];
            }
        }

        /**
         * Reads a 32-bit integer value from this stream.
         * 
         * @return the next <code>int</code> value from the source stream.
         * 
         * @throws IOException
         *             If a problem occurs reading from this DataInputStream.
         * 
         * @see DataOutput#writeInt(int)
         */
        public int ReadInt()
        {
            if (ReadToBuff(4) < 0)
            {
                throw new EndOfStreamException();
            }
            return ((buff[0] & 0xff) << 24) | ((buff[1] & 0xff) << 16) |
                ((buff[2] & 0xff) << 8) | (buff[3] & 0xff);
        }

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
         *             If the DataInputStream is already closed or some other IO
         *             error occurs.
         * 
         * @deprecated Use BufferedReader
         */
        public String ReadLine()
        {
            StringBuilder line = new StringBuilder(80); // Typical line length
            bool foundTerminator = false;
            while (true)
            {
                int nextByte = stream.ReadByte();
                switch (nextByte)
                {
                    case -1:
                        if (line.Length == 0 && !foundTerminator)
                        {
                            return null;
                        }
                        return line.ToString();
                    case (byte)'\r':
                        /*if (foundTerminator) {
                            ((PushbackInputStream) stream).unread(nextByte);
                            return line.ToString();
                        }
                        foundTerminator = true;
                        /* Have to be able to peek ahead one byte *
                        if (!(stream.GetType() == typeof(PushbackInputStream))) {
                            stream = new PushbackInputStream(stream);
                        }*/
                        break;
                    case (byte)'\n':
                        return line.ToString();
                    default:
                        /*if (foundTerminator) {
                            ((PushbackInputStream) stream).Unread(nextByte);
                            return line.ToString();
                        }*/
                        line.Append((char)nextByte);
                        break;
                }
            }
        }

        /**
         * Reads a 64-bit <code>long</code> value from this stream.
         * 
         * @return the next <code>long</code> value from the source stream.
         * 
         * @throws IOException
         *             If a problem occurs reading from this DataInputStream.
         * 
         * @see DataOutput#writeLong(long)
         */
        public long ReadLong()
        {
            if (ReadToBuff(8) < 0)
            {
                throw new EndOfStreamException();
            }
            int i1 = ((buff[0] & 0xff) << 24) | ((buff[1] & 0xff) << 16) |
                ((buff[2] & 0xff) << 8) | (buff[3] & 0xff);
            int i2 = ((buff[4] & 0xff) << 24) | ((buff[5] & 0xff) << 16) |
                ((buff[6] & 0xff) << 8) | (buff[7] & 0xff);

            return ((i1 & 0xffffffffL) << 32) | (i2 & 0xffffffffL);
        }

        /**
         * Reads a 16-bit <code>short</code> value from this stream.
         * 
         * @return the next <code>short</code> value from the source stream.
         * 
         * @throws IOException
         *             If a problem occurs reading from this DataInputStream.
         * 
         * @see DataOutput#writeShort(int)
         */
        public short ReadShort()
        {
            if (ReadToBuff(2) < 0)
            {
                throw new EndOfStreamException();
            }
            return (short)(((buff[0] & 0xff) << 8) | (buff[1] & 0xff));
        }

        /**
         * Reads an unsigned 8-bit <code>byte</code> value from this stream and
         * returns it as an int.
         * 
         * @return the next unsigned byte value from the source stream.
         * 
         * @throws IOException
         *             If a problem occurs reading from this DataInputStream.
         * 
         * @see DataOutput#writeByte(int)
         */
        public int ReadUnsignedByte()
        {
            int temp = stream.ReadByte();
            if (temp < 0)
            {
                throw new EndOfStreamException();
            }
            return temp;
        }

        /**
         * Reads a 16-bit unsigned <code>short</code> value from this stream and
         * returns it as an int.
         * 
         * @return the next unsigned <code>short</code> value from the source
         *         stream.
         * 
         * @throws IOException
         *             If a problem occurs reading from this DataInputStream.
         * 
         * @see DataOutput#writeShort(int)
         */
        public int ReadUnsignedShort()
        {
            if (ReadToBuff(2) < 0)
            {
                throw new EndOfStreamException();
            }
            return (char)(((buff[0] & 0xff) << 8) | (buff[1] & 0xff));
        }

        /**
         * Reads a UTF format String from this Stream.
         * 
         * @return the next UTF String from the source stream.
         * 
         * @throws IOException
         *             If a problem occurs reading from this DataInputStream.
         * 
         * @see DataOutput#writeUTF(java.lang.String)
         */
        public String ReadUTF()
        {
            return DecodeUTF(ReadUnsignedShort());
        }


        String DecodeUTF(int utfSize)
        {
            return DecodeUTF(utfSize, this);
        }

        private static String DecodeUTF(int utfSize, DataInput stream)
        {
            byte[] buf = new byte[utfSize];
            char[] outb = new char[utfSize];
            stream.ReadFully(buf, 0, utfSize);

            return ConvertUTF8WithBuf(buf, outb, 0, utfSize);
        }

        public static String ConvertUTF8WithBuf(byte[] buf, char[] outb, int offset,
            int utfSize)
        {
            int count = 0, s = 0, a;
            while (count < utfSize)
            {
                if ((outb[s] = (char)buf[offset + count++]) < '\u0080')
                    s++;
                else if (((a = outb[s]) & 0xe0) == 0xc0)
                {
                    if (count >= utfSize)
                        throw new Exception("K0062");
                    int b = buf[count++];
                    if ((b & 0xC0) != 0x80)
                        throw new Exception("K0062");
                    outb[s++] = (char)(((a & 0x1F) << 6) | (b & 0x3F));
                }
                else if ((a & 0xf0) == 0xe0)
                {
                    if (count + 1 >= utfSize)
                        throw new Exception("K0063");
                    int b = buf[count++];
                    int c = buf[count++];
                    if (((b & 0xC0) != 0x80) || ((c & 0xC0) != 0x80))
                        throw new Exception("K0064");
                    outb[s++] = (char)(((a & 0x0F) << 12) | ((b & 0x3F) << 6) | (c & 0x3F));
                }
                else
                {
                    throw new Exception("K0065");
                }
            }
            return new String(outb, 0, s);
        }

        /**
         * Reads a UTF format String from the DataInput Stream <code>in</code>.
         * 
         * @param in
         *            the input stream to read from
         * @return the next UTF String from the source stream.
         * 
         * @throws IOException
         *             If a problem occurs reading from this DataInputStream.
         * 
         * @see DataOutput#writeUTF(java.lang.String)
         */
        public static String ReadUTF(DataInput stream)
        {
            return DecodeUTF(stream.ReadUnsignedShort(), stream);
        }

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
         *             If the stream is already closed or another IOException
         *             occurs.
         */
        public int SkipBytes(int count)
        {
            int skipped = 0;
            while (skipped < count)
            {
                stream.ReadByte();
                skipped += 1;
            }
            if (skipped < 0)
            {
                throw new EndOfStreamException();
            }
            return skipped;
            //throw new NotImplementedException();
        }
    }
}
