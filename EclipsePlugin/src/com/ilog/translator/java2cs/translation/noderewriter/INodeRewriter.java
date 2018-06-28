package com.ilog.translator.java2cs.translation.noderewriter;

import java.util.Set;

import org.eclipse.jdt.core.ICompilationUnit;
import org.eclipse.jdt.core.dom.ASTNode;
import org.eclipse.text.edits.TextEditGroup;

import com.ilog.translator.java2cs.configuration.ChangeModifierDescriptor;
import com.ilog.translator.java2cs.translation.ITranslationContext;
import com.ilog.translator.java2cs.translation.TranslatorASTRewrite;

public interface INodeRewriter {

	public abstract void process(ITranslationContext context, ASTNode method,
			TranslatorASTRewrite rew, TranslatorASTRewrite subRewriter,
			TextEditGroup description);

	// namespace
	public abstract Set<String> getNamespaces();

	// modifiers
	public abstract void setChangeModifier(ChangeModifierDescriptor change);

	public abstract ChangeModifierDescriptor getChangeModifier();

	// format / pattern
	public abstract boolean hasFormat();
	public abstract boolean hasFormatWithPlaceholder();
	
	// compilation unit
	public abstract void setICompilationUnit(ICompilationUnit fCu);

	// remove
	public abstract void setRemove(boolean remove);
	public abstract boolean isRemove();

	public abstract INodeRewriter clone();
}
