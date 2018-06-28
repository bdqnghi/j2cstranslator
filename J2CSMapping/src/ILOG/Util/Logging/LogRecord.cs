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

    using ILOG.J2CsMapping.IO;
    using ILOG.J2CsMapping.Util;
    using System;
    using System.IO;
    using System.Resources;

    [Serializable]
    public class LogRecord : System.Runtime.Serialization.ISerializable
    {
        private static long theNextSequenceNumber = 1L;
        private Level theLevel;
        private string theMessage;
        private long theSequenceNumber;
        private long theMilliseconds;
        private int theThreadID;
        private string theLoggerName;
        private object[] theParameters;
        private ResourceManager theResourceBundle;
        private string theResourceBundleName;
        private string theSourceClassName;
        private string theSourceMethodName;
        private Exception theThrown;
        private string theThrownBacktrace;
        private bool guessed;

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.Synchronized)]
        private static long NextSequenceNumber
            ()
        {
            return ++theNextSequenceNumber;
        }

        public LogRecord(Level level, string str)
            : base()
        {
        }

        public Level GetLevel()
        {
            return theLevel;
        }

        public string GetLoggerName()
        {
            return theLoggerName;
        }

        public string GetMessage()
        {
            return theMessage;
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.Synchronized)]
        public long GetMillis
            ()
        {
            return theMilliseconds;
        }

        public object[] GetParameters()
        {
            return theParameters;
        }

        public ResourceManager GetResourceBundle()
        {
            return theResourceBundle;
        }

        public string GetResourceBundleName()
        {
            return theResourceBundleName;
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.Synchronized)]
        public long GetSequenceNumber
            ()
        {
            return theSequenceNumber;
        }

        public string GetSourceClassName()
        {
            if (theSourceClassName == null && !guessed)
                GuessClassAndMethod();
            return theSourceClassName;
        }

        public string GetSourceMethodName()
        {
            if (theSourceMethodName == null && !guessed)
                GuessClassAndMethod();
            return theSourceMethodName;
        }

        public int GetThreadID()
        {
            return theThreadID;
        }

        public Exception GetThrown()
        {
            return theThrown;
        }

        public void SetLevel(Level level)
        {
            theLevel = level;
        }

        public void SetLoggerName(string str)
        {
            theLoggerName = str;
        }

        public void SetMessage(string str)
        {
            theMessage = str;
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.Synchronized)]
        public void SetMillis
            (long l)
        {
            theMilliseconds = l;
        }

        public void SetParameters(object[] objects)
        {
            theParameters = objects;
        }

        public void SetResourceBundle(ResourceManager resourcebundle)
        {
            theResourceBundle = resourcebundle;
        }

        public void SetResourceBundleName(string str)
        {
            theResourceBundleName = str;
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.Synchronized)]
        public void SetSequenceNumber
            (long l)
        {
            theSequenceNumber = l;
        }

        public void SetSourceClassName(string str)
        {
            theSourceClassName = str;
        }

        public void SetSourceMethodName(string str)
        {
            theSourceMethodName = str;
        }

        public void SetThreadID(int i)
        {
            theThreadID = i;
        }

        public void SetThrown(Exception throwable)
        {
            theThrown = throwable;
            StringWriter stringwriter = new StringWriter();
            TextWriter printwriter = stringwriter;
            printwriter.WriteLine(throwable.StackTrace);
            printwriter.Close();
            theThrownBacktrace = stringwriter.GetStringBuilder().ToString();
        }

        internal void WriteObject(IlrSerializationStream objectoutputstream)
        {
        }

        private void GuessClassAndMethod()
        {
        }

        //
        //
        //
        public void GetObjectData(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext ctx)
        {
            //WriteObject(new IlrSerializationStream(info, ctx));
        }
    }
}
