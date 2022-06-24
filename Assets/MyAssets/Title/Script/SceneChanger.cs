using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    [SerializeField] string _mainSceneName;
    [SerializeField] string _titleSceneName;

    void SceneChangeMain()
    {
        SceneManager.LoadScene(_mainSceneName);
    }

    //Mainシーンを読み込む
    public void LoadMainScene()
    {
        Invoke("SceneChangeMain", 1f);
    }

    public void LoadTitleScene()
    {
        SceneManager.LoadScene(_titleSceneName);
    }

    public void GameExit()
    {
        //UnityEditor上で終了するときの処理。(ビルド時はコメントアウトする。)
        //UnityEditor.EditorApplication.isPlaying = false;
        //実行ファイル中での終了処理
        UnityEngine.Application.Quit();
    }
}
