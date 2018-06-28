package com.ilog.translator.java2cs.translation.astrewriter;

import java.util.List;

import org.eclipse.core.resources.IProject;
import org.eclipse.jdt.core.dom.ASTNode;
import org.eclipse.jdt.core.dom.Type;
import org.eclipse.jdt.core.dom.TypeDeclaration;
import org.eclipse.jdt.core.dom.rewrite.ListRewrite;
import org.eclipse.jdt.internal.corext.dom.ASTNodes;
import org.eclipse.text.edits.TextEditGroup;

import com.ilog.translator.java2cs.configuration.ChangeHierarchyDescriptor;
import com.ilog.translator.java2cs.configuration.info.ClassInfo;
import com.ilog.translator.java2cs.configuration.info.PackageInfo;
import com.ilog.translator.java2cs.translation.ITranslationContext;

/**
 * Modify type hierarchy
 * 
 * @author afau
 */
public class ChangeHierarchieVisitor extends ASTRewriterVisitor {

	//
	//
	//

	public ChangeHierarchieVisitor(ITranslationContext context) {
		super(context);
		transformerName = "Change Hierarchie";
		description = new TextEditGroup(transformerName);
	}

	//
	//
	//

	@SuppressWarnings("unchecked")
	@Override
	public void endVisit(TypeDeclaration node) {
		final Type superClass = node.getSuperclassType();
		final List interfaces = node.superInterfaceTypes();
		
		// Remove super class TestCase for junit tests  - C# TestCase is obsolete  --> 
		if((node.getSuperclassType()!= null ) && 
				(node.getSuperclassType().toString().equalsIgnoreCase( "TestCase" ))){
		  currentRewriter.remove(node.getSuperclassType(), description);
		}
		// Remove super class TestCase for junit tests  - C# TestCase is obsolete  <--
		
		//		
		final String handler = context.getHandlerFromDoc(node, false);
		try {
			final ClassInfo ci = context.getModel().findClassInfo(handler,
					false, false, true);
			String targetFramework = context.getConfiguration().getOptions().getTargetDotNetFramework().name();
			if (ci != null && ci.getTarget(targetFramework) != null && 
					ci.getTarget(targetFramework).getChangeHierarchyDescriptor() != null) {
				ChangeHierarchyDescriptor descr = ci.getTarget(targetFramework).getChangeHierarchyDescriptor();
				if (descr.getSuperClass() != null) {
					final String superClassName = descr
							.getSuperClass().replace(":", ".");
					if (superClass != null) {
						final ASTNode replacement = currentRewriter.getAST()
								.newSimpleType(
										currentRewriter.getAST().newName(
												superClassName));
						currentRewriter.replace(superClass, replacement,
								description);
					} else {
						final ASTNode replacement = currentRewriter.getAST()
								.newSimpleType(
										currentRewriter.getAST().newName(
												superClassName));
						currentRewriter.set(node,
								TypeDeclaration.SUPERCLASS_TYPE_PROPERTY,
								replacement, description);
					}
				}
				if (interfaces != null && interfaces.size() > 0) {
					if (descr.getInterfaceToRemove() != null) {
						for (int i = 0; i < interfaces.size(); i++) {
							final Type type = (Type) interfaces.get(i);
							final String interfaceName = ASTNodes
									.asString(type);
							if (contains(descr
									.getInterfaceToRemove(), interfaceName, ci
									.getType().getJavaProject().getProject())) {
								currentRewriter.remove(type, description);
							}
						}
					}
					if (descr.getInterfaceToAdd() != null) {
						final ListRewrite lr = currentRewriter.getListRewrite(
								node,
								TypeDeclaration.SUPER_INTERFACE_TYPES_PROPERTY);
						for (int i = 0; i < descr
								.getInterfaceToAdd().size(); i++) {
							final String interfaceName = descr.getInterfaceToAdd()
									.get(i).replace(":", ".");
							final ASTNode newInterface = currentRewriter
									.getAST().newSimpleType(
											currentRewriter.getAST().newName(
													interfaceName));
							lr.insertLast(newInterface, description);
						}
					}
				} else {
					final ListRewrite lr = currentRewriter.getListRewrite(node,
							TypeDeclaration.SUPER_INTERFACE_TYPES_PROPERTY);
					for (int i = 0; i < descr
							.getInterfaceToAdd().size(); i++) {
						final String interfaceName = descr
								.getInterfaceToAdd().get(i).replace(":", ".");
						final ASTNode newInterface = currentRewriter.getAST()
								.newSimpleType(
										currentRewriter.getAST().newName(
												interfaceName));
						lr.insertLast(newInterface, description);
					}
				}
			}
		} catch (final Exception e) {
			context.getLogger().logException(
					"Error when tying to modify hierarchie call ", e);
		}
	}

	private boolean contains(List<String> list, String element,
			IProject reference) {
		for (final String e : list) {
			try {
				final String pName = getPackage(e);
				final String fqName = e.replace(":", ".");
				final PackageInfo pi = context.getModel().findPackageInfo(
						pName, reference);
				if (pi != null) {
					String targetFramework = context.getConfiguration().getOptions().getTargetDotNetFramework().name();
					final ClassInfo ci = pi.getClass(fqName);
					if (ci != null && ci.getTarget(targetFramework) != null) {
						final String dName = ci.getTarget(targetFramework).getShortName();
						if (dName.equals(element))
							return true;
					} else {
						if (fqName.equals(element))
							return true;
						else {
							final String cName = getClassName(e);
							if (cName.equals(element))
								return true;
						}
					}
				}
			} catch (final Exception ex) {
				ex.printStackTrace();
			}
		}
		return false;
	}

	private String getPackage(String element) {
		final int index = element.indexOf(":");
		return element.substring(0, index);
	}

	private String getClassName(String element) {
		final int index = element.indexOf(":");
		return element.substring(index + 1);
	}
}
