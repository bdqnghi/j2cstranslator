package com.ilog.translator.java2cs.popup.actions;

import org.eclipse.jdt.core.dom.BodyDeclaration;
import org.eclipse.jdt.internal.corext.refactoring.structure.CompilationUnitRewrite;

import com.ilog.translator.java2cs.translation.util.TranslationUtils;

public abstract class AbstractModifyModifierAction extends AbstractMarkAsAction {

	/**
	 * Constructor for Action1.
	 */
	public AbstractModifyModifierAction() {
		super();
		applyOnType = true;
		applyOnMethod = true;
		applyOnField = true;
	}

	@Override
	public String getTagName() {
		return "modifiers";
	}

	public abstract String getAction();

	//
	//
	//
	public String getOppositeAction() {
		if (getAction().equals("+"))
			return "-";
		else
			return "+";
	}

	@Override
	protected void createOrUpdateTag(CompilationUnitRewrite cur,
			BodyDeclaration body) {
		TranslationUtils.createOrUpdateTag(cur, body, getTagName(),
				getAction(), getOppositeAction(), getTagValue());
	}

}
