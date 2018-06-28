package com.ilog.translator.java2cs.translation.astrewriter;

import org.eclipse.core.runtime.CoreException;
import org.eclipse.core.runtime.IProgressMonitor;
import org.eclipse.jdt.core.dom.BodyDeclaration;
import org.eclipse.jdt.core.dom.FieldDeclaration;
import org.eclipse.jdt.core.dom.MethodDeclaration;
import org.eclipse.jdt.core.dom.Modifier;
import org.eclipse.jdt.core.dom.TypeDeclaration;
import org.eclipse.ltk.core.refactoring.Change;
import org.eclipse.text.edits.TextEditGroup;

import com.ilog.translator.java2cs.configuration.ChangeModifierDescriptor;
import com.ilog.translator.java2cs.translation.ITranslationContext;
import com.ilog.translator.java2cs.translation.noderewriter.ModifiersRewriter;

//
// In C# interface members can't have public and/or abstract modifier !
//
public class ChangeInterfaceFieldModifierVisitor extends ASTRewriterVisitor {

	//
	//
	//

	public ChangeInterfaceFieldModifierVisitor(ITranslationContext context) {
		super(context);
		transformerName = "Remove forbidden Interface members modifiers";
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
		if (node.isInterface()) {
			for (final FieldDeclaration fd : node.getFields()) {
				removeForbiddenModifiers(fd);
			}

			for (final MethodDeclaration md : node.getMethods()) {
				removeForbiddenModifiers(md);
			}
		}
	}

	//
	//
	//

	private void removeForbiddenModifiers(BodyDeclaration node) {
		if (Modifier.isPublic(node.getModifiers())
				|| Modifier.isAbstract(node.getModifiers())) {
			ModifiersRewriter.rewriteModifiers(
					ChangeModifierDescriptor.REMOVE_PUBLIC_AND_ABSTRACT,
					context, node, currentRewriter, description);
		}
	}
}
