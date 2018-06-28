package com.ilog.translator.java2cs.configuration.info;

import java.io.File;
import java.io.FileNotFoundException;
import java.io.FileOutputStream;
import java.io.FileReader;
import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.io.PrintWriter;
import java.io.Reader;
import java.io.StringReader;
import java.util.ArrayList;
import java.util.Collection;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

import javax.xml.parsers.DocumentBuilder;
import javax.xml.parsers.DocumentBuilderFactory;
import javax.xml.parsers.ParserConfigurationException;

import org.eclipse.core.resources.IProject;
import org.eclipse.jdt.core.IField;
import org.eclipse.jdt.core.IJavaProject;
import org.eclipse.jdt.core.IMethod;
import org.eclipse.jdt.core.IPackageFragment;
import org.eclipse.jdt.core.JavaModelException;
import org.eclipse.jdt.core.Signature;
import org.w3c.dom.Document;
import org.w3c.dom.Element;
import org.w3c.dom.Node;
import org.w3c.dom.NodeList;
import org.xml.sax.SAXException;

import com.ilog.translator.java2cs.configuration.options.MethodMappingPolicy;
import com.ilog.translator.java2cs.configuration.options.OptionImpl;
import com.ilog.translator.java2cs.configuration.options.PackageMappingPolicy;
import com.ilog.translator.java2cs.configuration.options.StringOptionBuilder;
import com.ilog.translator.java2cs.configuration.options.StringOptionEditor;
import com.ilog.translator.java2cs.configuration.options.OptionImpl.XMLKind;
import com.ilog.translator.java2cs.configuration.parser.MappingsFileParser;
import com.ilog.translator.java2cs.configuration.target.TargetMethod;
import com.ilog.translator.java2cs.plugin.TranslationPlugin;
import com.ilog.translator.java2cs.translation.util.InterfaceRenamingUtil;
import com.ilog.translator.java2cs.util.CSharpModelUtil;
import com.ilog.translator.java2cs.util.Utils;

/**
 * Encapsulate the content of a mappings file
 * 
 * @author afau
 * 
 */
public class MappingsInfo implements Constants {

	protected TranslateInfo translateInfo;
	private final String filename;
	private boolean readonly;

	//
	private final Map<String, List<ClassMappingFromComments>> classMappings = new HashMap<String, List<ClassMappingFromComments>>();
	private final Map<String, List<FieldMappingFromComments>> fieldMappings = new HashMap<String, List<FieldMappingFromComments>>();
	private final Map<String, List<MethodMappingFromComments>> methodMappings = new HashMap<String, List<MethodMappingFromComments>>();
	private final Map<String, ConstantsRename> constantsRename = new HashMap<String, ConstantsRename>();
	private final Map<String, NestedRename> nestedClassRename = new HashMap<String, NestedRename>();
	private final Map<String, String> mappingNestedClassRename = new HashMap<String, String>();
	private final Map<String, MethodOverride> methodsOverride = new HashMap<String, MethodOverride>();
	private final Map<String, FieldRename> fieldsRename = new HashMap<String, FieldRename>();
	private final Map<String, PropertyRename> propertiesRename = new HashMap<String, PropertyRename>();
	private final Map<String, NestedToInnerRefactoring> nestedToInnerList = new HashMap<String, NestedToInnerRefactoring>();
	private final HashMap<String, String> javaDocMappings = new HashMap<String, String>();
	private final Map<String, VariableInfo> variablesList = new HashMap<String, VariableInfo>();
	private final Map<String, List<PackageInfo>> packagesList = new HashMap<String, List<PackageInfo>>();
	private final Map<String, String> keywordsList = new HashMap<String, String>();
	private final Map<String, PackageRename> packageRename = new HashMap<String, PackageRename>();

	//
	// Options
	//
	private OptionImpl<String> disclaimer = new OptionImpl<String>(
			"disclaimer", null, null, OptionImpl.Status.PRODUCTION,
			new StringOptionBuilder(), new StringOptionEditor(), XMLKind.CDATA,
			"");

	//
	// Constructor
	//

	public MappingsInfo(String filename, TranslateInfo translateInfo) {
		this.filename = filename;
		this.translateInfo = translateInfo;
	}

	//
	// variables
	//

	protected void addVariable(VariableInfo vinfo) {
		variablesList.put(vinfo.getName(), vinfo);
	}

	public VariableInfo getVariable(String name) {
		return variablesList.get(name);
	}
	
	public VariableInfo createVariable(String name, List<String> names,
			IProject reference) {
		final VariableInfo vInfo = new VariableInfo(this, name, names);
		addVariable(vInfo);
		return vInfo;
	}
	
	//
	// TranslateInfo
	//

	public TranslateInfo getTranslateInfo() {
		return translateInfo;
	}

	//
	// keyword
	//

	public void addKeyword(String key, String equivalent, IProject reference) {
		keywordsList.put(key, equivalent);
	}

	public String getKeyword(String key) {
		return keywordsList.get(key);
	}

	//
	// package
	//

	protected void addPackage(PackageInfo pck, IProject reference) {
		List<PackageInfo> res = packagesList.get(pck.getName());
		if (res != null) {
			res.add(pck);
		} else {
			res = new ArrayList<PackageInfo>();
			res.add(pck);
			packagesList.put(pck.getName(), res);
		}
	}

	public PackageInfo getPackage(String name, IProject reference, boolean exact) {
		// Cache already computed composite package ?
		final List<PackageInfo> res = packagesList.get(name);
		if (res == null)
			return null;
		if (res.size() == 0)
			return null;
		if (res.size() == 1)
			return res.get(0);

		if (exact) {
			for (final PackageInfo pInfo : res) {
				if (pInfo.getReference() == reference)
					return pInfo;
			}
			return res.get(0);
		}
		return new CompositePackageInfo(res);
	}

	public Collection<List<PackageInfo>> getPackages() {
		return packagesList.values();
	}

	public PackageInfo createPackage(String name, IProject reference) {
		final PackageInfo pInfo = new PackageInfoImpl(this, name, reference);
		addPackage(pInfo, reference);
		return pInfo;
	}

	//
	// read
	//

	public boolean read(String name, IJavaProject reference, boolean system)
			throws IOException, FileNotFoundException, JavaModelException {
		final MappingsFileParser parser = new MappingsFileParser(this,
				reference.getProject(), system);
		final Reader r = new FileReader(name);
		if (translateInfo.getConfiguration().getOptions().getGlobalOptions().isDebug()) {
			translateInfo.getConfiguration().getLogger().logInfo(
					"     Loading mapping file " + name);
		}
		parser.parse(name, r);
		return true;
	}

	public boolean readFromStream(String name, InputStream stream,
			boolean system) throws IOException, FileNotFoundException,
			JavaModelException {
		readonly = true;
		final MappingsFileParser parser = new MappingsFileParser(this, null,
				system);
		final Reader r = new InputStreamReader(stream);
		if (translateInfo.getConfiguration().getOptions().getGlobalOptions().isDebug()) {
			translateInfo.getConfiguration().getLogger().logInfo(
					"     Loading mapping file " + name);
		}
		parser.parse(name, r);
		return true;
	}

	//
	// save
	//
	
	public boolean save() throws IOException, FileNotFoundException {
		if (!readonly && filename != null) {
			final PrintWriter pw = new PrintWriter(filename);
			if (variablesList.size() > 0) {
				for (final VariableInfo vi : variablesList.values()) {
					pw.append(vi.toFile() + "\n");
				}
			}
			if (packagesList.size() > 0) {
				for (final List<PackageInfo> pi : packagesList.values()) {
					pw.append(pi.get(0).toFile() + "\n"); // TODO:
				}
			}
			if (keywordsList.size() > 0) {
				for (final String keyword : keywordsList.keySet()) {
					pw.append("keyword " + keyword + "="
							+ keywordsList.get(keyword) + "\n");
				}
			}
			pw.close();
			return true;
		}
		return false;
	}

	public synchronized boolean saveXML(String name) throws IOException,
			FileNotFoundException {
		final PrintWriter pw = new PrintWriter(name);
		pw.append("<?xml version=\"1.0\" encoding=\"utf-8\"?>\n");
		pw.append("<!--              -->\n");
		pw.append("<!-- " + name + " -->\n");
		pw.append("<!--              -->\n");
		pw.append("<mapping>\n");
		if (variablesList.size() > 0) {
			pw.append("<!--                              -->\n");
			pw.append("<!-- automatic variables renaming -->\n");
			pw.append("<!--                              -->\n");
			pw.append("<variables>\n");
			for (final VariableInfo vi : variablesList.values()) {
				StringBuilder builder = new StringBuilder();
				vi.toXML(builder, Constants.TAB);
				pw.append(builder + "\n"); // TODO:
			}
			pw.append("</variables>\n");
		}
		if (packagesList.size() > 0) {
			pw.append("<!--          -->\n");
			pw.append("<!-- packages -->\n");
			pw.append("<!--          -->\n");
			pw.append("<packages>\n");
			for (final List<PackageInfo> pi : packagesList.values()) {
				StringBuilder builder = new StringBuilder();
				pi.get(0).toXML(builder, Constants.TAB);
				pw.append(builder + "\n"); // TODO:
			}
			pw.append("</packages>\n");
		}
		if (keywordsList.size() > 0) {
			pw.append("<!--          -->\n");
			pw.append("<!-- keywords -->\n");
			pw.append("<!--          -->\n");
			pw.append("<keywords>\n");
			for (final String keyword : keywordsList.keySet()) {
				pw.append(Constants.TAB + "<!--                 -->\n");
				pw.append(Constants.TAB + "<!-- " + keyword + " -->\n");
				pw.append(Constants.TAB + "<!--                 -->\n");
				pw.append(Constants.TAB + "<keyword name=\"" + keyword
						+ "\" alias=\"" + keywordsList.get(keyword) + "\"/>\n");
			}
			pw.append("</keywords>\n");
		}
		if (this.getDisclaimer() != null) {
			pw.append("<!--            -->\n");
			pw.append("<!-- disclaimer -->\n");
			pw.append("<!--            -->\n");
			StringBuilder res = new StringBuilder();
			disclaimer.toXML(res, "");
			pw.append(res);
		}
		if (this.getJavaDocMappings().size() > 0) {
			pw.append("<!--                 -->\n");
			pw.append("<!-- javadoc mapping -->\n");
			pw.append("<!--                 -->\n");
			pw.append("<javadoc>\n");
			for (final String tag : javaDocMappings.keySet()) {
				pw
						.append(Constants.TAB + "<tag name=\"" + tag
								+ "\" newName=\"" + javaDocMappings.get(tag)
								+ "\"/>\n");
			}
			pw.append("</javadoc>\n");
		}
		pw.append("</mapping>\n");
		pw.flush();
		pw.close();
		return true;
	}

	//
	// readXML
	//
	
	public boolean readXmlFromStream(String name, InputStream stream,
			IJavaProject reference, boolean system) throws IOException,
			FileNotFoundException, JavaModelException,
			ParserConfigurationException, SAXException {
		final DocumentBuilderFactory docBuilderFactory = DocumentBuilderFactory
				.newInstance();
		final DocumentBuilder docBuilder = docBuilderFactory
				.newDocumentBuilder();
		final Document doc = docBuilder.parse(stream);
		return readXml(doc, reference, system);
	}

	public boolean readXml(String name, IJavaProject reference, boolean system)
			throws IOException, FileNotFoundException, JavaModelException,
			ParserConfigurationException, SAXException {
		final DocumentBuilderFactory docBuilderFactory = DocumentBuilderFactory
				.newInstance();
		final DocumentBuilder docBuilder = docBuilderFactory
				.newDocumentBuilder();
		final Document doc = docBuilder.parse(new File(name));

		return readXml(doc, reference, system);
	}

	public boolean readXml(Document doc, IJavaProject reference, boolean system)
			throws IOException, FileNotFoundException, JavaModelException,
			ParserConfigurationException, SAXException {

		final Element root = doc.getDocumentElement();
		String elemName = root.getNodeName();

		NodeList children = root.getChildNodes();

		for (int i = 0; i < children.getLength(); i++) {
			Node child = children.item(i);
			elemName = child.getNodeName();

			if (elemName.equals("variables")) {
				NodeList children2 = child.getChildNodes();
				for (int k = 0; k < children2.getLength(); k++) {
					Node child2 = children2.item(k);
					final List<String> names = new ArrayList<String>();
					if (child2.getNodeName().equals("variable")) {
						Element variable = (Element) child2;
						String aName = variable.getAttribute("name");
						NodeList aliases = variable.getChildNodes();
						for (int j = 0; j < aliases.getLength(); j++) {
							Node alias = aliases.item(j);
							if (alias.getNodeName().equals("alias")) {
								Element aliasElement = (Element) alias;
								String aValue = aliasElement
										.getAttribute("name");
								names.add(aValue);
							}
						}
						this.createVariable(aName, names, reference
								.getProject());
					}
				}
			} else if (elemName.equals("keywords")) {
				NodeList children2 = child.getChildNodes();
				for (int j = 0; j < children2.getLength(); j++) {
					Node child2 = children2.item(j);
					if (child2.getNodeName().equals("keyword")) {
						Element keyword = (Element) child2;
						String kName = keyword.getAttribute("name");
						String kAlias = keyword.getAttribute("alias");
						this.addKeyword(kName, kAlias, reference.getProject());
					}
				}
			} else if (elemName.equals("packages")) {
				NodeList children2 = child.getChildNodes();
				for (int j = 0; j < children2.getLength(); j++) {
					Node child2 = children2.item(j);
					if (child2.getNodeName().equals("package")) {
						Element pack = (Element) child2;
						String pName = pack.getAttribute("name");
						// exists ?
						IPackageFragment pf = getTranslateInfo()
								.packageforName(pName, reference);
						/*
						 * if (pf == null) {
						 * tInfo.getConfiguration().getLogger()
						 * .logError("Package <" + pName +
						 * "> does not exists."); }
						 */
						PackageInfo pInfo = getPackage(pName, reference
								.getProject(), true);
						if (pInfo == null)
							pInfo = createPackage(pName, reference.getProject());
						else {
							if (pInfo.getReference() != reference) {
								pInfo = createPackage(pName, reference
										.getProject());
							}
						}
						pInfo.fromXML(pack);
					}
				}
			} else if (elemName.equals("javadoc")) {
				NodeList children2 = child.getChildNodes();
				for (int j = 0; j < children2.getLength(); j++) {
					Node child2 = children2.item(j);
					if (child2.getNodeName().equals("tag")) {
						Element keyword = (Element) child2;
						String tname = keyword.getAttribute("name");
						String tnewName = keyword.getAttribute("newName");
						addJavaDocMapping(tname, tnewName, reference
								.getProject());
					}
				}
			} else if (elemName.equals("disclaimer")) {
				disclaimer.fromXML(root);				
			}
		}
		return true;
	}

	//
	// extractMappingFromComments
	//

	public void extractMappingFromComments(ClassInfo ci, String comments)
			throws JavaModelException {
		final MappingsFileParser parser = new MappingsFileParser(this, ci
				.getType().getJavaProject().getProject(), false);
		PackageInfo packageInfo = translateInfo.getPackage(ci.getPackageName(), ci
				.getType().getJavaProject().getProject());
		if (packageInfo == null) {
			packageInfo = createPackage(ci.getPackageName(), ci.getType()
					.getJavaProject().getProject());
		}
		parser.parseClass("", new StringReader(comments.trim()), packageInfo,
				ci);
		//
		addMappingFromComments(ci, comments.trim());
	}

	public void extractFieldMappingFromComments(ClassInfo ci, IField field,
			String comments) throws JavaModelException {
		final MappingsFileParser parser = new MappingsFileParser(this, ci
				.getType().getJavaProject().getProject(), false);
		FieldInfo fi = ci.getField(field);
		if (fi == null) {
			fi = ci.resolveField(field.getElementName());
		}
		parser.parseField("", new StringReader(comments.trim()), ci, fi);
		addMappingFromComments(fi, comments.trim());
	}

	public void extractMethodMappingFromComments(ClassInfo ci, IMethod method,
			String comments) throws JavaModelException {
		final MappingsFileParser parser = new MappingsFileParser(this, ci
				.getType().getJavaProject().getProject(), false);
		MethodInfo mi = ci.getMethod(null, method);
		if (mi == null) {
			mi = ci.resolveMethod(method.getElementName(), method
					.getParameterTypes(), true);
		}
		parser.parseMethod("", new StringReader(comments.trim()), ci, mi);
		addMappingFromComments(mi, comments.trim());
	}

	private static String extractOption(String trim) {
		final int startIndex = trim.indexOf("{");
		final int endIndex = trim.lastIndexOf("}");
		if (startIndex >= 0) {
			return trim.substring(startIndex + 1, endIndex);
		}
		return null;
	}

	private void addMappingFromComments(ClassInfo ci, String trim) {
		String cName = ci.getName().replace("$", ".");
		final String pName = ci.getPackageName();
		if (cName.startsWith(pName)) {
			cName = cName.substring(pName.length() + 1);
		}
		String targetCName = null;
		String targetPName = null;
		String targetFramework = getTranslateInfo().getConfiguration().getOptions().getTargetDotNetFramework().name();
		
		if (ci.getTarget(targetFramework) != null) {
			targetCName = ci.getTarget(targetFramework).getName();
			targetPName = ci.getTarget(targetFramework).getPackageName();
		}
		// try to "parse" the content
		TranslateInfo newTInfo = new TranslateInfo(translateInfo.getConfiguration());
		MappingsInfo newMinfo = new MappingsInfo("from class comments "
				+ ci.getName(), newTInfo);
		MappingsFileParser parser = new MappingsFileParser(newMinfo, null,
				false);
		StringBuilder xmlAttributeMappings = new StringBuilder();
		StringBuilder xmlElementsMappings = new StringBuilder();
		try {
			StringReader sr = new StringReader(trim);
			parser.parseClass("from class comments " + ci.getName(), sr, ci
					.getPackageInfo(), ci);
			if (ci.getTarget(targetFramework) != null) {
				ci.getTarget(targetFramework).xmlAttributeInTargetPart(xmlAttributeMappings);
				ci.getTarget(targetFramework).xmlElementsInTargetPart(xmlElementsMappings);
			}
		} catch (JavaModelException e) {

		}
		//
		final String extractedOptions = extractOption(trim); // comma
		// separated
		// options
		final ClassMappingFromComments cim = new ClassMappingFromComments(
				pName, cName, targetPName, targetCName, extractedOptions, "",
				xmlAttributeMappings.toString(), xmlElementsMappings.toString());
		List<ClassMappingFromComments> list = classMappings.get(pName
				+ CSharpModelUtil.CLASS_SEPARATOR + cName);
		if (list == null) {
			list = new ArrayList<ClassMappingFromComments>();
			classMappings.put(pName + CSharpModelUtil.CLASS_SEPARATOR + cName,
					list);
		}
		list.add(cim);
	}

	private void addMappingFromComments(FieldInfo fi, String trim) {
		final String fName = fi.getName();
		//
		final ClassInfo ci = fi.getClassInfo();
		String cName = ci.getName().replace("$", ".");
		final String pName = ci.getPackageName();
		if (cName.startsWith(pName)) {
			cName = cName.substring(pName.length() + 1);
		}
		String targetCName = null;
		String targetPName = null;
		String targetFramework = getTranslateInfo().getConfiguration().getOptions().getTargetDotNetFramework().name();
		
		if (ci.getTarget(targetFramework) != null) {
			targetCName = ci.getTarget(targetFramework).getName();
			targetPName = ci.getTarget(targetFramework).getPackageName();
		}
		//
		// try to "parse" the content
		TranslateInfo newTInfo = new TranslateInfo(translateInfo.getConfiguration());
		MappingsInfo newMinfo = new MappingsInfo("from class comments "
				+ ci.getName(), newTInfo);
		MappingsFileParser parser = new MappingsFileParser(newMinfo, null,
				false);
		StringBuilder xmlTextMapping = new StringBuilder();		
		try {
			StringReader sr = new StringReader(trim);
			parser
					.parseField("from class comments " + ci.getName(), sr, ci,
							fi);
			if (fi.getTarget(targetFramework) != null)
				fi.getTarget(targetFramework).toXML(xmlTextMapping, "");		
		} catch (JavaModelException e) {

		}
		final String extractedOptions = extractOption(trim); // comma
		// separated
		// options
		final FieldMappingFromComments cim = new FieldMappingFromComments(
				pName, cName, targetPName, targetCName, fName,
				extractedOptions, xmlTextMapping.toString(), "", "");				
		List<FieldMappingFromComments> list = fieldMappings.get(pName
				+ CSharpModelUtil.CLASS_SEPARATOR + cName);
		if (list == null) {
			list = new ArrayList<FieldMappingFromComments>();
			fieldMappings.put(pName + CSharpModelUtil.CLASS_SEPARATOR + cName,
					list);
		}
		list.add(cim);
	}

	private void addMappingFromComments(MethodInfo mi, String trim) {
		final String mName = mi.getName();
		final String[] ptypes = mi.getParameterTypes();
		final String[] resolvedTypes = new String[ptypes.length];
		for (int i = 0; i < ptypes.length; i++) {
			final String t = Signature.toString(ptypes[i]);
			final String type = mi.resolveType(t);
			resolvedTypes[i] = type;
		}
		//
		final ClassInfo ci = mi.getClassInfo();
		String cName = ci.getName().replace("$", ".");
		final String pName = ci.getPackageName();
		if (cName.startsWith(pName)) {
			cName = cName.substring(pName.length() + 1);
		}
		String targetCName = null;
		String targetPName = null;
		String targetFramework = getTranslateInfo().getConfiguration().getOptions().getTargetDotNetFramework().name();
		
		if (ci.getTarget(targetFramework) != null) {
			targetCName = ci.getTarget(targetFramework).getName();
			targetPName = ci.getTarget(targetFramework).getPackageName();
		}
		//
		// try to "parse" the content
		TranslateInfo newTInfo = new TranslateInfo(translateInfo.getConfiguration());
		MappingsInfo newMinfo = new MappingsInfo("from class comments "
				+ ci.getName(), newTInfo);
		MappingsFileParser parser = new MappingsFileParser(newMinfo, null,
				false);
		StringBuilder xmlMappings = new StringBuilder();	
		try {
			StringReader sr = new StringReader(trim);
			parser.parseMethod("from class comments " + ci.getName(), sr, ci,
					mi);
			if (mi.getTarget(targetFramework) != null)
				mi.getTarget(targetFramework).toXML(xmlMappings, "");
		} catch (JavaModelException e) {

		}
		//
		final String extractedOptions = extractOption(trim); // comma
		// separated
		// options
		final MethodMappingFromComments cim = new MethodMappingFromComments(
				pName, cName, targetPName, targetCName, mName, ptypes,
				resolvedTypes, extractedOptions, xmlMappings
						.toString(), "", "");
		final String key = pName + CSharpModelUtil.CLASS_SEPARATOR + cName;
		List<MethodMappingFromComments> list = methodMappings.get(key);
		if (list == null) {
			list = new ArrayList<MethodMappingFromComments>();
			methodMappings.put(pName + CSharpModelUtil.CLASS_SEPARATOR + cName,
					list);
		}
		list.add(cim);
	}

	//
	// Implicit modifications
	//

	/**
	 * An implicit modification is a mapping automatically
	 * added by the translator during firsts passes.
	 */
	static interface ImplictModification {
		public void print(StringBuilder stream);

		public void toXML(StringBuilder stream);

		public void xmlAttributesForTargetPart(StringBuilder stream);

		public void xmlElementsForTargetPart(StringBuilder stream);
	}

	static class NestedRename implements ImplictModification {
		public String sourcePackageName = null;
		public String sourceClassName = null;
		public String targetPackageName = null;
		public String targetClassName = null;

		public void print(StringBuilder stream) {
			// none
		}

		public void toXML(StringBuilder stream) {
			// none
		}

		public void xmlAttributesForTargetPart(StringBuilder stream) {
			// none
		}

		public void xmlElementsForTargetPart(StringBuilder stream) {
			// none
		}
	}

	static class PropertyRename implements ImplictModification {
		public String sourcePackageName = null;
		public String sourceClassName = null;
		public String getterName;
		public String setterName;
		public String propertyName;

		public void print(StringBuilder stream) {
			if (getterName != null)
				stream.append("      method " + getterName
						+ " { property_get = " + propertyName + "; }\n");
			if (setterName != null)
				stream.append("      method " + setterName
						+ " { property_set = " + propertyName + "; }\n");
		}

		public void toXML(StringBuilder stream) {
			if (getterName != null) {
				stream.append("<method name=\"" + getterName
						+ "\" signature=\""
						+ Utils.getSignature(getterName) + "\" >\n");
				stream.append("   <target propertyGet=\"" + propertyName
						+ "\"/>\n");
				stream.append("</method>\n");
			}
			if (setterName != null) {
				stream.append("<method name=\"" + setterName
						+ "\" signature=\""
						+ Utils.getSignature(setterName) + "\" >\n");
				stream.append("   <target propertySet=\"" + propertyName
						+ "\"/>\n");
				stream.append("</method>\n");
			}
		}

		public void xmlAttributesForTargetPart(StringBuilder stream) {
			// none
		}

		public void xmlElementsForTargetPart(StringBuilder stream) {
			// none
		}
	}

	static class NestedToInnerRefactoring implements ImplictModification {
		public String sourcePackageName = null;
		public String sourceClassName = null;

		public void print(StringBuilder stream) {
			stream.append("      nestedToInner = true;\n");
		}

		public void toXML(StringBuilder stream) {
			//
		}

		public void xmlAttributesForTargetPart(StringBuilder stream) {
			stream.append(" nestedToInner=\"true\"");
		}

		public void xmlElementsForTargetPart(StringBuilder stream) {
			// none
		}
	}

	class ConstantsRename implements ImplictModification {
		public String packageName = null;
		public String sourceClassName = null;
		public String targetClassName = null;
		public String[] elements;

		public void print(StringBuilder stream) {
			final String targetPck = buildTargetPackage(packageName);
			for (final String elem : elements) {
				stream.append("      field " + elem + " { pattern = "
						+ targetPck + "." + targetClassName + "." + elem
						+ "; }\n");
			}
		}

		public void toXML(StringBuilder stream) {
			final String targetPck = buildTargetPackage(packageName);
			for (final String elem : elements) {
				stream.append("           <field name=\"" + elem + "\">\n");
				stream.append("              <target>\n");
				stream.append("                 <format>\n");
				stream.append("<![CDATA[" + targetPck + "." + targetClassName
						+ "." + elem + "]]>\n");
				stream.append("                 </format>\n");
				stream.append("              </target>\n");
				stream.append("           </field>\n");
			}
		}

		public void xmlAttributesForTargetPart(StringBuilder stream) {
			// none
		}

		public void xmlElementsForTargetPart(StringBuilder stream) {
			// none
		}
	}

	static class FieldRename implements ImplictModification {
		public String packageName = null;
		public String className = null;
		public String oldName = null;
		public String newName = null;

		public void print(StringBuilder stream) {
			if (!className.contains("Anonymous_C")) {
				stream.append("      field " + oldName + " { name = " + newName
						+ " ; }\n");
			}
		}

		public void toXML(StringBuilder stream) {
			if (!className.contains("Anonymous_C")) {
				stream.append("            <field name=\"" + oldName + "\">\n");
				stream.append("               <target name=\"" + newName
						+ "\"/>\n");
				stream.append("            </field>\n");
			}
		}

		public void xmlAttributesForTargetPart(StringBuilder stream) {
			// none
		}

		public void xmlElementsForTargetPart(StringBuilder stream) {
			// none
		}
	}

	static class MethodOverride implements ImplictModification {
		public String sourceClassName = null;
		public String sourcePackageName = null;
		public String targetClassName = null;
		public String targetPackageName = null;
		public String methodName = null;
		public String[] methodParams = null;
		public TargetMethod targetMethod = null;

		public void print(StringBuilder stream) {
			// none
		}

		public void toXML(StringBuilder stream) {
			// none
		}

		public void xmlAttributesForTargetPart(StringBuilder stream) {
			// none
		}

		public void xmlElementsForTargetPart(StringBuilder stream) {
			// none
		}
	}

	static class ClassMappingFromComments implements ImplictModification {
		public String sourceClassName = null;
		public String sourcePackageName = null;
		public String targetClassName = null;
		public String targetPackageName = null;
		public String textMappings = null;
		public String xmlTextMappings = null;
		public String xmlElementMappings = null;
		public String xmlAttributeMappings = null;

		public ClassMappingFromComments(String pName, String cName,
				String targetPName, String targetCName, String textMappings,
				String xmlTextMappings,
				String xmlAttributeMappings, String xmlElementMappings) {
			sourceClassName = cName;
			sourcePackageName = pName;
			targetClassName = targetCName;
			targetPackageName = targetPName;
			this.textMappings = textMappings;
			this.xmlTextMappings = xmlTextMappings;
			this.xmlElementMappings = xmlElementMappings;
			this.xmlAttributeMappings = xmlAttributeMappings;
		}

		public void print(StringBuilder stream) {
			stream.append("      " + textMappings + "\n");
		}

		public void toXML(StringBuilder stream) {
			// how to skip <target> and </target>
		}

		public void xmlAttributesForTargetPart(StringBuilder stream) {
			stream.append(xmlAttributeMappings);
		}

		public void xmlElementsForTargetPart(StringBuilder stream) {
			stream.append(xmlElementMappings);
		}
	}

	static class FieldMappingFromComments implements ImplictModification {
		public String sourceClassName = null;
		public String sourcePackageName = null;
		public String targetClassName = null;
		public String targetPackageName = null;
		public String mappings = null;
		public String fieldName;
		public String textMappings = null;
		public String xmlTextMappings = null;
		public String xmlElementMappings = null;
		public String xmlAttributeMappings = null;

		public FieldMappingFromComments(String pName, String cName,
				String targetPName, String targetCName, String fieldName,
				String textMappings, String xmlTextMappings, String xmlAttributeMappings,
				String xmlElementMappings) {
			sourceClassName = cName;
			sourcePackageName = pName;
			targetClassName = targetCName;
			targetPackageName = targetPName;
			this.fieldName = fieldName;
			this.textMappings = textMappings;
			this.xmlTextMappings = xmlTextMappings;
			this.xmlElementMappings = xmlElementMappings;
			this.xmlAttributeMappings = xmlAttributeMappings;
		}

		public void print(StringBuilder stream) {
			stream.append("      field " + fieldName + " { " + mappings
					+ " } \n");
		}

		public void toXML(StringBuilder stream) {
			stream.append("            <!-- " + fieldName + " -->\n");
			stream.append("            <field name=\"" + fieldName + "\"");
			stream.append(xmlTextMappings);
			stream.append("            </field>\n\n");
			
		}

		public void xmlAttributesForTargetPart(StringBuilder stream) {
			// none
		}

		public void xmlElementsForTargetPart(StringBuilder stream) {
			// none
		}
	}

	static class MethodMappingFromComments implements ImplictModification {
		public String sourceClassName = null;
		public String sourcePackageName = null;
		public String targetClassName = null;
		public String targetPackageName = null;
		public String textMappings = null;
		public String xmlTextMappings = null;
		public String xmlElementMappings = null;
		public String xmlAttributeMappings = null;
		public String methodName = null;
		public String[] typesName = null;
		public String[] resolvedTypes = null;

		public MethodMappingFromComments(String pName, String cName,
				String targetPName, String targetCName, String methodName,
				String[] typesName, String[] resolvedTypes,
				String textMappings, String xmlTextMappings, String xmlAttributeMappings,
				String xmlElementMappings) {
			sourceClassName = cName;
			sourcePackageName = pName;
			targetClassName = targetCName;
			targetPackageName = targetPName;
			this.typesName = typesName;
			this.resolvedTypes = resolvedTypes;
			this.methodName = methodName;
			this.textMappings = textMappings;
			this.xmlTextMappings = xmlTextMappings;
			this.xmlElementMappings = xmlElementMappings;
			this.xmlAttributeMappings = xmlAttributeMappings;
		}

		public void print(StringBuilder stream) {
			stream.append("      method " + methodName + "("
					+ printTypeParam(resolvedTypes) + ") { " + textMappings
					+ "}\n");
		}

		public void toXML(StringBuilder stream) {
			stream.append("            <!-- " + methodName + " -->\n");
			stream.append("            <method name=\"" + methodName + "\" signature=\"("
					+ printTypeParam(resolvedTypes) + ")\" >\n");
			stream.append(xmlTextMappings);
			stream.append("            </method>\n\n");
		}

		public void xmlAttributesForTargetPart(StringBuilder stream) {
			// none
		}

		public void xmlElementsForTargetPart(StringBuilder stream) {
			// none
		}
	}

	static class PackageRename implements ImplictModification {
		public String oldName = null;
		public String newName = null;

		public void print(StringBuilder stream) {
			stream.append("package " + oldName + " :: " + newName + " {"
					+ "\n}\n");
		}

		public void toXML(StringBuilder stream) {
			stream.append("      <package name=\"" + oldName + "\">\n");
			stream.append("         <target name=\"" + newName + "\"/>\n");
			stream.append("      </package>\n");
		}

		public void xmlAttributesForTargetPart(StringBuilder stream) {
			// none
		}

		public void xmlElementsForTargetPart(StringBuilder stream) {
			// none
		}
	}

	//
	//
	//

	private String buildTargetPackage(String pname) {
		String targetFramework = getTranslateInfo().getConfiguration().getOptions().getTargetDotNetFramework().name();
		
		final PackageInfo packageInfo = translateInfo.getPackage(pname, null);
		if (packageInfo != null && packageInfo.getTarget(targetFramework) != null) {
			return packageInfo.getTarget(targetFramework).getName();
		} else {
			if (translateInfo.getConfiguration().getOptions().getPackageMappingPolicy() == PackageMappingPolicy.NONE)
				return pname;
			if (packageInfo != null
					&& packageInfo.getTarget(targetFramework) != null
					&& (packageInfo.getTarget(targetFramework).getPackageMappingBehavior() == null || packageInfo
							.getTarget(targetFramework).getPackageMappingBehavior().equals(
									PackageMappingPolicy.CAPITALIZED)))
				return Utils.capitalize(pname);
			else
				return pname;
		}
	}

	//

	public void addImplicitConstantsRename(String packageName, String name,
			String newClassName, String[] elements) {
		final ConstantsRename cRename = new ConstantsRename();
		cRename.packageName = packageName;
		cRename.sourceClassName = name;
		cRename.targetClassName = newClassName;
		cRename.elements = elements;

		constantsRename.put(packageName + CSharpModelUtil.CLASS_SEPARATOR
				+ newClassName, cRename);
	}

	public void addImplicitNestedRename(String newPackageName, String newName,
			String oldPackageName, String oldName) {
		final NestedRename nested = new NestedRename();
		String targetFramework = getTranslateInfo().getConfiguration().getOptions().getTargetDotNetFramework().name();
		
		nested.sourceClassName = oldName;
		nested.sourcePackageName = oldPackageName;
		nested.targetClassName = newName;
		nested.targetPackageName = (newPackageName == null) ? buildTargetPackage(oldPackageName)
				: newPackageName;
		nestedClassRename.put(oldPackageName + CSharpModelUtil.CLASS_SEPARATOR
				+ newName, nested);
		mappingNestedClassRename.put(oldPackageName
				+ CSharpModelUtil.CLASS_SEPARATOR + oldName, newName);
	}

	public void addImplicitMethodOverride(String oldPackageName,
			String oldClassName, MethodInfo method) {
		final MethodOverride mo = new MethodOverride();
		String targetFramework = getTranslateInfo().getConfiguration().getOptions().getTargetDotNetFramework().name();
		
		mo.sourceClassName = oldClassName;
		mo.sourcePackageName = oldPackageName;
		mo.targetClassName = oldClassName;
		mo.targetPackageName = buildTargetPackage(oldPackageName);
		mo.methodName = method.getName();
		mo.methodParams = method.getParameterTypes();
		
		mo.targetMethod = method.getTarget(targetFramework);
		methodsOverride.put(oldPackageName + CSharpModelUtil.CLASS_SEPARATOR
				+ oldClassName, mo);
	}

	public boolean isAnImplicitNestedRename(String packageName, String newName) {
		return nestedClassRename.get(packageName
				+ CSharpModelUtil.CLASS_SEPARATOR + newName) != null;
	}

	public MappingsInfo.NestedRename getImplicitNestedRename(String className) {
		return nestedClassRename.get(className);
	}

	static class TClass {
		public String sourceName;
		public String sourcePck;
		public String targetName;
		public String targetPck;
		public List<ImplictModification> list = new ArrayList<ImplictModification>();	

		public TClass(String spck, String sname, String tpck, String tname) {
			sourcePck = spck;
			sourceName = sname;
			targetName = tname;
			targetPck = tpck;
		}
	}

	private HashMap<String, TClass> getPackage(
			HashMap<String, HashMap<String, TClass>> classes, String sourcePck) {
		HashMap<String, TClass> currentPck = classes.get(sourcePck);
		if (!sourcePck.equals("")) {
			if (currentPck == null) {
				currentPck = new HashMap<String, TClass>();
				classes.put(sourcePck, currentPck);
			}
			return currentPck;
		}
		return null;
	}

	private TClass getTClass(HashMap<String, TClass> currentPck,
			String sourcePck, String className, String targetPck,
			String targetClass) {
		TClass currentClassList = currentPck.get(className);
		if (currentClassList == null) {
			currentClassList = new TClass(sourcePck, className, targetPck,
					targetClass);
			currentPck.put(className, currentClassList);
		}
		return currentClassList;
	}

	public StringBuilder prepareBuffer(boolean isXML) throws Exception {

		final HashMap<String, HashMap<String, TClass>> classes = new HashMap<String, HashMap<String, TClass>>();

		final boolean nonePackageMapping = translateInfo.getConfiguration()
				.getOptions().getPackageMappingPolicy() == PackageMappingPolicy.NONE;
		final boolean noneMemberMapping = translateInfo.getConfiguration().getOptions()
				.getMethodMappingPolicy() == MethodMappingPolicy.NONE;

		if (nonePackageMapping || noneMemberMapping) {
			// Easy way to inject mapping options in generated mapping files
			final IPackageFragment[] packagesFrags = translateInfo.getConfiguration()
					.getOriginalProject().getPackageFragments();
			for (final IPackageFragment pck : packagesFrags) {
				if (!pck.getClass().getName().contains("JarPackageFragment")) {
					getPackage(classes, pck.getElementName());
				}
			}
		}

		for (final NestedRename nested : nestedClassRename.values()) {
			final String sourcePck = nested.sourcePackageName;
			final HashMap<String, TClass> currentPck = getPackage(classes,
					sourcePck);
			final TClass currentClassList = getTClass(currentPck, sourcePck,
					nested.sourceClassName, nested.targetPackageName,
					nested.targetClassName);
			currentClassList.list.add(nested);
		}

		for (final ConstantsRename constants : constantsRename.values()) {
			final String sourcePck = constants.packageName;
			final HashMap<String, TClass> currentPck = getPackage(classes,
					sourcePck);
			final TClass currentClassList = getTClass(currentPck, sourcePck,
					constants.sourceClassName, constants.packageName,
					constants.sourceClassName);
			currentClassList.list.add(constants);
		}

		for (final FieldRename field : fieldsRename.values()) {
			final String sourcePck = field.packageName;
			final HashMap<String, TClass> currentPck = getPackage(classes,
					sourcePck);
			final TClass currentClassList = getTClass(currentPck, sourcePck,
					field.className, field.packageName, field.className);
			currentClassList.list.add(field);
		}

		for (final MethodOverride mo : methodsOverride.values()) {
			final String sourcePck = mo.sourcePackageName;
			final HashMap<String, TClass> currentPck = getPackage(classes,
					sourcePck);
			final TClass currentClassList = getTClass(currentPck, sourcePck,
					mo.sourceClassName, mo.targetPackageName,
					mo.sourceClassName);
			currentClassList.list.add(mo);
		}

		for (final PropertyRename mo : propertiesRename.values()) {
			final String sourcePck = mo.sourcePackageName;
			final HashMap<String, TClass> currentPck = getPackage(classes,
					sourcePck);
			final TClass currentClassList = getTClass(currentPck, sourcePck,
					mo.sourceClassName, mo.sourcePackageName,
					mo.sourceClassName); // !!
			currentClassList.list.add(mo);
		}

		for (final NestedToInnerRefactoring mo : nestedToInnerList.values()) {
			final String sourcePck = mo.sourcePackageName;
			final HashMap<String, TClass> currentPck = getPackage(classes,
					sourcePck);
			final TClass currentClassList = getTClass(currentPck, sourcePck,
					mo.sourceClassName, mo.sourcePackageName,
					mo.sourceClassName);
			currentClassList.list.add(mo);
		}

		for (final List<ClassMappingFromComments> mo : classMappings.values()) {
			for (final ClassMappingFromComments cmf : mo) {
				final String sourcePck = cmf.sourcePackageName;
				final HashMap<String, TClass> currentPck = getPackage(classes,
						sourcePck);
				final TClass currentClassList = getTClass(currentPck,
						sourcePck, cmf.sourceClassName, cmf.sourcePackageName,
						cmf.sourceClassName);
				currentClassList.list.add(cmf);
			}
		}

		for (String packRenamed : this.packageRename.keySet()) {
			HashMap<String, TClass> currentPck = getPackage(classes,
					packRenamed);
		}

		for (final List<FieldMappingFromComments> mo : fieldMappings.values()) {
			for (final FieldMappingFromComments cmf : mo) {
				final String sourcePck = cmf.sourcePackageName;
				final HashMap<String, TClass> currentPck = getPackage(classes,
						sourcePck);
				final TClass currentClassList = getTClass(currentPck,
						sourcePck, cmf.sourceClassName, cmf.sourcePackageName,
						cmf.sourceClassName);
				currentClassList.list.add(cmf);
			}
		}

		for (final List<MethodMappingFromComments> mo : methodMappings.values()) {
			for (final MethodMappingFromComments cmf : mo) {
				final String sourcePck = cmf.sourcePackageName;
				final HashMap<String, TClass> currentPck = getPackage(classes,
						sourcePck);
				final TClass currentClassList = getTClass(currentPck,
						sourcePck, cmf.sourceClassName, cmf.sourcePackageName,
						cmf.sourceClassName);
				currentClassList.list.add(cmf);
			}
		}

		//
		//
		//

		final StringBuilder builder = new StringBuilder();
		// boolean xml = false;
		if (isXML) {
			builder.append("<?xml version=\"1.0\" encoding=\"utf-8\"?>\n");
			builder.append("<!--              -->\n");
			builder.append("<!--   generated  -->\n");
			builder.append("<!--              -->\n");
			builder.append("<mapping>\n");
			builder.append("   <packages>\n");
			for (final String pck : classes.keySet()) {
				//
				//
				//
				builder.append("      <!--                       -->\n");
				builder.append("      <!--   package " + pck + " -->\n");
				builder.append("      <!--                       -->\n");
				builder.append("      <package name=\"" + pck + "\">\n");
				builder.append("         <target name=\""
						+ buildTargetPackage(pck) + "\"");
				if (nonePackageMapping) {
					builder.append("   packageMappingBehavior=\"none\"");
				}
				if (noneMemberMapping) {
					builder.append("   memberMappingBehavior=\"none\"");
				}
				builder.append(">\n");
				builder.append("         </target>\n");
				final HashMap<String, TClass> currentPck = classes.get(pck);
				for (final String className : currentPck.keySet()) {
					final TClass clazz = currentPck.get(className);
					if (!clazz.sourceName.contains("Anonymous_C")) {
						final String targetPck = buildTargetPackage(clazz.targetPck);
						builder
								.append("         <!--                                  -->\n");
						builder.append("         <!--   class "
								+ clazz.sourceName + " -->\n");
						builder
								.append("         <!--                                  -->\n");
						builder.append("         <class name=\""
								+ Utils.xmlify(clazz.sourceName)
								+ "\" packageName=\"" + clazz.sourcePck
								+ "\">\n");
						builder.append("            <target name=\""
								+ Utils.xmlify(clazz.targetName)
								+ "\" packageName=\"" + targetPck + "\" ");
						for (final ImplictModification rename : clazz.list) {
							rename.xmlAttributesForTargetPart(builder);
						}
						builder.append(">\n");
						for (final ImplictModification rename : clazz.list) {
							rename.xmlElementsForTargetPart(builder);
						}
						builder.append("</target>\n");
						for (final ImplictModification rename : clazz.list) {
							rename.toXML(builder);
						}
						builder.append("         </class>\n\n");
					}
				}
				builder.append("      </package>\n\n");
			}
			builder.append("   </packages>\n");
			builder.append("</mapping>\n");
		} else {
			for (final String pck : classes.keySet()) {
				builder.append("package " + pck + " :: "
						+ buildTargetPackage(pck) + " {" + "\n");
				if (nonePackageMapping) {
					builder.append("   packageMappingBehavior=none;\n");
				}
				if (noneMemberMapping) {
					builder.append("   memberMappingBehavior=none;\n");
				}

				final HashMap<String, TClass> currentPck = classes.get(pck);
				for (final String className : currentPck.keySet()) {
					final TClass clazz = currentPck.get(className);
					if (!clazz.sourceName.contains("Anonymous_C")) {
						final String targetPck = buildTargetPackage(clazz.targetPck);
						builder.append("   class " + clazz.sourcePck + "."
								+ clazz.sourceName + " :: " + targetPck + ":"
								+ clazz.targetName + " {\n");
						/*
						 * for (final ImplictModification rename : clazz.list) {
						 * rename.print(builder); }
						 */
						for (final ImplictModification rename : clazz.list) {
							rename.print(builder);
						}
						builder.append("   }\n");
					}
				}
				builder.append("}\n");
			}
		}
		return builder;
	}

	public void generateMappingInCommentsFile(boolean isXML) throws Exception {
		final StringBuilder builder = prepareBuffer(isXML);
		if (builder.toString() != null) {
			final File projectFile = translateInfo.getConfiguration()
					.getOriginalProject().getProject().getLocation().toFile();

			//
			String translatorDirectory = TranslateInfo.TRANSLATOR_K;
			File directory = new File(projectFile.getAbsolutePath()
					+ File.separator + translatorDirectory);
			if (!directory.exists()) {
				translatorDirectory = "src/main/translator";
				directory = new File(projectFile.getAbsolutePath()
						+ File.separator + translatorDirectory);
				if (!directory.exists()) {
					translateInfo.getConfiguration().getLogger().logError(
							"Error, can't find translator directory for project "
									+ projectFile.getName());
					return;
				}
			}
			//
			final String dirName = projectFile.getAbsolutePath()
					+ File.separator + translatorDirectory + File.separator
					+ "configuration" + File.separator;

			final File dir = new File(dirName);
			if (!dir.exists()) {
				if (!dir.mkdirs()) {
					translateInfo.getConfiguration().getOptions().getGlobalOptions()
							.getLogger().logError(
									"Can't create directory " + dirName);
				}
			}

			final String generatedName = dirName
					+ translateInfo.getConfiguration().getOriginalProject()
							.getProject().getName() + "-" + filename
					+ MAPPING_FILE_EXTENTION;

			final File file = new File(generatedName);
			if (!file.exists()) {
				if (!file.createNewFile()) {
					translateInfo.getConfiguration().getOptions().getGlobalOptions()
							.getLogger().logError(
									"Can't create file " + generatedName);
				}
			} else {
				if (file.delete()) {
					if (!file.createNewFile()) {
						translateInfo.getConfiguration().getOptions()
								.getGlobalOptions().getLogger().logError(
										"Can't create file " + generatedName);
					}
				} else {
					translateInfo.getConfiguration().getOptions().getGlobalOptions()
							.getLogger().logError(
									"Can't delete file " + generatedName);
				}
			}

			final FileOutputStream stream = new FileOutputStream(file);
			if (translateInfo.getConfiguration().getOptions().getGlobalOptions()
					.isDebug()) {
				translateInfo.getConfiguration().getOptions().getGlobalOptions()
						.getLogger().logInfo(
								"Trying to write " + builder.toString()
										+ " in " + filename);
			}

			stream.write(builder.toString().getBytes());
			stream.flush();
			stream.close();
		}
	}

	//
	public void generateImplicitMappingFile(boolean isXML) throws Exception {
		final StringBuilder builder = prepareBuffer(isXML);
		if (builder.toString() != null) {
			if (translateInfo.getConfiguration().getOptions()
					.isGenerateImplicitMappingFile()) {
				final File projectFile = translateInfo.getConfiguration()
						.getOriginalProject().getProject().getLocation()
						.toFile();

				//
				String translatorDirectory = TranslateInfo.TRANSLATOR_K;
				File directory = new File(projectFile.getAbsolutePath()
						+ File.separator + translatorDirectory);
				if (!directory.exists()) {
					translatorDirectory = "src/main/translator";
					directory = new File(projectFile.getAbsolutePath()
							+ File.separator + translatorDirectory);
					if (!directory.exists()) {
						translateInfo.getConfiguration().getLogger().logError(
								"Error, can't find translator directory for project "
										+ projectFile.getName());
						return;
					}
				}
				//
				final String dirName = projectFile.getAbsolutePath()
						+ File.separator + translatorDirectory + File.separator
						+ "configuration" + File.separator + "generated"
						+ File.separator;

				final File dir = new File(dirName);
				if (!dir.exists()) {
					if (!dir.mkdirs()) {
						translateInfo.getConfiguration().getOptions()
								.getGlobalOptions().getLogger().logError(
										"Can't create directory " + dirName);
					}
				}

				String revisionKind = "FULL";
				if (translateInfo.getConfiguration().getOptions().getClassFilter() != null) {
					// Ok it's a partial translation so we don't want to loose
					// info for other classes
					revisionKind = "UPDATE";
				} else {
					// remove partial translation file
					final String generatedName = dirName
							+ translateInfo.getConfiguration().getOriginalProject()
									.getProject().getName() + "-generated"
							+ "UPDATE" + XML_MAPPING_FILE_EXTENTION;
					final File f = new File(generatedName);
					if (f.exists()) {
						if (!f.delete()) {
							translateInfo.getConfiguration().getOptions()
									.getGlobalOptions().getLogger().logError(
											"Can't delete file "
													+ generatedName);
						}
					}
				}

				final String generatedName = dirName
						+ translateInfo.getConfiguration().getOriginalProject()
								.getProject().getName() + "-generated"
						+ revisionKind + XML_MAPPING_FILE_EXTENTION;

				final File file = new File(generatedName);
				if (!file.exists()) {
					if (!file.createNewFile()) {
						translateInfo.getConfiguration().getOptions()
								.getGlobalOptions().getLogger().logError(
										"Can't create file " + generatedName);
					}
				} else {
					if (file.delete()) {
						if (!file.createNewFile()) {
							translateInfo.getConfiguration().getOptions()
									.getGlobalOptions().getLogger().logError(
											"Can't create file "
													+ generatedName);
						}
					} else {
						translateInfo.getConfiguration().getOptions()
								.getGlobalOptions().getLogger().logError(
										"Can't delete file " + generatedName);
					}
				}

				final FileOutputStream stream = new FileOutputStream(file);
				if (translateInfo.getConfiguration().getOptions().getGlobalOptions()
						.isDebug()) {
					translateInfo.getConfiguration().getOptions().getGlobalOptions()
							.getLogger().logInfo(
									"Trying to write " + builder.toString()
											+ " in " + generatedName);
				}

				stream.write(builder.toString().getBytes());
				stream.flush();
				stream.close();
			} else {
				System.out.println(builder.toString());
			}
		}
	}

	public void addImplicitFieldRename(String packageName, String className,
			String oldName, String newName) {
		final FieldRename fr = new FieldRename();
		fr.packageName = packageName;
		fr.className = className.replace("_Constants", "");
		fr.oldName = oldName;
		fr.newName = newName;
		fieldsRename.put(packageName + "." + className + "." + newName, fr);
	}

	public void addImplicitPackageRename(String oldPackageName,
			String newPackageName) {
		PackageRename pack = new PackageRename();
		pack.oldName = oldPackageName;
		pack.newName = newPackageName;
		packageRename.put(oldPackageName, pack);
	}

	public String getFieldRename(String name) {
		final FieldRename fr = fieldsRename.get(name);
		if (fr != null)
			return fr.oldName;
		return null;
	}

	private static String printTypeParam(String[] typesName) {
		String res = "";
		if (typesName == null)
			return res;
		for (int i = 0; i < typesName.length; i++) {
			res += typesName[i];
			if (i < typesName.length - 1)
				res += ",";
		}
		return res;
	}

	public void addImplicitProperty(String packageName, String className,
			PropertyInfo info) {
		final PropertyRename nested = new PropertyRename();
		nested.sourceClassName = className;
		nested.sourcePackageName = packageName;
		nested.propertyName = info.getName();
		if (info.getGetMethod() != null)
			nested.getterName = buildSignature(info.getGetMethod());
		if (info.getSetMethod() != null)
			nested.setterName = buildSignature(info.getSetMethod());
		propertiesRename.put(packageName + CSharpModelUtil.CLASS_SEPARATOR
				+ className + "_" + info.getName(), nested);
	}

	private String buildSignature(MethodInfo getMethod) {
		final String name = getMethod.getName();
		final String[] pTypes = getMethod.getParameterTypes();
		String sig = "";
		sig += name;
		sig += "(";
		for (int i = 0; i < pTypes.length; i++) {
			sig += Signature.toString(pTypes[i]);
			if (i < pTypes.length - 1) {
				sig += ", ";
			}
		}
		sig += ")";
		return InterfaceRenamingUtil.replaceInterfaceRenamed(sig);
		// return sig;
	}

	public void addImplicitNestedToInnerTransformation(String pckName,
			String className) {
		final NestedToInnerRefactoring nested = new NestedToInnerRefactoring();
		nested.sourceClassName = className;
		nested.sourcePackageName = pckName;
		nestedToInnerList.put(pckName + CSharpModelUtil.CLASS_SEPARATOR
				+ className, nested);
	}

	public String getNewNestedName(String pck, String className) {
		return mappingNestedClassRename.get(pck + "." + className);
	}

	public String resolveTypeName(String packageName, String typeName) {
		final NestedRename res = nestedClassRename.get(packageName
				+ CSharpModelUtil.CLASS_SEPARATOR + typeName);
		if (res != null) {
			return res.sourcePackageName + CSharpModelUtil.CLASS_SEPARATOR
					+ res.sourceClassName;
		}
		return null;
	}

	//
	// javadoc mapping
	//

	public void addJavaDocMapping(String pattern, String replacement,
			IProject reference) {
		javaDocMappings.put(pattern, replacement);
	}

	public HashMap<String, String> getJavaDocMappings() {
		return javaDocMappings;
	}

	public String getJavaDocTagMapping(String tag) {
		return javaDocMappings.get(tag);
	}

	//
	// Disclaimer
	//

	public String getDisclaimer() {
		return disclaimer.getValue().replace("${Plugin-Version}", TranslationPlugin.getVersion());
	}

	public void setDisclaimer(String disclaimer) {
		this.disclaimer.setValue(disclaimer);
	}
}
