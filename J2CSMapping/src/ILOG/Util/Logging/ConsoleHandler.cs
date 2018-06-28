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
    using System.IO;

    public class ConsoleHandler : StreamHandler
    {

        public ConsoleHandler()
        {
        }

        public override void Close()
        {
            try
            {
                this.AssureHeadWritten();
                this.AssureTailWritten();
                this.Flush();
            }
            catch (IOException ioexception)
            {
                this.ReportError("Problems closing.", ioexception, 0);
            }
        }

        public override void Publish(LogRecord logrecord)
        {
            base.Publish(logrecord);
            this.Flush();
        }

    }

}
