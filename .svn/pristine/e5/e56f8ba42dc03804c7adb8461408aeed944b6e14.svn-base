package com.ilog.translator.java2cs.configuration;

import java.util.HashMap;
import java.util.List;

import org.eclipse.core.resources.IProject;
import org.eclipse.jdt.core.IType;
import org.eclipse.jdt.core.JavaModelException;
import org.eclipse.jdt.core.dom.ITypeBinding;
import org.eclipse.jdt.core.dom.TypeDeclaration;

import com.ilog.translator.java2cs.configuration.info.ClassInfo;
import com.ilog.translator.java2cs.configuration.info.PackageInfo;
import com.ilog.translator.java2cs.configuration.info.MethodInfo;
import com.ilog.translator.java2cs.configuration.info.TranslationModelException;
import com.ilog.translator.java2cs.configuration.target.TargetClass;
import com.ilog.translator.java2cs.configuration.target.TargetField;
import com.ilog.translator.java2cs.configuration.target.TargetIndexer;
import com.ilog.translator.java2cs.configuration.target.TargetMethod;
import com.ilog.translator.java2cs.configuration.target.TargetPackage;
import com.ilog.translator.java2cs.configuration.target.TargetProperty;
import com.ilog.translator.java2cs.translation.ITranslationContext;
import com.ilog.translator.java2cs.util.IKeywordConstants;

/**
 * That interface represents the query layer of the knowledge of the translator.
 * It's typically used during the translation pass.
 * 
 * @author afau
 * 
 */
public interface ITranslationModel extends IKeywordConstants {

	public void initialize();
	
	//
	// migrate
	//
	
	public void migrateProjectMappingFiles();
	public void migrateJDKMappingFiles();
	
	public abstract void propagate(ITranslationContext context);

	public abstract void generateImplicitMappingFile(boolean isXML) throws Exception;

	//
	// retrieve mappings (target)
	//

	public abstract TargetClass findClassMapping(String handleIdentifier,
			boolean reportError, boolean isGeneric);

	public abstract TargetMethod findMethodMapping(String className,
			String signature, String handleIdentifier, boolean scan,
			boolean store, boolean isoverride, boolean isGeneric)
			throws JavaModelException, TranslationModelException;

	public abstract TargetField findFieldMapping(String fName, String handleIdentifier);

	public abstract TargetField findArrayFieldMapping(ITypeBinding array,
			String name);

	public abstract TargetClass findPrimitiveClassMapping(String name);

	public abstract TargetPackage findPackageMapping(String pck,
			IProject reference);

	public abstract TargetPackage findImportMapping(String pck,
			IProject reference);

	public abstract TargetMethod findConstructorMapping(String packageName,
			String className, String[] params, boolean b, String handler);

	public abstract TargetClass findGenericClassMapping(String handleIdentifier)
			throws TranslationModelException;

	public abstract List<TargetProperty> findProperties(String handleIdentifier,
			boolean isGeneric);	

	public abstract List<TargetIndexer> findIndexers(String handler);

	//
	// retrieve mappings (info)
	//
	
	public abstract PackageInfo findPackageInfo(String name, IProject reference);
	public abstract ClassInfo findClassInfo(String handleIdentifier,
			boolean reportError, boolean create, boolean isGenerics)
			throws TranslationModelException, JavaModelException;
	public abstract MethodInfo findMethodInfo(String className, String signature,
			String handler, boolean isGeneric) throws TranslationModelException;
	
	//
	// implicit mapping
	//

	public abstract void addImplicitNestedRename(String newPackageName,
			String newName, String oldPackageName, String oldName);

	public abstract void addImplicitFieldRename(String packageName, String className,
			String oldName, String newName);

	public abstract void addImplicitConstantsRename(String packageName, String name,
			String newClassName, String[] elements);

	public void addImplicitNestedToInnerTransformation(String pckName,
			String className);
	
	public abstract boolean isRemovedOrInnerClass(String cName, String pName,
			IProject reference);

	public abstract boolean isAnImplicitNestedRename(String packageName, String newName);
	
	//
	//
	//

	public abstract String getVariable(String node);

	public abstract String getKeyword(int keyword_id, int model);

	public abstract String getTag(int tag_id);

	//
	//
	//

	public abstract String getSynchronizedAttribute();

	public abstract String getSerializableAttribute();
	
	public abstract String getPrefixForConstants();

	public abstract String getAnonymousClassNamePattern();

	public abstract String getNotBrowsableAttribute();

	public abstract String getAssertReplacementClassName();

	//
	//
	//

	public abstract boolean isRemovedPackage(String packageName,
			IProject reference);

	public abstract String resolveTypeName(String pckName, String typeName);

	public abstract TargetMethod updateMethodMapping(String elementName,
			String existingSignatureTag, String existingHandlerTag,
			String signatureKey, String handlerKey)
			throws TranslationModelException;

	//
	// mapping from comments
	//

	public abstract void extractMappingFromComments(ClassInfo ci,
			TypeDeclaration comments, boolean extractAndSave, boolean isXML)
			throws JavaModelException;

	//
	//
	//

	public abstract String getNewNestedName(String pck, String cName);

	public abstract void addClassInfo(String name, IType type, ClassInfo nci);

	
	//
	// Javadoc
	//
	
	public abstract HashMap<String, String> getJavaDocMappings();
	public abstract String getJavaDocTagMapping(String tag);
	
	//
	// Disclaimer
	//
	
	public abstract String getDisclaimer();

}