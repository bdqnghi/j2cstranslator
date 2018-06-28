package com.ilog.translator.java2cs.translation.noderewriter;

import org.eclipse.jdt.core.dom.ASTNode;
import org.eclipse.jdt.core.dom.Expression;
import org.eclipse.jdt.core.dom.MethodDeclaration;
import org.eclipse.jdt.core.dom.MethodInvocation;
import org.eclipse.jdt.core.dom.SuperMethodInvocation;
import org.eclipse.text.edits.TextEditGroup;

import com.ilog.translator.java2cs.translation.ITranslationContext;
import com.ilog.translator.java2cs.translation.TranslatorASTRewrite;
import com.ilog.translator.java2cs.translation.util.TranslationUtils;
import com.ilog.translator.java2cs.util.TranslationModelUtil;

public class PropertyRewriter extends ElementRewriter {

	public static enum ProperyKind {
		READ, WRITE, READ_WRITE
	}

	//
	//
	//
	
	private String name;
	protected String covariantType;
	private final PropertyRewriter.ProperyKind kind;

	//
	//
	//

	public PropertyRewriter(String name, PropertyRewriter.ProperyKind kind) {
		this.name = name;
		this.kind = kind;
	}

	//
	// name
	//

	public void setName(String name) {
		this.name = name;
	}
	
	public String getName() {
		return name;
	}

	//
	// has format
	//
	
	@Override
	public boolean hasFormat() {
		return true;
	}

	//
	//
	//

	@Override
	public void process(ITranslationContext context, ASTNode node,
			TranslatorASTRewrite rew, TranslatorASTRewrite subRewriter,
			TextEditGroup description) {
		switch (node.getNodeType()) {
		case ASTNode.METHOD_INVOCATION/* instanceof MethodInvocation*/:
			final MethodInvocation methodI = (MethodInvocation) node;
			final Expression expr = methodI.getExpression();

			final String replace = (expr == null) ? null : TranslationUtils
					.replaceByNewValue(expr, (subRewriter == null) ? rew
							: subRewriter, fCu, context.getLogger());

			String replacement = null;
			if (kind == ProperyKind.WRITE) {
				if(methodI.arguments().size()>0){
					String param = TranslationUtils
					.replaceByNewValue((ASTNode) methodI.arguments().get(0), (subRewriter == null) ? rew
							: subRewriter, this.fCu, context.getLogger());
					replacement = TranslationUtils.formatPropertyCall(replace, this.name, param);
				}
			} else {
				replacement = TranslationUtils
						.formatPropertyCall(replace, name);
			}

			if (covariantType != null) {
				replacement = "(("
						+ TranslationUtils.castToMethodReturnType(context,
								methodI, fCu) + ") " + replacement + ")";
			}
			
			replacement = TranslationUtils.getNodeWithComments(replacement,
					node, fCu, context);

			final ASTNode placeholder = rew.createStringPlaceholder(
					replacement, ASTNode.METHOD_INVOCATION);
			rew.replace(methodI, placeholder, null);
			break;
		case ASTNode.SUPER_METHOD_INVOCATION /* instanceof SuperMethodInvocation*/:
			final SuperMethodInvocation method = (SuperMethodInvocation) node;

			final String super_kw = context.getMapper().getKeyword(
					TranslationModelUtil.SUPER_KEYWORD,
					TranslationModelUtil.CSHARP_MODEL);

			String getter = TranslationUtils.formatPropertyCall(super_kw, name);

			getter = TranslationUtils.getNodeWithComments(getter, node, fCu,
					context);

			final ASTNode placeholder2 = rew.createStringPlaceholder(getter,
					ASTNode.SUPER_METHOD_INVOCATION);
			rew.replace(method, placeholder2, null);
			break;
		case ASTNode.METHOD_DECLARATION /* instanceof MethodDeclaration*/:
			/*
			 * MethodDeclaration decl = (MethodDeclaration) node; Javadoc doc =
			 * decl.getJavadoc(); Type retType = decl.getReturnType2();
			 * 
			 * if (this.changeModifiers == null) { this.changeModifiers = new
			 * ChangeModifierDescriptor(); }
			 * 
			 * TranslationUtils .getModifiersFromDoc(context, decl,
			 * this.changeModifiers); ModifiersRewriter rewriter = new
			 * ModifiersRewriter(this.changeModifiers); List<DotNetModifier>
			 * newModifiers = rewriter .getRewritedModifiersList(decl, rew,
			 * this.changeModifiers);
			 * 
			 * Block body = decl.getBody(); // public T getToto() { return toto; ); //
			 * public void setToto(T arg) { toto = arg; } // // public T Prop { //
			 * get { return toto; } // set { toto = value; } // }
			 * 
			 * StringBuilder builder = new StringBuilder();
			 * 
			 * String s_doc = TranslationUtils.replaceByNewValue(doc, rew,
			 * this.fCu, context.getLogger());
			 * 
			 * List<String> s_modifiers = new ArrayList<String>(); for
			 * (DotNetModifier mod : newModifiers) { if (!mod.isDotNetOnly()) {
			 * s_modifiers.add(mod.getKeyword()); } }
			 * 
			 * String s_returnType = TranslationUtils.replaceByNewValue(retType,
			 * rew, this.fCu, context.getLogger());
			 * 
			 * String s_body = (body == null) ? null : TranslationUtils
			 * .replaceByNewValue(body, rew, this.fCu, context.getLogger());
			 * 
			 * String formatted = TranslationUtils.formatPropertyDeclaration(
			 * s_doc, s_modifiers, s_returnType, s_body, this.name, this.kind);
			 * 
			 * builder.append(formatted);
			 * 
			 * ASTNode placeholder = rew.createStringPlaceholder(builder
			 * .toString(), ASTNode.METHOD_DECLARATION); rew.replace(decl,
			 * placeholder, null);
			 */
		}
	}

	//
	// Kind
	//
	
	public PropertyRewriter.ProperyKind getKind() {
		return kind;
	}
	
	// 
	// CovariantType
	//

	public void setCovariantType(String covariant) {
		covariantType = covariant;
	}

	public String getCovariantType() {
		return covariantType;
	}

}
