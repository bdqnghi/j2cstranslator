package com.ilog.translator.java2cs.translation.textrewriter.ast;

import java.util.ArrayList;
import java.util.List;

import org.eclipse.core.runtime.CoreException;
import org.eclipse.core.runtime.IProgressMonitor;
import org.eclipse.jdt.core.dom.CompilationUnit;
import org.eclipse.jdt.internal.corext.refactoring.changes.CompilationUnitChange;
import org.eclipse.ltk.core.refactoring.Change;
import org.eclipse.text.edits.MalformedTreeException;
import org.eclipse.text.edits.MultiTextEdit;
import org.eclipse.text.edits.TextEdit;

import com.ilog.translator.java2cs.translation.ITransformer;
import com.ilog.translator.java2cs.translation.ITranslationContext;
import com.ilog.translator.java2cs.translation.astrewriter.TranslationVisitor;

public abstract class ASTTextRewriter extends TranslationVisitor implements
		ITransformer {

	protected boolean runOnce = true;
	protected List<TextEdit> edits;

	//
	//
	//	

	public ASTTextRewriter(ITranslationContext context) {
		super(context);
		edits = new ArrayList<TextEdit>();
	}

	@Override
	public void finalize() {
		edits = null;
	}
	
	//
	//
	//
	
	public boolean needCompilable() {
		return false;	
	}
	
	//
	//
	//

	public boolean needRecovery() {
		return false;
	}

	//
	//
	//

	public void reset() {
		runOnce = true;
		edits = new ArrayList<TextEdit>();
	}

	//
	//
	//

	@Override
	public boolean needValidation() {
		return false;
	}

	//
	//
	//

	@Override
	public boolean runOnce() {
		return true;
	}

	@Override
	public boolean runAgain(CompilationUnit unit) {
		if (runOnce) {
			runOnce = false;
			return true;
		} else {
			return false;
		}
	}

	//
	//
	//

	@Override
	public Change createChange(IProgressMonitor pm, Object param)
			throws CoreException {
		final CompilationUnitChange change = new CompilationUnitChange(
				transformerName, fCu);
		final int length = fCu.getBuffer().getLength();
		change.setEdit(new MultiTextEdit(0, length));
		try {
			for (final TextEdit edit : edits) {
				change.addEdit(edit);
			}
		} catch (final MalformedTreeException e) {
			context.getLogger().logException("Malformed tree in " + fCu.getElementName(), e);
		}

		return change;
	}

}
