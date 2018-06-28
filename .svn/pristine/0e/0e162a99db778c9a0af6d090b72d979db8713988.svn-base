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

#if UNIT_TESTS
	public
#else
    internal
#endif
 class LoggingPermission
    {
        internal static readonly LoggingPermission PERMISSION
          = new LoggingPermission("control", null);

        public LoggingPermission(string str, string arg)
            : base()
        {
        }

        internal static void CheckAccess()
        {
        }

    }

}
