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

    using ILOG.J2CsMapping.Collections;
    using System;
    using System.Resources;
    using System.Text;
    using System.Collections;

    public class Logger
    {
        public static readonly Logger global = Logger.GetLogger("global");
        //private Logger parent;
        private string theName;
        //private Filter theFilter;
        private Level theLevel;
        private Level effectiveLevel = new Level("", 0);
        private IList handlers;
        private string theResourceBundleName;

        public static Logger GetAnonymousLogger()
        {
            return Logger.GetAnonymousLogger(null);
        }

        public static Logger GetAnonymousLogger(string str)
        {
            Logger logger = new Logger(null, str);
            return logger;
        }

        public static Logger GetLogger(string str)
        {
            return Logger.GetLogger(str, null);
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.Synchronized)]
        public static Logger GetLogger(string str, string arg)
        {
            return new Logger(str, arg);
        }

        internal Logger(string str, string arg)
            : base()
        {
            handlers = new ArrayList();
            theName = str;
            theResourceBundleName = arg;
        }

        internal Logger()
            : base()
        {
        }

        public void SetParent(Logger logger0)
        {
        }

        public void AddHandler(Handler handler)
        {
        }

        public void Config(string str)
        {
            this.Log(Level.CONFIG, str);
        }

        public void Entering(string str, string arg)
        {
            this.Logp(Level.FINER, str, arg, "ENTRY");
        }

        public void Entering(string str, string arg, object obj)
        {
            this.Logp(Level.FINER, str, arg, "ENTRY {0}", new object[] { obj });
        }

        public void Entering(string str, string arg, object[] objects)
        {
            StringBuilder stringbuffer = new StringBuilder("ENTRY");
            for (int i = 0; i < objects.Length; i++)
            {
                stringbuffer.Append(" {");
                stringbuffer.Append(i);
                stringbuffer.Append("}");
            }
            this.Logp(Level.FINER, str, arg, stringbuffer.ToString(), objects);
        }

        public void Exiting(string str, string arg)
        {
            this.Logp(Level.FINER, str, arg, "RETURN");
        }

        public void Exiting(string str, string arg, object obj)
        {
            this.Logp(Level.FINER, str, arg, "RETURN {0}", new object[] { obj });
        }

        public void Fine(string str)
        {
            this.Log(Level.FINE, str);
        }

        public void Finer(string str)
        {
            this.Log(Level.FINER, str);
        }

        public void Finest(string str)
        {
            this.Log(Level.FINEST, str);
        }

        public Filter GetFilter()
        {
            //return theFilter;
            return null;
        }

        public Handler[] GetHandlers()
        {
            return null;
        }

        public Level GetLevel()
        {
            return null;
        }

        public string GetName()
        {
            return "";
        }

        public ResourceManager GetResourceBundle()
        {
            return null;
        }

        public string GetResourceBundleName()
        {
            return "";
        }

        public bool GetUseParentHandlers()
        {
            return true;
        }

        public void Info(string str)
        {
            this.Log(Level.INFO, str);
        }

        public bool IsLoggable(Level level)
        {
            return level.IntValue() >= this.GetEffectiveLevel().IntValue();
        }

        public void Log(Level level, string str)
        {
            if (this.IsLoggable(level))
            {
                LogRecord logrecord = new LogRecord(level, str);
                this.Log(logrecord);
            }
        }

        public void Log(Level level, string str, object obj)
        {
            if (this.IsLoggable(level))
            {
                LogRecord logrecord = new LogRecord(level, str);
                logrecord.SetParameters(new object[] { obj });
                this.Log(logrecord);
            }
        }

        public void Log(Level level, string str, object[] objects)
        {
            if (this.IsLoggable(level))
            {
                LogRecord logrecord = new LogRecord(level, str);
                logrecord.SetParameters(objects);
                this.Log(logrecord);
            }
        }

        public void Log(Level level, string str, Exception throwable)
        {
            if (this.IsLoggable(level))
            {
                LogRecord logrecord = new LogRecord(level, str);
                if (throwable != null)
                    logrecord.SetThrown(throwable);
                this.Log(logrecord);
            }
        }

        public void Log(LogRecord logrecord)
        {
        }

        public void Logp(Level level, string str, string arg, string str0)
        {
        }

        public void Logp(Level level, string str, string arg, string str0, object obj)
        {
        }

        public void Logp(Level level, string str, string arg, string str0,
            object[] objects)
        {
        }

        public void Logp(Level level, string str, string arg, string str0,
            Exception throwable)
        {
        }

        public void Logrb(Level level, string str, string arg, string str0,
            string str1)
        {
        }

        public void Logrb(Level level, string str, string arg, string str0,
            string str1, object obj)
        {
        }

        public void Logrb(Level level, string str, string arg, string str0,
            string str1, object[] objects)
        {
        }

        public void Logrb(Level level, string str, string arg, string str0,
            string str1, Exception throwable)
        {
        }

        public void RemoveHandler(Handler handler)
        {
        }

        public void SetFilter(Filter filter)
        {
        }

        public void SetLevel(Level level)
        {
        }

        internal void SetEffectiveLevel(Level level)
        {
            effectiveLevel = level;
        }

        internal Level GetEffectiveLevel()
        {
            if (theLevel != null)
                return theLevel;
            return effectiveLevel;
        }

        public void SetUseParentHandlers(bool flag)
        {
        }

        public void Severe(string str)
        {
            this.Log(Level.SEVERE, str);
        }

        public void Throwing(string str, string arg, Exception throwable)
        {
            this.Logp(Level.FINER, str, arg, "THROW", throwable);
        }

        public void Warning(string str)
        {
            this.Log(Level.WARNING, str);
        }

        internal void Reset(Level level)
        {
        }
    }
}
