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

    //Item���̊i�[��
    struct ItemManager
    {
        /// <summary> �A�C�e�����̊i�[�� </summary>
        public static Item[] _itemDefinition;
        /// <summary> �A�C�e���̏����� </summary>
        public static int[] _itemHaveVolume;
    }
    //�A�C�e���Ǘ��p�ϐ�
    ItemManager[] _item = new ItemManager[(int)Item.ItemID.ITEM_ID_END];

    //�������̊i�[��

    void Start()
    {

    }

    void SetItem()
    {
        //�A�C�e�������X�v���b�h�V�[�g����ǂݍ��݁A�z��ɕۑ�����
    }
}
