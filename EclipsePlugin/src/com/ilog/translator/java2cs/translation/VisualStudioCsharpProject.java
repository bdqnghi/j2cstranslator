package com.ilog.translator.java2cs.translation;

import java.io.File;
import java.io.FileOutputStream;
import java.io.IOException;
import java.util.ArrayList;
import java.util.List;

import javax.xml.parsers.DocumentBuilder;
import javax.xml.parsers.DocumentBuilderFactory;
import javax.xml.parsers.ParserConfigurationException;
import javax.xml.transform.Transformer;
import javax.xml.transform.TransformerConfigurationException;
import javax.xml.transform.TransformerException;
import javax.xml.transform.TransformerFactory;
import javax.xml.transform.dom.DOMSource;
import javax.xml.transform.stream.StreamResult;

import org.w3c.dom.Document;
import org.w3c.dom.Element;
import org.w3c.dom.NamedNodeMap;
import org.w3c.dom.Node;
import org.w3c.dom.NodeList;
import org.xml.sax.SAXException;
import org.xml.sax.SAXParseException;

/**
 * This class can be used to manipulate an existing Visual Studio C# project
 * (.csproj) file. It allows to add, remove or merge files to the project.
 * 
 * @author Reinhold Bihler
 */
public class VisualStudioCsharpProject {

	private static final String ITEM_GROUP = "ItemGroup";
	private static final String INCLUDE = "Include";
	private static final String COMPILE = "Compile";

	private final Document xmlDocument;
	private final Element includeItemGroup;
	private File projectFile;

	/**
	 * Creates a VisualStudioCsharpProject object that corresponds to the Visual
	 * Studio C# project file passed.
	 * 
	 * @param projectPath
	 *            Path to a Visual Studio C# project file
	 */
	public VisualStudioCsharpProject(String projectPath) {
		this(new File(projectPath));
	}

	/**
	 * Creates a VisualStudioCsharpProject object that corresponds to the Visual
	 * Studio C# project file passed.
	 * 
	 * @param projectFile
	 *            Visual Studio C# project file
	 */
	public VisualStudioCsharpProject(File projectFile) {
		this.projectFile = projectFile;
		xmlDocument = loadDocument(projectFile);
		includeItemGroup = getCompileItemGroupElement(xmlDocument);
	}

	/**
	 * Loads a MS Visual Studio C# project file as DOM Document and returns it.
	 * 
	 * @param projectFile
	 *            Visual Studio C# project file
	 * @return A DOM Document of the passed project file
	 */
	private Document loadDocument(File projectFile) {
		Document document = null;
		try {
			final DocumentBuilderFactory factory = DocumentBuilderFactory
					.newInstance();
			final DocumentBuilder builder = factory.newDocumentBuilder();
			document = builder.parse(projectFile);
		} catch (final SAXParseException spe) {
			System.out.println("Error parsing " + projectFile.getAbsolutePath()
					+ " in line " + spe.getLineNumber());
			System.out.println(spe.getMessage());
			final Exception e = (spe.getException() != null) ? spe
					.getException() : spe;
			e.printStackTrace();
		} catch (final SAXException se) {
			final Exception e = (se.getException() != null) ? se.getException()
					: se;
			e.printStackTrace();
		} catch (final ParserConfigurationException pce) {
			pce.printStackTrace();
		} catch (final IOException ioe) {
			ioe.printStackTrace();
		}
		return document;
	}

	/**
	 * Removes all files from the project
	 */
	public void removeAllFiles() {
		while (includeItemGroup.getChildNodes().getLength() > 0) {
			includeItemGroup.removeChild(includeItemGroup.getFirstChild());
		}
	}

	/**
	 * Adds a list of file paths to the project. The paths are relative to the
	 * location of the project file. E.g. "impl\EPO2FactoryImpl.cs". Slashes are
	 * replaced with backslashes. Note: This method does not prevent the
	 * addition of duplicate files. Use the
	 * {@link #mergeIncludes() mergeIncludes() method} to prevent addition of
	 * duplicates}
	 * 
	 * @see #mergeIncludes()
	 * @param filePaths
	 */
	public void addFiles(List<String> filePaths) {
		if (filePaths == null || filePaths.size() < 1) {
			return;
		}
		filePaths = replaceSlashesWithBackslashes(filePaths);
		if (includeItemGroup.getChildNodes().getLength() == 0) {
			includeItemGroup.appendChild(xmlDocument.createTextNode("\n  "));
		}
		for (int i = 0; i < filePaths.size(); i++) {
			includeItemGroup.appendChild(xmlDocument.createTextNode("  "));
			final Element newElement = xmlDocument.createElement(COMPILE);
			newElement.setAttribute(INCLUDE, filePaths.get(i));
			includeItemGroup.appendChild(newElement);
			includeItemGroup.appendChild(xmlDocument.createTextNode("\n  "));
		}
	}

	private List<String> replaceSlashesWithBackslashes(List<String> paths) {
		for (int i = 0; i < paths.size(); i++) {
			paths.set(i, paths.get(i).replace('/', '\\'));
		}
		return paths;
	}

	/**
	 * @return a list of paths to files that are currently included in the
	 *         project
	 */
	public List<String> getIncludedFiles() {
		final List<String> includes = new ArrayList<String>();
		final NodeList childNoteList = includeItemGroup.getChildNodes();
		for (int i = 0; i < childNoteList.getLength(); i++) {
			final Node compileNode = childNoteList.item(i);
			if (compileNode.getNodeName().equalsIgnoreCase(COMPILE)) {
				final NamedNodeMap attributes = compileNode.getAttributes();
				if (attributes != null) {
					final Node includePath = attributes.getNamedItem(INCLUDE);
					final String include = includePath.getNodeValue();
					if (include != null && !include.equals("")) {
						includes.add(include);
					}
				}
			}
		}
		return includes;
	}

	/**
	 * Adds every path in the passed list of include paths to this project if
	 * not already present.
	 * 
	 * @param includePaths
	 *            list of paths to files to add to the project
	 */
	public void mergeIncludeFiles(List<String> includePaths) {
		if (includePaths == null) {
			return;
		}
		final List<String> uniqueIncludes = removeRedundantIncludes(
				getIncludedFiles(), includePaths);
		addFiles(uniqueIncludes);
	}

	/**
	 * @return a list that contains all strings form the newIncludes list that
	 *         have no duplicate (case insensitive) in the oldIncludes list
	 */
	private List<String> removeRedundantIncludes(List<String> oldIncludes,
			List<String> newIncludes) {
		final List<String> uniqueIncludes = new ArrayList<String>();
		oldIncludes = replaceSlashesWithBackslashes(oldIncludes);
		newIncludes = replaceSlashesWithBackslashes(newIncludes);
		for (final String newPath : newIncludes) {
			boolean duplicateFound = false;
			for (final String oldPath : oldIncludes) {
				if (oldPath.equalsIgnoreCase(newPath)) {
					duplicateFound = true;
					break;
				}
			}
			if (!duplicateFound) {
				uniqueIncludes.add(newPath);
			}
		}
		return uniqueIncludes;
	}

	/**
	 * Saves the Visual Studio C# project file (in place).
	 */
	public void save() {
		// a empty includeItemGroup element is not saved
		if (includeItemGroup.getChildNodes().getLength() < 1) {
			xmlDocument.getDocumentElement().removeChild(includeItemGroup);
		}
		FileOutputStream outputStream = null;
		try {
			final Transformer transformer = TransformerFactory.newInstance()
					.newTransformer();
			outputStream = new FileOutputStream(projectFile);
			transformer.transform(new DOMSource(xmlDocument), new StreamResult(
					outputStream));
		} catch (final TransformerConfigurationException tce) {
			System.out.println("Transformer Factory error");
			System.out.println("   " + tce.getMessage());
			final Throwable e = (tce.getException() != null) ? tce
					.getException() : tce;
			e.printStackTrace();
		} catch (final TransformerException tfe) {
			System.out.println("Transformation error");
			System.out.println("   " + tfe.getMessage());
			final Throwable e = (tfe.getException() != null) ? tfe
					.getException() : tfe;
			e.printStackTrace();
		} catch (final IOException ioe) {
			ioe.printStackTrace();
		} finally {
			if (outputStream != null) {
				try {
					outputStream.close();
				} catch (final Exception e) {
					e.printStackTrace();
				}
			}
		}
		if (includeItemGroup.getChildNodes().getLength() < 1) {
			xmlDocument.getDocumentElement().appendChild(includeItemGroup);
		}
	}

	/**
	 * Saves the Visual Studio C# project file at the specified path.
	 * 
	 * @param projectPath
	 *            The path where to store the Visual Studio C# project file
	 */
	public void saveAs(String projectPath) {
		projectFile = new File(projectPath);
		save();
	}

	/**
	 * Returns the ItemGroup XML Element that contains the Compile Elements that
	 * refer to the files belonging to the project. If no such ItemGroup XML
	 * Element exists it is created.
	 * 
	 * @param document
	 *            The XML DOM Document to search the ItemGroup XML Element in
	 * @return The ItemGroup XML Element whose sub elements refer to the files
	 *         that belong to the project
	 */
	private Element getCompileItemGroupElement(Document document) {
		Element matchingItemGroup = null;
		final Node rootNode = document.getDocumentElement();
		final NodeList childrenOfRoot = rootNode.getChildNodes();
		for (int i = 0; i < childrenOfRoot.getLength(); i++) {
			final Node childOfRoot = childrenOfRoot.item(i);
			if (childOfRoot != null
					&& childOfRoot.getNodeType() == Node.ELEMENT_NODE
					&& ITEM_GROUP.equals(childOfRoot.getNodeName())) {
				final NodeList itemGroupChildren = childOfRoot.getChildNodes();
				for (int j = 0; j < itemGroupChildren.getLength(); j++) {
					final Node childOfChild = itemGroupChildren.item(j);
					if (childOfChild != null
							&& childOfChild.getNodeType() == Node.ELEMENT_NODE
							&& COMPILE.equals(childOfChild.getNodeName())
							&& childOfChild.getAttributes() != null
							&& childOfChild.getAttributes().getNamedItem(
									INCLUDE) != null) {
						matchingItemGroup = (Element) childOfRoot;
						break;
					}
				}
			}
		}
		if (matchingItemGroup == null) {
			matchingItemGroup = document.createElement(ITEM_GROUP);
			document.getDocumentElement().appendChild(matchingItemGroup);
		}
		return matchingItemGroup;
	}

}
