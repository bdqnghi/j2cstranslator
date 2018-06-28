package com.ilog.translator.java2cs.translation.astrewriter;

import org.eclipse.jdt.core.dom.FieldDeclaration;
import org.eclipse.jdt.core.dom.MethodDeclaration;
import org.eclipse.jdt.core.dom.TypeDeclaration;
import org.eclipse.text.edits.TextEditGroup;

import com.ilog.translator.java2cs.translation.ITranslationContext;

public class ProcessCommentsVisitor extends ASTRewriterVisitor {

	//
	//
	//

	public ProcessCommentsVisitor(ITranslationContext context) {
		super(context);
		transformerName = "Remove Custom Comments";
		description = new TextEditGroup(transformerName);
	}

	//
	//
	//

	@Override
	public boolean isAbridged() {
		return true;
	}

	//
	//
	//

	@Override
	public void endVisit(MethodDeclaration node) {
		// TranslationUtils.processDoc(currentRewriter, node);
	}

	@Override
	public void endVisit(FieldDeclaration node) {
		// TranslationUtils.processDoc(currentRewriter, node);
	}

	@Override
	public void endVisit(TypeDeclaration node) {
		// TranslationUtils.processDoc(currentRewriter, node);
	}

}
