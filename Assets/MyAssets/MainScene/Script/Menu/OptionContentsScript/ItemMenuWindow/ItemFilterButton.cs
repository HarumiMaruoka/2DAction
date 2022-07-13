using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemFilterButton : MonoBehaviour
{
    NewItemMenuWindowManager.ItemFilter _newMyFilter;
    NewItemMenuWindowManager _newItemMenuWindowManager;

    /// <summary> アイテムフィルターをセットする </summary>
    public void Set_ItemFilter(NewItemMenuWindowManager.ItemFilter itemFilter)
    {
        _newMyFilter = itemFilter;
    }

    void Start()
    {
        _newItemMenuWindowManager = GameObject.FindGameObjectWithTag("ToolWindow").GetComponent<NewItemMenuWindowManager>();
    }

    public void OnClick_FilterUpdate()
    {
        _newItemMenuWindowManager.Set_CurrentFillter(_newMyFilter);
    }
}
