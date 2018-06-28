package com.ilog.translator.java2cs.translation;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Set;
import java.util.TreeSet;

import org.eclipse.core.resources.IProject;
import org.eclipse.jdt.core.ICompilationUnit;
import org.eclipse.jdt.core.IImportDeclaration;
import org.eclipse.jdt.core.IJavaProject;
import org.eclipse.jdt.core.IPackageFragment;
import org.eclipse.jdt.core.IType;
import org.eclipse.jdt.core.JavaModelException;
import org.eclipse.jdt.core.dom.BodyDeclaration;
import org.eclipse.jdt.core.dom.ITypeBinding;
import org.eclipse.jdt.core.dom.MethodDeclaration;
import org.eclipse.jdt.core.dom.Modifier;
import org.eclipse.jdt.core.dom.SingleVariableDeclaration;
import org.eclipse.jdt.core.dom.TagElement;
import org.eclipse.jdt.core.dom.TextElement;
import org.eclipse.jdt.core.dom.Type;
import org.eclipse.jdt.core.dom.TypeDeclaration;
import org.eclipse.jdt.core.search.SearchMatch;
import org.eclipse.jdt.internal.corext.util.JavaModelUtil;
import org.eclipse.ltk.core.refactoring.Change;

import com.ilog.translator.java2cs.configuration.ITranslationModel;
import com.ilog.translator.java2cs.configuration.Java2CsModel;
import com.ilog.translator.java2cs.configuration.Logger;
import com.ilog.translator.java2cs.configuration.TranslationConfiguration;
import com.ilog.translator.java2cs.configuration.info.ClassInfo;
import com.ilog.translator.java2cs.configuration.info.MethodInfo;
import com.ilog.translator.java2cs.configuration.info.PropertyInfo;
import com.ilog.translator.java2cs.configuration.info.TranslationModelException;
import com.ilog.translator.java2cs.configuration.target.TargetClass;
import com.ilog.translator.java2cs.configuration.target.TargetPackage;
import com.ilog.translator.java2cs.configuration.target.TargetProperty;
import com.ilog.translator.java2cs.translation.noderewriter.PropertyRewriter;
//import com.ilog.translator.java2cs.translation.util.InheritanceHierarchy;
import com.ilog.translator.java2cs.translation.util.TranslationUtils;
import com.ilog.translator.java2cs.util.TranslationModelUtil;
import com.ilog.translator.java2cs.util.Utils;

public class TranslationContext implements ITranslationContext {

	private static final String SYSTEM_COMPONENT_MODEL = "System.ComponentModel.*";
	private static final String SYSTEM_RUNTIME_COMPILER_SERVICES = "System.Runtime.CompilerServices.*";
	private static final String SYSTEM_COLLECTIONS_GENERIC = "System.Collections.Generic.*";
	private static final String SYSTEM_COLLECTIONS = "System.Collections.*";
	private static final String SYSTEM_IO = "System.IO.*";
	private static final String SYSTEM = "System.*";

	//
	//
	//
	private HashMap<String, String> packagesNames = new HashMap<String, String>();
	private HashMap<ICompilationUnit, Change> changes = new HashMap<ICompilationUnit, Change>();
	private HashMap<String, String> synchronizedMedhods = new HashMap<String, String>();
	private HashMap<String, String> serializableTypes = new HashMap<String, String>();
	private HashMap<String, Object[] /* List<String>*/ > superContructorInvok = new HashMap<String, Object[] /*List<String>*/>();
	private HashMap<String, List<String>> thisContructorInvok = new HashMap<String, List<String>>();
	private HashMap<String, Set<String>> enclosingAccessList = new HashMap<String, Set<String>>();
	private HashMap<String, List<CovarianceData>> covarianceList = new HashMap<String, List<CovarianceData>>();
	private Set<String> genericList = new TreeSet<String>();
	private List<String> ignorableClasses = new ArrayList<String>();
	private List<String> tests = new ArrayList<String>();
	private HashMap<String, String> classesThatNeedImplementation = new HashMap<String, String>();
	private HashMap<String, String> publicDocumentedClasses = new HashMap<String, String>();
	private List<String> publicDocumentedPackage = new ArrayList<String>();
	private List<String> extensionsClasses = new ArrayList<String>();

	//
	// namespaces
	//
	private HashMap<String, Set<String>> namespaces = new HashMap<String, Set<String>>();
	private Set<String> defaultImports = new TreeSet<String>();
	private HashMap<ICompilationUnit, IImportDeclaration[]> importsList = new HashMap<ICompilationUnit, IImportDeclaration[]>();
	private HashMap<ICompilationUnit, List<String>> genericImportsList = new HashMap<ICompilationUnit, List<String>>();

	//
	//
	//
	private TranslationConfiguration configuration;
	private IMapper mapper;
	private ITranslationModel model;
	private Logger logger;

	//
	//
	//
	//private InheritanceHierarchy typeHierarchy = new InheritanceHierarchy();

	//
	// Constructor
	//

	public TranslationContext(IJavaProject project,
			TranslationConfiguration configuration) {
		this.configuration = configuration;
		logger = configuration.getLogger();
		mapper = new Java2CsMapper(this);
		model = new Java2CsModel(project, logger, configuration);
		init();
	}

	@Override
	public void finalize() {
		mapper = null;
		model = null;
		//
		packagesNames = null;
		changes = null;
		synchronizedMedhods = null;
		serializableTypes = null;
		superContructorInvok = null;
		thisContructorInvok = null;
		enclosingAccessList = null;
		covarianceList = null;
		genericList = null;
		ignorableClasses = null;
		tests = null;
		classesThatNeedImplementation = null;
		publicDocumentedClasses = null;
		publicDocumentedPackage = null;
		extensionsClasses = null;
		//
		namespaces = null;
		defaultImports = null;
		importsList = null;
		genericImportsList = null;
	}

	//
	// init
	//

	private final void init() {
		defaultImports.add(SYSTEM);
		defaultImports.add(SYSTEM_COLLECTIONS);
		defaultImports.add(SYSTEM_IO);
		if (configuration.getOptions().isUseGenerics())
			defaultImports.add(SYSTEM_COLLECTIONS_GENERIC);
		defaultImports.add(SYSTEM_RUNTIME_COMPILER_SERVICES);
		defaultImports.add(SYSTEM_COMPONENT_MODEL);
		//
		model.initialize();
	}

	//
	// type hierarchy
	//

	/*public InheritanceHierarchy getTypeHierarchy() {
		return typeHierarchy;
	}*/

	//
	// propagate
	//

	public void propagate() {
		model.propagate(this);
	}

	//
	// logger
	//

	public Logger getLogger() {
		return logger;
	}

	//
	// mapper
	//

	public IMapper getMapper() {
		return mapper;
	}

	//
	// model
	//

	public ITranslationModel getModel() {
		return model;
	}

	//
	// Configuration
	//

	public TranslationConfiguration getConfiguration() {
		return configuration;
	}

	//
	// Namespace / package
	//

	public boolean hasPackage(ICompilationUnit cu) {
		return packagesNames.get(cu.getPath().toString()) != null;
	}

	public String getPackageName(String cName, IProject reference) {
		final String pckName = packagesNames.get(cName);
		if (pckName != null) {
			if (configuration.getOptions().isExactDirectoryName()) {
				final TargetPackage tpck = model.findPackageMapping(pckName,
						reference);
				if (tpck != null) {
					return tpck.getName();
				} else {
					return TranslationUtils.defaultMappingForPackage(this,
							pckName, reference);
				}
			}
		}
		return pckName;
	}

	public void addPackageName(String cName, String pName) {
		packagesNames.put(cName, pName);
	}

	//
	// Change
	//

	public void addChange(ICompilationUnit cu, Change change) {
		changes.put(cu, change);
	}

	public Change getChange(ICompilationUnit cu) {
		return changes.get(cu);
	}

	public void clearChange() {
		changes = new HashMap<ICompilationUnit, Change>();
	}

	//
	// super/this constructor invocation management
	//

	public void addSuperConstructorInvoc(MethodDeclaration node,
			List<String> arguments, String pattern) {				
		superContructorInvok.put(getUniqID(node), new Object[] { arguments, pattern });
	}

	public Object[] /* List<String>*/ getSuperConstructorInvoc(MethodDeclaration node) {
		return superContructorInvok.get(getUniqID(node));
	}

	public void removeSuperConstructorInvoc(MethodDeclaration node) {
		superContructorInvok.remove(getUniqID(node));
	}

	public void removeThisConstructorInvoc(MethodDeclaration node) {
		thisContructorInvok.remove(getUniqID(node));
	}

	public void addThisConstructorInvoc(MethodDeclaration node,
			List<String> arguments) {
		thisContructorInvok.put(getUniqID(node), arguments);
	}

	public List<String> getThisConstructorInvoc(MethodDeclaration node) {
		return thisContructorInvok.get(getUniqID(node));
	}

	//
	// synchronized management
	//

	public void addSynchronized(MethodDeclaration node) {
		final String id = getUniqID(node);
		synchronizedMedhods.put(id, id);
	}

	public boolean isSynchronized(MethodDeclaration node) {
		final String id = getUniqID(node);
		final Object val = synchronizedMedhods.get(id);
		return (val != null);
	}

	//
	// serializable management
	//

	public void addSerializable(TypeDeclaration node) {
		final String id = getUniqID(node);
		serializableTypes.put(id, id);
	}

	public boolean isSerializable(TypeDeclaration node) {
		final String id = getUniqID(node);
		final Object val = serializableTypes.get(id);
		return (val != null);
	}

	//
	// getHandlerFromDoc
	//
	@SuppressWarnings("unchecked")
	public String getHandlerFromDoc(BodyDeclaration node, boolean renamed) {
		String key = null;
		String key2 = null;
		if (node.getJavadoc() != null) {
			final String handlerTag = mapper
					.getTag(TranslationModelUtil.HANDLER_TAG);
			final List tags = node.getJavadoc().tags();
			for (int i = 0; i < tags.size(); i++) {
				final TagElement te = (TagElement) tags.get(i);
				final String name = te.getTagName();
				if ((name != null) && name.equals(handlerTag)) {
					key = "";
					for (int j = 0; j < te.fragments().size(); j++) {
						TextElement text = (TextElement) te.fragments().get(j);
						key += text.getText().trim();
					}
				}
				if ((name != null) && name.equals(handlerTag + "2")) {
					key2 = "";
					for (int j = 0; j < te.fragments().size(); j++) {
						TextElement text = (TextElement) te.fragments().get(j);
						key2 += text.getText().trim();
					}
				}
			}
		}

		if (renamed) {
			return key2 == null ? Utils.unmangle(key) : Utils.unmangle(key2);
		} else {
			return Utils.unmangle(key);
		}
	}

	@SuppressWarnings("unchecked")
	public String getSignatureFromDoc(BodyDeclaration node, boolean renamed) {
		String sign = null;
		String sign2 = null;
		if (node.getJavadoc() != null) {
			final String signatureTag = mapper
					.getTag(TranslationModelUtil.SIGNATURE_TAG);
			final List tags = node.getJavadoc().tags();
			for (int i = 0; i < tags.size(); i++) {
				final TagElement te = (TagElement) tags.get(i);
				final String name = te.getTagName();
				if ((name != null) && name.equals(signatureTag)) {
					sign = "";
					for (int j = 0; j < te.fragments().size(); j++) {
						TextElement text = (TextElement) te.fragments().get(j);
						sign += text.getText().trim();
					}
				}
				if ((name != null) && name.equals(signatureTag + "2")) {
					sign2 = "";
					for (int j = 0; j < te.fragments().size(); j++) {
						TextElement text = (TextElement) te.fragments().get(j);
						sign2 += text.getText().trim();
					}
				}
			}
		}
		if (renamed) {
			return sign2 == null ? sign : sign2;
		} else {
			return sign;
		}
	}

	private String getUniqID(BodyDeclaration node) {
		return getHandlerFromDoc(node, false);
	}

	//
	// namespace
	//

	public void addNamespaces(String typeName, Set<String> arg) {
		if (arg == null || arg.size() == 0)
			return;
		Set<String> ns = namespaces.get(typeName);
		if (ns == null) {
			ns = new TreeSet<String>();
			namespaces.put(typeName, ns);
		}
		ns.addAll(arg);
	}

	public Set<String> getNamespaces(String typeName) {
		return namespaces.get(typeName);
	}

	public Set<String> getDefaultNamespaces() {
		return defaultImports;
	}

	//
	// imports
	//

	public IImportDeclaration[] getImports(ICompilationUnit cu) {
		return importsList.get(getCuName(cu));
	}

	public List<String> getGenericImports(ICompilationUnit cu) {
		return genericImportsList.get(getCuName(cu));
	}

	private ICompilationUnit getCuName(ICompilationUnit cu) {
		return cu;
	}

	public void addImports(ICompilationUnit icunit, IImportDeclaration[] imports) {
		final List<IImportDeclaration> res = new ArrayList<IImportDeclaration>();
		for (final IImportDeclaration impDecl : imports) {
			final String name = impDecl.getElementName();
			try {
				final IType type = TranslationUtils.findType(getConfiguration()
						.getWorkingProject(), name);
				if (type != null && type.getTypeParameters().length != 0) {
					addGenericType(type.getFullyQualifiedName(), type
							.getJavaProject().getProject());
					if (!impDecl.isOnDemand()) { // no ".*"
						final int idx = name.lastIndexOf(".");
						final String pckName = name.substring(0, idx);
						if (type.isMember()) { // pck.Class .InnerClass
							final String id = type.getHandleIdentifier();
							final TargetClass tc = getModel().findClassMapping(
									id, false, true);
							if (tc != null && tc.getName() != null)
								addGenericImport(icunit, tc.getName() + ".*");
							continue;
						} else {
							final TargetPackage tpck = getModel()
									.findPackageMapping(pckName,
											type.getJavaProject().getProject());
							if (tpck != null && tpck.getName() != null)
								addGenericImport(icunit, tpck.getName() + ".*");
							else
								addGenericImport(icunit, pckName + ".*");
						}
					}
				}
				if (type != null) {
					final String id = type.getHandleIdentifier();
					final TargetClass tc = getModel().findClassMapping(id,
							false, true);
					if (tc != null) {
						if (tc.isTranslated() && tc.isRemoveFromImport()) {
							continue;
						}
					}
				}
				res.add(impDecl); // filter generic
			} catch (final JavaModelException e) {
				getLogger().logException("addImports", e);
			}
		}
		importsList.put(getCuName(icunit), res
				.toArray(new IImportDeclaration[res.size()]));
	}

	public void addGenericImport(ICompilationUnit icunit, String imports) {
		List<String> imps = genericImportsList.get(getCuName(icunit));
		if (imps == null) {
			imps = new ArrayList<String>();
			genericImportsList.put(getCuName(icunit), imps);
		}
		imps.add(imports);
	}

	public void cleanImports(ICompilationUnit cu) {
		importsList.remove(getCuName(cu));
	}

	//
	// ignorable
	//

	public void addToIgnorable(ClassInfo info) {
		ignorableClasses.add(info.getType().getFullyQualifiedName().replace(
				'$', '.'));
	}

	public boolean ignore(ICompilationUnit icunit) throws JavaModelException {
		final IType primaryType = icunit.findPrimaryType();
		if (primaryType == null)
			return false;
		final IPackageFragment frag = primaryType.getPackageFragment();
		String packageName = "";
		if (frag != null) {
			packageName = frag.getElementName();
		}

		String className = primaryType.getElementName();

		final String cuName = getModel()
				.resolveTypeName(packageName, className);
		if (cuName != null) {
			final int idx = cuName.lastIndexOf(".");
			className = cuName.substring(idx + 1);
			packageName = cuName.substring(0, idx);
		}

		final String[] packageFilter = configuration.getOptions()
				.getPackageFilter();
		final String[] classFilter = configuration.getOptions()
				.getClassFilter();

		if (packageFilter != null) {
			if (classFilter != null) {
				return !contains(cuName, classFilter)
						|| !contains(packageName, packageFilter)
						|| this.ignore(className, packageName, icunit
								.getJavaProject().getProject());
			} else {
				return !contains(packageName, packageFilter)
						|| this.ignore(className, packageName, icunit
								.getJavaProject().getProject());
			}
		} else if (classFilter != null) {
			return !contains(cuName, classFilter)
					|| this.ignore(className, packageName, icunit
							.getJavaProject().getProject());
		} else {
			return this.ignore(className, packageName, icunit.getJavaProject()
					.getProject());
		}
	}

	private boolean contains(String pattern, String[] array) {
		for (final String s : array) {
			if (pattern.contains(s)) {
				return true;
			}
		}
		return false;
	}

	private boolean ignore(String className, String packageName,
			IProject reference) {
		boolean res = false;
		if ((packageName != null) && !packageName.equals("")) {
			if (mapper.isRemovePackage(packageName, reference)) {
				res = true;
			} else {
				final String typeName = getModel().resolveTypeName(packageName,
						className);
				res = ignorableClasses.contains(typeName);
			}
		} else {
			res = ignorableClasses.contains(className);
		}
		if (!res
				&& className.endsWith(mapper.getPrefixForConstants()
						+ JavaModelUtil.DEFAULT_CU_SUFFIX)) {
			final int index = className.lastIndexOf(mapper
					.getPrefixForConstants()
					+ JavaModelUtil.DEFAULT_CU_SUFFIX);
			if (index > 0) {
				className = className.substring(0, index);
			}
			return this.ignore(className + JavaModelUtil.DEFAULT_CU_SUFFIX,
					packageName, reference);
		}
		return res;
	}

	public boolean ignorablePackage(String packageName, IProject reference) {
		if ((packageName != null) && !packageName.equals("")) {
			return mapper.isRemovePackage(packageName, reference);
		}
		return false;
	}

	//
	// enclosing access
	//

	public boolean hasEnclosingAccess(String innerName, String enclosingName) {
		final Set<String> list = enclosingAccessList.get(innerName);
		if (list != null) {
			return list.contains(enclosingName);
		}
		return false;
	}

	public void resetInnerEnclosingAccess() {
		enclosingAccessList = null;
	}

	public void addInnerEnclosingAccess(String innerName, String enclosingName) {
		Set<String> list = enclosingAccessList.get(innerName);
		if (list != null) {
			list.add(enclosingName);
		} else {
			list = new TreeSet<String>();
			list.add(enclosingName);
			enclosingAccessList.put(innerName, list);
		}
	}

	public int innerEnclosingAccessListSize() {
		return enclosingAccessList.size();
	}

	//
	// generic type
	//

	public void addGenericType(String fullyQualifiedName, IProject reference) {
		final int index = fullyQualifiedName.lastIndexOf(".");
		final TargetPackage map = model.findPackageMapping(fullyQualifiedName
				.substring(0, index), reference);
		String pckName = null;
		if (map == null) {
			pckName = TranslationUtils.defaultMappingForPackage(this,
					fullyQualifiedName.substring(0, index), reference);
		} else {
			pckName = map.getName();
		}
		genericList.add(pckName
				+ fullyQualifiedName.substring(index).replace("$", "."));
	}

	public boolean isGenericType(String fullyQualifiedName) {
		return genericList.contains(fullyQualifiedName);
	}

	//
	// covariance
	//

	public void addCovariance(List<SearchMatch> ref, String returnType) {
		for (final SearchMatch match : ref) {
			final String name = match.getResource().getName();
			List<CovarianceData> l = covarianceList.get(name);
			if (l == null) {
				l = new ArrayList<CovarianceData>();
				covarianceList.put(name, l);
			}
			l.add(new CovarianceData(match, returnType));
		}
	}

	public List<CovarianceData> getCovariance(ICompilationUnit cu) {
		final String name = cu.getResource().getName();
		return covarianceList.get(name);
	}

	public void clearCovariance() {
		covarianceList.clear();
		covarianceList = new HashMap<String, List<CovarianceData>>();
	}

	//
	// mapping from comments
	//

	public void extractMappingFromComments(ClassInfo ci,
			TypeDeclaration typeDecl) {
		try {
			getModel().extractMappingFromComments(ci, typeDecl, false, true);
		} catch (final JavaModelException e) {
			e.printStackTrace();
			getLogger().logException("extractMappingFromComments", e);
		}
	}

	public void extractMappingFromCommentsToFile(TypeDeclaration typeDecl) {
		try {
			final ClassInfo ci = getModel().findClassInfo(
					typeDecl.resolveBinding().getJavaElement()
							.getHandleIdentifier(), true, true, true);
			if (ci != null)
				getModel().extractMappingFromComments(ci, typeDecl, true, true);
		} catch (final Exception e) {
			e.printStackTrace();
			getLogger().logException("extractMappingFromCommentsToFile", e);
		}
	}

	//
	// test
	//

	public boolean isATest(ICompilationUnit icunit) {
		if (getConfiguration().getOptions().isMavenStructure()) {
			final String path = icunit.getPath().toString();
			if (path.contains("src/test/"))
				return true;
		}
		return tests.contains(icunit.getElementName());
	}

	public void declareAsTest(ICompilationUnit icunit) {
		tests.add(icunit.getElementName());
	}

	//
	//
	//

	public void buildClassInfoAndScanMapping(TypeDeclaration typeDecl,
			boolean scan) {
		try {
			final int length = typeDecl.getTypes().length;
			if (typeDecl.getTypes() != null && length > 0) {
				for (int j = 0; j < length; j++) {
					final TypeDeclaration currentInner = typeDecl.getTypes()[j];
					final ClassInfo ci = getModel().findClassInfo(
							currentInner.resolveBinding().getJavaElement()
									.getHandleIdentifier(),
							true,
							true,
							TranslationUtils.isGeneric(currentInner
									.resolveBinding()));
					if (scan)
						extractMappingFromComments(ci, currentInner);
				}
			}
			final ClassInfo ci = getModel().findClassInfo(
					typeDecl.resolveBinding().getJavaElement()
							.getHandleIdentifier(), true, true,
					TranslationUtils.isGeneric(typeDecl.resolveBinding()));
			if (ci != null)
				if (scan)
					extractMappingFromComments(ci, typeDecl);
			/*
			 * else getLogger().logWarning( "Class <" +
			 * typeDecl.resolveBinding().getName() +
			 * "> could not be found. Handler is " + typeDecl.resolveBinding()
			 * .getJavaElement() .getHandleIdentifier());
			 */
		} catch (final JavaModelException e) {
			e.printStackTrace();
			getLogger().logException("buildClassInfoAndScanMapping", e);
		} catch (final TranslationModelException e) {
			e.printStackTrace();
			getLogger().logException("buildClassInfoAndScanMapping", e);
		}
	}

	//
	// extension class
	//

	public void addExtensionClassName(String className) {
		extensionsClasses.add(className);
	}

	public boolean IsAnExtensionClassName(String className) {
		return extensionsClasses.contains(className);
	}

	//
	// code to Add to implementation
	//

	public String getCodeToAddToImplementation(String handler) {
		return classesThatNeedImplementation.get(handler);
	}

	public void markClassForImplementationAdd(String handleIdentifier,
			String code) {
		classesThatNeedImplementation.put(handleIdentifier, code);
	}

	//
	// addClassToPublicDocumented
	//

	/**
	 * Add the given class names to the list of class marked as public
	 * documented
	 * 
	 * @param qualifiedName
	 *            The fully qualified name of the class
	 */
	public void addClassToPublicDocumented(String qualifiedName, String handler) {
		publicDocumentedClasses.put(qualifiedName, handler);
	}

	/**
	 * Add the given package names to the list of package marked as NOT public
	 * documented
	 * 
	 * @param qualifiedName
	 *            The fully qualified name of the class
	 */
	public void addPackageToPublicDocumented(String elementName) {
		getLogger().logInfo(
				"adding package " + elementName
						+ " to public documented package");
		publicDocumentedPackage.add(elementName);
	}

	/**
	 * List all public classes in a documented package (means with a package.html) and not marked as "internal"
	 * Don't know if it's a common usage outside ILOG to find "public documented package"
	 * This is useful to pass that list to sandcastle ...
	 * 
	 */
	public void listPublicDocumentedFiles() {
		List<String> result = new ArrayList<String>();
		for (String className : publicDocumentedClasses.keySet()) {
			for (String pckName : publicDocumentedPackage)
				if (isInPackage(className, pckName)) {
					TargetClass target = getModel()
							.findClassMapping(
									publicDocumentedClasses.get(className),
									false, true);
					if (target != null && target.getPackageName() != null
							&& target.getName() != null) {
						result.add(target.getPackageName() + "."
								+ target.getName());
					} else {
						TargetPackage pck = getModel().findPackageMapping(
								pckName, null);
						if (pck != null) {
							int index = className.lastIndexOf(".");
							result.add(pck.getName() + "."
									+ className.substring(index + 1));
						} else {
							result.add(className);
						}
					}
				}
		}
		//
		getLogger().logInfo(" --- public API ---");
		for (String className : result) {
			getLogger().logInfo(
					"   <filter entryType=\"Class\" fullName=\"" + className
							+ "\" isExposed=\"True\">");
		}
		getLogger().logInfo("");
	}

	/**
	 * Check if the given class is in the given package
	 * @param className
	 * @param pckName
	 * @return
	 */
	private boolean isInPackage(String className, String pckName) {
		return className.startsWith(pckName)
				&& (className.substring(pckName.length() + 1).indexOf(".") < 0);
	}
	
	//
	// autoPropertiesSearch
	//
	
	public void autoPropertiesSearch(TypeDeclaration typeDecl)  {
		try {
		final ClassInfo ci = getModel().findClassInfo(
				typeDecl.resolveBinding().getJavaElement()
						.getHandleIdentifier(), true, true,
				TranslationUtils.isGeneric(typeDecl.resolveBinding()));
		String targetFramework = getConfiguration().getOptions().getTargetDotNetFramework().name();
		
		if (ci != null && ci.getTarget(targetFramework) != null && ci.getTarget(targetFramework).isAutoProperty()) {			
				autoGetSetSearch(typeDecl, ci);			
		}
		} catch(TranslationModelException e) {
			getLogger().logException("", e);
		} catch(JavaModelException e) {
			getLogger().logException("", e);
		}
	}

	private void autoGetSetSearch(TypeDeclaration typeDecl, ClassInfo ci)
	throws TranslationModelException, JavaModelException {
		if (typeDecl.getTypes() != null && typeDecl.getTypes().length > 0) {
			for (int j = 0; j < typeDecl.getTypes().length; j++) {
				final TypeDeclaration currentInner = typeDecl.getTypes()[j];
				autoGetSetSearch(currentInner);
			}
		}
		List<MethodDeclaration> getMethods = getGetOrIsMethods(typeDecl);
		List<MethodDeclaration> setMethods = getSetMethods(typeDecl);

		final List<PropertyInfo> properties = findProperties(typeDecl,
				getMethods, setMethods);

		ci.addProperties(properties);
	}
	
	public void autoGetSetSearch(TypeDeclaration typeDecl)
			throws TranslationModelException, JavaModelException {

		final ClassInfo ci = getModel().findClassInfo(
				typeDecl.resolveBinding().getJavaElement()
						.getHandleIdentifier(), true, true,
				TranslationUtils.isGeneric(typeDecl.resolveBinding()));

		autoGetSetSearch(typeDecl, ci);
	}

	private List<PropertyInfo> findProperties(TypeDeclaration typeDecl,
			List<MethodDeclaration> getMethods,
			List<MethodDeclaration> setMethods) {
		final List<PropertyInfo> properties = new ArrayList<PropertyInfo>();
		final List<MethodDeclaration> usedSetter = new ArrayList<MethodDeclaration>();
		for (final MethodDeclaration getMethod : getMethods) {
			if ((getMethod.resolveBinding() != null)
					&& !getMethod.resolveBinding().isGenericMethod()) {
				String name = null;
				if (getMethod.getName().getIdentifier().startsWith("get"))
					name = getMethod.getName().getIdentifier().substring(3);
				else
					// handle isXXX
					name = getMethod.getName().getIdentifier().substring(2);
				final MethodInfo gMi = getMethodInfo(typeDecl, getMethod);
				// TODO if (gMi.getTargetMethod() == null)
				String targetFramework = getConfiguration().getOptions().getTargetDotNetFramework().name();
					
				gMi.addTargetMethod(targetFramework, new TargetProperty(name,
						PropertyRewriter.ProperyKind.READ));
				boolean foundSet = false;
				for (final MethodDeclaration setMethod : setMethods) {
					if (areProperty(getMethod, setMethod)) {
						final MethodInfo sMi = getMethodInfo(typeDecl,
								setMethod);
						// TODO if (sMi.getTargetMethod() == null)
						sMi.addTargetMethod(targetFramework, new TargetProperty(name,
								PropertyRewriter.ProperyKind.WRITE));
						properties.add(new PropertyInfo(name,
								PropertyRewriter.ProperyKind.READ_WRITE, gMi,
								sMi));
						foundSet = true;
						usedSetter.add(setMethod);
					}
				}
				if (!foundSet) {
					properties.add(new PropertyInfo(name,
							PropertyRewriter.ProperyKind.READ, gMi));
				}
			}
		}
		//
		setMethods.removeAll(usedSetter);
		for (final MethodDeclaration setMethod : setMethods) {
			if (!setMethod.resolveBinding().isGenericMethod()) {
				final String name = setMethod.getName().getIdentifier()
						.substring(3);
				final MethodInfo sMi = getMethodInfo(typeDecl, setMethod);
				String targetFramework = getConfiguration().getOptions().getTargetDotNetFramework().name();
				sMi.addTargetMethod(targetFramework, new TargetProperty(name,
						PropertyRewriter.ProperyKind.WRITE));
				properties.add(new PropertyInfo(name,
						PropertyRewriter.ProperyKind.WRITE, sMi));
			}
		}
		return properties;
	}

	private MethodInfo getMethodInfo(TypeDeclaration typeDecl,
			MethodDeclaration method) {
		if (method.resolveBinding() != null
				&& method.resolveBinding().getJavaElement() != null) {
			String handler = method.resolveBinding().getJavaElement()
					.getHandleIdentifier();
			String className = typeDecl.resolveBinding().getQualifiedName();
			try {
				return getModel().findMethodInfo(className, null,
						handler,
						TranslationUtils.isGeneric(typeDecl.resolveBinding()));
			} catch (Exception e) {
				e.printStackTrace();
				getLogger().logException("", e);
			}
		}
		return null;
	}

	private boolean areProperty(MethodDeclaration getMethod,
			MethodDeclaration setMethod) {
		String gname = getMethod.getName().getIdentifier().substring(3);
		final String sname = setMethod.getName().getIdentifier().substring(3);
		if (gname.equals(sname)) {
			final ITypeBinding retType = getMethod.resolveBinding()
					.getReturnType();
			final ITypeBinding parType = ((SingleVariableDeclaration) setMethod
					.parameters().get(0)).getType().resolveBinding();
			if (retType.isEqualTo(parType)) {
				return (Modifier.isStatic(getMethod.resolveBinding()
						.getModifiers()) && Modifier.isStatic(setMethod
						.resolveBinding().getModifiers()))
						|| (!Modifier.isStatic(getMethod.resolveBinding()
								.getModifiers()) && !Modifier
								.isStatic(setMethod.resolveBinding()
										.getModifiers()));
			}
		}
		// handle "isXXX"
		gname = getMethod.getName().getIdentifier().substring(2);
		if (gname.equals(sname)) {
			final ITypeBinding retType = getMethod.resolveBinding()
					.getReturnType();
			final ITypeBinding parType = ((SingleVariableDeclaration) setMethod
					.parameters().get(0)).getType().resolveBinding();
			if (retType.isEqualTo(parType)) {
				return (Modifier.isStatic(getMethod.resolveBinding()
						.getModifiers()) && Modifier.isStatic(setMethod
						.resolveBinding().getModifiers()))
						|| (!Modifier.isStatic(getMethod.resolveBinding()
								.getModifiers()) && !Modifier
								.isStatic(setMethod.resolveBinding()
										.getModifiers()));
			}
		}
		return false;
	}

	private List<MethodDeclaration> getSetMethods(TypeDeclaration typeDecl) {
		MethodDeclaration[] decls = typeDecl.getMethods();
		final List<MethodDeclaration> setMethods = new ArrayList<MethodDeclaration>();
		for (final MethodDeclaration mDecl : decls) {
			if (!mDecl.isConstructor()) {
				MethodInfo sMi = getMethodInfo(typeDecl, mDecl);
				if ((sMi == null) || (!sMi.isDisableAutoproperty())) {
					String mName = mDecl.getName().getIdentifier();
					if (mName.startsWith("set")
							&& mDecl.getName().getIdentifier().length() > 3
							&& hasNArguments(mDecl, 1)
							&& hasVoidReturnType(mDecl)) {
						setMethods.add(mDecl);
					}
				} else {
					getLogger()
							.logInfo(
									"[Custom Info] Method "
											+ typeDecl.getName()
													.getFullyQualifiedName()
											+ "."
											+ mDecl.getName()
											+ " will not be checked for property transformation.");
				}
			}
		}
		return setMethods;
	}

	private List<MethodDeclaration> getGetOrIsMethods(TypeDeclaration typeDecl) {
		MethodDeclaration[] decls = typeDecl.getMethods();
		final List<MethodDeclaration> getMethods = new ArrayList<MethodDeclaration>();
		for (final MethodDeclaration mDecl : decls) {
			if (!mDecl.isConstructor()) {
				MethodInfo sMi = getMethodInfo(typeDecl, mDecl);
				if ((sMi == null) || (!sMi.isDisableAutoproperty())) {
					String mName = mDecl.getName().getIdentifier();
					if (mName.startsWith("get") && mName.length() > 3
							&& hasNArguments(mDecl, 0) && hasReturnType(mDecl)) {
						getMethods.add(mDecl);
					} else if (mName.startsWith("is") && mName.length() > 2
							&& hasNArguments(mDecl, 0)
							&& hasBooleanReturnType(mDecl)) {
						getMethods.add(mDecl);
					}
				} else {
					getLogger()
							.logInfo(
									"[Custom Info] Method "
											+ typeDecl.getName()
													.getFullyQualifiedName()
											+ "."
											+ mDecl.getName()
											+ " will not be checked for property transformation.");
				}
			}
		}
		return getMethods;
	}

	private boolean hasReturnType(MethodDeclaration decl) {
		return decl.getReturnType2() != null;
	}

	private boolean hasVoidReturnType(MethodDeclaration decl) {
		final Type retType = decl.getReturnType2();
		return retType.resolveBinding().getName().equals("void");
	}

	private boolean hasBooleanReturnType(MethodDeclaration decl) {
		final Type retType = decl.getReturnType2();
		return retType.resolveBinding().getName().equals("boolean");
	}

	@SuppressWarnings("unchecked")
	private boolean hasNArguments(MethodDeclaration decl, int i) {
		final List parameters = decl.parameters();
		if (parameters == null) {
			return (i == 0);
		} else {
			return parameters.size() == i;
		}
	}

}
