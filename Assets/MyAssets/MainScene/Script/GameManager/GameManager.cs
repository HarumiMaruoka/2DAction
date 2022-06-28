using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //このクラスはシングルトンパターンを使用したものである。

    //インスタンスを生成
    private static GameManager _instance;

    /// <summary> アイテムデータが入ったファイルのパス </summary>
    [SerializeField] string _itemCSVPath;
    /// <summary> アイテムデータベース </summary>
    Item[] _itemData = new Item[(int)Item.ItemID.ITEM_ID_END];

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

    private void Start()
    {
        //このスクリプトがアタッチされたオブジェクトは、シーンを跨いでもデストロイされないようにする。
        DontDestroyOnLoad(gameObject);

        //BGM管理用変数を初期化
        //_bgm = GetComponent<AudioSource>();
        //if (_bgm == null)
        //{
        //    Debug.LogError("ゲームマネージャーにAudioSource Componentをアタッチしてください!");
        //}
    }

    //あとでBGMManagerに書く
    /// <summary> BGMを切り替える </summary>
    //public void ChangeBGM(AudioClip newAudio)
    //{
    //    _bgm.clip = newAudio;
    //    _bgm.Play();
    //}

    /// <summary> アイテムデータをファイルから読み込む </summary>
    void LoadItemCSV()
    {
        int index = 0;
        bool isFirstLine = true;
        //CSVファイルからアイテムデータを読み込み、配列に保存する
        StreamReader sr = new StreamReader(@_itemCSVPath);//ファイルを開く
        while (!sr.EndOfStream)// 末尾まで繰り返す
        {
            string[] values = sr.ReadLine().Split(',');//一行読み込み区切って保存する
            if (isFirstLine)//最初の行(ヘッダーの行)はスキップする
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

            //個数を取得する


            index++;
        }
    }

}
