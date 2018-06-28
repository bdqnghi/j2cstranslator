package com.ilog.translator.java2cs.translation.astrewriter;

import org.eclipse.jdt.core.dom.ASTNode;
import org.eclipse.jdt.core.dom.ConditionalExpression;
import org.eclipse.jdt.core.dom.Expression;
import org.eclipse.jdt.core.dom.ParenthesizedExpression;
import org.eclipse.text.edits.TextEditGroup;

import com.ilog.translator.java2cs.translation.ITranslationContext;

/**
 * 
 * 
 * @author afau
 * 
 */
public class NormalizeConditionalExpressionVisitor extends
		ASTRewriterVisitor {
	
	//
	//
	//

	public NormalizeConditionalExpressionVisitor(
			ITranslationContext context) {
		super(context);
		this.transformerName = "Normalize Conditional Expression";
		description = new TextEditGroup(transformerName);
	}

	//
	//
	//

	
	@Override
	public void endVisit(ConditionalExpression node) {
		// ITypeBinding tbinding = node.resolveTypeBinding();
		Expression thenE = node.getThenExpression();
		Expression elseE = node.getElseExpression();
		Expression test = node.getExpression();
		
		// VS2008 conditional test MUSE BE parenthesized !
		if (test.getNodeType() != ASTNode.PARENTHESIZED_EXPRESSION) {
			ParenthesizedExpression pExpr = currentRewriter.getAST().newParenthesizedExpression();
			pExpr.setExpression((Expression) currentRewriter.createMoveTarget(test));
			/*
			String repl = "(" + TranslationUtils.getNodeWithComments(ASTNodes.asString(test) , node, fCu, context) + ")?";
			repl += TranslationUtils.getNodeWithComments(ASTNodes.asString(thenE) , thenE, fCu, context);
			repl += ":";
			repl += TranslationUtils.getNodeWithComments(ASTNodes.asString(elseE) , thenE, fCu, context);
			ASTNode replacement = currentRewriter.createStringPlaceholder(repl, node.getNodeType());*/
			currentRewriter.remove(test, description);
			currentRewriter.replace(test, pExpr, description);
		}

	}
	
	
}
