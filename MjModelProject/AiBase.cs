using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MjModelProject
{
    public class AiBase
    {   

        //思考する関数群。AIでオーバーライドする。
        public virtual int ThinkDahai(Field field, Yama yama, List<Kawa> kawa, Tehai tehai)
        {
            return 0;//stub
        }

        public virtual bool ThinkNaki(Field field, Yama yama, List<Kawa> kawa, Tehai tehai) {
            return false;//stub
        }

        public virtual bool ThinkReach(Field field, Yama yama, List<Kawa> kawa, Tehai tehai)
        {
            return false;
        }

        public virtual bool ThinkHora(Field field, Yama yama, List<Kawa> kawa, Tehai tehai) {
            return false;
        }

    }
}
