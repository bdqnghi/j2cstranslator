package com.ilog.translator.java2cs.translation.noderewriter;

import java.util.Set;
import java.util.TreeSet;

import org.eclipse.jdt.core.ICompilationUnit;
import org.eclipse.jdt.core.dom.ASTNode;
import org.eclipse.text.edits.TextEditGroup;

import sun.reflect.generics.reflectiveObjects.NotImplementedException;

import com.ilog.translator.java2cs.configuration.ChangeModifierDescriptor;
import com.ilog.translator.java2cs.translation.ITranslationContext;
import com.ilog.translator.java2cs.translation.TranslatorASTRewrite;

public abstract class AbstractNodeRewriter implements INodeRewriter {

	protected Set<String> namespaces = new TreeSet<String>();
	protected String format;
	protected ICompilationUnit fCu;
	protected boolean remove = false;

	//
	// process
	//

	public abstract void process(ITranslationContext context, ASTNode method,
			TranslatorASTRewrite rew, TranslatorASTRewrite subRewriter,
			TextEditGroup description);

	//
	// Namespace
	//

	public Set<String> getNamespaces() {
		return namespaces;
	}

	protected void addNamespace(String ns) {
		namespaces.add(ns);
	}

	//
	// Format
	//

	public boolean hasFormat() {
		return format != null;
	}
	
	public boolean hasFormatWithPlaceholder() {
		return hasFormat() && format.contains("@");
	}
	
	public void setFormat(String format) {
		this.format = format;
	}

	//
	// Remove
	//

	public void setRemove(boolean remove) {
		this.remove = remove;
	}

	public boolean isRemove() {
		return remove;
	}

	//
	// ChangeModifier
	//

	public void setChangeModifier(ChangeModifierDescriptor change) {
		// default behavior
	}

	public ChangeModifierDescriptor getChangeModifier() {
		return null;
	}

	//
	// ICompilationUnit
	//
	public void setICompilationUnit(ICompilationUnit fCu) {
		this.fCu = fCu;
	}

	//
	// clone
	//

	@Override
	public INodeRewriter clone() {
		throw new NotImplementedException();
	}
}
