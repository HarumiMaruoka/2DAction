using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemButton : MonoBehaviour
{
    //アイテム名のテキスト
    Text _itemNameText;
    //個数のテキスト
    Text _itemVolumText;

    //このButtonが持つItem
    Item _myItem;
    public Item MyItem { get => _myItem; }

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
        if (PlayerManager.Instance == null)
        {
            Debug.Log("aaa");
        }
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
        if (_itemVolumText != null)
        {
            _itemVolumText.text = " × " + PlayerManager.Instance.ItemVolume._itemNumberOfPossessions[(int)_myItem._myID].ToString() + " ";
        }
    }


    /// <summary> ボタンを押したら実行する </summary>
    public void Use_ThisItem()
    {
        GameManager.Instance.ItemData[(int)MyItem._myID].UseItem();
    }
}
