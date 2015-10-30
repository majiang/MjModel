using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MjModelProject.Util
{
    public class NakiAnalizer
    {
        MJsonMessageChi lastConsumedChi;
        MJsonMessagePon lastConsumedPon;
        internal bool CanChi(int dapaiActor, int playerId, List<Pai> tehai, string pai)
        {
            var paiId = PaiConverter.STRING_TO_ID[pai];
            if (paiId > 27)
            {
                return false;
            }
            else if (paiId % 9 == 0)
            {
                if( tehai.Any(e => e.PaiNumber == paiId + 1) &&
                       tehai.Any(e => e.PaiNumber == paiId + 2) )
                {
                    lastConsumedChi = new MJsonMessageChi(playerId, dapaiActor, pai,
                        new List<string> {
                            tehai.First(e => e.PaiNumber == paiId + 1).PaiString,
                            tehai.First(e => e.PaiNumber == paiId + 2).PaiString
                        });
                    return true;
                }

                return false;

            }
            else if (paiId % 9 == 1)
            {
                if (tehai.Any(e => e.PaiNumber == paiId + 1) &&
                    tehai.Any(e => e.PaiNumber == paiId + 2))
                {
                
                    lastConsumedChi = new MJsonMessageChi(playerId, dapaiActor, pai,
                        new List<string> {
                            tehai.First(e => e.PaiNumber == paiId + 1).PaiString,
                            tehai.First(e => e.PaiNumber == paiId + 2).PaiString
                        });
                    return true;
                
                }else if(tehai.Any(e => e.PaiNumber == paiId - 1) &&
                       tehai.Any(e => e.PaiNumber == paiId + 1))
                {
                    lastConsumedChi = new MJsonMessageChi(playerId, dapaiActor, pai,
                        new List<string> {
                            tehai.First(e => e.PaiNumber == paiId - 1).PaiString,
                            tehai.First(e => e.PaiNumber == paiId + 1).PaiString
                        });
                    return true;

                }

                return false;

            }
            else if (paiId % 9 == 7)
            {
                if (tehai.Any(e => e.PaiNumber == paiId - 2) &&
                    tehai.Any(e => e.PaiNumber == paiId - 1))
                {

                    lastConsumedChi = new MJsonMessageChi(playerId, dapaiActor, pai,
                        new List<string> {
                            tehai.First(e => e.PaiNumber == paiId - 2).PaiString,
                            tehai.First(e => e.PaiNumber == paiId - 1).PaiString
                        });
                    return true;

                }
                else if (tehai.Any(e => e.PaiNumber == paiId - 1) &&
                         tehai.Any(e => e.PaiNumber == paiId + 1))
                {
                    lastConsumedChi = new MJsonMessageChi(playerId, dapaiActor, pai,
                        new List<string> {
                            tehai.First(e => e.PaiNumber == paiId - 1).PaiString,
                            tehai.First(e => e.PaiNumber == paiId + 1).PaiString
                        });
                    return true;

                }
                
                return false;
                
            }
            else if (paiId % 9 == 8)
            {
                if (tehai.Any(e => e.PaiNumber == paiId - 2) &&
                       tehai.Any(e => e.PaiNumber == paiId - 1))
                {
                    lastConsumedChi = new MJsonMessageChi(playerId, dapaiActor, pai,
                        new List<string> {
                            tehai.First(e => e.PaiNumber == paiId - 2).PaiString,
                            tehai.First(e => e.PaiNumber == paiId - 1).PaiString
                        });
                    return true;
                }

                return false;
            }
            else
            {
                if (tehai.Any(e => e.PaiNumber == paiId - 2) &&
                    tehai.Any(e => e.PaiNumber == paiId - 1))
                {
                    lastConsumedChi = new MJsonMessageChi(playerId, dapaiActor, pai,
                        new List<string> {
                            tehai.First(e => e.PaiNumber == paiId - 2).PaiString,
                            tehai.First(e => e.PaiNumber == paiId - 1).PaiString
                        });
                    return true;
                }
                else if (tehai.Any(e => e.PaiNumber == paiId - 1) &&
                         tehai.Any(e => e.PaiNumber == paiId + 1))
                {
                    lastConsumedChi = new MJsonMessageChi(playerId, dapaiActor, pai,
                        new List<string> {
                            tehai.First(e => e.PaiNumber == paiId - 1).PaiString,
                            tehai.First(e => e.PaiNumber == paiId + 1).PaiString
                        });
                    return true;

                }
                else if (tehai.Any(e => e.PaiNumber == paiId + 1) &&
                     tehai.Any(e => e.PaiNumber == paiId + 2))
                {
                    lastConsumedChi = new MJsonMessageChi(playerId, dapaiActor, pai,
                        new List<string> {
                            tehai.First(e => e.PaiNumber == paiId + 1).PaiString,
                            tehai.First(e => e.PaiNumber == paiId + 2).PaiString
                        });
                    return true;
                }
                return false;
                
            }
            
        }





        internal bool CanPon(int dapaiActor, int playerId, List<Pai> tehai, string pai)
        {
            var paiId = PaiConverter.STRING_TO_ID[pai];
            var consumedCandidates = tehai.Where(e => e.PaiNumber == paiId).ToList();

            if (tehai.Where(e => e.PaiNumber == paiId).Count() >= 2)
            {
                lastConsumedPon = new MJsonMessagePon(playerId, dapaiActor, pai, new List<string> { consumedCandidates[0].PaiString, consumedCandidates[1].PaiString });
                return true;
            }
            else
            {
                return false;
            }
        }



        internal MJsonMessageChi GetChiMessage()
        {
            return lastConsumedChi;
        }
        internal MJsonMessagePon GetPonMessage()
        {
            return lastConsumedPon;
        }



    }
}
