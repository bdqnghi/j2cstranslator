package com.ilog.translator.java2cs.translation.astrewriter;

import java.util.List;

import org.eclipse.core.runtime.CoreException;
import org.eclipse.core.runtime.IProgressMonitor;
import org.eclipse.jdt.core.Flags;
import org.eclipse.jdt.core.dom.ASTNode;
import org.eclipse.jdt.core.dom.BodyDeclaration;
import org.eclipse.jdt.core.dom.EnumDeclaration;
import org.eclipse.jdt.core.dom.FieldDeclaration;
import org.eclipse.jdt.core.dom.Javadoc;
import org.eclipse.jdt.core.dom.MethodDeclaration;
import org.eclipse.jdt.core.dom.Modifier;
import org.eclipse.jdt.core.dom.ParameterizedType;
import org.eclipse.jdt.core.dom.SingleVariableDeclaration;
import org.eclipse.jdt.core.dom.TagElement;
import org.eclipse.jdt.core.dom.Type;
import org.eclipse.jdt.core.dom.TypeDeclaration;
import org.eclipse.jdt.core.dom.TypeParameter;
import org.eclipse.jdt.internal.corext.dom.ASTNodes;
import org.eclipse.ltk.core.refactoring.Change;
import org.eclipse.text.edits.TextEditGroup;
import org.eclipse.jdt.core.dom.AnnotationTypeDeclaration;

import com.ilog.translator.java2cs.configuration.ChangeModifierDescriptor;
import com.ilog.translator.java2cs.configuration.DotNetModifier;
import com.ilog.translator.java2cs.configuration.TranslatorProjectOptions;
import com.ilog.translator.java2cs.translation.ITranslationContext;
import com.ilog.translator.java2cs.translation.noderewriter.ElementRewriter;
import com.ilog.translator.java2cs.translation.noderewriter.ModifiersRewriter;
import com.ilog.translator.java2cs.translation.util.TranslationUtils;
import com.ilog.translator.java2cs.util.TranslationModelUtil;

public class FillDotNetModifierVisitor extends ASTRewriterVisitor {

	private boolean isATestCase = false;
	
	//
	//
	//

	public FillDotNetModifierVisitor(ITranslationContext context) {
		super(context);
		transformerName = "Fill DotNet Modifier";
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

	@Override
	public void endVisit(EnumDeclaration node) {
		this.fillModifiers(node, null);
	}

	
	@Override	
	public boolean visit(AnnotationTypeDeclaration node) {
		return false; // skip annotation declaration
	}
	
	@Override
	public void endVisit(TypeDeclaration node) {
		if (TranslationUtils.containsTag(node, context.getMapper().getTag(
				TranslationModelUtil.TESTCASE_TAG))) {
			isATestCase = true;
		}
		final ElementRewriter r = (ElementRewriter) context.getMapper()
				.mapTypeDeclaration(null,
						context.getHandlerFromDoc(node, false));
		if (Flags.isProtected(node.getModifiers())) {
			this.fillModifiers(node, r, false);
		} else
			this.fillModifiers(node, r);

	}

	@SuppressWarnings("unchecked")
	@Override
	public boolean visit(MethodDeclaration node) {
		if (Modifier.isStatic(node.getModifiers()) && node.isConstructor()) {
			return false;
		}
		final ElementRewriter r = (ElementRewriter) context.getMapper()
				.mapMethodDeclaration(fCu.getElementName(),
						context.getSignatureFromDoc(node, false),
						context.getHandlerFromDoc(node, true), false, false,
						false);

		if (Flags.isProtected(node.getModifiers())) {
			this.fillModifiers(node, r, false);
		} else
			this.fillModifiers(node, r);
		//
		// generics : generic methods
		//
		final List typeParams = node.typeParameters();
		if (typeParams != null && typeParams.size() > 0) {
			String params = "<";
			for (int i = 0; i < typeParams.size(); i++) {
				final TypeParameter typeP = (TypeParameter) typeParams.get(i);
				final String paramName = typeP.getName().getIdentifier();
				params += paramName;
				if (i < typeParams.size() - 1)
					params += ",";
				currentRewriter.remove(typeP, description);
			}
			params += ">";
			final String newMName = node.getName().getIdentifier()
					+ "/* insert_here:" + params + " */";
			// TODO: break the compilation of the whole methode !!! -> avoid
			// Addconstructor invoaction to run !
			currentRewriter.replace(node.getName(), currentRewriter
					.createStringPlaceholder(newMName, node.getName()
							.getNodeType()), description);

			//			
		}
		//
		// generics : wildcard
		//
		final List<String> wildcards = TranslationUtils.getTagValuesFromDoc(
				node, "@modwildcard");

		if (wildcards != null) {
			for (int indice = 0; indice < node.parameters().size(); indice++) {
				final SingleVariableDeclaration astNode = (SingleVariableDeclaration) node
						.parameters().get(indice);

				for (int bindice = 0; bindice < wildcards.size(); bindice++) {
					final String currentWildcar = wildcards.get(bindice);
					int index = currentWildcar.indexOf("_/_");
					final String paramName = currentWildcar.substring(0, index)
							.trim();
					//
					final String rest = currentWildcar.substring(index + 3);
					index = rest.indexOf("_/_");
					if (index < 0)
						index = rest.length();
					final String bound = rest.substring(0, index).trim();
					//
					if (hasWildcardBound(astNode)
							&& /* BR 2901241 */ hasThisBound(astNode, bound)) {
						final ParameterizedType pp = (ParameterizedType) astNode
								.getType();
						final Type typeArg = (Type) pp.typeArguments().get(
								bindice);
						currentRewriter.replace(typeArg, currentRewriter
								.createStringPlaceholder(paramName,
										ASTNode.SIMPLE_TYPE), null);
						/*
						 * String tName = ASTNodes.getTypeName(pp.getType()) + "<" +
						 * paramName + ">"; this.currentRewriter.replace(pp,
						 * this.currentRewriter.createStringPlaceholder(tName,
						 * ASTNode.SIMPLE_TYPE), null);
						 */
					}
				}
			}
		}
		return false;
	}

	@Override
	public boolean visit(FieldDeclaration node) {
		final ElementRewriter r = (ElementRewriter) context.getMapper()
				.mapFieldDeclaration(node,
						context.getHandlerFromDoc(node, false));
		if (Flags.isProtected(node.getModifiers()))
			this.fillModifiers(node, r, false);
		else
			this.fillModifiers(node, r);

		return false;
	}

	//
	//
	//

	private void fillModifiers(BodyDeclaration node, ElementRewriter rew,
			boolean isProtectedConstructor) {
		// Fix : If we not clone the ChangeModifierDescriptor,
		// override/virtual
		// modifier are added and in case of inheritance we have method with
		// both virtual and override.
		ChangeModifierDescriptor desc = null;
		if (rew == null) {
			desc = new ChangeModifierDescriptor();
		} else {
			desc = new ChangeModifierDescriptor(rew.getChangeModifier());
		}
		if (!isProtectedConstructor) {
			desc.add(DotNetModifier.INTERNAL);
		}
		if (context.getConfiguration().getOptions().getUnitTestLibrary() == 
			TranslatorProjectOptions.UnitTestLibrary.JUNIT3) {
			if (node.getNodeType() == ASTNode.METHOD_DECLARATION) {
				final MethodDeclaration mDecl = (MethodDeclaration) node;
				if (mDecl.getName().getIdentifier().equals("SetUp")
						|| TranslationUtils.containsTag(node, context.getMapper()
								.getTag(TranslationModelUtil.TESTBEFORE_TAG))) {
					desc.remove(DotNetModifier.INTERNAL);
					desc.remove(DotNetModifier.OVERRIDE);
				}
				/*if (mDecl.getName().getIdentifier().equals("SetUp")) {
					desc.remove(DotNetModifier.INTERNAL);
				}*/
			}
		}
		if (node.getNodeType() == ASTNode.TYPE_DECLARATION
				&& Modifier.isFinal(node.getModifiers())) {
			desc.replace(DotNetModifier.FINAL, DotNetModifier.SEALED);
		}
		getTagFromDoc(node, desc);
		ModifiersRewriter modRewrite = new ModifiersRewriter(desc);
		modRewrite.setOnlyDotNet(true);
		modRewrite.setOnlyJava(false);
		modRewrite.process(context, node, currentRewriter, null, description);
		modRewrite = null;
		desc = null;
	}

	private void fillModifiers(BodyDeclaration node, ElementRewriter rew) {
		// Fix : If we not clone the ChangeModifierDescriptor,
		// override/virtual
		// modifier are added and in case of inheritance we have method with
		// both virtual and override.
		ChangeModifierDescriptor desc = null;
		if (rew == null) {
			desc = new ChangeModifierDescriptor();
		} else {
			desc = new ChangeModifierDescriptor(rew.getChangeModifier());
		}
		if (context.getConfiguration().getOptions().getUnitTestLibrary() == 
			TranslatorProjectOptions.UnitTestLibrary.JUNIT3) {
			if (node.getNodeType() == ASTNode.METHOD_DECLARATION) {
				final MethodDeclaration mDecl = (MethodDeclaration) node;
				if (mDecl.getName().getIdentifier().equals("SetUp")
						|| TranslationUtils.containsTag(node, context.getMapper()
								.getTag(TranslationModelUtil.TESTBEFORE_TAG))) {
					desc.remove(DotNetModifier.INTERNAL);
					desc.remove(DotNetModifier.OVERRIDE);
				}
				/*if (mDecl.getName().getIdentifier().equals("SetUp")) {
					desc.remove(DotNetModifier.INTERNAL);
				}*/
			}
		}
		if (node.getNodeType() == ASTNode.TYPE_DECLARATION
				&& Modifier.isFinal(node.getModifiers())) {
			desc.replace(DotNetModifier.FINAL, DotNetModifier.SEALED);
		}
		getTagFromDoc(node, desc);
		ModifiersRewriter modRewrite = new ModifiersRewriter(desc);
		modRewrite.setOnlyDotNet(true);
		modRewrite.setOnlyJava(false);
		modRewrite.process(context, node, currentRewriter, null, description);
		modRewrite = null;
		desc = null;
	}

	@SuppressWarnings("unchecked")
	private void getTagFromDoc(BodyDeclaration node,
			ChangeModifierDescriptor desc) {
		final Javadoc doc = node.getJavadoc();
		boolean virtual = false;
		boolean override = false;
		boolean const_modifier = false;

		if (doc != null) {
			List tags = doc.tags();
			for (int i = 0; i < tags.size(); i++) {
				final TagElement te = (TagElement) tags.get(i);
				final String name = te.getTagName();
				if (name != null) {
					if (name
							.equals(context.getMapper().getTag(
									TranslationModelUtil.VIRTUAL_TAG) /* "@virtual" */)) {
						virtual = true;
					} else if (name
							.equals(context.getMapper().getTag(
									TranslationModelUtil.OVERRIDE_TAG) /* "@override" */)) {
						override = true;
					}
					if (name.equals(context.getMapper().getTag(
							TranslationModelUtil.CONST_TAG) /* "@const" */)) {
						const_modifier = true;
					}
				}
			}
			tags = null;
		}

		if (desc != null) {
			if (virtual) {
				desc.add(DotNetModifier.VIRTUAL);
			} else if (override) {
				desc.add(DotNetModifier.OVERRIDE);
			}
		}
		if (const_modifier) {
			desc.add(DotNetModifier.CONST);
			desc.remove(DotNetModifier.STATIC);
			desc.remove(DotNetModifier.FINAL);
		}
	}

	private boolean hasWildcardBound(SingleVariableDeclaration param) {
		final Type type = param.getType();
		if (type.isParameterizedType()) {
			final ParameterizedType pp = (ParameterizedType) type;
			for (final Object ta : pp.typeArguments()) {
				final Type targ = (Type) ta;
				if (targ.isWildcardType()) {
					return true;
				}
			}
		}
		return false;
	}

	private boolean hasThisBound(SingleVariableDeclaration param,
			String typeName) {
		final Type type = param.getType();
		if (type.isParameterizedType()) {
			final ParameterizedType pp = (ParameterizedType) type;
			for (final Object ta : pp.typeArguments()) {
				final Type targ = (Type) ta;
				final String name = ASTNodes.getTypeName(targ);
				if (name.equals(typeName))
					return true;
			}
		}
		return false;
	}
}
