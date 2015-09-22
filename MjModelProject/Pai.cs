using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MjModelProject
{
    public class Pai : IComparable
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


        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            Pai other = obj as Pai;
            if (other == null)
            {
                return false;
            }

            return this.paiString.Equals(other.paiString);

        }


        public int CompareTo(object obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException();
            }
            Pai other = obj as Pai;
            if (other == null)
            {
                throw new ArgumentException();
            }

            return this.paiString.CompareTo(other.paiString);
            
        }

        public override int GetHashCode()
        {
            return this.paiString.GetHashCode();
        }
    }
}
