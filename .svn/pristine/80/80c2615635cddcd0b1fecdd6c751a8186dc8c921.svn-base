package com.ilog.translator.java2cs.plugin.wizard;

import java.lang.reflect.InvocationTargetException;
import java.util.ArrayList;
import java.util.Iterator;
import java.util.List;

import org.eclipse.core.resources.IWorkspaceRoot;
import org.eclipse.core.runtime.CoreException;
import org.eclipse.core.runtime.IConfigurationElement;
import org.eclipse.core.runtime.IExecutableExtension;
import org.eclipse.core.runtime.IProgressMonitor;
import org.eclipse.core.runtime.jobs.ISchedulingRule;
import org.eclipse.jface.operation.IRunnableWithProgress;
import org.eclipse.jface.viewers.ISelection;
import org.eclipse.jface.viewers.IStructuredSelection;
import org.eclipse.jface.viewers.StructuredSelection;
import org.eclipse.jface.wizard.IWizardPage;
import org.eclipse.jface.wizard.Wizard;
import org.eclipse.ui.INewWizard;
import org.eclipse.ui.IWorkbench;
import org.eclipse.ui.IWorkbenchPage;
import org.eclipse.ui.IWorkbenchPart;
import org.eclipse.ui.IWorkbenchPartReference;
import org.eclipse.ui.IWorkbenchWindow;
import org.eclipse.ui.actions.WorkspaceModifyOperation;
import org.eclipse.ui.part.ISetSelectionTarget;

public abstract class AbstractProjectWizard extends Wizard implements
		INewWizard, IExecutableExtension {
	protected IWorkbench workbench;
	protected IStructuredSelection selection;
	protected IConfigurationElement configurationElement;
	private final boolean listeningWorkspaceResources = false;
	private final boolean eventFiring = false;

	protected IWorkspaceRoot workspaceRoot;

	protected List<IWizardPage> wizardPages;

	public AbstractProjectWizard(IWorkspaceRoot workspaceRoot) {
		this.workspaceRoot = workspaceRoot;
		wizardPages = new ArrayList<IWizardPage>();
		setNeedsProgressMonitor(true);
		// setDialogSettings(IlrBrmUIPlugin.getPluginInstance()
		// .getDialogSettings());
	}

	public void init(IWorkbench workbench, IStructuredSelection selection) {
		this.workbench = workbench;
		this.selection = selection;
		// IlrResourceManager rm = IlrStudioModelPlugin.getResourceManager();
		// IlrRuleModel model = IlrStudioModelPlugin.getRuleModel();
		// this.listeningWorkspaceResources =
		// rm.isListeningWorkspaceResources();
		// this.eventFiring = model.isEventFiring();
		// model.disableEventFiring(true);
		// rm.disableWorkspaceResourceListener();
	}

	public void setInitializationData(
			IConfigurationElement configurationElement, String propertyName,
			Object data) {
		this.configurationElement = configurationElement;
	}

	public IConfigurationElement getConfigurationElement() {
		return configurationElement;
	}

	public IStructuredSelection getSelection() {
		return selection;
	}

	public IWorkbench getWorkbench() {
		return workbench;
	}

	private void registerWizardPageWithPluginContributions() {
		/*
		 * IlrWizardPageDescriptor[] descriptors =
		 * IlrWizardPageDescriptor.getWizardPageDescriptors(getWizardId()); if
		 * (descriptors == null || descriptors.length == 0) return; List
		 * descriptorList = new ArrayList(descriptors.length); List pageIds =
		 * new ArrayList(descriptors.length); for (int i = 0; i <
		 * descriptors.length; i++) { String id = descriptors[i].getId(); if
		 * (pageIds.contains(id)) {
		 * IlrBrmUIPlugin.getPluginInstance().getLogger().logErrorMessage("WARNING:
		 * Duplicate id for extension-point \"" + //No_i18n
		 * IlrBrmUIPlugin.getSymbolicName() + "." + //No_i18n
		 * IlrWizardPageDescriptor.EXTENSION_POINT_NAME + "\""); //No_i18n
		 * continue; } pageIds.add(id); descriptorList.add(descriptors[i]); }
		 * 
		 * Collections.sort(descriptorList, new Comparator() { public int
		 * compare(Object o1, Object o2) { int idx1 = ((IlrWizardPageDescriptor)
		 * o1).getIndex(); int idx2 = ((IlrWizardPageDescriptor) o2).getIndex();
		 * if (idx1 == -1 && idx2 == -1) return 0; if (idx1 == -1 && idx2 != -1)
		 * return 1; if (idx1 != -1 && idx2 == -1) return -1; return idx1 -
		 * idx2; } });
		 * 
		 * Iterator descriptorIterator = descriptorList.iterator(); while
		 * (descriptorIterator.hasNext()) { IlrWizardPageDescriptor descriptor =
		 * (IlrWizardPageDescriptor) descriptorIterator.next();
		 * IConfigurationElement element = descriptor.getElement(); IExtension
		 * extension = element.getDeclaringExtension(); Bundle bundle =
		 * Platform.getBundle(extension.getNamespace()); try { Class clazz =
		 * bundle.loadClass(descriptor.getWizardPageClass()); Object args[] = {
		 * descriptor.getId() }; WizardPage page = (WizardPage)
		 * IlrSharedUtil.buildInstance(clazz.getName(), args, true,
		 * clazz.getClassLoader()); page.setTitle(descriptor.getTitle());
		 * page.setDescription(descriptor.getDescription()); if (page instanceof
		 * IlrAbstractWizardPage) ((IlrAbstractWizardPage)
		 * page).setSelection(getSelection()); wizardPages.add(page); } catch
		 * (Exception e) { // log...
		 * IlrBrmUIPlugin.getPluginInstance().getExceptionHandler().handle(e); //
		 * ... and process the next descriptor } }
		 */
	}

	protected abstract String getWizardId();

	@Override
	public void addPages() {
		registerWizardPageWithPluginContributions();
		if (wizardPages == null || wizardPages.isEmpty())
			return;
		for (final IWizardPage page : wizardPages) {
			addPage(page);
		}
	}

	@Override
	public boolean performFinish() {
		final IRunnableWithProgress op = new WorkspaceModifyOperation(
				getSchedulingRule()) {
			@Override
			protected void execute(IProgressMonitor monitor)
					throws CoreException, InvocationTargetException,
					InterruptedException {
				try {
					finish(monitor);
				} catch (Exception e) {
					throw new InvocationTargetException(e);
				} finally {
					monitor.done();
				}
			}
		};
		try {
			getContainer().run(false, true, op);
		} catch (final InterruptedException e) {
			// IlrStudioModelPlugin.getPluginInstance().getExceptionHandler().handle(e);
		} catch (final InvocationTargetException e) {
			// IlrStudioModelPlugin.getPluginInstance().getExceptionHandler().handle(e);
		} finally {
			/*
			 * if (listeningWorkspaceResources)
			 * IlrStudioModelPlugin.getResourceManager().enableWorkspaceResourceListener();
			 * if (eventFiring)
			 * IlrStudioModelPlugin.getRuleModel().enableEventFiring(true);
			 */
		}
		return true;
	}

	/**
	 * The scheduling rule which will be used when this wizard will access the
	 * workspace.
	 * 
	 * @return
	 */
	protected abstract ISchedulingRule getSchedulingRule();

	protected void finish(IProgressMonitor monitor)
			throws InterruptedException, CoreException {
		final IWizardPage[] pages = getPages();
		monitor.beginTask(getWindowTitle(), pages == null ? 1
				: 100 * pages.length);
		try {
			if (pages != null) {
				for (int iWizardPage = 0; iWizardPage < pages.length; iWizardPage++) {
					final IWizardPage page = pages[iWizardPage];
					/*
					 * if (page instanceof AbstractProjectWizard &&
					 * ((AbstractProjectWizard) page).isActive()) {
					 * ((AbstractProjectWizard) page) .performFinish(new
					 * SubProgressMonitor(monitor, 100)); } else {
					 * monitor.worked(100); }
					 */
				}
			}
		} finally {
			monitor.done();
		}
	}

	/*
	 * (non-Javadoc)
	 * 
	 * @see org.eclipse.jface.wizard.IWizard#getNextPage(org.eclipse.jface.wizard.IWizardPage)
	 */
	@Override
	public IWizardPage getNextPage(IWizardPage page) {
		final IWizardPage nextPage = super.getNextPage(page);
		/*
		 * while (nextPage != null && nextPage instanceof AbstractProjectWizard &&
		 * !((AbstractProjectWizard) nextPage).isActive()) { nextPage =
		 * super.getNextPage(nextPage); }
		 */
		return nextPage;
	}

	@Override
	public boolean performCancel() {
		final IRunnableWithProgress op = new IRunnableWithProgress() {
			public void run(IProgressMonitor monitor)
					throws InvocationTargetException {
				try {
					cancel(monitor);
				} catch (Exception e) {
					throw new InvocationTargetException(e);
				} finally {
					monitor.done();
				}
			}
		};
		try {
			getContainer().run(false, false, op); // don't fork this runnable,
			// otherwise the resource events are not correctly disabled - LM
		} catch (final InterruptedException e) {
			// IlrStudioModelPlugin.getPluginInstance().getExceptionHandler().handle(e);
		} catch (final InvocationTargetException e) {
			// IlrStudioModelPlugin.getPluginInstance().getExceptionHandler().handle(e);
		} finally {
			if (eventFiring) {
				// As we are collecting events during the creation of the
				// element, we don't want
				// to notify on cancel to avoid creation event.
				// IlrStudioModelPlugin.getRuleModel().enableEventFiring(false);
			}
			// if (listeningWorkspaceResources)
			// IlrStudioModelPlugin.getResourceManager().enableWorkspaceResourceListener();
		}
		return true;
	}

	protected void cancel(IProgressMonitor monitor) throws CoreException {
		final IWizardPage[] pages = getPages();
		if (pages != null) {
			for (int iWizardPage = 0; iWizardPage < pages.length; iWizardPage++) {
				final IWizardPage page = pages[iWizardPage];
				/*
				 * if (page instanceof AbstractProjectWizard)
				 * ((AbstractProjectWizard) page).performCancel(monitor);
				 */
			}
		}
	}

	protected void selectAndReveal(Object obj) {
		selectAndReveal(obj, getWorkbench().getActiveWorkbenchWindow());
	}

	@SuppressWarnings("unchecked")
	public static void selectAndReveal(Object obj, IWorkbenchWindow window) {
		if (window == null || obj == null)
			return;
		final IWorkbenchPage page = window.getActivePage();
		if (page == null)
			return;

		// get all the view and editor parts
		final List parts = new ArrayList();
		IWorkbenchPartReference refs[] = page.getViewReferences();
		for (int i = 0; i < refs.length; i++) {
			final IWorkbenchPart part = refs[i].getPart(false);
			if (part != null)
				parts.add(part);
		}
		refs = page.getEditorReferences();
		for (int i = 0; i < refs.length; i++) {
			final IWorkbenchPart part = refs[i].getPart(false);
			if (part != null)
				parts.add(refs[i].getPart(false));
		}

		final ISelection selection = new StructuredSelection(obj);
		final Iterator enumVar = parts.iterator();
		while (enumVar.hasNext()) {
			final IWorkbenchPart part = (IWorkbenchPart) enumVar.next();

			ISetSelectionTarget targetSelection = null;
			if (part instanceof ISetSelectionTarget)
				targetSelection = (ISetSelectionTarget) part;
			else
				targetSelection = (ISetSelectionTarget) part
						.getAdapter(ISetSelectionTarget.class);

			if (targetSelection != null) {
				final ISetSelectionTarget finalTargetSelection = targetSelection;
				/*
				 * IlrSharedUtil.postUIJob(window.getShell(), new Runnable() {
				 * public void run() {
				 * finalTargetSelection.selectReveal(selection); } });
				 */
			}
		}
	}

	protected void expand(Object obj) {
		expand(obj, getWorkbench().getActiveWorkbenchWindow());
	}

	@SuppressWarnings("unchecked")
	public static void expand(Object obj, IWorkbenchWindow window) {
		if (window == null || obj == null)
			return;
		final IWorkbenchPage page = window.getActivePage();
		if (page == null)
			return;

		// get all the view and editor parts
		final List parts = new ArrayList();
		IWorkbenchPartReference refs[] = page.getViewReferences();
		for (int i = 0; i < refs.length; i++) {
			final IWorkbenchPart part = refs[i].getPart(false);
			if (part != null)
				parts.add(part);
		}
		refs = page.getEditorReferences();
		for (int i = 0; i < refs.length; i++) {
			final IWorkbenchPart part = refs[i].getPart(false);
			if (part != null)
				parts.add(refs[i].getPart(false));
		}

		final ISelection selection = new StructuredSelection(obj);
		final Iterator enumVar = parts.iterator();
		while (enumVar.hasNext()) {
			final IWorkbenchPart part = (IWorkbenchPart) enumVar.next();

			/*
			 * IlrIExpandTarget targetExpand = null; if (part instanceof
			 * IlrIExpandTarget) targetExpand = (IlrIExpandTarget) part; else
			 * targetExpand = (IlrIExpandTarget)
			 * part.getAdapter(IlrIExpandTarget.class);
			 * 
			 * if (targetExpand != null) { final IlrIExpandTarget
			 * finalTargetExpand = targetExpand;
			 * IlrSharedUtil.postUIJob(window.getShell(), new Runnable() {
			 * public void run() { finalTargetExpand.expand(selection, 1); } }); }
			 */
		}
	}
}
