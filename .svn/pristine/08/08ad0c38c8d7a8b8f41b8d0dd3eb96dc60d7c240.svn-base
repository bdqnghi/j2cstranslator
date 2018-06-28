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
using System.Text;
using ILOG.J2CsMapping.Collections;
using ILOG.J2CsMapping.XML.Sax;

namespace ILOG.J2CsMapping.XML
{
    /// <summary>An exception thrown by object model serializers when a syntax error
    ///  is found while reading an input stream.</summary>
    public class XMLSyntaxError : Exception
    {
        private int lineno;
        private string filename;
        private IList errors;

        public XMLSyntaxError()
            : base()
        {
            lineno = -1;
            filename = null;
        }

        public XMLSyntaxError(IList errors)
            : base()
        {
            lineno = -1;
            filename = null;
            this.errors = errors;
        }

        public XMLSyntaxError(string msg, IList errors)
            : base(msg)
        {
            lineno = -1;
            filename = null;
            this.errors = errors;
        }

        public XMLSyntaxError(string msg)
            : base(msg)
        {
            lineno = -1;
            filename = null;
        }

        public XMLSyntaxError(int lineno, string msg)
            : base("line " + lineno + " : " + msg)
        {
            this.lineno = -1;
            filename = null;
            this.lineno = lineno;
        }

        public XMLSyntaxError(string filename, int lineno, string msg)
            : base("file " + filename + ", line " + lineno + " : " + msg)
        {
            this.lineno = -1;
            this.filename = null;
            this.lineno = lineno;
        }

        /// <summary>Returns the line number where the syntax error occured or
        ///  -1 if the line number is not known.</summary>
        public int GetLineNumber()
        {
            return lineno;
        }

        /// <summary>Returns the file name. This only applies when an included file contains the reported error.</summary>
        public string GetFilename()
        {
            return filename;
        }

        /// <return>The list of errors</return>
        public IList GetErrors()
        {
            if (errors == null)
                return new ArrayList();
            return errors;
        }

        /// <summary></summary>
        public string[] GetErrorMessages()
        {
            if (errors == null)
                return new string[0];
            string[] result = new string[errors.Count];
            StringBuilder buffer = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                buffer.Length = 0;
                SAXParseException e
                  = (SAXParseException)errors[i];
                if (e.LineNumber != -1)
                    buffer.Append("line " + e.LineNumber + ": ");
                buffer.Append(e.Message);
                result[i] = buffer.ToString();
            }
            return result;
        }

    }

}
