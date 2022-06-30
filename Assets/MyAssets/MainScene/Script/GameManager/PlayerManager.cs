using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] string _itemFilePath;

    //このクラスはシングルトンパターンを使用したものである。
    //インスタンスを生成
    private static PlayerManager _instance;
    //インスタンスを読み取り専用かつインスタンスがなければインスタンスを生成し保存する
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

    /// <summary> アイテムの所持数 </summary>
    [System.Serializable]
    public struct ItemNumberOfPossessions
    {
        public int[] _itemNumberOfPossessions;
    }
    [SerializeField] ItemNumberOfPossessions _itemVolume;

    public ItemNumberOfPossessions ItemVolume
    {
        get
        {
            return _itemVolume;
        }
    }
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
        _itemVolume._itemNumberOfPossessions = new int[(int)Item.ItemID.ITEM_ID_END];
    }

    void Start()
    {
        //このオブジェクトは、シーンを跨いでもデストロイしない。
        DontDestroyOnLoad(gameObject);
        // アイテム所持数を、保存しているファイルのパスを取得。
        _itemFilePath = Path.Combine(Application.persistentDataPath, "ItemNumberOfPossessionsFile.json");
        OnLoad_ItemNumberOfPossessions(_itemFilePath);
        Debug.Log("初期化成功");
    }

    private void Update()
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

    /// <summary>　特定のアイテムの情報を変更 </summary>
    /// <param name="itemID"> 変更するアイテムのID </param>
    /// <param name="value"> 変更する数 </param>
    public void Set_ItemNumberOfPossessions(int itemID, int volume)
    {
        //インデックスが正しいか判定
        if (itemID <= 0 || itemID >= (int)Item.ItemID.ITEM_ID_END)
        {
            //インデックスが場外なら処理を抜ける
            Debug.LogError("無効なアイテムIDです。");
            return;
        }
        ItemVolume._itemNumberOfPossessions[itemID] += volume;
    }

    //装備情報の格納先

    /// <summary> アイテムの所持数をファイルからロード </summary>
    void OnLoad_ItemNumberOfPossessions(string filePath)
    {
        Debug.Log("アイテム所持数をロードします！");
        // 念のためファイルの存在チェック
        if (!File.Exists(filePath))
        {
            //ここにファイルが無い場合の処理を書く
            Debug.Log("アイテム所持数を保存するファイルが見つかりません。");

            //処理を抜ける
            return;
        }

        // JSONオブジェクトを、デシリアライズ(C#形式に変換)し、値をセット。
        _itemVolume = JsonUtility.FromJson<ItemNumberOfPossessions>(File.ReadAllText(filePath));
    }

    void OnSave_ItemNumberOfPossessions(string filePath)
    {
        Debug.Log("アイテム所持数をセーブします！");
        // アイテム所持数データを、JSON形式にシリアライズし、ファイルに保存
        File.WriteAllText(filePath, JsonUtility.ToJson(_itemVolume, false));
    }
}
