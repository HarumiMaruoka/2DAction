using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTest_Get_or_Lost : MonoBehaviour
{
    [SerializeField] Item.ItemID _itemIndex;
    [SerializeField] bool _isGet;
    [SerializeField] bool _isLost;
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void OnClick_GetItem()
    {
        OldItemDataBase.Instance.MakeChanges_ItemNumberOfPossessions((int)_itemIndex, 1);
    }

    public void OnClick_LostItem()
    {
        OldItemDataBase.Instance.MakeChanges_ItemNumberOfPossessions((int)_itemIndex, -1);
    }
}
