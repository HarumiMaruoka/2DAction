using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary> �A�C�e���E�B���h�E���Ǘ�����N���X </summary>
public class ItemMenuWindowManager : MonoBehaviour
{
    [System.Serializable]
    //�\������A�C�e���̃t�B���^�[
    public enum ItemFilter
    {
        HEAL,
        POWER_UP,
        MINUS_ITEM,
        KEY,

        ALL,

        ITEM_FILTER_END
    }

    //���̃N���X�Ŏg�p����ϐ�

    //�A�C�e���t�B���^�[
    /// <summary> �O�t���[���I�����Ă����A�C�e���t�B���^�[ </summary>
    ItemFilter _beforeItemFilter;
    /// <summary> ���݂̃t���[���őI�����Ă���A�C�e���t�B���^�[ </summary>
    ItemFilter _currentItemFilter;

    //�I�����Ă���A�C�e����ID
    /// <summary> �O�t���[���ɑI�����Ă����A�C�e����ID(index) </summary>
    int _beforeItemIndex;
    /// <summary> ���ݑI�����Ă���A�C�e����ID(index) </summary>
    int _currentItemIndex;

    //�e�v���n�u
    /// <summary> �A�C�e���{�^���̃v���n�u </summary>
    [Header("�A�C�e���{�^���v���n�u"), SerializeField] GameObject _itemButtonPrefab;


    //�z���
    /// <summary> �A�C�e���{�^���̃Q�[���I�u�W�F�N�g�̔z�� </summary>
    GameObject[] _items = new GameObject[(int)Item.ItemID.ITEM_ID_END];
    /// <summary> �A�C�e���A�C�R���̃C���[�W </summary>
    Sprite[] _sprites = new Sprite[(int)Item.ItemID.ITEM_ID_END];

    //�t�B���^�[�̃{�^����
    GameObject _allFilterButton;
    GameObject _healFilterButton;
    GameObject _powerUpFilterButton;
    GameObject _minusItemFilterButton;
    GameObject _keyFilterButton;

    //�A�T�C�����ׂ��I�u�W�F�N�g
    /// <summary> �������̃e�L�X�g </summary>
    [SerializeField] Text _ItemExplanatoryText;
    /// <summary> �A�C�R���̃C���[�W </summary>
    [SerializeField] Image _itemIconImage;
    /// <summary> �C�x���g�V�X�e�� </summary>
    [SerializeField] EventSystem _eventSystem;

    //�C���X�y�N�^����ݒ肷�ׂ��l
    [Header("�A�C�e���A�C�R�����i�[���ꂽ�t�H���_�̃p�X:resource�ȉ���"), SerializeField] string _folderPath;


    //����������
    void Start()
    {
        //�A�C�e���t�B���^�[
        _currentItemFilter = ItemFilter.ALL;

        if ((_allFilterButton = GameObject.FindGameObjectWithTag("ItemFilterALL")) == null) Debug.LogError("�A�C�e���t�B���^�[�{�^���̎擾�Ɏ��s���܂����B�^�O��ݒ肵�Ă��������B: ALL Button");
        if ((_healFilterButton = GameObject.FindGameObjectWithTag("ItemFilterHEAL")) == null) Debug.LogError("�A�C�e���t�B���^�[�{�^���̎擾�Ɏ��s���܂����B�^�O��ݒ肵�Ă��������B: HEAL Button");
        if ((_powerUpFilterButton = GameObject.FindGameObjectWithTag("ItemFilterPOWER_UP")) == null) Debug.LogError("�A�C�e���t�B���^�[�{�^���̎擾�Ɏ��s���܂����B�^�O��ݒ肵�Ă��������B: POWER_UP Button");
        if ((_minusItemFilterButton = GameObject.FindGameObjectWithTag("ItemFilterMINUS_ITEM")) == null) Debug.LogError("�A�C�e���t�B���^�[�{�^���̎擾�Ɏ��s���܂����B�^�O��ݒ肵�Ă��������B: MINUS_ITEM Button");
        if ((_keyFilterButton = GameObject.FindGameObjectWithTag("ItemFilterKEY")) == null) Debug.LogError("�A�C�e���t�B���^�[�{�^���̎擾�Ɏ��s���܂����B�^�O��ݒ肵�Ă��������B: KEY Button");

        _allFilterButton.GetComponent<ItemFilterButton>().Set_ItemFilter(ItemFilter.ALL);
        _healFilterButton.GetComponent<ItemFilterButton>().Set_ItemFilter(ItemFilter.HEAL);
        _powerUpFilterButton.GetComponent<ItemFilterButton>().Set_ItemFilter(ItemFilter.POWER_UP);
        _minusItemFilterButton.GetComponent<ItemFilterButton>().Set_ItemFilter(ItemFilter.MINUS_ITEM);
        _keyFilterButton.GetComponent<ItemFilterButton>().Set_ItemFilter(ItemFilter.KEY);

        //�A�C�e���̃A�C�R���摜���擾
        _sprites = Resources.LoadAll<Sprite>(_folderPath);

        //null�`�F�b�N
        if (_ItemExplanatoryText == null)
        {
            Debug.LogError("�������̃e�L�X�g���A�T�C�����Ă�������");
        }
        if (_eventSystem == null)
        {
            Debug.LogError("EventSystem���A�T�C�����Ă�������");
        }

        //�A�C�e���{�^�����C���X�^���V�G�C�g���鏀���̏����B
        if (_itemButtonPrefab == null)
        {
            Debug.LogError("�A�C�e���{�^���̃v���n�u���A�T�C������Ă��܂���B");
        }
        GameObject _itemContent = GameObject.FindGameObjectWithTag("ItemDrawContentALL");
        if (_itemContent == null)
        {
            Debug.LogError("ItemDrawContentALL�̃^�O���t�����A�I�u�W�F�N�g�̎擾�Ɏ��s���܂����B");
        }
        //�A�C�e���{�^����ScrollView�́AContent�̎q�Ƃ��ăC���X�^���V�G�C�g���Ă����B
        for (int i = 0; i < (int)Item.ItemID.ITEM_ID_END; i++)
        {
            _items[i] = Instantiate(_itemButtonPrefab, Vector3.zero, Quaternion.identity, _itemContent.transform);

            //ItemData���Z�b�g����
            _items[i].GetComponent<ItemButton>().SetItemData(GameManager.Instance.ItemData[i]);
        }

        _eventSystem.SetSelectedGameObject(_items[0]);


        //�{�^���̃L�[���͂̕Έڐ�̎w�菈���B
        //select on up �� down ��ݒ肷�� �����̂ł��ƂŒ���
        //Navigation navigation = _items[(int)Item.ItemID.ITEM_ID_00].GetComponent<Button>().navigation;
        //navigation.mode = Navigation.Mode.Explicit;
        //navigation.selectOnUp = _items[(int)Item.ItemID.ITEM_ID_END - 1].GetComponent<Button>();
        //navigation.selectOnDown = _items[(int)Item.ItemID.ITEM_ID_01].GetComponent<Button>();
        //_items[(int)Item.ItemID.ITEM_ID_00].GetComponent<Button>().navigation = navigation;
        ////select on up �� down ��ݒ肷��
        //navigation = _items[(int)Item.ItemID.ITEM_ID_END - 1].GetComponent<Button>().navigation;
        //navigation.mode = Navigation.Mode.Explicit;
        //navigation.selectOnDown = _items[(int)Item.ItemID.ITEM_ID_00].GetComponent<Button>();
        //navigation.selectOnUp = _items[(int)Item.ItemID.ITEM_ID_END - 2].GetComponent<Button>();
        //_items[(int)Item.ItemID.ITEM_ID_END - 1].GetComponent<Button>().navigation = navigation;


        //�e�{�^���̕Έڐ��ݒ肷��B�ŏ���ALL�̐ݒ�
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

    /// <summary> �A�C�e���E�B���h�E�̍X�V�֐� </summary>
    void UpdateItemWindow()
    {
        //����
        InputAbove();
        InputBelow();
        InputRight();
        InputLeft();

        //�K�v�ł���΁A�A�C�e���t�B���^�[���X�V����B
        Update_ItemFilter();

        //���ݑI������Ă���{�^�����擾
        GameObject nowSelectItem = EventSystem.current.currentSelectedGameObject;

        //�������Ɖ摜��ݒ�
        if (nowSelectItem != null && nowSelectItem.TryGetComponent<ItemButton>(out ItemButton item))
        {
            _ItemExplanatoryText.text = item.MyItem._myExplanatoryText;
            _itemIconImage.sprite = _sprites[(int)item.MyItem._myID];
        }

        //���݃t���[���̏�Ԃ�ۑ�
        _beforeItemFilter = _currentItemFilter;
        _beforeItemIndex = _currentItemIndex;
    }

    /// <summary> ��̓��͂����������̏��� </summary>
    void InputAbove()
    {

    }

    /// <summary> ���̓��͂����������̏��� </summary>
    void InputBelow()
    {

    }

    /// <summary> �E�̓��͂����������̏��� </summary>
    void InputRight()
    {
        //�E�̓��͂����m
        if (Input.GetButtonDown("Horizontal_Right"))
        {
            //�����ɃA�C�e���t�B���^�[��ύX���鏈��������
            _currentItemFilter = _currentItemFilter + 1;
            if (_currentItemFilter >= ItemFilter.ITEM_FILTER_END)
            {
                _currentItemFilter = ItemFilter.HEAL;
            }
        }
    }

    /// <summary> ���̓��͂����������̏��� </summary>
    void InputLeft()
    {
        //���̓��͂����m
        if (Input.GetButtonDown("Horizontal_Left"))
        {
            //�����ɃA�C�e���t�B���^�[��ύX���鏈��������
            _currentItemFilter = _currentItemFilter - 1;
            if (_currentItemFilter < ItemFilter.HEAL)
            {
                _currentItemFilter = ItemFilter.ALL;
            }
        }
    }

    /// <summary> �A�C�e������ޕʂŕ\������B���̃t�B���^�[�ɌW��A�C�e���{�^���̂݃A�N�e�B�u�ɂ���B����ȊO�͔�A�N�e�B�u�ɂ���B </summary>
    void Update_ItemFilter()
    {
        //�ŏ��̗v�f�𓪂ɂ���
        bool firstObj = true;

        //�O�ƍ��̃t�B���^�[���Ⴆ�΍X�V����
        if (_beforeItemFilter != _currentItemFilter)
        {
            //�ꎞ���X�g���쐬 : �{�^���̕Έڐ��ݒ肷��悤�B
            List<GameObject> temporaryList = new List<GameObject>();

            //�S�ĕ\������ꍇ�̏���
            if (_currentItemFilter == ItemFilter.ALL)
            {
                foreach (var item in _items)
                { 
                    //��������0�Ȃ�X�L�b�v����
                    if (PlayerManager.Instance.ItemVolume._itemNumberOfPossessions[(int)item.GetComponent<ItemButton>().MyItem._myID] == 0)
                    {
                        item.SetActive(false);
                        continue;
                    }
                    //���̗v�f��SelectedGameObject�Ɏw�肷��B
                    if (firstObj)
                    {
                        firstObj = false;
                        _eventSystem.SetSelectedGameObject(item);
                    }
                    item.SetActive(true);
                    temporaryList.Add(item);
                }
            }

            //�ꕔ�����\������ꍇ(�t�B���^�[�ɂ�����Ƃ��̏���)
            else
            {
                foreach (var item in _items)
                {
                    if ((int)item.GetComponent<ItemButton>().MyItem._myType == (int)_currentItemFilter)
                    {
                        //��������0�Ȃ�X�L�b�v����
                        if (PlayerManager.Instance.ItemVolume._itemNumberOfPossessions[(int)item.GetComponent<ItemButton>().MyItem._myID] == 0) 
                        {
                            item.SetActive(false);
                            continue;
                        }
                        //���̗v�f��SelectedGameObject�Ɏw�肷��B
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
            //�{�^���̐ݒ���X�V����
             Set_ItemButtonShiftDestination(_currentItemFilter, temporaryList);

            //�{�^���̐F���X�V����
            Change_FilterButtonColor();
        }
        _beforeItemFilter = _currentItemFilter;
    }

    /// <summary> �A�C�e���{�^���̕Έڐ�����߂鏈�� </summary>
    /// <param name="currentButton"> �ݒ肷��{�^�� </param>
    /// <param name="up">    ��ɐݒ肷��{�^�� </param>
    /// <param name="down">  ���ɐݒ肷��{�^�� </param>
    /// <param name="left">  ���ɐݒ肷��{�^�� </param>
    /// <param name="right"> �E�ɐݒ肷��{�^�� </param>
    void Set_ItemButtonShiftDestinationHelper(Button currentButton, Button up, Button down, Button left, Button right)
    {
        //�i�r�Q�[�V�������擾
        Navigation navigation = currentButton.navigation;
        //���[�h��ύX
        navigation.mode = Navigation.Mode.Explicit;
        //�Έڐ���w��
        navigation.selectOnUp = up;
        navigation.selectOnDown = down;
        navigation.selectOnLeft = left;
        navigation.selectOnRight = right;
        //�i�r�Q�[�V�������Z�b�g
        currentButton.navigation = navigation;
    }


    GameObject leftButton;
    GameObject rightButton;
    /// <summary> �A�C�e���{�^���̕Έڐ�����߂鏈���B : �v�f��0�܂���1�̎��̏������l����K�v�L��B </summary>
    void Set_ItemButtonShiftDestination(ItemFilter _currentItemFilter, List<GameObject> itemButton)
    {
        if (itemButton.Count <= 1) { return; }
        if (itemButton[0] != null)
        {
            //�t�B���^�[(��)�̑J�ڐ��ݒ肷��
            switch (_currentItemFilter)
            {
                case ItemFilter.ALL: leftButton = _keyFilterButton; rightButton = _healFilterButton; break;
                case ItemFilter.HEAL: leftButton = _allFilterButton; rightButton = _powerUpFilterButton; break;
                case ItemFilter.POWER_UP: leftButton = _healFilterButton; rightButton = _minusItemFilterButton; break;
                case ItemFilter.MINUS_ITEM: leftButton = _powerUpFilterButton; rightButton = _keyFilterButton; break;
                case ItemFilter.KEY: leftButton = _minusItemFilterButton; rightButton = _allFilterButton; break;
            }

            //�c�̕Έڐ��ݒ肷��
            //�擪��ݒ�
            Set_ItemButtonShiftDestinationHelper(
                itemButton[0].GetComponent<Button>(),
                itemButton[itemButton.Count - 1].GetComponent<Button>(),
                itemButton[1].GetComponent<Button>(),
                leftButton.GetComponent<Button>(),
                rightButton.GetComponent<Button>());

            //����ݒ�
            for (int i = 1; i < itemButton.Count - 1; i++)
            {
                Set_ItemButtonShiftDestinationHelper(
                itemButton[i].GetComponent<Button>(),
                itemButton[i - 1].GetComponent<Button>(),
                itemButton[i + 1].GetComponent<Button>(),
                leftButton.GetComponent<Button>(),
                rightButton.GetComponent<Button>());
            }

            //������ݒ�
            Set_ItemButtonShiftDestinationHelper(
                itemButton[itemButton.Count - 1].GetComponent<Button>(),
                itemButton[itemButton.Count - 2].GetComponent<Button>(),
                itemButton[0].GetComponent<Button>(),
                leftButton.GetComponent<Button>(),
                rightButton.GetComponent<Button>());
        }
    }
    //�I�[�o�[���[�h����(�s���͑����Ȃ邪�AToList��������s�����͗ǂ�����)
    void Set_ItemButtonShiftDestination(ItemFilter _currentItemFilter, GameObject[] itemButton)
    {
        if (itemButton.Length <= 1) { return; }
        if (itemButton[0] != null)
        {
            //�t�B���^�[(��)�̑J�ڐ��ݒ肷��
            switch (_currentItemFilter)
            {
                case ItemFilter.ALL: leftButton = _keyFilterButton; rightButton = _healFilterButton; break;
                case ItemFilter.HEAL: leftButton = _allFilterButton; rightButton = _powerUpFilterButton; break;
                case ItemFilter.POWER_UP: leftButton = _healFilterButton; rightButton = _minusItemFilterButton; break;
                case ItemFilter.MINUS_ITEM: leftButton = _powerUpFilterButton; rightButton = _keyFilterButton; break;
                case ItemFilter.KEY: leftButton = _minusItemFilterButton; rightButton = _allFilterButton; break;
            }

            //�c�̕Έڐ��ݒ肷��
            //�擪��ݒ�
            Set_ItemButtonShiftDestinationHelper(
                itemButton[0].GetComponent<Button>(),
                itemButton[itemButton.Length - 1].GetComponent<Button>(),
                itemButton[1].GetComponent<Button>(),
                leftButton.GetComponent<Button>(),
                rightButton.GetComponent<Button>());

            //����ݒ�
            for (int i = 1; i < itemButton.Length - 1; i++)
            {
                Set_ItemButtonShiftDestinationHelper(
                itemButton[i].GetComponent<Button>(),
                itemButton[i - 1].GetComponent<Button>(),
                itemButton[i + 1].GetComponent<Button>(),
                leftButton.GetComponent<Button>(),
                rightButton.GetComponent<Button>());
            }

            //������ݒ�
            Set_ItemButtonShiftDestinationHelper(
                itemButton[itemButton.Length - 1].GetComponent<Button>(),
                itemButton[itemButton.Length - 2].GetComponent<Button>(),
                itemButton[0].GetComponent<Button>(),
                leftButton.GetComponent<Button>(),
                rightButton.GetComponent<Button>());
        }
    }

    /// <summary> ���݂̃t�B���^�[�̐F��ς���B </summary>
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
