using UnityEngine;

/// <summary> 武器の基底クラス </summary>
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
public abstract class PlayerWeaponBase : MonoBehaviour
{
    //<====== 各コンポーネント ======>//
    protected Rigidbody2D _rigidBody2D;
    protected Collider2D _collider2D;
    protected Animator _animator;
    /// <summary> 自身の武器ID </summary>
    protected int _myWeaponID;
    [Header("プレイヤーの位置からのオフセット"), SerializeField]
    protected Vector3 _offsetFromPlayerPosition;
    /// <summary> プレイヤーのポジション </summary>
    protected Transform _playerPos;
    /// <summary>
    /// 押下時に実行する武器か? : 
    /// この変数がtrueの場合、この武器は、押下時のみ実行する武器。
    /// falseの場合、押している間ずっと実行すべき武器。 
    /// </summary>
    protected bool _isPressType;

    /// <summary> 武器の初期化処理 : オーバーライド可能 </summary>
    protected virtual void WeaponInit() { _playerPos = transform.parent; }

    /// <summary> 攻撃実行処理 : Fireボタンが押された時の処理。 : オーバーライド可 </summary>
    public virtual void Run_FireProcess() { }

    /// <summary> このオブジェクトがアクティブになった時に実行すべき関数。 : オーバーライド可 </summary>
    protected virtual void OnEnable_ThisWeapon() { }

    /// <summary> アップデート関数。 : オーバーライド可 </summary>
    protected virtual void Update_ThisClass() { }

    /// <summary> 必要であれば移動する。 : オーバーライド可 </summary>
    public virtual void Move() { }

    /// <summary> 自身とプレイヤーの向きをチェックし、必要であれば向きを変更する。 : 向きは他のコンポーネントにも適用したいのでスケールで管理する。 </summary>
    protected void DirectionCheck()
    {
        //向きによってローテーションを変える。
        //プレイヤーが右に向いている時 かつ、武器オブジェクトが向いている方向が左向きのとき、武器オブジェクトが向いている方向を反転させる。
        if (PlayerStatusManager.Instance.IsRight && transform.localScale.x < 0)
        {
            Vector3 localScale = transform.localScale;
            localScale.x *= -1;
            transform.localScale = localScale;
        }
        //プレイヤーが左に向いている時 かつ、武器オブジェクトが向いている方向が右向きのとき、武器オブジェクトが向いている方向を反転させる。
        else if (!PlayerStatusManager.Instance.IsRight && transform.localScale.x > 0)
        {
            Vector3 localScale = transform.localScale;
            localScale.x *= -1;
            transform.localScale = localScale;
        }
    }
    /// <summary> 位置を更新する。 </summary>
    /// <returns> 更新後の位置 </returns>
    protected Vector3 UpdatePosition() { return transform.position = _playerPos.position + _offsetFromPlayerPosition; }
}
