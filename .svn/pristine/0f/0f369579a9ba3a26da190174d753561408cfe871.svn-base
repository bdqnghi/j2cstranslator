package com.ilog.translator.java2cs.translation.astrewriter;

import org.eclipse.jdt.core.JavaModelException;
import org.eclipse.jdt.core.dom.ASTNode;
import org.eclipse.jdt.core.dom.EnumDeclaration;
import org.eclipse.jdt.core.dom.IBinding;
import org.eclipse.jdt.core.dom.ITypeBinding;
import org.eclipse.jdt.core.dom.IVariableBinding;
import org.eclipse.jdt.core.dom.ImportDeclaration;
import org.eclipse.jdt.core.dom.Name;
import org.eclipse.jdt.core.dom.QualifiedName;
import org.eclipse.jdt.core.dom.SimpleName;
import org.eclipse.jdt.internal.corext.dom.ASTNodes;
import org.eclipse.text.edits.TextEditGroup;

import com.ilog.translator.java2cs.configuration.target.TargetClass;
import com.ilog.translator.java2cs.configuration.target.TargetPackage;
import com.ilog.translator.java2cs.translation.ITranslationContext;
import com.ilog.translator.java2cs.translation.util.TranslationUtils;

/**
 * Check if an access to a field is non ambigues (csharp way) means the there is
 * no class with the same short name than the class that contains the field
 * 
 * @author afau
 * 
 */
public class FullyQualifiedEnumInvocationVisitor extends ASTRewriterVisitor {

	//
	//
	//

	public FullyQualifiedEnumInvocationVisitor(ITranslationContext context) {
		super(context);
		transformerName = "Fully Qualified Enum access";
		description = new TextEditGroup(transformerName);
	}

	//
	//
	//

	@Override
	public boolean visit(EnumDeclaration node) {
		return false;
	}

	@Override
	public boolean visit(QualifiedName node) {
		return false;
	}

	@Override
	public void endVisit(SimpleName node) {
		final IBinding binding = node.resolveBinding();
		if (ASTNodes.getParent(node, ASTNode.IMPORT_DECLARATION) == null
				&& (binding != null)
				&& (binding.getKind() == IBinding.VARIABLE)) {
			final IVariableBinding vb = (IVariableBinding) binding;
			fqnEnumAccess(node, vb, node.getFullyQualifiedName());
		}
	}

	@Override
	public void endVisit(ImportDeclaration node) {
		if (node.isStatic()) {
			currentRewriter.remove(node, description);
		}
	}

	//
	//
	//

	private void fqnEnumAccess(ASTNode node, IVariableBinding vb,
			String fieldName) {
		final ITypeBinding declaringTypeB = vb.getDeclaringClass();
		// ITypeBinding enclosingTypeB = ASTNodes.getEnclosingType(node);
		if ((declaringTypeB != null) && declaringTypeB.isEnum()) {
			// declaringTypeB
			try {
				if (declaringTypeB.isMember()
						&& declaringTypeB.getDeclaringClass()
								.getQualifiedName().equals(
										fCu.getTypes()[0]
												.getFullyQualifiedName())) {
					final String fqTypeName = declaringTypeB.getName();
					String replacement = "";

					replacement = "/* insert_here:" + fqTypeName + ". */"
							+ fieldName;
					// }
					final Name replacementNode = (Name) currentRewriter
							.createStringPlaceholder(replacement,
									ASTNode.QUALIFIED_NAME);
					currentRewriter.replace(node, replacementNode, description);
				} else {
					//final String fqTypeName = Bindings
					//		.getFullyQualifiedName(declaringTypeB);
					String replacement = "";
					String typeName = declaringTypeB.getName();
					ITypeBinding current = declaringTypeB.getDeclaringClass();
					while (current != null) {
						typeName = current.getName() + "." + typeName;
						current = current.getDeclaringClass();
					}

					String pck = declaringTypeB.getPackage().getJavaElement()
							.getElementName();
					final TargetPackage mappedPck = context.getModel()
							.findPackageMapping(
									pck,
									declaringTypeB.getPackage()
											.getJavaElement().getJavaProject()
											.getProject());

					final TargetClass tc = context.getModel().findClassMapping(
							declaringTypeB.getJavaElement()
									.getHandleIdentifier(), false, false);
					if (tc != null && tc.getName() != null) {
						replacement = "/* insert_here:" + tc.getName() + ". */"
								+ fieldName;
					} else if (mappedPck != null)
						replacement = "/* insert_here:" + mappedPck.getName()
								+ "." + typeName + ". */" + fieldName;
					else {
						pck = TranslationUtils.defaultMappingForPackage(
								context, pck, declaringTypeB.getPackage()
										.getJavaElement().getJavaProject()
										.getProject());
						replacement = "/* insert_here:" + pck + "." + typeName
								+ ". */" + fieldName;
					}
					final Name replacementNode = (Name) currentRewriter
							.createStringPlaceholder(replacement,
									ASTNode.QUALIFIED_NAME);
					currentRewriter.replace(node, replacementNode, null);
				}
			} catch (final JavaModelException e) {
				e.printStackTrace();
				context.getLogger().logException("", e);
			}
		}
	}
}
