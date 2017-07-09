using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
	float m_MoveSpeed = 3.0f;
	/// <summary> プレイヤーのジャンプ力 </summary>
	[SerializeField]
	float m_JumpPower = 10.0f;

	/// <summary> 現在のプレイヤーの色 </summary>
	OwnColor m_OwnColor = OwnColor.WHITE;

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
    float m_DeadUnderPosY = -5.0f;//この値より下に落ちたらゲームオーバー

    StateMachine<Player> m_StateMachine;

	/// <summary> 現在ジャンプ中かどうかのフラグ </summary>
	bool m_IsJumping = false;

    public StateMachine<Player> stateMachine
    {
        get
        {
            return m_StateMachine;
        }
    }

    private void Awake()
    {
        m_StateMachine = new StateMachine<Player>(this);
        m_StateMachine.ChangeState(PlayerPouse.Instance);
    }

    // 初期化処理
    void Start()
	{
		// 現在の自分の色を黒→白にする
		m_OwnColor = OwnColor.BLACK;
		OwnColorChange();

		// テスト：ゲームマネージャのスコアを０に戻す
		GameManager.Instance.score = 0;
    }

	// 毎フレームの更新処理
	void Update()
	{
        m_StateMachine.Update();
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
		// 何かに当たったら、ジャンプしているフラグを偽にする
		m_IsJumping = false;

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

    public void DefaultBehaviour()
    {
        // キーの入力を受け取る変数を作成
        bool jump = false;
        bool colorChange = false;
        // ジャンプキーの入力
        jump = Input.GetButton("Jump");
        // 色を変更するキー
        colorChange = Input.GetButtonDown("Fire1");

        // キーの入力によって、キャラクターを動かす
        // 左右の入力
        Move(new Vector2(1.0f, 0));
        // ジャンプキー
        if (jump)
        {
            Jump();
        }
        // 色の変更
        if (colorChange && !CheckFilledWithOtherColor())
        {
            OwnColorChange();
        }
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
		Vector2 MoveVec = InputVec * m_MoveSpeed;

		// 自分のリジッドボディの移動速度を、渡された移動速度に近づける
		// １．現在の移動速度を取得
		Vector2 moveVel = m_Rigid.velocity;
		// ２．渡された移動速度のうち、Ｙ軸の速度は現在の速度に合わせる（自由落下を適用するため）
		MoveVec.y = moveVel.y;
		// ３．現在の移動速度を渡された移動速度に近づけていく
		m_Rigid.velocity = Vector2.Lerp(m_Rigid.velocity, MoveVec, 0.5f);

		// 入力されたキーの移動速度情報を、アニメーション管理コンポーネントに渡す（横方向の移動速度）
		m_Animator.SetFloat("AbsXSpeed", Mathf.Abs(MoveVec.x));

		// 移動ベクトルが遅ければ待機する
		if (InputVec.sqrMagnitude < 0.1f * 0.1f)
		{
			// 待機（なにもしない）
			return;
		}

		// 現在の移動ベクトルによって、左右の向きを決める（基本は左向き）
		// 移動ベクトルが左方向なら、左を向く
		if (m_Rigid.velocity.x < 0.0f)
			m_Renderer.flipX = false;
		// 移動ベクトルが右向きなら、右を向く
		else
			m_Renderer.flipX = true;
	}

	/// <summary>
	/// ジャンプする関数
	/// </summary>
	void Jump()
	{
		// 現在ジャンプ中なら、処理を行わない
		if (m_IsJumping)
		{
			return;
		}

		// ここまで来るとジャンプ可能なので、ジャンプ処理を行う
		// ジャンプ中のフラグを真にする
		m_IsJumping = true;

		// アニメーション管理コンポーネントのパラメータを更新する
		m_Animator.SetTrigger("Jump");

		// 現在のリジッドボディの移動ベクトルを取得
		Vector2 moveVel = m_Rigid.velocity;
		// 移動ベクトルのＹ軸速度をジャンプする力に変換
		moveVel.y = m_JumpPower;

		// 現在のリジッドボディの移動ベクトルを更新
		m_Rigid.velocity = moveVel;


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

public class PlayerPouse : State<Player>
{
    private static PlayerPouse m_Instance;
    public static PlayerPouse Instance
    {
        get
        {
            if (m_Instance == null)
                m_Instance = CreateInstance<PlayerPouse>();

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
    }

    public override void Execute(Player obj)
    {
        base.Execute(obj);

        Action action = () =>
        {
            var scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        };
        obj.StartCoroutine(GameManager.Instance.WaitAndAction(1.5f, action));
    }

    public override void Exit(Player obj)
    {
        base.Exit(obj);
    }
}
