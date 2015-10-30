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
            foreach (var omote in testYama.GetDoraMarkers())
            {
                Debug.WriteLine("Dora = "+omote.PaiString);
            }



            while( testYama.GetRestYamaNum() > 0 )
            {
                Debug.WriteLine(testYama.DoTsumo().PaiString);
            }
        }


        [TestMethod]
        public void 山リンシャンテスト()
        {
            Yama testYama = new Yama();

            Assert.AreEqual(testYama.GetRestYamaNum(), FIRST_YAMA_LENGTH);
            
            Debug.WriteLine(testYama.DoRinshan().PaiString);
            Assert.AreEqual(testYama.GetRestYamaNum(), FIRST_YAMA_LENGTH - 1);
            Assert.IsTrue(testYama.CanKan());
            
            Debug.WriteLine(testYama.DoRinshan().PaiString);
            Assert.IsTrue(testYama.CanKan());
            
            Debug.WriteLine(testYama.DoRinshan().PaiString);
            Assert.IsTrue(testYama.CanKan());
            
            Debug.WriteLine(testYama.DoRinshan().PaiString);
            Assert.IsFalse(testYama.CanKan());

        }


    }
}
