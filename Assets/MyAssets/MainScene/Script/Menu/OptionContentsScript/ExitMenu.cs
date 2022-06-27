using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitMenu : MonoBehaviour
{
    //ゲームを終了する
    public void GameExit()
    {
        //エディター上で終了する場合の処理(ビルド時には消す)
        UnityEditor.EditorApplication.isPlaying = false;
        //ビルドしたゲームを終了する場合の処理(エディター実行中はコメントアウトする)
        //Application.Quit();
    }

    //ゲームを終了しない
    public void CancelExit()
    {
        //メニュー画面に戻る
        gameObject.SetActive(false);
    }
}
