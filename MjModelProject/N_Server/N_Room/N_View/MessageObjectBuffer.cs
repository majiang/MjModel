using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MjModelProject.N_Room
{
    class MessageObjectBuffer
    {
        public bool IsSetup { get; set; }
        public List<string> Messages { get; set; }

        //セット時にメッセージオブジェクトを望む形式に変換する。
    }
}
