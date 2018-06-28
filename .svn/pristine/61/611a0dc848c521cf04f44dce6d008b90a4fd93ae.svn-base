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
    /// .NET CharBuffer replacement for java class java.nio.CharBuffer
    /// </summary>
    public class CharBuffer : Buffer
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static CharBuffer Wrap(String data)
        {
            CharBuffer bb = new CharBuffer();
            bb.internal_sb = new StringBuilder(data);

            return bb;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bb"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        /*public static CharBuffer Decode(ByteBuffer bb, Decoder d)
        {
            CharBuffer cb = new CharBuffer();

            byte[] content = bb.GetBytes();
            char[] buffer = new char[content.Length * 2];
            int bytes_used;
            int char_used;
            bool completed;
            d.Fallback = new CharDecoderFallBack();
            try
            {
                d.Convert(content, 0, content.Length, buffer, 0, buffer.Length, false, out bytes_used, out char_used, out completed);
            }
            catch (ArgumentException)
            {
                //  Catch arg exception from URLEncoding objects. DOES NOT WORK
                //
                throw new CharacterCodingException();
            }

            cb.SetChars(buffer, char_used);

            return cb;
        }*/
    }

    /*class CharDecoderFallBack : DecoderFallback
    {
        public override int MaxCharCount
        {
            get { throw new CharacterCodingException(); }
        }
        public override DecoderFallbackBuffer CreateFallbackBuffer()
        {
            throw new CharacterCodingException();
        }
    }*/
}
