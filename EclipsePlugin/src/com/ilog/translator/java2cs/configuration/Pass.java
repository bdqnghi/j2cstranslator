/**
 * 
 */
package com.ilog.translator.java2cs.configuration;

import java.lang.reflect.InvocationTargetException;
import java.util.ArrayList;
import java.util.List;

import com.ilog.translator.java2cs.translation.ITransformer;
import com.ilog.translator.java2cs.translation.ITranslationContext;

/**
 * A "pass" is a set of ITransformers.
 * An ITransformer is a AST visitor which modify a class content.
 * The pass content is describe in an xml file (java2csharp.xml for example).
 * 
 * @author afau
 *
 */
public class Pass {

	private final String name;
	private final boolean launch;
	private final String description;
	private final List<TransformerProxy> transformers = new ArrayList<TransformerProxy>();
	private final List<ITransformer> instantiatedTransformers = new ArrayList<ITransformer>();

	//
	//
	//

	public Pass(String name, String description, boolean launch) {
		this.name = name;
		this.launch = launch;
		this.description = description;
	}

	//
	// Name
	//

	public String getName() {
		return name;
	}

	//
	// Description
	//

	public String getDescription() {
		return description;
	}

	//
	// Launch
	//

	public boolean getLaunch() {
		return launch;
	}

	//
	// Transformers
	//

	public List<TransformerProxy> getTransformers() {
		return transformers;
	}

	//
	// InstantiateTransformers
	//

	public List<ITransformer> getInstantiatedTransformers() {
		return instantiatedTransformers;
	}

	//
	// createTransformers
	// 

	public void createTransformers(ITranslationContext context)
			throws NoSuchMethodException, InvocationTargetException,
			InstantiationException, IllegalAccessException {
		for (final TransformerProxy proxy : getTransformers()) {
			final ITransformer t = proxy.instantiate(context);
			instantiatedTransformers.add(t);
		}
	}
}