using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Enemy / Bossが扱う武器の基底クラス : <br/>
/// このクラスにはプレイヤーに対する攻撃に関する処理を記載してある。<br/>
/// 
/// 敵が放つ
/// </summary>
public class EnemyWeaponBase : MonoBehaviour, IAttackOnPlayer
{
    //===== フィールド / プロパティ =====//
    Collider2D _collider2D;

    [Header("攻撃力"), SerializeField] float _offensivePower;
    [Header("ノックバック力"), SerializeField] float _blowingPower;

    protected Animator _animator;
    protected Rigidbody2D _rigidbody2D;

    // ポーズ用
    float _angularVelocity;
    Vector2 _velocity;

    //===== Unityメッセージ =====//
    /// <summary>
    /// 初期化処理。コライダーを取得する。
    /// </summary>
    protected virtual void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _collider2D = GetComponent<Collider2D>();
        _animator = GetComponent<Animator>();
    }
    /// <summary> プレイヤーに接触したらプレイヤーの体力を減らす。 </summary>
    /// <param name="collision"> 接触相手 </param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == Constants.PLAYER_TAG_NAME)
        {
            // プレイヤーの体力を減らす。
            if (collision.TryGetComponent(out Rigidbody2D playerRb2D))
                HitPlayer(playerRb2D);
        }
    }

    protected virtual void OnEnable()
    {
        GameManager.OnPause += OnPause;
        GameManager.OnResume += OnResume;
    }
    protected virtual void OnDisable()
    {
        GameManager.OnPause -= OnPause;
        GameManager.OnResume -= OnResume;
    }

    /// <summary>
    /// ポーズ処理
    /// </summary>
    protected virtual void OnPause()
    {
        // アニメーションをポーズする。
        if (_animator != null)
        {
            _animator.speed = Constants.PAUSE_ANIM_SPEED;
        }
        // RigidBody2Dをポーズする。
        if (_rigidbody2D != null)
        {
            _angularVelocity = _rigidbody2D.angularVelocity;
            _velocity = _rigidbody2D.velocity;
            _rigidbody2D.Sleep();
            _rigidbody2D.simulated = false;
        }
    }
    /// <summary>
    /// ポーズ解除処理
    /// </summary>
    protected virtual void OnResume()
    {
        // アニメーションのポーズを解除する。
        if (_animator != null)
        {
            _animator.speed = Constants.NOMAL_ANIM_SPEED;
        }
        // RigidBody2Dのポーズを解除する。
        if (_rigidbody2D != null)
        {
            _rigidbody2D.simulated = true;
            _rigidbody2D.WakeUp();
            _rigidbody2D.angularVelocity = _angularVelocity;
            _rigidbody2D.velocity = _velocity;
        }

    }

    /// <summary> 
    /// このオブジェクトにアタッチされているコライダーをアクティブにする。<br/>
    /// このメソッドは、アニメーションイベントから呼び出す想定で作成したもの。<br/>
    /// </summary>
    void ClliderOn()
    {
        _collider2D.enabled = true;
    }
    /// <summary>
    /// このオブジェクトにアタッチされているコライダーを非アクティブにする。<br/>
    /// このメソッドは、アニメーションイベントから呼び出す想定で作成したもの。<br/>
    /// </summary>
    void ClliderOff()
    {
        _collider2D.enabled = false;
    }
    /// <summary>
    /// このゲームオブジェクトおよびアタッチされたコンポーネントを破棄する。<br/>
    /// このメソッドは、アニメーションイベントから呼び出す想定で作成したもの。<br/>
    /// </summary>
    void DestroyThisObject()
    {
        Destroy(gameObject);
    }

    public virtual void HitPlayer(Rigidbody2D playerRb2D)
    {
        //プレイヤーのHitPointを減らす
        PlayerStatusManager.Instance.PlayerHealthPoint -= _offensivePower;
        playerRb2D.velocity = Vector2.zero;
        //プレイヤーをノックバックする
        if (GameObject.FindGameObjectWithTag(Constants.PLAYER_TAG_NAME).transform.position.x >
            transform.position.x)
        {
            playerRb2D.AddForce((Vector2.right + Vector2.up) * _blowingPower, ForceMode2D.Impulse);
        }
        else
        {
            playerRb2D.AddForce((Vector2.left + Vector2.up) * _blowingPower, ForceMode2D.Impulse);
        }
    }
}
