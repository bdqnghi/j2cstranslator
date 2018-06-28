package com.ilog.translator.java2cs.translation;

import org.eclipse.jdt.core.dom.AST;
import org.eclipse.jdt.core.dom.rewrite.ASTRewrite;
import org.eclipse.jdt.internal.core.dom.rewrite.NodeInfoStore;
import org.eclipse.jdt.internal.core.dom.rewrite.RewriteEventStore;

public class TranslatorASTRewrite extends ASTRewrite {

	/**
	 * Creates a new instance for describing manipulations of the given AST.
	 * 
	 * @param ast
	 *            the AST whose nodes will be rewritten
	 * @return the new rewriter instance
	 */
	public static TranslatorASTRewrite create(AST ast) {
		return new TranslatorASTRewrite(ast);
	}

	/**
	 * Internal constructor. Creates a new instance for the given AST. Clients
	 * should use {@link #create(AST)} to create instances.
	 * 
	 * @param ast
	 *            the AST being rewritten
	 */
	protected TranslatorASTRewrite(AST ast) {
		super(ast);
	}

	/**
	 * Internal method. Returns the internal event store. Clients should not
	 * use.
	 * 
	 * @return Returns the internal event store. Clients should not use.
	 */
	public RewriteEventStore getEventStore() {
		return getRewriteEventStore();
	}

	public NodeInfoStore getInternalNodeStore() {
		return getNodeStore();
	}
}
