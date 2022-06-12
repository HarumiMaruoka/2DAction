using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shot : MonoBehaviour
{
    //各パラメータ
    [SerializeField] float _moveSpeed;//弾の移動スピード
    [SerializeField] float _dashSpeed;//ダッシュ時の移動速度
    [SerializeField] int _barrettPower;//弾の攻撃力

    bool _isRigth;//プレイヤーが向いている方向を向く
    float _dethTimer;//敵に当たったら、タイマースタート。
    float _dashMode;//プレイヤーが歩いているときは、1が入る。

    //プレイヤーのコンポーネント
    SpriteRenderer _playersSpriteRendere;

    //自身のコンポーネント
    SpriteRenderer _spriteRenderer;
    Rigidbody2D _rigidBody2D;

    //破壊時のエフェクト
    [SerializeField] GameObject _destroyingEffectPrefab;

    //破壊管理用
    bool _isDeth;

    bool _isDethMode;
    float _dethTimer2;


    // Start is called before the first frame update
    void Start()
    {
        //各変数の初期化
        _dashMode = 1f;
        _dethTimer = 0f;
        _dethTimer2 = 1.5f;

        //SpriteRendererを取得する
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rigidBody2D = GetComponent<Rigidbody2D>();

        //プレイヤーの向きを取得する
        _playersSpriteRendere = GameObject.Find("ChibiRobo").GetComponent<SpriteRenderer>();
        _isRigth = !_playersSpriteRendere.flipX;

        int direction = 1;//発射位置調整用
        if (!_isRigth)//必要であれば左向きにする
        {
            _spriteRenderer.flipX = true;
            direction = -1;
        }

        if (Input.GetButton("Dash"))
        {
            _dashMode *= _dashSpeed;
        }

        //発射位置を設定する
        transform.position = GameObject.Find("ChibiRobo").transform.position + (Vector3.down * 0.25f) + (Vector3.right * direction * 0.8f);//初期位置は銃口辺り

        //向いている方向に進み続ける
        if (_isRigth)
        {
            _rigidBody2D.velocity = Vector2.right * _moveSpeed * _dashMode;
        }
        else
        {
            _rigidBody2D.velocity = Vector2.left * _moveSpeed * _dashMode;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Destroy(this.gameObject,1.5f);

        //敵と接触したときは少し遅らせて、弾を消失させる
        if (_isDeth)
        {
            _dethTimer += Time.deltaTime;
        }
        if (_dethTimer > 0.03f)
        {
            Instantiate(_destroyingEffectPrefab, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out EnemyBase enemy))//敵に接触したときの処理
        {
            enemy.HitPlayerAttadk(_barrettPower);
            _isDeth = true;
        }
        if (collision.TryGetComponent(out BossBase boss))
        {
            boss.HitPlayerAttack(_barrettPower);
            _isDeth = true;
        }
        else if (collision.gameObject.tag == "Ground")//Groundと接触した時、弾は消失する
        {
            Instantiate(_destroyingEffectPrefab, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }

    IEnumerator DethTimer()
    {
        _isDethMode = true;
        yield return new WaitForSeconds(_dethTimer2);
        _isDethMode = false;
    }
}
