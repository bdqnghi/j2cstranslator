package com.ilog.translator.java2cs.translation.noderewriter;

import org.eclipse.jdt.core.dom.ASTNode;
import org.eclipse.jdt.core.dom.Block;
import org.eclipse.jdt.core.dom.MethodDeclaration;
import org.eclipse.text.edits.TextEditGroup;

import com.ilog.translator.java2cs.translation.ITranslationContext;
import com.ilog.translator.java2cs.translation.TranslatorASTRewrite;

public class ConstructorInvocationRewriter extends AbstractNodeRewriter {

	//
	//
	//

	public ConstructorInvocationRewriter() {
	}

	//
	//
	//

	@Override
	public void process(ITranslationContext context, ASTNode node,
			TranslatorASTRewrite rew, TranslatorASTRewrite subRewriter,
			TextEditGroup description) {
		if (node instanceof MethodDeclaration) {
			final MethodDeclaration method = (MethodDeclaration) node;

			final Block block = method.getBody();
			if ((block != null) && (block.statements().get(0) != null)) {
				final ASTNode constructorInvok = (ASTNode) block.statements()
						.get(0);
				if (constructorInvok.getNodeType() == ASTNode.SUPER_CONSTRUCTOR_INVOCATION) {
					rew.remove(constructorInvok, null);
				} else if (constructorInvok.getNodeType() == ASTNode.CONSTRUCTOR_INVOCATION) {
					rew.remove(constructorInvok, null);
				}
			}
		}
	}
}
