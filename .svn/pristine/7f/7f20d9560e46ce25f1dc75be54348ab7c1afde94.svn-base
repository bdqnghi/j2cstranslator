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
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using ILOG.J2CsMapping.XML.Sax;

namespace ILOG.J2CsMapping.XML
{
    /// <summary>
    /// 
    /// </summary>
    public class XmlDocumentBuilder
    {
        DefaultHandler ihandler;
        XmlDocumentBuilderFactory dbf;

        public XmlDocumentBuilder(XmlDocumentBuilderFactory xml_dbf)
        {
            dbf = xml_dbf;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public XmlDocument Parse(FileInfo file)
        {
            return Parse(file.FullName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public XmlDocument Parse(String uri)
        {
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.ProhibitDtd = false;
            XmlReader reader = XmlReader.Create(uri, settings);
            return Parse(reader);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public XmlDocument Parse(XmlReader reader)
        {
            XmlDocument doc = new XmlDocument();

            ArrayList[] schemas = dbf.GetAttributes();
            if (schemas != null && schemas[0] != null)
            {
                for (int i = 0; i < schemas[0].Count; i++)
                {
                    String schema_uri = (String)schemas[1][i];
                    if (schema_uri.IndexOf(".xsd") == -1)
                        schema_uri += ".xsd";

                    XmlTextReader schema_reader = new XmlTextReader(schema_uri);
                    try
                    {
                        XmlSchema schema = XmlSchema.Read(schema_reader, null);
                        doc.Schemas.Add(schema);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Failed to add schema definition (XSD):" + e);
                    }
                }
            }

            doc.Load(reader);

            if (dbf.GetValidating())
            {
                //  Need to validate XML...
                //  
                ValidationEventHandler eventHandler = new ValidationEventHandler(ValidationEventHandler);
                try
                {
                    doc.Validate(eventHandler);
                }
                catch (XmlSchemaException e)
                {
                    if (ihandler != null)
                    {
                        SAXParseException saxe = new SAXParseException("SAXParseException parsing " + reader.BaseURI + " - " + e.Message, e);
                        saxe.LineNumber = e.LineNumber;
                        saxe.ColumnNumber = e.LinePosition;
                    }
                }
            }

            return doc;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="readerStream"></param>
        /// <returns></returns>
        public XmlDocument Parse( Stream readerStream ) {
            XmlDocument doc = new XmlDocument();

            ArrayList[] schemas = dbf.GetAttributes();
            if (schemas != null && schemas[0] != null) {
                for (int i = 0; i < schemas[0].Count; i++) {
                    String schema_uri = (String)schemas[1][i];
                    if (schema_uri.IndexOf(".xsd") == -1)
                        schema_uri += ".xsd";

                    XmlTextReader schema_reader = new XmlTextReader(schema_uri);
                    try {
                        XmlSchema schema = XmlSchema.Read(schema_reader, null);
                        doc.Schemas.Add(schema);
                    } catch (Exception e) {
                        Console.WriteLine("Failed to add schema definition (XSD):" + e);
                    }
                }
            }

            doc.Load(readerStream);

            if (dbf.GetValidating()) {
                //  Need to validate XML...
                //  
                ValidationEventHandler eventHandler = new ValidationEventHandler(ValidationEventHandler);
                try {
                    doc.Validate(eventHandler);
                } catch (XmlSchemaException e) {
                    if (ihandler != null) {
                        SAXParseException saxe = new SAXParseException("SAXParseException parsing " + readerStream + " - " + e.Message, e);
                        saxe.LineNumber = e.LineNumber;
                        saxe.ColumnNumber = e.LinePosition;
                    }
                }
            }

            return doc;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="handler"></param>
        public void SetErrorHandler(DefaultHandler handler)
        {
            ihandler = handler;
        }

        private static void ValidationEventHandler(object sender, ValidationEventArgs e)
        {
            throw e.Exception;
        }
    }
}
