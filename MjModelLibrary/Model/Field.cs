using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

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

        public Field(int kyokuid, int honba, int kyotaku, int oyaPlayerId, string bakaze)
        {

            KyokuId = kyokuid;
            Honba = honba;
            Kyotaku = kyotaku;
            OyaPlayerId = oyaPlayerId;
            Bakaze = new Pai(bakaze);
        }


        static int getOyaPlayerID(int kyokuId)
        {
            return (kyokuId-1) % 4;
        }

        static int getNextOyaPlayerId(int beforeOyaPlayerId)
        {
            return (beforeOyaPlayerId + 1) % 4;
        }


        static readonly int SAME_WIND_KYOKU_NUM = 4;
        public static Field ChangeOnHora(Field fld, int horaActor)
        {
            var IsOyaChange = horaActor != fld.OyaPlayerId;
            var nextKyokuId = IsOyaChange ? fld.KyokuId % SAME_WIND_KYOKU_NUM + 1 : fld.KyokuId;
            var nextHonba = IsOyaChange ? 0 : fld.Honba++;
            var nextkyotaku = 0;
            var nextOyaPlayerId = IsOyaChange ? getNextOyaPlayerId(fld.OyaPlayerId) : fld.OyaPlayerId;
            var nextBakaze = (nextKyokuId == 1 && IsOyaChange) ? getNextBakaze(fld.Bakaze) : fld.Bakaze;

            return new Field(nextKyokuId, nextHonba, nextkyotaku, nextOyaPlayerId, nextBakaze.PaiString);
        }
        
        public static Field ChangeOnRyukyoku(Field fld, List<bool> tenpais)
        {

            var IsOyaChange = tenpais[fld.OyaPlayerId] == false ;
            var nextKyokuId = IsOyaChange ? fld.KyokuId % SAME_WIND_KYOKU_NUM + 1: fld.KyokuId;
            var nextHonba = fld.Honba + 1;
            var nextkyotaku = fld.Kyotaku;
            var nextOyaPlayerId = IsOyaChange ? getNextOyaPlayerId(fld.OyaPlayerId) : fld.OyaPlayerId;
            var nextBakaze = (nextKyokuId == 1 && IsOyaChange) ? getNextBakaze(fld.Bakaze) : fld.Bakaze;

            return new Field(nextKyokuId, nextHonba, nextkyotaku, nextOyaPlayerId,  nextBakaze.PaiString);
        }

        public void AddKyotaku()
        {
            Kyotaku += 1;
        }



        private static Pai getNextBakaze(Pai bakaze)
        {
            Debug.Assert(bakazeTemplate.Contains(bakaze));
            var nowBakazeIndex = bakazeTemplate.IndexOf(bakaze);
            var nextBakazeIndex = (nowBakazeIndex + 1) % 4;
            return bakazeTemplate[nextBakazeIndex];
        }



        private void AssertIdIsInPlayerIdRange(int id)
        {
            Debug.Assert(0 <= id && id < Constants.PLAYER_NUM);
        }

    }
}
