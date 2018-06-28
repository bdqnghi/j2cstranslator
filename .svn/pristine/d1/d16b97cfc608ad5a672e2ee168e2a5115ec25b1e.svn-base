package com.ilog.translator.java2cs.translation.astrewriter.astchange;

import java.util.ArrayList;
import java.util.List;

import org.eclipse.core.runtime.CoreException;
import org.eclipse.core.runtime.IProgressMonitor;
import org.eclipse.core.runtime.NullProgressMonitor;
import org.eclipse.core.runtime.SubProgressMonitor;
import org.eclipse.jdt.core.IBuffer;
import org.eclipse.jdt.core.ICompilationUnit;
import org.eclipse.jdt.core.IField;
import org.eclipse.jdt.core.IMember;
import org.eclipse.jdt.core.IPackageFragment;
import org.eclipse.jdt.core.IType;
import org.eclipse.jdt.core.JavaModelException;
import org.eclipse.jdt.core.dom.ASTNode;
import org.eclipse.jdt.core.dom.BodyDeclaration;
import org.eclipse.jdt.core.dom.CompilationUnit;
import org.eclipse.jdt.core.dom.EnumDeclaration;
import org.eclipse.jdt.core.dom.FieldDeclaration;
import org.eclipse.jdt.core.dom.ITypeBinding;
import org.eclipse.jdt.core.dom.TypeDeclaration;
import org.eclipse.jdt.core.dom.VariableDeclarationFragment;
// import org.eclipse.jdt.internal.corext.refactoring.structure.JavaMoveRefactoring;
import org.eclipse.jdt.internal.corext.util.JavaModelUtil;
import org.eclipse.jdt.internal.ui.preferences.JavaPreferencesSettings;
import org.eclipse.ltk.core.refactoring.Change;
import org.eclipse.ltk.core.refactoring.RefactoringStatus;
import org.eclipse.ltk.core.refactoring.participants.MoveRefactoring;
import org.eclipse.text.edits.TextEditGroup;

import com.ilog.translator.java2cs.configuration.info.ClassInfo;
import com.ilog.translator.java2cs.configuration.target.TargetClass;
import com.ilog.translator.java2cs.configuration.target.TargetPackage;
import com.ilog.translator.java2cs.translation.ITranslationContext;
import com.ilog.translator.java2cs.translation.noderewriter.INodeRewriter;
import com.ilog.translator.java2cs.translation.util.TranslationUtils;

/**
 * Move field declarations from interface to a *new* abstract class
 * 
 * @author afau
 * 
 */
public class ReplaceInterfaceFieldDeclarationVisitor extends ASTChangeVisitor {

	protected boolean runOnce = true;

	//
	//
	//

	public ReplaceInterfaceFieldDeclarationVisitor(ITranslationContext context) {
		super(context);
		transformerName = "Replace interface field declaration";
		description = new TextEditGroup(transformerName);
	}

	//
	//
	//

	@Override
	public boolean runOnce() {
		return true;
	}

	//
	//
	//

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
	public void reset() {
		super.reset();
		runOnce = true;
	}

	//
	//
	//

	public Change createChange(IProgressMonitor pm, IMember[] elements,
			IType destination) throws JavaModelException, CoreException {
		if (simulate) {
			return null;
		}
		final MoveStaticMemberProcessor2 processor = new MoveStaticMemberProcessor2(
				elements, JavaPreferencesSettings.getCodeGenerationSettings(fCu
						.getJavaProject()));
		MoveRefactoring refactoring = new MoveRefactoring(processor);
		// MoveArguments mArguments = new MoveArguments(destination, true);
		processor.setDestinationTypeFullyQualifiedName(destination
				.getFullyQualifiedName());		
		// refactoring.initialize(mArguments);
		processor.setUpdateReference(true);
		
		final RefactoringStatus status = refactoring.checkAllConditions(pm);

		if (status.hasFatalError()) {
			System.out.println(" ERROR in ReplaceInterfaceField >> " + status
					+ " for " + fCu.getElementName());
			return null;
		}
		return refactoring.createChange(pm);
	}

	//
	//
	//

	@Override
	public void endVisit(TypeDeclaration node) {
		try {
			if (!node.isMemberTypeDeclaration() && node.isInterface()) {
				final IProgressMonitor pm = new NullProgressMonitor();

				final IMember[] elements = collectFieldElement(node);

				if ((elements != null) && (elements.length > 0)) {
					final String newClassName = node.getName()
							+ context.getMapper().getPrefixForConstants();
					final IType createdType = createType(pm, node, newClassName);
					change = this.createChange(pm, elements, createdType);

					// ok add the fact that each element of 'elements' in class
					// Name
					// are renamed
					final String packageName = createdType.getPackageFragment()
							.getElementName();

					final List<String> fieldsName = new ArrayList<String>();

					for (final IMember member : elements) {
						if (member instanceof IField) {
							fieldsName.add(member.getElementName());
							// TODO: copy mapping to new class
						}
						if (member instanceof IType) {
							final String typeName = member.getElementName();
							context.getModel().addImplicitNestedRename(null,
									newClassName + "." + typeName, packageName,
									node.getName() + "." + typeName);
						}
					}
					//
					//
					final ClassInfo ci = context.getModel().findClassInfo(
							node.resolveBinding().getJavaElement()
									.getHandleIdentifier(), true, false,
							TranslationUtils.isGeneric(node.resolveBinding()));
					final ClassInfo nci = ci.cloneContentFor(createdType,
							fieldsName);
					context.getModel().addClassInfo(
							node.resolveBinding().getPackage().getName(),
							createdType, nci);
					String targetFramework = context.getConfiguration().getOptions().getTargetDotNetFramework().name();
					
					if (ci.getTarget(targetFramework) != null && ci.getTarget(targetFramework).isTranslated())
						context.addToIgnorable(nci);
					//
					context.getModel().addImplicitConstantsRename(packageName,
							node.getName().getIdentifier(), newClassName,
							fieldsName.toArray(new String[fieldsName.size()]));
				}
			} else {
				// a class
				ITypeBinding typeB  = node.resolveBinding();
				final ClassInfo thisCi = context.getModel().findClassInfo(
						node.resolveBinding().getJavaElement()
								.getHandleIdentifier(), true, false,
						TranslationUtils.isGeneric(node.resolveBinding()));
				for(ITypeBinding interf : typeB.getInterfaces()) {
					String handleIdentifier = interf.getJavaElement().getHandleIdentifier();
					ClassInfo ci = context.getModel().findClassInfo(handleIdentifier, false, false, true);
					if (ci != null) {
						String targetFramework = context.getConfiguration().getOptions().getTargetDotNetFramework().name();
						
						TargetClass tc = ci.getTarget(targetFramework);
						if (tc != null) {
							String code = tc.getCodeToAddToImplementation();
							if (code != null) {
								code = "\n\t#region AddedByTranslator\n\n\t" + code + 
										"\n\n\t#endregion\n\n";
								String newName = null;
								if (thisCi != null && thisCi.getTarget(targetFramework) != null && 
										thisCi.getTarget(targetFramework).getName() != null) {
									String cN = thisCi.getTarget(targetFramework).getName();
									String pN = thisCi.getTarget(targetFramework).getPackageName();
									newName = pN + "." + cN;
								} else {
									String jName = typeB.getPackage().getName();
									TargetPackage tPck = context.getModel().findPackageMapping(jName, null);
									if (tPck != null) {
										newName = tPck.getName() + "." + typeB.getName();
									} else {
										newName = typeB.getQualifiedName();
									}
								}
								code = code.replace("%NAME%", newName);
								context.markClassForImplementationAdd(typeB.getJavaElement().getHandleIdentifier(), code);
							}
						}
					}
				}
			}
		} catch (final Exception e) {
			context.getLogger().logException("endVisit(TypeDeclaration)", e);
		}
	}

	//
	//
	//

	@SuppressWarnings("deprecation")
	private IType createType(IProgressMonitor pm, TypeDeclaration node,
			String newClassName) throws JavaModelException, CoreException {
		final IPackageFragment pack = (IPackageFragment) fCu.getParent();

		final String modifiers = java.lang.reflect.Modifier.toString(node
				.getModifiers());
		final String contents = "package " + pack.getElementName() + ";\n"
				+ modifiers + " class " + newClassName + " { \n}\n";

		final String cuName = getCompilationUnitName(newClassName);

		final ICompilationUnit parentCU = pack.createCompilationUnit(cuName,
				"", false, new SubProgressMonitor(pm, 2)); //$NON-NLS-1$
		// create a working copy with a new owner
		parentCU.becomeWorkingCopy(null, new SubProgressMonitor(pm, 1));

		final IBuffer buffer = parentCU.getBuffer();

		buffer.setContents(contents);

		final IType createdType = parentCU.getType(newClassName);

		final ICompilationUnit cu = createdType.getCompilationUnit();

		JavaModelUtil.reconcile(cu);

		cu.commitWorkingCopy(true, new SubProgressMonitor(pm, 1));

		return createdType;
	}

	private String getCompilationUnitName(String typeName) {
		return typeName + JavaModelUtil.DEFAULT_CU_SUFFIX;
	}

	@SuppressWarnings("unchecked")
	private IMember[] collectFieldElement(TypeDeclaration node) {
		final IType itype = (IType) node.resolveBinding().getJavaElement();

		//
		// Fields
		//
		final FieldDeclaration[] fields = node.getFields();
		final List<IMember> fieldInit = new ArrayList<IMember>();
		for (final FieldDeclaration field : fields) {
			final VariableDeclarationFragment varDecl = TranslationUtils
					.getFrament(field);
			if (varDecl.getInitializer() != null) {
				final String fieldName = varDecl.getName()
						.getFullyQualifiedName();
				fieldInit.add(itype.getField(fieldName));
			}
		}

		//		
		// Types
		//
		final TypeDeclaration[] innerTypes = node.getTypes();
		for (final TypeDeclaration innerType : innerTypes) {
			final INodeRewriter rewriter = context.getMapper()
					.mapTypeDeclaration(
							innerType.resolveBinding(),
							innerType.resolveBinding().getJavaElement()
									.getHandleIdentifier());
			if (rewriter != null && !rewriter.isRemove())
				fieldInit.add((IMember) innerType.resolveBinding()
						.getJavaElement());
		}

		//
		// Enums
		//
		final List<BodyDeclaration> bodiesDecl = node.bodyDeclarations();
		for (final BodyDeclaration body : bodiesDecl) {
			if (body.getNodeType() == ASTNode.ENUM_DECLARATION) {
				final EnumDeclaration enumDecl = (EnumDeclaration) body;
				fieldInit.add((IMember) enumDecl.resolveBinding()
						.getJavaElement());
			}
		}

		final IMember[] ifields = fieldInit.toArray(new IMember[fieldInit
				.size()]);

		return ifields;
	}
}
