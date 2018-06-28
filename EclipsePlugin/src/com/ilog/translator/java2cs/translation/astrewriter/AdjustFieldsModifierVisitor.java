package com.ilog.translator.java2cs.translation.astrewriter;

import org.eclipse.core.runtime.CoreException;
import org.eclipse.core.runtime.IProgressMonitor;
import org.eclipse.jdt.core.dom.ASTNode;
import org.eclipse.jdt.core.dom.FieldDeclaration;
import org.eclipse.jdt.core.dom.MethodDeclaration;
import org.eclipse.jdt.core.dom.Modifier;
import org.eclipse.jdt.core.dom.ParenthesizedExpression;
import org.eclipse.jdt.core.dom.SimpleName;
import org.eclipse.jdt.core.dom.TypeDeclaration;
import org.eclipse.jdt.core.dom.VariableDeclarationFragment;
import org.eclipse.ltk.core.refactoring.Change;
import org.eclipse.text.edits.TextEditGroup;

import com.ilog.translator.java2cs.configuration.ChangeModifierDescriptor;
import com.ilog.translator.java2cs.configuration.DotNetModifier;
import com.ilog.translator.java2cs.translation.ITranslationContext;
import com.ilog.translator.java2cs.translation.noderewriter.ModifiersRewriter;
import com.ilog.translator.java2cs.translation.util.TranslationUtils;

/**
 * Modify field modifiers
 * 
 * @author afau
 *
 */
public class AdjustFieldsModifierVisitor extends ASTRewriterVisitor {

	//
	//
	//

	public AdjustFieldsModifierVisitor(ITranslationContext context) {
		super(context);
		transformerName = "Adjust Fields Modifier";
		description = new TextEditGroup(transformerName);
	}

	//
	//
	//

	@Override
	public boolean isAbridged() {
		return true;
	}

	//
	//
	//

	@Override
	public boolean applyChange(IProgressMonitor pm) throws CoreException {
		final Change change = createChange(pm, null);
		if (change != null) {
			context.addChange(fCu, change);
		}
		return true;
	}

	//
	//
	//

	@Override
	public void endVisit(TypeDeclaration node) {
		final SimpleName name = node.getName();
		// TODO : improve tests !
		if (TranslationUtils.isAGeneratedConstantsClass(name, context)) {
			final FieldDeclaration[] fields = node.getFields();
			for (final FieldDeclaration field : fields) {
				// Don't know if it's really useful
				final ChangeModifierDescriptor desc = new ChangeModifierDescriptor();
				desc.remove(DotNetModifier.FINAL);
				ModifiersRewriter.rewriteModifiers(desc, context, field,
						currentRewriter, true, true, description);
			}
		} else if (!node.isInterface()) {
			for (final FieldDeclaration fd : node.getFields()) {
				filterFinalFields(fd);
			}
			for (final MethodDeclaration md : node.getMethods()) {
				filterFinalMethods(md);
			}
		}
		if (Modifier.isFinal(node.getModifiers())) {
			final ChangeModifierDescriptor desc = new ChangeModifierDescriptor();
			desc.replace(DotNetModifier.FINAL, DotNetModifier.SEALED);
			ModifiersRewriter.rewriteModifiers(desc, context, node,
					currentRewriter, true, true, description);
		}
	}

	//
	//
	//

	private boolean isConstable(FieldDeclaration field) {
		final VariableDeclarationFragment frag = (VariableDeclarationFragment) field
				.fragments().get(0);
		if (frag.getInitializer() != null) {
			if (frag.resolveBinding() != null
					&& frag.resolveBinding().getType() != null
					&& TranslationUtils.isStringType(frag.resolveBinding()
							.getType()))
				return false;
			switch (frag.getInitializer().getNodeType()) {
			case ASTNode.METHOD_INVOCATION:
				return false;
			case ASTNode.PARENTHESIZED_EXPRESSION:
				final ParenthesizedExpression pe = (ParenthesizedExpression) frag
						.getInitializer();
				if (pe.getExpression().getNodeType() == ASTNode.CONDITIONAL_EXPRESSION) {
					return false;
				}
				break;
			case ASTNode.CONDITIONAL_EXPRESSION:
				return false;
			default:
				return frag.getInitializer().resolveConstantExpressionValue() != null;
			}
			return true;
		}
		return false;
	}

	private void filterFinalMethods(MethodDeclaration node) {
		// ChangeModifierDescriptor desc = new ChangeModifierDescriptor();
		if (Modifier.isFinal(node.getModifiers())) {
			/*
			 * desc.remove(DotNetModifier.FINAL);
			 * ModifiersRewriter.rewriteModifiers(desc, this.context, node,
			 * this.currentRewriter, true, true, description);
			 */
		}
	}

	private void filterFinalFields(FieldDeclaration node) {
		final ChangeModifierDescriptor desc = new ChangeModifierDescriptor();
		if (TranslationUtils.isStaticFinal(node.getModifiers())
				&& TranslationUtils.isPrimitiveType(node.getType())
				&& isConstable(node)) {
			desc.remove(DotNetModifier.STATIC);
			desc.remove(DotNetModifier.FINAL);
			desc.add(DotNetModifier.CONST);
			ModifiersRewriter.rewriteModifiers(desc, context, node,
					currentRewriter, true, true, description);
		} else if (Modifier.isFinal(node.getModifiers())) {
			if (isConstable(node))
				desc.replace(DotNetModifier.FINAL, DotNetModifier.CONST);
			else
				desc.replace(DotNetModifier.FINAL, DotNetModifier.READONLY);
			ModifiersRewriter.rewriteModifiers(desc, context, node,
					currentRewriter, true, true, description);
		}
	}
}
