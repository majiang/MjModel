using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MjModelLibrary.CommonMjModel.Model
{
    class TehaiAnalizer
    {
        

        public bool CanChi(Tehai tehai, int dapaiActor, int playerId, string pai)
        {
            if ((dapaiActor != playerId) && ((dapaiActor + 1) % 4 == playerId))
            {
                // can chi 
                return false;
            }
            // cann't chi
            return false;
        }

        public bool CanPon(Tehai tehai, int dapaiActor, int playerId, string pai)
        {
            if ((dapaiActor != playerId))
            {
                // can pon 
                return false;
            }
            return false;
        }

        public bool CanHora(Tehai tehai, InfoForResult ifr, Field field, int dapaiActor, int playerId, string pai)
        {
            if (dapaiActor == playerId)
            {
                return CanHoraIntsumo( tehai,  ifr,  field, dapaiActor, playerId, pai);
            }
            else
            {
                return false;
                //                return CanHoraInOtherPlayerDoroped(tehai, ifr, field, dapaiActor, playerId, pai);
            }
        }

        private bool CanHoraIntsumo(Tehai tehai, InfoForResult ifr, Field field, int dapaiActor, int playerId, string pai)
        {
            if( tehai.GetShanten() == -1)
            {
                var horaResult = ResultCalclator.CalcHoraResult(tehai, ifr, field, pai);
                if (horaResult.yakuResult.HasYakuExcludeDora)
                {
                    return true;
                }
            } 
            return false;
        }



    }
}
