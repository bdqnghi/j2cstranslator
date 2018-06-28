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
using System.Collections.Generic;
using System.Text;

namespace ILOG.J2CsMapping.Util.Beans {

    /// <summary>
    /// 
    /// </summary>
    public class PropertyChangeEvent : EventObject {
        private String propertyName;
        private Object newValue;
        private Object oldValue;
        private Object propagationId;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="propertyName"></param>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        public PropertyChangeEvent(Object source, String propertyName,
                         Object oldValue, Object newValue)
            : base(source) {
            this.propertyName = propertyName;
            this.newValue = newValue;
            this.oldValue = oldValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public String GetPropertyName() {
            return propertyName;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Object GetNewValue() {
            return newValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Object GetOldValue() {
            return oldValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="propagationId"></param>
        public void SetPropagationId(Object propagationId) {
            this.propagationId = propagationId;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Object GetPropagationId() {
            return propagationId;
        }
    }
}
