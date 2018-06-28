/**
 * 
 */
package com.ilog.translator.java2cs.configuration.options;

import com.ilog.translator.java2cs.plugin.Messages;

public abstract class AbstractEditor<T> implements Editor<T> {
	static private final String MESSAGE_PREFIX = "WizardProjectTranslateOptionsPage.option.";
	static private final String TOOLTIP_SUFFIX = ".tooltip";

	public String getTextLabel(String name) {
		return Messages.getString(MESSAGE_PREFIX + name);
	}

	public String getTooltip(String name) {
		return Messages.getString(MESSAGE_PREFIX + name + TOOLTIP_SUFFIX);
	}
}