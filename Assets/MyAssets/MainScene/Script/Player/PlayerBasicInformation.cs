using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//プレイヤーの体力などの基礎情報を持つクラス
public class PlayerBasicInformation : MonoBehaviour
{
    AudioSource _hitEnemySound;
    Rigidbody2D _rigidbody2D;

    [Header("Button群"), SerializeField] GameObject _button;

    //各コンポーネント
    PlayerStateManagement _newPlayerStateManagement;

    //無敵関連(ヒット後の無敵時間とか)
    float _godModeTime = 1.5f;
    bool _isGodMode = false;

    //ホバー制限
    public float _hoverValue { get; set; }
    [Tooltip("最大ガス量"), SerializeField] private float _maxHealthForHover;
    public float MaxHealthForHover { get => _maxHealthForHover; }

    IEnumerator _waitGodModeIntervalCoroutine = default;

    void Start()
    {
        _hoverValue = MaxHealthForHover;
        //自身のコンポーネントを取得する。
        _newPlayerStateManagement = GetComponent<PlayerStateManagement>();
        _hitEnemySound = GetComponent<AudioSource>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        //player の体力がなくなったら消滅する
        if (PlayerStatusManager.Instance.PlayerHealthPoint < 1)
        {
            _newPlayerStateManagement.IsDead = true;
            _button.SetActive(true);
        }
    }
    private void OnEnable()
    {
        GameManager.OnPause += OnPause;
        GameManager.OnResume += OnResume;
    }
    private void OnDisable()
    {
        GameManager.OnPause -= OnPause;
        GameManager.OnResume -= OnResume;
    }

    void OnPause()
    {
        if (_waitGodModeIntervalCoroutine != null)
        {
            StopCoroutine(_waitGodModeIntervalCoroutine);
        }
    }
    void OnResume()
    {
        if (_waitGodModeIntervalCoroutine != null)
        {
            StartCoroutine(_waitGodModeIntervalCoroutine);
        }
    }


    //敵と接触したとき、敵の攻撃力分damageを受け、敵のforce分後方へ飛ばされる
    private void OnCollisionStay2D(Collision2D collision)
    {
        //無敵状態であれば攻撃を受けない
        if (!_isGodMode && !_newPlayerStateManagement.IsDead)
        {
            //Enemyと接触したらEnemyのHitPlayer関数を実行する
            if (collision.gameObject.TryGetComponent(out EnemyBase enemy))
            {
                enemy.HitPlayer(_rigidbody2D);
                _newPlayerStateManagement.IsHitEnemy = true;
                _hitEnemySound.Play();
                _waitGodModeIntervalCoroutine = WaitGodModeInterval();
                StartCoroutine(_waitGodModeIntervalCoroutine);
            }
        }
    }

    IEnumerator WaitGodModeInterval()
    {
        float timer = 0f;
        _isGodMode = true;
        while (timer < _godModeTime)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        _isGodMode = false;
        _waitGodModeIntervalCoroutine = null;
    }

    // 何かと接触したときの処理
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Godモードでも"Deadステート"でもなければ実行する。
        if (!_isGodMode && !_newPlayerStateManagement.IsDead)
        {
            if (collision.gameObject.TryGetComponent(out IAttackOnPlayer _enemy))
            {
                _enemy.HitPlayer(_rigidbody2D);
                _newPlayerStateManagement.IsHitEnemy = true;
                _hitEnemySound.Play();
                //一定時間無敵にする。
                _waitGodModeIntervalCoroutine = WaitGodModeInterval();
                StartCoroutine(_waitGodModeIntervalCoroutine);
            }
            //バッテリーと接触したときの処理
            if (collision.gameObject.tag == "Battery")
            {
                PlayerStatusManager.Instance.PlayerHealthPoint = PlayerStatusManager.Instance.ConsequentialPlayerStatus._maxHp;
                AudioSource.PlayClipAtPoint(collision.gameObject.GetComponent<AudioSource>().clip, transform.position);
            }
        }
    }
}
