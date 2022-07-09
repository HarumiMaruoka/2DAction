using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemFilterButton : MonoBehaviour
{
    ItemMenuWindowManager.ItemFilter _myItemFilter;
    ItemMenuWindowManager _itemMenuWindowManager;

    /// <summary> アイテムフィルターをセットする </summary>
    public void Set_ItemFilter(ItemMenuWindowManager.ItemFilter itemFilter)
    {
        _myItemFilter = itemFilter;
    }

    void Start()
    {
        _itemMenuWindowManager = GameObject.FindGameObjectWithTag("ToolWindow").GetComponent<ItemMenuWindowManager>();
    }

    public void OnClick_FilterUpdate()
    {
        _itemMenuWindowManager.Set_CurrentFilter(_myItemFilter);
    }
}
