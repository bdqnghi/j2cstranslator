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
import com.ilog.translator.java2cs.util.TranslationModelUtil;

public class IndentTransformer extends TextRewriter {

	//
	//
	//	

	public IndentTransformer(ITranslationContext context) {
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
		// TODO : use mapper to retrieve that string

		final String namespace_k = context.getMapper().getKeyword(
				TranslationModelUtil.PACKAGE_KEYWORD,
				TranslationModelUtil.CSHARP_MODEL); // WAS: "namespace";
		final int namespace_l = namespace_k.length();
		int start = 0;
		boolean hasPackageName = false;

		// search for namespace
		for (int i = 0; i < buffer.getLength() - 1; i++) {
			if ((i + namespace_l < buffer.getLength())
					&& buffer.getText(i, namespace_l).equals(namespace_k)) {
				start = i + 1;
				hasPackageName = true;
				break;
			}
		}


		
		String tab = context.getConfiguration().getOptions().getTabValue();
		
		if (hasPackageName) {
			final char[] chars = buffer.getCharacters();

			for (int i = start; i < chars.length - 1; i++) {
				if (chars[i] == '\n') {
				    // IDENTATION MODIFICATION - use spaces instead of tab
					edits.add(new InsertEdit(i + 1, tab));
				}
			}

			edits.add(new InsertEdit(buffer.getLength(), "}"));
		}

		edits.add(new InsertEdit(buffer.getLength(), "\r\n")); // BR-1901363

		return edits;
	}
}
