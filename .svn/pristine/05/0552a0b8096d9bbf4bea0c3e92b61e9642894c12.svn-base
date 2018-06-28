package com.ilog.translator.java2cs.translation.noderewriter;

import org.eclipse.jdt.core.dom.ASTNode;
import org.eclipse.jdt.core.dom.PrimitiveType;
import org.eclipse.jdt.core.dom.TypeLiteral;
import org.eclipse.text.edits.TextEditGroup;

import com.ilog.translator.java2cs.translation.ITranslationContext;
import com.ilog.translator.java2cs.translation.TranslatorASTRewrite;
import com.ilog.translator.java2cs.translation.util.TranslationUtils;
import com.ilog.translator.java2cs.util.TranslationModelUtil;

public class PrimitiveTypeRewriter extends AbstractNodeRewriter implements
		Cloneable {

	private String name;

	//
	//
	//

	public PrimitiveTypeRewriter(String name) {
		this.name = name;
	}

	//
	// name
	//

	public String getName() {
		return name;
	}

	public void setName(String name) {
		this.name = name;
	}

	//
	//
	//

	@Override
	public void process(ITranslationContext context, ASTNode node,
			TranslatorASTRewrite rew, TranslatorASTRewrite subRewriter,
			TextEditGroup description) {
		final String typeof = context.getMapper().getKeyword(
				TranslationModelUtil.TYPEOF_KEYWORD,
				TranslationModelUtil.CSHARP_MODEL);
		switch (node.getNodeType()) {
		case ASTNode.PRIMITIVE_TYPE:
			final PrimitiveType stype = (PrimitiveType) rew
					.createStringPlaceholder(name, ASTNode.PRIMITIVE_TYPE);
			rew.replace(node, stype, null);
			break;
		case ASTNode.TYPE_LITERAL:
			if (name != null) {

				// TODO : same code as in TypeOfRewriter !!!!
				final StringBuilder builder = new StringBuilder();
				builder.append(typeof + "(");
				builder.append(name);
				builder.append(")");

				final ASTNode placeholder = rew.createStringPlaceholder(builder
						.toString(), ASTNode.TYPE_LITERAL);
				rew.replace(node, placeholder, null);
			}
		}
	}

	//
	// clone
	//

	@Override
	public INodeRewriter clone() {
		final PrimitiveTypeRewriter newR = new PrimitiveTypeRewriter(name);
		return newR;
	}

}
