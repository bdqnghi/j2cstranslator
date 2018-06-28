package com.ilog.translator.java2cs.translation;

import java.io.File;
import java.io.FileReader;
import java.io.IOException;
import java.io.PrintWriter;
import java.io.Reader;
import java.io.Writer;
import java.text.DecimalFormat;
import java.util.ArrayList;
import java.util.Collections;
import java.util.GregorianCalendar;
import java.util.HashMap;
import java.util.Hashtable;
import java.util.List;
import java.util.Map;
import java.util.regex.Pattern;

import org.eclipse.core.filebuffers.FileBuffers;
import org.eclipse.core.filebuffers.ITextFileBuffer;
import org.eclipse.core.filebuffers.ITextFileBufferManager;
import org.eclipse.core.resources.IFile;
import org.eclipse.core.resources.IFolder;
import org.eclipse.core.resources.IResource;
import org.eclipse.core.runtime.CoreException;
import org.eclipse.core.runtime.IPath;
import org.eclipse.core.runtime.IProgressMonitor;
import org.eclipse.core.runtime.NullProgressMonitor;
import org.eclipse.core.runtime.OperationCanceledException;
import org.eclipse.core.runtime.SubProgressMonitor;
import org.eclipse.jdt.core.ICompilationUnit;
import org.eclipse.jdt.core.IJavaElement;
import org.eclipse.jdt.core.IJavaProject;
import org.eclipse.jdt.core.IType;
import org.eclipse.jdt.core.ITypeHierarchy;
import org.eclipse.jdt.core.JavaCore;
import org.eclipse.jdt.core.JavaModelException;
import org.eclipse.jdt.core.dom.AST;
import org.eclipse.jdt.core.dom.ASTNode;
import org.eclipse.jdt.core.dom.ASTParser;
import org.eclipse.jdt.core.dom.AbstractTypeDeclaration;
import org.eclipse.jdt.core.dom.CompilationUnit;
import org.eclipse.jdt.core.dom.ITypeBinding;
import org.eclipse.jdt.core.dom.Message;
import org.eclipse.jdt.core.dom.Type;
import org.eclipse.jdt.core.dom.TypeDeclaration;
import org.eclipse.jdt.core.formatter.DefaultCodeFormatterConstants;
import org.eclipse.jdt.internal.core.PackageFragment;
import org.eclipse.jdt.internal.core.PackageFragmentRoot;
import org.eclipse.jdt.internal.corext.codemanipulation.CodeGenerationSettings;
import org.eclipse.jdt.internal.corext.codemanipulation.OrganizeImportsOperation;
import org.eclipse.jdt.internal.corext.util.JavaModelUtil;
import org.eclipse.jdt.internal.ui.actions.ActionMessages;
import org.eclipse.jdt.internal.ui.preferences.JavaPreferencesSettings;
import org.eclipse.jdt.internal.ui.text.java.JavaFormattingContext;
import org.eclipse.jdt.internal.ui.text.java.JavaFormattingStrategy;
import org.eclipse.jdt.ui.text.IJavaPartitions;
import org.eclipse.jface.text.DocumentRewriteSession;
import org.eclipse.jface.text.DocumentRewriteSessionType;
import org.eclipse.jface.text.IDocument;
import org.eclipse.jface.text.IDocumentExtension;
import org.eclipse.jface.text.IDocumentExtension4;
import org.eclipse.jface.text.formatter.FormattingContextProperties;
import org.eclipse.jface.text.formatter.IFormattingContext;
import org.eclipse.jface.text.formatter.MultiPassContentFormatter;
import org.eclipse.ltk.core.refactoring.Change;
import org.eclipse.ltk.core.refactoring.RefactoringStatus;

import com.ilog.translator.java2cs.configuration.Logger;
import com.ilog.translator.java2cs.configuration.Pass;
import com.ilog.translator.java2cs.configuration.ResourceProcessor;
import com.ilog.translator.java2cs.configuration.TranslationConfiguration;
import com.ilog.translator.java2cs.configuration.TranslatorProjectOptions;
import com.ilog.translator.java2cs.configuration.info.TranslationModelException;
import com.ilog.translator.java2cs.translation.astrewriter.CovarianceChangeVisitor;
import com.ilog.translator.java2cs.translation.astrewriter.CovarianceFinderVisitor;
import com.ilog.translator.java2cs.translation.astrewriter.CovarianceFinderVisitor2;
import com.ilog.translator.java2cs.translation.astrewriter.CovarianceManualFinderVisitor;
import com.ilog.translator.java2cs.translation.astrewriter.RemoveMethodBodyVisitor;
import com.ilog.translator.java2cs.translation.astrewriter.UpdateHandlerTagVisitor;
import com.ilog.translator.java2cs.translation.astrewriter.astchange.RenameClassVisitor;
import com.ilog.translator.java2cs.translation.astrewriter.astchange.SearchForEnclosingAccess;
import com.ilog.translator.java2cs.translation.util.TranslationUtils;
import com.ilog.translator.java2cs.util.CSharpModelUtil;

public class Translator implements ITranslator {

	private static class DefaultProcessor implements ResourceProcessor {
		public boolean process(Reader reader, Writer writer) throws IOException {
			int c = -1;
			while ((c = reader.read()) != -1) {
				writer.write(c);
			}
			return true;
		}

		public String processFilename(String filename) {
			return filename;
		}
	}

	//
	//
	//

	private static final String PASS_1 = "Propagate data"; // JDT refactoring
	private static final String PASS_2 = "preCollect"; // JDT refactoring
	private static final String PASS_3 = "astRefactoring"; // JDT refactoring
	private static final String PASS_4 = "Collect data"; // JDT refactoring
	private static final String PASS_6 = "ast2Refactoring"; // JDT refactoring
	private static final String PASS_7 = "collectAndFormat"; // Collect and
	// format
	private static final String PASS_8 = "astReplacement"; // C# AST
	private static final String PASS_9 = "astTextReplacement"; // C# AST
	private static final String PASS_10 = "textReplacement"; // C# Text
	private static final String PASS_11 = "save"; // Saving

	//
	//
	//
	private static final boolean DEBUG = false;

	private ASTParser parser = ASTParser.newParser(AST.JLS3);

	private static ResourceProcessor defaultProcessor = new DefaultProcessor();
	protected Map<String, Pass> passes = new HashMap<String, Pass>();
	private DocumentRewriteSession fRewriteSession;
	private ITranslationContext context;
	private List<ICompilationUnit> computedCU = null;

	//
	// Constructor
	//

	public Translator(IJavaProject javaProject,
			TranslationConfiguration configuration) {
		context = new TranslationContext(javaProject, configuration);
		createTransformers();
	}

	//
	// finalize
	//

	@Override
	public void finalize() {
		context = null;
		passes = null;
		fRewriteSession = null;
		computedCU = null;
		parser = null;
	}

	//
	// Main translation method
	//

	public boolean translate(IProgressMonitor monitor) throws CoreException,
			IOException {
		if (context.getConfiguration().getWorkingProject() != null) {
			try {
				monitor.beginTask("Translation", 80);
				final IProgressMonitor subMonitor = new NullProgressMonitor();
				//
				// Compute compilation unit
				//
				final long startTime = beginTask(monitor,
						"Compute Compilation Units");
				final long startFreeMemory = Runtime.getRuntime().freeMemory();
				final List<ICompilationUnit> children = getCompilationUnits(true);
				//
				if (context.getConfiguration().getOptions()
						.collectPublicDocumentedClass())
					checkPackageDotHtmlFiles();
				//
				if (children.size() == 0) {
					context
							.getLogger()
							.logWarning(
									"Project have zero source file. Check your project or filters.");
					return false;
				}

				endTask(monitor, 3);
				//
				context.getLogger().logInfo(
						"Translation of " + children.size() + " files "
								+ printMemory(startFreeMemory, true));

				final boolean isSimulation = context.getConfiguration()
						.getOptions().isSimulation();
				if (isSimulation) {
					context.getLogger().logInfo("***********************");
					context.getLogger().logInfo("*** SIMULATION MODE ***");
					context.getLogger().logInfo("***********************");
				}
				long previousFreeMemory = startFreeMemory;
				// Propagate - 1
				previousFreeMemory = launchPass(monitor, Translator.PASS_1,
						children, previousFreeMemory, true,
						new MeasurableCode() {
							public void run(List<ITransformer> transformers)
									throws CoreException {
								propagate(subMonitor,
										getCompilationUnits(false));
							}
						});
				// Pre Collect - 2
				previousFreeMemory = launchPass(monitor, Translator.PASS_2,
						children, previousFreeMemory, true,
						new MeasurableCode() {
							public void run(List<ITransformer> transformers)
									throws CoreException {
								preCollectData(subMonitor,
										getCompilationUnits(false), false);
								// format
								format(subMonitor, getCompilationUnits(true));
							}
						});
				// Java compatible refactoring 3
				previousFreeMemory = launchPass(monitor, Translator.PASS_3,
						children, previousFreeMemory, true,
						new MeasurableCode() {
							public void run(List<ITransformer> transformers)
									throws CoreException {
								launchTransformersOnChildren(subMonitor,
										getCompilationUnits(false),
										transformers, "AST Refactoring");
								if (!isSimulation) {
									List<ICompilationUnit> newChilds = getNewCompilationUnits(getCompilationUnits(true));
									while (newChilds.size() > 0) {
										context
												.getLogger()
												.logInfo(
														"   ("
																+ newChilds
																		.size()
																+ " new child found, relaunch AST Refactoring)");
										launchTransformersOnChildren(
												subMonitor,
												getCompilationUnits(false),
												transformers, "AST Refactoring");
										newChilds = getNewCompilationUnits(getCompilationUnits(true));
									}
									newChilds = null;
								}
								// TODO: not efficient but it's work !
								// to be removed ??? We do the exact same thing
								// in "collect"
								preCollectData(subMonitor,
										getCompilationUnits(true), true);
							}
						});

				// collect - 4
				previousFreeMemory = launchPass(monitor, Translator.PASS_4,
						children, previousFreeMemory, true,
						new MeasurableCode() {
							public void run(List<ITransformer> transformers)
									throws CoreException {
								preCollectData(subMonitor,
										getCompilationUnits(false), false);
							}
						});
				// Java compatible transformation - 6
				previousFreeMemory = launchPass(monitor, Translator.PASS_6,
						children, previousFreeMemory, !isSimulation,
						new MeasurableCode() {
							public void run(List<ITransformer> transformers)
									throws CoreException {
								launchTransformersOnChildren(subMonitor,
										getCompilationUnits(false),
										transformers, "AST Transformation");
							}
						});
				// Collect data and Format - 7
				previousFreeMemory = launchPass(monitor, Translator.PASS_7,
						children, previousFreeMemory, !isSimulation,
						new MeasurableCode() {
							public void run(List<ITransformer> transformers)
									throws CoreException {
								collectData(subMonitor,
										getCompilationUnits(true));

							}
						});
				// AST transformation to C#, pass 1 - 8
				previousFreeMemory = launchPass(monitor, Translator.PASS_8,
						children, previousFreeMemory, !isSimulation,
						new MeasurableCode() {
							public void run(List<ITransformer> transformers)
									throws CoreException {
								launchASTNodesReplacementTransformers(
										subMonitor, getCompilationUnits(false),
										transformers);
							}
						});
				// AST transformation to C#, pass 2 - 9
				previousFreeMemory = launchPass(monitor, Translator.PASS_9,
						children, previousFreeMemory, !isSimulation,
						new MeasurableCode() {
							public void run(List<ITransformer> transformers)
									throws CoreException {
								launchChildrenOnTransformers(subMonitor,
										getCompilationUnits(false),
										transformers, "AST Text Replacement");
							}
						});
				// Text transformation to C# - 10
				previousFreeMemory = launchPass(monitor, Translator.PASS_10,
						children, previousFreeMemory, !isSimulation,
						new MeasurableCode() {
							public void run(List<ITransformer> transformers)
									throws CoreException {
								launchChildrenOnTransformers(subMonitor,
										getCompilationUnits(false),
										transformers, "Text Replacement");
							}
						});
				// Save - 11
				previousFreeMemory = launchPass(monitor, Translator.PASS_11,
						children, previousFreeMemory, !isSimulation,
						new MeasurableCode() {
							public void run(List<ITransformer> transformers)
									throws CoreException {
								save(subMonitor, getCompilationUnits(false));
								copyResources(subMonitor);
							}
						});
				//
				final long end = System.nanoTime();
				final long time = (end - startTime) / 1000000000;
				final int nbChildren = children.size();
				context.getLogger().logInfo(
						"Project done in " + getDuration(startTime, end));
				context.getLogger().logInfo(
						"  average of " + (time / nbChildren) + "s per files ("
								+ nbChildren + " files) "
								+ printMemory(startFreeMemory, true) + ".");

				context.getModel().generateImplicitMappingFile(true /* !!!! */);
				if (context.getConfiguration().getOptions()
						.collectPublicDocumentedClass())
					context.listPublicDocumentedFiles();
			} catch (final JavaModelException e) {
				e.printStackTrace();
				context.getLogger().logException("", e);
				return false;
			} catch (final Exception e) {
				context.getLogger().logException(
						"Unable to generate configuration file", e);
			}
			return true;
		}
		monitor.done();
		return false;
	}

	//
	//
	//

	private static interface MeasurableCode {
		public void run(List<ITransformer> transformers) throws CoreException;
	}

	//
	//
	//

	private long launchPass(IProgressMonitor monitor, String passId,
			List<ICompilationUnit> children, long previousFreeMemory,
			boolean condition, MeasurableCode code) throws CoreException {
		final Pass pass = passes.get(passId);
		if (pass != null && pass.getLaunch() && condition) {
			final long previousTime = beginTask(monitor, "Starting Step <"
					+ pass.getDescription() + ">");
			final List<ITransformer> transformers = pass
					.getInstantiatedTransformers();
			code.run(transformers);
			final long time = endTask(monitor, 7);
			printStats(previousTime, previousFreeMemory, time);
		}
		return Runtime.getRuntime().freeMemory();
	}

	//
	// getDuration
	//

	public static String getDuration(long timestamp, long endTimestamp) {
		if (timestamp != 0 && endTimestamp != 0) {
			try {
				final long duration = (endTimestamp - timestamp) / 1000000;
				// final SimpleDateFormat format = new SimpleDateFormat(
				// "H'h' m'm' s's' SSS'ms'");
				// format.setTimeZone(TimeZone.getTimeZone("GMT"));
				// return format.format(new Date(duration));
				return stringify(duration);
			} catch (final RuntimeException e) {
				e.printStackTrace();
			}
		}

		return "";
	}

	private static String stringify(long duration) {
		double denominator = 1000.0 * 60.0 * 60.0 * 24.0;

		int days = 0;
		int remainder = (int) duration;
		int hours = 0;
		int minutes = 0;
		int seconds = 0;
		int milliseconds = 0;

		days = (int) (duration / denominator);
		if (days > 0) {
			remainder = (int) (duration - (days * denominator));
		}

		hours = (int) remainder / (1000 * 60 * 60);
		if (hours > 0) {
			remainder -= (hours * 1000 * 60 * 60);
		}

		minutes = (int) remainder / (1000 * 60);
		if (minutes > 0) {
			remainder -= (minutes * 1000 * 60);
		}

		seconds = (int) remainder / (1000);
		if (seconds > 0) {
			remainder -= (seconds * 1000);
		}

		milliseconds = remainder;

		StringBuffer result = new StringBuffer();
		if (days > 0) {
			result.append(days + " day(s) ");
		}
		if (hours > 0) {
			result.append(hours + "h ");
		}
		if (minutes > 0) {
			result.append(minutes + "m ");
		}
		if (seconds > 0) {
			result.append(seconds + "s ");
		}
		if (milliseconds > 0) {
			result.append(milliseconds + "ms ");
		}

		return result.toString();
	}

	private long printStats(long previousTime, long previousFreeMemory,
			long stepTime) {
		context.getLogger().logInfo(
				"   done in " + getDuration(previousTime, stepTime) + " "
						+ printMemory(previousFreeMemory, false) + ".");
		return stepTime;
	}

	private long beginTask(IProgressMonitor monitor, String msg) {
		monitor.subTask(msg); // 3
		context.getLogger().logInfo(msg);
		return System.nanoTime();
	}

	private long endTask(IProgressMonitor monitor, int count) {
		monitor.worked(count);
		return System.nanoTime();
	}

	//
	// Print Memory
	//

	public static String printMemory(long oldFreeMemory, boolean full) {
		final long freeMemory = Runtime.getRuntime().freeMemory();
		final long maxMemory = Runtime.getRuntime().maxMemory();
		final long totalMemory = Runtime.getRuntime().totalMemory();
		final double units = 1000000.0;

		final DecimalFormat df = (DecimalFormat) DecimalFormat.getInstance();
		df.applyPattern("# ##0.00");
		if (full) {
			return "[ Memory: delta = "
					+ df.format((freeMemory - oldFreeMemory) / units)
					+ "Mb / free = " + df.format(freeMemory / units)
					+ "Mb / max = " + df.format(maxMemory / units)
					+ "Mb / total = " + df.format(totalMemory / units) + "Mb ]";
		} else {
			return "[ Memory: delta = "
					+ df.format((freeMemory - oldFreeMemory) / units)
					+ "Mb / free = " + df.format(freeMemory / units) + "Mb ]";
		}
	}

	//
	// formatting
	//

	@SuppressWarnings( { "unchecked", "deprecation" })
	private void doRunOnMultiple(List<ICompilationUnit> cus,
			IProgressMonitor monitor) throws OperationCanceledException {
		if (monitor == null) {
			monitor = new NullProgressMonitor();
		}
		monitor.setTaskName(ActionMessages.FormatAllAction_description);

		monitor.beginTask("", cus.size() * 4); //$NON-NLS-1$
		try {
			Map lastOptions = null;
			IJavaProject lastProject = null;

			for (int i = 0; i < cus.size(); i++) {
				final ICompilationUnit cu = cus.get(i);
				final IPath path = cu.getPath();
				if (lastProject == null
						|| !lastProject.equals(cu.getJavaProject())) {
					lastProject = cu.getJavaProject();
					lastOptions = Translator.getFomatterSettings(lastProject);
				}

				if (monitor.isCanceled()) {
					throw new OperationCanceledException();
				}

				final ITextFileBufferManager manager = FileBuffers
						.getTextFileBufferManager();
				try {
					try {
						manager.connect(path,
								new SubProgressMonitor(monitor, 1));

						monitor.subTask(path.makeRelative().toString());
						final ITextFileBuffer fileBuffer = manager
								.getTextFileBuffer(path);

						formatCompilationUnit(fileBuffer, lastOptions);

						if (fileBuffer.isDirty() && !fileBuffer.isShared()) {
							fileBuffer.commit(
									new SubProgressMonitor(monitor, 2), false);
						} else {
							monitor.worked(2);
						}
					} finally {
						manager.disconnect(path, new SubProgressMonitor(
								monitor, 1));
					}
				} catch (final CoreException e) {
					// status.add(e.getStatus());
				}
			}
		} catch (final Exception e) {
			context.getLogger().logException("Problem during formating", e);
		} finally {
			monitor.done();
		}
	}

	@SuppressWarnings("unchecked")
	private void formatCompilationUnit(final ITextFileBuffer fileBuffer,
			final Map options) {
		if (fileBuffer.isShared()) {
			// do nothing
		} else {
			doFormat(fileBuffer.getDocument(), options);
		}
	}

	@SuppressWarnings("unchecked")
	private void doFormat(IDocument document, Map options) {
		final IFormattingContext context = new JavaFormattingContext();
		try {
			context.setProperty(
					FormattingContextProperties.CONTEXT_PREFERENCES, options);
			context.setProperty(FormattingContextProperties.CONTEXT_DOCUMENT,
					true);
			final MultiPassContentFormatter formatter = new MultiPassContentFormatter(
					IJavaPartitions.JAVA_PARTITIONING,
					IDocument.DEFAULT_CONTENT_TYPE);

			JavaFormattingStrategy strategy = new JavaFormattingStrategy();
			Hashtable currentOptions = JavaCore.getOptions();
			currentOptions
					.put(
							DefaultCodeFormatterConstants.FORMATTER_COMMENT_FORMAT_BLOCK_COMMENT,
							DefaultCodeFormatterConstants.FALSE);
			currentOptions
					.put(
							DefaultCodeFormatterConstants.FORMATTER_COMMENT_FORMAT_LINE_COMMENT,
							DefaultCodeFormatterConstants.FALSE);
			currentOptions
					.put(
							DefaultCodeFormatterConstants.FORMATTER_COMMENT_FORMAT_JAVADOC_COMMENT,
							DefaultCodeFormatterConstants.FALSE);
			JavaCore.setOptions(currentOptions);
			formatter.setMasterStrategy(strategy);

			try {
				startSequentialRewriteMode(document);
				formatter.format(document, context);
			} finally {
				stopSequentialRewriteMode(document);
			}
		} finally {
			context.dispose();
		}
	}

	@SuppressWarnings("deprecation")
	private void startSequentialRewriteMode(IDocument document) {
		if (document instanceof IDocumentExtension4) {
			final IDocumentExtension4 extension = (IDocumentExtension4) document;
			fRewriteSession = extension
					.startRewriteSession(DocumentRewriteSessionType.SEQUENTIAL);
		} else if (document instanceof IDocumentExtension) {
			final IDocumentExtension extension = (IDocumentExtension) document;
			extension.startSequentialRewrite(false);
		}
	}

	@SuppressWarnings("deprecation")
	private void stopSequentialRewriteMode(IDocument document) {
		if (document instanceof IDocumentExtension4) {
			final IDocumentExtension4 extension = (IDocumentExtension4) document;
			extension.stopRewriteSession(fRewriteSession);
		} else if (document instanceof IDocumentExtension) {
			final IDocumentExtension extension = (IDocumentExtension) document;
			extension.stopSequentialRewrite();
		}
	}

	@SuppressWarnings("unchecked")
	private static Map getFomatterSettings(IJavaProject project) {
		return new HashMap(project.getOptions(true));
	}

	private void format(IProgressMonitor monitor,
			List<ICompilationUnit> children) {
		doRunOnMultiple(children, monitor);
	}

	//
	// Save
	//

	private void save(IProgressMonitor monitor, List<ICompilationUnit> children) {
		for (int i = 0; i < children.size(); i++) {
			final ICompilationUnit icunit = children.get(i);
			try {
				if (!context.ignore(icunit)) {
					final long localstart = System.nanoTime();
					this.save(monitor, icunit);
					final long localend = System.nanoTime();
					monitor.subTask("Saved class " + icunit.getElementName()
							+ " in " + getDuration(localstart, localend) + ".");
				} else {
					context.getLogger().logInfo(
							"Ignoring class " + icunit.getElementName());
				}
			} catch (final Exception e) {
				e.printStackTrace();
				context.getLogger().logException(
						"Error during saving file " + icunit.getElementName(),
						e);
			}
		}
		try {
			context.getConfiguration().generateVSProjectFile(context, children);
		} catch (final Exception e) {
			e.printStackTrace();
			context.getLogger().logException(
					"Error during generating VS Project", e);
		}
	}

	private void save(IProgressMonitor pm, ICompilationUnit icunit)
			throws Exception {
		final String path = TranslationUtils.computePathForSource(context,
				icunit);

		final File directory = new File(path);
		if (!directory.exists()) {
			if (!directory.mkdirs()) {
				context.getLogger().logError("Can't create directory " + path);
				return;
			}
		}

		String suffix = context.getConfiguration().getOptions()
				.getSuffixForGenerated();
		if (suffix == null)
			suffix = "";

		final String javafilename = icunit.getElementName();
		final String csfilename = javafilename.replace(
				JavaModelUtil.DEFAULT_CU_SUFFIX, suffix
						+ CSharpModelUtil.DEFAULT_CU_SUFFIX);

		if (context.getConfiguration().getOptions()
				.getSourcesReplacementPolicy() == TranslatorProjectOptions.SourcesReplacementPolicy.DELETE) {
			final File csFile = new File(path + File.separator + csfilename);
			if (csFile.exists()) {
				if (!csFile.delete()) {
					context.getLogger().logError("Can't delete file " + csFile);
				}
			}
		}
		if (context.getConfiguration().getOptions().getGlobalOptions()
				.isDebug())
			context.getLogger().logInfo(
					"Saving file " + path + File.separator
							+ icunit.getElementName());
		final Writer writer = new PrintWriter(path + File.separator
				+ csfilename);
		writer.write(icunit.getBuffer().getCharacters());
		writer.close();
	}

	//
	// resources
	//

	public void copyResources(IProgressMonitor pm) {
		final String destDir = context.getConfiguration().getOptions()
				.getResourcesDestDir();
		final String filterInclude = context.getConfiguration().getOptions()
				.getResourcesIncludePattern();
		final String filterExclude = context.getConfiguration().getOptions()
				.getResourcesExcludePattern();
		if (destDir != null && (filterExclude != null || filterInclude != null)) {
			final IJavaProject project = context.getConfiguration()
					.getWorkingProject();
			try {
				final IResource[] elements = project.getProject().members(true);
				for (final IResource res : elements) {
					if (res instanceof IFile) {
						copyResource(filterInclude, filterExclude, (IFile) res,
								destDir);
					} else if (res instanceof IFolder) {
						copyFolder(filterInclude, filterExclude, (IFolder) res,
								destDir);
					}
				}
			} catch (final Exception e) {
				context.getLogger().logWarning(
						"Problem during copy resources " + e.getMessage() + " "
								+ e.toString());
			}
		}
	}

	private void copyFolder(String filterInclude, String filterExclude,
			IFolder folder, String destDir) throws CoreException, IOException {
		final IResource[] resources = folder.members();
		final String folderName = folder.getName();
		for (final Object res : resources) {
			if (res instanceof IFile) {
				copyResource(filterInclude, filterExclude, (IFile) res, destDir
						+ File.separator + folderName);
			} else if (res instanceof IFolder) {
				copyFolder(filterInclude, filterExclude, (IFolder) res, destDir
						+ File.separator + folderName);
			}
		}
	}

	private void copyResource(String filterInclude, String filterExclude,
			IFile file, String destDir) throws CoreException, IOException {
		final IPath fullPath = file.getFullPath();
		final IPath relative = file.getParent().getProjectRelativePath();
		final String path = fullPath.toString();

		if (filterOk(path, filterInclude, filterExclude)) {
			final IPath loc = file.getLocation();
			// String relativePath = "";
			final String dirName = destDir + File.separator; /*
															 * relativePath +
															 * File.separator;
															 */
			if (relative != null) {
				// relativePath = relative.toString();
				File f = new File(dirName);
				if (!f.exists()) {
					if (!f.mkdirs()) {
						context.getLogger().logError(
								"Can't create directory " + dirName);
					}
				}
				f = new File(dirName + file.getName());
				if (f.exists()
						&& context.getConfiguration().getOptions()
								.getResourcesCopyPolicy() != TranslatorProjectOptions.ResourcesCopyPolicy.OVERRIDE_POLICY)
					// skip
					return;
			}

			for (final String filter : context.getConfiguration()
					.getTranslationDescriptor().getResourcesProcessors()
					.keySet()) {
				ResourceProcessor processor = null;
				if (file.getName().endsWith(filter)) {
					processor = context.getConfiguration()
							.getTranslationDescriptor()
							.getResourcesProcessors().get(filter);
				} else {
					processor = defaultProcessor;
				}

				context.getLogger().logInfo(
						"Processing resource " + loc.toString());

				final Reader reader = new FileReader(loc.toString());
				final Writer writer = new PrintWriter(dirName
						+ processor.processFilename(file.getName()));
				processor.process(reader, writer);
				reader.close();
				writer.close();
			}
		}
	}

	private boolean filterOk(String path, String inclPattern, String exclPattern) {
		return match(path, inclPattern, true)
				&& !match(path, exclPattern, false);
	}

	private boolean match(String path, String regex, boolean defValue) {
		if (regex == null)
			return defValue;
		return Pattern.matches(regex, path);
	}

	//
	// propagate mapping to intermediate classes
	//

	private void propagate(IProgressMonitor monitor,
			List<ICompilationUnit> children) {
		// First, Create dummy ClassInfo for all IType
		for (final ICompilationUnit icunit : children) {
			final CompilationUnit cunit = parse(monitor, icunit, true, false,
					false);
			final int size = cunit.types().size();
			for (int i = 0; i < size; i++) {
				final AbstractTypeDeclaration decl = (AbstractTypeDeclaration) cunit
						.types().get(i);
				if (decl.getNodeType() == ASTNode.TYPE_DECLARATION) {
					final TypeDeclaration typeDecl = (TypeDeclaration) decl;
					context.buildClassInfoAndScanMapping(typeDecl, true);
				}
			}
		}
		// Then propagate
		context.propagate();
	}

	//
	// pre collectData
	//

	@SuppressWarnings("serial")
	private void preCollectData(IProgressMonitor monitor,
			List<ICompilationUnit> children, boolean rerun) {
		monitor
				.subTask("Collect Data on compilation units (organize imports, enclosing access, some tricks case, properties and covariance search) ");
		final boolean proxyMode = context.getConfiguration().getOptions()
				.isProxyMode();

		for (final ICompilationUnit icunit : children) {
			try {
				if (proxyMode) {
					applyTransformerOnCompilationUnit(monitor, "Proxy Mode",
							icunit, new RemoveMethodBodyVisitor(context), false);
				}
				// Organize imports
				final IJavaProject project = icunit.getJavaProject();
				final CodeGenerationSettings settings = JavaPreferencesSettings
						.getCodeGenerationSettings(project);
				final ICompilationUnit cu = icunit;
				final boolean save = !cu.isWorkingCopy();
				final OrganizeImportsOperation op = new OrganizeImportsOperation(
						cu, null, settings.importIgnoreLowercase, save, true,
						null);
				op.run(new NullProgressMonitor());

			} catch (final JavaModelException e) {
				e.printStackTrace();
				context.getLogger().logException(
						"JavaModel Error in pre collect data", e);
			} catch (final CoreException e) {
				e.printStackTrace();
				context.getLogger().logException(
						"Core Error in pre collect data", e);
			}
		}

		// Search enclosing access
		for (final ICompilationUnit icunit : children) {

			final CompilationUnit cunit = parse(monitor, icunit, true, false,
					false);
			final int size = cunit.types().size();
			for (int i = 0; i < size; i++) {
				final AbstractTypeDeclaration decl = (AbstractTypeDeclaration) cunit
						.types().get(i);
				if (decl.getNodeType() == ASTNode.TYPE_DECLARATION) {
					final TypeDeclaration typeDecl = (TypeDeclaration) decl;
					searchEnclosingAccess(typeDecl,
							new ArrayList<TypeDeclaration>() {
								{
									this.add(typeDecl);
								}
							}, context);
					// final IType itype = (IType) typeDecl.resolveBinding()
					// .getJavaElement();
					// TODO: context.getTypeHierarchy().addClass(itype);
				}
			}
		}
		// Tricky case where an inner of inner has a superclass that is
		// an inner ....
		for (final ICompilationUnit icunit : children) {
			final CompilationUnit cunit = parse(monitor, icunit, true, false,
					false);
			final int size = cunit.types().size();
			for (int i = 0; i < size; i++) {
				final AbstractTypeDeclaration decl = (AbstractTypeDeclaration) cunit
						.types().get(i);
				if (decl.getNodeType() == ASTNode.TYPE_DECLARATION) {
					final TypeDeclaration typeDecl = (TypeDeclaration) decl;
					if (typeDecl.getTypes() != null
							&& typeDecl.getTypes().length > 0) {
						for (int j = 0; j < typeDecl.getTypes().length; j++) {
							final TypeDeclaration currentInner = typeDecl
									.getTypes()[j];
							if (currentInner.getTypes() != null
									&& currentInner.getTypes().length > 0) {
								for (int k = 0; k < currentInner.getTypes().length; k++) {
									final TypeDeclaration currentInnerInner = currentInner
											.getTypes()[k];
									if (currentInnerInner.getSuperclassType() != null) {
										final Type superOfInnerInner = currentInnerInner
												.getSuperclassType();
										if (contains(superOfInnerInner,
												typeDecl.getTypes())) {
											// currentInner need access to
											// typeDecl
											context
													.addInnerEnclosingAccess(
															currentInner
																	.resolveBinding()
																	.getQualifiedName(),
															typeDecl
																	.resolveBinding()
																	.getQualifiedName());
										}
									}
								}
							}
						}
					}
				}
			}
		}

		// Let's start for auto get/set search
		// TODO: indexers ?
		if (context.getConfiguration().getOptions().isAutoProperties()) {
			for (final ICompilationUnit icunit : children) {
				final CompilationUnit cunit = parse(monitor, icunit, true,
						false, false);
				final int size = cunit.types().size();
				for (int i = 0; i < size; i++) {
					final AbstractTypeDeclaration decl = (AbstractTypeDeclaration) cunit
							.types().get(i);
					if (decl.getNodeType() == ASTNode.TYPE_DECLARATION) {
						final TypeDeclaration typeDecl = (TypeDeclaration) decl;
						try {
							context.autoGetSetSearch(typeDecl);
						} catch (final JavaModelException e) {
							e.printStackTrace();
							context.getLogger().logException("", e);
						} catch (final TranslationModelException e) {
							e.printStackTrace();
							context.getLogger().logException("", e);
						}
					}
				}
			}
		}

		if (!rerun && context.getConfiguration().getOptions().isUseGenerics()
				&& !context.getConfiguration().getOptions().isAutoCovariant()) {
			// find co-variance
			for (final ICompilationUnit icunit : children) {
				try {
					applyTransformerOnCompilationUnit(monitor,
							"Find covariance", icunit,
							new CovarianceManualFinderVisitor(context), false);
				} catch (final Exception e) {
					e.printStackTrace();
					context.getLogger().logException("", e);
				}
			}
		}
	}

	//
	//
	//

	private static boolean contains(Type superOfInnerInner,
			TypeDeclaration[] types) {
		for (final TypeDeclaration currentInner : types) {
			if (currentInner.resolveBinding().isEqualTo(
					superOfInnerInner.resolveBinding())) {
				return true;
			}
		}
		return false;
	}

	private static void searchEnclosingAccess(TypeDeclaration typeDecl,
			List<TypeDeclaration> enclosingList, ITranslationContext context) {
		final int size2 = typeDecl.getTypes().length;
		for (int j = 0; j < size2; j++) {
			final TypeDeclaration inner = typeDecl.getTypes()[j];
			enclosingAccess(inner, enclosingList, context);
			if (inner.getTypes() != null) {
				enclosingList.add(inner);
				searchEnclosingAccess(inner, enclosingList, context);
				enclosingList.remove(inner);
			}
		}
	}

	private static void enclosingAccess(TypeDeclaration inner,
			List<TypeDeclaration> enclosingList, ITranslationContext context) {
		boolean hasAlreadyAnAccess = false;
		for (final TypeDeclaration typeDecl : enclosingList) {
			if (hasAlreadyAnAccess) {
				context.addInnerEnclosingAccess(inner.resolveBinding()
						.getQualifiedName(), typeDecl.resolveBinding()
						.getQualifiedName());
			} else {
				final SearchForEnclosingAccess visitor = new SearchForEnclosingAccess(
						typeDecl, context, false);
				visitor.initTypeHierarchy(inner.resolveBinding());
				inner.accept(visitor);
				if (visitor.hasEnclosingAccess()) {
					hasAlreadyAnAccess = !inner.resolveBinding().getName()
							.contains("Anonymous_C");
					context.addInnerEnclosingAccess(inner.resolveBinding()
							.getQualifiedName(), typeDecl.resolveBinding()
							.getQualifiedName());
				} else {
					// To deal with case where super class of that inner
					// class is an inner class of the super class of the
					// enclosing class.
					// In that case inner must be considered as having access to
					// enclosing !
					Type superClassOfInner = inner.getSuperclassType();
					if (superClassOfInner != null) {
						ITypeBinding superClassOfInnerBinding = superClassOfInner
								.resolveBinding();
						if (superClassOfInnerBinding != null) {
							if (superClassOfInnerBinding.isMember()) {
								final ITypeBinding enclosingOfSuperOfInner = inner
										.getSuperclassType().resolveBinding()
										.getDeclaringClass();
								final ITypeBinding superOfEnclosing = typeDecl
										.resolveBinding().getSuperclass();
								if (enclosingOfSuperOfInner
										.isEqualTo(superOfEnclosing)) {
									final String superOfInnerName = inner
											.getSuperclassType()
											.resolveBinding()
											.getQualifiedName();
									final String superOfEnclosingName = superOfEnclosing
											.getQualifiedName();
									final boolean hasEnc = context
											.hasEnclosingAccess(
													superOfInnerName,
													superOfEnclosingName);
									if (hasEnc) {
										context.addInnerEnclosingAccess(inner
												.resolveBinding()
												.getQualifiedName(), typeDecl
												.resolveBinding()
												.getQualifiedName());
									}
								}
							}
						} else {
							context.getLogger().logError("Super class (" + superClassOfInner + 
									") of inner class (" + inner + ") has no binding !");
						}
					}
				}
			}
		}
	}

	//
	// collect data
	//

	private void collectData(IProgressMonitor monitor,
			List<ICompilationUnit> children) throws CoreException {
		monitor
				.subTask("Collect Data on compilation units (compute virtual/override, store packageName, create custom tags, rename class, search covariance and store imports)");
		final int nbChild = children.size();
		final MethodHierarchy mHierarchy = new MethodHierarchy(context);
		// Compute virtual-override
		for (int i = 0; i < nbChild; i++) {
			final ICompilationUnit icunit = children.get(i);
			try {
				mHierarchy.computeVirtualOverride(icunit);
				// Data we need later at save time
				String packageName = null;
				if (icunit.getPackageDeclarations().length > 0) {
					packageName = icunit.getPackageDeclarations()[0]
							.getElementName();
				}
				if (packageName != null)
					context.addPackageName(icunit.getPath().toString(),
							packageName);
			} catch (final Exception e) {
				e.printStackTrace();
				context.getLogger().logException("", e);
			}
		}

		children = getCompilationUnits(true);
		// Create "virtover" tag
		for (int i = 0; i < nbChild; i++) {
			final ICompilationUnit icunit = children.get(i);
			try {
				mHierarchy.createVirtualOverridTag(icunit);
			} catch (final Exception e) {
				e.printStackTrace();
				context.getLogger().logException("", e);
			}
		}

		// rename class
		for (int i = 0; i < nbChild; i++) {
			final ICompilationUnit icunit = children.get(i);
			try {
				applyTransformerOnCompilationUnit(monitor, "Rename class",
						icunit, new RenameClassVisitor(context), false);
			} catch (final Exception e) {
				e.printStackTrace();
				context.getLogger().logException("", e);
			}
		}
		children = getCompilationUnits(true);

		if (context.getConfiguration().getOptions().isUseGenerics()
				&& context.getConfiguration().getOptions().isAutoCovariant()) {
			// find covariance
			for (int i = 0; i < nbChild; i++) {
				final ICompilationUnit icunit = children.get(i);
				try {
					applyTransformerOnCompilationUnit(monitor,
							"Find covariance", icunit,
							new CovarianceFinderVisitor(context), false);
				} catch (final Exception e) {
					e.printStackTrace();
					context.getLogger().logException("", e);
				}
			}

			// find covariance part 2
			for (int i = 0; i < nbChild; i++) {
				final ICompilationUnit icunit = children.get(i);
				try {
					applyTransformerOnCompilationUnit(monitor,
							"Find covariance 2", icunit,
							new CovarianceFinderVisitor2(context), false);
				} catch (final Exception e) {
					e.printStackTrace();
					context.getLogger().logException("", e);
				}
			}

			// apply covariance
			for (int i = 0; i < nbChild; i++) {
				final ICompilationUnit icunit = children.get(i);
				try {
					applyTransformerOnCompilationUnit(monitor,
							"Change covariance", icunit,
							new CovarianceChangeVisitor(context), false);
				} catch (final Exception e) {
					e.printStackTrace();
					context.getLogger().logException("", e);
				}
			}

			context.clearCovariance();
		}

		//
		// imports / generics
		//
		for (int i = 0; i < nbChild; i++) {
			final ICompilationUnit icunit = children.get(i);
			try {
				context.addImports(icunit, icunit.getImports());
				//
				final IType[] types = icunit.getTypes();
				markgenericTypes(types);
			} catch (final Exception e) {
				e.printStackTrace();
				context.getLogger().logException("", e);
			}
		}

	}

	private void markgenericTypes(IType[] types) throws JavaModelException {
		if (types == null)
			return;
		for (final IType currentType : types) {
			if (currentType.getTypeParameters().length != 0) {
				context.addGenericType(currentType.getFullyQualifiedName(),
						currentType.getJavaProject().getProject());
			}
			markgenericTypes(currentType.getTypes());
		}
	}

	//
	// Delayed transformers
	//

	private void launchASTNodesReplacementTransformers(
			IProgressMonitor monitor, List<ICompilationUnit> children,
			List<ITransformer> transformers) throws CoreException {
		context.clearChange();
		for (final ITransformer transformer : transformers) {
			final long start = System.nanoTime();
			for (final ICompilationUnit icunit : children) {
				// First launch the transformer on the CU
				applyTransformerOnCompilationUnit(monitor,
						"AST Nodes Replacement", icunit, transformer, false);
				// apply the change
				applyChange(monitor, children, icunit);
				// clear them
				context.clearChange();
				// If transformer has a port action to perform
				if (transformer.hasPostActionOnAST()) {
					// Then launch the post action (a visitor, ...) with the
					// previous transformer state !
					applyTransformerOnCompilationUnit(monitor,
							"AST Nodes Replacement", icunit, transformer, true);
					// Then, apply change
					applyChange(monitor, children, icunit);
				} else
					transformer.reset(); // just in case
			}
			final long end = System.nanoTime();
			context.getLogger().logInfo(
					"      " + transformer.getName() + " in "
							+ getDuration(start, end));
		}
		context.clearChange();
	}

	private boolean applyChange(IProgressMonitor monitor,
			List<ICompilationUnit> children, ICompilationUnit icunit)
			throws CoreException {
		final Change change = context.getChange(icunit);
		if (change != null) {
			if (Translator.DEBUG) {
				context.getLogger().logInfo("   # Start Applying change");
			}
			final long localstart = new GregorianCalendar().getTimeInMillis();
			final IProgressMonitor subMonitor = new SubProgressMonitor(monitor,
					1);
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
						" On class " + icunit.getElementName(), e);
			} finally {
				change.dispose();
				final long localend = new GregorianCalendar().getTimeInMillis();
				monitor.subTask("Applying change(s) on class "
						+ icunit.getElementName() + " done in "
						+ (localend - localstart) + "ms");
			}
		}
		return true;
	}

	//
	// launch transformers
	//

	@SuppressWarnings("deprecation")
	private void launchTransformersOnChildren(IProgressMonitor monitor,
			List<ICompilationUnit> children, List<ITransformer> transformers,
			String message) throws CoreException {

		for (final ITransformer transformer : transformers) {
			final long start = System.nanoTime();
			for (final ICompilationUnit icunit : children) {
				try {
					applyTransformerOnCompilationUnit(monitor, message, icunit,
							transformer, false);
				} catch (final Exception e) {
					context.getLogger().logException(
							"Error during " + transformer.getName() + " on "
									+ icunit.getElementName(), e);
					// e.printStackTrace();
				} finally {
					// icunit.discardWorkingCopy();
					// icunit.close();
				}
			}
			final long end = System.nanoTime();
			context.getLogger().logInfo(
					"      " + transformer.getName() + " in "
							+ getDuration(start, end));
		}
	}

	@SuppressWarnings("deprecation")
	private void launchChildrenOnTransformers(IProgressMonitor monitor,
			List<ICompilationUnit> children, List<ITransformer> transformers,
			String message) throws CoreException {

		for (final ICompilationUnit icunit : children) {
			for (final ITransformer transformer : transformers) {
				try {
					applyTransformerOnCompilationUnit(monitor, message, icunit,
							transformer, false);
				} catch (final Exception e) {
					context.getLogger().logException(
							"Error during " + transformer.getName() + " on "
									+ icunit.getElementName(), e);
				}
			}
		}
	}

	/**
	 * 
	 * @param monitor
	 * @param message
	 * @param icunit
	 * @param transformer
	 * @param runPostAction
	 *            : if false, reset the transformer state, launch it and DO NOT
	 *            RESET the it state. if true, do not reset the transformer
	 *            state, launch the post action (if exist) and RESET the
	 *            transformer state at the end.
	 * @throws CoreException
	 */
	private void applyTransformerOnCompilationUnit(IProgressMonitor monitor,
			String message, ICompilationUnit icunit, ITransformer transformer,
			boolean runPostAction) throws CoreException {
		final IProgressMonitor subMonitor = new NullProgressMonitor();

		// Check if the transformer has any chance to run
		if (!transformer.canRun())
			return;

		// If we do not want to keep the state, reset the state
		if (!runPostAction) {
			transformer.reset();
			// Set the ICU
			transformer.setCompilationUnit(icunit);
		}

		int i = 0;

		final long localstart = System.nanoTime();
		// Build the AST
		CompilationUnit unit = parse(subMonitor, icunit, transformer
				.needValidation(), transformer.isAbridged(), transformer
				.needRecovery());
		// Is all the CU need to be compilable, if yes reporting comilation
		// error could be a good idea
		if (transformer.needCompilable()
				&& context.getLogger().getLogLevel() != Logger.LogLevel.INFO) {
			Message[] messages = unit.getMessages();
			if (messages != null && messages.length > 0) {
				context.getLogger().logWarning(
						"      Compilation units have " + messages.length
								+ " error(s)");
				/*
				 * for(Message m : messages) {
				 * context.getLogger().logWarning(m.getMessage()); }
				 */
			}
		}

		// To save parsing time is case of run only once visitor
		if (transformer.runOnce()) {
			monitor.subTask("<" + transformer.getName() + "> on class "
					+ icunit.getElementName() + " " + i++);
			context.getLogger().logVerboseNoLN(
					"<" + transformer.getName() + "> on class "
							+ icunit.getElementName());
			runTransformerOnAST(icunit, transformer, runPostAction, subMonitor,
					unit);
			transformer.postAction(icunit, unit);
			context.getLogger().logVerbose(
					" in " + getDuration(localstart, System.nanoTime()) + ".");
		} else {
			long thisStart = localstart;
			while (transformer.runAgain(unit)) {
				monitor.subTask("<" + transformer.getName() + "> on class "
						+ icunit.getElementName() + " " + i++);
				context.getLogger().logVerboseNoLN(
						"<" + transformer.getName() + "> on class "
								+ icunit.getElementName() + " "
								+ ((i > 1) ? " Take " + i : ""));
				runTransformerOnAST(icunit, transformer, runPostAction,
						subMonitor, unit);
				unit = icunit.reconcile(AST.JLS3, false, null, subMonitor); // JavaModelUtil.reconcile(icunit);
				if (unit == null) {
					unit = parse(subMonitor, icunit, transformer
							.needValidation(), transformer.isAbridged(),
							transformer.needRecovery());
				}
				transformer.postAction(icunit, unit);
				long thisEnd = System.nanoTime();
				context.getLogger().logVerbose(
						" in " + getDuration(thisStart, thisEnd) + ".");
				thisStart = thisEnd;
			}
		}
		// We dot not need anymore the state. Clear !
		if (runPostAction)
			transformer.reset();

		unit = null;
		// 
		final long localend = System.nanoTime();
		if (i >= 1) {
			monitor.subTask(message + " done in "
					+ getDuration(localstart, localend) + ".");
		}
	}

	private void runTransformerOnAST(ICompilationUnit icunit,
			ITransformer transformer, boolean runPostAction,
			final IProgressMonitor subMonitor, CompilationUnit unit)
			throws CoreException, JavaModelException {
		if (!runPostAction) {
			transformer.transform(subMonitor, unit);
			transformer.applyChange(subMonitor);
		} else if (transformer.hasPostActionOnAST()) {
			// unit = icunit.reconcile(AST.JLS3, false, null, subMonitor); //
			// JavaModelUtil.reconcile(icunit);
			/*
			 * if (unit == null) { unit = parse(subMonitor, icunit,
			 * transformer.needValidation(), transformer.isAbridged(),
			 * transformer.needRecovery()); }
			 */
			transformer.postActionOnAST(icunit, unit);
		}
	}

	//
	// Transformers
	//

	private final void createTransformers() {
		try {
			for (final Pass pass : context.getConfiguration()
					.getTranslationDescriptor().getPasses()) {
				pass.createTransformers(context);
				passes.put(pass.getName(), pass);
			}
		} catch (final Exception e) {
			context.getLogger().logException(" Error in create transformers.",
					e);
		}
	}

	//
	// getCompilationUnits
	//

	private List<ICompilationUnit> getCompilationUnits(boolean refresh)
			throws CoreException {
		if (computedCU == null || refresh) {
			final List<ICompilationUnit> cus = new ArrayList<ICompilationUnit>();
			final IJavaElement[] children = context.getConfiguration()
					.getWorkingProject().getChildren();
			for (final IJavaElement element : children) {
				if (element.getElementType() == IJavaElement.PACKAGE_FRAGMENT_ROOT) {
					final PackageFragmentRoot root = (PackageFragmentRoot) element;
					final IJavaElement[] child = root.getChildren();
					for (final IJavaElement element0 : child) {
						if (element0.getElementType() == IJavaElement.PACKAGE_FRAGMENT) {
							final PackageFragment fragment = (PackageFragment) element0;
							String pName = fragment.getElementName();
							if (!context.ignorablePackage(pName, null)) {
								final ICompilationUnit[] units = fragment
										.getCompilationUnits();
								if (units != null) {
									for (final ICompilationUnit unit : units) {
										if (!context.ignore(unit)) {
											cus.add(unit);
										}
									}
								}
							}
						}
					}
				}
			}
			computedCU = sortCompilationUnits(cus);
		}
		return computedCU;
	}

	@SuppressWarnings("restriction")
	private void checkPackageDotHtmlFiles() throws CoreException {
		final IJavaElement[] children = context.getConfiguration()
				.getWorkingProject().getChildren();
		for (final IJavaElement element : children) {
			if (element.getElementType() == IJavaElement.PACKAGE_FRAGMENT_ROOT) {
				final PackageFragmentRoot root = (PackageFragmentRoot) element;
				final IJavaElement[] child = root.getChildren();
				for (final IJavaElement element0 : child) {
					if (element0.getElementType() == IJavaElement.PACKAGE_FRAGMENT) {
						final PackageFragment fragment = (PackageFragment) element0;
						final Object[] nonJavaResource = fragment
								.getNonJavaResources();
						if (nonJavaResource != null) {
							for (final Object obj : nonJavaResource) {
								if (obj instanceof org.eclipse.core.internal.resources.File) {
									org.eclipse.core.internal.resources.File file = (org.eclipse.core.internal.resources.File) obj;
									if (file.getName().equals("package.html")) {
										context
												.addPackageToPublicDocumented(fragment
														.getElementName());
									}
								}
							}
						}
					}
				}
			}
		}
	}

	private static List<ICompilationUnit> sortCompilationUnits(
			List<ICompilationUnit> children) throws CoreException {
		// ordered
		final Map<Integer, List<ICompilationUnit>> map = new Hashtable<Integer, List<ICompilationUnit>>();
		for (final ICompilationUnit icUnit : children) {
			int weight = 0;
			final IType[] types = icUnit.getTypes();
			if (types != null) {
				for (final IType type : types) {
					final ITypeHierarchy hierachy = type
							.newSupertypeHierarchy(new NullProgressMonitor());
					final IType[] superTypes = hierachy.getAllTypes();
					weight = superTypes.length;
				}
			}
			List<ICompilationUnit> list = map.get(weight);
			if (list == null) {
				list = new ArrayList<ICompilationUnit>();
				map.put(weight, list);
			}
			list.add(icUnit);
		}
		//
		final List<ICompilationUnit> cus2 = new ArrayList<ICompilationUnit>();
		final List<Integer> keySet = new ArrayList<Integer>(map.keySet());
		Collections.sort(keySet);
		for (final int indice : keySet) {
			final List<ICompilationUnit> list = map.get(indice);
			if (list != null)
				for (final ICompilationUnit ic : list)
					cus2.add(ic);
		}
		return cus2;
	}

	private List<ICompilationUnit> getNewCompilationUnits(
			List<ICompilationUnit> old) throws CoreException {
		final List<ICompilationUnit> cus = new ArrayList<ICompilationUnit>();
		final IJavaElement[] children = context.getConfiguration()
				.getWorkingProject().getChildren();
		for (final IJavaElement element : children) {
			if (element.getElementType() == IJavaElement.PACKAGE_FRAGMENT_ROOT) {
				final PackageFragmentRoot root = (PackageFragmentRoot) element;
				final IJavaElement[] child = root.getChildren();
				for (final IJavaElement element0 : child) {
					if (element0.getElementType() == IJavaElement.PACKAGE_FRAGMENT) {
						final PackageFragment fragment = (PackageFragment) element0;
						final ICompilationUnit[] units = fragment
								.getCompilationUnits();
						if (units != null) {
							for (final ICompilationUnit unit : units) {
								if (!context.ignore(unit)
										&& !old.contains(unit)) {
									cus.add(unit);
								}
							}
						}
					}
				}
			}
		}
		return cus;
	}

	//
	// parse
	//

	public CompilationUnit parse(IProgressMonitor pm, ICompilationUnit icunit,
			boolean validation, boolean abridged, boolean recovery) {
		final IProgressMonitor monitor = new SubProgressMonitor(pm, 1);
		CompilationUnit result = null;
		try {
			parser.setProject(icunit.getJavaProject());
			parser.setSource(icunit);
			if (abridged) {
				parser.setFocalPosition(0);
			}
			parser.setResolveBindings(validation);
			parser.setStatementsRecovery(recovery);
			result = (CompilationUnit) parser.createAST(monitor);
		} catch (Exception e) {
			context.getLogger().logException(icunit.getElementName(), e);
			return null;
		}
		monitor.done();
		return result;
	}

	private Object[] searchForNewCompilationUnit(ICompilationUnit icunit2)
			throws JavaModelException, CoreException {
		CompilationUnit unit2;
		final IJavaElement[] children = context.getConfiguration()
				.getWorkingProject().getChildren();
		for (final IJavaElement element : children) {
			if (element.getElementType() == IJavaElement.PACKAGE_FRAGMENT_ROOT) {
				final PackageFragmentRoot root = (PackageFragmentRoot) element;
				final IJavaElement[] child = root.getChildren();
				for (final IJavaElement element0 : child) {
					if (element0.getElementType() == IJavaElement.PACKAGE_FRAGMENT) {
						final PackageFragment fragment = (PackageFragment) element0;
						String pName = fragment.getElementName();
						if (!context.ignorablePackage(pName, null)) {
							final ICompilationUnit[] units = fragment
									.getCompilationUnits();
							if (units != null) {
								for (final ICompilationUnit thisunit : units) {
									if (!context.ignore(thisunit)) {
										if (thisunit.getElementName() == icunit2
												.getElementName()) {
											UpdateHandlerTagVisitor v = new UpdateHandlerTagVisitor(
													context);
											final IProgressMonitor subMonitor = new NullProgressMonitor();
											unit2 = parse(subMonitor, thisunit,
													v.needValidation(), v
															.isAbridged(), v
															.needRecovery());
											return new Object[] { thisunit,
													unit2 };

										}
									}
								}
							}
						}
					}
				}
			}
		}
		return null;
	}

	public static CompilationUnit parseAbridged(IProgressMonitor pm,
			ICompilationUnit icunit, boolean validation) {
		final IProgressMonitor monitor = new SubProgressMonitor(pm, 1);
		final ASTParser parser = ASTParser.newParser(AST.JLS3);
		parser.setProject(icunit.getJavaProject());
		parser.setSource(icunit);
		parser.setFocalPosition(0);
		parser.setResolveBindings(validation);
		final CompilationUnit result = (CompilationUnit) parser
				.createAST(monitor);
		monitor.done();
		return result;
	}
}
