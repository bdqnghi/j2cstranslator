package com.ilog.translator.java2cs.translation.astrewriter;

import java.util.ArrayList;
import java.util.List;

import org.eclipse.jdt.core.dom.AST;
import org.eclipse.jdt.core.dom.ASTNode;
import org.eclipse.jdt.core.dom.Block;
import org.eclipse.jdt.core.dom.BreakStatement;
import org.eclipse.jdt.core.dom.IfStatement;
import org.eclipse.jdt.core.dom.LabeledStatement;
import org.eclipse.jdt.core.dom.Statement;
import org.eclipse.jdt.core.dom.SwitchStatement;
import org.eclipse.jdt.core.dom.rewrite.ListRewrite;
import org.eclipse.text.edits.TextEditGroup;

import com.ilog.translator.java2cs.translation.ITranslationContext;

//
// Add a break in case statement except if it already ended
// by a return or a continue
//
public class AddBreakInSwichCaseVisitor extends ASTRewriterVisitor {

	public AddBreakInSwichCaseVisitor(ITranslationContext context) {
		super(context);
		transformerName = "Add Missing Break In Swich Case";
		description = new TextEditGroup(transformerName);
	}

	//
	//
	//

	@SuppressWarnings("unchecked")
	@Override
	public void endVisit(LabeledStatement node) {
		if (node.getBody() != null) {
			final AST ast = node.getAST();
			final Block block = ast.newBlock();
			block.statements().add(
					currentRewriter.createCopyTarget(node.getBody()));
			currentRewriter.replace(node.getBody(), block, description);
		}
	}

	@SuppressWarnings("unchecked")
	@Override
	public void endVisit(SwitchStatement node) {
		final List stats = node.statements();
		ASTNode currentNode = null;
		ASTNode previousNode = null;
		final List<List<ASTNode>> cases = new ArrayList<List<ASTNode>>();
		List<ASTNode> currentCaseBody = null;
		for (int pos = 0; pos < stats.size(); pos++) {
			currentNode = (ASTNode) stats.get(pos);
			if (currentNode.getNodeType() == ASTNode.SWITCH_CASE) {
				currentCaseBody = new ArrayList<ASTNode>();
				cases.add(currentCaseBody);
				if (pos > 0) {
					previousNode = (ASTNode) stats.get(pos - 1);
				}
			} else if (currentCaseBody != null)
				currentCaseBody.add(currentNode);
		}
		removeFallThrough(node, cases);
		// the last statement (default one ?)
		for (int pos = 0; pos < stats.size(); pos++) {
			currentNode = (ASTNode) stats.get(pos);
			if (currentNode.getNodeType() == ASTNode.SWITCH_CASE) {
				if (pos > 0) {
					previousNode = (ASTNode) stats.get(pos - 1);
					addBreakIfNeeded(node, currentNode, previousNode);
				}
			}
		}
		if (currentNode != null) {
			addBreakIfNeeded(node, currentNode, currentNode);
		}
	}

	//
	//
	//

	@SuppressWarnings("unchecked")
	private void removeFallThrough(SwitchStatement node,
			List<List<ASTNode>> cases) {

		if (cases.size() > 0) {
			final List<ASTNode> lastCase = cases.get(cases.size() - 1);
			//
			if (lastCase != null && (lastCase.size() == 0)) {
				final ListRewrite lr = currentRewriter.getListRewrite(node,
						SwitchStatement.STATEMENTS_PROPERTY);
				final List list = node.statements();
				final ASTNode lastNode = (ASTNode) list.get(list.size() - 1);
				final BreakStatement newBreak = node.getAST()
						.newBreakStatement();
				lr.insertAfter(newBreak, lastNode, null);
				lastCase.add(newBreak);
			}
			//
			List<ASTNode> currentBreakCase = ASTNode.copySubtrees(
					node.getAST(), lastCase);
			for (int i = cases.size() - 2; i >= 0; i--) {
				final List<ASTNode> switchCase = cases.get(i);
				if (fallthrough(switchCase)) {
					final List<ASTNode> newCase = ASTNode.copySubtrees(node
							.getAST(), currentBreakCase);
					for (int j = switchCase.size() - 1; j >= 0; j--) {
						final ASTNode astNode = switchCase.get(j);
						currentBreakCase.add(0, astNode);
					}
					final ListRewrite lr = currentRewriter.getListRewrite(node,
							SwitchStatement.STATEMENTS_PROPERTY);
					final ASTNode lastNode = switchCase
							.get(switchCase.size() - 1);
					if (newCase.size() == 1) {
						lr.insertAfter(newCase.get(0), lastNode, null);
					} else if (newCase.size() > 1) {
						final Block newBlock = node.getAST().newBlock();
						newBlock.statements().addAll(newCase);
						lr.insertAfter(newBlock, lastNode, null);
					}
				} else if (switchCase.size() == 0) {
					// don't want to store that empty case
				} else {
					currentBreakCase = ASTNode.copySubtrees(node.getAST(),
							switchCase);
				}
			}
		}
	}

	/**
	 * True if not all paths "break" (break, continue, return or thrwo)
	 * 
	 * @param scase
	 * @return
	 */
	private boolean fallthrough(List<ASTNode> scase) {
		if (scase.size() == 0)
			return false;
		return !breaks(scase.get(scase.size() - 1));
	}

	@SuppressWarnings("unchecked")
	private boolean breaks(ASTNode node) {
		switch (node.getNodeType()) {
		case ASTNode.BREAK_STATEMENT:
		case ASTNode.RETURN_STATEMENT:
		case ASTNode.CONTINUE_STATEMENT:
		case ASTNode.THROW_STATEMENT:
		case ASTNode.SWITCH_CASE:
			return true;
		case ASTNode.SWITCH_STATEMENT:
			final SwitchStatement swi = (SwitchStatement) node;
			final List<List<ASTNode>> cases = new ArrayList<List<ASTNode>>();
			List<ASTNode> currentCaseBody = null;
			final List stats = swi.statements();
			ASTNode currentNode = null;			
			ASTNode previousNode = null;
			for (int pos = 0; pos < stats.size(); pos++) {
				currentNode = (ASTNode) stats.get(pos);
				if (currentNode.getNodeType() == ASTNode.SWITCH_CASE) {
					currentCaseBody = new ArrayList<ASTNode>();
					cases.add(currentCaseBody);
					if (pos > 0) {
						previousNode = (ASTNode) stats.get(pos - 1);
						// this.addBreakIfNeeded(node, currentNode,
						// previousNode);
					}
				} else if (currentCaseBody != null)
					currentCaseBody.add(currentNode);
			}
			boolean breaks = true;
			for (final List<ASTNode> currentCase : cases) {
				breaks &= breaks(currentNode);
			}
			return breaks;
		case ASTNode.BLOCK:
			final Block body = (Block) node;
			final List<ASTNode> statements = body.statements();
			return !fallthrough(statements);
		case ASTNode.IF_STATEMENT:
			final IfStatement ifs = (IfStatement) node;
			final Statement elseStat = ifs.getElseStatement();
			return breaks(ifs.getThenStatement())
					&& (elseStat != null && breaks(elseStat));
		default:
			return false;
		}
	}

	@SuppressWarnings("unchecked")
	private void addBreakIfNeeded(SwitchStatement node, ASTNode currentNode,
			ASTNode previousNode) {
		switch (previousNode.getNodeType()) {
		case ASTNode.BREAK_STATEMENT:
		case ASTNode.RETURN_STATEMENT:
		case ASTNode.CONTINUE_STATEMENT:
		case ASTNode.THROW_STATEMENT:
		case ASTNode.SWITCH_CASE:
			// do nothing;
			break;
		case ASTNode.SWITCH_STATEMENT:
			addBreak(node, currentNode, (currentNode == previousNode));
			break;
		case ASTNode.BLOCK:
			final Block body = (Block) previousNode;
			final List statements = body.statements();
			final int size = statements.size();
			if (size > 0) {
				final ASTNode lastStat = (ASTNode) statements.get(size - 1);
				switch (lastStat.getNodeType()) {
				case ASTNode.BREAK_STATEMENT:
				case ASTNode.RETURN_STATEMENT:
				case ASTNode.CONTINUE_STATEMENT:
				case ASTNode.SWITCH_CASE:
				case ASTNode.THROW_STATEMENT:
					// do nothing;
					break;
				default: {
					addBreak(node, currentNode, (currentNode == previousNode));
				}
				}
			}
			break;
		default: {
			addBreak(node, currentNode, (currentNode == previousNode));
		}
		}
	}

	private void addBreak(SwitchStatement node, ASTNode currentNode,
			boolean insertLast) {
		final ListRewrite lRewrite = currentRewriter.getListRewrite(node,
				SwitchStatement.STATEMENTS_PROPERTY);
		final ASTNode breakNode = node.getAST().newBreakStatement();
		if (insertLast) {
			lRewrite.insertLast(breakNode, null);
		} else {
			lRewrite.insertBefore(breakNode, currentNode, null);
		}
	}
}
