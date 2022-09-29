using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

/// <summary>
/// ゲーム全体で使用可能なクラス。未だに何も記述していないので必要ないかもしれない。
/// </summary>
public class GameManager : MonoBehaviour
{
    //===== フィールド / プロパティ =====//

    /// <summary> 初期化に成功したかどうかを表す変数。 </summary>
    bool _isInitialized = false;

    [Header("フェードイン / アウト関連")]
    [Tooltip("フェードイン/アウト用のパネル"), SerializeField]
    GameObject _fadePanel;
    [Tooltip("フェードインに掛ける時間"), SerializeField]
    float _timeToFadeIn;

    // ポーズ開始時に実行するデリゲート
    static public System.Action OnPause = default;
    // ポーズ解除時に実行するデリゲート
    static public System.Action OnResume = default;

    bool _isPause = false;


    //===== Unityメッセージ =====//
    void Awake()
    {

    }
    void Start()
    {

    }
    void Update()
    {

    }

    //===== priveteメソッド =====//
    /// <summary> 初期化関数 </summary>
    /// <returns> 初期化に成功したら true を返す。 </returns>
    bool Initialized()
    {

        return true;
    }

    //===== publicメソッド =====//
    // フェードインする
    public void FadeIn(string nextSceneName)
    {
        //フェードイン用パネルをアクティブにする。
        _fadePanel.SetActive(true);
        //DOTweenを利用してフェードインする。
        _fadePanel.
            GetComponent<Image>().
            DOFade(1, _timeToFadeIn).//_timeToFadeIn秒掛けて、パネルのアルファ値を1にする。
            SetDelay(0.5f).          //0.5秒待って実行する。
            OnComplete               //フェードインが完了したら、シーンを読み込む。
            (
                () => SceneManager.LoadScene(nextSceneName)
            );
    }
    // フェードアウトする
    public void FadeOut()
    {

    }

    //===== ボタンから呼び出す想定で作成したメソッド =====//
    /// <summary>
    /// ポーズ開始処理
    /// </summary>
    public void OnPauseStart()
    {
        if (!_isPause)
        {
            _isPause = true;
            OnPause();
        }
    }
    /// <summary>
    /// ポーズ終了処理
    /// </summary>
    public void OnPauseEnd()
    {
        if (_isPause)
        {
            _isPause = false;
            OnResume();
        }
    }
    /// <summary>
    /// シーンを変更する。
    /// </summary>
    /// <param name="nextSceneName"> 読み込むシーン名 </param>
    public void SceneChange(string nextSceneName)
    {
        SceneManager.LoadScene(nextSceneName);
    }
}
