package com.ilog.translator.java2cs.plugin;

import java.io.File;
import java.io.FileInputStream;
import java.io.FileOutputStream;
import java.io.IOException;
import java.util.ArrayList;
import java.util.Date;
import java.util.List;
import java.util.Map;

import org.eclipse.core.resources.IProject;
import org.eclipse.core.resources.IProjectDescription;
import org.eclipse.core.resources.IResource;
import org.eclipse.core.resources.IWorkspace;
import org.eclipse.core.resources.IWorkspaceDescription;
import org.eclipse.core.resources.IncrementalProjectBuilder;
import org.eclipse.core.resources.ResourcesPlugin;
import org.eclipse.core.runtime.CoreException;
import org.eclipse.core.runtime.IPath;
import org.eclipse.core.runtime.IProgressMonitor;
import org.eclipse.core.runtime.OperationCanceledException;
import org.eclipse.core.runtime.Path;
import org.eclipse.core.runtime.Platform;
import org.eclipse.core.runtime.SubProgressMonitor;
import org.eclipse.core.runtime.jobs.IJobManager;
import org.eclipse.core.runtime.jobs.Job;
import org.eclipse.core.runtime.preferences.IEclipsePreferences;
import org.eclipse.core.runtime.preferences.InstanceScope;
import org.eclipse.jdt.core.IClasspathEntry;
import org.eclipse.jdt.core.IJavaProject;
import org.eclipse.jdt.core.JavaCore;

import org.osgi.service.prefs.Preferences;

import com.ilog.translator.java2cs.configuration.TranslationConfiguration;
import com.ilog.translator.java2cs.configuration.TranslatorProjectOptions;
import com.ilog.translator.java2cs.translation.ITranslator;
import com.ilog.translator.java2cs.translation.Translator;

public class ProjectBuilder {

	// @jojo
	public static void waitForFamily(Object family,
			TranslatorProjectOptions options) {
		boolean wasInterrupted = false;
		IJobManager jobManager = Job.getJobManager();
		do {
			try {
				jobManager.join(family, null);
				wasInterrupted = false;
			} catch (OperationCanceledException e) {
				options.getGlobalOptions().getLogger().logException("", e);
				e.printStackTrace();
			} catch (InterruptedException e) {
				wasInterrupted = true;
				options.getGlobalOptions().getLogger().logException("", e);
				e.printStackTrace();
			}
		} while (wasInterrupted);
	}

	// @jojo
	public static void waitForCurrentJob(TranslatorProjectOptions options) {
		boolean wasInterrupted = false;
		IJobManager jobManager = Job.getJobManager();
		do {
			try {
				if (jobManager.currentJob() != null
						&& jobManager.currentJob().getThread() != Thread
								.currentThread())
					jobManager.currentJob().join();
				wasInterrupted = false;
			} catch (OperationCanceledException e) {
				options.getGlobalOptions().getLogger().logException("", e);
				e.printStackTrace();
			} catch (InterruptedException e) {
				options.getGlobalOptions().getLogger().logException("", e);
				e.printStackTrace();
			}
		} while (wasInterrupted);
	}

	public static boolean copyAndTranslate(IJavaProject javaProject,
			TranslatorProjectOptions options, boolean needToReadOptions,
			IProgressMonitor monitor, boolean commandLine) throws IOException,
			CoreException {
		try {
			monitor.beginTask("Translation", 95);
			IWorkspace ws = ResourcesPlugin.getWorkspace();
			IProject sourceProject = javaProject.getProject();
			//
			monitor.subTask("Creating temp project");
			String pName = "translation_" + javaProject.getProject().getName()
					+ "_"
					+ new Date().toString().replace(" ", "_").replace(":", "_");

			// refesh + compile
			if (options.getRefreshAndBuild()) {
				options.getGlobalOptions().getLogger().logInfo(
						"Refresh and build");
				sourceProject.refreshLocal(IResource.DEPTH_INFINITE,
						new SubProgressMonitor(monitor, 2));
				sourceProject.build(
						IncrementalProjectBuilder.FULL_BUILD, new SubProgressMonitor(monitor, 3));
			} else {
				monitor.worked(5);
			}

			// may cause exception
			if (!commandLine) {
				// @jojo
				waitForCurrentJob(options);
				waitForFamily(ResourcesPlugin.FAMILY_AUTO_BUILD, options);
				waitForFamily(ResourcesPlugin.FAMILY_MANUAL_BUILD, options);
				waitForFamily(ResourcesPlugin.FAMILY_AUTO_REFRESH, options);
			}
			// disable auto-build, as it is not needed here:
			IWorkspaceDescription desc = ws.getDescription();
			String qualifier = ResourcesPlugin.getPlugin().getBundle()
					.getSymbolicName();
			IEclipsePreferences root = Platform.getPreferencesService()
					.getRootNode();

			boolean wasAutoBuilding = desc.isAutoBuilding();
			String key = ResourcesPlugin.PREF_AUTO_BUILDING;
			Preferences node = root.node(InstanceScope.SCOPE).node(qualifier);
			boolean wasAutoBuildingPref = node.getBoolean(key, true);
			key = ResourcesPlugin.PREF_AUTO_REFRESH;
			node = root.node(InstanceScope.SCOPE).node(qualifier);
			boolean wasAutoRefreshPref = node.getBoolean(key, true);

			try {
				// disable auto building
				desc.setAutoBuilding(false);
				ws.setDescription(desc);
				key = ResourcesPlugin.PREF_AUTO_BUILDING;
				root.node(InstanceScope.SCOPE).node(qualifier).putBoolean(key,
						false);
				// disable auto refresh
				key = ResourcesPlugin.PREF_AUTO_REFRESH;
				root.node(InstanceScope.SCOPE).node(qualifier).putBoolean(key,
						false);
				// copy & build
				IProject copyProject = ws.getRoot().getProject(pName);
				sourceProject.copy(new Path(pName), false,
						new SubProgressMonitor(monitor, 5)); // TODO
				copyProject.build(IncrementalProjectBuilder.FULL_BUILD, new SubProgressMonitor(monitor, 5));
				//
				IPath projectPath = sourceProject.getLocation(); /* javaProject.getCorrespondingResource()
						.getLocation();*/
				File projectFile = projectPath.toFile();
				//
				setWritable(projectFile.toString() + File.separator + ".."
						+ File.separator + pName);				
				options.getGlobalOptions().getLogger().logInfo(
						"Finish copying project");
				return performTranslation(javaProject, copyProject, options, new SubProgressMonitor(monitor, 80),
						needToReadOptions);
			} catch (Exception e) {
				options.getGlobalOptions().getLogger().logException("", e);
				return false;
			} finally {
				// restore auto building
				desc.setAutoBuilding(wasAutoBuilding);
				ws.setDescription(desc);
				key = ResourcesPlugin.PREF_AUTO_BUILDING;
				root.node(InstanceScope.SCOPE).node(qualifier).putBoolean(key,
						wasAutoBuildingPref);
				// disable auto refresh
				key = ResourcesPlugin.PREF_AUTO_REFRESH;
				root.node(InstanceScope.SCOPE).node(qualifier).putBoolean(key,
						wasAutoRefreshPref);
			}
		} catch (Exception e) {
			options.getGlobalOptions().getLogger().logException("", e);
			e.printStackTrace();
			return false;
		}

	}

	private static void setWritable(String path) {
		File dir = new File(path);
		List<File> allSources = listFiles(dir);
		for (File f : allSources) {
			if (!f.canWrite())
				f.setWritable(true);
		}
	}

	private static List<File> listFiles(File dir) {
		List<File> result = new ArrayList<File>();
		File[] files = dir.listFiles();
		if (files != null) {
			for (File f : files) {
				if (f.isDirectory())
					result.addAll(listFiles(f));
				else if (f.getName().endsWith(".java")) {
					result.add(f);
				}
			}
		}
		return result;
	}

	private static boolean performTranslation(final IJavaProject oldProject,
			final IProject project, final TranslatorProjectOptions options,
			final IProgressMonitor pm, final boolean needToReadOptions)
			throws Exception {
		if (project != null && project.isOpen()
				&& project.hasNature(JavaCore.NATURE_ID)) {
			final IJavaProject javaProject = JavaCore.create(project);
			TranslationConfiguration configuration = new TranslationConfiguration(
					oldProject, javaProject, options);
			configuration.init(options.getGlobalOptions()
					.getConfigurationFileName(), needToReadOptions);
			ITranslator translator = new Translator(javaProject, configuration);
			boolean res = translator.translate(pm);
			if (options.getRemoveTempProject())
				project.delete(IResource.ALWAYS_DELETE_PROJECT_CONTENT
						| IResource.FORCE, pm);
			javaProject.close();
			options.getGlobalOptions().getLogger().close();
			translator = null;
			configuration = null;
			return res;
		}
		return false;
	}

	private static IProject createCopyProject(IProject project, String pName,
			IWorkspace ws, IProgressMonitor pm) throws Exception {
		pm.beginTask("Creating temp project", 1);

		IPath destination = new Path(pName);

		IJavaProject oldJavaproj = JavaCore.create(project);
		IClasspathEntry[] classPath = oldJavaproj.getRawClasspath();

		IProject newProject = ResourcesPlugin.getWorkspace().getRoot()
				.getProject(pName);
		newProject.create(null);
		newProject.open(null);

		IProjectDescription desc = newProject.getDescription();
		desc.setNatureIds(new String[] { JavaCore.NATURE_ID });
		newProject.setDescription(desc, null);

		//
		List<IClasspathEntry> newClassPath = new ArrayList<IClasspathEntry>();
		for (IClasspathEntry cEntry : classPath) {
			switch (cEntry.getEntryKind()) {
			case IClasspathEntry.CPE_SOURCE:
				System.out.println("Source folder " + cEntry.getPath());
				newClassPath.add(copySourceFolder(project, newProject, cEntry,
						destination));
				break;
			case IClasspathEntry.CPE_LIBRARY:
				System.out.println("library folder " + cEntry.getPath());
				newClassPath.add(cEntry);
				break;
			case IClasspathEntry.CPE_PROJECT:
				System.out.println("project folder " + cEntry.getPath());
				newClassPath.add(cEntry);
				break;
			case IClasspathEntry.CPE_VARIABLE:
				System.out.println("variable folder " + cEntry.getPath());
				newClassPath.add(cEntry);
				break;
			default:
				System.out.println("container folder " + cEntry.getPath());
				newClassPath.add(cEntry);
			}
		}
		//
		copyDir(project.getLocation().toString(), "/translator", newProject
				.getLocation().toString(), "", new ArrayList<String>() {
			{
				add("generated");
				add("classes");
				add(".svn");
			}
		});
		//
		newProject.refreshLocal(IResource.DEPTH_INFINITE, pm);
		newProject.build(IncrementalProjectBuilder.AUTO_BUILD, pm);
		newProject.touch(pm);
		//
		IJavaProject javaproj = JavaCore.create(newProject);
		javaproj.setOutputLocation(new Path("/" + newProject.getName()
				+ "/classes/bin"), null);
		javaproj.setRawClasspath(newClassPath
				.toArray(new IClasspathEntry[newClassPath.size()]), pm);
		//

		Map opts = oldJavaproj.getOptions(true);
		javaproj.setOptions(opts);
		javaproj.makeConsistent(pm);
		javaproj.save(pm, true);
		//

		return newProject;
	}

	private static IClasspathEntry copySourceFolder(IProject oldProject,
			IProject newProject, IClasspathEntry entry, IPath destination) {
		IPath oldSourcePath = entry.getPath();
		IPath srcDir = oldSourcePath.removeFirstSegments(1);
		IPath p = new Path("/" + newProject.getName() + "/"
				+ entry.getPath().removeFirstSegments(1).toString());
		IPath outputLocation = new Path(entry.getOutputLocation().toString()
				.replace(oldProject.getName(), newProject.getName()));
		IClasspathEntry newSrcEntry = JavaCore.newSourceEntry(p, entry
				.getInclusionPatterns(), entry.getExclusionPatterns(),
				outputLocation);
		copyDir(oldProject.getLocation().toString(), srcDir.toString(),
				newProject.getLocation().toString(), ".java",
				new ArrayList<String>() {
					{
						add(".svn");
						add("classes");
					}
				});
		return newSrcEntry;
	}

	private static void copyDir(String projectLocation, String srcDirF,
			String newProjectLocation, String fileFilter,
			List<String> folderExcludeFilter) {
		File srcDir = new File(projectLocation + File.separator + srcDirF);
		File[] files = srcDir.listFiles();
		if (files != null) {
			for (File file : files) {
				if (file.isDirectory()) {
					String p = file.getName();
					if (!folderExcludeFilter.contains(p)) {
						File f = new File(newProjectLocation + "/" + srcDirF
								+ File.separator + p);
						if (!f.exists())
							f.mkdirs();
						copyDir(projectLocation, srcDirF + File.separator + p,
								newProjectLocation, fileFilter,
								folderExcludeFilter);
					}
				} else {
					if (file.getAbsolutePath().endsWith(fileFilter)) {
						/*
						 * System.out.println("I need to copy " +
						 * file.getAbsolutePath() + " in " + newProjectLocation +
						 * srcDirF + File.separator + file.getName());
						 */
						copyFile(file.getAbsolutePath(), newProjectLocation
								+ "/" + srcDirF + File.separator, file
								.getName());
					}
				}
			}
		}
	}

	private static void copyFile(String absolutePath, String dir, String name) {
		try {
			File f = new File(dir);
			if (!f.exists())
				f.mkdirs();
			FileInputStream fs = new FileInputStream(absolutePath);
			FileOutputStream fo = new FileOutputStream(dir + name);
			int value = 0;
			while ((value = fs.read()) != -1) {
				fo.write(value);
			}
		} catch (Exception e) {
			e.printStackTrace();
		}
	}
}
