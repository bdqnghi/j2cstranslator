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

    public abstract class Handler
    {
        private Formatter theFormatter;
        private string theEncodingName;
        private Filter theFilter;
        private Level theLevel;
        private ErrorManager errorManager;
        private static readonly byte[] bytes = new byte[] { 98 };

        internal Handler()
            : base()
        {
            errorManager = new ErrorManager();
            theLevel = Level.ALL;
        }

        public abstract void Close();

        public abstract void Flush();

        public ErrorManager GetErrorManager()
        {
            return errorManager;
        }

        public void SetErrorManager(ErrorManager errormanager)
        {
            errormanager.GetHashCode();
            errorManager = errormanager;
        }

        public void ReportError(string str, Exception exception, int i)
        {
            errorManager.Error(str, exception, i);
        }

        public string GetEncoding()
        {
            return theEncodingName;
        }

        public Filter GetFilter()
        {
            return theFilter;
        }

        public Formatter GetFormatter()
        {
            return theFormatter;
        }

        public Level GetLevel()
        {
            return theLevel;
        }

        public virtual bool IsLoggable(LogRecord logrecord)
        {
            if (theLevel.IntValue() != Level.OFF.IntValue()
                && logrecord.GetLevel().IntValue() >= theLevel.IntValue()
                && (theFilter == null || theFilter.IsLoggable(logrecord)))
                return true;
            return false;
        }

        public abstract void Publish(LogRecord logrecord);

        public virtual void SetEncoding(string str)
        {
            this.SetEncoding(str, true);
        }

        internal void SetEncoding(string str, bool flag)
        {
        }

        internal void SetKnownEncoding(string str)
        {
            theEncodingName = str;
        }

        internal void ClearEncoding()
        {
            theEncodingName = null;
        }

        public void SetFilter(Filter filter)
        {
            theFilter = filter;
        }

        public void SetFormatter(Formatter formatter)
        {
            theFormatter = formatter;
        }

        public void SetLevel(Level level)
        {
            theLevel = level;
        }

    }

}
