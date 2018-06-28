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
using System.Reflection;

namespace ILOG.J2CsMapping.Reflect
{
    /// <summary>
    /// Summary description for IlrMethodAdapter.
    /// </summary>
    public class IlrMethodInfoAdapter
    {
        private Type[] types = null;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameters"></param>
        public IlrMethodInfoAdapter(ParameterInfo[] parameters)
        {
            types = new Type[parameters.Length];
            int i = 0;
            foreach (ParameterInfo info in parameters)
            {
                types[i++] = info.ParameterType;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Type[] GetTypes()
        {
            return types;
        }
    }
}
