using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MjModelProject.Result;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;


namespace MjModelProjectTests
{
    [TestClass]
    public class ShantenCalclatorTest
    {
        [TestMethod]
        public void 単体シャンテン数計算テスト()
        {
            var ss = new ShantenCalclator();

            using (StreamReader sr = new StreamReader(@"C:\Users\desk\Dropbox\shanten_benchmark_data.num.txt", System.Text.Encoding.GetEncoding("shift_jis")))
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
