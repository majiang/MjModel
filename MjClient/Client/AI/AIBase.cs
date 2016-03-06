using System;
using System.Collections.Generic;
using MjNetworkProtocol;
using MjModelLibrary;

namespace MjClient
{
    abstract class AIBase
    {
        //ツモ順が来た時にどんなアクションするか
        //許可されているレスポンスは　ツモ、打牌、暗槓、加槓
        abstract public MjsonMessageBase thinkDahai(int myPositionId, string pai, List<Tehai> tehais, List<Kawa> kawas, Field field);

        //外家が打牌した時にどんなアクションするか
        //許可されているレスポンスは　何もしない、和了、ポン、カン、チー、ミンカン
        abstract public MjsonMessageBase thinkAction(int myPositionId,int dapaiActor, string pai, List<Tehai> tehais, List<Kawa> kawas, Field field);

    }
}
