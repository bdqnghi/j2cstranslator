package com.ilog.translator.java2cs.translation.astrewriter;

import java.util.ArrayList;
import java.util.List;
import java.util.Set;
import java.util.TreeSet;

import org.eclipse.core.resources.IProject;
import org.eclipse.core.runtime.CoreException;
import org.eclipse.core.runtime.IProgressMonitor;
import org.eclipse.core.runtime.NullProgressMonitor;
import org.eclipse.jdt.core.IImportDeclaration;
import org.eclipse.jdt.core.IPackageDeclaration;
import org.eclipse.jdt.core.IType;
import org.eclipse.jdt.core.JavaModelException;
import org.eclipse.jdt.core.dom.AST;
import org.eclipse.jdt.core.dom.ASTNode;
import org.eclipse.jdt.core.dom.CompilationUnit;
import org.eclipse.jdt.core.dom.TypeDeclaration;
import org.eclipse.ltk.core.refactoring.Change;
import org.eclipse.text.edits.TextEditGroup;

import com.ilog.translator.java2cs.configuration.info.PackageInfo;
import com.ilog.translator.java2cs.configuration.target.TargetClass;
import com.ilog.translator.java2cs.configuration.target.TargetPackage;
import com.ilog.translator.java2cs.translation.ITranslationContext;
import com.ilog.translator.java2cs.translation.TranslatorASTRewrite;
import com.ilog.translator.java2cs.translation.noderewriter.ImportKeywordRewriter;
import com.ilog.translator.java2cs.translation.noderewriter.INodeRewriter;
import com.ilog.translator.java2cs.translation.util.TranslationUtils;
import com.ilog.translator.java2cs.util.TranslationModelUtil;

public class ImportsRewriterVisitor extends ASTRewriterVisitor {

	//
	//
	//

	public ImportsRewriterVisitor(ITranslationContext context) {
		super(context);
		transformerName = "Imports Rewriter";
		description = new TextEditGroup(transformerName);
	}

	//
	//
	//

	@Override
	public boolean isAbridged() {
		return true;
	}

	//
	//
	//

	@Override
	public boolean applyChange(IProgressMonitor pm) throws CoreException {
		final Change change = createChange(pm, null);
		if (change != null) {
			context.addChange(fCu, change);
		}
		return true;
	}

	//
	//
	//

	@SuppressWarnings("unchecked")
	private boolean needJunit(CompilationUnit cuint) {
		final List types = cuint.types();
		if (types != null && types.size() > 0) {
			if (types.get(0) instanceof TypeDeclaration) {
				final TypeDeclaration typeDecl = (TypeDeclaration) types.get(0);
				if (TranslationUtils.containsTag(typeDecl, context.getModel()
						.getTag(TranslationModelUtil.TESTCASE_TAG))) {
					return true;
				}
			}
		}
		return false;
	}

	@Override
	public boolean transform(IProgressMonitor pm, ASTNode cunit) {
		final IImportDeclaration[] listImports = context.getImports(fCu);

		String currentPck = null;
		try {
			final IPackageDeclaration[] pDecl = fCu.getPackageDeclarations();
			if ((pDecl != null) && (pDecl.length > 0)) {
				final String thisPck = pDecl[0].getElementName();
				final TargetPackage tPackage = context.getModel()
						.findPackageMapping(thisPck,
								pDecl[0].getJavaProject().getProject());
				if (tPackage != null) {
					currentPck = tPackage.getName();
				} else {
					currentPck = TranslationUtils.defaultMappingForPackage(
							context, thisPck, pDecl[0].getJavaProject()
									.getProject());
				}
			}

			final TargetClass tClass = context.getModel().findClassMapping(
					fCu.findPrimaryType().getHandleIdentifier(), true, false);
			if (tClass != null) {
				if (tClass.isRenamed()) {
					currentPck = tClass.getPackageName();
				}
			}

			Set<String> ns = new TreeSet<String>();

			if ((listImports != null) && (listImports.length > 0)) {
				for (final IImportDeclaration imp : listImports) {
					final String importName = imp.getElementName();
					final int index = importName.lastIndexOf(".");
					if (index > 0) {
						String className = importName
							.substring(index + 1);		
						// TODO:
						final IType itype = fCu.getJavaProject().findType(
								importName, new NullProgressMonitor());
						final String p = context.getMapper().mapImport(
								importName,
								imp.isOnDemand(),
								itype == null ? null : itype.getJavaProject()
										.getProject());
						String id = importName;
						if (itype != null)
							id = itype.getHandleIdentifier();
						final TargetClass tc = context.getModel()
								.findClassMapping(id, false, true);
						boolean removeFromImport = false;
						if (tc != null) {
							removeFromImport = tc.isTranslated() && tc.isRemoveFromImport();
							if (removeFromImport) {
								continue;
							}
						}
						if (p != null) {
							final String pck = importName.substring(0, index);
							final int index2 = importName.lastIndexOf(".*");
							IProject project = imp.getJavaProject()
							.getProject();
							if (index2 == -1) {													
								if (!isInIgnoredPackage(pck, project)
										&& !toBeRemovedFromImportClass(className, pck, project, removeFromImport)) {
									ns.add(p);
								}
							} else if ((index2 >= 0)
									|| !isInIgnoredPackage(pck, project)) {
								ns.add(p);
							}

						} else {
							context.getLogger().logWarning(
									transformerName + " Warning, package "
											+ importName + " has no mapping.");
						}
					}

				}
			}

			String eName = fCu.getElementName();
			if ((fCu.getPackageDeclarations() != null)
					&& (fCu.getPackageDeclarations().length > 0)) {
				final String pck = fCu.getPackageDeclarations()[0]
						.getElementName();
				if ((pck != null) && !pck.equals("")) {
					eName = pck + "." + eName;
				}
			}

			Set<String> ns1 = context.getNamespaces(eName);
			if ((ns1 != null) && (ns1.size() > 0)) {
				ns.addAll(ns1);
			}
			final Set<String> ns2 = context.getDefaultNamespaces();
			ns.addAll(ns2);

			final List<String> genImport = context.getGenericImports(fCu);
			if (genImport != null) {
				ns.addAll(genImport);
			}

			if (needJunit((CompilationUnit) cunit)) {
				ns.add("NUnit.Framework");
			}
			
			// ADD OR REMOVE USING    --> 
			if(tClass != null){
				if (tClass.getChangeUsingDescriptor() != null) {
	              for ( String removeUsing : tClass.getChangeUsingDescriptor().getUsingToRemove()) {
	                System.out.println( "[Custom Info] Remove using of " + removeUsing + " from " + tClass.getName() );
	              }
	              for ( String addUsing : tClass.getChangeUsingDescriptor().getUsingToAdd() ) {
	                System.out.println( "[Custom Info] Add using of " + addUsing + " into " + tClass.getName() );
	              }				
				
	              ns.removeAll( tClass.getChangeUsingDescriptor().getUsingToRemove() );
	              ns.addAll( tClass.getChangeUsingDescriptor().getUsingToAdd() );
	              ArrayList<String> searchWithSuffix = new ArrayList<String>();
	              for ( String removeUsing :  tClass.getChangeUsingDescriptor().getUsingToRemove() ) {
	                searchWithSuffix.add( removeUsing + ".*" );
	              }
	              ns.removeAll( searchWithSuffix );
				}
			}
			// ADD OR REMOVE USING    <-- 
			
			if (ns.size() > 0) {
				if (currentRewriter == null) {
					final AST ast = cunit.getAST();
					currentRewriter = TranslatorASTRewrite.create(ast);
				}

				Set<String> listOfImport = ns;
				if (currentPck != null) {
					listOfImport = filterForForbiddenImport(ns, currentPck);
				}

				final INodeRewriter rewriter = new ImportKeywordRewriter(context
						.getMapper().getKeyword(
								TranslationModelUtil.IMPORT_KEYWORD,
								TranslationModelUtil.CSHARP_MODEL),
						listOfImport);
				rewriter.setICompilationUnit(fCu);
				rewriter.process(context, cunit, currentRewriter, null,
						description);
			}
			ns = null;
			ns1 = null;

		} catch (final JavaModelException e) {
			context.getLogger().logException(
					"Java Model Error on compilation unit "
							+ fCu.getElementName(), e);
			e.printStackTrace();
		} catch (final Exception e) {
			context.getLogger().logException(
					"Error on compilation unit " + fCu.getElementName(), e);
			e.printStackTrace();
		}

		context.cleanImports(fCu);

		return true;
	}

	private boolean toBeRemovedFromImportClass(String className, String pck, IProject project, boolean removeFromImport ) {
		return removeFromImport && context
				.getModel()
				.isRemovedOrInnerClass(className,
						pck,
						project);
	}

	//
	//
	//

	private boolean isInIgnoredPackage(String pck, IProject reference) {
		/*if (context.ignorablePackage(pck, reference)) {
			return true;
		}*/
		PackageInfo pckInfo = context.getModel().findPackageInfo(pck, reference);
		String targetFramework = context.getConfiguration().getOptions().getTargetDotNetFramework().name();
		
		if (pckInfo != null && pckInfo.getTarget(targetFramework) != null && pckInfo.getTarget(targetFramework).isRemoveFromImport()) {
			return true;
		}
		int index = -1;
		while ((index = pck.lastIndexOf(".")) > 0) {
			pck = pck.substring(0, index);
			pckInfo = context.getModel().findPackageInfo(pck, reference);
			if (pckInfo != null && pckInfo.getTarget(targetFramework) != null) {
				return pckInfo.getTarget(targetFramework).isRemoveFromImport();
			}
			/*
			if (context.ignorablePackage(pck, reference)) {
				return true;
			}
			*/
		}
		return false;
	}

	private Set<String> filterForForbiddenImport(Set<String> ns,
			String currentNS) {
		final Set<String> list = new TreeSet<String>();
		for (final String nspc : ns) {
			if (!nspc.endsWith(".*")) {
				final int idx = nspc.lastIndexOf(".");
				if (idx > 0) {
					final String importNS = nspc.substring(0, idx);
					if (!importNS.equals(currentNS)) {
						try {
							final IType type = context.getConfiguration()
									.getWorkingProject().findType(nspc);
							if (type != null) {
								if (!type.isMember()) {
									// Only Top level class can be used in
									// a using on demand form.
									list.add(nspc);
								}
							} else {
								// Well just in case ...
								list.add(nspc);
							}
						} catch (final JavaModelException e) {
							e.printStackTrace();
							context.getLogger().logException("", e);
						}
					}
				}
			} else if (!nspc.equals(currentNS + ".*")) {
				list.add(nspc);
			}
		}
		return list;
	}
}
