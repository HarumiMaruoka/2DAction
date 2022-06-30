using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    /// <summary> �O�t���[���ɑI�����Ă����A�C�e����ID(index) </summary>
    int _beforeItemIndex;
    /// <summary> ���ݑI�����Ă���A�C�e����ID(index) </summary>
    int _currentItemIndex;

    //����������
    void Start()
    {
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
    }

    void Update()
    {

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
