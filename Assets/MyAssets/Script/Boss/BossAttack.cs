using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttack : MonoBehaviour
{
    //プレイヤーのコンポーネント
    GameObject _playerObjedt;
    protected Transform _playerPos;
    protected PlayerBasicInformation _playerBasicInformation;
    protected Rigidbody2D _playersRigidBody2D;
    protected PlayerMoveManager _playerMoveManager;

    //自身のコンポーネント
    Animator _animator;
    Collider2D _collider2D;
    SpriteRenderer _spriteRenderer;

    //各ステータス
    [Tooltip("攻撃力"), SerializeField] protected int _offensive_Power;
    [Tooltip("プレイヤーに対するノックバック力"), SerializeField] protected Vector2 _playerKnockBackPower;

    [Tooltip("コライダーのオフセット"), SerializeField] Vector2 _offset;


    void Start()
    {
        _animator = GetComponent<Animator>();
        _collider2D = GetComponent<Collider2D>();
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
        if (transform.parent.GetComponent<SpriteRenderer>().flipX)
        {
            _collider2D.offset = new Vector3(_offset.x,_offset.y);
        }
        else
        {
            _collider2D.offset = new Vector3(-_offset.x, _offset.y);
        }
    }

    //プレイヤーから呼び出す
    public void HitPlayer()//プレイヤーの体力を減らし、ノックバックさせる。
    {
        //プレイヤーのHitPointを減らす
        _playerBasicInformation._playerHitPoint -= _offensive_Power;
        _playersRigidBody2D.velocity = Vector2.zero;
        //プレイヤーをノックバックする
        if (transform.parent.GetComponent<SpriteRenderer>().flipX)
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

    public void OnCollider()
    {
        _collider2D.enabled = true;
    }

    public void OffCollider()
    {
        _collider2D.enabled = false;
    }
}
