package com.ilog.translator.java2cs.configuration.target;

import java.util.List;

import org.eclipse.jdt.core.dom.ASTNode;

import com.ilog.translator.java2cs.configuration.ChangeModifierDescriptor;
import com.ilog.translator.java2cs.configuration.XMLElement;
import com.ilog.translator.java2cs.configuration.options.BooleanOptionBuilder;
import com.ilog.translator.java2cs.configuration.options.BooleanOptionEditor;
import com.ilog.translator.java2cs.configuration.options.DotNetFramework;
import com.ilog.translator.java2cs.configuration.options.DotNetFrameworkOptionBuilder;
import com.ilog.translator.java2cs.configuration.options.DotNetFrameworkOptionEditor;
import com.ilog.translator.java2cs.configuration.options.JDK;
import com.ilog.translator.java2cs.configuration.options.JDKOptionBuilder;
import com.ilog.translator.java2cs.configuration.options.JDKOptionEditor;
import com.ilog.translator.java2cs.configuration.options.OptionImpl;
import com.ilog.translator.java2cs.configuration.options.StringOptionBuilder;
import com.ilog.translator.java2cs.configuration.options.StringOptionEditor;
import com.ilog.translator.java2cs.configuration.options.OptionImpl.XMLKind;
import com.ilog.translator.java2cs.translation.noderewriter.ElementRewriter;
import com.ilog.translator.java2cs.translation.noderewriter.INodeRewriter;

/**
 * 
 * @author afau
 * 
 *         Represents a target element (i.e. a C# artifact).
 * 
 *         options : - removed - renamed - constraints - name - packageName - typeParameters -
 *         comments - modifiers
 */
public abstract class TargetElement implements XMLElement {

	// ??
	protected String sourcePackageName;

	//
	// Options
	//
	protected OptionImpl<Boolean> translated = new OptionImpl<Boolean>("removed", new String[] { "isRemove" },
			false, OptionImpl.Status.PRODUCTION, new BooleanOptionBuilder(),
			new BooleanOptionEditor(), XMLKind.ATTRIBUT, 
			"???");
	protected OptionImpl<Boolean> renamed = new OptionImpl<Boolean>("renamed", null,
			false, OptionImpl.Status.PRODUCTION, new BooleanOptionBuilder(),
			new BooleanOptionEditor(), XMLKind.ATTRIBUT, 
			"indicate that element must be renamed (instead of just map name)");
	protected OptionImpl<String> constraints = new OptionImpl<String>("constraints",
			null, null, OptionImpl.Status.PRODUCTION, new StringOptionBuilder(),
			new StringOptionEditor(), XMLKind.ATTRIBUT, 
			"type parameter constraint to add");
	protected OptionImpl<String> name = new OptionImpl<String>("name", null, null,
			OptionImpl.Status.PRODUCTION, new StringOptionBuilder(),
			new StringOptionEditor(), XMLKind.ATTRIBUT, 
			"new name for this element");
	protected OptionImpl<String> packageName = new OptionImpl<String>("packageName",
			null, null, OptionImpl.Status.PRODUCTION, new StringOptionBuilder(),
			new StringOptionEditor(), XMLKind.ATTRIBUT, 
			"new package name for this element");
	protected OptionImpl<String> typeParameters = new OptionImpl<String>("typeParameters",
			null, null, OptionImpl.Status.PRODUCTION, new StringOptionBuilder(),
			new StringOptionEditor(), XMLKind.ATTRIBUT, 
			"type parameter for this element");
	protected OptionImpl<String> comments = new OptionImpl<String>("comments", null,
			null, OptionImpl.Status.PRODUCTION, new StringOptionBuilder(),
			new StringOptionEditor(), XMLKind.CDATA, 
			"new comments for this elements");
	//

	protected OptionImpl<DotNetFramework> dotnetFramework = new OptionImpl<DotNetFramework>(
			"dotnetFramework", null, DotNetFramework.NET3_5, OptionImpl.Status.EXPERIMENTAL,
			new DotNetFrameworkOptionBuilder(),
			new DotNetFrameworkOptionEditor(), XMLKind.ATTRIBUT, ".NET Framework target");
	// 
	protected ChangeModifierDescriptor changeModifiersDescriptor = new ChangeModifierDescriptor();

	//
	@SuppressWarnings("unchecked")
	protected List typeArgs;
	protected INodeRewriter rewriter = null;

	//
	// Constructor
	//

	/**
	 * Create a new target element with given name.
	 * 
	 * @param packageName
	 *            The package name of this
	 * @param name
	 *            The name of this target field.
	 */
	protected TargetElement(String packageName, String name) {
		this.packageName.setValue(packageName);
		this.name.setValue(name);
	}

	/**
	 * 
	 */
	protected TargetElement() {
	}

	//
	// Translated
	//

	public boolean isTranslated() {
		return translated.getValue();
	}

	public void setTranslated(boolean rem) {
		translated.setValue(rem);
	}

	//
	// Renamed
	//

	public void setRenamed(boolean b) {
		renamed.setValue(b);
	}

	public boolean isRenamed() {
		return renamed.getValue();
	}

	//
	// Comments
	//
	
	public boolean hasComments() {
		return !comments.isDefaultValue();
	}
	
	public String getComments() {
		return comments.getValue();
	}
	
	//
	// Source package name
	//

	public void setSourcePackageName(String packageName) {
		sourcePackageName = packageName;
	}

	public String getSourcePackageName() {
		return sourcePackageName;
	}

	//
	// package name
	//
	
	/**
	 * @param packageName
	 *            The packageName to set.
	 */
	public void setPackageName(String packageName) {
		this.packageName.setValue(packageName);

	}

	/**
	 * @return Returns the packageName.
	 */
	public String getPackageName() {
		return packageName.getValue();
	}

	//
	// name
	//

	/**
	 * @param name
	 *            The name to set.
	 */
	public void setName(String name) {
		this.name.setValue(name);
	}

	/**
	 * @return Returns the name.
	 */
	public String getName() {
		if (name.isDefaultValue() && packageName.isDefaultValue()) {
			return null;
		}
		return (packageName.isDefaultValue() ? "" : packageName.getValue()
				+ ".")
				+ name.getValue();
	}
	
	//
	// TypeParameters
	//

	/**
	 * @return Returns the name
	 */
	public OptionImpl<String> getTypeParametersOption() {
		return typeParameters;
	}

	//
	// Rewriter
	//

	public INodeRewriter getRewriter() {
		if (rewriter != null) {
			rewriter.setRemove(translated.getValue());
			if (rewriter instanceof ElementRewriter) {
				((ElementRewriter) rewriter).setTypeArguments(typeArgs);
			}
		}
		return rewriter;
	}

	public void setRewriter(INodeRewriter rewriter) {
		this.rewriter = rewriter;
	}

	//
	// ChangeModifierDescriptor
	//

	public void setChangeModifierDescriptor(ChangeModifierDescriptor mods) {
		changeModifiersDescriptor = mods;
		if (rewriter != null) {
			rewriter.setChangeModifier(mods);
		}
	}

	public ChangeModifierDescriptor getChangeModifierDescriptor() {
		return changeModifiersDescriptor;
	}

	//
	// process
	//

	public ASTNode process(ASTNode node) {
		return null;
	}

	//
	// Constraints
	//

	public void setConstraints(String constraints) {
		this.constraints.setValue(constraints);
	}

	public String getConstraints() {
		return constraints.getValue();
	}

	//
	// options
	//

	public OptionImpl<Boolean> getRemoveOption() {
		return translated;
	}

	public OptionImpl<Boolean> getRenamedOption() {
		return renamed;
	}

	public OptionImpl<String> getNameOption() {
		return name;
	}

	public OptionImpl<String> getPackageNameOption() {
		return packageName;
	}

}