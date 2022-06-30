using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary> アイテムウィンドウを管理するクラス </summary>
public class ItemMenuWindowManager : MonoBehaviour
{
    //表示するアイテムのフィルター
    public enum ItemFilter
    {
        ALL,
        HEAL,
        POWER_UP,
        MINUS_ITEM,
        KEY,

        ITEM_FILTER_END
    }
    //アイテムフィルター
    ItemFilter _beforeItemFilter;//前フレーム選択していたアイテムフィルター
    ItemFilter _currentItemFilter;//現在のフレームで選択しているアイテムフィルター

    /// <summary> アイテムボタンのプレハブ </summary>
    [Header("アイテムボタンプレハブ"), SerializeField] GameObject _itemButtonPrefab;
    /// <summary> アイテムボタンのゲームオブジェクトの配列 </summary>
    GameObject[] _items = new GameObject[(int)Item.ItemID.ITEM_ID_END];

    /// <summary> 前フレームに選択していたアイテムのID(index) </summary>
    int _beforeItemIndex;
    /// <summary> 現在選択しているアイテムのID(index) </summary>
    int _currentItemIndex;

    //初期化処理
    void Start()
    {
        //アイテムボタンをインスタンシエイトする準備の処理。
        if (_itemButtonPrefab == null)
        {
            Debug.LogError("アイテムボタンのプレハブがアサインされていません。");
        }
        GameObject _itemContent = GameObject.FindGameObjectWithTag("ItemDrawContent");
        if (_itemContent == null)
        {
            Debug.LogError("ItemDrawContentのタグが付いた、オブジェクトの取得に失敗しました。");
        }
        //アイテムボタンをScrollViewの、Contentの子としてインスタンシエイトしておく。
        for (int i = 0; i < (int)Item.ItemID.ITEM_ID_END; i++)
        {
            _items[i] = Instantiate(_itemButtonPrefab, Vector3.zero, Quaternion.identity, _itemContent.transform);

            //ItemDataをセットする
            _items[i].GetComponent<ItemButtonTextManager>().SetItemData(GameManager.Instance.ItemData[i]);
        }
    }

    void Update()
    {

    }

    /// <summary> アイテムウィンドウの更新関数 </summary>
    void UpdateItemWindow()
    {
        //入力
        InputAbove();
        InputBelow();
        InputRight();
        InputLeft();

        //必要があれば(前フレームと現在のフレームで選択しているものが違えば)更新する
        if (_beforeItemFilter != _currentItemFilter ||
            _beforeItemIndex != _currentItemIndex)
        {

        }

        //現在フレームの状態を保存
        _beforeItemFilter = _currentItemFilter;
        _beforeItemIndex = _currentItemIndex;
    }

    /// <summary> 上の入力 </summary>
    void InputAbove()
    {

    }

    /// <summary> 下の入力 </summary>
    void InputBelow()
    {

    }

    /// <summary> 右の入力 </summary>
    void InputRight()
    {

    }

    /// <summary> 左の入力 </summary>
    void InputLeft()
    {

    }
}
