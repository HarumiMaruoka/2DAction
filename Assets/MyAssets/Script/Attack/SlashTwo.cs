using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashTwo : MonoBehaviour
{
    //�e�p�����[�^
    [SerializeField] int _slashOffensivePower;//�G�l�~�[�ɑ΂���U����
    [SerializeField] Vector2 _knockBackPower;//�m�b�N�o�b�N�p���[
    [SerializeField] float _knockBackTimer;//�m�b�N�o�b�N�^�C�}�[
    bool _isRigth;//�v���C���[�Ɠ���������������p

    //�v���C���[�̃R���|�[�l���g
    Transform _playerPos;
    SpriteRenderer _playerSpriteRendere;

    //���g�̃R���|�[�l���g
    CapsuleCollider2D _capsuleCollider2D;
    SpriteRenderer _mySpriteRendere;


    // Start is called before the first frame update
    void Start()
    {
        //�v���C���[�̃R���|�[�l���g���擾����B
        _playerPos = GameObject.Find("ChibiRobo").GetComponent<Transform>();
        _playerSpriteRendere = GameObject.Find("ChibiRobo").GetComponent<SpriteRenderer>();

        //�����̃R���|�[�l���g���擾����B
        _capsuleCollider2D = GetComponent<CapsuleCollider2D>();
        _mySpriteRendere = GetComponent<SpriteRenderer>();

        //Slash�̌����A�ʒu�𒲐�����B
        _mySpriteRendere.flipX = _playerSpriteRendere.flipX;
        transform.position = _playerSpriteRendere.flipX ? _playerPos.position + Vector3.left * 1.5f : _playerPos.position + Vector3.right * 1.5f;
        if (_mySpriteRendere.flipX)
        {
            _capsuleCollider2D.offset = new Vector2(-0.3f, 0.15f);
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
            enemy.HitPlayerAttadk(_slashOffensivePower, _knockBackTimer);
        }
    }
}
