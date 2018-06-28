package com.ilog.translator.java2cs.configuration.info;

import java.util.Collection;
import java.util.HashMap;

import org.eclipse.core.resources.IProject;
import org.eclipse.jdt.core.IType;
import org.eclipse.jdt.core.ITypeParameter;
import org.eclipse.jdt.core.JavaModelException;
import org.eclipse.jdt.core.Signature;
import org.w3c.dom.Element;
import org.w3c.dom.Node;
import org.w3c.dom.NodeList;

import com.ilog.translator.java2cs.configuration.options.BooleanOptionBuilder;
import com.ilog.translator.java2cs.configuration.options.BooleanOptionEditor;
import com.ilog.translator.java2cs.configuration.options.DotNetFramework;
import com.ilog.translator.java2cs.configuration.options.OptionImpl;
import com.ilog.translator.java2cs.configuration.options.OptionImpl.XMLKind;
import com.ilog.translator.java2cs.configuration.target.TargetPackage;

/**
 * Encapsulate a java package and its c# counterpart (target)
 * 
 * @author afau
 * 
 */
public class PackageInfoImpl extends ElementInfo implements PackageInfo {
		
	//
	//
	//
	private HashMap<String, TargetPackage> targetPackages = new HashMap<String, TargetPackage>();
	private final HashMap<String, ClassInfo> classesList = new HashMap<String, ClassInfo>();
	private final IProject referenceProject;

	//
	// Options
	//
	private OptionImpl<Boolean> isWildcard = new OptionImpl<Boolean>("isWildcard",
			null, false, OptionImpl.Status.PRODUCTION, new BooleanOptionBuilder(),
			new BooleanOptionEditor(), XMLKind.ATTRIBUT, "Does the package has .* ?");


	//
	// Constructor
	//

	protected PackageInfoImpl(MappingsInfo mappingInfo, String name, IProject reference) {
		super(mappingInfo, name);
		this.referenceProject = reference;
	}

	//
	// Wildcard (.*)
	//

	public boolean isWildcard() {
		return isWildcard.getValue();
	}

	public void setWildcard(boolean isWildcard) {
		this.isWildcard.setValue(isWildcard);
	}

	//
	// Target
	//

	public TargetPackage getTarget(String targetFramework) {
		// check jdk and framework compatibility ?
		return targetPackages.get(targetFramework);
	}

	public void addTarget(String targetFramework, TargetPackage pck) {
		targetPackages.put(targetFramework,pck);
	}

	//
	//
	//
	
	public IProject getReference() {
		return referenceProject;
	}
	
	//
	// classes
	//

	public void addClass(IType type, ClassInfo ci, boolean generics) {
		try {
			String stp = null;
			if (generics) {
				final ITypeParameter[] tparams = type.getTypeParameters();
				stp = "";
				if (tparams != null && tparams.length > 0) {
					for (int i = 0; i < tparams.length; i++) {
						stp += "%" + (i + 1);
						if (i < tparams.length - 1) {
							stp += ",";
						}
					}
				}
			}
			final String signature2 = resolve(type, stp);
			classesList.put(signature2, ci);
		} catch (final Exception e) {
			mappingInfo.getTranslateInfo().configuration.getLogger().logException("", e);
			// e.printStackTrace();
		}
	}

	public ClassInfo getClass(IType type) {
		final String signature2 = resolve(type, null);
		final ClassInfo ci = classesList.get(signature2);
		// check jdk and framework compatibility ?
		return ci;

	}

	public ClassInfo getClass(String fqname) throws JavaModelException {
		final boolean generics = isGenericsName(fqname);
		// need a way to associate the "true" generic type here !
		final IType type = mappingInfo.getTranslateInfo().typeforName(fqname,
				generics);
		if (type == null) {
			return null;
		}
		final String signature2 = resolve(type, generics ? fqname
				.substring(fqname.indexOf("<")) : null);
		final ClassInfo ci = classesList.get(signature2);
		// check jdk and framework compatibility ?
		return ci;
	}

	public ClassInfo createClass(String fqname, boolean isGeneric)
			throws JavaModelException {
		final boolean generics = isGenericsName(fqname);
		IType type = mappingInfo.getTranslateInfo().typeforName(fqname, generics);
		if (type == null) {
			type = mappingInfo.getTranslateInfo().typeforName(
					fqname.replace("$", "."), generics);
			if (type == null)
				return null;
		}
		final ClassInfo ci = new ClassInfoImpl(mappingInfo, type, generics || isGeneric,
				this);
		addClass(type, ci, generics || isGeneric);
		return ci;
	}

	private boolean isGenericsName(String fqname) {
		return fqname.contains("<");
	}

	private String resolve(IType type, String generics) {
		String typeName = "";
		while (type.getDeclaringType() != null) {
			typeName += "." + type.getElementName();
			type = type.getDeclaringType();
		}

		typeName = type.getElementName() + typeName;
		final String resolvedTypeName = mappingInfo.getTranslateInfo()
				.resolveTypeName(name.getValue(), typeName);

		final String signature2 = Signature.createTypeSignature(
				resolvedTypeName, true);
		if (generics != null) {
			return signature2 + eraseTypeParameter(generics);
		}
		return signature2;
	}

	private String eraseTypeParameter(String className) {
		final int sindex = className.indexOf("<");
		if (sindex >= 0) {
			final int vindex = className.indexOf(",");
			String NclassName = className.substring(0, sindex);// + "<";
			NclassName += "%1";
			if (vindex > 0) {
				NclassName += ",%2";
			}
			return NclassName;
		}
		return className;
	}

	public Collection<ClassInfo> getAllClasses() {
		return classesList.values();
	}

	public ClassInfo getGenericClass(IType type)
			throws TranslationModelException {
		try {
			final ITypeParameter[] tparams = type.getTypeParameters();
			String stp = "";
			if (tparams != null && tparams.length > 0) {
				for (int i = 0; i < tparams.length; i++) {
					stp += "%" + (i + 1);
					if (i < tparams.length - 1) {
						stp += ",";
					}
				}
			}
			final String signature2 = resolve(type, stp);
			final ClassInfo ci = classesList.get(signature2);
			// check jdk and framework compatibility ?
			return ci;
		} catch (final JavaModelException e) {
			throw new TranslationModelException(
					"PackageInfo::getGenericClass: JDT Trouble with class "
							+ type.getElementName(), e);
		}
	}

	//
	// toFile
	//

	public String toFile() {
		final StringBuilder res = new StringBuilder();
		res.append("package " + name);
		printTargetPackage(res);
		res.append(" {");
		
		for (final ClassInfo ci : classesList.values()) {
			res.append(ci.toFile());
		}
		res.append("}\n");
		return res.toString();
	}

	private void printTargetPackage(StringBuilder res) {
		if (targetPackages != null && targetPackages.size() > 0) {
			for(TargetPackage tp : targetPackages.values())
				res.append(" :: " + tp.getName());
		}
	}

	//
	// toXML
	//
	// <package name="...">
    //   <target ... />
	//   <class ... />
	// </package>
	public void toXML(StringBuilder res, String tabValue) {
		res.append(Constants.TAB + "<!--                      -->\n");
		res.append(Constants.TAB + "<!-- package " + name.getValue() + " -->\n");
		res.append(Constants.TAB + "<!--                      -->\n");
		res.append(Constants.TAB + "<package name=\"" + name.getValue() + "\"");

		//ElementInfo
		//.toXML(res, isExcluded, "\n" + TWOTAB + "        ", "", "");
		toXML(res, sinceJDK, "\n" + Constants.FOURTAB + "       ", "", "");
		
		res.append(">\n");
		if (targetPackages != null && targetPackages.size() > 0) {
			for(TargetPackage tp : targetPackages.values())
				tp.toXML(res, tabValue);
		}
		for (final ClassInfo ci : classesList.values()) {
			ci.toXML(res, tabValue);
		}
		res.append(Constants.TAB + "</package>\n");
	}

	//
	// fromXML
	//
	// <package name="...">
    //   <target ... />
	//   <class ... />
	// </package>
	public void fromXML(Element pack) {
		NodeList child = pack.getChildNodes();
		
		//isExcluded.fromXML(pack);
		sinceJDK.fromXML(pack);
		
		for (int i = 0; i < child.getLength(); i++) {
			Node pckNode = child.item(i);

			if (pckNode.getNodeType() == Node.ELEMENT_NODE) {
				Element pckElement = (Element) pckNode;
				String targetFramework = pckElement.getAttribute("dotnetFramework");
				if (targetFramework == null || targetFramework.isEmpty())
					targetFramework = DotNetFramework.NET3_5.name();
				String elemName = pckElement.getNodeName();
				if (elemName.equals("target")) {
					TargetPackage targetPackage = new TargetPackage();
					targetPackage.setSourcePackageName(name.getValue());
					targetPackage.fromXML(pckElement);
					targetPackages.put(targetFramework, targetPackage);
				} else if (elemName.equals("class")) {
					String className = pckElement.getAttribute("name");
					String pckName   = pckElement.getAttribute("packageName");
					if (pckName.isEmpty() && name.getValue() != null) {
						// allow empty package name (reuse package one) for class !!! :-)
						pckName = name.getValue();
					}
					String typeParam = pckElement.getAttribute("typeParameters");
					boolean generics = !typeParam.isEmpty();
					if (generics) {
						typeParam = "<" + typeParam + ">";
					}
					try {
						ClassInfo cinfo = getClass(pckName + "." + className + typeParam);
						if (cinfo == null) {
							cinfo = createClass(
									pckName + "." + className + typeParam, generics);
						}
						if (cinfo == null) {
							mappingInfo.getTranslateInfo().getConfiguration().getLogger().logError(
									"Can't find class : " + pckName + "." + className + typeParam);
						} else
							cinfo.fromXML(pckElement);
					} catch (JavaModelException e) {
						// report error
					}
				}
			}
		}
	}

}