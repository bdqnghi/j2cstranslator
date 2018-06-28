package com.ilog.translator.java2cs.translation.astrewriter;

import org.eclipse.core.runtime.CoreException;
import org.eclipse.core.runtime.NullProgressMonitor;
import org.eclipse.jdt.core.ICompilationUnit;
import org.eclipse.jdt.core.IJavaProject;
import org.eclipse.jdt.core.dom.AST;
import org.eclipse.jdt.core.dom.CompilationUnit;
import org.eclipse.jdt.core.dom.Modifier;
import org.eclipse.jdt.core.dom.TypeDeclaration;
import org.eclipse.jdt.internal.corext.codemanipulation.CodeGenerationSettings;
import org.eclipse.jdt.internal.corext.codemanipulation.OrganizeImportsOperation;
import org.eclipse.jdt.internal.ui.preferences.JavaPreferencesSettings;
import org.eclipse.text.edits.TextEditGroup;

import com.ilog.translator.java2cs.translation.ITranslationContext;
import com.ilog.translator.java2cs.translation.noderewriter.AddMissingMethodsRewriter;

/**
 * Add all methods not implemented in an abstract class.
 * 
 * @author afau
 *
 */
public class AbstractClassExplicitImplementMethodFromInterfaceVisitor extends
		ASTRewriterVisitor {

	//
	// 
	//
	//

	public AbstractClassExplicitImplementMethodFromInterfaceVisitor(
			ITranslationContext context) {
		super(context);
		transformerName = "Abstract Class Explicit Implement Methods From Interface";
		description = new TextEditGroup(transformerName);
	}

	//
	//
	//

	@Override
	public boolean isAbridged() {
		return true;
	}

	//
	//
	//

	@Override
	public void postAction(ICompilationUnit icunit, CompilationUnit unit2) {
		try {
			final NullProgressMonitor pm = new NullProgressMonitor();
			icunit.reconcile(AST.JLS3, false, null, pm);
			final IJavaProject project = fCu.getJavaProject();
			final CodeGenerationSettings settings = JavaPreferencesSettings
					.getCodeGenerationSettings(project);
			final ICompilationUnit cu = fCu;
			final boolean save = !cu.isWorkingCopy();
			final OrganizeImportsOperation op = new OrganizeImportsOperation(
					cu, null, settings.importIgnoreLowercase, save, true, null);
			op.run(new NullProgressMonitor());
		} catch (final CoreException e) {
			context.getLogger().logException(
					"Error during organize imports " + e.getMessage() + " "
							+ e.toString(), e);
		}
	}
	
	//
	//
	//

	@Override
	public void endVisit(TypeDeclaration node) {
		if (node.resolveBinding().isClass()
				&& Modifier.isAbstract(node.getModifiers())) {
			final AddMissingMethodsRewriter rewriter = new AddMissingMethodsRewriter();
			rewriter.setICompilationUnit(fCu);
			rewriter.process(context, node, currentRewriter, null, description);
		}
	}

}
