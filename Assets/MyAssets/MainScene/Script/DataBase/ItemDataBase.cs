
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// 全てのアイテムの情報と、
/// プレイヤーが所持しているアイテムを管理するクラス。
/// </summary>
public class ItemDataBase : MonoBehaviour
{
    //<===== このクラスで使用する型 =====>//
    /// <summary> アイテムの所持数を管理する構造体 </summary>
    [System.Serializable]
    public struct ItemNumberOfPossessions
    {
        /// <summary> インデックスはIDで所持数が取得できる。 </summary>
        public int[] _itemNumberOfPossessions;
    }

    //<===== シングルトン関係 =====>//
    /// <summary> GameManagerのインスタンス </summary>
    private static ItemDataBase _instance;
    /// <summary> GameManagerのインスタンス </summary>
    public static ItemDataBase Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new ItemDataBase();
            }
            return _instance;
        }
    }
    //プライベートなコンストラクタ
    private ItemDataBase() { }

    //<===== メンバー変数 =====>//
    /// <summary> アイテムデータが入ったcsvファイルの絶対パス </summary>
    [Header("アイテムデータが入ったcsvファイルへの絶対パス"), SerializeField]
    string _itemCSVPath;
    /// <summary> アイテム所持数を保存しているjsonファイルへのパス </summary>
    [Header("確認用 : アイテム所持数を保存しているjsonファイルへのパス"), SerializeField]
    string _itemJsonPath;
    [Header("ItemData csv"), SerializeField]
    TextAsset _itemDataCsv = default;

    /// <summary> 全てのアイテムの情報を格納している変数 </summary>
    Item[] _itemData = new Item[(int)Item.ItemID.ITEM_ID_END];
    /// <summary> 全てのアイテムの情報を格納している変数 </summary>
    public Item[] ItemData { get => _itemData; }

    /// <summary> アイテム所持数を格納している変数 </summary>
    [Header("確認用 : アイテム所持数"), SerializeField] ItemNumberOfPossessions _itemVolume;
    /// <summary> アイテム所持数を格納している変数 </summary>
    public ItemNumberOfPossessions ItemVolume { get => _itemVolume; }

    bool _isInitialized = false;

    //<===== Unityメッセージ =====>//
    void Awake()
    {
        if (_isInitialized = Initialized())
        {
            Debug.Log($"初期化に成功しました。{gameObject.name}");
        }
        else
        {
            Debug.Log($"初期化に失敗しました。{gameObject.name}");
        }
    }
    void Start()
    {

    }
    void Update()
    {

    }

    //<===== publicメンバー関数 =====>//
    /// <summary>　特定のアイテムの所持数を変更する。 </summary>
    /// <param name="itemID"> 変更するアイテムのID </param>
    /// <param name="volume"> 変更する数 : <br/>
    /// 1ならそのIDのアイテムを一つ増やす <br/>
    /// -1ならそのIDのアイテムを一つ減らす。<br/>
    /// </param>
    public void MakeChanges_ItemNumberOfPossessions(int itemID, int volume)
    {
        //インデックスが正しいか判定
        if (itemID < (int)Item.ItemID.ITEM_ID_00 || itemID >= (int)Item.ItemID.ITEM_ID_END)
        {
            //インデックスが範囲外なら処理を抜ける
            Debug.LogError("無効なアイテムIDです。");
            return;
        }
        ItemVolume._itemNumberOfPossessions[itemID] += volume;
    }
    //<===== privateメンバー関数 =====>//
    /// <summary> 初期化関数 </summary>
    /// <returns> 初期化に成功したらtrueを返す。 </returns>
    bool Initialized()
    {
        /*** シングルトン関係の処理 ***/
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

        // アイテム所持数を保存しているファイルへのパスを取得し変数に保存。
        _itemJsonPath = Path.Combine(Application.persistentDataPath, "ItemNumberOfPossessionsFile.json");

        /*** ロード処理 ***/
        OnLoad_ItemCSV();
        OnLoad_ItemJson(_itemJsonPath);

        return true;
    }
    /// <summary> csvファイルから全てのアイテムの情報を取得し変数に格納する。 </summary>
    void OnLoad_ItemCSV()
    {
        //ファイルをロードするのに必要な初期化
        int index = 0;//インデックスの初期化
        bool isFirstLine = true;//一行目かどうかを判断するBooleanの初期化

        //CSVファイルからアイテムデータを読み込み、配列に保存する
        //StreamReader sr = new StreamReader(@_itemCSVPath);//ファイルを開く
        try
        {
            // ファイルを開く
            var sr = new StringReader(_itemDataCsv.text);
            // ファイルから読み込み、フィールドに保存する。
            string value = "";
            while ((value = sr.ReadLine()) != null)// 末尾まで繰り返す
            {
                //最初の行(ヘッダーの行)はスキップする
                if (isFirstLine)
                {
                    isFirstLine = false;
                    continue;
                }

                string[] values = value.Split(',');//一行読み込み区切って保存する

                //種類別で生成し保存する
                switch (values[2])
                {
                    case "HealItem": _itemData[index] = new HealItem((Item.ItemID)int.Parse(values[0]), values[1], Item.ItemType.HEAL, int.Parse(values[3]), values[4]); break;
                    case "PowerUpItem": _itemData[index] = new PowerUpItem((Item.ItemID)int.Parse(values[0]), values[1], Item.ItemType.POWER_UP, int.Parse(values[3]), values[4]); break;
                    case "MinusItem": _itemData[index] = new MinusItem((Item.ItemID)int.Parse(values[0]), values[1], Item.ItemType.MINUS_ITEM, int.Parse(values[3]), values[4]); break;
                    case "KeyItem": _itemData[index] = new KeyItem((Item.ItemID)int.Parse(values[0]), values[1], Item.ItemType.KEY, int.Parse(values[3]), values[4]); break;
                    default: Debug.LogError("設定されていないItemTypeです。"); break;
                }
                index++;
            }
        }
        catch(FileNotFoundException e)
        {
            Debug.LogError($"エラー！修正してください!\n{e.Message}");
        }
    }

    /// <summary> アイテムの所持数をjsonファイルからロードする。 </summary>
    void OnLoad_ItemJson(string filePath)
    {
        Debug.Log("アイテム所持数をロードします！");
        // 念のためファイルの存在チェック
        if (!File.Exists(filePath))
        {
            //ここにファイルが無い場合の処理を書く
            Debug.LogError("アイテム所持数を保存しているファイルが見つかりません。");

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
