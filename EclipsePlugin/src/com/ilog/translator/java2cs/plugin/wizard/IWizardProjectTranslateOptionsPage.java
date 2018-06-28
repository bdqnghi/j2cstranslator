package com.ilog.translator.java2cs.plugin.wizard;

import org.eclipse.swt.SWT;
import org.eclipse.swt.widgets.Composite;
import org.eclipse.swt.widgets.Control;
import org.eclipse.swt.widgets.Label;

import com.ilog.translator.java2cs.configuration.options.Editor;
import com.ilog.translator.java2cs.configuration.options.OptionImpl;
import com.ilog.translator.java2cs.configuration.options.OptionImpl.XMLKind;

public interface IWizardProjectTranslateOptionsPage {

	public class EmptyOptionEditor implements Editor<OptionImpl<Void>> {

		public Control createUIEditor(Composite parent, OptionImpl<Void> option) {
			final Label label = new Label(parent, SWT.NONE);
			return label;
		}

		public void initializeValue(OptionImpl<Void> option) {

		}
	}

	public final OptionImpl<Void> EMPTY_OPTION = new OptionImpl<Void>("EMPTY", null,
			null, OptionImpl.Status.PRODUCTION, null, new EmptyOptionEditor());

	/**
	 * Save the basic options of this page. This is called when the method
	 * performFinish() of the Wizard is called.
	 */
	public void saveOptionsSettings();

	/**
	 * Init the ui of the page with the options value.
	 */
	public void initOptionsValue();

}