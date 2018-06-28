package com.ilog.translator.java2cs.translation.astrewriter;

import java.util.List;

import org.eclipse.jdt.core.dom.ASTNode;
import org.eclipse.jdt.core.dom.Assignment;
import org.eclipse.jdt.core.dom.ClassInstanceCreation;
import org.eclipse.jdt.core.dom.DoStatement;
import org.eclipse.jdt.core.dom.Expression;
import org.eclipse.jdt.core.dom.IMethodBinding;
import org.eclipse.jdt.core.dom.ITypeBinding;
import org.eclipse.jdt.core.dom.IfStatement;
import org.eclipse.jdt.core.dom.InfixExpression;
import org.eclipse.jdt.core.dom.MethodDeclaration;
import org.eclipse.jdt.core.dom.MethodInvocation;
import org.eclipse.jdt.core.dom.PrefixExpression;
import org.eclipse.jdt.core.dom.ReturnStatement;
import org.eclipse.jdt.core.dom.VariableDeclarationFragment;
import org.eclipse.jdt.core.dom.WhileStatement;
import org.eclipse.jdt.internal.corext.dom.ASTNodes;
import org.eclipse.text.edits.TextEditGroup;

import com.ilog.translator.java2cs.translation.ITranslationContext;
import com.ilog.translator.java2cs.translation.util.TranslationUtils;

/**
 * Remove autoboxing
 * 
 * 
 * @author afau
 * 
 */
public class AutoboxingRemovalVisitor extends ASTRewriterVisitor {

	//
	//
	//

	public AutoboxingRemovalVisitor(ITranslationContext context) {
		super(context);
		transformerName = "Autoboxing Removal";
		description = new TextEditGroup(transformerName);
	}

	//
	//
	//

	@Override
	public void endVisit(WhileStatement node) {
		final Expression test = node.getExpression();
		final ITypeBinding type = test.resolveTypeBinding();

		if (TranslationUtils.isBooleanWrapper(type)) {
			unbox(test, type);
		}
	}

	@Override
	public void endVisit(IfStatement node) {
		final Expression test = node.getExpression();
		final ITypeBinding type = test.resolveTypeBinding();

		if (TranslationUtils.isBooleanWrapper(type)) {
			unbox(test, type);
		}
	}

	@Override
	public void endVisit(PrefixExpression node) {
		if (node.getOperator() == PrefixExpression.Operator.NOT) {
			final Expression operand = node.getOperand();
			final ITypeBinding type = operand.resolveTypeBinding();
			if (TranslationUtils.isBooleanWrapper(type)) {
				unbox(operand, type);
			}
		}
	}

	@Override
	public void endVisit(DoStatement node) {
		final Expression test = node.getExpression();
		final ITypeBinding type = test.resolveTypeBinding();

		if (TranslationUtils.isBooleanWrapper(type)) {
			unbox(test, type);
		}
	}

	@Override
	public void endVisit(VariableDeclarationFragment node) {
		final Expression initializer = node.getInitializer();
		if (initializer != null && node.resolveBinding() != null) {
			final ITypeBinding type = node.resolveBinding().getType();
			final ITypeBinding iType = initializer.resolveTypeBinding();

			if (type.isPrimitive() && TranslationUtils.isWrapperOf(iType, type)) {
				unbox(initializer, iType);
			}
		}
	}

	@Override
	public void endVisit(InfixExpression node) {
		final Expression left = node.getLeftOperand();
		final Expression right = node.getRightOperand();

		final ITypeBinding rType = right.resolveTypeBinding();
		final ITypeBinding lType = left.resolveTypeBinding();

		if ((node.getOperator() == InfixExpression.Operator.CONDITIONAL_AND)
				|| (node.getOperator() == InfixExpression.Operator.CONDITIONAL_OR)) {
			// left and right parts are boolean and must be primitive type
			if (!rType.isPrimitive()) {
				unbox(right, rType);
			}
			if (!lType.isPrimitive()) {
				unbox(left, lType);
			}
		} else if (lType.isPrimitive()
				&& TranslationUtils.isWrapperOf(rType, lType)) {
			unbox(right, rType);
		} else if (rType.isPrimitive()
				&& TranslationUtils.isWrapperOf(lType, rType)) {
			unbox(left, lType);
		}
	}

	@Override
	public void endVisit(Assignment node) {
		final Expression left = node.getLeftHandSide();
		final Expression right = node.getRightHandSide();

		final ITypeBinding rType = right.resolveTypeBinding();
		final ITypeBinding lType = left.resolveTypeBinding();

		if (lType.isPrimitive() && TranslationUtils.isWrapperOf(rType, lType)) {
			unbox(right, rType);
		}
	}

	@SuppressWarnings("unchecked")
	@Override
	public void endVisit(MethodInvocation node) {
		final List list = node.arguments();
		final IMethodBinding method = node.resolveMethodBinding();

		for (int i = 0; i < list.size(); i++) {
			final Expression e = (Expression) list.get(i);
			final ITypeBinding type = e.resolveTypeBinding();
			if (type != null && TranslationUtils.isWrapper(type)
					&& TranslationUtils.isWrapperOfCorrespondingParam(type, i, method)) {
				unbox(e, type);
			}
		}

	}

	@SuppressWarnings("unchecked")
	@Override
	public void endVisit(ClassInstanceCreation node) {
		final List list = node.arguments();
		final IMethodBinding method = node.resolveConstructorBinding();

		for (int i = 0; i < list.size(); i++) {
			final Expression e = (Expression) list.get(i);
			final ITypeBinding type = e.resolveTypeBinding();
			if (type != null && TranslationUtils.isWrapper(type)
					&& TranslationUtils.isWrapperOfCorrespondingParam(type, i, method)) {
				unbox(e, type);
			}
		}

	}

	@Override
	public void endVisit(ReturnStatement node) {
		if (node.getExpression() != null) {
			final ITypeBinding typeOfReturn = node.getExpression()
					.resolveTypeBinding();
			if (typeOfReturn != null
					&& TranslationUtils.isWrapper(typeOfReturn)) {
				final MethodDeclaration methodDecl = (MethodDeclaration) ASTNodes
						.getParent(node, ASTNode.METHOD_DECLARATION);
				final IMethodBinding method = methodDecl.resolveBinding();
				final ITypeBinding returnTypeOfMethod = method.getReturnType();
				if (returnTypeOfMethod.isPrimitive()
						&& TranslationUtils.isWrapperOf(typeOfReturn,
								returnTypeOfMethod)) {
					unbox(node.getExpression(), typeOfReturn);
				}
			}
		}
	}

	//
	//
	//

	private void unbox(Expression e, ITypeBinding wrapper) {
		String methodToCall = "";
		if (TranslationUtils.isIntegerWrapper(wrapper)) {
			methodToCall = ".intValue()";
		} else if (TranslationUtils.isLongWrapper(wrapper)) {
			methodToCall = ".longValue()";
		} else if (TranslationUtils.isCharacterWrapper(wrapper)) {
			methodToCall = ".charValue()";
		} else if (TranslationUtils.isBooleanWrapper(wrapper)) {
			methodToCall = ".booleanValue()";
		} else if (TranslationUtils.isDoubleWrapper(wrapper)) {
			methodToCall = ".doubleValue()";
		} else if (TranslationUtils.isFloatWrapper(wrapper)) {
			methodToCall = ".floatValue()";
		} else if (TranslationUtils.isShortWrapper(wrapper)) {
			methodToCall = ".shortValue()";
		} else if (TranslationUtils.isByteWrapper(wrapper)) {
			methodToCall = ".byteValue()";
		}
		final ASTNode replacement = currentRewriter.createStringPlaceholder("("
				+ ASTNodes.asString(e) + ")" + methodToCall,
				ASTNode.METHOD_INVOCATION);
		currentRewriter.replace(e, replacement, description);
	}
}
