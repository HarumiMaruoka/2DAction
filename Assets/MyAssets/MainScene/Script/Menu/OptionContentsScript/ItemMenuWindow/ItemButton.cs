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
        _nowItemVolume = OldItemDataBase.Instance.ItemVolume._itemNumberOfPossessions[(int)_myItem._myID];
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
            _itemVolumText.text = " �~ " + OldItemDataBase.Instance.ItemVolume._itemNumberOfPossessions[(int)_myItem._myID].ToString() + " ";
        }
    }
    /// <summary> �{�^��������������s���� </summary>
    public void Use_ThisItem()
    {
        // �Q�[���I�u�W�F�N�g�ɏ�����a����K�v���Ȃ������̓A�C�e�����g������s����B
        OldItemDataBase.Instance.ItemData[(int)MyItem._myID].UseItem();

        // �Q�[���I�u�W�F�N�g�Ƃ��ĕ\�����ׂ����̂̓C���X�^���V�G�C�g����B
        switch (MyItem._myID)
        {
            case Item.ItemID.ITEM_ID_03: Instantiate(UseItemManager.Instance._itemID3); break;
            case Item.ItemID.ITEM_ID_04: Instantiate(UseItemManager.Instance._itemID4); break;
            case Item.ItemID.ITEM_ID_05: Instantiate(UseItemManager.Instance._itemID5); break;
            case Item.ItemID.ITEM_ID_06: Instantiate(UseItemManager.Instance._itemID6); break;
        }
    }
}
