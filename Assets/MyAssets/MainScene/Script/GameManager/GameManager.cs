using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //このクラスはシングルトンパターンを使用したものである。

    //インスタンスを生成
    private static GameManager _instance;

    //インスタンスをカプセル化
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
        //このスクリプトがアタッチされたオブジェクトは、シーンを跨いでもデストロイされないようにする。
        DontDestroyOnLoad(gameObject);

        //BGM管理用変数を初期化
        //_bgm = GetComponent<AudioSource>();
        //if (_bgm == null)
        //{
        //    Debug.LogError("ゲームマネージャーにAudioSource Componentをアタッチしてください!");
        //}
    }


    //あとでBGMManagerに書く
    /// <summary> BGMを切り替える </summary>
    //public void ChangeBGM(AudioClip newAudio)
    //{
    //    _bgm.clip = newAudio;
    //    _bgm.Play();
    //}
}
