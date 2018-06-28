package com.ilog.translator.java2cs.configuration.parser;

import java.io.StringReader;

import junit.framework.TestCase;

import org.eclipse.jdt.core.JavaModelException;

import com.ilog.translator.java2cs.configuration.TranslationConfiguration;
import com.ilog.translator.java2cs.configuration.info.MappingsInfo;
import com.ilog.translator.java2cs.configuration.info.TranslateInfo;

public class MappingsFileParserTests extends TestCase {

	public void testGenerics() {
		final TranslationConfiguration config = new TranslationConfiguration(
				null, null, null);
		final TranslateInfo ti = new TranslateInfo(config);
		final MappingsInfo thisMappingsInfo = new MappingsInfo("generated", ti);
		final MappingsFileParser parser = new MappingsFileParser(
				thisMappingsInfo, null, false);
		final StringReader reader = new StringReader(
				":: ILOG.CoreRuleEngine.Engine.Util:IlrIntervalBinaryTree<%1,%2>.Node {  field value { name = value_ren ; }}class ilog.rules.engine.util.IlrAtomicInterval :: ILOG.CoreRuleEngine.Engine.Util:IlrAtomicInterval { field value { name = value_ren ; }}");
		try {
			parser.parseClass("toto", reader, null, null);
		} catch (final JavaModelException e) {
			e.printStackTrace();
		}
	}
}