package com.ilog.translator.java2cs.translation.noderewriter;

import java.util.ArrayList;

import org.eclipse.jdt.core.dom.AST;
import org.eclipse.jdt.core.dom.ASTNode;
import org.eclipse.jdt.core.dom.PackageDeclaration;
import org.eclipse.jdt.core.dom.QualifiedName;
import org.eclipse.jdt.core.dom.SimpleName;
import org.eclipse.text.edits.TextEditGroup;

import com.ilog.translator.java2cs.translation.ITranslationContext;
import com.ilog.translator.java2cs.translation.TranslatorASTRewrite;
import com.ilog.translator.java2cs.translation.util.DocUtils;
import com.ilog.translator.java2cs.translation.util.TranslationUtils;
import com.ilog.translator.java2cs.util.TranslationModelUtil;

public class PackageRewriter extends AbstractNodeRewriter {

	private final String name;

	//
	//
	//

	public PackageRewriter(String name) {
		this.name = name;
	}

	//
	// package name
	//

	public String getPackageName() {
		return name;
	}

	//
	//
	//

	@Override
	public void process(ITranslationContext context, ASTNode node,
			TranslatorASTRewrite rew, TranslatorASTRewrite subRewriter,
			TextEditGroup description) {
		switch (node.getNodeType()) {
		case ASTNode.QUALIFIED_NAME: /* instanceof QualifiedName ) */
			final QualifiedName pck2 = (QualifiedName) node;
			final AST ast = pck2.getAST();

			if (pck2.getQualifier() instanceof QualifiedName) {
				final QualifiedName newName = (QualifiedName) rew
						.createStringPlaceholder(name, ASTNode.QUALIFIED_NAME);
				rew.replace(pck2.getQualifier(), newName, null);
			} else {
				final SimpleName newName = ast.newSimpleName(name);
				rew.replace(pck2.getQualifier(), newName, null);
			}
			break;
		case ASTNode.PACKAGE_DECLARATION: {
			final String disclaimer = context.getMapper().getDisclaimer();

			if (disclaimer != null && !disclaimer.isEmpty()) {
				final PackageDeclaration pck = (PackageDeclaration) node;

				final String nonJavadoccomments = TranslationUtils
						.getBeginBufferComments((PackageDeclaration) node, fCu,
								context);
				String javaDocComments = "";
				if (pck.getJavadoc() != null) {
					final StringBuffer summary = new StringBuffer();
					final StringBuffer buffer = new StringBuffer();
					DocUtils.applyJavadocReplacement(context,
							new ArrayList<String>(), pck.getJavadoc().tags(),
							summary, buffer, "", null, false);
					if (!buffer.toString().equals("")) {
						javaDocComments = buffer.toString() + "\n";
					}
				}

				String comment = null;

				//try {
					// This retrieve non javadoc comment on top of the buffer
					// but ...
					// I can't remove it so if i add it again in the buffer
					// that comments appear twice.
					//comment = TranslationUtils
					//		.searchForNonJavadocPackageComment(node, fCu);
				/*} catch (JavaModelException e) {

				}*/

				final String keyword = context.getMapper().getKeyword(
						TranslationModelUtil.PACKAGE_KEYWORD,
						TranslationModelUtil.CSHARP_MODEL);
				final StringBuilder builder = new StringBuilder();
				if (disclaimer != null)
					builder.append(disclaimer);
				if (comment != null) {
					builder.append(comment + "\n");
				}
				builder.append(nonJavadoccomments);
				builder.append(javaDocComments);
				builder.append(keyword);
				builder.append(" ");
				builder.append(name);
				builder.append(" {");

				final ASTNode placeholder = rew.createStringPlaceholder(builder
						.toString(), ASTNode.PACKAGE_DECLARATION);
				rew.replace(node, placeholder, null);
								
			}
		}
		}
	}
}
