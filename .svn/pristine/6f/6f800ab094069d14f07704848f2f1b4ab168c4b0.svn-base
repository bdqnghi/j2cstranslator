package com.ilog.translator.java2cs.translation.astrewriter;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

import org.eclipse.core.runtime.IProgressMonitor;
import org.eclipse.core.runtime.NullProgressMonitor;
import org.eclipse.core.runtime.SubProgressMonitor;
import org.eclipse.jdt.core.Flags;
import org.eclipse.jdt.core.ICompilationUnit;
import org.eclipse.jdt.core.IJavaElement;
import org.eclipse.jdt.core.IMethod;
import org.eclipse.jdt.core.IType;
import org.eclipse.jdt.core.JavaCore;
import org.eclipse.jdt.core.JavaModelException;
import org.eclipse.jdt.core.Signature;
import org.eclipse.jdt.core.dom.AST;
import org.eclipse.jdt.core.dom.ASTParser;
import org.eclipse.jdt.core.dom.CompilationUnit;
import org.eclipse.jdt.core.dom.IMethodBinding;
import org.eclipse.jdt.core.dom.MethodDeclaration;
import org.eclipse.jdt.core.dom.TypeDeclaration;
import org.eclipse.jdt.core.search.IJavaSearchConstants;
import org.eclipse.jdt.core.search.IJavaSearchScope;
import org.eclipse.jdt.internal.ui.search.JavaSearchQuery;
import org.eclipse.jdt.internal.ui.search.JavaSearchResult;
import org.eclipse.jdt.internal.ui.search.JavaSearchScopeFactory;
import org.eclipse.jdt.ui.search.ElementQuerySpecification;
import org.eclipse.jdt.ui.search.QuerySpecification;
import org.eclipse.jface.text.IRegion;
import org.eclipse.jface.text.Position;
import org.eclipse.jface.text.Region;
import org.eclipse.search.ui.text.Match;
import org.eclipse.search2.internal.ui.InternalSearchUI;
import org.eclipse.search2.internal.ui.text.PositionTracker;
import org.eclipse.text.edits.TextEditGroup;

import com.ilog.translator.java2cs.translation.ITranslationContext;
import com.ilog.translator.java2cs.translation.util.HandlerUtil;
import com.ilog.translator.java2cs.translation.util.TranslationUtils;

/**
 * 
 * @author afau
 * 
 */
public class ChainOfCallVisitor extends ASTRewriterVisitor {

	//
	//
	//

	public ChainOfCallVisitor(ITranslationContext context) {
		super(context);
		transformerName = "Chain of call";
		description = new TextEditGroup(transformerName);
	}

	//
	//
	//

	@Override
	public void reset() {
		super.reset();
		methoddecl = null;
		alreadyVisited = new HashMap<String, Object>();
	}

	//
	//
	//

	IMethod methoddecl = null;

	//
	//
	//

	@Override
	public void endVisit(MethodDeclaration node) {
		if (monitoredMethod(node)) {
			alreadyVisited = new HashMap<String, Object>();
			methoddecl = (IMethod) node.resolveBinding().getJavaElement();
			try {
				performNewSearch(methoddecl, new ArrayList<String>(),
						new ArrayList<String>());
				//

			} catch (final Exception e) {
				e.printStackTrace();
			}
		}
	}

	private boolean monitoredMethod(MethodDeclaration node) {
		final IType type = (IType) node.resolveBinding().getDeclaringClass()
				.getJavaElement();
		final String name = type.getFullyQualifiedName();
		if (name.equals("ilog.rules.engine.IlrNetwork") /*
		 * &&
		 * node.getName().getIdentifier().equals("addRuleInternal")
		 */) {
			return true;
		}
		if (name.equals("ilog.rules.engine.IlrEngine") /*
		 * &&
		 * node.getName().getIdentifier().equals("addToNetwork")
		 */) {
			return true;
		}
		return false;
	}

	private Map<String, Object> alreadyVisited = new HashMap<String, Object>();

	private boolean performNewSearch(IMethod element, List<String> chain,
			List<String> lineNumberChain) throws JavaModelException,
			InterruptedException {
		//

		if (isPublicDocumentedMethod(element)) {
			printSolution(element, chain, lineNumberChain);
			return true;
		}
		//
		if (chain.size() > 100) {
			context.getLogger().logDebug("   --- abort chain too long ");
			return false;
		}
		//
		final JavaSearchQuery query = new JavaSearchQuery(createQuery(element));
		final NullProgressMonitor npm = new NullProgressMonitor();
		query.run(npm);
		final JavaSearchResult res = (JavaSearchResult) query.getSearchResult();
		final Object[] eleme = res.getElements();
		for (final Object e : eleme) {
			if (e instanceof IMethod) {
				final IMethod caller = (IMethod) e;

				if (isAnEnginePackage(caller)) {
					Object hasSolution = alreadyVisited.get(caller
							.getHandleIdentifier());
					if ((hasSolution == null || ((Boolean) hasSolution))
							&& !chain.contains(caller.getHandleIdentifier())) {
						chain.add(caller.getHandleIdentifier());
						//
						/*
						 * String lines = ""; Match[] matches =
						 * res.getMatches(caller); for (Match m : matches) {
						 * 
						 * int offset = m.getOffset(); IRegion ir =
						 * getCurrentMatchLocation(m); lines += offset + " "; }
						 * 
						 * lineNumberChain.add(lines);
						 */
						//
						hasSolution = performNewSearch(caller, chain,
								lineNumberChain);
						alreadyVisited.put(caller.getHandleIdentifier(),
								hasSolution);
						//
						chain.remove(chain.size() - 1);
						// lineNumberChain.remove(lineNumberChain.size() - 1);
					}
				}
			}
		}
		return false;
	}

	public IRegion getCurrentMatchLocation(Match match) {
		final PositionTracker tracker = InternalSearchUI.getInstance()
				.getPositionTracker();

		int offset, length;
		final Position pos = tracker.getCurrentPosition(match);
		if (pos == null) {
			offset = match.getOffset();
			length = match.getLength();
		} else {
			offset = pos.getOffset();
			length = pos.getLength();
		}
		return new Region(offset, length);
	}

	private boolean isAnEnginePackage(IMethod caller) {
		return caller.getDeclaringType().getPackageFragment().getElementName()
				.startsWith("ilog.rules.engine");
	}

	private boolean isPublicDocumentedMethod(IMethod element)
			throws JavaModelException {
		final String fqClassName = element.getDeclaringType()
				.getFullyQualifiedName();
		final String ilrcontext = "ilog.rules.engine.IlrContext";
		//final String ilrruleset = "ilog.rules.engine.IlrRuleset";

		boolean internal = false;

		if (fqClassName.equals(ilrcontext) /* || fqClassName.equals(ilrruleset) */) {
			final String javaDoc = element
					.getAttachedJavadoc(new NullProgressMonitor());
			if (javaDoc == null) {
				final ICompilationUnit unit = element.getDeclaringType()
						.getCompilationUnit();
				final NullProgressMonitor npm = new NullProgressMonitor();
				final CompilationUnit parsedUnit = parse(npm, unit, true,
						false, false);
				if (parsedUnit != null) {
					final TypeDeclaration tdecl = (TypeDeclaration) parsedUnit
							.types().get(0);
					final MethodDeclaration[] mdecl = tdecl.getMethods();
					for (final MethodDeclaration md : mdecl) {
						final IMethodBinding mb = md.resolveBinding();
						if (mb != null
								&& mb.getJavaElement().getHandleIdentifier()
										.equals(element.getHandleIdentifier())) {
							//final Javadoc jdoc = md.getJavadoc();
							internal = TranslationUtils.containsTag(md,
									"@internal");
						}
					}
				}
			}
			return Flags.isPublic(element.getFlags()) && !internal;
		}
		return false;
	}

	public CompilationUnit parse(IProgressMonitor pm, ICompilationUnit icunit,
			boolean validation, boolean abridged, boolean recovery) {
		final IProgressMonitor monitor = new SubProgressMonitor(pm, 1);
		final ASTParser parser = ASTParser.newParser(AST.JLS3);
		parser.setProject(icunit.getJavaProject());
		parser.setSource(icunit);
		if (abridged) {
			parser.setFocalPosition(0);
		}
		parser.setResolveBindings(validation);
		parser.setStatementsRecovery(recovery);
		final CompilationUnit result = (CompilationUnit) parser
				.createAST(monitor);
		monitor.done();
		return result;
	}

	private void printSolution(IMethod element, List<String> chain,
			List<String> lineNumerChain) throws JavaModelException {
		final String javaDoc = element
				.getAttachedJavadoc(new NullProgressMonitor());

		if ((javaDoc == null)
				|| (javaDoc != null && !javaDoc.contains("@internal"))) {
			context.getLogger().logDebug(
					"Chain of call for "
							+ methoddecl.getDeclaringType()
									.getFullyQualifiedName() + "."
							+ methoddecl.getElementName() + " signature : "
							+ Signature.toString(methoddecl.getSignature())
							+ " : ");
			for (int i = 0; i < chain.size(); i++) {
				final String l = chain.get(i);
				final IMethod c = (IMethod) HandlerUtil.createElementFromHandler(l);
				String fqnName = "   +  ";
				if (c != null) {
					if (c.getDeclaringType() != null) {
						fqnName += c.getDeclaringType().getFullyQualifiedName()
								+ ".";
					}
					fqnName += c.getElementName() + " signature : "
							+ Signature.toString(c.getSignature())
					/* + " line(s) " + lineNumerChain.get(i) */;
					context.getLogger().logDebug(fqnName);
				} else {
					context.getLogger().logDebug(
							fqnName + "<can't find java element : " + l + ">");
				}
			}
			context.getLogger().logDebug("");
		}
	}

	QuerySpecification createQuery(IJavaElement element)
			throws JavaModelException, InterruptedException {
		final JavaSearchScopeFactory factory = JavaSearchScopeFactory
				.getInstance();
		final boolean isInsideJRE = factory.isInsideJRE(element);

		final IJavaSearchScope scope = factory
				.createWorkspaceScope(isInsideJRE);
		final String description = factory
				.getWorkspaceScopeDescription(isInsideJRE);
		return new ElementQuerySpecification(element,
				IJavaSearchConstants.REFERENCES, scope, description);
	}
}
