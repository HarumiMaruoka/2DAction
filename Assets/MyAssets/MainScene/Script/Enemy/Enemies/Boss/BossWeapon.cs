using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> 
/// Bossの攻撃を管理するコンポーネント : 
/// コライダーだけ持つ子オブジェクトに持たせる。
/// こいつも、EnemyBaseを継承させる方がいいかな？
/// </summary>
public class BossWeapon : MonoBehaviour
{
    //<=========== メンバー変数 ===========>//
    //プレイヤーのコンポーネント
    GameObject _playerObject;
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

    [Tooltip("Playerに設定されたTagの名前"), SerializeField] string _playerTagName = "";

    bool _isInitialized = false;


    //<=========== Unityメッセージ ===========>//
    void Start()
    {
        if (!(_isInitialized = Initialize_BossAttack()))
        {
            Debug.LogError($"初期化に失敗しました。{gameObject.name}");
        }
        gameObject.SetActive(false);
    }
    void Update()
    {
        if (transform.parent.GetComponent<SpriteRenderer>().flipX)
        {
            _collider2D.offset = new Vector3(_offset.x, _offset.y);
        }
        else
        {
            _collider2D.offset = new Vector3(-_offset.x, _offset.y);
        }
    }

    //<=========== privateメンバー関数 ===========>//
    /// <summary> BossAttackの初期化関数 </summary>
    /// <returns> 初期化に成功したら true を返す。 </returns>
    bool Initialize_BossAttack()
    {
        if (!BossAttackInitialize_Get_PlayerComponents()) return false;
        if (!BossAttackInitialize_Get_ThisGameObjectComponents()) return false;
        return true;
    }
    /// <summary> プレイヤーにアタッチされているコンポーネントを取得する。 </summary>
    /// <returns> 成功したら true を返す。 </returns>
    bool BossAttackInitialize_Get_PlayerComponents()
    {
        //プレイヤーのコンポーネントを取得
        _playerObject = GameObject.FindGameObjectWithTag(_playerTagName);
        if (_playerObject == null)
        {
            Debug.LogError($"PlayerGameObjectの取得に失敗しました。Playerに設定されたTagは {_playerTagName} ですか？");
            return false;
        }

        _playerBasicInformation = _playerObject.GetComponent<PlayerBasicInformation>();
        if (_playerBasicInformation == null)
        {
            Debug.LogError($"PlayerにアタッチされたPlayerBasicInformationコンポーネントの取得に失敗しました : {gameObject.name}");
            return false;
        }

        _playerPos = _playerObject.GetComponent<Transform>();
        if (_playerPos == null)
        {
            Debug.LogError($"PlayerにアタッチされたTransformコンポーネントの取得に失敗しました : {gameObject.name}");
            return false;
        }

        _playersRigidBody2D = _playerObject.GetComponent<Rigidbody2D>();
        if (_playersRigidBody2D == null)
        {
            Debug.LogError($"PlayerにアタッチされたRigidbody2Dコンポーネントの取得に失敗しました : {gameObject.name}");
            return false;
        }

        _playerMoveManager = _playerObject.GetComponent<PlayerMoveManager>();
        if (_playerMoveManager == null)
        {
            Debug.LogError($"PlayerにアタッチされたPlayerMoveManagerコンポーネントの取得に失敗しました : {gameObject.name}");
            return false;
        }

        return true;
    }
    /// <summary> このゲームオブジェクトにアタッチされているコンポーネントを取得する。 </summary>
    /// <returns> 成功したら true を返す。 </returns>
    bool BossAttackInitialize_Get_ThisGameObjectComponents()
    {
        _collider2D = GetComponent<Collider2D>();
        if (_collider2D == null)
        {
            Debug.LogError($"Colliderコンポーネントの取得に失敗しました。 : {gameObject.name}");
            return false;
        }
        _spriteRenderer = GetComponent<SpriteRenderer>();
        if (_spriteRenderer == null)
        {
            Debug.LogWarning($"SpriteRendererコンポーネントの取得に失敗しました。 \n このクラスでSpriteRendererコンポーネントを使用しますか？ : オブジェクト名 \"{gameObject.name}\"");
        }
        _animator = GetComponent<Animator>();
        if (_animator == null)
        {
            Debug.LogWarning($"Animatorコンポーネントの取得に失敗しました。 \n このクラスでAnimatorコンポーネントを使用しますか？ : オブジェクト名 \" {gameObject.name}\"");
        }
        return true;
    }




    //<=========== publicメンバー関数 ===========>//
    //プレイヤーから呼び出す
    public void HitPlayer()//プレイヤーの体力を減らし、ノックバックさせる。
    {
        //プレイヤーのHitPointを減らす
        PlayerStatusManager.Instance.PlayerHealthPoint -= _offensive_Power;
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

    //<=== 以下アニメーションイベントから呼び出す？作ったの結構前で忘れてしまった。要確認。 ===>//
    public void OnCollider()
    {
        _collider2D.enabled = true;
    }

    public void OffCollider()
    {
        _collider2D.enabled = false;
    }
}
