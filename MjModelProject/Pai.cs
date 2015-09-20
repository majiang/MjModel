using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MjModelProject
{
   public class Pai
    {
        public int paiNumber;
        public string paiString;
        public bool isRedDora;

        public Pai(int paiNum)
        {
            paiNumber = paiNum;
            paiString = PaiConverter.ID_TO_STRING[paiNum];
            isRedDora = false;
        }

        public Pai(string paiStr)
        {
            paiNumber = PaiConverter.STRING_TO_ID[paiStr];
            paiString = paiStr;
            isRedDora = PaiConverter.RED_DORA_STRING_ID.ContainsKey(paiStr);
        }

        public Pai() { }
    }
}
