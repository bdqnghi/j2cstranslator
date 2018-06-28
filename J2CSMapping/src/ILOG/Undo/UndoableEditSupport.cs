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
using System.Collections;

namespace ILOG.J2CsMapping.Undo {

    public class UndoableEditSupport {

        protected ArrayList listeners;
        protected CompoundEdit compoundEdit;
        protected int updateLevel;

        private object source;

        public UndoableEditSupport() : this(null) {
        }

        public UndoableEditSupport(object source) {
            this.source = (source == null) ? this : source;
            listeners = new ArrayList();
        }

        public int GetUpdateLevel() {
            return updateLevel;
        }

        public void AddUndoableEditListener(IUndoableEditListener l) {
            lock (this) {
                listeners.Add(l);
            }
        }

        public void RemoveUndoableEditListener(IUndoableEditListener l) {
            lock (this) {
                listeners.Remove(l);
            }
        }

        public IUndoableEditListener[] GetUndoableEditListeners() {
            lock (this) {
                IUndoableEditListener[] result = new IUndoableEditListener[listeners.Count];
                listeners.CopyTo(result);
                return result;
            }
        }

        public virtual void PostEdit(IUndoableEdit e) {
            lock (this) {
                if (updateLevel == 0) {
                    _postEdit(e);
                } else {
                    compoundEdit.AddEdit(e);
                }
            }
        }

        public virtual void BeginUpdate() {
            lock (this) {
                if (updateLevel == 0) {
                    compoundEdit = CreateCompoundEdit();
                }
                updateLevel++;
            }
        }

        public virtual void EndUpdate() {
            lock (this) {
                updateLevel--;
                if (updateLevel == 0) {
                    compoundEdit.End();
                    _postEdit(compoundEdit);
                    compoundEdit = null;
                }
            }
        }

        protected void _postEdit(IUndoableEdit e) {
            UndoableEditEvent ev = new UndoableEditEvent(source, e);
            IUndoableEditListener[] l = GetUndoableEditListeners();
            foreach (IUndoableEditListener el in l) {
                el.UndoableEditHappened(ev);
            }
        }

        protected virtual CompoundEdit CreateCompoundEdit() {
            return new CompoundEdit();
        }
    }
}