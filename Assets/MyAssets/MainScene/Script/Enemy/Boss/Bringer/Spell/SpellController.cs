using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellController : MonoBehaviour
{
    //プレイヤーのコンポーネント
    GameObject _playerObjedt;
    protected Transform _playerPos;
    protected PlayerBasicInformation _playerBasicInformation;
    protected Rigidbody2D _playersRigidBody2D;
    protected PlayerMoveManager _playerMoveManager;

    //自身のコンポーネント
    Animator _animator;
    BoxCollider2D _collider2D;
    SpriteRenderer _spriteRenderer;

    //各ステータス
    [Tooltip("攻撃力"), SerializeField] protected int _offensivePower;
    [Tooltip("プレイヤーに対するノックバック力"), SerializeField] protected Vector2 _playerKnockBackPower;
    [Tooltip("高さ"), SerializeField] float _height;
    [Tooltip("スペルが落ちるべき位置"), SerializeField] protected Vector2 _fallPos;


    void Start()
    {
        _animator = GetComponent<Animator>();
        _collider2D = GetComponent<BoxCollider2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

        //プレイヤーのコンポーネントを取得
        _playerObjedt = GameObject.Find("ChibiRobo");
        _playerBasicInformation = _playerObjedt.GetComponent<PlayerBasicInformation>();
        _playerPos = _playerObjedt.GetComponent<Transform>();
        _playersRigidBody2D = _playerObjedt.GetComponent<Rigidbody2D>();
        _playerMoveManager = _playerObjedt.GetComponent<PlayerMoveManager>();

        gameObject.SetActive(false);
    }

    void Update()
    {
        transform.position = _fallPos;
        //向きを設定:親の向きを取得
        _spriteRenderer.flipX = transform.parent.GetComponent<SpriteRenderer>().flipX;
    }

    /// <summary> スペル発動時の処理 </summary>
    public void PlaySpell()
    {
        //アニメーションを再生
        _animator.SetTrigger("Spell");

        //位置をプレイヤーの頭上に調節
        _fallPos = _playerPos.position + (Vector3.up * _height);
    }


    //アニメーションイベントから呼び出す
    /// <summary> 雷が落ちる瞬間の処理。コライダーをオンにする </summary>
    public void LightningFalls()
    {
        //コライダーをOnにする
        _collider2D.enabled = true;
    }

    /// <summary> 雷が消えるときの処理。コライダーをオフにする </summary>
    public void LightningDisappears()
    {
        _collider2D.enabled = false;
    }

    /// <summary> スペル終了時の処理 </summary>
    public void EndSpell()
    {
        gameObject.SetActive(false);
    }

    //プレイヤーから呼び出す
    public void HitPlayer()//プレイヤーの体力を減らし、ノックバックさせる。
    {
        //プレイヤーのHitPointを減らす
        PlayerStatusManager.Instance.PlayerHealthPoint -= _offensivePower;
        _playersRigidBody2D.velocity = Vector2.zero;
        //プレイヤーをノックバックする
        if (_spriteRenderer.flipX)
        {
            _playersRigidBody2D.velocity = Vector2.zero;
            _playersRigidBody2D.AddForce((Vector2.right + Vector2.up) * _playerKnockBackPower, ForceMode2D.Impulse);
        }
        else
        {
            _playersRigidBody2D.velocity = Vector2.zero;
            _playersRigidBody2D.AddForce((Vector2.left + Vector2.up) * _playerKnockBackPower, ForceMode2D.Impulse);
        }
    }
}
