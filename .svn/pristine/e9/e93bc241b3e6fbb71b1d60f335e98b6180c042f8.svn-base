package com.ilog.translator.java2cs.configuration.info;

import java.util.ArrayList;
import java.util.Arrays;
import java.util.HashMap;
import java.util.Hashtable;
import java.util.Iterator;
import java.util.List;
import java.util.Map;

import org.eclipse.core.runtime.NullProgressMonitor;
import org.eclipse.jdt.core.Flags;
import org.eclipse.jdt.core.IField;
import org.eclipse.jdt.core.IJavaProject;
import org.eclipse.jdt.core.IMember;
import org.eclipse.jdt.core.IMethod;
import org.eclipse.jdt.core.IType;
import org.eclipse.jdt.core.JavaModelException;
import org.eclipse.jdt.core.Signature;
import org.eclipse.jdt.internal.codeassist.SelectionEngine;
import org.w3c.dom.Element;
import org.w3c.dom.Node;
import org.w3c.dom.NodeList;

import com.ilog.translator.java2cs.configuration.options.BooleanOptionBuilder;
import com.ilog.translator.java2cs.configuration.options.BooleanOptionEditor;
import com.ilog.translator.java2cs.configuration.options.DotNetFramework;
import com.ilog.translator.java2cs.configuration.options.MappingOverridingPolicy;
import com.ilog.translator.java2cs.configuration.options.MappingOverridingPolicyOptionBuilder;
import com.ilog.translator.java2cs.configuration.options.MappingOverridingPolicyOptionEditor;
import com.ilog.translator.java2cs.configuration.options.OptionImpl;
import com.ilog.translator.java2cs.configuration.options.OptionImpl.XMLKind;
import com.ilog.translator.java2cs.configuration.target.TargetClass;
import com.ilog.translator.java2cs.configuration.target.TargetMethod;
import com.ilog.translator.java2cs.configuration.target.TargetProperty;
import com.ilog.translator.java2cs.translation.noderewriter.FieldRewriter;
import com.ilog.translator.java2cs.translation.noderewriter.IndexerRewriter.IndexerKind;
import com.ilog.translator.java2cs.translation.noderewriter.PropertyRewriter.ProperyKind;
import com.ilog.translator.java2cs.translation.util.InterfaceRenamingUtil;
import com.ilog.translator.java2cs.translation.util.TranslationUtils;
import com.ilog.translator.java2cs.util.Utils;

/**
 * 
 * @author afau
 * 
 *         Encapsulate a java class and its translation ("target" class)
 */
@SuppressWarnings("restriction")
public class ClassInfoImpl extends ElementInfo implements ClassInfo {
	
	private static final String CONSTRUCTOR_NAME = "<init>";
	
	//
	private HashMap<String, TargetClass> targetClasses = new HashMap<String, TargetClass>();
	private final IType clazz;
	private final HashMap<String, FieldInfo> fields;
	private final HashMap<String, MethodInfo> methods;
	private final HashMap<String, PropertyInfo> properties;
	private final HashMap<String, IndexerInfo> indexers;
	protected HashMap<IMethod, String> methodsCache = new HashMap<IMethod, String>();
	protected HashMap<IField, String> fieldsCache = new HashMap<IField, String>();
	private final List<ClassInfo> parents = new ArrayList<ClassInfo>();
	private boolean parentComputed= false;
	private PackageInfo packageInfo;
	private boolean nullable;

	//
	// options
	//
	private OptionImpl<Boolean> generics = new OptionImpl<Boolean>("generics",
			null, false, OptionImpl.Status.PRODUCTION,
			new BooleanOptionBuilder(), new BooleanOptionEditor(),
			XMLKind.ATTRIBUT, "");
	private OptionImpl<Boolean> hasCovariantMethod = new OptionImpl<Boolean>(
			"hasCovariantMethod", null, false, OptionImpl.Status.PRODUCTION,
			new BooleanOptionBuilder(), new BooleanOptionEditor(),
			XMLKind.ATTRIBUT, "The class has covariant method");		
	protected OptionImpl<MappingOverridingPolicy> mappingOverridePolicy = new OptionImpl<MappingOverridingPolicy>(
			"mappingOverridePolicy", null, MappingOverridingPolicy.OVERRIDE, OptionImpl.Status.PRODUCTION,
			new MappingOverridingPolicyOptionBuilder(), new MappingOverridingPolicyOptionEditor(),
			XMLKind.ATTRIBUT, "Mapping overriding policy behavior (override or replace)");
	
	//
	private final List<String> explicitInterfaceMethods;

	//	
	private String simpleName = null;
	private boolean isMember = false;

	//
	// constructor
	//


	/**
	 * Create a new ClassInfo with given name.
	 * 
	 * @param name
	 *            the name of this ClassInfo.
	 * @throws a
	 *             ClassNotFoundException.
	 */
	protected ClassInfoImpl(MappingsInfo mappingInfo, IType type, boolean generics,
			PackageInfo packageInfo) throws JavaModelException {
		super(mappingInfo, type.getElementName());
		clazz = type;
		methods = new HashMap<String, MethodInfo>();
		fields = new HashMap<String, FieldInfo>();
		properties = new HashMap<String, PropertyInfo>();
		indexers = new HashMap<String, IndexerInfo>();
		explicitInterfaceMethods = new ArrayList<String>();
		packageName.setValue(type.getPackageFragment().getElementName());
		this.generics.setValue(generics);
		name.setValue(TranslationUtils.computeName(clazz, generics));
		simpleName = TranslationUtils.computeSimpleName(clazz, generics);
		this.packageInfo = packageInfo;
		isMember = type.isMember();
	}

	//
	// type
	//

	public IType getType() {
		return clazz;
	}
	
	//
	// isMember
	//
	
	public boolean isMember() {
		return isMember;
	}

	
	//
	// name
	//

	@Override
	public String getName() {
		return name.getValue();
	}

	//
	// Method
	//	

	public HashMap<String, MethodInfo> getMethodsMap() {
		return methods;
	}

	/**
	 * 
	 * @param signature
	 *            The signature of a method
	 * @return Returns the MethodInfo
	 */
	public MethodInfo getMethod(String signature, IMethod method)
			throws JavaModelException {
		MethodInfo res = null;
		if (method != null) {
			String key = null;

			if ((signature != null) && !signature.equals("")) {
				key = signature;
			} else {
				key = this.getSignature(null, method, false);
			}
			res = methods.get(key);
			if (res == null) {
				res = this.methods.get(key.replace("_", "$")); // TODO: HACK
				// !!!
			}
			if (res == null) {
				res = this.methods.get(key.replace("_1", "")); // Type parameter
				// rename ...
			}
			if (res == null) {
				// TODO: Time consuming .... :-(
				final String[] pTypes = method.getParameterTypes();
				if (pTypes != null && pTypes.length > 0) {
					for (final String pType : pTypes) {
						final String cName = Signature.toString(pType);
						final String[][] resolved = method.getDeclaringType()
								.resolveType(cName);
						if (resolved != null && resolved[0] != null) {
							final MappingsInfo.NestedRename res0 = mappingInfo.translateInfo
									.getImplicitNestedRename(resolved[0][0]
											+ "." + resolved[0][1]);
							if (res0 != null) {
								final String s = res0.sourcePackageName + "."
										+ res0.sourceClassName;
								key = key.replace(resolved[0][0] + "."
										+ resolved[0][1], s);
							}
						}
					}
					res = methods.get(key);
				}
			}
		} else if (signature != null) {
			final String key = signature;
			res = methods.get(key);
			if (res == null) {
				res = this.methods.get(key.replace("_", "$")); // TODO: HACK
				// !!!
			}
		}
		if (res == null && method != null && !isConstructor(method)) {
			for (final ClassInfo parent : parents) {
				res = parent.getMethod(signature, method);
				if (res != null) {
					String targetFramework = getPackageInfo().getMappingInfo().getTranslateInfo().
							getConfiguration().getOptions().getTargetDotNetFramework().name();
					if ((res.getTarget(targetFramework) != null)
							&& (res.getTarget(targetFramework).getRewriter() != null)
							&& (res.getTarget(targetFramework).getRewriter()
									.getChangeModifier() != null)) {
						// hum hum hum
						res = res.cloneForChild();
					}
					return res;
				}
			}
			// We don't find a MethodInfo
			// skip future search
			if (res == null) {
				String key = null;
				if ((signature != null) && !signature.equals("")) {
					key = signature;
				} else {
					key = this.getSignature(null, method, false);
				}
				if (key != null)
					methods.put(key, null);
			}
		}
		// check jdk and framework compatibility ?
		return res;
	}

	/**
	 * Return the method info that correspond to the given signature.
	 * 
	 * @param sign
	 *            The signature of the method
	 * @param name
	 *            The name of the method
	 * @param classes
	 *            The parameters types
	 * @return Returns the MethodInfo
	 * @throws NoSuchMethodEcxeption
	 */
	public MethodInfo resolveMethod(String name, String[] classes)
			throws JavaModelException {
		return resolveMethod(name, classes, false);
	}

	/**
	 * Return the method info that correspond to the given signature.
	 * 
	 * @param sign
	 *            The signature of the method
	 * @param name
	 *            The name of the method
	 * @param classes
	 *            The parameters types
	 * @return Returns the MethodInfo
	 * @throws NoSuchMethodEcxeption
	 */
	public MethodInfo resolveMethod(String name, String[] classes, boolean fqn)
			throws JavaModelException {
		MethodInfo minfo = null; // (MethodInfo) methods.get(sign);
		IMethod meth = null;
		if (minfo == null) {
			if (name.equals(ClassInfoImpl.CONSTRUCTOR_NAME) || name.equals( simpleName)) {
				meth = this.getMethod(name, classes);
				if (meth == null) {
					logError("ClassInfo.resolveMethod::Constructor for class "
							+ clazz.getFullyQualifiedName() + " not found.");
					// getMethod(name, classes);
				} else {
					minfo = new MethodInfo(mappingInfo, meth);
					minfo.setConstructro(true);
				}
				// sign = cons.getSignature();
			} else {
				meth = this.getMethod(name, classes);
				if (meth == null) {
					logError("ClassInfo.resolveMethod::Method " + name
							+ " for class " + clazz.getFullyQualifiedName()
							+ " not found.");
					meth = this.getMethod(name, classes);
					if (mappingInfo.getTranslateInfo().getConfiguration()
							.getOptions().getGlobalOptions().isDebug()) {
						final IMethod[] methods = clazz.getMethods();
						for (final IMethod m : methods) {
							log(m.toString());
						}
					}
					return null;
				} else {
					minfo = new MethodInfo(mappingInfo, meth);
				}
			}
			minfo.setClassInfo(this);
			final String key = this.getSignature(name, meth, fqn);
			methods.put(key, minfo);
		}
		return minfo;
	}

	private IMethod getMethod(String name, String[] itypes) {
		try {
			boolean isConstructor = false;
			if (name.equals(ClassInfoImpl.CONSTRUCTOR_NAME) || 
					name.equals(simpleName)) {
				isConstructor = true;
				name = null;
			}
			return ClassInfoImpl.findMethod(name, itypes, isConstructor, clazz);
		} catch (final JavaModelException e) {
			logError("ClassInfo.getMethod::error for " + name + " : " + e);
			return clazz.getMethod(name, itypes);
		}
	}

	/**
	 * Finds a method in a type. This searches for a method with the same name
	 * and signature. Parameter types are only compared by the simple name, no
	 * resolving for the fully qualified type name is done. Constructors are
	 * only compared by parameters, not the name.
	 * 
	 * @param name
	 *            The name of the method to find
	 * @param paramTypes
	 *            The type signatures of the parameters e.g.
	 *            <code>{"QString;","I"}</code>
	 * @param isConstructor
	 *            If the method is a constructor
	 * @return The first found method or <code>null</code>, if nothing found
	 */
	public static IMethod findMethod(String name, String[] paramTypes,
			boolean isConstructor, IType type) throws JavaModelException {
		final IMethod[] methods = type.getMethods();
		for (final IMethod element : methods) {
			if (isConstructor && type.isBinary() && type.isMember()
					&& !Flags.isStatic(type.getFlags())) {
				final List<String> newParamTypes = new ArrayList<String>();
				newParamTypes.add(Signature.createTypeSignature(type
						.getDeclaringType().getFullyQualifiedName(), true));
				for (final String t : paramTypes) {
					newParamTypes.add(t);
				}
				if (ClassInfoImpl.isSameMethodSignature(name, newParamTypes
						.toArray(new String[newParamTypes.size()]),
						isConstructor, element)) {
					return element;
				}
			} else if (ClassInfoImpl.isSameMethodSignature(name, paramTypes,
					isConstructor, element)) {
				return element;
			}
		}
		return null;
	}

	/**
	 * Tests if a method equals to the given signature. Parameter types are only
	 * compared by the simple name, no resolving for the fully qualified type
	 * name is done. Constructors are only compared by parameters, not the name.
	 * 
	 * @param name
	 *            Name of the method
	 * @param paramTypes
	 *            The type signatures of the parameters e.g.
	 *            <code>{"QString;","I"}</code>
	 * @param isConstructor
	 *            Specifies if the method is a constructor
	 * @return Returns <code>true</code> if the method has the given name and
	 *         parameter types and constructor state.
	 */
	public static boolean isSameMethodSignature(String name,
			String[] paramTypes, boolean isConstructor, IMethod curr)
			throws JavaModelException {
		if (isConstructor || name.equals(curr.getElementName())) {
			if (isConstructor == curr.isConstructor()) {
				final String[] currParamTypes = curr.getParameterTypes();
				if (paramTypes.length == currParamTypes.length) {
					for (int i = 0; i < paramTypes.length; i++) {
						final String t1 = Signature.getSimpleName(Signature
								.toString(paramTypes[i]));
						final String t2 = Signature.getSimpleName(Signature
								.toString(currParamTypes[i]));
						if (!t1.equals(t2)) {
							return false;
						}
					}
					return true;
				}
			}
		}
		return false;
	}

	//
	// signature
	//

	public String getSignature(String mName, IMethod m, boolean fqn)
			throws JavaModelException {
		final String name = methodsCache.get(m);

		if (name != null) {
			return name;
		}

		if (isConstructor(m) /* m.isConstructor() */) {
			if (m.getDeclaringType().isMember()
					&& (m.getParameterTypes().length > 0)) {
				final String firstArgName = Signature.toString(m
						.getParameterTypes()[0]);
				final String enclosingClassName = m.getDeclaringType()
						.getDeclaringType().getFullyQualifiedName();
				// IType firstArgType =
				// TranslationUtils.findTypeCustom(m.getJavaProject(),
				// m.getDeclaringType(), firstArgName); //
				String[][] type = null;
				try {
					type = m.getDeclaringType().resolveType(firstArgName);
				} catch (final Exception e) {
					logError("ClassInfo.getSignature::Warning *** Type '"
							+ firstArgName + "' could not be found. " + mName
							+ " " + m);
					e.printStackTrace();
				}
				String fqnFirstArg = null;
				if (type != null) {
					final String[] m2 = type[0];
					fqnFirstArg = m2[0] + "." + firstArgName;
				} else {
					fqnFirstArg = firstArgName;
				}
				/*
				 * if (firstArgType != null) { String fqnFirstArg =
				 * firstArgType.getFullyQualifiedName('.'); if
				 * (fqnFirstArg.equals(enclosingClassName)) { // ok let's remove
				 * it String[] params = null; if (m.getParameterTypes().length
				 * == 1) { String ss = Signature.createMethodSignature( new
				 * String[0], "V"); return ss + ClassInfo.CONSTRUCTOR_NAME; }
				 * else { params = new String[m.getParameterTypes().length - 1];
				 * for (int i = 1; i < m.getParameterTypes().length; i++) {
				 * params[i - 1] = m.getParameterTypes()[i]; } String ss =
				 * Signature .createMethodSignature(params, "V"); return ss +
				 * ClassInfo.CONSTRUCTOR_NAME; } } } else {
				 * this.logError("ClassInfo.getSignature::Warning *** Type '" +
				 * firstArgName + "' could not be found. " + mName + " " + m);
				 * // e.printStackTrace(); }
				 */
				if (fqnFirstArg.equals(enclosingClassName)) {
					// ok let's remove it
					String[] params = null;
					if (m.getParameterTypes().length == 1) {
						final String ss = Signature.createMethodSignature(
								new String[0], "V");
						return InterfaceRenamingUtil.replaceInterfaceRenamed(ss) + ClassInfoImpl.CONSTRUCTOR_NAME;
						// return ss + ClassInfoImpl.CONSTRUCTOR_NAME;
					} else {
						params = new String[m.getParameterTypes().length - 1];
						for (int i = 1; i < m.getParameterTypes().length; i++) {
							params[i - 1] = m.getParameterTypes()[i];
						}
						final String ss = Signature.createMethodSignature(
								params, "V");
						return InterfaceRenamingUtil.replaceInterfaceRenamed(ss) + ClassInfoImpl.CONSTRUCTOR_NAME;
						// return ss + ClassInfoImpl.CONSTRUCTOR_NAME;
					}
				}
			}
			final String ss = Signature.createMethodSignature(m
					.getParameterTypes(), "V");
			return InterfaceRenamingUtil.replaceInterfaceRenamed(ss) + ClassInfoImpl.CONSTRUCTOR_NAME;
			// return ss + ClassInfoImpl.CONSTRUCTOR_NAME;
		} else if (mName == null) {
			mName = m.getElementName();
		}

		final String sign = buildSignature(m, mName, fqn); // TODO : hum hum
		// hum
		return sign;
	}

	private String buildSignature(IMethod m, String methodName, boolean fqn)
			throws JavaModelException {
		return TranslationUtils.computeSignature(m) + methodName;
	}

	//
	// Constructor
	//

	public boolean isConstructor(IMethod m) {
		final String typeName = m.getDeclaringType().getElementName();
		return m.getElementName().equals(typeName);
	}
	
	public MethodInfo getConstructor(IMethod method) throws JavaModelException {
		final String key = this.getSignature(null, method, false);
		// check jdk and framework compatibility ?
		return methods.get(key);
	}

	//
	// Field
	//

	private void addField(String sign, FieldInfo field)
			throws JavaModelException {
		field.setClassInfo(this);
		fields.put(sign, field);
	}

	/**
	 * Return the field info that correspond to the given name.
	 * 
	 * @param description
	 * @return Returns the field
	 * @throws NoSuchFieldException
	 */
	public FieldInfo resolveField(String description) throws JavaModelException {
		FieldInfo field = fields.get(description);
		if (field == null) {
			final IField f = clazz.getField(description);
			field = new FieldInfo(mappingInfo, f);
			addField(this.getSignature(f), field);
		}
		return field;
	}

	private String getSignature(IField fi) throws JavaModelException {
		String signature = fieldsCache.get(fi);

		if (signature != null) {
			return signature;
		}

		String fname = fi.getElementName();
		final String fqn = fi.getDeclaringType().getFullyQualifiedName()
				.replace("$", ".");

		final String oldName = mappingInfo.getTranslateInfo().getFieldRename(
				fqn + "." + fname);

		if (oldName != null)
			fname = oldName;

		signature = fname;

		fieldsCache.put(fi, signature);

		return signature;
	}

	/**
	 * 
	 * @param description
	 * @return
	 */
	public FieldInfo getField(IField field) throws JavaModelException {
		final FieldInfo f = fields.get(this.getSignature(field));
		// check jdk and framework compatibility ?
		return f;
	}

	//
	// target class
	//
	
	public TargetClass getTarget(String targetFramework) {
		// check jdk and framework compatibility ?
		return targetClasses.get(targetFramework);
	}

	public void addTargetClass(String targetFramework, TargetClass tclazz) {
		targetClasses.put(targetFramework, tclazz);
		/*if (isPartial.getValue()) {
			tclass.setPartial(isPartial.getValue());
		}*/
	}

	public boolean hasTargetClass() {
		return (targetClasses != null && targetClasses.size() > 0);
	}

	//
	// explicitInterfaceMethods
	//

	public void addExplicitInterfaceMethods(String interf) {
		explicitInterfaceMethods.add(interf);
	}

	//
	// toString
	//

	@SuppressWarnings("unchecked")
	@Override
	public String toString() {
		String descr = "";
		descr += name.getValue() + " :: ";
		for(TargetClass tc : targetClasses.values())
			descr += tc + "{\n";
		Iterator iter = methods.values().iterator();
		while (iter.hasNext()) {
			descr += "  " + iter.next() + "\n";
		}
		iter = fields.values().iterator();
		while (iter.hasNext()) {
			descr += "  " + iter.next() + "\n";
		}
		descr += "}\n";
		return descr;
	}

	//
	// computeParents
	//
	
	public void computeParents(IJavaProject project,
			Hashtable<String, List<ClassInfo>> allClasses) throws JavaModelException {
		final IType type = getType();
		SelectionEngine.PERF = true;

		final String superClassName = type.getSuperclassName();
		final String[] superInterfaceNames = type.getSuperInterfaceNames();

		List<String> superTypes = null;
		if (superInterfaceNames != null) {
			superTypes = new ArrayList<String>(Arrays
					.asList(superInterfaceNames));
		} else {
			superTypes = new ArrayList<String>();
		}

		if (superClassName != null) {
			superTypes.add(superClassName);
		}

		for (String superInterName : superTypes) {
			final boolean isGenerics = isGenerics(superInterName)
					|| generics.getValue();

			// IType superClass = TranslationUtils.findTypeCustom(project, type,
			// superInterName);

			IType superClass = project.findType(superInterName,
					new NullProgressMonitor());

			if (superClass == null) {
				String[][] resolved = type.resolveType(superInterName);

				if (resolved != null) {
					superInterName = Signature.toQualifiedName(resolved[0]);
					superClass = project.findType(superInterName,
							new NullProgressMonitor());
				}
				if (superClass == null) { // print out resolved
					String res = "";
					if (resolved != null) {
						for (int i = 0; i < resolved.length; i++) {
							for (int j = 0; j < resolved[i].length; j++) {
								res += resolved[i][j] + " ";
							}
							res += " / ";
						}
					}
					this
							.logError("ClassInfo.computeParents:: Can't find Class "
									+ superInterName
									+ "/"
									+ res
									+ " in project " + project.getElementName());
					continue;

				}
			}

			// if (superClass != null) {
			String handler = TranslationUtils.computeName(superClass,
					isGenerics); // compute name
			List<ClassInfo> superClassInfo = allClasses.get(handler);
			if (superClassInfo != null) {
				this.parents.addAll(superClassInfo);
			} else {
				handler = TranslationUtils.computeName(superClass, isGenerics); // compute
				// name
				superClassInfo = allClasses.get(handler);
				if (superClassInfo != null) {
					this.parents.addAll(superClassInfo);
				} 
			}
			/*
			 * } else {
			 * this.logError("ClassInfo.computeParents:: Can't find Class " +
			 * superInterName + " in project " + project.getElementName());
			 * continue; }
			 */
		}
		parentComputed = true;
	}

	private void computeParentsOLD(IJavaProject project,
			Hashtable<String, ClassInfo> allClasses) throws JavaModelException {
		IType type = this.getType();

		String superClassName = type.getSuperclassName();
		String[] superInterfaceNames = type.getSuperInterfaceNames();

		List<String> superTypes = null;
		if (superInterfaceNames != null) {
			superTypes = new ArrayList<String>(Arrays
					.asList(superInterfaceNames));
		} else {
			superTypes = new ArrayList<String>();
		}

		if (superClassName != null) {
			superTypes.add(superClassName);
		}

		for (String superInterName : superTypes) {
			boolean isGenerics = isGenerics(superInterName)
					|| generics.getValue();

			final String[][] resolved = type.resolveType(superInterName);
			if (resolved != null) {
				superInterName = Signature.toQualifiedName(resolved[0]);
			}
			final IType superClass = project.findType(superInterName,
					new NullProgressMonitor());
			if (superClass == null) {
				// print out resolved
				String res = "";
				if (resolved != null) {
					for (int i = 0; i < resolved.length; i++) {
						for (int j = 0; j < resolved[i].length; j++) {
							res += resolved[i][j] + " ";
						}
						res += " / ";
					}
					logError("ClassInfo.computeParents:: Can't find Class "
							+ superInterName + "/" + res + " in project "
							+ project.getElementName());
				}
			} else {
				String handler = TranslationUtils.computeName(superClass,
						isGenerics); // compute name
				ClassInfo superClassInfo = allClasses.get(handler);
				if (superClassInfo != null) {
					parents.add(superClassInfo);
				} else {
					handler = TranslationUtils.computeName(superClass, false); // compute
					// name
					superClassInfo = allClasses.get(handler);
					if (superClassInfo != null) {
						parents.add(superClassInfo);
					}
				}
			}
		}
	}

	private boolean isGenerics(String typename) throws JavaModelException {
		final boolean res = false;
		if (typename.contains("<")) {
			final int index = typename.indexOf("<");
			final String gTypes = typename.substring(index + 1, typename
					.lastIndexOf(">"));
			// String[] args = gTypes.split(",");
			/*
			 * res += "<"; for (int i = 0; i < args.length; i++) { res += "%" +
			 * (i + 1); if (i < args.length - 1) { res += ","; } }
			 */
			return true;
		}
		return res;
	}

	//
	// log
	//
	
	private void log(String message) {
		mappingInfo.getTranslateInfo().getConfiguration().getLogger()
				.logInfo(message);
	}

	private void logError(String message) {
		mappingInfo.getTranslateInfo().getConfiguration().getLogger().logError(
				message);
	}

	//
	// Property
	//

	public void addProperty(String targetFramework, String name, ProperyKind kind, MethodInfo info, TargetProperty tProperty) {
		PropertyInfo pInfo = properties.get(name);
		if (pInfo != null) {
			pInfo.addGetOrSetMethod(kind, info);
			pInfo.addTarget(targetFramework, tProperty);
		} else {
			pInfo = new PropertyInfo(name, kind, info);
			pInfo.addTarget(targetFramework, tProperty);
			properties.put(name, pInfo);
		}
	}

	public Map<String, PropertyInfo> getProperties() {
		if (parents.size() == 0 && !parentComputed) {
			// 			
		}
		final Map<String, PropertyInfo> allProps = new HashMap<String, PropertyInfo>();
		for (final ClassInfo parent : parents) {			
			allProps.putAll(parent.getProperties());
		}
		allProps.putAll(properties);
		return allProps;
	}

	/**
	 * Add a list of propertyinfo computed
	 * 
	 * @param properties2
	 */
	public void addProperties(List<PropertyInfo> properties2) {
		for (final PropertyInfo info : properties2) {
			// PropertyInfo pInfo = properties.get(info.getName());
			/*
			 * if (pInfo != null) { // override ? properties.put(info.getName(),
			 * info); } else {
			 */
			properties.put(info.getName(), info);
			// }
			//
			String className = clazz.getElementName();
			IType currentType = clazz.getDeclaringType();
			while (currentType != null) {
				className = currentType.getElementName() + "." + className;
				currentType = currentType.getDeclaringType();
			}
			//
			mappingInfo.getTranslateInfo().addImplicitProperty(
					packageName.getValue(), className, info);
		}
	}

	//
	// Indexers
	//

	public void addIndexer(IndexerKind kind, int[] paramsIndexs,
			int valueIndex, MethodInfo info) {
		String key = "this" + paramsIndexs.length;
		final IndexerInfo pInfo = indexers.get(key /* name.getValue()*/);
		if (pInfo != null) {
			pInfo.addGetOrSetMethod(kind, paramsIndexs, valueIndex, info);
		} else {
			indexers.put(key /* name.getValue()*/, new IndexerInfo(kind, paramsIndexs,
					valueIndex, info));
		}
	}

	public Map<String, IndexerInfo> getIndexers() {
		final Map<String, IndexerInfo> allIndexes = new HashMap<String, IndexerInfo>();
		for (final ClassInfo parent : parents) {
			allIndexes.putAll(parent.getIndexers());
		}
		allIndexes.putAll(indexers);
		// check jdk and framework compatibility ?
		return allIndexes;
	}

	/**
	 * Add a list of propertyinfo computed
	 * 
	 * @param properties2
	 */
	public void addIndexers(List<IndexerInfo> indexers) {
	}

	//
	// CovariantMethod
	//

	public void setCovariantMethod(boolean b) {
		hasCovariantMethod.setValue(b);
	}

	public boolean hasCovariantMethod() {
		return hasCovariantMethod.getValue();
	}

	//
	// cloneContentFor
	//

	public ClassInfo cloneContentFor(IType otherType, List<String> fieldsName)
			throws JavaModelException {
		final ClassInfoImpl ci = new ClassInfoImpl(mappingInfo, otherType, generics
				.getValue(), packageInfo);
		for (final String fName : fieldsName) {
			final IField f = clazz.getField(fName);
			final FieldInfo fieldI = fields.get(fName);
			if (fieldI != null)
				ci.addField(getSignature(f), (FieldInfo) fieldI.clone());
		}
		//ci.setPartial(isPartial.getValue());
		// TODO: ci.setExcluded(isExcluded.getValue());
		//ci.setRemoved(isRemoved.getValue());
		//ci.setRemoveGenerics(isRemoveGenerics());
		String targetFramework = getMappingInfo().getTranslateInfo().getConfiguration().getOptions().getTargetDotNetFramework().name();
		
		ci.addTargetClass(targetFramework, getTarget(targetFramework));
		return ci;
	}

	public ClassInfo cloneContentFor(IType otherType, IMember[] methodsToClone)
			throws JavaModelException {
		ClassInfoImpl ci = new ClassInfoImpl(mappingInfo, otherType, generics.getValue(),
				packageInfo);
		for (IMember method : methodsToClone) {
			if (method instanceof IMethod) {
				IMethod m = (IMethod) method;
				MethodInfo methodI = this.methods.get(m.getElementName());
				if (methodI != null) {

					// Clone method info
					MethodInfo newMethodInfo = new MethodInfo(this.mappingInfo, m);
					newMethodInfo.setClassInfo(ci);

					// Clone target method
					String targetFramework = getPackageInfo().getMappingInfo().getTranslateInfo().
					getConfiguration().getOptions().getTargetDotNetFramework().name();
					TargetMethod newTargetMethod = methodI.getTarget(targetFramework);
					newMethodInfo.addTargetMethod(targetFramework, newTargetMethod
							.cloneForChild());

					ci.methods.put(this.getSignature(null, m, false),
							newMethodInfo);
					// NOT Clone, but same method info
					/*
					 * methodI.setClassInfo( ci ); ci.methods.put(
					 * this.getSignature(null, m, false), methodI );
					 */
				}
			}
		}
		//ci.setPartial(false);
		//ci.setExcluded(false);
		//ci.setRemoved(false);
		//ci.setRemoveGenerics(isRemoveGenerics());
		return ci;
	}

	//
	// implicitFieldRename
	//

	public void implicitFieldRename(IField ifield, String newName)
			throws JavaModelException {
		final FieldInfo fi = fields.get(getSignature(ifield));
		if (fi != null) {
			final FieldInfo nfi = new FieldInfo(mappingInfo, newName);
			String targetFramework = getMappingInfo().getTranslateInfo().getConfiguration().getOptions().getTargetDotNetFramework().name();
			
			nfi.addTargetField(targetFramework, fi.getTarget(targetFramework));
			if (nfi.getTarget(targetFramework) != null) {
				nfi.getTarget(targetFramework).setName(newName);
				if (nfi.getTarget(targetFramework).getRewriter() != null
						&& nfi.getTarget(targetFramework).getRewriter() instanceof FieldRewriter) {
					((FieldRewriter) nfi.getTarget(targetFramework).getRewriter())
							.setName(newName);
				}
			}
			fields.put(newName, nfi);
		}
	}

	//
	// package info
	//

	public PackageInfo getPackageInfo() {
		return packageInfo;
	}

	public void setPackageInfo(PackageInfo info) {
		packageInfo = info;
	}

	//
	// toFile
	//

	public String toFile() {
		final StringBuilder res = new StringBuilder();
		res.append("   class ");
		printClassName(res);
		printTargetClass(res);

		res.append("{\n");
		if (!generics.isDefaultValue())
			res.append("      generics=true;\n");
		//if (isPartial.getValue())
		//	res.append("      partial=true;\n");
		//if (isRemoved.getValue())
		//	res.append("      removed=true;\n");
		if (hasCovariantMethod.getValue())
			res.append("      covariant=true;\n");
		//if (removeGenerics.getValue())
		//	res.append("      removeGenerics=true;\n");
		res.append("\n");
		printMethods(res);
		printFields(res);
		printProperties(res);
		printIndexers(res);
		res.append("}\n");
		return null;
	}

	private void printIndexers(StringBuilder res) {
		for (final IndexerInfo mi : indexers.values()) {
			res.append(mi.toFile() + "\n");
		}
	}

	private void printProperties(StringBuilder res) {
		for (final PropertyInfo pi : properties.values()) {
			res.append(pi.toFile() + "\n");
		}
	}

	private void printFields(StringBuilder res) {
		for (final FieldInfo fi : fields.values()) {
			res.append(fi.toFile() + "\n");
		}
	}

	private void printMethods(StringBuilder res) {
		for (final MethodInfo mi : methods.values()) {
			res.append(mi.toFile() + "\n");
		}
	}

	private void printTargetClass(StringBuilder res) {
		if (targetClasses != null && targetClasses.size() > 0) {
			for(TargetClass tc : targetClasses.values()) {
				res.append(" :: ");
				res.append(tc.getPackageName() + ":");
				res.append(tc.getName());
			}
		}
	}

	private void printClassName(StringBuilder res) {
		if (packageName != null)
			res.append(packageName);
		res.append(name);
	}

	//
	// toXML
	//
	//  <class packageName="..." name="..." 
	//         isExcluded="true|false" 
	//         generics="true|false" 
	//         typeParameters="..."
	//         hasCovariantMethod="true|false">
    //   <target ... />
	//   <method ... />
	//   <constructor ... />
	//   <field ... />
	// </class>
	public void toXML(StringBuilder res, String tabValue) {
		res.append(Constants.TWOTAB + "<!--                    -->\n");
		res.append(Constants.TWOTAB + "<!-- class " + Utils.xmlify(name.getValue())
				+ " -->\n");
		res.append(Constants.TWOTAB + "<!--                    -->\n");
		res.append(Constants.TWOTAB + "<class ");

		// no more needed : toXML(res, packageName, "", " ", "");

		res.append("name=\"" + Utils.removeGenerics(simpleName) + "\"");
		if (Utils.hasGenerics(simpleName)) {
			res.append("\n" + Constants.TWOTAB + "       typeParameters=\""
					+ Utils.getGenerics(simpleName) + "\"");
		}

		// TODO : toXML(res, isExcluded, "\n" + TWOTAB + "       ", "", "");
		toXML(res, generics, "\n" + Constants.TWOTAB + "       ", "", "");
		toXML(res, hasCovariantMethod, "\n" + Constants.TWOTAB + "       ", "", "");
		toXML(res, sinceJDK, "\n" + Constants.TWOTAB + "       ", "", "");

		res.append(">\n");

		if (targetClasses != null && targetClasses.size() > 0) {
			for(TargetClass tc : targetClasses.values()) 
				tc.toXML(res, tabValue);
		}

		for (final MethodInfo mi : methods.values()) {
			mi.toXML(res, tabValue);
			res.append("\n");
		}
		for (final FieldInfo fi : fields.values()) {
			fi.toXML(res, tabValue);
			res.append("\n");
		}
		/*for (final PropertyInfo pi : properties.values()) {
			pi.toXML(res, tabValue);
			res.append("\n");
		}*/
		for (final IndexerInfo ii : indexers.values()) {
			ii.toXML(res, tabValue);
			res.append("\n");
		}
		res.append(Constants.TWOTAB + "</class>\n");
	}

	//
	// fromXML
	//
	//  <class packageName="..." name="..." 
	//         isExcluded="true|false" 
	//         generics="true|false" 
	//         typeParameters="..."
	//         hasCovariantMethod="true|false">
    //   <target ... />
	//   <method ... />
	//   <constructor ... />
	//   <field ... />
	// </class>
	public void fromXML(Element pack) {
		NodeList child = pack.getChildNodes();
		//
		// TODO : isExcluded.fromXML(pack);
		generics.fromXML(pack);
		mappingOverridePolicy.fromXML(pack);
		hasCovariantMethod.fromXML(pack);
		sinceJDK.fromXML(pack);
		//
		for (int i = 0; i < child.getLength(); i++) {
			Node pckNode = child.item(i);

			if (pckNode.getNodeType() == Node.ELEMENT_NODE) {
				Element pckElement = (Element) pckNode;
				String elemName = pckElement.getNodeName();
				if (elemName.equals("target")) {
					String targetFramework = pckElement.getAttribute("dotnetFramework");
					if (targetFramework == null || targetFramework.isEmpty())
						targetFramework = DotNetFramework.NET3_5.name();
					TargetClass targetClass = new TargetClass(isMember);
					targetClass.fromXML(pckElement);
					targetClass.setSourcePackageName(getPackageName());
					if (targetClass.getName() != null && targetClass.getPackageNameOption().isDefaultValue() && 
							packageInfo.getTarget(targetFramework) != null && 
							packageInfo.getTarget(targetFramework).getName() != null) {
						targetClass.getPackageNameOption().setValue(packageInfo.getTarget(targetFramework).getName());
					}
					targetClasses.put(targetFramework, targetClass);
				} else if (elemName.equals("method")) {
					String mName = pckElement.getAttribute("name");
					String signature = pckElement.getAttribute("signature");
					try {
						String[] classes = parseArguments(signature);					
						MethodInfo mInfo = resolveMethod(mName, classes);
						mInfo.fromXML(pckElement);
					} catch (Exception e) {
						mappingInfo.getTranslateInfo().getConfiguration().getLogger().logError(
								"Can't find method " + mName + signature, e);					
					}
				} else if (elemName.equals("constructor")) {
					String mName = ClassInfoImpl.CONSTRUCTOR_NAME;
					String signature = pckElement.getAttribute("signature");
					try {
						String[] classes = parseArguments(signature);					
						MethodInfo mInfo = resolveMethod(mName, classes);
						mInfo.fromXML(pckElement);
					} catch (Exception e) {
						mappingInfo.getTranslateInfo().getConfiguration().getLogger().logError(
								"Can't find constructor " + mName + signature, e);		
					}
				} else if (elemName.equals("field")) {
					String filedName = pckElement.getAttribute("name");
					try {
						final FieldInfo fInfo = resolveField(filedName);
						fInfo.fromXML(pckElement);
					} catch (Exception e) {
						mappingInfo.getTranslateInfo().getConfiguration().getLogger().logError(
								"Can't find field " + filedName, e);		
					}
				}
			}
		}

	}

	private String[] parseArguments(String parameters)
			throws JavaModelException {
		char[] chars = parameters.toCharArray();
		int pos = 0;
		if (chars[pos] != '(') {
			// printError("expecting a '('");
			return null;
		}
		pos++;
		final List<String> args = new ArrayList<String>();
		StringBuffer buffer = new StringBuffer();
		boolean inGenerics = false;
		while (pos < parameters.length() - 1 && chars[pos] != ')') {
			switch (chars[pos]) {
			case '.':
				buffer.append(".");
				break;
			case ',':
				if (!inGenerics) {
					final String type = TranslateInfo
							.resolve(buffer.toString());
					args.add(type);
					buffer = new StringBuffer();
				} else {
					buffer.append(chars[pos]);
				}
				break;
			default: {
				if (chars[pos] == '<') {
					inGenerics = true;
					buffer.append(chars[pos]);
					break;
				} else if (chars[pos] == '>') {
					inGenerics = false;
					buffer.append(chars[pos]);
					break;
				} else {									
					buffer.append(chars[pos]);

					if (((pos > 1) && parameters
							.substring(pos - 1, pos).equals("?"))
							|| ((pos > 7) && parameters
									.substring(pos - 7, pos).equals("extends"))
							|| ((pos > 5) && parameters
									.substring(pos - 5, pos).equals("super"))) {
						buffer.append(" ");
					}
				}
			}
			}
			pos++;
		}
		if (!buffer.toString().equals("")) {
			final String type = TranslateInfo.resolve(buffer.toString());
			args.add(type);
		}

		if (chars[pos] != ')') {
			// printError("expecting a ')'");
			return null;
		}

		return args.toArray(new String[args.size()]);
	}

	//
	// getMappingOverringPolicy
	//
	
	public MappingOverridingPolicy getMappingOverringPolicy() {		
		return mappingOverridePolicy.getValue();
	}

}
