using Microsoft.VisualStudio.TestTools.UnitTesting;
using MjModelLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using MjModelLibrary.Result;
using System.Diagnostics;
     

namespace MjModelLibrary.Tests
{
    [TestClass()]
    public class ShantenCalclatorTests
    {
        [TestMethod]
        public void Unit_ShantenCalclatorTest()
        {
            var ss = ShantenCalclator.GetInstance();
            ss.SetUseChitoitsu(false);
            ss.SetUseKokushi(false);



            using (StreamReader sr = new StreamReader(@"../../shanten_benchmark_data.num.txt", System.Text.Encoding.GetEncoding("shift_jis")))
            {
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

        [TestMethod]
        public void Unit_FastShantenCalclatorTest()
        {
            var ss = ArrayShantenCalculator.GetInstance();
            ss.SetUseChitoitsu(false);
            ss.SetUseKokushi(false);

            using (StreamReader sr = new StreamReader(@"../../shanten_benchmark_data.num.txt", System.Text.Encoding.GetEncoding("shift_jis")))
            {
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

        [TestMethod]
        public void CompareSpeed()
        {
            var ss = ShantenCalclator.GetInstance();
            var ass = ArrayShantenCalculator.GetInstance();

            int[] testPais = new int[MJUtil.LENGTH_SYU_ALL];

            testPais[0] = 4;
            testPais[1] = 4;
            testPais[2] = 4;
            testPais[3] = 2;

            var compareNum = 1000;
            var sw_normal = new Stopwatch();

            sw_normal.Start();
            for(int i = 0; i < compareNum; i++)
            {
                ss.CalcShanten(testPais);
            }
            sw_normal.Stop();


            var sw_fast = new Stopwatch();
            sw_fast.Start();
            for (int i = 0; i < compareNum; i++)
            {
                ass.CalcShanten(testPais);
            }
            sw_fast.Stop();

            Console.WriteLine("compareNum = {0}, normal = {1}msec, fast = {2}msec.",
                compareNum,sw_normal.ElapsedMilliseconds, sw_fast.ElapsedMilliseconds);

        }

    }
}
