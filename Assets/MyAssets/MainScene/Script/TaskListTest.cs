using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

// 原島さんに教えてもらったタスクリストというものをここに記す。

// まずタスクリストとは、リストに貯められたタスクを順番に消化してゆくものである。 

// 使用方法としては、
// 1.TaskListTest<>()のインスタンスを作成する。
// 2.

// ジェネリッククラス :
// T にはEnum型が入る。
// where T : Enum は、型"T"には"Enum"型が入るように指定するモノ。
// where句は、型を制限するためのモノ。

/// <summary>
/// タスクリストクラス。<br/>
/// タスク(ステート)をリストで管理し、<br/>
/// 追加した順番に処理していく。<br/>
/// </summary>
/// <typeparam name="T"> 任意のEnum型 </typeparam>
public class TaskListTest<T> where T : Enum
{
    /// <summary>
    /// <para>
    /// 一つのタスクを表す型。<br/>
    /// </para>
    /// メンバーとして、<br/>
    /// ステートを表す Enum型 <br/>
    /// Enter  : 開始時に一度だけ実行する処理 <br/>
    /// Update : このステート中実行する処理   <br/> 
    /// Exit   : 終了時に一度だけ実行する処理 <br/>
    /// を持つ。
    /// </summary>
    private class Task<T> where T : Enum
    {
        /// <summary> ステートを表す Enum型 の値 </summary>
        public T TaskType;
        /// <summary> 開始時に一度だけ実行するAction </summary>
        public Action Enter { get; set; }
        /// <summary> このステート中実行するFunc </summary>
        public Func<bool> Update { get; set; }
        /// <summary> 終了時に一度だけ実行するAction </summary>
        public Action Exit { get; set; }

        /// <summary> コンストラクタ </summary>
        /// <param name="t"> 指定のEnum型 </param>
        /// <param name="enter"> 開始時に一度だけ実行するAction </param>
        /// <param name="update"> このステート中実行するFunc </param>
        /// <param name="exit"> 終了時に一度だけ実行するAction </param>
        public Task(T t, Action enter, Func<bool> update, Action exit)
        {
            TaskType = t;
            Enter = enter;
            // ??は null合体演算子 と呼ばれる。
            // updateがnullでない場合左側のオペランドを返す。
            // nullの場合、右側のオペランドを返す。
            Update = update ?? delegate { return true; };
            //↓ラムダ式で書いてみた
            //Update = update ?? (() => true);
            Exit = exit;
        }
    }

    /// <summary> 
    /// 全てのタスクを持つ、データベース的なもの。
    /// </summary>
    Dictionary<T, Task<T>> _DefineTaskDictionary = new Dictionary<T, Task<T>>();
    /// <summary> 
    /// 現在消化中のタスクリスト
    /// </summary>
    List<Task<T>> _CurrentTaskList = new List<Task<T>>();
    /// <summary>
    /// 現在実行中のタスク 
    /// </summary>
    Task<T> _CurrentTask = null;
    /// <summary> 
    /// 現在実行中のタスクのインデックス 
    /// </summary>
    int _currentIndex = 0;

    /// <summary>
    /// インデックスがタスクリストの大きさを超えないようにするために存在する。<br/>
    /// 現在消化中のタスクリストの範囲からインデックスが超えた場合true,<br/>
    /// そうでない場合、falseを返す。<br/>
    /// </summary>
    public bool IsEnd { get => _CurrentTaskList.Count <= _currentIndex; }

    /// <summary>
    /// 現在実行中のタスクがあるかどうか、(nullかどうか)をチェックする。<br/>
    /// nullであればfalse,そうでなければtrueを返す。<br/>
    /// </summary>
    public bool IsMoveTask { get => _CurrentTask != null; }

    /// <summary>
    /// 現在実行中のタスクがnullであれば 指定された Enum型 T の規定値を返す。<br/>
    /// nullでなければ、現在実行中のタスクのタスク名(ステート名)が帰ってくる。
    /// </summary>
    public T CurrentTaskType
    {
        get
        {
            if (_CurrentTask == null)
                return default(T);
            return _CurrentTask.TaskType;
        }
    }

    /// <summary>
    /// 消化すべきタスクリストのキーのリスト。
    /// </summary>
    public List<T> CurrentTaskTypeList
    { get => _CurrentTaskList.Select(x => x.TaskType).ToList(); }

    /// <summary>
    /// 現在実行しているタスクのインデックスのプロパティ
    /// </summary>
    public int CurrentIndex { get => _currentIndex; }

    /// <summary>
    /// タスクの更新処理
    /// </summary>
    public void UpdateTask()
    {
        // 終了したかどうかチェック
        if (IsEnd)
        {
            return;
        }

        // 実行中のタスクが終了した時に実行される。
        // 次のタスクをこれから実行するタスクとして設定する。
        if (_CurrentTask == null)
        {
            _CurrentTask = _CurrentTaskList[_currentIndex];

            _CurrentTask.Enter?.Invoke();
        }

        // 現在のステート(タスク)の更新処理。 更新処理内の結果を保存する。
        var isEndOneTask = _CurrentTask.Update();

        // ステート終了を検知した(更新処理がtrueを返した)場合に以下のブロックを実行する。
        if (isEndOneTask)
        {
            // 現在のステートの終了処理を実行する。
            _CurrentTask?.Exit();

            // インデックスカウントアップ
            _currentIndex++;

            // インデックスが範囲外になった場合の処理。
            if (IsEnd)
            {
                _currentIndex = 0;        // インデックスをリセット
                _CurrentTask = null;      // 現在実行中のタスクを破棄
                _CurrentTaskList.Clear(); // 消化すべきタスクリストをクリア
                return;                   // 処理を抜ける。
            }

            // インデックスが範囲内であれば引き続き実行するので、
            // 実行するタスクを更新する。
            _CurrentTask = _CurrentTaskList[_currentIndex];
            // タスクの開始処理を実行する。
            _CurrentTask?.Enter();
        }
    }

    /// <summary>
    /// 指定のタスクを、タスクのデータベースに登録する処理
    /// </summary>
    /// <param name="t"> 
    /// キーとなる値。<br/>
    /// ステート(タスク)の種類。<br/>
    /// </param>
    /// <param name="enter">
    /// このタスクの開始処理。(開始時に一度だけ実行したい処理。)
    /// </param>
    /// <param name="update">
    /// このタスクの更新処理。(このタスクを実行中毎フレーム実行してほしい処理。)
    /// </param>
    /// <param name="exit">
    /// このタスクの終了処理。(このタスクが終了される際に一度だけ実行してほしい処理。)
    /// </param>
    public void DefineTask(T t, Action enter, Func<bool> update, Action exit)
    {
        // 指定のタスクを生成
        var task = new Task<T>(t, enter, update, exit);
        // 指定されたキーが既に登録されているかチェックする。
        var exist = _DefineTaskDictionary.ContainsKey(t);
        // 既に登録されている場合、エラーメッセージを表示し、処理を抜ける。
        if (exist)
        {
#if UNITY_EDITOR
            Debug.LogError($"既にそのキーのタスクは登録されています！");
#endif
            return;
        }
        _DefineTaskDictionary.Add(t, task);
    }

    /// <summary>
    /// 開始処理と終了処理のみのタスクを <br/>
    /// タスク用データベースに保存する。<br/>
    /// </summary>
    /// <param name="t">
    /// キーとなる値。<br/>
    /// ステート(タスク)の種類。<br/>
    /// </param>
    /// <param name="enter">
    /// このタスクの開始処理。(開始時に一度だけ実行したい処理。)
    /// </param>
    /// <param name="exit">
    /// このタスクの終了処理。(このタスクが終了される際に一度だけ実行してほしい処理。)
    /// </param>
    public void DefineTask(T t, Action enter, Action exit)
    {
        DefineTask(t, enter, () => true, exit);
    }

    /// <summary>
    /// 引数に受け取ったタスクを消化すべきタスクリストに追加する。
    /// </summary>
    /// <param name="t">
    /// 対象のステート(タスク)
    /// </param>
    public void AddTask(T t)
    {
        // 引数として受け取ったステート(タスク)が
        // 全てのタスクのデータベースに含まれているかチェックする。
        var exist = _DefineTaskDictionary.TryGetValue(t, out Task<T> task);
        // 含まれていなければ
        if (exist == false)
        {
#if UNITY_EDITOR
            Debug.LogError(
                $"対象のステート(タスク)が、\n" +
                $"全てのタスクのデータベースには含まれていません！");
#endif
            return;
        }
        // 待機中のタスクリストに加える。
        _CurrentTaskList.Add(task);
    }

    /// <summary>
    /// 現在実行中のタスクを強制的に止める。
    /// </summary>
    public void ForceStop()
    {
        if (_CurrentTask != null)
        {
            _CurrentTask.Exit();
        }
        _CurrentTask = null;
        _CurrentTaskList.Clear();
        _currentIndex = 0;
    }
}
