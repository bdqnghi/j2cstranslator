package test;

import junit.framework.Assert;

import org.eclipse.core.resources.IWorkspace;
import org.eclipse.core.resources.IWorkspaceDescription;
import org.junit.Before;
import org.junit.Test;

import com.ilog.translator.java2cs.plugin.TranslationPlugin;
import com.ilog.translator.java2cs.translation.astrewriter.MoveInitializerToConstructorVisitor;

public class SimpleTest {

	IWorkspace workspace = null;
	IWorkspaceDescription wsDesc = null;
	String pathToTests = "C:/work/workspaces/workspaceGenerics/";

	@Before
	public void setUp() throws Exception {
		workspace = TranslationPlugin.getWorkspace();
		wsDesc = workspace.getDescription();
	}

	@Test
	public void testMoveInitializerToConstructor() {
		try {
			final TestContext testContext = new TestContext(workspace);
			testContext.init(
					"C:/work/workspaces/workspaceGenerics/InitializerTest/",
					"InitializerTest");
			//
			testContext
					.createICompilationUnit("tests/enums/InitializerTest.java");
			Assert.assertNotNull(testContext.getICompilationUnit());
			//
			testContext.createCompilationUnit();
			Assert.assertNotNull(testContext.getCompilationUnit());
			Assert.assertNotNull(testContext.getBuffer());
			//
			final MoveInitializerToConstructorVisitor transformer = new MoveInitializerToConstructorVisitor(
					testContext.getContext());
			testContext.testTransformer(transformer);
			//
			Assert.assertNotSame(testContext.getBuffer(), testContext
					.getTransformedBuffer());
			//
			final String expected = "package tests.enums;public class InitializerTest {public InitializerTest(){int a=2;}}";
			//
			Assert.assertEquals(testContext.getTransformedBuffer(), expected);
			//
		} catch (final Exception e) {
			Assert.fail(e.getMessage() + " " + e.toString());
		}
	}

}
