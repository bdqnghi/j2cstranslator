package com.ilog.translator.java2cs.translation.astrewriter;

import org.eclipse.core.runtime.CoreException;
import org.eclipse.core.runtime.IProgressMonitor;
import org.eclipse.core.runtime.SubProgressMonitor;
import org.eclipse.jdt.core.ICompilationUnit;
import org.eclipse.jdt.core.dom.ASTNode;
import org.eclipse.jdt.core.dom.ASTVisitor;
import org.eclipse.jdt.core.dom.CompilationUnit;
import org.eclipse.ltk.core.refactoring.Change;
import org.eclipse.ltk.core.refactoring.RefactoringStatus;
import org.eclipse.text.edits.TextEditGroup;

import com.ilog.translator.java2cs.configuration.options.OptionImpl;
import com.ilog.translator.java2cs.translation.ITransformer;
import com.ilog.translator.java2cs.translation.ITranslationContext;

public abstract class TranslationVisitor extends ASTVisitor implements
		ITransformer {

	protected ICompilationUnit fCu;

	protected ITranslationContext context;

	protected String transformerName = null;

	protected TextEditGroup description;

	protected boolean simulate = false;

	protected String triggerOptionName;
	protected OptionImpl<?> triggerOption;

	//
	//
	//

	public TranslationVisitor(ITranslationContext context) {
		this.context = context;
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

	public boolean transform(IProgressMonitor pm, ASTNode cunit) {
		cunit.accept(this);
		return true;
	}

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
		Change change = createChange(pm, null);

		if (change != null) {
			final IProgressMonitor subMonitor = new SubProgressMonitor(pm, 1);

			try {
				change.initializeValidationData(subMonitor);
				if (!change.isEnabled()) {
					context.getLogger().logError(
							transformerName + " Change is not enabled on "
									+ fCu);
					return false;
				}
				final RefactoringStatus valid = change.isValid(subMonitor);
				if (valid.hasFatalError()) {
					context.getLogger().logError(
							transformerName + " JDT Fatal Error on " + fCu
									+ " " + " " + valid.toString());
					return false;
				}
				change.perform(subMonitor);
				return true;
			} catch (final Exception e) {
				context.getLogger().logException(
						transformerName + " Error on " + fCu + " "
								+ e.getMessage() + " " + e.toString(), e);
				// throw e;
				return false;
			} finally {
				change.dispose();
				change = null;
			}
		} else {
			return false;
		}
	}

	//
	//
	//

	public abstract Change createChange(IProgressMonitor pm, Object param)
			throws CoreException;

	//
	//
	//

	public abstract boolean runAgain(CompilationUnit unit);

	public abstract boolean runOnce();

	//
	//
	//

	public abstract boolean needValidation();

	public boolean isAbridged() {
		return false;
	}

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
					return true; /* option.getValue() ?? */
			} else {
				return true; /* option.getValue() ?? */
			}
		}
	}

	public void setTriggerConditionName(String option) {
		triggerOptionName = option;
	}
}
