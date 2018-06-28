package com.ilog.translator.java2cs.translation.noderewriter;

import java.util.ArrayList;
import java.util.List;

import org.eclipse.jdt.core.dom.ASTNode;
import org.eclipse.jdt.core.dom.Expression;
import org.eclipse.jdt.core.dom.MethodInvocation;
import org.eclipse.jdt.core.dom.SuperMethodInvocation;
import org.eclipse.text.edits.TextEditGroup;

import com.ilog.translator.java2cs.translation.ITranslationContext;
import com.ilog.translator.java2cs.translation.TranslatorASTRewrite;
import com.ilog.translator.java2cs.translation.util.TranslationUtils;

public class IndexerRewriter extends ElementRewriter {

	public enum IndexerKind {
		READ, WRITE, READ_WRITE
	}

	private final int[] parametersIndices;
	private int value_index;
	private IndexerKind sens = null;

	//
	//
	//

	public IndexerRewriter(IndexerKind sens, int[] parameters) {
		parametersIndices = parameters;
		this.sens = sens;
	}

	public IndexerRewriter(IndexerKind sens, int[] parameters, int value_index) {
		parametersIndices = parameters;
		this.sens = sens;
		this.value_index = value_index;
	}

	//
	//
	//

	@Override
	public boolean hasFormat() {
		return true;
	}

	//
	//
	//

	@SuppressWarnings("unchecked")
	@Override
	public void process(ITranslationContext context, ASTNode node,
			TranslatorASTRewrite rew, TranslatorASTRewrite subRewriter,
			TextEditGroup description) {
		// TODO: class cast sometime ??
		if (node.getNodeType() == ASTNode.METHOD_INVOCATION) {
			processMethodInvocation(context, node, rew, subRewriter);
		} else if (node.getNodeType() == ASTNode.METHOD_DECLARATION) {
			// Do in another visitor
		} else if (node.getNodeType() == ASTNode.SUPER_METHOD_INVOCATION) {
			processSuperMethodInvocation(context, node, rew, subRewriter);
		} else {
			context.getLogger().logError(
					"IndexerRewriter :: not implemented, node is " + node);
		}
	}

	private void processSuperMethodInvocation(ITranslationContext context,
			ASTNode node, TranslatorASTRewrite rew,
			TranslatorASTRewrite subRewriter) {
		final SuperMethodInvocation method = (SuperMethodInvocation) node;

		final Expression expr = null;
		final List args = method.arguments();
		final List<String> s_args = new ArrayList<String>();
		String s_expr = "base";
		if (expr != null) { // TODO: check this one !
			s_expr = TranslationUtils.replaceByNewValue(expr,
					(subRewriter == null) ? rew : subRewriter, fCu, context
							.getLogger());
		}

		if (sens == IndexerRewriter.IndexerKind.READ) {
			// class.getMethod(arg); -> class[arg];
			for (final int element : parametersIndices) {
				s_args.add(TranslationUtils.replaceByNewValue(
						(ASTNode) args.get(element - 1/* i */),
						(subRewriter == null) ? rew : subRewriter, fCu,
						context.getLogger()));
			}

			String getter = TranslationUtils.formatIndexerCall(s_expr,
					s_args);
			getter = TranslationUtils.getNodeWithComments(getter, node,
					fCu, context);
			final ASTNode placeholder = rew.createStringPlaceholder(getter,
					ASTNode.METHOD_INVOCATION);
			rew.replace(method, placeholder, null);
		} else {
			// WRITE
			// class.setMethod(arg, value); -> class[arg] = value;
			for (final int element : parametersIndices) {
				s_args.add(TranslationUtils.replaceByNewValue(
						(ASTNode) args.get(element - 1),
						(subRewriter == null) ? rew : subRewriter, fCu,
						context.getLogger()));
			}
			final String s_value = TranslationUtils.replaceByNewValue(
					(ASTNode) args.get(value_index - 1),
					(subRewriter == null) ? rew : subRewriter, fCu, context
							.getLogger());
			String setter = TranslationUtils.formatIndexerCall(s_expr,
					s_args, s_value);
			setter = TranslationUtils.getNodeWithComments(setter, node,
					fCu, context);

			final ASTNode placeholder = rew.createStringPlaceholder(setter,
					ASTNode.METHOD_INVOCATION);
			rew.replace(method, placeholder, null);
		}
	}

	private void processMethodInvocation(ITranslationContext context,
			ASTNode node, TranslatorASTRewrite rew,
			TranslatorASTRewrite subRewriter) {
		final MethodInvocation method = (MethodInvocation) node;

		final Expression expr = method.getExpression();
		final List args = method.arguments();

		final List<String> s_args = new ArrayList<String>();
		String s_expr = "this";

		if (expr != null) {
			s_expr = TranslationUtils.replaceByNewValue(expr,
					(subRewriter == null) ? rew : subRewriter, fCu, context
							.getLogger());
		}

		if (sens == IndexerRewriter.IndexerKind.READ) {
			// class.getMethod(arg); -> class[arg];
			for (final int element : parametersIndices) {
				s_args.add(TranslationUtils.replaceByNewValue(
						(ASTNode) args.get(element - 1/* i */),
						(subRewriter == null) ? rew : subRewriter, fCu,
						context.getLogger()));
			}

			String getter = TranslationUtils.formatIndexerCall(s_expr,
					s_args);
			getter = TranslationUtils.getNodeWithComments(getter, node,
					fCu, context);

			final ASTNode placeholder = rew.createStringPlaceholder(getter,
					ASTNode.METHOD_INVOCATION);
			rew.replace(method, placeholder, null);
		} else {
			// WRITE
			// class.setMethod(arg, value); -> class[arg] = value;
			for (final int element : parametersIndices) {
				s_args.add(TranslationUtils.replaceByNewValue(
						(ASTNode) args.get(element - 1),
						(subRewriter == null) ? rew : subRewriter, fCu,
						context.getLogger()));
			}
			final String s_value = TranslationUtils.replaceByNewValue(
					(ASTNode) args.get(value_index - 1),
					(subRewriter == null) ? rew : subRewriter, fCu, context
							.getLogger());
			String setter = TranslationUtils.formatIndexerCall(s_expr,
					s_args, s_value);
			setter = TranslationUtils.getNodeWithComments(setter, node,
					fCu, context);
			final ASTNode placeholder = rew.createStringPlaceholder(setter,
					ASTNode.METHOD_INVOCATION);
			rew.replace(method, placeholder, null);
		}
	}

	public IndexerKind getKind() {
		return sens;
	}

	public int[] getParamsIndex() {
		return parametersIndices;
	}

	public int getValueIndex() {
		return value_index;
	}

}
