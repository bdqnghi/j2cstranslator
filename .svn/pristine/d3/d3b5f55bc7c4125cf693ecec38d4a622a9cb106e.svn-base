package com.ilog.translator.java2cs.popup.actions;

import java.util.ArrayList;
import java.util.List;

import org.eclipse.jdt.core.dom.BodyDeclaration;
import org.eclipse.jdt.core.dom.TagElement;
import org.eclipse.jdt.internal.corext.refactoring.structure.CompilationUnitRewrite;

import com.ilog.translator.java2cs.translation.util.DocUtils;
import com.ilog.translator.java2cs.translation.util.TranslationUtils;

public class MarkAsTestCaseAction extends AbstractMarkAsAction {

	private final String TESTCASE_TAG = "@testcase";

	/**
	 * Constructor for Action1.
	 */
	public MarkAsTestCaseAction() {
		super();
		applyOnType = true;
	}

	@Override
	public String getTagName() {
		return "testcase"; // dummy
	}

	@Override
	public String getTagValue() {
		return "true"; // dummy
	}

	//
	//
	//

	@Override
	protected void createOrUpdateTag(CompilationUnitRewrite cur,
			BodyDeclaration methodDecl) {
		TagElement tag = null;
		if (!TranslationUtils.containsTag(methodDecl, TESTCASE_TAG)) {
			tag = cur.getAST().newTagElement();
			final List<TagElement> tags = new ArrayList<TagElement>();
			tag.setTagName(TESTCASE_TAG);
			tags.add(tag);
			DocUtils
					.addTagsToDoc(cur.getASTRewrite(), methodDecl, tags);
		}
	}
}
