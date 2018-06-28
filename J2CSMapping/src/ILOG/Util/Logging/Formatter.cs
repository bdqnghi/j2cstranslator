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

namespace ILOG.J2CsMapping.Util.Logging
{

    using System;
    using System.Resources;

    public abstract class Formatter
    {
        internal Formatter()
            : base()
        {
        }

        public abstract string Format(LogRecord logrecord);

        public string FormatMessage(LogRecord logrecord)
        {
            return "";
        }

        public virtual string GetHead(Handler handler)
        {
            return "";
        }

        public virtual string GetTail(Handler handler)
        {
            return "";
        }

    }

}
