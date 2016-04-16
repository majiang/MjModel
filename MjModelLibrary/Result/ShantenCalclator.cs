using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MjModelLibrary
{
    public class ShantenCalclator {
        const int SYU_LENGTH = 9;
        const int JI_LENGTH = 7;
        const int SHANTEN_MAX_NORMAL = 8;
        const int SHANTEN_MAX_CHITOITSU = 6;
        const int SHANTEN_MAX_KOKUSHI = 13;
        const int CHITOITSU_SYU_NUM = 7;
        private static ShantenCalclator ss;

	    private ShantenCalclator(){}

    

        public bool UseChitoitsu { get; set; }
        public bool UseKokushi { get; set; }

        public static ShantenCalclator GetInstance()
        {
            if (ss == null)
            {
                ss = new ShantenCalclator();
            }

            return ss;
        }

        public int CalcShanten(int[] syu){
		    return CalcShantenWithFuro(syu,0);
	    }

        public int CalcShanten(Tehai tehai, string pai)
        {
            var syu = new int[MJUtil.LENGTH_SYU_ALL];
            foreach (var p in tehai.tehai)
            {
                syu[p.PaiNumber]++;
            }
            syu[PaiConverter.STRING_TO_ID[pai]]++;
            return CalcShantenWithFuro(syu, tehai.furos.Count);
        }


        public int CalcShanten(Tehai tehai){
            var syu = new int[MJUtil.LENGTH_SYU_ALL];
            foreach (var p in tehai.tehai)
            {
                syu[p.PaiNumber]++;
            }

            return CalcShantenWithFuro(syu, tehai.furos.Count);
        }

	    public int CalcShantenWithFuro(int[] syu,int furoNum){

            int[] workSyu = new int[syu.Length];
            Array.Copy(syu, workSyu, syu.Length);
		

		    int minSyantenWithHead = CalcNormalShanten(workSyu,furoNum);

            int minSyantenWithoutHead = SHANTEN_MAX_NORMAL;
		    for (int i = 0; i < workSyu.Length; i++) {
			    if( workSyu[i] >= 2 ){
				    workSyu[i] -= 2;
				    int tempMin = CalcNormalShanten(workSyu,furoNum) - 1;
				    if(minSyantenWithoutHead > tempMin){
					    minSyantenWithoutHead = tempMin;
				    }
				    workSyu[i] += 2;
			    }
		    }

            int syantenChitoitsu = SHANTEN_MAX_CHITOITSU;
            int syantenKokushi = SHANTEN_MAX_KOKUSHI;
		    if(furoNum==0)
		    {
                if (UseChitoitsu)
                {
                    syantenChitoitsu = CalcChitoitsuShanten(syu);
                }
                if (UseKokushi)
                {
                    syantenKokushi = CalcKokushiShanten(syu);
                }
		    }
		    int syantenNotNormal = Math.Min(syantenChitoitsu,syantenKokushi);
		    int syantenNormal = Math.Min(minSyantenWithHead,minSyantenWithoutHead);
		
		    return Math.Min(syantenNotNormal,syantenNormal);
	    }
	
	    private int CalcChitoitsuShanten(int[] syu)
	    {
            int shanten = SHANTEN_MAX_CHITOITSU;
		    int syuNum = 0;
		
		
		    for(int i=0;i<syu.Length;i++)
		    {
                if (syu[i] >= 1)
                {
                    syuNum++;
                    if (syu[i] >= 2)
                    {
                        shanten--;
                    }
                }
		    }
            //3枚以上の牌が複数あり、待ち候補牌が無い場合を想定
            //ex.111122255m55577p→チートイツ2シャンテン
		    if(CHITOITSU_SYU_NUM - syuNum > shanten){
			    shanten = CHITOITSU_SYU_NUM - syuNum;
		    }
		    return shanten;
	    }
	
	    private int CalcKokushiShanten(int[] syu)
	    {
            int shanten = SHANTEN_MAX_KOKUSHI;
            int[] target = { 0, 8, 9, 17, 18, 26, 27, 28, 29, 30, 31, 32, 33 };//Kokushi effective tiles
		    bool hasHead = false;
		
		    for(int i=0;i<target.Length;i++)
		    {
			    if(syu[ (target[i]) ]>=1)
			    {
				    shanten--;
			    }
			
			    if( (syu[ (target[i]) ]>=2) )
			    {
				    hasHead=true;
			    }
		    }
		    if(hasHead)
		    {
			    shanten--;
		    }
		
		    return shanten;
	    }
	
	    private int CalcNormalShanten(int[] syu,int furoNum){
            int minSyanten = SHANTEN_MAX_NORMAL;



            int[] man = new int[SYU_LENGTH];
            int[] pin = new int[SYU_LENGTH];
            int[] sou = new int[SYU_LENGTH];
            int[] ji = new int[JI_LENGTH];

            Array.Copy(syu, 0, man, 0, SYU_LENGTH); 
            Array.Copy(syu, SYU_LENGTH  , pin, 0, SYU_LENGTH);
            Array.Copy(syu, SYU_LENGTH*2, sou, 0, SYU_LENGTH);
            Array.Copy(syu, SYU_LENGTH*3, ji , 0, JI_LENGTH);

            int[] manResult = CountBaseMemtsuNum(man, new int[2], new int[4]);//Result[4] = {mentsuNum_coefficient2,kouhoNum_coef2,mentsNum_coef10,kouhoNum,coef10}
            int[] pinResult = CountBaseMemtsuNum(pin, new int[2], new int[4]);
            int[] souResult = CountBaseMemtsuNum(sou, new int[2], new int[4]);
		    int[] jiResult  = CalcMentsuJi(ji);

		
		    for (int i = 0; i < 2; i++) {
			    for (int j = 0; j < 2; j++) {
				    for (int k = 0; k < 2; k++) {
					
					    int mentsNum = manResult[i*2]+pinResult[j*2]+souResult[k*2]+jiResult[0];
					    int kouhoNum = manResult[i*2+1]+pinResult[j*2+1]+souResult[k*2+1]+jiResult[1];
					    if( (mentsNum+kouhoNum+furoNum) > 4 ){
						    kouhoNum = 4 - mentsNum - furoNum;
					    }
                        int tempSyanten = SHANTEN_MAX_NORMAL - mentsNum * 2 - furoNum * 2 - kouhoNum;
					    if(minSyanten > tempSyanten){
						    minSyanten = tempSyanten;
					    }
				    }
			    }
		    }


		    return minSyanten;
	    }
	    private int[] CountBaseMemtsuNum(int[] numberPai,int[] tempResult,int[] maxResult){
		    int[] mentsuNum = new int[4];
            mentsuNum = CountMentsuNum(0,numberPai,new int[2],new int[4]);//Result[4] = {mentsuNum_coefficient2,kouhoNum_coef2,mentsNum_coef10,kouhoNum,coef10}
            return mentsuNum;
	    }
	
	    private int[] CountMentsuNum(int startIdx,int[] numberPai,int[] tempResult,int[] maxResult){

            for (int i = startIdx; i < SYU_LENGTH; i++)
            {
			
			    //anko
			    if( numberPai[i] >= 3 ){
				    numberPai[i] -= 3;
				    tempResult[0]++;
				    if( (tempResult[0]*2+tempResult[1]) > (maxResult[0]*2+maxResult[1] ) ){//coef = 2
					    maxResult[0] = tempResult[0];
					    maxResult[1] = tempResult[1];
				    }
				    if( (tempResult[0]*10+tempResult[1]) > (maxResult[2]*10+maxResult[3] ) ){//coef = 10
					    maxResult[2] = tempResult[0];
					    maxResult[3] = tempResult[1];
				    }
				    CountMentsuNum(i,numberPai, tempResult,maxResult);
				    numberPai[i] += 3;
				    tempResult[0]--;
			    }
			
			    //syuntsu
			    if(  ( i<7 )  &&  ( (numberPai[i]>=1)&&(numberPai[i+1]>=1)&&(numberPai[i+2]>=1) )  ){
				    numberPai[i]--;
				    numberPai[i+1]--;
				    numberPai[i+2]--;
				    tempResult[0]++;
				    if( (tempResult[0]*2+tempResult[1]) > (maxResult[0]*2+maxResult[1] ) ){
					    maxResult[0] = tempResult[0];
					    maxResult[1] = tempResult[1];
				    }
				    if( (tempResult[0]*10+tempResult[1]) > (maxResult[2]*10+maxResult[3] ) ){
					    maxResult[2] = tempResult[0];
					    maxResult[3] = tempResult[1];
				    }
				    CountMentsuNum(i,numberPai, tempResult, maxResult);
				    numberPai[i]++;
				    numberPai[i+1]++;
				    numberPai[i+2]++;
				    tempResult[0]--;
			    }
			
			    //kotsu
			    if( numberPai[i] >= 2 ){
				    numberPai[i] -= 2;
				    tempResult[1]++;
				    if( (tempResult[0]*2+tempResult[1]) > (maxResult[0]*2+maxResult[1] ) ){
					    maxResult[0] = tempResult[0];
					    maxResult[1] = tempResult[1];
				    }
				    if( (tempResult[0]*10+tempResult[1]) > (maxResult[2]*10+maxResult[3] ) ){
					    maxResult[2] = tempResult[0];
					    maxResult[3] = tempResult[1];
				    }
				    CountMentsuNum(i,numberPai, tempResult,maxResult);
				    numberPai[i] += 2;
				    tempResult[1]--;
			    }
			
			    //kanchan
			    if(  ( i<7 )  &&  ( (numberPai[i]>=1)&&(numberPai[i+2]>=1) )  ){
				    numberPai[i]--;
				    numberPai[i+2]--;
				    tempResult[1]++;
				    if( (tempResult[0]*2+tempResult[1]) > (maxResult[0]*2+maxResult[1] ) ){
					    maxResult[0] = tempResult[0];
					    maxResult[1] = tempResult[1];
				    }
				    if( (tempResult[0]*10+tempResult[1]) > (maxResult[2]*10+maxResult[3] ) ){
					    maxResult[2] = tempResult[0];
					    maxResult[3] = tempResult[1];
				    }
				    CountMentsuNum(i,numberPai, tempResult,maxResult);
				    numberPai[i]++;
				    numberPai[i+2]++;
				    tempResult[1]--;
			    }
			
			    //ryanmen or penchan
			    if(  ( i<8 )  &&  ( (numberPai[i]>=1)&&(numberPai[i+1]>=1) )  ){
				    numberPai[i]--;
				    numberPai[i+1]--;
				    tempResult[1]++;
				    if( (tempResult[0]*2+tempResult[1]) > (maxResult[0]*2+maxResult[1] ) ){
					    maxResult[0] = tempResult[0];
					    maxResult[1] = tempResult[1];
				    }
				    if( (tempResult[0]*10+tempResult[1]) > (maxResult[2]*10+maxResult[3] ) ){
					    maxResult[2] = tempResult[0];
					    maxResult[3] = tempResult[1];
				    }
				    CountMentsuNum(i,numberPai, tempResult,maxResult);
				    numberPai[i]++;
				    numberPai[i+1]++;
				    tempResult[1]--;
			    }
		    }
		
		    return maxResult;
	    }
	
	    private int[] CalcMentsuJi(int[] ji){
		    int[] result = new int[2];
		
		    for (int i = 0; i < ji.Length; i++) {
			    if( ji[i] >= 3 ){
				    result[0]++;
			    }else if( ji[i] >= 2 ){
				    result[1]++;
			    }
		    }
		    return result;
	    }
    }

}
