package com.ilog.translator.java2cs.translation.astrewriter;

import java.util.Iterator;
import java.util.List;

import org.eclipse.jdt.core.dom.ASTNode;
import org.eclipse.jdt.core.dom.Block;
import org.eclipse.jdt.core.dom.EnumDeclaration;
import org.eclipse.jdt.core.dom.Javadoc;
import org.eclipse.jdt.core.dom.MethodDeclaration;
import org.eclipse.jdt.core.dom.Modifier;
import org.eclipse.jdt.core.dom.SimpleName;
import org.eclipse.jdt.core.dom.SingleVariableDeclaration;
import org.eclipse.jdt.core.dom.Type;
import org.eclipse.jdt.internal.corext.dom.ASTNodeFactory;
import org.eclipse.text.edits.TextEditGroup;

import com.ilog.translator.java2cs.translation.ITranslationContext;

/**
 * Prepare instance methods from enumerations to be transformed in extension methods:
 * Copy the instance method and add to the new method a parameter of type enumeration 
 * @author nrrg
 *
 */
@SuppressWarnings("restriction")
public class PrepareExtensionMethodsForEnum1 extends ASTRewriterVisitor {
	
	public static final String PARAMETER_NAME = "extensionParam";
	
	public PrepareExtensionMethodsForEnum1(ITranslationContext context) {
		super(context);
		transformerName = "Prepare Extension Methods For Transforming Enums - create new method with enumeration parameter";
		description = new TextEditGroup(transformerName);
	}

	@Override
	public void endVisit(EnumDeclaration node) {
		for (final Iterator iter = node.bodyDeclarations().iterator(); iter.hasNext();) {
			final ASTNode astNode = (ASTNode) iter.next();
			if (astNode.getNodeType() == ASTNode.METHOD_DECLARATION) {
				final MethodDeclaration mDecl = (MethodDeclaration) astNode;
				if (!mDecl.isConstructor()) {
					//Only instance methods can be transformed in extension methods
					if(!Modifier.isStatic(mDecl.getModifiers())){
						MethodDeclaration newMethod = this.createExtensionMethodSignature(mDecl, node.getName().getFullyQualifiedName());
						currentRewriter.getListRewrite(node, EnumDeclaration.BODY_DECLARATIONS_PROPERTY).insertLast(newMethod, description);
					}
				}
				// We need a solution for static method define in Enum ...
			}
		}
	}
	
	private MethodDeclaration createExtensionMethodSignature(MethodDeclaration node, String parent){
		context.getLogger().logInfo("[Custom info] Copy for creating extension method in "+parent+" method "+node.getName());
		//context.getLogger().logInfo("[Custom info] method:"+node);
		
		MethodDeclaration newMethodDeclaration= currentRewriter.getAST().newMethodDeclaration();
		newMethodDeclaration.setName(currentRewriter.getAST().newSimpleName(node.getName().getIdentifier()));
		newMethodDeclaration.setConstructor(false);
		newMethodDeclaration.setExtraDimensions(node.getExtraDimensions());
		newMethodDeclaration.modifiers().addAll(ASTNodeFactory.newModifiers(currentRewriter.getAST(), Modifier.PUBLIC | Modifier.STATIC));
		
		/* you can also just copy the modifiers
		final List memberModifiers = node.modifiers();
		newMethodDeclaration.modifiers().addAll(
				ASTNode.copySubtrees(currentRewriter.getAST(),
						memberModifiers));*/
		newMethodDeclaration.setReturnType2((Type) ASTNode.copySubtree(
				currentRewriter.getAST(), node.getReturnType2()));
		
		newMethodDeclaration.setBody((Block)ASTNode.copySubtree(currentRewriter.getAST(),	node.getBody()));
		SimpleName newParamInMethodDeclaration = currentRewriter.getAST().newSimpleName(PARAMETER_NAME);
		
		/*final Block body = currentRewriter.getAST().newBlock();
		body.statements().addAll(ASTNode.copySubtrees(currentRewriter.getAST(), node.getBody().statements()));
		newMethodDeclaration.setBody(body);*/
		
        //Adding new parameter of type enum in the parameters declaration
		final SingleVariableDeclaration declaration = currentRewriter.getAST().newSingleVariableDeclaration();
		declaration.setName(newParamInMethodDeclaration);
		declaration.setType(currentRewriter.getAST().newSimpleType(currentRewriter.getAST().newSimpleName(parent)));
		newMethodDeclaration.parameters().add(declaration);
		newMethodDeclaration.setJavadoc((Javadoc) ASTNode.copySubtree(currentRewriter.getAST(), node.getJavadoc()));
		newMethodDeclaration.parameters().addAll((List)ASTNode.copySubtrees(currentRewriter.getAST(), node.parameters()));
		return newMethodDeclaration;
	}
}
