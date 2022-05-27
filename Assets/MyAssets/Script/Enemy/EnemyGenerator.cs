using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    //player �Ƃ̋����Ŕ��肷��
    [SerializeField] Transform _playerTransform;
    float _diffX = 0f;
    float _diffY = 0f;

    //�������ׂ��G�l�~�[
    GameObject _child;

    [SerializeField, Tooltip("Gizmo�\��")] bool _isGizmo = true;

    private void Start()
    {
        _child = transform.GetChild(0).gameObject;
    }

    //�Œ�t���[���ŌĂ΂��
    private void FixedUpdate()
    {
        _diffX = transform.position.x - _playerTransform.position.x;
        _diffY = transform.position.y - _playerTransform.position.y;

        //�v���C���[�ƃG�l�~�[����苗���߂Â�����A�q�I�u�W�F�N�g���A�N�e�B�u�ɂ��A���̃R���|�[�l���g���A�N�e�B�u�ɂ���B
        if (Mathf.Abs(_diffX) < 10f && Mathf.Abs(_diffY) < 6.5f)
        {
            _child.SetActive(true);
            enabled = false;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (_isGizmo)
        {
            Gizmos.DrawCube(transform.position, new Vector3(1,1,0));
        }
    }


}
