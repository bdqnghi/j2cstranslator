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
	/// <summary>
	/// Summary description for DefaultHandler.
	/// </summary>
	public class DefaultHandler : IContentHandler, IErrorHandler
	{
		public DefaultHandler()
		{
		}

		#region IErrorHandler Members

		public virtual void Error(SAXParseException ex)
		{
			// TODO:  Add DefaultHandler.Error implementation
		}

		public virtual void FatalError(SAXParseException ex)
		{
			// TODO:  Add DefaultHandler.FatalError implementation
		}

		public virtual void Warning(SAXParseException ex)
		{
			// TODO:  Add DefaultHandler.WarningError implementation
		}

		#endregion

		#region IContentHandler Members

		public virtual void SetDocumentLocator(Locator locator)
		{
			// TODO:  Add DefaultHandler.SetDocumentLocator implementation
		}

		public virtual void StartDocument()
		{
			// TODO:  Add DefaultHandler.StartDocument implementation
		}

		public virtual void EndDocument()
		{
			// TODO:  Add DefaultHandler.EndDocument implementation
		}

		public virtual void ProcessingInstruction(string target, string data)
		{
			// TODO:  Add DefaultHandler.ProcessingInstruction implementation
		}

		public virtual void StartPrefixMapping(string prefix, string uri)
		{
			// TODO:  Add DefaultHandler.StartPrefixMapping implementation
		}

		public virtual void EndPrefixMapping(string prefix)
		{
			// TODO:  Add DefaultHandler.EndPrefixMapping implementation
		}

		public virtual void StartElement(string namespaceURI, string localName, string rawName, IAttributes atts)
		{
			// TODO:  Add DefaultHandler.StartElement implementation
		}

		public virtual void EndElement(string namespaceURI, string localName, string rawName)
		{
			// TODO:  Add DefaultHandler.EndElement implementation
		}

		public virtual void Characters(char[] ch, int start, int end)
		{
			// TODO:  Add DefaultHandler.Characters implementation
		}

		public virtual void IgnorableWhitespace(char[] ch, int start, int end)
		{
			// TODO:  Add DefaultHandler.IgnorableWhitespace implementation
		}

		public virtual void SkippedEntity(string name)
		{
			// TODO:  Add DefaultHandler.SkippedEntity implementation
		}

		#endregion
	}
}
