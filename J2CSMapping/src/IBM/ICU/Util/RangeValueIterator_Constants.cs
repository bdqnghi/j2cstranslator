// --------------------------------------------------------------------------------------------------
// This file was automatically generated by J2CS Translator (http://j2cstranslator.sourceforge.net/). 
// Version 1.3.6.20101125_01     
// 12/2/10 11:47 AM    
// ${CustomMessageForDisclaimer}                                                                             
// --------------------------------------------------------------------------------------------------
 namespace IBM.ICU.Util {
	
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.IO;
	using System.Runtime.CompilerServices;
	public class RangeValueIterator_Constants {
	
	    /// <summary>
	    /// Return result wrapper for com.ibm.icu.util.RangeValueIterator. Stores the
	    /// start and limit of the continous result range and the common value all
	    /// integers between [start, limit - 1] has.
	    /// </summary>
	    ///
	    /// @stable ICU 2.6
	    public class Element {
	        // public data member ---------------------------------------------
	    
	        /// <summary>
	        /// Starting integer of the continuous result range that has the same
	        /// value
	        /// </summary>
	        ///
	        /// @stable ICU 2.6
	        public int start;
	    
	        /// <summary>
	        /// (End + 1) integer of continuous result range that has the same value
	        /// </summary>
	        ///
	        /// @stable ICU 2.6
	        public int limit;
	    
	        /// <summary>
	        /// Gets the common value of the continous result range
	        /// </summary>
	        ///
	        /// @stable ICU 2.6
	        public int value_ren;
	    
	        // public constructor --------------------------------------------
	    
	        /// <summary>
	        /// Empty default constructor to make javadoc happy
	        /// </summary>
	        ///
	        /// @stable ICU 2.4
	        public Element() {
	        }
	    } 
	}
}
