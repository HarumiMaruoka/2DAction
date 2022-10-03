using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// テスト用スクリプト
/// アイテムの取得と喪失コンポーネント
/// ボタンにアタッチして使用する
/// </summary>
public class ItemTest_Get_or_Lost : MonoBehaviour
{
    [SerializeField] Item.ItemID _itemIndex;
    [SerializeField] bool _isGet;
    [SerializeField] bool _isLost;

    public void OnClick_GetItem()
    {
        NewItemDataBase.Instance.PlayerHaveItemData.MakeChangesHaveItemData((int)_itemIndex, 1);
    }

    public void OnClick_LostItem()
    {
        NewItemDataBase.Instance.PlayerHaveItemData.MakeChangesHaveItemData((int)_itemIndex, -1);
    }
}
