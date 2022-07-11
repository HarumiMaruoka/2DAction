using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NewItemMenuWindowManager : MonoBehaviour
{
    //=====�\������A�C�e���̃t�B���^�[��enum=====//
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

    /////<=======���̃N���X�Ŏg�p����ϐ�=======>/////
    //=====�A�C�e���t�B���^�[=====//
    /// <summary> �O�t���[���I�����Ă����A�C�e���t�B���^�[ </summary>
    ItemFilter _beforeItemFilter;
    /// <summary> ���݂̃t���[���őI�����Ă���A�C�e���t�B���^�[ </summary>
    ItemFilter _currentItemFilter = ItemFilter.ALL;
    /// <summary> ���̃N���X���������������ǂ��� </summary>
    bool _whetherInitialized;


    //=========�z���=========//
    /// <summary> �A�C�e���{�^���̃Q�[���I�u�W�F�N�g�̔z�� </summary>
    GameObject[] _itemButtons = new GameObject[(int)Item.ItemID.ITEM_ID_END];
    /// <summary> �A�C�e���A�C�R���̃C���[�W </summary>
    Sprite[] _sprites = new Sprite[(int)Item.ItemID.ITEM_ID_END];

    //=====�A�T�C�����ׂ��I�u�W�F�N�g=====//
    //=====�t�B���^�[�̃{�^����=====//
    [Header("�t�B���^�[�{�^��"), SerializeField] GameObject[] _filters;
    //=====�e�{�^���̐e�ƂȂ�R���e���g=====//
    [SerializeField] GameObject _itemContent_ALL;
    [SerializeField] GameObject _itemContent_Heal;
    [SerializeField] GameObject _itemContent_PowerUp;
    [SerializeField] GameObject _itemContent_Minus;
    [SerializeField] GameObject _itemContent_Key;
    //=====�R���e���g�̎q�ƂȂ�{�^���B=====//
    ItemButton[] _contentALLChildren;
    ItemButton[] _contentHealChildren;
    ItemButton[] _contentPowerUpChildren;
    ItemButton[] _contentMinusChildren;
    ItemButton[] _contentKeyChildren;
    /// <summary> �A�C�e���{�^���̃v���n�u </summary>
    [Header("�A�C�e���{�^���v���n�u"), SerializeField] GameObject _itemButtonPrefab;
    /// <summary> �������̃e�L�X�g </summary>
    [SerializeField] Text _ItemExplanatoryText;
    /// <summary> �A�C�R���̃C���[�W </summary>
    [SerializeField] Image _itemIconImage;
    /// <summary> �C�x���g�V�X�e�� </summary>
    [SerializeField] EventSystem _eventSystem;

    //=====�C���X�y�N�^����ݒ肷�ׂ��l=====//
    [Header("�A�C�e���A�C�R�����i�[���ꂽ�t�H���_�̃p�X:resource�ȉ���"), SerializeField] string _folderPath;

    //=====���̓��ꕨ=====//
    GameObject temporaryObject;

    //����������
    void Start()
    {
        //���̃N���X������������B
        _whetherInitialized = Initialize_ThisClass();
        ItemButton[][] obj = { _contentALLChildren, _contentHealChildren, _contentPowerUpChildren, _contentMinusChildren, _contentKeyChildren };
        SetALL_ItemButtonShiftDestination(_filters, obj);
    }

    void Update()
    {

    }

    /// <summary> �����ʂ��A�N�e�B�u�ɂȂ������̏��� </summary>
    private void OnEnable()
    {

    }

    /// <summary> �A�C�e���{�^���̕Έڐ��ݒ肷��B </summary>
    /// <param name="currentButton"> �ݒ肷��{�^�� </param>
    /// <param name="up">    ��ɐݒ肷��{�^�� </param>
    /// <param name="down">  ���ɐݒ肷��{�^�� </param>
    /// <param name="left">  ���ɐݒ肷��{�^�� </param>
    /// <param name="right"> �E�ɐݒ肷��{�^�� </param>
    void Set_ItemButtonShiftDestinationHelper(ItemButton currentButton, ItemButton up, ItemButton down, GameObject left, GameObject right)
    {
        //�i�r�Q�[�V�������擾
        Navigation navigation = currentButton.GetComponent<Button>().navigation;
        //���[�h��ύX
        navigation.mode = Navigation.Mode.Explicit;
        //�Έڐ���w��
        navigation.selectOnUp = up.GetComponent<Button>();
        navigation.selectOnDown = down.GetComponent<Button>();
        navigation.selectOnLeft = left.GetComponent<Button>();
        navigation.selectOnRight = right.GetComponent<Button>();
        //�i�r�Q�[�V�������Z�b�g
        currentButton.GetComponent<Button>().navigation = navigation;
    }
    /// <summary> �e�{�^���̕Έڐ��ݒ肷��B </summary>
    /// <param name="filters"> �t�B���^�[�{�^���̔z�� </param>
    /// <param name="itemButton"> �A�C�e���{�^���̔z�� </param>
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
                    itemButton[filterIndex][itemIndexIndex], //�J�����g
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

    /// <summary> �󂯎�����l�̑召���r���A�ő�l���傫����΍ŏ��l��Ԃ��A�ŏ��l��菬������΍ő�l��Ԃ��B </summary>
    /// <param name="value"> ��r���ׂ��l </param>
    /// <param name="maxValue"> �ő�l </param>
    /// <param name="minValue"> �ŏ��l </param>
    /// <returns> value�̒l���r���Amin�l��菬�������max�l��Ԃ��Amax���傫�����min�l��Ԃ��B </returns>
    int LargeOrSmall(int value, int maxValue, int minValue)
    {
        //�ő�l�𒴂��Ă���΍ŏ��l��Ԃ��B
        if (value > maxValue) return minValue;
        //�ŏ��l��������Ă���΍ő�l��Ԃ��B
        else if (value < minValue) return maxValue;
        //�ǂ���ł��Ȃ���΂��̂܂ܕԂ��B
        else return value;
    }

    //==========���̃N���X�̏������֘A==========//
    /// <summary> ���̃N���X������������B </summary>
    /// <returns>����������true��Ԃ��B</returns>
    bool Initialize_ThisClass()
    {
        //null�`�F�b�N
        if (!CheckNull()) return false;
        //�t�B���^�[�{�^���ɒl���Z�b�g����B
        Set_FilterButton();
        //�A�C�e���{�^���ɏ����Z�b�g����B
        Set_ItemButton();
        //�A�C�e���̃A�C�R���摜��ݒ肷��B
        _sprites = Resources.LoadAll<Sprite>(_folderPath);
        //�Z���N�e�b�h�I�u�W�F�N�g���w�肷��B
        _eventSystem.SetSelectedGameObject(_itemButtons[0]);
        //�R���e���g�̎q���擾����
        Set_ContentChildren();

        //�e�{�^���̕Έڐ��ݒ肷��R�[�h�������ɏ����B

        return true;
    }
    /// <summary> null�`�F�b�N </summary>
    bool CheckNull()
    {
        if (_ItemExplanatoryText == null) { Debug.LogError("�������̃e�L�X�g���A�T�C�����Ă�������"); return false; }
        if (_eventSystem == null) { Debug.LogError("EventSystem���A�T�C�����Ă�������"); return false; }
        if (_itemButtonPrefab == null) { Debug.LogError("�A�C�e���{�^���̃v���n�u���A�T�C������Ă��܂���B"); return false; }

        if (_filters[(int)ItemFilter.ALL] == null) { Debug.LogError("�A�C�e���t�B���^�[�{�^���̎擾�Ɏ��s���܂����B�A�T�C�����Ă��������B: ALL Button"); return false; }
        if (_filters[(int)ItemFilter.HEAL] == null) { Debug.LogError("�A�C�e���t�B���^�[�{�^���̎擾�Ɏ��s���܂����B�A�T�C�����Ă��������B: HEAL Button"); return false; }
        if (_filters[(int)ItemFilter.POWER_UP] == null) { Debug.LogError("�A�C�e���t�B���^�[�{�^���̎擾�Ɏ��s���܂����B�A�T�C�����Ă��������B: POWER_UP Button"); return false; }
        if (_filters[(int)ItemFilter.MINUS_ITEM] == null) { Debug.LogError("�A�C�e���t�B���^�[�{�^���̎擾�Ɏ��s���܂����B�A�T�C�����Ă��������B: MINUS_ITEM Button"); return false; }
        if (_filters[(int)ItemFilter.KEY] == null) { Debug.LogError("�A�C�e���t�B���^�[�{�^���̎擾�Ɏ��s���܂����B�A�T�C�����Ă��������B: KEY Button"); return false; }

        if (_itemContent_ALL == null) { Debug.LogError("�A�C�e���R���e���gALL���A�T�C�����Ă��������B"); return false; }
        if (_itemContent_Heal == null) { Debug.LogError("�A�C�e���R���e���gHeal���A�T�C�����Ă��������B"); return false; }
        if (_itemContent_PowerUp == null) { Debug.LogError("�A�C�e���R���e���gPowerUp���A�T�C�����Ă��������B"); return false; }
        if (_itemContent_Minus == null) { Debug.LogError("�A�C�e���R���e���gMinus���A�T�C�����Ă��������B"); return false; }
        if (_itemContent_Key == null) { Debug.LogError("�A�C�e���R���e���gKey���A�T�C�����Ă��������B"); return false; }

        return true;
    }
    /// <summary> �t�B���^�[�{�^���ɏ���ݒ肷��B </summary>
    void Set_FilterButton()
    {
        _filters[(int)ItemFilter.ALL].GetComponent<ItemFilterButton>().Set_ItemFilter(ItemMenuWindowManager.ItemFilter.ALL);
        _filters[(int)ItemFilter.HEAL].GetComponent<ItemFilterButton>().Set_ItemFilter(ItemMenuWindowManager.ItemFilter.HEAL);
        _filters[(int)ItemFilter.POWER_UP].GetComponent<ItemFilterButton>().Set_ItemFilter(ItemMenuWindowManager.ItemFilter.POWER_UP);
        _filters[(int)ItemFilter.MINUS_ITEM].GetComponent<ItemFilterButton>().Set_ItemFilter(ItemMenuWindowManager.ItemFilter.MINUS_ITEM);
        _filters[(int)ItemFilter.KEY].GetComponent<ItemFilterButton>().Set_ItemFilter(ItemMenuWindowManager.ItemFilter.KEY);
    }
    /// <summary> �A�C�e���{�^���ɏ���ݒ肷��B </summary>
    void Set_ItemButton()
    {
        //�A�C�e���{�^����ScrollView�́AContent�̎q�Ƃ��ăC���X�^���V�G�C�g���Ă����A�f�[�^���Z�b�g����B
        for (int i = 0; i < (int)Item.ItemID.ITEM_ID_END; i++)
        {
            //ALL�R���e���g�̎q�Ƃ��ăC���X�^���V�G�C�g����B
            _itemButtons[i] = Instantiate(_itemButtonPrefab, Vector3.zero, Quaternion.identity, _itemContent_ALL.transform);
            //�f�[�^���Z�b�g
            _itemButtons[i].GetComponent<ItemButton>().SetItemData(GameManager.Instance.ItemData[i]);

            //�e�R���e���g�Ɏq�Ƃ��ăC���X�^���V�G�C�g����B
            switch (_itemButtons[i].GetComponent<ItemButton>().MyItem._myType)
            {
                case Item.ItemType.HEAL: temporaryObject = Instantiate(_itemButtonPrefab, Vector3.zero, Quaternion.identity, _itemContent_Heal.transform); break;
                case Item.ItemType.POWER_UP: temporaryObject = Instantiate(_itemButtonPrefab, Vector3.zero, Quaternion.identity, _itemContent_PowerUp.transform); break;
                case Item.ItemType.MINUS_ITEM: temporaryObject = Instantiate(_itemButtonPrefab, Vector3.zero, Quaternion.identity, _itemContent_Minus.transform); break;
                case Item.ItemType.KEY: temporaryObject = Instantiate(_itemButtonPrefab, Vector3.zero, Quaternion.identity, _itemContent_Key.transform); break;
            }
            //�f�[�^���Z�b�g
            temporaryObject.GetComponent<ItemButton>().SetItemData(GameManager.Instance.ItemData[i]);
        }
    }
    /// <summary> �R���e���g�̎q���擾���A�ϐ��ɕۑ�����B </summary>
    void Set_ContentChildren()
    {
        _contentALLChildren = _itemContent_ALL.GetComponentsInChildren<ItemButton>();
        _contentHealChildren = _itemContent_Heal.GetComponentsInChildren<ItemButton>();
        _contentPowerUpChildren = _itemContent_PowerUp.GetComponentsInChildren<ItemButton>();
        _contentMinusChildren = _itemContent_Minus.GetComponentsInChildren<ItemButton>();
        _contentKeyChildren = _itemContent_Key.GetComponentsInChildren<ItemButton>();
    }
}
