package com.ilog.translator.java2cs.translation.noderewriter;

import org.eclipse.jdt.core.dom.ASTNode;
import org.eclipse.jdt.core.dom.StringLiteral;
import org.eclipse.text.edits.TextEditGroup;

import com.ilog.translator.java2cs.translation.ITranslationContext;
import com.ilog.translator.java2cs.translation.TranslatorASTRewrite;
import com.ilog.translator.java2cs.util.CSharpModelUtil;

public class StringRewriter extends AbstractNodeRewriter {

	private final String name;

	//
	//
	//

	public StringRewriter(String name) {
		this.name = name;
	}

	//
	//
	//

	@Override
	public void process(ITranslationContext context, ASTNode node,
			TranslatorASTRewrite rew, TranslatorASTRewrite subRewriter,
			TextEditGroup description) {
		if (node.getNodeType() == ASTNode.STRING_LITERAL) {
			if (needArobace(name)) {
				final ASTNode placeholder = rew.createStringPlaceholder(
						CSharpModelUtil.AROBACE + ((StringLiteral) node).getEscapedValue(), 
						ASTNode.STRING_LITERAL);
						//"\""
								//+ ((StringLiteral) node).getLiteralValue()
								//+ "\"", ASTNode.STRING_LITERAL);
				rew.replace(node, placeholder, null);
			}
		}
	}

	//
	//
	//

	private boolean needArobace(String name2) {
		for (int i = 0; i < name2.length() - 1; i++) {
			final char c1 = name2.charAt(i);
			final char c2 = name2.charAt(i + 1);
			if ((c1 == '\\') && Character.isDigit(c2)) {
				return true;
			}
		}
		return false;
	}
}
