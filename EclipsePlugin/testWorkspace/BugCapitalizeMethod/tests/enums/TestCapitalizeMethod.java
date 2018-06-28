package tests.enums;

import java.util.EnumSet;

public class TestCapitalizeMethod {
	public static void createPlaceHolder(IlrSyntaxTree.Node roleNode,
			Object placeHolderConcept) {
		if (roleNode != null) {
			IlrBRLGrammar.Node grammarNode = roleNode.getGrammarNode();
			IlrSyntaxTree.Node placeHolderNode = roleNode.getSyntaxTree().new Node(
					grammarNode);
			//placeHolderNode.setPlaceHolder(placeHolderConcept);
			roleNode.replaceBy(placeHolderNode);
		}
	}
}
