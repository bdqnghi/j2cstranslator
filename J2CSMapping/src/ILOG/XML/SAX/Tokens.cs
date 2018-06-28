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

    public class Tokens_Constants
    {
        public static readonly TokenDictionary Dictionary = new TokenDictionary();
        public static readonly string INCLUDE
          = Tokens_Constants.Dictionary.Intern("include");
        public static readonly string IMPORT
          = Tokens_Constants.Dictionary.Intern("import");
        public static readonly string NATIVE
          = Tokens_Constants.Dictionary.Intern("native");
        public static readonly string CLASS
          = Tokens_Constants.Dictionary.Intern("class");
        public static readonly string INTERFACE
          = Tokens_Constants.Dictionary.Intern("interface");
        public const string INSTANCE = "instance";
        public static readonly string PACKAGE
          = Tokens_Constants.Dictionary.Intern("package");
        public static readonly string STATIC
          = Tokens_Constants.Dictionary.Intern("static");
        public static readonly string PUBLIC
          = Tokens_Constants.Dictionary.Intern("public");
        public static readonly string PROTECTED
          = Tokens_Constants.Dictionary.Intern("protected");
        public static readonly string PRIVATE
          = Tokens_Constants.Dictionary.Intern("private");
        public static readonly string FINAL
          = Tokens_Constants.Dictionary.Intern("final");
        public static readonly string TRANSIENT
          = Tokens_Constants.Dictionary.Intern("transient");
        public static readonly string ABSTRACT
          = Tokens_Constants.Dictionary.Intern("abstract");
        public static readonly string VOLATILE
          = Tokens_Constants.Dictionary.Intern("volatile");
        public static readonly string SYNCHRONIZED
          = Tokens_Constants.Dictionary.Intern("synchronized");
        public static readonly string EXTENDS
          = Tokens_Constants.Dictionary.Intern("extends");
        public static readonly string SUPER
          = Tokens_Constants.Dictionary.Intern("super");
        public static readonly string IMPLEMENTS
          = Tokens_Constants.Dictionary.Intern("implements");
        public static readonly string THROWS
          = Tokens_Constants.Dictionary.Intern("throws");
        public static readonly string OPERATOR
          = Tokens_Constants.Dictionary.Intern("operator");
        public static readonly string ENUM
          = Tokens_Constants.Dictionary.Intern("enum");
        public static readonly string DOMAIN
          = Tokens_Constants.Dictionary.Intern("domain");
        public const string DEFAULT = "default";
        public const string PROPERTY = "property";
        public static readonly string RESTRICTION
          = Tokens_Constants.Dictionary.Intern("restriction");
        public static readonly string TRUE
          = Tokens_Constants.Dictionary.Intern("true");
        public static readonly string FALSE
          = Tokens_Constants.Dictionary.Intern("false");
        public static readonly string NULL
          = Tokens_Constants.Dictionary.Intern("null");
        public const string READONLY = "readonly";
        public const string WRITEONLY = "writeonly";
        public static readonly string VOID
          = Tokens_Constants.Dictionary.InternType("void");
        public static readonly string BOOLEAN
          = Tokens_Constants.Dictionary.InternType("boolean");
        public static readonly string BYTE
          = Tokens_Constants.Dictionary.InternType("byte");
        public static readonly string CHAR
          = Tokens_Constants.Dictionary.InternType("char");
        public static readonly string FLOAT
          = Tokens_Constants.Dictionary.InternType("float");
        public static readonly string DOUBLE
          = Tokens_Constants.Dictionary.InternType("double");
        public static readonly string SHORT
          = Tokens_Constants.Dictionary.InternType("short");
        public static readonly string INT
          = Tokens_Constants.Dictionary.InternType("int");
        public static readonly string LONG
          = Tokens_Constants.Dictionary.InternType("long");
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
        public const char IDENT_ESC = '@';
        public const string INCLUDED_PROPERTY = "includedFrom";
    }

    public interface Tokens
    {

    }
}
