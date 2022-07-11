using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

        if ((_allFilterButton = GameObject.FindGameObjectWithTag("ItemFilterALL")) == null) Debug.LogError("アイテムフィルターボタンの取得に失敗しました。タグを設定してください。: ALL Button");
        if ((_healFilterButton = GameObject.FindGameObjectWithTag("ItemFilterHEAL")) == null) Debug.LogError("アイテムフィルターボタンの取得に失敗しました。タグを設定してください。: HEAL Button");
        if ((_powerUpFilterButton = GameObject.FindGameObjectWithTag("ItemFilterPOWER_UP")) == null) Debug.LogError("アイテムフィルターボタンの取得に失敗しました。タグを設定してください。: POWER_UP Button");
        if ((_minusItemFilterButton = GameObject.FindGameObjectWithTag("ItemFilterMINUS_ITEM")) == null) Debug.LogError("アイテムフィルターボタンの取得に失敗しました。タグを設定してください。: MINUS_ITEM Button");
        if ((_keyFilterButton = GameObject.FindGameObjectWithTag("ItemFilterKEY")) == null) Debug.LogError("アイテムフィルターボタンの取得に失敗しました。タグを設定してください。: KEY Button");

        _allFilterButton.GetComponent<ItemFilterButton>().Set_ItemFilter(ItemFilter.ALL);
        _healFilterButton.GetComponent<ItemFilterButton>().Set_ItemFilter(ItemFilter.HEAL);
        _powerUpFilterButton.GetComponent<ItemFilterButton>().Set_ItemFilter(ItemFilter.POWER_UP);
        _minusItemFilterButton.GetComponent<ItemFilterButton>().Set_ItemFilter(ItemFilter.MINUS_ITEM);
        _keyFilterButton.GetComponent<ItemFilterButton>().Set_ItemFilter(ItemFilter.KEY);

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
        GameObject _itemContent = GameObject.FindGameObjectWithTag("ItemDrawContentALL");
        if (_itemContent == null)
        {
            Debug.LogError("ItemDrawContentALLのタグが付いた、オブジェクトの取得に失敗しました。");
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
        //Navigation navigation = _items[(int)Item.ItemID.ITEM_ID_00].GetComponent<Button>().navigation;
        //navigation.mode = Navigation.Mode.Explicit;
        //navigation.selectOnUp = _items[(int)Item.ItemID.ITEM_ID_END - 1].GetComponent<Button>();
        //navigation.selectOnDown = _items[(int)Item.ItemID.ITEM_ID_01].GetComponent<Button>();
        //_items[(int)Item.ItemID.ITEM_ID_00].GetComponent<Button>().navigation = navigation;
        ////select on up と down を設定する
        //navigation = _items[(int)Item.ItemID.ITEM_ID_END - 1].GetComponent<Button>().navigation;
        //navigation.mode = Navigation.Mode.Explicit;
        //navigation.selectOnDown = _items[(int)Item.ItemID.ITEM_ID_00].GetComponent<Button>();
        //navigation.selectOnUp = _items[(int)Item.ItemID.ITEM_ID_END - 2].GetComponent<Button>();
        //_items[(int)Item.ItemID.ITEM_ID_END - 1].GetComponent<Button>().navigation = navigation;


        //各ボタンの偏移先を設定する。最初はALLの設定
        Set_ItemButtonShiftDestination(ItemFilter.ALL, _items);
    }

    void Update()
    {
        UpdateItemWindow();
    }

    private void OnEnable()
    {
        Update_ItemFilter();
        _currentItemFilter = ItemFilter.ALL;
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

    /// <summary> 上の入力があった時の処理 </summary>
    void InputAbove()
    {

    }

    /// <summary> 下の入力があった時の処理 </summary>
    void InputBelow()
    {

    }

    /// <summary> 右の入力があった時の処理 </summary>
    void InputRight()
    {
        //右の入力を検知
        if (Input.GetButtonDown("Horizontal_Right"))
        {
            //ここにアイテムフィルターを変更する処理を書く
            _currentItemFilter = _currentItemFilter + 1;
            if (_currentItemFilter >= ItemFilter.ITEM_FILTER_END)
            {
                _currentItemFilter = ItemFilter.HEAL;
            }
        }
    }

    /// <summary> 左の入力があった時の処理 </summary>
    void InputLeft()
    {
        //左の入力を検知
        if (Input.GetButtonDown("Horizontal_Left"))
        {
            //ここにアイテムフィルターを変更する処理を書く
            _currentItemFilter = _currentItemFilter - 1;
            if (_currentItemFilter < ItemFilter.HEAL)
            {
                _currentItemFilter = ItemFilter.ALL;
            }
        }
    }

    /// <summary> アイテムを種類別で表示する。そのフィルターに係るアイテムボタンのみアクティブにする。それ以外は非アクティブにする。 </summary>
    void Update_ItemFilter()
    {
        //最初の要素を頭にする
        bool firstObj = true;

        //前と今のフィルターが違えば更新する
        if (_beforeItemFilter != _currentItemFilter)
        {
            //一時リストを作成 : ボタンの偏移先を設定するよう。
            List<GameObject> temporaryList = new List<GameObject>();

            //全て表示する場合の処理
            if (_currentItemFilter == ItemFilter.ALL)
            {
                foreach (var item in _items)
                { 
                    //所持数が0ならスキップする
                    if (PlayerManager.Instance.ItemVolume._itemNumberOfPossessions[(int)item.GetComponent<ItemButton>().MyItem._myID] == 0)
                    {
                        item.SetActive(false);
                        continue;
                    }
                    //頭の要素をSelectedGameObjectに指定する。
                    if (firstObj)
                    {
                        firstObj = false;
                        _eventSystem.SetSelectedGameObject(item);
                    }
                    item.SetActive(true);
                    temporaryList.Add(item);
                }
            }

            //一部だけ表示する場合(フィルターにかけるときの処理)
            else
            {
                foreach (var item in _items)
                {
                    if ((int)item.GetComponent<ItemButton>().MyItem._myType == (int)_currentItemFilter)
                    {
                        //所持数が0ならスキップする
                        if (PlayerManager.Instance.ItemVolume._itemNumberOfPossessions[(int)item.GetComponent<ItemButton>().MyItem._myID] == 0) 
                        {
                            item.SetActive(false);
                            continue;
                        }
                        //頭の要素をSelectedGameObjectに指定する。
                        if (firstObj)
                        {
                            firstObj = false;
                            _eventSystem.SetSelectedGameObject(item);
                        }
                        item.SetActive(true);
                        temporaryList.Add(item);
                    }
                    else
                    {
                        item.SetActive(false);
                    }
                }
            }
            //ボタンの設定を更新する
             Set_ItemButtonShiftDestination(_currentItemFilter, temporaryList);

            //ボタンの色を更新する
            Change_FilterButtonColor();
        }
        _beforeItemFilter = _currentItemFilter;
    }

    /// <summary> アイテムボタンの偏移先を決める処理 </summary>
    /// <param name="currentButton"> 設定するボタン </param>
    /// <param name="up">    上に設定するボタン </param>
    /// <param name="down">  下に設定するボタン </param>
    /// <param name="left">  左に設定するボタン </param>
    /// <param name="right"> 右に設定するボタン </param>
    void Set_ItemButtonShiftDestinationHelper(Button currentButton, Button up, Button down, Button left, Button right)
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


    GameObject leftButton;
    GameObject rightButton;
    /// <summary> アイテムボタンの偏移先を決める処理。 : 要素が0または1個の時の処理を考える必要有り。 </summary>
    void Set_ItemButtonShiftDestination(ItemFilter _currentItemFilter, List<GameObject> itemButton)
    {
        if (itemButton.Count <= 1) { return; }
        if (itemButton[0] != null)
        {
            //フィルター(横)の遷移先を設定する
            switch (_currentItemFilter)
            {
                case ItemFilter.ALL: leftButton = _keyFilterButton; rightButton = _healFilterButton; break;
                case ItemFilter.HEAL: leftButton = _allFilterButton; rightButton = _powerUpFilterButton; break;
                case ItemFilter.POWER_UP: leftButton = _healFilterButton; rightButton = _minusItemFilterButton; break;
                case ItemFilter.MINUS_ITEM: leftButton = _powerUpFilterButton; rightButton = _keyFilterButton; break;
                case ItemFilter.KEY: leftButton = _minusItemFilterButton; rightButton = _allFilterButton; break;
            }

            //縦の偏移先を設定する
            //先頭を設定
            Set_ItemButtonShiftDestinationHelper(
                itemButton[0].GetComponent<Button>(),
                itemButton[itemButton.Count - 1].GetComponent<Button>(),
                itemButton[1].GetComponent<Button>(),
                leftButton.GetComponent<Button>(),
                rightButton.GetComponent<Button>());

            //中を設定
            for (int i = 1; i < itemButton.Count - 1; i++)
            {
                Set_ItemButtonShiftDestinationHelper(
                itemButton[i].GetComponent<Button>(),
                itemButton[i - 1].GetComponent<Button>(),
                itemButton[i + 1].GetComponent<Button>(),
                leftButton.GetComponent<Button>(),
                rightButton.GetComponent<Button>());
            }

            //末尾を設定
            Set_ItemButtonShiftDestinationHelper(
                itemButton[itemButton.Count - 1].GetComponent<Button>(),
                itemButton[itemButton.Count - 2].GetComponent<Button>(),
                itemButton[0].GetComponent<Button>(),
                leftButton.GetComponent<Button>(),
                rightButton.GetComponent<Button>());
        }
    }
    //オーバーロードする(行数は多くなるが、ToListするより実行効率は良いかと)
    void Set_ItemButtonShiftDestination(ItemFilter _currentItemFilter, GameObject[] itemButton)
    {
        if (itemButton.Length <= 1) { return; }
        if (itemButton[0] != null)
        {
            //フィルター(横)の遷移先を設定する
            switch (_currentItemFilter)
            {
                case ItemFilter.ALL: leftButton = _keyFilterButton; rightButton = _healFilterButton; break;
                case ItemFilter.HEAL: leftButton = _allFilterButton; rightButton = _powerUpFilterButton; break;
                case ItemFilter.POWER_UP: leftButton = _healFilterButton; rightButton = _minusItemFilterButton; break;
                case ItemFilter.MINUS_ITEM: leftButton = _powerUpFilterButton; rightButton = _keyFilterButton; break;
                case ItemFilter.KEY: leftButton = _minusItemFilterButton; rightButton = _allFilterButton; break;
            }

            //縦の偏移先を設定する
            //先頭を設定
            Set_ItemButtonShiftDestinationHelper(
                itemButton[0].GetComponent<Button>(),
                itemButton[itemButton.Length - 1].GetComponent<Button>(),
                itemButton[1].GetComponent<Button>(),
                leftButton.GetComponent<Button>(),
                rightButton.GetComponent<Button>());

            //中を設定
            for (int i = 1; i < itemButton.Length - 1; i++)
            {
                Set_ItemButtonShiftDestinationHelper(
                itemButton[i].GetComponent<Button>(),
                itemButton[i - 1].GetComponent<Button>(),
                itemButton[i + 1].GetComponent<Button>(),
                leftButton.GetComponent<Button>(),
                rightButton.GetComponent<Button>());
            }

            //末尾を設定
            Set_ItemButtonShiftDestinationHelper(
                itemButton[itemButton.Length - 1].GetComponent<Button>(),
                itemButton[itemButton.Length - 2].GetComponent<Button>(),
                itemButton[0].GetComponent<Button>(),
                leftButton.GetComponent<Button>(),
                rightButton.GetComponent<Button>());
        }
    }

    /// <summary> 現在のフィルターの色を変える。 </summary>
    void Change_FilterButtonColor()
    {
        _allFilterButton.GetComponent<Image>().color = Color.white;
        _healFilterButton.GetComponent<Image>().color = Color.white;
        _powerUpFilterButton.GetComponent<Image>().color = Color.white;
        _minusItemFilterButton.GetComponent<Image>().color = Color.white;
        _keyFilterButton.GetComponent<Image>().color = Color.white;

        switch (_currentItemFilter)
        {
            case ItemFilter.ALL: _allFilterButton.GetComponent<Image>().color = Color.cyan; break;
            case ItemFilter.HEAL: _healFilterButton.GetComponent<Image>().color = Color.cyan; break;
            case ItemFilter.POWER_UP: _powerUpFilterButton.GetComponent<Image>().color = Color.cyan; break;
            case ItemFilter.MINUS_ITEM: _minusItemFilterButton.GetComponent<Image>().color = Color.cyan; break;
            case ItemFilter.KEY: _keyFilterButton.GetComponent<Image>().color = Color.cyan; break;
        }
    }

    public void Set_CurrentFilter(ItemFilter itemFilter)
    {
        _currentItemFilter = itemFilter;
    }
}
