﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MjModelProject
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
            KyokuId = 0;
            Honba = 0;
            Kyotaku = 0;
            OyaPlayerId = getOyaPlayerID(0);
            Bakaze = getBakaze(0);
        }

        public Field(int kyokuid, int honba, int kyotaku)
        {

            KyokuId = kyokuid;
            Honba = honba;
            Kyotaku = kyotaku;
            OyaPlayerId = getOyaPlayerID(kyokuid);
            Bakaze = getBakaze(kyokuid);
        }

        private static int getOyaPlayerID(int kyokuId)
        {
            return kyokuId % 4;
        }

        private static Pai getBakaze(int kyokuId)
        {
            return bakazeTemplate[kyokuId % 4];
        }

        public static Field ChangeOnRyukyoku(Field fld, List<bool> tenpais)
        {
            var nextKyokuId = tenpais[fld.OyaPlayerId] ? fld.KyokuId : fld.KyokuId + 1;
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
