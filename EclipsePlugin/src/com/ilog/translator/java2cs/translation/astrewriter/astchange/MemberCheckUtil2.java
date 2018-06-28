package com.ilog.translator.java2cs.translation.astrewriter.astchange;

import java.util.ArrayList;
import java.util.Arrays;
import java.util.List;

import org.eclipse.jdt.core.IField;
import org.eclipse.jdt.core.IJavaElement;
import org.eclipse.jdt.core.IMember;
import org.eclipse.jdt.core.IMethod;
import org.eclipse.jdt.core.IType;
import org.eclipse.jdt.core.JavaModelException;
import org.eclipse.jdt.internal.corext.refactoring.Checks;
import org.eclipse.jdt.internal.corext.refactoring.RefactoringCoreMessages;
import org.eclipse.jdt.internal.corext.refactoring.base.JavaStatusContext;
import org.eclipse.jdt.internal.corext.util.JavaModelUtil;
import org.eclipse.jdt.internal.corext.util.Messages;
import org.eclipse.ltk.core.refactoring.RefactoringStatus;
import org.eclipse.ltk.core.refactoring.RefactoringStatusContext;

import com.ilog.translator.java2cs.translation.util.TranslationUtils;

public class MemberCheckUtil2 {

	private MemberCheckUtil2() {
		// static only
	}

	public static RefactoringStatus checkMembersInDestinationType(
			IMember[] members, IType destinationType) throws JavaModelException {
		final RefactoringStatus result = new RefactoringStatus();
		for (final IMember element : members) {
			if (element.getElementType() == IJavaElement.METHOD) {
				MemberCheckUtil2.checkMethodInType(destinationType, result,
						(IMethod) element);
			} else if (element.getElementType() == IJavaElement.FIELD) {
				MemberCheckUtil2.checkFieldInType(destinationType, result,
						(IField) element);
			} else if (element.getElementType() == IJavaElement.TYPE) {
				MemberCheckUtil2.checkTypeInType(destinationType, result,
						(IType) element);
			}
		}
		return result;
	}

	private static void checkMethodInType(IType destinationType,
			RefactoringStatus result, IMethod method) throws JavaModelException {
		final IMethod[] destinationTypeMethods = destinationType.getMethods();
		final IMethod found = MemberCheckUtil2.findMethod(method,
				destinationTypeMethods);
		if (found != null) {
			final RefactoringStatusContext context = JavaStatusContext.create(
					destinationType.getCompilationUnit(), found
							.getSourceRange());
			final String message = Messages.format(
					RefactoringCoreMessages.MemberCheckUtil_signature_exists,
					new String[] {
							method.getElementName(),
							TranslationUtils
									.getFullyQualifiedName(destinationType) });
			result.addError(message, context);
		} else {
			final IMethod similar = Checks.findMethod(method, destinationType);
			if (similar != null) {
				final String message = Messages
						.format(
								RefactoringCoreMessages.MemberCheckUtil_same_param_count,
								new String[] {
										method.getElementName(),
										TranslationUtils
												.getFullyQualifiedName(destinationType) });
				final RefactoringStatusContext context = JavaStatusContext
						.create(destinationType.getCompilationUnit(), similar
								.getSourceRange());
				result.addWarning(message, context);
			}
		}
	}

	private static void checkFieldInType(IType destinationType,
			RefactoringStatus result, IField field) throws JavaModelException {
		final IField destinationTypeField = destinationType.getField(field
				.getElementName());
		if (!destinationTypeField.exists()) {
			return;
		}
		final String message = Messages.format(
				RefactoringCoreMessages.MemberCheckUtil_field_exists,
				new String[] { field.getElementName(),
						TranslationUtils.getFullyQualifiedName(destinationType) });
		final RefactoringStatusContext context = JavaStatusContext.create(
				destinationType.getCompilationUnit(), destinationTypeField
						.getSourceRange());
		result.addError(message, context);
	}

	private static void checkTypeInType(IType destinationType,
			RefactoringStatus result, IType type) throws JavaModelException {
		final String typeName = type.getElementName();
		final IType destinationTypeType = destinationType.getType(typeName);
		if (destinationTypeType.exists()) {
			final String message = Messages
					.format(
							RefactoringCoreMessages.MemberCheckUtil_type_name_conflict0,
							new String[] {
									typeName,
									TranslationUtils
											.getFullyQualifiedName(destinationType) });
			final RefactoringStatusContext context = JavaStatusContext.create(
					destinationType.getCompilationUnit(), destinationTypeType
							.getNameRange());
			result.addError(message, context);
		} else {
			// need to check the hierarchy of enclosing and enclosed types
			if (destinationType.getElementName().equals(typeName)) {
				final String message = Messages
						.format(
								RefactoringCoreMessages.MemberCheckUtil_type_name_conflict1,
								new String[] { TranslationUtils
										.getFullyQualifiedName(type) });
				final RefactoringStatusContext context = JavaStatusContext
						.create(destinationType.getCompilationUnit(),
								destinationType.getNameRange());
				result.addError(message, context);
			}
			if (MemberCheckUtil2.typeNameExistsInEnclosingTypeChain(
					destinationType, typeName)) {
				final String message = Messages
						.format(
								RefactoringCoreMessages.MemberCheckUtil_type_name_conflict2,
								new String[] { TranslationUtils
										.getFullyQualifiedName(type) });
				final RefactoringStatusContext context = JavaStatusContext
						.create(destinationType.getCompilationUnit(),
								destinationType.getNameRange());
				result.addError(message, context);
			}
			MemberCheckUtil2.checkHierarchyOfEnclosedTypes(destinationType,
					result, type);
		}
	}

	private static void checkHierarchyOfEnclosedTypes(IType destinationType,
			RefactoringStatus result, IType type) throws JavaModelException {
		final IType[] enclosedTypes = MemberCheckUtil2
				.getAllEnclosedTypes(type);
		for (final IType enclosedType : enclosedTypes) {
			if (destinationType.getElementName().equals(
					enclosedType.getElementName())) {
				final String message = Messages
						.format(
								RefactoringCoreMessages.MemberCheckUtil_type_name_conflict3,
								new String[] {
										TranslationUtils
												.getFullyQualifiedName(enclosedType),
												TranslationUtils
												.getFullyQualifiedName(type) });
				final RefactoringStatusContext context = JavaStatusContext
						.create(destinationType.getCompilationUnit(),
								destinationType.getNameRange());
				result.addError(message, context);
			}
			if (MemberCheckUtil2.typeNameExistsInEnclosingTypeChain(
					destinationType, enclosedType.getElementName())) {
				final String message = Messages
						.format(
								RefactoringCoreMessages.MemberCheckUtil_type_name_conflict4,
								new String[] {
										TranslationUtils
												.getFullyQualifiedName(enclosedType),
												TranslationUtils
												.getFullyQualifiedName(type) });
				final RefactoringStatusContext context = JavaStatusContext
						.create(destinationType.getCompilationUnit(),
								destinationType.getNameRange());
				result.addError(message, context);
			}
		}
	}

	@SuppressWarnings("unchecked")
	private static IType[] getAllEnclosedTypes(IType type)
			throws JavaModelException {
		final List result = new ArrayList(2);
		final IType[] directlyEnclosed = type.getTypes();
		result.addAll(Arrays.asList(directlyEnclosed));
		for (final IType enclosedType : directlyEnclosed) {
			result.addAll(Arrays.asList(MemberCheckUtil2
					.getAllEnclosedTypes(enclosedType)));
		}
		return (IType[]) result.toArray(new IType[result.size()]);
	}

	private static boolean typeNameExistsInEnclosingTypeChain(IType type,
			String typeName) {
		IType enclosing = type.getDeclaringType();
		while (enclosing != null) {
			if (enclosing.getElementName().equals(typeName)) {
				return true;
			}
			enclosing = enclosing.getDeclaringType();
		}
		return false;
	}

	/**
	 * Finds a method in a list of methods. Compares methods by signature (only
	 * SimpleNames of types), and not by the declaring type.
	 * 
	 * @return The found method or <code>null</code>, if nothing found
	 */
	public static IMethod findMethod(IMethod method, IMethod[] allMethods)
			throws JavaModelException {
		final String name = method.getElementName();
		final String[] paramTypes = method.getParameterTypes();
		final boolean isConstructor = method.isConstructor();

		for (final IMethod element : allMethods) {
			if (JavaModelUtil.isSameMethodSignature(name, paramTypes,
					isConstructor, element)) {
				return element;
			}
		}
		return null;
	}
}
