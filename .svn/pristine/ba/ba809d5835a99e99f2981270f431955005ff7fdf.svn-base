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

    public class AbstractUndoableEdit : IUndoableEdit {

        protected const string UndoName = "Undo";
        protected const string RedoName = "Redo";

        private bool dead;
        private bool canUndo;

        public AbstractUndoableEdit() {
            canUndo = true;
        }

        #region IUndoableEdit Members

        public virtual void Undo() {
            if (!CanUndo()) {
                throw new CannotUndoException();
            }
            canUndo = false;
        }

        public virtual bool CanUndo() {
            return !dead && canUndo;
        }

        public virtual void Redo() {
            if (!CanRedo()) {
                throw new CannotRedoException();
            }
            canUndo = true;
        }

        public virtual bool CanRedo() {
            return !dead && !canUndo;
        }

        public virtual void Die() {
            dead = true;
        }

        public virtual bool AddEdit(IUndoableEdit anEdit) {
            return false;
        }

        public bool ReplaceEdit(IUndoableEdit anEdit) {
            return false;
        }

        public virtual bool IsSignificant() {
            return true;
        }

        public virtual string GetPresentationName() {
            return "";
        }

        public virtual string GetUndoPresentationName() {
            string name = GetPresentationName();
            // TODO: i18n
            if (string.IsNullOrEmpty(name)) {
                name = "Undo";
            } else {
                name = "Undo " + name;
            }
            return name;
        }

        public virtual string GetRedoPresentationName() {
            string name = GetPresentationName();
            // TODO: i18n
            if (string.IsNullOrEmpty(name)) {
                name = "Redo";
            } else {
                name = "Redo " + name;
            }
            return name;
        }

        #endregion
    }
}
