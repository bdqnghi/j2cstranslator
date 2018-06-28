package com.ilog.translator.java2cs.translation.astrewriter;

import org.eclipse.jdt.core.Flags;
import org.eclipse.jdt.core.IType;
import org.eclipse.jdt.core.dom.ASTNode;
import org.eclipse.jdt.core.dom.EnumDeclaration;
import org.eclipse.jdt.core.dom.FieldAccess;
import org.eclipse.jdt.core.dom.IBinding;
import org.eclipse.jdt.core.dom.IMethodBinding;
import org.eclipse.jdt.core.dom.ITypeBinding;
import org.eclipse.jdt.core.dom.IVariableBinding;
import org.eclipse.jdt.core.dom.MethodInvocation;
import org.eclipse.jdt.core.dom.Modifier;
import org.eclipse.jdt.core.dom.Name;
import org.eclipse.jdt.core.dom.QualifiedName;
import org.eclipse.jdt.core.dom.SimpleName;
import org.eclipse.jdt.core.dom.TypeDeclaration;
import org.eclipse.jdt.internal.corext.dom.ASTNodes;
import org.eclipse.jdt.internal.corext.dom.Bindings;
import org.eclipse.text.edits.TextEditGroup;

import com.ilog.translator.java2cs.translation.ITranslationContext;
import com.ilog.translator.java2cs.translation.util.TranslationUtils;

/**
 * Check if an access to a field is non ambigues (csharp way) means the there is
 * no class with the same short name than the class that contains the field
 * 
 * @author afau
 * 
 */
public class FullyQualifiedStaticMethodInvocationVisitor extends
		ASTRewriterVisitor {

	//
	//
	//

	public FullyQualifiedStaticMethodInvocationVisitor(
			ITranslationContext context) {
		super(context);
		transformerName = "Fully Qualified Static methods access and static fields access";
		description = new TextEditGroup(transformerName);
	}

	//
	//
	//

	@Override
	public boolean visit(EnumDeclaration node) {
		return false;
	}

	@Override
	public boolean visit(QualifiedName node) {
		final IBinding binding = node.resolveBinding();
		if ((binding != null) && (binding.getKind() == IBinding.VARIABLE)) {
			final IVariableBinding vb = (IVariableBinding) binding;
			replaceStaticAccess(node, vb, node.getName()
					.getFullyQualifiedName());
		}

		return false;
	}

	@Override
	public void endVisit(SimpleName node) {
		final IBinding binding = node.resolveBinding();
		if ((binding != null) && (binding.getKind() == IBinding.VARIABLE)) {
			final IVariableBinding vb = (IVariableBinding) binding;
			replaceStaticAccess(node, vb, node.getFullyQualifiedName());
		}
	}

	@Override
	public void endVisit(FieldAccess node) {
		final IVariableBinding vb = node.resolveFieldBinding();
		replaceStaticAccess(node, vb, node.getName().getFullyQualifiedName());
	}

	@Override
	public void endVisit(MethodInvocation node) {
		final IMethodBinding mbinding = node.resolveMethodBinding();
		if ((mbinding != null) && Flags.isStatic(mbinding.getModifiers())) {
			final ITypeBinding tbinding = mbinding.getDeclaringClass();

			final TypeDeclaration parent = (TypeDeclaration) ASTNodes
					.getParent(node, ASTNode.TYPE_DECLARATION);
			final ITypeBinding parentBinding = parent.resolveBinding();

			final ITypeBinding parentRawBinding = parentBinding.getErasure();
			final ITypeBinding methodRawBinding = tbinding.getErasure();

			boolean equals = false;

			if (parentRawBinding != null && methodRawBinding != null) {
				equals = parentRawBinding.isEqualTo(methodRawBinding);
			} else {
				equals = tbinding.isEqualTo(parentBinding);
			}

			if (parent != null && !equals) {
				// TODO: fqn is not really necessary ...
				final String cde = TranslationUtils
						.getFullyQualifiedName((IType) tbinding
								.getJavaElement());

				// AST ast = node.getAST();

				final ASTNode typeName = currentRewriter
						.createStringPlaceholder(cde, ASTNode.SIMPLE_NAME);

				// this.currentRewriter.replace(node.getExpression(), typeName,
				// description);
				currentRewriter.set(node, MethodInvocation.EXPRESSION_PROPERTY,
						typeName, description);
			}
		}
	}

	//
	//
	//

	private void replaceStaticAccess(ASTNode node, IVariableBinding vb,
			String fieldName) {
		if (vb.isField() && Modifier.isStatic(vb.getModifiers())) {
			if (ASTNodes.getParent(node, ASTNode.SWITCH_CASE) != null
					&& vb.isEnumConstant()) {
				return;
			}
			final ITypeBinding declaringTypeB = vb.getDeclaringClass();
			final ITypeBinding enclosingTypeB = ASTNodes.getEnclosingType(node);
			if ((enclosingTypeB != null) && (declaringTypeB != null)) {
				final ITypeBinding eraseDeclaringType = declaringTypeB
						.getErasure();
				if (!enclosingTypeB.isEqualTo(declaringTypeB)
						&& !enclosingTypeB.isEqualTo(eraseDeclaringType) &&
						isVisibleFrom(enclosingTypeB, declaringTypeB)) {
					// declaringTypeB
					String fqTypeName = Bindings
							.getFullyQualifiedName(declaringTypeB);
					if (declaringTypeB.isParameterizedType()) {
						final ITypeBinding[] typeArgs = declaringTypeB
								.getTypeArguments();
						if (typeArgs != null && typeArgs.length > 0) {
							fqTypeName += "/* insert_here:<";
							for (int i = 0; i < typeArgs.length; i++) {
								final ITypeBinding typeB = typeArgs[i];
								fqTypeName += typeB.getName();
								if (i < typeArgs.length - 1)
									fqTypeName += ",";
							}
							fqTypeName += "> */";
						}
					}
					final Name replacement = (Name) currentRewriter
							.createStringPlaceholder(fqTypeName + "."
									+ fieldName, ASTNode.QUALIFIED_NAME);
					currentRewriter.replace(node, replacement, description);
				}
			}
		}
	}

	private boolean isVisibleFrom(ITypeBinding enclosingTypeB,
			ITypeBinding declaringTypeB) {
		// at least declaringTypeB type must be "public"
		return Modifier.isPublic(declaringTypeB.getModifiers()) || inSamePackage(enclosingTypeB, declaringTypeB);
	}

	private boolean inSamePackage(ITypeBinding enclosingTypeB,
			ITypeBinding declaringTypeB) {	
		return enclosingTypeB.getPackage() == declaringTypeB.getPackage();
	}
}
