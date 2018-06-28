package com.ilog.translator.java2cs.translation;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Set;
import java.util.TreeSet;

import org.eclipse.core.resources.IProject;
import org.eclipse.jdt.core.ICompilationUnit;
import org.eclipse.jdt.core.IJavaElement;
import org.eclipse.jdt.core.IMethod;
import org.eclipse.jdt.core.IType;
import org.eclipse.jdt.core.JavaModelException;
import org.eclipse.jdt.core.Signature;
import org.eclipse.jdt.core.dom.ASTNode;
import org.eclipse.jdt.core.dom.ASTVisitor;
import org.eclipse.jdt.core.dom.ArrayCreation;
import org.eclipse.jdt.core.dom.ArrayType;
import org.eclipse.jdt.core.dom.BreakStatement;
import org.eclipse.jdt.core.dom.ClassInstanceCreation;
import org.eclipse.jdt.core.dom.ContinueStatement;
import org.eclipse.jdt.core.dom.EnumDeclaration;
import org.eclipse.jdt.core.dom.FieldAccess;
import org.eclipse.jdt.core.dom.FieldDeclaration;
import org.eclipse.jdt.core.dom.IBinding;
import org.eclipse.jdt.core.dom.IMethodBinding;
import org.eclipse.jdt.core.dom.IPackageBinding;
import org.eclipse.jdt.core.dom.ITypeBinding;
import org.eclipse.jdt.core.dom.IVariableBinding;
import org.eclipse.jdt.core.dom.Initializer;
import org.eclipse.jdt.core.dom.LabeledStatement;
import org.eclipse.jdt.core.dom.MethodInvocation;
import org.eclipse.jdt.core.dom.Modifier;
import org.eclipse.jdt.core.dom.Name;
import org.eclipse.jdt.core.dom.PackageDeclaration;
import org.eclipse.jdt.core.dom.ParameterizedType;
import org.eclipse.jdt.core.dom.PrimitiveType;
import org.eclipse.jdt.core.dom.QualifiedName;
import org.eclipse.jdt.core.dom.QualifiedType;
import org.eclipse.jdt.core.dom.SimpleName;
import org.eclipse.jdt.core.dom.SimpleType;
import org.eclipse.jdt.core.dom.Statement;
import org.eclipse.jdt.core.dom.SuperMethodInvocation;
import org.eclipse.jdt.core.dom.Type;
import org.eclipse.jdt.core.dom.TypeDeclaration;
import org.eclipse.jdt.core.dom.TypeLiteral;
import org.eclipse.jdt.core.dom.VariableDeclaration;
import org.eclipse.jdt.core.dom.VariableDeclarationExpression;
import org.eclipse.jdt.core.dom.VariableDeclarationFragment;
import org.eclipse.jdt.core.dom.VariableDeclarationStatement;
import org.eclipse.jdt.internal.corext.dom.Bindings;
import org.eclipse.jdt.internal.corext.util.JavaModelUtil;

import com.ilog.translator.java2cs.configuration.ChangeModifierDescriptor;
import com.ilog.translator.java2cs.configuration.DotNetModifier;
import com.ilog.translator.java2cs.configuration.info.TranslationModelException;
import com.ilog.translator.java2cs.configuration.target.TargetClass;
import com.ilog.translator.java2cs.configuration.target.TargetField;
import com.ilog.translator.java2cs.configuration.target.TargetIndexer;
import com.ilog.translator.java2cs.configuration.target.TargetMethod;
import com.ilog.translator.java2cs.configuration.target.TargetPackage;
import com.ilog.translator.java2cs.configuration.target.TargetProperty;
import com.ilog.translator.java2cs.translation.noderewriter.BreakAndContinueLabelRewriter;
import com.ilog.translator.java2cs.translation.noderewriter.CompositeRewriter;
import com.ilog.translator.java2cs.translation.noderewriter.FieldRewriter;
import com.ilog.translator.java2cs.translation.noderewriter.INodeRewriter;
import com.ilog.translator.java2cs.translation.noderewriter.InitializerRewriter;
import com.ilog.translator.java2cs.translation.noderewriter.MethodRewriter;
import com.ilog.translator.java2cs.translation.noderewriter.PackageRewriter;
import com.ilog.translator.java2cs.translation.noderewriter.PrimitiveTypeRewriter;
import com.ilog.translator.java2cs.translation.noderewriter.PropertyRewriter;
import com.ilog.translator.java2cs.translation.noderewriter.TypeRewriter;
import com.ilog.translator.java2cs.translation.util.HandlerUtil;
import com.ilog.translator.java2cs.translation.util.TranslationUtils;
import com.ilog.translator.java2cs.util.CSharpModelUtil;
import com.ilog.translator.java2cs.util.Utils;

public class Java2CsMapper implements IMapper {

	private static final String JAVA_EXT = ".java";
	private final ITranslationContext context;

	//
	// Constructor
	//

	/**
	 * 
	 */
	public Java2CsMapper(ITranslationContext context) {
		this.context = context;
	}
	
	//
	// Declaration :
	// - Package
	// - Type
	// - Initializer
	// - Method
	// - Field
	//

	public INodeRewriter mapPackageDeclaration(PackageDeclaration node,
			ICompilationUnit icunit) {
		final IPackageBinding pck = node.resolveBinding();
		//
		if (pck != null) {
			return mapPackage(pck, icunit.getJavaProject().getProject());
		} else {
			final Name pckName = node.getName();
			final TargetPackage tp = context.getModel().findPackageMapping(
					pckName.getFullyQualifiedName(),
					icunit.getJavaProject().getProject());
			if (tp != null) {
				return tp.getRewriter();
			} else {
				final String newName = TranslationUtils
						.defaultMappingForPackage(context, pckName
								.getFullyQualifiedName(), icunit
								.getJavaProject().getProject());
				final INodeRewriter rewriter = new PackageRewriter(newName);
				return rewriter;
			}
		}
	}

	//
	// Type Declaration
	//

	public TargetClass getTargetClass(TypeDeclaration node) {
		final TargetClass tc = context.getModel().findClassMapping(
				node.resolveBinding().getJavaElement().getHandleIdentifier(),
				true, false);
		if (tc != null && tc.isRenamed()) {
			return tc;
		}
		return null;
	}
	
	public TargetClass getTargetClass(EnumDeclaration node) {
		final TargetClass tc = context.getModel().findClassMapping(
				node.resolveBinding().getJavaElement().getHandleIdentifier(),
				true, false);
		if (tc != null && tc.isRenamed()) {
			return tc;
		}
		return null;
	}

	//
	//
	//
	
	public INodeRewriter mapTypeDeclaration(ITypeBinding type, String handler) {
		if (handler == null) {
			handler = type.getJavaElement().getHandleIdentifier();
		}
		final TargetClass tc = context.getModel().findClassMapping(handler,
				true, true);
		INodeRewriter rewriter = null;
		if (tc != null) {
			rewriter = tc.getRewriter();
			if (type != null)
				addSourcePackage(tc, type.getJavaElement().getJavaProject()
						.getProject());
			else
				addSourcePackage(tc, null);
		} else {
			if (type != null && type.isParameterizedType()) {
				// String newName = type.getTypeDeclaration().getName();
				// TODO:
			} else {
				rewriter = new TypeRewriter(null, null, null, null, false, false,
						false, false); // TODO:
																					// what
																					// for
																					// ?
			}
		}

		return rewriter;
	}

	private void addSourcePackage(TargetClass tc, IProject reference) {
		if (tc.isRenamed()) {
			final Set<String> ns = new TreeSet<String>();
			String pck = tc.getSourcePackageName();
			final TargetPackage tPackage = context.getModel()
					.findPackageMapping(pck, reference);
			if (tPackage != null) {
				pck = tPackage.getName();
			} else {
				pck = TranslationUtils.defaultMappingForPackage(context, pck,
						reference);
			}
			ns.add(pck + ".*");
			final String cName = tc.getSourcePackageName() + "."
					+ tc.getShortName() + JAVA_EXT;
			context.addNamespaces(cName, ns);
		}
	}

	public INodeRewriter mapFieldDeclaration(FieldDeclaration node,
			String handler) {
		final TargetField tm = context.getModel().findFieldMapping(node.toString(), handler);
		if (tm != null) {
			return tm.getRewriter();
		} else {
			final VariableDeclarationFragment fragment = TranslationUtils
					.getFrament(node);
			return this.mapVariableDeclaration(fragment);
		}
	}

	//
	// Variable Declaration
	// 
	// SingleVariableDeclaration ?

	public INodeRewriter mapVariableDeclaration(
			VariableDeclarationExpression node) {
		final VariableDeclarationFragment fragment = TranslationUtils
				.getFrament(node);
		return this.mapVariableDeclaration(fragment);
	}

	public INodeRewriter mapVariableDeclaration(VariableDeclarationStatement node) {
		final VariableDeclarationFragment fragment = TranslationUtils
				.getFrament(node);
		return this.mapVariableDeclaration(fragment);
	}

	public INodeRewriter mapVariableDeclaration(VariableDeclaration fragment) {
		final String name = fragment.getName().getFullyQualifiedName();
		final String newName = context.getModel().getVariable(name);
		return new FieldRewriter(newName);
	}

	//
	// Method Declaration
	//

	public INodeRewriter mapMethodDeclaration(String className,
			String signature, String handler, boolean scan, boolean isOverride,
			boolean isGeneric) {
		try {
			final IJavaElement jElement = HandlerUtil.createElementFromHandler(handler);
			if (jElement != null) {
				final TargetMethod tm = context.getModel().findMethodMapping(
						className, signature, handler, scan, true, isOverride,
						isGeneric);
				final String name = jElement.getElementName();
				return fillRewriter(name, tm, TranslationUtils
						.needCapitalization(context, (IMethod) jElement));
			} else {
				context.getLogger().logError(
						"Java2CsMapper.mapMethodDeclaration(String, String) :: No JavaElement for <"
								+ className + "/" + handler + "/" + signature
								+ ">");
				return null;
			}
		} catch (final JavaModelException e) {
			e.printStackTrace();
			context.getLogger().logException("mapMethodDeclaration", e);
			return null;
		} catch (final TranslationModelException e) {
			context.getLogger().logException(
					"Java2CsMapper:mapMethodDeclaration: Unable to find model info for "
							+ className + " / " + signature + " / " + handler, e);
			return null;
		}
	}

	//
	// Initializer Declaration
	//

	public INodeRewriter mapInitializer(Initializer node) {
		if (node.getParent().getNodeType() == ASTNode.TYPE_DECLARATION) {
			final ITypeBinding mb = ((TypeDeclaration) node.getParent())
					.resolveBinding();
			return this.mapInitializer(mb, mb.getJavaElement()
					.getHandleIdentifier());
		} else {
			// enum for example
			return null;
		}
	}

	protected INodeRewriter mapInitializer(ITypeBinding type, String handler) {
		if (type == null) {
			context.getLogger().logError(
					"*** Error on initializer " + " : " + handler);
			return null;
		} else {
			return new InitializerRewriter(type.getName());
		}
	}

	//
	// Map Access
	// - method
	// - field/variable/qualifiedName
	// - type
	// - array
	// - labeledStatement
	// - package
	//

	//
	// MethodInvocation
	//

	public INodeRewriter mapMethodInvocation(MethodInvocation node,
			ICompilationUnit fCu) {
		final IMethodBinding mb = node.resolveMethodBinding();

		String newMethodName = node.getName().getIdentifier();

		if (mb == null) {
			context.getLogger().logWarning(
					"Java2CsMapper.mapMethodInvocation(MethodInvocation) *** Warning : "
							+ node.getName() + " has no binding / class "
							+ fCu.getElementName());
			newMethodName = Utils.capitalize(node.getName().getIdentifier());
			newMethodName = Utils.replaceForbiddenChar(newMethodName);
			return new MethodRewriter(newMethodName);
		} else {
			boolean isGeneric = false;
			if (context.getConfiguration().getOptions().isUseGenerics()) {
				final ITypeBinding tBinding = mb.getDeclaringClass();
				isGeneric = mb.isGenericMethod() || mb.isParameterizedMethod()
						|| mb.isRawMethod() || tBinding.isCapture()
						|| tBinding.isGenericType()
						|| tBinding.isParameterizedType();
			}
			try {
				if (mb.getJavaElement() == null) {
					final IJavaElement e = getJavaElement(mb);
					if (e == null) {
						return new MethodRewriter(newMethodName);
					} else {
						return this.mapMethodInvocation(fCu.getElementName(),
								mb, e.getHandleIdentifier(), isGeneric);
					}
				}
				return this.mapMethodInvocation(fCu.getElementName(), mb, mb
						.getJavaElement().getHandleIdentifier(), isGeneric);
			} catch (final JavaModelException e) {
				e.printStackTrace();
				context.getLogger().logException("mapMethodInvocation", e);
			}
		}
		return null;
	}

	private IJavaElement getJavaElement(IMethodBinding mb) {
		final IMethodBinding decl = mb.getMethodDeclaration();
		return decl.getJavaElement();
	}

	public INodeRewriter mapMethodInvocation(SuperMethodInvocation node,
			ICompilationUnit fCu) {
		final IMethodBinding mb = node.resolveMethodBinding();

		if (mb == null) {
			context.getLogger().logError(
					"Java2CsMapper  *** Error on method super method invocation"
							+ node.getStartPosition() + " : " + node.getName()
							+ " " + node.getParent());
		} else {
			try {
				final boolean isGeneric = true;
				return this.mapMethodInvocation(fCu.getElementName(), mb, mb
						.getJavaElement().getHandleIdentifier(), isGeneric);
			} catch (final JavaModelException e) {
				e.printStackTrace();
				context.getLogger().logException("mapMethodInvocation", e);
			}
		}
		return null;
	}

	public INodeRewriter mapMethodInvocation(ClassInstanceCreation node,
			ICompilationUnit fCu) {
		final IMethodBinding mb = node.resolveConstructorBinding();

		if (mb == null) {
			context.getLogger().logError(
					"Java2CsMapper.mapMethodInvocation *** Error on class instance creation : "
							+ node + " " + node.getParent() + " on class "
							+ fCu.getElementName());
		} else {
			final IJavaElement eleme = mb.getJavaElement();
			if (eleme != null) {
				final IMethod im = (IMethod) mb.getJavaElement();
				final ITypeBinding typeB = node.getType().resolveBinding();
				String[] params = null;
				// OK, the ugly case !!!
				// When it's an inner class, and the considering method is a
				// constructor
				// the corresponding IMethod contains an extra parameter that is
				// the enclosing
				// type !
				if (typeB.isNested()
						&& im.getNumberOfParameters() == 1
						&& im.getParameterTypes()[0].equals(Signature
								.createTypeSignature(typeB.getDeclaringClass()
										.getBinaryName(), true))) {
					params = new String[0];
				} else {
					params = im.getParameterTypes();
				}
				String className = typeB.getName();
				ITypeBinding declaringType = typeB;
				while (!declaringType.isTopLevel()) {
					declaringType = declaringType.getDeclaringClass();
					className = declaringType.getName() + "." + className;
				}
				final String packageName = declaringType.getPackage().getName();
				final TargetMethod tm = context.getModel()
						.findConstructorMapping(packageName, className, params,
								false, eleme.getHandleIdentifier());

				return fillRewriterForType(mb.getName(), tm);
			} else {
				if (mb.isDefaultConstructor()) {
					return null;
				}
				context.getLogger().logWarning(
						"Java2CsMapper.mapMethodInvocation(ClassInstanceCreation) *** Warning : "
								+ node + " has no JavaElement / class : "
								+ fCu.getElementName());
			}
		}
		return null;
	}

	private INodeRewriter mapMethodInvocation(String className,
			IMethodBinding node, String handleIdentifier, boolean isGeneric)
			throws JavaModelException {
		final IMethodBinding mb = node;

		IMethod eleme = (IMethod) node.getJavaElement();
		if (eleme == null) {
			eleme = (IMethod) getJavaElement(node);
		}

		final IMethodBinding raw = mb.getMethodDeclaration();
		try {
			final boolean isOverride = Bindings.findOverriddenMethod(mb, false) != null;
			if (!mb.isEqualTo(raw) || eleme.isResolved()) {
				/*final String signatureKey = TranslationUtils
						.computeSignature(eleme)
						+ node.getName();
				final TargetMethod tm = context.getModel().findMethodMapping(
						className, signatureKey, handleIdentifier, false,
						false, isOverride, isGeneric);
				return fillRewriter(mb.getName(), tm, TranslationUtils
						.needCapitalization(context, eleme));
			} else if (eleme.isResolved()) {*/
				final String signatureKey = TranslationUtils
						.computeSignature(eleme)
						+ node.getName();

				final TargetMethod tm = context.getModel().findMethodMapping(
						className, signatureKey, handleIdentifier, false,
						false, isOverride, isGeneric);

				return fillRewriter(mb.getName(), tm, TranslationUtils
						.needCapitalization(context, eleme));
			} else {
				return null;
			}
		} catch (final TranslationModelException e) {
			context.getLogger().logException(
					"Java2CsMapper:mapMethodInvocation: Unable to found model info for "
							+ node, e);
			return null;
		} catch (final JavaModelException e) {
			context.getLogger().logException(
					"Java2CsMapper:mapMethodInvocation: Java Model throw an exception "
							+ node + " ", e);
			return null;
		}
	}

	//
	// Array
	//

	public INodeRewriter mapArrayCreation(ICompilationUnit unit,
			ArrayCreation node) {
		final ITypeBinding mb = node.getType().getElementType()
				.resolveBinding();

		if (mb == null) {
			context.getLogger().logError(
					"Java2CsMapper *** Error on array creation "
							+ node.getStartPosition() + " : " + node + " "
							+ node.getParent());
		} else {
			return this.mapType(unit, mb);
		}
		return null;
	}

	protected INodeRewriter mapArrayFieldAccess(ITypeBinding array,
			IVariableBinding field) {
		if (array == null || field == null) {
			return null;
		}

		final TargetField tm = context.getModel().findArrayFieldMapping(array,
				field.getName());
		if (tm != null) {
			return tm.getRewriter();
		} else {
			return null;
		}
	}

	//
	// QualifiedName
	//

	// TODO : remove it and replace by mapName
	public INodeRewriter mapFieldAccess(ICompilationUnit fCu, QualifiedName node) {
		final SimpleName name = node.getName();
		final IBinding namebinding = name.resolveBinding();
		if (namebinding != null) {
			if (namebinding instanceof IVariableBinding) {
				// So it's a field access
				final IVariableBinding field = (IVariableBinding) namebinding;
				try {
					final INodeRewriter rew = this.mapFieldAccess(field, false); // it's
					// an access
					/* TODO:afau */
					if (rew != null && rew.hasFormat()) {
						return rew;
					}
					/* TODO:afau */
					final Name qual = node.getQualifier();
					if (qual != null && qual.resolveBinding() != null) {
						final INodeRewriter r2 = this.mapArrayType(fCu, node); // TODO
						if (r2 != null) {
							if (rew != null) {
								return new CompositeRewriter(rew, r2);
							} else {
								return r2;
							}
						}
					}
					return rew;
				} catch (final Exception e) {
					e.printStackTrace();
					context.getLogger().logException("", e);
				}
			} else if (namebinding instanceof ITypeBinding) {
				// TODO
				/*
				 * ITypeBinding binding = (ITypeBinding) namebinding;
				 * NodeRewriter result = mapType(fCu, binding);
				 * 
				 * return result;
				 */
			} else {
				// TODO
			}
		}
		return null;
	}

	//
	// Field
	//

	public INodeRewriter mapFieldAccess(FieldAccess node) {
		final IVariableBinding vb = node.resolveFieldBinding();

		if (vb == null) {
			context.getLogger().logError(
					"Java2CsMapper.mapFieldAccess *** Error on field access "
							+ node.getStartPosition() + " : " + node + " "
							+ node.getParent());
			return null;
		}

		if (node.getExpression() != null) {
			// Array field access (typically "length")
			final ITypeBinding typeBinding = node.getExpression()
					.resolveTypeBinding();
			if (typeBinding.isArray()) {
				return mapArrayFieldAccess(typeBinding, vb);
			}
		}

		return this.mapFieldAccess(vb, false);
	}

	public INodeRewriter mapFieldAccess(SimpleName node) {
		final IBinding namebinding = node.resolveBinding();
		if (namebinding != null) {
			if (namebinding instanceof IVariableBinding) {
				// So it's a field access
				final IVariableBinding field = (IVariableBinding) namebinding;
				try {
					return this.mapFieldAccess(field, false);
				} catch (final Exception e) {
					e.printStackTrace();
					context.getLogger().logException("", e);
				}
			}
		}
		return null;
	}

	public INodeRewriter mapFieldAccess(VariableDeclarationFragment node) {
		final SimpleName name = node.getName();
		final IVariableBinding field = (IVariableBinding) name.resolveBinding();
		if (field == null) {
			// TODO : context.getLogger().logError("Java2CsMapper mapFieldAccess
			// :: Binding is null for node : " + node);
			return null;
		} else {
			return this.mapFieldAccess(field, true);
		}
	}

	private INodeRewriter mapFieldAccess(IVariableBinding vb, boolean definition) {
		if (vb == null) {
			return null;
		}
		TargetField tm = null;
		// Binding have no java element when declared in static initializer ...
		if (vb.getJavaElement() != null) {
			tm = context.getModel().findFieldMapping(vb.getName(), 
					vb.getJavaElement().getHandleIdentifier());
		}
		if (tm != null) {
			return tm.getRewriter();
		} else {
			final String name = context.getModel().getVariable(vb.getName());
			if (name != null) {
				return new FieldRewriter(name);
			} else {
				if (definition) {
					final ChangeModifierDescriptor desc = new ChangeModifierDescriptor();
					desc.remove(DotNetModifier.FINAL);
					return new FieldRewriter(desc);
				}
			}
		}
		return null;
	}

	//
	// Map Type
	//

	public INodeRewriter mapType(ICompilationUnit fCu, TypeLiteral node) {
		final ITypeBinding binding = node.getType().resolveBinding();

		if (binding != null) {
			final INodeRewriter rewriter = this.mapType(fCu, binding);
			return rewriter;
		} else {
			context.getLogger().logError(
					"Java2CsMapper mapType node binding is null " + node + " "
							+ fCu.getElementName());
			return null;
		}
	}

	// TODO : remove it and replace by mapName
	public INodeRewriter mapType2(ICompilationUnit fCu, QualifiedName node) {
		final IBinding qualifierbinding = node.resolveBinding();
		if (qualifierbinding != null) {
			switch (qualifierbinding.getKind()) {
			case IBinding.TYPE: {
				final ITypeBinding binding = (ITypeBinding) qualifierbinding;
				return this.mapType(fCu, binding);
			}
			}
		}
		return null;
	}

	//
	// array type
	//

	public INodeRewriter mapArrayType(ICompilationUnit fCu, QualifiedName node) {
		final IBinding qualifierbinding = node.resolveBinding();
		final QualifiedName qName = node;
		final SimpleName name = qName.getName();
		final Name qualifier = qName.getQualifier();

		final IBinding namebinding = name.resolveBinding();
		if (qualifierbinding != null) {
			switch (qualifierbinding.getKind()) {
			case IBinding.VARIABLE: {
				if (namebinding instanceof IVariableBinding) {
					final IVariableBinding field = (IVariableBinding) namebinding;
					if (qualifier.resolveBinding() != null
							&& qualifier.resolveBinding().getKind() == IBinding.VARIABLE) {
						final IVariableBinding variable = (IVariableBinding) qualifier
								.resolveBinding();
						final ITypeBinding classOfVarible = variable.getType();
						if (classOfVarible != null && classOfVarible.isArray()) {
							// ok it's a call to a array field
							return mapArrayFieldAccess(classOfVarible, field);
						}
					}
				}
			}
			}
		}
		return null;
	}

	// TODO : remove it and replace by mapName
	public INodeRewriter mapType(ICompilationUnit fCu, Name node) {
		if (node.getNodeType() == ASTNode.QUALIFIED_NAME) {
			final QualifiedName qName = (QualifiedName) node;
			final Name qualifier = qName.getQualifier();
			final SimpleName name = qName.getName();
			final IBinding qualifierbinding = qualifier.resolveBinding();
			final IBinding namebinding = name.resolveBinding();
			if (qualifierbinding != null) {
				switch (qualifierbinding.getKind()) {
				case IBinding.TYPE: {
					final ITypeBinding binding = (ITypeBinding) qualifierbinding;
					return this.mapType(fCu, binding);
				}
				case IBinding.PACKAGE: {
					// TODO : cause trouble with some field ....
					final IPackageBinding binding = (IPackageBinding) qualifierbinding;
					if (!isImportOrPackageDecl(node)) {
						return mapPackage(binding, binding.getJavaElement()
								.getJavaProject().getProject());
					}
					return null;
				}
				case IBinding.VARIABLE: {
					if (namebinding instanceof IVariableBinding) {
						final IVariableBinding field = (IVariableBinding) namebinding;
						final IVariableBinding variable = (IVariableBinding) qualifierbinding;
						final ITypeBinding classOfVarible = variable.getType();
						if (classOfVarible != null && classOfVarible.isArray()) {
							// ok it's a call to a array field
							return mapArrayFieldAccess(classOfVarible, field);
						}
					}

				}
				}
			}
		} else if (node.getNodeType() == ASTNode.SIMPLE_NAME) {
			final SimpleName sName = (SimpleName) node;
			final IBinding namebinding = sName.resolveBinding();
			if (namebinding instanceof ITypeBinding) {
				final ITypeBinding binding = (ITypeBinding) namebinding;
				return this.mapType(fCu, binding);
			}
		}
		return null;
	}

	public INodeRewriter mapQualifiedType(ICompilationUnit cu, QualifiedType node) {
		final ITypeBinding type = node.resolveBinding();
		return this.mapSimpleType(cu, type);
	}

	public INodeRewriter mapSimpleType(ICompilationUnit icunit, SimpleType node) {
		final ITypeBinding type = node.resolveBinding();
		return this.mapSimpleType(icunit, type);
	}

	protected INodeRewriter mapSimpleType(ICompilationUnit icunit,
			ITypeBinding type) {
		if (type == null) {
			return null;
		}
		INodeRewriter rewriter = null;

		final TargetClass tc = context.getModel().findClassMapping(
				type.getJavaElement().getHandleIdentifier(), true,
				TranslationUtils.isGeneric(type));
		if (tc != null) {
			// addTargetPackage(tc);
			rewriter = tc.getRewriter();
		} else {
			if (type.isParameterizedType()) {
				// String newName = type.getTypeDeclaration().getName();
				// TODO:
			}
		}

		if (rewriter != null) {
			try {
				final Set<String> ns = rewriter.getNamespaces();
				String eName = icunit.getElementName();
				if (icunit.getPackageDeclarations() != null
						&& icunit.getPackageDeclarations().length > 0) {
					final String pck = icunit.getPackageDeclarations()[0]
							.getElementName();
					if (pck != null && !pck.equals("")) {
						eName = pck + CSharpModelUtil.CLASS_SEPARATOR + eName;
					}
				}
				context.addNamespaces(eName, ns);
			} catch (final JavaModelException e) {
				// e.printStackTrace();
				context.getLogger().logException("", e);
			}
		}

		if (type != null && type.isEnum()
				&& context.getConfiguration().getOptions().isNullableEnum()) {
			if (rewriter != null) {
				if (rewriter instanceof TypeRewriter)
					((TypeRewriter) rewriter).setNullable(true);
			} else {
				final String pckName = type.getPackage().getName();
				String className = type.getName();
				ITypeBinding currentType = type.getDeclaringClass();
				while (currentType != null) {
					className = currentType.getName() + "." + className;
					currentType = currentType.getDeclaringClass();
				}
				rewriter = new TypeRewriter(pckName, className, null, null, false,
						false, true, type.isMember());
			}
		}
		return rewriter;
	}

	protected INodeRewriter mapType(ICompilationUnit icunit, ITypeBinding type) {
		return this.mapType(icunit, type, true);
	}

	protected INodeRewriter mapType(ICompilationUnit icunit, ITypeBinding type,
			boolean useGenerics) {
		try {
			if (type.isPrimitive()) {
				return this.mapPrimitiveType(type);
			} else if (type.isArray()) {
				return this.mapArrayType(icunit, type);
			} else if (type.isParameterizedType()) {
				return this.mapParameterizedType(type);
			} else if (type.isClass() || type.isInterface() || type.isEnum()) {
				return this.mapSimpleType(icunit, type);
			} else if (type.isWildcardType()) {
				return mapWildcardType(type);
			} else {
				return null;
			}
		} catch (final Exception e) {
			e.printStackTrace();
			context.getLogger().logException("mapType", e);
			return null;
		}
	}

	public INodeRewriter mapPrimitiveType(PrimitiveType node) {
		final ITypeBinding type = node.resolveBinding();
		return this.mapPrimitiveType(type);
	}

	protected INodeRewriter mapPrimitiveType(ITypeBinding type) {
		if (type == null) {
			return null;
		}
		final TargetClass tc = context.getModel().findPrimitiveClassMapping(
				type.getName());
		if (tc != null) {
			return tc.getRewriter();
		} else {
			return null;
		}
	}

	public INodeRewriter mapArrayType(ICompilationUnit icunit, ITypeBinding at) {
		final INodeRewriter rew = mapType(icunit, at.getComponentType());
		if (rew instanceof TypeRewriter) {
			if (((TypeRewriter) rew).getName() == null) {
				return null;
			}
			final TypeRewriter tr = (TypeRewriter) rew.clone();
			tr.setName(TypeRewriter.filterGenerics(tr.getName()) + "[]");
			return tr;
		} else if (rew instanceof PrimitiveTypeRewriter) {
			final PrimitiveTypeRewriter ptr = (PrimitiveTypeRewriter) rew
					.clone();
			ptr.setName(ptr.getName() + "[]");
			return ptr;
		}
		return null;
	}

	public INodeRewriter mapParameterizedType(ICompilationUnit cu,
			ParameterizedType node) {
		try {
			final ITypeBinding typeB = node.resolveBinding();
			if (typeB != null && typeB.getJavaElement() != null) {
				final TargetClass tc = context.getModel()
						.findGenericClassMapping(
								typeB.getJavaElement().getHandleIdentifier());
				if (tc != null) {
					if (tc.getName() != null)
						context.addGenericImport(cu, tc.getName());
					return tc.getRewriter();
				}
			}
		} catch (final TranslationModelException e) {
			context.getLogger().logException(
					"Java2CsMapper:mapParameterizedType: Unable to found model info for "
							+ node + " in class " + cu.getElementName(), e);
		}
		return null;
	}

	private INodeRewriter mapParameterizedType(ITypeBinding pt) {
		return null;
	}

	private INodeRewriter mapWildcardType(ITypeBinding wt) {
		return null;
	}

	//
	// Labeled Statement
	//

	public INodeRewriter mapLabeledStatement(LabeledStatement node) {
		final SimpleName label = node.getLabel();
		final Statement body = node.getBody();

		final List<BreakStatement> blabels = searchBreakLabel(body, label);
		final List<ContinueStatement> clabels = searchContinueLabel(body, label);

		return new BreakAndContinueLabelRewriter(blabels, clabels);
	}

	//
	// package / import
	//

	public INodeRewriter mapPackageAccess(QualifiedName node, IProject reference) {
		final IBinding binding = node.getQualifier().resolveBinding();

		if (binding != null && binding.getKind() == IBinding.PACKAGE
				&& !isImportOrPackageDecl(node)) {
			final IPackageBinding pbinding = (IPackageBinding) binding;
			return mapPackage(pbinding, reference);
		}

		return null;
	}

	public boolean isRemovePackage(String packageName, IProject reference) {
		return context.getModel().isRemovedPackage(packageName, reference);
	}

	protected INodeRewriter mapPackage(IPackageBinding pck, IProject reference) {
		final TargetPackage tp = context.getModel().findPackageMapping(
				pck.getName(), reference);

		if (tp != null) {
			return tp.getRewriter();
		} else {
			final String pckName = pck.getName();
			final String newName = TranslationUtils.defaultMappingForPackage(
					context, pckName, reference);
			final INodeRewriter rewriter = new PackageRewriter(newName);
			return rewriter;
		}
	}

	//
	// Import
	//

	public String mapImport(String pck, IProject reference) {
		final TargetPackage tp = context.getModel().findImportMapping(pck,
				reference);
		if (tp != null) {
			return tp.getName();
		} else {
			final int index = pck.lastIndexOf(CSharpModelUtil.CLASS_SEPARATOR);
			if (index > 0) {
				final String supPck = pck.substring(0, index);
				final String rest = pck.substring(index + 1, pck.length());
				if (rest.equals("*")) {
					return this.mapImport(supPck, reference);
				} else {
					return this.mapImport(supPck, reference)
							+ CSharpModelUtil.CLASS_SEPARATOR
							+ TranslationUtils.defaultMappingForPackage(
									context, rest, reference);
				}
			} else {
				return TranslationUtils.defaultMappingForPackage(context, pck,
						reference);
			}
		}
	}

	public String mapImport(String pck, boolean onDemand, IProject reference) {
		if (onDemand) {
			return this.mapImport(pck, reference) + ".*";
		} else {
			try {
				final IType itype = context.getConfiguration()
						.getWorkingProject().findType(pck);
				if (itype == null) {
					// Strange but could happen ...
					return null;
				}
				final TargetClass tc = context.getModel().findClassMapping(
						itype.getHandleIdentifier(), true, true);
				if (tc == null
						|| (tc.getPackageName() == null && tc.getShortName() == null)) {
					final int idx = pck.lastIndexOf(".");
					final String pck2 = pck.substring(0, idx);
					final String map2 = this.mapImport(pck2, reference);
					if (map2 != null) {
						return map2 + "." + pck.substring(idx + 1);
					}
					return null;
				}
				return tc.getName();
			} catch (final JavaModelException e) {
				e.printStackTrace();
				context.getLogger().logException("", e);
			}
		}
		return null;
	}

	@SuppressWarnings("unchecked")
	public void addImportsFromSuperTypes(TypeDeclaration node,
			IProject reference) {
		final ITypeBinding binding = node.resolveBinding();
		final List superinterfaces = node.superInterfaceTypes();
		final Type superclass = node.getSuperclassType();

		final Set<String> imports = new TreeSet<String>();
		if (superclass != null) {
			final String imp = this.mapImport(getName(superclass), reference);
			if (imp != null) {
				imports.add(imp);
			}
		}
		if (superinterfaces != null && superinterfaces.size() > 0) {
			for (final Object superInter : superinterfaces) {
				final String imp = this.mapImport(getName((Type) superInter),
						reference);
				if (imp != null) {
					imports.add(imp);
				}
			}
		}
		//
		if (imports.size() > 0) {
			final IType type = (IType) binding.getJavaElement();
			JavaModelUtil.getTypeContainerName(type);
		}
	}

	//
	// Modifiers
	//

	public String mapModifiers(int modifiers) {
		Modifier.ModifierKeyword.PUBLIC_KEYWORD.toString();
		if (Modifier.isPublic(modifiers)) {
			return Modifier.ModifierKeyword.PUBLIC_KEYWORD.toString() + " ";//$NON-NLS-1$
		}
		if (Modifier.isProtected(modifiers)) {
			return Modifier.ModifierKeyword.PROTECTED_KEYWORD.toString() + " ";//$NON-NLS-1$
		}
		if (Modifier.isPrivate(modifiers)) {
			return Modifier.ModifierKeyword.PRIVATE_KEYWORD.toString() + " ";//$NON-NLS-1$
		}
		if (Modifier.isStatic(modifiers)) {
			return Modifier.ModifierKeyword.STATIC_KEYWORD.toString() + " ";//$NON-NLS-1$
		}
		if (Modifier.isAbstract(modifiers)) {
			return Modifier.ModifierKeyword.ABSTRACT_KEYWORD.toString() + " ";//$NON-NLS-1$
		}
		if (Modifier.isFinal(modifiers)) {
			return Modifier.ModifierKeyword.FINAL_KEYWORD.toString() + " ";//$NON-NLS-1$
		}
		if (Modifier.isSynchronized(modifiers)) {
			return Modifier.ModifierKeyword.SYNCHRONIZED_KEYWORD.toString()
					+ " ";//$NON-NLS-1$
		}
		if (Modifier.isVolatile(modifiers)) {
			return Modifier.ModifierKeyword.VOLATILE_KEYWORD.toString() + " ";//$NON-NLS-1$
		}
		if (Modifier.isNative(modifiers)) {
			return Modifier.ModifierKeyword.NATIVE_KEYWORD.toString() + " ";//$NON-NLS-1$
		}
		if (Modifier.isStrictfp(modifiers)) {
			return Modifier.ModifierKeyword.STRICTFP_KEYWORD.toString() + " ";//$NON-NLS-1$
		}
		if (Modifier.isTransient(modifiers)) {
			return Modifier.ModifierKeyword.TRANSIENT_KEYWORD.toString() + " ";//$NON-NLS-1$
		}
		return " ";
	}

	//
	// Keyword
	//

	public String getKeyword(int keyword_id, int model) {
		return context.getModel().getKeyword(keyword_id, model);
	}

	//
	// Attribute
	//

	public String getSynchronizedAttribute() {
		return context.getModel().getSynchronizedAttribute();
	}

	public String getSerializableAttribute() {
		return context.getModel().getSerializableAttribute();
	}

	public String getNotBrowsableAttribute() {
		return context.getModel().getNotBrowsableAttribute();
	}

	public String getPrefixForConstants() {
		return context.getModel().getPrefixForConstants();
	}

	public String getAnonymousClassNamePattern() {
		return context.getModel().getAnonymousClassNamePattern();
	}

	//
	// Tags
	//

	public String getTag(int id) {
		return context.getModel().getTag(id);
	}

	//
	// name
	//	

	private String getName(Type superclass) {
		final ITypeBinding itype = superclass.resolveBinding();
		final IPackageBinding pbinding = itype.getPackage();
		return pbinding.getName();
	}

	private INodeRewriter fillRewriter(String methodName, TargetMethod tm,
			boolean needCapitalization) {
		String newMethodName = methodName;
		if (needCapitalization)
			newMethodName = Utils.capitalize(methodName);
		newMethodName = Utils.replaceForbiddenChar(newMethodName);
		if (tm != null) {
			if (tm.getPattern() == null && tm.getName() == null) {
				if (tm.getRewriter() instanceof MethodRewriter) {
					final MethodRewriter rew = (MethodRewriter) tm
							.getRewriter();
					rew.setName(newMethodName);
					return rew;
				} else if (tm.getRewriter() instanceof PropertyRewriter) {
					// TODO: Do I decide to not capitalize in case of property ?
					final PropertyRewriter rew = (PropertyRewriter) tm
							.getRewriter();
					if (rew.getName() == null)
						rew.setName(newMethodName);
					return rew;
				}
				return tm.getRewriter();
			} else {
				return tm.getRewriter();
			}
		} else {
			// If the default behavior of the translator is to capitalize
			if (needCapitalization
					&& Character.isLowerCase(methodName.charAt(0))) {
				return new MethodRewriter(newMethodName);
			} else {
				return new MethodRewriter();
			}
		}
	}

	private INodeRewriter fillRewriterForType(String typeName, TargetMethod tm) {
		if (tm != null) {
			if (tm.getPattern() == null && tm.getName() == null) {
				if (tm.getRewriter() instanceof MethodRewriter) {
					final MethodRewriter rew = (MethodRewriter) tm
							.getRewriter();
					rew.setName(typeName);
					return rew;
				} else if (tm.getRewriter() instanceof PropertyRewriter) {
					final PropertyRewriter rew = (PropertyRewriter) tm
							.getRewriter();
					rew.setName(typeName);
					return rew;
				}
				return tm.getRewriter();
			} else {
				return tm.getRewriter();
			}
		} else {
			// Default behavior ot the translator is to capitalize
			// TODO: mb.geName() can be "" witch raised an exception in
			// capitalize ...
			// return new MethodRewriter();
			return null;
		}
	}

	private boolean isImportOrPackageDecl(ASTNode node) {
		if (node == null) {
			return false;
		}
		switch (node.getNodeType()) {
		case ASTNode.IMPORT_DECLARATION:
		case ASTNode.PACKAGE_DECLARATION:
			return true;
		case ASTNode.COMPILATION_UNIT:
			return false;
		default:
			return isImportOrPackageDecl(node.getParent());
		}
	}

	private List<BreakStatement> searchBreakLabel(Statement body,
			SimpleName label) {
		final SearchBreakLabelVisitor search = new SearchBreakLabelVisitor();
		body.accept(search);
		return search.getBrealLabels();
	}

	private List<ContinueStatement> searchContinueLabel(Statement body,
			SimpleName label) {
		final SearchBreakLabelVisitor search = new SearchBreakLabelVisitor();
		body.accept(search);
		return search.getContinueLabels();
	}

	//
	// properties
	//

	public List<TargetProperty> getProperties(TypeDeclaration node,
			String handler) {
		final List<TargetProperty> properties = context.getModel()
				.findProperties(handler,
						TranslationUtils.isGeneric(node.resolveBinding()));
		return properties;
	}

	public List<TargetIndexer> getIndexers(TypeDeclaration node, String handler) {
		final List<TargetIndexer> indexers = context.getModel().findIndexers(
				handler);
		return indexers;
	}

	//
	// jagged array creation
	//

	public String mapJaggedArrayCreation(ArrayCreation node, String comments,
			String[] args) {
		final ArrayType arrayType = node.getType();
		String pattern = "";
		pattern += "(" + arrayType.getElementType() + "[][])";
		pattern += "ILOG.J2CsMapping.Collections.Arrays.CreateJaggedArray(";
		pattern += comments + " " + arrayType.getElementType() + ".class, ";
		for (int i = 0; i < args.length; i++) {
			final String new_arg = args[i];
			pattern += new_arg;
			if (i < args.length - 1) {
				pattern += ", ";
			}
		}
		pattern += ")";
		return pattern;
	}

	//
	// enum
	//
	
	public String mapEnumValues(ITypeBinding enumType) {
		String enumName = "";
		if (enumType.isMember()) {
			enumName = enumType.getDeclaringClass().getName() + ".";
		}
		enumName += enumType.getName();
		final String s = "(" + enumName + "[]) Enum.GetValues(typeof("
				+ enumName + "))";
		return s;
	}

	public String mapEnumValueOf(ITypeBinding enumType, String arg) {
		String enumName = "";
		if (enumType.isMember()) {
			enumName = enumType.getDeclaringClass().getName() + ".";
		}
		enumName += enumType.getName();
		final String s = "(" + enumName + ") Enum.Parse(typeof(" + enumName
				+ "), " + arg + ")";
		return s;
	}

	public String mapURS(String sLeft, String sRight) {
		final String mCall = "ILOG.J2CsMapping.Util" + "." + "MathUtil.URS("
				+ sLeft + "," + sRight + ")";
		return mCall;
	}

	//
	// SearchBreakLabelVisitor
	//

	private static class SearchBreakLabelVisitor extends ASTVisitor {

		protected List<BreakStatement> breakLabels = new ArrayList<BreakStatement>();
		protected List<ContinueStatement> continueLabels = new ArrayList<ContinueStatement>();

		public List<BreakStatement> getBrealLabels() {
			return breakLabels;
		}

		public List<ContinueStatement> getContinueLabels() {
			return continueLabels;
		}

		/*
		 * @see ASTVisitor#visit(BreakStatement)
		 */
		@Override
		public boolean visit(BreakStatement node) {
			final SimpleName label = node.getLabel();
			if (label != null) {
				// Well it's a break label
				breakLabels.add(node);
			}
			return true;
		}

		@Override
		public boolean visit(ContinueStatement node) {
			final SimpleName label = node.getLabel();
			if (label != null) {
				// Well it's a break label
				continueLabels.add(node);
			}
			return true;
		}
	}

	public HashMap<String, String> getJavaDocMappings() {
		return context.getModel().getJavaDocMappings();
	}

	public String getJavaDocTagMapping(String tag) {
		return context.getModel().getJavaDocTagMapping(tag);
	}
	
	//
	// disclaimer
	//
	
	public String getDisclaimer() {
		return context.getModel().getDisclaimer();
	}
}
