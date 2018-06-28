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
    using System.Collections;
    using ILOG.J2CsMapping.Collections;
    using ILOG.J2CsMapping.XML.Sax;


    public class SAXRecorder : DefaultHandler
    {
        internal ArrayList events;
        internal object startDoc;
        internal object endDoc;

        public SAXRecorder()
            : base()
        {
            events = new ArrayList();
            startDoc = new object();
            endDoc = new object();
        }

        public void Clear()
        {
            events.Clear();
        }

        public void PlayBack(SaxParser reader)
        {
            int count = events.Count;
            for (int i = 0; i < count; i++)
            {
                object evt = events[i];
                if (evt is string)
                {
                    string chars = (string)evt;
                    reader.Handler.Characters(chars.ToCharArray(), 0,
                  chars.Length);
                }
                else if (evt is StartElementClass)
                {
                    StartElementClass startElement = (StartElementClass)evt;
                    reader.Handler.StartElement(startElement.uri,
                  startElement.localName, startElement.qName,
                  startElement.attributes);
                }
                else if (evt is EndElementClass)
                {
                    EndElementClass endElement = (EndElementClass)evt;
                    reader.Handler.EndElement(endElement.uri,
                  endElement.localName, endElement.qName);
                }
                else if (evt == startDoc)
                    reader.Handler.StartDocument();
                else if (evt == endDoc)
                    reader.Handler.EndDocument();
            }
        }

        public override void Characters(char[] ch, int start, int length)
        {
            events.Add(new string(ch, start, length));
        }

        public override void EndElement(string uri, string localName, string qName)
        {
            events.Add(new EndElementClass(uri, localName, qName));
        }

        public override void EndDocument()
        {
            events.Add(endDoc);
        }

        public override void StartDocument()
        {
            events.Add(startDoc);
        }

        public override void StartElement
          (string uri, string localName, string qName,
           IAttributes attributes)
        {
            events.Add(new StartElementClass(uri, localName, qName,
                                   attributes)
          );
        }

        //-----------------------------------------------------
        // Inner classes
        //-----------------------------------------------------

        internal class EndElementClass
        {
            internal string uri;
            internal string localName;
            internal string qName;

            internal EndElementClass(string uri, string localName, string qName)
                : base()
            {
                this.uri = uri;
                this.localName = localName;
                this.qName = qName;
            } 
        }

        //-----------------------------------------------------
        // Inner classes
        //-----------------------------------------------------

        internal class StartElementClass
        {
            internal string uri;
            internal string localName;
            internal string qName;
            internal Attributes attributes;

            internal StartElementClass(string uri, string localName, string qName,
                       IAttributes attributes)
                : base()
            {
                this.uri = uri;
                this.localName = localName;
                this.qName = qName;
                this.attributes = new Attributes(attributes);
            }
        }

    }

}
