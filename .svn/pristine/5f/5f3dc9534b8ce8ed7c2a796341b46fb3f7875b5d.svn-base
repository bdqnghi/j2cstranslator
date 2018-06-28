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

using System;
using System.Collections;
using System.Text;
using System.IO;
using System.Collections.Generic;
using ILOG.J2CsMapping.NIO.charset;
using System.Runtime.CompilerServices;

namespace ILOG.J2CsMapping.Util
{

    /// <summary>
    /// .NET replacement for Java properties
    /// </summary>
    public class Properties : Dictionary<object, object>
    {
        /// <summary>
        /// The default values for this Properties.
        /// </summary>
        protected Properties defaults;

        private static int NONE = 0, SLASH = 1, UNICODE = 2, CONTINUE = 3,
                KEY_DONE = 4, IGNORE = 5;

        /// <summary>
        /// Constructs a new Properties object.
        /// </summary>
        public Properties()
        {
        }

        /// <summary>
        /// Constructs a new Properties object using the specified default
        /// properties.
        /// </summary>
        /// <param name="properties">the default properties</param>
        public Properties(Properties properties)
        {
            defaults = properties;
        }

        private void DumpString(StringBuilder buffer, String str, bool key)
        {
            int i = 0;
            if (!key && i < str.Length && str[i] == ' ')
            {
                buffer.Append("\\ ");
                i++;
            }

            for (; i < str.Length; i++)
            {
                char ch = str[i];
                switch (ch)
                {
                    case '\t':
                        buffer.Append("\\t");
                        break;
                    case '\n':
                        buffer.Append("\\n");
                        break;
                    case '\f':
                        buffer.Append("\\f");
                        break;
                    case '\r':
                        buffer.Append("\\r");
                        break;
                    default:
                        if ("\\#!=:".IndexOf(ch) >= 0 || (key && ch == ' '))
                        {
                            buffer.Append('\\');
                        }
                        if (ch >= ' ' && ch <= '~')
                        {
                            buffer.Append(ch);
                        }
                        else
                        {
                            String hex = ILOG.J2CsMapping.Util.IlNumber.ToString(ch, 16);
                            buffer.Append("\\u");
                            for (int j = 0; j < 4 - hex.Length; j++)
                            {
                                buffer.Append("0");
                            }
                            buffer.Append(hex);
                        }
                        break;
                }
            }
        }

        /// <summary>
        /// Searches for the property with the specified name. If the property is not
        /// found, look in the default properties. If the property is not found in
        /// the default properties, answer null.
        /// </summary>
        /// <param name="name">the name of the property to find</param>
        /// <returns> the named property value</returns>
        public String GetProperty(String name)
        {
            try
            {
                Object result = base[name];
                String property = result is String ? (String)result : null;
                if (property == null && defaults != null)
                {
                    property = defaults.GetProperty(name);
                }
                return property;
            }
            catch (KeyNotFoundException)
            {
                return null;
            }
        }

        /// <summary>
        /// Searches for the property with the specified name. If the property is not
        /// found, look in the default properties. If the property is not found in
        /// the default properties, answer the specified default.
        /// </summary>
        /// <param name="name">the name of the property to find</param>
        /// <param name="defaultValue">the default value</param>
        /// <returns>the named property value</returns>
        public String GetProperty(String name, String defaultValue)
        {
            try
            {
                Object result = base[name];
                String property = result is String ? (String)result : null;
                if (property == null && defaults != null)
                {
                    property = defaults.GetProperty(name);
                }
                if (property == null)
                {
                    return defaultValue;
                }
                return property;
            }
            catch (KeyNotFoundException)
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// Lists the mappings in this Properties to the specified PrintStream in a
        /// human readable form.
        /// </summary>
        /// <param name="pout">the PrintStream</param>
        public void List(TextWriter pout)
        {
            
            if (pout == null)
            {
                throw new NullReferenceException();
            }
            StringBuilder buffer = new StringBuilder(80);
            foreach (String key in PropertyNames())
            {
                buffer.Append(key);
                buffer.Append('=');
                String property = (String)this[key];
                Properties def = defaults;
                while (property == null)
                {
                    property = (String)def[key];
                    def = def.defaults;
                }
                if (property.Length > 40)
                {
                    buffer.Append(property.Substring(0, 37));
                    buffer.Append("...");
                }
                else
                {
                    buffer.Append(property);
                }
                pout.WriteLine(buffer.ToString());
                buffer.Length = 0;
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void Load(Stream ins)
        {
            Load(new StreamReader(new BufferedStream(ins), Encoding.Default));           
        }

        /// <summary>
        /// Loads properties from the specified InputStream. The properties are of
        /// the form <code>key=value</code>, one property per line.
        /// </summary>
        /// <param name="ins">the input stream</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void Load(TextReader ins)
        {
            int mode = NONE, unicode = 0, count = 0;
            char nextChar;
            char[] buf = new char[40];
            int offset = 0, keyLength = -1;
            bool firstChar = true;
            char[] inbuf = new char[256];
            int inbufCount = 0, inbufPos = 0;

            while (true)
            {
                /*if (inbufPos == -1)
                    break;*/
                if (inbufPos == inbufCount)
                {
                    try
                    {
                        if ((inbufCount = ins.ReadBlock(inbuf, 0, 256)) == 0)
                        {
                            break;
                        }
                        inbufPos = 0;
                    }
                    catch (ArgumentException e)
                    {
                        break;
                    }
                }
                nextChar = (char)(inbuf[inbufPos++] & 0xff);

                if (offset == buf.Length)
                {
                    char[] newBuf = new char[buf.Length * 2];
                    Array.Copy(buf, 0, newBuf, 0, offset);
                    buf = newBuf;
                }
                if (mode == UNICODE)
                {
                    int digit = Character.Digit(nextChar, 16);
                    if (digit >= 0)
                    {
                        unicode = (unicode << 4) + digit;
                        if (++count < 4)
                        {
                            continue;
                        }
                    }
                    else if (count <= 4)
                    {
                        // luni.09=Invalid Unicode sequence: illegal character
                        throw new ArgumentException("illegal character");
                    }
                    mode = NONE;
                    buf[offset++] = (char)unicode;
                    if (nextChar != '\n')
                    {
                        continue;
                    }
                }
                if (mode == SLASH)
                {
                    mode = NONE;
                    switch (nextChar)
                    {
                        case '\r':
                            mode = CONTINUE; // Look for a following \n
                            continue;
                        case '\n':
                            mode = IGNORE; // Ignore whitespace on the next line
                            continue;
                        case 'b':
                            nextChar = '\b';
                            break;
                        case 'f':
                            nextChar = '\f';
                            break;
                        case 'n':
                            nextChar = '\n';
                            break;
                        case 'r':
                            nextChar = '\r';
                            break;
                        case 't':
                            nextChar = '\t';
                            break;
                        case 'u':
                            mode = UNICODE;
                            unicode = count = 0;
                            continue;
                    }
                }
                else
                {
                    switch (nextChar)
                    {
                        case '#':
                        case '!':
                            if (firstChar)
                            {
                                while (true)
                                {
                                    if (inbufPos == inbufCount)
                                    {
                                        try
                                        {
                                            if ((inbufCount = ins.ReadBlock(inbuf, 0, 256)) == 0)
                                            {
                                                inbufPos = -1;
                                                break;
                                            }
                                            inbufPos = 0;
                                        }
                                        catch (ArgumentException e)
                                        {
                                            inbufPos = -1;
                                            //return;
                                            break;
                                        }
                                    }
                                    nextChar = (char)inbuf[inbufPos++]; // & 0xff
                                    // not
                                    // required
                                    if (nextChar == '\r' || nextChar == '\n')
                                    {
                                        break;
                                    }
                                }
                                continue;
                            }
                            break;
                        case '\n':
                        // fall into the next case
                        case '\r':
                            if (mode == CONTINUE && nextChar == '\n')
                            { // Part of a \r\n sequence
                                mode = IGNORE; // Ignore whitespace on the next line
                                continue;
                            }
                            mode = NONE;
                            firstChar = true;
                            if (offset > 0)
                            {
                                if (keyLength == -1)
                                {
                                    keyLength = offset;
                                }
                                String temp = new String(buf, 0, offset);
                                this[temp.Substring(0, keyLength)] = temp
                                        .Substring(keyLength);
                            }
                            keyLength = -1;
                            offset = 0;
                            continue;
                        case '\\':
                            if (mode == KEY_DONE)
                            {
                                keyLength = offset;
                            }
                            mode = SLASH;
                            continue;
                        case ':':
                        case '=':
                            if (keyLength == -1)
                            { // if parsing the key
                                mode = NONE;
                                keyLength = offset;
                                continue;
                            }
                            break;
                    }
                    if (Char.IsWhiteSpace(nextChar))
                    {
                        if (mode == CONTINUE)
                        {
                            mode = IGNORE;
                        }
                        // if key length == 0 or value length == 0
                        if (offset == 0 || offset == keyLength || mode == IGNORE)
                        {
                            continue;
                        }
                        if (keyLength == -1)
                        { // if parsing the key
                            mode = KEY_DONE;
                            continue;
                        }
                    }
                    if (mode == IGNORE || mode == CONTINUE)
                    {
                        mode = NONE;
                    }
                }
                firstChar = false;
                if (mode == KEY_DONE)
                {
                    keyLength = offset;
                    mode = NONE;
                }
                buf[offset++] = nextChar;
            }
            if (mode == UNICODE && count <= 4)
            {
                // luni.08=Invalid Unicode sequence: expected format \\uxxxx
                throw new ArgumentException("expected format \\uxxxx");
            }
            if (keyLength == -1 && offset > 0)
            {
                keyLength = offset;
            }
            if (keyLength >= 0)
            {
                String temp = new String(buf, 0, offset);
                String key = temp.Substring(0, keyLength);
                String value = temp.Substring(keyLength);
                if (mode == SLASH)
                {
                    value += "\u0000";
                }
                this[key] = value;
            }
        }

        /// <summary>
        /// Answers all of the property names that this Properties contains.
        /// </summary>
        /// <returns>an Enumeration containing the names of all properties</returns>
        public ICollection<Object> PropertyNames()
        {
            if (defaults == null)
            {
                return base.Keys;
            }

            IDictionary<Object, Object> set = new Dictionary<Object, Object>(defaults
                    .Count
                    + Count);
            ICollection<Object> keys = defaults.PropertyNames();
            foreach (String elem in defaults.PropertyNames())
            {
                set[elem] = set;
            }
            foreach (String elem in Keys)
            {
                set[elem] = set;
            }
            return set.Keys;
        }

        /// <summary>
        /// Saves the mappings in this Properties to the specified OutputStream,
        /// putting the specified comment at the beginning. The output from this
        /// method is suitable for being read by the load() method.
        /// </summary>
        /// <param name="outs">the OutputStream</param>
        /// <param name="comment"> the comment</param>
        public void Save(Stream outs, String comment)
        {
            try
            {
                Store(outs, comment);
            }
            catch (IOException e)
            {
            }
        }

        /// <summary>
        /// Maps the specified key to the specified value. If the key already exists,
        /// the old value is replaced. The key and value cannot be null.
        /// </summary>
        /// <param name="name">the key</param>
        /// <param name="value">the value</param>
        /// <returns>the old value mapped to the key, or null</returns>
        public Object SetProperty(String name, String value)
        {
            return this[name] = value;
        }

        private static String lineSeparator;

        // synchronized
        /// <summary>
        /// Stores the mappings in this Properties to the specified OutputStream,
        /// putting the specified comment at the beginning. The output from this
        /// method is suitable for being read by the load() method.
        /// </summary>
        /// <param name="outs">the OutputStream</param>
        /// <param name="comment">the comment</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void Store(Stream outs, String comment)
        {
            BufferedStream awriter = new BufferedStream(outs);
            StreamWriter writer = new StreamWriter(outs, Encoding.Default);

            
            if (lineSeparator == null)
            {
                lineSeparator = "/n";
            }

            if (comment != null)
            {
                writer.Write("#");
                writer.Write(comment);
                writer.Write(lineSeparator);
            }
            writer.Write("#");
            writer.Write(DateTime.Now);
            writer.Write(lineSeparator);

            StringBuilder buffer = new StringBuilder(200);
            foreach (KeyValuePair<Object, Object> entry in this)
            {
                String key = (String)entry.Key;
                DumpString(buffer, key, true);
                buffer.Append('=');
                DumpString(buffer, (String)entry.Value, false);
                buffer.Append(lineSeparator);
                writer.Write(buffer.ToString());
                buffer.Length = 0;
            }
            writer.Flush();
        }
    }
}
