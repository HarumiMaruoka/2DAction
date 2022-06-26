using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Item
{
    //�A�C�e���̊��N���X:�e�A�C�e���͂��̃N���X���p������
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

    /// <summary> �A�C�e�����g�p����:�p����œ��e���`����B </summary>
    virtual public void UseItem() { }
}
