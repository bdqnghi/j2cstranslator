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
using System.Collections.Generic;
using System.Text;

namespace ILOG.J2CsMapping.Text
{
    public class RegExUtil
    {

        public static String[] Split(String expr, String regex)
        {
            return Split(expr, regex, 0);
        }

        public static String[] Split(String expr, String regex, int limit)
        {
            // TODO: In C# limit CAN'T BE less than zero !
            return Pattern.Compile(regex).Split(expr, limit);
        }
    }
}
