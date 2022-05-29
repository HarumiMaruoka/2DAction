using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slash : MonoBehaviour
{
    [SerializeField] int _slashPower;
    Transform _playerPos;
    SpriteRenderer _playerSpriteRendere;
    CapsuleCollider2D _capsuleCollider2D;
    SpriteRenderer _mySpriteRendere;

    [SerializeField] Vector2 _knockBackPower;

    bool _isRigth;

    bool _isEnemyKnockBack;

    // Start is called before the first frame update
    void Start()
    {
        _playerPos = GameObject.Find("ChibiRobo").GetComponent<Transform>();
        _playerSpriteRendere = GameObject.Find("ChibiRobo").GetComponent<SpriteRenderer>();
        _capsuleCollider2D = GetComponent<CapsuleCollider2D>();
        _mySpriteRendere = GetComponent<SpriteRenderer>();

        _mySpriteRendere.flipX = _playerSpriteRendere.flipX;
        transform.position = _playerSpriteRendere.flipX ? _playerPos.position + Vector3.left * 1.5f : _playerPos.position + Vector3.right * 1.5f;
        if (_mySpriteRendere.flipX)
        {
            _capsuleCollider2D.offset = new Vector2(-0.1f, 0.1f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = _playerSpriteRendere.flipX ? _playerPos.position + Vector3.left * 1.2f : _playerPos.position + Vector3.right * 1.2f;
        //プレイヤーが振り向いたら斬撃は消える
        if (_mySpriteRendere.flipX != _playerSpriteRendere.flipX)
        {
            Destroy(this.gameObject);
        }
    }

    //斬撃を消す処理、アニメーションイベントから呼び出す
    public void DestroyObject()
    {
        Destroy(this.gameObject);
    }

    //敵と接触したときに行う処理
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out EnemyBase enemy) && collision.TryGetComponent(out Rigidbody2D enemyiesRigitBody2D))
        {
            enemy.HitPlayerAttadk(_slashPower,_knockBackPower);
            enemyiesRigitBody2D.AddForce(_knockBackPower, ForceMode2D.Impulse);
        }
        //if (collision.gameObject.tag == "Enemy")
        //{
        //    //ここに敵と接触したときの処理を書く
        //    collision.gameObject.GetComponent<EnemyBase>().HitPlayerAttadk(_slashPower);
        //    collision.gameObject.GetComponent<Rigidbody2D>().AddForce(_knockBackPower, ForceMode2D.Impulse);
        //}
    }
}
