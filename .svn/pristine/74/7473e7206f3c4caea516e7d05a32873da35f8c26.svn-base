package com.ilog.translator.java2cs.translation.astrewriter;

import org.eclipse.jdt.core.JavaModelException;
import org.eclipse.jdt.core.dom.ASTNode;
import org.eclipse.jdt.core.dom.AbstractTypeDeclaration;
import org.eclipse.jdt.core.dom.CompilationUnit;
import org.eclipse.jdt.core.dom.PackageDeclaration;
import org.eclipse.jdt.core.dom.SuperMethodInvocation;
import org.eclipse.jdt.core.dom.TypeDeclaration;
import org.eclipse.text.edits.TextEdit;
import org.eclipse.text.edits.TextEditGroup;

import com.ilog.translator.java2cs.configuration.target.TargetClass;
import com.ilog.translator.java2cs.translation.ITranslationContext;
import com.ilog.translator.java2cs.translation.noderewriter.INodeRewriter;
import com.ilog.translator.java2cs.translation.noderewriter.PackageRewriter;
import com.ilog.translator.java2cs.translation.noderewriter.SuperMethodInvocationRewriter;
import com.ilog.translator.java2cs.translation.util.TranslationUtils;

public class ReplaceKeywordVisitor extends ASTRewriterVisitor {

	//
	//
	//

	public ReplaceKeywordVisitor(ITranslationContext context) {
		super(context);
		transformerName = "Replace keyword in class";
		description = new TextEditGroup(transformerName);
	}

	//
	//
	//

	@Override
	public boolean needValidation() {
		return false;
	}

	//
	//
	//

	@Override
	public void endVisit(PackageDeclaration node) {
		INodeRewriter result = null;
		//
		final CompilationUnit cunit = (CompilationUnit) node.getParent();
		final AbstractTypeDeclaration decl = (AbstractTypeDeclaration) cunit
				.types().get(0);
		if (decl.getNodeType() == ASTNode.TYPE_DECLARATION) {
			final TypeDeclaration typeDecl = (TypeDeclaration) decl;
			final TargetClass tc = context.getModel().findClassMapping(
					context.getHandlerFromDoc(typeDecl, false), true, false);
			if (tc != null && tc.isRenamed()) {
				result = new PackageRewriter(tc.getPackageName());
				context.addPackageName(fCu.getPath().toString(), tc
						.getPackageName());
			} else {
				result = context.getMapper().mapPackageDeclaration(node, fCu);
			}
		} else if (decl.getNodeType() == ASTNode.ENUM_DECLARATION) {
			result = context.getMapper().mapPackageDeclaration(node, fCu);
		}
		applyNodeRewriter(node, result, description);
		// 
		// 
		// Have to remove non java doc package comments (already copied in PackageRewriter)
		/*final String disclaimer = context.getMapper().getDisclaimer();
		if (disclaimer != null && !disclaimer.isEmpty()) {
			try {
				TextEdit edit = TranslationUtils
						.removeNonJavadocPackageComment(node, fCu);
				if (edit != null) {
					edits.add(edit);
				}
			} catch (JavaModelException e) {
				context.getLogger().logException("", e);
			}
		}*/
	}

	@Override
	public void endVisit(SuperMethodInvocation node) {
		final INodeRewriter result = new SuperMethodInvocationRewriter();
		applyNodeRewriter(node, result, description);
	}

}
