using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary> �����Ă��鑕�����Ǘ�����N���X </summary>
public class EquipmentManager : MonoBehaviour
{
    //<======= ���̃N���X�Ŏg�p����^ =======>//
    /// <summary> ������ID </summary>
    public enum EquipmentID
    {
        ID_0,
        ID_1,
        ID_2,
        ID_3,
        ID_4,
        ID_5,
        ID_6,
        ID_7,
        ID_8,
        ID_9,

        ID_10,
        ID_11,

        ID_END,
    }
    /// <summary> ���ݑ������Ă��鑕����\���\���� </summary>
    struct MyEquipped
    {
        Equipment _head;
        Equipment _torso;
        Equipment _arm;
        Equipment _foot;
    }
    /// <summary> �������Ă��鑕�����i�[����\���� </summary>
    struct HaveEquipped
    {
        public List<Equipment> _equipments;
    }

    //<=========== �K�v�Ȓl ===========>//
    /// <summary> �S�Ă̑����̏����ꎞ�ۑ����Ă����ϐ� </summary>
    Equipment[] _equipmentData;
    /// <summary> �������Ă��鑕���̃��X�g </summary>
    HaveEquipped _haveEquipment;
    /// <summary> ���ݑ������Ă��鑕�� </summary>
    MyEquipped _myEquipped;

    //<======== �A�T�C�����ׂ��l ========>//


    //<===== �C���X�y�N�^����ݒ肷�ׂ��l =====>//
    [Header("�����̊�{��񂪊i�[���ꂽcsv�t�@�C���ւ̃p�X"), SerializeField] string _equipmentCsvFilePath;
    [Header("�������Ă��鑕���̏�񂪊i�[���ꂽjson�t�@�C���ւ̃p�X"), SerializeField] string _equipmentHaveJsonFilePath;
    [Header("���ݑ������Ă��鑕���̏�񂪊i�[���ꂽjson�t�@�C���ւ̃p�X"), SerializeField] string _equippedJsonFilePath;
    /// <summary> �v���C���[�������ł��鑕���̍ő吔 </summary>
    [Header("�v���C���[�������ł��鑕���̍ő吔"), SerializeField] int _maxHaveValue;


    //<======�V���O���g���p�^�[���֘A======>//
    private static EquipmentManager _instance;
    public static EquipmentManager Instance
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
    private EquipmentManager() { }

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
            Destroy(this.gameObject);
        }

        // �������Ă��鑕����ۑ����Ă���t�@�C���̃p�X���擾���A�t�@�C�����J���B
        _equipmentHaveJsonFilePath = Path.Combine(Application.persistentDataPath, "HaveEquipmentFile.json");
        //���̃I�u�W�F�N�g�́A�V�[�����ׂ��ł��f�X�g���C���Ȃ��B
        DontDestroyOnLoad(gameObject);
        //���X�g�p�̃����������蓖�Ă�B
        _haveEquipment._equipments = new List<Equipment>();
        //�N���X��������
        Initialize_EquipmentBase();
    }
    void Start()
    {

    }

    void Update()
    {

    }

    //<======== ���̃N���X�̏������֘A ========>//
    /// <summary> �������N���X�̏������֐��B(�h����ŌĂяo���B) </summary>
    /// <returns> �������ɐ��������ꍇtrue�A���s������false��Ԃ��B </returns>
    bool Initialize_EquipmentBase()
    {
        //csv���瑕�������擾
        OnLoad_EquipmentData_csv();
        return true;
    }

    //<======== ���[�h & �Z�[�u�֘A ========>//
    /// <summary> csv�t�@�C������A�S�Ă̑����̃f�[�^��ǂݍ��ފ֐� </summary>
    /// <returns> �ǂݍ��񂾌��ʂ�Ԃ��B���s�����ꍇ��null��Ԃ��B </returns>
    void OnLoad_EquipmentData_csv()
    {
        _equipmentData = new Equipment[(int)EquipmentID.ID_END];

        int index = 0;
        bool isFirstLine = true;//��s�ڂ��ǂ����𔻒f����l
        //CSV�t�@�C������A�C�e���f�[�^��ǂݍ��݁A�z��ɕۑ�����
        StreamReader sr = new StreamReader(@_equipmentCsvFilePath);//�t�@�C�����J��
        while (!sr.EndOfStream)// �����܂ŌJ��Ԃ�
        {
            string[] values = sr.ReadLine().Split(',');//��s�ǂݍ��݋�؂��ĕۑ�����
            //�ŏ��̍s(�w�b�_�[�̍s)�̓X�L�b�v����
            if (isFirstLine)
            {
                isFirstLine = false;
                continue;
            }
            //��ޕʂŐ������ۑ�����
            switch (values[1])
            {
                //���p�������擾���ۑ�
                case "Head":
                    _equipmentData[index] = new HeadParts(
           (EquipmentID)int.Parse(values[0]),//ID
           Equipment.EquipmentType.HEAD_PARTS,//Type
           values[2],//name
           Get_EquipmentRarity(values[3]), //rarity
           float.Parse(values[4]),
           float.Parse(values[5]),
           float.Parse(values[6]),
           float.Parse(values[7]),
           float.Parse(values[8]),
           float.Parse(values[9]),
           float.Parse(values[10]),
           float.Parse(values[11]),
           values[12],
           values[13]); break;
                //���p�������擾���ۑ�
                case "Torso":
                    _equipmentData[index] = new TorsoParts(
           (EquipmentID)int.Parse(values[0]),//ID
           Equipment.EquipmentType.TORSO_PARTS,//Type
           values[2],//name
           Get_EquipmentRarity(values[3]), //rarity
           float.Parse(values[4]),
           float.Parse(values[5]),
           float.Parse(values[6]),
           float.Parse(values[7]),
           float.Parse(values[8]),
           float.Parse(values[9]),
           float.Parse(values[10]),
           float.Parse(values[11]),
           values[12],
           values[13]); break;
                //�r�p�������擾���ۑ�
                case "Arm":
                    _equipmentData[index] = new ArmParts(
           (EquipmentID)int.Parse(values[0]),//ID
           Equipment.EquipmentType.ARM_PARTS,//Type
           values[2],//name
           Get_EquipmentRarity(values[3]), //rarity
           float.Parse(values[4]),
           float.Parse(values[5]),
           float.Parse(values[6]),
           float.Parse(values[7]),
           float.Parse(values[8]),
           float.Parse(values[9]),
           float.Parse(values[10]),
           float.Parse(values[11]),
           values[12],
           values[13],
           ArmParts.Get_AttackType(values[14])); break;
                //���p�������擾���ۑ�
                case "Foot":
                    _equipmentData[index] = new FootParts(
           (EquipmentID)int.Parse(values[0]),//ID
           Equipment.EquipmentType.FOOT_PARTS,//Type
           values[2],//name
           Get_EquipmentRarity(values[3]), //rarity
           float.Parse(values[4]),
           float.Parse(values[5]),
           float.Parse(values[6]),
           float.Parse(values[7]),
           float.Parse(values[8]),
           float.Parse(values[9]),
           float.Parse(values[10]),
           float.Parse(values[11]),
           values[12],
           values[13]); break;
                default: Debug.LogError("�ݒ肳��Ă��Ȃ�EquipmentType�ł��B"); break;
            }
            index++;
        }

    }
    /// <summary> �������Ă��鑕�����Ajson�t�@�C������f�[�^��ǂݍ��݁A�����o�[�ϐ��Ɋi�[���鏈���B </summary>
    public void OnLoad_EquipmentHaveData_Json()
    {
        Debug.Log("�������Ă��鑕���f�[�^�����[�h���܂��I");
        // �O�̂��߃t�@�C���̑��݃`�F�b�N
        if (!File.Exists(_equipmentHaveJsonFilePath))
         {
            //�����Ƀt�@�C���������ꍇ�̏���������
            Debug.Log("�������Ă��鑕���f�[�^��ۑ����Ă���t�@�C����������܂���B");

            //�����𔲂���
            return;
        }
        // JSON�I�u�W�F�N�g���A�f�V���A���C�Y(C#�`���ɕϊ�)���A�l���Z�b�g�B
        //_itemVolume = JsonUtility.FromJson<ItemNumberOfPossessions>(File.ReadAllText(filePath));
    }
    /// <summary> �������Ă��鑕���̃f�[�^���Ajson�t�@�C���ɕۑ����鏈���B </summary>
    public void OnSave_EquipmentHaveData_Json()
    {
        Debug.Log("�������Ă��鑕���f�[�^���Z�[�u���܂��I");
        // �A�C�e���������f�[�^���AJSON�`���ɃV���A���C�Y���A�t�@�C���ɕۑ�
        //File.WriteAllText(filePath, JsonUtility.ToJson(_itemVolume, false));
    }
    /// <summary> ���ݑ������Ă��鑕����json�t�@�C������擾���A�����o�[�ϐ��Ɋi�[���鏈���B </summary>
    public void OnLoad_EquippedData_Json()
    {

    }
    /// <summary> ���ݑ������Ă��鑕���̃f�[�^���Ajson�t�@�C���ɕۑ����鏈���B </summary>
    public void OnSave_EquippedData_Json()
    {

    }

    Equipment.EquipmentRarity Get_EquipmentRarity(string str)
    {
        switch (str)
        {
            case "A": return Equipment.EquipmentRarity.A;
            case "B": return Equipment.EquipmentRarity.B;
            case "C": return Equipment.EquipmentRarity.C;
            case "D": return Equipment.EquipmentRarity.D;
            case "E": return Equipment.EquipmentRarity.E;
        }
        Debug.LogError("�s���Ȓl�ł��B");
        return Equipment.EquipmentRarity.ERROR;
    }

    //�ȉ��e�X�g�p�A���ۂɎg���郂�m�Ɣ��f������{�Ԉڍs����B
    /// <summary> �e�X�g�p�X�N���v�g�B(����)�{�^������Ăяo���B����̑������擾����B </summary>
    /// <param name="id"> �擾���鑕����ID </param>
    public void Get_Equipment(int id)
    {
        _haveEquipment._equipments.Add(_equipmentData[id]);
    }

    /// <summary> �e�X�g�p�X�N���v�g�B(����)�{�^������Ăяo���B����̑����������B </summary>
    /// <param name="id"> ���炷������ID </param>
    public void Lost_Equipment(int id)
    {
        _haveEquipment._equipments.Remove(_equipmentData[id]);
    }
}
