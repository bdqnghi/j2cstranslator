package com.ilog.translator.java2cs.configuration;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

import org.eclipse.core.resources.IFile;
import org.eclipse.core.resources.IProject;
import org.eclipse.jdt.core.IField;
import org.eclipse.jdt.core.IJavaProject;
import org.eclipse.jdt.core.IMethod;
import org.eclipse.jdt.core.IType;
import org.eclipse.jdt.core.JavaModelException;
import org.eclipse.jdt.core.dom.FieldDeclaration;
import org.eclipse.jdt.core.dom.ITypeBinding;
import org.eclipse.jdt.core.dom.IVariableBinding;
import org.eclipse.jdt.core.dom.MethodDeclaration;
import org.eclipse.jdt.core.dom.TypeDeclaration;
import org.eclipse.jdt.core.dom.VariableDeclarationFragment;

import com.ilog.translator.java2cs.configuration.info.ClassInfo;
import com.ilog.translator.java2cs.configuration.info.FieldInfo;
import com.ilog.translator.java2cs.configuration.info.IndexerInfo;
import com.ilog.translator.java2cs.configuration.info.MappingsInfo;
import com.ilog.translator.java2cs.configuration.info.MethodInfo;
import com.ilog.translator.java2cs.configuration.info.PackageInfo;
import com.ilog.translator.java2cs.configuration.info.PropertyInfo;
import com.ilog.translator.java2cs.configuration.info.TranslateInfo;
import com.ilog.translator.java2cs.configuration.info.TranslationModelException;
import com.ilog.translator.java2cs.configuration.info.VariableInfo;
import com.ilog.translator.java2cs.configuration.options.DotNetFramework;
import com.ilog.translator.java2cs.configuration.options.JDK;
import com.ilog.translator.java2cs.configuration.target.TargetClass;
import com.ilog.translator.java2cs.configuration.target.TargetField;
import com.ilog.translator.java2cs.configuration.target.TargetIndexer;
import com.ilog.translator.java2cs.configuration.target.TargetMethod;
import com.ilog.translator.java2cs.configuration.target.TargetPackage;
import com.ilog.translator.java2cs.configuration.target.TargetProperty;
import com.ilog.translator.java2cs.translation.ITranslationContext;
import com.ilog.translator.java2cs.translation.util.TranslationUtils;
import com.ilog.translator.java2cs.util.IKeywordConstants;
import com.ilog.translator.java2cs.util.TranslationModelUtil;

public class Java2CsModel implements ITranslationModel {

	private TranslateInfo info;
	private DotNetFramework targetFramework = DotNetFramework.NET3_5; // default value
	private JDK sourceJDK = JDK.JDK1_5; // default value

	//
	//
	//

	/**
	 * 
	 */
	public Java2CsModel(IJavaProject project, Logger logger,
			TranslationConfiguration configuration) {
		info = new TranslateInfo(configuration);
		sourceJDK = configuration.getOptions().getSourceJDK();
		targetFramework = configuration.getOptions().getTargetDotNetFramework();
	}

	//
	// finalize
	//
	
	@Override
	public void finalize() {
		info = null;
	}
	
	//
	// initialize
	//

	public void initialize() {
		try {
			info.readJDKMappings();
			info.readProjectMappings();
		} catch (final Exception e) {
			info.getConfiguration().getLogger().logException(
					"Error during Initialization of Java2CsModel", e);
		}
	}
	
	//
	// migrate mapping file
	//

	public void migrateProjectMappingFiles() {
		try {
			// info.migrateJDKMappings("c:/temp/");
			info.migrateProjectMappings("c:/temp/");
		} catch (final Exception e) {
			info.getConfiguration().getLogger().logException(
					"Error during Initialization of Java2CsModel", e);
		}
	}
	
	public void migrateThisMappingFile(IFile file) {
		try {
			// info.migrateJDKMappings("c:/temp/");
			info.migrateThisMappingFile("c:/temp/", file);
		} catch (final Exception e) {
			info.getConfiguration().getLogger().logException(
					"Error during Initialization of Java2CsModel", e);
		}
	}
	
	public void migrateJDKMappingFiles() {
		try {
			info.migrateJDKMappings("c:/temp/");
			// info.migrateProjectMappings("c:/temp/");
		} catch (final Exception e) {
			info.getConfiguration().getLogger().logException(
					"Error during Initialization of Java2CsModel", e);
		}
	}

	//
	// propagate
	//

	public void propagate(ITranslationContext context) {
		try {
			info.propagate(context);
		} catch (final Exception e) {
			info.getConfiguration().getLogger().logException(
					"Error during propagate in Java2CsModel", e);
		}
	}

	//
	// generateImplicitMappingFile
	//

	public void generateImplicitMappingFile(boolean isXML) throws Exception {
		info.generateImplictMappingFile(isXML);
	}

	//
	// ---------------------------------------------------------------------
	//

	//
	// isRemovedPackage
	//

	public boolean isRemovedPackage(String packageName, IProject reference) {
		final PackageInfo pInfo = info.getPackage(packageName, reference);
		
		if (pInfo != null && pInfo.getTarget(targetFramework.name()) != null) {
			return pInfo.getTarget(targetFramework.name()).isTranslated();
		} else {
			return false;
		}
	}

	//
	// isRemovedOrInnerClass
	//

	public boolean isRemovedOrInnerClass(String cName, String pName,
			IProject reference) {
		try {
			final PackageInfo pInfo = info.getPackage(pName, reference);
			if (pInfo != null) {
				final ClassInfo ci = pInfo.getClass(pName + "." + cName); //$NON-NLS-1$
				if (ci != null && ci.getTarget(targetFramework.name()) != null) {
					return ci.getTarget(targetFramework.name()).isTranslated();
				}
			}
		} catch (final Exception e) {
			info.getConfiguration().getLogger().logException("", e);			
		}
		return false;
	}

	//
	// findPackageMapping
	//

	public TargetPackage findPackageMapping(String name, IProject reference) {
		final PackageInfo pInfo = findPackageInfo(name, reference);
		if (pInfo != null) {
			return pInfo.getTarget(targetFramework.name());
		}
		return null;
	}

	//
	// findPackageInfo
	//
	
	public PackageInfo findPackageInfo(String name, IProject reference) {
		final PackageInfo pInfo = info.getPackage(name, reference);
		return pInfo;
	}

	//
	// findImportMapping
	//

	public TargetPackage findImportMapping(String pck, IProject reference) {
		final PackageInfo pInfo = findPackageInfo(pck, reference);
		if (pInfo != null) {
			return pInfo.getTarget(targetFramework.name());
		}
		return null;
	}

	//
	// findPrimitiveClassMapping
	//

	public TargetClass findPrimitiveClassMapping(String name) {
		if (name.equals(IKeywordConstants.BOOLEAN_K)) {
			return TranslateInfo.BOOLEAN_TC;
		} else if (info.getConfiguration().getOptions().isMapByteToSByte()
				&& name.equals(IKeywordConstants.BYTE_K)) {
			return TranslateInfo.SBYTE_TC;
		} else {
			return null;
		}
	}

	/**
	 *  findClassInfo
	 */
	public ClassInfo findClassInfo(String handler, boolean reportError,
			boolean create, boolean isGeneric)
			throws TranslationModelException, JavaModelException {
		ClassInfo ci = info.getClassInfoFromHandler(handler, reportError,
				isGeneric);
		if (ci == null) {
			ci = info.createClassInfoFromHandler(handler, reportError, create,
					isGeneric);
		}
		return ci;
	}

	//
	// addClassInfo
	//
	public void addClassInfo(String name, IType type, ClassInfo nci) {
		final PackageInfo pi = info.getPackage(name, type.getJavaProject()
				.getProject());
		if (pi != null) {
			pi.addClass(type, nci, false);
		}
	}

	/**
	 * 
	 */
	public TargetClass findClassMapping(String handler, boolean reportError,
			boolean isGeneric) {
		final ClassInfo ci = info.getClassInfoFromHandler(handler, reportError,
				isGeneric);
		if (ci == null) {
			return null;
		} else {
			return ci.getTarget(targetFramework.name());
		}
	}

	/**
	 * 
	 */
	public TargetClass findGenericClassMapping(String handleIdentifier)
			throws TranslationModelException {
		final ClassInfo ci = info
				.getGenericClassInfoFromHandler(handleIdentifier);
		if (ci == null) {
			return null;
		} else {
			return ci.getTarget(targetFramework.name());
		}
	}

	/**
	 * 
	 * @param typeBinding
	 * @param handler
	 * @return
	 */
	public TargetClass findClassMapping(ITypeBinding typeBinding, String handler) {
		if (typeBinding == null) {
			return null;
		}

		if (typeBinding.isPrimitive()) {
			final String name = typeBinding.getName();
			return findPrimitiveClassMapping(name);
		}

		if (typeBinding.isTypeVariable()) {
			return null;
		}
		final String pckName = typeBinding.getTypeDeclaration().getPackage()
				.getName();
		final PackageInfo pInfo = info.getPackage(pckName, typeBinding
				.getJavaElement().getJavaProject().getProject());

		if (pInfo == null) {
			return null;
		}

		final IType type = (IType) typeBinding.getJavaElement();

		// If case of parametrized type e.g. "Collection<Integer>"
		// we only want to retrieve Type declaration e.g. "Collection"
		if (typeBinding.isParameterizedType()) {
			final ITypeBinding[] wild = typeBinding.getTypeArguments();
			String gname = "<"; //$NON-NLS-1$
			for (final ITypeBinding element : wild) {
				gname += "T"; //$NON-NLS-1$
			}
			gname += ">"; //$NON-NLS-1$

			/*
			 * ClassInfo ci = pInfo.getClass(n2 + gname); if (ci == null) { name =
			 * n2; } else { return ci.getTargetClass(); }
			 */
			return null;
		}
		final ClassInfo ci = pInfo.getClass(type);
		if (ci == null) {
			return null;
		} else {
			return ci.getTarget(targetFramework.name());
		}
	}

	//
	// array fields mapping
	//

	/**
	 * 
	 */
	public TargetField findArrayFieldMapping(ITypeBinding array, String name) {
		return info.getArrayField(name);
	}

	//
	// fields mapping
	//

	/**
	 * 
	 */
	public TargetField findFieldMapping(String fName, String handler) {
		final FieldInfo mi = info.getFieldInfoFromHandler(fName, handler);
		if (mi != null) {
			return mi.getTarget(targetFramework.name());
		} else {
			return null;
		}
	}

	//
	// Method
	//

	/**
	 * 
	 */
	public TargetMethod findConstructorMapping(String packageName,
			String className, String[] params, boolean b, String handler) {
		final MethodInfo ci = info.getConstructorInfo(packageName, className,
				params, b, handler);
		if (ci != null) {
			return ci.getTarget(targetFramework.name());
		}

		return null;
	}

	/**
	 * 
	 */
	public MethodInfo findMethodInfo(String className, String signature,
			String handler, boolean isGeneric) throws TranslationModelException {
		MethodInfo mi = info.getMethodInfoFromHandler(className, null, handler,
				false, false, false, isGeneric);
		if (mi == null) {
			mi = info.createMethodInfoFromHandler(signature, handler, true,
					isGeneric);
		}
		return mi;
	}

	/**
	 * 
	 */
	public TargetMethod findMethodMapping(String className, String signature,
			String handler, boolean scan, boolean store, boolean isoverride,
			boolean isGeneric) throws TranslationModelException {
		final MethodInfo mi = info.getMethodInfoFromHandler(className,
				signature, handler, scan, store, isoverride, isGeneric);
		if (mi != null) {
			return mi.getTarget(targetFramework.name());
		}

		return null;
	}

	/**
	 * 
	 */
	public TargetMethod updateMethodMapping(String elementName,
			String existingSignatureTag, String existingHandlerTag,
			String signatureKey, String handlerKey)
			throws TranslationModelException {
		final MethodInfo ci = info.updateMethodInfo(elementName,
				existingSignatureTag, existingHandlerTag, signatureKey,
				handlerKey);
		if (ci != null) {
			return ci.getTarget(targetFramework.name());
		}

		return null;
	}

	//
	// Properties / indexers
	//

	/**
	 * 
	 */
	public List<TargetProperty> findProperties(String handler, boolean isGeneric) {
		final ClassInfo ci = info.getClassInfoFromHandler(handler, true,
				isGeneric);
		if (ci != null) {
			final Map<String, PropertyInfo> properties = ci.getProperties();
			final List<TargetProperty> tProperties = new ArrayList<TargetProperty>();
			for (final PropertyInfo pInfo : properties.values()) {
				tProperties.add(pInfo.getTarget(targetFramework.name()));
			}
			return tProperties;
		}
		return null;
	}

	/**
	 * 
	 */
	public List<TargetIndexer> findIndexers(String handler) {
		final ClassInfo ci = info.getClassInfoFromHandler(handler, true, false);
		if (ci != null) {
			final Map<String, IndexerInfo> indexers = ci.getIndexers();
			final List<TargetIndexer> tIndexers = new ArrayList<TargetIndexer>();
			for (final IndexerInfo pInfo : indexers.values()) {
				tIndexers.add(pInfo.getTarget(targetFramework.name()));
			}
			return tIndexers;
		}
		return null;
	}

	//
	// ---------------------------------------------------------------------------------------
	//
	
	//
	// Attributes
	//

	public String getSynchronizedAttribute() {
		return Messages.getString("Java2CsModel.13"); //$NON-NLS-1$
	}

	public String getSerializableAttribute() {
		return Messages.getString("Java2CsModel.31"); //$NON-NLS-1$
	}

	public String getNotBrowsableAttribute() {
		return Messages.getString("Java2CsModel.14"); //$NON-NLS-1$
	}

	public String getPrefixForConstants() {
		return Messages.getString("Java2CsModel.15"); //$NON-NLS-1$
	}

	public String getAnonymousClassNamePattern() {
		return Messages.getString("Java2CsModel.16"); //$NON-NLS-1$
	}

	//
	// keywords
	//
	
	private String getKeyword(String keyword) {
		return info.getKeyword(keyword);
	}

	public String getKeyword(int keyword_id, int model) {
		switch (keyword_id) {
		case TranslationModelUtil.SUPER_KEYWORD:
			if (model == TranslationModelUtil.JAVA_MODEL) {
				return IKeywordConstants.JAVA_SUPER_KEYWORD;
			} else {
				return this.getKeyword(IKeywordConstants.JAVA_SUPER_KEYWORD);
			}
		case TranslationModelUtil.SYNCHRONIZED_KEYWORD:
			if (model == TranslationModelUtil.JAVA_MODEL) {
				return IKeywordConstants.JAVA_SYNCHRONIZED_KEYWORD;
			} else {
				return this
						.getKeyword(IKeywordConstants.JAVA_SYNCHRONIZED_KEYWORD);
			}
		case TranslationModelUtil.EXTENDS_KEYWORD:
			if (model == TranslationModelUtil.JAVA_MODEL) {
				return IKeywordConstants.JAVA_EXTENDS_KEYWORD;
			} else {
				return this.getKeyword(IKeywordConstants.JAVA_EXTENDS_KEYWORD);
			}
		case TranslationModelUtil.IMPLEMENTS_KEYWORD:
			if (model == TranslationModelUtil.JAVA_MODEL) {
				return IKeywordConstants.JAVA_IMPLEMENTS_KEYWORD;
			} else {
				return this
						.getKeyword(IKeywordConstants.JAVA_IMPLEMENTS_KEYWORD);
			}
		case TranslationModelUtil.TYPEOF_KEYWORD:
			if (model == TranslationModelUtil.JAVA_MODEL) {
				return IKeywordConstants.JAVA_TYPEOF_KEYWORD;
			} else {
				return this.getKeyword(IKeywordConstants.JAVA_TYPEOF_KEYWORD);
			}
		case TranslationModelUtil.PACKAGE_KEYWORD:
			if (model == TranslationModelUtil.JAVA_MODEL) {
				return IKeywordConstants.JAVA_PACKAGE_KEYWORD;
			} else {
				return this.getKeyword(IKeywordConstants.JAVA_PACKAGE_KEYWORD);
			}
		case TranslationModelUtil.IMPORT_KEYWORD:
			if (model == TranslationModelUtil.JAVA_MODEL) {
				return IKeywordConstants.JAVA_IMPORT_KEYWORD;
			} else {
				return this.getKeyword(IKeywordConstants.JAVA_IMPORT_KEYWORD);
			}
		case TranslationModelUtil.INSTANCEOF_KEYWORD:
			if (model == TranslationModelUtil.JAVA_MODEL) {
				return IKeywordConstants.JAVA_INSTANCEOF_KEYWORD;
			} else {
				return this
						.getKeyword(IKeywordConstants.JAVA_INSTANCEOF_KEYWORD);
			}
		case TranslationModelUtil.GOTO_KEYWORD:
			if (model == TranslationModelUtil.JAVA_MODEL) {
				return IKeywordConstants.JAVA_GOTO_KEYWORD;
			} else {
				return this.getKeyword(IKeywordConstants.JAVA_GOTO_KEYWORD);
			}
		case TranslationModelUtil.THIS_KEYWORD:
			if (model == TranslationModelUtil.JAVA_MODEL) {
				return IKeywordConstants.JAVA_THIS_KEYWORD;
			} else {
				return this.getKeyword(IKeywordConstants.JAVA_THIS_KEYWORD);
			}
		}
		return null;
	}

	//
	// Tags
	//

	public String getTag(int tag_id) {
		switch (tag_id) {
		case TranslationModelUtil.BINDING_TAG:
			return Messages.getString("Java2CsModel.17"); //$NON-NLS-1$
		case TranslationModelUtil.OVERRIDE_TAG:
			return Messages.getString("Java2CsModel.18"); //$NON-NLS-1$
		case TranslationModelUtil.VIRTUAL_TAG:
			return Messages.getString("Java2CsModel.19"); //$NON-NLS-1$
		case TranslationModelUtil.CONST_TAG:
			return Messages.getString("Java2CsModel.20"); //$NON-NLS-1$
		case TranslationModelUtil.SIGNATURE_TAG:
			return Messages.getString("Java2CsModel.21"); //$NON-NLS-1$
		case TranslationModelUtil.HANDLER_TAG:
			return Messages.getString("Java2CsModel.22"); //$NON-NLS-1$
		case TranslationModelUtil.REMOVE_TAG:
			return Messages.getString("Java2CsModel.23"); //$NON-NLS-1$
		case TranslationModelUtil.COVARIANCE_TAG:
			return Messages.getString("Java2CsModel.24"); //$NON-NLS-1$
		case TranslationModelUtil.PUBLICAPI_TAG:
			return Messages.getString("Java2CsModel.25"); //$NON-NLS-1$
		case TranslationModelUtil.TRANSLATORMAPPING_TAG:
			return Messages.getString("Java2CsModel.26"); //$NON-NLS-1$
		case TranslationModelUtil.TESTCASE_TAG:
			return Messages.getString("Java2CsModel.27"); //$NON-NLS-1$
		case TranslationModelUtil.TESTMETHOD_TAG:
			return Messages.getString("Java2CsModel.28"); //$NON-NLS-1$
		case TranslationModelUtil.TESTAFTER_TAG:
			return Messages.getString("Java2CsModel.29"); //$NON-NLS-1$
		case TranslationModelUtil.TESTBEFORE_TAG:
			return Messages.getString("Java2CsModel.30"); //$NON-NLS-1$
		case TranslationModelUtil.TESTCATEGORIE_TAG:
			return Messages.getString("Java2CsModel.32"); //$NON-NLS-1$
		case TranslationModelUtil.JAVADOC_VALUE_TAG:
			return Messages.getString("Java2CsModel.33"); //$NON-NLS-1$
		}
		return null;
	}

	//
	//
	//

	public String getAssertReplacementClassName() {
		return "System.Diagnostics.Debug";
	}

	//
	// --------------------------------------------------------------------
	//

	//
	// Variable
	//

	public String getVariable(String name) {
		final VariableInfo in = info.getVariable(name);
		if (in != null) {
			return in.getNames().get(0);
		} else {
			// TODO : externalize
			if (name.contains("$"))
				return name.replace("$", "_");
			return null;
		}
	}

	//
	// Implicit change
	//

	public void addImplicitFieldRename(String packageName, String className,
			String oldName, String newName) {
		info.addImplicitFieldRename(packageName, className, oldName, newName);
	}

	public void addImplicitNestedToInnerTransformation(String pckName,
			String className) {
		info.addImplicitNestedToInnerTransformation(pckName, className);
	}

	public void addImplicitConstantsRename(String packageName, String name,
			String newClassName, String[] elements) {
		info.addImplicitConstantsRename(packageName, name, newClassName,
				elements);
	}

	public void addImplicitNestedRename(String newPackageName, String newName,
			String oldPackageName, String oldName) {
		info.addImplicitNestedRename(newPackageName, newName, oldPackageName,
				oldName);
	}

	public boolean isAnImplicitNestedRename(String packageName, String newName) {
		return info.isAnImplicitNestedRename(packageName, newName);
	}

	//
	// resolve type name
	//

	public String resolveTypeName(String pckName, String typeName) {
		return info.resolveTypeName(pckName, typeName);
	}

	//
	// nested name
	//
	
	public String getNewNestedName(String pck, String className) {
		return info.getNewNestedName(pck, className);
	}

	//
	// mappings in comments
	//

	public void extractMappingFromComments(ClassInfo ci,
			TypeDeclaration typeDecl, boolean extractAndSave, boolean isXML)
			throws JavaModelException {
		MappingsInfo mappingsInfo = null;
		if (extractAndSave) {
			mappingsInfo = new MappingsInfo("FromComments", info);
		}
		final String mappingTag = getTag(TranslationModelUtil.TRANSLATORMAPPING_TAG);
		String comments = TranslationUtils.getTagValueFromDoc(typeDecl,
				mappingTag);
		if (comments != null)
			info.extractMappingFromComments(ci, comments, mappingsInfo);

		for (final FieldDeclaration field : typeDecl.getFields()) {
			comments = TranslationUtils.getTagValueFromDoc(field, mappingTag);
			if (comments != null)
				for (int i = 0; i < field.fragments().size(); i++) {
					final VariableDeclarationFragment frag = (VariableDeclarationFragment) field
							.fragments().get(i);
					final IVariableBinding binding = frag.resolveBinding();
					if (binding.getJavaElement() instanceof IField) {
						info.extractFieldMappingFromComments(ci,
								(IField) binding.getJavaElement(), comments,
								mappingsInfo);
					}
				}
		}
		for (final MethodDeclaration method : typeDecl.getMethods()) {
			comments = TranslationUtils.getTagValueFromDoc(method, mappingTag);
			final IMethod imethod = (IMethod) method.resolveBinding()
					.getJavaElement();
			if (comments != null)
				info.extractMethodMappingFromComments(ci, imethod, comments,
						mappingsInfo);
		}
		if (extractAndSave) {
			try {
				mappingsInfo.generateMappingInCommentsFile(isXML);
			} catch (final Exception e) {
				info.getConfiguration().getLogger().logException(
						"Error during savinf implicit mappings file", e);
			}
		}
	}

	//
	// java doc mappings
	//
	
	public HashMap<String, String> getJavaDocMappings() {
		return info.getJavaDocMappings();
	}
	
	public String getJavaDocTagMapping(String tag) {
		return info.getJavaDocTagMapping(tag);
	}

	//
	// disclaimer
	//
	
	public String getDisclaimer() {
		return this.info.getDisclaimer();
	}
}
