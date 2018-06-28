package com.ilog.translator.java2cs.translation.astrewriter;

import org.eclipse.jdt.core.dom.ASTNode;
import org.eclipse.jdt.core.dom.Expression;
import org.eclipse.jdt.core.dom.SingleMemberAnnotation;
import org.eclipse.jdt.internal.corext.dom.ASTNodes;
import org.eclipse.text.edits.TextEditGroup;

import com.ilog.translator.java2cs.translation.ITranslationContext;

/**
 * Comments the "single member annotation" (== annotation before single variable
 * declaration)
 * 
 * @author afau
 */
public class SingleAnnotationVisitor extends ASTRewriterVisitor {

	//
	//
	//

	public SingleAnnotationVisitor(ITranslationContext context) {
		super(context);
		transformerName = "Single Member Annotation";
		description = new TextEditGroup(transformerName);
	}

	//
	//
	//

	// @Override
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
			final String str = "/* " + ASTNodes.asString(node) + "*/";
			final ASTNode replacement = currentRewriter
					.createStringPlaceholder(str, node.getNodeType());
			currentRewriter.replace(node, replacement, description);
		}
	}

}
