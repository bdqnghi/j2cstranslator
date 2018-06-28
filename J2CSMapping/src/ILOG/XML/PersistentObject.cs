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

using ILOG.J2CsMapping.Collections;
using System;

namespace ILOG.J2CsMapping.XML
{
    /// <summary>
    /// Interface for XML-persistent objects.
    /// </summary>
    public interface PersistentObject
    {
        void AddXmlProperty(string str, object obj);

        void RemoveXmlProperty(string str);

        object GetXmlProperty(string str);

        /// <summary></summary>
        IIterator GetXmlPropertyNames();

        /// <summary>Get a class implementing the ilog.rules.util.xml.IlrXmlObjectHandler
        ///  interface.</summary>
        Type GetXmlHandlerClass();

        /// <summary>Get the version of this handler.
        ///  When this handler is saved in an XML document, its class name
        ///  and version are stored as attributes of the root tag
        ///  representing the object.
        ///  When reading the XML document, the storage manager will
        ///  first try to find the "className_version.class" class. If
        ///  the class is not found, the "className.class" will be looked
        ///  up instead.</summary>
        string GetXmlVersion();
    }
}
