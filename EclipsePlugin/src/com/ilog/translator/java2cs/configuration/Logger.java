package com.ilog.translator.java2cs.configuration;

import java.io.FileOutputStream;
import java.io.IOException;
import java.io.OutputStream;
import java.io.PrintStream;

import org.eclipse.ui.console.MessageConsoleStream;

public class Logger {
	
	public enum LogLevel {
		INFO,
		DEBUG,
		VERBOSE
	}

	private static final String TRANSLATOR_INFO = "[Info] ";
	private static final String TRANSLATOR_WARNING = "[Warning] ";
	private static final String TRANSLATOR_ERROR = "[Error] ";
	private static final String TRANSLATOR_DEBUG = "[Debug] ";
	private static final String TRANSLATOR_VERBOSE = "[Verbose] ";
	private static PrintStream out;
	private static PrintStream err;

	//
	
	private LogLevel level = LogLevel.INFO;	
	private OutputStream log;
	private static MessageConsoleStream console;

	//
	//
	//

	public Logger(String logFile, LogLevel level) throws IOException {
		Logger.out = System.out;
		Logger.err = System.out; // System.err;
		log = new FileOutputStream(logFile);
		this.level = level;
	}
	
	//
	// log level
	//
	
	public LogLevel getLogLevel() {
		return level;
	}

	//
	// changeLogFile
	//

	public void changeLogFile(String logFile) throws IOException {
		if (log != null)
			log.close();
		log = new FileOutputStream(logFile);
	}

	//
	// close
	//

	public void close() throws IOException {
		if (log != null) {
			log.close();
			log = null;
		}
	}

	//
	// logException
	//

	public void logException(String msg, Exception ex) {
		String message = TRANSLATOR_ERROR + msg + " : " + ex.getMessage();
		for (final StackTraceElement element : ex.getStackTrace()) {
			message += element.toString() + "\n";
		}

		if (Logger.console != null) {
			println(Logger.console, message);
		} else {
			Logger.err.println(message);
		}
		if (log != null)
			println(log, message);
	}

	//
	// logError
	//
	
	public void logError(String msg) {
		if (Logger.console != null) {
			println(Logger.console, TRANSLATOR_ERROR + msg);
		} else {
			Logger.err.println(TRANSLATOR_ERROR + msg);
		}
		if (log != null)
			println(log, TRANSLATOR_ERROR + msg);
	}
	
	public void logError(String msg, Exception e) {
		StackTraceElement[] elements = e.getStackTrace();
		String extraInfo = "";
		if (elements != null && elements.length > 0) {
			StackTraceElement last = elements[0];
			extraInfo = " [Exection in " + last.getFileName() + "." + last.getMethodName() + 
				" line " + last.getLineNumber() + "]";
		}
		if (Logger.console != null) {
			println(Logger.console, TRANSLATOR_ERROR + msg + extraInfo);
		} else {
			Logger.err.println(TRANSLATOR_ERROR + msg + extraInfo);
		}
		if (log != null)
			println(log, TRANSLATOR_ERROR + msg + extraInfo);
	}

	//
	// logWarning
	//
	
	public void logWarning(String msg) {
		if (Logger.console != null) {
			println(Logger.console, TRANSLATOR_WARNING + msg);
		} else {
			Logger.out.println(TRANSLATOR_WARNING + msg);
		}
		if (log != null)
			println(log, TRANSLATOR_WARNING + msg);
	}

	//
	// logInfo
	//
	public void logInfo(String msg) {
		if (Logger.console != null) {
			println(Logger.console, TRANSLATOR_INFO + msg);
		} else {
			Logger.out.println(TRANSLATOR_INFO + msg);
		}
		if (log != null)
			println(log, TRANSLATOR_INFO + msg);
	}

	//
	// logVerbose
	//
	public void logVerbose(String msg) {
		if (level == LogLevel.VERBOSE) {
			if (Logger.console != null) {
				println(Logger.console, TRANSLATOR_VERBOSE + msg);
			} else {
				Logger.out.println(TRANSLATOR_VERBOSE + msg);
			}
			if (log != null)
				println(log, TRANSLATOR_VERBOSE + msg);
		}
	}
	
	public void logVerboseNoLN(String msg) {
		if (level == LogLevel.VERBOSE) {
			if (Logger.console != null) {
				print(Logger.console, TRANSLATOR_VERBOSE + msg);
			} else {
				Logger.out.print(TRANSLATOR_VERBOSE + msg);
			}
			if (log != null)
				print(log, TRANSLATOR_VERBOSE + msg);
		}
	}

	//
	// logDebug
	//
	public void logDebug(String msg) {
		if (Logger.console != null) {
			println(Logger.console, TRANSLATOR_DEBUG + msg);
		} else {
			Logger.out.println(TRANSLATOR_DEBUG + msg);
		}
		if (log != null)
			println(log, TRANSLATOR_DEBUG + msg);
	}

	//
	// println
	//

	private void println(OutputStream stream, String msg) {
		try {
			stream.write(msg.getBytes());
			stream.write('\n');
		} catch (final IOException e) {

		}
	}
	
	private void print(OutputStream stream, String msg) {
		try {
			stream.write(msg.getBytes());			
		} catch (final IOException e) {

		}
	}

	//
	// setOut
	// 
	
	public void setOut(MessageConsoleStream out2) {
		Logger.console = out2;
	}
}
