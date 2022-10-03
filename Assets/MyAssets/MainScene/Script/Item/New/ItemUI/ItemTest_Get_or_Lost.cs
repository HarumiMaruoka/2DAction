using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �e�X�g�p�X�N���v�g
/// �A�C�e���̎擾�Ƒr���R���|�[�l���g
/// �{�^���ɃA�^�b�`���Ďg�p����
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
