package com.ilog.translator.java2cs.translation.astrewriter;

import java.util.ArrayList;
import java.util.List;

import org.eclipse.core.resources.IProject;
import org.eclipse.core.runtime.CoreException;
import org.eclipse.core.runtime.IProgressMonitor;
import org.eclipse.jdt.core.Flags;
import org.eclipse.jdt.core.JavaModelException;
import org.eclipse.jdt.core.dom.ASTNode;
import org.eclipse.jdt.core.dom.ASTVisitor;
import org.eclipse.jdt.core.dom.Block;
import org.eclipse.jdt.core.dom.BodyDeclaration;
import org.eclipse.jdt.core.dom.FieldAccess;
import org.eclipse.jdt.core.dom.IBinding;
import org.eclipse.jdt.core.dom.Javadoc;
import org.eclipse.jdt.core.dom.MethodDeclaration;
import org.eclipse.jdt.core.dom.Modifier;
import org.eclipse.jdt.core.dom.SimpleName;
import org.eclipse.jdt.core.dom.SingleVariableDeclaration;
import org.eclipse.jdt.core.dom.TagElement;
import org.eclipse.jdt.core.dom.Type;
import org.eclipse.jdt.core.dom.TypeDeclaration;
import org.eclipse.jdt.core.dom.rewrite.ListRewrite;
import org.eclipse.jdt.internal.corext.dom.ASTNodes;
import org.eclipse.ltk.core.refactoring.Change;
import org.eclipse.text.edits.TextEditGroup;

import com.ilog.translator.java2cs.configuration.ChangeModifierDescriptor;
import com.ilog.translator.java2cs.configuration.DotNetModifier;
import com.ilog.translator.java2cs.configuration.info.MethodInfo;
import com.ilog.translator.java2cs.configuration.target.TargetClass;
import com.ilog.translator.java2cs.configuration.target.TargetIndexer;
import com.ilog.translator.java2cs.configuration.target.TargetPackage;
import com.ilog.translator.java2cs.configuration.target.TargetProperty;
import com.ilog.translator.java2cs.translation.ITranslationContext;
import com.ilog.translator.java2cs.translation.util.DocUtils;
import com.ilog.translator.java2cs.translation.util.TranslationUtils;
import com.ilog.translator.java2cs.util.TranslationModelUtil;

@SuppressWarnings("restriction")
/**
 * Replace all method marked as property by property declaration
 */
public class PropertiesVisitor extends ASTRewriterVisitor {

	//
	//
	//

	private static final String EDITOR_BROWSABLE_ATTRIBUTE = "/* insert_here:[EditorBrowsable(EditorBrowsableState.Never)] */\n";

	public PropertiesVisitor(ITranslationContext context) {
		super(context);
		transformerName = "Properties";
		description = new TextEditGroup(transformerName);
	}

	//
	//
	//

	@Override
	public boolean needRecovery() {
		// To deal with extreme case !
		return true;
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
	public void endVisit(TypeDeclaration node) {
		final List<TargetProperty> properties = context.getMapper()
				.getProperties(node, context.getHandlerFromDoc(node, false));
		if (properties != null) {
			for (final TargetProperty property : properties) {
				final MethodInfo getter = property.getGetter();
				String covariantType = null;
				if (!property.getCovariantOption().isDefaultValue()) {
					//				
					covariantType = property.getCovariantOption().getValue();
					final int index = covariantType.lastIndexOf(":");
					final String pck = covariantType.substring(0, index);
					final String className = covariantType.substring(index + 1);
					String newClassName = context.getModel().getNewNestedName(pck,
							className);
					if (newClassName != null) {
						covariantType = pck + "." + newClassName;
					} else {
						newClassName = className;
					}
					IProject reference = null;
					if (node.resolveBinding() != null) {
						reference = node.resolveBinding().getJavaElement()
								.getJavaProject().getProject();
					}
					final TargetPackage mappedPck = context.getModel()
							.findPackageMapping(pck, reference);
					if (mappedPck != null) {
						covariantType = mappedPck.getName() + "." + newClassName;
					} else {
						covariantType = TranslationUtils.defaultMappingForPackage(
								context, pck, reference)
								+ "." + newClassName;
					}
				} else if (property.getReturnType() != null) {
					covariantType = property.getReturnType();
					final int index = covariantType.lastIndexOf(".");
					if (index >= 0) {
					final String pck = covariantType.substring(0, index);
					final String className = covariantType.substring(index + 1);
					String newClassName = context.getModel().getNewNestedName(pck,
							className);
					if (newClassName != null) {
						covariantType = pck + "." + newClassName;
					} else {
						newClassName = className;
					}
					IProject reference = null;
					if (node.resolveBinding() != null) {
						reference = node.resolveBinding().getJavaElement()
								.getJavaProject().getProject();
					}
					final TargetPackage mappedPck = context.getModel()
							.findPackageMapping(pck, reference);
					if (mappedPck != null) {
						covariantType = mappedPck.getName() + "." + newClassName;
					} else {
						covariantType = TranslationUtils.defaultMappingForPackage(
								context, pck, reference)
								+ "." + newClassName;
					}
					}
				}
							
				//
				MethodDeclaration mgDecl = null;
				if (getter != null)
					mgDecl = findMethod(node, getter);
				
				final MethodInfo setter = property.getSetter();
				MethodDeclaration msDecl = null;
				if (setter != null)
					msDecl = findMethod(node, setter);
	
				// BUG FIX : keep comments
				RemoveCustomCommentsVisitor commentsVisitor = new RemoveCustomCommentsVisitor(context,currentRewriter);
				String getterComments="";
				String setterComments="";
				String tagToAdd = "";
				if (context.getConfiguration().getOptions().getTagToAddtoComment().getValue() != null
						&& !context.getConfiguration().getOptions()
								.getTagToAddtoComment().equals("")) {
					tagToAdd = "/// <"
							+ context.getConfiguration().getOptions()
									.getTagToAddtoComment().getValue() + "/>";
				}
				if ((mgDecl != null) && (mgDecl.getJavadoc() != null)) {
					List elements = mgDecl.getJavadoc().tags();
					if ((elements != null) && (elements.size() > 0)) {
						StringBuffer summary = new StringBuffer();
						StringBuffer buffer = new StringBuffer();
						final String existingHandlerTag = TranslationUtils
						.getTagValueFromDoc((BodyDeclaration) mgDecl, context
								.getMapper().getTag(
										TranslationModelUtil.HANDLER_TAG));
						DocUtils.applyJavadocReplacement(context,
								commentsVisitor.getTags(), elements, summary,
								buffer, tagToAdd, existingHandlerTag, false);
						if (!buffer.toString().equals("")) {
							getterComments = buffer.toString()+"\n";
						}
					}					
				}
				
				if ( (msDecl != null) && (msDecl.getJavadoc() != null)) {
					List elements = msDecl.getJavadoc().tags();
					if ((elements != null) && (elements.size() > 0)) {
						StringBuffer summary = new StringBuffer();
						StringBuffer buffer = new StringBuffer();
						final String existingHandlerTag = TranslationUtils
						.getTagValueFromDoc((BodyDeclaration) msDecl, context
								.getMapper().getTag(
										TranslationModelUtil.HANDLER_TAG));
						DocUtils.applyJavadocReplacement(context,
								commentsVisitor.getTags(), elements, summary,
								buffer, tagToAdd, existingHandlerTag, false);
						if (!buffer.toString().equals("")) {
							setterComments = buffer.toString()+"\n";							
						}
					}					
				}
				
				if (getter != null && setter != null && mgDecl != null
						&& msDecl != null) {
					// Retrieve getter body of that method
					Block getterBody = mgDecl.getBody();
					// Retrieve setter body of that method
					Block setterBody = msDecl.getBody();
					
					String toAdd = TranslationUtils.getAfterComments(mgDecl.getReturnType2(), fCu, context).trim();
					
					final List<MyRegion> setterReplacement = new ArrayList<MyRegion>();
					buildSetterBody(msDecl, "value", 1, setterReplacement);
					//
					final String modifiers = writeModifiers(getter, mgDecl/*
																			 * ,
																			 * setter,
																			 * msDecl
						// fix : no body for abstract properties													 */);
					if (modifiers.contains("abstract")) {
						getterBody = null;
						setterBody = null;
					}
					
					String newProperty = "";
					if (mgDecl.getJavadoc() != null || msDecl.getJavadoc() != null) {
						newProperty += DocUtils.processPropertyDoc(this.context, mgDecl, msDecl) + "\n";
					}
					if (context.getConfiguration().getOptions()
							.isAddNotBrowsableAttribute())
						newProperty += EDITOR_BROWSABLE_ATTRIBUTE;
					newProperty += modifiers;
					if (covariantType != null)
						newProperty += covariantType;
					else
						newProperty += ASTNodes.asString(mgDecl.getReturnType2()) + toAdd;
					newProperty += " " + property.getName() + " {\n";
					newProperty += getterComments;					
					newProperty += "  get";
					newProperty += ((getterBody == null) ? ";\n" : (" "
							+ getString(getterBody))
							+ "\n");
					newProperty += setterComments;
					newProperty += "  set";
					newProperty += ((setterBody == null) ? ";\n" : (" "
							+ getString(setterBody, setterReplacement))
							+ "\n");
					newProperty += "}\n";

					final ASTNode newNode = currentRewriter
							.createStringPlaceholder(newProperty,
									ASTNode.METHOD_DECLARATION);
					currentRewriter.getListRewrite(node,
							node.getBodyDeclarationsProperty()).insertAfter(
							newNode, mgDecl, description);
					currentRewriter.remove(mgDecl, description);
					currentRewriter.remove(msDecl, description);
				} else if (getter == null && setter != null && msDecl != null) {
					// Ok, retrieve body of that method
					Block setterBody = msDecl.getBody();				
					
					final List<MyRegion> setterReplacement = new ArrayList<MyRegion>();
					buildSetterBody(msDecl, "value", 1, setterReplacement);
					//
					final String modifiers = writeModifiers(setter, msDecl);
					String newProperty = "";
					
					// fix : no body for abstract properties
					if (modifiers.contains("abstract")) {
						setterBody = null;
					}
					
					final Type propertyType = getParameterType(msDecl);
					if (propertyType != null) {
						String toAdd = TranslationUtils.getAfterComments(propertyType, fCu, context).trim();
						if (msDecl.getJavadoc() != null) {
							newProperty += DocUtils.processPropertyDoc(this.context, mgDecl, msDecl) + "\n";
						}
						if (context.getConfiguration().getOptions()
								.isAddNotBrowsableAttribute())
							newProperty += EDITOR_BROWSABLE_ATTRIBUTE;
						newProperty += modifiers;
						if (covariantType != null)
							newProperty += covariantType;
						else
							newProperty += ASTNodes.asString(propertyType) + toAdd;
						newProperty += " " + property.getName() + " {\n";
						newProperty += setterComments;
						newProperty += "  set";
						newProperty += ((setterBody == null) ? ";\n" : (" "
								+ getString(setterBody, setterReplacement))
								+ "\n");
						newProperty += "}\n";

						final ASTNode newNode = currentRewriter
								.createStringPlaceholder(newProperty,
										ASTNode.METHOD_DECLARATION);

						currentRewriter.getListRewrite(node,
								node.getBodyDeclarationsProperty())
								.insertAfter(newNode, msDecl, description);
						currentRewriter.remove(msDecl, description);
					}
				} else if (setter == null && getter != null && mgDecl != null) {
					// Ok, retrieve body of that method
					Block getterBody = mgDecl.getBody();					
					//
					final String modifiers = writeModifiers(getter, mgDecl);
					// fix : no body for abstract properties
					if (modifiers.contains("abstract")) {
						getterBody = null;
					}
					String newProperty = "";

					if (mgDecl.getJavadoc() != null || msDecl.getJavadoc() != null) {
						newProperty += DocUtils.processPropertyDoc(this.context, mgDecl, msDecl) + "\n";
					}
					if (context.getConfiguration().getOptions()
							.isAddNotBrowsableAttribute())
						newProperty += EDITOR_BROWSABLE_ATTRIBUTE;
					
					String toAdd = TranslationUtils.getAfterComments(mgDecl.getReturnType2(), fCu, context).trim();
					
					newProperty += modifiers;
					if (covariantType != null)
						newProperty += covariantType;
					else
						newProperty +=(mgDecl.getReturnType2()==null?" ERROR ":ASTNodes.asString(mgDecl.getReturnType2()) + toAdd);
					newProperty += " " + property.getName() + " {\n";
					newProperty += getterComments;
					newProperty += "  get";

					newProperty += ((getterBody == null) ? ";\n" : (" "
							+ getString(getterBody))
							+ "\n");
					newProperty += "}\n";

					final ASTNode newNode = currentRewriter
							.createStringPlaceholder(newProperty,
									ASTNode.METHOD_DECLARATION);

					currentRewriter.getListRewrite(node,
							node.getBodyDeclarationsProperty()).insertAfter(
							newNode, mgDecl, description);

					currentRewriter.remove(mgDecl, description);
				}
			}
		}
		//
		final List<TargetIndexer> indexers = context.getMapper().getIndexers(
				node, context.getHandlerFromDoc(node, false));
		if (indexers != null) {
			for (final TargetIndexer index : indexers) {
				final MethodInfo getter = index.getGetter();
				MethodDeclaration mgDecl = null;
				if (getter != null)
					mgDecl = findMethod(node, getter);
				//
				final MethodInfo setter = index.getSetter();
				MethodDeclaration msDecl = null;
				if (setter != null)
					msDecl = findMethod(node, setter);
				//
				
				// BUG FIX : keep comments
				RemoveCustomCommentsVisitor commentsVisitor = new RemoveCustomCommentsVisitor(context,currentRewriter);
				String getterComments="";
				String setterComments="";
				String tagToAdd = "";
				if (context.getConfiguration().getOptions().getTagToAddtoComment().getValue() != null
						&& !context.getConfiguration().getOptions()
								.getTagToAddtoComment().equals("")) {
					tagToAdd = "/// <"
							+ context.getConfiguration().getOptions()
									.getTagToAddtoComment().getValue() + "/>";
				}
				if ((mgDecl != null) && (mgDecl.getJavadoc() != null)) {
					List elements = mgDecl.getJavadoc().tags();
					if ((elements != null) && (elements.size() > 0)) {
						StringBuffer summary = new StringBuffer();
						StringBuffer buffer = new StringBuffer();
						DocUtils.applyJavadocReplacement(context,
								commentsVisitor.getTags(), elements, summary,
								buffer, tagToAdd, null, false);
						if (!buffer.toString().equals("")) {
							getterComments = buffer.toString()+"\n";
						}
					}					
				}
				
				if ( (msDecl != null) && (msDecl.getJavadoc() != null)) {
					List elements = msDecl.getJavadoc().tags();
					if ((elements != null) && (elements.size() > 0)) {
						StringBuffer summary = new StringBuffer();
						StringBuffer buffer = new StringBuffer();
						DocUtils.applyJavadocReplacement(context,
								commentsVisitor.getTags(), elements, summary,
								buffer, tagToAdd, null, false);
						if (!buffer.toString().equals("")) {
							setterComments = buffer.toString()+"\n";							
						}
					}					
				}
				
				if (getter != null && setter != null && mgDecl != null
						&& msDecl != null) {
					final String argTypeName = getArgTypeNameFromGetter(mgDecl);
					final String argName = getArgNameFromSetter(msDecl);
					// Ok, retrieve body of that method
					Block getterBody = mgDecl.getBody();
					// Ok, retrieve body of that method
					Block setterBody = msDecl.getBody();
					final List<MyRegion> setterReplacement = new ArrayList<MyRegion>();
					buildSetterBody(msDecl, argName, 1, setterReplacement);
					buildSetterBody(msDecl, "value", 2, setterReplacement);
					//
					final String modifiers = writeModifiers(getter, mgDecl);
					String newProperty = "";
					if (modifiers.contains("abstract")) {
						getterBody = null;
						setterBody = null;
					}

					if (mgDecl.getJavadoc() != null || msDecl.getJavadoc() != null) {
						newProperty += DocUtils.processIndexerDoc(this.context, mgDecl, msDecl) + "\n";
					}
					if (context.getConfiguration().getOptions()
							.isAddNotBrowsableAttribute())
						newProperty += EDITOR_BROWSABLE_ATTRIBUTE;
					newProperty += modifiers
							+ ASTNodes.asString(mgDecl.getReturnType2());
					newProperty += " this[" + argTypeName + " " + argName
							+ "] {\n"+getterComments;
					newProperty += "  get";					
					newProperty += ((getterBody == null) ? ";\n" : (" "
							+ getString(getterBody))
							+ "\n");
					newProperty += "  set";
					newProperty += ((setterBody == null) ? ";\n" : (" "
							+ getString(setterBody, setterReplacement))
							+ "\n");
					newProperty += "}\n";

					final ASTNode newNode = currentRewriter
							.createStringPlaceholder(newProperty,
									ASTNode.METHOD_DECLARATION);

					currentRewriter.getListRewrite(node,
							node.getBodyDeclarationsProperty()).insertAfter(
							newNode, mgDecl, description);

					currentRewriter.remove(mgDecl, description);
					currentRewriter.remove(msDecl, description);
				} else if (getter == null && setter != null && msDecl != null) {
					// Ok, retrieve body of that method
					final String argTypeName = getArgTypeNameFromSetter(msDecl);
					final String argName = getArgNameFromSetter(msDecl);
					Block setterBody = msDecl.getBody();
					final List<MyRegion> setterReplacement = new ArrayList<MyRegion>();
					buildSetterBody(msDecl, argName, 1, setterReplacement);
					buildSetterBody(msDecl, "value", 2, setterReplacement);
					//
					final String modifiers = writeModifiers(setter, msDecl);
					String newProperty = "";

					if (modifiers.contains("abstract")) {					
						setterBody = null;
					}
					
					final Type propertyType = getParameterType(msDecl);
					if (propertyType != null) {

						if (mgDecl.getJavadoc() != null || msDecl.getJavadoc() != null) {
							newProperty += DocUtils.processIndexerDoc(this.context, mgDecl, msDecl) + "\n";
						}
						if (context.getConfiguration().getOptions()
								.isAddNotBrowsableAttribute())
							newProperty += EDITOR_BROWSABLE_ATTRIBUTE;
						
						newProperty += modifiers
								+ ASTNodes.asString(propertyType);
						newProperty += " this[" + argTypeName + " " + argName
								+ "] {\n"+setterComments;
						newProperty += "  set";
						newProperty += ((setterBody == null) ? ";\n" : (" "
								+ getString(setterBody, setterReplacement))
								+ "\n");
						newProperty += "}\n";

						final ASTNode newNode = currentRewriter
								.createStringPlaceholder(newProperty,
										ASTNode.METHOD_DECLARATION);

						currentRewriter.getListRewrite(node,
								node.getBodyDeclarationsProperty())
								.insertAfter(newNode, msDecl, description);
						currentRewriter.remove(msDecl, description);
					}
				} else if (setter == null && getter != null && mgDecl != null) {
					// Ok, retrieve body of that method
					Block getterBody = mgDecl.getBody();
					//
					final String argTypeName = getArgTypeNameFromGetter(mgDecl);
					final String argName = getArgNameFromGetter(mgDecl);
					final String modifiers = writeModifiers(getter, mgDecl);
					String newProperty = "";

					if (modifiers.contains("abstract")) {
						getterBody = null;
					}
					
					if (mgDecl.getJavadoc() != null || msDecl.getJavadoc() != null) {
						newProperty += DocUtils.processIndexerDoc(this.context, mgDecl, msDecl) + "\n";
					}					
					if (context.getConfiguration().getOptions()
							.isAddNotBrowsableAttribute())
						newProperty += EDITOR_BROWSABLE_ATTRIBUTE;
					
					newProperty += modifiers
							+ ASTNodes.asString(mgDecl.getReturnType2());
					newProperty += " this[" + argTypeName + " " + argName
							+ "] {\n"+getterComments;
					newProperty += "  get";
					newProperty += ((getterBody == null) ? ";\n" : (" "
							+ getString(getterBody))
							+ "\n");
					newProperty += "}\n";

					final ASTNode newNode = currentRewriter
							.createStringPlaceholder(newProperty,
									ASTNode.METHOD_DECLARATION);

					currentRewriter.getListRewrite(node,
							node.getBodyDeclarationsProperty()).insertAfter(
							newNode, mgDecl, description);

					currentRewriter.remove(mgDecl, description);
				}
			}
		}
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
	
	private String getString(ASTNode node) {
		try {
			final int startposition = node.getStartPosition();
			final int length = node.getLength();
			final String mthd = fCu.getBuffer().getText(startposition, length);
			return mthd;
		} catch (final JavaModelException e) {
			e.printStackTrace();
		}
		return null;
	}

	private String getString(ASTNode node, List<MyRegion> replacements) {
		try {
			final int startposition = node.getStartPosition();
			final int length = node.getLength();
			String mthd = fCu.getBuffer().getText(startposition, length);
			//
			int previousDelta = 0;
			for (final MyRegion region : replacements) {
				mthd = replaceRegion(mthd, region, previousDelta);
				previousDelta += region.getDelta();
			}
			//
			return mthd;
		} catch (final JavaModelException e) {
			e.printStackTrace();
		}
		return null;
	}

	private String replaceRegion(String buffer, MyRegion region, int delta) {
		String res = buffer.substring(0, region.getOffset() + delta);
		res += region.getNewValue();
		res += buffer.substring(
				region.getOffset() + region.getLength() + delta, buffer
						.length());
		return res;
	}

	private static class MyRegion {
		int offset;
		int length;
		String newValue;

		public MyRegion(String value, int offset, int length) {
			newValue = value;
			this.offset = offset;
			this.length = length;
		}

		public int getDelta() {
			return newValue.length() - length;
		}

		public int getOffset() {
			return offset;
		}

		public void setOffset(int offset) {
			this.offset = offset;
		}

		public int getLength() {
			return length;
		}

		public void setLength(int length) {
			this.length = length;
		}

		public String getNewValue() {
			return newValue;
		}

		public void setNewValue(String newValue) {
			this.newValue = newValue;
		}
	}

	/**
	 * put(A, B) we want the B
	 * 
	 * @param mgDecl
	 * @return
	 */
	@SuppressWarnings("unchecked")
	private String getArgNameFromSetter(MethodDeclaration mgDecl) {
		final List parameters = mgDecl.parameters();
		if (parameters != null && parameters.size() == 2) {
			final SingleVariableDeclaration param = (SingleVariableDeclaration) parameters
					.get(0);
			return param.getName().getIdentifier();
		}
		return null;
	}

	@SuppressWarnings("unchecked")
	private String getArgTypeNameFromSetter(MethodDeclaration mgDecl) {
		final List parameters = mgDecl.parameters();
		if (parameters != null && parameters.size() == 2) {
			final SingleVariableDeclaration param = (SingleVariableDeclaration) parameters
					.get(0);
			try {
				return param.getType().resolveBinding().getName();
			} catch (final Exception e) {
				return param.getType().toString();
				/*context.getLogger().logError(
						fCu.getElementName() + " : " + e.getMessage() + " "
								+ e.toString());*/
			}
		}
		return null;
	}

	/**
	 * A get(B) we want the return type
	 * 
	 * @param msDecl
	 * @return
	 */
	@SuppressWarnings("unchecked")
	private String getArgNameFromGetter(MethodDeclaration msDecl) {
		final List parameters = msDecl.parameters();
		if (parameters != null && parameters.size() == 1) {
			final SingleVariableDeclaration param = (SingleVariableDeclaration) parameters
					.get(0);
			return param.getName().getIdentifier();
		}
		return null;
	}

	@SuppressWarnings("unchecked")
	private String getArgTypeNameFromGetter(MethodDeclaration msDecl) {
		final List parameters = msDecl.parameters();
		if (parameters != null && parameters.size() == 1) {
			final SingleVariableDeclaration param = (SingleVariableDeclaration) parameters
			.get(0);
			try {
				return param.getType().resolveBinding().getName();
			} catch (final Exception e) {
				return param.getType().toString();
				/*context.getLogger().logError(
						fCu.getElementName() + " : " + e.getMessage() + " "
								+ e.toString());*/
			}
		}
		return null;
	}

	//
	//
	//

	@SuppressWarnings("unchecked")
	private Type getParameterType(MethodDeclaration methodDecl) {
		// Basically we wan't to writer "value = ....";
		// But we can imagine tricky case where the body in not a simple
		// assignement ... So let start with the simple assingment case
		final List parameters = methodDecl.parameters();
		if (parameters != null && parameters.size() == 1) {
			// normal case
			final SingleVariableDeclaration param = (SingleVariableDeclaration) parameters
					.get(0);
			return param.getType();
		}
		return null;
	}

	@SuppressWarnings("unchecked")
	private void buildSetterBody(MethodDeclaration methodDecl,
			final String newValue, int ind, final List<MyRegion> res) {
		// Basically we wan't to replace reference to setter argument name
		// by newValue
		final List parameters = methodDecl.parameters();
		final Block body = methodDecl.getBody();
		if (body != null) {
			final int mainOffset = body.getStartPosition();
			if (parameters != null && parameters.size() >= ind) {
				// normal case
				final SingleVariableDeclaration param = (SingleVariableDeclaration) parameters
						.get(ind - 1);
				final String paramName = param.getName().getIdentifier();
				// Real action is to replace paramName by newValue ...
				if (body != null) {
					final ASTVisitor visitor = new ASTVisitor() {
						@Override
						public void endVisit(SimpleName node) {
							//Bug fix - was not replacing with value all usages of param 
							if (node.getParent().getNodeType() == ASTNode.FIELD_ACCESS) {
								FieldAccess fa = (FieldAccess) node.getParent();
								if (fa.getExpression() != null && 
										fa.getExpression().getNodeType() == ASTNode.THIS_EXPRESSION)
									return;
							}
							//return;
							// Trouble if there is a local variable class value !
							if (node.getIdentifier().equals(paramName)) {
								// SimpleName replacement =
								// node.getAST().newSimpleName(newValue);
								// currentRewriter.replace(node, replacement,
								// description);
								node.setIdentifier(newValue);
								int offset = node.getStartPosition();
								int length = node.getLength();
								MyRegion r = new MyRegion(newValue, offset
										- mainOffset, length);
								res.add(r);
							}
						}
					};
					body.accept(visitor);
				}
			}
		}
	}

	private String writeModifiers(MethodInfo getter, MethodDeclaration mgDecl) {
		ChangeModifierDescriptor desc = null;
		String targetFramework = context.getConfiguration().getOptions().getTargetDotNetFramework().name();
		
		if (getter.getTarget(targetFramework) != null)
			desc = getter.getTarget(targetFramework)
			.getChangeModifierDescriptor();
		if (desc == null) {
			desc = new ChangeModifierDescriptor();
		}
		getTagFromDoc(mgDecl, desc);

		desc.remove(DotNetModifier.FINAL);
		
		final List<DotNetModifier> toAdd = desc.getModifiersToAdd();
		// we need to find modifier in the targetMethod !
		String res = "";
		for (final Object mod : mgDecl.modifiers()) {
			final Modifier m = (Modifier) mod;
			if (!desc.toRemove(m)) {
				res += ASTNodes.asString(m) + " ";
			}
		}
		
		int flags = mgDecl.getModifiers();
		
		boolean isInInterface = false;

		ASTNode parent = ASTNodes.getParent(mgDecl, ASTNode.TYPE_DECLARATION);
		if (parent != null) {
			TypeDeclaration parentType = (TypeDeclaration) parent;
			if (parentType.isInterface()) {
				isInInterface = true;
			}
		}
		
		//BUG FIX - default from java doesn'n mean default in C#, but "internal"
		if(Flags.isPackageDefault(flags) && !toAdd.contains(DotNetModifier.INTERNAL) && !isInInterface){
			res += DotNetModifier.INTERNAL.getKeyword() + " ";
		}
		
		//BUG FIX - "protected" from java has to be transformed in "protected internal" in C#
		if(Flags.isProtected(flags) && !toAdd.contains(DotNetModifier.INTERNAL)){
			res += DotNetModifier.INTERNAL.getKeyword() + " ";
		}
		
		// BUG FIX : no virtual in interface !
		// see below toAdd is not modifiable ...
		//if (isInInterface && toAdd.contains(DotNetModifier.VIRTUAL))
		//	toAdd.remove(DotNetModifier.VIRTUAL);
		
		for (DotNetModifier dMod : toAdd) {
			if (isInInterface && dMod.equals(DotNetModifier.VIRTUAL))
				res += "";
			else
				res += dMod.getKeyword() + " ";
		}
		return res;

	}

	// copy past from FillDotNetModifier
	@SuppressWarnings("unchecked")
	private void getTagFromDoc(BodyDeclaration node,
			ChangeModifierDescriptor desc) {
		final Javadoc doc = node.getJavadoc();
		boolean virtual = false;
		boolean override = false;
		boolean const_modifier = false;

		if (doc != null) {
			List tags = doc.tags();
			for (int i = 0; i < tags.size(); i++) {
				final TagElement te = (TagElement) tags.get(i);
				final String name = te.getTagName();
				if (name != null) {
					if (name
							.equals(context.getMapper().getTag(
									TranslationModelUtil.VIRTUAL_TAG) /* "@virtual" */)) {
						virtual = true;
					} else if (name
							.equals(context.getMapper().getTag(
									TranslationModelUtil.OVERRIDE_TAG) /* "@override" */)) {
						override = true;
					}
					if (name.equals(context.getMapper().getTag(
							TranslationModelUtil.CONST_TAG) /* "@const" */)) {
						const_modifier = true;
					}
				}
			}
			tags = null;
		}

		if (desc != null) {
			if (virtual) {
				desc.add(DotNetModifier.VIRTUAL);
			} else if (override) {
				desc.add(DotNetModifier.OVERRIDE);
			}
		}
		if (const_modifier) {
			desc.add(DotNetModifier.CONST);
			desc.remove(DotNetModifier.STATIC);
			desc.remove(DotNetModifier.FINAL);
		}
	}

	private MethodDeclaration findMethod(TypeDeclaration typeDecl,
			MethodInfo getter) {
		for (final MethodDeclaration mDecl : typeDecl.getMethods()) {
			// TODO: take into account parameters types
			final String methodNameFromType = mDecl.getName().getIdentifier();
			final String getterName = getter.getName();
			if (methodNameFromType.equals(getterName)) {
				return mDecl;
			} else {
				// case of property
				String targetFramework = context.getConfiguration().getOptions().getTargetDotNetFramework().name();
				
				if (getter.getTarget(targetFramework) != null && 
						getter.getTarget(targetFramework).isMappedToAProperty()) {
					if (methodNameFromType.equalsIgnoreCase(getterName))
						return mDecl;
				}
			}
		}
		return null;
	}
}
	
