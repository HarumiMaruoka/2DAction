using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//�v���C���[�̈ړ��Ɋւ���N���X
public class PlayerController : MonoBehaviour
{
    public float move_speed_x;
    float previous_move_speed;
    const float dash_speed = 1.6f;
    public float jump_power;
    Rigidbody2D _rigidbody2D;
    Jump_Script jump_script;
    PlayerAnimationManagement _playerAnimationManagement;
    PlayerBasicInformation _playerBasicInformation;

    [SerializeField] GameObject _burrettPrefab;

    [Tooltip("�E�N���b�N�U�� �ꌂ�ڂ̃v���n�u")]
    [SerializeField] GameObject _slashPrefabOne;

    [Tooltip("�E�N���b�N�U�� �񌂖ڂ̃v���n�u")]
    [SerializeField] GameObject _slashPrefabTwo;

    GameObject _temporary;//���̓��ꕨ
    //[SerializeField] GameObject _slashPrefabTow;
    float _burrettCoolTime;
    float _slashCoolTimeOne;
    float _slashCoolTimeTow;

    [SerializeField] float _hoverPower;

    [SerializeField] float _gasValue;


    // Start is called before the first frame update
    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        jump_script = GetComponent<Jump_Script>();
        _playerAnimationManagement = GetComponent<PlayerAnimationManagement>();
        _playerBasicInformation = GetComponent<PlayerBasicInformation>();
        _burrettCoolTime = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (_playerAnimationManagement._isMove)//�s���\�ł���Ύ��s����
        {
            //���͂��擾����
            float h = Input.GetAxisRaw("Horizontal");//������
            float v = Input.GetAxisRaw("Vertical");//�c����

            //���ړ�
            if (v == 0 && h != 0)//�c�̓��͂����鎞�͉��ړ��ł��Ȃ�
            {
                if (Input.GetButton("Dash"))
                {
                    _rigidbody2D.AddForce(Vector2.right * h * move_speed_x * Time.deltaTime * dash_speed, ForceMode2D.Force);
                }
                else
                {
                    _rigidbody2D.AddForce(Vector2.right * h * move_speed_x * Time.deltaTime, ForceMode2D.Force);
                }
            }

            //�W�����v
            if (v == 0)
            {
                jump_script.Jump(jump_power);
            }

            //�z�o�[
            HoverManagement();

            //�V���b�g
            if (v == 0 && !_playerAnimationManagement._isHover)//�c�̓��͂����鎞�͑łĂȂ��A�z�o�[�����łĂȂ�
            {
                if (_burrettCoolTime < 0f)
                {
                    if (Input.GetButton("Fire1"))
                    {
                        Instantiate(_burrettPrefab, Vector3.zero, Quaternion.identity);
                        _burrettCoolTime = 0.15f;
                    }
                }
                else if (_burrettCoolTime >= 0f)
                {
                    _burrettCoolTime -= Time.deltaTime;
                }
            }

            //�X���b�V��
            if (v == 0 && !_playerAnimationManagement._isHover)//�c�̓��͂����鎞�͑łĂȂ��A�z�o�[�����łĂȂ�
            {
                //�񌂖ڔ����������ł���Ȃ甭������
                if (_slashCoolTimeTow < 0f && _slashCoolTimeOne > 0f)
                {
                    if (Input.GetButtonDown("Fire2"))
                    {
                        Destroy(_temporary);
                        Instantiate(_slashPrefabTwo, Vector3.zero, Quaternion.identity);
                        _slashCoolTimeTow = 0.35f;
                        _slashCoolTimeOne = 0.35f;
                    }
                }

                //�ꌂ��
                if (_slashCoolTimeOne < 0f)
                {
                    if (Input.GetButtonDown("Fire2"))
                    {
                        _temporary = Instantiate(_slashPrefabOne, Vector3.zero, Quaternion.identity);
                        _slashCoolTimeOne = 0.35f;
                    }
                }

                //�N�[���^�C������
                if (_slashCoolTimeOne >= 0f)
                {
                    _slashCoolTimeOne -= Time.deltaTime;
                }
                if (_slashCoolTimeTow >= 0f)
                {
                    _slashCoolTimeTow -= Time.deltaTime;
                }
            }

            v = 100f;
        }
    }

    /// <summary> �z�o�[�Ǘ��p�֐� </summary>
    void HoverManagement()//�z�o�[�Ǘ�
    {
        //�z�o�[���Ă���Ƃ��̏���
        if (_playerAnimationManagement._isHover)
        {
            //�z�o�[�p�̗͂�0���傫�����
            if (_playerBasicInformation._hoverValue > 0f)
            {
                //�㏸�����A�K�X�������
                _rigidbody2D.velocity = Vector2.up * _hoverPower;
                _playerBasicInformation._hoverValue -= Time.deltaTime * _gasValue;
            }
        }
        //�z�o�[���ĂȂ��Ƃ��̏���
        else
        {
            //�̗͂��ő�ł����
            if (_playerBasicInformation._maxHealthForHover <= _playerBasicInformation._hoverValue)
            {
                _playerBasicInformation._hoverValue = _playerBasicInformation._maxHealthForHover;
            }
            //�̗͂��ő�ł͂Ȃ����
            else
            {
                _playerBasicInformation._hoverValue += Time.deltaTime * _gasValue;
            }
        }
    }
}
