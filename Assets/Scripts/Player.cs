using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.UI;

/// <summary>
/// プレイヤー。ゲームの操作キャラクター。
/// 左右で横に移動、ジャンプキーでジャンプ、
/// 色変更ボタンで状態が変わる。
/// 
/// 床との接触判定やゴールとの接触判定はすべてこのプレイヤーが行う。
/// 床やゴールは基本的に他のオブジェクトとの接触判定が必要ないため。
/// </summary>
public class Player : MonoBehaviour
{

	/// <summary> プレイヤーの移動速度 </summary>
	[SerializeField]
	float m_MoveSpeed = 4.0f;
	/// <summary> プレイヤーのジャンプ力 </summary>
	[SerializeField]
	float m_JumpPower = 6.0f;

	/// <summary> 現在のプレイヤーの色 </summary>
	OwnColor m_OwnColor = OwnColor.WHITE;

	/// <summary>ジャンプ押されたかどうか </summary>
	bool m_Jump = false;
	/// <summary>色変更ボタンが押されたかどうか</summary>
	bool m_ChangeColor = false;

	/// <summary>現在何段階目のジャンプかをカウントする</summary>
	int m_JumpCount = 0;

	/// <summary>何段ジャンプまでできるか</summary>
	[SerializeField]
	int m_JumpStep = 2;

	/// <summary>次の固定アップデートでジャンプするかどうか</summary>
	bool m_IsNextJump = false;

	/// <summary> キャラクターの描画コンポーネント </summary>
	[SerializeField, HideInInspector]
	SpriteRenderer m_Renderer;
	/// <summary> 自分のトランスフォームのキャッシュ </summary>
	[SerializeField, HideInInspector]
	Transform m_Trans;
	/// <summary> 自分のリジッドボディのキャッシュ </summary>
	[SerializeField, HideInInspector]
	Rigidbody2D m_Rigid;
	/// <summary> 自分のアニメーション管理コンポーネントのキャッシュ </summary>
	[SerializeField, HideInInspector]
	Animator m_Animator;
	/// <summary> 自分の描画マテリアル </summary>
	[SerializeField, HideInInspector]
	Material m_Mat;

    [SerializeField]
    float m_DeadJumpPower = 250.0f;

    [SerializeField]
    float m_DeadUnderPosY = -8.0f;//この値より下に落ちたらゲームオーバー

	[SerializeField]
	MainSceneManager m_SceneManager;

    StateMachine<Player> m_StateMachine;

    public StateMachine<Player> stateMachine
    {
        get
        {
            return m_StateMachine;
        }
    }

	public MainSceneManager sceneManager
	{
		get{
			return m_SceneManager;
		}
	}

    private void Awake()
    {
        m_StateMachine = new StateMachine<Player>(this);
        m_StateMachine.ChangeState(PlayerPause.Instance);
    }

    // 初期化処理
    void Start()
	{
		// 現在の自分の色を黒→白にする
		m_OwnColor = OwnColor.BLACK;
		OwnColorChange();

		if (!m_SceneManager)
			m_SceneManager = GameObject.FindObjectOfType<MainSceneManager> ();
    }

	// 毎フレームの更新処理
	void Update()
	{
		//入力情報の取得
		m_Jump = Input.GetButtonDown("Jump") || CrossPlatformInputManager.GetButtonDown("Jump");
		m_ChangeColor = Input.GetButtonDown("Fire1") || CrossPlatformInputManager.GetButtonDown("Fire1");

        m_StateMachine.Update();
	}

	void FixedUpdate()
	{
		m_StateMachine.FixedUpdate ();
	}

	//=================================================================================================
	// Unity（class MonoBehaviour）が呼び出す関数群
	//=================================================================================================
	/// <summary>
	/// 何かに当たった瞬間に呼ばれる関数
	/// </summary>
	/// <param name="col"> 当たった相手 </param>
	void OnCollisionEnter2D(Collision2D other)
	{
		m_JumpCount = 0;

        //ブロックに横からぶつかった場合ゲームオーバー
        if(other.gameObject.CompareTag("Block"))
        {
            var ownPos = m_Trans.position;
            Vector2 contactPos;
            foreach(var contact in other.contacts)
            {
                contactPos = contact.point;
                //自分からコンタクトポイントの方向
                Vector2 dir = contactPos - (Vector2)ownPos;

                //右ベクトルと自分からのコンタクトポイントの方向の角度が30度以下であったらゲームオーバー
                if(Vector2.Angle(Vector2.right,dir) < 30.0f)
                {
                    m_StateMachine.ChangeState(PlayerDead.Instance);
                }
            }
        }
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("DamageObj"))
        {
            m_StateMachine.ChangeState(PlayerDead.Instance);
        }
    }

    /// <summary>
    /// リセット関数　このスクリプトをオブジェクトにアタッチしたときとかに自動的に呼ばれる
    /// </summary>
    void Reset()
	{
		// 描画コンポーネントを取得
		if (!m_Renderer)
			m_Renderer = GetComponent<SpriteRenderer>();
		// トランスフォームを取得
		if (!m_Trans)
			m_Trans = GetComponent<Transform>();
		// リジッドボディを取得
		if (!m_Rigid)
			m_Rigid = GetComponent<Rigidbody2D>();
		// アニメーション管理コンポーネントを取得
		if (!m_Animator)
			m_Animator = GetComponent<Animator>();
		// 描画マテリアルを取得
		if (!m_Mat)
			m_Mat = m_Renderer.sharedMaterial;
	}

    //=================================================================================================
    // 自作関数
    //=================================================================================================

	/// <summary>
	/// デフォルトの行動（Updateで使用）
	/// </summary>
    public void DefaultBehaviour()
    {
        // 色の変更
		if (m_ChangeColor && !CheckFilledWithOtherColor())
        {
            OwnColorChange();
        }

		if (m_Jump) {
			m_IsNextJump = true;
		}
    }

	/// <summary>
	/// デフォルトの行動（FixedUpdateで使用）
	/// </summary>
	public void FixedDefaultBehaviour()
	{
		Move (new Vector2 (1.0f, 0));
		if (m_IsNextJump)
			Jump ();
	}

    /// <summary>
    /// 落下したか調べる
    /// </summary>
    /// <returns>true 落下した場合</returns>
    public bool IsFall()
    {
        if (m_Trans.position.y < m_DeadUnderPosY)
            return true;

        return false;
    }

    public void StopMove()
    {
        m_Rigid.velocity = Vector2.zero;
    }

    public void DeadJump()
    {
        m_Rigid.AddForce(new Vector2(0, m_DeadJumpPower));
    }

    //----------------
    // private（非公開関数。自分のクラスだけで使う関数）
    //----------------

    /// <summary>
    /// 渡されたベクトルに従い移動する処理
    /// </summary>
    void Move(Vector2 InputVec)
	{
		// 渡されたベクトルを、移動するベクトルに変換する
		Vector2 moveVec = InputVec * m_MoveSpeed;

		Vector2 nowVelocity = m_Rigid.velocity;

		moveVec.y = nowVelocity.y;

		m_Rigid.velocity = moveVec;

		// 入力されたキーの移動速度情報を、アニメーション管理コンポーネントに渡す（横方向の移動速度）
		m_Animator.SetFloat("AbsXSpeed", Mathf.Abs(moveVec.x));
	}

	/// <summary>
	/// ジャンプする関数
	/// </summary>
	void Jump()
	{
		// 現在ジャンプ中なら、処理を行わない
		if (m_JumpCount >= m_JumpStep)
		{
			return;
		}

		// アニメーション管理コンポーネントのパラメータを更新する
		m_Animator.SetTrigger("Jump");

		Vector2 newVelocity = m_Rigid.velocity;
		newVelocity.y = m_JumpPower;
		if (m_JumpCount >= 1) {
			newVelocity.y += m_JumpPower * 0.2f;
		}
		m_Rigid.velocity = newVelocity;

		m_JumpCount++;

		m_IsNextJump = false;
	}

	/// <summary>
	/// 現在のプレイヤーの色を変える関数　白→黒→白...　と変わる
	/// </summary>
	void OwnColorChange()
	{
		// 変更後の色（列挙型）
		OwnColor ownColor = OwnColor.WHITE;
		// 変更後のレイヤー
		int layer = -1;
		// 現在のプレイヤーの色によって、変更後の色とレイヤーを確定する
		// 現在の色が白の時は黒になる、黒の時は白になる
		switch (m_OwnColor)
		{
			case OwnColor.WHITE:
				ownColor = OwnColor.BLACK;
				layer = LayerMask.NameToLayer("Black");
				m_Renderer.sharedMaterial.SetColor("_MinusColor", Color.white);
				break;

			case OwnColor.BLACK:
				ownColor = OwnColor.WHITE;
				layer = LayerMask.NameToLayer("White");
				m_Renderer.sharedMaterial.SetColor("_MinusColor", Color.black);
				break;

			default:
				Debug.LogError("[Player.cs] プレイヤーの色変更処理が不正です");
				return;
		}
		// 変更後のパラメータに合わせる
		// 色
		m_OwnColor = ownColor;
		// レイヤー
		gameObject.layer = layer;

	}

    /// <summary>
    /// 他の色のコリジョンレイヤーに埋まっているかしているか判定する
    /// </summary>
    /// <returns>他の色のコリジョンに埋まっていたらtrue</returns>
    bool CheckFilledWithOtherColor()
    {
        var ownCol = GetComponent<CircleCollider2D>();

        int layerMask = 0;

        switch (m_OwnColor)
        {
            case OwnColor.BLACK:
                layerMask = LayerMask.GetMask(new string[] { "White" });
                break;
            case OwnColor.WHITE:
                layerMask = LayerMask.GetMask(new string[] { "Black" });
                break;
        }


        var hitCol = Physics2D.OverlapBox(m_Trans.position, new Vector2(ownCol.radius*2.0f,ownCol.radius*2.0f), 0,layerMask);

        if(hitCol)
        {
            var hitObject = hitCol.gameObject;

            var acceptLen = ownCol.radius * 2.0f  * 0.75f;

            var len = (hitObject.GetComponent<Transform>().position - m_Trans.position).magnitude;

            if(len < acceptLen)
            {
                return true;
            }
        }

        return false;
    }
}

public class PlayerPause : State<Player>
{
    private static PlayerPause m_Instance;
    public static PlayerPause Instance
    {
        get
        {
            if (m_Instance == null)
                m_Instance = CreateInstance<PlayerPause>() ;

            return m_Instance;
        }
    }

    public override void Enter(Player obj)
    {
        base.Enter(obj);
    }

    public override void Execute(Player obj)
    {
        base.Execute(obj);
    }

    public override void Exit(Player obj)
    {
        base.Exit(obj);
    }
}

public class PlayerDefault : State<Player>
{
    private static PlayerDefault m_Instance;
    public static PlayerDefault Instance
    {
        get
        {
            if (m_Instance == null)
                m_Instance = CreateInstance<PlayerDefault>();

            return m_Instance;
        }
    }

    public override void Enter(Player obj)
    {
        base.Enter(obj);
    }

    public override void Execute(Player obj)
    {
        base.Execute(obj);

        obj.DefaultBehaviour();

        if (obj.IsFall())
            obj.stateMachine.ChangeState(PlayerDead.Instance);
    }

	public override void FixedExecute (Player obj)
	{
		base.FixedExecute (obj);

		obj.FixedDefaultBehaviour ();
	}

    public override void Exit(Player obj)
    {
        base.Exit(obj);
    }
}

public class PlayerDead : State<Player>
{
    private static PlayerDead m_Instance;
    public static PlayerDead Instance
    {
        get
        {
            if (m_Instance == null)
                m_Instance = CreateInstance<PlayerDead>();

            return m_Instance;
        }
    }

    public override void Enter(Player obj)
    {
        base.Enter(obj);

        obj.GetComponent<Animator>().speed = 0;//アニメーションをストップ
        obj.gameObject.layer = LayerMask.NameToLayer("IgnoreCollision");

        obj.StopMove();
        obj.DeadJump();

		Action action = () =>
		{
			var canvasTrans = GameObject.Find("Canvas").gameObject.GetComponent<Transform>();
			FailedPanel panel = Instantiate(Resources.Load("Prefabs/FailedPanel"),canvasTrans) as FailedPanel;
		};
		obj.StartCoroutine(GameManager.Instance.WaitAndAction(1.5f, action));

		//ステージ情報のセーブ
		//進行状況が前の結果よりよかったらセーブする
		GameManager manager = GameManager.Instance;
		int nowProgress = obj.sceneManager.progress;
		Scene thisScene = SceneManager.GetActiveScene ();
		StageInfo thisStageInfo = manager.stageInfo [thisScene.name];
		if (thisStageInfo.progress < nowProgress) {
			thisStageInfo.progress = nowProgress;
			manager.ChangeStageInfo (thisScene.name, thisStageInfo);
		}
    }

    public override void Execute(Player obj)
    {
        base.Execute(obj);
    }

    public override void Exit(Player obj)
    {
        base.Exit(obj);
    }
}
