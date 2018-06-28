package com.ilog.translator.java2cs.translation;

import java.util.List;
import java.util.Set;

import org.eclipse.core.resources.IProject;
import org.eclipse.jdt.core.ICompilationUnit;
import org.eclipse.jdt.core.IImportDeclaration;
import org.eclipse.jdt.core.JavaModelException;
import org.eclipse.jdt.core.dom.BodyDeclaration;
import org.eclipse.jdt.core.dom.MethodDeclaration;
import org.eclipse.jdt.core.dom.TypeDeclaration;
import org.eclipse.jdt.core.search.SearchMatch;
import org.eclipse.ltk.core.refactoring.Change;

import com.ilog.translator.java2cs.configuration.ITranslationModel;
import com.ilog.translator.java2cs.configuration.Logger;
import com.ilog.translator.java2cs.configuration.TranslationConfiguration;
import com.ilog.translator.java2cs.configuration.info.ClassInfo;
import com.ilog.translator.java2cs.configuration.info.TranslationModelException;
//import com.ilog.translator.java2cs.translation.util.InheritanceHierarchy;

public interface ITranslationContext {

	// Configuration
	public TranslationConfiguration getConfiguration();

	// logger
	public Logger getLogger();

	// mapper
	public IMapper getMapper();

	// model
	public ITranslationModel getModel();

	// Namespace / package
	public boolean hasPackage(ICompilationUnit cu);

	public String getPackageName(String cName, IProject reference);

	public void addPackageName(String cName, String pName);

	//
	public boolean ignore(ICompilationUnit icunit) throws JavaModelException;

	// Change
	public void addChange(ICompilationUnit cu, Change change);

	public Change getChange(ICompilationUnit cu);

	public void clearChange();

	// super/this constructor invocation management
	public void addSuperConstructorInvoc(MethodDeclaration node,
			List<String> arguments, String pattern);

	public Object[] /*List<String>*/ getSuperConstructorInvoc(MethodDeclaration node);

	public void addThisConstructorInvoc(MethodDeclaration node,
			List<String> arguments);

	public List<String> getThisConstructorInvoc(MethodDeclaration node);

	public void removeSuperConstructorInvoc(MethodDeclaration node);

	public void removeThisConstructorInvoc(MethodDeclaration node);

	// Synchronized management
	public void addSynchronized(MethodDeclaration node);

	public boolean isSynchronized(MethodDeclaration node);

	// serializable
	public void addSerializable(TypeDeclaration node);

	public boolean isSerializable(TypeDeclaration node);

	//
	public String getSignatureFromDoc(BodyDeclaration node, boolean renamed);

	// Namespaces
	public void addNamespaces(String typeName, Set<String> arg);

	public Set<String> getNamespaces(String typeName);

	public Set<String> getDefaultNamespaces();

	// Import
	public IImportDeclaration[] getImports(ICompilationUnit cu);

	public void addImports(ICompilationUnit icunit, IImportDeclaration[] imports);

	public void addGenericImport(ICompilationUnit icunit, String imports);

	public List<String> getGenericImports(ICompilationUnit cu);

	public void cleanImports(ICompilationUnit cu);

	// Handler
	public String getHandlerFromDoc(BodyDeclaration node, boolean renamed);

	//
	// inner enclosing access
	//
	public boolean hasEnclosingAccess(String innerName, String enclosingName);

	public void resetInnerEnclosingAccess();

	public void addInnerEnclosingAccess(String innerName, String enclosingName);

	public int innerEnclosingAccessListSize();

	// ignorable
	public boolean ignorablePackage(String string, IProject reference);

	public void addToIgnorable(ClassInfo info);

	// genericType
	public void addGenericType(String fullyQualifiedName, IProject reference);

	public boolean isGenericType(String fullyQualifiedName);

	// Covariance
	public void addCovariance(List<SearchMatch> ref, String returnType);

	public List<CovarianceData> getCovariance(ICompilationUnit cu);

	public void clearCovariance();

	//
	//public InheritanceHierarchy getTypeHierarchy();

	//
	public void propagate();

	//
	public void extractMappingFromComments(ClassInfo ci,
			TypeDeclaration typeDecl);

	// Tests
	public boolean isATest(ICompilationUnit icunit);

	public void declareAsTest(ICompilationUnit icunit);

	//	
	public void buildClassInfoAndScanMapping(TypeDeclaration typeDecl,
			boolean scan);
	
	//
	
	public void addExtensionClassName(String className);
	
	public boolean IsAnExtensionClassName(String className);
	
	//
	//
	// code to Add to implementation
	public String getCodeToAddToImplementation(String handler);
	public void markClassForImplementationAdd(String handleIdentifier,
			String code);

	/**
	 * Add the given class names to the list of class marked as public documented
	 * 
	 * @param qualifiedName The fully qualified name of the class
	 */
	public void addClassToPublicDocumented(String qualifiedName, String handler);

	/**
	 * Add the given package names to the list of package marked as NOT public documented
	 * 
	 * @param qualifiedName The fully qualified name of the class
	 */
	public void addPackageToPublicDocumented(String elementName);

	public void listPublicDocumentedFiles();

	public void autoGetSetSearch(TypeDeclaration typeDecl)
	throws TranslationModelException, JavaModelException;
	
	public void autoPropertiesSearch(TypeDeclaration typeDecl);
	
}
