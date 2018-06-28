package com.ilog.translator.java2cs.translation.noderewriter;

import org.eclipse.jdt.core.dom.ASTNode;
import org.eclipse.jdt.core.dom.SuperMethodInvocation;
import org.eclipse.text.edits.TextEditGroup;

import com.ilog.translator.java2cs.translation.ITranslationContext;
import com.ilog.translator.java2cs.translation.TranslatorASTRewrite;
import com.ilog.translator.java2cs.translation.util.TranslationUtils;
import com.ilog.translator.java2cs.util.TranslationModelUtil;

public class SuperMethodInvocationRewriter extends ElementRewriter {


	public SuperMethodInvocationRewriter() {
	}

	//
	//
	//

	@Override
	public void process(ITranslationContext context, ASTNode node,
			TranslatorASTRewrite rew, TranslatorASTRewrite subRewriter,
			TextEditGroup description) {
		final SuperMethodInvocation decl = (SuperMethodInvocation) node;

		final String newName = context.getMapper().getKeyword(
				TranslationModelUtil.SUPER_KEYWORD,
				TranslationModelUtil.CSHARP_MODEL);

		final StringBuilder builder = new StringBuilder();
		builder.append(newName);
		builder.append(".");
		builder.append(TranslationUtils.replaceByNewValue(decl.getName(), rew,
				fCu, context.getLogger()));
		builder.append("(");
		for (int i = 0; i < decl.arguments().size() - 1; i++) {
			final String argName = getArgumentName(context, (ASTNode) decl
					.arguments().get(i), rew);
			builder.append(argName + ",");
		}
		if (decl.arguments().size() >= 1) {
			final String argName = getArgumentName(context, (ASTNode) decl
					.arguments().get(decl.arguments().size() - 1), rew);
			builder.append(argName);
		}
		builder.append(")");

		final ASTNode placeholder = rew.createStringPlaceholder(builder
				.toString(), ASTNode.SUPER_METHOD_INVOCATION);
		rew.replace(node, placeholder, null);
	}

	//
	// ArgumentName
	//

	private String getArgumentName(ITranslationContext context, ASTNode node,
			TranslatorASTRewrite rew) {
		final String name = TranslationUtils.replaceByNewValue(node, rew, fCu,
				context.getLogger());
		return name;
	}
}
