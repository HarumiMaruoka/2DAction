using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpItem : Item
{
    //コンストラクタ
    public PowerUpItem(ItemID id, string name, ItemType type, int effectSize,string explanatoryText) 
        : base(id, name, type, effectSize, explanatoryText)
    {

    }

    public override void UseItem()
    {
        base.UseItem();
        Debug.Log("Use PowerUpItem");
    }
}
