using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slash : MonoBehaviour
{
    //�e�p�����[�^
    [SerializeField] int _slashOffensivePower;//�G�l�~�[�ɑ΂���U����
    [SerializeField] Vector2 _knockBackPower;//�m�b�N�o�b�N�p���[
    [SerializeField] float _knockBackTimer;//�m�b�N�o�b�N�^�C�}�[

    [SerializeField] AudioClip _hitSlash;
    [SerializeField] AudioClip _slash;
    bool _isRigth;//�v���C���[�Ɠ���������������p

    [SerializeField] float _soundVolume;

    //�v���C���[�̃R���|�[�l���g
    Transform _playerPos;
    SpriteRenderer _playerSpriteRendere;

    //���g�̃R���|�[�l���g
    CapsuleCollider2D _capsuleCollider2D;
    SpriteRenderer _mySpriteRendere;

    void Update()
    {
        transform.position = _playerSpriteRendere.flipX ? _playerPos.position + Vector3.left * 1.5f : _playerPos.position + Vector3.right * 1.5f;
        //�v���C���[���U���������a���͏�����
        if (_mySpriteRendere.flipX != _playerSpriteRendere.flipX)
        {
            gameObject.SetActive(false);
        }
    }

    //�a�������������A�A�j���[�V�����C�x���g����Ăяo��
    public void DestroyObject()
    {
        gameObject.SetActive(false);
    }

    //�G�ƐڐG�����Ƃ��ɍs������
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out EnemyBase enemy))
        {
            AudioSource.PlayClipAtPoint(_hitSlash, transform.position, _soundVolume);
            enemy.HitPlayerAttack(_slashOffensivePower);
        }
        if (collision.TryGetComponent(out BossBase boss))
        {
            AudioSource.PlayClipAtPoint(_hitSlash, transform.position, _soundVolume);
            boss.HitPlayerAttack(_slashOffensivePower);
        }
    }

    private void OnEnable()
    {
        if (_playerPos == null)
        {
            _playerPos = transform.root.gameObject.GetComponent<Transform>();
            _playerSpriteRendere = transform.root.gameObject.GetComponent<SpriteRenderer>();

            //�����̃R���|�[�l���g���擾����B
            _capsuleCollider2D = GetComponent<CapsuleCollider2D>();
            _mySpriteRendere = GetComponent<SpriteRenderer>();
        }

        _mySpriteRendere.flipX = _playerSpriteRendere.flipX;
        transform.position = _playerSpriteRendere.flipX ? _playerPos.position + Vector3.left * 1.5f : _playerPos.position + Vector3.right * 1.5f;
        if (_mySpriteRendere.flipX)
        {
            _capsuleCollider2D.offset = new Vector2(-0.3f, 0.15f);
        }

        //���ˉ����Đ�����
        AudioSource.PlayClipAtPoint(_slash, transform.position, _soundVolume);
    }
}
