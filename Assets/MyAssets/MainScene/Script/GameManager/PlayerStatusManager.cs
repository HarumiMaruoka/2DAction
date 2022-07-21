using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

/// <summary> このクラスはプレイヤーのステータスを管理する。 </summary>
public class PlayerStatusManager : MonoBehaviour
{
    //このクラスはシングルトンパターンを使用したものである。
    //インスタンス
    private static PlayerStatusManager _instance;
    //インスタンスは読み取り専用
    public static PlayerStatusManager Instance
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
    //プライベートなコンストラクタを定義する。
    private PlayerStatusManager() { }

    /// <summary> プレイヤーの名前 </summary>
    [SerializeField] string _playerName;
    public string PlayerName { get => _playerName; }
    //各パラメータ
    /// <summary> プレイヤーの最大体力 </summary>
    [Header("プレイヤーの最大体力"), SerializeField] float _maxPlayerHealthPoint;
    /// <summary> プレイヤーの最大体力 </summary>
    public float PlayerMaxHealthPoint { get => _maxPlayerHealthPoint; set { Debug.Log("プレイヤーの最大体力の値を変更しました"); _maxPlayerHealthPoint = value; } }
    /// <summary> プレイヤーの体力 </summary>
    [Header("プレイヤーの体力"), SerializeField] float _playerHealthPoint;
    /// <summary> プレイヤーの体力 </summary>
    public float PlayerHealthPoint { get => _playerHealthPoint; set { Debug.Log("プレイヤーの体力の値を変更しました"); _playerHealthPoint = value; } }
    /// <summary> プレイヤーの最大スタミナ </summary>
    [Header("プレイヤーの最大スタミナ"), SerializeField] float _playerMaxStamina;
    /// <summary> プレイヤーの最大スタミナ </summary>
    public float PlayerMaxStamina { get => _playerMaxStamina; set => _playerMaxStamina = value; }
    /// <summary> プレイヤーのスタミナ </summary>
    [Header("プレイヤーのスタミナ"), SerializeField] float _playerStamina;
    /// <summary> プレイヤーのスタミナ </summary>
    public float PlayerStamina { get => _playerStamina; set => _playerStamina = value; }
    /// <summary> プレイヤーの近距離攻撃力 </summary>
    [Header("プレイヤーの近距離攻撃力"), SerializeField] float _playerShortRangeAttackPower;
    /// <summary> プレイヤーの近距離攻撃力 </summary>
    public float PlayerShortRangeAttackPower { get => _playerShortRangeAttackPower; set { Debug.Log("攻撃力の値を変更しました"); _playerShortRangeAttackPower = value; } }
    /// <summary> プレイヤーの遠距離攻撃力 </summary>
    [Header("プレイヤーの遠距離攻撃力"), SerializeField] float _playerLongRangeAttackPower;
    /// <summary> プレイヤーの遠距離攻撃力 </summary>
    public float PlayerLongRangeAttackPower { get => _playerLongRangeAttackPower; set { Debug.Log("攻撃力の値を変更しました"); _playerLongRangeAttackPower = value; } }
    /// <summary> プレイヤーの防御力 </summary>
    [Header("プレイヤーの防御力"), SerializeField] float _playerDefensePower;
    /// <summary> プレイヤーの防御力 </summary>
    public float PlayerDefensePower { get => _playerDefensePower; set { Debug.Log("防御力の値を変更しました"); _playerDefensePower = value; } }
    /// <summary> プレイヤーの移動力 </summary>
    [Header("プレイヤーの移動力"), SerializeField] float _playerMoveSpeed;
    /// <summary> プレイヤーの移動力 </summary>
    public float PlayerMoveSpeed { get => _playerMoveSpeed; set { Debug.Log("プレイヤーの移動力の値を変更しました"); _playerMoveSpeed = value; } }
    /// <summary> プレイヤーの吹っ飛びにくさ </summary>
    [Header("プレイヤーの吹っ飛びにくさ"), SerializeField] float _playerDifficultToBlowOff;
    /// <summary> プレイヤーの吹っ飛びにくさ </summary>
    public float PlayerDifficultToBlowOff { get => _playerDifficultToBlowOff; set { Debug.Log("プレイヤーの移動力の値を変更しました"); _playerDifficultToBlowOff = value; } }

    /// <summary> プレイヤーが向いている方向 </summary>
    private bool _isRight;
    /// <summary> プレイヤーが向いている方向 </summary>
    public bool IsRight { get => _isRight; }

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
        //このオブジェクトは、シーンを跨いでもデストロイしない。
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        Debug.Log("PlayerManagerの初期化に成功しました。");
    }

    //Updateではテストでアイテム所持数をセーブしたりロードしたりする処理を書いている。
    private void Update()
    {
        //プレイヤーが向いている方向を保存する
        if (Input.GetAxisRaw("Horizontal") < 0)
        {
            _isRight = true;
        }
        if (Input.GetAxisRaw("Horizontal") > 0)
        {
            _isRight = false;
        }
    }
}
