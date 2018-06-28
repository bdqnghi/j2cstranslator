package com.ilog.translator.java2cs.translation.astrewriter.astchange;

import org.eclipse.core.runtime.CoreException;
import org.eclipse.core.runtime.IProgressMonitor;
import org.eclipse.jdt.core.IField;
import org.eclipse.jdt.core.JavaModelException;
import org.eclipse.jdt.core.dom.ASTNode;
import org.eclipse.jdt.core.dom.ASTVisitor;
import org.eclipse.jdt.core.dom.CompilationUnit;
import org.eclipse.jdt.core.dom.FieldDeclaration;
import org.eclipse.jdt.core.dom.ITypeBinding;
import org.eclipse.jdt.core.dom.VariableDeclarationFragment;
import org.eclipse.jdt.internal.corext.refactoring.rename.RenameFieldProcessor;
import org.eclipse.ltk.core.refactoring.Change;
import org.eclipse.ltk.core.refactoring.RefactoringStatus;
import org.eclipse.ltk.core.refactoring.participants.RenameRefactoring;
import org.eclipse.text.edits.TextEditGroup;

import com.ilog.translator.java2cs.configuration.info.ClassInfo;
import com.ilog.translator.java2cs.configuration.target.TargetField;
import com.ilog.translator.java2cs.translation.ITranslationContext;

public class RenameFieldDeclarationWithForbiddenNameClassVisitor extends
		ASTChangeVisitor {

	protected FieldDeclaration firstField = null;

	//
	//
	//

	public RenameFieldDeclarationWithForbiddenNameClassVisitor(
			ITranslationContext context) {
		super(context);
		transformerName = "Rename Field with forbidden name Class";
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
		final SearchFieldWithForibbidenName visitor = new SearchFieldWithForibbidenName();
		unit.accept(visitor);
		if (visitor.first != null) {
			change = null;
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

	public Change createChange(IProgressMonitor pm, IField field, String newName)
			throws JavaModelException, CoreException {
		// IJavaProject project = fCu.getJavaProject();
		RenameFieldProcessor processor = new RenameFieldProcessor(field);
		RenameRefactoring refactoring = new RenameRefactoring(processor);
		processor.setNewElementName(newName);
		final RefactoringStatus status = refactoring.checkAllConditions(pm);

		// TODO:
		// log that rename in order to add it to implicit renaming

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
				final VariableDeclarationFragment fragment = (VariableDeclarationFragment) firstField
						.fragments().get(0);
				final String oldName = fragment.getName().getIdentifier();
				final String newName = context.getModel().getVariable(oldName);
				if (newName != null) {
					change = this.createChange(pm, (IField) fragment
							.resolveBinding().getJavaElement(), newName);
					final IField ifield = (IField) fragment.resolveBinding()
							.getJavaElement();
					final ClassInfo ci = context.getModel().findClassInfo(
							ifield.getDeclaringType().getHandleIdentifier(),
							true, false, false);
					ci.implicitFieldRename(ifield, newName);
				}
			}
		} catch (final Exception e) {
			context.getLogger()
					.logException(
							fCu.getElementName()
									+ " exception in transform for class ", e);
		}
		return true;
	}

	//
	//
	//

	private class SearchFieldWithForibbidenName extends ASTVisitor {

		private FieldDeclaration first = null;

		@Override
		public boolean visit(FieldDeclaration node) {

			final VariableDeclarationFragment fragment = (VariableDeclarationFragment) node
					.fragments().get(0);

			final TargetField tField = context.getModel().findFieldMapping(fragment.getName().getIdentifier(),
					fragment.resolveBinding().getJavaElement()
							.getHandleIdentifier());

			final String oldName = fragment.getName().getIdentifier();
			final String newName = context.getModel().getVariable(oldName);
			if (newName != null
					&& (tField == null || (tField.getName() == null))) {
				firstField = first = node;
				final ITypeBinding tb = fragment.resolveBinding()
						.getDeclaringClass();
				final String packageName = tb.getPackage().getName();
				String className = tb.getName();
				ITypeBinding inner = tb;
				while (inner != null && inner.isMember()) {
					final ITypeBinding outerClass = inner.getDeclaringClass();
					String outerName = outerClass.getName();
					if (outerClass.getTypeArguments() != null
							&& outerClass.getTypeArguments().length > 0) {
						outerName += "<";
						for (int i = 0; i < outerClass.getTypeArguments().length; i++) {
							final ITypeBinding typeArg = outerClass
									.getTypeArguments()[i];
							outerName += typeArg.getName();
							if (i < outerClass.getTypeArguments().length - 1)
								outerName += ",";
						}
						outerName += ">";
					} else if (outerClass.getTypeParameters() != null
							&& outerClass.getTypeParameters().length > 0) {
						outerName += "<";
						for (int i = 0; i < outerClass.getTypeParameters().length; i++) {
							@SuppressWarnings("unused")
							final ITypeBinding typeArg = outerClass
									.getTypeParameters()[i];
							outerName += "%" + (i + 1); // typeArg.getName();
							if (i < outerClass.getTypeParameters().length - 1)
								outerName += ",";
						}
						outerName += ">";
					}
					className = outerName + "." + className;
					inner = outerClass;
				}
				context.getModel().addImplicitFieldRename(packageName,
						className, oldName, newName);
			}

			//
			return false;
		}

		public FieldDeclaration getFirstNested() {
			return first;
		}
	}
}
