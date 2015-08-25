using Microsoft.VisualStudio.TestTools.UnitTesting;
using MjModelProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MjModelProject.Tests
{
    [TestClass()]
    public class TehaiTests
    {
        [TestMethod()]
        public void TsumoTest()
        {
            Tehai testTehai = new Tehai(new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 });
            testTehai.Tsumo(14);
            Assert.AreEqual(testTehai.tehai.Last(), 14);

            //chi
            var actor = 0;
            var target = 3;
            var furopai = 13;
            var consumed = new List<int> { 8, 12 };
            consumed.Sort();
            testTehai.Chi(actor, target, furopai, consumed);
            Assert.AreEqual(testTehai.furos[0].ftype,Furo.Furotype.chi);
            Assert.AreEqual(testTehai.furos[0].furopai, furopai);
            CollectionAssert.AreEqual(testTehai.furos[0].consumed, consumed);
    

        }
    }
}