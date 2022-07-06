using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary> アイテムウィンドウを管理するクラス </summary>
public class ItemMenuWindowManager : MonoBehaviour
{
    [System.Serializable]
    //表示するアイテムのフィルター
    public enum ItemFilter
    {
        HEAL,
        POWER_UP,
        MINUS_ITEM,
        KEY,

        ALL,

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

    //フィルターのボタン類
    GameObject _allFilterButton;
    GameObject _healFilterButton;
    GameObject _powerUpFilterButton;
    GameObject _minusItemFilterButton;
    GameObject _keyFilterButton;

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
        //アイテムフィルター
        _currentItemFilter = ItemFilter.ALL;

        if ((_allFilterButton = GameObject.FindGameObjectWithTag("ItemFilterALL"))==null) Debug.LogError("アイテムフィルターボタンの取得に失敗しました。タグを設定してください。: ALL Button");
        if ((_healFilterButton = GameObject.FindGameObjectWithTag("ItemFilterHEAL"))==null) Debug.LogError("アイテムフィルターボタンの取得に失敗しました。タグを設定してください。: HEAL Button");
        if ((_powerUpFilterButton = GameObject.FindGameObjectWithTag("ItemFilterPOWER_UP"))== null) Debug.LogError("アイテムフィルターボタンの取得に失敗しました。タグを設定してください。: POWER_UP Button");
        if ((_minusItemFilterButton = GameObject.FindGameObjectWithTag("ItemFilterMINUS_ITEM")) == null) Debug.LogError("アイテムフィルターボタンの取得に失敗しました。タグを設定してください。: MINUS_ITEM Button");
        if ((_keyFilterButton = GameObject.FindGameObjectWithTag("ItemFilterKEY")) == null) Debug.LogError("アイテムフィルターボタンの取得に失敗しました。タグを設定してください。: KEY Button");

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


        //ボタンのキー入力の偏移先の指定処理。
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

        //各ボタンの偏移先を設定する。最初はALLの設定
        Change_ItemFilter(ItemFilter.ALL);
    }

    void Update()
    {
        UpdateItemWindow();
    }

    private void OnEnable()
    {
        //初期選択オブジェクトを指定
        _eventSystem.SetSelectedGameObject(_items[0]);
        //アイテムフィルターはオール
        Change_ItemFilter(ItemFilter.ALL);
        _currentItemFilter = ItemFilter.ALL;
    }

    /// <summary> アイテムウィンドウの更新関数 </summary>
    void UpdateItemWindow()
    {
        //入力
        InputAbove();
        InputBelow();
        InputRight();
        InputLeft();

        //必要であれば、アイテムフィルターを更新する。
        Update_ItemFilter();

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

    /// <summary> アイテムを種類別で表示する </summary>
    void Update_ItemFilter()
    {
        //最初の要素を頭にする
        bool firstObj = true;

        //前と今のフィルターが違えば更新する
        if (_beforeItemFilter != _currentItemFilter)
        {
            //全て表示する場合
            if (_currentItemFilter == ItemFilter.ALL)
            {
                foreach (var item in _items)
                {
                    //頭の要素をSelectedGameObjectに指定する。
                    if (firstObj)
                    {
                        firstObj = false;
                        _eventSystem.SetSelectedGameObject(item);
                    }
                    item.SetActive(true);
                }
            }
            //一部表示する場合
            else
            {
                foreach (var item in _items)
                {
                    if ((int)item.GetComponent<ItemButton>().MyItem._myType == (int)_currentItemFilter)
                    {
                        //頭の要素をSelectedGameObjectに指定する。
                        if (firstObj)
                        {
                            firstObj = false;
                            _eventSystem.SetSelectedGameObject(item);
                        }
                        item.SetActive(true);
                    }
                    else
                    {
                        item.SetActive(false);
                    }
                }
            }
        }
        _beforeItemFilter = _currentItemFilter;
    }

    //アイテムボタンの偏移先を決める処理
    void Set_ItemButtonShiftDestination(Button currentButton, Button up, Button down, Button left, Button right)
    {
        //ナビゲーションを取得
        Navigation navigation = currentButton.navigation;
        //モードを変更
        navigation.mode = Navigation.Mode.Explicit;
        //偏移先を指定
        navigation.selectOnUp = up;
        navigation.selectOnDown = down;
        navigation.selectOnLeft = left;
        navigation.selectOnRight = right;
        //ナビゲーションをセット
        currentButton.navigation = navigation;
    }

    /// <summary> アイテムフィルターが、切り替わった時に行うべき処理。 </summary>
    /// <param name="itemFilter"> このフィルターに合わせた設定を行う。 </param>
    void Change_ItemFilter(ItemFilter itemFilter)
    {
        switch (itemFilter)
        {

        }
    }

    void Set_ItemFilter_ALL()
    {

    }

    void Set_ItemFilter_HEAL()
    {

    }

    void Set_ItemFilter_POWERUP()
    {

    }

    void Set_ItemFilter_MINUSITEM()
    {

    }

    void Set_ItemFilter_KEY()
    {

    }
}
