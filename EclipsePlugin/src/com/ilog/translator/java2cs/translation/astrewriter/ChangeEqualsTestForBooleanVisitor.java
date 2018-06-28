package com.ilog.translator.java2cs.translation.astrewriter;

import org.eclipse.jdt.core.dom.AST;
import org.eclipse.jdt.core.dom.ASTNode;
import org.eclipse.jdt.core.dom.Assignment;
import org.eclipse.jdt.core.dom.CastExpression;
import org.eclipse.jdt.core.dom.Expression;
import org.eclipse.jdt.core.dom.ITypeBinding;
import org.eclipse.jdt.core.dom.IVariableBinding;
import org.eclipse.jdt.core.dom.InfixExpression;
import org.eclipse.jdt.core.dom.MethodDeclaration;
import org.eclipse.jdt.core.dom.Name;
import org.eclipse.jdt.core.dom.NumberLiteral;
import org.eclipse.jdt.core.dom.QualifiedName;
import org.eclipse.jdt.core.dom.SimpleName;
import org.eclipse.jdt.core.dom.Type;
import org.eclipse.jdt.core.dom.VariableDeclarationFragment;
import org.eclipse.jdt.core.dom.VariableDeclarationStatement;
import org.eclipse.jdt.core.dom.InfixExpression.Operator;
import org.eclipse.jdt.internal.corext.dom.ASTNodes;
import org.eclipse.text.edits.TextEditGroup;

import com.ilog.translator.java2cs.translation.ITranslationContext;
import com.ilog.translator.java2cs.translation.util.TranslationUtils;

// Object a = Boolean.False;
// (a == Boolean.True)  -> a.equals(Boolean.True)
public class ChangeEqualsTestForBooleanVisitor extends ASTRewriterVisitor {

	//
	//
	//

	public ChangeEqualsTestForBooleanVisitor(ITranslationContext context) {
		super(context);
		transformerName = "Change Equals Test For Boolean";
		description = new TextEditGroup(transformerName);
	}

	//
	//
	//

	/**
	 * char a = 0; -> char a = (char) 0;
	 */
	@Override
	public void endVisit(VariableDeclarationStatement node) {
		if ((node.fragments() != null) && (node.fragments().size() > 0)) {
			final Type type = node.getType();
			final ITypeBinding typeB = type.resolveBinding();
			if (TranslationUtils.isCharType(typeB)) {
				final VariableDeclarationFragment frag = (VariableDeclarationFragment) node
						.fragments().get(0);
				final Expression init = frag.getInitializer();
				if (init != null) {
					switch (init.getNodeType()) {
					case ASTNode.NUMBER_LITERAL:
						final NumberLiteral nl = (NumberLiteral) init;
						if (!TranslationUtils.isCharType(nl
								.resolveTypeBinding())) {
							final AST ast = node.getAST();
							final CastExpression cast = ast.newCastExpression();
							cast.setType((Type) ASTNode.copySubtree(ast, type));
							cast.setExpression((Expression) ASTNode
									.copySubtree(ast, nl));
							currentRewriter.replace(init, cast, description);
						}
						break;
					}
				}
			}
		}
	}

	@Override
	public void endVisit(Assignment node) {
		final Expression left = node.getLeftHandSide();
		final Expression right = node.getRightHandSide();

		final ITypeBinding rType = right.resolveTypeBinding();
		final ITypeBinding lType = left.resolveTypeBinding();

		if (TranslationUtils.isCharType(left)
				&& !TranslationUtils.isCharType(right)) {
			final CastExpression cast = createCastToChar(right);
			currentRewriter.replace(right, cast, description);
		} else if (lType != null && rType != null
				&& TranslationUtils.isFloatType(lType)
				&& TranslationUtils.isDoubleType(rType)) {
			final CastExpression castE = createCastToFloat(right);
			currentRewriter.replace(right, castE, description);
		}
	}

	@Override
	public void endVisit(InfixExpression node) {
		final Expression left = node.getLeftOperand();
		final Expression right = node.getRightOperand();
		final ITypeBinding rType = right.resolveTypeBinding();
		final ITypeBinding lType = left.resolveTypeBinding();

		if ((rType != null) && (rType == lType)
				&& !TranslationUtils.isBooleanWrapper(rType)
				&& !rType.isTypeVariable()) {
			return;
		}

		if (((rType != null) && (rType.isNullType()))
				|| ((lType != null) && (lType.isNullType()))) {
			return;
		}

		if (rType == null || lType == null) {
			MethodDeclaration mDecl = (MethodDeclaration) ASTNodes.getParent(node, ASTNode.METHOD_DECLARATION);
			if (mDecl != null) {
				this.context.getLogger().logError("Error, that infix expression <" + node + "> in method " + 
						mDecl.getName().getIdentifier() + " is no more compilable." );
				return;
			}
		}
		
		final Operator op = node.getOperator();

		if (searchForBoolean(left) && !TranslationUtils.isBooleanWrapper(rType)) {
			final CastExpression castE = createCastToBoolean(right);
			currentRewriter.replace(right, castE, description);
		} else if (searchForBoolean(right)
				&& !TranslationUtils.isBooleanWrapper(lType)) {
			final CastExpression castE = createCastToBoolean(left);
			currentRewriter.replace(left, castE, description);
		} else if (!TranslationUtils.isPrimitiveOrWrapper(lType)
				&& !TranslationUtils.isPrimitiveOrWrapper(rType)
				&& (op == Operator.EQUALS || op == Operator.NOT_EQUALS)) {
			// pointer equality
			if (!TranslationUtils.isObjectType(lType)) {
				final CastExpression castLeft = createCastToObject(left);
				currentRewriter.replace(left, castLeft, description);
			}

			if (!TranslationUtils.isObjectType(rType)) {
				final CastExpression castRight = createCastToObject(right);
				currentRewriter.replace(right, castRight, description);
			}
		} else if (TranslationUtils.isObjectType(lType)
				&& TranslationUtils.isPrimitiveOrWrapper(rType)
				&& (op == Operator.EQUALS || op == Operator.NOT_EQUALS)) {
			final CastExpression castRight = createCastToObject(right);
			currentRewriter.replace(right, castRight, description);
		} else if (TranslationUtils.isObjectType(rType)
				&& TranslationUtils.isPrimitiveOrWrapper(lType)
				&& (op == Operator.EQUALS || op == Operator.NOT_EQUALS)) {
			// So right is a primitive type but left no ...
			// cast right into int
			final CastExpression castLeft = createCastToObject(left);
			currentRewriter.replace(left, castLeft, description);
		}
	}

	//
	//
	//

	private CastExpression createCastToObject(Expression expr) {
		final AST ast = expr.getAST();
		final CastExpression castLeft = ast.newCastExpression();
		castLeft.setExpression((Expression) currentRewriter
				.createCopyTarget(expr));
		castLeft.setType(ast.newSimpleType(ast
				.newName(TranslationUtils.FQN_OBJECT)));
		return castLeft;
	}

	private CastExpression createCastToBoolean(Expression expr) {
		final AST ast = expr.getAST();
		final CastExpression castLeft = ast.newCastExpression();
		castLeft.setExpression((Expression) currentRewriter
				.createCopyTarget(expr));
		castLeft.setType(ast.newSimpleType(ast
				.newName(TranslationUtils.FQN_BOOLEAN)));
		return castLeft;
	}

	private CastExpression createCastToFloat(Expression expr) {
		final AST ast = expr.getAST();
		final CastExpression castLeft = ast.newCastExpression();
		castLeft.setExpression((Expression) currentRewriter
				.createCopyTarget(expr));
		castLeft.setType(ast.newSimpleType(ast
				.newName(TranslationUtils.FQN_FLOAT)));
		return castLeft;
	}

	private CastExpression createCastToChar(Expression expr) {
		final AST ast = expr.getAST();
		final CastExpression castLeft = ast.newCastExpression();
		castLeft.setExpression((Expression) currentRewriter
				.createCopyTarget(expr));
		castLeft.setType(ast.newSimpleType(ast
				.newName(TranslationUtils.FQN_CHAR)));
		return castLeft;
	}

	private boolean searchForBoolean(Expression node) {
		if (node.getNodeType() == ASTNode.QUALIFIED_NAME) {
			final QualifiedName qName = (QualifiedName) node;
			final SimpleName sName = qName.getName();
			final Name qqName = qName.getQualifier();
			if (sName.resolveBinding() instanceof IVariableBinding) {
				final IVariableBinding varB = (IVariableBinding) sName
						.resolveBinding();
				final String fqName = varB.getType().getQualifiedName();
				return fqName.equals(TranslationUtils.FQN_BOOLEAN);
			}
			if (qqName.resolveBinding() instanceof ITypeBinding) {
				final ITypeBinding typeB = (ITypeBinding) qqName
						.resolveBinding();
				final String fqName = typeB.getQualifiedName();
				if (fqName.equals(TranslationUtils.FQN_BOOLEAN)) {
					return true;
				}
			}
		}
		return false;
	}

}
