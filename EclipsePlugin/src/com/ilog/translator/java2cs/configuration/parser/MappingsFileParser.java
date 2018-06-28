package com.ilog.translator.java2cs.configuration.parser;

import java.io.IOException;
import java.io.Reader;
import java.io.StringReader;
import java.util.ArrayList;
import java.util.Hashtable;
import java.util.List;
import java.util.StringTokenizer;
import java.util.Vector;

import org.eclipse.core.resources.IProject;
import org.eclipse.jdt.core.JavaModelException;

import com.ilog.translator.java2cs.configuration.ChangeHierarchyDescriptor;
import com.ilog.translator.java2cs.configuration.ChangeModifierDescriptor;
import com.ilog.translator.java2cs.configuration.DotNetModifier;
import com.ilog.translator.java2cs.configuration.info.ClassInfo;
import com.ilog.translator.java2cs.configuration.info.FieldInfo;
import com.ilog.translator.java2cs.configuration.info.ClassInfo;
import com.ilog.translator.java2cs.configuration.info.PackageInfo;
import com.ilog.translator.java2cs.configuration.info.MappingsInfo;
import com.ilog.translator.java2cs.configuration.info.MethodInfo;
import com.ilog.translator.java2cs.configuration.info.PackageInfo;
import com.ilog.translator.java2cs.configuration.info.TranslateInfo;
import com.ilog.translator.java2cs.configuration.options.MethodMappingPolicy;
import com.ilog.translator.java2cs.configuration.options.PackageMappingPolicy;
import com.ilog.translator.java2cs.configuration.target.TargetClass;
import com.ilog.translator.java2cs.configuration.target.TargetField;
import com.ilog.translator.java2cs.configuration.target.TargetIndexer;
import com.ilog.translator.java2cs.configuration.target.TargetMethod;
import com.ilog.translator.java2cs.configuration.target.TargetPackage;
import com.ilog.translator.java2cs.configuration.target.TargetProperty;
import com.ilog.translator.java2cs.translation.noderewriter.FieldRewriter;
import com.ilog.translator.java2cs.translation.noderewriter.IndexerRewriter;
import com.ilog.translator.java2cs.translation.noderewriter.PropertyRewriter;
import com.ilog.translator.java2cs.translation.util.ReplaceClassCall;

/*
 * 
 * 
 */
public class MappingsFileParser implements IlrConstants {

	private String fileName;

	private final IlrCharStream charStream;

	private final IlrTokenManager source;

	private Token current;

	private boolean returnStatus;

	@SuppressWarnings("unchecked")
	private Hashtable mappings = new Hashtable();

	// Documentation mapping tables
	@SuppressWarnings("unchecked")
	private Hashtable htmlDocMapping = new Hashtable();

	@SuppressWarnings("unchecked")
	private Hashtable tagsDocMapping = new Hashtable();

	private final MappingsInfo mInfo;

	private String basedir;

	private final IProject reference;

	private final boolean isSystemFile;

	//
	// Constructor
	//

	/**
	 * Creates a new <code>ConfigurationFileParser</code> instance.
	 * 
	 */
	public MappingsFileParser(MappingsInfo mInfo, IProject reference, boolean isSystemFile) {
		this.mInfo = mInfo;
		charStream = new IlrCharStream(new StringReader(""));
		source = new IlrTokenManager(charStream);
		this.reference = reference;
		this.isSystemFile = isSystemFile;
		nextToken();
	}

	//
	//
	//

	@SuppressWarnings("unchecked")
	public void clean() {
		mappings = new Hashtable();
		tagsDocMapping = new Hashtable();
		htmlDocMapping = new Hashtable();
	}

	//
	//
	//

	/**
	 * Get the Basedir value.
	 * 
	 * @return the Basedir value.
	 */
	public String getBasedir() {
		return basedir;
	}

	/**
	 * Set the Basedir value.
	 * 
	 * @param newBasedir
	 *            The new Basedir value.
	 */
	public void setBasedir(String newBasedir) {
		basedir = newBasedir;
	}

	// ------------------------------------------------------------
	// Initializations
	// ------------------------------------------------------------

	void reset() {
		current = null;
	}

	void initSource(Reader reader) {
		charStream.ReInit(reader);
		source.ReInit(charStream);
		nextToken();
	}

	// ------------------------------------------------------------
	// Initializations and parsing
	// ------------------------------------------------------------

	@SuppressWarnings("unchecked")
	private void addNameAlias(String key, String value) {
		mappings.put(key, "L" + value.replace('.', '/') + ';');
	}

	public void convertArgument(StringBuffer buffer, String arg) {
		final int len = arg.length();
		if (arg.endsWith("[]")) {
			buffer.append('[');
			convertArgument(buffer, arg.substring(0, len - 2));
		} else if (arg.equals("boolean")) {
			buffer.append('Z');
		} else if (arg.equals("char")) {
			buffer.append('C');
		} else if (arg.equals("byte")) {
			buffer.append('B');
		} else if (arg.equals("short")) {
			buffer.append('S');
		} else if (arg.equals("int")) {
			buffer.append('I');
		} else if (arg.equals("long")) {
			buffer.append('J');
		} else if (arg.equals("float")) {
			buffer.append('F');
		} else if (arg.equals("double")) {
			buffer.append('D');
		} else {
			final String rep = (String) mappings.get(arg);
			if (rep != null) {
				buffer.append(rep);
			} else {
				buffer.append('L');
				for (int i = 0; i < len; i++) {
					final char c = arg.charAt(i);
					if (c == '.') {
						buffer.append('/');
					} else {
						buffer.append(c);
					}
				}
				buffer.append(';');
			}
		}
	}

	public String convertMethod(String sign) {
		final StringBuffer buffer = new StringBuffer(30);
		final int index = sign.indexOf('(');
		if (index == -1) {
			throw new IllegalArgumentException();
		}

		buffer.append(sign.substring(0, index + 1));
		sign = sign.substring(index + 1);

		final StringTokenizer tokens = new StringTokenizer(sign, ",)");
		while (tokens.hasMoreElements()) {
			final String arg = (String) tokens.nextElement();
			convertArgument(buffer, arg);
		}
		buffer.append(')');
		return buffer.toString();
	}

	void parseAliases() {
		Token t = current;
		if (t.kind != IlrConstants.LBRACE) {
			printError("expecting '{' to initiate alias values");
			return;
		}

		nextToken();
		while (true) {
			final String s1 = parseSourcePart();
			if (current.kind == IlrConstants.ASSIGN) {
				nextToken();
			} else {
				printError("expecting '=' followed by the alias value");
			}
			final String s2 = parseTargetPart();
			addNameAlias(s1, s2);
			t = current;
			if ((t.kind == IlrConstants.RBRACE) || (t.kind == IlrConstants.EOF)) {
				break;
			}
		}
		expectSemiColon();
	}

	/**
	 * documentation { html { NAME = NAME } tags { NAME $n = STRING $n } }
	 */
	void parseDocumentation() {
		Token t = current;
		if (t.kind != IlrConstants.LBRACE) {
			printError("expecting '{' to initiate documentation values");
			return;
		}
		nextToken();
		//
		t = current;
		if (t.image.equals("html")) {
			nextToken();
			parseHtmlDoc();
		}
		//
		t = current;
		if (t.image.equals("tags")) {
			nextToken();
			parseTagsDoc();
		}
		//
		t = current;
		if (t.image.equals("mappings")) {
			nextToken();
			parseDocMappings();
		}
		//
		expectSemiColon();
	}

	@SuppressWarnings("unchecked")
	void parseHtmlDoc() {
		Token t = current;
		if (t.kind != IlrConstants.LBRACE) {
			printError("expecting '{' to initiate html documentation values");
			return;
		}
		nextToken();
		//
		while (true) {
			final String s1 = parseSourcePart();
			t = current;
			if (t.kind == IlrConstants.ASSIGN) {
				nextToken();
			} else {
				printError("expecting '=' followed by the map value");
			}
			final String s2 = parseTargetPart();
			htmlDocMapping.put(s1, s2);
			// System.out.println("..." + s1 + " -> " + s2);
			t = current;
			if ((t.kind == IlrConstants.RBRACE) || (t.kind == IlrConstants.EOF)) {
				break;
			}
		}
		expectSemiColon();
	}

	String parseTagName() {
		String tagName = null;
		if (current.kind == IlrConstants.IDENTIFIER) {
			tagName = current.image;
		}
		nextToken();
		return tagName;
	}

	@SuppressWarnings("unchecked")
	void parseTagsDoc() {
		Token t = current;
		if (t.kind != IlrConstants.LBRACE) {
			printError("expecting '{' to initiate tags documentation values");
			return;
		}
		nextToken();
		//
		while (true) {
			final String tagName = parseTagName();
			final Vector args = new Vector();
			while (current.kind != IlrConstants.ASSIGN) {
				// System.out.println("kind = " + current.kind);
				if (current.kind == IlrConstants.IDENTIFIER) {
					args.add(current.image);
				} else {
					printError("expecting an identifier ($n)");
				}
				nextToken();
			}
			t = current;
			//
			if (t.kind == IlrConstants.ASSIGN) {
				nextToken();
			} else {
				printError("expecting '=' followed by the map value");
			}
			//
			final Vector rightPart = new Vector();
			while (current.kind != IlrConstants.SEMICOLON) {
				switch (current.kind) {
				case STRING_LITERAL:
					rightPart.add(new StringLiteral((String) parseValue()));
					break;
				case IDENTIFIER:
					rightPart.add(new ArgsRef(current.image));
					break;
				default:
					printError("expecting an identifier ($n) or string literal");
				}
				nextToken();
			}
			// System.out.println("...<" + tagName + "> " + args +
			// " -> " + rightPart + " " + createDocTranslator(tagName,
			// args,
			// rightPart));
			// tagsDocMapping.put(s1, new Object[] {pre,post});
			tagsDocMapping.put(tagName, createDocTranslator(tagName, args,
					rightPart));

			//
			// expectSemiColon();
			nextToken();
			t = current;
			if ((t.kind == IlrConstants.RBRACE) || (t.kind == IlrConstants.EOF)) {
				break;
			}
		}
		expectSemiColon();
	}

	@SuppressWarnings("unchecked")
	public DocTranslator createDocTranslator(String name, Vector args,
			Vector rightPart) {
		final int size = rightPart.size();
		final RightPartElem[] rightPartElem = new RightPartElem[size];
		for (int i = 0; i < size; i++) {
			rightPartElem[i] = (RightPartElem) rightPart.elementAt(i);
		}
		return new DocTranslator(name, args.size(), rightPartElem);
	}

	@SuppressWarnings("unchecked")
	void parseDocMappings() {
		Token t = current;
		if (t.kind != IlrConstants.LBRACE) {
			printError("expecting '{' to initiate tags documentation values");
			return;
		}
		nextToken();
		//
		while (true) {
			final String pattern = (String) parseValue();
			nextToken();
			t = current;
			//
			if (t.kind == IlrConstants.ASSIGN) {
				nextToken();
			} else {
				printError("expecting '=' followed by the map value");
			}
			//
			final String replacement = (String) parseValue();

			mInfo.addJavaDocMapping(pattern, replacement, reference);
			//
			expectSemiColon();
			nextToken();
			t = current;
			if ((t.kind == IlrConstants.RBRACE) || (t.kind == IlrConstants.EOF)) {
				break;
			}
		}
		// this.expectSemiColon();
	}

	// ------------------------------------------------------------
	// 
	// ------------------------------------------------------------

	public boolean parsePackage(String filename, Reader reader)
			throws JavaModelException {
		fileName = filename;
		returnStatus = true;
		reset();
		initSource(reader);
		this.parsePackage();
		return returnStatus;
	}

	public boolean parseClass(String filename, Reader reader,
			PackageInfo pInfo, ClassInfo cInfo) throws JavaModelException {
		fileName = filename;
		returnStatus = true;
		reset();
		initSource(reader);
		// this.nextToken(); // skip the "{"
		this.parseClass(pInfo, cInfo);
		return returnStatus;
	}

	public boolean parseMethod(String filename, Reader reader, ClassInfo cInfo,
			MethodInfo mInfo) throws JavaModelException {
		fileName = filename;
		returnStatus = true;
		reset();
		initSource(reader);
		nextToken(); // skip the "{"
		this.parseParameterBlock(cInfo, mInfo);
		return returnStatus;
	}

	public boolean parseField(String filename, Reader reader, ClassInfo cInfo,
			FieldInfo fInfo) throws JavaModelException {
		fileName = filename;
		returnStatus = true;
		reset();
		initSource(reader);
		nextToken(); // skip the "{"
		this.parseParameterBlock(cInfo, fInfo);
		return returnStatus;
	}

	public boolean parse(String filename, Reader reader)
			throws JavaModelException {
		fileName = filename;
		returnStatus = true;
		reset();
		initSource(reader);
		for (Token t = current; t.kind != IlrConstants.EOF; t = current) {
			if (t.kind == IlrConstants.EOF) {
				break;
			}

			if (t.kind != IlrConstants.IDENTIFIER) {
				printError("expecting an identifier");
				skipThisBlock();
				continue;
			}

			if (t.image.equals("variable")) {
				nextToken();
				parseVariable();
			} else if (t.image.equals("keyword")) {
				nextToken();
				parseKeyword();
			} else if (t.image.equals("aliases")) {
				nextToken();
				parseAliases();
			} else if (t.image.equals("package")) {
				nextToken();
				this.parsePackage();
			} else if (t.image.equals("javadoctag")) {
				nextToken();
				parseJavadoctag();
			} else if (t.image.equals("javadoc")) {
				nextToken();
				parseDocumentation();
			} else if (t.image.equals("disclaimer")) {
				this.nextToken();
				this.parseDisclaimer();
			} else {
				printError("expecting keyword 'package'");
				skipThisBlock();
			}
		}
		reset();
		return returnStatus;
	}

	// ------------------------------------------------------------
	// Error management
	// ------------------------------------------------------------

	void printError(String message) {
		returnStatus = false;
		final Token t = current;
		String error = "";
		error += "File " + fileName;
		error += ", line ";
		error += t.beginLine;
		error += ": ";
		error += message;
		error += "\n";
		mInfo.getTranslateInfo().getConfiguration().getLogger().logError(error);
	}

	// ------------------------------------------------------------
	// Parsing methods
	// ------------------------------------------------------------

	Token nextToken() {
		return (current = source.getNextToken());
	}

	void skipThisBlock() {
		for (Token t = nextToken(); t.kind != IlrConstants.EOF; t = nextToken()) {
			if ((t.kind == IlrConstants.IDENTIFIER)
					&& (t.image.equals("method") || t.image.equals("field")
							|| t.image.equals("class")
							|| t.image.equals("package") || t.image
							.equals("variable"))) {
				return;
			}
		}
	}

	void skipClassBlock() {
		for (Token t = nextToken(); t.kind != IlrConstants.EOF; t = nextToken()) {
			if ((t.kind == IlrConstants.IDENTIFIER)
					&& (t.image.equals("class") || t.image.equals("package") || t.image
							.equals("variable"))) {
				return;
			}
		}
	}

	void expectSemiColon() {
		boolean isBlock = (current.kind == IlrConstants.RBRACE);
		final Token t = nextToken();
		if (t.image.equals(";")) {
			nextToken();
		} else if (!isBlock) {
			printError("expecting the terminator ';'");
		}
	}

	void expectSeparator() {
		final Token t = current;
		if (t.image.equals("::")) {
			nextToken();
		} else {
			printError("expecting a mapping sign '::'");
		}
	}

	String parseName() {
		Token t = current;
		if (t.kind != IlrConstants.IDENTIFIER) {
			printError("expecting a valid identifier");
			return null;
		}

		final StringBuffer buffer = new StringBuffer(t.image);
		for (t = nextToken(); (t.kind == IlrConstants.DOT)
				|| (t.kind == IlrConstants.PKG); t = nextToken()) {
			buffer.append(t.image);

			t = nextToken();
			if (t.kind == IlrConstants.IDENTIFIER) {
				buffer.append(t.image);
			} else {
				break;
			}
		}

		if (t.image.equals("?")) {
			buffer.append(t.image);
			nextToken();
		}

		return buffer.toString();
	}

	String parseClassName() {
		Token t = current;
		if (t.kind != IlrConstants.IDENTIFIER) {
			printError("expecting a valid identifier");
			return null;
		}

		final StringBuffer buffer = new StringBuffer(t.image);
		for (t = nextToken(); (t.kind == IlrConstants.DOT)
				|| (t.kind == IlrConstants.PKG) || t.image.equals("<")
				|| t.image.equals(">") || t.image.equals(",")
				|| t.image.equals("%")
				|| t.kind == IlrConstants.INTEGER_LITERAL; t = nextToken()) {
			buffer.append(t.image);

			t = nextToken();
			if (t.kind == IlrConstants.IDENTIFIER) {
				buffer.append(t.image);
			} else if (t.image.equals("%")) {
				buffer.append(t.image);
			} else if (t.image.equals(">")) {
				buffer.append(t.image);
			} else if (t.image.equals(",")) {
				buffer.append(t.image);
			} else if (t.kind == IlrConstants.INTEGER_LITERAL) {
				buffer.append(t.image);
			} else if (t.kind == IlrConstants.DOT) {
				buffer.append(t.image);
				t = nextToken();
				if (t.kind == IlrConstants.IDENTIFIER) {
					buffer.append(t.image);
				}
			} else {
				break;
			}
		}

		if (t.image.equals("?")) {
			buffer.append(t.image);
			nextToken();
		}

		return buffer.toString();
	}

	private int[] parseIndexerGetArguments() throws JavaModelException {
		Token t = current;
		if (t.kind != IlrConstants.LBRACKET) {
			printError("expecting a '['");
			return null;
		}

		StringBuffer buffer = new StringBuffer();
		final List<String> args = new ArrayList<String>();
		for (t = nextToken(); t.kind != IlrConstants.RBRACKET; t = nextToken()) {
			if (t.kind == IlrConstants.COMMA) {
				final String pos = buffer.toString();
				if (!pos.startsWith("@")) {
					printError("expecting a '@'");
					return null;
				} else {
					final String s = pos.substring(1);
					args.add(s);
				}
				buffer = new StringBuffer();
			} else {
				buffer.append(t.image);
			}
		}
		if (buffer.length() > 0) {
			final String pos = buffer.toString();
			if (!pos.startsWith("@")) {
				printError("expecting a '@'");
				return null;
			} else {
				final String s = pos.substring(1);
				args.add(s);
			}
		}

		if (t.kind != IlrConstants.RBRACKET) {
			printError("expecting a ']'");
			return null;
		}

		final int[] res = new int[args.size()];
		for (int i = 0; i < args.size(); i++) {
			res[i] = Integer.parseInt(args.get(i));
		}

		return res;
	}

	private int parseIndexerSetArguments() throws JavaModelException {
		Token t = nextToken();

		if (!t.image.equals("@")) {
			printError("expecting a '@'");
			return -1;
		} else {
			t = nextToken();
			return Integer.parseInt(t.image);
		}
	}

	@SuppressWarnings("unchecked")
	private String[] parseArguments() throws JavaModelException {
		Token t = current;
		if (t.kind != IlrConstants.LPAREN) {
			printError("expecting a '('");
			return null;
		}

		final List<String> args = new ArrayList<String>();
		StringBuffer buffer = new StringBuffer();
		boolean inGenerics = false;
		for (t = nextToken(); t.kind != IlrConstants.RPAREN; t = nextToken()) {
			switch (t.kind) {
			case DOT:
				buffer.append(".");
				break;
			case COMMA:
				if (!inGenerics) {
					final String type = TranslateInfo
							.resolve(buffer.toString());
					args.add(type);
					buffer = new StringBuffer();
				} else {
					buffer.append(t.image);
				}
				break;
			default: {
				if (t.image.equals("<")) {
					inGenerics = true;
					buffer.append(t.image);
					break;
				} else if (t.image.equals(">")) {
					inGenerics = false;
					buffer.append(t.image);
					break;
				} else {
					buffer.append(t.image);
					if (t.image.equals("?") || t.image.equals("extends")
							|| t.image.equals("super")) {
						buffer.append(" ");
					}
				}
			}
			}
		}
		if (!buffer.toString().equals("")) {
			final String type = TranslateInfo.resolve(buffer.toString());
			args.add(type);
		}

		if (t.kind != IlrConstants.RPAREN) {
			printError("expecting a ')'");
			return null;
		}

		return args.toArray(new String[args.size()]);
	}

	String parseSourcePart() {
		Token t = current;
		String res = "";
		while ((t.kind != IlrConstants.MAP) && (t.kind != IlrConstants.ASSIGN)
				&& (t.kind != IlrConstants.LBRACE)
				&& (t.kind != IlrConstants.SEMICOLON)
				&& (t.kind != IlrConstants.EOF)) {
			res += t.image;
			t = nextToken();
		}
		if (t.kind == IlrConstants.EOF) {
			printError("unexpected EOF seen");
			return null;
		}
		return res;
	}

	String parseTargetPart() {
		Token t = current;
		String res = "";
		for (; (t.kind != IlrConstants.SEMICOLON)
				&& (t.kind != IlrConstants.EOF); t = nextToken()) {
			res += t.image;
		}
		if (t.kind == IlrConstants.EOF) {
			printError("unexpected EOF seen");
			return null;
		} else {
			nextToken();
		}
		return res;
	}

	void parseField(ClassInfo cInfo) throws JavaModelException {
		final String name = parseName();

		if (name == null) {
			skipThisBlock();
			return;
		}

		final FieldInfo fInfo = cInfo.resolveField(name);

		if (fInfo == null) {
			printError("Field " + source + " could not be found!");
			skipThisBlock();
			return;
		}

		final Token t = current;
		if (t.kind != IlrConstants.LBRACE) {
			printError("Field: expect '{' !");
			skipThisBlock();
			return;
		}

		nextToken();
		this.parseParameterBlock(cInfo, fInfo);
	}

	void parseMethod(ClassInfo cInfo) throws JavaModelException {
		Token t = current;
		String name = "";
		boolean constructor = false;
		if (t.image.equals("<")) {
			nextToken();
			constructor = true;
			name = "<";
		}
		name += parseName();
		t = current;
		if (constructor) {
			if (t.image.equals(">")) {
				nextToken();
				name += ">";
			} else {
				printError("Method: missing '>' could not be found!");
			}
		}
		final String[] params = parseArguments();

		if (name == null) {
			skipThisBlock();
			return;
		}

		try {

			final MethodInfo mInfo = cInfo.resolveMethod(name, params);
			if (mInfo == null) {
				String strParam = "";
				for (int i = 0; i < params.length; i++) {
					strParam += params[i];
					if (i < params.length - 1)
						strParam += ",";
				}
				printError("Method: " + name + "(" + strParam
						+ ") could not be found!");
				skipThisBlock();
				return;
			}

			t = nextToken();
			if (t.kind != IlrConstants.LBRACE) {
				printError("Method: expect '{' !");
				skipThisBlock();
				return;
			}

			nextToken();
			this.parseParameterBlock(cInfo, mInfo);
		} catch (final Exception e) {
			if (mInfo.getTranslateInfo().getConfiguration().getOptions()
					.getGlobalOptions().isDebug()) {
				e.printStackTrace();
			}
			String strParam = "";
			for (int i = 0; i < params.length; i++) {
				strParam += params[i];
				if (i < params.length - 1)
					strParam += ",";
			}
			printError("Method: " + name + "(" + strParam
					+ ") could not be found!");
			skipThisBlock();
			return;
		}
	}

	protected void parseParameterBlock(ClassInfo cinfo, MethodInfo minfo)
			throws JavaModelException {
		TargetMethod tMethod = new TargetMethod();
		ChangeModifierDescriptor mods = null;
		while (true) {
			Token t = current;
			switch (t.kind) {
			case EOF:
				printError("EOF encountered while parsing a method parameter node");
				return;
			case RBRACE:
				expectSemiColon();
				//if (tMethod != null) {
				String targetFramework = mInfo.getTranslateInfo().getConfiguration().getOptions().getTargetDotNetFramework().name();
					tMethod.setChangeModifierDescriptor(mods);
					minfo.addTargetMethod(targetFramework, tMethod);
				//} 
					/*else {
					if (mods != null) {
						tMethod = new TargetMethod(mods);
						if ((minfo.getTargetMethod() != null)
								&& (minfo.getTargetMethod()
										.getCodeReplacement() != null)) {
							tMethod.setCodeReplacement(minfo.getTargetMethod()
									.getCodeReplacement());
						}
						minfo.setTargetMethod(tMethod);
					}
				}*/
				return;
			case IDENTIFIER:
				if (t.image.equals("constraints")) {
					// need a target method
					t = nextToken();
					if (t.kind == IlrConstants.ASSIGN) {
						t = nextToken();
						final String constraints = parseStringLiteral(t.image);
						tMethod.setConstraints(constraints);
						expectSemiColon();
					} else {
						printError("expecting an assignment with '='");
					}
				} else if (t.image.equals("modifiers")) {
					// need a target method
					t = nextToken();
					if (t.kind == IlrConstants.ASSIGN) {
						nextToken();
						mods = parseModifiers();
						nextToken();
					} else {
						printError("expecting an assignment with '='");
					}
				}// REPLACING BLOCK -->
				else if (t.image.equals("codeReplacement")) {
					// need a target method
					t = this.nextToken();
					if (t.kind == IlrConstants.ASSIGN) {
						t = this.nextToken();
						if (t.image.equalsIgnoreCase("#")) {
							String code = null;
							try {
								code = this.source.getEverythingUntilChar('#');
							} catch (IOException e) {
								this
										.printError("Reading code replacement error "
												+ e.getMessage());
							}
							/*if (minfo.getTargetMethod() == null) {
								tMethod = new TargetMethod();
								tMethod.setCodeReplacement(code);
								minfo.setTargetMethod(tMethod);
							} else {*/
							tMethod
										.setCodeReplacement(code);
							//}

							// this.nextToken();//this reads the '#' from end
							this.expectSemiColon();
						} else {
							this
									.printError("expecting # after 'codeReplacement' but received "
											+ t.kind + "->" + t.image);
						}
					} else {
						this.printError("expecting an assignment with '='");
					}
					// REPLACING BLOCK <--
				} else if (t.image.equals("name")) {
					// need a target method
					t = nextToken();
					if (t.kind == IlrConstants.ASSIGN) {
						nextToken();
						final String name = parseTargetPart();
						/*if (tMethod == null) {
							tMethod = new TargetMethod(name, (int[]) null);
						} else {*/
						tMethod.setName(name);
						//}
						/*if ((minfo.getTargetMethod() != null)
								&& (minfo.getTargetMethod()
										.getCodeReplacement() != null)) {
							tMethod.setCodeReplacement(minfo.getTargetMethod()
									.getCodeReplacement());
						}*/
					} else {
						printError("expecting an assignment with '='");
					}
				}else if (t.image.equals("disable_autoproperty")) {
					minfo.setDisableAutoproperty(true);
					this.expectSemiColon();
				}else if (t.image.equals("property_get")) {
					// need a target method
					t = nextToken();
					if (t.kind == IlrConstants.ASSIGN) {
						nextToken();
						final String pName = parseTargetPart();
						// WARNING
						tMethod = new TargetProperty(pName,
								PropertyRewriter.ProperyKind.READ);
					} else {
						printError("expecting an assignment with '='");
					}
				} else if (t.image.equals("property_set")) {
					// need a target method
					t = nextToken();
					if (t.kind == IlrConstants.ASSIGN) {
						nextToken();
						final String pName = parseTargetPart();
						// WARNING
						tMethod = new TargetProperty(pName,
								PropertyRewriter.ProperyKind.WRITE);
					} else {
						printError("expecting an assignment with '='");
					}
				} else if (t.image.equals("indexer_get")) {
					// need a target method
					t = nextToken();
					if (t.kind == IlrConstants.ASSIGN) {
						nextToken();
						final int[] args = parseIndexerGetArguments();
						expectSemiColon();
						// WARNING
						tMethod = new TargetIndexer(
								IndexerRewriter.IndexerKind.READ, args, -1);
					} else {
						printError("expecting an assignment with '='");
					}
				} else if (t.image.equals("indexer_set")) {
					// need a target method
					t = nextToken();
					if (t.kind == IlrConstants.ASSIGN) {
						nextToken();
						final int[] args_get = parseIndexerGetArguments();
						nextToken();
						final int args_set = parseIndexerSetArguments();
						expectSemiColon();
						// WARNING
						tMethod = new TargetIndexer(
								IndexerRewriter.IndexerKind.WRITE, args_get,
								args_set);
					} else {
						printError("expecting an assignment with '='");
					}
				} else if (t.image.equals("pattern")) {
					// need a target method
					t = nextToken();
					if (t.kind == IlrConstants.ASSIGN) {
						nextToken();
						String pattern = parseTargetPart();
						pattern = pattern.replace("#{", "/* ").replace("#}",
								" */");
						pattern = pattern.replace("#\\\"", "\"");
						/*if (tMethod == null) {
							tMethod = new TargetMethod(pattern);
						} else {*/
							tMethod.getPatternOption().setValue(pattern);
						//}
						/*tMethod = new TargetMethod(pattern);
						if ((minfo.getTargetMethod() != null)
								&& (minfo.getTargetMethod()
										.getCodeReplacement() != null)) {
							tMethod.setCodeReplacement(minfo.getTargetMethod()
									.getCodeReplacement());
						}*/
					} else {
						printError("expecting an assignment with '='");
					}
				} else if (t.image.equals("genericsif")) {
					// need a target method
					t = nextToken();
					if (t.kind == IlrConstants.ASSIGN) {
						nextToken();
						final String genericsTest = parseTargetPart();
						/*if (tMethod == null) {
							tMethod = new TargetMethod();
						} */
						tMethod.setGenericsTest(genericsTest);
					} else {
						printError("expecting an assignment with '='");
					}
				} else if (t.image.equals("generation")) {
					// does not need a target method
					t = nextToken();
					if (t.kind == IlrConstants.ASSIGN) {
						nextToken();
						final Object res = parseValue();
						if (res.equals(Boolean.FALSE)) {
							tMethod.setTranslated(true);
						}
						expectSemiColon();
					} else {
						printError("expecting an assignment with '='");
					}
				} else if (t.image.equals("virtual")) {
					// not used
					t = nextToken();
					if (t.kind == IlrConstants.ASSIGN) {
						nextToken();
						// Object res = parseValue();
						expectSemiColon();
					} else {
						printError("expecting an assignment with '='");
					}
				} else if (t.image.equals("covariant")) {
					// does not need a target method
					t = nextToken();
					if (t.kind == IlrConstants.ASSIGN) {
						nextToken();
						final String res = parseClassName();
						tMethod.getCovariantOption().setValue((String) res);
						cinfo.setCovariantMethod(true);
						// this.expectSemiColon();
						nextToken();
					} else {
						printError("expecting an assignment with '='");
					}
				} else if (t.image.equals("comments")) {
					// not used
					t = nextToken();
					if (t.kind == IlrConstants.ASSIGN) {
						nextToken();
						//Object res = parseValue();
						expectSemiColon();
					} else {
						printError("expecting an assignment with '='");
					}
				} else {
					printError("expecting a 'method' mapping");
					skipThisBlock();
					return;
				}
				break;
			default:
				printError("expecting method parameters");
				skipThisBlock();
				return;
			}
		}
	}

	private ChangeModifierDescriptor parseModifiers() {
		final ChangeModifierDescriptor change = new ChangeModifierDescriptor();
		DotNetModifier modifier = null;
		boolean toAdd = false;
		while (true) {
			final Token t = current;
			switch (t.kind) {
			case EOF:
				printError("EOF encountered while parsing a field parameter node");
				return null;
			case IDENTIFIER:
				modifier = DotNetModifier.fromKeyword(t.image);
				if (modifier == null) {
					printError("Expecting a valid modifier : public, private, protected ...");
					return null;
				}
				nextToken();
				break;
			case COMMA:
				if (toAdd) {
					change.add(modifier);
				} else {
					change.remove(modifier);
				}
				nextToken();
				break;
			case SEMICOLON:
				if (toAdd) {
					change.add(modifier);
				} else {
					change.remove(modifier);
				}
				return change;
			case UNMATCHED:
				if (t.image.equals("+")) {
					toAdd = true;
				} else if (t.image.equals("-")) {
					toAdd = false;
				} else {
					printError("Expecting '+' or '-' for modifiers");
					return null;
				}
				nextToken();
				break;
			}
		}
	}

	private ChangeHierarchyDescriptor parseInterfaces(
			ChangeHierarchyDescriptor change) {
		boolean toAdd = false;
		StringBuilder interf = new StringBuilder();
		while (true) {
			final Token t = current;
			switch (t.kind) {
			case EOF:
				printError("EOF encountered while parsing a field parameter node");
				return null;
			case DOT:
			case PKG:
				interf.append(t.image);
				nextToken();
				break;
			case IDENTIFIER:
				interf.append(t.image);
				nextToken();
				break;
			case COMMA:
				if (toAdd)
					change.addInterface(interf.toString());
				else
					change.removeInterface(interf.toString());
				interf = new StringBuilder();
				nextToken();
				break;
			case SEMICOLON:
				if (toAdd)
					change.addInterface(interf.toString());
				else
					change.removeInterface(interf.toString());
				interf = new StringBuilder();
				return change;
			case UNMATCHED:
				if (t.image.equals("+")) {
					toAdd = true;
				} else if (t.image.equals("-")) {
					toAdd = false;
				} else {
					printError("Expecting '+' or '-' for modifiers");
					return null;
				}
				nextToken();
				break;
			}
		}
	}

	void parseParameterBlock(ClassInfo cInfo, FieldInfo fInfo) {
		TargetField tField = null;
		ChangeModifierDescriptor mods = null;
		String targetFramework = mInfo.getTranslateInfo().getConfiguration().getOptions().getTargetDotNetFramework().name();
		
		while (true) {
			Token t = current;
			switch (t.kind) {
			case EOF:
				printError("EOF encountered while parsing a field parameter node");
				return;
			case RBRACE:
				expectSemiColon();
				
				if (tField != null) {
					if (mods != null)
						tField.setChangeModifierDescriptor(mods);
					fInfo.addTargetField(targetFramework, tField);
				} else {
					if (mods != null) {
						fInfo.addTargetField(targetFramework, new TargetField(mods));
					}
				}
				return;
			case IDENTIFIER:
				if (t.image.equals("type")) {
					t = nextToken();
					if (t.kind == IlrConstants.ASSIGN) {
						nextToken();
						String type = parseTargetPart();
						tField = fInfo.getTarget(targetFramework);
						tField.setReturnType(type);
						//nextToken();
					} else {
						printError("expecting an assignment with '='");
					}
				} else if (t.image.equals("modifiers")) {
					t = nextToken();
					if (t.kind == IlrConstants.ASSIGN) {
						nextToken();
						mods = parseModifiers();
						tField = fInfo.getTarget(targetFramework);
						nextToken();
					} else {
						printError("expecting an assignment with '='");
					}
				} else if (t.image.equals("name")) {
					t = nextToken();
					if (t.kind == IlrConstants.ASSIGN) {
						nextToken();
						final String name = parseTargetPart();
						tField = fInfo.getTarget(targetFramework);
						if (tField == null) {
							tField = new TargetField(name);
						} else {
							tField.setName(name);
							if (mods == null && tField.getRewriter() != null)
								mods = tField.getRewriter().getChangeModifier();
							tField.setRewriter(new FieldRewriter(name));
						}
					} else {
						printError("expecting an assignment with '='");
					}
				} else if (t.image.equals("pattern")) {
					t = nextToken();
					if (t.kind == IlrConstants.ASSIGN) {
						final Token t2 = nextToken();
						String pattern = parseTargetPart();
						pattern = pattern.replace("#{", "/* ").replace("#}",
								" */");
						pattern = pattern.replace("##", "\"");
						// tField = fInfo.getTargetField();
						// if (tField == null) {
						tField = new TargetField(0, pattern);
						// } else {

						// }
					} else {
						printError("expecting an assignment with '='");
					}
				} else if (t.image.equals("generation")) {
					t = nextToken();
					if (t.kind == IlrConstants.ASSIGN) {
						nextToken();
						final Object res = parseValue();
						if (res.equals(Boolean.FALSE)) {
							fInfo.getTarget(targetFramework).setTranslated(true);
						}
						expectSemiColon();
					} else {
						printError("expecting an assignment with '='");
					}
				} else {
					printError("expecting field mapping properties");
					skipThisBlock();
					return;
				}
				break;
			default:
				printError("expecting field parameters");
				skipThisBlock();
				return;
			}
		}
	}

	void parseVariable() {
		final String name = parseName();
		if (name == null) {
			skipThisBlock();
			return;
		}

		if (current.kind == IlrConstants.LBRACE) {
			nextToken();
		} else {
			printError("expecting a block starting with '{'");
			skipThisBlock();
			return;
		}

		final List<String> names = new ArrayList<String>();
		while (true) {
			Token t = current;
			switch (t.kind) {
			case EOF:
				printError("EOF encountered while parsing a mapping node");
				return;
			case RBRACE:
				expectSemiColon();
				mInfo.createVariable(name, names, reference);
				return;
			case IDENTIFIER:
				names.add(t.image);
				t = nextToken();
				if (t.kind == IlrConstants.COMMA) {
					nextToken();
				}
				break;
			default:
				printError("expecting variable replacement names");
				skipThisBlock();
				return;
			}
		}

	}

	void parseJavadoctag() {
		final String name = parseName();
		if (name == null) {
			skipThisBlock();
			return;
		}

		if (current.kind == IlrConstants.ASSIGN) {
			nextToken();
		} else {
			printError("expecting a '='");
			skipThisBlock();
			return;
		}
		
		Token t = current;
		String newName = t.image;
		nextToken();
		
		if (current.kind == IlrConstants.SEMICOLON) {
			nextToken();
		} else {
			printError("expecting a '='");
			skipThisBlock();
			return;
		}
		
		mInfo.addJavaDocMapping(name, newName, null);
	}
	
	void parseKeyword() {
		String name = "";
		Token t = current;
		if (t.kind == IlrConstants.DOT) {
			nextToken();
			name += ".";
		}
		name += parseName();
		if (name == null) {
			skipThisBlock();
			return;
		}

		if (current.kind == IlrConstants.ASSIGN) {
			nextToken();
		} else {
			printError("expecting '='");
			skipThisBlock();
			return;
		}

		t = current;

		if (t.kind == IlrConstants.IDENTIFIER) {
			final String newName = t.image;
			expectSemiColon();
			mInfo.addKeyword(name, newName, reference);
		} else {
			printError("expecting identifier");
			skipThisBlock();
			return;
		}
	}

	private void parseClass(PackageInfo pInfo) throws JavaModelException {
		final String fqname = parseClassName(); // was parseName();
		final boolean partial = false;

		if (fqname == null) {
			skipThisBlock();
			return;
		}

		ClassInfo cinfo = pInfo.getClass(fqname);

		if (cinfo == null) {
			cinfo = pInfo.createClass(fqname, false);
		}

		if (cinfo == null) {
			printError("Class <" + fqname + "> does not exists!");
			//
			skipClassBlock();
			return;
		}

		parseClass(pInfo, cinfo);
	}

	private void parseClass(PackageInfo pInfo, ClassInfo cinfo)
			throws JavaModelException {
		ChangeModifierDescriptor mods = null;
		String targetFramework = mInfo.getTranslateInfo().getConfiguration().getOptions().getTargetDotNetFramework().name();
		
		final ChangeHierarchyDescriptor changeHierarchyDescriptor = new ChangeHierarchyDescriptor();
		if (cinfo.getTarget(targetFramework) == null)
			cinfo.addTargetClass(targetFramework, new TargetClass(cinfo.isMember()));
		cinfo.getTarget(targetFramework).setChangeHierarchyDescriptor(changeHierarchyDescriptor);
		boolean nullable = false;
		boolean partial = false;

		if (current.image.equals("::")) {
			expectSeparator();

			final String targetname = parseClassName();
			if (targetname == null) {
				skipThisBlock();
				return;
			}

			String pname = null;
			if (pInfo.getTarget(targetFramework) != null) {
				pname = pInfo.getTarget(targetFramework).getName();
			}
			String classname = targetname;
			final int index = targetname.indexOf(":");
			if (index != -1) {
				pname = targetname.substring(0, index);
				classname = targetname.substring(index + 1);
			}

			TargetClass tClass = null;
			if (cinfo.getTarget(targetFramework) != null) {
				tClass = cinfo.getTarget(targetFramework);
				tClass.setPackageName(pname);
				tClass.setName(classname);
				//do this only for the configuration file of the current project
				if(!this.fileName.isEmpty() && !this.fileName.contains("FULL") && !isSystemFile){
					if(!cinfo.getTarget(targetFramework).getShortName().equals(TargetClass.shortClassName(cinfo.getName()))){
						this.mInfo.getTranslateInfo().addImplicitNestedRename(cinfo.getTarget(targetFramework).getPackageName(), 
								cinfo.getTarget(targetFramework).getShortName(), cinfo.getPackageName(), TargetClass.shortClassName(cinfo.getName()));				
					}
				}
				tClass.setPartial(partial);
			} else {
				tClass = new TargetClass(pname, classname, null, partial,
						false, false, cinfo.isMember());
				cinfo.addTargetClass(targetFramework, tClass);
			}
			tClass.setSourcePackageName(cinfo.getPackageName());
		}

		if (current.kind == IlrConstants.LBRACE) {
			nextToken();
		} else {
			printError("expecting a block starting with '{'");
			skipThisBlock();
			return;
		}

		while (true) {
			Token t = current;

			switch (t.kind) {
			case EOF:
				printError("EOF encountered while parsing a mapping node");
				return;
			case RBRACE:
				expectSemiColon();
				if (cinfo.getTarget(targetFramework) == null) {
					if (mods != null) {
						cinfo.addTargetClass(targetFramework, new TargetClass(null, null, mods,
								partial, false, nullable, cinfo.isMember()));
					} else if (partial) {
						cinfo.addTargetClass(targetFramework, new TargetClass(null, null,
								new ChangeModifierDescriptor(), partial, false,
								nullable, cinfo.isMember()));
					}
				} else if (mods != null) {
					cinfo.getTarget(targetFramework).setChangeModifierDescriptor(mods);
					cinfo.getTarget(targetFramework).setPartial(partial);
				}
				return;
			case END_FOOTER: {
				nextToken();
				break;
			}
			case IDENTIFIER:
				if (t.image.equals("constraints")) {
					t = nextToken();
					if (t.kind == IlrConstants.ASSIGN) {
						t = nextToken();
						final String constraints = parseStringLiteral(t.image);
						cinfo.getTarget(targetFramework).setConstraints(constraints);
						expectSemiColon();
					} else {
						printError("expecting an assignment with '='");
					}
				}  else if (t.image.equals("codeToAddToImplementation")) {
                    t = this.nextToken();
                    if (t.kind == IlrConstants.ASSIGN) {
                        t = this.nextToken();
                        if(t.image.equalsIgnoreCase( "#" )){
                          String code = null;
                          try {
                            code = this.source.getEverythingUntilChar( '#' );
                          } catch ( IOException e ) {
                            this.printError("Reading code replacement error "+e.getMessage());
                          }
                          cinfo.getTarget(targetFramework).setCodeToAddToImplementation( code );                                                    
                          //this.nextToken();//this reads the '#' from end
                          this.expectSemiColon();
                        } else {
                          this.printError("expecting # after 'codeToAddToImplementation' but received "+t.kind+"->"+t.image);
                        }
                    } else {
                        this.printError("expecting an assignment with '='");
                    } 
				} else if (t.image.equals("instanceOfTypeName")) {
					t = nextToken();
					if (t.kind == IlrConstants.ASSIGN) {
						t = nextToken();
						final String instanceOfTypeName = parseStringLiteral(t.image);
						cinfo.getTarget(targetFramework).setInstanceOfTypeName(
								instanceOfTypeName);
						expectSemiColon();
					} else {
						printError("expecting an assignment with '='");
					}
				} else if (t.image.equals("method")) {
					nextToken();
					this.parseMethod(cinfo);
				} else if (t.image.equals("rename")) {
					t = nextToken();
					if (t.kind == IlrConstants.ASSIGN) {
						nextToken();
						final Object res = parseValue();
						partial = true;
						if (res.equals(Boolean.TRUE)) {
							cinfo.getTarget(targetFramework).setRenamed(true);
						}
						expectSemiColon();
					}
				} else if (t.image.equals("checkReplacement")) {
					t = this.nextToken();
					if (t.kind == IlrConstants.ASSIGN) {
						this.nextToken();
						Object res = this.parseValue();
						partial = true;
						if (res.equals(Boolean.TRUE)) {
							ReplaceClassCall.replaceClassCall(TargetClass.shortClassName(cinfo.getName()), cinfo.getTarget(targetFramework).getShortName());
						}
						this.expectSemiColon();
					}
				} else if (t.image.equals("field")) {
					nextToken();
					this.parseField(cinfo);
				} else if (t.image.equals("generation")) {
					t = nextToken();
					if (t.kind == IlrConstants.ASSIGN) {
						nextToken();
						final Object res = parseValue();
						if (res.equals(Boolean.FALSE)) {
							cinfo.getTarget(targetFramework).setTranslated(true);
						}
						expectSemiColon();
					} else {
						printError("expecting an assignment with '='");
					}
				} else if (t.image.equals("processDoc")) {
					t = nextToken();
					if (t.kind == IlrConstants.ASSIGN) {
						nextToken();
						final Object res = parseValue();
						if (res.equals(Boolean.FALSE)) {
							cinfo.getTarget(targetFramework).setProcessDoc(false);
						} else if (res.equals(Boolean.TRUE)) {
							cinfo.getTarget(targetFramework).setProcessDoc(true);
						}
						expectSemiColon();
					} else {
						printError("expecting an assignment with '='");
					}
				} else if (t.image.equals("remove")) {
					t = nextToken();
					if (t.kind == IlrConstants.ASSIGN) {
						nextToken();
						final Object res = parseValue();
						if (res.equals(Boolean.TRUE)) {
							cinfo.getTarget(targetFramework).setTranslated(true);
						}
						expectSemiColon();
					} else {
						printError("expecting an assignment with '='");
					}
				} else if (t.image.equals("removeGenerics")) {
					t = nextToken();
					if (t.kind == IlrConstants.ASSIGN) {
						nextToken();
						final Object res = parseValue();
						if (res.equals(Boolean.TRUE)) {
							cinfo.getTarget(targetFramework).setRemoveGenerics(true);
						}
						expectSemiColon();
					} else {
						printError("expecting an assignment with '='");
					}
				} else if (t.image.equals("nullable")) {
					t = nextToken();
					if (t.kind == IlrConstants.ASSIGN) {
						nextToken();
						final Object res = parseValue();
						if (res.equals(Boolean.TRUE)) {
							nullable = true;
							cinfo.getTarget(targetFramework).setNullable(true);
						} else {
							nullable = false;
							cinfo.getTarget(targetFramework).setNullable(false);
						}
						expectSemiColon();
					} else {
						printError("expecting an assignment with '='");
					}
				} else if (t.image.equals("modifiers")) {
					t = nextToken();
					if (t.kind == IlrConstants.ASSIGN) {
						nextToken();
						mods = parseModifiers();
						nextToken();
					} else {
						printError("expecting an assignment with '='");
					}
				} else if (t.image.equals("explicitInterfaceMethods")) {
					t = nextToken();
					if (t.kind == IlrConstants.ASSIGN) {
						nextToken();
						final String ifaceName = parseTargetPart();
						cinfo.addExplicitInterfaceMethods(ifaceName);
					}
				} else if (t.image.equals("nestedToInner")) {
					t = nextToken();
					if (t.kind == IlrConstants.ASSIGN) {
						nextToken();
						final Object res = parseValue();
						if (res.equals(Boolean.TRUE)) {
							cinfo.getTarget(targetFramework).setNestedToInner(true);
							expectSemiColon();
						}
					}
				} else if (t.image.equals("partial")) {
					t = nextToken();
					if (t.kind == IlrConstants.ASSIGN) {
						nextToken();
						final Object res = parseValue();
						partial = true;
						if (res.equals(Boolean.TRUE)) {
							cinfo.getTarget(targetFramework).setPartial(partial);
						}
						expectSemiColon();
					}
				} else if (t.image.equals("memberMappingBehavior")) {
					t = nextToken();
					if (t.kind == IlrConstants.ASSIGN) {
						nextToken();
						final String res = parseName();
						if (res.equals("none")) {
							cinfo.getTarget(targetFramework)
									.setMemberMappingBehavior(MethodMappingPolicy.NONE);
						}
						if (res.equals("capitalized")) {
							cinfo.getTarget(targetFramework)
									.setMemberMappingBehavior(MethodMappingPolicy.CAPITALIZED);
						}
						nextToken();
					}
				} else if (t.image.equals("superClass")) {
					t = nextToken();
					if (t.kind == IlrConstants.ASSIGN) {
						nextToken();
						final String res = parseName();
						cinfo.getTarget(targetFramework).getChangeHierarchyDescriptor().setSuperClass(res);
						nextToken();
					}
				} else if (t.image.equals("interfaces")) {
					t = nextToken();
					if (t.kind == IlrConstants.ASSIGN) {
						nextToken();
						parseInterfaces(changeHierarchyDescriptor);
						nextToken();
					}
				} else if (t.image.equals("removeStaticInitializers")) {
					t = nextToken();
					if (t.kind == IlrConstants.ASSIGN) {
						nextToken();
						final Object res = parseValue();
						if (res.equals(Boolean.TRUE)) {
							cinfo.getTarget(targetFramework).setRemoveStaticInitializers(true);
						}
						expectSemiColon();
					} else {
						printError("expecting an assignment with '='");
					}
				}
				// ADD OR REMOVE USING -->
				else if (t.image.equals("remove_using")) {
					t = this.nextToken();
					if (t.kind == IlrConstants.ASSIGN) {
						String code = null;
						try {
							code = this.source.getEverythingUntilChar(';');
						} catch (IOException e) {
							this.printError("Reading remove_using error "
									+ e.getMessage());
						}
						/*cinfo.getTargetClass().getRemoveUsing()
								.add(code.trim());*/
						cinfo.getTarget(targetFramework).getChangeUsingDescriptor().setUsingToRemove(code.trim().split(","));
						this.nextToken();
					} else {
						this.printError("expecting an assignment with '='");
					}
				} else if (t.image.equals("add_using")) {
					t = this.nextToken();
					if (t.kind == IlrConstants.ASSIGN) {
						String code = null;
						try {
							code = this.source.getEverythingUntilChar(';');
						} catch (IOException e) {
							this.printError("Reading add_using error "
									+ e.getMessage());
						}
						//cinfo.getTargetClass().getAddUsing().add(code.trim());
						cinfo.getTarget(targetFramework).getChangeUsingDescriptor().setUsingToAdd(code.trim().split(","));
						this.nextToken();
					} else {
						this.printError("expecting an assignment with '='");
					}
				}
				// ADD OR REMOVE USING <--
				else {
					if (t.image.equals("class")) {
						return;
					}
					printError("expecting a 'method' mapping");
					skipThisBlock();
					return;
				}
				break;
			default:
				printError("expecting a 'method' mapping");
				skipThisBlock();
				return;
			}
		}
	}

	void parsePackage() throws JavaModelException {
		final String name = parseName();
		if (name == null) {
			skipThisBlock();
			return;
		}
		String targetFramework = mInfo.getTranslateInfo().getConfiguration().getOptions().getTargetDotNetFramework().name();
		
		// Check if packageinfo already exists
		PackageInfo pInfo = mInfo.getPackage(name, reference, true);
		if (pInfo == null) {
			pInfo = mInfo.createPackage(name, reference);
			pInfo.addTarget(targetFramework, new TargetPackage());
		} else {
			boolean b = mInfo.getTranslateInfo().getConfiguration().getOptions().useIsolationForMapping();
			if (b && (pInfo.getReference() != reference)) {
				pInfo = mInfo.createPackage(name, reference);
				pInfo.addTarget(targetFramework, new TargetPackage());
			}
		}
		
		if (pInfo == null) {
			printError("package " + name + " not found. Ignore it.");
			skipThisBlock();
			return;
		}

		if (current.image.equals("::")) {
			expectSeparator();

			final String targetname = parseName();
			if (targetname == null) {
				skipThisBlock();
				return;
			}

			// String pname = null;
			String classname = targetname;
			final int index = targetname.indexOf(":");
			if (index != -1) {
				// String pname = targetname.substring(0, index);
				classname = targetname.substring(index + 1);
			}
			pInfo.getTarget(targetFramework).setName(classname);
			/*
			pInfo.setTarget(new TargetPackage(classname, mInfo
					.getKeyword("package")));
					*/
		}

		if (current.kind == IlrConstants.LBRACE) {
			nextToken();
		} else {
			printError("expecting a block starting with '{'");
			skipThisBlock();
			return;
		}

		while (true) {
			Token t = current;
			switch (t.kind) {
			case EOF:
				printError("EOF encountered while parsing a 'package' node");
				return;
			case RBRACE:
				expectSemiColon();
				return;
			case IDENTIFIER:
				if (t.image.equals("generation")) {
					t = nextToken();
					if (t.kind == IlrConstants.ASSIGN) {
						nextToken();
						final Object res = parseValue();
						if (res.equals(Boolean.FALSE)) {
							pInfo.getTarget(targetFramework).setTranslated(true);
						} else {
							pInfo.getTarget(targetFramework).setTranslated(false);
						}
						expectSemiColon();
					} else {
						printError("expecting an assignment with '='");
					}
				} else if (t.image.equals("class")) {
					nextToken();
					try {
						this.parseClass(pInfo);
					} catch (final JavaModelException e) {
						skipThisBlock();
					}
				} else if (t.image.equals("package")) {
					// case of error during parsing
					nextToken();
					parsePackage();
					return;
				} else if (t.image.equals("packageMappingBehavior")) {
					// case of error during parsing
					t = nextToken();
					if (t.kind == IlrConstants.ASSIGN) {
						nextToken();
						final Object res = parseName();
						if (res.equals("none")) {
							pInfo.getTarget(targetFramework)
									.setPackageMappingBehavior(PackageMappingPolicy.NONE);
						}
						nextToken();
					} else {
						printError("expecting an assignment with '='");
					}
				} else if (t.image.equals("memberMappingBehavior")) {
					// case of error during parsing
					t = nextToken();
					if (t.kind == IlrConstants.ASSIGN) {
						nextToken();
						final Object res = parseName();
						if (res.equals("none")) {
							pInfo.getTarget(targetFramework)
									.setMemberMappingBehavior(MethodMappingPolicy.NONE);
						}
						nextToken();
					} else {
						printError("expecting an assignment with '='");
					}
				} else {
					printError("expecting 'package' properties here");
					skipThisBlock();
					return;
				}
				break;
			default:
				printError("expecting a 'package' property keyword.");
				skipThisBlock();
				return;
			}
		}
	}

	void parseDisclaimer() throws JavaModelException {
		Token t = this.current;
		if (t.kind == IlrConstants.ASSIGN) {
			t = this.nextToken();
			if (t.image.equalsIgnoreCase("#")) {
				String code = "";
				try {
					code = this.source.getEverythingUntilChar('#') + "\n";
				} catch (IOException e) {
					this.printError("Reading disclaimer error "
							+ e.getMessage());
				}
				mInfo.setDisclaimer(code);
				this.expectSemiColon();
			} else {
				this.printError("expecting # after 'disclaimer' but received "
						+ t.kind + "->" + t.image);
			}
		} else {
			this.printError("expecting an assignment with '='");
		}
	}

	Object parseValue() {
		final Token t = current;
		switch (t.kind) {
		case TRUE:
			return Boolean.TRUE;
		case FALSE:
			return Boolean.FALSE;
		case STRING_LITERAL:
			return parseStringLiteral(t.image);
			/*
			 * case CHARACTER_LITERAL: return parseCharacterLiteral(t.image);
			 * case INTEGER_LITERAL: return parseIntegerLiteral(t); case
			 * FLOATING_POINT_LITERAL: return parseFloatLiteral(t); case LBRACE:
			 * return parseArray();
			 */
		default:
			printError("expecting a valid value");
			return null;
		}
	}

	private String parseStringLiteral(String str) {
		final int len = str.length();
		final StringBuffer buffer = new StringBuffer(len);
		char input;
		int index = 0;
		while (index < len) {
			input = str.charAt(index++);
			if (input != '\\') {
				buffer.append(input);
				continue;
			}

			// Now, we have an escape sequence character!
			input = str.charAt(index++);
			switch (input) {
			case 'n':
				buffer.append('\n');
				break;
			case 't':
				buffer.append('\t');
				break;
			case 'b':
				buffer.append('\b');
				break;
			case 'r':
				buffer.append('\r');
				break;
			case 'f':
				buffer.append('\f');
				break;
			case '\\':
				buffer.append('\\');
				break;
			case '\'':
				buffer.append('\'');
				break;
			case '\"':
				buffer.append('\"');
				break;
			}
		}
		return buffer.toString();
	}

	abstract class RightPartElem {
		static final int STRING_LITERAL = 0;

		static final int REF = 1;

		int kind = -1;
	}

	class ArgsRef extends RightPartElem {
		public int ref;

		public ArgsRef(String ref) {
			this.ref = new Integer(ref.substring(1)).intValue();
			kind = RightPartElem.REF;
		}
	}

	class StringLiteral extends RightPartElem {
		public String value;

		public StringLiteral(String value) {
			this.value = value;
			kind = RightPartElem.STRING_LITERAL;
		}
	}

	public class DocTranslator {
		int arg; // number of arg needed

		String name = null;

		RightPartElem[] rightPartElems;

		public DocTranslator(String name, int arg, RightPartElem[] rightPart) {
			this.name = name;
			this.arg = arg;
			rightPartElems = rightPart;
		}

		public String translate(String[] params) {
			if (params.length != arg) {
				System.err.println("DocTranslator : bad number of arg ("
						+ params.length + ") for " + name + " [" + arg + "]");
				return null;
			} else {
				String result = "";
				for (final RightPartElem elem : rightPartElems) {
					switch (elem.kind) {
					case RightPartElem.STRING_LITERAL:
						result += ((StringLiteral) elem).value;
						break;
					case RightPartElem.REF:
						final int pos = ((ArgsRef) elem).ref - 1;
						result += params[pos];
						break;
					default:
						//
					}
				}
				return result;
			}
		}

		//
		// Shortcut
		//

		public String translate(String param) {
			return this.translate(new String[] { param });
		}

		public String translate(String param0, String param1) {
			return this.translate(new String[] { param0, param1 });
		}
	}
}
