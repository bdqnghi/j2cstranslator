package com.ilog.translator.java2cs.translation.astrewriter;

import java.util.ArrayList;
import java.util.List;

import org.eclipse.jdt.core.Flags;
import org.eclipse.jdt.core.JavaModelException;
import org.eclipse.jdt.core.dom.EnumDeclaration;
import org.eclipse.jdt.core.dom.FieldDeclaration;
import org.eclipse.jdt.core.dom.MethodDeclaration;
import org.eclipse.jdt.core.dom.TypeDeclaration;
import org.eclipse.text.edits.TextEditGroup;

import com.ilog.translator.java2cs.configuration.info.ClassInfo;
import com.ilog.translator.java2cs.configuration.info.TranslationModelException;
import com.ilog.translator.java2cs.configuration.target.TargetField;
import com.ilog.translator.java2cs.configuration.target.TargetMethod;
import com.ilog.translator.java2cs.translation.ITranslationContext;
import com.ilog.translator.java2cs.translation.TranslatorASTRewrite;
import com.ilog.translator.java2cs.translation.util.DocUtils;
import com.ilog.translator.java2cs.translation.util.TranslationUtils;
import com.ilog.translator.java2cs.util.TranslationModelUtil;

public class RemoveCustomCommentsVisitor extends ASTRewriterVisitor {

	private final List<String> tags = new ArrayList<String>();
	private boolean processDoc = true;
	private String commentsForClass = null;

	//
	//
	//
	public RemoveCustomCommentsVisitor(ITranslationContext context, TranslatorASTRewrite currentRewriter) {
		this(context);
		this.currentRewriter = currentRewriter;
	}
	
	public RemoveCustomCommentsVisitor(ITranslationContext context) {
		super(context);
		transformerName = "Remove custom Comments";
		description = new TextEditGroup(transformerName);
		//
		tags.add(context.getMapper().getTag(TranslationModelUtil.HANDLER_TAG));
		tags
				.add(context.getMapper().getTag(
						TranslationModelUtil.SIGNATURE_TAG));
		tags.add(context.getMapper().getTag(TranslationModelUtil.HANDLER_TAG)
				+ "2");
		tags.add(context.getMapper().getTag(TranslationModelUtil.SIGNATURE_TAG)
				+ "2");
		tags.add(context.getMapper().getTag(TranslationModelUtil.OVERRIDE_TAG));
		tags.add(context.getMapper().getTag(TranslationModelUtil.VIRTUAL_TAG));
		tags.add(context.getMapper().getTag(TranslationModelUtil.CONST_TAG));
		tags.add(context.getMapper()
				.getTag(TranslationModelUtil.COVARIANCE_TAG));
		tags
				.add(context.getMapper().getTag(
						TranslationModelUtil.PUBLICAPI_TAG));
		tags.add(context.getMapper().getTag(TranslationModelUtil.TESTCASE_TAG));
		tags.add(context.getMapper()
				.getTag(TranslationModelUtil.TESTMETHOD_TAG));
		tags.add(context.getMapper().getTag(
				TranslationModelUtil.TESTCATEGORIE_TAG));
		tags
				.add(context.getMapper().getTag(
						TranslationModelUtil.TESTAFTER_TAG));
		tags.add(context.getMapper()
				.getTag(TranslationModelUtil.TESTBEFORE_TAG));
		tags.add(context.getMapper().getTag(TranslationModelUtil.BINDING_TAG));
		tags.add(context.getMapper().getTag(TranslationModelUtil.REMOVE_TAG));
		tags.add("@modgenericmethodconstraint");
		tags.add("@modgenericmethod");
		tags.add("@modwildcard");
		tags.add("@translator_mapping");
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
	public void endVisit(MethodDeclaration node) {
		String annotationToAdd = "";
		if (!TranslationUtils.containsTag(node, context.getMapper().getTag(
				TranslationModelUtil.PUBLICAPI_TAG))) {
			if (node.getJavadoc() != null
					&& context.getConfiguration().getOptions()
							.isAddNotBrowsableAttribute()) {
				annotationToAdd = context.getMapper()
						.getNotBrowsableAttribute();
			}
		}
		if (context.isSynchronized(node)) {
			annotationToAdd += context.getMapper().getSynchronizedAttribute();
		}
		if (processDoc) {
			String newComments = null;
			try {
				if (!node.isConstructor() && !Flags.isStatic(node.getFlags())) {
					TargetMethod tMethod = context.getModel()
							.findMethodMapping(fCu.getElementName(),
									context.getSignatureFromDoc(node, false),
									context.getHandlerFromDoc(node, true),
									false, false, false, true);
					if (tMethod != null && tMethod.hasComments()) {
						newComments = tMethod.getComments();
					}
				}
			} catch (TranslationModelException e) {
				context.getLogger().logException("", e);
			} catch (JavaModelException e) {
				context.getLogger().logException("", e);
			}
			DocUtils.processDoc(context, currentRewriter, node, tags,
					annotationToAdd, newComments);
		} else {
			DocUtils.removeDocTags(context, currentRewriter, node,
					tags, annotationToAdd);
		}
	}

	@Override
	public void endVisit(FieldDeclaration node) {
		String annotationToAdd = "";
		if (!TranslationUtils.containsTag(node, context.getMapper().getTag(
				TranslationModelUtil.PUBLICAPI_TAG))) {
			if (node.getJavadoc() != null
					&& context.getConfiguration().getOptions()
							.isAddNotBrowsableAttribute()) {
				annotationToAdd = context.getMapper()
						.getNotBrowsableAttribute();
			}
		}

		if (processDoc) {
			String handler = context.getHandlerFromDoc(node, false);
			String newComments = null;
			if (handler != null) {
				TargetField tField = context.getModel().findFieldMapping(
						node.toString(), handler);

				if (tField != null && tField.hasComments()) {
					newComments = tField.getComments();
				}
			}
			DocUtils.processDoc(context, currentRewriter, node, tags,
					annotationToAdd, newComments);
		} else {
			DocUtils.removeDocTags(context, currentRewriter, node,
					tags, annotationToAdd);
		}
	}

	@Override
	public boolean visit(TypeDeclaration node) {
		processDoc = true;
		commentsForClass = null;
		try {
			String targetFramework = context.getConfiguration().getOptions().getTargetDotNetFramework().name();
			
			final ClassInfo ci = context.getModel().findClassInfo(
					context.getHandlerFromDoc(node, false), false, false, true);
			if (ci != null && ci.getTarget(targetFramework) != null) {
				processDoc = ci.getTarget(targetFramework).isProcessDoc();
				if (ci.getTarget(targetFramework) != null
						&& ci.getTarget(targetFramework).hasComments()) {
					commentsForClass = ci.getTarget(targetFramework).getComments();
				}
			}
		} catch (final TranslationModelException e) {
			context.getLogger().logException("", e);
		} catch (final JavaModelException e) {
			context.getLogger().logException("", e);
		}

		return true;
	}
	
	@Override
	public boolean visit(EnumDeclaration node) {
		processDoc = true;
		commentsForClass = null;
		try {
			String targetFramework = context.getConfiguration().getOptions().getTargetDotNetFramework().name();
			
			final ClassInfo ci = context.getModel().findClassInfo(
					context.getHandlerFromDoc(node, false), false, false, true);
			if (ci != null && ci.getTarget(targetFramework) != null) {
				processDoc = ci.getTarget(targetFramework).isProcessDoc();
				if (ci.getTarget(targetFramework) != null
						&& ci.getTarget(targetFramework).hasComments()) {
					commentsForClass = ci.getTarget(targetFramework).getComments();
				}
			}
		} catch (final TranslationModelException e) {
			context.getLogger().logException("", e);
		} catch (final JavaModelException e) {
			context.getLogger().logException("", e);
		}

		return true;
	}

	@Override
	public void endVisit(TypeDeclaration node) {
		String annotationToAdd = "";
		if (!TranslationUtils.containsTag(node, context.getMapper().getTag(
				TranslationModelUtil.PUBLICAPI_TAG))) {
			if (node.getJavadoc() != null
					&& context.getConfiguration().getOptions()
							.isAddNotBrowsableAttribute()) {
				annotationToAdd = context.getMapper()
						.getNotBrowsableAttribute();
			}
		}
		if (context.isSerializable(node)) {
			annotationToAdd += context.getMapper().getSerializableAttribute();
		}
		//

		if (processDoc)
			DocUtils.processDoc(context, currentRewriter, node, tags,
					annotationToAdd, commentsForClass);
		else {
			DocUtils.removeDocTags(context, currentRewriter, node,
					tags, annotationToAdd);
		}
	}
	
	@Override
	public void endVisit(EnumDeclaration node) {
		String annotationToAdd = "";
		if (!TranslationUtils.containsTag(node, context.getMapper().getTag(
				TranslationModelUtil.PUBLICAPI_TAG))) {
			if (node.getJavadoc() != null
					&& context.getConfiguration().getOptions()
							.isAddNotBrowsableAttribute()) {
				annotationToAdd = context.getMapper()
						.getNotBrowsableAttribute();
			}
		}
		//

		if (processDoc)
			DocUtils.processDoc(context, currentRewriter, node, tags,
					annotationToAdd, commentsForClass);
		else {
			DocUtils.removeDocTags(context, currentRewriter, node,
					tags, annotationToAdd);
		}
	}
	
	public List<String> getTags() {
		return tags;
	}

}
