package com.ilog.translator.java2cs.translation.astrewriter;

import java.util.ArrayList;
import java.util.List;

import org.eclipse.core.runtime.CoreException;
import org.eclipse.core.runtime.IProgressMonitor;
import org.eclipse.core.runtime.NullProgressMonitor;
import org.eclipse.jdt.core.ICompilationUnit;
import org.eclipse.jdt.core.dom.ASTNode;
import org.eclipse.jdt.core.dom.AnnotationTypeDeclaration;
import org.eclipse.jdt.core.dom.AnnotationTypeMemberDeclaration;
import org.eclipse.jdt.core.dom.Assignment;
import org.eclipse.jdt.core.dom.Block;
import org.eclipse.jdt.core.dom.CompilationUnit;
import org.eclipse.jdt.core.dom.Expression;
import org.eclipse.jdt.core.dom.FieldAccess;
import org.eclipse.jdt.core.dom.FieldDeclaration;
import org.eclipse.jdt.core.dom.Javadoc;
import org.eclipse.jdt.core.dom.MethodDeclaration;
import org.eclipse.jdt.core.dom.NumberLiteral;
import org.eclipse.jdt.core.dom.ReturnStatement;
import org.eclipse.jdt.core.dom.SimpleName;
import org.eclipse.jdt.core.dom.SingleVariableDeclaration;
import org.eclipse.jdt.core.dom.Type;
import org.eclipse.jdt.core.dom.TypeDeclaration;
import org.eclipse.jdt.core.dom.VariableDeclarationFragment;
import org.eclipse.jdt.core.dom.Modifier.ModifierKeyword;
import org.eclipse.jdt.internal.corext.dom.ASTNodes;
import org.eclipse.ltk.core.refactoring.Change;
import org.eclipse.text.edits.TextEditGroup;

import com.ilog.translator.java2cs.translation.ITranslationContext;

/**
 * Annotation transformation
 * 
 * @author afau
 *
 */
public class AnnotationVisitor extends ASTRewriterVisitor {


	private boolean hasChange = false;

	//
	//
	//

	public AnnotationVisitor(ITranslationContext context) {
		super(context);
		transformerName = "Annotation to Attribute visitor";
		description = new TextEditGroup(transformerName);
	}

	//
	//
	//

	@Override
	public void reset() {
		super.reset();
		hasChange = false;
	}

	//
	//
	//

	@Override
	public boolean isAbridged() {
		return false;
	}

	public boolean needRecovery() {
		return true;
	}

	//
	//
	//

	@Override
	public boolean applyChange(IProgressMonitor pm) throws CoreException {
		final Change change = createChange(pm, null);
		if (change != null) {
			context.addChange(fCu, change);
		
		}
		return true;
	}

	@Override
	public void postActionOnAST(ICompilationUnit icunit2, CompilationUnit unit2) {
		if (hasChange) {
			try {
				UpdateHandlerTagVisitor v = new UpdateHandlerTagVisitor(
						context);
				final IProgressMonitor subMonitor = new NullProgressMonitor();
				//												
				v.setCompilationUnit(icunit2);
				v.transform(subMonitor,
						unit2);
				v.applyChange(subMonitor);
			} catch (Exception e) {
				context.getLogger().logException("", e);
			}
		}
	}

	@Override
	public boolean hasPostActionOnAST() {
		return true;
	}

	//
	//
	//

	@SuppressWarnings("unchecked")
	@Override
	public void endVisit(AnnotationTypeDeclaration node) {
		final List bodies = node.bodyDeclarations();
		final List modifiers = node.modifiers();
		final SimpleName typeName = node.getName();
		//		
		// For java, annotations are interfaces and you define methods in it.
		// For C#, attributes are classes so I need to implement the methods,
		// add fields that hold the values, and add the constructor(s) to
		// initialize the values
		final TypeDeclaration td = currentRewriter.getAST()
				.newTypeDeclaration();
		td.setJavadoc((Javadoc) ASTNode.copySubtree(currentRewriter.getAST(),
				node.getJavadoc()));
		td.modifiers().addAll(
				ASTNode.copySubtrees(currentRewriter.getAST(), modifiers));
		//
		final List<FieldDeclaration> typesForConstructor = new ArrayList<FieldDeclaration>();
		for (final Object element : bodies) {
			final ASTNode currentElement = ASTNode.copySubtree(currentRewriter
					.getAST(), (ASTNode) element);
			if (currentElement.getNodeType() == ASTNode.ANNOTATION_TYPE_MEMBER_DECLARATION) {
				createNewMethodFromAnnotationTypeMember(td,
						typesForConstructor, currentElement);
			} else {
				td.bodyDeclarations().add(currentElement);
			}
		}
		if (typesForConstructor.size() > 0) {
			final MethodDeclaration constDecl = currentRewriter.getAST()
					.newMethodDeclaration();
			constDecl.setConstructor(true);
			constDecl.setName((SimpleName) ASTNode.copySubtree(currentRewriter
					.getAST(), typeName));
			constDecl.modifiers().add(
					currentRewriter.getAST().newModifier(
							ModifierKeyword.PUBLIC_KEYWORD));
			constDecl.setBody(currentRewriter.getAST().newBlock());
			int i = 0;
			for (final FieldDeclaration t : typesForConstructor) {
				final String identifier = "arg" + i++;
				final SingleVariableDeclaration decl = currentRewriter.getAST()
						.newSingleVariableDeclaration();
				decl.setType((Type) ASTNode.copySubtree(currentRewriter
						.getAST(), t.getType()));
				decl
						.setName(currentRewriter.getAST().newSimpleName(
								identifier));
				constDecl.parameters().add(decl);

				final FieldAccess fAccess = currentRewriter.getAST()
						.newFieldAccess();
				fAccess.setName((SimpleName) ASTNode.copySubtree(
						currentRewriter.getAST(),
						((VariableDeclarationFragment) t.fragments().get(0))
								.getName()));
				fAccess.setExpression(currentRewriter.getAST()
						.newThisExpression());
				final Assignment assign = currentRewriter.getAST()
						.newAssignment();
				assign.setLeftHandSide(fAccess);
				assign.setRightHandSide(currentRewriter.getAST().newSimpleName(
						identifier));
				constDecl.getBody().statements()
						.add(
								currentRewriter.getAST()
										.newExpressionStatement(assign));

			}
			//
			td.bodyDeclarations().add(constDecl);
		}
		//
		final MethodDeclaration constDecl = currentRewriter.getAST()
				.newMethodDeclaration();
		constDecl.setConstructor(true);
		constDecl.setName((SimpleName) ASTNode.copySubtree(currentRewriter
				.getAST(), typeName));
		constDecl.modifiers().add(
				currentRewriter.getAST().newModifier(
						ModifierKeyword.PUBLIC_KEYWORD));
		constDecl.setBody(currentRewriter.getAST().newBlock());
		td.bodyDeclarations().add(constDecl);
		//
		td.setName((SimpleName) ASTNode.copySubtree(currentRewriter.getAST(),
				typeName));
		td.setSuperclassType(currentRewriter.getAST().newSimpleType(
				currentRewriter.getAST().newName("System.Attribute")));
		currentRewriter.replace(node, td, description);
		//
		hasChange = true;
	}

	private void createNewMethodFromAnnotationTypeMember(
			final TypeDeclaration td,
			final List<FieldDeclaration> typesForConstructor,
			final ASTNode currentElement) {
		final AnnotationTypeMemberDeclaration memberDecl = (AnnotationTypeMemberDeclaration) currentElement;
		final MethodDeclaration methDecl = currentRewriter.getAST()
				.newMethodDeclaration();
		methDecl.setJavadoc((Javadoc) ASTNode.copySubtree(currentRewriter
				.getAST(), memberDecl.getJavadoc()));
		final List memberModifiers = memberDecl.modifiers();
		methDecl.modifiers()
				.addAll(
						ASTNode.copySubtrees(currentRewriter.getAST(),
								memberModifiers));
		methDecl.setName((SimpleName) ASTNode.copySubtree(currentRewriter
				.getAST(), memberDecl.getName()));
		methDecl.setReturnType2((Type) ASTNode.copySubtree(currentRewriter
				.getAST(), memberDecl.getType()));
		final Block block = currentRewriter.getAST().newBlock();
		final FieldDeclaration field = createField(td, (Type) ASTNode
				.copySubtree(currentRewriter.getAST(), methDecl
						.getReturnType2()));
		final FieldAccess fAccess = currentRewriter.getAST().newFieldAccess();
		fAccess.setName((SimpleName) ASTNode.copySubtree(currentRewriter
				.getAST(), ((VariableDeclarationFragment) field.fragments()
				.get(0)).getName()));
		fAccess.setExpression(currentRewriter.getAST().newThisExpression());
		final ReturnStatement ret = currentRewriter.getAST()
				.newReturnStatement();
		ret.setExpression(fAccess);
		block.statements().add(ret);
		methDecl.setBody(block);
		td.bodyDeclarations().add(methDecl);
		typesForConstructor.add(field);
	}

	@SuppressWarnings("unchecked")
	private FieldDeclaration createField(TypeDeclaration td, Type returnType2) {
		final String identifier = "a"
				+ ASTNodes.getTypeName(returnType2).replace("[]", "Array");
		final VariableDeclarationFragment fragment = currentRewriter.getAST()
				.newVariableDeclarationFragment();
		fragment.setName(currentRewriter.getAST().newSimpleName(identifier));
		final FieldDeclaration fd = currentRewriter.getAST()
				.newFieldDeclaration(fragment);
		fd.modifiers().add(
				currentRewriter.getAST().newModifier(
						ModifierKeyword.PUBLIC_KEYWORD));
		fd.setType(returnType2);
		td.bodyDeclarations().add(fd);
		//
		return fd;
	}

	@SuppressWarnings("unused")
	private Expression computeDefaultValue(Type returnType2) {
		if (returnType2.isPrimitiveType()) {
			final NumberLiteral number = currentRewriter.getAST()
					.newNumberLiteral();
			number.setToken("0");
			return number;
		}
		return currentRewriter.getAST().newNullLiteral();
	}

}
