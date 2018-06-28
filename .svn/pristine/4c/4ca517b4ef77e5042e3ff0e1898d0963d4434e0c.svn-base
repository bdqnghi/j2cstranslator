package com.ilog.translator.java2cs.translation.noderewriter;

import org.eclipse.jdt.core.dom.ASTNode;
import org.eclipse.jdt.core.dom.InstanceofExpression;
import org.eclipse.text.edits.TextEditGroup;

import com.ilog.translator.java2cs.translation.ITranslationContext;
import com.ilog.translator.java2cs.translation.TranslatorASTRewrite;
import com.ilog.translator.java2cs.translation.util.TranslationUtils;
import com.ilog.translator.java2cs.util.TranslationModelUtil;

public class InstanceofKeywordRewriter extends AbstractNodeRewriter {

	public InstanceofKeywordRewriter() {
	}

	//
	//
	//

	@Override
	public void process(ITranslationContext context, ASTNode node,
			TranslatorASTRewrite rew, TranslatorASTRewrite subRewriter,
			TextEditGroup description) {
		final InstanceofExpression decl = (InstanceofExpression) node;

		final String newName = context.getMapper().getKeyword(
				TranslationModelUtil.INSTANCEOF_KEYWORD,
				TranslationModelUtil.CSHARP_MODEL);
		final StringBuilder builder = new StringBuilder();
		builder.append(TranslationUtils.replaceByNewValue(
				decl.getLeftOperand(), rew, fCu, context.getLogger()));
		builder.append(" " + newName + " ");
		builder.append(TranslationUtils.replaceByNewValue(decl
				.getRightOperand(), rew, fCu, context.getLogger()));
		final ASTNode placeholder = rew.createStringPlaceholder(builder
				.toString(), ASTNode.INSTANCEOF_EXPRESSION);
		rew.replace(node, placeholder, null);
	}
}
