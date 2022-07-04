using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinusItem : Item
{
    //�R���X�g���N�^
    public MinusItem(ItemID id, string name, ItemType type, int effectSize, string explanatoryText)
        : base(id, name, type, effectSize, explanatoryText)
    {

    }

    public override void UseItem()
    {
        base.UseItem();
        Debug.Log("Use MinusItem");
    }
}
