package com.ilog.translator.java2cs.translation.astrewriter;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

import org.eclipse.core.runtime.CoreException;
import org.eclipse.core.runtime.IProgressMonitor;
import org.eclipse.jdt.core.IJavaProject;
import org.eclipse.jdt.core.dom.AST;
import org.eclipse.jdt.core.dom.ASTNode;
import org.eclipse.jdt.core.dom.CompilationUnit;
import org.eclipse.jdt.core.refactoring.descriptors.JavaRefactoringDescriptor;
import org.eclipse.jdt.internal.corext.refactoring.JDTRefactoringDescriptorComment;
import org.eclipse.jdt.internal.corext.refactoring.changes.CompilationUnitChange;
import org.eclipse.ltk.core.refactoring.Change;
import org.eclipse.ltk.core.refactoring.RefactoringChangeDescriptor;
import org.eclipse.ltk.core.refactoring.RefactoringDescriptor;
import org.eclipse.text.edits.MultiTextEdit;
import org.eclipse.text.edits.TextEdit;
import org.eclipse.text.edits.TextEditGroup;

import com.ilog.translator.java2cs.translation.ITranslationContext;
import com.ilog.translator.java2cs.translation.TranslatorASTRewrite;
import com.ilog.translator.java2cs.translation.noderewriter.INodeRewriter;

public abstract class ASTRewriterVisitor extends TranslationVisitor {

	protected TranslatorASTRewrite currentRewriter;
	protected List<TextEdit> edits;
	private boolean runOnce = true;

	//
	//
	//

	public ASTRewriterVisitor(ITranslationContext context) {
		super(context);
		currentRewriter = null;
		edits = new ArrayList<TextEdit>();
	}

	//
	//
	//

	public TranslatorASTRewrite getRewriter() {
		return currentRewriter;
	}

	//
	//
	//

	@SuppressWarnings("unchecked")
	@Override
	public Change createChange(IProgressMonitor pm, Object param)
			throws CoreException {
		if (currentRewriter != null) {
			if (simulate) {
				return null;
			}

			final Map arguments = new HashMap();
			String project = null;
			final IJavaProject javaProject = fCu.getJavaProject();
			if (javaProject != null) {
				project = javaProject.getElementName();
			}
			final int flags = RefactoringDescriptor.STRUCTURAL_CHANGE
					| JavaRefactoringDescriptor.JAR_REFACTORING
					| JavaRefactoringDescriptor.JAR_SOURCE_ATTACHMENT;
			final String header = " header ";
			final JDTRefactoringDescriptorComment comment = new JDTRefactoringDescriptorComment(
					project, this, header);
			final String description = " Java 2 Cs Translator ";
			final JavaRefactoringDescriptor descriptor= 
				new JavaRefactoringDescriptor(description, project, description, 
						comment.asString(), arguments, flags){}; //REVIEW Unregistered ID!
			
			//final JavaRefactoringDescriptor descriptor = new MoveDescriptor(project, description, comment.asString(), 
			//		arguments, flags);
			RefactoringChangeDescriptor ch = new RefactoringChangeDescriptor(descriptor);
			CompilationUnitChange cuChange = new CompilationUnitChange("", fCu);
			MultiTextEdit multiEdit = new MultiTextEdit();
			cuChange.setEdit(multiEdit);
			final TextEdit rewriteEdit = currentRewriter.rewriteAST();
			multiEdit.addChild(rewriteEdit);
			if (edits.size() > 0) {
				for(TextEdit edit : edits) {				
					multiEdit.addChild(edit.copy());
				}
			}
			cuChange.setDescriptor(ch);
			return cuChange;
		}
		return null;
	}

	//
	//
	//

	protected boolean applyNodeRewriter(ASTNode node, INodeRewriter result,
			TextEditGroup description) {
		if (result != null) {
			try {
				result.setICompilationUnit(fCu);
				result.process(context, node, currentRewriter, null,
						description);
				return true;
			} catch (final Exception e) {
				e.printStackTrace();
				context.getLogger().logException("", e);
			}
		}
		return false;
	}

	//
	//
	//

	@Override
	public boolean transform(IProgressMonitor pm, ASTNode cunit) {
		if (currentRewriter == null) {
			final AST ast = cunit.getAST();
			currentRewriter = TranslatorASTRewrite.create(ast);
		}

		super.transform(pm, cunit);

		return true;
	}

	//
	//
	//
	
	
	public boolean needCompilable() {
		return true;	
	}

	@Override
	public final boolean runOnce() {
		return runOnce;
	}

	@Override
	public final boolean runAgain(CompilationUnit unit) {
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

	public boolean needRecovery() {
		return false;
	}

	//
	//
	//

	public void reset() {
		simulate = false;
		runOnce = true;
		currentRewriter = null;
	}

	//
	//
	//

	@Override
	public boolean needValidation() {
		return true;
	}

}
