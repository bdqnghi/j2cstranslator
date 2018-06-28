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
using ILOG.J2CsMapping.Util;

namespace ILOG.J2CsMapping.XML.Sax
{

    public class Locator : ILocator
    {
        int lineNumber;
        int columnNumber;

        public Locator()
        {
            lineNumber = 0;
            columnNumber = 0;
        }

        public virtual int LineNumber
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

        public string GetPublicId()
        {
            throw new NotImplementedException();
        }
    }
}
