package com.ilog.translator.java2cs.translation.noderewriter;

import java.util.ArrayList;
import java.util.List;

import org.eclipse.jdt.core.dom.ASTNode;
import org.eclipse.jdt.core.dom.rewrite.ASTRewrite;

import com.ilog.translator.java2cs.configuration.ChangeModifierDescriptor;

public abstract class ElementRewriter extends AbstractNodeRewriter {

	protected ChangeModifierDescriptor changeModifiers = null;
	protected boolean modifyModifiers = false;	
	@SuppressWarnings("unchecked")
	protected List typeArgs;

	//
	//
	//

	protected ElementRewriter() {
	}

	//
	//
	//

	@SuppressWarnings("unchecked")
	public void setTypeArguments(List list) {
		typeArgs = list;
	}

	//
	//
	//

	@Override
	public void setChangeModifier(ChangeModifierDescriptor change) {
		changeModifiers = change;
	}

	@Override
	public ChangeModifierDescriptor getChangeModifier() {
		return changeModifiers;
	}

	//
	//
	//

	public void setModifyModifiers(boolean b) {
		modifyModifiers = b;
	}

	//
	//
	//
	//	

	@SuppressWarnings("unchecked")
	protected List copyArguments(List args, ASTRewrite rew) {
		final List copiedArgs = new ArrayList();
		for (int i = 0; i < args.size(); i++) {
			final ASTNode n = (ASTNode) args.get(i);
			copiedArgs.add(rew.createCopyTarget(n));
		}
		return copiedArgs;
	}

	protected boolean containsObjectArgument() {
		if (format != null) {
			final int len = format.length();
			for (int i = 0; i < len; i++) {
				if ((format.charAt(i) == '@') && (format.charAt(i + 1) == 0)) {
					return true;
				}
			}
		}
		return false;
	}
}
