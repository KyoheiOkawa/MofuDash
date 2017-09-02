using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainSceneManager : MonoBehaviour
{
    [SerializeField]
    Player player;

    [SerializeField]
    GameObject canvas;

	[SerializeField]
	Coin[] coin = new Coin[3];

	bool[] hasCoin = new bool[3];

    StateMachine<MainSceneManager> stateMachine;

    //ステージの進行度（百分率）
    int progress = 0;

    public StateMachine<MainSceneManager> StateMachine
    {
        get
        {
            return stateMachine;
        }
    }

    public Player Player
    {
        get
        {
            return player;
        }
    }

    public GameObject Canvas
    {
        get
        {
            return canvas;
        }
    }

	public int Progress
	{
		get {
			return progress;
		}
		set{
			progress = value;
		}
	}

	void Start ()
    {
        if (!player)
            player = GameObject.FindGameObjectWithTag("Player").gameObject.GetComponent<Player>();
        if (!canvas)
            canvas = GameObject.Find("Canvas").gameObject;

        Application.targetFrameRate = 60;

        stateMachine = new StateMachine<MainSceneManager>(this);
        stateMachine.ChangeState(StartState.Instance);

		SetCatchedCoin ();
		UpdateCoinChatcedState ();
    }
	
	void Update ()
    {
        stateMachine.Update();
    }

	private void SetCatchedCoin()
	{
		Scene thisScene = SceneManager.GetActiveScene ();
		StageInfo thisStageInfo = GameManager.Instance.StageInfo[thisScene.name];

		for (int i = 0; i < hasCoin.Length; i++)
        {
			coin [i].IsCatched = thisStageInfo.coin[i];
		}
	}

	public void UpdateCoinChatcedState()
	{
		for (int i = 0; i < coin.Length; i++)
        {
			hasCoin [i] = coin [i].IsCatched;
		}
	}

	public bool GetCoinState(int id)
	{
		if (0 <= id && id < hasCoin.Length)
			return hasCoin [id];

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

        obj.Player.StateMachine.ChangeState(PlayerPause.Instance);

        m_StartImage = Instantiate(Resources.Load<Image>("Prefabs/Start"), obj.Canvas.transform);
    }

    public override void Execute(MainSceneManager obj)
    {
        base.Execute(obj);

        if (m_StartImage.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
            obj.StateMachine.ChangeState(PlayingState.Instance);
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

        obj.Player.StateMachine.ChangeState(PlayerPause.Instance);

        m_ClearImage = Instantiate(Resources.Load<Image>("Prefabs/Clear"), obj.Canvas.transform);

		Action action = () => 
        {
			Transform canvasTrans = GameObject.Find("Canvas").gameObject.GetComponent<Transform>();
			ClearPanel panel = Instantiate(Resources.Load("Prefabs/ClearPanel"),canvasTrans) as ClearPanel;
		};

		GameManager manager = GameManager.Instance;
		obj.StartCoroutine (manager.WaitAndAction (1.5f, action));

		//ステージ情報のセーブ
		Scene thisScene = SceneManager.GetActiveScene ();
		StageInfo thisStageInfo = manager.StageInfo [thisScene.name];
		thisStageInfo.progress = 100;
		for (int i = 0; i < 3; i++)
        {
			thisStageInfo.coin [i] = obj.GetCoinState (i);
		}
		manager.ChangeStageInfo (thisScene.name, thisStageInfo);

		SoundManager sound = SoundManager.Instance;
		sound.StopBGM ();
		sound.PlayJingle ("GameClear");
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

        obj.Player.StateMachine.ChangeState(PlayerDefault.Instance);
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