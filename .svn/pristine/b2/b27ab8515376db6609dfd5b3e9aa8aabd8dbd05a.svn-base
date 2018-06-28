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
using System.Reflection;
using System.IO;

namespace ILOG.J2CsMapping.IO
{
    /// <summary>
    /// Utility method for IO
    /// </summary>
    public class IOUtility
    {

        public static StreamWriter NewStreamWriter(Stream stream, bool autoflush)
        {
            StreamWriter writer = new StreamWriter(stream);
            writer.AutoFlush = autoflush;
            return writer;
        }

        public static StreamWriter NewStreamWriter(Stream stream)
        {
            StreamWriter writer = new StreamWriter(stream);
            return writer;
        }

        public static StreamWriter NewStreamWriter(StreamWriter writer, bool autoflush)
        {
            writer.AutoFlush = autoflush;
            return writer;
        }

        public static StreamWriter NewStreamWriter(StreamWriter writer)
        {
            return writer;
        }

        public static StreamWriter NewStreamWriter(TextWriter writer, bool autoflush)
        {
            if (writer is StreamWriter)
            {
                StreamWriter s_writer = (StreamWriter) writer;
                s_writer.AutoFlush = autoflush;
                return s_writer;
            }
            if (writer is StringWriter)
            {
                throw new Exception("In .NET a StringWriter can'b be a StreamWriter");
            }
            throw new Exception("Text writer is not a stream !");
        }

        public static StreamWriter NewStreamWriter(TextWriter writer)            
        {            
            if (writer is StreamWriter)
            {
                StreamWriter s_writer = (StreamWriter)writer;
                return s_writer;
            }
            if (writer is StringWriter)
            {
                throw new Exception("In .NET a StringWriter can'b be a StreamWriter");
            }
            throw new Exception("Text writer is not a stream !");
        }




        /// <summary>
        /// 
        /// </summary>
        /// <param name="ass"></param>
        /// <param name="resourceName"></param>
        /// <returns></returns>
        public static Uri GetResource(Assembly ass, String resourceName)
        {
            String path = @"Resources\"; // TODO
            String res = (ass.GetName().Name + "." + path + resourceName).Replace(@"\", ".").Replace("/", ".");
            Stream stream = ass.GetManifestResourceStream(res);
            if (stream != null)
                return new Uri(res);
            else
                return null;
            // throw new NotImplementedException("IOUtility.GetResource(Assembly ass, String resourceName)");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ass"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        public static Stream GetResourceAsStream(Assembly ass, String file)
        {
            String path = @"src\resources\"; // TODO
            String res = (ass.GetName().Name + "." + path + file).Replace(@"\", ".").Replace("/", ".");
            Stream stream = ass.GetManifestResourceStream(res);
           //  String[] names = ass.GetManifestResourceNames();
            return stream;
        }

        public static Stream GetResourceAsStream(Assembly ass, String path, String file)
        {
            String res = (ass.GetName().Name + "." + path + file).Replace(@"\", ".").Replace("/", ".");
            Stream stream = ass.GetManifestResourceStream(res);
            //  String[] names = ass.GetManifestResourceNames();
            return stream;
        }
    }
}
