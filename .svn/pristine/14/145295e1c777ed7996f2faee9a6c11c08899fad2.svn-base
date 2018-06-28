package com.ilog.translator.java2cs.translation.noderewriter;

import org.eclipse.jdt.core.dom.ASTNode;
import org.eclipse.jdt.core.dom.SuperFieldAccess;
import org.eclipse.text.edits.TextEditGroup;

import com.ilog.translator.java2cs.translation.ITranslationContext;
import com.ilog.translator.java2cs.translation.TranslatorASTRewrite;
import com.ilog.translator.java2cs.translation.util.TranslationUtils;
import com.ilog.translator.java2cs.util.TranslationModelUtil;

public class SuperFieldAccessRewriter extends ElementRewriter {


	public SuperFieldAccessRewriter() {
	}

	//
	//
	//

	@Override
	public void process(ITranslationContext context, ASTNode node,
			TranslatorASTRewrite rew, TranslatorASTRewrite subRewriter,
			TextEditGroup description) {
		final SuperFieldAccess decl = (SuperFieldAccess) node;

		final String newName = context.getMapper().getKeyword(
				TranslationModelUtil.SUPER_KEYWORD,
				TranslationModelUtil.CSHARP_MODEL);

		final StringBuilder builder = new StringBuilder();
		builder.append(newName);
		builder.append(".");
		builder.append(TranslationUtils.replaceByNewValue(decl.getName(), rew,
				fCu, context.getLogger()));

		final ASTNode placeholder = rew.createStringPlaceholder(builder
				.toString(), ASTNode.SUPER_METHOD_INVOCATION);
		rew.replace(node, placeholder, null);
	}
}
