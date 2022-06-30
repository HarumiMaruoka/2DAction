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
    virtual public void UseItem() { }
}
