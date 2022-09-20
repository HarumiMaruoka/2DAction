using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> Enemy : Stomperのコンポーネント </summary>
public class E_Stomper : EnemyBase
{
    //<=========== メンバー変数 ===========>//
    [Header("移動速度"), SerializeField] float _moveSpeed;
    [Header("デバッグ用Gizmoを表示するか"), SerializeField] bool _isGizmo;

    /// <summary> 右に何があるか判定用オーバーラップボックスのオフセット </summary>
    [Tooltip("右に何かがないか判定用"), SerializeField]
    Vector3 _overLapBoxOffsetRight;
    /// <summary> 左に何があるか判定用オーバーラップボックスのオフセット </summary>
    [Tooltip("左に何かがないか判定用"), SerializeField]
    Vector3 _overLapBoxOffsetLeft;
    /// <summary> オーバーラップボックスのサイズ </summary>
    [Tooltip("上記オーバーラップボックスのサイズ"), SerializeField]
    Vector2 _overLapBoxSizeVertical;
    /// <summary> オーバーラップボックスのLayerMask </summary>
    [SerializeField] LayerMask _layerMask;


    //<=========== Unityメッセージ ===========>//
    protected override void Start()
    {
        base.Start();
    }
    protected override void Update()
    {
        base.Update();
    }
    // デバッグ用 : 接触判定用Gizmoを表示する。
    void OnDrawGizmos()
    {
        // オーバーラップボックスを描画する
        if (_isGizmo)
        {
            //色を指定する。
            Gizmos.color = Color.red;
            //右のgizmo
            Gizmos.DrawCube(_overLapBoxOffsetRight + transform.position, _overLapBoxSizeVertical);
            //左のgizmo
            Gizmos.DrawCube(_overLapBoxOffsetLeft + transform.position, _overLapBoxSizeVertical);
        }
    }

    //<======== protectedメンバー関数 ========>//
    /// <summary> Stomperの移動処理 : 横に動くだけ。何かあれば反転する。 </summary>
    protected override void Move()
    {
        // 向いている方向へ進む。
        if (_isRight)
        {
            _spriteRenderer.flipX = _isRight;
            _rigidBody2d.velocity = new Vector2(1 * _moveSpeed, _rigidBody2d.velocity.y);
        }
        else
        {
            _spriteRenderer.flipX = _isRight;
            _rigidBody2d.velocity = new Vector2(-1 * _moveSpeed, _rigidBody2d.velocity.y);
        }

        // 進んでいる方向に何かあれば反転する。
        if (BodyContactLeft())
        {
            _isRight = true;
        }
        else if (BodyContactRight())
        {
            _isRight = false;
        }
    }

    //<======== privateメンバー関数 ========>//
    /// <summary> Physics2D.OverlapBoxAllを利用して、左に何かあるか判定する。 </summary>
    /// <returns> 何かあればtrueを返す。 </returns>
    bool BodyContactLeft()
    {
        // コライダーをすべて取得
        Collider2D[] collision = Physics2D.OverlapBoxAll(
            _overLapBoxOffsetLeft + transform.position,
            _overLapBoxSizeVertical,
            0f,
            _layerMask);
        //何かあればtrueを返す
        if (collision.Length != 0)
        {
            return true;
        }
        return false;
    }
    /// <summary> Physics2D.OverlapBoxAllを利用して、右に何かあるか判定する。 </summary>
    /// <returns> 何かあればtrueを返す。 </returns>
    bool BodyContactRight()
    {
        // コライダーをすべて取得
        Collider2D[] collision = Physics2D.OverlapBoxAll(
            _overLapBoxOffsetRight + transform.position,
            _overLapBoxSizeVertical,
            0f,
            _layerMask);
        //何かあればtrueを返す
        if (collision.Length != 0)
        {
            return true;
        }
        return false;
    }
}
