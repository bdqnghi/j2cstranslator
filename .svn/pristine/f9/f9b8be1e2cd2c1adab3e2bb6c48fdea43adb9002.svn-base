package com.ilog.translator.java2cs.translation.astrewriter;

import java.util.List;

import org.eclipse.jdt.core.dom.ASTNode;
import org.eclipse.jdt.core.dom.MethodDeclaration;
import org.eclipse.jdt.core.dom.Modifier;
import org.eclipse.jdt.core.dom.TypeDeclaration;
import org.eclipse.text.edits.TextEditGroup;

import com.ilog.translator.java2cs.translation.ITranslationContext;
import com.ilog.translator.java2cs.translation.util.TranslationUtils;
import com.ilog.translator.java2cs.util.TranslationModelUtil;

public class FillAnnotationsVisitor extends ASTRewriterVisitor {

	private boolean isATestCase = false;

	//
	//
	//

	public FillAnnotationsVisitor(ITranslationContext context) {
		super(context);
		transformerName = "Fill annotations";
		description = new TextEditGroup(transformerName);
	}

	//
	//
	//

	@Override
	public boolean isAbridged() {
		return true;
	}

	//
	//
	//

	@SuppressWarnings("unchecked")
	@Override
	public void endVisit(MethodDeclaration node) {
		if (isATestCase) {
			if (isATestmethod(node)) {
				if (context.getConfiguration().getOptions().isForceJUnitTest()) {
					final List params = node.parameters();
					for (int i = 0; i < params.size(); i++) {
						currentRewriter.remove((ASTNode) params.get(i),
								description);
					}
					final List mods = node.modifiers();
					for (int i = 0; i < mods.size(); i++) {
						final ASTNode mod = (ASTNode) mods.get(i);
						if (mod.getNodeType() == ASTNode.MODIFIER) {
							final Modifier m = (Modifier) mod;
							if (m.getKeyword().toFlagValue() == Modifier.STATIC)
								currentRewriter.remove(m, description);
						}
					}
				}
				final String categorie = TranslationUtils.getTagValueFromDoc(
						node, context.getMapper().getTag(
								TranslationModelUtil.TESTCATEGORIE_TAG));
				String categorie_s = null;
				if (categorie != null && categorie.trim().equals("Ignore")) {
					categorie_s = "/* insert_here:[NUnit.Framework.Ignore]";
				} else if (categorie != null && !categorie.equals("")) {
					categorie_s = "/* insert_here:[NUnit.Framework.Category(\""
							+ categorie.trim() + "\")]";
				} else {
					categorie_s = "/* insert_here:";
				}
				TranslationUtils.createAnnotation(currentRewriter, node,
						categorie_s + "[NUnit.Framework.Test] */");
			}
			if (node.getName().getIdentifier().equals("SetUp")
					|| TranslationUtils.containsTag(node, context.getMapper()
							.getTag(TranslationModelUtil.TESTBEFORE_TAG))) {
				TranslationUtils.createAnnotation(currentRewriter, node,
						"/* insert_here:[NUnit.Framework.SetUp] */");
				// TODO: remove override
			} else if (node.getName().getIdentifier().equals("TearDown")
					|| TranslationUtils.containsTag(node, context.getMapper()
							.getTag(TranslationModelUtil.TESTAFTER_TAG))) {
				TranslationUtils.createAnnotation(currentRewriter, node,
						"/* insert_here:[NUnit.Framework.TearDown] */");
				// TODO: remove override
			}
		}
	}

	@Override
	public boolean visit(TypeDeclaration node) {
		if (TranslationUtils.containsTag(node, context.getMapper().getTag(
				TranslationModelUtil.TESTCASE_TAG))) {
			isATestCase = true;
			if (!node.resolveBinding().isInterface()) {
				final String categorie = TranslationUtils.getTagValueFromDoc(
						node, context.getMapper().getTag(
								TranslationModelUtil.TESTCATEGORIE_TAG));
				String categorie_s = null;
				if (categorie != null && categorie.trim().equals("Ignore")) {
					categorie_s = "/* insert_here:[NUnit.Framework.Ignore]";
				} else if (categorie != null && !categorie.equals("")) {
					categorie_s = "/* insert_here:[NUnit.Framework.Category(\""
							+ categorie.trim() + "\")]";
				} else {
					categorie_s = "/* insert_here:";
				}
				TranslationUtils.createAnnotation(currentRewriter, node,
						categorie_s + "[NUnit.Framework.TestFixture] */");
			}
		}
		return true;
	}

	//
	//
	//

	private boolean isATestmethod(MethodDeclaration node) {
		return TranslationUtils.containsTag(node, context.getMapper().getTag(
				TranslationModelUtil.TESTMETHOD_TAG));
	}
}
