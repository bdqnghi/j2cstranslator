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

namespace ILOG.J2CsMapping.XML.Sax
{
    using System.Text;
    using ILOG.J2CsMapping.XML.Sax;


    public class BasicSAXHandler : DefaultHandler
    {
        private StringBuilder buffer;
        private Locator locator;

        public BasicSAXHandler()
            : base()
        {
        }

        public override void SetDocumentLocator
          (Locator locator)
        {
            this.locator = locator;
        }

        public Locator GetLocator()
        {
            return locator;
        }

        public override void EndElement(string uri, string localName, string qName)
        {
            if (buffer != null)
                buffer.Length = 0;
        }


        public override void StartElement
          (string uri, string localName, string qName, IAttributes attributes)
        {
            if (buffer != null)
                buffer.Length = 0;
        }

        public override void Characters(char[] ch, int start, int length)
        {
            if (buffer == null)
                buffer = new StringBuilder(length);
            buffer.Append(ch, start, length);
        }

        public string GetBuffer()
        {
            return buffer.ToString();
        }

    }

}
