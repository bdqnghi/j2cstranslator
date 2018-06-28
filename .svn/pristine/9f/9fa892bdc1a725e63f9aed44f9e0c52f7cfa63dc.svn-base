package com.ilog.translator.java2cs.translation.textrewriter;

import java.util.List;

import org.eclipse.core.runtime.CoreException;
import org.eclipse.core.runtime.IProgressMonitor;
import org.eclipse.core.runtime.SubProgressMonitor;
import org.eclipse.jdt.core.IBuffer;
import org.eclipse.jdt.core.ICompilationUnit;
import org.eclipse.jdt.core.dom.ASTNode;
import org.eclipse.jdt.core.dom.CompilationUnit;
import org.eclipse.jdt.internal.corext.refactoring.changes.CompilationUnitChange;
import org.eclipse.ltk.core.refactoring.Change;
import org.eclipse.ltk.core.refactoring.RefactoringStatus;
import org.eclipse.text.edits.MultiTextEdit;
import org.eclipse.text.edits.TextEdit;

import com.ilog.translator.java2cs.configuration.options.OptionImpl;
import com.ilog.translator.java2cs.translation.ITransformer;
import com.ilog.translator.java2cs.translation.ITranslationContext;

public abstract class TextRewriter implements ITransformer {

	protected ICompilationUnit fCu;

	protected boolean runOnce = true;

	protected ITranslationContext context;

	protected String transformerName = null;

	protected boolean simulate = false;

	protected String triggerOptionName;
	protected OptionImpl<?> triggerOption;

	//
	//
	//	

	public TextRewriter(ITranslationContext context) {
		this.context = context;
	}

	//
	//
	//

	public boolean needRecovery() {
		return false;
	}

	//
	//
	//

	public void setSimulation(boolean sim) {
		simulate = sim;
	}

	//
	//
	//

	public boolean needCompilable() {
		return false;	
	}
	
	//
	// name
	//

	public String getName() {
		return transformerName;
	}

	//
	//
	//

	public void setCompilationUnit(ICompilationUnit icunit) {
		fCu = icunit;
	}

	//
	//
	//

	public void reset() {
		runOnce = true;
	}

	//
	//
	//

	public boolean needValidation() {
		return false;
	}

	//
	//
	//

	public boolean isAbridged() {
		return false;
	}

	//
	//
	//

	public boolean runOnce() {
		return true;
	}

	public boolean runAgain(CompilationUnit unit) {
		if (runOnce) {
			runOnce = false;
			return true;
		} else {
			return false;
		}
	}

	//
	//
	//

	public abstract boolean transform(IProgressMonitor pm, ASTNode cunit);

	//
	//
	//

	public void postAction(ICompilationUnit icunit, CompilationUnit unit) {
		// Do nothing
	}
	
	public void postActionOnAST(ICompilationUnit icunit, CompilationUnit unit) {
		// Do nothing
	}

	public boolean hasPostActionOnAST() {
		return false;
	}
	
	//
	//
	//

	public boolean applyChange(IProgressMonitor pm) throws CoreException {
		final Change change = createChange(pm, null);

		if (change != null) {
			final IProgressMonitor subMonitor = new SubProgressMonitor(pm, 1);

			try {
				change.initializeValidationData(subMonitor);

				if (!change.isEnabled()) {
					return false;
				}
				final RefactoringStatus valid = change.isValid(subMonitor);
				if (valid.hasFatalError()) {
					return false;
				}
				final Change undo = change.perform(subMonitor);
				if (undo != null) {
					// TODO : undo.initializeValidationData(subMonitor);
					// do something with the undo object
					return true;
				}
				return true;
			} catch (final Exception e) {
				context.getLogger().logException(
						"Error on proccesing class " + fCu.getElementName(), e);
				e.printStackTrace();
				return false;
			} finally {
				change.dispose();
			}
		} else {
			return false;
		}
	}

	//
	//
	//

	public Change createChange(IProgressMonitor pm, Object param)
			throws CoreException {
		final CompilationUnitChange change = new CompilationUnitChange(
				transformerName, fCu);

		// int offset = 0;
		final int length = fCu.getBuffer().getLength();
		change.setEdit(new MultiTextEdit(0, length));
		final List<TextEdit> edits = computeEdit(pm, fCu.getBuffer());
		for (final TextEdit edit : edits) {
			try {
				change.addEdit(edit);
			} catch (final Exception e) {
				context.getLogger().logException(
						"Error on file " + fCu.getElementName() + " on edit "
								+ edit, e);
				e.printStackTrace();
				return change;
			}
		}

		return change;
	}

	public abstract List<TextEdit> computeEdit(IProgressMonitor pm,
			IBuffer buffer) throws CoreException;

	//
	//
	//
	public boolean canRun() {
		if (triggerOptionName == null)
			return true;
		else {
			final OptionImpl<?> option = context.getConfiguration().getOptions()
					.findOption(triggerOptionName);
			if (option != null) {
				triggerOption = option;
				if (option.isDefaultValue())
					return false;
				else
					return true;
			} else {
				return true;
			}
		}
	}

	public void setTriggerConditionName(String option) {
		triggerOptionName = option;
	}

}
