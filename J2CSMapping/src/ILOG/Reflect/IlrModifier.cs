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
    /// Summary description for IlrModifier.
    /// </summary>
    public class IlrModifier
    {
        public const int STATIC = 1; // 1
        public const int PRIVATE = 2; // 2
        public const int PROTECTED = 4; // 4
        public const int PUBLIC = 8; // 8
        public const int FINAL = 16; // 16
        public const int ABSTRACT = 32; // 32
        public const int SYNCHRONIZED = 64; // 64
        public const int PACKAGE = 128; // 128
        //private static int VOLATILE     = 0x100000000;
        public const int INTERFACE = 256;
        public const int CLASS = 512;

        public const int NATIVE = 1024;
        public const int STRICT = 2048;
        public const int TRANSIENT = 4096;
        public const int VOLATILE = 4096 * 2;
        public const int VIRTUAL = 4096 * 4;
        public const int OVERRIDE = 4096 * 8;
        public const int SEALED = 4096 * 16;
        public const int READONLY = 4096 * 32;
        public const int NEW = 4096 * 64;
        public const int INTERNAL = 4096 * 128;
        public const int EXTERN = 4096 * 256;
        public const int BRIDGE = 4096 * 512;
        public const int SYNTHETIC = 4096 * 1024;
        public const int CONST = 4096 * 2048;
        public const int VARARGS = 4096 * 4096;

        private int modifiers = 0x000;
        private string name = null;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        public IlrModifier(MethodInfo info)
        {
            MethodAttributes attr = info.Attributes;
            name = info.Name;
            bool varArgs = IsVarargs(info);
            InitModifiers(varArgs, attr);
        }

       

        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        public IlrModifier(ConstructorInfo info)
        {
            MethodAttributes attr = info.Attributes;
            name = info.Name;
            bool varArgs = IsVarargs(info);
            InitModifiers(varArgs, attr);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        public IlrModifier(FieldInfo info)
        {
            FieldAttributes attr = info.Attributes;
            name = info.Name;
            InitModifiers(attr);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        public IlrModifier(Type info)
        {
            TypeAttributes attr = info.Attributes;
            name = info.Name;
            InitModifiers(attr);
            if (info.IsInterface)
            {
                modifiers |= INTERFACE;
            }
        }

        //
        //
        //


        private bool IsVarargs(ConstructorInfo info)
        {
            int size = info.GetParameters().Length;
            if (size >= 1)
            {
                ParameterInfo pi = info.GetParameters()[size - 1];
                ParameterAttributes pa = pi.Attributes;
                Object[] customAttributes = pi.GetCustomAttributes(true);
                if (customAttributes != null && customAttributes.Length > 0)
                {
                    foreach(Object custaomAttribute in customAttributes) {
                        if (custaomAttribute is ParamArrayAttribute)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        private bool IsVarargs(MethodInfo info)
        {
            int size = info.GetParameters().Length;
            if (size >= 1)
            {
                ParameterInfo pi = info.GetParameters()[size - 1];
                ParameterAttributes pa = pi.Attributes;
                Object[] customAttributes = pi.GetCustomAttributes(true);
                if (customAttributes != null && customAttributes.Length > 0)
                {
                    foreach (Object custaomAttribute in customAttributes)
                    {
                        if (custaomAttribute is ParamArrayAttribute)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="attr"></param>
        private void InitModifiers(TypeAttributes attr)
        {
            //Console.Write("Attributes for method " + name + " : ");
            if ((attr & TypeAttributes.Public) == TypeAttributes.Public)
                modifiers |= PUBLIC;
            if ((attr & TypeAttributes.Interface) == TypeAttributes.Interface)
                modifiers |= INTERFACE;
            if ((attr & TypeAttributes.Abstract) == TypeAttributes.Abstract)
                modifiers |= ABSTRACT;


            //Console.WriteLine();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attr"></param>
        private void InitModifiers(bool varArgs, MethodAttributes attr)
        {
            //Console.Write("Attributes for method " + name + " : ");
            if ((attr & MethodAttributes.Abstract) == MethodAttributes.Abstract)
                modifiers |= ABSTRACT;
            if ((attr & MethodAttributes.Static) == MethodAttributes.Static)
                modifiers |= STATIC;
            if (varArgs)
                modifiers |= VARARGS;
            if ((attr & MethodAttributes.Public) == MethodAttributes.Public)
            {
                modifiers |= PUBLIC;
                return;
            }
            if ((attr & MethodAttributes.Assembly) == MethodAttributes.Assembly)
            {
                modifiers |= PACKAGE;
                return;
            }
            if ((attr & MethodAttributes.Private) == MethodAttributes.Private)
                modifiers |= PRIVATE;

            //Console.WriteLine();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attr"></param>
        private void InitModifiers(FieldAttributes attr)
        {
            //Console.Write("Attributes for field " + name + " : ");
            if ((attr & FieldAttributes.Public) == FieldAttributes.Public)
                modifiers |= PUBLIC;
            if ((attr & FieldAttributes.Static) == FieldAttributes.Static)
                modifiers |= STATIC;
            if ((attr & FieldAttributes.Private) == FieldAttributes.Private)
                modifiers |= PRIVATE;
            if ((attr & FieldAttributes.Assembly) == FieldAttributes.Assembly)
                modifiers |= PACKAGE;
            if ((attr & FieldAttributes.Literal) == FieldAttributes.Literal)
                modifiers |= CONST;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int GetModifiers()
        {
            return modifiers;
        }

        //
        // Interogation methods
        //

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mod"></param>
        /// <returns></returns>
        static public bool IsPublic(int mod)
        {
            return (mod & PUBLIC) == PUBLIC;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mod"></param>
        /// <returns></returns>
        static public bool IsVarargs(int mod)
        {
            return (mod & VARARGS) == VARARGS;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mod"></param>
        /// <returns></returns>
        static public bool IsPrivate(int mod)
        {
            return (mod & PRIVATE) == PRIVATE;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mod"></param>
        /// <returns></returns>
        static public bool IsProtected(int mod)
        {
            return (mod & PROTECTED) == PROTECTED;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mod"></param>
        /// <returns></returns>
        static public bool IsStatic(int mod)
        {
            return (mod & STATIC) == STATIC;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mod"></param>
        /// <returns></returns>
        static public bool IsFinal(int mod)
        {
            return (mod & FINAL) == FINAL;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mod"></param>
        /// <returns></returns>
        static public bool IsAbstract(int mod)
        {
            return (mod & ABSTRACT) == ABSTRACT;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mod"></param>
        /// <returns></returns>
        static public bool IsSynchronized(int mod)
        {
            return (mod & SYNCHRONIZED) == SYNCHRONIZED;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mod"></param>
        /// <returns></returns>
        static public bool IsTransient(int mod)
        {
            return (mod & TRANSIENT) == TRANSIENT;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mod"></param>
        /// <returns></returns>
        static public bool IsVolatile(int mod)
        {
            return (mod & VOLATILE) == VOLATILE;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mod"></param>
        /// <returns></returns>
        static public bool IsInterface(int mod)
        {
            return (mod & INTERFACE) == INTERFACE;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mod"></param>
        /// <returns></returns>
        public static bool IsConst(int mod)
        {
            return (mod & CONST) == CONST;
        }
    }
}
