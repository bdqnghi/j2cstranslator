package com.ilog.translator.java2cs.tools.ant;

import java.io.BufferedReader;
import java.io.BufferedWriter;
import java.io.Closeable;
import java.io.File;
import java.io.FileNotFoundException;
import java.io.FileReader;
import java.io.FileWriter;
import java.io.IOException;

/**
 * Ant task utils
 * 
 * @author vpotapenko
 */
public class J2CSTranslatorTaskUtils {

    public static BufferedReader createReader(File file) throws FileNotFoundException {
        return new BufferedReader(new FileReader(file));
    }

    public static BufferedWriter createWriter(File file) throws IOException {
        return new BufferedWriter(new FileWriter(file));
    }

    public static void closeQuately(Closeable closable) {
        try {
            if (closable != null) closable.close();
        } catch (Exception ignore) {}
    }
}
