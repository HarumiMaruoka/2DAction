using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//プレイヤーの体力などの基礎情報を持つクラス
public class PlayerBasicInformation : MonoBehaviour
{
    //基本パラメータ
    [SerializeField] public int _maxHitPoint = 3;//最大HP
    [SerializeField] public int _playerHitPoint = 3;//現在のHP

    //各コンポーネント
    PlayerAnimationManagement _changePlayerState;
    PlayerController _playerController;

    //無敵関連(ヒット後の無敵時間とか)
    float _godModeTime = 1.5f;
    bool _isGodMode = false;

    // Start is called before the first frame update
    void Start()
    {
        //自身のコンポーネントを取得する。
        _changePlayerState = GetComponent<PlayerAnimationManagement>();
        _playerController = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        //player の体力がなくなったら消滅する
        if (_playerHitPoint < 1)
        {
            _playerController.enabled = false;
            _changePlayerState._isDead = true;
        }
    }

    //敵と接触したとき、敵の攻撃力分damageを受け、敵のforce分後方へ飛ばされる
    private void OnCollisionStay2D(Collision2D collision)
    {
        //無敵状態であれば攻撃を受けない
        if (!_isGodMode)
        {
            //Enemyと接触したらEnemyのHitPlayer関数を実行する
            if (collision.gameObject.TryGetComponent(out EnemyBase enemy))
            {
                enemy.HitPlayer();
                _changePlayerState._isHitEnemy = true;
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
}
