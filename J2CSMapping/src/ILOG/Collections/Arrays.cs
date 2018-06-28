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
namespace ILOG.J2CsMapping.Collections
{
    using System;
    using System.Collections;

    /// <summary>
    /// Utility method for arrays
    /// </summary>
    public class Arrays
    {
        //
        // AsList
        //

        /// <summary>
        /// 
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static IList AsList(params Object[] array)
        {
            return new ArrayList(array);
        }

        //
        // Fill
        //

        /// <summary>
        /// Fills the section of the array between the given indices with the given value.
        /// </summary>
        /// <param name="array">the int array to fill</param>
        /// <param name="fromIndex">the start index</param>
        /// <param name="toIndex">the end index + 1</param>
        /// <param name="val">the int element</param>
        public static void Fill(int[] array, int fromIndex, int toIndex, int val)
        {
            RangeCheck(array.Length, fromIndex, toIndex);
            for (int i = fromIndex; i < toIndex; i++)
                array[i] = val;
        }

        /// <summary>
        /// Fills the array with the given value.
        /// </summary>
        /// <param name="array">the array to fill</param>
        /// <param name="val">the element</param>
        static public void Fill(Array array, object val)
        {
            int len = array.Length;
            for (int i = 0; i < len; i++)
            {
                array.SetValue(val, i);
            }
        }

        /// <summary>
        /// Fills the array with the given value.
        /// </summary>
        /// <param name="array">the long array to fill</param>
        /// <param name="val">the long element</param>
        static public void Fill(long[] array, long val)
        {
            int len = array.Length;
            for (int i = 0; i < len; i++)
            {
                array.SetValue(val, i);
            }
        }

        /// <summary>
        /// Fills the section of the array between the given indices with the given value.
        /// </summary>
        /// <param name="array">the array to fill</param>
        /// <param name="start">the start index</param>
        /// <param name="end">the end index + 1</param>
        /// <param name="val">the element</param>
        static public void Fill(Array array, int start, int end, object val)
        {
            int len = end;
            for (int i = start; i < end; i++)
            {
                array.SetValue(val, i);
            }
        }

        //
        // Range Check
        //

        /// <summary>
        /// 
        /// </summary>
        /// <param name="length"></param>
        /// <param name="fromIndex"></param>
        /// <param name="toIndex"></param>
        private static void RangeCheck(int length, int fromIndex, int toIndex)
        {
            if (fromIndex > toIndex)
                throw new ArgumentException("fromIndex(" + fromIndex + ") > toIndex(" + toIndex + ")");
            if (fromIndex < 0)
                throw new IndexOutOfRangeException("fromIndex(" + fromIndex + ")");
            if (toIndex > length)
                throw new IndexOutOfRangeException("toIndex(" + toIndex + ")");
        }

        //------------------------------------------------------------
        // Creation of Java-like jagged arrays.
        //------------------------------------------------------------

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="dim1"></param>
        /// <param name="dim2"></param>
        /// <returns></returns>
        public static Array CreateJaggedArray(Type type, int dim1, int dim2)
        {
            // Type[][] z = new Type[dim1][dim2]
            Array a = Array.CreateInstance(Array.CreateInstance(type, 1).GetType(), dim1);
            for (int i = 0; i < dim1; i++)
            {
                a.SetValue(Array.CreateInstance(type, dim2), i);
            }
            return a;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="dim"></param>
        /// <returns></returns>
        public static Array CreateArray(Type type, int dim)
        {
            return Array.CreateInstance(type, dim);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="dims"></param>
        /// <returns></returns>
        public static Array CreateArray(Type type, params int[] dims)
        {
            return Array.CreateInstance(type, dims);
        }

        //
        // NewInstance
        //

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="dims"></param>
        /// <returns></returns>
        public static Array NewInstance(Type type, params int[] dims)
        {
            return Array.CreateInstance(type, dims);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="dims"></param>
        /// <returns></returns>
        public static Array NewInstance(Type type, params int?[] dims)
        {
            throw new NotImplementedException("NewInstance(Type type, params int?[] dims)");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="arraytype"></param>
        /// <param name="dims"></param>
        /// <returns></returns>
        static Array InternalCreateArray(Type type, Type arraytype, int[] dims)
        {
            int len = dims.Length;
            if (len == 1) return Array.CreateInstance(type, dims[0]);

            Array a = Array.CreateInstance(arraytype, dims[0]);
            int[] subdims = new int[len - 1];
            for (int x = 0; x < len - 1; x++) subdims[x] = dims[x + 1];
            for (int e = 0; e < dims[0]; e++)
                a.SetValue(CreateArray(type, subdims), e);

            return a;
        }

        //
        // Get
        //

        /// <summary>
        /// 
        /// </summary>
        /// <param name="array"></param>
        /// <param name="indice"></param>
        /// <returns></returns>
        static public object Get(object array, int indice)
        {
            return ((Array)array).GetValue(indice);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="array"></param>
        /// <param name="indice"></param>
        /// <returns></returns>
        static public object Get(object array, int? indice)
        {
            return ((Array)array).GetValue(indice.Value);
        }

        //
        // Set
        //

        /// <summary>
        /// 
        /// </summary>
        /// <param name="array"></param>
        /// <param name="indice"></param>
        /// <param name="newValue"></param>
        static public void Set(object array, int indice, Object newValue)
        {
            ((Array)array).SetValue(newValue, indice);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="array"></param>
        /// <param name="indice"></param>
        /// <param name="newValue"></param>
        static public void Set(object array, int? indice, Object newValue)
        {
            ((Array)array).SetValue(newValue, indice.Value);
        }

        //
        // CopyOf
        //

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="U"></typeparam>
        /// <param name="original"></param>
        /// <param name="newLength"></param>
        /// <param name="newType"></param>
        /// <returns></returns>
        public static T[] CopyOf<T, U>(U[] original, int newLength, Type newType) /*where T : class*/
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="U"></typeparam>
        /// <param name="original"></param>
        /// <param name="newLength"></param>
        /// <returns></returns>
        public static T[] CopyOf<T>(T[] original, int newLength) /*where T : class*/
        {
            T[] newArray = new T[newLength];
            Array.Copy(original, newArray, newLength);
            return newArray;
        }


        //
        // Equals
        //

        // Stupid typo ...
        static public bool Equal(byte[] array1, byte[] array2)
        {
            return Equals(array1, array2);
        }

        // Stupid typo ...
        static public bool Equal(sbyte[] array1, sbyte[] array2)
        {
            return Equals(array1, array2);
        }

        /// <summary>
        /// Compares the two arrays.
        /// </summary>
        /// <param name="array1">the first byte array</param>
        /// <param name="array2">the second byte array</param>
        /// <returns>true when the arrays have the same length and the elements at
        /// each index in the two arrays are equal, false otherwise</returns>
        static public bool Equals(byte[] array1, byte[] array2)
        {
            if (array1 == array2)
            {
                return true;
            }
            if (array1 == null || array2 == null || array1.Length != array2.Length)
            {
                return false;
            }
            for (int i = 0; i < array1.Length; i++)
            {
                if (array1[i] != array2[i])
                {
                    return false;
                }
            }
            return true;           
        }

        /// <summary>
        /// Compares the two arrays.
        /// </summary>
        /// <param name="array1">the first sbyte array</param>
        /// <param name="array2">the second sbyte array</param>
        /// <returns>true when the arrays have the same length and the elements at
        /// each index in the two arrays are equal, false otherwise</returns>
        static public bool Equals(sbyte[] array1, sbyte[] array2)
        {
            if (array1 == array2)
            {
                return true;
            }
            if (array1 == null || array2 == null || array1.Length != array2.Length)
            {
                return false;
            }
            for (int i = 0; i < array1.Length; i++)
            {
                if (array1[i] != array2[i])
                {
                    return false;
                }
            }
            return true;           
        }

        /// <summary>
        /// Compares the two arrays.
        /// </summary>
        /// <param name="array1">the first short array</param>
        /// <param name="array2">the second short array</param>
        /// <returns>true when the arrays have the same length and the elements at
        /// each index in the two arrays are equal, false otherwise</returns>
        static public bool Equals(short[] array1, short[] array2)
        {
            if (array1 == array2)
            {
                return true;
            }
            if (array1 == null || array2 == null || array1.Length != array2.Length)
            {
                return false;
            }
            for (int i = 0; i < array1.Length; i++)
            {
                if (array1[i] != array2[i])
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Compares the two arrays.
        /// </summary>
        /// <param name="array1">the first char array</param>
        /// <param name="array2">the second char array</param>
        /// <returns>true when the arrays have the same length and the elements at
        /// each index in the two arrays are equal, false otherwise</returns>
        static public bool Equals(char[] array1, char[] array2)
        {
            if (array1 == array2)
            {
                return true;
            }
            if (array1 == null || array2 == null || array1.Length != array2.Length)
            {
                return false;
            }
            for (int i = 0; i < array1.Length; i++)
            {
                if (array1[i] != array2[i])
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Compares the two arrays.
        /// </summary>
        /// <param name="array1">the first int array</param>
        /// <param name="array2">the second int array</param>
        /// <returns>true when the arrays have the same length and the elements at
        /// each index in the two arrays are equal, false otherwise</returns>
        static public bool Equals(int[] array1, int[] array2)
        {
            if (array1 == array2)
            {
                return true;
            }
            if (array1 == null || array2 == null || array1.Length != array2.Length)
            {
                return false;
            }
            for (int i = 0; i < array1.Length; i++)
            {
                if (array1[i] != array2[i])
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Compares the two arrays.
        /// </summary>
        /// <param name="array1">the first long array</param>
        /// <param name="array2">the second long array</param>
        /// <returns>true when the arrays have the same length and the elements at
        /// each index in the two arrays are equal, false otherwise</returns>
        static public bool Equals(long[] array1, long[] array2)
        {
            if (array1 == array2)
            {
                return true;
            }
            if (array1 == null || array2 == null || array1.Length != array2.Length)
            {
                return false;
            }
            for (int i = 0; i < array1.Length; i++)
            {
                if (array1[i] != array2[i])
                {
                    return false;
                }
            }
            return true;
        }


        /// <summary>
        /// Compares the two arrays.
        /// </summary>
        /// <param name="array1">the first float array</param>
        /// <param name="array2">the second float array</param>
        /// <returns>true when the arrays have the same length and the elements at
        /// each index in the two arrays are equal, false otherwise</returns>
        static public bool Equals(float[] array1, float[] array2)
        {
            if (array1 == array2)
            {
                return true;
            }
            if (array1 == null || array2 == null || array1.Length != array2.Length)
            {
                return false;
            }
            for (int i = 0; i < array1.Length; i++)
            {
                if (array1[i] != array2[i])
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Compares the two arrays.
        /// </summary>
        /// <param name="array1">the first double array</param>
        /// <param name="array2">the second double array</param>
        /// <returns>true when the arrays have the same length and the elements at
        /// each index in the two arrays are equal, false otherwise</returns>
        static public bool Equals(double[] array1, double[] array2)
        {
            if (array1 == array2)
            {
                return true;
            }
            if (array1 == null || array2 == null || array1.Length != array2.Length)
            {
                return false;
            }
            for (int i = 0; i < array1.Length; i++)
            {
                if (array1[i] != array2[i])
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Compares the two arrays.
        /// </summary>
        /// <param name="array1">the first bool array</param>
        /// <param name="array2">the second bool array</param>
        /// <returns>true when the arrays have the same length and the elements at
        /// each index in the two arrays are equal, false otherwise</returns>
        static public bool Equals(bool[] array1, bool[] array2)
        {
            if (array1 == array2)
            {
                return true;
            }
            if (array1 == null || array2 == null || array1.Length != array2.Length)
            {
                return false;
            }
            for (int i = 0; i < array1.Length; i++)
            {
                if (array1[i] != array2[i])
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Compares the two arrays.
        /// </summary>
        /// <param name="array1">the first Object array</param>
        /// <param name="array2">the second Object array</param>
        /// <returns>true when the arrays have the same length and the elements at
        /// each index in the two arrays are equal, false otherwise</returns>
        public static bool Equals(Object[] array1, Object[] array2)
        {
            if (array1 == array2)
            {
                return true;
            }
            if (array1 == null || array2 == null || array1.Length != array2.Length)
            {
                return false;
            }
            for (int i = 0; i < array1.Length; i++)
            {
                Object e1 = array1[i], e2 = array2[i];
                if (!(e1 == null ? e2 == null : e1.Equals(e2)))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
