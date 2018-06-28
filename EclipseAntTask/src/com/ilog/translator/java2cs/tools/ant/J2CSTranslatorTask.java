package com.ilog.translator.java2cs.tools.ant;

import java.util.ArrayList;
import java.util.List;

import org.apache.tools.ant.BuildException;
import org.apache.tools.ant.Task;
import org.eclipse.core.resources.IProject;
import org.eclipse.core.resources.ResourcesPlugin;
import org.eclipse.core.runtime.CoreException;
import org.eclipse.core.runtime.IProgressMonitor;
import org.eclipse.jdt.core.IJavaProject;
import org.eclipse.jdt.core.JavaCore;

import com.ilog.translator.java2cs.configuration.GlobalOptions;
import com.ilog.translator.java2cs.configuration.TranslatorProjectOptions;
import com.ilog.translator.java2cs.plugin.ProjectBuilder;
import com.ilog.translator.java2cs.tools.ant.patterns.Pattern;
import com.ilog.translator.java2cs.tools.ant.patterns.PatternExecutor;

/**
 * Ant task for translation project
 * 
 * @author vpotapenko
 */
public class J2CSTranslatorTask extends Task implements ITaskLogger {

    private String project;

    private final List<Pattern> patterns = new ArrayList<Pattern>();

    @Override
    public void execute() throws BuildException {
        checkParameters();

        IJavaProject javaProject = findProject();
        if (javaProject == null)
            throw new BuildException("Cannot find java project with name " + project);

        try {
            translateProject(javaProject);
            executePatterns(javaProject);
        } catch (Exception e) {
            e.printStackTrace();
            throw new BuildException(e);
        }
    }

    /**
     * Sets project name for translation. It calls from ant script.
     * 
     * @param project a name of project for translation
     */
    public void setProject(String project) {
        this.project = project;
    }

    /**
     * Add patterns from ant task
     * 
     * @param pattern
     */
    public void addPattern(Pattern pattern) {
        patterns.add(pattern);
    }

    private void translateProject(IJavaProject javaProject) throws Exception {
        TranslatorProjectOptions options = readOptions(javaProject);

        IProgressMonitor monitor = new AntTaskProgressMonitor(this);
        ProjectBuilder.copyAndTranslate(javaProject, options, true, monitor, true);
    }

    private TranslatorProjectOptions readOptions(IJavaProject javaProject) throws Exception {
        GlobalOptions globalOptions = new GlobalOptions();
        String translatorDirectory = TranslatorProjectOptions.searchTranslatorDir(javaProject, globalOptions,
                true, false, null /* TODO */);
        String confFileName = TranslatorProjectOptions.getOrCreateTranslatorConfigurationFile(javaProject,
                globalOptions, true, translatorDirectory, false);
        TranslatorProjectOptions options = new TranslatorProjectOptions(confFileName, globalOptions);
        options.read(javaProject, true);

        return options;
    }

    private IJavaProject findProject() {
        IProject workspaceProject = ResourcesPlugin.getWorkspace().getRoot().getProject(project);
        if (!workspaceProject.exists()) return null;

        try {
            return (IJavaProject) workspaceProject.getNature(JavaCore.NATURE_ID);
        } catch (CoreException e) {
            throw new BuildException(e);
        }
    }

    private void checkParameters() {
        if (project == null)
            throw new BuildException("Project name must be set");
    }

    private void executePatterns(IJavaProject javaProject) throws Exception {
        if (patterns.isEmpty()) return;

        TranslatorProjectOptions options = readOptions(javaProject);

        PatternExecutor executor = new PatternExecutor(options.getSourcesDestDir());
        executor.execute(patterns);
    }
}
