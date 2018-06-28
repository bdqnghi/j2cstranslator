package com.ilog.translator.java2cs.translation.astrewriter;

import java.util.ArrayList;
import java.util.List;

import org.eclipse.jdt.core.ICompilationUnit;
import org.eclipse.jdt.core.dom.AST;
import org.eclipse.jdt.core.dom.ASTNode;
import org.eclipse.jdt.core.dom.Annotation;
import org.eclipse.jdt.core.dom.AnnotationTypeDeclaration;
import org.eclipse.jdt.core.dom.AnnotationTypeMemberDeclaration;
import org.eclipse.jdt.core.dom.ArrayInitializer;
import org.eclipse.jdt.core.dom.EnumDeclaration;
import org.eclipse.jdt.core.dom.Expression;
import org.eclipse.jdt.core.dom.FieldDeclaration;
import org.eclipse.jdt.core.dom.IBinding;
import org.eclipse.jdt.core.dom.IExtendedModifier;
import org.eclipse.jdt.core.dom.IMethodBinding;
import org.eclipse.jdt.core.dom.ITypeBinding;
import org.eclipse.jdt.core.dom.IVariableBinding;
import org.eclipse.jdt.core.dom.MemberValuePair;
import org.eclipse.jdt.core.dom.MethodDeclaration;
import org.eclipse.jdt.core.dom.Modifier;
import org.eclipse.jdt.core.dom.NormalAnnotation;
import org.eclipse.jdt.core.dom.ParameterizedType;
import org.eclipse.jdt.core.dom.SimpleName;
import org.eclipse.jdt.core.dom.SimpleType;
import org.eclipse.jdt.core.dom.SingleVariableDeclaration;
import org.eclipse.jdt.core.dom.StringLiteral;
import org.eclipse.jdt.core.dom.TagElement;
import org.eclipse.jdt.core.dom.TextElement;
import org.eclipse.jdt.core.dom.Type;
import org.eclipse.jdt.core.dom.TypeDeclaration;
import org.eclipse.jdt.core.dom.TypeParameter;
import org.eclipse.jdt.core.dom.VariableDeclarationFragment;
import org.eclipse.jdt.core.dom.WildcardType;
import org.eclipse.jdt.internal.corext.dom.ASTNodes;
import org.eclipse.jdt.internal.corext.dom.Bindings;
import org.eclipse.text.edits.TextEditGroup;

import com.ilog.translator.java2cs.configuration.TranslatorProjectOptions;
import com.ilog.translator.java2cs.configuration.info.MethodInfo;
import com.ilog.translator.java2cs.configuration.info.TranslationModelException;
import com.ilog.translator.java2cs.configuration.target.TargetClass;
import com.ilog.translator.java2cs.translation.ITranslationContext;
import com.ilog.translator.java2cs.translation.noderewriter.ModifiersRewriter;
import com.ilog.translator.java2cs.translation.noderewriter.TypeRewriter;
import com.ilog.translator.java2cs.translation.util.DocUtils;
import com.ilog.translator.java2cs.translation.util.TranslationUtils;
import com.ilog.translator.java2cs.util.TranslationModelUtil;

public class FillBindingTagVisitor extends ASTRewriterVisitor {

	private boolean isATest;

	//
	//
	//

	public FillBindingTagVisitor(ITranslationContext context) {
		super(context);
		transformerName = "Fill Binding Tag Declaration";
		description = new TextEditGroup(transformerName);

	}

	//
	//
	//

	@Override
	public void reset() {
		super.reset();
		isATest = false;
	}

	//
	//
	//

	@Override
	public boolean isAbridged() {
		return true;
	}

	//
	//
	//	

	@Override
	public void endVisit(TypeDeclaration node) {
		final List<TagElement> tags = new ArrayList<TagElement>();
		DocUtils.createBindingTag(context, fCu, node, node.resolveBinding(),
				tags);
		DocUtils.createSignatureTag(context, node, node.resolveBinding(), tags);
		createTestCaseTagIfNeeded(node, node.resolveBinding(), tags);
		DocUtils.addTagsToDoc(currentRewriter, node, tags);
	}

	@Override
	public void endVisit(AnnotationTypeDeclaration node) {
		final List<TagElement> tags = new ArrayList<TagElement>();
		DocUtils.createBindingTag(context, fCu, node, node.resolveBinding(),
				tags);
		DocUtils.createSignatureTag(context, node, node.resolveBinding(), tags);
		DocUtils.addTagsToDoc(currentRewriter, node, tags);
	}

	@SuppressWarnings("unchecked")
	@Override
	public boolean visit(AnnotationTypeMemberDeclaration node) {
		final List<TagElement> tags = new ArrayList<TagElement>();
		//
		DocUtils.createBindingTag(context, fCu, node, node.resolveBinding(),
				tags);
		DocUtils.createSignatureTag(context, node, node.resolveBinding(), tags);
		//
		DocUtils.addTagsToDoc(currentRewriter, node, tags);
		//
		return false;
	}

	@SuppressWarnings("unchecked")
	@Override
	public boolean visit(MethodDeclaration node) {
		final List<TagElement> tags = new ArrayList<TagElement>();
		//
		DocUtils.createBindingTag(context, fCu, node, node.resolveBinding(),
				tags);
		DocUtils.createSignatureTag(context, node, node.resolveBinding(), tags);
		createTestCaseTagIfNeeded(node, node.resolveBinding(), tags);

		// generics : generic methods
		final List typeParams = node.typeParameters();
		if (typeParams != null && typeParams.size() > 0) {
			final TypeParameter typeP = (TypeParameter) typeParams.get(0);
			final List<Type> tBounds = typeP.typeBounds();
			final String paramName = typeP.getName().getIdentifier();
			if (tBounds != null && tBounds.size() > 0) {
				final Type t = tBounds.get(0);
				final ITypeBinding tBind = t.resolveBinding();
				final ITypeBinding[] typeArgs = tBind.getTypeArguments();
				final boolean boundGeneric = typeArgs != null
						&& typeArgs.length > 0;
				TargetClass tClass = null;
				if (boundGeneric)
					try {
						tClass = context.getModel().findGenericClassMapping(
								tBind.getJavaElement().getHandleIdentifier());
					} catch (final Exception e) {
						context.getLogger().logException("", e);
						return true;
					}
				else
					tClass = context.getModel().findClassMapping(
							tBind.getJavaElement().getHandleIdentifier(),
							false, TranslationUtils.isGeneric(tBind));
				String mapped = ASTNodes.getTypeName(t);
				if (tClass != null && tClass.getName() != null) {
					mapped = tClass.getName();
				}

				if (boundGeneric && mapped.contains("<")) {
					final int length = typeArgs.length;
					for (int i = 0; i < length; i++) {
						mapped = mapped.replace("%" + (i + 1), typeArgs[i]
								.getName());
					}
				}
				tags.add(createTag(node.getAST(),
						"@modgenericmethodconstraint ", paramName + "_/_"
								+ mapped));

				// Set type arguments explicitly - cannot be inferred from the
				// usage
				try {
					// TODO: In some case the infered type is false !!!!
					MethodInfo methodInfo = context.getModel().findMethodInfo(
							((TypeDeclaration) ASTNodes.getParent(node,
									ASTNode.TYPE_DECLARATION)).getName()
									.getFullyQualifiedName(),
							null,
							node.resolveBinding().getJavaElement()
									.getHandleIdentifier(),
							node.resolveBinding().isGenericMethod());
					String targetFramework = context.getConfiguration().getOptions().getTargetDotNetFramework().name();
					
					if (methodInfo.getTarget(targetFramework) != null)
						methodInfo.getTarget(targetFramework)
								.setParametersForGenericMethods(mapped);
				} catch (TranslationModelException e) {
					context
							.getLogger()
							.logInfo(
									"ERROR: Set type arguments explicitly - cannot be inferred from the usage");
				}

			} else {
				tags.add(createTag(node.getAST(), "@modgenericmethod ",
						paramName));
			}
		}

		// generics : wildcard
		final List<ParamType> listParamType = new ArrayList<ParamType>();
		int i = 0;
		for (int indice = 0; indice < node.parameters().size(); indice++) {
			final SingleVariableDeclaration astNode = (SingleVariableDeclaration) node
					.parameters().get(indice);
			final List<WildcardType> boundOrWildcard = getWildcardBounds(astNode);
			if (boundOrWildcard != null) {
				removeWildcardExtends(astNode);
				for (final WildcardType wType : boundOrWildcard) {
					final ParamType pt = new ParamType("T" + (i++), wType);
					if (!alreadyContaint(pt, listParamType))
						listParamType.add(pt);
				}
			}
		}
		if (listParamType.size() > 0) {
			for (final ParamType pt : listParamType) {
				final String paramName = pt.name;
				String paramType = null;
				if (pt.type.getBound() != null
						&& pt.type.getBound().getNodeType() == ASTNode.SIMPLE_TYPE) {
					final TypeRewriter tw = (TypeRewriter) context
							.getMapper()
							.mapSimpleType(fCu, (SimpleType) pt.type.getBound());
					if (tw != null)
						paramType = TypeRewriter.filterGenerics(tw.getName());
				}
				if (paramType == null) {
					paramType = ASTNodes.getTypeName(pt.type);
				}
				String bound = "";
				if (pt.type.getBound() != null) {
					bound = "_/_";
					if (pt.type.isUpperBound())
						bound += "extends_/_"
								+ ASTNodes.getTypeName(pt.type.getBound());
					else
						bound += "super_/_"
								+ ASTNodes.getTypeName(pt.type.getBound());
				}
				tags.add(createTag(node.getAST(), "@modwildcard ", paramName
						+ "_/_" + paramType + bound));
			}
		}
		//
		if (hasBeforeAnnotation(node)) {
			createBeforeTag(node, tags);
		} else if (hasAfterAnnotation(node)) {
			createAfterTag(node, tags);
		}
		//
		DocUtils.addTagsToDoc(currentRewriter, node, tags);
		//
		return false;
	}

	@Override
	public boolean visit(TypeDeclaration node) {
		if (TranslationUtils.containsTag(node, context.getModel().getTag(
				TranslationModelUtil.TESTCASE_TAG))) {
			isATest = true;
		}
		return true;
	}

	@Override
	public boolean visit(AnnotationTypeDeclaration node) {
		return true;
	}

	@Override
	public boolean visit(EnumDeclaration node) {
		if (TranslationUtils.containsTag(node, context.getModel().getTag(
				TranslationModelUtil.TESTCASE_TAG))) {
			isATest = true;
		}
		return true;
	}

	@Override
	public boolean visit(FieldDeclaration node) {
		// TODO: failed in case of static ...
		final VariableDeclarationFragment vdf = (VariableDeclarationFragment) node
				.fragments().get(0);
		final List<TagElement> tags = createTagsForField(context, fCu, node,
				vdf);
		// DocUtils.addTagsToDoc(currentRewriter, node, tags);
		final boolean isPrimitive = node.getType().resolveBinding()
				.isPrimitive();
		if (Modifier.isStatic(node.getModifiers())
				&& Modifier.isFinal(node.getModifiers())
				&& (isPrimitive || TranslationUtils.isStringType(node.getType()
						.resolveBinding()))
				&& ModifiersRewriter.isConstable(node)) {
			createConstModifierTag(context, node, vdf.resolveBinding(), tags);
		}
		DocUtils.addTagsToDoc(currentRewriter, node, tags);
		return false;
	}

	public static List<TagElement> createTagsForField(
			ITranslationContext context, ICompilationUnit fCu,
			FieldDeclaration node, final VariableDeclarationFragment vdf) {
		final List<TagElement> tags = new ArrayList<TagElement>();
		DocUtils.createBindingTag(context, fCu, node, vdf.resolveBinding(),
				tags);
		DocUtils.createSignatureTag(context, node, vdf.resolveBinding(), tags);
		return tags;
	}

	//
	//
	//

	private static void createConstModifierTag(ITranslationContext context,
			FieldDeclaration node, IVariableBinding binding,
			List<TagElement> tags) {
		final AST ast = node.getAST();
		Object cst_value = binding.getConstantValue();
		// create handler key
		final TagElement handlerTag = ast.newTagElement();
		handlerTag.setTagName(context.getMapper().getTag(
				TranslationModelUtil.CONST_TAG));
		TextElement textEleme = ast.newTextElement();
		textEleme.setText(cst_value.toString());
		handlerTag.fragments().add(textEleme);
		tags.add(handlerTag);
	}

	@SuppressWarnings("unchecked")
	private void createTestCaseTagIfNeeded(TypeDeclaration node,
			IBinding binding, List<TagElement> tags) {
		final AST ast = node.getAST();

		if (binding != null) {
			if (isAUnitTest() && isATest) {
				final ITypeBinding typeB = (ITypeBinding) binding;
				if (!typeB.isMember() && !typeB.isInterface()) {
					final TagElement testCaseTag = ast.newTagElement();
					testCaseTag.setTagName(context.getMapper().getTag(
							TranslationModelUtil.TESTCASE_TAG));
					tags.add(testCaseTag);
				}
				context.declareAsTest(fCu);
			}
		} else {
			context.getLogger().logError(
					"FillBindingTag.createTestCaseTag : Can't find binding for "
							+ node);
		}
	}

	private void createAfterTag(MethodDeclaration node, List<TagElement> tags) {
		final AST ast = node.getAST();
		final TagElement testCaseTag = ast.newTagElement();
		testCaseTag.setTagName(context.getMapper().getTag(
				TranslationModelUtil.TESTAFTER_TAG));
		tags.add(testCaseTag);
	}

	private void createBeforeTag(MethodDeclaration node, List<TagElement> tags) {
		final AST ast = node.getAST();
		final TagElement testCaseTag = ast.newTagElement();
		testCaseTag.setTagName(context.getMapper().getTag(
				TranslationModelUtil.TESTBEFORE_TAG));
		tags.add(testCaseTag);
	}

	private boolean isAUnitTest() {
		return !(context.getConfiguration().getOptions().getUnitTestLibrary() == TranslatorProjectOptions.UnitTestLibrary.NONE);
	}

	@SuppressWarnings("unchecked")
	private void createTestCaseTagIfNeeded(MethodDeclaration node,
			IBinding binding, List<TagElement> tags) {
		final AST ast = node.getAST();

		if (binding != null) {
			final IMethodBinding methodB = (IMethodBinding) binding;
			if (isAUnitTestMethod(node, methodB)) {
				final TagElement testCaseTag = ast.newTagElement();
				testCaseTag.setTagName(context.getMapper().getTag(
						TranslationModelUtil.TESTMETHOD_TAG));
				final TagElement testCategoryTag = ast.newTagElement();
				testCategoryTag.setTagName(context.getMapper().getTag(
						TranslationModelUtil.TESTCATEGORIE_TAG));
				if (context.getConfiguration().getOptions()
						.getUnitTestLibrary() == TranslatorProjectOptions.UnitTestLibrary.JUNIT4
						&& hasJUnit4IgnoreAnontation(node)) {
					final TextElement te = node.getAST().newTextElement();
					te.setText("ignore");
					testCategoryTag.fragments().add(te);
				} else if (context.getConfiguration().getOptions()
						.getUnitTestLibrary() == TranslatorProjectOptions.UnitTestLibrary.TESTNG) {
					final String categorie = getTestngCategorie(node);
					if (categorie != null) {
						final TextElement te = node.getAST().newTextElement();
						te.setText(categorie);
						testCategoryTag.fragments().add(te);
					}
				}
				tags.add(testCaseTag);
				tags.add(testCategoryTag);
				isATest = true;
			}
		} else {
			context.getLogger().logError(
					"FillBindingTag.createTestCaseTag : Can't find binding for "
							+ node);
		}
	}

	private boolean isAUnitTestMethod(MethodDeclaration node,
			IMethodBinding methodB) {
		return isAJunitTestMethod(node, methodB) || isAJunit4TestMethod(node)
				|| isATestNgTestMethod(node);
	}

	private boolean isAJunitTestMethod(MethodDeclaration node,
			IMethodBinding methodB) {
		return (methodB.getName().startsWith("test")
				&& methodB.getParameterTypes().length == 0
				&& Bindings.isVoidType(methodB.getReturnType()) && !Modifier
				.isStatic(methodB.getModifiers()));
	}

	@SuppressWarnings("unchecked")
	private boolean isAJunit4TestMethod(MethodDeclaration node) {
		final List<IExtendedModifier> modifiers = node.modifiers();
		for (final IExtendedModifier mod : modifiers) {
			if (mod.isAnnotation()) {
				final Annotation ann = (Annotation) mod;
				final String typename = ann.getTypeName()
						.getFullyQualifiedName();
				if (typename.equals("Test"))
					return true;
			}
		}
		return false;
	}

	@SuppressWarnings("unchecked")
	private String getTestngCategorie(MethodDeclaration node) {
		final List<IExtendedModifier> modifiers = node.modifiers();
		for (final IExtendedModifier mod : modifiers) {
			if (mod.isAnnotation()) {
				final Annotation ann = (Annotation) mod;
				final String typename = ann.getTypeName()
						.getFullyQualifiedName();
				if (typename.equals("Test") && ann.isNormalAnnotation()) {
					final NormalAnnotation sma = (NormalAnnotation) ann;
					final List<ASTNode> values = sma.values();
					for (final ASTNode v : values) {
						final MemberValuePair mvp = (MemberValuePair) v;
						final Expression value = mvp.getValue();
						final SimpleName name = mvp.getName();
						if (name.getIdentifier().equals("groups")) {
							if (value.getNodeType() == ASTNode.STRING_LITERAL) {
								final StringLiteral stringValue = (StringLiteral) value;
								return stringValue.getLiteralValue();
							} else if (value.getNodeType() == ASTNode.ARRAY_INITIALIZER) {
								final ArrayInitializer ai = (ArrayInitializer) value;
								final StringLiteral stringValue = (StringLiteral) ai
										.expressions().get(0);
								return stringValue.getLiteralValue();
							}
						}
					}
				}
			}
		}
		return null;
	}

	@SuppressWarnings("unchecked")
	private boolean hasJUnit4IgnoreAnontation(MethodDeclaration node) {
		final List<IExtendedModifier> modifiers = node.modifiers();
		for (final IExtendedModifier mod : modifiers) {
			if (mod.isAnnotation()) {
				final Annotation ann = (Annotation) mod;
				final String typename = ann.getTypeName()
						.getFullyQualifiedName();
				if (typename.equals("org.junit.Ignore"))
					return true;
			}
		}
		return false;
	}

	@SuppressWarnings("unchecked")
	private boolean isATestNgTestMethod(MethodDeclaration node) {
		final List<IExtendedModifier> modifiers = node.modifiers();
		for (final IExtendedModifier mod : modifiers) {
			if (mod.isAnnotation()) {
				final Annotation ann = (Annotation) mod;
				final String typename = ann.getTypeName()
						.getFullyQualifiedName();
				if (typename.equals("Test"))
					return true;
			}
		}
		return false;
	}

	@SuppressWarnings("unchecked")
	private boolean hasAfterAnnotation(MethodDeclaration node) {
		final List<IExtendedModifier> modifiers = node.modifiers();
		for (final IExtendedModifier mod : modifiers) {
			if (mod.isAnnotation()) {
				final Annotation ann = (Annotation) mod;
				final String typename = ann.getTypeName()
						.getFullyQualifiedName();
				if (typename.equals("After"))
					return true;
			}
		}
		return false;
	}

	@SuppressWarnings("unchecked")
	private boolean hasBeforeAnnotation(MethodDeclaration node) {
		final List<IExtendedModifier> modifiers = node.modifiers();
		for (final IExtendedModifier mod : modifiers) {
			if (mod.isAnnotation()) {
				final Annotation ann = (Annotation) mod;
				final String typename = ann.getTypeName()
						.getFullyQualifiedName();
				if (typename.equals("Before"))
					return true;
			}
		}
		return false;
	}

	@SuppressWarnings("unchecked")
	private TagElement createTag(AST ast, String tagName, String tagValue) {
		final TagElement handlerTag = ast.newTagElement();
		handlerTag.setTagName(tagName);
		final TextElement tE = ast.newTextElement();
		tE.setText(tagValue);
		handlerTag.fragments().add(tE);
		return handlerTag;
	}

	static class ParamType {
		String name;
		WildcardType type;

		public ParamType(String s, WildcardType t) {
			name = s;
			type = t;
		}
	}

	private ParamType removeWildcardExtends(SingleVariableDeclaration astNode) {
		return null;
	}

	private List<WildcardType> getWildcardBounds(SingleVariableDeclaration param) {
		final Type type = param.getType();
		if (type.isParameterizedType()) {
			try {
				final TargetClass ci = context.getModel()
						.findGenericClassMapping(
								type.resolveBinding().getJavaElement()
										.getHandleIdentifier());
				if ((ci == null) || (ci != null && !ci.isRemoveGenerics())) {
					// When the mapping class of generic type in java is not
					// generic in .NET we do not want to deal with generics and
					// constraint
					final ParameterizedType pp = (ParameterizedType) type;
					final List<WildcardType> wilcardBounds = new ArrayList<WildcardType>();
					for (final Object ta : pp.typeArguments()) {
						final Type targ = (Type) ta;
						if (targ.isWildcardType()) {
							final WildcardType wType = (WildcardType) targ;
							wilcardBounds.add(wType);
						}
					}
					return wilcardBounds;
				}
			} catch (final Exception e) {
				return null;
			}
		}
		return null;
	}

	private boolean alreadyContaint(ParamType type,
			List<ParamType> wilcardBounds) {
		for (final ParamType wType : wilcardBounds) {
			if (wType.type.getBound() != null && type.type.getBound() != null) {
				if (ASTNodes.asString(wType.type.getBound()).equals(
						ASTNodes.asString(type.type.getBound())))
					return true;
			}
		}
		return false;
	}
}
