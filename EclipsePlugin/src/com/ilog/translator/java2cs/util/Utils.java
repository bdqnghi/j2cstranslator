package com.ilog.translator.java2cs.util;

import java.io.File;
import java.util.List;
import java.util.StringTokenizer;

public class Utils {

	public static String normalize(String name) {
		return name.replace("/", "_").replace("\\", "_");
	}

	public static String capitalize(String name) {
		if(name != null){
			if (name.contains(IKeywordConstants.DOT)) {
				StringTokenizer st = new StringTokenizer(name, IKeywordConstants.DOT);
				StringBuffer buffer = new StringBuffer();
				while (st.hasMoreTokens()) {
					String current = Utils.capitalize(st.nextToken());
					buffer.append(current);
					if (st.hasMoreTokens()) {
						buffer.append(IKeywordConstants.DOT);
					}
				}
				return buffer.toString();
			} else if (!"".equals(name)) {
				return name.substring(0, 1).toUpperCase()
				+ name.substring(1, name.length());
			} else {
				System.err.println("Java2CsMapper : name is " + name);
				return name;
			}
		}
		return name;
	}

	public static String buildPath(String... segments) {
		String result = "";
		for (final String seg : segments) {
			result += seg + File.separator;
		}
		return result;
	}

	public static String getValueFromCommandLine(List<String> cmdLine,
			String string, String... defaultValue) throws Exception {
		for (int i = 0; i < cmdLine.size(); i++) {
			final String cmd = cmdLine.get(i);
			if (cmd.startsWith("/" + string + ":")) {
				return cmd.substring(("/" + string + ":").length());
			}
		}
		if (defaultValue == null || defaultValue.length == 0) {
			throw new Exception("Missing value for commande " + string);
		}
		return defaultValue[0];
	}

	//
	//
	//

	public static String mangle(String handlerKey) {
		if (handlerKey != null) {
			return handlerKey.replaceAll(" ", "%");
		}
		return handlerKey;
	}

	public static String unmangle(String handlerKey) {
		if (handlerKey != null) {
			return handlerKey.replaceAll("%", " ");
		}
		return handlerKey;
	}

	//
	//
	//
	
	public static String replaceForbiddenChar(String name) {
		return name.replace("$", "_");
	}

	public static String xmlify(String aXML) {
		int length = aXML.length();
		StringBuffer result = new StringBuffer(length * 2);
		for (int i = 0; i < length; i++) {
			char c = aXML.charAt(i);
			switch (c) {
			case '<':
				result.append("&lt;");
				break;
			case '>':
				result.append("&gt;");
				break;
			case '\'':
				result.append("&apos;");
				break;
			case '"':
				result.append("&quot;");
				break;
			case '&':
				result.append("&amp;");
				break;
			default:
				result.append(c);
				break;
			}
		}
		return new String(result);
	}

	public static String dexmlify(String aXML) {
		return aXML.replace("&lt;", "<").replace("&gt;", ">").replace("&apos;", "\'").replace("&quot;", "\"").replace("&amp;", "&").replace("&#169;","©");		
	}

	public static boolean isLegalXmlChar(int character) {
		return (character == 0x9 /* == '\t' == 9 */|| character == 0xA /*
																	 * == '\n'
																	 * == 10
																	 */
				|| character == 0xD /* == '\r' == 13 */
				|| (character >= 0x20 && character <= 0xD7FF)
				|| (character >= 0xE000 && character <= 0xFFFD) || (character >= 0x10000 && character <= 0x10FFFF));
	}

	public static boolean ensureXMLIsClean(String dirty) {
		for (char c : dirty.toCharArray()) {
			if (!isLegalXmlChar(c))
				return false;
		}
	
		return true;
	}

	public static String removeGenerics(String simpleName) {
		int index = simpleName.indexOf("<");
		if (index > 0)
			return simpleName.substring(0, index);
		else
			return simpleName;
	}

	public static boolean hasGenerics(String simpleName) {	
		return simpleName.indexOf("<") > 0;
	}

	public static String getGenerics(String simpleName) {
		int index = simpleName.indexOf("<");
		if (index > 0)
			return simpleName.substring(index + 1, simpleName.length() - 1);
		else
			return simpleName;
	}

	public static String getSignature(String simpleName) {
		int index = simpleName.indexOf("(");
		if (index > 0)
			return xmlify(simpleName.substring(index + 1, simpleName.length() - 1));
		else
			return xmlify(simpleName);
	}

}
