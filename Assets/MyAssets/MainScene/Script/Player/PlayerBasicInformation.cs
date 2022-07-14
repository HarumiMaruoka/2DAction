using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//プレイヤーの体力などの基礎情報を持つクラス
public class PlayerBasicInformation : MonoBehaviour
{
    //基本パラメータ
    [SerializeField] public int _maxHitPoint = 3;//最大HP
    [SerializeField] public int _playerHitPoint = 3;//現在のHP

    AudioSource _hitEnemySound;

    [Header("Botton群"), SerializeField] GameObject _botton;

    //各コンポーネント
    PlayerStateManagement _newPlayerStateManagement;

    //無敵関連(ヒット後の無敵時間とか)
    float _godModeTime = 1.5f;
    bool _isGodMode = false;

    //ホバー制限
    public float _hoverValue { get; set; }
    [Tooltip("最大ガス量"), SerializeField] private float _maxHealthForHover;
    public float MaxHealthForHover { get => _maxHealthForHover; }

    // Start is called before the first frame update
    void Start()
    {
        _hoverValue = MaxHealthForHover;
        //自身のコンポーネントを取得する。
        _newPlayerStateManagement = GetComponent<PlayerStateManagement>();
        _hitEnemySound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //player の体力がなくなったら消滅する
        if (PlayerStatusManager.Instance.PlayerHealthPoint < 1)
        {
            _newPlayerStateManagement._isDead = true;
            _botton.SetActive(true);
        }
    }

    //敵と接触したとき、敵の攻撃力分damageを受け、敵のforce分後方へ飛ばされる
    private void OnCollisionStay2D(Collision2D collision)
    {
        //無敵状態であれば攻撃を受けない
        if (!_isGodMode && !_newPlayerStateManagement._isDead)
        {
            //Enemyと接触したらEnemyのHitPlayer関数を実行する
            if (collision.gameObject.TryGetComponent(out EnemyBase enemy))
            {
                enemy.HitPlayer();
                _newPlayerStateManagement._isHitEnemy = true;
                _hitEnemySound.Play();
                StartCoroutine(GodMode());
            }
            if (collision.gameObject.TryGetComponent(out BossBase boss))
            {
                boss.HitPlayer();
                _newPlayerStateManagement._isHitEnemy = true;
                _hitEnemySound.Play();
                StartCoroutine(GodMode());
            }
        }
    }

    IEnumerator GodMode()
    {
        _isGodMode = true;
        yield return new WaitForSeconds(_godModeTime);
        _isGodMode = false;
    }

    //ボスと接触したときの処理
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!_isGodMode && !_newPlayerStateManagement._isDead)
        {
            if (collision.gameObject.TryGetComponent(out SpellController spell))
            {
                spell.HitPlayer();
                _newPlayerStateManagement._isHitEnemy = true;
                _hitEnemySound.Play();
                StartCoroutine(GodMode());
            }
            if (collision.gameObject.TryGetComponent(out BossAttack bossAttack))
            {
                bossAttack.HitPlayer();
                _newPlayerStateManagement._isHitEnemy = true;
                _hitEnemySound.Play();
                StartCoroutine(GodMode());
            }
            //バッテリーと接触したときの処理
            if (collision.gameObject.tag == "Battery")
            {
                PlayerStatusManager.Instance.PlayerHealthPoint = PlayerStatusManager.Instance.PlayerMaxHealthPoint;
                AudioSource.PlayClipAtPoint(collision.gameObject.GetComponent<AudioSource>().clip, transform.position);
            }
        }
    }
}
