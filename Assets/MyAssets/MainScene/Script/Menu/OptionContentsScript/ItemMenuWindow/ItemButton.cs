using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemButton : MonoBehaviour
{
    //<===== �����o�[�ϐ� =====>//
    //�A�C�e�����̃e�L�X�g
    Text _itemNameText;
    //���̃e�L�X�g
    Text _itemVolumText;

    //����Button������Item
    Item _myItem;
    public Item MyItem { get => _myItem; }

    int _beforeItemVolume;
    int _nowItemVolume;

    ItemMenuWindowManager _itemWindowManager;

    //<===== Unity���b�Z�[�W =====>//
    void Start()
    {
        _itemWindowManager = GameObject.FindGameObjectWithTag("ToolWindow").GetComponent<ItemMenuWindowManager>();
        //�e�L�X�g�̎擾
        _itemNameText = transform.GetChild(0).GetComponent<Text>();
        _itemVolumText = transform.GetChild(1).GetComponent<Text>();

        //���O��ݒ�
        _itemNameText.text = " " + _myItem._name;
        //��������ݒ�
        Update_ItemVolume();
    }
    void Update()
    {
        if (PlayerStatusManager.Instance == null)
        {
            Debug.LogError("PlayerManager��null�ł��I");
        }
        //���������擾
        _nowItemVolume = ItemDataBase.Instance.ItemVolume._itemNumberOfPossessions[(int)_myItem._myID];
        if (_nowItemVolume != _beforeItemVolume)
        {
            Update_ItemVolume();
        }
        //��������0�ȉ��ɂȂ������̏���
        if (_nowItemVolume <= 0)
        {
            Debug.Log(_myItem._name + "�̏�������0�ɂȂ�܂����B\n" +
                "_myItem._name�̃{�^�����A�N�e�B�u�ɂ��܂��B");
            _itemWindowManager.ShouldDo_HaveItemZero(this, _myItem._myID, (ItemMenuWindowManager.ItemFilter)_myItem._myType);
        }
        _beforeItemVolume = _nowItemVolume;
    }
    /// <summary> �A�C�e���{�^�����A�N�e�B�u�ɂȂ������̏����B </summary>
    void OnEnable()
    {
        //���������Z�b�g����
        Update_ItemVolume();
    }

    //<===== public�����o�[�֐� =====>//
    public void SetItemData(Item item)
    {
        _myItem = item;
    }
    /// <summary> ���������X�V����B </summary>
    public void Update_ItemVolume()
    {
        if (_itemVolumText != null)
        {
            _itemVolumText.text = " �~ " + ItemDataBase.Instance.ItemVolume._itemNumberOfPossessions[(int)_myItem._myID].ToString() + " ";
        }
    }
    /// <summary> �{�^��������������s���� </summary>
    public void Use_ThisItem()
    {
        ItemDataBase.Instance.ItemData[(int)MyItem._myID].UseItem();
    }
}
