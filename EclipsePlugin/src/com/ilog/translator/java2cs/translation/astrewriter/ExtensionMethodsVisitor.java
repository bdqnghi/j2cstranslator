package com.ilog.translator.java2cs.translation.astrewriter;

import org.eclipse.jdt.core.dom.ASTNode;
import org.eclipse.jdt.core.dom.MethodDeclaration;
import org.eclipse.jdt.core.dom.SingleVariableDeclaration;
import org.eclipse.jdt.core.dom.TypeDeclaration;
import org.eclipse.text.edits.TextEditGroup;

import com.ilog.translator.java2cs.configuration.ChangeModifierDescriptor;
import com.ilog.translator.java2cs.translation.ITranslationContext;
import com.ilog.translator.java2cs.translation.noderewriter.ModifiersRewriter;

/**
 * Makes static the class containing extension methods and adds "this" in parameter declaration
 * @author nrrg
 *
 */
@SuppressWarnings("restriction")
public class ExtensionMethodsVisitor extends ASTRewriterVisitor {


	public ExtensionMethodsVisitor(ITranslationContext context) {
		super(context);
		transformerName = "[Custom info] Extension methods visitor";
		description = new TextEditGroup(transformerName);
	}

	@Override
	public boolean needValidation() {
		return false;
	}

	@Override
	public void endVisit(TypeDeclaration node) {
		if(context.IsAnExtensionClassName(node.getName().getIdentifier()))
			/*node.getName().getIdentifier().contains(MoveEnumMembers.EXTENSION_CLASSES_SUFFIX)) */
		{
			context.getLogger().logInfo("[Custom info] Make static class for extension methods: "+node.getName());
			this.addStaticModifier(node);
		}
	}
	
	private void addStaticModifier(TypeDeclaration node) {
		ModifiersRewriter.rewriteModifiers(ChangeModifierDescriptor.ADD_STATIC,
				context, node, currentRewriter, description);
	}

	@Override
	public void endVisit(MethodDeclaration node) {
		ASTNode parent = node.getParent();
		if (parent.getNodeType() == ASTNode.TYPE_DECLARATION) {
			TypeDeclaration parentType = (TypeDeclaration) parent;
			if (context.IsAnExtensionClassName(parentType.getName().getIdentifier()))
				/*
					parentType.getName().getIdentifier().contains(
					MoveEnumMembers.EXTENSION_CLASSES_SUFFIX)) */ {
				if (node.parameters().size() == 0)
					return;
				if(node.parameters().get(0) instanceof SingleVariableDeclaration){
					    SingleVariableDeclaration enumParameter = (SingleVariableDeclaration) node.parameters().get(0);
						final String replacement = "this "+enumParameter	;
						final ASTNode replNode = currentRewriter
								.createStringPlaceholder(replacement, enumParameter
										.getNodeType());
						currentRewriter
								.replace(enumParameter, replNode, description);
					context.getLogger().logInfo(
							"[Custom info] Make extension methods: "
									+ node.getName() + " for class "
									+ parentType.getName().getIdentifier());
				}
			}
		}
	}
}
