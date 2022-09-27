using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ゲームの終了処理
public class ExitMenu : MonoBehaviour
{
    //ゲームを終了する
    public void GameExit()
    {
        //エディター上で終了する場合の処理(ビルド時にはエラーを投げるので消す)
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
#if UNITY_EDITOR_WIN
        //ビルドしたゲームを終了する場合の処理(エディター実行中はコメントアウトする)
        Application.Quit();
#endif
    }
}
