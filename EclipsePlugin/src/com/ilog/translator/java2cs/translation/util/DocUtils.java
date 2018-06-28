package com.ilog.translator.java2cs.translation.util;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map.Entry;

import org.eclipse.jdt.core.Flags;
import org.eclipse.jdt.core.ICompilationUnit;
import org.eclipse.jdt.core.IField;
import org.eclipse.jdt.core.IMethod;
import org.eclipse.jdt.core.IType;
import org.eclipse.jdt.core.JavaModelException;
import org.eclipse.jdt.core.Signature;
import org.eclipse.jdt.core.dom.AST;
import org.eclipse.jdt.core.dom.ASTNode;
import org.eclipse.jdt.core.dom.BodyDeclaration;
import org.eclipse.jdt.core.dom.IBinding;
import org.eclipse.jdt.core.dom.ITypeBinding;
import org.eclipse.jdt.core.dom.Javadoc;
import org.eclipse.jdt.core.dom.MethodDeclaration;
import org.eclipse.jdt.core.dom.PackageDeclaration;
import org.eclipse.jdt.core.dom.TagElement;
import org.eclipse.jdt.core.dom.TextElement;
import org.eclipse.jdt.core.dom.TypeDeclaration;
import org.eclipse.jdt.core.dom.rewrite.ASTRewrite;
import org.eclipse.jdt.core.dom.rewrite.ListRewrite;
import org.eclipse.jdt.internal.corext.dom.ASTNodes;
import org.eclipse.jdt.internal.corext.template.java.SignatureUtil;

import com.ilog.translator.java2cs.configuration.info.ClassInfo;
import com.ilog.translator.java2cs.configuration.info.FieldInfo;
import com.ilog.translator.java2cs.configuration.info.MethodInfo;
import com.ilog.translator.java2cs.configuration.info.PackageInfo;
import com.ilog.translator.java2cs.configuration.info.TranslationModelException;
import com.ilog.translator.java2cs.configuration.target.TargetField;
import com.ilog.translator.java2cs.configuration.target.TargetMethod;
import com.ilog.translator.java2cs.configuration.target.TargetProperty;
import com.ilog.translator.java2cs.translation.ITranslationContext;
import com.ilog.translator.java2cs.util.TranslationModelUtil;
import com.ilog.translator.java2cs.util.Utils;

//
// see JavadocTagsSubProcessor
//

// Java Syntax:
// * @author name-text     (classes and interfaces only, required)
//
// * @version     (classes and interfaces only, required. See footnote 1)
//
// * @param    name description   (methods and constructors only)
//
// * @return  description   (methods only)
//
// * @exception class-name description  (@throws is a synonym added in Javadoc 1.2)
//
// * @see reference       
// @see "string"
// @see <a href="URL#value">label</a>
// @see package.class#member label
// Example:
// @see #field : a field in current class 
// @see #Constructor(Type, Type...) : a constructor in current class 
// @see #Constructor(Type id, Type id...) : a constructor in current class
// @see #method(Type, Type,...) : a method in current class
// @see #method(Type id, Type, id...) : a method in current class 
// @see Class : a class in imported packages
// @see Class#field : a field in a class in imported packages 
// @see Class#Constructor(Type, Type...) : a constructor in a class in imported packages 
// @see Class#Constructor(Type id, Type id) : a constructor in a class in imported packages
// @see Class#method(Type, Type,...) : a method in a class in imported packages
// @see Class#method(Type id, Type id,...) : a method in a class in imported packages
// @see package.Class : a class a given package
// @see package.Class#field : a field in a class a given package
// @see package.Class#Constructor(Type, Type...) : a constructor in a class a given package
// @see package.Class#Constructor(Type id, Type id) : a constructor in a class a given package
// @see package.Class#method(Type, Type,...) : a method in a class a given package
// @see package.Class#method(Type id, Type, id) : a method in a class a given package
// @see package : a package
//
// * @since since-text
//
// * @serial field-description     (or @serialField or @serialData)
//
// * @deprecated deprecated-text (see How and When To Deprecate APIs)
// 
// * @throws  class-name  description
// 
// * {@value  package.class#field}
// Example: 
//  The value of this constant is {@value}.
//  Evaluates the script starting with {@value #SCRIPT_START}.
// 
// * {@link package.class#member label}
//
// * {@linkplain package.class#member label}
//
// * {@literal text}
//
// * {@code text}
//
// * {@inheritDoc}
//
// * <p>
// * <pre>
// * <b>
// * <br>
//
// ------------------------------------------------------
//
// DotNet syntax:
//
// c: This tag is used to denote code, so it can be used to identify sample code associated with a 
// particular entity within the source code. Note that the tag can only be used to represent a single 
// line of code or to denote that some of the text on a line should be marked up as code. 
// Example: <c>Dim MyObject As MyType</c>
//
// code: This tag is used to denote more than one line of code, so it can be used to identify longer 
// areas of sample code.
//
// example: This tag may be used to describe an example of using a particular method or class. It is 
// possible to include code samples within the example tag by making use of either the c or code tags.
//
// exception: The exception tag allows a method's exception handling to be documented. The cref attribute 
// allows the namespace of the exception handler to be included. Example: <exception cref="System.Exception" >
// Throws an exception if the customer is not found.</exception>.
//
// include: This tag allows documentation to be imported from an external XML file rather than being 
// stored within the source code itself. As such, it may be useful if the documentation is written by 
// people other than the software developers (e.g. technical writers).
//
// list: This tag allows bulleted or numbered lists or tables to be added to XML comments.
//
// para: This tag indicates that the text within the tags should be formatted within a paragraph. 
// As such, it can be used to format text within other tags such as summary or returns. 
// Example: <para>This is a paragraph</para>
// 
// param: The param tag is used to document a method's arguments. The tag's name attribute specifies 
// the name of the argument to which the tag refers. The tag is also used by Visual Studio's Intellisense 
// system to show a description of a method's arguments. It is also used by the Visual Studio Object Browser. 
// Example: <param name="FileName"> The filename of the file to be loaded.</param>.
//
// paramref: This tag can be used to refer to a method's argument elsewhere within the method's XML comments. 
// The tag's name attribute specifies the name of the argument to which the tag refers. Note that the param tag 
// is actually used to describe the parameter. 
// Example: Use the <paramref name="FileName"/> argument to specify which filename is loaded.
//
// permission: The permission tag can be used to describe any special permissions a specific object needs. 
// The object to which the permission refers is included as the cref attribute. 
// Example: Class needs to write to the file system and ensure the user has appropriate access.
//
// summary: The summary tag describes a method or class, so it is the most important tag. As well as being used 
// in a project's documentation, the tag is also used by Visual Studio's Intellisense system to show a description 
// of the method or class being referenced. It is also used by the Visual Studio Object Browser.
//
// remarks: The remarks tag can be used to supply additional information about a method or class, supplementing 
// the details given in the summary tag. As with the summary tag, this tag is also used by Visual Studio's 
// Intellisense system and the Visual Studio Object Browser.
//
// returns: Describes the return value of a method. 
// Example: <returns>True if user has permission to access the resource, otherwise False.</returns>.
//
// see: The see tag is used to reference other entities (such as classes) in the project. 
// The see tag is intended for use within other tags such as the summary tag. 
// Example: <seealso cref="System.Configuration"/>
//  cref="T:SomeClass"
//  cref="F:SomeClass.myName"
//  cref="M:SomeClass.#ctro"
//  cref="M:SomeClass.SomeMethod(My.Pck.MyClass)"
//  cref="M:SomeClass.SomeOtherMethod"
//  cref="P:SomeClass.Name"
//
// seealso: The seealso tag resembles the see tag and has identical syntax, except that the text 
// is intended to be used to create a separate seealso section for the entity's documentation.
//
// typeparam: Typeparam is used in an identical way to param, except that it is used to document a 
// type associated with a generic class or function.
//
// value: Value is only used when documenting a property and is used to describe the value assigned 
// to that. The comment is not required for properties that are marked as read only. 
// Example: <value>Sets the employee's salary</value>
//
// ----------------------------------------
//
public class DocUtils {

	private static final String CORE_TAG = "@core";
	private static final String JAVAONLY_TAG = "@javaonly";
	private static final String ILOG_INTERNAL_TAG = "@ilog-internal";
	private static final String INTERNAL_TAG = "@internal";
	private static final String START_XML_TAG = " <";
	// private static final String END_EXCEPTION_TAG = "</exception>";
	private static final String START_EXCEPTION_TAG = "<exception cref=\"";
	private static final String END_EXCEPTION_TAG = "</exception>";
	private static final String LF = "\n";
	private static final String END_XML_TAG = "/>";
	private static final String END_RETURNS_TAG = "</returns>";
	private static final String START_RETURNS_TAG = "<returns>";
	private static final String END_SHORT_CODE_TAG = "</c>";
	private static final String START_SHORT_CODE_TAG = "<c>";
	private static final String END_LONG_CODE_TAG = "</code>";
	private static final String START_LONG_CODE_TAG = "<code>";
	private static final String SEEALSO_TAG = "<seealso cref=\"";
	private static final String DOTNET_COMMENT = "///";
	private static final String EXCLUDE = DOTNET_COMMENT + " <exclude/>";
	private static final String END_SUMMARY = DOTNET_COMMENT + " </summary>\n";
	private static final String START_SUMMARY = DOTNET_COMMENT + " <summary>";

	//
	//
	//

	@SuppressWarnings("unchecked")
	public static void addTagsToDoc(ASTRewrite currentRewriter,
			BodyDeclaration node, List<TagElement> tags) {
		if (tags.size() > 0) {
			final AST ast = node.getAST();
			Javadoc doc = node.getJavadoc();
			if (doc == null) {
				doc = ast.newJavadoc();
				for (final TagElement tag : tags) {
					doc.tags().add(tag);
				}
				currentRewriter.set(node, node.getJavadocProperty(), doc, null);
			} else {
				final ListRewrite lr = currentRewriter.getListRewrite(doc,
						Javadoc.TAGS_PROPERTY);
				for (final TagElement tag : tags) {
					lr.insertLast(tag, null);
				}
			}
		}
	}

	//
	//
	//

	public static String cleanDoc(ITranslationContext context,
			ASTRewrite currentRewriter, ASTNode node, Javadoc doc) {
		if (doc != null) {
			try {
				/*
				 * String str = doc.toString(); Tidy tidy = new Tidy();
				 * StringWriter wr = new StringWriter(); tidy.parse(new
				 * StringReader(str), wr); return wr.toString();
				 */
			} catch (Exception e) {
				e.printStackTrace();
				return null;
			}
		}
		return null;
	}

	//
	//
	//
	public static void processDoc(ITranslationContext context,
			ASTRewrite currentRewriter, PackageDeclaration node,
			List<String> tags, String annotationToAdd, String newComments) {
		DocUtils.processDoc(context, currentRewriter, node, node.getJavadoc(),
				tags, annotationToAdd, newComments);
	}

	public static void processDoc(ITranslationContext context,
			ASTRewrite currentRewriter, BodyDeclaration node,
			List<String> tags, String annotationToAdd, String newComments) {
		final Javadoc newJavaDoc = DocUtils.processDoc(context,
				currentRewriter, node, node.getJavadoc(), tags,
				annotationToAdd, newComments);
		if (newJavaDoc != null
				&& node.getNodeType() == ASTNode.METHOD_DECLARATION) {
			currentRewriter.set(node, MethodDeclaration.JAVADOC_PROPERTY,
					newJavaDoc, null);
		}
	}

	@SuppressWarnings("unchecked")
	public static Javadoc processDoc(ITranslationContext context,
			ASTRewrite currentRewriter, ASTNode node, Javadoc doc,
			List<String> tags, String annotationToAdd, String newComments) {
		String tagToAdd = "";
		if (context.getConfiguration().getOptions().getTagToAddtoComment()
				.getValue() != null
				&& !context.getConfiguration().getOptions()
						.getTagToAddtoComment().getValue().equals("")) {
			tagToAdd = DOTNET_COMMENT
					+ START_XML_TAG
					+ context.getConfiguration().getOptions()
							.getTagToAddtoComment().getValue() + END_XML_TAG;
		}
		if (newComments != null) {
			tagToAdd += LF + newComments;
			if (doc != null) {
				final ASTNode replacement = currentRewriter
						.createStringPlaceholder(tagToAdd, node.getNodeType());
				currentRewriter.replace(doc, replacement, null);
			} else {
				return (Javadoc) currentRewriter.createStringPlaceholder(
						tagToAdd, ASTNode.JAVADOC);
			}
		} else if (doc != null) {
			final List elements = doc.tags();
			//
			final String existingHandlerTag = TranslationUtils
					.getTagValueFromDoc((BodyDeclaration) node, context
							.getMapper().getTag(
									TranslationModelUtil.HANDLER_TAG));
			//
			if ((elements == null) || (elements.size() == 0)) {
				String text = annotationToAdd;
				if (text == null) {
					if (tagToAdd != null) {
						text = tagToAdd;
					}
				} else {
					text += LF + tagToAdd;
				}
				if (text == null || text.equals(TranslationUtils.EMPTY_STRING)) {
					currentRewriter.remove(doc, null);
				} else {
					final ASTNode replacement = currentRewriter
							.createStringPlaceholder(text, node.getNodeType());
					currentRewriter.replace(doc, replacement, null);
				}
			} else {
				final StringBuffer summary = new StringBuffer();
				final StringBuffer buffer = new StringBuffer();
				DocUtils.applyJavadocReplacement(context, tags, elements,
						summary, buffer, tagToAdd, existingHandlerTag, false);
				if (buffer.toString().equals(TranslationUtils.EMPTY_STRING)) {
					if (annotationToAdd.equals(TranslationUtils.EMPTY_STRING)) {
						currentRewriter.remove(doc, null);
					} else {
						final ASTNode replacement = currentRewriter
								.createStringPlaceholder(annotationToAdd, doc
										.getNodeType());
						currentRewriter.replace(doc, replacement, null);
					}
				} else {
					if (!annotationToAdd.equals(TranslationUtils.EMPTY_STRING)) {
						buffer.append(LF + annotationToAdd);
					}
					final ASTNode replacement = currentRewriter
							.createStringPlaceholder(buffer.toString(), doc
									.getNodeType());
					currentRewriter.replace(doc, replacement, null);
				}
			}
			return null;
		} else if (!tagToAdd.equals("")) {
			return (Javadoc) currentRewriter.createStringPlaceholder(tagToAdd,
					ASTNode.JAVADOC);
		}
		return null;
	}

	@SuppressWarnings("unchecked")
	public static void applyJavadocReplacement(ITranslationContext context,
			List<String> tags, List elements, StringBuffer summary,
			StringBuffer buffer, String tagToAdd, String handlerTag,
			boolean isAProperty) {
		boolean tagSeen = false;
		String cstValue = extractCstValue(elements);
		final String pckName = extractPackageFromHandler(handlerTag);
		final String className = extractClassFromHandler(handlerTag);
		for (int i = 0; i < elements.size(); i++) {
			final ASTNode astNode = (ASTNode) elements.get(i);
			if (astNode.getNodeType() == ASTNode.TAG_ELEMENT) {
				final TagElement tagElem = (TagElement) astNode;
				final String tagName = tagElem.getTagName();
				if (!tags.contains(tagName)) {
					if (tagName != null) {
						if (!tagSeen) {
							tagSeen = true;
						}
						final List<ASTNode> frag = tagElem.fragments();
						if (tagName.equals(TagElement.TAG_PARAM)) {
							processMethodParamTag(context, buffer, frag,
									pckName, className, cstValue, isAProperty);
						} else if (tagName.equals(TagElement.TAG_RETURN)) {
							processReturnTag(context, buffer, frag, pckName,
									className, cstValue, isAProperty);
						} else if (tagName.equals(TagElement.TAG_LINK)) {
							processLinkTag(context, buffer, frag, pckName,
									className, cstValue);
						} else if (tagName.equals(TagElement.TAG_THROWS)
								|| tagName.equals(TagElement.TAG_EXCEPTION)) {
							processThrowsTag(context, buffer, frag, pckName,
									className, cstValue);
						} else if (tagName.equals(TagElement.TAG_CODE)) {
							processCodeTag(context, buffer, frag, pckName,
									className, cstValue);
						} else if (tagName.equals(TagElement.TAG_SEE)) {
							processSeeTag(context, buffer, frag, pckName,
									className, cstValue);
						} else if (DocUtils.isMappedToExcludeTag(tagName)) {
							tagToAdd += EXCLUDE;
						} else if (DocUtils.isNotMappedJavadocTag(tagName)) {
							// do nothing
						} else {
							final String text = ASTNodes.asString(astNode)
									.replace(" *", DOTNET_COMMENT);
							buffer.append(DocUtils.processJavadocBody(context,
									text, pckName, className, cstValue));
						}
					} else {
						final String text = ASTNodes.asString(astNode).replace(
								" *", DOTNET_COMMENT);
						if (!tagSeen) {
							summary.append(text);
						} else {
							buffer.append(DocUtils.processJavadocBody(context,
									text, pckName, className, cstValue));
						}
					}
				}
			} else {
				final String text = ASTNodes.asString(astNode).replace(" *",
						DOTNET_COMMENT);
				if (!tagSeen) {
					summary.append(text);
				} else {
					buffer.append(DocUtils.processJavadocBody(context, text,
							pckName, className, cstValue));
				}
			}
		}
		if (summary.toString().trim().length() > 0) {
			if (!tagToAdd.equals("")) {
				buffer.insert(0, tagToAdd
						+ LF
						+ START_SUMMARY
						+ DocUtils.processJavadocBody(context, summary
								.toString(), pckName, className, cstValue) + LF
						+ END_SUMMARY + DOTNET_COMMENT);
			} else {
				buffer.insert(0, START_SUMMARY
						+ DocUtils.processJavadocBody(context, summary
								.toString(), pckName, className, cstValue) + LF
						+ END_SUMMARY + DOTNET_COMMENT);
			}
		} else if (!tagToAdd.equals("")) {
			buffer.insert(0, tagToAdd);
		}
	}

	private static String extractCstValue(List elements) {
		if (elements != null) {
			for (Object o : elements) {
				TagElement tag = (TagElement) o;
				if (tag.getTagName() != null
						&& tag.getTagName().equals("@modconstant")) {
					TextElement tt = (TextElement) tag.fragments().get(0);
					return tt.getText().trim();
				}
			}
		}
		return null;
	}

	private static String extractPackageFromHandler(String handlerTag) {
		if (handlerTag == null)
			return null;
		int pckStart = handlerTag.indexOf("<");
		int pckEnd = handlerTag.indexOf("{");
		if (pckStart > 0 && pckEnd > pckStart) {
			return handlerTag.substring(pckStart + 1, pckEnd);
		}
		return null;
	}

	private static String extractClassFromHandler(String handlerTag) {
		if (handlerTag == null)
			return null;
		int pckStart = handlerTag.indexOf("[");
		int pckEnd = handlerTag.indexOf("^");
		if (pckStart > 0 && pckEnd > pckStart) {
			return handlerTag.substring(pckStart + 1, pckEnd);
		}
		pckEnd = handlerTag.indexOf("~");
		if (pckStart > 0 && pckEnd > pckStart) {
			return handlerTag.substring(pckStart + 1, pckEnd);
		}
		pckStart = handlerTag.indexOf("{");
		pckEnd = handlerTag.indexOf(".java");
		if (pckStart > 0 && pckEnd > pckStart) {
			return handlerTag.substring(pckStart + 1, pckEnd);
		}
		return null;
	}

	//
	// process tags
	//
	// Is there a way to 'link' javadoc tag content to the real java element
	// pointed ?

	// TODO: 'See' and its dotnet counterpart 'seealso' acts differently.
	// 'see' is only a link to a class/method/field name
	// 'seealso' has a different syntax for method, class or field ...
	//
	// So we need to :
	// - analyze the see target member
	// - determine the correct format syntax for dotnet 'seealso'
	// - find the .NET equivalent for this java element
	// - gather all and build String for the dotnet doc.
	//
	// @see ClassName-> <seealso></seealso>
	private static void processSeeTag(ITranslationContext context,
			StringBuffer buffer, final List<ASTNode> frag, String pckName,
			String className, String cstValue) {
		String referenceElement = TranslationUtils.EMPTY_STRING;
		String theRest = TranslationUtils.EMPTY_STRING;
		if (frag.size() > 0) {
			referenceElement += ASTNodes.asString((ASTNode) frag.get(0));
			for (int j = 1; j < frag.size(); j++)
				theRest += ASTNodes.asString((ASTNode) frag.get(1));
		}
		String res = findTargetElement(context, pckName, className,
				referenceElement.trim(), true);
		buffer.append(LF + DOTNET_COMMENT + " " + SEEALSO_TAG + res + "\""
				+ END_XML_TAG);
		if (!theRest.isEmpty())
			buffer.append(LF + DOTNET_COMMENT + " " + theRest);
	}

	// So we need to :
	// - analyze the see target member
	// - determine the correct format syntax for dotnet doc
	// - find the .NET equivalent for this java element
	// - gather all and build String for the dotnet doc.
	private static void processThrowsTag(ITranslationContext context,
			StringBuffer buffer, final List<ASTNode> frag, String pckName,
			String className, String cstValue) {
		String exceptionClass = TranslationUtils.EMPTY_STRING;
		String description = TranslationUtils.EMPTY_STRING;
		if (frag.size() > 0) {
			exceptionClass = ASTNodes.asString((ASTNode) frag.get(0));
			for (int j = 1; j < frag.size(); j++)
				description += ASTNodes.asString((ASTNode) frag.get(j));
		}
		try {
			IType itype = context.getConfiguration().getOriginalProject()
					.findType(exceptionClass.trim());
			if (itype != null) {
				ClassInfo ci = context.getModel().findClassInfo(
						itype.getHandleIdentifier(), false, false, false);
				String targetFramework = context.getConfiguration().getOptions().getTargetDotNetFramework().name();
				
				if (ci != null && ci.getTarget(targetFramework) != null) {
					String targetName = ci.getTarget(targetFramework).getName();
					buffer.append(LF
							+ DOTNET_COMMENT
							+ " "
							+ START_EXCEPTION_TAG
							+ targetName
							+ "\">"
							+ DocUtils.processJavadocBody(context, description
									.trim(), pckName, className, cstValue)
							+ END_EXCEPTION_TAG);
					return;
				} else {
					String pck = itype.getPackageFragment().getElementName();
					PackageInfo pInfo = context.getModel().findPackageInfo(pck, context.getConfiguration().getOriginalProject().getProject());
					if (pInfo != null && pInfo.getTarget(targetFramework) != null) {
						buffer.append(LF
								+ DOTNET_COMMENT
								+ " "
								+ START_EXCEPTION_TAG
								+ pInfo.getTarget(targetFramework).getName() + "." + itype.getElementName()
								+ "\">"
								+ DocUtils.processJavadocBody(context, description
										.trim(), pckName, className, cstValue)
								+ END_EXCEPTION_TAG);
						return;
					}
				}
			}
		} catch (Exception e) {

		}
		buffer.append(LF
				+ DOTNET_COMMENT
				+ " "
				+ START_EXCEPTION_TAG
				+ exceptionClass
				+ "\">"
				+ DocUtils.processJavadocBody(context, description.trim(),
						pckName, className, cstValue) + END_EXCEPTION_TAG);
	}

	// So we need to :
	// - analyze the see target member
	// - determine the correct format syntax for dotnet doc
	// - find the .NET equivalent for this java element
	// - gather all and build String for the dotnet doc.
	//
	// If CODE is single line of code
	// <code>CODE</code> -> <c>CODE</c>
	// If CODE is multi-line
	// <code>CODE</code> -> <code>CODE</code>
	private static void processCodeTag(ITranslationContext context,
			StringBuffer buffer, final List<ASTNode> frag, String pckName,
			String className, String cstValue) {
		String body = TranslationUtils.EMPTY_STRING;
		if (frag.size() > 0) {
			for (int j = 0; j < frag.size(); j++) {
				body += ASTNodes.asString((ASTNode) frag.get(j));
			}
		}
		if (containsLF(body.toCharArray(), 0, body.length())) {
			buffer.append(LF
					+ DOTNET_COMMENT
					+ " "
					+ START_LONG_CODE_TAG
					+ DocUtils.processJavadocBody(context, body.trim(),
							pckName, className, cstValue) + END_LONG_CODE_TAG);
		} else {
			buffer.append(LF
					+ DOTNET_COMMENT
					+ " "
					+ START_SHORT_CODE_TAG
					+ DocUtils.processJavadocBody(context, body.trim(),
							pckName, className, cstValue) + END_SHORT_CODE_TAG);
		}
	}

	private static String scanCodeTag(ITranslationContext context,
			String pckName, String className, String javaDocBody,
			String jElement) {
		char[] buffer = javaDocBody.toCharArray();
		StringBuilder newBuffer = new StringBuilder();
		int index = 0;
		int END = buffer.length;
		while (index < END) {
			char current = buffer[index];
			if (current == '<') {
				index++;
				int startIdx = index;
				// enter in searchable zone
				while (index < END && buffer[index] != '>') {
					index++;
				}
				// END of buffer or ' '
				if (index <= END) {
					int endIdx = index;
					String tagName = getString(buffer, startIdx, endIdx);
					if (tagName.equals("code")) {
						// search for </code>
						while (index < END - 1 && buffer[index] != '<' && buffer[index + 1] != '/') {
							index++;
						}
						if (index <= END) {
							int startIdx2 = index + 2;
							while (index < END && buffer[index] != '>') {
								index++;
							}
							// END of buffer or ' '
							if (index <= END) {
								int endIdx2 = index;
								index++; // skip final '>'
								String tagName2 = getString(buffer, startIdx2,
										endIdx2);
								if (tagName2.equals("code")) {
									String codeBody = getString(buffer, endIdx + 1,
											startIdx2 - 2);
									if (containsLF(buffer,
											endIdx + 1, startIdx2 - 2)) {
										String res = START_LONG_CODE_TAG
												+ DocUtils.processJavadocBody(
														context, codeBody,
														pckName, className,
														jElement)
												+ END_LONG_CODE_TAG;
										newBuffer.append(res);
									} else {
										String res = START_SHORT_CODE_TAG
												+ DocUtils.processJavadocBody(
														context, codeBody
																.trim(),
														pckName, className,
														jElement)
												+ END_SHORT_CODE_TAG;
										newBuffer.append(res);
									}
								} else {
									// add from index startIdx2 to index
									newBuffer.append(getString(buffer, startIdx - 1,
											index + 1));
									index++;
								}
							}
						}
					} else {
						// add from index startIdx2 to index
						newBuffer.append(getString(buffer, startIdx - 1,
								index + 1));
						index++;
					}
				}
			} else {
				newBuffer.append(current);
				index++;
			}
		}
		return newBuffer.toString();
	}

	private static String scanForCodeTag(ITranslationContext context,
			String pckName, String className, String javaDocBody,
			String jElement) {

		int indexOfStartCode = javaDocBody.indexOf("<code>");
		while (indexOfStartCode >= 0) {
			int indeOfEndCode = javaDocBody.indexOf("</code>");
			if (indeOfEndCode > 0) {
				String codeBody = javaDocBody.substring(indexOfStartCode + 6,
						indeOfEndCode);
				String before = javaDocBody.substring(0, indexOfStartCode);
				String after = javaDocBody.substring(indeOfEndCode + 7);

				if (containsLF(javaDocBody.toCharArray(), indexOfStartCode + 6,
						indeOfEndCode)) {
					String res = START_LONG_CODE_TAG
							+ DocUtils.processJavadocBody(context, codeBody,
									pckName, className, jElement)
							+ END_LONG_CODE_TAG;
					javaDocBody = before + res + after;
				} else {
					String res = START_SHORT_CODE_TAG
							+ DocUtils.processJavadocBody(context, codeBody
									.trim(), pckName, className, jElement)
							+ END_SHORT_CODE_TAG;
					javaDocBody = before + res + after;
				}
			} else {
				return javaDocBody;
			}
			indexOfStartCode = javaDocBody.indexOf("<code>", indeOfEndCode);
		}
		return javaDocBody;
	}

	private static boolean containsLF(char[] buffer, int start, int end) {
		int index = start;
		while (index < end) {
			if (10 == buffer[index++]) {
				return true;
			}
		}
		return false;
	}

	// So we need to :
	// - analyze the see target member
	// - determine the correct format syntax for dotnet doc
	// - find the .NET equivalent for this java element
	// - gather all and build String for the dotnet doc.
	//
	// @link -> <seealso></seealso>
	private static void processLinkTag(ITranslationContext context,
			StringBuffer buffer, final List<ASTNode> frag, String pckName,
			String className, String cstValue) {
		String body = TranslationUtils.EMPTY_STRING;
		if (frag.size() > 0) {
			for (int j = 0; j < frag.size(); j++)
				body += ASTNodes.asString((ASTNode) frag.get(j));
		}
		buffer.append(LF
				+ DOTNET_COMMENT
				+ " "
				+ SEEALSO_TAG
				+ DocUtils.processJavadocBody(context, body.trim(), pckName,
						className, cstValue) + "\"" + END_XML_TAG);
	}

	// So we need to :
	// - analyze the see target member
	// - determine the correct format syntax for dotnet doc
	// - find the .NET equivalent for this java element
	// - gather all and build String for the dotnet doc.
	// 
	// @return -> <return>
	private static void processReturnTag(ITranslationContext context,
			StringBuffer buffer, final List<ASTNode> frag, String pckName,
			String className, String cstValuye, boolean isAProperty) {
		String retText = TranslationUtils.EMPTY_STRING;
		if (frag.size() > 0) {
			for (int j = 0; j < frag.size() - 1; j++)
				retText += ASTNodes.asString((ASTNode) frag.get(j)) + "\n/// ";
			retText += ASTNodes.asString((ASTNode) frag.get(frag.size() - 1));
		}
		if (isAProperty) {
			// only for !readonly ....
			buffer.append(LF
					+ DOTNET_COMMENT
					+ " "
					+ "<value>"
					+ DocUtils.processJavadocBody(context, retText.trim(),
							pckName, className, cstValuye) + "</value>");
		} else {
			buffer.append(LF
					+ DOTNET_COMMENT
					+ " "
					+ START_RETURNS_TAG
					+ DocUtils.processJavadocBody(context, retText.trim(),
							pckName, className, cstValuye) + END_RETURNS_TAG);
		}
	}

	// So we need to :
	// - analyze the see target member
	// - determine the correct format syntax for dotnet doc
	// - find the .NET equivalent for this java element
	// - gather all and build String for the dotnet doc.
	//
	// @param name description -> <param name="name"\>description</param>
	private static void processMethodParamTag(ITranslationContext context,
			StringBuffer buffer, final List<ASTNode> frag, String pckName,
			String className, String cstValue, boolean isAProperty) {
		String argName = TranslationUtils.EMPTY_STRING;
		String body = TranslationUtils.EMPTY_STRING;
		if (frag.size() > 0) {
			argName = ASTNodes.asString((ASTNode) frag.get(0));
			if (frag.size() > 1) {
				for (int j = 1; j < frag.size(); j++)
					body += ASTNodes.asString((ASTNode) frag.get(j));
			}
		}
		if (isAProperty) {
			// only for !readonly ....
			buffer.append(LF
					+ DOTNET_COMMENT
					+ " "
					+ "<value>"
					+ DocUtils.processJavadocBody(context, body.trim(),
							pckName, className, cstValue) + "</value>");
		} else {
			buffer.append(LF
					+ DOTNET_COMMENT
					+ " "
					+ "<param name=\""
					+ argName
					+ "\""
					+ ">"
					+ DocUtils.processJavadocBody(context, body.trim(),
							pckName, className, cstValue) + "</param>");
		}
	}

	// TODO: Much more complicated than that ..
	// We can have "@link" ...
	public static String processJavadocBody(ITranslationContext context,
			String originalText, String pckName, String className,
			String cstValue) {

		final HashMap<String, String> mappings = context.getMapper()
				.getJavaDocMappings();
		String res = originalText;
		for (final Entry<String, String> entry : mappings.entrySet()) {
			// So we need to :
			// - analyze the see target member
			// - determine the correct format syntax for dotnet tag
			// - find the .NET equivalent for this java element
			// - gather all and build String for the dotnet doc.
			res = res.replace(entry.getKey(), entry.getValue());
		}
		//
		res = scanForPlainTag(context, pckName, className, cstValue, res);
		res = res.replace("*", "///"); // FIXME:
		res = scanCodeTag(context, pckName, className, res, cstValue);
		return res;
	}

	private static String scanForPlainTag(ITranslationContext context,
			String pckName, String className, String cstValue, String res) {
		char[] buffer = res.toCharArray();
		StringBuilder newBuffer = new StringBuilder();
		int index = 0;
		int END = buffer.length;
		while (index < END) {
			char current = buffer[index];
			if (index < END - 1) {
				char next = buffer[index + 1];
				if (current == '{' && next == '@') {
					index++;
					int startIdx = index + 1;
					// enter in searchable zone
					while (index < END && buffer[index] != ' ') {
						index++;
					}
					// END of buffer or ' '
					if (index <= END) {
						int endNameIdx = index;
						int startValueIDx = index + 1;
						// end of searchable zone
						while (index < END && buffer[index] != '}') {
							index++;
						}
						if (index <= END) {
							int endOfTag = index;
							String tagValue = getString(buffer, startValueIDx,
									endOfTag);
							String tagName = getString(buffer, startIdx,
									endNameIdx);
							if (tagName.equals("linkplain")) {
								String see = "<see cref=\""
										+ findTargetElement(context, pckName,
												className, tagValue, true)
										+ "\"/>";
								newBuffer.append(see);
								index++;
							} else if (tagName.equals("link")) {
								String see = "<see cref=\""
										+ findTargetElement(context, pckName,
												className, tagValue, true)
										+ "\"/>";
								newBuffer.append(see);
								index++;
							} else if (tagName.equals("literal")) {
								String see = "<b>" + tagValue.trim() + "</b>";
								newBuffer.append(see);
								index++;
							} else if (tagName.equals("value")) {
								String see = "<b>" + cstValue + "</b>";
								newBuffer.append(see);
								index++;
							}
						} else {
							newBuffer.append(current);
							index++;
						}
					}
				} else {
					newBuffer.append(current);
					index++;
				}
			} else {
				newBuffer.append(current);
				index++;
			}
		}
		return newBuffer.toString();
		/*
		 * int beginAtLinkplanIndex = res.indexOf("{@linkplain"); while
		 * (beginAtLinkplanIndex >= 0) { int endAtLinkplanIndex =
		 * res.indexOf("}", beginAtLinkplanIndex); if (endAtLinkplanIndex >=
		 * beginAtLinkplanIndex) { String reference =
		 * res.substring(beginAtLinkplanIndex + 7 + 5, endAtLinkplanIndex);
		 * String see = "<see cref=\"" + findTargetElement(context, pckName,
		 * className, reference, true) + "\"/>"; res = res.substring(0,
		 * beginAtLinkplanIndex) + see + res.substring(endAtLinkplanIndex + 1);
		 * beginAtLinkplanIndex = res.indexOf("{@linkplain"); } } int
		 * beginAtLinkIndex = res.indexOf("{@link"); while (beginAtLinkIndex >=
		 * 0) { int endAtLinkIndex = res.indexOf("}", beginAtLinkIndex); if
		 * (endAtLinkIndex >= beginAtLinkIndex) { String reference =
		 * res.substring(beginAtLinkIndex + 7, endAtLinkIndex); String see =
		 * "<see cref=\"" + findTargetElement(context, pckName, className,
		 * reference, true) + "\"/>"; res = res.substring(0, beginAtLinkIndex) +
		 * see + res.substring(endAtLinkIndex + 1); beginAtLinkIndex =
		 * res.indexOf("{@link"); } } int beginAtLiteralIndex =
		 * res.indexOf("{@literal"); while (beginAtLiteralIndex >= 0) { int
		 * endAtLiteralIndex = res.indexOf("}", beginAtLiteralIndex); if
		 * (endAtLiteralIndex >= beginAtLiteralIndex) { String reference =
		 * res.substring(beginAtLiteralIndex + 10, endAtLiteralIndex); String
		 * see = "<b>" + reference.trim() + "</b>"; res = res.substring(0,
		 * beginAtLiteralIndex) + see + res.substring(endAtLiteralIndex + 1);
		 * beginAtLiteralIndex = res.indexOf("{@literal"); } } int
		 * beginAtValueIndex = res.indexOf("{@value"); while (beginAtValueIndex
		 * >= 0) { int endAtValueIndex = res.indexOf("}", beginAtValueIndex); if
		 * (endAtValueIndex >= beginAtValueIndex) { String see = "<b>" +
		 * cstValue + "</b>"; res = res.substring(0, beginAtValueIndex) + see +
		 * res.substring(endAtValueIndex + 1); beginAtValueIndex =
		 * res.indexOf("{@value"); } } return res;
		 */
	}

	private static String getString(char[] buffer, int start, int end) {
		int index = start;
		StringBuilder res = new StringBuilder();
		while (index < buffer.length && index < end) {
			res.append(buffer[index++]);
		}
		return res.toString();
	}

	private static String findTargetElement(ITranslationContext context,
			String refPackageName, String refClassName, String element,
			boolean withPrefix) {
		try {
			int sharpIndex = element.indexOf('#');
			if (sharpIndex == 0) {
				IType refType = context.getConfiguration().getOriginalProject()
						.findType(refPackageName + "." + refClassName);
				String prefix = "";
				if (withPrefix)
					prefix = findPrefix(context, refType, element.substring(1));
				return findTargetElementOnModel(context, refPackageName,
						element.replace("#", "."), prefix, refType);
			} else if (sharpIndex > 0) {
				String otherRefClass = element.substring(0, sharpIndex);
				IType refType = context.getConfiguration().getOriginalProject()
				.findType(otherRefClass);
				if (refType == null) {
					refType = context.getConfiguration().getOriginalProject()
					.findType(refPackageName + "." + otherRefClass);
				}
				String prefix = "";
				if (withPrefix)
					prefix = findPrefix(context, refType, element.substring(1));
				return findTargetElementOnModel(context, refPackageName,
						element.substring(sharpIndex+1), prefix, refType);
			}
			String prefix = "T:"; // a type
			if (!withPrefix)
				prefix = "";
			if (element.equals("int")) {
				return prefix + "System.Int32";
			}
			if (element.equals("char")) {
				return prefix + "System.Character";
			}
			if (element.equals("long")) {
				return prefix + "System.Int64";
			}
			if (element.equals("float")) {
				return prefix + "System.Single";
			}
			if (element.equals("double")) {
				return prefix + "System.Double";
			}
			if (element.equals("byte")) {
				return prefix + "System.Byte";
			}
			if (element.equals("short")) {
				return prefix + "System.Int16";
			}
			if (element.equals("boolean")) {
				return prefix + "System.Boolean";
			}
			String elementName = null;
			String className = element;
			if (!element.contains(".") && refPackageName != null) {
				className = refPackageName + "." + element;
				sharpIndex = className.indexOf('#');
			}
			if (sharpIndex > 0) {
				elementName = className.substring(sharpIndex + 1);
				className = className.substring(0, sharpIndex);
			} else {
				// case of no '#' -> a class name.
			}
			// add here support for method and field ref.
			IType itype = context.getConfiguration().getOriginalProject()
					.findType(className);
			if (itype == null && refPackageName != null) {
				itype = context.getConfiguration().getOriginalProject()
						.findType(refPackageName + "." + className);
			}
			if (itype != null) {
				if (withPrefix && sharpIndex > 0) {
					prefix = findPrefix(context, itype, elementName);
				}
				return findTargetElementOnModel(context, refPackageName,
						elementName, prefix, itype);
			} else if (!element.contains(".")) {
				// default package lookup
				// TODO: should be all imported packages ?
				itype = context.getConfiguration().getOriginalProject()
						.findType("java.lang." + element);
				if (itype != null) {
					if (withPrefix && sharpIndex > 0) {
						prefix = findPrefix(context, itype, elementName);
					}
					return findTargetElementOnModel(context, "java.lang",
							elementName, prefix, itype);
				}
			}
			return null;
		} catch (Exception e) {
			return null;
		}
	}

	private static String findTargetElementOnModel(ITranslationContext context,
			String refPackageName, String elementName, String prefix,
			IType itype) throws TranslationModelException, JavaModelException {
		ClassInfo ci = context.getModel().findClassInfo(
				itype.getHandleIdentifier(), false, false, false);
		if (elementName != null && elementName.startsWith("."))
			elementName = elementName.substring(1);
		if (ci != null) {
			if (prefix.equals("F:")) {
				elementName = findFieldOnModel(elementName, itype, ci);
			} else if (prefix.equals("M:")) {
				// tricky case
				elementName = findMethodOnModel(context, refPackageName,
						elementName, itype, ci);
				// could contains a "P:" when a methods is turn into a Property !				
				if (elementName.contains(":")) {
					int idx = elementName.indexOf(":");
					prefix = elementName.substring(0, idx + 1);
					elementName = elementName.substring(idx + 1);
				}
			}
			String res = prefix;
			String targetFramework = context.getConfiguration().getOptions().getTargetDotNetFramework().name();
			
			if (ci.getTarget(targetFramework) != null) {
				String targetClassName = ci.getTarget(targetFramework).getName();
				res += targetClassName;
			} else {
				PackageInfo pInfo = context.getModel().findPackageInfo(
						refPackageName,
						context.getConfiguration().getOriginalProject()
								.getProject());
				if (pInfo != null && pInfo.getTarget(targetFramework) != null ) {
					res += pInfo.getTarget(targetFramework).getName() + "." + itype.getElementName();
				} else {
					res += Utils.capitalize(refPackageName) + "."
							+ itype.getElementName();
				}
			}
			res += (elementName == null ? "" : "." + elementName);
			return res;
		} else {
			PackageInfo pInfo = context.getModel().findPackageInfo(
					refPackageName,
					context.getConfiguration().getOriginalProject()
							.getProject());
			String res = prefix;
			String targetFramework = context.getConfiguration().getOptions().getTargetDotNetFramework().name();
			
			if (pInfo != null && pInfo.getTarget(targetFramework) != null) {
				res += pInfo.getTarget(targetFramework).getName() + "." + itype.getElementName();
			} else if (refPackageName != null) {
				res += Utils.capitalize(refPackageName) + "."
						+ itype.getElementName();
			} else {
				res += itype.getElementName();
			}
			res += (elementName == null ? "" : "." + elementName);
			return res;
		}
	}

	private static String findMethodOnModel(ITranslationContext context,
			String refPackageName, String elementName, IType itype, ClassInfo ci)
			throws JavaModelException {
		String mName = extractMethodNameFromDocSignature(elementName);
		//
		String[] parameterTypeSignatures = extractParameterTypeDocSignature(elementName);

		IMethod m = null;

		if (parameterTypeSignatures != null && parameterTypeSignatures.length > 0) {
			String[] pSignature = new String[parameterTypeSignatures.length];
			for (int i = 0; i < parameterTypeSignatures.length; i++) {
				pSignature[i] = Signature.createTypeSignature(
						parameterTypeSignatures[i], true);
				pSignature[i] = SignatureUtil.qualifySignature(pSignature[i],
						itype);
			}
			m = itype.getMethod(mName, pSignature);
		} else {
			IMethod[] methods = itype.getMethods();
			for (IMethod candidate : methods) {
				if (candidate.getElementName().equals(mName)) {
					m = candidate;
					String[] pTypeSignatures = m.getParameterTypes();
					parameterTypeSignatures = new String[pTypeSignatures.length];
					for (int i = 0; i < pTypeSignatures.length; i++) {
						if (pTypeSignatures[i].equals("QString;"))
							parameterTypeSignatures[i] = "java.lang.String";
						else
							parameterTypeSignatures[i] = Signature
									.toString(pTypeSignatures[i]);
					}
					break;
				}
			}
		}

		try {
			String sig = TranslationUtils.computeSignature(m) + mName;
			MethodInfo mInfo = ci.getMethod(sig, null);
			String targetFramework = context.getConfiguration().getOptions().getTargetDotNetFramework().name();
			
			if (mInfo != null && mInfo.getTarget(targetFramework) != null) {
				TargetMethod tm = mInfo.getTarget(targetFramework);
				if (tm instanceof TargetProperty) {
					if (tm.getName() != null)
						elementName = "P:" + tm.getName();
					if (((TargetProperty) tm).getPropertyGetValue() != null)
						elementName = "P:" + ((TargetProperty) tm).getPropertyGetValue();
					if (((TargetProperty) tm).getPropertySetValue() != null) 
						elementName = "P:" + ((TargetProperty) tm).getPropertySetValue();
				} else if (tm.getPattern() == null) {
					elementName = buildSignature(context, refPackageName, ci,
							parameterTypeSignatures, tm.getName());
				} else {
					elementName = tm.getName(); // + "(" + tm.getPattern() + ")";
				}
			} else {
				if (parameterTypeSignatures != null && parameterTypeSignatures.length > 0) {
					elementName = buildSignature(context, refPackageName, ci,
							parameterTypeSignatures, Utils.capitalize(mName));
				} else
					elementName = Utils.capitalize(mName);
			}
		} catch (Exception e) {
			if (parameterTypeSignatures != null) {
				// no mapping found ...
				elementName = buildSignature(context, refPackageName, ci,
						parameterTypeSignatures, Utils.capitalize(mName));
			} else {
				elementName = Utils.capitalize(mName);
			}
		}
		return elementName;
	}

	private static String findFieldOnModel(String elementName, IType itype,
			ClassInfo ci) throws JavaModelException {
		IField f = itype.getField(elementName);
		if (f != null && f.exists()) {
			FieldInfo fInfo = ci.getField(f);
			String targetFramework = ci.getPackageInfo().getMappingInfo().getTranslateInfo().getConfiguration().getOptions().getTargetDotNetFramework().name();
			
			if (fInfo != null && fInfo.getTarget(targetFramework) != null) {
				TargetField tf = fInfo.getTarget(targetFramework);
				elementName = tf.getName();
			}
		}
		return elementName;
	}

	private static String buildSignature(ITranslationContext context,
			String refPackageName, ClassInfo ci,
			String[] parameterTypeSignatures, String mName) {
		String signature = "(";
		for (int i = 0; i < parameterTypeSignatures.length - 1; i++) {
			String replacement = lookForMappingInModel(context, refPackageName,
					ci.getName(), parameterTypeSignatures[i], false);
			signature += replacement + ", ";
		}
		signature += lookForMappingInModel(context, refPackageName, ci
				.getName(),
				parameterTypeSignatures[parameterTypeSignatures.length - 1],
				false)
				+ ")";
		return mName + signature;
	}

	//
	// remove tags
	//

	private static String lookForMappingInModel(ITranslationContext context,
			String refPackageName, String refClassName, String type,
			boolean withPrefix) {
		String res = findTargetElement(context, refPackageName, refClassName,
				type, withPrefix);
		return res;
	}

	private static String[] extractParameterTypeDocSignature(String elementName) {
		int noarg = elementName.indexOf("()");
		if (noarg > 0)
			return null;
		int index = elementName.indexOf("(");
		if (index > 0) {
			String params = elementName.substring(index + 1, elementName
					.length() - 1);
			String[] arrayOfParams = params.split(",");
			if (arrayOfParams != null && arrayOfParams.length > 0) {
				String[] res = new String[arrayOfParams.length];
				for (int i = 0; i < arrayOfParams.length; i++) {
					int indexOfWS = arrayOfParams[i].indexOf(" ");
					if (indexOfWS > 0) {
						res[i] = arrayOfParams[i].substring(0, indexOfWS);
					} else {
						res[i] = arrayOfParams[i];
					}
					// res[i] = Signature.createTypeSignature(res[i], true);
				}
				return res;
			}
		}
		return null;
	}

	private static String extractMethodNameFromDocSignature(String elementName) {
		int index = elementName.indexOf("(");
		if (index > 0) {
			return elementName.substring(0, index);
		}
		return elementName;
	}

	// / Find the correct prefix (T:, M:, F: or P:)
	private static String findPrefix(ITranslationContext context, IType itype,
			String elementName) {
		if (elementName == null || elementName.isEmpty())
			return "T:";
		if (elementName.contains("("))
			return "M:";
		if (elementName.startsWith("#")) {
			elementName = elementName.substring(1);
		}
		IField f = itype.getField(elementName);
		if (f != null && f.exists()) {
			// could be a field OR a property
			// need to ask to the context to be sure
			return "F:";
		}
		return "M:";
	}

	public static void removeDocTags(ITranslationContext context,
			ASTRewrite currentRewriter, BodyDeclaration node,
			List<String> tags, String annotationToAdd) {
		final Javadoc newJavaDoc = DocUtils
				.removeDocTags(context, currentRewriter, node, node
						.getJavadoc(), tags, annotationToAdd);
		if (newJavaDoc != null
				&& node.getNodeType() == ASTNode.METHOD_DECLARATION) {
			currentRewriter.set(node, MethodDeclaration.JAVADOC_PROPERTY,
					newJavaDoc, null);
		}
	}

	@SuppressWarnings("unchecked")
	public static Javadoc removeDocTags(ITranslationContext context,
			ASTRewrite currentRewriter, ASTNode node, Javadoc doc,
			List<String> tags, String annotationToAdd) {
		String tagToAdd = "";
		if (context.getConfiguration().getOptions().getTagToAddtoComment()
				.getValue() != null
				&& !context.getConfiguration().getOptions()
				// any tag to add from configuration file ?
						.getTagToAddtoComment().getValue().equals("")) {
			tagToAdd = DOTNET_COMMENT
					+ START_XML_TAG
					+ context.getConfiguration().getOptions()
							.getTagToAddtoComment().getValue() + END_XML_TAG;
		}
		if (doc != null) {
			final List elements = doc.tags();
			if ((elements == null) || (elements.size() == 0)) {
				// no javadoc
				String text = annotationToAdd;
				if (text == null) {
					if (tagToAdd != null) {
						text = tagToAdd;
					}
				} else {
					text += LF + tagToAdd;
				}
				if (text == null || text.equals(TranslationUtils.EMPTY_STRING)) {
					currentRewriter.remove(doc, null);
				} else {
					final ASTNode replacement = currentRewriter
							.createStringPlaceholder(text, node.getNodeType());
					currentRewriter.replace(doc, replacement, null);
				}
			} else {
				// just remove the custom comments
				for (int i = 0; i < elements.size(); i++) {
					final ASTNode astNode = (ASTNode) elements.get(i);
					if (astNode.getNodeType() == ASTNode.TAG_ELEMENT) {
						final TagElement tagElem = (TagElement) astNode;
						final String tagName = tagElem.getTagName();
						if (tags.contains(tagName)) {
							currentRewriter.remove(tagElem, null);
						}
					}
				}
				ListRewrite lr = currentRewriter.getListRewrite(doc,
						Javadoc.TAGS_PROPERTY);
				//
				TagElement zzz = (TagElement) currentRewriter
						.createStringPlaceholder(tagToAdd, ASTNode.TAG_ELEMENT);
				lr.insertLast(zzz, null);
			}
			return null;
		} else if (!tagToAdd.equals("")) {
			return (Javadoc) currentRewriter.createStringPlaceholder(tagToAdd,
					ASTNode.JAVADOC);
		}
		return null;
	}

	static boolean isMappedToExcludeTag(String tagName) {
		return tagName.equals(INTERNAL_TAG)
				|| tagName.equals(ILOG_INTERNAL_TAG)
				|| tagName.equals(JAVAONLY_TAG);
	}

	static boolean isNotMappedJavadocTag(String tagName) {
		return tagName.equals(TagElement.TAG_AUTHOR)
				|| tagName.equals(TagElement.TAG_SINCE)
				|| tagName.equals(TagElement.TAG_DEPRECATED)
				|| tagName.equals(TagElement.TAG_VERSION)
				|| tagName.equals(CORE_TAG)
				|| tagName.equals(TagElement.TAG_THROWS);
	}

	@SuppressWarnings("unchecked")
	public static void createBindingTag(ITranslationContext context,
			ICompilationUnit fCu, BodyDeclaration node, IBinding binding,
			List<TagElement> tags) {
		final AST ast = node.getAST();

		String handlerKey = null;

		if (binding == null) {
			if (node.getNodeType() == ASTNode.INITIALIZER) {
				final TypeDeclaration parent = ((TypeDeclaration) ASTNodes
						.getParent(node, ASTNode.TYPE_DECLARATION));
				handlerKey = (Flags.isStatic(node.getModifiers()) ? "static"
						: TranslationUtils.EMPTY_STRING)
						+ "<init>" + parent.resolveBinding().getName();
			} else {
				context.getLogger().logError(
						"creatBindingTag : error binding == null for : " + node
								+ " / " + fCu.getElementName());
				return;
			}
		} else {
			handlerKey = binding.getJavaElement().getHandleIdentifier();
		}

		// create handler key
		final TagElement handlerTag = ast.newTagElement();
		handlerTag.setTagName(context.getMapper().getTag(
				TranslationModelUtil.HANDLER_TAG));
		final TextElement tE = ast.newTextElement();
		tE.setText(Utils.mangle(handlerKey));
		handlerTag.fragments().add(tE);

		tags.add(handlerTag);
	}

	//
	// Property
	//

	@SuppressWarnings("unchecked")
	public static String processPropertyDoc(ITranslationContext context,
			MethodDeclaration mgDecl, MethodDeclaration msDecl) {
		String tagToAdd = "";
		if (context.getConfiguration().getOptions().getTagToAddtoComment()
				.getValue() != null
				&& !context.getConfiguration().getOptions()
						.getTagToAddtoComment().getValue().equals("")) {
			tagToAdd = DOTNET_COMMENT
					+ START_XML_TAG
					+ context.getConfiguration().getOptions()
							.getTagToAddtoComment().getValue() + END_XML_TAG;
		}
		String existingHandlerTag = null;
		StringBuffer summary = new StringBuffer();
		StringBuffer buffer = new StringBuffer();
		List<String> tags = DocUtils.buildJavadocTagsToRemove(context);
		List elements = null;
		if (msDecl == null && mgDecl != null) {
			existingHandlerTag = TranslationUtils.getTagValueFromDoc(
					(BodyDeclaration) mgDecl, context.getMapper().getTag(
							TranslationModelUtil.HANDLER_TAG));
			Javadoc doc = mgDecl.getJavadoc();
			elements = doc.tags();
		} else if (msDecl != null && mgDecl == null) {
			existingHandlerTag = TranslationUtils.getTagValueFromDoc(
					(BodyDeclaration) msDecl, context.getMapper().getTag(
							TranslationModelUtil.HANDLER_TAG));
			Javadoc doc = msDecl.getJavadoc();
			elements = doc.tags();
		} else if (msDecl != null && mgDecl != null) {
			final String existingHandlerTagForSetter = TranslationUtils
					.getTagValueFromDoc((BodyDeclaration) msDecl, context
							.getMapper().getTag(
									TranslationModelUtil.HANDLER_TAG));
			final String existingHandlerTagForGetter = TranslationUtils
					.getTagValueFromDoc((BodyDeclaration) msDecl, context
							.getMapper().getTag(
									TranslationModelUtil.HANDLER_TAG));
			existingHandlerTag = existingHandlerTagForGetter;
			Javadoc docForSetter = msDecl.getJavadoc();
			Javadoc docForGetter = msDecl.getJavadoc();
			final List elementsForGetter = docForGetter.tags();
			final List elementsForSetter = docForSetter.tags();
			elements = DocUtils.mergePropertyDoc(elementsForGetter,
					elementsForSetter);
		} else {
			return null;
		}
		applyJavadocReplacement(context, tags, elements, summary, buffer,
				tagToAdd, existingHandlerTag, true);
		return buffer.toString();
	}

	@SuppressWarnings("unchecked")
	static List mergePropertyDoc(List elementsForGetter, List elementsForSetter) {
		// TODO: !!!
		List res = new ArrayList();
		for (Object tagO : elementsForGetter) {
			TagElement tag = (TagElement) tagO;
			if (tag.getTagName() != null) {
				if (!tag.getTagName().equals("@return")) {
					res.add(tag);
				}
			} else {
				res.add(tag);
			}
		}
		return res;
	}

	//
	// Indexer
	//

	@SuppressWarnings("unchecked")
	public static String processIndexerDoc(ITranslationContext context,
			MethodDeclaration mgDecl, MethodDeclaration msDecl) {
		String tagToAdd = "";
		if (context.getConfiguration().getOptions().getTagToAddtoComment()
				.getValue() != null
				&& !context.getConfiguration().getOptions()
						.getTagToAddtoComment().getValue().equals("")) {
			tagToAdd = DOTNET_COMMENT
					+ START_XML_TAG
					+ context.getConfiguration().getOptions()
							.getTagToAddtoComment().getValue() + END_XML_TAG;
		}
		final StringBuffer summary = new StringBuffer();
		final StringBuffer buffer = new StringBuffer();
		List<String> tags = DocUtils.buildJavadocTagsToRemove(context);
		List elements = null;
		String existingHandlerTag = null;
		if (msDecl == null && mgDecl != null) {
			existingHandlerTag = TranslationUtils.getTagValueFromDoc(
					(BodyDeclaration) mgDecl, context.getMapper().getTag(
							TranslationModelUtil.HANDLER_TAG));
			Javadoc doc = mgDecl.getJavadoc();
			elements = doc.tags();
			applyJavadocReplacement(context, tags, elements, summary, buffer,
					tagToAdd, existingHandlerTag, true);
			return buffer.toString();
		} else if (msDecl != null && mgDecl == null) {
			existingHandlerTag = TranslationUtils.getTagValueFromDoc(
					(BodyDeclaration) msDecl, context.getMapper().getTag(
							TranslationModelUtil.HANDLER_TAG));
			Javadoc doc = msDecl.getJavadoc();
			elements = doc.tags();
			applyJavadocReplacement(context, tags, elements, summary, buffer,
					tagToAdd, existingHandlerTag, true);
			return buffer.toString();
		} else if (msDecl != null && mgDecl != null) {
			final String existingHandlerTagForSetter = TranslationUtils
					.getTagValueFromDoc((BodyDeclaration) msDecl, context
							.getMapper().getTag(
									TranslationModelUtil.HANDLER_TAG));
			final String existingHandlerTagForGetter = TranslationUtils
					.getTagValueFromDoc((BodyDeclaration) msDecl, context
							.getMapper().getTag(
									TranslationModelUtil.HANDLER_TAG));
			existingHandlerTag = existingHandlerTagForGetter;
			Javadoc docForSetter = msDecl.getJavadoc();
			Javadoc docForGetter = msDecl.getJavadoc();
			final List elementsForGetter = docForGetter.tags();
			final List elementsForSetter = docForSetter.tags();
			elements = DocUtils.mergeIndexerDoc(elementsForGetter,
					elementsForSetter);
		} else {
			return null;
		}
		applyJavadocReplacement(context, tags, elements, summary, buffer,
				tagToAdd, existingHandlerTag, true);
		return buffer.toString();
	}

	@SuppressWarnings("unchecked")
	static List mergeIndexerDoc(List elementsForGetter, List elementsForSetter) {
		List res = new ArrayList();
		for (Object tagO : elementsForGetter) {
			TagElement tag = (TagElement) tagO;
			if (tag.getTagName() != null) {
				if (!tag.getTagName().equals("@return")) {
					res.add(tag);
				}
			} else {
				res.add(tag);
			}
		}
		return res;
	}

	static List<String> buildJavadocTagsToRemove(ITranslationContext context) {
		List<String> tags = new ArrayList<String>();
		tags.add(context.getMapper().getTag(TranslationModelUtil.HANDLER_TAG));
		tags
				.add(context.getMapper().getTag(
						TranslationModelUtil.SIGNATURE_TAG));
		tags.add(context.getMapper().getTag(TranslationModelUtil.HANDLER_TAG)
				+ "2");
		tags.add(context.getMapper().getTag(TranslationModelUtil.SIGNATURE_TAG)
				+ "2");
		tags.add(context.getMapper().getTag(TranslationModelUtil.OVERRIDE_TAG));
		tags.add(context.getMapper().getTag(TranslationModelUtil.VIRTUAL_TAG));
		tags.add(context.getMapper().getTag(TranslationModelUtil.CONST_TAG));
		tags.add(context.getMapper()
				.getTag(TranslationModelUtil.COVARIANCE_TAG));
		tags
				.add(context.getMapper().getTag(
						TranslationModelUtil.PUBLICAPI_TAG));
		tags.add(context.getMapper().getTag(TranslationModelUtil.TESTCASE_TAG));
		tags.add(context.getMapper()
				.getTag(TranslationModelUtil.TESTMETHOD_TAG));
		tags.add(context.getMapper().getTag(
				TranslationModelUtil.TESTCATEGORIE_TAG));
		tags
				.add(context.getMapper().getTag(
						TranslationModelUtil.TESTAFTER_TAG));
		tags.add(context.getMapper()
				.getTag(TranslationModelUtil.TESTBEFORE_TAG));
		tags.add(context.getMapper().getTag(TranslationModelUtil.BINDING_TAG));
		tags.add(context.getMapper().getTag(TranslationModelUtil.REMOVE_TAG));
		tags.add(TranslationModelUtil.MODGENERICMETHODCONSTRAINT_TAG);
		tags.add(TranslationModelUtil.MODGENERICMETHOD_TAG);
		tags.add(TranslationModelUtil.MODWILDCARD_TAG);
		tags.add(TranslationModelUtil.TRANSLATOR_MAPPING_TAG);
		return tags;
	}

	@SuppressWarnings("unchecked")
	public static void createSignatureTag(ITranslationContext context,
			BodyDeclaration node, IBinding binding, List<TagElement> tags) {
		final AST ast = node.getAST();

		if (binding != null) {
			String signatureKey = null;
			try {
				if (node.getNodeType() == ASTNode.METHOD_DECLARATION) {
					final MethodDeclaration md = (MethodDeclaration) node;
					final IMethod eleme = (IMethod) binding.getJavaElement();
					if (md.isConstructor()) {
						signatureKey = eleme.getSignature() + "<init>";
					} else {
						signatureKey = TranslationUtils.computeSignature(eleme) /*
																				 * eleme.
																				 * getSignature
																				 * (
																				 * )
																				 */
								+ binding.getName();
					}
				} else if (node.getNodeType() == ASTNode.ANNOTATION_TYPE_MEMBER_DECLARATION) {
					final IMethod eleme = (IMethod) binding.getJavaElement();
					signatureKey = TranslationUtils.computeSignature(eleme) /*
																			 * eleme.
																			 * getSignature
																			 * (
																			 * )
																			 */
							+ binding.getName();
				} else if (node.getNodeType() == ASTNode.TYPE_DECLARATION) {
					final ITypeBinding typeB = (ITypeBinding) binding;
					signatureKey = Signature.createTypeSignature(typeB
							.getName(), true);
				} else if (node.getNodeType() == ASTNode.ANNOTATION_TYPE_DECLARATION) {
					final ITypeBinding typeB = (ITypeBinding) binding;
					signatureKey = Signature.createTypeSignature(typeB
							.getName(), true);
				} else if (node.getNodeType() == ASTNode.FIELD_DECLARATION) {
					signatureKey = ""; // TODO
				}

				final TagElement bindingTag = ast.newTagElement();
				bindingTag.setTagName(context.getMapper().getTag(
						TranslationModelUtil.SIGNATURE_TAG) /* "@binding" */);
				final TextElement tE = ast.newTextElement();
				tE.setText(signatureKey);
				bindingTag.fragments().add(tE);

				tags.add(bindingTag);
			} catch (final JavaModelException e) {
				context.getLogger().logException(
						"Can't create signature for " + node, e);
			}
		} else {
			context.getLogger().logError(
					"FillBindingTag.createSignatureTag : Can't find binding for "
							+ node);
		}
	}

}
