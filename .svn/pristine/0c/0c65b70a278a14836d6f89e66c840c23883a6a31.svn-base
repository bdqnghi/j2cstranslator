package com.ilog.translator.java2cs.translation.astrewriter;

import org.eclipse.jdt.core.JavaModelException;
import org.eclipse.jdt.core.dom.ASTNode;
import org.eclipse.jdt.core.dom.Block;
import org.eclipse.jdt.core.dom.Comment;
import org.eclipse.jdt.core.dom.CompilationUnit;
import org.eclipse.jdt.core.dom.EnhancedForStatement;
import org.eclipse.jdt.core.dom.InstanceofExpression;
import org.eclipse.jdt.core.dom.Type;
import org.eclipse.jdt.core.dom.TypeLiteral;
import org.eclipse.jdt.core.dom.rewrite.ListRewrite;
import org.eclipse.jdt.internal.corext.dom.ASTNodes;
import org.eclipse.text.edits.TextEditGroup;

import com.ilog.translator.java2cs.translation.ITranslationContext;
import com.ilog.translator.java2cs.translation.util.TranslationUtils;

public class ForeachCheckerVisitor extends ASTRewriterVisitor {

	//
	//
	//

	public ForeachCheckerVisitor(ITranslationContext context) {
		super(context);
		transformerName = "Foreach checker";
		description = new TextEditGroup(transformerName);
	}

	//
	//
	//	

	@SuppressWarnings("unchecked")
	@Override
	public void endVisit(EnhancedForStatement node) {
		final ASTNode element = currentRewriter.createStringPlaceholder(
				"/* foreach */", ASTNode.BLOCK);
		final ASTNode block = node.getParent();
		if (block.getNodeType() != ASTNode.BLOCK) {
			final Block b = node.getAST().newBlock();
			try {
				b.statements().add(element);
				b.statements().add(ASTNode.copySubtree(node.getAST(), node));
				currentRewriter.replace(node, b, description);
			} catch (final Exception e) {
				e.printStackTrace();
				context.getLogger().logException("", e);
			}
		} else {
			final ListRewrite lrew = currentRewriter.getListRewrite(block,
					Block.STATEMENTS_PROPERTY);
			lrew.insertBefore(element, node, null);
		}

		// return true;
	}

	@Override
	public boolean visit(InstanceofExpression node) {
		String metaComment = " instanceof ";
		//
		final CompilationUnit cu = (CompilationUnit) node.getRoot();
		final int end = cu.lastTrailingCommentIndex(node);
		if (end >= 0) {
			// BR 2103672. Patch from faron
			final Comment comment = (Comment) cu.getCommentList().get(end);
			try {
				final int s = comment.getStartPosition();
				final int l = comment.getLength();
				final String c = fCu.getBuffer().getText(s, l);
				final String content = c.substring(2, c.length() - 2).trim();
				if (content.startsWith("?=")) {
					final String genericType = content.substring(2);
					metaComment += genericType + " ";
				}
			} catch (final JavaModelException e) {
				e.printStackTrace();
				context.getLogger().logException("", e);
			}
		}
		final ASTNode replacement = currentRewriter.createStringPlaceholder(
				ASTNodes.asString(node.getLeftOperand()) + "/*" + metaComment
						+ "*/", node.getLeftOperand().getNodeType());
		// }
		currentRewriter
				.replace(node.getLeftOperand(), replacement, description);
		return true;
	}

	@Override
	public void endVisit(TypeLiteral node) {
		final Type type = node.getType();
		final String comment = TranslationUtils.createCommentForTypeOfNode(
				node, type, context, fCu);
		final String replacement = comment + " " + ASTNodes.asString(node);
		final ASTNode replacementNode = currentRewriter
				.createStringPlaceholder(replacement, node.getNodeType());
		currentRewriter.replace(node, replacementNode, description);
	}
}
