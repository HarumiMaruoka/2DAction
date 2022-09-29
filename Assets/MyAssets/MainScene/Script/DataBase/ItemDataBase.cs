
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// �S�ẴA�C�e���̏��ƁA
/// �v���C���[���������Ă���A�C�e�����Ǘ�����N���X�B
/// </summary>
public class ItemDataBase : MonoBehaviour
{
    //<===== ���̃N���X�Ŏg�p����^ =====>//
    /// <summary> �A�C�e���̏��������Ǘ�����\���� </summary>
    [System.Serializable]
    public struct ItemNumberOfPossessions
    {
        /// <summary> �C���f�b�N�X��ID�ŏ��������擾�ł���B </summary>
        public int[] _itemNumberOfPossessions;
    }

    //<===== �V���O���g���֌W =====>//
    /// <summary> GameManager�̃C���X�^���X </summary>
    private static ItemDataBase _instance;
    /// <summary> GameManager�̃C���X�^���X </summary>
    public static ItemDataBase Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new ItemDataBase();
            }
            return _instance;
        }
    }
    //�v���C�x�[�g�ȃR���X�g���N�^
    private ItemDataBase() { }

    //<===== �����o�[�ϐ� =====>//
    /// <summary> �A�C�e���f�[�^��������csv�t�@�C���̐�΃p�X </summary>
    [Header("�A�C�e���f�[�^��������csv�t�@�C���ւ̐�΃p�X"), SerializeField]
    string _itemCSVPath;
    /// <summary> �A�C�e����������ۑ����Ă���json�t�@�C���ւ̃p�X </summary>
    [Header("�m�F�p : �A�C�e����������ۑ����Ă���json�t�@�C���ւ̃p�X"), SerializeField]
    string _itemJsonPath;
    [Header("ItemData csv"), SerializeField]
    TextAsset _itemDataCsv = default;

    /// <summary> �S�ẴA�C�e���̏����i�[���Ă���ϐ� </summary>
    Item[] _itemData = new Item[(int)Item.ItemID.ITEM_ID_END];
    /// <summary> �S�ẴA�C�e���̏����i�[���Ă���ϐ� </summary>
    public Item[] ItemData { get => _itemData; }

    /// <summary> �A�C�e�����������i�[���Ă���ϐ� </summary>
    [Header("�m�F�p : �A�C�e��������"), SerializeField] ItemNumberOfPossessions _itemVolume;
    /// <summary> �A�C�e�����������i�[���Ă���ϐ� </summary>
    public ItemNumberOfPossessions ItemVolume { get => _itemVolume; }

    bool _isInitialized = false;

    //<===== Unity���b�Z�[�W =====>//
    void Awake()
    {
        if (_isInitialized = Initialized())
        {
            Debug.Log($"�������ɐ������܂����B{gameObject.name}");
        }
        else
        {
            Debug.Log($"�������Ɏ��s���܂����B{gameObject.name}");
        }
    }
    void Start()
    {

    }
    void Update()
    {

    }

    //<===== public�����o�[�֐� =====>//
    /// <summary>�@����̃A�C�e���̏�������ύX����B </summary>
    /// <param name="itemID"> �ύX����A�C�e����ID </param>
    /// <param name="volume"> �ύX���鐔 : <br/>
    /// 1�Ȃ炻��ID�̃A�C�e��������₷ <br/>
    /// -1�Ȃ炻��ID�̃A�C�e��������炷�B<br/>
    /// </param>
    public void MakeChanges_ItemNumberOfPossessions(int itemID, int volume)
    {
        //�C���f�b�N�X��������������
        if (itemID < (int)Item.ItemID.ITEM_ID_00 || itemID >= (int)Item.ItemID.ITEM_ID_END)
        {
            //�C���f�b�N�X���͈͊O�Ȃ珈���𔲂���
            Debug.LogError("�����ȃA�C�e��ID�ł��B");
            return;
        }
        ItemVolume._itemNumberOfPossessions[itemID] += volume;
    }
    //<===== private�����o�[�֐� =====>//
    /// <summary> �������֐� </summary>
    /// <returns> �������ɐ���������true��Ԃ��B </returns>
    bool Initialized()
    {
        /*** �V���O���g���֌W�̏��� ***/
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
        //���̃X�N���v�g���A�^�b�`���ꂽ�I�u�W�F�N�g�́A�V�[�����ׂ��ł��f�X�g���C����Ȃ��悤�ɂ���B
        DontDestroyOnLoad(gameObject);

        // �A�C�e����������ۑ����Ă���t�@�C���ւ̃p�X���擾���ϐ��ɕۑ��B
        _itemJsonPath = Path.Combine(Application.persistentDataPath, "ItemNumberOfPossessionsFile.json");

        /*** ���[�h���� ***/
        OnLoad_ItemCSV();
        OnLoad_ItemJson(_itemJsonPath);

        return true;
    }
    /// <summary> csv�t�@�C������S�ẴA�C�e���̏����擾���ϐ��Ɋi�[����B </summary>
    void OnLoad_ItemCSV()
    {
        //�t�@�C�������[�h����̂ɕK�v�ȏ�����
        int index = 0;//�C���f�b�N�X�̏�����
        bool isFirstLine = true;//��s�ڂ��ǂ����𔻒f����Boolean�̏�����

        //CSV�t�@�C������A�C�e���f�[�^��ǂݍ��݁A�z��ɕۑ�����
        //StreamReader sr = new StreamReader(@_itemCSVPath);//�t�@�C�����J��
        try
        {
            // �t�@�C�����J��
            var sr = new StringReader(_itemDataCsv.text);
            // �t�@�C������ǂݍ��݁A�t�B�[���h�ɕۑ�����B
            string value = "";
            while ((value = sr.ReadLine()) != null)// �����܂ŌJ��Ԃ�
            {
                //�ŏ��̍s(�w�b�_�[�̍s)�̓X�L�b�v����
                if (isFirstLine)
                {
                    isFirstLine = false;
                    continue;
                }

                string[] values = value.Split(',');//��s�ǂݍ��݋�؂��ĕۑ�����

                //��ޕʂŐ������ۑ�����
                switch (values[2])
                {
                    case "HealItem": _itemData[index] = new HealItem((Item.ItemID)int.Parse(values[0]), values[1], Item.ItemType.HEAL, int.Parse(values[3]), values[4]); break;
                    case "PowerUpItem": _itemData[index] = new PowerUpItem((Item.ItemID)int.Parse(values[0]), values[1], Item.ItemType.POWER_UP, int.Parse(values[3]), values[4]); break;
                    case "MinusItem": _itemData[index] = new MinusItem((Item.ItemID)int.Parse(values[0]), values[1], Item.ItemType.MINUS_ITEM, int.Parse(values[3]), values[4]); break;
                    case "KeyItem": _itemData[index] = new KeyItem((Item.ItemID)int.Parse(values[0]), values[1], Item.ItemType.KEY, int.Parse(values[3]), values[4]); break;
                    default: Debug.LogError("�ݒ肳��Ă��Ȃ�ItemType�ł��B"); break;
                }
                index++;
            }
        }
        catch(FileNotFoundException e)
        {
            Debug.LogError($"�G���[�I�C�����Ă�������!\n{e.Message}");
        }
    }

    /// <summary> �A�C�e���̏�������json�t�@�C�����烍�[�h����B </summary>
    void OnLoad_ItemJson(string filePath)
    {
        Debug.Log("�A�C�e�������������[�h���܂��I");
        // �O�̂��߃t�@�C���̑��݃`�F�b�N
        if (!File.Exists(filePath))
        {
            //�����Ƀt�@�C���������ꍇ�̏���������
            Debug.LogError("�A�C�e����������ۑ����Ă���t�@�C����������܂���B");

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
