using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//�v���C���[�̗̑͂Ȃǂ̊�b�������N���X
public class PlayerBasicInformation : MonoBehaviour
{
    AudioSource _hitEnemySound;
    Rigidbody2D _rigidbody2D;

    [Header("Button�Q"), SerializeField] GameObject _button;

    //�e�R���|�[�l���g
    PlayerStateManagement _newPlayerStateManagement;

    //���G�֘A(�q�b�g��̖��G���ԂƂ�)
    float _godModeTime = 1.5f;
    bool _isGodMode = false;

    //�z�o�[����
    public float _hoverValue { get; set; }
    [Tooltip("�ő�K�X��"), SerializeField] private float _maxHealthForHover;
    public float MaxHealthForHover { get => _maxHealthForHover; }

    IEnumerator _waitGodModeIntervalCoroutine = default;

    void Start()
    {
        _hoverValue = MaxHealthForHover;
        //���g�̃R���|�[�l���g���擾����B
        _newPlayerStateManagement = GetComponent<PlayerStateManagement>();
        _hitEnemySound = GetComponent<AudioSource>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        //player �̗̑͂��Ȃ��Ȃ�������ł���
        if (PlayerStatusManager.Instance.PlayerHealthPoint < 1)
        {
            _newPlayerStateManagement.IsDead = true;
            _button.SetActive(true);
        }
    }
    private void OnEnable()
    {
        GameManager.OnPause += OnPause;
        GameManager.OnResume += OnResume;
    }
    private void OnDisable()
    {
        GameManager.OnPause -= OnPause;
        GameManager.OnResume -= OnResume;
    }

    void OnPause()
    {
        if (_waitGodModeIntervalCoroutine != null)
        {
            StopCoroutine(_waitGodModeIntervalCoroutine);
        }
    }
    void OnResume()
    {
        if (_waitGodModeIntervalCoroutine != null)
        {
            StartCoroutine(_waitGodModeIntervalCoroutine);
        }
    }


    //�G�ƐڐG�����Ƃ��A�G�̍U���͕�damage���󂯁A�G��force������֔�΂����
    private void OnCollisionStay2D(Collision2D collision)
    {
        //���G��Ԃł���΍U�����󂯂Ȃ�
        if (!_isGodMode && !_newPlayerStateManagement.IsDead)
        {
            //Enemy�ƐڐG������Enemy��HitPlayer�֐������s����
            if (collision.gameObject.TryGetComponent(out EnemyBase enemy))
            {
                enemy.HitPlayer(_rigidbody2D);
                _newPlayerStateManagement.IsHitEnemy = true;
                _hitEnemySound.Play();
                _waitGodModeIntervalCoroutine = WaitGodModeInterval();
                StartCoroutine(_waitGodModeIntervalCoroutine);
            }
        }
    }

    IEnumerator WaitGodModeInterval()
    {
        float timer = 0f;
        _isGodMode = true;
        while (timer < _godModeTime)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        _isGodMode = false;
        _waitGodModeIntervalCoroutine = null;
    }

    // �����ƐڐG�����Ƃ��̏���
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // God���[�h�ł�"Dead�X�e�[�g"�ł��Ȃ���Ύ��s����B
        if (!_isGodMode && !_newPlayerStateManagement.IsDead)
        {
            if (collision.gameObject.TryGetComponent(out IAttackOnPlayer _enemy))
            {
                _enemy.HitPlayer(_rigidbody2D);
                _newPlayerStateManagement.IsHitEnemy = true;
                _hitEnemySound.Play();
                //��莞�Ԗ��G�ɂ���B
                _waitGodModeIntervalCoroutine = WaitGodModeInterval();
                StartCoroutine(_waitGodModeIntervalCoroutine);
            }
            //�o�b�e���[�ƐڐG�����Ƃ��̏���
            if (collision.gameObject.tag == "Battery")
            {
                PlayerStatusManager.Instance.PlayerHealthPoint = PlayerStatusManager.Instance.ConsequentialPlayerStatus._maxHp;
                AudioSource.PlayClipAtPoint(collision.gameObject.GetComponent<AudioSource>().clip, transform.position);
            }
        }
    }
}
