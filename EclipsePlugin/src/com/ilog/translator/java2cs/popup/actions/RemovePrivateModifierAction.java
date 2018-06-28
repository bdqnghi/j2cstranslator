package com.ilog.translator.java2cs.popup.actions;

public class RemovePrivateModifierAction extends AbstractModifyModifierAction {

	/**
	 * Constructor for Action1.
	 */
	public RemovePrivateModifierAction() {
		super();
	}

	//
	//
	//

	@Override
	public String getTagValue() {
		return "private";
	}

	@Override
	public String getAction() {
		return "-";
	}

}
