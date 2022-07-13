using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponManage : MonoBehaviour
{
    //このクラスはシングルトンパターンを使用したものである。
    //インスタンスを生成
    private static PlayerWeaponManage _instance;
    //インスタンスをは読み取り専用
    public static PlayerWeaponManage Instance
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
    private PlayerWeaponManage() { }

    //弾に関してはオブジェクトプール？を使用する
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
    }

    void Start()
    {
        _nomalShotBullet = new GameObject[_maxBullet];
    }

    void Update()
    {
        
    }
}
