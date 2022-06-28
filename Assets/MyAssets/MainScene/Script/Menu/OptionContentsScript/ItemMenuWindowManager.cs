using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemMenuWindowManager : MonoBehaviour
{
    public enum ItemFilter
    {
        ALL,
        HEAL,
        POWER_UP,
        MINUS_ITEM,
        KEY,

        ITEM_FILTER_END
    }

    [Header("アイテムボタンプレハブ"), SerializeField] GameObject _itemButtonPrefab;
    GameObject[] _items = new GameObject[(int)Item.ItemID.ITEM_ID_END];

    //アイテムフィルター
    ItemFilter _beforeItemFilter;//前フレーム選択していたアイテムフィルター
    ItemFilter _currentItemFilter;//現在のフレームで選択しているアイテムフィルター

    //現在選択しているアイテムのインデックス
    int _beforeItemIndex;
    int _currentItemIndex;




    void Start()
    {
        if (_itemButtonPrefab == null)
        {
            Debug.LogError("アイテムボタンのプレハブがアサインされていません。");
        }
        //必要なものを、子としてインスタンシエイトしておく。
        for (int i = 0; i < (int)Item.ItemID.ITEM_ID_END; i++)
        {
            _items[i] = Instantiate(_itemButtonPrefab, Vector3.zero, Quaternion.identity, gameObject.transform);

            //テキストをセットする
            Text itemText = _items[i].transform.GetChild(0).gameObject.GetComponent<Text>();
            itemText.text = "ここにアイテムの名前と個数を入れる。名前と個数の間にはたくさんSpaceを入れる";
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
