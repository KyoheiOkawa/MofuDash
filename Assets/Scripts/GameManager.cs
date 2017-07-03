using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ゲームマネージャクラス
/// スコアなどの数値を持ちます
/// 
/// どこからでもアクセスできるように「シングルトン」
/// ゲームオブジェクトに貼り付けなくても動作する「static関数」
/// などを持ちます
/// 
/// ゲームオブジェクトに貼り付けずに動作させるとき、
/// スクリプトのクラスは「MonoBehaviour」の継承はしません
/// </summary>
public class GameManager {

	/// <summary> このクラスの実体 </summary>
	static GameManager m_Instance;
	static public GameManager Instance
	{
		get
		{
			if (m_Instance == null)
			{
				m_Instance = new GameManager();
			}
			return m_Instance;
		}
	}


	/// <summary> 現在のスコア </summary>
	int m_Score;
	/// <summary> 現在のスコア </summary>
	public int score { set { m_Score = value; } get { return m_Score; } }


}
