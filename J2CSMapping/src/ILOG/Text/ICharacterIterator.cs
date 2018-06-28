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

namespace ILOG.J2CsMapping.Text
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class CharacterIterator : ICharacterIterator
    {
        public const char Done = '\uFFFF';

        #region ICharacterIterator Members

        public char First()
        {
            throw new NotImplementedException();
        }

        public char Last()
        {
            throw new NotImplementedException();
        }

        public char Current()
        {
            throw new NotImplementedException();
        }

        public char Next()
        {
            throw new NotImplementedException();
        }

        public char Previous()
        {
            throw new NotImplementedException();
        }

        public char SetIndex(int position)
        {
            throw new NotImplementedException();
        }

        public int GetBeginIndex()
        {
            throw new NotImplementedException();
        }

        public int GetEndIndex()
        {
            throw new NotImplementedException();
        }

        public int GetIndex()
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    public interface ICharacterIterator
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        char First();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        char Last();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        char Current();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        char Next();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        char Previous();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        char SetIndex(int position);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        int GetBeginIndex();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        int GetEndIndex();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        int GetIndex();
    }
}
