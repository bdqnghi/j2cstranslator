/**
 * 
 */
package com.ilog.translator.java2cs.translation;

import java.lang.reflect.Modifier;
import java.util.HashMap;

import org.eclipse.core.runtime.CoreException;
import org.eclipse.core.runtime.IProgressMonitor;
import org.eclipse.core.runtime.NullProgressMonitor;
import org.eclipse.jdt.core.Flags;
import org.eclipse.jdt.core.ICompilationUnit;
import org.eclipse.jdt.core.IMethod;
import org.eclipse.jdt.core.IType;
import org.eclipse.jdt.core.ITypeHierarchy;
import org.eclipse.jdt.core.JavaModelException;
import org.eclipse.jdt.core.dom.AST;
import org.eclipse.jdt.core.dom.BodyDeclaration;
import org.eclipse.jdt.core.dom.CompilationUnit;
import org.eclipse.jdt.core.dom.Javadoc;
import org.eclipse.jdt.core.dom.MethodDeclaration;
import org.eclipse.jdt.core.dom.TagElement;
import org.eclipse.jdt.core.dom.rewrite.ListRewrite;
import org.eclipse.jdt.internal.corext.util.JavaModelUtil;
import org.eclipse.jdt.internal.corext.util.MethodOverrideTester;
import org.eclipse.jdt.internal.corext.util.SuperTypeHierarchyCache;
import org.eclipse.ltk.core.refactoring.Change;
import org.eclipse.ltk.core.refactoring.RefactoringStatus;
import org.eclipse.text.edits.TextEditGroup;

import com.ilog.translator.java2cs.translation.astrewriter.ASTRewriterVisitor;
import com.ilog.translator.java2cs.util.TranslationModelUtil;

public class MethodHierarchy {

	public static enum OVERRIDE_MODIFIER {
		VIRTUAL, OVERRIDE
	};
	
	/**
	 * 
	 */
	private final ITranslationContext context;
	private HashMap<String, OVERRIDE_MODIFIER> virtualOverrideList = new HashMap<String, OVERRIDE_MODIFIER>();

	//
	//
	//

	/**
	 * @param context
	 */
	MethodHierarchy(ITranslationContext context) {
		this.context = context;
	}

	//
	//
	//

	public void computeVirtualOverride(ICompilationUnit icunit)
			throws CoreException {
		final IType[] types = icunit.getAllTypes();
		for (final IType type : types) {
			final NullProgressMonitor monitor = new NullProgressMonitor();
			final ITypeHierarchy hierarchy = SuperTypeHierarchyCache.getTypeHierarchy(type); // type.newTypeHierarchy(monitor);
			//
			final IType[] subTypes = hierarchy.getAllSubtypes(type);
			//
			final MethodOverrideTester tester = new MethodOverrideTester(type,
					hierarchy);
			//
			boolean classIsFinal = Flags.isFinal(type.getFlags()); // TODO
			final IMethod[] methods = type.getMethods();
			//
			for (final IMethod method : methods) {
				boolean methodIsFinal = Flags.isFinal(method.getFlags());
				if (!method.isConstructor()
						&& !Flags.isPrivate(method.getFlags())
						&& virtualOverrideList
								.get(method.getHandleIdentifier()) == null) {
					// Valid conditions for compute virtual/override are :
					// not a method, not private and not already computed
					IMethod declaringMethod = null;
					if (type.isClass()) {
						declaringMethod = tester.findDeclaringMethod(method,
								false);
						if (declaringMethod != null
								&& declaringMethod.getDeclaringType().isClass()) {
							if (!declaringMethod.getDeclaringType().isBinary()) {
								// mark declaringMethod as virtual
								changeModifier(declaringMethod,
										OVERRIDE_MODIFIER.VIRTUAL);
							}
							// MARK method as override
							if (isMethodFromObject(declaringMethod)) {
								// Special case for some method coming from
								// Object
								// and that not necessary exist/have the
								// same behavior
								// in Java and in C#
								final IMethod overridenMethod = tester
										.findOverriddenMethod(method, false);
								if (overridenMethod == declaringMethod) {
									if (!classIsFinal && !methodIsFinal)
										changeModifier(method,
												OVERRIDE_MODIFIER.VIRTUAL);
								} else {
									changeModifier(method,
											OVERRIDE_MODIFIER.OVERRIDE);
								}
							} else {
								if (CheckOverriding(declaringMethod, method,
										type))
									changeModifier(method,
											OVERRIDE_MODIFIER.OVERRIDE);
							}
						} else if (declaringMethod != null
								&& !declaringMethod.getDeclaringType()
										.isClass()) {
							// Check if it's the first class to implements
							// that
							// interface ?
							//
							final IMethod overridenMethod = tester
									.findOverriddenMethod(method, false);
							if (overridenMethod != null
									&& overridenMethod != declaringMethod
									&& !Flags.isAbstract(overridenMethod
											.getFlags())
									&& overridenMethod.getDeclaringType()
											.isClass()) {
								// If overriden method exists, is different of
								// the declaring method
								// is not abstract, and its declaring class is
								// not an interface
								/*
								 * if (overridenMethod.getDeclaringType()
								 * .getSuperclassName() != null) { }
								 */
								changeModifier(method,
										OVERRIDE_MODIFIER.OVERRIDE);

							} else {
								// If the first implementation of the method
								// is not abstract add override !
								if (Flags
										.isAbstract(overridenMethod.getFlags())
										&& overridenMethod.getDeclaringType()
												.isClass())
									changeModifier(method,
											OVERRIDE_MODIFIER.OVERRIDE);
								else
								// if somewhere is the hierarchy there is an
								// abstract class and that abstract class is not
								// on the
								// current project !
								// we can't put it as VIRTUAL, because the
								// translation
								// of that abstract class will add that
								// method !
								// IType[] allSuperClasses =
								// hierarchy.getAllSuperclasses(type);
								if (hasAbstractClass(type, declaringMethod
										.getDeclaringType(), hierarchy)) {
									changeModifier(method,
											OVERRIDE_MODIFIER.OVERRIDE);
								} else {
									if (!classIsFinal && !methodIsFinal)
										changeModifier(method,
												OVERRIDE_MODIFIER.VIRTUAL);
								}
							}
						}

						for (final IType subType : subTypes) {
							final IMethod overriding = tester
									.findOverridingMethodInType(subType, method);
							if (overriding != null
									&& CheckOverriding(overriding, method, type)) {
								// mark overring as override
								// if (!isObjectCloneMethod(type,
								// overriding))
								changeModifier(overriding,
										OVERRIDE_MODIFIER.OVERRIDE);
								if (declaringMethod == null) {
									// mark method as virtual
									if (!classIsFinal && !methodIsFinal)
										changeModifier(method,
												OVERRIDE_MODIFIER.VIRTUAL);
								}
							}
						}
					}
				}
			}
		}
	}

	private boolean CheckOverriding(IMethod overriding, IMethod method,
			IType contextType) throws JavaModelException {
		final String[] overPType = overriding.getParameterTypes();
		final String[] mthdPType = method.getParameterTypes();

		for (int i = 0; i < overPType.length; i++) {
			final String overTN = resolveTypeNameInContext(overPType[i],
					overriding.getDeclaringType());
			final String mthdTN = resolveTypeNameInContext(mthdPType[i],
					contextType);
			/*
			 * boolean subOne = overPType[i].contains(mthdPType[i]); boolean
			 * subTwo = mthdPType[i].contains(overPType[i]); if (!subOne &&
			 * !subTwo)
			 */
			if (overTN == null || mthdTN == null)
				return true; // Can't decide .. may be a type parameter ?
			if (!overTN.equals(mthdTN))
				return false;
		}
		return true;
	}

	private String resolveTypeNameInContext(String typeName, IType context)
			throws JavaModelException {
		final String resolved = JavaModelUtil.getResolvedTypeName(typeName,
				context);
		if (resolved != null)
			return resolved;
		/*
		 * NullProgressMonitor npm = new NullProgressMonitor(); ITypeHierarchy
		 * hierarchy = context.newSupertypeHierarchy(npm); for(IType current :
		 * hierarchy.getAllSupertypes(context)) { resolved =
		 * JavaModelUtil.getResolvedTypeName(typeName, current); if (resolved !=
		 * null) return resolved; }
		 */
		return null;
	}

	private boolean isMethodFromObject(IMethod method)
			throws JavaModelException {
		final IType type = method.getDeclaringType();
		if (type.getFullyQualifiedName().equals("java.lang.Object")) {
			return (method.getNumberOfParameters() == 0)
					&& (method.getElementName().equals("clone") || method
							.getElementName().equals("finalize"));
		} else {
			return false;
		}
	}

	//
	//
	//

	private boolean hasAbstractClass(IType type, IType typeThatDeclare,
			ITypeHierarchy hierarchy) throws CoreException {
		final IType[] allSuperClasses = hierarchy.getAllSuperclasses(type);
		if (!typeThatDeclare.isClass()
				&& contains(hierarchy.getSuperInterfaces(type),
						typeThatDeclare, hierarchy)) {
			boolean result = false;
			for (final IType superClass : allSuperClasses) {
				result = result
						|| contains(hierarchy.getSuperInterfaces(superClass),
								typeThatDeclare, hierarchy);
			}
			if (!result)
				return false;
		}
		for (final IType currentType : allSuperClasses) {
			if (Flags.isAbstract(currentType.getFlags())
					&& contains(hierarchy.getSuperInterfaces(currentType),
							typeThatDeclare, hierarchy)) {
				// && currentType implements/extends typeThatDeclare
				return true;
			}
			if (currentType == typeThatDeclare
					|| contains(hierarchy.getSuperInterfaces(currentType),
							typeThatDeclare, hierarchy)) {
				return false;
			}
		}
		return false;
	}

	private boolean contains(IType[] superInterfaces, IType typeThatDeclare,
			ITypeHierarchy hierarchy) {
		for (final IType currentInterface : superInterfaces) {
			final String cuKey = currentInterface.getKey();
			final String tdKey = typeThatDeclare.getKey();
			/*
			 * String cuHandler = currentInterface.getHandleIdentifier(); String
			 * tdHandler = typeThatDeclare.getHandleIdentifier();
			 */
			if (currentInterface == typeThatDeclare
					|| cuKey.equals(tdKey)
					|| contains(hierarchy.getSuperInterfaces(currentInterface),
							typeThatDeclare, hierarchy)) {
				return true;
			}
		}
		return false;
	}

	private void changeModifier(IMethod method, OVERRIDE_MODIFIER newModifier) {
  	// ------------------>DON'T PUT ABSTRACT VIRTUAL
      try {
        int flags = method.getFlags();
        if ( ( newModifier == OVERRIDE_MODIFIER.VIRTUAL ) && ( Modifier.isAbstract( flags ) || Modifier.isStatic( flags ) ) ) return;
      } catch ( JavaModelException e ) {
        System.err
        .println("ERROR: Trying to get method's flags for preventing abstract virtual methods.");
      }
      // ------------------>DON'T PUT ABSTRACT VIRTUAL
		final Object oldValue = virtualOverrideList.get(method
				.getHandleIdentifier());
		if (oldValue == null || newModifier == OVERRIDE_MODIFIER.OVERRIDE) {
			virtualOverrideList.put(method.getHandleIdentifier(), newModifier);
		} else {
			// override always win ?
			if (oldValue != newModifier) {
				System.err
						.println("ERROR: Trying to change already defined modifier ("
								+ oldValue
								+ " by "
								+ newModifier
								+ ") on method " + method);
			}
		}
	}

	public void createVirtualOverridTag(ICompilationUnit icunit)
			throws Exception {
		final NullProgressMonitor pm = new NullProgressMonitor();
		final CompilationUnit cUnit = Translator
				.parseAbridged(pm, icunit, true);
		final FillVirtualOverrideTag v = new FillVirtualOverrideTag(context);
		v.setTable(virtualOverrideList);
		v.setCompilationUnit(icunit);
		v.transform(pm, cUnit);
		v.applyChange(pm);
		v.postAction(icunit, cUnit);
	}

	//
	//
	//

	final static class FillVirtualOverrideTag extends ASTRewriterVisitor {
		@SuppressWarnings("unchecked")
		private HashMap set;

		public FillVirtualOverrideTag(ITranslationContext context) {
			super(context);
			transformerName = "Fill Virtual/Override Tag Declaration";
			description = new TextEditGroup(transformerName);
		}

		//
		//
		//

		@SuppressWarnings("unchecked")
		public void setTable(HashMap set) {
			this.set = set;
		}

		@Override
		public boolean applyChange(IProgressMonitor pm) throws CoreException {
			final Change change = createChange(pm, null);
			final IProgressMonitor subMonitor = new NullProgressMonitor();

			try {
				change.initializeValidationData(subMonitor);

				if (!change.isEnabled()) {
					return false;
				}
				final RefactoringStatus valid = change.isValid(subMonitor);
				if (valid.hasFatalError()) {
					return false;
				}
				final Change undo = change.perform(subMonitor);
				if (undo != null) {
					undo.initializeValidationData(subMonitor);
					// do something with the undo object
				}
			} finally {
				change.dispose();
			}
			return true;
		}

		@Override
		public boolean visit(MethodDeclaration node) {
			final AST ast = node.getAST();
			if (node.resolveBinding() == null) {
				context.getLogger().logError(
						"Error : binding null for " + node + " on "
								+ fCu.getElementName());
				return false;
			} else if (node.resolveBinding().getJavaElement() == null) {
				context.getLogger().logError(
						"Error : no JavaElement for " + node + " on "
								+ fCu.getElementName());
				return false;
			}
			final String id = node.resolveBinding().getJavaElement()
					.getHandleIdentifier();
			final Object value = set.get(id);
			if (value != null) {
				final TagElement bindingTag = ast.newTagElement();
				if (value == OVERRIDE_MODIFIER.OVERRIDE) {
					bindingTag.setTagName(context.getMapper().getTag(
							TranslationModelUtil.OVERRIDE_TAG));
				} else { // VIRTUAL
					bindingTag.setTagName(context.getMapper().getTag(
							TranslationModelUtil.VIRTUAL_TAG));
				}
				addTagsToDoc(node, bindingTag);
			}
			return false;
		}

		@SuppressWarnings("unchecked")
		private void addTagsToDoc(BodyDeclaration node, TagElement bindingTag) {
			final AST ast = node.getAST();
			Javadoc doc = node.getJavadoc();
			if (doc == null) {
				doc = ast.newJavadoc();
				doc.tags().add(bindingTag);
				currentRewriter.set(node, node.getJavadocProperty(), doc, null);
			} else {
				final ListRewrite lr = currentRewriter.getListRewrite(doc,
						Javadoc.TAGS_PROPERTY);
				lr.insertLast(bindingTag, null);
			}
		}
	}
}