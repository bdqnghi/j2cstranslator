package com.ilog.translator.java2cs.translation.astrewriter;

import org.eclipse.jdt.core.dom.IBinding;
import org.eclipse.jdt.core.dom.IVariableBinding;
import org.eclipse.jdt.core.dom.SimpleName;
import org.eclipse.text.edits.TextEditGroup;

import com.ilog.translator.java2cs.translation.ITranslationContext;
import com.ilog.translator.java2cs.translation.noderewriter.FieldRewriter;

/**
 * 
 * @author afau
 */
public class RenameFieldsWithForbiddenNameVisitor extends ASTRewriterVisitor {

	//
	//
	//

	public RenameFieldsWithForbiddenNameVisitor(ITranslationContext context) {
		super(context);
		transformerName = "Rename fields with forbiddenName";
		description = new TextEditGroup(transformerName);
	}

	//
	//
	//

	@Override
	public void endVisit(SimpleName node) {
		final IBinding namebinding = node.resolveBinding();
		if (namebinding != null) {
			if (namebinding instanceof IVariableBinding) {
				// So it's a field access
				final IVariableBinding vb = (IVariableBinding) namebinding;
				if (vb.isParameter()) {
					final String name = context.getModel().getVariable(
							vb.getName());
					if (name != null) {
						final FieldRewriter fr = new FieldRewriter(name);
						applyNodeRewriter(node, fr, description);
					}
				}
			}
		}
	}
}
