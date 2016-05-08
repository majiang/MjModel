using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MjModelLibrary.Result
{
    public class ReadOnlyTehai
    {
        public List<string> tehai { get; private set; }
        public List<IDictionary<string, string>> furos { get; private set; }

        public ReadOnlyTehai(List<string> tehai, List<Dictionary<string,string>> furos)
        {
            this.tehai = new List<string>(tehai);

            this.furos = new List<IDictionary<string, string>>(furos);
        }
        public int CalcShanten()
        {
            return 1;
        }


        public override string ToString()
        {
            var tehaiString = string.Join(",", tehai);
            var furoString = string.Join(",", furos.Select(e => DictToString(e)));
            return "Tehai:"+ tehaiString + " Furo:" + furoString;
        }

        string DictToString(IDictionary<string, string> table)
        {
            StringBuilder sb = new StringBuilder();
            foreach(var map in table)
            {
                sb.Append(map.ToString());
            }

            return sb.ToString();


        }
    }

    public class ReadOnlyKawa
    {
    }

    public struct FieldInfomation
    {

    }
}
