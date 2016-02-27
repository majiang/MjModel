using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MjModelProject.N_Controller
{
    class N_Controller
    {
        public N_Controller()
        {
            StartMessageThread();
        }




        //複数room対応アーキテクチャは？
        
        bool StartMessageThread()
        {
            //TODO
            //ネットワーク待ち受け開始
            //新クライアントが来たらヘローを返す
            


            //ハンドラでメッセージプロセッサに文字列を渡す
            
            return true;
        }
        
        
        bool ProcessMessage()
        {

            //TODO
            //メッセージプロセッサで受信メッセージ処理
            //ここでmjsonをパースする
            //ハンドラでモデルの処理関数呼び出す。
            return true;
        } 


        
        //ゲームモデル
        //ハンドラをここに記載
        bool OnJoin()
        {
            return true;
        }







        //メッセージ送信ハンドラ
        //メッセージオブジェクトをメッセージプロセッサに渡す
        //メッセージ文字列を送信する
        bool SendMessage()
        {
            //メッセージ送信スレッドにメッセージを送信
            return true;
        }

        

    }
}
