package com.ilog.translator.java2cs.popup.actions;

public class MarkAsNotGeneratedAction extends AbstractMarkAsAction {

	/**
	 * Constructor for Action1.
	 */
	public MarkAsNotGeneratedAction() {
		super();
		applyOnType = true;
		applyOnMethod = true;
		applyOnField = true;
	}

	@Override
	public String getTagName() {
		return "generation";
	}

	@Override
	public String getTagValue() {
		return "false";
	}
}
