using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ILOG.J2CsMapping.Util;

namespace ILOG.J2CsMapping.Text
{

    /*
     *  Licensed to the Apache Software Foundation (ASF) under one or more
     *  contributor license agreements.  See the NOTICE file distributed with
     *  this work for additional information regarding copyright ownership.
     *  The ASF licenses this file to You under the Apache License, Version 2.0
     *  (the "License"); you may not use this file except in compliance with
     *  the License.  You may obtain a copy of the License at
     *
     *     http://www.apache.org/licenses/LICENSE-2.0
     *
     *  Unless required by applicable law or agreed to in writing, software
     *  distributed under the License is distributed on an "AS IS" BASIS,
     *  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
     *  See the License for the specific language governing permissions and
     *  limitations under the License.
     */
    public class TextAttribute : AttributedCharacterIterator_Constants.Attribute
    {

        // set of available text attributes
        private static IDictionary<String, TextAttribute> attrMap = new Dictionary<String, TextAttribute>();

        protected TextAttribute(String name)
            : base(name)
        {
            attrMap[name] = this;
        }

        protected internal override Object ReadResolve()
        {
            TextAttribute result = attrMap[this.GetName()];
            if (result != null)
            {
                return result;
            }
            // awt.194=Unknown attribute name
            throw new Exception("InvalideObject" /* Messages.getString("awt.194") */); //$NON-NLS-1$
        }

        public static TextAttribute BACKGROUND = new TextAttribute("background"); //$NON-NLS-1$

        public static TextAttribute BIDI_EMBEDDING = new TextAttribute("bidi_embedding"); //$NON-NLS-1$

        public static TextAttribute CHAR_REPLACEMENT = new TextAttribute("char_replacement"); //$NON-NLS-1$

        public static TextAttribute FAMILY = new TextAttribute("family"); //$NON-NLS-1$

        public static TextAttribute FONT = new TextAttribute("font"); //$NON-NLS-1$

        public static TextAttribute FOREGROUND = new TextAttribute("foreground"); //$NON-NLS-1$

        public static TextAttribute INPUT_METHOD_HIGHLIGHT = new TextAttribute(
                "input method highlight"); //$NON-NLS-1$

        public static TextAttribute INPUT_METHOD_UNDERLINE = new TextAttribute(
                "input method underline"); //$NON-NLS-1$

        public static TextAttribute JUSTIFICATION = new TextAttribute("justification"); //$NON-NLS-1$

        public static float JUSTIFICATION_FULL = 1.0f;

        public static float JUSTIFICATION_NONE = 0.0f;

        public static TextAttribute NUMERIC_SHAPING = new TextAttribute("numeric_shaping"); //$NON-NLS-1$

        public static TextAttribute POSTURE = new TextAttribute("posture"); //$NON-NLS-1$

        public static float POSTURE_REGULAR = 0.0f;

        public static float POSTURE_OBLIQUE = 0.20f;

        public static TextAttribute RUN_DIRECTION = new TextAttribute("run_direction"); //$NON-NLS-1$

        public static Boolean RUN_DIRECTION_LTR = false;

        public static Boolean RUN_DIRECTION_RTL = true;

        public static TextAttribute SIZE = new TextAttribute("size"); //$NON-NLS-1$

        public static TextAttribute STRIKETHROUGH = new TextAttribute("strikethrough"); //$NON-NLS-1$

        public static Boolean STRIKETHROUGH_ON = true;

        public static TextAttribute SUPERSCRIPT = new TextAttribute("superscript"); //$NON-NLS-1$

        public static Int32 SUPERSCRIPT_SUB = -1;

        public static Int32 SUPERSCRIPT_SUPER = 1;

        public static TextAttribute SWAP_COLORS = new TextAttribute("swap_colors"); //$NON-NLS-1$

        public static Boolean SWAP_COLORS_ON = true;

        public static TextAttribute TRANSFORM = new TextAttribute("transform"); //$NON-NLS-1$

        public static TextAttribute UNDERLINE = new TextAttribute("underline"); //$NON-NLS-1$

        public static Int32 UNDERLINE_ON = 0;

        public static Int32 UNDERLINE_LOW_ONE_PIXEL = 1;

        public static Int32 UNDERLINE_LOW_TWO_PIXEL = 2;

        public static Int32 UNDERLINE_LOW_DOTTED = 3;

        public static Int32 UNDERLINE_LOW_GRAY = 4;

        public static Int32 UNDERLINE_LOW_DASHED = 5;

        public static TextAttribute WEIGHT = new TextAttribute("weight"); //$NON-NLS-1$

        public static float WEIGHT_EXTRA_LIGHT = 0.5f;

        public static float WEIGHT_LIGHT = 0.75f;

        public static float WEIGHT_DEMILIGHT = 0.875f;

        public static float WEIGHT_REGULAR = 1.0f;

        public static float WEIGHT_SEMIBOLD = 1.25f;

        public static float WEIGHT_MEDIUM = 1.5f;

        public static float WEIGHT_DEMIBOLD = 1.75f;

        public static float WEIGHT_BOLD = 2.0f;

        public static float WEIGHT_HEAVY = 2.25f;

        public static float WEIGHT_EXTRABOLD = 2.5f;

        public static float WEIGHT_ULTRABOLD = 2.75f;

        public static TextAttribute WIDTH = new TextAttribute("width"); //$NON-NLS-1$

        public static float WIDTH_CONDENSED = 0.75f;

        public static float WIDTH_SEMI_CONDENSED = 0.875f;

        public static float WIDTH_REGULAR = 1.0f;

        public static float WIDTH_SEMI_EXTENDED = 1.25f;

        public static float WIDTH_EXTENDED = 1.5f;

    }

}
