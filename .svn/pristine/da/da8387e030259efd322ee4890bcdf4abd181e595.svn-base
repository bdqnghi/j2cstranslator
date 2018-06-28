package com.ilog.translator.java2cs.tools.ant.patterns;

import java.io.BufferedReader;
import java.io.BufferedWriter;
import java.io.File;
import java.util.ArrayList;
import java.util.List;

import org.apache.tools.ant.BuildException;

import com.ilog.translator.java2cs.tools.ant.J2CSTranslatorTaskUtils;

/**
 * Helper class for finding and replacing lines
 * 
 * @author vpotapenko
 */
public class FileCursor {

    private final List<String> fileLines = new ArrayList<String>();

    private int startIndex = 0;
    private int count;

    private boolean replaced;

    public void readFile(File file) {
        reset();
        readLines(file);
    }

    private void reset() {
        fileLines.clear();

        startIndex = 0;
        count = 0;
    }

    private void readLines(File file) {
        BufferedReader reader = null;
        try {
            reader = J2CSTranslatorTaskUtils.createReader(file);
            String line;
            while ((line = reader.readLine()) != null) {
                fileLines.add(line);
            }
        } catch (Exception e) {
             throw new BuildException(e);
        } finally {
            J2CSTranslatorTaskUtils.closeQuately(reader);
        }
    }

    public boolean find(List<String> lines) {
        if (lines.isEmpty()) return false;

        for (int i = startIndex; i < (fileLines.size() - lines.size() + 1); i++) {
            if (hasFileLines(lines, i)) {
                startIndex = i;
                count = lines.size();

                return true;
            }
        }
        return false;
    }

    private boolean hasFileLines(List<String> lines, int startFileIndex) {
        for (int i = 0; i < lines.size(); i++) {
            if (!equalLines(lines.get(i), fileLines.get(startFileIndex + i)))
                return false;
        }
        return true;
    }

    private boolean equalLines(String line1, String line2) {
        if (line1 == null || line2 == null) return false;

        return line1.equals(line2);
    }

    public void replace(List<String> lines) {
        for (int i = 0; i < count; i++) {
            fileLines.remove(startIndex);
        }

        fileLines.addAll(startIndex, lines);
        startIndex += lines.size();
        replaced = true;
    }

    public void save(File file) {
        if (!replaced) return;

        BufferedWriter writer = null;
        try {
            writer = J2CSTranslatorTaskUtils.createWriter(file);
            for (String line : fileLines) {
                writer.write(line);
                writer.newLine();
            }
        } catch (Exception e) {
            throw new BuildException(e);
        } finally {
            J2CSTranslatorTaskUtils.closeQuately(writer);
        }
    }
}
