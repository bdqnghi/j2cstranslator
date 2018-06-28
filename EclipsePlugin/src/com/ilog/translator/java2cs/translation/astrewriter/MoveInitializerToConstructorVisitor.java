package com.ilog.translator.java2cs.translation.astrewriter;

import java.util.ArrayList;
import java.util.Iterator;
import java.util.List;

import org.eclipse.jdt.core.IType;
import org.eclipse.jdt.core.JavaModelException;
import org.eclipse.jdt.core.dom.AST;
import org.eclipse.jdt.core.dom.ASTNode;
import org.eclipse.jdt.core.dom.Block;
import org.eclipse.jdt.core.dom.Initializer;
import org.eclipse.jdt.core.dom.MethodDeclaration;
import org.eclipse.jdt.core.dom.Modifier;
import org.eclipse.jdt.core.dom.SimpleName;
import org.eclipse.jdt.core.dom.SingleVariableDeclaration;
import org.eclipse.jdt.core.dom.SuperConstructorInvocation;
import org.eclipse.jdt.core.dom.TypeDeclaration;
import org.eclipse.jdt.internal.corext.refactoring.util.JavaElementUtil;
import org.eclipse.text.edits.TextEditGroup;

import com.ilog.translator.java2cs.translation.ITranslationContext;

/**
 * Move non static initializer to constructor.
 * 
 * @author afau
 * 
 */
public class MoveInitializerToConstructorVisitor extends ASTRewriterVisitor {

	//
	//
	//

	public MoveInitializerToConstructorVisitor(ITranslationContext context) {
		super(context);
		transformerName = "Move initializer to constructor";
		description = new TextEditGroup(transformerName);
	}

	//
	//
	//

	@Override
	public void endVisit(TypeDeclaration node) {
		// add it to all constructors
		if (!node.isInterface()) {
			final AST ast = node.getAST();
			final SimpleName newName = node.getName();

			final TypeDeclaration typeDecl = node;
			final IType fType = (IType) typeDecl.resolveBinding()
					.getJavaElement();

			final ASTNode[] initializer = getInitializer(ast, node);

			if (initializer != null && initializer.length > 0)
				addToConstructor(node, initializer, ast, newName, typeDecl,
						fType);
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
				if (!Modifier.isStatic(init.getModifiers())) {
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
		try {
			if (JavaElementUtil.getAllConstructors(fType).length == 0) {
				final MethodDeclaration constructor = ast
						.newMethodDeclaration();
				constructor.setConstructor(true);
				constructor.setName((SimpleName) ASTNode.copySubtree(ast,
						newName));
				;
				final Block body = ast.newBlock();
				//					
				final int paramCount = constructor.parameters().size();
				if (paramCount > 0) {
					// hum hum
					final SuperConstructorInvocation superConstructorInvocation = ast
							.newSuperConstructorInvocation();
					for (int i = 0; i < paramCount; i++) {
						final SingleVariableDeclaration param = (SingleVariableDeclaration) constructor
								.parameters().get(i);
						superConstructorInvocation.arguments().add(
								ast.newSimpleName(param.getName()
										.getIdentifier()));
					}
					body.statements().add(superConstructorInvocation);
				}
				for (final ASTNode element : initializer) {
					final ASTNode copyOfStatement = ASTNode.copySubtree(ast,
							element);
					body.statements().add(copyOfStatement);
				}

				//
				constructor.setBody(body);
				constructor
						.modifiers()
						.add(
								ast
										.newModifier(Modifier.ModifierKeyword.PUBLIC_KEYWORD));
				currentRewriter.getListRewrite(typeDecl,
						typeDecl.getBodyDeclarationsProperty()).insertFirst(
						constructor, description);
			} else {
				for (final Iterator iter = typeDecl.bodyDeclarations()
						.iterator(); iter.hasNext();) {
					final ASTNode astNode = (ASTNode) iter.next();
					if (astNode.getNodeType() == ASTNode.METHOD_DECLARATION) {
						final MethodDeclaration mDecl = (MethodDeclaration) astNode;
						if (mDecl.isConstructor()) {
							final ASTNode superOrThis = getThisOrSuper(mDecl
									.getBody());
							for (final ASTNode element : initializer) {
								final ASTNode copyOfStatement = ASTNode
										.copySubtree(ast, element);
								if (superOrThis != null) {
									currentRewriter.getListRewrite(
											mDecl.getBody(),
											Block.STATEMENTS_PROPERTY)
											.insertAfter(copyOfStatement,
													superOrThis, description);
								} else
									currentRewriter.getListRewrite(
											mDecl.getBody(),
											Block.STATEMENTS_PROPERTY)
											.insertFirst(copyOfStatement,
													description);
							}
						}
					}
				}
			}

		} catch (final JavaModelException e) {
			context.getLogger().logException("", e);
		}
	}

	@SuppressWarnings("unchecked")
	private ASTNode getThisOrSuper(Block body) {
		final List<ASTNode> stats = body.statements();
		if (stats != null && stats.size() > 0) {
			final ASTNode first = stats.get(0);
			if ((first.getNodeType() == ASTNode.CONSTRUCTOR_INVOCATION)
					|| (first.getNodeType() == ASTNode.SUPER_CONSTRUCTOR_INVOCATION))
				return first;
		}
		return null;
	}
}
