/**
 * 
 */
package com.ilog.translator.java2cs.configuration;

import org.eclipse.jdt.core.IJavaProject;

public class TranslatorDependency implements Comparable<TranslatorDependency> {

	public static enum Kind {
		PROJECT, JAR, DIRECTORY
	}

	//
	//
	//
	
	private String name;
	private TranslatorDependency.Kind kind;
	private String baseDirectory;
	private String profile;

	private String location;
	private final IJavaProject reference;
	//
	//
	//

	public TranslatorDependency(String name, Kind kind, String location,
			IJavaProject reference, String profile) {
		this.name = name;
		this.kind = kind;
		this.location = location;
		this.reference = reference;
		this.profile = profile;
	}

	//
	// Reference
	//
	
	public IJavaProject getReference() {
		return reference;
	}
	
	//
	// Location
	//
	
	public String getLocation() {
		return location;
	}

	public void setLocation(String location) {
		this.location = location;
	}
	
	//
	// Profile
	//
	
	public String getProfile() {
		return profile;
	}

	public void setProfile(String profile) {
		this.profile = profile;
	}

	//
	// BaseDirectory
	//
	
	public String getBaseDirectory() {
		return baseDirectory;
	}

	public void setBaseDirectory(String baseDirectory) {
		this.baseDirectory = baseDirectory;
	}
	
	//
	// Kind
	//

	public TranslatorDependency.Kind getKind() {
		return kind;
	}

	public void setKind(TranslatorDependency.Kind kind) {
		this.kind = kind;
	}
	
	//
	// Name
	//

	public String getName() {
		return name;
	}

	public void setName(String name) {
		this.name = name;
	}
	
	//
	//
	//

	@Override
	public boolean equals(Object arg0) {
		final TranslatorDependency other = (TranslatorDependency) arg0;
		return name.equals(other.name);
	}

	@Override
	public int hashCode() {
		return name.hashCode();
	}

	public int compareTo(TranslatorDependency other) {
		return name.compareTo(other.name);
	}

	//
	//
	//
	
	@Override
	public String toString() {
		return name;
	}

}