using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealItem : Item
{
    //コンストラクタ
    public HealItem(ItemID id, string name, ItemType type, int effectSize, string explanatoryText)
        : base(id, name, type, effectSize, explanatoryText)
    {

    }

    public override void UseItem()
    {
        base.UseItem();

        Debug.Log($"Use HealItem : itemID {_myID}");
        if (PlayerStatusManager.Instance.ConsequentialPlayerStatus._maxHp < PlayerStatusManager.Instance.PlayerHealthPoint + _myEffectSize)
        {
            PlayerStatusManager.Instance.PlayerHealthPoint = PlayerStatusManager.Instance.ConsequentialPlayerStatus._maxHp;
        }
        else
        {
            PlayerStatusManager.Instance.PlayerHealthPoint += _myEffectSize;
        }
    }
}
