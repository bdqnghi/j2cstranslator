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
using System.Collections;
using System.IO;
using System.Xml;
using ILOG.J2CsMapping.XML.Sax;

namespace ILOG.J2CsMapping.XML.Sax
{
    /// <summary>
    ///   The SaxParser class build a SAX push model
    ///   from the pull model found in the XmlTextReader.
    /// </summary>
    /// 
    public class SaxParser : ISaxParser
    {
        private IContentHandler handler = null;
        private IErrorHandler errorHandler = null;

        public IErrorHandler ErrorHandler
        {
            get { return errorHandler; }

            set { errorHandler = value; }
        }

        public IContentHandler Handler
        {
            get { return handler; }

            set { handler = value; }
        }

        public virtual void Parse(String text)
        {
            throw new NotImplementedException();
        }

        public virtual void Parse(TextReader text)
        {
            Stack nsstack = new Stack();
            Locator locator = new Locator();
            SAXParseException saxException = new SAXParseException();
            Attributes atts = new Attributes();
            XmlTextReader reader = null;
            try
            {
                reader = new XmlTextReader(text);
                object nsuri = reader.NameTable.Add(
                    "http://www.w3.org/2000/xmlns/");
                handler.StartDocument();
                while (reader.Read())
                {
                    string prefix = "";
                    locator.LineNumber = reader.LineNumber;
                    locator.ColumnNumber = reader.LinePosition;
                    handler.SetDocumentLocator(locator);
                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Element:
                            nsstack.Push(null); //marker
                            atts = new Attributes();
                            while (reader.MoveToNextAttribute())
                            {
                                if (reader.NamespaceURI.Equals(nsuri))
                                {
                                    prefix = "";
                                    if (reader.Prefix == "xmlns")
                                    {
                                        prefix = reader.LocalName;
                                    }
                                    nsstack.Push(prefix);
                                    handler.StartPrefixMapping(prefix, reader.Value);
                                }
                                else
                                {
                                    atts.AddAttribute(reader.NamespaceURI, reader.Name, reader.Name, reader.GetType().ToString(), reader.Value);
                                }
                            }
                            reader.MoveToElement();
                            Handler.StartElement(reader.NamespaceURI, reader.LocalName, reader.Name, atts);
                            if (reader.IsEmptyElement)
                            {
                                handler.EndElement(reader.NamespaceURI, reader.LocalName, reader.Name);
                            }
                            break;
                        case XmlNodeType.EndElement:
                            handler.EndElement(reader.NamespaceURI, reader.LocalName, reader.Name);
                            while (prefix != null)
                            {
                                handler.EndPrefixMapping(prefix);
                                prefix = (string)nsstack.Pop();
                            }
                            break;
                        case XmlNodeType.Text:
                            handler.Characters(reader.Value.ToCharArray(), 0, reader.Value.Length);
                            break;
                        case XmlNodeType.ProcessingInstruction:
                            handler.ProcessingInstruction(reader.Name, reader.Value);
                            break;
                        case XmlNodeType.Whitespace:
                            char[] whiteSpace = reader.Value.ToCharArray();
                            handler.IgnorableWhitespace(whiteSpace, 0, 1);
                            break;
                        case XmlNodeType.Entity:
                            handler.SkippedEntity(reader.Name);
                            break;
                    }
                } //While
                handler.EndDocument();
            } //try
            catch (Exception exception)
            {
                saxException.LineNumber = reader.LineNumber;
                saxException.SystemID = "";
                saxException.setMessage(exception.GetBaseException().ToString());
                errorHandler.Error(saxException);
            }
            finally
            {
                if (reader.ReadState != ReadState.Closed)
                {
                    reader.Close();
                }
            }
        } //parse()

        public void SetFeature(string s, bool b)
        {
            //##################to be implemented
            //throw new NotImplementedException();
        }


        public void SetProperty(string s, object schemaLanguage)
        {
            //##################to be implemented
            // throw new Exception("The method or operation is not implemented.");
        }
    }
}