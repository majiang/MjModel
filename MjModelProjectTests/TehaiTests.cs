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
        public void ツモテスト()
        {
            Tehai testTehai = new Tehai(new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 });
            testTehai.Tsumo(13);
            Assert.AreEqual(testTehai.tehai.Last(), 13);

        }

        [TestMethod()]
        public void チーテスト()
        {
            Tehai testTehai = new Tehai(new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 });
            testTehai.Tsumo(13);
            Assert.AreEqual(testTehai.tehai.Last(), 13);

            //pon
            var actor = 0;
            var target = 3;
            var furopai = 14;
            var consumed = new List<int> { 12, 13 };
            consumed.Sort();

            Assert.IsTrue(testTehai.tehai.Contains(12));
            Assert.IsTrue(testTehai.tehai.Contains(13));

            //ポン実施
            testTehai.Pon(actor, target, furopai, consumed);

            //フーロオブジェクトの構成が正しいか
            Assert.AreEqual(testTehai.furos[0].ftype, Furo.Furotype.pon);
            Assert.AreEqual(testTehai.furos[0].furopai, furopai);
            CollectionAssert.AreEqual(testTehai.furos[0].consumed, consumed);

            //晒した牌が手配に残っていないか
            Assert.IsFalse(testTehai.tehai.Contains(12));
            Assert.IsFalse(testTehai.tehai.Contains(13));

        }

        [TestMethod()]
        public void 大明槓テスト()
        {
            Tehai testTehai = new Tehai(new List<int> { 0, 1, 2 });
            
            //daiminkan
            var actor = 0;
            var target = 2;
            var furopai = 3;
            var consumed = new List<int> { 0, 1, 2  };
            consumed.Sort();

            Assert.IsTrue(testTehai.tehai.Contains(0));
            Assert.IsTrue(testTehai.tehai.Contains(1));
            Assert.IsTrue(testTehai.tehai.Contains(2));

            //ポン実施
            testTehai.Daiminkan(actor, target, furopai, consumed);

            //フーロオブジェクトの構成が正しいか
            Assert.AreEqual(testTehai.furos[0].ftype, Furo.Furotype.daiminkan);
            Assert.AreEqual(testTehai.furos[0].furopai, furopai);
            CollectionAssert.AreEqual(testTehai.furos[0].consumed, consumed);

            //晒した牌が手配に残っていないか
            Assert.IsFalse(testTehai.tehai.Contains(0));
            Assert.IsFalse(testTehai.tehai.Contains(1));
            Assert.IsFalse(testTehai.tehai.Contains(2));

        }

        //正常系しかないよね
        //どこかで異常な入力を弾く仕組みが必要だが...
        //サーバー対戦の場合はサーバー側で検証するから問題なし。

        //ローカル対戦でも使うから...
        //コントローラで弾く？
        //いやいやいやロジック層でやるべき
        //なき牌選択の部分でやろう。
        //なき候補牌を返す関数をつくろう。


    }
}