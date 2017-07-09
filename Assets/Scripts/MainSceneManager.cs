using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainSceneManager : MonoBehaviour {
    [SerializeField]
    Player m_Player;

    [SerializeField]
    Canvas m_Canvas;

    StateMachine<MainSceneManager> m_StateMachine;

    public StateMachine<MainSceneManager> stateMachine
    {
        get
        {
            return m_StateMachine;
        }
    }

    public Player player
    {
        get
        {
            return m_Player;
        }
    }

    public Canvas canvas
    {
        get
        {
            return m_Canvas;
        }
    }

	// Use this for initialization
	void Start () {
        m_StateMachine = new StateMachine<MainSceneManager>(this);
        m_StateMachine.ChangeState(StartState.Instance);
    }
	
	// Update is called once per frame
	void Update () {
        m_StateMachine.Update();
    }
}

public class StartState : State<MainSceneManager>
{
    static StartState m_Instance;
    static public StartState Instance
    {
        get
        {
            if (m_Instance == null)
                m_Instance = new StartState();

            return m_Instance;
        }
    }

    Image m_StartImage;

    public override void Enter(MainSceneManager obj)
    {
        base.Enter(obj);

        obj.player.stateMachine.ChangeState(PlayerPouse.Instance);

        m_StartImage = Instantiate(Resources.Load<Image>("Prefabs/Start"), obj.canvas.transform);
    }

    public override void Execute(MainSceneManager obj)
    {
        base.Execute(obj);

        if (m_StartImage.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
            obj.stateMachine.ChangeState(PlayingState.Instance);
    }

    public override void Exit(MainSceneManager obj)
    {
        base.Exit(obj);

        Destroy(m_StartImage.gameObject);
    }
}

public class PlayingState : State<MainSceneManager>
{
    static PlayingState m_Instance;
    static public PlayingState Instance
    {
        get
        {
            if (m_Instance == null)
                m_Instance = new PlayingState();

            return m_Instance;
        }
    }

    public override void Enter(MainSceneManager obj)
    {
        base.Enter(obj);

        obj.player.stateMachine.ChangeState(PlayerDefault.Instance);
    }

    public override void Execute(MainSceneManager obj)
    {
        base.Execute(obj);
    }

    public override void Exit(MainSceneManager obj)
    {
        base.Exit(obj);
    }
}