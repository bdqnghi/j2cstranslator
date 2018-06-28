package com.ilog.translator.java2cs.translation.noderewriter;

import org.eclipse.jdt.core.dom.ASTNode;
import org.eclipse.jdt.core.dom.TypeLiteral;
import org.eclipse.text.edits.TextEditGroup;

import com.ilog.translator.java2cs.translation.ITranslationContext;
import com.ilog.translator.java2cs.translation.TranslatorASTRewrite;
import com.ilog.translator.java2cs.translation.util.TranslationUtils;
import com.ilog.translator.java2cs.util.TranslationModelUtil;

public class TypeOfKeywordRewriter extends ElementRewriter {
	//
	//
	//

	public TypeOfKeywordRewriter() {
	}

	//
	//
	//

	@Override
	public void process(ITranslationContext context, ASTNode node,
			TranslatorASTRewrite rew, TranslatorASTRewrite subRewriter,
			TextEditGroup description) {
		final TypeLiteral decl = (TypeLiteral) node;

		final String newName = context.getMapper().getKeyword(
				TranslationModelUtil.TYPEOF_KEYWORD,
				TranslationModelUtil.CSHARP_MODEL);

		final StringBuilder builder = new StringBuilder();
		builder.append(newName + "(");
		builder.append(TranslationUtils.replaceByNewValue(decl.getType(), rew,
				fCu, context.getLogger()));
		builder.append(")");

		final ASTNode placeholder = rew.createStringPlaceholder(builder
				.toString(), ASTNode.TYPE_LITERAL);
		rew.replace(node, placeholder, null);
	}
}
