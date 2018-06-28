package com.ilog.translator.java2cs.configuration;

import java.io.File;
import java.io.FileInputStream;
import java.io.FileNotFoundException;
import java.io.FileWriter;
import java.io.IOException;
import java.util.ArrayList;
import java.util.Collection;
import java.util.HashMap;
import java.util.Iterator;
import java.util.List;
import java.util.Map;
import java.util.Set;
import java.util.TreeSet;

import javax.xml.parsers.DocumentBuilder;
import javax.xml.parsers.DocumentBuilderFactory;
import javax.xml.parsers.ParserConfigurationException;

import org.eclipse.core.resources.IFile;
import org.eclipse.core.resources.IFolder;
import org.eclipse.core.resources.IProject;
import org.eclipse.core.runtime.CoreException;
import org.eclipse.core.runtime.IPath;
import org.eclipse.core.runtime.NullProgressMonitor;
import org.eclipse.jdt.core.IJavaProject;
import org.eclipse.jdt.core.JavaCore;
import org.eclipse.jdt.core.JavaModelException;
import org.w3c.dom.Document;
import org.w3c.dom.Element;
import org.w3c.dom.Node;
import org.w3c.dom.NodeList;
import org.xml.sax.SAXException;

import com.ilog.translator.java2cs.configuration.options.ArrayOfStringOptionBuilder;
import com.ilog.translator.java2cs.configuration.options.ArrayOfStringOptionEditor;
import com.ilog.translator.java2cs.configuration.options.BooleanOptionBuilder;
import com.ilog.translator.java2cs.configuration.options.BooleanOptionEditor;
import com.ilog.translator.java2cs.configuration.options.Builder;
import com.ilog.translator.java2cs.configuration.options.DotNetFramework;
import com.ilog.translator.java2cs.configuration.options.DotNetFrameworkOptionBuilder;
import com.ilog.translator.java2cs.configuration.options.DotNetFrameworkOptionEditor;
import com.ilog.translator.java2cs.configuration.options.JDK;
import com.ilog.translator.java2cs.configuration.options.JDKOptionBuilder;
import com.ilog.translator.java2cs.configuration.options.JDKOptionEditor;
import com.ilog.translator.java2cs.configuration.options.MethodMappingPolicy;
import com.ilog.translator.java2cs.configuration.options.MethodMappingPolicyOptionBuilder;
import com.ilog.translator.java2cs.configuration.options.MethodMappingPolicyOptionEditor;
import com.ilog.translator.java2cs.configuration.options.OptionImpl;
import com.ilog.translator.java2cs.configuration.options.PackageMappingPolicy;
import com.ilog.translator.java2cs.configuration.options.PackageMappingPolicyOptionBuilder;
import com.ilog.translator.java2cs.configuration.options.PackageMappingPolicyOptionEditor;
import com.ilog.translator.java2cs.configuration.options.ResourcesCopyPolicyOptionEditor;
import com.ilog.translator.java2cs.configuration.options.SourcesReplacementPolicyOptionEditor;
import com.ilog.translator.java2cs.configuration.options.StringOptionBuilder;
import com.ilog.translator.java2cs.configuration.options.StringOptionEditor;
import com.ilog.translator.java2cs.configuration.options.UnitTestLibraryOptionEditor;
import com.ilog.translator.java2cs.configuration.options.VSGenerationPolicyOptionEditor;
import com.ilog.translator.java2cs.configuration.options.VSProjectKindOptionEditor;
import com.ilog.translator.java2cs.configuration.options.VSVersionOptionEditor;
import com.ilog.translator.java2cs.util.Utils;

public class TranslatorProjectOptions {

	//
	// enum for options
	//
	
	public enum SourcesReplacementPolicy {
		DELETE, REPLACE, ADD_NEW {
			@Override
			public String toString() {
				return "ONLY_ADD_NEW";
			}
		}
	}

	public enum VSGenerationPolicy {
		NO_GENERATION {
			@Override
			public String toString() {
				return "none";
			}
		},
		MERGE_FILES {
			@Override
			public String toString() {
				return "merge";
			}
		},
		REPLACE_FILES {
			@Override
			public String toString() {
				return "replace";
			}
		},
		GENERATE {
			@Override
			public String toString() {
				return "generation";
			}
		}
	}

	public enum VSVersion {
		VS2005, VS2008, VS2010
	}

	public enum VSProjectKind {
		CLASS_LIBRARY, CONSOLE_APPLICATION, WINDOWS_APPLICATION
	}

	public enum ResourcesCopyPolicy {
		OVERRIDE_POLICY {
			@Override
			public String toString() {
				return "override";
			}
		},
		DONOTREPLACE_POLICY {
			@Override
			public String toString() {
				return "donotreplace";
			}
		}
	}

	public enum UnitTestLibrary {
		JUNIT3 {
			@Override
			public String toString() {
				return "junit";
			}
		},
		JUNIT4 {
			@Override
			public String toString() {
				return "junit4";
			}
		},
		TESTNG {
			@Override
			public String toString() {
				return "testng";
			}
		},
		NONE {
			@Override
			public String toString() {
				return "none";
			}
		}
	}

	//
	// Option class
	//
	
	static class OldVSProjectOptionBuilder implements
			Builder<OptionImpl<VSGenerationPolicy>> {
		public void build(String value, OptionImpl<VSGenerationPolicy> option) {
			if (Boolean.parseBoolean(value))
				option.setValue(VSGenerationPolicy.GENERATE);
		}

		public String createStringValue(OptionImpl<VSGenerationPolicy> option) {
			return option.getValue().toString();
		}
	}

	static class VSGenerationPolicyOptionBuilder implements
			Builder<OptionImpl<VSGenerationPolicy>> {
		public void build(String value, OptionImpl<VSGenerationPolicy> option) {
			if (value.equals("merge"))
				option.setValue(VSGenerationPolicy.MERGE_FILES);
			if (value.equals("replace"))
				option.setValue(VSGenerationPolicy.REPLACE_FILES);
			if (value.equals("generation"))
				option.setValue(VSGenerationPolicy.GENERATE);
		}

		public String createStringValue(OptionImpl<VSGenerationPolicy> option) {
			return option.getValue().toString();
		}
	}

	static class VSProjecKindOptionBuilder implements
			Builder<OptionImpl<VSProjectKind>> {
		public void build(String value, OptionImpl<VSProjectKind> option) {
			if (value.equals("CLASS_LIBRARY"))
				option.setValue(VSProjectKind.CLASS_LIBRARY);
			if (value.equals("CONSOLE_APPLICATION"))
				option.setValue(VSProjectKind.CONSOLE_APPLICATION);
			if (value.equals("WINDOWS_APPLICATION"))
				option.setValue(VSProjectKind.WINDOWS_APPLICATION);
		}

		public String createStringValue(OptionImpl<VSProjectKind> option) {
			return option.getValue().toString();
		}
	}

	static class VSVersionOptionBuilder implements Builder<OptionImpl<VSVersion>> {
		public void build(String value, OptionImpl<VSVersion> option) {
			if (value.equals("VS2005"))
				option.setValue(VSVersion.VS2005);
			if (value.equals("VS2008"))
				option.setValue(VSVersion.VS2008);
			if (value.equals("VS2010"))
				option.setValue(VSVersion.VS2010);
		}

		public String createStringValue(OptionImpl<VSVersion> option) {
			return option.getValue().toString();
		}
	}

	static class SourcesReplacementPolicyOptionBuilder implements
			Builder<OptionImpl<SourcesReplacementPolicy>> {
		public void build(String value, OptionImpl<SourcesReplacementPolicy> option) {
			if (value.equals("REPLACE"))
				option.setValue(SourcesReplacementPolicy.REPLACE);
			if (value.equals("DELETE"))
				option.setValue(SourcesReplacementPolicy.DELETE);
			if (value.equals("ONLY_ADD_NEW"))
				option.setValue(SourcesReplacementPolicy.ADD_NEW);
		}

		public String createStringValue(OptionImpl<SourcesReplacementPolicy> option) {
			return option.getValue().toString();
		}
	}

	static class ResourcesCopyPolicyOptionBuilder implements
			Builder<OptionImpl<ResourcesCopyPolicy>> {
		public void build(String value, OptionImpl<ResourcesCopyPolicy> option) {
			if (value.equals("override"))
				option.setValue(ResourcesCopyPolicy.OVERRIDE_POLICY);
			if (value.equals("donotreplace"))
				option.setValue(ResourcesCopyPolicy.DONOTREPLACE_POLICY);
		}

		public String createStringValue(OptionImpl<ResourcesCopyPolicy> option) {
			return option.getValue().toString();
		}
	}

	static class UnitTestLibraryOptionBuilder implements
			Builder<OptionImpl<UnitTestLibrary>> {
		public void build(String value, OptionImpl<UnitTestLibrary> option) {
			if (value.equals("junit"))
				option.setValue(UnitTestLibrary.JUNIT3);
			else if (value.equals("junit4"))
				option.setValue(UnitTestLibrary.JUNIT4);
			else if (value.equals("testng"))
				option.setValue(UnitTestLibrary.TESTNG);
			else
				option.setValue(UnitTestLibrary.NONE);
		}

		public String createStringValue(OptionImpl<UnitTestLibrary> option) {
			return option.getValue().toString();
		}
	}

	//
	//
	//
	
	//
	// The PRODUCTION options used by the translator
	//
	// OK
	private final OptionImpl<Boolean> removeTempProject = new OptionImpl<Boolean>(
			"removeTempProject", null, false, OptionImpl.Status.PRODUCTION,
			new BooleanOptionBuilder(), new BooleanOptionEditor(), 
			"remove the temporary project");
	// OK
	private final OptionImpl<Boolean> refreshAndBuild = new OptionImpl<Boolean>(
			"refreshAndBuild", null, false, OptionImpl.Status.PRODUCTION,
			new BooleanOptionBuilder(), new BooleanOptionEditor(), 
			"Refresh and build besfore translation");
	// OK
	private final OptionImpl<Boolean> autoProperties = new OptionImpl<Boolean>(
			"autoProperties", null, false, OptionImpl.Status.PRODUCTION,
			new BooleanOptionBuilder(), new BooleanOptionEditor(),
			"auto translate get/set into .NET properties");
	// OK
	private final OptionImpl<Boolean> exactDirectoryName = new OptionImpl<Boolean>(
			"exactDirectoryName", null, false, OptionImpl.Status.PRODUCTION,
			new BooleanOptionBuilder(), new BooleanOptionEditor(), 
			"By default the generated directory name for the C# sources is the same as in Java (means for a package com.ilog, the created directory to put the csharp sources in will be com/ilog). Setting this option to true will generate the directory name that match exactly what you specified in the mapping file (if you choose to map com.ilog to Com.ILOG the created directory will be Com/ILOG/)");
	// OK
	private final OptionImpl<String> flatPattern = new OptionImpl<String>("flatPattern",
			null, "", OptionImpl.Status.PRODUCTION, new StringOptionBuilder(),
			new StringOptionEditor(), 
			"portion of the directory name structure to skip");
	// OK
	private final OptionImpl<Boolean> addNotBrowsableAttribute = new OptionImpl<Boolean>(
			"addNotBrowsableAttribute", null, false, OptionImpl.Status.PRODUCTION,
			new BooleanOptionBuilder(), new BooleanOptionEditor(), 
			"add [NotBrowsable] attribute to each member of generated classes");
	// OK
	private final OptionImpl<UnitTestLibrary> unitTestLibrary = new OptionImpl<UnitTestLibrary>(
			"unitTestLibrary", null, UnitTestLibrary.NONE,
			OptionImpl.Status.PRODUCTION, new UnitTestLibraryOptionBuilder(),
			new UnitTestLibraryOptionEditor(), "unit test library used");
	// OK
	private OptionImpl<String> suffixForGenerated = new OptionImpl<String>(
			"suffixForGenerated", null, null, OptionImpl.Status.PRODUCTION,
			new StringOptionBuilder(), new StringOptionEditor(), 
			"suffix to add to the filename for each translated class");
	// OK
	private final OptionImpl<Boolean> autoComputeDepends = new OptionImpl<Boolean>(
			"autoComputeDepends", null, false, OptionImpl.Status.PRODUCTION,
			new BooleanOptionBuilder(), new BooleanOptionEditor(), 
			"automatically compute dependences on other project in the workspace");
	// OK
	private final OptionImpl<Boolean> fillRawTypesUse = new OptionImpl<Boolean>(
			"fillRawTypesUse", null, true, OptionImpl.Status.PRODUCTION,
			new BooleanOptionBuilder(), new BooleanOptionEditor(), 
			"automaticcaly fill raw types names by infering generic arguments");
	// OK .. NAME
	private OptionImpl<Boolean> useGenerics = new OptionImpl<Boolean>("useGenerics",
			new String[] { "generics" }, false, OptionImpl.Status.PRODUCTION,
			new BooleanOptionBuilder(), new BooleanOptionEditor(), 
			"indicate that the translated use generics (translation is simpler if not)");
	// OK ... NAME
	private final OptionImpl<Boolean> autoCovariant = new OptionImpl<Boolean>(
			"autoCovariant", new String[] { "covariant" }, false,
			OptionImpl.Status.PRODUCTION, new BooleanOptionBuilder(),
			new BooleanOptionEditor(), "auto compute covariance (time consuming)");
	// OK ... NAME
	private final OptionImpl<Boolean> generateImplicitMappingfile = new OptionImpl<Boolean>(
			"generateImplicitMappingfile", new String[] { "generateconffile" },
			false, OptionImpl.Status.PRODUCTION, new BooleanOptionBuilder(),
			new BooleanOptionEditor());
	// OK
	private final OptionImpl<String> resourcesIncludePattern = new OptionImpl<String>(
			"resourcesIncludePattern", null, null, OptionImpl.Status.PRODUCTION,
			new StringOptionBuilder(), new StringOptionEditor(), 
			"pattern (rexgexp) of resources to include");
	// OK
	private final OptionImpl<String> resourcesExcludePattern = new OptionImpl<String>(
			"resourcesExcludePattern", null, null, OptionImpl.Status.PRODUCTION,
			new StringOptionBuilder(), new StringOptionEditor(), 
			"pattern (rexgexp) of resources to exclude");
	// OK
	private final OptionImpl<String> resourcesDestDir = new OptionImpl<String>(
			"resourcesOutputDir", null, null, OptionImpl.Status.PRODUCTION,
			new StringOptionBuilder(), new StringOptionEditor(), 
			"destination directory for ressources");
	// OK
	private final OptionImpl<ResourcesCopyPolicy> resourcesCopyPolicy = new OptionImpl<ResourcesCopyPolicy>(
			"resourcesCopyPolicy", null,
			ResourcesCopyPolicy.DONOTREPLACE_POLICY, OptionImpl.Status.PRODUCTION,
			new ResourcesCopyPolicyOptionBuilder(),
			new ResourcesCopyPolicyOptionEditor(), "resources copy policy");
	// OK
	private final OptionImpl<VSGenerationPolicy> vsGenerationMode = new OptionImpl<VSGenerationPolicy>(
			"vsGenerationMode", null, VSGenerationPolicy.NO_GENERATION,
			OptionImpl.Status.PRODUCTION, new VSGenerationPolicyOptionBuilder(),
			new VSGenerationPolicyOptionEditor(), "vsproject generation mode ()");
	// OK
	private final OptionImpl<VSProjectKind> vsProjectType = new OptionImpl<VSProjectKind>(
			"vsProjectType", null, VSProjectKind.CLASS_LIBRARY,
			OptionImpl.Status.PRODUCTION, new VSProjecKindOptionBuilder(),
			new VSProjectKindOptionEditor(), "indicate the vsproject kind ()");
	// OK
	private final OptionImpl<VSVersion> vsVersion = new OptionImpl<VSVersion>(
			"vsVersion", null, VSVersion.VS2005, OptionImpl.Status.PRODUCTION,
			new VSVersionOptionBuilder(), new VSVersionOptionEditor(), 
			"Version of Visual Studio to target");
	// OK
	private final OptionImpl<String> vsProjectEntryPoint = new OptionImpl<String>(
			"vsProjectEntryPoint", null, null, OptionImpl.Status.PRODUCTION,
			new StringOptionBuilder(), new StringOptionEditor(), 
			"Entry point (a fqn class name) for the executable VS project type");
	// OK
	private OptionImpl<String> vsProjectName = new OptionImpl<String>("vsProjectName",
			null, null, OptionImpl.Status.PRODUCTION, new StringOptionBuilder(),
			new StringOptionEditor(), "name of the visual studio project name");
	// NOP
	private OptionImpl<SourcesReplacementPolicy> sourcesReplacementPolicy = new OptionImpl<SourcesReplacementPolicy>(
			"sourcesReplacementPolicy", null, SourcesReplacementPolicy.REPLACE,
			OptionImpl.Status.PRODUCTION,
			new SourcesReplacementPolicyOptionBuilder(),
			new SourcesReplacementPolicyOptionEditor(), "sources replacement policy ()");
	// OK
	private final OptionImpl<String[]> package_filter = new OptionImpl<String[]>(
			"package_filter", null, null, OptionImpl.Status.PRODUCTION,
			new ArrayOfStringOptionBuilder(), new ArrayOfStringOptionEditor(), 
			"filter for packages to be included");
	// OK
	private final OptionImpl<String[]> class_filter = new OptionImpl<String[]>(
			"class_filter", null, null, OptionImpl.Status.PRODUCTION,
			new ArrayOfStringOptionBuilder(), new ArrayOfStringOptionEditor(), 
			"filter for class to be included");
	// OK
	private final OptionImpl<String> sourcesDestDir = new OptionImpl<String>(
			"sourcesOutputDir", new String[] { "output" }, "c:/Temp",
			OptionImpl.Status.PRODUCTION, new StringOptionBuilder(),
			new StringOptionEditor(), "You can use Eclipse or Environement Variable with ${My_VAR} : indicate where to produce c# sources files");
	// OK
	private OptionImpl<String> testsDestDir = new OptionImpl<String>("testsOutputDir",
			null, "c:/Temp", OptionImpl.Status.PRODUCTION,
			new StringOptionBuilder(), new StringOptionEditor(), 
			"destination directory for tests structure");			
	// OK
	private OptionImpl<String> projectName = new OptionImpl<String>("projectName",
			new String[] { "name" }, null, OptionImpl.Status.PRODUCTION,
			new StringOptionBuilder(), new StringOptionEditor(), 
			"name of the translator project");
	// OK
	private final OptionImpl<Boolean> createTranslatorConfigurationFiles = new OptionImpl<Boolean>(
			"createTranslatorConfigurationFiles",
			new String[] { "createFiles" }, false, OptionImpl.Status.PRODUCTION,
			new BooleanOptionBuilder(), new BooleanOptionEditor(), 
			"indicate if the translator can automatically create the (default) needed files for translation");
	// OK
	private final OptionImpl<Boolean> debug = new OptionImpl<Boolean>("debug", null,
			false, OptionImpl.Status.PRODUCTION, new BooleanOptionBuilder(),
			new BooleanOptionEditor(), "activate debugging");
	// OK
	private OptionImpl<Boolean> forceJUnitTest = new OptionImpl<Boolean>(
			"forceJUnitTest", null, false, OptionImpl.Status.PRODUCTION,
			new BooleanOptionBuilder(), new BooleanOptionEditor(), "force junit test (clarify)");
	// OK
	private OptionImpl<String> addPragmaForTest = new OptionImpl<String>(
			"addPragmaForTest", null, null, OptionImpl.Status.PRODUCTION,
			new StringOptionBuilder(), new StringOptionEditor(), 
			"name of the pragma (conditional compilation) to add in the begining of each test files");
	// OK
	private OptionImpl<String> tagToAddtoComment = new OptionImpl<String>(
			"tagToAddtoComment", null, null, OptionImpl.Status.PRODUCTION,
			new StringOptionBuilder(), new StringOptionEditor(), 
			"a tagname to add on each comment");
    // OK
	private final OptionImpl<PackageMappingPolicy> defaultPackageMappingBehavior = new OptionImpl<PackageMappingPolicy>(
			"defaultPackageMappingBehavior", null,
			PackageMappingPolicy.CAPITALIZED, OptionImpl.Status.PRODUCTION,
			new PackageMappingPolicyOptionBuilder(),
			new PackageMappingPolicyOptionEditor(), 
			"default package mapping behavior (capitalize or not)");
	// OK
	private final OptionImpl<MethodMappingPolicy> defaultMemberMappingBehavior = new OptionImpl<MethodMappingPolicy>(
			"defaultMemberMappingBehavior",
			new String[] { "defaultIdentifierMappingBehavior" },
			MethodMappingPolicy.CAPITALIZED, OptionImpl.Status.PRODUCTION,
			new MethodMappingPolicyOptionBuilder(),
			new MethodMappingPolicyOptionEditor(), 
			"default member mapping beahvior (capitalize or not)");
	// OK
	private final OptionImpl<Boolean> removeSerialVersionUIDFields = new OptionImpl<Boolean>(
			"removeSerialVersionUIDFields", null, false,
			OptionImpl.Status.PRODUCTION, new BooleanOptionBuilder(),
			new BooleanOptionEditor(), 
			"indicate if we need to remove serialeVersion fields");
	//
	// Beta options
	//	
	// OK
	private OptionImpl<Boolean> mavenStructure = new OptionImpl<Boolean>(
			"mavenStructure", null, false,
			OptionImpl.Status.BETA, new BooleanOptionBuilder(),
			new BooleanOptionEditor(), 
			"provided project follow the maven directory structure");
	// NOP
	private OptionImpl<Boolean> mapByteToSByte = new OptionImpl<Boolean>(
			"mapByteToSByte", null, false,
			OptionImpl.Status.BETA, new BooleanOptionBuilder(),
			new BooleanOptionEditor(), 
			"Indicate that (java) byte must be mapped into sbyte.");
	// NOP
	private final OptionImpl<String> profileForTranslation = new OptionImpl<String>(
			"profile", null, null, OptionImpl.Status.BETA,
			new StringOptionBuilder(), new StringOptionEditor(), 
			"customize the translation");
	// NOP
	private final OptionImpl<Boolean> useAlias = new OptionImpl<Boolean>(
			"useAlias", null, false, OptionImpl.Status.BETA,
			new BooleanOptionBuilder(), new BooleanOptionEditor(), 
			"allow import alias in generated sources");
	// NOP
	private final OptionImpl<Boolean> useGlobal = new OptionImpl<Boolean>(
			"useGlobal", null, false, OptionImpl.Status.BETA,
			new BooleanOptionBuilder(), new BooleanOptionEditor(), 
			"add global:: direcive in import");
	// NOP
	private final OptionImpl<Boolean> normalizeInterfaceName = new OptionImpl<Boolean>(
			"normalizeInterfaceName", null, false, OptionImpl.Status.BETA,
			new BooleanOptionBuilder(), new BooleanOptionEditor(), 
			"normalize interface name (add 'I') to the name");
	// NOP
	private final OptionImpl<String> tabValue = new OptionImpl<String>(
			"tabValue", null, "\t", OptionImpl.Status.BETA,
			new StringOptionBuilder(), new StringOptionEditor(), "tab value");

	private final OptionImpl<Boolean> useIsolationForMapping = new OptionImpl<Boolean>(
			"useIsolationForMapping", null, false, OptionImpl.Status.BETA,
			new BooleanOptionBuilder(), new BooleanOptionEditor());
	
	private final OptionImpl<Boolean> nullablePrimitiveTypes = new OptionImpl<Boolean>(
			"nullablePrimitiveTypes", null, false, OptionImpl.Status.BETA,
			new BooleanOptionBuilder(), new BooleanOptionEditor());
	
	// OK
	private OptionImpl<Boolean> useOnlyXML = new OptionImpl<Boolean>("useOnlyXML", null,
			false, OptionImpl.Status.BETA, new BooleanOptionBuilder(),
			new BooleanOptionEditor(), "only use new xml mapping files instead of text in case of both");
	
	// 
	// Experimental options
	//
	// OK
	private OptionImpl<Boolean> nullableEnum = new OptionImpl<Boolean>("nullableEnum",
			null, false, OptionImpl.Status.EXPERIMENTAL,
			new BooleanOptionBuilder(), new BooleanOptionEditor(), 
	"indicate if the translator treat enum as nullable");
	// OK
	private OptionImpl<Boolean> simulation = new OptionImpl<Boolean>("simulation",
			null, false, OptionImpl.Status.EXPERIMENTAL,
			new BooleanOptionBuilder(), new BooleanOptionEditor(), 
			"simulatation mode (clarify ....)");
	// OK
	private OptionImpl<Boolean> proxyMode = new OptionImpl<Boolean>("proxyMode", null,
			false, OptionImpl.Status.EXPERIMENTAL, new BooleanOptionBuilder(),
			new BooleanOptionEditor(), "only generate proxy for each method body");
	//
	private OptionImpl<Boolean> collectPublicDocumentedClasses = new OptionImpl<Boolean>("collectPublicDocumentedClasses",
			null, false, OptionImpl.Status.EXPERIMENTAL,
			new BooleanOptionBuilder(), new BooleanOptionEditor(), 
			"collect public documented classes");
	
	private OptionImpl<JDK> sourceJDK = new OptionImpl<JDK>("sourceJDK",
			null, JDK.JDK1_5, OptionImpl.Status.EXPERIMENTAL,
			new JDKOptionBuilder(), new JDKOptionEditor(), 
			"JDK source level");	


	private OptionImpl<DotNetFramework> targetDotNetFramework = new OptionImpl<DotNetFramework>("targetDotNetFramework",
			null, DotNetFramework.NET3_5, OptionImpl.Status.EXPERIMENTAL,
			new DotNetFrameworkOptionBuilder(), new DotNetFrameworkOptionEditor(), 
			"targeted .NET Framework");
	
	public DotNetFramework getTargetDotNetFramework() {
		return targetDotNetFramework.getValue();
	}

	public JDK getSourceJDK() {
		return sourceJDK.getValue();
	}
	
	//
	// Deprecated options
	//
	private OptionImpl<Boolean> generateVsProject = new OptionImpl<Boolean>(
			"vsProject", null, false, OptionImpl.Status.DEPCRECATED,
			new BooleanOptionBuilder(), new BooleanOptionEditor(), 
			"indicate if a vsproject file need to be generated");

	// Contains all the options : convenient way to list all available
	// properties
	private List<OptionImpl<?>> options = new ArrayList<OptionImpl<?>>();

	//
	//
	//
	private String projectConfigurationFileName;
	private boolean cmdLine = false;
	private GlobalOptions globalOptions = null;
	Collection<TranslatorDependency> referencedProjectsLocation = new ArrayList<TranslatorDependency>();
	private List<String> projectDependencies = null;

	//
	//
	//

	public TranslatorProjectOptions(String confFileName, GlobalOptions options) {
		this(options);
		projectConfigurationFileName = confFileName;
	}

	public TranslatorProjectOptions(GlobalOptions goptions) {
		globalOptions = goptions;
		//
		scanForFilters();
		//
		options.add(projectName);
		options.add(removeTempProject);
		options.add(refreshAndBuild);
		options.add(autoProperties);
		options.add(exactDirectoryName);
		options.add(flatPattern);
		options.add(addNotBrowsableAttribute);
		options.add(unitTestLibrary);
		options.add(suffixForGenerated);
		options.add(autoComputeDepends);
		options.add(fillRawTypesUse);
		options.add(useGenerics);
		options.add(autoCovariant);
		options.add(generateImplicitMappingfile);
		options.add(simulation);
		options.add(proxyMode);
		options.add(resourcesIncludePattern);
		options.add(resourcesExcludePattern);
		options.add(resourcesDestDir);
		options.add(resourcesCopyPolicy);
		options.add(generateVsProject);
		options.add(vsGenerationMode);
		options.add(vsProjectType);
		options.add(vsVersion);
		options.add(vsProjectEntryPoint);
		options.add(vsProjectName);
		options.add(sourcesReplacementPolicy);
		options.add(package_filter);
		options.add(class_filter);
		options.add(sourcesDestDir);
		options.add(testsDestDir);
		options.add(nullableEnum);
		options.add(createTranslatorConfigurationFiles);
		options.add(debug);
		options.add(forceJUnitTest);
		options.add(defaultMemberMappingBehavior);
		options.add(defaultPackageMappingBehavior);
		options.add(removeSerialVersionUIDFields);
		options.add(mavenStructure);
		options.add(mapByteToSByte);
		options.add(tagToAddtoComment);
		options.add(profileForTranslation);
		//
		options.add(useGlobal);
		options.add(useAlias);
		options.add(normalizeInterfaceName);
		options.add(tabValue);
		options.add(nullablePrimitiveTypes);
		options.add(useIsolationForMapping);
		options.add(useOnlyXML);		
		//
		options.add(addPragmaForTest);
		options.add(collectPublicDocumentedClasses);
		options.add(sourceJDK);
		options.add(targetDotNetFramework);
	}
	
	//
	//
	//
	
	private void scanForFilters() {
		String[] pckFilters   = globalOptions.getPackageFilters();
		String[] classFilters = globalOptions.getClassFilters();
		
		if (pckFilters.length > 0)
			package_filter.setValue(pckFilters);
		if (classFilters.length > 0)
			class_filter.setValue(classFilters);		
	}
	
	//
	//
	//

	@Override
	public void finalize() {
		options = null;
		globalOptions = null;
		referencedProjectsLocation = null;
		projectDependencies = null;
	}
	
	//
	// findOption
	//

	public OptionImpl<?> findOption(String name) {
		for (final OptionImpl<?> opt : options) {
			if (opt.getName().equals(name))
				return opt;
		}
		return null;
	}

	//
	// Command Line version
	//

	public boolean isCommandLineVersion() {
		return cmdLine;
	}

	public void setCommandLineVersion(boolean maven) {
		cmdLine = maven;
	}

	//
	// -------------------------
	//

	//
	// Auto properties
	//

	public OptionImpl<Boolean> getAutoProperties() {
		return autoProperties;
	}

	public boolean isAutoProperties() {
		return autoProperties.getValue();
	}

	public void setAutoProperties(boolean autoProperties) {
		this.autoProperties.setValue(autoProperties);
	}

	//
	// Use alias in using
	//

	public OptionImpl<Boolean> getUseAlias() {
		return useAlias;
	}

	public boolean isUseAlias() {
		return useAlias.getValue();
	}

	public void setUseAlias(boolean autoProperties) {
		this.useAlias.setValue(autoProperties);
	}
	
	//
	// Use global in using
	//

	public OptionImpl<Boolean> getUseGlobal() {
		return useGlobal;
	}

	public boolean isUseGlobal() {
		return useGlobal.getValue();
	}

	public void setUseGlobal(boolean autoProperties) {
		this.useGlobal.setValue(autoProperties);
	}
	
	//
	// Normalize interface name (i.e. add "I")
	//

	public OptionImpl<Boolean> getNormalizeInterfaceName() {
		return normalizeInterfaceName;
	}

	public boolean isNormalizeInterfaceName() {
		return normalizeInterfaceName.getValue();
	}

	public void setNormalizeInterfaceName(boolean autoProperties) {
		this.normalizeInterfaceName.setValue(autoProperties);
	}

	//
	// profile for translation
	//

	public OptionImpl<String> getTabValueOption() {
		return tabValue;
	}

	public String getTabValue() {
		return tabValue.getValue();
	}

	public void setTabValue(String profile) {
		tabValue.setValue(profile);
	}
	
	//
	// profile for translation
	//

	public OptionImpl<String> getProfileForTranslation() {
		return profileForTranslation;
	}

	public String getProfile() {
		return profileForTranslation.getValue();
	}

	public void setProfileForTranslation(String profile) {
		profileForTranslation.setValue(profile);
	}

	//
	// Generate implicit mapping file
	//

	public OptionImpl<Boolean> getGenerateImplicitMappingfile() {
		return generateImplicitMappingfile;
	}

	public boolean isGenerateImplicitMappingFile() {
		return generateImplicitMappingfile.getValue();
	}

	public void setGenerateImplicitMappingFile(
			boolean generateImplicitMappingfile) {
		this.generateImplicitMappingfile.setValue(generateImplicitMappingfile);
	}

	//
	// Auto covariant
	//

	public OptionImpl<Boolean> getAutoCovariant() {
		return autoCovariant;
	}

	public boolean isAutoCovariant() {
		return autoCovariant.getValue();
	}

	public void setAutoCovariant(boolean covariant) {
		autoCovariant.setValue(covariant);
	}

	//
	// Creates translator files
	//

	public OptionImpl<Boolean> getCreateTranslatorConfigurationFiles() {
		return createTranslatorConfigurationFiles;
	}

	public boolean isCreateTranslatorConfigurationFiles() {
		return createTranslatorConfigurationFiles.getValue();
	}

	public void setCreateTranslatorConfigurationFiles(boolean createFiles) {
		createTranslatorConfigurationFiles.setValue(createFiles);
	}

	//
	// use generics
	//

	public OptionImpl<Boolean> getUseGenerics() {
		return useGenerics;
	}

	public boolean isUseGenerics() {
		return useGenerics.getValue();
	}

	public void setUseGenerics(boolean useGenerics) {
		this.useGenerics.setValue(useGenerics);
	}

	//
	// project dependencies
	//

	public List<String> getProjectDependencies() {
		return projectDependencies;
	}

	public void setProjectDependencies(List<String> projectDependencies) {
		this.projectDependencies = projectDependencies;
	}

	//
	// class filter
	//


	public String[] getClassFilter() {
		return class_filter.getValue();
	}

	public void setClassFilter(String[] class_filter) {
		this.class_filter.setValue(class_filter);
	}

	//
	// package filter
	//

	public OptionImpl<String[]> getPackage_filter() {
		return package_filter;
	}

	public String[] getPackageFilter() {
		return package_filter.getValue();
	}

	public void setPackageFilter(String[] package_filter) {
		this.package_filter.setValue(package_filter);
	}

	//
	// relative dest dir
	//

	public OptionImpl<String> getSourcesDestDirOption() {
		return sourcesDestDir;
	}

	public String getSourcesDestDir() {
		return replaceVariables(sourcesDestDir.getValue());
	}

	public String getRealSourcesDestDir() {
		return sourcesDestDir.getValue();
	}

	public void setSourcesDestDir(String sourcesDestDir) {
		this.sourcesDestDir.setValue(sourcesDestDir);
	}

	//
	// tests output dest dir
	//

	public OptionImpl<String> getTestsDestDirOption() {
		return testsDestDir;
	}

	public String getTestsDestDir() {
		return replaceVariables(testsDestDir.getValue());
	}

	public void setTestsDestDir(String testsDestDir) {
		this.testsDestDir.setValue(testsDestDir);
	}

	//
	// simulation
	//

	public OptionImpl<Boolean> getSimulation() {
		return simulation;
	}

	public boolean isSimulation() {
		return simulation.getValue();
	}

	public void setSimulation(boolean simulation) {
		this.simulation.setValue(simulation);
	}

	//
	// Maven Structure
	//

	public OptionImpl<Boolean> getMavenStructure() {
		return mavenStructure;
	}

	public boolean isMavenStructure() {
		return mavenStructure.getValue();
	}

	public void setMavenStructure(boolean simulation) {
		mavenStructure.setValue(simulation);
	}

	//
	// project name
	//

	public String getProjectName() {
		return projectName.getValue();
	}

	public void setProjectName(String projectName) {
		this.projectName.setValue(projectName);
	}

	//
	// not browsable attribute
	//

	public OptionImpl<Boolean> getAddNotBrowsableAttribute() {
		return addNotBrowsableAttribute;
	}

	public boolean isAddNotBrowsableAttribute() {
		return addNotBrowsableAttribute.getValue();
	}

	public void setAddNotBrowsableAttribute(boolean addNotBrowsableAttribute) {
		this.addNotBrowsableAttribute.setValue(addNotBrowsableAttribute);
	}

	//
	// referenced projects location
	//

	public Collection<TranslatorDependency> getReferencedProjectsLocation() {
		return referencedProjectsLocation;
	}

	//
	// force junit test (to be removed)
	//

	public OptionImpl<Boolean> getForceJUnitTest() {
		return forceJUnitTest;
	}

	public boolean isForceJUnitTest() {
		return forceJUnitTest.getValue();
	}

	//
	// proxy mode
	//

	public OptionImpl<Boolean> getProxyMode() {
		return proxyMode;
	}

	public boolean isProxyMode() {
		return proxyMode.getValue();
	}

	public void setProxyMode(boolean proxyMode) {
		this.proxyMode.setValue(proxyMode);
	}

	//
	// fill raw type use
	//

	public OptionImpl<Boolean> getFillRawTypesUse() {
		return fillRawTypesUse;
	}

	public boolean isFillRawTypesUse() {
		return fillRawTypesUse.getValue();
	}

	public void setFillRawTypesUse(boolean fillRawTypesUse) {
		this.fillRawTypesUse.setValue(fillRawTypesUse);
	}

	//
	// unit test library
	//

	public OptionImpl<UnitTestLibrary> getUnitTestLibraryOption() {
		return unitTestLibrary;
	}

	public UnitTestLibrary getUnitTestLibrary() {
		return unitTestLibrary.getValue();
	}

	public void setUnitTestLibrary(UnitTestLibrary unitTestLibrary) {
		this.unitTestLibrary.setValue(unitTestLibrary);
	}

	//
	// exact directory name
	//

	public OptionImpl<Boolean> getExactDirectoryName() {
		return exactDirectoryName;
	}

	public boolean isExactDirectoryName() {
		return exactDirectoryName.getValue();
	}

	public void setExactDirectoryName(boolean val) {
		exactDirectoryName.setValue(val);
	}

	//
	// Suffix for generated
	//

	public OptionImpl<String> getSuffixForGeneratedOption() {
		return suffixForGenerated;
	}

	public String getSuffixForGenerated() {
		return suffixForGenerated.getValue();
	}

	public void setSuffixForGenerated(String suffix_for_generated) {
		suffixForGenerated.setValue(suffix_for_generated);
	}

	//
	//   RemoveTempProject
	//

	public OptionImpl<Boolean> getRemoveTempProjectOption() {
		return removeTempProject;
	}

	public boolean getRemoveTempProject() {
		return removeTempProject.getValue();
	}

	//
	// RefreshAndBuild
	//

	public OptionImpl<Boolean> getRefreshAndBuildOption() {
		return refreshAndBuild;
	}

	public boolean getRefreshAndBuild() {
		return refreshAndBuild.getValue();
	}

	public void setRefreshAndBuild(boolean clean) {
		refreshAndBuild.setValue(clean);
	}

	//
	// Flat pattern
	//

	public OptionImpl<String> getFlatPatternOption() {
		return flatPattern;
	}

	public String getFlatPattern() {
		return flatPattern.getValue();
	}

	public void setFlatPattern(String flatPattern) {
		this.flatPattern.setValue(flatPattern);
	}

	//
	// KeepWorkingProject
	//

	public void setKeepWorkingProject(boolean keepWorkingProject) {
		removeTempProject.setValue(keepWorkingProject);
	}

	//
	// NullableEnum
	//

	public OptionImpl<Boolean> getNullableEnum() {
		return nullableEnum;
	}

	public boolean isNullableEnum() {
		return nullableEnum.getValue();
	}

	//
	// Generate VS project files
	//

	public OptionImpl<VSProjectKind> getVsProjectType() {
		return vsProjectType;
	}

	public VSProjectKind getVSProjectType() {
		return vsProjectType.getValue();
	}

	public void setVSProjectType(VSProjectKind kind) {
		vsProjectType.setValue(kind);
	}

	//
	// VsProjectEntryPoint
	//

	public OptionImpl<String> getVsProjectEntryPoint() {
		return vsProjectEntryPoint;
	}

	public String getVSProjectEntryPoint() {
		return vsProjectEntryPoint.getValue();
	}

	public void setVSProjectEntryPoint(String fqnClassName) {
		vsProjectEntryPoint.setValue(fqnClassName);
	}

	//
	// VsGenerationMode
	//

	public OptionImpl<VSGenerationPolicy> getVsGenerationMode() {
		return vsGenerationMode;
	}

	public VSGenerationPolicy getVSGenerationMode() {
		return vsGenerationMode.getValue();
	}

	public void setVSGenerationMode(VSGenerationPolicy mode) {
		vsGenerationMode.setValue(mode);
	}

	//
	// VsProjectName
	//
	public OptionImpl<String> getVsProjectName() {
		return vsProjectName;
	}

	public String getVSProjectName() {
		return replaceVariables(vsProjectName.getValue());
	}

	public void setVSProjectName(String vsprojname_s) {
		vsProjectName.setValue(vsprojname_s);
	}

	//
	// VsVersionOption
	//

	public OptionImpl<VSVersion> getVsVersionOption() {
		return vsVersion;
	}

	public VSVersion getVsVersion() {
		return vsVersion.getValue();
	}

	public void setVsVersion(VSVersion vsVersion) {
		this.vsVersion.setValue(vsVersion);
	}

	//
	// Resources include pattern
	//

	public OptionImpl<String> getResourcesIncludePatternOption() {
		return resourcesIncludePattern;
	}

	public String getResourcesIncludePattern() {
		return resourcesIncludePattern.getValue();
	}

	public void setResourcesIncludePattern(String resourcesIncludePattern) {
		this.resourcesIncludePattern.setValue(resourcesIncludePattern);
	}

	//
	// Resources exclude pattern
	//

	public OptionImpl<String> getResourcesExcludePatternOption() {
		return resourcesExcludePattern;
	}

	public String getResourcesExcludePattern() {
		return resourcesExcludePattern.getValue();
	}

	public void setResourcesExcludePattern(String resourcesExcludePattern) {
		this.resourcesExcludePattern.setValue(resourcesExcludePattern);
	}

	//
	// Resources dest dir
	//

	public OptionImpl<String> getResourcesDestDirOption() {
		return resourcesDestDir;
	}

	public String getResourcesDestDir() {
		return replaceVariables(resourcesDestDir.getValue());
	}

	public void setResourcesDestDir(String resourcesOutputDir) {
		resourcesDestDir.setValue(resourcesOutputDir);
	}

	//
	// Resources copy policy
	//

	public OptionImpl<ResourcesCopyPolicy> getResourcesCopyPolicyOption() {
		return resourcesCopyPolicy;
	}

	public ResourcesCopyPolicy getResourcesCopyPolicy() {
		return resourcesCopyPolicy.getValue();
	}

	public void SetResourcesCopyPolicy(ResourcesCopyPolicy policy) {
		resourcesCopyPolicy.setValue(policy);
	}

	//
	// ProjectConfigurationFileName
	//

	public String getProjectConfigurationFileName() {
		return projectConfigurationFileName;
	}

	public void setProjectConfigurationFileName(String confFileName) {
		projectConfigurationFileName = confFileName;
	}

	//
	// GlobalOptions
	//

	public GlobalOptions getGlobalOptions() {
		return globalOptions;
	}

	public void setGlobalOptions(GlobalOptions globalOptions) {
		this.globalOptions = globalOptions;
	}

	//
	// AutoComputeDepends
	//

	public OptionImpl<Boolean> getAutoComputeDepends() {
		return autoComputeDepends;
	}

	public boolean isAutoComputeDepends() {
		return autoComputeDepends.getValue();
	}

	public void setAutoComputeDepends(boolean autoComputeDepends) {
		this.autoComputeDepends.setValue(autoComputeDepends);
	}

	//
	// DefaultMemberMappingBehavior
	//

	public OptionImpl<MethodMappingPolicy> getDefaultMemberMappingBehavior() {
		return defaultMemberMappingBehavior;
	}

	public MethodMappingPolicy getMethodMappingPolicy() {
		return defaultMemberMappingBehavior.getValue();
	}

	public void setMethodMappingPolicy(MethodMappingPolicy replacement) {
		defaultMemberMappingBehavior.setValue(replacement);
	}

	//
	// DefaultPackageMappingBehavior
	//

	public OptionImpl<PackageMappingPolicy> getDefaultPackageMappingBehavior() {
		return defaultPackageMappingBehavior;
	}

	public PackageMappingPolicy getPackageMappingPolicy() {
		return defaultPackageMappingBehavior.getValue();
	}

	public void setPackageMappingPolicy(PackageMappingPolicy replacement) {
		defaultPackageMappingBehavior.setValue(replacement);
	}

	//
	// SourcesReplacementPolicy
	//

	public OptionImpl<SourcesReplacementPolicy> getSourcesReplacementPolicyOption() {
		return sourcesReplacementPolicy;
	}

	public SourcesReplacementPolicy getSourcesReplacementPolicy() {
		return sourcesReplacementPolicy.getValue();
	}

	public void setSourcesReplacementPolicy(SourcesReplacementPolicy replacement) {
		sourcesReplacementPolicy.setValue(replacement);
	}

	//
	// removeSerialVersionUIDFields
	//

	public OptionImpl<Boolean> getRemoveSerialVersionUIDFields() {
		return removeSerialVersionUIDFields;
	}

	public boolean isRemoveSerialVersionUIDFields() {
		return removeSerialVersionUIDFields.getValue();
	}

	public void setRemoveSerialVersionUIDFields(
			boolean removeSerialVersionUIDFields) {
		this.removeSerialVersionUIDFields
				.setValue(removeSerialVersionUIDFields);
	}

	//
	// log translation process debug information
	//
	public OptionImpl<Boolean> getDebug() {
		return debug;
	}

	//
	// Generate Visual Studio Project file
	//
	public OptionImpl<Boolean> getGenerateVsProject() {
		return generateVsProject;
	}

	//
	// byte mapping
	//

	public OptionImpl<Boolean> getMapByteToSByte() {
		return mapByteToSByte;
	}

	public boolean isMapByteToSByte() {
		return mapByteToSByte.getValue();
	}

	public void setMapByteToSByte(boolean covariant) {
		mapByteToSByte.setValue(covariant);
	}

	//
	// tagToAddtoComment
	//

	public OptionImpl<String> getTagToAddtoComment() {
		return tagToAddtoComment;
	}

	public void setTagToAddtoComment(String covariant) {
		tagToAddtoComment.setValue(covariant);
	}
	
	//
	// tagToAddtoComment
	//

	public OptionImpl<Boolean> getUseOnlyXMLOption() {
		return useOnlyXML;
	}
	
	public boolean isUseOnlyXML() {
		return useOnlyXML.getValue();
	}

	public void setUseOnlyXMLOption(Boolean covariant) {
		useOnlyXML.setValue(covariant);
	}

	//
	// -----------------------------------------------------------------------------------------
	//

	//
	// Save
	//

	public void save() throws IOException {
		final FileWriter writer = new FileWriter(projectConfigurationFileName);
		final List<OptionImpl<?>> eligibleOptions = new ArrayList<OptionImpl<?>>();
		for (final OptionImpl<?> opt : options) {
			if (!opt.isDefaultValue())
				eligibleOptions.add(opt);
		}
		writer.append("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
		writer.append("<translation ");
		boolean begin = true;
		for (int i = 0; i < eligibleOptions.size(); i++) {
			final OptionImpl<?> option = eligibleOptions.get(i);
			if (begin)
				begin = !begin;
			else
				writer.append("             ");
			writer.append(option.getName() + "=\"" + option.getStringValue()
					+ "\"");
			if (i < eligibleOptions.size() - 1)
				writer.append("\n");

		}
		writer.append(">\n");
		for (final TranslatorDependency ref : referencedProjectsLocation) {
			writer.append("   <depends ");
			switch (ref.getKind()) {
			case PROJECT:
				writer.append("project=");
				break;
			case JAR:
				writer.append("jar=");
				break;
			case DIRECTORY:
				writer.append("dir=");
				break;
			}
			writer.append("\"" + ref.getName() + "\"/>\n");
		}
		writer.append("</translation>\n");
		writer.flush();
		writer.close();
	}

	//
	// Read
	//	

	private String getStringAttributeValue(Element element, String name,
			String defaultValue) {
		final String value = element.getAttribute(name);
		if (value != null && !value.equals("")) {
			return value;
		} else
			return defaultValue;
	}

	private <T> void scanForAttributeValue(Element element, OptionImpl<T> option) {
		String value = element.getAttribute(option.getName());
		if (value != null && !value.equals("")) {
			if (option.getStatus() != OptionImpl.Status.PRODUCTION) {
				globalOptions.getLogger().logWarning(
						"<" + option.getName() + "> usage of an "
								+ option.getStatus() + " option.");
			}
			option.parse(value);
		} else if (option.getSynonymes() != null) {
			for (final String alternate : option.getSynonymes()) {
				value = element.getAttribute(alternate);
				if (value != null && !value.equals("")) {
					globalOptions.getLogger().logWarning(
							"<" + alternate + "> is a deprecated name use <"
									+ option.getName() + "> instead.");
					option.parse(value);
					return;
				}
			}
		}
	}

	//
	// read
	//

	public boolean read(IJavaProject jProject, boolean readDependance)
			throws ParserConfigurationException, SAXException,
			JavaModelException, IOException {
		readDependance = true; //

		if (projectConfigurationFileName == null)
			return false;

		final DocumentBuilderFactory docBuilderFactory = DocumentBuilderFactory
				.newInstance();
		final DocumentBuilder docBuilder = docBuilderFactory
				.newDocumentBuilder();
		final Document doc = docBuilder.parse(new File(
				projectConfigurationFileName));

		final Element root = doc.getDocumentElement();

		scanForAttributeValue(root, debug);

		// normalize text representation doc.getDocumentElement ().normalize ();
		if (debug.getValue()) {
			globalOptions.setDebug(true);
			globalOptions.getLogger().logInfo(
					"Root element of the doc is " + root.getNodeName());
		}

		scanForAttributeValue(root, package_filter);
		scanForAttributeValue(root, class_filter);
		scanForAttributeValue(root, sourcesDestDir);
		scanForAttributeValue(root, testsDestDir);
		if (testsDestDir.isDefaultValue())
			testsDestDir = sourcesDestDir;
		scanForAttributeValue(root, projectName);
		scanForAttributeValue(root, flatPattern);
		scanForAttributeValue(root, suffixForGenerated);
		scanForAttributeValue(root, simulation);
		scanForAttributeValue(root, generateImplicitMappingfile);
		scanForAttributeValue(root, useGenerics);
		scanForAttributeValue(root, removeTempProject);
		scanForAttributeValue(root, refreshAndBuild);
		scanForAttributeValue(root, nullableEnum);
		scanForAttributeValue(root, proxyMode);
		scanForAttributeValue(root, autoProperties);
		scanForAttributeValue(root, exactDirectoryName);
		scanForAttributeValue(root, autoCovariant);
		scanForAttributeValue(root, fillRawTypesUse);
		scanForAttributeValue(root, addNotBrowsableAttribute);
		scanForAttributeValue(root, autoComputeDepends);
		scanForAttributeValue(root, resourcesIncludePattern);
		scanForAttributeValue(root, resourcesExcludePattern);
		scanForAttributeValue(root, resourcesDestDir);
		scanForAttributeValue(root, resourcesCopyPolicy);
		scanForAttributeValue(root, unitTestLibrary);
		scanForAttributeValue(root, vsGenerationMode);
		scanForAttributeValue(root, vsProjectName);
		scanForAttributeValue(root, vsGenerationMode);
		scanForAttributeValue(root, vsProjectType);
		scanForAttributeValue(root, vsProjectEntryPoint);
		scanForAttributeValue(root, vsVersion);
		scanForAttributeValue(root, sourcesReplacementPolicy);
		scanForAttributeValue(root, defaultMemberMappingBehavior);
		scanForAttributeValue(root, defaultPackageMappingBehavior);
		scanForAttributeValue(root, removeSerialVersionUIDFields);
		scanForAttributeValue(root, mavenStructure);
		scanForAttributeValue(root, mapByteToSByte);
		scanForAttributeValue(root, nullablePrimitiveTypes);
		scanForAttributeValue(root, useIsolationForMapping);
		scanForAttributeValue(root, useOnlyXML);
		//
		scanForAttributeValue(root, addPragmaForTest);
		scanForAttributeValue(root, collectPublicDocumentedClasses);
		scanForAttributeValue(root, targetDotNetFramework);
		scanForAttributeValue(root, sourceJDK);
		scanForAttributeValue(root, tagToAddtoComment);
		scanForAttributeValue(root, profileForTranslation);
		scanForAttributeValue(root, useAlias);
		scanForAttributeValue(root, useGlobal);
		scanForAttributeValue(root, normalizeInterfaceName);
		scanForAttributeValue(root, tabValue);
		//
		globalOptions.setLogFile(replaceVariables(getStringAttributeValue(root,
				"logFile", globalOptions.getLogFile())));
		final String mappingAssemblyLocation_s = root
				.getAttribute("mappingAssemblyLocation");
		if (mappingAssemblyLocation_s != null
				&& !mappingAssemblyLocation_s.equals("")) {
			globalOptions.setMappingAssemblyLocation(mappingAssemblyLocation_s);
		} else {
			globalOptions
					.setMappingAssemblyLocation(replaceVariables(globalOptions
							.getMappingAssemblyLocation())); // new
		}

		//

		final NodeList listOfDependence = doc.getElementsByTagName("depends");
		final int totalDependance = listOfDependence.getLength();
		if (debug.getValue()) {
			globalOptions.getLogger().logInfo(
					"Total no of dependance : " + totalDependance);
		}

		if (readDependance) {
			referencedProjectsLocation.clear();
			if (autoComputeDepends.getValue())
				readDependance(jProject);
			else
				for (int s = 0; s < totalDependance; s++) {
					final Node firstPassNode = listOfDependence.item(s);
					if (firstPassNode.getNodeType() == Node.ELEMENT_NODE) {
						final Element firstPassElement = (Element) firstPassNode;
						TranslatorDependency referenceLoc = null;

						final String profile = firstPassElement
								.getAttribute("profile");

						final String location = firstPassElement
								.getAttribute("project");
						if (location != null && !location.equals("")) {
							final IProject reference = jProject.getProject()
									.getWorkspace().getRoot().getProject(
											location);
							referenceLoc = new TranslatorDependency(reference.getName(),
									TranslatorDependency.Kind.PROJECT, reference
											.getLocation().toFile()
											.getAbsolutePath(), JavaCore.create(reference),
									profile);
						}

						final String jar = firstPassElement.getAttribute("jar");
						if (jar != null && !jar.equals("")) {
							final int fsi = jar.lastIndexOf(File.separator);
							final int ji = jar.indexOf("jar");
							final String name = jar.substring(fsi, ji);
							referenceLoc = new TranslatorDependency(name,
									TranslatorDependency.Kind.JAR, jar, null, profile);
						}

						final String dir = firstPassElement.getAttribute("dir");
						if (dir != null && !dir.equals("")) {
							referenceLoc = new TranslatorDependency(dir,
									TranslatorDependency.Kind.DIRECTORY, dir, null,
									profile);
						}

						if (debug.getValue()) {
							globalOptions.getLogger().logInfo(
									"Depends on " + location + " ("
											+ referenceLoc + ")");
						}

						if (referenceLoc != null)
							referencedProjectsLocation.add(referenceLoc);
					}
				}
		}

		return true;
	}

	//
	// readDependance
	//

	private void readDependance(IJavaProject project) throws JavaModelException {
		final List<TranslatorDependency> dependencies = getListOfTranslatorDependence(project);
		final DependenciesGraph<TranslatorDependency, List<TranslatorDependency>> depGraph = new DependenciesGraph<TranslatorDependency, List<TranslatorDependency>>();
		for (final TranslatorDependency dep : dependencies) {
			final List<TranslatorDependency> currentDependencies = getListOfTranslatorDependence(dep
					.getReference());
			depGraph.add(dep, currentDependencies);
		}
		depGraph.propagate();
		final Collection<TranslatorDependency> orderedDependencies = depGraph
				.getOrderedDependencies();
		//
		for (final TranslatorDependency dep : orderedDependencies) {
			/*final TranslatorDependency referenceLoc = new TranslatorDependency(dep.getJProject()
					.getProject().getName(), TranslatorDependency.Kind.PROJECT, dep
					.getJProject().getProject().getLocation().toFile()
					.getAbsolutePath(), dep.getJProject().getProject(), dep
					.getProfile());*/
			//			
			if (debug.getValue()) {
				globalOptions.getLogger().logInfo(
						"Depends on " + dep.getReference().getElementName()
								+ " (" + dep + ")");
			}

			referencedProjectsLocation.add(dep);
		}
	}

	private List<TranslatorDependency> getListOfTranslatorDependence(
			IJavaProject project) throws JavaModelException {
		final List<TranslatorDependency> dependencies = new ArrayList<TranslatorDependency>();
		final String[] projectNames = project.getRequiredProjectNames();
		for (final String pName : projectNames) {
			final IProject refProject = project.getProject().getWorkspace()
					.getRoot().getProject(pName);
			if (refProject.exists()) {
				final IJavaProject refJProject = JavaCore.create(refProject);
				if (refJProject.exists()
						&& !refJProject.getElementName().equals(
								project.getElementName())) {
					final String translatorDir = TranslatorProjectOptions
							.searchTranslatorDir(refJProject, globalOptions,
									false, false, null);
					if (translatorDir != null) {
						final IProject reference = refJProject.getProject()
						.getWorkspace().getRoot().getProject(refJProject.getElementName());									
						final TranslatorDependency dep = new TranslatorDependency(
								refJProject.getElementName(), 
								TranslatorDependency.Kind.PROJECT, 
								reference.getLocation().toFile().getAbsolutePath(), 
								JavaCore.create(reference),
								null);
						dependencies.add(dep);
					}
				}
			}
		}
		return dependencies;
	}

	/**
	 * An utility class use to ordered dependency
	 * 
	 * @author afau
	 * 
	 * @param <T>
	 *            The node type of the graph
	 * @param <U>
	 *            The list of connected node (the dependencies), a collection of
	 *            T
	 */
	public static class DependenciesGraph<T extends Comparable<T>, U extends Collection<T>> {

		Map<String, T> keyMap = new HashMap<String, T>();

		Map<String, U> map = new HashMap<String, U>();

		Map<Integer, U> ponderedMap = new HashMap<Integer, U>();

		Collection<T> orderedMap = new ArrayList<T>();

		Map<String, TranslatorDependency> modules = null;

		public DependenciesGraph() {
			this.modules = new HashMap<String, TranslatorDependency>();
		}

		public DependenciesGraph(Map<String, TranslatorDependency> modules) {
			this.modules = modules;
		}

		public boolean contains(T node) {
			final TranslatorDependency tDep = modules.get(node.toString());
			// getLog().info(" node " + node.toString() + " in module = " +
			// tDep);
			if (tDep != null && tDep.getKind() != TranslatorDependency.Kind.JAR) {
				return keyMap.containsKey(node.toString());
			} else {
				// getLog().info(node.toString() + " is a JAR, add it without
				// dependency !");
				map.put(node.toString(), null);
				keyMap.put(node.toString(), node);
				return true;
			}
		}

		public void add(T node, U dependencies) {
			if (keyMap.containsKey(node.toString())) {
				map.get(node.toString()).addAll(dependencies);
			} else {
				map.put(node.toString(), dependencies);
				keyMap.put(node.toString(), node);
			}
		}

		@SuppressWarnings("unchecked")
		public void propagate() {
			final Iterator<Map.Entry<String, U>> it = map.entrySet().iterator();
			while (it.hasNext()) {
				final Map.Entry<String, U> entry = (Map.Entry<String, U>) it
						.next();
				int score = 0;
				// getLog().info(" key " + entry.getKey());
				if (entry.getValue() != null)
					for (final T dep : entry.getValue()) {
						final U deps = map.get(dep);
						if (deps != null) {
							score += deps.size();
						}
						score += 1;
					}
				U keys = ponderedMap.get(score);
				if (keys == null) {
					keys = (U) new ArrayList<T>();
				}
				keys.add(keyMap.get(entry.getKey()));
				ponderedMap.put(score, keys);
			}
			//	    	
			final Set<Integer> set = new TreeSet<Integer>();
			set.addAll(ponderedMap.keySet());
			final Iterator<Integer> it2 = set.iterator();
			while (it2.hasNext()) {
				final Integer score = it2.next();
				final U keys = ponderedMap.get(score);
				for (final T key : keys) {
					orderedMap.add(key);
				}
			}
		}

		public Collection<T> getOrderedDependencies() {
			return orderedMap;
		}
	}

	protected String replaceVariables(String value) {
		if (value == null)
			return null;
		final List<String> variables = extractVariables(value);
		String newValue = value;
		for (final String var : variables) {
			final String newVar = getVariableValues(var);
			if (newVar != null)
				newValue = newValue.replace("${" + var + "}", newVar);
		}
		return newValue;
	}

	private String getVariableValues(String var) {
		final IPath path = JavaCore.getClasspathVariable(var);
		if (path == null && var.equals("ROOT")) {
			return globalOptions.getBaseDestDir();
		}
		if (path != null)
			return path.toString();
		return System.getenv(var);
	}

	private List<String> extractVariables(String value) {
		final List<String> res = new ArrayList<String>();
		final String[] val = value.split("\\$\\{");
		for (final String v : val) {
			if (!v.equals("") && v.contains("}")) {
				res.add(v.substring(0, v.indexOf("}")));
			}
		}
		return res;
	}

	//
	// populate dependency
	//

	public void populateDependency(Logger logger, IJavaProject originalProject) {
		for (int s = 0; s < getProjectDependencies().size(); s++) {
			final String dependency = getProjectDependencies().get(s);
			TranslatorDependency referenceLoc = null;
			if (dependency.endsWith(".jar")) {
				final int fsi = dependency.lastIndexOf(File.separator);
				final int ji = dependency.indexOf("jar");
				final String name = dependency.substring(fsi, ji);
				referenceLoc = new TranslatorDependency(name, TranslatorDependency.Kind.JAR,
						dependency, null, null);
			} else {
				final IProject reference = originalProject.getProject()
						.getWorkspace().getRoot().getProject(dependency);
				referenceLoc = new TranslatorDependency(reference.getName(),
						TranslatorDependency.Kind.PROJECT, reference.getLocation()
								.toFile().getAbsolutePath(),  JavaCore.create(reference), null);
			}

			if (globalOptions.isDebug()) {
				logger.logInfo("Depends on " + dependency + " (" + referenceLoc
						+ ")");
			}

			if (referenceLoc != null)
				referencedProjectsLocation.add(referenceLoc);
		}
	}

	//
	//
	// createTranslatorFiles

	public static String createTranslatorFiles(IJavaProject javaProject,
			GlobalOptions globalOptions, String translatorDirectory,
			String confFileName) throws ParserConfigurationException,
			SAXException, JavaModelException, IOException, CoreException {
		final TranslatorProjectOptions options = new TranslatorProjectOptions(
				globalOptions);
		options.setProjectConfigurationFileName(confFileName);
		// options.read(javaProject, false);
		final TranslationConfiguration configuration = new TranslationConfiguration(
				javaProject, javaProject, options);
		return configuration.createAllConfigurationFiles(javaProject);
	}

	//
	// searchTranslatorDir
	//
	public static String searchTranslatorDir(IJavaProject jproject,
			GlobalOptions globalOptions, boolean reportError, boolean create,
			String profile) {
		if (jproject.getProject() == null
				|| jproject.getProject().getLocation() == null)
			globalOptions.getLogger().logInfo("Can't find project " + jproject);

		if (profile != null && !profile.equals("")) {
			final String translatorDirectory = searchTranslatorConfigurationDirectory(
					jproject, globalOptions, reportError, create, profile);
			if (translatorDirectory != null)
				return translatorDirectory;
		}
		final String translatorDirectory = searchTranslatorConfigurationDirectory(
				jproject, globalOptions, reportError, create, null);
		return translatorDirectory;
	}

	private static String searchTranslatorConfigurationDirectory(
			IJavaProject jproject, GlobalOptions globalOptions,
			boolean reportError, boolean create, String profile) {
		String translatorDirectory = File.separator + "translator"
				+ ((profile == null) ? "" : (File.separator + profile))
				+ File.separator;
		IFolder folder = jproject.getProject().getFolder(translatorDirectory);
		if (!folder.exists()) {
			if (profile == null)
				translatorDirectory = File.separator
						+ Utils.buildPath("src", "main", "translator");
			else
				translatorDirectory = File.separator
						+ Utils.buildPath("src", "main", "translator", profile);
			folder = jproject.getProject().getFolder(translatorDirectory);
			if (!folder.exists() && reportError && !create) {
				globalOptions.getLogger().logWarning(
						"Can't find translator directory for project "
								+ jproject.getProject().getName());
				return null;
			}
		}
		return translatorDirectory;
	}
	
	//
	// getOrCreateTranslatorConfigurationFile
	//
	public static String getOrCreateTranslatorConfigurationFile(
			IJavaProject jproject, GlobalOptions globalOptions,
			boolean reportError, String translatorDirectory, boolean create) {
		final IFolder folder = jproject.getProject().getFolder(
				translatorDirectory);
		if (!folder.exists() && create) {
			try {
				createFolders(jproject.getProject(), "src", "main",
						"translator");
			} catch (final CoreException e) {
				if (reportError)
					globalOptions.getLogger()
							.logException(
									"Can't create repository "
											+ translatorDirectory, e);
			}
		}
		final String projectLoc = jproject.getProject().getLocation().toFile()
				.getAbsolutePath();
		final IFile ifile = folder.getFile("translation.xml");
		if (!ifile.exists() && create) {
			try {
				final File dir = new File(projectLoc + translatorDirectory);
				final File file = new File(projectLoc + translatorDirectory
						+ "translation.xml");
				if (dir.exists() || dir.mkdirs()) {
					String fn = projectLoc + translatorDirectory
							+ "translation.xml";
					if (!file.exists()) {
						// file.createNewFile();
						fn = createTranslatorFiles(jproject, globalOptions,
								translatorDirectory, file.getAbsolutePath());
					} else {
						final FileInputStream stream = new FileInputStream(fn);
						ifile.create(stream, true, new NullProgressMonitor());
					}
				}
			} catch (final CoreException e) {
				if (reportError)
					globalOptions.getLogger().logException(
							"Can't create file " + translatorDirectory
									+ "translation.xml", e);
			} catch (final FileNotFoundException e) {
				if (reportError)
					globalOptions.getLogger().logException(
							"Can't create file " + translatorDirectory
									+ "translation.xml", e);
			} catch (final IOException e) {
				if (reportError)
					globalOptions.getLogger().logException(
							"Can't create file " + translatorDirectory
									+ "translation.xml", e);
			} catch (final SAXException e) {
				if (reportError)
					globalOptions.getLogger().logException(
							"Can't create file " + translatorDirectory
									+ "translation.xml", e);
			} catch (final ParserConfigurationException e) {
				if (reportError)
					globalOptions.getLogger().logException(
							"Can't create file " + translatorDirectory
									+ "translation.xml", e);
			}
		}
		return projectLoc + translatorDirectory + "translation.xml";
	}

	private static void createFolders(IProject project, String... dirs)
			throws CoreException {
		String res = "/";
		for (final String dirName : dirs) {
			res += dirName + "/";
			final IFolder folder = project.getFolder(res);
			if (!folder.exists())
				folder.create(true, true, new NullProgressMonitor());
		}
	}

	//
	// nullablePrimitiveTypes
	//
	
	public boolean nullablePrimitiveTypes() {
		return nullablePrimitiveTypes.getValue();
	}

	public boolean useIsolationForMapping() {
		return useIsolationForMapping.getValue();
	}

	//
	// collectPublicDocumentedClasses
	//
	
	public boolean collectPublicDocumentedClass() {	
		return collectPublicDocumentedClasses.getValue();
	}
}
