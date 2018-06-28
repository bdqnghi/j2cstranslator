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
using System.Web;
//using System.Web;

namespace ILOG.J2CsMapping.Net
{
    /// <summary>
    /// Utility method for url encoding
    /// </summary>
    public class URLEncoding
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="encoder_name"></param>
        /// <returns></returns>
        public static String UrlEncode(String input, String encoder_name)
        {
            String intermediate_result;
            String result = "";
            Encoding enc = EncoderFromName(encoder_name);

            //  Base encoding...
            //
            intermediate_result = HttpUtility.UrlEncode(input, enc);

            //  Additional processing to put all HEX codes in uppercase (ex: %ac%9e will become %AC%9E)
            //
            int i = 0;
            while (i < intermediate_result.Length)
            {
                if (intermediate_result[i] == '%' && i + 2 < intermediate_result.Length)
                {
                    //  Append '%' as-is
                    //
                    result += intermediate_result[i++];

                    //  Append first digit, transformed if a lowercase alpha
                    //
                    if (intermediate_result[i] >= 'a' && intermediate_result[i] <= 'z')
                        result += (char)((int)intermediate_result[i++] - 32);
                    else
                        result += intermediate_result[i++];

                    //  Append second digit, transformed if a lowercase alpha
                    //
                    if (intermediate_result[i] >= 'a' && intermediate_result[i] <= 'z')
                        result += (char)((int)intermediate_result[i++] - 32);
                    else
                        result += intermediate_result[i++];
                }
                else
                    //  Append characters as-is
                    //
                    result += intermediate_result[i++];
            }

            //  Return new results
            //
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="encoder_name"></param>
        /// <returns></returns>
        public static String UrlDecode(String input, String encoder_name)
        {
            String result;
            Encoding enc = EncoderFromName(encoder_name);

            //  Base decoding, no need for further processing...
            //
            result = HttpUtility.UrlDecode(input, enc);

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="charsetName"></param>
        /// <returns></returns>
        public static Encoding EncoderFromName(String charsetName)
        {
            //  Ensure encoding/decoding fails if unrecognized character are present.
            //
            //  Fall-back objects do not work for some reasons, they have to be used on encoder/decoder objects (see CharBuffer & ByteBuffer classes)
            //
            return System.Text.Encoding.GetEncoding(charsetName, new EncoderFB(), new DecoderFB());
        }
    }

    /// <summary>
    /// 
    /// </summary>
   class EncoderFB : EncoderFallback
    {
        /// <summary>
        /// 
        /// </summary>
        public override int MaxCharCount
        {
            get { throw new ArgumentException(); }
        }
        /// <summary>
        /// 
        /// </summary>
        public override EncoderFallbackBuffer CreateFallbackBuffer()
        {
            throw new ArgumentException();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    class DecoderFB : DecoderFallback
    {
        /// <summary>
        /// 
        /// </summary>
        public override int MaxCharCount
        {
            get { throw new ArgumentException(); }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override DecoderFallbackBuffer CreateFallbackBuffer()
        {
            throw new ArgumentException();
        }
    }
}
