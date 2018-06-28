package com.ilog.translator.java2cs.translation.astrewriter;

import org.eclipse.jdt.core.dom.ASTNode;
import org.eclipse.jdt.core.dom.IBinding;
import org.eclipse.jdt.core.dom.ITypeBinding;
import org.eclipse.jdt.core.dom.IVariableBinding;
import org.eclipse.jdt.core.dom.Name;
import org.eclipse.jdt.core.dom.QualifiedName;
import org.eclipse.jdt.core.dom.SimpleName;
import org.eclipse.text.edits.TextEditGroup;

import com.ilog.translator.java2cs.translation.ITranslationContext;

/**
 * Check if an access to a field is non ambigues (csharp way) means the there is
 * no class with the same short name than the class that contains the field
 * 
 * @author afau
 * 
 */
public class AdjustQualifierforMovedInterfaceFieldVisitor extends
		ASTRewriterVisitor {

	//
	//
	//

	public AdjustQualifierforMovedInterfaceFieldVisitor(
			ITranslationContext context) {
		super(context);
		transformerName = "Fully Qualified Interface Fields";
		description = new TextEditGroup(transformerName);
	}

	//
	//
	//

	@Override
	public boolean visit(QualifiedName node) {
		final Name qualifier = node.getQualifier();
		final SimpleName name = node.getName();

		final IBinding bindingQualifier = qualifier.resolveBinding();
		final IBinding bindingName = name.resolveBinding();

		if ((bindingQualifier != null) && (bindingName != null)
				&& (bindingQualifier.getKind() == IBinding.TYPE)
				&& (bindingName.getKind() == IBinding.VARIABLE)) {
			return false;
		}

		return true;
	}

	@Override
	public void endVisit(SimpleName node) {
		final IBinding binding = node.resolveBinding();
		if ((binding != null) && (binding.getKind() == IBinding.VARIABLE)) {
			final IVariableBinding typeB = (IVariableBinding) binding;
			final ITypeBinding typeOf = typeB.getDeclaringClass();

			if ((typeOf != null) && typeOf.isInterface() && isValid(node)) {
				rewrite(node, typeB, typeOf);
			}
		}
	}

	//
	//
	//

	private boolean isValid(ASTNode node) {
		switch (node.getNodeType()) {
		case ASTNode.BLOCK:
			return true;
		case ASTNode.INITIALIZER:
			return true;
		case ASTNode.TYPE_DECLARATION:
			return false;
		default:
			return isValid(node.getParent());
		}
	}

	private void rewrite(ASTNode node, IVariableBinding variableBinding,
			ITypeBinding typeBinding) {
		final String code = typeBinding.getQualifiedName() + "."
				+ variableBinding.getName();
		final int nodeType = node.getNodeType();

		final ASTNode newNode = currentRewriter.createStringPlaceholder(code,
				nodeType);
		currentRewriter.replace(node, newNode, description);
	}

}
