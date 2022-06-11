using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastController : MonoBehaviour
{
    //�u�����W���[�ƃv���C���[�̃g�����X�t�H�[��
    Transform _bringerTransForm;
    Transform _playerTransForm;

    //���g�̃R���|�[�l���g
    Animator _animator;
    BoxCollider2D _collider2D;

    void Start()
    {
        _bringerTransForm = transform.parent.gameObject.transform;
        _playerTransForm = GameObject.Find("ChibiRobo").transform;

        _animator = GetComponent<Animator>();
        _collider2D = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        UpdateCast();
    }

    void UpdateCast()
    {
        //�����ɁABringer���������U������Ƃ��̏���������
    }

    /// <summary> �L���X�g�̔������̏��� </summary>
    void PlayCast()
    {
        //�A�j���[�V�����Đ�
        _animator.SetTrigger("Cast");
    }
}
