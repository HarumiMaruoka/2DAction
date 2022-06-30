using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    void Start()
    {

    }

    void Update()
    {

    }

    public void PlusItem(int itemID)
    {
        PlayerManager.Instance.Set_ItemNumberOfPossessions(itemID, 1);
        Debug.Log("アイテムID : " + itemID + "の現在の個数は" + PlayerManager.Instance.ItemVolume._itemNumberOfPossessions[itemID] + "です。");
    }

    public void MinusItem(int itemID)
    {
        PlayerManager.Instance.Set_ItemNumberOfPossessions(itemID, -1);
        Debug.Log("アイテムID : " + itemID + "の現在の個数は" + PlayerManager.Instance.ItemVolume._itemNumberOfPossessions[itemID] + "です。");
    }
}
