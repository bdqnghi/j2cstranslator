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
	public interface IContentHandler 
	{
		void SetDocumentLocator(Locator locator);
		void StartDocument();
		void EndDocument();
		void ProcessingInstruction(string target, string data);
		void StartPrefixMapping(string prefix, string uri);
		void EndPrefixMapping(string prefix);
		void StartElement(string namespaceURI, string localName,string rawName, IAttributes atts);
		void EndElement(string namespaceURI, string localName,string rawName);
		void Characters(char[] ch, int start, int end);
		void IgnorableWhitespace(char[] ch, int start, int end);
		void SkippedEntity(string name);
	}
}