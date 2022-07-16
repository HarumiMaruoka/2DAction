using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary> �A�C�e�����������Ǘ�����N���X�B </summary>
public class ItemHaveValueManager : MonoBehaviour
{
    private static ItemHaveValueManager _instance;
    //�C���X�^���X�͓ǂݎ���p
    public static ItemHaveValueManager Instance
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
    //�v���C�x�[�g�ȃR���X�g���N�^���`����B
    private ItemHaveValueManager() { }

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
        //���̃I�u�W�F�N�g�́A�V�[�����ׂ��ł��f�X�g���C���Ȃ��B
        DontDestroyOnLoad(gameObject);

        //�z�񕪂̗̈���m�ۂ���B
        _itemVolume._itemNumberOfPossessions = new int[(int)Item.ItemID.ITEM_ID_END];
    }


    //�A�C�e���������̕ۑ���
    [SerializeField] string _itemFilePath;

    /// <summary> �A�C�e���̏����� </summary>
    [System.Serializable]
    public struct ItemNumberOfPossessions
    {
        public int[] _itemNumberOfPossessions;
    }
    [SerializeField] ItemNumberOfPossessions _itemVolume;
    public ItemNumberOfPossessions ItemVolume { get => _itemVolume; }

    void Start()
    {
        // �A�C�e�����������A�ۑ����Ă���t�@�C���̃p�X���擾���t�@�C�����J���B
        _itemFilePath = Path.Combine(Application.persistentDataPath, "ItemNumberOfPossessionsFile.json");
        // �A�C�e�����������A�t�@�C������擾����B
        OnLoad_ItemNumberOfPossessions(_itemFilePath);
        Debug.Log("PlayerManager�̏������ɐ������܂����B");
    }

    void Update()
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

    /// <summary>�@����̃A�C�e���̏�������ύX����B </summary>
    /// <param name="itemID"> �ύX����A�C�e����ID </param>
    /// <param name="value"> �ύX���鐔 </param>
    public void Set_ItemNumberOfPossessions(int itemID, int volume)
    {
        //�C���f�b�N�X��������������
        if (itemID < (int)Item.ItemID.ITEM_ID_00 || itemID >= (int)Item.ItemID.ITEM_ID_END)
        {
            //�C���f�b�N�X����O�Ȃ珈���𔲂���
            Debug.LogError("�����ȃA�C�e��ID�ł��B");
            return;
        }
        ItemVolume._itemNumberOfPossessions[itemID] += volume;
    }

    /// <summary> �A�C�e���̏�������json�t�@�C�����烍�[�h����B </summary>
    void OnLoad_ItemNumberOfPossessions(string filePath)
    {
        Debug.Log("�A�C�e�������������[�h���܂��I");
        // �O�̂��߃t�@�C���̑��݃`�F�b�N
        if (!File.Exists(filePath))
        {
            //�����Ƀt�@�C���������ꍇ�̏���������
            Debug.Log("�A�C�e����������ۑ����Ă���t�@�C����������܂���B");

            //�����𔲂���
            return;
        }
        // JSON�I�u�W�F�N�g���A�f�V���A���C�Y(C#�`���ɕϊ�)���A�l���Z�b�g�B
        _itemVolume = JsonUtility.FromJson<ItemNumberOfPossessions>(File.ReadAllText(filePath));
    }

    /// <summary> �A�C�e���̏�������json�t�@�C���ɃZ�[�u����B </summary>
    void OnSave_ItemNumberOfPossessions(string filePath)
    {
        Debug.Log("�A�C�e�����������Z�[�u���܂��I");
        // �A�C�e���������f�[�^���AJSON�`���ɃV���A���C�Y���A�t�@�C���ɕۑ�
        File.WriteAllText(filePath, JsonUtility.ToJson(_itemVolume, false));
    }
}
