package com.ilog.translator.java2cs.translation.astrewriter.astchange;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

import org.eclipse.core.runtime.CoreException;
import org.eclipse.core.runtime.IProgressMonitor;
import org.eclipse.core.runtime.NullProgressMonitor;
import org.eclipse.core.runtime.SubProgressMonitor;
import org.eclipse.jdt.core.IBuffer;
import org.eclipse.jdt.core.ICompilationUnit;
import org.eclipse.jdt.core.IMember;
import org.eclipse.jdt.core.IPackageFragment;
import org.eclipse.jdt.core.IType;
import org.eclipse.jdt.core.JavaModelException;
import org.eclipse.jdt.core.dom.CompilationUnit;
import org.eclipse.jdt.core.dom.EnumDeclaration;
import org.eclipse.jdt.core.dom.MethodDeclaration;
import org.eclipse.jdt.core.dom.Modifier;
import org.eclipse.jdt.core.dom.SingleVariableDeclaration;
import org.eclipse.jdt.internal.corext.refactoring.util.JavaElementUtil;
import org.eclipse.jdt.internal.corext.util.JavaModelUtil;
import org.eclipse.jdt.internal.ui.preferences.JavaPreferencesSettings;
import org.eclipse.ltk.core.refactoring.Change;
import org.eclipse.ltk.core.refactoring.RefactoringStatus;
import org.eclipse.ltk.core.refactoring.participants.MoveArguments;
import org.eclipse.ltk.core.refactoring.participants.MoveRefactoring;
import org.eclipse.text.edits.TextEditGroup;

import com.ilog.translator.java2cs.configuration.info.ClassInfo;
import com.ilog.translator.java2cs.configuration.info.TranslationModelException;
import com.ilog.translator.java2cs.translation.ITranslationContext;
import com.ilog.translator.java2cs.translation.astrewriter.PrepareExtensionMethodsForEnum1;
import com.ilog.translator.java2cs.translation.noderewriter.INodeRewriter;
import com.ilog.translator.java2cs.translation.util.TranslationUtils;

/**
 * Move instance methods from enumerations in new class and generate Extension Methods on the C# side
 *
 * @author nrrg
 */
@SuppressWarnings("restriction")
public class MoveEnumMembers extends ASTChangeVisitor {

	private static Map<String, List<IMember>> $methodInit = new HashMap<String, List<IMember>>();

	protected boolean runOnce = true;
	private boolean inEnum = false;
	public static final String EXTENSION_CLASSES_SUFFIX = "Extension";

	public MoveEnumMembers(ITranslationContext context) {
		super(context);
		this.transformerName = "Move Enum Members";
		description = new TextEditGroup(transformerName);
	}

	 public Change createChange(IProgressMonitor pm, IMember[] elements,
				IType destination) throws JavaModelException, CoreException {
			if (simulate) {
				return null;
			}
			/*final MoveStaticMembersProcessor processor = new MoveStaticMembersProcessor(
					elements, JavaPreferencesSettings.getCodeGenerationSettings(fCu.getJavaProject()));
			 */			
			
			final MoveStaticMemberProcessor2 processor = new MoveStaticMemberProcessor2(
					elements, JavaPreferencesSettings.getCodeGenerationSettings(fCu.getJavaProject()));

			 MoveRefactoring refactoring = new MoveRefactoring( processor );
		      processor.setDestinationTypeFullyQualifiedName(destination
		              .getFullyQualifiedName());  
				processor.setUpdateReference(false);
				processor.setDeprecateDelegates(false);
			//final MoveArguments mArguments = new MoveArguments(destination, false);
			//processor.initialize(mArguments);
			
			/*final JavaRefactoringArguments arguments = new JavaRefactoringArguments(
					fCu.getJavaProject().getElementName());
			arguments.setAttribute(JDTRefactoringDescriptor.ATTRIBUTE_REFERENCES, "false");
			processor.initialize(arguments);*/
			
			processor.setDelegateUpdating(true);
			for(int i=0;i<elements.length;i++){
				context.getLogger().logInfo("[Custom Info] Move method "+ elements[i].getElementName()+" to class "+destination.getFullyQualifiedName());	
			}
			
			processor.setDestinationTypeFullyQualifiedName(destination.getFullyQualifiedName());
			final RefactoringStatus status = refactoring.checkAllConditions(pm);

			if (status.hasFatalError()) {
				System.out.println(" ERROR in Move enum members >> " + status
						+ " for " + fCu.getElementName());
				return null;
			}
			return refactoring.createChange(pm);
		}
	
	@Override
	public void endVisit(EnumDeclaration node) {
		inEnum = false;
		try {
			IMember[] elements = this.collectMethodElement(node.getName()
					.toString());

			if ((elements != null) && (elements.length > 0)) {
				ClassInfo ci = this.context.getModel().findClassInfo(
						node.resolveBinding().getJavaElement()
						.getHandleIdentifier(), true, false,
						TranslationUtils.isGeneric(node.resolveBinding()));
				String className = node.getName() + EXTENSION_CLASSES_SUFFIX;
				IProgressMonitor pm = new NullProgressMonitor();
				IType createdType = this.createType(pm, node, className);
				change = this.createChange(pm, elements, createdType);
				for(IMember element: elements){
					String handler = createdType.getHandleIdentifier()+"~"+element.getElementName();
					//context.getLogger().logInfo(" HANDLER "+handler +" source "+createdType.getElementName()+".java");
					final INodeRewriter result = context.getMapper()
					.mapMethodDeclaration(createdType.getElementName()+".java", JavaElementUtil.createSignature(element),
							handler, false, false, false);
				}
				context.addExtensionClassName(className);
			}
		} catch (JavaModelException e) {
			context.getLogger().logException("endVisit(EnumVisitor)", e);
		} catch (CoreException e) {
			context.getLogger().logException("endVisit(EnumVisitor)", e);
		} catch (TranslationModelException e) {
			context.getLogger().logException("endVisit(EnumVisitor)", e);
		}
	}

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

	@Override
	public void reset() {
		super.reset();
		runOnce = true;
	}

	@Override
	public boolean visit(EnumDeclaration node) {
		inEnum = true;
		return true;
	}

	@Override
	public boolean visit(MethodDeclaration node) {
		if (inEnum && isExtensionMethod(node)) {
			String enumName = node.resolveBinding().getDeclaringClass()
					.getName();
			List<IMember> list = null;
			if ((list = $methodInit.get(enumName)) == null) {
				list = new ArrayList<IMember>();
				$methodInit.put(enumName, list);
			}
			list.add((IMember) node.resolveBinding().getJavaElement());
		}
		return false;
	}

    private boolean isExtensionMethod(MethodDeclaration node){
    	//Extension methods works only for static methods prepared with the transformation
		//The original static methods will not be moved - static methods can not be transformed in extension methods
    	if (!Modifier.isStatic(node.getModifiers()))
    		return false;
    	if (node.isConstructor())
    		return false;
    	if (node.resolveBinding() == null)
    		return false;
    	//Methods prepared for extension have first parameter of type Enumeration
    	if(node.parameters().size()==0) return false;
    	if (node.parameters().get(0) instanceof SingleVariableDeclaration) {
    		SingleVariableDeclaration declaration = (SingleVariableDeclaration) node.parameters().get(0);
    		if(declaration.getName().getIdentifier().equalsIgnoreCase(PrepareExtensionMethodsForEnum1.PARAMETER_NAME)){
    			return true;
    		}
		}
		return false;
    }
    
	private IMember[] collectMethodElement(String name) {
		if ($methodInit.get(name) == null)
			return null;
		IMember[] imethods = $methodInit.get(name).toArray(
				new IMember[$methodInit.get(name).size()]);

		return imethods;
	}

	private IType createType(IProgressMonitor pm, EnumDeclaration node,
			String newClassName) throws JavaModelException, CoreException {
		IPackageFragment pack = (IPackageFragment) this.fCu.getParent();

		String modifiers = java.lang.reflect.Modifier.toString(node
				.getModifiers());
		String contents = "package " + pack.getElementName() + ";\n"
		+ modifiers + " class " + newClassName + " { \n}\n";

		String cuName = this.getCompilationUnitName(newClassName);

		ICompilationUnit parentCU = pack.createCompilationUnit(cuName,
				"", false, new SubProgressMonitor(pm, 2)); //$NON-NLS-1$
		// create a working copy with a new owner
		parentCU.becomeWorkingCopy(null, new SubProgressMonitor(pm, 1));

		IBuffer buffer = parentCU.getBuffer();
		//context.getLogger().logInfo("parentCU.getHandleIdentifier()-"+parentCU.getHandleIdentifier());

		buffer.setContents(contents);

		IType createdType = parentCU.getType(newClassName);

		ICompilationUnit cu = createdType.getCompilationUnit();
		//context.getLogger().logInfo("cu.getHandleIdentifier()-"+cu.getHandleIdentifier());

		JavaModelUtil.reconcile(cu);

		cu.commitWorkingCopy(true, new SubProgressMonitor(pm, 1));

		return createdType;
	}

	private String getCompilationUnitName(String typeName) {
		return typeName + JavaModelUtil.DEFAULT_CU_SUFFIX;
	}
}
