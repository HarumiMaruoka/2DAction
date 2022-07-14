using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPartsManager : PlayerPartsMavagerBase
{
    //=======シングルトン関係=======//
    //インスタンスを生成
    private static PlayerPartsManager _instance;
    //インスタンスをは読み取り専用
    public static PlayerPartsManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("PlayerManager._instanceはnullです。");
            }
            return _instance;
        }
    }
    //プライベートなコンストラクタを定義する
    private PlayerPartsManager() { }

    [Header("所持できるパーツの最大数"), SerializeField] int _maxHaveValue;
    /// <summary> 所持できるパーツの最大数 </summary>
    public int MaxHaveValue { get => _maxHaveValue; }
    /// <summary> 現在所持しているパーツの配列 </summary>
    Equipment[] _havePartsList;

    //メモ : 弾に関してはオブジェクトプールを使用する。
    //弾の格納先
    [Header("弾の最大数"), SerializeField] int _maxBullet;
    GameObject[] _nomalShotBullet;



    private void Awake()
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
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        _nomalShotBullet = new GameObject[_maxBullet];
    }

    void Update()
    {
        
    }
}
