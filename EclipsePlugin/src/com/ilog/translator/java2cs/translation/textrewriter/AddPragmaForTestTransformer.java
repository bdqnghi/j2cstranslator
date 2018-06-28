package com.ilog.translator.java2cs.translation.textrewriter;

import java.util.ArrayList;
import java.util.List;

import org.eclipse.core.runtime.CoreException;
import org.eclipse.core.runtime.IProgressMonitor;
import org.eclipse.jdt.core.IBuffer;
import org.eclipse.jdt.core.ICompilationUnit;
import org.eclipse.jdt.core.dom.ASTNode;
import org.eclipse.text.edits.InsertEdit;
import org.eclipse.text.edits.TextEdit;

import com.ilog.translator.java2cs.translation.ITranslationContext;

public class AddPragmaForTestTransformer extends TextRewriter {

	//
	//
	//	

	private static final String IF_PRAGMA_TEST = "#if ";

	public AddPragmaForTestTransformer(ITranslationContext context) {
		super(context);
		transformerName = "Indent";
	}

	//
	//
	//

	@Override
	public void setCompilationUnit(ICompilationUnit icunit) {
		super.setCompilationUnit(icunit);
		if (!context.hasPackage(icunit)) {
			runOnce = false;
		}
	}

	//
	//
	//

	@Override
	public boolean transform(IProgressMonitor pm, ASTNode cunit) {
		// do nothing
		return true;
	}

	//
	//
	//

	@Override
	public List<TextEdit> computeEdit(IProgressMonitor pm, IBuffer buffer)
			throws CoreException {
		final List<TextEdit> edits = new ArrayList<TextEdit>();
		if (context.isATest(fCu)) {
			edits.add(new InsertEdit(0, IF_PRAGMA_TEST
					+ triggerOption.getValue().toString() + "\r\n"));
			edits.add(new InsertEdit(buffer.getLength(), "\r\n#endif\r\n"));
		}
		return edits;
	}
}
