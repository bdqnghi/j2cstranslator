package com.ilog.translator.java2cs.translation.astrewriter.astchange;

import java.util.ArrayList;
import java.util.Collection;
import java.util.List;

import org.eclipse.jdt.core.IType;
import org.eclipse.jdt.core.dom.AST;
import org.eclipse.jdt.core.dom.ASTNode;
import org.eclipse.jdt.core.dom.ASTVisitor;
import org.eclipse.jdt.core.dom.AnonymousClassDeclaration;
import org.eclipse.jdt.core.dom.ConstructorInvocation;
import org.eclipse.jdt.core.dom.Expression;
import org.eclipse.jdt.core.dom.FieldDeclaration;
import org.eclipse.jdt.core.dom.IBinding;
import org.eclipse.jdt.core.dom.IMethodBinding;
import org.eclipse.jdt.core.dom.ITypeBinding;
import org.eclipse.jdt.core.dom.IVariableBinding;
import org.eclipse.jdt.core.dom.Modifier;
import org.eclipse.jdt.core.dom.SimpleName;
import org.eclipse.jdt.core.dom.SuperConstructorInvocation;
import org.eclipse.jdt.core.dom.TypeDeclaration;
import org.eclipse.jdt.core.dom.TypeParameter;
import org.eclipse.jdt.core.dom.VariableDeclarationFragment;
import org.eclipse.jdt.internal.corext.dom.ASTNodes;
import org.eclipse.jdt.internal.corext.refactoring.structure.ASTNodeSearchUtil;
import org.eclipse.jdt.internal.corext.refactoring.util.JavaElementUtil;
import org.eclipse.text.edits.TextEditGroup;

import com.ilog.translator.java2cs.translation.ITranslationContext;
import com.ilog.translator.java2cs.translation.astrewriter.ASTRewriterVisitor;

public class MoveAnonymousInConstructorToField extends ASTRewriterVisitor {

	private int cpt = 0;

	//
	//
	//

	public MoveAnonymousInConstructorToField(ITranslationContext context) {
		super(context);
		transformerName = "Move Anonymous In Constructor To Field";
		description = new TextEditGroup(transformerName);
	}

	//
	//
	//

	@SuppressWarnings("unchecked")
	@Override
	public boolean visit(AnonymousClassDeclaration node) {
		final SuperConstructorInvocation superConstructorInovk = (SuperConstructorInvocation) ASTNodes
				.getParent(node, ASTNode.SUPER_CONSTRUCTOR_INVOCATION);
		
		if (!hasForNestedAccess(node)) {
		if (superConstructorInovk != null) {
			final List args = superConstructorInovk.arguments();
			int pos = -1;
			for (int i = 0; i < args.size(); i++) {
				if (((ASTNode) args.get(i)) == node.getParent()) {
					pos = i;
				}
			}
			final ITypeBinding type = superConstructorInovk
					.resolveConstructorBinding().getParameterTypes()[pos];
			moveAnonymous(node.getParent(), superConstructorInovk, type);
		} else {
			final ConstructorInvocation constructorInovk = (ConstructorInvocation) ASTNodes
					.getParent(node, ASTNode.CONSTRUCTOR_INVOCATION);
			if (constructorInovk != null) {
				final List args = constructorInovk.arguments();
				int pos = -1;
				for (int i = 0; i < args.size(); i++) {
					if (((ASTNode) args.get(i)) == node.getParent()) {
						pos = i;
					}
				}

				final ITypeBinding type = constructorInovk
						.resolveConstructorBinding().getParameterTypes()[pos];
				moveAnonymous(node.getParent(), constructorInovk, type);
			}
		}
		}
		return false;
	}

	private boolean hasForNestedAccess(AnonymousClassDeclaration node) {
		IBinding enclosingClassBinding = node.resolveBinding().getDeclaringClass();
		SearchEnclosingAccess v = new SearchEnclosingAccess(enclosingClassBinding);
		node.accept(v);
		return v.hasEnclosingAccess();
	}

	private static class SearchEnclosingAccess extends ASTVisitor {

		private boolean hasEnclosingAccess = false;
		private IBinding enclosingClass = null;
		
		public SearchEnclosingAccess(IBinding enclosingClass) {
			this.enclosingClass = enclosingClass;
		}
		
		public void endVisit(SimpleName node) {
			IBinding binding = node.resolveBinding();
			if (binding != null && binding.getKind() == IBinding.VARIABLE) {
				IVariableBinding vBind = (IVariableBinding) binding;
				IMethodBinding imb = vBind.getDeclaringMethod();
				if (imb != null) {
						ITypeBinding itb = imb.getDeclaringClass();
						if (itb != null && itb.isEqualTo(enclosingClass)) {
							hasEnclosingAccess = true;
						}
				}
			}
		}

		public boolean hasEnclosingAccess() {
			return hasEnclosingAccess;
		}
	}
	
	//
	//
	//
	@SuppressWarnings("unchecked")
	private void moveAnonymous(ASTNode node, ASTNode parent, ITypeBinding itype) {
		final AST ast = node.getAST();

		final String identifier = "anonymous_C" + cpt++;
		final VariableDeclarationFragment fragment = ast
				.newVariableDeclarationFragment();
		fragment.setName(ast.newSimpleName(identifier));
		fragment.setInitializer((Expression) ASTNode.copySubtree(ast, node));
		final FieldDeclaration fd = ast.newFieldDeclaration(fragment);
		fd.setType(ast.newSimpleType(ast.newName(itype.getQualifiedName())));
		fd.modifiers().addAll(
				ast.newModifiers(Modifier.STATIC | Modifier.PRIVATE));
		//
		final TypeDeclaration typeDecl = (TypeDeclaration) ASTNodes.getParent(
				node, ASTNode.TYPE_DECLARATION);
		// typeDecl
		currentRewriter.getListRewrite(typeDecl,
				TypeDeclaration.BODY_DECLARATIONS_PROPERTY).insertFirst(fd,
				null);
		//
		final SimpleName replacement = ast.newSimpleName(identifier);
		currentRewriter.replace(node, replacement, null);
	}

}
