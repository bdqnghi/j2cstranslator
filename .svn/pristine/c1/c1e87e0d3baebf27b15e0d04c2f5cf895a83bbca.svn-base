package com.ilog.translator.java2cs.translation.astrewriter;

import java.util.ArrayList;
import java.util.List;

import org.eclipse.jdt.core.IMethod;
import org.eclipse.jdt.core.JavaModelException;
import org.eclipse.jdt.core.dom.AST;
import org.eclipse.jdt.core.dom.IMethodBinding;
import org.eclipse.jdt.core.dom.MethodDeclaration;
import org.eclipse.jdt.core.dom.TagElement;
import org.eclipse.jdt.core.dom.TextElement;
import org.eclipse.jdt.core.dom.TypeDeclaration;
import org.eclipse.text.edits.TextEditGroup;

import com.ilog.translator.java2cs.configuration.info.ClassInfo;
import com.ilog.translator.java2cs.configuration.info.TranslationModelException;
import com.ilog.translator.java2cs.translation.ITranslationContext;
import com.ilog.translator.java2cs.translation.noderewriter.MethodRewriter;
import com.ilog.translator.java2cs.translation.noderewriter.INodeRewriter;
import com.ilog.translator.java2cs.translation.util.DocUtils;
import com.ilog.translator.java2cs.translation.util.TranslationUtils;
import com.ilog.translator.java2cs.util.TranslationModelUtil;

/**
 * Manual covariance finder
 * 
 * 
 * @author afau
 *
 */
public class CovarianceManualFinderVisitor extends ASTRewriterVisitor {

	//
	//
	//

	public CovarianceManualFinderVisitor(ITranslationContext context) {
		super(context);
		transformerName = "Covariance Manual Finder";
		description = new TextEditGroup(transformerName);
	}

	//
	//
	//	

	public boolean visit(TypeDeclaration node) {
		try {
			// TO TEST !
			final ClassInfo ci = context.getModel().findClassInfo(
				node.resolveBinding().getJavaElement()
						.getHandleIdentifier(), true, true,
				TranslationUtils.isGeneric(node.resolveBinding()));
			if (ci != null) {
				return ci.hasCovariantMethod();
			}
			return false;
		} catch(JavaModelException e) {
			return true;
		} catch(TranslationModelException e) {
			return true;
		}
	}
	
	@SuppressWarnings("unchecked")
	@Override
	public boolean visit(MethodDeclaration node) {
		try {
			// Try to avoid systematic visit of that. Perhaps by adding an attribute
			// to class that have covariance (inherited ?)
			if (!context.getConfiguration().getOptions().isAutoCovariant()
					&& !node.isConstructor()) {
				final IMethodBinding binding = node.resolveBinding();
				final IMethod eleme = (IMethod) binding.getJavaElement();
				String signature = null;
				if (node.isConstructor()) {
					signature = eleme.getSignature() + "<init>";
				} else {
					signature = TranslationUtils.computeSignature(eleme)
							+ binding.getName();
				}
				final String handler = node.resolveBinding().getJavaElement()
						.getHandleIdentifier();
				final INodeRewriter result = context.getMapper()
						.mapMethodDeclaration(fCu.getElementName(), signature,
								handler, false, false, false);
				if (result != null && result instanceof MethodRewriter) {
					final String covariantType = ((MethodRewriter) result)
							.getCovariantType();
					if (covariantType != null
							&& TranslationUtils
									.getTagValueFromDoc(
											node,
											context
													.getMapper()
													.getTag(
															TranslationModelUtil.COVARIANCE_TAG)) == null) {
						final AST ast = node.getAST();
						final List<TagElement> tags = new ArrayList<TagElement>();
						final TagElement tag = ast.newTagElement();
						tag.setTagName(context.getMapper().getTag(
								TranslationModelUtil.COVARIANCE_TAG));
						final TextElement text = ast.newTextElement();
						text.setText(covariantType);
						tag.fragments().add(text);
						tags.add(tag);
						DocUtils.addTagsToDoc(currentRewriter, node,
								tags);
					}
				}
			}
		} catch (final Exception e) {
			context.getLogger().logException("", e);
			e.printStackTrace();
		}
		return true;
	}

}
