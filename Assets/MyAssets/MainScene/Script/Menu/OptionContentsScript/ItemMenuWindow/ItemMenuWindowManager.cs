using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemMenuWindowManager : MonoBehaviour
{
    //=====表示するアイテムのフィルターのenum=====//
    [System.Serializable]
    public enum ItemFilter
    {
        ALL,
        HEAL,
        POWER_UP,
        MINUS_ITEM,
        KEY,

        ITEM_FILTER_END
    }

    /////<=======このクラスで使用する変数=======>/////
    //=====アイテムフィルター=====//
    /// <summary> 前フレーム選択していたアイテムフィルター </summary>
    ItemFilter _beforeItemFilter;
    /// <summary> 現在のフレームで選択しているアイテムフィルター </summary>
    ItemFilter _currentItemFilter = ItemFilter.ALL;
    /// <summary> 前回選択していたアイテムボタン </summary>
    GameObject _beforeItemButton;
    /// <summary> 現在のアイテムボタン </summary>
    ItemButton _currentItemButton;
    /// <summary> このクラスを初期化したかどうか </summary>
    bool _whetherInitialized;
    /// <summary> コンテントの子の配列 </summary>
    ItemButton[][] _contentChildren;
    //=====コンテントの子となるボタン達=====//
    ItemButton[] _contentALLChildren;
    ItemButton[] _contentHealChildren;
    ItemButton[] _contentPowerUpChildren;
    ItemButton[] _contentMinusChildren;
    ItemButton[] _contentKeyChildren;


    //=========配列類=========//
    /// <summary> アイテムボタンのゲームオブジェクトの配列 </summary>
    GameObject[] _itemButtons = new GameObject[(int)Item.ItemID.ITEM_ID_END];
    /// <summary> アイテムアイコンのイメージ </summary>
    Sprite[] _sprites = new Sprite[(int)Item.ItemID.ITEM_ID_END];

    //=====アサインすべきオブジェクト=====//
    //=====フィルターのボタン類=====//
    [Header("フィルターボタン"), SerializeField] GameObject[] _filters;
    //=====各ボタンの親となるコンテント=====//
    [Header("コンテントの配列"), SerializeField] GameObject[] _contents;
    /// <summary> アイテムボタンのプレハブ </summary>
    [Header("アイテムボタンプレハブ"), SerializeField] GameObject _itemButtonPrefab;
    /// <summary> 説明文のテキスト </summary>
    [SerializeField] Text _itemExplanatoryText;
    /// <summary> アイコンのイメージ </summary>
    [SerializeField] Image _itemIconImage;
    /// <summary> イベントシステム </summary>
    [SerializeField] EventSystem _eventSystem;
    /// <summary>  </summary>
    [SerializeField] ScrollRect _contentParent;

    //=====インスペクタから設定すべき値=====//
    [Header("アイテムアイコンが格納されたフォルダのパス:resource以下名"), SerializeField] string _folderPath;
    [Header("フィルターボタンの通常色"), SerializeField] Color _filterButton_NomalColor;
    [Header("フィルターボタンの選択時の色"), SerializeField] Color _filterButton_SelectedColor;

    //=====仮の入れ物=====//
    GameObject temporaryObject;

    //初期化処理
    void Start()
    {
        //このクラスを初期化する。
        _whetherInitialized = Initialize_ThisClass();
    }

    void Update()
    {
        if (_whetherInitialized)
        {
            Update_Filter();
        }
    }

    /// <summary> 道具画面がアクティブになった時の処理 </summary>
    private void OnEnable()
    {

    }

    //=========フィルター変更に関する処理=========//
    /// <summary> フィルター変更処理 </summary>
    void Update_Filter()
    {
        //フィルターを左にシフトする
        if (Input.GetButtonDown("Horizontal_Left"))
        {
            _currentItemFilter--;
            if (_currentItemFilter < 0)
            {
                _currentItemFilter = (ItemFilter)_filters.Length - 1;
            }
        }
        //フィルターを右にシフトする
        if (Input.GetButtonDown("Horizontal_Right"))
        {
            _currentItemFilter++;
            if (_currentItemFilter > (ItemFilter)_filters.Length - 1)
            {
                _currentItemFilter = 0;
            }
        }
        //必要であればフィルターを更新する
        if (_beforeItemFilter != _currentItemFilter)
        {
            //=====フィルターの更新処理=====//
            //コンテントを入れ替える
            _contentParent.content = _contents[(int)_currentItemFilter].GetComponent<RectTransform>();
            //古いコンテントを非アクティブにして、新しいコンテントをアクティブにする。
            _contents[(int)_currentItemFilter].SetActive(true);
            _contents[(int)_beforeItemFilter].SetActive(false);
            //選択中のオブジェクトを変更する
            Set_SelectedItemButton_ActiveTop(_contentChildren[(int)_currentItemFilter]);
            //ボタンの色を変える
            _filters[(int)_beforeItemFilter].GetComponent<Image>().color = _filterButton_NomalColor;
            _filters[(int)_currentItemFilter].GetComponent<Image>().color = _filterButton_SelectedColor;
        }

        //説明文とアイコンを更新する
        if (_eventSystem.currentSelectedGameObject != _beforeItemButton && _eventSystem.currentSelectedGameObject?.GetComponent<ItemButton>())
        {
            _itemExplanatoryText.text = _eventSystem.currentSelectedGameObject.GetComponent<ItemButton>().MyItem._myExplanatoryText;
            _itemIconImage.sprite = _sprites[(int)_eventSystem.currentSelectedGameObject.GetComponent<ItemButton>().MyItem._myID];
        }

        _beforeItemFilter = _currentItemFilter;
        _beforeItemButton = _eventSystem.currentSelectedGameObject;
    }

    //======アイテムの偏移先を設定する関連======//
    /// <summary> アイテムボタンの偏移先を設定する。 </summary>
    /// <param name="currentButton"> 設定するボタン </param>
    /// <param name="up">    上に設定するボタン </param>
    /// <param name="down">  下に設定するボタン </param>
    /// <param name="left">  左に設定するボタン </param>
    /// <param name="right"> 右に設定するボタン </param>
    void Set_ItemButtonShiftDestinationHelper(ItemButton currentButton, ItemButton up, ItemButton down, GameObject left, GameObject right)
    {
        //ナビゲーションを取得
        Navigation navigation = currentButton.GetComponent<Button>().navigation;
        //モードを変更
        navigation.mode = Navigation.Mode.Explicit;
        //偏移先を指定
        navigation.selectOnUp = up.GetComponent<Button>();
        navigation.selectOnDown = down.GetComponent<Button>();
        navigation.selectOnLeft = left.GetComponent<Button>();
        navigation.selectOnRight = right.GetComponent<Button>();
        //ナビゲーションをセット
        currentButton.GetComponent<Button>().navigation = navigation;
    }
    /// <summary> 各ボタンの偏移先を設定する。 </summary>
    /// <param name="filters"> フィルターボタンの配列 </param>
    /// <param name="itemButton"> アイテムボタンの配列 </param>
    void SetALL_ItemButtonShiftDestination(GameObject[] filters, ItemButton[][] itemButton)
    {
        //外側のアイテムのインデックス
        int itemIndex = 0;
        //外側のループ : フィルターごとの偏移先を設定する。
        for (int filterIndex = 0; filterIndex < filters.Length; filterIndex++)
        {
            //内側のアイテムのインデックス
            int itemIndexIndex = 0;
            //内側のループ : アイテムごとの偏移先を設定する。
            foreach (var item in itemButton[itemIndex])
            {
                //左右上下のボタンを取得。
                int fiL = LargeOrSmall(filterIndex - 1, filters.Length - 1, 0);
                int fiR = LargeOrSmall(filterIndex + 1, filters.Length - 1, 0);
                int iiU = LargeOrSmall(itemIndexIndex - 1, itemButton[itemIndex].Length - 1, 0);
                int iiD = LargeOrSmall(itemIndexIndex + 1, itemButton[itemIndex].Length - 1, 0);

                //偏移先を設定。
                Set_ItemButtonShiftDestinationHelper(
                    itemButton[filterIndex][itemIndexIndex], //カレント
                    itemButton[filterIndex][iiU],       //Up
                    itemButton[filterIndex][iiD],       //Down
                    filters[fiL],                       //Left
                    filters[fiR]);                      //Right
                //インデックスを更新
                itemIndexIndex++;
            }
            //インデックスを更新
            itemIndex++;
        }
    }
    /// <summary> 受け取った値の大小を比較し、最大値より大きければ最小値を返し、最小値より小さければ最大値を返す。 </summary>
    /// <param name="value"> 比較すべき値 </param>
    /// <param name="maxValue"> 最大値 </param>
    /// <param name="minValue"> 最小値 </param>
    /// <returns> valueの値を比較し、min値より小さければmax値を返し、maxより大きければmin値を返す。 </returns>
    int LargeOrSmall(int value, int maxValue, int minValue)
    {
        //最大値を超えていれば最小値を返す。
        if (value > maxValue) return minValue;
        //最小値を下回っていれば最大値を返す。
        else if (value < minValue) return maxValue;
        //どちらでもなければそのまま返す。
        else return value;
    }
    /// <summary> 上下のボタンの偏移先を繋げる </summary>
    /// <param name="upperButton"> 上のボタン </param>
    /// <param name="underButton"> 下のボタン </param>
    void ConnectButton_Vertical(Button currentButton, Button upperButton, Button underButton)
    {
        //上ボタンのナビゲーションを取得
        Navigation navigation = currentButton.navigation;
        //上のボタンを設定
        navigation.selectOnUp = upperButton;
        //下のボタンを設定
        navigation.selectOnDown = underButton;
        //ナビゲーションをセット
        currentButton.navigation = navigation;
    }
    /// <summary> アクティブなアイテムの偏移先を設定する。 </summary>
    /// <param name="item"> アイテムの2次元リスト </param>
    void SetALL_ActiveItem_ShiftDestination(List<List<ItemButton>> item)
    {
        //順番に偏移先を設定する。
        for (int i = 0; i < item.Count; i++)
        {
            for (int j = 0; j < item[i].Count; j++)
            {
                ConnectButton_Vertical(item[i][j].GetComponent<Button>(),
                    item[i][LargeOrSmall(j - 1, item[i].Count - 1, 0)].GetComponent<Button>(),
                    item[i][LargeOrSmall(j + 1, item[i].Count - 1, 0)].GetComponent<Button>());
            }
        }
    }
    /// <summary> アクティブな一番上のアイテムボタンを選択状態にする。 </summary>
    /// <param name="item"> 走査する配列 </param>
    void Set_SelectedItemButton_ActiveTop(ItemButton[] item)
    {
        foreach (var i in item)
        {
            //アクティブなボタンを見つけたら、そのボタンをセレクテッドボタンに設定し、ループを抜ける。
            if (i.gameObject.activeSelf)
            {
                _eventSystem.SetSelectedGameObject(i.gameObject);
                break;
            }
        }
    }
    /// <summary> 上下を繋げる </summary>
    /// <param name="item"> 間のアイテムボタン </param>
    void Connect_TargetButton(ItemButton item)
    {
        //間のナビゲーションから上下のボタンを取得
        Navigation navigation = item.GetComponent<Button>().navigation;
        var up = navigation.selectOnUp;
        var down = navigation.selectOnDown;
        Navigation upNavigation = up.navigation;
        Navigation downNavigation = down.navigation;

        //偏移先を設定
        upNavigation.selectOnDown = down;
        downNavigation.selectOnUp = up;

        //ナビゲーションをセット
        up.navigation = upNavigation;
        down.navigation = downNavigation;
    }

    //==========クラス初期化関連==========//
    /// <summary> このクラスを初期化する。 </summary>
    /// <returns>成功したらtrueを返す。</returns>
    bool Initialize_ThisClass()
    {
        //nullチェック
        if (!CheckNull()) return false;
        //フィルターボタンに値をセットする。
        Set_FilterButton();
        //アイテムボタンに情報をセットする。
        Set_ItemButton();
        //アイテムのアイコン画像を設定する。
        _sprites = Resources.LoadAll<Sprite>(_folderPath);
        //セレクテッドオブジェクトを指定する。
        _eventSystem.SetSelectedGameObject(_itemButtons[0]);
        //コンテントの子を取得する
        Set_ContentChildren();

        //コンテントの偏移先を設定する。
        _contentChildren = new ItemButton[][] { _contentALLChildren, _contentHealChildren, _contentPowerUpChildren, _contentMinusChildren, _contentKeyChildren };
        SetALL_ItemButtonShiftDestination(_filters, _contentChildren);

        //ここに所持数が0個のアイテムを非アクティブにする処理を書く
        Set_ActiveFalse_UnNeedItemALL(_contentChildren);

        SetALL_ActiveItem_ShiftDestination(Get_ActiveItemButton(_contentChildren));

        //フィルターボタンの色を変更する
        _filters[(int)_currentItemFilter].GetComponent<Image>().color = _filterButton_SelectedColor;

        return true;
    }
    /// <summary> nullチェック </summary>
    bool CheckNull()
    {
        if (_itemExplanatoryText == null) { Debug.LogError("説明文のテキストをアサインしてください"); return false; }
        if (_eventSystem == null) { Debug.LogError("EventSystemをアサインしてください"); return false; }
        if (_itemButtonPrefab == null) { Debug.LogError("アイテムボタンのプレハブがアサインされていません。"); return false; }

        if (_filters[(int)ItemFilter.ALL] == null) { Debug.LogError("アイテムフィルターボタンの取得に失敗しました。アサインしてください。: ALL Button"); return false; }
        if (_filters[(int)ItemFilter.HEAL] == null) { Debug.LogError("アイテムフィルターボタンの取得に失敗しました。アサインしてください。: HEAL Button"); return false; }
        if (_filters[(int)ItemFilter.POWER_UP] == null) { Debug.LogError("アイテムフィルターボタンの取得に失敗しました。アサインしてください。: POWER_UP Button"); return false; }
        if (_filters[(int)ItemFilter.MINUS_ITEM] == null) { Debug.LogError("アイテムフィルターボタンの取得に失敗しました。アサインしてください。: MINUS_ITEM Button"); return false; }
        if (_filters[(int)ItemFilter.KEY] == null) { Debug.LogError("アイテムフィルターボタンの取得に失敗しました。アサインしてください。: KEY Button"); return false; }

        if (_contents[(int)ItemFilter.ALL] == null) { Debug.LogError("アイテムコンテントALLをアサインしてください。"); return false; }
        if (_contents[(int)ItemFilter.HEAL] == null) { Debug.LogError("アイテムコンテントHealをアサインしてください。"); return false; }
        if (_contents[(int)ItemFilter.POWER_UP] == null) { Debug.LogError("アイテムコンテントPowerUpをアサインしてください。"); return false; }
        if (_contents[(int)ItemFilter.MINUS_ITEM] == null) { Debug.LogError("アイテムコンテントMinusをアサインしてください。"); return false; }
        if (_contents[(int)ItemFilter.KEY] == null) { Debug.LogError("アイテムコンテントKeyをアサインしてください。"); return false; }

        if (_contentParent == null) { Debug.LogError("ScrollViewのScrollRectをアサインしてください。"); return false; }

        return true;
    }
    /// <summary> フィルターボタンに情報を設定する。 </summary>
    void Set_FilterButton()
    {
        _filters[(int)ItemFilter.ALL].GetComponent<ItemFilterButton>().Set_ItemFilter(ItemFilter.ALL);
        _filters[(int)ItemFilter.HEAL].GetComponent<ItemFilterButton>().Set_ItemFilter(ItemFilter.HEAL);
        _filters[(int)ItemFilter.POWER_UP].GetComponent<ItemFilterButton>().Set_ItemFilter(ItemFilter.POWER_UP);
        _filters[(int)ItemFilter.MINUS_ITEM].GetComponent<ItemFilterButton>().Set_ItemFilter(ItemFilter.MINUS_ITEM);
        _filters[(int)ItemFilter.KEY].GetComponent<ItemFilterButton>().Set_ItemFilter(ItemFilter.KEY);
    }
    /// <summary> アイテムボタンに情報を設定する。 </summary>
    void Set_ItemButton()
    {
        //アイテムボタンをScrollViewの、Contentの子としてインスタンシエイトしておき、データをセットする。
        for (int i = 0; i < (int)Item.ItemID.ITEM_ID_END; i++)
        {
            //ALLコンテントの子としてインスタンシエイトする。
            _itemButtons[i] = Instantiate(_itemButtonPrefab, Vector3.zero, Quaternion.identity, _contents[(int)ItemFilter.ALL].transform);
            //データをセット
            _itemButtons[i].GetComponent<ItemButton>().SetItemData(GameManager.Instance.ItemData[i]);

            //各コンテントに子としてインスタンシエイトする。
            switch (_itemButtons[i].GetComponent<ItemButton>().MyItem._myType)
            {
                case Item.ItemType.HEAL: temporaryObject = Instantiate(_itemButtonPrefab, Vector3.zero, Quaternion.identity, _contents[(int)ItemFilter.HEAL].transform); break;
                case Item.ItemType.POWER_UP: temporaryObject = Instantiate(_itemButtonPrefab, Vector3.zero, Quaternion.identity, _contents[(int)ItemFilter.POWER_UP].transform); break;
                case Item.ItemType.MINUS_ITEM: temporaryObject = Instantiate(_itemButtonPrefab, Vector3.zero, Quaternion.identity, _contents[(int)ItemFilter.MINUS_ITEM].transform); break;
                case Item.ItemType.KEY: temporaryObject = Instantiate(_itemButtonPrefab, Vector3.zero, Quaternion.identity, _contents[(int)ItemFilter.KEY].transform); break;
            }
            //データをセット
            temporaryObject.GetComponent<ItemButton>().SetItemData(GameManager.Instance.ItemData[i]);
        }
    }
    /// <summary> コンテントの子を取得し、変数に保存する。 </summary>
    void Set_ContentChildren()
    {
        _contentALLChildren = _contents[(int)ItemFilter.ALL].GetComponentsInChildren<ItemButton>();
        _contentHealChildren = _contents[(int)ItemFilter.HEAL].GetComponentsInChildren<ItemButton>();
        _contentPowerUpChildren = _contents[(int)ItemFilter.POWER_UP].GetComponentsInChildren<ItemButton>();
        _contentMinusChildren = _contents[(int)ItemFilter.MINUS_ITEM].GetComponentsInChildren<ItemButton>();
        _contentKeyChildren = _contents[(int)ItemFilter.KEY].GetComponentsInChildren<ItemButton>();
    }
    /// <summary> 所持数0のアイテムを全て非アクティブにする </summary>
    void Set_ActiveFalse_UnNeedItemALL(ItemButton[][] item)
    {
        for (int i = 0; i < item.Length; i++)
        {
            for (int j = 0; j < item[i].Length; j++)
            {
                //所持数が0かどうか判定する。
                //0個であれば非アクティブにする
                if (ItemHaveValueManager.Instance.ItemVolume._itemNumberOfPossessions[(int)item[i][j].GetComponent<ItemButton>().MyItem._myID] == 0)
                {
                    item[i][j].gameObject.SetActive(false);
                }
                //そうでなければアクティブにする
                else
                {
                    item[i][j].gameObject.SetActive(true);
                }
            }
        }
    }

    //=========便利な関数群=========//
    /// <summary> アクティブなボタンの配列を取得する </summary>
    /// <param name="item"> 検索するアイテムボタン群 </param>
    /// <returns> アクティブなボタンのリスト </returns>
    List<List<ItemButton>> Get_ActiveItemButton(ItemButton[][] item)
    {
        List<List<ItemButton>> itemButtons = new List<List<ItemButton>>();

        //ここにアクティブなボタンを保存する処理を書く
        int index = 0;
        foreach (var i in item)
        {
            itemButtons.Add(new List<ItemButton>());
            foreach (var j in i)
            {
                if (j.gameObject.activeSelf)
                {
                    itemButtons[index].Add(j);
                }
            }
            index++;
        }

        return itemButtons;
    }
    /// <summary> 指定されたアイテムを非アクティブにする </summary>
    void Set_ActiveFalse_UnNeedItem(ItemButton item)
    {
        item.gameObject.SetActive(false);
    }
    /// <summary> そのフィルターの一番上のアクティブなアイテムボタンを取得する </summary>
    ItemButton Get_TopActiveObject(ItemFilter filter)
    {
        foreach (var item in _contentChildren[(int)filter])
        {
            if (item.gameObject.activeSelf) return item;
        }
        return null;
    }

    //=========他のクラスから呼び出すメソッド=========//
    /// <summary> フィルターを変更する </summary>
    /// <param name="itemFilter"> 新しいフィルター </param>
    public void Set_CurrentFillter(ItemFilter itemFilter)
    {
        _currentItemFilter = itemFilter;
    }
    /// <summary> アイテムの所持数が0になった時の処理 </summary>
    public void ShouldDo_HaveItemZero(ItemButton item, Item.ItemID ID, ItemFilter filter)
    {
        //一番上のアクティブなボタンを取得
        ItemButton itemTop = Get_TopActiveObject(_currentItemFilter);
        //受け取ったボタンを非アクティブにし、上下の偏移先を設定する。
        item.gameObject.SetActive(false);
        Connect_TargetButton(item);
        //selectedオブジェクトを変更する
        //対象のボタンが一番上であれば、一つ下をselectオブジェクトにする。

        //コンテントの一番上なら、一つ下のアイテムをセレクテッドオブジェクトに指定する
        if (itemTop == item)
        {
            _eventSystem.SetSelectedGameObject(item.GetComponent<Button>().navigation.selectOnDown.gameObject);
        }
        //それ以外ならセレクテッドオブジェクトを一つ上のボタンにする
        else
        {
            _eventSystem.SetSelectedGameObject(item.GetComponent<Button>().navigation.selectOnUp.gameObject);
        }
    }
}
