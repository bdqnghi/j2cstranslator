package com.ilog.translator.java2cs.configuration.target;

import org.w3c.dom.Element;

import com.ilog.translator.java2cs.configuration.info.Constants;
import com.ilog.translator.java2cs.configuration.info.ElementInfo;
import com.ilog.translator.java2cs.configuration.options.BooleanOptionBuilder;
import com.ilog.translator.java2cs.configuration.options.BooleanOptionEditor;
import com.ilog.translator.java2cs.configuration.options.OptionImpl;
import com.ilog.translator.java2cs.configuration.options.PackageMappingPolicy;
import com.ilog.translator.java2cs.configuration.options.PackageMappingPolicyOptionBuilder;
import com.ilog.translator.java2cs.configuration.options.PackageMappingPolicyOptionEditor;
import com.ilog.translator.java2cs.configuration.options.OptionImpl.XMLKind;
import com.ilog.translator.java2cs.translation.noderewriter.PackageRewriter;
import com.ilog.translator.java2cs.util.Utils;

/**
 * 
 * @author afau
 * 
 *         Represents a target element (i.e. an C# artifact).
 * 
 *         options : - memberMappingBehavior - packageMappingBehavior
 *         
 *         Warning: remove means "not translate". We need an option to say "remove it from import clauses"
 * 
 */
public class TargetPackage extends TargetImportableElement {

	//
	// Options
	//

	private OptionImpl<PackageMappingPolicy> packageMappingBehavior = new OptionImpl<PackageMappingPolicy>(
			"packageMappingBehavior", null, null, OptionImpl.Status.PRODUCTION,
			new PackageMappingPolicyOptionBuilder(),
			new PackageMappingPolicyOptionEditor(), XMLKind.ATTRIBUT, "default behavior for package mapping");
	protected OptionImpl<Boolean> removeFromImport = new OptionImpl<Boolean>("removeFromImport", null,
			false, OptionImpl.Status.BETA, new BooleanOptionBuilder(),
			new BooleanOptionEditor(), XMLKind.ATTRIBUT, 
			"Remove all occurence of that package from imports clauses");
	
	//
	// Constructor
	//
	
	public TargetPackage() {
	}
	

	//
	// PackageMappingBehavior
	//

	public void setPackageMappingBehavior(
			PackageMappingPolicy packageMappingBehavior) {
		this.packageMappingBehavior.setValue(packageMappingBehavior);
	}

	public PackageMappingPolicy getPackageMappingBehavior() {
		return packageMappingBehavior.getValue();
	}
	
	//
	// Rewriter
	//
	
	@Override
	public PackageRewriter getRewriter() {
		if (!name.isDefaultValue()) {
			setRewriter(new PackageRewriter(name.getValue()));
		} else {			
			if (needCapitalization())
				setRewriter(new PackageRewriter(Utils.capitalize(sourcePackageName)));
			else
				setRewriter(new PackageRewriter(sourcePackageName));
		}
		PackageRewriter rew = (PackageRewriter) super.getRewriter();
		return rew;
	}
	
	private boolean needCapitalization() {
		return packageMappingBehavior.isDefaultValue() || packageMappingBehavior.getValue().equals(
				PackageMappingPolicy.CAPITALIZED);
	}

	//
	// Serialization
	//

	// <target
	//    name="..."
	//    packageMappingBehavior="..."
	//    memberMappingBehavior="...">
	// </target>
	public void toXML(StringBuilder builder, String tabValue) {
		if (!name.isDefaultValue() || !translated.isDefaultValue() || !removeFromImport.isDefaultValue()) {
			builder.append(Constants.TWOTAB + "<target ");	
			ElementInfo.toXML(builder, name, "", "", "");
			ElementInfo.toXML(builder, translated, "", "", "");
			ElementInfo.toXML(builder, removeFromImport, "", "", "");
			builder.append(">\n");
			builder.append(Constants.TWOTAB + "</target>\n");
		}		
		ElementInfo.toXML(builder, memberMappingBehavior, "\n" + Constants.TWOTAB + "        ", "", "");
		ElementInfo.toXML(builder, packageMappingBehavior, "\n" + Constants.TWOTAB + "        ", "", "");
	}

	// <target
	//    name="..."
	//    packageMappingBehavior="..."
	//    memberMappingBehavior="...">
	// </target>
	public void fromXML(Element pckElement) {
		name.fromXML(pckElement);
		translated.fromXML(pckElement);
		removeFromImport.fromXML(pckElement);
		packageMappingBehavior.fromXML(pckElement);
		memberMappingBehavior.fromXML(pckElement);
		//
		
	}
}