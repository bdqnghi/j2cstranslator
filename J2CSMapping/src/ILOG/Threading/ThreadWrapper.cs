using System;
using System.Collections.Generic;
using System.Text;
using ILOG.J2CsMapping.Util;
using System.Threading;
using System.Reflection;

namespace ILOG.J2CsMapping.Threading
{

    /**
     * This class must be implemented by the VM vendor. The documented methods must
     * be implemented to support other provided class implementations in this
     * package. A Thread is a unit of concurrent execution in Java. It has its own
     * call stack for methods being called and their parameters. Threads in the same
     * VM interact and synchronize by the use of shared Objects and monitors
     * associated with these objects. Synchronized methods and part of the API in
     * Object also allow Threads to cooperate. When a Java program starts executing
     * there is an implicit Thread (called "main") which is automatically created by
     * the VM. This Thread belongs to a ThreadGroup (also called "main") which is
     * automatically created by the bootstrap sequence by the VM as well.
     * 
     */
    public class ThreadWrapper : IRunnable
    {
        /**
         * <p>
         * The maximum priority value allowed for a thread.
         * </p>
         */
        public const int MAX_PRIORITY = 10;

        /**
         * <p>
         * The minimum priority value allowed for a thread.
         * </p>
         */
        public const int MIN_PRIORITY = 1;

        /**
         * <p>
         * The normal (default) priority value assigned to threads.
         * </p>
         */
        public const int NORM_PRIORITY = 5;

        private Thread thread;

        /**
         * Constructs a new Thread with no runnable object and a newly generated
         * name. The new Thread will belong to the same ThreadGroup as the Thread
         * calling this constructor.
         */
        public ThreadWrapper()
            : base()
        {
        }

        /**
         * Constructs a new Thread with a runnable object and a newly generated
         * name. The new Thread will belong to the same ThreadGroup as the Thread
         * calling this constructor.
         */
        public ThreadWrapper(Thread runnable)
            : base()
        {
            this.thread = runnable;
        }

        /**
         * Constructs a new Thread with a runnable object and name provided. The new
         * Thread will belong to the same ThreadGroup as the Thread calling this
         * constructor.
         */
        public ThreadWrapper(Thread runnable, String threadName)
            : base()
        {
            this.thread = runnable;
            this.Name = threadName;
        }

        /**
         * Constructs a new Thread with no runnable object and the name provided.
         * The new Thread will belong to the same ThreadGroup as the Thread calling
         * this constructor.
         */
        public ThreadWrapper(String threadName)
            : base()
        {
            this.Name = threadName;
        }

        /**
         * Answers the instance of Thread that corresponds to the running Thread
         * which calls this method.
         * 
         * @return a ThreadWrapper corresponding to the code that called
         *         <code>CurrentThread</code>
         */
        public static ThreadWrapper CurrentThread
        {
            get
            {
                return new ThreadWrapper( System.Threading.Thread.CurrentThread);
            }
        }

        public Thread Thread { get; set; }

        /**
         * Answers the name of the receiver.
         * 
         * @return the receiver's name (a java.lang.String)
         */
        public String Name {
            get { return thread.Name;  }
            set { thread.Name = value;  }
        }

        /**
         * Answers the priority of the receiver.
         * 
         * @return the receiver's priority (an <code>int</code>)
         */
        public ThreadPriority Priority { 
            get { return thread.Priority; } 
            set { thread.Priority = value; } 
        }

        /**
         * Posts an interrupt request to the receiver.
         * 
         * @see Thread#interrupted
         * @see Thread#isInterrupted
         */
        public void Interrupt()
        {
            thread.Interrupt();
        }

        /**
         * Answers <code>true</code> if the receiver has already been started and
         * still runs code (hasn't died yet). Answers <code>false</code> either if
         * the receiver hasn't been started yet or if it has already started and run
         * to completion and died.
         * 
         * @return a <code>bool</code>
         * @see Thread#start
         */
        public bool IsAlive
        {
            get { return thread.IsAlive; }
        }

        /**
         * Answers a <code>boolean</code> indicating whether the receiver is a
         * daemon Thread (<code>true</code>) or not (<code>false</code>) A
         * daemon Thread only runs as long as there are non-daemon Threads running.
         * When the last non-daemon Thread ends, the whole program ends no matter if
         * it had daemon Threads still running or not.
         * 
         * @return a <code>bool</code>
         */
        public bool IsDaemon
        {
            get { return thread.IsBackground; }
            set { thread.IsBackground = value; }
        }

        /**
         * Blocks the current Thread (<code>Thread.currentThread()</code>) until
         * the receiver finishes its execution and dies.
         */
        public void Join()
        {
            thread.Join();
        }

        /**
         * Blocks the current Thread (<code>Thread.currentThread()</code>) until
         * the receiver finishes its execution and dies or the specified timeout
         * expires, whatever happens first.
         * 
         * @param timeoutInMilliseconds The maximum time to wait (in milliseconds).
         */
        public void Join(long timeoutInMilliseconds)
        {
            thread.Join(new System.TimeSpan(timeoutInMilliseconds * 10000));
        }

        /**
         * Blocks the current Thread (<code>Thread.currentThread()</code>) until
         * the receiver finishes its execution and dies or the specified timeout
         * expires, whatever happens first.
         * 
         * @param timeoutInMilliseconds The maximum time to wait (in milliseconds).
         * @param nanos Extra nanosecond precision
         */
        public void Join(long timeoutInMilliseconds, int nanos)
        {
            thread.Join(new System.TimeSpan(timeoutInMilliseconds * 10000 + nanos * 100));
        }

        /**
         * This is a no-op if the receiver was never suspended, or suspended and
         * already resumed. If the receiver is suspended, however, makes it resume
         * to the point where it was when it was suspended.
         */
        public void Resume()
        {
            thread.Resume();
        }

        /**
         * Calls the <code>run()</code> method of the Runnable object the receiver
         * holds. If no Runnable is set, does nothing.
         */
        public virtual void Run()
        {
            return;
        }

        /**
         * Causes the thread which sent this message to sleep an interval of time
         * (given in milliseconds). The precision is not guaranteed - the Thread may
         * sleep more or less than requested.
         * 
         * @param time The time to sleep in milliseconds.
         */
        public static void Sleep(long time)
        {
            Thread.Sleep(new System.TimeSpan(time * 10000));
        }

        /**
         * Causes the thread which sent this message to sleep an interval of time
         * (given in milliseconds). The precision is not guaranteed - the Thread may
         * sleep more or less than requested.
         * 
         * @param time The time to sleep in milliseconds.
         * @param nanos Extra nanosecond precision
         */
        public static void Sleep(long time, int nanos)
        {
            Thread.Sleep(new System.TimeSpan(time * 10000 + nanos * 100));
        }

        /**
         * Starts the new Thread of execution. The <code>run()</code> method of
         * the receiver will be called by the receiver Thread itself (and not the
         * Thread calling <code>start()</code>).
         */
        public void Start()
        {
            thread.Start();            
        }

        /**
         * This is a no-op if the receiver is suspended. If the receiver
         * <code>isAlive()</code> however, suspended it until
         * <code>resume()</code> is sent to it. Suspend requests are not queued,
         * which means that N requests are equivalent to just one - only one resume
         * request is needed in this case.
         */
        public void Suspend()
        {
            thread.Suspend();
        }

        /**
         * Answers a string containing a concise, human-readable description of the
         * receiver.
         * 
         * @return a printable representation for the receiver.
         */
        public override String ToString()
        {
            return "Thread " + Name + " Priority " + Priority;
        }

        internal static void DumpStack()
        {
            throw new NotImplementedException();
        }
    }
}
