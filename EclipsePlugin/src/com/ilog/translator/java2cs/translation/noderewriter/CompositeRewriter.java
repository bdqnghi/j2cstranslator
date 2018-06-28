package com.ilog.translator.java2cs.translation.noderewriter;

import org.eclipse.jdt.core.dom.ASTNode;
import org.eclipse.text.edits.TextEditGroup;

import com.ilog.translator.java2cs.translation.ITranslationContext;
import com.ilog.translator.java2cs.translation.TranslatorASTRewrite;

public class CompositeRewriter extends AbstractNodeRewriter {

	private final INodeRewriter r1;

	private final INodeRewriter r2;

	//
	//
	//

	public CompositeRewriter(INodeRewriter r1, INodeRewriter r2) {
		this.r1 = r1;
		this.r2 = r2;
	}

	//
	//
	//

	@Override
	public void process(ITranslationContext context, ASTNode node,
			TranslatorASTRewrite rew, TranslatorASTRewrite subRewriter,
			TextEditGroup description) {
		r1.process(context, node, rew, subRewriter, description);
		r2.process(context, node, rew, subRewriter, description);
	}
}
