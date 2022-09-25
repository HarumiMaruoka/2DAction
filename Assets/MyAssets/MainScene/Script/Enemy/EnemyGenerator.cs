using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    //player �Ƃ̋����Ŕ��肷��
    Transform _playerTransform;
    float _diffX = 0f;
    float _diffY = 0f;
    bool _isGenerat = false;

    //�������ׂ��G�l�~�[
    GameObject _child;

    //Gizmo�\��
    [Header("Gizmo�֘A")]
    [SerializeField, Tooltip("Gizmo�\��")] 
    bool _isGizmo = true;
    [SerializeField, Tooltip("Gizmo�̐F")]
    Color _gizmoColor;

    //Start�Ŏq�I�u�W�F�N�g���擾����B
    private void Start()
    {
        _playerTransform = GameObject.Find("ChibiRobo").transform;
        _child = transform.GetChild(0).gameObject;
    }

    //�v���C���[�ƃG�l�~�[����苗���߂Â�����A�q�I�u�W�F�N�g���A�N�e�B�u�ɂ��A���̃R���|�[�l���g���A�N�e�B�u�ɂ���B
    private void FixedUpdate()
    {
        _diffX = transform.position.x - _playerTransform.position.x;
        _diffY = transform.position.y - _playerTransform.position.y;

        if (Mathf.Abs(_diffX) < 12.5f && Mathf.Abs(_diffY) < 7f && !_isGenerat)
        {
            _child.SetActive(true);
            _isGenerat = true;
        }
    }

    //���̃I�u�W�F�N�g�̈ʒu�����₷���悤�ɕ\������B
    private void OnDrawGizmos()
    {
        Gizmos.color = _gizmoColor;
        if (_isGizmo)
        {
            Gizmos.DrawCube(transform.position, new Vector3(1, 1, 0));
        }
    }
}
