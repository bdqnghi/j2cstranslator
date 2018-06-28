package com.ilog.translator.java2cs.translation.astrewriter.astchange;

import org.eclipse.core.runtime.CoreException;
import org.eclipse.core.runtime.IProgressMonitor;
import org.eclipse.core.runtime.NullProgressMonitor;
import org.eclipse.jdt.core.IType;
import org.eclipse.jdt.core.JavaModelException;
import org.eclipse.jdt.core.dom.CompilationUnit;
import org.eclipse.jdt.core.dom.EnumDeclaration;
import org.eclipse.jdt.core.dom.TypeDeclaration;
import org.eclipse.jdt.internal.corext.refactoring.rename.JavaRenameProcessor;
import org.eclipse.jdt.internal.corext.refactoring.rename.RenameTypeProcessor;
import org.eclipse.ltk.core.refactoring.Change;
import org.eclipse.ltk.core.refactoring.RefactoringStatus;
import org.eclipse.ltk.core.refactoring.participants.RenameRefactoring;
import org.eclipse.text.edits.TextEditGroup;

import com.ilog.translator.java2cs.configuration.info.ClassInfo;
import com.ilog.translator.java2cs.configuration.target.TargetClass;
import com.ilog.translator.java2cs.translation.ITranslationContext;
import com.ilog.translator.java2cs.translation.util.TranslationUtils;

public class RenameClassVisitor extends ASTChangeVisitor {

	protected boolean runOnce = true;
	protected boolean automaticallyRenameInterface = false;
	

	//
	//
	//

	public RenameClassVisitor(ITranslationContext context) {
		super(context);
		transformerName = "Rename class";
		description = new TextEditGroup(transformerName);
		automaticallyRenameInterface = context.getConfiguration().getOptions().isNormalizeInterfaceName();
	}

	//
	//
	//

	@Override
	public boolean runOnce() {
		return true;
	}

	@Override
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

	@Override
	public boolean needValidation() {
		return true;
	}

	//
	//
	//

	public Change createChange(IProgressMonitor pm, IType itype, TargetClass tc)
			throws JavaModelException, CoreException {
		JavaRenameProcessor processor = new RenameTypeProcessor(itype);
		RenameRefactoring refactoring = new RenameRefactoring(processor);
		processor.setNewElementName(tc.getShortName());

		final RefactoringStatus status = refactoring.checkAllConditions(pm);

		// TODO:
		// log that rename in order to add it to implicit renaming
		String packageName = itype.getPackageFragment().getElementName();
		String typeName = itype.getElementName();
		//
		this.context.getModel().addImplicitNestedRename(tc.getPackageName(),
				tc.getShortName(), packageName, typeName);
		//
		if (status.hasFatalError()) {
			System.err.println(status);
			return null;
		}
		return refactoring.createChange(pm);
	}

	//
	//
	//
	
	@Override
	public boolean visit(TypeDeclaration node) {
		try {
			final NullProgressMonitor pm = new NullProgressMonitor();
			TargetClass tc = context.getMapper().getTargetClass(node);
			if (tc != null) {
				change = this.createChange(pm, (IType) node.resolveBinding()
						.getJavaElement(), tc);
			} else if (node.isInterface() && context.getConfiguration().getOptions().isNormalizeInterfaceName()) {
				  // AUTOMATICALLY ADD 'I' FOR INTERFACES -->	        
              
              ClassInfo ci = this.context.getModel().findClassInfo(node.resolveBinding().getJavaElement().getHandleIdentifier(), 
                  true, false, TranslationUtils.isGeneric(node.resolveBinding()));

              String packageName = ((IType) node.resolveBinding().getJavaElement()).getPackageFragment().getElementName();
              String className = "I"+node.getName().getFullyQualifiedName();
              String targetFramework = context.getConfiguration().getOptions().getTargetDotNetFramework().name();
				
              tc =  new TargetClass(ci.getPackageInfo().getTarget(targetFramework).getName(), className, null, false, false, false, ci.isMember());
              tc.setRenamed(true);
              ci.addTargetClass(targetFramework, tc );              
              tc.setSourcePackageName(ci.getPackageName());
              
              context.getLogger().logInfo( "[Custom Info] Rename interface"+node.resolveBinding().getJavaElement().getElementName()+" -> "+className);
			  this.change = this.createChange(pm, (IType) node.resolveBinding().getJavaElement(), tc);
			  // AUTOMATICALLY ADD 'I' FOR INTERFACES <--
			}
		} catch (final Exception e) {
			e.printStackTrace();
			context.getLogger().logException("", e);
		}
		return true;
	}
	
	//
	//
	//
	
	@Override
	public boolean visit(EnumDeclaration node) {
		try {
			final NullProgressMonitor pm = new NullProgressMonitor();
			TargetClass tc = context.getMapper().getTargetClass(node);
			if (tc != null) {
				change = this.createChange(pm, (IType) node.resolveBinding()
						.getJavaElement(), tc);
			}
		} catch (final Exception e) {
			e.printStackTrace();
			context.getLogger().logException("", e);
		}
		return true;
	}
}
