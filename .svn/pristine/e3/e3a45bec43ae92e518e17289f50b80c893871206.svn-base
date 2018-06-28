package com.ilog.translator.java2cs.translation.astrewriter.astchange;

import java.util.List;

import org.eclipse.jdt.core.dom.AST;
import org.eclipse.jdt.core.dom.ASTNode;
import org.eclipse.jdt.core.dom.ClassInstanceCreation;
import org.eclipse.jdt.core.dom.ITypeBinding;
import org.eclipse.jdt.core.dom.rewrite.ListRewrite;
import org.eclipse.jdt.internal.corext.dom.ASTNodes;
import org.eclipse.text.edits.TextEditGroup;

import com.ilog.translator.java2cs.translation.ITranslationContext;
import com.ilog.translator.java2cs.translation.astrewriter.ASTRewriterVisitor;

public class FixForBugInSearchConstructorVisitor extends ASTRewriterVisitor {

	//
	//
	//

	public FixForBugInSearchConstructorVisitor(ITranslationContext context) {
		super(context);
		transformerName = "Fix For Bug In Search Constructor";
		description = new TextEditGroup(transformerName);
	}

	//
	//
	//

	@Override
	public void reset() {
		super.reset();
	}

	//
	//
	//

	@Override
	public boolean needValidation() {
		return true;
	}

	//
	//
	//

	@SuppressWarnings("unchecked")
	@Override
	public void endVisit(ClassInstanceCreation node) {
		final ITypeBinding binding = node.resolveTypeBinding();
		final ITypeBinding enclosing = ASTNodes.getEnclosingType(node);
		if (binding != null) {
			if (context.hasEnclosingAccess(binding.getQualifiedName(),
					enclosing.getQualifiedName())) {
				// if first parameter is not of expected type (this)
				// we need to add it ...
				final List args = node.arguments();
				final int index = searchForThis(args);
				if ((args.size() == 0) || (index == -1)) {
					final AST ast = node.getAST();
					final ListRewrite listArgs = currentRewriter
							.getListRewrite(node,
									ClassInstanceCreation.ARGUMENTS_PROPERTY);
					listArgs.insertFirst(ast.newThisExpression(), description);
				}
			}
		}
	}

	//
	//
	//

	@SuppressWarnings("unchecked")
	private int searchForThis(List args) {
		for (int i = 0; i < args.size(); i++) {
			final ASTNode node = (ASTNode) args.get(i);
			if (node.getNodeType() == ASTNode.THIS_EXPRESSION) {
				return i;
			}
		}
		return -1;
	}

}