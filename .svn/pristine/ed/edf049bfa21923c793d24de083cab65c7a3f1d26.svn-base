package test;

import java.util.Date;

import org.eclipse.core.resources.IProject;
import org.eclipse.core.resources.IWorkspace;
import org.eclipse.core.runtime.CoreException;
import org.eclipse.core.runtime.IPath;
import org.eclipse.core.runtime.IProgressMonitor;
import org.eclipse.core.runtime.NullProgressMonitor;
import org.eclipse.core.runtime.Path;
import org.eclipse.core.runtime.SubProgressMonitor;
import org.eclipse.jdt.core.ICompilationUnit;
import org.eclipse.jdt.core.IJavaElement;
import org.eclipse.jdt.core.IJavaProject;
import org.eclipse.jdt.core.JavaCore;
import org.eclipse.jdt.core.dom.AST;
import org.eclipse.jdt.core.dom.ASTParser;
import org.eclipse.jdt.core.dom.CompilationUnit;

import com.ilog.translator.java2cs.configuration.GlobalOptions;
import com.ilog.translator.java2cs.configuration.TranslationConfiguration;
import com.ilog.translator.java2cs.configuration.TranslatorProjectOptions;
import com.ilog.translator.java2cs.plugin.util.ProjectImporter;
import com.ilog.translator.java2cs.translation.ITranslationContext;
import com.ilog.translator.java2cs.translation.TranslationContext;
import com.ilog.translator.java2cs.translation.astrewriter.TranslationVisitor;

public class TestUtil {

	public static IJavaProject copyProject(NullProgressMonitor pm,
			IWorkspace workspace, IJavaProject javaProject)
			throws CoreException {
		final String pName = "translation_"
				+ javaProject.getProject().getName() + "_"
				+ new Date().toString().replace(" ", "_").replace(":", "_");
		javaProject.getProject().copy(new Path(pName), false, pm);
		final IProject newP = workspace.getRoot().getProject(pName);
		return JavaCore.create(newP);
	}

	public static CompilationUnit launchVisitor(NullProgressMonitor pm,
			IJavaProject javaProject, ICompilationUnit cu, CompilationUnit c,
			TranslationVisitor visitor) throws CoreException {
		visitor.setCompilationUnit(cu);
		visitor.transform(pm, c);
		visitor.applyChange(pm);
		visitor.postAction(cu, c);
		visitor.reset();
		return TestUtil.createCU(javaProject, pm, cu);
	}

	public static ITranslationContext initializeTranslator(IProgressMonitor pm,
			IJavaProject javaProject, IJavaProject translateProject)
			throws Exception {
		final TranslatorProjectOptions options = new TranslatorProjectOptions(
				new GlobalOptions());
		final TranslationConfiguration configuration = new TranslationConfiguration(
				javaProject, translateProject, options);
		configuration.init(options.getGlobalOptions()
				.getConfigurationFileName(), true);
		final ITranslationContext context = new TranslationContext(javaProject,
				configuration);
		return context;
	}

	public static IJavaProject importProject(IProgressMonitor pm,
			IWorkspace workspace, String projectPath, String projectName)
			throws Exception {
		final IPath path = new Path(projectPath);
		final ProjectImporter importer = new ProjectImporter();
		importer.createExistingProject(path);
		final IProject project = workspace.getRoot().getProject(projectName);
		final IJavaProject javaProject = JavaCore.create(project);
		javaProject.open(pm);
		return javaProject;
	}

	public static ICompilationUnit createICU(IProgressMonitor pm,
			IJavaProject javaProject, String classPath) throws Exception {
		final IJavaElement clazz = javaProject.findElement(new Path(classPath));
		final ICompilationUnit cu = (ICompilationUnit) clazz;
		cu.open(pm);
		return cu;
	}

	public static CompilationUnit createCU(IJavaProject project,
			IProgressMonitor pm, String buffer) {
		return parse(project, pm, buffer, true, false, true);
	}

	public static CompilationUnit createCU(IJavaProject project,
			IProgressMonitor pm, ICompilationUnit icu) {
		return parse(pm, icu, true, false, true);
	}

	public static CompilationUnit parse(IJavaProject project,
			IProgressMonitor pm, String buffer, boolean validation,
			boolean abridged, boolean recovery) {
		final IProgressMonitor monitor = new SubProgressMonitor(pm, 1);
		final ASTParser parser = ASTParser.newParser(AST.JLS3);
		parser.setProject(project);
		parser.setSource(buffer.toCharArray());
		if (abridged) {
			parser.setFocalPosition(0);
		}
		parser.setResolveBindings(validation);
		parser.setStatementsRecovery(recovery);

		final CompilationUnit result = (CompilationUnit) parser
				.createAST(monitor);
		monitor.done();
		return result;
	}

	public static CompilationUnit parse(IProgressMonitor pm,
			ICompilationUnit icunit, boolean validation, boolean abridged,
			boolean recovery) {
		final IProgressMonitor monitor = new SubProgressMonitor(pm, 1);
		final ASTParser parser = ASTParser.newParser(AST.JLS3);
		parser.setProject(icunit.getJavaProject());
		parser.setSource(icunit);
		if (abridged) {
			parser.setFocalPosition(0);
		}
		parser.setResolveBindings(validation);
		parser.setStatementsRecovery(recovery);
		// 
		/*
		 * Map options = JavaCore.getOptions();
		 * options.put("org.eclipse.jdt.core.compiler.doc.comment.support",
		 * "enable"); parser.setCompilerOptions(options);
		 */
		//
		final CompilationUnit result = (CompilationUnit) parser
				.createAST(monitor);
		monitor.done();
		return result;
		/*
		 * return ASTProvider.getASTProvider().getAST(icunit,
		 * ASTProvider.WAIT_YES , monitor);
		 */
	}
}
