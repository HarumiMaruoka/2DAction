using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //���̃N���X�̓V���O���g���p�^�[�����g�p�������̂ł���B

    //�C���X�^���X�𐶐�
    private static GameManager _instance;

    /// <summary> �A�C�e���f�[�^���������t�@�C���̃p�X </summary>
    [SerializeField] string _itemCSVPath;
    /// <summary> �A�C�e���f�[�^�x�[�X </summary>
    Item[] _itemData = new Item[(int)Item.ItemID.ITEM_ID_END];

    //�C���X�^���X���J�v�Z����
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

    private void Start()
    {
        //���̃X�N���v�g���A�^�b�`���ꂽ�I�u�W�F�N�g�́A�V�[�����ׂ��ł��f�X�g���C����Ȃ��悤�ɂ���B
        DontDestroyOnLoad(gameObject);

        //BGM�Ǘ��p�ϐ���������
        //_bgm = GetComponent<AudioSource>();
        //if (_bgm == null)
        //{
        //    Debug.LogError("�Q�[���}�l�[�W���[��AudioSource Component���A�^�b�`���Ă�������!");
        //}
    }

    //���Ƃ�BGMManager�ɏ���
    /// <summary> BGM��؂�ւ��� </summary>
    //public void ChangeBGM(AudioClip newAudio)
    //{
    //    _bgm.clip = newAudio;
    //    _bgm.Play();
    //}

    /// <summary> �A�C�e���f�[�^���t�@�C������ǂݍ��� </summary>
    void LoadItemCSV()
    {
        int index = 0;
        bool isFirstLine = true;
        //CSV�t�@�C������A�C�e���f�[�^��ǂݍ��݁A�z��ɕۑ�����
        StreamReader sr = new StreamReader(@_itemCSVPath);//�t�@�C�����J��
        while (!sr.EndOfStream)// �����܂ŌJ��Ԃ�
        {
            string[] values = sr.ReadLine().Split(',');//��s�ǂݍ��݋�؂��ĕۑ�����
            if (isFirstLine)//�ŏ��̍s(�w�b�_�[�̍s)�̓X�L�b�v����
            {
                isFirstLine = false;
                continue;
            }
            //��ޕʂŐ������ۑ�����
            switch (values[2])
            {
                case "HealItem": _itemData[index] = new HealItem((Item.ItemID)int.Parse(values[0]), values[1], Item.ItemType.HEAL, int.Parse(values[3]), values[4]); break;
                case "PowerUpItem": _itemData[index] = new PowerUpItem((Item.ItemID)int.Parse(values[0]), values[1], Item.ItemType.POWER_UP, int.Parse(values[3]), values[4]); break;
                case "MinusItem": _itemData[index] = new MinusItem((Item.ItemID)int.Parse(values[0]), values[1], Item.ItemType.MINUS_ITEM, int.Parse(values[3]), values[4]); break;
                case "KeyItem": _itemData[index] = new KeyItem((Item.ItemID)int.Parse(values[0]), values[1], Item.ItemType.KEY, int.Parse(values[3]), values[4]); break;
                default: Debug.LogError("�ݒ肳��Ă��Ȃ�ItemType�ł��B"); break;
            }

            //�����擾����


            index++;
        }
    }

}
