package com.ilog.translator.java2cs.translation;

import java.util.HashMap;
import java.util.List;

import org.eclipse.core.resources.IProject;
import org.eclipse.jdt.core.ICompilationUnit;
import org.eclipse.jdt.core.dom.ArrayCreation;
import org.eclipse.jdt.core.dom.ClassInstanceCreation;
import org.eclipse.jdt.core.dom.EnumDeclaration;
import org.eclipse.jdt.core.dom.FieldAccess;
import org.eclipse.jdt.core.dom.FieldDeclaration;
import org.eclipse.jdt.core.dom.ITypeBinding;
import org.eclipse.jdt.core.dom.Initializer;
import org.eclipse.jdt.core.dom.LabeledStatement;
import org.eclipse.jdt.core.dom.MethodInvocation;
import org.eclipse.jdt.core.dom.Name;
import org.eclipse.jdt.core.dom.PackageDeclaration;
import org.eclipse.jdt.core.dom.ParameterizedType;
import org.eclipse.jdt.core.dom.PrimitiveType;
import org.eclipse.jdt.core.dom.QualifiedName;
import org.eclipse.jdt.core.dom.SimpleName;
import org.eclipse.jdt.core.dom.SimpleType;
import org.eclipse.jdt.core.dom.SuperMethodInvocation;
import org.eclipse.jdt.core.dom.TypeDeclaration;
import org.eclipse.jdt.core.dom.TypeLiteral;
import org.eclipse.jdt.core.dom.VariableDeclaration;
import org.eclipse.jdt.core.dom.VariableDeclarationExpression;
import org.eclipse.jdt.core.dom.VariableDeclarationFragment;
import org.eclipse.jdt.core.dom.VariableDeclarationStatement;

import com.ilog.translator.java2cs.configuration.target.TargetClass;
import com.ilog.translator.java2cs.configuration.target.TargetIndexer;
import com.ilog.translator.java2cs.configuration.target.TargetProperty;
import com.ilog.translator.java2cs.translation.noderewriter.INodeRewriter;

public interface IMapper {

	//
	// Method
	//
	public abstract INodeRewriter mapMethodInvocation(MethodInvocation node,
			ICompilationUnit fCu);

	public abstract INodeRewriter mapMethodInvocation(
			SuperMethodInvocation node, ICompilationUnit fCu);

	public abstract INodeRewriter mapMethodInvocation(
			ClassInstanceCreation node, ICompilationUnit fCu);

	public abstract INodeRewriter mapMethodDeclaration(String className,
			String signature, String handler, boolean search,
			boolean isoverride, boolean isGeneric);

	//
	// Array
	//
	public abstract INodeRewriter mapArrayCreation(ICompilationUnit icunit,
			ArrayCreation node);

	public abstract INodeRewriter mapArrayType(ICompilationUnit icunit, ITypeBinding at);

	//
	// Initializer
	//
	public abstract INodeRewriter mapInitializer(Initializer node);

	//
	// Field
	//
	public abstract INodeRewriter mapFieldAccess(FieldAccess node);

	public abstract INodeRewriter mapFieldAccess(SimpleName node);

	public abstract INodeRewriter mapFieldAccess(VariableDeclarationFragment node);

	public abstract INodeRewriter mapFieldAccess(ICompilationUnit icunit,
			QualifiedName node);

	public abstract INodeRewriter mapFieldDeclaration(FieldDeclaration node,
			String handler);

	//
	// Variable
	//
	public abstract INodeRewriter mapVariableDeclaration(
			VariableDeclarationExpression node);

	public abstract INodeRewriter mapVariableDeclaration(
			VariableDeclarationStatement node);

	public abstract INodeRewriter mapVariableDeclaration(VariableDeclaration node);

	//
	// Type
	//
	public abstract INodeRewriter mapTypeDeclaration(ITypeBinding node,
			String handler);

	public abstract INodeRewriter mapSimpleType(ICompilationUnit icunit,
			SimpleType type);

	public abstract INodeRewriter mapPrimitiveType(PrimitiveType type);

	public abstract INodeRewriter mapType(ICompilationUnit fCu, Name node);

	public abstract INodeRewriter mapType2(ICompilationUnit cu,
			QualifiedName node);

	public abstract INodeRewriter mapType(ICompilationUnit fCu, TypeLiteral node);

	//
	// Labeled Statement
	//
	public abstract INodeRewriter mapLabeledStatement(LabeledStatement node);

	//
	// Packages
	// 
	public abstract INodeRewriter mapPackageDeclaration(PackageDeclaration node,
			ICompilationUnit icunit);

	public abstract INodeRewriter mapPackageAccess(QualifiedName node,
			IProject reference);

	// TODO: Return type is strange
	public abstract String mapImport(String pck, boolean onDemand,
			IProject reference);
	
	public abstract boolean isRemovePackage(String packageName,
			IProject reference);

	//
	// keywords
	//	
	public abstract String getKeyword(int id, int model);

	//
	// Synchronized
	//
	public abstract String getSynchronizedAttribute();

	//
	// PrefixForConstants
	//
	public abstract String getPrefixForConstants();

	//
	// AnonymousClassNamePattern
	//
	public abstract String getAnonymousClassNamePattern();

	//
	// Tags
	//
	public abstract String getTag(int id);

	//
	// NotBrowsableAttribute
	//
	public abstract String getNotBrowsableAttribute();

	//
	// SerializableAttribute
	//
	public abstract String getSerializableAttribute();
	
	//
	// TargetClass
	// 
	// TODO: Why here and not on model ?
	public abstract TargetClass getTargetClass(TypeDeclaration node);	
	public abstract TargetClass getTargetClass(EnumDeclaration node);

	// mapParameterizedType
	public abstract INodeRewriter mapParameterizedType(ICompilationUnit cu,
			ParameterizedType node);

	

	// Properties
	// TODO: Why here and not on model ?
	public abstract List<TargetProperty> getProperties(TypeDeclaration node,
			String handler);

	// indexers
	// TODO: Why here and not on model ?
	public abstract List<TargetIndexer> getIndexers(TypeDeclaration node,
			String handler);

	// jagged array
	public abstract String mapJaggedArrayCreation(ArrayCreation node,
			String comments, String[] args);

	// enum
	public abstract String mapEnumValues(ITypeBinding enumType);
	public abstract String mapEnumValueOf(ITypeBinding enumType, String arg);

	// URS
	public abstract String mapURS(String sLeft, String sRight);

	// Javadoc mapping
	public abstract HashMap<String, String> getJavaDocMappings();
	public abstract String getJavaDocTagMapping(String tag);

	// disclaimer
	public abstract String getDisclaimer();
}