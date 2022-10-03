using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 接地判定が必要なゲームオブジェクトに対してアタッチすべきコンポーネント
/// </summary>
public class IsGroundComponent : MonoBehaviour
{
    //===== インスペクタ変数 =====//
    [Header("接地判定に使用する値")]
    [Tooltip("接地判定用オーバーラップボックスのサイズ"), SerializeField]
    private Vector2 _overLapBoxSize = default;
    [Tooltip("接地判定用レイヤーをここに登録してください"), SerializeField]
    private LayerMask _layerMaskGround = default;
    [Tooltip("接地判定用オーバーラップボックスの位置オフセット"), SerializeField]
    private Vector3 _groundCheckPosOffset = new Vector2(0f, 0f);
    [Tooltip("オーバーラップボックスの位置をデバッグ表示するかどうか"), SerializeField]
    private bool _isGizmo = true;
    [Tooltip("オーバーラップボックスの色"), SerializeField]
    private Color _overlapBoxGizmoColor = default;

    //===== フィールド =====//
    /// <summary>
    /// 接地しているかどうかを表すフィールド
    /// </summary>
    private bool _isGround = false;

    //===== プロパティ =====//
    /// <summary>
    /// 接地しているかどうかを表すプロパティ
    /// </summary>
    public bool IsGround => _isGround;

    //===== Unityメッセージ =====//
    private void Update()
    {
        _isGround = GetIsGround();
    }
    private void OnDrawGizmos()
    {
        if (_isGizmo)
        {
            Gizmos.color = _overlapBoxGizmoColor;
            Gizmos.DrawCube(transform.position + _groundCheckPosOffset, _overLapBoxSize);
        }
    }

    //===== privateメソッド =====//
    /// <summary> 接地判定 </summary>
    /// <returns> 接地していれば true,そうでなければ false を返す。 </returns>
    private bool GetIsGround()
    {
        var overLapBoxCenter = transform.position + _groundCheckPosOffset;
        Collider2D[] collision = Physics2D.OverlapBoxAll(
            overLapBoxCenter,
            _overLapBoxSize,
            0f,
            _layerMaskGround);

        if (collision.Length != 0)
        {
            return true;
        }

        else
        {
            return false;
        }
    }
}
