using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemButton : MonoBehaviour
{
    //�A�C�e�����̃e�L�X�g
    Text _itemNameText;
    //���̃e�L�X�g
    Text _itemVolumText;

    //����Button������Item
    Item _myItem;
    public Item MyItem { get => _myItem; }

    int _beforeItemVolume;
    int _nowItemVolume;

    public void SetItemData(Item item)
    {
        _myItem = item;
    }


    void Start()
    {
        //�e�L�X�g�̎擾
        _itemNameText = transform.GetChild(0).GetComponent<Text>();
        _itemVolumText = transform.GetChild(1).GetComponent<Text>();

        //���O��ݒ�
        _itemNameText.text = " " + _myItem._name;
        //��������ݒ�
        Update_ItemVolume();
    }

    private void Update()
    {
        if (PlayerManager.Instance == null)
        {
            Debug.Log("aaa");
        }
        _nowItemVolume = PlayerManager.Instance.ItemVolume._itemNumberOfPossessions[(int)_myItem._myID];
        if (_nowItemVolume != _beforeItemVolume)
        {
            Update_ItemVolume();
        }
        _beforeItemVolume = _nowItemVolume;
    }

    /// <summary> �A�C�e���{�^�����A�N�e�B�u�ɂȂ������̏����B </summary>
    private void OnEnable()
    {
        //���������Z�b�g����
        Update_ItemVolume();
    }

    /// <summary> ���������X�V����B </summary>
    public void Update_ItemVolume()
    {
        if (_itemVolumText != null)
        {
            _itemVolumText.text = " �~ " + PlayerManager.Instance.ItemVolume._itemNumberOfPossessions[(int)_myItem._myID].ToString() + " ";
        }
    }


    /// <summary> �{�^��������������s���� </summary>
    public void Use_ThisItem()
    {
        GameManager.Instance.ItemData[(int)MyItem._myID].UseItem();
    }
}
