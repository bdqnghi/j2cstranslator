package com.ilog.translator.java2cs.translation.textrewriter.ast;

import org.eclipse.jdt.core.IBuffer;
import org.eclipse.jdt.core.JavaModelException;
import org.eclipse.jdt.core.dom.ASTNode;
import org.eclipse.jdt.core.dom.EnumConstantDeclaration;
import org.eclipse.jdt.core.dom.EnumDeclaration;
import org.eclipse.jdt.core.dom.FieldDeclaration;
import org.eclipse.jdt.core.dom.MethodDeclaration;
import org.eclipse.jdt.core.dom.TypeDeclaration;
import org.eclipse.text.edits.InsertEdit;
import org.eclipse.text.edits.ReplaceEdit;
import org.eclipse.text.edits.TextEdit;
import org.eclipse.text.edits.TextEditGroup;

import com.ilog.translator.java2cs.translation.ITranslationContext;
import com.ilog.translator.java2cs.translation.util.TranslationUtils;

public class RemoveExtraSemiColonVisitor extends ASTTextRewriter {

	//
	//
	//

	public RemoveExtraSemiColonVisitor(ITranslationContext context) {
		super(context);
		transformerName = "Remove Extra semicolon";
		description = new TextEditGroup(transformerName);
	}

	//
	//
	//

	@Override
	public void endVisit(MethodDeclaration node) {
		removeSemiColon(node, false);
	}

	@Override
	public void endVisit(TypeDeclaration node) {
		removeSemiColon(node, false);
	}

	@Override
	public void endVisit(FieldDeclaration node) {
		removeSemiColon(node, false);
	}

	@Override
	public void endVisit(EnumConstantDeclaration node) {
		removeSemiColon(node, true);
	}

	@Override
	public void endVisit(EnumDeclaration node) {
		removeSemiColon(node, false);
	}

	//
	//
	//
	
	private void removeSemiColon(ASTNode node, boolean delayed) {
		try {
			final IBuffer buffer = fCu.getBuffer();
			int index = node.getStartPosition() + node.getLength();
			if (index >= buffer.getLength())
				return;
			String endS = buffer.getText(index, 1);
			int cpt = 0;
			while (endS.equals(";") && index < buffer.getLength() - 1) {
				index++;
				cpt++;
				endS = buffer.getText(index, 1);
			}
			if (cpt > 0) {
				TextEdit edit = null;
				if (delayed)
					edit = new InsertEdit(index - cpt, TranslationUtils.STAT_REMOVE_HERE + cpt
							+ TranslationUtils.END_REMOVE_HERE);
				else
					edit = new ReplaceEdit(index - cpt, 1, " ");
				edits.add(edit);
			}
		} catch (final JavaModelException e) {
			context.getLogger().logException("Probleme during remove extra semicolon", e);
		}
	}
}
