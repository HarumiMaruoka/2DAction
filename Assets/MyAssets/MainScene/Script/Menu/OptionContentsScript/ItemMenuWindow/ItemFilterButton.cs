using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemFilterButton : MonoBehaviour
{
    ItemMenuWindowManager.ItemFilter _newMyFilter;
    ItemMenuWindowManager _newItemMenuWindowManager;

    /// <summary> アイテムフィルターをセットする </summary>
    public void Set_ItemFilter(ItemMenuWindowManager.ItemFilter itemFilter)
    {
        _newMyFilter = itemFilter;
    }

    void Start()
    {
        _newItemMenuWindowManager = GameObject.FindGameObjectWithTag("ToolWindow").GetComponent<ItemMenuWindowManager>();
    }

    public void OnClick_FilterUpdate()
    {
        _newItemMenuWindowManager.Set_CurrentFillter(_newMyFilter);
    }
}
