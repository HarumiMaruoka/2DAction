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
    /// <summary> �R���e���g�̎q�̔z�� </summary>
    ItemButton[][] _contentChildren;
    //=====�R���e���g�̎q�ƂȂ�{�^���B=====//
    ItemButton[] _contentALLChildren;
    ItemButton[] _contentHealChildren;
    ItemButton[] _contentPowerUpChildren;
    ItemButton[] _contentMinusChildren;
    ItemButton[] _contentKeyChildren;


    //=========�z���=========//
    /// <summary> �A�C�e���{�^���̃Q�[���I�u�W�F�N�g�̔z�� </summary>
    GameObject[] _itemButtons = new GameObject[(int)Item.ItemID.ITEM_ID_END];
    /// <summary> �A�C�e���A�C�R���̃C���[�W </summary>
    Sprite[] _sprites = new Sprite[(int)Item.ItemID.ITEM_ID_END];

    //=====�A�T�C�����ׂ��I�u�W�F�N�g=====//
    //=====�t�B���^�[�̃{�^����=====//
    [Header("�t�B���^�[�{�^��"), SerializeField] GameObject[] _filters;
    //=====�e�{�^���̐e�ƂȂ�R���e���g=====//
    [Header("�R���e���g�̔z��"), SerializeField] GameObject[] _contents;
    /// <summary> �A�C�e���{�^���̃v���n�u </summary>
    [Header("�A�C�e���{�^���v���n�u"), SerializeField] GameObject _itemButtonPrefab;
    /// <summary> �������̃e�L�X�g </summary>
    [SerializeField] Text _ItemExplanatoryText;
    /// <summary> �A�C�R���̃C���[�W </summary>
    [SerializeField] Image _itemIconImage;
    /// <summary> �C�x���g�V�X�e�� </summary>
    [SerializeField] EventSystem _eventSystem;
    /// <summary>  </summary>
    [SerializeField] ScrollRect _contentParent;

    //=====�C���X�y�N�^����ݒ肷�ׂ��l=====//
    [Header("�A�C�e���A�C�R�����i�[���ꂽ�t�H���_�̃p�X:resource�ȉ���"), SerializeField] string _folderPath;
    [Header("�t�B���^�[�{�^���̒ʏ�F"), SerializeField] Color _filterButton_NomalColor;
    [Header("�t�B���^�[�{�^���̑I�����̐F"), SerializeField] Color _filterButton_SelectedColor;

    //=====���̓��ꕨ=====//
    GameObject temporaryObject;

    //����������
    void Start()
    {
        //���̃N���X������������B
        _whetherInitialized = Initialize_ThisClass();
    }

    void Update()
    {
        if (_whetherInitialized)
        {
            Update_Filter();
        }
    }

    /// <summary> �����ʂ��A�N�e�B�u�ɂȂ������̏��� </summary>
    private void OnEnable()
    {

    }

    //=========�t�B���^�[�ύX�Ɋւ��鏈��=========//
    /// <summary> �t�B���^�[�ύX���� </summary>
    void Update_Filter()
    {
        //�t�B���^�[�����ɃV�t�g����
        if (Input.GetButtonDown("Horizontal_Left"))
        {
            _currentItemFilter--;
            if (_currentItemFilter < 0)
            {
                _currentItemFilter = (ItemFilter)_filters.Length - 1;
            }
        }
        //�t�B���^�[���E�ɃV�t�g����
        if (Input.GetButtonDown("Horizontal_Right"))
        {
            _currentItemFilter++;
            if (_currentItemFilter > (ItemFilter)_filters.Length - 1)
            {
                _currentItemFilter = 0;
            }
        }
        if (_beforeItemFilter != _currentItemFilter)
        {
            //=====�t�B���^�[�̍X�V����=====//
            //�R���e���g�����ւ���
            _contentParent.content = _contents[(int)_currentItemFilter].GetComponent<RectTransform>();
            //�Â��R���e���g���A�N�e�B�u�ɂ��āA�V�����R���e���g���A�N�e�B�u�ɂ���B
            _contents[(int)_currentItemFilter].SetActive(true);
            _contents[(int)_beforeItemFilter].SetActive(false);
            //�I�𒆂̃I�u�W�F�N�g��ύX����
            _eventSystem.SetSelectedGameObject(_contentChildren[(int)_currentItemFilter][0].gameObject);
            //�{�^���̐F��ς���
            _filters[(int)_beforeItemFilter].GetComponent<Image>().color = _filterButton_NomalColor;
            _filters[(int)_currentItemFilter].GetComponent<Image>().color = _filterButton_SelectedColor;
        }
        _beforeItemFilter = _currentItemFilter;
    }

    //======�A�C�e���̕Έڐ��ݒ肷��֘A======//
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
        //�O���̃A�C�e���̃C���f�b�N�X
        int itemIndex = 0;
        //�O���̃��[�v : �t�B���^�[���Ƃ̕Έڐ��ݒ肷��B
        for (int filterIndex = 0; filterIndex < filters.Length; filterIndex++)
        {
            //�����̃A�C�e���̃C���f�b�N�X
            int itemIndexIndex = 0;
            //�����̃��[�v : �A�C�e�����Ƃ̕Έڐ��ݒ肷��B
            foreach (var item in itemButton[itemIndex])
            {
                //���E�㉺�̃{�^�����擾�B
                int fiL = LargeOrSmall(filterIndex - 1, filters.Length - 1, 0);
                int fiR = LargeOrSmall(filterIndex + 1, filters.Length - 1, 0);
                int iiU = LargeOrSmall(itemIndexIndex - 1, itemButton[itemIndex].Length - 1, 0);
                int iiD = LargeOrSmall(itemIndexIndex + 1, itemButton[itemIndex].Length - 1, 0);

                //�Έڐ��ݒ�B
                Set_ItemButtonShiftDestinationHelper(
                    itemButton[filterIndex][itemIndexIndex], //�J�����g
                    itemButton[filterIndex][iiU],       //Up
                    itemButton[filterIndex][iiD],       //Down
                    filters[fiL],                       //Left
                    filters[fiR]);                      //Right
                //�C���f�b�N�X���X�V
                itemIndexIndex++;
            }
            //�C���f�b�N�X���X�V
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

        //�R���e���g�̕Έڐ��ݒ肷��B
        _contentChildren = new ItemButton[][] { _contentALLChildren, _contentHealChildren, _contentPowerUpChildren, _contentMinusChildren, _contentKeyChildren };
        SetALL_ItemButtonShiftDestination(_filters, _contentChildren);

        //�t�B���^�[�{�^���̐F��ύX����
        _filters[(int)_currentItemFilter].GetComponent<Image>().color = _filterButton_SelectedColor;

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

        if (_contents[(int)ItemFilter.ALL] == null) { Debug.LogError("�A�C�e���R���e���gALL���A�T�C�����Ă��������B"); return false; }
        if (_contents[(int)ItemFilter.HEAL] == null) { Debug.LogError("�A�C�e���R���e���gHeal���A�T�C�����Ă��������B"); return false; }
        if (_contents[(int)ItemFilter.POWER_UP] == null) { Debug.LogError("�A�C�e���R���e���gPowerUp���A�T�C�����Ă��������B"); return false; }
        if (_contents[(int)ItemFilter.MINUS_ITEM] == null) { Debug.LogError("�A�C�e���R���e���gMinus���A�T�C�����Ă��������B"); return false; }
        if (_contents[(int)ItemFilter.KEY] == null) { Debug.LogError("�A�C�e���R���e���gKey���A�T�C�����Ă��������B"); return false; }

        if (_contentParent == null) { Debug.LogError("ScrollView��ScrollRect���A�T�C�����Ă��������B"); return false; }

        return true;
    }
    /// <summary> �t�B���^�[�{�^���ɏ���ݒ肷��B </summary>
    void Set_FilterButton()
    {
        _filters[(int)ItemFilter.ALL].GetComponent<ItemFilterButton>().Set_ItemFilter(ItemFilter.ALL);
        _filters[(int)ItemFilter.HEAL].GetComponent<ItemFilterButton>().Set_ItemFilter(ItemFilter.HEAL);
        _filters[(int)ItemFilter.POWER_UP].GetComponent<ItemFilterButton>().Set_ItemFilter(ItemFilter.POWER_UP);
        _filters[(int)ItemFilter.MINUS_ITEM].GetComponent<ItemFilterButton>().Set_ItemFilter(ItemFilter.MINUS_ITEM);
        _filters[(int)ItemFilter.KEY].GetComponent<ItemFilterButton>().Set_ItemFilter(ItemFilter.KEY);
    }
    /// <summary> �A�C�e���{�^���ɏ���ݒ肷��B </summary>
    void Set_ItemButton()
    {
        //�A�C�e���{�^����ScrollView�́AContent�̎q�Ƃ��ăC���X�^���V�G�C�g���Ă����A�f�[�^���Z�b�g����B
        for (int i = 0; i < (int)Item.ItemID.ITEM_ID_END; i++)
        {
            //ALL�R���e���g�̎q�Ƃ��ăC���X�^���V�G�C�g����B
            _itemButtons[i] = Instantiate(_itemButtonPrefab, Vector3.zero, Quaternion.identity, _contents[(int)ItemFilter.ALL].transform);
            //�f�[�^���Z�b�g
            _itemButtons[i].GetComponent<ItemButton>().SetItemData(GameManager.Instance.ItemData[i]);

            //�e�R���e���g�Ɏq�Ƃ��ăC���X�^���V�G�C�g����B
            switch (_itemButtons[i].GetComponent<ItemButton>().MyItem._myType)
            {
                case Item.ItemType.HEAL: temporaryObject = Instantiate(_itemButtonPrefab, Vector3.zero, Quaternion.identity, _contents[(int)ItemFilter.HEAL].transform); break;
                case Item.ItemType.POWER_UP: temporaryObject = Instantiate(_itemButtonPrefab, Vector3.zero, Quaternion.identity, _contents[(int)ItemFilter.POWER_UP].transform); break;
                case Item.ItemType.MINUS_ITEM: temporaryObject = Instantiate(_itemButtonPrefab, Vector3.zero, Quaternion.identity, _contents[(int)ItemFilter.MINUS_ITEM].transform); break;
                case Item.ItemType.KEY: temporaryObject = Instantiate(_itemButtonPrefab, Vector3.zero, Quaternion.identity, _contents[(int)ItemFilter.KEY].transform); break;
            }
            //�f�[�^���Z�b�g
            temporaryObject.GetComponent<ItemButton>().SetItemData(GameManager.Instance.ItemData[i]);
        }
    }
    /// <summary> �R���e���g�̎q���擾���A�ϐ��ɕۑ�����B </summary>
    void Set_ContentChildren()
    {
        _contentALLChildren = _contents[(int)ItemFilter.ALL].GetComponentsInChildren<ItemButton>();
        _contentHealChildren = _contents[(int)ItemFilter.HEAL].GetComponentsInChildren<ItemButton>();
        _contentPowerUpChildren = _contents[(int)ItemFilter.POWER_UP].GetComponentsInChildren<ItemButton>();
        _contentMinusChildren = _contents[(int)ItemFilter.MINUS_ITEM].GetComponentsInChildren<ItemButton>();
        _contentKeyChildren = _contents[(int)ItemFilter.KEY].GetComponentsInChildren<ItemButton>();
    }

    public void Set_CurrentFillter(ItemFilter itemFilter)
    {
        _currentItemFilter = itemFilter;
    }
}
