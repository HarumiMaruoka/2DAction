using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
public abstract class PlayerWeaponBase : MonoBehaviour
{
    //<====== 自身の各コンポーネント ======>//
    protected Rigidbody2D _rigidBody2D;
    protected Collider2D _collider2D;
    protected Animator _animator;
    /// <summary> 自身の武器ID </summary>
    protected int _myWeaponID;
    [Header("アクティブになった瞬間の位置オフセット"), SerializeField] protected Vector3 _positionOffsetAtBirth;
    /// <summary> 押下時に実行する武器か? : この変数がtrueの場合、この武器は、押下時のみ実行する武器。falseの場合、押している間ずっと実行すべき武器。 </summary>
    protected bool _isPressType;

    /// <summary> 武器の初期化処理 : オーバーライド可能 </summary>
    protected virtual void WeaponInit()
    {
        //コンポーネントを取得
        _animator = GetComponent<Animator>();
        _collider2D = GetComponent<Collider2D>();
        _rigidBody2D = GetComponent<Rigidbody2D>();
    }

    /// <summary> 攻撃実行処理 : Fireボタンが押された時の処理。 : オーバーライド可 </summary>
    public virtual void Run_FireProcess()
    {
        //このコンポーネントがアタッチされているゲームオブジェクトをアクティブにする。
        gameObject.SetActive(true);
        //アニメーションを再生する。 : アニメーションパラメータを設定する。
        _animator.SetInteger("WeaponID", _myWeaponID);
    }

    /// <summary> アクティブになった時の処理 : オーバーライド可 </summary>
    protected virtual void OnEnable_ThisWeapon()
    {
        //向きによってローテーションを変える。
        //プレイヤーが右に向いている時 かつ、オブジェクトが向いている方向が右の時
        if (PlayerStatusManager.Instance.IsRight && transform.rotation.x > 0)
        {

        }
        //位置を設定する。
        transform.position = transform.position + _positionOffsetAtBirth;
    }

    /// <summary> 必要であれば移動する。 : オーバーライド可 </summary>
    public virtual void Move() { }

    /// <summary> 攻撃開始の処理。 : オーバーライド可 </summary>
    protected virtual void OnStart_ThisAttack()
    {
        //このゲームオブジェクトをアクティブにする。
        gameObject.SetActive(true);
    }
    /// <summary> 攻撃終了の処理 : オーバーライド可 </summary>
    protected virtual void OnEnd_ThisAttack()
    {
        //このゲームオブジェクトを非アクティブにする。
        gameObject.SetActive(false);
    }

    /// <summary> このコンポーネントをアクティブにする。 : オーバーライド可 </summary>
    protected virtual void Activate_ThisComponen() { enabled = true; }
    /// <summary> このコンポーネントを非アクティブにする。 : オーバーライド可 </summary>
    protected virtual void DeactivateThisComponent() { enabled = false; }

    /// <summary> このゲームオブジェクトにアタッチされたコライダーをアクティブにする。 : オーバーライド可 </summary>
    protected virtual void ActivateCollider() { _collider2D.enabled = true; }
    /// <summary> このゲームオブジェクトにアタッチされたコライダーを非アクティブにする。 : オーバーライド可 </summary>
    protected virtual void DeactivateCollider() { _collider2D.enabled = false; }
}
