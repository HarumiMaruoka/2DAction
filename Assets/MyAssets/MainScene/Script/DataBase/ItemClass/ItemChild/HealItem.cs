using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealItem : Item
{
    //�R���X�g���N�^
    public HealItem(ItemID id, string name, ItemType type, int effectSize, string explanatoryText)
        : base(id, name, type, effectSize, explanatoryText)
    {

    }

    public override void UseItem()
    {
        base.UseItem();

        Debug.Log($"Use HealItem : itemID {_myID}");
    }


}
