package com.ilog.translator.java2cs.translation.textrewriter.ast;

import org.eclipse.jdt.core.dom.TypeDeclaration;
import org.eclipse.text.edits.InsertEdit;
import org.eclipse.text.edits.TextEdit;
import org.eclipse.text.edits.TextEditGroup;

import com.ilog.translator.java2cs.translation.ITranslationContext;

public class AddCodeToImplementationVisitor extends ASTTextRewriter {

	//
	//
	//

	public AddCodeToImplementationVisitor(ITranslationContext context) {
		super(context);
		transformerName = "Add Code To Implementation";
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
	public void endVisit(TypeDeclaration node) {
			try {
				String handler = context.getHandlerFromDoc(node, false);
				String codeToAdd = context.getCodeToAddToImplementation(handler);
				if (codeToAdd != null) {
					int endPos = node.getStartPosition() + node.getLength();
					final TextEdit added = new InsertEdit(endPos - 1, codeToAdd);
					edits.add(added);
				}
			} catch (final Exception e) {
				context.getLogger().logException(fCu.getElementName() + " ", e);
			}			
	}	
}
