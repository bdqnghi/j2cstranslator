package com.ilog.translator.java2cs.translation.noderewriter;

import java.util.List;

import org.eclipse.jdt.core.dom.AST;
import org.eclipse.jdt.core.dom.ASTNode;
import org.eclipse.jdt.core.dom.Block;
import org.eclipse.jdt.core.dom.BreakStatement;
import org.eclipse.jdt.core.dom.ContinueStatement;
import org.eclipse.jdt.core.dom.LabeledStatement;
import org.eclipse.jdt.core.dom.SimpleName;
import org.eclipse.jdt.core.dom.SwitchStatement;
import org.eclipse.jdt.core.dom.rewrite.ASTRewrite;
import org.eclipse.jdt.core.dom.rewrite.ListRewrite;
import org.eclipse.text.edits.TextEditGroup;

import com.ilog.translator.java2cs.translation.ITranslationContext;
import com.ilog.translator.java2cs.translation.TranslatorASTRewrite;
import com.ilog.translator.java2cs.util.TranslationModelUtil;

public class BreakAndContinueLabelRewriter extends AbstractNodeRewriter {

	private String name;

	private List<BreakStatement> blabels;

	private List<ContinueStatement> clabels;

	//
	//
	//

	public BreakAndContinueLabelRewriter(List<BreakStatement> blabels,
			List<ContinueStatement> clabels) {
		this.blabels = blabels;
		this.clabels = clabels;
	}

	public BreakAndContinueLabelRewriter(String name) {
		this.name = name;
	}

	//
	//
	//

	@Override
	public void process(ITranslationContext context, ASTNode node,
			TranslatorASTRewrite rew, TranslatorASTRewrite subRewriter,
			TextEditGroup description) {
		final String gotokw = context.getMapper().getKeyword(
				TranslationModelUtil.GOTO_KEYWORD,
				TranslationModelUtil.CSHARP_MODEL);
		if (node instanceof BreakStatement) {
			final ASTNode placeholder = rew.createStringPlaceholder(gotokw
					+ " " + name + ";", ASTNode.BREAK_STATEMENT);
			rew.replace(node, placeholder, null);
		} else if (node instanceof ContinueStatement) {
			final ASTNode placeholder = rew.createStringPlaceholder(gotokw
					+ " " + name + ";", ASTNode.BREAK_STATEMENT);
			rew.replace(node, placeholder, null);
		} else if (node instanceof LabeledStatement) {
			final LabeledStatement lStat = (LabeledStatement) node;
			final SimpleName label = lStat.getLabel();

			final AST ast = node.getAST();

			if (blabels != null) {
				final SimpleName newLabel = ast.newSimpleName(gotokw
						+ label.getIdentifier());
				this.replaceLabel(blabels, newLabel, rew);
				insertNewLabeledStatement(lStat, ast, newLabel, rew);
			}

			if (clabels != null) {
				final SimpleName newLabel = ast.newSimpleName(gotokw
						+ label.getIdentifier());
				this.replaceLabel(lStat, clabels, newLabel, rew);
			}
		}
	}

	//
	//
	//

	private void insertNewLabeledStatement(LabeledStatement node, AST ast,
			SimpleName newLabel, ASTRewrite rew) {
		// Create new LabeledStatement
		final LabeledStatement ls = ast.newLabeledStatement();
		ls.setBody(ast.newEmptyStatement());
		ls.setLabel(newLabel);

		// InsertIt in parent after current labeled statement
		final ListRewrite list = searchForBlock(rew, node);
		list.insertAfter(ls, node, null);
	}

	private ListRewrite searchForBlock(ASTRewrite rew, ASTNode node) {
		final ASTNode parent = node.getParent();
		if (parent.getNodeType() == ASTNode.BLOCK) {
			return rew.getListRewrite(parent, Block.STATEMENTS_PROPERTY);
		}
		if (parent.getNodeType() == ASTNode.SWITCH_STATEMENT) {
			return rew.getListRewrite(parent,
					SwitchStatement.STATEMENTS_PROPERTY);
		} else {
			return searchForBlock(rew, parent); // may loop ...
		}
	}

	private void replaceLabel(List<BreakStatement> labels, SimpleName newLabel,
			ASTRewrite rew) {
		for (final BreakStatement statement : labels) {
			rew.replace(statement.getLabel(), newLabel, null);
		}
	}

	private void replaceLabel(LabeledStatement node,
			List<ContinueStatement> labels, SimpleName newLabel, ASTRewrite rew) {
		for (final ContinueStatement statement : labels) {
			if (statement.getParent().getParent().equals(node.getBody())) {
				rew.replace(statement.getLabel(), newLabel, null);
			}
		}
	}
}
