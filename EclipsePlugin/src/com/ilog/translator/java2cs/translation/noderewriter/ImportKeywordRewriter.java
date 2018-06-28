package com.ilog.translator.java2cs.translation.noderewriter;

import java.util.ArrayList;
import java.util.Collections;
import java.util.List;
import java.util.Set;
import java.util.TreeSet;

import org.eclipse.jdt.core.dom.ASTNode;
import org.eclipse.jdt.core.dom.CompilationUnit;
import org.eclipse.jdt.core.dom.rewrite.ListRewrite;
import org.eclipse.text.edits.TextEditGroup;

import com.ilog.translator.java2cs.translation.ITranslationContext;
import com.ilog.translator.java2cs.translation.TranslatorASTRewrite;

public class ImportKeywordRewriter extends AbstractNodeRewriter {

	private final String newKeywordName;
	private final Set<String> newImportList;
	private boolean emptyBuilder = false;

	//
	//
	//

	public ImportKeywordRewriter(String newKeywordName,
			Set<String> newImportList) {
		this.newKeywordName = newKeywordName;
		this.newImportList = newImportList;
	}

	//
	//
	//

	@SuppressWarnings("unchecked")
	@Override
	public void process(ITranslationContext context, ASTNode node,
			TranslatorASTRewrite rew, TranslatorASTRewrite subRewriter,
			TextEditGroup description) {
		final ListRewrite importsRewrite = rew.getListRewrite(node,
				CompilationUnit.IMPORTS_PROPERTY);

		final List imports = importsRewrite.getOriginalList();

		for (final Object obj : imports) {
			final ASTNode nodeImport = (ASTNode) obj;
			importsRewrite.remove(nodeImport, null);
		}

		boolean useGlobal = context.getConfiguration().getOptions()
				.isUseGlobal();
		boolean useAlias = context.getConfiguration().getOptions().isUseAlias();
		String globaleString = (useGlobal) ? "global::" : "";

		Set<String> newImports = new TreeSet<String>();
		for (String nspc : this.newImportList) {
			int idx = nspc.lastIndexOf(".");
			if (!context.ignorablePackage(nspc.substring(0, idx), null /*
																		 * TODO:
																		 * check
																		 * !!!
																		 */)) {
				final StringBuilder builder = new StringBuilder();
				builder.append(newKeywordName);
				builder.append(" ");
				if (!nspc.endsWith(".*")) {
					if (nspc.contains("<")) {
						final int lastGenerics = nspc.lastIndexOf(">");
						final String newns = nspc.substring(0, nspc
								.indexOf("<"));
						if (lastGenerics < nspc.length()) {
							idx = newns.lastIndexOf(".");
						}
						if (!newImportList.contains(newns.substring(0, idx)
								+ ".*")) {
							builder.append(globaleString
									+ newns.substring(0, idx));
						} else {
							continue;
						}
					} else if (context.isGenericType(nspc)) {
						if (!newImportList.contains(nspc.substring(0, idx)
								+ ".*")) {
							builder.append(globaleString
									+ nspc.substring(0, idx));
						} else {
							continue;
						}
					} else {
						final String className = nspc.substring(idx + 1);
						final String pck = nspc.substring(0, idx);
						// Filter for : XX = System.XX
						if (pck.equals("System"))
							continue;
						if (!useAlias) {
							if (!this.newImportList.contains(pck)) {
								builder.append(globaleString + pck);
							} else {
								emptyBuilder = true;
							}
						} else {
							// was
							builder.append(className + " = ");
							builder.append(nspc);
						}
					}
				} else {
					final String shortForm = nspc.substring(0,
							nspc.length() - 2);
					if (shortForm.contains("<")) {
						final int lastGenerics = shortForm.lastIndexOf(">");
						final String newns = shortForm.substring(0, shortForm
								.indexOf("<"));
						if (lastGenerics < nspc.length()) {
							idx = newns.lastIndexOf(".");
						}
						if (!newImportList.contains(newns.substring(0, idx)
								+ ".*")) {
							builder.append(globaleString
									+ newns.substring(0, idx));
						} else {
							continue;
						}
					} else
						builder.append(globaleString + nspc.substring(0, idx));
				}
				if (!emptyBuilder) {
					newImports.add(builder.toString());
				}
			}
		}

		List<String> orderedImports = new ArrayList<String>();
		orderedImports.addAll(newImports);
		Collections.sort(orderedImports);

		for (final String anImport : newImports) {
			final ASTNode newNode = rew.createStringPlaceholder(anImport + ";",
					ASTNode.IMPORT_DECLARATION);
			importsRewrite.insertLast(newNode, null);
		}
	}
}
