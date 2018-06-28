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
using System.Collections;
using System.Collections.Generic;

namespace ILOG.J2CsMapping.XML
{
    /// <summary>
    /// 
    /// </summary>
    public class XmlDocumentBuilderFactory
    {
        private bool NamespaceAware = false;
        private bool Validating = false;
        private ArrayList attributes = null;
        private ArrayList values = null;
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static XmlDocumentBuilderFactory NewInstance()
        {
            return new XmlDocumentBuilderFactory();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public XmlDocumentBuilder NewDocumentBuilder()
        {
            return new XmlDocumentBuilder( this );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool GetNamespaceAware()
        {
            return NamespaceAware;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aware"></param>
        public void SetNamespaceAware( bool aware)
        {
            NamespaceAware = aware;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool GetValidating()
        {
            return Validating;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="validate"></param>
        public void SetValidating( bool validate)
        {
            Validating = validate;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attribute"></param>
        /// <param name="uri"></param>
        public void SetAttribute(String attribute, string uri)
        {
            InitAttributes();
            attributes.Add(attribute);
            values.Add(uri);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ArrayList[] GetAttributes()
        {
            ArrayList[] lists = new ArrayList[2];
            lists[0] = attributes;
            lists[1] = values;

            return lists;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attribute"></param>
        /// <param name="file"></param>
        public void SetAttribute(String attribute, FileInfo file)
        {
            SetAttribute(attribute, file.FullName);
        }

        /// <summary>
        /// 
        /// </summary>
        private void InitAttributes()
        {
            if (attributes == null)
            {
                attributes = new ArrayList();
                values = new ArrayList();
            }
        }
    }
}
