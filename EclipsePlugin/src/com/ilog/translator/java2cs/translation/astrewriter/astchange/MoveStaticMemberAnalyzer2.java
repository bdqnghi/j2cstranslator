package com.ilog.translator.java2cs.translation.astrewriter.astchange;

import java.util.HashSet;
import java.util.Set;

import org.eclipse.jdt.core.dom.AST;
import org.eclipse.jdt.core.dom.ASTNode;
import org.eclipse.jdt.core.dom.ASTVisitor;
import org.eclipse.jdt.core.dom.Expression;
import org.eclipse.jdt.core.dom.FieldAccess;
import org.eclipse.jdt.core.dom.IBinding;
import org.eclipse.jdt.core.dom.IPackageBinding;
import org.eclipse.jdt.core.dom.ITypeBinding;
import org.eclipse.jdt.core.dom.MemberRef;
import org.eclipse.jdt.core.dom.MethodInvocation;
import org.eclipse.jdt.core.dom.MethodRef;
import org.eclipse.jdt.core.dom.Name;
import org.eclipse.jdt.core.dom.QualifiedName;
import org.eclipse.jdt.core.dom.SimpleName;
import org.eclipse.jdt.core.dom.Type;
import org.eclipse.jdt.internal.corext.dom.ASTFlattener;
import org.eclipse.jdt.internal.corext.dom.ASTNodeFactory;
import org.eclipse.jdt.internal.corext.dom.ASTNodes;
import org.eclipse.jdt.internal.corext.dom.Bindings;
import org.eclipse.jdt.internal.corext.refactoring.RefactoringCoreMessages;
import org.eclipse.jdt.internal.corext.refactoring.base.JavaStatusContext;
import org.eclipse.jdt.internal.corext.refactoring.structure.CompilationUnitRewrite;
import org.eclipse.ltk.core.refactoring.RefactoringStatus;


public class MoveStaticMemberAnalyzer2 extends ASTVisitor {

	protected RefactoringStatus fStatus;

	protected ITypeBinding fSource;

	protected ITypeBinding fTarget;

	protected CompilationUnitRewrite fCuRewrite;

	protected IBinding[] fMembers;

	protected boolean fNeedsImport;

	@SuppressWarnings("unchecked")
	protected Set fProcessed;

	protected static final String REFERENCE_UPDATE = RefactoringCoreMessages.MoveMembersRefactoring_referenceUpdate;

	@SuppressWarnings("unchecked")
	public MoveStaticMemberAnalyzer2(CompilationUnitRewrite cuRewrite,
			IBinding[] members, ITypeBinding source, ITypeBinding target) {
		super(true);
		fStatus = new RefactoringStatus();
		fCuRewrite = cuRewrite;
		fMembers = members;
		fSource = source;
		fTarget = target;
		fProcessed = new HashSet();
	}

	public RefactoringStatus getStatus() {
		return fStatus;
	}

	protected boolean isProcessed(ASTNode node) {
		return fProcessed.contains(node);
	}

	@SuppressWarnings("unchecked")
	protected void rewrite(SimpleName node, ITypeBinding type) {
		final AST ast = node.getAST();
		final Type result = fCuRewrite.getImportRewrite().addImport(type,
				fCuRewrite.getAST());
		fCuRewrite.getImportRemover().registerAddedImport(
				type.getQualifiedName());
		final Name dummy = ASTNodeFactory.newName(fCuRewrite.getAST(),
				ASTFlattener.asString(result));
		final QualifiedName name = ast.newQualifiedName(dummy, ast
				.newSimpleName(node.getIdentifier()));
		fCuRewrite
				.getASTRewrite()
				.replace(
						node,
						name,
						fCuRewrite
								.createGroupDescription(MoveStaticMemberAnalyzer2.REFERENCE_UPDATE));
		fCuRewrite.getImportRemover().registerRemovedNode(node);
		fProcessed.add(node);
		fNeedsImport = true;
	}

	@SuppressWarnings("unchecked")
	protected void rewrite(QualifiedName node, ITypeBinding type) {
		rewriteName(node.getQualifier(), type);
		fProcessed.add(node.getName());
	}

	@SuppressWarnings("unchecked")
	protected void rewrite(FieldAccess node, ITypeBinding type) {
		Expression exp = node.getExpression();
		if (exp == null) {
			final Type result = fCuRewrite.getImportRewrite().addImport(type,
					fCuRewrite.getAST());
			fCuRewrite.getImportRemover().registerAddedImport(
					type.getQualifiedName());
			exp = ASTNodeFactory.newName(fCuRewrite.getAST(), ASTFlattener
					.asString(result));
			fCuRewrite
					.getASTRewrite()
					.set(
							node,
							FieldAccess.EXPRESSION_PROPERTY,
							exp,
							fCuRewrite
									.createGroupDescription(MoveStaticMemberAnalyzer2.REFERENCE_UPDATE));
			fNeedsImport = true;
		} else if (exp instanceof Name) {
			rewriteName((Name) exp, type);
		} else {
			rewriteExpression(node, exp, type);
		}
		fProcessed.add(node.getName());
	}

	@SuppressWarnings("unchecked")
	protected void rewrite(MethodInvocation node, ITypeBinding type) {
		Expression exp = node.getExpression();
		if (exp == null) {
			final Type result = fCuRewrite.getImportRewrite().addImport(type,
					fCuRewrite.getAST());
			fCuRewrite.getImportRemover().registerAddedImport(
					type.getQualifiedName());
			exp = ASTNodeFactory.newName(fCuRewrite.getAST(), ASTFlattener
					.asString(result));
			fCuRewrite
					.getASTRewrite()
					.set(
							node,
							MethodInvocation.EXPRESSION_PROPERTY,
							exp,
							fCuRewrite
									.createGroupDescription(MoveStaticMemberAnalyzer2.REFERENCE_UPDATE));
			fNeedsImport = true;
		} else if (exp instanceof Name) {
			rewriteName((Name) exp, type);
		} else {
			rewriteExpression(node, exp, type);
		}
		fProcessed.add(node.getName());
	}

	@SuppressWarnings("unchecked")
	protected void rewrite(MemberRef node, ITypeBinding type) {
		Name qualifier = node.getQualifier();
		if (qualifier == null) {
			final Type result = fCuRewrite.getImportRewrite().addImport(type,
					fCuRewrite.getAST());
			fCuRewrite.getImportRemover().registerAddedImport(
					type.getQualifiedName());
			qualifier = ASTNodeFactory.newName(fCuRewrite.getAST(),
					ASTFlattener.asString(result));
			fCuRewrite
					.getASTRewrite()
					.set(
							node,
							MemberRef.QUALIFIER_PROPERTY,
							qualifier,
							fCuRewrite
									.createGroupDescription(MoveStaticMemberAnalyzer2.REFERENCE_UPDATE));
			fNeedsImport = true;
		} else {
			rewriteName(qualifier, type);
		}
		fProcessed.add(node.getName());
	}

	@SuppressWarnings("unchecked")
	protected void rewrite(MethodRef node, ITypeBinding type) {
		Name qualifier = node.getQualifier();
		if (qualifier == null) {
			final Type result = fCuRewrite.getImportRewrite().addImport(type,
					fCuRewrite.getAST());
			fCuRewrite.getImportRemover().registerAddedImport(
					type.getQualifiedName());
			qualifier = ASTNodeFactory.newName(fCuRewrite.getAST(),
					ASTFlattener.asString(result));
			fCuRewrite
					.getASTRewrite()
					.set(
							node,
							MethodRef.QUALIFIER_PROPERTY,
							qualifier,
							fCuRewrite
									.createGroupDescription(MoveStaticMemberAnalyzer2.REFERENCE_UPDATE));
			fNeedsImport = true;
		} else {
			rewriteName(qualifier, type);
		}
		fProcessed.add(node.getName());
	}

	private void rewriteName(Name name, ITypeBinding type) {
		final AST creator = name.getAST();
		boolean fullyQualified = false;
		if (name instanceof QualifiedName) {
			final SimpleName left = ASTNodes.getLeftMostSimpleName(name);
			if (left.resolveBinding() instanceof IPackageBinding) {
				fullyQualified = true;
			}
		}
		if (fullyQualified) {
			fCuRewrite
					.getASTRewrite()
					.replace(
							name,
							ASTNodeFactory.newName(creator, type
									.getQualifiedName()),
							fCuRewrite
									.createGroupDescription(MoveStaticMemberAnalyzer2.REFERENCE_UPDATE));
			fCuRewrite.getImportRemover().registerRemovedNode(name);
		} else {
			final Type result = fCuRewrite.getImportRewrite().addImport(type,
					fCuRewrite.getAST());
			fCuRewrite.getImportRemover().registerAddedImport(
					type.getQualifiedName());
			final Name n = ASTNodeFactory.newName(fCuRewrite.getAST(),
					ASTFlattener.asString(result));
			fCuRewrite
					.getASTRewrite()
					.replace(
							name,
							n,
							fCuRewrite
									.createGroupDescription(MoveStaticMemberAnalyzer2.REFERENCE_UPDATE));
			fCuRewrite.getImportRemover().registerRemovedNode(name);
			fNeedsImport = true;
		}
	}

	private void rewriteExpression(ASTNode node, Expression exp,
			ITypeBinding type) {
		fCuRewrite
				.getASTRewrite()
				.replace(
						exp,
						fCuRewrite.getImportRewrite().addImport(type,
								fCuRewrite.getAST()),
						fCuRewrite
								.createGroupDescription(MoveStaticMemberAnalyzer2.REFERENCE_UPDATE));
		fCuRewrite.getImportRemover().registerAddedImport(
				type.getQualifiedName());
		fCuRewrite.getImportRemover().registerRemovedNode(exp);
		fNeedsImport = true;
		nonStaticAccess(node);
	}

	protected void nonStaticAccess(ASTNode node) {
		fStatus.addWarning(
				RefactoringCoreMessages.MoveStaticMemberAnalyzer_nonStatic,
				JavaStatusContext.create(fCuRewrite.getCu(), node));
	}

	protected boolean isStaticAccess(Expression exp, ITypeBinding type) {
		if (!(exp instanceof Name)) {
			return false;
		}
		return Bindings.equals(type, ((Name) exp).resolveBinding());
	}

	protected boolean isMovedMember(IBinding binding) {
		if (binding == null) {
			return false;
		}
		for (final IBinding element : fMembers) {
			// Bug
			if (binding.getKind() == IBinding.TYPE) {
				final ITypeBinding tBinding = (ITypeBinding) binding;
				final ITypeBinding raw = tBinding.getErasure();
				if (raw.isEqualTo(element)) {
					return true;
				}
			}
			//
			if (Bindings.equals(element, binding)) {
				return true;
			}
		}
		return false;
	}
}
