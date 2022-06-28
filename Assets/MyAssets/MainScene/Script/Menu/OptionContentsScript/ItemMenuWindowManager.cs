using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemMenuWindowManager : MonoBehaviour
{
    public enum ItemFilter
    {
        ALL,
        HEAL,
        POWER_UP,
        MINUS_ITEM,
        KEY,

        ITEM_FILTER_END
    }

    [Header("�A�C�e���{�^���v���n�u"), SerializeField] GameObject _itemButtonPrefab;
    GameObject[] _items = new GameObject[(int)Item.ItemID.ITEM_ID_END];

    //�A�C�e���t�B���^�[
    ItemFilter _beforeItemFilter;//�O�t���[���I�����Ă����A�C�e���t�B���^�[
    ItemFilter _currentItemFilter;//���݂̃t���[���őI�����Ă���A�C�e���t�B���^�[

    //���ݑI�����Ă���A�C�e���̃C���f�b�N�X
    int _beforeItemIndex;
    int _currentItemIndex;




    void Start()
    {
        if (_itemButtonPrefab == null)
        {
            Debug.LogError("�A�C�e���{�^���̃v���n�u���A�T�C������Ă��܂���B");
        }
        //�K�v�Ȃ��̂��A�q�Ƃ��ăC���X�^���V�G�C�g���Ă����B
        for (int i = 0; i < (int)Item.ItemID.ITEM_ID_END; i++)
        {
            _items[i] = Instantiate(_itemButtonPrefab, Vector3.zero, Quaternion.identity, gameObject.transform);

            //�e�L�X�g���Z�b�g����
            Text itemText = _items[i].transform.GetChild(0).gameObject.GetComponent<Text>();
            itemText.text = "�����ɃA�C�e���̖��O�ƌ�������B���O�ƌ��̊Ԃɂ͂�������Space������";
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
