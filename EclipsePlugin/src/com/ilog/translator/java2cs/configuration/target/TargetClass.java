package com.ilog.translator.java2cs.configuration.target;

import org.w3c.dom.Element;
import org.w3c.dom.Node;
import org.w3c.dom.NodeList;

import com.ilog.translator.java2cs.configuration.ChangeHierarchyDescriptor;
import com.ilog.translator.java2cs.configuration.ChangeModifierDescriptor;
import com.ilog.translator.java2cs.configuration.ChangeUsingDescriptor;
import com.ilog.translator.java2cs.configuration.DotNetModifier;
import com.ilog.translator.java2cs.configuration.info.Constants;
import com.ilog.translator.java2cs.configuration.info.ElementInfo;
import com.ilog.translator.java2cs.configuration.options.BooleanOptionBuilder;
import com.ilog.translator.java2cs.configuration.options.BooleanOptionEditor;
import com.ilog.translator.java2cs.configuration.options.OptionImpl;
import com.ilog.translator.java2cs.configuration.options.StringOptionBuilder;
import com.ilog.translator.java2cs.configuration.options.StringOptionEditor;
import com.ilog.translator.java2cs.configuration.options.OptionImpl.XMLKind;
import com.ilog.translator.java2cs.translation.noderewriter.ElementRewriter;
import com.ilog.translator.java2cs.translation.noderewriter.INodeRewriter;
import com.ilog.translator.java2cs.translation.noderewriter.MethodRewriter;
import com.ilog.translator.java2cs.translation.noderewriter.PrimitiveTypeRewriter;
import com.ilog.translator.java2cs.translation.noderewriter.TypeRewriter;
import com.ilog.translator.java2cs.util.CSharpModelUtil;
import com.ilog.translator.java2cs.util.Utils;

/**
 * 
 * @author afau
 * 
 *         Represents a 'target class' i.e. the description (package & name) of
 *         a C# classes.
 * 
 *         options : - memberMappingBehavior - instanceOfTypeName - partial -
 *         nullable - nestedToInner - removeGenerics - isExcluded - processDoc -
 *         removeUsing - addUsing
 */
public class TargetClass extends TargetImportableElement implements Cloneable {

	//
	//
	//
	public static final TargetClass SBYTE = new TargetClass(
			CSharpModelUtil.SBYTE_K);
	public static final TargetClass BOOL = new TargetClass(
			CSharpModelUtil.BOOL_K);

	//
	// Options
	//
	private OptionImpl<String> instanceOfTypeName = new OptionImpl<String>(
			"instanceOfTypeName", null, null, OptionImpl.Status.PRODUCTION,
			new StringOptionBuilder(), new StringOptionEditor(),
			XMLKind.ATTRIBUT, "What Type use in case of instanceof");
	private OptionImpl<Boolean> nullable = new OptionImpl<Boolean>("nullable",
			null, false, OptionImpl.Status.PRODUCTION,
			new BooleanOptionBuilder(), new BooleanOptionEditor(),
			XMLKind.ATTRIBUT, "Indicate that the class in nullable");
	private OptionImpl<Boolean> nestedToInner = new OptionImpl<Boolean>(
			"nestedToInner", null, false, OptionImpl.Status.PRODUCTION,
			new BooleanOptionBuilder(), new BooleanOptionEditor(),
			XMLKind.ATTRIBUT, "Indicate that the class move from nested to inner");
	private OptionImpl<Boolean> removeGenerics = new OptionImpl<Boolean>(
			"removeGenerics", null, false, OptionImpl.Status.PRODUCTION,
			new BooleanOptionBuilder(), new BooleanOptionEditor(),
			XMLKind.ATTRIBUT, "Erase generics");
	private OptionImpl<Boolean> processDoc = new OptionImpl<Boolean>(
			"processDoc", null, true, OptionImpl.Status.PRODUCTION,
			new BooleanOptionBuilder(), new BooleanOptionEditor(),
			XMLKind.ATTRIBUT, "Process the javadoc");
	private OptionImpl<Boolean> isPartial = new OptionImpl<Boolean>(
			"isPartial", null, false, OptionImpl.Status.PRODUCTION,
			new BooleanOptionBuilder(), new BooleanOptionEditor(),
			XMLKind.ATTRIBUT, "indicate that the C# class must be partial");
	private OptionImpl<Boolean> removeStaticInitializers = new OptionImpl<Boolean>(
			"removeStaticInitializers", null, false,
			OptionImpl.Status.PRODUCTION, new BooleanOptionBuilder(),
			new BooleanOptionEditor(), XMLKind.ATTRIBUT, "remove the static initializers");
	private OptionImpl<Boolean> requireFQN = new OptionImpl<Boolean>(
			"requireFQN", null, false,
			OptionImpl.Status.PRODUCTION, new BooleanOptionBuilder(),
			new BooleanOptionEditor(), XMLKind.ATTRIBUT, "Require fully qualified name");
	protected OptionImpl<String> codeToAddToImplementation = new OptionImpl<String>(
			"codeToAddToImplementation", null, null, OptionImpl.Status.BETA,
			new StringOptionBuilder(), new StringOptionEditor(), XMLKind.CDATA,
			"code to add to implementation of a given interface");
	private OptionImpl<Boolean> autoProperty = new OptionImpl<Boolean>(
			"autoProperty", null, false, OptionImpl.Status.PRODUCTION,
			new BooleanOptionBuilder(), new BooleanOptionEditor(),
			XMLKind.ATTRIBUT, "Indicate that get/set/is methods will be automatically convert into property for this class");
	
	private ChangeUsingDescriptor changeUsingDescriptor = new ChangeUsingDescriptor();
	private ChangeHierarchyDescriptor changeHierarchyDescriptor = new ChangeHierarchyDescriptor();
	private String shortname;
	private boolean isMember = false;
	
	//
	// Constructor
	//

	/**
	 * 
	 */
	public TargetClass(boolean isMember) {
		this.isMember = isMember;
	}

	/**
	 * 
	 * @param primitiveName
	 */
	protected TargetClass(String primitiveName) {
		shortname = TargetClass.shortClassName(primitiveName);		
		typeParameters.setValue(Utils.getGenerics(shortname));
		setRewriter(new PrimitiveTypeRewriter(primitiveName));
	}

	/**
	 * Create a new target field with given name.
	 * 
	 * @param packageName
	 *            The package name of this
	 * @param name
	 *            The name of this target field.
	 */
	public TargetClass(String packageName, String name,
			ChangeModifierDescriptor mod, boolean partial, boolean isRemoved,
			boolean nullable, boolean isMember) {
		super(packageName, name);
		changeModifiersDescriptor = mod;
		setPartial(this.isPartial.getValue());
		this.translated.setValue(isRemoved);
		this.nullable.setValue(nullable);
		//
		if (name != null) {
			shortname = TargetClass.shortClassName(name);
			typeParameters.setValue(Utils.getGenerics(shortname));
		}
		this.isMember = isMember;
		setRewriter(new TypeRewriter(packageName, name, null, mod, partial,
				this.translated.getValue(), nullable, isMember));
	}

	//
	// name
	//
	
	/**
	 * @return Returns the name.
	 */
	public String getName() {
		if (!typeParameters.isDefaultValue()) {
			return super.getName() + "<" + typeParameters.getValue() + ">";
		} else
			return super.getName();
	}

	//
	// Nullable
	//

	public boolean isNullable() {
		return nullable.getValue();
	}

	public void setNullable(boolean nullable) {
		this.nullable.setValue(nullable);
	}
	
	//
	// RequireFQN
	//

	public boolean isRequireFQN() {
		return requireFQN.getValue();
	}

	public void setRequireFQN(boolean nullable) {
		this.requireFQN.setValue(nullable);
	}

	//
	// RemoveStaticInitializers
	//

	public void setRemoveStaticInitializers(boolean b) {
		removeStaticInitializers.setValue(b);
	}

	public boolean isRemoveStaticInitializers() {
		return removeStaticInitializers.getValue();
	}

	//
	// short name
	//

	/**
	 * @param name
	 *            The name to set.
	 */
	@Override
	public void setName(String name) {
		this.name.setValue(name);
		shortname = TargetClass.shortClassName(name);
	}

	/**
	 * @return Returns the shortname.
	 */
	public String getShortName() {
		return shortname;
	}

	public static final String shortClassName(String classname) {
		final int index = classname.lastIndexOf('.');
		if (index == -1) {
			return classname;
		} else {
			return classname.substring(index + 1);
		}
	}

	//
	// Rewriter
	//

	@Override
	public INodeRewriter getRewriter() {
		if (rewriter == null) {
			rewriter = new TypeRewriter(packageName.getValue(),
					name.getValue(), typeParameters.getValue(),
					changeModifiersDescriptor, isPartial.getValue(), translated
							.getValue(), nullable.getValue(), isMember);
			((TypeRewriter) rewriter)
					.setNestedToInner(nestedToInner.getValue());
		} else {
			if (rewriter instanceof TypeRewriter) {
				((TypeRewriter) rewriter).setName(name.getValue());
				((TypeRewriter) rewriter).setMember(isMember);
				((TypeRewriter) rewriter)
						.setPackageName(packageName.getValue());
				((TypeRewriter) rewriter).setTypeParameter(typeParameters
						.getValue());
				((TypeRewriter) rewriter).setPartial(isPartial.getValue());
				((TypeRewriter) rewriter).setNullable(nullable.getValue());
				((TypeRewriter) rewriter).setNestedToInner(nestedToInner
						.getValue());
				((TypeRewriter) rewriter).setExcluded(translated.getValue());
				((TypeRewriter) rewriter)
						.setInstanceOfTypeName(instanceOfTypeName.getValue());
				((TypeRewriter) rewriter)
				.setRequireFQN(requireFQN.getValue());
			}
			if (isPartial.getValue() && changeModifiersDescriptor == null)
				changeModifiersDescriptor = new ChangeModifierDescriptor();
			rewriter.setChangeModifier(changeModifiersDescriptor);
			rewriter.setRemove(translated.getValue());
		}
		if (rewriter instanceof ElementRewriter)
			((ElementRewriter) rewriter).setTypeArguments(typeArgs);
		return rewriter;
	}

	//
	// toString
	//

	@Override
	public String toString() {
		String descr = "";
		descr += packageName.getValue() + ":" + name.getValue() + " ["
				+ shortname + "]";
		return descr;
	}

	//
	// partial
	//

	public void setPartial(boolean b) {
		isPartial.setValue(b);
		if ((rewriter != null) && (rewriter instanceof TypeRewriter)
				&& isPartial.getValue()) {
			((TypeRewriter) rewriter).setPartial(true);
		}
		if (b) {
			if (changeModifiersDescriptor == null) {
				changeModifiersDescriptor = new ChangeModifierDescriptor();
			}
			changeModifiersDescriptor.add(DotNetModifier.PARTIAL);
			changeModifiersDescriptor.remove(DotNetModifier.FINAL);
		}
	}

	//
	// clone
	//

	@Override
	public Object clone() {
		final TargetClass tClass = new TargetClass(isMember);
		tClass.setRewriter(rewriter.clone());
		return tClass;
	}

	//
	// NestedToInner
	//

	public void setNestedToInner(boolean b) {
		nestedToInner.setValue(b);
	}

	public boolean getNestedToInner() {
		return nestedToInner.getValue();
	}
	
	//
	// RemoveGenerics
	//

	public void setRemoveGenerics(boolean removeGenerics) {
		this.removeGenerics.setValue(removeGenerics);
	}

	public boolean isRemoveGenerics() {
		return removeGenerics.getValue();
	}

	//
	// InstanceOfTypeName
	//

	public void setInstanceOfTypeName(String instanceOfTypeName) {
		this.instanceOfTypeName.setValue(instanceOfTypeName);
	}

	//
	// ProcessDoc
	//

	public boolean isProcessDoc() {
		return processDoc.getValue();
	}

	public void setProcessDoc(boolean processDoc) {
		this.processDoc.setValue(processDoc);
	}

	//
	// CodeToAddToImplementation
	//

	public String getCodeToAddToImplementation() {
		return codeToAddToImplementation.getValue();
	}

	public void setCodeToAddToImplementation(String codeReplacement) {
		this.codeToAddToImplementation.setValue(codeReplacement);
		if (this.rewriter instanceof MethodRewriter) {
			MethodRewriter mt = (MethodRewriter) this.rewriter;
			mt.setReplacementCode(codeReplacement);
		}
	}
	
	//
	// options
	//

	public OptionImpl<Boolean> getPartialOption() {
		return isPartial;
	}

	public OptionImpl<Boolean> getRemoveGenericsOption() {
		return removeGenerics;
	}

	public OptionImpl<Boolean> getProcessDocOption() {
		return processDoc;
	}

	public OptionImpl<Boolean> getNestedToInnerOption() {
		return nestedToInner;
	}

	//
	// AutoProperty
	//
	
	public boolean isAutoProperty() {
		return autoProperty.getValue();
	}
	
	//
	// serialization
	//

	// <target name="..." packageName="..." typeParameters="..."
	// renamed="true|false"
	// isRemoved="true|false"
	// constraints="..."
	// nullable="true|false"
	// memberMappingBehavior="..."
	// instanceOfTypeName="..."
	// isPartial="true|false"
	// removeGenerics="true|false"
	// removeStaticInitializers="true|false"
	// isExcluded="true|false" ??
	// processDoc="true|false"/>
	// <modifiers ... />
	// <imports ... />
	// <hierarchy ... />
	// <comments>
	// <CDATA>...</CDATA>
	// </comments>
	// </target>
	public void toXML(StringBuilder res, String tabValue) {
		if (needTargetPart()) {
			res.append(Constants.THREETAB + "<target ");
			xmlAttributeInTargetPart(res);
			//
			res.append(">\n");
			xmlElementsInTargetPart(res);

			res.append(Constants.THREETAB + "</target>\n");
		}
	}

	public void xmlAttributeInTargetPart(StringBuilder res) {
		ElementInfo.toXML(res, getPackageNameOption(), "", " ", "");
		if (getName() != null) {				
			// shortName
			res.append("name=\"" + Utils.removeGenerics(getShortName())
					+ "\" ");
		}
		ElementInfo.toXML(res, typeParameters, "\n" + Constants.THREETAB
				+ "        ", "", "");
		ElementInfo.toXML(res, memberMappingBehavior, "", " ", "");
		ElementInfo.toXML(res, nullable, "", " ", "");
		ElementInfo.toXML(res, autoProperty, "", " ", "");
		ElementInfo.toXML(res, processDoc, "", " ", "");
		ElementInfo.toXML(res, isPartial, "", " ", "");
		ElementInfo.toXML(res, instanceOfTypeName, "", " ", "");
		ElementInfo.toXML(res, removeGenerics, "", " ", "");
		ElementInfo.toXML(res, removeStaticInitializers, "", " ", "");
		ElementInfo.toXML(res, renamed, "", " ", "");
		ElementInfo.toXML(res, translated, "", " ", "");
		ElementInfo.toXML(res, constraints, "", " ", "");
		ElementInfo.toXML(res, comments, "", " ", "");
		ElementInfo.toXML(res, removeFromImport, "", "", "");
		ElementInfo.toXML(res, dotnetFramework, "", "", "");
		ElementInfo.toXML(res, requireFQN, "", "", "");
	}

	public void xmlElementsInTargetPart(StringBuilder res) {
		ElementInfo.toXML(res, codeToAddToImplementation, "", "",
				Constants.FOURTAB);
		ElementInfo.toXML(res, changeModifiersDescriptor, Constants.FOURTAB);
		ElementInfo.toXML(res, changeHierarchyDescriptor, Constants.FOURTAB);
		ElementInfo.toXML(res, changeUsingDescriptor, Constants.FOURTAB);
	}
		
		boolean needTargetPart() {
			return getName() != null || ( getName() != null && !getName().isEmpty()) || !typeParameters.isDefaultValue() || !nullable.isDefaultValue() || 
					!processDoc.isDefaultValue() || !autoProperty.isDefaultValue() ||
					!isPartial.isDefaultValue() || !instanceOfTypeName.isDefaultValue() ||
					!removeGenerics.isDefaultValue() ||
					!removeStaticInitializers.isDefaultValue() || !renamed.isDefaultValue() || 
					!translated.isDefaultValue() || !constraints.isDefaultValue() ||
					!comments.isDefaultValue() || changeModifiersDescriptor != null||
					changeHierarchyDescriptor != null || changeUsingDescriptor != null;
		}

	public void fromXML(Element pckElement) {
		getNameOption().fromXML(pckElement);
		getTypeParametersOption().fromXML(pckElement);
		//
		if (getNameOption().getValue() != null) {
			shortname = TargetClass.shortClassName(getNameOption().getValue());
			setRewriter(new TypeRewriter(packageName.getValue(), name
					.getValue(), typeParameters.getValue(),
					changeModifiersDescriptor, isPartial.getValue(), translated
							.getValue(), nullable.getValue(), isMember));
			//
		}
		//
		getPackageNameOption().fromXML(pckElement);
		renamed.fromXML(pckElement);
		translated.fromXML(pckElement);
		constraints.fromXML(pckElement);
		comments.fromXML(pckElement);
		instanceOfTypeName.fromXML(pckElement);
		autoProperty.fromXML(pckElement);
		nullable.fromXML(pckElement);
		memberMappingBehavior.fromXML(pckElement);
		isPartial.fromXML(pckElement);
		removeGenerics.fromXML(pckElement);
		removeStaticInitializers.fromXML(pckElement);
		//isExcluded.fromXML(pckElement);
		processDoc.fromXML(pckElement);
		codeToAddToImplementation.fromXML(pckElement);
		nestedToInner.fromXML(pckElement);
		removeFromImport.fromXML(pckElement);
		dotnetFramework.fromXML(pckElement);
		requireFQN.fromXML(pckElement);
		//
		NodeList lNodes = pckElement.getChildNodes();
		if (lNodes != null) {
			for (int i = 0; i < lNodes.getLength(); i++) {
				Node child = lNodes.item(i);
				if (child.getNodeType() == Node.ELEMENT_NODE) {
					if (child.getNodeName().equals("modifiers")) {
						if (changeModifiersDescriptor == null)
							changeModifiersDescriptor = new ChangeModifierDescriptor();
						changeModifiersDescriptor.fromXML((Element) child);
					}
					if (child.getNodeName().equals("imports"))
						changeUsingDescriptor.fromXML((Element) child);
					if (child.getNodeName().equals("hierarchy"))
						changeHierarchyDescriptor.fromXML((Element) child);
				}
			}
		}
	}

	//
	// ChangeUsingDescriptor
	//
	
	public ChangeUsingDescriptor getChangeUsingDescriptor() {
		return changeUsingDescriptor;
	}

	//
	// ChangeHierarchyDescriptor
	//
	
	public void setChangeHierarchyDescriptor(
			ChangeHierarchyDescriptor changeHierarchyDescriptor2) {
		this.changeHierarchyDescriptor = changeHierarchyDescriptor2;
	}

	public ChangeHierarchyDescriptor getChangeHierarchyDescriptor() {
		return changeHierarchyDescriptor;
	}
};
