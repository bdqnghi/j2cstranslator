package com.ilog.translator.java2cs.tools.ant.patterns;

import java.util.LinkedList;
import java.util.List;


/**
 * LookFor from ant script
 * 
 * @author vpotapenko
 */
public class LookFor {

    private List<Line> lines = new LinkedList<Line>();

    public void addLine(Line line) {
        lines.add(line);
    }

    public List<String> getLines() {
        List<String> stringLines = new LinkedList<String>();
        for (Line line : lines) {
            stringLines.add(line.getText());
        }
        return stringLines;
    }
}
