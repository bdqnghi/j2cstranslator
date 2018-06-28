package com.ilog.translator.java2cs.translation.noderewriter;

import java.util.ArrayList;
import java.util.List;

import org.eclipse.jdt.core.dom.AST;
import org.eclipse.jdt.core.dom.ASTNode;
import org.eclipse.jdt.core.dom.Assignment;
import org.eclipse.jdt.core.dom.Block;
import org.eclipse.jdt.core.dom.CastExpression;
import org.eclipse.jdt.core.dom.CharacterLiteral;
import org.eclipse.jdt.core.dom.ClassInstanceCreation;
import org.eclipse.jdt.core.dom.ConstructorInvocation;
import org.eclipse.jdt.core.dom.EnhancedForStatement;
import org.eclipse.jdt.core.dom.Expression;
import org.eclipse.jdt.core.dom.FieldDeclaration;
import org.eclipse.jdt.core.dom.IMethodBinding;
import org.eclipse.jdt.core.dom.ITypeBinding;
import org.eclipse.jdt.core.dom.MethodDeclaration;
import org.eclipse.jdt.core.dom.MethodInvocation;
import org.eclipse.jdt.core.dom.Modifier;
import org.eclipse.jdt.core.dom.Name;
import org.eclipse.jdt.core.dom.ParameterizedType;
import org.eclipse.jdt.core.dom.ParenthesizedExpression;
import org.eclipse.jdt.core.dom.QualifiedName;
import org.eclipse.jdt.core.dom.ReturnStatement;
import org.eclipse.jdt.core.dom.SimpleName;
import org.eclipse.jdt.core.dom.StringLiteral;
import org.eclipse.jdt.core.dom.SuperConstructorInvocation;
import org.eclipse.jdt.core.dom.SuperMethodInvocation;
import org.eclipse.jdt.core.dom.Type;
import org.eclipse.jdt.core.dom.VariableDeclarationFragment;
import org.eclipse.jdt.core.dom.VariableDeclarationStatement;
import org.eclipse.jdt.core.dom.WildcardType;
import org.eclipse.jdt.internal.corext.dom.ASTNodes;
import org.eclipse.text.edits.TextEditGroup;

import com.ilog.translator.java2cs.configuration.ChangeModifierDescriptor;
import com.ilog.translator.java2cs.configuration.DotNetModifier;
import com.ilog.translator.java2cs.configuration.info.TranslationModelException;
import com.ilog.translator.java2cs.configuration.target.TargetClass;
import com.ilog.translator.java2cs.configuration.target.TargetPackage;
import com.ilog.translator.java2cs.translation.ITranslationContext;
import com.ilog.translator.java2cs.translation.TranslatorASTRewrite;
import com.ilog.translator.java2cs.translation.util.TranslationUtils;

public class MethodRewriter extends MemberElementRewriter implements Cloneable {

	private static final String THIS_REF = "@0";
	private static final String TYPE_PARAM_REF = "%";
	private static final String ARG_REF = "@";
	private static final String VARARG = "...";

	private int[] parametersIndices;
	protected String covariantType;
	private String genericsTest;
	private String replacementCode;
	private String parametersForGenericMethods = null;
	private String formatForSuperInvocation = null;
	
	//
	//
	//

	public MethodRewriter(String name, int[] parameters) {
		this.name = name;
		parametersIndices = parameters;
	}

	public MethodRewriter(String name) {
		this.name = name;
	}

	public MethodRewriter(int dummy, String rewriterPattern) {
		format = rewriterPattern;
	}

	public MethodRewriter(ChangeModifierDescriptor mod) {
		changeModifiers = mod;
	}

	public MethodRewriter() {
		// For modifier and remove exceptions
	}

	//
	// process
	// 

	@SuppressWarnings("unchecked")
	private void processClassInstanceCreation(ITranslationContext context,
			ClassInstanceCreation construct, TranslatorASTRewrite rew,
			TranslatorASTRewrite subRewriter, TextEditGroup description) {
		if (format != null) {
			String newMethod = new String(format);

			if (newMethod.contains(ARG_REF)) {
				final List<ASTNode> args = construct.arguments();
				for (int i = 0; i < args.size(); i++) {
					final ASTNode arg = args.get(i);
					newMethod = replaceNode(context, rew, newMethod, i + 1,
							arg, ARG_REF);
				}
				final ASTNode arg = construct.getExpression();
				if (arg != null) {
					newMethod = replaceNode(context, rew, newMethod, 0, arg,
							ARG_REF);
				}
			}

			if (newMethod.contains(TYPE_PARAM_REF)) {
				final ParameterizedType pType = (ParameterizedType) construct
						.getType();
				if (pType.isParameterizedType()) {
					newMethod = replaceTypeParam2(context, rew, newMethod,
							pType, construct);
				}
			}

			final ClassInstanceCreation newName = (ClassInstanceCreation) rew
					.createStringPlaceholder("/* j2cs:done */ " + newMethod,
							ASTNode.CLASS_INSTANCE_CREATION);

			rew.replace(construct, newName, null);
		} else if (name != null) {
			final AST ast = construct.getAST();
			final Type newType = ast.newSimpleType(ast.newSimpleName(name));
			rew.replace(construct.getType(), newType, null);
		} else if (construct.getExpression() != null) {
			// a.new B()
			final ITypeBinding tb = construct.getExpression()
					.resolveTypeBinding();
			if (tb != null) {
				final Type type = construct.getType();
				final ITypeBinding typeB = type.resolveBinding();
				if (typeB != null && typeB.isMember()) {
					final String typename = typeB.getQualifiedName();
					final AST ast = construct.getAST();
					final ASTNode newType = ast.newSimpleType(ast
							.newName(typename));
					rew.remove(construct.getExpression(), null);
					rew.replace(construct.getType(), newType, null);
				}
			}
		}
	}

	@SuppressWarnings("unchecked")
	private void processMethodInvocation(ITranslationContext context,
			MethodInvocation method, TranslatorASTRewrite rew,
			TranslatorASTRewrite subRewriter, TextEditGroup description) {
		final AST ast = method.getAST();

		if (parametersIndices != null) {
			if (name == null) {
				return;
			}
			final SimpleName newName = ast.newSimpleName(name);

			final MethodInvocation mInvoc = ast.newMethodInvocation();
			mInvoc.setName(newName);

			mInvoc.setExpression((Expression) rew.createCopyTarget(method
					.getExpression()));

			final List args = method.arguments();
			final List copiedArgs = copyArguments(args, rew);
			final List args2 = mInvoc.arguments();

			for (final int element : parametersIndices) {
				final int index = element - 1;
				final ASTNode n2 = (ASTNode) copiedArgs.get(index);
				args2.add(n2);
			}
			rew.replace(method, mInvoc, null);
		} else if (format != null) {
			String newMethod = new String(format);

			boolean generic = false;
			ITypeBinding typeThatHandleGeneric = null;
			final IMethodBinding mb = method.resolveMethodBinding();
			if (mb != null && genericsTest != null) {
				if (genericsTest.equals("typeReceiverIsGeneric")) {
					final ITypeBinding typeOfReceiver = getTypeOfCallee(method,
							context);
					if (typeOfReceiver != null && isGenericType(typeOfReceiver)) {
						newMethod = enabledGenerics(newMethod);
					} else {
						newMethod = disableGenerics(newMethod);
					}
				} else {
					final int indice = Integer.parseInt(genericsTest
							.substring(1));
					if (indice > 0) {
						final List args = method.arguments();
						final Expression genericType = (Expression) args
								.get(indice - 1);
						typeThatHandleGeneric = genericType
								.resolveTypeBinding();
					} else {
						typeThatHandleGeneric = method.resolveMethodBinding()
								.getDeclaringClass();
					}
					generic = !typeThatHandleGeneric.isRawType()
							&& !isSubClassOfRawType(typeThatHandleGeneric);
					if (generic) {
						newMethod = enabledGenerics(newMethod);
					} else {
						newMethod = disableGenerics(newMethod);
					}
				}
			} else {
				if (context.getConfiguration().getOptions().isUseGenerics()) {
					newMethod = enabledGenerics(newMethod);
				} else {
					newMethod = disableGenerics(newMethod);
				}
			}

			String commentForExpression = null;
			if (newMethod.contains(ARG_REF)) {
				List args = method.arguments();
				if (mb.isVarargs()) {
					try {
						boolean containsVARARG = format.contains(VARARG);
						for (int i = 0; i < args.size(); i++) {
							Expression arg = (Expression) args.get(i);
							final ITypeBinding pType = mb.getParameterTypes()[i];
							ITypeBinding aType = arg.resolveTypeBinding();
							if (isArrayOf(aType, pType)) {
								final List<String> res = new ArrayList<String>();
								final int index = i;
								while (isArrayOf(aType, pType)
										&& i < (args.size() - 1)) {
									final String new_arg = TranslationUtils
											.replaceByNewValue(arg,
													(subRewriter == null) ? rew
															: subRewriter, fCu,
													context.getLogger());
									res.add(new_arg);
									i++;
									arg = (Expression) args.get(i);
									aType = arg.resolveTypeBinding();
								}
								if (i == args.size() - 1) {
									final String new_arg = TranslationUtils
											.replaceByNewValue(arg,
													(subRewriter == null) ? rew
															: subRewriter, fCu,
													context.getLogger());
									res.add(new_arg);
								}
								String varArgs = "";
								for (int j = 0; j < res.size() - 1; j++) {
									varArgs += res.get(j) + ",";
								}
								varArgs += res.get(res.size() - 1);
								final String pattern = ARG_REF + (index + 1)
										+ (containsVARARG ? VARARG : "");
								newMethod = newMethod.replace(pattern, varArgs);
							} else {
								final String new_arg = TranslationUtils
										.replaceByNewValue(arg,
												(subRewriter == null) ? rew
														: subRewriter, fCu,
												context.getLogger());
								newMethod = newMethod.replace(
										ARG_REF + (i + 1), new_arg);
							}
						}
					} catch (ArrayIndexOutOfBoundsException e) {
						e.printStackTrace();
					}
				} else {
					for (int i = 0; i < args.size(); i++) {
						final ASTNode arg = (ASTNode) args.get(i);
						final String new_arg = TranslationUtils
								.replaceByNewValue(arg,
										(subRewriter == null) ? rew
												: subRewriter, fCu, context
												.getLogger());
						newMethod = newMethod.replace(ARG_REF + (i + 1),
								new_arg);
					}
				}
				final ASTNode arg = method.getExpression();

				if (arg != null) {
					String new_arg = null;
					//
					if (arg.getNodeType() == ASTNode.STRING_LITERAL) {
						StringLiteral sl = (StringLiteral) arg;
						new_arg = sl.getEscapedValue();
					} else {
						new_arg = TranslationUtils.replaceByNewValue(arg,
								(subRewriter == null) ? rew : subRewriter, fCu,
								context.getLogger());
					}
					//
					commentForExpression = TranslationUtils.getBeforeComments(
							method, fCu, context);
					if (commentForExpression == null
							|| !commentForExpression.trim().startsWith(
									"/* typeof"))
						commentForExpression = "";
					if (new_arg != null) {
						newMethod = newMethod.replace(THIS_REF,
								commentForExpression + new_arg);
					} else {
						new_arg = TranslationUtils.replaceByNewValue(arg, rew,
								fCu, context.getLogger());
					}
				} else if ((arg == null) && (newMethod.indexOf(THIS_REF) >= 0)) {
					newMethod = newMethod.replace(THIS_REF, "this");
				}

				final int rest = newMethod.indexOf("," + ARG_REF);
				if (rest >= 0) {
					final int idx = Integer.parseInt(newMethod.substring(
							rest + 2, rest + 1 + 2));
					newMethod = newMethod.replace("," + ARG_REF + idx, "");
				}
			}

			if (newMethod.contains(TYPE_PARAM_REF)) {
				final ITypeBinding declaring = method.resolveMethodBinding()
						.getDeclaringClass();
				if (declaring.isParameterizedType()) {
					newMethod = replaceTypeParam(context, newMethod, declaring,
							method);
				}

				// 
				if (method.typeArguments() != null
						&& method.typeArguments().size() > 0) {
					newMethod = this.replaceTypeArgs(context, newMethod,
							method, method);
				} else if (generic && typeThatHandleGeneric != null) {
					newMethod = replaceTypeParam(context, newMethod,
							typeThatHandleGeneric, method);
				} else {
					final ITypeBinding[] tArgs = method.resolveMethodBinding()
							.getTypeArguments();
					if (tArgs != null && tArgs.length > 0) {
						for (int i = 0; i < tArgs.length; i++) {
							final String cde = "<"
									+ TranslationUtils.replaceByCSharpName(
											context, tArgs, "", i, "", false)
									+ ">"; // tArgs[i].getName()
							// +
							// ",";
							newMethod = newMethod.replace("<%" + (i + 1) + ">",
									cde);
						}
					} else {
						newMethod = newMethod.replace("<%1>", "");
					}
				}
			}

			newMethod = newMethod.replace("<%1>", ""); // just in case ...
			newMethod = newMethod.replace("<>", ""); // just in case ...
			newMethod = newMethod.replace(VARARG, ""); // just in case ...

			if (covariantType != null) {
				newMethod = "(("
						+ TranslationUtils.castToMethodReturnType(context,
								method, fCu) + ") " + newMethod + ")";
			}

			if (commentForExpression == null || commentForExpression.equals(""))
				newMethod = TranslationUtils.getNodeWithComments(newMethod,
						method, fCu, context);

			final MethodInvocation newName = (MethodInvocation) rew
					.createStringPlaceholder(newMethod,
							ASTNode.METHOD_INVOCATION);
			rew.replace(method, newName, null);
		} else {
			if (name == null) {
				return;
			}

			try {
				String afterComment = TranslationUtils.getAfterComments(method
						.getName(), fCu, context);
				SimpleName newName = null;
				if (afterComment != null) {
					newName = (SimpleName) rew.createStringPlaceholder(name
							+ afterComment, ASTNode.SIMPLE_NAME);
					rew.replace(method.getName(), newName, null);
				} else {
					newName = ast.newSimpleName(name);
					rew.replace(method.getName(), newName, null);
				}

				if (covariantType != null) {
					final String castedName = TranslationUtils
							.castToMethodReturnType(context, method, fCu);
					final CastExpression cast = ast.newCastExpression();
					cast.setExpression((Expression) rew
							.createCopyTarget(method));
					cast.setType((Type) rew.createStringPlaceholder(castedName,
							ASTNode.SIMPLE_TYPE));
					final ParenthesizedExpression pExpr = ast
							.newParenthesizedExpression();
					pExpr.setExpression(cast);
					rew.replace(method, pExpr, null);
				}
				final IMethodBinding mb = method.resolveMethodBinding();
				// Another tricky case ..
				// if we have an array of call to static methods
				// during replacement by the corresponding method in c#
				// the modification of the first element of this array
				// invalidate the rest of the elements so it's not
				// possible to replace the qualifier part ... ouf !
				if (mb != null && Modifier.isStatic(mb.getModifiers())) {
					// a static call is fqn ... maybe :-(
					if (method.getExpression() != null
							&& method.getExpression().getNodeType() == ASTNode.QUALIFIED_NAME) {
						final QualifiedName qn = (QualifiedName) method
								.getExpression();
						if (qn != null) {
							final TypeRewriter trew = (TypeRewriter) context
									.getMapper().mapType2(fCu, qn);
							if (trew != null && trew.getPackageName() != null) {
								final String pName = trew.getPackageName()
										+ "."
										+ TypeRewriter.filterGenerics(trew
												.getName());
								rew.replace(qn, rew.createStringPlaceholder(
										pName, qn.getNodeType()), null);
							} else {
								final PackageRewriter rew2 = (PackageRewriter) context
										.getMapper()
										.mapPackageAccess(
												qn,
												mb.getJavaElement() == null ? null
														: mb
																.getJavaElement()
																.getJavaProject()
																.getProject());
								if (rew2 != null)
									rew2.process(context, qn, rew, null,
											description);
							}
						}
					}
				}

				final List typeArgs = method.typeArguments();
				if (typeArgs != null && typeArgs.size() > 0) {
					String targs = "<";
					for (int i = 0; i < typeArgs.size(); i++) {
						final Type t = ((Type) typeArgs.get(i));
						rew.remove(t, null);
						final TargetClass tc = findTargetClass(context, t);
						if (t.isParameterizedType()) {
							final ParameterizedType pType = (ParameterizedType) t;
							if (tc == null || tc.getName() == null)
								targs += getRawTypeName(pType)
										+ printTypeArguments(pType, context);
							else if (tc.isRemoveGenerics()) {
								targs += tc.getName();
							} else if (tc.getName().contains(TYPE_PARAM_REF)) {
								targs += replaceTypeArgs(tc.getName(), pType
										.typeArguments(), context);
							} else {
								targs += tc.getName()
										+ printTypeArguments(pType, context);
							}
						} else {
							if (tc == null || tc.getName() == null) {
								// TODO : Check if it's renamed
								if (t.resolveBinding() != null
										&& t.resolveBinding().isNested()) {
									final ITypeBinding declClass = t
											.resolveBinding()
											.getDeclaringClass();
									// String pName =
									// declClass.getPackage().getName();
									targs += declClass.getName() + "."
											+ ASTNodes.getTypeName(t);
								} else {
									targs += ASTNodes.getTypeName(t); // getRawTypeName(t);
								}
							} else {
								targs += tc.getName();
								if (tc.isNullable())
									targs += "?";
							}
						}
						if (i < typeArgs.size() - 1)
							targs += ",";
					}
					targs += ">";
					newName = (SimpleName) rew.createStringPlaceholder(name
							+ "/* insert_here:" + targs + " */",
							ASTNode.SIMPLE_NAME);
					rew.replace(method.getName(), newName, null);
				}
			} catch (final Exception e) {
				context
						.getLogger()
						.logException(
								"MethodRewriter:: ("
										+ fCu.getElementName()
										+ ") Error name is "
										+ name
										+ " we can't build a simplename with that !",
								e);
				e.printStackTrace();
			}
		}

		// generic methods
		if (parametersForGenericMethods != null) {
			// As it's time consuming and always false I remove It
			
			// need to check if the parameters computed with the method declaration
			// are "compatibles" with the effective type arguments.
			// As "parametersForGenericMethods" is a string we can't formally
			// verify if effective arguments are "compatibles", but as the code
			// is suppose to compile we can suppose it's true.
			/*final IMethodBinding mBinding = method.resolveMethodBinding();
			if (mBinding.isParameterizedMethod()) {
				ITypeBinding[] tArgs = mBinding.getTypeArguments();
				if (tArgs.length > 0) {					
					String res = tArgs[0].getQualifiedName();
					for(int i = 1; i < tArgs.length; i++) {
						res += "," + tArgs[i].getQualifiedName();
					}
					if (res.equals(parametersForGenericMethods)) {
						String targs = "<" + parametersForGenericMethods + ">";
						SimpleName newName = (SimpleName) rew.createStringPlaceholder(name
								+ "/* insert_here:" + targs + " *", ASTNode.SIMPLE_NAME);
						rew.replace(method.getName(), newName, null);
					} else {
						String targs = "<" + res + ">";
						SimpleName newName = (SimpleName) rew.createStringPlaceholder(name
								+ "/* insert_here:" + targs + " *", ASTNode.SIMPLE_NAME);
						rew.replace(method.getName(), newName, null);
					}
				}
			}	*/		
		}
	}

	private boolean isArrayOf(ITypeBinding type, ITypeBinding array) {
		final ITypeBinding elementType = array.getComponentType();
		if (elementType != null) {
			if (elementType.isEqualTo(type)
					|| type.isAssignmentCompatible(elementType))
				return true;
			ITypeBinding[] bounds = elementType.getTypeBounds();
			if (bounds != null) {
				for (ITypeBinding bound : bounds) {
					if (bound.isEqualTo(type)
							|| type.isAssignmentCompatible(bound))
						return true;
				}
			}
		}
		return false;
	}

	@SuppressWarnings("unchecked")
	private void processSuperMethodInvocation(ITranslationContext context,
			SuperMethodInvocation method, TranslatorASTRewrite rew,
			TranslatorASTRewrite subRewriter, TextEditGroup description) {
		final AST ast = method.getAST();

		if (name == null) {
			return;
		}
		final SimpleName newName = ast.newSimpleName(name);

		if (parametersIndices != null) {
			final SuperMethodInvocation mInvoc = ast.newSuperMethodInvocation();
			mInvoc.setName(newName);

			mInvoc.setQualifier((Name) rew.createCopyTarget(method
					.getQualifier()));

			final List args = method.arguments();
			final List copiedArgs = copyArguments(args, rew);
			final List args2 = mInvoc.arguments();

			for (final int element : parametersIndices) {
				final int index = element - 1;
				final ASTNode n2 = (ASTNode) copiedArgs.get(index);
				args2.add(n2);
			}
			rew.replace(method, mInvoc, null);
		} else {
			rew.replace(method.getName(), newName, null);
		}

	}

	@SuppressWarnings("unchecked")
	private void processMethodDeclaration(ITranslationContext context,
			MethodDeclaration method, TranslatorASTRewrite rew,
			TranslatorASTRewrite subRewriter, TextEditGroup description) {
		final AST ast = method.getAST();

		if (changeModifiers == null) {
			changeModifiers = new ChangeModifierDescriptor();
		}

		// new mapping strategy
		// - public final wo override method -> not virtual
		// - public final with override method -> sealed
		if (Modifier.isFinal(method.getModifiers())
				&& Modifier.isPublic(method.getModifiers())) {
		} else
			changeModifiers.remove(DotNetModifier.FINAL);

		if (method.isConstructor() && Modifier.isStatic(method.getModifiers())) {
			// no modifier for static constructor !
			changeModifiers.remove(DotNetModifier.PUBLIC);
			changeModifiers.remove(DotNetModifier.PROTECTED);
		}
		final ModifiersRewriter rewriter = new ModifiersRewriter(
				changeModifiers);
		rewriter.process(context, method, rew, subRewriter, description);

		// REPLACING BLOCK -->
		if (replacementCode != null) {
			// System.out.println("[Custom Info] Replacing block in "+name+" -> "+replacementCode);
			// Warning: could break the translation ....
			Block block = method.getBody();
			if (block != null && block.statements().size() > 0) {
				Block newBlock = ast.newBlock();
				ASTNode throwStat = rew.createStringPlaceholder(
						replacementCode, ASTNode.THROW_STATEMENT);
				newBlock.statements().add(throwStat);
				rew.replace(block, newBlock, description);
			} else {
				Block newBlock = ast.newBlock();
				ASTNode throwStat = rew.createStringPlaceholder(
						replacementCode, ASTNode.THROW_STATEMENT);
				newBlock.statements().add(throwStat);
				rew.set(method, MethodDeclaration.BODY_PROPERTY, newBlock,
						description);
			}
		}
		// REPLACING BLOCK <--

		final List l = method.thrownExceptions();
		for (int i = 0; i < l.size(); i++) {
			rew.remove((ASTNode) l.get(i), null);
		}

		if ((name != null) && !method.isConstructor()) {
			final SimpleName newName = ast.newSimpleName(name);
			rew.replace(method.getName(), newName, null);
		} else {
			// TODO
			return;
		}

		if (returnType != null) {
			final ASTNode newName = rew.createStringPlaceholder(returnType,
					method.getReturnType2().getNodeType());
			rew.replace(method.getReturnType2(), newName, description);
		}

	}

	@Override
	public void process(ITranslationContext context, ASTNode node,
			TranslatorASTRewrite rew, TranslatorASTRewrite subRewriter,
			TextEditGroup description) {
		switch (node.getNodeType()) {
		case ASTNode.CLASS_INSTANCE_CREATION:
			processClassInstanceCreation(context, (ClassInstanceCreation) node,
					rew, subRewriter, description);
			break;
		case ASTNode.METHOD_INVOCATION:
			processMethodInvocation(context, (MethodInvocation) node, rew,
					subRewriter, description);
			break;
		case ASTNode.SUPER_METHOD_INVOCATION:
			processSuperMethodInvocation(context, (SuperMethodInvocation) node,
					rew, subRewriter, description);
			break;
		case ASTNode.METHOD_DECLARATION:
			processMethodDeclaration(context, (MethodDeclaration) node, rew,
					subRewriter, description);
			break;
		}
	}

	//
	//
	//

	private String disableGenerics(String newMethod) {
		newMethod = newMethod.replace("/* Generics. */", "");
		return newMethod;
	}

	private String enabledGenerics(String newMethod) {
		newMethod = newMethod.replace("/* Generics. */", "Generics.");
		return newMethod;
	}

	//
	//
	//

	private boolean isGenericType(ITypeBinding typeOfReceiver) {
		if (typeOfReceiver.getTypeArguments().length > 0)
			return true;
		if (typeOfReceiver.getTypeParameters().length > 0)
			return true;
		if (typeOfReceiver.getSuperclass() != null)
		   if (isGenericType(typeOfReceiver.getSuperclass()))
			   return true;
		if (typeOfReceiver.getInterfaces() != null) {
			ITypeBinding[] interfaces = typeOfReceiver.getInterfaces();
			for(ITypeBinding interf : interfaces) {
				if (isGenericType(interf))
					return true;
			}
		}
		return false;
	}

	private ITypeBinding getTypeOfCallee(MethodInvocation node,
			ITranslationContext context) {
		if (node.getExpression() == null) {
			return node.resolveMethodBinding().getDeclaringClass();
		}
		final ASTNode parent = getTypeOfReceiver(node, context);
		if (parent == null) {
			context
					.getLogger()
					.logError(
							"MethodRewriter.getTypeOfCallee(MethodInvocation,ITranslationContext) : Unexpected node type for "
									+ node.getParent());
			return null;
		}
		switch (parent.getNodeType()) {
		case ASTNode.ENHANCED_FOR_STATEMENT:
			final EnhancedForStatement fors = (EnhancedForStatement) parent;
			return fors.getParameter().getType().resolveBinding();
		case ASTNode.RETURN_STATEMENT:
			final ReturnStatement ret = (ReturnStatement) parent;
			final MethodDeclaration methDecl = (MethodDeclaration) ASTNodes
					.getParent(ret, ASTNode.METHOD_DECLARATION);
			if (methDecl != null) {
				return methDecl.getReturnType2().resolveBinding();
			}
			return null;
		case ASTNode.FIELD_DECLARATION:
			final FieldDeclaration fDecl = (FieldDeclaration) parent;
			final VariableDeclarationFragment vdf = (VariableDeclarationFragment) fDecl
					.fragments().get(0);
			return vdf.resolveBinding().getType();
		case ASTNode.VARIABLE_DECLARATION_STATEMENT:
			final VariableDeclarationStatement vds = (VariableDeclarationStatement) parent;
			final ITypeBinding typeB = vds.getType().resolveBinding();
			return typeB;
		case ASTNode.ASSIGNMENT:
			final Assignment assign = (Assignment) parent;
			return assign.getLeftHandSide().resolveTypeBinding();
		case ASTNode.METHOD_INVOCATION:
			final MethodInvocation mi = (MethodInvocation) parent;
			final int index = getIndex(mi, node);
			if (index >= 0) {
				final ITypeBinding binding = mi.resolveMethodBinding()
						.getParameterTypes()[index];
				return binding;
			}
			return null;
		case ASTNode.CONSTRUCTOR_INVOCATION:
			final ConstructorInvocation ci = (ConstructorInvocation) parent;
			final int index1 = getIndex(ci, node);
			if (index1 >= 0) {
				final ITypeBinding binding = ci.resolveConstructorBinding()
						.getParameterTypes()[index1];
				return binding;
			}
			return null;
		case ASTNode.SUPER_CONSTRUCTOR_INVOCATION:
			final SuperConstructorInvocation sci = (SuperConstructorInvocation) parent;
			final int index2 = getIndex(sci, node);
			if (index2 >= 0) {
				final ITypeBinding binding = sci.resolveConstructorBinding()
						.getParameterTypes()[index2];
				return binding;
			}
			return null;
		}
		return null;
	}

	// Find the indice where the given node is on the method invocation
	@SuppressWarnings("unchecked")
	private int getIndex(MethodInvocation method, ASTNode node) {
		final List arguments = method.arguments();
		for (int i = 0; i < arguments.size(); i++) {
			final ASTNode element = (ASTNode) arguments.get(i);
			if (node.equals(element) || ASTNodes.isParent(node, element))
				return i;
		}
		return -1;
	}

	@SuppressWarnings("unchecked")
	private int getIndex(ConstructorInvocation method, ASTNode node) {
		final List arguments = method.arguments();
		for (int i = 0; i < arguments.size(); i++) {
			final ASTNode element = (ASTNode) arguments.get(i);
			if (node.equals(element) || ASTNodes.isParent(node, element))
				return i;
		}
		return -1;
	}

	@SuppressWarnings("unchecked")
	private int getIndex(SuperConstructorInvocation method, ASTNode node) {
		final List arguments = method.arguments();
		for (int i = 0; i < arguments.size(); i++) {
			final ASTNode element = (ASTNode) arguments.get(i);
			if (node.equals(element) || ASTNodes.isParent(node, element))
				return i;
		}
		return -1;
	}

	private ASTNode getTypeOfReceiver(MethodInvocation node,
			ITranslationContext context) {
		ASTNode currentNode = node.getParent();
		while (currentNode != null) {
			switch (currentNode.getNodeType()) {
			case ASTNode.VARIABLE_DECLARATION_STATEMENT:
			case ASTNode.CONSTRUCTOR_INVOCATION:
			case ASTNode.FIELD_DECLARATION:
			case ASTNode.SUPER_CONSTRUCTOR_INVOCATION:
			case ASTNode.ASSIGNMENT:
			case ASTNode.ENHANCED_FOR_STATEMENT:
			case ASTNode.RETURN_STATEMENT:
				return currentNode;
			case ASTNode.METHOD_INVOCATION:
				final MethodInvocation mi = (MethodInvocation) currentNode;
				final INodeRewriter nr = context.getMapper()
						.mapMethodInvocation(mi, fCu);
				if (nr != null && nr instanceof MethodRewriter) {
					final MethodRewriter mr = (MethodRewriter) nr;
					// In case the method invocation we found also need to
					// search
					// for a receiver type just continue the search
					if (mr.genericsTest == null
							|| !mr.genericsTest.equals("typeReceiverIsGeneric")) {
						return currentNode;
					}
				} else {
					return currentNode;
				}
			}
			currentNode = currentNode.getParent();
		}
		return null;
	}

	private String getRawTypeName(Type pType) {
		final ITypeBinding typeB = pType.resolveBinding();
		String name = typeB.getErasure().getName();
		ITypeBinding currentType = typeB.getDeclaringClass();
		while (currentType != null) {
			name = currentType.getErasure().getName() + "." + name;
			currentType = currentType.getDeclaringClass();
		}
		return name;
	}

	@SuppressWarnings("unchecked")
	private String replaceTypeArgs(String pattern, List<Type> typeArguments,
			ITranslationContext context) {
		for (int i = 0; i < typeArguments.size(); i++) {
			final Type t = typeArguments.get(i);
			final TargetClass tc = findTargetClass(context, t);
			String replacement = null;

			if (t.isParameterizedType()) {
				final ParameterizedType pType = (ParameterizedType) t;
				if (tc == null || tc.getName() == null)
					replacement = getRawTypeName(pType)
							+ printTypeArguments(pType, context);
				else if (tc.getName().contains(TYPE_PARAM_REF)) {
					replacement = replaceTypeArgs(tc.getName(), pType
							.typeArguments(), context);
				} else {
					replacement = tc.getName()
							+ printTypeArguments(pType, context);
				}
			} else {
				if (tc == null || tc.getName() == null)
					replacement = getRawTypeName(t);
				else
					replacement = tc.getName();
			}
			pattern = pattern.replace(TYPE_PARAM_REF + (i + 1), replacement);
		}
		return pattern;
	}

	@SuppressWarnings("unchecked")
	private String printTypeArguments(ParameterizedType type,
			ITranslationContext context) {
		String res = "";
		final List typeArgs = type.typeArguments();
		if (typeArgs != null && typeArgs.size() > 0) {
			String targs = "<";
			for (int i = 0; i < typeArgs.size(); i++) {
				final Type t = ((Type) typeArgs.get(i));
				if (t.isWildcardType()) {
					final WildcardType wType = (WildcardType) t;
					targs += getRawTypeName(wType);
				} else {
					final TargetClass tc = findTargetClass(context, t);
					if (t.isParameterizedType()) {
						final ParameterizedType pType = (ParameterizedType) t;
						if (tc == null || tc.getName() == null)
							targs += getRawTypeName(pType)
									+ printTypeArguments(pType, context);
						else
							targs += tc.getName()
									+ printTypeArguments(pType, context);
					} else {
						if (tc == null || tc.getName() == null)
							targs += getRawTypeName(t);
						else
							targs += tc.getName();
					}
				}
				if (i < typeArgs.size() - 1)
					targs += ",";
			}
			targs += ">";
			res += targs;
		}
		return res;
	}

	private TargetClass findTargetClass(ITranslationContext context, Type t) {
		TargetClass tc = null;
		try {
			tc = context.getModel().findGenericClassMapping(
					t.resolveBinding().getJavaElement().getHandleIdentifier());
		} catch (final Exception e) {
			// TODO
			context.getLogger().logException("findTargetClass for t " + t, e);
		}
		if (tc == null) {
			tc = context.getModel().findClassMapping(
					t.resolveBinding().getJavaElement().getHandleIdentifier(),
					true, TranslationUtils.isGeneric(t.resolveBinding()));
		}
		return tc;
	}

	private boolean isSubClassOfRawType(ITypeBinding typeThatHandleGeneric) {
		ITypeBinding cTypeB = typeThatHandleGeneric;
		cTypeB = cTypeB.getSuperclass();
		while (cTypeB != null) {
			if (cTypeB.isRawType())
				return true;
			if (cTypeB.isGenericType())
				return false;
			cTypeB = cTypeB.getSuperclass();
		}
		return false;
	}

	@SuppressWarnings("unchecked")
	private String replaceTypeParam2(ITranslationContext context,
			TranslatorASTRewrite rew, String newMethod,
			ParameterizedType pType, ASTNode node) {
		final List args = pType.typeArguments();
		for (int i = 0; i < args.size(); i++) {
			final Type targ = (Type) args.get(i);
			final ITypeBinding tb = targ.resolveBinding();
			final int index = newMethod.indexOf("/* insert_here:(%" + (i + 1)
					+ ") */");
			if (index == 0
					&& ASTNodes.getParent(node, ASTNode.EXPRESSION_STATEMENT) != null) {
				// avoid start a statement with a cast
				newMethod = newMethod.replace("/* insert_here:(%" + (i + 1)
						+ ") */", "");
			}
			TargetClass tc = null;
			try {
				tc = context.getModel().findGenericClassMapping(
						tb.getJavaElement().getHandleIdentifier());
			} catch (final TranslationModelException e) {
				// TODO
				context.getLogger().logException("", e);
			}
			if (tc == null && tb.getJavaElement() != null) {
				tc = context.getModel().findClassMapping(
						tb.getJavaElement().getHandleIdentifier(), true,
						TranslationUtils.isGeneric(tb));
			}
			if (tc != null && tc.getPackageName() != null
					&& tc.getName() != null) {
				String nCls = tc.getPackageName() + "." + tc.getShortName();
				if (tc.isNullable()) {
					// For some tricky case (put on hashtable with wrapper)
					// we can have insert_here in insert_here ... those
					// line try to avoid it
					if (index >= 0) {
						newMethod = newMethod.replace("/* insert_here:(%"
								+ (i + 1), "/* insert_here:(" + nCls + "?");
					}
					nCls += "/* insert_here:? */";
				}
				if (tb.isParameterizedType()) {
					nCls = replaceTypeParam2(context, rew, nCls,
							(ParameterizedType) targ, node);
				}
				newMethod = newMethod
						.replaceAll(TYPE_PARAM_REF + (i + 1), nCls);
			} else {
				newMethod = replaceNode(context, rew, newMethod, i + 1, targ,
						TYPE_PARAM_REF);
			}
		}
		return newMethod;
	}

	private String replaceTypeParam(ITranslationContext context,
			String newMethod, ITypeBinding declaring, ASTNode node) {
		final ITypeBinding pType = declaring;
		final ITypeBinding[] args = pType.getTypeArguments();
		for (int i = 0; i < args.length; i++) {
			final ITypeBinding tb = args[i];
			TargetClass tc = null;
			final int index = newMethod.indexOf("/* insert_here:(%" + (i + 1)
					+ ") */");
			if (index == 0
					&& ASTNodes.getParent(node, ASTNode.EXPRESSION_STATEMENT) != null) {
				// avoid start a statement with a cast
				newMethod = newMethod.replace("/* insert_here:(%" + (i + 1)
						+ ") */", "");
			}
			try {
				if (tb.getJavaElement() != null)
					tc = context.getModel().findGenericClassMapping(
							tb.getJavaElement().getHandleIdentifier());

			} catch (final TranslationModelException e) {
				// TODO
				context.getLogger().logException("", e);
			}
			if (tc == null && tb.getJavaElement() != null) {
				tc = context.getModel().findClassMapping(
						tb.getJavaElement().getHandleIdentifier(), true,
						TranslationUtils.isGeneric(tb));
			}
			if (tc != null && tc.getPackageName() != null
					&& tc.getShortName() != null) {
				String nCls = null;
				nCls = tc.getName();
				if (tb.isParameterizedType()) {
					nCls = replaceTypeParam(context, nCls, tb, node);
				}
				if (tb.isArray()) {
					nCls += "[]";
				}
				if (tc.isNullable()) {
					// For some tricky case (put on hashtable with wrapper)
					// we can have insert_here in insert_here ... those
					// line stry to avoid it
					if (index >= 0) {
						newMethod = newMethod.replace("/* insert_here:(%"
								+ (i + 1), "/* insert_here:(" + nCls + "?");
					}
					nCls += "/* insert_here:? */";
				}
				newMethod = newMethod
						.replaceAll(TYPE_PARAM_REF + (i + 1), nCls);
			} else {
				String nCls = "";
				TargetPackage tp = null;
				if (tb.getPackage() != null) {
					tp = context.getModel().findImportMapping(
							tb.getPackage().getName(),
							declaring.getJavaElement().getJavaProject()
									.getProject());
				}
				if (tp != null)
					nCls = tp.getName() + ".";
				else if (tb.getPackage() != null) {
					final String newName = TranslationUtils
							.defaultMappingForPackage(context, tb.getPackage()
									.getName(), tb.getJavaElement()
									.getJavaProject().getProject());
					nCls = newName + ".";
				}
				if (tb.isMember()) {
					final ITypeBinding typeOfDeclaring = tb.getDeclaringClass();
					final String enclosing = typeOfDeclaring.getName();
					nCls += enclosing;
					if (!enclosing.endsWith(">")) {
						final ITypeBinding erase = typeOfDeclaring.getErasure();
						final ITypeBinding[] typeParams = erase
								.getTypeParameters();
						if (typeParams != null && typeParams.length > 0) {
							nCls += "<";
							for (int j = 0; j < typeParams.length; j++) {
								final ITypeBinding itb = typeParams[j];
								nCls += itb.getName();
								if (j < typeParams.length - 1)
									nCls += ",";
							}
							nCls += ">";
						}
					}
					nCls += "." + tb.getName();
				} else {
					nCls += tb.getName();
				}
				/*
				 * if (tb.isArray()) { nCls+= "[]"; }
				 */
				newMethod = newMethod
						.replaceAll(TYPE_PARAM_REF + (i + 1), nCls);
			}
		}
		return newMethod;
	}

	private String replaceTypeArgs(ITranslationContext context,
			String newMethod, MethodInvocation method, ASTNode node) {
		final IMethodBinding mBinding = method.resolveMethodBinding();
		final ITypeBinding[] args = mBinding.getTypeArguments();
		for (int i = 0; i < args.length; i++) {
			final ITypeBinding tb = args[i];
			TargetClass tc = null;
			try {
				tc = context.getModel().findGenericClassMapping(
						tb.getJavaElement().getHandleIdentifier());
			} catch (final Exception e) {
				// TODO
				context.getLogger().logException(fCu.getElementName(), e);
			}
			if (tc == null) {
				tc = context.getModel().findClassMapping(
						tb.getJavaElement().getHandleIdentifier(), true,
						TranslationUtils.isGeneric(tb));
			}
			if (tc != null && tc.getName() != null) {
				String nCls = tc.getName();
				if (tb.isParameterizedType()) {
					nCls = replaceTypeParam(context, nCls, tb, node);
				}
				if (nCls == null)
					nCls = tb.getName();
				newMethod = newMethod
						.replaceAll(TYPE_PARAM_REF + (i + 1), nCls);
			} else {
				String newPckName = "";
				if (tb.getPackage() != null) {
					final TargetPackage tpck = context.getModel()
							.findPackageMapping(
									tb.getPackage().getName(),
									tb.getJavaElement().getJavaProject()
											.getProject());
					if (tpck != null) {
						newPckName = tpck.getName() + ".";
					} else {
						newPckName = TranslationUtils.defaultMappingForPackage(
								context, tb.getPackage().getName(), tb
										.getJavaElement().getJavaProject()
										.getProject())
								+ ".";
					}
				}
				String className = tb.getName();
				if (tb.isMember()) {
					className = tb.getDeclaringClass().getName() + "."
							+ tb.getName();
				}
				newMethod = newMethod.replaceAll(TYPE_PARAM_REF + (i + 1),
						newPckName + className);
			}
		}
		return newMethod;
	}

	private String replaceNode(ITranslationContext context,
			TranslatorASTRewrite rew, String newMethod, int i, ASTNode arg,
			String pattern) {
		if (arg.getNodeType() == ASTNode.CHARACTER_LITERAL) {
			final CharacterLiteral cl = (CharacterLiteral) arg;
			final String val = cl.getEscapedValue();
			int index = newMethod.indexOf(pattern + i);
			for (; index > 0;) {
				final String s = newMethod.substring(0, index) + val
						+ newMethod.substring(index + 2);
				newMethod = s;
				index = newMethod.indexOf(pattern + i);
			}
		} else {
			final String new_arg = TranslationUtils.replaceByNewValue(arg, rew,
					fCu, context.getLogger());
			newMethod = newMethod.replace(pattern + i, new_arg);
		}
		return newMethod;
	}

	public void filterChangeModifier() {
		if (changeModifiers != null) {
			if (changeModifiers.getModifiersToAdd() != null) {
				if (changeModifiers.getModifiersToAdd().contains(
						DotNetModifier.VIRTUAL)) {
					changeModifiers.getModifiersToAdd().remove(
							DotNetModifier.VIRTUAL);
				}
				if (changeModifiers.getModifiersToAdd().contains(
						DotNetModifier.OVERRIDE)) {
					changeModifiers.getModifiersToAdd().remove(
							DotNetModifier.OVERRIDE);
				}
			}
		}
	}

	@Override
	public INodeRewriter clone() {
		final MethodRewriter newRew = new MethodRewriter();
		newRew.setICompilationUnit(fCu);
		newRew.setName(name);
		newRew.setRemove(remove);
		if (changeModifiers == null) {
			newRew.setChangeModifier(new ChangeModifierDescriptor());
		} else {
			newRew.setChangeModifier((ChangeModifierDescriptor) changeModifiers
					.clone());
		}
		return newRew;
	}

	//
	// CovariantType
	//

	public void setCovariantType(String covariant) {
		covariantType = covariant;
	}

	public String getCovariantType() {
		return covariantType;
	}

	//
	//
	//

	public void setGenericsTest(String genericsTest) {
		this.genericsTest = genericsTest;
	}

	//
	// ReplacementCode
	//
	
	public String getReplacementCode() {
		return replacementCode;
	}

	public void setReplacementCode(String replacementCode) {
		this.replacementCode = replacementCode;
	}

	//
	//
	//

	public void setParametersForGenericMethods(
			String parametersForGenericMethods) {
		this.parametersForGenericMethods = parametersForGenericMethods;
	}

	//
	//
	//
	
	public void setFormatForSuperInvocation(String value) {
		formatForSuperInvocation = value;
	}

}
