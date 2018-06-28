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

namespace ILOG.J2CsMapping.Util.Logging
{

    using ILOG.J2CsMapping.Collections;
    using System;
    using System.IO;
    using System.Resources;
    using System.Collections;

    [Serializable]
    public class Level
    {
        public static readonly Level ALL = new Level("ALL", -2147483648);
        public static readonly Level FINEST = new Level("FINEST", 300);
        public static readonly Level FINER = new Level("FINER", 400);
        public static readonly Level FINE = new Level("FINE", 500);
        public static readonly Level CONFIG = new Level("CONFIG", 700);
        public static readonly Level INFO = new Level("INFO", 800);
        public static readonly Level WARNING = new Level("WARNING", 900);
        public static readonly Level SEVERE = new Level("SEVERE", 1000);
        public static readonly Level OFF = new Level("OFF", 2147483647);
        private static readonly Hashtable nameToLevelMap = new Hashtable(17);
        private string theName;
        private string bundleName;
        private int theValue;

        static Level()
        {
            nameToLevelMap.Add(ALL.theName, ALL);
            nameToLevelMap.Add(FINEST.theName, FINEST);
            nameToLevelMap.Add(FINER.theName, FINER);
            nameToLevelMap.Add(FINE.theName, FINE);
            nameToLevelMap.Add(CONFIG.theName, CONFIG);
            nameToLevelMap.Add(INFO.theName, INFO);
            nameToLevelMap.Add(WARNING.theName, WARNING);
            nameToLevelMap.Add(SEVERE.theName, SEVERE);
            nameToLevelMap.Add(OFF.theName, OFF);
        }

        public static Level Parse(string str)
        {
            Level level = (Level)nameToLevelMap[str];
            if (level != null)
                return level;
            try
            {
                int i = Int32.Parse(str);
                level = Level.GetByValue(i);
                if (level == null)
                    throw new ArgumentException("Integer value passed does not corresponding to any predefined Level!");
                return level;
            }
            catch (FormatException)
            {
                throw new ArgumentException("Provided name not a level name and not parseable as an integer!");
            }
        }

        internal Level(string str, int i)
            : base()
        {
            theName = str;
            theValue = i;
        }

        internal Level(string str, int i, string arg)
            : base()
        {
            theName = str;
            theValue = i;
            bundleName = arg;
        }

        public string GetLocalizedName()
        {
            if (bundleName == null)
                return theName;
            string str = null;
            return str;
        }

        public string GetName()
        {
            return theName;
        }

        public string GetResourceBundleName()
        {
            return bundleName;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Level))
                return false;
            Level level0 = (Level)obj;
            return theValue == level0.theValue;
        }

        public override int GetHashCode()
        {
            return theValue;
        }

        public int IntValue()
        {
            return theValue;
        }

        public override string ToString()
        {
            return theName;
        }

        private object ReadResolve()
        {
            Level level0 = Level.GetByValue(theValue);
            if (level0 == null)
                throw new IOException("Unable to resolve object!");
            return level0;
        }

        private static Level GetByValue(int i)
        {
            if (i == ALL.theValue)
                return ALL;
            if (i == FINEST.theValue)
                return FINEST;
            if (i == FINER.theValue)
                return FINER;
            if (i == FINE.theValue)
                return FINE;
            if (i == CONFIG.theValue)
                return CONFIG;
            if (i == INFO.theValue)
                return INFO;
            if (i == WARNING.theValue)
                return WARNING;
            if (i == SEVERE.theValue)
                return SEVERE;
            if (i == OFF.theValue)
                return OFF;
            return null;
        }

    }

}
