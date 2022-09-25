using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Boss, Bringer 用のコンポーネント : <br/>
/// BossBaseを継承する。
/// </summary>
public class BringerController : NewBossBase
{
    //<===== メンバー変数 =====>//
    #region Field and Property
    [Header("各攻撃後のクールタイム")]
    [Tooltip("弱攻撃後のクールタイム"), SerializeField]
    RandomRangeValue _cooltimeAfterLightAttack;
    [Tooltip("強攻撃後のクールタイム"), SerializeField]
    RandomRangeValue _cooltimeAfterHeavyAttack;
    [Tooltip("遠距離攻撃後のクールタイム"), SerializeField]
    RandomRangeValue _cooltimeAfterLongRangeAttack;

    [Header("各攻撃別に移行する確率 (全部足して100%になるように作成してください。)")]
    [Tooltip("弱攻撃を撃つ確率"), SerializeField]
    float _lightAttackProbability = 40f;
    [Tooltip("強攻撃を撃つ確率"), SerializeField]
    float _heavyAttackProbability = 30f;
    [Tooltip("遠距離攻撃を撃つ確率"), SerializeField]
    float _longRangeAttackProbability = 30f;

    [Header("各通常行動に移行する確率 (全部足して100%になるように作成してください。)")]
    [Tooltip("アイドルに移行する確率"), SerializeField]
    float _idleProbability = 15f;
    [Tooltip("接近行動に移行する確率"), SerializeField]
    float _approachProbability = 55f;
    [Tooltip("後退行動に移行する確率"), SerializeField]
    float _recessionProbability = 30f;

    [Header("アニメーション関連")]
    [Tooltip("アニメーションスピードのパラメーター名"), SerializeField]
    string _animSpeedParamName = "AnimSpeed";
    [Tooltip("アイドルを表現するアニメーションの名前"), SerializeField]
    string _idleAnimStateName = "Idle";
    [Tooltip("接近/後退を表現するアニメーションの名前"), SerializeField]
    string _runAnimStateName = "Run";
    [Tooltip("弱攻撃を表現するアニメーションの名前"), SerializeField]
    string _lightAttackAnimStateName = "LightAttack";
    [Tooltip("強攻撃を表現するアニメーションの名前"), SerializeField]
    string _heavyAttackAnimStateName = "HeavyAttack";
    [Tooltip("遠距離攻撃を表現するアニメーションの名前"), SerializeField]
    string _longRangeAttackAnimStateName = "LongRangeAttackMotion";
    [Tooltip("死を表現するアニメーションの名前"), SerializeField]
    string _dieAnimStateName = "Death";

    [Header("武器のプレハブ")]
    [Tooltip("弱攻撃のプレハブ"), SerializeField]
    GameObject _lightAttackPrefab;
    [Tooltip("強攻撃のプレハブ"), SerializeField]
    GameObject _heavyAttackPrefab;
    [Tooltip("遠距離攻撃のプレハブ"), SerializeField]
    GameObject _longRangeAttackPrefab;
    GameObject _weapon;

    /// <summary> 後退時の移動速度の倍率 </summary>
    const float _moveSpeedMagnificationAtRecession = 0.6f;
    #endregion

    //<===== Unityメッセージ =====>//
    #region Unity Message
    protected override void Start()
    {
        base.Start();
        _holdAnimSpeed = 1f;
    }
    protected override void Update()
    {
        base.Update();
    }
    #endregion

    //<===== メンバー関数 =====>//
    #region Methods
    #region overrides
    //===== このボスのステートを管理する軸となるメソッド =====//
    protected override void ManageState()
    {
        switch (_nowState)
        {
            //通常行動処理
            case BossState.IDLE: Idle(); break;
            case BossState.APPROACH: Approach(); break;
            case BossState.RECESSION: Recession(); break;

            //攻撃処理
            case BossState.LIGHT_ATTACK_PATTERN_ONE: LightAttack(); break;
            case BossState.HEAVY_ATTACK_PATTERN_ONE: HeavyAttack(); break;
            case BossState.LONG_RANGE_ATTACK_PATTERN_ONE: LongRangeAttack(); break;

            //その他の処理
            case BossState.DIE: TreatmentOfDeath(); break;

            //エラー値処理
            default: Debug.LogError("エラー値です。修正してください！"); break;
        }
    }
    /// <summary> 設定された確率を基に"攻撃"行動ステートに遷移する。 </summary>
    protected override void StartAttackProcess()
    {
        //速度をリセットする。(0にする)
        _rigidBody2d.velocity = Vector2.zero;

        // 入力された値を基にランダムに遷移先を決める。
        float probability = Random.Range(0f, 100f);
        // "弱攻撃"に遷移する処理 / 遷移時に実行する処理
        if (probability < _lightAttackProbability) MomentOfLightAttack();
        // "強攻撃"に遷移する処理 / 遷移時に実行する処理
        else if (probability < _heavyAttackProbability + _lightAttackProbability) MomentOfHeavyAttack();
        // "遠距離攻撃"に遷移する処理 / 遷移時に実行する処理
        else MomentOfLongRangeAttack();
    }
    /// <summary> 設定された確率を基に"通常"行動ステートに遷移する。 </summary>
    protected override void EndAttackProcess()
    {
        //速度をリセットする。(0にする)
        _rigidBody2d.velocity = Vector2.zero;

        // 入力された値を基にランダムに遷移先を決める
        float probability = Random.Range(0f, 100f);
        // "アイドル"に遷移する。
        if (probability < _idleProbability) MomentOfIdle();
        // "接近ステート"に遷移する。
        else if (probability < _approachProbability + _idleProbability) MomentOfApproach();
        // "後退ステート"に遷移する。
        else MomentOfRecession();
    }
    protected override void TreatmentAfterDeath()
    {
        // 特に何も思いつかないので何もしない。
        // 死亡中に何かさせる場合は、ここに記述すること。
    }
    protected override void MomentOfDeath()
    {
        // その場で止まる。
        _rigidBody2d.velocity = Vector2.zero;
        // 死亡時のアニメーションを再生する。
        _animator.Play(_dieAnimStateName);

    }
    protected override void OnPause()
    {
        base.OnPause();
        _animator.SetFloat(_animSpeedParamName, 0f);
    }
    protected override void OnResume()
    {
        base.OnResume();
        _animator.SetFloat(_animSpeedParamName, _holdAnimSpeed);
    }
    #endregion

    #region Private Method
    //===== 各ステート別の処理 =====//
    //===== 通常行動群 =====//
    /// <summary> アイドルの処理 </summary>
    void Idle()
    {
        // その場で停止する。(つまり何もしない)
    }
    /// <summary> 接近の処理 </summary>
    void Approach()
    {
        // プレイヤーに向かって接近する。
        if (_playerPos.position.x > transform.position.x)
        {
            _rigidBody2d.velocity = Vector2.right * _status._moveSpeed;
        }
        else
        {
            _rigidBody2d.velocity = Vector2.left * _status._moveSpeed;
        }
    }
    /// <summary> 後退の処理 </summary>
    void Recession()
    {
        // 指定された時間プレイヤーから遠ざかる。

        // プレイヤーに対して後退する。
        if (_playerPos.position.x < transform.position.x)
        {
            _rigidBody2d.velocity = Vector2.right * _status._moveSpeed * _moveSpeedMagnificationAtRecession;
        }
        else
        {
            _rigidBody2d.velocity = Vector2.left * _status._moveSpeed * _moveSpeedMagnificationAtRecession;
        }
    }
    //===== 攻撃行動群 =====//
    /// <summary> 
    /// 弱攻撃の処理<br/>
    /// 撃ってる間は止まる。(何もしない。)<br/>
    /// 他に何かある時はここに書く<br/>
    /// </summary>
    void LightAttack() { }
    /// <summary> 
    /// 強攻撃の処理<br/>
    /// 撃ってる間は止まる。(何もしない。)<br/>
    /// 他に何かある時はここに書く<br/>
    /// </summary>
    void HeavyAttack() { }
    /// <summary>
    /// 遠距離攻撃の処理<br/>
    /// 撃ってる間は止まる。(何もしない。)<br/>
    /// 他に何かある時はここに書く<br/>
    /// </summary>
    void LongRangeAttack() { }
    //===== 通常行動に遷移する際に実行すべき処理群 =====//
    /// <summary>
    /// "アイドルステート"に遷移する瞬間一度だけ実行する処理。
    /// </summary>
    void MomentOfIdle()
    {
        // アイドル状態のアニメーションを再生する。
        _animator.Play(_idleAnimStateName);
        // ポーズ用にアニメーションスピードを保存する。
        _holdAnimSpeed = Constants.NOMAL_ANIM_SPEED;
        // クールタイムを開始する。
        _waitCoolTimeCoroutine = WaitCoolTime();
        StartCoroutine(_waitCoolTimeCoroutine);
        // ステートを変更する。
        _nowState = BossState.IDLE;
    }
    /// <summary>
    /// "接近ステート"に遷移する瞬間一度だけ実行する処理。
    /// </summary>
    void MomentOfApproach()
    {
        // 歩行アニメーションを再生する。
        _animator.Play(_runAnimStateName);
        // 通常再生する。(逆再生している可能性があるので)
        _animator.SetFloat(_animSpeedParamName, Constants.NOMAL_ANIM_SPEED);
        // ポーズ用にアニメーションスピードを保存する。
        _holdAnimSpeed = Constants.NOMAL_ANIM_SPEED;
        // クールタイムを開始する。
        _waitCoolTimeCoroutine = WaitCoolTime();
        StartCoroutine(_waitCoolTimeCoroutine);

        // ステートを変更する。
        _nowState = BossState.APPROACH;
    }
    /// <summary>
    /// "後退ステート"に遷移する瞬間一度だけ実行する処理。
    /// </summary>
    void MomentOfRecession()
    {
        // 歩行アニメーションを再生する。
        _animator.Play(_runAnimStateName);
        // アニメーションを逆再生する。(これで後退を表す。)
        _animator.SetFloat(_animSpeedParamName, Constants.REVERSE_PLAYBACK_ANIM_SPEED);
        // アニメーションスピードを保持する。
        _holdAnimSpeed = Constants.REVERSE_PLAYBACK_ANIM_SPEED;
        // クールタイムを開始する。
        _waitCoolTimeCoroutine = WaitCoolTime();
        StartCoroutine(_waitCoolTimeCoroutine);

        // ステートを変更する。
        _nowState = BossState.RECESSION;
    }
    //===== 攻撃行動に遷移する際に実行すべき処理群 =====//
    /// <summary>
    /// "弱攻撃ステート"に遷移する瞬間に一度だけ実行する処理。<br/>
    /// </summary>
    void MomentOfLightAttack()
    {
        // アニメーションを再生する。
        _animator.Play(_lightAttackAnimStateName);
        // ポーズ用にアニメーションスピードを保存する。
        _holdAnimSpeed = Constants.NOMAL_ANIM_SPEED;
        // クールタイム時間を設定する。
        _coolTimeValue = Random.Range(_cooltimeAfterLightAttack._minValue, _cooltimeAfterLightAttack._maxValue);
        // ステートを変更する。
        _nowState = BossState.LIGHT_ATTACK_PATTERN_ONE;
    }
    /// <summary>
    /// "強攻撃ステート"に遷移する瞬間に一度だけ実行する処理。<br/>
    /// </summary>
    void MomentOfHeavyAttack()
    {
        // アニメーションを再生する。
        _animator.Play(_heavyAttackAnimStateName);
        // ポーズ用にアニメーションスピードを保存する。
        _holdAnimSpeed = Constants.NOMAL_ANIM_SPEED;
        // クールタイム時間を設定する。
        _coolTimeValue = Random.Range(_cooltimeAfterHeavyAttack._minValue, _cooltimeAfterHeavyAttack._maxValue);
        // ステートを変更する。
        _nowState = BossState.HEAVY_ATTACK_PATTERN_ONE;
    }
    /// <summary>
    /// "遠距離攻撃ステート"に遷移する瞬間に一度だけ実行する処理。<br/>
    /// </summary>
    void MomentOfLongRangeAttack()
    {
        // アニメーションを再生する。
        _animator.Play(_longRangeAttackAnimStateName);
        // ポーズ用にアニメーションスピードを保存する。
        _holdAnimSpeed = Constants.NOMAL_ANIM_SPEED;
        // クールタイム時間を設定する。
        _coolTimeValue = Random.Range(_cooltimeAfterLongRangeAttack._minValue, _cooltimeAfterLongRangeAttack._maxValue);
        // ステートを変更する。
        _nowState = BossState.LONG_RANGE_ATTACK_PATTERN_ONE;
    }
    #endregion

    //===== アニメーションイベントから呼び出す想定のメソッド =====//
    #region Call Animation Event
    /// <summary> 
    /// 弱攻撃判定を生成する。<br/>
    /// このメソッドは、アニメーションイベントから呼び出す想定で作成したもの。<br/>
    /// </summary>
    void GenerateAttack_LightAttack() => _weapon = Instantiate(_lightAttackPrefab, transform.position, Quaternion.identity, transform);
    /// <summary> 
    /// 強攻撃判定を生成する。<br/>
    /// このメソッドは、アニメーションイベントから呼び出す想定で作成したもの。<br/>
    /// </summary>
    void GenerateAttack_HeavyAttack() => _weapon = Instantiate(_lightAttackPrefab, transform.position, Quaternion.identity, transform);
    /// <summary> 
    /// 遠距離攻撃を生成する。<br/>
    /// このメソッドは、アニメーションイベントから呼び出す想定で作成したもの。<br/>
    /// </summary>
    void GenerateAttack_LongRangeAttack() => _weapon = Instantiate(_longRangeAttackPrefab, transform.position, Quaternion.identity);
    /// <summary> 
    /// 武器を破棄する。<br/>
    /// このメソッドは、アニメーションイベントから呼び出す想定で作成したもの。<br/>
    /// </summary>
    void DestroyAttack() => Destroy(_weapon);
    #endregion
    #endregion
}
