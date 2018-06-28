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
using System.IO;

namespace ILOG.J2CsMapping.IO
{
	/// <summary>
    /// Replacement for FilterInputStream
	/// </summary>
	public class IlFilterInputStream : Stream
	{
        protected Stream ins0;

        /// <summary>
        /// 
        /// </summary>
		public IlFilterInputStream(Stream stream)
		{
            this.ins0 = stream;
		}

        public override bool CanRead
        {
            get { return ins0.CanRead; }
        }

        public override bool CanSeek
        {
            get { return ins0.CanSeek; }
        }

        public override bool CanWrite
        {
            get { return ins0.CanWrite; }
        }

        public override void Flush()
        {
            throw new NotImplementedException();
        }

        public override long Length
        {
            get { return ins0.Length; }
        }

        public override long Position
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotImplementedException();
        }

        public override void SetLength(long value)
        {
            throw new NotImplementedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }
    }
}
