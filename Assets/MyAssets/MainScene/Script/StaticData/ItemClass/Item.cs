using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Item
{
    //アイテムの基底クラス:各アイテムはこのクラスを継承する
    public enum ItemID
    {
        ITEM_ID_01,

        ITEM_ID_END
    }

    public enum ItemType
    {
        KEY,
        POWER_UP,
        RECOVERY,
        MINUS_ITEM,

        ITEM_TYPE_END
    }

    ItemID _myID;
    ItemType _myType;

    /// <summary> アイテムを使用する:継承先で内容を定義する。 </summary>
    virtual public void UseItem() { }
}
