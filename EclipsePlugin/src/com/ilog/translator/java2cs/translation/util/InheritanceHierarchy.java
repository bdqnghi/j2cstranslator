package com.ilog.translator.java2cs.translation.util;

import java.io.Serializable;
import java.util.ArrayList;
import java.util.BitSet;
import java.util.List;

import org.eclipse.core.runtime.NullProgressMonitor;
import org.eclipse.jdt.core.IType;
import org.eclipse.jdt.core.JavaModelException;

/*
 * TODO: To be checked, but not use at all (despite call to method)
 * 
 */
class InheritanceHierarchy implements Serializable {
	/**
	 * 
	 */
	private static final long serialVersionUID = -5420903878391685696L;

	// used for indexing classes
	private final List<IType> classes = new ArrayList<IType>();

	private transient BitSet[] inheritanceHierarchy = null;

	public int addClass(IType clazz) {
		classes.add(clazz);
		return classes.size() - 1;
	}

	public void removeClass(IType clazz) {
		// clear cache
		inheritanceHierarchy = null;
		classes.set(getIndex(clazz), null);
	}

	public void updateSuperClasses(IType clazz) {
		final int classIndex = getIndex(clazz);
		if (inheritanceHierarchy != null
				&& inheritanceHierarchy.length > classIndex
				&& inheritanceHierarchy[classIndex] != null) {
			inheritanceHierarchy[classIndex] = null; // to be recalculated
			final int count = inheritanceHierarchy.length;
			for (int i = 0; i < count; i++) {
				final BitSet bitSet = inheritanceHierarchy[i];
				if (bitSet != null && bitSet.get(classIndex)) {
					inheritanceHierarchy[i] = null; // to be recalculated
				}
			}
		}
	}

	public boolean isSubclassOf(int subClassIndex, int classIndex) {
		final BitSet bitSet = getBitSet(subClassIndex);
		return bitSet.get(classIndex);
	}

	public boolean isSubclassOf(IType subClass, IType clazz) {
		int subClassIndex = getIndex(subClass);
		if (subClassIndex == -1) {
			subClassIndex = addClass(subClass);
			System.out.println("adding class " + subClass.getElementName());
		}
		int classIndex = getIndex(clazz);
		if (classIndex == -1) {
			classIndex = addClass(clazz);
			System.out.println("adding class " + clazz.getElementName());
		}
		return this.isSubclassOf(subClassIndex, classIndex);
	}

	/**
	 * Computes the lower upper bound
	 */
	public IType lub(List<IType> subTypes) {
		int classIndex = getIndex(subTypes.get(0));
		final BitSet lub = (BitSet) getBitSet(classIndex).clone();
		lub.set(classIndex);
		for (int i = 1; i < subTypes.size(); i++) {
			classIndex = getIndex(subTypes.get(i));
			final BitSet bitSet2 = (BitSet) getBitSet(classIndex).clone();
			bitSet2.set(classIndex); // add itself
			lub.and(bitSet2);
		}

		for (int i = 0; i < lub.length(); i++) {
			if (lub.get(i)) {
				lub.andNot(getBitSet(i)); // suppress parents of each
				// potential lub
			}
		}

		if (lub.length() == 0) {
			return null; // an empty intersection means there are no lub
		}

		for (int i = 0; i < lub.length(); i++) {
			if (lub.get(i)) {
				final IType clazz = classes.get(i); // return the first one
				// which is
				// a class
				try {
					if (clazz.isClass()) {
						return clazz;
					}
				} catch (final JavaModelException e) {
					e.printStackTrace();
					return null;
				}
			}
		}
		return classes.get(lub.length() - 1); // todo this may be incorrect.
	}

	/**
	 * Compute the lower upper bound
	 * 
	 * @param class1
	 * @param class2
	 */
	public IType lub(IType class1, IType class2) {
		return this.lub(getIndex(class1), getIndex(class2));
	}

	/**
	 * Computes the lower upper bound
	 * 
	 * @param classIndex1
	 * @param classIndex2
	 */
	public IType lub(int classIndex1, int classIndex2) {
		final BitSet lub = computeLub(classIndex1, classIndex2);
		if (lub.length() == 0) {
			return null; // an empty intersection means there are no lub
		}

		for (int i = 0; i < lub.length(); i++) {
			if (lub.get(i)) {
				final IType clazz = classes.get(i); // return the first one
				// which is
				// a class
				try {
					if (clazz.isClass()) {
						return clazz;
					}
				} catch (final JavaModelException e) {
					e.printStackTrace();
					return null;
				}
			}
		}
		return classes.get(lub.length() - 1); // todo this may be incorrect.
	}

	private final BitSet computeLub(int classIndex1, int classIndex2) {
		final BitSet bitSet1 = (BitSet) getBitSet(classIndex1).clone();
		bitSet1.set(classIndex1); // add itself
		final BitSet bitSet2 = (BitSet) getBitSet(classIndex2).clone();
		bitSet2.set(classIndex2); // add itself
		bitSet1.and(bitSet2);
		for (int i = 0; i < bitSet1.length(); i++) {
			if (bitSet1.get(i)) {
				bitSet1.andNot(getBitSet(i)); // suppress parents of each
				// potential lub
			}
		}
		return bitSet1;

	}

	final BitSet getBitSet(int classIndex) {
		BitSet bitSet = null;
		if (inheritanceHierarchy == null) {
			inheritanceHierarchy = new BitSet[classes.size()];
		} else if (classIndex < inheritanceHierarchy.length) {
			bitSet = inheritanceHierarchy[classIndex];
		}
		if (bitSet == null) {
			final IType clazz = classes.get(classIndex);
			if (clazz != null) {
				bitSet = computeInheritanceHierarchy(clazz, classIndex,
						getSuperclasses(clazz));
			}
		}
		return bitSet;
	}

	private List<IType> getSuperclasses(IType clazz) {
		try {
			final IType[] supers = clazz.newSupertypeHierarchy(
					new NullProgressMonitor()).getAllClasses();
			final List<IType> res = new ArrayList<IType>();
			for (final IType element : supers) {
				res.add(element);
			}
			return res;
		} catch (final JavaModelException e) {
			e.printStackTrace();
			return null;
		}
	}

	private void resizeInheritanceHierarchy() {
		// todo a more scalable resizing
		final BitSet[] tmp = new BitSet[classes.size()];
		System.arraycopy(inheritanceHierarchy, 0, tmp, 0,
				inheritanceHierarchy.length);
		inheritanceHierarchy = tmp;
	}

	private BitSet computeInheritanceHierarchy(IType clazz, int classIndex,
			List<IType> superClasses) {
		if (classIndex >= inheritanceHierarchy.length) {
			resizeInheritanceHierarchy();
		}
		final BitSet bitSet = new BitSet(classes.size());
		inheritanceHierarchy[classIndex] = bitSet;

		try {
			if (clazz.getTypeParameters().length > 0) {

			}
		} catch (final JavaModelException e) {
			e.printStackTrace();
		}
		/*
		 * if (!IlrMeta.CSHARP) { IType.IlrGenericClassInfo genericClassInfo =
		 * clazz.getGenericInfo(); if (genericClassInfo != null) { // a generic
		 * class is a subclass of its raw type IType rawClass = (IType)
		 * genericClassInfo.getRawClass(); BitSet rawBitSet =
		 * computeSuperInheritanceHierarchy(classIndex, rawClass); if (rawBitSet !=
		 * null) { bitSet.set(rawClass.classIndex); bitSet.or(rawBitSet); } } }
		 */

		for (final IType superClass : superClasses) {
			final BitSet superBitSet = computeSuperInheritanceHierarchy(
					classIndex, superClass);
			if (superBitSet != null) {
				bitSet.set(getIndex(superClass));
				bitSet.or(superBitSet);
			}
		}
		return bitSet;
	}

	private BitSet computeSuperInheritanceHierarchy(int classIndex,
			IType superClass) {
		if (getIndex(superClass) < 0) {
			return null; // ignore!
		}
		if (getIndex(superClass) == classIndex) {
			System.out.println("InheritanceHierarchy:"
					+ superClass.getFullyQualifiedName()
					+ " Loop In Inheritance Tree");
			return null;
		}
		BitSet superBitSet = null;
		if (getIndex(superClass) < inheritanceHierarchy.length) {
			superBitSet = inheritanceHierarchy[getIndex(superClass)];
		}
		if (superBitSet == null) {
			superBitSet = computeInheritanceHierarchy(superClass,
					getIndex(superClass), getSuperclasses(superClass));
		}
		if (superBitSet.get(classIndex)) {
			System.out.println("InheritanceHierarchy:"
					+ superClass.getFullyQualifiedName()
					+ " Loop In Inheritance Tree");
			return null;
		}
		return superBitSet;
	}

	public int getIndex(IType clazz) {
		return classes.indexOf(clazz);
	}
}
