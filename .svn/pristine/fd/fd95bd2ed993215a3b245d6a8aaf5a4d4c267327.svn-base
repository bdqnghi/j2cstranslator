package com.ilog.translator.java2cs.translation.noderewriter;

import java.util.ArrayList;
import java.util.List;

import org.eclipse.jdt.core.ICompilationUnit;
import org.eclipse.jdt.core.IPackageDeclaration;
import org.eclipse.jdt.core.IPackageFragment;
import org.eclipse.jdt.core.JavaModelException;
import org.eclipse.jdt.core.dom.AST;
import org.eclipse.jdt.core.dom.ASTNode;
import org.eclipse.jdt.core.dom.ArrayCreation;
import org.eclipse.jdt.core.dom.CastExpression;
import org.eclipse.jdt.core.dom.IPackageBinding;
import org.eclipse.jdt.core.dom.ITypeBinding;
import org.eclipse.jdt.core.dom.Name;
import org.eclipse.jdt.core.dom.ParameterizedType;
import org.eclipse.jdt.core.dom.PrimitiveType;
import org.eclipse.jdt.core.dom.QualifiedName;
import org.eclipse.jdt.core.dom.SimpleName;
import org.eclipse.jdt.core.dom.SimpleType;
import org.eclipse.jdt.core.dom.Type;
import org.eclipse.jdt.core.dom.PrimitiveType.Code;
import org.eclipse.jdt.internal.corext.dom.ASTNodes;
import org.eclipse.text.edits.TextEditGroup;

import com.ilog.translator.java2cs.configuration.ChangeModifierDescriptor;
import com.ilog.translator.java2cs.configuration.DotNetModifier;
import com.ilog.translator.java2cs.configuration.target.TargetPackage;
import com.ilog.translator.java2cs.translation.ITranslationContext;
import com.ilog.translator.java2cs.translation.TranslatorASTRewrite;
import com.ilog.translator.java2cs.translation.util.TranslationUtils;
import com.ilog.translator.java2cs.util.TranslationModelUtil;

public class TypeRewriter extends ElementRewriter implements Cloneable {

	private String name;

	private String packageName;
	
	private String typeParameter;

	private boolean partial;

	private boolean nullable;

	private boolean nestedToInner;

	private boolean isExcluded = false;

	private String instanceOfTypeName = null;

	private boolean isMember = false;
	
	private boolean requireFQN = false;
	
	//
	//
	//

	public TypeRewriter(String packageName, String name, String typeParameter,
			ChangeModifierDescriptor mod, boolean partial, boolean remove,
			boolean nullable, boolean isMember) {
		this.name = name;
		this.packageName = packageName;
		this.typeParameter = typeParameter;
		if (typeParameter != null) {
			name += "<" + typeParameter + ">";
		}
		this.partial = partial;
		this.remove = remove;
		changeModifiers = mod;
		this.nullable = nullable;
		this.isMember = isMember;
		if (packageName != null)
			addNamespace(packageName + ".*"); // what for ?
	}

	//
	// member
	//
	
	public boolean isMember() {
		return isMember;
	}

	public void setMember(boolean isMember) {
		this.isMember = isMember;
	}
	
	//
	// name
	//

	public void setName(String cname) {
		name = cname;
	}

	public String getName() {
		return name;
	}

	//
	// package name
	//

	public void setPackageName(String pname) {
		packageName = pname;
		// This automatically ad the namespace of that class to the import
		// of all classes that use it :-(
		if (packageName != null && name != null) {
			if (typeParameter != null) // Generic can't be imported
				addNamespace(packageName + "." + name + "<" + typeParameter + ">"); // what for ?
			else if (isMember) {
				int idx = name.lastIndexOf(".");
				if (idx > 0) {
					String enclosingName = name.substring(0, idx);
					addNamespace(packageName + "." + enclosingName); // what for ?
				} else {
					addNamespace(packageName + "." + name /* ".*" */); // what for ?
				}
			} else
				addNamespace(packageName + "." + name /* ".*" */); // what for ?
		}
	}

	public String getPackageName() {
		return packageName;
	}

	//
	//
	//

	@Override
	public void process(ITranslationContext context, ASTNode node,
			TranslatorASTRewrite rew, TranslatorASTRewrite subRewriter,
			TextEditGroup description) {
		final AST ast = node.getAST();

		if (node.getNodeType() == ASTNode.SIMPLE_TYPE) {
			processSimpleType(context, node, rew, ast);
		} else if (node.getNodeType() == ASTNode.QUALIFIED_NAME) {
			processQualifiedName(context, node, rew, ast);
		} else if (node.getNodeType() == ASTNode.SIMPLE_NAME) {
			processSimpleName(node, rew, ast);
		} else if (node.getNodeType() == ASTNode.TYPE_DECLARATION) {
			processTypeDeclaration(context, node, rew, subRewriter, description);
		} else if (node.getNodeType() == ASTNode.ARRAY_CREATION) {
			processArrayCreation(context, node, rew);
		} else if (node.getNodeType() == ASTNode.TYPE_LITERAL) {
			processTypeLiteral(context, node, rew);
		} else if (node.getNodeType() == ASTNode.CAST_EXPRESSION) {
			processCastExpression(node, rew);
		} else if (node.getNodeType() == ASTNode.PARAMETERIZED_TYPE) {
			processParametrisedType(context, node, rew, description);
		}
	}

	private void processParametrisedType(ITranslationContext context,
			ASTNode node, TranslatorASTRewrite rew, TextEditGroup description) {
		if (name != null) {
			final ParameterizedType pType = (ParameterizedType) node;
			//
			String newName = name;
			final SimpleType sType = getSimpleType(pType);
			boolean fqn = false;
			try {
				String mappedName = newName;

				if (TypeRewriter.hasConflictWithNamespace(fCu, context,
						mappedName) || requireFQN) {
					fqn = true;
				}
				String qualifiedName = ASTNodes.getQualifier(sType
						.getName());
				if (!qualifiedName.equals("")) {
					qualifiedName += ".";
				}
				if (fqn) {
					String pck = getPackageName();
					if (pck == null) {
						final IPackageBinding pBinding = sType
								.resolveBinding().getPackage();
						if (pBinding != null) {
							final String pckName = pBinding.getName();
							final TargetPackage tPck = context.getModel()
									.findPackageMapping(
											pckName,
											sType.resolveBinding()
													.getJavaElement()
													.getJavaProject()
													.getProject());
							if (tPck != null) {
								pck = tPck.getName();
							} else {
								pck = TranslationUtils
										.defaultMappingForPackage(context,
												pckName, sType
														.resolveBinding()
														.getJavaElement()
														.getJavaProject()
														.getProject());
							}
							pck += ".";
						} else {
							pck = "";
						}
						
					} else
						pck += ".";

					qualifiedName = ""; // Remove java pck name ....
					
					if (name.contains("<")) {
						mappedName = filterGenerics(mappedName);
						final Type newType = (Type) rew
								.createStringPlaceholder(pck
										+ qualifiedName + mappedName,
										ASTNode.PARAMETERIZED_TYPE);
						rew.replace(pType.getType(), newType, description);
					} else {
						// Replace a generic type by a non generic type ....
						final Type newType = (Type) rew
								.createStringPlaceholder(pck
										+ qualifiedName + mappedName,
										ASTNode.PARAMETERIZED_TYPE);
						rew.replace(pType, newType, description);

					}
				} else {
					//

					if (name.contains("<")) {
						newName = filterGenerics(name);
						final Type newType = (Type) rew
								.createStringPlaceholder(newName,
										ASTNode.PARAMETERIZED_TYPE);
						rew.replace(pType.getType(), newType, null);
					} else {
						// Replace a generic type by a non generic type ....
						final Type newType = (Type) rew
								.createStringPlaceholder(newName,
										ASTNode.PARAMETERIZED_TYPE);
						rew.replace(pType, newType, null);
					}
				}
			} catch (final Exception e) {
				context.getLogger().logException(
						"TypeRewriter:: Error, name is " + name, e);
			}
		}
	}

	private void processCastExpression(ASTNode node, TranslatorASTRewrite rew) {
		if (name != null) {
			final String filteredName = filterGenerics(name);
			final CastExpression castExpr = (CastExpression) node;
			ASTNode replacement = rew.createStringPlaceholder(filteredName,
					castExpr.getType().getNodeType());
			if (nullable) {
				replacement = rew.createStringPlaceholder(filteredName
						+ "/* insert_here:? */", castExpr.getType()
						.getNodeType());
			}
			rew.replace(castExpr.getType(), replacement, null);
		}
	}

	private void processTypeLiteral(ITranslationContext context, ASTNode node,
			TranslatorASTRewrite rew) {
		if (name != null) {
			// TODO : same code as in TypeOfRewriter !!!!
			final String typeof = context.getMapper().getKeyword(
					TranslationModelUtil.TYPEOF_KEYWORD,
					TranslationModelUtil.CSHARP_MODEL);
			final String newName = TypeRewriter.eraseNullable(name);
			final String builder = TranslationUtils.formatTypeOf(typeof,
					newName);
			final ASTNode placeholder = rew.createStringPlaceholder(
					builder, ASTNode.TYPE_LITERAL);
			rew.replace(node, placeholder, null);
		}
	}

	private void processArrayCreation(ITranslationContext context,
			ASTNode node, TranslatorASTRewrite rew) {
		final ArrayCreation aCreation = (ArrayCreation) node;

		if (name != null) {
			final String filteredName = filterGenerics(name);
			// Name can contain "." (inner class) or "<" (generics)
			String nameWithComments = filteredName;
			if (TranslationUtils.containsComments(node)) {
				nameWithComments = " "
						+ TranslationUtils.getNodeWithComments(
								filteredName, node, fCu, context) + " ";
			}
			Type newName = null;
			if (nullable
					&& !nameWithComments.contains("/* insert_here:? */")) {
				newName = (Type) rew.createStringPlaceholder(
						nameWithComments + "/* insert_here:? */",
						ASTNode.SIMPLE_TYPE);
			} else {
				newName = (Type) rew.createStringPlaceholder(
						nameWithComments, ASTNode.SIMPLE_TYPE);
			}
			rew
					.replace(aCreation.getType().getElementType(), newName,
							null);
		}
	}

	private void processTypeDeclaration(ITranslationContext context,
			ASTNode node, TranslatorASTRewrite rew,
			TranslatorASTRewrite subRewriter, TextEditGroup description) {
		if (changeModifiers == null) {
			if (partial) {
				changeModifiers = new ChangeModifierDescriptor();
				changeModifiers.add(DotNetModifier.PARTIAL);
				changeModifiers.remove(DotNetModifier.FINAL);
				final ModifiersRewriter rewriter = new ModifiersRewriter(
						changeModifiers);
				rewriter.process(context, node, rew, subRewriter,
						description);
			}
		} else {
			if (partial) {
				changeModifiers.add(DotNetModifier.PARTIAL);
			}
			changeModifiers.remove(DotNetModifier.FINAL);
			final ModifiersRewriter rewriter = new ModifiersRewriter(
					changeModifiers);
			rewriter.process(context, node, rew, subRewriter, description);
		}
	}

	private void processSimpleName(ASTNode node, TranslatorASTRewrite rew,
			final AST ast) {
		if (name != null) {
			SimpleName newName = null;
			final String filteredName = filterGenerics(name);
			if (name.indexOf(".") > 0) {
				newName = (SimpleName) rew.createStringPlaceholder(
						filteredName, ASTNode.SIMPLE_NAME);
			} else if (name.contains("<")) {
				newName = (SimpleName) rew.createStringPlaceholder(
						filteredName, ASTNode.SIMPLE_NAME);
			} else {
				newName = ast.newSimpleName(filteredName);
			}
			rew.replace(node, newName, null);
		}
	}

	private void processQualifiedName(ITranslationContext context,
			ASTNode node, TranslatorASTRewrite rew, final AST ast) {
		// Strange QualifiedName is supposed to be a type ...
		// I don't see why we only replace the qualified part ...
		if (name != null) {
			Name newName = null;
			try {
				final String filteredName = filterGenerics(name);
				if (filteredName.indexOf(".") > 0) {
					newName = (SimpleName) rew.createStringPlaceholder(
							filteredName, ASTNode.SIMPLE_NAME);
				} else {
					final Code code = PrimitiveType.toCode(filteredName);
					if (code != null) {
						newName = (SimpleName) rew.createStringPlaceholder(
								filteredName, ASTNode.SIMPLE_NAME);
					} else {
						if ((packageName != null)
								&& !packageName.equals("")) {
							newName = (QualifiedName) rew
									.createStringPlaceholder(packageName
											+ "." + filteredName,
											ASTNode.QUALIFIED_NAME);
						} else {
							newName = ast.newSimpleName(filteredName);
						}
					}
				}
				rew.replace(node, newName, null);
			} catch (final Exception e) {
				context
						.getLogger()
						.logException(
								"TypeRewriter:: Error, name is "
										+ name
										+ " we can't build a simplename with that !", e);
			}
		}
	}

	private void processSimpleType(ITranslationContext context, ASTNode node,
			TranslatorASTRewrite rew, final AST ast) {
		if (node.getParent().getNodeType() == ASTNode.INSTANCEOF_EXPRESSION
				|| node.getParent().getNodeType() == ASTNode.TYPE_LITERAL) {
			if (instanceOfTypeName != null) {
				final Name newName = (Name) rew.createStringPlaceholder(
						instanceOfTypeName, ASTNode.SIMPLE_NAME);
				final SimpleType newType = ast.newSimpleType(newName);
				rew.replace(node, newType, null);
				return;
			}
		}
		final SimpleType sType = (SimpleType) node;
		if (name != null) {
			// TODO: name is not always simple ????
			final String filteredName = filterGenerics(name);
			final String nodeName = ASTNodes.getSimpleNameIdentifier(sType
					.getName());
			boolean fqn = false;
			String beforeComments = TranslationUtils.getBeforeComments(
					node, fCu, context);
			if (beforeComments != null && !beforeComments.equals("")) {
				beforeComments = beforeComments.substring(2,
						beforeComments.length() - 3).trim();
				// hopings "insert_here:........."
				final int lastDotOfComments = beforeComments
						.lastIndexOf(".");
				final int firstDotOfName = filteredName.indexOf(".");
				if (lastDotOfComments > 0 && firstDotOfName > 0) {
					final String firstNameOfName = filteredName.substring(
							0, firstDotOfName);
					if (beforeComments.endsWith(firstNameOfName + "."))
						return;
				}
			}
			try {
				if (hasConflictWithNamespace(fCu, context, filteredName) || requireFQN) {
					fqn = true;
				}
				final Code code = PrimitiveType.toCode(filteredName);
				if (code != null) {
					final Type t = ast.newPrimitiveType(code);
					rew.replace(node, t, null);
				} else {
					//
					String nameWithComments = fqn ? getPackageName() + "."
							+ filteredName : filteredName;
					if (TranslationUtils.containsComments(node)) {
						// We already put something to add ...
						final String comments = TranslationUtils
								.getBeforeComments(node, fCu, context);
						final int index = filteredName.indexOf(".");
						// If case of incremental translation, I can read
						// from the UPDATE mapping
						// file data that are already computed and apply.
						final String pattern = "/* insert_here:"
								+ filteredName.substring(0,
										index > 0 ? index : filteredName
												.length() - 1);
						// Do i try to add something I already have ?
						// Clearly NO !
						if (!comments.startsWith(pattern)) {
							nameWithComments = " "
									+ TranslationUtils.getNodeWithComments(
											fqn ? getPackageName() + "."
													+ filteredName
													: filteredName, node,
											fCu, context) + " ";
						}
					}
					if (nullable
							&& !nameWithComments
									.contains("/* insert_here:? */")) {
						rew.replace(node, rew.createStringPlaceholder(
								nameWithComments + "/* insert_here:? */",
								node.getNodeType()), null);
					} else if (!nodeName.equals(filteredName)) {
						final Name newName = (Name) rew
								.createStringPlaceholder(nameWithComments,
										ASTNode.SIMPLE_NAME);
						final SimpleType newType = ast
								.newSimpleType(newName);
						rew.replace(node, newType, null);
					}
				}
			} catch (final Exception e) {
				context
						.getLogger()
						.logException(
								"TypeRewriter:: Error, name is "
										+ name
										+ " we can't build a simplename with that ! "
										, e);
			}
		} else {
			boolean fqn = false;
			try {
				final String mappedName = ASTNodes
						.getSimpleNameIdentifier(sType.getName());
				if (TypeRewriter.hasConflictWithNamespace(fCu, context,
						mappedName) || requireFQN) {
					fqn = true;
				}
				final String qualifiedName = "";
				final ITypeBinding declaringType = sType.resolveBinding()
						.getDeclaringClass();
				if (declaringType == null) // modified
					return;
				if (fqn) {
					String pck = getPackageName();
					if (pck == null) {
						final IPackageBinding pBinding = sType
								.resolveBinding().getPackage();
						if (pBinding != null) {
							final String pckName = pBinding.getName();
							final TargetPackage tPck = context.getModel()
									.findPackageMapping(
											pckName,
											declaringType.getJavaElement()
													.getJavaProject()
													.getProject());
							if (tPck != null) {
								pck = tPck.getName();
							} else {
								pck = TranslationUtils
										.defaultMappingForPackage(context,
												pckName, declaringType
														.getJavaElement()
														.getJavaProject()
														.getProject());
							}
							pck += ".";
						} else {
							pck = "";
						}
					}
					final Name newName = (Name) rew
							.createStringPlaceholder(pck + qualifiedName
									+ mappedName, ASTNode.SIMPLE_NAME);
					final SimpleType newType = rew.getAST().newSimpleType(
							newName);
					rew.replace(node, newType, null);
				}
			} catch (final Exception e) {
				context
						.getLogger()
						.logException(
								"TypeRewriter:: Error, name is "
										+ name
										+ " we can't build a simplename with that ! "
										, e);
			}
		}
	}

	public static String filterGenerics(String name2) {
		if (name2 == null)
			return name2;
		final int start = name2.indexOf("<");
		if (start >= 0) {
			return name2.substring(0, start);
		}
		return name2;
	}

	/*
	 * private String instantiate(String name2, String type) { return
	 * filterGenerics(name2); }
	 */

	private SimpleType getSimpleType(ParameterizedType type) {
		if (type.getType().isSimpleType())
			return ((SimpleType) type.getType());
		return null;
	}

	public static boolean hasConflictWithNamespace(ICompilationUnit fCu,
			ITranslationContext context, String zName)
			throws JavaModelException {
		final IPackageDeclaration[] packageDeclarations = fCu
				.getPackageDeclarations();
		if (packageDeclarations.length > 0) {
			final IPackageDeclaration pDecl = packageDeclarations[0];
			final String pName = pDecl.getElementName();
			final List<IPackageFragment> pF = getSubPackage(fCu, pName);
			for (final IPackageFragment iPf : pF) {
				final String pfName = iPf.getElementName();
				final TargetPackage tPck = context.getModel()
						.findPackageMapping(pfName,
								iPf.getJavaProject().getProject());
				if (tPck != null) {
					final String targetPName = tPck.getName();
					if (targetPName != null && targetPName.endsWith(zName))
						return true;
				} else {
					final String targetPName = TranslationUtils
							.defaultMappingForPackage(context, pfName, iPf
									.getJavaProject().getProject());
					if (targetPName.endsWith(zName))
						return true;
				}
			}
		}
		return false;
	}

	public static List<IPackageFragment> getSubPackage(ICompilationUnit fCu,
			String parentPck) throws JavaModelException {
		final List<IPackageFragment> result = new ArrayList<IPackageFragment>();
		final IPackageFragment[] pFragments = fCu.getJavaProject()
				.getPackageFragments();
		for (final IPackageFragment fragment : pFragments) {
			if (fragment.getCorrespondingResource() != null) {
				if (fragment.getElementName().startsWith(parentPck + ".")) {
					result.add(fragment);
				}
			}
		}
		return result;
	}

	private static String eraseNullable(String tName) {
		String newName = tName;
		final int index = tName.indexOf("Nullable<");
		if (index >= 0) {
			newName = tName.substring(9, tName.length() - 1);
		}
		return newName;
	}

	public void setPartial(boolean b) {
		partial = b;
	}

	public void setNullable(boolean nullable2) {
		nullable = nullable2;
	}

	//
	//
	//

	@Override
	public INodeRewriter clone() {
		final TypeRewriter newR = new TypeRewriter(packageName, name, typeParameter, 
				changeModifiers, partial, remove, nullable, isMember);
		newR.setNestedToInner(nestedToInner);
		return newR;
	}

	//
	//
	//

	public void setNestedToInner(boolean nestedToInner) {
		this.nestedToInner = nestedToInner;
	}

	public boolean getNestedToInner() {
		return nestedToInner;
	}

	//
	//
	//

	public void setExcluded(boolean isExcluded) {
		this.isExcluded = isExcluded;
	}

	public boolean isExluced() {
		return isExcluded;
	}

	public String getInstanceOfTypeName() {
		return instanceOfTypeName;
	}

	public void setInstanceOfTypeName(String instanceOfTypeName) {
		this.instanceOfTypeName = instanceOfTypeName;
	}

	public void setTypeParameter(String value) {
		this.typeParameter = value;
		if (typeParameter != null) {
			name += "<" + typeParameter + ">";
		}
	}

	public boolean isNullable() {		
		return nullable;
	}

	public void setRequireFQN(Boolean value) {
		requireFQN = value;
	}

}
