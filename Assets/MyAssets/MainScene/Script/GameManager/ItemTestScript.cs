using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> �����̃A�C�e����������ω�������ׂ����̃N���X�B(�e�X�g�p) </summary>
public class ItemTestScript : MonoBehaviour
{
    void Start()
    {

    }

    void Update()
    {

    }

    /// <summary> �����ID�̃A�C�e����1���₷�B </summary>
    public void PlusItem(int itemID)
    {
        ItemDataBase.Instance.Set_ItemNumberOfPossessions(itemID, 1);
    }

    /// <summary> �����ID�̃A�C�e����1���炷�B </summary>
    public void MinusItem(int itemID)
    {
        ItemDataBase.Instance.Set_ItemNumberOfPossessions(itemID, -1);
    }
}
