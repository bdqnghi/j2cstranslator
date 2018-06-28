package com.ilog.translator.java2cs.translation.astrewriter;

import java.text.MessageFormat;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

import org.eclipse.core.runtime.CoreException;
import org.eclipse.core.runtime.IProgressMonitor;
import org.eclipse.core.runtime.NullProgressMonitor;
import org.eclipse.jdt.core.dom.AST;
import org.eclipse.jdt.core.dom.ASTNode;
import org.eclipse.jdt.core.dom.ArrayCreation;
import org.eclipse.jdt.core.dom.ArrayInitializer;
import org.eclipse.jdt.core.dom.Assignment;
import org.eclipse.jdt.core.dom.Block;
import org.eclipse.jdt.core.dom.CastExpression;
import org.eclipse.jdt.core.dom.ClassInstanceCreation;
import org.eclipse.jdt.core.dom.Expression;
import org.eclipse.jdt.core.dom.FieldAccess;
import org.eclipse.jdt.core.dom.IBinding;
import org.eclipse.jdt.core.dom.ITypeBinding;
import org.eclipse.jdt.core.dom.IVariableBinding;
import org.eclipse.jdt.core.dom.InfixExpression;
import org.eclipse.jdt.core.dom.LabeledStatement;
import org.eclipse.jdt.core.dom.MethodInvocation;
import org.eclipse.jdt.core.dom.Name;
import org.eclipse.jdt.core.dom.QualifiedName;
import org.eclipse.jdt.core.dom.SimpleName;
import org.eclipse.jdt.core.dom.SimpleType;
import org.eclipse.jdt.core.dom.SingleVariableDeclaration;
import org.eclipse.jdt.core.dom.SuperConstructorInvocation;
import org.eclipse.jdt.core.dom.SuperMethodInvocation;
import org.eclipse.jdt.core.dom.SynchronizedStatement;
import org.eclipse.jdt.core.dom.Type;
import org.eclipse.jdt.core.dom.VariableDeclarationExpression;
import org.eclipse.jdt.core.dom.VariableDeclarationStatement;
import org.eclipse.jdt.core.dom.Assignment.Operator;
import org.eclipse.jdt.core.dom.rewrite.ListRewrite;
import org.eclipse.jdt.internal.corext.dom.ASTNodes;
import org.eclipse.ltk.core.refactoring.Change;
import org.eclipse.text.edits.TextEditGroup;

import com.ilog.translator.java2cs.configuration.info.ClassInfo;
import com.ilog.translator.java2cs.configuration.target.TargetClass;
import com.ilog.translator.java2cs.translation.ITranslationContext;
import com.ilog.translator.java2cs.translation.noderewriter.INodeRewriter;
import com.ilog.translator.java2cs.translation.util.TranslationUtils;

/**
 * Replace field access and method invocation.
 * 
 * @author afau
 *
 */
public class ComputeFieldAccessAndMethodInvocationVisitor extends
		ASTRewriterVisitor {

	// BR2071447 : patch from faron
	// TODO: externalize it !
	private static final Map<String, String> MAP_RIGHT_SHIFT_UNSIGNED = new HashMap<String, String>();
	static {
		MAP_RIGHT_SHIFT_UNSIGNED.put("byte", "(int) (((byte) {0}) >> {1})");
		MAP_RIGHT_SHIFT_UNSIGNED.put("java.lang.Byte",
				"(int) (((byte) {0}) >> {1})");

		MAP_RIGHT_SHIFT_UNSIGNED.put("char", "{0} >> {1}");
		MAP_RIGHT_SHIFT_UNSIGNED.put("java.lang.Character",
				"(int) ({0} >> {1})");

		MAP_RIGHT_SHIFT_UNSIGNED.put("int", "(int) (((uint) {0}) >> {1})");
		MAP_RIGHT_SHIFT_UNSIGNED.put("java.lang.Integer",
				"(int) (((uint) {0}) >> {1})");

		MAP_RIGHT_SHIFT_UNSIGNED.put("long", "(long) (((ulong) {0}) >> {1})");
		MAP_RIGHT_SHIFT_UNSIGNED.put("java.lang.Long",
				"(long) (((ulong) {0}) >> {1})");

		MAP_RIGHT_SHIFT_UNSIGNED.put("short", "(int) (((ushort) {0}) >> {1})");
		MAP_RIGHT_SHIFT_UNSIGNED.put("java.lang.Short",
				"(int) (((ushort) {0}) >> {1})");
	}

	// BR2071447 : patch from faron
	// TODO: externalize it !
	private static final Map<String, String> MAP_RIGHT_SHIFT_UNSIGNED_ASSIGN = new HashMap<String, String>();
	static {
		MAP_RIGHT_SHIFT_UNSIGNED_ASSIGN.put("byte",
				"(sbyte) (((byte) {0}) >> {1})");
		MAP_RIGHT_SHIFT_UNSIGNED_ASSIGN.put("java.lang.Byte",
				"(sbyte) (((byte) {0}) >> {1})");

		MAP_RIGHT_SHIFT_UNSIGNED_ASSIGN.put("char", "(char) ({0} >> {1})");
		MAP_RIGHT_SHIFT_UNSIGNED_ASSIGN.put("java.lang.Character",
				"(char) ({0} >> {1})");

		MAP_RIGHT_SHIFT_UNSIGNED_ASSIGN.put("int",
				"(int) (((uint) {0}) >> {1})");
		MAP_RIGHT_SHIFT_UNSIGNED_ASSIGN.put("java.lang.Integer",
				"(int) (((uint) {0}) >> {1})");

		MAP_RIGHT_SHIFT_UNSIGNED_ASSIGN.put("long",
				"(long) (((ulong) {0}) >> {1})");
		MAP_RIGHT_SHIFT_UNSIGNED_ASSIGN.put("java.lang.Long",
				"(long) (((ulong) {0}) >> {1})");

		MAP_RIGHT_SHIFT_UNSIGNED_ASSIGN.put("short",
				"(short) (((ushort) {0}) >> {1})");
		MAP_RIGHT_SHIFT_UNSIGNED_ASSIGN.put("java.lang.Short",
				"(short) (((ushort) {0}) >> {1})");
	}

	//
	// Compute Field Access And Method Invocation replacement
	// 

	public ComputeFieldAccessAndMethodInvocationVisitor(
			ITranslationContext context) {
		super(context);
		transformerName = "Replace Fields Access and Methods invocation";
		description = new TextEditGroup(transformerName);
	}

	//
	//
	//

	@Override
	public boolean applyChange(IProgressMonitor pm) throws CoreException {
		final Change change = createChange(pm, null);
		if (change != null) {
			context.addChange(fCu, change);
		}
		return true;
	}

	//
	//
	//

	@Override
	public void endVisit(CastExpression node) {
		final Type type = node.getType();
		if (type.isSimpleType()) {
			final INodeRewriter result = context.getMapper().mapSimpleType(fCu,
					(SimpleType) node.getType());
			applyNodeRewriter(node, result, description);
		}
	}

	@Override
	public boolean visit(SynchronizedStatement node) {
		final Block body = node.getBody();
		// don't visit the expr because in some case (.class) that break
		// the rest if the tree !
		body.accept(this);
		return false;
	}

	@Override
	public void endVisit(SynchronizedStatement node) {
		final Expression expr = node.getExpression();
		if (expr.getNodeType() == ASTNode.SIMPLE_NAME) {
			final INodeRewriter result = context.getMapper().mapFieldAccess(
					(SimpleName) expr);
			applyNodeRewriter(expr, result, description);
		}
	}

	@Override
	public void endVisit(InfixExpression node) {
		if (node.getOperator() == InfixExpression.Operator.RIGHT_SHIFT_UNSIGNED) {
			// BR2071447 : patch from faron
			final Expression lhs = node.getLeftOperand();
			final Expression rhs = node.getRightOperand();

			final ITypeBinding type = lhs.resolveTypeBinding();
			if (type != null) {
				final String qname = type.getQualifiedName();

				// char is a special case
				// char is unsigned in both languages
				// just change the operator
				if ("char".equals(qname) || "java.lang.Character".equals(qname)) {
					currentRewriter.set(node,
							InfixExpression.OPERATOR_PROPERTY,
							InfixExpression.Operator.RIGHT_SHIFT_SIGNED,
							description);
					return;
				}

				final String format = MAP_RIGHT_SHIFT_UNSIGNED.get(qname);
				if (format == null) {
					// This happens if the user has a compilation error.
					// For example, attempting to shift a non-integral
					// value. Leave the expression as it is but add a
					// comment to inform the user why it did not translate.
					final String body = "/* Cannot translate the following expression because the left operand is not an intergral value. */"
							+ ASTNodes.asString(node);
					final InfixExpression newNode = (InfixExpression) currentRewriter
							.createStringPlaceholder(body,
									ASTNode.INFIX_EXPRESSION);
					currentRewriter.replace(node, newNode, description);
				}

				final String body = MessageFormat.format(format, ASTNodes
						.asString(lhs), ASTNodes.asString(rhs));
				final CastExpression newNode = (CastExpression) currentRewriter
						.createStringPlaceholder(body, ASTNode.CAST_EXPRESSION);
				currentRewriter.replace(node, newNode, description);
			} else {
				// This only happens if we specified not to perform type
				// resolution when creating the AST, which is an internal
				// error. It may happen at other times but I have not seen
				// it.
				// FIXME: Wrap a CoreException in a RuntimeException.
				// The RuntimeException stops the operation and the
				// translator may use the CoreException to notify the
				// user that an internal error occurred.
				throw new RuntimeException("Type binding failed.");
			}
		}
	}

	@Override
	public void endVisit(Assignment node) {
		if (node.getOperator() == Assignment.Operator.RIGHT_SHIFT_UNSIGNED_ASSIGN) {
			// BR2071447 : patch from faron
			final AST ast = currentRewriter.getAST();
			final Expression lhs = node.getLeftHandSide();
			final Expression rhs = node.getRightHandSide();

			final ITypeBinding type = lhs.resolveTypeBinding();
			if (type != null) {
				final String qname = type.getQualifiedName();

				// char is a special case
				// char is unsigned in both languages
				// just change the operator
				if ("char".equals(qname) || "java.lang.Character".equals(qname)) {
					currentRewriter.set(node, Assignment.OPERATOR_PROPERTY,
							Assignment.Operator.RIGHT_SHIFT_SIGNED_ASSIGN,
							description);
					return;
				}

				final String format = MAP_RIGHT_SHIFT_UNSIGNED_ASSIGN
						.get(qname);
				if (format == null) {
					// This happens if the user has a compilation error.
					// For example, attempting to shift a non-integral
					// value. Leave the expression as it is but add a
					// comment to inform the user why it did not translate.
					final String body = "/* Cannot translate the following expression because the left hand side is not an intergral value. */"
							+ ASTNodes.asString(node);
					final Assignment newNode = (Assignment) currentRewriter
							.createStringPlaceholder(body, ASTNode.ASSIGNMENT);
					currentRewriter.replace(node, newNode, description);
				}

				final String body = MessageFormat.format(format, ASTNodes
						.asString(lhs), ASTNodes.asString(rhs));
				final CastExpression newRHS = (CastExpression) currentRewriter
						.createStringPlaceholder(body, ASTNode.CAST_EXPRESSION);

				final Assignment newAssignement = ast.newAssignment();
				newAssignement.setOperator(Operator.ASSIGN);
				newAssignement.setRightHandSide(newRHS);
				newAssignement.setLeftHandSide((Expression) ASTNode
						.copySubtree(ast, lhs));
				currentRewriter.replace(node, newAssignement, description);
			} else {
				// This only happens if we specified not to perform type
				// resolution when creating the AST, which is an internal
				// error. It may happen at other times but I have not seen
				// it.
				// FIXME: Wrap a CoreException in a RuntimeException.
				// The RuntimeException stops the operation and the
				// translator may use the CoreException to notify the
				// user that an internal error occurred.
				throw new RuntimeException("Type binding failed.");
			}
		}
	}

	@Override
	public void endVisit(SuperConstructorInvocation node) {
		if (node.getExpression() != null
				&& node.resolveConstructorBinding() != null
				&& node.resolveConstructorBinding().getDeclaringClass() != null) {
			final ITypeBinding superClazz = node.resolveConstructorBinding()
					.getDeclaringClass();
			try {
				final ClassInfo ci = context.getModel().findClassInfo(
						superClazz.getJavaElement().getHandleIdentifier(),
						false, false, TranslationUtils.isGeneric(superClazz));
				String targetFramework = context.getConfiguration().getOptions().getTargetDotNetFramework().name();
				
				if (ci != null && ci.getTarget(targetFramework) != null) {
					final TargetClass tc = ci.getTarget(targetFramework);
					if (tc.getNestedToInner()) {
						final ListRewrite argList = currentRewriter
								.getListRewrite(
										node,
										SuperConstructorInvocation.ARGUMENTS_PROPERTY);
						argList.insertFirst(currentRewriter
								.createCopyTarget(node.getExpression()),
								description);
						currentRewriter.remove(node.getExpression(),
								description);
					}
				}
			} catch (final Exception e) {
				context.getLogger().logException(
						"Error when tying to modify super constructor call "
								, e);
			}
		}
	}

	@Override
	public void endVisit(ClassInstanceCreation node) {
		final INodeRewriter result = context.getMapper().mapMethodInvocation(
				node, fCu);

		if ((result == null) && (node.getExpression() != null)) {
			// a.new B() -> new B(a, ...)
			final ITypeBinding tb = node.getExpression().resolveTypeBinding();
			if (tb != null) {
				final Type type = node.getType();
				final ITypeBinding typeB = type.resolveBinding();
				if (typeB != null) {
					if (typeB.isMember()) {
						String typename = typeB.getQualifiedName();
						final AST ast = node.getAST();
						if (typename.contains("<")) {
							typename = typeB.getName();
						}
						try {
							ASTNode newType = null;
							if (typename.contains("<")) {
								newType = currentRewriter
										.createStringPlaceholder(typename,
												ASTNode.PARAMETERIZED_TYPE);
							} else {
								newType = ast.newSimpleType(ast
										.newName(typename));
							}
							currentRewriter.remove(node.getExpression(),
									description);
							currentRewriter.replace(node.getType(), newType,
									description);
							final ClassInfo ci = context.getModel()
									.findClassInfo(
											node.getType().resolveBinding()
													.getJavaElement()
													.getHandleIdentifier(),
											false,
											false,
											TranslationUtils
													.isGeneric(node.getType()
															.resolveBinding()));
							String targetFramework = context.getConfiguration().getOptions().getTargetDotNetFramework().name();
							
							if (ci != null && ci.getTarget(targetFramework) != null) {
								final TargetClass tc = ci.getTarget(targetFramework);
								if (tc.getNestedToInner()) {
									final ListRewrite argList = currentRewriter
											.getListRewrite(
													node,
													ClassInstanceCreation.ARGUMENTS_PROPERTY);
									argList.insertFirst(currentRewriter
											.createCopyTarget(node
													.getExpression()),
											description);
								}
							}
						} catch (final Exception e) {
							context.getLogger().logException(
									"Error when tying to modify class instance call "
											, e);
						}
					}
				}
			}
		} else {
			applyNodeRewriter(node, result, description);
		}
	}

	@SuppressWarnings("unchecked")
	@Override
	public void endVisit(ArrayCreation node) {
		final INodeRewriter result = context.getMapper().mapArrayCreation(fCu,
				node);
		applyNodeRewriter(node, result, description);
		// TODO: merge it in Java2CsharpMapper
		final ITypeBinding arrayType = node.resolveTypeBinding();
		if (arrayType.isArray()) {
			if (arrayType.getDimensions() > 1) {
				final ArrayInitializer init = node.getInitializer();
				if (init == null) {
					final List dims = node.dimensions();
					if (dims.size() > 1) {
						rewriteArrayDeclaration(node, dims);
					}
				}
			}
		}
	}

	//
	//
	//

	@Override
	public void endVisit(SuperMethodInvocation node) {
		final INodeRewriter result = context.getMapper().mapMethodInvocation(
				node, fCu);
		applyNodeRewriter(node, result, description);
	}

	//
	// TODO: The tricky case !
	// Because method replacement can have a format is some case we invalidate
	// the tree or lose
	// the already computed modifications.
	//

	@Override
	public boolean visit(MethodInvocation node) {
		final INodeRewriter result = context.getMapper().mapMethodInvocation(
				node, fCu);

		if (result != null) {
			if (result.hasFormat()) {
				final ComputeFieldAccessAndMethodInvocationVisitor nv = new ComputeFieldAccessAndMethodInvocationVisitor(
						context);
				nv.setCompilationUnit(fCu);
				if (node.getExpression() != null) {
					nv.transform(new NullProgressMonitor(), node
							.getExpression());
				}
				if (node.arguments() != null) {
					for (int i = 0; i < node.arguments().size(); i++) {
						final ASTNode child = (ASTNode) node.arguments().get(i);
						nv.transform(new NullProgressMonitor(), child);
					}
				}
				// delayed.put(node, new Tuple<NodeRewriter,
				// TranslatorASTRewrite>(result, nv.getRewriter()));
				result.setICompilationUnit(fCu);
				result.process(context, node, currentRewriter,
						nv.getRewriter(), description);
				return false;
			}
		}
		return true;
	}

	static class Tuple<T, U> {
		T t;
		U u;

		public Tuple(T t, U u) {
			this.t = t;
			this.u = u;
		}

		public T getT() {
			return t;
		}

		public U getU() {
			return u;
		}
	}

	// HashMap<ASTNode, Tuple<NodeRewriter, TranslatorASTRewrite>> delayed = new
	// HashMap<ASTNode, Tuple<NodeRewriter, TranslatorASTRewrite>>();

	@Override
	public void endVisit(MethodInvocation node) {
		INodeRewriter result = context.getMapper()
				.mapMethodInvocation(node, fCu);

		// Tuple<NodeRewriter, TranslatorASTRewrite> delayedResult =
		// delayed.get(node);
		// if (delayedResult != null) {
		/*
		 * delayedResult.getT().setICompilationUnit(this.fCu);
		 * delayedResult.getT().process(this.context, node,
		 * this.currentRewriter, delayedResult.getU() , description);
		 * delayed.remove(node);
		 */
		// }
		if (node.resolveMethodBinding() != null) {
			final ITypeBinding tBinding = node.resolveMethodBinding()
					.getDeclaringClass();
			if (tBinding.isEnum()) {
				// ENUM methods ....
				processEnumMethods(node, tBinding);
			}
		}

		if ((result != null) && !result.hasFormat()) {
			result.setICompilationUnit(fCu);
			result.process(context, node, currentRewriter, null, description);
		}

		if (node.getExpression() != null) {
			final Expression o = node.getExpression();
			if (o.getNodeType() == ASTNode.SIMPLE_NAME) {
				final SimpleName sn = (SimpleName) o;
				final IBinding vb = sn.resolveBinding();
				if (!(vb instanceof IVariableBinding)) {
					result = context.getMapper().mapType(fCu, sn);
					if (result != null) {
						result.setICompilationUnit(fCu);
						result.process(context, sn, currentRewriter, null,
								description);
					}
				}
			}
		}
	}

	//
	// TODO: We have trouble here !
	// We may compute many times the same replacement
	// because :
	// - QualifiedName contains SimpleName
	// - FieldAccess contains VariableDeclaration

	@Override
	public void endVisit(SimpleName node) {
		final INodeRewriter result = context.getMapper().mapFieldAccess(node);
		applyNodeRewriter(node, result, description);
	}

	@Override
	public void endVisit(QualifiedName node) {
		// It's a package do nothing we only rely on field
		if ((node.resolveBinding() != null)
				&& (node.resolveBinding().getKind() == IBinding.PACKAGE)) {
			return;
		}

		if (ASTNodes.getParent(node, ASTNode.IMPORT_DECLARATION) != null)
			// To avoid replacing type in import !!!!
			return;

		// name part of the qualified
		final IBinding nameBinding = node.getName().resolveBinding();
		if (nameBinding == null) {
			return;
		}

		// A field do the replacement job
		if (nameBinding.getKind() == IBinding.VARIABLE) {
			INodeRewriter result = context.getMapper().mapFieldAccess(fCu, node);
			if (result != null) {
				result.setICompilationUnit(fCu);
				result.process(context, node, currentRewriter, null,
						description);
				return;
			}

			if (ASTNodes.getParent(node, ASTNode.ARRAY_INITIALIZER) != null) {
				// Ok, in case of array initializer we want to perform qualified
				// node replacement here ...
				// Copy paste from : ComputeTypeAndMEthodDeclaration ...
				if (node.getQualifier().getNodeType() == ASTNode.QUALIFIED_NAME) {
					result = context.getMapper().mapType2(fCu,
							(QualifiedName) node.getQualifier());
					// It's a type name ...
					if (result != null) {
						result.setICompilationUnit(fCu);
						result.process(context, node, currentRewriter, null,
								description);
					}

					result = context.getMapper().mapPackageAccess(
							(QualifiedName) node.getQualifier(),
							nameBinding.getJavaElement().getJavaProject()
									.getProject());
					if (result != null) {
						result.setICompilationUnit(fCu);
						result.process(context, node.getQualifier(),
								currentRewriter, null, description);
					}
				}
			}
		}

		// Qualifier part of the qualified
		final Name qualBinding = node.getQualifier();
		ITypeBinding itype = null;
		if (qualBinding != null) {
			itype = qualBinding.resolveTypeBinding();
		}
		// Don't understand ....
		if ((qualBinding == null) || !((itype != null) && itype.isArray())) {
			INodeRewriter result = context.getMapper().mapFieldAccess(fCu, node);
			if (result != null) {
				result.setICompilationUnit(fCu);
				result.process(context, node, currentRewriter, null,
						description);
				return;
			}
			if (qualBinding.isQualifiedName()) {
				result = context.getMapper().mapType2(fCu,
						(QualifiedName) qualBinding);
				if (result != null) {
					result.setICompilationUnit(fCu);
					result.process(context, qualBinding, currentRewriter, null,
							description);
					return;
				}
			}
		}
		return;
	}

	@Override
	public void endVisit(FieldAccess node) {
		final INodeRewriter result = context.getMapper().mapFieldAccess(node);
		applyNodeRewriter(node, result, description);
	}

	@Override
	public void endVisit(VariableDeclarationStatement node) {
		final INodeRewriter result = context.getMapper().mapVariableDeclaration(
				node);
		applyNodeRewriter(node, result, description);
	}

	@Override
	public void endVisit(SingleVariableDeclaration node) {
		final INodeRewriter result = context.getMapper().mapVariableDeclaration(
				node);
		applyNodeRewriter(node, result, description);
	}

	@Override
	public void endVisit(VariableDeclarationExpression node) {
		final INodeRewriter result = context.getMapper().mapVariableDeclaration(
				node);
		applyNodeRewriter(node, result, description);
	}

	@Override
	public void endVisit(LabeledStatement node) {
		final INodeRewriter result = context.getMapper().mapLabeledStatement(
				node);
		applyNodeRewriter(node, result, description);
	}

	@Override
	public boolean visit(SimpleType node) {
		return false;
	}

	//
	//
	//

	@SuppressWarnings("unchecked")
	private void rewriteArrayDeclaration(ArrayCreation node, List dims) {
		final String[] args = new String[dims.size()];
		for (int i = 0; i < dims.size(); i++) {
			final ASTNode arg = (ASTNode) dims.get(i);
			final String new_arg = TranslationUtils.replaceByNewValue(arg,
					currentRewriter, fCu, context.getLogger());
			args[i] = new_arg;
		}
		final String pattern = context.getMapper().mapJaggedArrayCreation(
				node,
				TranslationUtils.createCommentForTypeOfNode(node, node
						.getType().getElementType(), context, fCu), args);
		final ASTNode replacement = currentRewriter.createStringPlaceholder(
				pattern, ASTNode.EXPRESSION_STATEMENT);
		currentRewriter.replace(node, replacement, description);
	}

	private void processEnumMethods(MethodInvocation node, ITypeBinding enumType) {
		String enumName = "";
		if (enumType.isMember()) {
			enumName = enumType.getDeclaringClass().getName() + ".";
		}
		enumName += enumType.getName();
		final ITypeBinding[] params = node.resolveMethodBinding()
				.getParameterTypes();
		if (node.resolveMethodBinding().getName().equals("valueOf")) {
			if (params.length == 1 && TranslationUtils.isStringType(params[0])) {
				final ASTNode arg = (ASTNode) node.arguments().get(0);
				final String newArg = TranslationUtils.replaceByNewValue(arg,
						currentRewriter, fCu, context.getLogger());
				final String s = context.getMapper().mapEnumValueOf(enumType,
						newArg);
				final ASTNode newM = currentRewriter.createStringPlaceholder(s,
						ASTNode.METHOD_INVOCATION);
				currentRewriter.replace(node, newM, description);
				return;
			}
		} else if (node.resolveMethodBinding().getName().equals("values")) {
			if (params.length == 0) {
				final String s = context.getMapper().mapEnumValues(enumType);
				final ASTNode newM = currentRewriter.createStringPlaceholder(s,
						ASTNode.METHOD_INVOCATION);
				currentRewriter.replace(node, newM, description);
				return;
			}
		}
	}
}
