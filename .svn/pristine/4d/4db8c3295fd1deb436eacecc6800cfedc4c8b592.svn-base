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
    public interface XmlHandler
    {
        void StartDocument();

        void EndDocument();

        object ResolveEntity(string str, string arg);

        void StartExternalEntity(string str);

        void EndExternalEntity(string str);

        void DoctypeDecl(string str, string arg, string str0);

        void Attribute(string str, string arg, bool flag);

        void StartElement(string str);

        void EndElement(string str);

        void CharData(char[] cs, int i, int i0);

        void IgnorableWhitespace(char[] cs, int i, int i0);

        void ProcessingInstruction(string str, string arg);

        void Error(string str, string arg, int i, int i0);
    }
}
