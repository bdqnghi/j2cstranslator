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
using System.Threading;

namespace ILOG.J2CsMapping.Util
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ThreadLocal<T>
    {
        private LocalDataStoreSlot localSlot = Thread.AllocateDataSlot();

        /// <summary>
        /// 
        /// </summary>
        public ThreadLocal()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected virtual T InitialValue()
        {
            return default(T);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public T Get()
        {
            lock (this)
            {
                object obj = Thread.GetData(localSlot);
                if (obj == null)
                {
                    Thread.SetData(localSlot, obj = InitialValue());
                }
                return (T)obj;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public void Set(T value)
        {
            Thread.SetData(localSlot, value);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class ThreadLocal : ThreadLocal<object>
    {
    }
}
