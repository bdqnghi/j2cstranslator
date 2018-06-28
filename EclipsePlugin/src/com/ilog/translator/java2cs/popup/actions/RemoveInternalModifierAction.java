package com.ilog.translator.java2cs.popup.actions;

public class RemoveInternalModifierAction extends AbstractModifyModifierAction {

	/**
	 * Constructor for Action1.
	 */
	public RemoveInternalModifierAction() {
		super();
	}

	//
	//
	//

	@Override
	public String getTagValue() {
		return "internal";
	}

	@Override
	public String getAction() {
		return "-";
	}

}
