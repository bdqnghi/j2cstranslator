package com.ilog.translator.java2cs.configuration;

import java.lang.reflect.Constructor;
import java.lang.reflect.InvocationTargetException;

import com.ilog.translator.java2cs.translation.ITransformer;
import com.ilog.translator.java2cs.translation.ITranslationContext;

public class TransformerProxy {

	private String condition;
	private Class<ITransformer> transformer;

	//
	//
	//
	
	public TransformerProxy(Class<ITransformer> transformer, String condition) {
		this.transformer = transformer;
		this.condition = condition;
	}

	//
	// Condition
	//
	
	public String getCondition() {
		return condition;
	}

	public void setCondition(String condition) {
		this.condition = condition;
	}

	//
	// Transformer
	//
	
	public Class<ITransformer> getTransformer() {
		return transformer;
	}

	public void setTransformer(Class<ITransformer> transformer) {
		this.transformer = transformer;
	}

	//
	// instantiate
	//
	
	public ITransformer instantiate(ITranslationContext context)
			throws NoSuchMethodException, InvocationTargetException,
			InstantiationException, IllegalAccessException {
		final Constructor<ITransformer> c = transformer
				.getConstructor(new Class[] { ITranslationContext.class });
		final ITransformer t = c.newInstance(new Object[] { context });
		if (context.getConfiguration().getOptions().isSimulation()) {
			t.setSimulation(true);
		}
		if (condition != null && !condition.equals(""))
			t.setTriggerConditionName(condition);
		return t;
	}
}
