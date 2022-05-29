using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//�v���C���[�̗̑͂Ȃǂ̊�b�������N���X
public class PlayerBasicInformation : MonoBehaviour
{
    public int _maxHitPoint = 3;

    public int _playerHitPoint = 3;
    ChangePlayerState _changePlayerState;
    PlayerController _playerController;

    float _godModeTime = 1.5f;

    bool _isGodMode = false;

    // Start is called before the first frame update
    void Start()
    {
        _changePlayerState = GetComponent<ChangePlayerState>();
        _playerController = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        //player �̗̑͂��Ȃ��Ȃ�������ł���
        if (_playerHitPoint < 1)
        {
            _playerController.enabled = false;
            _changePlayerState._isDead = true;
        }
    }

    //�G�ƐڐG�����Ƃ��A�G�̍U���͕�damage���󂯁A�G��force������֔�΂����
    private void OnCollisionStay2D(Collision2D collision)
    {
        //���G��Ԃł���΍U�����󂯂Ȃ�
        if (!_isGodMode)
        {
            //Enemy�ƐڐG������Enemy��HitPlayer�֐������s����
            if (collision.gameObject.tag == "Enemy")
            {
                collision.gameObject.GetComponent<EnemyBase>().HitPlayer();
                _changePlayerState._isHitEnemy = true;
                StartCoroutine(GodMode());
            }
        }
    }
    ////�G�ƐڐG�����Ƃ��A�G�̍U���͕�damage���󂯁A�G��force������֔�΂����
    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (!_isGodMode)
    //    {
    //        //Enemy�ƐڐG������Enemy��HitPlayer�֐������s����
    //        if (collision.gameObject.tag == "Enemy")
    //        {
    //            collision.gameObject.GetComponent<EnemyBase>().HitPlayer();
    //            _changePlayerState.isHitEnemy = true;
    //            StartCoroutine(GodMode());
    //        }
    //    }
    //}
    IEnumerator GodMode()
    {
        _isGodMode = true;
        yield return new WaitForSeconds(_godModeTime);
        _isGodMode = false;
    }
}
