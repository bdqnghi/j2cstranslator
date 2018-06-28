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
    using ILOG.J2CsMapping.XML.Sax;

    public abstract class PooledSAXHandler : ChainedSAXHandler
    {
        private IList pool;

        public PooledSAXHandler(IList pool)
            : base()
        {
            this.pool = pool;
            if (pool != null)
                pool.Add(this);
        }

        public override void Activate
          (ChainedSAXHandler parent, IAttributes attributes)
        {
            base.Activate(parent, attributes);
            if (pool != null)
                pool.Remove(this);
        }

        public override void Deactivate()
        {
            base.Deactivate();
            if (pool != null)
                pool.Add(this);
        }
    }
}
