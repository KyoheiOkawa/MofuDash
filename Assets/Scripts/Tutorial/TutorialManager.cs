using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class TutorialManager : MonoBehaviour
{
    [SerializeField]
    DescriptionDirector descriptionDirector = null;

    public DescriptionDirector DescriptionDirector
    {
        get
        {
            return descriptionDirector;
        }
    }

    [SerializeField]
    Player player = null;

    public Player Player
    {
        get
        {
            return player;
        }
    }

    [SerializeField]
    string[] helloComments;

    public string[] HelloComments
    {
        get
        {
            return helloComments;
        }
    }

    [SerializeField]
    float intervalSwitchComment = 1.0f;

    public float IntervalSwitchComment
    {
        get
        {
            return intervalSwitchComment;
        }
    }

    StateMachine<TutorialManager> stateMachine;

    public StateMachine<TutorialManager> StateMachine
    {
        get
        {
            return stateMachine;
        }
    }

    public delegate void ReceiveCheckPointMessage(string msg,TutorialManager manager);

    public ReceiveCheckPointMessage RCPMessage;

    void Start()
    {
        stateMachine = new StateMachine<TutorialManager>(this);
        stateMachine.ChangeState(HelloState.Instance);
    }

    public void StartCommentCroutione(string[] comments, float interval,State<TutorialManager> affterState = null)
    {
        StartCoroutine(CommentCoroutine(comments, interval,affterState));
    }

    //helloCommentsが空かどうかの判定はしない
    IEnumerator CommentCoroutine(string[] comments,float interval, State<TutorialManager> affterState)
    {
        float count = 0;
        int commentIndex = 0;

        descriptionDirector.SetDescriptionText(comments[commentIndex]);

        while(true)
        {
            count += Time.deltaTime;

            if(count >= interval)
            {
                count = 0;
                commentIndex++;

                if (commentIndex >= comments.Length)
                    break;

                descriptionDirector.SetDescriptionText(comments[commentIndex]);
            }

            yield return null;
        }

        descriptionDirector.HidePanel();

        if(affterState != null)
            stateMachine.ChangeState(affterState);
    }

    public void StartWaitInputAndActionCoroutine(string buttonName, System.Action action)
    {
        StartCoroutine(WaitInputAndActionCoroutine(buttonName, action));
    }

    IEnumerator WaitInputAndActionCoroutine(string buttonName,System.Action action)
    {
        bool isPushed = false;

        while(true)
        {
            isPushed = Input.GetButtonDown(buttonName) || CrossPlatformInputManager.GetButtonDown(buttonName);

            if(isPushed)
            {
                break;
            }

            yield return null;
        }

        action();
    }

    public void OnCheckPoint(string msg)
    {
        RCPMessage(msg,this);
    }
}

public class HelloState : State<TutorialManager>
{
    private static HelloState m_Instance;
    public static HelloState Instance
    {
        get
        {
            if (m_Instance == null)
                m_Instance = CreateInstance<HelloState>();

            return m_Instance;
        }
    }

    public override void Enter(TutorialManager obj)
    {
        base.Enter(obj);

        obj.StartCommentCroutione(obj.HelloComments, obj.IntervalSwitchComment,MoveState.Instance);
    }

    public override void Execute(TutorialManager obj)
    {
        base.Execute(obj);
    }

    public override void Exit(TutorialManager obj)
    {
        base.Exit(obj);
    }
}

public class MoveState : State<TutorialManager>
{
    private static MoveState m_Instance;
    public static MoveState Instance
    {
        get
        {
            if (m_Instance == null)
                m_Instance = CreateInstance<MoveState>();

            return m_Instance;
        }
    }

    public override void Enter(TutorialManager obj)
    {
        base.Enter(obj);

        obj.RCPMessage += OnCheckPointMessage;

        obj.Player.StateMachine.ChangeState(TutorialMove.Instance);
    }

    public override void Execute(TutorialManager obj)
    {
        base.Execute(obj);
    }

    public override void Exit(TutorialManager obj)
    {
        base.Exit(obj);

        obj.RCPMessage -= OnCheckPointMessage;
    }

    public void OnCheckPointMessage(string msg,TutorialManager manager)
    {
        if(msg == "AppearBlack")
        {
            manager.Player.StateMachine.ChangeState(PlayerPause.Instance);

            manager.DescriptionDirector.ShowPanel();

            string[] comments =
            {
                "黒いブロックをすり抜けた！！",
                "モフが白いとき黒い物体を\nすり抜けることができます"
            };
            manager.StartCommentCroutione(comments, manager.IntervalSwitchComment, MoveState.Instance);
        }
        else if(msg == "AppearWhite")
        {
            manager.DescriptionDirector.ShowPanel();

            string[] comments =
            {
                "白いブロックだと当たる",
                "でも、大丈夫モフは体の色を\n変えることができます",
                "白⇆黒（白黒チェンジ）ボタンを押して\n体の色を変えよう"
            };

            manager.StartCommentCroutione(comments, manager.IntervalSwitchComment,WaitColorChangeState.Instance);
        }
        else if(msg == "NoFloor")
        {
            manager.DescriptionDirector.ShowPanel();

            string[] comments =
            {
                "白いブロックに飛び乗らなければ",
                "でも今のままではすり抜けて落ちてしまう",
                "まず、体の色を変えよう"
            };

            manager.Player.StateMachine.ChangeState(PlayerPause.Instance);
            manager.StartCommentCroutione(comments, manager.IntervalSwitchComment, WaitColorChangeState.Instance);
        }
        else if(msg == "Jump")
        {
            manager.DescriptionDirector.ShowPanel();

            string[] comments =
            {
                "そしてジャンプ！！",
                "ジャンプボタンを押そう"
            };

            manager.Player.StateMachine.ChangeState(PlayerPause.Instance);
            manager.StartCommentCroutione(comments, manager.IntervalSwitchComment, WaitJumpState.Instance);
        }
        else if(msg == "DamageObj")
        {
            manager.DescriptionDirector.ShowPanel();

            string[] comments =
            {
                "赤いものに当たってはいけません",
                "白黒いずれの時も当たってしまったら\nゲームオーバー",
                "ジャンプして避けましょう"
            };

            manager.Player.StateMachine.ChangeState(PlayerPause.Instance);
            manager.StartCommentCroutione(comments, manager.IntervalSwitchComment, WaitJumpState.Instance);
        }
        else if(msg == "Supplement")
        {
            manager.DescriptionDirector.ShowPanel();

            string[] comments =
            {
                "他に白いトゲや黒いトゲがあります",
                "違う色になることで\n避けることができます",
                "あと他の色のものと重なっているときは",
                "白黒チェンジできないので\n気を付けてください"
            };

            manager.Player.StateMachine.ChangeState(PlayerPause.Instance);
            manager.StartCommentCroutione(comments, manager.IntervalSwitchComment,MoveState.Instance);
        }
        else if(msg == "Finish")
        {
            manager.DescriptionDirector.ShowPanel();

            string[] comments =
            {
                "説明は以上になります",
                "あ、言い忘れていましたが\nモフは２段ジャンプすることができます",
                "白黒チェンジとジャンプを駆使して\n全ステージ制覇してください！！",
                "お疲れさまでした(><)"
            };

            manager.Player.StateMachine.ChangeState(PlayerPause.Instance);
            manager.StartCommentCroutione(comments, manager.IntervalSwitchComment,TutorialFinishState.Instance);
        }
    }
}

public class WaitColorChangeState : State<TutorialManager>
{
    private static WaitColorChangeState m_Instance;
    public static WaitColorChangeState Instance
    {
        get
        {
            if (m_Instance == null)
                m_Instance = CreateInstance<WaitColorChangeState>();

            return m_Instance;
        }
    }

    public override void Enter(TutorialManager obj)
    {
        base.Enter(obj);

        obj.DescriptionDirector.ShowChangeColorArrow();

        System.Action action = () =>
        {
            obj.Player.OwnColorChange();
            obj.StateMachine.ChangeState(MoveState.Instance);
        };

        obj.StartWaitInputAndActionCoroutine("Fire1", action);
    }

    public override void Execute(TutorialManager obj)
    {
        base.Execute(obj);
    }

    public override void Exit(TutorialManager obj)
    {
        base.Exit(obj);

        obj.DescriptionDirector.HideChangeColorArrow();
    }
}

public class WaitJumpState : State<TutorialManager>
{
    private static WaitJumpState m_Instance;
    public static WaitJumpState Instance
    {
        get
        {
            if (m_Instance == null)
                m_Instance = CreateInstance<WaitJumpState>();

            return m_Instance;
        }
    }

    public override void Enter(TutorialManager obj)
    {
        base.Enter(obj);

        obj.DescriptionDirector.ShowJumpArrow();

        System.Action action = () =>
        {
            obj.Player.Jump();
            obj.StateMachine.ChangeState(MoveState.Instance);
        };

        obj.StartWaitInputAndActionCoroutine("Jump", action);
    }

    public override void Execute(TutorialManager obj)
    {
        base.Execute(obj);
    }

    public override void Exit(TutorialManager obj)
    {
        base.Exit(obj);

        obj.DescriptionDirector.HideJumpArrow();
    }
}

public class TutorialFinishState : State<TutorialManager>
{
    private static TutorialFinishState m_Instance;
    public static TutorialFinishState Instance
    {
        get
        {
            if (m_Instance == null)
                m_Instance = CreateInstance<TutorialFinishState>();

            return m_Instance;
        }
    }

    public override void Enter(TutorialManager obj)
    {
        base.Enter(obj);

        SoundManager.Instance.StopBGM();

        string title = GameManager.Instance.StageSelectSceneName;
        var fade = FadeManager.Instance;
        fade.Transition(0.5f, title);
    }
}