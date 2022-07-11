using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NewItemMenuWindowManager : MonoBehaviour
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
    /// <summary> このクラスを初期化したかどうか </summary>
    bool _whetherInitialized;


    //=========配列類=========//
    /// <summary> アイテムボタンのゲームオブジェクトの配列 </summary>
    GameObject[] _itemButtons = new GameObject[(int)Item.ItemID.ITEM_ID_END];
    /// <summary> アイテムアイコンのイメージ </summary>
    Sprite[] _sprites = new Sprite[(int)Item.ItemID.ITEM_ID_END];

    //=====アサインすべきオブジェクト=====//
    //=====フィルターのボタン類=====//
    [Header("フィルターボタン"), SerializeField] GameObject[] _filters;
    //=====各ボタンの親となるコンテント=====//
    [SerializeField] GameObject _itemContent_ALL;
    [SerializeField] GameObject _itemContent_Heal;
    [SerializeField] GameObject _itemContent_PowerUp;
    [SerializeField] GameObject _itemContent_Minus;
    [SerializeField] GameObject _itemContent_Key;
    //=====コンテントの子となるボタン達=====//
    ItemButton[] _contentALLChildren;
    ItemButton[] _contentHealChildren;
    ItemButton[] _contentPowerUpChildren;
    ItemButton[] _contentMinusChildren;
    ItemButton[] _contentKeyChildren;
    /// <summary> アイテムボタンのプレハブ </summary>
    [Header("アイテムボタンプレハブ"), SerializeField] GameObject _itemButtonPrefab;
    /// <summary> 説明文のテキスト </summary>
    [SerializeField] Text _ItemExplanatoryText;
    /// <summary> アイコンのイメージ </summary>
    [SerializeField] Image _itemIconImage;
    /// <summary> イベントシステム </summary>
    [SerializeField] EventSystem _eventSystem;

    //=====インスペクタから設定すべき値=====//
    [Header("アイテムアイコンが格納されたフォルダのパス:resource以下名"), SerializeField] string _folderPath;

    //=====仮の入れ物=====//
    GameObject temporaryObject;

    //初期化処理
    void Start()
    {
        //このクラスを初期化する。
        _whetherInitialized = Initialize_ThisClass();
        ItemButton[][] obj = { _contentALLChildren, _contentHealChildren, _contentPowerUpChildren, _contentMinusChildren, _contentKeyChildren };
        SetALL_ItemButtonShiftDestination(_filters, obj);
    }

    void Update()
    {

    }

    /// <summary> 道具画面がアクティブになった時の処理 </summary>
    private void OnEnable()
    {

    }

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
        int itemIndex = 0;
        for (int filterIndex = 0; filterIndex < filters.Length - 1; filterIndex++)
        {
            int itemIndexIndex = 0;
            foreach (var item in itemButton[itemIndex])
            {
                int fiL = LargeOrSmall(filterIndex - 1, filters.Length - 1, 0);
                int fiR = LargeOrSmall(filterIndex + 1, filters.Length - 1, 0);
                int iiU = LargeOrSmall(itemIndexIndex - 1, itemButton[itemIndex].Length - 1, 0);
                int iiD = LargeOrSmall(itemIndexIndex + 1, itemButton[itemIndex].Length - 1, 0);

                Set_ItemButtonShiftDestinationHelper(
                    itemButton[filterIndex][itemIndexIndex], //カレント
                    itemButton[filterIndex][iiU],       //Up
                    itemButton[filterIndex][iiD],       //Down
                    filters[fiL],                       //Left
                    filters[fiR]);                      //Right

                Debug.Log(iiD);

                itemIndexIndex++;
            }
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

    //==========このクラスの初期化関連==========//
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

        //各ボタンの偏移先を設定するコードをここに書く。

        return true;
    }
    /// <summary> nullチェック </summary>
    bool CheckNull()
    {
        if (_ItemExplanatoryText == null) { Debug.LogError("説明文のテキストをアサインしてください"); return false; }
        if (_eventSystem == null) { Debug.LogError("EventSystemをアサインしてください"); return false; }
        if (_itemButtonPrefab == null) { Debug.LogError("アイテムボタンのプレハブがアサインされていません。"); return false; }

        if (_filters[(int)ItemFilter.ALL] == null) { Debug.LogError("アイテムフィルターボタンの取得に失敗しました。アサインしてください。: ALL Button"); return false; }
        if (_filters[(int)ItemFilter.HEAL] == null) { Debug.LogError("アイテムフィルターボタンの取得に失敗しました。アサインしてください。: HEAL Button"); return false; }
        if (_filters[(int)ItemFilter.POWER_UP] == null) { Debug.LogError("アイテムフィルターボタンの取得に失敗しました。アサインしてください。: POWER_UP Button"); return false; }
        if (_filters[(int)ItemFilter.MINUS_ITEM] == null) { Debug.LogError("アイテムフィルターボタンの取得に失敗しました。アサインしてください。: MINUS_ITEM Button"); return false; }
        if (_filters[(int)ItemFilter.KEY] == null) { Debug.LogError("アイテムフィルターボタンの取得に失敗しました。アサインしてください。: KEY Button"); return false; }

        if (_itemContent_ALL == null) { Debug.LogError("アイテムコンテントALLをアサインしてください。"); return false; }
        if (_itemContent_Heal == null) { Debug.LogError("アイテムコンテントHealをアサインしてください。"); return false; }
        if (_itemContent_PowerUp == null) { Debug.LogError("アイテムコンテントPowerUpをアサインしてください。"); return false; }
        if (_itemContent_Minus == null) { Debug.LogError("アイテムコンテントMinusをアサインしてください。"); return false; }
        if (_itemContent_Key == null) { Debug.LogError("アイテムコンテントKeyをアサインしてください。"); return false; }

        return true;
    }
    /// <summary> フィルターボタンに情報を設定する。 </summary>
    void Set_FilterButton()
    {
        _filters[(int)ItemFilter.ALL].GetComponent<ItemFilterButton>().Set_ItemFilter(ItemMenuWindowManager.ItemFilter.ALL);
        _filters[(int)ItemFilter.HEAL].GetComponent<ItemFilterButton>().Set_ItemFilter(ItemMenuWindowManager.ItemFilter.HEAL);
        _filters[(int)ItemFilter.POWER_UP].GetComponent<ItemFilterButton>().Set_ItemFilter(ItemMenuWindowManager.ItemFilter.POWER_UP);
        _filters[(int)ItemFilter.MINUS_ITEM].GetComponent<ItemFilterButton>().Set_ItemFilter(ItemMenuWindowManager.ItemFilter.MINUS_ITEM);
        _filters[(int)ItemFilter.KEY].GetComponent<ItemFilterButton>().Set_ItemFilter(ItemMenuWindowManager.ItemFilter.KEY);
    }
    /// <summary> アイテムボタンに情報を設定する。 </summary>
    void Set_ItemButton()
    {
        //アイテムボタンをScrollViewの、Contentの子としてインスタンシエイトしておき、データをセットする。
        for (int i = 0; i < (int)Item.ItemID.ITEM_ID_END; i++)
        {
            //ALLコンテントの子としてインスタンシエイトする。
            _itemButtons[i] = Instantiate(_itemButtonPrefab, Vector3.zero, Quaternion.identity, _itemContent_ALL.transform);
            //データをセット
            _itemButtons[i].GetComponent<ItemButton>().SetItemData(GameManager.Instance.ItemData[i]);

            //各コンテントに子としてインスタンシエイトする。
            switch (_itemButtons[i].GetComponent<ItemButton>().MyItem._myType)
            {
                case Item.ItemType.HEAL: temporaryObject = Instantiate(_itemButtonPrefab, Vector3.zero, Quaternion.identity, _itemContent_Heal.transform); break;
                case Item.ItemType.POWER_UP: temporaryObject = Instantiate(_itemButtonPrefab, Vector3.zero, Quaternion.identity, _itemContent_PowerUp.transform); break;
                case Item.ItemType.MINUS_ITEM: temporaryObject = Instantiate(_itemButtonPrefab, Vector3.zero, Quaternion.identity, _itemContent_Minus.transform); break;
                case Item.ItemType.KEY: temporaryObject = Instantiate(_itemButtonPrefab, Vector3.zero, Quaternion.identity, _itemContent_Key.transform); break;
            }
            //データをセット
            temporaryObject.GetComponent<ItemButton>().SetItemData(GameManager.Instance.ItemData[i]);
        }
    }
    /// <summary> コンテントの子を取得し、変数に保存する。 </summary>
    void Set_ContentChildren()
    {
        _contentALLChildren = _itemContent_ALL.GetComponentsInChildren<ItemButton>();
        _contentHealChildren = _itemContent_Heal.GetComponentsInChildren<ItemButton>();
        _contentPowerUpChildren = _itemContent_PowerUp.GetComponentsInChildren<ItemButton>();
        _contentMinusChildren = _itemContent_Minus.GetComponentsInChildren<ItemButton>();
        _contentKeyChildren = _itemContent_Key.GetComponentsInChildren<ItemButton>();
    }
}
