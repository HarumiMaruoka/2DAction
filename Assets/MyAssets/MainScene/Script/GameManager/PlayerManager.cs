using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{

    //���̃N���X�̓V���O���g���p�^�[�����g�p�������̂ł���B
    //�C���X�^���X�𐶐�
    private static GameManager _instance;
    //�C���X�^���X��ǂݎ���p���C���X�^���X���Ȃ���΃C���X�^���X�𐶐����ۑ�����
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameManager();
            }
            return _instance;
        }
    }

    /// <summary> �A�C�e���̏����� </summary>
    int[] _itemHaveVolume = new int[(int)Item.ItemID.ITEM_ID_END];
    public int[] ItemHaveVolume
    {
        get
        {
            return _itemHaveVolume;
        }
    }

    public void SetItemHaveVolume(int index,int value)
    {
        _itemHaveVolume[index] = value;
    }

    //�������̊i�[��

    void Start()
    {

    }

    void SetItem()
    {
        //�A�C�e�������X�v���b�h�V�[�g����ǂݍ��݁A�z��ɕۑ�����
    }
}
