using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MjServer
{
    public class Field
    {

        public int KyokuId { get; private set; }//1~8が入る
        public int Honba { get; private set; }
        public int Kyotaku { get; private set; }
        public int OyaPlayerId { get; private set; }
        public Pai Bakaze { get; private set; }

        private static List<Pai> bakazeTemplate = new List<Pai>() { new Pai("E"), new Pai("S"), new Pai("W"), new Pai("N") };


        public Field()
        {
            KyokuId = 1;//1index
            Honba = 0;
            Kyotaku = 0;
            OyaPlayerId = getOyaPlayerID(0);
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
            return kyokuId % 4;
        }



        public static Field ChangeOnRyukyoku(Field fld, List<bool> tenpais)
        {
            var nextKyokuId = tenpais[fld.OyaPlayerId] ? fld.KyokuId : fld.KyokuId + 1;
            var nextHonba = fld.Honba + 1;
            var nextkyotaku = fld.Kyotaku;
            var nextBakaze = GetNextBakaze(tenpais[fld.OyaPlayerId], fld.KyokuId, fld.Bakaze);

            return new Field(nextKyokuId, nextHonba, nextkyotaku,nextBakaze);
        }

        public void AddKyotaku()
        {
            Kyotaku += Constants.REACH_POINT;
        }

        static string GetNextBakaze(bool oyaTenpai, int ryuukyokedKyokuid, Pai ryuukyokedBakaze)
        {
            if (oyaTenpai)
            {
                return ryuukyokedBakaze.PaiString;
            }
            if (ryuukyokedKyokuid == Constants.PLAYER_NUM)
            {
                var index = bakazeTemplate.FindIndex(e => e == ryuukyokedBakaze);

                if (index == bakazeTemplate.Count - 1)
                {
                    return bakazeTemplate[0].PaiString;
                }
                else {
                    return bakazeTemplate[index+1].PaiString;
                }
            }
            else
            {
                return ryuukyokedBakaze.PaiString;
            }

            
        }

    }
}
