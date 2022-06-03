using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashTwo : MonoBehaviour
{
    //各パラメータ
    [SerializeField] int _slashOffensivePower;//エネミーに対する攻撃力
    [SerializeField] Vector2 _knockBackPower;//ノックバックパワー
    [SerializeField] float _knockBackTimer;//ノックバックタイマー
    bool _isRigth;//プレイヤーと同じ方向を向ける用

    //プレイヤーのコンポーネント
    Transform _playerPos;
    SpriteRenderer _playerSpriteRendere;

    //自身のコンポーネント
    CapsuleCollider2D _capsuleCollider2D;
    SpriteRenderer _mySpriteRendere;

    void Update()
    {
        transform.position = _playerSpriteRendere.flipX ? _playerPos.position + Vector3.left * 1.5f : _playerPos.position + Vector3.right * 1.5f;
        //プレイヤーが振り向いたら斬撃は消える
        if (_mySpriteRendere.flipX != _playerSpriteRendere.flipX)
        {
            gameObject.SetActive(false);
        }
    }

    //斬撃を消す処理、アニメーションイベントから呼び出す
    public void DestroyObject()
    {
        gameObject.SetActive(false);
    }

    //敵と接触したときに行う処理
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //GetComponentのより軽く短い書き方
        if (collision.TryGetComponent(out EnemyBase enemy))
        {
            enemy.HitPlayerAttadk(_slashOffensivePower, _knockBackTimer);
        }
    }

    private void OnEnable()
    {
        if (_playerPos == null)
        {
            _playerPos = transform.root.gameObject.GetComponent<Transform>();
            _playerSpriteRendere = transform.root.gameObject.GetComponent<SpriteRenderer>();

            //自分のコンポーネントを取得する。
            _capsuleCollider2D = GetComponent<CapsuleCollider2D>();
            _mySpriteRendere = GetComponent<SpriteRenderer>();
        }

        _mySpriteRendere.flipX = _playerSpriteRendere.flipX;
        transform.position = _playerSpriteRendere.flipX ? _playerPos.position + Vector3.left * 1.5f : _playerPos.position + Vector3.right * 1.5f;
        if (_mySpriteRendere.flipX)
        {
            _capsuleCollider2D.offset = new Vector2(-0.3f, 0.15f);
        }
    }
}
