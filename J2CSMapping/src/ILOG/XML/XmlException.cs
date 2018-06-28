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

namespace ILOG.J2CsMapping.XML
{

    /// <summary></summary>
    public class XmlException : Exception
    {
        private string message;
        private string systemId;
        private int line;
        private int column;

        public override string Message
        {
            get
            {
                return message;
            }
        }


        public XmlException(string message, string systemId, int line, int column)
            : base()
        {
            this.message = message;
            this.systemId = systemId;
            this.line = line;
            this.column = column;
        }

        /// <return>The URI as a string.</return>
        public string GetSystemId()
        {
            return systemId;
        }

        /// <return>The line number as an integer.</return>
        public int GetLine()
        {
            return line;
        }

        /// <return>The column number as an integer.</return>
        public int GetColumn()
        {
            return column;
        }

    }

}
