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

    using System;
    using ILOG.J2CsMapping.XML.Sax;


    public abstract class ChainedSAXHandler : BasicSAXHandler
    {
        private ChainedSAXHandler parent;

        public ChainedSAXHandler()
            : base()
        {
        }

        public ChainedSAXHandler GetParent()
        {
            return parent;
        }

        public virtual SaxParser GetReader()
        {
            return this.GetParent().GetReader();
        }

        public override void EndElement(string uri, string localName, string qName)
        {
            if (this.IsEnclosingTag(localName))
                this.Deactivate();
            base.EndElement(uri, localName, qName);
        }

        public virtual void Activate(ChainedSAXHandler parent, IAttributes attributes)
        {
            if (parent != null)
            {
                if (parent == this)
                    throw new ArgumentException("A ChainedSAXParser cannot be its parent");
                this.parent = parent;
                this.SetDocumentLocator(parent.GetLocator());
                this.GetReader().Handler = this;
            }
        }

        public virtual void Deactivate()
        {
            if (parent != null)
            {
                this.GetReader().Handler = parent;
                parent.ChildDeactivated(this);
                parent = null;
            }
        }

        public virtual void ChildDeactivated(ChainedSAXHandler child)
        {
        }

        public abstract bool IsEnclosingTag(string str);

    }

}
