// 
// J2CsMapping : runtime library for J2CsTranslator
// 
// Copyright (c) 2008-2010 Alexandre FAU.
// All rights reserved. This program and the accompanying materials
// are made available under the terms of the Eclipse Public License v1.0
// which accompanies this distribution, and is available at
// http://www.eclipse.org/legal/epl-v10.html
// Contributors:
//   Alexandre FAU (IBM)
//

namespace ILOG.J2CsMapping.XML.Sax
{

    using System;

    public class XTokens_Constants
    {
        internal static readonly TokenDictionary XDictionary = new TokenDictionary();
        public const string CLASS = "class";
        public const string INTERFACE = "interface";
        public const string NATIVE = "native";
        public const string PACKAGE = "package";
        public const string STATIC = "static";
        public const string PUBLIC = "public";
        public const string PROTECTED = "protected";
        public const string PRIVATE = "private";
        public const string FINAL = "final";
        public const string TRANSIENT = "transient";
        public const string ABSTRACT = "abstract";
        public const string VOLATILE = "volatile";
        public const string SYNCHRONIZED = "synchronized";
        public const string EXTENDS = "extends";
        public const string IMPLEMENTS = "implements";
        public const string THROWS = "throws";
        public const string RESTRICTION = "restriction";
        public const string OPERATOR = "operator";
        public const string READONLY = "readonly";
        public const string WRITEONLY = "writeonly";
        public const string ENUM = "enum";
        public const string DOMAIN = "domain";
        public const string DEFAULT = "default";
        public const string PROPERTY = "property";
        public const string VOID = "void";
        public const string BOOLEAN = "boolean";
        public const string BYTE = "byte";
        public const string CHAR = "char";
        public const string FLOAT = "float";
        public const string DOUBLE = "double";
        public const string SHORT = "short";
        public const string INT = "int";
        public const string LONG = "long";
        public const string NULL = "null";
        public const char DOT = '.';
        public const char COMMA = ',';
        public const char SEMICOLON = ';';
        public const char LT = '<';
        public const char GT = '>';
        public const char LBRACE = '{';
        public const char RBRACE = '}';
        public const char LBRACKET = '[';
        public const char RBRACKET = ']';
        public const char LPAREN = '(';
        public const char RPAREN = ')';
        public const char TILDE = '~';
        public const char EQ = '=';
        public const char SLASH = '/';
        public const string bomPackageName = "ilog.rules.bom";
        public const string IlrObjectModelTag = "IlrObjectModel";
        public const string IlrPackageTag = "IlrPackage";
        public const string IlrClassTag = "IlrClass";
        public const string IlrEnumTag = "IlrEnum";
        public const string IlrAttributeTag = "IlrAttribute";
        public const string IlrMethodTag = "IlrMethod";
        public const string IlrConstructorTag = "IlrConstructor";
        public const string IlrDestructorTag = "IlrDestructor";
        public const string IlrParameterTag = "IlrParameter";
        public const string IlrStaticReferenceTag = "IlrStaticReference";
        public const string IlrComponentPropertyTag = "IlrComponentProperty";
        public const string IlrIndexedComponentPropertyTag
          = "IlrIndexedComponentProperty";
        public const string IlrDomainTag = "IlrDomainTag";
        public const string attributesTag = "attributes";
        public const string methodsTag = "methods";
        public const string constructorsTag = "constructors";
        public const string destructorTag = "destructor";
        public const string propertiesTag = "properties";
        public const string propertyTag = "property";
        public const string nameTag = "name";
        public const string valueTag = "value";
        public const string typeTag = "type";
        public const string wildcardTag = "wildcard";
        public const string nestedClassesTag = "nestedClasses";
        public const string superclassesTag = "superclasses";
        public const string classNameTag = "className";
        public const string modifiersTag = "modifiers";
        public const string modifierTag = "modifier";
        public const string readMethodTag = "readMethod";
        public const string writeMethodTag = "writeMethod";
        public const string alternateNameTag = "alternate";
        public const string domainTag = "domain";
        public const string returnTypeTag = "returnType";
        public const string parametersTag = "parameters";
        public const string exceptionsTag = "exceptions";
        public const string INCLUDED_PROPERTY = "includedFrom";
    }

    public interface XTokens
    {

    }
}
