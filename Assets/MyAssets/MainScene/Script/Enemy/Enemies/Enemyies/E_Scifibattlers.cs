using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 第三のEnemy Scifibattlersのコンポーネント。<br/>
/// このエネミーは、プレイヤーの前にジャンプして立ちはだかる。<br/>
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class E_Scifibattlers : EnemyBase
{
    //===== フィールド / プロパティ =====//
    #region Field and Properties
    /// <summary> ジャンプ力 </summary>
    [Header("固有のステータス")]
    [Tooltip("ジャンプ力"), SerializeField]
    float _jumpPower;

    [Header("接地判定関連")]
    [Tooltip("オーバーラップボックスの範囲を描画するかどうか"), SerializeField]
    bool _isGizmo;
    [Tooltip("接地判定用オーバーラップボックスの位置オフセット"), SerializeField]
    float _groundCheckPosOffsetY;
    [Tooltip("接地判定用オーバーラップボックスのサイズ"), SerializeField]
    Vector3 _groundCheckOverLapBoxSize;
    [Tooltip("接地判定オーバーラップボックス用レイヤーマスク"), SerializeField]
    LayerMask _groundCheckLayerMask;

    int _jumpCount = 1;
    float _beforeSpeedY = 0f;
    #endregion

    //===== Unityイベント群 =====//
    #region Unity Events
    protected override void Start()
    {
        base.Start();
    }
    protected override void Update()
    {
        base.Update();
    }
    void OnDrawGizmos()
    {
        if (_isGizmo)
        {
            Gizmos.color = Color.red;
            // デバッグ用処理 : 接地判定エリアをSceneビューに表示する。
            Gizmos.DrawCube(Vector3.down * _groundCheckPosOffsetY + transform.position, _groundCheckOverLapBoxSize);
        }
    }
    #endregion

    //===== overrides =====//
    #region Overrides
    /// <summary>
    /// このエネミーはずーっとジャンプするだけ。
    /// </summary>
    protected override void Move()
    {
        // 接地判定を取り、接地していればジャンプする。
        if (Get_GroundCheck())
        {
            if ((_jumpCount % 3) == 0)
            {
                _rigidBody2d.velocity = Vector2.up * _jumpPower * 3f;
            }
            else
            {
                _rigidBody2d.velocity = Vector2.up * _jumpPower;
            }
        }
        // ジャンプの判定が取れたら、カウントアップする。
        if (_beforeSpeedY <= 0f && _rigidBody2d.velocity.y > 0f)
        {
            _jumpCount++;
        }

        _beforeSpeedY = _rigidBody2d.velocity.y;
    }
    #endregion

    //===== privateメソッド =====//
    #region Private Method
    /// <summary>
    /// 接地判定を取得する。
    /// </summary>
    /// <returns>
    /// 接地していれば true,<br/> 
    /// そうでなければ false を返す。<br/>
    /// </returns>
    bool Get_GroundCheck()
    {
        var _overLapBoxCenter = transform.position + Vector3.down * _groundCheckPosOffsetY;
        Collider2D[] collision = Physics2D.OverlapBoxAll(
            _overLapBoxCenter,
            _groundCheckOverLapBoxSize,
            0f,
            _groundCheckLayerMask);

        if (collision.Length != 0)
        {
            return true;
        }
        return false;
    }
    #endregion
}
