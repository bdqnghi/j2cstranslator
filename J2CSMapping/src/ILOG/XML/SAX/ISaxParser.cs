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
using System.IO;

namespace ILOG.J2CsMapping.XML.Sax
{
    interface ISaxParser
    {
        IErrorHandler ErrorHandler { set; }
        IContentHandler Handler { set; }
        void Parse(TextReader text);
        void Parse(String text);
    }
}
