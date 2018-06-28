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

namespace ILOG.J2CsMapping.Undo {

    public interface IUndoableEditListener : ILOG.J2CsMapping.Util.IEventListener {
        void UndoableEditHappened(UndoableEditEvent e);
    }

    public class UndoableEditEvent : ILOG.J2CsMapping.Util.EventObject {
        private IUndoableEdit edit;

        public UndoableEditEvent(object source, IUndoableEdit edit) : base(source) {
            this.edit = edit;
        }

        public IUndoableEdit GetEdit() {
            return edit;
        }
    }
}
