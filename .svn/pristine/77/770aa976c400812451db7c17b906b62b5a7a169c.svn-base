package com.ilog.translator.java2cs.translation.astrewriter;

import org.eclipse.jdt.core.dom.AST;
import org.eclipse.jdt.core.dom.ASTNode;
import org.eclipse.jdt.core.dom.ITypeBinding;
import org.eclipse.jdt.core.dom.NumberLiteral;
import org.eclipse.jdt.internal.corext.dom.ASTNodes;
import org.eclipse.text.edits.TextEditGroup;

import com.ilog.translator.java2cs.translation.ITranslationContext;
import com.ilog.translator.java2cs.translation.util.TranslationUtils;

/**
 * 
 * @author afau
 * 
 * 1/ In C# haxdecimal number with negative value (such as 0xFFFFF) are not
 * allowed I must replace it by a 10-base based value 2/ 1., 1.d and 1.f number
 * value are not allowrd must add a 0 after the "."
 */
public class LiteralsVisitor extends ASTRewriterVisitor {

	//
	//
	//

	public LiteralsVisitor(ITranslationContext context) {
		super(context);
		transformerName = "Change Literal";
		description = new TextEditGroup(transformerName);
	}

	//
	//
	//

	@Override
	public void endVisit(NumberLiteral node) {
		final ITypeBinding itype = node.resolveTypeBinding();
		final AST ast = node.getAST();
		if (itype.isPrimitive()) {
			final String value = node.getToken();
			if (itype.getName().equals(TranslationUtils.LONG)) {
				if (value.startsWith("0x")) {
					try {
						final Long l = (Long) node
								.resolveConstantExpressionValue();
						if (l < 0) {
							final NumberLiteral nLiteral = (NumberLiteral) ASTNode
									.copySubtree(ast, node);
							nLiteral.setToken(Long.toString(l));
							currentRewriter
									.replace(node, nLiteral, description);
						}
					} catch (final Exception e) {
						e.printStackTrace();
						context.getLogger().logException("", e);
					}
				} else if (value.startsWith("0") && (value.length() > 1)) {
					// Octal
					try {
						final Long l = (Long) node
								.resolveConstantExpressionValue();
						final NumberLiteral nLiteral = (NumberLiteral) ASTNode
								.copySubtree(ast, node);
						nLiteral.setToken("0x" + Long.toHexString(l) + "L");
						currentRewriter.replace(node, nLiteral, description);

					} catch (final Exception e) {
						e.printStackTrace();
						context.getLogger().logException("", e);
					}
				}
			} else if (itype.getName().equals(TranslationUtils.INT)) {
				if (value.startsWith("0x")) {
					try {
						final Integer l = (Integer) node
								.resolveConstantExpressionValue();
						if (l < 0) {
							final NumberLiteral nLiteral = (NumberLiteral) ASTNode
									.copySubtree(ast, node);
							nLiteral.setToken(Integer.toString(l));
							currentRewriter
									.replace(node, nLiteral, description);
						}
					} catch (final Exception e) {
						e.printStackTrace();
						context.getLogger().logException("", e);
					}
				} else if (value.startsWith("0") && (value.length() > 1)) {
					// Octal
					try {
						final Integer l = (Integer) node
								.resolveConstantExpressionValue();
						final NumberLiteral nLiteral = (NumberLiteral) ASTNode
								.copySubtree(ast, node);
						nLiteral.setToken("0x" + Integer.toHexString(l));
						currentRewriter.replace(node, nLiteral, description);

					} catch (final Exception e) {
						e.printStackTrace();
						context.getLogger().logException("", e);
					}
				}
			} else if (itype.getName().equals(TranslationUtils.FLOAT)) {
				if (!value.endsWith("f") && !value.endsWith("F")) {
					final ASTNode replacement = currentRewriter
							.createStringPlaceholder(ASTNodes.asString(node)
									+ "f", node.getNodeType());
					currentRewriter.replace(node, replacement, description);
				}
			} else if (itype.getName().equals(TranslationUtils.DOUBLE)) {
				if (!value.endsWith("d") && !value.endsWith("D")) {
					final ASTNode replacement = currentRewriter
							.createStringPlaceholder(ASTNodes.asString(node)
									+ "d", node.getNodeType());
					currentRewriter.replace(node, replacement, description);
				}
			}
			if (value.endsWith(".")) {
				final NumberLiteral nLiteral = (NumberLiteral) ASTNode
						.copySubtree(ast, node);
				nLiteral.setToken(value + "0"); // TODO :
				currentRewriter.replace(node, nLiteral, description);
			} else if (value.endsWith(".d")) {
				final NumberLiteral nLiteral = (NumberLiteral) ASTNode
						.copySubtree(ast, node);
				nLiteral.setToken(value.replace(".d", ".0d")); // TODO :
				currentRewriter.replace(node, nLiteral, description);
			} else if (value.endsWith(".f")) {
				final NumberLiteral nLiteral = (NumberLiteral) ASTNode
						.copySubtree(ast, node);
				nLiteral.setToken(value.replace(".f", ".0f")); // TODO :
				currentRewriter.replace(node, nLiteral, description);
			}
		}
	}
}
