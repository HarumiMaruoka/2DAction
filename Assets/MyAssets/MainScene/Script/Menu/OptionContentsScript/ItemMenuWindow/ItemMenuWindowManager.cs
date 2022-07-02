using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary> �A�C�e���E�B���h�E���Ǘ�����N���X </summary>
public class ItemMenuWindowManager : MonoBehaviour
{
    //�\������A�C�e���̃t�B���^�[
    public enum ItemFilter
    {
        ALL,
        HEAL,
        POWER_UP,
        MINUS_ITEM,
        KEY,

        ITEM_FILTER_END
    }
    //�A�C�e���t�B���^�[
    ItemFilter _beforeItemFilter;//�O�t���[���I�����Ă����A�C�e���t�B���^�[
    ItemFilter _currentItemFilter;//���݂̃t���[���őI�����Ă���A�C�e���t�B���^�[

    /// <summary> �A�C�e���{�^���̃v���n�u </summary>
    [Header("�A�C�e���{�^���v���n�u"), SerializeField] GameObject _itemButtonPrefab;
    /// <summary> �A�C�e���{�^���̃Q�[���I�u�W�F�N�g�̔z�� </summary>
    GameObject[] _items = new GameObject[(int)Item.ItemID.ITEM_ID_END];
    //
    [SerializeField] EventSystem _eventSystem;

    /// <summary> �O�t���[���ɑI�����Ă����A�C�e����ID(index) </summary>
    int _beforeItemIndex;
    /// <summary> ���ݑI�����Ă���A�C�e����ID(index) </summary>
    int _currentItemIndex;

    //�������̃e�L�X�g
    [SerializeField] Text _ItemExplanatoryText;

    //����������
    void Start()
    {
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
        GameObject _itemContent = GameObject.FindGameObjectWithTag("ItemDrawContent");
        if (_itemContent == null)
        {
            Debug.LogError("ItemDrawContent�̃^�O���t�����A�I�u�W�F�N�g�̎擾�Ɏ��s���܂����B");
        }
        //�A�C�e���{�^����ScrollView�́AContent�̎q�Ƃ��ăC���X�^���V�G�C�g���Ă����B
        for (int i = 0; i < (int)Item.ItemID.ITEM_ID_END; i++)
        {
            _items[i] = Instantiate(_itemButtonPrefab, Vector3.zero, Quaternion.identity, _itemContent.transform);

            //ItemData���Z�b�g����
            _items[i].GetComponent<ItemButtonTextManager>().SetItemData(GameManager.Instance.ItemData[i]);
        }

        _eventSystem.SetSelectedGameObject(_items[0]);

        //select on up �� down ��ݒ肷�� �����̂ł��ƂŒ���
        Navigation navigation = _items[(int)Item.ItemID.ITEM_ID_00].GetComponent<Button>().navigation;
        navigation.mode = Navigation.Mode.Explicit;
        navigation.selectOnUp = _items[(int)Item.ItemID.ITEM_ID_END - 1].GetComponent<Button>();
        navigation.selectOnDown = _items[(int)Item.ItemID.ITEM_ID_01].GetComponent<Button>();
        _items[(int)Item.ItemID.ITEM_ID_00].GetComponent<Button>().navigation = navigation;
        //select on up �� down ��ݒ肷��
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

    /// <summary> �A�C�e���E�B���h�E�̍X�V�֐� </summary>
    void UpdateItemWindow()
    {
        //����
        InputAbove();
        InputBelow();
        InputRight();
        InputLeft();

        //�K�v�������(�O�t���[���ƌ��݂̃t���[���őI�����Ă�����̂��Ⴆ��)�X�V����
        if (_beforeItemFilter != _currentItemFilter ||
            _beforeItemIndex != _currentItemIndex)
        {

        }
        //���ݑI������Ă���{�^�����擾
        GameObject go = EventSystem.current.currentSelectedGameObject;
        //��������ݒ�
        if (go != null && go.GetComponent<ItemButtonTextManager>() != null)
        {
            _ItemExplanatoryText.text = go.GetComponent<ItemButtonTextManager>().MyItem._myExplanatoryText;
        }

        //���݃t���[���̏�Ԃ�ۑ�
        _beforeItemFilter = _currentItemFilter;
        _beforeItemIndex = _currentItemIndex;
    }

    /// <summary> ��̓��� </summary>
    void InputAbove()
    {

    }

    /// <summary> ���̓��� </summary>
    void InputBelow()
    {

    }

    /// <summary> �E�̓��� </summary>
    void InputRight()
    {

    }

    /// <summary> ���̓��� </summary>
    void InputLeft()
    {

    }
}
