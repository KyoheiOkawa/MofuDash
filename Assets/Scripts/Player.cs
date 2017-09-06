using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
	[SerializeField]
	float moveSpeed = 5.0f;

	[SerializeField]
	float jumpPower = 7.0f;

	OwnColor ownColor = OwnColor.WHITE;

	bool isJumpButtonPressed = false;

	bool isColorChangeButtonPressed = false;

	/// <summary>現在何段階目のジャンプかをカウントする</summary>
	int jumpCount = 0;

	/// <summary>何段ジャンプまでできるか</summary>
	[SerializeField]
	int maxJumpSteps = 2;

	/// <summary>次の固定アップデートでジャンプするかどうか</summary>
	bool isJumpNextFixedFrame = false;

	[SerializeField, HideInInspector]
	SpriteRenderer spriteRender;

	[SerializeField, HideInInspector]
	Rigidbody2D rigid2D;
	/// <summary> 自分のアニメーション管理コンポーネントのキャッシュ </summary>
	[SerializeField, HideInInspector]
	Animator animator;
	/// <summary> 自分の描画マテリアル </summary>
	[SerializeField, HideInInspector]
	Material material;

    /// <summary>　死んだときに飛ぶ力</summary>
    [SerializeField]
    float jumpPowerWhenDead = 250.0f;

    //この値より下に落ちたらゲームオーバー
    [SerializeField]
    float deadUnderPosY = -8.0f;

	[SerializeField]
	MainSceneManager sceneManager;

    StateMachine<Player> stateMachine;

    public StateMachine<Player> StateMachine
    {
        get
        {
            return stateMachine;
        }
    }

	public MainSceneManager SceneManager
	{
		get{
			return sceneManager;
		}
	}

    private void Awake()
    {
        stateMachine = new StateMachine<Player>(this);
        stateMachine.ChangeState(PlayerPause.Instance);
    }

    void Start()
    {
        spriteRender = GetComponent<SpriteRenderer>();
        rigid2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        material = spriteRender.sharedMaterial;

        ownColor = OwnColor.BLACK;
        OwnColorChange(false);

        if (!sceneManager)
            sceneManager = GameObject.FindObjectOfType<MainSceneManager>();
    }

	void Update()
	{
		isJumpButtonPressed = Input.GetButtonDown("Jump") || CrossPlatformInputManager.GetButtonDown("Jump");
		isColorChangeButtonPressed = Input.GetButtonDown("Fire1") || CrossPlatformInputManager.GetButtonDown("Fire1");

        if (jumpCount >= maxJumpSteps)
            isJumpButtonPressed = false;

        stateMachine.Update();
	}

	void FixedUpdate()
	{
		stateMachine.FixedUpdate ();
	}

	void OnCollisionEnter2D(Collision2D other)
	{
        if (stateMachine.CurrentState != PlayerDefault.Instance)
            return;

        //ブロックに横からぶつかった場合ゲームオーバー
        if(other.gameObject.CompareTag("Block"))
        {
            var ownPos = transform.position;
            Vector2 contactPos;
            foreach(var contact in other.contacts)
            {
                contactPos = contact.point;
                //自分からコンタクトポイントの方向
                Vector2 dir = contactPos - (Vector2)ownPos;

				//足元での衝突でった場合、ジャンプのカウント数をリセット
				if(Vector2.Angle(Vector2.down,dir) < 30.0f)
					jumpCount = 0;

                //右ベクトルと自分からのコンタクトポイントの方向の角度が30度以下であったらゲームオーバー
                if(Vector2.Angle(Vector2.right,dir) < 30.0f)
                {
					//エフェクトを追加
					Instantiate (Resources.Load ("Prefabs/DeadEffect")as GameObject,
						contactPos, Quaternion.identity);
					
                    stateMachine.ChangeState(PlayerDead.Instance);
                }
            }
        }
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (stateMachine.CurrentState != PlayerDefault.Instance)
            return;

        if (collision.CompareTag("DamageObj"))
        {
			//死んだときのエフェクトを生成
			Instantiate (Resources.Load ("Prefabs/DeadEffect")as GameObject,
				GetComponent<Transform>().position, Quaternion.identity);

            stateMachine.ChangeState(PlayerDead.Instance);
        }
    }

	/// <summary>
    /// デフォルトの行動(Updateで使用）
	/// ユーザーからの入力によって色変更する行動
	/// </summary>
    public void DefaultBehaviour()
    {
        // 色の変更
		if (isColorChangeButtonPressed && !CheckFilledWithOtherColor())
        {
            OwnColorChange();
        }

		if (isJumpButtonPressed)
        {
			isJumpNextFixedFrame = true;
		}
    }

	/// <summary>
	/// デフォルトの行動（FixedUpdateで使用）
    /// 横移動、ジャンプ
	/// </summary>
	public void FixedDefaultBehaviour()
	{
		Move (new Vector2 (1.0f, 0));
		if (isJumpNextFixedFrame)
			Jump ();
	}

    /// <summary>
    /// 落下したか調べる
    /// </summary>
    /// <returns>true 落下した場合</returns>
    public bool IsFall()
    {
        if (transform.position.y < deadUnderPosY)
            return true;

        return false;
    }

    public void StopMove()
    {
        rigid2D.velocity = Vector2.zero;
    }

    public void DeadJump()
    {
        rigid2D.AddForce(new Vector2(0, jumpPowerWhenDead));
    }

    public void Move(Vector2 InputVec)
	{
		// 渡されたベクトルを、移動するベクトルに変換する
		Vector2 moveVec = InputVec * moveSpeed;

		Vector2 nowVelocity = rigid2D.velocity;

		moveVec.y = nowVelocity.y;

		rigid2D.velocity = moveVec;

		// 入力されたキーの移動速度情報を、アニメーション管理コンポーネントに渡す（横方向の移動速度）
		animator.SetFloat("AbsXSpeed", Mathf.Abs(moveVec.x));
	}

	public void Jump()
	{
		// 現在ジャンプ中なら、処理を行わない
		if (jumpCount >= maxJumpSteps)
		{
			return;
		}

		SoundManager sound = SoundManager.Instance;
		sound.PlaySE ("Jump");

		// アニメーション管理コンポーネントのパラメータを更新する
		animator.SetTrigger("Jump");

		Vector2 newVelocity = rigid2D.velocity;
		newVelocity.y = jumpPower;
		if (jumpCount >= 1) {
			newVelocity.y += jumpPower * 0.2f;
		}
		rigid2D.velocity = newVelocity;

		jumpCount++;

		isJumpNextFixedFrame = false;
	}

    /// <summary>
    /// 現在のプレイヤーの色を変える関数　白→黒→白...　と変わる
    /// </summary>
    /// <returns>
    /// 変更後の色
    /// </returns>
    public OwnColor OwnColorChange(bool playSe = true)
	{
		OwnColor ownColor = OwnColor.WHITE;
		// 変更後のレイヤー
		int layer = -1;
		// 現在のプレイヤーの色によって、変更後の色とレイヤーを確定する
		// 現在の色が白の時は黒になる、黒の時は白になる
		switch (this.ownColor)
		{
			case OwnColor.WHITE:
				ownColor = OwnColor.BLACK;
				layer = LayerMask.NameToLayer("Black");
                spriteRender.sharedMaterial.SetColor("_MinusColor", Color.white);
				break;

			case OwnColor.BLACK:
				ownColor = OwnColor.WHITE;
				layer = LayerMask.NameToLayer("White");
                spriteRender.sharedMaterial.SetColor("_MinusColor", Color.black);
				break;

			default:
                Debug.LogError("[Player.cs] プレイヤーの色変更処理が不正です");
				return ownColor;
		}

		this.ownColor = ownColor;

		gameObject.layer = layer;

        if (playSe)
        {
            SoundManager sound = SoundManager.Instance;
            sound.PlaySE("Change");
        }

        return ownColor;
	}

    /// <summary>
    /// 他の色のコリジョンレイヤーに埋まっているかしているか判定する
    /// </summary>
    /// <returns>他の色のコリジョンに埋まっていたらtrue</returns>
    bool CheckFilledWithOtherColor()
    {
        var ownCol = GetComponent<CircleCollider2D>();

        int layerMask = 0;

        switch (ownColor)
        {
            case OwnColor.BLACK:
                layerMask = LayerMask.GetMask(new string[] { "White" });
                break;
            case OwnColor.WHITE:
                layerMask = LayerMask.GetMask(new string[] { "Black" });
                break;
        }


        var hitCol = Physics2D.OverlapBox(transform.position, new Vector2(ownCol.radius*2.0f,ownCol.radius*2.0f), 0,layerMask);

        if(hitCol)
        {
            var hitObject = hitCol.gameObject;

            var acceptLen = ownCol.radius * 2.0f  * 0.75f;

            var len = (hitObject.GetComponent<Transform>().position - transform.position).magnitude;

            if(len < acceptLen)
            {
                //オブジェクトに埋まっているときに色変更しようとしたら×エフェクトを生成
                Instantiate(Resources.Load("Prefabs/Batsu"), transform.position, Quaternion.identity);

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

		obj.GetComponent<Rigidbody2D> ().velocity = Vector2.zero;
		obj.GetComponent<Animator>().SetFloat("AbsXSpeed", 0.0f);
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
            obj.StateMachine.ChangeState(PlayerDead.Instance);
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
		int nowProgress = obj.SceneManager.Progress;
		Scene thisScene = SceneManager.GetActiveScene ();
		StageInfo thisStageInfo = manager.StageInfo [thisScene.name];
		if (thisStageInfo.progress < nowProgress) {
			thisStageInfo.progress = nowProgress;
			manager.ChangeStageInfo (thisScene.name, thisStageInfo);
		}

		SoundManager sound = SoundManager.Instance;
		sound.StopBGM ();
		sound.PlayJingle ("GameOver");
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

public class TutorialMove : State<Player>
{
    private static TutorialMove m_Instance;
    public static TutorialMove Instance
    {
        get
        {
            if (m_Instance == null)
                m_Instance = CreateInstance<TutorialMove>();

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

    public override void FixedExecute(Player obj)
    {
        base.FixedExecute(obj);

        obj.Move(new Vector2(1.0f, 0));
    }

    public override void Exit(Player obj)
    {
        base.Exit(obj);
    }
}