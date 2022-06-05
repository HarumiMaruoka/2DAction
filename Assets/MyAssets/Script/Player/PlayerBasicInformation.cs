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
    NewPlayerStateManagement _newPlayerStateManagement;

    //���G�֘A(�q�b�g��̖��G���ԂƂ�)
    float _godModeTime = 1.5f;
    bool _isGodMode = false;

    //�z�o�[����
    [SerializeField] public float _hoverValue;
    public float _maxHealthForHover { get; } = 300f;

    // Start is called before the first frame update
    void Start()
    {
        _hoverValue = _maxHealthForHover;
        //���g�̃R���|�[�l���g���擾����B
        _newPlayerStateManagement = GetComponent<NewPlayerStateManagement>();
    }

    // Update is called once per frame
    void Update()
    {
        //player �̗̑͂��Ȃ��Ȃ�������ł���
        if (_playerHitPoint < 1)
        {
            _newPlayerStateManagement._isDead = true;
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
                _newPlayerStateManagement._isHitEnemy = true;
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
