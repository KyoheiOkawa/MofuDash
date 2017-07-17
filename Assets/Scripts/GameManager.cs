using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageInfo
{
	public string stageName;
	public int progress;
	public bool[] coin = new bool[3];
	public int unlockCoin;
}

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
public class GameManager
{

	/// <summary> このクラスの実体 </summary>
	static GameManager m_Instance;

	static public GameManager Instance {
		get {
			if (m_Instance == null) {
				m_Instance = new GameManager ();
			}
			return m_Instance;
		}
	}

	Dictionary<string,StageInfo> m_StageInfo = new Dictionary<string,StageInfo> ();

	public Dictionary<string,StageInfo> stageInfo {
		get { 
			ReadStageInfoFromCSV ();
			return m_StageInfo; 
		} 
	}

	public bool ChangeStageInfo(string stageName,StageInfo writeStageInfo)
	{
		ReadStageInfoFromCSV ();

		writeStageInfo.stageName = stageName;
		if(m_StageInfo.ContainsKey(stageName)){
			m_StageInfo [stageName] = writeStageInfo;

			WriteStageInfoFromCSV ();

			return true;
		}

		return false;
	}

	private void ReadStageInfoFromCSV ()
	{
		m_StageInfo.Clear ();

		try {
			string filePath = Application.streamingAssetsPath + "/CSV/StageInfo.csv";
#if UNITY_IOS
			filePath = Application.persistentDataPath + "/" + "StageInfo.csv";
#endif

			var sr = new System.IO.StreamReader (filePath);

			while (!sr.EndOfStream) {
				var line = sr.ReadLine ();

				var values = line.Split (',');

				StageInfo stageInfo = new StageInfo ();
				stageInfo.stageName = values [0].ToString ();
				stageInfo.progress = int.Parse (values [1].ToString ());

				for (int i = 0; i < stageInfo.coin.Length; i++) {
					if (values [2 + i].ToString () == "f") {
						stageInfo.coin [i] = false;
					} else if (values [2 + i].ToString () == "t") {
						stageInfo.coin [i] = true;
					}
				}

				stageInfo.unlockCoin = int.Parse (values [5].ToString ());

				m_StageInfo.Add (values [0].ToString (), stageInfo);
			}

			sr.Close();
		} catch (System.Exception e) {
			Debug.Log (e.Message);
		}
	}

	private void WriteStageInfoFromCSV()
	{
		try{
			string filePath = Application.streamingAssetsPath + "/CSV/StageInfo.csv";
#if UNITY_IOS
			filePath = Application.persistentDataPath + "/" + "StageInfo.csv";
#endif

			var sw = new System.IO.StreamWriter(filePath,false);

			foreach(var info in m_StageInfo){
				sw.WriteLine(
					info.Value.stageName+","+
					info.Value.progress.ToString()+","+
					ConvertBoolToStringtf(info.Value.coin[0])+","+
					ConvertBoolToStringtf(info.Value.coin[1])+","+
					ConvertBoolToStringtf(info.Value.coin[2])+","+
					info.Value.unlockCoin.ToString()
				);
			}

			sw.Close();
		}
		catch(System.Exception e){
			Debug.Log (e.Message);
		}
	}

	private string ConvertBoolToStringtf(bool b)
	{
		if (b)
			return "t";
		else
			return "f";
	}

	public IEnumerator WaitAndAction (float waitTime, Action action)
	{
		yield return new WaitForSeconds (waitTime);

		action ();
	}
}
