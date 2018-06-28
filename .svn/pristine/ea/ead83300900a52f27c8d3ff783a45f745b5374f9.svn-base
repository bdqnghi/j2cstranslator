using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using NUnit.Framework;
using ILOG.J2CsMapping.Collections;

namespace MappingTests
{
    [TestFixture]
    public class HelperTests
    {
        public void Mth(params String[] par)
        {
            foreach (String s in par)
            {
                Console.WriteLine(" arg type is " + s.GetType());
            }
        }


        [Test]
        public void InvokeTest()
        {
            try
            {
                String str = "";
                Mth(str);
                Mth(new String[] { str });
            }
            catch (Exception e)
            {
                Assert.Fail();
            }
        }
    }
}
