using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary> 
/// �S�Ă̑����̏��ƁA
/// �v���C���[���������Ă��鑕���E���p���Ă��鑕�����A�Ǘ�����N���X�B
/// </summary>
public class EquipmentDataBase : MonoBehaviour
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
        /// <summary> ���ɑ������Ă���p�[�c </summary>
        public int _headPartsID;
        /// <summary> ���ɑ������Ă���p�[�c </summary>
        public int _torsoPartsID;
        /// <summary> �E�r�ɑ������Ă���p�[�c </summary>
        public int _armRightPartsID;
        /// <summary> ���r�ɑ������Ă���p�[�c </summary>
        public int _armLeftPartsID;
        /// <summary> ���ɑ������Ă���p�[�c </summary>
        public int _footPartsID;
    }
    /// <summary> �������Ă��鑕�����i�[����\���� </summary>
    public struct HaveEquipped
    {
        /// <summary> �v�f�͑�����ID�B�������Ă��Ȃ����-1�B </summary>
        public int[] _equipmentsID;
    }

    //<=========== �����o�[�ϐ� ===========>//
    /// <summary> �����X�V���ɌĂяo�����f���Q�[�g�ϐ��B </summary>
    public System.Action ReplacedEquipment;
    /// <summary> �S�Ă̑����̏����ꎞ�ۑ����Ă����ϐ� </summary>
    Equipment[] _equipmentData;
    public Equipment[] EquipmentData { get => _equipmentData; }
    /// <summary> �������Ă��鑕���̔z�� </summary>
    HaveEquipped _haveEquipmentID;
    public HaveEquipped HaveEquipmentID { get => _haveEquipmentID; }
    /// <summary> ���ݑ������Ă��鑕�� </summary>
    MyEquipped _equipped;
    public MyEquipped Equipped { get => _equipped; }
    [Header("���ݒ��p���Ă��鑕���̕\�����Ǘ����Ă���N���X"), SerializeField] Draw_NowEquipped _draw_NowEquipped;
    [Header("�����̏㏸�l�̕\�����Ǘ����Ă���N���X"), SerializeField] ManagerOfPossessedEquipment _managerOfPossessedEquipment;

    //<===== �C���X�y�N�^����ݒ肷�ׂ��l =====>//
    [Header("�����̊�{��񂪊i�[���ꂽcsv�t�@�C���ւ̃p�X"), SerializeField] string _equipmentCsvFilePath;
    [Header("�m�F�p : �������Ă��鑕���̏�񂪊i�[���ꂽjson�t�@�C���ւ̃p�X"), SerializeField] string _equipmentHaveJsonFilePath;
    [Header("�m�F�p : ���ݑ������Ă��鑕���̏�񂪊i�[���ꂽjson�t�@�C���ւ̃p�X"), SerializeField] string _equippedJsonFilePath;
    [Header("�v���C���[�������ł��鑕���̍ő吔"), SerializeField] int _maxHaveVolume;

    /// <summary> �C�x���g�V�X�e�� </summary>
    [Header("�C�x���g�V�X�e��"), SerializeField] EventSystem _eventSystem;
    GameObject _beforeSelectedGameObject;

    bool _isTextUpdate = true;
    public bool IsTextUpdate { get => _isTextUpdate; set => _isTextUpdate = value; }

    /// <summary> �v���C���[�������ł��鑕���̍ő吔 </summary>
    public int MaxHaveValue { get => _maxHaveVolume; set => _maxHaveVolume = value; }

    public const int LEFT_ARM = 0;
    public const int RIGHT_ARM = 1;

    //<======�V���O���g���p�^�[���֘A======>//
    //�C���X�^���X
    private static EquipmentDataBase _instance;
    //�C���X�^���X�̃v���p�e�B
    public static EquipmentDataBase Instance
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
    //�v���C�x�[�g�ȃR���X�g���N�^
    private EquipmentDataBase() { }



    //<======= Unity���b�Z�[�W =======>//
    private void Awake()
    {
        //�N���X��������
        Initialize_EquipmentBase();
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

        _isTextUpdate = _beforeSelectedGameObject != _eventSystem.currentSelectedGameObject;
        if (_isTextUpdate)
        {
            _isTextUpdate = false;
            //StartCoroutine(WaitOneFrame_UpdateText());
            ReplacedEquipment();
        }
        _beforeSelectedGameObject = _eventSystem.currentSelectedGameObject;
    }


    //<======== private�����o�[�֐� ========>//
    /// <summary> �������N���X�̏������֐��B(�h����ŌĂяo���B) </summary>
    /// <returns> �������ɐ��������ꍇtrue�A���s������false��Ԃ��B </returns>
    bool Initialize_EquipmentBase()
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
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(gameObject);

        /***** �������Ă��鑕����ۑ����Ă���t�@�C���̃p�X���擾���A�t�@�C�����J���B *****/
        _equipmentHaveJsonFilePath = Path.Combine(Application.persistentDataPath, "HaveEquipmentFile.json");
        _equippedJsonFilePath = Path.Combine(Application.persistentDataPath, "EquippedFile.json");

        /***** �z��p�̃��������m�ۂ���B *****/
        _haveEquipmentID._equipmentsID = new int[_maxHaveVolume];
        _equipmentData = new Equipment[(int)EquipmentID.ID_END];

        // ***** �e�X�g�p�R�[�h ***** // : �e�L�g�[�ɏ������Ă��邱�Ƃɂ���B
        for (int i = 0; i < _haveEquipmentID._equipmentsID.Length; i++)
        {
            _haveEquipmentID._equipmentsID[i] = i % _equipmentData.Length;
        }

        /***** csv�t�@�C�����炷�ׂĂ̑��������擾 *****/
        OnLoad_EquipmentData_csv();

        /***** ���݂̒��p���Ă��鑕���������� *****/
        _equipped._headPartsID = (int)EquipmentID.Nan;
        _equipped._torsoPartsID = (int)EquipmentID.Nan;
        _equipped._armRightPartsID = (int)EquipmentID.Nan;
        _equipped._armLeftPartsID = (int)EquipmentID.Nan;
        _equipped._footPartsID = (int)EquipmentID.Nan;

        /*json�t�@�C�����珊�����Ă��鑕���ƒ��p���Ă��鑕����*/
        //OnLoad_EquipmentHaveData_Json();
        //OnLoad_EquippedData_Json();

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
           Conversion_EquipmentRarity(values[3]), //rarity
           float.Parse(values[4]),
           float.Parse(values[5]),
           float.Parse(values[6]),
           float.Parse(values[7]),
           float.Parse(values[8]),
           float.Parse(values[9]),
           float.Parse(values[10]),
           float.Parse(values[11]),
           float.Parse(values[12]),
           values[13],
           values[14]); break;
                //���p�������擾���ۑ�
                case "Torso":
                    _equipmentData[index] = new TorsoParts(
           (EquipmentID)int.Parse(values[0]),//ID
           Equipment.EquipmentType.TORSO_PARTS,//Type
           values[2],//name
           Conversion_EquipmentRarity(values[3]), //rarity
           float.Parse(values[4]),
           float.Parse(values[5]),
           float.Parse(values[6]),
           float.Parse(values[7]),
           float.Parse(values[8]),
           float.Parse(values[9]),
           float.Parse(values[10]),
           float.Parse(values[11]),
           float.Parse(values[12]),
           values[13],
           values[14]); break;
                //�r�p�������擾���ۑ�
                case "Arm":
                    _equipmentData[index] = new ArmParts(
           (EquipmentID)int.Parse(values[0]),//ID
           Equipment.EquipmentType.ARM_PARTS,//Type
           values[2],//name
           Conversion_EquipmentRarity(values[3]), //rarity
           float.Parse(values[4]),
           float.Parse(values[5]),
           float.Parse(values[6]),
           float.Parse(values[7]),
           float.Parse(values[8]),
           float.Parse(values[9]),
           float.Parse(values[10]),
           float.Parse(values[11]),
           float.Parse(values[12]),
           values[13],
           values[14],
           ArmParts.Get_AttackType(values[15]),
           float.Parse(values[16])); break;
                //���p�������擾���ۑ�
                case "Foot":
                    _equipmentData[index] = new FootParts(
           (EquipmentID)int.Parse(values[0]),//ID
           Equipment.EquipmentType.FOOT_PARTS,//Type
           values[2],//name
           Conversion_EquipmentRarity(values[3]), //rarity
           float.Parse(values[4]),
           float.Parse(values[5]),
           float.Parse(values[6]),
           float.Parse(values[7]),
           float.Parse(values[8]),
           float.Parse(values[9]),
           float.Parse(values[10]),
           float.Parse(values[11]),
           float.Parse(values[12]),
           values[13],
           values[14]); break;
                default: Debug.LogError("�ݒ肳��Ă��Ȃ�EquipmentType�ł��B"); break;
            }
            index++;
        }

    }
    /// <summary> ���p���Ă��鑕���̃X�e�[�^�X�㏸�l���v���C���[�X�e�[�^�X�ɓK�p����B : �S�g </summary>
    void ApplyEquipment_ALL()
    {
        //���Z�b�g����B
        PlayerStatusManager.Instance.Equipment_RisingValue = PlayerStatusManager.PlayerStatus._zero;
        //�����l��K�p����B
        ApplyEquipment_SpecificParts(_equipped._headPartsID);//��
        ApplyEquipment_SpecificParts(_equipped._torsoPartsID);//��
        ApplyEquipment_SpecificParts(_equipped._armLeftPartsID);//���r
        ApplyEquipment_SpecificParts(_equipped._armRightPartsID);//�E�r
        ApplyEquipment_SpecificParts(_equipped._footPartsID);//��
    }
    /// <summary> ���p���Ă��鑕���̃X�e�[�^�X�㏸�l���v���C���[�X�e�[�^�X�ɓK�p����B </summary>
    /// <param name="equipmentID"> �K�p���鑕����ID </param>
    void ApplyEquipment_SpecificParts(int equipmentID)
    {
        if (equipmentID >= 0)
        {
            PlayerStatusManager.Instance.Equipment_RisingValue += EquipmentData[equipmentID].ThisEquipment_StatusRisingValue;
        }
        else
        {
            Debug.LogWarning("�������̉ӏ��͂���܂���?�����łȂ���΃G���[�ł�! : �����}�l�[�W���[�R���|�[�l���g���");
        }
    }

    //<===== public�����o�[�֐� =====>//
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
    /// <summary> string �� enum �ɕϊ����� </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    Equipment.EquipmentRarity Conversion_EquipmentRarity(string str)
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
    /// <summary> �������Ă��鑕���ƁA���p���Ă��鑕������������B </summary>
    /// <param name="fromNowEquipmentID"> ���ꂩ�瑕�����鑕����ID </param>
    /// <param name="fromNowEquipmentType"> ���ꂩ�瑕�����鑕����Type </param>
    /// <param name="armFlag"> �ǂ���̘r�������邩���f����l�A0�Ȃ獶�r�A1�Ȃ�E�r�B </param>
    public void Swap_HaveToEquipped(int fromNowEquipmentID, Equipment.EquipmentType fromNowEquipmentType, EquipmentButton button, int armFlag = -1)
    {
        Debug.Log("���ꂩ�璅�p���鑕����ID : " + fromNowEquipmentID);
        Debug.Log("���ꂩ�璅�p���鑕����Type : " + fromNowEquipmentType);

        //�����ɑ�������������R�[�h������

        int temporary = -1;
        //Type����ɒ��p����
        //�r�ȊO�̏ꍇ
        if (fromNowEquipmentType != Equipment.EquipmentType.ARM_PARTS)
        {
            switch (fromNowEquipmentType)
            {
                //���p�[�c�̏ꍇ
                case Equipment.EquipmentType.HEAD_PARTS:
                    temporary = _equipped._headPartsID;
                    _equipped._headPartsID = fromNowEquipmentID;
                    break;

                //���p�[�c�̏ꍇ
                case Equipment.EquipmentType.TORSO_PARTS:
                    temporary = _equipped._torsoPartsID;
                    _equipped._torsoPartsID = fromNowEquipmentID;
                    break;

                //���p�[�c�̏ꍇ
                case Equipment.EquipmentType.FOOT_PARTS:
                    temporary = _equipped._footPartsID;
                    _equipped._footPartsID = fromNowEquipmentID;
                    break;
            }
            //���E�����������C���x���g���Ɋi�[����
            if (temporary != -1) button.Set_Equipment(EquipmentData[temporary]);
            else button.Set_Equipment(null);
            //�\�����X�V����
            _draw_NowEquipped.Update_Equipped(fromNowEquipmentType);
            if (temporary != -1) _managerOfPossessedEquipment.Update_RiseValueText(EquipmentData[temporary]);
        }
        //�r�̏ꍇ
        else
        {
            if (armFlag == 0)
            {
                temporary = _equipped._armLeftPartsID;
                _equipped._armLeftPartsID = fromNowEquipmentID;
            }
            else if (armFlag == 1)
            {
                temporary = _equipped._armRightPartsID;
                _equipped._armRightPartsID = fromNowEquipmentID;
            }
            else
            {
                Debug.LogError($"�s���Ȓl�ł�{armFlag}");
            }
            //���E�����������C���x���g���Ɋi�[����
            if (temporary != -1) button.Set_Equipment(EquipmentData[temporary]);
            else button.Set_Equipment(null);
            //�\�����X�V����
            _draw_NowEquipped.Update_Equipped(fromNowEquipmentType, armFlag);
            if (temporary != -1) _managerOfPossessedEquipment.Update_RiseValueText(EquipmentData[temporary]);
        }
        ApplyEquipment_ALL(); 
        ReplacedEquipment();
        //�ȉ��v�C��
        Debug.Log("���p����������ID : " + fromNowEquipmentType);
        Debug.Log("���p����������ID : " + fromNowEquipmentID);
    }


    //<===== �ȉ��e�X�g�p�A���ۂɎg���郂�m�Ɣ��f������{�Ԉڍs����B =====>//
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

    /// <summary> �I�𒆂̑����̃X�e�[�^�X�㏸�l�̍����擾����B </summary>
    /// <returns> ���݂̑����X�e�[�^�X�ƁA�I�𒆂̃p�[�c�𑕔����邱�Ƃɂ��X�e�[�^�X�̍� </returns>
    public PlayerStatusManager.PlayerStatus Get_SelectedStatusDifference(Equipment selectedEquipment, bool armFlag)
    {
        //���݂̃X�e�[�^�X���擾����B
        PlayerStatusManager.PlayerStatus result = PlayerStatusManager.Instance.ConsequentialPlayerStatus;
        //�I�𒆂̑����̎�ނ��擾����B
        Equipment.EquipmentType type = selectedEquipment._myType;

        return result;
    }

    IEnumerator WaitOneFrame_UpdateText()
    {
        yield return null;
        ReplacedEquipment();
    }
}
