package com.ilog.translator.java2cs.translation.astrewriter;

import org.eclipse.jdt.core.Flags;
import org.eclipse.jdt.core.dom.AST;
import org.eclipse.jdt.core.dom.ASTNode;
import org.eclipse.jdt.core.dom.Block;
import org.eclipse.jdt.core.dom.FieldDeclaration;
import org.eclipse.jdt.core.dom.Initializer;
import org.eclipse.jdt.core.dom.MethodDeclaration;
import org.eclipse.jdt.core.dom.Statement;
import org.eclipse.text.edits.TextEditGroup;

import com.ilog.translator.java2cs.translation.ITranslationContext;

public class RemoveMethodBodyVisitor extends ASTRewriterVisitor {

	//
	//
	//

	public RemoveMethodBodyVisitor(ITranslationContext context) {
		super(context);
		transformerName = "Change Covariance";
		description = new TextEditGroup(transformerName);
	}

	//
	//
	//	

	@Override
	public void endVisit(FieldDeclaration node) {
		if (Flags.isPrivate(node.getFlags())) {
			currentRewriter.remove(node, description);
		}
	}

	@Override
	public void endVisit(Initializer node) {
		final Block block = node.getBody();
		removeBody(block, node.getAST(), false);
	}

	@Override
	public void endVisit(MethodDeclaration node) {
		final Block block = node.getBody();
		removeBody(block, node.getAST(), true);
	}

	//
	//
	//

	@SuppressWarnings("unchecked")
	private void removeBody(Block block, AST ast,
			boolean checkForConsructorInvocation) {
		final String code = "throw new UnsupportedOperationException();";
		Statement statementToAdd = null;
		if (checkForConsructorInvocation && block.statements() != null
				&& block.statements().size() > 0) {
			final Statement firstStat = (Statement) block.statements().get(0);
			if (firstStat.getNodeType() == ASTNode.SUPER_CONSTRUCTOR_INVOCATION) {
				statementToAdd = firstStat;
			}
		}
		if (block != null) {
			final Block newBlock = ast.newBlock();
			final ASTNode throwStat = currentRewriter.createStringPlaceholder(
					code, ASTNode.THROW_STATEMENT);
			if (statementToAdd != null) {
				newBlock.statements().add(
						ASTNode.copySubtree(ast, statementToAdd));
			}
			newBlock.statements().add(throwStat);
			currentRewriter.replace(block, newBlock, description);
		}
	}
}
