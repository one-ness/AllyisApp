using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AllyisApps;
using AllyisApps.Utilities;

namespace AllyisApps.Tests
{
    [TestClass]
    public class SQLDateProtectorTest
    {
        [TestMethod]
        public void Low()
        {
            DateTime before = new DateTime(1752,12,31,23,59,59);
            DateTime on = new DateTime(1753, 1, 1, 0, 0, 0);
            DateTime after = new DateTime(1753, 1, 1, 0, 0, 1);

            SQLDateProtectorAttribute prot = new SQLDateProtectorAttribute();
            Assert.IsFalse(prot.IsValid(before), "Before start passed");
            Assert.IsTrue(prot.IsValid(on), "On start failed");
            Assert.IsTrue(prot.IsValid(after), "After start failed");
        }

        [TestMethod]
        public void High()
        {
            DateTime before = new DateTime(9999, 12, 31, 23, 59, 58);
            DateTime on = new DateTime(9999, 12, 31, 23, 59, 59);
            //DateTime after = new DateTime(10000, 1, 1, 0, 0, 0); //unrepresentable

            SQLDateProtectorAttribute prot = new SQLDateProtectorAttribute();
            Assert.IsTrue(prot.IsValid(before), "Before end failed");
            Assert.IsTrue(prot.IsValid(on), "On end failed");
        }

        [TestMethod]
        public void Now()
        {
            SQLDateProtectorAttribute prot = new SQLDateProtectorAttribute();
            Assert.IsTrue(prot.IsValid(DateTime.Now));
            Assert.IsTrue(prot.IsValid(DateTime.Now.AddYears(-40)));
        }
    }
}
