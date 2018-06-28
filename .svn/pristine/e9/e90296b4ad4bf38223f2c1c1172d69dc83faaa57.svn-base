package com.ilog.translator.java2cs.configuration;

import java.io.File;
import java.net.URL;
import java.util.Collection;
import java.util.HashMap;
import java.util.Map;

import javax.xml.parsers.DocumentBuilder;
import javax.xml.parsers.DocumentBuilderFactory;

import org.eclipse.core.runtime.FileLocator;
import org.eclipse.core.runtime.IPath;
import org.eclipse.core.runtime.Path;
import org.w3c.dom.Document;
import org.w3c.dom.Element;
import org.w3c.dom.Node;
import org.w3c.dom.NodeList;

import sun.reflect.generics.reflectiveObjects.NotImplementedException;

import com.ilog.translator.java2cs.plugin.TranslationPlugin;
import com.ilog.translator.java2cs.translation.ITransformer;

/**
 * A class who describe how to perform a translation.
 * What step in the translation (which transformer use), how to 
 * 'translate' resources.
 */
public class TranslationDescriptor {
	private Map<String, Pass> passes = new HashMap<String, Pass>();
	private Map<String, ResourceProcessor> resourceProcessors = new HashMap<String, ResourceProcessor>();
	
	private Logger logger;
	private boolean isDebug;
	
	/**
	 * 
	 */
	public TranslationDescriptor(Logger logger, boolean isDebug) {
		this.logger = logger;
		this.isDebug = isDebug;
	}
	
	/**
	 * 
	 * @param configName
	 */
	void saveTranslationProcessDescriptorFile(String configName) {
		throw new NotImplementedException();
	}
	
	/**
	 * Read the translation process descriptor file i.e. the xml file that contains all
	 * passes to execute during the translation
	 * 
	 * @throws Exception
	 */
	@SuppressWarnings("unchecked") 
	void readTranslationProcessDescriptorFile(String configName)
			throws Exception {		
		File f = new File(configName);
		String fName = null;
		if (f.exists()) {
			fName = configName;
		} else {
			final IPath path = new Path("/configuration/" + configName);
			URL url = FileLocator.find(TranslationPlugin.getDefault().getBundle(),
				path, null);
			url = FileLocator.toFileURL(url);
			fName = url.getFile();
		}
		if (isDebug) {
			logger.logInfo("Global configuration file is " + fName);
		}
		final DocumentBuilderFactory docBuilderFactory = DocumentBuilderFactory
				.newInstance();
		final DocumentBuilder docBuilder = docBuilderFactory
				.newDocumentBuilder();
		final Document doc = docBuilder.parse(new File(fName));

		final Element root = doc.getDocumentElement();
		// normalize text representation doc.getDocumentElement ().normalize ();
		if (isDebug) {
			logger.logInfo(
					"Root element of the doc is " + root.getNodeName());
		}

		final NodeList listOfPasses = doc.getElementsByTagName("pass");
		final int totalPasses = listOfPasses.getLength();
		if (isDebug) {
			logger.logInfo("Total no of passes : " + totalPasses);
		}

		for (int s = 0; s < listOfPasses.getLength(); s++) {
			final Node firstPassNode = listOfPasses.item(s);
			if (firstPassNode.getNodeType() == Node.ELEMENT_NODE) {
				final Element firstPassElement = (Element) firstPassNode;

				final String pName = firstPassElement.getAttribute("name");
				final String launch = firstPassElement.getAttribute("launch");
				final String description = firstPassElement
						.getAttribute("description");

				if (isDebug) {
					logger.logInfo(
							"Transformer " + pName + " " + launch);
				}

				final Pass pass = new Pass(pName, description, Boolean
						.parseBoolean(launch));
				passes.put(pName, pass);

				final NodeList listOfTransformers = firstPassElement
						.getElementsByTagName("transformer");

				for (int t = 0; t < listOfTransformers.getLength(); t++) {
					final Node firstTransformerNode = listOfTransformers
							.item(t);
					if (firstTransformerNode.getNodeType() == Node.ELEMENT_NODE) {
						final Element firstTransformerElement = (Element) firstTransformerNode;
						final String clazz = firstTransformerElement
								.getAttribute("class");
						final Class<ITransformer> classT = (Class<ITransformer>) Class
								.forName(clazz);
						final String condition = firstTransformerElement
								.getAttribute("condition");
						if (isDebug) {
							logger.logInfo("   Transformer " + clazz);
						}
						pass.getTransformers().add(
								new TransformerProxy(classT, condition));
					}
				}
			}
		}
		
		final NodeList listOfResourceProcessor = doc.getElementsByTagName("resourceProcessor");
		final int totalProcessors = listOfResourceProcessor.getLength();
		if (isDebug) {
			logger.logInfo("Total no of processor : " + totalProcessors);
		}

		for (int s = 0; s < listOfResourceProcessor.getLength(); s++) {
			final Node firstPassNode = listOfResourceProcessor.item(s);
			if (firstPassNode.getNodeType() == Node.ELEMENT_NODE) {
				final Element firstPassElement = (Element) firstPassNode;

				final String pName = firstPassElement.getAttribute("name");
				final String filter = firstPassElement.getAttribute("filter");
				final String fqnCName = firstPassElement.getAttribute("class");

				final Class<ResourceProcessor> classT = (Class<ResourceProcessor>) Class
				.forName(fqnCName);				
				resourceProcessors.put(filter, classT.newInstance());
			}
		}

	}

	//
	//
	//

	/**
	 * Return all passes to be launch during the translation
	 */
	public Collection<Pass> getPasses() {
		return passes.values();
	}
	
	/**
	 * Return resources processors to use during translation
	 * 
	 * @return
	 */
	public Map<String, ResourceProcessor> getResourcesProcessors() {
		return resourceProcessors;
	}
	
	@Override
	public void finalize() {
		passes = null;		
		resourceProcessors = null;
	}
}