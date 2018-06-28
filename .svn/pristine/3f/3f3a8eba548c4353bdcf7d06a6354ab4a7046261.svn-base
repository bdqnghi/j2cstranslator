package com.ilog.translator.java2cs.translation.astrewriter;

import java.util.ArrayList;
import java.util.List;

import org.eclipse.jdt.core.Flags;
import org.eclipse.jdt.core.IMethod;
import org.eclipse.jdt.core.JavaModelException;
import org.eclipse.jdt.core.Signature;
import org.eclipse.jdt.core.dom.AST;
import org.eclipse.jdt.core.dom.ASTNode;
import org.eclipse.jdt.core.dom.BodyDeclaration;
import org.eclipse.jdt.core.dom.FieldDeclaration;
import org.eclipse.jdt.core.dom.IBinding;
import org.eclipse.jdt.core.dom.ITypeBinding;
import org.eclipse.jdt.core.dom.MethodDeclaration;
import org.eclipse.jdt.core.dom.ParameterizedType;
import org.eclipse.jdt.core.dom.TagElement;
import org.eclipse.jdt.core.dom.TextElement;
import org.eclipse.jdt.core.dom.TypeDeclaration;
import org.eclipse.jdt.core.dom.VariableDeclarationFragment;
import org.eclipse.jdt.internal.corext.dom.ASTNodes;
import org.eclipse.text.edits.TextEditGroup;

import com.ilog.translator.java2cs.configuration.target.TargetMethod;
import com.ilog.translator.java2cs.translation.ITranslationContext;
import com.ilog.translator.java2cs.translation.util.DocUtils;
import com.ilog.translator.java2cs.translation.util.TranslationUtils;
import com.ilog.translator.java2cs.util.TranslationModelUtil;
import com.ilog.translator.java2cs.util.Utils;

public class UpdateHandlerTagVisitor extends ASTRewriterVisitor {

	//
	//
	//

	public UpdateHandlerTagVisitor(ITranslationContext context) {
		super(context);
		transformerName = "Update Handler Tag Declaration";
		description = new TextEditGroup(transformerName);
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
	public boolean visit(TypeDeclaration node) {
		final List<TagElement> tags = new ArrayList<TagElement>();
		updateSignatureTag(node, node.resolveBinding(), tags);
		updateHandlerTag(node, node.resolveBinding(), tags);
		DocUtils.addTagsToDoc(currentRewriter, node, tags);
		return true;
	}

	@Override
	public boolean visit(MethodDeclaration node) {
		final List<TagElement> tags = new ArrayList<TagElement>();
		updateSignatureTag(node, node.resolveBinding(), tags);
		updateHandlerTag(node, node.resolveBinding(), tags);
		DocUtils.addTagsToDoc(currentRewriter, node, tags);
		//
		update(node);
		//
		return false;
	}

	@Override
	public boolean visit(FieldDeclaration node) {
		// TODO: failed in case of static ...
		final VariableDeclarationFragment vdf = (VariableDeclarationFragment) node
				.fragments().get(0);
		final List<TagElement> tags = new ArrayList<TagElement>();
		updateSignatureTag(node, vdf.resolveBinding(), tags);
		updateHandlerTag(node, vdf.resolveBinding(), tags);
		DocUtils.addTagsToDoc(currentRewriter, node, tags);
		//
		return false;
	}

	@Override
	public boolean visit(ParameterizedType node) {
		if (node.resolveBinding().isMember()) {
			if (!isGeneric(node.resolveBinding().getDeclaringClass())) {
				final String fName = node.resolveBinding().getDeclaringClass()
						.getQualifiedName()
						+ "." + node.resolveBinding().getName();
				currentRewriter.replace(node, currentRewriter
						.createStringPlaceholder(fName,
								ASTNode.PARAMETERIZED_TYPE), description);
			}
			return false;
		}
		return true;
	}

	//
	//
	//

	private boolean isGeneric(ITypeBinding binding) {
		return binding.isGenericType() || binding.isWildcardType()
				|| binding.isParameterizedType() || binding.isRawType()
				|| binding.isCapture() || binding.getTypeArguments().length > 0
				|| binding.getTypeParameters().length > 0;
	}

	private void update(MethodDeclaration node) {

		final IBinding binding = node.resolveBinding();

		if (binding == null) {
			context.getLogger().logError("Binding null for " + node);
			return;
		}

		final String signatureKey = getSignature(node, binding);
		final String handlerKey = getHandler(node, binding);

		final String existingSignatureTag = TranslationUtils
				.getTagValueFromDoc(node, context.getMapper().getTag(
						TranslationModelUtil.SIGNATURE_TAG));
		final String existingHandlerTag = TranslationUtils.getTagValueFromDoc(
				node, context.getMapper().getTag(
						TranslationModelUtil.HANDLER_TAG));

		if (existingSignatureTag != null
				&& !existingSignatureTag.trim().equals(signatureKey.trim())) {
			// What we really need to do is : find if a targetclass exist, and
			// update
			try {
				final TargetMethod tm = context.getModel().updateMethodMapping(
						fCu.getElementName(), existingSignatureTag,
						existingHandlerTag, signatureKey, handlerKey);
				if (tm != null) {
					context.getLogger().logInfo(
							"Need to update method " + node + " (" + tm
									+ ") with " + signatureKey + "/"
									+ existingHandlerTag);
				} /*
					 * else { context.getLogger().log("Can't find " + node + "
					 * with " + signatureKey + "/" + existingHandlerTag); }
					 */
			} catch (final Exception e) {
				e.printStackTrace();
				context.getLogger().logException("", e);
			}
		}
	}

	@SuppressWarnings("unchecked")
	private void updateSignatureTag(BodyDeclaration node, IBinding binding,
			List<TagElement> tags) {
		final AST ast = node.getAST();

		if (binding == null) {
			context.getLogger().logError("Binding null for " + node);
			return;
		}

		final String signatureKey = getSignature(node, binding);

		final String existingSignatureTag = TranslationUtils
				.getTagValueFromDoc(node, context.getMapper().getTag(
						TranslationModelUtil.SIGNATURE_TAG));

		if (existingSignatureTag != null
				&& !existingSignatureTag.trim().equals(signatureKey.trim())) {
			// create handler key
			final TagElement handlerTag = ast.newTagElement();
			handlerTag.setTagName(context.getMapper().getTag(
					TranslationModelUtil.SIGNATURE_TAG)
					+ "2"); // TODO: !!!!!
			final TextElement tE = ast.newTextElement();
			tE.setText(signatureKey);
			handlerTag.fragments().add(tE);

			tags.add(handlerTag);
		} else if (existingSignatureTag == null) {
			// create handler key
			final TagElement handlerTag = ast.newTagElement();
			handlerTag.setTagName(context.getMapper().getTag(
					TranslationModelUtil.SIGNATURE_TAG)); // TODO: !!!!!
			final TextElement tE = ast.newTextElement();
			tE.setText(signatureKey);
			handlerTag.fragments().add(tE);

			tags.add(handlerTag);
		}
	}

	@SuppressWarnings("unchecked")
	private void updateHandlerTag(BodyDeclaration node, IBinding binding,
			List<TagElement> tags) {
		final AST ast = node.getAST();

		if (binding == null) {
			context.getLogger().logError("Binding null for " + node);
			return;
		}

		final String handlerKey = getHandler(node, binding);

		final String existingHandlerTag = TranslationUtils.getTagValueFromDoc(
				node, context.getMapper().getTag(
						TranslationModelUtil.HANDLER_TAG));

		if (existingHandlerTag != null
				&& !existingHandlerTag.trim().equals(handlerKey.trim())) {
			// create handler key
			final TagElement handlerTag = ast.newTagElement();
			handlerTag.setTagName(context.getMapper().getTag(
					TranslationModelUtil.HANDLER_TAG)
					+ "2"); // TODO: !!!!!
			final TextElement tE = ast.newTextElement();
			tE.setText(Utils.mangle(handlerKey));
			handlerTag.fragments().add(tE);

			tags.add(handlerTag);
		} else if (existingHandlerTag == null) {
			// create handler key
			final TagElement handlerTag = ast.newTagElement();
			handlerTag.setTagName(context.getMapper().getTag(
					TranslationModelUtil.HANDLER_TAG)); // TODO: !!!!!
			final TextElement tE = ast.newTextElement();
			tE.setText(Utils.mangle(handlerKey));
			handlerTag.fragments().add(tE);

			tags.add(handlerTag);
		}
	}

	private String getHandler(BodyDeclaration node, IBinding binding) {
		String handlerKey = null;

		if (binding == null) {
			if (node.getNodeType() == ASTNode.INITIALIZER) {
				final TypeDeclaration parent = ((TypeDeclaration) ASTNodes
						.getParent(node, ASTNode.TYPE_DECLARATION));
				handlerKey = (Flags.isStatic(node.getModifiers()) ? "static"
						: "")
						+ "<init>" + parent.resolveBinding().getName();
			} else {
				context.getLogger().logError(
						"creatBindingTag : error binding == null for : " + node
								+ " / " + fCu.getElementName());
			}
		} else {
			handlerKey = binding.getJavaElement().getHandleIdentifier();
		}

		return handlerKey;
	}

	private String getSignature(BodyDeclaration node, IBinding binding) {
		String signatureKey = null;
		try {
			if (node.getNodeType() == ASTNode.METHOD_DECLARATION) {
				final MethodDeclaration md = (MethodDeclaration) node;
				final IMethod eleme = (IMethod) binding.getJavaElement();
				if (md.isConstructor()) {
					signatureKey = eleme.getSignature() + "<init>";
				} else {
					signatureKey = TranslationUtils.computeSignature(eleme) /* eleme.getSignature() */
							+ binding.getName();
				}
			} else if (node.getNodeType() == ASTNode.TYPE_DECLARATION) {
				final ITypeBinding typeB = (ITypeBinding) binding;
				signatureKey = Signature.createTypeSignature(typeB.getName(),
						true);
			} else if (node.getNodeType() == ASTNode.FIELD_DECLARATION) {
				signatureKey = ""; // TODO
			}
		} catch (final JavaModelException e) {
			e.printStackTrace();
			context.getLogger().logException("", e);
		}

		return signatureKey;
	}

}
