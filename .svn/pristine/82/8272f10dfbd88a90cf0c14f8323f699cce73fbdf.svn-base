package com.ilog.translator.java2cs.translation.astrewriter;

import java.util.ArrayList;
import java.util.List;

import org.eclipse.jdt.core.IType;
import org.eclipse.jdt.core.JavaModelException;
import org.eclipse.jdt.core.dom.AST;
import org.eclipse.jdt.core.dom.ASTNode;
import org.eclipse.jdt.core.dom.Block;
import org.eclipse.jdt.core.dom.Initializer;
import org.eclipse.jdt.core.dom.Modifier;
import org.eclipse.jdt.core.dom.SimpleName;
import org.eclipse.jdt.core.dom.TypeDeclaration;
import org.eclipse.jdt.core.dom.Modifier.ModifierKeyword;
import org.eclipse.text.edits.TextEditGroup;

import com.ilog.translator.java2cs.configuration.info.ClassInfo;
import com.ilog.translator.java2cs.configuration.info.TranslationModelException;
import com.ilog.translator.java2cs.translation.ITranslationContext;

/**
 * Group static initializer
 * 
 * @author afau
 * 
 */
public class GroupStaticInitializerVisitor extends ASTRewriterVisitor {

	//
	//
	//

	public GroupStaticInitializerVisitor(ITranslationContext context) {
		super(context);
		transformerName = "Group Static initializer";
		description = new TextEditGroup(transformerName);
	}

	//
	//
	//

	@Override
	public void endVisit(TypeDeclaration node) {
		ClassInfo cinfo = null;

		try {
			cinfo = context.getModel().findClassInfo(
					node.resolveBinding().getJavaElement()
							.getHandleIdentifier(), false, false, true);
		} catch (final TranslationModelException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		} catch (final JavaModelException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
		// add it to all constructors
		if (!node.isInterface()) {
			final AST ast = node.getAST();
			final SimpleName newName = node.getName();

			final TypeDeclaration typeDecl = node;
			final IType fType = (IType) typeDecl.resolveBinding()
					.getJavaElement();

			final ASTNode[] initializer = getInitializer(ast, node);

			if (initializer != null && initializer.length > 0) {
				String targetFramework = context.getConfiguration().getOptions().getTargetDotNetFramework().name();
				
				if (cinfo == null || cinfo.getTarget(targetFramework) == null || 
						(cinfo.getTarget(targetFramework) != null && 
								!cinfo.getTarget(targetFramework).isRemoveStaticInitializers()))
					addToConstructor(node, initializer, ast, newName, typeDecl,
							fType);
			}
		}
	}

	//
	//
	//

	@SuppressWarnings("unchecked")
	private ASTNode[] getInitializer(AST ast, TypeDeclaration node) {
		final List result = new ArrayList();
		final List bodies = node.bodyDeclarations();
		for (int i = 0; i < bodies.size(); i++) {
			final ASTNode stat = (ASTNode) bodies.get(i);
			if (stat.getNodeType() == ASTNode.INITIALIZER) {
				final Initializer init = (Initializer) stat;
				if (Modifier.isStatic(init.getModifiers())) {
					result.addAll(ASTNode.copySubtrees(ast, init.getBody()
							.statements()));
					currentRewriter.remove(init, description);
				}
			}
		}
		return (ASTNode[]) result.toArray(new ASTNode[result.size()]);
	}

	@SuppressWarnings("unchecked")
	private void addToConstructor(TypeDeclaration node, ASTNode[] initializer,
			AST ast, SimpleName newName, TypeDeclaration typeDecl, IType fType) {
		final Initializer oneInitializer = ast.newInitializer();
		oneInitializer.modifiers().add(
				ast.newModifier(ModifierKeyword.STATIC_KEYWORD));
		;
		final Block body = ast.newBlock();
		//					
		for (final ASTNode element : initializer) {
			final ASTNode copyOfStatement = ASTNode.copySubtree(ast, element);
			body.statements().add(copyOfStatement);
		}

		//
		oneInitializer.setBody(body);

		currentRewriter.getListRewrite(typeDecl,
				typeDecl.getBodyDeclarationsProperty()).insertLast(
				oneInitializer, description);
	}
}
