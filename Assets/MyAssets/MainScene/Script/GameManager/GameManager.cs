using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //<===== シングルトン関係 =====>//
    private static GameManager _instance;
    /// <summary> GameManagerクラスの唯一のインスタンス </summary>
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
    //プライベートなコンストラクタ
    private GameManager() { }

    /// <summary> 初期化に成功したかどうかを表す変数。 </summary>
    bool _isInitialized = false;

    void Awake()
    {
        if( _isInitialized = Initialized())
        {
            Debug.Log($"初期化に成功しました。 : オブジェクト名{gameObject.name}");
        }
        else
        {
            Debug.LogError($"初期化に失敗しました。 : オブジェクト名{gameObject.name}");
        }
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
        //もしインスタンスが設定されていなかったら自身を代入する
        if (_instance == null)
        {
            _instance = this;
        }
        //もう既に存在する場合は、このオブジェクトを破棄する。
        else if (_instance != null)
        {
            Destroy(this);
        }
        //このスクリプトがアタッチされたオブジェクトは、シーンを跨いでもデストロイされないようにする。
        DontDestroyOnLoad(gameObject);

        return true;
    }
}
