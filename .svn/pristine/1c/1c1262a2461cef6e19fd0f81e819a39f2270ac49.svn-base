package com.ilog.translator.java2cs.translation.astrewriter;

import org.eclipse.core.runtime.CoreException;
import org.eclipse.core.runtime.IProgressMonitor;
import org.eclipse.jdt.core.dom.MethodDeclaration;
import org.eclipse.text.edits.TextEditGroup;

import com.ilog.translator.java2cs.translation.ITranslationContext;
import com.ilog.translator.java2cs.translation.util.TranslationUtils;

/**
 * 
 * @author afau
 * 
 * Propagate method declaration modification on all override
 */
public class ComputeMethodOverride extends ASTRewriterVisitor {

	//
	//
	//

	public ComputeMethodOverride(ITranslationContext context) {
		super(context);
		transformerName = "Compute Method override";
		description = new TextEditGroup(transformerName);
	}

	//
	//
	//

	@Override
	public boolean applyChange(IProgressMonitor pm) throws CoreException {
		return true;
	}

	//
	//
	//	

	@Override
	public void endVisit(MethodDeclaration node) {
		context.getMapper().mapMethodDeclaration(fCu.getElementName(),
				context.getSignatureFromDoc(node, true),
				getHandler(node), /* search */ false,
				TranslationUtils.isOverride(context, node), false /* isGeneric */);
	}

	private String getHandler(MethodDeclaration node) {
		String handler = null;
		try {
		  handler = node.resolveBinding().getJavaElement().getHandleIdentifier();
		} finally {
		  if (handler == null)
			  handler = context.getHandlerFromDoc(node, true);		
		}
		 return handler;
	}
}
