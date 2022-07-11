using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

//���̃N���X�ł͍��̂Ƃ���A�A�C�e���̏������Ƒ��������Ǘ�����
public class PlayerManager : MonoBehaviour
{
    //���̃N���X�̓V���O���g���p�^�[�����g�p�������̂ł���B
    //�C���X�^���X�𐶐�
    private static PlayerManager _instance;
    //�C���X�^���X���͓ǂݎ���p
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


    /// <summary> �������Ă��鑕���̏�� </summary>
    /// �����Ɋւ��鏈���������ɏ���

    /// <summary> �������Ă����v�ȃA�C�e���̏�� </summary>
    /// �������Ă����v�ȃA�C�e���̏��Ɋւ��鏈���������ɏ���


    //�e�p�����[�^
    [Header("�v���C���[�̍ő�̗�"), SerializeField] public float _maxPlayerHealthPoint;
    public float MaxPlayerHealthPoint { get => _maxPlayerHealthPoint; }

    [Header("�v���C���[�̗̑�"), SerializeField] float _playerHealthPoint;
    public float PlayerHealthPoint { get => _playerHealthPoint; set { Debug.Log("�v���C���[�̗̑͂̒l��ύX���܂���"); _playerHealthPoint = value; } }

    [Header("�v���C���[�̍ő�X�^�~�i"), SerializeField] float _playerMaxStamina;
    public float PlayerMaxStamina { get => _playerMaxStamina; set => _playerMaxStamina = value; }

    [Header("�v���C���[�̃X�^�~�i"), SerializeField] float _playerStamina;
    public float PlayerStamina { get => _playerStamina; set => _playerStamina = value; }

    [Header("�v���C���[�̋ߋ����U����"), SerializeField] float _playerOffensivePower_ShortDistance;
    public float PlayerOffensivePower_ShortDistance { get => _playerOffensivePower_ShortDistance; set { Debug.Log("�U���͂̒l��ύX���܂���"); _playerOffensivePower_ShortDistance = value; } }

    [Header("�v���C���[�̉������U����"), SerializeField] float _playerOffensivePower_LongDistance;
    public float PlayerOffensivePower_LongDistance { get => _playerOffensivePower_LongDistance; set { Debug.Log("�U���͂̒l��ύX���܂���"); _playerOffensivePower_LongDistance = value; } }

    [Header("�v���C���[�̖h���"), SerializeField] float _playerDefensePower;
    public float PlayerDefensePower { get => _playerDefensePower; set { Debug.Log("�h��͂̒l��ύX���܂���"); _playerDefensePower = value; } }

    [Header("�v���C���[�̈ړ���"), SerializeField] float _playerMoveSpeed;
    public float PlayerMoveSpeed { get => _playerMoveSpeed; set { Debug.Log("�v���C���[�̈ړ��͂̒l��ύX���܂���"); _playerMoveSpeed = value; } }


    /// <summary> �v���C���[�������Ă������ </summary>
    private bool _isRight;
    public bool IsRight { get => _isRight; }

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

        //�z�񕪂̗̈���m�ۂ���B
        _itemVolume._itemNumberOfPossessions = new int[(int)Item.ItemID.ITEM_ID_END];
    }

    void Start()
    {
        //���̃I�u�W�F�N�g�́A�V�[�����ׂ��ł��f�X�g���C���Ȃ��B
        DontDestroyOnLoad(gameObject);
        // �A�C�e�����������A�ۑ����Ă���t�@�C���̃p�X���擾���t�@�C�����J���B
        _itemFilePath = Path.Combine(Application.persistentDataPath, "ItemNumberOfPossessionsFile.json");
        // �A�C�e�����������A�t�@�C������擾����B
        OnLoad_ItemNumberOfPossessions(_itemFilePath);
        Debug.Log("PlayerManager�̏������ɐ������܂����B");
    }

    //Update�ł̓e�X�g�ŃA�C�e�����������Z�[�u�����胍�[�h�����肷�鏈���������Ă���B
    private void Update()
    {
        //�v���C���[�������Ă��������ۑ�����
        if (Input.GetAxisRaw("Horizontal") < 0)
        {
            _isRight = true;
        }
        if (Input.GetAxisRaw("Horizontal") > 0)
        {
            _isRight = false;
        }

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
        if (itemID < (int)Item.ItemID.ITEM_ID_00 || itemID >= (int)Item.ItemID.ITEM_ID_END)
        {
            //�C���f�b�N�X����O�Ȃ珈���𔲂���
            Debug.LogError("�����ȃA�C�e��ID�ł��B");
            return;
        }
        ItemVolume._itemNumberOfPossessions[itemID] += volume;
    }

    /// <summary> �A�C�e���̏��������t�@�C�����烍�[�h </summary>
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

    /// <summary> �A�C�e���̏��������t�@�C���ɃZ�[�u </summary>
    void OnSave_ItemNumberOfPossessions(string filePath)
    {
        Debug.Log("�A�C�e�����������Z�[�u���܂��I");
        // �A�C�e���������f�[�^���AJSON�`���ɃV���A���C�Y���A�t�@�C���ɕۑ�
        File.WriteAllText(filePath, JsonUtility.ToJson(_itemVolume, false));
    }
}
