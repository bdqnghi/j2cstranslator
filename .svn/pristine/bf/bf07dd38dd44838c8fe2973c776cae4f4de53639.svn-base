package com.ilog.translator.java2cs.translation.astrewriter;

import java.util.ArrayList;
import java.util.List;

import org.eclipse.core.runtime.IProgressMonitor;
import org.eclipse.core.runtime.NullProgressMonitor;
import org.eclipse.core.runtime.SubProgressMonitor;
import org.eclipse.jdt.core.ICompilationUnit;
import org.eclipse.jdt.core.IJavaElement;
import org.eclipse.jdt.core.dom.AST;
import org.eclipse.jdt.core.dom.ASTNode;
import org.eclipse.jdt.core.dom.ASTParser;
import org.eclipse.jdt.core.dom.CompilationUnit;
import org.eclipse.jdt.core.dom.IMethodBinding;
import org.eclipse.jdt.core.dom.MethodDeclaration;
import org.eclipse.jdt.core.dom.MethodInvocation;
import org.eclipse.jdt.core.dom.SuperMethodInvocation;
import org.eclipse.jdt.core.search.IJavaSearchConstants;
import org.eclipse.jdt.core.search.SearchMatch;
import org.eclipse.jdt.core.search.SearchPattern;
import org.eclipse.jdt.internal.corext.refactoring.RefactoringScopeFactory;
import org.eclipse.jdt.internal.corext.refactoring.RefactoringSearchEngine2;
import org.eclipse.jdt.internal.corext.refactoring.SearchResultGroup;
import org.eclipse.jdt.internal.corext.refactoring.structure.ASTNodeSearchUtil;
import org.eclipse.jdt.internal.corext.util.SearchUtils;
import org.eclipse.text.edits.TextEditGroup;

import com.ilog.translator.java2cs.translation.ITranslationContext;
import com.ilog.translator.java2cs.translation.util.TranslationUtils;
import com.ilog.translator.java2cs.util.TranslationModelUtil;

/**
 * Find covariance 
 * 
 * 
 * @author afau
 *
 */
public class CovarianceFinderVisitor2 extends ASTRewriterVisitor {

	//
	//
	//

	public CovarianceFinderVisitor2(ITranslationContext context) {
		super(context);
		transformerName = "Find Covariance 2";
		description = new TextEditGroup(transformerName);
	}

	//
	//
	//	

	@Override
	public boolean visit(MethodDeclaration node) {
		final IMethodBinding mb = node.resolveBinding();
		//
		if (isCovariant(node, mb)) {
			addReferences(mb);
		}
		return true;
	}

	//
	//
	//

	private void addReferences(IMethodBinding mb) {
		final List<SearchMatch> results = new ArrayList<SearchMatch>();
		try {
			final IJavaElement element = mb.getJavaElement();
			final RefactoringSearchEngine2 engine = new RefactoringSearchEngine2(
					SearchPattern.createPattern(element,
							IJavaSearchConstants.REFERENCES,
							SearchUtils.GENERICS_AGNOSTIC_MATCH_RULE));
			// engine.setOwner(fCu.getW);
			engine.setFiltering(true, true);
			engine.setScope(RefactoringScopeFactory.create(element));
			engine.searchPattern(new NullProgressMonitor());
			final SearchResultGroup[] res = (SearchResultGroup[]) engine
					.getResults();

			for (final SearchResultGroup r : res) {
				final SearchMatch[] searchResults = r.getSearchResults();
				for (final SearchMatch element0 : searchResults) {
					final IJavaElement referenceToMember = (IJavaElement) element0
							.getElement();
					//
					final ICompilationUnit icuNode = (ICompilationUnit) referenceToMember
							.getAncestor(IJavaElement.COMPILATION_UNIT);
					if (icuNode != null) {
						final CompilationUnit cuNode = parse(
								new NullProgressMonitor(), icuNode, true, false);
						final ASTNode node = ASTNodeSearchUtil.findNode(
								element0, cuNode);
						switch (node.getNodeType()) {
						case ASTNode.METHOD_INVOCATION:
							final MethodInvocation methodI = (MethodInvocation) node;
							final IMethodBinding mb2 = methodI
									.resolveMethodBinding();
							if (mb.isEqualTo(mb2)) {
								results.add(element0);
							}
							break;
						case ASTNode.SUPER_METHOD_INVOCATION:
							final SuperMethodInvocation smethodI = (SuperMethodInvocation) node;
							final IMethodBinding mb3 = smethodI
									.resolveMethodBinding();
							if (mb.isEqualTo(mb3)) {
								results.add(element0);
							}
							break;
						default:
							context.getLogger().logInfo(
									"Collector Unexpected node type " + node);
						}
					}
				}
			}

			context.addCovariance(results, mb.getReturnType()
					.getQualifiedName());
		} catch (final Exception e) {
			e.printStackTrace();
			context.getLogger().logException("", e);
		}
	}

	private CompilationUnit parse(IProgressMonitor pm, ICompilationUnit icunit,
			boolean validation, boolean abridged) {
		final IProgressMonitor monitor = new SubProgressMonitor(pm, 1);
		final ASTParser parser = ASTParser.newParser(AST.JLS3);
		parser.setProject(icunit.getJavaProject());
		parser.setSource(icunit);
		if (abridged) {
			parser.setFocalPosition(0);
		}
		parser.setResolveBindings(validation);
		final CompilationUnit result = (CompilationUnit) parser
				.createAST(monitor);
		monitor.done();
		return result;
	}

	private boolean isCovariant(MethodDeclaration node, IMethodBinding mb) {
		return TranslationUtils.getTagValueFromDoc(node, context.getMapper()
				.getTag(TranslationModelUtil.COVARIANCE_TAG)) != null;
	}

}
