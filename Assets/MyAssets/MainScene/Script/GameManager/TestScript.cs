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
        Debug.Log("�A�C�e��ID : " + itemID + "�̌��݂̌���" + PlayerManager.Instance.ItemVolume._itemNumberOfPossessions[itemID] + "�ł��B");
    }

    public void MinusItem(int itemID)
    {
        PlayerManager.Instance.Set_ItemNumberOfPossessions(itemID, -1);
        Debug.Log("�A�C�e��ID : " + itemID + "�̌��݂̌���" + PlayerManager.Instance.ItemVolume._itemNumberOfPossessions[itemID] + "�ł��B");
    }
}
