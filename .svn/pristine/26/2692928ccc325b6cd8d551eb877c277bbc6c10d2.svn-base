package com.ilog.translator.java2cs.configuration;

import java.io.File;
import java.io.IOException;
import java.util.ArrayList;
import java.util.List;

import javax.xml.parsers.DocumentBuilder;
import javax.xml.parsers.DocumentBuilderFactory;

import org.eclipse.ui.console.ConsolePlugin;
import org.eclipse.ui.console.IConsole;
import org.eclipse.ui.console.IConsoleManager;
import org.eclipse.ui.console.MessageConsole;
import org.eclipse.ui.console.MessageConsoleStream;
import org.w3c.dom.Document;
import org.w3c.dom.Element;
import org.w3c.dom.Node;
import org.w3c.dom.NodeList;

import com.ilog.translator.java2cs.plugin.util.ProjectImporter;

public class GlobalOptions {

	private static String CONSOLE_NAME = "translation";
	
	private Logger logger;
	private String tempDir = "c:/Temp/";

	// Options
	private boolean headless;
	private boolean debug = false;
	private String logFile = "j2cstranslator-log.txt";
	private Logger.LogLevel logLevel = Logger.LogLevel.INFO;
	private String baseDestDir = tempDir;
	private String mappingAssemblyLocation = "${J2CSMappingLibrary}";
	private String configurationFileName = "java2csharp.xml";
	private String projectName;
	private String workingWorkspacePath = null;
	private boolean compileAfterTranslation = false;
	private boolean startUnitTestAfterCompilation = false;
	private boolean perfCount = false;
	private List<TranslatorDependency> referencedProjects;
	private List<String> classFilters = new ArrayList<String>();
	private List<String> packageFilters = new ArrayList<String>();

	//
	//
	//

	public GlobalOptions() {
	}

	//
	//
	//

	@Override
	public void finalize() {
		referencedProjects = null;
		classFilters = null;
		packageFilters = null;
	}

	//
	// findConsole
	//

	private MessageConsole findConsole(String name) {
		ConsolePlugin plugin = ConsolePlugin.getDefault();
		IConsoleManager conMan = plugin.getConsoleManager();
		IConsole[] existing = conMan.getConsoles();
		for (IConsole element : existing) {
			if (name.equals(element.getName())) {
				return (MessageConsole) element;
			}
		}
		// no console found, so create a new one
		MessageConsole myConsole = new MessageConsole(name, null);
		conMan.addConsoles(new IConsole[] { myConsole });
		return myConsole;
	}

	//
	// ===============================
	//
	//
	// LogFile
	//

	public String getLogFile() {
		return logFile;
	}

	public void setLogFile(String logFile) {
		this.logFile = logFile;
		if (logger != null) {
			try {
				logger.changeLogFile(logFile);
			} catch (final IOException e) {
				logger.logError("Error during change the log file to "
						+ logFile, e);
			}
		}
	}

	//
	// Logger
	//

	public Logger getLogger() {
		if (logger == null) {
			try {
				logger = new Logger(getLogFile(), logLevel);
				if (!headless) {
					final MessageConsole myConsole = findConsole(CONSOLE_NAME);
					final MessageConsoleStream out = myConsole
							.newMessageStream();
					logger.setOut(out);
				}
				//
			} catch (IOException e) {
				e.printStackTrace();
			}
		}
		return logger;
	}

	public void setLogger(Logger logger) {
		this.logger = logger;
	}

	//
	// Headless
	//

	public boolean isHeadless() {
		return headless;
	}

	public void setHeadless(boolean headless) {
		this.headless = headless;
	}

	//
	// Debug
	//

	public boolean isDebug() {
		return debug;
	}

	public void setDebug(boolean debug) {
		this.debug = debug;
	}

	//
	// BaseDestDir
	//

	public String getBaseDestDir() {
		return baseDestDir;
	}

	public void setBaseDestDir(String baseDestDir) {
		this.baseDestDir = baseDestDir;
	}

	//
	// TempDir
	//

	public String getTempDir() {
		return tempDir;
	}

	public void setTempDir(String tempDir) {
		this.tempDir = tempDir;
	}

	//
	// MappingAssemblyLocation
	//

	public String getMappingAssemblyLocation() {
		return mappingAssemblyLocation;
	}

	public void setMappingAssemblyLocation(String mappingAssemblyLocation) {
		this.mappingAssemblyLocation = mappingAssemblyLocation;
	}

	//
	// ConfigurationFileName
	//

	public String getConfigurationFileName() {
		return configurationFileName;
	}

	public void setConfigurationFileName(String name) {
		configurationFileName = name;
	}

	//
	// ProjectName
	//

	public String getProjectName() {
		return projectName;
	}

	public void setProjectName(String projectName) {
		this.projectName = projectName;
	}

	//
	// CompileAfterTranslation
	//

	public boolean isCompileAfterTranslation() {
		return compileAfterTranslation;
	}

	public void setCompileAfterTranslation(boolean compileAfterTranslation) {
		this.compileAfterTranslation = compileAfterTranslation;
	}

	//
	// StartUnitTestAfterCompilation
	//

	public boolean isStartUnitTestAfterCompilation() {
		return startUnitTestAfterCompilation;
	}

	public void setStartUnitTestAfterCompilation(
			boolean startUnitTestAfterCompilation) {
		this.startUnitTestAfterCompilation = startUnitTestAfterCompilation;
	}

	//
	// PerfCount
	//

	public boolean isPerfCount() {
		return perfCount;
	}

	public void setPerfCount(boolean val) {
		perfCount = val;
	}

	//
	// ========================
	//

	//
	// parseCommandLine
	//

	public List<TranslatorDependency> parseCommandLine(List<String> strs,
			ProjectImporter importer) {
		return null;
	}

	//
	// ========================
	//

	//
	// read
	//

	public List<TranslatorDependency> read(String fName) throws Exception {

		//
		final DocumentBuilderFactory docBuilderFactory = DocumentBuilderFactory
				.newInstance();
		final DocumentBuilder docBuilder = docBuilderFactory
				.newDocumentBuilder();
		final Document doc = docBuilder.parse(new File(fName));

		final Element root = doc.getDocumentElement();

		final String debugflag = root.getAttribute("debug");
		if (debugflag != null && debugflag.equals("true")) {
			setDebug(true);
		}

		// normalize text representation doc.getDocumentElement ().normalize ();
		if (isDebug()) {
			getLogger().logInfo(
					"Root element of the doc is " + root.getNodeName());
		}

		projectName = root.getAttribute("name");
		//
		final String tempDir = root.getAttribute("tmpDir");
		if (tempDir != null && !tempDir.equals(""))
			setTempDir(tempDir);
		//
		final String baseDestDir = root.getAttribute("outputdir");
		setBaseDestDir(baseDestDir);
		//
		final String confFilename = root.getAttribute("configurationFileName");
		if (confFilename != null && !confFilename.equals(""))
			setConfigurationFileName(confFilename);
		//
		final String logLevel_s = root.getAttribute("logLevel");
		if (confFilename != null && !confFilename.equals(""))
			logLevel = Logger.LogLevel.valueOf(logLevel_s);

		workingWorkspacePath = root.getAttribute("workspace");

		final String compileAfterTranslation_s = root
				.getAttribute("compileAfterTranslation");
		if (compileAfterTranslation_s != null) {
			compileAfterTranslation = Boolean
					.parseBoolean(compileAfterTranslation_s);
		}
		final String startUnitTestAfterCompilation_s = root
				.getAttribute("startUnitTestAfterCompilation");
		if (startUnitTestAfterCompilation_s != null) {
			startUnitTestAfterCompilation = Boolean
					.parseBoolean(startUnitTestAfterCompilation_s);
		}

		final String mappingAssemblyLocation_s = root
				.getAttribute("mappingAssemblyLocation");
		if (mappingAssemblyLocation_s != null) {
			setMappingAssemblyLocation(mappingAssemblyLocation_s);
		}

		/*
		 * final String confFileName = root
		 * .getAttribute("configurationFileName"); if (confFileName != null &&
		 * confFi) { setConfigurationFileName(confFileName); }
		 */

		//
		//
		//
		final NodeList listOfDependance = doc.getElementsByTagName("project");
		final int totalDependance = listOfDependance.getLength();
		if (isDebug()) {
			getLogger().logInfo("Total no of dependance : " + totalDependance);
		}

		referencedProjects = new ArrayList<TranslatorDependency>();

		for (int s = 0; s < listOfDependance.getLength(); s++) {
			final Node firstPassNode = listOfDependance.item(s);
			if (firstPassNode.getNodeType() == Node.ELEMENT_NODE) {
				final Element firstPassElement = (Element) firstPassNode;
				final String name = firstPassElement.getAttribute("name");
				final String profile = firstPassElement.getAttribute("profile");
				final TranslatorDependency dep = new TranslatorDependency(name,
						null, null, null, profile);
				referencedProjects.add(dep);
			}
		}

		//
		//
		//

		final NodeList filters = doc.getElementsByTagName("filters");
		if (filters.getLength() == 1) {
			Element filtersElement = (Element) filters.item(0);

			final NodeList listOfClassFilter = filtersElement
					.getElementsByTagName("class");
			final int totalClassFilters = listOfClassFilter.getLength();
			// if (isDebug()) {
			getLogger().logInfo(
					"Total no of class filters : " + totalClassFilters);
			// }
			for (int s = 0; s < totalClassFilters; s++) {
				final Node firstPassNode = listOfClassFilter.item(s);
				if (firstPassNode.getNodeType() == Node.ELEMENT_NODE) {
					final Element firstPassElement = (Element) firstPassNode;
					final String name = firstPassElement.getAttribute("name");
					classFilters.add(name);
					getLogger().logInfo("class filter " + name);
				}
			}

			//

			final NodeList listOfPackagesFilter = filtersElement
					.getElementsByTagName("package");
			final int totalPackagesFilter = listOfPackagesFilter.getLength();
			if (isDebug()) {
				getLogger().logInfo(
						"Total no of class filters : " + totalPackagesFilter);
			}
			for (int s = 0; s < totalPackagesFilter; s++) {
				final Node firstPassNode = listOfPackagesFilter.item(s);
				if (firstPassNode.getNodeType() == Node.ELEMENT_NODE) {
					final Element firstPassElement = (Element) firstPassNode;
					final String name = firstPassElement.getAttribute("name");
					packageFilters.add(name);
				}
			}

		}

		return referencedProjects;
	}

	//
	// Save
	//

	public void save() {
		final StringBuilder builder = new StringBuilder();
		builder.append("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
		builder.append("<translatorproject name=\"" + projectName + "\"\n");
		builder
				.append("                   outputdir=\"" + baseDestDir
						+ "\"\n");
		builder.append("                   tmpdir=\"" + tempDir + "\">\n");
		for (final TranslatorDependency dep : referencedProjects) {
			builder
					.append("   <project name=\"" + dep.getName() + "\""
							+ ">\n");
		}
		if (packageFilters.size() > 0 && classFilters.size() > 0) {
			builder.append("   <filters>\n");
			builder.append("   </filters>\n");
		}
		builder.append("</translatorproject>\n");
	}

	//
	// Filters
	//

	public String[] getPackageFilters() {
		return packageFilters.toArray(new String[packageFilters.size()]);
	}

	public String[] getClassFilters() {
		return classFilters.toArray(new String[classFilters.size()]);
	}
}
