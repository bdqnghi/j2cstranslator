package com.ilog.translator.java2cs.configuration.target;

import java.util.ArrayList;
import java.util.List;

import org.w3c.dom.Element;

import com.ilog.translator.java2cs.configuration.info.Constants;
import com.ilog.translator.java2cs.configuration.info.MethodInfo;
import com.ilog.translator.java2cs.configuration.options.OptionImpl;
import com.ilog.translator.java2cs.configuration.options.StringOptionBuilder;
import com.ilog.translator.java2cs.configuration.options.StringOptionEditor;
import com.ilog.translator.java2cs.configuration.options.OptionImpl.XMLKind;
import com.ilog.translator.java2cs.configuration.parser.IlrConstants;
import com.ilog.translator.java2cs.translation.noderewriter.INodeRewriter;
import com.ilog.translator.java2cs.translation.noderewriter.IndexerRewriter;
import com.ilog.translator.java2cs.translation.noderewriter.MethodRewriter;
import com.ilog.translator.java2cs.translation.noderewriter.IndexerRewriter.IndexerKind;

/**
 * Represents a 'target property' i.e. the description (name, getter and/or
 * setter) of a property in a C# classes.
 */
public class TargetIndexer extends TargetMethod {

	private MethodInfo getter = null;
	private MethodInfo setter = null;

	//
	// Options
	//
	private int[] paramsIndexs;
	private int valueIndex;

	protected OptionImpl<String> indexerGet = new OptionImpl<String>(
			"indexerGet", null, null, OptionImpl.Status.PRODUCTION,
			new StringOptionBuilder(), new StringOptionEditor(),
			XMLKind.ATTRIBUT, "method to associate with a 'get' indexer");
	protected OptionImpl<String> indexerSet = new OptionImpl<String>(
			"indexerSet", null, null, OptionImpl.Status.PRODUCTION,
			new StringOptionBuilder(), new StringOptionEditor(),
			XMLKind.ATTRIBUT, "method to associate with a 'set' indexer");
	
	//
	// constructors
	//

	/**
	 * 
	 */
	public TargetIndexer() {
	}

	/**
	 * 
	 * @param kind
	 * @param paramsIndexs
	 * @param valueIndex
	 */
	public TargetIndexer(IndexerRewriter.IndexerKind kind, int[] paramsIndexs,
			int valueIndex) {
		super();
		this.paramsIndexs = paramsIndexs;
		this.valueIndex = valueIndex;
		setRewriter(new IndexerRewriter(kind, paramsIndexs, valueIndex));
		if (kind == IndexerKind.READ) {	
			indexerGet.parse("@" + paramsIndexs[0]);
		} else if (kind == IndexerKind.WRITE) {			
			indexerSet.parse("[@" + paramsIndexs[0] + "]=@" + valueIndex);
		}
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
		return true;
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
	// ParamsIndexs
	//

	public int[] getParamsIndexs() {
		return paramsIndexs;
	}

	public int getValueIndex() {
		return valueIndex;
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
	//   indexGet="..."
    //   indexSet="...">
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
			if (!indexerGet.isDefaultValue()) {
				indexerGet.toXML(res, tabValue);
			} else if (!indexerSet.isDefaultValue()) {
				indexerSet.toXML(res, tabValue);
			}
			toXMLInternal(res, tabValue);
		}
	}

	
	// <target
	//   isRemoved="" 
	//   renamed=""
	//   covariant="..."
	//   genericsTest="..."/>
	//   indexGet="..."
    //   indexSet="...">
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
		indexerGet.fromXML(node);
		if (!indexerGet.isDefaultValue()) {
			paramsIndexs = parseIndexerGetArguments(indexerGet.getValue());
			setRewriter(new IndexerRewriter(IndexerKind.READ, paramsIndexs, valueIndex));
		}
		indexerSet.fromXML(node);
		if (!indexerSet.isDefaultValue()) {
			String pattern = indexerSet.getValue();
			int equalsPos = pattern.indexOf("=");
			paramsIndexs = parseIndexerGetArguments(pattern.substring(1, equalsPos - 1));
			valueIndex = parseIndexerSetArguments(pattern.substring(equalsPos + 1));
			setRewriter(new IndexerRewriter(IndexerKind.WRITE, paramsIndexs, valueIndex));
		}
		fromXMLInternal(node);
	}
	
	private int[] parseIndexerGetArguments(String arg) {
		char[] chars = arg.toCharArray();
		int pos = 0;
		/*if (chars[pos] != '[') {
			// printError("expecting a '['");
			return null;
		}*/
		//pos++;
		StringBuffer buffer = new StringBuffer();
		final List<String> args = new ArrayList<String>();
		while(pos < chars.length) {	
			if (chars[pos] == IlrConstants.COMMA) {
				final String posi = buffer.toString();
				if (!posi.startsWith("@")) {
					// printError("expecting a '@'");
					return null;
				} else {
					final String s = posi.substring(1);
					args.add(s);
				}
				buffer = new StringBuffer();
			} else {
				buffer.append(chars[pos]);
			}
			pos++;
		}
		if (buffer.length() > 0) {
			final String posi = buffer.toString();
			if (!posi.startsWith("@")) {
				// printError("expecting a '@'");
				return null;
			} else {
				final String s = posi.substring(1);
				args.add(s);
			}
		}

		/*if (chars[pos] != ']') {
			// printError("expecting a ']'");
			return null;
		}*/

		final int[] res = new int[args.size()];
		for (int i = 0; i < args.size(); i++) {
			res[i] = Integer.parseInt(args.get(i));
		}

		return res;
	}

	private int parseIndexerSetArguments(String arg) {
		char[] chars = arg.toCharArray();
		int pos = 0;

		if (chars[pos] != '@') {
			// printError("expecting a '@'");
			return -1;
		} else {
			pos++;
			return Integer.parseInt(arg.substring(1));
		}
	}
	
	//
	// cloneForChild
	//
	
	public TargetMethod cloneForChild() {
		INodeRewriter rewriter = getRewriter();
		final TargetIndexer newTM = new TargetIndexer();
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
		//
		newTM.indexerGet = indexerGet;
		newTM.indexerSet = indexerSet;
		newTM.paramsIndexs = paramsIndexs;
		newTM.valueIndex = valueIndex;
		newTM.setter = setter;
		newTM.getter = getter;
		return newTM;
	}
}
