package com.ilog.translator.java2cs.translation.astrewriter.astchange;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

import org.eclipse.core.runtime.CoreException;
import org.eclipse.core.runtime.IProgressMonitor;
import org.eclipse.core.runtime.NullProgressMonitor;
import org.eclipse.jdt.core.Flags;
import org.eclipse.jdt.core.IType;
import org.eclipse.jdt.core.ITypeHierarchy;
import org.eclipse.jdt.core.JavaModelException;
import org.eclipse.jdt.core.dom.ASTNode;
import org.eclipse.jdt.core.dom.ASTVisitor;
import org.eclipse.jdt.core.dom.CompilationUnit;
import org.eclipse.jdt.core.dom.ITypeBinding;
import org.eclipse.jdt.core.dom.IVariableBinding;
import org.eclipse.jdt.core.dom.MethodDeclaration;
import org.eclipse.jdt.core.dom.SingleVariableDeclaration;
import org.eclipse.jdt.core.dom.Type;
import org.eclipse.jdt.core.dom.TypeDeclaration;
import org.eclipse.jdt.internal.corext.codemanipulation.CodeGenerationSettings;
import org.eclipse.jdt.internal.corext.dom.ASTNodes;
import org.eclipse.jdt.internal.corext.dom.Bindings;
import org.eclipse.jdt.internal.corext.refactoring.JavaRefactoringArguments;
import org.eclipse.jdt.internal.ui.preferences.JavaPreferencesSettings;
import org.eclipse.ltk.core.refactoring.Change;
import org.eclipse.ltk.core.refactoring.RefactoringStatus;
import org.eclipse.text.edits.TextEditGroup;

import com.ilog.translator.java2cs.translation.ITranslationContext;
import com.ilog.translator.java2cs.translation.util.TranslationUtils;

public class MoveInterfaceInnerOrNesterClassToTopVisitor extends
		ASTChangeVisitor {

	private static String REF_TO_OUTER_NAME = "outer";

	private HasInterfaceVisitor visitor = null;

	private List<String> history = new ArrayList<String>();

	//
	//
	//

	public MoveInterfaceInnerOrNesterClassToTopVisitor(
			ITranslationContext context) {
		super(context);
		transformerName = "Move Inner Interface or Nested Class To Top";
		description = new TextEditGroup(transformerName);
	}

	//
	//
	//

	@Override
	public void reset() {
		super.reset();
		history = new ArrayList<String>();
	}

	//
	//
	//

	@Override
	public boolean runAgain(CompilationUnit unit) {
		visitor = new HasInterfaceVisitor();
		unit.accept(visitor);
		if (visitor.getResult() != null) {
			change = null;
			return true;
		} else {
			return false;
		}

	}

	@Override
	public boolean runOnce() {
		return false; // ;
	}

	//
	//
	//

	@Override
	public boolean needValidation() {
		return true;
	}

	//
	//
	//

	@Override
	public boolean visit(TypeDeclaration node) {
		try {
			final IProgressMonitor pm = new NullProgressMonitor();
			final TypeDeclaration currentInnerDecl = visitor.getResult();
			final IType currentInnerType = (IType) currentInnerDecl
					.resolveBinding().getJavaElement();
			final TypeDeclaration parent = (TypeDeclaration) ASTNodes
					.getParent(currentInnerDecl, ASTNode.TYPE_DECLARATION);

			boolean refEnclosing = false;
			boolean needOuterField = true;
			if (!node.isInterface()) {
				if (currentInnerDecl.getName().getIdentifier().contains(
						context.getModel().getAnonymousClassNamePattern())
						&& superIsRenamed(currentInnerDecl, node)) {
					refEnclosing = false;
				} else if (currentInnerDecl.getName().getIdentifier().contains(
						context.getModel().getAnonymousClassNamePattern())
						&& hasOuter(currentInnerDecl, parent /* node */)) {
					refEnclosing = true;
					needOuterField = false;
				} else {
					refEnclosing = true;
				}
			}

			boolean enclosingIsSuperOfInner = false;

			if (Flags.isStatic(currentInnerType.getFlags())) {
				final IType thisType = (IType) node.resolveBinding()
						.getJavaElement();
				final ITypeHierarchy hierarchy = currentInnerType
						.newSupertypeHierarchy(new NullProgressMonitor());
				if (hierarchy.contains(thisType)) {
					enclosingIsSuperOfInner = true;
				}
			}

			change = this.createChange(pm, parent /* node */,
					currentInnerType, refEnclosing, needOuterField,
					enclosingIsSuperOfInner);

			// Add to the output conf file an hint to said
			// that if in dependent project we have z.new B(arg1)
			// we need to replace it by new B(z, arg1);
			String cName = currentInnerType.getElementName();
			IType currentType = currentInnerType.getDeclaringType();
			while (currentType != null) {
				cName = currentType.getElementName() + "." + cName;
				currentType = currentType.getDeclaringType();
			}
			context.getModel().addImplicitNestedToInnerTransformation(
					currentInnerType.getPackageFragment().getElementName(),
					cName);

		} catch (final Exception e) {
			context.getLogger().logException("visit(TypeDeclaration)", e);
		}
		// one level only !
		return false;
	}

	//
	//
	//

	@SuppressWarnings("unchecked")
	private boolean hasOuter(TypeDeclaration currentInnerDecl,
			TypeDeclaration enclosing) {
		final MethodDeclaration[] methods = currentInnerDecl.getMethods();
		for (final MethodDeclaration method : methods) {
			if (method.isConstructor()) {
				final List params = method.parameters();
				if (!containsEnclosing(params, enclosing)) {
					return false;
				}
			}
		}
		return true;
	}

	@SuppressWarnings("unchecked")
	private boolean containsEnclosing(List params, TypeDeclaration enclosing) {
		// Verify if parameters list contains a reference on enclosing type
		// means a parameter such as is type is enclosing type.
		for (int i = 0; i < params.size(); i++) {
			final SingleVariableDeclaration varDecl = (SingleVariableDeclaration) params
					.get(i);
			if (varDecl.resolveBinding().getType().getErasure().isEqualTo(
					enclosing.resolveBinding().getErasure())) {
				return true;
			}
		}
		return false;
	}

	private boolean superIsRenamed(TypeDeclaration currentInnerDecl,
			TypeDeclaration enclosing) throws CoreException {
		final ITypeBinding superClass = currentInnerDecl.resolveBinding()
				.getSuperclass();
		if (superClass != null) {
			return context.hasEnclosingAccess(superClass.getQualifiedName(),
					enclosing.resolveBinding().getQualifiedName());
		}
		return false;
	}

	public static boolean mustMoveToTop(TypeDeclaration node, IType inner,
			TypeDeclaration innerTypeDecl, ITranslationContext context) {
		if (!node.isInterface()) {
			if (innerTypeDecl.isInterface()
					|| TranslationUtils.isAnonymousRenamed(inner, context)
					|| !TranslationUtils.isImplicteInnerRename(inner, context)) {
				return false;
			}
		}
		return true;
	}

	//
	//
	//

	protected Change createChange(IProgressMonitor pm,
			TypeDeclaration enclosing, IType type, boolean refEnclosing,
			boolean needOuterField, boolean enclosingIsSuperOfInner)
			throws JavaModelException, CoreException {
		final CodeGenerationSettings settings = JavaPreferencesSettings
				.getCodeGenerationSettings(fCu.getJavaProject());
		settings.useKeywordThis = false;

		final ConvertNestedToInnerRefactoring refactoring = new ConvertNestedToInnerRefactoring(
				type, settings, enclosingIsSuperOfInner, refEnclosing,
				needOuterField, context);

		// TODO
		Map arg = new HashMap();
		arg.put("mandatory", "true");
		arg.put("possible", "true");
		final JavaRefactoringArguments arguments = new JavaRefactoringArguments(
				this.fCu.getJavaProject().getElementName(), arg);
		// TODO arguments.setAttribute("mandatory", "true");
		// TODO arguments.setAttribute("possible", "true");
		refactoring.initialize(arguments);

		refactoring.setMarkInstanceFieldAsFinal(false);
		if (refEnclosing) {
			refactoring
					.setEnclosingInstanceName(MoveInterfaceInnerOrNesterClassToTopVisitor.REF_TO_OUTER_NAME
							+ "_" + enclosing.resolveBinding().getName());
		}

		final RefactoringStatus status = refactoring.checkAllConditions(pm);

		if (status.hasFatalError()) {
			context.getLogger().logInfo(status.toString());
			return null;
		}

		return refactoring.createChange(pm);
	}

	//
	//
	//

	private class HasInterfaceVisitor extends ASTVisitor {
		private TypeDeclaration movableType = null;

		public void reset() {
			movableType = null;
		}

		@Override
		public boolean visit(TypeDeclaration node) {
			final TypeDeclaration[] types = node.getTypes();
			if ((types != null) && (types.length > 0)) {
				for (final TypeDeclaration currentInnerDecl : types) {
					final IType currentInnerType = (IType) currentInnerDecl
							.resolveBinding().getJavaElement();
					if ((movableType == null)
							&& (currentInnerType != null)
							&& (context.hasEnclosingAccess(currentInnerDecl
									.resolveBinding().getQualifiedName(), node
									.resolveBinding().getQualifiedName()) || parentHasEnclosingAccess(
									currentInnerDecl, node))
							&& !history.contains(currentInnerDecl
									.resolveBinding().getQualifiedName())) {
						movableType = currentInnerDecl;
						history.add(movableType.resolveBinding()
								.getQualifiedName());
						return true;
					}
				}
			} /*
				 * else if (ASTNodes.getParent(node, ASTNode.METHOD_DECLARATION) !=
				 * null) { IType currentInnerType = (IType)
				 * node.resolveBinding().getJavaElement(); String
				 * currentInnerName = node.resolveBinding().getName();
				 * ITypeBinding enclosingTypeB =
				 * ASTNodes.getEnclosingType(node); String enclosingName =
				 * enclosingTypeB.getQualifiedName(); IType enclosingType =
				 * (IType) enclosingTypeB.getJavaElement(); if (this.movableType ==
				 * null &&
				 * !MoveInterfaceInnerOrNesterClassToTopVisitor.this.history.contains(enclosingName +
				 * "." + currentInnerName)) { this.movableType = node;
				 * MoveInterfaceInnerOrNesterClassToTopVisitor.this.history.add(enclosingName +
				 * "." + currentInnerName); return true; } }
				 */

			return true;
		}

		private boolean parentHasEnclosingAccess(
				TypeDeclaration currentInnerDecl, TypeDeclaration enclosing) {
			final ITypeBinding innerBinding = currentInnerDecl.resolveBinding();
			if (innerBinding.getSuperclass() != null) {
				if (context.hasEnclosingAccess(innerBinding.getSuperclass()
						.getQualifiedName(), enclosing.resolveBinding()
						.getQualifiedName())) {
					return true;
				}
			}
			if (innerBinding.getInterfaces() != null) {
				for (int i = 0; i < innerBinding.getInterfaces().length; i++) {
					if (context.hasEnclosingAccess(
							innerBinding.getInterfaces()[i].getQualifiedName(),
							enclosing.resolveBinding().getQualifiedName())) {
						return true;
					}
				}
			}
			return false;
		}

		public TypeDeclaration getResult() {
			return movableType;
		}
	}

	public static boolean needOuterThis(TypeDeclaration node,
			TypeDeclaration nested, boolean checkOuter,
			ITranslationContext context) {

		if (checkOuter) {
			final Type type = nested.getSuperclassType();
			if (type != null) {
				final IVariableBinding outer = Bindings
						.findFieldInHierarchy(
								type.resolveBinding(),
								MoveInterfaceInnerOrNesterClassToTopVisitor.REF_TO_OUTER_NAME);
				if (outer != null) {
					return false;
				}
			}
		}

		return context.hasEnclosingAccess(nested.resolveBinding()
				.getQualifiedName(), node.resolveBinding().getQualifiedName());
	}
}
