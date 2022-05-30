using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//�v���C���[�̗̑͂Ȃǂ̊�b�������N���X
public class PlayerBasicInformation : MonoBehaviour
{
    //��{�p�����[�^
    [SerializeField] public int _maxHitPoint = 3;//�ő�HP
    [SerializeField] public int _playerHitPoint = 3;//���݂�HP

    //�e�R���|�[�l���g
    PlayerAnimationManagement _changePlayerState;
    PlayerController _playerController;

    //���G�֘A(�q�b�g��̖��G���ԂƂ�)
    float _godModeTime = 1.5f;
    bool _isGodMode = false;

    // Start is called before the first frame update
    void Start()
    {
        //���g�̃R���|�[�l���g���擾����B
        _changePlayerState = GetComponent<PlayerAnimationManagement>();
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
            if (collision.gameObject.TryGetComponent(out EnemyBase enemy))
            {
                enemy.HitPlayer();
                _changePlayerState._isHitEnemy = true;
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
}
