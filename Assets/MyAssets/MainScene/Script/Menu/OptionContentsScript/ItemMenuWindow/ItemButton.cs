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

    ItemMenuWindowManager _itemWindowManager;

    public void SetItemData(Item item)
    {
        _myItem = item;
    }


    void Start()
    {
        _itemWindowManager = GameObject.FindGameObjectWithTag("ToolWindow").GetComponent<ItemMenuWindowManager>();
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
        if (PlayerStatusManager.Instance == null)
        {
            Debug.LogError("PlayerManagerがnullです！");
        }
        //所持数を取得
        _nowItemVolume = ItemHaveValueManager.Instance.ItemVolume._itemNumberOfPossessions[(int)_myItem._myID];
        if (_nowItemVolume != _beforeItemVolume)
        {
            Update_ItemVolume();
        }
        //所持数が0以下になった時の処理
        if (_nowItemVolume <= 0)
        {
            Debug.Log(_myItem._name + "の所持数が0になりました。\n" +
                "_myItem._nameのボタンを非アクティブにします。");
            _itemWindowManager.ShouldDo_HaveItemZero(this, _myItem._myID, (ItemMenuWindowManager.ItemFilter)_myItem._myType);
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
            _itemVolumText.text = " × " + ItemHaveValueManager.Instance.ItemVolume._itemNumberOfPossessions[(int)_myItem._myID].ToString() + " ";
        }
    }


    /// <summary> ボタンを押したら実行する </summary>
    public void Use_ThisItem()
    {
        GameManager.Instance.ItemData[(int)MyItem._myID].UseItem();
    }
}
