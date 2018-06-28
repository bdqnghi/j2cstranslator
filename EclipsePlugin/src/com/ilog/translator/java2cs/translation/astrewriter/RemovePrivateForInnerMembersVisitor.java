package com.ilog.translator.java2cs.translation.astrewriter;

import java.util.List;

import org.eclipse.jdt.core.dom.AST;
import org.eclipse.jdt.core.dom.IExtendedModifier;
import org.eclipse.jdt.core.dom.IMethodBinding;
import org.eclipse.jdt.core.dom.ITypeBinding;
import org.eclipse.jdt.core.dom.MethodDeclaration;
import org.eclipse.jdt.core.dom.Modifier;
import org.eclipse.jdt.core.dom.Modifier.ModifierKeyword;
import org.eclipse.text.edits.TextEditGroup;

import com.ilog.translator.java2cs.translation.ITranslationContext;

public class RemovePrivateForInnerMembersVisitor extends ASTRewriterVisitor {

	//
	//
	//

	public RemovePrivateForInnerMembersVisitor(ITranslationContext context) {
		super(context);
		transformerName = "Remove Private For Inner Members";
		description = new TextEditGroup(transformerName);
	}

	//
	//
	//

	@SuppressWarnings("unchecked")
	@Override
	public void endVisit(MethodDeclaration node) {
		final IMethodBinding methodB = node.resolveBinding();
		final ITypeBinding typeB = methodB.getDeclaringClass();
		if (typeB.isMember()) {
			final List<IExtendedModifier> modifiers = node.modifiers();
			for (final IExtendedModifier mod : modifiers) {
				if (mod.isModifier()) {
					final Modifier m = (Modifier) mod;
					if (m.isPrivate()) {
						final AST ast = node.getAST();
						final Modifier publicM = ast
								.newModifier(ModifierKeyword.PUBLIC_KEYWORD);
						currentRewriter.replace(m, publicM, description);
					}
				}
			}
		}
	}
}
