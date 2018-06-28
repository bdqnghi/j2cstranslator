package com.ilog.translator.java2cs.tools.ant.patterns;

import java.io.File;
import java.util.List;

import org.apache.tools.ant.BuildException;

/**
 * Executor for post translation replacement
 * 
 * @author vpotapenko
 */
public class PatternExecutor {

    private final FileCursor cursor = new FileCursor();

    private final String sourceDestDir;

    public PatternExecutor(String sourcesDestDir) {
        this.sourceDestDir = sourcesDestDir;
    }

    public void execute(List<Pattern> patterns) {
        checkParameters(patterns);
        executePatterns(patterns);
    }

    private void executePatterns(List<Pattern> patterns) {
        for (Pattern pattern : patterns) {
            executePattern(pattern);
        }
    }

    private void executePattern(Pattern pattern) {
        File file = new File(sourceDestDir + File.separator + pattern.getFileName());
        if (!file.exists() || !file.isFile())
            throw new BuildException("File " + file.getPath() + " does not exists");

        List<String> lookFor = pattern.getLookfor().getLines();
        List<String> replacement = pattern.getReplacement().getLines();
        cursor.readFile(file);
        while (cursor.find(lookFor)) {
            cursor.replace(replacement);
        }
        cursor.save(file);
    }

    private void checkParameters(List<Pattern> patterns) {
        for (Pattern pattern : patterns) {
            pattern.checkParameters();
        }
    }
}
