using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{

    //このクラスはシングルトンパターンを使用したものである。
    //インスタンスを生成
    private static GameManager _instance;
    //インスタンスを読み取り専用かつインスタンスがなければインスタンスを生成し保存する
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

    //Item情報の格納先
    struct ItemManager
    {
        /// <summary> アイテム情報の格納先 </summary>
        public static Item[] _itemDefinition;
        /// <summary> アイテムの所持数 </summary>
        public static int[] _itemHaveVolume;
    }
    //アイテム管理用変数
    ItemManager[] _item = new ItemManager[(int)Item.ItemID.ITEM_ID_END];

    //装備情報の格納先

    void Start()
    {

    }

    void SetItem()
    {
        //アイテム情報をスプレッドシートから読み込み、配列に保存する
    }
}
