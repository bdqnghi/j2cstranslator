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

namespace ILOG.J2CsMapping.XML.Sax
{

    public class SAXParseException : Exception
    {
        int lineNumber = 0;
        int columnNumber = 0;
        string systemID = "";
        string message = "";
        ILocator locator;
        XMLSyntaxError e;

        public SAXParseException()
        {
        }

        public SAXParseException(string message, ILocator locator)
        {
            this.message = message;
            TheLocator = locator;
        }

        public SAXParseException(string message, ILocator locator, XMLSyntaxError e)
        {
            this.message = message;
            this.TheLocator = locator;
            this.e = e;
        }

        internal SAXParseException(string message, Object o)
        {
            this.message = message;
            if (o is Locator)
                TheLocator = (Locator)o;
        }

        public int ColumnNumber
        {
            set
            {
                columnNumber = value;
            }
            get
            {
                return columnNumber;
            }
        }

        public int LineNumber
        {
            set
            {
                lineNumber = value;
            }
            get
            {
                return lineNumber;
            }
        }

        public string SystemID
        {
            set
            {
                systemID = value;
            }
            get
            {
                return systemID;
            }
        }

        public override string Message
        {
            get
            {
                return message;
            }
        }

        public void setMessage(string message)
        {
            this.message = message;
        }

        public ILocator TheLocator
        {
            set
            {
                locator = value;
                LineNumber = value.LineNumber;
                ColumnNumber = value.ColumnNumber;
            }
        }

        public XMLSyntaxError GetException()
        {
            return e;
        }

        public int GetLineNumber()
        {
            return lineNumber;
        }

        public int GetColumnNumber()
        {
            return columnNumber;
        }

        public String GetMessage()
        {
            return Message;
        }

        public String GetPublicId()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public String GetSystemId()
        {
            return ILOG.J2CsMapping.Util.InetAddress.GetLocalHost() + " " + Message;
        }
    }
}
