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
using ILOG.J2CsMapping;
using ILOG.J2CsMapping.XML;
using ILOG.J2CsMapping.Util;
using System.IO;
using System.Xml;
using System.Xml.Xsl;

namespace ILOG.J2CsMapping.XML
{
    /// <summary>
    /// 
    /// </summary>
    public class XmlTransformerFactory
    {
        public static XmlTransformerFactory NewInstance()
        {
            return new XmlTransformerFactory();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public XslCompiledTransform NewTransformer(FileInfo file)
        {
            XslCompiledTransform transformer = new XslCompiledTransform();
            transformer.Load(file.FullName);
            return transformer;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public XslCompiledTransform NewTransformer(String uri)
        {
            XslCompiledTransform transformer = new XslCompiledTransform();
            transformer.Load(uri);
            return transformer;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public XslCompiledTransform NewTransformer(XmlReader source)
        {
            XslCompiledTransform transformer = new XslCompiledTransform();
            transformer.Load(source);
            return transformer;
        }
    }
}
