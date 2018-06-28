package com.ilog.translator.java2cs.plugin.wizard;

import java.util.ArrayList;
import java.util.HashSet;

import org.eclipse.core.resources.IProject;
import org.eclipse.core.resources.IWorkspace;
import org.eclipse.core.resources.ResourcesPlugin;
import org.eclipse.jface.dialogs.IDialogConstants;
import org.eclipse.jface.viewers.CheckStateChangedEvent;
import org.eclipse.jface.viewers.CheckboxTableViewer;
import org.eclipse.jface.viewers.ICheckStateListener;
import org.eclipse.jface.viewers.IStructuredContentProvider;
import org.eclipse.jface.viewers.TableLayout;
import org.eclipse.jface.wizard.WizardPage;
import org.eclipse.swt.SWT;
import org.eclipse.swt.layout.GridData;
import org.eclipse.swt.layout.GridLayout;
import org.eclipse.swt.widgets.Button;
import org.eclipse.swt.widgets.Composite;
import org.eclipse.swt.widgets.Event;
import org.eclipse.swt.widgets.Listener;
import org.eclipse.swt.widgets.Table;
import org.eclipse.ui.model.WorkbenchContentProvider;
import org.eclipse.ui.model.WorkbenchLabelProvider;

import com.ilog.translator.java2cs.plugin.Messages;
import com.ilog.translator.java2cs.plugin.util.UIHelpers;

public class WizardProjectTranslateSelectPage extends WizardPage {

	private Composite projectComposite;

	private CheckboxTableViewer tableViewer;
	private Table table;

	private final HashSet selectedProjects = new HashSet();
	private final ArrayList referenceCountProjects = new ArrayList();

	private class ProjectContentProvider extends WorkbenchContentProvider
			implements IStructuredContentProvider {

		@Override
		public Object[] getChildren(Object element) {
			if (!(element instanceof IWorkspace)) {
				return new Object[0];
			}

			return ((IWorkspace) element).getRoot().getProjects();
		}
	}

	/**
	 * Creates a new project reference wizard page.
	 * 
	 * @param pageName
	 *            the name of this page
	 */
	public WizardProjectTranslateSelectPage() {
		super(Messages.getString("WizardProjectTranslateSelectPage.page"));
		setPageComplete(false);
		setTitle(Messages.getString("WizardProjectTranslateSelectPage.title"));
		setDescription(Messages
				.getString("WizardProjectTranslateSelectPage.description"));
	}

	public void createControl(Composite parent) {

		projectComposite = new Composite(parent, SWT.NULL);
		final GridLayout gridLayout = new GridLayout(1, true);
		projectComposite.setLayout(gridLayout);

		initializeDialogUnits(projectComposite);

		// Adds the project table
		addProjectSection(projectComposite);
		initializeProjects();
		updateEnablement();
		setControl(projectComposite);
	}

	public void setFocus() {
		projectComposite.setFocus();
	}

	private void addProjectSection(Composite composite) {

		UIHelpers.createPlainLabel(composite, Messages
				.getString("WizardProjectTranslateSelectPage.selectProjects"));

		table = new Table(composite, SWT.CHECK | SWT.BORDER | SWT.V_SCROLL
				| SWT.H_SCROLL);
		tableViewer = new CheckboxTableViewer(table);
		table.setLayout(new TableLayout());
		GridData data = new GridData(GridData.FILL_BOTH);
		data.heightHint = 300;
		table.setLayoutData(data);
		tableViewer.setContentProvider(new ProjectContentProvider());
		tableViewer.setLabelProvider(new WorkbenchLabelProvider());
		tableViewer.addCheckStateListener(new ICheckStateListener() {
			public void checkStateChanged(CheckStateChangedEvent event) {
				final Object temp = event.getElement();
				if (temp instanceof IProject) {
					final IProject project = (IProject) event.getElement();
					if (event.getChecked()) {
						selectedProjects.add(project);
						referenceCountProjects.add(project);
					} else {
						selectedProjects.remove(project);
						referenceCountProjects.remove(project);
					}
				}
				updateEnablement();
			}
		});

		final Composite buttonComposite = new Composite(composite, SWT.NONE);
		final GridLayout layout = new GridLayout();
		layout.numColumns = 2;
		layout.marginWidth = 0;
		buttonComposite.setLayout(layout);
		data = new GridData(SWT.FILL, SWT.FILL, true, false);
		buttonComposite.setLayoutData(data);

		final Button selectAll = new Button(buttonComposite, SWT.PUSH);
		data = new GridData();
		data.verticalAlignment = GridData.BEGINNING;
		data.horizontalAlignment = GridData.END;
		int widthHint = convertHorizontalDLUsToPixels(IDialogConstants.BUTTON_WIDTH);
		data.widthHint = Math.max(widthHint, selectAll.computeSize(SWT.DEFAULT,
				SWT.DEFAULT, true).x);
		selectAll.setLayoutData(data);
		selectAll.setText(Messages
				.getString("WizardProjectTranslateSelectPage.selectAll"));
		selectAll.addListener(SWT.Selection, new Listener() {
			public void handleEvent(Event event) {
				tableViewer.setAllChecked(true);
				selectedProjects.removeAll(selectedProjects);
				final Object[] checked = tableViewer.getCheckedElements();
				for (int i = 0; i < checked.length; i++) {
					selectedProjects.add(checked[i]);
				}
				updateEnablement();
			}
		});

		final Button deselectAll = new Button(buttonComposite, SWT.PUSH);
		data = new GridData();
		data.verticalAlignment = GridData.BEGINNING;
		data.horizontalAlignment = GridData.END;
		widthHint = convertHorizontalDLUsToPixels(IDialogConstants.BUTTON_WIDTH);
		data.widthHint = Math.max(widthHint, deselectAll.computeSize(
				SWT.DEFAULT, SWT.DEFAULT, true).x);
		deselectAll.setLayoutData(data);
		deselectAll.setText(Messages
				.getString("WizardProjectTranslateSelectPage.deselectAll"));
		deselectAll.addListener(SWT.Selection, new Listener() {
			public void handleEvent(Event event) {
				tableViewer.setAllChecked(false);
				selectedProjects.removeAll(selectedProjects);
				updateEnablement();
			}
		});
	}

	private void initializeProjects() {
		tableViewer.setInput(ResourcesPlugin.getWorkspace());

		// Check any necessary projects
		if (selectedProjects != null) {
			tableViewer.setCheckedElements(selectedProjects
					.toArray(new IProject[selectedProjects.size()]));
		}
	}

	private void updateEnablement() {
		boolean complete;

		complete = (selectedProjects.size() != 0);

		if (complete) {
			setMessage(null);
		}
		setPageComplete(complete);
	}

	public ArrayList getReferenceCountProjects() {
		return referenceCountProjects;
	}

	public HashSet getSelectedProjects() {
		return selectedProjects;
	}

}
