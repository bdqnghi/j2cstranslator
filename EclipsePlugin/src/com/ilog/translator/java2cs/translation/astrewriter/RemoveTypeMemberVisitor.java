package com.ilog.translator.java2cs.translation.astrewriter;

import java.util.List;

import org.eclipse.core.runtime.CoreException;
import org.eclipse.core.runtime.IProgressMonitor;
import org.eclipse.jdt.core.Flags;
import org.eclipse.jdt.core.ICompilationUnit;
import org.eclipse.jdt.core.IType;
import org.eclipse.jdt.core.JavaModelException;
import org.eclipse.jdt.core.dom.ASTNode;
import org.eclipse.jdt.core.dom.AnonymousClassDeclaration;
import org.eclipse.jdt.core.dom.FieldDeclaration;
import org.eclipse.jdt.core.dom.ITypeBinding;
import org.eclipse.jdt.core.dom.IVariableBinding;
import org.eclipse.jdt.core.dom.MethodDeclaration;
import org.eclipse.jdt.core.dom.MethodInvocation;
import org.eclipse.jdt.core.dom.Modifier;
import org.eclipse.jdt.core.dom.ParameterizedType;
import org.eclipse.jdt.core.dom.SimpleName;
import org.eclipse.jdt.core.dom.SingleVariableDeclaration;
import org.eclipse.jdt.core.dom.Type;
import org.eclipse.jdt.core.dom.TypeDeclaration;
import org.eclipse.jdt.core.dom.VariableDeclarationFragment;
import org.eclipse.jdt.internal.corext.dom.ASTNodes;
import org.eclipse.ltk.core.refactoring.Change;
import org.eclipse.text.edits.TextEditGroup;

import com.ilog.translator.java2cs.configuration.info.TranslationModelException;
import com.ilog.translator.java2cs.configuration.target.TargetClass;
import com.ilog.translator.java2cs.translation.ITranslationContext;
import com.ilog.translator.java2cs.translation.noderewriter.INodeRewriter;
import com.ilog.translator.java2cs.translation.noderewriter.TypeRewriter;
import com.ilog.translator.java2cs.translation.util.TranslationUtils;
import com.ilog.translator.java2cs.util.TranslationModelUtil;

public class RemoveTypeMemberVisitor extends ASTRewriterVisitor {

	//
	//
	//

	public RemoveTypeMemberVisitor(ITranslationContext context) {
		super(context);
		transformerName = "Remove Type Member";
		description = new TextEditGroup(transformerName);
	}

	//
	//
	//

	@Override
	public boolean isAbridged() {
		return false;
	}

	//
	//
	//

	@Override
	public boolean applyChange(IProgressMonitor pm) throws CoreException {
		final Change change = createChange(pm, null);
		if (change != null) {
			context.addChange(fCu, change);
		}
		return true;
	}

	//
	//
	//

	private static final String TYPE_PARAM_REF = "%";

	private TargetClass findTargetClass(ITranslationContext context, Type t) {
		TargetClass tc = null;
		try {
			tc = context.getModel().findGenericClassMapping(
					t.resolveBinding().getJavaElement().getHandleIdentifier());
		} catch (final TranslationModelException e) {
			// TODO
			context.getLogger().logException(fCu.getElementName() + " ", e);
		}
		if (tc == null) {
			tc = context.getModel().findClassMapping(
					t.resolveBinding().getJavaElement().getHandleIdentifier(),
					true, TranslationUtils.isGeneric(t.resolveBinding()));
		}
		return tc;
	}

	private String getRawTypeName(Type pType) {
		final ITypeBinding typeB = pType.resolveBinding();
		String name = typeB.getErasure().getName();
		ITypeBinding currentType = typeB.getDeclaringClass();
		while (currentType != null) {
			name = currentType.getErasure().getName() + "." + name;
			currentType = currentType.getDeclaringClass();
		}
		return name;
	}

	@SuppressWarnings("unchecked")
	private String printTypeArguments(ParameterizedType type,
			ITranslationContext context) {
		String res = "";
		final List typeArgs = type.typeArguments();
		if (typeArgs != null && typeArgs.size() > 0) {
			String targs = "<";
			for (int i = 0; i < typeArgs.size(); i++) {
				final Type t = ((Type) typeArgs.get(i));
				final TargetClass tc = findTargetClass(context, t);
				if (t.isParameterizedType()) {
					final ParameterizedType pType = (ParameterizedType) t;
					if (tc == null || tc.getName() == null)
						targs += getRawTypeName(pType)
								+ printTypeArguments(pType, context);
					else
						targs += tc.getName()
								+ printTypeArguments(pType, context);
				} else {
					if (tc == null || tc.getName() == null)
						targs += getRawTypeName(t);
					else
						targs += tc.getName();
				}
				if (i < typeArgs.size() - 1)
					targs += ",";
			}
			targs += ">";
			res += targs;
		}
		return res;
	}

	@SuppressWarnings("unchecked")
	private String replaceTypeArgs(String pattern, List<Type> typeArguments,
			ITranslationContext context) {
		for (int i = 0; i < typeArguments.size(); i++) {
			final Type t = typeArguments.get(i);
			final TargetClass tc = findTargetClass(context, t);
			String replacement = null;

			if (t.isParameterizedType()) {
				final ParameterizedType pType = (ParameterizedType) t;
				if (tc == null || tc.getName() == null)
					replacement = getRawTypeName(pType)
							+ printTypeArguments(pType, context);
				else if (tc.getName().contains(TYPE_PARAM_REF)) {
					replacement = replaceTypeArgs(tc.getName(), pType
							.typeArguments(), context);
				} else {
					replacement = tc.getName()
							+ printTypeArguments(pType, context);
				}
			} else {
				if (tc == null || tc.getName() == null)
					replacement = getRawTypeName(t);
				else
					replacement = tc.getName();
			}
			pattern = pattern.replace(TYPE_PARAM_REF + (i + 1), replacement);
		}
		return pattern;
	}

	@SuppressWarnings("unchecked")
	@Override
	public void endVisit(MethodInvocation node) {
		final List typeArgs = node.typeArguments();
		if (typeArgs != null && typeArgs.size() > 0) {
			SimpleName newName = currentRewriter.getAST().newSimpleName(
					node.getName().getIdentifier());
			String targs = "<";
			for (int i = 0; i < typeArgs.size(); i++) {
				final Type t = ((Type) typeArgs.get(i));
				currentRewriter.remove(t, null);
				final TargetClass tc = findTargetClass(context, t);
				if (t.isParameterizedType()) {
					final ParameterizedType pType = (ParameterizedType) t;
					if (tc == null || tc.getName() == null)
						targs += getRawTypeName(pType)
								+ printTypeArguments(pType, context);
					else if (tc.getName().contains(TYPE_PARAM_REF)) {
						targs += replaceTypeArgs(tc.getName(), pType
								.typeArguments(), context);
					} else {
						targs += tc.getName()
								+ printTypeArguments(pType, context);
					}
				} else {
					if (tc == null || tc.getName() == null)
						targs += ASTNodes.getTypeName(t); // getRawTypeName(t);
					else
						targs += tc.getName();
				}
				if (i < typeArgs.size() - 1)
					targs += ",";
			}
			targs += ">";
			newName = (SimpleName) currentRewriter.createStringPlaceholder(node
					.getName().getIdentifier()
					+ "/* insert_here:" + targs + " */", ASTNode.SIMPLE_NAME);
			currentRewriter.replace(node.getName(), newName, null);
		}
	}

	@SuppressWarnings("unchecked")
	@Override
	public void endVisit(MethodDeclaration node) {
		if (Modifier.isStatic(node.getModifiers()) && node.isConstructor()) {
			return;
		}

		final String handler = context.getHandlerFromDoc(node, false);

		if (handler == null) {
			System.err.println(transformerName + " " + node
					+ " no handler found !");
		}

		final INodeRewriter rew = context.getMapper().mapMethodDeclaration(
				fCu.getElementName(), context.getSignatureFromDoc(node, false),
				handler, true, false, false);

		if (((rew != null) && rew.isRemove())
				|| TranslationUtils.containsTag(node, context.getModel()
						.getTag(TranslationModelUtil.REMOVE_TAG))
				|| isEnum(fCu)) {
			currentRewriter.remove(node, description);
		} else {
			final List paras = node.parameters();
			for (int i = 0; i < paras.size(); i++) {
				final SingleVariableDeclaration par = (SingleVariableDeclaration) paras
						.get(i);
				if (par.isVarargs()) {
					// var args
					final String typeName = ASTNodes.asString(par.getType()); // ASTNodes.getTypeName(par.getType());
					final String paramName = par.getName().getIdentifier();
					currentRewriter.replace(par, currentRewriter
							.createStringPlaceholder(
									"/* insert_here:params */ " + typeName
											+ "[] " + paramName, par
											.getNodeType()), description);
				}
			}
		}
	}

	@Override
	public void endVisit(TypeDeclaration node) {
		final INodeRewriter rew = context.getMapper().mapTypeDeclaration(null,
				context.getHandlerFromDoc(node, false));
		final boolean excluded = rew instanceof TypeRewriter ? ((TypeRewriter) rew)
				.isExluced()
				: false;
		if (rew != null && node.isMemberTypeDeclaration()
				&& (rew.isRemove() || excluded)) {
			currentRewriter.remove(node, description);
		}
	}

	@Override
	public void endVisit(AnonymousClassDeclaration node) {
		if (isEnum(fCu) || ASTNodes.getEnclosingType(node).isEnum()) {
			currentRewriter.remove(node, description);
		}
	}

	@Override
	public void endVisit(FieldDeclaration node) {
		final INodeRewriter rew = context.getMapper().mapFieldDeclaration(node,
				context.getHandlerFromDoc(node, false));

		if (((rew != null) && rew.isRemove()) || isEnum(fCu)) {
			currentRewriter.remove(node, description);
		}

		if (isSerialVersionUIDField(node)
				&& context.getConfiguration().getOptions()
						.isRemoveSerialVersionUIDFields()) {
			currentRewriter.remove(node, description);
		}

	}

	private boolean isSerialVersionUIDField(FieldDeclaration node) {
		final VariableDeclarationFragment varDecl = TranslationUtils
				.getFrament(node);
		final IVariableBinding varBind = varDecl.resolveBinding();
		if (varBind != null) {
			final int modifiers = varBind.getModifiers();
			if (Flags.isStatic(modifiers) && Flags.isFinal(modifiers)
					&& Flags.isPrivate(modifiers)) {
				if (TranslationUtils.isLongType(varBind.getType())) {
					final String fieldName = varBind.getName();
					if (fieldName.equals("serialVersionUID"))
						return true;
				}
			}
		}
		return false;
	}

	//
	//
	//

	private boolean isEnum(ICompilationUnit cu) {
		try {
			final IType type = cu.getTypes()[0];
			return type.isEnum();
		} catch (final JavaModelException e) {
			context.getLogger().logException(fCu.getElementName() + " ", e);
			return false;
		}
	}
}
