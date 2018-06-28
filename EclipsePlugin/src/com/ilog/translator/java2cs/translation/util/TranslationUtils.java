package com.ilog.translator.java2cs.translation.util;

import java.io.File;
import java.util.ArrayList;
import java.util.List;

//import org.w3c.tidy.*;

import org.eclipse.core.resources.IProject;
import org.eclipse.core.runtime.NullProgressMonitor;
import org.eclipse.jdt.core.ICompilationUnit;
import org.eclipse.jdt.core.IJavaElement;
import org.eclipse.jdt.core.IJavaProject;
import org.eclipse.jdt.core.IMethod;
import org.eclipse.jdt.core.IPackageFragment;
import org.eclipse.jdt.core.IPackageFragmentRoot;
import org.eclipse.jdt.core.IType;
import org.eclipse.jdt.core.ITypeHierarchy;
import org.eclipse.jdt.core.ITypeParameter;
import org.eclipse.jdt.core.JavaModelException;
import org.eclipse.jdt.core.Signature;
import org.eclipse.jdt.core.dom.ASTNode;
import org.eclipse.jdt.core.dom.BlockComment;
import org.eclipse.jdt.core.dom.BodyDeclaration;
import org.eclipse.jdt.core.dom.ClassInstanceCreation;
import org.eclipse.jdt.core.dom.CompilationUnit;
import org.eclipse.jdt.core.dom.Expression;
import org.eclipse.jdt.core.dom.FieldDeclaration;
import org.eclipse.jdt.core.dom.IMethodBinding;
import org.eclipse.jdt.core.dom.ITypeBinding;
import org.eclipse.jdt.core.dom.ImportDeclaration;
import org.eclipse.jdt.core.dom.Javadoc;
import org.eclipse.jdt.core.dom.LineComment;
import org.eclipse.jdt.core.dom.MethodInvocation;
import org.eclipse.jdt.core.dom.Modifier;
import org.eclipse.jdt.core.dom.Name;
import org.eclipse.jdt.core.dom.NormalAnnotation;
import org.eclipse.jdt.core.dom.PackageDeclaration;
import org.eclipse.jdt.core.dom.ParameterizedType;
import org.eclipse.jdt.core.dom.PrimitiveType;
import org.eclipse.jdt.core.dom.QualifiedName;
import org.eclipse.jdt.core.dom.SimpleName;
import org.eclipse.jdt.core.dom.SimpleType;
import org.eclipse.jdt.core.dom.StructuralPropertyDescriptor;
import org.eclipse.jdt.core.dom.TagElement;
import org.eclipse.jdt.core.dom.TextElement;
import org.eclipse.jdt.core.dom.Type;
import org.eclipse.jdt.core.dom.TypeDeclaration;
import org.eclipse.jdt.core.dom.TypeLiteral;
import org.eclipse.jdt.core.dom.VariableDeclarationExpression;
import org.eclipse.jdt.core.dom.VariableDeclarationFragment;
import org.eclipse.jdt.core.dom.VariableDeclarationStatement;
import org.eclipse.jdt.core.dom.rewrite.ASTRewrite;
import org.eclipse.jdt.core.dom.rewrite.ListRewrite;
import org.eclipse.jdt.internal.core.util.SimpleDocument;
import org.eclipse.jdt.internal.corext.dom.ASTFlattener;
import org.eclipse.jdt.internal.corext.dom.ASTNodes;
import org.eclipse.jdt.internal.corext.dom.Bindings;
import org.eclipse.jdt.internal.corext.refactoring.structure.CompilationUnitRewrite;
import org.eclipse.jdt.internal.corext.util.JavaModelUtil;
import org.eclipse.jface.text.BadLocationException;
import org.eclipse.jface.text.IDocument;
import org.eclipse.text.edits.DeleteEdit;
import org.eclipse.text.edits.MultiTextEdit;
import org.eclipse.text.edits.TextEdit;

import com.ilog.translator.java2cs.configuration.ChangeModifierDescriptor;
import com.ilog.translator.java2cs.configuration.DotNetModifier;
import com.ilog.translator.java2cs.configuration.Logger;
import com.ilog.translator.java2cs.configuration.info.ClassInfo;
import com.ilog.translator.java2cs.configuration.info.PackageInfo;
import com.ilog.translator.java2cs.configuration.info.TranslationModelException;
import com.ilog.translator.java2cs.configuration.options.MethodMappingPolicy;
import com.ilog.translator.java2cs.configuration.options.PackageMappingPolicy;
import com.ilog.translator.java2cs.configuration.target.TargetClass;
import com.ilog.translator.java2cs.configuration.target.TargetPackage;
import com.ilog.translator.java2cs.translation.ITranslationContext;
import com.ilog.translator.java2cs.translation.TranslatorASTRewrite;
import com.ilog.translator.java2cs.translation.noderewriter.INodeRewriter;
import com.ilog.translator.java2cs.translation.noderewriter.PackageRewriter;
import com.ilog.translator.java2cs.translation.noderewriter.PrimitiveTypeRewriter;
import com.ilog.translator.java2cs.translation.noderewriter.PropertyRewriter;
import com.ilog.translator.java2cs.translation.noderewriter.TypeRewriter;
import com.ilog.translator.java2cs.util.TranslationModelUtil;
import com.ilog.translator.java2cs.util.Utils;

@SuppressWarnings("restriction")
public class TranslationUtils {

	public static final String END_TYPEOF_COMMENT = ") */";
	public static final String EMPTY_STRING = "";
	public static final String END_INSERT_HERE_COMMENT = ". */";
	public static final String DOT = ".";
	public static final String START_TYPEOF_COMMENT = "/* typeof(";
	public static final String START_INSERT_HERE_COMMENT = "/* insert_here:";
	public static final String END_COMMENTS = " */";

	public static final String FQN_OBJECT = Object.class.getName();
	public static final String FQN_BOOLEAN = Boolean.class.getName();
	public static final String FQN_STRING = String.class.getName();
	public static final String FQN_CHAR = Character.class.getName();
	public static final String FQN_INT = Integer.class.getName();
	public static final String FQN_LONG = Long.class.getName();
	public static final String FQN_DOUBLE = Double.class.getName();
	public static final String FQN_FLOAT = Float.class.getName();
	public static final String FQN_BYTE = Byte.class.getName();
	public static final String FQN_SHORT = Short.class.getName();

	public static final String CHAR = "char";
	public static final String INT = "int";
	public static final String LONG = "long";
	public static final String DOUBLE = "double";
	public static final String FLOAT = "float";
	public static final String BOOLEAN = "boolean";
	public static final String BYTE = "byte";
	public static final String SHORT = "short";
	public static final String SBYTE = "sbyte";
	public static final String USHORT = "ushort";
	public static final String UINT = "uint";
	public static final String ULONG = "ulong";
	public static final String DECIMAL = "decimal";

	public static final String END_REMOVE_HERE = " */";
	public static final String STAT_REMOVE_HERE = "/* remove_here:";
	public static final String CLASS = "class";
	public static final String OBJECT = "Object";
	public static final String WHERE_SEPARATOR = " : ";
	public static final String MODGENERICMETHODCONSTRAINT = "@modgenericmethodconstraint";
	public static final String WHERE = " where ";
	public static final String SUPER_KW = "super";
	public static final String WILDCARD_SEPARATOR = "_/_";
	public static final String MODWILDCARD = "@modwildcard";
	public static final String THIS = "this";
	public static final String SUPER = "base";

	//
	//
	//

	@SuppressWarnings("unchecked")
	public static boolean isOverride(ITranslationContext context,
			BodyDeclaration node) {
		final Javadoc doc = node.getJavadoc();
		boolean override = false;

		if (doc != null) {
			final List tags = doc.tags();
			for (int i = 0; i < tags.size(); i++) {
				final TagElement te = (TagElement) tags.get(i);
				final String name = te.getTagName();
				if (name != null) {
					if (name.equals(context.getMapper().getTag(
							TranslationModelUtil.OVERRIDE_TAG))) {
						override = true;
					}
				}
			}
		}
		return override;
	}

	//
	//
	//
	/**
	 * Return the name of that binding including generic
	 * 
	 * @param typeB
	 * @return
	 */
	public static String getFQNNameWithGenerics(ITypeBinding typeB) {
		String fName = EMPTY_STRING;
		if (typeB.isMember()) {
			fName = getFQNNameWithGenerics(typeB.getDeclaringClass()) + DOT
					+ typeB.getName();
			if (typeB.getName().contains("<")) {
				return fName;
			}
		} else {
			fName = Bindings.getFullyQualifiedName(typeB);
		}
		final ITypeBinding[] typeArgs = typeB.getTypeArguments();
		final ITypeBinding[] typePars = typeB.getTypeParameters();
		if (typeArgs != null && typeArgs.length > 0) {
			fName += "<";
			for (int i = 0; i < typeArgs.length; i++) {
				fName += getFQNNameWithGenerics(typeArgs[i]);
				if (i < typeArgs.length - 1)
					fName += ",";
			}
			fName += ">";
		} else if (typeB.isCapture()) {
			final ITypeBinding wildcard = typeB.getWildcard();
			if (wildcard != null)
				return "?";
		}
		if (typePars != null && typePars.length > 0) {
			fName += "<";
			for (int i = 0; i < typePars.length; i++) {
				fName += getFQNNameWithGenerics(typePars[i]);
				if (i < typePars.length - 1)
					fName += ",";
			}
			fName += ">";
		}
		return fName;
	}

	public static String getName(ITypeBinding type) {
		String name = type.getQualifiedName();
		final int gindex = name.indexOf('<');
		if (gindex > 0) {
			name = name.substring(0, gindex);
		}
		final int index = name.lastIndexOf('.');
		if (index > 0) {
			name = name.substring(index + 1);
		}
		return name;
	}

	public static String getQualifiedNameWithQualifiedGenerics(
			ITypeBinding typeB) {
		String fName = EMPTY_STRING;
		if (typeB.isMember()) {
			fName = getQualifiedNameWithQualifiedGenerics(typeB
					.getDeclaringClass())
					+ DOT + typeB.getName();
			if (typeB.getName().contains("<")) {
				return fName;
			}
		} else {
			fName = getName(typeB);
		}
		final ITypeBinding[] typeArgs = typeB.getTypeArguments();
		final ITypeBinding[] typePars = typeB.getTypeParameters();
		if (typeArgs != null && typeArgs.length > 0) {
			fName += "<";
			for (int i = 0; i < typeArgs.length; i++) {
				fName += getQualifiedNameWithQualifiedGenerics(typeArgs[i]);
				if (i < typeArgs.length - 1)
					fName += ",";
			}
			fName += ">";
		} else if (typeB.isCapture()) {
			final ITypeBinding wildcard = typeB.getWildcard();
			if (wildcard != null)
				return "?";
		}
		if (typePars != null && typePars.length > 0) {
			fName += "<";
			for (int i = 0; i < typePars.length; i++) {
				fName += getQualifiedNameWithQualifiedGenerics(typePars[i]);
				if (i < typePars.length - 1)
					fName += ",";
			}
			fName += ">";
		}
		return fName;
	}

	//
	//
	//

	public static String castToMethodReturnType(ITranslationContext context,
			MethodInvocation method, ICompilationUnit fCu) {
		final IType retType = (IType) method.resolveMethodBinding()
				.getReturnType().getJavaElement();
		final TargetClass tc = context.getModel().findClassMapping(
				retType.getHandleIdentifier(),
				false,
				TranslationUtils.isGeneric(method.resolveMethodBinding()
						.getReturnType()));
		String castType = EMPTY_STRING;
		if (tc != null && tc.getPackageName() != null && tc.getName() != null) {
			castType = tc.getPackageName() + DOT + tc.getShortName();
		} else {
			final String pck = retType.getPackageFragment().getElementName();
			IType enclosingType = null;
			try {
				enclosingType = fCu.getAllTypes()[0];
			} catch (final JavaModelException e) {
				// TODO: !!!!
			}
			if (enclosingType != null && retType.getDeclaringType() != null
					&& isInHierarchy(retType, enclosingType)) {
				castType = retType.getElementName();
			} else {
				String cName = null;
				if (retType.getDeclaringType() != null) {
					cName = resolveInnerClassName(retType);
				} else {
					cName = DOT + retType.getElementName();
				}
				final TargetPackage tPck = context.getModel()
						.findPackageMapping(pck,
								retType.getJavaProject().getProject());
				if (tPck != null) {
					castType = tPck.getName() + cName;
				} else
					// TODO: Check if it's impacted by default mapping behavior
					castType = Utils
							.capitalize(retType.getFullyQualifiedName());
			}
		}
		return castType;
	}

	private static String resolveInnerClassName(IType retType) {
		String ret = EMPTY_STRING;
		IType currentType = retType;
		String generics = EMPTY_STRING;
		try {
			while (currentType.getDeclaringType() != null) {
				generics = buildGenericArgument(currentType);
				ret = ret + DOT + currentType.getElementName() + generics;
				currentType = currentType.getDeclaringType();
			}
			generics = buildGenericArgument(currentType);
			return DOT + currentType.getElementName() + generics + ret;
		} catch (final JavaModelException e) {
			e.printStackTrace();
			// TODO: !!!!
		}
		return retType.getElementName();
	}

	private static String buildGenericArgument(IType currentType)
			throws JavaModelException {
		if (currentType.getTypeParameters() == null
				|| currentType.getTypeParameters().length == 0)
			return EMPTY_STRING;
		String res = "<";
		for (int i = 0; i < currentType.getTypeParameters().length; i++) {
			final ITypeParameter tParam = currentType.getTypeParameters()[i];
			res += tParam.getElementName();
			if (i < currentType.getTypeParameters().length - 1)
				res += ",";
		}
		res += ">";
		return res;
	}

	//
	//
	//

	private static boolean isInHierarchy(IType innerClass, IType enclosingClass) {
		final NullProgressMonitor npm = new NullProgressMonitor();
		try {
			final ITypeHierarchy hierarchy = enclosingClass
					.newSupertypeHierarchy(npm);
			return hierarchy.contains(innerClass.getDeclaringType());
		} catch (final JavaModelException e) {
			return false;
		}
	}

	/*
	 * private static boolean needFullyQualifiedName(ITypeBinding tBinding,
	 * ICompilationUnit fCu) { String thisPck =
	 * fCu.findPrimaryType().getPackageFragment() .getElementName(); String
	 * typePck = ((IType) tBinding.getJavaElement())
	 * .getPackageFragment().getElementName(); return !isInHierarchy((IType)
	 * tBinding.getJavaElement(), fCu .findPrimaryType()) ||
	 * !typePck.equals(thisPck); }
	 */

	@SuppressWarnings("unchecked")
	public static String replaceByQualifiedName(Type type,
			ICompilationUnit fCu, ASTRewrite rewriter,
			ITranslationContext context) {
		final ITypeBinding tBinding = type.resolveBinding();
		String afterComments = getAfterComments(type, fCu, context);
		if (afterComments == null)
			afterComments = "";
		else
			afterComments += " ";		
		String beforeComments = getBeforeComments(type, fCu, context);
		String result = null;
		if (tBinding.isMember()) {
			final ITypeBinding enclosingType = tBinding.getDeclaringClass();
			if (type.isSimpleType()) {
				if (!isInHierarchy((IType) tBinding.getJavaElement(), fCu
						.findPrimaryType())) {
					// FullyQualified
					if (!isGenericOrRaw(enclosingType)) // Eclipse BUG 212122
						result = createFullyQualifiedTypeName(type, tBinding,
								enclosingType, false, context);
				} else {
					// Qualified i.e. add enclosing instance Name
					if (!isGenericOrRaw(enclosingType)) { // Eclipse BUG
						// 212122
						result = createQualifiedTypeName(type, tBinding,
								enclosingType, false, context);
						final ASTNode root = type.getRoot();
						if (root.getNodeType() == ASTNode.COMPILATION_UNIT) {
							final CompilationUnit cu = (CompilationUnit) root;
							final ListRewrite lr = rewriter.getListRewrite(cu,
									CompilationUnit.IMPORTS_PROPERTY);
							final ImportDeclaration id = root.getAST()
									.newImportDeclaration();
							id.setName(root.getAST().newName(
									tBinding.getPackage().getName()));
							id.setOnDemand(true);
							lr.insertLast(id, null);
						}
					}
				}
			} else if (type.isParameterizedType()) {
				String newName = null;
				if (!isInHierarchy((IType) tBinding.getJavaElement(), fCu
						.findPrimaryType())) {
					// if (!enclosingType.isParameterizedType()) {
					if (!isGenericOrRaw(enclosingType)) // Eclipse BUG 212122
						newName = createFullyQualifiedTypeName(type, tBinding,
								enclosingType, true, context);
					else
						newName = createFullyQualifiedTypeName(type, tBinding,
								enclosingType, false, context);
				} else {
					if (!isGenericOrRaw(enclosingType)) { // Eclipse BUG
						// 212122
						newName = createQualifiedTypeName(type, tBinding,
								enclosingType, true, context);
						final ASTNode root = type.getRoot();
						if (root.getNodeType() == ASTNode.COMPILATION_UNIT) {
							final CompilationUnit cu = (CompilationUnit) root;
							final ListRewrite lr = rewriter.getListRewrite(cu,
									CompilationUnit.IMPORTS_PROPERTY);
							final ImportDeclaration id = root.getAST()
									.newImportDeclaration();
							id.setName(root.getAST().newName(
									tBinding.getPackage().getName()));
							id.setOnDemand(true);
							lr.insertLast(id, null);
						}
					}
				}
				if (newName != null && !newName.endsWith(">")) {
					final List<Type> typeArgs = ((ParameterizedType) type)
							.typeArguments();
					newName += "<";
					for (int i = 0; i < typeArgs.size(); i++) {
						String typeArgName = replaceByQualifiedName(typeArgs
								.get(i), fCu, rewriter, context);
						;
						if (typeArgName == null)
							typeArgName = ASTNodes.asString(typeArgs.get(i));
						newName += typeArgName;
						if (i < typeArgs.size() - 1)
							newName += ",";
					}
					newName += ">";
				}
				result = newName;

			}
		}
		if (result != null)
			return beforeComments + result + afterComments;
		else
			return result;
	}

	public static boolean isGeneric(ITypeBinding type) {
		if (type == null)
			return false;
		return type.isParameterizedType() || type.isGenericType();
	}

	public static boolean isGenericOrRaw(ITypeBinding type) {
		if (type == null)
			return false;
		return type.isParameterizedType() || type.isGenericType()
				|| type.isRawType();
	}

	//
	//
	//

	private static String createFullyQualifiedTypeName(Type type,
			ITypeBinding tBinding, ITypeBinding enclosingType,
			boolean useRawType, ITranslationContext context) {
		String result;
		String innerName = tBinding.getName();
		if (tBinding.isParameterizedType()) {
			final ITypeBinding[] typeArgs = tBinding.getTypeArguments();
			if (typeArgs != null && typeArgs.length > 0) {
				String res = "<";
				for (int i = 0; i < typeArgs.length - 1; i++) {
					res = replaceByJavaName(context, typeArgs, res, i, ",");
				}
				res = replaceByJavaName(context, typeArgs, res,
						typeArgs.length - 1, ""); // typeArgs[typeArgs.length
				// -
				// 1].getQualifiedName();
				res += ">";
				res = tBinding.getErasure().getName() + res;
				innerName = res;
			}
		}
		if (useRawType)
			innerName = Bindings.getRawName(tBinding);
		if (ASTNodes.getParent(type, ASTNode.CLASS_INSTANCE_CREATION) == null
				|| ASTNodes.getParent(type, ASTNode.PARAMETERIZED_TYPE) != null) {
			final String enclosingName = enclosingType.getQualifiedName();
			if (enclosingType.isRawType()) {
				final ITypeBinding[] typeArgs = tBinding.getTypeArguments();
				if (typeArgs != null && typeArgs.length > 0) {
					String res = "/* insert_here:<";
					for (int i = 0; i < typeArgs.length - 1; i++) {
						res = replaceByCSharpName(context, typeArgs, res, i,
								",", true);
					}
					res = replaceByCSharpName(context, typeArgs, res,
							typeArgs.length - 1, "", true); // typeArgs[typeArgs.length
					// -
					// 1].getQualifiedName();
					res += "> */";

					res = enclosingName + res;
					result = res + DOT + innerName;
				} else {
					result = enclosingName + DOT + innerName;
				}
			} else {
				result = enclosingName + DOT + innerName;
			}
		} else {
			final String handler = enclosingType.getJavaElement()
					.getHandleIdentifier();
			final TargetClass targetClass = context.getModel()
					.findClassMapping(handler, true,
							TranslationUtils.isGeneric(enclosingType));
			if (targetClass != null && targetClass.getName() != null) {
				final String cName = targetClass.getName();
				final String pName = targetClass.getPackageName();
				// newName = "/* insert_here:"+ enclosingName + ". */" +
				// innerName;
				if (cName.startsWith(pName)) {
					result = START_INSERT_HERE_COMMENT + cName
							+ END_INSERT_HERE_COMMENT + innerName;
				} else
					result = START_INSERT_HERE_COMMENT + pName + DOT + cName
							+ END_INSERT_HERE_COMMENT + innerName;
			} else {
				final String pName = enclosingType.getPackage().getName();
				final TargetPackage tPck = context.getModel()
						.findPackageMapping(
								pName,
								enclosingType.getJavaElement().getJavaProject()
										.getProject());
				final String enclosingName = computeEnclosingName(
						enclosingType, type.resolveBinding(), context);
				if (type.getParent().getNodeType() == ASTNode.CLASS_INSTANCE_CREATION) {
					// outer.new Inner(...) case
					final ClassInstanceCreation cic = (ClassInstanceCreation) type
							.getParent();
					if (cic.getExpression() != null) {
						if (tPck != null) {
							result = START_INSERT_HERE_COMMENT + tPck.getName()
									+ DOT + enclosingName
									+ END_INSERT_HERE_COMMENT + innerName;
						} else {
							result = START_INSERT_HERE_COMMENT
									+ defaultMappingForPackage(context, pName,
											enclosingType.getJavaElement()
													.getJavaProject()
													.getProject()) + DOT
									+ enclosingName + END_INSERT_HERE_COMMENT
									+ innerName;
						}
						return result;
					}
				}
				if (tPck != null) {
					result = START_INSERT_HERE_COMMENT + tPck.getName()
							+ END_INSERT_HERE_COMMENT + enclosingName + DOT
							+ innerName;
				} else {
					result = START_INSERT_HERE_COMMENT
							+ defaultMappingForPackage(context, pName,
									enclosingType.getJavaElement()
											.getJavaProject().getProject())
							+ END_INSERT_HERE_COMMENT + enclosingName + DOT
							+ innerName;
				}
			}
		}
		return result;
	}

	public static String replaceByCSharpName(ITranslationContext context,
			ITypeBinding[] typeArgs, String res, int i, String end,
			boolean inComments) {
		final String handler = typeArgs[i].getJavaElement()
				.getHandleIdentifier();
		final TargetClass targetClass = context.getModel().findClassMapping(
				handler, true, TranslationUtils.isGeneric(typeArgs[i]));
		if (targetClass != null && targetClass.getName() != null) {
			String cName = targetClass.getName();
			if (targetClass.isNullable()) {
				if (inComments)
					cName += "?";
				else
					cName += "/* insert_here:? */";
			}
			// String pName = targetClass.getPackageName();
			res += /* pName + DOT + */cName + end;
		} else if (typeArgs[i].isMember()) {
			res += typeArgs[i].getDeclaringClass().getName() + "."
					+ typeArgs[i].getName() + end;
		} else
			res += typeArgs[i].getName() + end;
		return res;
	}

	private static String replaceByJavaName(ITranslationContext context,
			ITypeBinding[] typeArgs, String res, int i, String end) {
		if (typeArgs[i].isMember()) {
			res += typeArgs[i].getDeclaringClass().getName() + "."
					+ typeArgs[i].getName() + end;
		} else
			res += typeArgs[i].getName() + end;
		return res;
	}

	private static String createQualifiedTypeName(Type type,
			ITypeBinding tBinding, ITypeBinding enclosingType,
			boolean useRawType, ITranslationContext context) {
		String result;
		String innerName = tBinding.getName();
		if (useRawType)
			innerName = Bindings.getRawName(tBinding);
		if (ASTNodes.getParent(type, ASTNode.CLASS_INSTANCE_CREATION) == null
				|| ASTNodes.getParent(type, ASTNode.PARAMETERIZED_TYPE) != null) {
			final String enclosingName = enclosingType.getName();
			result = enclosingName + DOT + innerName;
		} else {
			final String handler = enclosingType.getJavaElement()
					.getHandleIdentifier();
			final TargetClass targetClass = context.getModel()
					.findClassMapping(handler, true,
							TranslationUtils.isGeneric(enclosingType));
			if (targetClass != null && targetClass.getName() != null) {
				final String cName = targetClass.getName();
				result = START_INSERT_HERE_COMMENT + cName
						+ END_INSERT_HERE_COMMENT + innerName;
			} else {
				// String pName = enclosingType.getPackage().getName();
				// TargetPackage tPck = context.getModel().findPackageMapping(
				// pName,
				// enclosingType.getJavaElement().getJavaProject().getProject());
				final String enclosingName = computeEnclosingName(
						enclosingType, type.resolveBinding(), context);
				result = START_INSERT_HERE_COMMENT + enclosingName
						+ END_INSERT_HERE_COMMENT + innerName;
			}
		}
		return result;
	}

	private static String computeEnclosingName(ITypeBinding enclosingType,
			ITypeBinding innerType, ITranslationContext context) {
		String result = null;
		if (enclosingType.isRawType() && innerType.isParameterizedType()) {
			final ITypeBinding[] typeArgs = innerType.getTypeArguments();
			if (typeArgs != null && typeArgs.length > 0) {
				String res = "/* insert_here:<";
				for (int i = 0; i < typeArgs.length - 1; i++) {
					res = replaceByCSharpName(context, typeArgs, res, i, ",",
							true);
				}
				res = replaceByCSharpName(context, typeArgs, res,
						typeArgs.length - 1, "", true); // typeArgs[typeArgs.length
				// -
				// 1].getQualifiedName();
				res += "> */";

				res = enclosingType.getName() + res;
				result = res;
			} else {
				result = enclosingType.getName();
			}
		} else {
			result = enclosingType.getName();
		}
		ITypeBinding currentType = enclosingType;
		while (currentType != null && currentType.isMember()) {
			currentType = currentType.getDeclaringClass();
			// String currentTypeName = currentType.getName();
			result = currentType.getName() + DOT + result;
		}
		return result;
	}

	//
	//
	//

	public static String computeSignature(IMethod m) throws JavaModelException {
		final String[] parameterTypes = computeTypes(m.getParameterTypes(), m
				.getDeclaringType());
		String retType = "V";
		String[] typeArgs = null;
		try {
			retType = m.getReturnType();
			typeArgs = Signature.getTypeArguments(retType);
		} catch(Exception e) {
			
		}
		if (typeArgs != null && typeArgs.length > 0) {
			String res = JavaModelUtil.getResolvedTypeName(retType, m
					.getDeclaringType());
			if (res == null)
				return InterfaceRenamingUtil.replaceInterfaceRenamed(retType);
			if (!res.endsWith(">")) {
				res = res + "<";
				for (int i = 0; i < typeArgs.length; i++) {
					final String nta = JavaModelUtil.getResolvedTypeName(
							typeArgs[i], m.getDeclaringType());
					res += nta;
					if (i < typeArgs.length - 1)
						res += ",";
				}
				res += ">";
			}
			if (res == null)
				res = retType;
			final String s = Signature.createMethodSignature(parameterTypes,
					Signature.createTypeSignature(res, false));
			return InterfaceRenamingUtil.replaceInterfaceRenamed(s);
		} else {
			String res = JavaModelUtil.getResolvedTypeName(retType, m
					.getDeclaringType());
			if (res == null)
				res = retType;

			final int dim = Signature.getArrayCount(res);
			if (dim > 0) {
				final String eType = Signature.getElementType(res);
				return InterfaceRenamingUtil.replaceInterfaceRenamed(Signature
						.createArraySignature(Signature.createTypeSignature(
								eType, false), dim));
			}
			final String s = Signature.createMethodSignature(parameterTypes,
					Signature.createTypeSignature(res, false));
			return InterfaceRenamingUtil.replaceInterfaceRenamed(s);
		}
	}

	public static String[] computeTypes(String[] typesName, IType context)
			throws JavaModelException {
		if (typesName == null)
			return null;
		final String[] resultName = new String[typesName.length];
		for (int i = 0; i < typesName.length; i++) {
			resultName[i] = JavaModelUtil.getResolvedTypeName(typesName[i],
					context);
			if (resultName[i] == null)
				resultName[i] = typesName[i];
		}
		return resultName;
	}

	//
	//
	//

	public static boolean isEnum(ICompilationUnit cu) {
		try {
			final IType type = cu.getTypes()[0];
			return type.isEnum();
		} catch (final JavaModelException e) {
			// TODO: !!!
			e.printStackTrace();
			return false;
		}
	}

	//
	//
	//

	public static VariableDeclarationFragment getFrament(ASTNode varDecl) {
		if (varDecl.getNodeType() == ASTNode.VARIABLE_DECLARATION_EXPRESSION) {
			return (VariableDeclarationFragment) ((VariableDeclarationExpression) varDecl)
					.fragments().get(0);
		} else if (varDecl.getNodeType() == ASTNode.VARIABLE_DECLARATION_STATEMENT) {
			return (VariableDeclarationFragment) ((VariableDeclarationStatement) varDecl)
					.fragments().get(0);
		} else if (varDecl.getNodeType() == ASTNode.FIELD_DECLARATION) {
			return (VariableDeclarationFragment) ((FieldDeclaration) varDecl)
					.fragments().get(0);
		} else {
			return null;
		}
	}

	//
	//
	//

	public static String replaceByNewValue(ASTNode node,
			TranslatorASTRewrite rew, ICompilationUnit fCu, Logger logger) {
		String replace = null;
		try {
			final CompilationUnit cu = (CompilationUnit) node.getRoot();
			final String beforeComment = getBeforeComment(node, fCu, cu,
					EMPTY_STRING, false);
			final String afterComment = getAfterComment(node, fCu, cu,
					EMPTY_STRING);
			//		
			/*
			 * if (rew.getEventStore().getChangeKind(node) !=
			 * RewriteEvent.UNCHANGED) { StructuralPropertyDescriptor property =
			 * node.getLocationInParent(); Object newValue =
			 * rew.getEventStore().getNewValue(node.getParent(), property); if
			 * (newValue != null) { if (property.isChildProperty()) { Object
			 * placeholderdata =
			 * rew.getInternalNodeStore().getPlaceholderData((ASTNode)
			 * newValue); if (placeholderdata != null) { int indexS =
			 * placeholderdata.toString().indexOf(":"); if (indexS > 0) {
			 * replace = beforeComment +
			 * placeholderdata.toString().substring(indexS + 1,
			 * placeholderdata.toString().length() - 1) + afterComment; return
			 * replace; } } else if (newValue != null) { replace = beforeComment
			 * + ASTNodes.asString((ASTNode) newValue) + afterComment; return
			 * replace; } replace = beforeComment +
			 * ASTFlattener.asString((ASTNode) newValue) + afterComment; return
			 * replace; } else if (property.isChildListProperty()) { ListRewrite
			 * lr = rew.getListRewrite(node.getParent(),
			 * (ChildListPropertyDescriptor) property); int index =
			 * lr.getOriginalList().indexOf(node); newValue =
			 * lr.getRewrittenList().get(index); Object placeholderdata =
			 * rew.getInternalNodeStore().getPlaceholderData((ASTNode)
			 * newValue); if (placeholderdata != null) { int indexS =
			 * placeholderdata.toString().indexOf(":"); if (indexS > 0) {
			 * replace = beforeComment +
			 * placeholderdata.toString().substring(indexS + 1,
			 * placeholderdata.toString().length() - 1) + afterComment; return
			 * replace; } } else if (newValue != null) { replace = beforeComment
			 * + ASTNodes.asString((ASTNode) newValue) + afterComment; return
			 * replace; } } }
			 */
			replace = getNewValueFromDocument(node, rew, fCu, beforeComment,
					afterComment);
		} catch (final Exception e) {
			logger.logException("Error during replaceByNewValue : ", e);
			replace = ASTFlattener.asString(node);
		}

		return replace;
	}

	private static String getNewValueFromDocument(ASTNode node,
			TranslatorASTRewrite rew, ICompilationUnit fCu,
			String beforeComment, String afterComment)
			throws JavaModelException, BadLocationException {
		String replace;
		final TextEdit tEdit = rew.rewriteAST();
		TranslationUtils.filter(tEdit, node);
		int offset = node.getStartPosition(); // hum hum
		final String value = fCu.getBuffer().getText(offset, node.getLength());
		if (tEdit.getOffset() < offset) {
			replace = beforeComment + value + afterComment;
		} else {
			tEdit.moveTree(-offset);
			final IDocument document = new SimpleDocument(value);
			tEdit.apply(document);
			replace = beforeComment + document.get() + afterComment;
		}		
		return replace;
	}

	private static String getAfterComment(ASTNode node, ICompilationUnit fCu,
			CompilationUnit cu, String afterComment) throws JavaModelException {
		/*
		 * int normalStart = node.getStartPosition(); int length =
		 * node.getLength(); int extendedLength = cu.getExtendedLength(node); if
		 * (length < extendedLength) { afterComment =
		 * fCu.getBuffer().getText(normalStart + length, extendedLength -
		 * length); } else {
		 */
		final int end = cu.lastTrailingCommentIndex(node);
		if (end >= 0) {
			final Object comment = cu.getCommentList().get(end);
			if (comment instanceof BlockComment) {
				final BlockComment bcomment = (BlockComment) comment;
				final int s = bcomment.getStartPosition();
				final int l = bcomment.getLength();
				afterComment = " " + fCu.getBuffer().getText(s, l);
			}
		}
		// }
		return afterComment;
	}

	@SuppressWarnings("unchecked")
	private static String getBeforeComment(ASTNode node, ICompilationUnit fCu,
			CompilationUnit cu, String beforeComment, boolean hardSearch)
			throws JavaModelException {
		final int normalStart = node.getStartPosition();
		final int extendedStart = cu.getExtendedStartPosition(node);
		if (extendedStart < normalStart) {
			beforeComment = fCu.getBuffer().getText(extendedStart,
					normalStart - extendedStart);
		} else {
			final int start = cu.firstLeadingCommentIndex(node);
			if (start >= 0) {
				final Object comment = cu.getCommentList().get(start);
				if (comment instanceof BlockComment) {
					final BlockComment bComment = (BlockComment) comment;
					final int s = bComment.getStartPosition();
					final int l = bComment.getLength();
					beforeComment = fCu.getBuffer().getText(s, l) + " ";
				} else if (comment instanceof LineComment) {
					final LineComment bComment = (LineComment) comment;
					final int s = bComment.getStartPosition();
					final int l = bComment.getLength();
					beforeComment = fCu.getBuffer().getText(s, l) + " ";
				}
			} else if (hardSearch) {
				// tricky way
				final List listComment = cu.getCommentList();
				for (int i = 0; i < listComment.size(); i++) {
					if (listComment.get(i) instanceof BlockComment) {
						final BlockComment bComment = (BlockComment) listComment
								.get(i);
						final int s = bComment.getStartPosition();
						final int l = bComment.getLength();
						final int ns = node.getStartPosition();
						if ((s < ns) && (ns - (s + l) >= 1)
								&& emptyOrNewline(fCu, (s + l), ns)) {
							beforeComment = fCu.getBuffer().getText(s, l) + " ";
						}
					}
				}
			}
		}
		return beforeComment;
	}

	@SuppressWarnings("unchecked")
	public static String searchForNonJavadocPackageComment(ASTNode node,
			ICompilationUnit fCu) throws JavaModelException {
		final CompilationUnit cu = (CompilationUnit) node.getRoot();
		String beforeComment = "";

		// tricky way
		final List listComment = cu.getCommentList();
	
		for (int i = 0; i < listComment.size(); i++) {
			if (listComment.get(i) instanceof BlockComment) {
				final BlockComment bComment = (BlockComment) listComment.get(i);
				final int s = bComment.getStartPosition();
				final int l = bComment.getLength();
				final int ns = node.getStartPosition();
				if ((s < ns) && (ns - (s + l) >= 1)
						&& emptyOrNewline(fCu, (s + l), ns)) {
					beforeComment = fCu.getBuffer().getText(s, l) + " ";														
					//
					break;
				}
			}
		}

		return beforeComment;
	}

	@SuppressWarnings("unchecked")
	public static TextEdit removeNonJavadocPackageComment(ASTNode node,
			ICompilationUnit fCu) throws JavaModelException {
		final CompilationUnit cu = (CompilationUnit) node.getRoot();
		// tricky way
		final List listComment = cu.getCommentList();
		final int extendedStart = cu.getExtendedStartPosition(node);
		for (int i = 0; i < listComment.size(); i++) {
			if (listComment.get(i) instanceof BlockComment) {
				final BlockComment bComment = (BlockComment) listComment.get(i);
				final int s = bComment.getStartPosition();
				final int l = bComment.getLength();
				final int ns = node.getStartPosition();
				if ((s < ns) && (ns - (s + l) >= 1)
						&& emptyOrNewline(fCu, (s + l), ns)) {
					return new DeleteEdit(s, extendedStart);
				}
			}
		}
		return null;
	}
	private static boolean emptyOrNewline(ICompilationUnit fCu, int i, int ns)
			throws JavaModelException {
		for (int j = i; j < ns; j++) {
			final char c = fCu.getBuffer().getChar(j);
			if (c != ' ' && c != '\r' && c != '\n' && c != '\t')
				return false;
		}
		return true;
	}

	private static void filter(TextEdit edit, ASTNode node) {
		final TextEdit[] children = edit.getChildren();
		for (final TextEdit child : children) {
			// If that edit is for a node located before
			if (child.getOffset() < node.getStartPosition()) {
				edit.removeChild(child);
			}
			// if that edit is for a node located after
			if (node.getStartPosition() + node.getLength() < child.getOffset()) {
				edit.removeChild(child);
			}
		}
	}

	public static String OLDreplaceByNewValue(ASTNode node,
			TranslatorASTRewrite rew, ICompilationUnit fCu, Logger logger) {
		String replace = null;
		try {
			final StructuralPropertyDescriptor property = node
					.getLocationInParent();
			if (property.isChildListProperty()) {
				replace = node.toString();
			} else {
				final Object value = rew.get(node, property);
				if (value == null) {
					replace = ASTFlattener.asString(node);
				}
				if (value instanceof ASTNode) {
					replace = ASTFlattener.asString((ASTNode) value);
				}

			}
		} catch (final Exception e) {
			replace = ASTFlattener.asString(node);
		}
		return replace;
	}

	public static String replaceByNewValue(ASTNode node, Logger logger) {
		String replace = null;
		replace = ASTFlattener.asString(node);
		return replace;
	}

	protected static String fillBuffer(ASTNode node, int pos_i) {
		final StringBuffer buffer = new StringBuffer();
		for (int i = 0; i < pos_i; i++) {
			buffer.append(" ");
		}
		buffer.append(ASTFlattener.asString(node));
		return buffer.toString();
	}

	protected static TextEdit filter(int pos_i, TextEdit te) {
		final TextEdit mte = new MultiTextEdit();
		for (final TextEdit t : te.getChildren()) {
			if (t.getOffset() == pos_i) {
				mte.addChild(t.copy());
			}
		}
		return mte;
	}

	//
	//
	//

	public static String formatIndexerCall(String expression,
			List<String> args, String value) {
		final StringBuilder builder = new StringBuilder();
		builder.append(expression);
		builder.append("[");

		for (int i = 0; i < args.size(); i++) {
			builder.append(args.get(i));
			if (i < args.size() - 1) {
				builder.append(", ");
			}
		}
		builder.append("]");
		builder.append("=");
		builder.append(value);
		return builder.toString();
	}

	public static String formatIndexerCall(String expression, List<String> args) {
		final StringBuilder builder = new StringBuilder();
		builder.append(expression);
		builder.append("[");

		for (int i = 0; i < args.size(); i++) {
			builder.append(args.get(i));
			if (i < args.size() - 1) {
				builder.append(", ");
			}
		}
		builder.append("]");
		return builder.toString();
	}

	public static String formatPropertyCall(String s_expression, String name) {
		final StringBuilder builder = new StringBuilder();
		if (s_expression != null) {
			builder.append(s_expression);
			builder.append(DOT);
		}
		builder.append(name);
		return builder.toString();
	}

	public static String formatPropertyCall(String s_expression, String name,
			String argument) {
		final StringBuilder builder = new StringBuilder();
		if (s_expression != null) {
			builder.append(s_expression);
			builder.append(DOT);
		}
		builder.append(name);
		builder.append(" = ");
		builder.append(argument);
		return builder.toString();
	}

	public static String formatPropertyDeclaration(String s_doc,
			List<String> s_modifiers, String s_returnType, String s_body,
			String name, PropertyRewriter.ProperyKind access) {
		final StringBuilder builder = new StringBuilder();
		builder.append(s_doc);
		builder.append("\n");

		for (final String mod : s_modifiers) {
			builder.append(mod + " ");
		}

		builder.append(s_returnType);
		builder.append(" ");
		builder.append(name);
		builder.append(" {\n");
		if ((access == PropertyRewriter.ProperyKind.READ)
				|| (access == PropertyRewriter.ProperyKind.READ_WRITE)) {
			//				
			String code = null;
			if (s_body != null) {
				code = s_body;
			} else {
				code = ";";
			}
			builder.append("\tget " + code + "\n");
		}
		if ((access == PropertyRewriter.ProperyKind.WRITE)
				|| (access == PropertyRewriter.ProperyKind.READ_WRITE)) {
			//
			builder.append("\tset");
			if (s_body == null) {
				builder.append(";\n");
			} else {
				builder.append(" { }\n"); // TODO
			}
		}
		builder.append("}\n");
		return builder.toString();
	}

	public static String formatTypeOf(String typeof, String name) {
		final StringBuilder builder = new StringBuilder();
		builder.append(typeof + "(");
		builder.append(name);
		builder.append(")");
		return builder.toString();
	}

	//
	//
	//

	@SuppressWarnings("unchecked")
	public static void getModifiersFromDoc(ITranslationContext context,
			BodyDeclaration node, ChangeModifierDescriptor desc) {
		final Javadoc doc = node.getJavadoc();
		boolean virtual = false;
		boolean override = false;

		if (doc != null) {
			final List tags = doc.tags();
			for (int i = 0; i < tags.size(); i++) {
				final TagElement te = (TagElement) tags.get(i);
				final String name = te.getTagName();
				if (name != null) {
					if (name.equals(context.getMapper().getTag(
							TranslationModelUtil.VIRTUAL_TAG))) {
						virtual = true;
					} else if (name.equals(context.getMapper().getTag(
							TranslationModelUtil.OVERRIDE_TAG))) {
						override = true;
					}
				}
			}
		}

		if (virtual) {
			desc.add(DotNetModifier.VIRTUAL);
		} else if (override) {
			desc.add(DotNetModifier.OVERRIDE);
		}
	}

	//
	//
	//

	public static boolean containsComments(ASTNode node) {
		if (node.getRoot().getNodeType() == ASTNode.COMPILATION_UNIT) {
			final CompilationUnit cU = (CompilationUnit) node.getRoot();
			final int cu_length = cU.getExtendedLength(node);
			final int n_length = node.getLength();
			return (cu_length != n_length);
		}
		return false;
	}

	//
	//
	//

	@SuppressWarnings("unchecked")
	public static String getTagValueFromDoc(BodyDeclaration node, String tag) {
		final Javadoc doc = node.getJavadoc();
		if (doc != null) {
			final List elements = doc.tags();
			for (int i = 0; i < elements.size(); i++) {
				final ASTNode astNode = (ASTNode) elements.get(i);
				if (astNode.getNodeType() == ASTNode.TAG_ELEMENT) {
					final TagElement tE = (TagElement) astNode;
					if (tE.fragments().size() > 0 && tE.getTagName() != null
							&& tE.getTagName().equals(tag)) {
						return ((TextElement) tE.fragments().get(0)).getText();
					}
				}
			}
		}
		return null;
	}

	@SuppressWarnings("unchecked")
	public static List<String> getTagValuesFromDoc(BodyDeclaration node,
			String tag) {
		final Javadoc doc = node.getJavadoc();

		if (doc != null) {
			final List<String> result = new ArrayList<String>();
			final List elements = doc.tags();
			for (int i = 0; i < elements.size(); i++) {
				final ASTNode astNode = (ASTNode) elements.get(i);
				if (astNode.getNodeType() == ASTNode.TAG_ELEMENT) {
					final TagElement tE = (TagElement) astNode;
					if (tE.fragments().size() > 0 && tE.getTagName() != null
							&& tE.getTagName().equals(tag)) {
						result.add(((TextElement) tE.fragments().get(0))
								.getText());
					}
				}
			}
			return result;
		}
		return null;
	}

	@SuppressWarnings("unchecked")
	public static TagElement getTagFromDoc(BodyDeclaration node, String tag) {
		final Javadoc doc = node.getJavadoc();
		if (doc != null) {
			final List elements = doc.tags();
			for (int i = 0; i < elements.size(); i++) {
				final ASTNode astNode = (ASTNode) elements.get(i);
				if (astNode.getNodeType() == ASTNode.TAG_ELEMENT) {
					final TagElement tE = (TagElement) astNode;
					if (tE.fragments().size() >= 0 && tE.getTagName() != null
							&& tE.getTagName().equals(tag)) {
						return tE;
					}
				}
			}
		}
		return null;
	}

	@SuppressWarnings("unchecked")
	public static boolean containsTag(BodyDeclaration node, String tag) {
		final Javadoc doc = node.getJavadoc();
		if (doc != null) {
			final List elements = doc.tags();
			for (int i = 0; i < elements.size(); i++) {
				final ASTNode astNode = (ASTNode) elements.get(i);
				if (astNode.getNodeType() == ASTNode.TAG_ELEMENT) {
					final TagElement tE = (TagElement) astNode;
					if (tE.getTagName() != null && tE.getTagName().equals(tag)) {
						return true;
					}
				}
			}
		}
		return false;
	}

	//
	// Annotation
	//

	public static void createAnnotation(TranslatorASTRewrite rew,
			BodyDeclaration node, String code) {
		final NormalAnnotation annot = (NormalAnnotation) rew
				.createStringPlaceholder(code, ASTNode.NORMAL_ANNOTATION);
		final ListRewrite lrewrite = rew.getListRewrite(node, node
				.getModifiersProperty());
		lrewrite.insertFirst(annot, null);
	}

	//
	// Comments
	//

	public static String getNodeWithComments(String replacemet, ASTNode node,
			ICompilationUnit fCu, ITranslationContext context) {
		return getNodeWithComments(replacemet, node, fCu, context, false);
	}

	public static String getNodeWithComments(String replacemet, ASTNode node,
			ICompilationUnit fCu, ITranslationContext context,
			boolean hardSearch) {
		final CompilationUnit cu = (CompilationUnit) node.getRoot();
		//	
		String beforeComment = EMPTY_STRING;
		String afterComment = EMPTY_STRING;
		try {
			beforeComment = getBeforeComment(node, fCu, cu, beforeComment,
					hardSearch);
			afterComment = getAfterComment(node, fCu, cu, afterComment);
		} catch (final Exception e) {
			context.getLogger().logException(
					"During comment processing ", e);
		}
		//
		return beforeComment + replacemet + afterComment;
	}

	public static String getBeforeComments(ASTNode node, ICompilationUnit fCu,
			ITranslationContext context) {
		final CompilationUnit cu = (CompilationUnit) node.getRoot();
		//	
		String beforeComment = EMPTY_STRING;
		try {
			beforeComment = getBeforeComment(node, fCu, cu, beforeComment,
					false);
		} catch (final Exception e) {
			context.getLogger().logException(
					"During comment processing ", e);
		}
		//
		return beforeComment;
	}

	public static String getBeforeComments(ASTNode node, ICompilationUnit fCu,
			ITranslationContext context, boolean hardSearch) {
		final CompilationUnit cu = (CompilationUnit) node.getRoot();
		//	
		String beforeComment = EMPTY_STRING;
		try {
			beforeComment = getBeforeComment(node, fCu, cu, beforeComment,
					hardSearch);
		} catch (final Exception e) {
			context.getLogger().logException(
					"During comment processing ", e);
		}
		//
		return beforeComment;
	}

	public static String getBeginBufferComments(PackageDeclaration node,
			ICompilationUnit fCu, ITranslationContext context) {
		final CompilationUnit cu = (CompilationUnit) node.getRoot();
		//	
		String beforeComment = EMPTY_STRING;
		try {
			final int i = node.getStartPosition();
			final int j = cu.getExtendedStartPosition(node);
			// from j to i it's comment
			if (i >= 0 && i > j)
				beforeComment = fCu.getBuffer().getText(j, i - j);
		} catch (final Exception e) {
			context.getLogger().logException(
					"During comment processing ", e);
		}
		return beforeComment;
	}

	public static String getAfterComments(ASTNode node, ICompilationUnit fCu,
			ITranslationContext context) {
		final CompilationUnit cu = (CompilationUnit) node.getRoot();
		//			
		String afterComment = EMPTY_STRING;
		try {
			afterComment = getAfterComment(node, fCu, cu, afterComment);
		} catch (final Exception e) {
			context.getLogger().logException(
					"During comment processing ", e);
		}
		//
		return afterComment;
	}

	//
	// Save
	//

	public static String computePathForSource(ITranslationContext context,
			ICompilationUnit icunit) {
		String destDir;
		boolean isATest = false;
		if (context.isATest(icunit)) {
			destDir = context.getConfiguration().getOptions().getTestsDestDir()
					+ File.separator;
			isATest = true;
		} else {
			destDir = context.getConfiguration().getOptions()
					.getSourcesDestDir()
					+ File.separator;
		}

		final String packageName = context.getPackageName(icunit.getPath()
				.toString(), icunit.getJavaProject().getProject());

		String path = destDir
				+ ((packageName == null) ? EMPTY_STRING : packageName.replace(
						DOT, File.separator));

		if (!context.getConfiguration().getOptions().getFlatPattern().equals(
				EMPTY_STRING)
				&& packageName != null) {
			final String flatPattern = context.getConfiguration().getOptions()
					.getFlatPattern();

			String[] patterns = flatPattern.split(",");
			final String targetPckName = findPackageMapping(context,
					packageName, icunit.getJavaProject().getProject());

			boolean replaced = false;
			int i = 0;
			while (i < patterns.length && !replaced) {
				String currentPattern = patterns[i];
				final String dest = findPackageMapping(context, currentPattern,
						icunit.getJavaProject().getProject());

				if (targetPckName.startsWith(dest)) {
					path = destDir
							+ File.separator
							+ targetPckName.substring(dest.length()).replace(
									DOT, File.separator);
					replaced = true;
				}
				i++;
			}
			if (!replaced && !isATest)
				context.getLogger().logInfo(
						"Can't map pck name = " + targetPckName
								+ " with that flatPatterns = " + flatPattern);
		}
		return path;
	}

	//
	//
	//

	public static String findPackageMapping(ITranslationContext context,
			String flatPattern, IProject reference) {
		final TargetPackage targetPackage = context.getModel()
				.findPackageMapping(flatPattern, reference);
		if (targetPackage != null) {
			return targetPackage.getName();
		} else {
			final PackageInfo pInfo = context.getModel().findPackageInfo(
					flatPattern, reference);
			if (pInfo != null) {
				String targetFramework = context.getConfiguration().getOptions().getTargetDotNetFramework().name();
				
				if (pInfo.getTarget(targetFramework) != null
						&& pInfo.getTarget(targetFramework).getPackageMappingBehavior() != null
						&& !(pInfo.getTarget(targetFramework).getPackageMappingBehavior() == PackageMappingPolicy.CAPITALIZED)) {
					return flatPattern;
				}
			}
			if (context.getConfiguration().getOptions()
					.getPackageMappingPolicy() == PackageMappingPolicy.NONE) {
				return flatPattern;
			}
			return Utils.capitalize(flatPattern);
		}
	}

	/*
	 * public static String findPackageMapping(ITranslationContext context,
	 * String flatPattern, IProject reference) { final TargetPackage
	 * targetPackage = context.getModel() .findPackageMapping(flatPattern,
	 * reference); if (targetPackage != null) { return targetPackage.getName();
	 * } else { final PackageInfo pInfo = context.getModel().findPackageInfo(
	 * flatPattern, reference); if (pInfo != null) { if
	 * (pInfo.getPackageMappingBehavior() != null &&
	 * !(pInfo.getPackageMappingBehavior() ==
	 * TranslatorProjectOptions.PackageMappingPolicy.CAPITALIZED)) { return
	 * flatPattern; } } if (context.getConfiguration().getOptions()
	 * .getPackageMappingPolicy() ==
	 * TranslatorProjectOptions.PackageMappingPolicy.NONE) { return flatPattern;
	 * } return Utils.capitalize(flatPattern); } }
	 */

	public static String defaultMappingForPackage(ITranslationContext context,
			String pck, IProject reference) {
		final PackageInfo pInfo = context.getModel().findPackageInfo(pck,
				reference);
		if (pInfo != null) {
			String targetFramework = context.getConfiguration().getOptions().getTargetDotNetFramework().name();
			
			if (pInfo.getTarget(targetFramework) != null
					&& pInfo.getTarget(targetFramework).getMemberMappingBehavior() != null
					&& pInfo.getTarget(targetFramework).getMemberMappingBehavior() != MethodMappingPolicy.CAPITALIZED)
				return pck;
		}
		if (context.getConfiguration().getOptions().getPackageMappingPolicy() == PackageMappingPolicy.NONE) {
			return pck;
		}
		return Utils.capitalize(pck);
	}

	/*
	 * public static String defaultMappingForPackage(ITranslationContext
	 * context, String pck, IProject reference) { final PackageInfo pInfo =
	 * context.getModel().findPackageInfo(pck, reference); if (pInfo != null) {
	 * if (pInfo.getMemberMappingBehavior() != null &&
	 * pInfo.getMemberMappingBehavior() !=
	 * TranslatorProjectOptions.MethodMappingPolicy.CAPITALIZED) return pck; }
	 * if (context.getConfiguration().getOptions().getPackageMappingPolicy() ==
	 * TranslatorProjectOptions.PackageMappingPolicy.NONE) { return pck; }
	 * return Utils.capitalize(pck); }
	 */

	public static boolean needCapitalization(ITranslationContext context,
			IMethod iMethod) throws JavaModelException,
			TranslationModelException {
		final IType iType = iMethod.getDeclaringType();
		final ClassInfo ci = context.getModel().findClassInfo(
				iType.getHandleIdentifier(), false, false, false);
		if (ci != null) {
			String targetFramework = context.getConfiguration().getOptions().getTargetDotNetFramework().name();
			
			if (ci.getTarget(targetFramework) != null) {
				if (ci.getTarget(targetFramework).getMemberMappingBehavior() != null) {
					if (ci.getTarget(targetFramework).getMemberMappingBehavior() != MethodMappingPolicy.CAPITALIZED)
						return false;
				}
			}
		}
		final IPackageFragment pf = iType.getPackageFragment();
		final String pName = pf.getElementName();
		final PackageInfo pInfo = context.getModel().findPackageInfo(pName,
				iType.getJavaProject().getProject());
		if (pInfo != null) {
			String targetFramework = context.getConfiguration().getOptions().getTargetDotNetFramework().name();
			
			if (pInfo.getTarget(targetFramework) != null
					&& pInfo.getTarget(targetFramework).getMemberMappingBehavior() != null
					&& pInfo.getTarget(targetFramework).getMemberMappingBehavior() != MethodMappingPolicy.CAPITALIZED)
				return false;
		}
		if (context.getConfiguration().getOptions().getMethodMappingPolicy() == MethodMappingPolicy.NONE) {
			return false;
		}
		return true;
	}

	/*
	 * public static boolean needCapitalization(ITranslationContext context,
	 * IMethod iMethod) throws JavaModelException, TranslationModelException {
	 * final IType iType = iMethod.getDeclaringType(); final ClassInfo ci =
	 * context.getModel().findClassInfo( iType.getHandleIdentifier(), false,
	 * false, false); if (ci != null) { if (ci.getMemberMappingBehavior() !=
	 * null) { if (ci.getMemberMappingBehavior() !=
	 * TranslatorProjectOptions.MethodMappingPolicy.CAPITALIZED) return false;
	 * else return true; } } final IPackageFragment pf =
	 * iType.getPackageFragment(); final String pName = pf.getElementName();
	 * final PackageInfo pInfo = context.getModel().findPackageInfo(pName,
	 * iType.getJavaProject().getProject()); if (pInfo != null) { if
	 * (pInfo.getMemberMappingBehavior() != null &&
	 * pInfo.getMemberMappingBehavior() !=
	 * TranslatorProjectOptions.MethodMappingPolicy.CAPITALIZED) return false; }
	 * if (context.getConfiguration().getOptions().getMethodMappingPolicy() ==
	 * TranslatorProjectOptions.MethodMappingPolicy.NONE) { return false; }
	 * return true; }
	 */
	//
	// JavaDoc
	//
	public static final String TRANSLATOR_TAG = "@translator_mapping";

	@SuppressWarnings("unchecked")
	public static void createOrUpdateTag(CompilationUnitRewrite cur,
			BodyDeclaration methodDecl, String tagName, String tagValue2) {
		if (TranslationUtils.containsTag(methodDecl, TRANSLATOR_TAG)) {
			final TagElement tag = TranslationUtils.getTagFromDoc(methodDecl,
					TRANSLATOR_TAG);
			final List list = tag.fragments();
			if (list == null)
				return;
			final TextElement textTag = (TextElement) list.get(0);
			String tagValue = textTag.getText();
			// looking for "tag = value"
			if (tagValue.contains(" " + tagName + " = ")) {
				/*
				 * if (tagValue .contains(" " + tagName + " = " + tagValue2 + ";
				 * }")) { // do nothing } else { // replace value }
				 */
			} else {
				// add value
				tagValue = tagValue.replace("}", " " + tagName + " = "
						+ tagValue2 + "; }");
				cur.getASTRewrite().replace(
						textTag,
						cur.getASTRewrite().createStringPlaceholder(tagValue,
								textTag.getNodeType()), null);
			}
		} else {
			final TagElement tag = cur.getAST().newTagElement();
			final List<TagElement> tags = new ArrayList<TagElement>();
			tag.setTagName(TRANSLATOR_TAG + " { " + tagName + " = " + tagValue2
					+ "; }");
			tags.add(tag);
			DocUtils
					.addTagsToDoc(cur.getASTRewrite(), methodDecl, tags);
		}
	}

	@SuppressWarnings("unchecked")
	public static void createOrUpdateTag(CompilationUnitRewrite cur,
			BodyDeclaration methodDecl, String tagName, String action,
			String oppositeAction, String tagValue) {
		TagElement tag = null;
		if (TranslationUtils.containsTag(methodDecl,
				TranslationUtils.TRANSLATOR_TAG)) {
			tag = TranslationUtils.getTagFromDoc(methodDecl,
					TranslationUtils.TRANSLATOR_TAG);
			final List list = tag.fragments();
			if (list == null)
				return;
			final TextElement textTag = (TextElement) list.get(0);
			String currentTagValue = textTag.getText();
			// looking for "modifiers = "
			final String pattern = " " + tagName + " = ";
			if (currentTagValue.contains(pattern)) {
				final int indexStart = currentTagValue.indexOf(pattern);
				final int indexend = currentTagValue.indexOf(";", indexStart
						+ pattern.length());
				final String subSeq = currentTagValue.substring(indexStart
						+ pattern.length(), indexend);
				final String[] modifiers = subSeq.split(",");
				String newModifiers = EMPTY_STRING;
				boolean found = false;
				for (int i = 0; i < modifiers.length; i++) {
					final String modifier = modifiers[i];
					if (modifier.endsWith(tagValue)) {
						final String curretnAction = modifier.substring(0, 1);
						if (curretnAction.equals(action)) {
							newModifiers += modifier;
						} else {
							// replace by "-"
							newModifiers += modifier.replace(oppositeAction,
									action);
						}
						found = true;
					} else {
						newModifiers += modifier;
					}
					if (i < modifiers.length - 1)
						newModifiers += ",";
				}
				if (!found) {
					newModifiers += "," + action + tagValue;
				}
				final String newValue = currentTagValue
						.substring(0, indexStart)
						+ pattern
						+ newModifiers
						+ currentTagValue.substring(indexend);
				cur.getASTRewrite().replace(
						textTag,
						cur.getASTRewrite().createStringPlaceholder(newValue,
								textTag.getNodeType()), null);
			} else {
				// add value
				currentTagValue = currentTagValue.replace("}", " " + tagName
						+ " = " + action + tagValue + "; }");
				cur.getASTRewrite().replace(
						textTag,
						cur.getASTRewrite().createStringPlaceholder(
								currentTagValue, textTag.getNodeType()), null);
			}
		} else {
			tag = cur.getAST().newTagElement();
			final List<TagElement> tags = new ArrayList<TagElement>();
			tag.setTagName(TranslationUtils.TRANSLATOR_TAG + " { " + tagName
					+ " = " + action + tagValue + "; }");
			tags.add(tag);
			DocUtils
					.addTagsToDoc(cur.getASTRewrite(), methodDecl, tags);
		}
		// return;
	}

	//
	//
	//

	public static boolean isAGeneratedConstantsClass(SimpleName name,
			ITranslationContext context) {
		return name.getFullyQualifiedName().contains(
				context.getMapper().getPrefixForConstants());
	}

	public static boolean isRenamedAnonymousClass(TypeDeclaration node,
			ITranslationContext context) {
		return node.getName().getFullyQualifiedName().startsWith(
				context.getMapper().getAnonymousClassNamePattern());
	}

	public static boolean isImplicteInnerRename(IType inner,
			ITranslationContext context) {
		final String pckName = inner.getPackageFragment().getElementName();
		final String typeName = inner.getElementName();
		return context.getModel().isAnImplicitNestedRename(pckName, typeName);
	}

	public static boolean isAnonymousRenamed(IType inner,
			ITranslationContext context) {
		return inner.getFullyQualifiedName().contains(
				context.getMapper().getAnonymousClassNamePattern());
	}

	//
	//
	//

	public static boolean isStaticFinal(int flags) {
		return Modifier.isStatic(flags) && Modifier.isFinal(flags);
	}

	//
	//
	//

	public static boolean isStringType(Type type) {
		return type.resolveBinding().getQualifiedName() != null
				&& type.resolveBinding().getQualifiedName().equals(FQN_STRING);
	}

	public static boolean isStringType(ITypeBinding type) {
		return type != null && type.getQualifiedName() != null
				&& type.getQualifiedName().equals(FQN_STRING);
	}

	public static boolean isPrimitiveType(Type type) {
		return type.isPrimitiveType() || isDotNetPrimitiveType(type);
	}

	public static boolean isObjectType(ITypeBinding type) {
		return type.getQualifiedName().equals(TranslationUtils.FQN_OBJECT);
	}

	public static boolean isCharType(Expression left) {
		final ITypeBinding itypeb = left.resolveTypeBinding();
		if (itypeb != null) {
			if (itypeb.isPrimitive() && itypeb.getQualifiedName().equals(CHAR)) {
				return true;
			}
		}
		return false;
	}

	public static boolean isPrimitiveOrWrapper(ITypeBinding type) {
		return type.isPrimitive() || isWrapper(type);
	}

	public static boolean isDoubleType(ITypeBinding itypeb) {
		return itypeb.isPrimitive() && itypeb.getQualifiedName().equals(DOUBLE);
	}

	public static boolean isFloatType(ITypeBinding itypeb) {
		return itypeb.isPrimitive() && itypeb.getQualifiedName().equals(FLOAT);
	}

	public static boolean isDotNetPrimitiveType(Type type) {
		if (type.isSimpleType()) {
			final SimpleType sType = (SimpleType) type;
			final Name tName = sType.getName();
			if (tName != null) {
				final String fqnName = tName.getFullyQualifiedName();
				if (fqnName != null) {
					return fqnName.equals(SBYTE) || fqnName.equals(USHORT)
							|| fqnName.equals(ULONG) || fqnName.equals(UINT);
				}
			}
		}
		return false;
	}

	public static boolean isWrapperOf(ITypeBinding wrapper,
			ITypeBinding primitiveType) {
		final String nameOfWrapper = wrapper.getQualifiedName();
		final String nameOfPrimitiveType = primitiveType.getQualifiedName();
		if (nameOfWrapper.equals(FQN_INT) && nameOfPrimitiveType.equals(INT)) {
			return true;
		} else if (nameOfWrapper.equals(FQN_LONG)
				&& nameOfPrimitiveType.equals(LONG)) {
			return true;
		} else if (nameOfWrapper.equals(FQN_CHAR)
				&& nameOfPrimitiveType.equals(CHAR)) {
			return true;
		} else if (nameOfWrapper.equals(FQN_BOOLEAN)
				&& nameOfPrimitiveType.equals(BOOLEAN)) {
			return true;
		} else if (nameOfWrapper.equals(FQN_DOUBLE)
				&& nameOfPrimitiveType.equals(DOUBLE)) {
			return true;
		} else if (nameOfWrapper.equals(FQN_FLOAT)
				&& nameOfPrimitiveType.equals(FLOAT)) {
			return true;
		} else if (nameOfWrapper.equals(FQN_SHORT)
				&& nameOfPrimitiveType.equals(SHORT)) {
			return true;
		} else if (nameOfWrapper.equals(FQN_BYTE)
				&& nameOfPrimitiveType.equals(BYTE)) {
			return true;
		}
		return false;
	}

	public static boolean isWrapper(ITypeBinding type) {
		final String name = type.getQualifiedName();
		if (name.equals(FQN_INT) || name.equals(FQN_LONG)
				|| name.equals(FQN_BOOLEAN) || name.equals(FQN_DOUBLE)
				|| name.equals(FQN_FLOAT) || name.equals(FQN_SHORT)
				|| name.equals(FQN_BYTE) || name.equals(FQN_CHAR))
			return true;
		return false;
	}

	//
	// Wrapper of primitive type
	//

	public static boolean isBooleanWrapper(ITypeBinding type) {
		return type.getQualifiedName().equals(FQN_BOOLEAN);
	}

	public static boolean isIntegerWrapper(ITypeBinding type) {
		return type.getQualifiedName().equals(FQN_INT);
	}

	public static boolean isLongWrapper(ITypeBinding type) {
		return type.getQualifiedName().equals(FQN_LONG);
	}

	public static boolean isCharacterWrapper(ITypeBinding type) {
		return type.getQualifiedName().equals(FQN_CHAR);
	}

	public static boolean isDoubleWrapper(ITypeBinding type) {
		return type.getQualifiedName().equals(FQN_DOUBLE);
	}

	public static boolean isFloatWrapper(ITypeBinding type) {
		return type.getQualifiedName().equals(FQN_FLOAT);
	}

	public static boolean isShortWrapper(ITypeBinding type) {
		return type.getQualifiedName().equals(FQN_SHORT);
	}

	public static boolean isByteWrapper(ITypeBinding type) {
		return type.getQualifiedName().equals(FQN_BYTE);
	}

	public static boolean isLongType(ITypeBinding type) {
		return type.isPrimitive() && type.getQualifiedName().equals(LONG);
	}

	public static boolean isCharType(ITypeBinding type) {
		return type.isPrimitive() && type.getQualifiedName().equals(CHAR);
	}

	public static boolean isIntType(ITypeBinding type) {
		return type.isPrimitive() && type.getQualifiedName().equals(INT);
	}

	//
	//
	//

	public static String createCommentForTypeOfNode(ASTNode node, Type type,
			ITranslationContext context, ICompilationUnit fCu) {
		final String afterComments = getAfterComments(type, fCu, context)
				.trim();
		String generic = EMPTY_STRING;
		if (afterComments.startsWith(START_INSERT_HERE_COMMENT)) {
			generic = afterComments.substring(15, afterComments.length() - 3);
		}
		if (type.isSimpleType()) {
			final SimpleType sType = (SimpleType) type;
			final Name name = sType.getName();
			if (name.isQualifiedName()) {
				final QualifiedName qname = (QualifiedName) name;
				if (node.getNodeType() == ASTNode.TYPE_LITERAL) {
					final TypeRewriter result = (TypeRewriter) context
							.getMapper().mapType(fCu, (TypeLiteral) node);
					if (result != null) {
						if (result.getInstanceOfTypeName() != null) {
							final String comment = START_TYPEOF_COMMENT
									+ result.getInstanceOfTypeName()
									+ END_TYPEOF_COMMENT;
							return comment;
						}
						String tName = TypeRewriter.filterGenerics(result
								.getName());
						if (tName == null) {
							tName = ASTNodes.asString(type);
						}
						String pName = result.getPackageName();
						if (pName != null) {
							pName += DOT;
						} else
							pName = EMPTY_STRING;
						final String comment = START_TYPEOF_COMMENT + pName
								+ tName + generic + END_TYPEOF_COMMENT;
						return comment;
					}
				}
				final PackageRewriter result = (PackageRewriter) context
						.getMapper().mapPackageAccess(
								qname,
								sType.resolveBinding().getJavaElement()
										.getJavaProject().getProject());
				if (result != null) {
					final String pName = result.getPackageName();
					final String tName = qname.getName().getIdentifier();

					final String comment = START_TYPEOF_COMMENT + pName + DOT
							+ tName + generic + END_TYPEOF_COMMENT;
					return comment;
				}
			} else {
				if (node.getNodeType() == ASTNode.TYPE_LITERAL) {
					final TypeRewriter result = (TypeRewriter) context
							.getMapper().mapType(fCu, (TypeLiteral) node);
					if (result != null) {
						if (result.getInstanceOfTypeName() != null) {
							final String comment = START_TYPEOF_COMMENT
									+ result.getInstanceOfTypeName()
									+ (result.isNullable()?"?":"")
									+ END_TYPEOF_COMMENT;
							return comment;
						}
						String tName = TypeRewriter.filterGenerics(result
								.getName());
						if (tName == null) {
							tName = ASTNodes.asString(type);
						}
						final String comment = START_TYPEOF_COMMENT + tName
								+ generic + (result.isNullable()?"?":"") + END_TYPEOF_COMMENT;
						return comment;
					}
				}
			}
		} else if (type.isPrimitiveType()) {
			final PrimitiveType priType = (PrimitiveType) type;
			final TargetClass tc = context.getModel()
					.findPrimitiveClassMapping(ASTNodes.asString(priType));
			String newName = ASTNodes.asString(priType);
			if (tc != null && tc.getShortName() != null) {
				newName = tc.getShortName();
			}
			final String comment = START_TYPEOF_COMMENT + newName + generic
					+ END_TYPEOF_COMMENT;
			return comment;
		} else if (type.isArrayType()) {
			final INodeRewriter nr = context.getMapper().mapArrayType(fCu,
					type.resolveBinding());
			String newName = null;
			if (nr instanceof TypeRewriter) {
				newName = TypeRewriter.filterGenerics(((TypeRewriter) nr)
						.getName());
			} else if (nr instanceof PrimitiveTypeRewriter) {
				newName = ((PrimitiveTypeRewriter) nr).getName();
			} else {
				newName = ASTNodes.asString(type);
			}
			/*
			 * if (newName != null && newName.equals("null[]")) { newName =
			 * ASTNodes.asString(type); }
			 */
			final String comment = START_TYPEOF_COMMENT + newName + generic
					+ END_TYPEOF_COMMENT;
			return comment;
		}
		final String comment = START_TYPEOF_COMMENT + ASTNodes.asString(type)
				+ generic + END_TYPEOF_COMMENT;
		return comment;
	}

	public static String extractValueToInsert(String comments) {
		final String trimmedValue = comments.trim();
		if (trimmedValue.startsWith(START_INSERT_HERE_COMMENT)) {
			final int length = trimmedValue.length() - 3;
			final String value = trimmedValue.substring(
					START_INSERT_HERE_COMMENT.length(), length);
			return value;
		}
		return null;
	}

	//
	//
	//

	public static String computeName(IType type) {
		boolean isGenerics = false;
		if (type.getElementName().contains("<")) {
			isGenerics = true;
		}
		return computeName(type, isGenerics);
	}

	public static String computeName(IType type, boolean generics) {
		try {
			String stp = EMPTY_STRING;
			if (generics) {
				final ITypeParameter[] tparams = type.getTypeParameters();

				if (tparams != null && tparams.length > 0) {
					stp += "<";
					for (int i = 0; i < tparams.length; i++) {
						stp += "%" + (i + 1);
						if (i < tparams.length - 1) {
							stp += ",";
						}
					}
					stp += ">";
				}
			}
			final String signature2 = type.getFullyQualifiedName() + stp;
			return signature2;
		} catch (final JavaModelException e) {
			return type.getElementName() + "ERROR";
		}
	}

	//
	//
	//

	public static String computeSimpleName(IType type, boolean generics) {
		try {
			String stp = EMPTY_STRING;
			if (generics) {
				final ITypeParameter[] tparams = type.getTypeParameters();

				if (tparams != null && tparams.length > 0) {
					stp += "<";
					for (int i = 0; i < tparams.length; i++) {
						stp += "%" + (i + 1);
						if (i < tparams.length - 1) {
							stp += ",";
						}
					}
					stp += ">";
				}
			}
			String enclosing = "";
			IType copy = type;
			while (copy.isMember()) {
				copy = copy.getDeclaringType();
				enclosing = copy.getElementName() + ".";
			}
			final String signature2 = enclosing + type.getElementName() + stp;
			return signature2;
		} catch (final JavaModelException e) {
			return type.getElementName() + "ERROR";
		}
	}

	/**
	 * Returns the qualified type name of the given type using '.' as
	 * separators. This is a replace for IType.getTypeQualifiedName() which uses
	 * '$' as separators. As '$' is also a valid character in an id this is
	 * ambiguous. JavaCore PR: 1GCFUNT
	 */
	public static String getTypeQualifiedName(IType type) {
		try {
			if (type.isBinary() && !type.isAnonymous()) {
				IType declaringType = type.getDeclaringType();
				if (declaringType != null) {
					return getTypeQualifiedName(declaringType) + '.'
							+ type.getElementName();
				}
			}
		} catch (JavaModelException e) {
			// ignore
		}
		return type.getTypeQualifiedName('.');
	}

	/**
	 * Returns the fully qualified name of the given type using '.' as
	 * separators. This is a replace for IType.getFullyQualifiedTypeName which
	 * uses '$' as separators. As '$' is also a valid character in an id this is
	 * ambiguous. JavaCore PR: 1GCFUNT
	 */
	public static String getFullyQualifiedName(IType type) {
		try {
			if (type.isBinary() && !type.isAnonymous()) {
				IType declaringType = type.getDeclaringType();
				if (declaringType != null) {
					return getFullyQualifiedName(declaringType) + '.'
							+ type.getElementName();
				}
			}
		} catch (JavaModelException e) {
			// ignore
		}
		return type.getFullyQualifiedName('.');
	}

	public static IType findTypeCustom2(IJavaProject jproject, IType type,
			String fullyQualifiedName) throws JavaModelException {
		// workaround for bug 22883
		IType superClass = jproject.findType(fullyQualifiedName,
				new NullProgressMonitor());
		if (superClass == null) {
			String[][] resolved = type.resolveType(fullyQualifiedName);
			if (resolved != null) {
				fullyQualifiedName = Signature.toQualifiedName(resolved[0]);
				superClass = jproject.findType(fullyQualifiedName,
						new NullProgressMonitor());
			}
			/*
			 * if (superClass == null) { // print out resolved String res = "";
			 * if (resolved != null) { for (int i = 0; i < resolved.length; i++)
			 * { for (int j = 0; j < resolved[i].length; j++) { res +=
			 * resolved[i][j] + " "; } res += " / "; } } this
			 * .logError("ClassInfo.computeParents:: Can't find Class " +
			 * fullyQualifiedName + "/" + res + " in project " +
			 * jproject.getElementName()); continue;
			 * 
			 * }
			 */
		}
		return superClass;
	}

	/**
	 * Finds a type by its qualified type name (dot separated).
	 * 
	 * @param jproject
	 *            The java project to search in
	 * @param fullyQualifiedName
	 *            The fully qualified name (type name with enclosing type names
	 *            and package (all separated by dots))
	 * @return The type found, or null if not existing
	 */
	public static IType findType(IJavaProject jproject,
			String fullyQualifiedName) throws JavaModelException {
		// workaround for bug 22883
		IType type = jproject.findType(fullyQualifiedName);
		if (type != null)
			return type;
		IPackageFragmentRoot[] roots = jproject.getPackageFragmentRoots();
		for (int i = 0; i < roots.length; i++) {
			IPackageFragmentRoot root = roots[i];
			type = findType(root, fullyQualifiedName);
			if (type != null && type.exists())
				return type;
		}
		return null;
	}

	private static IType findType(IPackageFragmentRoot root,
			String fullyQualifiedName) throws JavaModelException {
		IJavaElement[] children = root.getChildren();
		for (int i = 0; i < children.length; i++) {
			IJavaElement element = children[i];
			if (element.getElementType() == IJavaElement.PACKAGE_FRAGMENT) {
				IPackageFragment pack = (IPackageFragment) element;
				if (!fullyQualifiedName.startsWith(pack.getElementName()))
					continue;
				IType type = findType(pack, fullyQualifiedName);
				if (type != null && type.exists())
					return type;
			}
		}
		return null;
	}

	private static IType findType(IPackageFragment pack,
			String fullyQualifiedName) throws JavaModelException {
		ICompilationUnit[] cus = pack.getCompilationUnits();
		for (int i = 0; i < cus.length; i++) {
			ICompilationUnit unit = cus[i];
			IType type = findType(unit, fullyQualifiedName);
			if (type != null && type.exists())
				return type;
		}
		return null;
	}

	private static IType findType(ICompilationUnit cu, String fullyQualifiedName)
			throws JavaModelException {
		IType[] types = cu.getAllTypes();
		for (int i = 0; i < types.length; i++) {
			IType type = types[i];
			if (getFullyQualifiedName(type).equals(fullyQualifiedName))
				return type;
		}
		return null;
	}

	public static boolean isWrapperOfCorrespondingParam(ITypeBinding wrapper, int i,
			IMethodBinding method) {
		final ITypeBinding typeOfParam = method.getParameterTypes()[i];
		if (typeOfParam.isPrimitive()) {
			return isWrapperOf(wrapper, typeOfParam);
		}
		return false;
	}
}
