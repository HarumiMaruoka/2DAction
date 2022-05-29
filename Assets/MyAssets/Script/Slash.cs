using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slash : MonoBehaviour
{
    [SerializeField] int _slashPower;
    Transform _playerPos;
    SpriteRenderer _playerSpriteRendere;
    CapsuleCollider2D _capsuleCollider2D;
    SpriteRenderer _mySpriteRendere;

    [SerializeField] Vector2 _knockBackPower;
    [SerializeField] float _knockBackTimer;

    bool _isRigth;

    bool _isEnemyKnockBack;

    // Start is called before the first frame update
    void Start()
    {
        _playerPos = GameObject.Find("ChibiRobo").GetComponent<Transform>();
        _playerSpriteRendere = GameObject.Find("ChibiRobo").GetComponent<SpriteRenderer>();
        _capsuleCollider2D = GetComponent<CapsuleCollider2D>();
        _mySpriteRendere = GetComponent<SpriteRenderer>();

        _mySpriteRendere.flipX = _playerSpriteRendere.flipX;
        transform.position = _playerSpriteRendere.flipX ? _playerPos.position + Vector3.left * 1.5f : _playerPos.position + Vector3.right * 1.5f;
        if (_mySpriteRendere.flipX)
        {
            _capsuleCollider2D.offset = new Vector2(-0.1f, 0.1f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = _playerSpriteRendere.flipX ? _playerPos.position + Vector3.left * 1.2f : _playerPos.position + Vector3.right * 1.2f;
        //�v���C���[���U���������a���͏�����
        if (_mySpriteRendere.flipX != _playerSpriteRendere.flipX)
        {
            Destroy(this.gameObject);
        }
    }

    //�a�������������A�A�j���[�V�����C�x���g����Ăяo��
    public void DestroyObject()
    {
        Destroy(this.gameObject);
    }

    //�G�ƐڐG�����Ƃ��ɍs������
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //GetComponent�̂��y���Z��������
        if (collision.TryGetComponent(out EnemyBase enemy))
        {
            enemy.HitPlayerAttadk(_slashPower, _knockBackTimer);
        }
       }
}
