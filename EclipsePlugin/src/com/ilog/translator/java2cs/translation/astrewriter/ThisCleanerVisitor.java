package com.ilog.translator.java2cs.translation.astrewriter;

import org.eclipse.jdt.core.dom.ASTNode;
import org.eclipse.jdt.core.dom.IBinding;
import org.eclipse.jdt.core.dom.ITypeBinding;
import org.eclipse.jdt.core.dom.Name;
import org.eclipse.jdt.core.dom.ThisExpression;
import org.eclipse.jdt.core.dom.TypeDeclaration;
import org.eclipse.jdt.internal.corext.dom.ASTNodes;
import org.eclipse.text.edits.TextEditGroup;

import com.ilog.translator.java2cs.translation.ITranslationContext;

/**
 * 
 * @author afau
 */
public class ThisCleanerVisitor extends ASTRewriterVisitor {

	//
	//
	//

	public ThisCleanerVisitor(ITranslationContext context) {
		super(context);
		transformerName = "This Cleaner";
		description = new TextEditGroup(transformerName);
	}

	//
	//
	//

	@Override
	public void endVisit(ThisExpression node) {
		final Name thisQualifier = node.getQualifier();
		if (thisQualifier != null) {
			final IBinding thisQBinding = thisQualifier.resolveBinding();
			if (thisQBinding instanceof ITypeBinding) {
				final ITypeBinding tb = (ITypeBinding) thisQBinding;
				final TypeDeclaration enclosing = (TypeDeclaration) ASTNodes
						.getParent(node, ASTNode.TYPE_DECLARATION);
				if (enclosing != null) {
					final ITypeBinding eTb = enclosing.resolveBinding();
					if (eTb.getErasure().isEqualTo(tb.getErasure())) {
						currentRewriter
								.remove(node.getQualifier(), description);
					}
				}
			}
		}
	}
}
