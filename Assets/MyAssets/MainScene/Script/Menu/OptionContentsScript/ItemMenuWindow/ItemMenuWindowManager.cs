using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
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

    //このクラスで使用する変数

    //アイテムフィルター
    /// <summary> 前フレーム選択していたアイテムフィルター </summary>
    ItemFilter _beforeItemFilter;
    /// <summary> 現在のフレームで選択しているアイテムフィルター </summary>
    ItemFilter _currentItemFilter;

    //選択しているアイテムのID
    /// <summary> 前フレームに選択していたアイテムのID(index) </summary>
    int _beforeItemIndex;
    /// <summary> 現在選択しているアイテムのID(index) </summary>
    int _currentItemIndex;

    //各プレハブ
    /// <summary> アイテムボタンのプレハブ </summary>
    [Header("アイテムボタンプレハブ"), SerializeField] GameObject _itemButtonPrefab;


    //配列類
    /// <summary> アイテムボタンのゲームオブジェクトの配列 </summary>
    GameObject[] _items = new GameObject[(int)Item.ItemID.ITEM_ID_END];
    /// <summary> アイテムアイコンのイメージ </summary>
    Sprite[] _sprites = new Sprite[(int)Item.ItemID.ITEM_ID_END];

    //アサインすべきオブジェクト
    /// <summary> 説明文のテキスト </summary>
    [SerializeField] Text _ItemExplanatoryText;
    /// <summary> アイコンのイメージ </summary>
    [SerializeField] Image _itemIconImage;
    /// <summary> イベントシステム </summary>
    [SerializeField] EventSystem _eventSystem;

    //インスペクタから設定すべき値
    [Header("アイテムアイコンが格納されたフォルダのパス:resource以下名"), SerializeField] string _folderPath;


    //初期化処理
    void Start()
    {
        //アイテムのアイコン画像を取得
        _sprites = Resources.LoadAll<Sprite>(_folderPath);

        //nullチェック
        if (_ItemExplanatoryText == null)
        {
            Debug.LogError("説明文のテキストをアサインしてください");
        }
        if (_eventSystem == null)
        {
            Debug.LogError("EventSystemをアサインしてください");
        }

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
            _items[i].GetComponent<ItemButton>().SetItemData(GameManager.Instance.ItemData[i]);
        }

        _eventSystem.SetSelectedGameObject(_items[0]);

        //select on up と down を設定する 汚いのであとで直す
        Navigation navigation = _items[(int)Item.ItemID.ITEM_ID_00].GetComponent<Button>().navigation;
        navigation.mode = Navigation.Mode.Explicit;
        navigation.selectOnUp = _items[(int)Item.ItemID.ITEM_ID_END - 1].GetComponent<Button>();
        navigation.selectOnDown = _items[(int)Item.ItemID.ITEM_ID_01].GetComponent<Button>();
        _items[(int)Item.ItemID.ITEM_ID_00].GetComponent<Button>().navigation = navigation;
        //select on up と down を設定する
        navigation = _items[(int)Item.ItemID.ITEM_ID_END - 1].GetComponent<Button>().navigation;
        navigation.mode = Navigation.Mode.Explicit;
        navigation.selectOnDown = _items[(int)Item.ItemID.ITEM_ID_00].GetComponent<Button>();
        navigation.selectOnUp = _items[(int)Item.ItemID.ITEM_ID_END - 2].GetComponent<Button>();
        _items[(int)Item.ItemID.ITEM_ID_END - 1].GetComponent<Button>().navigation = navigation;
    }

    void Update()
    {
        UpdateItemWindow();
    }

    private void OnEnable()
    {
        _eventSystem.SetSelectedGameObject(_items[0]);
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
        //現在選択されているボタンを取得
        GameObject nowSelectItem = EventSystem.current.currentSelectedGameObject;

        //説明文と画像を設定
        if (nowSelectItem != null && nowSelectItem.TryGetComponent<ItemButton>(out ItemButton item))
        {
            _ItemExplanatoryText.text = item.MyItem._myExplanatoryText;
            _itemIconImage.sprite = _sprites[(int)item.MyItem._myID];
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
