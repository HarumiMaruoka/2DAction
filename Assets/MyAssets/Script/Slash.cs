using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slash : MonoBehaviour
{
    [SerializeField] int _slashPower;
    Transform _playerPos;
    SpriteRenderer _playerSpriteRendere;

    SpriteRenderer _mySpriteRendere;

    bool _isRigth;

    // Start is called before the first frame update
    void Start()
    {
        _playerPos = GameObject.Find("ChibiRobo").GetComponent<Transform>();
        _playerSpriteRendere = GameObject.Find("ChibiRobo").GetComponent<SpriteRenderer>();

        _mySpriteRendere = GetComponent<SpriteRenderer>();

        _mySpriteRendere.flipX = _playerSpriteRendere.flipX;
        transform.position = _playerSpriteRendere.flipX ? _playerPos.position + Vector3.left : _playerPos.position + Vector3.right;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = _playerPos.position;
        //�v���C���[���U���������a���͏�����
        if(_mySpriteRendere.flipX != _playerSpriteRendere.flipX)
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
        if (collision.gameObject.tag == "Enemy")
        {
            //�����ɓG�ƐڐG�����Ƃ��̏���������
            collision.gameObject.GetComponent<EnemyBase>().HitPlayerAttadk(_slashPower);
        }
    }
}
