package com.ilog.translator.java2cs.configuration.target;

import org.w3c.dom.Element;
import org.w3c.dom.Node;
import org.w3c.dom.NodeList;

import com.ilog.translator.java2cs.configuration.ChangeModifierDescriptor;
import com.ilog.translator.java2cs.configuration.info.Constants;
import com.ilog.translator.java2cs.configuration.info.ElementInfo;
import com.ilog.translator.java2cs.configuration.options.BooleanOptionBuilder;
import com.ilog.translator.java2cs.configuration.options.BooleanOptionEditor;
import com.ilog.translator.java2cs.configuration.options.OptionImpl;
import com.ilog.translator.java2cs.configuration.options.StringOptionBuilder;
import com.ilog.translator.java2cs.configuration.options.StringOptionEditor;
import com.ilog.translator.java2cs.configuration.options.OptionImpl.XMLKind;
import com.ilog.translator.java2cs.translation.noderewriter.INodeRewriter;
import com.ilog.translator.java2cs.translation.noderewriter.MethodRewriter;

/**
 * 
 * @author afau
 * 
 *         Represents a 'target method' i.e. the description (package & name) of
 *         a method in a C# classes.
 * 
 *         options: - format - isOverride - covariant - genericsTest -
 *         codeReplacement
 * 
 */
public class TargetMethod extends TargetMemberElement {

	public static int CONSTRUCTOR = 1;
	public static int INDEXER = 2;
	public static int FORMAT = 3;

	//
	// Options
	//

	protected OptionImpl<Boolean> isOverride = new OptionImpl<Boolean>(
			"isOverride", null, false, OptionImpl.Status.PRODUCTION,
			new BooleanOptionBuilder(), new BooleanOptionEditor(),
			XMLKind.ATTRIBUT, "is that method override a parent method");
	protected OptionImpl<String> covariant = new OptionImpl<String>(
			"covariant", null, null, OptionImpl.Status.PRODUCTION,
			new StringOptionBuilder(), new StringOptionEditor(),
			XMLKind.ATTRIBUT, "return type for covariant method");
	protected OptionImpl<String> genericsTest = new OptionImpl<String>(
			"genericsIf", null, null, OptionImpl.Status.PRODUCTION,
			new StringOptionBuilder(), new StringOptionEditor(),
			XMLKind.ATTRIBUT,
			"indicate how to 'guess' that a method is generic");
	protected OptionImpl<String> codeReplacement = new OptionImpl<String>(
			"codeReplacement", null, null, OptionImpl.Status.PRODUCTION,
			new StringOptionBuilder(), new StringOptionEditor(), XMLKind.CDATA,
			"code replacement for method body");
	protected OptionImpl<String> parametersForGenericMethods = new OptionImpl<String>(
			"parametersForGenericMethods", null, null, OptionImpl.Status.BETA,
			new StringOptionBuilder(), new StringOptionEditor(),
			XMLKind.ATTRIBUT, "name of this new type for this field");
	protected OptionImpl<String> patternForSuperInvocation = new OptionImpl<String>(
			"formatForSuperInvocation", null, null, 
			OptionImpl.Status.PRODUCTION, new StringOptionBuilder(), 
			new StringOptionEditor(), XMLKind.CDATA, "format to use for super method invocation");
	
	//
	// constructors
	//	

	public TargetMethod() {
		this.rewriter = new MethodRewriter();
	}

	/**
	 * 
	 * @param name
	 * @param rewrite
	 */
	public TargetMethod(String name, int[] rewrite) {
		// method
		super(null, name);
		setRewriter(new MethodRewriter(name, rewrite));
	}

	
	//
	// Rewriter
	//

	@Override
	public INodeRewriter getRewriter() {
		final INodeRewriter nr = super.getRewriter();
		if (nr instanceof MethodRewriter) {
			final MethodRewriter rew = (MethodRewriter) nr;
			rew.setName(name.getValue());
			if (covariant.getValue() != null)
				rew.setCovariantType(covariant.getValue());
			rew.setGenericsTest(genericsTest.getValue());
			rew.setChangeModifier(changeModifiersDescriptor);
			rew.setReplacementCode(codeReplacement.getValue());
			rew.setFormat(pattern.getValue());
			rew.setFormatForSuperInvocation(patternForSuperInvocation.getValue());
			if (!parametersForGenericMethods.isDefaultValue())
				rew.setParametersForGenericMethods(parametersForGenericMethods
						.getValue());
			if (!type.isDefaultValue())
				rew.setReturnType(type.getValue());
		}
		return nr;
	}

	//
	// PatternForSuperInvocation
	//

	public String getPatternForSuperInvocation() {
		return patternForSuperInvocation.getValue();
	}
	
	public OptionImpl<String> getPatternForSuperInvocationOption() {
		return patternForSuperInvocation;
	}
	
	//
	// toString
	//

	@Override
	public String toString() {
		final String descr = ((packageName.isDefaultValue()) ? "" : packageName
				.getValue()
				+ ".")
				+ ((pattern.isDefaultValue()) ? name.getValue() : pattern
						.getValue());
		return descr;
	}

	//
	// Override
	//

	public void setOverride(boolean b) {
		isOverride.setValue(b);
	}

	//
	// ParametersForGenericMethods
	//

	public void setParametersForGenericMethods(
			String parametersForGenericMethods) {
		this.parametersForGenericMethods.setValue(parametersForGenericMethods);
	}

	//
	// cloneForChild
	//

	public TargetMethod cloneForChild() {
		INodeRewriter rewriter = getRewriter();
		final TargetMethod newTM = new TargetMethod();
		// TODO: Copy modifier lead to un-compilable code
		// constructor for example.
		if (rewriter instanceof MethodRewriter) {
			rewriter = getRewriter().clone();
			// ((MethodRewriter)rewriter).filterChangeModifier();
		}
		newTM.setRewriter(rewriter);
		newTM.setName(name.getValue());
		// unused : newTM.setExpression(this.expression);
		newTM.setOverride(isOverride.getValue());
		newTM.setTranslated(isTranslated());
		newTM.setRenamed(isRenamed());
		newTM.pattern.setValue(pattern.getValue());
		newTM.covariant.setValue(covariant.getValue());
		// TODO newTM.setGenericsTest(genericsTest);
		return newTM;
	}

	//
	// MappedToAProperty
	//

	public boolean isMappedToAProperty() {
		return false;
	}

	//
	// MappedToAnIndexer
	//

	public boolean isMappedToAnIndexer() {
		return false;
	}

	//
	// GenericsTest
	//

	public void setGenericsTest(String genericsTest) {
		this.genericsTest.setValue(genericsTest);
	}

	public String getGenericsTest() {
		return genericsTest.getValue();
	}

	//
	// CodeReplacement
	//

	private String getCodeReplacement() {
		return codeReplacement.getValue();
	}

	public void setCodeReplacement(String codeReplacement) {
		this.codeReplacement.setValue(codeReplacement);
		if (this.rewriter instanceof MethodRewriter) {
			MethodRewriter mt = (MethodRewriter) this.rewriter;
			mt.setReplacementCode(codeReplacement);
		}
	}

	//
	// Covariant
	//

	public OptionImpl<String> getCovariantOption() {
		return covariant;
	}
	
	public void setCovariant(String type) {
		covariant.setValue(type);
	}	

	//
	// Serialization
	//

	// <target
	// isRemoved=""
	// renamed=""
	// covariant="..."
	// parametersForGenericMethods=".."
	// genericsTest="..."/>
	// name="...">
	// <format>
	// <CDATA>...</CDATA>
	// </format>
	// <comments>
	// <CDATA>...</CDATA>
	// </comments>
	// <codeReplacement>
	// <CDATA>...</CDATA>
	// </codeReplacement>
	// <modifiers ... />
	// </target>
	public void toXML(StringBuilder res, String tabValue) {
		if (hasValue()) {
			res.append(Constants.FOURTAB + "<target");
			if (getName() != null) {
				res.append(" name=\"" + getName() + "\"");
			}
			toXMLInternal(res, tabValue);
		}
	}

	protected void toXMLInternal(StringBuilder res, String tabValue) {
		xmlAttributeInTargetPart(res);
		if (needTargetPart()) {
			res.append(">\n");
			xmlElementsInTargetPart(res);
			res.append(Constants.FOURTAB + "</target>\n");
		} else {
			res.append("/>\n");
		}
	}

	public void xmlAttributeInTargetPart(StringBuilder res) {
		ElementInfo.toXML(res, translated, "\n" + Constants.FOURTAB + " ",
				"", "");
		ElementInfo.toXML(res, renamed, "\n" + Constants.FOURTAB + " ", "",
				"");
		ElementInfo.toXML(res, covariant, "\n" + Constants.FOURTAB
				+ "        ", "", "");
		ElementInfo.toXML(res, parametersForGenericMethods, "\n"
				+ Constants.FOURTAB + "        ", "", "");
		ElementInfo.toXML(res, genericsTest, "\n" + Constants.FOURTAB
				+ "        ", "", "");
		ElementInfo.toXML(res, dotnetFramework, "", "", "");
	}

	public void xmlElementsInTargetPart(StringBuilder res) {
		ElementInfo.toXML(res, codeReplacement, "", "", Constants.FIVETAB);
		ElementInfo.toXML(res, pattern, "", "", Constants.FIVETAB);
		ElementInfo.toXML(res, patternForSuperInvocation, "", "", Constants.FIVETAB);
		ElementInfo.toXML(res, comments, "", "", Constants.FIVETAB);
		ElementInfo.toXML(res, constraints, "", "", Constants.FIVETAB);
		ElementInfo.toXML(res, type, " ", "", "");
		ElementInfo.toXML(res, getChangeModifierDescriptor(),
				Constants.FIVETAB);
	}

	private boolean needTargetPart() {
		return !codeReplacement.isDefaultValue() || !pattern.isDefaultValue()
		 || !patternForSuperInvocation.isDefaultValue()
				|| getChangeModifierDescriptor() != null || !type.isDefaultValue() 
				|| !comments.isDefaultValue() || !constraints.isDefaultValue();
	}

	//
	//
	//

	// <target
	// isRemoved=""
	// renamed=""
	// covariant="..."
	// parametersForGenericMethods=".."
	// genericsTest="..."/>
	// name="...">
	// <format>
	// <CDATA>...</CDATA>
	// </format>
	// <comments>
	// <CDATA>...</CDATA>
	// </comments>
	// <codeReplacement>
	// <CDATA>...</CDATA>
	// </codeReplacement>
	// <modifiers ... />
	// </target>
	public void fromXML(Element pckElement) {
		name.fromXML(pckElement);
		fromXMLInternal(pckElement);
	}

	// isRemoved=""
	// renamed=""
	// parametersForGenericMethods=".."
	// covariant="..."
	// genericsTest="..."/>
	// <format>
	// <CDATA>...</CDATA>
	// </format>
	// <comments>
	// <CDATA>...</CDATA>
	// </comments>
	// <codeReplacement>
	// <CDATA>...</CDATA>
	// </codeReplacement>
	// <modifiers ... />
	protected void fromXMLInternal(Element pckElement) {
		translated.fromXML(pckElement);
		renamed.fromXML(pckElement);
		covariant.fromXML(pckElement);
		parametersForGenericMethods.fromXML(pckElement);
		genericsTest.fromXML(pckElement);
		codeReplacement.fromXML(pckElement);
		pattern.fromXML(pckElement);
		patternForSuperInvocation.fromXML(pckElement);
		comments.fromXML(pckElement);
		type.fromXML(pckElement);
		dotnetFramework.fromXML(pckElement);
		//
		NodeList lNodes = pckElement.getChildNodes();
		if (lNodes != null) {
			for (int i = 0; i < lNodes.getLength(); i++) {
				Node child = lNodes.item(i);
				if (child.getNodeName().equals("modifiers")) {
					if (changeModifiersDescriptor == null)
						changeModifiersDescriptor = new ChangeModifierDescriptor();
					getChangeModifierDescriptor().fromXML((Element) child);
				}
			}
		}
	}

	protected boolean hasValue() {
		// TODO Auto-generated method stub
		return isMappedToAProperty() || isMappedToAnIndexer()
				|| !name.isDefaultValue() || getGenericsTest() != null
				|| getCodeReplacement() != null || !pattern.isDefaultValue()
				|| !patternForSuperInvocation.isDefaultValue()
				|| getChangeModifierDescriptor() != null
				|| !getRemoveOption().isDefaultValue()
				|| !getRenamedOption().isDefaultValue()
				|| !getCovariantOption().isDefaultValue();
	}
};
