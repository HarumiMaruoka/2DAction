using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Item
{
    //�R���X�g���N�^
    public Item(ItemID id, string name, ItemType type, int effectSize, string explanatoryText)
    {
        _myID = id;
        _name = name;
        _myType = type;
        _myEffectSize = effectSize;
        _myExplanatoryText = explanatoryText;
    }
    //�A�C�e���̊��N���X:�e�A�C�e���͂��̃N���X���p������
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

    /// <summary> ���̃A�C�e����ID </summary>
    public ItemID _myID { get; }
    /// <summary> ���̃A�C�e���̖��O </summary>
    public string _name { get; }
    /// <summary> ���̃A�C�e���̎�� </summary>
    public ItemType _myType { get; }
    /// <summary> ���̃A�C�e���̌��ʗ� </summary>
    public int _myEffectSize { get; }
    /// <summary> ���̃A�C�e���̐����� </summary>
    public string _myExplanatoryText { get; }

    /// <summary> �A�C�e�����g�p����:�p����œ��e���`����B </summary>
    public virtual void UseItem()
    {
        Debug.Log($"�A�C�e�����ʂ̏��� : �A�C�e��ID.{_myID}���g�p�B");
        ItemDataBase.Instance.MakeChanges_ItemNumberOfPossessions((int)_myID, -1);
    }
}
