package com.ilog.translator.java2cs.translation.textrewriter.ast;

import java.util.List;

import org.eclipse.jdt.core.dom.Block;
import org.eclipse.jdt.core.dom.MethodDeclaration;
import org.eclipse.jdt.core.dom.SimpleName;
import org.eclipse.jdt.core.dom.Type;
import org.eclipse.jdt.core.dom.TypeDeclaration;
import org.eclipse.jdt.core.dom.TypeParameter;
import org.eclipse.jdt.internal.corext.dom.ASTNodes;
import org.eclipse.text.edits.InsertEdit;
import org.eclipse.text.edits.ReplaceEdit;
import org.eclipse.text.edits.TextEdit;
import org.eclipse.text.edits.TextEditGroup;

import com.ilog.translator.java2cs.configuration.info.ClassInfo;
import com.ilog.translator.java2cs.translation.ITranslationContext;
import com.ilog.translator.java2cs.translation.util.TranslationUtils;
import com.ilog.translator.java2cs.util.TranslationModelUtil;

public class AddConstructorInvocationVisitor extends ASTTextRewriter {

	//
	//
	//

	public AddConstructorInvocationVisitor(ITranslationContext context) {
		super(context);
		transformerName = "Add SuperConstructor Invocation";
		description = new TextEditGroup(transformerName);
	}

	//
	//
	//

	@Override
	public boolean isAbridged() {
		return true;
	}

	//
	//
	//

	@SuppressWarnings("unchecked")
	@Override
	public void endVisit(TypeDeclaration node) {
		if (node.typeParameters() != null && node.typeParameters().size() > 0) {
			final List typeParams = node.typeParameters();
			final int size = typeParams.size();
			String cond = "";
			TypeParameter typeP = null;
			Type bound = null;
			for (int i = 0; i < size; i++) {
				typeP = (TypeParameter) typeParams.get(i);
				if (typeP.typeBounds() != null) {
					final SimpleName name = typeP.getName();
					final List bounds = typeP.typeBounds();
					final int bsize = bounds.size();
					if (bsize > 0) {
						cond += TranslationUtils.START_INSERT_HERE_COMMENT
								+ TranslationUtils.WHERE + ASTNodes.asString(name)
								+ TranslationUtils.WHERE_SEPARATOR;
						for (int b = 0; b < bsize; b++) {
							bound = (Type) bounds.get(b);
							String cBound = ASTNodes.asString(bound);
							if (cBound.equals("Object"))
								cBound = "class";
							cond += cBound;
							if (b < bsize - 1)
								cond += ", ";
						}
						cond += "  */";
						final TextEdit removed = new ReplaceEdit(typeP
								.getStartPosition(), typeP.getLength(),
								ASTNodes.asString(name));
						edits.add(removed);
					}
				}
			}
			//
			try {
				final ClassInfo ci = context.getModel().findClassInfo(
						context.getHandlerFromDoc(node, false), false, false,
						true);
				String targetFramework = context.getConfiguration().getOptions().getTargetDotNetFramework().name();
				
				if (ci != null && ci.getTarget(targetFramework) != null
						&& ci.getTarget(targetFramework).getConstraints() != null) {
					cond = TranslationUtils.START_INSERT_HERE_COMMENT + " "
							+ ci.getTarget(targetFramework).getConstraints() + TranslationUtils.END_COMMENTS;
				}
			} catch (final Exception e) {
				context.getLogger().logException(fCu.getElementName() + " ", e);
			}
			//
			int whereToInsert = 0;
			if (node.superInterfaceTypes() != null
					&& node.superInterfaceTypes().size() > 0) {
				final Type type = (Type) node.superInterfaceTypes().get(
						node.superInterfaceTypes().size() - 1);
				whereToInsert = type.getStartPosition() + type.getLength() + 1;
			} else if (node.getSuperclassType() != null) {
				final Type type = node.getSuperclassType();
				whereToInsert = type.getStartPosition() + type.getLength() + 1;
			} else {
				whereToInsert = typeP.getStartPosition() + typeP.getLength()
						+ 1;
			}
			if (typeP != null) {
				final TextEdit added = new InsertEdit(whereToInsert, cond);
				edits.add(added);
			}
		}
	}

	@Override
	public void endVisit(MethodDeclaration node) {
		if (node.isConstructor()) {

			final Block block = node.getBody();
			List<String> arguments = null;
			boolean thisInvokB = false;

			Object[] res = context.getSuperConstructorInvoc(node);
			
			String pattern = null;
			if (res != null) {
				arguments = (List<String>) res [0];
				pattern = (String) res[1];
			}
			if (arguments == null) {
				arguments = context.getThisConstructorInvoc(node);
				thisInvokB = true;
			}
			if (arguments != null) {
				final int start = block.getStartPosition();

				StringBuilder invok = new StringBuilder();
				if (!thisInvokB) {
					invok.append(TranslationUtils.START_INSERT_HERE_COMMENT
							+ ": " + TranslationUtils.SUPER
							+ "(");
				} else {
					invok
							.append(TranslationUtils.START_INSERT_HERE_COMMENT
									+ ": "
									+ TranslationUtils.THIS
									+ "(");
				}
				final int size = arguments.size();
				if (size > 0) {
					if (pattern != null && !pattern.isEmpty()) {
						for (int i = 0; i < size; i++) {
							pattern = pattern.replace("@" + (i + 1), arguments.get(i));
						}
						invok.append(pattern);
					} else {
						for (int i = 0; i < size - 1; i++) {
							invok.append(arguments.get(i));
							invok.append(", ");
						}
						invok.append(arguments.get(size - 1));
					}
				}
				invok.append(") " + TranslationUtils.END_COMMENTS);

				final TextEdit edit = new InsertEdit(start, invok.toString());

				edits.add(edit);

				invok = null;
			}

			if (!thisInvokB) {
				context.removeSuperConstructorInvoc(node);
			} else {
				context.removeThisConstructorInvoc(node);
			}
		} else {
			// generics : generic method
			// generics : wildcard
			final List<String> wildcards = TranslationUtils
					.getTagValuesFromDoc(node, TranslationUtils.MODWILDCARD);

			if (wildcards != null && wildcards.size() > 0) {
				//
				if (!isOverrideMthd(node)) {
					//
					for (final String currentWildcar : wildcards) {
						int index = currentWildcar.indexOf(TranslationUtils.WILDCARD_SEPARATOR);
						final String paramName = currentWildcar.substring(0,
								index).trim();
						//
						String rest = currentWildcar.substring(index + 3);
						index = rest.indexOf(TranslationUtils.WILDCARD_SEPARATOR);
						if (index < 0)
							index = rest.length();
						String paramType = rest.substring(0, index).trim();
						//			
						boolean isSuper = false;
						if (index >= 0 && (rest.length() > (index + 3))) {
							rest = rest.substring(index + 3);
							index = rest.indexOf(TranslationUtils.WILDCARD_SEPARATOR);
							final String kind = rest.substring(0, index).trim();
							if (TranslationUtils.SUPER_KW.equals(kind))
								isSuper = true;
						}
						//
						final Block block = node.getBody();
						if (block != null && paramType != null
								&& !paramType.equals("")) {
							if (paramType.equals(TranslationUtils.OBJECT))
								paramType = TranslationUtils.CLASS;

							final int start = block.getStartPosition();

							String text = null;
							if (isSuper)
								text = TranslationUtils.WHERE + paramType + TranslationUtils.WHERE_SEPARATOR
										+ paramName;
							else
								text = TranslationUtils.WHERE + paramName + TranslationUtils.WHERE_SEPARATOR
										+ paramType;
							final TextEdit edit = new InsertEdit(start,
									TranslationUtils.START_INSERT_HERE_COMMENT
											+ text + " " + TranslationUtils.END_COMMENTS + " ");
							edits.add(edit);
						}
					}
				}
				String paramsName = "";
				for (int i = 0; i < wildcards.size(); i++) {
					final String currentWildcar = wildcards.get(i);
					final int index = currentWildcar
							.indexOf(TranslationUtils.WILDCARD_SEPARATOR);
					paramsName += currentWildcar.substring(0, index).trim();
					if (i < wildcards.size() - 1)
						paramsName += ",";
				}
				//
				final String comments = TranslationUtils.getAfterComments(node
						.getName(), fCu, context);
				final String toBeInserted = TranslationUtils
						.extractValueToInsert(comments);
				int commentsLength = 0;
				if (toBeInserted != null) {
					paramsName += ","
							+ toBeInserted.substring(1,
									toBeInserted.length() - 1);
					commentsLength = comments.trim().length();
				}
				if (!paramsName.equals("")) {
					final String newMName = node.getName().getIdentifier()
							+ "<" + paramsName + ">";
					final int s = node.getName().getStartPosition();
					final int e = node.getName().getLength() + commentsLength;
					final TextEdit edit = new ReplaceEdit(s, e, newMName);
					edits.add(edit);
				}

			}
			final String modgenericmethod = TranslationUtils
					.getTagValueFromDoc(node, TranslationUtils.MODGENERICMETHODCONSTRAINT);
			if (modgenericmethod != null) {
				final String paramName = modgenericmethod.substring(0,
						modgenericmethod.indexOf(TranslationUtils.WILDCARD_SEPARATOR)).trim();
				final String paramType = modgenericmethod.substring(
						modgenericmethod.indexOf(TranslationUtils.WILDCARD_SEPARATOR) + 3)
						.trim();

				final Block block = node.getBody();
				if (block != null) {
					final int start = block.getStartPosition();

					final TextEdit edit = new InsertEdit(start,
							TranslationUtils.START_INSERT_HERE_COMMENT + TranslationUtils.WHERE
									+ paramName + TranslationUtils.WHERE_SEPARATOR + paramType
									+ TranslationUtils.END_COMMENTS + " ");
					edits.add(edit);
				}
			}
		}
	}

	//
	//
	//
	
	private boolean isOverrideMthd(MethodDeclaration node) {
		boolean b = TranslationUtils.containsTag(node,
				context.getModel().getTag(TranslationModelUtil.OVERRIDE_TAG));
		return b;
	}
}
