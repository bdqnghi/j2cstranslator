package com.ilog.translator.java2cs.configuration.info;

import static com.ilog.translator.java2cs.configuration.TranslatorProjectOptions.UnitTestLibrary.NONE;

import java.io.File;
import java.io.FileNotFoundException;
import java.io.FilenameFilter;
import java.io.IOException;
import java.io.InputStream;
import java.net.URL;
import java.util.ArrayList;
import java.util.Collection;
import java.util.Enumeration;
import java.util.HashMap;
import java.util.Hashtable;
import java.util.List;
import java.util.Map;
import java.util.jar.JarEntry;
import java.util.jar.JarFile;

import javax.xml.parsers.ParserConfigurationException;

import org.eclipse.core.resources.IFile;
import org.eclipse.core.resources.IProject;
import org.eclipse.core.runtime.CoreException;
import org.eclipse.core.runtime.FileLocator;
import org.eclipse.core.runtime.IPath;
import org.eclipse.core.runtime.NullProgressMonitor;
import org.eclipse.core.runtime.Path;
import org.eclipse.jdt.core.IClassFile;
import org.eclipse.jdt.core.IField;
import org.eclipse.jdt.core.IJavaElement;
import org.eclipse.jdt.core.IJavaProject;
import org.eclipse.jdt.core.IMethod;
import org.eclipse.jdt.core.IPackageFragment;
import org.eclipse.jdt.core.IPackageFragmentRoot;
import org.eclipse.jdt.core.IType;
import org.eclipse.jdt.core.ITypeHierarchy;
import org.eclipse.jdt.core.JavaCore;
import org.eclipse.jdt.core.JavaModelException;
import org.eclipse.jdt.core.Signature;
import org.eclipse.jdt.internal.core.TypeParameter;
import org.eclipse.jdt.internal.corext.util.MethodOverrideTester;
import org.eclipse.jdt.internal.corext.util.SuperTypeHierarchyCache;
import org.eclipse.osgi.baseadaptor.bundlefile.BundleEntry;
import org.xml.sax.SAXException;

import com.ilog.translator.java2cs.configuration.TranslationConfiguration;
import com.ilog.translator.java2cs.configuration.TranslatorDependency;
import com.ilog.translator.java2cs.configuration.target.TargetClass;
import com.ilog.translator.java2cs.configuration.target.TargetField;
import com.ilog.translator.java2cs.configuration.target.TargetProperty;
import com.ilog.translator.java2cs.plugin.TranslationPlugin;
import com.ilog.translator.java2cs.translation.ITranslationContext;
import com.ilog.translator.java2cs.translation.noderewriter.IndexerRewriter;
import com.ilog.translator.java2cs.translation.noderewriter.PropertyRewriter;
import com.ilog.translator.java2cs.translation.util.InterfaceRenamingUtil;
import com.ilog.translator.java2cs.translation.util.HandlerUtil;
import com.ilog.translator.java2cs.util.CSharpModelUtil;

/**
 * 
 * @author afau
 * 
 *         Represents the consolidated mapping for a given project
 */
@SuppressWarnings("restriction")
public class TranslateInfo implements Constants {

	public static TargetClass SBYTE_TC = null;
	public static TargetClass BOOLEAN_TC = null;

	//
	//
	//
	
	private Collection<MappingsInfo> mappingsList = new ArrayList<MappingsInfo>();
	private MappingsInfo generatedMappingsInfo = new MappingsInfo(
			GENERATED, this);
	private MappingsInfo thisProjectMappingsInfo = new MappingsInfo(
			"thisProject", this);
	private Map<String, TargetField> arrayFields = new HashMap<String, TargetField>();
	private Map<String, MethodInfo> removedMethodCache = new HashMap<String, MethodInfo>();
	private Map<String, MethodInfo> hierarchyMethodCache = new HashMap<String, MethodInfo>();
	protected TranslationConfiguration configuration;

	//
	// Constructor
	//

	public TranslateInfo(TranslationConfiguration configuration) {
		this.configuration = configuration;
		mappingsList.add(thisProjectMappingsInfo);
	}

	//
	// finalize
	//
	
	@Override
	public void finalize() {
		mappingsList = null;
		generatedMappingsInfo =null;
		thisProjectMappingsInfo = null;
		arrayFields = null;
		removedMethodCache = null;
		hierarchyMethodCache = null;
		configuration = null;
	}
	
	//
	// resolve
	//

	/*
	 * To deal with vararg ...
	 */
	public static String resolve(String typeName) throws JavaModelException {
		// varargs support for signature
		if (typeName.equals("T...")) {
			return "[TT;";
		}
		if (typeName.endsWith("...")) {
			return "[L" + typeName.substring(0, typeName.length() - 3) + ";";
		}
		return Signature.createTypeSignature(typeName, false);
	}

	//
	// Initialize (load mapping files)
	//

	public void readProjectMappings() throws Exception {
		final IPath projectPath = configuration.getWorkingProject()
				.getCorrespondingResource().getLocation();
		final File projectFile = configuration.getWorkingProject().getProject()
				.getLocation().toFile();

		// First the dependences
		final Collection<TranslatorDependency> dependences = configuration
				.getOptions().getReferencedProjectsLocation();
		for (final TranslatorDependency dep : dependences) {
			switch (dep.getKind()) {
			case PROJECT:
				final String dirName = findTranslatorDirectory(dep
						.getLocation());
				this.loadMappingsFiles(dirName, dep.getReference(), dep
						.getProfile());
				this.loadMappingsFiles(dirName + GENERATED + File.separator,
						dep.getReference(), null);
				break;
			case JAR:
				final JarFile jar = new JarFile(dep.getLocation());
				final Enumeration<JarEntry> entries = jar.entries();
				while (entries.hasMoreElements()) {
					final JarEntry entry = entries.nextElement();
					final String name = entry.getName();
					if (isAMappingFile(name)) {
						final InputStream stream = jar.getInputStream(entry);
						/*
						 * MappingsInfo mInfo = new MappingsInfo(name, this);
						 * mInfo.readFromStream(conFile);
						 * mappingsList.add(mInfo);
						 */
						thisProjectMappingsInfo.readFromStream(name, stream, false);
					}
				}
				break;
			case DIRECTORY:
				final List<String> locations = scanForTranslatorDirectories(dep
						.getLocation());
				for (final String cloc : locations) {
					this.loadMappingsFiles(cloc, null, null);
					this.loadMappingsFiles(cloc + File.separator + GENERATED
							+ File.separator, null, null);
				}
				break;
			}
		}

		// In case of partial translation we need to know what the translator
		// did
		// for non translated classes
		final boolean readgenerated = (configuration.getOptions()
				.getClassFilter() != null || configuration.getOptions()
				.getPackageFilter() != null);
		// Then, the current configuration file(s)
		final String dirName = findTranslatorDirectory(projectFile
				.getAbsolutePath());
		if (dirName != null) {
			this.loadMappingsFiles(dirName, configuration.getWorkingProject(),
					configuration.getOptions().getProfile());
			if (readgenerated)
				this.loadMappingsFiles(dirName + GENERATED + File.separator,
						configuration.getWorkingProject(), null);
		}
	}

	private static boolean isAMappingFile(final String name) {
		return name.endsWith(MappingsInfo.MAPPING_FILE_EXTENTION)
				|| name.endsWith(MappingsInfo.XML_MAPPING_FILE_EXTENTION)
				|| name.endsWith(MappingsInfo.OLD_MAPPING_FILE_EXTENTION);
	}

	private String findTranslatorDirectory(String projectFile)
			throws IOException, JavaModelException {
		String translatorDirectory = TranslateInfo.TRANSLATOR_K;
		File directory = new File(projectFile + File.separator
				+ translatorDirectory);
		if (!directory.exists()) {
			translatorDirectory = SRC_MAIN_TRANSLATOR;
			directory = new File(projectFile + File.separator
					+ translatorDirectory);
			if (!directory.exists()) {
				configuration.getLogger().logError(
						"Can't find translator directory for project "
								+ projectFile);
				return null;
			}
		}
		final String dirName = projectFile + File.separator
				+ translatorDirectory + File.separator + CONFIGURATION
				+ File.separator;
		return dirName;
	}

	private List<String> scanForTranslatorDirectories(String location) {
		final File dir = new File(location);
		final File[] files = dir.listFiles();
		final List<String> results = new ArrayList<String>();
		for (final File file : files) {
			if (file.isDirectory()) {
				if (file.getName().equals(TRANSLATOR)) {
					results.add(file.getAbsolutePath());
				} else {
					results.addAll(scanForTranslatorDirectories(file
							.getAbsolutePath()));
				}
			}
		}
		return results;
	}

	//
	// readJDKMappings
	//

	@SuppressWarnings("unchecked")
	public void readJDKMappings() throws Exception {
		IPath path = new Path(CONFIGURATION);
		URL url = FileLocator.find(TranslationPlugin.getDefault().getBundle(),
				path, null);
		url = FileLocator.toFileURL(url);

		final Enumeration enume = TranslationPlugin.getDefault().getBundle()
				.findEntries("/configuration/", "*", true);
		this.loadMappingsFiles(enume, true);

		if (configuration.getOptions().isUseGenerics()) {
			path = new Path(CONFIGURATION + File.separator
					+ MappingsInfo.XML_GENERIC_MAPPING_FILE);
			try {
				InputStream stream = FileLocator.openStream(TranslationPlugin
						.getDefault().getBundle(), path, false);
				thisProjectMappingsInfo.readXmlFromStream(path.toString(),
						stream, this.configuration.getOriginalProject(), true);
			} catch (IOException e) {
				// e.printStackTrace();
				path = new Path(CONFIGURATION + File.separator
						+ MappingsInfo.GENERIC_MAPPING_FILE);
				InputStream stream = FileLocator.openStream(TranslationPlugin
						.getDefault().getBundle(), path, false);
				thisProjectMappingsInfo.readFromStream(path.toString(), stream, true);
			}
		}

		// unit test
		if (!(configuration.getOptions().getUnitTestLibrary() == NONE)) {
			String mappingFileName = null;
			switch (configuration.getOptions().getUnitTestLibrary()) {
			case JUNIT3:
				mappingFileName = "junit";
				break;
			case JUNIT4:
				mappingFileName = "junit4";
				break;
			case TESTNG:
				mappingFileName = "testng";
				break;
			}
			String fileName = CONFIGURATION + File.separator + mappingFileName
					+ MappingsInfo.MAPPING_FILE_EXTENTION + "ml";
			path = new Path(fileName);
			try {
				final InputStream stream = FileLocator
						.openStream(TranslationPlugin.getDefault().getBundle(),
								path, false);
				thisProjectMappingsInfo.readXmlFromStream(path.toString(),
						stream, this.configuration.getOriginalProject(), true);
			} catch (Exception e) {
				fileName = CONFIGURATION + File.separator + mappingFileName
						+ MappingsInfo.MAPPING_FILE_EXTENTION;
				path = new Path(fileName);
				final InputStream stream = FileLocator
						.openStream(TranslationPlugin.getDefault().getBundle(),
								path, false);
				thisProjectMappingsInfo.readFromStream(path.toString(), stream, true);
			}
		}

		// Core knowledge
		// TODO: need to be externalized
		addArrayField("length", new TargetField("Length"));
		TranslateInfo.SBYTE_TC = TargetClass.SBYTE;
		TranslateInfo.BOOLEAN_TC = TargetClass.BOOL;
	}

	//
	// migrate
	//

	@SuppressWarnings("unchecked")
	public void migrateJDKMappings(String dirName) throws Exception {
		IPath path = new Path(CONFIGURATION);
		URL url = FileLocator.find(TranslationPlugin.getDefault().getBundle(),
				path, null);
		url = FileLocator.toFileURL(url);

		final Enumeration enume = TranslationPlugin.getDefault().getBundle()
				.findEntries("/configuration/", "*", true);
		this.migrateMappingsFiles(enume, dirName, true);

		if (configuration.getOptions().isUseGenerics()) {
			path = new Path(CONFIGURATION + File.separator
					+ MappingsInfo.XML_GENERIC_MAPPING_FILE);
			try {
				InputStream stream = FileLocator.openStream(TranslationPlugin
						.getDefault().getBundle(), path, false);
				MappingsInfo tempProjectMappingsInfo = new MappingsInfo(path
						.toString(), this);
				tempProjectMappingsInfo.readXmlFromStream(path.toString(),
						stream, this.configuration.getOriginalProject(), true);
				String name = path.toString().substring(0, path.toString().indexOf("."));
				tempProjectMappingsInfo.saveXML(dirName + name + MappingsInfo.XML_MAPPING_FILE_EXTENTION
						/*+ "ml"*/);
			} catch (IOException e) {
				// e.printStackTrace();
				path = new Path(CONFIGURATION + File.separator
						+ MappingsInfo.OLD_GENERIC_MAPPING_FILE);
				InputStream stream = FileLocator.openStream(TranslationPlugin
						.getDefault().getBundle(), path, false);
				MappingsInfo tempProjectMappingsInfo = new MappingsInfo(path
						.toString(), this);
				tempProjectMappingsInfo.readFromStream(path.toString(), stream, true);
				String name = path.toString().substring(0, path.toString().indexOf("."));
				tempProjectMappingsInfo.saveXML(dirName + name + MappingsInfo.XML_MAPPING_FILE_EXTENTION
						/*+ "ml"*/);
			}
		}

		// unit test
		if (!(configuration.getOptions().getUnitTestLibrary() == NONE)) {

			String mappingFileName = null;
			switch (configuration.getOptions().getUnitTestLibrary()) {
			case JUNIT3:
				mappingFileName = "junit";
				break;
			case JUNIT4:
				mappingFileName = "junit4";
				break;
			case TESTNG:
				mappingFileName = "testng";
				break;
			}
			MappingsInfo tempProjectMappingsInfo = new MappingsInfo(
					mappingFileName, this);
			String fileName = CONFIGURATION + File.separator + mappingFileName
					+ MappingsInfo.XML_MAPPING_FILE_EXTENTION;
			try {
				path = new Path(fileName);
				final InputStream stream = FileLocator
						.openStream(TranslationPlugin.getDefault().getBundle(),
								path, false);
				tempProjectMappingsInfo.readXmlFromStream(path.toString(),
						stream, this.configuration.getOriginalProject(), true);
				String name = mappingFileName;
				if (name.contains("."))
					name = name.substring(0, name.lastIndexOf("."));
				tempProjectMappingsInfo.saveXML(dirName + name + MappingsInfo.XML_MAPPING_FILE_EXTENTION);
			} catch (Exception e) {
				fileName = CONFIGURATION + File.separator + mappingFileName
						+ MappingsInfo.OLD_MAPPING_FILE_EXTENTION;
				path = new Path(fileName);
				final InputStream stream = FileLocator
						.openStream(TranslationPlugin.getDefault().getBundle(),
								path, false);
				tempProjectMappingsInfo.readFromStream(path.toString(), stream, true);
				String name = mappingFileName;
				if (name.contains("."))
					name = name.substring(0, name.lastIndexOf("."));
				tempProjectMappingsInfo.saveXML(dirName + name + MappingsInfo.XML_MAPPING_FILE_EXTENTION);
			}
		}
	}

	public void migrateProjectMappings(String destDirName) throws Exception {
		final File projectFile = configuration.getWorkingProject().getProject()
				.getLocation().toFile();

		final String dirName = findTranslatorDirectory(projectFile
				.getAbsolutePath());
		if (dirName != null) {
			this.migrateMappingsFiles(dirName, configuration
					.getWorkingProject(), configuration.getOptions()
					.getProfile(), destDirName);
		}
	}

	public void migrateThisMappingFile(String string, IFile file) throws IOException, JavaModelException {		
		final String name = file.getRawLocation().toOSString();
		if (configuration.getOptions().getGlobalOptions().isDebug()) {
			configuration.getLogger().logInfo(
					"     Loading mapping file " + name);
		}
		MappingsInfo tempProjectMappingsInfo = new MappingsInfo(name,
				this);
		tempProjectMappingsInfo.read(name, configuration
				.getWorkingProject(), false);
		tempProjectMappingsInfo.saveXML(name.replace(".conf",
				".mappingml"));
	}
	
	@SuppressWarnings("unchecked")
	private void migrateMappingsFiles(Enumeration entries, String destDirName, boolean system)
			throws IOException, FileNotFoundException, JavaModelException {

		while (entries.hasMoreElements()) {
			final Object entry = entries.nextElement();
			InputStream stream = null;
			String name = null;
			if (entry instanceof BundleEntry) {
				final BundleEntry bEntry = ((BundleEntry) entry);
				stream = bEntry.getInputStream();
				name = bEntry.getName();
			} else if (entry instanceof URL) {
				final URL zEntry = (URL) entry;
				if (!zEntry.getFile().contains("/.svn/")
						&& !zEntry.getFile().contains("/_svn/") 
						&& !zEntry.getFile().contains("Napa")
						&& !zEntry.getFile().contains("OLD/")) {
					stream = zEntry.openStream();
					name = zEntry.getFile();
				}
			}

			if (stream != null && isAMappingFile(name)
					&& !name.endsWith(MappingsInfo.GENERIC_MAPPING_FILE)
					&& !name.endsWith(MappingsInfo.XML_GENERIC_MAPPING_FILE)
					&& !name.endsWith(MappingsInfo.OLD_GENERIC_MAPPING_FILE)
					&& !name.endsWith("junit.mapping")
					&& !name.endsWith("junit4.mapping")
					&& !name.endsWith("testng.mapping")
					&& !name.endsWith("junit.conf")
					&& !name.endsWith("junit4.conf")
					&& !name.endsWith("testng.conf")
					&& !name.endsWith("junit.mappingml")
					&& !name.endsWith("junit4.mappingml")
					&& !name.endsWith("testng.mappingml")) {
				try {
					MappingsInfo tempProjectMappingsInfo = new MappingsInfo(
							name, this);
					if (name.endsWith(MappingsInfo.XML_MAPPING_FILE_EXTENTION)) {
						tempProjectMappingsInfo.readXmlFromStream(name, stream,
								this.configuration.getOriginalProject(), system);
					} else {
						tempProjectMappingsInfo.readFromStream(name, stream, system);
					}
					name = name.substring(0, name.lastIndexOf("."));
					tempProjectMappingsInfo.saveXML(destDirName + name + 
							MappingsInfo.XML_MAPPING_FILE_EXTENTION);
				} catch (final Exception e) {
					configuration.getLogger().logException(
							"Error when reading mappinf file " + name, e);
				}
			}
		}
	}

	private void migrateMappingsFiles(String dirName, IJavaProject reference,
			String profile, String destDirName) throws IOException,
			FileNotFoundException, JavaModelException {
		if (configuration.getOptions().getGlobalOptions().isDebug()) {
			configuration.getLogger().logInfo(
					" Loading configuration file from " + dirName);
		}

		String directory = dirName;
		if (profile != null && !profile.equals("")) {
			directory = dirName + File.separator + profile;
		}

		final File dir = new File(directory);
		final File[] userfiles = dir.listFiles(new FilenameFilter() {
			public boolean accept(File file, String name) {
				if (name.equals(MappingsInfo.GENERIC_MAPPING_FILE)) {
					return false;
				} else {
					return isAMappingFile(name);
				}
			}
		});
		if (userfiles != null) {
			for (final File f : userfiles) {
				final String name = f.getCanonicalPath();
				if (configuration.getOptions().getGlobalOptions().isDebug()) {
					configuration.getLogger().logInfo(
							"     Loading mapping file " + name);
				}
				MappingsInfo tempProjectMappingsInfo = new MappingsInfo(name,
						this);
				tempProjectMappingsInfo.read(name, reference, false);
				tempProjectMappingsInfo.saveXML(name.replace(".conf",
						".mappingml"));
			}
		}
	}

	//
	// propagate
	//

	/*
	 * Propagate translation mapping from super classes to all child.
	 */
	public void propagate(ITranslationContext context)
			throws JavaModelException {
		Hashtable<String, List<ClassInfo>> allClasses = new Hashtable<String, List<ClassInfo>>();
		for (final PackageInfo pInfo : getPackages()) {
				for (final ClassInfo cInfo : pInfo.getAllClasses()) {
					List<ClassInfo> list = allClasses.get(cInfo.getName());
					if (list == null) {
						list = new ArrayList<ClassInfo>();
						allClasses.put(cInfo.getName(), list); // compute name
					}
					list.add(cInfo); 
				}
		}
		String targetFramework = getConfiguration().getOptions().getTargetDotNetFramework().name();
		for (final List<ClassInfo> listCInfo : allClasses.values()) {
			for(final ClassInfo cInfo : listCInfo) {
				if (cInfo.getTarget(targetFramework) != null && cInfo.getTarget(targetFramework).isTranslated()) {
					context.addToIgnorable(cInfo);
				}
				PackageInfo pInfo = cInfo.getPackageInfo(); 
				if (pInfo.getTarget(targetFramework) != null && pInfo.getTarget(targetFramework).isTranslated()) {
					context.addToIgnorable(cInfo);
				}
				cInfo.computeParents(configuration.getWorkingProject(), allClasses);
				// Look for properties/indexers be be created
				propertiesBuilder(cInfo);
				indexersBuilder(cInfo);
			}
		}
		allClasses = null;
	}

	//
	// propertiesBuilder
	//

	private void propertiesBuilder(ClassInfo info) {
		final Map<String, MethodInfo> methodsMap = info.getMethodsMap();
		String targetFramework = getConfiguration().getOptions().getTargetDotNetFramework().name();
		for (final MethodInfo mInfo : methodsMap.values()) {
			if (mInfo.getTarget(targetFramework) != null
					&& mInfo.getTarget(targetFramework).isMappedToAProperty()) {
				final PropertyRewriter pr = (PropertyRewriter) mInfo
						.getTarget(targetFramework).getRewriter();
				TargetProperty tProperty = (TargetProperty) mInfo.getTarget(targetFramework);
				info.addProperty(targetFramework, pr.getName() /* mInfo.getTargetMethod().getName()*/, pr
						.getKind(), mInfo, tProperty);
			}
		}
	}
	
	//
	// indexersBuilder
	//

	private void indexersBuilder(ClassInfo info) {
		final Map<String, MethodInfo> methodsMap = info.getMethodsMap();
		String targetFramework = getConfiguration().getOptions().getTargetDotNetFramework().name();
		for (final MethodInfo mInfo : methodsMap.values()) {
			if (mInfo.getTarget(targetFramework) != null
					&& mInfo.getTarget(targetFramework).isMappedToAnIndexer()) {
				final IndexerRewriter pr = (IndexerRewriter) mInfo
						.getTarget(targetFramework).getRewriter();
				info.addIndexer(pr.getKind(), pr.getParamsIndex(), pr
						.getValueIndex(), mInfo);
			}
		}
	}

	/**
	 * Load all mappings files (files with .mappingml extension) located in the given
	 * directory 
	 * Remarks: There is a special case for generated mappings files
	 * (i.e. files produce by the translator to figure out automatic mapping
	 * added during translation). These files are loaded after all others
	 * (content is prioritary).
	 */
	private void loadMappingsFiles(String dirName, IJavaProject reference,
			String profile) throws IOException, FileNotFoundException,
			JavaModelException, SAXException, ParserConfigurationException {
		if (configuration.getOptions().getGlobalOptions().isDebug()) {
			configuration.getLogger().logInfo(
					" Loading configuration file from " + dirName);
		}

		String directory = dirName;
		if (profile != null && !profile.equals("")) {
			directory = dirName + File.separator + profile;
		}

		final File dir = new File(directory);
		final File[] userfiles = dir.listFiles(new FilenameFilter() {
			public boolean accept(File file, String name) {
				if (name.equals(MappingsInfo.GENERIC_MAPPING_FILE)
						|| name.equals(MappingsInfo.XML_GENERIC_MAPPING_FILE)) {
					return false;
				} else {
					return isAMappingFile(name);
				}
			}
		});
		if (userfiles != null) {
			for (final File f : userfiles) {
				final String name = f.getCanonicalPath();
				if (configuration.getOptions().getGlobalOptions().isDebug()) {
					configuration.getLogger().logInfo(
							"     Loading mapping file " + name);
				}
				if (name.endsWith(MappingsInfo.XML_MAPPING_FILE_EXTENTION)) {
					thisProjectMappingsInfo.readXml(name, this.configuration
							.getOriginalProject(), false);
				} else {
					if (!configuration.getOptions().isUseOnlyXML())
						thisProjectMappingsInfo.read(name, reference, false);
				}
			}
		}
	}

	@SuppressWarnings("unchecked")
	private void loadMappingsFiles(Enumeration entries, boolean system) throws IOException,
			FileNotFoundException, JavaModelException {

		while (entries.hasMoreElements()) {
			final Object entry = entries.nextElement();
			InputStream stream = null;
			String name = null;
			if (entry instanceof BundleEntry) {
				final BundleEntry bEntry = ((BundleEntry) entry);
				stream = bEntry.getInputStream();
				name = bEntry.getName();
			} else if (entry instanceof URL) {
				final URL zEntry = (URL) entry;
				if (!zEntry.getFile().contains("/.svn/")
						&& !zEntry.getFile().contains("/_svn/")
						&& !zEntry.getFile().contains("OLD/")) {
					stream = zEntry.openStream();
					name = zEntry.getFile();
				}
			}

			if (stream != null && isAMappingFile(name)
					&& !name.endsWith(MappingsInfo.GENERIC_MAPPING_FILE)
					&& !name.endsWith(MappingsInfo.XML_GENERIC_MAPPING_FILE)
					&& !name.endsWith("junit.mapping")
					&& !name.endsWith("junit4.mapping")
					&& !name.endsWith("testng.mapping")
					&& !name.endsWith("junit.mappingml")
					&& !name.endsWith("junit4.mappingml")
					&& !name.endsWith("testng.mappingml")) {
				try {
					/*
					 * MappingsInfo mInfo = new MappingsInfo(name, this);
					 * mInfo.readFromStream(stream); mappingsList.add(mInfo);
					 */
					if (name.endsWith(MappingsInfo.XML_MAPPING_FILE_EXTENTION)) {
						/*thisProjectMappingsInfo.readXmlFromStream(name, stream,
								this.configuration.getOriginalProject());*/
						if ((name.contains(PRIMITIVE_TYPES_XML_CONFIGURATION_FILE) && 
								configuration.getOptions().nullablePrimitiveTypes()) || 
								(name.contains(PRIMITIVE_TYPES_NULLABLE_XML_CONFIGURATION_FILE) && 
										!configuration.getOptions().nullablePrimitiveTypes())) {
							// do nothing
						} else {
							thisProjectMappingsInfo.readXmlFromStream(name, stream, this.configuration.getOriginalProject(), system);
						}
					} else {
						// thisProjectMappingsInfo.readFromStream(name, stream);
						if ((name.contains(PRIMITIVE_TYPES_CONFIGURATION_FILE) && 
								configuration.getOptions().nullablePrimitiveTypes()) || 
								(name.contains(PRIMITIVE_TYPES_NULLABLE_CONFIGURATION_FILE) && 
										!configuration.getOptions().nullablePrimitiveTypes())) {
							// do nothing
						} else {
							thisProjectMappingsInfo.readFromStream(name, stream, system);
						}
					}
					// loadMappingsFile(name, stream);
				} catch (final Exception e) {
					configuration.getLogger().logException(
							"Error when reading mappinf file " + name, e);
				}
			}
		}
	}

	//
	// packages
	//

	public Collection<PackageInfo> getPackages() {
		final Collection<PackageInfo> res = new ArrayList<PackageInfo>();
		for (final MappingsInfo mInfo : mappingsList) {
			final Collection<List<PackageInfo>> coll = mInfo.getPackages();
			if (coll != null) {
				for (final List<PackageInfo> lpinfo : coll) {
					//res.add(lpinfo.get(0)); // TODO:
					res.addAll(lpinfo);
				}
			}
		}
		return res;
	}

	public PackageInfo getPackage(String name, IProject reference) {
		final List<PackageInfo> packages = new ArrayList<PackageInfo>();
		for (final MappingsInfo mInfo : mappingsList) {
			final PackageInfo pInfo = mInfo.getPackage(name, reference, false);
			if (pInfo != null)
				packages.add(pInfo);
		}
		if (packages.size() == 1)
			return packages.get(0);
		return mergePackages(name, packages);
	}

	public PackageInfo createPackage(String name, IProject reference) {
		return thisProjectMappingsInfo.createPackage(name, reference);
	}

	//
	// variables
	//

	public VariableInfo getVariable(String name) {
		for (final MappingsInfo mInfo : mappingsList) {
			final VariableInfo vInfo = mInfo.getVariable(name);
			if (vInfo != null)
				return vInfo;
		}
		return null;
	}

	//
	// keywords
	//

	public String getKeyword(String key) {
		for (final MappingsInfo mInfo : mappingsList) {
			final String keyword = mInfo.getKeyword(key);
			if (keyword != null)
				return keyword;
		}
		return null;
	}

	//
	// packageforName
	// 

	public IPackageFragment packageforName(String name, IJavaProject project) throws JavaModelException {
		//Path path = new Path("/" + name);
		IPackageFragment[] pfs = project.getPackageFragments();
		for (IPackageFragment pf : pfs) {
			String eName = pf.getElementName();
			if (eName.equals(name))
				return pf;
		}
		return null;
	}

	//
	// types
	//

	public IType typeforName(String name, boolean generics)
			throws JavaModelException {
		if (generics) {
			name = name.substring(0, name.indexOf("<"));
		}
		final IType type = configuration.getWorkingProject().findType(name,
				new NullProgressMonitor());

		if (type == null) {
			final String packageName = (name.lastIndexOf(DOT) == -1) ? ""
					: name.substring(0, name.lastIndexOf(DOT));
			for (final IPackageFragmentRoot pRoot : configuration
					.getWorkingProject().getAllPackageFragmentRoots()) {
				pRoot.open(new NullProgressMonitor());
				final IPackageFragment pf = pRoot
						.getPackageFragment(packageName);
				if (pf != null && pf.exists()) {
					pf.open(new NullProgressMonitor());
					final IClassFile cf = pf.getClassFile(name.substring(name
							.lastIndexOf(DOT) + 1)
							+ ".class");
					if (cf != null && cf.exists()) {
						final IType res = cf.getType();
						if (res.exists())
							return res;
					}
				}
			}
		}
		return type;
	}

	public String resolveTypeName(String packageName, String typeName) {
		final String res = generatedMappingsInfo.resolveTypeName(packageName,
				typeName);
		if (res != null)
			return res;
		return packageName + CSharpModelUtil.CLASS_SEPARATOR + typeName;
	}

	public String getNewNestedName(String pck, String className) {
		return generatedMappingsInfo.getNewNestedName(pck, className);
	}

	//
	// array field (such as length)
	//

	public void addArrayField(String name, TargetField tField) {
		arrayFields.put(name, tField);
	}

	public TargetField getArrayField(String name) {
		return arrayFields.get(name);
	}

	//
	// ClassInfo
	//

	public ClassInfo getClassInfoFromHandler(String handler,
			boolean reportError, boolean isGeneric) {
		final IJavaElement element = HandlerUtil.createElementFromHandler(handler);
		if (!(element instanceof IType)) {
			return null;
		}
		if (element == null) {
			if (reportError) {
				configuration.getLogger().logError(
						"TranslateInfo.getClassInfoFromHandler:: can't create type "
								+ handler);
			}
			return null;
		}
		final IType type = (IType) element;
		final IPackageFragment pfragment = type.getPackageFragment();
		final PackageInfo packageInfo = getPackage(pfragment.getElementName(),
				type.getJavaProject().getProject());
		if (packageInfo == null) {
			return null;
		}
		return findClassInfo(packageInfo, type, isGeneric, false);
	}

	private ClassInfo findClassInfo(PackageInfo packageInfo, IType type,
			boolean isGeneric, boolean create) {
		ClassInfo res = null;
		if (isGeneric) {
			try {
				res = packageInfo.getGenericClass(type);
				if (res != null)
					return res;
			} catch (final TranslationModelException e) {
				// this.configuration.getLogger().logError("TranslateInfo.getClassInfoFromHandler::
				// can't create type "
				// + handler);
			}
		}
		res = packageInfo.getClass(type);
		if (res != null)
			return res;

		if (res == null) {
			if (create) {
				try {
					return packageInfo.createClass(
							type.getFullyQualifiedName(), isGeneric);
				} catch (final JavaModelException e) {
					// this.configuration.getLogger().logError("TranslateInfo.getClassInfoFromHandler::
					// can't create type "
					// + handler);
					return null;
				}
			} else
				return null;
		} else
			return res;
	}


	private PackageInfo mergePackages(String name, List<PackageInfo> packages) {
		if (packages.size() == 0)
			return null;
		else if (packages.size() == 1) {
			return packages.get(0);
		} else {
			configuration.getLogger().logWarning(
					"More than one packages defined for package " + name);
			return packages.get(0);
		}
	}

	public ClassInfo createClassInfoFromHandler(String handler,
			boolean reportError, boolean create, boolean isGeneric)
			throws JavaModelException {
		final IJavaElement element = HandlerUtil.createElementFromHandler(handler); //
		if (element instanceof TypeParameter) {
			return null;
		}
		final IType type = (IType) element;
		if (type == null) {
			if (reportError) {
				configuration.getLogger().logError(
						"TranslateInfo.getClassInfoFromHandler:: can't create type "
								+ handler);
			}
			return null;
		}
		final IPackageFragment pfragment = type.getPackageFragment();
		PackageInfo packageInfo = getPackage(pfragment.getElementName(), type
				.getJavaProject().getProject());
		if (packageInfo == null) {
			if (create)
				packageInfo = createPackage(pfragment.getElementName(), type
						.getJavaProject().getProject());
			else
				return null;
		}
		final ClassInfo res = findClassInfo(packageInfo, type, isGeneric, true);
		return res;
	}

	//
	// MethodInfo
	//

	public MethodInfo getMethodInfoFromHandler(String className,
			String signature, String handler, boolean searchForRemove,
			boolean storeRemove, boolean isoverride, boolean isGeneric)
			throws TranslationModelException {

		MethodInfo mInfo = null;
		final String id = className + "_" + signature;

		if (searchForRemove) {
			return removedMethodCache.get(handler);
		}

		final IMethod method = (IMethod) HandlerUtil.createElementFromHandler(handler);
		if (method == null) {
			configuration.getLogger().logError(
					"TranslateInfo.getMethodInfoFromHandler::Can't find "
							+ handler + "/" + className + signature);
			throw new TranslationModelException("");
		}
		mInfo = this.getMethodInfo(signature, method, isGeneric); // TODO
		String targetFramework = getConfiguration().getOptions().getTargetDotNetFramework().name();
		if (mInfo != null) {			
			// TODO : hope it's what I want
			if (storeRemove && mInfo.getTarget(targetFramework) != null
					&& mInfo.getTarget(targetFramework).isTranslated()) {
				removedMethodCache.put(handler, mInfo);
			}
		}
		//
		//
		if (isoverride && method != null && mInfo == null && method.exists()) {
			try {
				if (hierarchyMethodCache.containsKey(id))
					return hierarchyMethodCache.get(id);
				
				final IType type = method.getDeclaringType();
				ITypeHierarchy hierarchy = SuperTypeHierarchyCache.getTypeHierarchy(type); // type.newTypeHierarchy(pm);
				//
				MethodOverrideTester tester = new MethodOverrideTester(
						type, hierarchy);
				//
				final IMethod declaringMethod = tester.findDeclaringMethod(
						method, false);
				tester = null;
				hierarchy = null;
				if (declaringMethod != null) {
					final IPackageFragment pfragment = type
							.getPackageFragment();
					PackageInfo packageInfo = getPackage(pfragment
							.getElementName(), type.getJavaProject()
							.getProject());
					if (packageInfo == null) {
						packageInfo = createPackage(pfragment.getElementName(),
								type.getJavaProject().getProject());
						// return null;
					}
					mInfo = this.getMethodInfo(null /* signature */,
							declaringMethod, isGeneric); // TODO
					// TODO: Add a new class info ....
					if (mInfo != null) {
						ClassInfo ci = packageInfo.getClass(type);
						if (ci == null) {
							ci = new ClassInfoImpl(generatedMappingsInfo, method
									.getDeclaringType(), isGeneric, packageInfo);
							packageInfo.addClass(type, ci, false);
							//
							// TODO: add this new method mapping to the
							// serializable part
							//
							// The idea is to avoid creating a ClassInfo for
							// each class referenced
							// in conf files because having a classinfo is the
							// only way to search
							// for parent when we looking for a method.
							// We do it in a lazy way ... If the class is on the
							// current project no
							// trouble we just create the necessary object. If
							// the class is in an
							// other project, we just add in the generated file
							// an line that create
							// the corresponding class info.
							if (mInfo.getTarget(targetFramework).getName() != null) {
								IType currentType = type;
								String cName = currentType.getElementName();
								while (currentType.isMember()) {
									currentType = currentType
											.getDeclaringType();
									cName = currentType.getElementName() + DOT
											+ cName;
								}
								addImplicitMethodOverride(pfragment
										.getElementName(), cName, mInfo);
							}
						}
						final MethodInfo mi = ci.resolveMethod(method
								.getElementName(), method.getParameterTypes());

						mi.addTargetMethod(targetFramework, mInfo.getTarget(targetFramework)
								.cloneForChild());
					}
					//
					hierarchyMethodCache.put(id, mInfo);
				}
				//
			} catch (final CoreException e) {
				// TODO: !!
				e.printStackTrace();
				configuration.getLogger().logException(
						"getMethodInfoFromHandler", e);
			}
		}
		return mInfo;
	}

	private MethodInfo getMethodInfo(String signature, IMethod method,
			boolean isGeneric) throws TranslationModelException {
		final IType type = method.getDeclaringType();
		return this.getMethodInfo(signature, method, type, isGeneric);
	}

	private MethodInfo getMethodInfo(String signature, IMethod method,
			IType type, boolean isGeneric) throws TranslationModelException {
		final IPackageFragment pfragment = type.getPackageFragment();
		PackageInfo packageInfo = getPackage(pfragment.getElementName(), type
				.getJavaProject().getProject());
		if (packageInfo == null) {
			return null;
		}
		try {
			ClassInfo cInfo = findClassInfo(packageInfo, type, isGeneric
					&& getConfiguration().getOptions().isUseGenerics(), false);
			if (cInfo != null) {
				if (method.isStructureKnown()) {
					MethodInfo mInfo = null;
					if (cInfo.isConstructor(method)) {
						mInfo = cInfo.getMethod(null, method);
					} else {
						mInfo = cInfo.getMethod(signature, method);
					}
					if (mInfo != null) {
						return mInfo;
					}
				}
			}
			// TODO: Special case for Object methods ... UGLY !
			if (method.getElementName().equals(HASH_CODE)) {
				packageInfo = getPackage(JAVA_LANG, type.getJavaProject()
						.getProject());
				cInfo = packageInfo.getClass(typeforName(JAVA_LANG_OBJECT,
						false));
				final MethodInfo mInfo = cInfo.getMethod(signature, method);
				if (mInfo != null) {
					return mInfo;
				}

			}
		} catch (final Exception e) {
			e.printStackTrace();
			configuration.getLogger().logException("getMethodInfo", e);
		}
		// Ok we can't find any info on
		return null;
	}

	//
	// FieldInfo
	//

	public FieldInfo getFieldInfoFromHandler(String fName, String handler) {
		final IJavaElement element = HandlerUtil.createElementFromHandler(handler);
		// BR 2103692. Patch from faron
		if (element == null) {
			configuration
					.getLogger()
					.logError(
							"TranslateInfo.getFieldInfoFromHandler::cannot create a Java element from field handler : "
									+ handler + " / name " + fName);
			return null;
		}
		switch (element.getElementType()) {
		case IJavaElement.FIELD:
			final IField field = (IField) element;
			final IType type = field.getDeclaringType();
			final IPackageFragment pfragment = type.getPackageFragment();
			final PackageInfo packageInfo = getPackage(pfragment
					.getElementName(), type.getJavaProject().getProject());
			if (packageInfo != null) {
				final ClassInfo cInfo = findClassInfo(packageInfo, type, false,
						false);
				if (cInfo != null) {
					try {
						return cInfo.getField(field);
					} catch (final JavaModelException e) {
						e.printStackTrace();
						return null;
					} catch (final Exception e) {
						configuration.getLogger().logException(
								"TranslateInfo.getFieldInfoFromHandler::can't create : "
										+ handler + " " + type, e);
						e.printStackTrace();
						return null;
					}
				}
			}
			// No package found
			return null;
		}
		return null;
	}

	//
	// ConstructorInfo
	//

	public MethodInfo getConstructorInfo(String packageName, String className,
			String[] params, boolean searchForRemove, String handler) {
		String signature = Signature.createMethodSignature(params, VOID);
		signature = InterfaceRenamingUtil.replaceInterfaceRenamed(signature);
		String pck = packageName == null ? "" : packageName + DOT;
		
		if (searchForRemove) {
			// TODO: unused ...
			return removedMethodCache.get(pck + className + "_" + signature);
		}

		final IMethod method = (IMethod) HandlerUtil.createElementFromHandler(handler);
		if (method == null) {
			configuration.getLogger().logError(
					"TranslateInfo.getMethodInfoFromHandler::Can't find "
							+ handler);
			return null;
		}

		// fond the constructor and the find the type
		final PackageInfo packageInfo = getPackage(packageName, method
				.getDeclaringType().getJavaProject().getProject());

		if (packageInfo == null) {
			return null;
		}
		MethodInfo mInfo = null;
		// TODO: Trouble with new Integer(12) ...
		try {
			final String eclassName = eraseTypeParameter(className);
			final IType type = typeforName(pck + eclassName, eclassName
					.contains("<"));
			if (type == null) {
				configuration.getLogger().logError(
						"TranslateInfo.getConstructorInfo::Can't find type "
								+ pck + className);
				return null;
			}
			IMethod im = null;
			if (className.indexOf(DOT) > 0) {
				im = type.getMethod(type.getElementName(), params);
			} else {
				im = type.getMethod(eclassName, params);
			}
			if (im == null) {
				configuration.getLogger().logError(
						"TranslateInfo.getConstructorInfo::Can't find constructor "
								+ params);
				return null;
			}
			final ClassInfo cInfo = packageInfo.getClass(pck + className);
			if (cInfo == null) {
				return null;
			}
			mInfo = cInfo.getConstructor(im);
		} catch (final Exception e) {
			e.printStackTrace();
			getConfiguration().getLogger()
					.logException("getConstructorInfo", e);
		}
		return mInfo;
	}

	private String eraseTypeParameter(String className) {
		final int sindex = className.indexOf("<");
		if (sindex > 0) {
			final String NclassName = className.substring(0, sindex); // +
			// "<";
			return NclassName;
		}
		return className;
	}

	//
	// update method info
	//

	public MethodInfo updateMethodInfo(String elementName,
			String existingSignatureTag, String existingHandlerTag,
			String signatureKey, String handlerKey)
			throws TranslationModelException {
		MethodInfo mInfo = null;

		final IMethod method = (IMethod) HandlerUtil.createElementFromHandler(handlerKey);
		if (method == null) {
			configuration.getLogger().logError(
					"TranslateInfo.updateMethodInfo::Can't find "
							+ existingHandlerTag);
			return null;
		}
		mInfo = this.getMethodInfo(existingSignatureTag, method, false);
		if (mInfo != null) {
			return mInfo;
		}

		return null;
	}

	//
	// generic class
	//
	
	public ClassInfo getGenericClassInfoFromHandler(String handleIdentifier)
			throws TranslationModelException {
		final IJavaElement element = HandlerUtil.createElementFromHandler(handleIdentifier);
		if (element instanceof IType) {
			final IType type = (IType) element;
			if (type == null) {
				configuration.getLogger().logError(
						"TranslateInfo.getClassInfoFromHandler:: can't create type "
								+ handleIdentifier);
				return null;
			}
			final IPackageFragment pfragment = type.getPackageFragment();
			final PackageInfo packageInfo = getPackage(pfragment
					.getElementName(), type.getJavaProject().getProject());
			if (packageInfo == null) {
				return null;
			}
			final ClassInfo res = packageInfo.getGenericClass(type);
			return res;
		} else {
			if (element.getElementType() != IJavaElement.TYPE_PARAMETER) {
				throw new TranslationModelException(
						"TranslateInfo.getClassInfoFromHandler:: Don't know what to do with "
								+ element);
			}
			return null;
		}
	}

	//
	// Configuration
	//

	public TranslationConfiguration getConfiguration() {
		return configuration;
	}

	//
	// createMethodInfoFromHandler
	//

	public MethodInfo createMethodInfoFromHandler(String signature,
			String mhandler, boolean reportError, boolean isGeneric) {
		final IMethod method = (IMethod) HandlerUtil.createElementFromHandler(mhandler); //
		if (method == null) {
			configuration.getLogger().logError(
					"TranslateInfo.getMethodInfoFromHandler::Can't find "
							+ mhandler);
			return null;
		}

		final IType type = method.getDeclaringType();
		final IPackageFragment pfragment = type.getPackageFragment();
		final PackageInfo packageInfo = getPackage(pfragment.getElementName(),
				type.getJavaProject().getProject());
		if (packageInfo == null) {
			return null;
		}
		try {
			final ClassInfo cInfo = findClassInfo(packageInfo, type, isGeneric,
					false);
			if (cInfo != null) {
				if (method.isStructureKnown()) {
					MethodInfo mInfo = null;
					if (cInfo.isConstructor(method)) {
						mInfo = cInfo.getMethod(null, method);
					} else {
						mInfo = cInfo.getMethod(signature, method);
					}
					if (mInfo == null) {
						mInfo = cInfo.resolveMethod(method.getElementName(),
								method.getParameterTypes());
					}
					return mInfo;
				}
			}
		} catch (final JavaModelException e) {
			e.printStackTrace();
			getConfiguration().getLogger().logException(
					"createMethodInfoFromHandler", e);
			return null;
		}
		return null;
	}

	//
	// mapping from comments
	//

	public void extractMappingFromComments(ClassInfo ci, String comments,
			MappingsInfo mappingsInfo) throws JavaModelException {
		if (mappingsInfo == null)
			mappingsInfo = generatedMappingsInfo;
		mappingsInfo.extractMappingFromComments(ci, comments);
	}

	public void extractFieldMappingFromComments(ClassInfo ci, IField field,
			String comments, MappingsInfo mappingsInfo)
			throws JavaModelException {
		if (mappingsInfo == null)
			mappingsInfo = generatedMappingsInfo;
		mappingsInfo.extractFieldMappingFromComments(ci, field, comments);
	}

	public void extractMethodMappingFromComments(ClassInfo ci, IMethod method,
			String comments, MappingsInfo mappingsInfo)
			throws JavaModelException {
		if (mappingsInfo == null)
			mappingsInfo = generatedMappingsInfo;
		mappingsInfo.extractMethodMappingFromComments(ci, method, comments);
	}

	//
	// Implicit Field Rename
	//

	public void addImplicitFieldRename(String packageName, String className,
			String oldName, String newName) {
		generatedMappingsInfo.addImplicitFieldRename(packageName, className,
				oldName, newName);
	}

	public String getFieldRename(String name) {
		return generatedMappingsInfo.getFieldRename(name);
	}

	//
	// addImplicit
	//

	public void addImplicitProperty(String packageName, String className,
			PropertyInfo info) {
		generatedMappingsInfo.addImplicitProperty(packageName, className, info);
	}

	public void addImplicitNestedToInnerTransformation(String pckName,
			String className) {
		generatedMappingsInfo.addImplicitNestedToInnerTransformation(pckName,
				className);
	}

	public void addImplicitConstantsRename(String packageName, String name,
			String newClassName, String[] elements) {
		generatedMappingsInfo.addImplicitConstantsRename(packageName, name,
				newClassName, elements);
	}

	public void addImplicitNestedRename(String newPackageName, String newName,
			String oldPackageName, String oldName) {
		generatedMappingsInfo.addImplicitNestedRename(newPackageName, newName,
				oldPackageName, oldName);
	}

	public void addImplicitPackageRename(String oldPackageName,String newPackageName) {
		generatedMappingsInfo.addImplicitPackageRename(oldPackageName,newPackageName);
	}
	
	public void addImplicitMethodOverride(String oldPackageName,
			String oldClassName, MethodInfo method) {
		generatedMappingsInfo.addImplicitMethodOverride(oldPackageName,
				oldClassName, method);
	}

	public boolean isAnImplicitNestedRename(String packageName, String newName) {
		return generatedMappingsInfo.isAnImplicitNestedRename(packageName,
				newName);
	}

	public MappingsInfo.NestedRename getImplicitNestedRename(String className) {
		return generatedMappingsInfo.getImplicitNestedRename(className);
	}

	public void generateImplictMappingFile(boolean isXML) throws Exception {
		generatedMappingsInfo.generateImplicitMappingFile(isXML);
	}

	//
	// JavaDoc Mappings
	//

	public HashMap<String, String> getJavaDocMappings() {
		final HashMap<String, String> res = new HashMap<String, String>();
		for (final MappingsInfo mInfo : mappingsList) {
			final HashMap<String, String> coll = mInfo.getJavaDocMappings();
			if (coll != null)
				res.putAll(coll);
		}
		return res;
	}

	public String getJavaDocTagMapping(String tag) {
		for (final MappingsInfo mInfo : mappingsList) {
			final String coll = mInfo.getJavaDocTagMapping(tag);
			if (coll != null)
				return coll;
		}
		return null;
	}

	//
	// Disclaimer
	//

	public String getDisclaimer() {
		List<String> res = new ArrayList<String>();
		for (final MappingsInfo mInfo : mappingsList) {
			final String disclaimer = mInfo.getDisclaimer();
			if (disclaimer != null)
				res.add(disclaimer);
		}
		if (res.size() == 1)
			return res.get(0);
		if (res.size() > 1)
			return res.get(res.size() - 1); // TODO
		return null;
	}
}
