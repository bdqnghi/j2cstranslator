package com.ilog.translator.java2cs.translation.textrewriter;

import java.util.ArrayList;
import java.util.List;

import org.eclipse.core.runtime.CoreException;
import org.eclipse.core.runtime.IProgressMonitor;
import org.eclipse.jdt.core.IBuffer;
import org.eclipse.jdt.core.dom.ASTNode;
import org.eclipse.text.edits.DeleteEdit;
import org.eclipse.text.edits.ReplaceEdit;
import org.eclipse.text.edits.TextEdit;

import com.ilog.translator.java2cs.translation.ITranslationContext;
import com.ilog.translator.java2cs.util.IKeywordConstants;

public class KeywordsTransformer extends TextRewriter {

	protected int pos = 1;

	//
	//
	//	

	public KeywordsTransformer(ITranslationContext context) {
		super(context);
		transformerName = "Keywords transformer";
	}

	//
	//
	//

	@Override
	public boolean transform(IProgressMonitor pm, ASTNode cunit) {
		// do nothing
		return true;
	}

	//
	//
	//

	@Override
	public List<TextEdit> computeEdit(IProgressMonitor pm, IBuffer buffer)
			throws CoreException {
		final List<TextEdit> edits = new ArrayList<TextEdit>();
		boolean extends_b = false;
		// TODO : use mapper to retrieve that strings
		final String extends_k = IKeywordConstants.JAVA_EXTENDS_KEYWORD; // "extends";
		final String implements_k = IKeywordConstants.JAVA_IMPLEMENTS_KEYWORD; // "implements";
		final String class_k = IKeywordConstants.CLASS_KEYWORD; // " class ";
		final String foreach_k = "/* foreach */";
		final String instanceof_c = "/* instanceof"; // " */";
		final String instanceof_k = "instanceof";
		final String default_C = "/* default(";
		final String for_k = "for";
		final String typeof_C = "/* typeof(";
		final String done_k = "/* j2cs:done */";

		final int extends_l = extends_k.length();
		final int implements_l = implements_k.length();
		final int foreach_l = foreach_k.length();
		final int instanceof_lc = instanceof_c.length();
		final int instanceof_lk = instanceof_k.length();
		final int for_l = for_k.length();
		final int default_lC = default_C.length();
		final int typeof_lC = typeof_C.length();
		final int done_l = done_k.length();

		final int size = buffer.getLength() - 2 /* implements_l */;

		// 

		try {
			if (pos > -1) {
				// First pass on insert to avoid modification overlap ...
				final List<String> allowedAfterKeyword = new ArrayList<String>();
				allowedAfterKeyword.add(" ");
				allowedAfterKeyword.add(":");
				allowedAfterKeyword.add("\n");
				allowedAfterKeyword.add("\r");
				allowedAfterKeyword.add("\t");
				allowedAfterKeyword.add("(");
				allowedAfterKeyword.add("/");
				//
				final List<String> allowedBeforeKeyword = new ArrayList<String>();
				allowedBeforeKeyword.add(" ");
				allowedBeforeKeyword.add("\n");
				allowedBeforeKeyword.add("\r");
				allowedBeforeKeyword.add("\t");
				// Then other replacement ...
				boolean seenStartComment = false;
				boolean seenEndComment = false;
				boolean isInComments = false;
				for (int i = pos; i < size; i++) {
					
					//char currentChar = buffer.getChar(i);
					
					// A way to say "we are in comments"
					// to avoid replace implements and extends keywords in comments.
					seenStartComment = seenStartComment || isStartDOTNETComments(buffer, i, size);// || isStartComments(buffer, i, size); 
					seenEndComment = isEndDOTNETComments(buffer, i, size); // || isEndComments(buffer, i, size);
					isInComments = seenStartComment && !seenEndComment;
					if (seenEndComment)
						seenStartComment = !seenEndComment;				
					
					if (!isInComments) {
						// extends
						if (searchForSpecial(extends_k, size, buffer, i,
							allowedBeforeKeyword, allowedAfterKeyword)) {
							edits.add(new ReplaceEdit(i, extends_l + 2, " : "));
							extends_b = true;
							i += extends_l + 2;
						}
						// implements
						else if (searchForSpecial(implements_k, size, buffer, i,
							allowedBeforeKeyword, allowedAfterKeyword)) {
							if (extends_b) {
								edits
										.add(new ReplaceEdit(i, implements_l + 2,
												", "));
							} else {
								edits.add(new ReplaceEdit(i, implements_l + 2,
										" : "));
							}
							i += implements_l + 2;
						}
						// class
						else if (searchForSpecial(class_k, size, buffer, i,
								allowedBeforeKeyword, allowedAfterKeyword)) {
							// a new class as started
							// TODO : to be improved !
							extends_b = false;
						}
						// done
						else if (searchFor(done_k, size, buffer, i,
								allowedAfterKeyword)) {
							edits.add(new ReplaceEdit(i, done_l + 1, ""));
						}
						// foreach
						else if (searchFor(foreach_k, size, buffer, i,
								allowedAfterKeyword)) {
							boolean ff = false;
							boolean end = false;
							final int dec = skipComments(size, buffer, i
									+ foreach_l, allowedAfterKeyword);
							// int start_for = -1;
							for (int j = dec /* + foreach_l */; !end; j++) {
								if (j + for_l < size
										&& buffer.getText(j, for_l).equals(for_k)) {
									// char after "for" must be " " or "(" or a
									// comments
									// if not we try to replace the three letter "f"
									// "o" "r"
									// in a word that contains them.
									final String nextChar = buffer.getText(j
											+ for_l, 1);
									if (nextChar.equals(" ")
											|| nextChar.equals("(")
											|| nextChar.equals("/")) {
										edits.add(new ReplaceEdit(j, for_l,
												"foreach"));
										ff = true;
									}
									// start_for = j;
								}
								if (ff && j + 1 < size
										&& buffer.getText(j, 1).equals(":")) {
									edits.add(new ReplaceEdit(j, 1, " in "));
									// edits.add(new DeleteEdit(i, start_for - i));
									end = true;
								}
							}
						}
						// instanceof
						else if (searchFor(instanceof_c, size, buffer, i, null)) {
							boolean end = false;
							int j = i + instanceof_lc;
							//				
							for (; !buffer.getText(j, 2).equals("*/"); j++) {
							}
							final int lengthOfRestOfComment = j
									- (i + instanceof_lc);
							final String genericType = buffer.getText(
									i + instanceof_lc, lengthOfRestOfComment)
									.trim();
							//
							for (; !end; j++) {
								if (j + for_l < size
										&& buffer.getText(j, instanceof_lk).equals(
												instanceof_k)) {
									edits.add(new DeleteEdit(i, instanceof_lc
											+ lengthOfRestOfComment + 2)); // remove
									// the
									// comment
									edits.add(new ReplaceEdit(j, instanceof_lk,
											" is ")); // replace
									// the
									// keyword
									end = true;
								}
							}
							//					
							if (!genericType.equals("")) {
								end = false;
								for (; !end; j++) {
									if (j < size
											&& buffer.getText(j, 3).equals("<?>")) {
										edits.add(new ReplaceEdit(j + 1, 1,
												genericType));
										end = true;
									}
								}
							}
						}
						// default
						else if (searchFor(default_C, size, buffer, i, null)) {
							// Ok, we found "/* default("
							// we know look for "*/"
							boolean end = false;
							for (int j = i + default_lC; !end; j++) {
								if (j + " */".length() < size
										&& buffer.getText(j, " */".length())
												.equals(" */")) {
									end = true;
									final String newValue = /* "return " + */
									buffer.getText(i + 3, j - i - 3)
											+ "/* was: null */";
									/* + ";"; */
									// edits.add(new DeleteEdit(i, j - i + 3)); //
									// remove
									// the
									// comment
									edits.add(new ReplaceEdit(i, j - i + 3 + 1
											+ /* " return */"null" /* ";" */
											.length(), newValue)); // replace
									// the
									// keyword
								}
							}
						}
						// typeof
						else if (searchFor(typeof_C, size, buffer, i, null)) {
							// Ok, we found "/* typeof("
							// we now look for "*/"
							final int start_comment = i;
							int end_comment = i;
							int end_typeof = i;
							boolean end = false;
							for (int j = i + typeof_lC; !end; j++) {
								if (j + " */".length() < size
										&& buffer.getText(j, " */".length())
												.equals(" */")) {
									end = true;
									end_comment = j;
									// Remove comment starting at i with (j - i +3)
									// length
									// Remove all until we found ".class"
									// Insert what we extract from comment
								}
							}
							end = false;
							final int length = buffer.getLength();
							for (int j = end_comment; !end; j++) {
								if (j + ".class".length() < length
										&& buffer.getText(j, ".class".length())
												.equals(".class")) {
									end = true;
									end_typeof = j + 6;
								}
							}
							//
							final String typeofexpr = buffer.getText(
									start_comment + 3, end_comment - start_comment
											- 3);
							edits.add(new ReplaceEdit(start_comment, end_typeof
									- start_comment, typeofexpr));
						}
	
					}
				}
			}
		} catch (final Exception e) {
			context.getLogger().logException(
					"Exception during keywords replacement in "
							+ fCu.getElementName(), e);
			e.printStackTrace();
		}

		return edits;
	}

	//
	//
	//

	private boolean isEndDOTNETComments(IBuffer buffer, int i, int size) {
		char c = buffer.getChar(i);
		return c == '\n';
	}

	private boolean isStartDOTNETComments(IBuffer buffer, int i, int size) {
		if (i < size - 3) {
			String text = buffer.getText(i, 3);
			return "///".equals(text);
		} else
		return false;
	}

	private boolean isEndComments(IBuffer buffer, int i, int size) {
		if (i < size - 2) {
			String text = buffer.getText(i, 2);
			return "*/".equals(text);
		} else
			return false;
	}

	private boolean isStartComments(IBuffer buffer, int i, int size) {
		if (i < size - 2) {
			String text = buffer.getText(i, 2);
			return "/*".equals(text);
		} else
			return false;
	}
		
	private int skipComments(int size, IBuffer buffer, int i,
			List<String> allowedafter) {
		int indx = i;
		while (indx < size && allowedafter.contains(buffer.getText(indx, 1)))
			indx++;
		if (buffer.getText(indx - 1, 2).equals("/*")) {
			for (int cpt = indx - 1 + 2; cpt < size; cpt++) {
				if (buffer.getText(cpt, 2).equals("*/"))
					return cpt;
			}
		}
		return i;
	}

	private boolean searchForSpecial(String pattern, int sizeOfbuffer,
			IBuffer buffer, int pos, List<String> allowedbefore,
			List<String> allowedafter) {
		final int patternLength = pattern.length() + 2;
		if ((pos + patternLength < sizeOfbuffer)
				&& (allowedbefore == null ? true : isIn(buffer.getText(pos, 1),
						allowedbefore))
				&& buffer.getText(pos + 1, patternLength - 2).equals(pattern)) {
			return (allowedafter == null ? true : isIn(buffer.getText(pos
					+ patternLength - 1, 1), allowedafter));
		} else
			return false;
	}

	private boolean searchFor(String pattern, int sizeOfbuffer, IBuffer buffer,
			int pos, List<String> allowedafter) {
		final int patternLength = pattern.length();
		if ((pos + patternLength < sizeOfbuffer)
				&& buffer.getText(pos, patternLength).equals(pattern)) {
			return (allowedafter == null ? true : isIn(buffer.getText(pos
					+ patternLength, 1), allowedafter));
		} else
			return false;
	}

	//
	//
	//

	private boolean isIn(String text, List<String> allowed) {
		return allowed.contains(text);
	}
}
