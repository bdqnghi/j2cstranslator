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
    /// Utility method for java.lang.relfect API
    /// </summary>
    public class Helper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="paramsType"></param>
        /// <returns></returns>
        public static ConstructorInfo GetConstructor(Type type, params Type[] paramsType)
        {
            if (paramsType.Length == 0)
            {
                return type.GetConstructor(new Type[0]);
            }
            else
            {
                return type.GetConstructor(paramsType);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <param name="paramsType"></param>
        /// <returns></returns>
        public static MethodInfo GetMethod(Type type, String name, params Type[] paramsType)
        {
            char upper = name.Substring(0, 1).ToUpper()[0];
            String upperMethodName = upper + name.Remove(0, 1);
            if (paramsType.Length == 0)
            {
                return type.GetMethod(upperMethodName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
            }
            else
            {
                return type.GetMethod(upperMethodName, paramsType);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cInfo"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static T Invoke<T>(ConstructorInfo cInfo, params Object[] parameters)
        {
            return (T)cInfo.Invoke(parameters);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cInfo"></param>
        /// <param name="obj"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static T Invoke<T>(MethodInfo cInfo, Object obj, params Object[] parameters)
        {
            return (T)cInfo.Invoke(obj, parameters);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cInfo"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static Object Invoke(ConstructorInfo cInfo, params Object[] parameters)
        {
            return cInfo.Invoke(parameters);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cInfo"></param>
        /// <param name="obj"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static Object Invoke(MethodInfo cInfo, Object obj, params Object[] parameters)
        {
            return cInfo.Invoke(obj, parameters);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="className"></param>
        /// <returns></returns>
        public static Type GetNativeType(string className)
        {
            return GetNativeType(Assembly.GetExecutingAssembly(), className);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="className"></param>
        /// <returns></returns>
        public static Type GetNativeType(Assembly a, string className)
        {
            Type clazz;
            //BRN-2808
            // Assembly a = Assembly.GetExecutingAssembly(); // typeof(Helper).Assembly;
            clazz = a.GetType(className);
            if (clazz != null)
                return clazz;
            int index = className.LastIndexOf(".");
            if (index > 0)
            {
                string rest = className.Substring(index + 1);
                string n = className.Substring(0, index) + "+" + rest;
                Type c2 = a.GetType(n);
                if (c2 != null)
                {
                    return c2;
                }
            }
            Assembly[] loaders = AppDomain.CurrentDomain.GetAssemblies();
            foreach (Assembly loader in loaders)
            {
                // Console.WriteLine("Referenced assembly is " + loader);
                // TODO : May crash if loader == null !
                clazz = loader.GetType(className);
                if (clazz == null)
                {
                    // TODO : May crash if className == null !
                    index = className.LastIndexOf(".");
                    if (index > 0)
                    {
                        string rest = className.Substring(index + 1);
                        string n = className.Substring(0, index) + "+" + rest;
                        Type c2 = loader.GetType(n);
                        if (c2 != null)
                        {
                            return c2;
                        }
                    }
                }
                if (clazz != null)
                    return clazz;
            }
            if (AssemblyScanner.Self == null)
            {
                AssemblyScanner.Initialize();
                AssemblyScanner.Self = new AssemblyScanner();
            }
            Type t = AssemblyScanner.Self.Resolve(className);
            return t;
        }
    }
}
