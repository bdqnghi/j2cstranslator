package com.ilog.translator.java2cs.translation.astrewriter;

import org.eclipse.jdt.core.dom.ITypeBinding;
import org.eclipse.jdt.core.dom.ParameterizedType;
import org.eclipse.jdt.core.dom.SimpleType;
import org.eclipse.text.edits.TextEditGroup;

import com.ilog.translator.java2cs.translation.ITranslationContext;
import com.ilog.translator.java2cs.translation.util.TranslationUtils;

/**
 * Check if an access to a field is non ambigues (csharp way) means the there is
 * no class with the same short name than the class that contains the field
 * 
 * @author afau
 * 
 */
public class QualifiedInnerSimpleTypeReference extends ASTRewriterVisitor {

	//
	//
	//

	public QualifiedInnerSimpleTypeReference(ITranslationContext context) {
		super(context);
		transformerName = "Qualified Inner SimpleType Reference";
		description = new TextEditGroup(transformerName);
	}

	//
	//
	//

	@Override
	public boolean visit(ParameterizedType type) {
		return false; // Computed in the next visitor
	}

	@Override
	public boolean visit(SimpleType type) {
		final ITypeBinding tBinding = type.resolveBinding();
		if (tBinding != null && tBinding.isMember()) {
			if (type.getName().isSimpleName()) {
				final String replacement = TranslationUtils
						.replaceByQualifiedName(type, fCu, currentRewriter,
								context);
				if (replacement != null)
					currentRewriter.replace(type, currentRewriter
							.createStringPlaceholder(replacement, type
									.getNodeType()), description);
				return false;
			}
		}
		return true;
	}
}
