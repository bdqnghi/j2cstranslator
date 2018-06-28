package com.ilog.translator.java2cs.translation.noderewriter;

import java.util.ArrayList;
import java.util.List;

import org.eclipse.jdt.core.dom.ASTNode;
import org.eclipse.jdt.core.dom.FieldAccess;
import org.eclipse.jdt.core.dom.FieldDeclaration;
import org.eclipse.jdt.core.dom.Modifier;
import org.eclipse.jdt.core.dom.QualifiedName;
import org.eclipse.jdt.core.dom.SimpleName;
import org.eclipse.jdt.core.dom.SingleVariableDeclaration;
import org.eclipse.jdt.core.dom.VariableDeclarationExpression;
import org.eclipse.jdt.core.dom.VariableDeclarationFragment;
import org.eclipse.jdt.core.dom.VariableDeclarationStatement;
import org.eclipse.text.edits.TextEditGroup;

import com.ilog.translator.java2cs.configuration.ChangeModifierDescriptor;
import com.ilog.translator.java2cs.configuration.DotNetModifier;
import com.ilog.translator.java2cs.translation.ITranslationContext;
import com.ilog.translator.java2cs.translation.TranslatorASTRewrite;
import com.ilog.translator.java2cs.translation.util.TranslationUtils;

public class FieldRewriter extends MemberElementRewriter {

	//
	//
	//

	public FieldRewriter() {
	}

	public FieldRewriter(String name) {
		this.name = name;
	}

	public FieldRewriter(int dummy, String pattern) {
		format = pattern;
	}

	public FieldRewriter(ChangeModifierDescriptor mod) {
		changeModifiers = mod;
	}

	//
	//
	//

	List<String> visited = new ArrayList<String>();

	@Override
	public void process(ITranslationContext context, ASTNode node,
			TranslatorASTRewrite rew, TranslatorASTRewrite subRewriter,
			TextEditGroup description) {
		if (context.getConfiguration().getOptions().getGlobalOptions()
				.isPerfCount()) {
			// 
			final String tracked = node.toString();
			if (visited.contains(node.getNodeType() + tracked
					+ node.getParent().toString())) {
				context.getLogger().logInfo(
						"Field " + tracked
								+ " already modified by FieldRewriter/"
								+ description.getName());
			} else {
				visited.add(node.getNodeType() + tracked
						+ node.getParent().toString());
			}
		}
		switch (node.getNodeType()) {
		case ASTNode.FIELD_ACCESS: {
			processFieldAccess(context, node, rew);
			break;
		}
		case ASTNode.QUALIFIED_NAME: {
			processQualifiedName(context, node, rew);

			break;
		}
		case ASTNode.SIMPLE_NAME: {
			processSimpleName(context, node, rew);
			break;
		}
		case ASTNode.FIELD_DECLARATION: {
			processFieldDeclaration(context, node, rew, subRewriter,
					description);
			break;
		}
		case ASTNode.VARIABLE_DECLARATION_FRAGMENT: {
			processVariableDeclarationFragment(context, node, rew, subRewriter,
					description);
			break;
		}
		case ASTNode.SINGLE_VARIABLE_DECLARATION: {
			processSingleVariableDeclaration(context, node, rew, subRewriter,
					description);
			break;
		}
		case ASTNode.VARIABLE_DECLARATION_EXPRESSION: {
			processVariableDeclarationExpression(context, node, rew);
			break;
		}
		case ASTNode.VARIABLE_DECLARATION_STATEMENT: {
			processVariableDeclarationStatement(context, node, rew,
					subRewriter, description);
			break;
		}
		}
	}

	private void processVariableDeclarationStatement(
			ITranslationContext context, ASTNode node,
			TranslatorASTRewrite rew, TranslatorASTRewrite subRewriter,
			TextEditGroup description) {
		final VariableDeclarationStatement field = (VariableDeclarationStatement) node;

		if (changeModifiers != null) {
			final ModifiersRewriter rewriter = new ModifiersRewriter(
					changeModifiers);
			if (field.getType() != null
					&& field.getType().resolveBinding() != null
					&& field.getType().resolveBinding().isPrimitive())
				if (Modifier.isFinal(field.getModifiers())
						&& ModifiersRewriter.isConstable(field))
					changeModifiers.replace(DotNetModifier.FINAL,
							DotNetModifier.CONST);
				else
					changeModifiers.remove(DotNetModifier.FINAL);
			rewriter.process(context, node, rew, subRewriter, description);
		} else {
			final ChangeModifierDescriptor desc = new ChangeModifierDescriptor();
			if (field.getType() != null
					&& field.getType().resolveBinding() != null
					&& field.getType().resolveBinding().isPrimitive())
				if (Modifier.isFinal(field.getModifiers())
						&& ModifiersRewriter.isConstable(field))
					desc.replace(DotNetModifier.FINAL, DotNetModifier.CONST);
				else
					desc.remove(DotNetModifier.FINAL);
			final ModifiersRewriter rewriter = new ModifiersRewriter(desc);
			rewriter.process(context, node, rew, subRewriter, description);
		}

		if (name != null) {
			final VariableDeclarationFragment fragment = TranslationUtils
					.getFrament(field);
			final String replacement = TranslationUtils.getNodeWithComments(
					name, node, fCu, context);
			final ASTNode newName = rew.createStringPlaceholder(replacement,
					fragment.getName().getNodeType());
			rew.replace(fragment.getName(), newName, null);
		}
	}

	private void processVariableDeclarationExpression(
			ITranslationContext context, ASTNode node, TranslatorASTRewrite rew) {
		final VariableDeclarationExpression field = (VariableDeclarationExpression) node;
		if (name != null) {
			final VariableDeclarationFragment fragment = TranslationUtils
					.getFrament(field);
			final String replacement = TranslationUtils.getNodeWithComments(
					name, node, fCu, context);
			final ASTNode newName = rew.createStringPlaceholder(replacement,
					fragment.getName().getNodeType());
			rew.replace(fragment.getName(), newName, null);
		}
	}

	private void processSingleVariableDeclaration(ITranslationContext context,
			ASTNode node, TranslatorASTRewrite rew,
			TranslatorASTRewrite subRewriter, TextEditGroup description) {
		final SingleVariableDeclaration field = (SingleVariableDeclaration) node;

		if (changeModifiers != null) {
			final ModifiersRewriter rewriter = new ModifiersRewriter(
					changeModifiers);
			changeModifiers.remove(DotNetModifier.FINAL);
			rewriter.process(context, node, rew, subRewriter, description);
		} else {
			final ChangeModifierDescriptor desc = new ChangeModifierDescriptor();
			desc.remove(DotNetModifier.FINAL);
			final ModifiersRewriter rewriter = new ModifiersRewriter(desc);
			rewriter.process(context, node, rew, subRewriter, description);
		}
		if (name != null) {
			final String replacement = TranslationUtils.getNodeWithComments(
					name, node, fCu, context);
			final ASTNode newName = rew.createStringPlaceholder(replacement,
					field.getName().getNodeType());
			rew.replace(field.getName(), newName, null);
		}
	}

	private void processVariableDeclarationFragment(
			ITranslationContext context, ASTNode node,
			TranslatorASTRewrite rew, TranslatorASTRewrite subRewriter,
			TextEditGroup description) {
		final VariableDeclarationFragment field = (VariableDeclarationFragment) node;

		if (modifyModifiers) {
			if (changeModifiers != null) {
				final ModifiersRewriter rewriter = new ModifiersRewriter(
						changeModifiers);
				final ASTNode parent = field.getParent();
				rewriter
						.process(context, parent, rew, subRewriter, description);
			}
		} else {
			if (name != null) {
				final String replacement = TranslationUtils
						.getNodeWithComments(name, node, fCu, context);
				final ASTNode newName = rew.createStringPlaceholder(
						replacement, field.getName().getNodeType());
				rew.replace(field.getName(), newName, null);
			}
		}
	}

	private void processFieldDeclaration(ITranslationContext context,
			ASTNode node, TranslatorASTRewrite rew,
			TranslatorASTRewrite subRewriter, TextEditGroup description) {
		final FieldDeclaration field = (FieldDeclaration) node;
		if (modifyModifiers && changeModifiers != null) {
			final ModifiersRewriter rewriter = new ModifiersRewriter(
					changeModifiers);
			rewriter.setOnlyJava(true); // TODO
			rewriter.process(context, node, rew, subRewriter, description);
		}

		if (name != null) {
			final VariableDeclarationFragment fragment = TranslationUtils
					.getFrament(field);
			final String replacement = TranslationUtils.getNodeWithComments(
					name, node, fCu, context);
			final ASTNode newName = rew.createStringPlaceholder(replacement,
					fragment.getName().getNodeType());
			rew.replace(fragment.getName(), newName, null);
		}

		if (returnType != null) {
			final ASTNode newName = rew.createStringPlaceholder(returnType,
					field.getType().getNodeType());
			rew.replace(field.getType(), newName, description);
		}
	}

	private void processSimpleName(ITranslationContext context, ASTNode node,
			TranslatorASTRewrite rew) {
		final SimpleName field = (SimpleName) node;
		if (name != null) {
			final String replacement = TranslationUtils.getNodeWithComments(
					name, node, fCu, context);
			final ASTNode newName = rew.createStringPlaceholder(replacement,
					field.getNodeType());
			rew.replace(field, newName, null);
		} else if (format != null
				&& node.getParent().getNodeType() != ASTNode.VARIABLE_DECLARATION_FRAGMENT) {
			final String replacement = TranslationUtils.getNodeWithComments(
					format, node, fCu, context);
			final ASTNode newName = rew.createStringPlaceholder(replacement,
					node.getNodeType());
			rew.replace(field, newName, null);
		}
	}

	private void processQualifiedName(ITranslationContext context,
			ASTNode node, TranslatorASTRewrite rew) {
		final QualifiedName field = (QualifiedName) node;
		if (name != null) {
			final String replacement = TranslationUtils.getNodeWithComments(
					name, node, fCu, context);
			final ASTNode newName = rew.createStringPlaceholder(replacement,
					field.getName().getNodeType());
			rew.replace(field.getName(), newName, null);
		} else if (format != null) {
			final String replacement = TranslationUtils.getNodeWithComments(
					format, node, fCu, context);
			final ASTNode newName = rew.createStringPlaceholder(replacement,
					node.getNodeType());
			rew.replace(field, newName, null);
		}
	}

	private void processFieldAccess(ITranslationContext context, ASTNode node,
			TranslatorASTRewrite rew) {
		final FieldAccess field = (FieldAccess) node;
		if (name != null) {
			final String replacement = TranslationUtils.getNodeWithComments(
					name, node, fCu, context);
			final ASTNode newName = rew.createStringPlaceholder(replacement,
					field.getName().getNodeType());
			rew.replace(field.getName(), newName, null);
		} else if (format != null) {
			final String replacement = TranslationUtils.getNodeWithComments(
					format, node, fCu, context);
			final ASTNode newName = rew.createStringPlaceholder(replacement,
					node.getNodeType());
			rew.replace(field, newName, null);
		}
	}
}
