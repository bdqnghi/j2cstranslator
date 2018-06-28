package com.ilog.translator.java2cs.popup.actions;

public class AddPublicModifierAction extends AbstractModifyModifierAction {

	/**
	 * Constructor for Action1.
	 */
	public AddPublicModifierAction() {
		super();
	}

	//
	//
	//

	@Override
	public String getTagValue() {
		return "public";
	}

	@Override
	public String getAction() {
		return "+";
	}

}
