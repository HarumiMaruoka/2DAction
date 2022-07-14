using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//�v���C���[�̗̑͂Ȃǂ̊�b�������N���X
public class PlayerBasicInformation : MonoBehaviour
{
    //��{�p�����[�^
    [SerializeField] public int _maxHitPoint = 3;//�ő�HP
    [SerializeField] public int _playerHitPoint = 3;//���݂�HP

    AudioSource _hitEnemySound;

    [Header("Botton�Q"), SerializeField] GameObject _botton;

    //�e�R���|�[�l���g
    PlayerStateManagement _newPlayerStateManagement;

    //���G�֘A(�q�b�g��̖��G���ԂƂ�)
    float _godModeTime = 1.5f;
    bool _isGodMode = false;

    //�z�o�[����
    public float _hoverValue { get; set; }
    [Tooltip("�ő�K�X��"), SerializeField] private float _maxHealthForHover;
    public float MaxHealthForHover { get => _maxHealthForHover; }

    // Start is called before the first frame update
    void Start()
    {
        _hoverValue = MaxHealthForHover;
        //���g�̃R���|�[�l���g���擾����B
        _newPlayerStateManagement = GetComponent<PlayerStateManagement>();
        _hitEnemySound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //player �̗̑͂��Ȃ��Ȃ�������ł���
        if (PlayerStatusManager.Instance.PlayerHealthPoint < 1)
        {
            _newPlayerStateManagement._isDead = true;
            _botton.SetActive(true);
        }
    }

    //�G�ƐڐG�����Ƃ��A�G�̍U���͕�damage���󂯁A�G��force������֔�΂����
    private void OnCollisionStay2D(Collision2D collision)
    {
        //���G��Ԃł���΍U�����󂯂Ȃ�
        if (!_isGodMode && !_newPlayerStateManagement._isDead)
        {
            //Enemy�ƐڐG������Enemy��HitPlayer�֐������s����
            if (collision.gameObject.TryGetComponent(out EnemyBase enemy))
            {
                enemy.HitPlayer();
                _newPlayerStateManagement._isHitEnemy = true;
                _hitEnemySound.Play();
                StartCoroutine(GodMode());
            }
            if (collision.gameObject.TryGetComponent(out BossBase boss))
            {
                boss.HitPlayer();
                _newPlayerStateManagement._isHitEnemy = true;
                _hitEnemySound.Play();
                StartCoroutine(GodMode());
            }
        }
    }

    IEnumerator GodMode()
    {
        _isGodMode = true;
        yield return new WaitForSeconds(_godModeTime);
        _isGodMode = false;
    }

    //�{�X�ƐڐG�����Ƃ��̏���
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!_isGodMode && !_newPlayerStateManagement._isDead)
        {
            if (collision.gameObject.TryGetComponent(out SpellController spell))
            {
                spell.HitPlayer();
                _newPlayerStateManagement._isHitEnemy = true;
                _hitEnemySound.Play();
                StartCoroutine(GodMode());
            }
            if (collision.gameObject.TryGetComponent(out BossAttack bossAttack))
            {
                bossAttack.HitPlayer();
                _newPlayerStateManagement._isHitEnemy = true;
                _hitEnemySound.Play();
                StartCoroutine(GodMode());
            }
            //�o�b�e���[�ƐڐG�����Ƃ��̏���
            if (collision.gameObject.tag == "Battery")
            {
                PlayerStatusManager.Instance.PlayerHealthPoint = PlayerStatusManager.Instance.PlayerMaxHealthPoint;
                AudioSource.PlayClipAtPoint(collision.gameObject.GetComponent<AudioSource>().clip, transform.position);
            }
        }
    }
}
