package com.ilog.translator.java2cs.translation.astrewriter;

import java.util.ArrayList;
import java.util.List;

import org.eclipse.jdt.core.IMethod;
import org.eclipse.jdt.core.IType;
import org.eclipse.jdt.core.ITypeHierarchy;
import org.eclipse.jdt.core.Signature;
import org.eclipse.jdt.core.dom.AST;
import org.eclipse.jdt.core.dom.IMethodBinding;
import org.eclipse.jdt.core.dom.MethodDeclaration;
import org.eclipse.jdt.core.dom.TagElement;
import org.eclipse.jdt.core.dom.TextElement;
import org.eclipse.jdt.internal.corext.util.MethodOverrideTester;
import org.eclipse.jdt.internal.corext.util.SuperTypeHierarchyCache;
import org.eclipse.text.edits.TextEditGroup;

import com.ilog.translator.java2cs.translation.ITranslationContext;
import com.ilog.translator.java2cs.translation.util.DocUtils;
import com.ilog.translator.java2cs.util.TranslationModelUtil;

/**
 * Find covariant methods and add correspondg tag to comments
 * 
 * @author afau
 *
 */
public class CovarianceFinderVisitor extends ASTRewriterVisitor {

	//
	//
	//

	public CovarianceFinderVisitor(ITranslationContext context) {
		super(context);
		transformerName = "Find Covariance 1";
		description = new TextEditGroup(transformerName);
	}

	//
	//
	//	

	@Override
	public boolean visit(MethodDeclaration node) {
		final IMethodBinding mb = node.resolveBinding();
		//
		if (isCovariant(node, mb)) {
			// addReferences(mb);
		}
		return true;
	}

	//
	//
	//

	@SuppressWarnings("unchecked")
	private boolean isCovariant(MethodDeclaration node, IMethodBinding mb) {
		if( mb == null || mb.getDeclaringClass() == null ) return false;
		try {
			final IType type = (IType) mb.getDeclaringClass().getJavaElement();
			//final NullProgressMonitor monitor = new NullProgressMonitor();
			final ITypeHierarchy hierarchy = SuperTypeHierarchyCache.getTypeHierarchy(type); // type.newTypeHierarchy(monitor);
			final MethodOverrideTester tester = new MethodOverrideTester(type,
					hierarchy);
			final IMethod method = ((IMethod) mb.getJavaElement());
			final IMethod declaring = tester.findDeclaringMethod(method, true);
			if (declaring != null) {
				if (!method.getReturnType().equals(declaring.getReturnType())) {
					final AST ast = node.getAST();
					final List<TagElement> tags = new ArrayList<TagElement>();
					final TagElement tag = ast.newTagElement();
					tag.setTagName(context.getMapper().getTag(
							TranslationModelUtil.COVARIANCE_TAG));
					final TextElement text = ast.newTextElement();
					final String signature = Signature.toString(declaring
							.getReturnType());
					final String[][] retType = declaring.getDeclaringType()
							.resolveType(signature);
					if (retType != null && retType.length > 0) {
						final String[] ret = retType[0];
						text.setText(ret[0] + "." + ret[1]);
						tag.fragments().add(text);
						tags.add(tag);
						DocUtils.addTagsToDoc(currentRewriter, node,
								tags);
					}
					//
					return true;
				}
			}
		} catch (final Exception e) {
			e.printStackTrace();
			context.getLogger().logException("", e);
		}
		return false;
	}

}
