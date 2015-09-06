using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MjModelProject
{
    public class MjModel
    {

        public Yama yama { get; set; }
        public List<Kawa> kawas { get; set; }
        public List<Tehai> tehais { get; set; }
        public Field field { get; set; }
        public List<int> turnds;

    }
}
