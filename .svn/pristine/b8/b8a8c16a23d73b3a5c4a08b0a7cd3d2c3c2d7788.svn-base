package com.ilog.translator.java2cs.translation.astrewriter.astchange;

import org.eclipse.core.runtime.IProgressMonitor;
import org.eclipse.core.runtime.NullProgressMonitor;
import org.eclipse.jdt.core.ICompilationUnit;
import org.eclipse.jdt.core.IMethod;
import org.eclipse.jdt.core.dom.ASTNode;
import org.eclipse.jdt.core.dom.ASTVisitor;
import org.eclipse.jdt.core.dom.AbstractTypeDeclaration;
import org.eclipse.jdt.core.dom.AnonymousClassDeclaration;
import org.eclipse.jdt.core.dom.CompilationUnit;
import org.eclipse.jdt.core.dom.EnumDeclaration;
import org.eclipse.jdt.core.dom.MethodDeclaration;
import org.eclipse.jdt.core.dom.Modifier;
import org.eclipse.jdt.core.dom.TypeDeclaration;
import org.eclipse.jdt.internal.corext.codemanipulation.CodeGenerationSettings;
import org.eclipse.jdt.internal.corext.dom.ASTNodes;
import org.eclipse.jdt.internal.ui.preferences.JavaPreferencesSettings;
import org.eclipse.text.edits.TextEditGroup;

import com.ilog.translator.java2cs.translation.ITranslationContext;
import com.ilog.translator.java2cs.translation.noderewriter.INodeRewriter;

public class ReplaceAnonymousByInnerVisitor extends ASTChangeVisitor {

	private int cpt = 0;

	private HasAnonymousClassVisitor visitor;

	//
	//
	//

	public ReplaceAnonymousByInnerVisitor(ITranslationContext context) {
		super(context);
		transformerName = "Replace anonymous by inner";
		description = new TextEditGroup(transformerName);
	}

	//
	//
	//

	@Override
	public boolean runOnce() {
		return false;
	}

	@Override
	public boolean runAgain(CompilationUnit unit) {
		if (simulate) {
			return false;
		}
		visitor = new HasAnonymousClassVisitor();
		unit.accept(visitor);
		if (visitor.getResult() != null) {
			change = null;
			return true;
		} else {
			return false;
		}
	}
	

	@Override
	public void postAction(ICompilationUnit icunit, CompilationUnit cunit) {
		final int size = cunit.types().size();
		for (int i = 0; i < size; i++) {
			final AbstractTypeDeclaration decl = (AbstractTypeDeclaration) cunit
					.types().get(i);
			if (decl.getNodeType() == ASTNode.TYPE_DECLARATION) {
				final TypeDeclaration typeDecl = (TypeDeclaration) decl;
				context.buildClassInfoAndScanMapping(typeDecl, false);
			}
		}
	}

	//
	//
	//

	@Override
	public void reset() {
		super.reset();
		cpt = 0;
	}

	//
	//
	//

	@Override
	public boolean transform(IProgressMonitor prm, ASTNode cunit) {
		if (simulate) {
			return true;
		}

		final AnonymousClassDeclaration node = visitor.getResult();
		if (node == null) {
			return true;
		}

		
		boolean isInThisOrSuperInvocConstuctor = ((node.getParent().getParent().getNodeType() == ASTNode.CONSTRUCTOR_INVOCATION) || 
				(node.getParent().getParent().getNodeType() == ASTNode.SUPER_CONSTRUCTOR_INVOCATION));
		
		try {

			final int offset = node.getStartPosition();
			final int length = node.getLength();
			final CodeGenerationSettings settings = JavaPreferencesSettings
					.getCodeGenerationSettings(fCu.getJavaProject());
			settings.useKeywordThis = true;
			final ConvertAnonymousToInnerRefactoring refactoring = new ConvertAnonymousToInnerRefactoring(
					fCu, settings, offset, length, context,
					isInAnIgnoredMethod(node));
			final IProgressMonitor pm = new NullProgressMonitor();
			final String anonName = context.getMapper()
					.getAnonymousClassNamePattern()
					+ cpt++;
			refactoring.setClassName(anonName);	
			refactoring.setVisibility(Modifier.PUBLIC);
			refactoring.checkAllConditions(pm);
			if (isInThisOrSuperInvocConstuctor)
				refactoring.setDeclareStatic(true);	
			change = refactoring.createChange(pm);
			//
		} catch (final Exception e) {
			context.getLogger().logException(
					"Error during " + transformerName + " on "
							+ fCu.getElementName(), e);
			e.printStackTrace();
		}
		return true;
	}

	//
	//
	//

	private boolean isInAnIgnoredMethod(AnonymousClassDeclaration node) {
		final MethodDeclaration methodDecl = (MethodDeclaration) ASTNodes
				.getParent(node, ASTNode.METHOD_DECLARATION);
		if (methodDecl != null) {
			final IMethod method = (IMethod) methodDecl.resolveBinding()
					.getJavaElement();
			final INodeRewriter result = context.getMapper()
					.mapMethodDeclaration(fCu.getElementName(), null,
							method.getHandleIdentifier(), false, false, false);
			if (result != null) {
				return result.isRemove();
			}
		}
		return false;
	}

	private static class HasAnonymousClassVisitor extends ASTVisitor {
		private AnonymousClassDeclaration result = null;

		@Override
		public boolean visit(EnumDeclaration node) {
			return false;
		}

		@Override
		public boolean visit(AnonymousClassDeclaration node) {
			result = node;
			return false;
		}

		public AnonymousClassDeclaration getResult() {
			return result;
		}
	}

}
