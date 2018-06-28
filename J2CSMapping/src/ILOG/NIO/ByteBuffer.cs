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
using ILOG.J2CsMapping.NIO.charset;

namespace ILOG.J2CsMapping.NIO
{
    /// <summary>
    /// .NET replacement for java class java.nio.ByteBuffer
    /// </summary>
    public class ByteBuffer : Buffer
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static ByteBuffer Wrap(byte[] data)
        {
            ByteBuffer bb = new ByteBuffer();

            //  TODO: Optimize string creation...
            //
            int i = 0;
            while (i < data.Length)
                bb.internal_sb.Append((char)data[i++]);

            return bb;
        }

        public static ByteBuffer Wrap(sbyte[] data)
        {
            ByteBuffer bb = new ByteBuffer();

            //  TODO: Optimize string creation...
            //
            int i = 0;
            while (i < data.Length)
                bb.internal_sb.Append((char)data[i++]);

            return bb;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cb"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        public static ByteBuffer Encode(CharBuffer cb, Encoder e)
        {
            ByteBuffer bb = new ByteBuffer();

            char[] content = cb.GetChars();
            byte[] buffer = new byte[e.GetByteCount(content, 0, content.Length, true)];

            int bytes_used;
            int char_used;
            bool completed;
            e.Fallback = new ByteEncoderFallBack();
            try
            {
                e.Convert(content, 0, content.Length, buffer, 0, buffer.Length, true, out char_used, out bytes_used, out completed);
            }
            catch (ArgumentException)
            {
                //  Catch arg exception from URLEncoding objects. DOES NOT WORK
                //
                throw new CharacterCodingException();
            }

            bb.SetBytes(buffer, bytes_used);

            return bb;
        }

        public static ByteBuffer Allocate(int p)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    class ByteEncoderFallBack : EncoderFallback
    {

        /// <summary>
        /// 
        /// </summary>
        public override int MaxCharCount
        {
            get { throw new CharacterCodingException(); }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override EncoderFallbackBuffer CreateFallbackBuffer()
        {
            throw new CharacterCodingException();
        }
    }
}
