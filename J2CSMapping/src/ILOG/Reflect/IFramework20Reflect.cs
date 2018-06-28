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
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace ILOG.J2CsMapping.Reflect
{
    /// <summary>
    /// 
    /// </summary>
    public interface IFramework20Reflect
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        Type[] GetGenericArguments(Type type);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        Type[] GetGenericArguments(MethodBase type);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        bool HasGenericArguments(Type type);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        bool HasGenericArguments(MethodBase type);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        bool IsGenericParameter(Type type);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        bool ContainsGenericParameters(Type type);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        Type[] GetGenericParameterConstraints(Type type);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        Type GetGenericTypeDefinition(Type type);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="method"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        MethodInfo Bind(MethodInfo method, Type[] parameters);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="clazz"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Type Bind(Type clazz, Type[] parameters);
    }

    /// <summary>
    /// 
    /// </summary>
    public class Framework20Reflect : IFramework20Reflect
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public Type[] GetGenericArguments(Type type)
        {
            return type.GetGenericArguments();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="methd"></param>
        /// <returns></returns>
        public Type[] GetGenericArguments(MethodBase methd)
        {
            return methd.GetGenericArguments();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool HasGenericArguments(Type type)
        {
            return type.IsGenericType;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="methd"></param>
        /// <returns></returns>
        public bool HasGenericArguments(MethodBase methd)
        {
            return methd.IsGenericMethod;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool IsGenericParameter(Type type)
        {
            return type.IsGenericParameter;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public Type[] GetGenericParameterConstraints(Type type)
        {
            if (type.IsGenericParameter)
                return type.GetGenericParameterConstraints();
            else
                return null;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public Type GetGenericTypeDefinition(Type type)
        {
            return type.GetGenericTypeDefinition();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="method"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public MethodInfo Bind(MethodInfo method, Type[] parameters)
        {
            return method.MakeGenericMethod(parameters);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="clazz"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public Type Bind(Type clazz, Type[] parameters)
        {
            return clazz.MakeGenericType(parameters);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool ContainsGenericParameters(Type type)
        {
            return type.ContainsGenericParameters;
        }
    }
}
