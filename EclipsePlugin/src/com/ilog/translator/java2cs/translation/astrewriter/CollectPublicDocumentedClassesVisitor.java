package com.ilog.translator.java2cs.translation.astrewriter;

import org.eclipse.jdt.core.dom.EnumDeclaration;
import org.eclipse.jdt.core.dom.ITypeBinding;
import org.eclipse.jdt.core.dom.TypeDeclaration;
import org.eclipse.text.edits.TextEditGroup;

import com.ilog.translator.java2cs.translation.ITranslationContext;
import com.ilog.translator.java2cs.translation.util.TranslationUtils;

/**
 * Comments the "single member annotation" (== annotation before single variable
 * declaration)
 * 
 * @author afau
 */
public class CollectPublicDocumentedClassesVisitor extends ASTRewriterVisitor {

	private String tag = "@internal";
	
	//
	//
	//

	public CollectPublicDocumentedClassesVisitor(ITranslationContext context) {
		super(context);
		transformerName = "Collect public documented classes";
		description = new TextEditGroup(transformerName);
	}

	//
	//
	//

	@Override
	public void endVisit(TypeDeclaration node) {
		if (TranslationUtils.getTagFromDoc(node, tag) == null) {
			ITypeBinding binding = node.resolveBinding();
			String handler = binding.getJavaElement().getHandleIdentifier();
			context.addClassToPublicDocumented(binding.getQualifiedName(), handler);
		}
	}

	@Override
	public void endVisit(EnumDeclaration node) {		
		if (TranslationUtils.getTagFromDoc(node, tag) == null) {
			ITypeBinding binding = node.resolveBinding();
			String handler = binding.getJavaElement().getHandleIdentifier();
			context.addClassToPublicDocumented(node.resolveBinding().getQualifiedName(), handler);
		}
	}
}
