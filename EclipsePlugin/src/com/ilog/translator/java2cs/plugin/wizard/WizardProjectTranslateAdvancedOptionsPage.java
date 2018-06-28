package com.ilog.translator.java2cs.plugin.wizard;

import java.util.ArrayList;
import java.util.List;

import org.eclipse.jface.wizard.WizardPage;
import org.eclipse.swt.SWT;
import org.eclipse.swt.layout.GridData;
import org.eclipse.swt.layout.GridLayout;
import org.eclipse.swt.widgets.Composite;
import org.eclipse.swt.widgets.Control;
import org.eclipse.swt.widgets.Label;

import com.ilog.translator.java2cs.configuration.TranslatorProjectOptions;
import com.ilog.translator.java2cs.configuration.options.OptionImpl;
import com.ilog.translator.java2cs.plugin.Messages;

public class WizardProjectTranslateAdvancedOptionsPage extends WizardPage
		implements IWizardProjectTranslateOptionsPage {

	protected TranslatorProjectOptions options = null;

	private Composite sShell = null;

	private final List<OptionImpl<?>> translatorAdvancedOptions = new ArrayList<OptionImpl<?>>();
	private final List<OptionImpl<?>> resourcesOptions = new ArrayList<OptionImpl<?>>();
	private final List<OptionImpl<?>> visualStudioProjectOptions = new ArrayList<OptionImpl<?>>();

	private Label otherOptionsLabel = null;
	private Label resourcesOptionsLabel = null;
	private Label visualStudioProjectOptionsLabel = null;

	public WizardProjectTranslateAdvancedOptionsPage(
			TranslatorProjectOptions options) {
		super(Messages
				.getString("WizardProjectTranslateAdvancedOptionsPage.page"));
		setPageComplete(false);
		setTitle(Messages
				.getString("WizardProjectTranslateAdvancedOptionsPage.title"));
		setDescription(Messages
				.getString("WizardProjectTranslateAdvancedOptionsPage.description"));
		this.options = options;
		initOptionsList();
	}

	public void saveOptionsSettings() {

	}

	private void initOptionsList() {
		translatorAdvancedOptions.add(options.getGenerateImplicitMappingfile());
		translatorAdvancedOptions.add(options.getFillRawTypesUse());
		translatorAdvancedOptions.add(options.getNullableEnum());
		translatorAdvancedOptions.add(options.getUnitTestLibraryOption());
		translatorAdvancedOptions.add(options.getRefreshAndBuildOption());
		translatorAdvancedOptions.add(EMPTY_OPTION);
		translatorAdvancedOptions.add(options.getDebug());

	}

	/*
	 * see @DialogPage.setVisible(boolean)
	 */
	@Override
	public void setVisible(boolean visible) {
		super.setVisible(visible);

		if (visible) {
			sShell.setFocus();
		}
	}

	//
	//
	//

	public void createControl(Composite parent) {
		initializeDialogUnits(parent);

		final GridLayout gridLayout = new GridLayout();
		gridLayout.numColumns = 1;

		sShell = new Composite(parent, SWT.NULL);
		sShell.setLayout(gridLayout);

		otherOptionsLabel = new Label(sShell, SWT.LEFT);
		otherOptionsLabel
				.setText(Messages
						.getString("WizardProjectTranslateAdvancedOptionsPage.otherOptionsLabel"));

		final Composite subComposite1 = createSubComposite(sShell);

		for (final OptionImpl<?> option : translatorAdvancedOptions) {
			option.createUIControl(subComposite1);
		}

		resourcesOptionsLabel = new Label(sShell, SWT.LEFT);
		resourcesOptionsLabel
				.setText(Messages
						.getString("WizardProjectTranslateAdvancedOptionsPage.resourcesOptionsLabel"));

		final Composite subComposite2 = createSubComposite(sShell);

		createResourcesOptionsGroup(subComposite2);

		visualStudioProjectOptionsLabel = new Label(sShell, SWT.LEFT);
		visualStudioProjectOptionsLabel
				.setText(Messages
						.getString("WizardProjectTranslateAdvancedOptionsPage.visualStudioOptionsLabel"));

		final Composite subComposite3 = createSubComposite(sShell);

		options.getVsProjectName().createUIControl(subComposite3);
		options.getVsVersionOption().createUIControl(subComposite3);

		final GridData gdSpan2 = new GridData();
		gdSpan2.horizontalSpan = 2;
		gdSpan2.horizontalAlignment = SWT.FILL;
		options.getVsProjectEntryPoint().createUIControl(subComposite3)
				.setLayoutData(gdSpan2);

		setErrorMessage(null);
		this.setMessage(null);
		setControl(sShell);
	}

	private void createResourcesOptionsGroup(Composite subComposite2) {
		final Control resourcesDirControl = options.getResourcesDestDirOption()
				.createUIControl(subComposite2);
		GridData gdSpan2 = new GridData();
		gdSpan2.horizontalSpan = 2;
		gdSpan2.horizontalAlignment = SWT.FILL;
		resourcesDirControl.setLayoutData(gdSpan2);

		options.getResourcesCopyPolicyOption().createUIControl(subComposite2);

		gdSpan2 = new GridData();
		gdSpan2.horizontalSpan = 2;
		gdSpan2.horizontalAlignment = SWT.FILL;
		gdSpan2.grabExcessHorizontalSpace = true;
		final Control resExcludeControl = options
				.getResourcesExcludePatternOption().createUIControl(
						subComposite2);
		resExcludeControl.setLayoutData(gdSpan2);

		gdSpan2 = new GridData();
		gdSpan2.horizontalSpan = 2;
		gdSpan2.horizontalAlignment = SWT.FILL;
		gdSpan2.grabExcessHorizontalSpace = true;
		final Control resInludeControl = options
				.getResourcesIncludePatternOption().createUIControl(
						subComposite2);
		resInludeControl.setLayoutData(gdSpan2);
	}

	private Composite createSubComposite(Composite parent) {
		final GridLayout gridLayout = new GridLayout();
		gridLayout.numColumns = 2;
		gridLayout.horizontalSpacing = 10;
		gridLayout.makeColumnsEqualWidth = true;
		gridLayout.marginLeft = 5;
		gridLayout.marginBottom = 5;

		final Composite subComposite = new Composite(sShell, SWT.NULL);
		subComposite.setLayout(gridLayout);

		return subComposite;
	}

	public void initOptionsValue() {
		if (getControl() != null) {
			for (final OptionImpl<?> option : translatorAdvancedOptions) {
				option.initializeControlValue();
			}
			options.getResourcesDestDirOption().initializeControlValue();
			options.getResourcesCopyPolicyOption().initializeControlValue();
			options.getResourcesExcludePatternOption().initializeControlValue();
			options.getResourcesIncludePatternOption().initializeControlValue();

			options.getVsProjectName().initializeControlValue();
			options.getVsVersionOption().initializeControlValue();
			options.getVsProjectEntryPoint().initializeControlValue();

		}
	}

}
