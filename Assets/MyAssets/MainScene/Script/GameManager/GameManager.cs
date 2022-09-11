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
    /// <summary> 初期化に成功したかどうかを表す変数。 </summary>
    bool _isInitialized = false;

    [Header("フェードイン/アウト用のパネル"), SerializeField] GameObject _fadePanel;

    public void FadeIn(string nextSceneName)
    {
        //フェードイン用パネルをアクティブにする。
        _fadePanel.SetActive(true);
        //DOTweenを利用してフェードインする。
        _fadePanel.
            GetComponent<Image>().DOFade(1, 1.5f).
            SetDelay(0.5f).
            OnComplete(() => SceneManager.LoadScene(nextSceneName));
    }
    public void FadeOut()
    {

    }

    //<===== Unityメッセージ =====>//
    void Awake()
    {

    }
    void Start()
    {

    }
    void Update()
    {

    }

    //<===== priveteメンバー関数 =====>//
    /// <summary> 初期化関数 </summary>
    /// <returns> 初期化に成功したら true を返す。 </returns>
    bool Initialized()
    {

        return true;
    }
}
