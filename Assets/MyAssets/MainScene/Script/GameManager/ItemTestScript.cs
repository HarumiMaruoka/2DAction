using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> ただのアイテム所持数を変化させる為だけのクラス。(テスト用) </summary>
public class ItemTestScript : MonoBehaviour
{
    void Start()
    {

    }

    void Update()
    {

    }

    /// <summary> 特定のIDのアイテムを1つ増やす。 </summary>
    public void PlusItem(int itemID)
    {
        // OldItemDataBase.Instance.MakeChanges_ItemNumberOfPossessions(itemID, 1);
        NewItemDataBase.Instance.PlayerHaveItemData.MakeChangesHaveItemData(itemID, 1);
    }

    /// <summary> 特定のIDのアイテムを1つ減らす。 </summary>
    public void MinusItem(int itemID)
    {
        // OldItemDataBase.Instance.MakeChanges_ItemNumberOfPossessions(itemID, -1);
        NewItemDataBase.Instance.PlayerHaveItemData.MakeChangesHaveItemData(itemID, -1);
    }
}
