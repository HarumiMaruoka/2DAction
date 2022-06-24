using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //このクラスはシングルトンパターンを使用したものである。

    //インスタンスを生成
    private static GameManager _instance;

    //インスタンスを取得
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameManager();
            }
            return _instance;
        }
    }

    private void Start()
    {
        if (_audioSource == null)
        {
            _audioSource = GetComponent<AudioSource>();
            if (_audioSource == null)
            {
                Debug.LogError("ゲームマネージャーにAudioSource Componentをアタッチしてください!");
            }
        }
    }

    AudioSource _audioSource;

    /// <summary> BGMを切り替える </summary>
    void ChangeBGM()
    {
        //そのシーンのBGMを取得
        AudioSource bgm = GameObject.Find("BGMManager").GetComponent<AudioSource>();
        if (bgm == null)
        {
            Debug.LogError("このエリアには、BGMが設定されていません!");
        }
    }
}
