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
        ITEM_ID_08,
        ITEM_ID_09,
        ITEM_ID_10,

        ITEM_ID_11,
        ITEM_ID_12,
        ITEM_ID_13,
        ITEM_ID_14,
        ITEM_ID_15,
        ITEM_ID_16,
        ITEM_ID_17,
        ITEM_ID_18,
        ITEM_ID_19,
        ITEM_ID_20,

        ITEM_ID_21,
        ITEM_ID_22,
        ITEM_ID_23,
        ITEM_ID_24,
        ITEM_ID_25,
        ITEM_ID_26,
        ITEM_ID_27,
        ITEM_ID_28,
        ITEM_ID_29,
        ITEM_ID_30,

        ITEM_ID_END
    }

    public enum ItemType
    {
        HEAL = 1,
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
    public virtual void UseItem()
    {
        Debug.Log($"アイテム共通の処理 : アイテムID.{_myID}を使用。");
        ItemDataBase.Instance.MakeChanges_ItemNumberOfPossessions((int)_myID, -1);
    }
}
