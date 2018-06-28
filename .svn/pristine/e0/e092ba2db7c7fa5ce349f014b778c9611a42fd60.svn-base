package com.ilog.translator.java2cs.translation.noderewriter;

import java.util.List;

import org.eclipse.jdt.core.dom.AST;
import org.eclipse.jdt.core.dom.ASTNode;
import org.eclipse.jdt.core.dom.Block;
import org.eclipse.jdt.core.dom.Initializer;
import org.eclipse.jdt.core.dom.MethodDeclaration;
import org.eclipse.jdt.core.dom.Modifier;
import org.eclipse.jdt.core.dom.SimpleName;
import org.eclipse.text.edits.TextEditGroup;

import com.ilog.translator.java2cs.translation.ITranslationContext;
import com.ilog.translator.java2cs.translation.TranslatorASTRewrite;
import com.ilog.translator.java2cs.translation.util.TranslationUtils;

public class InitializerRewriter extends AbstractNodeRewriter {

	private final String name;

	//
	//
	//

	public InitializerRewriter(String name) {
		this.name = name;
	}

	//
	//
	//

	@SuppressWarnings("unchecked")
	@Override
	public void process(ITranslationContext context, ASTNode node,
			TranslatorASTRewrite rew, TranslatorASTRewrite subRewriter,
			TextEditGroup description) {
		final Initializer init = (Initializer) node;
		final AST ast = node.getAST();
		final SimpleName newName = ast.newSimpleName(name);

		if (Modifier.isStatic(init.getModifiers())) {
			final MethodDeclaration m = ast.newMethodDeclaration();
			final String code = TranslationUtils.replaceByNewValue(init
					.getBody(), rew, fCu, context.getLogger());
			m.setBody((Block) rew.createStringPlaceholder(code, ASTNode.BLOCK));
			final List modifiers = init.modifiers();
			for (final Object modifier : modifiers) {
				final ASTNode mod_node = (ASTNode) modifier;
				if (mod_node.getNodeType() == ASTNode.MODIFIER) {
					final Modifier mod = (Modifier) mod_node;
					if (!mod.isPublic() && !mod.isProtected()) {
						m.modifiers().add(ASTNode.copySubtree(ast, mod));
					}
				}
			}
			m.setName(newName);
			m.setConstructor(true);
			rew.replace(node, m, null);
		}
	}
}
