using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

/// <summary> このクラスはプレイヤーのステータスを管理する。 </summary>
public class PlayerStatusManager : MonoBehaviour
{
    //<======== このクラスで使用する型 ========>//
    [System.Serializable]
    public struct PlayerStatus
    {
        /// <summary> 名前 </summary>
        public string _playerName;
        /// <summary> 最大体力 </summary>
        public float _maxHp;
        /// <summary> 最大スタミナ </summary>
        public float _maxStamina;
        /// <summary> 近距離攻撃力 </summary>
        public float _shortRangeAttackPower;
        /// <summary> 遠距離攻撃力 </summary>
        public float _longRangeAttackPower;
        /// <summary> 防御力 </summary>
        public float _defensePower;
        /// <summary> 移動速度 </summary>
        public float _moveSpeed;
        /// <summary> 吹っ飛びにくさ </summary>
        public float _difficultToBlowOff;


        static public PlayerStatus Zero = new PlayerStatus();

        //コンストラクタ
        public PlayerStatus(string name = "", float hp = 0f, float stamina = 0f, float shortAttackPow = 0f, float longAttackPow = 0f, float defensePow = 0f, float moveSpeed = 0f, float difficultToBlowOff = 0f)
        {
            _playerName = name;
            _maxHp = hp;
            _maxStamina = stamina;
            _shortRangeAttackPower = shortAttackPow;
            _longRangeAttackPower = longAttackPow;
            _defensePower = defensePow;
            _moveSpeed = moveSpeed;
            _difficultToBlowOff = difficultToBlowOff;
        }
        public static PlayerStatus _zero
        {
            get => new PlayerStatus();
        }

        /// <summary> PlayerStatus型 : +演算子のオーバーロード </summary>
        public static PlayerStatus operator +(PlayerStatus p1, PlayerStatus p2)
        {
            p1._maxHp += p2._maxHp;
            p1._maxStamina += p2._maxStamina;
            p1._shortRangeAttackPower += p2._shortRangeAttackPower;
            p1._longRangeAttackPower += p2._longRangeAttackPower;
            p1._defensePower += p2._defensePower;
            p1._moveSpeed += p2._moveSpeed;
            p1._difficultToBlowOff += p2._difficultToBlowOff;
            return p1;
        }
        /// <summary> PlayerStatus型 : -演算子のオーバーロード </summary>
        public static PlayerStatus operator -(PlayerStatus p1, PlayerStatus p2)
        {
            p1._maxHp -= p2._maxHp;
            p1._maxStamina -= p2._maxStamina;
            p1._shortRangeAttackPower -= p2._shortRangeAttackPower;
            p1._longRangeAttackPower -= p2._longRangeAttackPower;
            p1._defensePower -= p2._defensePower;
            p1._moveSpeed -= p2._moveSpeed;
            p1._difficultToBlowOff -= p2._difficultToBlowOff;
            return p1;
        }
    }
    //<===== シングルトンパターン関連 =====>//
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

    // <=========== メンバー変数 ===========> //
    /// <summary> 基礎ステータス </summary>
    [Header("プレイヤーの基礎ステータス"), SerializeField] PlayerStatus _baseStatus;
    /// <summary> 基礎ステータス </summary>
    public PlayerStatus BaseStatus { get => _baseStatus; set => _baseStatus = value; }
    /// <summary> 装備分の上昇値 </summary>
    [Header("確認用 : 装備分の上昇値"), SerializeField] PlayerStatus _equipment_RisingValue;
    /// <summary> 装備分の上昇値 </summary>
    public PlayerStatus Equipment_RisingValue { get => _equipment_RisingValue; set => _equipment_RisingValue = value; }
    /// <summary> レベル分の上昇値 </summary>
    [Header("確認用 : レベル分の上昇値"), SerializeField] PlayerStatus _level_RisingValue;
    /// <summary> レベル分の上昇値 </summary>
    public PlayerStatus Level_RisingValue { get=> _level_RisingValue; set=> _level_RisingValue=value; }
    /// <summary> その他(アイテム使用時の一時的な上昇値等)の上昇値 </summary>
    [Header("その他(アイテム使用時の一時的な上昇値等)の上昇値"), SerializeField] PlayerStatus _other_RisingValue;
    /// <summary> その他(アイテム使用時の一時的な上昇値等)の上昇値 </summary>
    public PlayerStatus Other_RisingValue { get=> _other_RisingValue; set=> _other_RisingValue=value; }
    /// <summary> 諸々を合計した、最終的なステータス </summary>
    public PlayerStatus ConsequentialPlayerStatus { get => _baseStatus + Equipment_RisingValue + Level_RisingValue + Other_RisingValue; }

    [Header("プレイヤーの現在の体力"), SerializeField] float _playerHealthPoint;
    /// <summary> プレイヤーの現在の体力 </summary>
    public float PlayerHealthPoint { get => _playerHealthPoint; set => _playerHealthPoint = value; }

    [Header("プレイヤーの現在のスタミナ"), SerializeField] float _playerStamina;
    /// <summary> プレイヤーの現在のスタミナ </summary>
    public float PlayerStamina { get => _playerStamina; set => _playerStamina = value; }

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
