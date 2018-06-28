using System;
using System.Collections.Generic;
using System.Text;
using ILOG.J2CsMapping.Util;
using System.Threading;
using System.Reflection;

namespace ILOG.J2CsMapping.Threading
{
    public class ThreadUtil
    {

        public static void NotifyAll(Object o)
        {
            Monitor.Pulse(o);
        }

        public static void Notify(Object o)
        {
            Monitor.Pulse(o);
        }

        public static void Wait(Object o)
        {
            Monitor.Wait(o);
        }

        public static void Wait(Object o, long timeout)
        {
            Monitor.Wait(o, new TimeSpan(timeout * 10000));
        }

        public static void Wait(Object o, long timeout, int nanos)
        {
            Monitor.Wait(o, new TimeSpan(timeout * 10000 + nanos * 100));
        }
    }
}
