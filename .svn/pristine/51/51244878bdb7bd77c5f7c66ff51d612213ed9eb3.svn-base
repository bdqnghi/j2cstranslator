package com.ilog.translator.java2cs.configuration;

import java.io.File;
import java.io.FileWriter;
import java.io.IOException;
import java.io.PrintWriter;
import java.io.Writer;
import java.util.ArrayList;
import java.util.Collection;
import java.util.List;
import java.util.Set;
import java.util.TreeSet;

import javax.xml.parsers.DocumentBuilder;
import javax.xml.parsers.DocumentBuilderFactory;
import javax.xml.parsers.ParserConfigurationException;

import org.eclipse.core.resources.IResource;
import org.eclipse.core.runtime.CoreException;
import org.eclipse.core.runtime.NullProgressMonitor;
import org.eclipse.jdt.core.ICompilationUnit;
import org.eclipse.jdt.core.IJavaElement;
import org.eclipse.jdt.core.IJavaProject;
import org.eclipse.jdt.internal.core.PackageFragment;
import org.eclipse.jdt.internal.core.PackageFragmentRoot;
import org.eclipse.jdt.internal.corext.util.JavaModelUtil;
import org.w3c.dom.Document;
import org.w3c.dom.Element;
import org.w3c.dom.Node;
import org.w3c.dom.NodeList;
import org.xml.sax.SAXException;

import com.ilog.translator.java2cs.configuration.TranslatorProjectOptions.VSGenerationPolicy;
import com.ilog.translator.java2cs.configuration.info.Constants;
import com.ilog.translator.java2cs.configuration.info.MappingsInfo;
import com.ilog.translator.java2cs.translation.ITranslationContext;
import com.ilog.translator.java2cs.translation.VisualStudioCsharpProject;
import com.ilog.translator.java2cs.translation.util.TranslationUtils;
import com.ilog.translator.java2cs.util.CSharpModelUtil;
import com.ilog.translator.java2cs.util.Utils;
import com.sun.org.apache.xml.internal.serialize.OutputFormat;
import com.sun.org.apache.xml.internal.serialize.XMLSerializer;

@SuppressWarnings("restriction")
public class TranslationConfiguration {

	private static final String CSPROJ_EXTENTION = ".csproj";
	private static final String translatorDirectory = File.separator
			+ Utils.buildPath("src", "main", "translator");

	/**
	 * The original project, the one we choose in eclipse
	 */
	private final IJavaProject originalProject;

	/**
	 * The copying project, the one which we works on (modify files)
	 */
	private final IJavaProject workingProject;

	/**
	 * The description on the translation (differents passes)
	 */
	private TranslationDescriptor descriptor;

	private final Logger logger;
	private TranslatorProjectOptions options;

	//
	//
	//

	public TranslationConfiguration(IJavaProject originalProject,
			IJavaProject workingProject, TranslatorProjectOptions options) {
		this.originalProject = originalProject;
		this.workingProject = workingProject;
		this.options = options;
		logger = options.getGlobalOptions().getLogger();
		descriptor = new TranslationDescriptor(logger, options.getGlobalOptions().isDebug());
	}

	//
	// finalize
	//
	
	@Override
	public void finalize() {
		descriptor = null;
		options = null;
	}
	
	//
	// descriptor
	//

	public TranslationDescriptor getTranslationDescriptor() {
		return descriptor;
	}
	
	//
	//
	//
	
	/**
	 * Generate a visual studio project file that contains all translated files.
	 */
	public void generateVSProjectFile(ITranslationContext context,
			List<ICompilationUnit> files) throws IOException,
			ParserConfigurationException, SAXException {
		final VSGenerationPolicy generationMode = options.getVSGenerationMode();
		if (generationMode == TranslatorProjectOptions.VSGenerationPolicy.GENERATE) {
			String projectFileName = options.getVSProjectName();
			if (projectFileName != null
					&& !projectFileName.endsWith(CSPROJ_EXTENTION))
				projectFileName += CSPROJ_EXTENTION;

			if (projectFileName == null)
				projectFileName = Utils.normalize(originalProject.getProject()
						.getName())
						+ CSPROJ_EXTENTION;

			String header = "";
			header += "<Project DefaultTargets=\"Build\" xmlns=\"http://schemas.microsoft.com/developer/msbuild/2003\"";
			if (options.getVsVersion() == TranslatorProjectOptions.VSVersion.VS2008)
				header += " ToolsVersion=\"3.5\"";
			else if (options.getVsVersion() == TranslatorProjectOptions.VSVersion.VS2010)
				header += " ToolsVersion=\"4.0\"";
			header += ">\n";
			header += "	 <PropertyGroup>\n";
			header += "    <Configuration Condition=\" '$(Configuration)' == '' \">Debug</Configuration>\n";
			header += "    <Platform Condition=\" '$(Platform)' == '' \">AnyCPU</Platform>\n";
			header += "    <ProjectGuid></ProjectGuid>\n";
			if (options.getVSProjectType() == TranslatorProjectOptions.VSProjectKind.CLASS_LIBRARY)
				header += "    <OutputType>Library</OutputType>\n";
			if (options.getVSProjectType() == TranslatorProjectOptions.VSProjectKind.CONSOLE_APPLICATION)
				header += "    <OutputType>Exe</OutputType>\n";
			if (options.getVSProjectType() == TranslatorProjectOptions.VSProjectKind.WINDOWS_APPLICATION)
				header += "    <OutputType>WinExe</OutputType>\n";
			header += "    <NoStandardLibraries>false</NoStandardLibraries>\n";
			header += "    <AssemblyName>TestShared</AssemblyName>\n";
			header += "    <RootNamespace>ILOG.Rules</RootNamespace>\n";
			if (options.getVSProjectType() == TranslatorProjectOptions.VSProjectKind.CONSOLE_APPLICATION) {
				header += "    <StartupObject>"
						+ options.getVSProjectEntryPoint()
						+ "</StartupObject>\n";
			}
			header += "  </PropertyGroup>\n";
			header += "  <PropertyGroup Condition=\" '$(Configuration)|$(Platform)' == 'Debug|AnyCPU' \">\n";
			header += "     <DebugSymbols>true</DebugSymbols>\n";
			header += "     <DebugType>full</DebugType>\n";
			header += "     <Optimize>false</Optimize>\n";
			header += "     <OutputPath>.\\bin\\Debug\\</OutputPath>\n";
			header += "     <DefineConstants>TRACE;DEBUG;UNIT_TESTS</DefineConstants>\n";
			header += "  </PropertyGroup>\n";
			header += "  <PropertyGroup Condition=\" '$(Configuration)|$(Platform)' == 'Release|AnyCPU' \">\n";
			header += "     <DebugType>pdbonly</DebugType>\n";
			header += "     <Optimize>true</Optimize>\n";
			header += "     <OutputPath>.\\bin\\Release\\</OutputPath>\n";
			header += "  <DefineConstants>TRACE</DefineConstants>\n";
			header += "  </PropertyGroup>\n";
			header += "  <ItemGroup>\n";
			header += "     <Reference Include=\"System\" />\n";
			header += "     <Reference Include=\"System.configuration\" />\n";
			header += "     <Reference Include=\"System.Data\" />\n";
			header += "     <Reference Include=\"System.Xml\" />\n";
			header += "     <Reference Include=\"nunit.framework\" />\n";
			header += "     <Reference Include=\"ILOG.J2CsMapping\">\n";
			header += "        <SpecificVersion>False</SpecificVersion>\n";
			header += "        <HintPath>"
					+ options.getGlobalOptions().getMappingAssemblyLocation()
					+ "</HintPath>\n";
			header += "     </Reference>\n";
			header += "  </ItemGroup>\n";
			header += "  <ItemGroup>\n";

			String content = "";
			final File destPathFile = new File(options.getSourcesDestDir()
					+ File.separator + projectFileName);
			final String absolutePathOfDest = destPathFile.getCanonicalFile()
					.getParentFile().getAbsolutePath();
			for (final ICompilationUnit cUnit : files) {
				final String path = TranslationUtils.computePathForSource(
						context, cUnit);
				//
				final String javafilename = cUnit.getElementName();
				final String csfilename = javafilename.replace(
						JavaModelUtil.DEFAULT_CU_SUFFIX,
						CSharpModelUtil.DEFAULT_CU_SUFFIX);

				final File f = new File(path);
				final String absolutePathOfFile = f.getAbsolutePath();

				String relativePath = absolutePathOfFile
						.substring(absolutePathOfDest.length());
				if (relativePath.startsWith("\\")) {
					relativePath = relativePath.substring(1);
				}

				content += "    <Compile Include=\"" + relativePath
						+ File.separator + csfilename + "\" />\n";
			}

			String footer = "";
			footer += "  </ItemGroup>\n";
			footer += "  <Import Project=\"$(MSBuildBinPath)\\Microsoft.CSHARP.Targets\" />\n";
			footer += "  <ProjectExtensions>\n";
			footer += "     <VisualStudio AllowExistingFolder=\"true\" />\n";
			footer += "  </ProjectExtensions>\n";
			footer += "</Project>\n";

			final Writer writer = new PrintWriter(options.getSourcesDestDir()
					+ File.separator + projectFileName);
			writer.write(header + content + footer);
			writer.flush();
			writer.close();
		} else if (generationMode == TranslatorProjectOptions.VSGenerationPolicy.MERGE_FILES
				|| generationMode == TranslatorProjectOptions.VSGenerationPolicy.REPLACE_FILES) {
			String projectFileName = options.getVSProjectName();
			if (!projectFileName.endsWith(CSPROJ_EXTENTION))
				projectFileName += CSPROJ_EXTENTION;

			final File destPathFile = new File(options.getSourcesDestDir()
					+ File.separator + projectFileName);
			final String absolutePathOfDest = destPathFile.getCanonicalFile()
					.getParentFile().getAbsolutePath()
					+ File.separator;

			final File projectFile = destPathFile.getCanonicalFile();
			final VisualStudioCsharpProject vsProject = new VisualStudioCsharpProject(
					projectFile);
			if (generationMode == TranslatorProjectOptions.VSGenerationPolicy.MERGE_FILES) {
				vsProject.mergeIncludeFiles(getFilesPath(absolutePathOfDest,
						context, files));
			}
			if (generationMode == TranslatorProjectOptions.VSGenerationPolicy.REPLACE_FILES) {
				vsProject.removeAllFiles();
				vsProject.addFiles(getFilesPath(absolutePathOfDest, context,
						files));
			}
			vsProject.save();
		}
	}

	private List<String> getFilesPath(String absolutePathOfDest,
			ITranslationContext context, List<ICompilationUnit> files) {
		final List<String> result = new ArrayList<String>();
		for (final ICompilationUnit cUnit : files) {
			final String path = TranslationUtils.computePathForSource(context,
					cUnit);
			//
			final String javafilename = cUnit.getElementName();
			final String csfilename = javafilename.replace(
					JavaModelUtil.DEFAULT_CU_SUFFIX,
					CSharpModelUtil.DEFAULT_CU_SUFFIX);

			final File f = new File(path);
			final String absolutePathOfFile = f.getAbsolutePath();

			String relativePath = absolutePathOfFile
					.substring(absolutePathOfDest.length());
			if (relativePath.startsWith("\\")) {
				relativePath = relativePath.substring(1);
			}
			result.add(relativePath + File.separator + csfilename);
		}
		return result;
	}

	//
	//
	//

	/**
	 * Update an existing visual studio solution with new files
	 * 
	 */
	private void updateVSProjectFile(String fName, ITranslationContext context,
			List<ICompilationUnit> files) throws ParserConfigurationException,
			SAXException, IOException {
		//
		final File destPathFile = new File(options.getSourcesDestDir());
		final String absolutePathOfDest = destPathFile.getAbsolutePath();
		final List<String> copyOfFiles = new ArrayList<String>();
		for (final ICompilationUnit cUnit : files) {
			final String destDir = options.getSourcesDestDir() + File.separator;
			final String packageName = context.getPackageName(cUnit.getPath()
					.toString(), cUnit.getJavaProject().getProject());
			final String path = destDir.replace("/", "\\")
					+ ((packageName == null) ? "" : packageName.replace(".",
							File.separator));

			final String javafilename = cUnit.getElementName();
			final String csfilename = javafilename.replace(
					JavaModelUtil.DEFAULT_CU_SUFFIX,
					CSharpModelUtil.DEFAULT_CU_SUFFIX);

			final File f = new File(path);
			final String absolutePathOfFile = f.getAbsolutePath();

			String relativePath = absolutePathOfFile
					.substring(absolutePathOfDest.length());
			if (relativePath.startsWith("\\")) {
				relativePath = relativePath.substring(1);
			}

			copyOfFiles.add(relativePath + File.separator + csfilename);
		}
		//
		final DocumentBuilderFactory docBuilderFactory = DocumentBuilderFactory
				.newInstance();
		final DocumentBuilder docBuilder = docBuilderFactory
				.newDocumentBuilder();
		final Document doc = docBuilder.parse(new File(fName));

		// Element root = doc.getDocumentElement();

		final NodeList listOfItemGroups = doc.getElementsByTagName("ItemGroup");
		final int totalItemGroups = listOfItemGroups.getLength();

		//
		for (int s = 0; s < totalItemGroups; s++) {
			final Node itemGroupNode = listOfItemGroups.item(s);
			if (itemGroupNode.getNodeType() == Node.ELEMENT_NODE) {
				final Element itemGroupElement = (Element) itemGroupNode;
				final NodeList listOfIncludes = itemGroupElement
						.getElementsByTagName("Compile");
				final int totalIncludes = listOfIncludes.getLength();
				Element includeElement = null;
				for (int i = 0; i < totalIncludes; i++) {
					final Node includeNode = listOfIncludes.item(i);
					if (includeNode.getNodeType() == Node.ELEMENT_NODE) {
						includeElement = (Element) includeNode;
						final String csFile = includeElement
								.getAttribute("Include");
						if (copyOfFiles.contains(csFile)) {
							copyOfFiles.remove(csFile);
						} else {
							// remove it !
							// itemGroupElement.removeChild(includeElement);
						}
					} else {
						includeElement = null;
					}
				}
				//
				if (includeElement != null) {
					for (final String newCsFile : copyOfFiles) {
						final Element newCompile = doc.createElement("Compile");
						newCompile.setAttribute("Include", newCsFile);
						itemGroupElement.appendChild(newCompile);
					}
				}
			}
		}

		final Writer writer = new FileWriter(fName);
		final XMLSerializer seri = new XMLSerializer(writer, new OutputFormat());
		seri.serialize(doc);
		writer.close();
	}

	//
	// original project
	//

	/**
	 * Return the original project
	 */
	public IJavaProject getOriginalProject() {
		return originalProject;
	}

	//
	// readOrCreateProjectConfigurationFile
	//

	/**
	 * Read the configuration file for the project to be translated, that file
	 * is called "translation.xml" and contains parameters values.
	 */
	public void readOrCreateProjectConfigurationFile() throws Exception {
		if (options.getProjectConfigurationFileName() != null) {
			try {
				final boolean res = options.read(originalProject, false);
			} catch (final SAXException e) {
				options.getGlobalOptions().getLogger().logException(
						"Error during reading file " + options.getProjectConfigurationFileName(), e);
			}
		} else {
			final String projectLoc = originalProject.getProject()
					.getLocation().toFile().getAbsolutePath();
			final File directory = new File(projectLoc + translatorDirectory);
			if (!directory.exists()) {
				if (options.isCreateTranslatorConfigurationFiles()) {
					createAllConfigurationFiles(originalProject);
					createAllConfigurationFiles(workingProject);
				} else {
					logger
							.logWarning("Can't find translator directory for project "
									+ originalProject.getProject().getName());
				}
				//
				return;
			}

		}
	}

	//
	// createAllConfigurationFiles
	//

	/**
	 * Create the translator configuration files :
	 * - the project translator configuration file (which contains options and dependance)
	 * - the project mapping file (which contains mapping from java to csharp for this project) 
	 */
	public String createAllConfigurationFiles(IJavaProject project)
			throws CoreException, IOException {
		if (options.getGlobalOptions().isDebug())
			getLogger().logInfo("Project is " + project.getProject().getName());

		final String baseDir = project.getProject().getLocation().toFile()
				.getAbsolutePath()
				+ File.separator;

		if (options.getGlobalOptions().isDebug())
			getLogger().logInfo("Project dir " + baseDir.toString());

		final String configurationDir = Utils.buildPath(translatorDirectory,
				"configuration");

		final File translatorDirFile = new File(baseDir + translatorDirectory);

		if (options.getGlobalOptions().isDebug())
			getLogger().logInfo("Translator dir " + translatorDirectory);

		if (!translatorDirFile.exists()) {
			if (!translatorDirFile.mkdirs()) {
				getLogger().logInfo(
						"Unable to create directory " + translatorDirectory);
				return null;
			}
		}

		final File configurationDirFile = new File(baseDir + configurationDir);

		if (options.getGlobalOptions().isDebug())
			getLogger().logInfo("Configuration dir " + configurationDir);

		if (!configurationDirFile.exists()) {
			if (!configurationDirFile.mkdirs()) {
				getLogger().logInfo(
						"Unable to create directory " + configurationDir);
				return null;
			}
		}

		//
		// Project mappings file
		//
		final String projectConfiguration = Utils.normalize(originalProject
				.getProject().getName())
				+ MappingsInfo.XML_MAPPING_FILE_EXTENTION;
		final File projectConfigurationFile = new File(baseDir
				+ configurationDir + projectConfiguration);
		createProjectMappingFile(project, projectConfigurationFile);

		//
		// Project translator configuration file
		//
		final String translatorConfiguration = "translation.xml";
		final File translatorConfigurationFile = new File(baseDir
				+ translatorDirectory + translatorConfiguration);
		createProjectTranslatorConfigurationFile(project, translatorConfigurationFile);

		return baseDir + translatorDirectory + translatorConfiguration;
	}

	/**
	 * Create translator configuration file (translator.xml).
	 * The file that contains options to use for translated the current project.
	 * 
	 * @param project
	 * @param translatorConfigurationFile
	 */
	private void createProjectTranslatorConfigurationFile(IJavaProject project,
			File translatorConfigurationFile) throws CoreException, IOException {
		if (options.getGlobalOptions().isDebug())
			getLogger().logInfo(
					"Translator Configuration file "
							+ translatorConfigurationFile.getAbsolutePath());

		if (!translatorConfigurationFile.exists()) {
			translatorConfigurationFile.createNewFile();
			if (translatorConfigurationFile.canWrite()) {
				options
						.setProjectConfigurationFileName(translatorConfigurationFile
								.getAbsolutePath());
				options.setProjectName(project.getElementName());
				options.save();
				project.getProject().refreshLocal(IResource.DEPTH_INFINITE,
						new NullProgressMonitor());
			} else {
				if (options.getGlobalOptions().isDebug())
					getLogger().logInfo(
							"Can't write Translator configuration file "
									+ translatorConfigurationFile
											.getAbsolutePath() + ".");
			}
		} else {
			if (options.getGlobalOptions().isDebug())
				getLogger().logInfo(
						"Translator configuration file "
								+ translatorConfigurationFile.getAbsolutePath()
								+ " already exists.");
		}
	}

	/**
	 * Creates the project default mapping file (myproject.mappingml)
	 * 
	 * @param project
	 * @param projectMappingFile
	 * @throws CoreException
	 */
	public void createProjectMappingFile(IJavaProject project,
			File projectMappingFile) throws CoreException, IOException {
		if (options.getGlobalOptions().isDebug())
			getLogger().logInfo(
					"Project Configuration file "
							+ projectMappingFile.getAbsolutePath());

		if (!projectMappingFile.exists()) {
			projectMappingFile.createNewFile();
			if (projectMappingFile.canWrite()) {
				final FileWriter writer = new FileWriter(projectMappingFile);
				writer.append("<!-- -->\n");
				writer
						.append("<!-- ILOG Java 2 CSharp translator mapping file for project "
								+ Utils.normalize(project.getProject()
										.getName()) + " -->\n");
				writer.append("<!-- -->\n");
				writer.append("<mapping>\n");
				final Collection<String> pcks = getCompilationUnits(project);
				writer.append(Constants.TAB + "<!--          -->\n");
				writer.append(Constants.TAB + "<!-- packages -->\n");
				writer.append(Constants.TAB + "<!--          -->\n");
				writer.append(Constants.TAB + "<packages>\n");
				for (final String pck : pcks) {
					final String pName = pck;
					if (pName != null && !pName.equals("")) {
						writer.append(Constants.TWOTAB + "<!--               -->\n");
						writer.append(Constants.TWOTAB + "<!-- " + pName + " -->\n");
						writer.append(Constants.TWOTAB + "<!--              -->\n");
						writer.append(Constants.TWOTAB + "<package name=\"" + pName + "\">\n");
						writer.append(Constants.TWOTAB + "</package>\n");
					}
				}
				writer.append(Constants.TAB + "</packages>\n");
				writer.append("</mapping>\n");
				writer.flush();
				writer.close();
				project.getProject().refreshLocal(IResource.DEPTH_INFINITE,
						new NullProgressMonitor());
			} else {
				if (options.getGlobalOptions().isDebug())
					getLogger().logInfo(
							"Can't write Project configuration file "
									+ projectMappingFile.getAbsolutePath()
									+ ".");
			}
		} else {
			if (options.getGlobalOptions().isDebug())
				getLogger().logInfo(
						"Project Configuration file "
								+ projectMappingFile.getAbsolutePath()
								+ " already exists.");
		}
	}

	/**
	 * Get all compilation unit from this project
	 * 
	 * @param project
	 * @return
	 * @throws CoreException
	 */
	private static Collection<String> getCompilationUnits(IJavaProject project)
			throws CoreException {
		final Set<String> cus = new TreeSet<String>();
		final IJavaElement[] children = project.getChildren();
		for (final IJavaElement element : children) {
			if (element.getElementType() == IJavaElement.PACKAGE_FRAGMENT_ROOT) {
				final PackageFragmentRoot root = (PackageFragmentRoot) element;
				final IJavaElement[] child = root.getChildren();
				for (final IJavaElement element0 : child) {
					if (element0.getElementType() == IJavaElement.PACKAGE_FRAGMENT) {
						final PackageFragment fragment = (PackageFragment) element0;
						final ICompilationUnit[] units = fragment
								.getCompilationUnits();
						if (units != null) {
							for (final ICompilationUnit unit : units) {
								if (unit.getPackageDeclarations() != null
										&& unit.getPackageDeclarations().length > 0) {
									final String pck = unit
											.getPackageDeclarations()[0]
											.getElementName();
									cus.add(pck);
								}
							}
						}
					}
				}
			}
		}
		return cus;
	}

	//
	// ==========================================================================================
	//

	/**
	 * Initialize the translator with both global and project variables
	 */
	public void init(String configName, boolean readProjectOptions)
			throws Exception {
		descriptor.readTranslationProcessDescriptorFile(configName);
		if (!options.isCommandLineVersion()) {
			if (readProjectOptions) {
				readOrCreateProjectConfigurationFile();
			}
		} else {
			// Ok we can find all need data in variables
			options.populateDependency(logger, originalProject);
		}
	}

	//
	// Get the logger
	//

	public Logger getLogger() {
		return logger;
	}

	//
	// Options
	//

	public TranslatorProjectOptions getOptions() {
		return options;
	}

	public void setOptions(TranslatorProjectOptions options) {
		this.options = options;
	}

	//
	// Working project
	//

	public IJavaProject getWorkingProject() {
		return workingProject;
	}

}
