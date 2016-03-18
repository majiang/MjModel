using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MjModelLibrary
{
    public class Field
    {

        public int KyokuId { get; private set; }//1~4
        public int Honba { get; private set; }
        public int Kyotaku { get; private set; }
        public int OyaPlayerId { get; private set; }//0~3
        public Pai Bakaze { get; private set; }

        private static List<Pai> bakazeTemplate = new List<Pai>() { new Pai("E"), new Pai("S"), new Pai("W"), new Pai("N") };


        public Field()
        {
            KyokuId = 1;//1index
            Honba = 0;
            Kyotaku = 0;
            OyaPlayerId = getOyaPlayerID(1);//start kyoku id = 1, start oya player id = 0
            Bakaze = bakazeTemplate[0];
        }

        public Field(int kyokuid, int honba, int kyotaku, string bakaze)
        {

            KyokuId = kyokuid;
            Honba = honba;
            Kyotaku = kyotaku;
            OyaPlayerId = getOyaPlayerID(kyokuid);
            Bakaze = new Pai(bakaze);
        }

        public Field(int kyokuid, int honba, int kyotaku)
        {

            KyokuId = kyokuid;
            Honba = honba;
            Kyotaku = kyotaku;
            OyaPlayerId = getOyaPlayerID(kyokuid);
            
        }

        private static int getOyaPlayerID(int kyokuId)
        {
            return (kyokuId-1) % 4;
        }



        public static Field ChangeOnRyukyoku(Field fld, List<bool> tenpais)
        {
            var nextKyokuId = tenpais[fld.OyaPlayerId] ? fld.KyokuId : (4 + fld.OyaPlayerId + 1)%4 + 1;
            var nextHonba = fld.Honba++;
            var nextkyotaku = fld.Kyotaku;

            return new Field(nextKyokuId, nextHonba, nextkyotaku);
        }

        public void AddKyotaku()
        {
            Kyotaku += Constants.REACH_POINT;
        }
    }
}
