using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemButtonTextManager : MonoBehaviour
{
    //アイテム名のテキスト
    Text _itemNameText;
    //個数のテキスト
    Text _itemVolumText;
    //このButtonが持つItem
    Item _myItem;

    int _beforeItemVolume;
    int _nowItemVolume;

    public void SetItemData(Item item)
    {
        _myItem = item;
    }


    void Start()
    {
        //テキストの取得
        _itemNameText = transform.GetChild(0).GetComponent<Text>();
        _itemVolumText = transform.GetChild(1).GetComponent<Text>();

        //名前を設定
        _itemNameText.text = " " + _myItem._name;
        //所持数を設定
        Update_ItemVolume();
    }

    private void Update()
    {
        _nowItemVolume = PlayerManager.Instance.ItemVolume._itemNumberOfPossessions[(int)_myItem._myID];
        if (_nowItemVolume != _beforeItemVolume)
        {
            Update_ItemVolume();
        }
        _beforeItemVolume = _nowItemVolume;
    }

    /// <summary> アイテムボタンがアクティブになった時の処理。 </summary>
    private void OnEnable()
    {
        //所持数をセットする
        Update_ItemVolume();
    }

    /// <summary> 所持数を更新する。 </summary>
    public void Update_ItemVolume()
    {
        _itemVolumText.text = " × " + PlayerManager.Instance.ItemVolume._itemNumberOfPossessions[(int)_myItem._myID].ToString() + " ";
    }
}
