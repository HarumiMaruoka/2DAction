using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    //プライベートなコンストラクタを定義する
    private GameManager() { }

    /// <summary> アイテムデータが入ったファイルのパス </summary>
    [SerializeField] string _itemCSVPath;
    [SerializeField] string _itemJSONPath;
    /// <summary> アイテムデータベース </summary>
    Item[] _itemData = new Item[(int)Item.ItemID.ITEM_ID_END];
    public Item[] ItemData { get => _itemData; }

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
        //このスクリプトがアタッチされたオブジェクトは、シーンを跨いでもデストロイされないようにする。
        DontDestroyOnLoad(gameObject);
        LoadItemCSV();
    }

    private void Start()
    {

    }

    private void Update()
    {

    }

    /// <summary> アイテムデータをファイルから読み込む </summary>
    void LoadItemCSV()
    {
        //ファイルをロードするのに必要な初期化
        int index = 0;//インデックスの初期化
        bool isFirstLine = true;//一行目かどうかを判断するBooleanの初期化

        //CSVファイルからアイテムデータを読み込み、配列に保存する
        StreamReader sr = new StreamReader(@_itemCSVPath);//ファイルを開く
        while (!sr.EndOfStream)// 末尾まで繰り返す
        {
            string[] values = sr.ReadLine().Split(',');//一行読み込み区切って保存する

            //最初の行(ヘッダーの行)はスキップする
            if (isFirstLine)
            {
                isFirstLine = false;
                continue;
            }

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
}
