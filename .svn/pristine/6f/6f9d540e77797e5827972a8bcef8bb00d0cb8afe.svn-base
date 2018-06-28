/**
 * 
 */
package test;

import java.io.File;
import java.io.InputStream;
import java.util.Date;

import junit.framework.Assert;

import org.eclipse.core.resources.IWorkspace;
import org.eclipse.core.runtime.NullProgressMonitor;
import org.eclipse.jdt.core.ICompilationUnit;
import org.eclipse.jdt.core.IJavaProject;
import org.eclipse.jdt.core.dom.CompilationUnit;
import org.eclipse.jdt.internal.corext.dom.ASTNodes;

import com.ilog.translator.java2cs.configuration.GlobalOptions;
import com.ilog.translator.java2cs.configuration.TranslationConfiguration;
import com.ilog.translator.java2cs.configuration.TranslatorProjectOptions;
import com.ilog.translator.java2cs.translation.ITranslationContext;
import com.ilog.translator.java2cs.translation.Translator;
import com.ilog.translator.java2cs.translation.astrewriter.TranslationVisitor;

public class TestContext {
	IWorkspace workspace = null;

	IJavaProject javaProject = null;
	IJavaProject workingProject = null;
	ITranslationContext context = null;

	ICompilationUnit iCompilationUnit = null;
	CompilationUnit compilationUnit = null;

	String buffer = null;
	String transformedBuffer = null;

	NullProgressMonitor pm = new NullProgressMonitor();

	public TestContext(IWorkspace workspace) {
		this.workspace = workspace;
	}

	public void init(String pathToProject, String projectName) throws Exception {
		javaProject = TestUtil.importProject(pm, workspace, pathToProject,
				projectName);
		workingProject = TestUtil.copyProject(pm, workspace, javaProject);
		context = TestUtil
				.initializeTranslator(pm, javaProject, workingProject);
	}

	public void createICompilationUnit(String pathToCu) throws Exception {
		//
		iCompilationUnit = TestUtil.createICU(pm, workingProject, pathToCu);
	}

	public void createCompilationUnit() throws Exception {
		compilationUnit = TestUtil.createCU(workingProject, pm,
				iCompilationUnit);
		buffer = ASTNodes.asString(compilationUnit);
	}

	public IWorkspace getWorkspace() {
		return workspace;
	}

	public IJavaProject getJavaProject() {
		return javaProject;
	}

	public IJavaProject getWorkingProject() {
		return workingProject;
	}

	public ITranslationContext getContext() {
		return context;
	}

	public ICompilationUnit getICompilationUnit() {
		return iCompilationUnit;
	}

	public CompilationUnit getCompilationUnit() {
		return compilationUnit;
	}

	public String getBuffer() {
		return buffer;
	}

	public String getTransformedBuffer() {
		return transformedBuffer;
	}

	public void testAll() throws Exception {
		final TranslatorProjectOptions options = new TranslatorProjectOptions(
				new GlobalOptions());
		final TranslationConfiguration configuration = new TranslationConfiguration(
				javaProject, workingProject, options);
		configuration.init(options.getGlobalOptions()
				.getConfigurationFileName(), true);

		final String date = new Date().toString().replace(" ", "").replace(":",
				"");
		final String projectDir = "c:/temp/" + date + "/"
				+ javaProject.getElementName();
		options.setSourcesDestDir(projectDir);
		final Translator translator = new Translator(workingProject,
				configuration);
		options
				.getGlobalOptions()
				.setMappingAssemblyLocation(
						"C:/work/Repositories/.NET/Mapping/bin/Debug/ILOG.J2CsMapping.dll");
		options
				.setVSGenerationMode(TranslatorProjectOptions.VSGenerationPolicy.GENERATE);
		translator.translate(pm);
		// compilationUnit = TestUtil.createCU(workingProject, pm,
		// iCompilationUnit);
		// transformedBuffer = iCompilationUnit.getSource();
		try {
			boolean success = compile(projectDir);
			if (!success)
				Assert.fail(".NET compilation failed");
		} catch (final Exception e) {
			Assert.fail(".NET compilation failed / Exception : "
					+ e.getMessage());
		}
	}

	private boolean compile(String dir) throws Exception {
		final String[] cmdline = { "msbuild" };

		final Process p = Runtime.getRuntime().exec(cmdline, null,
				new File(dir + File.separator));
		//
		final StringBuilder result = new StringBuilder();
		final int exitValue = doWaitFor(p, result);
		if (exitValue != 0) {
			Assert.fail("Error : " + result.toString());
			return false;
		}
		return true;
	}

	public int doWaitFor(Process p, StringBuilder res) {
		int exitValue = -1; // returned to caller when p is finished
		try {
			final InputStream in = p.getInputStream();
			final InputStream err = p.getErrorStream();
			boolean finished = false; // Set to true when p is finished
			while (!finished) {
				try {
					while (in.available() > 0) {
						// Print the output of our system call
						final Character c = new Character((char) in.read());
						res.append(c);
					}
					while (err.available() > 0) {
						// Print the output of our system call
						final Character c = new Character((char) err.read());
						res.append(c);
					}
					// Ask the process for its exitValue. If the process
					// is not finished, an IllegalThreadStateException
					// is thrown. If it is finished, we fall through and
					// the variable finished is set to true.
					exitValue = p.exitValue();
					finished = true;
				} catch (final IllegalThreadStateException e) {

					// Process is not finished yet;
					// Sleep a little to save on CPU cycles
					Thread.sleep(500);
				}
			}
		} catch (final Exception e) {
			// unexpected exception! print it out for debugging...
			System.err.println("doWaitFor(): unexpected exception - "
					+ e.getMessage());
		}
		// return completion status to caller
		return exitValue;
	}

	public void testTransformer(TranslationVisitor transformer)
			throws Exception {
		compilationUnit = TestUtil.launchVisitor(pm, workingProject,
				iCompilationUnit, compilationUnit, transformer);
		transformedBuffer = ASTNodes.asString(compilationUnit);
	}
}