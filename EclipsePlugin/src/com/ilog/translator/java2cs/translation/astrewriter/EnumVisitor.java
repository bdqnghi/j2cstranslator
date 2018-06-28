package com.ilog.translator.java2cs.translation.astrewriter;

import java.util.List;

import org.eclipse.core.runtime.CoreException;
import org.eclipse.core.runtime.IProgressMonitor;
import org.eclipse.jdt.core.Flags;
import org.eclipse.jdt.core.dom.ASTNode;
import org.eclipse.jdt.core.dom.EnumConstantDeclaration;
import org.eclipse.jdt.core.dom.EnumDeclaration;
import org.eclipse.jdt.core.dom.FieldDeclaration;
import org.eclipse.jdt.core.dom.MethodDeclaration;
import org.eclipse.ltk.core.refactoring.Change;
import org.eclipse.text.edits.TextEditGroup;

import com.ilog.translator.java2cs.configuration.ChangeModifierDescriptor;
import com.ilog.translator.java2cs.translation.ITranslationContext;
import com.ilog.translator.java2cs.translation.noderewriter.ModifiersRewriter;

/**
 * Enum
 * 
 * @author afau
 *
 */
public class EnumVisitor extends ASTRewriterVisitor {

	private boolean inEnum = false;

	//
	//
	//

	public EnumVisitor(ITranslationContext context) {
		super(context);
		transformerName = "Enum cleaner";
		description = new TextEditGroup(transformerName);
	}

	//
	//
	//

	@Override
	public boolean isAbridged() {
		return false;
	}

	//
	//
	//

	@Override
	public boolean applyChange(IProgressMonitor pm) throws CoreException {
		//

		//
		final Change change = createChange(pm, null);
		if (change != null) {
			context.addChange(fCu, change);
		}
		return true;
	}

	//
	//
	//

	@SuppressWarnings("unchecked")
	@Override
	public void endVisit(EnumConstantDeclaration node) {
		final List arguments = node.arguments();
		if (arguments != null) {
			for (int i = 0; i < arguments.size(); i++) {
				currentRewriter.remove((ASTNode) arguments.get(i), description);
			}
		}
	}

	@SuppressWarnings("unchecked")
	@Override
	public boolean visit(EnumDeclaration node) {
		inEnum = true;
		final List superIntterfaces = node.superInterfaceTypes();
		if (superIntterfaces != null) {
			for (int i = 0; i < superIntterfaces.size(); i++) {
				// TODO:
				currentRewriter.remove((ASTNode) superIntterfaces.get(i),
						description);
			}
		}
		if (node.isMemberTypeDeclaration()) {
			if (!Flags.isPublic(node.getFlags())
					&& !Flags.isProtected(node.getFlags())
					&& Flags.isPrivate(node.getFlags())) {
				final ChangeModifierDescriptor change = ChangeModifierDescriptor.CHANGE_PRIVATE_TO_PUBLIC;
				final ModifiersRewriter rew = new ModifiersRewriter(change);
				rew.process(context, node, currentRewriter, null, description);
			}
		}
		return true;
	}

	@Override
	public void endVisit(EnumDeclaration node) {
		inEnum = false;
	}

	@Override
	public void endVisit(MethodDeclaration node) {
		if (inEnum) {
			// TODO: Move it to a new class
			currentRewriter.remove(node, description);
		}
	}

	@Override
	public void endVisit(FieldDeclaration node) {
		if (inEnum) {
			// TODO: Move it to a new class
			currentRewriter.remove(node, description);
		}
	}
}
