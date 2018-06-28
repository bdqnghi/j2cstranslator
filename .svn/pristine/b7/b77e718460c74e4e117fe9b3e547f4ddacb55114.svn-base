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

namespace ILOG.J2CsMapping.XML.Sax
{
    using System.Collections;

    public class TokenDictionary
    {
        private IDictionary table;
        private ArrayList types;

        public TokenDictionary()
            : base()
        {
            table = new Hashtable();
            types = new ArrayList();
        }

        public string Intern(string name)
        {
            name = string.Intern(name);
            table[name] = name;
            return name;
        }

        public string InternType(string name)
        {
            name = string.Intern(name);
            table[name] = name;
            types.Add(name);
            return name;
        }

        public string Get(string name)
        {
            object o = table[name];
            return (string)o;
        }

        public bool IsType(string name)
        {
            return types.Contains(name);
        }

    }
}
