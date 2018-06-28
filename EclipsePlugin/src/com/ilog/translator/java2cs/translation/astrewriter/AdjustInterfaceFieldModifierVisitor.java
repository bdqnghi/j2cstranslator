package com.ilog.translator.java2cs.translation.astrewriter;

import java.util.List;

import org.eclipse.jdt.core.dom.FieldDeclaration;
import org.eclipse.jdt.core.dom.ITypeBinding;
import org.eclipse.jdt.core.dom.IVariableBinding;
import org.eclipse.jdt.core.dom.Modifier;
import org.eclipse.jdt.core.dom.SimpleName;
import org.eclipse.jdt.core.dom.TypeDeclaration;
import org.eclipse.jdt.core.dom.VariableDeclarationFragment;
import org.eclipse.text.edits.TextEditGroup;

import com.ilog.translator.java2cs.configuration.ChangeModifierDescriptor;
import com.ilog.translator.java2cs.configuration.DotNetModifier;
import com.ilog.translator.java2cs.translation.ITranslationContext;
import com.ilog.translator.java2cs.translation.noderewriter.ModifiersRewriter;
import com.ilog.translator.java2cs.translation.util.TranslationUtils;

/**
 * Modify interface fields modifiers
 * 
 * @author afau
 *
 */
public class AdjustInterfaceFieldModifierVisitor extends ASTRewriterVisitor {

	//
	//
	//

	public AdjustInterfaceFieldModifierVisitor(ITranslationContext context) {
		super(context);
		transformerName = "Adjust Interface Field Modifier";
		description = new TextEditGroup(transformerName);
	}

	//
	//
	//

	@Override
	public void endVisit(TypeDeclaration node) {
		final SimpleName name = node.getName();
		// TODO : improve tests !
		if (TranslationUtils.isAGeneratedConstantsClass(name, context)) {
			final FieldDeclaration[] fields = node.getFields();
			final ITypeBinding iTypeb = node.resolveBinding();
			final IVariableBinding[] fieldsBinding = iTypeb.getDeclaredFields();

			for (final FieldDeclaration field : fields) {
				final ChangeModifierDescriptor change = new ChangeModifierDescriptor();
				if (Modifier.isStatic(field.getModifiers())) {
					final IVariableBinding binding = findBinding(field,
							fieldsBinding);
					if (binding != null) {
						final ITypeBinding itype = binding.getType();
						if (!itype.isArray() && itype.isPrimitive()) {
							change.remove(DotNetModifier.STATIC);
							change.add(DotNetModifier.CONST);
							final ModifiersRewriter rewriter = new ModifiersRewriter(
									change);
							rewriter.setOnlyDotNet(true);
							rewriter.setOnlyJava(true);
							rewriter.process(context, field, currentRewriter,
									null, description);
						}
					}
				}
			}
		}
	}

	//
	//
	//

	@SuppressWarnings("unchecked")
	private IVariableBinding findBinding(FieldDeclaration node,
			IVariableBinding[] fieldsBinding) {
		final List l = node.fragments();
		final String n = ((VariableDeclarationFragment) l.get(0)).getName()
				.getFullyQualifiedName();

		IVariableBinding f = null;

		for (final IVariableBinding v : fieldsBinding) {
			if (v.getName().equals(n)) {
				f = v;
			}
		}

		return f;
	}
}
