using Microsoft.VisualStudio.TestTools.UnitTesting;
using MjModelLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MjModelLibrary.Tests
{
    [TestClass()]
    public class ShantenCalclatorTests
    {
        [TestMethod]
        public void シャンテン数計算テスト()
        {
            var ss = ShantenCalclator.GetInstance();

            using (StreamReader sr = new StreamReader(@"../../shanten_benchmark_data.num.txt", System.Text.Encoding.GetEncoding("shift_jis")))
            {
                ss.UseChitoitsu = false;
                ss.UseKokushi = false;

                while (true)
                {
                    string line = sr.ReadLine();
                    if (line == null) break;
                    string[] input = line.Split(' ');
                    int[] haiIds = new int[34];
                    for (int i = 0; i < input.Length - 1; i++)
                    {
                        haiIds[Int32.Parse(input[i])]++;
                    }
                    int expected = Int32.Parse(input[input.Length - 1]);
                    int actual = ss.CalcShanten(haiIds);
                    if (actual != expected) throw new Exception(line + " --> " + actual);
                }
            }
        }
    }
}
