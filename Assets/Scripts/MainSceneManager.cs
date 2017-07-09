using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainSceneManager : MonoBehaviour {
    [SerializeField]
    Player m_Player;

    [SerializeField]
    GameObject m_Canvas;

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

    public GameObject canvas
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

    void Reset()
    {
        if (!m_Player)
            m_Player = GameObject.FindGameObjectWithTag("Player").gameObject.GetComponent<Player>();

        if (!m_Canvas)
            m_Canvas = GameObject.Find("Canvas").gameObject;
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
                m_Instance = CreateInstance<StartState>();

            return m_Instance;
        }
    }

    Image m_StartImage;

    public override void Enter(MainSceneManager obj)
    {
        base.Enter(obj);

        obj.player.stateMachine.ChangeState(PlayerPause.Instance);

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

public class ClearState : State<MainSceneManager>
{
    static ClearState m_Instance;
    static public ClearState Instance
    {
        get
        {
            if (m_Instance == null)
                m_Instance = CreateInstance<ClearState>();

            return m_Instance;
        }
    }

    Image m_ClearImage;

    public override void Enter(MainSceneManager obj)
    {
        base.Enter(obj);

        obj.player.stateMachine.ChangeState(PlayerPause.Instance);

        m_ClearImage = Instantiate(Resources.Load<Image>("Prefabs/Clear"), obj.canvas.transform);
    }

    public override void Execute(MainSceneManager obj)
    {
        base.Execute(obj);

        if (m_ClearImage.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
        {

            var scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }
    }

    public override void Exit(MainSceneManager obj)
    {
        base.Exit(obj);

        Destroy(m_ClearImage.gameObject);
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
                m_Instance = CreateInstance<PlayingState>();

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