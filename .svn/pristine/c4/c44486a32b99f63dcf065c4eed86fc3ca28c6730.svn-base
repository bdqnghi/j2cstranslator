package com.ilog.translator.java2cs.tools.ant;

import org.eclipse.core.runtime.IProgressMonitor;

/**
 * Helper for display status of translation in ant scrip execution.
 * 
 * @author vpotapenko
 */
public class AntTaskProgressMonitor implements IProgressMonitor {

    private final String BEGIN_TASK_PATTERN = "Begin task: %s";
    private final String TASK_PATTERN = "Task: %s";
    private final String SUB_TASK_PATTERN = "Sub task: %s";

    private final ITaskLogger logger;

    private boolean canceled;

    public AntTaskProgressMonitor(ITaskLogger logger) {
        this.logger = logger;
    }

    @Override
    public void beginTask(String name, int totalWork) {
        logger.log(String.format(BEGIN_TASK_PATTERN, name));
    }

    @Override
    public void done() {
    }

    @Override
    public void internalWorked(double work) {
    }

    @Override
    public boolean isCanceled() {
        return canceled;
    }

    @Override
    public void setCanceled(boolean value) {
        this.canceled = value;
    }

    @Override
    public void setTaskName(String name) {
        logger.log(String.format(TASK_PATTERN, name));
    }

    @Override
    public void subTask(String name) {
        logger.log(String.format(SUB_TASK_PATTERN, name));
    }

    @Override
    public void worked(int work) {
    }

}
