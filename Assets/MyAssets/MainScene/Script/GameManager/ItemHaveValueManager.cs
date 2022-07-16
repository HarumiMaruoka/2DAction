using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary> アイテム所持数を管理するクラス。 </summary>
public class ItemHaveValueManager : MonoBehaviour
{
    private static ItemHaveValueManager _instance;
    //インスタンスは読み取り専用
    public static ItemHaveValueManager Instance
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
    private ItemHaveValueManager() { }

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

        //配列分の領域を確保する。
        _itemVolume._itemNumberOfPossessions = new int[(int)Item.ItemID.ITEM_ID_END];
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

    void Start()
    {
        // アイテム所持数を、保存しているファイルのパスを取得しファイルを開く。
        _itemFilePath = Path.Combine(Application.persistentDataPath, "ItemNumberOfPossessionsFile.json");
        // アイテム所持数を、ファイルから取得する。
        OnLoad_ItemNumberOfPossessions(_itemFilePath);
        Debug.Log("PlayerManagerの初期化に成功しました。");
    }

    void Update()
    {
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

    /// <summary>　特定のアイテムの所持数を変更する。 </summary>
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

    /// <summary> アイテムの所持数をjsonファイルからロードする。 </summary>
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

    /// <summary> アイテムの所持数をjsonファイルにセーブする。 </summary>
    void OnSave_ItemNumberOfPossessions(string filePath)
    {
        Debug.Log("アイテム所持数をセーブします！");
        // アイテム所持数データを、JSON形式にシリアライズし、ファイルに保存
        File.WriteAllText(filePath, JsonUtility.ToJson(_itemVolume, false));
    }
}
