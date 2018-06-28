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

namespace ILOG.J2CsMapping.Undo {

    public class UndoManager : CompoundEdit, IUndoableEditListener {

        private int limit;
        private int index;

        public UndoManager() {
            limit = 100;
        }

        public int GetLimit() {
            return limit;
        }

        public void SetLimit(int l) {
            lock (this) {
                if (!IsInProgress()) {
                    throw new Exception("UndoManager.SetLimit() called after UndoManager.End()");
                }
                limit = l;
                TrimForLimit();
            }
        }
        
        public void DiscardAllEdits() {
            lock (this) {
                foreach (IUndoableEdit e in edits) {
                    e.Die();
                }
                edits = new List<IUndoableEdit>();
            }
        }

        public void UndoOrRedo() {
            lock (this) {
                if (index == edits.Count) {
                    Undo();
                } else {
                    Redo();
                }
            }
        }

        public bool CanUndoOrRedo() {
            lock (this) {
                if (index == edits.Count) {
                    return CanUndo();
                } else {
                    return CanRedo();
                }
            }
        }

        public override void Undo() {
            lock (this) {
                if (IsInProgress()) {
                    IUndoableEdit e = EditToBeUndone();
                    if (e == null) {
                        throw new CannotUndoException();
                    }
                    UndoTo(e);
                } else {
                    base.Undo();
                }
            }
        }

        public override bool CanUndo() {
            lock (this) {
                if (IsInProgress()) {
                    IUndoableEdit e = EditToBeUndone();
                    return e != null && e.CanUndo();
                } else {
                    return base.CanUndo();
                }
            }
        }

        public override void Redo() {
            lock (this) {
                if (IsInProgress()) {
                    IUndoableEdit e = EditToBeUndone();
                    if (e == null) {
                        throw new CannotRedoException();
                    }
                    RedoTo(e);
                } else {
                    base.Redo();
                }
            }
        }

        public override bool CanRedo() {
            lock (this) {
                if (IsInProgress()) {
                    IUndoableEdit e = EditToBeUndone();
                    return e != null && e.CanRedo();
                } else {
                    return base.CanRedo();
                }
            }
        }

        public override bool AddEdit(IUndoableEdit anEdit) {
            lock (this) {
                bool result;
                TrimEdits(index, edits.Count - 1);
                result = base.AddEdit(anEdit);
                if (IsInProgress()) {
                    result = true;
                }
                index = edits.Count;
                TrimForLimit();
                return result;
            }
        }

        public override void End() {
            lock (this) {
                base.End();
                TrimEdits(index, edits.Count - 1);
            }
        }

        public string GetUndoOrRedoPresentationName() {
            lock (this) {
                if (index == edits.Count) {
                    return GetUndoPresentationName();
                } else {
                    return GetRedoPresentationName();
                }
            }
        }

        public override string GetUndoPresentationName() {
            lock (this) {
                if (IsInProgress()) {
                    if (CanUndo()) {
                        return EditToBeUndone().GetUndoPresentationName();
                    } else {
                        return "Undo"; // TODO: i18n
                    }
                } else {
                    return base.GetUndoPresentationName();
                }
            }
        }

        public override string GetRedoPresentationName() {
            lock (this) {
                if (IsInProgress()) {
                    if (CanRedo()) {
                        return EditToBeUndone().GetRedoPresentationName();
                    } else {
                        return "Redo"; // TODO: i18n
                    }
                } else {
                    return base.GetRedoPresentationName();
                }
            }
        }

        #region IUndoableEditListener Members

        public virtual void UndoableEditHappened(UndoableEditEvent e) {
            AddEdit(e.GetEdit());
        }

        #endregion

        protected IUndoableEdit EditToBeUndone() {
            int count = edits.Count;
            for (int i = index; i < count; i++) {
                IUndoableEdit e = edits[i];
                if (e.IsSignificant()) {
                    return e;
                }
            }
            return null;
        }

        protected void UndoTo(IUndoableEdit e) {
            while (true) {
                IUndoableEdit edit = edits[--index];
                edit.Undo();
                if (edit == e) {
                    return;
                }
            }
        }

        protected void RedoTo(IUndoableEdit e) {
            while (true) {
                IUndoableEdit edit = edits[index++];
                edit.Redo();
                if (edit == e) {
                    return;
                }
            }
        }

        protected void TrimEdits(int idx1, int idx2) {
            if (idx1 <= idx2) {
                for (int i = idx2; i >= idx1; --i) {
                    edits[i].Die();
                    edits.RemoveAt(i);
                }
                if (index > idx2) {
                    index -= idx2 - idx1 + 1;
                } else if (index >= idx1) {
                    index = idx1;
                }
            }
        }

        protected void TrimForLimit() {
            if (limit >= 0) {
                int count = edits.Count;
                if (count > limit) {
                    int halfLimit = limit / 2;
                    int keepFrom = index - 1 - halfLimit;
                    int keepTo = index - 1 + halfLimit;

                    if (keepTo - keepFrom + 1 > limit) {
                        keepFrom++;
                    }
                    if (keepFrom < 0) {
                        keepTo -= keepFrom;
                        keepFrom = 0;
                    }
                    if (keepTo >= count) {
                        int delta = count - keepTo - 1;
                        keepTo += delta;
                        keepFrom += delta;
                    }
                    TrimEdits(keepTo + 1, count - 1);
                    TrimEdits(0, keepFrom - 1);
                }
            }
        }
    }
}
