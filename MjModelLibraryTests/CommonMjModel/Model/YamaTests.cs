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
        public void 山ツモテスト()
        {
            Yama testYama = new Yama();

            Assert.AreEqual(testYama.GetRestYamaNum(), FIRST_YAMA_LENGTH);
            foreach (var omote in testYama.GetDoraMarkers())
            {
                Console.WriteLine("Dora = " + omote.PaiString);
            }



            while (testYama.GetRestYamaNum() > 0)
            {
                Console.WriteLine(testYama.DoTsumo().PaiString);
            }
        }


        [TestMethod]
        public void 山リンシャンテスト()
        {
            Yama testYama = new Yama();

            Assert.AreEqual(testYama.GetRestYamaNum(), FIRST_YAMA_LENGTH);

            Console.WriteLine(testYama.DoRinshan().PaiString);
            Assert.AreEqual(testYama.GetRestYamaNum(), FIRST_YAMA_LENGTH - 1);
            Assert.IsTrue(testYama.CanKan());

            Console.WriteLine(testYama.DoRinshan().PaiString);
            Assert.IsTrue(testYama.CanKan());

            Console.WriteLine(testYama.DoRinshan().PaiString);
            Assert.IsTrue(testYama.CanKan());

            Console.WriteLine(testYama.DoRinshan().PaiString);
            Assert.IsFalse(testYama.CanKan());

        }

    }
}