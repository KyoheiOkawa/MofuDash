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

	[SerializeField]
	Coin[] m_Coin = new Coin[3];

	bool[] m_IsGetCoin = new bool[3];

    StateMachine<MainSceneManager> m_StateMachine;

	int m_Progress = 0;//ステージの進行度（百分率）

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

	public int progress
	{
		get {
			return m_Progress;
		}
		set{
			m_Progress = value;
		}
	}

	// Use this for initialization
	void Start () {
		Application.targetFrameRate = 60;

        m_StateMachine = new StateMachine<MainSceneManager>(this);
        m_StateMachine.ChangeState(StartState.Instance);

		SetCatchedCoin ();
		UpdateCoinChatcedState ();
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

	private void SetCatchedCoin()
	{
		Scene thisScene = SceneManager.GetActiveScene ();
		StageInfo thisStageInfo = GameManager.Instance.stageInfo[thisScene.name];

		for (int i = 0; i < m_IsGetCoin.Length; i++) {
			m_Coin [i].isCatched = thisStageInfo.coin[i];
		}
	}

	public void UpdateCoinChatcedState()
	{
		for (int i = 0; i < m_Coin.Length; i++) {
			m_IsGetCoin [i] = m_Coin [i].isCatched;
		}
	}

	public bool GetCoinState(int id)
	{
		if (0 <= id && id < m_IsGetCoin.Length)
			return m_IsGetCoin [id];

		return false;
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

		Action action = () => {
			Transform canvasTrans = GameObject.Find("Canvas").gameObject.GetComponent<Transform>();
			ClearPanel panel = Instantiate(Resources.Load("Prefabs/ClearPanel"),canvasTrans) as ClearPanel;
		};

		GameManager manager = GameManager.Instance;
		obj.StartCoroutine (manager.WaitAndAction (1.5f, action));

		//ステージ情報のセーブ
		Scene thisScene = SceneManager.GetActiveScene ();
		StageInfo thisStageInfo = manager.stageInfo [thisScene.name];
		thisStageInfo.progress = 100;
		for (int i = 0; i < 3; i++) {
			thisStageInfo.coin [i] = obj.GetCoinState (i);
		}
		manager.ChangeStageInfo (thisScene.name, thisStageInfo);
    }

    public override void Execute(MainSceneManager obj)
    {
        base.Execute(obj);
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