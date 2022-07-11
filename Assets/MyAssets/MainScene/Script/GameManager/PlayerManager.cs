using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

//このクラスでは今のところ、アイテムの所持数と装備数を管理する
public class PlayerManager : MonoBehaviour
{
    //このクラスはシングルトンパターンを使用したものである。
    //インスタンスを生成
    private static PlayerManager _instance;
    //インスタンスをは読み取り専用
    public static PlayerManager Instance
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

    //アイテム所持数の保存先
    [SerializeField] string _itemFilePath;

    /// <summary> アイテムの所持数 </summary>
    [System.Serializable]
    public struct ItemNumberOfPossessions
    {
        public int[] _itemNumberOfPossessions;
    }
    [SerializeField] ItemNumberOfPossessions _itemVolume;
    public ItemNumberOfPossessions ItemVolume { get => _itemVolume; }


    /// <summary> 所持している装備の情報 </summary>
    /// 装備に関する処理をここに書く

    /// <summary> 所持している主要なアイテムの情報 </summary>
    /// 所持している主要なアイテムの情報に関する処理をここに書く


    //各パラメータ
    [Header("プレイヤーの最大体力"), SerializeField] public float _maxPlayerHealthPoint;
    public float MaxPlayerHealthPoint { get => _maxPlayerHealthPoint; }

    [Header("プレイヤーの体力"), SerializeField] float _playerHealthPoint;
    public float PlayerHealthPoint { get => _playerHealthPoint; set { Debug.Log("プレイヤーの体力の値を変更しました"); _playerHealthPoint = value; } }

    [Header("プレイヤーの最大スタミナ"), SerializeField] float _playerMaxStamina;
    public float PlayerMaxStamina { get => _playerMaxStamina; set => _playerMaxStamina = value; }

    [Header("プレイヤーのスタミナ"), SerializeField] float _playerStamina;
    public float PlayerStamina { get => _playerStamina; set => _playerStamina = value; }

    [Header("プレイヤーの近距離攻撃力"), SerializeField] float _playerOffensivePower_ShortDistance;
    public float PlayerOffensivePower_ShortDistance { get => _playerOffensivePower_ShortDistance; set { Debug.Log("攻撃力の値を変更しました"); _playerOffensivePower_ShortDistance = value; } }

    [Header("プレイヤーの遠距離攻撃力"), SerializeField] float _playerOffensivePower_LongDistance;
    public float PlayerOffensivePower_LongDistance { get => _playerOffensivePower_LongDistance; set { Debug.Log("攻撃力の値を変更しました"); _playerOffensivePower_LongDistance = value; } }

    [Header("プレイヤーの防御力"), SerializeField] float _playerDefensePower;
    public float PlayerDefensePower { get => _playerDefensePower; set { Debug.Log("防御力の値を変更しました"); _playerDefensePower = value; } }

    [Header("プレイヤーの移動力"), SerializeField] float _playerMoveSpeed;
    public float PlayerMoveSpeed { get => _playerMoveSpeed; set { Debug.Log("プレイヤーの移動力の値を変更しました"); _playerMoveSpeed = value; } }


    /// <summary> プレイヤーが向いている方向 </summary>
    private bool _isRight;
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

        //配列分の領域を確保する。
        _itemVolume._itemNumberOfPossessions = new int[(int)Item.ItemID.ITEM_ID_END];
    }

    void Start()
    {
        //このオブジェクトは、シーンを跨いでもデストロイしない。
        DontDestroyOnLoad(gameObject);
        // アイテム所持数を、保存しているファイルのパスを取得しファイルを開く。
        _itemFilePath = Path.Combine(Application.persistentDataPath, "ItemNumberOfPossessionsFile.json");
        // アイテム所持数を、ファイルから取得する。
        OnLoad_ItemNumberOfPossessions(_itemFilePath);
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

        //Kキー押下でアイテム所持数をセーブする
        if (Input.GetKeyDown(KeyCode.K))
        {
            OnSave_ItemNumberOfPossessions(_itemFilePath);
        }
        //Lキー押下でアイテム所持数をロードする
        if (Input.GetKeyDown(KeyCode.L))
        {
            OnLoad_ItemNumberOfPossessions(_itemFilePath);
        }
    }

    /// <summary>　特定のアイテムの情報を変更 </summary>
    /// <param name="itemID"> 変更するアイテムのID </param>
    /// <param name="value"> 変更する数 </param>
    public void Set_ItemNumberOfPossessions(int itemID, int volume)
    {
        //インデックスが正しいか判定
        if (itemID < (int)Item.ItemID.ITEM_ID_00 || itemID >= (int)Item.ItemID.ITEM_ID_END)
        {
            //インデックスが場外なら処理を抜ける
            Debug.LogError("無効なアイテムIDです。");
            return;
        }
        ItemVolume._itemNumberOfPossessions[itemID] += volume;
    }

    /// <summary> アイテムの所持数をファイルからロード </summary>
    void OnLoad_ItemNumberOfPossessions(string filePath)
    {
        Debug.Log("アイテム所持数をロードします！");
        // 念のためファイルの存在チェック
        if (!File.Exists(filePath))
        {
            //ここにファイルが無い場合の処理を書く
            Debug.Log("アイテム所持数を保存しているファイルが見つかりません。");

            //処理を抜ける
            return;
        }
        // JSONオブジェクトを、デシリアライズ(C#形式に変換)し、値をセット。
        _itemVolume = JsonUtility.FromJson<ItemNumberOfPossessions>(File.ReadAllText(filePath));
    }

    /// <summary> アイテムの所持数をファイルにセーブ </summary>
    void OnSave_ItemNumberOfPossessions(string filePath)
    {
        Debug.Log("アイテム所持数をセーブします！");
        // アイテム所持数データを、JSON形式にシリアライズし、ファイルに保存
        File.WriteAllText(filePath, JsonUtility.ToJson(_itemVolume, false));
    }
}
