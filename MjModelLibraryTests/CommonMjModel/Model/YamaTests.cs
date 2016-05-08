using Microsoft.VisualStudio.TestTools.UnitTesting;
using MjModelLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace MjModelLibrary.Tests
{
    [TestClass()]
    public class YamaTests
    {
        private const int FIRST_YAMA_LENGTH = 4 * 34 - 14;
        [TestMethod]
        public void Unit_YamaTsumoTest()
        {
            Yama testYama = new Yama();

            Assert.AreEqual(testYama.GetRestYamaNum(), FIRST_YAMA_LENGTH);
            foreach (var omote in testYama.GetDoraMarkers())
            {
                Debug.WriteLine("Dora = " + omote.PaiString);
            }



            while (testYama.GetRestYamaNum() > 0)
            {
                var tsumoObj = testYama.DoTsumo();
                Debug.WriteLine(tsumoObj.PaiString);
            }
        }


        [TestMethod]
        public void Unit_YamaRinshanTest()
        {
            Yama testYama = new Yama();

            Assert.AreEqual(testYama.GetRestYamaNum(), FIRST_YAMA_LENGTH);

            var rinshanObj = testYama.DoRinshan();
            Debug.WriteLine(rinshanObj.PaiString);
            Assert.AreEqual(testYama.GetRestYamaNum(), FIRST_YAMA_LENGTH - 1);
            Assert.IsTrue(testYama.CanKan());

            rinshanObj = testYama.DoRinshan();
            Debug.WriteLine(rinshanObj.PaiString);
            Assert.IsTrue(testYama.CanKan());

            rinshanObj = testYama.DoRinshan();
            Debug.WriteLine(rinshanObj.PaiString);
            Assert.IsTrue(testYama.CanKan());

            rinshanObj = testYama.DoRinshan();
            Debug.WriteLine(rinshanObj.PaiString);
            Assert.IsFalse(testYama.CanKan());

        }

    }
}