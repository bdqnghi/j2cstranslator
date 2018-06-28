package com.ilog.translator.java2cs.translation.astrewriter;

import org.eclipse.jdt.core.dom.ASTNode;
import org.eclipse.jdt.core.dom.Expression;
import org.eclipse.jdt.core.dom.MarkerAnnotation;
import org.eclipse.jdt.core.dom.NormalAnnotation;
import org.eclipse.jdt.core.dom.SingleMemberAnnotation;
import org.eclipse.jdt.internal.corext.dom.ASTNodes;
import org.eclipse.text.edits.TextEditGroup;

import com.ilog.translator.java2cs.translation.ITranslationContext;

public class RemoveAnnotationsVisitor extends ASTRewriterVisitor {

	//
	//
	//

	public RemoveAnnotationsVisitor(ITranslationContext context) {
		super(context);
		transformerName = "Remove Annotations";
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
	public void endVisit(MarkerAnnotation node) {
		currentRewriter.remove(node, description);
	}

	@Override
	public void endVisit(NormalAnnotation node) {
		currentRewriter.remove(node, description);
	}

	@Override
	public void endVisit(SingleMemberAnnotation node) {
		final ASTNode parent = node.getParent();
		if (parent.getNodeType() == ASTNode.ANNOTATION_TYPE_DECLARATION) {
			final String fqnName = node.getTypeName().getFullyQualifiedName();
			final Expression expression = node.getValue();
			if (fqnName.equals("Target")) {
				final String asString = ASTNodes.asString(expression);
				if (asString.indexOf("{java.lang.annotation.ElementType.") >= 0) {
					String target = asString.substring(
							"{java.lang.annotation.ElementType.".length(),
							asString.length() - 1);
					if (target.equals("FIELD"))
						target = "Field";
					final String str = "[AttributeUsage(AttributeTargets."
							+ target + ")]";
					final ASTNode replacement = currentRewriter
							.createStringPlaceholder(str, node.getNodeType());
					currentRewriter.replace(node, replacement, description);
				}
			}
		} else {
			currentRewriter.remove(node, description);
		}
	}

}
