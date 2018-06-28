package com.ilog.translator.java2cs.translation.noderewriter;

import org.eclipse.jdt.core.dom.ASTNode;
import org.eclipse.jdt.core.dom.PackageDeclaration;
import org.eclipse.text.edits.TextEditGroup;

import com.ilog.translator.java2cs.translation.ITranslationContext;
import com.ilog.translator.java2cs.translation.TranslatorASTRewrite;

public class PackageKeywordRewriter extends AbstractNodeRewriter {

	private final String newName;

	//
	//
	//

	public PackageKeywordRewriter(String newName) {
		this.newName = newName;
	}

	//
	//
	//

	@Override
	public void process(ITranslationContext context, ASTNode node,
			TranslatorASTRewrite rew, TranslatorASTRewrite subRewriter,
			TextEditGroup description) {
		final PackageDeclaration decl = (PackageDeclaration) node;

		final StringBuilder builder = new StringBuilder();
		builder.append(newName);
		builder.append(" ");
		builder.append(decl.getName());
		builder.append(" {");

		final ASTNode placeholder = rew.createStringPlaceholder(builder
				.toString(), ASTNode.PACKAGE_DECLARATION);
		rew.replace(node, placeholder, null);
	}
}
