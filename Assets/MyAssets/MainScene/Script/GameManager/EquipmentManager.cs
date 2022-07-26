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
        Nan = -1,
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
    public struct MyEquipped
    {
        public int _headPartsID;
        public int _torsoPartsID;
        public int _armRightPartsID;
        public int _armLeftPartsID;
        public int _footPartsID;
    }
    /// <summary> �������Ă��鑕�����i�[����\���� </summary>
    public struct HaveEquipped
    {
        /// <summary> �v�f�͑�����ID�B�������Ă��Ȃ����-1�B </summary>
        public int[] _equipmentsID;
    }

    //<=========== �K�v�Ȓl ===========>//
    /// <summary> �S�Ă̑����̏����ꎞ�ۑ����Ă����ϐ� </summary>
    Equipment[] _equipmentData;
    public Equipment[] EquipmentData { get=> _equipmentData; }
    /// <summary> �������Ă��鑕���̔z�� </summary>
    HaveEquipped _haveEquipmentID;
    public HaveEquipped HaveEquipmentID { get => _haveEquipmentID; }
    /// <summary> ���ݑ������Ă��鑕�� </summary>
    MyEquipped _equipped;
    public MyEquipped Equipped { get => _equipped; }

    //<======== �A�T�C�����ׂ��l ========>//

    //<===== �C���X�y�N�^����ݒ肷�ׂ��l =====>//
    [Header("�����̊�{��񂪊i�[���ꂽcsv�t�@�C���ւ̃p�X"), SerializeField] string _equipmentCsvFilePath;
    [Header("�������Ă��鑕���̏�񂪊i�[���ꂽjson�t�@�C���ւ̃p�X"), SerializeField] string _equipmentHaveJsonFilePath;
    [Header("���ݑ������Ă��鑕���̏�񂪊i�[���ꂽjson�t�@�C���ւ̃p�X"), SerializeField] string _equippedJsonFilePath;
    /// <summary> �v���C���[�������ł��鑕���̍ő吔 </summary>
    [Header("�v���C���[�������ł��鑕���̍ő吔"), SerializeField] int _maxHaveVolume;
    /// <summary> �v���C���[�������ł��鑕���̍ő吔 </summary>
    public int MaxHaveValue { get => _maxHaveVolume; set => _maxHaveVolume = value; }

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
        _equippedJsonFilePath = Path.Combine(Application.persistentDataPath, "EquippedFile.json");
        //���̃I�u�W�F�N�g�́A�V�[�����ׂ��ł��f�X�g���C���Ȃ��B
        DontDestroyOnLoad(gameObject);
        //�z��p�̃��������m�ۂ��A-1�ŏ���������B
        _haveEquipmentID._equipmentsID = new int[_maxHaveVolume];
        _equipmentData = new Equipment[(int)EquipmentID.ID_END];

        // �e�X�g�p�R�[�h : �e�L�g�[�ɏ������Ă��邱�Ƃɂ���B
        for (int i = 0; i < _haveEquipmentID._equipmentsID.Length; i++)
        {
            _haveEquipmentID._equipmentsID[i] = i % _equipmentData.Length;
        }

        //�N���X��������
        Initialize_EquipmentBase();

        //Debug.Log("�����֌W�����[�h����B");
        //OnLoad_EquipmentHaveData_Json();
        //OnLoad_EquippedData_Json();
    }
    void Start()
    {

    }

    void Update()
    {
        //K�L�[�����ŃZ�[�u����
        if (Input.GetKeyDown(KeyCode.K))
        {
            Debug.Log("�����֌W���Z�[�u����B");
            OnSave_EquipmentHaveData_Json();
            OnSave_EquippedData_Json();
        }
        //L�L�[�����Ń��[�h����
        if (Input.GetKeyDown(KeyCode.L))
        {
            Debug.Log("�����֌W�����[�h����B");
            OnLoad_EquipmentHaveData_Json();
            OnLoad_EquippedData_Json();
        }
    }

    //<======== ���̃N���X�̏������֘A ========>//
    /// <summary> �������N���X�̏������֐��B(�h����ŌĂяo���B) </summary>
    /// <returns> �������ɐ��������ꍇtrue�A���s������false��Ԃ��B </returns>
    bool Initialize_EquipmentBase()
    {
        //csv���瑕�������擾
        OnLoad_EquipmentData_csv();
        //���݂̒��p���Ă��鑕����������
        _equipped._headPartsID = (int)EquipmentID.Nan;
        _equipped._torsoPartsID = (int)EquipmentID.Nan;
        _equipped._armRightPartsID = (int)EquipmentID.Nan;
        _equipped._armLeftPartsID = (int)EquipmentID.Nan;
        _equipped._footPartsID = (int)EquipmentID.Nan;
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
        _haveEquipmentID = JsonUtility.FromJson<HaveEquipped>(File.ReadAllText(_equipmentHaveJsonFilePath));
        foreach (var i in _haveEquipmentID._equipmentsID)
        {
            if (i == -1) { Debug.Log("���̗v�f�͋�ł��B"); }
            else Debug.Log(_equipmentData[i]._myName);
        }
    }
    /// <summary> �������Ă��鑕���̃f�[�^���Ajson�t�@�C���ɕۑ����鏈���B </summary>
    public void OnSave_EquipmentHaveData_Json()
    {
        Debug.Log("�������Ă��鑕���f�[�^���Z�[�u���܂��I");
        // �������Ă��鑕���f�[�^���AJSON�`���ɃV���A���C�Y���A�t�@�C���ɕۑ�
        File.WriteAllText(_equipmentHaveJsonFilePath, JsonUtility.ToJson(_haveEquipmentID, false));
        foreach (var i in _haveEquipmentID._equipmentsID)
        {
            if (i == -1) { Debug.Log("���̗v�f�͋�ł��B"); }
            else Debug.Log(_equipmentData[i]._myName);
        }
    }
    /// <summary> �������Ă��鑕����json�t�@�C������擾���A�����o�[�ϐ��Ɋi�[���鏈���B </summary>
    public void OnLoad_EquippedData_Json()
    {
        Debug.Log("���ݑ������Ă��鑕���f�[�^�����[�h���܂��I");
        // �O�̂��߃t�@�C���̑��݃`�F�b�N
        if (!File.Exists(_equippedJsonFilePath))
        {
            //�����Ƀt�@�C���������ꍇ�̏���������
            Debug.Log("���ݑ������Ă��鑕���f�[�^��ۑ����Ă���t�@�C����������܂���B");

            //�����𔲂���
            return;
        }
        // JSON�I�u�W�F�N�g���A�f�V���A���C�Y(C#�`���ɕϊ�)���A�l���Z�b�g�B
        _equipped = JsonUtility.FromJson<MyEquipped>(File.ReadAllText(_equippedJsonFilePath));
    }
    /// <summary> ���ݑ������Ă��鑕���̃f�[�^���Ajson�t�@�C���ɕۑ����鏈���B </summary>
    public void OnSave_EquippedData_Json()
    {
        Debug.Log("���ݑ������Ă��鑕���f�[�^���Z�[�u���܂��I");
        // ���ݑ������Ă��鑕���f�[�^���AJSON�`���ɃV���A���C�Y���A�t�@�C���ɕۑ�
        File.WriteAllText(_equippedJsonFilePath, JsonUtility.ToJson(_equipped, false));
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
    public bool Get_Equipment(int id)
    {
        //�����̎擾����
        for (int i = 0; i < _haveEquipmentID._equipmentsID.Length; i++)
        {
            if (i == -1)
            {
                _haveEquipmentID._equipmentsID[i] = id;
                return true;
            }
        }
        return false;
    }

    /// <summary> �e�X�g�p�X�N���v�g�B(����)�{�^������Ăяo���B����̑����������B </summary>
    /// <param name="id"> ���炷������ID </param>
    public bool Lost_Equipment(int id)
    {
        //�����̑r������
        for (int i = 0; i < _haveEquipmentID._equipmentsID.Length; i++)
        {
            if (i == id)
            {
                _haveEquipmentID._equipmentsID[i] = -1;
                return true;
            }
        }
        return false;
    }

    /// <summary> ���ݑ������Ă��鑕����Console�ɕ\������B�e�X�g�p�B </summary>
    public void DrawDebugLog_Equipped()
    {
        Debug.Log(
            "���ݒ��p���Ă��鑕��\n" +
            "���p�[�c : " + _equipped._headPartsID
            + "/" +
            "���p�[�c : " + _equipped._torsoPartsID
            + "/" +
            "�E�r�p�[�c : " + _equipped._armRightPartsID
            + "/" +
            "���r�p�[�c : " + _equipped._armLeftPartsID
            + "/" +
            "���p�[�c : " + _equipped._footPartsID
            );
    }
}
