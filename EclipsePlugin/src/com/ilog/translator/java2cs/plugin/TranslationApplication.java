package com.ilog.translator.java2cs.plugin;

import java.io.File;
import java.io.IOException;
import java.io.InputStream;
import java.util.ArrayList;
import java.util.Date;
import java.util.List;
import java.util.Map;

import junit.framework.Assert;

import org.eclipse.core.resources.IProject;
import org.eclipse.core.resources.IProjectDescription;
import org.eclipse.core.resources.IWorkspace;
import org.eclipse.core.resources.IWorkspaceDescription;
import org.eclipse.core.resources.ResourcesPlugin;
import org.eclipse.core.runtime.CoreException;
import org.eclipse.core.runtime.IPath;
import org.eclipse.core.runtime.IProgressMonitor;
import org.eclipse.core.runtime.NullProgressMonitor;
import org.eclipse.core.runtime.Path;
import org.eclipse.equinox.app.IApplication;
import org.eclipse.equinox.app.IApplicationContext;
import org.eclipse.jdt.core.IClasspathEntry;
import org.eclipse.jdt.core.IJavaProject;
import org.eclipse.jdt.core.JavaCore;
import org.eclipse.swt.widgets.Display;
import org.eclipse.ui.PlatformUI;
import org.eclipse.ui.application.WorkbenchAdvisor;

import com.ilog.translator.java2cs.configuration.GlobalOptions;
import com.ilog.translator.java2cs.configuration.TranslatorDependency;
import com.ilog.translator.java2cs.configuration.TranslatorProjectOptions;
import com.ilog.translator.java2cs.plugin.util.IDEWorkbenchAdvisor;
import com.ilog.translator.java2cs.plugin.util.ProjectBuilder;
import com.ilog.translator.java2cs.plugin.util.ProjectImporter;
import com.ilog.translator.java2cs.translation.TranslatorProgressMonitor;
import com.ilog.translator.java2cs.util.Utils;

/**
 * Pseudo-Headless version of the translator
 * 
 * @author afau
 * 
 */
public class TranslationApplication implements IApplication {

	private final GlobalOptions globalOptions = new GlobalOptions();
	private boolean multiProjectMode = false;

	//
	//
	//

	public Object run(Object args) throws Exception {
		globalOptions.setHeadless(true);
		globalOptions.getLogger().logInfo("Java2Cs Translator v" + TranslationPlugin.getVersion());
		final String[] str = (String[]) args;
		if (globalOptions.isDebug()) {
			globalOptions.getLogger().logInfo(
					"Translator version : " + TranslationPlugin.getVersion());
			globalOptions.getLogger().logInfo("");
			globalOptions.getLogger().logInfo("Command Line is : ");
			for (final String s : str) {
				globalOptions.getLogger().logInfo("   -" + s + " ");
			}
		}

		final List<TranslatorDependency> referencedProjects = parseCommandLine(filterCommandLine(str));

		if (globalOptions.isDebug()) {
			globalOptions.getLogger().logInfo("");
			globalOptions.getLogger().logInfo("Tmpdir : " + globalOptions.getTempDir());
			globalOptions.getLogger().logInfo("Basedir : " + globalOptions.getBaseDestDir());
			globalOptions.getLogger().logInfo("Debug :" + globalOptions.isDebug());
			// globalOptions.getLogger().logInfo("ProjectName :" + globalOptions.getName());
			globalOptions.getLogger().logInfo(
					"Tmpdir : " + globalOptions.getTempDir());
			globalOptions.getLogger().logInfo(
					"Basedir : " + globalOptions.getBaseDestDir());
			globalOptions.getLogger().logInfo(
					"Debug :" + globalOptions.isDebug());
			// globalOptions.getLogger().logInfo("ProjectName :" +
			// globalOptions.getName());
			for (final TranslatorDependency s : referencedProjects) {
				System.out.println("project " + s.getName());
			}
		}
		//	
		//

		final Display disp = PlatformUI.createDisplay();
		final WorkbenchAdvisor adv = new IDEWorkbenchAdvisor();
		disp.timerExec(5000, new Runnable() {

			@SuppressWarnings("deprecation")
			public void run() {
				//
				final IWorkspace workspace = TranslationPlugin.getWorkspace();
				final IWorkspaceDescription wsDesc = workspace.getDescription();
				wsDesc.setAutoBuilding(false);
				boolean closed = false;
				try {
					// Try to close (hide) the workbench
					// globalOptions.getLogger().logInfo("Java2Cs Translator -
					// Trying to close workbench");
					closed = PlatformUI.getWorkbench().close();
					// globalOptions.getLogger().logInfo("Java2Cs Translator -
					// workbench closed");
				} catch (final Exception e) {
					globalOptions.getLogger().logInfo(
							"Java2Cs Translator - Closing (" + closed
									+ ") workbench exception: " + e);
				}
				if (!closed) {
					try {
						// globalOptions.getLogger().logInfo("Java2Cs Translator
						// - Trying to close workbench again");
						closed = PlatformUI.getWorkbench().close();
						// globalOptions.getLogger().logInfo("Java2Cs Translator
						// - workbench closed");
					} catch (final Exception e) {
						globalOptions.getLogger().logInfo(
								"Java2Cs Translator - Closing (" + closed
										+ ") workbench exception: " + e);
					}
				}
				//
				try {
					if (multiProjectMode)
						TranslationApplication.this
								.processManyProjects(referencedProjects);
					else {
						JavaCore.setClasspathVariable("M2_REPO", new Path(
								System.getenv().get("M2_REPO")));
						final String pName = referencedProjects.get(0)
								.getName();
						final IProject project = workspace.getRoot()
								.getProject(pName);
						final IJavaProject javaProject = JavaCore
								.create(project);
						// only read options not the dependencies
						final String confFile = TranslatorProjectOptions
								.searchTranslatorDir(javaProject,
										globalOptions, true, false,
										referencedProjects.get(0).getProfile());
						final TranslatorProjectOptions projectOptions = new TranslatorProjectOptions(
								confFile, globalOptions);
						if (referencedProjects.get(0).getProfile() != null
								&& !referencedProjects.get(0).getProfile()
										.equals("")) {
							projectOptions
									.setProfileForTranslation(referencedProjects
											.get(0).getProfile());
						}
						projectOptions.read(javaProject, false);
						// projectOptions.readDependance(javaProject);
						TranslationApplication.this.process(pName, javaProject,
								projectOptions);
					}
					globalOptions.getLogger().close();
				} catch (Exception e) {
					globalOptions.getLogger().logException("",e);
					e.printStackTrace();			
				} 
				System.exit(0);
			}

		});
		final int code = PlatformUI.createAndRunWorkbench(disp, adv);
		//
		return code;
	}

	private List<String> filterCommandLine(String[] str) {
		final List<String> result = new ArrayList<String>();
		for (final String arg : str) {
			if (!arg.startsWith("-")) {
				result.add(arg);
			}
		}
		return result;
	}

	/**
	 * 
	 * @param str
	 */
	private List<TranslatorDependency> parseCommandLine(List<String> strs)
			throws Exception {
		if (strs.size() != 0) {
			if (strs.size() > 1) {
				// In that case all options coming from command line
				final ProjectImporter pImporter = new ProjectImporter();
				return globalOptions.parseCommandLine(strs, pImporter);
			} else if (strs.size() == 1) {
				// Project file version
				// options will be read in each project
				final String configurationfile = Utils.getValueFromCommandLine(
						strs, "projectFile");
				return readProjectConfigurationFile(configurationfile);
			}
		}
		printUsage();
		return null;
	}

	private void printUsage() {
		System.out.println("ILOG Java 2 CSharp translator");
		System.out.println(" usage : ");
		System.out
				.println("eclipse.exe -data myWorkspace -application com.ilog.rules.Java2CSharpTranslator.Java2Cs /projectFile");
		System.out.println("   String /projectFile         : xml file ");
		System.out.println("   or ");
		System.out
				.println("eclipse.exe -data myWorkspace -application com.ilog.rules.Java2CSharpTranslator.Java2Cs [/debug] [/tmpDir] [/outputdir] /name [/useGenerics] [/dependencies] [/generateVsProject]");
		System.out.println("   boolean /debug                 : [false] ");
		System.out.println("   String  /tmpDir                : [c:/temp/] ");
		System.out
				.println("   String  /outputdir             : [c:/temp/] directory for genereated sources");
		System.out
				.println("   String  /name                  : name of the project to translate in the given workspace");
		System.out.println("   boolean /useGenerics           : [false] ");
		System.out.println("   boolean /covariant             : [false] ");
		System.out.println("   boolean /autoProperties        : [false] ");
		System.out.println("   boolean /generateconffile      : [false] ");
		System.out.println("   boolean /createTranslatorFiles : [false] ");
		System.out
				.println("   List    /dependencies          : List of '$' seperated of referenced project need to build the workspace (jar, or directory)");
		System.out
				.println("   boolean /generateVsProject     : [false] generate or not a vs project file");
	}

	public void switchWorkspace(String path) {
		if (globalOptions.isDebug()) {
			globalOptions.getLogger().logInfo("Switching to workspace " + path);
		}
		final String command_line = buildCommandLine(path);
		if (command_line == null) {
			globalOptions.getLogger().logInfo("Error command line is null");
			return;
		}
		if (globalOptions.isDebug()) {
			globalOptions.getLogger().logInfo("Command line is : " + command_line);
		}

		System.setProperty(TranslationApplication.PROP_EXIT_CODE, Integer
				.toString(24));
		System.setProperty(TranslationApplication.PROP_EXIT_DATA, command_line);
		PlatformUI.getWorkbench().restart();
	}

	//
	//
	//

	private List<TranslatorDependency> readProjectConfigurationFile(String fName)
			throws Exception {
		//
		if (globalOptions.isDebug()) {
			globalOptions.getLogger().logInfo("MultiProject mode");
		}
		multiProjectMode = true;

		return globalOptions.read(fName);
	}

	//
	//
	//

	private void processManyProjects(
			List<TranslatorDependency> referencedProjects) throws IOException,
			CoreException, Exception {
		final IWorkspace workspace = TranslationPlugin.getWorkspace();
		//
		final IWorkspaceDescription wdesc = workspace.getDescription();
		wdesc.setAutoBuilding(false);
		wdesc.setSnapshotInterval(3000000);
		workspace.setDescription(wdesc);
		//
		final long start = System.nanoTime();
		for (final TranslatorDependency p : referencedProjects) {
			// TranslatorOptions.setLogger(globalOptions.getLogger());
			IProject project = workspace.getRoot()
					.getProject(p.getName());
			IJavaProject javaProject = JavaCore.create(project);
			String translatorDir = TranslatorProjectOptions
					.searchTranslatorDir(javaProject, globalOptions, true,
							false, p.getProfile());
			String confFileName = TranslatorProjectOptions
					.getOrCreateTranslatorConfigurationFile(javaProject,
							globalOptions, true, translatorDir, false);
			TranslatorProjectOptions currentOptions = new TranslatorProjectOptions(
					confFileName, globalOptions);
			// currentOptions.setBaseDestDir(baseDestDir);
			// currentOptions.setProjectConfigurationFileName();
			if (p.getProfile() != null && !p.getProfile().equals("")) {
				currentOptions.setProfileForTranslation(p.getProfile());
			}
			currentOptions.read(javaProject, false);
			// currentOptions.readDependance(javaProject);
			process(p.getName(), javaProject, currentOptions);
			//
			currentOptions = null;
			confFileName = null;
			translatorDir = null;
			javaProject.close();
			javaProject = null;
			System.gc();
			System.gc();
			System.gc();			
		}
		final long end = System.nanoTime();
		globalOptions.getLogger().logInfo(
				"Done in " + (end - start) / 1000000000 + "s.");
		//
		// Compile
		if (globalOptions.isCompileAfterTranslation()) {
			compile(globalOptions.getBaseDestDir());
			if (globalOptions.isStartUnitTestAfterCompilation()) {
				unitTest(globalOptions.getBaseDestDir());
			}
		}
		// Start UnitTest
	}

	//
	//
	//

	private boolean unitTest(String dir) throws Exception {
		final String[] cmdline = { "nunit-console.exe" };

		final Process p = Runtime.getRuntime().exec(cmdline, null,
				new File(dir + File.separator));
		//
		final StringBuilder result = new StringBuilder();
		final int exitValue = doWaitFor(p, result);
		if (exitValue != 0) {
			Assert.fail("Error : " + result.toString());
			return false;
		}
		return true;
	}

	private boolean compile(String dir) throws Exception {
		final String[] cmdline = { "msbuild" };

		final Process p = Runtime.getRuntime().exec(cmdline, null,
				new File(dir + File.separator));
		//
		final StringBuilder result = new StringBuilder();
		final int exitValue = doWaitFor(p, result);
		if (exitValue != 0) {
			Assert.fail("Error : " + result.toString());
			return false;
		}
		return true;
	}

	public int doWaitFor(Process p, StringBuilder res) {
		int exitValue = -1; // returned to caller when p is finished
		try {
			final InputStream in = p.getInputStream();
			final InputStream err = p.getErrorStream();
			boolean finished = false; // Set to true when p is finished
			while (!finished) {
				try {
					while (in.available() > 0) {
						// Print the output of our system call
						final Character c = new Character((char) in.read());
						res.append(c);
					}
					while (err.available() > 0) {
						// Print the output of our system call
						final Character c = new Character((char) err.read());
						res.append(c);
					}
					// Ask the process for its exitValue. If the process
					// is not finished, an IllegalThreadStateException
					// is thrown. If it is finished, we fall through and
					// the variable finished is set to true.
					exitValue = p.exitValue();
					finished = true;
				} catch (final IllegalThreadStateException e) {

					// Process is not finished yet;
					// Sleep a little to save on CPU cycles
					Thread.sleep(500);
				}
			}
		} catch (final Exception e) {
			// unexpected exception! print it out for debugging...
			System.err.println("doWaitFor(): unexpected exception - "
					+ e.getMessage());
		}
		// return completion status to caller
		return exitValue;
	}

	//
	// One project
	//

	private void process(String pName, IJavaProject javaProject,
			TranslatorProjectOptions currentOptions) throws IOException,
			CoreException, Exception {
		currentOptions.getGlobalOptions().getLogger().logInfo(
				"--- Translating project '" + pName + "' to directory '"
						+ currentOptions.getSourcesDestDir() + "'");
		IProgressMonitor pm = null;
		if (currentOptions.getGlobalOptions().isDebug())
			pm = new TranslatorProgressMonitor(currentOptions
					.getGlobalOptions().getLogger(), 1);
		else
			pm = new NullProgressMonitor();
		final boolean res = ProjectBuilder.copyAndTranslate(javaProject,
				currentOptions, false, pm, true);
	}

	/**
	 * Copy the passed project
	 * 
	 * @param project
	 * @param ws
	 * @param pm
	 * @return
	 * @throws CoreException
	 */
	private IProject createCopyProject(IProject project, IWorkspace ws,
			IProgressMonitor pm) throws CoreException {
		pm.beginTask("Creating temp project", 1);
		final String pName = "translation_" + project.getName() + "_"
				+ new Date().toString().replace(" ", "_").replace(":", "_");
		//
		final IProgressMonitor npm = new NullProgressMonitor();
		//
		final IPath destination = new Path(pName);

		project.copy(destination, false, npm);

		//
		final IJavaProject oldJavaproj = JavaCore.create(project);
		final IClasspathEntry[] classPath = oldJavaproj.getRawClasspath();

		final IProject newProject = ResourcesPlugin.getWorkspace().getRoot()
				.getProject("NewProjectName");

		final IProjectDescription desc = project.getDescription();
		desc.setNatureIds(new String[] { JavaCore.NATURE_ID });
		project.setDescription(desc, null);

		final IJavaProject javaproj = JavaCore.create(newProject);
		javaproj.setOutputLocation(project.getFullPath(), null);
		// IClasspathEntry srcEntry =
		// JavaCore.newSourceEntry(project.getFullPath());
		final List<IClasspathEntry> newClassPath = new ArrayList<IClasspathEntry>();
		for (final IClasspathEntry cEntry : newClassPath) {
			switch (cEntry.getContentKind()) {
			case IClasspathEntry.CPE_SOURCE:
				System.out.println("Source folder " + cEntry.getPath());
				break;
			default:
				newClassPath.add(cEntry);
			}
		}

		javaproj.setRawClasspath(classPath, pm);

		//

		final IProject newP = ws.getRoot().getProject(pName);
		return newP;
	}

	private static final String PROP_VM = "eclipse.vm"; //$NON-NLS-1$

	private static final String PROP_VMARGS = "eclipse.vmargs"; //$NON-NLS-1$

	private static final String PROP_EXIT_CODE = "eclipse.exitcode"; //$NON-NLS-1$

	private static final String PROP_EXIT_DATA = "eclipse.exitdata"; //$NON-NLS-1$

	private static final String CMD_DATA = "-data"; //$NON-NLS-1$

	private static final String CMD_VMARGS = "-vmargs"; //$NON-NLS-1$

	private static final String NEW_LINE = "\n"; //$NON-NLS-1$

	private String buildCommandLine(String workspace) {
		String property = System.getProperty(TranslationApplication.PROP_VM);
		if (property == null) {
			System.err.println("Unable to found "
					+ TranslationApplication.PROP_VM + " property value");
			return null;
		}

		final StringBuffer result = new StringBuffer(512);
		result.append(property);
		result.append(TranslationApplication.NEW_LINE);

		// append the vmargs and commands. Assume that these already end in \n
		final String vmargs = System
				.getProperty(TranslationApplication.PROP_VMARGS);
		if (vmargs != null) {
			result.append(vmargs);
		}

		// append the rest of the args, replacing or adding -data as required
		// property = System.getProperty(PROP_COMMANDS);
		property = "-os win32 -wswin32 -arch x86 -launcher C:/work/eclipse/eclipse.exe -name Eclipse -showsplash 600";
		if (property == null) {
			result.append(TranslationApplication.CMD_DATA);
			result.append(TranslationApplication.NEW_LINE);
			result.append(workspace);
			result.append(TranslationApplication.NEW_LINE);
		} else {
			// find the index of the arg to replace its value
			int cmd_data_pos = property
					.lastIndexOf(TranslationApplication.CMD_DATA);
			if (cmd_data_pos != -1) {
				cmd_data_pos += TranslationApplication.CMD_DATA.length() + 1;
				result.append(property.substring(0, cmd_data_pos));
				result.append(workspace);
				result.append(property.substring(property.indexOf('\n',
						cmd_data_pos)));
			} else {
				result.append(TranslationApplication.CMD_DATA);
				result.append(TranslationApplication.NEW_LINE);
				result.append(workspace);
				result.append(TranslationApplication.NEW_LINE);
				result.append(property);
			}
		}

		// put the vmargs back at the very end (the eclipse.commands property
		// already contains the -vm arg)
		if (vmargs != null) {
			result.append(TranslationApplication.CMD_VMARGS);
			result.append(TranslationApplication.NEW_LINE);
			result.append(vmargs);
		}

		return result.toString();
	}

	//
	//
	//

	@SuppressWarnings("unchecked")
	public Object start(IApplicationContext context) throws Exception {
		final Map args = context.getArguments();
		try {
			final Integer code = (Integer) run(args
					.get(IApplicationContext.APPLICATION_ARGS));

			return code == PlatformUI.RETURN_RESTART ? EXIT_RESTART : EXIT_OK;
		} catch (final Exception e) {
			globalOptions.getLogger().logException(
					"Unexpected error, check log files.", e);
			throw e;
		}
	}

	public void stop() {
		// TODO Auto-generated method stub
	}
}
