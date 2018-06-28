package com.ilog.translator.java2cs.translation.astrewriter;

import java.util.ArrayList;
import java.util.List;

import org.eclipse.core.runtime.CoreException;
import org.eclipse.core.runtime.IProgressMonitor;
import org.eclipse.jdt.core.IMethod;
import org.eclipse.jdt.core.dom.ASTNode;
import org.eclipse.jdt.core.dom.ArrayCreation;
import org.eclipse.jdt.core.dom.ArrayInitializer;
import org.eclipse.jdt.core.dom.ArrayType;
import org.eclipse.jdt.core.dom.Block;
import org.eclipse.jdt.core.dom.CastExpression;
import org.eclipse.jdt.core.dom.ConstructorInvocation;
import org.eclipse.jdt.core.dom.Expression;
import org.eclipse.jdt.core.dom.IMethodBinding;
import org.eclipse.jdt.core.dom.MethodDeclaration;
import org.eclipse.jdt.core.dom.MethodInvocation;
import org.eclipse.jdt.core.dom.SuperConstructorInvocation;
import org.eclipse.jdt.core.dom.Type;
import org.eclipse.jdt.internal.corext.dom.ASTNodes;
import org.eclipse.ltk.core.refactoring.Change;
import org.eclipse.text.edits.TextEditGroup;

import com.ilog.translator.java2cs.configuration.info.ClassInfo;
import com.ilog.translator.java2cs.configuration.info.MethodInfo;
import com.ilog.translator.java2cs.configuration.target.TargetMethod;
import com.ilog.translator.java2cs.translation.ITranslationContext;
import com.ilog.translator.java2cs.translation.noderewriter.ConstructorInvocationRewriter;
import com.ilog.translator.java2cs.translation.noderewriter.INodeRewriter;
import com.ilog.translator.java2cs.translation.util.TranslationUtils;

public class RemoveConstructorInvocationVisitor extends ASTRewriterVisitor {

	//
	//
	//

	public RemoveConstructorInvocationVisitor(ITranslationContext context) {
		super(context);
		transformerName = "Remove Constructor Invocation";
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
	public void endVisit(MethodDeclaration node) {
		if (node.isConstructor()) {
			final Block block = node.getBody();
			if (hasFirstStatement(block)) {
				final ASTNode superInvok = (ASTNode) block.statements().get(0);
				if (superInvok.getNodeType() == ASTNode.SUPER_CONSTRUCTOR_INVOCATION) {
					final SuperConstructorInvocation sci = (SuperConstructorInvocation) superInvok;
					final List<String> arguments = new ArrayList<String>();
					for (final Object elem : sci.arguments()) {
						final String s = getArgumentNameWithoutThis((ASTNode) elem);
						arguments.add(s);
					}
					String pattern = null;
					try {
						IMethodBinding cstBinding = sci.resolveConstructorBinding();
						final ClassInfo ci = context.getModel().findClassInfo(
								cstBinding.getDeclaringClass().getJavaElement().getHandleIdentifier(),
								false, false, TranslationUtils.isGeneric(cstBinding.getDeclaringClass()));
						if (ci != null) {
							IMethod method = (IMethod) cstBinding.getJavaElement();
							MethodInfo mInfo = ci.getConstructor(method);
							if (mInfo != null) {
								String targetFramework = context.getConfiguration().getOptions().getTargetDotNetFramework().name();
								
								TargetMethod tm = mInfo.getTarget(targetFramework);
								if (tm != null) {
									pattern = tm.getPatternForSuperInvocation();
								}
							}
						}
					} catch(Exception e) {
						
					}

					context.addSuperConstructorInvoc(node, arguments, pattern);

					final INodeRewriter result = new ConstructorInvocationRewriter();
					result.process(context, node, currentRewriter, null,
							description);
				} else if (superInvok.getNodeType() == ASTNode.CONSTRUCTOR_INVOCATION) {
					final ConstructorInvocation sci = (ConstructorInvocation) superInvok;
					final List<String> arguments = new ArrayList<String>();
					for (final Object elem : sci.arguments()) {
						final String s = getArgumentNameWithoutThis((ASTNode) elem);
						arguments.add(s);
					}

					context.addThisConstructorInvoc(node, arguments);

					final INodeRewriter result = new ConstructorInvocationRewriter();
					result.process(context, node, currentRewriter, null,
							description);
				}
			}
		}
	}

	//
	//
	//

	private String getArgumentNameWithoutThis(ASTNode node) {
		String name = null;
		if (node.getNodeType() == ASTNode.METHOD_INVOCATION) {
			final MethodInvocation mi = (MethodInvocation) node;
			name = asString(mi);
		} else if (node.getNodeType() == ASTNode.ARRAY_CREATION) {
			final ArrayCreation mi = (ArrayCreation) node;
			name = asString(mi);
		} else if (node.getNodeType() == ASTNode.CAST_EXPRESSION) {
			final CastExpression mi = (CastExpression) node;
			name = asString(mi);
		}  else {
			name = TranslationUtils.getNodeWithComments(
					ASTNodes.asString(node), node, fCu, context, true); // //
																											// ASTFlattener.asString(node);
			if (name != null) {
				if (name.startsWith("/* default("))
					name = name.substring(2, name.indexOf("*/") - 1);
				else if (name.startsWith("/* typeof(")) {
					name = name.substring(3, name.indexOf("*/") - 1)
							+ name.substring(name.indexOf(".class") + 6, name
									.length());
				} else {
					name = TranslationUtils.replaceByNewValue(node, context
							.getLogger());
				}
			} else {
				name = TranslationUtils.replaceByNewValue(node, context
						.getLogger());
			}
		}
		final String THIS = "this";
		if (name.startsWith(THIS + ".")) {
			name = name.substring((THIS + ".").length());
		}
		return name;
	}

	@SuppressWarnings("unchecked")
	private String asString(ArrayCreation mi) {
		final ArrayInitializer init = mi.getInitializer();
		final ArrayType aType = mi.getType();
		String result = "new ";
		result += getNodeWithComments(aType.getComponentType(), false);
		result += getList("[", mi.dimensions(), ",", "]", false, true);
		if (init != null) {
			result += getList("{", init.expressions(), ",", "}", false, true);
		}
		return result;
	}

	@SuppressWarnings("unchecked")
	private String asString(MethodInvocation mi) {
		String s = "";
		if (mi.getExpression() != null)
			s += getNodeWithComments(mi.getExpression(), false) + ".";
		s += getNodeWithComments(mi.getName(), false);
		s += getList("<", mi.typeArguments(), ",", ">", true, false);
		s += getList("(", mi.arguments(), ",", ")", false, false);

		return s;
	}
	
	private String asString(CastExpression ce) {
		String s = "";
		Expression e = ce.getExpression();
		Type t = ce.getType();
		s += "(";
		s += getNodeWithComments(t, false);
		s += ")";
		s += getNodeWithComments(e, false);

		return s;
	}

	private String getList(String pre, List<ASTNode> list, String separator,
			String post, boolean checkSize, boolean hardSearch) {

		String result = "";
		if (checkSize && list.size() == 0) {
			return result;
		}

		result += pre;

		for (int i = 0; i < list.size(); i++) {
			final ASTNode arg = list.get(i);
			result += getNodeWithComments(arg, hardSearch);
			if (i < list.size() - 1)
				result += separator;
		}

		result += post;

		return result;
	}

	private String replaceTranslatorOrder(String expr) {
		if (expr.contains(" /* insert_here:? */")) {
			expr = expr.replace(" /* insert_here:? */", "?"); // hack for ?
		} else if (expr.contains("/* insert_here:")) {
			expr = expr.replace("/* insert_here:", "");
			if (expr.endsWith("*/")) {
				expr = expr.substring(0, expr.length() - 3);
			}
		} else if (expr.contains("/* typeof(")) {
			expr = expr.substring(3, expr.indexOf("*/") - 1)
					+ expr.substring(expr.indexOf(".class") + 6, expr.length());
		}
		return expr;
	}

	private String getNodeWithComments(ASTNode node, boolean hardSearch) {
		return replaceTranslatorOrder(TranslationUtils.getNodeWithComments(
				ASTNodes.asString(node), node, fCu, context, hardSearch));
	}

	private boolean hasFirstStatement(Block block) {
		return block != null && block.statements().size() > 0
				&& block.statements().get(0) != null;
	}
}
