using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> 
/// Enemy�̊��N���X :  <br/>
/// �X�e�[�^�X��\���ϐ���A�U������n���\�b�h�A<br/>
/// �m�b�N�o�b�N�n���\�b�h�A��p�ӁB<br/>
/// </summary>
public class EnemyBase : MonoBehaviour, IAttackOnPlayer
{
    //<============= �����o�[�ϐ� =============>//
    //�G�l�~�[���ʂ̃X�e�[�^�X
    [SerializeField] protected EnemyStatus _status;
    public EnemyStatus Status { get => _status; }

    /// <summary> �h���b�v�i�Ɗm���̏����i�[����ϐ��B </summary>
    [Header("�h���b�v�i�Ɗm��"), Tooltip("ID�͔͈͊O�ɂȂ�Ȃ��悤�ɒ��ӂ��Đݒ肵�Ă��������B"), SerializeField]
    protected DropItemAndProbability[] _dropItemAndProbabilities;

    //���̃G�l�~�[�������Ă������
    protected bool _isRight;
    public bool IsRight { get => _isRight; }


    //�v���C���[�̃R���|�[�l���g
    protected Transform _playerPos;

    //���g�̃R���|�[�l���g
    protected SpriteRenderer _spriteRenderer;
    protected Rigidbody2D _rigidBody2d;

    //�F�ύX�p
    /// <summary> �F��ς��邩�ǂ��� </summary>
    protected bool _isColorChange = false;
    /// <summary> �F��ύX���鎞�� </summary>
    const float _colorChangeTime = 0.1f;
    /// <summary> �F��ύX���鎞�� : ���݂̎c�莞�� </summary>
    protected float _colorChangeTimeValue = 0;

    //�m�b�N�o�b�N�֘A
    /// <summary> �m�b�N�o�b�N���� </summary>
    float _knockBackModeTime = 0f;
    protected bool _isMove = true;

    //===== Unity���b�Z�[�W =====//
    protected virtual void Start()
    {
        Initialize_EnemyBase();
    }
    protected virtual void Update()
    {
        Update_Enemy();
    }

    //<============== protected�����o�[�֐� ==============>//
    /// <summary> �S�G�l�~�[�ŋ��ʂ�Enemy�̏������֐��B�p�����Start�֐��ŌĂяo���B </summary>
    /// <returns> ���������� true ��Ԃ��B </returns>
    protected bool Initialize_EnemyBase()
    {
        // �v���C���[��Transform���擾
        _playerPos = GameObject.FindGameObjectWithTag(Constants.PLAYER_TAG_NAME).transform;
        if (_playerPos == null)
        {
            Debug.LogError($"Player��Transform�R���|�[�l���g���擾�ł��܂���ł����B : {gameObject.name}");
            return false;
        }

        // �X�v���C�g�����_���[�R���|�[�l���g���擾����B
        _spriteRenderer = GetComponent<SpriteRenderer>();
        if (_spriteRenderer == null)
        {
            Debug.LogError($"SpriteRenderer�̎擾�Ɏ��s���܂����B : {gameObject.name}");
            return false;
        }
        // Rigidbody2D���擾����B
        _rigidBody2d = GetComponent<Rigidbody2D>();
        if (_rigidBody2d == null)
        {
            Debug.LogError($"Rigidbody2D�̎擾�Ɏ��s���܂����B : {gameObject.name}");
            return false;
        }

        return true;
    }
    /// <summary> �S�G�l�~�[�ŋ��ʂ�Enemy��Update�֐��B�p�����Update�֐��ŌĂяo���B </summary>
    protected void Update_Enemy()
    {
        ChangeColor();
        Move();
    }
    /// <summary> �m�b�N�o�b�N���� </summary>
    /// <param name="knockBackPower"></param>
    protected void StartKnockBack(float knockBackPower)
    {
        _rigidBody2d.velocity = Vector2.zero;
        float diff = PlayerStatusManager.Instance.IsRight ? Constants.LEFT : Constants.RIGHT;
        _rigidBody2d.AddForce((Vector2.up + Vector2.right * diff) * knockBackPower, ForceMode2D.Impulse);
    }
    /// <summary>
    /// �A�C�e�����h���b�v���鏈���B
    /// </summary>
    protected void DropItems()
    {
        // �z��ɓ������I�u�W�F�N�g���A�h���b�v������ɗ��Ƃ����ǂ����𔻒肷��B
        foreach (var i in _dropItemAndProbabilities)
        {
            // null�`�F�b�N
            if (i._dropGameObjectPrefab != null)
            {
                // �m���𔻒肷��
                if (i._probability > Random.Range(0f, 99.99f))
                {
                    // �����ʂ蔲�����ꍇ�A�I�u�W�F�N�g�𐶐����AID���Z�b�g����B
                    var drop = Instantiate(i._dropGameObjectPrefab, transform.position, Quaternion.identity);
                    if (drop.TryGetComponent(out IDrops drops))
                    {
                        drops.SetID(i._iD);
                    }
                }
            }
            else
            {
                Debug.LogError("�A�C�e����ݒ肵�Ă��������I");
            }
        }
    }
    //<============= public�����o�[�֐� =============>//
    //***** �U���q�b�g�֘A *****//
    /// <summary> �v���C���[���炱�̃G�l�~�[�ɑ΂���U�������B : �m�b�N�o�b�N������ </summary>
    /// <param name="playerOffensivePower"> �_���[�W�� </param>
    public void HitPlayerAttack(float playerOffensivePower)
    {
        //���g�̗̑͂����炵�A��莞�� �F���_���p�̐F�ɕς���B
        _status._hitPoint -= playerOffensivePower;//�̗͂��Ȃ��Ȃ������̏���
        if (_status._hitPoint <= 0)
        {
            // �̗͂��Ȃ��Ȃ������̏��������s
            Deth();
            // �A�C�e�����h���b�v����B
            DropItems();
        }
        StartCoroutine(ColorChange());
    }
    /// <summary> �v���C���[���炱�̃G�l�~�[�ɑ΂���U�������B : �m�b�N�o�b�N�L��� </summary>
    /// <param name="playerOffensivePower"> �_���[�W�� </param>
    /// <param name="knockBackTimer"> �m�b�N�o�b�N���� </param>
    public void HitPlayerAttack(float playerOffensivePower, float knockBackTimer, float knockBackPower)
    {
        //���g�̗̑͂����炷�B
        _status._hitPoint -= playerOffensivePower;
        if (_status._hitPoint <= 0)
        {
            // �̗͂��Ȃ��Ȃ������̏��������s
            Deth();
            // �A�C�e�����h���b�v����B
            DropItems();
        }
        //�U�����q�b�g�������Ƃ�\������ׂɈ�莞�ԐF��ύX����B
        StartCoroutine(ColorChange());

        //�w�肳�ꂽ���Ԃ����ړ����~����B
        _knockBackModeTime = (knockBackTimer - _status._weight) > 0f ? (knockBackTimer - _status._weight) : 0f;
        StartCoroutine(MoveStop());
        //�m�b�N�o�b�N�����B
        StartKnockBack((knockBackPower - _status._weight) > 0f ? knockBackPower - _status._weight : 0f);
    }
    public virtual void HitPlayer(Rigidbody2D playerRigidbody2D)
    {
        //�v���C���[��HitPoint�����炷
        PlayerStatusManager.Instance.PlayerHealthPoint -= _status._offensivePower;
        playerRigidbody2D.velocity = Vector2.zero;
        //�v���C���[���m�b�N�o�b�N����
        if (_isRight)
        {
            playerRigidbody2D.AddForce(Vector2.right * _status._blowingPower.x + Vector2.up * _status._blowingPower.y, ForceMode2D.Impulse);
        }
        else
        {
            playerRigidbody2D.AddForce(Vector2.left * _status._blowingPower.x + Vector2.up * _status._blowingPower.y, ForceMode2D.Impulse);
        }
    }


    //<============= �R���[�`�� =============>//
    /// <summary> 
    /// �m�b�N�o�b�N���s�p�R���[�`���B : <br/>
    /// ��莞�� _isMove�ϐ����Afalse�ɂ���B<br/>
    /// </summary>
    IEnumerator MoveStop()
    {
        _isMove = false;
        yield return new WaitForSeconds(_knockBackModeTime);
        _isMove = true;
    }
    /// <summary> �v���C���[����U�����󂯂�ƁA�w�肳�ꂽ���Ԃ����F���ς��B </summary>
    IEnumerator ColorChange()
    {
        _isColorChange = true;
        yield return new WaitForSeconds(_colorChangeTime);
        _isColorChange = false;
    }

    //<============= ���z�֐� =============>//
    /// <summary> Enemy�ړ��p�֐� : �I�[�o�[���C�h�� </summary>
    protected virtual void Move() { }
    /// <summary>
    /// ���̃G�l�~�[�ɑ΂��čU�����q�b�g�������Ƃ����o���邽�߂̃��\�b�h�B<br/>
    /// �F����莞�ԕύX����B<br/>
    /// </summary>
    protected virtual void ChangeColor()
    {
        //�F��ς���K�v������Ες���
        if (_isColorChange)
        {
            _spriteRenderer.color = Color.red;
        }
        //�F�����ɖ߂�
        else
        {
            _spriteRenderer.color = new Color(255, 255, 255, 255);
        }
    }
    /// <summary>
    /// �̗͂��Ȃ��Ȃ������̏��� : <br/>
    /// �I�[�o�[���C�h�B<br/>
    /// ���̂܂܎g�p����ꍇ�͎������g��j������B<br/>
    /// </summary>
    protected virtual void Deth()
    {
        Destroy(gameObject);
    }
}
/// <summary> Enemy�̃X�e�[�^�X��\���^ </summary>
[System.Serializable]
public struct EnemyStatus
{
    /// <summary> �̗� </summary>
    public float _hitPoint;
    /// <summary> �U���� </summary>
    public float _offensivePower;
    /// <summary> ������΂���/���� </summary>
    public Vector2 _blowingPower;
    /// <summary> ������тɂ���/�d�� </summary>
    public float _weight;
    /// <summary> �ړ����x </summary>
    public float _moveSpeed;

    /// <summary> EnemyStatus�̃R���X�g���N�^ </summary>
    /// <param name="hitPoint"> �̗� </param>
    /// <param name="offensivePower"> �U���� </param>
    /// <param name="blowingPowerUp"> ������΂��� : ������̈З� </param>
    /// <param name="blowingPowerRight"> ������΂��� : �E�����̈З� </param>
    /// <param name="weight"> �d�� : ������тɂ��� </param>
    public EnemyStatus(int hitPoint = 1, int offensivePower = 1, int blowingPowerUp = 1, int blowingPowerRight = 1, float weight = 1f, float moveSpeed = 1f)
    {
        _hitPoint = hitPoint;
        _offensivePower = offensivePower;
        _blowingPower = Vector2.up * blowingPowerUp + Vector2.right * blowingPowerRight;
        _weight = weight;
        _moveSpeed = moveSpeed;
    }
}

/// <summary>
/// ���̌^�́A�|����A�󂹂���̂����ׂ��\���́B
/// ���Ƃ��A�C�e���A���Ƃ������A�h���b�v���ϐ��������o�[�Ɏ��B
/// </summary>
[System.Serializable]
public struct DropItemAndProbability
{
    /// <summary> ���Ƃ����m��ID </summary>
    public int _iD;
    /// <summary> ���Ƃ��A�C�e���A���邢�͑����̃v���n�u </summary>
    public GameObject _dropGameObjectPrefab;
    /// <summary> �h���b�v��(%) </summary>
    public float _probability;
}