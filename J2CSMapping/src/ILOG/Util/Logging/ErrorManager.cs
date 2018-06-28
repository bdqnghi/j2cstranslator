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

    public class ErrorManager
    {
        public const int CLOSE_FAILURE = 0;
        public const int FLUSH_FAILURE = 1;
        public const int FORMAT_FAILURE = 2;
        public const int GENERIC_FAILURE = 3;
        public const int OPEN_FAILURE = 4;
        public const int WRITE_FAILURE = 5;
        private bool firstTime;

        public ErrorManager()
            : base()
        {
            firstTime = true;
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.Synchronized)]
        public void Error
            (string str, Exception exception, int i)
        {
            if (firstTime)
            {
                firstTime = false;
                if (str != null)
                    Console.Error.WriteLine(str);
                if (exception != null)
                    Console.Error.WriteLine(exception.StackTrace);
                Console.Error.WriteLine(i);
            }
        }

    }
}
