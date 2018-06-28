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

/// ---------------------------------------------------------------------------------------------------
///  This file was automatically generated by J2CS Translator (http://j2cstranslator.sourceforge.net/). 
///  Version 1.0.0                                                                                      
/// ---------------------------------------------------------------------------------------------------
 namespace Text2 {
	
	using Junit.Framework;
	using NUnit;
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Runtime.CompilerServices;
	
	[NUnit.Framework.TestFixture]
	public class NumberFormatExceptionTest {
	
		
		/// @tests java.lang.NumberFormatException#NumberFormatException()
		[NUnit.Framework.Test]
		public void Test_Constructor() {
			NumberFormatException e = new NumberFormatException();
			Junit.Framework.Assert.AssertNull(e.Message);
			Junit.Framework.Assert.AssertNull(e.Message);
			Junit.Framework.Assert.AssertNull(e.InnerException);
		}
	
		
		/// @tests java.lang.NumberFormatException#NumberFormatException(java.lang.String)
		[NUnit.Framework.Test]
		public void Test_ConstructorLjava_lang_String() {
			NumberFormatException e = new NumberFormatException("fixture");
			Junit.Framework.Assert.AssertEquals("fixture", e.Message);
			Junit.Framework.Assert.AssertNull(e.InnerException);
		}
	}
}
