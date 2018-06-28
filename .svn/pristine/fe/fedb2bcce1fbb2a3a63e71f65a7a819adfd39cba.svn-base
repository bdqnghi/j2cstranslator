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

    public class CompoundEdit : AbstractUndoableEdit {

        private bool inProgress;
        protected List<IUndoableEdit> edits;

        public CompoundEdit() {
            inProgress = true;
            edits = new List<IUndoableEdit>();
        }

        public override void Undo() {
            base.Undo();
            for (int i = edits.Count - 1; i >= 0; --i) {
                edits[i].Undo();
            }
        }

        public override bool CanUndo() {
            return !inProgress && base.CanUndo();
        }

        public override void Redo() {
            base.Redo();
            foreach (IUndoableEdit e in edits) {
                e.Redo();
            }
        }

        public override bool CanRedo() {
            return !inProgress && base.CanRedo();
        }

        public override void Die() {
            for (int i = edits.Count - 1; i >= 0; --i) {
                edits[i].Die();
            }
            base.Die();
        }

        public override bool AddEdit(IUndoableEdit anEdit) {
            if (!inProgress) {
                return false;
            } else {
                IUndoableEdit last = LastEdit();
                if (last == null) {
                    edits.Add(anEdit);
                } else if (!last.AddEdit(anEdit) && anEdit.ReplaceEdit(last)) {
                    edits.RemoveAt(edits.Count - 1);
                }
                return true;
            }
        }

        public virtual void End() {
            inProgress = false;
        }

        public bool IsInProgress() {
            return inProgress;
        }

        public override bool IsSignificant() {
            foreach (IUndoableEdit e in edits) {
                if (e.IsSignificant()) {
                    return true;
                }
            }
            return false;
        }

        public override string GetPresentationName() {
            IUndoableEdit last = LastEdit();
            if (last == null) {
                return base.GetPresentationName();
            } else {
                return last.GetPresentationName();
            }
        }

        public override string GetUndoPresentationName() {
            IUndoableEdit last = LastEdit();
            if (last == null) {
                return base.GetUndoPresentationName();
            } else {
                return last.GetUndoPresentationName();
            }
        }

        public override string GetRedoPresentationName() {
            IUndoableEdit last = LastEdit();
            if (last == null) {
                return base.GetUndoPresentationName();
            } else {
                return last.GetUndoPresentationName();
            }
        }

        protected IUndoableEdit LastEdit() {
            int count = edits.Count;
            if (count > 0) {
                return edits[count - 1];
            } else {
                return null;
            }
        }

    }
}
