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
using System.Collections.Specialized;

namespace ILOG.J2CsMapping.Collections.Generics
{
    /// <summary>
    /// A collection that contains no duplicate elements.  More formally, sets
    /// contain no pair of elements <c>e1</c> and <c>e2</c> such that
    /// <c>e1.Equals(e2)</c>, and at most one null element.  As implied by
    /// its name, this interface models the mathematical <i>set</i> abstraction.
    /// <p>The <tt>ISet</tt> interface places additional stipulations, beyond those
    /// inherited from the <tt>ICollection</tt> interface, on the contracts of all
    /// constructors and on the contracts of the <tt>Add</tt>, <tt>Equals</tt> and
    /// <tt>GetHashCode</tt> methods.  Declarations for other inherited methods are
    /// also included here for convenience.  (The specifications accompanying these
    /// declarations have been tailored to the <tt>ISet</tt> interface, but they do
    /// not contain any additional stipulations.)
    /// <p>The additional stipulation on constructors is, not surprisingly,
    /// that all constructors must create a set that contains no duplicate elements
    /// (as defined above).
    /// <p>Note: Great care must be exercised if mutable objects are used as set
    /// elements.  The behavior of a set is not specified if the value of an object
    /// is changed in a manner that affects <tt>Equals</tt> comparisons while the
    /// object is an element in the set.  A special case of this prohibition is
    /// that it is not permissible for a set to contain itself as an element.
    /// <p>Some set implementations have restrictions on the elements that
    /// they may contain.  For example, some implementations prohibit null elements,
    /// and some have restrictions on the types of their elements.  Attempting to
    /// add an ineligible element throws an unchecked exception, typically
    /// <tt>NullReferenceException</tt> or <tt>ClassCastException</tt>.  Attempting
    /// to query the presence of an ineligible element may throw an exception,
    /// or it may simply return false; some implementations will exhibit the former
    /// behavior and some will exhibit the latter.  More generally, attempting an
    /// operation on an ineligible element whose completion would not result in
    /// the insertion of an ineligible element into the set may throw an
    /// exception or it may succeed, at the option of the implementation.
    /// Such exceptions are marked as "optional" in the specification for this
    /// interface.
    /// </summary>
    ///
    /// <typeparam name="T">the type of elements maintained by this set</param>
    /// 
    /// <seealso cref="T:ICollection"/>
	public interface ISet<T> : IExtendedCollection<T> {    
	}
}
