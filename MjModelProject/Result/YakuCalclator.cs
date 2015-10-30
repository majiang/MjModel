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
    public class YakuDetail
    {

        /*
        public Dictionary<string,int> yakus = new Dictionary<string,int>();1
        private int mHan = 0;
        private int mFu = 0;
        private int mDoraNum = 0;
        private int mYakuhaiNum = 0;
        private InfoForPointCalc mIfpc = new InfoForPointCalc();
	
	

        public void init(){
            yakus.Clear();
            mHan = 0;
            mFu = 0;
            mDoraNum = 0;
            mYakuhaiNum = 0;
        }
	

        private int calcFu(List<Tartsu> horaMentsu, InfoForPointCalc ifpc) {
            int fuSum = 0;
            int futei = 20;
            fuSum += futei;
		
            if( ifpc.isMenzen()&&( ! ifpc.isTsumo() ) ){
                fuSum += 10;
            }
		
		
            int head = horaMentsu.Where(e => e.TartsuType == MJUtil.TartsuType.HEAD).First().TartsuStartPaiSyu;
            if( ifpc.isJifuu(head) ){
                fuSum += 2;
            }
            if( ifpc.isBafuu(head) ){
                fuSum += 2;
            }
            if( MJUtil.isDragon(head) ){
                fuSum += 2;
            }
		
            if( ifpc.isTsumo() ){
                fuSum += 2;
            }
		
		
            int multiple;
            for(int i=1;i<horaMentsu.Count;i++){
                if( MJUtil.isYaochu(horaMentsu[i].TartsuStartPaiSyu) ){
                    multiple = 2;
                }else{
                    multiple = 1;
                }
			
                switch ( horaMentsu[i].TartsuType ){
                    case MJUtil.TartsuType.Minko:
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
		
		
            int lastAddedSyu = ifpc.getLastAddedPai().getSyu();


            //単騎待ちの場合＋２符
            if( lastAddedSyu == horaMentsu[0].TartsuStartPaiSyu )
            {
                fuSum += 2;
            }
            else
            {
                //カンチャンorペンチャンの場合＋２符
                for(int i=1;i<horaMentsu.Count;i++){
                    if( ( horaMentsu[i].TartsuType != MJUtil.TartsuType.ANSYUN )&&( horaMentsu[i].TartsuType != MJUtil.TartsuType.MINSYUN ) ){
                        continue;
                    }
                    //順子前提
                    if( lastAddedSyu == horaMentsu[i].TartsuStartPaiSyu+1 ){//カンチャン
                        fuSum += 2;
                        break;
                    }else if( (lastAddedSyu == horaMentsu[i].TartsuStartPaiSyu)&&(lastAddedSyu % 9 == 6) ){//7待ちの89ペンチャン
                        fuSum += 2;
                        break;
                    }else if( (lastAddedSyu == horaMentsu[i].TartsuStartPaiSyu+2 )&&(lastAddedSyu % 9 == 2) ){//3待ちの12ペンチャン
                        fuSum += 2;
                        break;
                    }
                }
            }
		
		
            //喰いタンのみ平和系の場合２０符であるが、２符足して３０符に切り上げる必要あり
            if( (fuSum == 20) && (ifpc.isMenzen()==false) ){
                fuSum += 2;
            }
		
            return (int) ( Math.Ceiling( fuSum/10.0 )*10 );
        }
	
        public void calcYaku(int[][] horaMentsu, InfoForPointCalc ifpc,int[] horaSyu){
            setFu( calcFu( horaMentsu, ifpc ) );
            setInfoForPointCalc( ifpc ); 
            //noemal yaku
            setREACH( ifpc.isReach()&&(!ifpc.isDoubleReach()) );
            setTSUMO( ifpc.isTsumo()&&ifpc.isMenzen() );
            setIPPATSU( ifpc.isIppatsu() );
            setPINFU( calcPinfu(horaMentsu, ifpc) );
            setTANNYAO( calcTannyao(horaMentsu) );
		
		
            setRYANPEIKO( calcRyanpeiko(horaMentsu) );
            if(getRYANPEIKO()==false){
                setIIPEIKOU( calcIipeikou(horaMentsu) );
            }
		
            setYAKUHAI( calcYakuhai(horaMentsu, ifpc) );
    //		setHOUTEI();
            setHAITEI( ifpc.getRestTurn() == 0 );
            setRINSYAN( ifpc.isRinshan() );
    //		setCHANKAN();
            setDORA( calcDora(horaMentsu,ifpc) );
            setDOUBLEREACH( ifpc.isDoubleReach() );
            setSANSYOKUDOJUN( calcSansyokuDoujun(horaMentsu) );
            setITTSUU( calcIttsuu( horaMentsu) );
		
            //kokokara not test yet
            setSANANKO( calcSananko(horaMentsu) );
            setTOITOI( calcToitoi( horaMentsu) );
            setSHOSANGEN( calcShosangen( horaMentsu, ifpc ) );
		
		
            setHONROTO( calcHonroto(horaMentsu, ifpc) );
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
            setCHURENPOTO( calcChurenpoto(ifpc, horaSyu));
            setSUKANTSU( calcSukantsu(horaMentsu) );
            setTENHO( ifpc.getPassedTurn() == 1 && ifpc.isNotFuroed());
    //		setCHIHO();
    //		setRENHO();
    //		setSHISANPUTA();
            checkYakuman();
		
		
            setPinfu20fu();
        }
	
        private void setPinfu20fu(){
            if(getPINFU()){
                setFu(20);
            }
        }

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
                if(MJUtil.isRoto(horaMentsu[i][SYU]) == false){
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
                if( MJUtil.isGreen(horaMentsu[i][SYU]) == false){
                    return false;
                }
            }
            return true;
        }


        private bool calcTsuiso(int[][] horaMentsu) {
            for(int i=0;i<horaMentsu.length;i++){
                if( MJUtil.isJihai(horaMentsu[i][SYU]) == false){
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
                if( MJUtil.isWind(horaMentsu[i][SYU]) ){
                    windCounter++;
                }
            }
            return windCounter==4;
        }


        private bool calcShosushi(int[][] horaMentsu) {
            if( MJUtil.isWind(horaMentsu[0][SYU]) == false){
                return false;
            }
            int windCounter = 0;
            for(int i=1;i<horaMentsu.length;i++){
                if( MJUtil.isWind(horaMentsu[i][SYU]) ){
                    windCounter++;
                }
            }
            return windCounter==3;
        }


        private bool calcDaisangen(int[][] horaMentsu) {
            int dragonCounter = 0;
            for(int i=1;i<horaMentsu.length;i++){
                if( MJUtil.isDragon(horaMentsu[i][SYU]) ){
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


        private bool calcRyanpeiko(int[][] horaMentsu) {
            int[] syuntsuCounter = new int[MJUtil.LENGTH_SYU];
            bool isIipeiko = false;
            for(int i=1;i<horaMentsu.length;i++){
                switch ( horaMentsu[i][TYPE] ){
                    case MJUtil.MINKAN:
                    case MJUtil.MINKO:
                    case MJUtil.MINSYUN:
                    case MJUtil.ANKO:
                    case MJUtil.ANKAN:
                        return false;
					
                    case MJUtil.ANSYUN:
                        syuntsuCounter[horaMentsu[i][SYU]]++;			
                        if(syuntsuCounter[horaMentsu[i][SYU]]==2){
                            if( isIipeiko ){
                                return true;
                            }
                            isIipeiko = true;
                        }else if(syuntsuCounter[horaMentsu[i][SYU]]==4){
                            return true;
                        }
                }
            }
            return false;
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
                        if(  MJUtil.isRoto(horaMentsu[i][SYU]) ){
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
                if(MJUtil.isJihai(horaMentsu[i][SYU])){
                    hasJihai = true;
                    continue;
                }else if( MJUtil.isRoto(horaMentsu[i][SYU]) ){
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
            if( MJUtil.isDragon(horaMentsu[0][SYU]) == false){
                return false;
            }
            int dragonCounter = 0;
            for(int i=1;i<horaMentsu.length;i++){
                if( MJUtil.isDragon(horaMentsu[i][SYU]) ){
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
                        if( MJUtil.isJihai(horaMentsu[i][SYU]) ){
                            hasJihai = true;
                            continue;
                        }else if( MJUtil.isRoto(horaMentsu[i][SYU]) ){
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

        private bool calcYakuhai(int[][] horaMentsu,InfoForPointCalc ifpc) {
            // TODO Auto-generated method stub
            int yakuhaiNum = 0;
            for(int i=1;i<horaMentsu.length;i++){
                switch ( horaMentsu[i][TYPE] ){
                    case MJUtil.MINKO:
                    case MJUtil.ANKO:
                    case MJUtil.MINKAN:
                    case MJUtil.ANKAN:
                        //自風と場風は両方trueで２役分の事がある（ダブ東、ダブ南）
                        //よってif文２つで両方調べる必要がある
                        if( ifpc.isJifuu(horaMentsu[i][SYU]) ){
                            yakuhaiNum++;
                        }
                        if( ifpc.isBafuu(horaMentsu[i][SYU]) ){
                            yakuhaiNum++;
                        }
					
                        if( MJUtil.isDragon(horaMentsu[i][SYU]) ){
                            yakuhaiNum++;
                        }
                }
            }
            setYakuhaiNum(yakuhaiNum);
            return getYakuhaiNum() >= 1;
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

        private bool calcTannyao(int[][] horaMentsu) {
            for(int i=0;i<horaMentsu.length;i++){
                switch ( horaMentsu[i][TYPE] ){
                    case MJUtil.ANSYUN:
                    case MJUtil.MINSYUN:
                        if( (horaMentsu[i][SYU]%9 == 0) || (horaMentsu[i][SYU]%9 == 6) ){
                            return false;
                        }
                    default:
                        if( (horaMentsu[i][SYU] >= 27) || (horaMentsu[i][SYU]%9 == 0) || (horaMentsu[i][SYU]%9 == 8) ){
                            return false;
                        }
                }
            }
            return true;
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

        private bool calcPinfu(int[][] horaMentsu, InfoForPointCalc ifpc) {
            int head = horaMentsu[0][SYU];
            if(  ifpc.isBafuu(head)||ifpc.isJifuu(head)||MJUtil.isDragon(head) ){
                return false;
            }
		
            for(int i=1;i<horaMentsu.length;i++){
                if ( horaMentsu[i][TYPE] != MJUtil.ANSYUN ){
                    return false;
                }
            }

            //リャンメン待ちか判定
            int lastAddedSyu = ifpc.getLastAddedPai().getSyu(); 
            bool tempresult=true;
            for(int i=1;i<horaMentsu.length;i++){
                if ( ( ( horaMentsu[i][SYU] == lastAddedSyu) && !(lastAddedSyu % 9 == 6) )
                    || (( horaMentsu[i][SYU] +2 == lastAddedSyu) && !(lastAddedSyu % 9 == 2))  ) 
                {
                    return true;
                }
            }
            return false;
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
	
	
        public void setFu(int fu){
            mFu = fu;
        }
        public int getFu(){
            return mFu;
        }
	
	
	
	
	
	
	
	
	
	
        public void setREACH(bool bool){
            mYakus[MJUtil.REACH] = bool;
        }
        public bool getREACH(){
            return mYakus[MJUtil.REACH];
        }
	
        public void setTSUMO(bool bool){
            mYakus[MJUtil.TSUMO] = bool;
        }
        public bool getTSUMO(){
            return mYakus[MJUtil.TSUMO];
        }
	
        public void setIPPATSU(bool bool){
            mYakus[MJUtil.IPPATSU] = bool;
        }
        public bool getIPPATSU(){
            return mYakus[MJUtil.IPPATSU];
        }
	
        public void setPINFU(bool bool){
            mYakus[MJUtil.PINFU] = bool;
        }
        public bool getPINFU(){
            return mYakus[MJUtil.PINFU];
        }
	
        public void setTANNYAO(bool bool){
            mYakus[MJUtil.TANNYAO] = bool;
        }
        public bool getTANNYAO(){
            return mYakus[MJUtil.TANNYAO];
        }
	
        public void setIIPEIKOU(bool bool){
            mYakus[MJUtil.IIPEIKOU] = bool;
        }
        public bool getIIPEIKOU(){
            return mYakus[MJUtil.IIPEIKOU];
        }
	
        public void setYAKUHAI(bool bool){
            mYakus[MJUtil.YAKUHAI] = bool;
        }
        public bool getYAKUHAI(){
            return mYakus[MJUtil.YAKUHAI];
        }
	
        public void setHOUTEI(bool bool){
            mYakus[MJUtil.HOUTEI] = bool;
        }
        public bool getHOUTEI(){
            return mYakus[MJUtil.HOUTEI];
        }
	
        public void setHAITEI(bool bool){
            mYakus[MJUtil.HAITEI] = bool;
        }
        public bool getHAITEI(){
            return mYakus[MJUtil.HAITEI];
        }
	
        public void setRINSYAN(bool bool){
            mYakus[MJUtil.RINSYAN] = bool;
        }
        public bool getRINSYAN(){
            return mYakus[MJUtil.RINSYAN];
        }
	
        public void setCHANKAN(bool bool){
            mYakus[MJUtil.CHANKAN] = bool;
        }
        public bool getCHANKAN(){
            return mYakus[MJUtil.CHANKAN];
        }
	
        public void setDORA(bool bool){
            mYakus[MJUtil.DORA] = bool;
        }
        public bool getDORA(){
            return mYakus[MJUtil.DORA];
        }
	
        public void setDOUBLEREACH(bool bool){
            mYakus[MJUtil.DOUBLEREACH] = bool;
        }
        public bool getDOUBLEREACH(){
            return mYakus[MJUtil.DOUBLEREACH];
        }
	
        public void setSANSYOKUDOJUN(bool bool){
            mYakus[MJUtil.SANSYOKUDOJUN] = bool;
        }
        public bool getSANSYOKUDOJUN(){
            return mYakus[MJUtil.SANSYOKUDOJUN];
        }
	
        public void setITTSUU(bool bool){
            mYakus[MJUtil.ITTSUU] = bool;
        }
        public bool getITTSUU(){
            return mYakus[MJUtil.ITTSUU];
        }
	
        public void setSANANKO(bool bool){
            mYakus[MJUtil.SANANKO] = bool;
        }
        public bool getSANANKO(){
            return mYakus[MJUtil.SANANKO];
        }
	
        public void setCHANTA(bool bool){
            mYakus[MJUtil.CHANTA] = bool;
        }
        public bool getCHANTA(){
            return mYakus[MJUtil.CHANTA];
        }
	
        public void setCHITOITSU(bool bool){
            mYakus[MJUtil.CHITOITSU] = bool;
        }
        public bool getCHITOITSU(){
            return mYakus[MJUtil.CHITOITSU];
        }
	
        public void setTOITOI(bool bool){
            mYakus[MJUtil.TOITOI] = bool;
        }
        public bool getTOITOI(){
            return mYakus[MJUtil.TOITOI];
        }
	
        public void setSHOSANGEN(bool bool){
            mYakus[MJUtil.SHOSANGEN] = bool;
        }
        public bool getSHOSANGEN(){
            return mYakus[MJUtil.SHOSANGEN];
        }
	
        public void setHONROTO(bool bool){
            mYakus[MJUtil.HONROTO] = bool;
        }
        public bool getHONROTO(){
            return mYakus[MJUtil.HONROTO];
        }
	
        public void setSANSYOKUDOKO(bool bool){
            mYakus[MJUtil.SANSYOKUDOKO] = bool;
        }
        public bool getSANSYOKUDOKO(){
            return mYakus[MJUtil.SANSYOKUDOKO];
        }
	
        public void setSANKANTSU(bool bool){
            mYakus[MJUtil.SANKANTSU] = bool;
        }
        public bool getSANKANTSU(){
            return mYakus[MJUtil.SANKANTSU];
        }
	
        public void setHONNITSU(bool bool){
            mYakus[MJUtil.HONNITSU] = bool;
        }
        public bool getHONNITSU(){
            return mYakus[MJUtil.HONNITSU];
        }
	
        public void setJUNCHANTA(bool bool){
            mYakus[MJUtil.JUNCHANTA] = bool;
        }
        public bool getJUNCHANTA(){
            return mYakus[MJUtil.JUNCHANTA];
        }
	
        public void setRYANPEIKO(bool bool){
            mYakus[MJUtil.RYANPEIKO] = bool;
        }
        public bool getRYANPEIKO(){
            return mYakus[MJUtil.RYANPEIKO];
        }
	
        public void setNAGASHIMANGAN(bool bool){
            mYakus[MJUtil.NAGASHIMANGAN] = bool;
        }
        public bool getNAGASHIMANGAN(){
            return mYakus[MJUtil.NAGASHIMANGAN];
        }
	
        public void setCHINNITSU(bool bool){
            mYakus[MJUtil.CHINNITSU] = bool;
        }
        public bool getCHINNITSU(){
            return mYakus[MJUtil.CHINNITSU];
        }
	
        public void setSUUANKO(bool bool){
            mYakus[MJUtil.SUUANKO] = bool;
        }
        public bool getSUUANKO(){
            return mYakus[MJUtil.SUUANKO];
        }
	
        public void setKOKUSHIMUSO(bool bool){
            mYakus[MJUtil.KOKUSHIMUSO] = bool;
        }
        public bool getKOKUSHIMUSO(){
            return mYakus[MJUtil.KOKUSHIMUSO];
        }
	
        public void setDAISANGEN(bool bool){
            mYakus[MJUtil.DAISANGEN] = bool;
        }
        public bool getDAISANGEN(){
            return mYakus[MJUtil.DAISANGEN];
        }
	
        public void setSHOSUSI(bool bool){
            mYakus[MJUtil.SHOSUSI] = bool;
        }
        public bool getSHOSUSI(){
            return mYakus[MJUtil.SHOSUSI];
        }
        public void setDAISUSI(bool bool){
            mYakus[MJUtil.DAISUSI] = bool;
        }
        public bool getDAISUSI(){
            return mYakus[MJUtil.DAISUSI];
        }
	
	
        public void setTSUISO(bool bool){
            mYakus[MJUtil.TSUISO] = bool;
        }
        public bool getTSUISO(){
            return mYakus[MJUtil.TSUISO];
        }
	
        public void setRYUISO(bool bool){
            mYakus[MJUtil.RYUISO] = bool;
        }
        public bool getRYUISO(){
            return mYakus[MJUtil.RYUISO];
        }
	
        public void setCHINROTO(bool bool){
            mYakus[MJUtil.CHINROTO] = bool;
        }
        public bool getCHINROTO(){
            return mYakus[MJUtil.CHINROTO];
        }
	
        public void setCHURENPOTO(bool bool){
            mYakus[MJUtil.CHURENPOTO] = bool;
        }
        public bool getCHURENPOTO(){
            return mYakus[MJUtil.CHURENPOTO];
        }
	
        public void setSUKANTSU(bool bool){
            mYakus[MJUtil.SUKANTSU] = bool;
        }
        public bool getSUKANTSU(){
            return mYakus[MJUtil.SUKANTSU];
        }
	
        public void setTENHO(bool bool){
            mYakus[MJUtil.TENHO] = bool;
        }
        public bool getTENHO(){
            return mYakus[MJUtil.TENHO];
        }
	
        public void setCHIHO(bool bool){
            mYakus[MJUtil.CHIHO] = bool;
        }
        public bool getCHIHO(){
            return mYakus[MJUtil.CHIHO];
        }
	
        public void setRENHO(bool bool){
            mYakus[MJUtil.RENHO] = bool;
        }
        public bool getRENHO(){
            return mYakus[MJUtil.RENHO];
        }
	
        public void setSHISANPUTA(bool bool){
            mYakus[MJUtil.SHISANPUTA] = bool;
        }
        public bool getSHISANPUTA(){
            return mYakus[MJUtil.SHISANPUTA];
        }

        public int getYakuhaiNum() {
            return mYakuhaiNum;
        }

        public void setYakuhaiNum(int mYakuhaiNum) {
            this.mYakuhaiNum = mYakuhaiNum;
        }


        public InfoForPointCalc getInfoForPointCalc() {
            return mIfpc;
        }


        public void setInfoForPointCalc(InfoForPointCalc mIfpc) {
            this.mIfpc = mIfpc;
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

	
	
	
	
	
    }

        */
        
    }
}
