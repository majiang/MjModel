using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MjModelProject;
using System.Diagnostics;
using System.Collections.Generic;

namespace MjModelProjectTests
{
    [TestClass]
    public class YamaTest
    {
        private const int FIRST_YAMA_LENGTH = 4 * 34 - 14;
        [TestMethod]
        public void 山ツモテスト()
        {
            Yama testYama = new Yama();

            Assert.AreEqual(testYama.GetRestYamaNum(), FIRST_YAMA_LENGTH);
            foreach (var omote in testYama.GetDoraOmote())
            {
                Debug.WriteLine("Dora = "+omote.paiString);
            }



            while( testYama.GetRestYamaNum() > 0 )
            {
                Debug.WriteLine(testYama.DoTsumo().paiString);
            }
        }


        [TestMethod]
        public void 山リンシャンテスト()
        {
            Yama testYama = new Yama();

            Assert.AreEqual(testYama.GetRestYamaNum(), FIRST_YAMA_LENGTH);
            
            Debug.WriteLine(testYama.DoRinshan().paiString);
            Assert.AreEqual(testYama.GetRestYamaNum(), FIRST_YAMA_LENGTH - 1);
            Assert.IsTrue(testYama.CanKan());
            
            Debug.WriteLine(testYama.DoRinshan().paiString);
            Assert.IsTrue(testYama.CanKan());
            
            Debug.WriteLine(testYama.DoRinshan().paiString);
            Assert.IsTrue(testYama.CanKan());
            
            Debug.WriteLine(testYama.DoRinshan().paiString);
            Assert.IsFalse(testYama.CanKan());

        }
    }
}
