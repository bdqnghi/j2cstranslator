using System;
using System.Collections.Generic;
using System.Text;
using ILOG.J2CsMapping.Collections.Generics;
using System.Reflection;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Resources;
using ILOG.J2CsMapping.Util;

namespace ILOG.J2CsMapping.Util
{
    public abstract class ResourceBundle
    {

        /// <summary>
        /// The parent of this ResourceBundle.
        /// </summary>
        ///
        protected internal ResourceBundle parent;

        private Locale locale;

        public sealed class Anonymous_C0 :
                Object
        {
            private readonly Assembly loader;
            private readonly String fileName;

            public Anonymous_C0(Assembly loader_0, String fileName_1)
            {
                this.loader = loader_0;
                this.fileName = fileName_1;
            }

            public Stream Run()
            {
                /*return (loader == null) ? System.Reflection.Assembly
                        .GetSystemResourceAsStream(fileName
                                + ".properties") : ILOG.J2CsMapping.IO.IOUtility.GetResourceAsStream(loader, fileName
                                                            + ".properties"); //$NON-NLS-1$*/
                return null;
            }
        }

        internal class MissingBundle : ResourceBundle
        {
            public override IIterator<String> GetKeys()
            {
                return null;
            }

            public override Object HandleGetObject(String name)
            {
                return null;
            }
        }

        private static readonly ResourceBundle MISSING = new ResourceBundle.MissingBundle();

        private static readonly ResourceBundle MISSINGBASE = new ResourceBundle.MissingBundle();

        private static readonly Dictionary<Object, Dictionary<String, ResourceBundle>> cache = new Dictionary<Object, Dictionary<String, ResourceBundle>>();

        /// <summary>
        /// Constructs a new instance of this class.
        /// </summary>
        ///
        public ResourceBundle()
        {
            /* empty */
        }

        /// <summary>
        /// Finds the named resource bundle for the default locale.
        /// </summary>
        ///
        /// <param name="bundleName">the name of the resource bundle</param>
        /// <returns>ResourceBundle</returns>
        /// <exception cref="MissingResourceException">when the resource bundle cannot be found</exception>
        public static ResourceBundle GetBundle(String bundleName)
        {
            return null; /*getBundleImpl(bundleName, Locale.getDefault(), VM
							.callerClassLoader()); */
        }

        /// <summary>
        /// Finds the named resource bundle for the specified locale.
        /// </summary>
        ///
        /// <param name="bundleName">the name of the resource bundle</param>
        /// <param name="locale_0">the locale</param>
        /// <returns>ResourceBundle</returns>
        /// <exception cref="MissingResourceException">when the resource bundle cannot be found</exception>
        public static ResourceBundle GetBundle(String bundleName,
                Locale locale_0)
        {
            return null; /* getBundleImpl(bundleName, locale, VM.callerClassLoader()); */
        }

        /// <summary>
        /// Finds the named resource bundle for the specified locale.
        /// </summary>
        ///
        /// <param name="bundleName">the name of the resource bundle</param>
        /// <param name="locale_0">the locale</param>
        /// <param name="loader_1">the ClassLoader to use</param>
        /// <returns>ResourceBundle</returns>
        /// <exception cref="MissingResourceException">when the resource bundle cannot be found</exception>
        public static ResourceBundle GetBundle(String bundleName, Locale locale_0,
                Assembly loader_1)
        {
            if (loader_1 == null)
            {
                throw new NullReferenceException();
            }
            if (bundleName != null)
            {
                ResourceBundle bundle;
                if (!locale_0.Equals(Locale.GetDefault()))
                {
                    if ((bundle = HandleGetBundle(bundleName, "_" + locale_0, false, //$NON-NLS-1$
                            loader_1)) != null)
                    {
                        return bundle;
                    }
                }
                if ((bundle = HandleGetBundle(bundleName,
                        "_" + Locale.GetDefault(), true, loader_1)) != null)
                { //$NON-NLS-1$
                    return bundle;
                }
                throw new MissingManifestResourceException("KA029"); //$NON-NLS-1$
            }
            throw new NullReferenceException();
        }

        private static ResourceBundle GetBundleImpl(String bundleName,
                Locale locale_0, Assembly loader_1)
        {
            if (bundleName != null)
            {
                ResourceBundle bundle;
                if (!locale_0.Equals(Locale.GetDefault()))
                {
                    String localeName = locale_0.ToString();
                    if (localeName.Length > 0)
                    {
                        localeName = "_" + localeName; //$NON-NLS-1$
                    }
                    if ((bundle = HandleGetBundle(bundleName, localeName, false,
                            loader_1)) != null)
                    {
                        return bundle;
                    }
                }
                String localeName_2 = Locale.GetDefault().ToString();
                if (localeName_2.Length > 0)
                {
                    localeName_2 = "_" + localeName_2; //$NON-NLS-1$
                }
                if ((bundle = HandleGetBundle(bundleName, localeName_2, true, loader_1)) != null)
                {
                    return bundle;
                }
                throw new MissingManifestResourceException("KA029"); //$NON-NLS-1$
            }
            throw new NullReferenceException();
        }

        /// <summary>
        /// Answers the names of the resources contained in this ResourceBundle.
        /// </summary>
        ///
        /// <returns>an Enumeration of the resource names</returns>
        public abstract IIterator<String> GetKeys();

        /// <summary>
        /// Gets the Locale of this ResourceBundle.
        /// </summary>
        ///
        /// <returns>the Locale of this ResourceBundle</returns>
        public virtual Locale GetLocale()
        {
            return locale;
        }

        /// <summary>
        /// Answers the named resource from this ResourceBundle.
        /// </summary>
        ///
        /// <param name="key">the name of the resource</param>
        /// <returns>the resource object</returns>
        /// <exception cref="MissingResourceException">when the resource is not found</exception>
        public virtual Object GetObject(String key)
        {
            ResourceBundle last, theParent = this;
            do
            {
                Object result = theParent.HandleGetObject(key);
                if (result != null)
                {
                    return result;
                }
                last = theParent;
                theParent = theParent.parent;
            } while (theParent != null);
            throw new MissingManifestResourceException("KA029"); //$NON-NLS-1$
        }

        /// <summary>
        /// Answers the named resource from this ResourceBundle.
        /// </summary>
        ///
        /// <param name="key">the name of the resource</param>
        /// <returns>the resource string</returns>
        /// <exception cref="MissingResourceException">when the resource is not found</exception>
        public String GetString(String key)
        {
            return (String)GetObject(key);
        }

        /// <summary>
        /// Answers the named resource from this ResourceBundle.
        /// </summary>
        ///
        /// <param name="key">the name of the resource</param>
        /// <returns>the resource string array</returns>
        /// <exception cref="MissingResourceException">when the resource is not found</exception>
        public String[] GetStringArray(String key)
        {
            return (String[])GetObject(key);
        }

        private static ResourceBundle HandleGetBundle(String bs, String locale_0,
                bool loadBase, Assembly loader_1)
        {
            ResourceBundle bundle = null;
            String bundleName = bs + locale_0;
            Object cacheKey = (loader_1 != null) ? (Object)loader_1 : (Object)"null"; //$NON-NLS-1$
            Dictionary<String, ResourceBundle> loaderCache;
            lock (cache)
            {
                loaderCache = ((System.Collections.Generic.Dictionary<System.String, ResourceBundle>)ILOG.J2CsMapping.Collections.Generics.Collections.Get(cache, cacheKey));
                if (loaderCache == null)
                {
                    loaderCache = new Dictionary<String, ResourceBundle>();
                    cache[cacheKey] = loaderCache;
                    //ILOG.J2CsMapping.Collections.Generics.Collections.Put(cache, (System.Object)(cacheKey), (System.Collections.Generic.Dictionary<System.String, ResourceBundle>)(loaderCache));
                }
            }
            ResourceBundle result = (ResourceBundle)ILOG.J2CsMapping.Collections.Generics.Collections.Get(loaderCache, bundleName);
            if (result != null)
            {
                if (result == MISSINGBASE)
                {
                    return null;
                }
                if (result == MISSING)
                {
                    if (!loadBase)
                    {
                        return null;
                    }
                    String extension = Strip(locale_0);
                    if (extension == null)
                    {
                        return null;
                    }
                    return HandleGetBundle(bs, extension, loadBase, loader_1);
                }
                return result;
            }

            try
            {
                Type bundleClass = ILOG.J2CsMapping.Reflect.Helper.GetNativeType(bundleName);

                if (typeof(ResourceBundle).IsAssignableFrom(bundleClass))
                {
                    bundle = (ResourceBundle)Activator.CreateInstance(bundleClass);
                }
            }
            catch (Exception e)
            {
            }

            if (bundle != null)
            {
                //bundle.SetLocale(locale_0);
            }
            else
            {
                String fileName_3 = bundleName.Replace('.', '/');
                Stream stream = new Anonymous_C0(loader_1, fileName_3).Run();
                if (stream != null)
                {
                    try
                    {
                        try
                        {
                            //bundle = new PropertyResourceBundle(stream);
                        }
                        finally
                        {
                            stream.Close();
                        }
                        //bundle.SetLocale(locale_0);
                    }
                    catch (IOException e_4)
                    {
                    }
                }
            }

            String extension_5 = Strip(locale_0);
            if (bundle != null)
            {
                if (extension_5 != null)
                {
                    ResourceBundle parent_6 = HandleGetBundle(bs, extension_5, true,
                            loader_1);
                    if (parent_6 != null)
                    {
                        // bundle.SetParent(parent_6);
                    }
                }
                ILOG.J2CsMapping.Collections.Generics.Collections.Put(loaderCache, (System.String)(bundleName), (ResourceBundle)(bundle));
                return bundle;
            }

            if (extension_5 != null && (loadBase || extension_5.Length > 0))
            {
                bundle = HandleGetBundle(bs, extension_5, loadBase, loader_1);
                if (bundle != null)
                {
                    ILOG.J2CsMapping.Collections.Generics.Collections.Put(loaderCache, (System.String)(bundleName), (ResourceBundle)(bundle));
                    return bundle;
                }
            }
            ILOG.J2CsMapping.Collections.Generics.Collections.Put(loaderCache, (System.String)(bundleName), (ResourceBundle)((loadBase) ? MISSINGBASE : MISSING));
            return null;
        }

        /// <summary>
        /// Answers the named resource from this ResourceBundle, or null if the
        /// resource is not found.
        /// </summary>
        ///
        /// <param name="key">the name of the resource</param>
        /// <returns>the resource object</returns>
        public abstract Object HandleGetObject(String key);

        /// <summary>
        /// Sets the parent resource bundle of this ResourceBundle. The parent is
        /// searched for resources which are not found in this resource bundle.
        /// </summary>
        ///
        /// <param name="bundle">the parent resource bundle</param>
        protected internal void SetParent(ResourceBundle bundle)
        {
            parent = bundle;
        }

        private static String Strip(String name)
        {
            int index = name.LastIndexOf('_');
            if (index != -1)
            {
                return name.Substring(0, (index) - (0));
            }
            return null;
        }

        private void SetLocale(String name)
        {
            String language = "", country = "", variant = ""; //$NON-NLS-1$//$NON-NLS-2$ //$NON-NLS-3$
            if (name.Length > 1)
            {
                int nextIndex = name.IndexOf('_', 1);
                if (nextIndex == -1)
                {
                    nextIndex = name.Length;
                }
                language = name.Substring(1, (nextIndex) - (1));
                if (nextIndex + 1 < name.Length)
                {
                    int index = nextIndex;
                    nextIndex = name.IndexOf('_', nextIndex + 1);
                    if (nextIndex == -1)
                    {
                        nextIndex = name.Length;
                    }
                    country = name.Substring(index + 1, (nextIndex) - (index + 1));
                    if (nextIndex + 1 < name.Length)
                    {
                        variant = name.Substring(nextIndex + 1, (name.Length) - (nextIndex + 1));
                    }
                }
            }
            locale = new Locale(language, country, variant);
        }
    }

    public abstract class ListResourceBundle : ResourceBundle
    {
        public sealed class Anonymous_C1 : IIterator<String>
        {
            private readonly ListResourceBundle outer_ListResourceBundle;
            internal IIterator<String> local;
            internal IIterator<String> pEnum;
            internal String nextElement;

            public Anonymous_C1(ListResourceBundle paramouter_ListResourceBundle)
            {
                this.outer_ListResourceBundle = paramouter_ListResourceBundle;
                this.pEnum = outer_ListResourceBundle.parent.GetKeys();
                this.local = new ILOG.J2CsMapping.Collections.Generics.IteratorAdapter<System.String>(new ILOG.J2CsMapping.Collections.Generics.ListSet<System.String>(outer_ListResourceBundle.table.Keys).GetEnumerator());
            }

            public bool FindNext()
            {
                if (nextElement != null)
                {
                    return true;
                }
                while (pEnum.HasNext())
                {
                    String next = pEnum.Next();
                    if (!outer_ListResourceBundle.table.ContainsKey(next))
                    {
                        nextElement = next;
                        return true;
                    }
                }
                return false;
            }

            public bool HasMoreElements()
            {
                if (local.HasNext())
                {
                    return true;
                }
                return FindNext();
            }

            public String NextElement()
            {
                if (local.HasNext())
                {
                    return local.Next();
                }
                if (FindNext())
                {
                    String result = nextElement;
                    nextElement = null;
                    return result;
                }
                // Cause an exception
                return pEnum.Next();
            }

            #region AddedByTranslator

            object ILOG.J2CsMapping.Collections.IIterator.Next()
            {
                throw new NotImplementedException();
            }

            #endregion


            #region IIterator<string> Members

            public bool HasNext()
            {
                throw new NotImplementedException();
            }

            string IIterator<string>.Next()
            {
                throw new NotImplementedException();
            }

            public void Remove()
            {
                throw new NotImplementedException();
            }

            #endregion
        }

        public sealed class Anonymous_C0 : IIterator<String>
        {
            private readonly ListResourceBundle outer_ListResourceBundle;
            internal IIterator<String> it;

            public Anonymous_C0(ListResourceBundle paramouter_ListResourceBundle)
            {
                this.outer_ListResourceBundle = paramouter_ListResourceBundle;
                this.it = new ILOG.J2CsMapping.Collections.Generics.IteratorAdapter<System.String>(new ILOG.J2CsMapping.Collections.Generics.ListSet<System.String>(outer_ListResourceBundle.table.Keys).GetEnumerator());
            }

            public bool HasMoreElements()
            {
                return it.HasNext();
            }

            public String NextElement()
            {
                return it.Next();
            }

            #region AddedByTranslator

            object ILOG.J2CsMapping.Collections.IIterator.Next()
            {
                throw new NotImplementedException();
            }

            #endregion


            #region IIterator<string> Members

            public bool HasNext()
            {
                return HasMoreElements();
            }

            string IIterator<string>.Next()
            {
                return NextElement();
            }

            public void Remove()
            {
                throw new NotImplementedException();
            }

            #endregion
        }

        internal Dictionary<String, Object> table;

        /// <summary>
        /// Constructs a new instance of this class.
        /// </summary>
        ///
        public ListResourceBundle()
            : base()
        {
        }

        /// <summary>
        /// Answers an Object array which contains the resources of this
        /// ListResourceBundle. Each element in the array is an array of two
        /// elements, the first is the resource key and the second is the resource.
        /// </summary>
        ///
        /// <returns>a Object array containing the resources</returns>
        public abstract Object[][] GetContents();

        /// <summary>
        /// Answers the names of the resources contained in this ListResourceBundle.
        /// </summary>
        ///
        /// <returns>an Enumeration of the resource names</returns>
        public override IIterator<String> GetKeys()
        {
            InitializeTable();
            if (parent != null)
            {
                return new ListResourceBundle.Anonymous_C1(this);
            }
            else
            {
                return new ListResourceBundle.Anonymous_C0(this);
            }
        }

        /// <summary>
        /// Answers the named resource from this ResourceBundle, or null if the
        /// resource is not found.
        /// </summary>
        ///
        /// <param name="key">the name of the resource</param>
        /// <returns>the resource object</returns>
        public sealed override Object HandleGetObject(String key)
        {
            InitializeTable();
            if (key == null)
            {
                throw new NullReferenceException();
            }
            return (System.Object)ILOG.J2CsMapping.Collections.Generics.Collections.Get(table, (System.String)key);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        private void InitializeTable()
        {
            if (table == null)
            {
                Object[][] contents = GetContents();
                table = new Dictionary<String, Object>(contents.Length / 3 * 4 + 3);
                /* foreach */
                foreach (Object[] content in contents)
                {
                    if (content[0] == null || content[1] == null)
                    {
                        throw new NullReferenceException();
                    }
                    ILOG.J2CsMapping.Collections.Generics.Collections.Put(table, (System.String)((String)content[0]), (System.Object)(content[1]));
                }
            }

        }
    }
}
    
