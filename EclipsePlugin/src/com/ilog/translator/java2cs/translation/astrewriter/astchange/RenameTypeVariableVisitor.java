package com.ilog.translator.java2cs.translation.astrewriter.astchange;

import java.util.ArrayList;
import java.util.Collection;
import java.util.List;

import org.eclipse.core.runtime.CoreException;
import org.eclipse.core.runtime.IProgressMonitor;
import org.eclipse.jdt.core.ITypeParameter;
import org.eclipse.jdt.core.JavaModelException;
import org.eclipse.jdt.core.dom.ASTNode;
import org.eclipse.jdt.core.dom.ASTVisitor;
import org.eclipse.jdt.core.dom.CompilationUnit;
import org.eclipse.jdt.core.dom.TypeDeclaration;
import org.eclipse.jdt.core.dom.TypeParameter;
import org.eclipse.jdt.internal.corext.refactoring.rename.JavaRenameProcessor;
import org.eclipse.jdt.internal.corext.refactoring.rename.RenameTypeParameterProcessor;
import org.eclipse.ltk.core.refactoring.Change;
import org.eclipse.ltk.core.refactoring.RefactoringStatus;
import org.eclipse.ltk.core.refactoring.participants.RenameRefactoring;
import org.eclipse.text.edits.TextEditGroup;

import com.ilog.translator.java2cs.translation.ITranslationContext;

public class RenameTypeVariableVisitor extends ASTChangeVisitor {

	protected TypeParameter firstField = null;

	//
	//
	//

	public RenameTypeVariableVisitor(ITranslationContext context) {
		super(context);
		transformerName = "Rename Type Variable Class";
		description = new TextEditGroup(transformerName);
	}

	//
	//
	//

	@Override
	public void reset() {
		super.reset();
		firstField = null;
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
		final SearchTypeVariableWithConflict visitor = new SearchTypeVariableWithConflict();
		unit.accept(visitor);
		if (visitor.getFirst() != null) {
			change = null;
			firstField = visitor.getFirst();
			return true;
		} else {
			return false;
		}
	}

	//
	//
	//

	@Override
	public boolean needValidation() {
		return true;
	}

	//
	//
	//

	public Change createChange(IProgressMonitor pm, ITypeParameter field, String newName)
			throws JavaModelException, CoreException {
		JavaRenameProcessor processor = new RenameTypeParameterProcessor(field);
		RenameRefactoring refactoring = new RenameRefactoring(processor);
		processor.setNewElementName(newName);

		final RefactoringStatus status = refactoring.checkAllConditions(pm);

		if (status.hasFatalError()) {
			System.err.println(status);
			return null;
		}
		return refactoring.createChange(pm);
	}

	@Override
	public boolean transform(IProgressMonitor pm, ASTNode cunit) {
		try {
			if (firstField != null) {
				final ITypeParameter typeParam = (ITypeParameter) firstField
						.resolveBinding().getJavaElement();
				final String oldName = typeParam.getElementName();
				final String newName = oldName + "_1";
				if (newName != null) {
					change = this.createChange(pm, typeParam, newName);
				}
				// ok need also to recompute signature for corresponding
				// classinfo
			}
		} catch (final Exception e) {
			e.printStackTrace();
			context.getLogger().logException(fCu.getElementName() , e);
		}
		return true;
	}

	//
	//
	//

	private static class SearchTypeVariableWithConflict extends ASTVisitor {

		private TypeParameter first = null;

		@SuppressWarnings("unchecked")
		@Override
		public boolean visit(TypeDeclaration node) {
			final List mainTypeParams = node.typeParameters();
			if (mainTypeParams != null && mainTypeParams.size() > 0) {
				final Collection<String> collectedName = collectNames(mainTypeParams);
				return searchForConflict(node, collectedName);
			}
			return false;
		}

		@SuppressWarnings("unchecked")
		private boolean searchForConflict(TypeDeclaration node,
				Collection<String> collectedName) {
			final TypeDeclaration[] tds = node.getTypes();
			for (final TypeDeclaration td : tds) {
				if (!searchForConflict(td, collectedName))
					return false;
				final List typeParams = td.typeParameters();
				if (typeParams != null && typeParams.size() > 0) {
					for (int j = 0; j < typeParams.size(); j++) {
						final TypeParameter tp = (TypeParameter) typeParams
								.get(j);
						if (collectedName
								.contains(tp.getName().getIdentifier())) {
							first = tp;
							return false;
						}
					}
				}
			}
			return true;
		}

		@SuppressWarnings("unchecked")
		private Collection<String> collectNames(List mainTypeParams) {
			final Collection<String> results = new ArrayList<String>();
			for (int j = 0; j < mainTypeParams.size(); j++) {
				final TypeParameter tp = (TypeParameter) mainTypeParams.get(j);
				results.add(tp.getName().getIdentifier());
			}
			return results;
		}

		public TypeParameter getFirst() {
			return first;
		}
	}
}
