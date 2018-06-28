package com.ilog.translator.java2cs.translation.astrewriter;

import org.eclipse.core.resources.IProject;
import org.eclipse.jdt.core.dom.ASTNode;
import org.eclipse.jdt.core.dom.MethodDeclaration;
import org.eclipse.text.edits.TextEditGroup;

import com.ilog.translator.java2cs.configuration.target.TargetPackage;
import com.ilog.translator.java2cs.translation.ITranslationContext;
import com.ilog.translator.java2cs.translation.util.TranslationUtils;
import com.ilog.translator.java2cs.util.TranslationModelUtil;

/**
 * Covariance changer
 * 
 * 
 * @author afau
 *
 */
public class CovarianceChangerVisitor extends ASTRewriterVisitor {

	//
	//
	//

	public CovarianceChangerVisitor(ITranslationContext context) {
		super(context);
		transformerName = "Changer Covariance";
		description = new TextEditGroup(transformerName);
	}

	
	//
	//
	//	

	@Override
	public boolean visit(MethodDeclaration node) {
		String covariantType = TranslationUtils
				.getTagValueFromDoc(node, context.getMapper().getTag(
						TranslationModelUtil.COVARIANCE_TAG));

		if (!context.getConfiguration().getOptions().isAutoCovariant()
				&& covariantType != null) {
			covariantType = covariantType.trim();
			final int index = covariantType.lastIndexOf(":");
			final String pck = covariantType.substring(0, index);
			final String className = covariantType.substring(index + 1);
			String newClassName = context.getModel().getNewNestedName(pck,
					className);
			if (newClassName != null) {
				covariantType = pck + "." + newClassName;
			} else {
				newClassName = className;
			}
			IProject reference = null;
			if (node.resolveBinding() != null) {
				reference = node.resolveBinding().getJavaElement()
						.getJavaProject().getProject();
			}
			final TargetPackage mappedPck = context.getModel()
					.findPackageMapping(pck, reference);
			if (mappedPck != null) {
				covariantType = mappedPck.getName() + "." + newClassName;
			} else {
				covariantType = TranslationUtils.defaultMappingForPackage(
						context, pck, reference)
						+ "." + newClassName;
			}
			//
			final ASTNode replacement = currentRewriter
					.createStringPlaceholder(covariantType, node
							.getReturnType2().getNodeType());
			currentRewriter.replace(node.getReturnType2(), replacement,
					description);
		}

		return true;
	}
}
