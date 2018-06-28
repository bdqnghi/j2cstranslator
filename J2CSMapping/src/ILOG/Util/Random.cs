/*
 * Copyright 1995-2007 Sun Microsystems, Inc.  All Rights Reserved.
 * DO NOT ALTER OR REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.  Sun designates this
 * particular file as subject to the "Classpath" exception as provided
 * by Sun in the LICENSE file that accompanied this code.
 *
 * This code is distributed in the hope that it will be useful, but WITHOUT
 * ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or
 * FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public License
 * version 2 for more details (a copy is included in the LICENSE file that
 * accompanied this code).
 *
 * You should have received a copy of the GNU General Public License version
 * 2 along with this work; if not, write to the Free Software Foundation,
 * Inc., 51 Franklin St, Fifth Floor, Boston, MA 02110-1301 USA.
 *
 * Please contact Sun Microsystems, Inc., 4150 Network Circle, Santa Clara,
 * CA 95054 USA or visit www.sun.com if you need additional information or
 * have any questions.
 */

namespace ILOG.Collections {
	
	//using AtomicLong = System.Collections.Concurrent.Atomic.AtomicLong;
	using ILOG.J2CsMapping.IO;
	using ILOG.J2CsMapping;
	using IOException = System.IO.IOException;
	using IlObjectInputStream = ILOG.J2CsMapping.IO.IlObjectInputStream;
	using IlObjectOutputStream = ILOG.J2CsMapping.IO.IlObjectOutputStream;
	//using ObjectStreamField = System.IO.ObjectStreamField;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.IO;
	using System.Runtime.CompilerServices;
	using System.Runtime.Serialization;
	using System.Xml;
	using System;
	//using Unsafe = Sun.Misc.Unsafe;
	
	/// <summary>
	/// An instance of this class is used to generate a stream of pseudorandom
	/// numbers. The class uses a 48-bit seed, which is modified using a linear
	/// congruential formula. (See Donald Knuth, <i>The Art of Computer Programming,
	/// Volume 3</i>, Section 3.2.1.)
	/// <p>
	/// If two instances of {@code Random} are created with the same seed, and the
	/// same sequence of method calls is made for each, they will generate and return
	/// identical sequences of numbers. In order to guarantee this property,
	/// particular algorithms are specified for the class {@code Random}. Java
	/// implementations must use all the algorithms shown here for the class{@code Random}, for the sake of absolute portability of Java code. However,
	/// subclasses of class {@code Random} are permitted to use other algorithms, so
	/// long as they adhere to the general contracts for all the methods.
	/// <p>
	/// The algorithms implemented by class {@code Random} use a {@code protected}utility method that on each invocation can supply up to 32 pseudorandomly
	/// generated bits.
	/// <p>
	/// Many applications will find the method {@link Math#random} simpler to use.
	/// </summary>
	///
	/// @author Frank Yellin
	/// @since 1.0
	[Serializable]
	public class Random {
		static Random() {
				/*try {
					seedOffset = unsaf0.ObjectFieldOffset(typeof(Random)
							.GetField("seed"));
				} catch (Exception ex) {
					throw new Exception(ex);
				}*/
			}
		/// <summary>
		/// use serialVersionUID from JDK 1.1 for interoperability
		/// </summary>
		///
		internal const long serialVersionUID = 3905348978240129619L;
	
		/// <summary>
		/// The internal state associated with this pseudorandom number generator.
		/// (The specs for the methods in this class describe the ongoing computation
		/// of this value.)
		/// </summary>
		///
		private long /*AtomicLong*/ seed;
	
		private const long multiplier = 0x5DEECE66DL;
		private const long addend = 0xBL;
		private const long mask = (1L << 48) - 1;
	
		/// <summary>
		/// Creates a new random number generator. This constructor sets the seed of
		/// the random number generator to a value very likely to be distinct from
		/// any other invocation of this constructor.
		/// </summary>
		///
		public Random() : this(++seedUniquifier + DateTime.Now.Ticks) {
		}
	
		private static long seedUniquifier = 8682522807148012L;
	
		/// <summary>
		/// Creates a new random number generator using a single {@code long} seed.
		/// The seed is the initial value of the internal state of the pseudorandom
		/// number generator which is maintained by method {@link #next}.
		/// <p>
		/// The invocation {@code new Random(seed)} is equivalent to:
		/// <pre>
		/// {
		/// &#064;code
		/// Random rnd = new Random();
		/// rnd.setSeed(seed);
		/// }
		/// </pre>
		/// </summary>
		///
		/// <param name="seed_0">the initial seed</param>
		/// @see #setSeed(long)
		public Random(long seed_0) {
			this.haveNextNextGaussian = false;
			this.seed = /*new AtomicLong(*/0x0L/*)*/;
			SetSeed(seed_0);
		}
	
		/// <summary>
		/// Sets the seed of this random number generator using a single {@code long}seed. The general contract of {@code setSeed} is that it alters the state
		/// of this random number generator object so as to be in exactly the same
		/// state as if it had just been created with the argument {@code seed} as a
		/// seed. The method {@code setSeed} is implemented by class {@code Random}by atomically updating the seed to
		/// <pre>{@code (seed &circ; 0x5DEECE66DL) &amp; ((1L &lt;&lt; 48) - 1)}</pre>
		/// and clearing the {@code haveNextNextGaussian} flag used by {@link #nextGaussian}.
		/// <p>
		/// The implementation of {@code setSeed} by class {@code Random} happens to
		/// use only 48 bits of the given seed. In general, however, an overriding
		/// method may use all 64 bits of the {@code long} argument as a seed value.
		/// </summary>
		///
		/// <param name="seed_0">the initial seed</param>
		[MethodImpl(MethodImplOptions.Synchronized)]
		public void SetSeed(long seed_0) {
			seed_0 = (seed_0 ^ multiplier) & mask;
			this.seed = seed_0;
			haveNextNextGaussian = false;
		}
	
		/// <summary>
		/// Generates the next pseudorandom number. Subclasses should override this,
		/// as this is used by all other methods.
		/// <p>
		/// The general contract of {@code next} is that it returns an {@code int}value and if the argument {@code bits} is between {@code 1} and{@code 32} (inclusive), then that many low-order bits of the returned
		/// value will be (approximately) independently chosen bit values, each of
		/// which is (approximately) equally likely to be {@code 0} or {@code 1}.
		/// The method {@code next} is implemented by class {@code Random} by
		/// atomically updating the seed to
		/// <pre>{@code (seed/// 0x5DEECE66DL + 0xBL) &amp; ((1L &lt;&lt; 48) - 1)}</pre>
		/// and returning
		/// <pre>{@code (int)(seed &gt;&gt;&gt; (48 - bits))}.
		/// </pre>
		/// This is a linear congruential pseudorandom number generator, as defined
		/// by D. H. Lehmer and described by Donald E. Knuth in <i>The Art of
		/// Computer Programming,</i> Volume 3: <i>Seminumerical Algorithms</i>,
		/// section 3.2.1.
		/// </summary>
		///
		/// <param name="bits">random bits</param>
		/// <returns>the next pseudorandom value from this random number generator's</returns>
		/// @since 1.1
		protected internal int Next(int bits) {
			long oldseed, nextseed;
			long seed_0 = this.seed;
			do {
				oldseed = seed_0; //.Get();
				nextseed = (oldseed * multiplier + addend) & mask;
            } while (!CompareAndSet(out seed_0, oldseed, nextseed));
			return (int) (ILOG.J2CsMapping.Util.MathUtil.URS(nextseed,(48 - bits)));
		}

        private bool CompareAndSet(out long seed, long oldSeed, long nextSeed)
        {
            bool cmp = (oldSeed < nextSeed);
            if (cmp)
            {
                seed = nextSeed;
            }
            else
            {
                seed = oldSeed;
            }
            return cmp;
        }
	
		public void NextBytes(sbyte[] bytes) {
			for (int i = 0, len = bytes.Length; i < len;)
				for (int rnd = NextInt(), n = Math.Min(len - i, 32 /*System.Int32.SIZE*/ / 8 /*System.Byte.SIZE*/); n-- > 0; rnd >>= 8 /*System.Byte.SIZE*/)
					bytes[i++] = (sbyte) rnd;
		}
	
		public int NextInt() {
			return Next(32);
		}
	
		public int NextInt(int n) {
			if (n <= 0)
				throw new ArgumentException("n must be positive");
	
			if ((n & -n) == n) // i.e., n is a power of 2
				return (int) ((n * (long) Next(31)) >> 31);
	
			int bits, val;
			do {
				bits = Next(31);
				val = bits % n;
			} while (bits - val + (n - 1) < 0);
			return val;
		}
	
		public long NextLong() {
			// it's okay that the bottom word remains signed.
			return ((long) (Next(32)) << 32) + Next(32);
		}
	
		public bool NextBoolean() {
			return Next(1) != 0;
		}
	
		public float NextFloat() {
			return Next(24) / ((float) (1 << 24));
		}
	
		public double NextDouble() {
			return (((long) (Next(26)) << 27) + Next(27)) / (double) (1L << 53);
		}
	
		private double nextNextGaussian;
		private bool haveNextNextGaussian;
	
		public double NextGaussian() {
			// See Knuth, ACP, Section 3.4.1 Algorithm C.
			if (haveNextNextGaussian) {
				haveNextNextGaussian = false;
				return nextNextGaussian;
			} else {
				double v1, v2, s;
				do {
					v1 = 2 * NextDouble() - 1; // between -1 and 1
					v2 = 2 * NextDouble() - 1; // between -1 and 1
					s = v1 * v1 + v2 * v2;
				} while (s >= 1 || s == 0);
				double multiplier_0 = Math.Sqrt(-2
						* Math.Log(s) / s);
				nextNextGaussian = v2 * multiplier_0;
				haveNextNextGaussian = true;
				return v1 * multiplier_0;
			}
		}
	
		/// <summary>
		/// Serializable fields for Random.
		/// </summary>
		///
		/// @serialField seed long seed for random computations
		/// @serialField nextNextGaussian double next Gaussian to be returned
		/// @serialField haveNextNextGaussian boolean nextNextGaussian is valid
		/*private static readonly ObjectStreamField[] serialPersistentFields = {
				new ObjectStreamField("seed", typeof(Int64)),
				new ObjectStreamField("nextNextGaussian", typeof(Double)),
				new ObjectStreamField("haveNextNextGaussian",
						typeof(Boolean)) };
	*/
		/// <summary>
		/// Reconstitute the {@code Random} instance from a stream (that is,
		/// deserialize it).
		/// </summary>
		///
		private void ReadObject(IlObjectInputStream s) {
	
			/*ObjectInputStream.GetField fields = s.ReadFields();
	
			// The seed is read in as {@code long} for
			// historical reasons, but it is converted to an AtomicLong.
			long seedVal = (long) fields.Get("seed", -1L);
			if (seedVal < 0)
				throw new System.IO.StreamCorruptedException("Random: invalid seed");
			ResetSeed(seedVal);
			nextNextGaussian = fields.Get("nextNextGaussian", 0.0d);
			haveNextNextGaussian = fields.Get("haveNextNextGaussian", false);*/
		}
	
		/// <summary>
		/// Save the {@code Random} instance to a stream.
		/// </summary>
		///
		[MethodImpl(MethodImplOptions.Synchronized)]
		private void WriteObject(IlObjectOutputStream s) {
	/*
			// set the values of the Serializable fields
			ObjectOutputStream.PutField fields = s.PutFields();
	
			// The seed is serialized as a long for historical reasons.
			fields.Put("seed", seed.Get());
			fields.Put("nextNextGaussian", nextNextGaussian);
			fields.Put("haveNextNextGaussian", haveNextNextGaussian);
	
			// save them
			s.WriteFields();*/
		}
	
		// Support for resetting seed while deserializing
		//private static readonly Unsafe unsaf0 = Sun.Misc.Unsafe.GetUnsafe();
		private static readonly long seedOffset;
	
		private void ResetSeed(long seedVal) {
			//unsaf0.PutObjectVolatile(this, seedOffset, new AtomicLong(seedVal));
		}
	}
}
