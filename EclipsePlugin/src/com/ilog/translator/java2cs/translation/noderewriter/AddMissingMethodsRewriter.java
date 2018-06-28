package com.ilog.translator.java2cs.translation.noderewriter;

import java.util.ArrayList;
import java.util.Collection;
import java.util.Iterator;
import java.util.List;
import java.util.Set;
import java.util.TreeSet;

import org.eclipse.core.runtime.NullProgressMonitor;
import org.eclipse.jdt.core.IMethod;
import org.eclipse.jdt.core.IType;
import org.eclipse.jdt.core.ITypeParameter;
import org.eclipse.jdt.core.JavaModelException;
import org.eclipse.jdt.core.Signature;
import org.eclipse.jdt.core.dom.AST;
import org.eclipse.jdt.core.dom.ASTNode;
import org.eclipse.jdt.core.dom.CompilationUnit;
import org.eclipse.jdt.core.dom.IMethodBinding;
import org.eclipse.jdt.core.dom.ITypeBinding;
import org.eclipse.jdt.core.dom.ImportDeclaration;
import org.eclipse.jdt.core.dom.Javadoc;
import org.eclipse.jdt.core.dom.MethodDeclaration;
import org.eclipse.jdt.core.dom.Modifier;
import org.eclipse.jdt.core.dom.Name;
import org.eclipse.jdt.core.dom.PrimitiveType;
import org.eclipse.jdt.core.dom.SimpleName;
import org.eclipse.jdt.core.dom.SingleVariableDeclaration;
import org.eclipse.jdt.core.dom.TagElement;
import org.eclipse.jdt.core.dom.TextElement;
import org.eclipse.jdt.core.dom.Type;
import org.eclipse.jdt.core.dom.TypeDeclaration;
import org.eclipse.jdt.core.dom.TypeParameter;
import org.eclipse.jdt.core.dom.Modifier.ModifierKeyword;
import org.eclipse.jdt.core.dom.rewrite.ASTRewrite;
import org.eclipse.jdt.core.dom.rewrite.ListRewrite;
import org.eclipse.text.edits.TextEditGroup;

import com.ilog.translator.java2cs.translation.ITranslationContext;
import com.ilog.translator.java2cs.translation.TranslatorASTRewrite;
import com.ilog.translator.java2cs.translation.util.TranslationUtils;

public class AddMissingMethodsRewriter extends AbstractNodeRewriter {

	//
	// Abstract Class Explicit Implement Method From Interface
	//
	private ITranslationContext context = null;

	//
	//
	//
	
	public AddMissingMethodsRewriter() {
	}

	//
	//
	//

	@Override
	public void process(ITranslationContext context, ASTNode node,
			TranslatorASTRewrite rew, TranslatorASTRewrite subRewriter,
			TextEditGroup description) {
		try {
			this.context = context;
			final TypeDeclaration typeDecl = (TypeDeclaration) node;
			final ITypeBinding binding = typeDecl.resolveBinding();
			addMissingMethods(typeDecl, binding, rew);
		} catch (final Exception e) {
			context.getLogger().logException(
					"During processing add missing method for node "
							+ fCu.getElementName(), e);
		}
	}

	//
	//
	//

	private void addMissingMethods(TypeDeclaration node, ITypeBinding type,
			ASTRewrite rew) throws JavaModelException {
		final List<IMethodBinding> allImplementedMethods = getAllMethodsFromClasses(type);
		final List<ITypeBinding> allInterfaces = getAllInterfaces(type);

		List<IMethodBinding> methodsToImplements = findMissingMethods(
				allImplementedMethods, allInterfaces);

		final Set<String> methodsImports = new TreeSet<String>();
		if (methodsToImplements.size() > 0) {
			final ListRewrite lr = rew.getListRewrite(node,
					TypeDeclaration.BODY_DECLARATIONS_PROPERTY);

			for (final IMethodBinding currentMethod : methodsToImplements) {
				// if (currentMethod.getTypeParameters() == null) {
				// TODO: Filter generic method ... not sur that works all
				// the times !
				final MethodDeclaration md = buildMethodDeclaration(node,
						currentMethod, rew, type, methodsImports);
				lr.insertLast(md, null);
			}
			addMissingImports(node, methodsImports, rew);
		}
		methodsToImplements = null;
	}

	@SuppressWarnings("unchecked")
	private MethodDeclaration buildMethodDeclaration(TypeDeclaration node,
			IMethodBinding currentMethod, ASTRewrite rew, ITypeBinding itype,
			Set<String> methodsImport) throws JavaModelException {

		final String name = currentMethod.getName();
		final String[] parameterNames = ((IMethod) currentMethod
				.getJavaElement()).getParameterNames();

		final boolean isVarArg = currentMethod.isVarargs();
		final ITypeBinding[] parameterTypes = currentMethod.getParameterTypes();
		final ITypeBinding[] exceptions = currentMethod.getExceptionTypes();
		final ITypeBinding retType = currentMethod.getReturnType();

		final int modifiers = currentMethod.getModifiers();

		final ITypeBinding typeThatDeclare = currentMethod.getDeclaringClass();

		// TODO : retType = eraseReturnType(currentMethod, retType);

		final AST ast = node.getAST();

		final MethodDeclaration md = ast.newMethodDeclaration();
		// Name
		final SimpleName methodName = ast.newSimpleName(name);
		md.setName(methodName);
		// modifiers
		md.modifiers().addAll(ast.newModifiers(modifiers));
		if (!Modifier.isAbstract(modifiers)) {
			md.modifiers().add(
					ast.newModifier(ModifierKeyword.ABSTRACT_KEYWORD));
		}
		if (!Modifier.isPublic(modifiers)) {
			md.modifiers().add(ast.newModifier(ModifierKeyword.PUBLIC_KEYWORD));
		}

		// Return type
		final Type returnType = this.buildType(rew, currentMethod,
				typeThatDeclare, retType, ast, itype, methodsImport, false);
		md.setReturnType2(returnType);

		for (final ITypeBinding currentException : exceptions) {
			final String tName = currentException.getName();
			final Name exc = ast.newSimpleName(tName);
			md.thrownExceptions().add(exc);
			addTypeToImport(rew, itype, typeThatDeclare, currentException,
					methodsImport);
		}

		for (int i = 0; i < parameterNames.length; i++) {
			final ITypeBinding currentParameterType = parameterTypes[i];
			final SingleVariableDeclaration decl = ast
					.newSingleVariableDeclaration();
			if (isVarArg && i == parameterNames.length - 1) {
				decl.setVarargs(true);
			}
			decl.setName(ast.newSimpleName(parameterNames[i]));
			final Type type = this.buildType(rew, currentMethod,
					typeThatDeclare, currentParameterType, ast, itype,
					methodsImport, isVarArg && i == parameterNames.length - 1);
			decl.setType(type);
			md.parameters().add(decl);
		}

		// TypeParameters
		final ITypeBinding[] typeParams = currentMethod.getTypeParameters();
		for (final ITypeBinding typeP : typeParams) {
			final TypeParameter tp = ast.newTypeParameter();
			tp.setName(ast.newSimpleName(typeP.getName()));
			md.typeParameters().add(tp);
		}

		// Javadoc
		final TagElement tag = ast.newTagElement();
		final TextElement text = ast.newTextElement();
		text.setText("from " + typeThatDeclare.getQualifiedName());
		tag.fragments().add(text);
		Javadoc doc = md.getJavadoc();
		if (doc == null) {
			doc = ast.newJavadoc();
			doc.tags().add(tag);
			rew.set(md, md.getJavadocProperty(), doc, null);
		} else {
			rew.getListRewrite(doc, Javadoc.TAGS_PROPERTY)
					.insertLast(tag, null);
		}

		return md;
	}

	private Type buildType(ASTRewrite rew, IMethodBinding method,
			ITypeBinding declaringType, ITypeBinding typeToBuild, AST ast,
			ITypeBinding itype, Set<String> methodsImport, boolean isVarArg)
			throws JavaModelException {
		if (typeToBuild.getName().equals("void"))
			return ast.newPrimitiveType(PrimitiveType.VOID);

		//
		Type returnType = null;
		if (typeToBuild.isTypeVariable()) {
			return (Type) rew.createStringPlaceholder(typeToBuild.getName(),
					ASTNode.PARAMETERIZED_TYPE);
		} else if (typeToBuild.isGenericType()) {
			return (Type) rew.createStringPlaceholder(typeToBuild.getName(),
					ASTNode.PARAMETERIZED_TYPE);
		} else if (typeToBuild.isParameterizedType()) {
			final ITypeBinding erasure = typeToBuild.getErasure();
			methodsImport.add(erasure.getQualifiedName());
			final String qualifiedName = TranslationUtils
					.getQualifiedNameWithQualifiedGenerics(typeToBuild);
			return (Type) rew.createStringPlaceholder(qualifiedName,
					ASTNode.PARAMETERIZED_TYPE);
		}

		final String foundedType = this.foundDefiningType(itype, typeToBuild);
		if (foundedType != null) {
			final PrimitiveType.Code code = PrimitiveType.toCode(foundedType);
			if (code != null) {
				returnType = ast.newPrimitiveType(code);
			} else {
				if (foundedType.contains("[]")) {
					final String arrayType = foundedType.substring(0,
							(foundedType.indexOf("[]")));
					final PrimitiveType.Code pcode = PrimitiveType
							.toCode(arrayType);
					if (pcode != null) {
						if (isVarArg) {
							returnType = ast.newPrimitiveType(pcode);
						} else {
							returnType = ast.newArrayType(ast
									.newPrimitiveType(pcode));
						}
					} else {
						if (isVarArg) {
							returnType = ast.newSimpleType(ast
									.newName(arrayType));
							methodsImport.add(arrayType);
						} else {
							returnType = ast.newArrayType(ast.newSimpleType(ast
									.newName(arrayType)));
							methodsImport.add(arrayType);
						}
					}
				} else {
					returnType = ast.newSimpleType(ast.newName(foundedType));
					methodsImport.add(foundedType);
				}
			}
		} else {
			returnType = ast.newSimpleType(ast.newName(typeToBuild
					.getQualifiedName()));
			methodsImport.add(typeToBuild.getQualifiedName());
		}
		return returnType;
	}

	private void addTypeToImport(ASTRewrite rew, ITypeBinding itype,
			ITypeBinding typeThatDeclare, ITypeBinding typeToImport,
			Set<String> imports) {
		imports.add(typeToImport.getQualifiedName());
	}

	private List<IMethodBinding> findMissingMethods(
			List<IMethodBinding> allImplementedMethods,
			List<ITypeBinding> allmethodsFromInterfaces)
			throws JavaModelException {
		final MethodsSet listOfMethods = new MethodsSet();
		for (final ITypeBinding currentInterface : allmethodsFromInterfaces) {
			final IMethodBinding[] methodsInter = currentInterface
					.getDeclaredMethods();
			for (final IMethodBinding currentInterfaceMethod : methodsInter) {
				listOfMethods.add(currentInterfaceMethod);
			}
		}
		//
		final List<IMethodBinding> methodsToImplements = new ArrayList<IMethodBinding>();

		final Iterator<IMethodBinding> iter = listOfMethods.getMethods()
				.iterator();
		while (iter.hasNext()) {
			final IMethodBinding currentInterfaceMethod = iter.next();
			if (!exist(currentInterfaceMethod, allImplementedMethods)) {
				methodsToImplements.add(currentInterfaceMethod);
				allImplementedMethods.add(currentInterfaceMethod);
			}
		}

		// TODO: Not so simple ... in case of repeated inheritance
		// method can have been defined in super class ....
		return methodsToImplements;
	}

	public static class MethodsSet {
		private final Collection<IMethodBinding> methods = new ArrayList<IMethodBinding>();

		public MethodsSet() {
		}

		public void add(IMethodBinding otherMethod) {
			for (final IMethodBinding currentMethod : methods) {

				final ITypeBinding currentMethodReturnType = currentMethod
						.getReturnType();
				final ITypeBinding otherMethodReturnType = otherMethod
						.getReturnType();
				boolean returnTypeCompatible = currentMethodReturnType
						.isAssignmentCompatible(otherMethodReturnType);
				if (currentMethod.isSubsignature(otherMethod)
						&& returnTypeCompatible) {
					methods.remove(currentMethod);
					methods.add(otherMethod);
					return;
				}
				if (currentMethod.isEqualTo(otherMethod)
						|| (otherMethod.isSubsignature(currentMethod) && !returnTypeCompatible)) {
					return;
				}
			}
			methods.add(otherMethod);
		}

		public Collection<IMethodBinding> getMethods() {
			return methods;
		}

	}

	private boolean exist(IMethodBinding currentInterfaceMethod,
			List<IMethodBinding> methods) throws JavaModelException {
		for (final IMethodBinding currentClassMethod : methods) {

			if (currentClassMethod.getName().equals(
					currentInterfaceMethod.getName())) {
				if (currentClassMethod.isSubsignature(currentInterfaceMethod)) {
					return true;
				}
			}
		}
		return false;
	}

	private List<IMethodBinding> getAllMethodsFromClasses(ITypeBinding type) {
		type.getSuperclass();

		final List<IMethodBinding> allMethods = new ArrayList<IMethodBinding>();

		allMethods.addAll(getMethods(type));

		final ITypeBinding superClass = type.getSuperclass();
		if (superClass != null) {
			allMethods.addAll(getAllMethodsFromClasses(superClass));
		}

		return allMethods;
	}

	private List<ITypeBinding> getAllInterfaces(ITypeBinding type) {
		type.getSuperclass();

		final List<ITypeBinding> allInterfaces = new ArrayList<ITypeBinding>();

		final ITypeBinding[] interfaces = type.getInterfaces();
		for (final ITypeBinding currentInterface : interfaces) {
			allInterfaces.addAll(getAllInterfaces(currentInterface));
			allInterfaces.add(currentInterface);
		}

		final ITypeBinding superClass = type.getSuperclass();
		if (superClass != null) {
			allInterfaces.addAll(getAllInterfaces(superClass));
		}

		return allInterfaces;
	}

	private Collection<? extends IMethodBinding> getMethods(
			ITypeBinding superClass) {
		final List<IMethodBinding> methods = new ArrayList<IMethodBinding>();
		final IMethodBinding[] declaredMethods = superClass
				.getDeclaredMethods();
		for (final IMethodBinding method : declaredMethods) {
			if (!method.isConstructor()) {
				methods.add(method);
			}
		}
		return methods;
	}


	private void addMissingImports(TypeDeclaration node, Set<String> imports,
			ASTRewrite rew) throws JavaModelException {
		if ((imports != null) && (imports.size() > 0)) {
			final AST ast = node.getAST();
			final ASTNode parent = getCompilationUnit(node);
			final ListRewrite importsR = rew.getListRewrite(parent,
					CompilationUnit.IMPORTS_PROPERTY);
			for (final String newImport : imports) {
				final ImportDeclaration imp = ast.newImportDeclaration();
				imp.setName(ast.newName(newImport));
				importsR.insertLast(imp, null);
			}
		}
	}

	private ASTNode getCompilationUnit(ASTNode node) {
		if (node.getParent().getNodeType() == ASTNode.COMPILATION_UNIT) {
			return node.getParent();
		} else {
			return getCompilationUnit(node.getParent());
		}
	}

	private String foundDefiningType(ITypeBinding itype,
			ITypeBinding typeToBuild) throws JavaModelException {
		return this.foundDefiningType((IType) itype.getJavaElement(),
				typeToBuild.getQualifiedName());
	}

	private String foundDefiningType(IType itype, String name)
			throws JavaModelException {
		for (int i = 0; i < itype.getSuperInterfaceNames().length; i++) {
			final String superName = itype.getSuperInterfaceNames()[i];
			final String[][] superR = itype.resolveType(superName);
			if (superR != null && superR.length > 0) {
				final IType r = itype.getJavaProject().findType(
						buildName(superR), new NullProgressMonitor());
				final ITypeParameter tp = r.getTypeParameter(name);
				if (tp.exists()) {
					if (tp.getBounds() != null && tp.getBounds().length > 0) {
						return tp.getBounds()[0];
					} else {
						return "Object";
					}
				}
				final String res = this.foundDefiningType(r, name);
				if (!res.equals(name)) {
					return res;
				}
			}
		}
		final String superName = itype.getSuperclassName();
		if (superName != null) {
			final String[][] superR = itype.resolveType(superName);
			if (superR != null && superR.length > 0) {
				final IType r = itype.getJavaProject().findType(
						buildName(superR));
				final ITypeParameter tp = r.getTypeParameter(name);
				if (tp.exists()) {
					if (tp.getBounds() != null && tp.getBounds().length > 0) {
						return tp.getBounds()[0];
					} else {
						return "Object";
					}
				}
				final String res = this.foundDefiningType(r, name);
				if (!res.equals(name)) {
					return res;
				}
			}
		}
		return name;
	}



	


	private String buildName(String[][] name) {
		String res = null;
		if (name != null && name.length > 0) {
			if (name[0].length > 1) {
				res = name[0][0] + "." + name[0][1];
			} else {
				res = name[0][0];
			}
		}
		return res;
	}

	protected static boolean areSimilarMethods(String name1, String[] params1,
			String name2, String[] params2, String[] simpleNames1) {

		if (name1.equals(name2)) {
			final int params1Length = params1.length;
			if (params1Length == params2.length) {
				for (int i = 0; i < params1Length; i++) {
					final String simpleName1 = simpleNames1 == null ? Signature
							.getSimpleName(Signature.toString(Signature
									.getTypeErasure(params1[i])))
							: simpleNames1[i];
					final String simpleName2 = Signature
							.getSimpleName(Signature.toString(Signature
									.getTypeErasure(params2[i])));
					if (!simpleName1.equals(simpleName2)) {
						return false;
					}
				}
				return true;
			}
		}
		return false;
	}
}
