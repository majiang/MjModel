
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MjModelProject.Util
{
public class MJUtil {
	public static readonly int ANSYUN = 1;
	public static readonly int MINSYUN = 2;
	public static readonly int ANKO = 3;
	public static readonly int MINKO = 4;
	public static readonly int ANKAN = 5;
	public static readonly int MINKAN = 6;
	public static readonly int HEAD = 7;
	public static readonly int KANCHAN = 8;
	public static readonly int NARABI = 9;
	public static readonly int TOITSU = 10;
	


	
	public static readonly int HORAMENTSU_TYPE = 0;
	public static readonly int HORAMENTSU_SYU = 1;
	
	public static readonly int LENGTH_ID = 144;
	public static readonly int LENGTH_SYU = 34;
	public static readonly int LENGTH_HAIPAI = 13;
	public static readonly int LENGTH_TEHAI = 14;
	
	public static readonly int REACH = 1; 
	public static readonly int TSUMO = 2; 
	public static readonly int IPPATSU = 3; 
	public static readonly int PINFU = 4; 
	public static readonly int TANNYAO = 5; 
	public static readonly int IIPEIKOU = 6; 
	public static readonly int YAKUHAI = 7; 
	public static readonly int HOUTEI = 8; 
	public static readonly int HAITEI = 9; 
	public static readonly int RINSYAN = 10; 
	public static readonly int CHANKAN = 11; 
	
	public static readonly int DOUBLEREACH = 12; 
	public static readonly int SANSYOKUDOJUN = 13; 
	public static readonly int ITTSUU = 14; 
	public static readonly int SANANKO = 15; 
	public static readonly int CHANTA = 16; 
	public static readonly int CHITOITSU = 17; 
	public static readonly int TOITOI = 18; 
	public static readonly int SHOSANGEN = 19; 
	public static readonly int HONROTO = 20; 
	public static readonly int SANSYOKUDOKO = 21; 
	public static readonly int SANKANTSU = 22; 
	public static readonly int HONNITSU = 23; 
	public static readonly int JUNCHANTA = 24; 
	public static readonly int RYANPEIKO = 25; 
	public static readonly int NAGASHIMANGAN = 26; 
	public static readonly int CHINNITSU = 27; 
	public static readonly int DORA = 28;
	
	
	//YAKUMAN
	public static readonly int YAKUMANSTART = 29; 
	public static readonly int SUUANKO 	= 29; 
	public static readonly int KOKUSHIMUSO = 30; 
	public static readonly int DAISANGEN 	= 31; 
	public static readonly int SHOSUSI 	= 32; 
	public static readonly int DAISUSI 	= 33; 
	public static readonly int TSUISO 		= 34;
	public static readonly int RYUISO 		= 35; 
	public static readonly int CHINROTO 	= 36; 
	public static readonly int CHURENPOTO 	= 37; 
	public static readonly int SUKANTSU 	= 38; 
	public static readonly int TENHO 		= 39; 
	public static readonly int CHIHO 		= 40; 
	public static readonly int RENHO 		= 41; 
	public static readonly int SHISANPUTA 	= 42;
	 
	public static readonly int LENGTH_YAKU = 43;

	
	public static readonly int PAI_URA_ID = 136;
	public static readonly int PAI_SOURCE_SYU_NUM = 36;
	
	private readonly bool[] mIsDragonArray = {
			false,false,false,false,false,false,false,false,false,false,
			false,false,false,false,false,false,false,false,false,false,
			false,false,false,false,false,false,false,false,false,false,
			true,true,true,		
	};

    public enum TartsuType
    {
        Head,
        Ansyun,
        Minsyun,
        Anko,
        Minko,
        Ankantsu,
        MinKantsu
    };
	
	public static readonly String[] YAKU_STRING = {
		"",
		"立直", 
		"門前清自摸和", 
		"一発",
		"平和", 
		"断么九", 
		"一盃口", 
		"役牌", 
		"河底撈魚", 
		"海底摸月", 
		"嶺上開花", 
		"槍槓", 
		"両立直", 
		"三色同順", 
		"一気通貫", 
		"三暗刻", 
		"混全帯么九", 
		"七対子",
		"対々和", 
		"小三元", 
		"混老頭", 
		"三色同刻",
		"三槓子", 
		"混一色", 
		"純全帯么九", 
		"二盃口", 
		"流し満貫", 
		"清一色", 
		"ドラ", 
		
		//YAKUMAN
		"四暗刻", 
		"国士無双", 
		"大三元", 
		"小四喜", 
		"大四喜",
		"字一色",
		"緑一色", 
		"清老頭", 
		"九蓮宝燈", 
		"四槓子", 
		"天和", 
		"地和", 
		"人和", 
		"十三不塔",
	};
	public static readonly bool[] isValyInSolo = {
		false,
		true, 
		true, 
		true,
		true, 
		true, 
		true, 
		true, 
		false,//"河底撈魚" 
		true, 
		true, 
		false,//"槍槓"
		true, 
		true, 
		true, 
		true, 
		true, 
		true,
		true, 
		true, 
		true, 
		true,
		true, 
		true, 
		true, 
		true, 
		false,//"流し満貫" 
		true, 
		true, 
		
		//YAKUMAN
		true, 
		true, 
		true, 
		true, 
		true,
		true,
		true, 
		true, 
		true, 
		true, 
		true, 
		false,//"地和"  
		false,//"人和"
		false,//"十三不塔"
	};
	public static readonly int[] YAKU_HAN_MENZEN = {
		0,//"",
		1,//"立直", 
		1,//"門前清自摸和", 
		1,//"一発",
		1,//"平和", 
		1,//"断么九", 
		1,//"一盃口", 
		0,//"役牌", 数が変動するので別に加算する 
		1,//"河底撈魚", 
		1,//"海底摸月", 
		1,//"嶺上開花", 
		1,//"槍槓", 
		2,//"両立直", 
		2,//"三色同順", 
		2,//"一気通貫", 
		2,//"三暗刻", 
		2,//"混全帯么九", 
		2,//"七対子",
		2,//"対々和", 
		2,//"小三元", 
		2,//"混老頭", 
		2,//"三色同刻",
		2,//"三槓子", 
		3,//"混一色", 
		3,//"純全帯么九", 
		3,//"二盃口", 
		5,//"流し満貫", 
		6,//"清一色", 
		0,//"ドラ", 数が変動するので別に加算する 
		
		//YAKUMAN
		13,//"四暗刻", 
		13,//"国士無双", 
		13,//"大三元", 
		13,//"小四喜", 
		13,//"大四喜",
		13,//"字一色",
		13,//"緑一色", 
		13,//"清老頭", 
		13,//"九蓮宝燈", 
		13,//"四槓子", 
		13,//"天和", 
		13,//"地和", 
		13,//"人和", 
		13,//"十三不塔",
	};
	public static readonly int[] YAKU_HAN_FUROED = {
		0,//"",
		0,//"立直", 
		0,//"門前清自摸和", 
		0,//"一発",
		0,//"平和", 
		1,//"断么九", 
		0,//"一盃口", 
		0,//"役牌",数が変動するので別に加算する 
		1,//"河底撈魚", 
		1,//"海底摸月", 
		1,//"嶺上開花", 
		1,//"槍槓", 

		0,//"両立直", 
		1,//"三色同順", 
		1,//"一気通貫", 
		2,//"三暗刻", 
		1,//"混全帯么九", 
		0,//"七対子",
		2,//"対々和", 
		2,//"小三元", 
		2,//"混老頭", 
		2,//"三色同刻",
		2,//"三槓子", 
		2,//"混一色", 
		2,//"純全帯么九", 
		0,//"二盃口", 
		0,//"流し満貫", 
		5,//"清一色", 
		0,//"ドラ",数が変動するので別に加算する 
		
		//YAKUMAN
		13,//"四暗刻", 
		13,//"国士無双", 
		13,//"大三元", 
		13,//"小四喜", 
		13,//"大四喜",
		13,//"字一色",
		13,//"緑一色", 
		13,//"清老頭", 
		13,//"九蓮宝燈", 
		13,//"四槓子", 
		13,//"天和", 
		13,//"地和", 
		13,//"人和", 
		13,//"十三不塔",
	};
	
	
	private MJUtil (){} 
	
	public static bool isRoto(int syu){
		if(syu >= 27){
			return false;
		}else if( (syu % 9 == 0)||(syu % 9 == 8) ){
			return true;
		}
		return false;
	}
	
	public static bool isYaochu(int syu){
		if(syu >= 27){
			return true;
		}else if( (syu % 9 == 0)||(syu % 9 == 8) ){
			return true;
		}
		return false;
	}
	public static bool isYaochu(Pai pai){
		return isYaochu(pai.PaiNumber);
	}
	public static bool isDragon(int syuIdx) {
		return (syuIdx == 31)||(syuIdx == 32)||(syuIdx == 33);
	}
	public static bool isJihai(int syuIdx) {
		return (27 <= syuIdx)&&(syuIdx < LENGTH_SYU);//0~26は数牌
	}
	public static bool isWind(int syuIdx) {
		return (27 <= syuIdx)&&(syuIdx <= 30);//27~30は風牌
	}
	public static bool isGreen(int syuIdx) {
		//緑一色を満たす牌をtrue
		return    (syuIdx == 19)//2s
				||(syuIdx == 20)//3s
				||(syuIdx == 21)//4s
				||(syuIdx == 23)//6s
				||(syuIdx == 25)//8s
				||(syuIdx == 32);//6z =hatsu
	}

	
}
}
