package com.ilog.translator.java2cs.configuration.target;

import org.w3c.dom.Element;

import com.ilog.translator.java2cs.configuration.info.Constants;
import com.ilog.translator.java2cs.configuration.info.MethodInfo;
import com.ilog.translator.java2cs.configuration.options.OptionImpl;
import com.ilog.translator.java2cs.configuration.options.StringOptionBuilder;
import com.ilog.translator.java2cs.configuration.options.StringOptionEditor;
import com.ilog.translator.java2cs.configuration.options.OptionImpl.XMLKind;
import com.ilog.translator.java2cs.translation.noderewriter.INodeRewriter;
import com.ilog.translator.java2cs.translation.noderewriter.MethodRewriter;
import com.ilog.translator.java2cs.translation.noderewriter.PropertyRewriter;
import com.ilog.translator.java2cs.translation.noderewriter.PropertyRewriter.ProperyKind;

/**
 * Represents a 'target property' i.e. the description (name, getter and/or
 * setter) of a property in a C# classes.
 */
public class TargetProperty extends TargetMethod {

	private MethodInfo getter = null;
	private MethodInfo setter = null;
	private PropertyRewriter.ProperyKind kind;
	
	//
	// Options
	//
	
	protected OptionImpl<String> propertyGet = new OptionImpl<String>(
			"propertyGet", null, null, OptionImpl.Status.PRODUCTION,
			new StringOptionBuilder(), new StringOptionEditor(),
			XMLKind.ATTRIBUT, "method to associate into a 'get' property");
	
	protected OptionImpl<String> propertySet = new OptionImpl<String>(
			"propertySet", null, null, OptionImpl.Status.PRODUCTION,
			new StringOptionBuilder(), new StringOptionEditor(),
			XMLKind.ATTRIBUT, "method to associate into a 'set' property");
		
	//
	// constructors
	//

	/**
	 * 
	 */
	public TargetProperty() {
	}

	/**
	 * 
	 * @param name
	 * @param kind
	 */
	public TargetProperty(String name, PropertyRewriter.ProperyKind kind) {
		this.name.setValue(name);
		this.kind = kind;
		setRewriter(new PropertyRewriter(name, kind));
		if (kind == ProperyKind.READ) {
			propertyGet.parse(name);
		} else if (kind == ProperyKind.WRITE) {
			propertySet.parse(name);
		}
	}


	//
	// PropertyGetOption
	//
	
	public OptionImpl<String> getPropertyGetOption() {
		return propertyGet;
	}

	public OptionImpl<String> getPropertySetOption() {
		return propertySet;
	}
	
	//
	// PropertyGetValue
	//
	
	public String getPropertyGetValue() {
		return propertyGet.getValue();
	}

	//
	// PropertySetValue
	//
	
	public String getPropertySetValue() {
		return propertySet.getValue();
	}
	
	//
	// MappedToAProperty
	//

	public boolean isMappedToAProperty() {
		return true;
	}

	//
	// MappedToAnIndexer
	//

	public boolean isMappedToAnIndexer() {
		return false;
	}
	
	//
	// Kind
	//

	public PropertyRewriter.ProperyKind getKind() {
		return kind;
	}
	
	public void setKind(PropertyRewriter.ProperyKind kind) {
		this.kind = kind;
	}

	//
	// getter
	//

	/**
	 * @param getter
	 *            The getter to set.
	 */
	public void setGetter(MethodInfo getter) {
		this.getter = getter;
	}

	/**
	 * @return Returns the getter.
	 */
	public MethodInfo getGetter() {
		return getter;
	}

	//
	// setter
	//

	/**
	 * @param setter
	 *            The setter to set.
	 */
	public void setSetter(MethodInfo setter) {
		this.setter = setter;
	}

	/**
	 * @return Returns the setter.
	 */
	public MethodInfo getSetter() {
		return setter;
	}
		
	//
	// Rewriter
	//
	
	@Override
	public INodeRewriter getRewriter() {
		final INodeRewriter nr = super.getRewriter();
		if (nr instanceof PropertyRewriter) {
			final PropertyRewriter rew = (PropertyRewriter) nr;
			if (!covariant.isDefaultValue())
				rew.setCovariantType(covariant.getStringValue());			
		}
		return nr;
	}

	//
	// toString
	//

	@Override
	public String toString() {
		String descr = "";
		descr += name + " " + getter + " " + setter;
		return descr;
	}

	//
	// Serialization
	//

	// <target
	//   isRemoved="" 
	//   renamed=""
	//   covariant="..."
	//   genericsTest="..."/>
	//   propertyGet="..."
    //   propertySet="...">
	//      <format>
    //         <CDATA>...</CDATA>
	//      </format>
	//      <comments>
	//         <CDATA>...</CDATA>
	//      </comments>
    //      <codeReplacement>
	//         <CDATA>...</CDATA>
	//      </codeReplacement>
    //      <modifiers ... />
	// </target>
	@Override
	public void toXML(StringBuilder res, String tabValue) {
		if (hasValue()) {
			res.append(Constants.FOURTAB + "<target ");
			if (getKind() == ProperyKind.READ) {
				propertyGet.toXML(res, "");
			}
			if (getKind() == ProperyKind.WRITE) {
				propertySet.toXML(res, "");
			}		
			toXMLInternal(res, tabValue);
		}
	}
	
	//
	//
	//
	
	// <target
	//   isRemoved="" 
	//   renamed=""
	//   covariant="..."
	//   genericsTest="..."/>
	//   propertyGet="..."
    //   propertySet="...">
	//      <format>
    //         <CDATA>...</CDATA>
	//      </format>
	//      <comments>
	//         <CDATA>...</CDATA>
	//      </comments>
    //      <codeReplacement>
	//         <CDATA>...</CDATA>
	//      </codeReplacement>
    //      <modifiers ... />
	// </target>
	@Override
	public void fromXML(Element node) {		
		propertyGet.fromXML(node);
		if (!propertyGet.isDefaultValue()) {
			kind =  ProperyKind.READ;
			setRewriter(new PropertyRewriter(propertyGet.getValue(), kind));
		}
		propertySet.fromXML(node);
		if (!propertySet.isDefaultValue()) {
			kind =  ProperyKind.WRITE;
			setRewriter(new PropertyRewriter(propertySet.getValue(), kind));
		}		
		fromXMLInternal(node);
	}
	
	//
	// cloneForChild
	//
	
	@Override
	public TargetMethod cloneForChild() {
		INodeRewriter rewriter = getRewriter();
		final TargetProperty newTM = new TargetProperty();
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
		newTM.propertyGet = propertyGet;
		newTM.propertySet = propertySet;
		newTM.setter = setter;
		newTM.getter = getter;
		newTM.kind = kind;
		return newTM;
	}
};
