package com.ilog.translator.java2cs.plugin.wizard;

import org.eclipse.core.resources.ResourcesPlugin;
import org.eclipse.core.runtime.jobs.ISchedulingRule;
import org.eclipse.jface.wizard.IWizardPage;

public class NewTranslatorProjectWizard extends AbstractProjectWizard {
	private static final String CLASSNAME = "NewTranslatorProjectWizard"; // No_i18n

	// private IlrRuleProjectWizardSelectionPage projectTypePage;

	public NewTranslatorProjectWizard() {
		super(ResourcesPlugin.getWorkspace().getRoot());
		// setDefaultPageImageDescriptor(IlrImages
		// .getImageDescriptorFromId(IlrImageConstants.ID_WIZARD_NEW_RULE_PROJECT));
		// setWindowTitle(getString("Title")); // i18n
		setForcePreviousAndNextButtons(true);
	}

	@Override
	public void addPages() {
		// add the rule project type page
		/*
		 * projectTypePage = new IlrRuleProjectWizardSelectionPage(CLASSNAME +
		 * ".ProjectTypePage"); // No_i18n
		 * projectTypePage.setTitle(getString("ProjectTypePage.Title")); //
		 * No_i18n
		 * projectTypePage.setDescription(getString("ProjectTypePage.Description")); //
		 * No_i18n addPage(projectTypePage);
		 */
	}

	@Override
	protected String getWizardId() {
		return "com.ilog.translator.j2cstranslator.NewTranslatorProjectWizard"; // No_i18n
	}

	private String getString(String key) {
		return ""; // IlrBrmUIPlugin.getPluginInstance().getString(CLASSNAME +
		// "." + key); // No_i18n
	}

	@Override
	protected ISchedulingRule getSchedulingRule() {
		return ResourcesPlugin.getWorkspace().getRoot();
	}

	/*
	 * (non-Javadoc)
	 * 
	 * @see org.eclipse.jface.wizard.Wizard#canFinish()
	 */
	@Override
	public boolean canFinish() {
		final IWizardPage page = getContainer().getCurrentPage();
		return super.canFinish(); // && page != projectTypePage;
	}
}
