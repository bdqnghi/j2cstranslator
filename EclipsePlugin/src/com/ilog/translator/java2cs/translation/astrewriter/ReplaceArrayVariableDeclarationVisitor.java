package com.ilog.translator.java2cs.translation.astrewriter;

import java.util.List;

import org.eclipse.jdt.core.dom.AST;
import org.eclipse.jdt.core.dom.ASTNode;
import org.eclipse.jdt.core.dom.ArrayCreation;
import org.eclipse.jdt.core.dom.ArrayInitializer;
import org.eclipse.jdt.core.dom.ArrayType;
import org.eclipse.jdt.core.dom.Block;
import org.eclipse.jdt.core.dom.Expression;
import org.eclipse.jdt.core.dom.FieldDeclaration;
import org.eclipse.jdt.core.dom.ITypeBinding;
import org.eclipse.jdt.core.dom.Javadoc;
import org.eclipse.jdt.core.dom.Name;
import org.eclipse.jdt.core.dom.PrimitiveType;
import org.eclipse.jdt.core.dom.QualifiedName;
import org.eclipse.jdt.core.dom.SimpleName;
import org.eclipse.jdt.core.dom.SimpleType;
import org.eclipse.jdt.core.dom.SingleVariableDeclaration;
import org.eclipse.jdt.core.dom.Type;
import org.eclipse.jdt.core.dom.TypeDeclaration;
import org.eclipse.jdt.core.dom.VariableDeclaration;
import org.eclipse.jdt.core.dom.VariableDeclarationExpression;
import org.eclipse.jdt.core.dom.VariableDeclarationFragment;
import org.eclipse.jdt.core.dom.VariableDeclarationStatement;
import org.eclipse.jdt.core.dom.rewrite.ListRewrite;
import org.eclipse.text.edits.TextEditGroup;

import com.ilog.translator.java2cs.translation.ITranslationContext;

public class ReplaceArrayVariableDeclarationVisitor extends ASTRewriterVisitor {

	// TODO: Warning change 'char[]' into 'int' in method declaration
	//
	//

	public ReplaceArrayVariableDeclarationVisitor(ITranslationContext context) {
		super(context);
		transformerName = "Replace Array Variable declaration";
		description = new TextEditGroup(transformerName);
	}

	//
	//
	//

	@Override
	public void endVisit(ArrayCreation node) {
		final ITypeBinding arrayType = node.resolveTypeBinding();
		if (arrayType.isArray()) {
			if (arrayType.getDimensions() > 1) {
				final ArrayInitializer init = node.getInitializer();
				if (init != null) {
					ArrayInitializer newInitializer = rewriteInitializer(node.getType(), init);
					if (newInitializer != null) {
						currentRewriter.replace(init, newInitializer, description);
					}
				}
			}
		}
	}

	@SuppressWarnings("unchecked")
	@Override
	public void endVisit(SingleVariableDeclaration node) {
		if (node.getExtraDimensions() >= 1) {
			final List modifiers = node.modifiers();
			final AST ast = node.getAST();

			final List newModifiers = ASTNode.copySubtrees(ast, modifiers);

			rewriteFragment(node, node.getType(), ast, newModifiers, node);
		}
	}

	@SuppressWarnings("unchecked")
	@Override
	public void endVisit(VariableDeclarationStatement node) {
		final List fragments = node.fragments();
		final List modifiers = node.modifiers();
		rewriteFragments(node, modifiers, node.getType(), fragments);
	}

	@SuppressWarnings("unchecked")
	@Override
	public void endVisit(VariableDeclarationExpression node) {
		final List fragments = node.fragments();
		final List modifiers = node.modifiers();
		rewriteFragments(node, modifiers, node.getType(), fragments);
	}

	@SuppressWarnings("unchecked")
	@Override
	public void endVisit(FieldDeclaration node) {
		final List fragments = node.fragments();
		final List modifiers = node.modifiers();
		rewriteFragments(node, modifiers, node.getType(), fragments);
	}

	//
	//
	//

	@SuppressWarnings("unchecked")
	private void rewriteFragments(ASTNode node, List modifiers, Type type,
			List fragments) {
		final AST ast = node.getAST();

		final List newModifiers = ASTNode.copySubtrees(ast, modifiers);

		final int size = fragments.size();

		if (size > 1) {
			for (int i = 0; i < size; i++) {
				final VariableDeclaration fragment = (VariableDeclaration) fragments
						.get(i);
				if (fragment.getExtraDimensions() > 0) {
					final ASTNode newField = createNewField(node, type,
							newModifiers, ast, fragment);
					if (newField != null) {
						currentRewriter.remove(fragment, description);
						node.getLocationInParent();
						//
						if (node.getParent() instanceof Block) {
							final Block block = (Block) node.getParent();
							final ListRewrite list = currentRewriter
									.getListRewrite(block,
											Block.STATEMENTS_PROPERTY);
							list.insertAfter(newField, node, null);
						} else if (node.getParent() instanceof TypeDeclaration) {
							final TypeDeclaration block = (TypeDeclaration) node
									.getParent();
							final ListRewrite list = currentRewriter
									.getListRewrite(
											block,
											TypeDeclaration.BODY_DECLARATIONS_PROPERTY);
							list.insertAfter(newField, node, null);
						}
					}
				}
			}
		} else if (size > 0) {
			final VariableDeclaration fragment = (VariableDeclaration) fragments
					.get(0);
			if (fragment.getExtraDimensions() >= 1) {
				rewriteFragment(node, type, ast, newModifiers, fragment);
			} else {
				final Expression expr = fragment.getInitializer();
				if ((expr != null)
						&& (expr.getNodeType() == ASTNode.ARRAY_INITIALIZER)
						&& type.isArrayType()
						&& ((ArrayType) type).getComponentType().isArrayType()) {
					final ArrayInitializer newInitializer = rewriteInitializer(
							(ArrayType) type, (ArrayInitializer) expr);
					currentRewriter.replace(expr, newInitializer, description);
				}
			}
		}
	}

	//
	// int[][] a = new int[][] { { 1,2 }, { 2, 3}}
	// =>
	// int[][] a = new int[][] { new int[]{ 1,2 }, new int[] { 2, 3}}
	//
	@SuppressWarnings("unchecked")
	private ArrayInitializer rewriteInitializer(ArrayType type,
			ArrayInitializer expr) {
		final AST ast = expr.getAST();
		if (!type.getComponentType().isArrayType()) {
			return (ArrayInitializer) ASTNode.copySubtree(ast, expr);
		}
		// ArrayInitializer aInit = expr;
		final List exprs = expr.expressions();
		final ArrayInitializer newInitializer = ast.newArrayInitializer();
		//
		for (final Object exp : exprs) {
			final ASTNode e = (ASTNode) exp;
			final ArrayCreation subArray = ast.newArrayCreation();
			try {
				if (e.getNodeType() == ASTNode.ARRAY_INITIALIZER) {
					subArray.setType((ArrayType) ASTNode.copySubtree(ast, type
							.getComponentType()));
					subArray.setInitializer((ArrayInitializer) ASTNode
							.copySubtree(ast, e));
					newInitializer.expressions().add(subArray);
				} else {
					newInitializer.expressions().add(
							ASTNode.copySubtree(ast, e));
				}
			} catch (final Exception ex) {
				ex.printStackTrace();
				context.getLogger().logException("", ex);
			}
		}
		return newInitializer;
	}

	@SuppressWarnings("unchecked")
	private void rewriteFragment(ASTNode node, Type type, AST ast,
			List newModifiers, VariableDeclaration fragment) {
		final ASTNode newField = createNewField(node, type, newModifiers, ast,
				fragment);
		if (newField != null) {
			currentRewriter.replace(node, newField, description);
		}
	}

	@SuppressWarnings("unchecked")
	private ASTNode createNewField(ASTNode node, Type type, List modifiers,
			AST ast, VariableDeclaration fragment) {
		Type elementType = null;
		int dim = 0;
		if (type.isSimpleType()) {
			final SimpleType sType = (SimpleType) type;
			Name sName = null;
			if (sType.getName().isQualifiedName()) {
				final QualifiedName qualName = (QualifiedName) ASTNode
						.copySubtree(ast, sType.getName());
				sName = qualName;
			} else {
				sName = ast.newSimpleName(sType.getName()
						.getFullyQualifiedName());
			}
			elementType = ast.newSimpleType(sName);
		} else if (type.isPrimitiveType()) {
			final PrimitiveType sType = (PrimitiveType) type;
			elementType = ast.newPrimitiveType(sType.getPrimitiveTypeCode());
		} else if (type.isArrayType()) {
			final ArrayType aType = (ArrayType) type;
			dim = aType.getDimensions();
			elementType = (Type) ASTNode.copySubtree(currentRewriter.getAST(),
					aType.getElementType());
		}
		final ArrayType newType = ast.newArrayType(elementType, fragment
				.getExtraDimensions()
				+ dim);
		//
		final Expression expr = fragment.getInitializer();
		Expression newExpression = null;
		if ((expr != null) && (expr.getNodeType() == ASTNode.ARRAY_INITIALIZER)) {
			newExpression = rewriteInitializer(newType, (ArrayInitializer) expr);
		} else {
			newExpression = (Expression) ASTNode.copySubtree(ast, fragment
					.getInitializer());
		}
		final SimpleName newFName = ast.newSimpleName(fragment.getName()
				.getFullyQualifiedName());

		VariableDeclaration newFragment = null;
		ASTNode results = null;

		if (node instanceof VariableDeclarationStatement) {
			newFragment = ast.newVariableDeclarationFragment();
			final VariableDeclarationStatement newField = ast
					.newVariableDeclarationStatement((VariableDeclarationFragment) newFragment);
			newField.setType(newType);
			newField.modifiers().addAll(modifiers);
			results = newField;
		} else if (node instanceof VariableDeclarationExpression) {
			newFragment = ast.newVariableDeclarationFragment();
			final VariableDeclarationExpression newField = ast
					.newVariableDeclarationExpression((VariableDeclarationFragment) newFragment);
			newField.setType(newType);
			newField.modifiers().addAll(modifiers);
			results = newField;
		} else if (node instanceof FieldDeclaration) {
			final FieldDeclaration oldField = (FieldDeclaration) node;
			newFragment = ast.newVariableDeclarationFragment();
			final FieldDeclaration newField = ast
					.newFieldDeclaration((VariableDeclarationFragment) newFragment);
			newField.setType(newType);
			newField.setJavadoc((Javadoc) ASTNode.copySubtree(ast, oldField
					.getJavadoc()));
			newField.modifiers().addAll(modifiers);
			results = newField;
		} else if (node instanceof SingleVariableDeclaration) {
			final SingleVariableDeclaration nsv = ast
					.newSingleVariableDeclaration();
			nsv.setType(newType);
			nsv.setName(newFName);
			nsv.setInitializer(newExpression);
			return nsv;
		} else {
			context.getLogger().logError(
					transformerName + " cas non prevu " + node);
		}

		newFragment.setExtraDimensions(0);
		newFragment.setName(newFName);
		newFragment.setInitializer(newExpression);

		return results;
	}
}
