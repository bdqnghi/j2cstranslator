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
    using System.Reflection;
    using System.Security;
    using System.Text;

    public class StreamHandler : Handler
    {
        private static readonly string DEFAULT_LEVEL = Level.INFO.ToString();

        private static readonly byte[] bytes = new byte[] { 98 };
        private StreamWriter writer;
        private bool headHasBeenWritten;
        private bool tailHasBeenWritten;

        public StreamHandler()
            : this(null, null)
        {
        }

        public StreamHandler(Stream outputstream, Formatter formatter)
            : this(outputstream, formatter,
            null, DEFAULT_LEVEL,
            null,
            null,
            null,
            null)
        {
        }

        internal StreamHandler(Stream outputstream, Formatter formatter, string str,
            string arg, string str0, string str1, string str2,
            string str3)
            : base()
        {
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.Synchronized)]
        public override void Close()
        {
            LoggingPermission.CheckAccess();
            if (writer != null)
            {
                try
                {
                    this.AssureHeadWritten();
                    this.AssureTailWritten();
                    writer.Close();
                }
                catch (IOException ioexception)
                {
                    this.ReportError(null, ioexception, 0);
                }
                writer = null;
            }
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.Synchronized)]
        public override void Flush()
        {
            if (writer != null)
            {
                try
                {
                    writer.Flush();
                }
                catch (IOException ioexception)
                {
                    this.ReportError(null, ioexception, 1);
                }
            }
        }

        public override bool IsLoggable(LogRecord logrecord)
        {
            bool flag = writer != null && base.IsLoggable(logrecord);
            return flag;
        }

        public override void Publish(LogRecord logrecord)
        {
            if (this.IsLoggable(logrecord))
            {
                try
                {
                    this.AssureHeadWritten();
                    writer.Write(this.GetFormatter().Format(logrecord));
                }
                catch (IOException ioexception)
                {
                    this.ReportError("Unable to publish record", ioexception, 5);
                }
            }
        }

        public override void SetEncoding(string str)
        {
            this.SetEncoding(str, true);
        }

        public void SetOutputStream(Stream outputstream)
        {
        }

        internal void AssureHeadWritten()
        {
            if (!headHasBeenWritten)
            {
                if (writer != null)
                    writer.Write(this.GetFormatter().GetHead(this));
                headHasBeenWritten = true;
            }
        }

        internal void AssureTailWritten()
        {
            if (!tailHasBeenWritten)
            {
                if (writer != null)
                    writer.Write(this.GetFormatter().GetTail(this));
                tailHasBeenWritten = true;
            }
        }

    }

}
