package com.ilog.translator.java2cs.translation.noderewriter;

import org.eclipse.jdt.core.dom.ASTNode;
import org.eclipse.jdt.core.dom.SynchronizedStatement;
import org.eclipse.text.edits.TextEditGroup;

import com.ilog.translator.java2cs.translation.ITranslationContext;
import com.ilog.translator.java2cs.translation.TranslatorASTRewrite;
import com.ilog.translator.java2cs.translation.util.TranslationUtils;
import com.ilog.translator.java2cs.util.TranslationModelUtil;

public class SynchronizedKeywordRewriter extends ElementRewriter {
	//
	//
	//

	public SynchronizedKeywordRewriter() {
	}

	//
	//
	//

	@Override
	public void process(ITranslationContext context, ASTNode node,
			TranslatorASTRewrite rew, TranslatorASTRewrite subRewriter,
			TextEditGroup description) {
		final SynchronizedStatement decl = (SynchronizedStatement) node;

		final String newName = context.getMapper().getKeyword(
				TranslationModelUtil.SYNCHRONIZED_KEYWORD,
				TranslationModelUtil.CSHARP_MODEL);

		final StringBuilder builder = new StringBuilder();
		builder.append(" " + newName + " (");
		builder.append(TranslationUtils.replaceByNewValue(decl.getExpression(),
				subRewriter, fCu, context.getLogger()));
		builder.append(") ");
		builder.append(TranslationUtils.replaceByNewValue(decl.getBody(), rew,
				fCu, context.getLogger()));

		final ASTNode placeholder = rew.createStringPlaceholder(builder
				.toString(), ASTNode.SYNCHRONIZED_STATEMENT);
		rew.replace(node, placeholder, null);
	}
}
