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

namespace ILOG.J2CsMapping.NIO
{
    /// <summary>
    /// .NET replacement for java class java.nio.Buffer
    /// </summary>
    public class Buffer
    {
        /// <summary>
        /// 
        /// </summary>
        protected StringBuilder internal_sb;

        /// <summary>
        /// 
        /// </summary>
        protected int cursor_position;

        /// <summary>
        /// 
        /// </summary>
        public Buffer()
        {
            internal_sb = new StringBuilder(50);
            cursor_position = 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="length"></param>
        public void SetChars(char[] buffer, int length)
        {
            internal_sb = new StringBuilder(length);
            internal_sb.Append(buffer, 0, length);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="length"></param>
        public void SetBytes(byte[] buffer, int length)
        {
            internal_sb = new StringBuilder(length);
            for (int i = 0; i < length; i++)
                internal_sb.Append((char)buffer[i]);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public byte[] GetBytes()
        {
            byte[] bytes = new byte[internal_sb.Length];

            for (int i = 0; i < internal_sb.Length; i++)
                bytes[i] = (byte)internal_sb[i];

            return bytes;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public char[] GetChars()
        {
            char[] chars = new char[internal_sb.Length];

            for (int i = 0; i < internal_sb.Length; i++)
                chars[i] = internal_sb[i];

            return chars;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Rewind()
        {
            cursor_position = 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public byte Get()
        {
            if (internal_sb.Length == 0 || cursor_position == internal_sb.Length)
                throw new BufferUnderflowException();
            return (byte)internal_sb[cursor_position++];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return internal_sb.ToString();
        }
    }
}
