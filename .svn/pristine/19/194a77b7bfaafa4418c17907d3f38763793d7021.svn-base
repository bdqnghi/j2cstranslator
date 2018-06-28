package com.ilog.translator.java2cs.tools.ant.patterns;

import org.apache.tools.ant.BuildException;

/**
 * Ant task pattern for post translation replacement
 * 
 * @author vpotapenko
 */
public class Pattern {

    private String fileName;

    private LookFor lookfor;
    private Replacement replacement;

    public void setFileName(String fileName) {
        this.fileName = fileName;
    }

    public void addLookfor(LookFor lookFor) {
        this.lookfor = lookFor;
    }

    public void addReplacement(Replacement replacement) {
        this.replacement = replacement;
    }

    public String getFileName() {
        return fileName;
    }

    public LookFor getLookfor() {
        return lookfor;
    }

    public Replacement getReplacement() {
        return replacement;
    }

    public void checkParameters() {
        if (fileName == null)
            throw new BuildException("Pattern's filename must be set");

        if (lookfor == null)
            throw new BuildException("Pattern's lookfor must be set");

        if (replacement == null)
            throw new BuildException("Pattern's replacement must be set");
    }
}
