package com.ilog.translator.java2cs.translation.astrewriter;

import org.eclipse.core.runtime.CoreException;
import org.eclipse.core.runtime.IProgressMonitor;
import org.eclipse.jdt.core.dom.BodyDeclaration;
import org.eclipse.jdt.core.dom.EnumDeclaration;
import org.eclipse.jdt.core.dom.FieldDeclaration;
import org.eclipse.jdt.core.dom.MethodDeclaration;
import org.eclipse.jdt.core.dom.Modifier;
import org.eclipse.jdt.core.dom.TypeDeclaration;
import org.eclipse.ltk.core.refactoring.Change;
import org.eclipse.text.edits.TextEditGroup;

import com.ilog.translator.java2cs.configuration.ChangeModifierDescriptor;
import com.ilog.translator.java2cs.configuration.DotNetModifier;
import com.ilog.translator.java2cs.translation.ITranslationContext;
import com.ilog.translator.java2cs.translation.noderewriter.ModifiersRewriter;
import com.ilog.translator.java2cs.translation.util.TranslationUtils;

/**
 * In c# an interface can't be static so, remove 'static' modifier to it and it's members
 * In c# an class can be static only if it's super class is System.Object
 *             If not, remove 'static' modifier to it (and it's members ?)
 *             
 * @author afau
 *
 */
public class ChangeStaticTypeMemberModifierVisitor extends ASTRewriterVisitor {

	//
	//
	//

	public ChangeStaticTypeMemberModifierVisitor(ITranslationContext context) {
		super(context);
		transformerName = "Change Static Member Modifier";
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
	public void endVisit(EnumDeclaration node) {
		removeStaticModifier(node);
	}

	@Override
	public void endVisit(TypeDeclaration node) {
		try {
			if (node.isInterface()) {
				// In c# an interface can't be static
				// so, remove 'static' modifier to it and it's
				// members
				if (Modifier.isStatic(node.getModifiers())) {
					for (final FieldDeclaration fd : node.getFields()) {
						addStaticModifier(fd);
					}

					for (final MethodDeclaration md : node.getMethods()) {
						addStaticModifier(md);
					}

					removeStaticModifier(node);
				}
			} else {
				// In c# an class can be static only if it's super
				// class is System.Object
				// If not, remove 'static' modifier to it (and it's
				// members ?)
				// TODO : no static class : definitly yes !
				if (Modifier.isStatic(node.getModifiers())) {
					removeStaticModifier(node);
				} else if (TranslationUtils.isRenamedAnonymousClass(node,
						context)) {
					// Renamed Anonymous Class
					for (final MethodDeclaration md : node.getMethods()) {
						replacePrivateConstructorModifierByPublic(md);
					}

					removeStaticAndAddPublicModifiers(node);
				}
			}
		} catch (final Exception e) {
			e.printStackTrace();
			context.getLogger().logException("", e);
		}
	}

	//
	//
	//

	private void removeStaticAndAddPublicModifiers(TypeDeclaration node) {
		final ChangeModifierDescriptor desc = new ChangeModifierDescriptor();
		desc.remove(DotNetModifier.STATIC);
		if (Modifier.isPrivate(node.getModifiers())) {
			desc.remove(DotNetModifier.PRIVATE);
		}
		desc.add(DotNetModifier.PUBLIC);
		//
		ModifiersRewriter.rewriteModifiers(desc, context, node,
				currentRewriter, description);
	}

	private void removeStaticModifier(BodyDeclaration node) {
		ModifiersRewriter.rewriteModifiers(
				ChangeModifierDescriptor.REMOVE_STATIC, context, node,
				currentRewriter, description);
	}

	private void addStaticModifier(BodyDeclaration node) {
		if (Modifier.isStatic(node.getModifiers())) {
			ModifiersRewriter.rewriteModifiers(
					ChangeModifierDescriptor.ADD_STATIC, context, node,
					currentRewriter, description);
		}
	}

	private void replacePrivateConstructorModifierByPublic(
			MethodDeclaration node) {
		if (node.isConstructor() && Modifier.isPrivate(node.getModifiers())) {
			final ChangeModifierDescriptor desc = new ChangeModifierDescriptor();
			desc.replace(DotNetModifier.PRIVATE, DotNetModifier.PUBLIC);
			//
			ModifiersRewriter.rewriteModifiers(desc, context, node,
					currentRewriter, description);
		}
	}
}
