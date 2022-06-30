using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] string _itemFilePath;

    //���̃N���X�̓V���O���g���p�^�[�����g�p�������̂ł���B
    //�C���X�^���X�𐶐�
    private static PlayerManager _instance;
    //�C���X�^���X��ǂݎ���p���C���X�^���X���Ȃ���΃C���X�^���X�𐶐����ۑ�����
    public static PlayerManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("PlayerManager._instance��null�ł��B");
            }
            return _instance;
        }
    }

    /// <summary> �A�C�e���̏����� </summary>
    [System.Serializable]
    public struct ItemNumberOfPossessions
    {
        public int[] _itemNumberOfPossessions;
    }
    [SerializeField] ItemNumberOfPossessions _itemVolume;

    public ItemNumberOfPossessions ItemVolume
    {
        get
        {
            return _itemVolume;
        }
    }
    private void Awake()
    {
        //�����C���X�^���X���ݒ肳��Ă��Ȃ������玩�g��������
        if (_instance == null)
        {
            _instance = this;
        }
        //�������ɑ��݂���ꍇ�́A���̃I�u�W�F�N�g��j������B
        else if (_instance != null)
        {
            Destroy(this);
        }
        _itemVolume._itemNumberOfPossessions = new int[(int)Item.ItemID.ITEM_ID_END];
    }

    void Start()
    {
        //���̃I�u�W�F�N�g�́A�V�[�����ׂ��ł��f�X�g���C���Ȃ��B
        DontDestroyOnLoad(gameObject);
        // �A�C�e�����������A�ۑ����Ă���t�@�C���̃p�X���擾�B
        _itemFilePath = Path.Combine(Application.persistentDataPath, "ItemNumberOfPossessionsFile.json");
        OnLoad_ItemNumberOfPossessions(_itemFilePath);
        Debug.Log("����������");
    }

    private void Update()
    {
        //K�L�[�����ŃA�C�e�����������Z�[�u����
        if (Input.GetKeyDown(KeyCode.K))
        {
            OnSave_ItemNumberOfPossessions(_itemFilePath);
        }
        //L�L�[�����ŃA�C�e�������������[�h����
        if (Input.GetKeyDown(KeyCode.L))
        {
            OnLoad_ItemNumberOfPossessions(_itemFilePath);
        }
    }

    /// <summary>�@����̃A�C�e���̏���ύX </summary>
    /// <param name="itemID"> �ύX����A�C�e����ID </param>
    /// <param name="value"> �ύX���鐔 </param>
    public void Set_ItemNumberOfPossessions(int itemID, int volume)
    {
        //�C���f�b�N�X��������������
        if (itemID <= 0 || itemID >= (int)Item.ItemID.ITEM_ID_END)
        {
            //�C���f�b�N�X����O�Ȃ珈���𔲂���
            Debug.LogError("�����ȃA�C�e��ID�ł��B");
            return;
        }
        ItemVolume._itemNumberOfPossessions[itemID] += volume;
    }

    //�������̊i�[��

    /// <summary> �A�C�e���̏��������t�@�C�����烍�[�h </summary>
    void OnLoad_ItemNumberOfPossessions(string filePath)
    {
        Debug.Log("�A�C�e�������������[�h���܂��I");
        // �O�̂��߃t�@�C���̑��݃`�F�b�N
        if (!File.Exists(filePath))
        {
            //�����Ƀt�@�C���������ꍇ�̏���������
            Debug.Log("�A�C�e����������ۑ�����t�@�C����������܂���B");

            //�����𔲂���
            return;
        }

        // JSON�I�u�W�F�N�g���A�f�V���A���C�Y(C#�`���ɕϊ�)���A�l���Z�b�g�B
        _itemVolume = JsonUtility.FromJson<ItemNumberOfPossessions>(File.ReadAllText(filePath));
    }

    void OnSave_ItemNumberOfPossessions(string filePath)
    {
        Debug.Log("�A�C�e�����������Z�[�u���܂��I");
        // �A�C�e���������f�[�^���AJSON�`���ɃV���A���C�Y���A�t�@�C���ɕۑ�
        File.WriteAllText(filePath, JsonUtility.ToJson(_itemVolume, false));
    }
}
