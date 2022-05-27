using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    //player との距離で判定する
    [SerializeField] Transform _playerTransform;
    float _diffX = 0f;
    float _diffY = 0f;

    //生成すべきエネミー
    GameObject _child;

    [SerializeField, Tooltip("Gizmo表示")] bool _isGizmo = true;

    private void Start()
    {
        _child = transform.GetChild(0).gameObject;
    }

    //固定フレームで呼ばれる
    private void FixedUpdate()
    {
        _diffX = transform.position.x - _playerTransform.position.x;
        _diffY = transform.position.y - _playerTransform.position.y;

        //プレイヤーとエネミーが一定距離近づいたら、子オブジェクトをアクティブにし、このコンポーネントを非アクティブにする。
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
