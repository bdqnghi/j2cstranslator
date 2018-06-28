package com.ilog.translator.java2cs.translation.noderewriter;

import java.util.ArrayList;
import java.util.Collection;
import java.util.Collections;
import java.util.Comparator;
import java.util.List;

import org.eclipse.jdt.core.Flags;
import org.eclipse.jdt.core.dom.ASTNode;
import org.eclipse.jdt.core.dom.BodyDeclaration;
import org.eclipse.jdt.core.dom.EnumDeclaration;
import org.eclipse.jdt.core.dom.FieldDeclaration;
import org.eclipse.jdt.core.dom.ITypeBinding;
import org.eclipse.jdt.core.dom.MethodDeclaration;
import org.eclipse.jdt.core.dom.Modifier;
import org.eclipse.jdt.core.dom.Name;
import org.eclipse.jdt.core.dom.ParenthesizedExpression;
import org.eclipse.jdt.core.dom.SimpleType;
import org.eclipse.jdt.core.dom.SingleVariableDeclaration;
import org.eclipse.jdt.core.dom.Type;
import org.eclipse.jdt.core.dom.TypeDeclaration;
import org.eclipse.jdt.core.dom.VariableDeclarationExpression;
import org.eclipse.jdt.core.dom.VariableDeclarationFragment;
import org.eclipse.jdt.core.dom.VariableDeclarationStatement;
import org.eclipse.jdt.core.dom.rewrite.ASTRewrite;
import org.eclipse.jdt.core.dom.rewrite.ListRewrite;
import org.eclipse.jdt.internal.corext.dom.ASTNodes;
import org.eclipse.text.edits.TextEditGroup;

import com.ilog.translator.java2cs.configuration.ChangeModifierDescriptor;
import com.ilog.translator.java2cs.configuration.DotNetModifier;
import com.ilog.translator.java2cs.translation.ITranslationContext;
import com.ilog.translator.java2cs.translation.TranslatorASTRewrite;
import com.ilog.translator.java2cs.translation.util.TranslationUtils;
import com.ilog.translator.java2cs.util.TranslationModelUtil;

public class ModifiersRewriter extends AbstractNodeRewriter {

	//
	// fast way to do replace
	//

	public static void rewriteModifiers(ChangeModifierDescriptor change,
			ITranslationContext context, ASTNode node,
			TranslatorASTRewrite rew, TextEditGroup description) {
		final ModifiersRewriter mod = new ModifiersRewriter(change);
		mod.process(context, node, rew, null, description);
	}

	public static void rewriteModifiers(ChangeModifierDescriptor change,
			ITranslationContext context, ASTNode node,
			TranslatorASTRewrite rew, boolean javaAndDotNEt, boolean strict,
			TextEditGroup description) {
		final ModifiersRewriter mod = new ModifiersRewriter(change);
		if (javaAndDotNEt) {
			mod.setOnlyDotNet(true);
			mod.setOnlyJava(true);
			mod.setStrict(true);
		}
		mod.process(context, node, rew, null, description);
	}

	//
	//
	//

	private void setStrict(boolean b) {
		strict = b;
	}

	public final static int NONE = -1;

	public boolean strict = false;

	public boolean onlydotnet = false;

	public boolean onlyjava = true; // default behavior

	private ChangeModifierDescriptor changeModifiers = new ChangeModifierDescriptor();

	//
	//
	//

	public ModifiersRewriter(ChangeModifierDescriptor change) {
		changeModifiers = change;
	}

	//
	//
	//

	public void setOnlyDotNet(boolean onlydotnet) {
		this.onlydotnet = onlydotnet;
	}

	public void setOnlyJava(boolean onlyjava) {
		this.onlyjava = onlyjava;
	}

	//
	//
	//

	@SuppressWarnings("unchecked")
	@Override
	public void process(ITranslationContext context, ASTNode node,
			TranslatorASTRewrite rew, TranslatorASTRewrite subRewriter,
			TextEditGroup description) {
		ListRewrite list = getListRewiteModifiers(node, rew);
		if ((list != null) && (changeModifiers != null)) {
			final int flags = getModifiers(node);
			final List modifiers = list.getOriginalList();
			List<DotNetModifier> modifiersToAdd = changeModifiers
					.getModifiersToAdd();
			final List<DotNetModifier> modifiersToRemove = changeModifiers
					.getModifiersToRemove();

			// 
			checkIntegrity(context, node, flags, modifiersToAdd,
					modifiersToRemove);

			// REMOVE
			for (int i = 0; i < modifiers.size(); i++) {
				final ASTNode mod = (ASTNode) modifiers.get(i);
				if (mod.getNodeType() == ASTNode.MODIFIER) {
					final Modifier m = (Modifier) mod;
					if (changeModifiers.toRemove(m)) {
						list.remove(mod, null);
					}
				}
			}

			modifiersToAdd = changeModifiers.getModifiersToAdd();
			modifiersToAdd = reorder(modifiersToAdd, list.getRewrittenList());

			// ADD
			for (final DotNetModifier newModifier : modifiersToAdd) {
				if (!exists(newModifier, modifiers)
						&& !ignore(newModifier, node)
						&& !modifiersToRemove.contains(newModifier)) {
					if (newModifier.isDotNetOnly() && onlydotnet) {
						list.insertLast(rew.createStringPlaceholder(
								"/* insert_here:" + newModifier.getKeyword()
										+ " */", ASTNode.MODIFIER), null);
						// "Pure" dotnet modifiers handled later because they
						// can create inconsistency too early !
					} else if (!newModifier.isDotNetOnly() && onlyjava) {
						final Modifier newM = (Modifier) rew
								.createStringPlaceholder(newModifier
										.getKeyword(), ASTNode.MODIFIER);
						if (list != null) {
							list.insertLast(newM, null);
						}
					}
				}
			}

		}
		list = null;
	}

	private List<DotNetModifier> reorder(List<DotNetModifier> modifiersToAdd,
			List<ASTNode> rewrittenList) {
		final List<DotNetModifier> result = new ArrayList<DotNetModifier>(
				modifiersToAdd);

		Collections.sort(result, new Comparator<DotNetModifier>() {

			public int compare(DotNetModifier o1, DotNetModifier o2) {
				if (o1.getHeight() == o2.getHeight())
					return 0;
				if (o1.getHeight() < o2.getHeight())
					return -1;
				return 1;
			}

		});
		return result;
	}

	private void checkIntegrity(ITranslationContext context, ASTNode node,
			int flags, Collection<DotNetModifier> modifiersToAdd,
			Collection<DotNetModifier> modifiersToRemove) {
		if (node.getNodeType() == ASTNode.METHOD_DECLARATION) {
			// if we want add override, member cannot have private access !
			// Replace Private by protected !
			if (Flags.isPrivate(flags)
					&& (modifiersToAdd != null)
					&& (modifiersToAdd.contains(DotNetModifier.OVERRIDE) || modifiersToAdd
							.contains(DotNetModifier.VIRTUAL))) {
				if (!strict)
					changeModifiers.replace(DotNetModifier.PROTECTED,
							DotNetModifier.PRIVATE);
			}

			if (Flags.isFinal(flags)
					&& (Flags.isPublic(flags) || Flags.isProtected(flags))) {
				if (isOverride(context, node, modifiersToAdd)) {
					if (onlydotnet)
						changeModifiers.replace(DotNetModifier.FINAL,
								DotNetModifier.SEALED);
				} else {
					changeModifiers.remove(DotNetModifier.FINAL);
				}
			}

			if(Flags.isAbstract( flags ) || Flags.isStatic( flags )){
				  this.changeModifiers.remove(DotNetModifier.VIRTUAL);
				}
			
			if ((modifiersToAdd != null) && (modifiersToRemove != null)
					&& modifiersToAdd.contains(DotNetModifier.INTERNAL)
					&& modifiersToRemove.contains(DotNetModifier.INTERNAL)) {
				// 
				changeModifiers.removeFromAddList(DotNetModifier.INTERNAL);
			}
			
			//Bug fix - replace "protected" with "protected internal"
			if (Flags.isProtected(flags) && !modifiersToAdd.contains(DotNetModifier.INTERNAL)){
				this.changeModifiers.add(DotNetModifier.INTERNAL);
			}
		} else if (node.getNodeType() == ASTNode.FIELD_DECLARATION) {
			if (ModifiersRewriter.isStaticFinal(flags)
					&& isPrimitiveType(((FieldDeclaration) node).getType())
					&& ModifiersRewriter.isConstable((FieldDeclaration) node)) {
				changeModifiers.remove(DotNetModifier.STATIC);
				changeModifiers.remove(DotNetModifier.FINAL);
				changeModifiers.add(DotNetModifier.CONST);
			} else if (Flags.isFinal(flags)) {
				if (modifiersToAdd.contains(DotNetModifier.CONST)) {
					changeModifiers.remove(DotNetModifier.STATIC);
					changeModifiers.replace(DotNetModifier.FINAL,
							DotNetModifier.CONST);
				}
				if (!modifiersToAdd.contains(DotNetModifier.CONST))
					changeModifiers.replace(DotNetModifier.FINAL,
							DotNetModifier.READONLY);
			}
		}

		// "package" -> "internal"
		final ITypeBinding enclosingType = ASTNodes.getEnclosingType(node);
		if (onlydotnet && (enclosingType != null)
				&& node.getNodeType() != ASTNode.VARIABLE_DECLARATION_STATEMENT
				&& (enclosingType.isClass() || enclosingType.isEnum())) {

			if (Flags.isPackageDefault(flags)
					&& !modifiersToAdd.contains(DotNetModifier.PUBLIC)
					&& !modifiersToAdd.contains(DotNetModifier.PROTECTED)
					&& !modifiersToAdd.contains(DotNetModifier.INTERNAL)
					&& !modifiersToRemove.contains(DotNetModifier.INTERNAL)) {
				if (!strict)
					changeModifiers.add(DotNetModifier.INTERNAL);
			}
		}
	}

	public static boolean isConstable(FieldDeclaration field) {
		final VariableDeclarationFragment frag = (VariableDeclarationFragment) field
				.fragments().get(0);
		return isConstable(frag);
	}

	public static boolean isConstable(VariableDeclarationStatement field) {
		final VariableDeclarationFragment frag = (VariableDeclarationFragment) field
				.fragments().get(0);
		return isConstable(frag);
	}

	public static boolean isConstable(VariableDeclarationFragment frag) {
		if (frag.getInitializer() != null) {
			switch (frag.getInitializer().getNodeType()) {
			case ASTNode.METHOD_INVOCATION:
				return false;
			case ASTNode.PARENTHESIZED_EXPRESSION:
				final ParenthesizedExpression pe = (ParenthesizedExpression) frag
						.getInitializer();
				if (pe.getExpression().getNodeType() == ASTNode.CONDITIONAL_EXPRESSION) {
					return false;
				} else
					return frag.getInitializer()
							.resolveConstantExpressionValue() != null;
			case ASTNode.CONDITIONAL_EXPRESSION:
				return false;
			default:
				return frag.getInitializer().resolveConstantExpressionValue() != null;
			}
		}
		return false;
	}

	private boolean isPrimitiveType(Type type) {
		return type.isPrimitiveType()
				|| ModifiersRewriter.isDotNetPrimitiveType(type);
	}

	private static boolean isDotNetPrimitiveType(Type type) {
		if (type.isSimpleType()) {
			final SimpleType sType = (SimpleType) type;
			final Name tName = sType.getName();
			if (tName != null) {
				final String fqnName = tName.getFullyQualifiedName();
				if (fqnName != null) {
					return fqnName.equals("sbyte") || fqnName.equals("ushort")
							|| fqnName.equals("ulong")
							|| fqnName.equals("uint");
				}
			}
		}
		return false;
	}

	private static boolean isStaticFinal(int flags) {
		return Modifier.isStatic(flags) && Modifier.isFinal(flags);
	}

	private boolean isOverride(ITranslationContext context, ASTNode node,
			Collection<DotNetModifier> modifiersToAdd) {
		return modifiersToAdd.contains(DotNetModifier.OVERRIDE)
				|| TranslationUtils.containsTag((MethodDeclaration) node,
						context.getModel().getTag(
								TranslationModelUtil.OVERRIDE_TAG));
	}

	private int getModifiers(ASTNode node) {
		if (node instanceof BodyDeclaration) {
			return ((BodyDeclaration) node).getModifiers();
		} else if (node instanceof VariableDeclarationStatement) {
			return ((VariableDeclarationStatement) node).getModifiers();
		}
		return -1;
	}

	@SuppressWarnings("unchecked")
	public List<DotNetModifier> getRewritedModifiersList(BodyDeclaration node,
			ASTRewrite rew, ChangeModifierDescriptor desc) {

		boolean isInInterface = false;

		final ASTNode parent = ASTNodes.getParent(node,
				ASTNode.TYPE_DECLARATION);
		if (parent != null) {
			final TypeDeclaration parentType = (TypeDeclaration) parent;
			if (parentType.isInterface()) {
				isInInterface = true;
			}
		}

		final ListRewrite list = getListRewiteModifiers(node, rew);
		final List<DotNetModifier> newList = new ArrayList<DotNetModifier>();

		if ((list != null) && (desc != null)) {
			final List modifiers = list.getOriginalList();

			final int flags = node.getModifiers();

			// REMOVE
			for (int i = 0; i < modifiers.size(); i++) {
				final ASTNode mod = (ASTNode) modifiers.get(i);
				if (mod.getNodeType() == ASTNode.MODIFIER) {
					final Modifier m = (Modifier) mod;
					if (!desc.toRemove(m)) {
						newList.add(DotNetModifier.fromKeyword(m.getKeyword()
								.toString()));
					}
				}
			}

			// REPLACE
			if (Flags.isPackageDefault(flags) && !isInInterface) {
				/*final Modifier newM = (Modifier) rew.createStringPlaceholder(
						DotNetModifier.INTERNAL.getKeyword(), ASTNode.MODIFIER);
				list.insertLast(newM, null);*/
				newList.add(DotNetModifier.INTERNAL);
			}

			// ADD
			for (int i = 0; i < desc.getModifiersToAdd().size(); i++) {
				final DotNetModifier newModifier = desc.getModifiersToAdd()
						.get(i);

				if (!exists(newModifier, modifiers)
						&& !ignore(newModifier, node)) {
					if (list != null) {
						newList.add(newModifier);
					}
				}
			}
		}
		return newList;
	}

	//
	//
	//

	private boolean ignore(DotNetModifier m, ASTNode node) {

		final int flags = getModifiers(node);
		// virtual : !package !static !abstract
		// override : !package !static !abstract
		if (m == DotNetModifier.VIRTUAL) {
			if (isAbstract(flags) || isStatic(flags)) {
				return true;
			}
		}
		if (m == DotNetModifier.OVERRIDE) {
			if (isStatic(flags)) {
				return true;
			}
		}
		return false;
	}

	private boolean isAbstract(int flags) {
		final List<DotNetModifier> addedModifiers = changeModifiers
				.getModifiersToAdd();
		final List<DotNetModifier> removedModifiers = changeModifiers
				.getModifiersToRemove();
		return (Modifier.isAbstract(flags) && !removedModifiers
				.contains(DotNetModifier.ABSTRACT))
				|| addedModifiers.contains(DotNetModifier.ABSTRACT);
	}

	private boolean isStatic(int flags) {
		final List<DotNetModifier> addedModifiers = changeModifiers
				.getModifiersToAdd();
		final List<DotNetModifier> removedModifiers = changeModifiers
				.getModifiersToRemove();
		return (Modifier.isStatic(flags) && !removedModifiers
				.contains(DotNetModifier.STATIC))
				|| addedModifiers.contains(DotNetModifier.STATIC);
	}

	@SuppressWarnings("unchecked")
	private boolean exists(DotNetModifier m, List modifiers) {
		for (int i = 0; i < modifiers.size(); i++) {
			final ASTNode mod = (ASTNode) modifiers.get(i);
			if (mod.getNodeType() == ASTNode.MODIFIER) {
				final Modifier current = (Modifier) mod;

				if (current.getKeyword().toFlagValue() == m.getKind()) {
					return true;
				}
			}
		}
		return false;
	}

	private ListRewrite getListRewiteModifiers(ASTNode node, ASTRewrite rew) {
		ListRewrite list = null;
		switch (node.getNodeType()) {
		case ASTNode.METHOD_DECLARATION:
			list = rew.getListRewrite(node,
					MethodDeclaration.MODIFIERS2_PROPERTY);
			break;
		case ASTNode.FIELD_DECLARATION:
			list = rew.getListRewrite(node,
					FieldDeclaration.MODIFIERS2_PROPERTY);
			break;
		case ASTNode.TYPE_DECLARATION:
			list = rew
					.getListRewrite(node, TypeDeclaration.MODIFIERS2_PROPERTY);
			break;
		case ASTNode.VARIABLE_DECLARATION_EXPRESSION:
			list = rew.getListRewrite(node,
					VariableDeclarationExpression.MODIFIERS2_PROPERTY);
			break;
		case ASTNode.VARIABLE_DECLARATION_STATEMENT:
			list = rew.getListRewrite(node,
					VariableDeclarationStatement.MODIFIERS2_PROPERTY);
			break;
		case ASTNode.VARIABLE_DECLARATION_FRAGMENT:
			list = rew.getListRewrite(node,
					VariableDeclarationStatement.MODIFIERS2_PROPERTY);
			break;
		case ASTNode.SINGLE_VARIABLE_DECLARATION:
			list = rew.getListRewrite(node,
					SingleVariableDeclaration.MODIFIERS2_PROPERTY);
			break;
		case ASTNode.ENUM_DECLARATION:
			list = rew
					.getListRewrite(node, EnumDeclaration.MODIFIERS2_PROPERTY);
			break;
		}
		return list;
	}
}
