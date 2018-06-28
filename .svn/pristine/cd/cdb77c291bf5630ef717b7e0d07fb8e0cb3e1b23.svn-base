package com.ilog.translator.java2cs.translation.textrewriter;

import java.util.ArrayList;
import java.util.List;
import java.util.regex.Matcher;
import java.util.regex.Pattern;

import org.eclipse.core.runtime.CoreException;
import org.eclipse.core.runtime.IProgressMonitor;
import org.eclipse.jdt.core.IBuffer;
import org.eclipse.jdt.core.dom.ASTNode;
import org.eclipse.text.edits.ReplaceEdit;
import org.eclipse.text.edits.TextEdit;

import com.ilog.translator.java2cs.translation.ITranslationContext;
import com.ilog.translator.java2cs.translation.util.ReplaceClassCall;

/**
 * Used for double checking the replacement in case of class renaming.
 * @author nrrg
 *
 */
@SuppressWarnings("restriction")
public class ReplacingClassCallTransformer extends TextRewriter {

	protected int pos = 1;

	private static String  allowedSeparators="\\b";
	//private static Pattern pattern = Pattern.compile(allowedSeparators);

	//
	//
	//	

	public ReplacingClassCallTransformer(ITranslationContext context) {
		super(context);
		this.transformerName = "Replace class call ";
	}

	//
	//
	//

	@Override
	public boolean transform(IProgressMonitor pm, ASTNode cunit) {
		// do nothing
		return true;
	}
	
	@Override
	public List<TextEdit> computeEdit(IProgressMonitor pm, IBuffer buffer)
	throws CoreException {
		List<TextEdit> edits = new ArrayList<TextEdit>();
		
		for(String key : ReplaceClassCall.getReplacedCallKeys()){
			// int i = 0 ;
			String replacingCall = ReplaceClassCall.getReplacingCall(key);
			Pattern pattern = Pattern.compile(allowedSeparators+key+allowedSeparators);
			Matcher matcher = pattern.matcher(buffer.getContents());
			while(matcher.find()) {
				edits.add(new ReplaceEdit(matcher.start(), key.length(), replacingCall));
			}
		}
		return edits;
	}

}
