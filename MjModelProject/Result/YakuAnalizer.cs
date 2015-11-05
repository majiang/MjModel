using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MjModelProject.Util;
using MjModelProject;
using MjModelProject.Model;

namespace MjModelProject.Result
{






    public class YakuResult
    {
        public Dictionary<string, int> yakus = new Dictionary<string, int>();
        public int Han = 0;
        public int Fu = 0;
        public int DoraNum = 0;
        public int YakuhaiNum = 0;

    }




    public static class YakuAnalizer
    {



        public static YakuResult AnalyzeYaku(HoraPattern horaMentsu, InfoForResult ifr, int[] horaSyu)
        {
            return new YakuResult();
        }
        
            public static YakuResult calcYaku(HoraPattern horaMentsu, InfoForResult ifr,int[] horaSyu){
                YakuResult result = new YakuResult();
            
                result.Fu = calcFu( horaMentsu, ifr );

                //役の文字列取得
                var yakuString = MJUtil.YAKU_STRING;
                //飜数の辞書選択
                var yakuHanNum = ifr.IsMenzen ? MJUtil.YAKU_HAN_MENZEN : MJUtil.YAKU_HAN_FUROED; 


            
                if( ifr.IsReach && ifr.IsDoubleReach == false )
                {
                    result.yakus.Add( yakuString[(int)MJUtil.yaku.REACH], yakuHanNum[(int)MJUtil.yaku.REACH]);
                }
                if( ifr.IsDoubleReach )
                {
                    result.yakus.Add( yakuString[(int)MJUtil.yaku.DOUBLEREACH], yakuHanNum[(int)MJUtil.yaku.DOUBLEREACH]);
                }
                if( ifr.IsTsumo && ifr.IsMenzen )
                {
                    result.yakus.Add( yakuString[(int)MJUtil.yaku.TSUMO], yakuHanNum[(int)MJUtil.yaku.TSUMO]);
                }
                if( ifr.IsIppatsu )
                {
                    result.yakus.Add( yakuString[(int)MJUtil.yaku.IPPATSU], yakuHanNum[(int)MJUtil.yaku.IPPATSU]);
                }
                if( IsPinfu(horaMentsu, ifr) )
                {
                    result.yakus.Add( yakuString[(int)MJUtil.yaku.PINFU], yakuHanNum[(int)MJUtil.yaku.PINFU]);
                } 
                if( IsTannyao(horaMentsu) )
                {
                    result.yakus.Add( yakuString[(int)MJUtil.yaku.TANNYAO], yakuHanNum[(int)MJUtil.yaku.TANNYAO]);
                } 
                if( IsRyanpeiko(horaMentsu, ifr) )
                {
                     result.yakus.Add( yakuString[(int)MJUtil.yaku.RYANPEIKO], yakuHanNum[(int)MJUtil.yaku.RYANPEIKO]);
                }
                if( IsIipeiko(horaMentsu, ifr) && IsRyanpeiko(horaMentsu, ifr) )
                {
                    result.yakus.Add( yakuString[(int)MJUtil.yaku.IIPEIKOU], yakuHanNum[(int)MJUtil.yaku.IIPEIKOU]);
                }
                if( IsYakuhai(horaMentsu, ifr) )
                {
                    result.yakus.Add( yakuString[(int)MJUtil.yaku.YAKUHAI], CalcYakuhaiNum(horaMentsu,ifr));
                }
                /*

		
                setYAKUHAI( calcYakuhai(horaMentsu, ifr) );
        //		setHOUTEI();
                setHAITEI( ifr.getRestTurn() == 0 );
                setRINSYAN( ifr.isRinshan() );
        //		setCHANKAN();
                setDORA( calcDora(horaMentsu,ifr) );
         
                setSANSYOKUDOJUN( calcSansyokuDoujun(horaMentsu) );
                setITTSUU( calcIttsuu( horaMentsu) );
		
                //kokokara not test yet
                setSANANKO( calcSananko(horaMentsu) );
                setTOITOI( calcToitoi( horaMentsu) );
                setSHOSANGEN( calcShosangen( horaMentsu, ifr ) );
		
		
                setHONROTO( calcHonroto(horaMentsu, ifr) );
                if(getHONROTO() == false){
                    setJUNCHANTA( calcJunchanta(horaMentsu) );
                    setCHANTA( calcChanta( horaMentsu) );
                }
		
                setSANSYOKUDOKO( calcSansyokudoko(horaMentsu) ); 
                setSANKANTSU( calcSankantsu(horaMentsu) );
                setHONNITSU( calcHonnitsu(horaMentsu) );
		
		
                setCHINNITSU( calcChinnitsu(horaMentsu) );
		
		
                setSUUANKO( calcSuuanko(horaMentsu) );
                setDAISANGEN( calcDaisangen(horaMentsu) );
                setSHOSUSI( calcShosushi(horaMentsu));
                setDAISUSI( calcDaisushi(horaMentsu));
                setTSUISO( calcTsuiso(horaMentsu));
                setRYUISO( calcRyuiso(horaMentsu));
                setCHINROTO( calcChinroto(horaMentsu));
                setCHURENPOTO( calcChurenpoto(ifr, horaSyu));
                setSUKANTSU( calcSukantsu(horaMentsu) );
                setTENHO( ifr.getPassedTurn() == 1 && ifr.isNotFuroed());
        //		setCHIHO();
        //		setRENHO();
        //		setSHISANPUTA();
                checkYakuman();
		
		
                setPinfu20fu();
                 * */

                return result;
            }
	
            private static int calcFu(HoraPattern horaMentsu, InfoForResult ifpc) {
                int fuSum = 0;
                int futei = 20;
                fuSum += futei;
		
                if( ifpc.IsMenzen &&( ! ifpc.IsTsumo ) ){
                    fuSum += 10;
                }
		
		
                int head = horaMentsu.TartsuList.Where(e => e.TartsuType == MJUtil.TartsuType.HEAD).First().TartsuStartPaiSyu;
                if( ifpc.IsJifuu(head) ){
                    fuSum += 2;
                }
                if( ifpc.IsJifuu(head) ){
                    fuSum += 2;
                }
                if( MJUtil.IsDragon(head) ){
                    fuSum += 2;
                }
		
                if( ifpc.IsTsumo ){
                    fuSum += 2;
                }
		
		
                int multiple;
                for(int i=1;i<horaMentsu.TartsuList.Count;i++){
                    if( MJUtil.IsYaochu(horaMentsu.TartsuList[i].TartsuStartPaiSyu) ){
                        multiple = 2;
                    }else{
                        multiple = 1;
                    }
			
                    switch ( horaMentsu.TartsuList[i].TartsuType ){
                        case MJUtil.TartsuType.MINKO:
                            fuSum += 2 * multiple;
                            continue;
                        case MJUtil.TartsuType.ANKO:
                            fuSum += 4 * multiple;
                            continue;
                        case MJUtil.TartsuType.MINKANTSU:
                            fuSum += 8 * multiple;
                            continue;
                        case MJUtil.TartsuType.ANKANTSU:
                            fuSum += 16 * multiple;
                            continue;
                    }
                }
		
		
                int lastAddedSyu = ifpc.LastAddedSyu;


                //単騎待ちの場合＋２符
                if( lastAddedSyu == horaMentsu.TartsuList[0].TartsuStartPaiSyu )
                {
                    fuSum += 2;
                }
                else
                {
                    //カンチャンorペンチャンの場合＋２符
                    for(int i=1;i<horaMentsu.TartsuList.Count;i++){
                        if ((horaMentsu.TartsuList[i].TartsuType != MJUtil.TartsuType.ANSYUN) && (horaMentsu.TartsuList[i].TartsuType != MJUtil.TartsuType.MINSYUN))
                        {
                            continue;
                        }
                        //順子前提
                        if( lastAddedSyu == horaMentsu.TartsuList[i].TartsuStartPaiSyu+1 ){//カンチャン
                            fuSum += 2;
                            break;
                        }else if( (lastAddedSyu == horaMentsu.TartsuList[i].TartsuStartPaiSyu)&&(lastAddedSyu % 9 == 6) ){//7待ちの89ペンチャン
                            fuSum += 2;
                            break;
                        }else if( (lastAddedSyu == horaMentsu.TartsuList[i].TartsuStartPaiSyu+2 )&&(lastAddedSyu % 9 == 2) ){//3待ちの12ペンチャン
                            fuSum += 2;
                            break;
                        }
                    }
                }
		
		
                //喰いタンのみ平和系の場合２０符であるが、２符足して３０符に切り上げる必要あり
                if( (fuSum == 20) && (ifpc.IsMenzen == false) ){
                    fuSum += 2;
                }
		
                return (int) ( Math.Ceiling( fuSum/10.0 )*10 );
            }

            private static bool IsPinfu(HoraPattern hp, InfoForResult ifr){
                int headSyu = hp.Head.TartsuStartPaiSyu;

                //頭が役牌でないか判定
                if( ifr.IsBafuu(headSyu) || ifr.IsJifuu(headSyu) || MJUtil.IsDragon(headSyu) )
                {
                    return false;
                }

                //頭もしくは門前順子であるか判定
                foreach(var tartsu in hp.TartsuList){
                    if ( ( tartsu.TartsuType != MJUtil.TartsuType.ANSYUN ) && ( tartsu.TartsuType != MJUtil.TartsuType.HEAD ) ){
                        return false;
                    }
                }

                //リャンメン待ちか判定
                int lastAddedSyu = ifr.LastAddedSyu; 
                foreach(var tartsu in hp.WithoutHeadTartsuList)
                {
                    if(   ( tartsu.TartsuStartPaiSyu == lastAddedSyu) && ( lastAddedSyu % 9 != 6 )    
                        ||( tartsu.TartsuStartPaiSyu == lastAddedSyu - 2) && ( lastAddedSyu % 9 != 2 ) )
                    {
                        return true;
                    }   
                }
                return false;
            }

            private static bool IsTannyao(HoraPattern hp)
            {
                foreach(var tartsu in hp.TartsuList)
                {
                    switch (tartsu.TartsuType)
                    {
                        case MJUtil.TartsuType.ANSYUN:
                            if(  ( tartsu.TartsuStartPaiSyu % 9 ) == 0  || ( tartsu.TartsuStartPaiSyu % 9 ) == 6 ) 
                            {
                                return false;
                            }
                            break;
                        case MJUtil.TartsuType.MINSYUN:
                            if(  ( tartsu.TartsuStartPaiSyu % 9 ) == 0  || ( tartsu.TartsuStartPaiSyu % 9 ) == 6 ) 
                            {
                                return false;
                            }
                            break;
                        default:
                            if( tartsu.TartsuStartPaiSyu >= 27 || ( tartsu.TartsuStartPaiSyu % 9 ) == 0 || ( tartsu.TartsuStartPaiSyu % 9 ) == 8 )
                            {
                                return false;
                            }
                            break;
                    }
                }
                 return true;
            }

            private static bool IsRyanpeiko(HoraPattern hp, InfoForResult ifr)
            {
                //門前でない場合は終了
                if( ifr.IsMenzen == false )
                {
                    return false;
                }

                //4ターツ全てが門前順子でない場合は終了
                if( hp.TartsuList.Select( e => e.TartsuType == MJUtil.TartsuType.ANSYUN ).Count() != 4 )
                {
                    return false;
                }

                
                var sorted = hp.WithoutHeadTartsuList.OrderBy( e => e.TartsuStartPaiSyu ).ToList();

                if( sorted[0].TartsuStartPaiSyu == sorted[1].TartsuStartPaiSyu && sorted[2].TartsuStartPaiSyu == sorted[3].TartsuStartPaiSyu )
                {
                    return true;
                }

                return false;
            }

            private static bool IsIipeiko(HoraPattern hp, InfoForResult ifr)
            {
                //門前でない場合は終了
                if( ifr.IsMenzen == false)
                {
                    return false;
                }


                var ansyuns = hp.WithoutHeadTartsuList.Where(e => e.TartsuType == MJUtil.TartsuType.ANSYUN)
                                                      .OrderBy( e => e.TartsuStartPaiSyu );
                var prevStartPaisyu = -1;
                foreach( var ansyun in ansyuns )
                {
                    if( ansyun.TartsuStartPaiSyu == prevStartPaisyu )
                    {
                        return true;
                    }
                    prevStartPaisyu = ansyun.TartsuStartPaiSyu;
                }
                return false;
            } 

            private static bool IsYakuhai(HoraPattern hp, InfoForResult ifr)
            {
                foreach( var tartsu in hp.TartsuList )
                {
                    if(ifr.jifuuList.Contains(tartsu.TartsuStartPaiSyu))
                    {
                        return true;
                    }
                }
                return false;
            }
            private static int CalcYakuhaiNum(HoraPattern hp, InfoForResult ifr)
            {
                //TODO
                foreach (var tartsu in hp.TartsuList)
                {
                    if (ifr.jifuuList.Contains(tartsu.TartsuStartPaiSyu))
                    {
                    }
                }
                return 0;
            }


        /*

            private void checkYakuman(){
                for(int i=MJUtil.YAKUMANSTART;i<MJUtil.LENGTH_YAKU;i++){
                    if( mYakus[i] ){
                        removeWithoutYakuman();
                        return;
                    }
                }
            }

            private void removeWithoutYakuman() {
                // TODO Auto-generated method stub
                Arrays.fill(mYakus,0, MJUtil.YAKUMANSTART, false);
                setDoraNum(0);
                setYakuhaiNum(0);
            }


            private bool calcChinnitsu(int[][] horaMentsu) {
                bool[] hasMPS = new bool[3];
                int syu =  horaMentsu[0][SYU];//head
                if ( syu >= 27){
                    return false;
                }else if( syu / 9 == 2){
                    hasMPS[2] = true;
                }else if( syu / 9 == 1){
                    hasMPS[1] = true;
                }else if( syu / 9 == 0){
                    hasMPS[0] = true;
                }
		
                for(int i=1;i<horaMentsu.length;i++){
                    syu = horaMentsu[i][SYU];
                    if( (syu / 9 == 2)&&(hasMPS[2]) ){
                        continue;
                    }else if( ( syu / 9 == 1)&&(hasMPS[1])){
                        continue;
                    }else if( ( syu / 9 == 0)&&(hasMPS[0])){
                        continue;
                    }else{
                        return false;
                    }
                }
                return true;
            }
            private bool calcChinnitsu(int[] horaSyu) {
                int manNum=0; 
                int pinNum=0; 
                int souNum=0;
                for(int i=0;i<9;i++){
                    manNum += horaSyu[i];//0~8 is manzu
                    pinNum += horaSyu[i+9];//9~17 is pinzu
                    souNum += horaSyu[i+18];//18~26 is souzu
                }
                return ( manNum == 14 )||( pinNum == 14 )||( souNum == 14 );
            }


            private bool calcSukantsu(int[][] horaMentsu) {
                for(int i=1;i<horaMentsu.length;i++){
                    if(( horaMentsu[i][TYPE] == MJUtil.ANKAN)||( horaMentsu[i][TYPE] == MJUtil.MINKAN)){
                        continue;
                    }else{
                        return false;
                    }
                }
                return true;
            }


            private bool calcChurenpoto(InfoForPointCalc ifpc ,int[] horaSyu) {
                if( ifpc.isMenzen() == false){
                    return false;
                }
		
                for(int mps=0; mps<3; mps++){
                    if( 
                        (horaSyu[0+mps*9]>=3)
                        &&(horaSyu[1+mps*9]>=1)
                        &&(horaSyu[2+mps*9]>=1)
                        &&(horaSyu[3+mps*9]>=1)
                        &&(horaSyu[4+mps*9]>=1)
                        &&(horaSyu[5+mps*9]>=1)
                        &&(horaSyu[6+mps*9]>=1)
                        &&(horaSyu[7+mps*9]>=1)
                        &&(horaSyu[8+mps*9]>=3)
                    ){
                        return true;
                    }
                }
                return false;
            }


            private bool calcChinroto(int[][] horaMentsu) {
                for(int i=0;i<horaMentsu.length;i++){
                    if(MJUtil.IsRoto(horaMentsu[i][SYU]) == false){
                        return false;	
                    }
                }
                return true;
            }


            private bool calcRyuiso(int[][] horaMentsu) {
                for(int i=0;i<horaMentsu.length;i++){
                    //順子は234sのみ受け入れ
                    if( (horaMentsu[i][TYPE] == MJUtil.ANSYUN )||( horaMentsu[i][TYPE] == MJUtil.MINSYUN )){
                        if(horaMentsu[i][SYU] == 19){//2s開始順子はOK
                            continue;
                        }else{
                            return false;
                        }
                    }
			
                    //刻子判定
                    if( MJUtil.IsGreen(horaMentsu[i][SYU]) == false){
                        return false;
                    }
                }
                return true;
            }


            private bool calcTsuiso(int[][] horaMentsu) {
                for(int i=0;i<horaMentsu.length;i++){
                    if( MJUtil.IsJihai(horaMentsu[i][SYU]) == false){
                        return false;
                    }
                }
                return true;
            }
            private bool calcTsuiso(int[] horaSyu) {
                return (horaSyu[27] +
                        horaSyu[28] +
                        horaSyu[29] +
                        horaSyu[30] +
                        horaSyu[31] +
                        horaSyu[32] +
                        horaSyu[33]) == 14;	
			
            }


            private bool calcDaisushi(int[][] horaMentsu) {
                int windCounter = 0;
                for(int i=1;i<horaMentsu.length;i++){
                    if( MJUtil.IsWind(horaMentsu[i][SYU]) ){
                        windCounter++;
                    }
                }
                return windCounter==4;
            }


            private bool calcShosushi(int[][] horaMentsu) {
                if( MJUtil.IsWind(horaMentsu[0][SYU]) == false){
                    return false;
                }
                int windCounter = 0;
                for(int i=1;i<horaMentsu.length;i++){
                    if( MJUtil.IsWind(horaMentsu[i][SYU]) ){
                        windCounter++;
                    }
                }
                return windCounter==3;
            }


            private bool calcDaisangen(int[][] horaMentsu) {
                int dragonCounter = 0;
                for(int i=1;i<horaMentsu.length;i++){
                    if( MJUtil.IsDragon(horaMentsu[i][SYU]) ){
                        dragonCounter++;
                    }
                }
                return dragonCounter==3;
            }


            private bool calcSuuanko(int[][] horaMentsu) {
                for(int i=1;i<horaMentsu.length;i++){
                    if( (horaMentsu[i][TYPE]==MJUtil.ANKO)||(horaMentsu[i][TYPE]==MJUtil.ANKAN) ){
                        continue;
                    }
                    return false;
                }
                return true;
            }





            private bool calcJunchanta(int[][] horaMentsu) {
                for(int i=0;i<horaMentsu.length;i++){
                    switch ( horaMentsu[i][TYPE] ){
                        case MJUtil.ANSYUN:
                        case MJUtil.MINSYUN:
                            if( (horaMentsu[i][SYU]%9 == 0) || (horaMentsu[i][SYU]%9 == 6) ){
                                continue;
                            }
                        default:
                            if(  MJUtil.IsRoto(horaMentsu[i][SYU]) ){
                                continue;
                            }
                    }
                    return false;
                }
                return true;
            }


            private bool calcSankantsu(int[][] horaMentsu) {
                int kantsuCounter = 0;
                for(int i=1;i<horaMentsu.length;i++){
                    switch ( horaMentsu[i][TYPE] ){
                        case MJUtil.ANKAN:
                        case MJUtil.MINKAN:
                            kantsuCounter++;
                    }
                }
                return kantsuCounter >= 3;
            }


            private bool calcSansyokudoko(int[][] horaMentsu) {
                bool[] syu = new bool[MJUtil.LENGTH_SYU];
                for(int i=1;i<horaMentsu.length;i++){
                    switch ( horaMentsu[i][TYPE] ){
                        case MJUtil.ANKO:
                        case MJUtil.MINKO:
                        case MJUtil.ANKAN:
                        case MJUtil.MINKAN:
                            syu[horaMentsu[i][SYU]]=true;
                    }
                }
		
                for(int i=0;i<8;i++){
                    if(syu[i]&&syu[9+i]&&syu[18+i]){
                        return true;
                    }
                }
                return false;
            }


            private bool calcHonroto(int[][] horaMentsu,InfoForPointCalc ifpc) {
                bool hasJihai = false;
                bool hasRoto = false;
                for(int i=0;i<horaMentsu.length;i++){
                    if(MJUtil.IsJihai(horaMentsu[i][SYU])){
                        hasJihai = true;
                        continue;
                    }else if( MJUtil.IsRoto(horaMentsu[i][SYU]) ){
                        hasRoto = true;
                        continue;
                    }
                    return false;
                }
                return hasJihai&&hasRoto;
            }
            private bool calcHonroto(int[] horaSyu) {
                return (horaSyu[0] +
                        horaSyu[8] +
                        horaSyu[9] +
                        horaSyu[17] +
                        horaSyu[18] +
                        horaSyu[26] +
				
                        horaSyu[27] +
                        horaSyu[28] +
                        horaSyu[29] +
                        horaSyu[30] +
                        horaSyu[31] +
                        horaSyu[32] +
                        horaSyu[33]) == 14;	
            }


            private bool calcShosangen(int[][] horaMentsu, InfoForPointCalc ifpc) {
                if( MJUtil.IsDragon(horaMentsu[0][SYU]) == false){
                    return false;
                }
                int dragonCounter = 0;
                for(int i=1;i<horaMentsu.length;i++){
                    if( MJUtil.IsDragon(horaMentsu[i][SYU]) ){
                        dragonCounter++;
                    }
                }
		
                return dragonCounter == 2;
            }


            private bool calcToitoi(int[][] horaMentsu) {
                for(int i=1;i<horaMentsu.length;i++){
                    if( (horaMentsu[i][TYPE] == MJUtil.ANKO)||(horaMentsu[i][TYPE] == MJUtil.ANKAN)
                            ||(horaMentsu[i][TYPE] == MJUtil.MINKO)||(horaMentsu[i][TYPE] == MJUtil.MINKAN) ){
                        continue;
                    }
                    return false;
                }
                return true;
            }


            private bool calcChanta(int[][] horaMentsu) {
                bool hasRotoMentsu = false;
                bool hasJihai = false;
                for(int i=0;i<horaMentsu.length;i++){
                    switch ( horaMentsu[i][TYPE] ){
                        case MJUtil.ANSYUN:
                        case MJUtil.MINSYUN:
                            if( (horaMentsu[i][SYU]%9 == 0) || (horaMentsu[i][SYU]%9 == 6) ){
                                hasRotoMentsu = true;
                                continue;
                            }
                        default:
                            if( MJUtil.IsJihai(horaMentsu[i][SYU]) ){
                                hasJihai = true;
                                continue;
                            }else if( MJUtil.IsRoto(horaMentsu[i][SYU]) ){
                                hasRotoMentsu = true;
                                continue;
                            }
                    }
                    return false;
                }
                return hasRotoMentsu && hasJihai;
            }







            private bool calcSananko(int[][] horaMentsu) {
                // TODO Auto-generated method stub
                int ankoCounter = 0;
                for(int i=1;i<horaMentsu.length;i++){
                    if( (horaMentsu[i][TYPE] == MJUtil.ANKO)||(horaMentsu[i][TYPE] == MJUtil.ANKAN) ){
                        ankoCounter++;
                    }
                }
                return ankoCounter >= 3;
            }


            private bool calcHonnitsu(int[][] horaMentsu) {
                bool[] mpsz = new bool[4];
                for(int i=0;i<horaMentsu.length;i++){
                    int syu =  horaMentsu[i][SYU];
                    if ( syu >= 27){
                        mpsz[3] = true;
                    }else if( syu / 9 == 2){
                        mpsz[2] = true;
                    }else if( syu / 9 == 1){
                        mpsz[1] = true;
                    }else if( syu / 9 == 0){
                        mpsz[0] = true;
                    }
                }
                bool issyoku = ( (mpsz[0]&&!mpsz[1]&&!mpsz[2]) || (!mpsz[0]&&mpsz[1]&&!mpsz[2]) || (!mpsz[0]&&!mpsz[1]&&mpsz[2]) );
                return issyoku&&mpsz[3];
            }
            private bool calcHonnitsu(int[] horaSyu) {
                int manNum=0; 
                int pinNum=0; 
                int souNum=0;
		
                for(int i=0;i<9;i++){
                    manNum += horaSyu[i];//0~8 is manzu
                    pinNum += horaSyu[i+9];//9~17 is pinzu
                    souNum += horaSyu[i+18];//18~26 is souzu
                }
                int jiNum = 14 - manNum + pinNum + souNum;
                return ( manNum+jiNum == 14 )||( pinNum+jiNum == 14 )||( souNum+jiNum == 14 );
            }


            private bool calcIttsuu(int[][] horaMentsu) {
                bool[] syu = new bool[MJUtil.LENGTH_SYU];
                for(int i=0;i<horaMentsu.length;i++){
                    switch ( horaMentsu[i][TYPE] ){
                        case MJUtil.ANSYUN:
                        case MJUtil.MINSYUN:
                            syu[horaMentsu[i][SYU]]=true;
                    }
                }
		
                if( (syu[0]&&syu[3]&&syu[6]) || (syu[9]&&syu[12]&&syu[15]) || (syu[18]&&syu[21]&&syu[24]) ){
                    return true;
                }
                return false;
            }

            private bool calcSansyokuDoujun(int[][] horaMentsu) {
                bool[] syu = new bool[MJUtil.LENGTH_SYU];
                for(int i=1;i<horaMentsu.length;i++){
                    switch ( horaMentsu[i][TYPE] ){
                        case MJUtil.ANSYUN:
                        case MJUtil.MINSYUN:
                            syu[horaMentsu[i][SYU]]=true;
                    }
                }
		
                for(int i=0;i<7;i++){//順子の開始番号は１〜７まで
                    if(syu[i]&&syu[9+i]&&syu[18+i]){
                        return true;
                    }
                }
                return false;
            }

            private bool calcDora(int[][] horaMentsu, InfoForPointCalc ifpc) {
                int[] syuContainFuro = new int[MJUtil.LENGTH_SYU];
                for(int i=0;i<horaMentsu.length;i++){
			
                    if( (horaMentsu[i][MJUtil.HORAMENTSU_TYPE] == MJUtil.ANSYUN)
                      ||(horaMentsu[i][MJUtil.HORAMENTSU_TYPE] == MJUtil.MINSYUN) ){
				
                        int base = horaMentsu[i][MJUtil.HORAMENTSU_SYU];
                        syuContainFuro[base]++;
                        syuContainFuro[base+1]++;
                        syuContainFuro[base+2]++;
				
                    }else if( (horaMentsu[i][MJUtil.HORAMENTSU_TYPE] == MJUtil.ANKO)
                            ||(horaMentsu[i][MJUtil.HORAMENTSU_TYPE] == MJUtil.MINKO) ){
				
                        int base = horaMentsu[i][MJUtil.HORAMENTSU_SYU];
                        syuContainFuro[base]+=3;
				
                    }else if( (horaMentsu[i][MJUtil.HORAMENTSU_TYPE] == MJUtil.ANKAN)
                            ||(horaMentsu[i][MJUtil.HORAMENTSU_TYPE] == MJUtil.MINKAN) ){
				
                        int base = horaMentsu[i][MJUtil.HORAMENTSU_SYU];
                        syuContainFuro[base]+=4;
                    }else{
                        //head
                        int base = horaMentsu[i][MJUtil.HORAMENTSU_SYU];
                        syuContainFuro[base]+=2;
                    }
                }
                int doraNum = ifpc.getDora().getDoraSum(syuContainFuro,ifpc.isReach());
                setDoraNum(doraNum);
                return doraNum >= 1;
            }



            private bool calcIipeikou(int[][] horaMentsu) {
                int[] counter = new int[MJUtil.LENGTH_SYU];
                for(int i=0;i<horaMentsu.length;i++){
                    switch ( horaMentsu[i][TYPE] ){
                        case MJUtil.MINKO:
                        case MJUtil.MINSYUN:
                        case MJUtil.MINKAN:
                            return false;
                        case MJUtil.ANSYUN:
                            counter[horaMentsu[i][SYU]]++;
                            if(counter[horaMentsu[i][SYU]]==2){
                                return true;
                            }
                    }
                }
                return false;
            }


            private bool calcTannyao(int[] horaSyu) {
                return  horaSyu[0] == 0 &&
                        horaSyu[8] == 0 &&
                        horaSyu[9] == 0 &&
                        horaSyu[17] == 0 &&
                        horaSyu[18] == 0 &&
                        horaSyu[26] == 0 &&
				
                        horaSyu[27] == 0 &&
                        horaSyu[28] == 0 &&
                        horaSyu[29] == 0 &&
                        horaSyu[30] == 0 &&
                        horaSyu[31] == 0 &&
                        horaSyu[32] == 0 &&
                        horaSyu[33] == 0;		
            }






            public List<String> getYakuStringList(){
                List<String> yakuStringList = new ArrayList<String>();
                for(int i=0;i<mYakus.length;i++){
                    if(mYakus[i]){
                        if(i == MJUtil.DORA){
                            yakuStringList.add(MJUtil.YAKU_STRING[i] + " " + mDoraNum);
                        }else if(i == MJUtil.YAKUHAI){
                            yakuStringList.add(MJUtil.YAKU_STRING[i] + " " + mYakuhaiNum);
                        }else{
                            yakuStringList.add(MJUtil.YAKU_STRING[i]);
                        }
                    }
                }
                return yakuStringList;
            }
	
            public int getHanSummation(){
                int hanSum = 0;
                int[] hanArray = mIfpc.isMenzen() ? MJUtil.YAKU_HAN_MENZEN : MJUtil.YAKU_HAN_FUROED; 
		
                for(int i=0;i<mYakus.length;i++){
                    if(mYakus[i]){
                        hanSum += hanArray[i];
                    }
                }
                return hanSum + mDoraNum + mYakuhaiNum;
            }
	
            public void setDoraNum(int calcDoraNum) {
                mDoraNum = calcDoraNum;
            }
            public int getDoraNum(){
                return mDoraNum;
            }
	
	
	
	
	
	
	
	
	



            public int getYakumanMultiple() {
                // TODO Auto-generated method stub
                int multiple = 0;
                for(int i=MJUtil.YAKUMANSTART;i<MJUtil.LENGTH_YAKU;i++){
                    if( mYakus[i] ){
                        multiple++;
                    }
                }
                return multiple;
            }
            private void setPinfu20fu(){
                if(getPINFU()){
                    setFu(20);
                }
            }

            public void calcYakuWithChitoitsu(InfoForPointCalc ifpc, int[] horaSyu) {
                // TODO Auto-generated method stub
                setInfoForPointCalc( ifpc ); 
                setREACH(ifpc.isReach()&&(!ifpc.isDoubleReach()) );
                setDOUBLEREACH(ifpc.isDoubleReach());
                setIPPATSU(ifpc.isIppatsu());
                setTSUMO(ifpc.isTsumo()&&ifpc.isMenzen());
                setDORA(calcDoraWithChitoitsu(ifpc,horaSyu));
                setCHINNITSU(calcChinnitsu(horaSyu));
                setHONNITSU(calcHonnitsu(horaSyu));
                setTANNYAO(calcTannyao(horaSyu));
                setHONROTO(calcHonroto(horaSyu));
                setTSUISO(calcTsuiso(horaSyu));
                setHAITEI(ifpc.getRestTurn()==0);
        //		setHOUTEI(ifpc.getRestTurn()==0); solo Mahjong can't houtei
                setTENHO(ifpc.getPassedTurn()==1);
                setCHITOITSU(true);
                setFu(25);
		
                checkYakuman();
            }

            private bool calcDoraWithChitoitsu(InfoForPointCalc ifpc, int[] horaSyu) {
                // TODO Auto-generated method stub
                int doraNum = ifpc.getDora().getDoraSum(horaSyu, ifpc.isReach());
		
		
                setDoraNum(doraNum);
                return doraNum > 0;
            }

	
	*/
	
	

        
    }
}
