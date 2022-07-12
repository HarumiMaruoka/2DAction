using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemFilterButton : MonoBehaviour
{
    ItemMenuWindowManager.ItemFilter _myItemFilter;
    ItemMenuWindowManager _itemMenuWindowManager;
    NewItemMenuWindowManager.ItemFilter _newMyFilter;
    NewItemMenuWindowManager _newItemMenuWindowManager;

    /// <summary> アイテムフィルターをセットする </summary>
    public void Set_ItemFilter(NewItemMenuWindowManager.ItemFilter itemFilter)
    {
        _newMyFilter = itemFilter;
    }

    void Start()
    {
        _itemMenuWindowManager = GameObject.FindGameObjectWithTag("ToolWindow").GetComponent<ItemMenuWindowManager>();
        _newItemMenuWindowManager = GameObject.FindGameObjectWithTag("ToolWindow").GetComponent<NewItemMenuWindowManager>();
    }

    public void OnClick_FilterUpdate()
    {
        _itemMenuWindowManager.Set_CurrentFilter(_myItemFilter);
        _newItemMenuWindowManager.Set_CurrentFillter(_newMyFilter);
    }
}
