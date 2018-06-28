package com.ilog.translator.java2cs.plugin.wizard;

import java.io.File;
import java.util.ArrayList;
import java.util.List;

import org.eclipse.core.resources.IWorkspace;
import org.eclipse.core.runtime.IPath;
import org.eclipse.core.runtime.Path;
import org.eclipse.jface.dialogs.IDialogConstants;
import org.eclipse.jface.dialogs.IMessageProvider;
import org.eclipse.jface.wizard.WizardPage;
import org.eclipse.swt.SWT;
import org.eclipse.swt.events.SelectionAdapter;
import org.eclipse.swt.events.SelectionEvent;
import org.eclipse.swt.graphics.Font;
import org.eclipse.swt.layout.GridData;
import org.eclipse.swt.layout.GridLayout;
import org.eclipse.swt.widgets.Button;
import org.eclipse.swt.widgets.Composite;
import org.eclipse.swt.widgets.Control;
import org.eclipse.swt.widgets.DirectoryDialog;
import org.eclipse.swt.widgets.Event;
import org.eclipse.swt.widgets.Label;
import org.eclipse.swt.widgets.Text;

import com.ilog.translator.java2cs.configuration.TranslatorProjectOptions;
import com.ilog.translator.java2cs.configuration.options.OptionImpl;
import com.ilog.translator.java2cs.plugin.Messages;
import com.ilog.translator.java2cs.plugin.TranslationPlugin;

public class WizardProjectTranslateOptionsPage extends WizardPage implements
		org.eclipse.swt.widgets.Listener, IWizardProjectTranslateOptionsPage {

	private TranslatorProjectOptions options = null;

	// Keep track of the directory that we browsed to last time
	// the wizard was invoked.
	private static String previouslyBrowsedDirectory = ""; //$NON-NLS-1$

	private Composite sShell = null;

	private final List<OptionImpl<?>> translatorBasicOptions = new ArrayList<OptionImpl<?>>();
	private final List<OptionImpl<?>> projectConfigOptions = new ArrayList<OptionImpl<?>>();

	private Label java2CsTranslationOptionsLabel = null;
	private Label renamingOptionsLabel = null;
	private Label projectTranslationOptionsLabel = null;

	private Text sourcesDestDirText = null;
	private Label destdirLabel = null;

	private Text mappingAssemblyLocationText = null;
	private Label mappingAssemblyLocationLabel = null;

	//
	//
	//

	public WizardProjectTranslateOptionsPage(TranslatorProjectOptions options) {
		super(Messages.getString("WizardProjectTranslateOptionsPage.page"));
		setPageComplete(false);
		setTitle(Messages.getString("WizardProjectTranslateOptionsPage.title"));
		setDescription(Messages
				.getString("WizardProjectTranslateOptionsPage.description"));
		this.options = options;
		initOptionsList();
	}

	/*
	 * (non-Javadoc)
	 * 
	 * @see com.ilog.translator.java2cs.plugin.IWizardProjectTranslateOptionsPage#saveOptionsSettings()
	 */
	public void saveOptionsSettings() {
		options.getGlobalOptions().setMappingAssemblyLocation(
				mappingAssemblyLocationText.getText());
		options.getSourcesDestDirOption()
				.setValue(sourcesDestDirText.getText());
		options.getGlobalOptions().setBaseDestDir(sourcesDestDirText.getText());
	}

	/**
	 * Init the ui options list
	 * 
	 * @param options
	 */
	private void initOptionsList() {
		translatorBasicOptions.add(options.getUseGenerics());
		translatorBasicOptions.add(options.getAutoCovariant());
		translatorBasicOptions.add(options.getAutoProperties());
		translatorBasicOptions.add(options.getAddNotBrowsableAttribute());
		translatorBasicOptions.add(options.getAutoComputeDepends());

		projectConfigOptions.add(options.getVsGenerationMode());
		projectConfigOptions.add(options.getRemoveTempProjectOption());

		// this.projectConfigOptions.add(options.getSourcesDestDirOption());

		// mapping

	}

	//
	//
	//

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
		// sShell.setSize(new Point(389, 147));

		java2CsTranslationOptionsLabel = new Label(sShell, SWT.LEFT);
		java2CsTranslationOptionsLabel
				.setText(Messages
						.getString("WizardProjectTranslateOptionsPage.java2csTranslatorOptionsLabel")); //$NON-NLS-1$

		final Composite subComposite1 = createSubComposite(2);

		for (final OptionImpl<?> option : translatorBasicOptions) {
			option.createUIControl(subComposite1);
		}

		renamingOptionsLabel = new Label(sShell, SWT.LEFT);
		renamingOptionsLabel
				.setText(Messages
						.getString("WizardProjectTranslateOptionsPage.renamingOptionsLabel")); //$NON-NLS-1$

		createRenamingOptionsGroup();

		projectTranslationOptionsLabel = new Label(sShell, SWT.LEFT);
		projectTranslationOptionsLabel
				.setText(Messages
						.getString("WizardProjectTranslateOptionsPage.projectTranslationOptionsLabel")); //$NON-NLS-1$

		final Composite subComposite3 = createSubComposite(2);

		for (final OptionImpl<?> option : projectConfigOptions) {
			option.createUIControl(subComposite3);
		}

		final GridLayout glSubComp4 = new GridLayout();
		glSubComp4.numColumns = 2;
		glSubComp4.horizontalSpacing = 10;
		glSubComp4.makeColumnsEqualWidth = false;
		glSubComp4.marginWidth = 0;

		final Composite subComposite4 = new Composite(sShell, SWT.NULL);
		subComposite4.setLayout(glSubComp4);

		final GridData gdSubComp4 = new GridData();
		gdSubComp4.horizontalAlignment = SWT.FILL;
		subComposite4.setLayoutData(gdSubComp4);

		final GridData gridData31 = new GridData();
		gridData31.horizontalAlignment = SWT.LEFT;
		gridData31.verticalAlignment = SWT.CENTER;

		mappingAssemblyLocationLabel = new Label(subComposite4, SWT.LEFT);
		mappingAssemblyLocationLabel
				.setText(Messages
						.getString("WizardProjectTranslateOptionsPage.option.mappingAssemblyLocation")); //$NON-NLS-1$
		mappingAssemblyLocationLabel
				.setToolTipText(Messages
						.getString("WizardProjectTranslateOptionsPage.option.mappingAssemblyLocation.tooltip")); //$NON-NLS-1$
		mappingAssemblyLocationLabel.setLayoutData(gridData31);

		final GridData gridData32 = new GridData();
		gridData32.horizontalAlignment = SWT.FILL;
		gridData32.grabExcessHorizontalSpace = true;
		gridData32.verticalAlignment = SWT.CENTER;

		mappingAssemblyLocationText = new Text(subComposite4, SWT.BORDER);
		mappingAssemblyLocationText.setText(options.getGlobalOptions()
				.getMappingAssemblyLocation());
		mappingAssemblyLocationText
				.setToolTipText(Messages
						.getString("WizardProjectTranslateOptionsPage.option.mappingAssemblyLocation.tooltip")); //$NON-NLS-1$
		mappingAssemblyLocationText.setLayoutData(gridData32);

		createDestinationGroup(subComposite4);

		updatePageCompletion();
		setControl(sShell);
	}

	private void createRenamingOptionsGroup() {
		final Composite subComposite2 = createSubComposite(2);

		options.getDefaultPackageMappingBehavior().createUIControl(
				subComposite2);
		options.getExactDirectoryName().createUIControl(subComposite2);

		final Control memberMappingControl = options
				.getDefaultMemberMappingBehavior().createUIControl(
						subComposite2);
		GridData gdSpan2 = new GridData();
		gdSpan2.horizontalSpan = 2;
		memberMappingControl.setLayoutData(gdSpan2);

		final Control flatPatternControl = options.getFlatPatternOption()
				.createUIControl(subComposite2);
		gdSpan2 = new GridData();
		gdSpan2.horizontalSpan = 2;
		gdSpan2.horizontalAlignment = SWT.FILL;
		flatPatternControl.setLayoutData(gdSpan2);
	}

	private Composite createSubComposite(int numColumns) {
		final GridLayout gridLayout = new GridLayout();
		gridLayout.numColumns = numColumns;
		gridLayout.horizontalSpacing = 10;
		gridLayout.makeColumnsEqualWidth = true;
		gridLayout.marginLeft = 5;
		gridLayout.marginBottom = 7;

		final Composite subComposite = new Composite(sShell, SWT.NULL);
		subComposite.setLayout(gridLayout);
		return subComposite;
	}

	protected void createDestinationGroup(Composite parent) {

		initializeDialogUnits(parent);

		// destination specification group
		final Composite destinationSelectionGroup = new Composite(parent,
				SWT.NONE);
		final GridLayout layout = new GridLayout();
		layout.numColumns = 3;
		layout.makeColumnsEqualWidth = false;
		layout.marginWidth = 0;

		destinationSelectionGroup.setLayout(layout);
		final GridData gdGroup = new GridData();
		gdGroup.horizontalSpan = 2;
		gdGroup.horizontalAlignment = SWT.FILL;
		destinationSelectionGroup.setLayoutData(gdGroup);

		GridData gridData = new GridData();
		gridData.horizontalAlignment = SWT.LEFT;
		destdirLabel = new Label(destinationSelectionGroup, SWT.NONE);
		destdirLabel
				.setText(Messages
						.getString("WizardProjectTranslateOptionsPage.option.sourcesOutputDir")); //$NON-NLS-1$
		destdirLabel
				.setToolTipText(Messages
						.getString("WizardProjectTranslateOptionsPage.option.sourcesOutputDir.tooltip")); //$NON-NLS-1$
		destdirLabel.setLayoutData(gridData);

		gridData = new GridData();
		gridData.horizontalAlignment = SWT.FILL;
		gridData.grabExcessHorizontalSpace = true;

		// destination name entry field
		sourcesDestDirText = new Text(destinationSelectionGroup, SWT.BORDER);
		sourcesDestDirText.setText(options.getRealSourcesDestDir());
		sourcesDestDirText
				.setToolTipText(Messages
						.getString("WizardProjectTranslateOptionsPage.option.sourcesOutputDir.tooltip")); //$NON-NLS-1$
		sourcesDestDirText.setLayoutData(gridData);
		sourcesDestDirText.addListener(SWT.Modify, this);

		// destination browse button
		createBrowseButton(destinationSelectionGroup, null);
	}

	@Override
	public boolean isPageComplete() {
		return validatePath(sourcesDestDirText, false);
	}

	protected boolean validatePath(Text pathField, boolean allowEmpty) {
		String pathFieldContents = ""; //$NON-NLS-1$
		if (pathField != null) {
			pathFieldContents = pathField.getText().trim();
		}

		if (!(allowEmpty && pathFieldContents.equals(""))) { //$NON-NLS-1$
			if (pathFieldContents.equals("")) { //$NON-NLS-1$
				setErrorMessage(null);
				this
						.setMessage(
								Messages
										.getString("WizardProjectTranslateOptionsPage.pathEmpty"), IMessageProvider.INFORMATION); //$NON-NLS-1$

				return false;
			}

			final IPath path = new Path(""); //$NON-NLS-1$

			if (!path.isValidPath(pathFieldContents)) {
				setErrorMessage(Messages
						.getString("WizardProjectTranslateOptionsPage.invalidPath")); //$NON-NLS-1$

				return false;
			}

			final File dir = new File(pathFieldContents);
			if (!dir.exists()) {
				setErrorMessage(Messages
						.getString("WizardProjectTranslateOptionsPage.pathDoesNotExist")); //$NON-NLS-1$

				return false;
			}
			if (!dir.isDirectory()) {
				setErrorMessage(Messages
						.getString("WizardProjectTranslateOptionsPage.notADirectoryPath")); //$NON-NLS-1$

				return false;
			}
		}

		setErrorMessage(null);
		this.setMessage(null);

		return true;
	}

	protected void createBrowseButton(Composite projectGroup, Font dialogFont) {
		final Button browseButton = new Button(projectGroup, SWT.PUSH);
		browseButton.setText(Messages
				.getString("WizardProjectTranslateOptionsPage.browse")); //$NON-NLS-1$
		browseButton.setFont(dialogFont);
		browseButton.setLayoutData(getButtonLayoutData());

		browseButton.addSelectionListener(new SelectionAdapter() {
			@Override
			public void widgetSelected(SelectionEvent event) {
				handleLocationBrowseButtonPressed();
			}
		});
	}

	@SuppressWarnings("deprecation")
	protected GridData getButtonLayoutData() {
		final GridData browseButtonData = new GridData();

		browseButtonData.heightHint = convertVerticalDLUsToPixels(IDialogConstants.BUTTON_HEIGHT);
		browseButtonData.widthHint = convertHorizontalDLUsToPixels(IDialogConstants.BUTTON_WIDTH);
		return browseButtonData;
	}

	/**
	 * Open an appropriate directory browser
	 */
	void handleLocationBrowseButtonPressed() {
		final String selectedDirectory = browseDirectory(sourcesDestDirText,
				previouslyBrowsedDirectory);
		if (selectedDirectory != null) {
			previouslyBrowsedDirectory = selectedDirectory;
		}
	}

	protected String browseDirectory(Text pathField, String directory) {
		final DirectoryDialog dialog = new DirectoryDialog(pathField.getShell());
		dialog.setMessage(Messages
				.getString("WizardProjectTranslateOptionsPage.directory")); //$NON-NLS-1$

		String dirName = pathField.getText().trim();

		if (dirName.length() == 0) {
			dirName = directory;
		}

		if (dirName.length() == 0) {
			dialog.setFilterPath(getWorkspace().getRoot().getLocation()
					.toOSString());
		} else {
			final File path = new File(dirName);

			if (path.exists()) {
				dialog.setFilterPath(new Path(dirName).toOSString());
			}
		}

		final String selectedDirectory = dialog.open();

		if (selectedDirectory != null) {
			pathField.setText(selectedDirectory);
		}

		return selectedDirectory;
	}

	private IWorkspace getWorkspace() {
		final IWorkspace workspace = TranslationPlugin.getWorkspace();

		return workspace;
	}

	/*
	 * (non-Javadoc)
	 * 
	 * @see com.ilog.translator.java2cs.plugin.IWizardProjectTranslateOptionsPage#initOptionsValue()
	 */
	public void initOptionsValue() {
		if (getControl() != null) {
			for (final OptionImpl<?> option : translatorBasicOptions) {
				option.initializeControlValue();
			}
			for (final OptionImpl<?> option : projectConfigOptions) {
				option.initializeControlValue();
			}

			// Renaming options
			options.getDefaultPackageMappingBehavior().initializeControlValue();
			options.getExactDirectoryName().initializeControlValue();
			options.getDefaultMemberMappingBehavior().initializeControlValue();
			options.getFlatPatternOption().initializeControlValue();

			options.getCreateTranslatorConfigurationFiles()
					.initializeControlValue();
		}
		sourcesDestDirText.setText(options.getRealSourcesDestDir());
		mappingAssemblyLocationText.setText(options.getGlobalOptions()
				.getMappingAssemblyLocation());
	}

	private void updatePageCompletion() {
		boolean complete;

		complete = isPageComplete();

		if (complete) {
			setMessage(null);
		}
		setPageComplete(complete);
	}

	//
	// Methods to implements Listener
	//
	public void handleEvent(Event event) {
		updatePageCompletion();
	}

}
