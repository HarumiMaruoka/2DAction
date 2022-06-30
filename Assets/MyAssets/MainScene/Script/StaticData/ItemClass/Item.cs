using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Item
{
    //コンストラクタ
    public Item(ItemID id, string name, ItemType type, int effectSize, string explanatoryText)
    {
        _myID = id;
        _name = name;
        _myType = type;
        _myEffectSize = effectSize;
        _myExplanatoryText = explanatoryText;
    }
    //アイテムの基底クラス:各アイテムはこのクラスを継承する
    public enum ItemID
    {
        ITEM_ID_00,
        ITEM_ID_01,
        ITEM_ID_02,
        ITEM_ID_03,
        ITEM_ID_04,
        ITEM_ID_05,
        ITEM_ID_06,
        ITEM_ID_07,

        ITEM_ID_END
    }

    public enum ItemType
    {
        HEAL,
        POWER_UP,
        MINUS_ITEM,
        KEY,

        ITEM_TYPE_END
    }

    /// <summary> このアイテムのID </summary>
    public ItemID _myID { get; }
    /// <summary> このアイテムの名前 </summary>
    public string _name { get; }
    /// <summary> このアイテムの種類 </summary>
    public ItemType _myType { get; }
    /// <summary> このアイテムの効果量 </summary>
    public int _myEffectSize { get; }
    /// <summary> このアイテムの説明文 </summary>
    public string _myExplanatoryText { get; }

    /// <summary> アイテムを使用する:継承先で内容を定義する。 </summary>
    virtual public void UseItem() { }
}
