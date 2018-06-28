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

    public interface IUndoableEdit {
        void Undo(); //throws CannotUndoException;
        bool CanUndo();
        void Redo(); //throws CannotRedoException;
        bool CanRedo();
        void Die();
        bool AddEdit(IUndoableEdit anEdit);
        bool ReplaceEdit(IUndoableEdit anEdit);
        bool IsSignificant();
        string GetPresentationName();
        string GetUndoPresentationName();
        string GetRedoPresentationName();
    }

    public class CannotUndoException : ApplicationException {
        public CannotUndoException() {
        }
        public CannotUndoException(string message) : base(message) {
        }
    }

    public class CannotRedoException : ApplicationException {
        public CannotRedoException() {
        }
        public CannotRedoException(string message) : base(message) {
        }
    }
}
