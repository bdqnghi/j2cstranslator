package com.ilog.translator.java2cs.translation.astrewriter.astchange;

/*******************************************************************************
 * Copyright (c) 2000, 2006 IBM Corporation and others.
 * All rights reserved. This program and the accompanying materials
 * are made available under the terms of the Eclipse Public License v1.0
 * which accompanies this distribution, and is available at
 * http://www.eclipse.org/legal/epl-v10.html
 *
 * Contributors:
 *     IBM Corporation - initial API and implementation
 *******************************************************************************/
import java.util.ArrayList;
import java.util.Arrays;
import java.util.Collection;
import java.util.List;

import org.eclipse.core.resources.IResource;
import org.eclipse.core.runtime.Assert;
import org.eclipse.core.runtime.IProgressMonitor;
import org.eclipse.core.runtime.SubProgressMonitor;
import org.eclipse.jdt.core.ICompilationUnit;
import org.eclipse.jdt.core.IMethod;
import org.eclipse.jdt.core.IType;
import org.eclipse.jdt.core.ITypeHierarchy;
import org.eclipse.jdt.core.JavaModelException;
import org.eclipse.jdt.core.WorkingCopyOwner;
import org.eclipse.jdt.core.dom.AST;
import org.eclipse.jdt.core.dom.ASTNode;
import org.eclipse.jdt.core.dom.AbstractTypeDeclaration;
import org.eclipse.jdt.core.dom.Block;
import org.eclipse.jdt.core.dom.ClassInstanceCreation;
import org.eclipse.jdt.core.dom.CompilationUnit;
import org.eclipse.jdt.core.dom.MethodDeclaration;
import org.eclipse.jdt.core.dom.ParameterizedType;
import org.eclipse.jdt.core.dom.SuperConstructorInvocation;
import org.eclipse.jdt.core.dom.Type;
import org.eclipse.jdt.core.search.IJavaSearchConstants;
import org.eclipse.jdt.core.search.IJavaSearchScope;
import org.eclipse.jdt.core.search.SearchEngine;
import org.eclipse.jdt.core.search.SearchMatch;
import org.eclipse.jdt.core.search.SearchPattern;
import org.eclipse.jdt.internal.corext.dom.ASTNodes;
import org.eclipse.jdt.internal.corext.refactoring.RefactoringScopeFactory;
import org.eclipse.jdt.internal.corext.refactoring.RefactoringSearchEngine;
import org.eclipse.jdt.internal.corext.refactoring.SearchResultGroup;
import org.eclipse.jdt.internal.corext.refactoring.structure.ASTNodeSearchUtil;
import org.eclipse.jdt.internal.corext.refactoring.util.JavaElementUtil;
import org.eclipse.jdt.internal.corext.refactoring.util.RefactoringASTParser;
import org.eclipse.jdt.internal.corext.util.JdtFlags;
import org.eclipse.jdt.internal.corext.util.SearchUtils;
import org.eclipse.ltk.core.refactoring.RefactoringStatus;

/**
 * This class is used to find references to constructors.
 */
class ConstructorReferenceFinder {
	private final IType fType;

	private final IMethod[] fConstructors;

	private ConstructorReferenceFinder(IType type) throws JavaModelException {
		fConstructors = JavaElementUtil.getAllConstructors(type);
		fType = type;
	}

	private ConstructorReferenceFinder(IMethod constructor) {
		fConstructors = new IMethod[] { constructor };
		fType = constructor.getDeclaringType();
	}

	public static SearchResultGroup[] getConstructorReferences(IType type,
			IProgressMonitor pm, RefactoringStatus status)
			throws JavaModelException {
		return new ConstructorReferenceFinder(type).getConstructorReferences(
				pm, null, IJavaSearchConstants.REFERENCES, status);
	}

	public static SearchResultGroup[] getConstructorReferences(IType type,
			WorkingCopyOwner owner, IProgressMonitor pm,
			RefactoringStatus status) throws JavaModelException {
		return new ConstructorReferenceFinder(type).getConstructorReferences(
				pm, owner, IJavaSearchConstants.REFERENCES, status);
	}

	public static SearchResultGroup[] getConstructorOccurrences(
			IMethod constructor, IProgressMonitor pm, RefactoringStatus status)
			throws JavaModelException {
		Assert.isTrue(constructor.isConstructor());
		return new ConstructorReferenceFinder(constructor)
				.getConstructorReferences(pm, null,
						IJavaSearchConstants.ALL_OCCURRENCES, status);
	}

	private SearchResultGroup[] getConstructorReferences(IProgressMonitor pm,
			WorkingCopyOwner owner, int limitTo, RefactoringStatus status)
			throws JavaModelException {
		final IJavaSearchScope scope = createSearchScope();
		final SearchPattern pattern = RefactoringSearchEngine.createOrPattern(
				fConstructors, limitTo);
		if (pattern == null) {
			if (fConstructors.length != 0) {
				return new SearchResultGroup[0];
			}
			return getImplicitConstructorReferences(pm, owner, status);
		}
		return removeUnrealReferences(RefactoringSearchEngine.search(pattern,
				owner, scope, pm, status));
	}

	// XXX this method is a workaround for jdt core bug 27236

	@SuppressWarnings("unchecked")
	private SearchResultGroup[] removeUnrealReferences(
			SearchResultGroup[] groups) {
		final List result = new ArrayList(groups.length);
		for (final SearchResultGroup group : groups) {
			final ICompilationUnit cu = group.getCompilationUnit();
			if (cu == null) {
				continue;
			}
			final CompilationUnit cuNode = new RefactoringASTParser(AST.JLS3)
					.parse(cu, false);
			final SearchMatch[] allSearchResults = group.getSearchResults();
			final List realConstructorReferences = new ArrayList(Arrays
					.asList(allSearchResults));
			for (final SearchMatch searchResult : allSearchResults) {
				if (!isRealConstructorReferenceNode(ASTNodeSearchUtil
						.getAstNode(searchResult, cuNode))) {
					realConstructorReferences.remove(searchResult);
				}
			}
			if (!realConstructorReferences.isEmpty()) {
				result
						.add(new SearchResultGroup(
								group.getResource(),
								(SearchMatch[]) realConstructorReferences
										.toArray(new SearchMatch[realConstructorReferences
												.size()])));
			}
		}
		return (SearchResultGroup[]) result
				.toArray(new SearchResultGroup[result.size()]);
	}

	// XXX this method is a workaround for jdt core bug 27236
	private boolean isRealConstructorReferenceNode(ASTNode node) {
		final String typeName = fConstructors[0].getDeclaringType()
				.getElementName();
		if (node.getParent() instanceof AbstractTypeDeclaration
				&& ((AbstractTypeDeclaration) node.getParent())
						.getNameProperty().equals(node.getLocationInParent())) {
			// Example:
			// class A{
			// A(){}
			// }
			// class B extends A {}
			// ==> "B" is found as reference to A()
			return false;
		}
		if (node.getParent() instanceof MethodDeclaration
				&& MethodDeclaration.NAME_PROPERTY.equals(node
						.getLocationInParent())) {
			final MethodDeclaration md = (MethodDeclaration) node.getParent();
			if (md.isConstructor()
					&& !md.getName().getIdentifier().equals(typeName)) {
				// Example:
				// class A{
				// A(){}
				// }
				// class B extends A{
				// B(){}
				// }
				// ==> "B" in "B(){}" is found as reference to A()
				return false;
			}
		}
		return true;
	}

	private IJavaSearchScope createSearchScope() throws JavaModelException {
		if (fConstructors.length == 0) {
			return RefactoringScopeFactory.create(fType);
		}
		return RefactoringScopeFactory.create(getMostVisibleConstructor());
	}

	private IMethod getMostVisibleConstructor() throws JavaModelException {
		Assert.isTrue(fConstructors.length > 0);
		IMethod candidate = fConstructors[0];
		final int visibility = JdtFlags.getVisibilityCode(fConstructors[0]);
		for (int i = 1; i < fConstructors.length; i++) {
			final IMethod constructor = fConstructors[i];
			if (JdtFlags.isHigherVisibility(JdtFlags
					.getVisibilityCode(constructor), visibility)) {
				candidate = constructor;
			}
		}
		return candidate;
	}

	@SuppressWarnings("unchecked")
	private SearchResultGroup[] getImplicitConstructorReferences(
			IProgressMonitor pm, WorkingCopyOwner owner,
			RefactoringStatus status) throws JavaModelException {
		pm.beginTask("", 2); //$NON-NLS-1$
		final List searchMatches = new ArrayList();
		searchMatches.addAll(getImplicitConstructorReferencesFromHierarchy(
				owner, new SubProgressMonitor(pm, 1)));
		searchMatches.addAll(getImplicitConstructorReferencesInClassCreations(
				owner, new SubProgressMonitor(pm, 1), status));
		pm.done();
		return RefactoringSearchEngine.groupByCu((SearchMatch[]) searchMatches
				.toArray(new SearchMatch[searchMatches.size()]), status);
	}

	// List of SearchResults
	@SuppressWarnings("unchecked")
	private List getImplicitConstructorReferencesInClassCreations(
			WorkingCopyOwner owner, IProgressMonitor pm,
			RefactoringStatus status) throws JavaModelException {
		// XXX workaround for jdt core bug 23112
		final SearchPattern pattern = SearchPattern.createPattern(fType,
				IJavaSearchConstants.REFERENCES,
				SearchUtils.GENERICS_AGNOSTIC_MATCH_RULE);
		final IJavaSearchScope scope = RefactoringScopeFactory.create(fType);
		final SearchResultGroup[] refs = RefactoringSearchEngine.search(
				pattern, owner, scope, pm, status);
		final List result = new ArrayList();
		for (final SearchResultGroup group : refs) {
			final ICompilationUnit cu = group.getCompilationUnit();
			if (cu == null) {
				continue;
			}
			final CompilationUnit cuNode = new RefactoringASTParser(AST.JLS3)
					.parse(cu, false);
			final SearchMatch[] results = group.getSearchResults();
			for (final SearchMatch searchResult : results) {
				final ASTNode node = ASTNodeSearchUtil.getAstNode(searchResult,
						cuNode);
				if (ConstructorReferenceFinder
						.isImplicitConstructorReferenceNodeInClassCreations(node)) {
					result.add(searchResult);
				}
			}
		}
		return result;
	}

	public static boolean isImplicitConstructorReferenceNodeInClassCreations(
			ASTNode node) {
		if (node instanceof Type) {
			final ASTNode parent = node.getParent();
			if (parent instanceof ClassInstanceCreation) {
				return (node.equals(((ClassInstanceCreation) parent).getType()));
			} else if (parent instanceof ParameterizedType) {
				final ASTNode grandParent = parent.getParent();
				if (grandParent instanceof ClassInstanceCreation) {
					final ParameterizedType type = (ParameterizedType) ((ClassInstanceCreation) grandParent)
							.getType();
					return (node.equals(type.getType()));
				}
			}
		}
		return false;
	}

	// List of SearchResults
	@SuppressWarnings("unchecked")
	private List getImplicitConstructorReferencesFromHierarchy(
			WorkingCopyOwner owner, IProgressMonitor pm)
			throws JavaModelException {
		final IType[] subTypes = ConstructorReferenceFinder
				.getNonBinarySubtypes(owner, fType, pm);
		final List result = new ArrayList(subTypes.length);
		for (final IType element : subTypes) {
			result.addAll(ConstructorReferenceFinder
					.getAllSuperConstructorInvocations(element));
		}
		return result;
	}

	@SuppressWarnings("unchecked")
	private static IType[] getNonBinarySubtypes(WorkingCopyOwner owner,
			IType type, IProgressMonitor monitor) throws JavaModelException {
		ITypeHierarchy hierarchy = null;
		if (owner == null) {
			hierarchy = type.newTypeHierarchy(monitor);
		} else {
			hierarchy = type.newSupertypeHierarchy(owner, monitor);
		}
		final IType[] subTypes = hierarchy.getAllSubtypes(type);
		final List result = new ArrayList(subTypes.length);
		for (final IType element : subTypes) {
			if (!element.isBinary()) {
				result.add(element);
			}
		}
		return (IType[]) result.toArray(new IType[result.size()]);
	}

	// Collection of SearchResults
	@SuppressWarnings("unchecked")
	private static Collection getAllSuperConstructorInvocations(IType type)
			throws JavaModelException {
		final IMethod[] constructors = JavaElementUtil.getAllConstructors(type);
		final CompilationUnit cuNode = new RefactoringASTParser(AST.JLS3)
				.parse(type.getCompilationUnit(), false);
		final List result = new ArrayList(constructors.length);
		for (final IMethod element : constructors) {
			final ASTNode superCall = ConstructorReferenceFinder
					.getSuperConstructorCallNode(element, cuNode);
			if (superCall != null) {
				result.add(ConstructorReferenceFinder.createSearchResult(
						superCall, element));
			}
		}
		return result;
	}

	private static SearchMatch createSearchResult(ASTNode superCall,
			IMethod constructor) {
		final int start = superCall.getStartPosition();
		final int end = ASTNodes.getInclusiveEnd(superCall); // TODO: why
																// inclusive?
		final IResource resource = constructor.getResource();
		return new SearchMatch(constructor, SearchMatch.A_ACCURATE, start, end
				- start, SearchEngine.getDefaultSearchParticipant(), resource);
	}

	@SuppressWarnings("unchecked")
	private static SuperConstructorInvocation getSuperConstructorCallNode(
			IMethod constructor, CompilationUnit cuNode)
			throws JavaModelException {
		Assert.isTrue(constructor.isConstructor());
		final MethodDeclaration constructorNode = ASTNodeSearchUtil
				.getMethodDeclarationNode(constructor, cuNode);
		Assert.isTrue(constructorNode.isConstructor());
		final Block body = constructorNode.getBody();
		Assert.isNotNull(body);
		final List statements = body.statements();
		if (!statements.isEmpty()
				&& statements.get(0) instanceof SuperConstructorInvocation) {
			return (SuperConstructorInvocation) statements.get(0);
		}
		return null;
	}
}
