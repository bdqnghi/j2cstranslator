package com.ilog.translator.java2cs.translation.textrewriter;

import java.util.ArrayList;
import java.util.List;
import java.util.Set;

import org.eclipse.core.runtime.CoreException;
import org.eclipse.core.runtime.IProgressMonitor;
import org.eclipse.jdt.core.IBuffer;
import org.eclipse.jdt.core.IImportDeclaration;
import org.eclipse.jdt.core.dom.ASTNode;
import org.eclipse.jdt.core.dom.ASTVisitor;
import org.eclipse.jdt.core.dom.CompilationUnit;
import org.eclipse.jdt.core.dom.ImportDeclaration;
import org.eclipse.jdt.core.dom.rewrite.ImportRewrite;
import org.eclipse.text.edits.TextEdit;

import com.ilog.translator.java2cs.translation.ITranslationContext;

public class ImportsTransformer extends TextRewriter {

	protected ImportRewrite iRewrite;

	//
	//
	//	

	public ImportsTransformer(ITranslationContext context) {
		super(context);
		transformerName = "Imports";
	}

	//
	//
	//

	@Override
	public boolean transform(IProgressMonitor pm, ASTNode cunit) {
		iRewrite = ImportRewrite.create((CompilationUnit) cunit, true);
		return true;
	}

	//
	//
	//

	public static class SearchForForbidenInitializerVisitor extends ASTVisitor {
		private final List<ImportDeclaration> result = new ArrayList<ImportDeclaration>();;

		@Override
		public boolean visit(ImportDeclaration node) {
			result.add(node);
			return true;
		}

		public List<ImportDeclaration> getResult() {
			return result;
		}
	}

	//
	//
	//

	@Override
	public List<TextEdit> computeEdit(IProgressMonitor pm, IBuffer buffer)
			throws CoreException {
		final List<TextEdit> edits = new ArrayList<TextEdit>();

		final IImportDeclaration[] listImports = context.getImports(fCu);
		if (iRewrite != null) {

			if (listImports != null && listImports.length > 0) {
				for (final IImportDeclaration imp : listImports) {
					iRewrite.removeImport(imp.getElementName());
					//final String n = null; // mapper.mapImport(imp);
					//iRewrite.addImport(n);
				}
			}

			final List<String> ns = new ArrayList<String>();
			String eName = fCu.getElementName();
			if (fCu.getPackageDeclarations() != null
					&& fCu.getPackageDeclarations().length > 0) {
				final String pck = fCu.getPackageDeclarations()[0]
						.getElementName();
				if (pck != null && !pck.equals("")) {
					eName = pck + "." + eName;
				}
			}

			final Set<String> ns1 = context.getNamespaces(eName);
			if (ns1 != null && ns1.size() > 0) {
				ns.addAll(ns1);
			}
			final Set<String> ns2 = context.getDefaultNamespaces();
			if (ns2 != null) {
				ns.addAll(ns2);
			}

			for (final String nspc : ns) {
				iRewrite.addImport(nspc);
			}

			final TextEdit importsEdit = iRewrite.rewriteImports(pm);

			edits.add(importsEdit);

			return edits;
		}
		return null;
	}

}
