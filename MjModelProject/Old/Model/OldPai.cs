using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MjServer.Util;

namespace MjServer
{
    public class Pai : IComparable
    {
        public int PaiNumber { get; set; }
        public string PaiString { get; set; }
        public bool IsRedDora { get; set; }

        public Pai(int paiNum)
        {
            PaiNumber = paiNum;
            PaiString = PaiConverter.ID_TO_STRING[paiNum];
            IsRedDora = false;
        }

        public Pai(string paiStr)
        {
            PaiNumber = PaiConverter.STRING_TO_ID[paiStr];
            PaiString = paiStr;
            IsRedDora = PaiConverter.RED_DORA_STRING_ID.ContainsKey(paiStr);
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

            return this.PaiString.Equals(other.PaiString);

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

            return this.PaiString.CompareTo(other.PaiString);
            
        }

        public override int GetHashCode()
        {
            return this.PaiString.GetHashCode();
        }
    }
}
